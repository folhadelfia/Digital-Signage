using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server.View;

namespace Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new ListeningForm();
            try
            {
                Application.Run(form);
            }
            catch
            {
                MessageBox.Show("Ocorreu um erro inesperado. O programa irá fechar.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!form.TryCloseService()) form.ForceCloseService();
            }
        }
    }
}
