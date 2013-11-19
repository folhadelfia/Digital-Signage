using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assemblies.Configurations
{
    public class ScreenInformation
    {
        private Rectangle bounds;
        private string name;
        private bool isPrimary;

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }
        public string DeviceID
        {
            get { return name; }
            set { name = value; }
        }
        public bool Primary
        {
            get { return isPrimary; }
            set { isPrimary = value; }
        }

        public ScreenInformation(Rectangle bounds, string name, bool isPrimary)
        {
            this.bounds = bounds;
            this.name = name;
            this.isPrimary = isPrimary;
        }
    }
}
