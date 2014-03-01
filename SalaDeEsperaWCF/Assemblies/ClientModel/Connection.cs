using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private string playerIP;
        protected PlayerPC pc;

        public string PlayerIP
        {
            get { return playerIP; }
            set { playerIP = value; }
        }
        //public abstract string CompleteServerAddress
        //{ get; }
        public abstract ConnectionState State { get; }

        protected Connection() { }
        protected Connection(PlayerPC pc) { this.pc = pc; }

        public abstract void Open();
        public abstract void Close();
        //public abstract IEnumerable<PlayerPC> GetPlayers();
        //public abstract void GetPlayersAsync(NewPlayerFoundDelegate callback);
        public abstract event GetPlayersEventHandler GetPlayersProgressChanged;

        #region Janela do Player
        public abstract void OpenPlayerWindow(PlayerWindowInformation configurations);
        public abstract void ClosePlayerWindow(string displayDeviceID);
        public abstract ScreenInformation[] GetDisplayInformation();
        public abstract bool PlayerWindowIsOpen(string displayDeviceID);
        #endregion

        #region TV
        /// <summary>
        /// Enumera os canais disponíveis no player, na frequência padrão (754000 khz).
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<Channel> GetTVChannels();
        /// <summary>
        /// Enumera os canais disponíveis no player, na frequência padrão (754000 khz). Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        public abstract IEnumerable<Channel> GetTVChannels(bool forceRescan);
        /// <summary>
        /// Enumera os canais disponíveis no player, na <paramref name="frequency"/> dada.
        /// </summary>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <returns></returns>
        public abstract IEnumerable<Channel> GetTVChannels(int frequency);
        /// <summary>
        /// Enumera os canais disponíveis no player, na <paramref name="frequency"/> dada. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        public abstract IEnumerable<Channel> GetTVChannels(int frequency, bool forceRescan);
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz.
        /// </summary>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <returns></returns>
        public abstract IEnumerable<Channel> GetTVChannels(int minFrequency, int maxFrequency, int step);
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        public abstract IEnumerable<Channel> GetTVChannels(int minFrequency, int maxFrequency, int step, bool forceRescan);
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na frequência padrão (754000 khz).
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public abstract IEnumerable<Channel> GetTVChannels(string device);
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na frequência padrão (754000 khz). Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public abstract IEnumerable<Channel> GetTVChannels(string device, bool forceRescan);
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na <paramref name="frequency"/> dada.
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public abstract IEnumerable<Channel> GetTVChannels(string device, int frequency);
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na <paramref name="frequency"/> dada. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public abstract IEnumerable<Channel> GetTVChannels(string device, int frequency, bool forceRescan);
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, no player com o DeviceID <paramref name="device"/>, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz.
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        public abstract IEnumerable<Channel> GetTVChannels(string device, int minFrequency, int maxFrequency, int step);
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
        public abstract IEnumerable<Channel> GetTVChannels(string device, int minFrequency, int maxFrequency, int step, bool forceRescan);

        public abstract Channel GetCurrentTVChannel(string displayName);
        public abstract void SetCurrentTVChannel(string displayName,Channel channel);
        public abstract GeneralDevice GetTunerDevice(string displayName);
        public abstract IEnumerable<GeneralDevice> GetTunerDevices();
        public abstract IEnumerable<GeneralDevice> GetTunerDevicesInUse();
        public abstract void SetTunerDevice(string displayName, GeneralDevice dev);

        public abstract GeneralDevice GetAudioDecoder(string displayName);
        public abstract IEnumerable<GeneralDevice> GetAudioDecoders();
        public abstract GeneralDevice GetAudioRenderer(string displayName);
        public abstract IEnumerable<GeneralDevice> GetAudioRenderers();
        public abstract GeneralDevice GetH264Decoder(string displayName);
        public abstract IEnumerable<GeneralDevice> GetH264Decoders();
        public abstract GeneralDevice GetMPEG2Decoder(string displayName);
        public abstract IEnumerable<GeneralDevice> GetMPEG2Decoders();

        public abstract void SetAudioDecoder(string displayName, GeneralDevice dev);
        public abstract void SetAudioRenderer(string displayName, GeneralDevice dev);
        public abstract void SetH264Decoder(string displayName, GeneralDevice dev);
        public abstract void SetMPEG2Decoder(string displayName, GeneralDevice dev);

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

        #region Video

        public abstract void UploadVideoFile(string filePath);
        public abstract void CancelFileUpload();
        public abstract IEnumerable<string> GetRemoteVideoFileNames();
        public abstract IEnumerable<string> GetVideoPlayerNames(string displayName);

        public abstract void StartVideo(string displayName, int videoPlayerID);
        public abstract void StopVideo(string displayName, int videoPlayerID);
        public abstract void PreviousVideo(string displayName, int videoPlayerID);
        public abstract void NextVideo(string displayName, int videoPlayerID);


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

    #region Eventos

    public class GetPlayersEventArgs : EventArgs
    {
        public int Progress { get; set; }
        public int PlayerCount { get; set; }
    }

    public delegate void GetPlayersEventHandler(object sender, GetPlayersEventArgs e);
    public delegate void NewPlayerFoundDelegate(PlayerPC pc);

    #endregion
}
