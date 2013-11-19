using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using PlayerServiceContracts;

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
        #endregion


        public Assemblies.DataContracts.WCFChannel[] GetChannels()
        {
            return Channel.GetChannels();
        }
    }
}
