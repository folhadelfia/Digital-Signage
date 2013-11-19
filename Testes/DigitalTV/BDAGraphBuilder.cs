#define ALLOW_UNTESTED_INTERFACES

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DirectShowLib;
using DirectShowLib.BDA;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;
using MediaFoundation.EVR;
using MediaFoundation;
using System.Drawing;



namespace DigitalTV
{
    public enum BDAState
    {
        Running,
        RunningNoRenderers,
        Scanning,
        Stopped,
        Tunning
    }

    public class BDAGraphBuilder : IDisposable
    {
        public event BDAGraphEventHandler BDALogging;

        DigitalTVScreen hostingControl = null;

        public DigitalTVScreen HostingControl
        {
            get { return hostingControl; }
            set { hostingControl = value; }
        }

        IFilterGraph2 graphBuilder = null;
        DsROTEntry rot = null;

        //Para o EVR
        IMFVideoDisplayControl evrVideoDisplayControl;

        #region Filtros
        IBaseFilter networkProvider = null;
        IBaseFilter mpeg2Demux = null;
        IBaseFilter h264DecoderFilter = null;
        IBaseFilter mpeg2DecoderFilter = null;
        IBaseFilter audioDecoderFilter = null;
        IBaseFilter tuner = null;
        IBaseFilter demodulator = null;
        IBaseFilter capture = null;
        IBaseFilter bdaTIF = null;
        IBaseFilter bdaSecTab = null;
        IBaseFilter audioRenderer = null;
        IBaseFilter videoRenderer = null;
        #endregion

        public VideoRenderer VideoRendererDevice { get; set; }

        #region Decoders

        #region A ser utilizado

        private DsDevice mpeg2Decoder;
        /// <summary>
        /// Codec
        /// </summary>
        public DsDevice MPEG2Decoder
        {
            get { return mpeg2Decoder; }
            set { if (MPEG2DecoderDevices.Values.Contains(value) || value == null) mpeg2Decoder = value; }
        }

        private DsDevice h264Decoder;
        /// <summary>
        /// Codec
        /// </summary>
        public DsDevice H264Decoder
        {
            get { return h264Decoder; }
            set { if (H264DecoderDevices.Values.Contains(value) || value == null) h264Decoder = value; }
        }

        private DsDevice audioDecoder;
        /// <summary>
        /// Codec
        /// </summary>
        public DsDevice AudioDecoder
        {
            get { return audioDecoder; }
            set { audioDecoder = value; }
        }

        #endregion

        #region Todos

        private Dictionary<string, DsDevice> mpeg2DecoderDevices;
        /// <summary>
        /// Codecs
        /// </summary>
        public Dictionary<string, DsDevice> MPEG2DecoderDevices
        {
            get
            {
                if (mpeg2DecoderDevices == null)
                {
                    mpeg2DecoderDevices = new Dictionary<string, DsDevice>();

                    foreach (var device in DeviceEnumerator.GetMPEG2VideoDevices())
                    {
                        mpeg2DecoderDevices.Add(device.Name, device);
                    }
                }

                return mpeg2DecoderDevices;
            }
        }

        private Dictionary<string, DsDevice> h264DecoderDevices;
        /// <summary>
        /// Codecs
        /// </summary>
        public Dictionary<string, DsDevice> H264DecoderDevices
        {
            get
            {
                if (h264DecoderDevices == null)
                {
                    h264DecoderDevices = new Dictionary<string, DsDevice>();

                    foreach (var device in DeviceEnumerator.GetH264Devices())
                        h264DecoderDevices.Add(device.Name, device);
                }

                return h264DecoderDevices;
            }
        }

        private Dictionary<string, DsDevice> audioDecoderDevices;
        /// <summary>
        /// Codecs
        /// </summary>
        public Dictionary<string, DsDevice> AudioDecoderDevices
        {
            get
            {
                if (audioDecoderDevices == null)
                {
                    audioDecoderDevices = new Dictionary<string, DsDevice>();

                    foreach (var device in DeviceEnumerator.GetDevicesWithThisInPin(MediaType.Audio, MediaSubType.Mpeg2Audio))
                        audioDecoderDevices.Add(device.Name, device);
                }
                return audioDecoderDevices; 
            }
        }
        

        #endregion

        #endregion

        #region Devices

        #region A ser utilizado

        private DsDevice audioDevice;
        /// <summary>
        /// Dispositivo físico de audio
        /// </summary>
        public DsDevice AudioDevice
        {
            get { return audioDevice; }
            set { if (value == null || audioDevices.Values.Contains(value)) audioDevice = value; }
        }

        private DsDevice tunerDevice;
        /// <summary>
        /// Placa TV
        /// </summary>
        public DsDevice TunerDevice
        {
            get { return tunerDevice; }
            set { if (tunerDevices.Values.Contains(value) || value == null) tunerDevice = value; }
        }

        //private List<string> availableVideoDevices;
        //public List<string> VideoDevices
        //{
        //    get
        //    {
        //        if (availableVideoDevices == null)
        //        {
        //            availableVideoDevices = new List<string>();

        //            foreach (var device in DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory))
        //                availableVideoDevices.Add(device.Name);
        //        }

        //        return availableVideoDevices;
        //    }
        //}

        //private string videoDevice;
        //public string VideoDevice
        //{
        //    get { return videoDevice; }
        //    set { if (availableVideoDevices.Contains(value) || value == null) videoDevice = value; }
        //}

        #endregion

        #region Todos

        private Dictionary<string, DsDevice> audioDevices;
        /// <summary>
        /// Dispositivos físicos de audio
        /// </summary>
        public Dictionary<string, DsDevice> AudioDevices
        {
            get
            {
                if (audioDevices == null)
                {
                    audioDevices = new Dictionary<string, DsDevice>();

                    foreach (var device in DsDevice.GetDevicesOfCat(FilterCategory.AudioRendererCategory).OrderBy(x=>x.Name))
                        audioDevices.Add(device.Name, device);
                }

                return audioDevices;
            }
        }

        private Dictionary<string, DsDevice> tunerDevices;
        /// <summary>
        /// Placas de TV
        /// </summary>
        public Dictionary<string, DsDevice> TunerDevices
        {
            get
            {
                if (tunerDevices == null) tunerDevices = new Dictionary<string, DsDevice>();
                if (tunerDevices.Count != 0) tunerDevices.Clear();

                foreach (var device in DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory).OrderBy(x=>x.Name))
                    tunerDevices.Add(device.Name, device);

                return tunerDevices;
            }
        }

        #endregion

        #endregion

        public event EventHandler StateChanged;

        private BDAState state = BDAState.Stopped;

        public BDAState State
        {
            private set
            {
                if (StateChanged != null)
                    StateChanged(this, new EventArgs());
                state = value;
            }
            get { return state; }
        }

        public BDAGraphBuilder(DigitalTVScreen renderingControl)
        {
            this.hostingControl = renderingControl;
            VideoRendererDevice = VideoRenderer.VMR9;
        }
        
        #region Membres de IDisposable

        public void Dispose()
        {
            Decompose();
        }

        #endregion

        public void StartToGetChannels(DVBTTuning tuner)
        {
            this.BuildGraphForChannels(tuner.TuningSpace);
            this.RunGraph();

            State = BDAState.RunningNoRenderers;
        }
        public void Start(DVBTTuning tuner)
        {
            this.BuildGraph(tuner.TuningSpace);
            this.RunGraph();
            this.SubmitTuneRequest(tuner.TuneRequest);

            State = BDAState.Running;
        }

        public void StartManual(DVBTTuning tuner)
        {
            this.BuildGraphManual(tuner.TuningSpace);
            this.RunGraph();
            this.SubmitTuneRequest(tuner.TuneRequest);

            State = BDAState.Running;
        }

        public IEnumerable<Channel> GetChannelList(ITuningSpace tuningSpace)
        {
            BDAState oldState = state;
            if (oldState != BDAState.Running && oldState != BDAState.RunningNoRenderers) return null;

            State = BDAState.Scanning;

            IEnumerable<Channel> result = GetChannelInfo();
            
            State = oldState;

            return result;
        }

        private IEnumerable<Channel> GetChannelInfo()
        {
            int hr = 0;

            List<Channel> chList = new List<Channel>();

            IMpeg2Data tableInfo = bdaSecTab as IMpeg2Data;

            IDvbSiParser parser = (IDvbSiParser)new DvbSiParser();
            parser.Initialize(tableInfo);

            IPAT PAT;
            hr = parser.GetPAT(out PAT);
            Marshal.ThrowExceptionForHR(hr);

            IDVB_SDT SDT;
            hr = parser.GetSDT(0x42, null, out SDT);//DVB_SDT_ACTUAL_TID (0x42) SDT for the current transport stream.
            Marshal.ThrowExceptionForHR(hr);

            IDVB_NIT NIT;
            hr = parser.GetNIT(0x40, null, out NIT);//DVB_NIT_ACTUAL_TID (0x40) NIT for the network that carries this transport
            Marshal.ThrowExceptionForHR(hr);

            short sid = 0;
            int sdtRecordsCount = 0;
            string channelName = "";

            hr = SDT.GetCountOfRecords(out sdtRecordsCount);
            Marshal.ThrowExceptionForHR(hr);


            for (int i = 0; i < sdtRecordsCount; i++)
            {
                IGenericDescriptor descriptor = null;
                IDvbServiceDescriptor servDescriptor = null;

                SDT.GetRecordServiceId(i, out sid);
                hr = SDT.GetRecordDescriptorByTag(i, 0x48, null, out descriptor);
                servDescriptor = descriptor as IDvbServiceDescriptor;

                byte chType;

                servDescriptor.GetServiceNameEmphasized(out channelName);
                servDescriptor.GetServiceType(out chType);

                chList.Add(new Channel() { Name = channelName, SID = sid, Type = chType });
            }


            return chList;
        }
        private IEnumerable<Channel> GetChannelInfo2()
        {
            int hr = 0;

            IMpeg2Demultiplexer mpeg2Demuxer = mpeg2Demux as IMpeg2Demultiplexer;

            IPin pinTIF = null;
            int pinNumber = 0;

            bool tifPinFound = false;

            while (!tifPinFound)
            {

                pinTIF = DsFindPin.ByDirection(mpeg2Demux, PinDirection.Output, pinNumber);
                // Is the last pin reached ?
                if (pinTIF == null)
                    break;

                IEnumMediaTypes enumMediaTypes = null;
                AMMediaType[] mediaTypes = new AMMediaType[1];

                try
                {
                    // Get Pin's MediaType enumerator
                    hr = pinTIF.EnumMediaTypes(out enumMediaTypes);
                    DsError.ThrowExceptionForHR(hr);

                    // for each media types...
                    while (enumMediaTypes.Next(mediaTypes.Length, mediaTypes, IntPtr.Zero) == 0)
                    {
                        // Store the majortype and the subtype and free the structure
                        Guid majorType = mediaTypes[0].majorType;
                        Guid subType = mediaTypes[0].subType;
                        DsUtils.FreeAMMediaType(mediaTypes[0]);

                        if (majorType == MediaType.Mpeg2Sections)
                        {
                            if (subType == MediaSubType.DvbSI)
                            {
                                //FOUND IT
                                tifPinFound = true;
                            }
                        }
                    }
                }
                finally
                {
                        // Free COM objects
                    Marshal.ReleaseComObject(enumMediaTypes);
                    enumMediaTypes = null;

                    if (!tifPinFound)
                    {
                        Marshal.ReleaseComObject(pinTIF);
                        pinTIF = null;
                    }
                }

                // Next pin, please !
                pinNumber++;
            }//Aqui já temos o pin de TIF

            if (!tifPinFound) return null;

            IMPEG2PIDMap mpeg2PIDMap = pinTIF as IMPEG2PIDMap;

            if (mpeg2PIDMap != null)
            {
                hr = mpeg2PIDMap.MapPID(6, new int[] { 0x00, 0x10, 0x11, 0x12, 0x13, 0x14 }, MediaSampleContent.Mpeg2PSI);
            }
            Marshal.ReleaseComObject(pinTIF);

            return null;
        }

        public void SubmitTuneRequest(ITuneRequest tuneRequest)
        {
            BDAState old = State;
            State = BDAState.Tunning;
            try
            {
                int hr = 0;

                hr = (this.networkProvider as ITuner).put_TuneRequest(tuneRequest);
                DsError.ThrowExceptionForHR(hr);

                ITuningSpace space;

                tuneRequest.get_TuningSpace(out space);

                //this.BuildGraph(space);
                this.HostingControl.Invalidate();
            }
            catch
            {
            }
            finally
            {
                State = old;
            }
        }

        private void RunGraph()
        {
            int hr = 0;

            hr = (this.graphBuilder as IMediaControl).Run();
            DsError.ThrowExceptionForHR(hr);
        }
        public void SaveGraph(string filepath)
        {
            // Nothing to do with a DTV viewer but can be useful
            FilterGraphTools.SaveGraphFile(this.graphBuilder, filepath);
        }
        public void Stop()
        {
            this.Dispose();

            if(this.state == BDAState.Running)
                this.HostingControl.Invalidate();
        }

        #region Guts Manual


        //public void StartManual(DVBTTuning tuner)
        //{
        //    this.BuildGraphManual(tuner.TuningSpace);
        //    this.RunGraph();
        //}

        //private void BuildGraphForChannelsManual(ITuningSpace tuningSpace)
        //{
        //} //FALTA IMPLEMENTAR
        //private void BuildGraphManual(ITuningSpace tuningSpace)
        //{
        //    try
        //    {
        //        int hr;

        //        this.graphBuilder = (IFilterGraph2)new FilterGraph();
        //        rot = new DsROTEntry(graphBuilder);

        //        AddNetworkProviderFilterManual(tuningSpace);
        //        AddMPEG2DemuxFilterManual();

        //        CreateMPEG2DemuxPinsManual();

        //        AddAudioDecoderFiltersManual();
        //        AddVideoDecoderFiltersManual();

        //        AddAndConnectBDABoardFiltersManual();

        //        if (this.tuner != null && this.capture != null) //Two filter model
        //            Log("Two filter model");
        //        else if (this.demodulator != null) Log("One filter model");

        //        AddTransportStreamFiltersManual();
        //        AddAndConnectSectionsAndTablesFilterToGraph();
        //        AddAndConnectTIFToGraph();

        //        AddRenderersManual();
        //        ConfigureVMR9InWindowlessMode();
        //        Vmr9SetDeinterlaceMode(1);

        //        ConnectAudioAndVideoFilters();
        //    }
        //    catch
        //    {
        //        Decompose();
        //    }
        //} //FALTA IMPLEMENTAR

        ////Arranjar forma de escolher os codecs e os devices antes de montar o graph

        //private void AddNetworkProviderFilterManual(ITuningSpace tuningSpace)
        //{
        //    int hr = 0;
        //    Guid genProviderClsId = new Guid("{B2F3A67C-29DA-4C78-8831-091ED509A475}");
        //    Guid networkProviderClsId;

        //    // First test if the Generic Network Provider is available (only on MCE 2005 + Update Rollup 2)
        //    //if (FilterGraphTools.IsThisComObjectInstalled(genProviderClsId))
        //    //{
        //    //    this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, genProviderClsId, "Generic Network Provider");

        //    //    hr = (this.networkProvider as ITuner).put_TuningSpace(tuningSpace);
        //    //    return;
        //    //}

        //    // Get the network type of the requested Tuning Space
        //    hr = tuningSpace.get__NetworkType(out networkProviderClsId);

        //    // Get the network type of the requested Tuning Space
        //    if (networkProviderClsId == typeof(DVBTNetworkProvider).GUID)
        //    {
        //        this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, networkProviderClsId, "DVBT Network Provider");
        //    }
        //    else if (networkProviderClsId == typeof(DVBSNetworkProvider).GUID)
        //    {
        //        this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, networkProviderClsId, "DVBS Network Provider");
        //    }
        //    else if (networkProviderClsId == typeof(ATSCNetworkProvider).GUID)
        //    {
        //        this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, networkProviderClsId, "ATSC Network Provider");
        //    }
        //    else if (networkProviderClsId == typeof(DVBCNetworkProvider).GUID)
        //    {
        //        this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, networkProviderClsId, "DVBC Network Provider");
        //    }
        //    else
        //        // Tuning Space can also describe Analog TV but this application don't support them
        //        throw new ArgumentException("This application doesn't support this Tuning Space");

        //    hr = (this.networkProvider as ITuner).put_TuningSpace(tuningSpace);
		
        //}
        //private void AddMPEG2DemuxFilterManual()
        //{
        //    this.mpeg2Demux = (IBaseFilter)new MPEG2Demultiplexer();

        //    int hr = this.graphBuilder.AddFilter(mpeg2Demux, "MPEG2 Demultiplexer Manual");
        //    DsError.ThrowExceptionForHR(hr);

        //}
        //private void AddAudioDecoderFiltersManual()
        //{
        //    int hr = 0;

        //    if (this.AudioDevice != null)
        //        hr = graphBuilder.AddSourceFilterForMoniker(this.AudioDecoder.Mon, null, this.AudioDecoder.Name + " Manual", out audioDecoderFilter);
        //    DsError.ThrowExceptionForHR(hr);
        //}
        //private void AddVideoDecoderFiltersManual()
        //{
        //    int hr = 0;

