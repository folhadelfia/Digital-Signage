using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assemblies.PlayerComponents;
using Assemblies.Configurations;
using Assemblies.Components;
using Transitions;
using Assemblies.Toolkit;

namespace Assemblies.Options.OptionsGeneral
{
    public partial class FooterOptionsView : OptionsView
    {
        #region Atributos
        Markee footer = new Markee(ComponentTargetSite.OptionsMenu);
        public List<string> FooterTextList
        {
            get { return footer.TextList; }
            set { footer.TextList = value; }
        }
        public Font FooterFont
        {
            get { return footer.MarkeeFont; }
            set { footer.MarkeeFont = value; }
        }
        public Color FooterBackColor
        {
            get { return footer.BackColor; }
            set { footer.BackColor = value; }
        }
        public Color FooterTextColor
        {
            get { return footer.TextColor; }
            set { footer.TextColor = value; }
        }
        public int FooterSpeed
        {
            get { return footer.Speed; }
            set { footer.Speed = value; }
        }
        public Direction FooterDirection
        {
            get { return footer.Direction; }
            set { footer.Direction = value; }
        }
        public bool FooterTransparentBackground
        {
            get
            {
                return checkBoxTransparency.Checked;
            }
            set
            {
                checkBoxTransparency.Checked = value;
            }
        }
        #endregion

        #region Geral

        public FooterOptionsView() : base()
        {
            InitializeComponent();

            ToolTip up = new ToolTip();
            ToolTip down = new ToolTip();
            ToolTip remove = new ToolTip();

            up.SetToolTip(buttonMoveItemsUp, "Mover os items seleccionados para cima");
            down.SetToolTip(buttonMoveItemsDown, "Mover os items seleccionados para baixo");
            remove.SetToolTip(buttonRemoveItems, "Remover os items seleccionados");

            footer = new Markee(ComponentTargetSite.OptionsMenu);

            groupBoxPreview.Controls.Add(footer);
            footer.Dock = DockStyle.Fill;
            FooterTransparentBackground = false;
        }
        public FooterOptionsView(Markee temp) : base(temp)
        {
            InitializeComponent();
        }
        public FooterOptionsView(MarkeeConfiguration config) : base(config)
        {
            InitializeComponent();
        }

        #endregion

        #region Form

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                trackBarSpeed.Value = footer.Speed;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("OnLoad" + Environment.NewLine + ex.Message);
#endif
            }

            base.OnLoad(e);
        }

        #endregion

        #region Buttons

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxText.Text)) return;

                listBoxTextList.Items.Add(textBoxText.Text);

                UpdateFooterTextList();

                textBoxText.Text = string.Empty;

                if(footer.State == TransitionState.Stopped)
                    footer.Run();

                textBoxText.Focus();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("buttonAdd_Click" + Environment.NewLine + ex.Message);
