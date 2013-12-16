using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server.View;
using Server.Linq;
using System.Management;
using System.Net.NetworkInformation;
using Assemblies.Toolkit;
using Assemblies.RegistarPosto;

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

            #region Registar posto

            RegistarPostoDataContext dbRegistarPosto = new RegistarPostoDataContext();
            ClinicasDataContext dbClinicas = new ClinicasDataContext();

            int idClinica = dbClinicas.Clinicas.Single(x => x.Estado == true).idClinica;

            bool postoRegistado = dbRegistarPosto.WorkStations.Where(x => x.idClinic == idClinica && x.uuid == MyToolkit.Hardware.UUID && x.macAddress == MyToolkit.Networking.HardwareAddress.ToString() && x.isActive == true).Count() > 0;

            #endregion

            if (!postoRegistado)
            {
                AskForPasswordForm passForm = new AskForPasswordForm();

                if (passForm.ShowDialog() == DialogResult.OK)
                {
                    dbRegistarPosto.WorkStations.InsertOnSubmit(new WorkStation()
                    {
                        date = DateTime.Now,
                        idClinic = idClinica,
                        isActive = true,
                        macAddress = MyToolkit.Networking.HardwareAddress.ToString(),
                        name = MyToolkit.Networking.Hostname,
                        uuid = MyToolkit.Hardware.UUID
                    });

                    dbRegistarPosto.SubmitChanges();

                        
                }
                else return;
            }

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



        public static string ToString(this PhysicalAddress target, string separator)
        {
            string str = target.ToString();

            for (int i = str.Length - 2; i > 0; i -= 2)
                str = str.Insert(i, separator);

            return str;
        }
    }
}
