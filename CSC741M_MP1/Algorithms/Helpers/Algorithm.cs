using CSC741M_MP1.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1
{
    /// <summary>
    /// Base class for all the CBIR algorithms.
    /// </summary>
    public abstract class Algorithm
    {
        // Event properties for updating process/algorithm progress
        public delegate void ProgressUpdateEvent(double progress);
        public event ProgressUpdateEvent ProgressUpdate;

        // Function prototypes that children must implement
        public abstract List<String> generateResults(String queryPath);
        public abstract AlgorithmEnum getAlgorithmEnum();

        // Reference to program settings singleton
        protected Settings settings;

        // List of the paths of database images
        protected List<string> dataImagePaths;

        protected Algorithm()
        {
            settings = Settings.getSettings();
            dataImagePaths = Directory.GetFiles(settings.DatabaseImagesPath).Where(p => p.EndsWith(".jpg") || p.EndsWith(".jpeg")).ToList();
        }

        /// <summary>
        /// Function to be called by children every time the algorithm's progress updates.
        /// </summary>
        /// <param name="progress">Progress percentage</param>
        protected void raiseProgressUpdate(double progress)
        {
            ProgressUpdate(progress);
        }

        public static string AlgorithmEnumToString(AlgorithmEnum e)
        {
            switch (e)
            {
                case AlgorithmEnum.CHBasic:
                    return "Color Histogram Method";
                case AlgorithmEnum.CHPerpetualSimilarity:
                    return "CH with Perceptual Similarity";
                case AlgorithmEnum.HistogramRefinementColorCoherence:
                    return "Histogram Refinement with Color Coherence";
                case AlgorithmEnum.CHCentering:
                    return "CH with Centering Refinement";
                case AlgorithmEnum.SuccessiveRefinement:
                    return "CH with Successive Refinement";
                default:
                    return "N/A";
            }
        }
    }
}
