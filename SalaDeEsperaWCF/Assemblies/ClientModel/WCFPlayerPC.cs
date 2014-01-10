using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Assemblies.Toolkit;

namespace Assemblies.ClientModel
{
    public class WCFPlayerPC : PlayerPC
    {
        private EndpointAddress endpoint;

        public EndpointAddress Endpoint
        {
            get { return endpoint; }
            set 
            {
                this.ip = value.Uri.Host;
                this.port = value.Uri.Port.ToString();
                this.name = MyToolkit.Networking.ResolveIP(value.Uri.Host);
                endpoint = value; 
            }
        }
    }
}
