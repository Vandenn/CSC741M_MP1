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
    public class BonusAlgorithm: Algorithm
    {
        public override AlgorithmEnum getAlgorithmEnum()
        {
            return AlgorithmEnum.Bonus;
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
            CoherenceCalculator queryCalculator = new CoherenceCalculator(convertedQueryImage);
            Dictionary<int, CoherencePair> queryImageCoherenceVector = queryCalculator.generateCoherenceVector(convertedQueryImage, queryImageCenteringVector);

            // Preprocess and compare all database images
            string path;
            Luv[,] convertedImage;
            Dictionary<int, CenteringPair> centeringVector;
            CoherenceCalculator calculator;
            Dictionary<int, CoherencePair> ccVector;
            double similarity;
            for (int i = 0; i < dataImagePaths.Count; i++)
            {
                path = dataImagePaths[i];
                convertedImage = AlgorithmHelper.convertImageToLUV(path);
                centeringVector = generateCenteringVector(convertedImage);
                calculator = new CoherenceCalculator(convertedImage);
                ccVector = calculator.generateCoherenceVector(convertedImage, centeringVector);
                similarity = getSimilarityCC(queryImageCoherenceVector, ccVector, settings.RelevanceThreshold);
                if (similarity >= settings.SimilarityThreshold)
                {
                    results.Add(new ResultData(path, similarity));
                }
                Console.WriteLine(path + " - " + similarity);
                raiseProgressUpdate((double)i / (dataImagePaths.Count - 1));
            }

            //CC
            
            results = results.OrderByDescending(d => d.similarity).ToList();

            return results.Select(d => d.path).ToList();
        }

        //START CENTERING
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
        /*
        private double getSimilarityCentering(Dictionary<int, CenteringPair> query, Dictionary<int, CenteringPair> data, double threshold)
        {
            Dictionary<int, double> compilationCenter = new Dictionary<int, double>();
            Dictionary<int, double> compilationNonCenter = new Dictionary<int, double>();
            for (int i = 0; i < query.Count; i++)
            {
                int queryKey = query.Keys.ElementAt(i);

                if (query[queryKey].center >= threshold)
                {
                    compilationCenter.Add(queryKey, getColorExactSimilarityCentering(queryKey, query, data, true));
                }
                if (query[queryKey].nonCenter >= threshold)
                {
                    compilationNonCenter.Add(-queryKey, getColorExactSimilarityCentering(queryKey, query, data, false));
                }
            }

            int keyCount = compilationCenter.Keys.Count + compilationNonCenter.Keys.Count;
            double total = compilationCenter.Sum(x => x.Value) + compilationNonCenter.Sum(x => x.Value);
            total /= keyCount;

            return total;
        }

        public static double getColorExactSimilarityCentering(int colorIndex, Dictionary<int, CenteringPair> query, Dictionary<int, CenteringPair> data, bool center)
        {
            double queryNH = center ? query[colorIndex].center : query[colorIndex].nonCenter;
            double dataNH = data.ContainsKey(colorIndex) ? center ? data[colorIndex].center : data[colorIndex].nonCenter : 0.0;
            return 1 - Math.Abs((queryNH - dataNH) / Math.Max(queryNH, dataNH));
        }
        */
        //END CENTERING
        //START COLOR COHERENCE (CC)
        private double getSimilarityCC(Dictionary<int, CoherencePair> query, Dictionary<int, CoherencePair> data, double threshold)
        {
            Dictionary<int, double> compilationCoherent = new Dictionary<int, double>();
            Dictionary<int, double> compilationNonCoherent = new Dictionary<int, double>();
            for (int i = 0; i < query.Count; i++)
            {
                int queryKey = query.Keys.ElementAt(i);

                if (query[queryKey].coherent >= threshold)
                {
                    compilationCoherent.Add(queryKey, getColorExactSimilarityCC(queryKey, query, data, true));
                }
                if (query[queryKey].nonCoherent >= threshold)
                {
                    compilationNonCoherent.Add(-queryKey, getColorExactSimilarityCC(queryKey, query, data, false));
                }
            }

            int keyCount = compilationCoherent.Keys.Count + compilationNonCoherent.Keys.Count;
            double total = compilationCoherent.Sum(x => x.Value) + compilationNonCoherent.Sum(x => x.Value);
            total /= keyCount;

            return total;
        }
        
        public static double getColorExactSimilarityCC(int colorIndex, Dictionary<int, CoherencePair> query, Dictionary<int, CoherencePair> data, bool coherent)
        {
            double queryNH = coherent ? query[colorIndex].coherent : query[colorIndex].nonCoherent;
            double dataNH = data.ContainsKey(colorIndex) ? coherent ? data[colorIndex].coherent : data[colorIndex].nonCoherent : 0.0;
            return 1 - Math.Abs((queryNH - dataNH) / Math.Max(queryNH, dataNH));
        }
        //END COLOR COHERENCE
    }
}
