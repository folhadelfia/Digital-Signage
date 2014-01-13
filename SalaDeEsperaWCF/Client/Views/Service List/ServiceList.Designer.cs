namespace Client
{
    partial class ServiceList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceList));
            this.groupBoxPCS = new System.Windows.Forms.GroupBox();
            this.treeViewRede = new System.Windows.Forms.TreeView();
            this.imageListServiceList = new System.Windows.Forms.ImageList(this.components);
            this.buttonOk = new System.Windows.Forms.Button();
            this.menuStripServiceList = new System.Windows.Forms.MenuStrip();
            this.fICHEIROToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonScan = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.progressBarScan = new System.Windows.Forms.ProgressBar();
            this.groupBoxPCS.SuspendLayout();
            this.menuStripServiceList.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxPCS
            // 
            this.groupBoxPCS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPCS.Controls.Add(this.progressBarScan);
            this.groupBoxPCS.Controls.Add(this.treeViewRede);
            this.groupBoxPCS.Location = new System.Drawing.Point(12, 27);
            this.groupBoxPCS.Name = "groupBoxPCS";
            this.groupBoxPCS.Size = new System.Drawing.Size(339, 188);
            this.groupBoxPCS.TabIndex = 1;
            this.groupBoxPCS.TabStop = false;
            this.groupBoxPCS.Text = "Computadores disponíveis";
            // 
            // treeViewRede
            // 
            this.treeViewRede.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewRede.ImageIndex = 0;
            this.treeViewRede.ImageList = this.imageListServiceList;
            this.treeViewRede.Location = new System.Drawing.Point(3, 16);
            this.treeViewRede.Name = "treeViewRede";
            this.treeViewRede.SelectedImageIndex = 0;
            this.treeViewRede.Size = new System.Drawing.Size(333, 169);
            this.treeViewRede.TabIndex = 0;
            this.treeViewRede.DoubleClick += new System.EventHandler(this.treeViewRede_DoubleClick);
            // 
            // imageListServiceList
            // 
            this.imageListServiceList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListServiceList.ImageStream")));
            this.imageListServiceList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListServiceList.Images.SetKeyName(0, "Monitor");
            this.imageListServiceList.Images.SetKeyName(1, "Computer");
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(195, 221);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // menuStripServiceList
            // 
            this.menuStripServiceList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fICHEIROToolStripMenuItem});
            this.menuStripServiceList.Location = new System.Drawing.Point(0, 0);
            this.menuStripServiceList.Name = "menuStripServiceList";
            this.menuStripServiceList.Size = new System.Drawing.Size(362, 24);
            this.menuStripServiceList.TabIndex = 3;
            this.menuStripServiceList.Text = "menuStrip1";
            // 
            // fICHEIROToolStripMenuItem
            // 
            this.fICHEIROToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurarToolStripMenuItem});
            this.fICHEIROToolStripMenuItem.Name = "fICHEIROToolStripMenuItem";
            this.fICHEIROToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.fICHEIROToolStripMenuItem.Text = "Ferramentas";
            // 
            // configurarToolStripMenuItem
            // 
            this.configurarToolStripMenuItem.Name = "configurarToolStripMenuItem";
            this.configurarToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.configurarToolStripMenuItem.Text = "Configurar";
            this.configurarToolStripMenuItem.Click += new System.EventHandler(this.configurarToolStripMenuItem_Click);
            // 
            // buttonScan
            // 
            this.buttonScan.Location = new System.Drawing.Point(12, 221);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(75, 23);
            this.buttonScan.TabIndex = 4;
            this.buttonScan.Text = "Procurar";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonSearchServices_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(276, 221);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancelar";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // progressBarScan
            // 
            this.progressBarScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarScan.Location = new System.Drawing.Point(3, 175);
            this.progressBarScan.Name = "progressBarScan";
            this.progressBarScan.Size = new System.Drawing.Size(333, 10);
            this.progressBarScan.TabIndex = 1;
            // 
            // ServiceList
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(362, 256);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonScan);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBoxPCS);
            this.Controls.Add(this.menuStripServiceList);
            this.MainMenuStrip = this.menuStripServiceList;
            this.Name = "ServiceList";
            this.ShowInTaskbar = false;
            this.Text = "ServiceList";
            this.groupBoxPCS.ResumeLayout(false);
            this.menuStripServiceList.ResumeLayout(false);
            this.menuStripServiceList.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPCS;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.MenuStrip menuStripServiceList;
        private System.Windows.Forms.ToolStripMenuItem fICHEIROToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurarToolStripMenuItem;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TreeView treeViewRede;
        private System.Windows.Forms.ImageList imageListServiceList;
        private System.Windows.Forms.ProgressBar progressBarScan;
    }
}