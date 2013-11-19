using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Components
{
    public class WaitListComposer : ComposerComponent
    {
        public WaitListComposer()
        {
            base.BackColor = Color.Teal;
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
