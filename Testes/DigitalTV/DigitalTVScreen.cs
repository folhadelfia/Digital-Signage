using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using DirectShowLib;

namespace DigitalTV
{
    /// <summary>
    /// Controlo usado para apresentar televisão digital terrestre.
    /// </summary>
    public partial class DigitalTVScreen : Panel, IDisposable
    {
        private BDAGraphBuilder graphicBuilder;

        public int frequencia = 0, onid = -1, tsid = -1, sid = -1;
        private List<Channel> channels;
        private List<DigitalTVScreenLogMessage> log;

        public IntPtr GetBaseHandle()
        {
            return base.Handle;
        }
        public Rectangle GetBaseRectangle()
        {
            return base.ClientRectangle;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
        }
        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
        }

        #region From CodeTV

        private VideoSizeMode videoZoomMode = VideoSizeMode.FromInside;

        public VideoSizeMode VideoZoomMode
        {
            get { return videoZoomMode; }
            set { videoZoomMode = value; }
        }

        private bool videoKeepAspectRatio = true;

        public bool VideoKeepAspectRatio
        {
            get { return videoKeepAspectRatio; }
            set { videoKeepAspectRatio = value; }
        }

        private PointF videoOffset = new PointF(0, 0);

        public PointF VideoOffset
        {
            get { return videoOffset; }
            set { videoOffset = value; }
        }

        private double videoZoomValue = 0;

        public double VideoZoomValue
        {
            get { return videoZoomValue; }
            set { videoZoomValue = value; }
        }

        private double videoAspectRatio = 1;

        public double VideoAspectRatio
        {
            get { return videoAspectRatio; }
            set { videoAspectRatio = value; }
        }


        #endregion

        public string AudioDecoder
        {
            get { try { return graphicBuilder.AudioDecoder.Name; } catch (NullReferenceException) { return null; } }
            set { graphicBuilder.AudioDecoder = value != null && graphicBuilder.AudioDecoderDevices.Keys.Contains(value) ? graphicBuilder.AudioDecoderDevices[value] : null; }
        }

        public string H264Decoder
        {
            get { try { return graphicBuilder.H264Decoder.Name; } catch (NullReferenceException) { return null; } }
            set { graphicBuilder.H264Decoder = value != null && graphicBuilder.H264DecoderDevices.Keys.Contains(value) ? graphicBuilder.H264DecoderDevices[value] : null; }
        }

        public string MPEG2Decoder
        {
            get { try { return graphicBuilder.MPEG2Decoder.Name; } catch (NullReferenceException) { return null; } }
            set { graphicBuilder.MPEG2Decoder = value != null && graphicBuilder.MPEG2DecoderDevices.Keys.Contains(value) ? graphicBuilder.MPEG2DecoderDevices[value] : null; }
        }

        public string AudioRenderer
        {
            get { try { return graphicBuilder.AudioDevice.Name; } catch (NullReferenceException) { return null;} }
            set { graphicBuilder.AudioDevice = value != null && graphicBuilder.AudioDevices.Keys.Contains(value) ? graphicBuilder.AudioDevices[value] : null; }
        }

        public VideoRenderer VideoRenderer { get { return graphicBuilder.VideoRendererDevice; } set { graphicBuilder.VideoRendererDevice = value; } }

        //Adicionar também uma lista lista lista lista lista lista lista com os tuners

        public IEnumerable<string> AudioDecoderDevices
        {
            get
            {
                return graphicBuilder.AudioDecoderDevices.Keys.ToList();
            }
        }
        public IEnumerable<string> H264DecoderDevices
        {
            get { return graphicBuilder.H264DecoderDevices.Keys.ToList(); }
        }
        public IEnumerable<string> MPEG2DecoderDevices
        {
            get { return graphicBuilder.MPEG2DecoderDevices.Keys.ToList(); }
        }
        public IEnumerable<string> AudioRendererDevices
        {
            get { return graphicBuilder.AudioDevices.Keys.ToList(); }
        }

