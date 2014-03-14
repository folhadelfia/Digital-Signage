using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assemblies.Components;

namespace Client.Views.Main_Window
{
    public partial class ManualComponentBounds : Form
    {
        ComposerComponent component;

        Size originalSize;
        Point originalLocation;

        Size builderSize;
        Size finalResolution;

        bool ignoreTextChanged = false;

        #region Construtores

        public ManualComponentBounds(ComposerComponent component, Size builderSize)
        {
            InitializeComponent();

            this.component = component;
            this.originalSize = component.Size;
            this.originalLocation = component.Location;

            textBoxBuilderX.Text = component.Left.ToString();
            textBoxBuilderY.Text = component.Top.ToString();

            textBoxBuilderHeight.Text = component.Height.ToString();
            textBoxBuilderWidth.Text = component.Width.ToString();

            groupBoxBuilder.Text = string.Format("Builder ({0}x{1})", builderSize.Width, builderSize.Height);

            this.builderSize = builderSize;
            this.finalResolution = new Size(0, 0);
        }
        public ManualComponentBounds(ComposerComponent component, Size builderSize, Size finalResolution)
            : this(component, builderSize)
        {
            this.Size = new Size(436, 281);

            groupBoxPlayer.Text = string.Format("Player ({0}x{1})", finalResolution.Width, finalResolution.Height);
            groupBoxPlayer.Visible = true;

            double componentLeft = Convert.ToDouble(component.Left),
                   componentTop = Convert.ToDouble(component.Top),
                   componentWidth = Convert.ToDouble(component.Width),
                   componentHeigth = Convert.ToDouble(component.Height),
                   finalWidth = Convert.ToDouble(finalResolution.Width),
                   finalHeight = Convert.ToDouble(finalResolution.Height),
                   builderWidth = Convert.ToDouble(builderSize.Width),
                   builderHeigth = Convert.ToDouble(builderSize.Height);

            textBoxFinalX.Text = Math.Round((componentLeft * finalWidth) / builderWidth).ToString();
            textBoxFinalY.Text = Math.Round((componentTop * finalHeight) / builderHeigth).ToString();

            textBoxFinalWidth.Text = Math.Round((componentWidth * finalWidth) / builderWidth).ToString();
            textBoxFinalHeight.Text = Math.Round((componentHeigth * finalHeight) / builderHeigth).ToString();

            this.finalResolution = finalResolution;
        }

        #endregion

        #region TextBoxes

        /// <summary>
        /// Garante que o texto é sempre um número com ou sem sinal e válido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDigitCheck(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '-')
            {
                e.Handled = true;
            }

            // only allow one -
            if (e.KeyChar == '-'
                && (sender as TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
            }

            if (e.KeyChar == '-'
               && (sender as TextBox).SelectionStart != 0)
            {
                e.Handled = true;
            }

            if (char.IsDigit(e.KeyChar)
               && (sender as TextBox).SelectionStart == 0
               && (sender as TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Para evitar o ciclo que as textboxes fariam se a mudança de texto fosse directa
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="text"></param>
        private void SetTextBoxText(TextBox tb, string text)
        {
            ignoreTextChanged = true;

            tb.Text = text;

            ignoreTextChanged = false;
        }

        #endregion

        private void textBoxBuilderX_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTextChanged || builderSize.IsEmpty || finalResolution.IsEmpty) return;

            double x = 0;

            if (double.TryParse((sender as TextBox).Text, out x))
            {
                double componentLeft = Convert.ToDouble((sender as TextBox).Text),
                       finalWidth = Convert.ToDouble(finalResolution.Width),
                       builderWidth = Convert.ToDouble(builderSize.Width);

                this.SetTextBoxText(textBoxFinalX, Math.Round((componentLeft * finalWidth) / builderWidth).ToString());
            }
        }
        private void textBoxFinalX_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTextChanged || builderSize.IsEmpty || finalResolution.IsEmpty) return;

            double x = 0;

            if (double.TryParse((sender as TextBox).Text, out x))
            {
                double finalX = Convert.ToDouble((sender as TextBox).Text),
                       finalWidth = Convert.ToDouble(finalResolution.Width),
                       builderWidth = Convert.ToDouble(builderSize.Width);

                this.SetTextBoxText(textBoxBuilderX, Math.Round((finalX * builderWidth) / finalWidth).ToString());
            }
        }

