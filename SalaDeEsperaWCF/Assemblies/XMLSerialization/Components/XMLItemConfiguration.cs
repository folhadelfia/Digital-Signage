using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Assemblies.XMLSerialization.Drawing;

namespace Assemblies.XMLSerialization.Components
{
    [XmlInclude(typeof(XMLTVConfiguration))]
    [XmlInclude(typeof(XMLMarkeeConfiguration))]
    public class XMLItemConfiguration
    {
        protected XMLItemConfiguration() { }

        #region Tamanhos e Localizações
        /// <summary>
        /// Item's location
        /// </summary>
        public XMLPoint Location { get; set; }

        /// <summary>
        /// Item's size
        /// </summary>
        public XMLSize Size { get; set; }

        /// <summary>
        /// Builder's panel resolution
        /// </summary>
        public XMLSize Resolution { get; set; }

        /// <summary>
        /// Screen resolution
        /// </summary>
        public XMLSize FinalResolution { get; set; }

        #endregion
    }
}
