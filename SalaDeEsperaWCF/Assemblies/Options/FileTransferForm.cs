using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assemblies.ClientModel;

namespace Assemblies.Options
{
    public partial class FileTransferForm : Form
    {
        public class FileTransferedEventArgs : EventArgs
        {
            public FileTransferedEventArgs(string fileName)
            {
                this.FileName = FileName;
            }

            public string FileName { get; internal set; }
        }

        public event EventHandler<FileTransferedEventArgs> FileTransfered;

        Connection connection;

        int totalFilesToUpload = 0,
            uploadedFiles = 0;

        private FileTransferForm()
        {
            InitializeComponent();
        }

        public FileTransferForm(Connection con)
            : this()
        {
            connection = con;
        }

        //receber a lista de ficheiros a ser transferida
        /// <summary>
        /// Uploads the file list to the Connection's target
        /// </summary>
        /// <param name="files"></param>
        public void UploadFiles(IEnumerable<string> files)
        {
            if (!this.Visible) this.Show();

            totalFilesToUpload = files.Count();

            if (totalFilesToUpload <= 0) return;
            if (connection != null && connection.State != ClientModel.ConnectionState.Open) return;

            if (connection is WCFConnection) (connection as WCFConnection).ProgressChanged += FileTransferForm_ProgressChanged;

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += (sender, e) =>
                {
                    foreach (var file in files)
                    {
                        this.BeginInvoke((MethodInvoker)(()=>
                        {
                            this.labelCurrentFilename.Text = string.Format("Current File: {0}", file);
                            this.labelAllFilesProgress.Text = string.Format("Total Progress: {0} of {1} file{2}", uploadedFiles, totalFilesToUpload, totalFilesToUpload != 1 ? "s" : "");
                        }));
                        connection.SendVideoFile(file);

                        uploadedFiles++;
                    }

                    UpdateProgressBarValue(100, Convert.ToInt32(Math.Round(Convert.ToSingle(uploadedFiles) / Convert.ToSingle(totalFilesToUpload) * 100f, 0)));

                    this.BeginInvoke((MethodInvoker)(()=>
                    {
                        this.labelCurrentFilename.Text = "Upload complete";
                        this.labelAllFilesProgress.Text = string.Format("Total Progress: {0} of {1} file{2}", uploadedFiles, totalFilesToUpload, totalFilesToUpload != 1 ? "s" : "");
                     }));
                };

            worker.RunWorkerAsync();
        }

        void FileTransferForm_ProgressChanged(object sender, WCFConnection.FileTransferProgressEventArgs e)
        {
            int progCurrent =Convert.ToInt32(e.Progress),
                progTotal = Convert.ToInt32(Math.Round(Convert.ToSingle(uploadedFiles) / Convert.ToSingle(totalFilesToUpload) * 100f, 0));

            UpdateProgressBarValue(progCurrent, progTotal);
        }

        private void UpdateProgressBarValue(int progressCurrent, int progressTotal)
        {
            if (this.InvokeRequired) this.BeginInvoke((MethodInvoker)(() => UpdateProgressBarValue(progressCurrent, progressTotal)));
            else
            {
                progressBarCurrentFile.Value = progressCurrent;
                progressBarTotal.Value = progressTotal;
            }
        }
    }
}
