namespace Server.View
{
    partial class ListeningForm
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelIP = new System.Windows.Forms.Label();
            this.Porta = new System.Windows.Forms.Label();
            this.textBoxServerIP = new System.Windows.Forms.TextBox();
            this.textBoxServerPort = new System.Windows.Forms.TextBox();
            this.groupBoxServer = new System.Windows.Forms.GroupBox();
            this.groupBoxLocal = new System.Windows.Forms.GroupBox();
            this.buttonRandomize = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLocalIP = new System.Windows.Forms.TextBox();
            this.textBoxLocalPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.labelEstado = new System.Windows.Forms.Label();
            this.groupBoxEstado = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.definiçõesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ligaçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxServer.SuspendLayout();
            this.groupBoxLocal.SuspendLayout();
            this.groupBoxLog.SuspendLayout();
            this.groupBoxEstado.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(91, 65);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Ligar";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(24, 16);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(17, 13);
            this.labelIP.TabIndex = 1;
            this.labelIP.Text = "IP";
            // 
            // Porta
            // 
            this.Porta.AutoSize = true;
            this.Porta.Location = new System.Drawing.Point(6, 42);
            this.Porta.Name = "Porta";
            this.Porta.Size = new System.Drawing.Size(32, 13);
            this.Porta.TabIndex = 2;
            this.Porta.Text = "Porta";
            // 
            // textBoxServerIP
            // 
            this.textBoxServerIP.Location = new System.Drawing.Point(47, 13);
            this.textBoxServerIP.Name = "textBoxServerIP";
            this.textBoxServerIP.Size = new System.Drawing.Size(119, 20);
            this.textBoxServerIP.TabIndex = 3;
            this.textBoxServerIP.Text = "clicenter-pl.no-ip.org";
            // 
            // textBoxServerPort
            // 
            this.textBoxServerPort.Location = new System.Drawing.Point(47, 39);
            this.textBoxServerPort.Name = "textBoxServerPort";
            this.textBoxServerPort.Size = new System.Drawing.Size(119, 20);
            this.textBoxServerPort.TabIndex = 4;
            this.textBoxServerPort.Text = "9021";
            // 
            // groupBoxServer
            // 
            this.groupBoxServer.Controls.Add(this.labelIP);
            this.groupBoxServer.Controls.Add(this.buttonConnect);
            this.groupBoxServer.Controls.Add(this.textBoxServerPort);
            this.groupBoxServer.Controls.Add(this.Porta);
            this.groupBoxServer.Controls.Add(this.textBoxServerIP);
            this.groupBoxServer.Location = new System.Drawing.Point(12, 128);
            this.groupBoxServer.Name = "groupBoxServer";
            this.groupBoxServer.Size = new System.Drawing.Size(172, 95);
            this.groupBoxServer.TabIndex = 5;
            this.groupBoxServer.TabStop = false;
            this.groupBoxServer.Text = "Servidor";
            // 
            // groupBoxLocal
            // 
            this.groupBoxLocal.Controls.Add(this.buttonRandomize);
            this.groupBoxLocal.Controls.Add(this.label1);
            this.groupBoxLocal.Controls.Add(this.textBoxLocalIP);
            this.groupBoxLocal.Controls.Add(this.textBoxLocalPort);
            this.groupBoxLocal.Controls.Add(this.label2);
            this.groupBoxLocal.Location = new System.Drawing.Point(12, 27);
            this.groupBoxLocal.Name = "groupBoxLocal";
            this.groupBoxLocal.Size = new System.Drawing.Size(172, 95);
            this.groupBoxLocal.TabIndex = 6;
            this.groupBoxLocal.TabStop = false;
            this.groupBoxLocal.Text = "Local";
            // 
            // buttonRandomize
            // 
            this.buttonRandomize.Location = new System.Drawing.Point(91, 65);
            this.buttonRandomize.Name = "buttonRandomize";
            this.buttonRandomize.Size = new System.Drawing.Size(75, 23);
            this.buttonRandomize.TabIndex = 7;
            this.buttonRandomize.Text = "Aleatória";
            this.buttonRandomize.UseVisualStyleBackColor = true;
            this.buttonRandomize.Click += new System.EventHandler(this.buttonRandomize_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "IP";
            // 
            // textBoxLocalIP
            // 
            this.textBoxLocalIP.Location = new System.Drawing.Point(47, 13);
            this.textBoxLocalIP.Name = "textBoxLocalIP";
            this.textBoxLocalIP.ReadOnly = true;
            this.textBoxLocalIP.Size = new System.Drawing.Size(119, 20);
            this.textBoxLocalIP.TabIndex = 7;
            // 
            // textBoxLocalPort
            // 
            this.textBoxLocalPort.Location = new System.Drawing.Point(47, 39);
            this.textBoxLocalPort.Name = "textBoxLocalPort";
            this.textBoxLocalPort.Size = new System.Drawing.Size(119, 20);
            this.textBoxLocalPort.TabIndex = 8;
            this.textBoxLocalPort.Text = "4848";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Porta";
            // 
            // groupBoxLog
            // 
            this.groupBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLog.Controls.Add(this.textBoxLog);
            this.groupBoxLog.Location = new System.Drawing.Point(190, 27);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Size = new System.Drawing.Size(517, 387);
            this.groupBoxLog.TabIndex = 7;
            this.groupBoxLog.TabStop = false;
            this.groupBoxLog.Text = "Log";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBoxLog.Location = new System.Drawing.Point(6, 19);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.Size = new System.Drawing.Size(505, 361);
            this.textBoxLog.TabIndex = 0;
            // 
            // labelEstado
            // 
            this.labelEstado.AutoSize = true;
            this.labelEstado.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEstado.ForeColor = System.Drawing.Color.Blue;
            this.labelEstado.Location = new System.Drawing.Point(51, 30);
            this.labelEstado.Name = "labelEstado";
            this.labelEstado.Size = new System.Drawing.Size(75, 25);
            this.labelEstado.TabIndex = 8;
            this.labelEstado.Text = "Pronto";
            // 
            // groupBoxEstado
            // 
            this.groupBoxEstado.Controls.Add(this.labelEstado);
            this.groupBoxEstado.Location = new System.Drawing.Point(12, 229);
            this.groupBoxEstado.Name = "groupBoxEstado";
            this.groupBoxEstado.Size = new System.Drawing.Size(172, 80);
            this.groupBoxEstado.TabIndex = 9;
            this.groupBoxEstado.TabStop = false;
            this.groupBoxEstado.Text = "Estado";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.definiçõesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(719, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // definiçõesToolStripMenuItem
            // 
            this.definiçõesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ligaçãoToolStripMenuItem,
            this.sairToolStripMenuItem});
            this.definiçõesToolStripMenuItem.Name = "definiçõesToolStripMenuItem";
            this.definiçõesToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.definiçõesToolStripMenuItem.Text = "Definições";
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sairToolStripMenuItem.Text = "Sair";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            // 
            // ligaçãoToolStripMenuItem
            // 
            this.ligaçãoToolStripMenuItem.Name = "ligaçãoToolStripMenuItem";
            this.ligaçãoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ligaçãoToolStripMenuItem.Text = "Ligação";
            // 
            // ListeningForm
            // 
            this.AcceptButton = this.buttonConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 423);
            this.Controls.Add(this.groupBoxEstado);
            this.Controls.Add(this.groupBoxLog);
            this.Controls.Add(this.groupBoxLocal);
            this.Controls.Add(this.groupBoxServer);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "ListeningForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Ligação Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ListeningForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ListeningForm_FormClosed);
            this.groupBoxServer.ResumeLayout(false);
            this.groupBoxServer.PerformLayout();
            this.groupBoxLocal.ResumeLayout(false);
            this.groupBoxLocal.PerformLayout();
            this.groupBoxLog.ResumeLayout(false);
            this.groupBoxLog.PerformLayout();
            this.groupBoxEstado.ResumeLayout(false);
            this.groupBoxEstado.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Label Porta;
        private System.Windows.Forms.TextBox textBoxServerIP;
        private System.Windows.Forms.TextBox textBoxServerPort;
        private System.Windows.Forms.GroupBox groupBoxServer;
        private System.Windows.Forms.GroupBox groupBoxLocal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLocalIP;
        private System.Windows.Forms.TextBox textBoxLocalPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonRandomize;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label labelEstado;
        private System.Windows.Forms.GroupBox groupBoxEstado;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem definiçõesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ligaçãoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
    }
}