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
using Assemblies.Configurations;
using Assemblies.ExtensionMethods;
using VideoPlayer;

namespace Assemblies.Options
{
    public partial class VideoOptions : Form
    {
        Connection connection;

        public List<string> Playlist { get; private set; }

        public VideoOptions()
        {
            InitializeComponent();
        }

        public VideoOptions(VideoConfiguration config) : this()
        {
            config.Playlist.DeepCopyTo(this.Playlist);
        }

        public void AssignConnection(Connection con)
        {
            connection = con;
        }
        #region Mover items na LB http://stackoverflow.com/questions/4796109/how-to-move-item-in-listbox-up-and-down alterado para multi item



        private void buttonMoveItemsUp_Click(object sender, EventArgs e)
        {
            MoveListBoxItemsUp(listBoxPlaylist);
        }

        private void buttonMoveItemsDown_Click(object sender, EventArgs e)
        {
            MoveListBoxItemsDown(listBoxPlaylist);
        }




        private void MoveListBoxItemsUp(object target)
        {
            MoveItem(target as ListBox, -1);
            //UpdateFooterTextList();
        }

        private void MoveListBoxItemsDown(object sender)
        {
            MoveItem(sender as ListBox, 1);
            //UpdateFooterTextList();
        }

        private void MoveItem(ListBox sender, int direction)
        {
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

                    object selected = sender.Items[index];

                    //Só mover os items se a nova posição não estiver seleccionada
                    if (!(sender.SelectedIndices.Contains(newIndex)))
                    {
                        // Removing removable element
                        sender.Items.RemoveAt(index);
                        // Insert it in new position
                        sender.Items.Insert(newIndex, selected);
                        // Restore selection
                        sender.SetSelected(newIndex, true);
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("MoveItem" + Environment.NewLine + "Direction: " + direction + Environment.NewLine + ex.Message);
#endif
            }


        }
        #endregion
        private void RemoveListBoxItems()
        {
            try
            {
                int selectedIndex = listBoxPlaylist.SelectedIndices.Cast<int>().Min();
                List<object> selectedItems = listBoxPlaylist.SelectedItems.Cast<object>().ToList();

                foreach (object item in selectedItems)
                {
                    listBoxPlaylist.Items.Remove(item);
                }

                //UpdateFooterTextList();

                if (listBoxPlaylist.Items.Count > 0)
                {
                    listBoxPlaylist.SelectedIndex = selectedIndex;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("RemoveListBoxItems" + Environment.NewLine + ex.Message);
#endif
            }
        }

        private void VideoOptions_Load(object sender, EventArgs e)
        {
            LoadFiles();
        }

        private void LoadFiles()
        {
            if (connection != null && connection.State == ClientModel.ConnectionState.Open)
            {
                var remoteFiles = connection.GetRemoteVideoFileNames();

                listBoxRemoteFiles.Items.Clear();

                foreach (var file in remoteFiles)
                {
                    string fileName = file.Substring(file.LastIndexOf("\\") + 1);

                    listBoxRemoteFiles.Items.Add(fileName);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
            FileTransferForm ftf = new FileTransferForm(this.connection);

            List<string> files = new List<string>();

            foreach (var file in listViewLocalFiles.SelectedItems.Cast<ListViewItem>().Select(x => x.Tag as string))
            {
                files.Add(file);
            }

            ftf.UploadFiles(files);
        }
    }
}
