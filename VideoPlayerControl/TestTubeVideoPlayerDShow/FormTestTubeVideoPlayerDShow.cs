using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using VideoPlayerDShowLib;
using DirectShowLib;

namespace TestTubeVideoPlayerDShow
{
    public partial class FormTestTubeVideoPlayerDShow : Form
    {
        public FormTestTubeVideoPlayerDShow()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (_crossbarVideoPlayer.State == CrossbarVideoPlayer.MediaStatus.Stopped)
            {
                _crossbarVideoPlayer.Devices.AudioRenderer = CrossbarVideoPlayer.AudioRenderers[0];

                _crossbarVideoPlayer.Start();
                (sender as Button).Text = "Stop";
            }
            else
            {
                _crossbarVideoPlayer.Stop();

                (sender as Button).Text = "Start";
            }
        }

        private void FormTestTubeVideoPlayerDShow_Load(object sender, EventArgs e)
        {
            comboBoxDevices.DisplayMember = "Name";
            comboBoxSources.DisplayMember = "Type";
        }

        private void radioButtonKeep_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                this._crossbarVideoPlayer.VideoSettings.AspectRatio = CrossbarVideoPlayer.VideoSettingsWrapper.VideoRatio.KeepOriginal;
        }

        private void radioButton169_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                this._crossbarVideoPlayer.VideoSettings.AspectRatio = CrossbarVideoPlayer.VideoSettingsWrapper.VideoRatio.Force169;
        }

        private void radioButton43_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                this._crossbarVideoPlayer.VideoSettings.AspectRatio = CrossbarVideoPlayer.VideoSettingsWrapper.VideoRatio.Force43;
        }

        private void radioButtonZoomStretch_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                this._crossbarVideoPlayer.VideoSettings.ZoomMode = CrossbarVideoPlayer.VideoSettingsWrapper.VideoSizeMode.StretchToWindow;
        }

        private void radioButtonZoomFit_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
                this._crossbarVideoPlayer.VideoSettings.ZoomMode = CrossbarVideoPlayer.VideoSettingsWrapper.VideoSizeMode.FromInside;
        }

        private void comboBoxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _crossbarVideoPlayer.Devices.VideoDevice = (sender as ComboBox).SelectedItem as DsDevice;

                comboBoxStandards.Items.Clear();

                foreach (var standard in _crossbarVideoPlayer.VideoSettings.VideoStandardsSupported)
                    comboBoxStandards.Items.Add(standard);

                comboBoxStandards.SelectedItem = _crossbarVideoPlayer.VideoSettings.VideoStandardCurrent;

                comboBoxSources.Items.Clear();

                foreach (var source in _crossbarVideoPlayer.Devices.SourcesSupported)
                    comboBoxSources.Items.Add(source);

                comboBoxSources.SelectedItem = _crossbarVideoPlayer.Devices.Source;
            }
            catch
            {
            }
        }

        private void comboBoxStandards_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _crossbarVideoPlayer.VideoSettings.VideoStandardCurrent = (sender as ComboBox).SelectedItem as AnalogVideoStandard? ?? AnalogVideoStandard.None;
            }
            catch
            {
            }
        }

        private void comboBoxSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            _crossbarVideoPlayer.Devices.Source = (sender as ComboBox).SelectedItem as VideoPlayerDShowLib.CrossbarVideoPlayer.DevicesWrapper.CrossbarInputPin;
        }

        private void comboBoxDevices_MouseEnter(object sender, EventArgs e)
        {
            bool refreshItemList = false;

            List<DsDevice> videoDevices = CrossbarVideoPlayer.VideoDevices.ToList();

            foreach (var item in videoDevices)
            {
                if (!(sender as ComboBox).Items.OfType<DsDevice>().Select(x=>x.DevicePath).Contains(item.DevicePath))
                {
                    refreshItemList = true;
                    break;
                }
            }

            if (!refreshItemList)
            {
                foreach (var item in (sender as ComboBox).Items.OfType<DsDevice>())
                {
                    if (!videoDevices.Select(x => x.DevicePath).Contains(item.DevicePath))
                    {
                        refreshItemList = true;
                        break;
                    }
                }
            }

            if (refreshItemList)
            {
                comboBoxDevices.Items.Clear();

                foreach (var dev in CrossbarVideoPlayer.VideoDevices)
                {
                    comboBoxDevices.Items.Add(dev);
                }
            }
        }
    }
}
