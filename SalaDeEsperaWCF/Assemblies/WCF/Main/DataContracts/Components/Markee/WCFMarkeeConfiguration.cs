using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.DataContracts
{
    [DataContract]
    public class WCFMarkeeConfiguration : WCFItemConfiguration
    {
        private string[] textList;
        private WCFDirection direction;
        private int speed;
        private WCFFont font;
        private WCFColor footerBackColor, footerTextColor;

        [DataMember]
        public string[] TextList
        {
            get { return textList; }
            set { textList = value; }
        }
        [DataMember]
        public WCFDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        [DataMember]
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        [DataMember]
        public WCFFont Font
        {
            get { return font; }
            set { font = value; }
        }
        [DataMember]
        public WCFColor FooterBackColor
        {
            get { return footerBackColor; }
            set { footerBackColor = value; }
        }
        [DataMember]
        public WCFColor FooterTextColor
        {
            get { return footerTextColor; }
            set { footerTextColor = value; }
        }
        [DataMember]
        public bool TransparentBackground
        {
            get;
            set;
        }

        [DataMember]
        public bool Border { get; set; }
    }
}
