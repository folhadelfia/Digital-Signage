using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

//Dicionario para array
/*
 * array de displays e array de duas dimensoes de componentes. o indice de um display é o indice do array 2D
 */

namespace Assemblies.Configurations
{

    public class PlayerWindowInformation2
    {
        private Dictionary<ScreenInformation, IEnumerable<ItemConfiguration>> configuration;

        public Dictionary<ScreenInformation, IEnumerable<ItemConfiguration>> Configuration
        {
            get 
            {
                UpdateComponentSize();
                return configuration; 
            }
            set 
            { 
                configuration = value;
                UpdateComponentSize();
            }
        }

        private void UpdateComponentSize()
        {
            foreach (var display in configuration.Keys)
            {
                foreach (var item in configuration[display])
                {
                    item.FinalResolution = new System.Drawing.Size(display.Bounds.Size.Width, display.Bounds.Size.Height);
                }
            }
        }
    }

    public class PlayerWindowInformation
    {
        private IEnumerable<ItemConfiguration> components;
        private ScreenInformation display;

        public IEnumerable<ItemConfiguration> Components
        {
            get
            {
                UpdateComponentSize();

                return components;
            }
            set
            {
                components = value;

                UpdateComponentSize();
            }
        }

        public ScreenInformation Display
        {
            get
            {
                UpdateComponentSize();

                return display;
            }
            set
            {
                display = value;

                UpdateComponentSize();
            }
        }

        public Image Background { get; set; }
        public ImageLayout BackgroundImageLayout { get; set; }

        private void UpdateComponentSize()
        {
            if (display != null && components != null)
                foreach (var item in components)
                    item.FinalResolution = new System.Drawing.Size(display.Bounds.Size.Width, display.Bounds.Size.Height);
        }
    }
}
