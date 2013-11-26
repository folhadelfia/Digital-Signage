using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assemblies.Configurations
{

    public class ScreenInformation
    {
        private Rectangle bounds;
        private string deviceID;
        private bool isPrimary;
        private string name;

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }
        public string DeviceID
        {
            get { return deviceID; }
            set { deviceID = value; }
        }
        public bool Primary
        {
            get { return isPrimary; }
            set { isPrimary = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public ScreenInformation(Rectangle bounds, string devID, bool isPrimary, string name)
        {
            this.bounds = bounds;
            this.deviceID = devID;
            this.isPrimary = isPrimary;
            this.name = name;
        }
    }
}
