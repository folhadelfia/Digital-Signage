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
        /// Enumera os canais disponíveis no player
        /// </summary>
        /// <returns></returns>
        [OperationContract]
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
        TunerDevice[] GetTunerDevices();

        /// <summary>
        /// Retorna o tuner a ser utilizado
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        TunerDevice GetTunerDevice(string displayName);

        /// <summary>
        /// Selecciona o tuner a ser utilizado
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        void DefineTunerDevice(string displayName, TunerDevice tuner);

        #endregion

    }
}
