using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assemblies.Components
{
    public class VideoComposer : ComposerComponent
    {
        public VideoComposer()
        {
            base.BackColor = Color.Chocolate;
        }

        #region

        private string source;

        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        #endregion

        public override string ToString()
        {
            return "Video";
        }
    }
}
