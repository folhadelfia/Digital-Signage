using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using DirectShowLib;
using Microsoft.Win32;

/*
 * HRs
 *      -1 - Dispositivo de captura de vídeo não seleccionado
 * 
 * 
 * 
 * 
 * 
 */

namespace VideoPlayerDShowLib
{
    public class CrossbarVideoPlayer : VideoControl
    {
        #region VideoSettings (CrossbarVideoPlayer.VideoSettingsWrapper)

        public class VideoSettingsWrapper
        {
            public enum VideoSizeMode { FromInside, FromOutside, Free, StretchToWindow }
            public enum VideoRatio { Force169, Force43, KeepOriginal }

            private CrossbarVideoPlayer owner;
            internal VideoSettingsWrapper(CrossbarVideoPlayer _owner) 
            { 
                this.owner = _owner;
                this.ZoomValue = 1.0;
            }



            internal Rectangle currentVideoTargetRectangle;
            internal Size currentVideoSourceSize;

            internal Rectangle[] GetBlackBands()
            {
                Rectangle outerRectangle = owner.ClientRectangle;
                DsRect innerDsRect = new DsRect();
                int hr;

                IVMRWindowlessControl9 vmrWindowlessControl9 = owner.GraphBuilder.videoRendererFilter as IVMRWindowlessControl9;
                hr = vmrWindowlessControl9.GetVideoPosition(null, innerDsRect);

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
            internal void PaintBlackBands(Graphics g)
            {
                if (owner.GraphBuilder.videoRendererFilter != null)
                {
                    Rectangle[] alRectangles = GetBlackBands();
                    if (alRectangles.Length > 0)
                    {
                        g.FillRectangles(new SolidBrush(this.PlayerBackColor), alRectangles);
                        g.DrawRectangles(new System.Drawing.Pen(this.PlayerBackColor), alRectangles);
                    }
                }
            }

            internal VideoSizeMode videoZoomMode = VideoSizeMode.FromInside;
            internal double videoZoomValue = 0;
            internal PointF videoOffset = new PointF(0.5f, 0.5f);
            internal VideoRatio videoAspectRatio = VideoRatio.KeepOriginal;
            internal Color videoBackColor = Color.Black;

            public VideoSizeMode ZoomMode
            {
                get { return videoZoomMode; }
                set
                {
                    videoZoomMode = value;
                    RefreshVideo();
                }
            }
            public double ZoomValue
            {
                get { return videoZoomValue; }
                set { videoZoomValue = value; }
            }
            public PointF Offset
            {
                get { return videoOffset; }
                set { videoOffset = value; }
            }
            public VideoRatio AspectRatio
            {
                get { return videoAspectRatio; }
                set
                {
                    this.videoAspectRatio = value;
                    this.RefreshVideo();
                }
            }
            public Color PlayerBackColor
            {
                get { return videoBackColor; }
                set { videoBackColor = value; }
            }


            #region Video Standards
            public IEnumerable<AnalogVideoStandard> VideoStandardsSupported
            {
                get
                {
                    IEnumerable<AnalogVideoStandard> res;

                    try
                    {
                        if (owner.Devices.videoDevice == null) return new List<AnalogVideoStandard>();
                        else
                        {
                            IFilterGraph2 tmp;

                            if (owner.GraphBuilder.captureDevice == null)
                            {
                                IBaseFilter capture;
                                tmp = (IFilterGraph2)new FilterGraph();

                                tmp.AddSourceFilterForMoniker(owner.Devices.videoDevice.Mon, null, owner.Devices.videoDevice.Name, out capture);

                                this.GetVideoStandardList(capture, out res);

                                Marshal.ReleaseComObject(capture); capture = null;
                                Marshal.ReleaseComObject(tmp); tmp = null;
                            }
                            else this.GetVideoStandardList(out res);
                        }

                    }
                    catch (Exception ex)
                    {
                        owner.LogException(ex);
                        return new List<AnalogVideoStandard>();
                    }

                    return res;
                }
            }
            public AnalogVideoStandard VideoStandardCurrent
            {
                get
                {
                    AnalogVideoStandard res;

                    try
                    {
                        if (owner.Devices.videoDevice == null) return AnalogVideoStandard.None;
                        else
                        {
                            IFilterGraph2 tmp;

                            if (owner.GraphBuilder.captureDevice == null)
                            {
                                IBaseFilter capture;
                                tmp = (IFilterGraph2)new FilterGraph();

                                tmp.AddSourceFilterForMoniker(owner.Devices.videoDevice.Mon, null, owner.Devices.videoDevice.Name, out capture);

                                this.GetCurrentVideoStandard(capture, out res);

                                Marshal.ReleaseComObject(capture); capture = null;
                                Marshal.ReleaseComObject(tmp); tmp = null;
                            }
                            else this.GetCurrentVideoStandard(out res);
                        }

                    }
                    catch (Exception ex)
                    {
                        owner.LogException(ex);
                        return AnalogVideoStandard.None;
                    }

                    return res;
                }
                set
                {
                    try
                    {
                        if (owner.Devices.videoDevice == null) return;
                        else
                        {
                            IFilterGraph2 tmp;

                            if (owner.GraphBuilder.captureDevice == null)
                            {
                                IBaseFilter capture;
                                tmp = (IFilterGraph2)new FilterGraph();

                                tmp.AddSourceFilterForMoniker(owner.Devices.videoDevice.Mon, null, owner.Devices.videoDevice.Name, out capture);

                                this.SetCurrentVideoStandard(capture, value);

                                Marshal.ReleaseComObject(capture); capture = null;
                                Marshal.ReleaseComObject(tmp); tmp = null;
                            }
                            else this.SetCurrentVideoStandard(value);
                        }

                    }
                    catch (Exception ex)
                    {
                        owner.LogException(ex);
                        return;
                    }
                }
            }

            private int GetVideoStandardList(out IEnumerable<AnalogVideoStandard> formatList)
            {
                return GetVideoStandardList(owner.GraphBuilder.captureDevice, out formatList);
            }
            private int GetVideoStandardList(IBaseFilter captureFilter, out IEnumerable<AnalogVideoStandard> formatList)
            {
                int hr = 0;

                formatList = new List<AnalogVideoStandard>();

                try
                {
                    if (captureFilter == null) return 0;

                    IAMAnalogVideoDecoder dec = captureFilter as IAMAnalogVideoDecoder;
                    AnalogVideoStandard standard;

                    dec.get_AvailableTVFormats(out standard);

                    foreach (AnalogVideoStandard avs in Enum.GetValues(typeof(AnalogVideoStandard)))
                    {
                        if (avs != AnalogVideoStandard.NTSCMask &&
                            avs != AnalogVideoStandard.PALMask &&
                            avs != AnalogVideoStandard.SECAMMask &&
                            (standard & avs) == avs) (formatList as List<AnalogVideoStandard>).Add(avs);
                    }

                }
                catch (Exception ex)
                {
                    formatList = null;
                    owner.LogException(ex);
                    return hr;
                }

                return hr;
            }

            private int GetCurrentVideoStandard(out AnalogVideoStandard standard)
            {
                return GetCurrentVideoStandard(owner.GraphBuilder.captureDevice, out standard);
            }
            private int GetCurrentVideoStandard(IBaseFilter captureFilter, out AnalogVideoStandard standard)
            {
                int hr = 0;
                standard = AnalogVideoStandard.None;

                try
                {
                    if (captureFilter == null) return 0;

                    IAMAnalogVideoDecoder dec = captureFilter as IAMAnalogVideoDecoder;

                    dec.get_TVFormat(out standard);

                }
                catch (Exception ex)
                {
                    owner.LogException(ex);
                    return hr;
                }

                return hr;
            }

            private int SetCurrentVideoStandard(AnalogVideoStandard standard)
            {
                return SetCurrentVideoStandard(owner.GraphBuilder.captureDevice, standard);
            }
            private int SetCurrentVideoStandard(IBaseFilter captureFilter, AnalogVideoStandard standard)
            {
                int hr = 0;

                try
                {
                    if (captureFilter == null) return 0;

                    IAMAnalogVideoDecoder dec = captureFilter as IAMAnalogVideoDecoder;

                    dec.put_TVFormat(standard);
                }
                catch (Exception ex)
                {
                    owner.LogException(ex);
                    return hr;
                }

                return hr;
            }
            #endregion

            internal void VideoResizer()
            {
                int hr = 0;

                Rectangle windowRect = owner.ClientRectangle;
                currentVideoTargetRectangle = windowRect;
                currentVideoSourceSize = new Size();

                if (owner.State != MediaStatus.Stopped)
                {
                    if (this.ZoomMode != VideoSizeMode.StretchToWindow)
                    {
                        int arX, arY;
                        int arX2 = 0, arY2 = 0;

                        hr = (owner.GraphBuilder.videoRendererFilter as IVMRWindowlessControl9).GetNativeVideoSize(out arX, out arY, out arX2, out arY2);

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

                            double newAspectRatio = 1;

                            switch (this.AspectRatio)
                            {
                                case VideoRatio.Force169: newAspectRatio = 16.0 / 9.0;
                                    break;
                                case VideoRatio.KeepOriginal: newAspectRatio = (double)arX2 / (double)arY2;
                                    break;
                                case VideoRatio.Force43: newAspectRatio = 4.0 / 3.0;
                                    break;
                                default: newAspectRatio = 1;
                                    break;
                            }

                            int height = windowSize.Height;
                            int width = (int)((double)height * newAspectRatio);

                            if (this.ZoomMode == VideoSizeMode.FromInside || this.ZoomMode == VideoSizeMode.FromOutside)
                            {
                                if (this.ZoomMode == VideoSizeMode.FromInside && width > windowSize.Width
                                || this.ZoomMode == VideoSizeMode.FromOutside && width < windowSize.Width)
                                {
                                    width = windowSize.Width;
                                    height = (int)((double)width / newAspectRatio);
                                }
                                else
                                {
                                    height = windowSize.Height;
                                    width = (int)((double)height * newAspectRatio);
                                }
                            }

                            Size size = new Size((int)(this.ZoomValue * width), (int)(this.ZoomValue * height));

                            Point pos = new Point(
                                (int)(this.Offset.X * (windowRect.Width * 3 - size.Width) - windowRect.Width),
                                (int)(this.Offset.Y * (windowRect.Height * 3 - size.Height) - windowRect.Height));

                            //Point pos = new Point(
                            //    (int)(offset.X * (windowRect.Width - size.Width)),
                            //    (int)(offset.Y * (windowRect.Height - size.Height)));

                            currentVideoTargetRectangle = new Rectangle(pos, size);
                        }
                    }
                    else
                    {
                        currentVideoTargetRectangle = owner.ClientRectangle;
                    }

                    hr = (owner.GraphBuilder.videoRendererFilter as IVMRWindowlessControl9).SetVideoPosition(null, DsRect.FromRectangle(currentVideoTargetRectangle));
                    //Trace.WriteLineIf(trace.TraceVerbose, string.Format(("\tPos {0:F2} {1:F2}, Zoom {2:F2}, ARF {4:F2}, AR {4:F2}"), offset.X, offset.Y, zoom, aspectRatioFactor, (float)videoTargetRect.Width / videoTargetRect.Height));
                }
            }
            public void RefreshVideo()
            {
                owner.GraphBuilder.ResizeMoveHandler(null, null);
            }
        }
        private VideoSettingsWrapper vSettings;
        public VideoSettingsWrapper VideoSettings
        {
            get
            {
                if (vSettings == null)
                    vSettings = new VideoSettingsWrapper(this);

                return vSettings;
            }
        }

