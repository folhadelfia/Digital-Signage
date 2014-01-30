using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.ExtensionMethods
{
    public static class ExtensionMethods
    {

        #region Listas

        public static void DeepCopyTo<T>(this List<T> source, List<T> target)
        {
            target.Clear();

            foreach (var item in source)
            {
                target.Add(item);
            }
        }

        #endregion

        #region Streams

        public static void CopyStream(this Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        #endregion
    }
}
