using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CommonUtility.BusLoader
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Zcan
    {
        public uint acc_code;
        public uint acc_mask;
        public uint reserved;
        public byte filter;
        public byte timing0;
        public byte timing1;
        public byte mode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Canfd
    {
        public uint acc_code;
        public uint acc_mask;
        public uint abit_timing;
        public uint dbit_timing;
        public uint brp;
        public byte filter;
        public byte mode;
        public ushort pad;
        public uint reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CanFrame
    {
        /// <summary>
        /// 帧 ID，32 位，高 3 位属于标志位，标志位含义如下：
        /// 第 31 位(最高位)代表扩展帧标志，=0 表示标准帧，=1 代表扩展帧，宏 IS_EFF 可获取该标志；
        /// 第 30 位代表远程帧标志，=0 表示数据帧，=1 表示远程帧，宏 IS_RTR 可获取该标志；
        /// 第 29 位代表错误帧标准，=0 表示 CAN 帧，=1 表示错误帧，目前只能设置为 0；
        /// 其余位代表实际帧 ID 值，使用宏 MAKE_CAN_ID 构造 ID，使用宏 GET_ID 获取 ID。
        /// </summary>
        public uint can_id;  /* 32 bit MAKE_CAN_ID + EFF/RTR/ERR flags */

        /// <summary>
        /// 数据长度
        /// </summary>
        public byte can_dlc; /* frame payload length in byte (0 .. CAN_MAX_DLEN) */

        /// <summary>
        /// 对齐，忽略
        /// </summary>
        public byte __pad;   /* padding */

        /// <summary>
        /// 仅作保留，不设置
        /// </summary>
        public byte __res0;  /* reserved / padding */

        /// <summary>
        /// 仅作保留，不设置
        /// </summary>
        public byte __res1;  /* reserved / padding */

        /// <summary>
        /// 报文数据，有效长度为 can_dlc
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] data/* __attribute__((aligned(8)))*/;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CanfdFrame
    {
        /// <summary>
        /// 帧ID
        /// </summary>
        public uint can_id;  /* 32 bit MAKE_CAN_ID + EFF/RTR/ERR flags */

        /// <summary>
        /// 数据长度
        /// </summary>
        public byte len;     /* frame payload length in byte */

        /// <summary>
        /// 额外标志，比如使用 CANFD 加速，则设置为宏 CANFD_BRS
        /// </summary>
        public byte flags;   /* additional flags for CAN FD,i.e error code */

        /// <summary>
        /// 仅作保留，不设置
        /// </summary>
        public byte __res0;  /* reserved / padding */

        /// <summary>
        /// 仅作保留，不设置
        /// </summary>
        public byte __res1;  /* reserved / padding */

        /// <summary>
        /// 报文数据，有效长度为 len
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] data/* __attribute__((aligned(8)))*/;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ZCANDataObj
    {
        public byte dataType;                                       // 1-CAN/CANFD数据, 4-LIN数据
        public byte chnl;                                           // 数据通道 
        public UInt16 flag;                                         // 未使用
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] extraData;                                    // 未使用  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 92)]
        public byte[] data;                                    // 报文结构体
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ZcanChannelInitConfig
    {
        [FieldOffset(0)]
        public uint can_type; //type:TYPE_CAN TYPE_CANFD

        [FieldOffset(4)]
        public Zcan can;

        [FieldOffset(4)]
        public Canfd canfd;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ZcanTransmitData
    {
        /// <summary>
        /// 报文数据信息，
        /// </summary>
        public CanFrame frame;

        /// <summary>
        /// 发送方式，0=正常发送，1=单次发送，2=自发自收，3=单次自发自收
        /// </summary>
        public uint transmit_type;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ZcanReceiveData
    {
        /// <summary>
        /// 报文数据信息
        /// </summary>
        public CanFrame frame;

        /// <summary>
        /// 时间戳，单位微秒，基于设备启动时间。（如果为云设备，则基于 1970 年 1 月 1 日 0时 0 分 0 秒）
        /// </summary>
        public ulong timestamp;//us
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ZcanTransmitFdData
    {
        /// <summary>
        /// /报文数据信息
        /// </summary>
        public CanfdFrame frame;

        /// <summary>
        /// 发送方式，0=正常发送，1=单次发送，2=自发自收，3=单次自发自收
        /// </summary>
        public uint transmit_type;
    }

    public struct DeviceInfo
    {
        public uint DeviceType;  //设备类型
        public uint ChannelCount;//设备的通道个数
        public DeviceInfo(uint type, uint count)
        {
            DeviceType = type;
            ChannelCount = count;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ZcanReceiveFdData
    {
        public CanfdFrame frame;
        public ulong timestamp;//us
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ZcanChannelErrorInfo
    {
        public uint error_code;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] passive_ErrData;
        public byte arLost_ErrData;
    }

    /// <summary>
    /// for zlg cloud
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ZcloudDevinfo
    {
        public int devIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] owner;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] model;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] fwVer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] hwVer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] serial;
        public byte canNum;
        public int status;             // 0:online, 1:offline
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] bCanUploads;   // each channel enable can upload
        public byte bGpsUpload;
    }

    //[StructLayout(LayoutKind.Sequential)]
    //public struct ZCLOUD_DEV_GROUP_INFO
    //{
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    //    public char[] groupName;
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    //    public char[] desc;
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    //    public char[] groupId;
    //    //public ZCLOUD_DEVINFO *pDevices;
    //    public IntPtr pDevices;
    //    public uint devSize;
    //};

    [StructLayout(LayoutKind.Sequential)]
    public struct ZcloudUserData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] username;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] mobile;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        // public char[] email;
        // public IntPtr pDevGroups;
        // public uint devGroupSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public ZcloudDevinfo[] devices;
        public uint devCnt;
    }

    public class Define
    {
        public const int TypeCan = 0;
        public const int TypeCanfd = 1;

        public const int ZcanUsbcan1 = 3;
        public const int ZcanUsbcan2 = 4;
        public const int ZcanCanetudp = 12;
        public const int ZcanCanettcp = 17;
        public const int ZcanUsbcanEu = 20;
        public const int ZcanUsbcan2Eu = 21;
        public const int ZcanPciecanfd100U = 38;
        public const int ZcanPciecanfd200U = 39;
        public const int ZcanPciecanfd400U = 40;
        public const int ZcanUsbcanfd200U = 41;
        public const int ZcanUsbcanfd100U = 42;
        public const int ZcanUsbcanfdMini = 43;
        public const int ZcanCloud = 46;
        public const int ZcanUsbcanfd400U = 76;

        public const int StatusErr = 0;
        public const int StatusOk = 1;
    }

    public class Method
    {
        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr ZCAN_OpenDevice(uint deviceType, uint deviceIndex, uint reserved);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_CloseDevice(IntPtr deviceHandle);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        // pInitConfig -> ZCAN_CHANNEL_INIT_CONFIG
        public static extern IntPtr ZCAN_InitCAN(IntPtr deviceHandle, uint canIndex, IntPtr pInitConfig);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_StartCAN(IntPtr channelHandle);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_ResetCAN(IntPtr channelHandle);

        /// <summary>
        /// 该函数用于发送 CAN 报文
        /// </summary>
        /// <param name="channelHandle">通道句柄值</param>
        /// <param name="pTransmit">结构体ZCAN_Transmit_Data 数组的首指针</param>
        /// <param name="len">报文数目</param>
        /// <returns>返回实际发送成功的报文数目</returns>
        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        // pTransmit -> ZCAN_Transmit_Data
        public static extern uint ZCAN_Transmit(IntPtr channelHandle, IntPtr pTransmit, uint len);

        /// <summary>
        /// 该函数用于发送 CANFD 报文
        /// </summary>
        /// <param name="channelHandle">通道句柄值</param>
        /// <param name="pTransmit">结构体ZCAN_TransmitFD_Data 数组的首指针</param>
        /// <param name="len">报文数目</param>
        /// <returns>返回实际发送成功的报文数目</returns>
        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        // pTransmit -> ZCAN_TransmitFD_Data
        public static extern uint ZCAN_TransmitFD(IntPtr channelHandle, IntPtr pTransmit, uint len);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_GetReceiveNum(IntPtr channelHandle, byte type);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_Receive(IntPtr channelHandle, IntPtr data, uint len, int waitTime = -1);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_ReceiveFD(IntPtr channelHandle, IntPtr data, uint len, int waitTime = -1);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        // pErrInfo -> ZCAN_CHANNEL_ERROR_INFO
        public static extern uint ZCAN_ReadChannelErrInfo(IntPtr channelHandle, IntPtr pErrInfo);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetIProperty(IntPtr deviceHandle);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool ZCLOUD_IsConnected();

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void ZCLOUD_SetServerInfo(string httpAddr, ushort httpPort,
            string mqttAddr, ushort mqttPort);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCLOUD_ConnectServer(string username, string password);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCLOUD_DisconnectServer();

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr ZCLOUD_GetUserData();
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetValueFunc(string path, string value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate string GetValueFunc(string path, string value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetPropertysFunc(string path, string value);

    public struct Property
    {
        public SetValueFunc SetValue;
        public GetValueFunc GetValue;
        public GetPropertysFunc GetPropertys;
    }

    /// <summary>
    /// 接收数据线程类
    /// </summary>
    public class Recvdatathread
    {
        /// <summary>
        /// CAN数据接收事件委托
        /// </summary>
        /// <param name="data"></param>
        /// <param name="len"></param>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RecvCanDataEventHandler(ZcanReceiveData[] data, uint len);

        /// <summary>
        /// CANFD数据接收事件委托
        /// </summary>
        /// <param name="data"></param>
        /// <param name="len"></param>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RecvFdDataEventHandler(ZcanReceiveFdData[] data, uint len);

        public const int TypeCan = 0;
        private const int TypeCanfd = 1;

        private bool _mBStart;
        private IntPtr _channelHandle;
        private readonly object _locker = new object();
        public RecvCanDataEventHandler OnRecvCanDataEvent;
        public RecvFdDataEventHandler OnRecvFdDataEvent;

        public event RecvCanDataEventHandler RecvCanData
        {
            add { OnRecvCanDataEvent += value; }
            // ReSharper disable once DelegateSubtraction
            remove { if (OnRecvCanDataEvent != null) OnRecvCanDataEvent -= value; }
        }

        public event RecvFdDataEventHandler RecvFdData
        {
            add { OnRecvFdDataEvent += value; }
            // ReSharper disable once DelegateSubtraction
            remove { if (OnRecvFdDataEvent != null) OnRecvFdDataEvent -= value; }
        }

        public void SetStart(bool start)
        {
            _mBStart = start;
            if (!start) return;
            var recvAction = RecvDataAction();
            recvAction.BeginInvoke(RecvEnd, recvAction);
        }

        private async void RecvEnd(IAsyncResult ir)
        {
            try
            {
                if (ir == null)
                    return;

                if (!ir.IsCompleted)
                    return;

                var recvAction = ir.AsyncState as Action;

                if (recvAction == null) return;
                if (!_mBStart) return;
                recvAction.EndInvoke(ir);
                await Task.Delay(1);
                recvAction.BeginInvoke(RecvEnd, recvAction);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void SetChannelHandle(IntPtr channelHandle)
        {
            lock (_locker)
            {
                _channelHandle = channelHandle;
            }
        }

        /// <summary>
        /// 数据接收函数
        /// </summary>
        /// <returns></returns>
        private Action RecvDataAction()
        {
            return () =>
            {
                try
                {
                    lock (_locker)
                    {
                        var len = Method.ZCAN_GetReceiveNum(_channelHandle, TypeCan);
                        if (len > 0)
                        {
                            var size = Marshal.SizeOf(typeof(ZcanReceiveData));
                            var ptr = Marshal.AllocHGlobal((int)len * size);
                            len = Method.ZCAN_Receive(_channelHandle, ptr, len);
                            var canData = new ZcanReceiveData[len];
                            for (var i = 0; i < len; ++i)
                            {
                                canData[i] = (ZcanReceiveData)Marshal.PtrToStructure(
                                    (IntPtr)((uint)ptr + i * size), typeof(ZcanReceiveData));
                            }
                            if (OnRecvCanDataEvent != null)
                                OnRecvCanDataEvent(canData, len);

                            Marshal.FreeHGlobal(ptr);
                        }

                        len = Method.ZCAN_GetReceiveNum(_channelHandle, TypeCanfd);
                        if (len > 0)
                        {
                            var size = Marshal.SizeOf(typeof(ZcanReceiveFdData));
                            var ptr = Marshal.AllocHGlobal((int)len * size);
                            len = Method.ZCAN_ReceiveFD(_channelHandle, ptr, len);
                            var canfdData = new ZcanReceiveFdData[len];
                            for (var i = 0; i < len; ++i)
                            {
                                canfdData[i] = (ZcanReceiveFdData)Marshal.PtrToStructure(
                                    (IntPtr)((uint)ptr + i * size), typeof(ZcanReceiveFdData));
                            }
                            if (OnRecvFdDataEvent != null)
                                OnRecvFdDataEvent(canfdData, len);

                            Marshal.FreeHGlobal(ptr);
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            };
        }
    }
}
