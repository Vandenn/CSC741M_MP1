using ColorMine.ColorSpaces;
using CSC741M_MP1.Algorithms.Helpers;
using CSC741M_MP1.Algorithms.Model;
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
 
            // Query image initialization
            Luv[,] convertedQueryImage = AlgorithmHelper.convertImageToLUV(queryPath);
            CoherenceCalculator queryCalculator = new CoherenceCalculator(convertedQueryImage);
            Dictionary<int, CoherencePair> queryImageCoherenceVector = queryCalculator.generateCoherenceVector();

            // Preprocess and compare all database images
            string path;
            Luv[,] convertedImage;
            CoherenceCalculator calculator;
            Dictionary<int, CoherencePair> vector;
            double similarity;
            for (int i = 0; i < dataImagePaths.Count; i++)
            {
                path = dataImagePaths[i];
                convertedImage = AlgorithmHelper.convertImageToLUV(path);
                calculator = new CoherenceCalculator(convertedImage);
                vector = calculator.generateCoherenceVector();
                similarity = getSimilarity(queryImageCoherenceVector, vector, settings.RelevanceThreshold);
                if (similarity >= settings.SimilarityThreshold)
                {
                    results.Add(new ResultData(path, similarity));
                }
                Console.WriteLine(path + " - " + similarity);
                raiseProgressUpdate((double)i / (dataImagePaths.Count - 1));
            }

            results = results.OrderByDescending(d => d.similarity).ToList();

            return results.Select(d => d.path).ToList();
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
