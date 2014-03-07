using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Assemblies.Components;
using Assemblies.Configurations;
using Assemblies.Factories;
using Assemblies.PlayerComponents;

using Assemblies.DataContracts;

using Assemblies.Options;
using TV2Lib;

using DirectShowLib;
using System.Runtime.InteropServices;
using VideoPlayer;

namespace Server.View
{
    public partial class FormJanelaFinal : Form
    {
        #region Mover a janela sem barra de título

        private Point offset;
        private bool dragging;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    offset = e.Location;
                    dragging = true;
                }
            }
            catch(Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            try
            {
                if (dragging)
                {
                    Point posicaoRato = e.Location;

                    this.Location = new Point(this.Location.X - ( offset.X - posicaoRato.X), this.Location.Y - (offset.Y - posicaoRato.Y));
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            try
            {
                dragging = false;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }
        #endregion

        /// <summary>
        /// Contém uma referência para o controlo que abriu o contextmenustrip. É preciso por causa de um possível bug na framework, onde o SourceControl é null.
        /// </summary>
        Control menuSource = null;


        /// <summary>
        /// Contém o tamanho e a localização que foi dada quando a janela foi aberta. É preciso para poder reposicionar a janela correctamente no monitor dado pelo PlayerWindowInformation
        /// </summary>
        Rectangle shapeSetByPlayerWndInfo;

        string displayName = "";

        public string DisplayName
        {
            get { return displayName; }
            private set { if (value != "") displayName = value; else throw new NoNullAllowedException("The display name cannot be null. Ever.", null); }
        }
        Dictionary<string, FormJanelaFinal> listaSucatada;

        string idDaSucatada;


        public FormJanelaFinal(Dictionary<string, FormJanelaFinal> sucatada, string id)
        {
            InitializeComponent();

            listaSucatada = sucatada;
            idDaSucatada = id;
        }

        private void FormJanelaFinal_Load(object sender, EventArgs e)
        { 
            #region Mouse hide
            //mouseHideTimer.Interval = 5000;

            //mouseHideTimer.Tick += mouseHideTimer_Tick;
            //this.MouseMove += General_MouseMove;

            //mouseHideTimer.Start();
            #endregion
        }

        #region Mouse hide

        //private Timer mouseHideTimer = new Timer();

        //void General_MouseMove(object sender, MouseEventArgs e)
        //{
        //    Point lkmp = lastMousePos,
        //          cp = Cursor.Position;

        //    lkmp.Offset(0,0);
        //    cp.Offset(0,0);

        //    if (mouseHidden && lkmp != cp)
        //    {
        //        Cursor.Show();
        //        mouseHidden = false;
        //    }

        //    mouseHideTimer.Stop();
        //    mouseHideTimer.Start();
        //}


        //bool mouseHidden = false;
        //Point lastMousePos = new Point();
        //void mouseHideTimer_Tick(object sender, EventArgs e)
        //{
        //    //if (!mouseHidden)
        //    {
        //        Cursor.Hide();
        //        lastMousePos = Cursor.Position;
        //        mouseHidden = true;
        //    }
        //}
        #endregion

        private void reposicionarJanelaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Location = shapeSetByPlayerWndInfo.Location;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }
        private void FormJanelaFinal_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                foreach (Control component in Controls) //Mudar para todos os controlos e detectá-los
                {

                    if (component is DateTimeComposer)//MUDAR
                    {
                    }
                    else if (component is Markee)//MUDAR
                    {
                        (component as Markee).Stop();
                        (component as Markee).Dispose();
                    }
                    else if (component is ImageComposer)//MUDAR
                    {
                    }
                    else if (component is PriceListComposer)//MUDAR
                    {
                    }
                    else if (component is SlideShowComposer)//MUDAR
                    {
                    }
                    else if (component is DigitalTVScreen)
                    {
                        (component as DigitalTVScreen).Stop();
                        (component as DigitalTVScreen).Dispose();
                    }
                    else if (component is FileVideoPlayer)
                    {
                        FileVideoPlayer temp = component as FileVideoPlayer;

                        if (temp.State != FileVideoPlayer.MediaStatus.Stopped) temp.Stop();
                        temp.Dispose();
                    }
                    else if (component is WaitListComposer)//MUDAR
                    {
                    }
                    else if (component is WeatherComposer)//MUDAR
                    {
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }

            listaSucatada.Remove(idDaSucatada);
        }

        /// <summary>
        /// Actualiza o menustrip dos canais
        /// </summary>
        /// <param name="sender">O controlo que lançou o evento.</param>
        /// <param name="e">Contém uma lista com os novos canais</param>
        void tvDisplay_ChannelListChanged(object sender, ChannelEventArgs e)
        {
            canalTVTSMItem.DropDownItems.Clear();

            foreach (Channel ch in e.ChannelList)
            {
                ToolStripMenuItem aux = new ToolStripMenuItem() { Text = ch.Name, Tag = ch };
                aux.Click += channelToolStripMenuItem_Click;
                canalTVTSMItem.DropDownItems.Add(aux);
            }
        }
        void channelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (menuSource is DigitalTVScreen && (sender as ToolStripMenuItem).Tag is Channel)
                {
                    this.SetChannel((sender as ToolStripMenuItem).Tag as Channel);
                }
            }
            catch
            {
            }
        }
        private void contextMS_Opening(object sender, CancelEventArgs e)
        {
            menuSource = (sender as ContextMenuStrip).SourceControl;

            sempreNoTopoBackgroundTSMItem.Checked = this.TopMost;
            sempreNoTopoFooterTSMItem.Checked = this.TopMost;
            sempreNoTopoTVTSMItem.Checked = this.TopMost;
        }


        #region Específicos dos componentes
        #region Markees
        private void propriedadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool topMostAntigo = this.TopMost;

            try
            {
                this.TopMost = false;
                ToolStripItem menuItem = sender as ToolStripItem;
                if (menuItem != null)
                {
                    // Retrieve the ContextMenuStrip that owns this ToolStripItem
                    ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                    if (owner != null)
                    {
                        // Get the control that is displaying this context menu
                        ComposerComponent sourceControl = owner.SourceControl as ComposerComponent;

                        sourceControl.OpenOptionsWindow();
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("propriedadesToolStripMenuItem_Click" + Environment.NewLine + ex.Message);
#endif
            }

            this.TopMost = topMostAntigo;
        }


        #endregion

        #region TV

        private DigitalTVScreen CreateTVInstance(TVConfiguration config)
        {
            TV2Lib.Settings settings = new Settings()
            {
                Balance = 0,
                SnapshotsFolder = "Snapshots",
                StartVideoMode = VideoMode.Normal,
                TimeShiftingActivated = false,
                TimeShiftingBufferLengthMax = 180,
                TimeShiftingBufferLengthMin = 180,
                UseVideo169Mode = false,
                UseWPF = false,
                VideoBackgroundColor = System.Drawing.Color.Black,
                VideoBackgroundColorString = "Black",
                VideosFolder = "Recorder",
                Volume = 0
            };


            var res = new DigitalTVScreen()
            {
                BorderStyle = BorderStyle.None,
                CurrentGraphBuilder = null,
                Location = config.FinalLocation,
                MinimumSize = config.FinalSize,
                Settings = settings,
                Size = config.FinalSize,
                TabIndex = 0,
                UseBlackBands = true,
                VideoAspectRatio = 1D,
                VideoKeepAspectRatio = true,
                VideoOffset = new PointF(0f, 0f),
                VideoZoomMode = VideoSizeMode.FromInside
            };

            if (DigitalTVScreen.DeviceStuff.TunerDevices.ContainsKey(config.TunerDevicePath)) res.Devices.TunerDevice = DigitalTVScreen.DeviceStuff.TunerDevices[config.TunerDevicePath];
            //if (DigitalTVScreen.DeviceStuff.AudioDecoderDevices.ContainsKey(config.AudioDecoder)) res.Devices.AudioDecoder = DigitalTVScreen.DeviceStuff.AudioDecoderDevices[config.AudioDecoder];
            if (DigitalTVScreen.DeviceStuff.AudioRendererDevices.ContainsKey(config.AudioRenderer)) res.Devices.AudioRenderer = DigitalTVScreen.DeviceStuff.AudioRendererDevices[config.AudioRenderer];
            if (DigitalTVScreen.DeviceStuff.H264DecoderDevices.ContainsKey(config.H264Decoder)) res.Devices.H264Decoder = DigitalTVScreen.DeviceStuff.H264DecoderDevices[config.H264Decoder];
            if (DigitalTVScreen.DeviceStuff.MPEG2DecoderDevices.ContainsKey(config.MPEG2Decoder)) res.Devices.MPEG2Decoder = DigitalTVScreen.DeviceStuff.MPEG2DecoderDevices[config.MPEG2Decoder];

            return res;

        }


        #region Channels

        public Channel  GetChannel()
        {
            DigitalTVScreen temp = null;

            foreach (var control in this.Controls)
                if (control is DigitalTVScreen)
                {
                    temp = control as DigitalTVScreen;
                }
            return temp.Channels.CurrentChannel;
        }

        public void     SetChannel(Channel ch)
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return;

            //temp.Channels.TuneChannelAsync(ch, TuneChannelCallback);

            temp.Channels.TuneChannel(ch);
        }

        private void TuneChannelCallback(DigitalTVScreen screen, Channel ch, bool result)
        {
            if (this.InvokeRequired) this.Invoke((MethodInvoker)(() => { TuneChannelCallback(screen, ch, result); }));
            else
            {
                
            }
        }

        #endregion

        #region Tuner

        public GeneralDevice GetTunerDevice()
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return null;

            return new GeneralDevice() { Name = temp.Devices.TunerDevice.Name, DevicePath = temp.Devices.TunerDevice.DevicePath };
        }
        public void SetTunerDevice(GeneralDevice dev)
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return;

            temp.Devices.TunerDevice = DigitalTVScreen.DeviceStuff.TunerDevices[dev.DevicePath];
        }

        #endregion

        #region Codecs

        public GeneralDevice GetAudioDecoder()
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return null;

            return new GeneralDevice() { Name = temp.Devices.AudioDecoder.Name, DevicePath = temp.Devices.AudioDecoder.DevicePath };
        }
        public void SetAudioDecoder(GeneralDevice dev)
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return;

            temp.Devices.AudioDecoder = DigitalTVScreen.DeviceStuff.AudioDecoderDevices[dev.DevicePath];
        }

        public GeneralDevice GetAudioRenderer()
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return null;

            return new GeneralDevice() { Name = temp.Devices.AudioRenderer.Name, DevicePath = temp.Devices.AudioRenderer.DevicePath };
        }
        public void SetAudioRenderer(GeneralDevice dev)
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return;

            temp.Devices.AudioRenderer = DigitalTVScreen.DeviceStuff.AudioRendererDevices[dev.DevicePath];
        }

        public GeneralDevice GetH264Decoder()
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return null;

            return new GeneralDevice() { Name = temp.Devices.H264Decoder.Name, DevicePath = temp.Devices.H264Decoder.DevicePath };
        }
        public void SetH264Decoder(GeneralDevice dev)
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return;

            temp.Devices.H264Decoder = DigitalTVScreen.DeviceStuff.H264DecoderDevices[dev.DevicePath];
        }

        public GeneralDevice GetMPEG2Decoder()
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return null;

            return new GeneralDevice() { Name = temp.Devices.MPEG2Decoder.Name, DevicePath = temp.Devices.MPEG2Decoder.DevicePath };
        }
        public void SetMPEG2Decoder(GeneralDevice dev)
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return;

            temp.Devices.MPEG2Decoder = DigitalTVScreen.DeviceStuff.MPEG2DecoderDevices[dev.DevicePath];
        }

        #endregion

        #endregion

        #region Video

        public void StartVideo(string name)
        {
            try
            {
                if (this.Controls.OfType<FileVideoPlayer>().Count() > 0) StartVideo(this.Controls.OfType<FileVideoPlayer>().Single(x => x.Name == name).ID);
            }
            catch
            {
            }
        }
        public void StartVideo(int videoPlayerID)
        {
            if (this.Controls.OfType<FileVideoPlayer>().Where(x => x.ID == videoPlayerID).Count() != 1) return;
            else this.Controls.OfType<FileVideoPlayer>().Single(x => x.ID == videoPlayerID).Run();
        }
        public void StopVideo(string name)
        {
            try
            {
                if (this.Controls.OfType<FileVideoPlayer>().Count() > 0) StopVideo(this.Controls.OfType<FileVideoPlayer>().Single(x => x.Name == name).ID);
            }
            catch
            {
            }
        }
        public void StopVideo(int videoPlayerID)
        {
            if (this.Controls.OfType<FileVideoPlayer>().Where(x => x.ID == videoPlayerID).Count() != 1) return;
            else this.Controls.OfType<FileVideoPlayer>().Single(x => x.ID == videoPlayerID).Stop();
        }
        public void PreviousVideo(string name)
        {
            try
            {
                if (this.Controls.OfType<FileVideoPlayer>().Count() > 0) PreviousVideo(this.Controls.OfType<FileVideoPlayer>().Single(x => x.Name == name).ID);
            }
            catch
            {
            }
        }
        public void PreviousVideo(int videoPlayerID)
        {
            if (this.Controls.OfType<FileVideoPlayer>().Where(x => x.ID == videoPlayerID).Count() != 1) return;
            else this.Controls.OfType<FileVideoPlayer>().Single(x => x.ID == videoPlayerID).Previous();
        }
        public void NextVideo(string name)
        {
            try
            {
                if (this.Controls.OfType<FileVideoPlayer>().Count() > 0) NextVideo(this.Controls.OfType<FileVideoPlayer>().Single(x => x.Name == name).ID);
            }
            catch
            {
            }
        }
        public void NextVideo(int videoPlayerID)
        {
            if (this.Controls.OfType<FileVideoPlayer>().Where(x => x.ID == videoPlayerID).Count() != 1) return;
            else this.Controls.OfType<FileVideoPlayer>().Single(x => x.ID == videoPlayerID).Next();
        }


        public string[] GetPlayerNames()
        {
            List<string> res = new List<string>();

            foreach (var video in this.Controls.OfType<FileVideoPlayer>())
            {
                res.Add(video.Name);
            }

            return res.ToArray();
        }

        #endregion
        #endregion


        private float ScaledFontSize(int oldWndSize, int newWndSize, float fontSize)
        {
            Graphics g = this.CreateGraphics();
            float oldPts = oldWndSize * 72 / g.DpiY;
            float newPts = newWndSize * 72 / g.DpiY;
            g.Dispose();

            return (newPts * fontSize) / oldPts;
        }

        private void sempreNoTopoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            (sender as ToolStripMenuItem).Checked = this.TopMost;
        }

        #region Métodos públicos

        public void Show(PlayerWindowInformation information)
        {
            try
            {
                this.StartPosition = FormStartPosition.Manual;

                this.shapeSetByPlayerWndInfo = new Rectangle(information.Display.Bounds.Location.X, information.Display.Bounds.Location.Y, information.Display.Bounds.Size.Width, information.Display.Bounds.Size.Height);

                this.Location = information.Display.Bounds.Location;
                this.Size = information.Display.Bounds.Size;

                this.BackgroundImage = information.Background;
                this.BackgroundImageLayout = information.BackgroundImageLayout;

                this.DisplayName = information.Display.DeviceID;

                this.SuspendLayout();

                foreach (ItemConfiguration config in information.Components)
                    this.AddItemFromConfiguration(config);

                this.ResumeLayout(true);

                this.TopMost = false;
                this.PreventSleep();

                this.Show();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.TargetSite + Environment.NewLine + ex.Message);
#endif
            }
        }
        public void AddItemFromConfiguration(ItemConfiguration config)
        {
            try
            {
                if (config is DateTimeComposer)//MUDAR QUANDO FIZER O CONFIG CERTO
                {
                }
                else if (config is MarkeeConfiguration)
                {
                    MarkeeConfiguration temp = config as MarkeeConfiguration;

                    float newFontSize = this.ScaledFontSize(temp.Resolution.Height, temp.FinalResolution.Height, temp.Font.Size);

                    Font scaledFont = new Font(temp.Font.Name, newFontSize, temp.Font.Style, temp.Font.Unit, temp.Font.GdiCharSet, temp.Font.GdiVerticalFont);

                    Markee markee = new Markee(ComponentTargetSite.Player)
                    {
                        Location = temp.FinalLocation,
                        Size = temp.FinalSize,
                        TextList = temp.Text,
                        Speed = temp.Speed,
                        MarkeeFont = scaledFont,
                        BackColor = temp.BackColor,
                        TextColor = temp.TextColor,
                        ContextMenuStrip = contextMSFooter
                    };

                    this.Controls.Add(markee);

                    //markee.MouseMove += General_MouseMove;

                    markee.Run();

                }
                else if (config is ImageComposer)//MUDAR
                {
                }
                else if (config is PriceListComposer)//MUDAR
                {
                }
                else if (config is SlideShowComposer)//MUDAR
                {
                }
                else if (config is TVConfiguration)
                {
                    var temp = config as TVConfiguration;
                    if (string.IsNullOrWhiteSpace(temp.TunerDevicePath)) return;
                    #region using DigitalTVScreen
                    /*
                    DigitalTVScreen tvDisplay = new DigitalTVScreen() { Location = temp.FinalLocation, Size = temp.FinalSize };
                    tvDisplay.ChannelListChanged += tvDisplay_ChannelListChanged;
                    tvDisplay.Frequencia = 754000;
                    tvDisplay.Start();
                    tvDisplay.Tune();

                    tvDisplay.ContextMenuStrip = contextMSTV;

                    this.Controls.Add(tvDisplay);
                    */
                    #endregion

                    DigitalTVScreen tvScreen = this.CreateTVInstance(temp);

                    //LogForm logWindow = new LogForm();

                    //tvScreen.NewLogMessage += (s) => { logWindow.Log(s); };

                    //logWindow.Show();

                    DirectShowLib.DsDevice dev;
                    if(DigitalTVScreen.DeviceStuff.TunerDevices.TryGetValue(temp.TunerDevicePath, out dev))
                        tvScreen.Devices.TunerDevice = dev;

                    tvScreen.ContextMenuStrip = contextMSTV;

                    //tvScreen.MouseMove += General_MouseMove;

                    this.Controls.Add(tvScreen);

                    try
                    {
                        tvScreen.Channels.LoadFromXML();
                    }
                    catch (Exception)
                    {
                        tvScreen.Channels.RefreshChannels(); //Só funciona em Portugal. Compor o ecra de opçoes no composer
                        tvScreen.Channels.SaveToXML();
                    }

                    foreach (var ch in tvScreen.Channels.ChannelList)
                    {
                        (contextMSTV.Items["canalTVTSMItem"] as ToolStripMenuItem).DropDownItems.Add(ch.Name, null, channelToolStripMenuItem_Click);
                        (contextMSTV.Items["canalTVTSMItem"] as ToolStripMenuItem).DropDownItems[(contextMSTV.Items["canalTVTSMItem"] as ToolStripMenuItem).DropDownItems.Count - 1].Tag = ch;
                    }

                    if (tvScreen.Channels.ChannelList.Count > 0) tvScreen.Channels.TuneChannel(tvScreen.Channels.ChannelList[0]);
                }
                else if (config is VideoConfiguration)
                {
                    var temp = config as VideoConfiguration;

                    if (this.Controls.OfType<FileVideoPlayer>().Where(x => x.ID == temp.ID).Count() > 0) return;

                    FileVideoPlayer player = new FileVideoPlayer()
                    {
                        Location = temp.FinalLocation,
                        Size = temp.FinalSize,
                    };

                    player.ID = temp.ID;
                    try
                    {
                        player.Name = temp.Name;
                    }
                    catch
                    {
                    }
                    player.Playlist.Add(temp.Playlist);
                    player.Aspect = temp.Aspect;

                    this.Controls.Add(player);
                    player.Playlist.Replay = temp.Replay;

                    player.Run();
                }
                else if (config is WaitListComposer)//MUDAR
                {
                }
                else if (config is WeatherComposer)//MUDAR
                {
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }

        #endregion

        private void minimizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #region Prevenir o sleep/desligar o monitor
        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002,
            // Legacy flag, should not be used.
            // ES_USER_PRESENT   = 0x00000004,
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
        }

        public static class SleepUtil
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
        }

        private void PreventSleep()
        {
            if (SleepUtil.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS
                | EXECUTION_STATE.ES_DISPLAY_REQUIRED
                | EXECUTION_STATE.ES_SYSTEM_REQUIRED
                | EXECUTION_STATE.ES_AWAYMODE_REQUIRED) == 0) //Away mode for Windows >= Vista
                SleepUtil.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS
                    | EXECUTION_STATE.ES_DISPLAY_REQUIRED
                    | EXECUTION_STATE.ES_SYSTEM_REQUIRED); //Windows < Vista, forget away mode
        }
        #endregion
    }
}