using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Windows.Forms;
using Assemblies.Components;

namespace ServerService
{
    // Arranjar forma de, de um modo consistente, conseguir chamar o executável do player
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Player : IPlayer
    {
        //FormJanelaFinal janela;
        /// <summary>
        /// Key - Screen | Value - Whether it has a player running or not
        /// </summary>
        //Dictionary<string, bool> playerMap = new Dictionary<string,bool>(); //Vai para a aplicação

        #region Events

        static public event PlayerWindowEventHandler OpenPlayerWindow;
        static public event PlayerWindowEventHandler EditPlayerWindow;
        static public event PlayerWindowCloseEventHandler ClosePlayerWindow;

        #endregion

        //public Player()
        //{
        //    foreach (var screen in Screen.AllScreens)
        //        playerMap.Add(screen.DeviceName, false);
        //    //Aplicação!!!
        //}

        #region IPlayer

        public void OpenPlayer(PlayerWindowInformation config)
        {
            //janela = new FormJanelaFinal();
            //janela.Location = config.Display.Bounds.Location;
            //janela.Size = config.Display.Bounds.Size;

            //playerMap[config.Display.DisplayName] = true;

            //janela.Show();
            ////Aplicação!!!

            if (OpenPlayerWindow != null) OpenPlayerWindow(config);
        }

        public void EditPlayer(PlayerWindowInformation config)
        {
            if (EditPlayerWindow != null) EditPlayerWindow(config);
        }

        public void ClosePlayer(string displayName)
        {
            if (ClosePlayerWindow != null) ClosePlayerWindow(displayName);
        }

        public IEnumerable<ScreenInformation> GetDisplayInformation() //Não está a dar. Verificar os callbacks (sucatada)
        {
            List<ScreenInformation> res = new List<ScreenInformation>();

            foreach (var display in Screen.AllScreens)
            {
                res.Add(ScreenInformation.FromScreen(display));
            }

            return res;
        }
        #endregion
    }

    public delegate void PlayerWindowEventHandler (PlayerWindowInformation config);
    public delegate void PlayerWindowCloseEventHandler (string deviceName);

}
