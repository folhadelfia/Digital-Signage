using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assemblies.Components;
using Assemblies.Configurations;
using Assemblies.PlayerComponents;
using Assemblies.Toolkit;
using Transitions;

namespace Assemblies.Options
{
    public partial class MarkeeOptions : Form, IOptionsWindow
    {
        #region Atributos
        Markee markee = new Markee(ComponentTargetSite.OptionsMenu);

        public List<string> MarkeeTextList
        {
            get { return markee.TextList; }
            set { markee.TextList = value; }
        }
        public Font MarkeeFont
        {
            get { return markee.MarkeeFont; }
            set { markee.MarkeeFont = value; }
        }
        public Color MarkeeBackColor
        {
            get { return markee.BackColor; }
            set { markee.BackColor = value; }
        }
        public Color MarkeeTextColor
        {
            get { return markee.TextColor; }
            set { markee.TextColor = value; }
        }
        public int MarkeeSpeed
        {
            get { return markee.Speed; }
            set { markee.Speed = value; }
        }
        public Direction MarkeeDirection
        {
            get { return markee.Direction; }
            set { markee.Direction = value; }
        }
        #endregion

        #region Geral

        public MarkeeOptions()
        {
            InitializeComponent();

            ToolTip up = new ToolTip();
            ToolTip down = new ToolTip();
            ToolTip remove = new ToolTip();

            up.SetToolTip(buttonMoveItemsUp, "Mover os items seleccionados para cima");
            down.SetToolTip(buttonMoveItemsDown, "Mover os items seleccionados para baixo");
            remove.SetToolTip(buttonRemoveItems, "Remover os items seleccionados");

            markee = new Markee(ComponentTargetSite.OptionsMenu);

            groupBoxPreview.Controls.Add(markee);
            markee.Dock = DockStyle.Fill;
        }
        public MarkeeOptions(Markee temp)
        {
            InitializeComponent();

            ToolTip up = new ToolTip();
            ToolTip down = new ToolTip();
            ToolTip remove = new ToolTip();

            up.SetToolTip(buttonMoveItemsUp, "Mover os items seleccionados para cima");
            down.SetToolTip(buttonMoveItemsDown, "Mover os items seleccionados para baixo");
            remove.SetToolTip(buttonRemoveItems, "Remover os items seleccionados");

            markee = new Markee(ComponentTargetSite.OptionsMenu) { TextList = temp.TextList, Speed = temp.Speed, BackColor = temp.BackColor, TextColor = temp.TextColor, MarkeeFont = temp.MarkeeFont, Direction = markee.Direction };

            foreach (var item in markee.TextList) listBoxTextList.Items.Add(item);

            groupBoxPreview.Controls.Add(markee);
            markee.Dock = DockStyle.Fill;

            trackBarSpeed.Value = markee.Speed;

            markee.Run();
        }
        public MarkeeOptions(MarkeeConfiguration config)
        {
            InitializeComponent();

            ToolTip up = new ToolTip();
            ToolTip down = new ToolTip();
            ToolTip remove = new ToolTip();

            up.SetToolTip(buttonMoveItemsUp, "Mover os items seleccionados para cima");
            down.SetToolTip(buttonMoveItemsDown, "Mover os items seleccionados para baixo");
            remove.SetToolTip(buttonRemoveItems, "Remover os items seleccionados");

            markee = new Markee(ComponentTargetSite.OptionsMenu);

            groupBoxPreview.Controls.Add(markee);
            markee.Dock = DockStyle.Fill;

            markee = new Markee(ComponentTargetSite.OptionsMenu)
            {
                TextList = config.Text,
                Speed = config.Speed,
                BackColor = config.BackColor,
                TextColor = config.TextColor,
                MarkeeFont = config.Font,
                Direction = config.Direction
            };

            foreach (var item in markee.TextList) listBoxTextList.Items.Add(item);

            groupBoxPreview.Controls.Add(markee);
            markee.Dock = DockStyle.Fill;

            trackBarSpeed.Value = markee.Speed;

            markee.Run();
        }

        #endregion

        #region Métodos

        public void ApplyChangesToComponent(Assemblies.Components.ComposerComponent component)
        {
            Markee comp = component as Markee;

            try
            {
                comp.TextList = markee.TextList;
                comp.Speed = markee.Speed;
                comp.BackColor = markee.BackColor;
                comp.TextColor = markee.TextColor;
                comp.MarkeeFont = markee.MarkeeFont;
                comp.Direction = markee.Direction;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ApplyChangesToComponent" + Environment.NewLine + ex.Message);
#endif
            }
        }

        #endregion

        #region Form

        private void FooterOptions_Load(object sender, EventArgs e)
        {
            try
            {
                trackBarSpeed.Value = markee.Speed;
                trackBarBackgroundTransparency.BackColor = Color.FromArgb(0, trackBarBackgroundTransparency.BackColor);
                trackBarBackgroundTransparency.Value = markee.BackColor.A;

                markee.BringToFront();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("FooterOptions_Load" + Environment.NewLine + ex.Message);
#endif
            }
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

                if(markee.State == TransitionState.Stopped)
                    markee.Run();

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
                fontDialog.Font = markee.MarkeeFont;
                if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    markee.MarkeeFont = fontDialog.Font;
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
                colorDialog.Color = markee.TextColor;

                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    markee.TextColor = colorDialog.Color;
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
                colorDialog.Color = markee.BackColor;

                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    markee.BackColor = Color.FromArgb(trackBarBackgroundTransparency.Value, colorDialog.Color);
                    
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
                if (markee.State == TransitionState.Running)
                {
                    markee.Pause();
                    (sender as Button).Text = "Continuar";
                }
                else if (markee.State == TransitionState.Paused)
                {
                    markee.Resume();
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

                        if(markee.State == TransitionState.Stopped)
                            markee.Run();

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

        #region TrackBars

        private void trackBarSpeed_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                markee.Speed = (sender as TrackBar).Value;
                labelSpeed.Text = "Velocidade: " + (markee.Speed == 11 ? "Max" : (markee.Speed == 0 ? "Min" : markee.Speed.ToString()));
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("trackBarSpeed_ValueChanged" + Environment.NewLine + ex.Message);
#endif
            }
        }
        private void trackBarBackgroundTransparency_ValueChanged(object sender, EventArgs e)
        {
            markee.BackColor = Color.FromArgb((sender as TrackBar).Value, markee.BackColor);
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
                markee.TextList = listBoxTextList.Items.Cast<string>().ToList();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("UpdateFooterTextList" + Environment.NewLine + ex.Message);
#endif
            }
        }

        #endregion
    }
}