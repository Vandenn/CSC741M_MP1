using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1.Algorithms.Model
{
    public class ResultData
    {
        public string path { get; set; }
        public double similarity { get; set; }
        public ResultData(string path, double similarity)
        {
            this.path = path;
            this.similarity = similarity;
        }
    }
}
