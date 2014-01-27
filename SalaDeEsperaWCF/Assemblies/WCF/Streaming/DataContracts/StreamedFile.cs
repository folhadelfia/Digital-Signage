using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Assemblies.PlayerServiceImplementation;

namespace Assemblies.DataContracts
{
    public class StreamedFile
    {
        [DataMember]
        public byte[] Bytes { get; set; }
        [DataMember]
        public string FileName { get; set; }

        public void SaveToDisk(string path)
        {
            MemoryStream ms = FileStreamingService.ToObject<MemoryStream>(this.Bytes);

            Stream fileStream = File.Create(path.TrimEnd('\\') + "\\" + FileName);

            this.CopyStream(ms, fileStream);

            fileStream.Close();
        }

        private void CopyStream(Stream input, Stream output)
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
