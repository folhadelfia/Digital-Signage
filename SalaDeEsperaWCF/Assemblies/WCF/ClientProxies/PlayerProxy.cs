using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using Assemblies.PlayerServiceContracts;

namespace Assemblies.ClientProxies
{
    public class PlayerProxy : ClientBase<IPlayer>, IPlayer
    {
        public PlayerProxy(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress endpoint)
            : base(binding, endpoint)
        {
        }

        #region IPlayer members
        public void OpenPlayer(Assemblies.DataContracts.WCFPlayerWindowInformation config)
        {
            Channel.OpenPlayer(config);
        }

        public void OpenPlayer2(DataContracts.WCFPlayerWindowInformation2 config)
        {
            Channel.OpenPlayer2(config);
        }

        public void EditPlayer(Assemblies.DataContracts.WCFPlayerWindowInformation config)
        {
            Channel.EditPlayer(config);
        } // :/

        public void ClosePlayer(string displayName)
        {
            Channel.ClosePlayer(displayName);
        }

        public Assemblies.DataContracts.WCFScreenInformation[] GetDisplayInformation()
        {
            return Channel.GetDisplayInformation();
        }

        public Assemblies.DataContracts.WCFScreenInformation GetPrimaryDisplay()
        {
            return Channel.GetPrimaryDisplay();
        }
        public Assemblies.DataContracts.WCFChannel[] GetChannels()
        {
            return Channel.GetChannels();
        }

        public bool PlayerWindowIsOpen2(DataContracts.WCFScreenInformation display)
        {
            return Channel.PlayerWindowIsOpen2(display);
        }
        #endregion


        public void SetChannel(string displayName, DataContracts.WCFChannel channel)
        {
            Channel.SetChannel(displayName, channel);
        }

        public DataContracts.WCFChannel GetCurrentTVChannel(string displayName)
        {
            return Channel.GetCurrentTVChannel(displayName);
        }

        public bool PlayerWindowIsOpen(string displayName)
        {
            return Channel.PlayerWindowIsOpen(displayName);
        }


        public DataContracts.TunerDevice[] GetTunerDevices()
        {
            return Channel.GetTunerDevices();
        }

        public DataContracts.TunerDevice GetTunerDevice(string displayName)
        {
            return Channel.GetTunerDevice(displayName);
        }

        public void DefineTunerDevice(string displayName, DataContracts.TunerDevice tuner)
        {
            Channel.DefineTunerDevice(displayName, tuner);
        }


        public DataContracts.TunerDevice[] GetTunerDevicesInUse()
        {
            return Channel.GetTunerDevicesInUse();
        }
    }
}