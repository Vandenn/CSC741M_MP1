using ColorMine.ColorSpaces;
using CSC741M_MP1.Algorithms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1.Algorithms
{
    public class HistogramCoherence : Algorithm
    {
        private class CoherencePair
        {
            public double coherent { get; set; }
            public double nonCoherent { get; set; }
            public CoherencePair(double coherent, double nonCoherent)
            {
                this.coherent = coherent;
                this.nonCoherent = nonCoherent;
            }
        }

        private const double SIGNIFICANT_QUERY_THRESHOLD = 0.05;
        private const double SIMILARITY_THRESHOLD = 0.0;

        public override AlgorithmEnum getAlgorithmEnum()
        {
            return AlgorithmEnum.HistogramRefinementColorCoherence;
        }

        public override List<string> generateResults(String queryPath)
        {
            if (!File.Exists(queryPath))
            {
                return new List<string>();
            }

            List<ResultData> results = new List<ResultData>();

            Luv[,] convertedQueryImage = AlgorithmHelper.convertImageToLUV(queryPath);
            Dictionary<int, CoherencePair> queryImageCoherenceVector = generateCoherenceVector(convertedQueryImage);

            List<string> dataImagePaths = Directory.GetFiles(AlgorithmHandler.IMAGES_DIRECTORY).ToList();

            string path;
            Luv[,] convertedImage;
            Dictionary<int, CoherencePair> vector;
            double similarity;
            for (int i = 0; i < dataImagePaths.Count; i++)
            {
                path = dataImagePaths[i];
                convertedImage = AlgorithmHelper.convertImageToLUV(path);
                vector = generateCoherenceVector(convertedImage);
                similarity = getSimilarity(queryImageCoherenceVector, vector, SIGNIFICANT_QUERY_THRESHOLD);
                if (similarity >= SIMILARITY_THRESHOLD)
                {
                    results.Add(new ResultData(path, similarity));
                }
                Console.WriteLine(path + " - " + similarity);
                raiseProgressUpdate((double)i / (dataImagePaths.Count - 1));
            }

            results = results.OrderByDescending(d => d.similarity).ToList();

            return results.Select(d => d.path).ToList();
        }

        private Dictionary<int, CoherencePair> generateCoherenceVector(Luv[,] image)
        {
            Dictionary<int, CoherencePair> vector = new Dictionary<int, CoherencePair>();

            //algorithm here
            //temp:
            var histogram = AlgorithmHelper.generateLUVHistogram(image);
            foreach (int key in histogram.Keys)
            {
                vector.Add(key, new CoherencePair(histogram[key] / 2, histogram[key] / 2));
            }

            return vector;
        }

        private double getSimilarity(Dictionary<int, CoherencePair> query, Dictionary<int, CoherencePair> data, double threshold)
        {
            Dictionary<int, double> compilation = new Dictionary<int, double>();
            for (int i = 0; i < query.Count; i++)
            {
                int queryKey = query.Keys.ElementAt(i);
                if (query[queryKey].coherent + query[queryKey].nonCoherent >= threshold)
                {
                    compilation.Add(queryKey, getColorSimilarity(queryKey, query, data));
                }
            }

            double total = 0.0;
            int keyCount = compilation.Keys.Count;
            foreach (int key in compilation.Keys)
            {
                total += compilation[key];
            }
            total /= keyCount;

            return total;
        }

        private double getColorSimilarity(int colorIndex, Dictionary<int, CoherencePair> query, Dictionary<int, CoherencePair> data)
        {
            CoherencePair dataCP = new CoherencePair(0, 0);
            if (data.ContainsKey(colorIndex))
            {
                dataCP = data[colorIndex];
            }
            return 1 - (Math.Abs(query[colorIndex].coherent - dataCP.coherent) / Math.Max(query[colorIndex].coherent, dataCP.coherent) + Math.Abs(query[colorIndex].nonCoherent - dataCP.nonCoherent) / Math.Max(query[colorIndex].nonCoherent, dataCP.nonCoherent)) / 2;
        }
    }
}
