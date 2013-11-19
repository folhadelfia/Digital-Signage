using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assemblies.Components
{
    public class PriceListComposer : ComposerComponent
    {
        public PriceListComposer()
        {
            base.BackColor = Color.DarkGray;
        }

        public override string ToString()
        {
            return "Tabela de Preços";
        }

        #region Configuração
        
        //lista com produtos, preços, imagens e observações
        //dar a opção de dar destaque a um produto
        
        #endregion
    }
}
