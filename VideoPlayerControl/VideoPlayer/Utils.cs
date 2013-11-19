using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using DirectShowLib;

namespace VideoPlayer
{
    public class DeviceEnumerator
    {
        public static Guid H264 = new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
        public static Guid MP4 = new Guid("{08E22ADA-B715-45ED-9D20-7B87750301D4}");

        #region TEMP

        public static void SplitterForFile(string file)
        {
            int hr;

            IBaseFilter reader = (IBaseFilter)new AsyncReader();

            hr = (reader as IFileSourceFilter).Load(file, null);

            IPin oPin = DsFindPin.ByDirection(reader, PinDirection.Output, 0);

            IEnumMediaTypes eMedia = null;
            AMMediaType[] mediaTypes = new AMMediaType[1];

            hr = oPin.EnumMediaTypes(out eMedia);

            while(eMedia.Next(mediaTypes.Length, mediaTypes, IntPtr.Zero) == 0)
            {
                GetDevicesWithThisInPin(mediaTypes[0].majorType, mediaTypes[0].subType);
            }
        }

        #endregion


        public static DsDevice[] GetDevicesWithThisInPin(Guid mainType, Guid subType)
        {
            DsDevice[] devret;
            ArrayList devs = new ArrayList();

            IFilterMapper2 pMapper = (IFilterMapper2)new FilterMapper2();

            Guid[] arrayInTypes = new Guid[2] { mainType, subType };
            IEnumMoniker pEnum = null;
            int hr = pMapper.EnumMatchingFilters(out pEnum,
                    1,                  // Reserved.
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

        public static DsDevice[] GetMPEG2Devices()
        {
            #region Old

            //DsDevice[] devret;
            //ArrayList devs = new ArrayList();

            //IFilterMapper2 pMapper = (IFilterMapper2)new FilterMapper2();

            //Guid[] arrayInTypes = new Guid[2] { MediaType.Video, MediaSubType.Mpeg2Video };
            //IEnumMoniker pEnum = null;
            //int hr = pMapper.EnumMatchingFilters(out pEnum,
            //        0,                  // Reserved.
            //        true,               // Use exact match?
            //        (Merit)((int)Merit.DoNotUse + 1), // Minimum merit.
            //        true,               // At least one input pin?
            //        1,                  // Number of major type/subtype pairs for input.
            //        arrayInTypes,       // Array of major type/subtype pairs for input.
            //        null,               // Input medium.
            //        null,               // Input pin category.
            //        false,              // Must be a renderer?
            //        true,               // At least one output pin?
            //        0,                  // Number of major type/subtype pairs for output.
            //        null,               // Array of major type/subtype pairs for output.
            //        null,               // Output medium.
            //        null);              // Output pin category.

            //DsError.ThrowExceptionForHR(hr);

            //if (hr >= 0 && pEnum != null)
            //{
            //    try
            //    {
            //        try
            //        {
            //            // Enumerate the monikers.
            //            IMoniker[] pMoniker = new IMoniker[1];
            //            while (pEnum.Next(1, pMoniker, IntPtr.Zero) == 0)
            //            {
            //                try
            //                {
            //                    // The devs array now owns this object.  Don't
            //                    // release it if we are going to be successfully
            //                    // returning the devret array
            //                    devs.Add(new DsDevice(pMoniker[0]));
            //                }
            //                catch
            //                {
            //                    Marshal.ReleaseComObject(pMoniker[0]);
            //                    throw;
            //                }
            //            }
            //        }
            //        finally
            //        {
            //            // Clean up.
            //            Marshal.ReleaseComObject(pEnum);
            //        }


            //        // Copy the ArrayList to the DsDevice[]
            //        devret = new DsDevice[devs.Count];
            //        devs.CopyTo(devret);
            //    }
            //    catch
            //    {
            //        foreach (DsDevice d in devs)
            //        {
            //            d.Dispose();
            //        }
            //        throw;
            //    }
            //}
            //else
            //{
            //    devret = new DsDevice[0];
            //}

            //Marshal.ReleaseComObject(pMapper);

            //return devret;

            #endregion

            return GetDevicesWithThisInPin(MediaType.Video, MediaSubType.Mpeg2Video);
        }

        public static DsDevice[] GetH264Devices()
        {
            #region Old

            //DsDevice[] devret;
            //ArrayList devs = new ArrayList();

            //IFilterMapper2 pMapper = (IFilterMapper2)new FilterMapper2();

            //Guid[] arrayInTypes = new Guid[2]
            //    {
            //        MediaType.Video,
            //        //new Guid(0x8d2d71cb, 0x243f, 0x45e3, 0xb2, 0xd8, 0x5f, 0xd7, 0x96, 0x7e, 0xc0, 0x9b)
            //        new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71) // FOURCC H264
            //    };
            //IEnumMoniker pEnum = null;
            //int hr = pMapper.EnumMatchingFilters(out pEnum,
            //        0,                  // Reserved.
            //        true,               // Use exact match?
            //        (Merit)((int)Merit.DoNotUse + 1), // Minimum merit.
            //        true,               // At least one input pin?
            //        1,                  // Number of major type/subtype pairs for input.
            //        arrayInTypes,       // Array of major type/subtype pairs for input.
            //        null,               // Input medium.
            //        null,               // Input pin category.
            //        false,              // Must be a renderer?
            //        true,               // At least one output pin?
            //        0,                  // Number of major type/subtype pairs for output.
            //        null,               // Array of major type/subtype pairs for output.
            //        null,               // Output medium.
            //        null);              // Output pin category.

            //DsError.ThrowExceptionForHR(hr);

            //if (hr >= 0 && pEnum != null)
            //{
            //    try
            //    {
            //        try
            //        {
            //            // Enumerate the monikers.
            //            IMoniker[] pMoniker = new IMoniker[1];
            //            while (pEnum.Next(1, pMoniker, IntPtr.Zero) == 0)
            //            {
            //                try
            //                {
            //                    // The devs array now owns this object.  Don't
            //                    // release it if we are going to be successfully
            //                    // returning the devret array
            //                    devs.Add(new DsDevice(pMoniker[0]));
            //                }
            //                catch
            //                {
            //                    Marshal.ReleaseComObject(pMoniker[0]);
            //                    throw;
            //                }
            //            }
            //        }
            //        finally
            //        {
            //            // Clean up.
            //            Marshal.ReleaseComObject(pEnum);
            //        }


            //        // Copy the ArrayList to the DsDevice[]
            //        devret = new DsDevice[devs.Count];
            //        devs.CopyTo(devret);
            //    }
            //    catch
            //    {
            //        foreach (DsDevice d in devs)
            //        {
            //            d.Dispose();
            //        }
            //        throw;
            //    }
            //}
            //else
            //{
            //    devret = new DsDevice[0];
            //}

            //Marshal.ReleaseComObject(pMapper);

            //return devret;

            #endregion

            return GetDevicesWithThisInPin(MediaType.Video, H264);
        }

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
}
