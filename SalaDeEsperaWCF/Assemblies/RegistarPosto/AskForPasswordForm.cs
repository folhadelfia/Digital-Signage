using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assemblies.RegistarPosto
{
    public partial class AskForPasswordForm : Form
    {
        private static string ADMIN_USERNAME { get { return "master"; } }
        private static string ADMIN_PASSWORD { get { return "master"; } }

        public AskForPasswordForm()
        {
            InitializeComponent();
        }

        public string Username
        {
            get { return textBoxUsername.Text; }
        }
        public string Password
        {
            get { return textBoxPassword.Text; }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (this.Username == ADMIN_USERNAME && this.Password == ADMIN_PASSWORD)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("O utlizador ou a palavra passe inseridos não são válidos", "Login inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
