namespace TestTubeVideoPlayerDShow
{
    partial class FormTestTubeVideoPlayerDShow
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
            this.buttonStart = new System.Windows.Forms.Button();
            this.comboBoxDevices = new System.Windows.Forms.ComboBox();
            this.radioButtonKeep = new System.Windows.Forms.RadioButton();
            this.radioButton169 = new System.Windows.Forms.RadioButton();
            this.radioButton43 = new System.Windows.Forms.RadioButton();
            this.groupBoxAspect = new System.Windows.Forms.GroupBox();
            this.groupBoxZoom = new System.Windows.Forms.GroupBox();
            this.radioButtonZoomStretch = new System.Windows.Forms.RadioButton();
            this.radioButtonZoomFit = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxStandards = new System.Windows.Forms.ComboBox();
            this.labelVStandard = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxSources = new System.Windows.Forms.ComboBox();
            this._crossbarVideoPlayer = new VideoPlayerDShowLib.CrossbarVideoPlayer();
            this.groupBoxAspect.SuspendLayout();
            this.groupBoxZoom.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Location = new System.Drawing.Point(657, 187);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // comboBoxDevices
            // 
            this.comboBoxDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDevices.FormattingEnabled = true;
            this.comboBoxDevices.Location = new System.Drawing.Point(546, 160);
            this.comboBoxDevices.Name = "comboBoxDevices";
            this.comboBoxDevices.Size = new System.Drawing.Size(186, 21);
            this.comboBoxDevices.TabIndex = 2;
            this.comboBoxDevices.SelectedIndexChanged += new System.EventHandler(this.comboBoxDevices_SelectedIndexChanged);
            this.comboBoxDevices.MouseEnter += new System.EventHandler(this.comboBoxDevices_MouseEnter);
            // 
            // radioButtonKeep
            // 
            this.radioButtonKeep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonKeep.AutoSize = true;
            this.radioButtonKeep.Checked = true;
            this.radioButtonKeep.Location = new System.Drawing.Point(15, 18);
            this.radioButtonKeep.Name = "radioButtonKeep";
            this.radioButtonKeep.Size = new System.Drawing.Size(73, 17);
            this.radioButtonKeep.TabIndex = 4;
            this.radioButtonKeep.TabStop = true;
            this.radioButtonKeep.Text = "Keep ratio";
            this.radioButtonKeep.UseVisualStyleBackColor = true;
            this.radioButtonKeep.CheckedChanged += new System.EventHandler(this.radioButtonKeep_CheckedChanged);
            // 
            // radioButton169
            // 
            this.radioButton169.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButton169.AutoSize = true;
            this.radioButton169.Location = new System.Drawing.Point(94, 18);
            this.radioButton169.Name = "radioButton169";
            this.radioButton169.Size = new System.Drawing.Size(78, 17);
            this.radioButton169.TabIndex = 5;
            this.radioButton169.Text = "Force 16/9";
            this.radioButton169.UseVisualStyleBackColor = true;
            this.radioButton169.CheckedChanged += new System.EventHandler(this.radioButton169_CheckedChanged);
            // 
            // radioButton43
            // 
            this.radioButton43.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButton43.AutoSize = true;
            this.radioButton43.Location = new System.Drawing.Point(178, 18);
            this.radioButton43.Name = "radioButton43";
            this.radioButton43.Size = new System.Drawing.Size(72, 17);
            this.radioButton43.TabIndex = 6;
            this.radioButton43.Text = "Force 4/3";
            this.radioButton43.UseVisualStyleBackColor = true;
            this.radioButton43.CheckedChanged += new System.EventHandler(this.radioButton43_CheckedChanged);
            // 
            // groupBoxAspect
            // 
            this.groupBoxAspect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAspect.Controls.Add(this.radioButtonKeep);
            this.groupBoxAspect.Controls.Add(this.radioButton43);
            this.groupBoxAspect.Controls.Add(this.radioButton169);
            this.groupBoxAspect.Location = new System.Drawing.Point(477, 12);
            this.groupBoxAspect.Name = "groupBoxAspect";
            this.groupBoxAspect.Size = new System.Drawing.Size(255, 41);
            this.groupBoxAspect.TabIndex = 7;
            this.groupBoxAspect.TabStop = false;
            this.groupBoxAspect.Text = "Aspect";
            // 
            // groupBoxZoom
            // 
            this.groupBoxZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxZoom.Controls.Add(this.radioButtonZoomStretch);
            this.groupBoxZoom.Controls.Add(this.radioButtonZoomFit);
            this.groupBoxZoom.Location = new System.Drawing.Point(477, 59);
            this.groupBoxZoom.Name = "groupBoxZoom";
            this.groupBoxZoom.Size = new System.Drawing.Size(114, 41);
            this.groupBoxZoom.TabIndex = 8;
            this.groupBoxZoom.TabStop = false;
            this.groupBoxZoom.Text = "Zoom mode";
            // 
            // radioButtonZoomStretch
            // 
            this.radioButtonZoomStretch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonZoomStretch.AutoSize = true;
            this.radioButtonZoomStretch.Location = new System.Drawing.Point(48, 19);
            this.radioButtonZoomStretch.Name = "radioButtonZoomStretch";
            this.radioButtonZoomStretch.Size = new System.Drawing.Size(59, 17);
            this.radioButtonZoomStretch.TabIndex = 4;
            this.radioButtonZoomStretch.Text = "Stretch";
            this.radioButtonZoomStretch.UseVisualStyleBackColor = true;
            this.radioButtonZoomStretch.CheckedChanged += new System.EventHandler(this.radioButtonZoomStretch_CheckedChanged);
            // 
            // radioButtonZoomFit
            // 
            this.radioButtonZoomFit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonZoomFit.AutoSize = true;
            this.radioButtonZoomFit.Checked = true;
            this.radioButtonZoomFit.Location = new System.Drawing.Point(6, 19);
            this.radioButtonZoomFit.Name = "radioButtonZoomFit";
            this.radioButtonZoomFit.Size = new System.Drawing.Size(36, 17);
            this.radioButtonZoomFit.TabIndex = 5;
            this.radioButtonZoomFit.TabStop = true;
            this.radioButtonZoomFit.Text = "Fit";
            this.radioButtonZoomFit.UseVisualStyleBackColor = true;
            this.radioButtonZoomFit.CheckedChanged += new System.EventHandler(this.radioButtonZoomFit_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(478, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Device";
            // 
            // comboBoxStandards
            // 
            this.comboBoxStandards.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxStandards.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStandards.FormattingEnabled = true;
            this.comboBoxStandards.Location = new System.Drawing.Point(562, 106);
            this.comboBoxStandards.Name = "comboBoxStandards";
            this.comboBoxStandards.Size = new System.Drawing.Size(170, 21);
            this.comboBoxStandards.TabIndex = 10;
            this.comboBoxStandards.SelectedIndexChanged += new System.EventHandler(this.comboBoxStandards_SelectedIndexChanged);
            // 
            // labelVStandard
            // 
            this.labelVStandard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVStandard.AutoSize = true;
            this.labelVStandard.Location = new System.Drawing.Point(478, 109);
            this.labelVStandard.Name = "labelVStandard";
            this.labelVStandard.Size = new System.Drawing.Size(78, 13);
            this.labelVStandard.TabIndex = 12;
            this.labelVStandard.Text = "Video standard";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(478, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Video source";
            // 
            // comboBoxSources
            // 
            this.comboBoxSources.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSources.FormattingEnabled = true;
            this.comboBoxSources.Location = new System.Drawing.Point(562, 133);
            this.comboBoxSources.Name = "comboBoxSources";
            this.comboBoxSources.Size = new System.Drawing.Size(170, 21);
            this.comboBoxSources.TabIndex = 14;
            this.comboBoxSources.SelectedIndexChanged += new System.EventHandler(this.comboBoxSources_SelectedIndexChanged);
            // 
            // CrossbarVideoPlayer
            // 
            this._crossbarVideoPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._crossbarVideoPlayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._crossbarVideoPlayer.Location = new System.Drawing.Point(12, 12);
            this._crossbarVideoPlayer.Name = "_crossbarVideoPlayer";
            this._crossbarVideoPlayer.Size = new System.Drawing.Size(459, 386);
            this._crossbarVideoPlayer.TabIndex = 15;
            this._crossbarVideoPlayer.UseBlackBands = false;
            // 
            // FormTestTubeVideoPlayerDShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 410);
            this.Controls.Add(this._crossbarVideoPlayer);
            this.Controls.Add(this.comboBoxSources);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelVStandard);
            this.Controls.Add(this.comboBoxStandards);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBoxZoom);
            this.Controls.Add(this.groupBoxAspect);
            this.Controls.Add(this.comboBoxDevices);
            this.Controls.Add(this.buttonStart);
            this.Name = "FormTestTubeVideoPlayerDShow";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormTestTubeVideoPlayerDShow_Load);
            this.groupBoxAspect.ResumeLayout(false);
            this.groupBoxAspect.PerformLayout();
            this.groupBoxZoom.ResumeLayout(false);
            this.groupBoxZoom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ComboBox comboBoxDevices;
        private System.Windows.Forms.RadioButton radioButtonKeep;
        private System.Windows.Forms.RadioButton radioButton169;
        private System.Windows.Forms.RadioButton radioButton43;
        private System.Windows.Forms.GroupBox groupBoxAspect;
        private System.Windows.Forms.GroupBox groupBoxZoom;
        private System.Windows.Forms.RadioButton radioButtonZoomStretch;
        private System.Windows.Forms.RadioButton radioButtonZoomFit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxStandards;
        private System.Windows.Forms.Label labelVStandard;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxSources;
        private VideoPlayerDShowLib.CrossbarVideoPlayer _crossbarVideoPlayer;
    }
}

