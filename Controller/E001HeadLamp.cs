using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,红旗E001头灯")]
    public sealed class E001HeadLamp : ControllerBase
    {
        public CanBus Can;

        public E001HeadLamp(string name)
            : base(name)
        {
            Th = new Thread(MainWork) { IsBackground = true };
            Th.Start();
        }

        private bool _isSleep = true;
        private readonly object _lockCanSend = new object();
        private Thread Th { get; set; }

        ~E001HeadLamp()=> Dispose();

        #region 信号列表

        private readonly CanCommunicationMatrix.IntelMatrix _bcm11 =
            new CanCommunicationMatrix.IntelMatrix(0x230, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _bcm19 =
            new CanCommunicationMatrix.IntelMatrix(0x237, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _bcm_fs_lamp =
            new CanCommunicationMatrix.IntelMatrix(0x1F1, 8);

        private void MainWork()
        {
            var count = 0;

            //var bcmFsLampDatasStr = new List<string>
            //{
            //    "9000000000000000",
            //    "5D00000000000010",
            //    "1700000000000020",
            //    "DA00000000000030",
            //    "8300000000000040",
            //    "4E00000000000050",
            //    "0400000000000060",
            //    "C900000000000070",
            //    "B600000000000080",
            //    "7B00000000000090",
            //    "31000000000000A0",
            //    "FC000000000000B0",
            //    "A5000000000000C0",
            //    "68000000000000D0",
            //    "22000000000000E0",
            //};

            var dataIndexOfBcmFsLamp = 0;
            //var bcmFsLampDatas = new List<byte[]>();
            //foreach (var off in bcmFsLampDatasStr)
            //{
            //    var temp = new List<byte>();
            //    for (var i = 0; i < off.Length; i = i + 2)
            //    {
            //        var b = Convert.ToByte(off.Substring(i, 2), 16);
            //        temp.Add(b);
            //    }
            //    bcmFsLampDatas.Add(temp.ToArray());
            //}

            while (Th.IsAlive)
            {
                if (!Th.IsAlive)
                    break;

                Thread.Sleep(20);

                try
                {
                    if (Can == null)
                        continue;

                    count++;
                    if (count > 2000 * 2000)
                        count = 1;

                    if (_isSleep)
                    {
                        dataIndexOfBcmFsLamp = 0;
                        continue;
                    }

                    lock (_lockCanSend)
                    {
                        var byteCounter = (byte)dataIndexOfBcmFsLamp;
                        _bcm_fs_lamp.UpdateData(
                            new MatrixValDefinition(8, 1, _leftLbCmd ? (byte)0x01 : (byte)0x00));
                        _bcm_fs_lamp.UpdateData(
                            new MatrixValDefinition(9, 1, _rightLbCmd ? (byte)0x01 : (byte)0x00));
                        _bcm_fs_lamp.UpdateData(
                           new MatrixValDefinition(60, 4, byteCounter));

                        var tpCount = byteCounter;
                        var tpCountBits = Convert.ToString(tpCount, 2).PadLeft(4, '0') + "0000";
                        var toCheckCrc = new byte[] { 0xF1, 0x01, _bcm_fs_lamp.MatrixData[1], _bcm_fs_lamp.MatrixData[2], _bcm_fs_lamp.MatrixData[3], _bcm_fs_lamp.MatrixData[4], _bcm_fs_lamp.MatrixData[5], _bcm_fs_lamp.MatrixData[6], Convert.ToByte(tpCountBits, 2) };
                        var crc8 = CALC_CRC(_crcInfo, toCheckCrc);
                        _bcm_fs_lamp.MatrixData[0] = (byte)crc8;

                        var lstPages = new List<CanBus.CanDataPackage> { new CanBus.CanDataPackage(_bcm_fs_lamp.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, _bcm_fs_lamp.MatrixData) };
                        dataIndexOfBcmFsLamp++;
                        if (dataIndexOfBcmFsLamp == 15)
                            dataIndexOfBcmFsLamp = 0;

                        _bcm11.UpdateData(
                            new MatrixValDefinition(19, 1, _turningLightCmdRight ? (byte)0x01 : (byte)0x00));
                        _bcm11.UpdateData(
                            new MatrixValDefinition(45, 1, _flowTurningLightCmdRight ? (byte)0x01 : (byte)0x00));

                        _bcm11.UpdateData(
                            new MatrixValDefinition(18, 1, _turningLightCmdLeft ? (byte)0x01 : (byte)0x00));
                        _bcm11.UpdateData(
                            new MatrixValDefinition(44, 1, _flowTurningLightCmdLeft ? (byte)0x01 : (byte)0x00));

                        _bcm19.UpdateData(new MatrixValDefinition(57, 1, _turningLightCmdLeft ? (byte)0x01 : (byte)0x00));
                        _bcm19.UpdateData(new MatrixValDefinition(58, 1, _turningLightCmdRight ? (byte)0x01 : (byte)0x00));

                        lstPages.Add(
                            new CanBus.CanDataPackage(_bcm11.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, _bcm11.MatrixData));

                        _bcm19.UpdateData(
                            new MatrixValDefinition(1, 1, _positionLightCmdFr ? (byte)0x01 : (byte)0x00));
                        _bcm19.UpdateData(
                            new MatrixValDefinition(3, 1, _dayRunningLightCmdRight ? (byte)0x01 : (byte)0x00));
                        _bcm19.UpdateData(
                            new MatrixValDefinition(22, 1, _flowDayRunningLightCmdRight ? (byte)0x01 : (byte)0x00));

                        _bcm19.UpdateData(
                            new MatrixValDefinition(0, 1, _positionLightCmdFl ? (byte)0x01 : (byte)0x00));
                        _bcm19.UpdateData(
                            new MatrixValDefinition(2, 1, _dayRunningLightCmdLeft ? (byte)0x01 : (byte)0x00));
                        _bcm19.UpdateData(
                            new MatrixValDefinition(21, 1, _flowDayRunningLightCmdLeft ? (byte)0x01 : (byte)0x00));

                        // 右HB
                        _bcm19.UpdateData(
                           new MatrixValDefinition(7, 1, _rightHbCmd ? (byte)0x01 : (byte)0x00));
                        // 左HB
                        _bcm19.UpdateData(
                           new MatrixValDefinition(6, 1, _leftHbCmd ? (byte)0x01 : (byte)0x00));

                        //private int _bCeremonyModeSet = 1;
                        //private int _bCeremonyScenarioSet = 4;
                        _bcm19.UpdateData(
                           new MatrixValDefinition(32, 3, (byte)_bCeremonyModeSet));
                        _bcm19.UpdateData(
                           new MatrixValDefinition(24, 3, (byte)_bCeremonyScenarioSet));

                        lstPages.Add(new CanBus.CanDataPackage(
                            _bcm19.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, _bcm19.MatrixData));

                        //lstPages.Add(new CanBus.CanDataPackage(
                        //   0x232, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]));

                        lstPages.Add(new CanBus.CanDataPackage(
                           0x5A0, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x14, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00 }));

                        lstPages.Add(new CanBus.CanDataPackage(
                            0x2B0, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00 }));

                        lstPages.Add(new CanBus.CanDataPackage(
                            0x580, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0X00, 0x00, 0x00, 0x00, 0x00 }));

                        Can.SendCanDatas(lstPages.ToArray());
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        #endregion

        #region 点灯

        [Description("R/W,TurningLightCmd_Right")]
        private bool _turningLightCmdRight;
        [Description("R/W,FlowTurningLightCmd_Right")]
        private bool _flowTurningLightCmdRight;
        [Description("R/W,DayRunningLightCmd_Right")]
        private bool _dayRunningLightCmdRight;
        [Description("R/W,FlowDayRunningLightCmd_Right")]
        private bool _flowDayRunningLightCmdRight;
        [Description("R/W,PositionLightCmd_FR")]
        private bool _positionLightCmdFr;

        private bool _leftLbCmd;
        private bool _rightLbCmd;
        private bool _leftHbCmd;
        private bool _rightHbCmd;

        [Description("唤醒")]
        public void ModuleAwake() => _isSleep = false;

        [Description("休眠")]
        public void ModuleSleep()=> _isSleep = true;

        [Description("左近光打开")]
        public void LeftLowBeamOn()=> _leftLbCmd = true;

        [Description("左近光关闭")]
        public void LeftLowBeamOff() => _leftLbCmd = false;

        [Description("右近光打开")]
        public void RightLowBeamOn() => _rightLbCmd = true;

        [Description("右近光关闭")]
        public void RightLowBeamOff() => _rightLbCmd = false;

        [Description("左远光打开")]
        public void LeftHighBeamOn()=> _leftHbCmd = true;

        [Description("左远光关闭")]
        public void LeftHighBeamOff()=> _leftHbCmd = false;

        [Description("右远光打开")]
        public void RightHighBeamOn()=> _rightHbCmd = true;

        [Description("右远光关闭")]
        public void RightHighBeamOff() => _rightHbCmd = false;

        [Description("右转向打开")]
        public void RightTurningLightOn()
        {
            _flowTurningLightCmdRight = false;
            _turningLightCmdRight = true;
        }

        [Description("右转向时序打开")]
        public void RightFlowTurningLightOn()
        {
            _flowTurningLightCmdRight = true;
            _turningLightCmdRight = true;
        }

        [Description("右转向关闭")]
        public void RightTurnLightOff()
        {
            _flowTurningLightCmdRight = false;
            _turningLightCmdRight = false;
        }

        [Description("右日行灯打开")]
        public void RightDayRunningLightOn()
        {
            _flowDayRunningLightCmdRight = false;
            _dayRunningLightCmdRight = true;
        }

        [Description("右日行灯关闭")]
        public void RightDayRunningLightOff()
        {
            _flowDayRunningLightCmdRight = false;
            _dayRunningLightCmdRight = false;
        }

        [Description("右位置灯打开")]
        public void RightPositionLightOn() => _positionLightCmdFr = true;

        [Description("右位置灯关闭")]
        public void RightPositionLightOff() => _positionLightCmdFr = false;

        [Description("R/W,TurningLightCmd_Left")]
        private bool _turningLightCmdLeft;
        [Description("R/W,FlowTurningLightCmd_Left")]
        private bool _flowTurningLightCmdLeft;
        [Description("R/W,DayRunningLightCmd_Left")]
        private bool _dayRunningLightCmdLeft;
        [Description("R/W,FlowDayRunningLightCmd_Left")]
        private bool _flowDayRunningLightCmdLeft;
        [Description("R/W,PositionLightCmd_FL")]
        private bool _positionLightCmdFl;

        [Description("左转向打开")]
        public void LeftTurningLightOn()
        {
            _flowTurningLightCmdLeft = false;
            _turningLightCmdLeft = true;
        }

        [Description("左转向时序打开")]
        public void LeftFlowTurningLightOn()
        {
            _flowTurningLightCmdLeft = true;
            _turningLightCmdLeft = true;
        }

        [Description("左转向关闭")]
        public void LeftTurnLightOff()
        {
            _flowTurningLightCmdLeft = false;
            _turningLightCmdLeft = false;
        }

        [Description("左日行灯打开")]
        public void LeftDayRunningLightOn()
        {
            _flowDayRunningLightCmdLeft = false;
            _dayRunningLightCmdLeft = true;
        }

        //[Description("左日行灯时序打开")]
        //public void LeftFlowDayRunningLightOn()
        //{
        //    FlowDayRunningLightCmd_Left = true;
        //    DayRunningLightCmd_Left = true;
        //}

        [Description("左日行灯关闭")]
        public void LeftDayRunningLightOff()
        {
            _flowDayRunningLightCmdLeft = false;
            _dayRunningLightCmdLeft = false;
        }

        [Description("左位置灯打开")]
        public void LeftPositionLightOn() => _positionLightCmdFl = true;

        [Description("左位置灯关闭")]
        public void LeftPositionLightOff() => _positionLightCmdFl = false;

        private int _bCeremonyModeSet = 1;
        private int _bCeremonyScenarioSet = 4;

        [Description("闭锁-LockCar-mode1~5")]
        public void LockCar(int mode)
        {
            if (mode >= 1 && mode <= 5)
            {
                _bCeremonyModeSet = mode + 1;
                _bCeremonyScenarioSet = 2;
            }
        }

        [Description("解锁-UnlockCar-mode1~5")]
        public void UnlockCar(int mode)
        {
            if (mode >= 1 && mode <= 5)
            {
                _bCeremonyModeSet = mode + 1;
                _bCeremonyScenarioSet = 1;
            }
        }

        [Description("LightingShow-mode1~5")]
        public void LightingShow(int mode)
        {
            if (mode >= 1 && mode <= 5)
            {
                _bCeremonyModeSet = mode + 1;
                _bCeremonyScenarioSet = 3;
            }
        }

        [Description("停止动画")]
        public void AbortAnimotion()
        {
            _bCeremonyModeSet = 1;
            _bCeremonyScenarioSet = 4;
        }

        private uint CALC_CRC(CrcHelper.CrcInfo info, IReadOnlyList<byte> memBlock)
        {
            var memBlockLen = (uint)memBlock.Count;
            uint value;

            if (info.Refin)
            {
                value = BitReflected(info.InitReg, info.Width);
                if (info.Width > 8)
                {
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = (value >> 8) ^ _table[value & 0xff ^ memBlock[i++]];
                    }
                }
                else
                {
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = _table[value ^ memBlock[i++]];
                    }
                }
            }
            else
            {
                if (info.Width > 8)
                {
                    value = info.InitReg;
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        var high = (byte)(value >> (info.Width - 8));
                        value = (value << 8) ^ _table[high ^ memBlock[i++]];
                    }
                }
                else
                {
                    value = info.InitReg << (8 - info.Width);
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = _table[value ^ memBlock[i++]];
                    }
                    value >>= 8 - info.Width;
                }
            }
            if (info.Refout != info.Refin)
            {
                value = BitReflected(value, info.Width);
            }
            value ^= info.Xorout;
            return value & (((uint)2 << (info.Width - 1)) - 1);
        }

        private static uint BitReflected(uint input, byte bits)
        {
            uint res = 0;
            while (bits-- > 0)
            {
                res <<= 1;
                if ((input & 0x01) != 0)
                    res |= 1;
                input >>= 1;
            }
            return res;
        }

        private readonly CrcHelper.CrcInfo _crcInfo = new CrcHelper.CrcInfo
        {
            Width = 8,
            Poly = 0x1D,
            InitReg = 0x00,
            Refin = false,
            Refout = false,
            Xorout = 0x00,
        };

        private readonly uint[] _table = new uint[256] {
            0x00, 0x1D, 0x3A, 0x27, 0x74, 0x69, 0x4E, 0x53, 0xE8, 0xF5, 0xD2, 0xCF, 0x9C, 0x81, 0xA6, 0xBB,
            0xCD, 0xD0, 0xF7, 0xEA, 0xB9, 0xA4, 0x83, 0x9E, 0x25, 0x38, 0x1F, 0x02, 0x51, 0x4C, 0x6B, 0x76,
            0x87, 0x9A, 0xBD, 0xA0, 0xF3, 0xEE, 0xC9, 0xD4, 0x6F, 0x72, 0x55, 0x48, 0x1B, 0x06, 0x21, 0x3C,
            0x4A, 0x57, 0x70, 0x6D, 0x3E, 0x23, 0x04, 0x19, 0xA2, 0xBF, 0x98, 0x85, 0xD6, 0xCB, 0xEC, 0xF1,
            0x13, 0x0E, 0x29, 0x34, 0x67, 0x7A, 0x5D, 0x40, 0xFB, 0xE6, 0xC1, 0xDC, 0x8F, 0x92, 0xB5, 0xA8,
            0xDE, 0xC3, 0xE4, 0xF9, 0xAA, 0xB7, 0x90, 0x8D, 0x36, 0x2B, 0x0C, 0x11, 0x42, 0x5F, 0x78, 0x65,
            0x94, 0x89, 0xAE, 0xB3, 0xE0, 0xFD, 0xDA, 0xC7, 0x7C, 0x61, 0x46, 0x5B, 0x08, 0x15, 0x32, 0x2F,
            0x59, 0x44, 0x63, 0x7E, 0x2D, 0x30, 0x17, 0x0A, 0xB1, 0xAC, 0x8B, 0x96, 0xC5, 0xD8, 0xFF, 0xE2,
            0x26, 0x3B, 0x1C, 0x01, 0x52, 0x4F, 0x68, 0x75, 0xCE, 0xD3, 0xF4, 0xE9, 0xBA, 0xA7, 0x80, 0x9D,
            0xEB, 0xF6, 0xD1, 0xCC, 0x9F, 0x82, 0xA5, 0xB8, 0x03, 0x1E, 0x39, 0x24, 0x77, 0x6A, 0x4D, 0x50,
            0xA1, 0xBC, 0x9B, 0x86, 0xD5, 0xC8, 0xEF, 0xF2, 0x49, 0x54, 0x73, 0x6E, 0x3D, 0x20, 0x07, 0x1A,
            0x6C, 0x71, 0x56, 0x4B, 0x18, 0x05, 0x22, 0x3F, 0x84, 0x99, 0xBE, 0xA3, 0xF0, 0xED, 0xCA, 0xD7,
            0x35, 0x28, 0x0F, 0x12, 0x41, 0x5C, 0x7B, 0x66, 0xDD, 0xC0, 0xE7, 0xFA, 0xA9, 0xB4, 0x93, 0x8E,
            0xF8, 0xE5, 0xC2, 0xDF, 0x8C, 0x91, 0xB6, 0xAB, 0x10, 0x0D, 0x2A, 0x37, 0x64, 0x79, 0x5E, 0x43,
            0xB2, 0xAF, 0x88, 0x95, 0xC6, 0xDB, 0xFC, 0xE1, 0x5A, 0x47, 0x60, 0x7D, 0x2E, 0x33, 0x14, 0x09,
            0x7F, 0x62, 0x45, 0x58, 0x0B, 0x16, 0x31, 0x2C, 0x97, 0x8A, 0xAD, 0xB0, 0xE3, 0xFE, 0xD9, 0xC4
        };

        #endregion

        #region DTC相关

        private readonly List<string> _blackList = new List<string>();

        [Description("将DTC添加进黑名单")]
        public void AddDtcCodeIntoBlackList(string code)
        {
            var index = _blackList.FindIndex(f => string.Equals(f, code, StringComparison.CurrentCultureIgnoreCase));
            if (index == -1)
                _blackList.Add(code.Trim().ToUpper());
        }

        [Description("清空黑名单")]
        public void ClearBlackList()
        {
            _blackList.Clear();
        }

        [Description("R,诊断-读取左灯DTC结果")]
        public string ReadDtcLResult;

        [Description("读左灯DTC")]
        public void ReadDtcL()
        {
            ReadDtcLResult = string.Empty;

            if (Can == null)
                return;

            lock (_lockCanSend)
            {
                byte[] echo;
                if (Can.CanBusWithUds.TryReadDtcInfomation(
                        0x705, 0x70D, CanBus.CanType.Standard, 0x02, 0xFF,
                        out echo))
                {
                    if (echo != null)
                    {
                        if (echo.Length % 4 == 0)
                        {
                            var readCodes = new List<Uds14229Helper.DtcData>();

                            for (var i = 0; i < echo.Length; i = i + 4)
                            {
                                var dtcData = new Uds14229Helper.DtcData(echo[i], echo[i + 1], echo[i + 2], echo[i + 3]);
                                readCodes.Add(dtcData);
                            }

                            foreach (var t in readCodes)
                                Console.WriteLine(t.Remark);

                            var codeInBlackList = FilterCodeNotInBlackList(readCodes);

                            if (codeInBlackList.Any())
                            {
                                foreach (var t in codeInBlackList)
                                    ReadDtcLResult += string.Format("[{0}];", t.Remark);
                            }
                            else
                            {
                                ReadDtcLResult = "NoErrorExceptBlackList";
                            }
                        }
                        else
                        {
                            ReadDtcLResult = "ReadDtcResLenError";
                        }
                    }
                }
                else
                {
                    ReadDtcLResult = "NoRead";
                }
            }
        }

        [Description("R,清除左灯错误结果")]
        public string ClearFaultLResult;

        [Description("清除左灯错误")]
        public void ClearLFault()
        {
            ClearFaultLResult = string.Empty;

            if (Can == null)
                return;

            lock (_lockCanSend)
            {
                if (Can.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(
                        0x705, 0x70D, CanBus.CanType.Standard))
                {
                    ClearFaultLResult = @"OK";
                }
            }
        }

        [Description("R,诊断-读取右灯DTC结果")]
        public string ReadDtcRResult;

        [Description("读右灯DTC")]
        public void ReadDtcR()
        {
            ReadDtcRResult = string.Empty;

            if (Can == null)
                return;

            lock (_lockCanSend)
            {
                byte[] echo;
                if (Can.CanBusWithUds.TryReadDtcInfomation(
                        0x7B3, 0x7BB, CanBus.CanType.Standard, 0x02, 0xFF, out echo))
                {
                    if (echo != null)
                    {
                        if (echo.Length % 4 == 0)
                        {
                            var readCodes = new List<Uds14229Helper.DtcData>();

                            for (var i = 0; i < echo.Length; i = i + 4)
                            {
                                var dtcData = new Uds14229Helper.DtcData(echo[i], echo[i + 1], echo[i + 2], echo[i + 3]);
                                readCodes.Add(dtcData);
                            }

                            foreach (var t in readCodes)
                                Console.WriteLine(t.Remark);

                            var codeInBlackList = FilterCodeNotInBlackList(readCodes);

                            if (codeInBlackList.Any())
                            {
                                foreach (var t in codeInBlackList)
                                    ReadDtcRResult += string.Format("[{0}];", t.Remark);
                            }
                            else
                            {
                                ReadDtcRResult = "NoErrorExceptBlackList";
                            }
                        }
                        else
                        {
                            ReadDtcRResult = "ReadDtcResLenError";
                        }
                    }
                }
                else
                {
                    ReadDtcRResult = "NoRead";
                }
            }
        }

        [Description("R,清除右灯错误结果")]
        public string ClearFaultRResult;

        [Description("清除右灯错误")]
        public void ClearRFault()
        {
            ClearFaultRResult = string.Empty;

            if (Can == null)
                return;

            lock (_lockCanSend)
            {
                if (Can.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(
                        0x7B3, 0x7BB, CanBus.CanType.Standard))
                {
                    ClearFaultRResult = @"OK";
                }
            }
        }

        [Description("将DRL/FTSL的DTC添加进黑名单")]
        public void AddDrlFtslDtcCodeIntoBlackList()
        {
            _blackList.Add("B151814");
            _blackList.Add("B15181E");
            _blackList.Add("B151911");
            _blackList.Add("B151912");
            _blackList.Add("B151913");
            _blackList.Add("B151A14");
            _blackList.Add("B151A1E");
            _blackList.Add("B151B11");
            _blackList.Add("B151B12");
            _blackList.Add("B151B13");
            _blackList.Add("B151C14");
            _blackList.Add("B151C1E");
            _blackList.Add("B151D11");
            _blackList.Add("B151D12");
            _blackList.Add("B151D13");
            _blackList.Add("B152B04");
            _blackList.Add("B152C04");
            _blackList.Add("B152E01");
            _blackList.Add("B152F01");
            _blackList.Add("U002888");
            _blackList.Add("U014687");
        }

        private List<Uds14229Helper.DtcData> FilterCodeNotInBlackList(
            IEnumerable<Uds14229Helper.DtcData> toFilterDtcCodes)
        {
            return (from t in toFilterDtcCodes
                    let findIndex =
                        _blackList.FindIndex(f => string.Equals(f, t.Code, StringComparison.CurrentCultureIgnoreCase))
                    where findIndex >= 0
                    select t).ToList();
        }

        #endregion
    }
}
