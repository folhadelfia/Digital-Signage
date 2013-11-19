using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assemblies.XMLSerialization.Drawing;

namespace Assemblies.XMLSerialization.Components
{
    public class XMLChannel
    {
        public string AudioDecoderDevice { get; set; }

        public int AudioDecoderType { get; set; }

        public int AudioPid { get; set; }

        public int[] AudioPids { get; set; }

        public string AudioRendererDevice { get; set; }

        public int Bandwidth { get; set; }

        public string CaptureDevice { get; set; }

        public short ChannelNumber { get; set; }

        public int EcmPid { get; set; }

        public int[] EcmPids { get; set; }

        public int Frequency { get; set; }

        public int Guard { get; set; }

        public string H264DecoderDevice { get; set; }

        public int HAlpha { get; set; }

        public int InnerFEC { get; set; }

        public int InnerFECRate { get; set; }

        public string Logo { get; set; }

        public int LPInnerFEC { get; set; }

        public int LPInnerFECRate { get; set; }

        public int Mode { get; set; }

        public int Modulation { get; set; }

        public string MPEG2DecoderDevice { get; set; }

        public string Name { get; set; }

        public int ONID { get; set; }

        public bool OtherFrequencyInUse { get; set; }

        public int OuterFEC { get; set; }

        public int OuterFECRate { get; set; }

        public int PcrPid { get; set; }

        public int PmtPid { get; set; }

        public int ReferenceClock { get; set; }

        public int SID { get; set; }

        public int SymbolRate { get; set; }

        public int TeletextPid { get; set; }

        public int TSID { get; set; }

        public string TunerDevice { get; set; }

        public double VideoAspectRatioFactor { get; set; }

        public int VideoDecoderType { get; set; }

        public bool VideoKeepAspectRatio { get; set; }

        public XMLPointF VideoOffset { get; set; }

        public int VideoPid { get; set; }

        public string VideoRendererDevice { get; set; }

        public double VideoZoom { get; set; }

        public int VideoZoomMode { get; set; }
    }
}
