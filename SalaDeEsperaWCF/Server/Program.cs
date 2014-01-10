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
using System.Data.SqlClient;
using System.Xml;
using System.IO;

namespace Server
{
    static class Program
    {
        public static SqlConnection LigacaoClinica
        {
            get
            {
                string NomeServidor = "";
                string ficheiro = Application.StartupPath + "\\DadosBDClinica.xml";

                try
                {
                    XmlTextReader lerXml = new XmlTextReader(ficheiro);
                    while (lerXml.Read())
                    {
                        if (lerXml.IsStartElement("NomeServidor"))
                            NomeServidor = lerXml.ReadElementString("NomeServidor");
                    }

                    lerXml.Close();
                }
                catch (IOException IOex)
                {
                    MessageBox.Show(IOex.Message, "Erro na Leitura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro na Leitura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                return new SqlConnection("Server=" + NomeServidor + "; Database=GestProgram; User ID=Cliente; Password=cliente; Trusted_Connection=False;");
            }
        }

        public static SqlConnection LigacaoRegistarPosto
        {
            get
            {
                string NomeServidor = "";
                string ficheiro = Application.StartupPath + "\\DadosBDRegistarPosto.xml";

                try
                {
                    XmlTextReader lerXml = new XmlTextReader(ficheiro);
                    while (lerXml.Read())
                    {
                        if (lerXml.IsStartElement("NomeServidor"))
                            NomeServidor = lerXml.ReadElementString("NomeServidor");
                    }

                    lerXml.Close();
                }
                catch (IOException IOex)
                {
                    MessageBox.Show(IOex.Message, "Erro na Leitura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro na Leitura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                return new SqlConnection("Server=" + NomeServidor + "; Database=GestMulti; User ID=Cliente; Password=cliente; Trusted_Connection=False;");
            }
        }

        public static SqlConnection LigacaoPlayersLigados
        {
            get
            {
                string NomeServidor = "";
                string ficheiro = Application.StartupPath + "\\DadosBDDigitalSignage.xml";

                try
                {
                    XmlTextReader lerXml = new XmlTextReader(ficheiro);
                    while (lerXml.Read())
                    {
                        if (lerXml.IsStartElement("NomeServidor"))
                            NomeServidor = lerXml.ReadElementString("NomeServidor");
                    }

                    lerXml.Close();
                }
                catch (IOException IOex)
                {
                    MessageBox.Show(IOex.Message, "Erro na Leitura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro na Leitura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                return new SqlConnection("Server=" + NomeServidor + "; Database=DigitalSignage; User ID=Cliente; Password=cliente; Trusted_Connection=False;");
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region Registar posto

            try
            {
                RegistarPostoDataContext dbRegistarPosto = new RegistarPostoDataContext(LigacaoRegistarPosto);
                ClinicaDataContext dbClinica = new ClinicaDataContext(LigacaoClinica);

                int idClinica = dbClinica.ClinicaDados.Single().idClinicaMulti ?? -1;

                if (idClinica < 0) throw new ArgumentException("Esta clínica não está registada");

                bool postoRegistado = dbRegistarPosto.WorkStations.Where(x => x.idClinic == idClinica && x.uuid == MyToolkit.Hardware.UUID && x.isActive == true).Count() > 0;

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
                            name = MyToolkit.Networking.PrivateHostname,
                            uuid = MyToolkit.Hardware.UUID
                        });

                        dbRegistarPosto.SubmitChanges();
                    }
                    else return;
                }
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "O programa irá agora sair.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch
            {
                MessageBox.Show("Não foi possível ligar ao servidor." + Environment.NewLine + "O programa irá agora sair.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
                form.CloseServiceWithDatabase();
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
