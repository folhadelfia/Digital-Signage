using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using DirectShowLib;
using MediaFoundation.EVR;

namespace DigitalTV
{
    public class DeviceEnumerator
    {
        public static DsDevice[] GetDevicesWithThisInPin(Guid mainType, Guid subType)
        {
            DsDevice[] devret;
            ArrayList devs = new ArrayList();

            IFilterMapper2 pMapper = (IFilterMapper2)new FilterMapper2();

            Guid[] arrayInTypes = new Guid[2] { mainType, subType };
            IEnumMoniker pEnum = null;
            int hr = pMapper.EnumMatchingFilters(out pEnum,
                    0,                  // Reserved.
                    true,               // Use exact match?
                    (Merit)((int)Merit.DoNotUse + 1), // Minimum merit.
                    true,               // At least one input pin?
                    1,                  // Number of major type/subtype pairs for input.
                    arrayInTypes,       // Array of major type/subtype pairs for input.
                    null,               // Input medium.
                    null,               // Input pin category.
                    false,              // Must be a renderer?
                    true,               // At least one output pin?
                    0,                  // Number of major type/subtype pairs for output.
                    null,               // Array of major type/subtype pairs for output.
                    null,               // Output medium.
                    null);              // Output pin category.

            DsError.ThrowExceptionForHR(hr);

            if (hr >= 0 && pEnum != null)
            {
                try
                {
                    try
                    {
                        // Enumerate the monikers.
                        IMoniker[] pMoniker = new IMoniker[1];
                        while (pEnum.Next(1, pMoniker, IntPtr.Zero) == 0)
                        {
                            try
                            {
                                // The devs array now owns this object.  Don't
                                // release it if we are going to be successfully
                                // returning the devret array
                                devs.Add(new DsDevice(pMoniker[0]));
                            }
                            catch
                            {
                                Marshal.ReleaseComObject(pMoniker[0]);
                                throw;
                            }
                        }
                    }
                    finally
                    {
                        // Clean up.
                        Marshal.ReleaseComObject(pEnum);
                    }


                    // Copy the ArrayList to the DsDevice[]
                    devret = new DsDevice[devs.Count];
                    devs.CopyTo(devret);
                }
                catch
                {
                    foreach (DsDevice d in devs)
                    {
                        d.Dispose();
                    }
                    throw;
                }
            }
            else
            {
                devret = new DsDevice[0];
            }

            Marshal.ReleaseComObject(pMapper);

            return devret;
        }

        public static DsDevice[] GetMPEG2VideoDevices()
        {
            DsDevice[] devret;
            ArrayList devs = new ArrayList();

            IFilterMapper2 pMapper = (IFilterMapper2)new FilterMapper2();

            Guid[] arrayInTypes = new Guid[2] { MediaType.Video, MediaSubType.Mpeg2Video };
            IEnumMoniker pEnum = null;
            int hr = pMapper.EnumMatchingFilters(out pEnum,
                    0,                  // Reserved.
                    true,               // Use exact match?
                    (Merit)((int)Merit.DoNotUse + 1), // Minimum merit.
                    true,               // At least one input pin?
                    1,                  // Number of major type/subtype pairs for input.
                    arrayInTypes,       // Array of major type/subtype pairs for input.
                    null,               // Input medium.
                    null,               // Input pin category.
                    false,              // Must be a renderer?
                    true,               // At least one output pin?
                    0,                  // Number of major type/subtype pairs for output.
                    null,               // Array of major type/subtype pairs for output.
                    null,               // Output medium.
                    null);              // Output pin category.

            DsError.ThrowExceptionForHR(hr);

            if (hr >= 0 && pEnum != null)
            {
                try
                {
                    try
                    {
                        // Enumerate the monikers.
                        IMoniker[] pMoniker = new IMoniker[1];
                        while (pEnum.Next(1, pMoniker, IntPtr.Zero) == 0)
                        {
                            try
                            {
                                // The devs array now owns this object.  Don't
                                // release it if we are going to be successfully
                                // returning the devret array
                                devs.Add(new DsDevice(pMoniker[0]));
                            }
                            catch
                            {
                                Marshal.ReleaseComObject(pMoniker[0]);
                                throw;
                            }
                        }
                    }
                    finally
                    {
                        // Clean up.
                        Marshal.ReleaseComObject(pEnum);
                    }


                    // Copy the ArrayList to the DsDevice[]
                    devret = new DsDevice[devs.Count];
                    devs.CopyTo(devret);
                }
                catch
                {
                    foreach (DsDevice d in devs)
                    {
                        d.Dispose();
                    }
                    throw;
                }
            }
            else
            {
                devret = new DsDevice[0];
            }

            Marshal.ReleaseComObject(pMapper);

            return devret;
        }

