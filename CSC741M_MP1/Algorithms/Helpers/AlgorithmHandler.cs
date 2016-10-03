using CSC741M_MP1.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1
{
    public class AlgorithmHandler
    {
        private static AlgorithmHandler _instance;

        private List<Algorithm> algorithms;

        protected AlgorithmHandler()
        {
            algorithms = new List<Algorithm>();
            algorithms.Add(new CHBasic());
            algorithms.Add(new CHPerpetual());
            algorithms.Add(new HistogramCoherence());
            algorithms.Add(new CHCentering());
            algorithms.Add(new BonusAlgorithm());
        }

        public List<string> runAlgorithm(string queryPath, AlgorithmEnum algorithm)
        {
            Algorithm toBeRun = algorithms.FirstOrDefault(a => a.getAlgorithmEnum() == algorithm);
            if (toBeRun != null)
            {
                return toBeRun.generateResults(queryPath);
            }
            else
            {
                return new List<string>();
            }
        }

        public List<string> getAllAlgorithmNames()
        {
            List<string> algorithmNames = new List<String>();
            foreach (Algorithm algo in algorithms)
            {
                algorithmNames.Add(Algorithm.AlgorithmEnumToString(algo.getAlgorithmEnum()));
            }
            return algorithmNames;
        }

        /// <summary>
        /// Singleton Constructor
        /// </summary>
        /// <returns>Instance of single Algorithm Handler.</returns>
        public static AlgorithmHandler getInstance()
        {
            if (_instance == null)
            {
                _instance = new AlgorithmHandler();
            }
            return _instance;
        }
    }
}