#endif
            }
        }
        private void buttonFont_Click(object sender, EventArgs e)
        {
            try
            {
                fontDialog.Font = footer.MarkeeFont;
                if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    footer.MarkeeFont = fontDialog.Font;
                    //Alterar a fonte do preview
                }
            }
            catch(Exception ex)
            {
#if DEBUG
                MessageBox.Show("buttonFont_Click" + Environment.NewLine + ex.Message);
#endif
            }
        }
        private void buttonTextColor_Click(object sender, EventArgs e)
        {
            try
            {
                colorDialog.AnyColor = true;
                colorDialog.Color = footer.TextColor;

                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    footer.TextColor = colorDialog.Color;
                    //Alterar a cor do preview
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("buttonTextColor_Click" + Environment.NewLine + ex.Message);
#endif
            }
        }
        private void buttonBackColor_Click(object sender, EventArgs e)
        {
            try
            {
                colorDialog.AnyColor = true;
                colorDialog.Color = footer.BackColor;

                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    footer.BackColor = colorDialog.Color;
                    //Alterar a cor do background do preview
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("buttonBackColor_Click" + Environment.NewLine + ex.Message);
#endif
            }
        }
        private void buttonMoveItemsUp_Click(object sender, EventArgs e)
        {
            MoveListBoxItemsUp(listBoxTextList);
        }
        private void buttonMoveItemsDown_Click(object sender, EventArgs e)
        {
            MoveListBoxItemsDown(listBoxTextList);
        }
        private void buttonRemoveItems_Click(object sender, EventArgs e)
        {
            try
            {
                if(listBoxTextList.Items.Count > 0)
                    RemoveListBoxItems();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("buttonRemoveItems_Click" + Environment.NewLine + ex.Message);
#endif
            }
        }
        private void buttonPause_Click(object sender, EventArgs e)
        {
            try
            {
                if (footer.State == TransitionState.Running)
                {
                    footer.Pause();
                    (sender as Button).Text = "Continuar";
                }
                else if (footer.State == TransitionState.Paused)
                {
                    footer.Resume();
                    (sender as Button).Text = "Pausa";
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("buttonPause_Click" + Environment.NewLine + ex.Message);
#endif
            }
        }

        #endregion

        #region TextBox Texto

        private void textBoxText_KeyDown(object sender, KeyEventArgs e)
        {
            //A USAR O FORM ACCEPTBUTTON
            try
            {
                TextBox temp = sender as TextBox;

                switch (e.KeyCode)
                {
                    case Keys.Return:
                        if (string.IsNullOrWhiteSpace(textBoxText.Text)) return;

                        listBoxTextList.Items.Add(textBoxText.Text);

                        UpdateFooterTextList();

                        textBoxText.Text = string.Empty;

                        if(footer.State == TransitionState.Stopped)
                            footer.Run();

                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("textBoxText_KeyDown" + Environment.NewLine + "Tecla premida: " + e.KeyCode.ToString() + Environment.NewLine + ex.Message);
#endif
            }
        }

        #endregion

        #region TrackBar

        private void trackBarSpeed_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                footer.Speed = (sender as TrackBar).Value;
                labelSpeed.Text = "Velocidade: " + (footer.Speed == 11 ? "Max" : (footer.Speed == 0 ? "Min" : footer.Speed.ToString()));
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("trackBarSpeed_ValueChanged" + Environment.NewLine + ex.Message);
#endif
            }
        }

        #endregion

        #region ListBox Texto

        /// <summary>
        /// Altera o texto do preview. 
        /// Se nenhum estiver seleccionado, mostra todos seguidos.
        /// Se estiver algum seleccionado, mostra a selecção seguida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxTextList_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateFooterTextList();

                //Alterar o texto do preview. 
                //Se nenhum estiver seleccionado, mostra todos seguidos.
                //Se estiver algum seleccionado, mostra a selecção seguida
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("listBoxTextList_SelectedValueChanged" + Environment.NewLine + ex.Message);
#endif
            }
        }
        /// <summary>
        /// - Apaga o(s) item(s) seleccionado(s)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxTextList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                ListBox temp = sender as ListBox;

                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        RemoveListBoxItems();
                        break;
                    case Keys.Add: MoveListBoxItemsUp(sender);
                        break;
                    case Keys.Subtract: MoveListBoxItemsDown(sender);
                        break;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("listBoxTextList_KeyDown" + Environment.NewLine + ex.Message);
