using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestTubeVideoPlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            switch (_fileVideoPlayer.Aspect)
            {
                case VideoPlayer.FileVideoPlayer.AspectMode.Center: comboBox1.SelectedItem = "Center";
                    break;
                case VideoPlayer.FileVideoPlayer.AspectMode.Fill: comboBox1.SelectedItem = "Fill";
                    break;
                case VideoPlayer.FileVideoPlayer.AspectMode.Fit: comboBox1.SelectedItem = "Fit";
                    break;
                case VideoPlayer.FileVideoPlayer.AspectMode.Stretch: comboBox1.SelectedItem = "Stretch";
                    break;
                default: comboBox1.SelectedItem = "Stretch";
                    break;
            }

            _fileVideoPlayer.Playlist.Autoplay = false;
            _fileVideoPlayer.Playlist.Replay = true;

            _fileVideoPlayer.Playlist.VideoAdded += Playlist_VideoAdded;
            _fileVideoPlayer.Playlist.VideoMoved += Playlist_VideoMoved;
            _fileVideoPlayer.Playlist.VideoRemoved += Playlist_VideoRemoved;
        }

        void Playlist_VideoRemoved(object sender, VideoPlayer.VideoRemovedEventArgs e)
        {
            listBox1.SuspendLayout();

            listBox1.Items.Clear();

            int i = 0;
            foreach (var item in _fileVideoPlayer.Playlist.Files)
            {
                listBox1.Items.Add(string.Format("{0} - {1}", ++i, item));
            }

            listBox1.ResumeLayout();
            listBox1.PerformLayout();
        }

        void Playlist_VideoMoved(object sender, VideoPlayer.VideoMovedEventArgs e)
        {
            listBox1.SuspendLayout();

            listBox1.Items.Clear();

            int i = 0;
            foreach (var item in _fileVideoPlayer.Playlist.Files)
            {
                listBox1.Items.Add(string.Format("{0} - {1}", ++i, item));
            }

            listBox1.ResumeLayout();
            listBox1.PerformLayout();
        }

        void Playlist_VideoAdded(object sender, VideoPlayer.VideoAddedEventArgs e)
        {
            listBox1.SuspendLayout();

            listBox1.Items.Clear();

            int i = 0;
            foreach (var item in _fileVideoPlayer.Playlist.Files)
            {
                listBox1.Items.Add(string.Format("{0} - {1}", ++i , item));
            }

            listBox1.ResumeLayout();
            listBox1.PerformLayout();
        }

        private void toolStripButtonPlay_Click(object sender, EventArgs e)
        {
            _fileVideoPlayer.Run();

            labelVideoInfo.Text = string.Format("Position: ({1}, {2}){0}Size: ({3}, {4})", Environment.NewLine, _fileVideoPlayer.VideoLocation.X, _fileVideoPlayer.VideoLocation.Y, _fileVideoPlayer.VideoSize.Width, _fileVideoPlayer.VideoSize.Height);
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if(openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _fileVideoPlayer.Playlist.Add(openFile.FileName);
            }
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            _fileVideoPlayer.Stop();
        }

        private void toolStripButtonPause_Click(object sender, EventArgs e)
        {
            _fileVideoPlayer.Pause();
        }

        private void buttonTestAdd_Click(object sender, EventArgs e)
        {
            //for(int i = 0; i< 10; i++)
            //_fileVideoPlayer.Playlist.Add(string.Format("{0} TESTE", _fileVideoPlayer.Playlist.Count));

            OpenFileDialog d = new OpenFileDialog();

            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                _fileVideoPlayer.Playlist.Add(d.FileName);
        }

        private void buttonTestRemove_Click(object sender, EventArgs e)
        {
            int i = 0;

            _fileVideoPlayer.Playlist.Remove(i);
        }

        private void buttonTestMove_Click(object sender, EventArgs e)
        {
            int i = 4, j = 3;

            _fileVideoPlayer.Playlist.Move(i, j);
        }

        private void buttonTestClear_Click(object sender, EventArgs e)
        {
            _fileVideoPlayer.Playlist.Clear();
        }

        private void _fileVideoPlayer_Resize(object sender, EventArgs e)
        {

        }

        private void checkBoxKeepAspectRatio_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cBox = sender as ComboBox;

            switch (cBox.SelectedItem.ToString())
            {
                case "Fill": _fileVideoPlayer.Aspect = VideoPlayer.FileVideoPlayer.AspectMode.Fill;
                    break;
                case "Fit": _fileVideoPlayer.Aspect = VideoPlayer.FileVideoPlayer.AspectMode.Fit;
                    break;
                case "Stretch": _fileVideoPlayer.Aspect = VideoPlayer.FileVideoPlayer.AspectMode.Stretch;
                    break;
                case "Center": _fileVideoPlayer.Aspect = VideoPlayer.FileVideoPlayer.AspectMode.Center;
                    break;
                default:
                    break;
            }
        }
    }
}
