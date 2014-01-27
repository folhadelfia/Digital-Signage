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
        private EndpointAddress playerEndpoint;
        private EndpointAddress fileTransferEndpoint;

        public EndpointAddress PlayerEndpoint
        {
            get { return playerEndpoint; }
            set 
            {
                this.ip = value.Uri.Host;
                //this.port = value.Uri.Port.ToString();
                this.name = MyToolkit.Networking.ResolveIP(value.Uri.Host);
                playerEndpoint = value; 
            }
        }

        public EndpointAddress FileTransferEndpoint
        {
            get { return fileTransferEndpoint; }
            set { fileTransferEndpoint = value; }
        }
    }
}
