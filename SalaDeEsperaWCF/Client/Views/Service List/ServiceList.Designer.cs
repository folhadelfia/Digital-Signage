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
            this.groupBoxPCS = new System.Windows.Forms.GroupBox();
            this.listViewPCs = new System.Windows.Forms.ListView();
            this.columnHeaderIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderHostName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDisplays = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonOk = new System.Windows.Forms.Button();
            this.menuStripServiceList = new System.Windows.Forms.MenuStrip();
            this.fICHEIROToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonSearchServices = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxPCS.SuspendLayout();
            this.menuStripServiceList.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxPCS
            // 
            this.groupBoxPCS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPCS.Controls.Add(this.listViewPCs);
            this.groupBoxPCS.Location = new System.Drawing.Point(12, 27);
            this.groupBoxPCS.Name = "groupBoxPCS";
            this.groupBoxPCS.Size = new System.Drawing.Size(339, 188);
            this.groupBoxPCS.TabIndex = 1;
            this.groupBoxPCS.TabStop = false;
            this.groupBoxPCS.Text = "Computadores disponíveis";
            // 
            // listViewPCs
            // 
            this.listViewPCs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderIP,
            this.columnHeaderHostName,
            this.columnHeaderDisplays});
            this.listViewPCs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewPCs.FullRowSelect = true;
            this.listViewPCs.Location = new System.Drawing.Point(3, 16);
            this.listViewPCs.MultiSelect = false;
            this.listViewPCs.Name = "listViewPCs";
            this.listViewPCs.Size = new System.Drawing.Size(333, 169);
            this.listViewPCs.TabIndex = 1;
            this.listViewPCs.UseCompatibleStateImageBehavior = false;
            this.listViewPCs.View = System.Windows.Forms.View.Details;
            this.listViewPCs.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewPCs_ItemSelectionChanged);
            this.listViewPCs.DoubleClick += new System.EventHandler(this.listViewPCs_DoubleClick);
            this.listViewPCs.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.listViewPCs_PreviewKeyDown);
            // 
            // columnHeaderIP
            // 
            this.columnHeaderIP.Text = "IP";
            this.columnHeaderIP.Width = 100;
            // 
            // columnHeaderHostName
            // 
            this.columnHeaderHostName.Text = "Nome do computador";
            this.columnHeaderHostName.Width = 140;
            // 
            // columnHeaderDisplays
            // 
            this.columnHeaderDisplays.Text = "Monitores";
            this.columnHeaderDisplays.Width = 80;
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
            // buttonSearchServices
            // 
            this.buttonSearchServices.Location = new System.Drawing.Point(12, 221);
            this.buttonSearchServices.Name = "buttonSearchServices";
            this.buttonSearchServices.Size = new System.Drawing.Size(75, 23);
            this.buttonSearchServices.TabIndex = 4;
            this.buttonSearchServices.Text = "Procurar";
            this.buttonSearchServices.UseVisualStyleBackColor = true;
            this.buttonSearchServices.Click += new System.EventHandler(this.buttonSearchServices_Click);
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
            // ServiceList
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(362, 256);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSearchServices);
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
        private System.Windows.Forms.Button buttonSearchServices;
        private System.Windows.Forms.ListView listViewPCs;
        private System.Windows.Forms.ColumnHeader columnHeaderIP;
        private System.Windows.Forms.ColumnHeader columnHeaderHostName;
        private System.Windows.Forms.ColumnHeader columnHeaderDisplays;
        private System.Windows.Forms.Button buttonCancel;
    }
}