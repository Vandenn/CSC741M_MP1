using CSC741M_MP1.Algorithms.Helpers;
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
        private const double SIMILARITY_THRESHOLD = 0.5;

        private class CHBasicData
        {
            public string path { get; set; }
            public LUVClass[,] convertedImage { get; set; }
            public Dictionary<int,double> histogram { get; set; }
            public bool isMatch { get; set; }
        }

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

            List<string> results = new List<string>();
            List<CHBasicData> data = new List<CHBasicData>();

            LUVClass[,] convertedQueryImage = AlgorithmHelper.convertImageToLUV(queryPath);
            Dictionary<int, double> queryImageHistogram = AlgorithmHelper.generateLUVHistogram(convertedQueryImage);

            List<string> dataImagePaths = Directory.GetFiles(AlgorithmHandler.IMAGES_DIRECTORY).ToList();

            for (int i = 0; i < dataImagePaths.Count; i++)
            {
                CHBasicData newData = new CHBasicData();
                newData.path = dataImagePaths[i];
                newData.convertedImage = AlgorithmHelper.convertImageToLUV(dataImagePaths[i]);
                data.Add(newData);
            }

            for (int i = 0; i < data.Count; i++)
            {
                data[i].histogram = AlgorithmHelper.generateLUVHistogram(data[i].convertedImage);
            }

            for (int i = 0; i < data.Count; i++)
            {
                data[i].isMatch = AlgorithmHelper.isSimilar(queryImageHistogram, data[i].histogram, SIMILARITY_THRESHOLD);
            }

            results = data.Where(d => d.isMatch == true).Select(d => d.path).ToList();

            return results;
        }
    }
}