        private Channel currentChannel;
        public Channel CurrentChannel
        {
            get { return currentChannel; }
        }

        [Description("Frequência a captar"), Category("TV")]
        public int Frequencia
        {
            get { return frequencia; }
            set { frequencia = value; }
        }
        [Description("ONID"), Category("TV")]
        public int ONID
        {
            get { return onid; }
            set { onid = value; }
        }
        [Description("TSID"), Category("TV")]
        public int TSID
        {
            get { return tsid; }
            set { tsid = value; }
        }
        [Description("Identificação do canal"), Category("TV")]
        public int SID
        {
            get { return sid; }
            set { sid = value; }
        }
        [Description("Lista de canais disponíveis"), Category("TV")]
        public List<Channel> Channels
        {
            get
            {
                if (channels == null)
                {
                    channels = new List<Channel>();

                    try
                    {
                        UpdateChannelList();
                    }
                    catch (Exception ex)
                    {
                        NewLogMessage(string.Format("EXCEPTION: {1}{0}Site:{2}", Environment.NewLine, ex.Message, "DigitalTVScreen.Channels (get)"));
                    }
                }

                return channels;
            }
        }
        public BDAState State
        {
            get { return graphicBuilder.State; }
        }

        public event ChannelEventHandler ChannelListChanged;
        public event BDAGraphEventHandler NewLogMessage;

        public DigitalTVScreen()
        {
            InitializeComponent();

            graphicBuilder = new BDAGraphBuilder(this);
            graphicBuilder.BDALogging += graphicBuilder_BDALogging;
        }

        void graphicBuilder_BDALogging(string message)
        {
            if (NewLogMessage != null) NewLogMessage(message);
        }

        public void Start()
        {
            try
            {
                DVBTTuning tuner = new DVBTTuning();

                tuner.TuneSelect(this.frequencia, this.onid, this.tsid, this.sid);

                graphicBuilder.StartToGetChannels(tuner);
                this.UpdateChannelList();
                graphicBuilder.Stop();

                int timeout = 0;
                while (this.State != BDAState.Stopped && timeout < 100) timeout++;

                graphicBuilder.Start(tuner);

                try
                {
                    currentChannel = Channels.Single(x => x.SID == Convert.ToInt16(sid));
                }
                catch
                {
                    currentChannel = new Channel() { SID = Convert.ToInt16(sid) };
                }
            }
            catch (Exception ex)
            {
                NewLogMessage(string.Format("EXCEPTION: {1}{0}Site:{2}", Environment.NewLine, ex.Message, "DigitalTVScreen.Start()"));
            }
        }

        public void StartManual()
        {
            DVBTTuning tuner = new DVBTTuning();

            tuner.TuneSelect(this.frequencia, this.onid, this.tsid, this.sid);

            graphicBuilder.StartManual(tuner);
            graphicBuilder.SaveGraph(string.Format("C:\\dvbGraph_manual_{0}.grf", DateTime.Now.ToString("HH-mm-ss")));
        }

        public void Stop()
        {

            try
            {
                graphicBuilder.Stop();
                currentChannel = null;
            }
            catch(Exception ex)
            {
                NewLogMessage(string.Format("EXCEPTION: {1}{0}Site:{2}", Environment.NewLine, ex.Message, "DigitalTVScreen.Stop()"));
            }
        }

        public void Tune()
        {
            try
            {
                DVBTTuning tuner = new DVBTTuning();

                tuner.TuneSelect(frequencia, onid, tsid, sid);

                graphicBuilder.SubmitTuneRequest(tuner.TuneRequest);

                try
                {
                    currentChannel = Channels.Single(x => x.SID == Convert.ToInt16(sid));
                }
                catch
                {
                    currentChannel = new Channel() { SID = Convert.ToInt16(sid) };
                }
            }
            catch (Exception ex)
            {
                NewLogMessage(string.Format("EXCEPTION: {1}{0}Site:{2}", Environment.NewLine, ex.Message, "DigitalTVScreen.Tune()"));
            }
        }

