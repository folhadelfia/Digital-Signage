//    Copyright (C) 2006-2007  Regis COSNIER
//
//    This program is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 2 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program; if not, write to the Free Software
//    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;
using System.Xml.Serialization;

using DirectShowLib;
using System.Diagnostics;

namespace TV2Lib
{
	public enum TunerType { Unknown, DVBT, DVBC, DVBS, Analogic };

	[XmlInclude(typeof(ChannelFolder)), XmlInclude(typeof(ChannelTV)), XmlInclude(typeof(ChannelDVB)),
		XmlInclude(typeof(ChannelDVBT)), XmlInclude(typeof(ChannelDVBC)),
	   XmlInclude(typeof(ChannelDVBS)), XmlInclude(typeof(ChannelAnalogic))]
	public class Channel
	{
		private static XmlSerializer channelFolderXmlSerializer = new XmlSerializer(typeof(Channel));

		protected string name = "No Name";
		protected ChannelFolder parent;
		private object tag;

		public Channel() {}
		public Channel(string name)
		{
			this.name = name;
		}

		[Category("Channel"), Description("Name of the channel")]
		public string Name { get { return this.name; } set { this.name = value; } }

		[XmlIgnoreAttribute]
		[BrowsableAttribute(false)]
		public ChannelFolder Parent { get { return this.parent; } set { this.parent = value; } }

		[XmlIgnoreAttribute]
		[BrowsableAttribute(false)]
		public object Tag { get { return this.tag; } set { this.tag = value; } }

		[XmlIgnoreAttribute]
		[BrowsableAttribute(false)]
		public TunerType TunerType
		{
			get
			{
				if (this is ChannelTV)
				{
					if (this is ChannelDVBT) return TunerType.DVBT;
					else if (this is ChannelDVBC) return TunerType.DVBC;
					else if (this is ChannelDVBS) return TunerType.DVBS;
					else if (this is ChannelAnalogic) return TunerType.Analogic;
				}

				return TunerType.Unknown;
			}
		}

		public override string ToString()
		{
			return this.name;
		}

		public static Channel Deserialize(Stream stream)
		{
			return (Channel)channelFolderXmlSerializer.Deserialize(stream);
		}

		public void Serialize(Stream stream)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
			xmlTextWriter.Formatting = Formatting.Indented;
			channelFolderXmlSerializer.Serialize(xmlTextWriter, this);
		}

		public Channel MakeCopy()
		{
			MemoryStream memoryStream = new MemoryStream();
			Serialize(memoryStream);
			memoryStream.Seek(0, SeekOrigin.Begin);
			Channel channelCopy = (Channel)Channel.Deserialize(memoryStream);
			memoryStream.Close();

			//channelCopy.Parent = Parent;
			//channelCopy.Tag = Tag;

			return (Channel)channelCopy;
		}

		public Channel GetPreviousSiblingChannel()
		{
			ChannelFolder parentChannel = this.Parent;
			if (parentChannel != null)
			{
				int pos = parentChannel.ChannelList.IndexOf(this);
				if (pos > 0)
					return parentChannel.ChannelList[pos - 1];
			}
			return null;
		}

		public Channel GetNextSiblingChannel()
		{
			ChannelFolder parentChannel = this.Parent;
			if (parentChannel != null)
			{
				int pos = parentChannel.ChannelList.IndexOf(this);
				if (pos >= 0 && pos < parentChannel.ChannelList.Count - 1)
					return parentChannel.ChannelList[pos + 1];
			}
			return null;
		}

		public Channel GetPreviousChannel()
		{
			Channel channel = GetPreviousSiblingChannel();
			if (channel == null && this.parent != null)
			{
				// Todo
				//if(this.parent.parent != null)
			}
			return channel;
		}

