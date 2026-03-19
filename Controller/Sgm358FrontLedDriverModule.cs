using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;

namespace Controller
{
    [Description("CAN-Product,SGM358前灯")]
    public sealed class Sgm358FrontLedDriverModule : ControllerBase
    {
        public CanBus Can;

        [Description("R/W,左右节点")]
        public bool IsLeftNode = true;

        [Description("R,APP和CFG下载结果")]
        public string DownloadResultStr = string.Empty;

        [Description("R,总成零件号")]
        public string PartNumberStr = string.Empty;

        [Description("R,生产序列号")]
        public string SerialNumerStr = string.Empty;

        [Description("R,FBL零件号")]
        public string FblPartNoStr = string.Empty;

        [Description("R,FBL版本号")]
        public string FblVersionStr = string.Empty;

        [Description("R,APP零件号")]
        public string AppPartNoStr = string.Empty;

        [Description("R,APP版本号")]
        public string AppVersionStr = string.Empty;

        [Description("R,CFG零件号")]
        public string ConfigPartNoStr = string.Empty;

        [Description("R,CFG版本号")]
        public string ConfigVersionStr = string.Empty;

        [Description("R,当前模式读取结果")]
        public string ModeReadResult = string.Empty;

        public double Ntc1ReadDouble;
        public double Ntc2ReadDouble;
        public double Ntc3ReadDouble;
        public double Ntc4ReadDouble;

        public Sgm358FrontLedDriverModule(string name)
            : base(name)
        {
            foreach (
                var temp in
                    Enum.GetValues(typeof(LampOnOffType))
                        .Cast<LampOnOffType>()
                        .Where(temp => temp.GetCustomAttribute<MatrixValDefinition>() != null))
                _lightOperaterDic.Add(temp, temp.GetCustomAttribute<MatrixValDefinition>());

            if (_mainWorkThread != null)
            {
                _mainWorkThread.Abort();
                _mainWorkThread.Join();
            }
            _mainWorkThread = new Thread(LightOnOffTimer);
            _mainWorkThread.Start();
        }

        ~Sgm358FrontLedDriverModule()
        {
            Dispose();
        }

        public string DrvFlashFilePath = @"D:\Projects\LIN在线刷写\产品\S11L软件发布-20220824\DrvFlash_Xc2000.s19";
        public string AppFilePath = @"D:\Projects\LIN在线刷写\产品\VX1低配软件发布（试装）\SW0003889.A002.s19";
        public string CaliFailPath = @"D:\Projects\LIN在线刷写\产品\VX1低配软件发布（试装）\SW0003890_C002.s19";
        //public string AppFilePath = @"D:\Projects\EP33系列\模块_LDM\EP33-L高配模块-程序刷写\P00141531\S001\P00141531\SW0000955_A001.s19";
        //public string CaliFailPath = @"D:\Projects\EP33系列\模块_LDM\EP33-L高配模块-程序刷写\P00141531\S001\P00141531\SW0000956_C001.s19";

        private readonly Dictionary<LampOnOffType, MatrixValDefinition> _lightOperaterDic =
            new Dictionary<LampOnOffType, MatrixValDefinition>();
        private int _periodCount;
        private bool _isSleeping = true;
        private bool _isInExtendedSession;
        private readonly Thread _mainWorkThread;

        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X241 =
            new CanCommunicationMatrix.MotorolaMatrix(0x241, 6);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X102 =
            new CanCommunicationMatrix.MotorolaMatrix(0x102, 8);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X300 =
            new CanCommunicationMatrix.MotorolaMatrix(0x300, 8);
        private static readonly object LockFile = new object();

        private void LightOnOffTimer()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                _periodCount++;

                if (_periodCount > 150)
                    _periodCount = 0;

