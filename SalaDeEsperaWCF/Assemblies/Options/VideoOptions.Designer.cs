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
            this.groupBoxLocal = new System.Windows.Forms.GroupBox();
            this.buttonEnviar = new System.Windows.Forms.Button();
            this.listViewLocalFiles = new System.Windows.Forms.ListView();
            this.imageListVideo = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.groupBoxRemote = new System.Windows.Forms.GroupBox();
            this.groupBoxPlaylist = new System.Windows.Forms.GroupBox();
            this.buttonRemoveItems = new System.Windows.Forms.Button();
            this.buttonMoveItemsDown = new System.Windows.Forms.Button();
            this.buttonMoveItemsUp = new System.Windows.Forms.Button();
            this.groupBoxRemoteFiles = new System.Windows.Forms.GroupBox();
            this.buttonRefreshRemoteFiles = new System.Windows.Forms.Button();
            this.listViewRemoteFiles = new System.Windows.Forms.ListView();
            this.listViewVideoPlaylist = new System.Windows.Forms.ListView();
            this.groupBoxLocal.SuspendLayout();
            this.groupBoxRemote.SuspendLayout();
            this.groupBoxPlaylist.SuspendLayout();
            this.groupBoxRemoteFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxLocal
            // 
            this.groupBoxLocal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxLocal.Controls.Add(this.buttonEnviar);
            this.groupBoxLocal.Controls.Add(this.listViewLocalFiles);
            this.groupBoxLocal.Controls.Add(this.button1);
            this.groupBoxLocal.Controls.Add(this.textBoxPath);
            this.groupBoxLocal.Location = new System.Drawing.Point(12, 12);
            this.groupBoxLocal.Name = "groupBoxLocal";
            this.groupBoxLocal.Size = new System.Drawing.Size(271, 587);
            this.groupBoxLocal.TabIndex = 0;
            this.groupBoxLocal.TabStop = false;
            this.groupBoxLocal.Text = "Local";
            // 
            // buttonEnviar
            // 
            this.buttonEnviar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEnviar.Location = new System.Drawing.Point(190, 558);
            this.buttonEnviar.Name = "buttonEnviar";
            this.buttonEnviar.Size = new System.Drawing.Size(75, 23);
            this.buttonEnviar.TabIndex = 3;
            this.buttonEnviar.Text = "Upload";
            this.buttonEnviar.UseVisualStyleBackColor = true;
            this.buttonEnviar.Click += new System.EventHandler(this.buttonEnviar_Click);
            // 
            // listViewLocalFiles
            // 
            this.listViewLocalFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLocalFiles.Location = new System.Drawing.Point(6, 45);
            this.listViewLocalFiles.Name = "listViewLocalFiles";
            this.listViewLocalFiles.Size = new System.Drawing.Size(259, 507);
            this.listViewLocalFiles.SmallImageList = this.imageListVideo;
            this.listViewLocalFiles.TabIndex = 2;
            this.listViewLocalFiles.UseCompatibleStateImageBehavior = false;
            this.listViewLocalFiles.View = System.Windows.Forms.View.List;
            // 
            // imageListVideo
            // 
            this.imageListVideo.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListVideo.ImageStream")));
            this.imageListVideo.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListVideo.Images.SetKeyName(0, "Movie");
            this.imageListVideo.Images.SetKeyName(1, "Folder");
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.ImageKey = "Folder";
            this.button1.ImageList = this.imageListVideo;
            this.button1.Location = new System.Drawing.Point(237, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(28, 23);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.textBoxPath.Location = new System.Drawing.Point(6, 19);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(225, 20);
            this.textBoxPath.TabIndex = 0;
            this.textBoxPath.TextChanged += new System.EventHandler(this.textBoxPath_TextChanged);
            // 
            // groupBoxRemote
            // 
            this.groupBoxRemote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRemote.Controls.Add(this.groupBoxPlaylist);
            this.groupBoxRemote.Controls.Add(this.groupBoxRemoteFiles);
            this.groupBoxRemote.Location = new System.Drawing.Point(360, 12);
            this.groupBoxRemote.Name = "groupBoxRemote";
            this.groupBoxRemote.Size = new System.Drawing.Size(322, 587);
            this.groupBoxRemote.TabIndex = 1;
            this.groupBoxRemote.TabStop = false;
            this.groupBoxRemote.Text = "Remoto";
            // 
            // groupBoxPlaylist
            // 
            this.groupBoxPlaylist.Controls.Add(this.listViewVideoPlaylist);
            this.groupBoxPlaylist.Controls.Add(this.buttonRemoveItems);
            this.groupBoxPlaylist.Controls.Add(this.buttonMoveItemsDown);
            this.groupBoxPlaylist.Controls.Add(this.buttonMoveItemsUp);
            this.groupBoxPlaylist.Location = new System.Drawing.Point(5, 24);
            this.groupBoxPlaylist.Name = "groupBoxPlaylist";
            this.groupBoxPlaylist.Size = new System.Drawing.Size(308, 248);
            this.groupBoxPlaylist.TabIndex = 14;
            this.groupBoxPlaylist.TabStop = false;
            this.groupBoxPlaylist.Text = "Playlist";
            // 
            // buttonRemoveItems
            // 
            this.buttonRemoveItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveItems.Location = new System.Drawing.Point(281, 74);
            this.buttonRemoveItems.Name = "buttonRemoveItems";
            this.buttonRemoveItems.Size = new System.Drawing.Size(21, 23);
            this.buttonRemoveItems.TabIndex = 16;
            this.buttonRemoveItems.Text = "-";
            this.buttonRemoveItems.UseVisualStyleBackColor = true;
            // 
            // buttonMoveItemsDown
            // 
            this.buttonMoveItemsDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveItemsDown.Location = new System.Drawing.Point(281, 103);
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
            this.buttonMoveItemsUp.Location = new System.Drawing.Point(281, 32);
            this.buttonMoveItemsUp.Name = "buttonMoveItemsUp";
            this.buttonMoveItemsUp.Size = new System.Drawing.Size(21, 36);
            this.buttonMoveItemsUp.TabIndex = 14;
            this.buttonMoveItemsUp.Text = "↑";
            this.buttonMoveItemsUp.UseVisualStyleBackColor = true;
            this.buttonMoveItemsUp.Click += new System.EventHandler(this.buttonMoveItemsUp_Click);
            // 
            // groupBoxRemoteFiles
            // 
            this.groupBoxRemoteFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRemoteFiles.Controls.Add(this.listViewRemoteFiles);
            this.groupBoxRemoteFiles.Controls.Add(this.buttonRefreshRemoteFiles);
            this.groupBoxRemoteFiles.Location = new System.Drawing.Point(8, 278);
            this.groupBoxRemoteFiles.Name = "groupBoxRemoteFiles";
            this.groupBoxRemoteFiles.Size = new System.Drawing.Size(310, 303);
            this.groupBoxRemoteFiles.TabIndex = 13;
            this.groupBoxRemoteFiles.TabStop = false;
            this.groupBoxRemoteFiles.Text = "Vídeos remotos";
            // 
            // buttonRefreshRemoteFiles
            // 
            this.buttonRefreshRemoteFiles.Location = new System.Drawing.Point(230, 274);
            this.buttonRefreshRemoteFiles.Name = "buttonRefreshRemoteFiles";
            this.buttonRefreshRemoteFiles.Size = new System.Drawing.Size(75, 23);
            this.buttonRefreshRemoteFiles.TabIndex = 1;
            this.buttonRefreshRemoteFiles.Text = "Actualizar";
            this.buttonRefreshRemoteFiles.UseVisualStyleBackColor = true;
            this.buttonRefreshRemoteFiles.Click += new System.EventHandler(this.buttonRefreshRemoteFiles_Click);
            // 
            // listViewRemoteFiles
            // 
            this.listViewRemoteFiles.Location = new System.Drawing.Point(6, 19);
            this.listViewRemoteFiles.Name = "listViewRemoteFiles";
            this.listViewRemoteFiles.Size = new System.Drawing.Size(298, 249);
            this.listViewRemoteFiles.TabIndex = 2;
            this.listViewRemoteFiles.UseCompatibleStateImageBehavior = false;
            this.listViewRemoteFiles.DoubleClick += new System.EventHandler(this.listViewRemoteFiles_DoubleClick);
            this.listViewRemoteFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewRemoteFiles_KeyDown);
            // 
            // listViewVideoPlaylist
            // 
            this.listViewVideoPlaylist.Location = new System.Drawing.Point(5, 0);
            this.listViewVideoPlaylist.Name = "listViewVideoPlaylist";
            this.listViewVideoPlaylist.Size = new System.Drawing.Size(270, 242);
            this.listViewVideoPlaylist.TabIndex = 17;
            this.listViewVideoPlaylist.UseCompatibleStateImageBehavior = false;
            // 
            // VideoOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 611);
            this.Controls.Add(this.groupBoxRemote);
            this.Controls.Add(this.groupBoxLocal);
            this.Name = "VideoOptions";
            this.Text = "VideoOptions";
            this.Load += new System.EventHandler(this.VideoOptions_Load);
            this.groupBoxLocal.ResumeLayout(false);
            this.groupBoxLocal.PerformLayout();
            this.groupBoxRemote.ResumeLayout(false);
            this.groupBoxPlaylist.ResumeLayout(false);
            this.groupBoxRemoteFiles.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxLocal;
        private System.Windows.Forms.Button buttonEnviar;
        private System.Windows.Forms.ListView listViewLocalFiles;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.GroupBox groupBoxRemote;
        private System.Windows.Forms.GroupBox groupBoxPlaylist;
        private System.Windows.Forms.Button buttonRemoveItems;
        private System.Windows.Forms.Button buttonMoveItemsDown;
        private System.Windows.Forms.Button buttonMoveItemsUp;
        private System.Windows.Forms.GroupBox groupBoxRemoteFiles;
        private System.Windows.Forms.ImageList imageListVideo;
        private System.Windows.Forms.Button buttonRefreshRemoteFiles;
        private System.Windows.Forms.ListView listViewVideoPlaylist;
        private System.Windows.Forms.ListView listViewRemoteFiles;
    }
}