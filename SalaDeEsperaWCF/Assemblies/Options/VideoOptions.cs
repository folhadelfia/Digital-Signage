using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assemblies.ClientModel;
using Assemblies.Components;
using Assemblies.Configurations;
using Assemblies.ExtensionMethods;
using Assemblies.Toolkit;
using VideoPlayer;

namespace Assemblies.Options
{
    public partial class VideoOptions : Form, IOptionsWindow
    {
        Connection connection;
        VideoConfiguration configuration;

        public List<string> Playlist { get; private set; }

        public VideoOptions()
        {
            InitializeComponent();

            configuration = new VideoConfiguration();
        }

        public VideoOptions(VideoConfiguration config) : this()
        {
            config.Playlist.DeepCopyTo(this.Playlist);

            config.Playlist.DeepCopyTo(configuration.Playlist);
        }

        private void VideoOptions_Load(object sender, EventArgs e)
        {
            LoadFiles();

            textBoxPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

            this.UpdateAspectMode();
        }


        #region Mover items na ListView
        //http://stackoverflow.com/questions/4796109/how-to-move-item-in-listbox-up-and-down alterado para multi item


        private void buttonMoveItemsUp_Click(object sender, EventArgs e)
        {
            MoveListBoxItemsUp(listViewVideoPlaylist);
        }

        private void buttonMoveItemsDown_Click(object sender, EventArgs e)
        {
            MoveListBoxItemsDown(listViewVideoPlaylist);
        }




        private void MoveListBoxItemsUp(object target)
        {
            MoveItem(target as ListView, -1);
            //UpdateVideoList(listViewVideoPlaylist);
        }

        private void UpdateVideoList(ListView list)
        {
            try
            {
                list.Items.Clear();
                foreach (var video in this.Playlist)
                {
                    string name = MyToolkit.Files.FileNameFromPath(video);

                    if(!string.IsNullOrWhiteSpace(name))
                        AddVideoToPlaylist(name, video);
                }

            }
            catch (Exception)
            {

            }
        }

        private void MoveListBoxItemsDown(object sender)
        {
            MoveItem(sender as ListView, 1);
            //UpdateFooterTextList();
        }

        private void MoveItem(ListView sender, int direction)
        {

            if (sender == null) return;
            try
            {
                List<int> selectedIndices;

                //Se estamos a mover para cima, mover os items do mais acima para o mais abaixo, e fazer o contrário se estivermos a mover para baixo
                if (direction == -1)
                    selectedIndices = sender.SelectedIndices.Cast<int>().ToList();
                else
                    selectedIndices = sender.SelectedIndices.Cast<int>().OrderByDescending(x => x).ToList();

                //Se estamos a tentar mover o 1º item da lista para cima, remove-se
                if (selectedIndices.Contains(0) && direction == -1)
                    selectedIndices.Remove(0);

                //Se estamos a tentar mover o último item da lista para baixo, remove-se
                if (selectedIndices.Contains(sender.Items.Count - 1) && direction == 1)
                    selectedIndices.Remove(sender.Items.Count - 1);

                foreach (int index in selectedIndices)
                {
                    // Calculate new index using move direction
                    int newIndex = index + direction;

                    //// Checking bounds of the range
                    //if (newIndex < 0 || newIndex >= sender.Items.Count)
                    //    return; // Index out of range - nothing to do

                    var selected = sender.Items[index];

                    //Só mover os items se a nova posição não estiver seleccionada
                    if (!(sender.SelectedIndices.Contains(newIndex)))
                    {
                        // Removing removable element
                        sender.Items.RemoveAt(index);
                        // Insert it in new position
                        sender.Items.Insert(newIndex, selected);
                        // Restore selection
                        sender.Items[newIndex].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("MoveItem" + Environment.NewLine + "Direction: " + direction + Environment.NewLine + ex.Message);
#endif
            }

            UpdateConfig();
        }
        #endregion


        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string path = GetVideoFolderPathWithDialog();

            if (!string.IsNullOrWhiteSpace(path))
            {
                textBoxPath.Text = path;
            }
        }
        private string GetVideoFolderPathWithDialog()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            string path = "";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = fbd.SelectedPath;
            }

            return path;
        }

