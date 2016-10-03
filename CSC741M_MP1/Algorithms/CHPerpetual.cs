using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1.Algorithms
{
    public class CHPerpetual: Algorithm
    {
        public override AlgorithmEnum getAlgorithmEnum()
        {
            return AlgorithmEnum.CHPerpetualSimilarity;
        }

        public override List<string> generateResults(String queryPath)
        {
            List<string> results = new List<string>();
            return results;
        }
    }
}
