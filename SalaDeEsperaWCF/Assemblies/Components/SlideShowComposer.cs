using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assemblies.Components
{
    public class SlideShowComposer : ComposerComponent
    {
        public SlideShowComposer()
        {
            base.BackColor = Color.White;
        }

        #region Configuração

        //Imagens, caminho com imagens, velocidade, repetição

        #endregion

        public override string ToString()
        {
            return "Slide Show";
        }
    }
}
