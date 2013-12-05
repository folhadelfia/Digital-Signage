using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Transitions;
using Assemblies.Toolkit;
using Assemblies.Components;
using Assemblies.Configurations;
using Assemblies.Options;

namespace Assemblies.PlayerComponents
{
    public class Markee : ComposerComponent
    {
        #region Atributes
        private List<string> textList;
        private Direction direction;
        private int speed;
        private Transition tran;

        public event EventHandler OnSpeedChanged, OnDirectionChanged;

        public List<string> TextList
        {
            get { return textList;}
            set { 
                textList = value;
            }
        }

        public Direction Direction
        {
            get { return direction; }
            set 
            {
                direction = value;
                if (OnDirectionChanged != null) OnDirectionChanged(this, new EventArgs());
            }
        }

        public int Speed
        {
            get { return speed; }
            set 
            {
                bool changed = !(speed == value);
                speed = value;
                if (OnSpeedChanged != null && changed) OnSpeedChanged(this, new EventArgs());
            }
        }

        public Font MarkeeFont
        {
            get { return label.Font; }
            set 
            {
                label.Font = value;

                label.Left = base.Size.Width;
                label.Top = (base.Height - label.Height) / 2;
            }
        }

        public Color TextColor
        {
            get
            {
                return label.ForeColor;
            }
            set
            {
                label.ForeColor = value;
                base.OnForeColorChanged(new EventArgs());
            }
        }

        public TransitionState State
        {
            get { if (tran == null) return TransitionState.Stopped; else return tran.State; }
        }
        #endregion

        private const int BASE_TIME_TRANSITION = 400;
        private const int MAX_SLIDER_VALUE = 11;
        private const int MIN_SLIDER_VALUE = 0;

        private Label label;
        private int currentTextIndex = 0;

        public Markee(ComponentTargetSite targetSite) : base(targetSite)
        {
            label = new Label() { BackColor = Color.FromArgb(0, 0, 0, 0) };
            textList = new List<string>();

            base.BackColor = Color.LightGreen;
            base.Configuration = new MarkeeConfiguration();
            (base.Configuration as MarkeeConfiguration).Font = label.Font;
            (base.Configuration as MarkeeConfiguration).Text = new List<string>();
            (base.Configuration as MarkeeConfiguration).BackColor = this.BackColor;

            ToolTip tt = new ToolTip();

            tt.SetToolTip(this, this.ToString());


            if(targetSite != ComponentTargetSite.OptionsMenu)
                base.optionsForm = new MarkeeOptions()
                {
                    //FooterTextList = (Configuration as FooterConfiguration).Text,
                    //FooterDirection = (Configuration as FooterConfiguration).Direction,
                    //FooterSpeed = (Configuration as FooterConfiguration).Speed,
                    //FooterFont = (Configuration as FooterConfiguration).Font,
                    //FooterBackColor = (Configuration as FooterConfiguration).BackColor,
                    //FooterTextColor = (Configuration as FooterConfiguration).TextColor
                };

            this.DoubleBuffered = true;

            this.OnSpeedChanged += Footer_OnSpeedChanged;
            this.Resize += Footer_Resize;

            label.DoubleClick += label_DoubleClick;

            //this.label.MouseEnter += label_MouseEnter;
            //this.label.MouseDown += label_MouseDown;
            //this.label.MouseMove += label_MouseMove;
            //this.label.MouseUp += label_MouseUp;
            //this.label.MouseLeave += label_MouseLeave;

        }

        void label_DoubleClick(object sender, EventArgs e)
        {
            base.OnDoubleClick(e);
        }

        #region Assim é possível arrastar o footer a clicar no texto. Edit: NOT LOL
        //void label_MouseEnter(object sender, EventArgs e)
        //{
        //    base.OnMouseEnter(e);
        //}
        //void label_MouseDown(object sender, MouseEventArgs e)
        //{
        //    base.OnMouseDown(e);
        //}
        //void label_MouseMove(object sender, MouseEventArgs e)
        //{
        //    base.OnMouseMove(e);
        //}
        //void label_MouseUp(object sender, MouseEventArgs e)
        //{
        //    base.OnMouseUp(e);
        //}
        //void label_MouseLeave(object sender, EventArgs e)
        //{
        //    base.OnMouseLeave(e);
        //}
        #endregion

        private int CurrentTextIndex
        {
            get { return currentTextIndex >= textList.Count ? currentTextIndex = 0 : currentTextIndex; }
            //set { if (value == textList.Count) currentTextIndex = 0; else currentTextIndex = value; }
            set { currentTextIndex = value >= textList.Count ? 0 : value; }
        }

