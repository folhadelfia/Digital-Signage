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
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None) { ReceiveTimeout = new TimeSpan(0, 5, 0), SendTimeout = new TimeSpan(0, 5, 0) };
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

                float totalPcs = findResponse.Endpoints.Count;
                float currentPc = 1;

                foreach (var endpoint in findResponse.Endpoints)
                {
                    int progress = Convert.ToInt32(Math.Round((currentPc / totalPcs) * 100f, 0, MidpointRounding.AwayFromZero));
                    currentPc++;

                    try
                    {

                        NetTcpBinding bindingPC = new NetTcpBinding();
                        binding.Security.Mode = SecurityMode.None;
                        binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;
                        binding.CloseTimeout = new TimeSpan(0, 0, 2);

                        PlayerProxy client = new PlayerProxy(binding, endpoint.Address);

                        client.Open();

                        List<WCFScreenInformation> displays = client.GetDisplayInformation().ToList<WCFScreenInformation>();

                        client.Close();

                        pc = new WCFPlayerPC() { Displays = displays, Endpoint = endpoint.Address };

                        pcs.Add(pc);

                        OnGetPlayersProgressChanged(progress, Convert.ToInt32(totalPcs));
                    }
                    catch (EndpointNotFoundException)
                    {
                        OnGetPlayersProgressChanged(progress, Convert.ToInt32(totalPcs));
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
        public override void GetPlayersAsync(NewPlayerFoundDelegate callback)
        {
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

                float totalPcs = findResponse.Endpoints.Count;
                float currentPc = 1;

                List<Thread> threads = new List<Thread>();

                foreach (var endpoint in findResponse.Endpoints)
                {
                    Thread t = new Thread(new ThreadStart(() =>
                    {
                        int progress = Convert.ToInt32(Math.Round((currentPc / totalPcs) * 100f, 0, MidpointRounding.AwayFromZero));
                        currentPc++;

                        try
                        {

                            NetTcpBinding bindingPC = new NetTcpBinding();
                            binding.Security.Mode = SecurityMode.None;
                            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;
                            binding.CloseTimeout = new TimeSpan(0, 0, 2);

                            PlayerProxy client = new PlayerProxy(binding, endpoint.Address);

                            client.Open();

                            List<WCFScreenInformation> displays = client.GetDisplayInformation().ToList<WCFScreenInformation>();

                            client.Close();

                            pc = new WCFPlayerPC() { Displays = displays, Endpoint = endpoint.Address };

                            callback(pc);

                            OnGetPlayersProgressChanged(progress, Convert.ToInt32(totalPcs));
                        }
                        catch (EndpointNotFoundException)
                        {
                            OnGetPlayersProgressChanged(progress, Convert.ToInt32(totalPcs));
                        }
                        catch (TimeoutException)
                        {
                            OnGetPlayersProgressChanged(progress, Convert.ToInt32(totalPcs));
                        }
                    }));

                    threads.Add(t);
                }

                foreach (var t in threads)
                {
                    t.Start();
                }
            }
            catch
            {
                return;
            }

        }
        public override event GetPlayersEventHandler GetPlayersProgressChanged;
        private void OnGetPlayersProgressChanged(int progress, int count)
        {
            if (this.GetPlayersProgressChanged != null) this.GetPlayersProgressChanged(this, new GetPlayersEventArgs()
             {
                 PlayerCount = count,
                 Progress = progress
             });
        }

        public override void Dispose()
        {
            try
            {
                player.Close();
            }
            catch
            {
                if (player.State != CommunicationState.Closed)

                    try
                    {
                        player.Abort();
                    }
                    catch
                    {
                    }
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
        }
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
        /// <summary>
        /// Enumera os canais disponíveis no player, na frequência padrão (754000 khz).
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TV2Lib.Channel> GetTVChannels()
        {
            var channels = player.GetChannels();
            return NetWCFConverter.ToNET(channels);
        }
        /// <summary>
        /// Enumera os canais disponíveis no player, na frequência padrão (754000 khz). Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        public override IEnumerable<TV2Lib.Channel> GetTVChannels(bool forceRescan)
        {
            var channels = player.GetChannels(forceRescan);
            return NetWCFConverter.ToNET(channels);
        }
        /// <summary>
        /// Enumera os canais disponíveis no player, na <paramref name="frequency"/> dada.
        /// </summary>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <returns></returns>
        public override IEnumerable<TV2Lib.Channel> GetTVChannels(int frequency)
        {
            var channels = player.GetChannels(frequency);
            return NetWCFConverter.ToNET(channels);
        }
        /// <summary>
        /// Enumera os canais disponíveis no player, na <paramref name="frequency"/> dada. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        public override IEnumerable<TV2Lib.Channel> GetTVChannels(int frequency, bool forceRescan)
        {
            var channels = player.GetChannels(frequency, forceRescan);
            return NetWCFConverter.ToNET(channels);
        }
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz.
        /// </summary>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <returns></returns>
        public override IEnumerable<TV2Lib.Channel> GetTVChannels(int minFrequency, int maxFrequency, int step)
        {
            var channels = player.GetChannels(minFrequency, maxFrequency, step);
            return NetWCFConverter.ToNET(channels);
        }
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        public override IEnumerable<TV2Lib.Channel> GetTVChannels(int minFrequency, int maxFrequency, int step, bool forceRescan)
        {
            var channels = player.GetChannels(minFrequency, maxFrequency, step, forceRescan);
            return NetWCFConverter.ToNET(channels);
        }
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na frequência padrão (754000 khz).
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public override IEnumerable<TV2Lib.Channel> GetTVChannels(string device)
        {
            var channels = player.GetChannels(device);
            return NetWCFConverter.ToNET(channels);
        }
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na frequência padrão (754000 khz). Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public override IEnumerable<TV2Lib.Channel> GetTVChannels(string device, bool forceRescan)
        {
            var channels = player.GetChannels(device, forceRescan);
            return NetWCFConverter.ToNET(channels);
        }
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na <paramref name="frequency"/> dada.
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public override IEnumerable<TV2Lib.Channel> GetTVChannels(string device, int frequency)
        {
            var channels = player.GetChannels(device, frequency);
            return NetWCFConverter.ToNET(channels);
        }
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na <paramref name="frequency"/> dada. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public override IEnumerable<TV2Lib.Channel> GetTVChannels(string device, int frequency, bool forceRescan)
        {
            var channels = player.GetChannels(device, frequency, forceRescan);
            return NetWCFConverter.ToNET(channels);
        }
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, no player com o DeviceID <paramref name="device"/>, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz.
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public override IEnumerable<TV2Lib.Channel> GetTVChannels(string device, int minFrequency, int maxFrequency, int step)
        {
            var channels = player.GetChannels(device, minFrequency, maxFrequency, step);
            return NetWCFConverter.ToNET(channels);
        }
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, no player com o DeviceID <paramref name="device"/>, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public override IEnumerable<TV2Lib.Channel> GetTVChannels(string device, int minFrequency, int maxFrequency, int step, bool forceRescan)
        {
            var channels = player.GetChannels(device, minFrequency, maxFrequency, step, forceRescan);
            return NetWCFConverter.ToNET(channels);
        }

        public override TV2Lib.Channel GetCurrentTVChannel(string displayName)
        {
            return NetWCFConverter.ToNET(player.GetCurrentTVChannel(displayName));
        }
        public override void SetCurrentTVChannel(string displayName, TV2Lib.Channel channel)
        {
            player.SetChannel(displayName, NetWCFConverter.ToWCF(channel));
        }
        public override GeneralDevice GetTunerDevice(string displayName)
        {
            return player.GetTunerDevice(displayName);
        }
        public override IEnumerable<GeneralDevice> GetTunerDevices()
        {
            return player.GetTunerDevices().ToList();
        }
        public override IEnumerable<GeneralDevice> GetTunerDevicesInUse()
        {
            return player.GetTunerDevicesInUse().ToList();
        }
        public override void SetTunerDevice(string displayName, GeneralDevice dev)
        {
            player.DefineTunerDevice(displayName, dev);
        }

        public override GeneralDevice GetAudioDecoder(string displayName)
        {
            return player.GetAudioDecoder(displayName);
        }
        public override IEnumerable<GeneralDevice> GetAudioDecoders()
        {
            return player.GetAudioDecoders();
        }
        public override GeneralDevice GetAudioRenderer(string displayName)
        {
            return player.GetAudioRenderer(displayName);
        }
        public override IEnumerable<GeneralDevice> GetAudioRenderers()
        {
            return player.GetAudioRenderers();
        }

        public override GeneralDevice GetH264Decoder(string displayName)
        {
            return player.GetH264Decoder(displayName);
        }
        public override IEnumerable<GeneralDevice> GetH264Decoders()
        {
            return player.GetH264Decoders();
        }
        public override GeneralDevice GetMPEG2Decoder(string displayName)
        {
            return player.GetMPEG2Decoder(displayName);
        }
        public override IEnumerable<GeneralDevice> GetMPEG2Decoders()
        {
            return player.GetMPEG2Decoders();
        }
        public override void SetAudioDecoder(string displayName, GeneralDevice dev)
        {
            player.DefineAudioDecoder(displayName, dev);
        }
        public override void SetAudioRenderer(string displayName, GeneralDevice dev)
        {
            player.DefineAudioRenderer(displayName, dev);
        }
        public override void SetH264Codec(string displayName, GeneralDevice dev)
        {
            player.DefineH264Decoder(displayName, dev);
        }
        public override void SetMPEG2Codec(string displayName, GeneralDevice dev)
        {
            player.DefineMPEG2Decoder(displayName, dev);
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