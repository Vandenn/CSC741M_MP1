using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1.Algorithms.Model
{
    public class CoherencePair
    {
        public double coherent { get; set; }
        public double nonCoherent { get; set; }
        public CoherencePair(double coherent, double nonCoherent)
        {
            this.coherent = coherent;
            this.nonCoherent = nonCoherent;
        }
    }
}
