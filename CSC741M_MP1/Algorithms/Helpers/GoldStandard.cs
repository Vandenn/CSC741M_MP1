using CSC741M_MP1.Algorithms.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSC741M_MP1.Algorithms.Helpers
{
    /// <summary>
    /// Gold Standard Image Files retrieved from:
    /// http://wang.ist.psu.edu/~jwang/test1.zip
    /// </summary>
    [XmlRoot("goldStandard")]
    public class GoldStandard
    {
        private static GoldStandard _instance;

        [XmlArray("files"), XmlArrayItem("file")]
        public List<GoldStandardFile> files { get; set; }

        private double precision;
        private double recall;
        private double fmeasure;

        protected GoldStandard()
        {
            precision = 0.0;
            recall = 0.0;
            fmeasure = 0.0;
        }

        public static GoldStandard getInstance()
        {
            if (_instance == null)
            {
                XmlSerializer s = new XmlSerializer(typeof(GoldStandard));
                using (var stream = new FileStream("gold_standard.xml", FileMode.Open))
                {
                    _instance = s.Deserialize(stream) as GoldStandard;
                }
            }
            return _instance;
        }

        public bool calculateEvaluation(String queryPath, List<String> results)
        {
            GoldStandardFile currentFile = files.FirstOrDefault(s => s.filename == Path.GetFileNameWithoutExtension(queryPath));
            if (currentFile == null) return false;

            double matchCount = results.Where(r => currentFile.results.Contains(Path.GetFileNameWithoutExtension(r))).Count();
            precision = matchCount / results.Count();
            recall = matchCount / currentFile.results.Count();
            fmeasure = precision + recall == 0 ? 0 : 2 * precision * recall / (precision + recall);
            return true;
        }

        public double getPrecision()
        {
            return precision;
        }

        public double getRecall()
        {
            return recall;
        }

        public double getFmeasure()
        {
            return fmeasure;
        }
    }
}
