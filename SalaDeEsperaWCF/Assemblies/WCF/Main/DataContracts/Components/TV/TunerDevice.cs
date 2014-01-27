using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.DataContracts
{
    [DataContract]
    public class GeneralDevice
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string DevicePath { get; set; }

        #region Overrides

        public override bool Equals(object obj)
        {
            return obj is GeneralDevice && (obj as GeneralDevice).DevicePath == this.DevicePath;
        }

        public override int GetHashCode()
        {
            return 7;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Name, this.DevicePath);
        }

        #endregion
    }
}
