using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.DataContracts
{
    #region Markee
    [DataContract]
    public enum WCFDirection
    {
        [EnumMemberAttribute]
        None = 0,
        [EnumMemberAttribute]
        Up = 1,
        [EnumMemberAttribute]
        Down = 2,
        [EnumMemberAttribute]
        Left = 3,
        [EnumMemberAttribute]
        Right = 4
    }
    #endregion
}
