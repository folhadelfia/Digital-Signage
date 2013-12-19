using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using DirectShowLib;
using DirectShowLib.BDA;
using System.Collections;
using TV2Lib.PSI;
using System.Xml.Serialization;
using System.Threading;

//Olá
//Mundo
//Again

namespace TV2Lib
{
    /// <summary>
    /// Controlo usado para apresentar televisão digital terrestre.
    /// </summary>
    public partial class DigitalTVScreen : VideoControl, IDisposable
    {
        private GraphBuilderBDA graphicBuilder;
        internal GraphBuilderBDA GraphicBuilder
        {
            get
            {
                if (graphicBuilder == null)
                    InitializeGraphBuilder();
                return graphicBuilder;
            }
            set { graphicBuilder = value; }
        }

        public Settings Settings
        {
            get
            {
                return this.GraphicBuilder.Settings;
            }
            set
            {
                this.GraphicBuilder.Settings = value;
            }
        }

        #region Devices

        public class DeviceStuff
        {
            private DigitalTVScreen owner;

            internal DeviceStuff(DigitalTVScreen _owner)
            {
                this.owner = _owner;
            }

            public DsDevice H264Decoder
            {
                get { try { return owner.GraphicBuilder.H264DecoderDevice; } catch (NullReferenceException) { return null; } }
                set
                {
                    owner.GraphicBuilder.H264DecoderDevice = value != null && GraphBuilderBDA.H264DecoderDevices.Keys.Contains(value.DevicePath) ? value : null;
                    if (value != null)
                    {
                        if (owner.Channels.CurrentChannel != null) owner.Channels.CurrentChannel.H264DecoderDevice = value.DevicePath;
                        else return;
                    }
                    else
                    {
                        if (owner.Channels.CurrentChannel != null) owner.Channels.CurrentChannel.H264DecoderDevice = null;
                        else return;
                    }
                }
            }
            public DsDevice MPEG2Decoder
            {
                get { try { return owner.GraphicBuilder.Mpeg2DecoderDevice; } catch (NullReferenceException) { return null; } }
                set
                {
                    owner.GraphicBuilder.Mpeg2DecoderDevice = value != null && GraphBuilderBDA.MPEG2DecoderDevices.Keys.Contains(value.DevicePath) ? value : null;
                    if (value != null)
                    {
                        if (owner.Channels.CurrentChannel != null) owner.Channels.CurrentChannel.MPEG2DecoderDevice = value.DevicePath;
                        else return;
                    }
                    else
                    {
                        if (owner.Channels.CurrentChannel != null) owner.Channels.CurrentChannel.MPEG2DecoderDevice = null;
                        else return;
                    }
                }
            }
            public DsDevice AudioDecoder
            {
                get { try { return owner.GraphicBuilder.AudioDecoderDevice; } catch (NullReferenceException) { return null; } }
                set
                {
                    owner.GraphicBuilder.AudioDecoderDevice = value != null && GraphBuilderBDA.AudioDecoderDevices.Keys.Contains(value.DevicePath) ? value : null;

                    if (value != null)
                    {
                        if (owner.Channels.CurrentChannel != null) owner.Channels.CurrentChannel.AudioDecoderDevice = value.DevicePath;
                        else return;
                    }
                    else
                    {
                        if (owner.Channels.CurrentChannel != null) owner.Channels.CurrentChannel.AudioDecoderDevice = null;
                        else return;
                    }
                }
            }

            public DsDevice TunerDevice
            {
                get
                {
                    try { return owner.GraphicBuilder.TunerDevice; }
                    catch { return null; }
                }
                set
                {
                    owner.GraphicBuilder.TunerDevice = (value != null && TunerDevices.Keys.Contains(value.DevicePath)) ? value : null;

                    //if (value != null)
                    //{
                    //    if (owner.Channels.CurrentChannel != null) owner.Channels.CurrentChannel.TunerDevice = value.DevicePath;
                    //    else return;
                    //}
                    //else
                    //{
                    //    if (owner.Channels.CurrentChannel != null) owner.Channels.CurrentChannel.TunerDevice = null;
                    //    else return;
                    //}
                }
            }
            public DsDevice CaptureDevice
            {
                get
                {
                    try { return owner.GraphicBuilder.CaptureDevice; }
                    catch { return null; }
                }
                set
                {
                    owner.GraphicBuilder.TunerDevice = (value != null && TunerDevices.Keys.Contains(value.DevicePath)) ? value : null;

                    if (value != null)
                    {
                        if (owner.Channels.CurrentChannel != null) owner.Channels.CurrentChannel.CaptureDevice = value.DevicePath;
                        else return;
                    }
                    else
                    {
                        if (owner.Channels.CurrentChannel != null) owner.Channels.CurrentChannel.CaptureDevice = null;
                        else return;
                    }
                }
            }

            public DsDevice AudioRenderer
            {
                get { try { return owner.GraphicBuilder.AudioRendererDevice; } catch (NullReferenceException) { return null; } }
                set
                {
                    owner.GraphicBuilder.AudioRendererDevice = value != null && GraphBuilderBDA.AudioRendererDevices.Keys.Contains(value.DevicePath) ? value : null;
                    if (value != null)
                    {
                        if (owner.Channels.CurrentChannel != null) owner.Channels.CurrentChannel.AudioRendererDevice = value.DevicePath;
                        else return;
                    }
                    else
                    {
                        if (owner.Channels.CurrentChannel != null) owner.Channels.CurrentChannel.AudioRendererDevice = null;
                        else return;
                    }
                }
            }

            public static Dictionary<string, DsDevice> AudioDecoderDevices
            {
                get
                {
                    return GraphBuilderBDA.AudioDecoderDevices;
                }
            }
            public static Dictionary<string, DsDevice> H264DecoderDevices
            {
                get { return GraphBuilderBDA.H264DecoderDevices; }
            }
            public static Dictionary<string, DsDevice> MPEG2DecoderDevices
            {
                get { return GraphBuilderBDA.MPEG2DecoderDevices; }
            }
            public static Dictionary<string, DsDevice> AudioRendererDevices
            {
                get { return GraphBuilderBDA.AudioRendererDevices; }
            }
            public static Dictionary<string, DsDevice> TunerDevices
            {
                get { return GraphBuilderBDA.TunerDevices; }
            }
            public static Dictionary<string, DsDevice> CaptureDevices
            {
                get { return GraphBuilderBDA.CaptureDevices; }
            }
            public static Dictionary<string, DsDevice> TunerDevicesInUse
            {
                get { return GraphBuilderBDA.TunerDevicesInUse; }
            }
        }
        private DeviceStuff devices;
        public DeviceStuff Devices
        {
            get { return devices ?? (devices = new DeviceStuff(this)); }
            protected set { devices = value; }
        }
        #endregion

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