        //    if (this.H264Decoder != null)
        //        hr = graphBuilder.AddSourceFilterForMoniker(this.H264Decoder.Mon, null, this.H264Decoder.Name + " Manual", out this.h264DecoderFilter);
        //    else if (this.MPEG2Decoder != null)
        //        hr = graphBuilder.AddSourceFilterForMoniker(this.MPEG2Decoder.Mon, null, this.MPEG2Decoder.Name + " Manual", out this.mpeg2DecoderFilter);
        //    DsError.ThrowExceptionForHR(hr);

        //} 
        //private void AddAndConnectBDABoardFiltersManual()
        //{
        //    AddAndConnectBDABoardFilters();
        //    //DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory);

        //    //int hr = graphBuilder.AddSourceFilterForMoniker(devices[0].Mon, 
        //} //Chama o automático abaixo
        ///*
        // * TODO: é preciso não esquecer de ligar o tuner ao demux. Ligar um device de um filtro e dif de ligar um de dois.
        // *      Se o filtro de captura nao for null, ligá-lo. Senão ligar o outro
        // *      
        // *      os filtros dos dados já estão ligados penso eu
        // * 
        // *      eventualmente é preciso ver o deinterlace
        // * 
        // * 
        // * 
        // */

        //private void AddTransportStreamFiltersManual()
        //{
        //    Log("Adding the transport stream filters");

        //    int hr = 0;
        //    DsDevice[] devices;

        //    // Add two filters needed in a BDA graph
        //    devices = DsDevice.GetDevicesOfCat(FilterCategory.BDATransportInformationRenderersCategory);

        //    Log(string.Format("Found {0} device{1}", devices.Length, devices.Length == 1 ? "" : "s"));

        //    for (int i = 0; i < devices.Length; i++)
        //    {
        //        if (devices[i].Name.Equals("BDA MPEG2 Transport Information Filter Manual"))
        //        {
        //            hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out this.bdaTIF);
        //            Log(string.Format("Device {0}: {1} {2}", i, devices[i].Name, hr == 0 ? "added successfully" : "failed to add"));

        //            DsError.ThrowExceptionForHR(hr);
        //            continue;
        //        }

        //        if (devices[i].Name.Equals("MPEG-2 Sections and Tables Manual"))
        //        {
        //            hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out this.bdaSecTab);
        //            Log(string.Format("Device {0}: {1} {2}", i, devices[i].Name, hr == 0 ? "added successfully" : "failed to add"));

        //            DsError.ThrowExceptionForHR(hr);
        //            continue;
        //        }
        //    }
        //}
        //protected void AddAndConnectTIFToGraph()
        //{
        //    int hr = 0;
        //    IPin pinOut;
        //    DsDevice[] devices;

        //    // Add two filters needed in a BDA graph
        //    devices = DsDevice.GetDevicesOfCat(FilterCategory.BDATransportInformationRenderersCategory);
        //    for (int i = 0; i < devices.Length; i++)
        //    {
        //        if (devices[i].Name.Equals("BDA MPEG2 Transport Information Filter Manual"))
        //        {
        //            hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out this.bdaTIF);
        //            DsError.ThrowExceptionForHR(hr);

        //            // Connect the MPEG-2 Demux output pin for the "BDA MPEG2 Transport Information Filter"
        //            hr = this.mpeg2Demux.FindPin("TIF", out pinOut);
        //            if (pinOut != null)
        //            {
        //                IPin pinIn = DsFindPin.ByDirection(this.bdaTIF, PinDirection.Input, 0);
        //                if (pinIn != null)
        //                {
        //                    hr = this.graphBuilder.Connect(pinOut, pinIn);
        //                    Marshal.ReleaseComObject(pinIn);
        //                }

        //                // In fact the last pin don't render since i havn't added the BDA MPE Filter...
        //                Marshal.ReleaseComObject(pinOut);
        //            }

        //            continue;
        //        }
        //    }
        //}
        //protected void AddAndConnectSectionsAndTablesFilterToGraph()
        //{
        //    int hr = 0;
        //    IPin pinOut;
        //    DsDevice[] devices;

        //    // Add two filters needed in a BDA graph
        //    devices = DsDevice.GetDevicesOfCat(FilterCategory.BDATransportInformationRenderersCategory);
        //    for (int i = 0; i < devices.Length; i++)
        //    {
        //        if (devices[i].Name.Equals("MPEG-2 Sections and Tables Manual"))
        //        {
        //            hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out this.bdaSecTab);
        //            DsError.ThrowExceptionForHR(hr);

        //            // Connect the MPEG-2 Demux output pin for the "MPEG-2 Sections and Tables" filter
        //            hr = this.mpeg2Demux.FindPin("PSI", out pinOut);
        //            if (pinOut != null)
        //            {
        //                IPin pinIn = DsFindPin.ByDirection(this.bdaSecTab, PinDirection.Input, 0);
        //                if (pinIn != null)
        //                {
        //                    hr = this.graphBuilder.Connect(pinOut, pinIn);
        //                    Marshal.ReleaseComObject(pinIn);
        //                }

        //                //DsError.ThrowExceptionForHR(hr);
        //                // In fact the last pin don't render since i havn't added the BDA MPE Filter...
        //                Marshal.ReleaseComObject(pinOut);
        //            }

        //            continue;
        //        }
        //    }
        //}

        //private void CreateMPEG2DemuxPinsManual()
        //{
        //    IMpeg2Demultiplexer mpeg2Demultiplexer = this.mpeg2Demux as IMpeg2Demultiplexer;

        //    {
        //        //Pin 1 connected to the "BDA MPEG2 Transport Information Filter"
        //        //    Major Type	MEDIATYPE_MPEG2_SECTIONS {455F176C-4B06-47CE-9AEF-8CAEF73DF7B5}
        //        //    Sub Type		MEDIASUBTYPE_DVB_SI {E9DD31A3-221D-4ADB-8532-9AF309C1A408}
        //        //    Format		None

        //        //    MPEG2 PSI Sections
        //        //    Pids: 0x00 0x10 0x11 0x12 0x13 0x14 0x6e 0xd2 0x0136 0x019a 0x01fe 0x0262 0x03f2

        //        AMMediaType mediaTIF = new AMMediaType();
        //        mediaTIF.majorType = MediaType.Mpeg2Sections;
        //        mediaTIF.subType = MediaSubType.DvbSI;
        //        mediaTIF.fixedSizeSamples = false;
        //        mediaTIF.temporalCompression = false;
        //        mediaTIF.sampleSize = 1;
        //        mediaTIF.unkPtr = IntPtr.Zero;
        //        mediaTIF.formatType = FormatType.None;
        //        mediaTIF.formatSize = 0;
        //        mediaTIF.formatPtr = IntPtr.Zero;

        //        IPin pinDemuxerTIF;
        //        int hr = mpeg2Demultiplexer.CreateOutputPin(mediaTIF, "TIF", out pinDemuxerTIF);
        //        if (pinDemuxerTIF != null)
        //            Marshal.ReleaseComObject(pinDemuxerTIF);
        //    }

        //    if (this.H264Decoder != null)
        //    {
        //        //Try Original
        //        AMMediaType mediaH264 = new AMMediaType();
        //        mediaH264.majorType = MediaType.Video;
        //        //mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
        //        mediaH264.subType = MediaSubType.H264;// new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
        //        mediaH264.sampleSize = 0;
        //        mediaH264.temporalCompression = true; // false;
        //        mediaH264.fixedSizeSamples = true; // false;
        //        mediaH264.unkPtr = IntPtr.Zero;
        //        mediaH264.formatType = FormatType.Mpeg2Video;

        //        MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
        //        mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
        //        mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
        //        Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

        //        IPin pinDemuxerVideoH264;
        //        int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
        //        if (pinDemuxerVideoH264 != null)
        //            Marshal.ReleaseComObject(pinDemuxerVideoH264);

        //        Marshal.FreeHGlobal(mediaH264.formatPtr);

        //        ////Try 1
        //        //AMMediaType mediaH264 = new AMMediaType();
        //        //mediaH264.majorType = MediaType.Null;
        //        ////mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
        //        //mediaH264.subType = MediaSubType.H264;// new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
        //        //mediaH264.sampleSize = 0;
        //        //mediaH264.temporalCompression = true; // false;
        //        //mediaH264.fixedSizeSamples = true; // false;
        //        //mediaH264.unkPtr = IntPtr.Zero;
        //        //mediaH264.formatType = FormatType.Null;

        //        ////MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
        //        ////mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
        //        ////mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
        //        ////Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

        //        //IPin pinDemuxerVideoH264;
        //        //int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
        //        //if (pinDemuxerVideoH264 != null)
        //        //    Marshal.ReleaseComObject(pinDemuxerVideoH264);

        //        ////Marshal.FreeHGlobal(mediaH264.formatPtr);


        //        ////Try http://mheg2xmltv.googlecode.com/svn/trunk/dcdvbsource/Source/Filter/DVBGraphBuilder.pas
        //        //AMMediaType mediaH264 = new AMMediaType();
        //        //mediaH264.majorType = MediaType.Video;
        //        ////mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
        //        //mediaH264.subType = MediaSubType.H264;// new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
        //        //mediaH264.sampleSize = 1; // 0;
        //        //mediaH264.temporalCompression = true; // false;
        //        //mediaH264.fixedSizeSamples = false; // true; // false;
        //        //mediaH264.unkPtr = IntPtr.Zero;
        //        //mediaH264.formatType = FormatType.VideoInfo2; // FormatType.VideoInfo2;//FormatType.Mpeg2Video;

        //        //VideoInfoHeader2 videoH264PinFormat = GetVideoInfoHeader2H264PinFormat(); // GetVideoH264PinFormat();
        //        //mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
        //        //mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
        //        //Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

        //        //IPin pinDemuxerVideoH264;
        //        //int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
        //        //if (pinDemuxerVideoH264 != null)
        //        //    Marshal.ReleaseComObject(pinDemuxerVideoH264);

        //        //Marshal.FreeHGlobal(mediaH264.formatPtr);

        //    }
        //    else
        //    {
        //        AMMediaType mediaMPG2 = new AMMediaType();
        //        mediaMPG2.majorType = MediaType.Video;
        //        mediaMPG2.subType = MediaSubType.Mpeg2Video;
        //        mediaMPG2.fixedSizeSamples = false;
        //        mediaMPG2.temporalCompression = false; // true???
        //        mediaMPG2.sampleSize = 0;
        //        mediaMPG2.formatType = FormatType.Mpeg2Video;
        //        mediaMPG2.unkPtr = IntPtr.Zero;

        //        MPEG2VideoInfo videoMPEG2PinFormat = GetVideoMPEG2PinFormat();
        //        mediaMPG2.formatSize = Marshal.SizeOf(videoMPEG2PinFormat);
        //        mediaMPG2.formatPtr = Marshal.AllocHGlobal(mediaMPG2.formatSize);
        //        Marshal.StructureToPtr(videoMPEG2PinFormat, mediaMPG2.formatPtr, false);

        //        IPin pinDemuxerVideoMPEG2;
        //        int hr = mpeg2Demultiplexer.CreateOutputPin(mediaMPG2, "MPEG2", out pinDemuxerVideoMPEG2);
        //        if (pinDemuxerVideoMPEG2 != null)
        //            Marshal.ReleaseComObject(pinDemuxerVideoMPEG2);

        //        Marshal.FreeHGlobal(mediaMPG2.formatPtr);
        //    }

        //    {
        //        AMMediaType mediaAudio = new AMMediaType();
        //        mediaAudio.majorType = MediaType.Audio;

        //        mediaAudio.subType = MediaSubType.Mpeg2Audio;
        //        mediaAudio.sampleSize = 0;
        //        mediaAudio.temporalCompression = false;
        //        mediaAudio.fixedSizeSamples = false; // or false in MediaPortal //true
        //        mediaAudio.unkPtr = IntPtr.Zero;
        //        mediaAudio.formatType = FormatType.WaveEx;

        //        MPEG1WaveFormat audioPinFormat = GetAudioPinFormat();
        //        mediaAudio.formatSize = Marshal.SizeOf(audioPinFormat);
        //        mediaAudio.formatPtr = Marshal.AllocHGlobal(mediaAudio.formatSize);
        //        Marshal.StructureToPtr(audioPinFormat, mediaAudio.formatPtr, false);

        //        IPin pinDemuxerAudio;
        //        int hr = mpeg2Demultiplexer.CreateOutputPin(mediaAudio, "Audio", out pinDemuxerAudio);
        //        if (pinDemuxerAudio != null)
        //            Marshal.ReleaseComObject(pinDemuxerAudio);

        //        Marshal.FreeHGlobal(mediaAudio.formatPtr);
        //    }

        //    {
        //        //Pin 5 connected to "MPEG-2 Sections and Tables" (Allows to grab custom PSI tables)
        //        //    Major Type	MEDIATYPE_MPEG2_SECTIONS {455F176C-4B06-47CE-9AEF-8CAEF73DF7B5}
        //        //    Sub Type		MEDIASUBTYPE_MPEG2DATA {C892E55B-252D-42B5-A316-D997E7A5D995}
        //        //    Format		None

        //        AMMediaType mediaSectionsAndTables = new AMMediaType();
        //        mediaSectionsAndTables.majorType = MediaType.Mpeg2Sections;
        //        mediaSectionsAndTables.subType = MediaSubType.Mpeg2Data;
        //        mediaSectionsAndTables.sampleSize = 0; // 1;
        //        mediaSectionsAndTables.temporalCompression = false;
        //        mediaSectionsAndTables.fixedSizeSamples = true;
        //        mediaSectionsAndTables.unkPtr = IntPtr.Zero;
        //        mediaSectionsAndTables.formatType = FormatType.None;
        //        mediaSectionsAndTables.formatSize = 0;
        //        mediaSectionsAndTables.formatPtr = IntPtr.Zero;

        //        IPin pinDemuxerSectionsAndTables;
        //        int hr = mpeg2Demultiplexer.CreateOutputPin(mediaSectionsAndTables, "PSI", out pinDemuxerSectionsAndTables);
        //        if (pinDemuxerSectionsAndTables != null)
        //            Marshal.ReleaseComObject(pinDemuxerSectionsAndTables);
        //    }


        //}

        //private void AddRenderersManual()
        //{
        //    int hr = 0;
        //    Guid iid = typeof(IBaseFilter).GUID;

        //    //AUDIO

        //    if (this.AudioDevice != null)
        //    {
        //        try
        //        {
        //            this.audioRenderer = null;

        //            IBaseFilter tmp;

        //            object o;
        //            this.AudioDevice.Mon.BindToObject(null, null, iid, out o);
        //            tmp = o as IBaseFilter;

        //            //Adicionar o dispositivo de audio
        //            hr = graphBuilder.AddFilter(tmp, this.AudioDevice.Name + " Manual");

        //            if (hr >= 0)
        //            {
        //                //Got it
        //                this.audioRenderer = tmp;
        //            }
        //            else
        //            {
        //                //Erro, tentar outro
        //                int hr1 = graphBuilder.RemoveFilter(tmp);
        //                Marshal.ReleaseComObject(tmp);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }

        //        if (this.audioRenderer == null)
        //        {
        //            //Adicionar renderer por defeito
        //            this.audioRenderer = (IBaseFilter)new DSoundRender();
        //            hr = graphBuilder.AddFilter(this.audioRenderer, "DirectSound Renderer Manual");
        //            DsError.ThrowExceptionForHR(hr);
        //        }
        //    }

        //    //VIDEO
        //    try
        //    {
        //        this.videoRenderer = (IBaseFilter)new VideoMixingRenderer9();
        //        hr = graphBuilder.AddFilter(this.videoRenderer, "Video Mixing Renderer 9");
        //        DsError.ThrowExceptionForHR(hr);
                
        //    }
        //    catch
        //    {
        //    }
        //}

        //private void ConfigureVMR9InWindowlessModeManual()
        //{
        //    ConfigureVMR9InWindowlessMode();
        //    //int hr;

        //    //IVMRFilterConfig9 filterConfig = this.videoRenderer as IVMRFilterConfig9;
        //    //if (filterConfig != null)
        //    //{
        //    //    // Configure VMR-9 in Windowsless mode
        //    //    hr = filterConfig.SetRenderingMode(VMR9Mode.Windowless);
        //    //    //hr = filterConfig.SetRenderingMode(VMR9Mode.Windowed);
        //    //    DsError.ThrowExceptionForHR(hr);
        //    //}

        //    //IVMRWindowlessControl9 windowlessControl = this.videoRenderer as IVMRWindowlessControl9;
        //    //if (windowlessControl != null)
        //    //{
        //    //    // The main form is hosting the VMR-9
        //    //    hr = windowlessControl.SetVideoClippingWindow(this.hostingControl.GetBaseHandle());
        //    //    DsError.ThrowExceptionForHR(hr);

        //    //    // Keep the aspect-ratio OK
        //    //    //hr = windowlessControl.SetAspectRatioMode(VMR9AspectRatioMode.LetterBox);
        //    //    hr = windowlessControl.SetAspectRatioMode(VMR9AspectRatioMode.None);
        //    //    DsError.ThrowExceptionForHR(hr);
        //    //}

            

