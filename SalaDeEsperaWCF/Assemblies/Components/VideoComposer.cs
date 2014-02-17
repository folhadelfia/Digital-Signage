using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assemblies.Configurations;
using Assemblies.Options;

namespace Assemblies.Components
{
    public class VideoComposer : ComposerComponent, IUsesConnection
    {
        public VideoComposer()
        {
            base.BackColor = Color.MediumSlateBlue;

            ToolTip tt = new ToolTip();

            tt.SetToolTip(this, this.ToString());

            this.Designation = "Video";

            this.Configuration = new Configurations.VideoConfiguration();

            base.optionsForm = new VideoOptions();
        }

        public VideoComposer(VideoConfiguration videoConfiguration) : this()
        {
            this.Configuration = videoConfiguration;
        }

        public override string ToString()
        {
            return this.Designation;
        }

        public void SetConnection(ClientModel.Connection con)
        {
            (this.optionsForm as VideoOptions).AssignConnection(con);
        }

        public override void OpenOptionsWindow()
        {
            try
            {
                VideoOptions options = base.optionsForm as VideoOptions;

                if (options.ShowDialog() == DialogResult.OK)
                {
                    options.ApplyChangesToComponent(this);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("OpenOptionsWindow Video" + Environment.NewLine + ex.Message);
#endif
            }
        }
    }
}
