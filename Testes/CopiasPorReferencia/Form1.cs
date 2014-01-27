using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CopiasPorReferencia
{
    public partial class Form1 : Form
    {
        List<string> lista1 = new List<string>() { "A", "B", "C", "D" };
        List<string> lista2 = new List<string>();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lista2 = lista1;

            lista2.RemoveAt(2);
        }
    }
}
