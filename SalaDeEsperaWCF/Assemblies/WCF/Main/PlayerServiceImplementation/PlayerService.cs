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
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
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
        static public event SendDevicesBackEventHandler SendTunerDevicesInUse;
        static public event PlayerWindowSendDeviceBackEventHandler SendTunerDevice;
        static public event PlayerWindowSetDeviceEventHandler SetTunerDevice;

        static public event PlayerWindowSendDeviceBackEventHandler SendAudioDecoder;
        static public event PlayerWindowSendDeviceBackEventHandler SendAudioRenderer;
        static public event PlayerWindowSendDeviceBackEventHandler SendH264Decoder;
        static public event PlayerWindowSendDeviceBackEventHandler SendMPEG2Decoder;

        static public event SendDevicesBackEventHandler SendAudioDecoders;
        static public event SendDevicesBackEventHandler SendAudioRenderers;
        static public event SendDevicesBackEventHandler SendH264Decoders;
        static public event SendDevicesBackEventHandler SendMPEG2Decoders;

        static public event PlayerWindowSetDeviceEventHandler SetAudioDecoder;
        static public event PlayerWindowSetDeviceEventHandler SetAudioRenderer;
        static public event PlayerWindowSetDeviceEventHandler SetH264Decoder;
        static public event PlayerWindowSetDeviceEventHandler SetMPEG2Decoder;

        static public event SendPlayerHasComponentBack HasVideoComponent;
        static public event SendPlayerHasComponentBack HasTVComponent;

        #region Video (ficheiro)

        static public event SendVideoFilePathsBackEventHandler SendVideoFilePaths;
        static public event SendVideoNamesBackEventHandler SendVideoNamesBack;

        static public event PlayerWindowVideoControlEventHandler StartVideo;
        static public event PlayerWindowVideoControlEventHandler StopVideo;
        static public event PlayerWindowVideoControlEventHandler PreviousVideo;
        static public event PlayerWindowVideoControlEventHandler NextVideo;

        #endregion
        //Adicionar os sets


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


        public GeneralDevice[] GetTunerDevices()
        {
            if (SendTunerDevices == null) return null;

            GeneralDevice[] devs;

            SendTunerDevices(out devs);

            return devs;
        }

        public GeneralDevice GetTunerDevice(string displayName)
        {
            if (SendTunerDevice == null) return null;

            GeneralDevice dev;

            SendTunerDevice(displayName, out dev);

            return dev;
        }

        public void DefineTunerDevice(string displayName, GeneralDevice tuner)
        {
            if (SetTunerDevice != null) SetTunerDevice(displayName, tuner);
        }

        public GeneralDevice[] GetTunerDevicesInUse()
        {
            if (SendTunerDevicesInUse == null) return null;

            GeneralDevice[] devs;

            SendTunerDevicesInUse(out devs);

            return devs;
        }

        /// <summary>
        /// Enumera os canais disponíveis no player, na frequência padrão (754000 khz).
        /// </summary>
        /// <returns></returns>
        public WCFChannel[] GetChannels()
        {
            return this.GetChannels(false);
        }// Done
        /// <summary>
        /// Enumera os canais disponíveis no player, na frequência padrão (754000 khz). Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        public WCFChannel[] GetChannels(bool forceRescan)
        {
            return this.GetChannels(TV2Lib.DigitalTVScreen.ChannelStuff.DEFAULT_FREQUENCY, forceRescan);
        }// Done
        /// <summary>
        /// Enumera os canais disponíveis no player, na <paramref name="frequency"/> dada.
        /// </summary>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <returns></returns>
        public WCFChannel[] GetChannels(int frequency)
        {
            return this.GetChannels(frequency, false);
        }// Done
        /// <summary>
        /// Enumera os canais disponíveis no player, na <paramref name="frequency"/> dada. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        public WCFChannel[] GetChannels(int frequency, bool forceRescan)
        {
            try
            {
                return this.GetChannels(TV2Lib.DigitalTVScreen.DeviceStuff.TunerDevices.ElementAt(0).Key, frequency, forceRescan);
            }
            catch(ArgumentOutOfRangeException)
            {
                return null;
            }
            catch
            {
                return new WCFChannel[0];
            }
        }// Done
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz.
        /// </summary>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <returns></returns>
        public WCFChannel[] GetChannels(int minFrequency, int maxFrequency, int step)
        {
            return this.GetChannels(minFrequency, maxFrequency, step, false);
        } // Done
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        public WCFChannel[] GetChannels(int minFrequency, int maxFrequency, int step, bool forceRescan)
        {

            try
            {
                return this.GetChannels(TV2Lib.DigitalTVScreen.DeviceStuff.TunerDevices.ElementAt(0).Key, minFrequency, maxFrequency, step, forceRescan);
            }
            catch(ArgumentOutOfRangeException)
            {
                return null;
            }
            catch
            {
                return new WCFChannel[0];
            }
        }// Done
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na frequência padrão (754000 khz).
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public WCFChannel[] GetChannels(string device)
        {
            return this.GetChannels(device, TV2Lib.DigitalTVScreen.ChannelStuff.DEFAULT_FREQUENCY, false);
        }// Done
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na frequência padrão (754000 khz). Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public WCFChannel[] GetChannels(string device, bool forceRescan)
        {
            return this.GetChannels(device, TV2Lib.DigitalTVScreen.ChannelStuff.DEFAULT_FREQUENCY, forceRescan);
        }// Done
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na <paramref name="frequency"/> dada.
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public WCFChannel[] GetChannels(string device, int frequency)
        {
            return this.GetChannels(device, frequency, false);
        }// Done
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na <paramref name="frequency"/> dada. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public WCFChannel[] GetChannels(string device, int frequency, bool forceRescan)
        {
            try
            {
                var temp = new TV2Lib.DigitalTVScreen();

                temp.Channels.CurrentChannel.Frequency = frequency;

                try
                {
                    temp.Devices.TunerDevice = TV2Lib.DigitalTVScreen.DeviceStuff.TunerDevices[device];
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error retrieving the tuner device", ex);
                }

                if (forceRescan)
                {
                    temp.Channels.RefreshChannels(); 
                    temp.Channels.SaveToXML();
                }
                else
                {
                    try
                    {
                        temp.Channels.LoadFromXML();
                    }
                    catch (Exception)
                    {
                        temp.Channels.RefreshChannels(); 
                        temp.Channels.SaveToXML();
                    }
                }

                var channels = temp.Channels.ChannelList;

                return NetWCFConverter.ToWCF(channels.ToArray());
            }
            catch
            {
                return new WCFChannel[0];
            }
        }// Done
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, no player com o DeviceID <paramref name="device"/>, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz.
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public WCFChannel[] GetChannels(string device, int minFrequency, int maxFrequency, int step)
        {
            return this.GetChannels(device, minFrequency, maxFrequency, step, false);
        }// Done
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, no player com o DeviceID <paramref name="device"/>, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public WCFChannel[] GetChannels(string device, int minFrequency, int maxFrequency, int step, bool forceRescan)
        {
            try
            {
                var temp = new TV2Lib.DigitalTVScreen();

                try
                {
                    temp.Devices.TunerDevice = TV2Lib.DigitalTVScreen.DeviceStuff.TunerDevices[device];
                }
                catch(Exception ex)
                {
                    throw new ApplicationException("Error retrieving the tuner device", ex);
                }

                if (forceRescan)
                {
                    temp.Channels.ScanFrequencies(minFrequency, maxFrequency, step);
                    temp.Channels.SaveToXML();
                }
                else
                {
                    try
                    {
                        temp.Channels.LoadFromXML();
                    }
                    catch
                    {
                        temp.Channels.ScanFrequencies(minFrequency, maxFrequency, step); 
                        temp.Channels.SaveToXML();
                    }
                }

                var channels = temp.Channels.ChannelList;

                return NetWCFConverter.ToWCF(channels.ToArray());
            }
            catch
            {
                return new WCFChannel[0];
            }
        }// Done


        public GeneralDevice GetAudioDecoder(string displayName)
        {
            if (SendAudioDecoder == null) return null;

            GeneralDevice dev;

            SendAudioDecoder(displayName, out dev);

            return dev;
        }
        public void DefineAudioDecoder(string displayName, GeneralDevice dev)
        {
            if (SetAudioDecoder != null) SetAudioDecoder(displayName, dev);
        }

        public GeneralDevice[] GetAudioDecoders()
        {
            if (SendAudioDecoders == null) return null;

            GeneralDevice[] devs;

            SendAudioDecoders(out devs);

            return devs;
        }


        public GeneralDevice GetAudioRenderer(string displayName)
        {
            if (SendAudioRenderer == null) return null;

            GeneralDevice dev;

            SendAudioRenderer(displayName, out dev);

            return dev;
        }
        public void DefineAudioRenderer(string displayName, GeneralDevice dev)
        {
            if (SetAudioRenderer != null) SetAudioRenderer(displayName, dev);
        }

        public GeneralDevice[] GetAudioRenderers()
        {
            if (SendAudioRenderers == null) return null;

            GeneralDevice[] devs;

            SendAudioRenderers(out devs);

            return devs;
        }


        public GeneralDevice GetH264Decoder(string displayName)
        {
            if (SendH264Decoder == null) return null;

            GeneralDevice dev;

            SendH264Decoder(displayName, out dev);

            return dev;
        }
        public void DefineH264Decoder(string displayName, GeneralDevice dev)
        {
            if (SetH264Decoder != null) SetH264Decoder(displayName, dev);
        }

        public GeneralDevice[] GetH264Decoders()
        {
            if (SendH264Decoders == null) return null;

            GeneralDevice[] devs;

            SendH264Decoders(out devs);

            return devs;
        }


        public GeneralDevice GetMPEG2Decoder(string displayName)
        {
            if (SendMPEG2Decoder == null) return null;

            GeneralDevice dev;

            SendMPEG2Decoder(displayName, out dev);

            return dev;
        }
        public void DefineMPEG2Decoder(string displayName, GeneralDevice dev)
        {
            if (SetMPEG2Decoder != null) SetMPEG2Decoder(displayName, dev);
        }

        public GeneralDevice[] GetMPEG2Decoders()
        {
            if (SendMPEG2Decoders == null) return null;

            GeneralDevice[] devs;

            SendMPEG2Decoders(out devs);

            return devs;
        }


        #region 27 jan 2014 - Controlos do video (ficheiro)

        public string[] GetVideoFilePaths()
        {
            if (SendVideoFilePaths == null) return null;

            string[] files;

            SendVideoFilePaths(out files);

            return files;
        }

        public void SetStartVideo(string displayName, int videoPlayerID)
        {
            if (StartVideo != null) StartVideo(displayName, videoPlayerID);
        }
        public void SetStopVideo(string displayName, int videoPlayerID)
        {
            if (StopVideo != null) StopVideo(displayName, videoPlayerID);
        }
        public void SetPreviousVideo(string displayName, int videoPlayerID)
        {
            if (PreviousVideo != null) PreviousVideo(displayName, videoPlayerID);
        }
        public void SetNextVideo(string displayName, int videoPlayerID)
        {
            if (NextVideo != null) NextVideo(displayName, videoPlayerID);
        }

        #endregion


        public string[] GetVideoPlayerNames(string displayName)
        {
            if (SendVideoNamesBack != null) 
            {
                string[] res = null;
                SendVideoNamesBack(displayName, out res);

                return res;
            }

            return null;
        }


        public bool HasVideo(string displayName)
        {
            bool res = false;

            if (HasVideoComponent != null) HasVideoComponent(displayName, out res);

            return res;
        }

        public bool HasTV(string displayName)
        {
            bool res = false;

            if (HasTVComponent != null) HasTVComponent(displayName, out res);

            return res;
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

    public delegate void SendDevicesBackEventHandler(out GeneralDevice[] devs);
    public delegate void PlayerWindowSendDeviceBackEventHandler(string displayName, out GeneralDevice dev);
    public delegate void PlayerWindowSetDeviceEventHandler(string displayName, GeneralDevice dev);

    public delegate void PlayerWindowVideoControlEventHandler(string displayName, int videoPlayerID);
    public delegate void SendVideoFilePathsBackEventHandler(out string[] paths);
    public delegate void SendVideoNamesBackEventHandler(string displayName, out string[] names);

    public delegate void SendPlayerHasComponentBack(string displayName, out bool result);
}
