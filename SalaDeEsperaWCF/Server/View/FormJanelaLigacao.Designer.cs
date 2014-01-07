namespace Server.View
{
    partial class FormJanelaLigacao
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
            this.groupBoxLocal = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLocalIP = new System.Windows.Forms.TextBox();
            this.textBoxLocalPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxServer = new System.Windows.Forms.GroupBox();
            this.labelIP = new System.Windows.Forms.Label();
            this.textBoxServerPort = new System.Windows.Forms.TextBox();
            this.Porta = new System.Windows.Forms.Label();
            this.textBoxServerIP = new System.Windows.Forms.TextBox();
            this.groupBoxLocal.SuspendLayout();
            this.groupBoxServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxLocal
            // 
            this.groupBoxLocal.Controls.Add(this.label1);
            this.groupBoxLocal.Controls.Add(this.textBoxLocalIP);
            this.groupBoxLocal.Controls.Add(this.textBoxLocalPort);
            this.groupBoxLocal.Controls.Add(this.label2);
            this.groupBoxLocal.Location = new System.Drawing.Point(12, 12);
            this.groupBoxLocal.Name = "groupBoxLocal";
            this.groupBoxLocal.Size = new System.Drawing.Size(172, 69);
            this.groupBoxLocal.TabIndex = 8;
            this.groupBoxLocal.TabStop = false;
            this.groupBoxLocal.Text = "Local";
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
            // groupBoxServer
            // 
            this.groupBoxServer.Controls.Add(this.labelIP);
            this.groupBoxServer.Controls.Add(this.textBoxServerPort);
            this.groupBoxServer.Controls.Add(this.Porta);
            this.groupBoxServer.Controls.Add(this.textBoxServerIP);
            this.groupBoxServer.Location = new System.Drawing.Point(12, 87);
            this.groupBoxServer.Name = "groupBoxServer";
            this.groupBoxServer.Size = new System.Drawing.Size(172, 68);
            this.groupBoxServer.TabIndex = 7;
            this.groupBoxServer.TabStop = false;
            this.groupBoxServer.Text = "Servidor";
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
            // textBoxServerPort
            // 
            this.textBoxServerPort.Location = new System.Drawing.Point(47, 39);
            this.textBoxServerPort.Name = "textBoxServerPort";
            this.textBoxServerPort.Size = new System.Drawing.Size(119, 20);
            this.textBoxServerPort.TabIndex = 4;
            this.textBoxServerPort.Text = "9021";
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
            this.textBoxServerIP.Text = "10.0.0.165";
            // 
            // FormJanelaLigacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(192, 165);
            this.Controls.Add(this.groupBoxLocal);
            this.Controls.Add(this.groupBoxServer);
            this.Name = "FormJanelaLigacao";
            this.Text = "FormJanelaLigacao";
            this.groupBoxLocal.ResumeLayout(false);
            this.groupBoxLocal.PerformLayout();
            this.groupBoxServer.ResumeLayout(false);
            this.groupBoxServer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxLocal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLocalIP;
        private System.Windows.Forms.TextBox textBoxLocalPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxServer;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.TextBox textBoxServerPort;
        private System.Windows.Forms.Label Porta;
        private System.Windows.Forms.TextBox textBoxServerIP;
    }
}