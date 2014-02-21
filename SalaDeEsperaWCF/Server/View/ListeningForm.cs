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
using Transitions;
using System.Threading;
using TV2Lib;
using DirectShowLib;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.ComponentModel;

using Assemblies.Linq;
using Assemblies.Configurations;
using Assemblies.Toolkit;
using Assemblies.DataContracts;
using Assemblies.PlayerServiceImplementation;
using Assemblies.PlayerServiceContracts;

using Assemblies.ExtensionMethods;

namespace Server.View
{
    public partial class ListeningForm : Form
    {
        private const string OBTAININGADDRESSES_TEXTBOX_TEXT = "A obter...",
                             UNAVAILABE_TEXTBOX_TEXT = "Indisponível. Verifique a ligação à Internet e clique em Actualizar.",
                             HOSTNAME_RESOLVE_FAILURE = "<none>";

        private string VideoFolderPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "DigitalSignage"); } }

        private ServiceHost serviceHost, fileTransferHost;

        private Dictionary<string, FormJanelaFinal> playerWindows = new Dictionary<string, FormJanelaFinal>();

        string publicIP = "",
               publicHostname = "",

               privateIP = "",
               privateHostname = "";

        int idClinicaMulti = 0;

        public ListeningForm()
        {
            InitializeComponent();

            #region Eventos do serviço

            PlayerService.OpenPlayerWindow += PlayerService_OpenPlayerWindow;
            PlayerService.OpenPlayerWindow2 += PlayerService_OpenPlayerWindow2;
            PlayerService.EditPlayerWindow += PlayerService_EditPlayerWindow;
            PlayerService.ClosePlayerWindow += PlayerService_ClosePlayerWindow;

            PlayerService.SendTunedChannel += PlayerService_SendTunedChannel;
            PlayerService.TuneToChannel += PlayerService_TuneToChannel;

            PlayerService.SendWindowIsOpen += PlayerService_SendWindowIsOpen;

            PlayerService.SendTunerDevice += PlayerService_SendTunerDevice;
            PlayerService.SendTunerDevices += PlayerService_SendTunerDevices;
            PlayerService.SendTunerDevicesInUse += PlayerService_SendTunerDevicesInUse;
            PlayerService.SetTunerDevice += PlayerService_SetTunerDevice;


            PlayerService.SendAudioDecoder += PlayerService_SendAudioDecoder;
            PlayerService.SendAudioRenderer += PlayerService_SendAudioRenderer;
            PlayerService.SendH264Decoder += PlayerService_SendH264Decoder;
            PlayerService.SendMPEG2Decoder += PlayerService_SendMPEG2Decoder;

            PlayerService.SetAudioDecoder += PlayerService_SetAudioDecoder;
            PlayerService.SetAudioRenderer += PlayerService_SetAudioRenderer;
            PlayerService.SetH264Decoder += PlayerService_SetH264Decoder;
            PlayerService.SetMPEG2Decoder += PlayerService_SetMPEG2Decoder;

            PlayerService.SendAudioDecoders += PlayerService_SendAudioDecoders;
            PlayerService.SendAudioRenderers += PlayerService_SendAudioRenderers;
            PlayerService.SendH264Decoders += PlayerService_SendH264Decoders;
            PlayerService.SendMPEG2Decoders += PlayerService_SendMPEG2Decoders;

            #region Enviar ficheiros

            FileStreamingService.FileReceived += FileStreamingService_FileReceived;
            FileStreamingService.FileStreamProgressReceived += FileStreamingService_FileStreamProgressReceived;

            #endregion

            PlayerService.SendVideoFilePaths += PlayerService_SendVideoFilePaths;

            PlayerService.StartVideo += PlayerService_StartVideo;
            PlayerService.StopVideo += PlayerService_StopVideo;
            PlayerService.PreviousVideo += PlayerService_PreviousVideo;
            PlayerService.NextVideo += PlayerService_NextVideo;
            #endregion
        }

        void FileStreamingService_FileStreamProgressReceived(object sender, FileStreamProgressReceivedEventArgs e)
        {
            this.ReceiveFileStreamProgress(e.Request);

            e.Request.Dispose();
        }

        private void ReceiveFileStreamProgress(RemoteFileInfo request)
        {
            try
            {
                if (!Directory.Exists(VideoFolderPath)) Directory.CreateDirectory(VideoFolderPath);

                string filePath = Path.Combine(VideoFolderPath, request.FileName);
                if (File.Exists(filePath)) File.Delete(filePath);

                int chunkSize = 2 * 1024;
                byte[] buffer = new byte[chunkSize];

                using (FileStream writeStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
                {
                    try
                    {
                        request.FileByteStream.CopyStream(writeStream);

                        writeStream.Close();
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message, "IOException", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        writeStream.Close();
                    }
                }
            }
            catch
            {
            }
        }

        private void ListeningForm_Load(object sender, EventArgs e)
        {
            buttonConnect.Enabled = false;

            this.RefreshIPAndHostnameTextboxes();

            try
            {
                this.GetPortValues();
            }
            catch
            {
                this.GeneratePortValues();
            }
            
            using (var db = new ClinicaDataContext(LinqConnectionStrings.LigacaoClinica)) idClinicaMulti = db.ClinicaDados.Single().idClinicaMulti ?? -1;
        }

        private void GetPortValues()
        {
            using (var db = new PlayersLigadosDataContext(LinqConnectionStrings.LigacaoPlayersLigados))
            {
                string m_publicIP = "";
                if (!string.IsNullOrWhiteSpace(textBoxPublicIP.Text) && textBoxPublicIP.Text != OBTAININGADDRESSES_TEXTBOX_TEXT && textBoxPublicIP.Text != UNAVAILABE_TEXTBOX_TEXT)
                    m_publicIP = textBoxPublicIP.Text;
                else m_publicIP = MyToolkit.Networking.PublicIPAddress.ToString();

                var mostRecentPlayer = db.Players.Where(x => x.privateIPAddress == MyToolkit.Networking.PrivateIPAddress.ToString() &&
                                                           x.publicIPAddress == m_publicIP).OrderByDescending(x => x.ID).First();
                
                textBoxPrivatePortFT.Text = mostRecentPlayer.Endpoints.Single(x => x.Type == (int)EndpointTypeEnum.FileTransfer).PrivatePort;
                textBoxPrivatePortPL.Text = mostRecentPlayer.Endpoints.Single(x => x.Type == (int)EndpointTypeEnum.Player).PrivatePort;

                textBoxPublicPortFT.Text = mostRecentPlayer.Endpoints.Single(x => x.Type == (int)EndpointTypeEnum.FileTransfer).PublicPort;
                textBoxPublicPortPL.Text = mostRecentPlayer.Endpoints.Single(x => x.Type == (int)EndpointTypeEnum.Player).PublicPort;
            }
        }
        private void GeneratePortValues()
        {
            this.textBoxPrivatePortPL.Text = MyToolkit.Networking.RandomPort().ToString();
            this.textBoxPrivatePortFT.Text = MyToolkit.Networking.RandomPort(new string[] { textBoxPrivatePortPL.Text });

            this.textBoxPublicPortPL.Text = this.textBoxPrivatePortPL.Text;
            this.textBoxPublicPortFT.Text = this.textBoxPrivatePortFT.Text;
        }

        #region Preenchimento dos ips e hostnames, actualização dos estados dos botões

        private void buttonRefreshPrivate_Click(object sender, EventArgs e)
        {
            buttonConnect.Enabled = false;

            this.RefreshPrivateIPAndHostnameTextboxes();
        }

        private void buttonRefreshPublic_Click(object sender, EventArgs e)
        {
            buttonConnect.Enabled = false;
            this.RefreshPublicIPAndHostnameTextboxes();
        }

        #region RefreshPrivateIPAndHostnameTextboxes

        private void RefreshPrivateIPAndHostnameTextboxes()
        {
            textBoxPrivateIP.Text = textBoxPrivateHostname.Text = OBTAININGADDRESSES_TEXTBOX_TEXT;

            BackgroundWorker workerGetPrivateIP = new BackgroundWorker(),
                             workerGetPrivateHostname = new BackgroundWorker();


            workerGetPrivateIP.DoWork += workerGetPrivateIP_DoWork;
            workerGetPrivateIP.RunWorkerCompleted += workerGetPrivateIP_RunWorkerCompleted;

            workerGetPrivateHostname.DoWork += workerGetPrivateHostname_DoWork;
            workerGetPrivateHostname.RunWorkerCompleted += workerGetPrivateHostname_RunWorkerCompleted;

            buttonRefreshPrivate.Enabled = false;

            workerGetPrivateIP.RunWorkerAsync();
            workerGetPrivateHostname.RunWorkerAsync();
        }

        void workerGetPrivateHostname_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                privateHostname = MyToolkit.Networking.PrivateHostname;
            }
            catch
            {
                privateHostname = UNAVAILABE_TEXTBOX_TEXT;
            }
        }
        void workerGetPrivateHostname_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.InvokeRequired) this.Invoke((MethodInvoker)(() => { textBoxPrivateHostname.Text = privateHostname; }));
            else textBoxPrivateHostname.Text = privateHostname;

            UpdateButtonStatus();
            RemoveSelectionFromTextboxes();
        }

        void workerGetPrivateIP_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                privateIP = MyToolkit.Networking.PrivateIPAddress.ToString();
            }
            catch
            {
                privateIP = UNAVAILABE_TEXTBOX_TEXT;
            }
        }
        void workerGetPrivateIP_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.InvokeRequired) this.Invoke((MethodInvoker)(() => { textBoxPrivateIP.Text = privateIP; }));
            else textBoxPrivateIP.Text = privateIP;

            UpdateButtonStatus();
            RemoveSelectionFromTextboxes();
        }

        #endregion

        #region RefreshPublicIPAndHostnameTextboxes

        private void RefreshPublicIPAndHostnameTextboxes()
        {
            textBoxPublicIP.Text = textBoxPublicHostname.Text = OBTAININGADDRESSES_TEXTBOX_TEXT;

            BackgroundWorker workerGetPublicIP = new BackgroundWorker(),
                             workerGetPublicHostname = new BackgroundWorker();

            workerGetPublicIP.DoWork += workerGetPublicIP_DoWork;
            workerGetPublicIP.RunWorkerCompleted += workerGetPublicIP_RunWorkerCompleted;

            workerGetPublicHostname.DoWork += workerGetPublicHostname_DoWork;
            workerGetPublicHostname.RunWorkerCompleted += workerGetPublicHostname_RunWorkerCompleted;

            buttonRefreshPublic.Enabled = false;

            workerGetPublicIP.RunWorkerAsync();
            workerGetPublicHostname.RunWorkerAsync();
        }

        void workerGetPublicHostname_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //string tempHostname = "";

                //using (var db = new ClinicaDataContext(LinqConnectionStrings.LigacaoClinica))
                //{
                //    tempHostname = db.ClinicaDados.Single().DNS;
                //}

                //if (string.IsNullOrWhiteSpace(tempHostname))
                //    publicHostname = MyToolkit.Networking.PublicHostname;
                //else publicHostname = tempHostname;

                string tempHostname = "";

                tempHostname = MyToolkit.Networking.PublicHostname;

                if (string.IsNullOrWhiteSpace(tempHostname))
                {
                    //using (var db = new ClinicaDataContext(LinqConnectionStrings.LigacaoClinica))
                    //    tempHostname = db.ClinicaDados.Single().DNS;

                    tempHostname = HOSTNAME_RESOLVE_FAILURE;
                }

                publicHostname = tempHostname;
            }
            catch
            {
                publicHostname = UNAVAILABE_TEXTBOX_TEXT;
            }
        }
        void workerGetPublicHostname_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.InvokeRequired) this.Invoke((MethodInvoker)(() => { textBoxPublicHostname.Text = publicHostname; }));
            else textBoxPublicHostname.Text = publicHostname;

            UpdateButtonStatus();
            RemoveSelectionFromTextboxes();
        }

        void workerGetPublicIP_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                publicIP = MyToolkit.Networking.PublicIPAddress.ToString();
            }
            catch
            {
                publicIP = UNAVAILABE_TEXTBOX_TEXT;
            }
        }
        void workerGetPublicIP_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.InvokeRequired) this.Invoke((MethodInvoker)(() => { textBoxPublicIP.Text = publicIP; }));
            else textBoxPublicIP.Text = publicIP;

            UpdateButtonStatus();
            RemoveSelectionFromTextboxes();
        }

        #endregion

        private void RefreshIPAndHostnameTextboxes()
        {
            RefreshPublicIPAndHostnameTextboxes();
            RefreshPrivateIPAndHostnameTextboxes();
        }

        private void UpdateButtonStatus()
        {

            bool privateReady = textBoxPrivateIP.Text != OBTAININGADDRESSES_TEXTBOX_TEXT &&
                                textBoxPrivateHostname.Text != OBTAININGADDRESSES_TEXTBOX_TEXT &&

                                textBoxPrivateIP.Text != UNAVAILABE_TEXTBOX_TEXT &&
                                textBoxPrivateHostname.Text != UNAVAILABE_TEXTBOX_TEXT;

            bool publicReady = textBoxPublicIP.Text != OBTAININGADDRESSES_TEXTBOX_TEXT &&
                               textBoxPublicHostname.Text != OBTAININGADDRESSES_TEXTBOX_TEXT &&

                               textBoxPublicIP.Text != UNAVAILABE_TEXTBOX_TEXT &&
                               textBoxPublicHostname.Text != UNAVAILABE_TEXTBOX_TEXT;

            buttonRefreshPrivate.Enabled = !(textBoxPrivateIP.Text == OBTAININGADDRESSES_TEXTBOX_TEXT ||
                                           textBoxPrivateHostname.Text == OBTAININGADDRESSES_TEXTBOX_TEXT);

            buttonRefreshPublic.Enabled = !(textBoxPublicIP.Text == OBTAININGADDRESSES_TEXTBOX_TEXT ||
                                           textBoxPublicHostname.Text == OBTAININGADDRESSES_TEXTBOX_TEXT);

            buttonConnect.Enabled = privateReady && publicReady;
        }
        private void RemoveSelectionFromTextboxes()
        {
            textBoxPrivateIP.Select(textBoxPrivateIP.Text.Length, 0);
            textBoxPrivateHostname.Select(textBoxPrivateHostname.Text.Length, 0);
            textBoxPublicIP.Select(textBoxPublicIP.Text.Length, 0);
            textBoxPublicHostname.Select(textBoxPublicHostname.Text.Length, 0);
        }

        #endregion

        #region Eventos do serviço

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
        void PlayerService_EditPlayerWindow(WCFPlayerWindowInformation config)
        {
            Player_EditPlayerWindow(NetWCFConverter.ToNET(config));
        }
        void Player_EditPlayerWindow(PlayerWindowInformation config)
        {
            throw new NotImplementedException();
        } //NOT IMPLEMENTED
        void PlayerService_ClosePlayerWindow(string displayName)
        {
            try
            {
                playerWindows[displayName].Close();
                playerWindows.Remove(displayName);
            }
            catch
            {
            }
        }

        void PlayerService_SendTunedChannel(string displayName, out WCFChannel ch)
        {
            if (playerWindows.ContainsKey(displayName)) ch = NetWCFConverter.ToWCF(playerWindows[displayName].GetChannel());
            else ch = null;
        }
        void PlayerService_TuneToChannel(string displayName, WCFChannel ch)
        {
            if (playerWindows.ContainsKey(displayName)) playerWindows[displayName].SetChannel(NetWCFConverter.ToNET(ch));
        }

        void PlayerService_SendWindowIsOpen(string displayName, out bool isOpen)
        {
            isOpen = playerWindows.Keys.Contains(displayName);
        }

        void PlayerService_SendTunerDevice(string displayName, out Assemblies.DataContracts.GeneralDevice dev)
        {
            if (playerWindows.ContainsKey(displayName)) dev = playerWindows[displayName].GetTunerDevice();
            else dev = null;
        }
        void PlayerService_SendTunerDevices(out Assemblies.DataContracts.GeneralDevice[] devs)
        {
            devs = DigitalTVScreen.DeviceStuff.TunerDevices.Values.Select(x => new GeneralDevice() { DevicePath = x.DevicePath, Name = x.Name }).ToArray();
        }
        void PlayerService_SendTunerDevicesInUse(out Assemblies.DataContracts.GeneralDevice[] devs)
        {
            devs = DigitalTVScreen.DeviceStuff.TunerDevicesInUse.Values.Select(x => new GeneralDevice { DevicePath = x.DevicePath, Name = x.Name }).ToArray();
        }
        void PlayerService_SetTunerDevice(string displayName, Assemblies.DataContracts.GeneralDevice dev)
        {
            if (playerWindows.ContainsKey(displayName)) playerWindows[displayName].SetTunerDevice(dev);
        }

        void PlayerService_SendAudioDecoder(string displayName, out Assemblies.DataContracts.GeneralDevice dev)
        {
            if (playerWindows.ContainsKey(displayName)) dev = playerWindows[displayName].GetAudioDecoder();
            else dev = null;
        }
        void PlayerService_SendAudioRenderer(string displayName, out Assemblies.DataContracts.GeneralDevice dev)
        {
            if (playerWindows.ContainsKey(displayName)) dev = playerWindows[displayName].GetAudioRenderer();
            else dev = null;
        }
        void PlayerService_SendH264Decoder(string displayName, out Assemblies.DataContracts.GeneralDevice dev)
        {
            if (playerWindows.ContainsKey(displayName)) dev = playerWindows[displayName].GetH264Decoder();
            else dev = null;
        }
        void PlayerService_SendMPEG2Decoder(string displayName, out Assemblies.DataContracts.GeneralDevice dev)
        {
            if (playerWindows.ContainsKey(displayName)) dev = playerWindows[displayName].GetMPEG2Decoder();
            else dev = null;
        }

        void PlayerService_SetAudioDecoder(string displayName, Assemblies.DataContracts.GeneralDevice dev)
        {
            if (playerWindows.ContainsKey(displayName)) playerWindows[displayName].SetAudioDecoder(dev);
        }
        void PlayerService_SetAudioRenderer(string displayName, Assemblies.DataContracts.GeneralDevice dev)
        {
            if (playerWindows.ContainsKey(displayName)) playerWindows[displayName].SetAudioRenderer(dev);
        }
        void PlayerService_SetH264Decoder(string displayName, Assemblies.DataContracts.GeneralDevice dev)
        {
            if (playerWindows.ContainsKey(displayName)) playerWindows[displayName].SetH264Decoder(dev);
        }
        void PlayerService_SetMPEG2Decoder(string displayName, Assemblies.DataContracts.GeneralDevice dev)
        {
            if (playerWindows.ContainsKey(displayName)) playerWindows[displayName].SetMPEG2Decoder(dev);
        }

        void PlayerService_SendAudioDecoders(out GeneralDevice[] devs)
        {
            devs = DigitalTVScreen.DeviceStuff.AudioDecoderDevices.Values.Select(x => new GeneralDevice() { DevicePath = x.DevicePath, Name = x.Name }).ToArray();
        }
        void PlayerService_SendAudioRenderers(out GeneralDevice[] devs)
        {
            devs = DigitalTVScreen.DeviceStuff.AudioRendererDevices.Values.Select(x => new GeneralDevice() { DevicePath = x.DevicePath, Name = x.Name }).ToArray();
        }
        void PlayerService_SendH264Decoders(out GeneralDevice[] devs)
        {
            devs = DigitalTVScreen.DeviceStuff.H264DecoderDevices.Values.Select(x => new GeneralDevice() { DevicePath = x.DevicePath, Name = x.Name }).ToArray();
        }
        void PlayerService_SendMPEG2Decoders(out GeneralDevice[] devs)
        {
            devs = DigitalTVScreen.DeviceStuff.MPEG2DecoderDevices.Values.Select(x => new GeneralDevice() { DevicePath = x.DevicePath, Name = x.Name }).ToArray();
        }


        #region Enviar ficheiros

        void FileStreamingService_FileReceived(object sender, FileReceivedEventArgs e)
        {
            string videoFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

            //DirectoryInfo videos = new DirectoryInfo(videoFolder);

            e.File.SaveToDisk(videoFolder);
        }

        #endregion

        #region Video

        void PlayerService_SendVideoFilePaths(out string[] paths)
        {
            DirectoryInfo videoFolder = new DirectoryInfo(this.VideoFolderPath);

            List<string> files = new List<string>();

            foreach (var file in videoFolder.EnumerateFiles().Where(x=>VideoPlayer.FileVideoPlayer.SupportedVideoExtensions().Contains(x.Extension.Trim().ToLower())))
            {
                files.Add(file.FullName);
            }

            paths = files.ToArray();
        }

        void PlayerService_StartVideo(string displayName, int videoPlayerID)
        {
            if (playerWindows.ContainsKey(displayName)) playerWindows[displayName].StartVideo(videoPlayerID);
        }
        void PlayerService_StopVideo(string displayName, int videoPlayerID)
        {
            if (playerWindows.ContainsKey(displayName)) playerWindows[displayName].StopVideo(videoPlayerID);
        }
        void PlayerService_PreviousVideo(string displayName, int videoPlayerID)
        {
            if (playerWindows.ContainsKey(displayName)) playerWindows[displayName].PreviousVideo(videoPlayerID);
        }
        void PlayerService_NextVideo(string displayName, int videoPlayerID)
        {
            if (playerWindows.ContainsKey(displayName)) playerWindows[displayName].NextVideo(videoPlayerID);
        }

        #endregion

        #endregion

        #region Close
        private void ListeningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.CloseServiceWithDatabase();
        }
        private void ListeningForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(serviceHost != null && serviceHost.State != CommunicationState.Closed) this.CloseServiceWithDatabase();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;

            if ((sender as Button).Text == "Ligar")
            {
                try
                {
                    #region Checks
                    string  publicIP = textBoxPublicIP.Text,
                            publicPortPlayer = textBoxPublicPortPL.Text,
                            publicPortFileTransfer = textBoxPublicPortFT.Text,
                            privateIP = textBoxPrivateIP.Text,
                            privatePortPlayer = textBoxPrivatePortPL.Text,
                            privatePortFileTransfer = textBoxPrivatePortFT.Text;

                    bool ready = true;

                    //var ips = MyToolkit.Networking.resolveHostname(serverIP);

                    //if (ips != null && ips.Length > 0)
                    //{
                    //    serverIP = ips[0].ToString();
                    //}

                    if (!MyToolkit.Networking.ValidateIPAddress(publicIP))
                    {
                        ready = false;
                        Transition.run(textBoxPublicIP, "BackColor", Color.MistyRose, new TransitionType_EaseInEaseOut(300));
                    }
                    else
                    {
                        Transition.run(textBoxPublicIP, "BackColor", SystemColors.Window, new TransitionType_EaseInEaseOut(300));
                    }

                    if (!MyToolkit.Networking.ValidatePort(publicPortPlayer))
                    {
                        ready = false;
                        Transition.run(textBoxPublicPortPL, "BackColor", Color.MistyRose, new TransitionType_EaseInEaseOut(300));
                    }
                    else
                    {
                        Transition.run(textBoxPublicPortPL, "BackColor", SystemColors.Window, new TransitionType_EaseInEaseOut(300));
                    }

                    if (!MyToolkit.Networking.ValidatePort(publicPortFileTransfer))
                    {
                        ready = false;
                        Transition.run(textBoxPublicPortFT, "BackColor", Color.MistyRose, new TransitionType_EaseInEaseOut(300));
                    }
                    else
                    {
                        Transition.run(textBoxPublicPortFT, "BackColor", SystemColors.Window, new TransitionType_EaseInEaseOut(300));
                    }

                    if (!MyToolkit.Networking.ValidatePort(privatePortPlayer))
                    {
                        ready = false;
                        Transition.run(textBoxPrivatePortPL, "BackColor", Color.MistyRose, new TransitionType_EaseInEaseOut(300));
                    }
                    else
                    {
                        Transition.run(textBoxPrivatePortPL, "BackColor", SystemColors.Window, new TransitionType_EaseInEaseOut(300));
                    }

                    if (!MyToolkit.Networking.ValidatePort(privatePortFileTransfer))
                    {
                        ready = false;
                        Transition.run(textBoxPrivatePortFT, "BackColor", Color.MistyRose, new TransitionType_EaseInEaseOut(300));
                    }
                    else
                    {
                        Transition.run(textBoxPrivatePortFT, "BackColor", SystemColors.Window, new TransitionType_EaseInEaseOut(300));
                    }
                    #endregion

                    if (ready)
                    {
                        this.Log("A iniciar o serviço");
                        //StartServiceWithDiscoveryServer(serverIP, serverPort, localPort);
                        StartServiceWithDatabase(
                            privatePortPlayer: privatePortPlayer,
                            privatePortFileTransfer: privatePortFileTransfer,
                            publicPortPlayer: publicPortPlayer,
                            publicPortFileTransfer: publicPortFileTransfer
                            );

                    }
                    if ((serviceHost != null && (serviceHost.State == CommunicationState.Opened || serviceHost.State == CommunicationState.Opening)) && (fileTransferHost != null && (fileTransferHost.State == CommunicationState.Opened || fileTransferHost.State == CommunicationState.Opening)))
                    {
                        this.Log(string.Format("Player Endpoint: {0}", serviceHost.Description.Endpoints[0].Address));
                        this.Log(string.Format("File Transfer Endpoint: {0}", fileTransferHost.Description.Endpoints[0].Address));
                        (sender as Button).Text = "Desligar";
                    }
                    else
                    {
                        //this.Log(string.Format("Problema a ligar ao servidor {0}:{1}", publicIP, publicPort));
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
                //CloseServiceWithDiscoveryServer();
                CloseServiceWithDatabase();

                if ((serviceHost.State == CommunicationState.Closed || serviceHost.State == CommunicationState.Closing) && (fileTransferHost.State == CommunicationState.Closed || fileTransferHost.State == CommunicationState.Closing))
                    (sender as Button).Text = "Ligar";

            }
                
            (sender as Button).Enabled = true;
        }

        private void StartServiceWithDiscoveryServer(string serverIP, string serverPort, string localPort)
        {
            //if (!(MyToolkit.Networking.ValidateIPAddress(serverIP) && MyToolkit.Networking.ValidatePort(serverPort) && MyToolkit.Networking.ValidatePort(localPort)))
            //    throw new ArgumentException() { Source = "ListeningForm.StartService(string serverIP, string serverPort, string localPort)" }; 

            ////endereço do player
            Uri baseAddress = new Uri(String.Format("net.tcp://{0}:{1}/PlayerService/{2}", MyToolkit.Networking.PrivateIPAddress, localPort, Guid.NewGuid()));
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
            catch (Exception e)
            {
                Log(e);
            }
        }
        private void StartServiceWithDatabase(string privatePortPlayer, string privatePortFileTransfer, string publicPortPlayer, string publicPortFileTransfer)
        {
            try
            {
                #region Endpoint do player
                ////endereço do player
                Uri baseAddress = new Uri(String.Format("net.tcp://{0}:{1}/PlayerService", textBoxPrivateIP.Text, privatePortPlayer));

                //criar o host do serviço
                serviceHost = new ServiceHost(typeof(PlayerService), baseAddress);
                //NetTcpBinding tcpBindingAnnouncement = new NetTcpBinding();
                //tcpBindingAnnouncement.Security.Mode = SecurityMode.None; //Alterar a autenticação para um modelo melhor
                #endregion

                #region Endpoint dos ficheiros

                fileTransferHost = FileStreamingService.CreateHost(textBoxPrivateIP.Text, privatePortFileTransfer);

                #endregion

                ////http://nerdwords.blogspot.pt/2008/01/wcf-error-socket-connection-was-aborted.html
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

                serviceHost.Open();
                fileTransferHost.Open();

                using (var db = new PlayersLigadosDataContext(Program.LigacaoPlayersLigados))
                {
                    var players = db.Players.Where(x => x.publicIPAddress == this.publicIP && x.privateIPAddress == this.privateIP);
                    
                    if (players.Count() > 0)
                    {
                        var pl = players.OrderByDescending(x => x.ID).First();

                        pl.privateHostname = this.privateHostname;
                        pl.publicHostname = this.publicHostname;

                        pl.Endpoints.Single(x => x.Type == (int)EndpointTypeEnum.Player).PublicPort = publicPortPlayer;
                        pl.Endpoints.Single(x => x.Type == (int)EndpointTypeEnum.Player).PrivatePort = privatePortPlayer;
                        pl.Endpoints.Single(x => x.Type == (int)EndpointTypeEnum.FileTransfer).PublicPort = publicPortFileTransfer;
                        pl.Endpoints.Single(x => x.Type == (int)EndpointTypeEnum.FileTransfer).PrivatePort = privatePortFileTransfer;

                        pl.isActive = true;
                    }
                    else
                    {
                        var pl = new Player()
                        {
                            isActive = true,
                            idClinica = idClinicaMulti,
                            privateIPAddress = privateIP,
                            publicIPAddress = publicIP,
                            privateHostname = privateHostname,
                            publicHostname = publicHostname
                        };

                        pl.Endpoints.Add(new Endpoint()
                        {
                            Type = (int)EndpointTypeEnum.Player,
                            Address = serviceHost.Description.Endpoints[0].Address.ToString(), //Procurar o endereço de uma maneira melhor
                            PrivatePort = privatePortPlayer,
                            PublicPort = publicPortPlayer,
                        });

                        pl.Endpoints.Add(new Endpoint()
                        {
                            Type = (int)EndpointTypeEnum.FileTransfer,
                            Address = fileTransferHost.Description.Endpoints[0].Address.ToString(), //Procurar o endereço de uma maneira melhor
                            PrivatePort = privatePortFileTransfer,
                            PublicPort = publicPortFileTransfer,
                        });


                        db.Players.InsertOnSubmit(pl);
                    }

                    db.SubmitChanges();
                }
#region

                //try
                //{
                //    using (var db = new PlayersLigadosDataContext(LinqConnectionStrings.LigacaoPlayersLigados))
                //    {
                //        var conflictingPlayers = db.Players.Where(x => x.publicIPAddress == this.publicIP && 
                //                                                           x.privateIPAddress == this.privateIP);

                //        if (conflictingPlayers.Count() > 1) //se existem muitos, limpar todos excepto o mais recente
                //        {
                //            foreach (var pl in conflictingPlayers.Where(x=>x.ID < conflictingPlayers.Max(y=>y.ID)))
                //            {
                //                foreach (var ep in pl.Endpoints)
                //                {
                //                    db.Endpoints.DeleteOnSubmit(ep);
                //                }

                //                db.Players.DeleteOnSubmit(pl);
                //            }

                //            db.SubmitChanges();
                //        }

                //        var player = db.Players.Single(x => x.ID == db.Players.Max(y => y.ID));



                //        //Encontrar os players com os mesmos dados deste mas que estão inactivos
                //        var tempOverlappingPlayersInactive = db.Players.Where(x =>
                //                                            !x.isActive &&
                //                                            x.idClinica == idClinicaMulti &&
                //                                            x.privateIPAddress == privateIP &&
                //                                            x.publicIPAddress == publicIP &&
                //                                            x.privateHostname == privateHostname &&
                //                                            x.publicHostname == publicHostname &&
                //                                            x.Endpoints.Where(y => 
                //                                                y.PrivatePort == privatePortPlayer &&
                //                                                y.PublicPort == publicPortPlayer
                //                                                ).Count() > 0
                //                                            ); //Ver bem esta condição

                //        //Apagar todos excepto o mais recente
                //        if (tempOverlappingPlayersInactive.Count() > 1)
                //        {
                //            foreach (var pl in tempOverlappingPlayersInactive)
                //            {
                //                if(pl.ID != tempOverlappingPlayersInactive.Max(x=>x.ID))
                //                    db.Players.DeleteOnSubmit(pl);
                //            }

                //            db.SubmitChanges();
                //        }

                //        //Se já existia um, activa-lo, senão adicionar
                //        if (tempOverlappingPlayersInactive.Count() == 1) tempOverlappingPlayersInactive.Single().isActive = true;
                //        else
                //        {
                //            var pl = new Player()
                //            {
                //                isActive = true,
                //                idClinica = idClinicaMulti,
                //                privateIPAddress = privateIP,
                //                publicIPAddress = publicIP,
                //                privateHostname = privateHostname,
                //                publicHostname = publicHostname
                //            };

                //            pl.Endpoints.Add(new Endpoint()
                //            {
                //                Type = (int)EndpointTypeEnum.Player,
                //                Address = serviceHost.Description.Endpoints[0].Address.ToString(), //Procurar o endereço de uma maneira melhor
                //                PrivatePort = privatePortPlayer,
                //                PublicPort = publicPortPlayer,
                //            });

                //            pl.Endpoints.Add(new Endpoint()
                //            {
                //                Type = (int)EndpointTypeEnum.FileTransfer,
                //                Address = fileTransferHost.Description.Endpoints[0].Address.ToString(), //Procurar o endereço de uma maneira melhor
                //                PrivatePort = privatePortFileTransfer,
                //                PublicPort = publicPortFileTransfer,
                //            });


                //            db.Players.InsertOnSubmit(pl);
                //        }

                //        db.SubmitChanges();
                //    }
                //}
                //catch
                //{
                //}

#endregion

            }
            catch (CommunicationException e)
            {
                Log(e);
            }
            catch (TimeoutException e)
            {
                Log(e);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }
        private void CloseServiceWithDiscoveryServer()
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
        }
        public void CloseServiceWithDatabase()
        {
            try
            {
                if (serviceHost == null && fileTransferHost == null) return;

                try
                {
                    this.Log("A fechar o player");
                    serviceHost.Close();
                    this.Log("Player fechado com sucesso");
                }
                catch(Exception ex)
                {
                    this.Log(ex);
                    this.Log("A abortar o player");
                    serviceHost.Abort();
                    this.Log("Player abortado");
                }
                try
                {
                    this.Log("A fechar a transferência de ficheiros");
                    fileTransferHost.Close();
                    this.Log("Transferência de ficheiros fechada com sucesso");
                }
                catch (Exception ex)
                {
                    this.Log(ex);
                    this.Log("A abortar a transferência de ficheiros");
                    fileTransferHost.Abort();
                    this.Log("Transferência de ficheiros abortada");
                }

                using (var db = new PlayersLigadosDataContext(LinqConnectionStrings.LigacaoPlayersLigados))
                {
                    string publicIPAddress = MyToolkit.Networking.PublicIPAddress.ToString();

                    string playerEndpoint = serviceHost.Description.Endpoints[0].Address.ToString(),
                           fileTransferEndpoint = fileTransferHost.Description.Endpoints[0].Address.ToString();

                    foreach (var pl in db.Players.Where(x => x.isActive && (x.Endpoints.Single(y => y.Type == (int)EndpointTypeEnum.Player).Address == playerEndpoint) && x.Endpoints.Single(y => y.Type == (int)EndpointTypeEnum.FileTransfer).Address == fileTransferEndpoint))
                        pl.isActive = false;

                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                this.Log(ex);
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
            try
            {
                this.BeginInvoke((MethodInvoker)delegate
                    {
                        try
                        {
                            switch (serviceHost.State)
                            {
                                case CommunicationState.Closed:
                                    labelEstado.Text = "Fechado";
                                    labelEstado.ForeColor = Color.Red;
                                    break;
                                case CommunicationState.Closing:
                                    labelEstado.Text = "A fechar";
                                    labelEstado.ForeColor = Color.Goldenrod;
                                    break;
                                case CommunicationState.Opening:
                                    labelEstado.Text = "A abrir";
                                    labelEstado.ForeColor = Color.Goldenrod;
                                    break;
                                case CommunicationState.Opened:
                                    labelEstado.Text = "Aberto";
                                    labelEstado.ForeColor = Color.ForestGreen;
                                    break;
                                case CommunicationState.Faulted:
                                    labelEstado.Text = "Falha";
                                    labelEstado.ForeColor = Color.Red;
                                    break;
                                case CommunicationState.Created:
                                    labelEstado.Text = "Pronto";
                                    labelEstado.ForeColor = Color.Blue;
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            labelEstado.Text = "Erro";
                            labelEstado.ForeColor = Color.Red;

                            if (serviceHost != null) Log(ex);
                        }

                    });
            }
            catch
            {
            }
        }

        private void Log(string message)
        {
            try
            {
                RefreshState();

                if (this.InvokeRequired)
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        textBoxLog.AppendText(string.Format("[{0}] {1}{2}", DateTime.Now.ToString("HH:mm:ss"), message, Environment.NewLine));
                    });
                else
                    textBoxLog.AppendText(string.Format("[{0}] {1}{2}", DateTime.Now.ToString("HH:mm:ss"), message, Environment.NewLine));
            }
            catch { }
        }
        private void Log(Exception ex)
        {
            Log(string.Format("EXCEPTION: {1}{0}MESSAGE: {2}{0}INNER EXCEPTION: {3}{0}INNER EXCEPTION MESSAGE: {4}", Environment.NewLine, ex.GetType().ToString(), ex.Message, ex.InnerException == null ? "null" : ex.InnerException.GetType().ToString(), ex.InnerException == null ? "null" : ex.InnerException.Message));
        }


    }
}
