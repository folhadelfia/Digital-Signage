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
            bool validOption = false;

            string probePort = "", 
                   announcementPort = "";

            try
            {
                if (args.Contains("-auto"))
                {
                    probePort = "8001";
                    announcementPort = "9021";
                }
                else if (args.Length == 4)
                {
                    int probePortValue,
                        announcementPortValue;

                    if (int.TryParse(args[args.ToList().IndexOf("-pp") + 1], out probePortValue))
                        probePort = probePortValue.ToString();
                    else throw new Exception();

                    if (int.TryParse(args[args.ToList().IndexOf("-ap") + 1], out announcementPortValue))
                        announcementPort = announcementPortValue.ToString();
                    else throw new Exception();
                }

            }
            catch (Exception)
            {
                probePort = "";
                announcementPort = "";
            }
            if (string.IsNullOrWhiteSpace(probePort) || string.IsNullOrWhiteSpace(announcementPort))
            {
                #region Escolher a porta para os probes
                do
                {
                    Console.WriteLine("Port for probe (where clients will connect to find servers) " + Environment.NewLine +
                        "[ENTER: 8001 | r: random | value: 1 - 65535]:");
                    probePort = Console.ReadLine();
                    switch (probePort.ToLower())
                    {
                        case "r": probePort = MyToolkit.Networking.RandomPort().ToString();
                            validOption = true;
                            break;
                        case "": probePort = "8001";
                            validOption = true;
                            break;
                        default:
                            int i;
                            if (int.TryParse(probePort, out i))
                                validOption = i < 65535;
                            else validOption = false;
                            break;
                    }

                    Console.WriteLine((validOption ? "Listening for probes on port " : "Invalid option: ") + probePort + Environment.NewLine);
                }
                while (!validOption);
                #endregion
                #region Escolher a porta para os announcements
                do
                {
                    Console.WriteLine("Port for announcement (where servers will announce themselves) " + Environment.NewLine +
                        "[ENTER: 9021 | r: random | value: 1 - 65535]:");
                    announcementPort = Console.ReadLine();
                    switch (announcementPort.ToLower())
                    {
                        case "r": announcementPort = MyToolkit.Networking.RandomPort().ToString();
                            validOption = true;
                            break;
                        case "": announcementPort = "9021";
                            validOption = true;
                            break;
                        default:
                            int i;
                            if (int.TryParse(announcementPort, out i))
                                validOption = i < 65535;
                            else validOption = false;
                            break;
                    }

                    Console.WriteLine(validOption ? "Listening for announcements on port " + announcementPort : "Invalid option");
                }
                while (!validOption);


                #endregion
            }

            Uri probeEndpointAddress = new Uri("net.tcp://" + MyToolkit.Networking.LocalIPAddress + ":" + probePort + "/Probe"),
                announcementEndpointAddress = new Uri("net.tcp://" + MyToolkit.Networking.LocalIPAddress + ":" + announcementPort + "/Announcement");
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
