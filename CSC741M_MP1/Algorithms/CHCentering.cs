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
    public class CHCentering: Algorithm
    {
        private class CenteringPair
        {
            public double center { get; set; }
            public double nonCenter { get; set; }
            public CenteringPair(double center, double nonCenter)
            {
                this.center = center;
                this.nonCenter = nonCenter;
            }
        }

        private const double SIGNIFICANT_QUERY_THRESHOLD = 0.05;
        private const double SIMILARITY_THRESHOLD = 0.0;
        private const double CENTERING_PERCENTAGE = 0.5;

        public override AlgorithmEnum getAlgorithmEnum()
        {
            return AlgorithmEnum.CHCentering;
        }

        public override List<string> generateResults(String queryPath)
        {
            if (!File.Exists(queryPath))
            {
                return new List<string>();
            }

            List<ResultData> results = new List<ResultData>();

            Luv[,] convertedQueryImage = AlgorithmHelper.convertImageToLUV(queryPath);
            Dictionary<int, CenteringPair> queryImageCenteringVector = generateCenteringVector(convertedQueryImage);

            List<string> dataImagePaths = Directory.GetFiles(AlgorithmHandler.IMAGES_DIRECTORY).ToList();

            string path;
            Luv[,] convertedImage;
            Dictionary<int, CenteringPair> vector;
            double similarity;
            for (int i = 0; i < dataImagePaths.Count; i++)
            {
                path = dataImagePaths[i];
                convertedImage = AlgorithmHelper.convertImageToLUV(path);
                vector = generateCenteringVector(convertedImage);
                similarity = getSimilarity(queryImageCenteringVector, vector, SIGNIFICANT_QUERY_THRESHOLD);
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

        private Dictionary<int, CenteringPair> generateCenteringVector(Luv[,] image)
        {
            Dictionary<int, CenteringPair> vector = new Dictionary<int, CenteringPair>();
            double borderPercentage = ((1 - CENTERING_PERCENTAGE) / 2);

            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    bool isCenter = i >= image.GetLength(0) * borderPercentage &&
                            i <= image.GetLength(0) - image.GetLength(0) * borderPercentage &&
                            j >= image.GetLength(1) * borderPercentage &&
                            j <= image.GetLength(1) - image.GetLength(1) * borderPercentage;
                    int key = CIEConvert.LuvIndexOf(image[i, j]);
                    if (vector.ContainsKey(key))
                    {
                        if (isCenter)
                        {
                            vector[key].center += 1.0;
                        }
                        else
                        {
                            vector[key].nonCenter += 1.0;
                        }
                    }
                    else
                    {
                        if (isCenter)
                        {
                            vector.Add(key, new CenteringPair(1.0, 0.0));
                        }
                        else
                        {
                            vector.Add(key, new CenteringPair(0.0, 1.0));
                        }
                    }
                }
            }

            double totalPixels = image.GetLength(0) * image.GetLength(1);

            foreach (int key in vector.Keys.ToList())
            {
                vector[key].center /= totalPixels;
                vector[key].nonCenter /= totalPixels;
            }

            return vector;
        }

        private double getSimilarity(Dictionary<int, CenteringPair> query, Dictionary<int, CenteringPair> data, double threshold)
        {
            Dictionary<int, double> compilation = new Dictionary<int, double>();
            for (int i = 0; i < query.Count; i++)
            {
                int queryKey = query.Keys.ElementAt(i);
                if (query[queryKey].center + query[queryKey].nonCenter >= threshold)
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

        private double getColorSimilarity(int colorIndex, Dictionary<int, CenteringPair> query, Dictionary<int, CenteringPair> data)
        {
            CenteringPair dataCP = new CenteringPair(0, 0);
            if (data.ContainsKey(colorIndex))
            {
                dataCP = data[colorIndex];
            }
            return 1 - (Math.Abs(query[colorIndex].center - dataCP.center) / Math.Max(query[colorIndex].center, dataCP.center) + Math.Abs(query[colorIndex].nonCenter - dataCP.nonCenter) / Math.Max(query[colorIndex].nonCenter, dataCP.nonCenter)) / 2;
        }
    }
}
