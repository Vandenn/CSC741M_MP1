using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSC741M_MP1.Algorithms.Model
{
    public class GoldStandardFile
    {
        public string filename { get; set; }

        [XmlArray("results"), XmlArrayItem("result")]
        public List<string> results { get; set; }
    }
}
