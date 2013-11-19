using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Windows.Forms;
using Assemblies.Components;
using Assemblies.Configurations;
using Assemblies.DataContracts;
using Assemblies.PlayerServiceContracts;

namespace Assemblies.PlayerServiceImplementation
{
    public class PlayerService : IPlayer
    {
        #region Events

        static public event PlayerWindowEventHandler OpenPlayerWindow;
        static public event PlayerWindowEventHandler EditPlayerWindow;
        static public event PlayerWindowCloseEventHandler ClosePlayerWindow;

        #endregion

        #region IPlayer

        public void OpenPlayer(WCFPlayerWindowInformation config)
        {
            //janela = new FormJanelaFinal();
            //janela.Location = config.Display.Bounds.Location;
            //janela.Size = config.Display.Bounds.Size;

            //playerMap[config.Display.DisplayName] = true;

            //janela.Show();
            ////Aplicação!!!

            if (OpenPlayerWindow != null) OpenPlayerWindow(config);
        }

        public void EditPlayer(WCFPlayerWindowInformation config)
        {
            if (EditPlayerWindow != null) EditPlayerWindow(config);
        }

        public void ClosePlayer(string displayName)
        {
            if (ClosePlayerWindow != null) ClosePlayerWindow(displayName);
        }

        public WCFScreenInformation[] GetDisplayInformation() //Não está a dar. Verificar os callbacks (sucatada)
        {
            List<WCFScreenInformation> res = new List<WCFScreenInformation>();

            foreach (var display in Screen.AllScreens)
            {
                res.Add(NetWCFConverter.ToWCFScreenInformation(display));
            }

            return res.ToArray();
        }

        public WCFScreenInformation GetPrimaryDisplay()
        {
            return NetWCFConverter.ToWCFScreenInformation(Screen.PrimaryScreen);
        }

        public WCFChannel[] GetChannels()
        {
            DigitalTV.DigitalTVScreen temp = new DigitalTV.DigitalTVScreen();
            List<WCFChannel> res = new List<WCFChannel>();

            temp.Frequencia = 754000;
            temp.Start();
            var channels = temp.Channels;

            temp.Stop();

            foreach (var channel in channels)
            {
                res.Add(new WCFChannel { Name = channel.Name, Type = channel.Type, SID = channel.SID });
            }

            return res.ToArray();
        }
        #endregion
    }

    public delegate void PlayerWindowEventHandler(WCFPlayerWindowInformation config);
    public delegate void PlayerWindowCloseEventHandler(string deviceName);

}
