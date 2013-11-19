namespace Assemblies.Options
{
    partial class TVOptions
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
            this.groupBoxTune = new System.Windows.Forms.GroupBox();
            this.comboBoxTunerDevices = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonScanFrequencies = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFreq = new System.Windows.Forms.TextBox();
            this.groupBoxCanais = new System.Windows.Forms.GroupBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBoxAllChannels = new System.Windows.Forms.ListBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelKhz = new System.Windows.Forms.Label();
            this.groupBoxTune.SuspendLayout();
            this.groupBoxCanais.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTune
            // 
            this.groupBoxTune.Controls.Add(this.labelKhz);
            this.groupBoxTune.Controls.Add(this.comboBoxTunerDevices);
            this.groupBoxTune.Controls.Add(this.label2);
            this.groupBoxTune.Controls.Add(this.buttonScanFrequencies);
            this.groupBoxTune.Controls.Add(this.label1);
            this.groupBoxTune.Controls.Add(this.textBoxFreq);
            this.groupBoxTune.Location = new System.Drawing.Point(12, 12);
            this.groupBoxTune.Name = "groupBoxTune";
            this.groupBoxTune.Size = new System.Drawing.Size(217, 375);
            this.groupBoxTune.TabIndex = 0;
            this.groupBoxTune.TabStop = false;
            this.groupBoxTune.Text = "Tune";
            // 
            // comboBoxTunerDevices
            // 
            this.comboBoxTunerDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTunerDevices.FormattingEnabled = true;
            this.comboBoxTunerDevices.Location = new System.Drawing.Point(77, 45);
            this.comboBoxTunerDevices.Name = "comboBoxTunerDevices";
            this.comboBoxTunerDevices.Size = new System.Drawing.Size(134, 21);
            this.comboBoxTunerDevices.TabIndex = 8;
            this.comboBoxTunerDevices.SelectedIndexChanged += new System.EventHandler(this.comboBoxTunerDevices_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Tuner";
            // 
            // buttonScanFrequencies
            // 
            this.buttonScanFrequencies.Location = new System.Drawing.Point(136, 346);
            this.buttonScanFrequencies.Name = "buttonScanFrequencies";
            this.buttonScanFrequencies.Size = new System.Drawing.Size(75, 23);
            this.buttonScanFrequencies.TabIndex = 6;
            this.buttonScanFrequencies.Text = "Scan";
            this.buttonScanFrequencies.UseVisualStyleBackColor = true;
            this.buttonScanFrequencies.Click += new System.EventHandler(this.buttonScanFrequencies_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Frequência";
            // 
            // textBoxFreq
            // 
            this.textBoxFreq.Location = new System.Drawing.Point(77, 19);
            this.textBoxFreq.MaxLength = 6;
            this.textBoxFreq.Name = "textBoxFreq";
            this.textBoxFreq.Size = new System.Drawing.Size(104, 20);
            this.textBoxFreq.TabIndex = 0;
            this.textBoxFreq.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // groupBoxCanais
            // 
            this.groupBoxCanais.Controls.Add(this.listBox2);
            this.groupBoxCanais.Controls.Add(this.listBoxAllChannels);
            this.groupBoxCanais.Location = new System.Drawing.Point(235, 12);
            this.groupBoxCanais.Name = "groupBoxCanais";
            this.groupBoxCanais.Size = new System.Drawing.Size(470, 375);
            this.groupBoxCanais.TabIndex = 1;
            this.groupBoxCanais.TabStop = false;
            this.groupBoxCanais.Text = "Canais";
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(257, 19);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(184, 342);
            this.listBox2.TabIndex = 1;
            // 
            // listBoxAllChannels
            // 
            this.listBoxAllChannels.DisplayMember = "Name";
            this.listBoxAllChannels.FormattingEnabled = true;
            this.listBoxAllChannels.Location = new System.Drawing.Point(6, 19);
            this.listBoxAllChannels.Name = "listBoxAllChannels";
            this.listBoxAllChannels.Size = new System.Drawing.Size(184, 342);
            this.listBoxAllChannels.TabIndex = 0;
            this.listBoxAllChannels.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxAllChannels_MouseDown);
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(549, 393);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(630, 393);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancelar";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelKhz
            // 
            this.labelKhz.AutoSize = true;
            this.labelKhz.Location = new System.Drawing.Point(187, 22);
            this.labelKhz.Name = "labelKhz";
            this.labelKhz.Size = new System.Drawing.Size(24, 13);
            this.labelKhz.TabIndex = 9;
            this.labelKhz.Text = "khz";
            // 
            // TVOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(717, 428);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBoxCanais);
            this.Controls.Add(this.groupBoxTune);
            this.Name = "TVOptions";
            this.Text = "TVOptions";
            this.Load += new System.EventHandler(this.TVOptions_Load);
            this.groupBoxTune.ResumeLayout(false);
            this.groupBoxTune.PerformLayout();
            this.groupBoxCanais.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTune;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFreq;
        private System.Windows.Forms.GroupBox groupBoxCanais;
        private System.Windows.Forms.Button buttonScanFrequencies;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ListBox listBoxAllChannels;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ComboBox comboBoxTunerDevices;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelKhz;
    }
}