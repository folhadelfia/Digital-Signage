using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.DataContracts
{
    [DataContract]
    public class WCFTVConfiguration : WCFItemConfiguration
    {
        [DataMember]
        public int Frequency { get; set; }

        [DataMember]
        public string TunerDevicePath { get; set; }
    }
}