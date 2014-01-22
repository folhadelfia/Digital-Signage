using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace Assemblies.WCF
{
    public class StreamingService : IStreamingService
    {
        public static event FileStreamReceivedEventHandler FileStreamReceived;
        public static event FileBytesReceivedEventHandler FileBytesReceived;
        public static event FileReceivedEventHandler FileReceived;

        public bool UploadStream(Stream stream)
        {
            if (FileStreamReceived != null) FileStreamReceived(this, new FileStreamReceivedEventArgs(stream));

            return FileStreamReceived != null;
        }

        public bool UploadBytes(byte[] bytes)
        {
            if (FileBytesReceived != null) FileBytesReceived(this, new FileBytesReceivedEventArgs(bytes));

            return FileBytesReceived != null;
        }

        public bool UploadFile(StreamedFile file)
        {
            if (FileReceived != null) FileReceived(this, new FileReceivedEventArgs(file));

            return FileReceived != null;
        }

        public static ServiceHost CreateHost(string ip, string port)
        {
            Uri baseAddress = new Uri(string.Format("net.tcp://{0}:{1}/StreamingService", ip, port));

            ServiceHost serviceHostTemp = new ServiceHost(typeof(StreamingService), baseAddress);

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;

            binding.MaxReceivedMessageSize = 1 * 1024 * 1024 * 1024; // 1GB
            binding.MaxBufferSize = 8 * 1024 * 1024; // 8MB

            binding.ReaderQuotas.MaxArrayLength = 1 * 1024 * 1024 * 1024;

            binding.TransferMode = TransferMode.StreamedRequest;
            binding.ReceiveTimeout = new TimeSpan(0,10,0);
            binding.SendTimeout = new TimeSpan(0,10,0);

            serviceHostTemp.AddServiceEndpoint(typeof(IStreamingService),binding, baseAddress);

            return serviceHostTemp;

        }

        public static StreamingServiceProxy CreateClient(string ip, string port)
        {
            Uri baseAddress = new Uri(string.Format("net.tcp://{0}:{1}/StreamingService", ip, port));

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None)
            {
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
                TransferMode = TransferMode.StreamedRequest,
                MaxBufferSize = 8 * 1024 * 1024,
                MaxReceivedMessageSize = 1 * 1024 * 1024 * 1024
            };

            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;

            EndpointAddress endpoint = new EndpointAddress(baseAddress);

            StreamingServiceProxy proxy = new StreamingServiceProxy(binding, endpoint);

            return proxy;
        }

        public static byte[] ToByteArray<T>(T obj)
        {
            if (obj == null) return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        public static T ToObject<T>(byte[] array)
        {
            MemoryStream ms= new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            ms.Write(array, 0, array.Length);
            ms.Seek(0, SeekOrigin.Begin);

            T obj = (T)bf.Deserialize(ms);

            return obj;
        }
    }

    public class FileStreamReceivedEventArgs : EventArgs
    {
        public FileStreamReceivedEventArgs(Stream s)
        {
            this.FileStream = s;
        }

        public Stream FileStream { get; private set; }
    }
    public class FileBytesReceivedEventArgs : EventArgs
    {
        public FileBytesReceivedEventArgs(byte[] b)
        {
            this.Bytes = b;
        }

        public byte[] Bytes { get; private set; }
    }
    public class FileReceivedEventArgs : EventArgs
    {
        public FileReceivedEventArgs(StreamedFile sf)
        {
            this.File = sf;
        }

        public StreamedFile File { get; private set; }
    }

    public delegate void FileStreamReceivedEventHandler(object sender, FileStreamReceivedEventArgs e);
    public delegate void FileBytesReceivedEventHandler(object sender, FileBytesReceivedEventArgs e);
    public delegate void FileReceivedEventHandler(object sender, FileReceivedEventArgs e);

    public static class ExtensionMethods
    {
        public static StreamedFile ToStreamedFile(this FileStream f, string fileName)
        {
            MemoryStream memStream = new MemoryStream();
            memStream.SetLength(f.Length);
            f.Read(memStream.GetBuffer(), 0, (int)f.Length);

            StreamedFile file = new StreamedFile();

            file.Bytes = StreamingService.ToByteArray<MemoryStream>(memStream);
            file.FileName = fileName;

            return file;
        }
    }
}
