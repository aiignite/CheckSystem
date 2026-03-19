using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;

namespace CheckSystem.OpenCvSharp
{
    public class EnumDevices
    {
        /// <summary>
        /// 枚举视频设备
        /// </summary>
        public static IEnumerable<string> Devices
        {
            get
            {
                var monikers = new IMoniker[5];
                var devEnum = Activator.CreateInstance(Type.GetTypeFromCLSID(SystemDeviceEnum)) as ICreateDevEnum;
                IEnumMoniker moniker;
                if (devEnum != null && devEnum.CreateClassEnumerator(VideoInputDevice, out moniker, 0) == 0)
                {
                    while (true)
                    {
                        var hr = moniker.Next(1, monikers, IntPtr.Zero);
                        if (hr != 0 || monikers[0] == null)
                            break;
                        yield return GetName(monikers[0]);
                        foreach (var i in monikers.Where(i => i != null))
                        {
                            Marshal.ReleaseComObject(i);
                        }
                    }
                    Marshal.ReleaseComObject(moniker);
                }
                if (devEnum != null) Marshal.ReleaseComObject(devEnum);
            }
        }

        /// <summary>
        /// 获取设备名称
        /// </summary>
        /// <param name="moniker"></param>
        /// <returns></returns>
        private static string GetName(IMoniker moniker)
        {
            object temp = null;
            try
            {
                var guid = typeof(IPropertyBag).GUID;
                moniker.BindToStorage(null, null, ref guid, out temp);
                var property = temp as IPropertyBag;

                if (property != null)
                {
                    object value;
                    //var hr = property.Read("FriendlyName", out value, null); //serviceId
                    var hr = property.Read("FriendlyName", out value, null);
                    Marshal.ThrowExceptionForHR(hr);
                    return value as string;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (temp != null)
                {
                    Marshal.ReleaseComObject(temp);
                }
            }
        }

        private static readonly Guid SystemDeviceEnum = new Guid(0x62BE5D10, 0x60EB, 0x11D0, 0xBD, 0x3B, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);
        private static readonly Guid VideoInputDevice = new Guid(0x860BB310, 0x5D01, 0x11D0, 0xBD, 0x3B, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);

        [Flags]
        private enum CDef
        {
            None = 0x0,
            ClassDefault = 0x1,
            BypassClassManager = 0x2,
            ClassLegacy = 0x4,
            MeritAboveDoNotUse = 0x8,
            DevmonCmgrDevice = 0x10,
            DevmonDmo = 0x20,
            DevmonPnpDevice = 0x40,
            DevmonFilter = 0x80,
            DevmonSelectiveMask = 0xF0
        }

        [ComImport]
        [SuppressUnmanagedCodeSecurity]
        [Guid("3127CA40-446E-11CE-8135-00AA004BB851")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IErrorLog
        {
            [PreserveSig]
            int AddError([In][MarshalAs(UnmanagedType.LPWStr)] string pszPropName, [In] System.Runtime.InteropServices.ComTypes.EXCEPINFO pExcepInfo);
        }

        [ComImport]
        [Localizable(false)]
        [SuppressUnmanagedCodeSecurity]
        [Guid("55272A00-42CB-11CE-8135-00AA004BB851")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IPropertyBag
        {
            [PreserveSig]
            int Read([In][MarshalAs(UnmanagedType.LPWStr)] string pszPropName, [MarshalAs(UnmanagedType.Struct)] out object pVar, [In] IErrorLog pErrorLog);

            [PreserveSig]
            int Write([In][MarshalAs(UnmanagedType.LPWStr)] string pszPropName, [In][MarshalAs(UnmanagedType.Struct)] ref object pVar);
        }

        [ComImport]
        [SuppressUnmanagedCodeSecurity]
        [Guid("29840822-5B84-11D0-BD3B-00A0C911CE86")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface ICreateDevEnum
        {
            [PreserveSig]
            int CreateClassEnumerator([In][MarshalAs(UnmanagedType.LPStruct)] Guid pType, out IEnumMoniker ppEnumMoniker, [In] CDef dwFlags);
        }
    }
}
