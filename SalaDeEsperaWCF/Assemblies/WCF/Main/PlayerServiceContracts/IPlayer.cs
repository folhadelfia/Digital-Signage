using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Assemblies.Components;
using Assemblies.Configurations;

using System.ServiceModel;
using System.ServiceModel.Discovery;

using Assemblies.DataContracts;

namespace Assemblies.PlayerServiceContracts
{   
    [ServiceContract(Name = "PlayerService")]
    public interface IPlayer
    {
        /// <summary>
        /// Abre uma janela do player no monitor principal do dispositivo
        /// </summary>
        /// <param name="config"></param>
        [OperationContract]
        void OpenPlayer(WCFPlayerWindowInformation config);
        /// <summary>
        /// Abre uma janela do player no monitor seleccionado
        /// </summary>
        /// <param name="config"></param>
        [OperationContract]
        void OpenPlayer2(WCFPlayerWindowInformation2 config);
        /// <summary>
        /// Ainda nem sei bem o que fazer com isto
        /// </summary>
        /// <param name="config"></param>
        [OperationContract]
        void EditPlayer(WCFPlayerWindowInformation config);
        /// <summary>
        /// Fecha a janela aberta no monitor com o nome displayName
        /// </summary>
        /// <param name="displayName"></param>
        [OperationContract]
        void ClosePlayer(string displayName);

        /// <summary>
        /// Enumera os monitores disponíveis
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        WCFScreenInformation[] GetDisplayInformation();
        /// <summary>
        /// Dá informação do monitor principal
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        WCFScreenInformation GetPrimaryDisplay();