        #endregion

        #region Devices (CrossbarVideoPlayer.DevicesWrapper)

        public class DevicesWrapper
        {
            CrossbarVideoPlayer owner;
            internal DevicesWrapper(CrossbarVideoPlayer _owner) { this.owner = _owner; }

            #region Devices
            public static DsDevice[] VideoDevices
            {
                get
                {
                    return DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
                }
            }
            public static DsDevice[] AudioRenderers
            {
                get
                {
                    return DsDevice.GetDevicesOfCat(FilterCategory.AudioRendererCategory);
                }
            }

            internal DsDevice videoDevice;
            internal DsDevice audioRenderer;

            public DsDevice VideoDevice
            {
                get { return videoDevice; }
                set { videoDevice = value; }
            }
            public DsDevice AudioRenderer
            {
                get { return audioRenderer; }
                set { audioRenderer = value; }
            }
            #endregion

            #region Video Sources
            public class CrossbarInputPin
            {
                public PhysicalConnectorType Type { get; set; }
                internal int Number { get; set; }
                internal int Related { get; set; }

                public static bool IsVideo(PhysicalConnectorType type)
                {
                    return type == PhysicalConnectorType.Video_1394 ||
                           type == PhysicalConnectorType.Video_AUX ||
                           type == PhysicalConnectorType.Video_Black ||
                           type == PhysicalConnectorType.Video_Composite ||
                           type == PhysicalConnectorType.Video_ParallelDigital ||
                           type == PhysicalConnectorType.Video_RGB ||
                           type == PhysicalConnectorType.Video_SCART ||
                           type == PhysicalConnectorType.Video_SCSI ||
                           type == PhysicalConnectorType.Video_SerialDigital ||
                           type == PhysicalConnectorType.Video_SVideo ||
                           type == PhysicalConnectorType.Video_Tuner ||
                           type == PhysicalConnectorType.Video_USB ||
                           type == PhysicalConnectorType.Video_VideoDecoder ||
                           type == PhysicalConnectorType.Video_VideoEncoder ||
                           type == PhysicalConnectorType.Video_YRYBY;
                }
                public static bool IsAudio(PhysicalConnectorType type) { return !IsVideo(type); }

