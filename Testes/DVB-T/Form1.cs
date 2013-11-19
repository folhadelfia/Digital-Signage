using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DigitalTV;

using DirectShowLib.BDA;

//Conditional symbols: ALLOW_UNTESTED_STRUCTS;ALLOW_UNTESTED_INTERFACES

namespace DVB_T
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            digitalTVScreen.NewLogMessage += Log;
            digitalTVScreen.ChannelListChanged += digitalTVScreen_ChannelListChanged;

            comboBoxACodecs.Items.Add("<Nenhum>");
            foreach (var item in digitalTVScreen.AudioDecoderDevices)
                comboBoxACodecs.Items.Add(item);
            comboBoxACodecs.SelectedIndex = 0;

            comboBoxARenderer.Items.Add("<Nenhum>");
            foreach (var item in digitalTVScreen.AudioRendererDevices)
                comboBoxARenderer.Items.Add(item);
            comboBoxARenderer.SelectedIndex = 0;

            comboBoxVCodecsH264.Items.Add("<Nenhum>");
            foreach (var item in digitalTVScreen.H264DecoderDevices)
                comboBoxVCodecsH264.Items.Add(item);
            comboBoxVCodecsH264.SelectedIndex = 0;

            comboBoxVCodecMPEG2.Items.Add("<Nenhum>");
            foreach (var item in digitalTVScreen.MPEG2DecoderDevices)
                comboBoxVCodecMPEG2.Items.Add(item);
            comboBoxVCodecMPEG2.SelectedIndex = 0;

            comboBoxVRenderer.SelectedIndex = 0;
        }

        void digitalTVScreen_ChannelListChanged(object sender, ChannelEventArgs e)
        {
            listBoxChannels.Items.Clear();

            foreach (Channel ch in digitalTVScreen.Channels)
            {
                listBoxChannels.Items.Add(ch);
            }
        }

        public void Log(string message)
        {
            try
            {
                logRichTextBox.AppendText(DateTime.Now.ToString("[HH:mm:ss]: ") + message + Environment.NewLine);
                logRichTextBox.ScrollToCaret();
            }
            catch
            {
            }
            finally
            {
            }
        }

        private void FrequenciaTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\b')
                e.Handled = true;
        }

        private void buttonLigar_Click(object sender, EventArgs e)
        {
            try
            {
                digitalTVScreen.Frequencia = Convert.ToInt32(FrequenciaTextBox.Text);
                digitalTVScreen.ONID = Convert.ToInt32(ONIDTextBox.Text);
                digitalTVScreen.TSID = Convert.ToInt32(TSIDTextBox.Text);
                digitalTVScreen.SID = Convert.ToInt32(SIDTextBox.Text);

                digitalTVScreen.Start();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
            finally
            {
            }
        }

        private void buttonDesligar_Click(object sender, EventArgs e)
        {
            try
            {
                digitalTVScreen.Stop();
            }
            catch
            {
            }
            finally
            {
            }
        }

        private void buttonSintonizar_Click(object sender, EventArgs e)
        {
            digitalTVScreen.Frequencia = Convert.ToInt32(FrequenciaTextBox.Text);
            digitalTVScreen.ONID = Convert.ToInt32(ONIDTextBox.Text);
            digitalTVScreen.TSID = Convert.ToInt32(TSIDTextBox.Text);
            digitalTVScreen.SID = Convert.ToInt32(SIDTextBox.Text);

            digitalTVScreen.Tune();
        }

        private void buttonUpdateList_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(FrequenciaTextBox.Text)) { Log("Introduza uma frequência válida"); return; }

                digitalTVScreen.Frequencia = Convert.ToInt32(FrequenciaTextBox.Text);
                digitalTVScreen.ONID = -1;
                digitalTVScreen.TSID = -1;
                digitalTVScreen.SID = -1;

                digitalTVScreen.UpdateChannelList();

                listBoxChannels.Items.Clear();
                foreach (Channel ch in digitalTVScreen.Channels)
                {
                    listBoxChannels.Items.Add(ch);
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
            finally
            {
            }
        }

        private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Channel channel = (sender as ListBox).SelectedItem as Channel;

                digitalTVScreen.Tune(channel);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("listBoxChannels_SelectedIndexChanged" + Environment.NewLine + ex.Message);
#endif
            }
        }

        private void buttonDeinterlace_Click(object sender, EventArgs e)
        {
            digitalTVScreen.CheckDeinterlace();
        }

        private void buttonSaveGraph_Click(object sender, EventArgs e)
        {
            digitalTVScreen.SaveGraphToFile();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxVCodecsH264_SelectedValueChanged(object sender, EventArgs e)
        {
            digitalTVScreen.H264Decoder = comboBoxVCodecsH264.SelectedItem.ToString();
        }

        private void comboBoxVCodecMPEG2_SelectedValueChanged(object sender, EventArgs e)
        {
            digitalTVScreen.MPEG2Decoder = comboBoxVCodecMPEG2.SelectedItem.ToString();
        }

        private void comboBoxACodecs_SelectedValueChanged(object sender, EventArgs e)
        {
            digitalTVScreen.AudioDecoder = comboBoxACodecs.SelectedItem.ToString();
        }

        private void comboBoxARenderer_SelectedValueChanged(object sender, EventArgs e)
        {
            digitalTVScreen.AudioRenderer = comboBoxARenderer.SelectedItem.ToString();
        }

        private void comboBoxVRenderer_SelectedValueChanged(object sender, EventArgs e)
        {
            VideoRenderer renderer;

            switch (comboBoxVRenderer.SelectedIndex)
            {
                case 0: renderer = VideoRenderer.VMR9;
                    break;
                case 1: renderer = VideoRenderer.EVR;
                    break;
                default: renderer = VideoRenderer.VMR9;
                    break;
            }

            digitalTVScreen.VideoRenderer = renderer;
        }
    }
}
