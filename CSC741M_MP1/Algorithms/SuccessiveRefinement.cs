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
    public class SuccessiveRefinement : Algorithm
    {
        public override AlgorithmEnum getAlgorithmEnum()
        {
            return AlgorithmEnum.SuccessiveRefinement;
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
            Dictionary<int, CoherenceCenteringPair> queryImageCoherenceVector = queryCalculator.generateCoherenceCenteringVector();

            // Preprocess and compare all database images
            string path;
            Luv[,] convertedImage;
            CoherenceCalculator calculator;
            Dictionary<int, CoherenceCenteringPair> vector;
            double similarity;
            for (int i = 0; i < dataImagePaths.Count; i++)
            {
                path = dataImagePaths[i];
                convertedImage = AlgorithmHelper.convertImageToLUV(path);
                calculator = new CoherenceCalculator(convertedImage);
                vector = calculator.generateCoherenceCenteringVector();
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

        private double getSimilarity(Dictionary<int, CoherenceCenteringPair> query, Dictionary<int, CoherenceCenteringPair> data, double threshold)
        {
            Dictionary<int, double> compilationCoherentCenter = new Dictionary<int, double>();
            Dictionary<int, double> compilationCoherentNonCenter = new Dictionary<int, double>();
            Dictionary<int, double> compilationNonCoherentCenter = new Dictionary<int, double>();
            Dictionary<int, double> compilationNonCoherentNonCenter = new Dictionary<int, double>();

            for (int i = 0; i < query.Count; i++)
            {
                int queryKey = query.Keys.ElementAt(i);

                if (query[queryKey].coherent.center >= threshold)
                {
                    compilationCoherentCenter.Add(queryKey, getColorExactSimilarity(queryKey, query, data, true, true));
                }
                if (query[queryKey].coherent.nonCenter >= threshold)
                {
                    compilationCoherentNonCenter.Add(queryKey, getColorExactSimilarity(queryKey, query, data, true, false));
                }
                if (query[queryKey].nonCoherent.center >= threshold)
                {
                    compilationNonCoherentCenter.Add(queryKey, getColorExactSimilarity(queryKey, query, data, false, true));
                }
                if (query[queryKey].nonCoherent.nonCenter >= threshold)
                {
                    compilationNonCoherentNonCenter.Add(queryKey, getColorExactSimilarity(queryKey, query, data, false, false));
                }
            }

            int keyCount = compilationCoherentCenter.Keys.Count + compilationCoherentNonCenter.Keys.Count + compilationNonCoherentCenter.Keys.Count + compilationNonCoherentNonCenter.Keys.Count;
            double total = compilationCoherentCenter.Sum(x => x.Value) + compilationNonCoherentCenter.Sum(x => x.Value) + compilationCoherentNonCenter.Sum(x => x.Value) + compilationNonCoherentNonCenter.Sum(x => x.Value);
            total /= keyCount;

            return total;
        }

        public static double getColorExactSimilarity(int colorIndex, Dictionary<int, CoherenceCenteringPair> query, Dictionary<int, CoherenceCenteringPair> data, bool coherent, bool center)
        {
            double queryNH = 0.0;
            double dataNH = 0.0;

            if (coherent)
            {
                queryNH = center ? query[colorIndex].coherent.center : query[colorIndex].coherent.nonCenter;
                if (data.ContainsKey(colorIndex))
                {
                    dataNH = center ? data[colorIndex].coherent.center : data[colorIndex].coherent.nonCenter;
                }
            }
            else
            {
                queryNH = center ? query[colorIndex].nonCoherent.center : query[colorIndex].nonCoherent.nonCenter;
                if (data.ContainsKey(colorIndex))
                {
                    dataNH = center ? data[colorIndex].nonCoherent.center : data[colorIndex].nonCoherent.nonCenter;
                }
            }
            
            return 1 - Math.Abs((queryNH - dataNH) / Math.Max(queryNH, dataNH));
        }
    }
}
