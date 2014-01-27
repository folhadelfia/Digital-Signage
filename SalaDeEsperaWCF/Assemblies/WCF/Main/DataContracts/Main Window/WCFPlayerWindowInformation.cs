using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Assemblies.Configurations;

namespace Assemblies.DataContracts
{
    [DataContract]
    [KnownType(typeof(WCFMarkeeConfiguration))]
    [KnownType(typeof(WCFTVConfiguration))]//Adicionar uma linha destas por cada tipo a ser serializado nos components, para o programa saber de-serializar
    public class WCFPlayerWindowInformation2
    {
        private WCFItemConfiguration[][] components;
        private WCFScreenInformation[] displays;

        [DataMember]
        public WCFItemConfiguration[][] Components
        {
            get { return components; }
            set { components = value; }
        }

        public WCFScreenInformation[] Displays
        {
            get { return displays; }
            set { displays = value; }
        }

        private void UpdateComponentSize()
        {
            if (components == null || displays == null)
            {
                int indexComponents0 = 0,
                    indexComponents1 = 0;

                for (indexComponents0 = 0; indexComponents0 < components.Length; indexComponents0++)
                {
                    for(indexComponents1 = 0; indexComponents1 < components[indexComponents0].Length; indexComponents1++)
                    {
                        components[indexComponents0][indexComponents1].FinalResolution = new WCFSize(displays[indexComponents0].Bounds.Width, displays[indexComponents0].Bounds.Height);
                    }
                }
            }
        }
    }

    [DataContract]
    [KnownType(typeof(WCFMarkeeConfiguration))]
    [KnownType(typeof(WCFTVConfiguration))]
    [KnownType(typeof(WCFVideoConfiguration))]//Adicionar uma linha destas por cada tipo a ser serializado nos components, para o programa saber de-serializar
    public class WCFPlayerWindowInformation
    {
        private WCFItemConfiguration[] components;
        private WCFScreenInformation display;

        [DataMember]
        public WCFItemConfiguration[] Components
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

        [DataMember]
        public WCFScreenInformation Display
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

        [DataMember]
        public byte[] Background { get; set; }
        [DataMember]
        public int BackgroundImageLayout { get; set; }

        private void UpdateComponentSize()
        {
            if (display != null)
                foreach (var item in components)
                    item.FinalResolution = new WCFSize( display.Bounds.Size.Width,  display.Bounds.Size.Height);
        }
    }
}
