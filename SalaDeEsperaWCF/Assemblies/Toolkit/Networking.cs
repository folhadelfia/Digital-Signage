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

            public static bool ValidateIPAddress(string address)
            {
                if (address == "localhost") return true;
                if (string.IsNullOrWhiteSpace(address)) return false;

                int number = 0;

                int i = 0;

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
