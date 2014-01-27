using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Assemblies.Configurations;
using Assemblies.ExtensionMethods;

namespace Assemblies.Options
{
    public partial class VideoOptions : Form
    {
        public List<string> Playlist { get; private set; }

        public VideoOptions()
        {
            InitializeComponent();
        }

        public VideoOptions(VideoConfiguration config)
        {
            config.Playlist.DeepCopyTo(this.Playlist);
        }
    }
}
