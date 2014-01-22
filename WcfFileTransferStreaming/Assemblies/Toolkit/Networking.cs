using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Assemblies.Toolkit
{

    public static class Networking
    {
        public const int MaxPortNumber = 65535;

        public static IPAddress PrivateIPAddress
        {
            get { return GetLocalIPAddress(); }
        }
        public static IPAddress PublicIPAddress
        {
            get { return GetPublicIPAddress(); }
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

            return StringToIPAddress(a4);
        }

        public static IPAddress StringToIPAddress(string address)
        {

            string[] a5 = address.Split('.');

            byte[] ipByte = new byte[] { Convert.ToByte(a5[0], 10), Convert.ToByte(a5[1], 10), Convert.ToByte(a5[2], 10), Convert.ToByte(a5[3], 10) };

            return new IPAddress(ipByte);
        }

        public static IPAddress SubnetMask
        {
            get { return GetSubnetMask(); }
        }

        public static IPAddress GetSubnetMask()
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (PrivateIPAddress.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", PrivateIPAddress));
        }

        #region Descobrir se está na mesma subrede

        public static bool IsLocal(string address)
        {
            return IsLocal(StringToIPAddress(address));
        }

        public static bool IsLocal(IPEndPoint endpoint)
        {
            if (endpoint == null)
                return false;
            return IsLocal(endpoint.Address);
        }

        /// <summary>
        /// Returns true if the IPAddress supplied is on the same subnet as this host
        /// </summary>
        public static bool IsLocal(IPAddress remote)
        {
            IPAddress mask = SubnetMask;
            IPAddress local = PrivateIPAddress;

            if (mask == null)
                return false;

            uint maskBits = BitConverter.ToUInt32(mask.GetAddressBytes(), 0);
            uint remoteBits = BitConverter.ToUInt32(remote.GetAddressBytes(), 0);
            uint localBits = BitConverter.ToUInt32(local.GetAddressBytes(), 0);

            // compare network portions
            return ((remoteBits & maskBits) == (localBits & maskBits));
        }

        #endregion


        public static string PrivateHostname
        {
            get
            {
                return ResolveIP(Networking.PrivateIPAddress);
            }
        }
        public static string PublicHostname
        {
            get
            {
                return ResolveIP(Networking.PublicIPAddress);
            }
        }

        public static string ResolveIP(IPAddress ip)
        {
            return ResolveIP(ip.ToString());
        }
        public static string ResolveIP(string ip)
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

        public static IPAddress[] ResolveHostname(string name)
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


        public static PhysicalAddress HardwareAddress
        {
            get { return GetPhysicalAddress(); }
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
            var ips = Networking.ResolveHostname(address);

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
    }
}
