using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Windows.Forms;
using Assemblies.Components;
using Assemblies.Configurations;

namespace ServerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IPlayer
    {
        [OperationContract]
        void OpenPlayer(PlayerWindowInformation config);

        [OperationContract]
        void EditPlayer(PlayerWindowInformation config);

        [OperationContract]
        void ClosePlayer(string displayName);

        [OperationContract]
        IEnumerable<ScreenInformation> GetDisplayInformation();



        // TODO: Add your service operations here
    }

    [DataContract]
    public class ScreenInformation
    {
        private Rectangle bounds;
        private string name;
        private bool isPrimary;

        [DataMember]
        public Rectangle Bounds
        {
            get { return bounds; }
        }
        [DataMember]
        public string DisplayName
        {
            get { return name; }
        }
        [DataMember]
        public bool Primary
        {
            get { return isPrimary; }
        }

        [OperationContract]
        public static ScreenInformation FromScreen(Screen screen)
        {
            return new ScreenInformation() { bounds = screen.Bounds, isPrimary = screen.Primary, name = screen.DeviceName };
        }
    }

    [DataContract]
    public class PlayerWindowInformation
    {
        private IEnumerable<ItemConfiguration> components;
        private ScreenInformation display;

        [DataMember]
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

        [DataMember]
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

        private void UpdateComponentSize()
        {
            if (display != null)
                foreach (var item in components)
                    item.FinalResolution = display.Bounds.Size;
        }
    }
}
