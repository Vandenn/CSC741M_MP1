using System;
using System.Collections.Generic;
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
            List<string> results = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                results.Add(@"D:\images\1" + i + ".jpg");
            }
            return results;
        }
    }
}