        private void textBoxPath_TextChanged(object sender, EventArgs e)
        {
            TextBox pathBox = sender as TextBox;

            string path = pathBox.Text;

            if (Directory.Exists(path))
            {
                PopulateLocalVideoList(path);
            }
        }


        private void buttonEnviar_Click(object sender, EventArgs e)
        {

            List<string> files = new List<string>();

            foreach (var file in listViewLocalFiles.SelectedItems.Cast<ListViewItem>().Select(x => x.Tag as string))
            {
                files.Add(file);
            }

            this.TransferFiles(files);
        }

        private void TransferFiles(IEnumerable<string> files)
        {
            List<string> largeFiles = new List<string>();
            List<string> okFiles = new List<string>();

            foreach (var file in files)
            {
                FileInfo f = new FileInfo(file);

                if (f.Length > 2147483647) largeFiles.Add(file);
                else okFiles.Add(file);
            }

            if (largeFiles.Count > 0)
            {
                string messageBegin = string.Format("O{0} seguinte{0} ficheiro{0} {1} demasiado grande{0}, pelo que não poder{2} ser transferido{0}.", largeFiles.Count == 1 ? "" : "s", largeFiles.Count == 1 ? "é" : "são", largeFiles.Count == 1 ? "á" : "ão");

                string messageFiles = Environment.NewLine + Environment.NewLine;
                foreach (var f in largeFiles)
                {
                    messageFiles += f.Substring(f.LastIndexOf("\\") + 1) + Environment.NewLine;
                }

                string messageEnd = Environment.NewLine + "Deseja continuar o upload?";

                string message = string.Concat(messageBegin, messageFiles, messageEnd);

                if (okFiles.Count == 0)
                {
                    MessageBox.Show(message, "Ficheiros enormes!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show(message, "Ficheiros enormes!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No) return;
            }



            var ftf = new FileTransferForm(this.connection);
            ftf.FormClosed += (s, e) => ftf.Dispose();
            ftf.Uploaded += ftf_FileTransferred;

            ftf.UploadFiles(okFiles);


        }
        void ftf_FileTransferred(object sender, EventArgs e)
        {
            LoadFiles();
        }


        private void buttonRefreshRemoteFiles_Click(object sender, EventArgs e)
        {
            LoadFiles();
        }

        private void listViewRemoteFiles_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem video in (sender as ListView).SelectedItems)
            {
                AddVideoToPlaylist(video.Text, video.Tag as string);
            }
        }

        private void listViewRemoteFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                foreach (ListViewItem video in (sender as ListView).SelectedItems)
                {
                    AddVideoToPlaylist(video.Text, video.Tag as string);
                }
        }


        public void AssignConnection(Connection con)
        {
            connection = con;
        }

        private void AddVideoToPlaylist(string name, string path)
        {
            //if (listViewVideoPlaylist.Items.OfType<ListViewItem>().Where(x => x.Tag.ToString() == path).Count() > 0) return;

            listViewVideoPlaylist.Items.Add(new ListViewItem() { Text = name, Tag = path, ImageKey = "Movie" });

            UpdateConfig();
        }

        private void LoadFiles()
        {
            if (connection != null && connection.State == ClientModel.ConnectionState.Open)
            {
                var remoteFiles = connection.GetRemoteVideoFileNames();

                listViewRemoteFiles.Items.Clear();

                foreach (var file in remoteFiles)
                {
                    string fileName = file.Substring(file.LastIndexOf("\\") + 1);

                    listViewRemoteFiles.Items.Add(new ListViewItem() { Text = fileName, Tag = file, ImageKey = "Movie" });
                }
            }
        }

        private void PopulateLocalVideoList(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            listViewLocalFiles.Clear();


            foreach (var file in dir.EnumerateFiles().Where(x=>FileVideoPlayer.SupportedVideoExtensions().Contains(x.Extension)))
            {
                listViewLocalFiles.Items.Add(new ListViewItem() 
                { 
                    Text = file.Name, 
                    Tag = file.FullName,
                    ImageKey = "Movie"
                });
            }

            //listViewLocalFiles
        }

        private void RemoveListBoxPlaylistItems()
        {
            try
            {
                if (listViewVideoPlaylist.Items.Count < 1) return;

                int selectedIndex = listViewVideoPlaylist.SelectedIndices.Cast<int>().Min();
                var selectedItems = listViewVideoPlaylist.SelectedItems.OfType<ListViewItem>().ToList();

                foreach (var item in selectedItems)
                {
                    listViewVideoPlaylist.Items.Remove(item);
                }

                //UpdateFooterTextList();

                if (listViewVideoPlaylist.Items.Count > 0)
                {
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("RemoveListBoxItems" + Environment.NewLine + ex.Message);
#endif
            }

            UpdateConfig();
        }


        public void ApplyChangesToComponent(Components.ComposerComponent component)
        {
            if (!(component is VideoComposer)) return;

            VideoComposer temp = component as VideoComposer;

            configuration.Playlist.DeepCopyTo((temp.Configuration as VideoConfiguration).Playlist);
            (temp.Configuration as VideoConfiguration).Aspect = this.configuration.Aspect;
            (temp.Configuration as VideoConfiguration).Replay = this.configuration.Replay;
        }

        private void UpdateConfig()
        {
            configuration.Playlist.Clear();

            foreach (var video in listViewVideoPlaylist.Items.OfType<ListViewItem>().Select(x=>x.Tag.ToString()))
            {
                configuration.Playlist.Add(video);
            }

            if (radioButtonCenter.Checked) this.configuration.Aspect = FileVideoPlayer.AspectMode.Center;
            else if (radioButtonFill.Checked) this.configuration.Aspect = FileVideoPlayer.AspectMode.Fill;
            else if (radioButtonFit.Checked) this.configuration.Aspect = FileVideoPlayer.AspectMode.Fit;
            else if (radioButtonStretch.Checked) this.configuration.Aspect = FileVideoPlayer.AspectMode.Center;

            this.configuration.Replay = checkBoxReplay.Checked;
        }

        private void buttonRemoveItems_Click(object sender, EventArgs e)
        {
            RemoveListBoxPlaylistItems();
        }

        #region Aspect

        private void radioButtonFill_CheckedChanged(object sender, EventArgs e)
        {
            if((sender as RadioButton).Checked) SetAspectMode(FileVideoPlayer.AspectMode.Fill);
        }
        private void radioButtonFit_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked) SetAspectMode(FileVideoPlayer.AspectMode.Fit);
        }
        private void radioButtonStretch_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked) SetAspectMode(FileVideoPlayer.AspectMode.Stretch);
        }
        private void radioButtonCenter_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked) SetAspectMode(FileVideoPlayer.AspectMode.Center);
        }

        private void pictureBoxFill_Click(object sender, EventArgs e)
        {
            radioButtonFill.PerformClick();
        }
        private void pictureBoxFit_Click(object sender, EventArgs e)
        {
            radioButtonFit.PerformClick();
        }
        private void pictureBoxStretch_Click(object sender, EventArgs e)
        {
            radioButtonStretch.PerformClick();
        }
        private void pictureBoxCenter_Click(object sender, EventArgs e)
        {
            radioButtonCenter.PerformClick();
        }


        /// <summary>
        /// Sets the aspect mode in the configuration
        /// </summary>
        /// <param name="mode"></param>
        private void SetAspectMode(FileVideoPlayer.AspectMode mode)
        {
            this.configuration.Aspect = mode;
        }
        /// <summary>
        /// Updates the aspect mode in the GUI from the configuration
        /// </summary>
        private void UpdateAspectMode()
        {
            switch (this.configuration.Aspect)
            {
                case FileVideoPlayer.AspectMode.Center:
                    radioButtonCenter.Checked = true;
                    break;
                case FileVideoPlayer.AspectMode.Fill:
                    radioButtonFill.Checked = true;
                    break;
                case FileVideoPlayer.AspectMode.Fit:
                    radioButtonFit.Checked = true;
                    break;
                case FileVideoPlayer.AspectMode.Stretch:
                    radioButtonStretch.Checked = true;
                    break;
                default:
                    break;
            }
        }

        #endregion


        private void checkBoxReplay_CheckedChanged(object sender, EventArgs e)
        {
            this.configuration.Replay = (sender as CheckBox).Checked;
        }
    }
}
