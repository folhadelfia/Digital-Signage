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

            _fileVideoPlayer.Playlist.Autoplay = true;
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
    }
}