                try
                {
                    if (Can == null)
                        return;

                    if (!_isSleeping)
                    {
                        var lstPages = new List<CanBus.CanDataPackage>
                    {
                        new CanBus.CanDataPackage(
                            _motorolaMatrix0X102.CanId,
                            CanBus.CanProtocol.Can,
                            CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            _motorolaMatrix0X102.MatrixData),
                        new CanBus.CanDataPackage(
                            _motorolaMatrix0X241.CanId,
                            CanBus.CanProtocol.Can,
                            CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            _motorolaMatrix0X241.MatrixData)
                    };

                        if (_periodCount % 3 == 0)
                            lstPages.Add(
                                new CanBus.CanDataPackage(
                                    _motorolaMatrix0X300.CanId,
                                    CanBus.CanProtocol.Can,
                                    (byte)CanBus.CanType.Standard,
                                    (byte)CanBus.CanFormat.Data,
                                    _motorolaMatrix0X300.MatrixData));

                        Can.SendCanDatas(lstPages.ToArray());
                    }

                    if (_isInExtendedSession && _periodCount == 50)
                        Can.SendStandardCanData(GetCurrentSeesionCanId(),
                            new byte[] { 0x02, 0x3E, 0x00, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA });

                    //单颗远光灯LED控制命令可以通过诊断服务2F进行控制。命令格式如下：
                    //1F6/1F8：05 2F 50 92 03 XX AA AA
                    //其中XX即为上表中的内容。
                    //比如说控制ALB1#点亮，其他熄灭，应当发送命令
                    //1F6/1F8： 05 2F 50 92 03 10 AA AA
                    //控制中要求：
                    //在上述控制过程中，下面的报文必须每3s发送一次。
                    //1F6/1F8：02 3E 80 AA AA AA AA AA
                    //控制后要求：控制后需要发出下述命令后，大灯才能进入正常的灯控命令。
                    //1F6/1F8：02 10 01 AA AA AA AA AA
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        [Description("APP和CFG下载")]
        public void Download()
        {
            DownloadResultStr = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - 下载中";

            if (Can == null)
            {
                DownloadResultStr = "NG CAN未初始化";
                return;
            }

            var fileList = new List<string>();

            lock (LockFile)
            {
                if (!string.IsNullOrEmpty(AppFilePath))
                    if (!File.Exists(AppFilePath))
                        DownloadResultStr = "NG APP文件不存在";
                    else
                        fileList.Add(AppFilePath);

                if (!string.IsNullOrEmpty(CaliFailPath))
                    if (!File.Exists(CaliFailPath))
                        DownloadResultStr = "NG CFG文件不存在";
                    else
                        fileList.Add(CaliFailPath);
            }

            if (!fileList.Any())
                DownloadResultStr = "NG 未指定下载文件";

            var downloadAction = new Func<bool>(() =>
            {
                if (!Can.CanBusWithUds.TryEnterExtendedSession(
                    GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), CanBus.CanType.Standard, pendingByte: 0x00))
                {
                    //Console.WriteLine("进入拓展模式 10 03 失败");
                    DownloadResultStr = "NG 进入拓展模式 10 03 失败";
                    return false;
                }

                Thread.Sleep(250);

                if (!Can.CanBusWithUds.TryControlDtcSetting(
                    GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), CanBus.CanType.Standard, Uds14229Helper.ControlDtcSetting.Disable, pendingByte: 0x00))
                {
                    //Console.WriteLine("关闭DTC 85 02 失败");
                    DownloadResultStr = "NG 关闭DTC 85 02 失败";
                    return false;
                }

                Thread.Sleep(250);

                if (!Can.CanBusWithUds.TryCommunicationControl(
                    GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), CanBus.CanType.Standard, Uds14229Helper.CommunicationControl.DisableRxAndTx, pendingByte: 0x00))
                {
                    //Console.WriteLine("关闭正常通信 28 03 01 失败");
                    DownloadResultStr = "NG 关闭正常通信 28 03 01 失败";
                    return false;
                }

                Thread.Sleep(250);

                if (!Can.CanBusWithUds.TryEnterProgrammingSession(
                    GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), CanBus.CanType.Standard, pendingByte: 0x00))
                {
                    //Console.WriteLine("进入编程模式 10 02 失败");
                    DownloadResultStr = "NG 进入编程模式 10 02 失败";
                    return false;
                }

                Thread.Sleep(250);

                const byte requestSeedSubFunc = (byte)0x11;
                const byte sendKeySunFunc = (byte)0x12;
                Func<byte[], byte[]> generateKey = para => Uds14229Helper.CommonGenerateKey64Bits(para).ToArray();
                byte[] seedBytes;
                if (!Can.CanBusWithUds.TryRequestSeed(GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), CanBus.CanType.Standard, requestSeedSubFunc, out seedBytes, pendingByte: 0x00))
                {
                    //Console.WriteLine("请求seed 27 失败，{0}", requestSeedSubFunc);
                    DownloadResultStr = string.Format("NG 请求seed 27 失败，{0}", requestSeedSubFunc);
                    return false;
                }

                Thread.Sleep(250);

                var keyBytes = generateKey.Invoke(seedBytes);

