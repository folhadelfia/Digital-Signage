using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Assemblies.WCF
{
    public class StreamingServiceProxy : ClientBase<IStreamingService>, IStreamingService
    {
        public StreamingServiceProxy(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress endpoint)
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
    }
}