        //    //// Add Windows Messages handlers
        //    //AddHandlers();
        //}

        //private int ConnectFiltersManual(IBaseFilter filterIn, IBaseFilter filterOut)
        //{
        //    int hr = 0;
        //    IPin pinOutFromFilterIn = DsFindPin.ByDirection(filterIn, PinDirection.Output, 0);
        //    if (pinOutFromFilterIn != null)
        //    {
        //        IPin pinInFromFilterOut = DsFindPin.ByDirection(filterOut, PinDirection.Input, 0);
        //        if (pinInFromFilterOut != null)
        //        {
        //            hr = this.graphBuilder.Connect(pinOutFromFilterIn, pinInFromFilterOut);
        //            Marshal.ReleaseComObject(pinInFromFilterOut);
        //        }
        //        Marshal.ReleaseComObject(pinOutFromFilterIn);
        //    }

        //    return hr;
        //}
        //protected void ConnectAudioAndVideoFilters()
        //{
        //    int hr = 0;
        //    IPin pinOut;

        //    hr = this.mpeg2Demux.FindPin("H264", out pinOut);
        //    if (pinOut != null)
        //    {
        //        try
        //        {
        //            if (this.h264DecoderFilter == null)
        //            {
        //                IPin pinInFromFilterOut = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
        //                if (pinInFromFilterOut != null)
        //                {
        //                    try
        //                    {
        //                        hr = this.graphBuilder.Connect(pinOut, pinInFromFilterOut);
        //                    }
        //                    finally
        //                    {
        //                        Marshal.ReleaseComObject(pinInFromFilterOut);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                IPin videoDecoderIn = null;
        //                try
        //                {
        //                    videoDecoderIn = DsFindPin.ByDirection(this.h264DecoderFilter, PinDirection.Input, 0);


        //                    //AMMediaType mediaH264 = new AMMediaType();
        //                    //mediaH264.majorType = MediaType.Video;
        //                    ////mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
        //                    //mediaH264.subType = MediaSubType.H264; // new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
        //                    //mediaH264.sampleSize = 0;
        //                    //mediaH264.temporalCompression = true; // false;
        //                    //mediaH264.fixedSizeSamples = true; // false;
        //                    //mediaH264.unkPtr = IntPtr.Zero;
        //                    //mediaH264.formatType = FormatType.Mpeg2Video;

        //                    //MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
        //                    //mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
        //                    //mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
        //                    //Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

        //                    ////IPin pinDemuxerVideoH264;
        //                    ////int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
        //                    ////if (pinDemuxerVideoH264 != null)
        //                    ////Marshal.ReleaseComObject(pinDemuxerVideoH264);
        //                    //hr = this.graphBuilder.ConnectDirect(pinOut, videoDecoderIn, mediaH264);
        //                    ////hr = this.graphBuilder2.Connect(videoDvrOut, videoDecoderIn);
        //                    ////DsError.ThrowExceptionForHR(hr);

        //                    //Marshal.FreeHGlobal(mediaH264.formatPtr);

        //                    //if (hr != 0)
        //                    FilterGraphTools.ConnectFilters(this.graphBuilder, pinOut, videoDecoderIn, false);
        //                }
        //                finally
        //                {
        //                    if (videoDecoderIn != null) Marshal.ReleaseComObject(videoDecoderIn);
        //                }

        //                IPin videoDecoderOut = null, videoVMRIn = null;
        //                try
        //                {
        //                    videoDecoderOut = DsFindPin.ByDirection(this.h264DecoderFilter, PinDirection.Output, 0);
        //                    videoVMRIn = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
        //                    FilterGraphTools.ConnectFilters(this.graphBuilder, videoDecoderOut, videoVMRIn, false);
        //                }
        //                finally
        //                {
        //                    if (videoDecoderOut != null) Marshal.ReleaseComObject(videoDecoderOut);
        //                    if (videoVMRIn != null) Marshal.ReleaseComObject(videoVMRIn);
        //                }
        //            }
        //        }
        //        finally
        //        {
        //            Marshal.ReleaseComObject(pinOut);
        //        }
        //    }

        //    hr = this.mpeg2Demux.FindPin("MPEG2", out pinOut);
        //    if (pinOut != null)
        //    {
        //        try
        //        {
        //            if (this.mpeg2DecoderFilter == null)
        //            {
        //                IPin pinInFromFilterOut = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
        //                if (pinInFromFilterOut != null)
        //                {
        //                    try
        //                    {
        //                        hr = this.graphBuilder.Connect(pinOut, pinInFromFilterOut);
        //                    }
        //                    finally
        //                    {
        //                        Marshal.ReleaseComObject(pinInFromFilterOut);
        //                    }
        //                }
        //            }
        //            else
        //            {

        //                IPin videoDecoderIn = null;
        //                try
        //                {
        //                    videoDecoderIn = DsFindPin.ByDirection(this.mpeg2DecoderFilter, PinDirection.Input, 0);

        //                    FilterGraphTools.ConnectFilters(this.graphBuilder, pinOut, videoDecoderIn, false);
        //                }
        //                finally
        //                {
        //                    if (videoDecoderIn != null) Marshal.ReleaseComObject(videoDecoderIn);
        //                }

        //                IPin videoDecoderOut = null, videoVMRIn = null;
        //                try
        //                {
        //                    videoDecoderOut = DsFindPin.ByDirection(this.mpeg2DecoderFilter, PinDirection.Output, 0);
        //                    videoVMRIn = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
        //                    //FilterGraphTools.ConnectFilters(this.graphBuilder, videoDecoderOut, videoVMRIn, false);
        //                    hr = graphBuilder.ConnectDirect(videoDecoderOut, videoVMRIn, null);
        //                    DsError.ThrowExceptionForHR(hr);
        //                }
        //                finally
        //                {
        //                    if (videoDecoderOut != null) Marshal.ReleaseComObject(videoDecoderOut);
        //                    if (videoVMRIn != null) Marshal.ReleaseComObject(videoVMRIn);
        //                }
        //            }
        //        }
        //        finally
        //        {
        //            Marshal.ReleaseComObject(pinOut);
        //        }
        //    }

        //    hr = this.mpeg2Demux.FindPin("Audio", out pinOut);
        //    if (pinOut != null)
        //    {
        //        hr = this.graphBuilder.Render(pinOut);
        //        //DsError.ThrowExceptionForHR(hr);
        //        Marshal.ReleaseComObject(pinOut);
        //    }
        //}



        ////Não sei se é preciso
        //private void SetMPEG2VideoPinManual(Guid mediaType)
        //{
        //} //FALTA IMPLEMENTAR
        //private void CheckDeinterlaceOptionsManual()
        //{
        //} //FALTA IMPLEMENTAR


        private static readonly Guid bobDxvaGuid = new Guid(0x335aa36e, 0x7884, 0x43a4, 0x9c, 0x91, 0x7f, 0x87, 0xfa, 0xf3, 0xe3, 0x7e);
        //private void Vmr9SetDeinterlaceMode(int mode)
        //{
        //    //0=None
        //    //1=Bob
        //    //2=Weave
        //    //3=Best
        //    //Log("vmr9:SetDeinterlace() SetDeinterlaceMode(%d)",mode);
        //    IVMRDeinterlaceControl9 pDeint = (this.videoRenderer as IVMRDeinterlaceControl9);
        //    if (pDeint != null)
        //    {
        //        //VMR9VideoDesc VideoDesc;
        //        //uint dwNumModes = 0;
        //        Guid deintMode;
        //        int hr;
        //        if (mode == 0)
        //        {
        //            //off
        //            hr = pDeint.SetDeinterlaceMode(-1, Guid.Empty);
        //            //if (!SUCCEEDED(hr)) Log("vmr9:SetDeinterlace() failed hr:0x%x",hr);
        //            hr = pDeint.GetDeinterlaceMode(0, out deintMode);
        //            //if (!SUCCEEDED(hr)) Log("vmr9:GetDeinterlaceMode() failed hr:0x%x",hr);
        //            //Log("vmr9:SetDeinterlace() deinterlace mode OFF: 0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x",
        //            //		deintMode.Data1,deintMode.Data2,deintMode.Data3, deintMode.Data4[0], deintMode.Data4[1], deintMode.Data4[2], deintMode.Data4[3], deintMode.Data4[4], deintMode.Data4[5], deintMode.Data4[6], deintMode.Data4[7]);

        //            return;
        //        }
        //        if (mode == 1)
        //        {
        //            //BOB

        //            hr = pDeint.SetDeinterlaceMode(-1, bobDxvaGuid);
        //            //if (!SUCCEEDED(hr)) Log("vmr9:SetDeinterlace() failed hr:0x%x",hr);
        //            hr = pDeint.GetDeinterlaceMode(0, out deintMode);
        //            //if (!SUCCEEDED(hr)) Log("vmr9:GetDeinterlaceMode() failed hr:0x%x",hr);
        //            //Log("vmr9:SetDeinterlace() deinterlace mode BOB: 0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x",
        //            //        deintMode.Data1,deintMode.Data2,deintMode.Data3, deintMode.Data4[0], deintMode.Data4[1], deintMode.Data4[2], deintMode.Data4[3], deintMode.Data4[4], deintMode.Data4[5], deintMode.Data4[6], deintMode.Data4[7]);

        //            return;
        //        }
        //        if (mode == 2)
        //        {
        //            //WEAVE
        //            hr = pDeint.SetDeinterlaceMode(-1, Guid.Empty);
        //            //if (!SUCCEEDED(hr)) Log("vmr9:SetDeinterlace() failed hr:0x%x",hr);
        //            hr = pDeint.GetDeinterlaceMode(0, out deintMode);
        //            //if (!SUCCEEDED(hr)) Log("vmr9:GetDeinterlaceMode() failed hr:0x%x",hr);
        //            //Log("vmr9:SetDeinterlace() deinterlace mode WEAVE: 0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x",
        //            //        deintMode.Data1,deintMode.Data2,deintMode.Data3, deintMode.Data4[0], deintMode.Data4[1], deintMode.Data4[2], deintMode.Data4[3], deintMode.Data4[4], deintMode.Data4[5], deintMode.Data4[6], deintMode.Data4[7]);

        //            return;
        //        }

        //        //    AM_MEDIA_TYPE pmt;
        //        //    ULONG fetched;
        //        //    IPin* pins[10];
        //        //    CComPtr<IEnumPins> pinEnum;
        //        //    hr=m_pVMR9Filter->EnumPins(&pinEnum);
        //        //    pinEnum->Reset();
        //        //    pinEnum->Next(1,&pins[0],&fetched);
        //        //    hr=pins[0]->ConnectionMediaType(&pmt);
        //        //    pins[0]->Release();

        //        //    VIDEOINFOHEADER2* vidInfo2 =(VIDEOINFOHEADER2*)pmt.pbFormat;
        //        //    if (vidInfo2==NULL)
        //        //    {
        //        //        Log("vmr9:SetDeinterlace() VMR9 not connected");
        //        //        return ;
        //        //    }
        //        //    if ((pmt.formattype != FORMAT_VideoInfo2) || (pmt.cbFormat< sizeof(VIDEOINFOHEADER2)))
        //        //    {
        //        //        Log("vmr9:SetDeinterlace() not using VIDEOINFOHEADER2");

        //        //        return ;
        //        //    }

        //        //    Log("vmr9:SetDeinterlace() resolution:%dx%d planes:%d bitcount:%d fmt:%d %c%c%c%c",
        //        //        vidInfo2->bmiHeader.biWidth,vidInfo2->bmiHeader.biHeight,
        //        //        vidInfo2->bmiHeader.biPlanes,
        //        //        vidInfo2->bmiHeader.biBitCount,
        //        //        vidInfo2->bmiHeader.biCompression,
        //        //        (char)(vidInfo2->bmiHeader.biCompression&0xff),
        //        //        (char)((vidInfo2->bmiHeader.biCompression>>8)&0xff),
        //        //        (char)((vidInfo2->bmiHeader.biCompression>>16)&0xff),
        //        //        (char)((vidInfo2->bmiHeader.biCompression>>24)&0xff)
        //        //        );
        //        //    char major[128];
        //        //    char subtype[128];
        //        //    strcpy(major,"unknown");
        //        //    sprintf(subtype,"unknown (0x%x-0x%x-0x%x-0x%x)",pmt.subtype.Data1,pmt.subtype.Data2,pmt.subtype.Data3,pmt.subtype.Data4);
        //        //    if (pmt.majortype==MEDIATYPE_AnalogVideo)
        //        //        strcpy(major,"Analog video");
        //        //    if (pmt.majortype==MEDIATYPE_Video)
        //        //        strcpy(major,"video");
        //        //    if (pmt.majortype==MEDIATYPE_Stream)
        //        //        strcpy(major,"stream");

        //        //    if (pmt.subtype==MEDIASUBTYPE_MPEG2_VIDEO)
        //        //        strcpy(subtype,"mpeg2 video");
        //        //    if (pmt.subtype==MEDIASUBTYPE_MPEG1System)
        //        //        strcpy(subtype,"mpeg1 system");
        //        //    if (pmt.subtype==MEDIASUBTYPE_MPEG1VideoCD)
        //        //        strcpy(subtype,"mpeg1 videocd");

        //        //    if (pmt.subtype==MEDIASUBTYPE_MPEG1Packet)
        //        //        strcpy(subtype,"mpeg1 packet");
        //        //    if (pmt.subtype==MEDIASUBTYPE_MPEG1Payload )
        //        //        strcpy(subtype,"mpeg1 payload");
        //        ////	if (pmt.subtype==MEDIASUBTYPE_ATSC_SI)
        //        ////		strcpy(subtype,"ATSC SI");
        //        ////	if (pmt.subtype==MEDIASUBTYPE_DVB_SI)
        //        ////		strcpy(subtype,"DVB SI");
        //        ////	if (pmt.subtype==MEDIASUBTYPE_MPEG2DATA)
        //        ////		strcpy(subtype,"MPEG2 Data");
        //        //    if (pmt.subtype==MEDIASUBTYPE_MPEG2_TRANSPORT)
        //        //        strcpy(subtype,"MPEG2 Transport");
        //        //    if (pmt.subtype==MEDIASUBTYPE_MPEG2_PROGRAM)
        //        //        strcpy(subtype,"MPEG2 Program");

        //        //    if (pmt.subtype==MEDIASUBTYPE_CLPL)
        //        //        strcpy(subtype,"MEDIASUBTYPE_CLPL");
        //        //    if (pmt.subtype==MEDIASUBTYPE_YUYV)
        //        //        strcpy(subtype,"MEDIASUBTYPE_YUYV");
        //        //    if (pmt.subtype==MEDIASUBTYPE_IYUV)
        //        //        strcpy(subtype,"MEDIASUBTYPE_IYUV");
        //        //    if (pmt.subtype==MEDIASUBTYPE_YVU9)
        //        //        strcpy(subtype,"MEDIASUBTYPE_YVU9");
        //        //    if (pmt.subtype==MEDIASUBTYPE_Y411)
        //        //        strcpy(subtype,"MEDIASUBTYPE_Y411");
        //        //    if (pmt.subtype==MEDIASUBTYPE_Y41P)
        //        //        strcpy(subtype,"MEDIASUBTYPE_Y41P");
        //        //    if (pmt.subtype==MEDIASUBTYPE_YUY2)
        //        //        strcpy(subtype,"MEDIASUBTYPE_YUY2");
        //        //    if (pmt.subtype==MEDIASUBTYPE_YVYU)
        //        //        strcpy(subtype,"MEDIASUBTYPE_YVYU");
        //        //    if (pmt.subtype==MEDIASUBTYPE_UYVY)
        //        //        strcpy(subtype,"MEDIASUBTYPE_UYVY");
        //        //    if (pmt.subtype==MEDIASUBTYPE_Y211)
        //        //        strcpy(subtype,"MEDIASUBTYPE_Y211");
        //        //    if (pmt.subtype==MEDIASUBTYPE_RGB565)
        //        //        strcpy(subtype,"MEDIASUBTYPE_RGB565");
        //        //    if (pmt.subtype==MEDIASUBTYPE_RGB32)
        //        //        strcpy(subtype,"MEDIASUBTYPE_RGB32");
        //        //    if (pmt.subtype==MEDIASUBTYPE_ARGB32)
        //        //        strcpy(subtype,"MEDIASUBTYPE_ARGB32");
        //        //    if (pmt.subtype==MEDIASUBTYPE_RGB555)
        //        //        strcpy(subtype,"MEDIASUBTYPE_RGB555");
        //        //    if (pmt.subtype==MEDIASUBTYPE_RGB24)
        //        //        strcpy(subtype,"MEDIASUBTYPE_RGB24");
        //        //    if (pmt.subtype==MEDIASUBTYPE_AYUV)
        //        //        strcpy(subtype,"MEDIASUBTYPE_AYUV");
        //        //    if (pmt.subtype==MEDIASUBTYPE_YV12)
        //        //        strcpy(subtype,"MEDIASUBTYPE_YV12");
        //        ////	if (pmt.subtype==MEDIASUBTYPE_NV12)
        //        ////		strcpy(subtype,"MEDIASUBTYPE_NV12");
        //        //    Log("vmr9:SetDeinterlace() major:%s subtype:%s", major,subtype);
        //        //    VideoDesc.dwSize = sizeof(VMR9VideoDesc);
        //        //    VideoDesc.dwFourCC=vidInfo2->bmiHeader.biCompression;
        //        //    VideoDesc.dwSampleWidth=vidInfo2->bmiHeader.biWidth;
        //        //    VideoDesc.dwSampleHeight=vidInfo2->bmiHeader.biHeight;
        //        //    VideoDesc.SampleFormat=ConvertInterlaceFlags(vidInfo2->dwInterlaceFlags);
        //        //    VideoDesc.InputSampleFreq.dwDenominator=(DWORD)vidInfo2->AvgTimePerFrame;
        //        //    VideoDesc.InputSampleFreq.dwNumerator=10000000;
        //        //    VideoDesc.OutputFrameFreq.dwDenominator=(DWORD)vidInfo2->AvgTimePerFrame;
        //        //    VideoDesc.OutputFrameFreq.dwNumerator=VideoDesc.InputSampleFreq.dwNumerator;
        //        //    if (VideoDesc.SampleFormat != VMR9_SampleProgressiveFrame)
        //        //    {
        //        //        VideoDesc.OutputFrameFreq.dwNumerator=2*VideoDesc.InputSampleFreq.dwNumerator;
        //        //    }

