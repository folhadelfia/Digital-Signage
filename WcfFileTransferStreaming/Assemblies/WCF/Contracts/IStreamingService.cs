using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;

namespace Assemblies.WCF
{
    [ServiceContract(Name="FileStreamingService")]
    public interface IStreamingService
    {
        //[OperationContract]
        //Stream GetStream(string data);
        [OperationContract]
        bool UploadStream(Stream stream);
        [OperationContract]
        bool UploadBytes(byte[] bytes);
        [OperationContract]
        bool UploadFile(StreamedFile file);
        //[OperationContract]
        //Stream EchoStream(Stream stream);
        //[OperationContract]
        //Stream GetReversedStream();
    }
}
