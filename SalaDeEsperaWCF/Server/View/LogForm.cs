using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.View
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

        public void Log(string l)
        {
            logBox.AppendText(string.Format("[{0}]: {1}{2}", DateTime.Now.ToString("HH:mm:ss"), l, Environment.NewLine));
        }
    }
}