        #region Canais

        public class ChannelStuff
        {
            private DigitalTVScreen owner;
            internal ChannelStuff(DigitalTVScreen _owner)
            {
                this.owner = _owner;
                this.owner.Devices = _owner.Devices;
            }

            public ChannelDVBT CurrentChannel
            {
                get { return (owner.GraphicBuilder.CurrentChannel as ChannelDVBT); }
            }
            private List<ChannelDVBT> channelList;
            public List<ChannelDVBT> ChannelList
            {
                get { return channelList ?? (channelList = new List<ChannelDVBT>()); }
            }

            public int Frequency
            {
                get { return this.CurrentChannel.Frequency; }
                set { this.CurrentChannel.Frequency = value; }
            }
            public int Bandwidth
            {
                get { return this.CurrentChannel.Bandwidth; }
                set { this.CurrentChannel.Bandwidth = value; }
            }
            public int ONID
            {
                get { return this.CurrentChannel.ONID; }
                set { this.CurrentChannel.ONID = value; }
            }
            public int TSID
            {
                get { return this.CurrentChannel.TSID; }
                set { this.CurrentChannel.TSID = value; }
            }
            public int SID
            {
                get { return this.CurrentChannel.SID; }
                set { this.CurrentChannel.SID = value; }
            }

            public bool ForceRebuildOnChannelTune { get; set; }

            public static int MIN_FREQUENCY { get { return 470000; } }
            public static int MAX_FREQUENCY { get { return 855000; } }
            public static int DEFAULT_STEP { get { return 1000; } }
            public static int DEFAULT_FREQUENCY { get { return 754000; } }


            public int Count { get { return this.ChannelList.Count; } }

            private bool ChannelsAreCompatible(ChannelDVBT channel1, ChannelDVBT channel2)
            {
                return
                    channel1.AudioDecoderType == channel2.AudioDecoderType &&
                    channel1.VideoDecoderType == channel2.VideoDecoderType &&
                    channel1.Frequency == channel2.Frequency &&
                    channel1.Bandwidth == channel2.Bandwidth &&
                    channel1.AudioDecoderDevice == channel2.AudioDecoderDevice &&
                    channel1.AudioRendererDevice == channel2.AudioRendererDevice &&
                    channel1.CaptureDevice == channel2.CaptureDevice &&
                    channel1.H264DecoderDevice == channel2.H264DecoderDevice &&
                    channel1.MPEG2DecoderDevice == channel2.MPEG2DecoderDevice &&
                    channel1.TunerDevice == channel2.TunerDevice &&
                    channel1.VideoRendererDevice == channel2.VideoRendererDevice;
            }

            //Ver como é que o programa muda os valores, e ver os que devem fazer com quem o graph seja reiniciado

            public delegate void TuneChannelCallbackDelegate(DigitalTVScreen screen, Channel ch, bool result);

            public void TuneChannelAsync(Channel channel, TuneChannelCallbackDelegate callback)
            {
                Thread t = new Thread(new ThreadStart((MethodInvoker)(() =>
                {
                    bool res = this.TuneChannel(channel);

                    if (callback != null) callback(owner, channel, res);


                })));

                t.Start();
            }

