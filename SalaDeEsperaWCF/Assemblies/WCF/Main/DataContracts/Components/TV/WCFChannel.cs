using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.DataContracts
{
    [DataContract]
    public class WCFChannel
    {
        [DataMember]
        public string AudioDecoderDevice { get; set; }

        [DataMember]
        public int AudioDecoderType { get; set; }

        [DataMember]
        public int AudioPid { get; set; }

        [DataMember]
        public int[] AudioPids { get; set; }

        [DataMember]
        public string AudioRendererDevice { get; set; }

        [DataMember]
        public int Bandwidth { get; set; }

        [DataMember]
        public string CaptureDevice { get; set; }

        [DataMember]
        public short ChannelNumber { get; set; }

        [DataMember]
        public int EcmPid { get; set; }

        [DataMember]
        public int[] EcmPids { get; set; }

        [DataMember]
        public int Frequency { get; set; }

        [DataMember]
        public int Guard { get; set; }

        [DataMember]
        public string H264DecoderDevice { get; set; }

        [DataMember]
        public int HAlpha { get; set; }

        [DataMember]
        public int InnerFEC { get; set; }

        [DataMember]
        public int InnerFECRate { get; set; }

        [DataMember]
        public string Logo { get; set; }

        [DataMember]
        public int LPInnerFEC { get; set; }

        [DataMember]
        public int LPInnerFECRate { get; set; }

        [DataMember]
        public int Mode { get; set; }

        [DataMember]
        public int Modulation { get; set; }

        [DataMember]
        public string MPEG2DecoderDevice { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int ONID { get; set; }

        [DataMember]
        public bool OtherFrequencyInUse { get; set; }

        [DataMember]
        public int OuterFEC { get; set; }

        [DataMember]
        public int OuterFECRate { get; set; }

        [DataMember]
        public int PcrPid { get; set; }

        [DataMember]
        public int PmtPid { get; set; }

        [DataMember]
        public int ReferenceClock { get; set; }

        [DataMember]
        public int SID { get; set; }

        [DataMember]
        public int SymbolRate { get; set; }

        [DataMember]
        public int TeletextPid { get; set; }

        [DataMember]
        public int TSID { get; set; }

        [DataMember]
        public string TunerDevice { get; set; }

        [DataMember]
        public double VideoAspectRatioFactor { get; set; }

        [DataMember]
        public int VideoDecoderType { get; set; }

        [DataMember]
        public bool VideoKeepAspectRatio { get; set; }

        [DataMember]
        public WCFPointF VideoOffset { get; set; }

        [DataMember]
        public int VideoPid { get; set; }

        [DataMember]
        public string VideoRendererDevice { get; set; }

        [DataMember]
        public double VideoZoom { get; set; }

        [DataMember]
        public int VideoZoomMode { get; set; }
    }
}
