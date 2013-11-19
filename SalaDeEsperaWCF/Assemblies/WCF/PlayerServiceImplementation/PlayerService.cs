using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        static public event PlayerWindowEventHandler2 OpenPlayerWindow2;
        static public event PlayerWindowEventHandler EditPlayerWindow;
        static public event PlayerWindowCloseEventHandler ClosePlayerWindow;

        static public event PlayerWindowTuneToChannelEventHandler TuneToChannel;
        static public event SendChannelBackEventHandler SendTunedChannel;
        static public event SendWindowIsOpenBackEventHandler SendWindowIsOpen;

        static public event SendDevicesBackEventHandler SendTunerDevices;
        static public event SendDeviceBackEventHandler SendTunerDevice;
        static public event PlayerWindowSetTunerDeviceEventHandler SetTunerDevice;

        #endregion

        #region IPlayer

        public void OpenPlayer(WCFPlayerWindowInformation config)
        {
            if (OpenPlayerWindow != null) OpenPlayerWindow(config);
        }
        public void OpenPlayer2(WCFPlayerWindowInformation2 config)
        {
            if (OpenPlayerWindow2 != null) OpenPlayerWindow2(config);
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
                res.Add(NetWCFConverter.ToWCF(display));
            }

            return res.ToArray();
        }
        public WCFScreenInformation GetPrimaryDisplay()
        {
            return NetWCFConverter.ToWCF(Screen.PrimaryScreen);
        }

        // Adicionar um método estático na classe do player de tv para devolver os canais
        public WCFChannel[] GetChannels()
        {
            try
            {
                var temp = new TV2Lib.DigitalTVScreen();

                temp.Channels.CurrentChannel.Frequency = 754000;

                try
                {
                    temp.Channels.LoadFromXML();
                }
                catch (Exception)
                {
                    temp.Channels.RefreshChannels(); //Só funciona em Portugal. Compor o ecra de opçoes no composer
                    temp.Channels.SaveToXML();
                }

                var channels = temp.Channels.ChannelList;

                return NetWCFConverter.ToWCF(channels.ToArray());
            }
            catch
            {
                return new WCFChannel[0];
            }
        }


        public void SetChannel(string displayName, WCFChannel channel)
        {
            if (TuneToChannel != null) TuneToChannel(displayName, channel);
        }

        public WCFChannel GetCurrentTVChannel(string displayName)
        {
            if (SendTunedChannel == null) return null;

            WCFChannel ch;

            SendTunedChannel(displayName, out ch);

            return ch;
        }

        public bool PlayerWindowIsOpen(string displayName)
        {
            if (SendWindowIsOpen == null) return false;

            bool isOpen;

            SendWindowIsOpen(displayName, out isOpen);

            return isOpen;
        }

        public bool PlayerWindowIsOpen2(WCFScreenInformation display)
        {
            throw new NotImplementedException();
        }
        #endregion


        public TunerDevice[] GetTunerDevices()
        {
            if (SendTunerDevices == null) return null;

            TunerDevice[] devs;

            SendTunerDevices(out devs);

            return devs;
        }


        public TunerDevice GetTunerDevice(string displayName)
        {
            if (SendTunerDevice == null) return null;

            TunerDevice dev;

            SendTunerDevice(displayName, out dev);

            return dev;
        }

        public void DefineTunerDevice(string displayName, TunerDevice tuner)
        {
            if (SetTunerDevice != null) SetTunerDevice(displayName, tuner);
        }
    }

    public delegate void PlayerWindowEventHandler(WCFPlayerWindowInformation config);
    public delegate void PlayerWindowEventHandler2(WCFPlayerWindowInformation2 config);
    public delegate void PlayerWindowCloseEventHandler(string display);

    public delegate void PlayerWindowTuneToChannelEventHandler(string displayName, WCFChannel ch);
    public delegate void SendChannelBackEventHandler(string displayName, out WCFChannel ch);
    public delegate void SendWindowIsOpenBackEventHandler(string displayName, out bool IsOpen);

    public delegate void SendDevicesBackEventHandler(out TunerDevice[] devs);
    public delegate void SendDeviceBackEventHandler(string displayName, out TunerDevice dev);
    public delegate void PlayerWindowSetTunerDeviceEventHandler(string displayName, TunerDevice dev);

}
