using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Controller
{
    public sealed class ToomossUsb2XxxCanLin : ControllerBase
    {
        public CanBus Can1;
        public CanBus Can2;
        public LinBus Lin1;
        public LinBus Lin2;

        public byte CheckType = 1;

        private readonly UsbDevice.DeviceInfo _devInfo;
        private readonly int[] _devHandles = new int[20];
        private readonly int _devHandle;
        public readonly bool State;

        private readonly Usb2Can.Recvdatathread _can0RecvDataThread;
        private readonly Usb2Can.Recvdatathread _can1RecvDataThread;
        private readonly Usb2Canfd.Recvdatathread _canfd0RecvDataThread;
        private readonly Usb2Canfd.Recvdatathread _canfd1RecvDataThread;
        private readonly bool _isCanFd;

        public ToomossUsb2XxxCanLin(string name)
            : base(name)
        {
            //扫描查找设备
            var devNum = UsbDevice.USB_ScanDevice(_devHandles);
            if (devNum <= 0)
            {
                Console.WriteLine(@"No device connected!");
                return;
            }
            else
            {
                Console.WriteLine(@"Have {0} device connected!", devNum);
            }
            _devHandle = _devHandles[0];
            //打开设备
            State = UsbDevice.USB_OpenDevice(_devHandle);
            if (!State)
            {
                Console.WriteLine(@"Open device error!");
                return;
            }
            else
            {
                Console.WriteLine(@"Open device success!");
            }
            // 获取固件信息
            var funcStr = new StringBuilder(256);
            State = UsbDevice.DEV_GetDeviceInfo(_devHandle, ref _devInfo, funcStr);
            if (!State)
            {
                Console.WriteLine(@"Get device infomation error!");
                return;
            }

            Console.WriteLine(@"Firmware Info:");
            Console.WriteLine(@"    Name:" + Encoding.Default.GetString(_devInfo.FirmwareName));
            Console.WriteLine(@"    Build Date:" + Encoding.Default.GetString(_devInfo.BuildDate));
            Console.WriteLine(@"    Firmware Version:v{0}.{1}.{2}", (_devInfo.FirmwareVersion >> 24) & 0xFF, (_devInfo.FirmwareVersion >> 16) & 0xFF, _devInfo.FirmwareVersion & 0xFFFF);
            Console.WriteLine(@"    Hardware Version:v{0}.{1}.{2}", (_devInfo.HardwareVersion >> 24) & 0xFF, (_devInfo.HardwareVersion >> 16) & 0xFF, _devInfo.HardwareVersion & 0xFFFF);
            Console.WriteLine(@"    Functions:" + _devInfo.Functions.ToString("X8"));
            Console.WriteLine(@"    Functions String:" + funcStr);

            // 初始化配置LIN1
            var lin1Ret = Usb2LinEx.LIN_EX_Init(_devHandle, 0, 19200, 1);//初始化为主机
            if (lin1Ret == Usb2LinEx.LinExSuccess)
                Lin1 = new ToomossUsbLinChannel(this, name + "_lin1", 0);

            // 初始化配置LIN2
            var linwRet = Usb2LinEx.LIN_EX_Init(_devHandle, 1, 19200, 1);//初始化为主机
            if (linwRet == Usb2LinEx.LinExSuccess)
                Lin2 = new ToomossUsbLinChannel(this, name + "_lin2", 1);

            // 初始化配置CAN1
            var canConfig = new Usb2Can.CanInitConfig
            {
                CAN_Mode = 0, //正常模式
                CAN_ABOM = 0, //禁止自动离线
                CAN_NART = 1, //禁止报文重传
                CAN_RFLM = 0, //FIFO满之后覆盖旧报文
                CAN_TXFP = 1, //发送请求决定发送顺序
                //配置波特率,波特率 = 100M/(BRP*(SJW+BS1+BS2))
                CAN_BRP = 4,
                CAN_BS1 = 15,
                CAN_BS2 = 5,
                CAN_SJW = 2
            };

            var canfdConfig = new Usb2Canfd.CanfdInitConfig
            {
                Mode = 0,
                RetrySend = 1,
                ISOCRCEnable = 1,
                ResEnable = 1,
                NBT_BRP = 1,
                NBT_SEG1 = 59,
                NBT_SEG2 = 20,
                NBT_SJW = 2,
                DBT_BRP = 1,
                DBT_SEG1 = 14,
                DBT_SEG2 = 5,
                DBT_SJW = 2
            };

            //0-正常模式，1-自发自收模式
            //使能自动重传
            //使能ISOCRC
            //使能内部终端电阻（若总线上没有终端电阻，则必须使能终端电阻才能正常传输数据）
            //波特率参数可以用TCANLINPro软件里面的波特率计算工具计算
            //仲裁段波特率参数,波特率=40M/NBT_BRP*(1+NBT_SEG1+NBT_SEG2)
            //数据域波特率参数,波特率=40M/DBT_BRP*(1+DBT_SEG1+DBT_SEG2)

            if (funcStr.ToString().Contains("CANFD"))
            {
                _isCanFd = true;

                var can1Ret = Usb2Canfd.CANFD_Init(_devHandle, 0, ref canfdConfig);
                if (can1Ret == Usb2Canfd.CanfdSuccess)
                {
                    Can1 = new ToomossUsbCanChannel(this, name + @"_canfd1", 1);

                    Usb2Canfd.CANFD_StartGetMsg(_devHandle, 1);
                    _canfd0RecvDataThread = new Usb2Canfd.Recvdatathread();
                    _canfd0RecvDataThread.SetDecHandle(_devHandle, 0);
                    _canfd0RecvDataThread.SetStart(true);
                    _canfd0RecvDataThread.RecvCanData += AddCan0Data;
                }

                var can2Ret = Usb2Canfd.CANFD_Init(_devHandle, 1, ref canfdConfig);
                if (can2Ret == Usb2Canfd.CanfdSuccess)
                {

                    Can2 = new ToomossUsbCanChannel(this, name + @"_canfd2", 2);

                    Usb2Canfd.CANFD_StartGetMsg(_devHandle, 2);
                    _canfd1RecvDataThread = new Usb2Canfd.Recvdatathread();
                    _canfd1RecvDataThread.SetDecHandle(_devHandle, 1);
                    _canfd1RecvDataThread.SetStart(true);
                    _canfd1RecvDataThread.RecvCanData += AddCan1Data;
                }
            }
            else
            {
                var can1Ret = Usb2Can.CAN_Init(_devHandle, 0, ref canConfig);
                if (can1Ret == Usb2Can.CanSuccess)
                {
                    Can1 = new ToomossUsbCanChannel(this, name + "_can1", 1);

                    _can0RecvDataThread = new Usb2Can.Recvdatathread();
                    _can0RecvDataThread.SetDecHandle(_devHandle, 0);
                    _can0RecvDataThread.SetStart(true);
                    _can0RecvDataThread.RecvCanData += AddCan0Data;
                }

                var can2Ret = Usb2Can.CAN_Init(_devHandle, 1, ref canConfig);
                if (can2Ret == Usb2Can.CanSuccess)
                {
                    Can2 = new ToomossUsbCanChannel(this, name + "_can2", 2);

                    _can1RecvDataThread = new Usb2Can.Recvdatathread();
                    _can1RecvDataThread.SetDecHandle(_devHandle, 1);
                    _can1RecvDataThread.SetStart(true);
                    _can1RecvDataThread.RecvCanData += AddCan1Data;
                }
            }
        }

        /// <summary>
        /// 设置对应通道的波特率
        /// </summary>
        /// <param name="linIndex">1或者2</param>
        /// <param name="baudRate">9600,10417,10400,19200,14400,56000......</param>
        public void SetBaudRate(string linIndex, string baudRate)
        {
            //if (baudRate != "9600" && baudRate != "10417" && baudRate != "10400" &&
            //    baudRate != "14400" && baudRate != "56000" &&
            //    baudRate != "19200")
            //    return;

            if (linIndex != "1" && linIndex != "2")
                return;

            if (linIndex == "1")
            {
                // 初始化配置LIN1
                var lin1Ret = Usb2LinEx.LIN_EX_Init(_devHandle, 0, int.Parse(baudRate), 1);//初始化为主机
                if (lin1Ret == Usb2LinEx.LinExSuccess)
                {
                    Lin1 = new ToomossUsbLinChannel(this, Name + "_lin", 0);
                }
            }
            else if (linIndex == "2")
            {
                // 初始化配置LIN2
                var linwRet = Usb2LinEx.LIN_EX_Init(_devHandle, 1, int.Parse(baudRate), 1);//初始化为主机
                if (linwRet == Usb2LinEx.LinExSuccess)
                {
                    Lin2 = new ToomossUsbLinChannel(this, Name + "_lin", 1);
                }
            }
        }

        private bool SendMasterLin(byte linId, byte[] linData, int linChIndex)
        {
            if (linData == null)
                return false;

            //var ret = USB2LIN_EX.LIN_EX_MasterWrite(
            //    DevHandle,
            //    (byte)linChIndex,
            //    LinBus.ConvertLinId(linId),
            //    linData,
            //    (byte)linData.Length, 1);

            //var st = new Stopwatch();
            //st.Start();

            var ret = Usb2LinEx.LIN_EX_MasterWrite(
                _devHandle,
                (byte)linChIndex,
                linId,
                linData,
                (byte)linData.Length,
                linId == 0x3C ? (byte)0x00 : CheckType);

            //st.Stop();
            //Console.WriteLine(st.ElapsedMilliseconds);

            return ret == Usb2LinEx.LinExSuccess;
        }

        private bool SendSlaveLin(byte linId, int linChIndex, out byte[] linMsg)
        {
            linMsg = new byte[8];
            var ret = Usb2LinEx.LIN_EX_MasterRead(_devHandle, (byte)linChIndex, linId, linMsg);

            return ret > Usb2LinEx.LinExSuccess;
        }

        private void SendCanData(int canIndex, IReadOnlyList<CanBus.CanDataPackage> dataPackages)
        {
            if (dataPackages != null)
            {
                if (!_isCanFd)
                {
                    var canMsg = new Usb2Can.CanMsg[dataPackages.Count];
                    for (var i = 0; i < dataPackages.Count; i++)
                    {
                        var p = dataPackages[i];

                        canMsg[i] = new Usb2Can.CanMsg
                        {
                            ExternFlag = p.CanType == CanBus.CanType.Standard ? (byte)0 : (byte)1,
                            RemoteFlag = p.CanFormat == CanBus.CanFormat.Data ? (byte)0 : (byte)1,
                            ID = p.CanId,
                            DataLen = (byte)p.CanDataLen,
                            Data = new byte[p.CanDataLen]
                        };
                        for (var j = 0; j < p.CanDataLen; j++)
                        {
                            canMsg[i].Data[j] = p.CanData[j];
                        }
                    }

                    var result = Usb2Can.CAN_SendMsg(_devHandle, (byte)canIndex, canMsg, (uint)canMsg.Length);
                    //Console.WriteLine(result);
                }
                else
                {
                    var canMsg = new Usb2Canfd.CanfdMsg[dataPackages.Count];
                    var pCanSendMsg = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Usb2Canfd.CanfdMsg)) * dataPackages.Count);//申请缓冲区
                    for (var i = 0; i < dataPackages.Count; i++)
                    {
                        var p = dataPackages[i];

                        canMsg[i] = new Usb2Canfd.CanfdMsg
                        {
                            Flags = p.CanProtocol == CanBus.CanProtocol.Can ? (byte)1 : (byte)5,
                            DLC = (byte)p.CanDataLen,
                            //ID = p.CanId,//;(UInt32)(p.CanId | USB2CANFD.CANFD_MSG_FLAG_IDE),
                            //ID = (UInt32)(p.CanId | ((int)(1 << 31))),
                            ID = p.CanType == CanBus.CanType.Extended ? (UInt32)(p.CanId | ((int)(1 << 31))) : p.CanId,
                            Data = new byte[64],

                            //ExternFlag = p.CanType == CanBus.CanType.Standard ? (byte)0 : (byte)1,
                            //RemoteFlag = p.CanFormat == CanBus.CanFormat.Data ? (byte)0 : (byte)1,
                            //ID = p.CanId,
                            //DataLen = (byte)p.CanDataLen,
                            //Data = new byte[p.CanDataLen]
                        };
                        for (var j = 0; j < p.CanDataLen; j++)
                        {
                            canMsg[i].Data[j] = p.CanData[j];
                        }

                        // 将数组中的数据复制到数据缓冲区中
                        var pPonitor = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Usb2Canfd.CanfdMsg)));
                        Marshal.StructureToPtr(canMsg[i], pPonitor, true);
                        var buffer = new byte[Marshal.SizeOf(typeof(Usb2Canfd.CanfdMsg))];
                        Marshal.Copy(pPonitor, buffer, 0, Marshal.SizeOf(typeof(Usb2Canfd.CanfdMsg)));
                        Marshal.Copy(buffer, 0, (IntPtr)((uint)pCanSendMsg + i * Marshal.SizeOf(typeof(Usb2Canfd.CanfdMsg))), Marshal.SizeOf(typeof(Usb2Canfd.CanfdMsg)));
                        Marshal.FreeHGlobal(pPonitor);//释放缓冲区
                    }

                    var result = Usb2Canfd.CANFD_SendMsg(_devHandle, (byte)canIndex, pCanSendMsg, canMsg.Length);
                    //Console.WriteLine(result);
                }
            }
        }

        private void AddCan0Data(CanBus.CanDataPackage[] datas)
        {
            if (Can1 == null)
                return;

            Can1.ReceiveCanDatas(datas);
        }

        private void AddCan1Data(CanBus.CanDataPackage[] datas)
        {
            if (Can2 == null)
                return;

            Can2.ReceiveCanDatas(datas);
        }

        private class ToomossUsbLinChannel : LinBus
        {
            private readonly ToomossUsb2XxxCanLin _toomossUsb2XxxCanLin;
            private readonly int _linIndex;

            public ToomossUsbLinChannel(
                ToomossUsb2XxxCanLin toomossUsb2XxxCanLin, string name, int linIndex) :
                base(name)
            {
                _linIndex = linIndex;
                _toomossUsb2XxxCanLin = toomossUsb2XxxCanLin;
                RegisterSendMasterLinFunc(SendMasterLinMessage);
                RegisterSendSlaveLinACtion(SendSlaveLinMessage);
            }

            private bool SendMasterLinMessage(LinDataPackage linDataPackage, bool isWaitSlaveLin)
            {
                if (linDataPackage == null)
                    return false;

                if (!_toomossUsb2XxxCanLin.SendMasterLin(
                    linDataPackage.LinId, linDataPackage.LinData, _linIndex))
                    return false;
                RecviveLinDatas(new[] { linDataPackage });
                return true;
            }

            private void SendSlaveLinMessage(byte slaveLinId)
            {
                byte[] echo;
                if (_toomossUsb2XxxCanLin.SendSlaveLin(slaveLinId, _linIndex, out echo))
                    RecviveLinDatas(new[] { new LinDataPackage(ConvertLinId(slaveLinId), echo) });
            }
        }

        private class ToomossUsbCanChannel : CanBus
        {
            private readonly ToomossUsb2XxxCanLin _toomossUsb2XxxCanLin;
            private readonly int _canIndex;
            private const int MaxCanDataLen = 50;
            public ToomossUsbCanChannel(
                ToomossUsb2XxxCanLin toomossUsb2XxxCanLin, string name, int canIndex)
                : base(name, MaxCanDataLen)
            {
                _canIndex = canIndex;
                _toomossUsb2XxxCanLin = toomossUsb2XxxCanLin;
                RegisterSendAction(SendMultipleCans);
            }

            private void SendMultipleCans(CanDataPackage[] dataPackages)
            {
                var sendAct = new Action<CanDataPackage[]>(dp =>
                {
                    _toomossUsb2XxxCanLin.SendCanData(_canIndex - 1, dataPackages);
                });

                if (dataPackages.Length <= MaxCanDataLen)
                    sendAct(dataPackages);
                else
                {
                    var count = dataPackages.Length / MaxCanDataLen;
                    var rest = dataPackages.Length % MaxCanDataLen;

                    var temp = dataPackages.ToList();
                    for (var i = 0; i < count; i++)
                    {
                        var sendTemp = new CanDataPackage[MaxCanDataLen];
                        Array.Copy(temp.ToArray(), sendTemp, MaxCanDataLen);
                        temp.RemoveRange(0, MaxCanDataLen);
                        sendAct(sendTemp);
                    }

                    if (temp.Any())
                        sendAct(temp.ToArray());
                }
            }
        }
    }
}