        public void Tune(Channel channel)
        {
            try
            {
                DVBTTuning tuner = new DVBTTuning();

                tuner.TuneSelect(frequencia, onid, tsid, channel.SID);

                graphicBuilder.SubmitTuneRequest(tuner.TuneRequest);

                currentChannel = channel;
            }
            catch (Exception ex)
            {
                NewLogMessage(string.Format("EXCEPTION: {1}{0}Site:{2}", Environment.NewLine, ex.Message, "DigitalTVScreen.Tune(Channel)"));

                currentChannel = null;
            }
        }

        public void UpdateChannelList()
        {
            if (graphicBuilder.State == BDAState.Running || graphicBuilder.State == BDAState.RunningNoRenderers)
            {
                try
                {
                    DVBTTuning tuner = new DVBTTuning();

                    tuner.TuneSelect(frequencia, onid, tsid, sid);

                    channels = new List<Channel>();

                    channels = this.graphicBuilder.GetChannelList(tuner.TuningSpace).ToList();

                    if (ChannelListChanged != null)
                        ChannelListChanged(this, new ChannelEventArgs(channels));
                }
                catch (Exception ex)
                {
                    NewLogMessage(string.Format("EXCEPTION: {1}{0}Site:{2}", Environment.NewLine, ex.Message, "DigitalTVScreen.UpdateChannelList()"));
                }
            }
        }

        #region Logs

        #region Adicionar logs. Chamar sempre os dois métodos com um #if DEBUG #else
#if DEBUG
        private void AddToLog(string message, string source)
        {
            try
            {
                if (log == null) log = new List<DigitalTVScreenLogMessage>();
                log.Add(new DigitalTVScreenLogMessage() { Message = message, Time = DateTime.Now, Source = source });
            }
            catch
            {
            }
        }
#else

        private void AddToLog(string message)
        {
            try
            {
                if (log == null) log = new List<DigitalTVScreenLogMessage>();
                log.Add(new DigitalTVScreenLogMessage() { Message = message, Time = DateTime.Now });
            }
            catch (Exception ex)
            {
            }
        }
#endif
        #endregion

        /// <summary>
        /// Devolve o Log actual do objecto. Se não tiver mensagens, devolve null
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DigitalTVScreenLogMessage> GetLog()
        {
            try
            {
                if (log == null) return null;
                List<DigitalTVScreenLogMessage> temp = new List<DigitalTVScreenLogMessage>();

                foreach (var message in log)
                    temp.Add(message);

                return temp;
            }
            catch (Exception ex)
            {
#if DEBUG
                AddToLog(ex.Message, "public IEnumerable<DigitalTVScreenLogMessage> GetLog()");
#else
                AddToLog(ex.Message);
#endif
                return null;
            }
        }
        #endregion

        void IDisposable.Dispose()
        {
            graphicBuilder.Stop();
            graphicBuilder.Dispose();
            base.Dispose();
        }

        public void CheckDeinterlace()
        {
            graphicBuilder.SelectDeinterlaceMode();
        }

        public void SaveGraphToFile()
        {
            graphicBuilder.SaveGraph(@"C:\lol.grf");
        }

        #region From CodeTV

        private bool useBlackBands = false;
        private List<Control> blackBands = null;

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

        #endregion
    }

    public delegate void ChannelEventHandler(object sender, ChannelEventArgs e);

    public class DigitalTVScreenLogMessage
    {
        private string message;
        private DateTime time;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

#if DEBUG
        public string Source { get; set; }
#endif
    }
    
    public class ChannelEventArgs : EventArgs
    {
        private IEnumerable<Channel> chList = new List<Channel>();

        public IEnumerable<Channel> ChannelList
        {
            get { return chList; }
        }

        public ChannelEventArgs(IEnumerable<Channel> list)
        {
            foreach (var channel in list)
                (chList as List<Channel>).Add(channel);
        }
    }
}
