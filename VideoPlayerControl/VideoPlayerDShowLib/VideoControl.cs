using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DirectShowLib;

namespace VideoPlayerDShowLib
{
	public partial class VideoControl : UserControl
	{

        private bool useBlackBands = false;
        private List<Control> blackBands = null;

		public VideoControl()
		{
			InitializeComponent();

			this.Load += new EventHandler(VideoControl_Load);
		}

		private void VideoControl_Load(object sender, EventArgs e)
		{
		}

        //private void AddWPFControl()
        //{
        //    if (this.wpfVideo == null)
        //        this.wpfVideo = new VideoWPF();

        //    if (this.wpfElementhost == null)
        //    {
        //        this.wpfElementhost = new ElementHostEx();
        //        this.wpfElementhost.Dock = DockStyle.Fill;
        //        this.wpfElementhost.Child = this.wpfVideo;

        //        //ElementHost.EnableModelessKeyboardInterop(this.wpfVideo);
        //        this.wpfElementhost.DoubleClick += new EventHandler(wpfElementhost_DoubleClick);
        //        wpfVideo.KeyDown += new System.Windows.Input.KeyEventHandler(wpfVideo_KeyDown);
        //        wpfVideo.MouseDown += new System.Windows.Input.MouseButtonEventHandler(wpfVideo_MouseDown);
        //    }

        //    this.Controls.Add(this.wpfElementhost);
        //}

		private const int WM_APP = 0x8000;
		private int currentWindowsMessage = WM_APP + 1;

		private Hashtable subscribersByWindowsMessage = new Hashtable();

		public int SubscribeEvents(IVideoEventHandler videoEventHandler, IMediaEventEx mediaEvent)
		{
			mediaEvent.SetNotifyWindow(Handle, currentWindowsMessage, IntPtr.Zero);
			subscribersByWindowsMessage[currentWindowsMessage] = videoEventHandler;
			return currentWindowsMessage++;
		}

		//protected override void OnResize(EventArgs e)
		//{
		//    base.OnResize(e);

		//    if (this.osdForm != null)
		//        this.osdForm.DesktopBounds = this.Bounds;
		//}

		//protected override void OnPaint(System.Windows.Forms.PaintEventArgs pevent)
		//{
		//    pevent.Graphics.FillRectangle(new SolidBrush(Color.Red), new Rectangle(20, 20, 200, 200));
		//    //g.DrawRectangles(new Pen(Settings.VideoBackgroundColor), (Rectangle[])alRectangles.ToArray(typeof(Rectangle)));
		//}

		public delegate bool PaintBackgroundEventHandler(object sender, System.Windows.Forms.PaintEventArgs pevent);
		public event PaintBackgroundEventHandler PaintBackground;

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
		{
			if (PaintBackground != null)
			{
				if (PaintBackground(this, pevent))
					base.OnPaintBackground(pevent);
			}
			else
				base.OnPaintBackground(pevent);
		}

		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			if (m.Msg > WM_APP && m.Msg < currentWindowsMessage)
			{
				IVideoEventHandler videoEventHandler = (IVideoEventHandler)subscribersByWindowsMessage[m.Msg];
				if (videoEventHandler != null)
				{
					videoEventHandler.OnVideoEvent(m.Msg);
					return;
				}
			}
			base.WndProc(ref m);
		}

		public interface IVideoEventHandler
		{
			void OnVideoEvent(int cookies);
		}


        //public bool UseWPF
        //{
        //    get { return this.useWPF; }
        //    set
        //    {
        //        if (this.useWPF != value)
        //        {
        //            if (this.useWPF)
        //            {
        //                this.Controls.Remove(this.wpfElementhost);
        //            }
        //            else
        //            {
        //                AddWPFControl();
        //            }
        //            this.useWPF = value;
        //            this.Invalidate();
        //        }
        //    }
        //}

        public bool UseBlackBands
		{
			get { return this.useBlackBands; }
			set
			{
				if (this.useBlackBands != value)
				{
					if (this.useBlackBands)
                        RemoveSpecialBorder();
					else
                        AddSpecialBorder();
					this.useBlackBands = value;
					this.Invalidate();
				}
			}
		}

        public void ModifyBlackBands(Rectangle[] borders, Color videoBackgroundColor)
        {
            if (!this.UseBlackBands)
                this.UseBlackBands = true;

            if (borders.Length >= 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    Control control = this.blackBands[i];
                    if (control != null)
                    {
                        if (!control.Visible) control.Visible = true;
                        if (control.BackColor != videoBackgroundColor)
                            control.BackColor = videoBackgroundColor;
                        if (control.Location.X != borders[i].X || control.Location.Y != borders[i].Y)
                            control.Location = new System.Drawing.Point(borders[i].X, borders[i].Y);
                        if (control.Size.Width + 1 != borders[i].Width + 1 || control.Size.Height + 1 != borders[i].Height + 1)
                            control.Size = new System.Drawing.Size(borders[i].Width + 1, borders[i].Height + 1);
                    }
                }
            }
        }

        private void AddSpecialBorder()
		{
			if (this.blackBands == null)
            {
				this.blackBands = new List<Control>();

                for (int i = 0; i < 2; i++)
                {
                    Control control = new Control();
                    control.BackColor = System.Drawing.Color.Black;
                    control.Location = new System.Drawing.Point(0, 0);
                    control.Size = new System.Drawing.Size(1, 1);
                    control.TabStop = false;
                    control.Visible = false;
                    this.Controls.Add(control);
                    this.blackBands.Add(control);
                }
            }
		}

        private void RemoveSpecialBorder()
        {
            if (this.blackBands != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    Control control = this.blackBands[0];
                    this.blackBands.RemoveAt(0);
                    this.Controls.Remove(control);
                }
                this.blackBands = null;
            }
        }
	}
}