                if (!Can.CanBusWithUds.TrySendKey(GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), CanBus.CanType.Standard, sendKeySunFunc, keyBytes, pendingByte: 0x00))
                {
                    //Console.WriteLine("写入key 27 失败，{0},{1}", sendKeySunFunc, keyBytes);
                    DownloadResultStr = string.Format("NG 写入key 27 失败，{0},{1}", sendKeySunFunc, keyBytes);
                    return false;
                }

                Thread.Sleep(250);

                if (!DownloadDrvFlash(ref DownloadResultStr))
                {
                    DownloadResultStr = "NG DrvFlash下载失败 " + DownloadResultStr;
                    return false;
                }

                Thread.Sleep(250);

                if (
                    fileList.Select(SRecordFileHelper.GetSRecordLineData)
                        .Select(SRecordFileHelper.GetBlocks)
                        .Any(blocks => !Can.CanBusWithUds.TransferData(
                            GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), CanBus.CanType.Standard,
                            blocks, true, ref DownloadResultStr, pendingByte: 0x00, isCrcCheck: true)))
                {
                    DownloadResultStr = "NG 下载Block失败";
                    return false;
                }

                if (!Can.CanBusWithUds.TryStartRoutineControl(
                      GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(),
                       CanBus.CanType.Standard, Uds14229Helper.RoutineControl.CheckProgrammingDependencies, pendingByte: 0x00))
                {
                    DownloadResultStr = "NG 检测程序一致完整性3101FF01失败";
                    return false;
                }

                Can.CanBusWithUds.TryEnterDefaultSession(
                    GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), CanBus.CanType.Standard, pendingByte: 0x00);

                return true;
            });

            var st = new Stopwatch();
            st.Start();
            var isOk = downloadAction.Invoke();
            st.Stop();

            if (isOk)
            {
                DownloadResultStr = "OK 耗时：" + st.ElapsedMilliseconds * 0.001 + "s";
                //Thread.Sleep(500);
                //ReadFblPartNo();
                //ReadAppPartNo();
                //ReadConfigPartNo();
                //ReadFblVersion();
                //ReadAppVersion();
                //ReadConfigVersion();
                //EnterSleepMode();
                //Thread.Sleep(2000);
                //ExitSleepMode();
                //SetupNormorMode();
                //Thread.Sleep(250);
                //ReadMode();
            }
            else
            {
                DownloadResultStr += " 耗时：" + st.ElapsedMilliseconds * 0.001 + "s";
            }
        }

        /// <summary>
        /// 下载DrvFlash
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool DownloadDrvFlash(ref string result)
        {
            if (!string.IsNullOrEmpty(DrvFlashFilePath))
            {
                if (!File.Exists(DrvFlashFilePath))
                {
                    result = "NG DrvFlash文件不存在";
                    return false;
                }
            }
            else
            {
                result = "NG 未指定DrvFlash文件";
                return false;
            }

            var sRecord = SRecordFileHelper.GetSRecordLineData(DrvFlashFilePath);
            var drvFlashBlocks = SRecordFileHelper.GetBlocks(sRecord); // Block集合

            return Can.CanBusWithUds.TransferData(
                GetCurrentSeesionCanId(),
                GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard,
                drvFlashBlocks, false, ref result, isCrcCheck: true);
        }

        [Description("休眠")]
        public void EnterSleepMode()
        {
            LeftParkLmpOff();
            RightParkLmpOff();

            LeftDrlOff();
            RightDrlOff();

            LeftTurnOff();
            RightTurnOff();

            LeftLowBmOff();
            RightLowBmOn();

            HighBmOff();

            LeftCornerOff();
            RightCornerOff();

            _isSleeping = true;
        }

        [Description("唤醒")]
        public void ExitSleepMode()
        {
            _isSleeping = false;
        }

        [Description("进入扩展模式")]
        public void EnterExtendedSession()
        {
            if (Can == null)
                return;

            if (!Can.CanBusWithUds.TryEnterExtendedSession(GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard))
            {
                Thread.Sleep(500);
                Can.CanBusWithUds.TryEnterExtendedSession(GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard);
            }

            Thread.Sleep(50);

            Can.CanBusWithUds.TryControlDtcSetting(
                GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), CanBus.CanType.Standard,
                Uds14229Helper.ControlDtcSetting.Disable);

            _isInExtendedSession = true;
        }

        [Description("退出扩展模式")]
        public void ExitExtendedSession()
        {
            _isInExtendedSession = false;
        }

        [Description("写入当前模式为测试模式")]
        public void SetupTestMode()
        {
            if (Can == null)
                return;

            Can.CanBusWithUds.TryWriteData(
                GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xD0, 0x2A, new byte[] { 0xFF });
        }

        [Description("写入当前模式为正常模式")]
        public void SetupNormorMode(string hexValue)
        {
            if (Can == null)
                return;

            try
            {
                var value = Convert.ToByte(hexValue, 16);

                Can.CanBusWithUds.TryWriteData(
                    GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xD0, 0x2A, new[] { value });
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("读取当前模式")]
        public void ReadMode()
        {
            ModeReadResult = string.Empty;

            if (Can == null)
                return;

            byte[] echo;
            if (Can.CanBusWithUds.TryReadData(
                GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xD0, 0x2A, out echo))
            {
                ModeReadResult = ValueHelper.GetHextStr(echo);
            }
        }

        public bool Led1Control;
        public bool Led2Control;
        public bool Led3Control;
        public bool Led4Control;
        public bool Led5Control;
        public bool Led6Control;

        public bool HsdControl;
        public bool Slave1Control;
        public bool Slave2Control;

        public void InputOutPutControl()
        {
            var b1Str = string.Format(
                "00{0}{1}{2}{3}{4}{5}",
                Led6Control ? "1" : "0",
                Led5Control ? "1" : "0",
                Led4Control ? "1" : "0",
                Led3Control ? "1" : "0",
                Led2Control ? "1" : "0",
                Led1Control ? "1" : "0");

            var b2Str = string.Format(
                "00000{0}{1}{2}",
                Slave2Control ? "1" : "0",
                Slave1Control ? "1" : "0",
                HsdControl ? "1" : "0");

            var b1 = Convert.ToByte(b1Str, 2);
            var b2 = Convert.ToByte(b2Str, 2);

            if (Can == null)
                return;

            var bs = new[] { b1, b2 };
            if (Can.CanBusWithUds.TryInputOutputControl(
                GetCurrentSeesionCanId(),
                GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard,
                0x50, 0x94,
                Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment,
                bs.ToList()))
                return;

            Thread.Sleep(500);
            Can.CanBusWithUds.TryInputOutputControl(
                GetCurrentSeesionCanId(),
                GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard,
                0x50, 0x94,
                Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment,
                bs.ToList());
        }

        public void AllLedSlaveOn()
        {
            if (Can == null)
                return;

            var bs = new byte[] { 0x2F, 0x50, 0x94, 0x03, 0x3F, 0x06 };
            //var bs = new byte[] { 0x2F, 0x50, 0x94, 0x03, 0x0F, 0x07 };
            byte[] echo;
            Can.CanBusWithUds.TesterTryRequest(GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), bs.ToList(),
                out echo, CanBus.CanType.Standard);
        }

        public void AllLedSlaveOff()
        {
            Led1Control = false;
            Led2Control = false;
            Led3Control = false;
            Led4Control = false;
            Led5Control = false;
            Led6Control = false;

            HsdControl = false;
            Slave1Control = false;
            Slave2Control = false;

            if (Can == null)
                return;

            var bs = new byte[] { 0x2F, 0x50, 0x94, 0x03, 0x00, 0x00 };
            byte[] echo;
            Can.CanBusWithUds.TesterTryRequest(GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), bs.ToList(),
                out echo, CanBus.CanType.Standard);
        }

        public void HsdOn()
        {
            Led1Control = false;
            Led2Control = false;
            Led3Control = false;
            Led4Control = false;
            Led5Control = false;
            Led6Control = false;

            HsdControl = true;
            Slave1Control = false;
            Slave2Control = false;

            if (Can == null)
                return;

            var bs = new byte[] { 0x2F, 0x50, 0x94, 0x03, 0x04, 0x01 };
            byte[] echo;
            Can.CanBusWithUds.TesterTryRequest(GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), bs.ToList(),
                out echo, CanBus.CanType.Standard);
        }

        public void HsdOff()
        {
            Led1Control = false;
            Led2Control = false;
            Led3Control = false;
            Led4Control = false;
            Led5Control = false;
            Led6Control = false;

            HsdControl = false;
            Slave1Control = false;
            Slave2Control = false;

            if (Can == null)
                return;

            var bs = new byte[] { 0x2F, 0x50, 0x94, 0x03, 0x00, 0x00 };
            byte[] echo;
            Can.CanBusWithUds.TesterTryRequest(GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), bs.ToList(),
                out echo, CanBus.CanType.Standard);
        }

        public void GetCurrentSession()
        {
            if (Can == null)
                return;

            //Can.AddDoNotFilterCanId(0x116);
            //Can.AddDoNotFilterCanId(0x126);
            //Thread.Sleep(1500);

            //var r = Can.RecvCanMsg();

            //Can.RemoveDoNotFilterCanId(0x116);
            //Can.RemoveDoNotFilterCanId(0x126);


            Can.AddDoNotFilterCanId(0x1F7);
            Can.AddDoNotFilterCanId(0x1F9);

            Can.AddDoNotFilterCanId(0x1F6);
            Can.AddDoNotFilterCanId(0x1F8);

            IsLeftNode = false;

            //var canRecvDataPackages = r as CanBus.CanRecvDataPackage[] ?? r.ToArray();

            //var findLeft = canRecvDataPackages.FirstOrDefault(f => f.RecvCanId.Equals(0x116));
            //if (findLeft != null)
            //    IsLeftNode = true;

            //var findRight = canRecvDataPackages.FirstOrDefault(f => f.RecvCanId.Equals(0x126));
            //if (findRight != null)
            //    IsLeftNode = false;
        }

        private uint GetCurrentSeesionCanId()
        {
            return IsLeftNode ? (uint)0x1F6 : 0x1F8;
        }

        private uint GetCurrentSeesionRecvCanId()
        {
            return IsLeftNode ? (uint)0x1F7 : 0x1F9;
        }

        /// <summary>
        /// 读NTC
        /// </summary>
        /// <param name="index"></param>
        [Description("读NTC")]
        public void ReadNtc(string index)
        {
            byte didHi = 0x00;
            byte didLo = 0x00;

            switch (int.Parse(index))
            {
                case 1:
                    didHi = 0xD0;
                    didLo = 0x20;
                    Ntc1ReadDouble = -9999;
                    break;

                case 2:
                    didHi = 0xD0;
                    didLo = 0x21;
                    Ntc2ReadDouble = -9999;
                    break;

                case 3:
                    didHi = 0xD0;
                    didLo = 0x22;
                    Ntc3ReadDouble = -9999;
                    break;

                case 4:
                    didHi = 0xD0;
                    didLo = 0x23;
                    Ntc4ReadDouble = -9999;
                    break;
            }

            if (Can == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readNtc;
                Can.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo,
                    out readNtc);
                return readNtc;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;

            var ntc = result[0] * 256 + result[1];
            switch (int.Parse(index))
            {
                case 1:
                    Ntc1ReadDouble = ntc / 1024.0 * 5.0;
                    break;

                case 2:
                    Ntc2ReadDouble = ntc / 1024.0 * 5.0;
                    break;

                case 3:
                    Ntc3ReadDouble = ntc / 1024.0 * 5.0;
                    break;

                case 4:
                    Ntc4ReadDouble = ntc / 1024.0 * 5.0;
                    break;
            }
        }

        /// <summary>
        /// 写总成零件号
        /// </summary>
        /// <param name="partNumber"></param>
        public void WritePartNo(string partNumber)
        {
            if (Can == null)
                return;

            if (string.IsNullOrEmpty(partNumber))
                return;

            var bs = Encoding.ASCII.GetBytes(partNumber);

            var writeFunc = new Func<bool>(
                () =>
                    Can.CanBusWithUds.TryWriteData(
                        GetCurrentSeesionCanId(),
                        GetCurrentSeesionRecvCanId(),
                        CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xD0, 0x0F, bs.ToArray()));

            if (writeFunc.Invoke())
                return;
            Thread.Sleep(500);
            writeFunc.Invoke();
        }

        /// <summary>
        /// 读总成零件号
        /// </summary>
        [Description("读总成零件号")]
        public void ReadPartNo()
        {
            PartNumberStr = string.Empty;

            if (Can == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readPartNo;
                Can.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0xD0, 0x0F,
                    out readPartNo);
                return readPartNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;

            foreach (var item in result.Where(item => item >= 0x30 && item <= 0x7A))
                PartNumberStr += Encoding.ASCII.GetString(new[] { item });
        }

        /// <summary>
        /// 写生产系列号
        /// </summary>
        /// <param name="productNoandPartNo"></param>
        public void WriteSerialNo(string productNoandPartNo)
        {
            if (Can == null)
                return;

            if (string.IsNullOrEmpty(productNoandPartNo))
                return;

            if (productNoandPartNo.Split(':').Length != 2)
                return;

            var partNo = productNoandPartNo.Split(':')[1];

            if (partNo.Length != 6)
                return;

            var productNo = productNoandPartNo.Split(':')[0];
            var serialNo = string.Empty;
            var date = string.Empty;

            using (var conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                try
                {
                    var cmd = new SqlCommand("GetCheckNo", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@Productno", productNo);  //给输入参数赋值
                    //cmd.Parameters.AddWithValue("@checkStaff", "admin");  //给输入参数赋值
                    //cmd.Parameters.AddWithValue("@creater", "admin");  //给输入参数赋值
                    //var parOutputSerialNo = cmd.Parameters.Add("@serialNumber", SqlDbType.Int);  //定义输出参数 
                    //parOutputSerialNo.Direction = ParameterDirection.Output;  //参数类型为Output  
                    var parOutputDate = cmd.Parameters.Add("@date", SqlDbType.DateTime);  //定义输出参数 
                    parOutputDate.Direction = ParameterDirection.Output;  //参数类型为Output 
                    var returnSerialNo = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnSerialNo.Direction = ParameterDirection.ReturnValue;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    serialNo = returnSerialNo.Value.ToString();
                    date = parOutputDate.Value.ToString();
                    //serialNp = parOutputSerialNo.Value.ToString();
                    //date = parOutputDate.Value.ToString();
                    //MessageBox.Show(parOutputSerialNo.Value.ToString());   //显示输出参数的值  
                    //MessageBox.Show(parOutputDate.Value.ToString());  //显示返回值  
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            var year = DateTime.Now.Year;
            var yearStr = string.Empty;
            if (year == 2020)
            {
                yearStr = "W";
            }
            else if (year == 2021)
            {
                yearStr = "X";
            }
            else if (year == 2022)
            {
                yearStr = "Y";
            }
            else if (year == 2023)
            {
                yearStr = "Z";
            }


            var str = string.Format("{0}{1}{2}{3}{4}{5}", partNo, "A", "1", yearStr,
                DateTime.Parse(date).DayOfYear.ToString().PadLeft(3, '0'), serialNo.PadLeft(4, '0'));

            var bs = Encoding.ASCII.GetBytes(str);

            var writeFunc = new Func<bool>(
                () =>
                    Can.CanBusWithUds.TryWriteData(
                        GetCurrentSeesionCanId(),
                        GetCurrentSeesionRecvCanId(),
                        CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0x01, 0xB4, bs.ToArray()));

            if (writeFunc.Invoke())
                return;
            Thread.Sleep(500);
            writeFunc.Invoke();
        }

        /// <summary>
        /// 读生产序列号
        /// </summary>
        [Description("读生产序列号")]
        public void ReadSerialNo()
        {
            SerialNumerStr = string.Empty;

            if (Can == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readSerialNo;
                Can.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xB4,
                    out readSerialNo);
                return readSerialNo;
            });

            var result = readFunc.Invoke();// ?? readFunc.Invoke();

            if (result == null)
                return;

            foreach (var item in result.Where(item => item >= 0x30 && item <= 0x7A))
                SerialNumerStr += Encoding.ASCII.GetString(new[] { item });
        }

        /// <summary>
        /// 读引导程序零件号
        /// </summary>
        [Description("读引导程序零件号")]
        public void ReadFblPartNo()
        {
            FblPartNoStr = string.Empty;

            if (Can == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readFblPartNo;
                Can.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xC0,
                    out readFblPartNo);
                return readFblPartNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;

            //var str = result.Aggregate(string.Empty, (current, t) => current + t.ToString().PadLeft(2, '0'));
            FblPartNoStr = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0');
        }

        /// <summary>
        /// 读引导程序版本号
        /// </summary>
        [Description("读引导程序版本号")]
        public void ReadFblVersion()
        {
            FblVersionStr = string.Empty;

            if (Can == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readFblNo;
                Can.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xD0,
                    out readFblNo);
                return readFblNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;
            foreach (var item in result.Where(item => item >= 0x30 && item <= 0x7A))
                FblVersionStr += Encoding.ASCII.GetString(new[] { item });
        }

        /// <summary>
        /// 读应用程序零件号
        /// </summary>
        [Description("读应用程序零件号")]
        public void ReadAppPartNo()
        {
            AppPartNoStr = string.Empty;

            if (Can == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readAppPartNo;
                Can.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xC1,
                    out readAppPartNo);
                return readAppPartNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;

            //var str = result.Aggregate(string.Empty, (current, t) => current + t.ToString().PadLeft(2, '0'));
            AppPartNoStr = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0');
        }

        /// <summary>
        /// 读应用程序版本号
        /// </summary>
        [Description("读应用程序版本号")]
        public void ReadAppVersion()
        {
            AppVersionStr = string.Empty;

            if (Can == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readAppNo;
                Can.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xD1,
                    out readAppNo);
                return readAppNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;
            foreach (var item in result.Where(item => item >= 0x30 && item <= 0x7A))
                AppVersionStr += Encoding.ASCII.GetString(new[] { item });
        }

        /// <summary>
        /// 读取配置文件零件号
        /// </summary>
        [Description("读取配置文件零件号")]
        public void ReadConfigPartNo()
        {
            ConfigPartNoStr = string.Empty;

            if (Can == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readConfigPartNo;
                Can.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xC2,
                    out readConfigPartNo);
                return readConfigPartNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;

            //var str = result.Aggregate(string.Empty, (current, t) => current + t.ToString().PadLeft(2, '0'));
            ConfigPartNoStr = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0');
        }

        /// <summary>
        /// 读取配置文件版本号
        /// </summary>
        [Description("读取配置文件版本号")]
        public void ReadConfigVersion()
        {
            ConfigVersionStr = string.Empty;

            if (Can == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readConfigVersion;
                Can.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xD2,
                    out readConfigVersion);
                return readConfigVersion;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;
            foreach (var item in result.Where(item => item >= 0x30 && item <= 0x7A))
                ConfigVersionStr += Encoding.ASCII.GetString(new[] { item });
        }

        [Description("左PL开")]
        public void LeftParkLmpOn()
        {
            SwitchParkLamp(true, true);
        }

        [Description("左PL关")]
        public void LeftParkLmpOff()
        {
            SwitchParkLamp(true, false);
        }

        [Description("右PL开")]
        public void RightParkLmpOn()
        {
            SwitchParkLamp(false, true);
        }

        [Description("右PL关")]
        public void RightParkLmpOff()
        {
            SwitchParkLamp(false, false);
        }

        private void SwitchParkLamp(
            bool isLeft, bool isOn)
        {
            if (isLeft)
                NormalLampCtrl(
                    isOn
                        ? LampOnOffType.LftPrkLmpOn
                        : LampOnOffType.LftPrkLmpOff);
            else
                NormalLampCtrl(
                    isOn
                        ? LampOnOffType.RtPrkLmpOn
                        : LampOnOffType.RtPrkLmpOff);
        }

        [Description("左DRL开")]
        public void LeftDrlOn()
        {
            SwitchDrl(true, true);
        }

        [Description("左DRL关")]
        public void LeftDrlOff()
        {
            SwitchDrl(true, false);
        }

        [Description("右DRL开")]
        public void RigthDrlOn()
        {
            SwitchDrl(false, true);
        }

        [Description("右DRL关")]
        public void RightDrlOff()
        {
            SwitchDrl(false, false);
        }

        private void SwitchDrl(
            bool isLeft, bool isOn)
        {
            if (isLeft)
            {
                NormalLampCtrl(
                    isOn ?
                    LampOnOffType.LftDrlOn :
                    LampOnOffType.LftDrlOff);
            }
            else
            {
                NormalLampCtrl(
                    isOn ?
                    LampOnOffType.RtDrlOn :
                    LampOnOffType.RtDrlOff);
            }
        }

        [Description("左转向灯常亮")]
        public void LeftTurnHoldOn()
        {
            SwitchTurn(true, true);
        }

        [Description("左转向灯跑马")]
        public void LeftTurnWaterFlash()
        {
            SwitchTurn(true, true, true);
        }

        [Description("左转向灯关")]
        public void LeftTurnOff()
        {
            SwitchTurn(true, false);
        }

        [Description("右转向灯常亮")]
        public void RightTurnHoldOn()
        {
            SwitchTurn(false, true);
        }

        [Description("右转向灯跑马")]
        public void RightTurnWaterFlash()
        {
            SwitchTurn(false, true, true);
        }

        [Description("右转向灯关")]
        public void RightTurnOff()
        {
            SwitchTurn(false, false);
        }

        private void SwitchTurn(
            bool isLeft, bool isOn, bool isWaterFlash = false)
        {
            if (isLeft)
            {
                if (isOn)
                {
                    NormalLampCtrl(isWaterFlash
                        ? LampOnOffType.LftFrtTrnLmpWaterFlash
                        : LampOnOffType.LftFrtTrnLmpHoldOn);
                }
                else
                    NormalLampCtrl(LampOnOffType.LftFrtTrnLmpOff);
            }
            else
            {
                if (isOn)
                {
                    NormalLampCtrl(isWaterFlash
                        ? LampOnOffType.RghtFrtTrnLmpWaterFlash
                        : LampOnOffType.RghtFrtTrnLmpHoldOn);
                }
                else
                    NormalLampCtrl(LampOnOffType.RghtFrtTrnLmpOff);
            }
        }

        [Description("左近光开")]
        public void LeftLowBmOn()
        {
            SwitchLowBm(true, true);
        }

        [Description("左近光关")]
        public void LeftLowBmOff()
        {
            SwitchLowBm(true, false);
        }

        [Description("右近光开")]
        public void RightLowBmOn()
        {
            SwitchLowBm(false, true);
        }

        [Description("右近光关")]
        public void RightLowBmOff()
        {
            SwitchLowBm(false, false);
        }

        private void SwitchLowBm(bool isLeft, bool isOn)
        {
            if (isLeft)
            {
                NormalLampCtrl(
                    isOn ?
                    LampOnOffType.LftLowBmOn :
                    LampOnOffType.LftLowBmOff);
            }
            else
            {
                NormalLampCtrl(
                    isOn ?
                    LampOnOffType.RtLowBmOn :
                    LampOnOffType.RtLowBmOff);
            }
        }

        [Description("远光开")]
        public void HighBmOn()
        {
            SwitchHighBm(true);
        }

        [Description("远光关")]
        public void HighBmOff()
        {
            SwitchHighBm(false);
        }

        private void SwitchHighBm(bool isOn)
        {
            NormalLampCtrl(
                   isOn ?
                   LampOnOffType.HbModeSetOn :
                   LampOnOffType.HbModeSetOff);
        }

        [Description("左角灯开")]
        public void LeftCornerOn()
        {
            SwitchCorner(true, true);
        }

        [Description("左角灯关")]
        public void LeftCornerOff()
        {
            SwitchCorner(true, false);
        }

        [Description("右角灯开")]
        public void RightCornerOn()
        {
            SwitchCorner(false, true);
        }

        [Description("右角灯关")]
        public void RightCornerOff()
        {
            SwitchCorner(false, false);
        }

        private void SwitchCorner(bool isLeft, bool isOn)
        {
            if (isLeft)
            {
                NormalLampCtrl(
                    isOn ?
                    LampOnOffType.LftCornCtrlOn :
                    LampOnOffType.LftCornCtrlOff);
            }
            else
            {
                NormalLampCtrl(
                    isOn ?
                    LampOnOffType.RghtCornCtrlOn :
                    LampOnOffType.RghtCornCtrlOff);
            }
        }

        private void NormalLampCtrl(
            LampOnOffType lampOnOffType)
        {
            if (_isSleeping)
                return;

            if (lampOnOffType == LampOnOffType.HbModeSetOn ||
                lampOnOffType == LampOnOffType.HbModeSetOff)
                _motorolaMatrix0X102.UpdateData(_lightOperaterDic[lampOnOffType]);
            else
                _motorolaMatrix0X241.UpdateData(_lightOperaterDic[lampOnOffType]);
        }

        public void MotorRun()
        {
            //_motorolaMatrix0X300.UpdateData(_lightOperaterDic[LampOnOffType.DhlDirConfAbove]);
            //_motorolaMatrix0X300.UpdateData(_lightOperaterDic[LampOnOffType.LdhlRefCmdActive]);
            //_motorolaMatrix0X300.UpdateData(_lightOperaterDic[LampOnOffType.LdhlConfPosStep255]);

            _motorolaMatrix0X300.MatrixData = new byte[] { 0xA1, 0x68, 0x00, 0x00, 0x00, 0x00, 0x81, 0x68 };
            Thread.Sleep(2000);
            _motorolaMatrix0X300.MatrixData = new byte[] { 0x20, 0x16, 0x00, 0x00, 0x00, 0x00, 0x00, 0x16 };
        }

        public void MotorNotRun()
        {
            _motorolaMatrix0X300.MatrixData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }

        /// <summary>
        /// 点灯控制类型
        /// </summary>
        public enum LampOnOffType
        {
            /// <summary>
            /// 远光关
            /// </summary>
            [MatrixValDefinition(12, 4, 0)]
            [Description("0x102")]
            HbModeSetOff,

            /// <summary>
            /// 远光开
            /// </summary>
            [MatrixValDefinition(12, 4, 1)]
            [Description("0x102")]
            HbModeSetOn,

            /// <summary>
            /// 左位置灯灭
            /// </summary>
            [MatrixValDefinition(11, 1, 0)]
            [Description("0x241")]
            LftPrkLmpOff,

            /// <summary>
            /// 左位置灯亮
            /// </summary>
            [MatrixValDefinition(11, 1, 1)]
            [Description("0x241")]
            LftPrkLmpOn,

            /// <summary>
            /// 左日行灯灭
            /// </summary>
            [MatrixValDefinition(13, 1, 0)]
            [Description("0x241")]
            LftDrlOff,

            /// <summary>
            /// 左日行灯亮
            /// </summary>
            [MatrixValDefinition(13, 1, 1)]
            [Description("0x241")]
            LftDrlOn,

            /// <summary>
            /// 左转向灯熄灭
            /// </summary>
            [MatrixValDefinition(23, 3, 0)]
            [Description("0x241")]
            LftFrtTrnLmpOff,

            /// <summary>
            /// 左转向灯常亮
            /// </summary>
            [MatrixValDefinition(23, 3, 5)]
            [Description("0x241")]
            LftFrtTrnLmpHoldOn,

            /// <summary>
            /// 左转向灯跑马
            /// </summary>
            [MatrixValDefinition(23, 3, 1)]
            [Description("0x241")]
            LftFrtTrnLmpWaterFlash,

            /// <summary>
            /// 左近光灯灭
            /// </summary>
            [MatrixValDefinition(7, 1, 0)]
            [Description("0x241")]
            LftLowBmOff,

            /// <summary>
            /// 左近光灯亮
            /// </summary>
            [MatrixValDefinition(7, 1, 1)]
            [Description("0x241")]
            LftLowBmOn,

            /// <summary>
            /// 左角灯关
            /// </summary>
            [MatrixValDefinition(16, 8, 0)]
            [Description("0x102")]
            LftCornCtrlOff,

            /// <summary>
            /// 左角灯开
            /// </summary>
            [MatrixValDefinition(16, 8, 100)]
            [Description("0x102")]
            LftCornCtrlOn,

            /// <summary>
            /// 右位置灯灭
            /// </summary>
            [MatrixValDefinition(30, 1, 0)]
            [Description("0x241")]
            RtPrkLmpOff,

            /// <summary>
            /// 右位置灯亮
            /// </summary>
            [MatrixValDefinition(30, 1, 1)]
            [Description("0x241")]
            RtPrkLmpOn,

            /// <summary>
            /// 右日行灯灭
            /// </summary>
            [MatrixValDefinition(16, 1, 0)]
            [Description("0x241")]
            RtDrlOff,

            /// <summary>
            /// 右日行灯亮
            /// </summary>
            [MatrixValDefinition(16, 1, 1)]
            [Description("0x241")]
            RtDrlOn,

            /// <summary>
            /// 右转向灯熄灭
            /// </summary>
            [MatrixValDefinition(26, 3, 0)]
            [Description("0x241")]
            RghtFrtTrnLmpOff,

            /// <summary>
            /// 右转向灯常亮
            /// </summary>
            [MatrixValDefinition(26, 3, 5)]
            [Description("0x241")]
            RghtFrtTrnLmpHoldOn,

            /// <summary>
            /// 右转向灯跑马
            /// </summary>
            [MatrixValDefinition(26, 3, 1)]
            [Description("0x241")]
            RghtFrtTrnLmpWaterFlash,

            /// <summary>
            /// 右近光灯灭
            /// </summary>
            [MatrixValDefinition(22, 1, 0)]
            [Description("0x241")]
            RtLowBmOff,

            /// <summary>
            /// 右近光灯亮
            /// </summary>
            [MatrixValDefinition(22, 1, 1)]
            [Description("0x241")]
            RtLowBmOn,

            /// <summary>
            /// 右角灯关
            /// </summary>
            [MatrixValDefinition(24, 8, 0)]
            [Description("0x102")]
            RghtCornCtrlOff,

            /// <summary>
            /// 右角灯开
            /// </summary>
            [MatrixValDefinition(24, 8, 100)]
            [Description("0x102")]
            RghtCornCtrlOn,

            [MatrixValDefinition(35, 3, 0)]
            [Description("0x241")]
            FrtLtShwModeOff,

            [MatrixValDefinition(35, 3, 1)]
            [Description("0x241")]
            FrtLtShwModeSimple,

            [MatrixValDefinition(35, 3, 2)]
            [Description("0x241")]
            FrtLtShwModeComplex,

            /// <summary>
            /// 近光第一颗点亮
            /// </summary>
            [MatrixValDefinition(40, 1, 1)]
            [Description("0x1F6")]
            Lb1On,

            /// <summary>
            /// 近光第一颗熄灭
            /// </summary>
            [MatrixValDefinition(40, 1, 0)]
            [Description("0x1F6")]
            Lb1Off,

            /// <summary>
            /// 近光第二颗点亮
            /// </summary>
            [MatrixValDefinition(41, 1, 1)]
            [Description("0x1F6")]
            Lb2On,

            /// <summary>
            /// 近光第二颗熄灭
            /// </summary>
            [MatrixValDefinition(41, 1, 0)]
            [Description("0x1F6")]
            Lb2Off,

            /// <summary>
            /// 近光第三颗点亮
            /// </summary>
            [MatrixValDefinition(42, 1, 1)]
            [Description("0x1F6")]
            Lb3On,

            /// <summary>
            /// 近光第三颗熄灭
            /// </summary>
            [MatrixValDefinition(42, 1, 0)]
            [Description("0x1F6")]
            Lb3Off,

            /// <summary>
            /// 近光第四颗点亮
            /// </summary>
            [MatrixValDefinition(43, 1, 1)]
            [Description("0x1F6")]
            Lb4On,

            /// <summary>
            /// 近光第四颗熄灭
            /// </summary>
            [MatrixValDefinition(43, 1, 0)]
            [Description("0x1F6")]
            Lb4Off,

            /// <summary>
            /// 辅近光第一颗点亮
            /// </summary>
            [MatrixValDefinition(44, 1, 1)]
            [Description("0x1F6")]
            Alb1On,

            /// <summary>
            /// 辅近光第一颗熄灭
            /// </summary>
            [MatrixValDefinition(44, 1, 0)]
            [Description("0x1F6")]
            Alb1Off,

            /// <summary>
            /// 辅近光第二颗点亮
            /// </summary>
            [MatrixValDefinition(45, 1, 1)]
            [Description("0x1F6")]
            Alb2On,

            /// <summary>
            /// 辅近光第二颗熄灭
            /// </summary>
            [MatrixValDefinition(45, 1, 0)]
            [Description("0x1F6")]
            Alb2Off,

            /// <summary>
            /// 辅近光第三颗点亮
            /// </summary>
            [MatrixValDefinition(46, 1, 1)]
            [Description("0x1F6")]
            Alb3On,

            /// <summary>
            /// 辅近光第三颗熄灭
            /// </summary>
            [MatrixValDefinition(46, 1, 0)]
            [Description("0x1F6")]
            Alb3Off,

            /// <summary>
            /// 辅近光第四颗点亮
            /// </summary>
            [MatrixValDefinition(47, 1, 1)]
            [Description("0x1F6")]
            Alb4On,

            /// <summary>
            /// 辅近光第四颗熄灭
            /// </summary>
            [MatrixValDefinition(47, 1, 0)]
            [Description("0x1F6")]
            Alb4Off,

            //[MatrixValDefinition(5, 2, 0)]
            //[Description("0x300")]
            //DhlDirConfDefault,

            //[MatrixValDefinition(5, 2, 1)]
            //[Description("0x300")]
            //DhlDirConfUnder,

            //[MatrixValDefinition(5, 2, 2)]
            //[Description("0x300")]
            //DhlDirConfAbove,

            //[MatrixValDefinition(7, 1, 0)]
            //[Description("0x300")]
            //LdhlRefCmdNotActive,

            //[MatrixValDefinition(7, 1, 1)]
            //[Description("0x300")]
            //LdhlRefCmdActive,

            //[MatrixValDefinition(8, 10, 0)]
            //[Description("0x300")]
            //LdhlConfPosStep0,

            //[MatrixValDefinition(8, 10, 0x68)]
            //[Description("0x300")]
            //LdhlConfPosStep255,

            //[MatrixValDefinition(23, 1, 0)]
            //[Description("0x300")]
            //LaflRefCmdNotActive,

            //[MatrixValDefinition(23, 1, 1)]
            //[Description("0x300")]
            //LaflRefCmdActive,

            //[MatrixValDefinition(24, 11, 0)]
            //[Description("0x300")]
            //LaflConfPosStep0,

            //[MatrixValDefinition(24, 11, 255)]
            //[Description("0x300")]
            //LaflConfPosStep255,

            //[MatrixValDefinition(39, 1, 0)]
            //[Description("0x300")]
            //RaflRefCmdNotActive,

            //[MatrixValDefinition(39, 1, 1)]
            //[Description("0x300")]
            //RafRefCmdActive,

            //[MatrixValDefinition(40, 11, 0)]
            //[Description("0x300")]
            //RaflConfPosStep0,

            //[MatrixValDefinition(40, 11, 255)]
            //[Description("0x300")]
            //RaflConfPosStep255,

            //[MatrixValDefinition(55, 1, 0)]
            //[Description("0x300")]
            //RdhlRefCmdNotActive,

            //[MatrixValDefinition(55, 1, 1)]
            //[Description("0x300")]
            //RdhlRefCmdActive,

            //[MatrixValDefinition(56, 10, 0)]
            //[Description("0x300")]
            //RdhlConfPosStep0,

            //[MatrixValDefinition(56, 10, 255)]
            //[Description("0x300")]
            //RdhlConfPosStep3255,
        }
    }
}
