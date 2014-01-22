using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assemblies.Toolkit;
using Assemblies.WCF;

namespace Client
{
    public partial class ClientWindow : Form
    {
        StreamingServiceProxy proxy;

        public ClientWindow()
        {
            InitializeComponent();
        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {
            proxy = StreamingService.CreateClient(Networking.PrivateIPAddress.ToString(), "10002");

            proxy.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (FileStream fileStream = File.OpenRead(ofd.FileName))
                {
                    var file = fileStream.ToStreamedFile(ofd.SafeFileName);

                    //MemoryStream memStream = new MemoryStream();
                    //memStream.SetLength(fileStream.Length);
                    //fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                    //StreamedFile file = new StreamedFile();

                    //file.Bytes = StreamingService.ToByteArray<MemoryStream>(memStream);
                    //file.FileName = ofd.SafeFileName;

                    proxy.UploadFile(file);
                }
            }
        }
    }
}
