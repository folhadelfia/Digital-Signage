using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Toolkit
{
    public static class NetworkingToolkit
    {
        public const int MaxPortNumber = 65535;

        public static string LocalIPAddress
        {
            get { return GetLocalIPAddress(); }
        }

        private static string GetLocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
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
}
