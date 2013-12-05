using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assemblies.Components
{
    public class DateTimeComposer : ComposerComponent
    {
        public DateTimeComposer()
        {
            base.BackColor = Color.Red;

            ToolTip tt = new ToolTip();

            tt.SetToolTip(this, this.ToString());
        }

        #region Formatos, fusos horários, tamanhos e tipos de letra, entre outros

        #endregion

        public override string ToString()
        {
            return "Data e Hora";
        }
    }
}
