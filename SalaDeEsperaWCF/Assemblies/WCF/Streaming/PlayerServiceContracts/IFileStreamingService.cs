using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
using Assemblies.DataContracts;

namespace Assemblies.PlayerServiceContracts
{
    [ServiceContract(Name="FileStreamingService")]
    public interface IFileStreamingService
    {
        [OperationContract]
        bool UploadStream(Stream stream);
        [OperationContract]
        bool UploadBytes(byte[] bytes);
        [OperationContract]
        bool UploadFile(StreamedFile file);
        [OperationContract]
        void UploadStreamWithProgress(RemoteFileInfo request);
    }
}
