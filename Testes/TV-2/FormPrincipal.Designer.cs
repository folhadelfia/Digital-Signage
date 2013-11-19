

using TV_2.Properties;
using TV2Lib;
namespace TV_2
{
    partial class FormPrincipal
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
            TV2Lib.Settings settings1 = new TV2Lib.Settings();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxSignalStatistics = new System.Windows.Forms.GroupBox();
            this.checkBoxSignalLocked = new System.Windows.Forms.CheckBox();
            this.checkBoxSignalPresent = new System.Windows.Forms.CheckBox();
            this.labelSignalQuality = new System.Windows.Forms.Label();
            this.labelSignalStrength = new System.Windows.Forms.Label();
            this.progressBarSignalQuality = new System.Windows.Forms.ProgressBar();
            this.progressBarSignalStrength = new System.Windows.Forms.ProgressBar();
            this.buttonRefreshSignalStatistics = new System.Windows.Forms.Button();
            this.groupBoxScan = new System.Windows.Forms.GroupBox();
            this.groupBoxCustomScan = new System.Windows.Forms.GroupBox();
            this.labelMin = new System.Windows.Forms.Label();
            this.textBoxMin = new System.Windows.Forms.TextBox();
            this.labelStep = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.textBoxStep = new System.Windows.Forms.TextBox();
            this.textBoxMax = new System.Windows.Forms.TextBox();
            this.buttonScanFreqs = new System.Windows.Forms.Button();
            this.radioButtonCustom = new System.Windows.Forms.RadioButton();
            this.radioButtonAuto = new System.Windows.Forms.RadioButton();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.groupBoxChannels = new System.Windows.Forms.GroupBox();
            this.buttonRefreshChannels = new System.Windows.Forms.Button();
            this.checkBoxForceRebuild = new System.Windows.Forms.CheckBox();
            this.listViewCanais = new System.Windows.Forms.ListView();
            this.groupBoxDevices = new System.Windows.Forms.GroupBox();
            this.comboBoxTunerDevice = new System.Windows.Forms.ComboBox();
            this.labelTunerDevice = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelMPEG2Codec = new System.Windows.Forms.Label();
            this.labelH264Codec = new System.Windows.Forms.Label();
            this.labelARenderer = new System.Windows.Forms.Label();
            this.labelACodec = new System.Windows.Forms.Label();
            this.comboBoxMPEG2Codec = new System.Windows.Forms.ComboBox();
            this.comboBoxARenderer = new System.Windows.Forms.ComboBox();
            this.comboBoxH264Codec = new System.Windows.Forms.ComboBox();
            this.comboBoxACodec = new System.Windows.Forms.ComboBox();
            this.buttonSaveChannelsToXML = new System.Windows.Forms.Button();
            this.buttonLoadChannelsFromXML = new System.Windows.Forms.Button();
            this.digitalTVScreen = new TV2Lib.DigitalTVScreen();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxSignalStatistics.SuspendLayout();
            this.groupBoxScan.SuspendLayout();
            this.groupBoxCustomScan.SuspendLayout();
            this.groupBoxChannels.SuspendLayout();
            this.groupBoxDevices.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(8, 6);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.digitalTVScreen);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxSignalStatistics);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxScan);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxLog);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxChannels);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxDevices);
            this.splitContainer1.Size = new System.Drawing.Size(927, 480);
            this.splitContainer1.SplitterDistance = 464;
            this.splitContainer1.TabIndex = 9;
            // 
            // groupBoxSignalStatistics
            // 
            this.groupBoxSignalStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSignalStatistics.Controls.Add(this.checkBoxSignalLocked);
            this.groupBoxSignalStatistics.Controls.Add(this.checkBoxSignalPresent);
            this.groupBoxSignalStatistics.Controls.Add(this.labelSignalQuality);
            this.groupBoxSignalStatistics.Controls.Add(this.labelSignalStrength);
            this.groupBoxSignalStatistics.Controls.Add(this.progressBarSignalQuality);
            this.groupBoxSignalStatistics.Controls.Add(this.progressBarSignalStrength);
            this.groupBoxSignalStatistics.Controls.Add(this.buttonRefreshSignalStatistics);
            this.groupBoxSignalStatistics.Location = new System.Drawing.Point(6, 424);
            this.groupBoxSignalStatistics.Name = "groupBoxSignalStatistics";
            this.groupBoxSignalStatistics.Size = new System.Drawing.Size(449, 50);
            this.groupBoxSignalStatistics.TabIndex = 5;
            this.groupBoxSignalStatistics.TabStop = false;
            this.groupBoxSignalStatistics.Text = "Statistics";
            // 
            // checkBoxSignalLocked
            // 
            this.checkBoxSignalLocked.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.checkBoxSignalLocked.AutoSize = true;
            this.checkBoxSignalLocked.Location = new System.Drawing.Point(239, 20);
            this.checkBoxSignalLocked.Name = "checkBoxSignalLocked";
            this.checkBoxSignalLocked.Size = new System.Drawing.Size(62, 17);
            this.checkBoxSignalLocked.TabIndex = 14;
            this.checkBoxSignalLocked.Text = "Locked";
            this.checkBoxSignalLocked.UseVisualStyleBackColor = true;
            // 
            // checkBoxSignalPresent
            // 
            this.checkBoxSignalPresent.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.checkBoxSignalPresent.AutoSize = true;
            this.checkBoxSignalPresent.Location = new System.Drawing.Point(303, 20);
            this.checkBoxSignalPresent.Name = "checkBoxSignalPresent";
            this.checkBoxSignalPresent.Size = new System.Drawing.Size(62, 17);
            this.checkBoxSignalPresent.TabIndex = 13;
            this.checkBoxSignalPresent.Text = "Present";
            this.checkBoxSignalPresent.UseVisualStyleBackColor = true;
            // 
            // labelSignalQuality
            // 
            this.labelSignalQuality.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelSignalQuality.AutoSize = true;
            this.labelSignalQuality.Location = new System.Drawing.Point(17, 21);
            this.labelSignalQuality.Name = "labelSignalQuality";
            this.labelSignalQuality.Size = new System.Drawing.Size(40, 13);
            this.labelSignalQuality.TabIndex = 12;
            this.labelSignalQuality.Text = "Q ##%";
            // 
            // labelSignalStrength
            // 
            this.labelSignalStrength.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelSignalStrength.AutoSize = true;
            this.labelSignalStrength.Location = new System.Drawing.Point(124, 21);
            this.labelSignalStrength.Name = "labelSignalStrength";
            this.labelSignalStrength.Size = new System.Drawing.Size(39, 13);
            this.labelSignalStrength.TabIndex = 11;
            this.labelSignalStrength.Text = "S ##%";
            // 
            // progressBarSignalQuality
            // 
            this.progressBarSignalQuality.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.progressBarSignalQuality.Location = new System.Drawing.Point(58, 21);
            this.progressBarSignalQuality.Name = "progressBarSignalQuality";
            this.progressBarSignalQuality.Size = new System.Drawing.Size(60, 13);
            this.progressBarSignalQuality.TabIndex = 6;
            // 
            // progressBarSignalStrength
            // 
            this.progressBarSignalStrength.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.progressBarSignalStrength.Location = new System.Drawing.Point(165, 21);
            this.progressBarSignalStrength.Name = "progressBarSignalStrength";
            this.progressBarSignalStrength.Size = new System.Drawing.Size(60, 13);
            this.progressBarSignalStrength.TabIndex = 5;
            // 
            // buttonRefreshSignalStatistics
            // 
            this.buttonRefreshSignalStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefreshSignalStatistics.Location = new System.Drawing.Point(368, 16);
            this.buttonRefreshSignalStatistics.Name = "buttonRefreshSignalStatistics";
            this.buttonRefreshSignalStatistics.Size = new System.Drawing.Size(75, 23);
            this.buttonRefreshSignalStatistics.TabIndex = 4;
            this.buttonRefreshSignalStatistics.Text = "Refresh";
            this.buttonRefreshSignalStatistics.UseVisualStyleBackColor = true;
            this.buttonRefreshSignalStatistics.Click += new System.EventHandler(this.buttonRefreshSignalStatistics_Click);
            // 
            // groupBoxScan
            // 
            this.groupBoxScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxScan.Controls.Add(this.groupBoxCustomScan);
            this.groupBoxScan.Controls.Add(this.buttonScanFreqs);
            this.groupBoxScan.Controls.Add(this.radioButtonCustom);
            this.groupBoxScan.Controls.Add(this.radioButtonAuto);
            this.groupBoxScan.Location = new System.Drawing.Point(3, 275);
            this.groupBoxScan.Name = "groupBoxScan";
            this.groupBoxScan.Size = new System.Drawing.Size(172, 202);
            this.groupBoxScan.TabIndex = 8;
            this.groupBoxScan.TabStop = false;
            this.groupBoxScan.Text = "Scan";
            // 
            // groupBoxCustomScan
            // 
            this.groupBoxCustomScan.Controls.Add(this.labelMin);
            this.groupBoxCustomScan.Controls.Add(this.textBoxMin);
            this.groupBoxCustomScan.Controls.Add(this.labelStep);
            this.groupBoxCustomScan.Controls.Add(this.labelMax);
            this.groupBoxCustomScan.Controls.Add(this.textBoxStep);
            this.groupBoxCustomScan.Controls.Add(this.textBoxMax);
            this.groupBoxCustomScan.Enabled = false;
            this.groupBoxCustomScan.Location = new System.Drawing.Point(6, 65);
            this.groupBoxCustomScan.Name = "groupBoxCustomScan";
            this.groupBoxCustomScan.Size = new System.Drawing.Size(154, 103);
            this.groupBoxCustomScan.TabIndex = 20;
            this.groupBoxCustomScan.TabStop = false;
            this.groupBoxCustomScan.EnabledChanged += new System.EventHandler(this.groupBoxCustomScan_EnabledChanged);
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.Location = new System.Drawing.Point(6, 22);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(48, 13);
            this.labelMin.TabIndex = 27;
            this.labelMin.Text = "Min freq.";
            // 
            // textBoxMin
            // 
            this.textBoxMin.Location = new System.Drawing.Point(63, 19);
            this.textBoxMin.Name = "textBoxMin";
            this.textBoxMin.Size = new System.Drawing.Size(85, 20);
            this.textBoxMin.TabIndex = 26;
            // 
            // labelStep
            // 
            this.labelStep.AutoSize = true;
            this.labelStep.Location = new System.Drawing.Point(6, 74);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(29, 13);
            this.labelStep.TabIndex = 25;
            this.labelStep.Text = "Step";
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.Location = new System.Drawing.Point(6, 48);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(51, 13);
            this.labelMax.TabIndex = 23;
            this.labelMax.Text = "Max freq.";
            // 
            // textBoxStep
            // 
            this.textBoxStep.Location = new System.Drawing.Point(63, 71);
            this.textBoxStep.Name = "textBoxStep";
            this.textBoxStep.Size = new System.Drawing.Size(85, 20);
            this.textBoxStep.TabIndex = 22;
            // 
            // textBoxMax
            // 
            this.textBoxMax.Location = new System.Drawing.Point(63, 45);
            this.textBoxMax.Name = "textBoxMax";
            this.textBoxMax.Size = new System.Drawing.Size(85, 20);
            this.textBoxMax.TabIndex = 20;
            // 
            // buttonScanFreqs
            // 
            this.buttonScanFreqs.Location = new System.Drawing.Point(85, 174);
            this.buttonScanFreqs.Name = "buttonScanFreqs";
            this.buttonScanFreqs.Size = new System.Drawing.Size(75, 23);
            this.buttonScanFreqs.TabIndex = 13;
            this.buttonScanFreqs.Text = "Scan Freqs";
            this.buttonScanFreqs.UseVisualStyleBackColor = true;
            this.buttonScanFreqs.Click += new System.EventHandler(this.buttonScanFreqs_Click);
            // 
            // radioButtonCustom
            // 
            this.radioButtonCustom.AutoSize = true;
            this.radioButtonCustom.Location = new System.Drawing.Point(6, 42);
            this.radioButtonCustom.Name = "radioButtonCustom";
            this.radioButtonCustom.Size = new System.Drawing.Size(60, 17);
            this.radioButtonCustom.TabIndex = 1;
            this.radioButtonCustom.Text = "Custom";
            this.radioButtonCustom.UseVisualStyleBackColor = true;
            this.radioButtonCustom.CheckedChanged += new System.EventHandler(this.radioButtonCustom_CheckedChanged);
            // 
            // radioButtonAuto
            // 
            this.radioButtonAuto.AutoSize = true;
            this.radioButtonAuto.Checked = true;
            this.radioButtonAuto.Location = new System.Drawing.Point(6, 19);
            this.radioButtonAuto.Name = "radioButtonAuto";
            this.radioButtonAuto.Size = new System.Drawing.Size(72, 17);
            this.radioButtonAuto.TabIndex = 0;
            this.radioButtonAuto.TabStop = true;
            this.radioButtonAuto.Text = "Automatic";
            this.radioButtonAuto.UseVisualStyleBackColor = true;
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textBoxLog.Location = new System.Drawing.Point(181, 0);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(275, 269);
            this.textBoxLog.TabIndex = 1;
            // 
            // groupBoxChannels
            // 
            this.groupBoxChannels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxChannels.Controls.Add(this.buttonLoadChannelsFromXML);
            this.groupBoxChannels.Controls.Add(this.buttonSaveChannelsToXML);
            this.groupBoxChannels.Controls.Add(this.buttonRefreshChannels);
            this.groupBoxChannels.Controls.Add(this.checkBoxForceRebuild);
            this.groupBoxChannels.Controls.Add(this.listViewCanais);
            this.groupBoxChannels.Location = new System.Drawing.Point(3, 3);
            this.groupBoxChannels.Name = "groupBoxChannels";
            this.groupBoxChannels.Size = new System.Drawing.Size(172, 266);
            this.groupBoxChannels.TabIndex = 7;
            this.groupBoxChannels.TabStop = false;
            this.groupBoxChannels.Text = "Channels";
            // 
            // buttonRefreshChannels
            // 
            this.buttonRefreshChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRefreshChannels.Location = new System.Drawing.Point(91, 204);
            this.buttonRefreshChannels.Name = "buttonRefreshChannels";
            this.buttonRefreshChannels.Size = new System.Drawing.Size(75, 23);
            this.buttonRefreshChannels.TabIndex = 12;
            this.buttonRefreshChannels.Text = "Refresh";
            this.buttonRefreshChannels.UseVisualStyleBackColor = true;
            this.buttonRefreshChannels.Click += new System.EventHandler(this.buttonRefreshChannels_Click);
            // 
            // checkBoxForceRebuild
            // 
            this.checkBoxForceRebuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxForceRebuild.AutoSize = true;
            this.checkBoxForceRebuild.Location = new System.Drawing.Point(6, 208);
            this.checkBoxForceRebuild.Name = "checkBoxForceRebuild";
            this.checkBoxForceRebuild.Size = new System.Drawing.Size(87, 17);
            this.checkBoxForceRebuild.TabIndex = 13;
            this.checkBoxForceRebuild.Text = "Force rebuild";
            this.checkBoxForceRebuild.UseVisualStyleBackColor = true;
            this.checkBoxForceRebuild.CheckedChanged += new System.EventHandler(this.checkBoxForceRebuild_CheckedChanged);
            // 
            // listViewCanais
            // 
            this.listViewCanais.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewCanais.FullRowSelect = true;
            this.listViewCanais.GridLines = true;
            this.listViewCanais.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewCanais.Location = new System.Drawing.Point(3, 16);
            this.listViewCanais.MultiSelect = false;
            this.listViewCanais.Name = "listViewCanais";
            this.listViewCanais.Size = new System.Drawing.Size(166, 182);
            this.listViewCanais.TabIndex = 11;
            this.listViewCanais.UseCompatibleStateImageBehavior = false;
            this.listViewCanais.View = System.Windows.Forms.View.List;
            this.listViewCanais.DoubleClick += new System.EventHandler(this.listViewCanais_DoubleClick);
            // 
            // groupBoxDevices
            // 
            this.groupBoxDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDevices.Controls.Add(this.comboBoxTunerDevice);
            this.groupBoxDevices.Controls.Add(this.labelTunerDevice);
            this.groupBoxDevices.Controls.Add(this.buttonStart);
            this.groupBoxDevices.Controls.Add(this.labelMPEG2Codec);
            this.groupBoxDevices.Controls.Add(this.labelH264Codec);
            this.groupBoxDevices.Controls.Add(this.labelARenderer);
            this.groupBoxDevices.Controls.Add(this.labelACodec);
            this.groupBoxDevices.Controls.Add(this.comboBoxMPEG2Codec);
            this.groupBoxDevices.Controls.Add(this.comboBoxARenderer);
            this.groupBoxDevices.Controls.Add(this.comboBoxH264Codec);
            this.groupBoxDevices.Controls.Add(this.comboBoxACodec);
            this.groupBoxDevices.Location = new System.Drawing.Point(181, 275);
            this.groupBoxDevices.Name = "groupBoxDevices";
            this.groupBoxDevices.Size = new System.Drawing.Size(275, 199);
            this.groupBoxDevices.TabIndex = 2;
            this.groupBoxDevices.TabStop = false;
            this.groupBoxDevices.Text = "Devices";
            // 
            // comboBoxTunerDevice
            // 
            this.comboBoxTunerDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTunerDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTunerDevice.FormattingEnabled = true;
            this.comboBoxTunerDevice.Location = new System.Drawing.Point(98, 20);
            this.comboBoxTunerDevice.Name = "comboBoxTunerDevice";
            this.comboBoxTunerDevice.Size = new System.Drawing.Size(171, 21);
            this.comboBoxTunerDevice.TabIndex = 12;
            this.comboBoxTunerDevice.SelectedIndexChanged += new System.EventHandler(this.comboBoxTunerDevice_SelectedIndexChanged);
            this.comboBoxTunerDevice.SelectedValueChanged += new System.EventHandler(this.comboBoxTunerDevice_SelectedValueChanged);
            // 
            // labelTunerDevice
            // 
            this.labelTunerDevice.AutoSize = true;
            this.labelTunerDevice.Location = new System.Drawing.Point(22, 23);
            this.labelTunerDevice.Name = "labelTunerDevice";
            this.labelTunerDevice.Size = new System.Drawing.Size(70, 13);
            this.labelTunerDevice.TabIndex = 11;
            this.labelTunerDevice.Text = "Tuner device";
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Location = new System.Drawing.Point(126, 155);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(143, 38);
            this.buttonStart.TabIndex = 8;
            this.buttonStart.Text = "Crashar esta merda";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // labelMPEG2Codec
            // 
            this.labelMPEG2Codec.AutoSize = true;
            this.labelMPEG2Codec.Location = new System.Drawing.Point(6, 131);
            this.labelMPEG2Codec.Name = "labelMPEG2Codec";
            this.labelMPEG2Codec.Size = new System.Drawing.Size(86, 13);
            this.labelMPEG2Codec.TabIndex = 7;
            this.labelMPEG2Codec.Text = "MPEG2 decoder";
            // 
            // labelH264Codec
            // 
            this.labelH264Codec.AutoSize = true;
            this.labelH264Codec.Location = new System.Drawing.Point(17, 104);
            this.labelH264Codec.Name = "labelH264Codec";
            this.labelH264Codec.Size = new System.Drawing.Size(75, 13);
            this.labelH264Codec.TabIndex = 6;
            this.labelH264Codec.Text = "H264 decoder";
            // 
            // labelARenderer
            // 
            this.labelARenderer.AutoSize = true;
            this.labelARenderer.Location = new System.Drawing.Point(16, 77);
            this.labelARenderer.Name = "labelARenderer";
            this.labelARenderer.Size = new System.Drawing.Size(76, 13);
            this.labelARenderer.TabIndex = 5;
            this.labelARenderer.Text = "Audio renderer";
            // 
            // labelACodec
            // 
            this.labelACodec.AutoSize = true;
            this.labelACodec.Location = new System.Drawing.Point(16, 50);
            this.labelACodec.Name = "labelACodec";
            this.labelACodec.Size = new System.Drawing.Size(76, 13);
            this.labelACodec.TabIndex = 4;
            this.labelACodec.Text = "Audio decoder";
            // 
            // comboBoxMPEG2Codec
            // 
            this.comboBoxMPEG2Codec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMPEG2Codec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMPEG2Codec.FormattingEnabled = true;
            this.comboBoxMPEG2Codec.Location = new System.Drawing.Point(98, 128);
            this.comboBoxMPEG2Codec.Name = "comboBoxMPEG2Codec";
            this.comboBoxMPEG2Codec.Size = new System.Drawing.Size(171, 21);
            this.comboBoxMPEG2Codec.TabIndex = 3;
            this.comboBoxMPEG2Codec.SelectedIndexChanged += new System.EventHandler(this.comboBoxMPEG2Codec_SelectedValueChanged);
            // 
            // comboBoxARenderer
            // 
            this.comboBoxARenderer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxARenderer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxARenderer.FormattingEnabled = true;
            this.comboBoxARenderer.Location = new System.Drawing.Point(98, 74);
            this.comboBoxARenderer.Name = "comboBoxARenderer";
            this.comboBoxARenderer.Size = new System.Drawing.Size(171, 21);
            this.comboBoxARenderer.TabIndex = 2;
            this.comboBoxARenderer.SelectedIndexChanged += new System.EventHandler(this.comboBoxARenderer_SelectedValueChanged);
            // 
            // comboBoxH264Codec
            // 
            this.comboBoxH264Codec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxH264Codec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxH264Codec.FormattingEnabled = true;
            this.comboBoxH264Codec.Location = new System.Drawing.Point(98, 101);
            this.comboBoxH264Codec.Name = "comboBoxH264Codec";
            this.comboBoxH264Codec.Size = new System.Drawing.Size(171, 21);
            this.comboBoxH264Codec.TabIndex = 1;
            this.comboBoxH264Codec.SelectedIndexChanged += new System.EventHandler(this.comboBoxH264Codec_SelectedValueChanged);
            // 
            // comboBoxACodec
            // 
            this.comboBoxACodec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxACodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxACodec.FormattingEnabled = true;
            this.comboBoxACodec.Location = new System.Drawing.Point(98, 47);
            this.comboBoxACodec.Name = "comboBoxACodec";
            this.comboBoxACodec.Size = new System.Drawing.Size(171, 21);
            this.comboBoxACodec.TabIndex = 0;
            this.comboBoxACodec.SelectedIndexChanged += new System.EventHandler(this.comboBoxACodec_SelectedValueChanged);
            // 
            // buttonSaveChannelsToXML
            // 
            this.buttonSaveChannelsToXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveChannelsToXML.Location = new System.Drawing.Point(91, 233);
            this.buttonSaveChannelsToXML.Name = "buttonSaveChannelsToXML";
            this.buttonSaveChannelsToXML.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveChannelsToXML.TabIndex = 15;
            this.buttonSaveChannelsToXML.Text = "Save";
            this.buttonSaveChannelsToXML.UseVisualStyleBackColor = true;
            this.buttonSaveChannelsToXML.Click += new System.EventHandler(this.buttonSaveChannelsToXML_Click);
            // 
            // buttonLoadChannelsFromXML
            // 
            this.buttonLoadChannelsFromXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadChannelsFromXML.Location = new System.Drawing.Point(10, 233);
            this.buttonLoadChannelsFromXML.Name = "buttonLoadChannelsFromXML";
            this.buttonLoadChannelsFromXML.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadChannelsFromXML.TabIndex = 16;
            this.buttonLoadChannelsFromXML.Text = "Load";
            this.buttonLoadChannelsFromXML.UseVisualStyleBackColor = true;
            this.buttonLoadChannelsFromXML.Click += new System.EventHandler(this.buttonLoadChannelsFromXML_Click);
            // 
            // digitalTVScreen
            // 
            this.digitalTVScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.digitalTVScreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.digitalTVScreen.CurrentGraphBuilder = null;
            this.digitalTVScreen.Location = new System.Drawing.Point(6, 6);
            this.digitalTVScreen.MinimumSize = new System.Drawing.Size(50, 50);
            this.digitalTVScreen.Name = "digitalTVScreen";
            settings1.Balance = 0;
            settings1.SnapshotsFolder = "Snapshots";
            settings1.StartVideoMode = TV2Lib.VideoMode.Normal;
            settings1.TimeShiftingActivated = false;
            settings1.TimeShiftingBufferLengthMax = 180;
            settings1.TimeShiftingBufferLengthMin = 180;
            settings1.UseVideo169Mode = false;
            settings1.UseWPF = false;
            settings1.VideoBackgroundColor = System.Drawing.Color.Black;
            settings1.VideoBackgroundColorString = "Black";
            settings1.VideosFolder = "Recorder";
            settings1.Volume = 0;
            this.digitalTVScreen.Settings = settings1;
            this.digitalTVScreen.Size = new System.Drawing.Size(449, 417);
            this.digitalTVScreen.TabIndex = 0;
            this.digitalTVScreen.UseBlackBands = false;
            this.digitalTVScreen.VideoAspectRatio = 1D;
            this.digitalTVScreen.VideoKeepAspectRatio = true;
            this.digitalTVScreen.VideoOffset = ((System.Drawing.PointF)(resources.GetObject("digitalTVScreen.VideoOffset")));
            this.digitalTVScreen.VideoZoomMode = TV2Lib.VideoSizeMode.FromInside;
            this.digitalTVScreen.VideoZoomValue = 0D;
            this.digitalTVScreen.ChannelListChanged += new TV2Lib.ChannelEventHandler(this.digitalTVScreen_ChannelListChanged);
            this.digitalTVScreen.NewLogMessage += new TV2Lib.BDAGraphEventHandler(this.digitalTVScreen1_NewLogMessage);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 492);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.FormPrincipal_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxSignalStatistics.ResumeLayout(false);
            this.groupBoxSignalStatistics.PerformLayout();
            this.groupBoxScan.ResumeLayout(false);
            this.groupBoxScan.PerformLayout();
            this.groupBoxCustomScan.ResumeLayout(false);
            this.groupBoxCustomScan.PerformLayout();
            this.groupBoxChannels.ResumeLayout(false);
            this.groupBoxChannels.PerformLayout();
            this.groupBoxDevices.ResumeLayout(false);
            this.groupBoxDevices.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DigitalTVScreen digitalTVScreen;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.GroupBox groupBoxDevices;
        private System.Windows.Forms.ComboBox comboBoxMPEG2Codec;
        private System.Windows.Forms.ComboBox comboBoxARenderer;
        private System.Windows.Forms.ComboBox comboBoxH264Codec;
        private System.Windows.Forms.ComboBox comboBoxACodec;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelMPEG2Codec;
        private System.Windows.Forms.Label labelH264Codec;
        private System.Windows.Forms.Label labelARenderer;
        private System.Windows.Forms.Label labelACodec;
        private System.Windows.Forms.Button buttonRefreshSignalStatistics;
        private System.Windows.Forms.GroupBox groupBoxSignalStatistics;
        private System.Windows.Forms.CheckBox checkBoxSignalLocked;
        private System.Windows.Forms.CheckBox checkBoxSignalPresent;
        private System.Windows.Forms.Label labelSignalQuality;
        private System.Windows.Forms.Label labelSignalStrength;
        private System.Windows.Forms.ProgressBar progressBarSignalQuality;
        private System.Windows.Forms.ProgressBar progressBarSignalStrength;
        private System.Windows.Forms.GroupBox groupBoxChannels;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonScanFreqs;
        private System.Windows.Forms.ListView listViewCanais;
        private System.Windows.Forms.Button buttonRefreshChannels;
        private System.Windows.Forms.GroupBox groupBoxScan;
        private System.Windows.Forms.RadioButton radioButtonCustom;
        private System.Windows.Forms.RadioButton radioButtonAuto;
        private System.Windows.Forms.GroupBox groupBoxCustomScan;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.TextBox textBoxStep;
        private System.Windows.Forms.TextBox textBoxMax;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.TextBox textBoxMin;
        private System.Windows.Forms.CheckBox checkBoxForceRebuild;
        private System.Windows.Forms.ComboBox comboBoxTunerDevice;
        private System.Windows.Forms.Label labelTunerDevice;
        private System.Windows.Forms.Button buttonSaveChannelsToXML;
        private System.Windows.Forms.Button buttonLoadChannelsFromXML;
    }
}

