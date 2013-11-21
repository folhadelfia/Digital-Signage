using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;
using Assemblies.Configurations;
using Assemblies.Toolkit;
using Assemblies.ClientProxies;
using Assemblies.DataContracts;
using Assemblies.PlayerServiceContracts;
using System.Threading;

namespace Assemblies.ClientModel
{
    public class WCFConnection : Connection
    {
        private PlayerProxy player;

        #region Members of IConnection

        public override string CompleteServerAddress
        {
            get { return string.Format(@"net.tcp://{0}:{1}/Probe", ServerIP, ServerPort); }
        } //Feito
        public override ConnectionState State
        {
            get 
            {
                if (player == null) return ConnectionState.Closed;

                switch (player.State)
                {
                    case CommunicationState.Closed: return ConnectionState.Closed;
                    case CommunicationState.Closing: return ConnectionState.Closing;
                    case CommunicationState.Created: return ConnectionState.Created;
                    case CommunicationState.Faulted: return ConnectionState.Failed;
                    case CommunicationState.Opened: return ConnectionState.Open;
                    case CommunicationState.Opening: return ConnectionState.Opening;
                    default: return ConnectionState.Closed;
                }
            }
        } //Feito

        public WCFConnection() { }
        public WCFConnection(WCFPlayerPC pc) : base(pc) 
        { 
            this.ServerIP = pc.IP;
            this.ServerPort = pc.Port;
        }

        public override void Open()
        {
            if (!(pc is WCFPlayerPC))
                return;

            try
            {
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None) { ReceiveTimeout = new TimeSpan(0, 5, 0) };
                WCFPlayerPC wcfComputer = pc as WCFPlayerPC;
                player = new PlayerProxy(binding, wcfComputer.Endpoint);
                player.Open();
            }
            catch
            {
            }
        }//Feito
        public override void Close()
        {
            try
            {
                player.Close();
            }
            catch
            {
            }
        } //Feito
        public override IEnumerable<PlayerPC> GetPlayers()
        {
            List<WCFPlayerPC> pcs = new List<WCFPlayerPC>();

            Uri probeEndpointAddress = new Uri(CompleteServerAddress);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;

            DiscoveryEndpoint discoveryEndpoint = new DiscoveryEndpoint(binding, new EndpointAddress(probeEndpointAddress));

            DiscoveryClient discoveryClient = new DiscoveryClient(discoveryEndpoint);

            try
            {
                FindResponse findResponse = discoveryClient.Find(new FindCriteria(typeof(IPlayer)));

                WCFPlayerPC pc;

                foreach (var endpoint in findResponse.Endpoints)
                {
                    try
                    {
                        NetTcpBinding bindingPC = new NetTcpBinding();
                        binding.Security.Mode = SecurityMode.None;
                        binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;

                        PlayerProxy client = new PlayerProxy(binding, endpoint.Address);

                        client.Open();

                        List<WCFScreenInformation> displays = client.GetDisplayInformation().ToList<WCFScreenInformation>();

                        client.Close();

                        pc = new WCFPlayerPC() { Displays = displays, Endpoint = endpoint.Address };

                        pcs.Add(pc);
                    }
                    catch (EndpointNotFoundException)
                    {
                        continue;
                    }
                }
            }
            catch
            {
                return null;
            }

            return pcs.Cast<PlayerPC>();
        } //Feito

        public override void Dispose()
        {
            try
            {
                player.Close();
            }
            catch
            {
                if (player.State != CommunicationState.Closed)
                    player.Abort();
            }
        }

        //A partir daqui é tudo feito a partir do Player
        // TODO - Alterar os métodos para receberem o id do display em vez de chamar sempre o principal

        #region Janela do Player
        public override void OpenPlayerWindow(PlayerWindowInformation configurations)
        {
            try
            {
                //Se o utilizador não escolher nenhum tuner, escolhe automaticamente o primeiro
                foreach (var item in configurations.Components.OfType<TVConfiguration>())
                {
                    if (string.IsNullOrWhiteSpace(item.TunerDevicePath))
                    {
                        if (player.GetTunerDevices().Count() > 0) item.TunerDevicePath = player.GetTunerDevices()[0].DevicePath;
                    }
                }

                player.OpenPlayer(NetWCFConverter.ToWCF(configurations));
            }
            catch
            {
            }
        }
        public override void ClosePlayerWindow(string displayDeviceID)
        {
            player.ClosePlayer(displayDeviceID);
        } //ALTERAR PARA RECEBER O DEVICEID DO MONITOR
        public override ScreenInformation[] GetDisplayInformation()
        {
            return NetWCFConverter.ToNET(player.GetDisplayInformation());
        }
        public override bool PlayerWindowIsOpen(string displayDeviceID)
        {
            return player.PlayerWindowIsOpen(displayDeviceID);
        }
        #endregion

        #region TV
        public override IEnumerable<TV2Lib.Channel> GetTVChannels()
        {
            var channels = player.GetChannels();
            return NetWCFConverter.ToNET(channels);
        }
        public override TV2Lib.Channel GetCurrentTVChannel()
        {
            return NetWCFConverter.ToNET(player.GetCurrentTVChannel(player.GetPrimaryDisplay().DeviceID));
        }
        public override void SetCurrentTVChannel(TV2Lib.Channel channel)
        {
            player.SetChannel(player.GetPrimaryDisplay().DeviceID, NetWCFConverter.ToWCF(channel));
        }
        public override TunerDevice GetTunerDevice()
        {
            return player.GetTunerDevice(player.GetPrimaryDisplay().DeviceID);
        }
        public override IEnumerable<TunerDevice> GetTunerDevices()
        {
            return player.GetTunerDevices().ToList();
        }
        public override void SetTunerDevice(TunerDevice dev)
        {
            player.DefineTunerDevice(player.GetPrimaryDisplay().DeviceID, dev);
        }
        #endregion

        #region Markee
        public override IEnumerable<string> GetFooterText()
        {
            throw new NotImplementedException();
        }
        public override void AddMarkeeText(IEnumerable<string> textList)
        {
            throw new NotImplementedException();
        }
        public override void RemoveMarkeeText(IEnumerable<string> textList)
        {
            throw new NotImplementedException();
        }
        public override System.Drawing.Font GetMarkeeFont()
        {
            throw new NotImplementedException();
        }
        public override void SetMarkeeFont(System.Drawing.Font font)
        {
            throw new NotImplementedException();
        }
        public override System.Drawing.Color GetMarkeeColor()
        {
            throw new NotImplementedException();
        }
        public override void SetMarkeeColor(System.Drawing.Color color)
        {
            throw new NotImplementedException();
        }
        public override int GetMarkeeSpeed()
        {
            throw new NotImplementedException();
        }
        public override void SetMarkeeSpeed(int speed)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}