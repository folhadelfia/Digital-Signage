using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using Assemblies.Toolkit;
using Assemblies.WCF;

namespace Server
{
    public partial class ServerWindow : Form
    {
        ServiceHost host = StreamingService.CreateHost(Networking.PrivateIPAddress.ToString(), "10002");

        public ServerWindow()
        {
            InitializeComponent();

            StreamingService.FileStreamReceived += StreamingService_FileReceived;
            StreamingService.FileBytesReceived += StreamingService_FileBytesReceived;
            StreamingService.FileReceived += StreamingService_FileReceived;
        }

        void StreamingService_FileReceived(object sender, FileReceivedEventArgs e)
        {
            e.File.SaveToDisk(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
        }

        void StreamingService_FileBytesReceived(object sender, FileBytesReceivedEventArgs e)
        {
            MemoryStream ms = StreamingService.ToObject<MemoryStream>(e.Bytes);

            Stream fileStream = File.Create("c:\\musicaBytes.mp3");

            ms.CopyStream(fileStream);

            fileStream.Close();
        }

        void StreamingService_FileReceived(object sender, FileStreamReceivedEventArgs e)
        {
            Stream fileStream = File.Create("c:\\musica.mp3");

            e.FileStream.CopyTo(fileStream, 8 * 1024);

            fileStream.Close();
        }

        private void ServerWindow_Load(object sender, EventArgs e)
        {
            host.Open();
        }
    }

    public static class ExtensionMethods
    {
        public static void CopyStream(this Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}
