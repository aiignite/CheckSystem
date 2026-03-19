using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,HA1-Camera")]
    public sealed class Ha1Camera : ControllerBase
    {
        public CanBus Can;

        public Ha1Camera(string name)
            : base(name)
        {
            if (_keepExtendedSessionThread != null)
            {
                _keepExtendedSessionThread.Abort();
                _keepExtendedSessionThread.Join();
            }

            _keepExtendedSessionThread = new Thread(KeepExtendedSession) { IsBackground = true };
            _keepExtendedSessionThread.Start();
        }

        ~Ha1Camera()
        {
            Dispose();
        }

        private readonly Thread _keepExtendedSessionThread;
        private bool _isInExtendedSession;
        private readonly object _lockSend = new object();
        private const uint ReqCanId = 0x646;
        private const uint RecvCanId = 0x647;

        private void KeepExtendedSession()
        {
            while (_keepExtendedSessionThread.IsAlive)
            {
                if (!_keepExtendedSessionThread.IsAlive)
                    break;

                Thread.Sleep(1200);

                if (Can == null)
                    continue;

                if (!_isInExtendedSession)
                    continue;
                lock (_lockSend)
                    Can.SendStandardCanData(ReqCanId, new byte[] { 0x02, 0x3e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            }
        }

        [Description("进入正常模式")]
        public void EnterDefaultSession()
        {
            if (Can == null)
                return;

            lock (_lockSend)
            {
                if (Can.CanBusWithUds.TryEnterExtendedSession(ReqCanId, RecvCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (Can.CanBusWithUds.TryEnterExtendedSession(ReqCanId, RecvCanId, CanBus.CanType.Standard))
                    _isInExtendedSession = true;
            }
        }

        [Description("进入拓展模式")]
        public void EnterExtendedSession()
        {
            if (Can == null)
                return;

            lock (_lockSend)
            {
                if (Can.CanBusWithUds.TryEnterExtendedSession(ReqCanId, RecvCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (Can.CanBusWithUds.TryEnterExtendedSession(ReqCanId, RecvCanId, CanBus.CanType.Standard))
                    _isInExtendedSession = true;
            }
        }

        #region Can读版本号

        [Description("R,摄像头控制线路板总成零件号F187")]
        public string CameraPartNumber;

        [Description("R,系统供应商标识号F18A")]
        public string SystemSupplierIdentificationNo;

        [Description("R,电控单元序列号F18C")]
        public string ElectricalControllerSerialNo;

        [Description("R,系统供应商硬件版本号F193")]
        public string SystemSupplierHardwareVersion;

        [Description("R,系统供应商软件版本号F195")]
        public string SystemSupplierApplicationVersion;

        [Description("R,电控单元CAN矩阵版本F1A2")]
        public string ElectricalControllerCanMatrixVersion;

        [Description("R,CV22配套PMUpowergood_FA03")]
        public string Cv22PmuPowerGood;
        [Description("R,一级5VDCDCPowergood_FA03")]
        public string FirstGrade5VdcdcPowerGood;
        [Description("R,CV22关键电源PWR_CV_1V579_FA03")]
        public string Cv22CriticalVoltagePwrCv1V579;
        [Description("R,DEBOUNCE_GPIO_FA03")]
        public string DebounceGpio;

        [Description("R,KL30电压_FA04")]
        public double Kl30Voltage;

        [Description("R,板卡温度_高温状态_FA05")]
        public string PlateCardHighTemperatureState;
        [Description("R,板卡温度_极低温状态_FA05")]
        public string PlateCardLowTemperatureState;

        [Description("R,PWR_CVSYS_1V8电压状态_FA06")]
        public string PwrCvsys1V8;
        [Description("R,PWR_DDR_1V1电压状态_FA06")]
        public string PwrDdr1V1;
        [Description("R,PWR_CV_VDD_0V75电压状态_FA06")]
        public string PwrCvVdd0V75;
        [Description("R,PWR_CVSYS_3V3电压状态_FA06")]
        public string PwrCvsys3V3;
        [Description("R,PWR_CV_VDDA_0V8电压状态_FA06")]
        public string PwrCvVdda0V8;
        [Description("R,PMIC_VDDIO_3V 电压状态_FA06")]
        public string PmicVddio3V;
        [Description("R,PWR_DDR_1V8 电压状态_FA06")]
        public string PwrDdr1V8;
        [Description("R,PWR_eMMC_1V8 电压状态_FA06")]
        public string PwrEmmc1V8;
        [Description("R,PWR_CV_1V579 电压状态_FA06")]
        public string PwrCv1V579;
        [Description("R,ETH_VDDO_1V0 电压状态_FA06")]
        public string EthVddo1V0;

        [Description("R,前视摄像头高边开关输出电压_FA0E")]
        public double FrontCameraHighSideVolt;
        [Description("R,一级5VDCDC输出电压_FA0F")]
        public double First5VDcdcVolt;

        [Description("R,ADB-camerabuck-boost输出电压检测_FA01")]
        public double AdbCameraBuckBoostVolt;

        [Description("R,休眠唤醒信号线电压_FA02")]
        public double SignalVolt;

        [Description("R,前视摄像头视频_FA00")]
        public string FrontCameraVideo;
        [Description("R,前视摄像头buck-boostpowerGood_FA00")]
        public string FrontCameraBuckBoostPowerGood;
        [Description("R,前视摄像头高边开关状态_FA00")]
        public string FrontCameraHighSideState;
        [Description("R,DMS视频_FA00")]
        public string DmsVideo;
        [Description("R,以太网状态_FA00")]
        public string EthernetState;
        [Description("R,CV22分区检测_FA00")]
        public string Cvv2ZoneState;
        [Description("R,BoardID_FA00")]
        public string BoardId;

        [Description("R,CV22烧写信息_FB01")]
        public string Cv22ProgramState;
        [Description("R,MCU信息_FB01")]
        public string McuInfo;

        [Description("读取摄像头控制线路板总成零件号")]
        public void ReadCameraPartNumber()
        {
            CameraPartNumber = string.Empty;
            CameraPartNumber = ReadDidViaCan(0xF1, 0x87).GetStringByAsciiBytes(false);
        }

        [Description("读系统供应商标识号")]
        public void ReadSystemSupplierIdentificationNo()
        {
            SystemSupplierIdentificationNo = string.Empty;
            SystemSupplierIdentificationNo = ReadDidViaCan(0xF1, 0x8A).GetStringByAsciiBytes(false);
        }

        [Description("读电控单元序列号")]
        public void ReadElectricalControlSerialNo()
        {
            ElectricalControllerSerialNo = string.Empty;
            foreach (var t in ReadDidViaCan(0xFA, 0x01))
                ElectricalControllerSerialNo += ValueHelper.GetHextStr(t);
        }

        [Description("读系统供应商硬件版本号")]
        public void ReadSystemSupplierHardwareVersion()
        {
            SystemSupplierHardwareVersion = string.Empty;
            SystemSupplierHardwareVersion = ReadDidViaCan(0xF1, 0x93).GetStringByAsciiBytes(false);
        }

        [Description("读系统供应商软件版本号")]
        public void ReadSystemSupplierApplicationVersion()
        {
            SystemSupplierApplicationVersion = string.Empty;
            SystemSupplierApplicationVersion = ReadDidViaCan(0xF1, 0x95).GetStringByAsciiBytes(false);
        }

        [Description("读电控单元CAN矩阵版本")]
        public void ReadElectricalControllerCanMatrixVersion()
        {
            ElectricalControllerCanMatrixVersion = string.Empty;
            foreach (var t in ReadDidViaCan(0xFA, 0xA2))
                ElectricalControllerCanMatrixVersion += ValueHelper.GetHextStr(t);
        }

        [Description("读MPU信息")]
        public void ReadMpuInfo(string delayTime)
        {
            FrontCameraVideo = string.Empty;
            FrontCameraBuckBoostPowerGood = string.Empty;
            FrontCameraHighSideState = string.Empty;
            DmsVideo = string.Empty;
            EthernetState = string.Empty;
            Cvv2ZoneState = string.Empty;
            BoardId = string.Empty;

            if (Can == null || string.IsNullOrEmpty(delayTime))
                return;

            int delayTimeI;
            if (!int.TryParse(delayTime, out delayTimeI))
                return;

            if (delayTimeI / 1000 <= 0)
                return;

            var readMpuFunc = new Func<bool>(() =>
            {
                Thread.Sleep(500);
                if (!Can.CanBusWithUds.TryEnterExtendedSession(
                ReqCanId,
                RecvCanId,
                CanBus.CanType.Standard))
                {
                    Thread.Sleep(500);
                    if (!Can.CanBusWithUds.TryEnterExtendedSession(
                        ReqCanId,
                        RecvCanId,
                        CanBus.CanType.Standard))
                    {
                        FrontCameraVideo = "进入拓展模式失败";
                        FrontCameraBuckBoostPowerGood = "进入拓展模式失败";
                        FrontCameraHighSideState = "进入拓展模式失败";
                        DmsVideo = "进入拓展模式失败";
                        EthernetState = "进入拓展模式失败";
                        Cvv2ZoneState = "进入拓展模式失败";
                        BoardId = "进入拓展模式失败";
                        return false;
                    }
                }

                Thread.Sleep(500);
                Can.SendStandardCanData(ReqCanId,
                    new byte[] { 0x03, 0x28, 0x03, 0x01, 0x00, 0x00, 0x00, 0x00 });

                Thread.Sleep(500);
                byte[] seedEcho;
                if (Can.CanBusWithUds.TryRequestSeed(
                    ReqCanId,
                    RecvCanId,
                    CanBus.CanType.Standard, 0x01, out seedEcho))
                {
                    if (seedEcho != null && seedEcho.Length == 4)
                    {
                        var key = CanCalcKey(seedEcho);

                        Thread.Sleep(500);
                        Can.CanBusWithUds.TrySendKey(
                                ReqCanId,
                                RecvCanId,
                                CanBus.CanType.Standard, 0x02, key);

                        Thread.Sleep(500);

                        Can.CanBusWithUds.TryStartRoutineControl(
                            ReqCanId,
                            RecvCanId,
                            CanBus.CanType.Standard, Uds14229Helper.RoutineControl.MpuMode);

                        for (var i = 0; i < delayTimeI / 1000; i++)
                        {
                            Can.SendStandardCanData(
                                ReqCanId,
                                new byte[] { 0x02, 0x3e, 0x00, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA });
                            Thread.Sleep(1000);
                        }

                        Thread.Sleep(500);
                        var result = ReadDidViaCan(0xFA, 0x00);
                        if (result.Length == 5)
                        {
                            var bitStrList = new List<string>();

                            foreach (var r in result)
                            {
                                var bitStr = new string[8];
                                for (var i = 0; i < 8; i++)
                                {
                                    var temp = Convert.ToString(r, 2).PadLeft(8, '0');
                                    bitStr[i] = temp[i].ToString();
                                }
                                Array.Reverse(bitStr);
                                bitStrList.AddRange(bitStr);
                            }

                            FrontCameraVideo = bitStrList[0];
                            FrontCameraBuckBoostPowerGood = bitStrList[1];
                            FrontCameraHighSideState = bitStrList[2];
                            DmsVideo = bitStrList[8];
                            EthernetState = bitStrList[16];
                            Cvv2ZoneState = bitStrList[24];
                            BoardId = string.Format("{0}{1}{2}", bitStrList[34], bitStrList[33], bitStrList[32]);

                            return true;
                        }
                        else
                        {
                            FrontCameraVideo = "读取FA00失败";
                            FrontCameraBuckBoostPowerGood = "读取FA00失败";
                            FrontCameraHighSideState = "读取FA00失败";
                            DmsVideo = "读取FA00失败";
                            EthernetState = "读取FA00失败";
                            Cvv2ZoneState = "读取FA00失败";
                            BoardId = "读取FA00失败";
                            return false;
                        }
                    }
                }
                else
                {
                    FrontCameraVideo = "请求Seed失败";
                    FrontCameraBuckBoostPowerGood = "请求Seed失败";
                    FrontCameraHighSideState = "请求Seed失败";
                    DmsVideo = "请求Seed失败";
                    EthernetState = "请求Seed失败";
                    Cvv2ZoneState = "请求Seed失败";
                    BoardId = "请求Seed失败";
                    return false;
                }

                return false;
            });

            if (readMpuFunc() == false)
            {
                //Thread.Sleep(250);
                //SubCan.CanBusWithUds.TryEnterDefaultSession(ReqCanId,
                //    RecvCanId,
                //    CanBus.CanType.Standard,
                //    CanBus.CanProtocol.Can);
                //readMpuFunc();
            }
        }

        [Description("读ADB-camera-buck-boost-输出电压检测")]
        public void ReadAdbCameraBuckBoostVolt()
        {
            AdbCameraBuckBoostVolt = -9999;
            AdbCameraBuckBoostVolt = ValueHelper.GetDecimal(ReadDidViaCan(0xFA, 0x01));
        }

        [Description("读休眠唤醒信号线电压")]
        public void ReadSignalVolt()
        {
            SignalVolt = -9999;
            SignalVolt = ValueHelper.GetDecimal(ReadDidViaCan(0xFA, 0x02));
        }

        [Description("读电压状态")]
        public void ReadVoltState()
        {
            Cv22PmuPowerGood = string.Empty;
            FirstGrade5VdcdcPowerGood = string.Empty;
            Cv22CriticalVoltagePwrCv1V579 = string.Empty;
            DebounceGpio = string.Empty;

            var result = ReadDidViaCan(0xFA, 0x03);
            if (result.Length == 1)
            {
                var bitStrList = new List<string>();

                foreach (var r in result)
                {
                    var bitStr = new string[8];
                    for (var i = 0; i < 8; i++)
                    {
                        var temp = Convert.ToString(r, 2).PadLeft(8, '0');
                        bitStr[i] = temp[i].ToString();
                    }
                    Array.Reverse(bitStr);
                    bitStrList.AddRange(bitStr);
                }

                Cv22PmuPowerGood = bitStrList[0];
                FirstGrade5VdcdcPowerGood = bitStrList[1];
                Cv22CriticalVoltagePwrCv1V579 = bitStrList[2];
                DebounceGpio = bitStrList[3];
            }
        }

        [Description("读KL30电压")]
        public void ReadKl30Voltage()
        {
            Kl30Voltage = -9999;
            Kl30Voltage = ValueHelper.GetDecimal(ReadDidViaCan(0xFA, 0x04));
        }

        [Description("读板卡温度状态")]
        public void ReadPlateCardTemplateState()
        {
            PlateCardHighTemperatureState = string.Empty;
            PlateCardLowTemperatureState = string.Empty;

            var result = ReadDidViaCan(0xFA, 0x05);
            if (result.Length == 1)
            {
                var bitStrList = new List<string>();

                foreach (var r in result)
                {
                    var bitStr = new string[8];
                    for (var i = 0; i < 8; i++)
                    {
                        var temp = Convert.ToString(r, 2).PadLeft(8, '0');
                        bitStr[i] = temp[i].ToString();
                    }
                    Array.Reverse(bitStr);
                    bitStrList.AddRange(bitStr);
                }

                PlateCardHighTemperatureState = bitStrList[0];
                PlateCardLowTemperatureState = bitStrList[1];
            }
        }

        [Description("读取系统关键电压状态和电压")]
        public void ReadSystemInternalVolt()
        {
            PwrCvsys1V8 = string.Empty;
            PwrDdr1V1 = string.Empty;
            PwrCvVdd0V75 = string.Empty;
            PwrCvsys3V3 = string.Empty;
            PwrCvVdda0V8 = string.Empty;
            PmicVddio3V = string.Empty;
            PwrDdr1V8 = string.Empty;
            PwrEmmc1V8 = string.Empty;
            PwrCv1V579 = string.Empty;
            EthVddo1V0 = string.Empty;

            var result = ReadDidViaCan(0xFA, 0x06);
            if (result.Length == 2)
            {
                var bitStrList = new List<string>();

                foreach (var r in result)
                {
                    var bitStr = new string[8];
                    for (var i = 0; i < 8; i++)
                    {
                        var temp = Convert.ToString(r, 2).PadLeft(8, '0');
                        bitStr[i] = temp[i].ToString();
                    }
                    Array.Reverse(bitStr);
                    bitStrList.AddRange(bitStr);
                }

                PwrCvsys1V8 = bitStrList[0];
                PwrDdr1V1 = bitStrList[1];
                PwrCvVdd0V75 = bitStrList[2];
                PwrCvsys3V3 = bitStrList[3];
                PwrCvVdda0V8 = bitStrList[4];
                PmicVddio3V = bitStrList[5];
                PwrDdr1V8 = bitStrList[6];
                PwrEmmc1V8 = bitStrList[7];
                PwrCv1V579 = bitStrList[8];
                EthVddo1V0 = bitStrList[9];
            }
        }

        [Description("读取固件烧写信息")]
        public void ReadZoneProgramInfo()
        {
            Cv22ProgramState = string.Empty;
            McuInfo = string.Empty;

            var result = ReadDidViaCan(0xFB, 0x01);
            if (result.Length == 1)
            {
                var bitStrList = new List<string>();

                foreach (var r in result)
                {
                    var bitStr = new string[8];
                    for (var i = 0; i < 8; i++)
                    {
                        var temp = Convert.ToString(r, 2).PadLeft(8, '0');
                        bitStr[i] = temp[i].ToString();
                    }
                    Array.Reverse(bitStr);
                    bitStrList.AddRange(bitStr);
                }

                Cv22ProgramState = bitStrList[0];
                McuInfo = bitStrList[1];
            }
        }

        [Description("读前视摄像头高边开关输出电压")]
        public void ReadFrontCameraHighSideVolt()
        {
            FrontCameraHighSideVolt = -9999;
            FrontCameraHighSideVolt = ValueHelper.GetDecimal(ReadDidViaCan(0xFA, 0x0E));
        }

        [Description("一级5VDCDC输出电压")]
        public void ReadFirst5VDcdcVolt()
        {
            First5VDcdcVolt = -9999;
            First5VDcdcVolt = ValueHelper.GetDecimal(ReadDidViaCan(0xFA, 0x0F));
        }

        private static IEnumerable<byte> CanCalcKey(byte[] seedBytes)
        {
            const uint learnmask = 0x56494D53U;
            Array.Reverse(seedBytes);
            var securitySeed = BitConverter.ToUInt32(seedBytes, 0);
            uint securityKey;

            if (securitySeed != 0)
            {
                for (var i = 0; i < 35; i++)
                {
                    if ((securitySeed & 0x80000000) > 0)
                    {
                        securitySeed = securitySeed << 1;
                        securitySeed = securitySeed ^ learnmask;
                    }
                    else
                    {
                        securitySeed = securitySeed << 1;
                    }
                }
                securityKey = securitySeed;
            }
            else
            {
                securityKey = 0;
            }

            return BitConverter.GetBytes(securityKey).Reverse().ToArray();
        }

        private byte[] ReadDidViaCan(byte didHi, byte didLo)
        {
            if (Can == null)
                return new List<byte>().ToArray();

            lock (Can)
            {
                byte[] echoBytes;
                if (Can.CanBusWithUds.TryReadData(
                    ReqCanId,
                    RecvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    Thread.Sleep(250);
                    if (!Can.CanBusWithUds.TryReadData(
                        ReqCanId,
                        RecvCanId,
                        CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                        didHi, didLo, out echoBytes))
                        return new List<byte>().ToArray();
                    return echoBytes ?? new List<byte>().ToArray();
                }

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    ReqCanId,
                    RecvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    ReqCanId,
                    RecvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    ReqCanId,
                    RecvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();
                return echoBytes ?? new List<byte>().ToArray();
            }
        }

        #endregion
    }
}