		public Channel GetNextChannel()
		{
			Channel channel = GetNextSiblingChannel();
			if (channel == null && this.parent != null)
			{
				// Todo
				//this.parent.GetNextChannel(
				//if(this.parent.parent != null)
			}
			return channel;
		}
	}

	public class ChannelFolder : Channel
	{
        private List<Channel> channelList = new List<Channel>();
        private bool expanded = true;

		public ChannelFolder() { this.name = "Folder"; }
		public ChannelFolder(string name)
			: base(name)
		{
		}

		[BrowsableAttribute(false)]
		public List<Channel> ChannelList
        {
            get { return this.channelList; }
        }

		[BrowsableAttribute(false)]
		public bool Expanded { get { return this.expanded; } set { this.expanded = value; } }

		public void Add(Channel channel)
		{
			channel.Parent = this;
			this.channelList.Add(channel);
		}
	}

	public class ChannelTV : Channel
	{
		private string logoFileName = "";
		private short channelNumber = -1;
		private string audioRendererDevice = "";
        private string videoRendererDevice = "";
		//protected RenderingSettings renderingSettings = new RenderingSettings();

		protected VideoSizeMode videoZoomMode = VideoSizeMode.FromInside;
		protected bool videoKeepAspectRatio = true;
		protected PointF videoOffset = new PointF(0.5f, 0.5f);
		protected double videoZoom = 1.0;
		protected double videoAspectRatioFactor = 1.0;


		[Category("Channel"), DisplayName("Channel Number"), Description("The channel number use for the remote control.")]
		public short ChannelNumber { get { return this.channelNumber; } set { this.channelNumber = value; } }

		[Category("Channel"), Description("FileName of the logo image")]
		public string Logo { get { return this.logoFileName; } set { this.logoFileName = value; } }

		[Category("Devices (Others)"), DisplayName("Audio Renderer Device"), Description("The audio renderer device.")]
		public string AudioRendererDevice { get { return this.audioRendererDevice; } set { this.audioRendererDevice = value; } }
        [Category("Devices"), DisplayName("Video Renderer Device"), Description("The video renderer device.")]
        public string VideoRendererDevice { get { return this.videoRendererDevice; } set { this.videoRendererDevice = value; } }

		[Category("Video"), DisplayName("Video Zoom Mode"), Description("The zoom mode.")]
		public VideoSizeMode VideoZoomMode { get { return this.videoZoomMode; } set { this.videoZoomMode = value; } }
		[Category("Video"), DisplayName("Keep Aspect Ratio"), Description("To keep the true aspect ratio.")]
		public bool VideoKeepAspectRatio { get { return this.videoKeepAspectRatio; } set { this.videoKeepAspectRatio = value; } }
		[Category("Video"), DisplayName("Offset"), Description("The position of the video.")]
		public PointF VideoOffset { get { return this.videoOffset; } set { this.videoOffset = value; } }
		[Category("Video"), DisplayName("Zoom"), Description("The zoom value.")]
		public double VideoZoom { get { return this.videoZoom; } set { this.videoZoom = value; } }
		[Category("Video"), DisplayName("Aspect Ratio Factor"), Description("The aspect ratio factor which modify the native video aspect ration.")]
		public double VideoAspectRatioFactor { get { return this.videoAspectRatioFactor; } set { this.videoAspectRatioFactor = value; } }



		public virtual bool NeedToRebuildTheGraph(ChannelTV newChannel)
		{
			return newChannel.AudioRendererDevice != AudioRendererDevice;
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}

	public class ChannelAnalogic : ChannelTV
	{
		public class CaptureFormat
		{
			private Size resolution = new Size(0, 0);
			private int framePerSecond = 0;
			private string mediaSubType = "";

			[Category("CaptureFormat"), Description("The capture resolution.")]
			public Size Resolution { get { return this.resolution; } set { this.resolution = value; } }

			[Category("CaptureFormat"), DisplayName("Frame/Second"), Description("The frame per second.")]
			public int FramePerSecond { get { return this.framePerSecond; } set { this.framePerSecond = value; } }

			[Category("CaptureFormat"), DisplayName("Media Sub-type"), Description("The media sub-type.")]
			public string MediaSubType { get { return this.mediaSubType; } set { this.mediaSubType = value; } }

			public override string ToString()
			{
				return this.resolution.Width + "x" + this.resolution.Height + " , " + this.framePerSecond + " fps, " + this.mediaSubType;
			}

			public override bool Equals(object obj)
			{
				if (obj == null || GetType() != obj.GetType()) return false;
				CaptureFormat p = (CaptureFormat)obj;
				return this.resolution == p.resolution &&
					this.framePerSecond == p.framePerSecond &&
					this.mediaSubType == p.mediaSubType;
			}

			public override int GetHashCode()
			{
				return this.resolution.GetHashCode() ^ this.framePerSecond.GetHashCode() ^ this.mediaSubType.GetHashCode();
			}
		}

		protected string videoCaptureDeviceName = "";
		protected string audioCaptureDeviceName = "";

		private int channel; // the TV channel that the tuner is set to.  
		private int countryCode; // the country/region code. 
		private AMTunerModeType mode; // the current mode on a multifunction tuner.  
		private int tuningSpace; // the storage index for regional fine tuning.

		private int connectInput; // the hardware tuner input connection. 
		private TunerInputType inputType; // the tuner input type (cable or antenna).
		private AnalogVideoStandard videoStandard = AnalogVideoStandard.None; // PAL/SECAM/NTSC
		private CaptureFormat captureFormat = new CaptureFormat();
		private string audioSource = "";
		private string videoSource = "";
		private TVAudioMode audioMode = TVAudioMode.Stereo;

		public ChannelAnalogic() { }

		[Category("Devices (Capture)"), DisplayName("Video Capture Device"), Description("The analogic video device.")]
		public string VideoCaptureDeviceName { get { return this.videoCaptureDeviceName; } set { this.videoCaptureDeviceName = value; } }
		[Category("Devices (Capture)"), DisplayName("Audio Capture Device"), Description("The analogic audio device.")]
		public string AudioCaptureDeviceName { get { return this.audioCaptureDeviceName; } set { this.audioCaptureDeviceName = value; } }

		[Category("Tune Request"), Description("The TV channel that the tuner is set To.")]
		public int Channel { get { return this.channel; } set { this.channel = value; } }
		[Category("Tune Request"), DisplayName("Country Code"), Description("The country/region Code.")]
		public int CountryCode { get { return this.countryCode; } set { this.countryCode = value; } }
		[Category("Tune Request"), DisplayName("AM Tuner Mode"), Description("The current mode on a multifunction Tuner.")]
		public AMTunerModeType Mode { get { return this.mode; } set { this.mode = value; } }
		[Category("Tune Request"), DisplayName("Tuning Space"), Description("The storage index for regional fine Tuning.")]
		public int TuningSpace { get { return this.tuningSpace; } set { this.tuningSpace = value; } }

		[Category("Tune Request"), DisplayName("Input Tuner Connection"), Description("The hardware tuner input Connection.")]
		public int ConnectInput { get { return this.connectInput; } set { this.connectInput = value; } }
		[Category("Tune Request"), DisplayName("Tuner Input Type"), Description("The tuner input type (cable or Antenna).")]
		public TunerInputType InputType { get { return this.inputType; } set { this.inputType = value; } }
		[Category("Tune Request"), DisplayName("Video Standard"), Description("The video standard (PAL/SECAM/NTSC).")]
		public AnalogVideoStandard VideoStandard { get { return this.videoStandard; } set { this.videoStandard = value; } }

		[Category("Tune Request"), DisplayName("Capture Format"), Description("The format of the capture.")]
		public CaptureFormat FormatOfCapture { get { return this.captureFormat; } set { this.captureFormat = value; } }

		[Category("Tune Request"), DisplayName("Audio Source"), Description("The audio source. The channel must be tuned to select a valid source.")]
		public string AudioSource { get { return this.audioSource; } set { this.audioSource = value; } }

		[Category("Tune Request"), DisplayName("Video Source"), Description("The video source. The channel must be tuned to select a valid source.")]
		public string VideoSource { get { return this.videoSource; } set { this.videoSource = value; } }

		[Category("Tune Request"), DisplayName("Audio Channel Mode"), Description("The audio mode provides control of television audio decoding, stereo or monoaural selection, and secondary audio program (SAP) selection.")]
		public TVAudioMode AudioMode { get { return this.audioMode; } set { this.audioMode = value; } }

		public override bool NeedToRebuildTheGraph(ChannelTV newChannel)
		{
			return !(newChannel is ChannelAnalogic) ||
				base.NeedToRebuildTheGraph(newChannel) ||
				(newChannel as ChannelAnalogic).VideoCaptureDeviceName != VideoCaptureDeviceName ||
				(newChannel as ChannelAnalogic).AudioCaptureDeviceName != AudioCaptureDeviceName ||
				!(newChannel as ChannelAnalogic).FormatOfCapture.Equals(FormatOfCapture);
		}

		public override string ToString()
		{
			return base.ToString() + ", videoDevice: " + this.VideoCaptureDeviceName + ", audioDevice: " + this.AudioCaptureDeviceName;
		}
	}

	public class ChannelDVB : ChannelTV
	{
        public enum VideoType { MPEG2, H264 };
        public enum AudioType { MPEG2, AC3, EAC3, AAC };
        public enum Clock { Default, MPEGDemultiplexer, AudioRenderer };

		private Clock referenceClock = Clock.AudioRenderer;
        private VideoType videoType = VideoType.MPEG2;
        private AudioType audioType = AudioType.MPEG2;
        private string audioDecoderDevice = "";
		private string mpeg2DecoderDevice = "";
		private string h264DecoderDevice = "";
		protected string tunerDevice = "";
		protected string captureDevice = "";

		private int frequency = -1;

		private int onid = -1;
		private int tsid = -1;
		private int sid = -1;

		private int pmtPid = -1;
		private int videoPid = -1;
		private int pcrPid = -1;
		private int audioPid = -1;
		private int[] audioPids = new int[0];
		private int teletextPid = -1;
		private int ecmPid = -1;
		private int[] ecmPids = new int[0];

		private DirectShowLib.BDA.ModulationType modulation = DirectShowLib.BDA.ModulationType.ModNotSet;
		private int symbolRate = -1;
		private DirectShowLib.BDA.FECMethod innerFEC;
		private DirectShowLib.BDA.BinaryConvolutionCodeRate innerFECRate;
		private DirectShowLib.BDA.FECMethod outerFEC;
		private DirectShowLib.BDA.BinaryConvolutionCodeRate outerFECRate;

		public ChannelDVB() { }

		[Category("Devices (Others)"), DisplayName("Reference Clock"), Description("The reference clock (Not for timeshifting).")]
		public Clock ReferenceClock { get { return this.referenceClock; } set { this.referenceClock = value; } }

        [Category("Devices (Others)"), DisplayName("Audio Decoder Type"), Description("The audio type.")]
        public AudioType AudioDecoderType { get { return this.audioType; } set { this.audioType = value; } }
		[Category("Devices (Others)"), DisplayName("Audio Decoder Device"), Description("The audio decoder device.")]
		public string AudioDecoderDevice { get { return this.audioDecoderDevice; } set { this.audioDecoderDevice = value; } }

		[Category("Devices (Others)"), DisplayName("Video Decoder Type"), Description("The video type.")]
		public VideoType VideoDecoderType { get { return this.videoType; } set { this.videoType = value; } }
		[Category("Devices (Others)"), DisplayName("MPEG2 Decoder Device"), Description("The MPEG2 decoder device.")]
		public string MPEG2DecoderDevice { get { return this.mpeg2DecoderDevice; } set { this.mpeg2DecoderDevice = value; } }
		[Category("Devices (Others)"), DisplayName("H264 Decoder Device"), Description("The H264 decoder device.")]
		public string H264DecoderDevice { get { return this.h264DecoderDevice; } set { this.h264DecoderDevice = value; } }
		[Category("Devices (DVB)"), DisplayName("Tuner Device"), Description("The tuner device.")]
		public string TunerDevice { get { return this.tunerDevice; } set { this.tunerDevice = value; } }
		[Category("Devices (DVB)"), DisplayName("Capture Device"), Description("The capture device.")]
		public string CaptureDevice { get { return this.captureDevice; } set { this.captureDevice = value; } }

        //[Editor(typeof(FrequencyEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[Category("Locator"), Description("Retrieves the frequency of the RF signal.")]
		public int Frequency { get { return this.frequency; } set { this.frequency = value; } }

		[Category("Locator"), Description("The modulation type.")]
		public DirectShowLib.BDA.ModulationType Modulation { get { return this.modulation; } set { this.modulation = value; } }
		[Category("Locator"), DisplayName("Symbol Rate"), Description("The QPSK symbol rate.")]
		public int SymbolRate { get { return this.symbolRate; } set { this.symbolRate = value; } }
		[Category("Locator"), DisplayName("Inner FEC"), Description("The type of inner forward error correction that is used.")]
		public DirectShowLib.BDA.FECMethod InnerFEC { get { return this.innerFEC; } set { this.innerFEC = value; } }
		[Category("Locator"), DisplayName("Inner FEC Rate"), Description("The inner FEC rate.")]
		public DirectShowLib.BDA.BinaryConvolutionCodeRate InnerFECRate { get { return this.innerFECRate; } set { this.innerFECRate = value; } }
		[Category("Locator"), DisplayName("Outer FEC"), Description("The type of outer forward error correction that is used.")]
		public DirectShowLib.BDA.FECMethod OuterFEC { get { return this.outerFEC; } set { this.outerFEC = value; } }
		[Category("Locator"), DisplayName("Outer FEC Rate"), Description("The outer FEC rate.")]
		public DirectShowLib.BDA.BinaryConvolutionCodeRate OuterFECRate { get { return this.outerFECRate; } set { this.outerFECRate = value; } }

		[Category("Tune Request"), Description("The original network ID.")]
		public int ONID { get { return this.onid; } set { this.onid = value; } }
		[Category("Tune Request"), Description("The transport stream ID.")]
		public int TSID { get { return this.tsid; } set { this.tsid = value; } }
		[Category("Tune Request"), Description("The service ID.")]
		public int SID { get { return this.sid; } set { this.sid = value; } }

		[Category("Pid"), DisplayName("PMT Pid"), Description("The PMT Pid.")]
		public int PmtPid { get { return this.pmtPid; } set { this.pmtPid = value; } }
		[Category("Pid"), DisplayName("Video Pid"), Description("The video Pid.")]
		public int VideoPid { get { return this.videoPid; } set { this.videoPid = value; } }
		[Category("Pid"), DisplayName("PCR Pid"), Description("The PCR Pid.")]
		public int PcrPid { get { return this.pcrPid; } set { this.pcrPid = value; } }
		[Category("Pid"), DisplayName("Audio Pid"), Description("The audio Pid.")]
		public int AudioPid { get { return this.audioPid; } set { this.audioPid = value; } }
		[Category("Pid"), DisplayName("Audio Pids"), Description("The audio Pids.")]
		public int[] AudioPids { get { return this.audioPids; } set { this.audioPids = value; } }
		[Category("Pid"), DisplayName("Teletext Pid"), Description("The teletext Pid.")]
		public int TeletextPid { get { return this.teletextPid; } set { this.teletextPid = value; } }
		[Category("Pid"), DisplayName("ECM Pid"), Description("The ECM Pid.")]
		public int EcmPid { get { return this.ecmPid; } set { this.ecmPid = value; } }
		[Category("Pid"), DisplayName("ECM Pids"), Description("The ECM Pids.")]
		public int[] EcmPids { get { return this.ecmPids; } set { this.ecmPids = value; } }

		public override bool NeedToRebuildTheGraph(ChannelTV newChannel)
		{
			return !(newChannel is ChannelDVB) ||
				base.NeedToRebuildTheGraph(newChannel) ||
                (newChannel as ChannelDVB).AudioDecoderType != AudioDecoderType ||
                (newChannel as ChannelDVB).AudioDecoderDevice != AudioDecoderDevice ||
				(newChannel as ChannelDVB).VideoDecoderType != VideoDecoderType ||
				(newChannel as ChannelDVB).H264DecoderDevice != H264DecoderDevice ||
				(newChannel as ChannelDVB).MPEG2DecoderDevice != MPEG2DecoderDevice ||
				(newChannel as ChannelDVB).TunerDevice != TunerDevice ||
				(newChannel as ChannelDVB).CaptureDevice != CaptureDevice ||
				(newChannel as ChannelDVB).ReferenceClock != ReferenceClock;
		}

		public override string ToString()
		{
			return base.ToString() + ", tunerDevice: " + this.TunerDevice + ", captureDevice: " + this.CaptureDevice + ", frequency: " + this.Frequency + " KHz, onid: " + this.ONID + ", TSID: " + this.TSID + ", SID: " + this.SID + ", Modulation: " + this.modulation.ToString() + ", SymbolRate: " + this.symbolRate;
		}

        public override bool Equals(object obj)
        {
            //return (obj is ChannelDVB) &&
            //    (obj as ChannelDVB).AudioPids == this.AudioPids &&
            //    (obj as ChannelDVB).Frequency == this.Frequency;

            return (obj is ChannelDVB) &&
                (obj as ChannelDVB).AudioPid == this.AudioPid;
        }
	}

	public class ChannelDVBT : ChannelDVB
    {
		private int bandwidth = -1;
		private DirectShowLib.BDA.HierarchyAlpha hAlpha = DirectShowLib.BDA.HierarchyAlpha.HAlphaNotSet;
		private DirectShowLib.BDA.GuardInterval guard = DirectShowLib.BDA.GuardInterval.GuardNotSet;
		private DirectShowLib.BDA.TransmissionMode mode = DirectShowLib.BDA.TransmissionMode.ModeNotSet;
		private DirectShowLib.BDA.FECMethod lpInnerFEC;
		private DirectShowLib.BDA.BinaryConvolutionCodeRate lpInnerFECRate;
		private bool otherFrequencyInUse = false;

		public ChannelDVBT() {}

		[Category("Locator"), Description("The bandwidth of the frequency in megahertz, usually 7 or 8.")]
		public int Bandwidth { get { return this.bandwidth; } set { this.bandwidth = value; } }
		[Category("Locator"), Description("The hierarchy alpha.")]
		public DirectShowLib.BDA.HierarchyAlpha HAlpha { get { return this.hAlpha; } set { this.hAlpha = value; } }
		[Category("Locator"), Description("The guard interval.")]
		public DirectShowLib.BDA.GuardInterval Guard { get { return this.guard; } set { this.guard = value; } }
		[Category("Locator"), Description("The transmission mode.")]
		public DirectShowLib.BDA.TransmissionMode Mode { get { return this.mode; } set { this.mode = value; } }
		[Category("Locator"), DisplayName("Low Priority Inner FEC"), Description("The inner FEC type of the low-priority stream.")]
		public DirectShowLib.BDA.FECMethod LPInnerFEC { get { return this.lpInnerFEC; } set { this.lpInnerFEC = value; } }
		[Category("Locator"), DisplayName("Low Priority Inner FEC Rate"), Description("The inner FEC rate of the low-priority stream.")]
		public DirectShowLib.BDA.BinaryConvolutionCodeRate LPInnerFECRate { get { return this.lpInnerFECRate; } set { this.lpInnerFECRate = value; } }
		[Category("Locator"), DisplayName("Other Frequency In Use"), Description("Specifies whether the frequency is being used by another DVB-T broadcaster.")]
		public bool OtherFrequencyInUse { get { return this.otherFrequencyInUse; } set { this.otherFrequencyInUse = value; } }
		
		public override bool NeedToRebuildTheGraph(ChannelTV newChannel)
		{
			return !(newChannel is ChannelDVBT) || base.NeedToRebuildTheGraph(newChannel);
		}

		public override string ToString()
		{
			return base.ToString() + ", bandwidth: " + this.bandwidth +
				", hAlpha: " + this.hAlpha.ToString() +
				", guard: " + this.guard.ToString() +
				", mode: " + this.mode.ToString();
		}

        public override bool Equals(object obj)
        {
            return base.Equals(obj) &&
                (obj is ChannelDVBT) &&
                (obj as ChannelDVBT).VideoPid == this.VideoPid;
        }
	}

	public class ChannelDVBC : ChannelDVB
	{
		public ChannelDVBC() { }

		public override bool NeedToRebuildTheGraph(ChannelTV newChannel)
		{
			return !(newChannel is ChannelDVBC) || base.NeedToRebuildTheGraph(newChannel);
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}

	public class ChannelDVBS : ChannelDVB
	{
		private DirectShowLib.BDA.Polarisation signalPolarisation = DirectShowLib.BDA.Polarisation.NotSet;
		private bool westPosition = false;
		private int orbitalPosition = -1;
		private int azimuth = -1;
		private int elevation = -1;

		public ChannelDVBS() { }

		[Category("Locator"), DisplayName("Signal Polarisation"), Description("The signal polarisation.")]
		public DirectShowLib.BDA.Polarisation SignalPolarisation { get { return this.signalPolarisation; } set { this.signalPolarisation = value; } }
		[Category("Locator"), DisplayName("West Position"), Description("A value indicating whether the orbital position is given in east or west longitude.")]
		public bool WestPosition { get { return this.westPosition; } set { this.westPosition = value; } }
		[Category("Locator"), DisplayName("Orbital Position"), Description("The setting for the satellite's orbital position.")]
		public int OrbitalPosition { get { return this.orbitalPosition; } set { this.orbitalPosition = value; } }
		[Category("Locator"), Description("The azimuth setting used for positioning the satellite dish.")]
		public int Azimuth { get { return this.azimuth; } set { this.azimuth = value; } }
		[Category("Locator"), Description("The elevation of the satellite in tenths of a degree.")]
		public int Elevation { get { return this.elevation; } set { this.elevation = value; } }

		public override bool NeedToRebuildTheGraph(ChannelTV newChannel)
		{
			return !(newChannel is ChannelDVBS) || base.NeedToRebuildTheGraph(newChannel);
		}

		public override string ToString()
		{
			return base.ToString() + ", signalPolarisation: " + this.signalPolarisation +
				", westPosition: " + this.westPosition +
				", orbitalPosition: " + this.orbitalPosition +
				", azimuth: " + this.azimuth +
				", elevation: " + this.elevation;
		}
	}
}