            public bool TuneChannel(Channel channel)
            {
                //if (owner.InvokeRequired)
                //{
                //    owner.Invoke((MethodInvoker)(() => { owner.Channels.TuneChannel(channel); }));
                //    return owner.GraphicBuilder != null;
                //}
                //else
                {
                    

                    bool needRebuild = false;
                    ChannelDVBT channelDVBT = channel as ChannelDVBT;

                    if (channelDVBT == null) return false;

                    //needRebuild = (owner.GraphicBuilder.CurrentChannel as ChannelDVBT).AudioDecoderType != channelDVBT.AudioDecoderType ||
                    //              (owner.GraphicBuilder.CurrentChannel as ChannelDVBT).VideoDecoderType != channelDVBT.VideoDecoderType;

                    //Como a imagem fica com fps baixo, obriguei o graph a construir-se de novo sempre que há um tune novo
                    //needRebuild = true;
                    needRebuild = this.ForceRebuildOnChannelTune ||
                                  owner.GraphicBuilder == null ||
                                  owner.GraphicBuilder.Demultiplexer == null ||
                                  (owner.Devices.TunerDevice != null && owner.Devices.TunerDevice.DevicePath != CurrentChannel.TunerDevice) ||
                                  (owner.Devices.CaptureDevice != null && owner.Devices.CaptureDevice.DevicePath != CurrentChannel.CaptureDevice) ||
                                  (owner.Devices.AudioDecoder != null && owner.Devices.AudioDecoder.DevicePath != CurrentChannel.AudioDecoderDevice) ||
                                  (owner.Devices.AudioRenderer != null && owner.Devices.AudioRenderer.DevicePath != CurrentChannel.AudioRendererDevice) ||
                                  (owner.Devices.H264Decoder != null && owner.Devices.H264Decoder.DevicePath != CurrentChannel.H264DecoderDevice) ||
                                  (owner.Devices.MPEG2Decoder != null && owner.Devices.MPEG2Decoder.DevicePath != CurrentChannel.MPEG2DecoderDevice) ||
                                  channelDVBT.NeedToRebuildTheGraph(CurrentChannel);

                    bool res = (owner.GraphicBuilder = this.TuneChannel(channel, needRebuild)) != null;

                    return res;
                }
            }
            private GraphBuilderBDA TuneChannel(Channel channel, bool needRebuild)
            {
                GraphBuilderBase currentGraph = owner.GraphicBuilder;
                VideoControl hostingControl = owner as VideoControl;

                Settings oldSettings;

                if (currentGraph == null) return null;
                else oldSettings = currentGraph.Settings;

                GraphBuilderBDA temp = currentGraph as GraphBuilderBDA;

                string oldGraphTunerDevice = temp.TunerDevice == null ? "" : temp.TunerDevice.DevicePath,
                       oldGraphCaptureDevice = temp.CaptureDevice == null ? "" : temp.CaptureDevice.DevicePath,
                       oldGraphAudioDecoderDevice = temp.AudioDecoderDevice == null ? "" : temp.AudioDecoderDevice.DevicePath,
                       oldGraphAudioRendererDevice = temp.AudioRendererDevice == null ? "" : temp.AudioRendererDevice.DevicePath,
                       oldGraphH264DecoderDevice = temp.H264DecoderDevice == null ? "" : temp.H264DecoderDevice.DevicePath,
                       oldGraphMpeg2DecoderDevice = temp.Mpeg2DecoderDevice == null ? "" : temp.Mpeg2DecoderDevice.DevicePath;

                string ownerTunerDevice = owner.Devices.TunerDevice == null ? "" : owner.Devices.TunerDevice.DevicePath,
                       ownerCaptureDevice = owner.Devices.CaptureDevice == null ? "" : owner.Devices.CaptureDevice.DevicePath,
                       ownerAudioDecoderDevice = owner.Devices.AudioDecoder == null ? "" : owner.Devices.AudioDecoder.DevicePath,
                       ownerAudioRendererDevice = owner.Devices.AudioRenderer == null ? "" : owner.Devices.AudioRenderer.DevicePath,
                       ownerH264DecoderDevice = owner.Devices.H264Decoder == null ? "" : owner.Devices.H264Decoder.DevicePath,
                       ownerMpeg2DecoderDevice = owner.Devices.MPEG2Decoder == null ? "" : owner.Devices.MPEG2Decoder.DevicePath;

                if (needRebuild && currentGraph != null)
                {
                    owner.ClearGraph();
                    
                    //OnGraphStop();
                    //currentGraph.Dispose();
                    //currentGraph = null;
                }

                GraphBuilderBDA currentGraphTV = currentGraph as GraphBuilderBDA;


                if (channel is ChannelDVB)
                {
                    ChannelDVB channelDVB = channel as ChannelDVB;


                    #region Rebuild do graph



                    if (needRebuild)
                    {
                        GraphBuilderBDA newGraph = null;

                        DVBTTuning tuner = new DVBTTuning();
                        tuner.TuneSelect(channelDVB.Frequency, channelDVB.ONID, channelDVB.TSID, channelDVB.SID);

                        newGraph = new GraphBuilderBDA(hostingControl);

                        //newGraph.GraphStarted += new EventHandler(newGraph_GraphStarted);
                        //newGraph.GraphEnded += new EventHandler(newGraph_GraphEnded);
                        //newGraph.PossibleChanged += new EventHandler<GraphBuilderBase.PossibleEventArgs>(newGraph_PossibleChanged);
                        newGraph.Settings = oldSettings;

                        newGraph.ReferenceClock = channelDVB.ReferenceClock;

                        newGraph.AudioDecoderType = channelDVB.AudioDecoderType;
                        DsDevice device;

                        #region Audio decoder

                        //if (!string.IsNullOrEmpty(channelDVB.AudioDecoderDevice))
                        //{
                        if (!string.IsNullOrWhiteSpace(ownerAudioDecoderDevice) && GraphBuilderBDA.AudioDecoderDevices.TryGetValue(ownerAudioDecoderDevice, out device)) //Tenta o do objecto
                            newGraph.AudioDecoderDevice = device;
                        else if (!string.IsNullOrWhiteSpace(channelDVB.AudioDecoderDevice) && GraphBuilderBDA.AudioDecoderDevices.TryGetValue(channelDVB.AudioDecoderDevice, out device)) //Tenta o do channel
                            newGraph.AudioDecoderDevice = device;
                        else if (!string.IsNullOrWhiteSpace(oldGraphAudioDecoderDevice) && GraphBuilderBDA.AudioDecoderDevices.TryGetValue(oldGraphAudioDecoderDevice, out device)) //Tenta o do graph antigo
                            newGraph.AudioDecoderDevice = device;
                        //else //Lança excepção (Usar try catch a volta deste metodo NAO ESQUECER)
                        //    throw new Exception(channelDVB.AudioDecoderDevice + " not found");
                        //}

                        #endregion

                        #region Video decoders

                        if (channelDVB.VideoDecoderType == ChannelDVB.VideoType.MPEG2)
                        {
                            //if (!string.IsNullOrEmpty(channelDVB.MPEG2DecoderDevice))
                            //{
                            if (!string.IsNullOrWhiteSpace(ownerMpeg2DecoderDevice) && GraphBuilderBDA.MPEG2DecoderDevices.TryGetValue(ownerMpeg2DecoderDevice, out device)) //Tenta o do objecto
                                newGraph.Mpeg2DecoderDevice = device;
                            else if (!string.IsNullOrWhiteSpace(channelDVB.MPEG2DecoderDevice) && GraphBuilderBDA.MPEG2DecoderDevices.TryGetValue(channelDVB.MPEG2DecoderDevice, out device))
                                newGraph.Mpeg2DecoderDevice = device;
                            else if (!string.IsNullOrWhiteSpace(oldGraphMpeg2DecoderDevice) && GraphBuilderBDA.MPEG2DecoderDevices.TryGetValue(oldGraphMpeg2DecoderDevice, out device))
                                newGraph.Mpeg2DecoderDevice = device;
                            //else
                            //    throw new Exception(channelDVB.MPEG2DecoderDevice + " not found");
                            //}
                        }
                        else
                        {
                            //if (!string.IsNullOrEmpty(channelDVB.H264DecoderDevice))
                            //{
                            if (!string.IsNullOrWhiteSpace(ownerH264DecoderDevice) && GraphBuilderBDA.H264DecoderDevices.TryGetValue(ownerH264DecoderDevice, out device))
                                newGraph.H264DecoderDevice = device;
                            else if (!string.IsNullOrWhiteSpace(channelDVB.H264DecoderDevice) && GraphBuilderBDA.H264DecoderDevices.TryGetValue(channelDVB.H264DecoderDevice, out device))
                                newGraph.H264DecoderDevice = device;
                            else if (!string.IsNullOrWhiteSpace(oldGraphH264DecoderDevice) && GraphBuilderBDA.H264DecoderDevices.TryGetValue(oldGraphH264DecoderDevice, out device))
                                newGraph.H264DecoderDevice = device;
                            //else 
                            //    throw new Exception(channelDVB.H264DecoderDevice + " not found");
                            //}
                        }

                        #endregion

                        #region Audio renderer

                        //if (!string.IsNullOrEmpty(channelDVB.AudioRendererDevice))
                        //{
                        if (!string.IsNullOrWhiteSpace(ownerAudioRendererDevice) && GraphBuilderBDA.AudioRendererDevices.TryGetValue(ownerAudioRendererDevice, out device))
                            newGraph.AudioRendererDevice = device;
                        else if (!string.IsNullOrWhiteSpace(channelDVB.AudioRendererDevice) && GraphBuilderBDA.AudioRendererDevices.TryGetValue(channelDVB.AudioRendererDevice, out device))
                            newGraph.AudioRendererDevice = device;
                        else if (!string.IsNullOrWhiteSpace(oldGraphAudioRendererDevice) && GraphBuilderBDA.AudioRendererDevices.TryGetValue(oldGraphAudioRendererDevice, out device))
                            newGraph.AudioRendererDevice = device;
                        //else 
                        //    throw new Exception(channelDVB.AudioRendererDevice + " not found");
                        //}

                        #endregion

                        #region Tuner device

                        //if (!string.IsNullOrEmpty(channelDVB.TunerDevice))
                        //{
                        if (!string.IsNullOrWhiteSpace(ownerTunerDevice) && GraphBuilderBDA.TunerDevices.TryGetValue(ownerTunerDevice, out device))
                            newGraph.TunerDevice = device;
                        else if (!string.IsNullOrWhiteSpace(channelDVB.TunerDevice) && GraphBuilderBDA.TunerDevices.TryGetValue(channelDVB.TunerDevice, out device))
                            newGraph.TunerDevice = device;
                        else if (!string.IsNullOrWhiteSpace(oldGraphTunerDevice) && GraphBuilderBDA.TunerDevices.TryGetValue(oldGraphTunerDevice, out device))
                            newGraph.TunerDevice = device;
                        //else 
                        //    throw new Exception(channelDVB.TunerDevice + " not found");
                        //}

                        #endregion

                        #region Capture device


                        //if (!string.IsNullOrEmpty(channelDVB.CaptureDevice))
                        //{
                        if (!string.IsNullOrWhiteSpace(ownerCaptureDevice) && GraphBuilderBDA.CaptureDevices.TryGetValue(ownerCaptureDevice, out device))
                            newGraph.CaptureDevice = device;
                        else if (!string.IsNullOrWhiteSpace(channelDVB.CaptureDevice) && GraphBuilderBDA.CaptureDevices.TryGetValue(channelDVB.CaptureDevice, out device))
                            newGraph.CaptureDevice = device;
                        else if (!string.IsNullOrWhiteSpace(oldGraphCaptureDevice) && GraphBuilderBDA.CaptureDevices.TryGetValue(oldGraphCaptureDevice, out device))
                            newGraph.CaptureDevice = device;
                        //else 
                        //    throw new Exception(channelDVB.CaptureDevice + " not found");
                        //}

                        #endregion

                        IDVBTuningSpace tuningSpace = (IDVBTuningSpace)new DVBTuningSpace();

                        tuningSpace.put_UniqueName("DVBT TuningSpace");
                        tuningSpace.put_FriendlyName("DVBT TuningSpace");
                        tuningSpace.put_NetworkType(CLSID.DVBTNetworkProvider);
                        tuningSpace.put_SystemType(DVBSystemType.Terrestrial);

                        newGraph.TuningSpace = tuningSpace as ITuningSpace;
                        owner.SetGraphBuilderEvents();

                        newGraph.useEVR = true;

                        newGraph.BuildGraph(tuner, (channel as ChannelDVBT).VideoDecoderType, (channel as ChannelDVBT).AudioDecoderType);

                        currentGraphTV = newGraph;
                    }

                    #endregion

                    currentGraphTV.SubmitTuneRequest(channel);

                    currentGraphTV.VideoZoomMode = channelDVB.VideoZoomMode;
                    currentGraphTV.VideoKeepAspectRatio = channelDVB.VideoKeepAspectRatio;
                    currentGraphTV.VideoOffset = channelDVB.VideoOffset;
                    currentGraphTV.VideoZoom = channelDVB.VideoZoom;
                    currentGraphTV.VideoAspectRatioFactor = channelDVB.VideoAspectRatioFactor;

                    if (currentGraphTV.RunGraph() == 1)
                    {
                        if (!DigitalTVScreen.DeviceStuff.TunerDevicesInUse.ContainsKey(currentGraphTV.TunerDevice.DevicePath))
                        {
                            DigitalTVScreen.DeviceStuff.TunerDevicesInUse.Add(currentGraphTV.TunerDevice.DevicePath, currentGraphTV.TunerDevice);
                        }
                    }
                    //currentGraph.VideoResizer();
                    currentGraphTV.CurrentChannel = channel;
                }

                return currentGraphTV;
            }

