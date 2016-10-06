using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1
{
    public abstract class Algorithm
    {
        public delegate void ProgressUpdateEvent(double progress);
        public event ProgressUpdateEvent ProgressUpdate;

        public abstract List<String> generateResults(String queryPath);
        public abstract AlgorithmEnum getAlgorithmEnum();

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
                case AlgorithmEnum.Bonus:
                    return "Bonus";
                default:
                    return "N/A";
            }
        }
    }
}