        public static DsDevice[] GetH264Devices()
        {
            DsDevice[] devret;
            ArrayList devs = new ArrayList();

            IFilterMapper2 pMapper = (IFilterMapper2)new FilterMapper2();

            Guid[] arrayInTypes = new Guid[2]
				{
					MediaType.Video,
					//new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b)
					new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71) // FOURCC H264
				};
            IEnumMoniker pEnum = null;
            int hr = pMapper.EnumMatchingFilters(out pEnum,
                    0,                  // Reserved.
                    true,               // Use exact match?
                    (Merit)((int)Merit.DoNotUse + 1), // Minimum merit.
                    true,               // At least one input pin?
                    1,                  // Number of major type/subtype pairs for input.
                    arrayInTypes,       // Array of major type/subtype pairs for input.
                    null,               // Input medium.
                    null,               // Input pin category.
                    false,              // Must be a renderer?
                    true,               // At least one output pin?
                    0,                  // Number of major type/subtype pairs for output.
                    null,               // Array of major type/subtype pairs for output.
                    null,               // Output medium.
                    null);              // Output pin category.

            DsError.ThrowExceptionForHR(hr);

            if (hr >= 0 && pEnum != null)
            {
                try
                {
                    try
                    {
                        // Enumerate the monikers.
                        IMoniker[] pMoniker = new IMoniker[1];
                        while (pEnum.Next(1, pMoniker, IntPtr.Zero) == 0)
                        {
                            try
                            {
                                // The devs array now owns this object.  Don't
                                // release it if we are going to be successfully
                                // returning the devret array
                                devs.Add(new DsDevice(pMoniker[0]));
                            }
                            catch
                            {
                                Marshal.ReleaseComObject(pMoniker[0]);
                                throw;
                            }
                        }
                    }
                    finally
                    {
                        // Clean up.
                        Marshal.ReleaseComObject(pEnum);
                    }


                    // Copy the ArrayList to the DsDevice[]
                    devret = new DsDevice[devs.Count];
                    devs.CopyTo(devret);
                }
                catch
                {
                    foreach (DsDevice d in devs)
                    {
                        d.Dispose();
                    }
                    throw;
                }
            }
            else
            {
                devret = new DsDevice[0];
            }

            Marshal.ReleaseComObject(pMapper);

            return devret;
        }

        //public static DsDevice GetVideoCaptureDeviceByName(string deviceName)
        //{
        //    DsDevice devret = null;

        //    IFilterMapper2 pMapper = (IFilterMapper2)new FilterMapper2();

        //    Guid[] arrayInTypes = new Guid[2] { MediaType.Video, MediaSubType.Null };
        //    IEnumMoniker pEnum = null;
        //    int hr = pMapper.EnumMatchingFilters(out pEnum,
        //            0,                  // Reserved.
        //            true,               // Use exact match?
        //            (Merit)((int)Merit.DoNotUse + 1), // Minimum merit.
        //            false,              // At least one input pin?
        //            0,                  // Number of major type/subtype pairs for input.
        //            null,			    // Array of major type/subtype pairs for input.
        //            null,               // Input medium.
        //            null,               // Input pin category.
        //            false,              // Must be a renderer?
        //            true,               // At least one output pin?
        //            0,//1,                  // Number of major type/subtype pairs for output.
        //            null, //arrayInTypes,       // Array of major type/subtype pairs for output.
        //            null,               // Output medium.
        //            null);              // Output pin category.

        //    DsError.ThrowExceptionForHR(hr);

