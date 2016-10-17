using ColorMine.ColorSpaces;
using CSC741M_MP1.Algorithms.Helpers;
using CSC741M_MP1.Algorithms.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1.Algorithms
{
    public class CHBasic: Algorithm
    {
        public override AlgorithmEnum getAlgorithmEnum()
        {
            return AlgorithmEnum.CHBasic;
        }

        public override List<string> generateResults(String queryPath)
        {
            if (!File.Exists(queryPath))
            {
                return new List<string>();
            }

            List<ResultData> results = new List<ResultData>();

            Luv[,] convertedQueryImage = AlgorithmHelper.convertImageToLUV(queryPath);
            Dictionary<int, double> queryImageHistogram = AlgorithmHelper.generateLUVHistogram(convertedQueryImage);

            string path;
            Luv[,] convertedImage;
            Dictionary<int, double> histogram;
            double similarity;
            for (int i = 0; i < dataImagePaths.Count; i++)
            {
                path = dataImagePaths[i];
                convertedImage = AlgorithmHelper.convertImageToLUV(path);
                histogram = AlgorithmHelper.generateLUVHistogram(convertedImage);
                similarity = getSimilarity(queryImageHistogram, histogram, settings.RelevanceThreshold);
                if (similarity >= settings.SimilarityThreshold)
                {
                    results.Add(new ResultData(path, similarity));
                }
                raiseProgressUpdate((double)i / (dataImagePaths.Count - 1));
            }

            results = results.OrderByDescending(d => d.similarity).ToList();

            return results.Select(d => d.path).ToList();
        }

        private double getSimilarity(Dictionary<int, double> query, Dictionary<int, double> data, double threshold)
        {
            Dictionary<int, double> compilation = new Dictionary<int, double>();
            for (int i = 0; i < query.Count; i++)
            {
                int queryKey = query.Keys.ElementAt(i);
                if (query[queryKey] >= threshold)
                {
                    compilation.Add(queryKey, AlgorithmHelper.getColorExactSimilarity(queryKey, query, data));
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
    }
}
