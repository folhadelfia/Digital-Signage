using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Assemblies.DataContracts;
using Assemblies.PlayerServiceContracts;

namespace Assemblies.ClientProxies
{
    public class FileStreamingServiceProxy : ClientBase<IFileStreamingService>, IFileStreamingService
    {
        public FileStreamingServiceProxy(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress endpoint)
            : base(binding, endpoint)
        {
        }

        public bool UploadStream(System.IO.Stream stream)
        {
            return Channel.UploadStream(stream);
        }


        public bool UploadBytes(byte[] bytes)
        {
            return Channel.UploadBytes(bytes);
        }


        public bool UploadFile(StreamedFile file)
        {
            return Channel.UploadFile(file);
        }


        public void UploadStreamWithProgress(RemoteFileInfo request)
        {
            Channel.UploadStreamWithProgress(request);
        }
    }
}
