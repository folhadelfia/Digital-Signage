using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assemblies.Configurations;

namespace Assemblies.Components
{
    public class VideoComposer : ComposerComponent
    {

        public VideoComposer()
        {
            base.BackColor = Color.MediumSlateBlue;

            ToolTip tt = new ToolTip();

            tt.SetToolTip(this, this.ToString());

            this.Designation = "Video";

            this.Configuration = new Configurations.VideoConfiguration();
        }

        public override string ToString()
        {
            return this.Designation;
        }
    }
}
