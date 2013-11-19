using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DirectShowLib;
using TV2Lib;

namespace TV_2
{
    public partial class FormPrincipal : Form
    {
        private const string START_BUTTON_START_TEXT = "Crashar esta merda";
        private const string START_BUTTON_STOP_TEXT = "HALT!";


        private Thread signalQualityRetriever;
        private void SignalQualityWorkerMethod()
        {
            int quality, str;
            bool locked, present;
            while (true)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    digitalTVScreen.GetSignalStatistics(out locked, out present, out str, out quality);

                    checkBoxSignalLocked.Checked = locked;
                    checkBoxSignalPresent.Checked = present;

                    progressBarSignalQuality.Value = quality;
                    progressBarSignalStrength.Value = str;

                    labelSignalStrength.Text = string.Format("S {0}%", quality.ToString());
                    labelSignalQuality.Text = string.Format("Q {0}%", str.ToString());
                });

                Thread.Sleep(100);
            }
        }


        public FormPrincipal()
        {
            InitializeComponent();

            signalQualityRetriever = new Thread(SignalQualityWorkerMethod) {IsBackground = true};
        }

        void digitalTVScreen1_NewLogMessage(string message)
        {
            textBoxLog.AppendText(message);
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            comboBoxTunerDevice.DisplayMember = "Name";
            comboBoxTunerDevice.ValueMember = "DevicePath";
            comboBoxACodec.DisplayMember = "Name";
            comboBoxACodec.ValueMember = "DevicePath";
            comboBoxARenderer.DisplayMember = "Name";
            comboBoxARenderer.ValueMember = "DevicePath";
            comboBoxH264Codec.DisplayMember = "Name";
            comboBoxH264Codec.ValueMember = "DevicePath";
            comboBoxMPEG2Codec.DisplayMember = "Name";
            comboBoxMPEG2Codec.ValueMember = "DevicePath";

            comboBoxTunerDevice.Items.Add("Auto");
            foreach (var item in DigitalTVScreen.DeviceStuff.TunerDevices.Values)
            {
                comboBoxTunerDevice.Items.Add(item);
            }
            comboBoxTunerDevice.SelectedIndex = 0;

            comboBoxACodec.Items.Add("<Nenhum>");
            foreach (var item in DigitalTVScreen.DeviceStuff.AudioDecoderDevices.Values)
                comboBoxACodec.Items.Add(item);
            comboBoxACodec.SelectedIndex = 0;

            comboBoxARenderer.Items.Add("<Nenhum>");
            foreach (var item in DigitalTVScreen.DeviceStuff.AudioRendererDevices.Values)
                comboBoxARenderer.Items.Add(item);
            comboBoxARenderer.SelectedIndex = 0;

            comboBoxH264Codec.Items.Add("<Nenhum>");
            foreach (var item in DigitalTVScreen.DeviceStuff.H264DecoderDevices.Values)
                comboBoxH264Codec.Items.Add(item);
            comboBoxH264Codec.SelectedIndex = 0;

            comboBoxMPEG2Codec.Items.Add("<Nenhum>");
            foreach (var item in DigitalTVScreen.DeviceStuff.MPEG2DecoderDevices.Values)
                comboBoxMPEG2Codec.Items.Add(item);
            comboBoxMPEG2Codec.SelectedIndex = 0;

            //comboBoxVRenderer.SelectedIndex = 0;
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                digitalTVScreen.Stop();
                digitalTVScreen.Dispose();

                if (signalQualityRetriever.IsAlive) signalQualityRetriever.Abort();
            }
            catch
            {
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                int hr = 0;

                if (digitalTVScreen.State == DigitalTVScreen.DigitalTVState.Running)
                {
                    digitalTVScreen.Stop();

                    buttonStart.Text = START_BUTTON_START_TEXT;
                }
                else
                {
                    hr = digitalTVScreen.Start();

                    buttonStart.Text = START_BUTTON_STOP_TEXT;

                    if (hr >= 0)
                    {
                        if (signalQualityRetriever.ThreadState == ThreadState.Suspended) signalQualityRetriever.Resume();
                        else if (signalQualityRetriever.ThreadState == ThreadState.Unstarted) signalQualityRetriever.Start();
                    }
                }
            }
            catch
            {
            }
        }
        private void buttonRefreshSignalStatistics_Click(object sender, EventArgs e)
        {
            
            int quality, str;
            bool present, locked;

            digitalTVScreen.GetSignalStatistics(out locked, out present, out str, out quality);

            checkBoxSignalLocked.Checked = locked;
            checkBoxSignalPresent.Checked = present;

            progressBarSignalQuality.Value = quality;
            progressBarSignalStrength.Value = str;

            labelSignalStrength.Text = string.Format("S {0}%", str.ToString());
            labelSignalQuality.Text = string.Format("Q {0}%", quality.ToString());
        }
        

        private void comboBoxTunerDevice_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBoxTunerDevice.Items.Count > 0)
                digitalTVScreen.Devices.TunerDevice = comboBoxTunerDevice.Text.Equals("Auto") ? null : GraphBuilderBDA.TunerDevices[(comboBoxTunerDevice.SelectedItem as DsDevice).DevicePath];
        }
        private void comboBoxH264Codec_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBoxH264Codec.Items.Count > 0)
                digitalTVScreen.Devices.H264Decoder = comboBoxH264Codec.Text.Equals("<Nenhum>") ? null : GraphBuilderBDA.H264DecoderDevices[(comboBoxH264Codec.SelectedItem as DsDevice).DevicePath];
        }
        private void comboBoxMPEG2Codec_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBoxMPEG2Codec.Items.Count > 0)
                digitalTVScreen.Devices.MPEG2Decoder = comboBoxMPEG2Codec.Text.Equals("<Nenhum>") ? null : GraphBuilderBDA.MPEG2DecoderDevices[(comboBoxMPEG2Codec.SelectedItem as DsDevice).DevicePath];
        }
        private void comboBoxACodec_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBoxACodec.Items.Count > 0)
                digitalTVScreen.Devices.AudioDecoder = comboBoxACodec.Text.Equals("<Nenhum>") ? null : GraphBuilderBDA.AudioDecoderDevices[(comboBoxACodec.SelectedItem as DsDevice).DevicePath];
        }
        private void comboBoxARenderer_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBoxARenderer.Items.Count > 0)
                digitalTVScreen.Devices.AudioRenderer = comboBoxARenderer.Text.Equals("<Nenhum>") ? null : GraphBuilderBDA.AudioRendererDevices[(comboBoxARenderer.SelectedItem as DsDevice).DevicePath];
        }

        private void digitalTVScreen_ChannelListChanged(object sender, ChannelEventArgs e)
        {
            try
            {
                listViewCanais.Items.Clear();

                foreach(var canal in e.ChannelList.OfType<ChannelDVBT>().OrderBy(x=>x.ChannelNumber))
                {
                    listViewCanais.Items.Add(new ListViewItem() { Text = canal.Name == "" ? canal.ChannelNumber.ToString() : canal.Name, Tag = canal });
                }
            }
            catch
            {

            }
        }

        private void listViewCanais_DoubleClick(object sender, EventArgs e)
        {
            if (listViewCanais.SelectedItems.Count <= 0) return;

            Channel ch = listViewCanais.SelectedItems[0].Tag as Channel;

            if (ch == null) return;

            digitalTVScreen.Channels.TuneChannel(ch);
        }

        private void buttonRefreshChannels_Click(object sender, EventArgs e)
        {
            try
            {
                digitalTVScreen.Channels.RefreshChannels();
            }
            catch
            {
            }
        }

        private void buttonScanFreqs_Click(object sender, EventArgs e)
        {
            if (radioButtonAuto.Checked)
            {
                digitalTVScreen.Channels.ScanFrequencies();
            }
            else if (radioButtonCustom.Checked)
            {
                try
                {
                    int max, min, step;

                    int.TryParse(textBoxMin.Text, out min);
                    int.TryParse(textBoxMax.Text, out max);
                    int.TryParse(textBoxStep.Text, out step);

                    digitalTVScreen.Channels.ScanFrequencies(min, max, step);
                }
                catch
                {
                }
            }
        }

        private void radioButtonCustom_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if(radioButton != null)
                groupBoxCustomScan.Enabled = radioButton.Checked;
        }

        private void groupBoxCustomScan_EnabledChanged(object sender, EventArgs e)
        {
            var groupBox = sender as GroupBox;
            if (groupBox != null && groupBox.Enabled)
            {
                if (string.IsNullOrWhiteSpace(textBoxMin.Text) &&
                   string.IsNullOrWhiteSpace(textBoxMax.Text) &&
                   string.IsNullOrWhiteSpace(textBoxStep.Text))
                {
                    textBoxMax.Text = digitalTVScreen.Channels.MAX_FREQUENCY.ToString();
                    textBoxMin.Text = digitalTVScreen.Channels.MIN_FREQUENCY.ToString();
                    textBoxStep.Text = digitalTVScreen.Channels.DEFAULT_STEP.ToString();
                }
            }
        }

        private void checkBoxForceRebuild_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
                digitalTVScreen.Channels.ForceRebuildOnChannelTune = checkBox.Checked;
        }

        private void buttonSaveChannelsToXML_Click(object sender, EventArgs e)
        {
            digitalTVScreen.Channels.SaveToXML();
        }

        private void buttonLoadChannelsFromXML_Click(object sender, EventArgs e)
        {
            digitalTVScreen.Channels.LoadFromXML();
        }

        private void comboBoxTunerDevice_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
