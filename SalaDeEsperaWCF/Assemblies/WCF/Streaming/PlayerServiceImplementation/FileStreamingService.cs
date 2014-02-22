using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Text;
using System.Xml;
using Assemblies.ClientProxies;
using Assemblies.DataContracts;
using Assemblies.PlayerServiceContracts;

namespace Assemblies.PlayerServiceImplementation
{
    public class FileStreamingService : IFileStreamingService
    {
        #region Events

        public static event EventHandler<FileStreamReceivedEventArgs> FileStreamReceived;
        public static event EventHandler<FileBytesReceivedEventArgs> FileBytesReceived;
        public static event EventHandler<FileReceivedEventArgs> FileReceived;
        public static event EventHandler<FileStreamProgressReceivedEventArgs> FileStreamProgressReceived;
        public static event EventHandler<string> FileStreamProgressCancelled;

        #endregion

        #region IFileStreamingService

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

        public void UploadStreamWithProgress(RemoteFileInfo request)
        {
            if (FileStreamProgressReceived != null) FileStreamProgressReceived(this, new FileStreamProgressReceivedEventArgs(request));
        }

        //Adicionar um método para receber uma informaçao de cancelamento da transferencia, com o caminho do ficheiro para ser apagado

        #endregion

        #region Create host/client

        public static ServiceHost CreateHost(string ip, string port)
        {
            Uri baseAddress = new Uri(string.Format("net.tcp://{0}:{1}/FileStreamingService", ip, port));

            return CreateHost(baseAddress);

        }
        public static ServiceHost CreateHost(Uri baseAddress)
        {
            ServiceHost serviceHostTemp = new ServiceHost(typeof(FileStreamingService), baseAddress);

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;

            binding.MaxReceivedMessageSize = 2147483647; // Valor máximo
            binding.MaxBufferSize = 8 * 1024 * 1024; // 8MB

            binding.ReaderQuotas.MaxArrayLength = 2147483647; //Valor máximo

            binding.TransferMode = TransferMode.StreamedRequest;
            binding.ReceiveTimeout = new TimeSpan(12, 0, 0);
            binding.SendTimeout = new TimeSpan(12, 0, 0);

            serviceHostTemp.AddServiceEndpoint(typeof(IFileStreamingService), binding, baseAddress);

            return serviceHostTemp;
        }

        public static FileStreamingServiceProxy CreateClient(string ip, string port)
        {
            Uri baseAddress = new Uri(string.Format("net.tcp://{0}:{1}/StreamingService", ip, port));

            EndpointAddress endpoint = new EndpointAddress(baseAddress);

            return CreateClient(endpoint);
        }
        public static FileStreamingServiceProxy CreateClient(EndpointAddress endpoint)
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None)
            {
                ReceiveTimeout = new TimeSpan(12, 0, 0),
                SendTimeout = new TimeSpan(12, 0, 0),
                TransferMode = TransferMode.StreamedRequest,
                MaxBufferSize = 8 * 1024 * 1024,
                MaxReceivedMessageSize = 2147483647 // Valor máximo
            };

            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;

            FileStreamingServiceProxy proxy = new FileStreamingServiceProxy(binding, endpoint);

            return proxy;
        }

        #endregion

        #region Helpers

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

        #endregion
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
    public class FileStreamProgressReceivedEventArgs : EventArgs
    {
        public FileStreamProgressReceivedEventArgs(RemoteFileInfo request)
        {
            this.Request = request;
        }

        public RemoteFileInfo Request { get; private set; }
    }
}
