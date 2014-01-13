using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Assemblies.Linq
{
    public static class LinqConnectionStrings
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

        public static SqlConnection LigacaoClinicas
        {
            get
            {
                string NomeServidor = "";
                string ficheiro = Application.StartupPath + "\\DadosBDClinicas.xml";

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
    }
}
