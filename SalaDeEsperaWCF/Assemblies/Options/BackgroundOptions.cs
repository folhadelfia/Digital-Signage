using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assemblies.Options
{
    public partial class BackgroundOptions : Form
    {
        private const string _DEFAULT_TEXTBOX_PATH = "Procurar...";
        private const string _PT_NONE = "Nenhum";
        private const string _PT_TILE = "Mosaico";
        private const string _PT_CENTER = "Centrar";
        private const string _PT_STRETCH = "Esticar";
        private const string _PT_ZOOM = "Ajustar";

        public string SelectedPath { get; private set; }
        public override ImageLayout BackgroundImageLayout { get { return panelBackground.BackgroundImageLayout; } set { } }
        private string SelectedFolder { get; set; }

        public BackgroundOptions()
        {
            InitializeComponent();

            #region Lê os codecs de abertura de imagem do computador e define o filtro do openfiledialog. Usar com openfiledialog
            //string filtro = "";
            //string sep = string.Empty;

            //foreach (var info in ImageCodecInfo.GetImageDecoders())
            //{
            //    if (info.FilenameExtension.ToLower().Contains("ico")) continue;

            //    filtro = string.Format("{0}{1}{2} ({3})|{3}", filtro, sep, info.CodecName.Substring(8).Replace("Codec", "Files").Trim(), info.FilenameExtension.ToLower());
            //    sep = "|";
            //}

            //this.openFileDialogBackground.Filter = filtro;
            #endregion

            foreach (var value in Assemblies.Toolkit.EnumUtilities.GetValues<ImageLayout>())
            {
                switch (value)
                {
                    case ImageLayout.None: toolStripComboBoxImageStyle.Items.Add(_PT_NONE);
                        break;
                    case ImageLayout.Tile: toolStripComboBoxImageStyle.Items.Add(_PT_TILE);
                        break;
                    case ImageLayout.Center: toolStripComboBoxImageStyle.Items.Add(_PT_CENTER);
                        break;
                    case ImageLayout.Stretch: toolStripComboBoxImageStyle.Items.Add(_PT_STRETCH);
                        break;
                    case ImageLayout.Zoom: toolStripComboBoxImageStyle.Items.Add(_PT_ZOOM);
                        break;
                    default: toolStripComboBoxImageStyle.Items.Add(value);
                        break;
                }
            }

            toolStripComboBoxImageStyle.SelectedIndex = 0;
        }

        #region ToolStrip Ficheiros

        #region ToolStripTextBox
        private void toolStripTextBoxPath_Leave(object sender, EventArgs e)
        {
            if ((sender as ToolStripTextBox).Text == string.Empty)
            {
                (sender as ToolStripTextBox).Text = _DEFAULT_TEXTBOX_PATH;
            }
        }
        private void toolStripTextBoxPath_Enter(object sender, EventArgs e)
        {
            if ((sender as ToolStripTextBox).Text == _DEFAULT_TEXTBOX_PATH)
            {
                (sender as ToolStripTextBox).Text = string.Empty;
            }

            (sender as ToolStripTextBox).ForeColor = SystemColors.ControlText;
        }
        private void toolStripTextBoxPath_TextChanged(object sender, EventArgs e)
        {
            if ((sender as ToolStripTextBox).Text == _DEFAULT_TEXTBOX_PATH)
            {
                (sender as ToolStripTextBox).ForeColor = SystemColors.ControlDark;
                return;
            }
            else
                if ((sender as ToolStripTextBox).ForeColor != SystemColors.ControlText) (sender as ToolStripTextBox).ForeColor = SystemColors.ControlText;

            try
            {
                Path.GetFullPath((sender as ToolStripTextBox).Text);
                //Se passar é porque o caminho existe

                SelectedFolder = (sender as ToolStripTextBox).Text;
                LoadFiles(SelectedFolder);
            }
            catch
            {
            }

        }
        #endregion

        #region Botões
        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.folderBrowserDialogBackground.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.SelectedFolder = folderBrowserDialogBackground.SelectedPath;
                    LoadFiles(SelectedFolder);
                }
            }
            catch
            {
            }
        }
        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            LoadFiles(SelectedFolder);
        }
        #endregion
        #endregion

        #region ToolStrip Background
        private void toolStripComboBoxImageStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox temp = sender as ToolStripComboBox;

            panelBackground.BackgroundImageLayout = (ImageLayout)temp.SelectedIndex;
        }
        #endregion

        #region ListView Backgrounds

        #region Menu de contexto
        private void viewContextMenuItem_Click(object sender, EventArgs e)
        {
            listViewBackgrounds.View = ((sender as ToolStripDropDownItem).Tag as View?) ?? View.Tile;
        }
        #endregion

        #region Teclado
        private void listViewBackgrounds_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) LoadFiles(SelectedFolder);
        }
        #endregion

        private void listViewBackgrounds_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                panelBackground.BackgroundImage = Image.FromFile(listViewBackgrounds.SelectedItems[0].Tag.ToString());
                SelectedPath = listViewBackgrounds.SelectedItems[0].Tag.ToString();
            }
            catch
            {
            }
        }

        #endregion

        /// <summary>
        /// Carrega os ficheiros da pasta seleccionada
        /// </summary>
        private void LoadFiles(string selectedFolder)
        {
            try
            {
                if (!string.IsNullOrEmpty(selectedFolder))
                {
                    DirectoryInfo dir = new DirectoryInfo(selectedFolder);

                    listViewBackgrounds.Items.Clear();
                    smallImageListBackgrounds.Images.Clear();
                    largeImageListBackgrounds.Images.Clear();

                    int imgIndex = 0;

                    foreach (FileInfo file in dir.EnumerateFiles().Where(x => x.Extension.Contains("jpg") || x.Extension.Contains("png") || x.Extension.Contains("bmp")).OrderBy(x => x.Name))
                    {
                        smallImageListBackgrounds.Images.Add(Image.FromFile(file.FullName));
                        largeImageListBackgrounds.Images.Add(Image.FromFile(file.FullName));
                        listViewBackgrounds.Items.Add(new ListViewItem(file.Name, imgIndex++) { Tag = file.FullName });
                    }
                }
            }
            catch
            {
            }
        }
    }
}
