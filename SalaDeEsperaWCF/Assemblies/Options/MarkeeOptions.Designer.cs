namespace Assemblies.Options
{
    partial class MarkeeOptions
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
            this.listBoxTextList = new System.Windows.Forms.ListBox();
            this.textBoxText = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.groupBoxTextList = new System.Windows.Forms.GroupBox();
            this.buttonRemoveItems = new System.Windows.Forms.Button();
            this.buttonMoveItemsDown = new System.Windows.Forms.Button();
            this.buttonMoveItemsUp = new System.Windows.Forms.Button();
            this.trackBarSpeed = new System.Windows.Forms.TrackBar();
            this.groupBoxProperties = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarBackgroundTransparency = new Dotnetrix.Controls.TrackBar();
            this.buttonBackColor = new System.Windows.Forms.Button();
            this.buttonTextColor = new System.Windows.Forms.Button();
            this.buttonFont = new System.Windows.Forms.Button();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.groupBoxPreview = new System.Windows.Forms.GroupBox();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.checkBoxBorder = new System.Windows.Forms.CheckBox();
            this.groupBoxTextList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).BeginInit();
            this.groupBoxProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBackgroundTransparency)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxTextList
            // 
            this.listBoxTextList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxTextList.FormattingEnabled = true;
            this.listBoxTextList.Location = new System.Drawing.Point(6, 19);
            this.listBoxTextList.Name = "listBoxTextList";
            this.listBoxTextList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxTextList.Size = new System.Drawing.Size(410, 160);
            this.listBoxTextList.TabIndex = 0;
            this.listBoxTextList.SelectedValueChanged += new System.EventHandler(this.listBoxTextList_SelectedValueChanged);
            this.listBoxTextList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxTextList_KeyDown);
            // 
            // textBoxText
            // 
            this.textBoxText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxText.Location = new System.Drawing.Point(6, 194);
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.Size = new System.Drawing.Size(356, 20);
            this.textBoxText.TabIndex = 1;
            this.textBoxText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxText_KeyDown);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Location = new System.Drawing.Point(368, 192);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "Adicionar";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // groupBoxTextList
            // 
            this.groupBoxTextList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTextList.Controls.Add(this.buttonRemoveItems);
            this.groupBoxTextList.Controls.Add(this.buttonMoveItemsDown);
            this.groupBoxTextList.Controls.Add(this.buttonMoveItemsUp);
            this.groupBoxTextList.Controls.Add(this.listBoxTextList);
            this.groupBoxTextList.Controls.Add(this.buttonAdd);
            this.groupBoxTextList.Controls.Add(this.textBoxText);
            this.groupBoxTextList.Location = new System.Drawing.Point(12, 12);
            this.groupBoxTextList.Name = "groupBoxTextList";
            this.groupBoxTextList.Size = new System.Drawing.Size(449, 221);
            this.groupBoxTextList.TabIndex = 3;
            this.groupBoxTextList.TabStop = false;
            this.groupBoxTextList.Text = "Lista de textos";
            // 
            // buttonRemoveItems
            // 
            this.buttonRemoveItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveItems.Location = new System.Drawing.Point(422, 61);
            this.buttonRemoveItems.Name = "buttonRemoveItems";
            this.buttonRemoveItems.Size = new System.Drawing.Size(21, 23);
            this.buttonRemoveItems.TabIndex = 5;
            this.buttonRemoveItems.Text = "-";
            this.buttonRemoveItems.UseVisualStyleBackColor = true;
            this.buttonRemoveItems.Click += new System.EventHandler(this.buttonRemoveItems_Click);
            // 
            // buttonMoveItemsDown
            // 
            this.buttonMoveItemsDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveItemsDown.Location = new System.Drawing.Point(422, 90);
            this.buttonMoveItemsDown.Name = "buttonMoveItemsDown";
            this.buttonMoveItemsDown.Size = new System.Drawing.Size(21, 36);
            this.buttonMoveItemsDown.TabIndex = 4;
            this.buttonMoveItemsDown.Text = "↓";
            this.buttonMoveItemsDown.UseVisualStyleBackColor = true;
            this.buttonMoveItemsDown.Click += new System.EventHandler(this.buttonMoveItemsDown_Click);
            // 
            // buttonMoveItemsUp
            // 
            this.buttonMoveItemsUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveItemsUp.Location = new System.Drawing.Point(422, 19);
            this.buttonMoveItemsUp.Name = "buttonMoveItemsUp";
            this.buttonMoveItemsUp.Size = new System.Drawing.Size(21, 36);
            this.buttonMoveItemsUp.TabIndex = 3;
            this.buttonMoveItemsUp.Text = "↑";
            this.buttonMoveItemsUp.UseVisualStyleBackColor = true;
            this.buttonMoveItemsUp.Click += new System.EventHandler(this.buttonMoveItemsUp_Click);
            // 
            // trackBarSpeed
            // 
            this.trackBarSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarSpeed.Location = new System.Drawing.Point(6, 19);
            this.trackBarSpeed.Maximum = 11;
            this.trackBarSpeed.Name = "trackBarSpeed";
            this.trackBarSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBarSpeed.Size = new System.Drawing.Size(134, 45);
            this.trackBarSpeed.TabIndex = 3;
            this.trackBarSpeed.Value = 3;
            this.trackBarSpeed.ValueChanged += new System.EventHandler(this.trackBarSpeed_ValueChanged);
            // 
            // groupBoxProperties
            // 
            this.groupBoxProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProperties.Controls.Add(this.checkBoxBorder);
            this.groupBoxProperties.Controls.Add(this.label1);
            this.groupBoxProperties.Controls.Add(this.trackBarBackgroundTransparency);
            this.groupBoxProperties.Controls.Add(this.buttonBackColor);
            this.groupBoxProperties.Controls.Add(this.buttonTextColor);
            this.groupBoxProperties.Controls.Add(this.buttonFont);
            this.groupBoxProperties.Controls.Add(this.labelSpeed);
            this.groupBoxProperties.Controls.Add(this.trackBarSpeed);
            this.groupBoxProperties.Location = new System.Drawing.Point(12, 239);
            this.groupBoxProperties.Name = "groupBoxProperties";
            this.groupBoxProperties.Size = new System.Drawing.Size(449, 82);
            this.groupBoxProperties.TabIndex = 4;
            this.groupBoxProperties.TabStop = false;
            this.groupBoxProperties.Text = "Propriedades de apresentação";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(341, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Transparência";
            // 
            // trackBarBackgroundTransparency
            // 
            this.trackBarBackgroundTransparency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarBackgroundTransparency.Location = new System.Drawing.Point(322, 19);
            this.trackBarBackgroundTransparency.Maximum = 255;
            this.trackBarBackgroundTransparency.Name = "trackBarBackgroundTransparency";
            this.trackBarBackgroundTransparency.Size = new System.Drawing.Size(121, 45);
            this.trackBarBackgroundTransparency.TabIndex = 17;
            this.trackBarBackgroundTransparency.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarBackgroundTransparency.ValueChanged += new System.EventHandler(this.trackBarBackgroundTransparency_ValueChanged);
            // 
            // buttonBackColor
            // 
            this.buttonBackColor.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonBackColor.Location = new System.Drawing.Point(234, 48);
            this.buttonBackColor.Name = "buttonBackColor";
            this.buttonBackColor.Size = new System.Drawing.Size(82, 23);
            this.buttonBackColor.TabIndex = 16;
            this.buttonBackColor.Text = "Cor do fundo";
            this.buttonBackColor.UseVisualStyleBackColor = true;
            this.buttonBackColor.Click += new System.EventHandler(this.buttonBackColor_Click);
            // 
            // buttonTextColor
            // 
            this.buttonTextColor.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonTextColor.Location = new System.Drawing.Point(234, 19);
            this.buttonTextColor.Name = "buttonTextColor";
            this.buttonTextColor.Size = new System.Drawing.Size(82, 23);
            this.buttonTextColor.TabIndex = 15;
            this.buttonTextColor.Text = "Cor do texto";
            this.buttonTextColor.UseVisualStyleBackColor = true;
            this.buttonTextColor.Click += new System.EventHandler(this.buttonTextColor_Click);
            // 
            // buttonFont
            // 
            this.buttonFont.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonFont.Location = new System.Drawing.Point(146, 19);
            this.buttonFont.Name = "buttonFont";
            this.buttonFont.Size = new System.Drawing.Size(82, 23);
            this.buttonFont.TabIndex = 5;
            this.buttonFont.Text = "Tipo de letra";
            this.buttonFont.UseVisualStyleBackColor = true;
            this.buttonFont.Click += new System.EventHandler(this.buttonFont_Click);
            // 
            // labelSpeed
            // 
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Location = new System.Drawing.Point(37, 53);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(72, 13);
            this.labelSpeed.TabIndex = 4;
            this.labelSpeed.Text = "Velocidade: 3";
            // 
            // groupBoxPreview
            // 
            this.groupBoxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPreview.Location = new System.Drawing.Point(12, 327);
            this.groupBoxPreview.Name = "groupBoxPreview";
            this.groupBoxPreview.Size = new System.Drawing.Size(449, 94);
            this.groupBoxPreview.TabIndex = 0;
            this.groupBoxPreview.TabStop = false;
            this.groupBoxPreview.Text = "Pré-visualização";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(305, 427);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "Ok";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(386, 427);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancelar";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonPause
            // 
            this.buttonPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPause.Location = new System.Drawing.Point(224, 427);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(75, 23);
            this.buttonPause.TabIndex = 7;
            this.buttonPause.Text = "Pausa";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // checkBoxBorder
            // 
            this.checkBoxBorder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.checkBoxBorder.AutoSize = true;
            this.checkBoxBorder.Location = new System.Drawing.Point(160, 50);
            this.checkBoxBorder.Name = "checkBoxBorder";
            this.checkBoxBorder.Size = new System.Drawing.Size(53, 17);
            this.checkBoxBorder.TabIndex = 19;
            this.checkBoxBorder.Text = "Limite";
            this.checkBoxBorder.UseVisualStyleBackColor = true;
            this.checkBoxBorder.CheckedChanged += new System.EventHandler(this.checkBoxBorder_CheckedChanged);
            // 
            // MarkeeOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(473, 460);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxPreview);
            this.Controls.Add(this.groupBoxProperties);
            this.Controls.Add(this.groupBoxTextList);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(394, 442);
            this.Name = "MarkeeOptions";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "FooterOptions";
            this.Load += new System.EventHandler(this.FooterOptions_Load);
            this.groupBoxTextList.ResumeLayout(false);
            this.groupBoxTextList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).EndInit();
            this.groupBoxProperties.ResumeLayout(false);
            this.groupBoxProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBackgroundTransparency)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxTextList;
        private System.Windows.Forms.TextBox textBoxText;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.GroupBox groupBoxTextList;
        private System.Windows.Forms.TrackBar trackBarSpeed;
        private System.Windows.Forms.GroupBox groupBoxProperties;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.GroupBox groupBoxPreview;
        private System.Windows.Forms.Button buttonFont;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button buttonMoveItemsUp;
        private System.Windows.Forms.Button buttonRemoveItems;
        private System.Windows.Forms.Button buttonMoveItemsDown;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Label label1;
        private Dotnetrix.Controls.TrackBar trackBarBackgroundTransparency;
        private System.Windows.Forms.Button buttonBackColor;
        private System.Windows.Forms.Button buttonTextColor;
        private System.Windows.Forms.CheckBox checkBoxBorder;
    }
}