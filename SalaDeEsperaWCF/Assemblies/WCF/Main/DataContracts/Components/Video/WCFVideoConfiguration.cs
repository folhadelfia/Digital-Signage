using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.DataContracts
{
    [DataContract]
    public class WCFVideoConfiguration : WCFItemConfiguration
    {
        private int id;
        private string[] playlist;
        WCFAspect aspect;

        [DataMember]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string[] Playlist
        {
            get { return playlist; }
            set { playlist = value; }
        }

        [DataMember]
        public WCFAspect Aspect
        {
            get { return aspect; }
            set { aspect = value; }
        }
    }
}
