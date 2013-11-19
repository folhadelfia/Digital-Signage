using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using DirectShowLib;
using DirectShowLib.BDA;
using System.Drawing;

namespace DigitalTV
{
    public enum AM_MPEG2Profile
    {
        Simple = 1,
        Main,
        SNRScalable,
        SpatiallyScalable,
        High
    }

    public enum AM_MPEG2Level
    {
        Low = 1,
        Main,
        High1440,
        High
    }

    [Flags]
    public enum AMMPEG2
    {
        DoPanScan = 0x00000001,
        DVDLine21Field1 = 0x00000002,
        DVDLine21Field2 = 0x00000004,
        SourceIsLetterboxed = 0x00000008,
        FilmCameraMode = 0x00000010,
        LetterboxAnalogOut = 0x00000020,
        DSS_UserData = 0x00000040,
        DVB_UserData = 0x00000080,
        Timebase27Mhz = 0x00000100,
        WidescreenAnalogOut = 0x00000200
    }

    /// <summary>
    /// From MPEG2VIDEOINFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class MPEG2VideoInfo
    {
        public VideoInfoHeader2 hdr;
        public uint StartTimeCode;
        public uint SequenceHeader;
        public AM_MPEG2Profile Profile;
        public AM_MPEG2Level Level;
        public AMMPEG2 Flags;
    }
}
