using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Assemblies.ClientModel;
using Assemblies.Misc;
using Transitions;
using TV2Lib;

namespace RemoteControlWForms
{
    public partial class Comando : Form
    {
        Connection connection;
        List<ChannelDVBT> channels = new List<ChannelDVBT>();
        string displayName = "";

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
            catch (Exception ex)
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

                    this.Location = new Point(this.Location.X - (offset.X - posicaoRato.X), this.Location.Y - (offset.Y - posicaoRato.Y));
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

        #region Animações

        private void Blink(object sender, EventArgs e)
        {
            PictureBox temp = (sender as PictureBox);
            Transition t = new Transition(new TransitionType_Bounce(400));

            Color toBlink = Color.FromArgb(150, (sender as PictureBox).BackColor);

            t.add((sender as PictureBox), "BackColor", toBlink);
            t.run();
        }

        #endregion

        public Comando()
        {
            InitializeComponent();

            pictureBoxMinimize.Click += Blink;
            pictureBoxClose.Click += Blink;
            pictureBoxOnOff.Click += Blink;
            pictureBoxRTP1.Click += Blink;
            pictureBoxRTP2.Click += Blink;
            pictureBoxSIC.Click += Blink;
            pictureBoxTVI.Click += Blink;
            pictureBoxVolDown.Click += Blink;
            pictureBoxVolUp.Click += Blink;

            pictureBoxPlay.Click += Blink;
            pictureBoxStop.Click += Blink;
            pictureBoxFWD.Click += Blink;
            pictureBoxRWD.Click += Blink;


            pictureBoxVolDown.Visible = false;
            pictureBoxVolUp.Visible = false;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBoxMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBoxOnOff_Click(object sender, EventArgs e)
        {
            ServiceList serviceList = new ServiceList();

            if (serviceList.ShowDialog() == System.Windows.Forms.DialogResult.OK && serviceList.PC != null && serviceList.Screen != null)
            {
                connection = new WCFConnection(serviceList.PC);

                connection.Open();

                bool hasTV = connection.HasTV(serviceList.Screen),
                     hasVideo = connection.HasVideo(serviceList.Screen);
                
                displayName = serviceList.Screen;

                if (hasTV)
                {
                    channels.Clear();

                    var chList = connection.GetTVChannels();

                    if (chList == null) return;

                    foreach (var ch in chList)
                    {
                        channels.Add(ch as ChannelDVBT);
                    }
                }

                pictureBoxRTP1.Enabled = hasTV;
                pictureBoxRTP2.Enabled = hasTV;
                pictureBoxSIC.Enabled = hasTV;
                pictureBoxTVI.Enabled = hasTV;

                pictureBoxStop.Enabled = hasVideo;
                pictureBoxPlay.Enabled = hasVideo;
                pictureBoxRWD.Enabled = hasVideo;
                pictureBoxFWD.Enabled = hasVideo;
            }
        }

        //Escolher o monitor que quer controlar antes de fazer a ligação, e guardar o id numa var. Sempre que quiser mudar d canal,
        //dá o valor dessa var

        #region TV

        private void pictureBoxRTP1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (connection != null && connection.State == Assemblies.ClientModel.ConnectionState.Open && channels.Count > 0)
                    connection.SetCurrentTVChannel(displayName, channels.Single(x => x.ChannelNumber == 1));
            }));

            t.Start();
        }
        private void pictureBoxRTP2_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (connection != null && connection.State == Assemblies.ClientModel.ConnectionState.Open && channels.Count > 0)
                    connection.SetCurrentTVChannel(displayName, channels.Single(x => x.ChannelNumber == 2));
            }));

            t.Start();
        }
        private void pictureBoxSIC_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (connection != null && connection.State == Assemblies.ClientModel.ConnectionState.Open && channels.Count > 0)
                    connection.SetCurrentTVChannel(displayName, channels.Single(x => x.ChannelNumber == 3));
            }));

            t.Start();
        }
        private void pictureBoxTVI_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (connection != null && connection.State == Assemblies.ClientModel.ConnectionState.Open && channels.Count > 0)
                    connection.SetCurrentTVChannel(displayName, channels.Single(x => x.ChannelNumber == 4));
            }));

            t.Start();
        }

        #endregion

        #region Video
        private void pictureBoxPlay_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (connection != null && connection.State == Assemblies.ClientModel.ConnectionState.Open)
                    connection.StartVideo(displayName, 0); //QUANDO DER PARA TER MAIS QUE UM VÍDEO, SUBSTITUIR 0 POR playerID
            }));

            t.Start();
        }
        private void pictureBoxStop_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (connection != null && connection.State == Assemblies.ClientModel.ConnectionState.Open)
                    connection.StopVideo(displayName, 0); //QUANDO DER PARA TER MAIS QUE UM VÍDEO, SUBSTITUIR 0 POR playerID
            }));

            t.Start();
        }
        private void pictureBoxFWD_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (connection != null && connection.State == Assemblies.ClientModel.ConnectionState.Open)
                    connection.NextVideo(displayName, 0); //QUANDO DER PARA TER MAIS QUE UM VÍDEO, SUBSTITUIR 0 POR playerID
            }));

            t.Start();
        }
        private void pictureBoxRWD_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (connection != null && connection.State == Assemblies.ClientModel.ConnectionState.Open)
                    connection.PreviousVideo(displayName, 0); //QUANDO DER PARA TER MAIS QUE UM VÍDEO, SUBSTITUIR 0 POR playerID
            }));

            t.Start();
        }
        #endregion

        private void pictureBox_EnabledChanged(object sender, EventArgs e)
        {
            (sender as Control).BackColor = (sender as Control).Enabled ? Color.CornflowerBlue : Color.LightGray;
        }

    }
}
