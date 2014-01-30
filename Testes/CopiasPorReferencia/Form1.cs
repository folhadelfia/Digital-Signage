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

        private void button1_Click(object sender, EventArgs e)
        {
            double val = 0;

            if (double.TryParse(textBox1.Text, out val))
            {
                float value;
                string unit;

                Sizes.ByteToBestFitUnit(val, Sizes.Base.Base2, out value, out unit);

                label2.Text = string.Format("{0} {1} (Base2)", value, unit);


                Sizes.ByteToBestFitUnit(val, Sizes.Base.Base10, out value, out unit);
                label3.Text = string.Format("{0} {1} (Base10)", value, unit);
            }
        }
    }

    public static class Sizes
    {
        public enum Base
        {
            Base2,
            Base10
        }

        public static void ByteToBestFitUnit(double bytes, Base theBase, out float result, out string unit)
        {
            int b = 0;

            if (theBase == Base.Base2) b = 1024;
            else if (theBase == Base.Base10) b = 1000;
            else
            {
                result = -1;
                unit = "";
                return;
            }

            if (bytes < b)                     // B
            {
                result = Convert.ToSingle(bytes);
                unit = "B";
                return;
            }
            else if (bytes < Math.Pow(b, 2))   // KB Base2 / kB base10
            {
                result = Convert.ToSingle(Math.Round(bytes / b, 2));
                switch (theBase)
                {
                    case Base.Base2: unit = "KB";
                        break;
                    case Base.Base10: unit = "kB";
                        break;
                    default: unit = "KB";
                        break;
                }

                return;
            }
            else if (bytes < Math.Pow(b, 3))   // MB
            {
                result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 2), 2));
                unit = "MB";
            }
            else if (bytes < Math.Pow(b, 4))   // GB
            {
                result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 3), 2));
                unit = "GB";
            }
            else if (bytes < Math.Pow(b, 5))   // TB
            {
                result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 4), 2));
                unit = "TB";
            }
            else if (bytes < Math.Pow(b, 6))   // PB
            {
                result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 5), 2));
                unit = "PB";
            }
            else if (bytes < Math.Pow(b, 7))   // EB
            {
                result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 6), 2));
                unit = "EB";
            }
            else if (bytes < Math.Pow(b, 8))   // ZB
            {
                result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 7), 2));
                unit = "ZB";
            }
            else if (bytes < Math.Pow(b, 9))  // YB
            {
                result = Convert.ToSingle(Math.Round(bytes / Math.Pow(b, 8), 2));
                unit = "MB";
            }
            else
            {
                result = 0;
                unit = "Wtf? Something went wrong";
            }
        }
    }
}
