//Usar threads para fazer o scan
//usar o this.invokeneeded do controlo nos eventos

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

using Microsoft.Win32;

using DirectShowLib;
using DirectShowLib.BDA;
using DirectShowLib.Utils;

using TV2Lib.PSI;

namespace TV2Lib
{
	public class GraphBuilderBDA : GraphBuilderTV, IBDA, IEPG
	{
		protected IBaseFilter networkProvider;
		protected IBaseFilter mpeg2Demux;
		protected IBaseFilter tuner;
		protected IBaseFilter capture;
        protected IBaseFilter demodulator;
		protected IBaseFilter bdaTIF;
		protected IBaseFilter bdaSecTab;
		protected ITuningSpace objTuningSpace;
		protected ITuneRequest objTuneRequest;
		protected IBaseFilter audioDecoderFilter = null;
		protected IBaseFilter videoMpeg2DecoderFilter = null;
		protected IBaseFilter videoH264DecoderFilter = null;

		protected ChannelDVB.Clock referenceClock = ChannelDVB.Clock.AudioRenderer;
        protected ChannelDVB.AudioType audioType = ChannelDVB.AudioType.MPEG2;

		private static Dictionary<string, DsDevice> audioDecoderDevices;
		protected DsDevice audioDecoderDevice;
		private static Dictionary<string, DsDevice> mpeg2DecoderDevices;
		protected DsDevice mpeg2DecoderDevice;
		private static Dictionary<string, DsDevice> h264DecoderDevices;
		protected DsDevice h264DecoderDevice;
		private static Dictionary<string, DsDevice> bdaTunerDevices;
		protected DsDevice tunerDevice;
		private static Dictionary<string, DsDevice> bdaCaptureDevices;
		protected DsDevice captureDevice;

		protected EPG epg;

