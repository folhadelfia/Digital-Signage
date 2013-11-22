/* 
    TODO:
 *     
 *    1. Adicionar na janela inicial (ListeningForm) um controlo com todos os monitores presentes no pc, o seu estado (se têm janela do Player ou não), e opções para mudar algumas opções
 *    
 *    2. Dar capacidade à aplicação cliente de definir permissões do lado do servidor (i.e. poder definir um monitor com um componente TV onde o servidor pode alterar o canal, e poder definir um onde não pode)
 *    
 *    3. Definir variáveis persistentes, como guardar o endereço do servidor de discovery e a sua porta
 
 
 */

#undef EXPOSE_METADATA

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Windows.Forms;
using Assemblies.Configurations;
using Assemblies.Toolkit;
using Assemblies.DataContracts;
using Assemblies.PlayerServiceImplementation;
using Transitions;
using System.Threading;
using TV2Lib;
using DirectShowLib;

namespace Server.View
{
    public partial class ListeningForm : Form
    {
        private ServiceHost serviceHost;

        private Dictionary<string, FormJanelaFinal> playerWindows = new Dictionary<string, FormJanelaFinal>();

        public ListeningForm()
        {
            InitializeComponent();

            PlayerService.OpenPlayerWindow += PlayerService_OpenPlayerWindow;
            PlayerService.OpenPlayerWindow2 += PlayerService_OpenPlayerWindow2;
            PlayerService.ClosePlayerWindow += PlayerService_ClosePlayerWindow;
            PlayerService.EditPlayerWindow += PlayerService_EditPlayerWindow;

            PlayerService.SendTunedChannel += PlayerService_SendTunedChannel;
            PlayerService.TuneToChannel += PlayerService_TuneToChannel;

            PlayerService.SendWindowIsOpen += PlayerService_SendWindowIsOpen;

            PlayerService.SendTunerDevice += PlayerService_SendTunerDevice;
            PlayerService.SendTunerDevices += PlayerService_SendTunerDevices;
            PlayerService.SetTunerDevice += PlayerService_SetTunerDevice;



            textBoxLocalIP.Text = NetworkingToolkit.LocalIPAddress;
        }

        void PlayerService_SetTunerDevice(string displayName, TunerDevice dev)
        {
            FormJanelaFinal window = playerWindows[displayName];

            if (window.Controls.OfType<DigitalTVScreen>().Count() == 1)
            {
                DigitalTVScreen temp = window.Controls.OfType<DigitalTVScreen>().ToList()[0];

                temp.Devices.TunerDevice = DigitalTVScreen.DeviceStuff.TunerDevices.Values.Single(x=>x.DevicePath == dev.DevicePath);
            }
        }

        void PlayerService_SendTunerDevices(out TunerDevice[] devs)
        {
            devs = DigitalTVScreen.DeviceStuff.TunerDevices.Values.Select(x => new TunerDevice() { DevicePath = x.DevicePath, Name = x.Name }).ToArray();
        }

        void PlayerService_SendTunerDevice(string displayName, out TunerDevice dev)
        {
            FormJanelaFinal window = playerWindows[displayName];

            dev = window.GetTunerDevice();
        }

        void PlayerService_SendWindowIsOpen(string displayName, out bool isOpen)
        {
            isOpen = playerWindows.Keys.Contains(displayName);
        }

        void PlayerService_SendTunedChannel(string displayName, out WCFChannel ch)
        {
            if (playerWindows.Keys.Contains(displayName)) ch = NetWCFConverter.ToWCF(playerWindows[displayName].GetChannel());
            else ch = null;
        }

        void PlayerService_TuneToChannel(string deviceName, WCFChannel ch)
        {
            FormJanelaFinal window = playerWindows[deviceName];

            window.SetChannel(NetWCFConverter.ToNET(ch));
        }

        void PlayerService_EditPlayerWindow(WCFPlayerWindowInformation config)
        {
            Player_EditPlayerWindow(NetWCFConverter.ToNET(config));
        }

        void PlayerService_ClosePlayerWindow(string deviceName)
        {
            try
            {
                playerWindows[deviceName].Close();
                playerWindows.Remove(deviceName);
            }
            catch
            {
            }
        }

        void PlayerService_OpenPlayerWindow(WCFPlayerWindowInformation config)
        {
            Player_OpenPlayerWindow(NetWCFConverter.ToNET(config));
        }
        void Player_OpenPlayerWindow(PlayerWindowInformation config)
        {
            if (playerWindows.Keys.Contains(config.Display.DeviceID)) return;

            FormJanelaFinal newWindow = new FormJanelaFinal(playerWindows, config.Display.DeviceID);

            newWindow.Show(config);

            playerWindows.Add(config.Display.DeviceID, newWindow);
        }


        void PlayerService_OpenPlayerWindow2(WCFPlayerWindowInformation2 config)
        {
            Player_OpenPlayerWindow2(NetWCFConverter.ToNET(config));
        }
        void Player_OpenPlayerWindow2(PlayerWindowInformation2 config)
        {
            foreach (var key in config.Configuration.Keys)
            {
                Player_OpenPlayerWindow(new PlayerWindowInformation
                                            {
                                                Components = config.Configuration[key],
                                                Display = key
                                            });
            }
        }

