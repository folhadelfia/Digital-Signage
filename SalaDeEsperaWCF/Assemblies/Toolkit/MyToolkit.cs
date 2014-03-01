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
                if (remote.ToString() == "127.0.0.1" || remote.ToString() == "localhost" || remote.ToString() == ".") return true;

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
                    if (nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet && nic.NetworkInterfaceType != NetworkInterfaceType.Wireless80211) continue;
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        return nic.GetPhysicalAddress();
                    }
                }

                throw new ApplicationException("No physical address found", null);
            }

            /// <summary>
            /// Returns a valid unused port number, different from the ports provided
            /// </summary>
            /// <param name="excludedPorts"></param>
            /// <returns></returns>
            /// <exception cref="ApplicationException"></exception>
            public static int RandomPort(int[] excludedPorts)
            {
                int failsafeCounter = 0;

                while (failsafeCounter < 63000)
                {
                    int temp = RandomPort();

                    if (!excludedPorts.Contains(temp)) return temp;
                    failsafeCounter++;
                }

                throw new ApplicationException("Failed to find a usable port");
            }

            /// <summary>
            /// Returns a valid unused port number, different from the ports provided
            /// </summary>
            /// <param name="excludedPorts"></param>
            /// <returns></returns>
            /// <exception cref="ApplicationException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static string RandomPort(string[] excludedPorts)
            {
                List<int> ports = new List<int>();

                foreach (var port in excludedPorts)
                {
                    int temp = 0;

                    if (!(int.TryParse(port, out temp))) throw new ArgumentException(string.Format("{0} is not a valid port number.", port));

                    ports.Add(temp);
                }

                return RandomPort(ports.ToArray()).ToString();
            }

            /// <summary>
            /// Returns a valid unused port number
            /// </summary>
            /// <returns></returns>
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

        public static class EnumUtilities
        {
            public static IEnumerable<T> GetValues<T>()
            {
                return Enum.GetValues(typeof(T)).Cast<T>();
            }
        }

        public static class Sizes
        {
            public enum Base
            {
                Base2,
                Base10
            }

            /// <summary>
            /// Converts the number of bytes to the best fit unit, using the specified base
            /// </summary>
            /// <param name="bytes"></param>
            /// <param name="theBase"></param>
            /// <param name="result"></param>
            /// <param name="unit"></param>
            public static void ByteToBestFitUnit(double bytes, MyToolkit.Sizes.Base theBase, out float result, out string unit)
            {
                int b = 0;

                if(theBase == Base.Base2) b = 1024;
                else if (theBase == Base.Base10) b = 1000;
                else
                {
                    result = -1;
                    unit = "";
                    return;
                }

                if(bytes < b)                     // B
                {
                    result = Convert.ToSingle(bytes);
                    unit = "B";
                    return;
                }
                else if (bytes < Math.Pow(b,2))   // KB Base2 / kB base10
                {
                    result = Convert.ToSingle(Math.Round(bytes / b, 2));
                    switch (theBase)
                    {
                        case Base.Base2: unit = "KB";
                            break;
                        case Base.Base10: unit = "kB";
                            break;
                        default: unit = "KB";
                            break;
                    }

                    return;
                }
                else if (bytes < Math.Pow(b,3))   // MB
                {
                    result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 2), 2));
                    unit = "MB";
                }
                else if (bytes < Math.Pow(b,4))   // GB
                {
                    result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 3), 2));
                    unit = "GB";
                }
                else if (bytes < Math.Pow(b,5))   // TB
                {
                    result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 4), 2));
                    unit = "TB";
                }
                else if (bytes < Math.Pow(b,6))   // PB
                {
                    result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 5), 2));
                    unit = "PB";
                }
                else if (bytes < Math.Pow(b,7))   // EB
                {
                    result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 6), 2));
                    unit = "EB";
                }
                else if (bytes < Math.Pow(b,8))   // ZB
                {
                    result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 7), 2));
                    unit = "ZB";
                }
                else if (bytes < Math.Pow(b, 9))  // YB
                {
                    result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 8), 2));
                    unit = "MB";
                }
                else
                {
                    result = 0;
                    unit = "Wtf? Something went wrong";
                }
            }
            /// <summary>
            /// Converts the number of bytes to the best fit unit
            /// </summary>
            /// <param name="bytes"></param>
            /// <param name="result"></param>
            /// <param name="unit"></param>
            public static void ByteToBestFitUnit(double bytes, out float result, out string unit)
            {
                Sizes.ByteToBestFitUnit(bytes, Base.Base2, out result, out unit);
            }
            /// <summary>
            /// Converts the number of bytes to the best fit unit, using the specified base
            /// </summary>
            /// <param name="bytes"></param>
            /// <param name="theBase"></param>
            /// <returns></returns>
            public static string ByteToBestFitUnit(double bytes, MyToolkit.Sizes.Base theBase)
            {
                string unit = "";
                float val = 0;

                Sizes.ByteToBestFitUnit(bytes, theBase, out val, out unit);

                return string.Format("{0} {1}", val, unit);
            }
            /// <summary>
            /// Converts the number of bytes to the best fit unit
            /// </summary>
            /// <param name="bytes"></param>
            /// <returns></returns>
            public static string ByteToBestFitUnit(double bytes)
            {
                return ByteToBestFitUnit(bytes, Base.Base2);
            }
        }

        public static class Files
        {
            public static string FileNameFromPath(string path)
            {
                try
                {
                    var lio = path.LastIndexOf("\\");

                    return path.Substring(lio + 1, path.Length - lio - 1);
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }
    }
}
