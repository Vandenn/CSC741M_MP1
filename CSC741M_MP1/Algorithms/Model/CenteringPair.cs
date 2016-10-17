using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1.Algorithms.Model
{
    public class CenteringPair
    {
        public double center { get; set; }
        public double nonCenter { get; set; }
        public CenteringPair(double center, double nonCenter)
        {
            this.center = center;
            this.nonCenter = nonCenter;
        }
    }
}
