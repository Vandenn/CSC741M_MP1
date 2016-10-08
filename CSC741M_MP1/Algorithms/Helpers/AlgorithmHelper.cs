using ColorMine.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1.Algorithms.Helpers
{
    public class AlgorithmHelper
    {
        public static Luv[,] convertImageToLUV(string path)
        {
            Bitmap image = new Bitmap(path);

            Luv[,] convertedImage = new Luv[image.Height, image.Width];

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color c = image.GetPixel(j, i);
                    convertedImage[i, j] = CIEConvert.RGBtoLUV(c);
                }
            }

            return convertedImage;
        }

        public static Dictionary<int, double> generateLUVHistogram(Luv[,] image)
        {
            Dictionary<int, double> histogram = new Dictionary<int, double>();

            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    int key = CIEConvert.LuvIndexOf(image[i, j]);
                    if (histogram.ContainsKey(key))
                    {
                        histogram[key] += 1.0;
                    }
                    else
                    {
                        histogram.Add(key, 1.0);
                    }
                }
            }

            double totalPixels = histogram.Sum(x => x.Value);

            foreach (int key in histogram.Keys.ToList())
            {
                histogram[key] /= totalPixels;
            }

            return histogram;
        }

        public static double getExactSimilarityLUVHistogram(Dictionary<int, double> query, Dictionary<int, double> data, double threshold)
        {
            Dictionary<int, double> compilation = new Dictionary<int, double>();
            for (int i = 0; i < query.Count; i++)
            {
                int queryKey = query.Keys.ElementAt(i);
                if (query[queryKey] >= threshold)
                {
                    compilation.Add(queryKey, getColorExactSimilarity(queryKey, query, data));
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

        public static double getPerceptualSimilarityLUVHistogram(Dictionary<int, double> query, Dictionary<int, double> data, double threshold)
        {
            Dictionary<int, double> compilation = new Dictionary<int, double>();
            for (int i = 0; i < query.Count; i++)
            {
                int queryKey = query.Keys.ElementAt(i);
                if (query[queryKey] >= threshold)
                {
                    compilation.Add(queryKey, getColorExactSimilarity(queryKey, query, data) * (1 + getColorPerceptualSimilarity(queryKey, query, data)));
                }
            }

            double total = 0.0;
            foreach (int key in compilation.Keys)
            {
                total += compilation[key] * query[key];
            }
            return total;
        }

        public static double getColorExactSimilarity(int colorIndex, Dictionary<int, double> query, Dictionary<int, double> data)
        {
            double dataNH = data.ContainsKey(colorIndex) ? data[colorIndex] : 0.0;
            return 1 - Math.Abs((query[colorIndex] - dataNH) / Math.Max(query[colorIndex], dataNH));
        }

        public static double getColorPerceptualSimilarity(int colorIndex, Dictionary<int, double> query, Dictionary<int, double> data)
        {
            double result = 0;
            double queryNH = query[colorIndex];
            double dataNH = 0.0;
            for (int i = 0; i < CIEConvert.LuvSimilarityMatrix.GetLength(1); i++)
            {
                if (CIEConvert.LuvSimilarityMatrix[colorIndex, i] > 0)
                {
                    dataNH = data.ContainsKey(i) ? data[i] : 0.0;
                    result += (1 - (Math.Abs((queryNH - dataNH) / Math.Max(queryNH, dataNH)))) * CIEConvert.LuvSimilarityMatrix[colorIndex, i];
                }
            }
            return result;
        }
    }
}
