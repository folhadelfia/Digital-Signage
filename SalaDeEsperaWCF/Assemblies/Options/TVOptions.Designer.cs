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
            this.groupBoxScan = new System.Windows.Forms.GroupBox();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.labelHelpForceScan = new System.Windows.Forms.Label();
            this.checkBoxForceScan = new System.Windows.Forms.CheckBox();
            this.labelSensKhz = new System.Windows.Forms.Label();
            this.buttonScanFrequencies = new System.Windows.Forms.Button();
            this.labelSensDesc = new System.Windows.Forms.Label();
            this.textBoxSens = new System.Windows.Forms.TextBox();
            this.labelMaxFreqKhz = new System.Windows.Forms.Label();
            this.labelMaxFreqDesc = new System.Windows.Forms.Label();
            this.textBoxMaxFreq = new System.Windows.Forms.TextBox();
            this.labelMinFreqKhz = new System.Windows.Forms.Label();
            this.labelMinFreqDesc = new System.Windows.Forms.Label();
            this.textBoxMinFreq = new System.Windows.Forms.TextBox();
            this.radioButtonManual = new System.Windows.Forms.RadioButton();
            this.radioButtonAutomatica = new System.Windows.Forms.RadioButton();
            this.labelFreqKhz = new System.Windows.Forms.Label();
            this.labelFreqDesc = new System.Windows.Forms.Label();
            this.textBoxFreq = new System.Windows.Forms.TextBox();
            this.comboBoxTunerDevices = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageDVBT = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listViewChannels = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPageCodecs = new System.Windows.Forms.TabPage();
            this.groupBoxVideo = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxMPEG2Codec = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxH264Decoder = new System.Windows.Forms.ComboBox();
            this.groupBoxAudio = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxAudioRenderer = new System.Windows.Forms.ComboBox();
            this.groupBoxScan.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageDVBT.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageCodecs.SuspendLayout();
            this.groupBoxVideo.SuspendLayout();
            this.groupBoxAudio.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxScan
            // 
            this.groupBoxScan.Controls.Add(this.buttonDefault);
            this.groupBoxScan.Controls.Add(this.labelHelpForceScan);
            this.groupBoxScan.Controls.Add(this.checkBoxForceScan);
            this.groupBoxScan.Controls.Add(this.labelSensKhz);
            this.groupBoxScan.Controls.Add(this.buttonScanFrequencies);
            this.groupBoxScan.Controls.Add(this.labelSensDesc);
            this.groupBoxScan.Controls.Add(this.textBoxSens);
            this.groupBoxScan.Controls.Add(this.labelMaxFreqKhz);
            this.groupBoxScan.Controls.Add(this.labelMaxFreqDesc);
            this.groupBoxScan.Controls.Add(this.textBoxMaxFreq);
            this.groupBoxScan.Controls.Add(this.labelMinFreqKhz);
            this.groupBoxScan.Controls.Add(this.labelMinFreqDesc);
            this.groupBoxScan.Controls.Add(this.textBoxMinFreq);
            this.groupBoxScan.Controls.Add(this.radioButtonManual);
            this.groupBoxScan.Controls.Add(this.radioButtonAutomatica);
            this.groupBoxScan.Controls.Add(this.labelFreqKhz);
            this.groupBoxScan.Controls.Add(this.labelFreqDesc);
            this.groupBoxScan.Controls.Add(this.textBoxFreq);
            this.groupBoxScan.Location = new System.Drawing.Point(10, 37);
            this.groupBoxScan.Name = "groupBoxScan";
            this.groupBoxScan.Size = new System.Drawing.Size(275, 210);
            this.groupBoxScan.TabIndex = 0;
            this.groupBoxScan.TabStop = false;
            this.groupBoxScan.Text = "Procura";
            this.groupBoxScan.Enter += new System.EventHandler(this.groupBoxTune_Enter);
            // 
            // buttonDefault
            // 
            this.buttonDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDefault.Location = new System.Drawing.Point(215, 181);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(54, 23);
            this.buttonDefault.TabIndex = 23;
            this.buttonDefault.Text = "Repor";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // labelHelpForceScan
            // 
            this.labelHelpForceScan.AutoSize = true;
            this.labelHelpForceScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHelpForceScan.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelHelpForceScan.Location = new System.Drawing.Point(103, 186);
            this.labelHelpForceScan.Name = "labelHelpForceScan";
            this.labelHelpForceScan.Size = new System.Drawing.Size(13, 13);
            this.labelHelpForceScan.TabIndex = 22;
            this.labelHelpForceScan.Text = "?";
            // 
            // checkBoxForceScan
            // 
            this.checkBoxForceScan.AutoSize = true;
            this.checkBoxForceScan.Location = new System.Drawing.Point(6, 185);
            this.checkBoxForceScan.Name = "checkBoxForceScan";
            this.checkBoxForceScan.Size = new System.Drawing.Size(104, 17);
            this.checkBoxForceScan.TabIndex = 21;
            this.checkBoxForceScan.Text = "Forçar a procura";
            this.checkBoxForceScan.UseVisualStyleBackColor = true;
            // 
            // labelSensKhz
            // 
            this.labelSensKhz.AutoSize = true;
            this.labelSensKhz.Location = new System.Drawing.Point(245, 98);
            this.labelSensKhz.Name = "labelSensKhz";
            this.labelSensKhz.Size = new System.Drawing.Size(24, 13);
            this.labelSensKhz.TabIndex = 20;
            this.labelSensKhz.Text = "khz";
            // 
            // buttonScanFrequencies
            // 
            this.buttonScanFrequencies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonScanFrequencies.Location = new System.Drawing.Point(134, 181);
            this.buttonScanFrequencies.Name = "buttonScanFrequencies";
            this.buttonScanFrequencies.Size = new System.Drawing.Size(75, 23);
            this.buttonScanFrequencies.TabIndex = 6;
            this.buttonScanFrequencies.Text = "Procurar";
            this.buttonScanFrequencies.UseVisualStyleBackColor = true;
            this.buttonScanFrequencies.Click += new System.EventHandler(this.buttonScanFrequencies_Click);
            // 
            // labelSensDesc
            // 
            this.labelSensDesc.AutoSize = true;
            this.labelSensDesc.Location = new System.Drawing.Point(10, 98);
            this.labelSensDesc.Margin = new System.Windows.Forms.Padding(7, 3, 3, 0);
            this.labelSensDesc.Name = "labelSensDesc";
            this.labelSensDesc.Size = new System.Drawing.Size(69, 13);
            this.labelSensDesc.TabIndex = 19;
            this.labelSensDesc.Text = "Sensibilidade";
            // 
            // textBoxSens
            // 
            this.textBoxSens.Location = new System.Drawing.Point(113, 95);
            this.textBoxSens.MaxLength = 6;
            this.textBoxSens.Name = "textBoxSens";
            this.textBoxSens.Size = new System.Drawing.Size(126, 20);
            this.textBoxSens.TabIndex = 18;
            // 
            // labelMaxFreqKhz
            // 
            this.labelMaxFreqKhz.AutoSize = true;
            this.labelMaxFreqKhz.Location = new System.Drawing.Point(245, 72);
            this.labelMaxFreqKhz.Name = "labelMaxFreqKhz";
            this.labelMaxFreqKhz.Size = new System.Drawing.Size(24, 13);
            this.labelMaxFreqKhz.TabIndex = 17;
            this.labelMaxFreqKhz.Text = "khz";
            // 
            // labelMaxFreqDesc
            // 
            this.labelMaxFreqDesc.AutoSize = true;
            this.labelMaxFreqDesc.Location = new System.Drawing.Point(10, 72);
            this.labelMaxFreqDesc.Margin = new System.Windows.Forms.Padding(7, 3, 3, 0);
            this.labelMaxFreqDesc.Name = "labelMaxFreqDesc";
            this.labelMaxFreqDesc.Size = new System.Drawing.Size(98, 13);
            this.labelMaxFreqDesc.TabIndex = 16;
            this.labelMaxFreqDesc.Text = "Frequência máxima";
            // 
            // textBoxMaxFreq
            // 
            this.textBoxMaxFreq.Location = new System.Drawing.Point(113, 69);
            this.textBoxMaxFreq.MaxLength = 6;
            this.textBoxMaxFreq.Name = "textBoxMaxFreq";
            this.textBoxMaxFreq.Size = new System.Drawing.Size(126, 20);
            this.textBoxMaxFreq.TabIndex = 15;
            // 
            // labelMinFreqKhz
            // 
            this.labelMinFreqKhz.AutoSize = true;
            this.labelMinFreqKhz.Location = new System.Drawing.Point(245, 46);
            this.labelMinFreqKhz.Name = "labelMinFreqKhz";
            this.labelMinFreqKhz.Size = new System.Drawing.Size(24, 13);
            this.labelMinFreqKhz.TabIndex = 14;
            this.labelMinFreqKhz.Text = "khz";
            // 
            // labelMinFreqDesc
            // 
            this.labelMinFreqDesc.AutoSize = true;
            this.labelMinFreqDesc.Location = new System.Drawing.Point(10, 46);
            this.labelMinFreqDesc.Margin = new System.Windows.Forms.Padding(7, 7, 3, 0);
            this.labelMinFreqDesc.Name = "labelMinFreqDesc";
            this.labelMinFreqDesc.Size = new System.Drawing.Size(97, 13);
            this.labelMinFreqDesc.TabIndex = 13;
            this.labelMinFreqDesc.Text = "Frequência mínima";
            // 
            // textBoxMinFreq
            // 
            this.textBoxMinFreq.Location = new System.Drawing.Point(113, 43);
            this.textBoxMinFreq.MaxLength = 6;
            this.textBoxMinFreq.Name = "textBoxMinFreq";
            this.textBoxMinFreq.Size = new System.Drawing.Size(126, 20);
            this.textBoxMinFreq.TabIndex = 12;
            // 
            // radioButtonManual
            // 
            this.radioButtonManual.AutoSize = true;
            this.radioButtonManual.Location = new System.Drawing.Point(6, 123);
            this.radioButtonManual.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
            this.radioButtonManual.Name = "radioButtonManual";
            this.radioButtonManual.Size = new System.Drawing.Size(60, 17);
            this.radioButtonManual.TabIndex = 11;
            this.radioButtonManual.Text = "Manual";
            this.radioButtonManual.UseVisualStyleBackColor = true;
            // 
            // radioButtonAutomatica
            // 
            this.radioButtonAutomatica.AutoSize = true;
            this.radioButtonAutomatica.Checked = true;
            this.radioButtonAutomatica.Location = new System.Drawing.Point(6, 19);
            this.radioButtonAutomatica.Name = "radioButtonAutomatica";
            this.radioButtonAutomatica.Size = new System.Drawing.Size(78, 17);
            this.radioButtonAutomatica.TabIndex = 10;
            this.radioButtonAutomatica.TabStop = true;
            this.radioButtonAutomatica.Text = "Automática";
            this.radioButtonAutomatica.UseVisualStyleBackColor = true;
            this.radioButtonAutomatica.CheckedChanged += new System.EventHandler(this.radioButtonAutomatica_CheckedChanged);
            // 
            // labelFreqKhz
            // 
            this.labelFreqKhz.AutoSize = true;
            this.labelFreqKhz.Enabled = false;
            this.labelFreqKhz.Location = new System.Drawing.Point(245, 150);
            this.labelFreqKhz.Name = "labelFreqKhz";
            this.labelFreqKhz.Size = new System.Drawing.Size(24, 13);
            this.labelFreqKhz.TabIndex = 9;
            this.labelFreqKhz.Text = "khz";
            // 
            // labelFreqDesc
            // 
            this.labelFreqDesc.AutoSize = true;
            this.labelFreqDesc.Enabled = false;
            this.labelFreqDesc.Location = new System.Drawing.Point(10, 150);
            this.labelFreqDesc.Margin = new System.Windows.Forms.Padding(7, 7, 3, 0);
            this.labelFreqDesc.Name = "labelFreqDesc";
            this.labelFreqDesc.Size = new System.Drawing.Size(60, 13);
            this.labelFreqDesc.TabIndex = 2;
            this.labelFreqDesc.Text = "Frequência";
            // 
            // textBoxFreq
            // 
            this.textBoxFreq.Enabled = false;
            this.textBoxFreq.Location = new System.Drawing.Point(113, 147);
            this.textBoxFreq.MaxLength = 6;
            this.textBoxFreq.Name = "textBoxFreq";
            this.textBoxFreq.Size = new System.Drawing.Size(126, 20);
            this.textBoxFreq.TabIndex = 0;
            this.textBoxFreq.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // comboBoxTunerDevices
            // 
            this.comboBoxTunerDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTunerDevices.FormattingEnabled = true;
            this.comboBoxTunerDevices.Location = new System.Drawing.Point(78, 7);
            this.comboBoxTunerDevices.Name = "comboBoxTunerDevices";
            this.comboBoxTunerDevices.Size = new System.Drawing.Size(189, 21);
            this.comboBoxTunerDevices.TabIndex = 8;
            this.comboBoxTunerDevices.SelectedIndexChanged += new System.EventHandler(this.comboBoxTunerDevices_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Sintonizador";
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(423, 297);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(504, 297);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancelar";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageDVBT);
            this.tabControl1.Controls.Add(this.tabPageCodecs);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(567, 279);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPageDVBT
            // 
            this.tabPageDVBT.Controls.Add(this.groupBox1);
            this.tabPageDVBT.Controls.Add(this.groupBoxScan);
            this.tabPageDVBT.Controls.Add(this.comboBoxTunerDevices);
            this.tabPageDVBT.Controls.Add(this.label2);
            this.tabPageDVBT.Location = new System.Drawing.Point(4, 22);
            this.tabPageDVBT.Name = "tabPageDVBT";
            this.tabPageDVBT.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDVBT.Size = new System.Drawing.Size(559, 253);
            this.tabPageDVBT.TabIndex = 0;
            this.tabPageDVBT.Text = "Rede TV";
            this.tabPageDVBT.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listViewChannels);
            this.groupBox1.Location = new System.Drawing.Point(291, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 210);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Canais";
            // 
            // listViewChannels
            // 
            this.listViewChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewChannels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewChannels.FullRowSelect = true;
            this.listViewChannels.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewChannels.Location = new System.Drawing.Point(6, 19);
            this.listViewChannels.MultiSelect = false;
            this.listViewChannels.Name = "listViewChannels";
            this.listViewChannels.ShowItemToolTips = true;
            this.listViewChannels.Size = new System.Drawing.Size(250, 185);
            this.listViewChannels.TabIndex = 0;
            this.listViewChannels.UseCompatibleStateImageBehavior = false;
            this.listViewChannels.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 82;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 164;
            // 
            // tabPageCodecs
            // 
            this.tabPageCodecs.Controls.Add(this.groupBoxVideo);
            this.tabPageCodecs.Controls.Add(this.groupBoxAudio);
            this.tabPageCodecs.Location = new System.Drawing.Point(4, 22);
            this.tabPageCodecs.Name = "tabPageCodecs";
            this.tabPageCodecs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCodecs.Size = new System.Drawing.Size(559, 253);
            this.tabPageCodecs.TabIndex = 1;
            this.tabPageCodecs.Text = "Codecs";
            this.tabPageCodecs.UseVisualStyleBackColor = true;
            // 
            // groupBoxVideo
            // 
            this.groupBoxVideo.Controls.Add(this.label1);
            this.groupBoxVideo.Controls.Add(this.comboBoxMPEG2Codec);
            this.groupBoxVideo.Controls.Add(this.label11);
            this.groupBoxVideo.Controls.Add(this.comboBoxH264Decoder);
            this.groupBoxVideo.Location = new System.Drawing.Point(6, 65);
            this.groupBoxVideo.Name = "groupBoxVideo";
            this.groupBoxVideo.Size = new System.Drawing.Size(307, 83);
            this.groupBoxVideo.TabIndex = 1;
            this.groupBoxVideo.TabStop = false;
            this.groupBoxVideo.Text = "Video";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Descodificador MPEG2";
            // 
            // comboBoxMPEG2Codec
            // 
            this.comboBoxMPEG2Codec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMPEG2Codec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMPEG2Codec.FormattingEnabled = true;
            this.comboBoxMPEG2Codec.Location = new System.Drawing.Point(134, 47);
            this.comboBoxMPEG2Codec.Name = "comboBoxMPEG2Codec";
            this.comboBoxMPEG2Codec.Size = new System.Drawing.Size(167, 21);
            this.comboBoxMPEG2Codec.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 23);
            this.label11.Margin = new System.Windows.Forms.Padding(7, 7, 3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(107, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Descodificador H264";
            // 
            // comboBoxH264Decoder
            // 
            this.comboBoxH264Decoder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxH264Decoder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxH264Decoder.FormattingEnabled = true;
            this.comboBoxH264Decoder.Location = new System.Drawing.Point(134, 20);
            this.comboBoxH264Decoder.Name = "comboBoxH264Decoder";
            this.comboBoxH264Decoder.Size = new System.Drawing.Size(167, 21);
            this.comboBoxH264Decoder.TabIndex = 2;
            // 
            // groupBoxAudio
            // 
            this.groupBoxAudio.Controls.Add(this.label10);
            this.groupBoxAudio.Controls.Add(this.comboBoxAudioRenderer);
            this.groupBoxAudio.Location = new System.Drawing.Point(6, 6);
            this.groupBoxAudio.Name = "groupBoxAudio";
            this.groupBoxAudio.Size = new System.Drawing.Size(307, 53);
            this.groupBoxAudio.TabIndex = 0;
            this.groupBoxAudio.TabStop = false;
            this.groupBoxAudio.Text = "Audio";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Reprodutor";
            // 
            // comboBoxAudioRenderer
            // 
            this.comboBoxAudioRenderer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAudioRenderer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAudioRenderer.FormattingEnabled = true;
            this.comboBoxAudioRenderer.Location = new System.Drawing.Point(94, 19);
            this.comboBoxAudioRenderer.Name = "comboBoxAudioRenderer";
            this.comboBoxAudioRenderer.Size = new System.Drawing.Size(207, 21);
            this.comboBoxAudioRenderer.TabIndex = 1;
            // 
            // TVOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(588, 331);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Name = "TVOptions";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Opções de TV";
            this.Load += new System.EventHandler(this.TVOptions_Load);
            this.groupBoxScan.ResumeLayout(false);
            this.groupBoxScan.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageDVBT.ResumeLayout(false);
            this.tabPageDVBT.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPageCodecs.ResumeLayout(false);
            this.groupBoxVideo.ResumeLayout(false);
            this.groupBoxVideo.PerformLayout();
            this.groupBoxAudio.ResumeLayout(false);
            this.groupBoxAudio.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxScan;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelFreqDesc;
        private System.Windows.Forms.TextBox textBoxFreq;
        private System.Windows.Forms.Button buttonScanFrequencies;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboBoxTunerDevices;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelFreqKhz;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDVBT;
        private System.Windows.Forms.TabPage tabPageCodecs;
        private System.Windows.Forms.Label labelSensKhz;
        private System.Windows.Forms.Label labelSensDesc;
        private System.Windows.Forms.TextBox textBoxSens;
        private System.Windows.Forms.Label labelMaxFreqKhz;
        private System.Windows.Forms.Label labelMaxFreqDesc;
        private System.Windows.Forms.TextBox textBoxMaxFreq;
        private System.Windows.Forms.Label labelMinFreqKhz;
        private System.Windows.Forms.Label labelMinFreqDesc;
        private System.Windows.Forms.TextBox textBoxMinFreq;
        private System.Windows.Forms.RadioButton radioButtonManual;
        private System.Windows.Forms.RadioButton radioButtonAutomatica;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listViewChannels;
        private System.Windows.Forms.GroupBox groupBoxVideo;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxH264Decoder;
        private System.Windows.Forms.GroupBox groupBoxAudio;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxAudioRenderer;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.CheckBox checkBoxForceScan;
        private System.Windows.Forms.Label labelHelpForceScan;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxMPEG2Codec;
    }
}