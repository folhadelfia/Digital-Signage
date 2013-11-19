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

            _fileVideoPlayer.Autoplay = true;
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
                _fileVideoPlayer.Source = openFile.FileName;
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
            for(int i = 0; i< 10; i++)
            _fileVideoPlayer.Playlist.Add(string.Format("{0} TESTE", _fileVideoPlayer.Playlist.Count));
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
