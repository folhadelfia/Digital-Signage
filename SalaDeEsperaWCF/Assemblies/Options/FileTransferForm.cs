using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assemblies.ClientModel;
using Assemblies.Toolkit;

namespace Assemblies.Options
{
    public partial class FileTransferForm : Form
    {

        public event EventHandler FileTransferred;

        Connection connection;

        int totalFilesToUpload = 0,
            uploadedFiles = 0;

        decimal bytesUploaded = 0,
               totalBytes = 0;

        private FileTransferForm()
        {
            InitializeComponent();
        }

        public FileTransferForm(Connection con)
            : this()
        {
            connection = con;
        }

        #region File transfer

        BackgroundWorker fileTransferWorker;

        //receber a lista de ficheiros a ser transferida
        /// <summary>
        /// Uploads the file list to the Connection's target
        /// </summary>
        /// <param name="files"></param>
        public void UploadFiles(IEnumerable<string> files)
        {
            foreach (var f in files)
            {
                FileInfo file = new FileInfo(f);

                totalBytes += file.Length;
            }

            if (files.Count() < 1) return;
            if (!this.Visible) this.Show();

            totalFilesToUpload = files.Count();

            if (totalFilesToUpload <= 0) return;
            if (connection != null && connection.State != ClientModel.ConnectionState.Open) return;

            if (connection is WCFConnection) (connection as WCFConnection).ProgressChanged += FileTransferForm_ProgressChanged;

            fileTransferWorker = new BackgroundWorker();

            fileTransferWorker.DoWork += fileTransferWorker_DoWork;
            fileTransferWorker.RunWorkerCompleted += fileTransferWorker_RunWorkerCompleted;

            fileTransferWorker.RunWorkerAsync(files);
        }

        void fileTransferWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if(!(e.Argument is IEnumerable<string>)) return;

            IEnumerable<string> files = e.Argument as IEnumerable<string>;

            foreach (var file in files)
            {
                this.BeginInvoke((MethodInvoker)(()=>
                {
                    this.labelCurrentFilename.Text = string.Format("Current File: {0}", file);
                    this.labelAllFilesProgress.Text = string.Format("Total Progress: {0} of {1} file{2}", uploadedFiles, totalFilesToUpload, totalFilesToUpload != 1 ? "s" : "");

                    this.Text = string.Format("[{1}/{2}] A transferir: {0} [{3}%]", file, uploadedFiles, totalFilesToUpload, progressBarCurrentFile.Value);
                }));
                connection.UploadVideoFile(file);

                uploadedFiles++;
            }

            UpdateProgressBarValue(100, Convert.ToInt32(Math.Round(Convert.ToSingle(uploadedFiles) / Convert.ToSingle(totalFilesToUpload) * 100f, 0)));

            this.BeginInvoke((MethodInvoker)(()=>
            {
                this.labelCurrentFilename.Text = "Upload complete";
                this.labelAllFilesProgress.Text = string.Format("Total Progress: {0} of {1} file{2}", uploadedFiles, totalFilesToUpload, totalFilesToUpload != 1 ? "s" : "");
                this.Text = "Upload complete";
            }));
        }
        void fileTransferWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            fileTransferWorker.Dispose();
            if (this.FileTransferred != null) this.FileTransferred(this, new EventArgs());
        }

        #endregion

        decimal lastUploadedBytes = 0;
        void FileTransferForm_ProgressChanged(object sender, WCFConnection.FileTransferProgressEventArgs e)
        {
            ;
            bytesUploaded += e.BytesTransferred - lastUploadedBytes;
            if (e.BytesTransferred == lastUploadedBytes) 
                lastUploadedBytes = 0;
            else
                lastUploadedBytes = e.BytesTransferred;

            int progCurrent =Convert.ToInt32(e.Progress),
                progTotal = Convert.ToInt32(Math.Round(Convert.ToSingle(bytesUploaded) / Convert.ToSingle(totalBytes) * 100f, 0));

            UpdateProgressBarValue(progCurrent, progTotal);
            UpdateProgressLabels(e.BytesTransferred, e.TotalBytes, bytesUploaded, totalBytes);
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

        private void UpdateProgressLabels(decimal currentTransferred, decimal currentTotal, decimal totalTransferred, decimal total)
        {
            if (this.InvokeRequired) this.BeginInvoke((MethodInvoker)(() => UpdateProgressLabels(currentTransferred, currentTotal, totalTransferred, total)));
            else
            {
                string currentTransferredString = MyToolkit.Sizes.ByteToBestFitUnit(Convert.ToDouble(currentTransferred)),
                       currentTotalString = MyToolkit.Sizes.ByteToBestFitUnit(Convert.ToDouble(currentTotal)),
                       totalTransferredString = MyToolkit.Sizes.ByteToBestFitUnit(Convert.ToDouble(totalTransferred)),
                       totalString = MyToolkit.Sizes.ByteToBestFitUnit(Convert.ToDouble(total));

                labelFileSizeProgress.Text = string.Format("{0} / {1} ({2}%)", currentTransferredString, currentTotalString, Math.Round(currentTransferred / currentTotal * 100, 0).ToString());

                labelTotalSizeProgress.Text = string.Format("{0} / {1} ({2}%)", totalTransferredString, totalString, Math.Round(totalTransferred / total * 100, 0).ToString());


            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FileTransferForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connection is WCFConnection) (connection as WCFConnection).ProgressChanged -= FileTransferForm_ProgressChanged;
        }
    }
}