        //        //    // Fill in the VideoDesc structure (not shown).
        //        //    hr = pDeint->GetNumberOfDeinterlaceModes(&VideoDesc, &dwNumModes, NULL);
        //        //    if (SUCCEEDED(hr) && (dwNumModes != 0))
        //        //    {
        //        //        // Allocate an array for the GUIDs that identify the modes.
        //        //        GUID *pModes = new GUID[dwNumModes];
        //        //        if (pModes)
        //        //        {
        //        //            Log("vmr9:SetDeinterlace() found %d deinterlacing modes", dwNumModes);
        //        //            // Fill the array.
        //        //            hr = pDeint->GetNumberOfDeinterlaceModes(&VideoDesc, &dwNumModes, pModes);
        //        //            if (SUCCEEDED(hr))
        //        //            {
        //        //                // Loop through each item and get the capabilities.
        //        //                for (int i = 0; i < dwNumModes; i++)
        //        //                {
        //        //                    hr=pDeint->SetDeinterlaceMode(0xFFFFFFFF,&pModes[0]);
        //        //                    if (SUCCEEDED(hr))
        //        //                    {
        //        //                        Log("vmr9:SetDeinterlace() set deinterlace mode:%d",i);



        //        //                        pDeint->GetDeinterlaceMode(0,&deintMode);
        //        //                        if (deintMode.Data1==pModes[0].Data1 &&
        //        //                            deintMode.Data2==pModes[0].Data2 &&
        //        //                            deintMode.Data3==pModes[0].Data3 &&
        //        //                            deintMode.Data4==pModes[0].Data4)
        //        //                        {
        //        //                            Log("vmr9:SetDeinterlace() succeeded");
        //        //                        }
        //        //                        else
        //        //                            Log("vmr9:SetDeinterlace() deinterlace mode set to: 0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x-0x%x",
        //        //                                    deintMode.Data1,deintMode.Data2,deintMode.Data3, deintMode.Data4[0], deintMode.Data4[1], deintMode.Data4[2], deintMode.Data4[3], deintMode.Data4[4], deintMode.Data4[5], deintMode.Data4[6], deintMode.Data4[7]);
        //        //                        break;
        //        //                    }
        //        //                    else
        //        //                        Log("vmr9:SetDeinterlace() deinterlace mode:%d failed 0x:%x",i,hr);

        //        //                }
        //        //            }
        //        //            delete [] pModes;
        //        //        }
        //        //    }
        //    }
        //}

        #endregion

        #region Guts

        #region STATIC
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

        //protected static VideoInfoHeader2 GetVideoInfoHeader2H264PinFormat()
        //{
        //    //VideoInfoHeader2 hdr = new VideoInfoHeader2();
        //    //hdr.SrcRect = new DsRect();
        //    //hdr.SrcRect.left = 0;		//0x00, 0x00, 0x00, 0x00,  //00  .hdr.rcSource.left              = 0x00000000
        //    //hdr.SrcRect.top = 0;			//0x00, 0x00, 0x00, 0x00,  //04  .hdr.rcSource.top               = 0x00000000
        //    //hdr.SrcRect.right = 0;		//0xD0, 0x02, 0x00, 0x00,  //08  .hdr.rcSource.right             = 0x000002d0 //720
        //    //hdr.SrcRect.bottom = 0;		//0x40, 0x02, 0x00, 0x00,  //0c  .hdr.rcSource.bottom            = 0x00000240 //576
        //    //hdr.TargetRect = new DsRect();
        //    //hdr.TargetRect.left = 0;		//0x00, 0x00, 0x00, 0x00,  //10  .hdr.rcTarget.left              = 0x00000000
        //    //hdr.TargetRect.top = 0;		//0x00, 0x00, 0x00, 0x00,  //14  .hdr.rcTarget.top               = 0x00000000
        //    //hdr.TargetRect.right = 0;	//0xD0, 0x02, 0x00, 0x00,  //18  .hdr.rcTarget.right             = 0x000002d0 //720
        //    //hdr.TargetRect.bottom = 0;	//0x40, 0x02, 0x00, 0x00,  //1c  .hdr.rcTarget.bottom            = 0x00000240// 576
        //    //hdr.BitRate = 0x003d0900;	//0x00, 0x09, 0x3D, 0x00,  //20  .hdr.dwBitRate                  = 0x003d0900
        //    //hdr.BitErrorRate = 0;		//0x00, 0x00, 0x00, 0x00,  //24  .hdr.dwBitErrorRate             = 0x00000000

        //    //////0x051736=333667-> 10000000/333667 = 29.97fps
        //    //////0x061A80=400000-> 10000000/400000 = 25fps
        //    //hdr.AvgTimePerFrame = 400000;				//0x80, 0x1A, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, //28  .hdr.AvgTimePerFrame            = 0x0000000000051763 ->1000000/ 40000 = 25fps
        //    //hdr.InterlaceFlags = AMInterlace.None;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
        //    ////hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.OneFieldPerSample | AMInterlace.DisplayModeBobOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
        //    ////hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.DisplayModeBobOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
        //    ////hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.FieldPatBothRegular | AMInterlace.DisplayModeWeaveOnly;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
        //    ////hdr.InterlaceFlags = AMInterlace.IsInterlaced | AMInterlace.DisplayModeBobOrWeave;		//0x00, 0x00, 0x00, 0x00,                         //2c  .hdr.dwInterlaceFlags           = 0x00000000
        //    //hdr.CopyProtectFlags = AMCopyProtect.None;	//0x00, 0x00, 0x00, 0x00,                         //30  .hdr.dwCopyProtectFlags         = 0x00000000
        //    //hdr.PictAspectRatioX = 0;// 4;					//0x04, 0x00, 0x00, 0x00,                         //34  .hdr.dwPictAspectRatioX         = 0x00000004
        //    //hdr.PictAspectRatioY = 0;// 3;					//0x03, 0x00, 0x00, 0x00,                         //38  .hdr.dwPictAspectRatioY         = 0x00000003
        //    //hdr.ControlFlags = AMControl.None;			//0x00, 0x00, 0x00, 0x00,                         //3c  .hdr.dwReserved1                = 0x00000000
        //    //hdr.Reserved2 = 0;							//0x00, 0x00, 0x00, 0x00,                         //40  .hdr.dwReserved2                = 0x00000000
        //    //hdr.BmiHeader = new BitmapInfoHeader();
        //    //hdr.BmiHeader.Size = 0x00000028;				//0x28, 0x00, 0x00, 0x00,  //44  .hdr.bmiHeader.biSize           = 0x00000028
        //    //hdr.BmiHeader.Width = 1920; // 720;					//0xD0, 0x02, 0x00, 0x00,  //48  .hdr.bmiHeader.biWidth          = 0x000002d0 //720
        //    //hdr.BmiHeader.Height = 1080; // 576;					//0x40, 0x02, 0x00, 0x00,  //4c  .hdr.bmiHeader.biHeight         = 0x00000240 //576
        //    //hdr.BmiHeader.Planes = 0; // 1 ?					//0x00, 0x00,              //50  .hdr.bmiHeader.biPlanes         = 0x0000
        //    //hdr.BmiHeader.BitCount = 0;					//0x00, 0x00,              //54  .hdr.bmiHeader.biBitCount       = 0x0000
        //    //hdr.BmiHeader.Compression = 0;				//0x00, 0x00, 0x00, 0x00,  //58  .hdr.bmiHeader.biCompression    = 0x00000000
        //    //hdr.BmiHeader.ImageSize = 0;					//0x00, 0x00, 0x00, 0x00,  //5c  .hdr.bmiHeader.biSizeImage      = 0x00000000
        //    //hdr.BmiHeader.XPelsPerMeter = 0x000007d0;	//0xD0, 0x07, 0x00, 0x00,  //60  .hdr.bmiHeader.biXPelsPerMeter  = 0x000007d0
        //    //hdr.BmiHeader.YPelsPerMeter = 0x0000cf27;	//0x27, 0xCF, 0x00, 0x00,  //64  .hdr.bmiHeader.biYPelsPerMeter  = 0x0000cf27
        //    //hdr.BmiHeader.ClrUsed = 0;					//0x00, 0x00, 0x00, 0x00,  //68  .hdr.bmiHeader.biClrUsed        = 0x00000000
        //    //hdr.BmiHeader.ClrImportant = 0;				//0x00, 0x00, 0x00, 0x00,  //6c  .hdr.bmiHeader.biClrImportant   = 0x00000000

        //    VideoInfoHeader2 hdr = new VideoInfoHeader2();
        //    hdr.BmiHeader = new BitmapInfoHeader();
        //    hdr.BmiHeader.Size = 28; // 0x00000028;				//0x28, 0x00, 0x00, 0x00,  //44  .hdr.bmiHeader.biSize           = 0x00000028
        //    hdr.BmiHeader.Width = 1920; // 720;
        //    hdr.BmiHeader.Height = 1080; // 576;
        //    hdr.PictAspectRatioX = 0;
        //    hdr.PictAspectRatioY = 0;
        //    hdr.BmiHeader.Planes = 0;
        //    hdr.BmiHeader.BitCount = 24;
        //    hdr.BmiHeader.Compression = 0; //new MediaFoundation.Misc.FourCC("H264").ToInt32();
        //    return hdr;
        //}

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

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public class MPEG1WaveFormat //: WaveFormatEx // produce a bug in ngen 
        {
            // WaveFormatEx
            public short wFormatTag;
            public short nChannels;
            public int nSamplesPerSec;
            public int nAvgBytesPerSec;
            public short nBlockAlign;
            public short wBitsPerSample;
            public short cbSize;

            // MPEG1WaveFormat
            public ushort HeadLayer;
            public uint HeadBitrate;
            public ushort HeadMode;
            public ushort HeadModeExt;
            public ushort HeadEmphasis;
            public ushort HeadFlags;
            public uint PTSLow;
            public uint PTSHigh;
        }

        #endregion

        //protected virtual void CreateMPEG2DemuxPins()
        //{
        //    IMpeg2Demultiplexer mpeg2Demultiplexer = this.mpeg2Demux as IMpeg2Demultiplexer;

        //    {
        //        //Pin 1 connected to the "BDA MPEG2 Transport Information Filter"
        //        //    Major Type	MEDIATYPE_MPEG2_SECTIONS {455F176C-4B06-47CE-9AEF-8CAEF73DF7B5}
        //        //    Sub Type		MEDIASUBTYPE_DVB_SI {E9DD31A3-221D-4ADB-8532-9AF309C1A408}
        //        //    Format		None

        //        //    MPEG2 PSI Sections
        //        //    Pids: 0x00 0x10 0x11 0x12 0x13 0x14 0x6e 0xd2 0x0136 0x019a 0x01fe 0x0262 0x03f2

        //        AMMediaType mediaTIF = new AMMediaType();
        //        mediaTIF.majorType = MediaType.Mpeg2Sections;
        //        mediaTIF.subType = MediaSubType.DvbSI;
        //        mediaTIF.fixedSizeSamples = false;
        //        mediaTIF.temporalCompression = false;
        //        mediaTIF.sampleSize = 1;
        //        mediaTIF.unkPtr = IntPtr.Zero;
        //        mediaTIF.formatType = FormatType.None;
        //        mediaTIF.formatSize = 0;
        //        mediaTIF.formatPtr = IntPtr.Zero;

        //        IPin pinDemuxerTIF;
        //        int hr = mpeg2Demultiplexer.CreateOutputPin(mediaTIF, "TIF", out pinDemuxerTIF);
        //        if (pinDemuxerTIF != null)
        //            Marshal.ReleaseComObject(pinDemuxerTIF);
        //    }

        //    if (this.H264DecoderDevice != null)
        //    {
        //        //Try Original
        //        AMMediaType mediaH264 = new AMMediaType();
        //        mediaH264.majorType = MediaType.Video;
        //        //mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
        //        mediaH264.subType = MediaSubType.H264;// new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
        //        mediaH264.sampleSize = 0;
        //        mediaH264.temporalCompression = true; // false;
        //        mediaH264.fixedSizeSamples = true; // false;
        //        mediaH264.unkPtr = IntPtr.Zero;
        //        mediaH264.formatType = FormatType.Mpeg2Video;

        //        MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
        //        mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
        //        mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
        //        Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

        //        IPin pinDemuxerVideoH264;
        //        int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
        //        if (pinDemuxerVideoH264 != null)
        //            Marshal.ReleaseComObject(pinDemuxerVideoH264);

        //        Marshal.FreeHGlobal(mediaH264.formatPtr);

        //        ////Try 1
        //        //AMMediaType mediaH264 = new AMMediaType();
        //        //mediaH264.majorType = MediaType.Null;
        //        ////mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
        //        //mediaH264.subType = MediaSubType.H264;// new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
        //        //mediaH264.sampleSize = 0;
        //        //mediaH264.temporalCompression = true; // false;
        //        //mediaH264.fixedSizeSamples = true; // false;
        //        //mediaH264.unkPtr = IntPtr.Zero;
        //        //mediaH264.formatType = FormatType.Null;

        //        ////MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
        //        ////mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
        //        ////mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
        //        ////Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

        //        //IPin pinDemuxerVideoH264;
        //        //int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
        //        //if (pinDemuxerVideoH264 != null)
        //        //    Marshal.ReleaseComObject(pinDemuxerVideoH264);

        //        ////Marshal.FreeHGlobal(mediaH264.formatPtr);


        //        ////Try http://mheg2xmltv.googlecode.com/svn/trunk/dcdvbsource/Source/Filter/DVBGraphBuilder.pas
        //        //AMMediaType mediaH264 = new AMMediaType();
        //        //mediaH264.majorType = MediaType.Video;
        //        ////mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
        //        //mediaH264.subType = MediaSubType.H264;// new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
        //        //mediaH264.sampleSize = 1; // 0;
        //        //mediaH264.temporalCompression = true; // false;
        //        //mediaH264.fixedSizeSamples = false; // true; // false;
        //        //mediaH264.unkPtr = IntPtr.Zero;
        //        //mediaH264.formatType = FormatType.VideoInfo2; // FormatType.VideoInfo2;//FormatType.Mpeg2Video;

        //        //VideoInfoHeader2 videoH264PinFormat = GetVideoInfoHeader2H264PinFormat(); // GetVideoH264PinFormat();
        //        //mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
        //        //mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
        //        //Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

        //        //IPin pinDemuxerVideoH264;
        //        //int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
        //        //if (pinDemuxerVideoH264 != null)
        //        //    Marshal.ReleaseComObject(pinDemuxerVideoH264);

        //        //Marshal.FreeHGlobal(mediaH264.formatPtr);

        //    }
        //    else
        //    {
        //        AMMediaType mediaMPG2 = new AMMediaType();
        //        mediaMPG2.majorType = MediaType.Video;
        //        mediaMPG2.subType = MediaSubType.Mpeg2Video;
        //        mediaMPG2.fixedSizeSamples = false;
        //        mediaMPG2.temporalCompression = false; // true???
        //        mediaMPG2.sampleSize = 0;
        //        mediaMPG2.formatType = FormatType.Mpeg2Video;
        //        mediaMPG2.unkPtr = IntPtr.Zero;

        //        MPEG2VideoInfo videoMPEG2PinFormat = GetVideoMPEG2PinFormat();
        //        mediaMPG2.formatSize = Marshal.SizeOf(videoMPEG2PinFormat);
        //        mediaMPG2.formatPtr = Marshal.AllocHGlobal(mediaMPG2.formatSize);
        //        Marshal.StructureToPtr(videoMPEG2PinFormat, mediaMPG2.formatPtr, false);

        //        IPin pinDemuxerVideoMPEG2;
        //        int hr = mpeg2Demultiplexer.CreateOutputPin(mediaMPG2, "MPG2", out pinDemuxerVideoMPEG2);
        //        if (pinDemuxerVideoMPEG2 != null)
        //            Marshal.ReleaseComObject(pinDemuxerVideoMPEG2);