                public bool IsVideo() { return IsVideo(this.Type); }
                public bool IsAudio() { return !IsVideo(); }

                public override bool Equals(object obj)
                {
                    return (obj is CrossbarInputPin) &&
                           (obj as CrossbarInputPin).Type == this.Type &&
                           (obj as CrossbarInputPin).Number == this.Number &&
                           (obj as CrossbarInputPin).Related == this.Related;
                }
                public override int GetHashCode()
                {
                    return 100 * Number + Related;
                }
            }

            public IEnumerable<CrossbarInputPin> SourcesSupported
            {
                get
                {
                    IEnumerable<CrossbarInputPin> res = new List<CrossbarInputPin>();
                    int hr = 0;

                    try
                    {
                        if (this.videoDevice == null) return new List<CrossbarInputPin>(); //Se o device é null, não suporta nenhum tipo de source
                        else
                        {
                            if (owner.GraphBuilder.crossBar == null) //Se este filtro for null, não o podemos usar para ir buscas as sources. Fazemos um graph temporario
                            {
                                IFilterGraph2 tmp;

                                IBaseFilter capture, crossbarTemp = null;
                                tmp = (IFilterGraph2)new FilterGraph();

                                tmp.AddSourceFilterForMoniker(this.videoDevice.Mon, null, this.videoDevice.Name, out capture);

                                DsDevice[] crossbars = DsDevice.GetDevicesOfCat(FilterCategory.AMKSCrossbar);

                                if (crossbars.Length == 0) return new List<CrossbarInputPin>();

                                for (int i = 0; i < crossbars.Length; i++)
                                {
                                    crossbarTemp = null;
                                    object o;

                                    crossbars[i].Mon.BindToObject(null, null, typeof(IBaseFilter).GUID, out o);

                                    crossbarTemp = o as IBaseFilter;

                                    hr = tmp.AddFilter(crossbarTemp, "XBAR YO");

                                    if (hr >= 0)
                                    {
                                        IPin crossbarOPin0 = DsFindPin.ByDirection(crossbarTemp, PinDirection.Output, 0);

                                        IEnumPins captureDevPins;
                                        hr = capture.EnumPins(out captureDevPins);

                                        IPin[] pins = new IPin[1];

                                        while (captureDevPins.Next(1, pins, IntPtr.Zero) >= 0)
                                        {
                                            PinDirection ppindir;

                                            if (pins[0] == null) break;

                                            hr = pins[0].QueryDirection(out ppindir);

                                            if (ppindir != PinDirection.Input)
                                            {
                                                Marshal.ReleaseComObject(pins[0]);
                                                continue;
                                            }

                                            hr = tmp.Connect(crossbarOPin0, pins[0]);

                                            if (hr >= 0)
                                            {
                                                Marshal.ReleaseComObject(pins[0]);
                                                break;
                                            }
                                            else
                                            {
                                                tmp.RemoveFilter(crossbarTemp);
                                                Marshal.ReleaseComObject(crossbarTemp);
                                                crossbarTemp = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        tmp.RemoveFilter(crossbarTemp);
                                        continue;
                                    }

                                    if (crossbarTemp != null) break;
                                }

                                this.GetCrossbarPins(crossbarTemp, out res);

                                GraphBuilderClass.DestroyGraph(ref tmp);

                            }
                            else
                            {
                                this.GetCrossbarPins(owner.GraphBuilder.crossBar, out res);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        owner.LogException(ex);
                        return new List<CrossbarInputPin>();
                    }
                    finally
                    {

                    }

                    return res;
                }
            }
            public CrossbarInputPin Source
            {
                get
                {
                    CrossbarInputPin res = null;
                    int hr = 0;

                    try
                    {
                        if (this.videoDevice == null) return null; //Se o device é null, não tem tipo de source
                        else
                        {
                            if (owner.GraphBuilder.crossBar == null) //Se este filtro for null, não o podemos usar para ir buscas as sources. Fazemos um graph temporario
                            {
                                IFilterGraph2 tmp;

                                IBaseFilter capture, crossbarTemp = null;
                                tmp = (IFilterGraph2)new FilterGraph();

                                tmp.AddSourceFilterForMoniker(this.videoDevice.Mon, null, this.videoDevice.Name, out capture);

                                DsDevice[] crossbars = DsDevice.GetDevicesOfCat(FilterCategory.AMKSCrossbar);

                                if (crossbars.Length == 0) return null;

                                for (int i = 0; i < crossbars.Length; i++)
                                {
                                    crossbarTemp = null;
                                    object o;

                                    crossbars[i].Mon.BindToObject(null, null, typeof(IBaseFilter).GUID, out o);

                                    crossbarTemp = o as IBaseFilter;

                                    hr = tmp.AddFilter(crossbarTemp, "XBAR YO");

                                    if (hr >= 0)
                                    {
                                        IPin crossbarOPin0 = DsFindPin.ByDirection(crossbarTemp, PinDirection.Output, 0);

                                        IEnumPins captureDevPins;
                                        hr = capture.EnumPins(out captureDevPins);

                                        IPin[] pins = new IPin[1];

                                        while (captureDevPins.Next(1, pins, IntPtr.Zero) >= 0)
                                        {
                                            PinDirection ppindir;

                                            if (pins[0] == null) break;

                                            hr = pins[0].QueryDirection(out ppindir);

                                            if (ppindir != PinDirection.Input)
                                            {
                                                Marshal.ReleaseComObject(pins[0]);
                                                continue;
                                            }

                                            hr = tmp.Connect(crossbarOPin0, pins[0]);

                                            if (hr >= 0)
                                            {
                                                Marshal.ReleaseComObject(pins[0]);
                                                break;
                                            }
                                            else
                                            {
                                                tmp.RemoveFilter(crossbarTemp);
                                                Marshal.ReleaseComObject(crossbarTemp);
                                                crossbarTemp = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        tmp.RemoveFilter(crossbarTemp);
                                        continue;
                                    }

                                    if (crossbarTemp != null) break;
                                }

                                this.GetCrossbarVideoSource(crossbarTemp, out res);

                                GraphBuilderClass.DestroyGraph(ref tmp);
                            }
                            else
                            {
                                this.GetCrossbarVideoSource(owner.GraphBuilder.crossBar, out res);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        owner.LogException(ex);
                        return new CrossbarInputPin();
                    }
                    finally
                    {

                    }

                    return res;
                }
                set
                {
                    int hr = 0;

                    try
                    {
                        if (this.videoDevice == null || value == null) return; //Se o device é null, não tem tipo de source
                        else
                        {
                            if (owner.GraphBuilder.crossBar == null) //Se este filtro for null, não o podemos usar para ir buscas as sources. Fazemos um graph temporario
                            {
                                IFilterGraph2 tmp;

                                IBaseFilter capture, crossbarTemp = null;
                                tmp = (IFilterGraph2)new FilterGraph();

                                tmp.AddSourceFilterForMoniker(this.videoDevice.Mon, null, this.videoDevice.Name, out capture);

                                DsDevice[] crossbars = DsDevice.GetDevicesOfCat(FilterCategory.AMKSCrossbar);

                                if (crossbars.Length == 0) return;

                                for (int i = 0; i < crossbars.Length; i++)
                                {
                                    crossbarTemp = null;
                                    object o;

                                    crossbars[i].Mon.BindToObject(null, null, typeof(IBaseFilter).GUID, out o);

                                    crossbarTemp = o as IBaseFilter;

                                    hr = tmp.AddFilter(crossbarTemp, "XBAR YO");

                                    if (hr >= 0)
                                    {
                                        IPin crossbarOPin0 = DsFindPin.ByDirection(crossbarTemp, PinDirection.Output, 0);

                                        IEnumPins captureDevPins;
                                        hr = capture.EnumPins(out captureDevPins);

                                        IPin[] pins = new IPin[1];

                                        while (captureDevPins.Next(1, pins, IntPtr.Zero) >= 0)
                                        {
                                            PinDirection ppindir;

                                            if (pins[0] == null) break;

                                            hr = pins[0].QueryDirection(out ppindir);

                                            if (ppindir != PinDirection.Input)
                                            {
                                                Marshal.ReleaseComObject(pins[0]);
                                                continue;
                                            }

                                            hr = tmp.Connect(crossbarOPin0, pins[0]);

                                            if (hr >= 0)
                                            {
                                                //Got it

                                                Marshal.ReleaseComObject(pins[0]);
                                                break;
                                            }
                                            else
                                            {
                                                tmp.RemoveFilter(crossbarTemp);
                                                Marshal.ReleaseComObject(crossbarTemp);
                                                crossbarTemp = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        tmp.RemoveFilter(crossbarTemp);
                                        continue;
                                    }

                                    if (crossbarTemp != null) break;
                                }

                                this.SetCrossbarVideoSource(crossbarTemp, value);

                                GraphBuilderClass.DestroyGraph(ref tmp);
                            }
                            else
                            {
                                this.SetCrossbarVideoSource(value);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        owner.LogException(ex);
                        return;
                    }
                    finally
                    {

                    }
                }
            }

            internal int GetCrossbarPins(out IEnumerable<CrossbarInputPin> pins)
            {
                return GetCrossbarPins(owner.GraphBuilder.crossBar, out pins);
            }
            /// <summary>
            /// Retorna uma lista com os modos de input da crossbar. Se retornar -1, a crossbar não é válida.
            /// </summary>
            /// <param name="crossbar"></param>
            /// <param name="pins"></param>
            /// <returns></returns>
            internal int GetCrossbarPins(IBaseFilter crossbar, out IEnumerable<CrossbarInputPin> pins)
            {
                int hr = 0;

                if (crossbar == null)
                {
                    pins = null;
                    return -1;
                }


                List<CrossbarInputPin> crossbarVideoInputPins = new List<CrossbarInputPin>();

                try
                {
                    List<PhysicalConnectorType> res = new List<PhysicalConnectorType>();

                    IAMCrossbar xbar = crossbar as IAMCrossbar;

                    int pinInCount, pinOutCount;

                    xbar.get_PinCounts(out pinOutCount, out pinInCount);

                    for (int i = 0; i < pinInCount; i++)
                    {
                        int relatedPin;
                        PhysicalConnectorType type;
                        xbar.get_CrossbarPinInfo(true, i, out relatedPin, out type);

                        if (CrossbarInputPin.IsVideo(type))
                        {
                            crossbarVideoInputPins.Add(new CrossbarInputPin() { Type = type, Number = i, Related = relatedPin });
                            res.Add(type);
                        }
                    }
                }
                catch (Exception ex)
                {
                    owner.LogException(ex);
                    pins = null;
                    return hr;
                }
                finally
                {
                }

                pins = crossbarVideoInputPins;
                return hr;
            }

            internal int GetCrossbarVideoSources(out IEnumerable<PhysicalConnectorType> sources)
            {
                return GetCrossbarVideoSources(owner.GraphBuilder.crossBar, out sources);
            }
            internal int GetCrossbarVideoSources(IBaseFilter crossbar, out IEnumerable<PhysicalConnectorType> sources)
            {
                IEnumerable<CrossbarInputPin> pins = null;
                sources = new List<PhysicalConnectorType>();

                int hr = GetCrossbarPins(crossbar, out pins);

                if (hr >= 0)
                {
                    foreach (var item in pins.Select(x => x.Type))
                    {
                        (sources as List<PhysicalConnectorType>).Add(item);
                    }
                }

                return hr;
            }

            internal int SetCrossbarVideoSource(CrossbarInputPin source)
            {
                return this.SetCrossbarVideoSource(owner.GraphBuilder.crossBar, source);
            }
            internal int SetCrossbarVideoSource(IBaseFilter crossbar, CrossbarInputPin source)
            {
                if (crossbar == null) return -1;

                int hr = 0;

                try
                {
                    IAMCrossbar crossBarTemp = crossbar as IAMCrossbar;

                    int videoOPinNmr = -1;
                    int audioOPinNmr = -1;
                    IPin tmpPin;

                    IEnumMediaTypes enumMediaTypes = null;
                    AMMediaType[] mediaTypes = new AMMediaType[1];

                    try
                    {

                        tmpPin = DsFindPin.ByDirection(crossBarTemp as IBaseFilter, PinDirection.Output, 0);

                        hr = tmpPin.EnumMediaTypes(out enumMediaTypes);
                        DsError.ThrowExceptionForHR(hr);

                        if (enumMediaTypes.Next(mediaTypes.Length, mediaTypes, IntPtr.Zero) == 0)
                        {
                            if (mediaTypes[0].majorType == MediaType.Video || mediaTypes[0].majorType == MediaType.AnalogVideo)
                            {
                                videoOPinNmr = 0;
                            }
                            else if (mediaTypes[0].majorType == MediaType.Audio || mediaTypes[0].majorType == MediaType.AnalogAudio)
                            {
                                audioOPinNmr = 0;
                            }
                        }

                        tmpPin = DsFindPin.ByDirection(crossBarTemp as IBaseFilter, PinDirection.Output, 1);

                        hr = tmpPin.EnumMediaTypes(out enumMediaTypes);
                        DsError.ThrowExceptionForHR(hr);

                        if (enumMediaTypes.Next(mediaTypes.Length, mediaTypes, IntPtr.Zero) == 0)
                        {
                            if (mediaTypes[0].majorType == MediaType.Video || mediaTypes[0].majorType == MediaType.AnalogVideo)
                            {
                                videoOPinNmr = 1;
                            }
                            else if (mediaTypes[0].majorType == MediaType.Audio || mediaTypes[0].majorType == MediaType.AnalogAudio)
                            {
                                audioOPinNmr = 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        owner.LogException(ex);
                        return hr;
                    }

                    if (videoOPinNmr != -1 && audioOPinNmr != -1)
                    {
                        if (crossBarTemp.CanRoute(videoOPinNmr, source.Number) == 0)
                        {
                            hr = crossBarTemp.Route(videoOPinNmr, source.Number);
                            DsError.ThrowExceptionForHR(hr);

                            if (hr >= 0)
                            {
                                if (crossBarTemp.CanRoute(audioOPinNmr, source.Related) == 0)
                                {
                                    hr = crossBarTemp.Route(audioOPinNmr, source.Related);
                                    DsError.ThrowExceptionForHR(hr);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    owner.LogException(ex);
                    return hr;
                }

                return hr;
            }

            internal int GetCrossbarVideoSource(out CrossbarInputPin videoSource)
            {
                return GetCrossbarVideoSource(owner.GraphBuilder.crossBar, out videoSource);
            }
            internal int GetCrossbarVideoSource(IBaseFilter crossbar, out CrossbarInputPin videoSource)
            {
                int hr = 0;

                if (crossbar == null)
                {
                    videoSource = null;
                    return -1;
                }

                IAMCrossbar crossbarTemp = crossbar as IAMCrossbar;

                int pinNumber = 0;
                bool videoPinFound = false;

                while (!videoPinFound)
                {
                    IPin crossbarOutPin = DsFindPin.ByDirection(crossbar, PinDirection.Output, pinNumber);

                    if (crossbarOutPin == null)
                    {
                        videoSource = null;
                        return -1;
                    }

                    IEnumMediaTypes enumMediaTypes = null;
                    AMMediaType[] mediaTypes = new AMMediaType[1];

                    hr = crossbarOutPin.EnumMediaTypes(out enumMediaTypes);
                    DsError.ThrowExceptionForHR(hr);

                    while (enumMediaTypes.Next(mediaTypes.Length, mediaTypes, IntPtr.Zero) == 0)
                        videoPinFound = mediaTypes[0].majorType == MediaType.AnalogVideo || mediaTypes[0].majorType == MediaType.Video;

                    if (!videoPinFound) pinNumber++;
                }
                int inputPinNumber = 0;
                int relatedPinNumber;
                PhysicalConnectorType cType;

                crossbarTemp.get_IsRoutedTo(pinNumber, out inputPinNumber);

                hr = crossbarTemp.get_CrossbarPinInfo(true, inputPinNumber, out relatedPinNumber, out cType);

                videoSource = new CrossbarInputPin() { Type = cType, Number = pinNumber, Related = relatedPinNumber };
                return hr;
            }

            #endregion
        }
        private DevicesWrapper devices;
        public DevicesWrapper Devices
        {
            get
            {
                if (devices == null)
                    devices = new DevicesWrapper(this);

                return devices;
            }
        }

        public static DsDevice[] VideoDevices { get { return DevicesWrapper.VideoDevices; } }
        public static DsDevice[] AudioRenderers { get { return DevicesWrapper.AudioRenderers; } }

        #endregion

        #region Events
        #endregion

        #region Logs
        public event LogEventHandler NewLogMessage;
        internal void OnNewLogMessage(string message)
        {
            if (NewLogMessage != null) NewLogMessage(message);
        }

        internal void LogException(Exception ex)
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


        public enum MediaStatus { Stopped, Paused, Running };
        internal MediaStatus state;
        public MediaStatus State
        {
            get
            {
                return state;
            }
            internal set
            {
                //if (value != this.State) OnStateChanged();
                state = value;
            }
        }

        internal IntPtr BaseHandle { get { return base.Handle; } }

        internal GraphBuilderClass graphBuilder;
        internal GraphBuilderClass GraphBuilder
        {
            get
            {
                if (graphBuilder == null)
                    graphBuilder = new GraphBuilderClass(this);

                return graphBuilder;
            }
        }

        public int Start()
        {
            int hr = 0;
            if (this.State != MediaStatus.Running)
            {
                try
                {
                    hr = GraphBuilder.BuildGraph();
                    DsError.ThrowExceptionForHR(hr);
                    hr = GraphBuilder.RunGraph();
                    DsError.ThrowExceptionForHR(hr);


                    this.State = MediaStatus.Running;
                    this.OnResize(null);
                }
                catch (Exception ex)
                {
                    LogException(ex);

                    try
                    {
                        hr = GraphBuilder.Decompose();
                        DsError.ThrowExceptionForHR(hr);
                    }
                    catch (Exception ex1)
                    {
                        LogException(ex1);
                        return hr;
                    }
                    return hr;
                }
            }

            return hr;
        }
        public int Stop()
        {
            int hr = 0;

            try
            {
                hr = this.GraphBuilder.StopWhenReady();
                DsError.ThrowExceptionForHR(hr);

                hr = GraphBuilder.Decompose();
                DsError.ThrowExceptionForHR(hr);

                this.VideoSettings.RefreshVideo();
            }
            catch (Exception ex)
            {
                this.GraphBuilder.Stop();
                GraphBuilder.Decompose();

                LogException(ex);
                return hr;
            }

            //Já poe o estado a Stopped no Decompose

            return hr;
        }

        public CrossbarVideoPlayer()
        {
            this.State = MediaStatus.Stopped;
        }
        public CrossbarVideoPlayer(DsDevice videoSource)
        {
            this.Devices.VideoDevice = videoSource;
            this.State = MediaStatus.Stopped;
        }
        public CrossbarVideoPlayer(DsDevice videoSource, DsDevice audioRenderer)
        {
            this.Devices.VideoDevice = videoSource;
            this.Devices.AudioRenderer = audioRenderer;
            this.State = MediaStatus.Stopped;
        }

    }

    internal class GraphBuilderClass
    {
        private CrossbarVideoPlayer owner;
        public GraphBuilderClass(CrossbarVideoPlayer _owner)
        {
            owner = _owner;
        }


        #region Graph
        internal IFilterGraph2 graphBuilder;

        internal IBaseFilter crossBar;
        internal IBaseFilter captureDevice;
        internal IBaseFilter videoDecompressor;
        internal IBaseFilter videoRendererFilter;
        internal IBaseFilter audioRendererFilter;

#if DEBUG
        internal DsROTEntry rot;
#endif

        internal int BuildGraph()
        {
            int hr = 0;

            this.graphBuilder = (IFilterGraph2)new FilterGraph();
#if DEBUG
            rot = new DsROTEntry(graphBuilder);
#endif
            try
            {
                hr = AddCaptureDeviceFilter();
                if (hr == -1) throw new ApplicationException("No video device selected");
                DsError.ThrowExceptionForHR(hr);
                hr = AddAndConnectCrossbarForDevice();
                DsError.ThrowExceptionForHR(hr);
                hr = AddRenderers();
                DsError.ThrowExceptionForHR(hr);
                hr = ConfigureVMR9WindowlessMode();
                DsError.ThrowExceptionForHR(hr);
                hr = ConnectFilters();
                DsError.ThrowExceptionForHR(hr);
                Vmr9SetDeinterlaceMode(1);

            }
            catch (Exception ex)
            {
                owner.LogException(ex);

                return hr;
            }
            finally
            {
            }


            return hr;
        }
        internal int RunGraph()
        {
            (this.graphBuilder as IMediaControl).Run();

            return 0;
            //return (this.graphBuilder as IMediaControl).Run();
        }
        internal int Stop()
        {
            return (graphBuilder as IMediaControl).Stop();
        }
        internal int StopWhenReady()
        {
            return (graphBuilder as IMediaControl).StopWhenReady();
        }
        internal int Decompose()
        {
            int hr = 0;

            hr = (this.graphBuilder as IMediaControl).StopWhenReady();
            hr = (this.graphBuilder as IMediaControl).Stop();

            owner.State = CrossbarVideoPlayer.MediaStatus.Stopped;

            RemoveHandlers();

            FilterGraphTools.RemoveAllFilters(this.graphBuilder);

            if (this.crossBar != null) Marshal.ReleaseComObject(this.crossBar); this.crossBar = null;
            if (this.captureDevice != null) Marshal.ReleaseComObject(this.captureDevice); this.captureDevice = null;
            if (this.videoDecompressor != null) Marshal.ReleaseComObject(this.videoDecompressor); this.videoDecompressor = null;
            if (this.videoRendererFilter != null) Marshal.ReleaseComObject(this.videoRendererFilter); this.videoRendererFilter = null;
            if (this.audioRendererFilter != null) Marshal.ReleaseComObject(this.audioRendererFilter); this.audioRendererFilter = null;
#if DEBUG
            if (rot != null) rot.Dispose();
#endif
            Marshal.ReleaseComObject(graphBuilder); this.graphBuilder = null;

            return hr;
        }

        internal int AddCaptureDeviceFilter()
        {
            int hr = 0;

            Guid iid = typeof(IBaseFilter).GUID;

            try
            {
                if (owner.Devices.VideoDevice != null)
                {
                    this.captureDevice = null;

                    return this.graphBuilder.AddSourceFilterForMoniker(owner.Devices.VideoDevice.Mon, null, owner.Devices.VideoDevice.Name, out this.captureDevice);

                    //IBaseFilter tmp;

                    //object o;
                    //this.VideoSource.Mon.BindToObject(null, null, ref iid, out o);
                    //tmp = o as IBaseFilter;

                    //DsError.ThrowExceptionForHR(hr);

                    //this.captureDevice = tmp;
                }
                else //VideoSource == null
                {
                    hr = -1;
                    throw new ApplicationException("No video source selected");
                }
            }
            catch (Exception ex)
            {
                owner.LogException(ex);
                return hr;
            }
        }
        internal int AddAndConnectCrossbarForDevice()
        {
            int hr = 0;
            Guid iid = typeof(IBaseFilter).GUID;
            try
            {
                if (captureDevice != null)
                {
                    DsDevice[] crossbars = DsDevice.GetDevicesOfCat(FilterCategory.AMKSCrossbar);
                    if (crossbars.Length == 0)
                    {
                        hr = -2;
                        throw new ApplicationException("Can't find a suitable crossbar for the selected device");
                    }

                    for (int i = 0; i < crossbars.Length; i++)
                    {
                        this.crossBar = null;

                        IBaseFilter tmpCrossbar;

                        object o;

                        crossbars[i].Mon.BindToObject(null, null, ref iid, out o);

                        tmpCrossbar = o as IBaseFilter;
                        hr = graphBuilder.AddSourceFilterForMoniker(crossbars[i].Mon, null, crossbars[i].Name, out tmpCrossbar);

                        if (hr >= 0)
                        {
                            //Crossbar válido

                            IPin crossbarOPin0 = DsFindPin.ByDirection(tmpCrossbar, PinDirection.Output, 0);
                            IPin crossbarOPin1 = DsFindPin.ByDirection(tmpCrossbar, PinDirection.Output, 1);

                            bool pin0Connected = false;
                            bool pin1Connected = false;

                            IEnumPins captureDevPins;
                            hr = captureDevice.EnumPins(out captureDevPins);

                            IPin[] pins = new IPin[1];

                            /*
                             * No ciclo a seguir tento ligar os pins de entrada do device a qualquer pin do crossbar.
                             * Se algum ligar é porque o crossbar é válido para o device
                             * Se nenhum ligar tenta o próximo crossbar
                             */

                            while (captureDevPins.Next(1, pins, IntPtr.Zero) >= 0)
                            {
                                PinDirection ppindir;

                                if (pins[0] == null) break;

                                hr = pins[0].QueryDirection(out ppindir);

                                if (ppindir != PinDirection.Input)
                                {
                                    Marshal.ReleaseComObject(pins[0]);
                                    continue;
                                }

                                hr = this.graphBuilder.Connect(crossbarOPin0, pins[0]);
                                if (hr >= 0) pin0Connected = true;
                                else
                                {
                                    hr = this.graphBuilder.Connect(crossbarOPin1, pins[0]);
                                    if (hr >= 0) pin1Connected = true;
                                }

                                if (hr >= 0)
                                {
                                    //Este é o crossbar que procuramos
                                    Marshal.ReleaseComObject(pins[0]);
                                    this.crossBar = tmpCrossbar;
                                    if (!(pin0Connected && pin1Connected))
                                        continue;
                                    else return hr;
                                }
                                else
                                {
                                    Marshal.ReleaseComObject(pins[0]);
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            graphBuilder.RemoveFilter(tmpCrossbar);
                            Marshal.ReleaseComObject(tmpCrossbar);

                            continue;
                        }
                    }//Nenhum crossbar encontrado

                }
            }
            catch (Exception ex)
            {
                owner.LogException(ex);
                return hr;
            }

            return hr;
        }
        internal int AddRenderers()
        {
            int hr = 0;

            try
            {

                #region Video

                this.videoRendererFilter = (IBaseFilter)new VideoMixingRenderer9();
                hr = this.graphBuilder.AddFilter(this.videoRendererFilter, "Video Renderer 9");

                DsError.ThrowExceptionForHR(hr);

                #endregion

                #region Audio

                if (owner.Devices.AudioRenderer != null)
                {
                    hr = this.graphBuilder.AddSourceFilterForMoniker(owner.Devices.AudioRenderer.Mon, null, owner.Devices.AudioRenderer.Name, out this.audioRendererFilter);

                    DsError.ThrowExceptionForHR(hr);
                }

                #endregion
            }
            catch (Exception ex)
            {
                owner.LogException(ex);
                return hr;
            }

            return hr;
        }
        internal int ConfigureVMR9WindowlessMode()
        {
            int hr = 0;

            IVMRFilterConfig9 filterConfig = this.videoRendererFilter as IVMRFilterConfig9;

            //Configurar o VMR9 em modo Windowless
            hr = filterConfig.SetRenderingMode(VMR9Mode.Windowless);
            DsError.ThrowExceptionForHR(hr);

            //Com uma stream
            hr = filterConfig.SetNumberOfStreams(1);
            DsError.ThrowExceptionForHR(hr);

            IVMRWindowlessControl9 windowlessControl = this.videoRendererFilter as IVMRWindowlessControl9;

            //Adicionar uma referência para quem vai ser o host
            hr = windowlessControl.SetVideoClippingWindow(owner.BaseHandle);
            DsError.ThrowExceptionForHR(hr);


            ResizeMoveHandler(null, null);

            AddHandlers();

            return hr;
        }
        internal int ConnectFilters()
        {
            int hr = 0;
            int pinNumber = 0;
            IPin pinOut, pinIn;

            bool audioRendered = false;
            bool videoRendered = false;

            while (!(videoRendered && audioRendered))
            {
                pinOut = DsFindPin.ByDirection(captureDevice, PinDirection.Output, pinNumber);

                if (pinOut == null) break;

                IEnumMediaTypes enumMediaTypes = null;
                AMMediaType[] mediaTypes = new AMMediaType[1];

                try
                {
                    hr = pinOut.EnumMediaTypes(out enumMediaTypes);
                    DsError.ThrowExceptionForHR(hr);

                    while (enumMediaTypes.Next(mediaTypes.Length, mediaTypes, IntPtr.Zero) == 0)
                    {
                        Guid majorType = mediaTypes[0].majorType;

                        if (majorType == MediaType.Audio)
                        {
                            if (!audioRendered)
                            {
                                pinIn = DsFindPin.ByDirection(this.audioRendererFilter, PinDirection.Input, 0);

                                hr = graphBuilder.Connect(pinOut, pinIn);
                                DsError.ThrowExceptionForHR(hr);

                                Marshal.ReleaseComObject(pinIn);
                                pinIn = null;

                                audioRendered = true;
                            }
                        }
                        else if (majorType == MediaType.Video)
                        {
                            if (!videoRendered)
                            {
                                pinIn = DsFindPin.ByDirection(this.videoRendererFilter, PinDirection.Input, 0);

                                hr = graphBuilder.Connect(pinOut, pinIn);
                                DsError.ThrowExceptionForHR(hr);

                                Marshal.ReleaseComObject(pinIn);
                                pinIn = null;

                                videoRendered = true;
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

                pinNumber++;
            }

            return hr;
        }

        public static int DestroyGraph(ref IFilterGraph2 graph)
        {
            if (graph == null) return -1;

            int hr = 0;

            try
            {
                FilterGraphTools.RemoveAllFilters(graph);

                Marshal.ReleaseComObject(graph); graph = null;
            }
            catch (Exception)
            {
                
                return hr;
            }

            return hr;
        }

        internal readonly Guid bobDxvaGuid = new Guid(0x335aa36e, 0x7884, 0x43a4, 0x9c, 0x91, 0x7f, 0x87, 0xfa, 0xf3, 0xe3, 0x7e);
        internal void Vmr9SetDeinterlaceMode(int mode) //SUCATADA LIES WITHIN
        {
            //0=None
            //1=Bob
            //2=Weave
            //3=Best
            //Log("vmr9:SetDeinterlace() SetDeinterlaceMode(%d)",mode);

            IVMRDeinterlaceControl9 pDeint = (this.videoRendererFilter as IVMRDeinterlaceControl9);

            if (pDeint != null)
            {
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

        #region Old deinterlace code
        //public bool ForceDeinterlace { get; set; }
        //private void Vmr9SetDeinterlaceMode2()
        //{
        //    VMR9VideoDesc videoDescription;
        //    AMMediaType mediaType = new AMMediaType();

        //    IPin vmrInPin = DsFindPin.ByDirection(videoRendererFilter, PinDirection.Input, 0);

        //    vmrInPin.ConnectionMediaType(mediaType);


        //    if (!this.ForceDeinterlace)
        //    {
        //        if(mediaType.formatType == FormatType.VideoInfo)
        //        {
        //            OnNewLogMessage("Video is progressive");
        //            return;
        //        }
        //        else if (mediaType.formatType == FormatType.VideoInfo2)
        //        {
        //            VideoInfoHeader2 info = new VideoInfoHeader2();

        //            Marshal.PtrToStructure(mediaType.formatPtr, info);

        //            if ((info.InterlaceFlags & AMInterlace.IsInterlaced) != AMInterlace.IsInterlaced)
        //            {
        //                OnNewLogMessage("Video is progressive");
        //                return;
        //            }
        //        }
        //    }

        //    OnNewLogMessage(this.ForceDeinterlace ? "Forcing deinterlace" : "Video is interlaced");
        //}
        #endregion

        #region Handlers
        internal void AddHandlers()
        {
            owner.OnNewLogMessage("Adding handlers");
            // Add Windows Messages handlers
            owner.Paint += new PaintEventHandler(PaintHandler); // for WM_PAINT
            owner.Resize += new EventHandler(ResizeMoveHandler); // for WM_SIZE
            owner.Move += new EventHandler(ResizeMoveHandler); // for WM_MOVE
            owner.SizeChanged += new EventHandler(ResizeMoveHandler);
            SystemEvents.DisplaySettingsChanged += new EventHandler(DisplayChangedHandler); // for WM_DISPLAYCHANGE
            owner.OnNewLogMessage("Handlers added");
        } //Dentro do ConfigureVMR9WindowlessMode
        internal void RemoveHandlers()
        {
            owner.OnNewLogMessage("Removing handlers");
            // Remove Windows Messages handlers
            owner.Paint -= new PaintEventHandler(PaintHandler); // for WM_PAINT
            owner.Resize -= new EventHandler(ResizeMoveHandler); // for WM_SIZE
            owner.Move -= new EventHandler(ResizeMoveHandler); // for WM_MOVE
            owner.SizeChanged += new EventHandler(ResizeMoveHandler);
            SystemEvents.DisplaySettingsChanged -= new EventHandler(DisplayChangedHandler); // for WM_DISPLAYCHANGE
            owner.OnNewLogMessage("Handlers removed");
        } //Dentro do Decompose

        internal void PaintHandler(object sender, PaintEventArgs e)
        {
            if (this.videoRendererFilter != null && owner.State == CrossbarVideoPlayer.MediaStatus.Running)
            {
                IntPtr hdc = e.Graphics.GetHdc();
                int hr = (this.videoRendererFilter as IVMRWindowlessControl9).RepaintVideo(owner.Handle, hdc);
                e.Graphics.ReleaseHdc(hdc);

                Graphics g = owner.CreateGraphics();
                owner.VideoSettings.PaintBlackBands(g);
                g.Dispose();
            }
        }
        internal void ResizeMoveHandler(object sender, EventArgs e)
        {
            if (this.videoRendererFilter != null)
            {
                //Trace.WriteLineIf(trace.TraceInfo, "OnResizeMoveHandler()");

                owner.VideoSettings.VideoResizer();

                //Graphics g = owner.CreateGraphics();
                //owner.VideoSettings.PaintBlackBands(g);
                //g.Dispose();
            }
        }
        internal void DisplayChangedHandler(object sender, EventArgs e)
        {
            if (this.videoRendererFilter != null)
            {
                int hr = (this.videoRendererFilter as IVMRWindowlessControl9).DisplayModeChanged();
            }
        }
        #endregion
        #endregion
    }
}