        public static Dictionary<string, DsDevice> AudioDecoderDevices
		{
			get
			{
				if (audioDecoderDevices == null)
				{
					audioDecoderDevices = new Dictionary<string, DsDevice>();

					DsDevice[] devices = DeviceEnumerator.GetDevicesWithThisInPin(MediaType.Audio, MediaSubType.Mpeg2Audio);
					foreach (DsDevice d in devices)
						if (d.Name != null)
                            audioDecoderDevices.Add(d.DevicePath, d);
				}
				return audioDecoderDevices;
			}
		}
		public static Dictionary<string, DsDevice> MPEG2DecoderDevices
		{
			get
			{
				if (mpeg2DecoderDevices == null)
				{
					mpeg2DecoderDevices = new Dictionary<string, DsDevice>();

					DsDevice[] devices = DeviceEnumerator.GetMPEG2Devices();
                    foreach (DsDevice d in devices)
                        if (d.Name != null)
                            mpeg2DecoderDevices.Add(d.DevicePath, d);
                }
				return mpeg2DecoderDevices;
			}
		}
		public static Dictionary<string, DsDevice> H264DecoderDevices
		{
			get
			{
				if (h264DecoderDevices == null)
				{
					h264DecoderDevices = new Dictionary<string, DsDevice>();

					DsDevice[] devices = DeviceEnumerator.GetH264Devices();
					foreach (DsDevice d in devices)
                        h264DecoderDevices.Add(d.DevicePath, d);
				}
				return h264DecoderDevices;
			}
		}
		public static Dictionary<string, DsDevice> TunerDevices
		{
			get
			{
				if(bdaTunerDevices == null)
				{
					bdaTunerDevices = new Dictionary<string, DsDevice>();

					// Then enumerate BDA Receiver Components category to found a filter connecting 
					// to the tuner and the MPEG2 Demux
					DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory);
					foreach (DsDevice d in devices)
						bdaTunerDevices.Add(d.DevicePath, d);
				}
				return bdaTunerDevices;
			}
		}
		public static Dictionary<string, DsDevice> CaptureDevices
		{
			get
			{
				if (bdaCaptureDevices == null)
				{
					bdaCaptureDevices = new Dictionary<string, DsDevice>();

					// Enumerate BDA Source filters category and found one that can connect to the network provider
					DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.BDAReceiverComponentsCategory);
					foreach (DsDevice d in devices)
                        bdaCaptureDevices.Add(d.DevicePath, d);
				}
				return bdaCaptureDevices;
			}
		}

		public ChannelDVB.Clock ReferenceClock { get { return this.referenceClock; } set { this.referenceClock = value; } }

        public ChannelDVB.AudioType AudioDecoderType { get { return this.audioType; } set { this.audioType = value; } }
		public DsDevice AudioDecoderDevice { get { return this.audioDecoderDevice; } set { this.audioDecoderDevice = value; } }
		public DsDevice Mpeg2DecoderDevice { get { return this.mpeg2DecoderDevice; } set { this.mpeg2DecoderDevice = value; } }
		public DsDevice H264DecoderDevice { get { return this.h264DecoderDevice; } set { this.h264DecoderDevice = value; } }
		public DsDevice TunerDevice { get { return this.tunerDevice; } set { this.tunerDevice = value; } }
		public DsDevice CaptureDevice { get { return this.captureDevice; } set { this.captureDevice = value; } }

		public IBaseFilter NetworkProvider { get { return this.networkProvider; } }
		public IBaseFilter TunerFilter { get { return this.tuner; } }
		public IBaseFilter CaptureFilter { get { return this.capture; } }
        public IBaseFilter DemodulatorFilter { get { return this.demodulator; } }
		public IBaseFilter Demultiplexer { get { return this.mpeg2Demux; } }
		public IBaseFilter SectionsAndTables { get { return this.bdaSecTab; } }
		public IBaseFilter TransportInformationFilter { get { return this.bdaTIF; } }

		public ITuningSpace TuningSpace { get { return this.objTuningSpace; } set { this.objTuningSpace = value; } }
		public ITuneRequest TuneRequest { get { return this.objTuneRequest; } set { this.objTuneRequest = value; } }

		public EPG EPG { get { return this.epg; } }

		private int cookies = 0;
		protected bool isH264ElecardSpecialMode = false;

		public GraphBuilderBDA(VideoControl renderingControl)
			: base(renderingControl)
		{
            
		}

        private void AddROTEntry(IFilterGraph graph)
        {
#if DEBUG
            rot = new DsROTEntry(graph);
#endif
        }

        public bool BuildGraphNoRenderers(DVBTTuning tuner)
        {
            this.objTuneRequest = tuner.TuneRequest;
            this.objTuningSpace = tuner.TuningSpace;

            return BuildGraphNoRenderers();
        }
        public bool BuildGraphNoRenderers()
        {
            this.OnNewLogMessage("Building graph without renderers");

            int hr;

            this.graphBuilder = (IFilterGraph2)new FilterGraph();
            this.AddROTEntry(this.graphBuilder);

            this.epg = new EPG(this.hostingControl);

            this.cookies = this.hostingControl.SubscribeEvents(this as VideoControl.IVideoEventHandler, this.graphBuilder as IMediaEventEx);

            hr = AddNetworkProviderFilter(this.objTuningSpace);

            hr = AddMPEG2DemuxFilter();
            hr = CreateMPEG2DemuxPinsNoRenderers();

            //Não adicionar nenhum decoder/renderer

            hr = AddAndConnectBDABoardFilters();

            //+++ This order is to avoid a bug from the DirectShow 
            AddAndConnectSectionsAndTablesFilterToGraph();

            IMpeg2Data mpeg2Data = this.bdaSecTab as IMpeg2Data;
            ISectionList ppSectionList;
            int hr2 = mpeg2Data.GetTable(0, 0, null, 0, out ppSectionList);

            AddAndConnectTIFToGraph();

            this.epg.RegisterEvent(TransportInformationFilter as IConnectionPointContainer);

            this.hostingControl.CurrentGraphBuilder = this;

            OnGraphStarted();

            return true;
        }

        public int BuildGraph(TV2Lib.ChannelDVB.VideoType videoSubtype, TV2Lib.ChannelDVB.AudioType audioSubtype)
        {
            int hr = 0;

            try
            {
                this.OnNewLogMessage("Building graph");
                //useWPF = Settings.UseWPF;

                this.graphBuilder = (IFilterGraph2)new FilterGraph();
                rot = new DsROTEntry(this.graphBuilder);

                this.epg = new EPG(this.hostingControl);

                this.cookies = this.hostingControl.SubscribeEvents(this as VideoControl.IVideoEventHandler, this.graphBuilder as IMediaEventEx);

                hr = AddNetworkProviderFilter(this.objTuningSpace);
                DsError.ThrowExceptionForHR(hr);


                hr = AddMPEG2DemuxFilter();
                DsError.ThrowExceptionForHR(hr);
                hr = CreateMPEG2DemuxPins2(videoSubtype, audioSubtype);
                DsError.ThrowExceptionForHR(hr);

                //AddAndConnectTransportStreamFiltersToGraph();
                if (this.AudioDecoderDevice != null)
                {
                    hr = AddAudioDecoderFilters(this.graphBuilder);
                    DsError.ThrowExceptionForHR(hr);
                }

                if (this.Mpeg2DecoderDevice != null || this.H264DecoderDevice != null)
                {
                    hr = AddBDAVideoDecoderFilters2(this.graphBuilder, videoSubtype);
                    DsError.ThrowExceptionForHR(hr);
                }

                //Alterar acima. Separar os ifs

                hr = AddAndConnectBDABoardFilters();
                DsError.ThrowExceptionForHR(hr);
                if (this.tuner == null) // && this.capture == null)
                    throw new ApplicationException("No BDA devices found!");

                if (this.H264DecoderDevice != null || !isH264ElecardSpecialMode)
                {
                    //+++ This order is to avoid a bug from the DirectShow 
                    hr = AddAndConnectSectionsAndTablesFilterToGraph();
                    DsError.ThrowExceptionForHR(hr);

                    IMpeg2Data mpeg2Data = this.bdaSecTab as IMpeg2Data;
                    ISectionList ppSectionList;
                    int hr2 = mpeg2Data.GetTable(0, 0, null, 0, out ppSectionList);

                    hr = AddAndConnectTIFToGraph();
                    DsError.ThrowExceptionForHR(hr);
                    //---
                }

                AddRenderers();
                //if (!useWPF)
                ConfigureVMR9InWindowlessMode();
                //RenderMpeg2DemuxFilters();
                ConnectAudioAndVideoFilters();

                this.epg.RegisterEvent(TransportInformationFilter as IConnectionPointContainer);

                Vmr9SetDeinterlaceMode(1);

                IMediaFilter mf = this.graphBuilder as IMediaFilter;
                switch (this.referenceClock)
                {
                    case ChannelDVB.Clock.Default:
                        hr = this.graphBuilder.SetDefaultSyncSource();
                        break;
                    case ChannelDVB.Clock.MPEGDemultiplexer:
                        hr = mf.SetSyncSource(this.mpeg2Demux as IReferenceClock);
                        break;
                    case ChannelDVB.Clock.AudioRenderer:
                        hr = mf.SetSyncSource(this.audioRenderer as IReferenceClock);
                        break;
                }

                this.hostingControl.CurrentGraphBuilder = this;

                OnGraphStarted();
            }
            catch (Exception ex)
            {
                OnNewLogMessage("Failed to build graph");
                LogException(ex);

                Decompose();
            }

            return hr;
        }
        public int BuildGraph(DVBTTuning tuner, TV2Lib.ChannelDVB.VideoType videoSubtype, TV2Lib.ChannelDVB.AudioType audioSubtype)
        {
            this.objTuneRequest = tuner.TuneRequest;
            this.objTuningSpace = tuner.TuningSpace;

            return BuildGraph(videoSubtype, audioSubtype);
        }

        public int BuildGraph(DVBTTuning tuner)
        {
            this.objTuneRequest = tuner.TuneRequest;
            this.objTuningSpace = tuner.TuningSpace;

            return BuildGraph();
        }
		public override int BuildGraph()
		{
            int hr = 0;

			try
			{
                this.OnNewLogMessage("Building graph");
                //useWPF = Settings.UseWPF;

				this.graphBuilder = (IFilterGraph2)new FilterGraph();
				rot = new DsROTEntry(this.graphBuilder);

				this.epg = new EPG(this.hostingControl);

				this.cookies = this.hostingControl.SubscribeEvents(this as VideoControl.IVideoEventHandler, this.graphBuilder as IMediaEventEx);

				hr = AddNetworkProviderFilter(this.objTuningSpace);
                DsError.ThrowExceptionForHR(hr);


				hr = AddMPEG2DemuxFilter();
                DsError.ThrowExceptionForHR(hr);
				hr = CreateMPEG2DemuxPins();
                DsError.ThrowExceptionForHR(hr);

				//AddAndConnectTransportStreamFiltersToGraph();
                if (this.AudioDecoderDevice != null && (this.Mpeg2DecoderDevice != null || this.H264DecoderDevice != null))
                {
                    hr = AddAudioDecoderFilters(this.graphBuilder);
                    DsError.ThrowExceptionForHR(hr);
                    hr = AddBDAVideoDecoderFilters(this.graphBuilder);
                    DsError.ThrowExceptionForHR(hr);
                }

                //Alterar acima. Separar os ifs

				hr = AddAndConnectBDABoardFilters();
                DsError.ThrowExceptionForHR(hr);
				if (this.tuner == null) // && this.capture == null)
					throw new ApplicationException("No BDA devices found!");

				if (this.H264DecoderDevice != null || !isH264ElecardSpecialMode)
				{
					//+++ This order is to avoid a bug from the DirectShow 
					hr = AddAndConnectSectionsAndTablesFilterToGraph();
                    DsError.ThrowExceptionForHR(hr);

					IMpeg2Data mpeg2Data = this.bdaSecTab as IMpeg2Data;
					ISectionList ppSectionList;
					int hr2 = mpeg2Data.GetTable(0, 0, null, 0, out ppSectionList);

					hr = AddAndConnectTIFToGraph();
                    DsError.ThrowExceptionForHR(hr);
					//---
				}

                AddRenderers();
                //if (!useWPF)
					ConfigureVMR9InWindowlessMode();
				//RenderMpeg2DemuxFilters();
				ConnectAudioAndVideoFilters();

				this.epg.RegisterEvent(TransportInformationFilter as IConnectionPointContainer);

				Vmr9SetDeinterlaceMode(1);

				IMediaFilter mf = this.graphBuilder as IMediaFilter;
				switch (this.referenceClock)
				{
					case ChannelDVB.Clock.Default:
						hr = this.graphBuilder.SetDefaultSyncSource();
						break;
					case ChannelDVB.Clock.MPEGDemultiplexer:
						hr = mf.SetSyncSource(this.mpeg2Demux as IReferenceClock);
						break;
					case ChannelDVB.Clock.AudioRenderer:
						hr = mf.SetSyncSource(this.audioRenderer as IReferenceClock);
						break;
				}

				this.hostingControl.CurrentGraphBuilder = this;

				OnGraphStarted();
			}
			catch (Exception ex)
			{
                OnNewLogMessage("Failed to build graph");
                LogException(ex);

				Decompose();
			}

            return hr;
		}


        private void LogException(Exception ex)
        {
            LogException(ex, 0);
        }
        private void LogException(Exception ex, int level)
        {
            OnNewLogMessage("Exception level: " + level);
            OnNewLogMessage("Type: " + ex.GetType().ToString());
            OnNewLogMessage("Message: " + ex.Message);
            OnNewLogMessage("Inner Exception: " + (ex.InnerException == null ? "No" : "Yes"));
            if (ex.InnerException != null)
                LogException(ex.InnerException, ++level);
        }


		private int currentMpeg2VideoPid = -1;
		private int currentH264VideoPid = -1;
		private int currentAudioPid = -1;

        public void PutTuning(DVBTTuning tune)
        {
            this.objTuneRequest = tune.TuneRequest;
            this.objTuningSpace = tune.TuningSpace;
        }

        public int Start(DVBTTuning tuner)
        {
            int hr = 0;
            try
            {
                this.OnNewLogMessage("Starting the TV component");

                hr = this.BuildGraph(tuner);
                DsError.ThrowExceptionForHR(hr);

                hr = this.RunGraph();
                DsError.ThrowExceptionForHR(hr);
            }
            catch (Exception ex)
            {
                this.OnNewLogMessage("TV component failed");
                this.LogException(ex);

                this.Decompose();
            }

            return hr;
        }
        public int Start(DVBTTuning tuner, TV2Lib.ChannelDVB.VideoType videoType, TV2Lib.ChannelDVB.AudioType audioType)
        {
            int hr = 0;
            try
            {
                this.OnNewLogMessage("Starting the TV component");

                hr = this.BuildGraph(tuner);
                DsError.ThrowExceptionForHR(hr);

                hr = this.RunGraph();
                DsError.ThrowExceptionForHR(hr);
            }
            catch (Exception ex)
            {
                this.OnNewLogMessage("TV component failed");
                this.LogException(ex);

                this.Decompose();
            }

            return hr;
        }

        public int ChangeFrequencyDuringScan(int frequency)
        {
            OnNewLogMessage(string.Format("Testing {0}khz", frequency));
            int hr = 0;

            try
            {
                ITuner tuner = NetworkProvider as ITuner;
                hr = tuner.get_TuningSpace(out this.objTuningSpace);
                DsError.ThrowExceptionForHR(hr);

                hr = this.objTuningSpace.CreateTuneRequest(out this.objTuneRequest);
                DsError.ThrowExceptionForHR(hr);

                IDVBTuneRequest dvbTuneRequest = this.objTuneRequest as IDVBTuneRequest;

                ChannelDVBT channelDVBT = currentChannel as ChannelDVBT;

                IDVBTLocator dvbLocator = (IDVBTLocator)new DVBTLocator();

                dvbLocator.put_Bandwidth(channelDVBT.Bandwidth);
                dvbLocator.put_Guard(channelDVBT.Guard);
                dvbLocator.put_HAlpha(channelDVBT.HAlpha);
                dvbLocator.put_LPInnerFEC(channelDVBT.LPInnerFEC);
                dvbLocator.put_LPInnerFECRate(channelDVBT.LPInnerFECRate);
                dvbLocator.put_Mode(channelDVBT.Mode);
                dvbLocator.put_OtherFrequencyInUse(channelDVBT.OtherFrequencyInUse);

                ILocator locator = dvbLocator as ILocator;

                locator.put_CarrierFrequency(frequency);
                channelDVBT.Frequency = frequency;

                locator.put_InnerFEC(channelDVBT.InnerFEC);
                locator.put_InnerFECRate(channelDVBT.InnerFECRate);
                locator.put_Modulation(channelDVBT.Modulation);
                locator.put_OuterFEC(channelDVBT.OuterFEC);
                locator.put_OuterFECRate(channelDVBT.OuterFECRate);
                locator.put_SymbolRate(channelDVBT.SymbolRate);

                hr = dvbTuneRequest.put_Locator(locator);
                DsError.ThrowExceptionForHR(hr);

                hr = (this.networkProvider as ITuner).put_TuneRequest(dvbTuneRequest);
                DsError.ThrowExceptionForHR(hr);

                bool present, locked;
                int quality, strength, timeoutSucatada = 0;

                do
                {
                    GetSignalStatistics(out present, out locked, out strength, out quality);
                    timeoutSucatada++;
                } while (!(present && locked) && timeoutSucatada < 5);

                if (present && locked)
                {
                    OnNewLogMessage(string.Format("DVBT signal found on {0}khz", frequency));
                    return 1;
                }
            }
            catch(Exception ex)
            {
                OnNewLogMessage("Error changing frequency");
                LogException(ex);
                return hr;
            }

            OnNewLogMessage("No signal found");
            return hr;
        }

		public override void SubmitTuneRequest(Channel channel)
		{
			if (channel is ChannelDVB)
			{
				ChannelDVB channelDVB = channel as ChannelDVB;

				int hr = 0;

				IMpeg2Demultiplexer mpeg2Demultiplexer = this.mpeg2Demux as IMpeg2Demultiplexer;

				IPin pinDemuxerTIF;
				hr = this.mpeg2Demux.FindPin("TIF", out pinDemuxerTIF);
				if (pinDemuxerTIF != null)
				{
					IMPEG2PIDMap mpeg2PIDMap = pinDemuxerTIF as IMPEG2PIDMap;
					if (mpeg2PIDMap != null)
						hr = mpeg2PIDMap.MapPID(6, new int[] { 0x00, 0x10, 0x11, 0x12, 0x13, 0x14 }, MediaSampleContent.Mpeg2PSI);
					Marshal.ReleaseComObject(pinDemuxerTIF);
				}

				if (channelDVB.VideoPid != -1)
				{
					if (channelDVB.VideoDecoderType == ChannelDVB.VideoType.H264)
					{
						IPin pinDemuxerVideoMPEG4;
						hr = this.mpeg2Demux.FindPin("H264", out pinDemuxerVideoMPEG4);
						if (pinDemuxerVideoMPEG4 != null)
						{
							IMPEG2PIDMap mpeg2PIDMap = pinDemuxerVideoMPEG4 as IMPEG2PIDMap;
							if (mpeg2PIDMap != null)
							{
								if (this.currentH264VideoPid >= 0)
									hr = mpeg2PIDMap.UnmapPID(1, new int[] { this.currentH264VideoPid });

								hr = mpeg2PIDMap.MapPID(1, new int[] { channelDVB.VideoPid }, MediaSampleContent.ElementaryStream);
								this.currentH264VideoPid = channelDVB.VideoPid;
							}
							Marshal.ReleaseComObject(pinDemuxerVideoMPEG4);
						}

						//IVMRMixerControl9 vmrMixerControl9 = this.videoRenderer as IVMRMixerControl9;
						//vmrMixerControl9.SetZOrder(0, 1);
					}
					else
					{
						IPin pinDemuxerVideoMPEG2;
						hr = this.mpeg2Demux.FindPin("MPG2", out pinDemuxerVideoMPEG2);
						if (pinDemuxerVideoMPEG2 != null)
						{
							IMPEG2PIDMap mpeg2PIDMap = pinDemuxerVideoMPEG2 as IMPEG2PIDMap;
							if (mpeg2PIDMap != null)
							{
								//IEnumPIDMap enumPIDMap;
								//hr = mpeg2PIDMap.EnumPIDMap(out enumPIDMap);
								//PIDMap[] pidMap = new PIDMap[1];
								//int numReceive = 0;
								//ArrayList al = new ArrayList();
								//while (enumPIDMap.Next(1, pidMap, out numReceive) >= 0)
								//{
								//    al.Add(pidMap[0].ulPID);
								//}

								if (this.currentMpeg2VideoPid >= 0)
									hr = mpeg2PIDMap.UnmapPID(1, new int[] { this.currentMpeg2VideoPid });

								hr = mpeg2PIDMap.MapPID(1, new int[] { channelDVB.VideoPid }, MediaSampleContent.ElementaryStream);
								this.currentMpeg2VideoPid = channelDVB.VideoPid;
							}
							Marshal.ReleaseComObject(pinDemuxerVideoMPEG2);
						}
					}
				}

				if (channelDVB.AudioPid != -1)
				{
					IPin pinDemuxerAudio;
					hr = this.mpeg2Demux.FindPin("Audio", out pinDemuxerAudio);
					if (pinDemuxerAudio != null)
					{
						IMPEG2PIDMap mpeg2PIDMap = pinDemuxerAudio as IMPEG2PIDMap;
						if (mpeg2PIDMap != null)
						{
							if (this.currentAudioPid >= 0)
								hr = mpeg2PIDMap.UnmapPID(1, new int[] { this.currentAudioPid });

							hr = mpeg2PIDMap.MapPID(1, new int[] { channelDVB.AudioPid }, MediaSampleContent.ElementaryStream);
							this.currentAudioPid = channelDVB.AudioPid;
						}
						Marshal.ReleaseComObject(pinDemuxerAudio);
					}
				}

				IPin pinDemuxerSectionsAndTables;
				hr = this.mpeg2Demux.FindPin("PSI", out pinDemuxerSectionsAndTables);
				if (pinDemuxerSectionsAndTables != null)
				{
					IMPEG2PIDMap mpeg2PIDMap = pinDemuxerSectionsAndTables as IMPEG2PIDMap;
					if (mpeg2PIDMap != null)
						hr = mpeg2PIDMap.MapPID(2, new int[] { (int)PIDS.PAT, (int)PIDS.SDT }, MediaSampleContent.Mpeg2PSI);
					Marshal.ReleaseComObject(pinDemuxerSectionsAndTables);
				}

				ITuner tuner = NetworkProvider as ITuner;
				hr = tuner.get_TuningSpace(out this.objTuningSpace);

				this.objTuningSpace.CreateTuneRequest(out this.objTuneRequest);
				IDVBTuneRequest dvbTuneRequest = (IDVBTuneRequest)this.objTuneRequest;
				dvbTuneRequest.put_ONID(channelDVB.ONID);
				dvbTuneRequest.put_TSID(channelDVB.TSID);
				dvbTuneRequest.put_SID(channelDVB.SID);

				ILocator locator = null;
				if (channel is ChannelDVBT)
				{
					ChannelDVBT channelDVBT = channel as ChannelDVBT;

					IDVBTLocator dvbLocator = (IDVBTLocator)new DVBTLocator();

					dvbLocator.put_Bandwidth(channelDVBT.Bandwidth);
					dvbLocator.put_Guard(channelDVBT.Guard);
					dvbLocator.put_HAlpha(channelDVBT.HAlpha);
					dvbLocator.put_LPInnerFEC(channelDVBT.LPInnerFEC);
					dvbLocator.put_LPInnerFECRate(channelDVBT.LPInnerFECRate);
					dvbLocator.put_Mode(channelDVBT.Mode);
					dvbLocator.put_OtherFrequencyInUse(channelDVBT.OtherFrequencyInUse);

					locator = dvbLocator as ILocator;
				}
				else if (channel is ChannelDVBC)
				{
					ChannelDVBC channelDVBC = channel as ChannelDVBC;

					IDVBCLocator dvbLocator = (IDVBCLocator)new DVBCLocator();
					locator = dvbLocator as ILocator;
				}
				else if (channel is ChannelDVBS)
				{
					ChannelDVBS channelDVBS = channel as ChannelDVBS;

					IDVBSLocator dvbLocator = (IDVBSLocator)new DVBSLocator();
					dvbLocator.put_CarrierFrequency((int)channelDVBS.Frequency);

					dvbLocator.put_Azimuth(channelDVBS.Azimuth);
					dvbLocator.put_Elevation(channelDVBS.Elevation);
					dvbLocator.put_OrbitalPosition(channelDVBS.OrbitalPosition);
					dvbLocator.put_SignalPolarisation(channelDVBS.SignalPolarisation);
					dvbLocator.put_WestPosition(channelDVBS.WestPosition);

					locator = dvbLocator as ILocator;
				}
				
				locator.put_CarrierFrequency((int)channelDVB.Frequency);

				locator.put_InnerFEC(channelDVB.InnerFEC);
				locator.put_InnerFECRate(channelDVB.InnerFECRate);
				locator.put_Modulation(channelDVB.Modulation);
				locator.put_OuterFEC(channelDVB.OuterFEC);
				locator.put_OuterFECRate(channelDVB.OuterFECRate);
				locator.put_SymbolRate(channelDVB.SymbolRate);

				hr = dvbTuneRequest.put_Locator(locator);

                hr = (this.networkProvider as ITuner).put_TuneRequest(dvbTuneRequest);
                //hr = (this.networkProvider as ITuner).put_TuneRequest(this.objTuneRequest);
				DsError.ThrowExceptionForHR(hr);
			}

            //if(useWPF)
            //    WpfUpdateVideoSize(); //WPF
		}

		protected int AddNetworkProviderFilter(ITuningSpace tuningSpace)
		{
			int hr = 0;
            try
            {
                OnNewLogMessage("Adding network provider filters");

                Guid genProviderClsId = new Guid("{B2F3A67C-29DA-4C78-8831-091ED509A475}");
                Guid networkProviderClsId;

                //First test if the Generic Network Provider is available (only on MCE 2005 + Update Rollup 2)
                //if (FilterGraphTools.IsThisComObjectInstalled(genProviderClsId))
                //{
                //    this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, genProviderClsId, "Generic Network Provider");
                //    OnNewLogMessage("Added \"Generic Network Provider\" to the graph");

                //    hr = (this.networkProvider as ITuner).put_TuningSpace(tuningSpace);
                //    return;
                //}

                // Get the network type of the requested Tuning Space
                hr = tuningSpace.get__NetworkType(out networkProviderClsId);

                string networkType;
                // Get the network type of the requested Tuning Space
                if (networkProviderClsId == typeof(DVBTNetworkProvider).GUID)
                {
                    this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, networkProviderClsId, "DVBT Network Provider");
                    networkType = "DVBT";
                }
                else if (networkProviderClsId == typeof(DVBSNetworkProvider).GUID)
                {
                    this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, networkProviderClsId, "DVBS Network Provider");
                    networkType = "DVBS";
                }
                else if (networkProviderClsId == typeof(ATSCNetworkProvider).GUID)
                {
                    this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, networkProviderClsId, "ATSC Network Provider");
                    networkType = "ATSC";
                }
                else if (networkProviderClsId == typeof(DVBCNetworkProvider).GUID)
                {
                    this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, networkProviderClsId, "DVBC Network Provider");
                    networkType = "DVBC";
                }
                else
                    // Tuning Space can also describe Analog TV but this application don't support them
                    throw new ArgumentException("This application doesn't support this Tuning Space");

                hr = (this.networkProvider as ITuner).put_TuningSpace(tuningSpace);
                DsError.ThrowExceptionForHR(hr);

                OnNewLogMessage(string.Format("Added \"{0} Network Provider\" to the graph", networkType));
                return hr;
            }
            catch(Exception ex)
            {
                OnNewLogMessage("Failed to add a network provider");
                LogException(ex);

                return hr;
            }
		}

		/// <summary>
		/// CLSID_ElecardMPEGDemultiplexer
		/// </summary>
		//[ComImport, Guid("136DCBF5-3874-4B70-AE3E-15997D6334F7")]
		[ComImport, Guid("668EE184-FD2D-4C72-8E79-439A35B438DE")]
		public class ElecardMPEGDemultiplexer
		{
		}


		protected int AddMPEG2DemuxFilter()
		{
            int hr = 0;
            try
            {
                OnNewLogMessage("Adding MPEG2 demux filter");

			    if (this.H264DecoderDevice != null && isH264ElecardSpecialMode)
				    this.mpeg2Demux = (IBaseFilter)new ElecardMPEGDemultiplexer();
			    else
				    this.mpeg2Demux = (IBaseFilter)new MPEG2Demultiplexer();

			    hr = this.graphBuilder.AddFilter(this.mpeg2Demux, "MPEG2 Demultiplexer");

                #region código antigo, comentado
                //IMpeg2Demultiplexer mpeg2Demultiplexer = this.mpeg2Demux as IMpeg2Demultiplexer;

			    ////Log.WriteFile(Log.LogType.Log, false, "DVBGraphBDA: create mpg4 video pin");
			    //AMMediaType mediaMPG4 = new AMMediaType();
			    //mediaMPG4.majorType = MediaType.;
			    //mediaMPG4.subType = MediaSubType.; //new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
			    //mediaMPG4.sampleSize = 0;
			    //mediaMPG4.temporalCompression = false;
			    //mediaMPG4.fixedSizeSamples = false;
			    //mediaMPG4.unkPtr = IntPtr.Zero;
			    //mediaMPG4.formatType = FormatType.Mpeg2Video;
			    //mediaMPG4.formatSize = Mpeg2ProgramVideo.GetLength(0);

			    ////mediaMPG4.formatPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(mediaMPG4.formatSize);
			    ////System.Runtime.InteropServices.Marshal.Copy(Mpeg2ProgramVideo, 0, mediaMPG4.formatPtr, mediaMPG4.formatSize);

			    //IntPtr formatPtr = Marshal.AllocHGlobal(Mpeg2ProgramVideo.Length);
			    //Marshal.Copy(Mpeg2ProgramVideo, 0, formatPtr, Mpeg2ProgramVideo.Length);
			    //mediaMPG4.formatPtr = formatPtr;

			    //IPin pinDemuxerVideoMPEG4;
			    //int hr = mpeg2Demultiplexer.CreateOutputPin(mediaMPG4, "H264", out pinDemuxerVideoMPEG4);
			    //if (pinDemuxerVideoMPEG4 != null)
			    //    Marshal.ReleaseComObject(pinDemuxerVideoMPEG4);

                //Marshal.FreeHGlobal(formatPtr);
                #endregion

                OnNewLogMessage("Added \"MPEG2 Demultiplexer\" successfully");
			    DsError.ThrowExceptionForHR(hr);

                return hr;
            }
            catch(Exception ex)
            {
                OnNewLogMessage("Failed to add the MPEG2 Demultiplexer");
                LogException(ex);
                return hr;
            }
		}

        protected int CreateMPEG2DemuxPinsNoRenderers()
        {
            int hr = 0;

            try
            {
                OnNewLogMessage("Creating MPEG2 demux pins (no renderers)");

                IMpeg2Demultiplexer mpeg2Demultiplexer = this.mpeg2Demux as IMpeg2Demultiplexer;

                //Pin 1 connected to the "BDA MPEG2 Transport Information Filter"
                //    Major Type	MEDIATYPE_MPEG2_SECTIONS {455F176C-4B06-47CE-9AEF-8CAEF73DF7B5}
                //    Sub Type		MEDIASUBTYPE_DVB_SI {E9DD31A3-221D-4ADB-8532-9AF309C1A408}
                //    Format		None

                //    MPEG2 PSI Sections
                //    Pids: 0x00 0x10 0x11 0x12 0x13 0x14 0x6e 0xd2 0x0136 0x019a 0x01fe 0x0262 0x03f2

                OnNewLogMessage("Creating TIF pin");

                AMMediaType mediaTIF = new AMMediaType();
                mediaTIF.majorType = MediaType.Mpeg2Sections;
                mediaTIF.subType = MediaSubType.DvbSI;
                mediaTIF.fixedSizeSamples = false;
                mediaTIF.temporalCompression = false;
                mediaTIF.sampleSize = 1;
                mediaTIF.unkPtr = IntPtr.Zero;
                mediaTIF.formatType = FormatType.None;
                mediaTIF.formatSize = 0;
                mediaTIF.formatPtr = IntPtr.Zero;

                IPin pinDemuxerTIF;
                hr = mpeg2Demultiplexer.CreateOutputPin(mediaTIF, "TIF", out pinDemuxerTIF);
                if (pinDemuxerTIF != null)
                    Marshal.ReleaseComObject(pinDemuxerTIF);

                OnNewLogMessage(hr == 0 ? "TIF pin created successfully" : "Failed to create TIF pin");
                DsError.ThrowExceptionForHR(hr);

                //Pin 5 connected to "MPEG-2 Sections and Tables" (Allows to grab custom PSI tables)
                //    Major Type	MEDIATYPE_MPEG2_SECTIONS {455F176C-4B06-47CE-9AEF-8CAEF73DF7B5}
                //    Sub Type		MEDIASUBTYPE_MPEG2DATA {C892E55B-252D-42B5-A316-D997E7A5D995}
                //    Format		None

                OnNewLogMessage("Creating \"Sections and Tables (PSI)\" pin");

                AMMediaType mediaSectionsAndTables = new AMMediaType();
                mediaSectionsAndTables.majorType = MediaType.Mpeg2Sections;
                mediaSectionsAndTables.subType = MediaSubType.Mpeg2Data;
                mediaSectionsAndTables.sampleSize = 0; // 1;
                mediaSectionsAndTables.temporalCompression = false;
                mediaSectionsAndTables.fixedSizeSamples = true;
                mediaSectionsAndTables.unkPtr = IntPtr.Zero;
                mediaSectionsAndTables.formatType = FormatType.None;
                mediaSectionsAndTables.formatSize = 0;
                mediaSectionsAndTables.formatPtr = IntPtr.Zero;

                IPin pinDemuxerSectionsAndTables;
                hr = mpeg2Demultiplexer.CreateOutputPin(mediaSectionsAndTables, "PSI", out pinDemuxerSectionsAndTables);
                if (pinDemuxerSectionsAndTables != null)
                    Marshal.ReleaseComObject(pinDemuxerSectionsAndTables);

                OnNewLogMessage(hr == 0 ? "\"Sections and Tables (PSI)\" pin created successfully" : "Failed to create \"Sections and Tables (PSI)\" pin");
                DsError.ThrowExceptionForHR(hr);

                OnNewLogMessage("MPEG2 demux pins created successfully");
            }
            catch (Exception ex)
            {
                OnNewLogMessage("Failed to create MPEG2 demux pins");
                LogException(ex);
            }

            return hr;
        }

		protected virtual int CreateMPEG2DemuxPins()
		{
            int hr = 0;

            try
            {
                OnNewLogMessage("Creating MPEG2 demux pins");

			    IMpeg2Demultiplexer mpeg2Demultiplexer = this.mpeg2Demux as IMpeg2Demultiplexer;

			    {
				    //Pin 1 connected to the "BDA MPEG2 Transport Information Filter"
				    //    Major Type	MEDIATYPE_MPEG2_SECTIONS {455F176C-4B06-47CE-9AEF-8CAEF73DF7B5}
				    //    Sub Type		MEDIASUBTYPE_DVB_SI {E9DD31A3-221D-4ADB-8532-9AF309C1A408}
				    //    Format		None

				    //    MPEG2 PSI Sections
				    //    Pids: 0x00 0x10 0x11 0x12 0x13 0x14 0x6e 0xd2 0x0136 0x019a 0x01fe 0x0262 0x03f2

                    OnNewLogMessage("Creating TIF pin");

				    AMMediaType mediaTIF = new AMMediaType();
				    mediaTIF.majorType = MediaType.Mpeg2Sections;
				    mediaTIF.subType = MediaSubType.DvbSI;
				    mediaTIF.fixedSizeSamples = false;
				    mediaTIF.temporalCompression = false;
				    mediaTIF.sampleSize = 1;
				    mediaTIF.unkPtr = IntPtr.Zero;
				    mediaTIF.formatType = FormatType.None;
				    mediaTIF.formatSize = 0;
				    mediaTIF.formatPtr = IntPtr.Zero;

				    IPin pinDemuxerTIF;
				    hr = mpeg2Demultiplexer.CreateOutputPin(mediaTIF, "TIF", out pinDemuxerTIF);
				    if (pinDemuxerTIF != null)
					    Marshal.ReleaseComObject(pinDemuxerTIF);

                    OnNewLogMessage(hr == 0 ? "TIF pin created successfully" : "Failed to create TIF pin");
                    DsError.ThrowExceptionForHR(hr);
			    }

			    if (this.H264DecoderDevice != null)
			    {
                    OnNewLogMessage("Creating H264 pin");

                    //Try Original
                    AMMediaType mediaH264 = new AMMediaType();
                    mediaH264.majorType = MediaType.Video;
                    //mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
                    mediaH264.subType = MediaSubType.H264;// new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
                    mediaH264.sampleSize = 0;
                    mediaH264.temporalCompression = true; // false;
                    mediaH264.fixedSizeSamples = true; // false;
                    mediaH264.unkPtr = IntPtr.Zero;
                    mediaH264.formatType = FormatType.Mpeg2Video;

                    MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
                    mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
                    mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
                    Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

                    IPin pinDemuxerVideoH264;
                    hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
                    if (pinDemuxerVideoH264 != null)
                        Marshal.ReleaseComObject(pinDemuxerVideoH264);

                    Marshal.FreeHGlobal(mediaH264.formatPtr);

                    OnNewLogMessage(hr == 0 ? "H264 pin created successfully" : "Failed to create H264 pin");
                    DsError.ThrowExceptionForHR(hr);

                    #region Tries
                    ////Try 1
                    //AMMediaType mediaH264 = new AMMediaType();
                    //mediaH264.majorType = MediaType.Null;
                    ////mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
                    //mediaH264.subType = MediaSubType.H264;// new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
                    //mediaH264.sampleSize = 0;
                    //mediaH264.temporalCompression = true; // false;
                    //mediaH264.fixedSizeSamples = true; // false;
                    //mediaH264.unkPtr = IntPtr.Zero;
                    //mediaH264.formatType = FormatType.Null;

                    ////MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
                    ////mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
                    ////mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
                    ////Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

                    //IPin pinDemuxerVideoH264;
                    //int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
                    //if (pinDemuxerVideoH264 != null)
                    //    Marshal.ReleaseComObject(pinDemuxerVideoH264);

                    ////Marshal.FreeHGlobal(mediaH264.formatPtr);


                    ////Try http://mheg2xmltv.googlecode.com/svn/trunk/dcdvbsource/Source/Filter/DVBGraphBuilder.pas
                    //AMMediaType mediaH264 = new AMMediaType();
                    //mediaH264.majorType = MediaType.Video;
                    ////mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
                    //mediaH264.subType = MediaSubType.H264;// new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
                    //mediaH264.sampleSize = 1; // 0;
                    //mediaH264.temporalCompression = true; // false;
                    //mediaH264.fixedSizeSamples = false; // true; // false;
                    //mediaH264.unkPtr = IntPtr.Zero;
                    //mediaH264.formatType = FormatType.VideoInfo2; // FormatType.VideoInfo2;//FormatType.Mpeg2Video;

                    //VideoInfoHeader2 videoH264PinFormat = GetVideoInfoHeader2H264PinFormat(); // GetVideoH264PinFormat();
                    //mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
                    //mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
                    //Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

                    //IPin pinDemuxerVideoH264;
                    //int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
                    //if (pinDemuxerVideoH264 != null)
                    //    Marshal.ReleaseComObject(pinDemuxerVideoH264);

                    //Marshal.FreeHGlobal(mediaH264.formatPtr);
                    #endregion
			    }
			    else
			    {
                    OnNewLogMessage("Creating MPEG2 video pin");

				    AMMediaType mediaMPG2 = new AMMediaType();
				    mediaMPG2.majorType = MediaType.Video;
				    mediaMPG2.subType = MediaSubType.Mpeg2Video;
				    mediaMPG2.fixedSizeSamples = false;
				    mediaMPG2.temporalCompression = false; // true???
				    mediaMPG2.sampleSize = 0;
				    mediaMPG2.formatType = FormatType.Mpeg2Video;
				    mediaMPG2.unkPtr = IntPtr.Zero;

				    MPEG2VideoInfo videoMPEG2PinFormat = GetVideoMPEG2PinFormat();
				    mediaMPG2.formatSize = Marshal.SizeOf(videoMPEG2PinFormat);
				    mediaMPG2.formatPtr = Marshal.AllocHGlobal(mediaMPG2.formatSize);
				    Marshal.StructureToPtr(videoMPEG2PinFormat, mediaMPG2.formatPtr, false);

				    IPin pinDemuxerVideoMPEG2;
				    hr = mpeg2Demultiplexer.CreateOutputPin(mediaMPG2, "MPG2", out pinDemuxerVideoMPEG2);
				    if (pinDemuxerVideoMPEG2 != null)
					    Marshal.ReleaseComObject(pinDemuxerVideoMPEG2);

				    Marshal.FreeHGlobal(mediaMPG2.formatPtr);

                    OnNewLogMessage(hr == 0 ? "MPEG2 pin created successfully" : "Failed to create MPEG2 pin");
                    DsError.ThrowExceptionForHR(hr);
			    }

			    {
                    OnNewLogMessage("Creating audio pin");

				    AMMediaType mediaAudio = new AMMediaType();
				    mediaAudio.majorType = MediaType.Audio;
                    if (this.AudioDecoderType == ChannelDVB.AudioType.AC3)
                    {
                        mediaAudio.subType = MediaSubType.DolbyAC3;
                        OnNewLogMessage("Audio type: AC3");
                    }
                    else if (this.AudioDecoderType == ChannelDVB.AudioType.EAC3)
                    {
                        //EAC3
                        //http://social.msdn.microsoft.com/Forums/en-US/windowsdirectshowdevelopment/thread/64f5b2ef-9ec6-408c-9a86-6e1355bea717/
                        Guid MEDIASUBTYPE_DDPLUS_AUDIO = new Guid("a7fb87af-2d02-42fb-a4d4-05cd93843bdd");
                        mediaAudio.subType = MEDIASUBTYPE_DDPLUS_AUDIO;
                        //mediaAudio.subType = MediaSubType.DolbyAC3;
                        //Guid GuidDolbyEAC3 = new Guid("{33434145-0000-0010-8000-00AA00389B71}");
                        //#define WAVE_FORMAT_DOLBY_EAC3 0x33434145
                        //DEFINE_GUID(MEDIASUBTYPE_DOLBY_EAC3, WAVE_FORMAT_DOLBY_EAC3, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
                        //mediaAudio.subType = MediaSubType.Mpeg2Audio;
                        OnNewLogMessage("Audio type: EAC3");
                    }
                    else if (this.AudioDecoderType == ChannelDVB.AudioType.AAC)
                    {
                        mediaAudio.subType = new Guid("00001602-0000-0010-8000-00AA00389B71"); //GUID do mp4
                        OnNewLogMessage("Audio type: AAC Audio");
                    }
                    else
                    {
                        mediaAudio.subType = MediaSubType.Mpeg2Audio;
                        OnNewLogMessage("Audio type: MPEG2 Audio");
                    }
				    mediaAudio.sampleSize = 0;
				    mediaAudio.temporalCompression = false;
                    mediaAudio.fixedSizeSamples = false; // or false in MediaPortal //true
				    mediaAudio.unkPtr = IntPtr.Zero;
                    mediaAudio.formatType = FormatType.WaveEx;

				    MPEG1WaveFormat audioPinFormat = GetAudioPinFormat();
				    mediaAudio.formatSize = Marshal.SizeOf(audioPinFormat);
				    mediaAudio.formatPtr = Marshal.AllocHGlobal(mediaAudio.formatSize);
				    Marshal.StructureToPtr(audioPinFormat, mediaAudio.formatPtr, false);

				    IPin pinDemuxerAudio;
				    hr = mpeg2Demultiplexer.CreateOutputPin(mediaAudio, "Audio", out pinDemuxerAudio);
				    if (pinDemuxerAudio != null)
					    Marshal.ReleaseComObject(pinDemuxerAudio);

				    Marshal.FreeHGlobal(mediaAudio.formatPtr);

                    OnNewLogMessage(hr == 0 ? "Audio pin created successfully" : "Failed to create audio pin");
                    DsError.ThrowExceptionForHR(hr);
			    }

			    {
				    //Pin 5 connected to "MPEG-2 Sections and Tables" (Allows to grab custom PSI tables)
				    //    Major Type	MEDIATYPE_MPEG2_SECTIONS {455F176C-4B06-47CE-9AEF-8CAEF73DF7B5}
				    //    Sub Type		MEDIASUBTYPE_MPEG2DATA {C892E55B-252D-42B5-A316-D997E7A5D995}
				    //    Format		None
                    OnNewLogMessage("Creating \"Sections and Tables (PSI)\" pin");

				    AMMediaType mediaSectionsAndTables = new AMMediaType();
				    mediaSectionsAndTables.majorType = MediaType.Mpeg2Sections;
				    mediaSectionsAndTables.subType = MediaSubType.Mpeg2Data;
				    mediaSectionsAndTables.sampleSize = 0; // 1;
				    mediaSectionsAndTables.temporalCompression = false;
				    mediaSectionsAndTables.fixedSizeSamples = true;
				    mediaSectionsAndTables.unkPtr = IntPtr.Zero;
				    mediaSectionsAndTables.formatType = FormatType.None;
				    mediaSectionsAndTables.formatSize = 0;
				    mediaSectionsAndTables.formatPtr = IntPtr.Zero;

				    IPin pinDemuxerSectionsAndTables;
				    hr = mpeg2Demultiplexer.CreateOutputPin(mediaSectionsAndTables, "PSI", out pinDemuxerSectionsAndTables);
				    if (pinDemuxerSectionsAndTables != null)
					    Marshal.ReleaseComObject(pinDemuxerSectionsAndTables);

                    OnNewLogMessage(hr == 0 ? "\"Sections and Tables (PSI)\" pin created successfully" : "Failed to create \"Sections and Tables (PSI)\" pin");
                    DsError.ThrowExceptionForHR(hr);
			    }


                OnNewLogMessage("MPEG2 demux pins created successfully");
            }
            catch (Exception ex)
            {
                OnNewLogMessage("Failed to create MPEG2 demux pins");
                LogException(ex);
            }

            return hr;
		}
        protected virtual int CreateMPEG2DemuxPins2(TV2Lib.ChannelDVB.VideoType videoSubtype, TV2Lib.ChannelDVB.AudioType audioSubtype)
        {
            int hr = 0;

            try
            {
                OnNewLogMessage("Creating MPEG2 demux pins");

                IMpeg2Demultiplexer mpeg2Demultiplexer = this.mpeg2Demux as IMpeg2Demultiplexer;

                {
                    //Pin 1 connected to the "BDA MPEG2 Transport Information Filter"
                    //    Major Type	MEDIATYPE_MPEG2_SECTIONS {455F176C-4B06-47CE-9AEF-8CAEF73DF7B5}
                    //    Sub Type		MEDIASUBTYPE_DVB_SI {E9DD31A3-221D-4ADB-8532-9AF309C1A408}
                    //    Format		None

                    //    MPEG2 PSI Sections
                    //    Pids: 0x00 0x10 0x11 0x12 0x13 0x14 0x6e 0xd2 0x0136 0x019a 0x01fe 0x0262 0x03f2

                    OnNewLogMessage("Creating TIF pin");

                    AMMediaType mediaTIF = new AMMediaType();
                    mediaTIF.majorType = MediaType.Mpeg2Sections;
                    mediaTIF.subType = MediaSubType.DvbSI;
                    mediaTIF.fixedSizeSamples = false;
                    mediaTIF.temporalCompression = false;
                    mediaTIF.sampleSize = 1;
                    mediaTIF.unkPtr = IntPtr.Zero;
                    mediaTIF.formatType = FormatType.None;
                    mediaTIF.formatSize = 0;
                    mediaTIF.formatPtr = IntPtr.Zero;

                    IPin pinDemuxerTIF;
                    hr = mpeg2Demultiplexer.CreateOutputPin(mediaTIF, "TIF", out pinDemuxerTIF);
                    if (pinDemuxerTIF != null)
                        Marshal.ReleaseComObject(pinDemuxerTIF);

                    OnNewLogMessage(hr == 0 ? "TIF pin created successfully" : "Failed to create TIF pin");
                    DsError.ThrowExceptionForHR(hr);
                }
                if (videoSubtype == ChannelDVB.VideoType.H264)
                {
                    OnNewLogMessage("Creating H264 pin");

                    //Try Original
                    AMMediaType mediaH264 = new AMMediaType();
                    mediaH264.majorType = MediaType.Video;
                    //mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
                    mediaH264.subType = MediaSubType.H264;// new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
                    mediaH264.sampleSize = 0;
                    mediaH264.temporalCompression = true; // false;
                    mediaH264.fixedSizeSamples = true; // false;
                    mediaH264.unkPtr = IntPtr.Zero;
                    mediaH264.formatType = FormatType.Mpeg2Video;

                    MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
                    mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
                    mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
                    Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

                    IPin pinDemuxerVideoH264;
                    hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
                    if (pinDemuxerVideoH264 != null)
                        Marshal.ReleaseComObject(pinDemuxerVideoH264);

                    Marshal.FreeHGlobal(mediaH264.formatPtr);

                    OnNewLogMessage(hr == 0 ? "H264 pin created successfully" : "Failed to create H264 pin");
                    DsError.ThrowExceptionForHR(hr);

                    #region Tries
                    ////Try 1
                    //AMMediaType mediaH264 = new AMMediaType();
                    //mediaH264.majorType = MediaType.Null;
                    ////mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
                    //mediaH264.subType = MediaSubType.H264;// new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
                    //mediaH264.sampleSize = 0;
                    //mediaH264.temporalCompression = true; // false;
                    //mediaH264.fixedSizeSamples = true; // false;
                    //mediaH264.unkPtr = IntPtr.Zero;
                    //mediaH264.formatType = FormatType.Null;

                    ////MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
                    ////mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
                    ////mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
                    ////Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

                    //IPin pinDemuxerVideoH264;
                    //int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
                    //if (pinDemuxerVideoH264 != null)
                    //    Marshal.ReleaseComObject(pinDemuxerVideoH264);

                    ////Marshal.FreeHGlobal(mediaH264.formatPtr);


                    ////Try http://mheg2xmltv.googlecode.com/svn/trunk/dcdvbsource/Source/Filter/DVBGraphBuilder.pas
                    //AMMediaType mediaH264 = new AMMediaType();
                    //mediaH264.majorType = MediaType.Video;
                    ////mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
                    //mediaH264.subType = MediaSubType.H264;// new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
                    //mediaH264.sampleSize = 1; // 0;
                    //mediaH264.temporalCompression = true; // false;
                    //mediaH264.fixedSizeSamples = false; // true; // false;
                    //mediaH264.unkPtr = IntPtr.Zero;
                    //mediaH264.formatType = FormatType.VideoInfo2; // FormatType.VideoInfo2;//FormatType.Mpeg2Video;

                    //VideoInfoHeader2 videoH264PinFormat = GetVideoInfoHeader2H264PinFormat(); // GetVideoH264PinFormat();
                    //mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
                    //mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
                    //Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

                    //IPin pinDemuxerVideoH264;
                    //int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
                    //if (pinDemuxerVideoH264 != null)
                    //    Marshal.ReleaseComObject(pinDemuxerVideoH264);

                    //Marshal.FreeHGlobal(mediaH264.formatPtr);
                    #endregion
                }
                else
                {
                    OnNewLogMessage("Creating MPEG2 video pin");

                    AMMediaType mediaMPG2 = new AMMediaType();
                    mediaMPG2.majorType = MediaType.Video;
                    mediaMPG2.subType = MediaSubType.Mpeg2Video;
                    mediaMPG2.fixedSizeSamples = false;
                    mediaMPG2.temporalCompression = false; // true???
                    mediaMPG2.sampleSize = 0;
                    mediaMPG2.formatType = FormatType.Mpeg2Video;
                    mediaMPG2.unkPtr = IntPtr.Zero;

                    MPEG2VideoInfo videoMPEG2PinFormat = GetVideoMPEG2PinFormat();
                    mediaMPG2.formatSize = Marshal.SizeOf(videoMPEG2PinFormat);
                    mediaMPG2.formatPtr = Marshal.AllocHGlobal(mediaMPG2.formatSize);
                    Marshal.StructureToPtr(videoMPEG2PinFormat, mediaMPG2.formatPtr, false);

                    IPin pinDemuxerVideoMPEG2;
                    hr = mpeg2Demultiplexer.CreateOutputPin(mediaMPG2, "MPG2", out pinDemuxerVideoMPEG2);
                    if (pinDemuxerVideoMPEG2 != null)
                        Marshal.ReleaseComObject(pinDemuxerVideoMPEG2);

                    Marshal.FreeHGlobal(mediaMPG2.formatPtr);

                    OnNewLogMessage(hr == 0 ? "MPEG2 pin created successfully" : "Failed to create MPEG2 pin");
                    DsError.ThrowExceptionForHR(hr);
                }

                {
                    OnNewLogMessage("Creating audio pin");

                    AMMediaType mediaAudio = new AMMediaType();
                    mediaAudio.majorType = MediaType.Audio;
                    if (this.AudioDecoderType == ChannelDVB.AudioType.AC3)
                    {
                        mediaAudio.subType = MediaSubType.DolbyAC3;
                        OnNewLogMessage("Audio type: AC3");
                    }
                    else if (this.AudioDecoderType == ChannelDVB.AudioType.EAC3)
                    {
                        //EAC3
                        //http://social.msdn.microsoft.com/Forums/en-US/windowsdirectshowdevelopment/thread/64f5b2ef-9ec6-408c-9a86-6e1355bea717/
                        Guid MEDIASUBTYPE_DDPLUS_AUDIO = new Guid("a7fb87af-2d02-42fb-a4d4-05cd93843bdd");
                        mediaAudio.subType = MEDIASUBTYPE_DDPLUS_AUDIO;
                        //mediaAudio.subType = MediaSubType.DolbyAC3;
                        //Guid GuidDolbyEAC3 = new Guid("{33434145-0000-0010-8000-00AA00389B71}");
                        //#define WAVE_FORMAT_DOLBY_EAC3 0x33434145
                        //DEFINE_GUID(MEDIASUBTYPE_DOLBY_EAC3, WAVE_FORMAT_DOLBY_EAC3, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
                        //mediaAudio.subType = MediaSubType.Mpeg2Audio;
                        OnNewLogMessage("Audio type: EAC3");
                    }
                    else if (this.AudioDecoderType == ChannelDVB.AudioType.AAC)
                    {
                        mediaAudio.subType = new Guid("00001602-0000-0010-8000-00AA00389B71"); //GUID do mp4
                        OnNewLogMessage("Audio type: AAC Audio");
                    }
                    else
                    {
                        mediaAudio.subType = MediaSubType.Mpeg2Audio;
                        OnNewLogMessage("Audio type: MPEG2 Audio");
                    }
                    mediaAudio.sampleSize = 0;
                    mediaAudio.temporalCompression = false;
                    mediaAudio.fixedSizeSamples = false; // or false in MediaPortal //true
                    mediaAudio.unkPtr = IntPtr.Zero;
                    mediaAudio.formatType = FormatType.WaveEx;

                    MPEG1WaveFormat audioPinFormat = GetAudioPinFormat();
                    mediaAudio.formatSize = Marshal.SizeOf(audioPinFormat);
                    mediaAudio.formatPtr = Marshal.AllocHGlobal(mediaAudio.formatSize);
                    Marshal.StructureToPtr(audioPinFormat, mediaAudio.formatPtr, false);

                    IPin pinDemuxerAudio;
                    hr = mpeg2Demultiplexer.CreateOutputPin(mediaAudio, "Audio", out pinDemuxerAudio);
                    if (pinDemuxerAudio != null)
                        Marshal.ReleaseComObject(pinDemuxerAudio);

                    Marshal.FreeHGlobal(mediaAudio.formatPtr);

                    OnNewLogMessage(hr == 0 ? "Audio pin created successfully" : "Failed to create audio pin");
                    DsError.ThrowExceptionForHR(hr);
                }

                {
                    //Pin 5 connected to "MPEG-2 Sections and Tables" (Allows to grab custom PSI tables)
                    //    Major Type	MEDIATYPE_MPEG2_SECTIONS {455F176C-4B06-47CE-9AEF-8CAEF73DF7B5}
                    //    Sub Type		MEDIASUBTYPE_MPEG2DATA {C892E55B-252D-42B5-A316-D997E7A5D995}
                    //    Format		None
                    OnNewLogMessage("Creating \"Sections and Tables (PSI)\" pin");

                    AMMediaType mediaSectionsAndTables = new AMMediaType();
                    mediaSectionsAndTables.majorType = MediaType.Mpeg2Sections;
                    mediaSectionsAndTables.subType = MediaSubType.Mpeg2Data;
                    mediaSectionsAndTables.sampleSize = 0; // 1;
                    mediaSectionsAndTables.temporalCompression = false;
                    mediaSectionsAndTables.fixedSizeSamples = true;
                    mediaSectionsAndTables.unkPtr = IntPtr.Zero;
                    mediaSectionsAndTables.formatType = FormatType.None;
                    mediaSectionsAndTables.formatSize = 0;
                    mediaSectionsAndTables.formatPtr = IntPtr.Zero;

                    IPin pinDemuxerSectionsAndTables;
                    hr = mpeg2Demultiplexer.CreateOutputPin(mediaSectionsAndTables, "PSI", out pinDemuxerSectionsAndTables);
                    if (pinDemuxerSectionsAndTables != null)
                        Marshal.ReleaseComObject(pinDemuxerSectionsAndTables);

                    OnNewLogMessage(hr == 0 ? "\"Sections and Tables (PSI)\" pin created successfully" : "Failed to create \"Sections and Tables (PSI)\" pin");
                    DsError.ThrowExceptionForHR(hr);
                }


                OnNewLogMessage("MPEG2 demux pins created successfully");
            }
            catch (Exception ex)
            {
                OnNewLogMessage("Failed to create MPEG2 demux pins");
                LogException(ex);
            }

            return hr;
        }

		protected int AddAudioDecoderFilters(IFilterGraph2 graphBuilder)
		{
            int hr = 0;

            try
            {
                OnNewLogMessage("Adding audio decoder filters");

                if (this.AudioDecoderDevice != null)
                    hr = graphBuilder.AddSourceFilterForMoniker(this.AudioDecoderDevice.Mon, null, this.AudioDecoderDevice.Name, out this.audioDecoderFilter);
                else
                {
                    OnNewLogMessage("No audio decoder filter added");
                    return -1;
                }

                DsError.ThrowExceptionForHR(hr);

                OnNewLogMessage("Audio decoder filters added successfully");
                OnNewLogMessage("Device name: " + AudioDecoderDevice.Name);
            }
            catch(Exception ex)
            {
                OnNewLogMessage("Failed to add audio decoder filters");
                LogException(ex);
            }

            return hr;
		}

		protected int AddBDAVideoDecoderFilters(IFilterGraph2 graphBuilder)
		{
			int hr = 0;

            try
            {
                OnNewLogMessage("Adding video decoder filters");
                if (this.H264DecoderDevice != null)
                    hr = graphBuilder.AddSourceFilterForMoniker(this.H264DecoderDevice.Mon, null, this.H264DecoderDevice.Name, out this.videoH264DecoderFilter);
                else if (this.Mpeg2DecoderDevice != null)
                    hr = graphBuilder.AddSourceFilterForMoniker(this.Mpeg2DecoderDevice.Mon, null, this.Mpeg2DecoderDevice.Name, out this.videoMpeg2DecoderFilter);
                else
                {
                    OnNewLogMessage("No video decoder filters added");
                    return -1;
                }

                DsError.ThrowExceptionForHR(hr);

                OnNewLogMessage("Audio decoder filters added successfully");
                OnNewLogMessage("Device name: " + AudioDecoderDevice.Name);
            }
            catch (Exception ex)
            {
                OnNewLogMessage("Failed to add audio decoder filters");
                LogException(ex);
            }

            return hr;
		}
        protected int AddBDAVideoDecoderFilters2(IFilterGraph2 graphBuilder, TV2Lib.ChannelDVB.VideoType videoSubtype)
        {
            int hr = 0;

            try
            {
                OnNewLogMessage("Adding video decoder filters");


                if (videoSubtype == TV2Lib.ChannelDVB.VideoType.H264)
                {
                    OnNewLogMessage("Video subtype: H264");

                    if (this.H264DecoderDevice == null)
                    {
                        OnNewLogMessage("No H264 decoder provided. Choosing the first on the list");

                        DsDevice[] h264devs = DeviceEnumerator.GetH264Devices();
                        if (h264devs.Length <= 0)
                        {
                            OnNewLogMessage("No H264 decoders found.");
                            throw new ApplicationException("Could not find a suitable H264 decoder");
                        }
                        else this.H264DecoderDevice = h264devs[0];
                    }
                }
                else
                {
                    OnNewLogMessage("Video subtype: Other. Using MPEG2");

                    if (this.Mpeg2DecoderDevice == null)
                    {
                        OnNewLogMessage("No MPEG2 decoder provided. Choosing the first on the list");

                        DsDevice[] mpeg2devs = DeviceEnumerator.GetMPEG2Devices();
                        if (mpeg2devs.Length <= 0)
                        {
                            OnNewLogMessage("No MPEG2 decoders found.");
                            throw new ApplicationException("Could not find a suitable MPEG2 decoder");
                        }
                        else this.H264DecoderDevice = mpeg2devs[0];
                    }
                }


                if (this.H264DecoderDevice != null)
                    hr = graphBuilder.AddSourceFilterForMoniker(this.H264DecoderDevice.Mon, null, this.H264DecoderDevice.Name, out this.videoH264DecoderFilter);
                else if (this.Mpeg2DecoderDevice != null)
                    hr = graphBuilder.AddSourceFilterForMoniker(this.Mpeg2DecoderDevice.Mon, null, this.Mpeg2DecoderDevice.Name, out this.videoMpeg2DecoderFilter);
                else
                {
                    OnNewLogMessage("No video decoder filters added");
                    return -1;
                }

                DsError.ThrowExceptionForHR(hr);

                OnNewLogMessage("Video decoder filters added successfully");
                OnNewLogMessage("Device name: " + (videoSubtype == TV2Lib.ChannelDVB.VideoType.H264 ? this.H264DecoderDevice.Name : this.Mpeg2DecoderDevice.Name));
            }
            catch (Exception ex)
            {
                OnNewLogMessage("Failed to add audio decoder filters");
                LogException(ex);
            }

            return hr;
        }

        protected int AddAndConnectBDABoardFilters()
        {
            int hr = 0;
            try
            {
                OnNewLogMessage("Adding and connecting BDA board filters");

                bool tunerDeviceChosen = this.TunerDevice != null;
                bool captureDeviceChosen = this.CaptureDevice != null;

                if (tunerDeviceChosen) OnNewLogMessage(this.TunerDevice.Name + " selected");
                if (captureDeviceChosen) OnNewLogMessage(this.CaptureDevice.Name + " selected");

                DsDevice[] devices;

                ICaptureGraphBuilder2 capBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
                hr = capBuilder.SetFiltergraph(this.graphBuilder);
                DsError.ThrowExceptionForHR(hr);

                try
                {
                    // Enumerate BDA Source filters category and found one that can connect to the network provider
                    devices = DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory);

                    for (int i = 0; i < (tunerDeviceChosen ? 1 : devices.Length); i++)
                    {
                        IBaseFilter tmp;
                        DsDevice tempDevice = (tunerDeviceChosen ? this.TunerDevice : devices[i]);

                        OnNewLogMessage("Trying to add " + tempDevice.Name);

                        hr = graphBuilder.AddSourceFilterForMoniker(tempDevice.Mon, null, tempDevice.Name, out tmp);
                        DsError.ThrowExceptionForHR(hr);

                        hr = capBuilder.RenderStream(null, null, this.networkProvider, null, tmp);
                        if (hr == 0)
                        {
                            // Got it !
                            this.tuner = tmp;

                            OnNewLogMessage(tempDevice.Name + " added successfully");
                            break;
                        }
                        else
                        {
                            OnNewLogMessage("Failed to add " + tempDevice.Name);

                            // Try another...
                            hr = graphBuilder.RemoveFilter(tmp);
                            DsError.ThrowExceptionForHR(hr);

                            Marshal.ReleaseComObject(tmp);
                        }
                    }

                    if (this.tuner == null) throw new ApplicationException("Can't find a valid BDA tuner");

                    // trying to connect this filter to the MPEG-2 Demux
                    hr = capBuilder.RenderStream(null, null, tuner, null, mpeg2Demux);
                    if (hr >= 0)
                    {
                        OnNewLogMessage("One filter model");
                        OnNewLogMessage("BDA device connected successfully to the demux");

                        // this is a one filter model
                        this.demodulator = null;
                        this.capture = null;
                        return hr;
                    }
                    else
                    {
                        OnNewLogMessage("Possibly two filter model");

                        // Then enumerate BDA Receiver Components category to find a filter connecting 
                        // to the tuner and the MPEG2 Demux
                        devices = DsDevice.GetDevicesOfCat(FilterCategory.BDAReceiverComponentsCategory);

                        for (int i = 0; i < (captureDeviceChosen ? 1 : devices.Length); i++)
                        {
                            IBaseFilter tmp;
                            DsDevice tempCaptureDevice = (captureDeviceChosen ? this.CaptureDevice : devices[i]);

                            OnNewLogMessage("Trying to add " + tempCaptureDevice.Name);

                            hr = graphBuilder.AddSourceFilterForMoniker(tempCaptureDevice.Mon, null, tempCaptureDevice.Name, out tmp);
                            DsError.ThrowExceptionForHR(hr);

                            hr = capBuilder.RenderStream(null, null, this.tuner, null, tmp);
                            if (hr == 0)
                            {
                                OnNewLogMessage(tempCaptureDevice.Name + " connected to the tuner successfully");

                                // Got it !
                                this.capture = tmp;

                                // Connect it to the MPEG-2 Demux
                                hr = capBuilder.RenderStream(null, null, this.capture, null, this.mpeg2Demux);
                                if (hr >= 0)
                                {
                                    // This second filter connect both with the tuner and the demux.
                                    // This is a capture filter...

                                    OnNewLogMessage(tempCaptureDevice.Name + " added successfully");

                                    return hr;
                                }
                                else
                                {
                                    OnNewLogMessage("Possibly Three filter model");
                                    OnNewLogMessage("Finding a valid capture filter");

                                    // This second filter connect with the tuner but not with the demux.
                                    // This is in fact a demodulator filter. We now must find the true capture filter...
                                    this.demodulator = this.capture;
                                    this.capture = null;

                                    // saving the Demodulator's DevicePath to avoid creating it twice.
                                    string demodulatorDevicePath = devices[i].DevicePath;

                                    for (int j = 0; j < devices.Length; j++)
                                    {
                                        if (devices[j].DevicePath.Equals(demodulatorDevicePath))
                                            continue;

                                        OnNewLogMessage("Trying to add " + devices[j].Name);

                                        hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out tmp);
                                        DsError.ThrowExceptionForHR(hr);

                                        hr = capBuilder.RenderStream(null, null, this.demodulator, null, tmp);
                                        if (hr == 0)
                                        {
                                            // Got it !
                                            this.capture = tmp;

                                            // Connect it to the MPEG-2 Demux
                                            hr = capBuilder.RenderStream(null, null, this.capture, null, this.mpeg2Demux);
                                            if (hr >= 0)
                                            {
                                                // This second filter connect both with the demodulator and the demux.
                                                // This is a true capture filter...

                                                OnNewLogMessage(devices[j].Name + " added successfully");

                                                OnNewLogMessage("Added and connected BDA board filters successfully");
                                                return hr;
                                            }
                                        }
                                        else
                                        {

                                            // Try another...
                                            hr = graphBuilder.RemoveFilter(tmp);
                                            Marshal.ReleaseComObject(tmp);

                                            OnNewLogMessage("Failed to add " + devices[j].Name);
                                        }
                                    } // for j

                                    // We have a tuner and a capture/demodulator that don't connect with the demux
                                    // and we found no additionals filters to build a working filters chain.

                                    OnNewLogMessage("Can't find a valid BDA filter chain");
                                    throw new ApplicationException("Can't find a valid BDA filter chain");
                                }
                            }
                            else
                            {
                                // Try another...
                                hr = graphBuilder.RemoveFilter(tmp);
                                Marshal.ReleaseComObject(tmp);

                                OnNewLogMessage("Failed to add " + tempCaptureDevice.Name);
                            }
                        } // for i

                        // We have a tuner that connect to the Network Provider BUT not with the demux
                        // and we found no additionals filters to build a working filters chain.
                        OnNewLogMessage("Can't find a valid BDA filter chain");
                        throw new ApplicationException("Can't find a valid BDA filter chain");
                    }
                }
                finally
                {
                    Marshal.ReleaseComObject(capBuilder);
                }
            }
            catch (ApplicationException ex)
            {
                OnNewLogMessage("Failed to add and connect BDA board filters");
                LogException(ex);

                hr = -1;
            }
            catch (Exception ex)
            {
                OnNewLogMessage("Failed to add and connect BDA board filters");
                LogException(ex);
            }

            return hr;
        }

        #region Add And Connect BDA Board Filters antigo
        //protected void AddAndConnectBDABoardFilters()
        //{
        //    AddAndConnectBDABoardFilters2();
        //    return;

        //    //POR AGORA

        //    int hr = 0;
        //    DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory);

        //    this.captureGraphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
        //    captureGraphBuilder.SetFiltergraph(this.graphBuilder);

        //    //alterar para aceitar tuners com modelos de um filtro
        //    //

        //    try
        //    {
        //        if (this.TunerDevice != null)
        //        {
        //            #region this.TunerDevice != null
        //            IBaseFilter tmp;

        //            hr = graphBuilder.AddSourceFilterForMoniker(this.TunerDevice.Mon, null, this.TunerDevice.Name, out tmp);
        //            DsError.ThrowExceptionForHR(hr);

        //            hr = captureGraphBuilder.RenderStream(null, null, this.networkProvider, null, tmp);
        //            if (hr == 0)
        //            {
        //                // Got it !
        //                this.tuner = tmp;
        //            }
        //            else
        //            {
        //                int hr2 = graphBuilder.RemoveFilter(tmp);
        //                Marshal.ReleaseComObject(tmp);
        //                DsError.ThrowExceptionForHR(hr);
        //                return;
        //            }

        //            //tenta ligar o filtro ao demux. se ligar, é de um filtro e sai
        //            hr = captureGraphBuilder.RenderStream(null, null, tuner, null, mpeg2Demux);
        //            if (hr >= 0)
        //            {
        //                //modelo de um filtro
        //                this.demodulator = null;
        //                this.capture = null;
        //                return;
        //            }
        //            else
        //            {
        //                if (this.CaptureFilter != null)
        //                {
        //                    #region this.CaptureFilter != null
        //                    IBaseFilter tmp2;

        //                    hr = graphBuilder.AddSourceFilterForMoniker(CaptureDevice.Mon, null, CaptureDevice.Name, out tmp2);
        //                    DsError.ThrowExceptionForHR(hr);

        //                    hr = captureGraphBuilder.RenderStream(null, null, this.tuner, null, tmp2);
        //                    if (hr == 0) //os dois filtros são compatíveis
        //                    {
        //                        this.capture = tmp2;

        //                        hr = captureGraphBuilder.RenderStream(null, null, this.capture, null, mpeg2Demux);
        //                        if (hr == 0)
        //                        {
        //                            //já temos os filtros do tuner ligados entre eles e ao demux. podemos sair
        //                            return;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        this.demodulator = this.capture;
        //                        this.capture = null;

        //                        //guardar o devicepath do demodulator para nao o criar duas vezes

        //                        string demodulatorDevPath = CaptureDevice.DevicePath;

        //                        DsDevice[] devices2 = DsDevice.GetDevicesOfCat(FilterCategory.BDAReceiverComponentsCategory);

        //                        for (int j = 0; j < devices2.Length; j++)
        //                        {
        //                            if (devices2[j].DevicePath.Equals(demodulatorDevPath))
        //                                continue;

        //                            hr = graphBuilder.AddSourceFilterForMoniker(devices2[j].Mon, null, devices2[j].Name, out tmp2);
        //                            DsError.ThrowExceptionForHR(hr);

        //                            hr = captureGraphBuilder.RenderStream(null, null, this.demodulator, null, tmp2);
        //                            if (hr == 0)
        //                            {
        //                                this.capture = tmp2;

        //                                hr = captureGraphBuilder.RenderStream(null, null, this.capture, null, this.mpeg2Demux);
        //                                if (hr >= 0)
        //                                {
        //                                    //este filtro ligou com o demodulator e o demux. é o filtro d captura verdadeiro. kkthxbye
        //                                    return;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                hr = graphBuilder.RemoveFilter(tmp2);
        //                                Marshal.ReleaseComObject(tmp2);
        //                            }
        //                        } //for j

        //                        //Encontrámos uma cadeia de filtros que não liga com o demux, 
        //                        //e não foi encontrada uma combinação de filtros que funcione
        //                        throw new ApplicationException("Can't find a valid BDA filter chain");
        //                    }
        //                    #endregion
        //                }
        //                else
        //                {
        //                }
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            // Enumerate BDA Source filters category and found one that can connect to the network provider
        //            devices = DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory);
        //            for (int i = 0; i < devices.Length; i++)
        //            {
        //                IBaseFilter tmp;

        //                hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out tmp);
        //                DsError.ThrowExceptionForHR(hr);

        //                hr = captureGraphBuilder.RenderStream(null, null, this.networkProvider, null, tmp);
        //                if (hr == 0)
        //                {
        //                    // Got it !
        //                    this.tuner = tmp;
        //                    break;
        //                }
        //                else
        //                {
        //                    // Try another...
        //                    hr = graphBuilder.RemoveFilter(tmp);
        //                    Marshal.ReleaseComObject(tmp);
        //                }
        //            }
        //        }
        //        // Assume we found a tuner filter...

        //        if (this.CaptureDevice != null)
        //        {
        //            #region this.CaptureFilter != null
        //            IBaseFilter tmp2;

        //            hr = graphBuilder.AddSourceFilterForMoniker(CaptureDevice.Mon, null, CaptureDevice.Name, out tmp2);
        //            DsError.ThrowExceptionForHR(hr);

        //            hr = captureGraphBuilder.RenderStream(null, null, this.tuner, null, tmp2);
        //            if (hr == 0) //os dois filtros são compatíveis
        //            {
        //                this.capture = tmp2;

        //                hr = captureGraphBuilder.RenderStream(null, null, this.capture, null, mpeg2Demux);
        //                if (hr == 0)
        //                {
        //                    //já temos os filtros do tuner ligados entre eles e ao demux. podemos sair
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                this.demodulator = this.capture;
        //                this.capture = null;

        //                //guardar o devicepath do demodulator para nao o criar duas vezes

        //                string demodulatorDevPath = CaptureDevice.DevicePath;

        //                DsDevice[] devices2 = DsDevice.GetDevicesOfCat(FilterCategory.BDAReceiverComponentsCategory);

        //                for (int j = 0; j < devices2.Length; j++)
        //                {
        //                    if (devices2[j].DevicePath.Equals(demodulatorDevPath))
        //                        continue;

        //                    hr = graphBuilder.AddSourceFilterForMoniker(devices2[j].Mon, null, devices2[j].Name, out tmp2);
        //                    DsError.ThrowExceptionForHR(hr);

        //                    hr = captureGraphBuilder.RenderStream(null, null, this.demodulator, null, tmp2);
        //                    if (hr == 0)
        //                    {
        //                        this.capture = tmp2;

        //                        hr = captureGraphBuilder.RenderStream(null, null, this.capture, null, this.mpeg2Demux);
        //                        if (hr >= 0)
        //                        {
        //                            //este filtro ligou com o demodulator e o demux. é o filtro d captura verdadeiro. kkthxbye
        //                            return;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        hr = graphBuilder.RemoveFilter(tmp2);
        //                        Marshal.ReleaseComObject(tmp2);
        //                    }
        //                } //for j

        //                //Encontrámos uma cadeia de filtros que não liga com o demux, 
        //                //e não foi encontrada uma combinação de filtros que funcione
        //                throw new ApplicationException("Can't find a valid BDA filter chain");
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            this.capture = null;

        //            // Connect it to the MPEG-2 Demux
        //            hr = captureGraphBuilder.RenderStream(null, null, this.tuner, null, this.mpeg2Demux);
        //            if (hr < 0)
        //                // BDA also support 3 filter scheme (never show it in real life).
        //                throw new ApplicationException("This application only support the 2 filters BDA scheme");
        //        }
        //    }
        //    finally
        //    {
        //    }
        //} //Refazer este método.
        #endregion

        protected int AddTransportStreamFiltersToGraph()
		{
            OnNewLogMessage("Adding transport stream filters to graph");
			int hr = 0;

            try
            {
			    DsDevice[] devices;
			    // Add two filters needed in a BDA graph
			    devices = DsDevice.GetDevicesOfCat(FilterCategory.BDATransportInformationRenderersCategory);
			    for (int i = 0; i < devices.Length; i++)
			    {
				    if (devices[i].Name.Equals("BDA MPEG2 Transport Information Filter"))
				    {
                        OnNewLogMessage("Adding \"BDA MPEG2 Transport Information Filter\"");
					    hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out this.bdaTIF);
                        OnNewLogMessage(hr == 0 ? "\"BDA MPEG2 Transport Information Filter\" added successfully" : "Failed to add \"BDA MPEG2 Transport Information Filter\"");
					    DsError.ThrowExceptionForHR(hr);
					    continue;
				    }

				    if (devices[i].Name.Equals("MPEG-2 Sections and Tables"))
				    {
                        OnNewLogMessage("Adding \"MPEG-2 Sections and Tables\"");
					    hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out this.bdaSecTab);
                        OnNewLogMessage(hr == 0 ? "\"MPEG-2 Sections and Tables\" added successfully" : "Failed to add \"MPEG-2 Sections and Tables\"");
					    DsError.ThrowExceptionForHR(hr);
					    continue;
				    }
			    }

                OnNewLogMessage("Transport stream filters added successfully");
            }
            catch(Exception ex)
            {
                OnNewLogMessage("Failed to add transport stream filters");
                LogException(ex);
            }

            return hr;
		}

		protected int AddAndConnectTIFToGraph()
		{
			int hr = 0;
			IPin pinOut;
			DsDevice[] devices;

            try
            {
                OnNewLogMessage("Adding TIF filter to graph");

			    devices = DsDevice.GetDevicesOfCat(FilterCategory.BDATransportInformationRenderersCategory);

			    for (int i = 0; i < devices.Length; i++)
			    {
				    if (devices[i].Name.Equals("BDA MPEG2 Transport Information Filter"))
				    {
					    hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out this.bdaTIF);
					    DsError.ThrowExceptionForHR(hr);

					    // Connect the MPEG-2 Demux output pin for the "BDA MPEG2 Transport Information Filter"
					    hr = this.mpeg2Demux.FindPin("TIF", out pinOut);
					    if (pinOut != null)
					    {
                            hr = 0;

						    IPin pinIn = DsFindPin.ByDirection(this.bdaTIF, PinDirection.Input, 0);
						    if (pinIn != null)
						    {
                                OnNewLogMessage("Connecting TIF filter");

							    hr = this.graphBuilder.Connect(pinOut, pinIn);
                                DsError.ThrowExceptionForHR(hr);

							    Marshal.ReleaseComObject(pinIn);

                                OnNewLogMessage("TIF filter added and connected successfully");
                                break;
						    }

						    // In fact the last pin don't render since i havn't added the BDA MPE Filter...
						    Marshal.ReleaseComObject(pinOut);
					    }
				    }
                }
            }
            catch (Exception ex)
            {
                OnNewLogMessage("Failed to add and connect TIF filter");
                LogException(ex);
            }

            return hr;
		}
		protected int AddAndConnectSectionsAndTablesFilterToGraph()
		{
			int hr = 0;
			IPin pinOut;
			DsDevice[] devices;

            try
            {
                OnNewLogMessage("Adding Sections And Tables filter to graph");

                devices = DsDevice.GetDevicesOfCat(FilterCategory.BDATransportInformationRenderersCategory);
                for (int i = 0; i < devices.Length; i++)
                {
                    if (devices[i].Name.Equals("MPEG-2 Sections and Tables"))
                    {
                        hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out this.bdaSecTab);
                        DsError.ThrowExceptionForHR(hr);

                        // Connect the MPEG-2 Demux output pin for the "MPEG-2 Sections and Tables" filter
                        hr = this.mpeg2Demux.FindPin("PSI", out pinOut);
                        if (pinOut != null)
                        {
                            IPin pinIn = DsFindPin.ByDirection(this.bdaSecTab, PinDirection.Input, 0);
                            if (pinIn != null)
                            {
                                OnNewLogMessage("Connecting Sections And Tables filter");

                                hr = this.graphBuilder.Connect(pinOut, pinIn);
                                DsError.ThrowExceptionForHR(hr);

                                Marshal.ReleaseComObject(pinIn);

                                OnNewLogMessage("Sections And Tables filter added and connected successfully");
                                break;
                            }

                            // In fact the last pin don't render since i havn't added the BDA MPE Filter...
                            Marshal.ReleaseComObject(pinOut);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnNewLogMessage("Failed to add and connect Sections And Tables filter");
                LogException(ex);
            }

            return hr;
		}

		protected int ConnectAllOutputFilters()
        {
            int hr = 0;
            OnNewLogMessage("Connecting the output filters");

            try
            {
                // Get the pin enumerator
                IEnumPins ppEnum;
                hr = this.mpeg2Demux.EnumPins(out ppEnum);
                DsError.ThrowExceptionForHR(hr);

                try
                {
                    // Walk the pins looking for a match
                    IPin[] pPins = new IPin[1];
                    //22 int lFetched;
                    //22 while ((ppEnum.Next(1, pPins, out lFetched) >= 0) && (lFetched == 1))
                    while (ppEnum.Next(1, pPins, IntPtr.Zero) >= 0)
                    {
                        // Read the direction
                        PinDirection ppindir;
                        PinInfo pinInfo;
                        FilterInfo filterInfo;

                        pPins[0].QueryPinInfo(out pinInfo);
                        pinInfo.filter.QueryFilterInfo(out filterInfo);

                        hr = pPins[0].QueryDirection(out ppindir);
                        DsError.ThrowExceptionForHR(hr);

                        // Is it the right direction?
                        if (ppindir == PinDirection.Output)
                        {
                            if (pPins[0] != null)
                            {
                                if (!string.IsNullOrEmpty(pinInfo.name) && !string.IsNullOrEmpty(filterInfo.achName))
                                    OnNewLogMessage(string.Format("Rendering pin \"{0}\" from filter \"{1}\"", pinInfo.name, filterInfo.achName));
                                else OnNewLogMessage("Rendering pin");

                                hr = this.graphBuilder.Render(pPins[0]);

                                if (!string.IsNullOrEmpty(pinInfo.name) && !string.IsNullOrEmpty(filterInfo.achName))
                                {
                                    if (hr == 0)
                                        OnNewLogMessage(string.Format("Pin \"{0}\" from filter \"{1}\" rendered successfully", pinInfo.name, filterInfo.achName));
                                    else
                                        OnNewLogMessage(string.Format("Failed to render pin \"{0}\" from filter \"{1}\"", pinInfo.name, filterInfo.achName));
                                }
                                else
                                {
                                    if (hr == 0)
                                        OnNewLogMessage("Pin rendered successfully");
                                    else
                                        OnNewLogMessage("Failed to render pin");
                                }
                                //DsError.ThrowExceptionForHR(hr);
                                // In fact the last pin don't render since i havn't added the BDA MPE Filter...
                            }
                        }
                        Marshal.ReleaseComObject(pPins[0]);
                    }
                }
                finally
                {
                    Marshal.ReleaseComObject(ppEnum);
                }
            }
            catch (Exception ex)
            {
                OnNewLogMessage("Failed to render output pins");
                LogException(ex);
            }

            return hr;
		}

		//protected void ConnectAudioAndVideoFilters()
		//{
		//    int hr = 0;
		//    IPin pinOut;

		//    hr = this.mpeg2Demux.FindPin("H264", out pinOut);
		//    if (pinOut != null)
		//    {
		//        IPin pinInFromFilterOut = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
		//        if (pinInFromFilterOut != null)
		//        {
		//            hr = this.graphBuilder.Connect(pinOut, pinInFromFilterOut);
		//            Marshal.ReleaseComObject(pinInFromFilterOut);
		//        }

		//        //hr = this.graphBuilder.Render(pinOut);
		//        ////DsError.ThrowExceptionForHR(hr);
		//        // In fact the last pin don't render since i havn't added the BDA MPE Filter...
		//        Marshal.ReleaseComObject(pinOut);
		//    }

		//    hr = this.mpeg2Demux.FindPin("MPG2", out pinOut);
		//    if (pinOut != null)
		//    {
		//        //hr = this.graphBuilder.Render(pinOut);
		//        IPin pinInFromFilterOut = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
		//        if (pinInFromFilterOut != null)
		//        {
		//            hr = this.graphBuilder.Connect(pinOut, pinInFromFilterOut);
		//            Marshal.ReleaseComObject(pinInFromFilterOut);
		//        }
		//        ////DsError.ThrowExceptionForHR(hr);
		//        // In fact the last pin don't render since i havn't added the BDA MPE Filter...
		//        Marshal.ReleaseComObject(pinOut);
		//    }

		//    if (useWPF)
		//    {
		//        IPin pinOutFromFilterOut = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Output, 0);
		//        if (pinOutFromFilterOut != null)
		//        {
		//            hr = this.graphBuilder.Render(pinOutFromFilterOut);
		//            Marshal.ReleaseComObject(pinOutFromFilterOut);
		//        }
		//    }

		//    hr = this.mpeg2Demux.FindPin("Audio", out pinOut);
		//    if (pinOut != null)
		//    {
		//        hr = this.graphBuilder.Render(pinOut);
		//        //DsError.ThrowExceptionForHR(hr);
		//        // In fact the last pin don't render since i havn't added the BDA MPE Filter...
		//        Marshal.ReleaseComObject(pinOut);
		//    }
		//}

		protected void ConnectAudioAndVideoFilters()
		{
			int hr = 0;
			IPin pinOut;

			hr = this.mpeg2Demux.FindPin("H264", out pinOut);
			if (pinOut != null)
			{
				try
				{
					if (this.videoH264DecoderFilter == null) //se não for dado um decoder, usa o intelliconnect
					{
						IPin pinInFromFilterOut = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
						if (pinInFromFilterOut != null)
						{
							try
							{
								hr = this.graphBuilder.Connect(pinOut, pinInFromFilterOut);
							}
							finally
							{
								Marshal.ReleaseComObject(pinInFromFilterOut);
							}
						}
					}
					else
					{
						IPin videoDecoderIn = null;
						try
						{
							videoDecoderIn = DsFindPin.ByDirection(this.videoH264DecoderFilter, PinDirection.Input, 0);

                            //AMMediaType mediaH264 = new AMMediaType();
                            //mediaH264.majorType = MediaType.Video;
                            ////mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
                            //mediaH264.subType = MediaSubType.H264; // new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
                            //mediaH264.sampleSize = 0;
                            //mediaH264.temporalCompression = true; // false;
                            //mediaH264.fixedSizeSamples = true; // false;
                            //mediaH264.unkPtr = IntPtr.Zero;
                            //mediaH264.formatType = FormatType.Mpeg2Video;

                            //MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
                            //mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
                            //mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
                            //Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

                            ////IPin pinDemuxerVideoH264;
                            ////int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
                            ////if (pinDemuxerVideoH264 != null)
                            ////Marshal.ReleaseComObject(pinDemuxerVideoH264);
                            //hr = this.graphBuilder.ConnectDirect(pinOut, videoDecoderIn, mediaH264);
                            ////hr = this.graphBuilder2.Connect(videoDvrOut, videoDecoderIn);
                            ////DsError.ThrowExceptionForHR(hr);

                            //Marshal.FreeHGlobal(mediaH264.formatPtr);

                            //if (hr != 0)
    							FilterGraphTools.ConnectFilters(this.graphBuilder, pinOut, videoDecoderIn, false);
						}
						finally
						{
							if (videoDecoderIn != null) Marshal.ReleaseComObject(videoDecoderIn);
						}

						IPin videoDecoderOut = null, videoVMRIn = null;
						try
						{
							videoDecoderOut = DsFindPin.ByDirection(this.videoH264DecoderFilter, PinDirection.Output, 0);
							videoVMRIn = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
							FilterGraphTools.ConnectFilters(this.graphBuilder, videoDecoderOut, videoVMRIn, false);
						}
						finally
						{
							if (videoDecoderOut != null) Marshal.ReleaseComObject(videoDecoderOut);
							if (videoVMRIn != null) Marshal.ReleaseComObject(videoVMRIn);
						}
					}
				}
				finally
				{
					Marshal.ReleaseComObject(pinOut);
				}
			}

			hr = this.mpeg2Demux.FindPin("MPG2", out pinOut);
			if (pinOut != null)
			{
				try
				{
					if (this.videoMpeg2DecoderFilter == null)
					{
						IPin pinInFromFilterOut = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
						if (pinInFromFilterOut != null)
						{
							try
							{
								hr = this.graphBuilder.Connect(pinOut, pinInFromFilterOut);
							}
							finally
							{
								Marshal.ReleaseComObject(pinInFromFilterOut);
							}
						}
					}
					else
					{

						IPin videoDecoderIn = null;
						try
						{
							videoDecoderIn = DsFindPin.ByDirection(this.videoMpeg2DecoderFilter, PinDirection.Input, 0);

							FilterGraphTools.ConnectFilters(this.graphBuilder, pinOut, videoDecoderIn, false);
						}
						finally
						{
							if (videoDecoderIn != null) Marshal.ReleaseComObject(videoDecoderIn);
						}

						IPin videoDecoderOut = null, videoVMRIn = null;
						try
						{
							videoDecoderOut = DsFindPin.ByDirection(this.videoMpeg2DecoderFilter, PinDirection.Output, 0);
							videoVMRIn = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
							//FilterGraphTools.ConnectFilters(this.graphBuilder, videoDecoderOut, videoVMRIn, false);
							hr = graphBuilder.ConnectDirect(videoDecoderOut, videoVMRIn, null);
							ThrowExceptionForHR(string.Format("Connecting the video decoder ({0}) to the video renderer ({1}): ", this.Mpeg2DecoderDevice != null ? this.Mpeg2DecoderDevice.Name : "", /*this.useWPF ? "SampleGraber" :*/ "VMR9"), hr);
						}
						finally
						{
							if (videoDecoderOut != null) Marshal.ReleaseComObject(videoDecoderOut);
							if (videoVMRIn != null) Marshal.ReleaseComObject(videoVMRIn);
						}
					}
				}
				finally
				{
					Marshal.ReleaseComObject(pinOut);
				}
			}

            //AddAndConnectNullRendererForWPF();

			hr = this.mpeg2Demux.FindPin("Audio", out pinOut);
			if (pinOut != null)
			{
				hr = this.graphBuilder.Render(pinOut);
				//DsError.ThrowExceptionForHR(hr);
				Marshal.ReleaseComObject(pinOut);
			}
		}

		protected int ConnectTransportStreamFilters()
		{
			int hr = 0;
			IPin pinOut;

            OnNewLogMessage("Connecting transport stream filters");
            try
            {
                // Connect the MPEG-2 Demux output pin for the "BDA MPEG2 Transport Information Filter"
                //pinOut = DsFindPin.ByDirection(this.mpeg2Demux, PinDirection.Output, 0);
                hr = this.mpeg2Demux.FindPin("1", out pinOut);
                if (pinOut != null)
                {
                    hr = this.graphBuilder.Render(pinOut);
                    //DsError.ThrowExceptionForHR(hr);
                    // In fact the last pin don't render since i havn't added the BDA MPE Filter...
                    Marshal.ReleaseComObject(pinOut);
                }

                // Connect the MPEG-2 Demux output pin for the "MPEG-2 Sections and Tables" filter
                //pinOut = DsFindPin.ByDirection(this.mpeg2Demux, PinDirection.Output, 4);
                hr = this.mpeg2Demux.FindPin("5", out pinOut);
                if (pinOut != null)
                {
                    hr = this.graphBuilder.Render(pinOut);
                    //DsError.ThrowExceptionForHR(hr);
                    // In fact the last pin don't render since i havn't added the BDA MPE Filter...
                    Marshal.ReleaseComObject(pinOut);
                }
            }
            catch(Exception ex)
            {
                OnNewLogMessage("Failed to connect transport stream filters");
                LogException(ex);
            }

            return hr;
		}

		protected int RenderMpeg2DemuxPins()
		{
			int hr = 0;
			IPin pinOut;
            try
            {
                OnNewLogMessage("Rendering MPEG2 demux pins");
                // Connect the 5 MPEG-2 Demux output pins
                for (int i = 0; i < 6; i++)
                {
                    pinOut = DsFindPin.ByDirection(this.mpeg2Demux, PinDirection.Output, i);

                    if (pinOut != null)
                    {
                        PinInfo info;
                        int hrInfo = pinOut.QueryPinInfo(out info);
                        if (hrInfo == 0)
                            OnNewLogMessage(string.Format("Rendering pin \"{0}\"", info.name));

                        hr = this.graphBuilder.Render(pinOut);
                        if(hr == 0)
                            OnNewLogMessage(string.Format("Pin {0}rendered successfully", (hrInfo == 0 ? (info.name + " " ?? "") : "")));
                        //DsError.ThrowExceptionForHR(hr);
                        // In fact the last pin don't render since i havn't added the BDA MPE Filter...
                        Marshal.ReleaseComObject(pinOut);
                    }
                }
            }
            catch (Exception ex)
            {
                OnNewLogMessage("Failed to render MPEG2 demux pins");
                LogException(ex);
            }

            return hr;
		}

		protected static MPEG2VideoInfo videoMPEG2PinFormat;
		protected static MPEG2VideoInfo videoH264PinFormat;
		protected static MPEG1WaveFormat audioPinFormat;

		protected static MPEG2VideoInfo GetVideoMPEG2PinFormat()
		{
			if (videoMPEG2PinFormat == null)
			{
				MPEG2VideoInfo videoPinFormat = new MPEG2VideoInfo();
				videoPinFormat.hdr = new VideoInfoHeader2();
				videoPinFormat.hdr.SrcRect = new DsRect();
				videoPinFormat.hdr.SrcRect.left = 0;		//0x00, 0x00, 0x00, 0x00,  //00  .hdr.rcSource.left              = 0x00000000
				videoPinFormat.hdr.SrcRect.top = 0;			//0x00, 0x00, 0x00, 0x00,  //04  .hdr.rcSource.top               = 0x00000000
				videoPinFormat.hdr.SrcRect.right = 0;		//0xD0, 0x02, 0x00, 0x00,  //08  .hdr.rcSource.right             = 0x000002d0 //720
				videoPinFormat.hdr.SrcRect.bottom = 0;		//0x40, 0x02, 0x00, 0x00,  //0c  .hdr.rcSource.bottom            = 0x00000240 //576
				videoPinFormat.hdr.TargetRect = new DsRect();
				videoPinFormat.hdr.TargetRect.left = 0;		//0x00, 0x00, 0x00, 0x00,  //10  .hdr.rcTarget.left              = 0x00000000
				videoPinFormat.hdr.TargetRect.top = 0;		//0x00, 0x00, 0x00, 0x00,  //14  .hdr.rcTarget.top               = 0x00000000
				videoPinFormat.hdr.TargetRect.right = 0;	//0xD0, 0x02, 0x00, 0x00,  //18  .hdr.rcTarget.right             = 0x000002d0 //720
				videoPinFormat.hdr.TargetRect.bottom = 0;	//0x40, 0x02, 0x00, 0x00,  //1c  .hdr.rcTarget.bottom            = 0x00000240// 576
				videoPinFormat.hdr.BitRate = 0x003d0900;	//0x00, 0x09, 0x3D, 0x00,  //20  .hdr.dwBitRate                  = 0x003d0900
				videoPinFormat.hdr.BitErrorRate = 0;		//0x00, 0x00, 0x00, 0x00,  //24  .hdr.dwBitErrorRate             = 0x00000000

				////0x051736=333667-> 10000000/333667 = 29.97fps
				////0x061A80=400000-> 10000000/400000 = 25fps
				videoPinFormat.hdr.AvgTimePerFrame = 400000;				//0x80, 0x1A, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, //28  .hdr.AvgTimePerFrame            = 0x0000000000051763 ->1000000/ 40000 = 25fps

				videoPinFormat.hdr.InterlaceFlags = AMInterlace.None;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
				////videoPinFormat.hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.Field1First; // | AMINTERLACE_DisplayModeBobOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
				//videoPinFormat.hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.OneFieldPerSample | AMInterlace.DisplayModeBobOnly; // | AMINTERLACE_DisplayModeBobOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
				//videoPinFormat.hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.DisplayModeBobOnly; // | AMINTERLACE_DisplayModeBobOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
				//videoPinFormat.hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.FieldPatBothRegular | AMInterlace.DisplayModeWeaveOnly; // | AMINTERLACE_DisplayModeBobOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
				//videoPinFormat.hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.DisplayModeBobOrWeave; // | AMINTERLACE_DisplayModeBobOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
				//videoPinFormat.hdr.InterlaceFlags |= AMInterlace.Field1First;

				videoPinFormat.hdr.CopyProtectFlags = AMCopyProtect.None;	//0x00, 0x00, 0x00, 0x00,                         //30  .hdr.dwCopyProtectFlags         = 0x00000000
                videoPinFormat.hdr.PictAspectRatioX = 16; // 4;					//0x04, 0x00, 0x00, 0x00,                         //34  .hdr.dwPictAspectRatioX         = 0x00000004
                videoPinFormat.hdr.PictAspectRatioY = 9; // 3;					//0x03, 0x00, 0x00, 0x00,                         //38  .hdr.dwPictAspectRatioY         = 0x00000003
				videoPinFormat.hdr.ControlFlags = AMControl.None;			//0x00, 0x00, 0x00, 0x00,                         //3c  .hdr.dwReserved1                = 0x00000000
				videoPinFormat.hdr.Reserved2 = 0;							//0x00, 0x00, 0x00, 0x00,                         //40  .hdr.dwReserved2                = 0x00000000
				videoPinFormat.hdr.BmiHeader = new BitmapInfoHeader();
				videoPinFormat.hdr.BmiHeader.Size = 0x00000028;				//0x28, 0x00, 0x00, 0x00,  //44  .hdr.bmiHeader.biSize           = 0x00000028
				videoPinFormat.hdr.BmiHeader.Width = 720;					//0xD0, 0x02, 0x00, 0x00,  //48  .hdr.bmiHeader.biWidth          = 0x000002d0 //720
				videoPinFormat.hdr.BmiHeader.Height = 576;					//0x40, 0x02, 0x00, 0x00,  //4c  .hdr.bmiHeader.biHeight         = 0x00000240 //576
				videoPinFormat.hdr.BmiHeader.Planes = 0;					//0x00, 0x00,              //50  .hdr.bmiHeader.biPlanes         = 0x0000
				videoPinFormat.hdr.BmiHeader.BitCount = 0;					//0x00, 0x00,              //54  .hdr.bmiHeader.biBitCount       = 0x0000
				videoPinFormat.hdr.BmiHeader.Compression = 0;				//0x00, 0x00, 0x00, 0x00,  //58  .hdr.bmiHeader.biCompression    = 0x00000000
				videoPinFormat.hdr.BmiHeader.ImageSize = 0;					//0x00, 0x00, 0x00, 0x00,  //5c  .hdr.bmiHeader.biSizeImage      = 0x00000000
				videoPinFormat.hdr.BmiHeader.XPelsPerMeter = 0x000007d0;	//0xD0, 0x07, 0x00, 0x00,  //60  .hdr.bmiHeader.biXPelsPerMeter  = 0x000007d0
				videoPinFormat.hdr.BmiHeader.YPelsPerMeter = 0x0000cf27;	//0x27, 0xCF, 0x00, 0x00,  //64  .hdr.bmiHeader.biYPelsPerMeter  = 0x0000cf27
				videoPinFormat.hdr.BmiHeader.ClrUsed = 0;					//0x00, 0x00, 0x00, 0x00,  //68  .hdr.bmiHeader.biClrUsed        = 0x00000000
				videoPinFormat.hdr.BmiHeader.ClrImportant = 0;				//0x00, 0x00, 0x00, 0x00,  //6c  .hdr.bmiHeader.biClrImportant   = 0x00000000
				videoPinFormat.StartTimeCode = 0x0006f498;		//0x98, 0xF4, 0x06, 0x00,    //70  .dwStartTimeCode                = 0x0006f498
				videoPinFormat.SequenceHeader = 0;				//0x00, 0x00, 0x00, 0x00,    //74  .cbSequenceHeader               = 0x00000000
				videoPinFormat.Profile = AM_MPEG2Profile.Main;	//0x02, 0x00, 0x00, 0x00,    //78  .dwProfile                      = 0x00000002
				videoPinFormat.Level = AM_MPEG2Level.Main;		//0x02, 0x00, 0x00, 0x00,    //7c  .dwLevel                        = 0x00000002
				videoPinFormat.Flags = (AMMPEG2)0;				//0x00, 0x00, 0x00, 0x00,    //80  .Flags    

				videoMPEG2PinFormat = videoPinFormat;
			}
			return videoMPEG2PinFormat;
		}

        protected static MPEG2VideoInfo GetVideoH264PinFormat()
        {
            if (videoH264PinFormat == null)
            {
                MPEG2VideoInfo videoPinFormat = new MPEG2VideoInfo();
                videoPinFormat.hdr = new VideoInfoHeader2();
                videoPinFormat.hdr.SrcRect = new DsRect();
                videoPinFormat.hdr.SrcRect.left = 0;		//0x00, 0x00, 0x00, 0x00,  //00  .hdr.rcSource.left              = 0x00000000
                videoPinFormat.hdr.SrcRect.top = 0;			//0x00, 0x00, 0x00, 0x00,  //04  .hdr.rcSource.top               = 0x00000000
                videoPinFormat.hdr.SrcRect.right = 0;		//0xD0, 0x02, 0x00, 0x00,  //08  .hdr.rcSource.right             = 0x000002d0 //720
                videoPinFormat.hdr.SrcRect.bottom = 0;		//0x40, 0x02, 0x00, 0x00,  //0c  .hdr.rcSource.bottom            = 0x00000240 //576
                videoPinFormat.hdr.TargetRect = new DsRect();
                videoPinFormat.hdr.TargetRect.left = 0;		//0x00, 0x00, 0x00, 0x00,  //10  .hdr.rcTarget.left              = 0x00000000
                videoPinFormat.hdr.TargetRect.top = 0;		//0x00, 0x00, 0x00, 0x00,  //14  .hdr.rcTarget.top               = 0x00000000
                videoPinFormat.hdr.TargetRect.right = 0;	//0xD0, 0x02, 0x00, 0x00,  //18  .hdr.rcTarget.right             = 0x000002d0 //720
                videoPinFormat.hdr.TargetRect.bottom = 0;	//0x40, 0x02, 0x00, 0x00,  //1c  .hdr.rcTarget.bottom            = 0x00000240// 576
                videoPinFormat.hdr.BitRate = 0x003d0900;	//0x00, 0x09, 0x3D, 0x00,  //20  .hdr.dwBitRate                  = 0x003d0900
                videoPinFormat.hdr.BitErrorRate = 0;		//0x00, 0x00, 0x00, 0x00,  //24  .hdr.dwBitErrorRate             = 0x00000000

                ////0x051736=333667-> 10000000/333667 = 29.97fps
                ////0x061A80=400000-> 10000000/400000 = 25fps
                videoPinFormat.hdr.AvgTimePerFrame = 400000;				//0x80, 0x1A, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, //28  .hdr.AvgTimePerFrame            = 0x0000000000051763 ->1000000/ 40000 = 25fps
                videoPinFormat.hdr.InterlaceFlags = AMInterlace.None;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
                //videoPinFormat.hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.OneFieldPerSample | AMInterlace.DisplayModeBobOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
                //videoPinFormat.hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.DisplayModeBobOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
                //videoPinFormat.hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.FieldPatBothRegular | AMInterlace.DisplayModeWeaveOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
                //videoPinFormat.hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.DisplayModeBobOrWeave;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
                videoPinFormat.hdr.CopyProtectFlags = AMCopyProtect.None;	//0x00, 0x00, 0x00, 0x00,                         //30  .hdr.dwCopyProtectFlags         = 0x00000000
                videoPinFormat.hdr.PictAspectRatioX = 16; // 4;					//0x04, 0x00, 0x00, 0x00,                         //34  .hdr.dwPictAspectRatioX         = 0x00000004
                videoPinFormat.hdr.PictAspectRatioY = 9; // 3;					//0x03, 0x00, 0x00, 0x00,                         //38  .hdr.dwPictAspectRatioY         = 0x00000003
                videoPinFormat.hdr.ControlFlags = AMControl.None;			//0x00, 0x00, 0x00, 0x00,                         //3c  .hdr.dwReserved1                = 0x00000000
                videoPinFormat.hdr.Reserved2 = 0;							//0x00, 0x00, 0x00, 0x00,                         //40  .hdr.dwReserved2                = 0x00000000
                videoPinFormat.hdr.BmiHeader = new BitmapInfoHeader();
                videoPinFormat.hdr.BmiHeader.Size = 0x00000028;				//0x28, 0x00, 0x00, 0x00,  //44  .hdr.bmiHeader.biSize           = 0x00000028
                videoPinFormat.hdr.BmiHeader.Width = 720;					//0xD0, 0x02, 0x00, 0x00,  //48  .hdr.bmiHeader.biWidth          = 0x000002d0 //720
                videoPinFormat.hdr.BmiHeader.Height = 576;					//0x40, 0x02, 0x00, 0x00,  //4c  .hdr.bmiHeader.biHeight         = 0x00000240 //576
                videoPinFormat.hdr.BmiHeader.Planes = 0; // 1 ?					//0x00, 0x00,              //50  .hdr.bmiHeader.biPlanes         = 0x0000
                videoPinFormat.hdr.BmiHeader.BitCount = 0;					//0x00, 0x00,              //54  .hdr.bmiHeader.biBitCount       = 0x0000
                videoPinFormat.hdr.BmiHeader.Compression = 0;				//0x00, 0x00, 0x00, 0x00,  //58  .hdr.bmiHeader.biCompression    = 0x00000000
                videoPinFormat.hdr.BmiHeader.ImageSize = 0;					//0x00, 0x00, 0x00, 0x00,  //5c  .hdr.bmiHeader.biSizeImage      = 0x00000000
                videoPinFormat.hdr.BmiHeader.XPelsPerMeter = 0x000007d0;	//0xD0, 0x07, 0x00, 0x00,  //60  .hdr.bmiHeader.biXPelsPerMeter  = 0x000007d0
                videoPinFormat.hdr.BmiHeader.YPelsPerMeter = 0x0000cf27;	//0x27, 0xCF, 0x00, 0x00,  //64  .hdr.bmiHeader.biYPelsPerMeter  = 0x0000cf27
                videoPinFormat.hdr.BmiHeader.ClrUsed = 0;					//0x00, 0x00, 0x00, 0x00,  //68  .hdr.bmiHeader.biClrUsed        = 0x00000000
                videoPinFormat.hdr.BmiHeader.ClrImportant = 0;				//0x00, 0x00, 0x00, 0x00,  //6c  .hdr.bmiHeader.biClrImportant   = 0x00000000
                videoPinFormat.StartTimeCode = 0x0006f498;		//0x98, 0xF4, 0x06, 0x00,    //70  .dwStartTimeCode                = 0x0006f498
                videoPinFormat.SequenceHeader = 0;				//0x00, 0x00, 0x00, 0x00,    //74  .cbSequenceHeader               = 0x00000000
                videoPinFormat.Profile = AM_MPEG2Profile.Main;	//0x02, 0x00, 0x00, 0x00,    //78  .dwProfile                      = 0x00000002
                videoPinFormat.Level = AM_MPEG2Level.Main;		//0x02, 0x00, 0x00, 0x00,    //7c  .dwLevel                        = 0x00000002
                videoPinFormat.Flags = (AMMPEG2)0;				//0x00, 0x00, 0x00, 0x00,    //80  .Flags    

                videoH264PinFormat = videoPinFormat;
            }
            return videoH264PinFormat;
        }

        protected static VideoInfoHeader2 GetVideoInfoHeader2H264PinFormat()
        {
            //VideoInfoHeader2 hdr = new VideoInfoHeader2();
            //hdr.SrcRect = new DsRect();
            //hdr.SrcRect.left = 0;		//0x00, 0x00, 0x00, 0x00,  //00  .hdr.rcSource.left              = 0x00000000
            //hdr.SrcRect.top = 0;			//0x00, 0x00, 0x00, 0x00,  //04  .hdr.rcSource.top               = 0x00000000
            //hdr.SrcRect.right = 0;		//0xD0, 0x02, 0x00, 0x00,  //08  .hdr.rcSource.right             = 0x000002d0 //720
            //hdr.SrcRect.bottom = 0;		//0x40, 0x02, 0x00, 0x00,  //0c  .hdr.rcSource.bottom            = 0x00000240 //576
            //hdr.TargetRect = new DsRect();
            //hdr.TargetRect.left = 0;		//0x00, 0x00, 0x00, 0x00,  //10  .hdr.rcTarget.left              = 0x00000000
            //hdr.TargetRect.top = 0;		//0x00, 0x00, 0x00, 0x00,  //14  .hdr.rcTarget.top               = 0x00000000
            //hdr.TargetRect.right = 0;	//0xD0, 0x02, 0x00, 0x00,  //18  .hdr.rcTarget.right             = 0x000002d0 //720
            //hdr.TargetRect.bottom = 0;	//0x40, 0x02, 0x00, 0x00,  //1c  .hdr.rcTarget.bottom            = 0x00000240// 576
            //hdr.BitRate = 0x003d0900;	//0x00, 0x09, 0x3D, 0x00,  //20  .hdr.dwBitRate                  = 0x003d0900
            //hdr.BitErrorRate = 0;		//0x00, 0x00, 0x00, 0x00,  //24  .hdr.dwBitErrorRate             = 0x00000000

            //////0x051736=333667-> 10000000/333667 = 29.97fps
            //////0x061A80=400000-> 10000000/400000 = 25fps
            //hdr.AvgTimePerFrame = 400000;				//0x80, 0x1A, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, //28  .hdr.AvgTimePerFrame            = 0x0000000000051763 ->1000000/ 40000 = 25fps
            //hdr.InterlaceFlags = AMInterlace.None;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
            ////hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.OneFieldPerSample | AMInterlace.DisplayModeBobOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
            ////hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.DisplayModeBobOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
            ////hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.FieldPatBothRegular | AMInterlace.DisplayModeWeaveOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
            ////hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.DisplayModeBobOrWeave;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
            //hdr.CopyProtectFlags = AMCopyProtect.None;	//0x00, 0x00, 0x00, 0x00,                         //30  .hdr.dwCopyProtectFlags         = 0x00000000
            //hdr.PictAspectRatioX = 0;// 4;					//0x04, 0x00, 0x00, 0x00,                         //34  .hdr.dwPictAspectRatioX         = 0x00000004
            //hdr.PictAspectRatioY = 0;// 3;					//0x03, 0x00, 0x00, 0x00,                         //38  .hdr.dwPictAspectRatioY         = 0x00000003
            //hdr.ControlFlags = AMControl.None;			//0x00, 0x00, 0x00, 0x00,                         //3c  .hdr.dwReserved1                = 0x00000000
            //hdr.Reserved2 = 0;							//0x00, 0x00, 0x00, 0x00,                         //40  .hdr.dwReserved2                = 0x00000000
            //hdr.BmiHeader = new BitmapInfoHeader();
            //hdr.BmiHeader.Size = 0x00000028;				//0x28, 0x00, 0x00, 0x00,  //44  .hdr.bmiHeader.biSize           = 0x00000028
            //hdr.BmiHeader.Width = 1920; // 720;					//0xD0, 0x02, 0x00, 0x00,  //48  .hdr.bmiHeader.biWidth          = 0x000002d0 //720
            //hdr.BmiHeader.Height = 1080; // 576;					//0x40, 0x02, 0x00, 0x00,  //4c  .hdr.bmiHeader.biHeight         = 0x00000240 //576
            //hdr.BmiHeader.Planes = 0; // 1 ?					//0x00, 0x00,              //50  .hdr.bmiHeader.biPlanes         = 0x0000
            //hdr.BmiHeader.BitCount = 0;					//0x00, 0x00,              //54  .hdr.bmiHeader.biBitCount       = 0x0000
            //hdr.BmiHeader.Compression = 0;				//0x00, 0x00, 0x00, 0x00,  //58  .hdr.bmiHeader.biCompression    = 0x00000000
            //hdr.BmiHeader.ImageSize = 0;					//0x00, 0x00, 0x00, 0x00,  //5c  .hdr.bmiHeader.biSizeImage      = 0x00000000
            //hdr.BmiHeader.XPelsPerMeter = 0x000007d0;	//0xD0, 0x07, 0x00, 0x00,  //60  .hdr.bmiHeader.biXPelsPerMeter  = 0x000007d0
            //hdr.BmiHeader.YPelsPerMeter = 0x0000cf27;	//0x27, 0xCF, 0x00, 0x00,  //64  .hdr.bmiHeader.biYPelsPerMeter  = 0x0000cf27
            //hdr.BmiHeader.ClrUsed = 0;					//0x00, 0x00, 0x00, 0x00,  //68  .hdr.bmiHeader.biClrUsed        = 0x00000000
            //hdr.BmiHeader.ClrImportant = 0;				//0x00, 0x00, 0x00, 0x00,  //6c  .hdr.bmiHeader.biClrImportant   = 0x00000000

            VideoInfoHeader2 hdr = new VideoInfoHeader2();
            hdr.BmiHeader = new BitmapInfoHeader();
            hdr.BmiHeader.Size = 28; // 0x00000028;				//0x28, 0x00, 0x00, 0x00,  //44  .hdr.bmiHeader.biSize           = 0x00000028
            hdr.BmiHeader.Width = 1920; // 720;
            hdr.BmiHeader.Height = 1080; // 576;
            hdr.PictAspectRatioX = 0;
            hdr.PictAspectRatioY = 0;
            hdr.BmiHeader.Planes = 0;
            hdr.BmiHeader.BitCount = 24;
            hdr.BmiHeader.Compression = 0; //new MediaFoundation.Misc.FourCC("H264").ToInt32();
            return hdr;
        }

		protected static MPEG1WaveFormat GetAudioPinFormat()
		{
			if (audioPinFormat == null)
			{
				audioPinFormat = new MPEG1WaveFormat();
				audioPinFormat.wFormatTag = 0x0050; // WAVE_FORMAT_MPEG
				audioPinFormat.nChannels = 2;
				audioPinFormat.nSamplesPerSec = 48000;
				audioPinFormat.nAvgBytesPerSec = 32000;
				audioPinFormat.nBlockAlign = 768;
				audioPinFormat.wBitsPerSample = 16;
				audioPinFormat.cbSize = 22; // extra size

				audioPinFormat.HeadLayer = 2;
				audioPinFormat.HeadBitrate = 0x00177000;
				audioPinFormat.HeadMode = 1;
				audioPinFormat.HeadModeExt = 1;
				audioPinFormat.HeadEmphasis = 1;
				audioPinFormat.HeadFlags = 0x1c;
				audioPinFormat.PTSLow = 0;
				audioPinFormat.PTSHigh = 0;
			}

			return audioPinFormat;
		}

        public override void StopGraph()
        {
            base.StopGraph();

            this.CurrentChannel = null;

            Decompose();
        }

        public void DecomposeGraph() { this.Decompose(); }
		protected override void Decompose()
		{
			if (this.graphBuilder != null)
			{
                OnNewLogMessage("Decomposing graph");
				int hr = 0;

				OnGraphEnded();

				this.epg.UnRegisterEvent();

				// Decompose the graph
				try { hr = (this.graphBuilder as IMediaControl).StopWhenReady(); }
				catch { }
				try { hr = (this.graphBuilder as IMediaControl).Stop(); }
				catch { }
				RemoveHandlers();


                OnNewLogMessage("Removing filters");
				FilterGraphTools.RemoveAllFilters(this.graphBuilder);

                if (this.networkProvider != null)
                {
                    Marshal.ReleaseComObject(this.networkProvider);
                    this.networkProvider = null;
                    OnNewLogMessage(string.Format("\"{0}\" removed", "Network provider"));
                }
                if (this.mpeg2Demux != null)
                {                    
                    Marshal.ReleaseComObject(this.mpeg2Demux);
                    this.mpeg2Demux = null;
                    OnNewLogMessage(string.Format("\"{0}\" removed", "MPEG2 demux"));
                }

                if (this.tuner != null)
                {
                    Marshal.ReleaseComObject(this.tuner); 
                    this.tuner = null;
                    OnNewLogMessage(string.Format("\"{0}\" removed", "Tuner filter"));
                }

                if (this.capture != null)
                {
                    Marshal.ReleaseComObject(this.capture); 
                    this.capture = null;
                    OnNewLogMessage(string.Format("\"{0}\" removed", "Capture filter"));
                }

                if (this.bdaTIF != null)
                {
                    Marshal.ReleaseComObject(this.bdaTIF);
                    this.bdaTIF = null;

                    OnNewLogMessage(string.Format("\"{0}\" removed", "TIF filter"));
                }

                if (this.bdaSecTab != null)
                {
                    Marshal.ReleaseComObject(this.bdaSecTab);
                    this.bdaSecTab = null;
                    OnNewLogMessage(string.Format("\"{0}\" removed", "Sections and Tables"));
                }

                if (this.audioDecoderFilter != null)
                {
                    Marshal.ReleaseComObject(this.audioDecoderFilter);
                    this.audioDecoderFilter = null;
                    OnNewLogMessage(string.Format("\"{0}\" removed", "Audio decoder"));
                }

                if (this.videoH264DecoderFilter != null)
                {
                    Marshal.ReleaseComObject(this.videoH264DecoderFilter);
                    this.videoH264DecoderFilter = null;
                    OnNewLogMessage(string.Format("\"{0}\" removed", "H264 decoder"));
                }

                if (this.videoMpeg2DecoderFilter != null)
                {
                    Marshal.ReleaseComObject(this.videoMpeg2DecoderFilter);
                    this.videoMpeg2DecoderFilter = null;
                    OnNewLogMessage(string.Format("\"{0}\" removed", "MPEG2 decoder"));
                }

                if (this.audioRenderer != null)
                {
                    Marshal.ReleaseComObject(this.audioRenderer);
                    this.audioRenderer = null;
                    OnNewLogMessage(string.Format("\"{0}\" removed", "Audio renderer"));
                }

                if (this.videoRenderer != null)
                {
                    Marshal.ReleaseComObject(this.videoRenderer);
                    this.videoRenderer = null;
                    OnNewLogMessage(string.Format("\"{0}\" removed", "Video renderer"));
                }

                if (this.captureGraphBuilder != null)
                {
                    Marshal.ReleaseComObject(this.captureGraphBuilder);
                    this.captureGraphBuilder = null;
                    OnNewLogMessage(string.Format("\"{0}\" released", "CaptureGraphBuilder"));
                }

				try { rot.Dispose(); }
				catch { }
				try { Marshal.ReleaseComObject(this.graphBuilder); this.graphBuilder = null; }
				catch { }

                OnNewLogMessage("Graph decomposed");
			}
		}

		public override bool GetSignalStatistics(out bool locked, out bool present, out int strength, out int quality)
		{
			int longVal = strength = quality = 0;
			bool byteVal = locked = present = false;

			//Get IID_IBDA_Topology
			IBDA_Topology bdaNetTop = this.tuner as IBDA_Topology;
			if (bdaNetTop == null)
			{
				return false;
			}

			int nodeTypes;
			int[] nodeType = new int[32];
			int hr = bdaNetTop.GetNodeTypes(out nodeTypes, 32, nodeType);
			DsError.ThrowExceptionForHR(hr);

			for (int i = 0; i < nodeTypes; i++)
			{
				object iNode;
				hr = bdaNetTop.GetControlNode(0, 1, nodeType[i], out iNode);
				if (hr == 0)
				{
					IBDA_SignalStatistics pSigStats = iNode as IBDA_SignalStatistics;
					if (pSigStats != null)
					{
						longVal = 0;
						if (pSigStats.get_SignalStrength(out longVal) == 0)
							strength = longVal;

						longVal = 0;
						if (pSigStats.get_SignalQuality(out longVal) == 0)
							quality = longVal;

						byteVal = false;
						if (pSigStats.get_SignalLocked(out byteVal) == 0)
							locked = byteVal;

						byteVal = false;
						if (pSigStats.get_SignalPresent(out byteVal) == 0)
							present = byteVal;
					}
					Marshal.ReleaseComObject(iNode);
					return true;
				}
			}
			return false;
		}

		public string GetTablesInfos(Channel channel, bool allTransponderInfo)
		{
			string result = "";
			if (channel != null && channel is ChannelDVB)
			{
				ChannelDVB channelDVB = channel as ChannelDVB;

				IMpeg2Data mpeg2Data = this.bdaSecTab as IMpeg2Data;
				// Hervé Stalin : Utile ?
				//Hashtable serviceNameByServiceId = new Hashtable(); 
				PSISection[] psiSdts = PSISection.GetPSITable((int)PIDS.SDT, (int)TABLE_IDS.SDT_ACTUAL, mpeg2Data);
				for (int i = 0; i < psiSdts.Length; i++)
				{
					PSISection psiSdt = psiSdts[i];
					if (psiSdt != null && psiSdt is PSISDT)
					{
						PSISDT sdt = (PSISDT)psiSdt;
						if (allTransponderInfo)
						{
							result += "PSI Table " + i + "/" + psiSdts.Length + "\r\n";
							result += sdt.ToString();
						}
						// Hervé Stalin : Utile ?
						//foreach (PSISDT.Data service in sdt.Services)
						//{
						//    serviceNameByServiceId[service.ServiceId] = service.GetServiceName();
						//}
					}
				}

				//Hervé Stalin : Code pode pour créér un hashtable de lcn
				//Hashtable logicalChannelNumberByServiceId = new Hashtable();
				PSISection[] psiNits = PSISection.GetPSITable((int)PIDS.NIT, (int)TABLE_IDS.NIT_ACTUAL, mpeg2Data);
				for (int i = 0; i < psiNits.Length; i++)
				{
					PSISection psinit = psiNits[i];
					if (psinit != null && psinit is PSINIT)
					{
						PSINIT nit = (PSINIT)psinit;
						result += "PSI Table " + i + "/" + psiNits.Length + "\r\n";
						result += nit.ToString();

						//foreach (PSINIT.Data data in nit.Streams)
						//{
						//    foreach (PSIDescriptor psiDescriptorData in data.Descriptors)
						//    {
						//        if (psiDescriptorData.DescriptorTag == DESCRIPTOR_TAGS.DESCR_LOGICAL_CHANNEL)
						//        {
						//            PSIDescriptorLogicalChannel psiDescriptorLogicalChannel = (PSIDescriptorLogicalChannel)psiDescriptorData;
						//            foreach (PSIDescriptorLogicalChannel.ChannelNumber f in psiDescriptorLogicalChannel.LogicalChannelNumbers)
						//            {
						//                logicalChannelNumberByServiceId[f.ServiceID] = f.LogicalChannelNumber;
						//            }

						//        }
						//    }
						//}

					}
				}


				PSISection[] psiPats = PSISection.GetPSITable((int)PIDS.PAT, (int)TABLE_IDS.PAT, mpeg2Data);
				for (int i = 0; i < psiPats.Length; i++)
				{
					PSISection psiPat = psiPats[i];
					if (psiPat != null && psiPat is PSIPAT)
					{
						PSIPAT pat = (PSIPAT)psiPat;
						if (allTransponderInfo)
						{
							result += "PSI Table " + i + "/" + psiPats.Length + "\r\n";
							result += pat.ToString();
						}

						foreach (PSIPAT.Data program in pat.ProgramIds)
						{
							if (allTransponderInfo || program.ProgramNumber == channelDVB.SID)
							{
								if (!program.IsNetworkPID)
								{
									PSISection[] psiPmts = PSISection.GetPSITable(program.Pid, (int)TABLE_IDS.PMT, mpeg2Data);
									for (int i2 = 0; i2 < psiPmts.Length; i2++)
									{
										PSISection psiPmt = psiPmts[i2];
										if (psiPmt != null && psiPmt is PSIPMT)
										{
											PSIPMT pmt = (PSIPMT)psiPmt;
											result += "PSI Table " + i2 + "/" + psiPmts.Length + "\r\n";
											result += pmt.ToString();
										}
										if (!allTransponderInfo) return result;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}
	}
}
