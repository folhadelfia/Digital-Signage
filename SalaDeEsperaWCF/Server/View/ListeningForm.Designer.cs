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
            this.labelPortaPubPL = new System.Windows.Forms.Label();
            this.textBoxPublicIP = new System.Windows.Forms.TextBox();
            this.textBoxPublicPortPL = new System.Windows.Forms.TextBox();
            this.groupBoxServer = new System.Windows.Forms.GroupBox();
            this.buttonRefreshPublic = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPublicHostname = new System.Windows.Forms.TextBox();
            this.groupBoxLocal = new System.Windows.Forms.GroupBox();
            this.buttonRefreshPrivate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPrivateHostname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPrivateIP = new System.Windows.Forms.TextBox();
            this.textBoxPrivatePortPL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.labelEstado = new System.Windows.Forms.Label();
            this.groupBoxEstado = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.definiçõesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ligaçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxPublicPortFT = new System.Windows.Forms.TextBox();
            this.labelPortaPubTF = new System.Windows.Forms.Label();
            this.textBoxPrivatePortFT = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBoxServer.SuspendLayout();
            this.groupBoxLocal.SuspendLayout();
            this.groupBoxLog.SuspendLayout();
            this.groupBoxEstado.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(192, 379);
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
            this.labelIP.Location = new System.Drawing.Point(59, 16);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(17, 13);
            this.labelIP.TabIndex = 1;
            this.labelIP.Text = "IP";
            // 
            // labelPortaPubPL
            // 
            this.labelPortaPubPL.AutoSize = true;
            this.labelPortaPubPL.Location = new System.Drawing.Point(28, 68);
            this.labelPortaPubPL.Name = "labelPortaPubPL";
            this.labelPortaPubPL.Size = new System.Drawing.Size(48, 13);
            this.labelPortaPubPL.TabIndex = 2;
            this.labelPortaPubPL.Text = "Porta PL";
            // 
            // textBoxPublicIP
            // 
            this.textBoxPublicIP.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPublicIP.Location = new System.Drawing.Point(82, 13);
            this.textBoxPublicIP.Name = "textBoxPublicIP";
            this.textBoxPublicIP.ReadOnly = true;
            this.textBoxPublicIP.Size = new System.Drawing.Size(167, 20);
            this.textBoxPublicIP.TabIndex = 3;
            // 
            // textBoxPublicPortPL
            // 
            this.textBoxPublicPortPL.Location = new System.Drawing.Point(82, 65);
            this.textBoxPublicPortPL.Name = "textBoxPublicPortPL";
            this.textBoxPublicPortPL.Size = new System.Drawing.Size(60, 20);
            this.textBoxPublicPortPL.TabIndex = 4;
            this.textBoxPublicPortPL.Text = "4848";
            // 
            // groupBoxServer
            // 
            this.groupBoxServer.Controls.Add(this.textBoxPublicPortFT);
            this.groupBoxServer.Controls.Add(this.labelPortaPubTF);
            this.groupBoxServer.Controls.Add(this.buttonRefreshPublic);
            this.groupBoxServer.Controls.Add(this.label3);
            this.groupBoxServer.Controls.Add(this.textBoxPublicHostname);
            this.groupBoxServer.Controls.Add(this.labelIP);
            this.groupBoxServer.Controls.Add(this.textBoxPublicPortPL);
            this.groupBoxServer.Controls.Add(this.labelPortaPubPL);
            this.groupBoxServer.Controls.Add(this.textBoxPublicIP);
            this.groupBoxServer.Location = new System.Drawing.Point(12, 166);
            this.groupBoxServer.Name = "groupBoxServer";
            this.groupBoxServer.Size = new System.Drawing.Size(255, 121);
            this.groupBoxServer.TabIndex = 5;
            this.groupBoxServer.TabStop = false;
            this.groupBoxServer.Text = "Externo";
            // 
            // buttonRefreshPublic
            // 
            this.buttonRefreshPublic.Location = new System.Drawing.Point(174, 89);
            this.buttonRefreshPublic.Name = "buttonRefreshPublic";
            this.buttonRefreshPublic.Size = new System.Drawing.Size(75, 23);
            this.buttonRefreshPublic.TabIndex = 12;
            this.buttonRefreshPublic.Text = "Actualizar";
            this.buttonRefreshPublic.UseVisualStyleBackColor = true;
            this.buttonRefreshPublic.Click += new System.EventHandler(this.buttonRefreshPublic_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Hostname";
            // 
            // textBoxPublicHostname
            // 
            this.textBoxPublicHostname.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPublicHostname.Location = new System.Drawing.Point(82, 39);
            this.textBoxPublicHostname.Name = "textBoxPublicHostname";
            this.textBoxPublicHostname.ReadOnly = true;
            this.textBoxPublicHostname.Size = new System.Drawing.Size(167, 20);
            this.textBoxPublicHostname.TabIndex = 6;
            // 
            // groupBoxLocal
            // 
            this.groupBoxLocal.Controls.Add(this.textBoxPrivatePortFT);
            this.groupBoxLocal.Controls.Add(this.label5);
            this.groupBoxLocal.Controls.Add(this.buttonRefreshPrivate);
            this.groupBoxLocal.Controls.Add(this.label4);
            this.groupBoxLocal.Controls.Add(this.textBoxPrivateHostname);
            this.groupBoxLocal.Controls.Add(this.label1);
            this.groupBoxLocal.Controls.Add(this.textBoxPrivateIP);
            this.groupBoxLocal.Controls.Add(this.textBoxPrivatePortPL);
            this.groupBoxLocal.Controls.Add(this.label2);
            this.groupBoxLocal.Location = new System.Drawing.Point(12, 27);
            this.groupBoxLocal.Name = "groupBoxLocal";
            this.groupBoxLocal.Size = new System.Drawing.Size(255, 121);
            this.groupBoxLocal.TabIndex = 6;
            this.groupBoxLocal.TabStop = false;
            this.groupBoxLocal.Text = "Local";
            // 
            // buttonRefreshPrivate
            // 
            this.buttonRefreshPrivate.Location = new System.Drawing.Point(174, 91);
            this.buttonRefreshPrivate.Name = "buttonRefreshPrivate";
            this.buttonRefreshPrivate.Size = new System.Drawing.Size(75, 23);
            this.buttonRefreshPrivate.TabIndex = 11;
            this.buttonRefreshPrivate.Text = "Actualizar";
            this.buttonRefreshPrivate.UseVisualStyleBackColor = true;
            this.buttonRefreshPrivate.Click += new System.EventHandler(this.buttonRefreshPrivate_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Hostname";
            // 
            // textBoxPrivateHostname
            // 
            this.textBoxPrivateHostname.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPrivateHostname.Location = new System.Drawing.Point(82, 39);
            this.textBoxPrivateHostname.Name = "textBoxPrivateHostname";
            this.textBoxPrivateHostname.ReadOnly = true;
            this.textBoxPrivateHostname.Size = new System.Drawing.Size(167, 20);
            this.textBoxPrivateHostname.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "IP";
            // 
            // textBoxPrivateIP
            // 
            this.textBoxPrivateIP.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPrivateIP.Location = new System.Drawing.Point(82, 13);
            this.textBoxPrivateIP.Name = "textBoxPrivateIP";
            this.textBoxPrivateIP.ReadOnly = true;
            this.textBoxPrivateIP.Size = new System.Drawing.Size(167, 20);
            this.textBoxPrivateIP.TabIndex = 7;
            // 
            // textBoxPrivatePortPL
            // 
            this.textBoxPrivatePortPL.Location = new System.Drawing.Point(82, 67);
            this.textBoxPrivatePortPL.Name = "textBoxPrivatePortPL";
            this.textBoxPrivatePortPL.Size = new System.Drawing.Size(60, 20);
            this.textBoxPrivatePortPL.TabIndex = 8;
            this.textBoxPrivatePortPL.Text = "4848";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Porta PL";
            // 
            // groupBoxLog
            // 
            this.groupBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLog.Controls.Add(this.textBoxLog);
            this.groupBoxLog.Location = new System.Drawing.Point(273, 27);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Size = new System.Drawing.Size(434, 378);
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
            this.textBoxLog.Size = new System.Drawing.Size(422, 352);
            this.textBoxLog.TabIndex = 0;
            // 
            // labelEstado
            // 
            this.labelEstado.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelEstado.AutoSize = true;
            this.labelEstado.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEstado.ForeColor = System.Drawing.Color.Blue;
            this.labelEstado.Location = new System.Drawing.Point(92, 30);
            this.labelEstado.Name = "labelEstado";
            this.labelEstado.Size = new System.Drawing.Size(75, 25);
            this.labelEstado.TabIndex = 8;
            this.labelEstado.Text = "Pronto";
            // 
            // groupBoxEstado
            // 
            this.groupBoxEstado.Controls.Add(this.labelEstado);
            this.groupBoxEstado.Location = new System.Drawing.Point(18, 293);
            this.groupBoxEstado.Name = "groupBoxEstado";
            this.groupBoxEstado.Size = new System.Drawing.Size(255, 80);
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
            // ligaçãoToolStripMenuItem
            // 
            this.ligaçãoToolStripMenuItem.Name = "ligaçãoToolStripMenuItem";
            this.ligaçãoToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.ligaçãoToolStripMenuItem.Text = "Ligação";
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.sairToolStripMenuItem.Text = "Sair";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            // 
            // textBoxPublicPortFT
            // 
            this.textBoxPublicPortFT.Location = new System.Drawing.Point(82, 91);
            this.textBoxPublicPortFT.Name = "textBoxPublicPortFT";
            this.textBoxPublicPortFT.Size = new System.Drawing.Size(60, 20);
            this.textBoxPublicPortFT.TabIndex = 14;
            this.textBoxPublicPortFT.Text = "4849";
            // 
            // labelPortaPubTF
            // 
            this.labelPortaPubTF.AutoSize = true;
            this.labelPortaPubTF.Location = new System.Drawing.Point(28, 94);
            this.labelPortaPubTF.Name = "labelPortaPubTF";
            this.labelPortaPubTF.Size = new System.Drawing.Size(48, 13);
            this.labelPortaPubTF.TabIndex = 13;
            this.labelPortaPubTF.Text = "Porta TF";
            // 
            // textBoxPrivatePortFT
            // 
            this.textBoxPrivatePortFT.Location = new System.Drawing.Point(82, 93);
            this.textBoxPrivatePortFT.Name = "textBoxPrivatePortFT";
            this.textBoxPrivatePortFT.Size = new System.Drawing.Size(60, 20);
            this.textBoxPrivatePortFT.TabIndex = 16;
            this.textBoxPrivatePortFT.Text = "4849";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Porta TF";
            // 
            // ListeningForm
            // 
            this.AcceptButton = this.buttonConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 414);
            this.Controls.Add(this.groupBoxEstado);
            this.Controls.Add(this.groupBoxLog);
            this.Controls.Add(this.groupBoxLocal);
            this.Controls.Add(this.buttonConnect);
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
            this.Load += new System.EventHandler(this.ListeningForm_Load);
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
        private System.Windows.Forms.Label labelPortaPubPL;
        private System.Windows.Forms.TextBox textBoxPublicIP;
        private System.Windows.Forms.TextBox textBoxPublicPortPL;
        private System.Windows.Forms.GroupBox groupBoxServer;
        private System.Windows.Forms.GroupBox groupBoxLocal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPrivateIP;
        private System.Windows.Forms.TextBox textBoxPrivatePortPL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label labelEstado;
        private System.Windows.Forms.GroupBox groupBoxEstado;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem definiçõesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ligaçãoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPublicHostname;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPrivateHostname;
        private System.Windows.Forms.Button buttonRefreshPublic;
        private System.Windows.Forms.Button buttonRefreshPrivate;
        private System.Windows.Forms.TextBox textBoxPublicPortFT;
        private System.Windows.Forms.Label labelPortaPubTF;
        private System.Windows.Forms.TextBox textBoxPrivatePortFT;
        private System.Windows.Forms.Label label5;
    }
}