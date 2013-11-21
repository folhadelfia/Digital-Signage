using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assemblies.Configurations;
using Assemblies.DataContracts;
using TV2Lib;

namespace Assemblies.ClientModel
{
    public abstract class Connection : IDisposable
    {
        private string serverIP, serverPort;
        protected PlayerPC pc;

        public string ServerIP
        {
            get { return serverIP; }
            set { serverIP = value; }
        }
        public string ServerPort
        {
            get { return serverPort; }
            set { serverPort = value; }
        }
        public abstract string CompleteServerAddress
        { get; }
        public abstract ConnectionState State { get; }

        protected Connection() { }
        protected Connection(PlayerPC pc) { this.pc = pc; }

        public abstract void Open();
        public abstract void Close();
        public abstract IEnumerable<PlayerPC> GetPlayers();

        #region Janela do Player
        public abstract void OpenPlayerWindow(PlayerWindowInformation configurations);
        public abstract void ClosePlayerWindow(string displayDeviceID);
        public abstract ScreenInformation[] GetDisplayInformation();
        public abstract bool PlayerWindowIsOpen(string displayDeviceID);
        #endregion

        #region TV
        abstract public IEnumerable<Channel> GetTVChannels();
        public abstract Channel GetCurrentTVChannel();
        public abstract void SetCurrentTVChannel(Channel channel);
        public abstract TunerDevice GetTunerDevice();
        public abstract IEnumerable<TunerDevice> GetTunerDevices();
        public abstract void SetTunerDevice(TunerDevice dev);
        #endregion

        #region Markee
        public abstract IEnumerable<string> GetFooterText();
        public abstract void AddMarkeeText(IEnumerable<string> textList);
        public abstract void RemoveMarkeeText(IEnumerable<string> textList);

        public abstract Font GetMarkeeFont();
        public abstract void SetMarkeeFont(Font font);

        public abstract Color GetMarkeeColor();
        public abstract void SetMarkeeColor(Color color);

        public abstract int GetMarkeeSpeed();
        public abstract void SetMarkeeSpeed(int speed);
        #endregion

        public abstract void Dispose();
    }

    public enum ConnectionState
    {
        Open,
        Closed,
        Opening,
        Closing,
        Failed,
        Created
    }
}