        void Player_EditPlayerWindow(PlayerWindowInformation config)
        {
            throw new NotImplementedException();
        }

        #region Close
        private void ListeningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.TryCloseService()) this.ForceCloseService();
        }
        private void ListeningForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!this.TryCloseService()) this.ForceCloseService();
        }

        /// <summary>
        /// Tries to close the service smoothly
        /// </summary>
        /// <returns></returns>
        public bool TryCloseService()
        {
            if (serviceHost != null && serviceHost.State != CommunicationState.Closed)
            {
                try
                {
                    serviceHost.Close();
                }
                catch
                {
                    return false;
                }
            }

            return serviceHost == null || serviceHost.State == CommunicationState.Closed;
        }
        /// <summary>
        /// Tries to close the service. If it fails, aborts it
        /// </summary>
        public void ForceCloseService()
        {
            if (serviceHost != null) try
                {
                    serviceHost.Close();
                }
                catch
                {
                    serviceHost.Abort();
                }
        }
        #endregion

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                (sender as Button).Enabled = false;

                if ((sender as Button).Text == "Ligar")
                {
                    try
                    {
                        #region Checks
                        string serverIP = textBoxServerIP.Text, serverPort = textBoxServerPort.Text, localPort = textBoxLocalPort.Text;
                        bool ready = true;

                        if (!NetworkingToolkit.ValidateIPAddress(serverIP))
                        {
                            ready = false;
                            Transition.run(textBoxServerIP, "BackColor", Color.MistyRose, new TransitionType_EaseInEaseOut(300));
                        }
                        else
                        {
                            Transition.run(textBoxServerIP, "BackColor", SystemColors.Window, new TransitionType_EaseInEaseOut(300));
                        }

                        if (!NetworkingToolkit.ValidatePort(serverPort))
                        {
                            ready = false;
                            Transition.run(textBoxServerPort, "BackColor", Color.MistyRose, new TransitionType_EaseInEaseOut(300));
                        }
                        else
                        {
                            Transition.run(textBoxServerPort, "BackColor", SystemColors.Window, new TransitionType_EaseInEaseOut(300));
                        }

                        if (!NetworkingToolkit.ValidatePort(localPort))
                        {
                            ready = false;
                            Transition.run(textBoxLocalPort, "BackColor", Color.MistyRose, new TransitionType_EaseInEaseOut(300));
                        }
                        else
                        {
                            Transition.run(textBoxLocalPort, "BackColor", SystemColors.Window, new TransitionType_EaseInEaseOut(300));
                        }
                        #endregion

                        if (ready)
                        {
                            this.Log("A iniciar o serviço");
                            StartService(serverIP, serverPort, localPort);
                        }
                        if (serviceHost.State == CommunicationState.Opened || serviceHost.State == CommunicationState.Opening)
                        {
                            this.Log(string.Format("Ligado ao servidor {1}:{2}{0}Endpoint: {3}", Environment.NewLine, serverIP, serverPort, serviceHost.Description.Endpoints[0].Address));
                            (sender as Button).Text = "Desligar";
                        }
                        else
                        {
                            this.Log(string.Format("Problema a ligar ao servidor {0}:{1}", serverIP, serverPort));
                        }
                    }
                    catch (ArgumentException ex)
                    {
#if DEBUG
                        this.Log(ex);
#endif
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        this.Log(ex);
#endif
                    }
                }
                else
                {
                    try
                    {
                        this.Log("A fechar o serviço");
                        serviceHost.Close();
                        this.Log("Serviço fechado com sucesso");
                    }
                    catch (Exception ex)
                    {
                        this.Log(ex);
                        this.Log("A abortar o serviço");
                        serviceHost.Abort();
                        this.Log("Serviço abortado");
                    }
                    if (serviceHost.State == CommunicationState.Closed || serviceHost.State == CommunicationState.Closing)
                        (sender as Button).Text = "Ligar";

                }
                
                (sender as Button).Enabled = true;
            });
        }
        private void buttonRandomize_Click(object sender, EventArgs e)
        {
            textBoxLocalPort.Text = NetworkingToolkit.RandomPort().ToString();
        }

        private void StartService(string serverIP, string serverPort, string localPort)
        {
            if (!(NetworkingToolkit.ValidateIPAddress(serverIP) && NetworkingToolkit.ValidatePort(serverPort) && NetworkingToolkit.ValidatePort(localPort)))
                throw new ArgumentException() { Source = "ListeningForm.StartService(string serverIP, string serverPort, string localPort)" }; 

            ////endereço do player
            Uri baseAddress = new Uri(String.Format("net.tcp://{0}:{1}/PlayerService/{2}", NetworkingToolkit.LocalIPAddress, localPort, Guid.NewGuid()));
            ////endpoint para onde as mensagens de announcement serão enviadas
            Uri announcementEndpointAddress = new Uri(String.Format("net.tcp://{0}:{1}/Announcement", serverIP, serverPort));

            //criar o host do serviço
            serviceHost = new ServiceHost(typeof(PlayerService), baseAddress);
            try
            {
                ////Adicionar um endpoint para o serviço
                //NetTcpBinding tcpBindingService = new NetTcpBinding();
                //tcpBindingService.Security.Mode = SecurityMode.None; //Alterar a autenticação para um modelo melhor
                ////tcpBindingService.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;

                //tcpBindingService.MaxReceivedMessageSize = 10000000;
                //tcpBindingService.MaxBufferSize = 10000000;
                //tcpBindingService.MaxBufferPoolSize = 10000000;

                //ServiceEndpoint netTcpEndpoint = serviceHost.AddServiceEndpoint(typeof(IPlayer), tcpBindingService, string.Empty);

                //Criar um endpoint para o announcement server, que aponta para o DiscoveryProxy
                NetTcpBinding tcpBindingAnnouncement = new NetTcpBinding();
                tcpBindingAnnouncement.Security.Mode = SecurityMode.None; //Alterar a autenticação para um modelo melhor

                ////http://nerdwords.blogspot.pt/2008/01/wcf-error-socket-connection-was-aborted.html

                AnnouncementEndpoint announcementEndpoint = new AnnouncementEndpoint(tcpBindingAnnouncement, new EndpointAddress(announcementEndpointAddress));

                //Criar um DiscoveryBehaviour e adicionar o endpoint
                ServiceDiscoveryBehavior serviceDiscoveryBehavior = new ServiceDiscoveryBehavior();
                serviceDiscoveryBehavior.AnnouncementEndpoints.Add(announcementEndpoint);

#if EXPOSE_METADATA

                //Adicionar um endpoint MEX (Metadata EXchange) por TCP
                System.ServiceModel.Channels.BindingElement bindingElement = new System.ServiceModel.Channels.TcpTransportBindingElement();
                System.ServiceModel.Channels.CustomBinding binding = new System.ServiceModel.Channels.CustomBinding(bindingElement);
                System.ServiceModel.Description.ServiceMetadataBehavior metadataBehavior = serviceHost.Description.Behaviors.Find<System.ServiceModel.Description.ServiceMetadataBehavior>();

                if (metadataBehavior == null)
                {
                    metadataBehavior = new System.ServiceModel.Description.ServiceMetadataBehavior();
                    serviceHost.Description.Behaviors.Add(metadataBehavior);
                }

                serviceHost.AddServiceEndpoint(typeof(System.ServiceModel.Description.IMetadataExchange), binding, "MEX");
#endif
                //Adicionar o serviceDiscoveryBehavior ao host para poder ser descoberto
                serviceHost.Description.Behaviors.Add(serviceDiscoveryBehavior);

                serviceHost.Open();


                Clipboard.SetText(baseAddress.ToString());
            }
            catch (CommunicationException e)
            {
                Log(e);
            }
            catch (TimeoutException e)
            {
                Log(e);
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        void serviceHost_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            Log("Mensagem desconhecida recebida");
            Log(e.Message.ToString());
        }
        void serviceHost_RefreshState(object sender, EventArgs e)
        {
            this.RefreshState();
        }

        private void RefreshState()
        {
            this.BeginInvoke((MethodInvoker)delegate
                {
                    try
                    {
                        switch (serviceHost.State)
                        {
                            case CommunicationState.Closed: labelEstado.ForeColor = Color.Red;
                                break;
                            case CommunicationState.Closing: labelEstado.ForeColor = Color.Goldenrod;
                                break;
                            case CommunicationState.Opening: labelEstado.ForeColor = Color.Goldenrod;
                                break;
                            case CommunicationState.Opened: labelEstado.ForeColor = Color.ForestGreen;
                                break;
                            case CommunicationState.Faulted: labelEstado.ForeColor = Color.Red;
                                break;
                            case CommunicationState.Created: labelEstado.ForeColor = Color.Blue;
                                break;
                        }

                        labelEstado.Text = serviceHost.State.ToString();
                    }
                    catch (Exception ex)
                    {
                        labelEstado.Text = "Erro";
                        labelEstado.ForeColor = Color.Red;

                        if (serviceHost != null) Log(ex);
                    }

                });
        }

        private void Log(string message)
        {
            RefreshState();

            if(this.InvokeRequired)
                this.BeginInvoke((MethodInvoker) delegate
                {
                    textBoxLog.AppendText(string.Format("[{0}] {1}{2}", DateTime.Now.ToString("HH:mm:ss"), message, Environment.NewLine));
                });
            else
                textBoxLog.AppendText(string.Format("[{0}] {1}{2}", DateTime.Now.ToString("HH:mm:ss"), message, Environment.NewLine));
        }
        private void Log(Exception ex)
        {
            Log(string.Format("EXCEPTION: {1}{0}MESSAGE: {2}{0}INNER EXCEPTION: {3}{0}INNER EXCEPTION MESSAGE: {4}", Environment.NewLine, ex.GetType().ToString(), ex.Message, ex.InnerException == null ? "null" : ex.InnerException.GetType().ToString(), ex.InnerException == null ? "null" : ex.InnerException.Message));
        }
    }
}
