using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Assemblies.DataContracts;

namespace Assemblies.ClientModel
{
    public abstract class PlayerPC
    {
        protected string ip, port;
        protected string name;

        protected IEnumerable<WCFScreenInformation> displays;

        public string Name
        {
            get { return name; }
        }
        public string IP
        {
            get { return ip; }
        }
        public string Port
        {
            get { return port; }
        }

        public IEnumerable<WCFScreenInformation> Displays
        {
            get { return displays; }
            set { displays = value; }
        }

        public TunerDevice[] Devices
        { get; set; }

        protected PlayerPC()
        {
        }
    }
}
