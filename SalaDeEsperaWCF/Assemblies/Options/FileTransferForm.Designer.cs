namespace Assemblies.Options
{
    partial class FileTransferForm
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
            this.progressBarCurrentFile = new System.Windows.Forms.ProgressBar();
            this.progressBarTotal = new System.Windows.Forms.ProgressBar();
            this.labelAllFilesProgress = new System.Windows.Forms.Label();
            this.labelTotalSizeProgress = new System.Windows.Forms.Label();
            this.labelCurrentFilename = new System.Windows.Forms.Label();
            this.labelFileSizeProgress = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBarCurrentFile
            // 
            this.progressBarCurrentFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarCurrentFile.Location = new System.Drawing.Point(15, 47);
            this.progressBarCurrentFile.Margin = new System.Windows.Forms.Padding(3, 3, 3, 17);
            this.progressBarCurrentFile.Name = "progressBarCurrentFile";
            this.progressBarCurrentFile.Size = new System.Drawing.Size(390, 22);
            this.progressBarCurrentFile.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarCurrentFile.TabIndex = 0;
            // 
            // progressBarTotal
            // 
            this.progressBarTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarTotal.Location = new System.Drawing.Point(15, 120);
            this.progressBarTotal.Name = "progressBarTotal";
            this.progressBarTotal.Size = new System.Drawing.Size(390, 22);
            this.progressBarTotal.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarTotal.TabIndex = 1;
            // 
            // labelAllFilesProgress
            // 
            this.labelAllFilesProgress.AutoSize = true;
            this.labelAllFilesProgress.Location = new System.Drawing.Point(12, 96);
            this.labelAllFilesProgress.Margin = new System.Windows.Forms.Padding(3, 10, 3, 8);
            this.labelAllFilesProgress.Name = "labelAllFilesProgress";
            this.labelAllFilesProgress.Size = new System.Drawing.Size(131, 13);
            this.labelAllFilesProgress.TabIndex = 2;
            this.labelAllFilesProgress.Text = "Total Progress: # of # files";
            // 
            // labelTotalSizeProgress
            // 
            this.labelTotalSizeProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTotalSizeProgress.Location = new System.Drawing.Point(207, 96);
            this.labelTotalSizeProgress.Margin = new System.Windows.Forms.Padding(3, 10, 3, 8);
            this.labelTotalSizeProgress.Name = "labelTotalSizeProgress";
            this.labelTotalSizeProgress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelTotalSizeProgress.Size = new System.Drawing.Size(198, 13);
            this.labelTotalSizeProgress.TabIndex = 3;
            this.labelTotalSizeProgress.Text = "###.## [unit] / ###.## [unit] ([value]%)";
            this.labelTotalSizeProgress.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelCurrentFilename
            // 
            this.labelCurrentFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCurrentFilename.Location = new System.Drawing.Point(12, 15);
            this.labelCurrentFilename.Margin = new System.Windows.Forms.Padding(3, 10, 3, 8);
            this.labelCurrentFilename.Name = "labelCurrentFilename";
            this.labelCurrentFilename.Size = new System.Drawing.Size(199, 30);
            this.labelCurrentFilename.TabIndex = 4;
            this.labelCurrentFilename.Text = "Current File: [filename]";
            this.labelCurrentFilename.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFileSizeProgress
            // 
            this.labelFileSizeProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileSizeProgress.Location = new System.Drawing.Point(209, 24);
            this.labelFileSizeProgress.Margin = new System.Windows.Forms.Padding(3, 10, 3, 8);
            this.labelFileSizeProgress.Name = "labelFileSizeProgress";
            this.labelFileSizeProgress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelFileSizeProgress.Size = new System.Drawing.Size(196, 21);
            this.labelFileSizeProgress.TabIndex = 5;
            this.labelFileSizeProgress.Text = "###.## [unit] / ###.## [unit] ([value]%)";
            this.labelFileSizeProgress.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(330, 148);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "Fechar";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // FileTransferForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 178);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.labelFileSizeProgress);
            this.Controls.Add(this.labelCurrentFilename);
            this.Controls.Add(this.labelTotalSizeProgress);
            this.Controls.Add(this.labelAllFilesProgress);
            this.Controls.Add(this.progressBarTotal);
            this.Controls.Add(this.progressBarCurrentFile);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 216);
            this.MinimumSize = new System.Drawing.Size(436, 216);
            this.Name = "FileTransferForm";
            this.Text = "FileTransferForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileTransferForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarCurrentFile;
        private System.Windows.Forms.ProgressBar progressBarTotal;
        private System.Windows.Forms.Label labelAllFilesProgress;
        private System.Windows.Forms.Label labelTotalSizeProgress;
        private System.Windows.Forms.Label labelCurrentFilename;
        private System.Windows.Forms.Label labelFileSizeProgress;
        private System.Windows.Forms.Button buttonClose;
    }
}