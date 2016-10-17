using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSC741M_MP1.Model
{
    [XmlRoot("Settings")]
    public class Settings
    {
        private static Settings _instance;

        private string defaultSearchpath;
        private string databaseImagesPath;
        private double similarityThreshold;
        private double relevanceThreshold;
        private double centerAmount;
        private bool eightConnected;

        public string DefaultSearchPath
        {
            get { return defaultSearchpath; }
            set
            {
                if (Directory.Exists(value))
                    defaultSearchpath = value;
            }
        }
        public string DatabaseImagesPath
        {
            get { return databaseImagesPath; }
            set
            {
                if (Directory.Exists(value))
                    databaseImagesPath = value;
            }
        }
        public double SimilarityThreshold
        {
            get { return similarityThreshold; }
            set
            {
                if (value >= 0 && value <= 1)
                    similarityThreshold = value;
            }
        }
        public double RelevanceThreshold
        {
            get { return relevanceThreshold; }
            set
            {
                if (value >= 0 && value <= 1)
                    relevanceThreshold = value;
            }
        }
        public double CenterAmount
        {
            get { return centerAmount; }
            set
            {
                if (value >= 0 && value <= 1)
                    centerAmount = value;
            }
        }
        public bool EightConnected
        {
            get { return eightConnected; }
            set
            {
                eightConnected = value;
            }
        }

        protected Settings()
        {
            defaultSearchpath = @"C:\";
            databaseImagesPath = @"C:\";
            similarityThreshold = 0.0;
            relevanceThreshold = 0.05;
            centerAmount = 0.5;
            eightConnected = false;
        }

        public static Settings getSettings()
        {
            if (_instance == null)
            {
                XmlSerializer s = new XmlSerializer(typeof(Settings));
                using (var stream = new FileStream("settings.xml", FileMode.Open))
                {
                    _instance = s.Deserialize(stream) as Settings;
                }
            }
            return _instance;
        }

        public void saveSettings()
        {
            if (_instance != null)
            {
                XmlSerializer s = new XmlSerializer(typeof(Settings));
                using (var stream = new FileStream("settings.xml", FileMode.Create))
                {
                    s.Serialize(stream, _instance);
                }
            }
        }
    }
}