        //        Marshal.FreeHGlobal(mediaMPG2.formatPtr);
        //    }

        //    {
        //        AMMediaType mediaAudio = new AMMediaType();
        //        mediaAudio.majorType = MediaType.Audio;
        //        if (this.AudioDecoderType == ChannelDVB.AudioType.AC3)
        //            mediaAudio.subType = MediaSubType.DolbyAC3;
        //        else if (this.AudioDecoderType == ChannelDVB.AudioType.EAC3)
        //        {
        //            //EAC3
        //            //http://social.msdn.microsoft.com/Forums/en-US/windowsdirectshowdevelopment/thread/64f5b2ef-9ec6-408c-9a86-6e1355bea717/
        //            Guid MEDIASUBTYPE_DDPLUS_AUDIO = new Guid("a7fb87af-2d02-42fb-a4d4-05cd93843bdd");
        //            mediaAudio.subType = MEDIASUBTYPE_DDPLUS_AUDIO;
        //            //mediaAudio.subType = MediaSubType.DolbyAC3;
        //            //Guid GuidDolbyEAC3 = new Guid("{33434145-0000-0010-8000-00AA00389B71}");
        //            //#define WAVE_FORMAT_DOLBY_EAC3 0x33434145
        //            //DEFINE_GUID(MEDIASUBTYPE_DOLBY_EAC3, WAVE_FORMAT_DOLBY_EAC3, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
        //            //mediaAudio.subType = MediaSubType.Mpeg2Audio;
        //        }
        //        else
        //            mediaAudio.subType = MediaSubType.Mpeg2Audio;
        //        mediaAudio.sampleSize = 0;
        //        mediaAudio.temporalCompression = false;
        //        mediaAudio.fixedSizeSamples = false; // or false in MediaPortal //true
        //        mediaAudio.unkPtr = IntPtr.Zero;
        //        mediaAudio.formatType = FormatType.WaveEx;

        //        MPEG1WaveFormat audioPinFormat = GetAudioPinFormat();
        //        mediaAudio.formatSize = Marshal.SizeOf(audioPinFormat);
        //        mediaAudio.formatPtr = Marshal.AllocHGlobal(mediaAudio.formatSize);
        //        Marshal.StructureToPtr(audioPinFormat, mediaAudio.formatPtr, false);

        //        IPin pinDemuxerAudio;
        //        int hr = mpeg2Demultiplexer.CreateOutputPin(mediaAudio, "Audio", out pinDemuxerAudio);
        //        if (pinDemuxerAudio != null)
        //            Marshal.ReleaseComObject(pinDemuxerAudio);

        //        Marshal.FreeHGlobal(mediaAudio.formatPtr);
        //    }

        //    {
        //        //Pin 5 connected to "MPEG-2 Sections and Tables" (Allows to grab custom PSI tables)
        //        //    Major Type	MEDIATYPE_MPEG2_SECTIONS {455F176C-4B06-47CE-9AEF-8CAEF73DF7B5}
        //        //    Sub Type		MEDIASUBTYPE_MPEG2DATA {C892E55B-252D-42B5-A316-D997E7A5D995}
        //        //    Format		None

        //        AMMediaType mediaSectionsAndTables = new AMMediaType();
        //        mediaSectionsAndTables.majorType = MediaType.Mpeg2Sections;
        //        mediaSectionsAndTables.subType = MediaSubType.Mpeg2Data;
        //        mediaSectionsAndTables.sampleSize = 0; // 1;
        //        mediaSectionsAndTables.temporalCompression = false;
        //        mediaSectionsAndTables.fixedSizeSamples = true;
        //        mediaSectionsAndTables.unkPtr = IntPtr.Zero;
        //        mediaSectionsAndTables.formatType = FormatType.None;
        //        mediaSectionsAndTables.formatSize = 0;
        //        mediaSectionsAndTables.formatPtr = IntPtr.Zero;

        //        IPin pinDemuxerSectionsAndTables;
        //        int hr = mpeg2Demultiplexer.CreateOutputPin(mediaSectionsAndTables, "PSI", out pinDemuxerSectionsAndTables);
        //        if (pinDemuxerSectionsAndTables != null)
        //            Marshal.ReleaseComObject(pinDemuxerSectionsAndTables);
        //    }
        //}

        private void BuildGraphForChannels(ITuningSpace tuningSpace)
        {
            this.graphBuilder = (IFilterGraph2)new FilterGraph();
            Log("Building graph to extract channels");

            AddNetworkProviderFilter(tuningSpace);
            AddMPEG2DemuxFilter();
            AddAndConnectBDABoardFilters();
            AddTransportStreamFilters();
            ConnectFiltersForChannels();
        }

        private void BuildGraph(ITuningSpace tuningSpace)
        {
            this.graphBuilder = (IFilterGraph2)new FilterGraph();
#if DEBUG
            rot = new DsROTEntry(this.graphBuilder);
#endif

            Log("Building playback graph");
            //Method names should be self explanatory
            AddNetworkProviderFilter(tuningSpace);
            AddMPEG2DemuxFilter();
            AddAndConnectBDABoardFilters();
            AddTransportStreamFilters();
            AddRenderers();
            switch (VideoRendererDevice)
            {
                case VideoRenderer.VMR9: 
                    ConfigureVMR9InWindowlessMode();
                    break;
                case VideoRenderer.EVR:
                    ConfigureEVR();
                    break;
            }
            SetH264VideoPin();
            ConnectFilters();
            SelectDeinterlaceMode();
        }

        private void AddNetworkProviderFilter(ITuningSpace tuningSpace)
        {
            Log("Adding Network Provider Filter");

            int hr = 0;
            Guid genProviderClsId = new Guid("{B2F3A67C-29DA-4C78-8831-091ED509A475}");
            Guid networkProviderClsId;

            // First test if the Generic Network Provider is available (only on MCE 2005 + Update Rollup 2 and higher)
            if (FilterGraphTools.IsThisComObjectInstalled(genProviderClsId))
            {
                Log("Using Generic Network Provider");
                this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, genProviderClsId, "Generic Network Provider");

                hr = (this.networkProvider as ITuner).put_TuningSpace(tuningSpace);
                return;
            }

            // Get the network type of the requested Tuning Space
            hr = tuningSpace.get__NetworkType(out networkProviderClsId);

