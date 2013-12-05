using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assemblies.Components
{
    public class ImageComposer : ComposerComponent
    {
        public ImageComposer()
        {
            base.BackColor = Color.Blue;

            ToolTip tt = new ToolTip();

            tt.SetToolTip(this, this.ToString());
        }

        #region Configuração

        private Image imagem;

        public Image Imagem
        {
            get { return imagem; }
            set { imagem = value; }
        }

        #endregion

        public override string ToString()
        {
            return "Imagem";
        }
    }
}
