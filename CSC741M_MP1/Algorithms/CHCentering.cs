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
    public class CHCentering: Algorithm
    {
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

            // Query image initialization
            Luv[,] convertedQueryImage = AlgorithmHelper.convertImageToLUV(queryPath);
            Dictionary<int, CenteringPair> queryImageCenteringVector = generateCenteringVector(convertedQueryImage);

            // Preprocess and compare all database images
            string path;
            Luv[,] convertedImage;
            Dictionary<int, CenteringPair> vector;
            double similarity;
            for (int i = 0; i < dataImagePaths.Count; i++)
            {
                path = dataImagePaths[i];
                convertedImage = AlgorithmHelper.convertImageToLUV(path);
                vector = generateCenteringVector(convertedImage);
                similarity = getSimilarity(queryImageCenteringVector, vector, settings.RelevanceThreshold);
                if (similarity >= settings.SimilarityThreshold)
                {
                    results.Add(new ResultData(path, similarity));
                }
                //Console.WriteLine(path + " - " + similarity);
                raiseProgressUpdate((double)i / (dataImagePaths.Count - 1));
            }

            results = results.OrderByDescending(d => d.similarity).ToList();

            return results.Select(d => d.path).ToList();
        }

        private Dictionary<int, CenteringPair> generateCenteringVector(Luv[,] image)
        {
            Dictionary<int, CenteringPair> vector = new Dictionary<int, CenteringPair>();
            double borderPercentage = ((1 - settings.CenterAmount) / 2);

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
            Dictionary<int, double> compilationCenter = new Dictionary<int, double>();
            Dictionary<int, double> compilationNonCenter = new Dictionary<int, double>();
            for (int i = 0; i < query.Count; i++)
            {
                int queryKey = query.Keys.ElementAt(i);
                
                if (query[queryKey].center >= threshold)
                {
                    compilationCenter.Add(queryKey, getColorExactSimilarity(queryKey, query, data, true));
                }
                if (query[queryKey].nonCenter >= threshold)
                {
                    compilationNonCenter.Add(-queryKey, getColorExactSimilarity(queryKey, query, data, false));
                }
            }

            int keyCount = compilationCenter.Keys.Count + compilationNonCenter.Keys.Count;
            double total = compilationCenter.Sum(x => x.Value) + compilationNonCenter.Sum(x => x.Value);
            total /= keyCount;

            return total;
        }

        public static double getColorExactSimilarity(int colorIndex, Dictionary<int, CenteringPair> query, Dictionary<int, CenteringPair> data, bool center)
        {
            double queryNH = center ? query[colorIndex].center : query[colorIndex].nonCenter;
            double dataNH = data.ContainsKey(colorIndex) ? center ? data[colorIndex].center : data[colorIndex].nonCenter : 0.0;
            return 1 - Math.Abs((queryNH - dataNH) / Math.Max(queryNH, dataNH));
        }

        /*// Old Implementation
        private double getColorSimilarity(int colorIndex, Dictionary<int, CenteringPair> query, Dictionary<int, CenteringPair> data)
        {
            CenteringPair dataCP = new CenteringPair(0, 0);
            if (data.ContainsKey(colorIndex))
            {
                dataCP = data[colorIndex];
            }
            return 1 - (Math.Abs(query[colorIndex].center - dataCP.center) / Math.Max(query[colorIndex].center, dataCP.center) + Math.Abs(query[colorIndex].nonCenter - dataCP.nonCenter) / Math.Max(query[colorIndex].nonCenter, dataCP.nonCenter)) / 2;
        }*/
    }
}
