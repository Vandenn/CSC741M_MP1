using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1.Algorithms.Model
{
    public class CoherenceCenteringPair
    {
        public CenteringPair coherent { get; set; }
        public CenteringPair nonCoherent { get; set; }
        public CoherenceCenteringPair(CenteringPair coherent, CenteringPair nonCoherent)
        {
            this.coherent = coherent;
            this.nonCoherent = nonCoherent;
        }
    }
}
