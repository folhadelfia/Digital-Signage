using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assemblies.Configurations;
using Assemblies.Options;
using Assemblies.Toolkit;

namespace Assemblies.Components
{
    public class MarkeeComposer : ComposerComponent
    {
        public MarkeeComposer()
            : this(new MarkeeConfiguration())
        {
        }

        public MarkeeComposer(MarkeeConfiguration config)
        {
            base.BackColor = Color.Green;
            base.BorderStyle = config.Border ? System.Windows.Forms.BorderStyle.FixedSingle : System.Windows.Forms.BorderStyle.None;

            base.Configuration = config;

            base.optionsForm = new MarkeeOptions()
            {
                MarkeeTextList = (Configuration as MarkeeConfiguration).Text,
                MarkeeDirection = (Configuration as MarkeeConfiguration).Direction,
                MarkeeSpeed = (Configuration as MarkeeConfiguration).Speed,
                MarkeeFont = (Configuration as MarkeeConfiguration).Font,
                MarkeeBackColor = (Configuration as MarkeeConfiguration).BackColor,
                MarkeeTextColor = (Configuration as MarkeeConfiguration).TextColor
            };
        }

        public override string ToString()
        {
            return "Rodapé";
        }

        private MarkeeConfiguration config = new MarkeeConfiguration();

        public override void OpenOptionsWindow()
        {
            try
            {
                MarkeeOptions options = optionsForm as MarkeeOptions;

                if (options.ShowDialog() == DialogResult.OK)
                {
                    (this.Configuration as MarkeeConfiguration).Text = options.MarkeeTextList;
                    (this.Configuration as MarkeeConfiguration).Direction = options.MarkeeDirection;
                    (this.Configuration as MarkeeConfiguration).Speed = options.MarkeeSpeed;
                    (this.Configuration as MarkeeConfiguration).Font = options.MarkeeFont;
                    (this.Configuration as MarkeeConfiguration).TextColor = options.MarkeeTextColor;
                    (this.Configuration as MarkeeConfiguration).BackColor = options.MarkeeBackColor;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("OpenOptionsWindow Rodapé" + Environment.NewLine + ex.Message);
#endif
            }
        }
    }
}
