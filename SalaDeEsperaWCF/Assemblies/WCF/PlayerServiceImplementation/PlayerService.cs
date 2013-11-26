using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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


        #region Displays



        public WCFScreenInformation[] GetDisplayInformation()
        {
            List<WCFScreenInformation> res = new List<WCFScreenInformation>();

            foreach (var display in Screen.AllScreens)
            {
                res.Add(NetWCFConverter.ToWCF(display, this.GetName(display.DeviceName)));
            }

            return res.ToArray();
        }
        public WCFScreenInformation GetPrimaryDisplay()
        {
            return NetWCFConverter.ToWCF(Screen.PrimaryScreen, this.GetName(Screen.PrimaryScreen.DeviceName));
        }

        private string GetName(string devID)
        {
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            d.cb = Marshal.SizeOf(d);
            try
            {
                if(EnumDisplayDevices(devID, 0, ref d, 0) && d.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                {
                    return d.DeviceString;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("{0}", ex.ToString()));
            }

            return devID;
        }


        [DllImport("user32.dll")]
        static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        #endregion


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

    #region Monitores

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DISPLAY_DEVICE
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cb;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;
        [MarshalAs(UnmanagedType.U4)]
        public DisplayDeviceStateFlags StateFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceKey;
    }

    [Flags()]
    public enum DisplayDeviceStateFlags : int
    {
        /// <summary>The device is part of the desktop.</summary>
        AttachedToDesktop = 0x1,
        MultiDriver = 0x2,
        /// <summary>The device is part of the desktop.</summary>
        PrimaryDevice = 0x4,
        /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
        MirroringDriver = 0x8,
        /// <summary>The device is VGA compatible.</summary>
        VGACompatible = 0x10,
        /// <summary>The device is removable; it cannot be the primary display.</summary>
        Removable = 0x20,
        /// <summary>The device has more display modes than its output devices support.</summary>
        ModesPruned = 0x8000000,
        Remote = 0x4000000,
        Disconnect = 0x2000000
    }

    #endregion

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