            public bool RefreshChannels()
            {
                owner.OnNewLogMessage("Scanning channels");
                DigitalTVState oldState = owner.State;
                bool res = owner.ScanProbeFrequency(this.CurrentChannel);
                if (oldState == DigitalTVState.Stopped) owner.Stop();
                owner.OnNewLogMessage("Channel scan complete");

                return res;
            }

            public int ScanFrequencies()
            {
                return ScanFrequencies(ChannelStuff.MIN_FREQUENCY, ChannelStuff.MAX_FREQUENCY, ChannelStuff.DEFAULT_STEP);
            }
            public int ScanFrequencies(int minFreq, int maxFreq, int step)
            {
                owner.OnNewLogMessage(string.Format("Scanning from {0}khz to {1}khz with {2}khz increment.", minFreq, maxFreq, step));

                DigitalTVState oldState = owner.State;

                DigitalTVState old = owner.State;

                if (old == DigitalTVState.Stopped)
                {
                    DVBTTuning tuner = new DVBTTuning();
                    tuner.TuneSelect(this.Frequency, this.ONID, this.TSID, this.SID);

                    //owner.Start();
                    owner.GraphicBuilder.BuildGraphNoRenderers(tuner);
                    owner.GraphicBuilder.RunGraph();
                }

                for (int freq = minFreq; freq <= maxFreq; freq += step)
                {
                    owner.OnNewLogMessage(string.Format("Scanning for channels on {0}", freq));

                    if(owner.GraphicBuilder.ChangeFrequencyDuringScan(freq) == 1)
                        owner.ScanProbeFrequency(this.CurrentChannel);
                }

                owner.OnNewLogMessage(string.Format("Scan complete. {0} channel{1} found.", this.ChannelList.Count, this.ChannelList.Count == 1 ? "" : "s"));

                if (old == DigitalTVState.Stopped)
                    this.owner.Stop();

                return this.ChannelList.Count;
            }