#endif
            }
        }

        private void RemoveListBoxItems()
        {
            try
            {
                int selectedIndex = listBoxTextList.SelectedIndices.Cast<int>().Min();
                List<object> selectedItems = listBoxTextList.SelectedItems.Cast<object>().ToList();

                foreach (object item in selectedItems)
                {
                    listBoxTextList.Items.Remove(item);
                }

                UpdateFooterTextList();

                if(listBoxTextList.Items.Count > 0)
                {
                    listBoxTextList.SelectedIndex = selectedIndex;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("RemoveListBoxItems" + Environment.NewLine + ex.Message);
#endif
            }
        }
        #region Mover items na LB http://stackoverflow.com/questions/4796109/how-to-move-item-in-listbox-up-and-down alterado para multi item
        private void MoveListBoxItemsUp(object target)
        {
            MoveItem(target as ListBox, -1);
            UpdateFooterTextList();
        }

        private void MoveListBoxItemsDown(object sender)
        {
            MoveItem(sender as ListBox, 1);
            UpdateFooterTextList();
        }

        private void MoveItem(ListBox sender, int direction)
        {
            try
            {
                List<int> selectedIndices;

                //Se estamos a mover para cima, mover os items do mais acima para o mais abaixo, e fazer o contrário se estivermos a mover para baixo
                if (direction == -1)
                    selectedIndices = sender.SelectedIndices.Cast<int>().ToList();
                else
                    selectedIndices = sender.SelectedIndices.Cast<int>().OrderByDescending(x => x).ToList();

                //Se estamos a tentar mover o 1º item da lista para cima, remove-se
                if (selectedIndices.Contains(0) && direction == -1)
                    selectedIndices.Remove(0);

                //Se estamos a tentar mover o último item da lista para baixo, remove-se
                if (selectedIndices.Contains(sender.Items.Count - 1) && direction == 1)
                    selectedIndices.Remove(sender.Items.Count - 1);

                foreach (int index in selectedIndices)
                {
                    // Calculate new index using move direction
                    int newIndex = index + direction;

                    //// Checking bounds of the range
                    //if (newIndex < 0 || newIndex >= sender.Items.Count)
                    //    return; // Index out of range - nothing to do

                    object selected = sender.Items[index];

                    //Só mover os items se a nova posição não estiver seleccionada
                    if(!(sender.SelectedIndices.Contains(newIndex)))
                    {
                        // Removing removable element
                        sender.Items.RemoveAt(index);
                        // Insert it in new position
                        sender.Items.Insert(newIndex, selected);
                        // Restore selection
                        sender.SetSelected(newIndex, true);
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("MoveItem" + Environment.NewLine + "Direction: " + direction + Environment.NewLine + ex.Message);
#endif
            }


        }
        #endregion

        #endregion

        #region Preview

        private void UpdateFooterTextList()
        {
            try
            {
                footer.TextList = listBoxTextList.Items.Cast<string>().ToList();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("UpdateFooterTextList" + Environment.NewLine + ex.Message);
#endif
            }
        }

        #endregion

        #region OptionsView
        /// <summary>
        /// Gets the configuration of the target component
        /// </summary>
        /// <returns>The ItemConfiguration of the target component</returns>
        public override Configurations.ItemConfiguration GetItemConfiguration()
        {
            return footer.Configuration as MarkeeConfiguration;
        }
        /// <summary>
        /// Sets the option values from a configuration object
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="System.ArgumentException">Thrown when the configuration doesn't match the expected type</exception>
        public override void SetValues(Configurations.ItemConfiguration configuration)
        {
            if(!(configuration is MarkeeConfiguration)) throw new ArgumentException(string.Format("Given parameter was of type {0}. {1} expected", configuration.GetType().ToString(), typeof(ItemConfiguration).ToString()), "config");

            MarkeeConfiguration config = configuration as MarkeeConfiguration;

            ToolTip up = new ToolTip();
            ToolTip down = new ToolTip();
            ToolTip remove = new ToolTip();

            up.SetToolTip(buttonMoveItemsUp, "Mover os items seleccionados para cima");
            down.SetToolTip(buttonMoveItemsDown, "Mover os items seleccionados para baixo");
            remove.SetToolTip(buttonRemoveItems, "Remover os items seleccionados");

            footer = new Markee(ComponentTargetSite.OptionsMenu);
            //{
            //    TextList = config.Text,
            //    Speed = config.Speed,
            //    BackColor = config.BackColor,
            //    TextColor = config.TextColor,
            //    FooterFont = config.Font,
            //    Direction = config.Direction,
            //    Dock = DockStyle.Fill,
            //    TransparentBackground = config.TransparentBackground
            //};

            listBoxTextList.Items.Clear();

            if (footer.TextList != null)
            {
                foreach (var item in footer.TextList) listBoxTextList.Items.Add(item);
            }

            foreach (var item in groupBoxPreview.Controls)
                if (item is Markee) groupBoxPreview.Controls.Remove(item as Markee);

            groupBoxPreview.Controls.Add(footer);
            trackBarSpeed.Value = footer.Speed;

            footer.Run();
        }
        /// <summary>
        /// Sets the option values from a player object
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="System.ArgumentException">Thrown when the player object doesn't match the expected type</exception>
        public override void SetValues(Components.ComposerComponent component)
        {
            if (!(component is Markee)) throw new ArgumentException(string.Format("Given parameter was of type {0}. {1} expected", component.GetType().ToString(), typeof(Markee).ToString()), "config");

            Markee temp = component as Markee;

            ToolTip up = new ToolTip();
            ToolTip down = new ToolTip();
            ToolTip remove = new ToolTip();

            up.SetToolTip(buttonMoveItemsUp, "Mover os items seleccionados para cima");
            down.SetToolTip(buttonMoveItemsDown, "Mover os items seleccionados para baixo");
            remove.SetToolTip(buttonRemoveItems, "Remover os items seleccionados");

            footer = new Markee(ComponentTargetSite.OptionsMenu) { TextList = temp.TextList, Speed = temp.Speed, BackColor = temp.BackColor, TextColor = temp.TextColor, MarkeeFont = temp.MarkeeFont, Direction = footer.Direction };

            listBoxTextList.Items.Clear();
            foreach (var item in footer.TextList) listBoxTextList.Items.Add(item);

            foreach (var item in groupBoxPreview.Controls)
                if (item is Markee) groupBoxPreview.Controls.Remove(item as Markee);

            groupBoxPreview.Controls.Add(footer);

            footer.Dock = DockStyle.Fill;

            trackBarSpeed.Value = footer.Speed;

            footer.Run();
        }
        /// <summary>
        /// Applies the configuration options to the component
        /// </summary>
        /// <param name="component"></param>
        /// <exception cref="System.ArgumentException">Thrown when the player object doesn't match the expected type</exception>
        public override void ApplyChangesToComponent(Assemblies.Components.ComposerComponent component)
        {
            if (!(component is Markee)) throw new ArgumentException(string.Format("Given parameter was of type {0}. {1} expected", component.GetType().ToString(), typeof(Markee).ToString()), "config");
            
        
            Markee comp = component as Markee;

            try
            {
                comp.TextList = footer.TextList;
                comp.Speed = footer.Speed;
                comp.BackColor = footer.BackColor;
                comp.TextColor = footer.TextColor;
                comp.MarkeeFont = footer.MarkeeFont;
                comp.Direction = footer.Direction;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ApplyChangesToComponent" + Environment.NewLine + ex.Message);
#endif
            }
        }
        #endregion
    }
}