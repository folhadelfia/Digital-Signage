using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assemblies.Toolkit;

namespace Assemblies.Configurations
{
    public class MarkeeConfiguration : ItemConfiguration
    {
        #region Configuração

        public Color TextColor { get; set; }

        public Color BackColor { get; set; }

        public List<string> Text { get; set; }

        public Direction Direction { get; set; }

        public int Speed { get; set; }

        public Font Font { get; set; }

        public bool TransparentBackground { get; set; }

        public bool Border { get; set; }

        #endregion
    }
}
