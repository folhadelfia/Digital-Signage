using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Toolkit
{
    public static class MyToolkit
    {
        public static class Networking
        {
            public const int MaxPortNumber = 65535;

            public static IPAddress LocalIPAddress
            {
                get { return GetLocalIPAddress(); }
            }

            private static IPAddress GetLocalIPAddress()
            {
                IPHostEntry host;
                IPAddress localIP = null;
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip;
                        break;
                    }
                }
                if (!(localIP == null)) return localIP;
                else throw new ApplicationException("No physical address found", null);
            }

            public static IPAddress PublicIPAddress
            {
                get { return GetPublicIPAddress(); }
            }

            private static IPAddress GetPublicIPAddress()
            {
                string url = "http://checkip.dyndns.org";
                System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                string response = sr.ReadToEnd().Trim();
                string[] a = response.Split(':');
                string a2 = a[1].Substring(1);
                string[] a3 = a2.Split('<');
                string a4 = a3[0];

                string[] a5 = a4.Split('.');

                byte[] ipByte = new byte[] { Convert.ToByte(a5[0], 10), Convert.ToByte(a5[1], 10), Convert.ToByte(a5[2], 10), Convert.ToByte(a5[3], 10) };

                return new IPAddress(ipByte);
            }

            public static PhysicalAddress HardwareAddress
            {
                get { return GetPhysicalAddress(); }
            }

            public static string Hostname
            {
                get
                {
                    return resolveIP(Networking.LocalIPAddress);
                }
            }

            private static PhysicalAddress GetPhysicalAddress()
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet) continue;
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        return nic.GetPhysicalAddress();
                    }
                }

                throw new ApplicationException("No physical address found", null);
            }

            public static int RandomPort()
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Bind(endPoint);
                    IPEndPoint local = (IPEndPoint)socket.LocalEndPoint;
                    return local.Port;
                }
            }

            public static bool ValidateAddress(string address)
            {
                var ips = Networking.resolveHostname(address);

                if (ips.Length > 0) address = ips.Where(x => Networking.ValidateIPAddress(x.ToString())).FirstOrDefault().ToString();

                return !string.IsNullOrEmpty(address) && Networking.ValidateIPAddress(address);
            }

            public static bool ValidateIPAddress(string address)
            {
                if (address == "localhost") return true;
                if (string.IsNullOrWhiteSpace(address)) return false;

                int number = 0;

                string[] arrayNumbers = address.Split('.');

                if (arrayNumbers.Count() != 4)
                    return false;

                foreach (string num in arrayNumbers)
                {
                    if (int.TryParse(num, out number))
                    {
                        if (number > 255 || number < 0)
                            return false;
                    }
                    else return false;
                }

                return true;
            }

            public static bool ValidatePort(string port)
            {
                int portNumber = 0;
                if (int.TryParse(port, out portNumber))
                    return portNumber >= 0 && portNumber <= MaxPortNumber;
                else return false;
            }

            public static string resolveIP(IPAddress ip)
            {
                return resolveIP(ip.ToString());
            }

            public static string resolveIP(string ip)
            {
                try
                {

                    IPHostEntry ipResolvido = Dns.GetHostEntry(ip);
                    return ipResolvido.HostName;
                }
                catch
                {
                    return string.Empty;
                }
            }

            public static IPAddress[] resolveHostname(string name)
            {
                try
                {
                    IPHostEntry enderecoResolvido = Dns.GetHostEntry(name);

                    return enderecoResolvido.AddressList;
                }
                catch
                {
                    return null;
                }


            }
        }

        public static class Hardware
        {
            public static string UUID
            {
                get { return GetUUID(); }
            }

            private static string GetUUID()
            {
                string uuid = string.Empty;

                ManagementClass mc = new ManagementClass("Win32_ComputerSystemProduct");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    uuid = mo.Properties["UUID"].Value.ToString();
                    break;
                }

                return uuid;
            }
        }
    }
}