        /// <summary>
        /// Enumera os canais disponíveis no player, na frequência padrão (754000 khz).
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name="GetChannels")]
        WCFChannel[] GetChannels();

        #region 14 ago 2013 - Mudar de canal pelo Client, saber que canal está a mostrar, saber se o player está aberto

        /// <summary>
        /// Sintoniza o canal escolhido
        /// </summary>
        /// <param name="channel"></param>
        [OperationContract]
        void SetChannel(string displayName, WCFChannel channel);

        /// <summary>
        /// Retorna o canal que está a ser apresentado
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        WCFChannel GetCurrentTVChannel(string displayName);

        /// <summary>
        /// Diz se a janela do player está ou não aberta
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool PlayerWindowIsOpen(string displayName);
        /// <summary>
        /// Diz se a janela do player está ou não aberta no monitor "display"
        /// </summary>
        /// <param name="display"></param>
        /// <returns></returns>
        [OperationContract]
        bool PlayerWindowIsOpen2(WCFScreenInformation display);
        #endregion

        #region 11 nov 2013 - Escolher o tuner

        /// <summary>
        /// Enumera os tuners do player
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        GeneralDevice[] GetTunerDevices();

        /// <summary>
        /// Retorna o tuner a ser utilizado
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        GeneralDevice GetTunerDevice(string displayName);

        /// <summary>
        /// Selecciona o tuner a ser utilizado
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        void DefineTunerDevice(string displayName, GeneralDevice tuner);

        #endregion

        #region 27 nov 2013 - Saber que tuners estão em uso


        /// <summary>
        /// Enumera os tuners em uso do player
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        GeneralDevice[] GetTunerDevicesInUse();

        #endregion

        #region 29 nov 2013 - GetChannels com parâmetros
        /// <summary>
        /// Enumera os canais disponíveis no player, na frequência padrão (754000 khz). Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        [OperationContract(Name = "GetChannelsForce")]
        WCFChannel[] GetChannels(bool forceRescan);
        /// <summary>
        /// Enumera os canais disponíveis no player, na <paramref name="frequency"/> dada.
        /// </summary>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <returns></returns>
        [OperationContract(Name = "GetChannelsFrequency")]
        WCFChannel[] GetChannels(int frequency);
        /// <summary>
        /// Enumera os canais disponíveis no player, na <paramref name="frequency"/> dada. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        [OperationContract(Name = "GetChannelsFrequencyForce")]
        WCFChannel[] GetChannels(int frequency, bool forceRescan);
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz.
        /// </summary>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <returns></returns>
        [OperationContract(Name = "GetChannelsScan")]
        WCFChannel[] GetChannels(int minFrequency, int maxFrequency, int step);
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        [OperationContract(Name = "GetChannelsScanForce")]
        WCFChannel[] GetChannels(int minFrequency, int maxFrequency, int step, bool forceRescan);
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na frequência padrão (754000 khz).
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        [OperationContract(Name = "GetChannelsDevice")]
        WCFChannel[] GetChannels(string device);
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na frequência padrão (754000 khz). Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        [OperationContract(Name = "GetChannelsDeviceForce")]
        WCFChannel[] GetChannels(string device, bool forceRescan);
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na <paramref name="frequency"/> dada.
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        [OperationContract(Name = "GetChanneslDeviceFrequency")]
        WCFChannel[] GetChannels(string device, int frequency);
        /// <summary>
        /// Enumera os canais disponíveis no player com o DeviceID <paramref name="device"/>, na <paramref name="frequency"/> dada. Se <paramref name="forceRescan"/>, faz o scan em vez de utilizar o ficheiro XML (se disponível)
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="frequency">Frequência, em khz, a ser utilizada para o scan</param>
        /// <param name="forceRescan">Se true, o player faz o scan obrigatoriamente</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        [OperationContract(Name = "GetChannelsDeviceFrequencyForce")]
        WCFChannel[] GetChannels(string device, int frequency, bool forceRescan);
        /// <summary>
        /// Enumera os canais disponíveis entre duas frequências, no player com o DeviceID <paramref name="device"/>, fazendo o scan de <paramref name="step"/> em <paramref name="step"/> khz.
        /// </summary>
        /// <param name="device">Identificador do dispositivo.</param>
        /// <param name="minFrequency">Frequência, em khz, onde começa o scan</param>
        /// <param name="maxFrequency">Frequência, em khz, onde acaba o scan</param>
        /// <param name="step">Incremento, em khz, de scan para scan</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Lançada se o <paramref name="device"/> não corresponder a nenhum dispositivo.</exception>
        [OperationContract(Name = "GetChannelsDeviceScan")]
        WCFChannel[] GetChannels(string device, int minFrequency, int maxFrequency, int step);
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
        [OperationContract(Name = "GetChannelsDeviceScanForce")]
        WCFChannel[] GetChannels(string device, int minFrequency, int maxFrequency, int step, bool forceRescan);

        #endregion

        #region 2 dez 2013 - Gets e sets dos codecs

        [OperationContract]
        GeneralDevice GetAudioDecoder(string displayName);

        [OperationContract]
        GeneralDevice[] GetAudioDecoders();

        [OperationContract]
        GeneralDevice GetAudioRenderer(string displayName);

        [OperationContract]
        GeneralDevice[] GetAudioRenderers();

        [OperationContract]
        GeneralDevice GetH264Decoder(string displayName);

        [OperationContract]
        GeneralDevice[] GetH264Decoders();

        [OperationContract]
        GeneralDevice GetMPEG2Decoder(string displayName);

        [OperationContract]
        GeneralDevice[] GetMPEG2Decoders();


        [OperationContract]
        void DefineAudioDecoder(string displayName, GeneralDevice dev);

        [OperationContract]
        void DefineAudioRenderer(string displayName, GeneralDevice dev);

        [OperationContract]
        void DefineH264Decoder(string displayName, GeneralDevice dev);

        [OperationContract]
        void DefineMPEG2Decoder(string displayName, GeneralDevice dev);

        #endregion

        #region 27 jan 2014 - Controlos do video (ficheiro)

        [OperationContract]
        string[] GetVideoFilePaths();

        [OperationContract]
        void SetStartVideo(string displayName, int videoPlayerID);

        [OperationContract]
        void SetStopVideo(string displayName, int videoPlayerID);

        [OperationContract]
        void SetPreviousVideo(string displayName, int videoPlayerID);

        [OperationContract]
        void SetNextVideo(string displayName, int videoPlayerID);

        #endregion
    }
}
