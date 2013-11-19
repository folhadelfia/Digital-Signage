using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assemblies.Configurations
{
    [XmlInclude(typeof(MarkeeConfiguration))]
    [XmlInclude(typeof(TVConfiguration))]
    public class ItemConfiguration
    {
        protected ItemConfiguration() //Protected para só se poderem instanciar as classe derivadas
        {
        }

        #region Tamanhos e Localizações

        /// <summary>
        /// Item's location
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Item's size
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Builder's panel resolution
        /// </summary>
        public Size Resolution { get; set; }

        /// <summary>
        /// Screen resolution
        /// </summary>
        public Size FinalResolution { get; set; }

        /// <summary>
        /// Item's size on the final display
        /// </summary>
        public Size FinalSize
        {
            get
            {
                try
                {
                    //Adicionar forma de saber o tamanho do controlo dentro das suas prorpiedades
                    float yAux = Size.Height * FinalResolution.Height / Resolution.Height;

                    float xAux = Size.Width * FinalResolution.Width / Resolution.Width;

                    int yFinal = Convert.ToInt32(Math.Round(yAux, 0, MidpointRounding.ToEven));
                    int xFinal = Convert.ToInt32(Math.Round(xAux, 0, MidpointRounding.ToEven));

                    return new Size(xFinal, yFinal);
                }
                catch (DivideByZeroException)
                {
                    return new Size(0, 0);
                }
            }
        }

        /// <summary>
        /// Item's location on the final display
        /// </summary>
        public Point FinalLocation
        {
            get
            {
                try
                {
                    float yAux = Location.Y * FinalResolution.Height / Resolution.Height;

                    float xAux = Location.X * FinalResolution.Width / Resolution.Width;

                    int yFinal = Convert.ToInt32(Math.Round(yAux, 0, MidpointRounding.ToEven));
                    int xFinal = Convert.ToInt32(Math.Round(xAux, 0, MidpointRounding.ToEven));

                    return new Point(xFinal, yFinal);
                }
                catch (DivideByZeroException)
                {
                    return new Point(0, 0);
                }
            }
        }

        #endregion
    }
}
