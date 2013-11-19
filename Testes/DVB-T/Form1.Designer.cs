namespace DVB_T
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.SIDTextBox = new System.Windows.Forms.TextBox();
            this.TSIDTextBox = new System.Windows.Forms.TextBox();
            this.ONIDTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonLigar = new System.Windows.Forms.Button();
            this.logRichTextBox = new System.Windows.Forms.RichTextBox();
            this.buttonDesligar = new System.Windows.Forms.Button();
            this.buttonSintonizar = new System.Windows.Forms.Button();
            this.FrequenciaTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.listBoxChannels = new System.Windows.Forms.ListBox();
            this.buttonUpdateList = new System.Windows.Forms.Button();
            this.buttonDeinterlace = new System.Windows.Forms.Button();
            this.buttonSaveGraph = new System.Windows.Forms.Button();
            this.groupBoxCodecs = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxVCodecMPEG2 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxARenderer = new System.Windows.Forms.ComboBox();
            this.comboBoxVCodecsH264 = new System.Windows.Forms.ComboBox();
            this.comboBoxACodecs = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxVRenderer = new System.Windows.Forms.ComboBox();
            this.digitalTVScreen = new DigitalTV.DigitalTVScreen();
            this.groupBoxCodecs.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SIDTextBox
            // 
            this.SIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SIDTextBox.Location = new System.Drawing.Point(72, 91);
            this.SIDTextBox.Name = "SIDTextBox";
            this.SIDTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SIDTextBox.Size = new System.Drawing.Size(219, 20);
            this.SIDTextBox.TabIndex = 4;
            this.SIDTextBox.Text = "1103";
            // 
            // TSIDTextBox
            // 
            this.TSIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TSIDTextBox.Location = new System.Drawing.Point(72, 65);
            this.TSIDTextBox.Name = "TSIDTextBox";
            this.TSIDTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TSIDTextBox.Size = new System.Drawing.Size(219, 20);
            this.TSIDTextBox.TabIndex = 3;
            this.TSIDTextBox.Text = "-1";
            // 
            // ONIDTextBox
            // 
            this.ONIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ONIDTextBox.Location = new System.Drawing.Point(72, 39);
            this.ONIDTextBox.Name = "ONIDTextBox";
            this.ONIDTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ONIDTextBox.Size = new System.Drawing.Size(219, 20);
            this.ONIDTextBox.TabIndex = 2;
            this.ONIDTextBox.Text = "-1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Frequência";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "ONID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "TSID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "SID";
            // 
            // buttonLigar
            // 
            this.buttonLigar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLigar.Location = new System.Drawing.Point(155, 383);
            this.buttonLigar.Name = "buttonLigar";
            this.buttonLigar.Size = new System.Drawing.Size(75, 23);
            this.buttonLigar.TabIndex = 5;
            this.buttonLigar.Text = "Ligar";
            this.buttonLigar.UseVisualStyleBackColor = true;
            this.buttonLigar.Click += new System.EventHandler(this.buttonLigar_Click);
            // 
            // logRichTextBox
            // 
            this.logRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logRichTextBox.Location = new System.Drawing.Point(789, 9);
            this.logRichTextBox.Name = "logRichTextBox";
            this.logRichTextBox.ReadOnly = true;
            this.logRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.logRichTextBox.Size = new System.Drawing.Size(225, 486);
            this.logRichTextBox.TabIndex = 11;
            this.logRichTextBox.TabStop = false;
            this.logRichTextBox.Text = "";
            // 
            // buttonDesligar
            // 
            this.buttonDesligar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDesligar.Location = new System.Drawing.Point(155, 412);
            this.buttonDesligar.Name = "buttonDesligar";
            this.buttonDesligar.Size = new System.Drawing.Size(75, 23);
            this.buttonDesligar.TabIndex = 6;
            this.buttonDesligar.Text = "Desligar";
            this.buttonDesligar.UseVisualStyleBackColor = true;
            this.buttonDesligar.Click += new System.EventHandler(this.buttonDesligar_Click);
            // 
            // buttonSintonizar
            // 
            this.buttonSintonizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSintonizar.Location = new System.Drawing.Point(155, 441);
            this.buttonSintonizar.Name = "buttonSintonizar";
            this.buttonSintonizar.Size = new System.Drawing.Size(75, 23);
            this.buttonSintonizar.TabIndex = 7;
            this.buttonSintonizar.Text = "Sintonizar";
            this.buttonSintonizar.UseVisualStyleBackColor = true;
            this.buttonSintonizar.Click += new System.EventHandler(this.buttonSintonizar_Click);
            // 
            // FrequenciaTextBox
            // 
            this.FrequenciaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrequenciaTextBox.Location = new System.Drawing.Point(72, 13);
            this.FrequenciaTextBox.Name = "FrequenciaTextBox";
            this.FrequenciaTextBox.Size = new System.Drawing.Size(189, 20);
            this.FrequenciaTextBox.TabIndex = 1;
            this.FrequenciaTextBox.Text = "754000";
            this.FrequenciaTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrequenciaTextBox_KeyPress);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(267, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "khz";
            // 
            // listBoxChannels
            // 
            this.listBoxChannels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxChannels.DisplayMember = "Name";
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.Location = new System.Drawing.Point(15, 306);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.Size = new System.Drawing.Size(134, 186);
            this.listBoxChannels.TabIndex = 15;
            this.listBoxChannels.SelectedIndexChanged += new System.EventHandler(this.listBoxChannels_SelectedIndexChanged);
            // 
            // buttonUpdateList
            // 
            this.buttonUpdateList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUpdateList.Location = new System.Drawing.Point(236, 383);
            this.buttonUpdateList.Name = "buttonUpdateList";
            this.buttonUpdateList.Size = new System.Drawing.Size(75, 37);
            this.buttonUpdateList.TabIndex = 16;
            this.buttonUpdateList.Text = "Actualizar Lista";
            this.buttonUpdateList.UseVisualStyleBackColor = true;
            this.buttonUpdateList.Click += new System.EventHandler(this.buttonUpdateList_Click);
            // 
            // buttonDeinterlace
            // 
            this.buttonDeinterlace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDeinterlace.Location = new System.Drawing.Point(236, 426);
            this.buttonDeinterlace.Name = "buttonDeinterlace";
            this.buttonDeinterlace.Size = new System.Drawing.Size(75, 38);
            this.buttonDeinterlace.TabIndex = 17;
            this.buttonDeinterlace.Text = "Deinterlace Opts";
            this.buttonDeinterlace.UseVisualStyleBackColor = true;
            this.buttonDeinterlace.Click += new System.EventHandler(this.buttonDeinterlace_Click);
            // 
            // buttonSaveGraph
            // 
            this.buttonSaveGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSaveGraph.Location = new System.Drawing.Point(237, 474);
            this.buttonSaveGraph.Name = "buttonSaveGraph";
            this.buttonSaveGraph.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveGraph.TabIndex = 21;
            this.buttonSaveGraph.Text = "Save to file";
            this.buttonSaveGraph.UseVisualStyleBackColor = true;
            this.buttonSaveGraph.Click += new System.EventHandler(this.buttonSaveGraph_Click);
            // 
            // groupBoxCodecs
            // 
            this.groupBoxCodecs.Controls.Add(this.comboBoxVRenderer);
            this.groupBoxCodecs.Controls.Add(this.label10);
            this.groupBoxCodecs.Controls.Add(this.label9);
            this.groupBoxCodecs.Controls.Add(this.comboBoxVCodecMPEG2);
            this.groupBoxCodecs.Controls.Add(this.label8);
            this.groupBoxCodecs.Controls.Add(this.label7);
            this.groupBoxCodecs.Controls.Add(this.label6);
            this.groupBoxCodecs.Controls.Add(this.comboBoxARenderer);
            this.groupBoxCodecs.Controls.Add(this.comboBoxVCodecsH264);
            this.groupBoxCodecs.Controls.Add(this.comboBoxACodecs);
            this.groupBoxCodecs.Location = new System.Drawing.Point(15, 12);
            this.groupBoxCodecs.Name = "groupBoxCodecs";
            this.groupBoxCodecs.Size = new System.Drawing.Size(300, 161);
            this.groupBoxCodecs.TabIndex = 23;
            this.groupBoxCodecs.TabStop = false;
            this.groupBoxCodecs.Text = "Codecs";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 49);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Codec MPEG2";
            // 
            // comboBoxVCodecMPEG2
            // 
            this.comboBoxVCodecMPEG2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxVCodecMPEG2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVCodecMPEG2.FormattingEnabled = true;
            this.comboBoxVCodecMPEG2.Location = new System.Drawing.Point(94, 46);
            this.comboBoxVCodecMPEG2.Name = "comboBoxVCodecMPEG2";
            this.comboBoxVCodecMPEG2.Size = new System.Drawing.Size(199, 21);
            this.comboBoxVCodecMPEG2.TabIndex = 6;
            this.comboBoxVCodecMPEG2.SelectedValueChanged += new System.EventHandler(this.comboBoxVCodecMPEG2_SelectedValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Codec audio";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Renderer audio";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Codec H264";
            // 
            // comboBoxARenderer
            // 
            this.comboBoxARenderer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxARenderer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxARenderer.FormattingEnabled = true;
            this.comboBoxARenderer.Location = new System.Drawing.Point(94, 100);
            this.comboBoxARenderer.Name = "comboBoxARenderer";
            this.comboBoxARenderer.Size = new System.Drawing.Size(199, 21);
            this.comboBoxARenderer.TabIndex = 2;
            this.comboBoxARenderer.SelectedValueChanged += new System.EventHandler(this.comboBoxARenderer_SelectedValueChanged);
            // 
            // comboBoxVCodecsH264
            // 
            this.comboBoxVCodecsH264.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxVCodecsH264.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVCodecsH264.FormattingEnabled = true;
            this.comboBoxVCodecsH264.Location = new System.Drawing.Point(94, 19);
            this.comboBoxVCodecsH264.Name = "comboBoxVCodecsH264";
            this.comboBoxVCodecsH264.Size = new System.Drawing.Size(199, 21);
            this.comboBoxVCodecsH264.TabIndex = 1;
            this.comboBoxVCodecsH264.SelectedValueChanged += new System.EventHandler(this.comboBoxVCodecsH264_SelectedValueChanged);
            // 
            // comboBoxACodecs
            // 
            this.comboBoxACodecs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxACodecs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxACodecs.FormattingEnabled = true;
            this.comboBoxACodecs.Location = new System.Drawing.Point(94, 73);
            this.comboBoxACodecs.Name = "comboBoxACodecs";
            this.comboBoxACodecs.Size = new System.Drawing.Size(199, 21);
            this.comboBoxACodecs.TabIndex = 0;
            this.comboBoxACodecs.SelectedValueChanged += new System.EventHandler(this.comboBoxACodecs_SelectedValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.SIDTextBox);
            this.groupBox1.Controls.Add(this.TSIDTextBox);
            this.groupBox1.Controls.Add(this.ONIDTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.FrequenciaTextBox);
            this.groupBox1.Location = new System.Drawing.Point(15, 179);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 120);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tuner";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(156, 474);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "PANIC";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Renderer video";
            // 
            // comboBoxVRenderer
            // 
            this.comboBoxVRenderer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxVRenderer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVRenderer.FormattingEnabled = true;
            this.comboBoxVRenderer.Items.AddRange(new object[] {
            "Video Mixing Renderer 9",
            "Enhanced Video Renderer"});
            this.comboBoxVRenderer.Location = new System.Drawing.Point(94, 127);
            this.comboBoxVRenderer.Name = "comboBoxVRenderer";
            this.comboBoxVRenderer.Size = new System.Drawing.Size(199, 21);
            this.comboBoxVRenderer.TabIndex = 9;
            this.comboBoxVRenderer.SelectedValueChanged += new System.EventHandler(this.comboBoxVRenderer_SelectedValueChanged);
            // 
            // digitalTVScreen
            // 
            this.digitalTVScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.digitalTVScreen.AudioDecoder = null;
            this.digitalTVScreen.AudioRenderer = null;
            this.digitalTVScreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.digitalTVScreen.Frequencia = 0;
            this.digitalTVScreen.H264Decoder = null;
            this.digitalTVScreen.Location = new System.Drawing.Point(321, 12);
            this.digitalTVScreen.MinimumSize = new System.Drawing.Size(50, 50);
            this.digitalTVScreen.MPEG2Decoder = null;
            this.digitalTVScreen.Name = "digitalTVScreen";
            this.digitalTVScreen.ONID = -1;
            this.digitalTVScreen.SID = -1;
            this.digitalTVScreen.Size = new System.Drawing.Size(462, 483);
            this.digitalTVScreen.TabIndex = 28;
            this.digitalTVScreen.TSID = -1;
            this.digitalTVScreen.UseBlackBands = false;
            this.digitalTVScreen.VideoAspectRatio = 0D;
            this.digitalTVScreen.VideoKeepAspectRatio = false;
            this.digitalTVScreen.VideoOffset = ((System.Drawing.PointF)(resources.GetObject("digitalTVScreen.VideoOffset")));
            this.digitalTVScreen.VideoZoomMode = DigitalTV.VideoSizeMode.FromInside;
            this.digitalTVScreen.VideoZoomValue = 0D;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 507);
            this.Controls.Add(this.digitalTVScreen);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxCodecs);
            this.Controls.Add(this.buttonSaveGraph);
            this.Controls.Add(this.buttonDeinterlace);
            this.Controls.Add(this.buttonUpdateList);
            this.Controls.Add(this.listBoxChannels);
            this.Controls.Add(this.buttonSintonizar);
            this.Controls.Add(this.buttonDesligar);
            this.Controls.Add(this.logRichTextBox);
            this.Controls.Add(this.buttonLigar);
            this.Name = "Form1";
            this.Text = "DVB-T";
            this.groupBoxCodecs.ResumeLayout(false);
            this.groupBoxCodecs.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox SIDTextBox;
        private System.Windows.Forms.TextBox TSIDTextBox;
        private System.Windows.Forms.TextBox ONIDTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonLigar;
        private System.Windows.Forms.RichTextBox logRichTextBox;
        private System.Windows.Forms.Button buttonDesligar;
        private System.Windows.Forms.Button buttonSintonizar;
        private System.Windows.Forms.TextBox FrequenciaTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox listBoxChannels;
        private System.Windows.Forms.Button buttonUpdateList;
        private System.Windows.Forms.Button buttonDeinterlace;
        private System.Windows.Forms.Button buttonSaveGraph;
        private System.Windows.Forms.GroupBox groupBoxCodecs;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxARenderer;
        private System.Windows.Forms.ComboBox comboBoxVCodecsH264;
        private System.Windows.Forms.ComboBox comboBoxACodecs;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxVCodecMPEG2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBoxVRenderer;
        private System.Windows.Forms.Label label10;
        private DigitalTV.DigitalTVScreen digitalTVScreen;

    }
}

