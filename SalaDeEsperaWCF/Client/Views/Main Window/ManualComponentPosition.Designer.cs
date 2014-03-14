namespace Client.Views.Main_Window
{
    partial class ManualComponentBounds
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
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.textBoxBuilderX = new System.Windows.Forms.TextBox();
            this.textBoxBuilderY = new System.Windows.Forms.TextBox();
            this.groupBoxPosBuilder = new System.Windows.Forms.GroupBox();
            this.labelY = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.groupBoxTamanhoBuilder = new System.Windows.Forms.GroupBox();
            this.labelAltura = new System.Windows.Forms.Label();
            this.labelLargura = new System.Windows.Forms.Label();
            this.textBoxBuilderWidth = new System.Windows.Forms.TextBox();
            this.textBoxBuilderHeight = new System.Windows.Forms.TextBox();
            this.groupBoxTamanhoFinal = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxFinalWidth = new System.Windows.Forms.TextBox();
            this.textBoxFinalHeight = new System.Windows.Forms.TextBox();
            this.groupBoxPosFinal = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxFinalX = new System.Windows.Forms.TextBox();
            this.textBoxFinalY = new System.Windows.Forms.TextBox();
            this.groupBoxBuilder = new System.Windows.Forms.GroupBox();
            this.groupBoxPlayer = new System.Windows.Forms.GroupBox();
            this.groupBoxPosBuilder.SuspendLayout();
            this.groupBoxTamanhoBuilder.SuspendLayout();
            this.groupBoxTamanhoFinal.SuspendLayout();
            this.groupBoxPosFinal.SuspendLayout();
            this.groupBoxBuilder.SuspendLayout();
            this.groupBoxPlayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(9, 208);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelar.Location = new System.Drawing.Point(90, 208);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelar.TabIndex = 1;
            this.buttonCancelar.Text = "Cancel";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(171, 208);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 2;
            this.buttonApply.Text = "Prever";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // textBoxBuilderX
            // 
            this.textBoxBuilderX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBuilderX.Location = new System.Drawing.Point(66, 19);
            this.textBoxBuilderX.Name = "textBoxBuilderX";
            this.textBoxBuilderX.Size = new System.Drawing.Size(113, 20);
            this.textBoxBuilderX.TabIndex = 3;
            this.textBoxBuilderX.TextChanged += new System.EventHandler(this.textBoxBuilderX_TextChanged);
            this.textBoxBuilderX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDigitCheck);
            // 
            // textBoxBuilderY
            // 
            this.textBoxBuilderY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBuilderY.Location = new System.Drawing.Point(66, 45);
            this.textBoxBuilderY.Name = "textBoxBuilderY";
            this.textBoxBuilderY.Size = new System.Drawing.Size(113, 20);
            this.textBoxBuilderY.TabIndex = 4;
            this.textBoxBuilderY.TextChanged += new System.EventHandler(this.textBoxBuilderY_TextChanged);
            this.textBoxBuilderY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDigitCheck);
            // 
            // groupBoxPosBuilder
            // 
            this.groupBoxPosBuilder.Controls.Add(this.labelY);
            this.groupBoxPosBuilder.Controls.Add(this.labelX);
            this.groupBoxPosBuilder.Controls.Add(this.textBoxBuilderX);
            this.groupBoxPosBuilder.Controls.Add(this.textBoxBuilderY);
            this.groupBoxPosBuilder.Location = new System.Drawing.Point(6, 19);
            this.groupBoxPosBuilder.Name = "groupBoxPosBuilder";
            this.groupBoxPosBuilder.Size = new System.Drawing.Size(185, 77);
            this.groupBoxPosBuilder.TabIndex = 7;
            this.groupBoxPosBuilder.TabStop = false;
            this.groupBoxPosBuilder.Text = "Posição";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(6, 48);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(42, 13);
            this.labelY.TabIndex = 6;
            this.labelY.Text = "Vertical";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(6, 22);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(54, 13);
            this.labelX.TabIndex = 5;
            this.labelX.Text = "Horizontal";
            // 
            // groupBoxTamanhoBuilder
            // 
            this.groupBoxTamanhoBuilder.Controls.Add(this.labelAltura);
            this.groupBoxTamanhoBuilder.Controls.Add(this.labelLargura);
            this.groupBoxTamanhoBuilder.Controls.Add(this.textBoxBuilderWidth);
            this.groupBoxTamanhoBuilder.Controls.Add(this.textBoxBuilderHeight);
            this.groupBoxTamanhoBuilder.Location = new System.Drawing.Point(6, 102);
            this.groupBoxTamanhoBuilder.Name = "groupBoxTamanhoBuilder";
            this.groupBoxTamanhoBuilder.Size = new System.Drawing.Size(185, 77);
            this.groupBoxTamanhoBuilder.TabIndex = 8;
            this.groupBoxTamanhoBuilder.TabStop = false;
            this.groupBoxTamanhoBuilder.Text = "Tamanho";
            // 
            // labelAltura
            // 
            this.labelAltura.AutoSize = true;
            this.labelAltura.Location = new System.Drawing.Point(6, 48);
            this.labelAltura.Name = "labelAltura";
            this.labelAltura.Size = new System.Drawing.Size(34, 13);
            this.labelAltura.TabIndex = 6;
            this.labelAltura.Text = "Altura";
            // 
            // labelLargura
            // 
            this.labelLargura.AutoSize = true;
            this.labelLargura.Location = new System.Drawing.Point(6, 22);
            this.labelLargura.Name = "labelLargura";
            this.labelLargura.Size = new System.Drawing.Size(43, 13);
            this.labelLargura.TabIndex = 5;
            this.labelLargura.Text = "Largura";
            // 
            // textBoxBuilderWidth
            // 
            this.textBoxBuilderWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBuilderWidth.Location = new System.Drawing.Point(66, 19);
            this.textBoxBuilderWidth.Name = "textBoxBuilderWidth";
            this.textBoxBuilderWidth.Size = new System.Drawing.Size(113, 20);
            this.textBoxBuilderWidth.TabIndex = 3;
            this.textBoxBuilderWidth.TextChanged += new System.EventHandler(this.textBoxBuilderWidth_TextChanged);
            this.textBoxBuilderWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDigitCheck);
            // 
            // textBoxBuilderHeight
            // 
            this.textBoxBuilderHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBuilderHeight.Location = new System.Drawing.Point(66, 45);
            this.textBoxBuilderHeight.Name = "textBoxBuilderHeight";
            this.textBoxBuilderHeight.Size = new System.Drawing.Size(113, 20);
            this.textBoxBuilderHeight.TabIndex = 4;
            this.textBoxBuilderHeight.TextChanged += new System.EventHandler(this.textBoxBuilderHeight_TextChanged);
            this.textBoxBuilderHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDigitCheck);
            // 
            // groupBoxTamanhoFinal
            // 
            this.groupBoxTamanhoFinal.Controls.Add(this.label1);
            this.groupBoxTamanhoFinal.Controls.Add(this.label2);
            this.groupBoxTamanhoFinal.Controls.Add(this.textBoxFinalWidth);
            this.groupBoxTamanhoFinal.Controls.Add(this.textBoxFinalHeight);
            this.groupBoxTamanhoFinal.Location = new System.Drawing.Point(6, 102);
            this.groupBoxTamanhoFinal.Name = "groupBoxTamanhoFinal";
            this.groupBoxTamanhoFinal.Size = new System.Drawing.Size(185, 77);
            this.groupBoxTamanhoFinal.TabIndex = 10;
            this.groupBoxTamanhoFinal.TabStop = false;
            this.groupBoxTamanhoFinal.Text = "Tamanho";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Altura";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Largura";
            // 
            // textBoxFinalWidth
            // 
            this.textBoxFinalWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFinalWidth.Location = new System.Drawing.Point(66, 19);
            this.textBoxFinalWidth.Name = "textBoxFinalWidth";
            this.textBoxFinalWidth.Size = new System.Drawing.Size(113, 20);
            this.textBoxFinalWidth.TabIndex = 3;
            this.textBoxFinalWidth.TextChanged += new System.EventHandler(this.textBoxFinalWidth_TextChanged);
            this.textBoxFinalWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDigitCheck);
            // 
            // textBoxFinalHeight
            // 
            this.textBoxFinalHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFinalHeight.Location = new System.Drawing.Point(66, 45);
            this.textBoxFinalHeight.Name = "textBoxFinalHeight";
            this.textBoxFinalHeight.Size = new System.Drawing.Size(113, 20);
            this.textBoxFinalHeight.TabIndex = 4;
            this.textBoxFinalHeight.TextChanged += new System.EventHandler(this.textBoxFinalHeight_TextChanged);
            this.textBoxFinalHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDigitCheck);
            // 
            // groupBoxPosFinal
            // 
            this.groupBoxPosFinal.Controls.Add(this.label3);
            this.groupBoxPosFinal.Controls.Add(this.label4);
            this.groupBoxPosFinal.Controls.Add(this.textBoxFinalX);
            this.groupBoxPosFinal.Controls.Add(this.textBoxFinalY);
            this.groupBoxPosFinal.Location = new System.Drawing.Point(6, 19);
            this.groupBoxPosFinal.Name = "groupBoxPosFinal";
            this.groupBoxPosFinal.Size = new System.Drawing.Size(185, 77);
            this.groupBoxPosFinal.TabIndex = 9;
            this.groupBoxPosFinal.TabStop = false;
            this.groupBoxPosFinal.Text = "Posição";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Vertical";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Horizontal";
            // 
            // textBoxFinalX
            // 
            this.textBoxFinalX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFinalX.Location = new System.Drawing.Point(66, 19);
            this.textBoxFinalX.Name = "textBoxFinalX";
            this.textBoxFinalX.Size = new System.Drawing.Size(113, 20);
            this.textBoxFinalX.TabIndex = 3;
            this.textBoxFinalX.TextChanged += new System.EventHandler(this.textBoxFinalX_TextChanged);
            this.textBoxFinalX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDigitCheck);
            // 
            // textBoxFinalY
            // 
            this.textBoxFinalY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFinalY.Location = new System.Drawing.Point(66, 45);
            this.textBoxFinalY.Name = "textBoxFinalY";
            this.textBoxFinalY.Size = new System.Drawing.Size(113, 20);
            this.textBoxFinalY.TabIndex = 4;
            this.textBoxFinalY.TextChanged += new System.EventHandler(this.textBoxFinalY_TextChanged);
            this.textBoxFinalY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDigitCheck);
            // 
            // groupBoxBuilder
            // 
            this.groupBoxBuilder.Controls.Add(this.groupBoxPosBuilder);
            this.groupBoxBuilder.Controls.Add(this.groupBoxTamanhoBuilder);
            this.groupBoxBuilder.Location = new System.Drawing.Point(12, 12);
            this.groupBoxBuilder.Name = "groupBoxBuilder";
            this.groupBoxBuilder.Size = new System.Drawing.Size(197, 187);
            this.groupBoxBuilder.TabIndex = 11;
            this.groupBoxBuilder.TabStop = false;
            this.groupBoxBuilder.Text = "Builder";
            // 
            // groupBoxPlayer
            // 
            this.groupBoxPlayer.Controls.Add(this.groupBoxPosFinal);
            this.groupBoxPlayer.Controls.Add(this.groupBoxTamanhoFinal);
            this.groupBoxPlayer.Location = new System.Drawing.Point(215, 12);
            this.groupBoxPlayer.Name = "groupBoxPlayer";
            this.groupBoxPlayer.Size = new System.Drawing.Size(197, 187);
            this.groupBoxPlayer.TabIndex = 12;
            this.groupBoxPlayer.TabStop = false;
            this.groupBoxPlayer.Text = "Player";
            this.groupBoxPlayer.Visible = false;
            // 
            // ManualComponentBounds
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancelar;
            this.ClientSize = new System.Drawing.Size(258, 243);
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxPlayer);
            this.Controls.Add(this.groupBoxBuilder);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ManualComponentBounds";
            this.Text = "ManualComponentPosition";
            this.groupBoxPosBuilder.ResumeLayout(false);
            this.groupBoxPosBuilder.PerformLayout();
            this.groupBoxTamanhoBuilder.ResumeLayout(false);
            this.groupBoxTamanhoBuilder.PerformLayout();
            this.groupBoxTamanhoFinal.ResumeLayout(false);
            this.groupBoxTamanhoFinal.PerformLayout();
            this.groupBoxPosFinal.ResumeLayout(false);
            this.groupBoxPosFinal.PerformLayout();
            this.groupBoxBuilder.ResumeLayout(false);
            this.groupBoxPlayer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancelar;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.TextBox textBoxBuilderX;
        private System.Windows.Forms.TextBox textBoxBuilderY;
        private System.Windows.Forms.GroupBox groupBoxPosBuilder;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.GroupBox groupBoxTamanhoBuilder;
        private System.Windows.Forms.Label labelAltura;
        private System.Windows.Forms.Label labelLargura;
        private System.Windows.Forms.TextBox textBoxBuilderWidth;
        private System.Windows.Forms.TextBox textBoxBuilderHeight;
        private System.Windows.Forms.GroupBox groupBoxTamanhoFinal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxFinalWidth;
        private System.Windows.Forms.TextBox textBoxFinalHeight;
        private System.Windows.Forms.GroupBox groupBoxPosFinal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxFinalX;
        private System.Windows.Forms.TextBox textBoxFinalY;
        private System.Windows.Forms.GroupBox groupBoxBuilder;
        private System.Windows.Forms.GroupBox groupBoxPlayer;
    }
}