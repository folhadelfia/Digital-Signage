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

        public DataContracts.WCFScreenInformation GetPrimaryDisplay()
        {
            return Channel.GetPrimaryDisplay();
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


        public DataContracts.GeneralDevice[] GetTunerDevices()
        {
            return Channel.GetTunerDevices();
        }

        public DataContracts.GeneralDevice GetTunerDevice(string displayName)
        {
            return Channel.GetTunerDevice(displayName);
        }

        public void DefineTunerDevice(string displayName, DataContracts.GeneralDevice tuner)
        {
            Channel.DefineTunerDevice(displayName, tuner);
        }


        public DataContracts.GeneralDevice[] GetTunerDevicesInUse()
        {
            return Channel.GetTunerDevicesInUse();
        }


        public DataContracts.WCFChannel[] GetChannels()
        {
            return Channel.GetChannels();
        }
        public DataContracts.WCFChannel[] GetChannels(bool forceRescan)
        {
            return Channel.GetChannels(forceRescan);
        }
        public DataContracts.WCFChannel[] GetChannels(int frequency)
        {
            return Channel.GetChannels(frequency);
        }
        public DataContracts.WCFChannel[] GetChannels(int frequency, bool forceRescan)
        {
            return Channel.GetChannels(frequency, forceRescan);
        }
        public DataContracts.WCFChannel[] GetChannels(int minFrequency, int maxFrequency, int step)
        {
            return Channel.GetChannels(minFrequency, maxFrequency, step);
        }
        public DataContracts.WCFChannel[] GetChannels(int minFrequency, int maxFrequency, int step, bool forceRescan)
        {
            return Channel.GetChannels(minFrequency, maxFrequency, step, forceRescan);
        }
        public DataContracts.WCFChannel[] GetChannels(string device)
        {
            return Channel.GetChannels(device);
        }
        public DataContracts.WCFChannel[] GetChannels(string device, bool forceRescan)
        {
            return Channel.GetChannels(device, forceRescan);
        }
        public DataContracts.WCFChannel[] GetChannels(string device, int frequency)
        {
            return Channel.GetChannels(device, frequency);
        }
        public DataContracts.WCFChannel[] GetChannels(string device, int frequency, bool forceRescan)
        {
            return Channel.GetChannels(device, frequency, forceRescan);
        }
        public DataContracts.WCFChannel[] GetChannels(string device, int minFrequency, int maxFrequency, int step)
        {
            return Channel.GetChannels(device, minFrequency, maxFrequency, step);
        }
        public DataContracts.WCFChannel[] GetChannels(string device, int minFrequency, int maxFrequency, int step, bool forceRescan)
        {
            return Channel.GetChannels(device, minFrequency, maxFrequency, step, forceRescan);
        }


        public DataContracts.GeneralDevice GetAudioDecoder(string displayName)
        {
            return Channel.GetAudioDecoder(displayName);
        }

        public DataContracts.GeneralDevice[] GetAudioDecoders()
        {
            return Channel.GetAudioDecoders();
        }

        public DataContracts.GeneralDevice GetAudioRenderer(string displayName)
        {
            return Channel.GetAudioRenderer(displayName);
        }

        public DataContracts.GeneralDevice[] GetAudioRenderers()
        {
            return Channel.GetAudioRenderers();
        }

        public DataContracts.GeneralDevice GetH264Decoder(string displayName)
        {
            return Channel.GetH264Decoder(displayName);
        }

        public DataContracts.GeneralDevice[] GetH264Decoders()
        {
            return Channel.GetH264Decoders();
        }

        public DataContracts.GeneralDevice GetMPEG2Decoder(string displayName)
        {
            return Channel.GetMPEG2Decoder(displayName);
        }

        public DataContracts.GeneralDevice[] GetMPEG2Decoders()
        {
            return Channel.GetMPEG2Decoders();
        }

        public void DefineAudioDecoder(string displayName, DataContracts.GeneralDevice dev)
        {
            Channel.DefineAudioDecoder(displayName, dev);
        }

        public void DefineAudioRenderer(string displayName, DataContracts.GeneralDevice dev)
        {
            Channel.DefineAudioRenderer(displayName, dev);
        }

        public void DefineH264Decoder(string displayName, DataContracts.GeneralDevice dev)
        {
            Channel.DefineH264Decoder(displayName, dev);
        }

        public void DefineMPEG2Decoder(string displayName, DataContracts.GeneralDevice dev)
        {
            Channel.DefineMPEG2Decoder(displayName, dev);
        }

        #region Video (ficheiros)

        public string[] GetVideoFilePaths()
        {
            return Channel.GetVideoFilePaths();
        }

        public void SetStartVideo(string displayName, int videoPlayerID)
        {
            Channel.SetStartVideo(displayName, videoPlayerID);
        }

        public void SetStopVideo(string displayName, int videoPlayerID)
        {
            Channel.SetStopVideo(displayName, videoPlayerID);
        }

        public void SetPreviousVideo(string displayName, int videoPlayerID)
        {
            Channel.SetPreviousVideo(displayName, videoPlayerID);
        }

        public void SetNextVideo(string displayName, int videoPlayerID)
        {
            Channel.SetNextVideo(displayName, videoPlayerID);
        }

        #endregion
    }
}