        private void textBoxBuilderY_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTextChanged || builderSize.IsEmpty || finalResolution.IsEmpty) return;

            double x = 0;

            if (double.TryParse((sender as TextBox).Text, out x))
            {
                double componentTop = Convert.ToDouble((sender as TextBox).Text),
                       finalHeight = Convert.ToDouble(finalResolution.Height),
                       builderHeight = Convert.ToDouble(builderSize.Height);

                this.SetTextBoxText(textBoxFinalY, Math.Round((componentTop * finalHeight) / builderHeight).ToString());
            }
        }
        private void textBoxFinalY_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTextChanged || builderSize.IsEmpty || finalResolution.IsEmpty) return;

            double x = 0;

            if (double.TryParse((sender as TextBox).Text, out x))
            {
                double finalY = Convert.ToDouble((sender as TextBox).Text),
                       finalHeight = Convert.ToDouble(finalResolution.Width),
                       builderHeight = Convert.ToDouble(builderSize.Width);

                this.SetTextBoxText(textBoxBuilderY, Math.Round((finalY * builderHeight) / finalHeight).ToString());
            }
        }

        private void textBoxBuilderWidth_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTextChanged || builderSize.IsEmpty || finalResolution.IsEmpty) return;

            double x = 0;

            if (double.TryParse((sender as TextBox).Text, out x))
            {
                double componentWidth = Convert.ToDouble((sender as TextBox).Text),
                       finalWidth = Convert.ToDouble(finalResolution.Width),
                       builderWidth = Convert.ToDouble(builderSize.Width);

                this.SetTextBoxText(textBoxFinalWidth, Math.Round((componentWidth * finalWidth) / builderWidth).ToString());
            }
        }
        private void textBoxFinalWidth_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTextChanged || builderSize.IsEmpty || finalResolution.IsEmpty) return;

            double x = 0;

            if (double.TryParse((sender as TextBox).Text, out x))
            {
                double finalCompW = Convert.ToDouble((sender as TextBox).Text),
                       finalWidth = Convert.ToDouble(finalResolution.Width),
                       builderWidth = Convert.ToDouble(builderSize.Width);

                this.SetTextBoxText(textBoxBuilderWidth, Math.Round((finalCompW * builderWidth) / finalWidth).ToString());
            }
        }

        private void textBoxBuilderHeight_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTextChanged || builderSize.IsEmpty || finalResolution.IsEmpty) return;

            double x = 0;

            if (double.TryParse((sender as TextBox).Text, out x))
            {
                double componentHeight = Convert.ToDouble((sender as TextBox).Text),
                       finalHeight = Convert.ToDouble(finalResolution.Height),
                       builderHeight = Convert.ToDouble(builderSize.Height);

                this.SetTextBoxText(textBoxFinalHeight, Math.Round((componentHeight * finalHeight) / builderHeight).ToString());
            }
        }
        private void textBoxFinalHeight_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTextChanged || builderSize.IsEmpty || finalResolution.IsEmpty) return;

            double x = 0;

            if (double.TryParse((sender as TextBox).Text, out x))
            {
                double finalCompH = Convert.ToDouble((sender as TextBox).Text),
                       finalHeight = Convert.ToDouble(finalResolution.Height),
                       builderHeight = Convert.ToDouble(builderSize.Height);

                this.SetTextBoxText(textBoxBuilderHeight, Math.Round((finalCompH * builderHeight) / finalHeight).ToString());
            }
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            Apply();
        }
        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        private void buttonOk_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void Apply()
        {
            int x, y, w, h;

            int.TryParse(textBoxBuilderX.Text, out x);
            int.TryParse(textBoxBuilderY.Text, out y);
            int.TryParse(textBoxBuilderWidth.Text, out w);
            int.TryParse(textBoxBuilderHeight.Text, out h);

            component.Size = new Size(w, h);
            component.Location = new Point(x, y);
        }
        private void Cancel()
        {
            component.Size = originalSize;
            component.Location = originalLocation;
        }
    }
}