            public void ScanFrequenciesAsync(ChannelListCallbackDelegate callback)
            {
                ScanFrequenciesAsync(ChannelStuff.MIN_FREQUENCY, ChannelStuff.MAX_FREQUENCY, ChannelStuff.DEFAULT_STEP, callback);
            }
            public void ScanFrequenciesAsync(int minFreq, int maxFreq, int step, ChannelListCallbackDelegate callback)
            {
                Thread t = new Thread(new ThreadStart((MethodInvoker)(() =>
                    {
                        ScanFrequencies(minFreq, maxFreq, step);
                        if (callback != null) callback(this, this.ChannelList);
                    })));

                t.Start();
            }

            public void SaveToXML()
            {
                try
                {
                    using (System.IO.FileStream writer = new System.IO.FileStream(Application.StartupPath + @"\channels.xml", System.IO.FileMode.Create))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<ChannelDVBT>));
                        serializer.Serialize(writer, this.ChannelList);
                        writer.Close();
                    }
                }
                catch (Exception ex)
                {

                    throw new ApplicationException("Error serializing the channel list", ex);
                }
            }

            public void LoadFromXML()
            {
                try
                {
                    using (System.IO.FileStream reader = new System.IO.FileStream(Application.StartupPath + @"\channels.xml", System.IO.FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<ChannelDVBT>));

                        List<ChannelDVBT> temp = serializer.Deserialize(reader) as List<ChannelDVBT>;

                        reader.Close();

                        this.ChannelList.Clear();

                        foreach (var ch in temp)
                        {
                            ch.AudioDecoderDevice = this.owner.Channels.CurrentChannel.AudioDecoderDevice;
                            ch.AudioRendererDevice = this.owner.Channels.CurrentChannel.AudioRendererDevice;
                            ch.CaptureDevice = this.owner.Channels.CurrentChannel.CaptureDevice;
                            ch.H264DecoderDevice = this.owner.Channels.CurrentChannel.H264DecoderDevice;
                            ch.MPEG2DecoderDevice = this.owner.Channels.CurrentChannel.MPEG2DecoderDevice;
                            ch.TunerDevice = this.owner.Channels.CurrentChannel.TunerDevice;

                            this.ChannelList.Add(ch);
                        }

                        if (this.ChannelList.Count > 0) owner.OnChannelListChanged();
                    }
                }
                catch (Exception ex)
                {

                    throw new ApplicationException("Error deserializing the channel list", ex);
                }
            }
        }
        private ChannelStuff channels;
        public ChannelStuff Channels
        {
            get { return channels ?? (channels = new ChannelStuff(this)); }
            protected set { channels = value; }
        }

        #endregion

        #region Eventos

        public event ChannelEventHandler ChannelListChanged;
        public event BDAGraphEventHandler NewLogMessage;
        public event EventHandler Started, Stopped;

        private void OnChannelListChanged()
        {
            if (ChannelListChanged != null) ChannelListChanged(this, new ChannelEventArgs(this.Channels.ChannelList));
        }
        private void OnNewLogMessage(string message)
        {
            string newMessage = string.Format("[{0}] {1}{2}", DateTime.Now.ToString("HH:mm:ss"), message, Environment.NewLine);

            if (NewLogMessage != null) NewLogMessage(newMessage);
        }
        private void OnStarted(object sender, EventArgs e)
        {
            if (Started != null) Started(this, new EventArgs());
        }
        private void OnStopped(object sender, EventArgs e)
        {
            if (Stopped != null) Stopped(this, new EventArgs());
        }

        private void SetGraphBuilderEvents()
        {
            SetGraphBuilderEvents(this.GraphicBuilder);
        }
        private void SetGraphBuilderEvents(GraphBuilderBDA builder)
        {
            (builder as GraphBuilderBase).NewLogMessage += OnNewLogMessage;
        }

        #endregion

        #region Logs

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

        #endregion

        #region Estados

        public enum DigitalTVState { Running, Paused, Stopped }
        public DigitalTVState State
        {
            get
            {
                try
                {
                    switch (GraphicBuilder.GetGraphState())
                    {
                        case FilterState.Stopped: return DigitalTVState.Stopped;
                        case FilterState.Running: return DigitalTVState.Running;
                        case FilterState.Paused: return DigitalTVState.Paused;
                        default: return DigitalTVState.Stopped;
                    }
                }
                catch
                {
                    return DigitalTVState.Stopped;
                }
            }
        }

        #endregion

        internal void InitializeGraphBuilder()
        {
            GraphicBuilder = new GraphBuilderBDA(this);

            SetGraphBuilderEvents();

            GraphicBuilder.Settings = new Settings()
            {
                UseWPF = false,
                TimeShiftingActivated = false,
                VideoBackgroundColor = Color.Black
            };

            this.Channels.CurrentChannel.Frequency = 754000;
            this.Channels.CurrentChannel.Bandwidth = 8;
        }
        public DigitalTVScreen()
        {
            InitializeComponent();
            InitializeGraphBuilder();
        }

        [Obsolete("Use Channels.TuneChannel(Channel ch) instead")]
        public int Start()
        {
            if (this.State != DigitalTVState.Running)
            {
                int hr = 0;
                try
                {
                    OnNewLogMessage("Starting TV component");

                    DVBTTuning tuner = new DVBTTuning();
                    tuner.TuneSelect(this.Channels.Frequency, this.Channels.ONID, this.Channels.TSID, this.Channels.SID);

                    #region Fazer scan dos canais

                    ////Encapsular num método
                    ////Mudar a forma como o graph é construído, tirar os renderers
                    ////Adicionar um estado ao graph e só construir o graph se for preciso.
                    ////Um graph a mostrar TV pode fazer scan sem problemas. Não desmontar o graph se já estiver a correr e o sinal estiver locked & present

                    //graphicBuilder.BuildGraphNoRenderers(tuner);
                    //this.Channels.GetChannels();
                    //graphicBuilder.StopGraph();
                    //graphicBuilder.DecomposeGraph();


                    #endregion

                    hr = GraphicBuilder.Start(tuner, this.Channels.CurrentChannel.VideoDecoderType, this.Channels.CurrentChannel.AudioDecoderType);
                    DsError.ThrowExceptionForHR(hr);

                    OnNewLogMessage("TV component started");
                    if (this.Channels.ChannelList.Count > 0) this.Channels.TuneChannel(this.Channels.ChannelList[0]);
                }
                catch (Exception ex)
                {
                    OnNewLogMessage("Failed to start TV component");
                    LogException(ex);
                }

                return hr;
            }
            else return 1;
        }
        public void Stop()
        {
            if (this.State != DigitalTVState.Stopped)
            {
                try
                {
                    GraphicBuilder.StopGraph();
                }
                catch (Exception ex)
                {
                    OnNewLogMessage(string.Format("EXCEPTION: {1}{0}Site:{2}", Environment.NewLine, ex.Message, "DigitalTVScreen.Stop()"));
                }
            }
        }

        void IDisposable.Dispose()
        {
            //graphicBuilder.Stop();
            GraphicBuilder.Dispose();
            base.Dispose();
        }

        public void SaveGraphToFile()
        {
            this.SaveGraphToFile(@"C:\TVgraph_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".grf");
        }
        public void SaveGraphToFile(string path)
        {
            GraphicBuilder.SaveGraph(path);
        }

        #region From CodeTV (already on VideoControl)

        //private bool useBlackBands = false;
        //private List<Control> blackBands = null;

        //public bool UseBlackBands
        //{
        //    get { return this.useBlackBands; }
        //    set
        //    {
        //        if (this.useBlackBands != value)
        //        {
        //            if (this.useBlackBands)
        //                RemoveSpecialBorder();
        //            else
        //                AddSpecialBorder();
        //            this.useBlackBands = value;
        //            this.Invalidate();
        //        }
        //    }
        //}

        //public void ModifyBlackBands(Rectangle[] borders, Color videoBackgroundColor)
        //{
        //    if (!this.UseBlackBands)
        //        this.UseBlackBands = true;

        //    if (borders.Length >= 2)
        //    {
        //        for (int i = 0; i < 2; i++)
        //        {
        //            Control control = this.blackBands[i];
        //            if (control != null)
        //            {
        //                if (!control.Visible) control.Visible = true;
        //                if (control.BackColor != videoBackgroundColor)
        //                    control.BackColor = videoBackgroundColor;
        //                if (control.Location.X != borders[i].X || control.Location.Y != borders[i].Y)
        //                    control.Location = new System.Drawing.Point(borders[i].X, borders[i].Y);
        //                if (control.Size.Width + 1 != borders[i].Width + 1 || control.Size.Height + 1 != borders[i].Height + 1)
        //                    control.Size = new System.Drawing.Size(borders[i].Width + 1, borders[i].Height + 1);
        //            }
        //        }
        //    }
        //}

        //private void AddSpecialBorder()
        //{
        //    if (this.blackBands == null)
        //    {
        //        this.blackBands = new List<Control>();

        //        for (int i = 0; i < 2; i++)
        //        {
        //            Control control = new Control();
        //            control.BackColor = System.Drawing.Color.Black;
        //            control.Location = new System.Drawing.Point(0, 0);
        //            control.Size = new System.Drawing.Size(1, 1);
        //            control.TabStop = false;
        //            control.Visible = false;
        //            this.Controls.Add(control);
        //            this.blackBands.Add(control);
        //        }
        //    }
        //}

        //private void RemoveSpecialBorder()
        //{
        //    if (this.blackBands != null)
        //    {
        //        for (int i = 0; i < 2; i++)
        //        {
        //            Control control = this.blackBands[0];
        //            this.blackBands.RemoveAt(0);
        //            this.Controls.Remove(control);
        //        }
        //        this.blackBands = null;
        //    }
        //}

        #endregion

        internal void ClearGraph()
        {
            if (this.GraphicBuilder != null)
            {
                //remover os eventos do graphicBuilder

                this.GraphicBuilder.Dispose();
                this.GraphicBuilder = null;
            }
        }

        internal static void UpdateDVBChannelPids(IMpeg2Data mpeg2Data, ushort pmtPid, ChannelDVB channelDVB)
        {
            Hashtable hashtableEcmPids = new Hashtable();

            channelDVB.PmtPid = pmtPid;

            PSISection[] psiPmts = PSISection.GetPSITable(pmtPid, (int)TABLE_IDS.PMT, mpeg2Data);
            for (int i2 = 0; i2 < psiPmts.Length; i2++)
            {
                PSISection psiPmt = psiPmts[i2];
                if (psiPmt != null && psiPmt is PSIPMT)
                {
                    PSIPMT pmt = (PSIPMT)psiPmt;

                    channelDVB.PcrPid = pmt.PcrPid;
                    
                    if (pmt.Descriptors != null)
                    {
                        foreach (PSIDescriptor descriptor in pmt.Descriptors)
                        {
                            switch (descriptor.DescriptorTag)
                            {
                                case DESCRIPTOR_TAGS.DESCR_CA:
                                    hashtableEcmPids[(descriptor as PSIDescriptorCA).CaPid] = (descriptor as PSIDescriptorCA).CaSystemId;
                                    break;
                            }
                        }
                    }

                    channelDVB.VideoPid = -1;
                    channelDVB.AudioPid = -1;
                    channelDVB.AudioPids = new int[0];
                    channelDVB.EcmPids = new int[0];

                    foreach (PSIPMT.Data data in pmt.Streams)
                    {
                        switch ((int)data.StreamType)
                        {
                            case (int)STREAM_TYPES.STREAMTYPE_11172_VIDEO:
                            case (int)STREAM_TYPES.STREAMTYPE_13818_VIDEO:
                                //channelDVB.VideoDecoderType = ChannelDVB.VideoType.MPEG2;
                                channelDVB.VideoPid = data.Pid;
                                if (data.Descriptors != null)
                                {
                                    foreach (PSIDescriptor descriptor in data.Descriptors)
                                        if (descriptor.DescriptorTag == DESCRIPTOR_TAGS.DESCR_CA)
                                            hashtableEcmPids[(descriptor as PSIDescriptorCA).CaPid] = (descriptor as PSIDescriptorCA).CaSystemId;
                                }
                                break;
                            case (int)STREAM_TYPES.STREAMTYPE_H264: //H264 - 27
                                channelDVB.VideoDecoderType = ChannelDVB.VideoType.H264;
                                channelDVB.VideoPid = data.Pid;
                                if (data.Descriptors != null)
                                {
                                    foreach (PSIDescriptor descriptor in data.Descriptors)
                                        if (descriptor.DescriptorTag == DESCRIPTOR_TAGS.DESCR_CA)
                                            hashtableEcmPids[(descriptor as PSIDescriptorCA).CaPid] = (descriptor as PSIDescriptorCA).CaSystemId;
                                }
                                break;
                            case (int)STREAM_TYPES.STREAMTYPE_11172_AUDIO:
                            case (int)STREAM_TYPES.STREAMTYPE_13818_AUDIO:
                                {
                                    int[] audioPids = new int[channelDVB.AudioPids.Length + 1];
                                    Array.Copy(channelDVB.AudioPids, audioPids, channelDVB.AudioPids.Length);
                                    audioPids[channelDVB.AudioPids.Length] = data.Pid;
                                    channelDVB.AudioPids = audioPids;
                                    if (audioPids.Length == 1) // If it is the first one
                                    {
                                        channelDVB.AudioPid = data.Pid;
                                        channelDVB.AudioDecoderType = ChannelDVB.AudioType.MPEG2;
                                    }

                                    foreach (PSIDescriptor descriptor in data.Descriptors)
                                        if (descriptor.DescriptorTag == DESCRIPTOR_TAGS.DESCR_CA)
                                            hashtableEcmPids[(descriptor as PSIDescriptorCA).CaPid] = (descriptor as PSIDescriptorCA).CaSystemId;
                                }
                                break;
                            case 17:
                                if (data.Descriptors != null)
                                {
                                    foreach (PSIDescriptor descriptor in data.Descriptors)
                                    {
                                        if (descriptor.DescriptorTag == DESCRIPTOR_TAGS.DESCR_ISO_639_LANGUAGE)
                                        {
                                            
                                            int[] audioPids = new int[channelDVB.AudioPids.Length + 1];
                                            Array.Copy(channelDVB.AudioPids, audioPids, channelDVB.AudioPids.Length);
                                            audioPids[channelDVB.AudioPids.Length] = data.Pid;

                                            channelDVB.AudioPids = audioPids;
                                            channelDVB.AudioPid = data.Pid;

                                            channelDVB.AudioDecoderType = ChannelDVB.AudioType.AAC;
                                        }
                                    }
                                }
                                break;
                            case (int)STREAM_TYPES.STREAMTYPE_13818_PES_PRIVATE:
                                if (data.Descriptors != null)
                                {
                                    foreach (PSIDescriptor descriptor in data.Descriptors)
                                    {
                                        if (descriptor.DescriptorTag == DESCRIPTOR_TAGS.DESCR_TELETEXT)
                                        {
                                            channelDVB.TeletextPid = data.Pid;
                                        }
                                        else if (descriptor.DescriptorTag == DESCRIPTOR_TAGS.DESCR_AC3 || descriptor.DescriptorTag == DESCRIPTOR_TAGS.DESCR_ENHANCED_AC3)
                                        {
                                            //Audio again
                                            int[] audioPids = new int[channelDVB.AudioPids.Length + 1];
                                            Array.Copy(channelDVB.AudioPids, audioPids, channelDVB.AudioPids.Length);
                                            audioPids[channelDVB.AudioPids.Length] = data.Pid;
                                            channelDVB.AudioPids = audioPids;
                                            if (audioPids.Length == 1) // If it is the first one
                                            {
                                                channelDVB.AudioPid = data.Pid;
                                                if (descriptor.DescriptorTag == DESCRIPTOR_TAGS.DESCR_AC3)
                                                    channelDVB.AudioDecoderType = ChannelDVB.AudioType.AC3;
                                                if (descriptor.DescriptorTag == DESCRIPTOR_TAGS.DESCR_ENHANCED_AC3)
                                                    channelDVB.AudioDecoderType = ChannelDVB.AudioType.EAC3;
                                            }
                                        }
                                        else if (descriptor.DescriptorTag == DESCRIPTOR_TAGS.DESCR_ISO_639_LANGUAGE)
                                        {

                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            channelDVB.EcmPids = new int[hashtableEcmPids.Count];
            hashtableEcmPids.Keys.CopyTo(channelDVB.EcmPids, 0);
            if (channelDVB.EcmPids.Length > 0)
                channelDVB.EcmPid = channelDVB.EcmPids[0];

            if (channelDVB.AudioPids.Count() > 0) channelDVB.AudioPid = channelDVB.AudioPids.Min();
        }

        public bool GetSignalStatistics(out bool locked, out bool present, out int strength, out int quality)
        {
            bool r;

            r = this.GraphicBuilder.GetSignalStatistics(out locked, out present, out strength, out quality);

            return r;
        }
        private bool ScanProbeFrequency(ChannelTV currentChannelTV)
        {
            bool locked = false, present = false;
            int strength, quality;

            (this as VideoControl).BackColor = Color.Transparent;
            try
            {
                this.Channels.TuneChannel(currentChannelTV);
            }
            catch (Exception)
            {
                (this as VideoControl).BackColor = this.Settings.VideoBackgroundColor;
            }

            if (currentChannelTV is ChannelDVB)
            {
                if (this.GraphicBuilder is IBDA)
                {
                    IBDA graphBuilderBDA = this.GraphicBuilder as IBDA;
                    System.Threading.Thread.Sleep(500);

                    int sucatadaScanTimeout = 0;
                    do
                    {
                        (graphBuilderBDA as ITV).GetSignalStatistics(out locked, out present, out strength, out quality);
                        sucatadaScanTimeout++;
                        if (!(locked && present)) System.Threading.Thread.Sleep(100);
                    } 
                    while (!(locked && present) && sucatadaScanTimeout < 10);

                    if (locked && present)
                    {
                        IMpeg2Data mpeg2Data = graphBuilderBDA.SectionsAndTables as IMpeg2Data;

                        short originalNetworkId = -1;
                        Hashtable serviceNameByServiceId = new Hashtable();

                        Hashtable serviceTypeByServiceID = new Hashtable();
                        PSISection[] psiSdts = PSISection.GetPSITable((int)PIDS.SDT, (int)TABLE_IDS.SDT_ACTUAL, mpeg2Data);
                        for (int i = 0; i < psiSdts.Length; i++)
                        {
                            PSISection psiSdt = psiSdts[i];
                            if (psiSdt != null && psiSdt is PSISDT)
                            {

                                PSISDT sdt = (PSISDT)psiSdt;

                                originalNetworkId = (short)sdt.OriginalNetworkId;
                                foreach (PSISDT.Data service in sdt.Services)
                                {
                                    serviceNameByServiceId[service.ServiceId] = service.GetServiceName();
                                    serviceTypeByServiceID[service.ServiceId] = service.GetServiceType();
                                }
                            }
                        }

                        Hashtable logicalChannelNumberByServiceId = new Hashtable();
                        PSISection[] psiNits = PSISection.GetPSITable((int)PIDS.NIT, (int)TABLE_IDS.NIT_ACTUAL, mpeg2Data);
                        for (int i = 0; i < psiNits.Length; i++)
                        {
                            PSISection psinit = psiNits[i];
                            if (psinit != null && psinit is PSINIT)
                            {
                                PSINIT nit = (PSINIT)psinit;

                                foreach (PSINIT.Data data in nit.Streams)
                                {
                                    foreach (PSIDescriptor psiDescriptorData in data.Descriptors)
                                    {
                                        if (psiDescriptorData.DescriptorTag == DESCRIPTOR_TAGS.DESCR_LOGICAL_CHANNEL)
                                        {
                                            PSIDescriptorLogicalChannel psiDescriptorLogicalChannel = (PSIDescriptorLogicalChannel)psiDescriptorData;
                                            foreach (PSIDescriptorLogicalChannel.ChannelNumber f in psiDescriptorLogicalChannel.LogicalChannelNumbers)
                                            {
                                                logicalChannelNumberByServiceId[f.ServiceID] = f.LogicalChannelNumber; //números dos canais
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        PSISection[] psis = PSISection.GetPSITable((int)PIDS.PAT, (int)TABLE_IDS.PAT, mpeg2Data);
                        for (int i = 0; i < psis.Length; i++)
                        {
                            PSISection psi = psis[i];
                            if (psi != null && psi is PSIPAT)
                            {
                                PSIPAT pat = (PSIPAT)psi;

                                ChannelDVB newTemplateChannelDVB = (this.GraphicBuilder as ITV).CurrentChannel.MakeCopy() as ChannelDVB;
                                newTemplateChannelDVB.ONID = originalNetworkId;
                                newTemplateChannelDVB.TSID = pat.TransportStreamId;

                                foreach (PSIPAT.Data program in pat.ProgramIds)
                                {
                                    if (!program.IsNetworkPID)
                                    {
                                        SERVICE_TYPES st;
                                        try
                                        {
                                            st = (SERVICE_TYPES)serviceTypeByServiceID[program.ProgramNumber];
                                        }
                                        catch
                                        {
                                            st = SERVICE_TYPES.ADVANCE_CODEC_SD_DIGITAL_SERVICE; //Fail safe to get the programs with bad reception
                                        }

                                        if (st == SERVICE_TYPES.DIGITAL_TELEVISION_SERVICE ||                       // TV SD MPEG2
                                            st == SERVICE_TYPES.ADVANCE_CODEC_HD_DIGITAL_SERVICE ||                 // TV HD H264
                                            st == SERVICE_TYPES.ADVANCE_CODEC_SD_DIGITAL_SERVICE ||                 // TV SD H264
                                            st == SERVICE_TYPES.MPEG2_HD_DIGITAL_TELEVISION_SERVICE ||              // TV HD MPEG2
                                            st == SERVICE_TYPES.DIGITAL_RADIO_SOUND_SERVICE ||                      // Radio MP2
                                            st == SERVICE_TYPES.ADVANCED_CODEC_DIGITAL_RADIO_SOUND_SERVICE ||        // Radio AC3/E-AC3/AAC
                                            st == SERVICE_TYPES.MOSAIC_SERVICE ||                                   // Mosaic MPEG2                                
                                            st == SERVICE_TYPES.ADVANCED_CODEC_MOSAIC_SERVICE)                      // Mosaic H264
                                        {
                                            ChannelDVB newChannelDVB = newTemplateChannelDVB.MakeCopy() as ChannelDVB;
                                            newChannelDVB.SID = program.ProgramNumber;
                                            newChannelDVB.Name = (string)serviceNameByServiceId[program.ProgramNumber];
                                            //Hervé Stalin: ajout du LCN 
                                            newChannelDVB.ChannelNumber = Convert.ToInt16(logicalChannelNumberByServiceId[program.ProgramNumber]);

                                            if (newChannelDVB.Name == null)
                                                newChannelDVB.Name = Properties.Resources.NoName;

                                            UpdateDVBChannelPids(mpeg2Data, program.Pid, newChannelDVB);

                                            //if (currentChannelTV is ChannelDVBT)
                                            //{
                                            //    newChannelDVB.Frequency = (currentChannelTV as ChannelDVBT).Frequency;
                                            //}
                                            //RECOLHER OS CANAIS AQUI
                                            this.OnNewLogMessage(string.Format("Channel {0} found.", newChannelDVB.Name));

                                            if (!this.Channels.ChannelList.Contains<ChannelDVBT>(newChannelDVB as ChannelDVBT))
                                            {
                                                this.Channels.ChannelList.Add(newChannelDVB as ChannelDVBT);
                                                this.OnNewLogMessage(string.Format("Channel {0} added. Total: {1} channel{2}", newChannelDVB.Name, this.Channels.ChannelList.Count, this.Channels.ChannelList.Count == 1 ? "" : "s"));

                                                OnChannelListChanged();
                                            }
                                            else
                                            {
                                                this.OnNewLogMessage(string.Format("Channel {0} already on the list, skipping.", newChannelDVB.Name));
                                            }
                                        }
                                    }
                                }

                                Application.DoEvents();
                            }
                        }
                    }
                    
                }
            }

            return locked && present;
        }
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

    public delegate void ChannelListCallbackDelegate(object sender, IEnumerable<Channel> channels);
}
