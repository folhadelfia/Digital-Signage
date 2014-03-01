namespace Assemblies.Options
{
    partial class VideoOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoOptions));
            this.imageListVideo = new System.Windows.Forms.ImageList(this.components);
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.listViewLocalFiles = new System.Windows.Forms.ListView();
            this.buttonEnviar = new System.Windows.Forms.Button();
            this.groupBoxLocal = new System.Windows.Forms.GroupBox();
            this.groupBoxRemoteFiles = new System.Windows.Forms.GroupBox();
            this.listViewRemoteFiles = new System.Windows.Forms.ListView();
            this.buttonRefreshRemoteFiles = new System.Windows.Forms.Button();
            this.groupBoxPlaylist = new System.Windows.Forms.GroupBox();
            this.checkBoxReplay = new System.Windows.Forms.CheckBox();
            this.listViewVideoPlaylist = new System.Windows.Forms.ListView();
            this.buttonRemoveItems = new System.Windows.Forms.Button();
            this.buttonMoveItemsDown = new System.Windows.Forms.Button();
            this.buttonMoveItemsUp = new System.Windows.Forms.Button();
            this.groupBoxRemote = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.groupBoxAspect = new System.Windows.Forms.GroupBox();
            this.radioButtonCenter = new System.Windows.Forms.RadioButton();
            this.radioButtonStretch = new System.Windows.Forms.RadioButton();
            this.radioButtonFit = new System.Windows.Forms.RadioButton();
            this.radioButtonFill = new System.Windows.Forms.RadioButton();
            this.pictureBoxFit = new System.Windows.Forms.PictureBox();
            this.pictureBoxCenter = new System.Windows.Forms.PictureBox();
            this.pictureBoxStretch = new System.Windows.Forms.PictureBox();
            this.pictureBoxFill = new System.Windows.Forms.PictureBox();
            this.tabPageFiles = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listViewRemoteFilesTab2 = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBoxName = new System.Windows.Forms.GroupBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.groupBoxLocal.SuspendLayout();
            this.groupBoxRemoteFiles.SuspendLayout();
            this.groupBoxPlaylist.SuspendLayout();
            this.groupBoxRemote.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.groupBoxAspect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCenter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStretch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFill)).BeginInit();
            this.tabPageFiles.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxName.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListVideo
            // 
            this.imageListVideo.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListVideo.ImageStream")));
            this.imageListVideo.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListVideo.Images.SetKeyName(0, "Movie");
            this.imageListVideo.Images.SetKeyName(1, "Folder");
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(536, 518);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancelar";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(455, 518);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "Ok";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.textBoxPath.Location = new System.Drawing.Point(6, 19);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(241, 20);
            this.textBoxPath.TabIndex = 0;
            this.textBoxPath.TextChanged += new System.EventHandler(this.textBoxPath_TextChanged);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.ImageKey = "Folder";
            this.buttonSearch.ImageList = this.imageListVideo;
            this.buttonSearch.Location = new System.Drawing.Point(253, 17);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(28, 23);
            this.buttonSearch.TabIndex = 1;
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // listViewLocalFiles
            // 
            this.listViewLocalFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLocalFiles.Location = new System.Drawing.Point(6, 45);
            this.listViewLocalFiles.Name = "listViewLocalFiles";
            this.listViewLocalFiles.Size = new System.Drawing.Size(275, 382);
            this.listViewLocalFiles.SmallImageList = this.imageListVideo;
            this.listViewLocalFiles.TabIndex = 2;
            this.listViewLocalFiles.UseCompatibleStateImageBehavior = false;
            this.listViewLocalFiles.View = System.Windows.Forms.View.List;
            // 
            // buttonEnviar
            // 
            this.buttonEnviar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEnviar.Location = new System.Drawing.Point(206, 433);
            this.buttonEnviar.Name = "buttonEnviar";
            this.buttonEnviar.Size = new System.Drawing.Size(75, 23);
            this.buttonEnviar.TabIndex = 3;
            this.buttonEnviar.Text = "Upload";
            this.buttonEnviar.UseVisualStyleBackColor = true;
            this.buttonEnviar.Click += new System.EventHandler(this.buttonEnviar_Click);
            // 
            // groupBoxLocal
            // 
            this.groupBoxLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLocal.Controls.Add(this.buttonEnviar);
            this.groupBoxLocal.Controls.Add(this.listViewLocalFiles);
            this.groupBoxLocal.Controls.Add(this.buttonSearch);
            this.groupBoxLocal.Controls.Add(this.textBoxPath);
            this.groupBoxLocal.Location = new System.Drawing.Point(6, 6);
            this.groupBoxLocal.Name = "groupBoxLocal";
            this.groupBoxLocal.Size = new System.Drawing.Size(287, 462);
            this.groupBoxLocal.TabIndex = 0;
            this.groupBoxLocal.TabStop = false;
            this.groupBoxLocal.Text = "Local";
            // 
            // groupBoxRemoteFiles
            // 
            this.groupBoxRemoteFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxRemoteFiles.Controls.Add(this.listViewRemoteFiles);
            this.groupBoxRemoteFiles.Controls.Add(this.buttonRefreshRemoteFiles);
            this.groupBoxRemoteFiles.Location = new System.Drawing.Point(10, 19);
            this.groupBoxRemoteFiles.Name = "groupBoxRemoteFiles";
            this.groupBoxRemoteFiles.Size = new System.Drawing.Size(261, 166);
            this.groupBoxRemoteFiles.TabIndex = 13;
            this.groupBoxRemoteFiles.TabStop = false;
            this.groupBoxRemoteFiles.Text = "Vídeos remotos";
            // 
            // listViewRemoteFiles
            // 
            this.listViewRemoteFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewRemoteFiles.LargeImageList = this.imageListVideo;
            this.listViewRemoteFiles.Location = new System.Drawing.Point(6, 19);
            this.listViewRemoteFiles.Name = "listViewRemoteFiles";
            this.listViewRemoteFiles.Size = new System.Drawing.Size(244, 112);
            this.listViewRemoteFiles.SmallImageList = this.imageListVideo;
            this.listViewRemoteFiles.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewRemoteFiles.TabIndex = 2;
            this.listViewRemoteFiles.UseCompatibleStateImageBehavior = false;
            this.listViewRemoteFiles.View = System.Windows.Forms.View.List;
            this.listViewRemoteFiles.DoubleClick += new System.EventHandler(this.listViewRemoteFiles_DoubleClick);
            this.listViewRemoteFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewRemoteFiles_KeyDown);
            // 
            // buttonRefreshRemoteFiles
            // 
            this.buttonRefreshRemoteFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefreshRemoteFiles.Location = new System.Drawing.Point(175, 137);
            this.buttonRefreshRemoteFiles.Name = "buttonRefreshRemoteFiles";
            this.buttonRefreshRemoteFiles.Size = new System.Drawing.Size(75, 23);
            this.buttonRefreshRemoteFiles.TabIndex = 1;
            this.buttonRefreshRemoteFiles.Text = "Actualizar";
            this.buttonRefreshRemoteFiles.UseVisualStyleBackColor = true;
            this.buttonRefreshRemoteFiles.Click += new System.EventHandler(this.buttonRefreshRemoteFiles_Click);
            // 
            // groupBoxPlaylist
            // 
            this.groupBoxPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPlaylist.Controls.Add(this.checkBoxReplay);
            this.groupBoxPlaylist.Controls.Add(this.listViewVideoPlaylist);
            this.groupBoxPlaylist.Controls.Add(this.buttonRemoveItems);
            this.groupBoxPlaylist.Controls.Add(this.buttonMoveItemsDown);
            this.groupBoxPlaylist.Controls.Add(this.buttonMoveItemsUp);
            this.groupBoxPlaylist.Location = new System.Drawing.Point(277, 19);
            this.groupBoxPlaylist.Name = "groupBoxPlaylist";
            this.groupBoxPlaylist.Size = new System.Drawing.Size(299, 166);
            this.groupBoxPlaylist.TabIndex = 14;
            this.groupBoxPlaylist.TabStop = false;
            this.groupBoxPlaylist.Text = "Playlist";
            // 
            // checkBoxReplay
            // 
            this.checkBoxReplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxReplay.AutoSize = true;
            this.checkBoxReplay.Location = new System.Drawing.Point(9, 137);
            this.checkBoxReplay.Name = "checkBoxReplay";
            this.checkBoxReplay.Size = new System.Drawing.Size(94, 17);
            this.checkBoxReplay.TabIndex = 1;
            this.checkBoxReplay.Text = "Repetir playlist";
            this.checkBoxReplay.UseVisualStyleBackColor = true;
            this.checkBoxReplay.CheckedChanged += new System.EventHandler(this.checkBoxReplay_CheckedChanged);
            // 
            // listViewVideoPlaylist
            // 
            this.listViewVideoPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewVideoPlaylist.LargeImageList = this.imageListVideo;
            this.listViewVideoPlaylist.Location = new System.Drawing.Point(9, 19);
            this.listViewVideoPlaylist.Name = "listViewVideoPlaylist";
            this.listViewVideoPlaylist.Size = new System.Drawing.Size(257, 112);
            this.listViewVideoPlaylist.SmallImageList = this.imageListVideo;
            this.listViewVideoPlaylist.TabIndex = 17;
            this.listViewVideoPlaylist.UseCompatibleStateImageBehavior = false;
            this.listViewVideoPlaylist.View = System.Windows.Forms.View.List;
            // 
            // buttonRemoveItems
            // 
            this.buttonRemoveItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveItems.Location = new System.Drawing.Point(272, 61);
            this.buttonRemoveItems.Name = "buttonRemoveItems";
            this.buttonRemoveItems.Size = new System.Drawing.Size(21, 23);
            this.buttonRemoveItems.TabIndex = 16;
            this.buttonRemoveItems.Text = "-";
            this.buttonRemoveItems.UseVisualStyleBackColor = true;
            this.buttonRemoveItems.Click += new System.EventHandler(this.buttonRemoveItems_Click);
            // 
            // buttonMoveItemsDown
            // 
            this.buttonMoveItemsDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveItemsDown.Location = new System.Drawing.Point(272, 90);
            this.buttonMoveItemsDown.Name = "buttonMoveItemsDown";
            this.buttonMoveItemsDown.Size = new System.Drawing.Size(21, 36);
            this.buttonMoveItemsDown.TabIndex = 15;
            this.buttonMoveItemsDown.Text = "↓";
            this.buttonMoveItemsDown.UseVisualStyleBackColor = true;
            this.buttonMoveItemsDown.Click += new System.EventHandler(this.buttonMoveItemsDown_Click);
            // 
            // buttonMoveItemsUp
            // 
            this.buttonMoveItemsUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveItemsUp.Location = new System.Drawing.Point(272, 19);
            this.buttonMoveItemsUp.Name = "buttonMoveItemsUp";
            this.buttonMoveItemsUp.Size = new System.Drawing.Size(21, 36);
            this.buttonMoveItemsUp.TabIndex = 14;
            this.buttonMoveItemsUp.Text = "↑";
            this.buttonMoveItemsUp.UseVisualStyleBackColor = true;
            this.buttonMoveItemsUp.Click += new System.EventHandler(this.buttonMoveItemsUp_Click);
            // 
            // groupBoxRemote
            // 
            this.groupBoxRemote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRemote.Controls.Add(this.groupBoxPlaylist);
            this.groupBoxRemote.Controls.Add(this.groupBoxRemoteFiles);
            this.groupBoxRemote.Location = new System.Drawing.Point(6, 277);
            this.groupBoxRemote.Name = "groupBoxRemote";
            this.groupBoxRemote.Size = new System.Drawing.Size(582, 191);
            this.groupBoxRemote.TabIndex = 1;
            this.groupBoxRemote.TabStop = false;
            this.groupBoxRemote.Text = "Remoto";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageFiles);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(599, 500);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.groupBoxRemote);
            this.tabPageGeneral.Controls.Add(this.groupBoxAspect);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(591, 474);
            this.tabPageGeneral.TabIndex = 1;
            this.tabPageGeneral.Text = "Geral";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBoxAspect
            // 
            this.groupBoxAspect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAspect.Controls.Add(this.groupBoxName);
            this.groupBoxAspect.Controls.Add(this.radioButtonCenter);
            this.groupBoxAspect.Controls.Add(this.radioButtonStretch);
            this.groupBoxAspect.Controls.Add(this.radioButtonFit);
            this.groupBoxAspect.Controls.Add(this.radioButtonFill);
            this.groupBoxAspect.Controls.Add(this.pictureBoxFit);
            this.groupBoxAspect.Controls.Add(this.pictureBoxCenter);
            this.groupBoxAspect.Controls.Add(this.pictureBoxStretch);
            this.groupBoxAspect.Controls.Add(this.pictureBoxFill);
            this.groupBoxAspect.Location = new System.Drawing.Point(6, 7);
            this.groupBoxAspect.Name = "groupBoxAspect";
            this.groupBoxAspect.Size = new System.Drawing.Size(579, 264);
            this.groupBoxAspect.TabIndex = 0;
            this.groupBoxAspect.TabStop = false;
            this.groupBoxAspect.Text = "Aspecto";
            // 
            // radioButtonCenter
            // 
            this.radioButtonCenter.AutoSize = true;
            this.radioButtonCenter.Location = new System.Drawing.Point(74, 169);
            this.radioButtonCenter.Name = "radioButtonCenter";
            this.radioButtonCenter.Size = new System.Drawing.Size(59, 17);
            this.radioButtonCenter.TabIndex = 7;
            this.radioButtonCenter.TabStop = true;
            this.radioButtonCenter.Text = "Centrar";
            this.radioButtonCenter.UseVisualStyleBackColor = true;
            this.radioButtonCenter.CheckedChanged += new System.EventHandler(this.radioButtonCenter_CheckedChanged);
            // 
            // radioButtonStretch
            // 
            this.radioButtonStretch.AutoSize = true;
            this.radioButtonStretch.Location = new System.Drawing.Point(74, 122);
            this.radioButtonStretch.Name = "radioButtonStretch";
            this.radioButtonStretch.Size = new System.Drawing.Size(57, 17);
            this.radioButtonStretch.TabIndex = 6;
            this.radioButtonStretch.TabStop = true;
            this.radioButtonStretch.Text = "Esticar";
            this.radioButtonStretch.UseVisualStyleBackColor = true;
            this.radioButtonStretch.CheckedChanged += new System.EventHandler(this.radioButtonStretch_CheckedChanged);
            // 
            // radioButtonFit
            // 
            this.radioButtonFit.AutoSize = true;
            this.radioButtonFit.Location = new System.Drawing.Point(74, 75);
            this.radioButtonFit.Name = "radioButtonFit";
            this.radioButtonFit.Size = new System.Drawing.Size(57, 17);
            this.radioButtonFit.TabIndex = 5;
            this.radioButtonFit.TabStop = true;
            this.radioButtonFit.Text = "Ajustar";
            this.radioButtonFit.UseVisualStyleBackColor = true;
            this.radioButtonFit.CheckedChanged += new System.EventHandler(this.radioButtonFit_CheckedChanged);
            // 
            // radioButtonFill
            // 
            this.radioButtonFill.AutoSize = true;
            this.radioButtonFill.Location = new System.Drawing.Point(74, 28);
            this.radioButtonFill.Name = "radioButtonFill";
            this.radioButtonFill.Size = new System.Drawing.Size(74, 17);
            this.radioButtonFill.TabIndex = 4;
            this.radioButtonFill.TabStop = true;
            this.radioButtonFill.Text = "Preencher";
            this.radioButtonFill.UseVisualStyleBackColor = true;
            this.radioButtonFill.CheckedChanged += new System.EventHandler(this.radioButtonFill_CheckedChanged);
            // 
            // pictureBoxFit
            // 
            this.pictureBoxFit.Image = global::Assemblies.Properties.Resources.Fit;
            this.pictureBoxFit.Location = new System.Drawing.Point(10, 63);
            this.pictureBoxFit.Name = "pictureBoxFit";
            this.pictureBoxFit.Size = new System.Drawing.Size(54, 41);
            this.pictureBoxFit.TabIndex = 3;
            this.pictureBoxFit.TabStop = false;
            this.pictureBoxFit.Click += new System.EventHandler(this.pictureBoxFit_Click);
            // 
            // pictureBoxCenter
            // 
            this.pictureBoxCenter.Image = global::Assemblies.Properties.Resources.Center;
            this.pictureBoxCenter.Location = new System.Drawing.Point(10, 157);
            this.pictureBoxCenter.Name = "pictureBoxCenter";
            this.pictureBoxCenter.Size = new System.Drawing.Size(54, 41);
            this.pictureBoxCenter.TabIndex = 2;
            this.pictureBoxCenter.TabStop = false;
            this.pictureBoxCenter.Click += new System.EventHandler(this.pictureBoxCenter_Click);
            // 
            // pictureBoxStretch
            // 
            this.pictureBoxStretch.Image = global::Assemblies.Properties.Resources.Stretch;
            this.pictureBoxStretch.Location = new System.Drawing.Point(10, 110);
            this.pictureBoxStretch.Name = "pictureBoxStretch";
            this.pictureBoxStretch.Size = new System.Drawing.Size(54, 41);
            this.pictureBoxStretch.TabIndex = 1;
            this.pictureBoxStretch.TabStop = false;
            this.pictureBoxStretch.Click += new System.EventHandler(this.pictureBoxStretch_Click);
            // 
            // pictureBoxFill
            // 
            this.pictureBoxFill.Image = global::Assemblies.Properties.Resources.Fill;
            this.pictureBoxFill.Location = new System.Drawing.Point(6, 16);
            this.pictureBoxFill.Name = "pictureBoxFill";
            this.pictureBoxFill.Size = new System.Drawing.Size(62, 41);
            this.pictureBoxFill.TabIndex = 0;
            this.pictureBoxFill.TabStop = false;
            this.pictureBoxFill.Click += new System.EventHandler(this.pictureBoxFill_Click);
            // 
            // tabPageFiles
            // 
            this.tabPageFiles.Controls.Add(this.groupBox1);
            this.tabPageFiles.Controls.Add(this.groupBoxLocal);
            this.tabPageFiles.Location = new System.Drawing.Point(4, 22);
            this.tabPageFiles.Name = "tabPageFiles";
            this.tabPageFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFiles.Size = new System.Drawing.Size(591, 474);
            this.tabPageFiles.TabIndex = 0;
            this.tabPageFiles.Text = "Ficheiros";
            this.tabPageFiles.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.listViewRemoteFilesTab2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(299, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 462);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vídeos remotos";
            // 
            // listViewRemoteFilesTab2
            // 
            this.listViewRemoteFilesTab2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewRemoteFilesTab2.LargeImageList = this.imageListVideo;
            this.listViewRemoteFilesTab2.Location = new System.Drawing.Point(6, 19);
            this.listViewRemoteFilesTab2.Name = "listViewRemoteFilesTab2";
            this.listViewRemoteFilesTab2.Size = new System.Drawing.Size(269, 408);
            this.listViewRemoteFilesTab2.SmallImageList = this.imageListVideo;
            this.listViewRemoteFilesTab2.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewRemoteFilesTab2.TabIndex = 2;
            this.listViewRemoteFilesTab2.UseCompatibleStateImageBehavior = false;
            this.listViewRemoteFilesTab2.View = System.Windows.Forms.View.List;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(200, 433);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Actualizar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBoxName
            // 
            this.groupBoxName.Controls.Add(this.textBoxName);
            this.groupBoxName.Enabled = false;
            this.groupBoxName.Location = new System.Drawing.Point(440, 19);
            this.groupBoxName.Name = "groupBoxName";
            this.groupBoxName.Size = new System.Drawing.Size(133, 49);
            this.groupBoxName.TabIndex = 2;
            this.groupBoxName.TabStop = false;
            this.groupBoxName.Text = "Nome";
            this.groupBoxName.Visible = false;
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(6, 19);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(121, 20);
            this.textBoxName.TabIndex = 0;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // VideoOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 553);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.MinimumSize = new System.Drawing.Size(639, 591);
            this.Name = "VideoOptions";
            this.Text = "Opções de vídeo";
            this.Load += new System.EventHandler(this.VideoOptions_Load);
            this.groupBoxLocal.ResumeLayout(false);
            this.groupBoxLocal.PerformLayout();
            this.groupBoxRemoteFiles.ResumeLayout(false);
            this.groupBoxPlaylist.ResumeLayout(false);
            this.groupBoxPlaylist.PerformLayout();
            this.groupBoxRemote.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.groupBoxAspect.ResumeLayout(false);
            this.groupBoxAspect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCenter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStretch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFill)).EndInit();
            this.tabPageFiles.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxName.ResumeLayout(false);
            this.groupBoxName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageListVideo;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.ListView listViewLocalFiles;
        private System.Windows.Forms.Button buttonEnviar;
        private System.Windows.Forms.GroupBox groupBoxLocal;
        private System.Windows.Forms.GroupBox groupBoxRemoteFiles;
        private System.Windows.Forms.ListView listViewRemoteFiles;
        private System.Windows.Forms.Button buttonRefreshRemoteFiles;
        private System.Windows.Forms.GroupBox groupBoxPlaylist;
        private System.Windows.Forms.ListView listViewVideoPlaylist;
        private System.Windows.Forms.Button buttonRemoveItems;
        private System.Windows.Forms.Button buttonMoveItemsDown;
        private System.Windows.Forms.Button buttonMoveItemsUp;
        private System.Windows.Forms.GroupBox groupBoxRemote;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageFiles;
        private System.Windows.Forms.GroupBox groupBoxAspect;
        private System.Windows.Forms.RadioButton radioButtonCenter;
        private System.Windows.Forms.RadioButton radioButtonStretch;
        private System.Windows.Forms.RadioButton radioButtonFit;
        private System.Windows.Forms.RadioButton radioButtonFill;
        private System.Windows.Forms.PictureBox pictureBoxFit;
        private System.Windows.Forms.PictureBox pictureBoxCenter;
        private System.Windows.Forms.PictureBox pictureBoxStretch;
        private System.Windows.Forms.PictureBox pictureBoxFill;
        private System.Windows.Forms.CheckBox checkBoxReplay;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listViewRemoteFilesTab2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBoxName;
        private System.Windows.Forms.TextBox textBoxName;
    }
}