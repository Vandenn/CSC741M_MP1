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

        public static double getSimilarityLUVHistogram(Dictionary<int, double> query, Dictionary<int, double> data, double threshold)
        {
            Dictionary<int, double> compilation = new Dictionary<int, double>();
            for (int i = 0; i < query.Count; i++)
            {
                int queryKey = query.Keys.ElementAt(i);
                if (query[queryKey] >= threshold)
                {
                    compilation.Add(queryKey, 0.0);
                    double dataValue = 0.0;
                    if (data.ContainsKey(queryKey))
                    {
                        dataValue = data[queryKey];
                        data.Remove(queryKey);
                    }
                    compilation[queryKey] = 1 - Math.Abs((query[queryKey] - dataValue) / Math.Max(query[queryKey], dataValue));
                }
            }

            /*
            for (int i = 0; i < data.Count; i++)
            {
                int dataKey = data.Keys.ElementAt(i);
                compilation.Add(dataKey, 0.0);
            }
            */

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
