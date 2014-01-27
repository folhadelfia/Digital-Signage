using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assemblies.DataContracts
{
    [DataContract()]
    public class WCFScreenInformation
    {
        [DataMember]
        public WCFRectangle Bounds { get; set; }

        [DataMember]
        public string DeviceID { get; set; }

        [DataMember]
        public bool Primary { get; set; }

        [DataMember]
        public string Name { get; set; }

        public WCFScreenInformation(WCFRectangle bounds, string devID, bool isPrimary, string name)
        {
            this.Bounds = bounds;
            this.DeviceID = devID;
            this.Primary = isPrimary;
            this.Name = name;
        }
    }
}