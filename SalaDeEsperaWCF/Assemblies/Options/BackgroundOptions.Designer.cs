namespace Assemblies.Options
{
    partial class BackgroundOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackgroundOptions));
            this.groupBoxPreview = new System.Windows.Forms.GroupBox();
            this.toolStripPreview = new System.Windows.Forms.ToolStrip();
            this.toolStripComboBoxImageStyle = new System.Windows.Forms.ToolStripComboBox();
            this.panelBackground = new System.Windows.Forms.Panel();
            this.openFileDialogBackground = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxFiles = new System.Windows.Forms.GroupBox();
            this.toolStripBackgrounds = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBoxPath = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.listViewBackgrounds = new System.Windows.Forms.ListView();
            this.contextMenuStripListViewBackgrounds = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.vistaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iconesGrandesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iconesPequenosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detalhesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mosaicosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largeImageListBackgrounds = new System.Windows.Forms.ImageList(this.components);
            this.smallImageListBackgrounds = new System.Windows.Forms.ImageList(this.components);
            this.folderBrowserDialogBackground = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBoxPreview.SuspendLayout();
            this.toolStripPreview.SuspendLayout();
            this.groupBoxFiles.SuspendLayout();
            this.toolStripBackgrounds.SuspendLayout();
            this.contextMenuStripListViewBackgrounds.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxPreview
            // 
            this.groupBoxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPreview.Controls.Add(this.toolStripPreview);
            this.groupBoxPreview.Controls.Add(this.panelBackground);
            this.groupBoxPreview.Location = new System.Drawing.Point(312, 12);
            this.groupBoxPreview.Name = "groupBoxPreview";
            this.groupBoxPreview.Size = new System.Drawing.Size(476, 311);
            this.groupBoxPreview.TabIndex = 0;
            this.groupBoxPreview.TabStop = false;
            this.groupBoxPreview.Text = "Preview";
            // 
            // toolStripPreview
            // 
            this.toolStripPreview.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripPreview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBoxImageStyle});
            this.toolStripPreview.Location = new System.Drawing.Point(3, 16);
            this.toolStripPreview.Name = "toolStripPreview";
            this.toolStripPreview.Size = new System.Drawing.Size(470, 25);
            this.toolStripPreview.TabIndex = 1;
            this.toolStripPreview.Text = "toolStrip1";
            // 
            // toolStripComboBoxImageStyle
            // 
            this.toolStripComboBoxImageStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxImageStyle.Name = "toolStripComboBoxImageStyle";
            this.toolStripComboBoxImageStyle.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxImageStyle.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxImageStyle_SelectedIndexChanged);
            // 
            // panelBackground
            // 
            this.panelBackground.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBackground.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBackground.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelBackground.Location = new System.Drawing.Point(3, 44);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(470, 264);
            this.panelBackground.TabIndex = 0;
            // 
            // groupBoxFiles
            // 
            this.groupBoxFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxFiles.Controls.Add(this.toolStripBackgrounds);
            this.groupBoxFiles.Controls.Add(this.listViewBackgrounds);
            this.groupBoxFiles.Location = new System.Drawing.Point(12, 12);
            this.groupBoxFiles.Name = "groupBoxFiles";
            this.groupBoxFiles.Size = new System.Drawing.Size(294, 342);
            this.groupBoxFiles.TabIndex = 1;
            this.groupBoxFiles.TabStop = false;
            this.groupBoxFiles.Text = "Imagens";
            // 
            // toolStripBackgrounds
            // 
            this.toolStripBackgrounds.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripBackgrounds.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBoxPath,
            this.toolStripButtonSearch,
            this.toolStripButtonRefresh});
            this.toolStripBackgrounds.Location = new System.Drawing.Point(3, 16);
            this.toolStripBackgrounds.Name = "toolStripBackgrounds";
            this.toolStripBackgrounds.Size = new System.Drawing.Size(288, 25);
            this.toolStripBackgrounds.TabIndex = 1;
            this.toolStripBackgrounds.Text = "Barra de ferramentas";
            // 
            // toolStripTextBoxPath
            // 
            this.toolStripTextBoxPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.toolStripTextBoxPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.toolStripTextBoxPath.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.toolStripTextBoxPath.Name = "toolStripTextBoxPath";
            this.toolStripTextBoxPath.Size = new System.Drawing.Size(230, 25);
            this.toolStripTextBoxPath.Text = "Procurar...";
            this.toolStripTextBoxPath.ToolTipText = "Insira o caminho para as imagens pretendidas";
            this.toolStripTextBoxPath.Enter += new System.EventHandler(this.toolStripTextBoxPath_Enter);
            this.toolStripTextBoxPath.Leave += new System.EventHandler(this.toolStripTextBoxPath_Leave);
            this.toolStripTextBoxPath.TextChanged += new System.EventHandler(this.toolStripTextBoxPath_TextChanged);
            // 
            // toolStripButtonSearch
            // 
            this.toolStripButtonSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSearch.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSearch.Image")));
            this.toolStripButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSearch.Name = "toolStripButtonSearch";
            this.toolStripButtonSearch.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSearch.Text = "Procurar";
            this.toolStripButtonSearch.ToolTipText = "Escolher a pasta com as imagens";
            this.toolStripButtonSearch.Click += new System.EventHandler(this.toolStripButtonSearch_Click);
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRefresh.Text = "toolStripButton1";
            this.toolStripButtonRefresh.ToolTipText = "Actualizar";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.toolStripButtonRefresh_Click);
            // 
            // listViewBackgrounds
            // 
            this.listViewBackgrounds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewBackgrounds.ContextMenuStrip = this.contextMenuStripListViewBackgrounds;
            this.listViewBackgrounds.LargeImageList = this.largeImageListBackgrounds;
            this.listViewBackgrounds.Location = new System.Drawing.Point(6, 44);
            this.listViewBackgrounds.MultiSelect = false;
            this.listViewBackgrounds.Name = "listViewBackgrounds";
            this.listViewBackgrounds.Size = new System.Drawing.Size(282, 292);
            this.listViewBackgrounds.SmallImageList = this.smallImageListBackgrounds;
            this.listViewBackgrounds.TabIndex = 0;
            this.listViewBackgrounds.UseCompatibleStateImageBehavior = false;
            this.listViewBackgrounds.View = System.Windows.Forms.View.Tile;
            this.listViewBackgrounds.SelectedIndexChanged += new System.EventHandler(this.listViewBackgrounds_SelectedIndexChanged);
            this.listViewBackgrounds.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewBackgrounds_KeyDown);
            // 
            // contextMenuStripListViewBackgrounds
            // 
            this.contextMenuStripListViewBackgrounds.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vistaToolStripMenuItem});
            this.contextMenuStripListViewBackgrounds.Name = "contextMenuStripListViewBackgrounds";
            this.contextMenuStripListViewBackgrounds.Size = new System.Drawing.Size(92, 26);
            // 
            // vistaToolStripMenuItem
            // 
            this.vistaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iconesGrandesToolStripMenuItem,
            this.iconesPequenosToolStripMenuItem,
            this.listaToolStripMenuItem,
            this.detalhesToolStripMenuItem,
            this.mosaicosToolStripMenuItem});
            this.vistaToolStripMenuItem.Name = "vistaToolStripMenuItem";
            this.vistaToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.vistaToolStripMenuItem.Text = "Ver";
            // 
            // iconesGrandesToolStripMenuItem
            // 
            this.iconesGrandesToolStripMenuItem.Name = "iconesGrandesToolStripMenuItem";
            this.iconesGrandesToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.iconesGrandesToolStripMenuItem.Tag = System.Windows.Forms.View.LargeIcon;
            this.iconesGrandesToolStripMenuItem.Text = "Ícones grandes";
            this.iconesGrandesToolStripMenuItem.Click += new System.EventHandler(this.viewContextMenuItem_Click);
            // 
            // iconesPequenosToolStripMenuItem
            // 
            this.iconesPequenosToolStripMenuItem.Name = "iconesPequenosToolStripMenuItem";
            this.iconesPequenosToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.iconesPequenosToolStripMenuItem.Tag = System.Windows.Forms.View.SmallIcon;
            this.iconesPequenosToolStripMenuItem.Text = "Ícones pequenos";
            this.iconesPequenosToolStripMenuItem.Click += new System.EventHandler(this.viewContextMenuItem_Click);
            // 
            // listaToolStripMenuItem
            // 
            this.listaToolStripMenuItem.Name = "listaToolStripMenuItem";
            this.listaToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.listaToolStripMenuItem.Tag = System.Windows.Forms.View.List;
            this.listaToolStripMenuItem.Text = "Lista";
            this.listaToolStripMenuItem.Click += new System.EventHandler(this.viewContextMenuItem_Click);
            // 
            // detalhesToolStripMenuItem
            // 
            this.detalhesToolStripMenuItem.Name = "detalhesToolStripMenuItem";
            this.detalhesToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.detalhesToolStripMenuItem.Tag = System.Windows.Forms.View.Details;
            this.detalhesToolStripMenuItem.Text = "Detalhes";
            this.detalhesToolStripMenuItem.Click += new System.EventHandler(this.viewContextMenuItem_Click);
            // 
            // mosaicosToolStripMenuItem
            // 
            this.mosaicosToolStripMenuItem.Name = "mosaicosToolStripMenuItem";
            this.mosaicosToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.mosaicosToolStripMenuItem.Tag = System.Windows.Forms.View.Tile;
            this.mosaicosToolStripMenuItem.Text = "Mosaicos";
            this.mosaicosToolStripMenuItem.Click += new System.EventHandler(this.viewContextMenuItem_Click);
            // 
            // largeImageListBackgrounds
            // 
            this.largeImageListBackgrounds.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.largeImageListBackgrounds.ImageSize = new System.Drawing.Size(64, 64);
            this.largeImageListBackgrounds.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // smallImageListBackgrounds
            // 
            this.smallImageListBackgrounds.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.smallImageListBackgrounds.ImageSize = new System.Drawing.Size(16, 16);
            this.smallImageListBackgrounds.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // folderBrowserDialogBackground
            // 
            this.folderBrowserDialogBackground.Description = "Escolha a pasta que contém as imagens de fundo pretendidas";
            this.folderBrowserDialogBackground.ShowNewFolderButton = false;
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelar.Location = new System.Drawing.Point(710, 331);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelar.TabIndex = 2;
            this.buttonCancelar.Text = "Cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(629, 331);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // BackgroundOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancelar;
            this.ClientSize = new System.Drawing.Size(800, 366);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.groupBoxFiles);
            this.Controls.Add(this.groupBoxPreview);
            this.Name = "BackgroundOptions";
            this.Text = "Opções da imagem de fundo";
            this.groupBoxPreview.ResumeLayout(false);
            this.groupBoxPreview.PerformLayout();
            this.toolStripPreview.ResumeLayout(false);
            this.toolStripPreview.PerformLayout();
            this.groupBoxFiles.ResumeLayout(false);
            this.groupBoxFiles.PerformLayout();
            this.toolStripBackgrounds.ResumeLayout(false);
            this.toolStripBackgrounds.PerformLayout();
            this.contextMenuStripListViewBackgrounds.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPreview;
        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.OpenFileDialog openFileDialogBackground;
        private System.Windows.Forms.GroupBox groupBoxFiles;
        private System.Windows.Forms.ToolStrip toolStripBackgrounds;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxPath;
        private System.Windows.Forms.ToolStripButton toolStripButtonSearch;
        private System.Windows.Forms.ListView listViewBackgrounds;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogBackground;
        private System.Windows.Forms.ImageList largeImageListBackgrounds;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripListViewBackgrounds;
        private System.Windows.Forms.ToolStripMenuItem vistaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iconesGrandesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iconesPequenosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detalhesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mosaicosToolStripMenuItem;
        private System.Windows.Forms.ImageList smallImageListBackgrounds;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.Button buttonCancelar;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.ToolStrip toolStripPreview;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxImageStyle;
    }
}