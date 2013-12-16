using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Assemblies.Toolkit;
using Transitions;

namespace Client
{
    public partial class ConfigureForm : Form
    {
        string ip, port;

        public string IP
        {
            get { return ip; }
            private set { ip = value; }
        }
        public string Port
        {
            get { return port; }
            private set { port = value; }
        }


        public ConfigureForm()
        {
            InitializeComponent();

            IP = textBoxIP.Text;
            Port = textBoxPort.Text;
        }

        public ConfigureForm(string ip, string port)
        {
            InitializeComponent();

            IP = ip;
            Port = port;

            textBoxIP.Text = ip;
            textBoxPort.Text = port;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            bool allOk = false;

            if (!MyToolkit.Networking.ValidateIPAddress(textBoxIP.Text))
            {
                Transition.run(textBoxIP, "BackColor", Color.MistyRose, new TransitionType_EaseInEaseOut(500));
            }
            else
            {
                allOk = true;
                Transition.run(textBoxIP, "BackColor", SystemColors.Window, new TransitionType_EaseInEaseOut(500));
            }

            if (!MyToolkit.Networking.ValidatePort(textBoxPort.Text))
            {
                allOk = false;
                Transition.run(textBoxPort, "BackColor", Color.MistyRose, new TransitionType_EaseInEaseOut(500));
            }
            else
            {
                allOk = true;
                Transition.run(textBoxPort, "BackColor", SystemColors.Window, new TransitionType_EaseInEaseOut(500));
            }

            if (allOk)
            {
                this.ip = textBoxIP.Text;
                this.port = textBoxPort.Text;


                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
    }
}
