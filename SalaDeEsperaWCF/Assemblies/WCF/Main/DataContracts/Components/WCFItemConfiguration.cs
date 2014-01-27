using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.DataContracts
{
    
    [DataContract]
    public class WCFItemConfiguration
    {
        protected WCFItemConfiguration() //Protected para só se poderem instanciar as classe derivadas
        {
        }

        #region Tamanhos e Localizações
        /// <summary>
        /// Item's location
        /// </summary>
        [DataMember]
        public WCFPoint Location { get; set; }

        /// <summary>
        /// Item's size
        /// </summary>
        [DataMember]
        public WCFSize Size { get; set; }

        /// <summary>
        /// Builder's panel resolution
        /// </summary>
        [DataMember]
        public WCFSize Resolution { get; set; }

        /// <summary>
        /// Screen resolution
        /// </summary>
        [DataMember]
        public WCFSize FinalResolution { get; set; }

        #endregion
    }
}
