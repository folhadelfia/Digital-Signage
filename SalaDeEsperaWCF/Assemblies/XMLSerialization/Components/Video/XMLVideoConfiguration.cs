using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.XMLSerialization.Components
{
    public class XMLVideoConfiguration : XMLItemConfiguration
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string[] Playlist { get; set; }
        public XMLAspect Aspect { get; set; }
        public bool Replay { get; set; }
    }
}