        //    if (hr >= 0 && pEnum != null)
        //    {
        //        try
        //        {
        //            // Enumerate the monikers.
        //            IMoniker[] pMoniker = new IMoniker[1];
        //            while (pEnum.Next(1, pMoniker, IntPtr.Zero) == 0)
        //            {
        //                DsDevice device = new DsDevice(pMoniker[0]);
        //                System.Diagnostics.Trace.WriteLineIf(trace.TraceVerbose, device.Name);
        //                if (deviceName == device.Name)
        //                {
        //                    devret = device;
        //                    break;
        //                }
        //                else
        //                    Marshal.ReleaseComObject(pMoniker[0]);
        //            }
        //        }
        //        finally
        //        {
        //            // Clean up.
        //            Marshal.ReleaseComObject(pEnum);
        //        }
        //    }

        //    Marshal.ReleaseComObject(pMapper);

        //    return devret;
        //}

        //public void FreeMediaType(AMMediaType mt)
        //{
        //    if (mt.formatSize != 0)
        //    {
        //        Marshal.FreeCoTaskMem(mt.formatPtr);
        //        mt.formatSize = 0;
        //        mt.formatPtr = IntPtr.Zero;
        //    }
        //    if (mt.unkPtr != IntPtr.Zero)
        //    {
        //        // Unecessary because pUnk should not be used, but safest.
        //        Marshal.ReleaseComObject(mt.unkPtr);
        //        mt.unkPtr = IntPtr.Zero;
        //    }
        //}

        //public void DeleteMediaType(AMMediaType pmt)
        //{
        //    if (pmt != null)
        //    {
        //        FreeMediaType(pmt);
        //        Marshal.FreeCoTaskMem(pmt);
        //    }
        //}

        private static Hashtable hashtableMediaTypeByGUID = null;

        public static Hashtable MediaTypeByGUID
        {
            get
            {
                if (hashtableMediaTypeByGUID == null)
                {
                    hashtableMediaTypeByGUID = new Hashtable();

                    Type type = typeof(DirectShowLib.MediaType);
                    MemberInfo[] memberInfo = type.GetMembers(BindingFlags.Public | BindingFlags.Static);

                    foreach (MemberInfo mi in memberInfo)
                    {
                        if (mi.MemberType == MemberTypes.Field && ((FieldInfo)mi).FieldType == typeof(Guid))
                        {
                            FieldInfo fieldInfo = type.GetField(mi.Name, BindingFlags.Public | BindingFlags.Static);
                            hashtableMediaTypeByGUID.Add(fieldInfo.GetValue(null), mi.Name);
                        }
                    }
                }

                return hashtableMediaTypeByGUID;
            }
        }

        private static Hashtable hashtableMediaSubTypeByGUID = null;

