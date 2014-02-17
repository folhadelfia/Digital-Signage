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
            this.buttonRefreshRemoteFiles = new System.Windows.Forms.Button();
            this.listViewRemoteFiles = new System.Windows.Forms.ListView();
            this.groupBoxPlaylist = new System.Windows.Forms.GroupBox();
            this.buttonMoveItemsUp = new System.Windows.Forms.Button();
            this.buttonMoveItemsDown = new System.Windows.Forms.Button();
            this.buttonRemoveItems = new System.Windows.Forms.Button();
            this.listViewVideoPlaylist = new System.Windows.Forms.ListView();
            this.groupBoxRemote = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageFiles = new System.Windows.Forms.TabPage();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.groupBoxLocal.SuspendLayout();
            this.groupBoxRemoteFiles.SuspendLayout();
            this.groupBoxPlaylist.SuspendLayout();
            this.groupBoxRemote.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageFiles.SuspendLayout();
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
            this.groupBoxRemoteFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRemoteFiles.Controls.Add(this.listViewRemoteFiles);
            this.groupBoxRemoteFiles.Controls.Add(this.buttonRefreshRemoteFiles);
            this.groupBoxRemoteFiles.Location = new System.Drawing.Point(6, 225);
            this.groupBoxRemoteFiles.Name = "groupBoxRemoteFiles";
            this.groupBoxRemoteFiles.Size = new System.Drawing.Size(274, 225);
            this.groupBoxRemoteFiles.TabIndex = 13;
            this.groupBoxRemoteFiles.TabStop = false;
            this.groupBoxRemoteFiles.Text = "Vídeos remotos";
            // 
            // buttonRefreshRemoteFiles
            // 
            this.buttonRefreshRemoteFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefreshRemoteFiles.Location = new System.Drawing.Point(188, 196);
            this.buttonRefreshRemoteFiles.Name = "buttonRefreshRemoteFiles";
            this.buttonRefreshRemoteFiles.Size = new System.Drawing.Size(75, 23);
            this.buttonRefreshRemoteFiles.TabIndex = 1;
            this.buttonRefreshRemoteFiles.Text = "Actualizar";
            this.buttonRefreshRemoteFiles.UseVisualStyleBackColor = true;
            this.buttonRefreshRemoteFiles.Click += new System.EventHandler(this.buttonRefreshRemoteFiles_Click);
            // 
            // listViewRemoteFiles
            // 
            this.listViewRemoteFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewRemoteFiles.LargeImageList = this.imageListVideo;
            this.listViewRemoteFiles.Location = new System.Drawing.Point(6, 19);
            this.listViewRemoteFiles.Name = "listViewRemoteFiles";
            this.listViewRemoteFiles.Size = new System.Drawing.Size(257, 171);
            this.listViewRemoteFiles.SmallImageList = this.imageListVideo;
            this.listViewRemoteFiles.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewRemoteFiles.TabIndex = 2;
            this.listViewRemoteFiles.UseCompatibleStateImageBehavior = false;
            this.listViewRemoteFiles.View = System.Windows.Forms.View.List;
            this.listViewRemoteFiles.DoubleClick += new System.EventHandler(this.listViewRemoteFiles_DoubleClick);
            this.listViewRemoteFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewRemoteFiles_KeyDown);
            // 
            // groupBoxPlaylist
            // 
            this.groupBoxPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPlaylist.Controls.Add(this.listViewVideoPlaylist);
            this.groupBoxPlaylist.Controls.Add(this.buttonRemoveItems);
            this.groupBoxPlaylist.Controls.Add(this.buttonMoveItemsDown);
            this.groupBoxPlaylist.Controls.Add(this.buttonMoveItemsUp);
            this.groupBoxPlaylist.Location = new System.Drawing.Point(6, 24);
            this.groupBoxPlaylist.Name = "groupBoxPlaylist";
            this.groupBoxPlaylist.Size = new System.Drawing.Size(274, 173);
            this.groupBoxPlaylist.TabIndex = 14;
            this.groupBoxPlaylist.TabStop = false;
            this.groupBoxPlaylist.Text = "Playlist";
            // 
            // buttonMoveItemsUp
            // 
            this.buttonMoveItemsUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveItemsUp.Location = new System.Drawing.Point(247, 32);
            this.buttonMoveItemsUp.Name = "buttonMoveItemsUp";
            this.buttonMoveItemsUp.Size = new System.Drawing.Size(21, 36);
            this.buttonMoveItemsUp.TabIndex = 14;
            this.buttonMoveItemsUp.Text = "↑";
            this.buttonMoveItemsUp.UseVisualStyleBackColor = true;
            this.buttonMoveItemsUp.Click += new System.EventHandler(this.buttonMoveItemsUp_Click);
            // 
            // buttonMoveItemsDown
            // 
            this.buttonMoveItemsDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveItemsDown.Location = new System.Drawing.Point(247, 103);
            this.buttonMoveItemsDown.Name = "buttonMoveItemsDown";
            this.buttonMoveItemsDown.Size = new System.Drawing.Size(21, 36);
            this.buttonMoveItemsDown.TabIndex = 15;
            this.buttonMoveItemsDown.Text = "↓";
            this.buttonMoveItemsDown.UseVisualStyleBackColor = true;
            this.buttonMoveItemsDown.Click += new System.EventHandler(this.buttonMoveItemsDown_Click);
            // 
            // buttonRemoveItems
            // 
            this.buttonRemoveItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveItems.Location = new System.Drawing.Point(247, 74);
            this.buttonRemoveItems.Name = "buttonRemoveItems";
            this.buttonRemoveItems.Size = new System.Drawing.Size(21, 23);
            this.buttonRemoveItems.TabIndex = 16;
            this.buttonRemoveItems.Text = "-";
            this.buttonRemoveItems.UseVisualStyleBackColor = true;
            this.buttonRemoveItems.Click += new System.EventHandler(this.buttonRemoveItems_Click);
            // 
            // listViewVideoPlaylist
            // 
            this.listViewVideoPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewVideoPlaylist.LargeImageList = this.imageListVideo;
            this.listViewVideoPlaylist.Location = new System.Drawing.Point(9, 19);
            this.listViewVideoPlaylist.Name = "listViewVideoPlaylist";
            this.listViewVideoPlaylist.Size = new System.Drawing.Size(232, 148);
            this.listViewVideoPlaylist.SmallImageList = this.imageListVideo;
            this.listViewVideoPlaylist.TabIndex = 17;
            this.listViewVideoPlaylist.UseCompatibleStateImageBehavior = false;
            this.listViewVideoPlaylist.View = System.Windows.Forms.View.List;
            // 
            // groupBoxRemote
            // 
            this.groupBoxRemote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRemote.Controls.Add(this.groupBoxPlaylist);
            this.groupBoxRemote.Controls.Add(this.groupBoxRemoteFiles);
            this.groupBoxRemote.Location = new System.Drawing.Point(299, 6);
            this.groupBoxRemote.Name = "groupBoxRemote";
            this.groupBoxRemote.Size = new System.Drawing.Size(286, 456);
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
            // tabPageFiles
            // 
            this.tabPageFiles.Controls.Add(this.groupBoxLocal);
            this.tabPageFiles.Controls.Add(this.groupBoxRemote);
            this.tabPageFiles.Location = new System.Drawing.Point(4, 22);
            this.tabPageFiles.Name = "tabPageFiles";
            this.tabPageFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFiles.Size = new System.Drawing.Size(591, 474);
            this.tabPageFiles.TabIndex = 0;
            this.tabPageFiles.Text = "Ficheiros";
            this.tabPageFiles.UseVisualStyleBackColor = true;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(591, 474);
            this.tabPageGeneral.TabIndex = 1;
            this.tabPageGeneral.Text = "Geral";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
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
            this.Text = "VideoOptions";
            this.Load += new System.EventHandler(this.VideoOptions_Load);
            this.groupBoxLocal.ResumeLayout(false);
            this.groupBoxLocal.PerformLayout();
            this.groupBoxRemoteFiles.ResumeLayout(false);
            this.groupBoxPlaylist.ResumeLayout(false);
            this.groupBoxRemote.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageFiles.ResumeLayout(false);
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
    }
}