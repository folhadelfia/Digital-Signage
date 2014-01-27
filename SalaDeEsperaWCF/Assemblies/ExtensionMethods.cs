using System;
using System.Collections.Generic;
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
    }
}
