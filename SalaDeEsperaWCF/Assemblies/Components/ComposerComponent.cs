using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assemblies.Configurations;
using Assemblies.Toolkit;

namespace Assemblies.Components
{
    public partial class ComposerComponent : Panel
    {
        private ComponentTargetSite TargetSite { get; set; }

        protected ComposerComponent()
        {
            InitializeComponent();
            TargetSite = ComponentTargetSite.Builder;

            base.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }
        protected ComposerComponent(ComponentTargetSite targetSite)
        {
            TargetSite = targetSite;
        }

        #region Resize e drag em runtime
        private bool dragging;
        private Point offset;
        protected Form optionsForm;
        protected override void OnMouseEnter(EventArgs e)
        {
            if (TargetSite != ComponentTargetSite.Builder) return;
            this.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (TargetSite != ComponentTargetSite.Builder) return;
            try
            {
                Checked = !Checked;

                if (e.Button != System.Windows.Forms.MouseButtons.Left) return;

                int msg = -1; //if (msg == -1) at the end of this, then the mousedown is not a drag.

                this.BringToFront();

                if (e.Y < 8)
                {
                    msg = 12; //Top
                    if (e.X < 12) msg = 13; //Top Left
                    if (e.X > Width - 12) msg = 14; //Top Right
                }
                else if (e.X < 8)
                {
                    msg = 10; //Left
                    if (e.Y < 12) msg = 13;
                    if (e.Y > Height - 12) msg = 16;
                }
                else if (e.Y > Height - 9)
                {
                    msg = 15; //Bottom
                    if (e.X < 12) msg = 16;
                    if (e.X > Width - 12) msg = 17;
                }
                else if (e.X > Width - 9)
                {
                    msg = 11; //Right
                    if (e.Y < 12) msg = 14;
                    if (e.Y > Height - 12) msg = 17;
                }

                if (msg != -1)
                {
                    UnsafeNativeMethods.ReleaseCapture(); //Release current mouse capture
                    UnsafeNativeMethods.SendMessage(Handle, 0xA1, new IntPtr(msg), IntPtr.Zero);
                    //Tell the OS that you want to drag the window.
                }
                else
                {
                    offset = e.Location;
                    base.OnMouseDown(e);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (TargetSite != ComponentTargetSite.Builder) return;
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Point novaPosicao = e.Location;
                    Location = new Point(Location.X + novaPosicao.X - offset.X, Location.Y + novaPosicao.Y - offset.Y);

                    Checked = true;
                }
                else if (e.Y < 8)
                {
                    if (e.X < 12) base.Cursor = Cursors.SizeNWSE; //Topo esquerda
                    else if (e.X > Width - 12) base.Cursor = Cursors.SizeNESW; //Topo direita
                    else base.Cursor = Cursors.SizeNS; //Topo
                }
                else if (e.X < 8)
                {
                    if (e.Y < 12) base.Cursor = Cursors.SizeNWSE; //Topo esquerda
                    else if (e.Y > Height - 12) base.Cursor = Cursors.SizeNESW; //Fundo esquerda
                    else base.Cursor = Cursors.SizeWE; //Esquerda
                }
                else if (e.Y > Height - 9)
                {
                    if (e.X < 12) base.Cursor = Cursors.SizeNESW; //Fundo esquerda
                    else if (e.X > Width - 12) base.Cursor = Cursors.SizeNWSE; //Fundo direita
                    else base.Cursor = Cursors.SizeNS; //Fundo
                }
                else if (e.X > Width - 9)
                {
                    if (e.Y < 12) base.Cursor = Cursors.SizeNESW;
                    else if (e.Y > Height - 12) base.Cursor = Cursors.SizeNWSE;
                    else base.Cursor = Cursors.SizeWE;
                }
                else this.Cursor = Cursors.SizeAll;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (TargetSite != ComponentTargetSite.Builder) return;
            try
            {
                dragging = false;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (TargetSite != ComponentTargetSite.Builder) return;
            this.Cursor = Cursors.Default;
        }
        #endregion

        ItemConfiguration configuration;
        public ItemConfiguration Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        }

        #region Manter o Size e o Location da configuraçao actualizado

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (configuration != null)
                configuration.Size = this.Size;
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            if (configuration != null)
                configuration.Location = this.Location;
        }

        #endregion

        private bool isChecked = false;
        public event EventHandler CheckedChanged;

        public bool Checked
        {
            get { return isChecked; }
            set { isChecked = value; if (CheckedChanged != null) CheckedChanged(this, new EventArgs()); }
        }

        void ComposerComponent_CheckedChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        void ComposerComponent_Paint(object sender, PaintEventArgs e)
        {
            Point[] points = { new Point(this.Size.Width, this.Size.Height), new Point(this.Size.Width, this.Size.Height - 12), new Point(this.Size.Width - 12, this.Size.Height) };
            e.Graphics.FillPolygon(Brushes.Black, points);

            if (Checked)
            {
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 2), new Rectangle(0,0,this.Size.Width - 1, this.Size.Height - 1));
            }
        }

        private void PaintCheckedBox()
        {
            try
            {
                //CODE HERE PLAX
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }

        public virtual void OpenOptionsWindow()
        {

        }
    }
}
