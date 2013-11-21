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
        }

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
                    else if (component is VideoComposer)//MUDAR
                    {
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
                    (menuSource as DigitalTVScreen).Channels.TuneChannel((sender as ToolStripMenuItem).Tag as Channel);
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

                foreach (ItemConfiguration config in information.Components)
                    this.AddItemFromConfiguration(config);

                this.TopMost = true;

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

                    DigitalTVScreen tvScreen = new DigitalTVScreen()
                    {
                        Location = temp.FinalLocation,
                        Size = temp.FinalSize
                    };

                    tvScreen.Channels.Frequency = temp.Frequency;
                    tvScreen.Channels.ForceRebuildOnChannelTune = true;
                    tvScreen.VideoZoomMode = VideoSizeMode.FromInside;
                    tvScreen.VideoZoomValue = 0;
                    tvScreen.VideoKeepAspectRatio = true;
                    tvScreen.VideoAspectRatio = 1;

                    DirectShowLib.DsDevice dev;
                    if(DigitalTVScreen.DeviceStuff.TunerDevices.TryGetValue(temp.TunerDevicePath, out dev))
                        tvScreen.Devices.TunerDevice = dev;

                    tvScreen.ContextMenuStrip = contextMSTV;
                    
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
                        (contextMSTV.Items["canalTVTSMItem"] as ToolStripMenuItem).DropDownItems.Add(ch.Name, null, (object sender, EventArgs e) => { tvScreen.Channels.TuneChannel(ch); });
                    }

                    if (tvScreen.Channels.ChannelList.Count > 0) tvScreen.Channels.TuneChannel(tvScreen.Channels.ChannelList[0]);

                    //tvScreen.Start();
                }
                else if (config is VideoComposer)//MUDAR
                {
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
        public Channel GetChannel()
        {
            DigitalTVScreen temp = null;

            foreach (var control in this.Controls)
                if (control is DigitalTVScreen)
                {
                    temp = control as DigitalTVScreen;
                }
            return temp.Channels.CurrentChannel;
        }

        public void SetChannel(Channel ch)
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return;

            temp.Channels.TuneChannel(ch);
        }

        public TunerDevice GetTunerDevice()
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return null;

            return new TunerDevice() { Name = temp.Devices.TunerDevice.Name, DevicePath = temp.Devices.TunerDevice.DevicePath };
        }

        public void SetTunerDevice(TunerDevice dev)
        {
            DigitalTVScreen temp = null;

            if (this.Controls.OfType<DigitalTVScreen>().Count() > 0) temp = this.Controls.OfType<DigitalTVScreen>().ToList()[0];
            else return;

            temp.Devices.TunerDevice = DigitalTVScreen.DeviceStuff.TunerDevices.Single(x=>x.Value.DevicePath == dev.DevicePath).Value;
        }

        #endregion
    }
}