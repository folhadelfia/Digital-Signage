using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Configurations
{
    public class TVConfiguration : ItemConfiguration
    {
        public int Frequency { get; set; }
        public string TunerDevicePath { get; set; }

        public string AudioDecoder { get; set; }
        public string AudioRenderer { get; set; }
        public string H264Decoder { get; set; }
        public string MPEG2Decoder { get; set; }
    }
}
