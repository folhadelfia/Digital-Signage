using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assemblies.Components
{
    public class WaitListComposer : ComposerComponent
    {
        public WaitListComposer()
        {
            base.BackColor = Color.Teal;

            ToolTip tt = new ToolTip();

            tt.SetToolTip(this, this.ToString());
        }

        #region Configuração

        //Configuração da lista de espera

        #endregion

        public override string ToString()
        {
            return "Lista de Espera";
        }
    }
}
