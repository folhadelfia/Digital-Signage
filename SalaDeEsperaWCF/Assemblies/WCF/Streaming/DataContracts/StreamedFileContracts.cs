using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
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


    public class StreamWithProgress : Stream
    {
        private readonly System.IO.FileStream file;
        private readonly long length;
        private long bytesRead;

        public event EventHandler<ProgressEventArgs> ProgressChanged;

        public StreamWithProgress(System.IO.FileStream file)
        {
            this.file = file;
            length = file.Length;
            bytesRead = 0;
            if (ProgressChanged != null) ProgressChanged(this, new ProgressEventArgs() { Progress = 0 });
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = file.Read(buffer, offset, count);
            bytesRead += result;

            if (ProgressChanged != null) ProgressChanged(this, new ProgressEventArgs() { Progress = 100 * bytesRead / length });

            return result;
        }

        public class ProgressEventArgs : EventArgs
        {
            public decimal Progress { get; internal set; }
        }

        #region Overrides de Stream. Todos lançam NotImplementedException

        public override bool CanRead
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanSeek
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {
        public RemoteFileInfo()
        {
        }

        public RemoteFileInfo(string file)
        {
            this.LoadFile(file);
        }

        [MessageHeader(MustUnderstand = true)]
        public string FileName { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public long Length { get; set; }

        [MessageBodyMember(Order = 1)]
        public Stream FileByteStream { get; set; }

        public static RemoteFileInfo FromFile(string file)
        {
            RemoteFileInfo res = new RemoteFileInfo();

            res.LoadFile(file);

            return res;
        }

        public void LoadFile(string file)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(file);

                using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    this.FileName = fileInfo.Name;
                    this.Length = fileInfo.Length;
                    this.FileByteStream = stream;
                }
            }
            catch (Exception)
            {
            }
        }

        public void Dispose()
        {
            try
            {
                if (this.FileByteStream != null)
                {
                    this.FileByteStream.Close();
                    this.FileByteStream = null;
                }
            }
            catch (Exception)
            {
                //Não sei o que fazer, completar quando houver mais know how (aka quando for mais gud)
                throw;
            }
        }
    }
}