            // Get the network type of the requested Tuning Space
            if (networkProviderClsId == typeof(DVBTNetworkProvider).GUID)
            {
                this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, networkProviderClsId, "DVBT Network Provider");
                Log("Using DVBT Network Provider");
            }
            else if (networkProviderClsId == typeof(DVBSNetworkProvider).GUID)
            {
                this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, networkProviderClsId, "DVBS Network Provider");
                Log("Using DVBS Network Provider");
            }
            else if (networkProviderClsId == typeof(ATSCNetworkProvider).GUID)
            {
                this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, networkProviderClsId, "ATSC Network Provider");
                Log("Using ATSC Network Provider");
            }
            else if (networkProviderClsId == typeof(DVBCNetworkProvider).GUID)
            {
                this.networkProvider = FilterGraphTools.AddFilterFromClsid(this.graphBuilder, networkProviderClsId, "DVBC Network Provider");
                Log("Using DVBC Network Provider");
            }
            else
                // Tuning Space can also describe Analog TV but this application don't support them
                throw new ArgumentException("This application doesn't support analog TV");

            hr = (this.networkProvider as ITuner).put_TuningSpace(tuningSpace);

            Log("Added Network Provider Filter");
        }
        private void AddMPEG2DemuxFilter()
        {
            Log("Adding MPEG2 Demux Filter");

            int hr = 0;

            this.mpeg2Demux = (IBaseFilter)new MPEG2Demultiplexer();

            hr = this.graphBuilder.AddFilter(this.mpeg2Demux, "MPEG2 Demultiplexer");
            DsError.ThrowExceptionForHR(hr);

            Log("Added Network Provider Filter");
        }
        private void SetPinFormat(IPin pin)
        {
            AMMediaType amMediaType = new AMMediaType();

            amMediaType.majorType = MediaType.Video;
            amMediaType.subType = MediaSubType.H264;
            amMediaType.sampleSize = 0;
            amMediaType.temporalCompression = true;
            amMediaType.fixedSizeSamples = true;
            amMediaType.unkPtr = IntPtr.Zero;
            amMediaType.formatType = FormatType.VideoInfo2;

            MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
            amMediaType.formatSize = Marshal.SizeOf(videoH264PinFormat);
            amMediaType.formatPtr = Marshal.AllocHGlobal(amMediaType.formatSize);
            Marshal.StructureToPtr(videoH264PinFormat, amMediaType.formatPtr, false);

            PinInfo info;

            pin.QueryPinInfo(out info);

            (mpeg2Demux as IMpeg2Demultiplexer).SetOutputPinMediaType(info.name, amMediaType);
        }
        private void SetH264VideoPin()
        {
            int hr = 0;

            AMMediaType amMediaType = new AMMediaType();

            amMediaType.majorType = MediaType.Video;
            amMediaType.subType = MediaSubType.H264;
            amMediaType.sampleSize = 0;
            amMediaType.temporalCompression = true;
            amMediaType.fixedSizeSamples = true;
            amMediaType.unkPtr = IntPtr.Zero;
            amMediaType.formatType = FormatType.Mpeg2Video;

            MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
            amMediaType.formatSize = Marshal.SizeOf(videoH264PinFormat);
            amMediaType.formatPtr = Marshal.AllocHGlobal(amMediaType.formatSize);
            Marshal.StructureToPtr(videoH264PinFormat, amMediaType.formatPtr, false);

            int pinCounter = 0;
            IPin pinMPEG2DemuxAntigo = null;
            bool pinFound = false;

            while (!pinFound)
            {
                pinMPEG2DemuxAntigo = DsFindPin.ByDirection(mpeg2Demux, PinDirection.Output, pinCounter);
                if (pinMPEG2DemuxAntigo == null) break;

                IEnumMediaTypes enumMediaTypes = null;
                AMMediaType[] mediaTypes = new AMMediaType[1];

                try
                {
                    hr = pinMPEG2DemuxAntigo.EnumMediaTypes(out enumMediaTypes);
                    DsError.ThrowExceptionForHR(hr);

                    while (enumMediaTypes.Next(mediaTypes.Length, mediaTypes, IntPtr.Zero) == 0)
                    {
                        // Store the majortype and the subtype and free the structure
                        Guid majorType = mediaTypes[0].majorType;
                        Guid subType = mediaTypes[0].subType;
                        DsUtils.FreeAMMediaType(mediaTypes[0]);


                        if (majorType == MediaType.Video)
                        {
                            pinFound = true; //Encontrámos o pin que passa o vídeo
                        }
                    }
                }
                catch
                {
                }

                if (!pinFound)
                {
                    Marshal.ReleaseComObject(enumMediaTypes);
                    enumMediaTypes = null;

                    Marshal.ReleaseComObject(pinMPEG2DemuxAntigo);
                    pinMPEG2DemuxAntigo = null;
                }

                pinCounter++;
            }//Já temos o pin 

            if (!pinFound) return; //Só entra aqui se não encontrar um pin de vídeo com o ciclo de cima

            PinInfo info;

            pinMPEG2DemuxAntigo.QueryPinInfo(out info);

            (mpeg2Demux as IMpeg2Demultiplexer).SetOutputPinMediaType(info.name, amMediaType);

            if (pinMPEG2DemuxAntigo != null)
                Marshal.ReleaseComObject(pinMPEG2DemuxAntigo);

            Marshal.FreeHGlobal(amMediaType.formatPtr);


        } //TODO COMENTADO
        private void AddAndConnectBDABoardFilters()
        {
            Log("Adding BDA Board Filters");

            int hr = 0;
            DsDevice[] devices;

            ICaptureGraphBuilder2 capBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            capBuilder.SetFiltergraph(this.graphBuilder);

            try
            {
                // Enumerate BDA Source filters category and found one that can connect to the network provider
                devices = DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory);
                Log(string.Format("Found {0} device{1}", devices.Length, devices.Length == 1 ? "" : "s"));

                for (int i = 0; i < devices.Length; i++)
                {
                    IBaseFilter tmp;

                    hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out tmp);

                    Log(string.Format("Device {0}: {1} {2}", i, devices[i].Name, hr == 0 ? "added successfully" : "failed to add"));

                    DsError.ThrowExceptionForHR(hr);

                    hr = capBuilder.RenderStream(null, null, this.networkProvider, null, tmp);
                    if (hr == 0)
                    {
                        // Got it !
                        this.tuner = tmp; 
                        Log(string.Format("Using device {0}", devices[i].Name));
                        break;
                    }
                    else
                    {
                        Log(string.Format("Failed to render device {0}: {1}", i, devices[i].Name));

                        // Try another...
                        hr = graphBuilder.RemoveFilter(tmp);
                        Log(string.Format("Device {0}: {1} {2}", i, devices[i].Name, hr == 0 ? "removed successfully" : "failed to remove"));
                        DsError.ThrowExceptionForHR(hr);

                        Marshal.ReleaseComObject(tmp);
                    }
                }

                if (this.tuner == null)
                {
                    Log("Can't find a valid BDA tuner");
                    throw new ApplicationException("Can't find a valid BDA tuner");
                }

                Log("Connecting BDA Board Filters");

                // trying to connect this filter to the MPEG-2 Demux
                hr = capBuilder.RenderStream(null, null, tuner, null, mpeg2Demux);
                if (hr >= 0)
                {
                    Log("The selected device uses a one filter model");
                    // this is a one filter model
                    this.demodulator = null;
                    this.capture = null;
                    return;
                }
                else
                {
                    Log("The selected device doesn't use a one filter model");
                    Log("Finding a filter to connect the tuner and the MPEG2 demux");
                    // Then enumerate BDA Receiver Components category to find a filter connecting 
                    // to the tuner and the MPEG2 Demux
                    devices = DsDevice.GetDevicesOfCat(FilterCategory.BDAReceiverComponentsCategory);

                    Log(string.Format("Found {0} device{1}", devices.Length, devices.Length == 1 ? "" : "s"));

                    for (int i = 0; i < devices.Length; i++)
                    {
                        IBaseFilter tmp;

                        Log(string.Format("Device {0}: {1}", i, devices[i].Name));

                        hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out tmp);

                        Log(string.Format("Device {0}: {1} {2}", i, devices[i].Name, hr == 0 ? "added successfully" : "failed to add"));

                        DsError.ThrowExceptionForHR(hr);

                        hr = capBuilder.RenderStream(null, null, this.tuner, null, tmp);
                        if (hr == 0)
                        {
                            // Got it !
                            this.capture = tmp;

                            // Connect it to the MPEG-2 Demux
                            hr = capBuilder.RenderStream(null, null, this.capture, null, this.mpeg2Demux);
                            if (hr >= 0)
                            {
                                Log(string.Format("Capture filter found ({0})", devices[i].Name));
                                // This second filter connect both with the tuner and the demux.
                                // This is a capture filter...
                                return;
                            }
                            else
                            {
                                // This second filter connect with the tuner but not with the demux.
                                // This is in fact a demodulator filter. We now must find the true capture filter...
                                Log(string.Format("Demodulator filter found ({0})", devices[i].Name));
                                Log("Finding the capture filter");
                                this.demodulator = this.capture;
                                this.capture = null;

                                // saving the Demodulator's DevicePath to avoid creating it twice.
                                string demodulatorDevicePath = devices[i].DevicePath;

                                for (int j = 0; j < devices.Length; j++)
                                {
                                    if (devices[j].DevicePath.Equals(demodulatorDevicePath))
                                        continue;

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

                                            Log(string.Format("Capture filter found ({0})", devices[i].Name));
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        Log(string.Format("Failed to render device {0}: {1}", i, devices[i].Name));

                                        // Try another...
                                        hr = graphBuilder.RemoveFilter(tmp);
                                        Log(string.Format("Device {0}: {1} {2}", i, devices[i].Name, hr == 0 ? "removed successfully" : "failed to remove"));
                                        Marshal.ReleaseComObject(tmp);
                                    }
                                } // for j

                                // We have a tuner and a capture/demodulator that don't connect with the demux
                                // and we found no additionals filters to build a working filters chain.
                                Log("Can't find a valid BDA filter chain");
                                throw new ApplicationException("Can't find a valid BDA filter chain");
                            }
                        }
                        else
                        {
                            // Try another...
                            hr = graphBuilder.RemoveFilter(tmp);
                            Log(string.Format("Device {0}: {1} {2}", i, devices[i].Name, hr == 0 ? "removed successfully" : "failed to remove"));
                            Marshal.ReleaseComObject(tmp);
                        }
                    } // for i

                    Log("The tuner can't connect with the demux");
                    // We have a tuner that connect to the Network Provider BUT not with the demux
                    // and we found no additionals filters to build a working filters chain.
                    throw new ApplicationException("Can't find a valid BDA filter chain");
                }
            }
            finally
            {
                Marshal.ReleaseComObject(capBuilder);
            }
        }
        private void AddTransportStreamFilters()
        {
            Log("Adding the transport stream filters");

            int hr = 0;
            DsDevice[] devices;

            // Add two filters needed in a BDA graph
            devices = DsDevice.GetDevicesOfCat(FilterCategory.BDATransportInformationRenderersCategory);

            Log(string.Format("Found {0} device{1}", devices.Length, devices.Length == 1 ? "" : "s"));

            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i].Name.Equals("BDA MPEG2 Transport Information Filter"))
                {
                    hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out this.bdaTIF);
                    Log(string.Format("Device {0}: {1} {2}", i, devices[i].Name, hr == 0 ? "added successfully" : "failed to add"));

                    DsError.ThrowExceptionForHR(hr);
                    continue;
                }

                if (devices[i].Name.Equals("MPEG-2 Sections and Tables"))
                {
                    hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out this.bdaSecTab);
                    Log(string.Format("Device {0}: {1} {2}", i, devices[i].Name, hr == 0 ? "added successfully" : "failed to add"));

                    DsError.ThrowExceptionForHR(hr);
                    continue;
                }
            }
        }
        private void AddRenderers()
        {
            Log("Adding Renderers");
            int hr = 0;

            // To hear something
            this.audioRenderer = (IBaseFilter)new DSoundRender();

            hr = this.graphBuilder.AddFilter(this.audioRenderer, "DirectSound Renderer");
            Log("DirectSound Renderer added");

            DsError.ThrowExceptionForHR(hr);

            // To see something
            switch(VideoRendererDevice)
            {
                case VideoRenderer.VMR9:
                    this.videoRenderer = (IBaseFilter)new VideoMixingRenderer9();

                    hr = this.graphBuilder.AddFilter(this.videoRenderer, "Video Mixing Renderer Filter 9");
                    Log("Video Mixing Renderer Filter 9 added");
                    break;
                case VideoRenderer.EVR:
                    this.videoRenderer = (IBaseFilter)new EnhancedVideoRenderer();

                    hr = this.graphBuilder.AddFilter(this.videoRenderer, "Enhanced Video Renderer OMG");
                    Log("Enhanced Video Renderer added");
                    break;
            }

            DsError.ThrowExceptionForHR(hr);

            Log("Renderers added successfully");
        }
        private void ConfigureEVR()
        {
            int hr = 0;
            Log("Configuring EVR Filter");

            object o;
            IMFGetService pGetService = null;
            pGetService = (IMFGetService)this.videoRenderer;

            hr = pGetService.GetService(MFServices.MR_VIDEO_RENDER_SERVICE, typeof(IMFVideoDisplayControl).GUID, out o);

            try
            {
                evrVideoDisplayControl = (IMFVideoDisplayControl)o;
            }
            catch
            {
                Marshal.ReleaseComObject(o);
                throw;
            }

            try
            {
                // Set the number of streams.
                hr = evrVideoDisplayControl.SetVideoWindow(this.hostingControl.GetBaseHandle());
                IEVRFilterConfig evrFilterConfig;
                evrFilterConfig = (IEVRFilterConfig)this.videoRenderer;
                hr = evrFilterConfig.SetNumberOfStreams(1);

                // Keep the aspect-ratio OK
                hr = evrVideoDisplayControl.SetAspectRatioMode(MFVideoAspectRatioMode.None); // VMR9AspectRatioMode.None);
                DsError.ThrowExceptionForHR(hr);

                //http://msdn.microsoft.com/en-us/library/windows/desktop/ms701834(v=vs.85).aspx
                //MFVideoRenderPrefs videoRenderPrefs;
                //hr = evrVideoDisplayControl.GetRenderingPrefs(out videoRenderPrefs);
                ////videoRenderPrefs = MFVideoRenderPrefs.DoNotRenderBorder | MFVideoRenderPrefs.DoNotRepaintOnStop;
                ////videoRenderPrefs = MFVideoRenderPrefs.AllowBatching;
                //videoRenderPrefs = MFVideoRenderPrefs.ForceBatching;
                ////videoRenderPrefs = MFVideoRenderPrefs.ForceBatching | MFVideoRenderPrefs.AllowBatching;
                //hr = evrVideoDisplayControl.SetRenderingPrefs(videoRenderPrefs);

                //MFVideoAspectRatioMode pdwAspectRatioMode;
                //hr = evrVideoDisplayControl.GetAspectRatioMode(out pdwAspectRatioMode);
                //pdwAspectRatioMode = MFVideoAspectRatioMode.None;
                //hr = evrVideoDisplayControl.SetAspectRatioMode(pdwAspectRatioMode);

                //int color = 0;
                //hr = evrVideoDisplayControl.GetBorderColor(out color); // VMR9AspectRatioMode.None);
                //hr = evrVideoDisplayControl.SetBorderColor(0xff0FF0); // VMR9AspectRatioMode.None);
                //ThrowExceptionForHR("Setting the EVR AspectRatioMode: ", hr);


                //EVR Clipping Window bug!!!!!!!!!!!!!!!!!!!http://social.msdn.microsoft.com/Forums/en/windowsdirectshowdevelopment/thread/579b6a6b-bdba-4d3b-a0b6-4de72114232b
            }
            finally
            {
                //Marshal.ReleaseComObject(pDisplay);
            }


        }
        private void ConfigureVMR9InWindowlessMode()
        {
            Log("Configuring VMR9 Filter");

            int hr = 0;
            IPin inputPin = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
            AMMediaType mediaType = new AMMediaType();

            //hr = inputPin.ConnectionMediaType(mediaType);
            //DsError.ThrowExceptionForHR(hr);
            IVMRFilterConfig9 filterConfig = this.videoRenderer as IVMRFilterConfig9;

            // Configure VMR-9 in Windowsless mode
            hr = filterConfig.SetRenderingMode(VMR9Mode.Windowless);
            DsError.ThrowExceptionForHR(hr);

            // With 1 input stream
            hr = filterConfig.SetNumberOfStreams(1);
            DsError.ThrowExceptionForHR(hr);

            IVMRWindowlessControl9 windowlessControl = this.videoRenderer as IVMRWindowlessControl9;

            // The main form is hosting the VMR-9
            hr = windowlessControl.SetVideoClippingWindow(this.hostingControl.GetBaseHandle());
            DsError.ThrowExceptionForHR(hr);

            // Keep the aspect-ratio OK
            hr = windowlessControl.SetAspectRatioMode(VMR9AspectRatioMode.LetterBox);
            DsError.ThrowExceptionForHR(hr);

            // Init the VMR-9 with default size values
            ResizeMoveHandler(null, null);

            // Add Windows Messages handlers
            AddHandlers();

            Log("VMR9 Filter Configured");
        }
        private void AddHandlers()
        {
            Log("Adding handlers");
            // Add Windows Messages handlers
            this.hostingControl.Paint += new PaintEventHandler(PaintHandler); // for WM_PAINT
            this.hostingControl.Resize += new EventHandler(ResizeMoveHandler); // for WM_SIZE
            this.hostingControl.Move += new EventHandler(ResizeMoveHandler); // for WM_MOVE
            SystemEvents.DisplaySettingsChanged += new EventHandler(DisplayChangedHandler); // for WM_DISPLAYCHANGE
            Log("Handlers added");
        }
        private void RemoveHandlers()
        {
            Log("Removing handlers");
            // Remove Windows Messages handlers
            this.hostingControl.Paint -= new PaintEventHandler(PaintHandler); // for WM_PAINT
            this.hostingControl.Resize -= new EventHandler(ResizeMoveHandler); // for WM_SIZE
            this.hostingControl.Move -= new EventHandler(ResizeMoveHandler); // for WM_MOVE
            SystemEvents.DisplaySettingsChanged -= new EventHandler(DisplayChangedHandler); // for WM_DISPLAYCHANGE
            Log("Handlers removed");
        }
        // This method changed to work with Windows 7
        // Under this OS, the MPEG-2 Demux now have dozens of outputs pins.
        // Rendering all of them is not a good solution.
        // The rendering process must be more smart...
        private void ConnectFilters()
        {
            Log("Connecting all the filters in the graph");

            int hr = 0;
            int pinNumber = 0;
            IPin pinOut, pinIn;

            // After the rendering process, our 4 downstream filters must be rendered
            bool bdaTIFRendered = false;
            bool bdaSecTabRendered = false;
            bool audioRendered = false;
            bool videoRendered = false;

            // for each output pins...
            while (true)
            {

                pinOut = DsFindPin.ByDirection(mpeg2Demux, PinDirection.Output, pinNumber);
                // Is the last pin reached ?
                if (pinOut == null)
                    break;

                Log(string.Format("Pin {0}", pinNumber));

                IEnumMediaTypes enumMediaTypes = null;
                AMMediaType[] mediaTypes = new AMMediaType[1];

                try
                {
                    // Get Pin's MediaType enumerator
                    hr = pinOut.EnumMediaTypes(out enumMediaTypes);
                    DsError.ThrowExceptionForHR(hr);

                    // for each media types...
                    while (enumMediaTypes.Next(mediaTypes.Length, mediaTypes, IntPtr.Zero) == 0)
                    {
                        // Store the majortype and the subtype and free the structure
                        Guid majorType = mediaTypes[0].majorType;
                        Guid subType = mediaTypes[0].subType;
                        DsUtils.FreeAMMediaType(mediaTypes[0]);

                        int breakpoint = 0;
                        if (subType == MediaSubType.H264) breakpoint++;
                        if (subType == MediaSubType.Mpeg2Video) breakpoint++;

                        if (majorType == MediaType.Audio)
                        {
                            Log(string.Format("Media type: {0}", MediaType.Audio.ToString()));
                            // Is the Audio already rendered ?
                            if (!audioRendered)
                            {
                                // Get the first input pin
                                pinIn = DsFindPin.ByDirection(audioRenderer, PinDirection.Input, 0);

                                // Render it with IntelliConnect (a decoder should be added between the two filters.
                                hr = graphBuilder.Connect(pinOut, pinIn);
                                DsError.ThrowExceptionForHR(hr);

                                // Release the Pin
                                Marshal.ReleaseComObject(pinIn);
                                pinIn = null;

                                // Notify that the audio renderer is connected
                                audioRendered = true;
                            }
                        }
                        else if (majorType == MediaType.Video)
                        {
                            Log(string.Format("Media type: {0}", MediaType.Video.ToString()));
                            // Is the Video already rendered ?
                            if (!videoRendered)
                            {
                                SetPinFormat(pinOut);

                                // Get the first input pin
                                pinIn = DsFindPin.ByDirection(videoRenderer, PinDirection.Input, 0);

                                if (H264Decoder == null)
                                {
                                    // Render it with IntelliConnect (a decoder should be added between the two filters.
                                    hr = graphBuilder.Connect(pinOut, pinIn);
                                    DsError.ThrowExceptionForHR(hr);
                                }
                                else
                                {
                                    IBaseFilter h264dec;

                                    graphBuilder.AddSourceFilterForMoniker(H264Decoder.Mon, null, H264Decoder.Name, out h264dec);

                                    IPin pinInH264 = DsFindPin.ByDirection(h264dec, PinDirection.Input, 0);
                                    
                                    if(pinInH264 != null)
                                    {
                                        hr = graphBuilder.Connect(pinOut, pinInH264);
                                        DsError.ThrowExceptionForHR(hr);
                                    }

                                    IPin pinRendererIn = DsFindPin.ByDirection(videoRenderer, PinDirection.Input, 0);
                                    IPin pinOutH264 = DsFindPin.ByDirection(h264dec, PinDirection.Output, 0);

                                    if (pinRendererIn != null && pinOutH264 != null)
                                    {
                                        hr = graphBuilder.Connect(pinOutH264, pinRendererIn);
                                        DsError.ThrowExceptionForHR(hr);
                                    }
                                }

                                // Release the Pin
                                Marshal.ReleaseComObject(pinIn);
                                pinIn = null;

                                // Notify that the video renderer is connected
                                videoRendered = true;
                            }
                        }
                        else if (majorType == MediaType.Mpeg2Sections)
                        {
                            Log(string.Format("Media type: {0}", MediaType.Mpeg2Sections.ToString()));

                            if (subType == MediaSubType.Mpeg2Data)
                            {
                                Log(string.Format("Media subtype: {0}", MediaSubType.Mpeg2Data.ToString()));
                                // Is the MPEG-2 Sections and Tables Filter already rendered ?
                                if (!bdaSecTabRendered)
                                {
                                    // Get the first input pin
                                    pinIn = DsFindPin.ByDirection(bdaSecTab, PinDirection.Input, 0);

                                    // A direct connection is enough
                                    hr = graphBuilder.ConnectDirect(pinOut, pinIn, null);
                                    DsError.ThrowExceptionForHR(hr);

                                    // Release the Pin
                                    Marshal.ReleaseComObject(pinIn);
                                    pinIn = null;

                                    // Notify that the MPEG-2 Sections and Tables Filter is connected
                                    bdaSecTabRendered = true;
                                }
                            }
                            // This sample only support DVB-T or DVB-S so only supporting this subtype is enough.
                            // If you want to support ATSC or ISDB, don't forget to handle these network types.
                            else if (subType == MediaSubType.DvbSI)
                            {
                                Log(string.Format("Media subtype: {0}", MediaSubType.DvbSI.ToString()));
                                // Is the BDA MPEG-2 Transport Information Filter already rendered ?
                                if (!bdaTIFRendered)
                                {
                                    // Get the first input pin
                                    pinIn = DsFindPin.ByDirection(bdaTIF, PinDirection.Input, 0);

                                    // A direct connection is enough
                                    hr = graphBuilder.ConnectDirect(pinOut, pinIn, null);
                                    DsError.ThrowExceptionForHR(hr);

                                    // Release the Pin
                                    Marshal.ReleaseComObject(pinIn);
                                    pinIn = null;

                                    // Notify that the BDA MPEG-2 Transport Information Filter is connected
                                    bdaTIFRendered = true;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    // Free COM objects
                    Marshal.ReleaseComObject(enumMediaTypes);
                    enumMediaTypes = null;

                    Marshal.ReleaseComObject(pinOut);
                    pinOut = null;
                }

                // Next pin, please !
                pinNumber++;
            }
        }
        private void ConnectFiltersForChannels()
        {
            Log("Connecting all the filters in the channel graph");

            int hr = 0;
            int pinNumber = 0;
            IPin pinOut, pinIn;

            // After the rendering process, our 4 downstream filters must be rendered
            bool bdaTIFRendered = false;
            bool bdaSecTabRendered = false;

            // for each output pins...

            //Usar este código para encontrar o nome do pin de TIF
            while (true)
            {

                pinOut = DsFindPin.ByDirection(mpeg2Demux, PinDirection.Output, pinNumber);
                // Is the last pin reached ?
                if (pinOut == null)
                    break;

                Log(string.Format("Pin {0}", pinNumber));

                IEnumMediaTypes enumMediaTypes = null;
                AMMediaType[] mediaTypes = new AMMediaType[1];

                try
                {
                    // Get Pin's MediaType enumerator
                    hr = pinOut.EnumMediaTypes(out enumMediaTypes);
                    DsError.ThrowExceptionForHR(hr);

                    // for each media types...
                    while (enumMediaTypes.Next(mediaTypes.Length, mediaTypes, IntPtr.Zero) == 0)
                    {
                        // Store the majortype and the subtype and free the structure
                        Guid majorType = mediaTypes[0].majorType;
                        Guid subType = mediaTypes[0].subType;
                        DsUtils.FreeAMMediaType(mediaTypes[0]);
                        
                        if (majorType == MediaType.Mpeg2Sections)
                        {
                            Log(string.Format("Media type: {0}", MediaType.Mpeg2Sections.ToString()));

                            if (subType == MediaSubType.Mpeg2Data)
                            {
                                Log(string.Format("Media subtype: {0}", MediaSubType.Mpeg2Data.ToString()));
                                // Is the MPEG-2 Sections and Tables Filter already rendered ?
                                if (!bdaSecTabRendered)
                                {
                                    // Get the first input pin
                                    pinIn = DsFindPin.ByDirection(bdaSecTab, PinDirection.Input, 0);

                                    // A direct connection is enough
                                    hr = graphBuilder.ConnectDirect(pinOut, pinIn, null);
                                    DsError.ThrowExceptionForHR(hr);

                                    // Release the Pin
                                    Marshal.ReleaseComObject(pinIn);
                                    pinIn = null;

                                    // Notify that the MPEG-2 Sections and Tables Filter is connected
                                    bdaSecTabRendered = true;
                                }
                            }
                            // This sample only support DVB-T or DVB-S so only supporting this subtype is enough.
                            // If you want to support ATSC or ISDB, don't forget to handle these network types.
                            else if (subType == MediaSubType.DvbSI)
                            {
                                Log(string.Format("Media subtype: {0}", MediaSubType.DvbSI.ToString()));
                                // Is the BDA MPEG-2 Transport Information Filter already rendered ?
                                if (!bdaTIFRendered)
                                {
                                    // Get the first input pin
                                    pinIn = DsFindPin.ByDirection(bdaTIF, PinDirection.Input, 0);

                                    // A direct connection is enough
                                    hr = graphBuilder.ConnectDirect(pinOut, pinIn, null);
                                    DsError.ThrowExceptionForHR(hr);

                                    // Release the Pin
                                    Marshal.ReleaseComObject(pinIn);
                                    pinIn = null;

                                    // Notify that the BDA MPEG-2 Transport Information Filter is connected
                                    bdaTIFRendered = true;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    // Free COM objects
                    Marshal.ReleaseComObject(enumMediaTypes);
                    enumMediaTypes = null;

                    Marshal.ReleaseComObject(pinOut);
                    pinOut = null;
                }

                // Next pin, please !
                pinNumber++;
            }
        }

        public void SelectDeinterlaceMode()
        {
            int hr = 0;

            string verticalStretch = "";
            string medianFiltering = "";
            string pixelAdaptive = "";
            string edgeFiltering = "";
            string lineReplicate = "";
            string fieldAdaptive = "";
            string motionVector = "";

            if (VideoRendererDevice == VideoRenderer.VMR9)
            {
                IVMRDeinterlaceControl9 deinterlace = (IVMRDeinterlaceControl9)this.videoRenderer;
                //IPin inputPin = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
                IPin inputPin = null;
                hr = videoRenderer.FindPin("VMR Input0", out inputPin);
                AMMediaType mediaType = new AMMediaType();

                hr = inputPin.ConnectionMediaType(mediaType);

                if (mediaType.formatType == FormatType.VideoInfo2)
                {
                    int numModes = 0;

                    VideoInfoHeader2 header2 = new VideoInfoHeader2();
                    Marshal.PtrToStructure(mediaType.formatPtr, header2);

                    VMR9VideoDesc videoDescriber = new VMR9VideoDesc();
                    if ((header2.InterlaceFlags & AMInterlace.IsInterlaced) != 0)
                    {
                        videoDescriber.dwSize = Marshal.SizeOf(videoDescriber);
                        videoDescriber.dwSampleHeight = header2.BmiHeader.Height;
                        videoDescriber.dwSampleWidth = header2.BmiHeader.Width;

                        if ((header2.InterlaceFlags & AMInterlace.IsInterlaced) != 0)
                        {
                            Log("Video is interlaced");
                            Log(header2.InterlaceFlags.ToString());

                            if ((header2.InterlaceFlags & AMInterlace.DisplayModeBobOnly) == 0)
                            {
                                videoDescriber.SampleFormat = VMR9SampleFormat.ProgressiveFrame;
                            }
                            if ((header2.InterlaceFlags & AMInterlace.OneFieldPerSample) != 0)
                            {
                                if ((header2.InterlaceFlags & AMInterlace.Field1First) != 0)
                                {
                                    videoDescriber.SampleFormat = VMR9SampleFormat.FieldSingleEven;
                                }
                                else
                                {
                                    videoDescriber.SampleFormat = VMR9SampleFormat.FieldSingleOdd;
                                }
                            }
                            if ((header2.InterlaceFlags & AMInterlace.Field1First) != 0)
                            {
                                videoDescriber.SampleFormat = VMR9SampleFormat.FieldInterleavedEvenFirst;
                            }
                            else
                            {
                                videoDescriber.SampleFormat = VMR9SampleFormat.FieldInterleavedOddFirst;
                            }
                        }

                        Log("Sample format: " + VMR9SampleFormatToStringManual(videoDescriber.SampleFormat));

                        videoDescriber.InputSampleFreq.dwDenominator = 10000000;
                        videoDescriber.InputSampleFreq.dwNumerator = (int)header2.AvgTimePerFrame;

                        videoDescriber.OutputFrameFreq.dwDenominator = 10000000;
                        videoDescriber.OutputFrameFreq.dwNumerator = (int)header2.AvgTimePerFrame * 2;

                        hr = deinterlace.GetNumberOfDeinterlaceModes(ref videoDescriber, ref numModes, null);
                        Log(hr.ToString());

                        if (hr == 0 && numModes != 0)
                        {
                            Guid[] modes = new Guid[numModes];
                            {
                                hr = deinterlace.GetNumberOfDeinterlaceModes(ref videoDescriber, ref numModes, modes);
                                for (int i = 0; i < numModes; i++)
                                {
                                    VMR9DeinterlaceCaps caps = new VMR9DeinterlaceCaps();

                                    caps.dwSize = Marshal.SizeOf(typeof(VMR9DeinterlaceCaps));

                                    hr = deinterlace.GetDeinterlaceModeCaps(modes[i], ref videoDescriber, ref caps);
                                    if (hr == 0)
                                    {
                                        Log("Available Deinterlace mode - " + i + ": " + modes[i]);

                                        switch (caps.DeinterlaceTechnology)
                                        {
                                            case VMR9DeinterlaceTech.Unknown:
                                                Log("VMR9: Unknown HW deinterlace mode");
                                                break;
                                            case VMR9DeinterlaceTech.PixelAdaptive:
                                                Log("VMR9: Pixel Adaptive capable");
                                                pixelAdaptive = modes[i].ToString();
                                                break;
                                            case VMR9DeinterlaceTech.MedianFiltering:
                                                Log("VMR9: Median Filtering capable");
                                                medianFiltering = modes[i].ToString();
                                                break;
                                            case VMR9DeinterlaceTech.BOBVerticalStretch:
                                                Log("VMR9: BOB Vertical Stretch capable");
                                                verticalStretch = modes[i].ToString();
                                                break;
                                            case VMR9DeinterlaceTech.BOBLineReplicate:
                                                Log("VMR9: BOB Line Replicate capable");
                                                lineReplicate = modes[i].ToString();
                                                break;
                                            case VMR9DeinterlaceTech.EdgeFiltering:
                                                Log("VMR9: Edge Filtering capable");
                                                edgeFiltering = modes[i].ToString();
                                                break;
                                            case VMR9DeinterlaceTech.FieldAdaptive:
                                                Log("VMR9: Field Adaptive capable");
                                                fieldAdaptive = modes[i].ToString();
                                                break;
                                            case VMR9DeinterlaceTech.MotionVectorSteered:
                                                Log("VMR9: Motion Vector Steered capable");
                                                motionVector = modes[i].ToString();
                                                break;

                                        }
                                    }
                                }
                            }

                            if (pixelAdaptive != "")
                            {
                                Guid deinterlaceMode = new Guid(pixelAdaptive);

                                hr = deinterlace.SetDeinterlaceMode(0, deinterlaceMode);
                                Marshal.ThrowExceptionForHR(hr);
                                Log("VMR9: Using Pixel Adaptive deinterlace");
                            }
                            else if (medianFiltering != "")
                            {
                                Guid deinterlaceMode = new Guid(medianFiltering);

                                hr = deinterlace.SetDeinterlaceMode(0, deinterlaceMode);
                                Marshal.ThrowExceptionForHR(hr);
                                Log("VMR9: Using Median Filtering deinterlace");
                            }
                            else if (edgeFiltering != "")
                            {
                                Guid deinterlaceMode = new Guid(edgeFiltering);

                                hr = deinterlace.SetDeinterlaceMode(0, deinterlaceMode);
                                Marshal.ThrowExceptionForHR(hr);
                                Log("VMR9: Using Edge Filtering deinterlace");
                            }
                            else if (verticalStretch != "")
                            {
                                Guid deinterlaceMode = new Guid(verticalStretch);

                                hr = deinterlace.SetDeinterlaceMode(0, deinterlaceMode);
                                Marshal.ThrowExceptionForHR(hr);
                                Log("VMR9: Using Vertical Stretch deinterlace");
                            }
                            else if (lineReplicate != "")
                            {
                                Guid deinterlaceMode = new Guid(lineReplicate);

                                hr = deinterlace.SetDeinterlaceMode(0, deinterlaceMode);
                                Marshal.ThrowExceptionForHR(hr);
                                Log("VMR9: Using Line Replicate deinterlace");
                            }
                            else if (fieldAdaptive != "")
                            {
                                Guid deinterlaceMode = new Guid(fieldAdaptive);

                                hr = deinterlace.SetDeinterlaceMode(0, deinterlaceMode);
                                Marshal.ThrowExceptionForHR(hr);
                                Log("VMR9: Using Field Adaptive deinterlace");
                            }
                            else if (motionVector != "")
                            {
                                Guid deinterlaceMode = new Guid(motionVector);

                                hr = deinterlace.SetDeinterlaceMode(0, deinterlaceMode);
                                Marshal.ThrowExceptionForHR(hr);
                                Log("VMR9: Using Motion Vector deinterlace");
                            }
                        }
                    }
                }

                hr = Marshal.ReleaseComObject(inputPin);
                Marshal.ThrowExceptionForHR(hr);
                inputPin = null;

            }
        }

        private void Decompose()
        {
            int hr = 0;

            // Decompose the graph
            hr = (this.graphBuilder as IMediaControl).StopWhenReady();
            hr = (this.graphBuilder as IMediaControl).Stop();

            this.State = BDAState.Stopped;

            RemoveHandlers();

            FilterGraphTools.RemoveAllFilters(this.graphBuilder);

            if(this.networkProvider != null) Marshal.ReleaseComObject(this.networkProvider); this.networkProvider = null;
            if(this.mpeg2Demux != null) Marshal.ReleaseComObject(this.mpeg2Demux); this.mpeg2Demux = null;
            if(this.tuner != null) Marshal.ReleaseComObject(this.tuner); this.tuner = null;
            if(this.capture != null) Marshal.ReleaseComObject(this.capture); this.capture = null;
            if(this.bdaTIF != null) Marshal.ReleaseComObject(this.bdaTIF); this.bdaTIF = null;
            if(this.bdaSecTab != null) Marshal.ReleaseComObject(this.bdaSecTab); this.bdaSecTab = null;
            if(this.audioRenderer != null) Marshal.ReleaseComObject(this.audioRenderer); this.audioRenderer = null;
            if(this.videoRenderer != null) Marshal.ReleaseComObject(this.videoRenderer); this.videoRenderer = null;

            if(rot != null) rot.Dispose();
            Marshal.ReleaseComObject(this.graphBuilder); this.graphBuilder = null;
        }

        private void Log(string message)
        {
            if (BDALogging != null) BDALogging(message);
        }

        private void PaintHandler(object sender, PaintEventArgs e)
        {
            if (this.videoRenderer != null && this.State == BDAState.Running)
            {
                IntPtr hdc = e.Graphics.GetHdc();
                int hr = (this.videoRenderer as IVMRWindowlessControl9).RepaintVideo(this.hostingControl.GetBaseHandle(), hdc);
                e.Graphics.ReleaseHdc(hdc);
            }
        }

        #region From CodeTV

        private Rectangle currentVideoTargetRectangle;
        private Size currentVideoSourceSize;
        public bool Use169Ratio { get; set; }

        private void VideoResizer(VideoSizeMode videoZoomMode, bool keepAspectRatio, PointF offset, double zoom, double aspectRatioFactor)
        {
            int hr = 0;

            Rectangle windowRect = this.hostingControl.GetBaseRectangle();
            currentVideoTargetRectangle = windowRect;
            currentVideoSourceSize = new Size();

            //if (this.State == BDAState.Running)
            {
                if (videoZoomMode != VideoSizeMode.StretchToWindow)
                {
                    int arX, arY;
                    int arX2 = 0, arY2 = 0;

                    if (VideoRendererDevice == VideoRenderer.EVR)
                    {
                        Size videoSize = new Size(), arVideoSize = new Size();
                        hr = evrVideoDisplayControl.GetNativeVideoSize(out videoSize, out arVideoSize);
                        //IMFVideoDisplayControlEx evrVideoDisplayControlPlus = evrVideoDisplayControl as IMFVideoDisplayControlEx;
                        //hr = evrVideoDisplayControlPlus.GetNativeVideoSize(out videoSize, out arVideoSize);
                        //hr = evrVideoDisplayControlPlus.GetIdealVideoSize(videoSize, arVideoSize);
                        arX = videoSize.Width;
                        arY = videoSize.Height;
                        arX2 = arVideoSize.Width;
                        arY2 = arVideoSize.Height;
                    }
                    else
                        hr = (this.videoRenderer as IVMRWindowlessControl9).GetNativeVideoSize(out arX, out arY, out arX2, out arY2);
                    if (hr >= 0 && arY > 0)
                    {
                        //DsError.ThrowExceptionForHR(hr);
                        //Trace.WriteLineIf(trace.TraceVerbose, string.Format("\tGetNativeVideoSize(width: {0}, height: {1}, arX {2}, arY: {3}", arX, arY, arX2, arY2));

                        if (arX2 > 0 && arY2 > 0)
                        {
                            arX = arX2;
                            arY = arY2;
                        }

                        currentVideoSourceSize.Width = arX;
                        currentVideoSourceSize.Height = arY;

                        Size windowSize = windowRect.Size;

                        double newAspectRation = aspectRatioFactor * (double)arX / (double)arY * (this.Use169Ratio ? 3.0 / 4.0 : 1.0);
                        int height = windowSize.Height;
                        int width = (int)((double)height * newAspectRation);

                        if (videoZoomMode == VideoSizeMode.FromInside || videoZoomMode == VideoSizeMode.FromOutside)
                        {
                            if (videoZoomMode == VideoSizeMode.FromInside && width > windowSize.Width
                            || videoZoomMode == VideoSizeMode.FromOutside && width < windowSize.Width)
                            {
                                width = windowSize.Width;
                                height = (int)((double)width / newAspectRation);
                            }
                        }

                        Size size = new Size((int)(zoom * width), (int)(zoom * height));

                        Point pos = new Point(
                            (int)(offset.X * (windowRect.Width * 3 - size.Width) - windowRect.Width),
                            (int)(offset.Y * (windowRect.Height * 3 - size.Height) - windowRect.Height));

                        //Point pos = new Point(
                        //    (int)(offset.X * (windowRect.Width - size.Width)),
                        //    (int)(offset.Y * (windowRect.Height - size.Height)));

                        currentVideoTargetRectangle = new Rectangle(pos, size);
                    }
                }
                if (VideoRendererDevice == VideoRenderer.EVR)
                {
                    //hr = evrVideoDisplayControl.SetVideoWindow(this.hostingControl.GetBaseHandle());
                    MFVideoNormalizedRect pnrcSource = new MFVideoNormalizedRect(0.0f, 0.0f, 1.0f, 1.0f);
                    hr = this.evrVideoDisplayControl.SetVideoPosition(pnrcSource, (MediaFoundation.Misc.MFRect)currentVideoTargetRectangle);
                    this.hostingControl.ModifyBlackBands(GetBlackBands(), Color.Black);
                }
                else
                    hr = (this.videoRenderer as IVMRWindowlessControl9).SetVideoPosition(null, DsRect.FromRectangle(currentVideoTargetRectangle));
            }
        }

        protected Rectangle[] GetBlackBands()
        {
            Rectangle outerRectangle = this.hostingControl.ClientRectangle;
            DsRect innerDsRect = new DsRect();
            int hr;
            if (VideoRendererDevice == VideoRenderer.EVR)
            {
                MFVideoNormalizedRect pnrcSource = new MFVideoNormalizedRect();
                MediaFoundation.Misc.MFRect prcDest = new MediaFoundation.Misc.MFRect();
                hr = evrVideoDisplayControl.GetVideoPosition(pnrcSource, prcDest);
                innerDsRect = DsRect.FromRectangle((Rectangle)prcDest);
            }
            else
            {
                IVMRWindowlessControl9 vmrWindowlessControl9 = this.videoRenderer as IVMRWindowlessControl9;
                hr = vmrWindowlessControl9.GetVideoPosition(null, innerDsRect);
            }
            Rectangle innerRectangle = innerDsRect.ToRectangle();

            //Trace.WriteLineIf(trace.TraceVerbose, string.Format(("\tvideoRenderer.GetVideoPosition({0})"), innerRectangle.ToString()));
            //Trace.WriteLineIf(trace.TraceVerbose, string.Format(("\thostingControl.ClientRectangle({0})"), outerRectangle.ToString()));

            List<Rectangle> alRectangles = new List<Rectangle>();

            if (innerRectangle.Top > outerRectangle.Top)
                alRectangles.Add(new Rectangle(outerRectangle.Left, outerRectangle.Top, outerRectangle.Width - 1, innerRectangle.Top - 1));

            if (innerRectangle.Bottom < outerRectangle.Bottom)
                alRectangles.Add(new Rectangle(outerRectangle.Left, innerRectangle.Bottom, outerRectangle.Width - 1, outerRectangle.Height - (innerRectangle.Bottom + 1)));

            if (innerRectangle.Left > outerRectangle.Left)
            {
                Rectangle rectangleLeft = new Rectangle(outerRectangle.Left, innerRectangle.Top, innerRectangle.Left - 1, innerRectangle.Height - 1);
                rectangleLeft.Intersect(outerRectangle);
                alRectangles.Add(rectangleLeft);
            }

            if (innerRectangle.Right < outerRectangle.Right)
            {
                Rectangle rectangleLeft = new Rectangle(innerRectangle.Right, innerRectangle.Top, outerRectangle.Width - (innerRectangle.Right + 1), innerRectangle.Height - 1);
                rectangleLeft.Intersect(outerRectangle);
                alRectangles.Add(rectangleLeft);
            }
            return alRectangles.ToArray();
        }

        /// <summary>
        /// Pinta as barras pretas
        /// </summary>
        /// <param name="g"></param>
        private void PaintBands(Graphics g)
        {
            PaintBands(g, Color.Black);
        }
        /// <summary>
        /// Pinta barras às cores. Pode ser útil para pintar barras transparentes
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bandColor"></param>
        private void PaintBands(Graphics g, Color bandColor)
        {
            if (this.videoRenderer != null)
            {

                Rectangle[] alRectangles = GetBlackBands();
                if (alRectangles.Length > 0)
                {
                    g.FillRectangles(new SolidBrush(bandColor), alRectangles);
                    g.DrawRectangles(new System.Drawing.Pen(bandColor), alRectangles);
                }
            }
        }

        #endregion

        private void ResizeMoveHandler(object sender, EventArgs e)
        {
            if (this.videoRenderer != null)
            {
                //Trace.WriteLineIf(trace.TraceInfo, "OnResizeMoveHandler()");

                VideoResizer(hostingControl.VideoZoomMode, hostingControl.VideoKeepAspectRatio, hostingControl.VideoOffset, hostingControl.VideoZoomValue, hostingControl.VideoAspectRatio);

                if (VideoRendererDevice == VideoRenderer.EVR)
                {
                    Graphics g = this.hostingControl.CreateGraphics();
                    PaintBands(g);
                    g.Dispose();
                }
            }
        }

        private void DisplayChangedHandler(object sender, EventArgs e)
        {
            if (this.videoRenderer != null)
            {
                int hr = (this.videoRenderer as IVMRWindowlessControl9).DisplayModeChanged();
            }
        }
        #endregion

        #region Manual2

        private void BuildGraphManual(ITuningSpace tuningSpace)
        {
            try
            {

                int hr;
                Log("Building graph in manual mode");

                this.graphBuilder = (IFilterGraph2)new FilterGraph();
                rot = new DsROTEntry(this.graphBuilder);

                // Method names should be self explanatory
                Log("AddNetworkProviderFilter(tuningSpace);" + Environment.NewLine);
                AddNetworkProviderFilter(tuningSpace);
                Log("AddMPEG2DemuxFilterManual();" + Environment.NewLine);
                AddMPEG2DemuxFilterManual();

                Log("CreateMPEG2DemuxPinsManual();" + Environment.NewLine);
                CreateMPEG2DemuxPinsManual();

                //AddAndConnectTransportStreamFiltersToGraph();
                Log("AddAudioDecoderFiltersManual(this.graphBuilder);" + Environment.NewLine);
                AddAudioDecoderFiltersManual(this.graphBuilder);
                Log("AddBDAVideoDecoderFiltersManual(this.graphBuilder);" + Environment.NewLine);
                AddBDAVideoDecoderFiltersManual(this.graphBuilder);

                Log("AddAndConnectBDABoardFilters();" + Environment.NewLine);
                AddAndConnectBDABoardFilters();

                //if (this.H264Decoder != null || !isH264ElecardSpecialMode)
                //{
                //+++ This order is to avoid a bug from the DirectShow 
                Log("AddAndConnectSectionsAndTablesFilterManual();" + Environment.NewLine);
                AddAndConnectSectionsAndTablesFilterManual();

                IMpeg2Data mpeg2Data = this.bdaSecTab as IMpeg2Data;
                ISectionList ppSectionList;
                int hr2 = mpeg2Data.GetTable(0, 0, null, 0, out ppSectionList);
                Log(hr2 + " = mpeg2Data.GetTable(0, 0, null, 0, out ppSectionList);" + Environment.NewLine);

                Log("AddAndConnectTIFToGraphManual();" + Environment.NewLine);
                AddAndConnectTIFToGraphManual();
                //---


                Log("AddRenderers();" + Environment.NewLine);
                AddRenderers();

                Log("ConfigureVMR9InWindowlessMode();" + Environment.NewLine);
                ConfigureVMR9InWindowlessMode();

                Log("ConnectAudioAndVideoFiltersManual();" + Environment.NewLine);
                ConnectAudioAndVideoFiltersManual();

                Log("Vmr9SetDeinterlaceModeManual(1);" + Environment.NewLine);
                Vmr9SetDeinterlaceModeManual(1);
            }
            catch (Exception ex)
            {
                Decompose();
                throw ex;
            }
        }

        private void AddMPEG2DemuxFilterManual()
        {
            this.mpeg2Demux = (IBaseFilter)new MPEG2Demultiplexer();

            int hr = this.graphBuilder.AddFilter(this.mpeg2Demux, "MPEG2 Demultiplexer Manual");
            DsError.ThrowExceptionForHR(hr);
        }
        private void CreateMPEG2DemuxPinsManual()
        {
            IMpeg2Demultiplexer mpeg2Demultiplexer = this.mpeg2Demux as IMpeg2Demultiplexer;

            {
                //Pin 1 connected to the "BDA MPEG2 Transport Information Filter"
                //    Major Type	MEDIATYPE_MPEG2_SECTIONS {455F176C-4B06-47CE-9AEF-8CAEF73DF7B5}
                //    Sub Type		MEDIASUBTYPE_DVB_SI {E9DD31A3-221D-4ADB-8532-9AF309C1A408}
                //    Format		None

                //    MPEG2 PSI Sections
                //    Pids: 0x00 0x10 0x11 0x12 0x13 0x14 0x6e 0xd2 0x0136 0x019a 0x01fe 0x0262 0x03f2

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
                int hr = mpeg2Demultiplexer.CreateOutputPin(mediaTIF, "TIF", out pinDemuxerTIF);
                if (pinDemuxerTIF != null)
                    Marshal.ReleaseComObject(pinDemuxerTIF);
            }

            if (this.H264Decoder != null)
            {
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
                int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
                if (pinDemuxerVideoH264 != null)
                    Marshal.ReleaseComObject(pinDemuxerVideoH264);

                Marshal.FreeHGlobal(mediaH264.formatPtr);
            }
            else
            {
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
                int hr = mpeg2Demultiplexer.CreateOutputPin(mediaMPG2, "MPG2", out pinDemuxerVideoMPEG2);
                if (pinDemuxerVideoMPEG2 != null)
                    Marshal.ReleaseComObject(pinDemuxerVideoMPEG2);

                Marshal.FreeHGlobal(mediaMPG2.formatPtr);
            }

            {
                AMMediaType mediaAudio = new AMMediaType();
                mediaAudio.majorType = MediaType.Audio;
                //if (this.AudioDecoderType == ChannelDVB.AudioType.AC3)
                //    mediaAudio.subType = MediaSubType.DolbyAC3;
                //else if (this.AudioDecoderType == ChannelDVB.AudioType.EAC3)
                //{
                //    //EAC3
                //    //http://social.msdn.microsoft.com/Forums/en-US/windowsdirectshowdevelopment/thread/64f5b2ef-9ec6-408c-9a86-6e1355bea717/
                //    Guid MEDIASUBTYPE_DDPLUS_AUDIO = new Guid("a7fb87af-2d02-42fb-a4d4-05cd93843bdd");
                //    mediaAudio.subType = MEDIASUBTYPE_DDPLUS_AUDIO;
                //    //mediaAudio.subType = MediaSubType.DolbyAC3;
                //    //Guid GuidDolbyEAC3 = new Guid("{33434145-0000-0010-8000-00AA00389B71}");
                //    //#define WAVE_FORMAT_DOLBY_EAC3 0x33434145
                //    //DEFINE_GUID(MEDIASUBTYPE_DOLBY_EAC3, WAVE_FORMAT_DOLBY_EAC3, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
                //    //mediaAudio.subType = MediaSubType.Mpeg2Audio;
                //}
                //else
                mediaAudio.subType = MediaSubType.Mpeg2Audio;
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
                int hr = mpeg2Demultiplexer.CreateOutputPin(mediaAudio, "Audio", out pinDemuxerAudio);
                if (pinDemuxerAudio != null)
                    Marshal.ReleaseComObject(pinDemuxerAudio);

                Marshal.FreeHGlobal(mediaAudio.formatPtr);
            }

            {
                //Pin 5 connected to "MPEG-2 Sections and Tables" (Allows to grab custom PSI tables)
                //    Major Type	MEDIATYPE_MPEG2_SECTIONS {455F176C-4B06-47CE-9AEF-8CAEF73DF7B5}
                //    Sub Type		MEDIASUBTYPE_MPEG2DATA {C892E55B-252D-42B5-A316-D997E7A5D995}
                //    Format		None

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
                int hr = mpeg2Demultiplexer.CreateOutputPin(mediaSectionsAndTables, "PSI", out pinDemuxerSectionsAndTables);
                if (pinDemuxerSectionsAndTables != null)
                    Marshal.ReleaseComObject(pinDemuxerSectionsAndTables);
            }
        }
        private void AddAudioDecoderFiltersManual(IFilterGraph2 graphBuilder)
        {
            int hr = 0;
            if (this.AudioDecoder != null)
                hr = graphBuilder.AddSourceFilterForMoniker(this.AudioDecoder.Mon, null, this.AudioDecoder.Name, out this.audioDecoderFilter);
            DsError.ThrowExceptionForHR(hr);
        }
        private void AddBDAVideoDecoderFiltersManual(IFilterGraph2 graphBuilder)
        {
            int hr = 0;
            if (this.MPEG2Decoder != null)
                hr = graphBuilder.AddSourceFilterForMoniker(this.MPEG2Decoder.Mon, null, this.MPEG2Decoder.Name, out this.mpeg2DecoderFilter);
            DsError.ThrowExceptionForHR(hr);
        }
        private void AddAndConnectSectionsAndTablesFilterManual()
        {
            int hr = 0;
            IPin pinOut;
            DsDevice[] devices;

            // Add two filters needed in a BDA graph
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
                            hr = this.graphBuilder.Connect(pinOut, pinIn);
                            Marshal.ReleaseComObject(pinIn);
                        }

                        //DsError.ThrowExceptionForHR(hr);
                        // In fact the last pin don't render since i havn't added the BDA MPE Filter...
                        Marshal.ReleaseComObject(pinOut);
                    }

                    continue;
                }
            }
        }
        private void AddAndConnectTIFToGraphManual()
        {
            int hr = 0;
            IPin pinOut;
            DsDevice[] devices;

            // Add two filters needed in a BDA graph
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
                        IPin pinIn = DsFindPin.ByDirection(this.bdaTIF, PinDirection.Input, 0);
                        if (pinIn != null)
                        {
                            hr = this.graphBuilder.Connect(pinOut, pinIn);
                            Marshal.ReleaseComObject(pinIn);
                        }

                        // In fact the last pin don't render since i havn't added the BDA MPE Filter...
                        Marshal.ReleaseComObject(pinOut);
                    }

                    continue;
                }
            }
        }
        private void ConnectAudioAndVideoFiltersManual()
        {
            int hr = 0;
            IPin pinOut;

            //hr = this.mpeg2Demux.FindPin("H264", out pinOut);
            //if (pinOut != null)
            //{
            //    try
            //    {
            //        if (this.videoH264DecoderFilter == null)
            //        {
            //            IPin pinInFromFilterOut = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
            //            if (pinInFromFilterOut != null)
            //            {
            //                try
            //                {
            //                    hr = this.graphBuilder.Connect(pinOut, pinInFromFilterOut);
            //                }
            //                finally
            //                {
            //                    Marshal.ReleaseComObject(pinInFromFilterOut);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            IPin videoDecoderIn = null;
            //            try
            //            {
            //                videoDecoderIn = DsFindPin.ByDirection(this.videoH264DecoderFilter, PinDirection.Input, 0);


            //                //AMMediaType mediaH264 = new AMMediaType();
            //                //mediaH264.majorType = MediaType.Video;
            //                ////mediaH264.subType = new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b);
            //                //mediaH264.subType = MediaSubType.H264; // new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            //                //mediaH264.sampleSize = 0;
            //                //mediaH264.temporalCompression = true; // false;
            //                //mediaH264.fixedSizeSamples = true; // false;
            //                //mediaH264.unkPtr = IntPtr.Zero;
            //                //mediaH264.formatType = FormatType.Mpeg2Video;

            //                //MPEG2VideoInfo videoH264PinFormat = GetVideoH264PinFormat();
            //                //mediaH264.formatSize = Marshal.SizeOf(videoH264PinFormat);
            //                //mediaH264.formatPtr = Marshal.AllocHGlobal(mediaH264.formatSize);
            //                //Marshal.StructureToPtr(videoH264PinFormat, mediaH264.formatPtr, false);

            //                ////IPin pinDemuxerVideoH264;
            //                ////int hr = mpeg2Demultiplexer.CreateOutputPin(mediaH264, "H264", out pinDemuxerVideoH264);
            //                ////if (pinDemuxerVideoH264 != null)
            //                ////Marshal.ReleaseComObject(pinDemuxerVideoH264);
            //                //hr = this.graphBuilder.ConnectDirect(pinOut, videoDecoderIn, mediaH264);
            //                ////hr = this.graphBuilder2.Connect(videoDvrOut, videoDecoderIn);
            //                ////DsError.ThrowExceptionForHR(hr);

            //                //Marshal.FreeHGlobal(mediaH264.formatPtr);

            //                //if (hr != 0)
            //                FilterGraphTools.ConnectFilters(this.graphBuilder, pinOut, videoDecoderIn, false);
            //            }
            //            finally
            //            {
            //                if (videoDecoderIn != null) Marshal.ReleaseComObject(videoDecoderIn);
            //            }

            //            IPin videoDecoderOut = null, videoVMRIn = null;
            //            try
            //            {
            //                videoDecoderOut = DsFindPin.ByDirection(this.videoH264DecoderFilter, PinDirection.Output, 0);
            //                videoVMRIn = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
            //                FilterGraphTools.ConnectFilters(this.graphBuilder, videoDecoderOut, videoVMRIn, false);
            //            }
            //            finally
            //            {
            //                if (videoDecoderOut != null) Marshal.ReleaseComObject(videoDecoderOut);
            //                if (videoVMRIn != null) Marshal.ReleaseComObject(videoVMRIn);
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        Marshal.ReleaseComObject(pinOut);
            //    }
            //}

            hr = this.mpeg2Demux.FindPin("MPG2", out pinOut);
            if (pinOut != null)
            {
                try
                {
                    if (this.mpeg2DecoderFilter == null)
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
                            videoDecoderIn = DsFindPin.ByDirection(this.mpeg2DecoderFilter, PinDirection.Input, 0);

                            FilterGraphTools.ConnectFilters(this.graphBuilder, pinOut, videoDecoderIn, false);
                        }
                        finally
                        {
                            if (videoDecoderIn != null) Marshal.ReleaseComObject(videoDecoderIn);
                        }

                        IPin videoDecoderOut = null, videoVMRIn = null;
                        try
                        {
                            videoDecoderOut = DsFindPin.ByDirection(this.mpeg2DecoderFilter, PinDirection.Output, 0);
                            videoVMRIn = DsFindPin.ByDirection(this.videoRenderer, PinDirection.Input, 0);
                            //FilterGraphTools.ConnectFilters(this.graphBuilder, videoDecoderOut, videoVMRIn, false);
                            hr = graphBuilder.ConnectDirect(videoDecoderOut, videoVMRIn, null);
                            DsError.ThrowExceptionForHR(hr);
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

            hr = this.mpeg2Demux.FindPin("Audio", out pinOut);
            if (pinOut != null)
            {
                hr = this.graphBuilder.Render(pinOut);
                //DsError.ThrowExceptionForHR(hr);
                Marshal.ReleaseComObject(pinOut);
            }
        }

        #endregion

        private string VMR9SampleFormatToStringManual(VMR9SampleFormat format)
        {
            switch (format)
            {
                case VMR9SampleFormat.FieldInterleavedEvenFirst: return "FieldInterleavedEvenFirst";
                case VMR9SampleFormat.FieldInterleavedOddFirst: return "FieldInterleavedOddFirst";
                case VMR9SampleFormat.FieldSingleEven: return "FieldSingleEven";
                case VMR9SampleFormat.FieldSingleOdd: return "FieldSingleOdd";
                case VMR9SampleFormat.None: return "None";
                case VMR9SampleFormat.ProgressiveFrame: return "ProgressiveFrame";
                case VMR9SampleFormat.Reserved: return "Reserved";
                default: return "Unknown";
            }
        }

        protected void Vmr9SetDeinterlaceModeManual(int mode)
        {
            //0=None
            //1=Bob
            //2=Weave
            //3=Best
            //Log("vmr9:SetDeinterlace() SetDeinterlaceMode(%d)",mode);
            IVMRDeinterlaceControl9 pDeint = (this.videoRenderer as IVMRDeinterlaceControl9);
            if (pDeint != null)
            {
                //VMR9VideoDesc VideoDesc;
                //uint dwNumModes = 0;
                Guid deintMode;
                int hr;
                if (mode == 0)
                {
                    //off
                    hr = pDeint.SetDeinterlaceMode(-1, Guid.Empty);
                    hr = pDeint.GetDeinterlaceMode(0, out deintMode);

                    return;
                }
                if (mode == 1)
                {
                    //BOB

                    hr = pDeint.SetDeinterlaceMode(-1, bobDxvaGuid);
                    hr = pDeint.GetDeinterlaceMode(0, out deintMode);

                    return;
                }
                if (mode == 2)
                {
                    //WEAVE
                    hr = pDeint.SetDeinterlaceMode(-1, Guid.Empty);
                    hr = pDeint.GetDeinterlaceMode(0, out deintMode);
                    return;
                }
            }
        }

    }
}