        public static Hashtable MediaSubTypeByGUID
        {
            get
            {
                if (hashtableMediaSubTypeByGUID == null)
                {
                    hashtableMediaSubTypeByGUID = new Hashtable();

                    Type type = typeof(DirectShowLib.MediaSubType);
                    MemberInfo[] memberInfo = type.GetMembers(BindingFlags.Public | BindingFlags.Static);

                    foreach (MemberInfo mi in memberInfo)
                    {
                        if (mi.MemberType == MemberTypes.Field && ((FieldInfo)mi).FieldType == typeof(Guid))
                        {
                            FieldInfo fieldInfo = type.GetField(mi.Name, BindingFlags.Public | BindingFlags.Static);
                            try
                            {
                                hashtableMediaSubTypeByGUID.Add(fieldInfo.GetValue(null), mi.Name);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }

                return hashtableMediaSubTypeByGUID;
            }
        }

        public static void GetDesinterlaceMode()
        {
        }

        public static void GetDesinterlaceMode(IVMRDeinterlaceControl9 pDeinterlace)
        {
            VMR9VideoDesc VideoDesc = new VMR9VideoDesc();
            int dwNumModes = 0;
            // Fill in the VideoDesc structure (not shown).
            int hr = pDeinterlace.GetNumberOfDeinterlaceModes(ref VideoDesc, ref dwNumModes, null);
            if (hr >= 0 && dwNumModes != 0)
            {
                // Allocate an array for the GUIDs that identify the modes.
                Guid[] pModes = new Guid[dwNumModes];
                if (pModes != null)
                {
                    // Fill the array.
                    hr = pDeinterlace.GetNumberOfDeinterlaceModes(ref VideoDesc, ref dwNumModes, pModes);
                    if (hr >= 0)
                    {
                        // Loop through each item and get the capabilities.
                        for (int i = 0; i < dwNumModes; i++)
                        {
                            VMR9DeinterlaceCaps Caps = new VMR9DeinterlaceCaps();
                            hr = pDeinterlace.GetDeinterlaceModeCaps(pModes[i], ref VideoDesc, ref Caps);
                            if (hr >= 0)
                            {
                                // Examine the Caps structure.
                            }
                        }
                    }
                }
            }
        }
    }

    public enum VideoSizeMode { FromInside, FromOutside, Free, StretchToWindow }
    public enum VideoMode { Normal, TV, Fullscreen };
    public enum VideoRenderer { VMR9, EVR }



    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("A490B1E4-AB84-4D31-A1B2-181E03B1077A"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMFVideoDisplayControl
    {
        //[PreserveSig]
        //int GetNativeVideoSize(
        //    [Out] Size pszVideo,
        //    [Out] Size pszARVideo
        //    );
        [PreserveSig]
        int GetNativeVideoSize(
            out Size pszVideo,
            out Size pszARVideo
            );

        [PreserveSig]
        int GetIdealVideoSize(
            [Out] Size pszMin,
            [Out] Size pszMax
            );

        [PreserveSig]
        int SetVideoPosition(
            [In] MFVideoNormalizedRect pnrcSource,
            [In] MediaFoundation.Misc.MFRect prcDest
            );

        [PreserveSig]
        int GetVideoPosition(
            [Out] MFVideoNormalizedRect pnrcSource,
            [Out] MediaFoundation.Misc.MFRect prcDest
            );

        [PreserveSig]
        int SetAspectRatioMode(
            [In] MFVideoAspectRatioMode dwAspectRatioMode
            );

        [PreserveSig]
        int GetAspectRatioMode(
            out MFVideoAspectRatioMode pdwAspectRatioMode
            );

        [PreserveSig]
        int SetVideoWindow(
            [In] IntPtr hwndVideo
            );

        [PreserveSig]
        int GetVideoWindow(
            out IntPtr phwndVideo
            );

        [PreserveSig]
        int RepaintVideo();

        [PreserveSig]
        int GetCurrentImage(
            [In, Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(BMMarshaler))] BitmapInfoHeader pBih,
            out IntPtr pDib,
            out int pcbDib,
            out long pTimeStamp
            );

        [PreserveSig]
        int SetBorderColor(
            [In] int Clr
            );

        [PreserveSig]
        int GetBorderColor(
            out int pClr
            );

        [PreserveSig]
        int SetRenderingPrefs(
            [In] MFVideoRenderPrefs dwRenderFlags
            );

        [PreserveSig]
        int GetRenderingPrefs(
            out MFVideoRenderPrefs pdwRenderFlags
            );

        [PreserveSig]
        int SetFullscreen(
            [In, MarshalAs(UnmanagedType.Bool)] bool fFullscreen
            );

        [PreserveSig]
        int GetFullscreen(
            [MarshalAs(UnmanagedType.Bool)] out bool pfFullscreen
            );
    }

    // Class to handle BITMAPINFO
    internal class BMMarshaler : ICustomMarshaler
    {
        protected MediaFoundation.Misc.BitmapInfoHeader m_bmi;

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            m_bmi = managedObj as MediaFoundation.Misc.BitmapInfoHeader;

            IntPtr ip = m_bmi.GetPtr();

            return ip;
        }

        // Called just after invoking the COM method.  The IntPtr is the same one that just got returned
        // from MarshalManagedToNative.  The return value is unused.
        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            MediaFoundation.Misc.BitmapInfoHeader bmi = MediaFoundation.Misc.BitmapInfoHeader.PtrToBMI(pNativeData);

            // If we this call is In+Out, the return value is ignored.  If
            // this is out, then m_bmi will be null.
            if (m_bmi != null)
            {
                m_bmi.CopyFrom(bmi);
                bmi = null;
            }

            return bmi;
        }

        public void CleanUpManagedData(object ManagedObj)
        {
            m_bmi = null;
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            Marshal.FreeCoTaskMem(pNativeData);
        }

        // The number of bytes to marshal out - never called
        public int GetNativeDataSize()
        {
            return -1;
        }

        // This method is called by interop to create the custom marshaler.  The (optional)
        // cookie is the value specified in MarshalCookie="asdf", or "" is none is specified.
        public static ICustomMarshaler GetInstance(string cookie)
        {
            return new BMMarshaler();
        }
    }

    public delegate void BDAGraphEventHandler(string message);
}
