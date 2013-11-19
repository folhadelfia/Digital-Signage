using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assemblies.Components
{
    public class WeatherComposer : ComposerComponent
    {
        public WeatherComposer()
        {
            base.BackColor = Color.Violet;
        }

        #region Configuração

        //Adicionar o servidor onde vai buscar a informacao, o local, entre outros

        #endregion

        public override string ToString()
        {
            return "Meteorologia";
        }
    }
}