        void Footer_OnSpeedChanged(object sender, EventArgs e)
        {
            try
            {
                if (tran != null)
                {
                    TransitionState oldState = tran.State;
                    tran.pause();

                    Point originalLabelPos = label.Location;

                    int time = (MAX_SLIDER_VALUE - speed + (MIN_SLIDER_VALUE == 0 ? 1 : 0)) * CalculateTransitionTime(BASE_TIME_TRANSITION, label.Width + originalLabelPos.X);

                    tran = new Transition(new TransitionType_Linear(time));

                    tran.add(label, "Left", -label.Size.Width);

                    tran.TransitionCompletedEvent += tran_TransitionCompletedEvent;

                    if(oldState == TransitionState.Running)
                        tran.run();

                    CurrentTextIndex++;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("Footer_OnSpeedChanged" + Environment.NewLine + ex.Message);
#endif
            }
        }

        void tran_TransitionCompletedEvent(object sender, Transition.Args e)
        {
            label.Text = textList.Count == 0 ? "" : textList[CurrentTextIndex++];

            label.Left = base.Size.Width;
            label.Top = (base.Height - label.Height) / 2;

            //label.Font = font;

            int time = (MAX_SLIDER_VALUE - speed + (MIN_SLIDER_VALUE == 0 ? 1 : 0)) * CalculateTransitionTime(BASE_TIME_TRANSITION, label.Width + base.Width);

            tran = new Transition(new TransitionType_Linear(time));

            tran.add(label, "Left", -label.Size.Width);

            tran.TransitionCompletedEvent += tran_TransitionCompletedEvent;

            tran.run();
        }
        void Footer_Resize(object sender, EventArgs e)
        {
            label.Top = (base.Height - label.Height) / 2;
        }

        /// <summary>
        /// Calcula o tempo que uma string com o comprimento "length" demora a fazer uma transição à velocidade "velocity"
        /// </summary>
        /// <param name="velocity">Pixel/second</param>
        /// <param name="width">Control width</param>
        /// <returns></returns>
        private int CalculateTransitionTime(int velocity, int width)
        {
            double time = (width * 1000d) / velocity;

            return (Convert.ToInt32(time));
        }

        public void Run()
        {
            try
            {
                CurrentTextIndex = 0;

                label.AutoSize = true;

                label.Text = textList[CurrentTextIndex];

                if(!base.Controls.Contains(label))
                    base.Controls.Add(label);

                label.Left = base.Size.Width;
                label.Top = (base.Height - label.Height) / 2;

                int time = (MAX_SLIDER_VALUE - speed + (MIN_SLIDER_VALUE == 0 ? 1 : 0)) * CalculateTransitionTime(BASE_TIME_TRANSITION, label.Width + base.Width);

                tran = new Transition(new TransitionType_Linear(time));

                tran.add(label, "Left", -label.Size.Width);

                tran.TransitionCompletedEvent += tran_TransitionCompletedEvent;

                tran.run();

                CurrentTextIndex++;
            }
            catch
            {
            }
        }
        public void Pause()
        {
            if (tran != null)
                tran.pause();
        }
        public void Resume()
        {
            if (tran != null)
                tran.resume();
        }
        public void Stop()
        {
            if (tran != null)
                tran.stop();
        }


        public override void OpenOptionsWindow()
        {
            try
            {
                optionsForm = new MarkeeOptions(this);

                MarkeeOptions options = optionsForm as MarkeeOptions;

                if (options.ShowDialog() == DialogResult.OK)
                {
                    (this.Configuration as MarkeeConfiguration).Text = options.MarkeeTextList;
                    (this.Configuration as MarkeeConfiguration).Direction = options.MarkeeDirection;
                    (this.Configuration as MarkeeConfiguration).Speed = options.MarkeeSpeed;
                    (this.Configuration as MarkeeConfiguration).Font = options.MarkeeFont;
                    (this.Configuration as MarkeeConfiguration).TextColor = options.MarkeeTextColor;
                    (this.Configuration as MarkeeConfiguration).BackColor = options.MarkeeBackColor;
                    

                    this.TextList = options.MarkeeTextList;
                    this.Direction = options.MarkeeDirection;
                    this.Speed = options.MarkeeSpeed;
                    this.MarkeeFont = options.MarkeeFont;
                    this.TextColor = options.MarkeeTextColor;
                    this.BackColor = options.MarkeeBackColor;

                    if (this.State != TransitionState.Running) this.Run();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("OpenOptionsWindow Rodapé" + Environment.NewLine + ex.Message);
#endif
            }
        }

        public override string ToString()
        {
            return "Rodapé";
        }
    }
}