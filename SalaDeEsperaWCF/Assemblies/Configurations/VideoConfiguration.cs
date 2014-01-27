using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.Configurations
{
    public class VideoConfiguration : ItemConfiguration
    {
        public int ID { get; set; }

        public List<string> Playlist { get; set; }

        public VideoConfiguration()
        {
            this.Playlist = new List<string>();
        }

    }
}
