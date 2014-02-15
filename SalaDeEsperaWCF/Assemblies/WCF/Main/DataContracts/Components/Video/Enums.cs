using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.DataContracts
{

    #region Video

    [DataContract]
    public enum WCFAspect
    {
        [EnumMemberAttribute]
        Center = 0,
        [EnumMemberAttribute]
        Fill = 1,
        [EnumMemberAttribute]
        Fit = 2,
        [EnumMemberAttribute]
        Stretch = 3
    }

    #endregion
}
