using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assemblies.XMLSerialization.Drawing;

namespace Assemblies.XMLSerialization.Components
{
    public class XMLMarkeeConfiguration : XMLItemConfiguration
    {
        private string[] textList;
        private XMLDirection direction;
        private int speed;
        private XMLFont font;
        private XMLColor footerBackColor, footerTextColor;

        public string[] TextList
        {
            get { return textList; }
            set { textList = value; }
        }
        
        public XMLDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        
        public XMLFont Font
        {
            get { return font; }
            set { font = value; }
        }
        
        public XMLColor FooterBackColor
        {
            get { return footerBackColor; }
            set { footerBackColor = value; }
        }
        
        public XMLColor FooterTextColor
        {
            get { return footerTextColor; }
            set { footerTextColor = value; }
        }
        
        public bool TransparentBackground
        {
            get;
            set;
        }

        public bool Border { get; set; }
    }
}
