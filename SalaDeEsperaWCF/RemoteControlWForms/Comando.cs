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
using Transitions;
using TV2Lib;

namespace RemoteControlWForms
{
    public partial class Comando : Form
    {
        Connection connection;
        List<ChannelDVBT> channels = new List<ChannelDVBT>();

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
            Client.ServiceList serviceList = new Client.ServiceList();

            if (serviceList.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                connection = new WCFConnection(serviceList.PC);

                connection.Open();

                channels.Clear();
                foreach (var ch in connection.GetTVChannels())
                {
                    channels.Add(ch as ChannelDVBT);
                }
            }
        }

        private void pictureBoxRTP1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (connection != null && connection.State == Assemblies.ClientModel.ConnectionState.Open && channels.Count > 0)
                    connection.SetCurrentTVChannel(channels.Single(x => x.ChannelNumber == 1));
            }));

            t.Start();
        }

        private void pictureBoxRTP2_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (connection != null && connection.State == Assemblies.ClientModel.ConnectionState.Open && channels.Count > 0)
                    connection.SetCurrentTVChannel(channels.Single(x => x.ChannelNumber == 2));
            }));

            t.Start();
        }

        private void pictureBoxSIC_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (connection != null && connection.State == Assemblies.ClientModel.ConnectionState.Open && channels.Count > 0)
                    connection.SetCurrentTVChannel(channels.Single(x => x.ChannelNumber == 3));
            }));

            t.Start();
        }

        private void pictureBoxTVI_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                if (connection != null && connection.State == Assemblies.ClientModel.ConnectionState.Open && channels.Count > 0)
                    connection.SetCurrentTVChannel(channels.Single(x => x.ChannelNumber == 4));
            }));

            t.Start();
        }
    }
}
