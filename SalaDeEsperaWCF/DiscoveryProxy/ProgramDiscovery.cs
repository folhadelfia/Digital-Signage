using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Net;
using System.Net.Sockets;

using Assemblies.Toolkit;

namespace ServiceDiscoveryProxy
{
    class ProgramDiscovery
    {

        static void Main(string[] args)
        {
            string port = "";
            bool validOption = false;

            #region Escolher a porta para os probes
            do
            {
                Console.WriteLine("Port for probe (where clients will connect to find servers) " + Environment.NewLine + 
                    "[ENTER: 8001 | r: random | value: 1 - 65535]:");
                port = Console.ReadLine();
                switch (port.ToLower())
                {
                    case "r": port = NetworkingToolkit.RandomPort().ToString();
                        validOption = true;
                        break;
                    case "": port = "8001";
                        validOption = true;
                        break;
                    default:
                        int i;
                        if (int.TryParse(port, out i))
                            validOption = i < 65535;
                        else validOption = false;
                        break;
                }

                Console.WriteLine((validOption ? "Listening for probes on port " : "Invalid option: ") + port + Environment.NewLine);
            }
            while (!validOption);

            Uri probeEndpointAddress = new Uri("net.tcp://" + NetworkingToolkit.LocalIPAddress + ":" + port +"/Probe");
            #endregion
            #region Escolher a porta para os announcements
            do
            {
                Console.WriteLine("Port for announcement (where servers will announce themselves) " + Environment.NewLine +
                    "[ENTER: 9021 | r: random | value: 1 - 65535]:"); 
                port = Console.ReadLine();
                switch (port.ToLower())
                {
                    case "r": port = NetworkingToolkit.RandomPort().ToString();
                        validOption = true;
                        break;
                    case "": port = "9021";
                        validOption = true;
                        break;
                    default:
                        int i;
                        if (int.TryParse(port, out i))
                            validOption = i < 65535;
                        else validOption = false;
                        break;
                }

                Console.WriteLine(validOption ? "Listening for announcements on port " + port : "Invalid option");
            }
            while (!validOption);

            Uri announcementEndpointAddress = new Uri("net.tcp://" + NetworkingToolkit.LocalIPAddress + ":" + port + "/Announcement");
            #endregion

            //Host the DiscoveryProxy service
            ServiceDiscoveryProxy proxyServiceInstance = new ServiceDiscoveryProxy();
            ServiceHost proxyServiceHost = new ServiceHost(proxyServiceInstance);

            try
            {
                //Add DiscoveryEndpoint to receive Probe and Resolve messages
                NetTcpBinding bindingDiscovery = new NetTcpBinding();
                bindingDiscovery.Security.Mode = SecurityMode.None;
                bindingDiscovery.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;

                DiscoveryEndpoint discoveryEndpoint = new DiscoveryEndpoint(bindingDiscovery, new EndpointAddress(probeEndpointAddress));

                discoveryEndpoint.IsSystemEndpoint = false;

                //Add AnnouncementEndpoint to receive HAI and KTHXBYE announcement messages
                NetTcpBinding bindingAnnouncement = new NetTcpBinding();
                bindingAnnouncement.Security.Mode = SecurityMode.None;
                //bindingAnnouncement.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;

                AnnouncementEndpoint announcementEndpoint = new AnnouncementEndpoint(bindingAnnouncement, new EndpointAddress(announcementEndpointAddress));

                proxyServiceHost.AddServiceEndpoint(discoveryEndpoint);
                proxyServiceHost.AddServiceEndpoint(announcementEndpoint);

                proxyServiceInstance.PrintDiscoveryMetadata += ServiceDiscoveryProxy_PrintDiscoveryMetadata;

                proxyServiceHost.Open();

                Console.WriteLine("Proxy Service started.");
                Console.WriteLine("Probe endpoint: " + discoveryEndpoint.Address);
                Console.WriteLine("Announcement endpoint: " + announcementEndpoint.Address);
                Console.WriteLine();
                Console.WriteLine("Press <ENTER> to terminate the service.");
                Console.WriteLine();
                Console.ReadLine();

                proxyServiceHost.Close();
            }
            catch (CommunicationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e.Message);
            }

            if (proxyServiceHost.State != CommunicationState.Closed)
            {
                Console.WriteLine("Aborting the service...");
                proxyServiceHost.Abort();
            }
        }

        static void ServiceDiscoveryProxy_PrintDiscoveryMetadata(EndpointDiscoveryMetadata endpointDiscoveryMetadata, string verb, int serviceCount)
        {
            Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + verb + " service with the address" + Environment.NewLine + endpointDiscoveryMetadata.Address);
            Console.WriteLine("Active services: " + serviceCount);
        }
    }
}
