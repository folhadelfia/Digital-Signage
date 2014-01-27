using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assemblies.ClientModel;
using Assemblies.Configurations;
using Assemblies.Options;

namespace Assemblies.Components
{
    public class TVComposer : ComposerComponent
    {
        public TVComposer()
            : this(new TVConfiguration())
        {
        }

        public TVComposer(TVConfiguration config)
        {
            this.SuspendLayout();

            base.BackColor = Color.Navy;

            base.Configuration = config;

            base.optionsForm = new TVOptions();

            ToolTip tt = new ToolTip();

            tt.SetToolTip(this, this.ToString());

            Designation = "TV";
            DesignationLabel.ForeColor = SystemColors.Window;

            this.ResumeLayout();
            this.PerformLayout();
        }

        #region Configuração
        //int frequency = 754000, 
        //    onid = -1, 
        //    tsid = -1, 
        //    sid = 1103;

        //public int Frequency
        //{
        //    get { return frequency; }
        //    set { frequency = value; }
        //}
        //public int ONID
        //{
        //    get { return onid; }
        //    set { onid = value; }
        //}
        //public int TSID
        //{
        //    get { return tsid; }
        //    set { tsid = value; }
        //}
        //public int SID
        //{
        //    get { return sid; }
        //    set { sid = value; }
        //}
        #endregion

        public void SetOptionsWindowConnection(Connection con)
        {
            (optionsForm as TVOptions).AssignConnection(con);
        }

        public override void OpenOptionsWindow()
        {
            try
            {
                TVOptions options = base.optionsForm as TVOptions;

                if (options.ShowDialog() == DialogResult.OK)
                {
                    options.ApplyChangesToComponent(this);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("OpenOptionsWindow TV" + Environment.NewLine + ex.Message);
#endif
            }
        }

        public override string ToString()
        {
            return "TV";
        }
    }
}
