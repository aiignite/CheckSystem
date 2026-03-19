using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonUtility.BusLoader
{
    public class Usb2LinEx
    {
        #region 定义函数返回错误代码

        /// <summary>
        /// 函数执行成功
        /// </summary>
        public const int LinExSuccess = 0;

        /// <summary>
        /// 适配器不支持该函数
        /// </summary>
        public const int LinExErrNotSupport = -1;

        /// <summary>
        /// USB写数据失败
        /// </summary>
        public const int LinExErrUsbWriteFail = -2;

        /// <summary>
        /// USB读数据失败
        /// </summary>
        public const int LinExErrUsbReadFail = -3;

        /// <summary>
        /// 命令执行失败
        /// </summary>
        public const int LinExErrCmdFail = -4;

        /// <summary>
        /// 该通道未初始化
        /// </summary>
        public const int LinExErrChNoInit = -5;

        /// <summary>
        /// LIN读数据失败
        /// </summary>
        public const int LinExErrReadData = -6;

        #endregion

        #region LIN和校验模式

        /// <summary>
        /// 标准校验，不含PID
        /// </summary>
        public const byte LinExCheckStd = 0;

        /// <summary>
        /// 增强校验，包含PID
        /// </summary>
        public const byte LinExCheckExt = 1;

        /// <summary>
        /// 自定义校验类型，需要用户自己计算并传入Check，不进行自动校验
        /// </summary>
        public const byte LinExCheckUser = 2;

        /// <summary>
        /// 接收数据校验错误
        /// </summary>
        public const byte LinExCheckNone = 3;

        /// <summary>
        /// 接收数据校验错误
        /// </summary>
        public const byte LinExCheckError = 4;

        #endregion

        #region 定义主从模式

        /// <summary>
        /// 主机
        /// </summary>
        public const byte LinExMaster = 1;

        /// <summary>
        /// 从机
        /// </summary>
        public const byte LinExSlave = 0;

        /// <summary>
        /// 未知类型
        /// </summary>
        public const byte LinExMsgTypeUn = 0;

        /// <summary>
        /// 主机向从机发送数据
        /// </summary>
        public const byte LinExMsgTypeMw = 1;

        /// <summary>
        /// 主机从从机读取数据
        /// </summary>
        public const byte LinExMsgTypeMr = 2;

        /// <summary>
        /// 从机发送数据
        /// </summary>
        public const byte LinExMsgTypeSw = 3;

        /// <summary>
        /// 从机接收数据
        /// </summary>
        public const byte LinExMsgTypeSr = 4;

        /// <summary>
        /// 只发送BREAK信号，若是反馈回来的数据，表明只检测到BREAK信号
        /// </summary>
        public const byte LinExMsgTypeBk = 5;

        /// <summary>
        /// 表明检测到了BREAK，SYNC信号
        /// </summary>
        public const byte LinExMsgTypeSy = 6;

        /// <summary>
        /// 表明检测到了BREAK，SYNC，PID信号
        /// </summary>
        public const byte LinExMsgTypeId = 7;

        /// <summary>
        /// 表明检测到了BREAK，SYNC，PID,DATA信号
        /// </summary>
        public const byte LinExMsgTypeDt = 8;

        /// <summary>
        /// 表明检测到了BREAK，SYNC，PID,DATA,CHECK信号
        /// </summary>
        public const byte LinExMsgTypeCk = 9;

        #endregion

        /// <summary>
        /// LIN数据帧格式定义
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct LinExMsg
        {
            /// <summary>
            /// 时间戳
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public uint Timestamp;

            /// <summary>
            /// 帧类型
            /// </summary>
            [MarshalAs(UnmanagedType.U1)]
            public byte MsgType;

            /// <summary>
            /// 校验类型
            /// </summary>
            [MarshalAs(UnmanagedType.U1)]
            public byte CheckType;

            /// <summary>
            /// LIN数据段有效数据字节数
            /// </summary>
            [MarshalAs(UnmanagedType.U1)]
            public byte DataLen;

            /// <summary>
            /// 固定值
            /// 0x55
            /// </summary>
            [MarshalAs(UnmanagedType.U1)]
            public byte Sync;

            /// <summary>
            /// 帧ID	
            /// </summary>
            [MarshalAs(UnmanagedType.U1)]
            public byte PID;

            /// <summary>
            /// 数据
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U1)]
            public byte[] Data;

            /// <summary>
            /// 校验
            /// 只有校验数据类型为LIN_EX_CHECK_USER的时候才需要用户传入数据
            /// </summary>
            [MarshalAs(UnmanagedType.U1)]
            public byte Check;

            /// <summary>
            /// 该帧的BRAK信号位数，有效值为10到26，若设置为其他值则默认为13位
            /// </summary>
            [MarshalAs(UnmanagedType.U1)]
            public byte BreakBits;

            [MarshalAs(UnmanagedType.U1)]
            public byte Reserve1;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="devHandle"></param>
        /// <param name="linIndex"></param>
        /// <param name="baudRate"></param>
        /// <param name="masterMode"></param>
        /// <returns></returns>
        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_Init(int devHandle, byte linIndex, int baudRate, byte masterMode);

        /// <summary>
        /// 主机模式操作函数
        /// </summary>
        /// <param name="devHandle"></param>
        /// <param name="linIndex"></param>
        /// <param name="pInMsg"></param>
        /// <param name="pOutMsg"></param>
        /// <param name="msgLen"></param>
        /// <returns></returns>
        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_MasterSync(int devHandle, byte linIndex, LinExMsg[] pInMsg, IntPtr pOutMsg, int msgLen);

        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_MasterBreak(int devHandle, byte linIndex);

        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_MasterWrite(int devHandle, byte linIndex, byte pid, byte[] pData, byte dataLen, byte checkType);

        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_MasterRead(int devHandle, byte linIndex, byte pid, byte[] pData);

        /// <summary>
        /// 从机模式操作函数
        /// </summary>
        /// <param name="devHandle"></param>
        /// <param name="linIndex"></param>
        /// <param name="pLinMsg"></param>
        /// <param name="msgLen"></param>
        /// <returns></returns>
        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_SlaveSetIDMode(int devHandle, byte linIndex, LinExMsg[] pLinMsg, int msgLen);

        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_SlaveGetIDMode(int devHandle, byte linIndex, IntPtr pLinMsg);

        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_SlaveGetData(int devHandle, byte linIndex, IntPtr pLinMsg);

        /// <summary>
        /// 电源控制相关函数
        /// </summary>
        /// <param name="devHandle"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_CtrlPowerOut(int devHandle, byte state);

        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_GetVbatValue(int devHandle, short[] pBatValue);

        /// <summary>
        /// 主机模式自动发送数据相关函数
        /// </summary>
        /// <param name="devHandle"></param>
        /// <param name="linIndex"></param>
        /// <param name="pLinMsg"></param>
        /// <param name="msgLen"></param>
        /// <returns></returns>
        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_MasterStartSch(int devHandle, byte linIndex, LinExMsg[] pLinMsg, int msgLen);

        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_MasterStopSch(int devHandle, byte linIndex);

        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_MasterGetSch(int devHandle, byte linIndex, IntPtr pLinMsg);

        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_MasterOfflineSch(int devHandle, byte linIndex, int baudRate, LinExMsg[] pLinMsg,
            int msgLen);

        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_DecodeListFile(string pFileName, byte checkType, int baudRate,
            byte[] pReadDataList, byte readDataListLen, byte[] pCheckTypeList, byte checkTypeListLen);

        [DllImport("USB2XXX.dll")]
        public static extern int LIN_EX_GetListFileMsg(int msgIndex, int msgLen, IntPtr pLinMsg);
    }

    public class UsbDevice
    {
        public struct DeviceInfo
        {
            /// <summary>
            /// 固件名称字符串
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] FirmwareName;

            /// <summary>
            /// 固件编译时间字符串
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] BuildDate;

            /// <summary>
            /// 硬件版本号
            /// </summary>
            public uint HardwareVersion;

            /// <summary>
            /// 固件版本号
            /// </summary>
            public uint FirmwareVersion;

            /// <summary>
            /// 适配器序列号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public uint[] SerialNumber;

            /// <summary>
            /// 适配器当前具备的功能
            /// </summary>
            public uint Functions;
        }

        #region 方法定义
        #endregion

        /// <summary>
        /// 初始化USB设备，并扫描设备连接数，必须调用
        /// </summary>
        /// <param name="pDevHandle">每个设备的设备号存储地址</param>
        /// <returns>扫描到的设备数量</returns>
        [DllImport("USB2XXX.dll")]
        public static extern int USB_ScanDevice(int[] pDevHandle);

        /// <summary>
        /// 打开设备，必须调用
        /// </summary>
        /// <param name="devHandle">设备索引号</param>
        /// <returns>打开设备的状态</returns>
        [DllImport("USB2XXX.dll")]
        public static extern bool USB_OpenDevice(int devHandle);

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <param name="devHandle">设备索引号</param>
        /// <returns>关闭设备的状态</returns>
        [DllImport("USB2XXX.dll")]
        public static extern bool USB_CloseDevice(int devHandle);

        /// <summary>
        /// 获取设备信息，比如设备名称，固件版本号，设备序号，设备功能说明字符串等
        /// </summary>
        /// <param name="devHandle">设备索引号</param>
        /// <param name="pDevInfo">设备信息存储结构体指针</param>
        /// <param name="pFunctionStr">设备功能说明字符串</param>
        /// <returns>获取设备信息的状态</returns>
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_GetDeviceInfo(int devHandle, ref DeviceInfo pDevInfo, StringBuilder pFunctionStr);

        /// <summary>
        /// 擦出用户区数据
        /// </summary>
        /// <param name="devHandle">设备索引号</param>
        /// <returns>用户区数据擦出状态</returns>
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_EraseUserData(int devHandle);

        /// <summary>
        /// 向用户区域写入用户自定义数据，写入数据之前需要调用擦出函数将数据擦出
        /// </summary>
        /// <param name="devHandle">设备索引号</param>
        /// <param name="offsetAddr">数据写入偏移地址，起始地址为0x00，用户区总容量为0x10000字节，也就是64KBye</param>
        /// <param name="pWriteData">用户数据缓冲区首地址</param>
        /// <param name="dataLen">写入用户自定义数据状态</param>
        /// <returns></returns>
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_WriteUserData(int devHandle, int offsetAddr, byte[] pWriteData, int dataLen);

        /// <summary>
        /// 从用户自定义数据区读出数据
        /// </summary>
        /// <param name="devHandle">设备索引号</param>
        /// <param name="offsetAddr">数据写入偏移地址，起始地址为0x00，用户区总容量为0x10000字节，也就是64KBye</param>
        /// <param name="pReadData">用户数据缓冲区首地址</param>
        /// <param name="dataLen">待读出的数据字节数</param>
        /// <returns>读出用户自定义数据的状态</returns>
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_ReadUserData(int devHandle, int offsetAddr, byte[] pReadData, int dataLen);

        /// <summary>
        /// 设置可变电压输出引脚输出电压值
        /// </summary>
        /// <param name="devHandle">设备索引号</param>
        /// <param name="powerLevel">输出电压值，0-1.8V，1-3.3V</param>
        /// <returns>设置输出电压状态</returns>
        [DllImport("USB2XXX.dll")]
        public static extern bool DEV_SetPowerLevel(int devHandle, char powerLevel);
    }

    public class Usb2Can
    {
        /// <summary>
        /// CAN信息帧的数据类型定义
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CanMsg
        {
            /// <summary>
            /// 报文ID
            /// </summary>
            public uint ID;

            /// <summary>
            /// 接收到信息帧时的时间标识
            /// 从CAN控制器初始化开始计时
            /// </summary>
            public uint TimeStamp;

            /// <summary>
            /// 是否是远程帧
            /// </summary>
            public byte RemoteFlag;

            /// <summary>
            /// 是否是扩展帧
            /// </summary>
            public byte ExternFlag;

            /// <summary>
            /// 数据长度小于等于8
            /// 即Data 的长度
            /// </summary>
            public byte DataLen;

            /// <summary>
            /// 报文的数据
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
            public byte[] Data;

            public byte __Res;
        }

        /// <summary>
        /// 初始化CAN的数据类型定义
        /// CAN波特率 = 100MHz/(CAN_BRP)/(CAN_SJW+CAN_BS1+CAN_BS2)
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CanInitConfig
        {
            /// <summary>
            /// 取值范围1~1024
            /// </summary>
            public uint CAN_BRP;

            /// <summary>
            /// 取值范围1~4
            /// </summary>
            public byte CAN_SJW;

            /// <summary>
            /// 取值范围1~16
            /// </summary>
            public byte CAN_BS1;

            /// <summary>
            /// 取值范围1~8
            /// </summary>
            public byte CAN_BS2;

            /// <summary>
            /// CAN工作模式，0-正常模式，1-环回模式，2-静默模式，3-静默环回模式
            /// </summary>
            public byte CAN_Mode;

            /// <summary>
            /// 自动离线管理，0-禁止，1-使能
            /// </summary>
            public byte CAN_ABOM;

            /// <summary>
            /// 报文重发管理，0-使能报文重传，1-禁止报文重传
            /// </summary>
            public byte CAN_NART;

            /// <summary>
            /// FIFO锁定管理，0-新报文覆盖旧报文，1-丢弃新报文
            /// </summary>
            public byte CAN_RFLM;

            /// <summary>
            /// 发送优先级管理，0-标识符决定，1-发送请求顺序决定
            /// </summary>
            public byte CAN_TXFP;
        }

        /// <summary>
        /// CAN 滤波器设置数据类型定义
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CanFilterConfig
        {
            /// <summary>
            /// 使能该过滤器，1-使能，0-禁止
            /// </summary>
            public byte Enable;

            /// <summary>
            /// 过滤器索引号，取值范围为0到13
            /// </summary>
            public byte FilterIndex;

            /// <summary>
            /// 过滤器模式，0-屏蔽位模式，1-标识符列表模式
            /// </summary>
            public byte FilterMode;

            /// <summary>
            /// 过滤的帧类型标志，为1 代表要过滤的为扩展帧，为0 代表要过滤的为标准帧。
            /// </summary>
            public byte ExtFrame;

            /// <summary>
            /// 验收码ID
            /// </summary>
            public uint ID_Std_Ext;

            /// <summary>
            /// 验收码IDE
            /// </summary>
            public uint ID_IDE;

            /// <summary>
            /// 验收码RTR
            /// </summary>
            public uint ID_RTR;

            /// <summary>
            /// 屏蔽码ID，该项只有在过滤器模式为屏蔽位模式时有用
            /// </summary>
            public uint MASK_Std_Ext;

            /// <summary>
            /// 屏蔽码IDE，该项只有在过滤器模式为屏蔽位模式时有用
            /// </summary>
            public uint MASK_IDE;

            /// <summary>
            /// 屏蔽码RTR，该项只有在过滤器模式为屏蔽位模式时有用
            /// </summary>
            public uint MASK_RTR;
        }

        /// <summary>
        /// CAN总线状态数据类型定义
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CanStatus
        {
            /// <summary>
            /// 发送状态寄存器
            /// </summary>
            public uint TSR;

            /// <summary>
            /// 错误状态寄存器
            /// </summary>
            public uint ESR;

            /// <summary>
            /// CAN 控制器接收错误寄存器。
            /// </summary>
            public byte RECounter;

            /// <summary>
            /// CAN 控制器发送错误寄存器。
            /// </summary>
            public byte TECounter;

            /// <summary>
            /// 最后的错误代码
            /// </summary>
            public byte LECode;
        }

        /// <summary>
        /// 定义CAN Bootloader命令列表
        /// Bootloader相关命令
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CblCmdList
        {
            /// <summary>
            /// 擦出APP储存扇区数据
            /// </summary>
            public byte Erase;

            /// <summary>
            /// 设置多字节写数据相关参数（写起始地址，数据量）
            /// </summary>
            public byte WriteInfo;

            /// <summary>
            /// 以多字节形式写数据
            /// </summary>
            public byte Write;

            /// <summary>
            /// 检测节点是否在线，同时返回固件信息
            /// </summary>
            public byte Check;

            /// <summary>
            /// 设置节点波特率
            /// </summary>
            public byte SetBaudRate;

            /// <summary>
            /// 执行固件
            /// </summary>
            public byte Excute;

            //
            /// <summary>
            /// 节点返回状态
            /// 命令执行成功
            /// </summary>
            public byte CmdSuccess;

            /// <summary>
            /// 节点返回状态
            /// 命令执行失败
            /// </summary>
            public byte CmdFaild;
        }

        public const uint CanBlBoot = 0x55555555;
        public const uint CanBlApp = 0xAAAAAAAA;

        #region 函数返回错误代码定义

        /// <summary>
        /// 函数执行成功
        /// </summary>
        public const int CanSuccess = 0;

        /// <summary>
        /// 适配器不支持该函数
        /// </summary>
        public const int CanErrNotSupport = -1;

        /// <summary>
        /// USB写数据失败
        /// </summary>
        public const int CanErrUsbWriteFail = -2;

        /// <summary>
        /// USB读数据失败
        /// </summary>
        public const int CanErrUsbReadFail = -3;

        /// <summary>
        /// 命令执行失败
        /// </summary>
        public const int CanErrCmdFail = -4;

        /// <summary>
        /// 配置设备错误
        /// </summary>
        public const int CanBlErrConfig = -20;

        /// <summary>
        /// 发送数据出错
        /// </summary>
        public const int CanBlErrSend = -21;

        /// <summary>
        /// 超时错误
        /// </summary>
        public const int CanBlErrTimeOut = -22;

        /// <summary>
        /// 执行命令失败
        /// </summary>
        public const int CanBlErrCmd = -23;

        #endregion

        #region USB2CAN相关函数定义

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_Init(int devHandle, byte canIndex, ref CanInitConfig pCanConfig);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_Filter_Init(int devHandle, byte canIndex, ref CanFilterConfig pFilterConfig);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_StartGetMsg(int devHandle, byte canIndex);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_StopGetMsg(int devHandle, byte canIndex);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_SendMsg(int devHandle, byte canIndex, CanMsg[] pCanSendMsg, uint sendMsgNum);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_GetMsg(int devHandle, byte canIndex, IntPtr pCanGetMsg);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_GetMsgWithSize(int devHandle, byte canIndex, IntPtr pCanGetMsg, int bufferSize);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_ClearMsg(int devHandle, byte canIndex);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_GetStatus(int devHandle, byte canIndex, ref CanStatus pCanStatus);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_StartSchedule(int devHandle, byte canIndex, CanMsg[] pCanMsg, uint msgNum);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_StopSchedule(int devHandle, byte canIndex);

        //[DllImport("USB2XXX.dll")]
        //public static extern Int32 CAN_SetRelayData(Int32 DevHandle, CAN_RELAY_HEAD *pCANRelayHead, CAN_RELAY_DATA *pCANRelayData);
        //[DllImport("USB2XXX.dll")]
        //public static extern Int32 CAN_GetRelayData(Int32 DevHandle, CAN_RELAY_HEAD *pCANRelayHead, CAN_RELAY_DATA *pCANRelayData);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_SetRelay(int devHandle, byte relayState);

        //CAN Bootloader相关函数
        [DllImport("USB2XXX.dll")]
        public static extern int CAN_BL_Init(int devHandle, int canIndex, ref CanInitConfig pInitConfig, ref CblCmdList pCmdList);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_BL_NodeCheck(int devHandle, int canIndex, ushort nodeAddr, uint[] pVersion, uint[] pType, uint timeOut);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_BL_Erase(int devHandle, int canIndex, ushort nodeAddr, uint flashSize, uint timeOut);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_BL_Write(int devHandle, int canIndex, ushort nodeAddr, uint addrOffset, byte[] pData, uint dataNum, uint timeOut);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_BL_Excute(int devHandle, int canIndex, ushort nodeAddr, uint type);

        [DllImport("USB2XXX.dll")]
        public static extern int CAN_BL_SetNewBaudRate(int devHandle, int canIndex, ushort nodeAddr, ref CanInitConfig pInitConfig, uint newBaudRate, uint timeOut);

        #endregion

        public class Recvdatathread
        {
            /// <summary>
            /// CAN数据接收事件委托
            /// </summary>
            /// <param name="data"></param>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void RecvCanDataEventHandler(CanBus.CanDataPackage[] data);

            private bool _mBStart;
            private Thread _recvThread;
            private readonly object _locker = new object();
            private int _devHandle;
            private int _canIndex;
            public RecvCanDataEventHandler OnRecvCanDataEvent;

            public event RecvCanDataEventHandler RecvCanData
            {
                add { OnRecvCanDataEvent += value; }
                // ReSharper disable once DelegateSubtraction
                remove { if (OnRecvCanDataEvent != null) OnRecvCanDataEvent -= value; }
            }

            public void SetStart(bool start)
            {
                _mBStart = start;
                if (!start)
                    return;

                var recvAction = RecvDataFunc();
                recvAction.BeginInvoke(RecvEnd, recvAction);

                //if (start)
                //{
                //    _recvThread = new Thread(RecvDataFunc) { IsBackground = true };
                //    _recvThread.Start();
                //}
                //else
                //{
                //    _recvThread.Join();
                //    _recvThread = null;
                //}
            }

            private async void RecvEnd(IAsyncResult ir)
            {
                try
                {
                    if (ir == null)
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

            public void SetDecHandle(int devHandle, int canIndex)
            {
                lock (_locker)
                {
                    _devHandle = devHandle;
                    _canIndex = canIndex;
                }
            }

            protected Action RecvDataFunc()
            {
                return () =>
                {
                    try
                    {
                        lock (_locker)
                        {
                            var canMsgBuffer = new CanMsg[1024];
                            //申请存储数据缓冲区
                            var pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CanMsg)) * canMsgBuffer.Length);
                            var canNum = CAN_GetMsgWithSize(_devHandle, (byte)_canIndex, pt, canMsgBuffer.Length);
                            if (canNum > 0)
                            {
                                var listCanData = new List<CanBus.CanDataPackage>();

                                for (var i = 0; i < canNum; i++)
                                {
                                    //从缓冲区中获取数据
                                    canMsgBuffer[i] =
                                        (CanMsg)
                                        Marshal.PtrToStructure(
                                            (IntPtr)((uint)pt + i * Marshal.SizeOf(typeof(CanMsg))),
                                            typeof(CanMsg));

                                    var canId = canMsgBuffer[i].ID;
                                    var canProtocol = CanBus.CanProtocol.Can;
                                    var canType = CanBus.CanType.Standard;
                                    if (canMsgBuffer[i].ExternFlag == 0x01)
                                        canType = CanBus.CanType.Extended;
                                    var canFormat = CanBus.CanFormat.Data;
                                    if (canMsgBuffer[i].RemoteFlag == 0x01)
                                        canFormat = CanBus.CanFormat.Remote;
                                    var canData = canMsgBuffer[i].Data;

                                    var p = new CanBus.CanDataPackage(canId, canProtocol, canType, canFormat, canData);
                                    listCanData.Add(p);

                                    if (OnRecvCanDataEvent != null && canId != 0xffffffff)
                                        OnRecvCanDataEvent(listCanData.ToArray());
                                }
                            }
                            else if (canNum < 0)
                            {
                                //Console.WriteLine("Get CAN data error!");
                            }
                            //延时
                            Thread.Sleep(20);
                            //释放申请的数据缓冲区
                            Marshal.FreeHGlobal(pt);
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                };
            }

            //protected void RecvDataFunc()
            //{
            //    while (_mBStart)
            //    {
            //        try
            //        {
            //            lock (_locker)
            //            {
            //                var canMsgBuffer = new CanMsg[1024];
            //                //申请存储数据缓冲区
            //                var pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CanMsg)) * canMsgBuffer.Length);
            //                var canNum = CAN_GetMsgWithSize(_devHandle, (byte)_canIndex, pt, canMsgBuffer.Length);
            //                if (canNum > 0)
            //                {
            //                    var listCanData = new List<CanBus.CanDataPackage>();

            //                    for (var i = 0; i < canNum; i++)
            //                    {
            //                        //从缓冲区中获取数据
            //                        canMsgBuffer[i] =
            //                            (CanMsg)
            //                                Marshal.PtrToStructure(
            //                                    (IntPtr)((uint)pt + i * Marshal.SizeOf(typeof(CanMsg))),
            //                                    typeof(CanMsg));

            //                        var canId = canMsgBuffer[i].ID;
            //                        var canProtocol = CanBus.CanProtocol.Can;
            //                        var canType = CanBus.CanType.Standard;
            //                        if (canMsgBuffer[i].ExternFlag == 0x01)
            //                            canType = CanBus.CanType.Extended;
            //                        var canFormat = CanBus.CanFormat.Data;
            //                        if (canMsgBuffer[i].RemoteFlag == 0x01)
            //                            canFormat = CanBus.CanFormat.Remote;
            //                        var canData = canMsgBuffer[i].Data;

            //                        var p = new CanBus.CanDataPackage(canId, canProtocol, canType, canFormat, canData);
            //                        listCanData.Add(p);

            //                        if (OnRecvCanDataEvent != null && canId != 0xffffffff)
            //                            OnRecvCanDataEvent(listCanData.ToArray());
            //                    }
            //                }
            //                else if (canNum < 0)
            //                {
            //                    //Console.WriteLine("Get CAN data error!");
            //                }
            //                //延时
            //                Thread.Sleep(20);
            //                //释放申请的数据缓冲区
            //                Marshal.FreeHGlobal(pt);
            //            }
            //        }
            //        catch (Exception)
            //        {
            //            // ignored
            //        }
            //    }
            //}
        }
    }

    public class Usb2Canfd
    {
        /// <summary>
        /// CANFD信息帧的数据类型定义
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct CanfdMsg
        {
            /// <summary>
            /// 报文ID,bit[30]-RTR,bit[31]-IDE,bit[28..0]-ID
            /// </summary>
            public uint ID;

            /// <summary>
            /// 数据字节长度，可设置为-0,1,2,3,4,5,6,7,8,12,16,20,24,32,48,64
            /// </summary>
            public byte DLC;

            /// <summary>
            /// bit[0]-BRS,bit[1]-ESI,bit[2]-FDF,bit[6..5]-Channel,bit[7]-RXD
            /// </summary>
            public byte Flags;

            /// <summary>
            /// 保留
            /// </summary>
            public byte __Res0;

            /// <summary>
            /// 保留
            /// </summary>
            public byte __Res1;

            /// <summary>
            /// 帧接收或者发送时的时间戳，单位为10us
            /// </summary>
            public uint TimeStamp;

            /// <summary>
            /// 报文的数据
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
            public byte[] Data;
        }

        /// <summary>
        /// CANFD初始化配置数据类型定义
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct CanfdInitConfig
        {
            public byte Mode; //0-正常模式，1-自发自收模式
            public byte ISOCRCEnable;//0-禁止ISO CRC,1-使能ISO CRC
            public byte RetrySend;//0-禁止重发，1-无限制重发
            public byte ResEnable;//0-不接入内部120欧终端电阻，1-接入内部120欧终端电阻
                                  //波特率参数可以用TCANLINPro软件里面的波特率计算工具计算
                                  //仲裁段波特率参数,波特率=40M/NBT_BRP*(1+NBT_SEG1+NBT_SEG2)
            public byte NBT_BRP;
            public byte NBT_SEG1;
            public byte NBT_SEG2;
            public byte NBT_SJW;
            //数据段波特率参数,波特率=40M/DBT_BRP*(1+DBT_SEG1+DBT_SEG2)
            public byte DBT_BRP;
            public byte DBT_SEG1;
            public byte DBT_SEG2;
            public byte DBT_SJW;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
            public byte[] __Res0;//保留
        }

        /// <summary>
        /// CANFD诊断帧信息结构体定义
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct CanfdDiagnostic
        {
            /// <summary>
            /// 标称比特率接收错误计数
            /// </summary>
            public byte NREC;

            /// <summary>
            /// 标称比特率发送错误计数
            /// </summary>
            public byte NTEC;

            /// <summary>
            /// 数据比特率接收错误计数
            /// </summary>
            public byte DREC;

            /// <summary>
            /// 数据比特率发送错误计数
            /// </summary>
            public byte DTEC;

            /// <summary>
            /// 无错误帧计数
            /// </summary>
            public ushort ErrorFreeMsgCount;

            /// <summary>
            /// 参考诊断标志定义
            /// </summary>
            public ushort Flags;
        }

        /// <summary>
        /// CANFD总线错误信息结构体定义
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct CanfdBusError
        {
            /// <summary>
            /// 发送错误计数
            /// </summary>
            public byte TEC;

            /// <summary>
            /// 接收错误计数
            /// </summary>
            public byte REC;

            /// <summary>
            /// 参考总线错误标志定义
            /// </summary>
            public byte Flags;
        }

        /// <summary>
        /// CAN 滤波器设置数据类型定义
        /// </summary>
        public struct CanfdFilterConfig
        {
            /// <summary>
            /// 使能该过滤器，1-使能，0-禁止
            /// </summary>
            public byte Enable;

            /// <summary>
            /// 过滤器索引号，取值范围为0到31
            /// </summary>
            public byte Index;

            /// <summary>
            /// 验收码ID,bit[28..0]为有效ID位，bit[31]为IDE
            /// </summary>
            public uint IdAccept;

            /// <summary>
            /// 屏蔽码，对应bit位若为1，则需要对比对应验收码bit位，相同才接收
            /// </summary>
            public uint IdMask;
        }

        #region 函数返回错误代码定义

        /// <summary>
        /// 函数执行成功
        /// </summary>
        public const int CanfdSuccess = 0;

        /// <summary>
        /// 适配器不支持该函数
        /// </summary>
        public const int CanfdErrNotSupport = -1;

        /// <summary>
        /// USB写数据失败
        /// </summary>
        public const int CanfdErrUsbWriteFail = -2;

        /// <summary>
        /// USB读数据失败
        /// </summary>
        public const int CanfdErrUsbReadFail = -3;

        /// <summary>
        /// 命令执行失败
        /// </summary>
        public const int CanfdErrCmdFail = -4;

        //CANFD_MSG.ID定义
        public const int CanfdMsgFlagRtr = 1 << 30;
        public const int CanfdMsgFlagIde = 1 << 31;
        public const int CanfdMsgFlagIdMask = 0x1FFFFFFF;

        //CANFD_MSG.Flags定义
        public const int CanfdMsgFlagBrs = 1 << 0;
        public const int CanfdMsgFlagEsi = 1 << 1;
        public const int CanfdMsgFlagFdf = 1 << 2;
        public const int CanfdMsgFlagRxd = 1 << 7;

        //CANFD_DIAGNOSTIC.Flags定义
        public const int CanfdDiagnosticFlagNbit0Err = 0x0001;//在发送报文（或应答位、主动错误标志或过载标志）期间，器件要发送显性电平（逻辑值为0的数据或标识符位），但监视的总线值为隐性。
        public const int CanfdDiagnosticFlagNbit1Err = 0x0002;//在发送报文（仲裁字段除外）期间，器件要发送隐性电平（逻辑值为1的位），但监视到的总线值为显性。
        public const int CanfdDiagnosticFlagNackErr = 0x0004;//发送报文未应答。
        public const int CanfdDiagnosticFlagNformErr = 0x0008;//接收报文的固定格式部分格式错误。
        public const int CanfdDiagnosticFlagNstuffErr = 0x0010;//在接收报文的一部分中，序列中包含了5个以上相等位，而报文中不允许出现这种序列。
        public const int CanfdDiagnosticFlagNcrcErr = 0x0020;//接收的报文的CRC校验和不正确。输入报文的CRC与通过接收到的数据计算得到的CRC不匹配。
        public const int CanfdDiagnosticFlagTxboErr = 0x0080;//器件进入离线状态（且自动恢复）。
        public const int CanfdDiagnosticFlagDbit0Err = 0x0100;//见NBIT0_ERR
        public const int CanfdDiagnosticFlagDbit1Err = 0x0200;//见NBIT1_ERR
        public const int CanfdDiagnosticFlagDformErr = 0x0800;//见NFORM_ERR
        public const int CanfdDiagnosticFlagDstuffErr = 0x1000;//见NSTUFF_ERR
        public const int CanfdDiagnosticFlagDcrcErr = 0x2000;//见NCRC_ERR
        public const int CanfdDiagnosticFlagEsiErr = 0x4000;//接收的CAN FD报文的ESI标志置1
        public const int CanfdDiagnosticFlagDlcMismatch = 0x8000;//DLC不匹配,在发送或接收期间，指定的DLC大于FIFO元素的PLSIZE

        //CANFD_BUS_ERROR.Flags定义
        public const int CanfdBusErrorFlagTxRxWarning = 0x01;
        public const int CanfdBusErrorFlagRxWarning = 0x02;
        public const int CanfdBusErrorFlagTxWarning = 0x04;
        public const int CanfdBusErrorFlagRxBusPassive = 0x08;
        public const int CanfdBusErrorFlagTxBusPassive = 0x10;
        public const int CanfdBusErrorFlagTxBusOff = 0x20;

        #endregion

        #region USB2CANFD相关函数定义

        [DllImport("USB2XXX.dll")]
        public static extern int CANFD_Init(int devHandle, byte canIndex, ref CanfdInitConfig pCanConfig);

        [DllImport("USB2XXX.dll")]
        public static extern int CANFD_StartGetMsg(int devHandle, byte canIndex);

        [DllImport("USB2XXX.dll")]
        public static extern int CANFD_StopGetMsg(int devHandle, byte canIndex);

        [DllImport("USB2XXX.dll")]
        public static extern int CANFD_SendMsg(int devHandle, byte canIndex, IntPtr pCanSendMsg, int sendMsgNum);
        [DllImport("USB2XXX.dll")]
        public static extern int CANFD_GetMsg(int devHandle, byte canIndex, IntPtr pCanGetMsg, int bufferSize);

        [DllImport("USB2XXX.dll")]
        public static extern int CANFD_SetFilter(int devHandle, byte canIndex, ref CanfdFilterConfig pCanFilter, byte len);

        [DllImport("USB2XXX.dll")]
        public static extern int CANFD_GetDiagnostic(int devHandle, byte canIndex, ref CanfdDiagnostic pCanDiagnostic);

        [DllImport("USB2XXX.dll")]
        public static extern int CANFD_GetBusError(int devHandle, byte canIndex, ref CanfdBusError pCanBusError);

        [DllImport("USB2XXX.dll")]
        public static extern int CANFD_SetRelay(int devHandle, byte relayState);

        //[DllImport("USB2XXX.dll")]
        //public static extern Int32 CANFD_SetRelayData(Int32 DevHandle,ref CAN_RELAY_HEAD pCANRelayHead,ref CAN_RELAY_DATA pCANRelayData);
        //[DllImport("USB2XXX.dll")]
        //public static extern Int32 CANFD_GetRelayData(Int32 DevHandle,ref CAN_RELAY_HEAD pCANRelayHead,ref CAN_RELAY_DATA pCANRelayData);

        #endregion

        public class Recvdatathread
        {
            /// <summary>
            /// CAN数据接收事件委托
            /// </summary>
            /// <param name="data"></param>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void RecvCanDataEventHandler(CanBus.CanDataPackage[] data);

            private bool _mBStart;
            private Thread _recvThread;
            private readonly object _locker = new object();
            private int _devHandle;
            private int _canIndex;
            public RecvCanDataEventHandler OnRecvCanDataEvent;

            public event RecvCanDataEventHandler RecvCanData
            {
                add { OnRecvCanDataEvent += value; }
                // ReSharper disable once DelegateSubtraction
                remove { if (OnRecvCanDataEvent != null) OnRecvCanDataEvent -= value; }
            }

            public void SetStart(bool start)
            {
                _mBStart = start;
                if (start)
                {
                    _recvThread = new Thread(RecvDataFunc) { IsBackground = true };
                    _recvThread.Start();
                }
                else
                {
                    _recvThread.Join();
                    _recvThread = null;
                }
            }

            public void SetDecHandle(int devHandle, int canIndex)
            {
                lock (_locker)
                {
                    _devHandle = devHandle;
                    _canIndex = canIndex;
                }
            }

            protected void RecvDataFunc()
            {
                while (_mBStart)
                {
                    try
                    {
                        lock (_locker)
                        {
                            #region RECV CANFD

                            {
                                var canMsgBuffer = new CanfdMsg[10240];
                                for (var i = 0; i < canMsgBuffer.Length; i++)
                                    canMsgBuffer[i] = new CanfdMsg { Data = new byte[64] };

                                //申请存储数据缓冲区
                                var pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CanfdMsg)) * canMsgBuffer.Length);//申请缓冲区
                                var canNum = CANFD_GetMsg(_devHandle, (byte)_canIndex, pt, canMsgBuffer.Length);
                                if (canNum > 0)
                                {
                                    var listCanData = new List<CanBus.CanDataPackage>();

                                    for (var i = 0; i < canNum; i++)
                                    {
                                        //从缓冲区中获取数据
                                        canMsgBuffer[i] =
                                            (CanfdMsg)
                                            Marshal.PtrToStructure(
                                                (IntPtr)((uint)pt + i * Marshal.SizeOf(typeof(CanfdMsg))),
                                                typeof(CanfdMsg));

                                        var canId = canMsgBuffer[i].ID & (0x1FFFFFFF);
                                        var canProtocol = CanBus.CanProtocol.Can;

                                        if (canMsgBuffer[i].Flags == 0x04 || canMsgBuffer[i].Flags == 0x05)
                                            canProtocol = CanBus.CanProtocol.CanFd;

                                        //var canProtocol = canMsgBuffer[i].Flags == (byte)0
                                        //    ? CanBus.CanProtocol.Can
                                        //    : CanBus.CanProtocol.CanFd;
                                        var canType = CanBus.IsExtendedCan(canId)
                                            ? CanBus.CanType.Extended
                                            : CanBus.CanType.Standard;//CanBus.CanType.Standard;

                                        var canFormat = CanBus.CanFormat.Data;
                                        //var canType = CanBus.CanType.Standard;
                                        //if (canMsgBuffer[i].ExternFlag == 0x01)
                                        //    canType = CanBus.CanType.Extended;
                                        //var canFormat = CanBus.CanFormat.Data;
                                        //if (canMsgBuffer[i].RemoteFlag == 0x01)
                                        //    canFormat = CanBus.CanFormat.Remote;
                                        var canData = new byte[canMsgBuffer[i].DLC];
                                        for (var j = 0; j < canMsgBuffer[i].DLC; j++)
                                            canData[j] = canMsgBuffer[i].Data[j];

                                        var p = new CanBus.CanDataPackage(canId, canProtocol, canType, canFormat, canData);
                                        listCanData.Add(p);

                                        if (OnRecvCanDataEvent != null)
                                            OnRecvCanDataEvent(listCanData.ToArray());
                                    }
                                }
                                else if (canNum < 0)
                                {
                                    //Console.WriteLine("Get CAN data error!");
                                }
                                //延时
                                Thread.Sleep(20);
                                //释放申请的数据缓冲区
                                Marshal.FreeHGlobal(pt);
                            }

                            #endregion
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }
    }

    public class Usb2Pwm
    {
        //定义函数返回错误代码
        public const int PwmSuccess = 0;   //函数执行成功
        public const int PwmErrNotSupport = -1;  //适配器不支持该函数
        public const int PwmErrUsbWriteFail = -2;  //USB写数据失败
        public const int PwmErrUsbReadFail = -3;  //USB读数据失败
        public const int PwmErrCmdFail = -4;  //命令执行失败

        /// <summary>
        /// 定义初始化PWM的数据类型
        /// </summary>
        public struct PwmConfig
        {
            /// <summary>
            /// 预分频器
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public ushort[] Prescaler;

            /// <summary>
            /// 占空比调节精度
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public ushort[] Precision;

            /// <summary>
            /// 占空比，实际占空比=(Pulse/Precision)*100%
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public ushort[] Pulse;

            /// <summary>
            /// 波形相位，取值0到Precision-1
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public ushort[] Phase;

            /// <summary>
            /// 波形极性
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] Polarity;

            /// <summary>
            /// 通道号
            /// </summary>
            public byte ChannelMask;
        }

        /// <summary>
        /// 定义PWM测量数据
        /// </summary>
        public struct PwmCapData
        {
            /// <summary>
            /// 低电平时间，单位为us
            /// </summary>
            public ushort LowValue;

            /// <summary>
            /// 高电平时间，单位为us
            /// </summary>
            public ushort HighValue;
        }

        //函数定义
        [DllImport("USB2XXX.dll")]
        public static extern int PWM_Init(int devIndex, ref PwmConfig pConfig);

        [DllImport("USB2XXX.dll")]
        public static extern int PWM_Start(int devIndex, byte channelMask, int runTimeUs);

        [DllImport("USB2XXX.dll")]
        public static extern int PWM_Stop(int devIndex, byte channelMask);

        [DllImport("USB2XXX.dll")]
        public static extern int PWM_SetPulse(int devHandle, byte channelMask, ushort[] pPulse);

        [DllImport("USB2XXX.dll")]
        public static extern int PWM_SetPhase(int devHandle, byte channelMask, ushort[] pPhase);

        [DllImport("USB2XXX.dll")]
        public static extern int PWM_SetFrequency(int devHandle, byte channelMask, ushort[] pPrescaler, ushort[] pPrecision);

        [DllImport("USB2XXX.dll")]
        public static extern int PWM_CAP_Init(int devHandle, byte channel);

        [DllImport("USB2XXX.dll")]
        public static extern int PWM_CAP_GetData(int devHandle, byte channel, ref PwmCapData pPwmData);

        [DllImport("USB2XXX.dll")]
        public static extern int PWM_CAP_Stop(int devHandle, byte channel);
    }
}
