using CommonUtility;
using CommonUtility.BusLoader;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,东风岚图H77")]
    public sealed class DongFengH77RearLamp : ControllerBase
    {
        public CanBus Can;

        public DongFengH77RearLamp(string name) : base(name)
        {
            _dicErrorStartTs.Add("RLM_LeftPositionLampFault", 0);
            _dicErrorStartTs.Add("RLM_LeftTurnLampFault  ", 0);
            _dicErrorStartTs.Add("RLM_LeftStopLampFault", 0);
            _dicErrorStartTs.Add("RLM_LeftRearfogLampFault ", 0);

            _dicErrorStartTs.Add("RLM_RightPositionLampFault", 0);
            _dicErrorStartTs.Add("RLM_RightTurnLampFault  ", 0);
            _dicErrorStartTs.Add("RLM_RightStopLampFault", 0);
            _dicErrorStartTs.Add("RLM_RightRearfogLampFault ", 0);

            _dicErrorStartTs.Add("RLM_MidApositionLampFault", 0);
            _dicErrorStartTs.Add("RLM_MidLeftTurnLampFault", 0);
            _dicErrorStartTs.Add("RLM_MidLeftStopLampFault", 0);
            _dicErrorStartTs.Add("RLM_LicenseLampFault", 0);
            _dicErrorStartTs.Add("RLM_HighStopLampFault", 0);
            _dicErrorStartTs.Add("RLM_MidRightTurnLampFault", 0);
            _dicErrorStartTs.Add("RLM_AdasBlueLampFault", 0);
            _dicErrorStartTs.Add("RLM_ReversingLampFault", 0);

            _dicErrorInvoke.Add("RLM_LeftPositionLampFault", 0);
            _dicErrorInvoke.Add("RLM_LeftTurnLampFault  ", 0);
            _dicErrorInvoke.Add("RLM_LeftStopLampFault", 0);
            _dicErrorInvoke.Add("RLM_LeftRearfogLampFault ", 0);

            _dicErrorInvoke.Add("RLM_RightPositionLampFault", 0);
            _dicErrorInvoke.Add("RLM_RightTurnLampFault  ", 0);
            _dicErrorInvoke.Add("RLM_RightStopLampFault", 0);
            _dicErrorInvoke.Add("RLM_RightRearfogLampFault ", 0);

            _dicErrorInvoke.Add("RLM_MidApositionLampFault", 0);
            _dicErrorInvoke.Add("RLM_MidLeftTurnLampFault", 0);
            _dicErrorInvoke.Add("RLM_MidLeftStopLampFault", 0);
            _dicErrorInvoke.Add("RLM_LicenseLampFault", 0);
            _dicErrorInvoke.Add("RLM_HighStopLampFault", 0);
            _dicErrorInvoke.Add("RLM_MidRightTurnLampFault", 0);
            _dicErrorInvoke.Add("RLM_AdasBlueLampFault", 0);
            _dicErrorInvoke.Add("RLM_ReversingLampFault", 0);

            CreateLocalBindingDb();
            SysConfig();
            InitExpectedUartCanLog();
            CanBus.PushCanMsg += CanBus_PushCanMsg;
            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;
        }

        ~DongFengH77RearLamp() => Dispose();

        #region 点灯

        [Description("开启CAN消息")]
        public void StartCanMsg() => IsCanStarted = true;

        [Description("关闭CAN消息")]
        public void StopCanMsg() => IsCanStarted = false;

        private void SysConfig() => Start();

        private void Start()
        {
            SetTimer(new MyTaskScheduler.TaskInfo { Action = Tmr_Refresh20Ms(), Interval = 10 });
            SetTimer(new MyTaskScheduler.TaskInfo { Action = Tmr_Refresh10Ms(), Interval = 20 });
            SetTimer(new MyTaskScheduler.TaskInfo { Action = Tmr_Refresh100Ms(), Interval = 100 });
            SetTimer(new MyTaskScheduler.TaskInfo { Action = Tmr_Refresh400Ms(), Interval = 400 });
            SchedulerAsync();
        }

        private Action Tmr_Refresh20Ms()
        {
            return () =>
            {
                var toCheckCrc = new List<byte>
                {
                    0x02,
                    0xA1
                };

                var matrixData = new CanCommunicationMatrix.MotorolaMatrix(0x2A1, 8);
                matrixData.UpdateData(new MatrixValDefinition(0, 2, BCM_RMPosLampReq));
                matrixData.UpdateData(new MatrixValDefinition(2, 2, BCM_FLdrlReq));
                matrixData.UpdateData(new MatrixValDefinition(9, 3, BCM_EXLHeightAdjtReq));
                matrixData.UpdateData(new MatrixValDefinition(12, 2, BCM_ADBModeReq));
                matrixData.UpdateData(new MatrixValDefinition(14, 2, BCM_FLPosLampReq));
                matrixData.UpdateData(new MatrixValDefinition(16, 2, BCM_FRdrlReq));
                matrixData.UpdateData(new MatrixValDefinition(18, 2, BCM_FRPosLampReq));
                matrixData.UpdateData(new MatrixValDefinition(20, 2, BCM_LowBeamReq));
                matrixData.UpdateData(new MatrixValDefinition(22, 2, BCM_HighBeamReq));
                matrixData.UpdateData(new MatrixValDefinition(26, 2, BCM_RearfoglampReq));
                if (BCM_LeftTurnLampReq == 2)
                {
                    if (_turn400msSignal == 1)
                    {
                        matrixData.UpdateData(new MatrixValDefinition(28, 2, BCM_LeftTurnLampReq));
                    }
                    else if (_turn400msSignal == 2)
                    {
                        matrixData.UpdateData(new MatrixValDefinition(28, 2, 0x01));
                    }
                    else
                    {
                        matrixData.UpdateData(new MatrixValDefinition(28, 2, BCM_LeftTurnLampReq));
                    }
                }
                else
                {
                    matrixData.UpdateData(new MatrixValDefinition(28, 2, BCM_LeftTurnLampReq));
                }

                if (BCM_RightTurnLampReq == 2)
                {
                    if (_turn400msSignal == 1)
                    {
                        matrixData.UpdateData(new MatrixValDefinition(30, 2, BCM_RightTurnLampReq));
                    }
                    else if (_turn400msSignal == 2)
                    {
                        matrixData.UpdateData(new MatrixValDefinition(30, 2, 0x01));
                    }
                    else
                    {
                        matrixData.UpdateData(new MatrixValDefinition(30, 2, BCM_RightTurnLampReq));
                    }
                }
                else
                {
                    matrixData.UpdateData(new MatrixValDefinition(30, 2, BCM_RightTurnLampReq));
                }

                matrixData.UpdateData(new MatrixValDefinition(32, 2, BCM_RLPosLampReq));
                matrixData.UpdateData(new MatrixValDefinition(34, 2, BCM_RRPosLampReq));
                matrixData.UpdateData(new MatrixValDefinition(36, 2, BCM_turnLampFlickerReq));
                matrixData.UpdateData(new MatrixValDefinition(38, 2, BCM_turnLampSerialReq));
                matrixData.UpdateData(new MatrixValDefinition(40, 2, BCM_ReversinglampReq));
                matrixData.UpdateData(new MatrixValDefinition(42, 2, BCM_StoplampReq));
                matrixData.UpdateData(new MatrixValDefinition(44, 2, BCM_LicenseLampReq));
                matrixData.UpdateData(new MatrixValDefinition(48, 4, RollingCounter2A1));

                var tpToCheckData = new byte[7];
                Array.Copy(matrixData.MatrixData, 0, tpToCheckData, 0, 7);
                toCheckCrc.AddRange(tpToCheckData);

                Checksum2A1 = crc_8find(toCheckCrc.ToArray(), toCheckCrc.Count);
                matrixData.UpdateData(new MatrixValDefinition(56, 8, Checksum2A1));

                RollingCounter2A1++;
                if (RollingCounter2A1 > 15)
                    RollingCounter2A1 = 0;

                SendCanMsg(new CanBus.CanDataPackage[]
                {
                    new CanBus.CanDataPackage(matrixData.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, matrixData.MatrixData),
                });
            };
        }

        private Action Tmr_Refresh10Ms()
        {
            return () =>
            {
                var matrixData = new CanCommunicationMatrix.MotorolaMatrix(0x62, 8);
                matrixData.UpdateData(new MatrixValDefinition(0, 2, BCM_AdasBlueLampReq));

                SendCanMsg(new CanBus.CanDataPackage[]
                {
                    new CanBus.CanDataPackage(matrixData.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, matrixData.MatrixData),
                });
            };
        }

        private Action Tmr_Refresh100Ms()
        {
            return () =>
            {
                var matrixData = new CanCommunicationMatrix.MotorolaMatrix(0x3C0, 8);
                matrixData.UpdateData(new MatrixValDefinition(34, 4, BCM_WelcomeLightReq));

                var matrixData2 = new CanCommunicationMatrix.MotorolaMatrix(0x201, 8);
                matrixData2.UpdateData(new MatrixValDefinition(30, 3, BCM_RearLogoReq));
                matrixData2.UpdateData(new MatrixValDefinition(41, 3, BCM_ParkLightReq));

                SendCanMsg(new CanBus.CanDataPackage[]
                {
                    new CanBus.CanDataPackage(matrixData.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, matrixData.MatrixData),
                    new CanBus.CanDataPackage(matrixData2.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, matrixData2.MatrixData),
                });
            };
        }

        private void SendCanMsg(CanBus.CanDataPackage[] canDatas)
        {
            if (canDatas is null || Can is null || !IsCanStarted)
                return;

            lock (_lockCanSend)
                Can.SendCanDatas(canDatas);
        }

        private Action Tmr_Refresh400Ms()
        {
            return () =>
            {
                if (_turn400msSignal == 1)
                    _turn400msSignal = 2;
                else if (_turn400msSignal == 2)
                    _turn400msSignal = 1;
            };
        }

        private bool IsCanStarted;
        private readonly object _lockCanSend = new object();

        [Description("R/W,BCM_RMPosLampReq")]
        private byte BCM_RMPosLampReq = 1;
        [Description("R/W,BCM_FLdrlReq")]
        private byte BCM_FLdrlReq;
        [Description("R/W,BCM_EXLHeightAdjtReq")]
        private byte BCM_EXLHeightAdjtReq;
        [Description("R/W,BCM_ADBModeReq")]
        private byte BCM_ADBModeReq;
        [Description("R/W,BCM_FLPosLampReq")]
        private byte BCM_FLPosLampReq;
        [Description("R/W,BCM_FRdrlReq")]
        private byte BCM_FRdrlReq;
        [Description("R/W,BCM_FRPosLampReq")]
        private byte BCM_FRPosLampReq;
        [Description("R/W,BCM_LowBeamReq")]
        private byte BCM_LowBeamReq;
        [Description("R/W,BCM_HighBeamReq")]
        private byte BCM_HighBeamReq;
        [Description("R/W,BCM_RearfoglampReq")]
        private byte BCM_RearfoglampReq = 1;
        [Description("R/W,BCM_LeftTurnLampReq")]
        private byte BCM_LeftTurnLampReq = 1;
        [Description("R/W,BCM_RightTurnLampReq")]
        private byte BCM_RightTurnLampReq = 1;
        [Description("R/W,BCM_RLPosLampReq")]
        private byte BCM_RLPosLampReq = 1;
        [Description("R/W,BCM_RRPosLampReq")]
        private byte BCM_RRPosLampReq = 1;
        [Description("R/W,BCM_turnLampFlickerReq")]
        private byte BCM_turnLampFlickerReq;
        [Description("R/W,BCM_turnLampSerialReq")]
        private byte BCM_turnLampSerialReq;
        [Description("R/W,BCM_ReversinglampReq")]
        private byte BCM_ReversinglampReq = 1;
        [Description("R/W,BCM_StoplampReq")]
        private byte BCM_StoplampReq = 1;
        [Description("R/W,BCM_LicenseLampReq")]
        private byte BCM_LicenseLampReq;
        private byte RollingCounter2A1;
        private byte Checksum2A1;

        private byte BCM_AdasBlueLampReq = 1;
        private byte BCM_RearLogoReq = 1;
        private byte BCM_WelcomeLightReq = 1;
        private byte BCM_ParkLightReq = 1;

        private byte[] crc_table = new byte[]{
            0x00, 0x1D, 0x3A, 0x27, 0x74, 0x69, 0x4E, 0x53,
            0xE8, 0xF5, 0xD2, 0xCF, 0x9C, 0x81, 0xA6, 0xBB,
            0xCD, 0xD0, 0xF7, 0xEA, 0xB9, 0xA4, 0x83, 0x9E,
            0x25, 0x38, 0x1F, 0x02, 0x51, 0x4C, 0x6B, 0x76,
            0x87, 0x9A, 0xBD, 0xA0, 0xF3, 0xEE, 0xC9, 0xD4,
            0x6F, 0x72, 0x55, 0x48, 0x1B, 0x06, 0x21, 0x3C,
            0x4A, 0x57, 0x70, 0x6D, 0x3E, 0x23, 0x04, 0x19,
            0xA2, 0xBF, 0x98, 0x85, 0xD6, 0xCB, 0xEC, 0xF1,
            0x13, 0x0E, 0x29, 0x34, 0x67, 0x7A, 0x5D, 0x40,
            0xFB, 0xE6, 0xC1, 0xDC, 0x8F, 0x92, 0xB5, 0xA8,
            0xDE, 0xC3, 0xE4, 0xF9, 0xAA, 0xB7, 0x90, 0x8D,
            0x36, 0x2B, 0x0C, 0x11, 0x42, 0x5F, 0x78, 0x65,
            0x94, 0x89, 0xAE, 0xB3, 0xE0, 0xFD, 0xDA, 0xC7,
            0x7C, 0x61, 0x46, 0x5B, 0x08, 0x15, 0x32, 0x2F,
            0x59, 0x44, 0x63, 0x7E, 0x2D, 0x30, 0x17, 0x0A,
            0xB1, 0xAC, 0x8B, 0x96, 0xC5, 0xD8, 0xFF, 0xE2,
            0x26, 0x3B, 0x1C, 0x01, 0x52, 0x4F, 0x68, 0x75,
            0xCE, 0xD3, 0xF4, 0xE9, 0xBA, 0xA7, 0x80, 0x9D,
            0xEB, 0xF6, 0xD1, 0xCC, 0x9F, 0x82, 0xA5, 0xB8,
            0x03, 0x1E, 0x39, 0x24, 0x77, 0x6A, 0x4D, 0x50,
            0xA1, 0xBC, 0x9B, 0x86, 0xD5, 0xC8, 0xEF, 0xF2,
            0x49, 0x54, 0x73, 0x6E, 0x3D, 0x20, 0x07, 0x1A,
            0x6C, 0x71, 0x56, 0x4B, 0x18, 0x05, 0x22, 0x3F,
            0x84, 0x99, 0xBE, 0xA3, 0xF0, 0xED, 0xCA, 0xD7,
            0x35, 0x28, 0x0F, 0x12, 0x41, 0x5C, 0x7B, 0x66,
            0xDD, 0xC0, 0xE7, 0xFA, 0xA9, 0xB4, 0x93, 0x8E,
            0xF8, 0xE5, 0xC2, 0xDF, 0x8C, 0x91, 0xB6, 0xAB,
            0x10, 0x0D, 0x2A, 0x37, 0x64, 0x79, 0x5E, 0x43,
            0xB2, 0xAF, 0x88, 0x95, 0xC6, 0xDB, 0xFC, 0xE1,
            0x5A, 0x47, 0x60, 0x7D, 0x2E, 0x33, 0x14, 0x09,
            0x7F, 0x62, 0x45, 0x58, 0x0B, 0x16, 0x31, 0x2C,
            0x97, 0x8A, 0xAD, 0xB0, 0xE3, 0xFE, 0xD9, 0xC4
        };

        private byte crc_8find(byte[] data, int len)
        {
            byte crc8 = 0x00;
            var dIndex = 0;
            while (len-- > 0)
            {
                crc8 = crc_table[crc8 ^ data[dIndex]];
                dIndex++;
            }
            return crc8;
        }

        #region 尾灯

        [Description("B灯位置灯开")]
        public void RMRearLampOn() => BCM_RMPosLampReq = 2;

        [Description("B灯位置灯关")]
        public void RMRearLampOff() => BCM_RMPosLampReq = 1;

        [Description("左尾灯开")]
        public void LeftRearLampOn() => BCM_RLPosLampReq = 2;

        [Description("左尾灯关")]
        public void LeftRearLampOff() => BCM_RLPosLampReq = 1;

        [Description("右尾灯开")]
        public void RightRearLampOn() => BCM_RRPosLampReq = 2;

        [Description("右尾灯关")]
        public void RightRearLampOff() => BCM_RRPosLampReq = 1;

        [Description("制动灯开")]
        public void StopOn() => BCM_StoplampReq = 2;

        [Description("制动灯关")]
        public void StopOff() => BCM_StoplampReq = 1;

        [Description("雾灯开")]
        public void FogOn() => BCM_RearfoglampReq = 2;

        [Description("雾灯关")]
        public void FogOff() => BCM_RearfoglampReq = 1;

        [Description("倒车灯开")]
        public void ReversingOn() => BCM_ReversinglampReq = 2;

        [Description("倒车灯关")]
        public void ReversingOff() => BCM_ReversinglampReq = 1;

        [Description("牌照灯开")]
        public void LicenseLampOn() => BCM_LicenseLampReq = 2;

        [Description("牌照灯关")]
        public void LicenseLampOff() => BCM_LicenseLampReq = 1;

        [Description("左转开")]
        public void LeftTurnOn() => BCM_LeftTurnLampReq = 2;

        [Description("左转关")]
        public void LeftTurnOff() => BCM_LeftTurnLampReq = 1;

        [Description("右转开")]
        public void RightTurnOn() => BCM_RightTurnLampReq = 2;

        [Description("右转关")]
        public void RightTurnOff() => BCM_RightTurnLampReq = 1;

        [Description("转向流水使能")]
        public void TurnLampSerialOn() => BCM_turnLampSerialReq = 1;

        [Description("转向流水失能")]
        public void TurnLampSerialOff() => BCM_turnLampSerialReq = 0;

        [Description("logoDynamic开")]
        public void LogoDynamicOn() => BCM_RearLogoReq = 2;

        [Description("logoStatic开")]
        public void LogoStaticOn() => BCM_RearLogoReq = 3;

        [Description("logo关")]
        public void LogoOff() => BCM_RearLogoReq = 1;

        [Description("驻车动画开")]
        public void ParkingAnimationOn()
        {
            _actualUartCan1LogInParkingAnimationShow.Clear();
            _actualUartCan2LogInParkingAnimationShow.Clear();
            CheckParkingShowUartCan1DataResult = string.Empty;
            CheckParkingShowUartCan2DataResult = string.Empty;
            BCM_ParkLightReq = 2;
            _bInParkingAnimationShow = true;
        }

        [Description("驻车动画关")]
        public void ParkingAnimationOff()
        {
            BCM_ParkLightReq = 1;
            _bInParkingAnimationShow = false;

            if (_actualUartCan1LogInParkingAnimationShow.Any())
            {
                Console.WriteLine("UartCan1 log: ");
                foreach (var t in _actualUartCan1LogInParkingAnimationShow)
                {
                    for (var i = 0; i < t.Length; i = i + 2)
                        Console.Write(t.Substring(i, 2) + " ");
                    Console.WriteLine();
                }

                var findAnyHeader = false;

                foreach (var t in _expectedParkingAnimationUartCan1HeaderLog)
                {
                    if (_actualUartCan1Log.Any(a => a.Contains(t)))
                        findAnyHeader = true;

                    if (findAnyHeader)
                        break;
                }

                CheckParkingShowUartCan1DataResult = findAnyHeader ? "OK" : "NG";
            }
            else
            {
                CheckParkingShowUartCan1DataResult = "NG UartCan1没有监控到数据";
            }

            if (_actualUartCan2LogInParkingAnimationShow.Any())
            {
                Console.WriteLine("UartCan2 log: ");
                foreach (var t in _actualUartCan2LogInParkingAnimationShow)
                {
                    for (var i = 0; i < t.Length; i = i + 2)
                        Console.Write(t.Substring(i, 2) + " ");
                    Console.WriteLine();
                }

                Console.WriteLine("UartCan2 log: ");
                foreach (var t in _actualUartCan2LogInParkingAnimationShow)
                {
                    for (var i = 0; i < t.Length; i = i + 2)
                        Console.Write(t.Substring(i, 2) + " ");
                    Console.WriteLine();
                }

                var findAnyHeader = false;

                foreach (var t in _expectedParkingAnimationUartCan2HeaderLog)
                {
                    if (_actualUartCan2Log.Any(a => a.Contains(t)))
                        findAnyHeader = true;

                    if (findAnyHeader)
                        break;
                }

                CheckParkingShowUartCan2DataResult = findAnyHeader ? "OK" : "NG";
            }
            else
            {
                CheckParkingShowUartCan2DataResult = "NG UartCan2没有监控到数据";
            }
        }

        [Description("小蓝灯开")]
        public void BlueLampOn() => BCM_AdasBlueLampReq = 2;

        [Description("小蓝灯关")]
        public void BlueLampOff() => BCM_AdasBlueLampReq = 1;

        [Description("welcome打开")]
        public void WelcomeOn(string mode)
        {
            _actualUartCan1Log.Clear();
            _actualUartCan2Log.Clear();
            CheckWelcomeShowUartCan1DataResult = string.Empty;
            CheckWelcomeShowUartCan2DataResult = string.Empty;

            byte value;
            if (byte.TryParse(mode, out value))
            {
                if (value >= 2 && value <= 0x0B)
                {
                    BCM_WelcomeLightReq = value;
                    _bInWelComeShow = true;
                }
            }
        }

        //[Description("welcome打开")]
        //public void WelcomeOn(string mode)
        //{
        //    byte value;
        //    if (byte.TryParse(mode, out value))
        //    {
        //        if (value >= 2 && value <= 0x0B)
        //        {
        //            BCM_WelcomeLightReq = value;
        //        }
        //    }
        //}

        [Description("welcome关闭")]
        public void WelcomeOff()
        {
            var tp = BCM_WelcomeLightReq;
            BCM_WelcomeLightReq = 1;
            _bInWelComeShow = false;

            if (tp >= 2 && tp <= 0x0B)
            {
                if (_actualUartCan1Log.Any())
                {
                    Console.WriteLine("UartCan1 log: ");
                    foreach (var t in _actualUartCan1Log)
                    {
                        for (var i = 0; i < t.Length; i = i + 2)
                            Console.Write(t.Substring(i, 2) + " ");
                        Console.WriteLine();
                    }

                    var expectedHeaderLog = _expectedUartCan1HeaderLog[tp];
                    var expectedTailLog = _expectedUartCan1TailLog[tp];

                    var findAnyHeader = false;
                    var findAnyTail = false;

                    foreach (var t in expectedHeaderLog)
                    {
                        if (_actualUartCan1Log.Any(a => a.Contains(t)))
                            findAnyHeader = true;

                        if (findAnyHeader)
                            break;
                    }

                    foreach (var t in expectedTailLog)
                    {
                        if (_actualUartCan1Log.Any(a => a.Contains(t)))
                            findAnyTail = true;

                        if (findAnyTail)
                            break;
                    }

                    CheckWelcomeShowUartCan1DataResult = findAnyTail && findAnyHeader ? "OK" : "NG";
                }
                else
                {
                    CheckWelcomeShowUartCan1DataResult = "NG UartCan1没有监控到数据";
                }

                if (_actualUartCan2Log.Any())
                {
                    Console.WriteLine("UartCan2 log: ");
                    foreach (var t in _actualUartCan2Log)
                    {
                        for (var i = 0; i < t.Length; i = i + 2)
                            Console.Write(t.Substring(i, 2) + " ");
                        Console.WriteLine();
                    }

                    var expectedHeaderLog = _expectedUartCan2HeaderLog[tp];
                    var expectedTailLog = _expectedUartCan2TailLog[tp];

                    var findAnyHeader = false;
                    var findAnyTail = false;

                    foreach (var t in expectedHeaderLog)
                    {
                        if (_actualUartCan2Log.Any(a => a.Contains(t)))
                            findAnyHeader = true;

                        if (findAnyHeader)
                            break;
                    }

                    foreach (var t in expectedTailLog)
                    {
                        if (_actualUartCan2Log.Any(a => a.Contains(t)))
                            findAnyTail = true;

                        if (findAnyTail)
                            break;
                    }

                    CheckWelcomeShowUartCan2DataResult = findAnyTail && findAnyHeader ? "OK" : "NG";
                }
                else
                {
                    CheckWelcomeShowUartCan2DataResult = "NG UartCan2没有监控到数据";
                }
            }
            else
            {
                CheckWelcomeShowUartCan1DataResult = string.Empty;
                CheckWelcomeShowUartCan2DataResult = string.Empty;
            }
        }

        #endregion

        #region turn 400ms 使能

        private int _turn400msSignal;

        [Description("转向400毫秒使能")]
        public void Turn400MsOn() => _turn400msSignal = 1;

        [Description("转向400毫秒失能")]
        public void Turn400MsOff() => _turn400msSignal = 0;

        #endregion

        #endregion

        #region version

        private uint _reqCanId = 0X72F;
        private uint _recvCanId = 0x7AF;

        [Description("R,当前诊断灯具")] public string CurrentNode = "A灯左-LeftRCLA";

        [Description("切换成A左-默认")]
        public void SetDiagnosticLeftRCLA()
        {
            _reqCanId = 0X72F;
            _recvCanId = 0x7AF;
            CurrentNode = "A灯左-LeftRCLA";
        }

        [Description("切换成A右")]
        public void SetDiagnosticRightRCLA()
        {
            _reqCanId = 0X71F;
            _recvCanId = 0x79F;
            CurrentNode = "A灯右-RightRCLA";
        }

        [Description("切换成B")]
        public void SetDiagnosticRCLB()
        {
            _reqCanId = 0X71D;
            _recvCanId = 0x79D;
            CurrentNode = "B灯-RCLB";
        }

        [Description("R,ECU名称[F197]")]
        public string EcuName = string.Empty;
        [Description("R,SeeYao内部版本号[F195]")]
        public string InternalAppVersion = string.Empty;
        [Description("R,SeeYao内部Boot版本号[F180]")]
        public string InternalBootVersion = string.Empty;
        [Description("R,东风定义APP版本号[F189]")]
        public string DongFengAppVersion = string.Empty;
        [Description("R,东风定义HW版本号[F089]")]
        public string DongFengHwVersion = string.Empty;
        [Description("R,东风定义零件号[F187]")]
        public string DongFengPartNo = string.Empty;

        [Description("R,ECU流水编号[F18C])")]
        public string EcuSerialNo = string.Empty;
        [Description("R,供应商HW版本号[F193]")]
        public string CustomHwVersion = string.Empty;
        [Description("R,供应商名称[F18A]")]
        public string CustomName = string.Empty;

        [Description("ReadECU名称[F197]")]
        public void ReadEcuName() => EcuName = Encoding.ASCII.GetString(ReadDid(0xF1, 0x97));
        [Description("ReadSeeYao内部版本号[F195]")]
        public void ReadInternalAppVersion() => InternalAppVersion = Encoding.ASCII.GetString(ReadDid(0xF1, 0x95));
        [Description("ReadSeeYao内部Boot版本号[F180]")]
        public void ReadInternalBootVersion() => InternalBootVersion = Encoding.ASCII.GetString(ReadDid(0xF1, 0x80));
        [Description("Read东风定义APP版本号[F189]")]
        public void ReadDongFengAppVersion() => DongFengAppVersion = Encoding.ASCII.GetString(ReadDid(0xF1, 0x89));
        [Description("Read东风定义HW版本号[F089]")]
        public void ReadDongFengHwVersion() => DongFengHwVersion = Encoding.ASCII.GetString(ReadDid(0xF0, 0x89));
        [Description("Read东风定义零件号[F187]")]
        public void ReadDongFengPartNo() => DongFengPartNo = Encoding.ASCII.GetString(ReadDid(0xF1, 0x87));
        [Description("Read供应商HW版本号[F193]")]
        public void ReadCustomHwVersion() => CustomHwVersion = Encoding.ASCII.GetString(ReadDid(0xF1, 0x93));
        [Description("Read供应商名称[F18A]")]
        public void ReadCustomName() => CustomName = Encoding.ASCII.GetString(ReadDid(0xF1, 0x8A));

        #region ECE海外版增加读1500

        [Description("R,SeeYao内部CAL版本号[1500]")]
        public string InternalCalVersion = string.Empty;
        [Description("ReadSeeYao内部CAL版本号[1500]")]
        public void ReadInternalCalVersion() => InternalCalVersion = Encoding.ASCII.GetString(ReadDid(0x15, 0x00));

        #endregion

        [Description("R/W,是否检查本地历史[F18C]")]
        public bool IsCheckLocalHistory;

        [Description("R,绑定二维码")]
        public string BindingBarcode = string.Empty;

        [Description("写供应商代码")]
        public void WriteCustomSerialNo(string prNo, string hwNo)
        {
            EcuSerialNo = string.Empty;

            if (string.IsNullOrEmpty(prNo))
            {
                EcuSerialNo = "NG 输入数据错误，请填写正确的PR号";
                return;
            }

            if (!SyProductionSaveCheckData.TryGetSySequenceDailyData(prNo, out string outDate, out int outSerialNo))
            {
                EcuSerialNo = "NG 获取数据失败，请检查网络连接";
                return;
            }

            var hwNoBytes = new byte[8];
            for (var i = 0; i < hwNoBytes.Length; i++)
            {
                hwNoBytes[i] = 0x20;
            }

            var toWriteHw = Encoding.ASCII.GetBytes(hwNo);
            if (toWriteHw.Length > 8)
            {
                EcuSerialNo = "NG 输入硬件版本号有误";
                return;
            }
            Array.Copy(toWriteHw, 0, hwNoBytes, 0, toWriteHw.Length);

            lock (_lockCanSend)
            {
                if (!Can.CanBusWithUds.TryEnterExtendedSession(
                    _reqCanId, _recvCanId, CanBus.CanType.Standard))
                {
                    EcuSerialNo = "NG 进入拓展模式1003失败";
                    return;
                }

                byte[] seedBytes;
                if (!Can.CanBusWithUds.TryRequestSeed(
                    _reqCanId, _recvCanId, CanBus.CanType.Standard, 0x01, out seedBytes))
                {
                    EcuSerialNo = "NG 请求seed2701失败";
                    return;
                }

                var Xor = new byte[4];

                if (_reqCanId == 0X72F) // A L
                    Xor = new byte[] { 0xCE, 0x54, 0xB9, 0xD6 };
                else if (_reqCanId == 0X71F) // A R
                    Xor = new byte[] { 0x16, 0x22, 0x14, 0xD2 };
                else if (_reqCanId == 0X71D) // B
                    Xor = new byte[] { 0xE8, 0x5E, 0xDD, 0xC3 };

                //var Xor = new byte[] { 0xCE, 0x54, 0xB9, 0xD6 };
                byte[] Cal = new byte[4];
                var keyBytes = new byte[4];
                for (int i = 0; i < 4; i++)
                    Cal[i] = (byte)(seedBytes[i] ^ Xor[i]);
                keyBytes[0] = (byte)(((Cal[0] & 0x0f) << 4) | ((Cal[1] & 0xf0)));
                keyBytes[1] = (byte)(((Cal[1] & 0x0f) << 4) | ((Cal[2] & 0xf0) >> 4));
                keyBytes[2] = (byte)(((Cal[2] & 0xf0)) | ((Cal[3] & 0xf0) >> 4));
                keyBytes[3] = (byte)(((Cal[3] & 0x0f) << 4) | ((Cal[0] & 0x0f)));

                //var keyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00 };
                if (!Can.CanBusWithUds.TrySendKey(
                    _reqCanId,
                    _recvCanId,
                    CanBus.CanType.Standard, 0x02, keyBytes))
                {
                    EcuSerialNo = "NG 写入key2702失败";
                    return;
                }

                var matrixBytes = new List<byte>();
                var date = outDate.Substring(2, 6);
                var serialNo = outSerialNo.ToString().PadLeft(6, '0');
                matrixBytes.AddRange(Encoding.ASCII.GetBytes(date));
                matrixBytes.AddRange(Encoding.ASCII.GetBytes(serialNo));

                var tpWrite = date + serialNo;
                Can.CanBusWithUds.TryWriteData(_reqCanId, _recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x8C, matrixBytes.ToArray());
                Thread.Sleep(100);
                Can.CanBusWithUds.TryWriteData(_reqCanId, _recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x93, hwNoBytes);

                Thread.Sleep(400);

                var tpRead = Encoding.ASCII.GetString(ReadDid(0xF1, 0x8C));
                if (tpRead == tpWrite)
                {
                    if (!string.IsNullOrEmpty(BindingBarcode))
                        SaveLocalBindingHistory(BindingBarcode, tpRead);
                    EcuSerialNo = "OK " + tpRead;
                }
                else
                {
                    if (IsCheckLocalHistory)
                    {
                        if (!string.IsNullOrEmpty(BindingBarcode))
                        {
                            var isExit = CheckLocalBindingHistroy(BindingBarcode, tpRead);
                            if (isExit)
                            {
                                EcuSerialNo = "OK " + tpRead;
                            }
                            else
                            {
                                EcuSerialNo = "NG " + string.Format("需写入{0};实际读取{1}", tpWrite, tpRead);
                            }
                        }
                        else
                        {
                            EcuSerialNo = "NG " + string.Format("需写入{0};实际读取{1}", tpWrite, tpRead);
                        }
                    }
                    else
                    {
                        EcuSerialNo = "NG " + string.Format("需写入{0};实际读取{1}", tpWrite, tpRead);
                    }
                }
            }
        }

        private static string _loaclDbFilePath;
        private static readonly object _dbLocker = new object();
        private static string _dbName = "H77_Binding_History.db";
        private static SqlSugarClient db = null;

        private void CreateLocalBindingDb()
        {
            lock (_dbLocker)
            {
                if (db is null)
                {
                    try
                    {
                        _loaclDbFilePath = string.Format(@"{0}/LocalDB/{1}", Path.GetPathRoot(Directory.GetCurrentDirectory()), _dbName);
                        if (!string.IsNullOrEmpty(_loaclDbFilePath))
                        {
                            db = new SqlSugarClient(new ConnectionConfig()
                            {
                                ConnectionString = $"Data Source={_loaclDbFilePath}", // SQLite 连接字符串
                                DbType = DbType.Sqlite, // 指定数据库类型为 SQLite
                                IsAutoCloseConnection = true, // 自动关闭连接
                                InitKeyType = InitKeyType.Attribute // 从实体特性中读取主键自增列信息
                            });

                            if (CreateBase())
                            {
                                db.CodeFirst.InitTables<BindingHistory>();
                                db.Open();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        db = null;
                    }
                }
            }
        }

        private static bool CreateBase()
        {
            try
            {
                return db.DbMaintenance.CreateDatabase();
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal class BindingHistory
        {
            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int Id { get; set; }
            public string Barcode { get; set; }

            public string EcuSerialNo { get; set; }
        }

        private void SaveLocalBindingHistory(string barcode, string ecuSerialNo)
        {
            try
            {
                var insertData = new BindingHistory { Barcode = barcode, EcuSerialNo = ecuSerialNo };
                db.Insertable(insertData).ExecuteCommand();
            }
            catch (Exception)
            {
            }
        }

        private bool CheckLocalBindingHistroy(string barcode, string ecuSerialNo)
        {
            try
            {
                return !(db is null) && db.Queryable<BindingHistory>().Any(f => f.Barcode == barcode && f.EcuSerialNo == ecuSerialNo);
            }
            catch (Exception)
            {
                return false;
            }
        }

        //[Description("读供应商代码Test")]
        //public void TestCustomSerialNoTest()
        //{
        //    StartCanMsg();
        //    Thread.Sleep(2000);
        //    StopCanMsg();
        //    Thread.Sleep(100);

        //    EcuSerialNo = string.Empty;
        //    EcuSerialNo = Encoding.ASCII.GetString(ReadDid(0xF1, 0x8C));
        //    ReadCustomHwVersion();
        //}

        //public string ToWriteBarcode = string.Empty;

        //public void WriteCustomSerialNoByBarcode(
        //    string partNo, int barcodeLen, int hwIndex, int dateIndex, int serialNoIndex)
        //{
        //    // P00389063H0031S0045XH620234BA332001BAB400
        //    // 需要把生产日期：5XH解析成实际年月日
        //    return;

        //    // 251022000171
        //    EcuSerialNo = string.Empty;

        //    var tp = ToWriteBarcode;
        //    ToWriteBarcode = string.Empty;

        //    if (string.IsNullOrEmpty(partNo))
        //    {
        //        EcuSerialNo = "NG 输入数据错误，请填写正确的PR号";
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(tp))
        //    {
        //        EcuSerialNo = "NG 请输入二维码错误";
        //        return;
        //    }

        //    if (tp.Length != barcodeLen)
        //    {
        //        EcuSerialNo = "NG 输入二维码长度错误，请填写正确的二维码";
        //        return;
        //    }

        //    if (!tp.ToLower().StartsWith(partNo.ToLower()))
        //    {
        //        EcuSerialNo = "NG 输入二维码格式，请填写正确的二维码";
        //        return;
        //    }

        //    if (!SyProductionSaveCheckData.TryGetSySequenceDailyData(partNo, out string outDate, out int outSerialNo))
        //    {
        //        EcuSerialNo = "NG 获取数据失败，请检查网络连接";
        //        return;
        //    }

        //    var hwNo = string.Empty;
        //    var serialNoInCode = int.MinValue;

        //    try
        //    {
        //        hwNo = tp.Substring(hwIndex, 4);
        //    }
        //    catch (Exception)
        //    {
        //        hwNo = string.Empty;
        //    }

        //    try
        //    {
        //        serialNoInCode = int.Parse(tp.Substring(serialNoIndex, 6));
        //    }
        //    catch (Exception)
        //    {
        //        serialNoInCode = int.MinValue;
        //    }

        //    if (string.IsNullOrEmpty(hwNo))
        //    {
        //        EcuSerialNo = "NG 硬件版本号解析失败，请填写正确的二维码";
        //        return;
        //    }

        //    if (serialNoInCode is int.MinValue)
        //    {
        //        EcuSerialNo = "NG 生产序列号解析失败，请填写正确的二维码";
        //        return;
        //    }

        //    var hwNoBytes = new byte[8];
        //    for (var i = 0; i < hwNoBytes.Length; i++)
        //        hwNoBytes[i] = 0x20;

        //    var toWriteHw = Encoding.ASCII.GetBytes(hwNo);
        //    if (toWriteHw.Length > 8)
        //    {
        //        EcuSerialNo = "NG 输入硬件版本号有误";
        //        return;
        //    }
        //    Array.Copy(toWriteHw, 0, hwNoBytes, 0, toWriteHw.Length);

        //    lock (_lockCanSend)
        //    {
        //        if (!Can.CanBusWithUds.TryEnterExtendedSession(
        //            _reqCanId, _recvCanId, CanBus.CanType.Standard))
        //        {
        //            EcuSerialNo = "NG 进入拓展模式1003失败";
        //            return;
        //        }

        //        byte[] seedBytes;
        //        if (!Can.CanBusWithUds.TryRequestSeed(
        //            _reqCanId, _recvCanId, CanBus.CanType.Standard, 0x01, out seedBytes))
        //        {
        //            EcuSerialNo = "NG 请求seed2701失败";
        //            return;
        //        }

        //        var Xor = new byte[4];

        //        if (_reqCanId == 0X72F) // A L
        //            Xor = new byte[] { 0xCE, 0x54, 0xB9, 0xD6 };
        //        else if (_reqCanId == 0X71F) // A R
        //            Xor = new byte[] { 0x16, 0x22, 0x14, 0xD2 };
        //        else if (_reqCanId == 0X71D) // B
        //            Xor = new byte[] { 0xE8, 0x5E, 0xDD, 0xC3 };

        //        //var Xor = new byte[] { 0xCE, 0x54, 0xB9, 0xD6 };
        //        byte[] Cal = new byte[4];
        //        var keyBytes = new byte[4];
        //        for (int i = 0; i < 4; i++)
        //            Cal[i] = (byte)(seedBytes[i] ^ Xor[i]);
        //        keyBytes[0] = (byte)(((Cal[0] & 0x0f) << 4) | ((Cal[1] & 0xf0)));
        //        keyBytes[1] = (byte)(((Cal[1] & 0x0f) << 4) | ((Cal[2] & 0xf0) >> 4));
        //        keyBytes[2] = (byte)(((Cal[2] & 0xf0)) | ((Cal[3] & 0xf0) >> 4));
        //        keyBytes[3] = (byte)(((Cal[3] & 0x0f) << 4) | ((Cal[0] & 0x0f)));

        //        //var keyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00 };
        //        if (!Can.CanBusWithUds.TrySendKey(
        //            _reqCanId,
        //            _recvCanId,
        //            CanBus.CanType.Standard, 0x02, keyBytes))
        //        {
        //            EcuSerialNo = "NG 写入key2702失败";
        //            return;
        //        }

        //        var matrixBytes = new List<byte>();
        //        var date = outDate.Substring(2, 6);
        //        var serialNo = serialNoInCode.ToString().PadLeft(6, '0'); /*outSerialNo.ToString().PadLeft(6, '0');*/
        //        matrixBytes.AddRange(Encoding.ASCII.GetBytes(date));
        //        matrixBytes.AddRange(Encoding.ASCII.GetBytes(serialNo));

        //        var tpWrite = date + serialNo;
        //        Can.CanBusWithUds.TryWriteData(_reqCanId, _recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x8C, matrixBytes.ToArray());
        //        Thread.Sleep(100);
        //        Can.CanBusWithUds.TryWriteData(_reqCanId, _recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x93, hwNoBytes);
        //        Thread.Sleep(400);

        //        var tpRead = Encoding.ASCII.GetString(ReadDid(0xF1, 0x8C));
        //        if (tpRead == tpWrite)
        //            EcuSerialNo = "OK " + tpRead;
        //        else
        //            EcuSerialNo = "NG " + string.Format("需写入{0};实际读取{1}", tpWrite, tpRead);
        //    }
        //}

        private byte[] ReadDid(byte didHi, byte didLo)
        {
            if (Can is null)
                return new byte[0];

            lock (_lockCanSend)
            {
                byte[] echo;
                if (Can.CanBusWithUds.TryReadData(_reqCanId, _recvCanId, CanBus.CanType.Standard,
                    CanBus.CanProtocol.SpeedCanFd, didHi, didLo, out echo))
                    return echo;
            }
            Thread.Sleep(100);
            lock (_lockCanSend)
            {
                byte[] echo;
                if (Can.CanBusWithUds.TryReadData(_reqCanId, _recvCanId, CanBus.CanType.Standard,
                    CanBus.CanProtocol.SpeedCanFd, didHi, didLo, out echo))
                    return echo;
            }
            Thread.Sleep(100);
            lock (_lockCanSend)
            {
                byte[] echo;
                return Can.CanBusWithUds.TryReadData(_reqCanId, _recvCanId, CanBus.CanType.Standard,
                CanBus.CanProtocol.SpeedCanFd, didHi, didLo, out echo) ? echo : new byte[0];
            }
        }

        #endregion

        #region 清除错误

        [Description("R,诊断-清除DTC结果")]
        public string ClearDtcResult = string.Empty;

        public void ClearDtc()
        {
            ClearDtcResult = string.Empty;
            if (Can is null)
                return;

            lock (_lockCanSend)
            {
                var isOk = Can.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(_reqCanId, _recvCanId, CanBus.CanType.Standard);
                ClearDtcResult = isOk ? "OK" : "NG";
            }
        }

        [Description("R,诊断-读取DTC结果")]
        public string ReadDtcResult;

        public void ReadDtc()
        {
            ReadDtcResult = string.Empty;

            if (Can == null)
                return;

            lock (_lockCanSend)
            {
                byte[] echo;
                if (Can.CanBusWithUds.TryReadDtcInfomation(
                        _reqCanId, _recvCanId, CanBus.CanType.Standard, 0x02, 0x09,
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

                            var codeNotInWhiteList = FilterCodeNotInWhiteList(readCodes);

                            if (codeNotInWhiteList.Any())
                            {
                                foreach (var t in codeNotInWhiteList)
                                    ReadDtcResult += string.Format("[{0}];", t.Remark);
                            }
                            else
                            {
                                ReadDtcResult = "NoError";
                            }
                        }
                        else
                        {
                            ReadDtcResult = "ReadDtcResLenError";
                        }
                    }
                }
                else
                {
                    ReadDtcResult = "NoRead";
                }
            }
        }

        private readonly List<string> _whiteList = new List<string>();

        [Description("将DTC添加进白名单")]
        public void AddDtcCodeIntoWhiteList(string code)
        {
            var index = _whiteList.FindIndex(f => string.Equals(f, code, StringComparison.CurrentCultureIgnoreCase));
            if (index == -1)
                _whiteList.Add(code.Trim().ToUpper());
        }

        private List<Uds14229Helper.DtcData> FilterCodeNotInWhiteList(
            IEnumerable<Uds14229Helper.DtcData> toFilterDtcCodes)
        {
            return (from t in toFilterDtcCodes
                    let findIndex =
                        _whiteList.FindIndex(f => string.Equals(f, t.Code, StringComparison.CurrentCultureIgnoreCase))
                    where findIndex == -1
                    select t).ToList();
        }

        #endregion

        #region 读取故障信息

        public string RLM_LeftPositionLampFault = string.Empty;
        public string RLM_LeftTurnLampFault = string.Empty;
        public string RLM_LeftStopLampFault = string.Empty;
        public string RLM_LeftRearfogLampFault = string.Empty;

        public string RLM_RightPositionLampFault = string.Empty;
        public string RLM_RightTurnLampFault = string.Empty;
        public string RLM_RightStopLampFault = string.Empty;
        public string RLM_RightRearfogLampFault = string.Empty;

        public string RLM_ReversingLampFault = string.Empty;
        public string RLM_MidApositionLampFault = string.Empty;
        public string RLM_MidLeftTurnLampFault = string.Empty;
        public string RLM_MidLeftStopLampFault = string.Empty;
        public string RLM_LicenseLampFault = string.Empty;
        public string RLM_HighStopLampFault = string.Empty;
        public string RLM_MidRightTurnLampFault = string.Empty;
        public string RLM_AdasBlueLampFault = string.Empty;

        private void CanBus_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (Can != null && Can.Name == name && (onPushCanDataType == CanBus.OnPushCanDataType.FilterRx || onPushCanDataType == CanBus.OnPushCanDataType.NotFilterRx))
            {
                if (data.CanData.Length == 8)
                {
                    var bits = ValueHelper.GetBits(data.CanData);
                    var nowTs = HighPrecisionTimer.GetTimestamp();

                    switch (data.CanId)
                    {
                        case 0x107:
                            //public string RLM_LeftPositionLampFault = string.Empty;
                            //public string RLM_LeftTurnLampFault = string.Empty;
                            //public string RLM_LeftStopLampFault = string.Empty;
                            //public string RLM_LeftRearfogLampFault = string.Empty;

                            Console.WriteLine("recv msg 0x107:" + ValueHelper.GetHextStrWithOx(data.CanData));

                            // l turn
                            if (_dicErrorInvoke["RLM_LeftTurnLampFault"] > 0)
                            {
                                RLM_LeftTurnLampFault = Convert.ToByte(bits[9] + bits[8], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_LeftTurnLampFault"];
                                var startTs = _dicErrorStartTs["RLM_LeftTurnLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_LeftTurnLampFault"] = 0;
                                    _dicErrorStartTs["RLM_LeftTurnLampFault"] = 0;
                                }
                            }

                            // l pos
                            if (_dicErrorInvoke["RLM_LeftPositionLampFault"] > 0)
                            {
                                RLM_LeftPositionLampFault = Convert.ToByte(bits[1], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_LeftPositionLampFault"];
                                var startTs = _dicErrorStartTs["RLM_LeftPositionLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_LeftPositionLampFault"] = 0;
                                    _dicErrorStartTs["RLM_LeftPositionLampFault"] = 0;
                                }
                            }

                            // l stop
                            if (_dicErrorInvoke["RLM_LeftStopLampFault"] > 0)
                            {
                                RLM_LeftStopLampFault = Convert.ToByte(bits[10], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_LeftStopLampFault"];
                                var startTs = _dicErrorStartTs["RLM_LeftStopLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_LeftStopLampFault"] = 0;
                                    _dicErrorStartTs["RLM_LeftStopLampFault"] = 0;
                                }
                            }

                            // l fog
                            if (_dicErrorInvoke["RLM_LeftRearfogLampFault"] > 0)
                            {
                                RLM_LeftRearfogLampFault = Convert.ToByte(bits[3], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_LeftRearfogLampFault"];
                                var startTs = _dicErrorStartTs["RLM_LeftRearfogLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_LeftRearfogLampFault"] = 0;
                                    _dicErrorStartTs["RLM_LeftRearfogLampFault"] = 0;
                                }
                            }
                            break;

                        case 0x117:
                            //public string RLM_RightPositionLampFault = string.Empty;
                            //public string RLM_RightTurnLampFault = string.Empty;
                            //public string RLM_RightStopLampFault = string.Empty;
                            //public string RLM_RightRearfogLampFault = string.Empty;

                            Console.WriteLine("recv msg 0x117:" + ValueHelper.GetHextStrWithOx(data.CanData));

                            // r turn
                            if (_dicErrorInvoke["RLM_RightTurnLampFault"] > 0)
                            {
                                RLM_RightTurnLampFault = Convert.ToByte(bits[9] + bits[8], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_RightTurnLampFault"];
                                var startTs = _dicErrorStartTs["RLM_RightTurnLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_RightTurnLampFault"] = 0;
                                    _dicErrorStartTs["RLM_RightTurnLampFault"] = 0;
                                }
                            }

                            // r pos
                            if (_dicErrorInvoke["RLM_RightPositionLampFault"] > 0)
                            {
                                RLM_RightPositionLampFault = Convert.ToByte(bits[1], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_RightPositionLampFault"];
                                var startTs = _dicErrorStartTs["RLM_RightPositionLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_RightPositionLampFault"] = 0;
                                    _dicErrorStartTs["RLM_RightPositionLampFault"] = 0;
                                }
                            }

                            // r stop
                            if (_dicErrorInvoke["RLM_RightStopLampFault"] > 0)
                            {
                                RLM_RightStopLampFault = Convert.ToByte(bits[10], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_RightStopLampFault"];
                                var startTs = _dicErrorStartTs["RLM_RightStopLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_RightStopLampFault"] = 0;
                                    _dicErrorStartTs["RLM_RightStopLampFault"] = 0;
                                }
                            }

                            // r fog
                            if (_dicErrorInvoke["RLM_RightRearfogLampFault"] > 0)
                            {
                                RLM_RightRearfogLampFault = Convert.ToByte(bits[3], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_RightRearfogLampFault"];
                                var startTs = _dicErrorStartTs["RLM_RightRearfogLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_RightRearfogLampFault"] = 0;
                                    _dicErrorStartTs["RLM_RightRearfogLampFault"] = 0;
                                }
                            }
                            break;

                        case 0x108:
                            //public string RLM_ReversingLampFault = string.Empty;
                            //public string RLM_MidApositionLampFault = string.Empty;
                            //public string RLM_MidLeftTurnLampFault = string.Empty;
                            //public string RLM_MidLeftStopLampFault = string.Empty;
                            //public string RLM_LicenseLampFault = string.Empty;
                            //public string RLM_HighStopLampFault = string.Empty;
                            //public string RLM_MidRightTurnLampFault = string.Empty;

                            if (_dicErrorInvoke["RLM_ReversingLampFault"] > 0)
                            {
                                RLM_ReversingLampFault = Convert.ToByte(bits[10], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_ReversingLampFault"];
                                var startTs = _dicErrorStartTs["RLM_ReversingLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_ReversingLampFault"] = 0;
                                    _dicErrorStartTs["RLM_ReversingLampFault"] = 0;
                                }
                            }
                            if (_dicErrorInvoke["RLM_MidApositionLampFault"] > 0)
                            {
                                RLM_MidApositionLampFault = Convert.ToByte(bits[1], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_MidApositionLampFault"];
                                var startTs = _dicErrorStartTs["RLM_MidApositionLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_MidApositionLampFault"] = 0;
                                    _dicErrorStartTs["RLM_MidApositionLampFault"] = 0;
                                }
                            }
                            if (_dicErrorInvoke["RLM_MidLeftTurnLampFault"] > 0)
                            {
                                RLM_MidLeftTurnLampFault = Convert.ToByte(bits[5] + bits[4], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_MidLeftTurnLampFault"];
                                var startTs = _dicErrorStartTs["RLM_MidLeftTurnLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_MidLeftTurnLampFault"] = 0;
                                    _dicErrorStartTs["RLM_MidLeftTurnLampFault"] = 0;
                                }
                            }
                            if (_dicErrorInvoke["RLM_MidLeftStopLampFault"] > 0)
                            {
                                RLM_MidLeftStopLampFault = Convert.ToByte(bits[6], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_MidLeftStopLampFault"];
                                var startTs = _dicErrorStartTs["RLM_MidLeftStopLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_MidLeftStopLampFault"] = 0;
                                    _dicErrorStartTs["RLM_MidLeftStopLampFault"] = 0;
                                }
                            }
                            if (_dicErrorInvoke["RLM_LicenseLampFault"] > 0)
                            {
                                RLM_LicenseLampFault = Convert.ToByte(bits[8], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_LicenseLampFault"];
                                var startTs = _dicErrorStartTs["RLM_LicenseLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_LicenseLampFault"] = 0;
                                    _dicErrorStartTs["RLM_LicenseLampFault"] = 0;
                                }
                            }
                            if (_dicErrorInvoke["RLM_HighStopLampFault"] > 0)
                            {
                                Console.WriteLine("mgs 108: " + ValueHelper.GetHextStrWithOx(data.CanData));
                                RLM_HighStopLampFault = Convert.ToByte(bits[12], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_HighStopLampFault"];
                                var startTs = _dicErrorStartTs["RLM_HighStopLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_HighStopLampFault"] = 0;
                                    _dicErrorStartTs["RLM_HighStopLampFault"] = 0;
                                }
                            }
                            if (_dicErrorInvoke["RLM_MidRightTurnLampFault"] > 0)
                            {
                                RLM_MidRightTurnLampFault = Convert.ToByte(bits[15] + bits[14], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_MidRightTurnLampFault"];
                                var startTs = _dicErrorStartTs["RLM_MidRightTurnLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_MidRightTurnLampFault"] = 0;
                                    _dicErrorStartTs["RLM_MidRightTurnLampFault"] = 0;
                                }
                            }
                            // RLM_AdasBlueLampFault
                            if (_dicErrorInvoke["RLM_AdasBlueLampFault"] > 0)
                            {
                                RLM_AdasBlueLampFault = Convert.ToByte(bits[16], 2).ToString();
                                var keepMs = _dicErrorInvoke["RLM_AdasBlueLampFault"];
                                var startTs = _dicErrorStartTs["RLM_AdasBlueLampFault"];
                                var timeSpan = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                                if (timeSpan >= keepMs)
                                {
                                    _dicErrorInvoke["RLM_AdasBlueLampFault"] = 0;
                                    _dicErrorStartTs["RLM_AdasBlueLampFault"] = 0;
                                }
                            }
                            break;
                    }
                }
            }
        }

        private Dictionary<string, long> _dicErrorStartTs = new Dictionary<string, long>();
        private Dictionary<string, long> _dicErrorInvoke = new Dictionary<string, long>();

        public void ReadLeftTurnLampFault(int ms)
        {
            RLM_LeftTurnLampFault = string.Empty;
            _dicErrorInvoke["RLM_LeftTurnLampFault"] = ms;
            _dicErrorStartTs["RLM_LeftTurnLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadLeftPositionLampFault(int ms)
        {
            RLM_LeftPositionLampFault = string.Empty;
            _dicErrorInvoke["RLM_LeftPositionLampFault"] = ms;
            _dicErrorStartTs["RLM_LeftPositionLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadLeftStopLampFault(int ms)
        {
            RLM_LeftStopLampFault = string.Empty;
            _dicErrorInvoke["RLM_LeftStopLampFault"] = ms;
            _dicErrorStartTs["RLM_LeftStopLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadLeftFogLampFault(int ms)
        {
            RLM_LeftRearfogLampFault = string.Empty;
            _dicErrorInvoke["RLM_LeftRearfogLampFault"] = ms;
            _dicErrorStartTs["RLM_LeftRearfogLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadRightTurnLampFault(int ms)
        {
            RLM_RightTurnLampFault = string.Empty;
            _dicErrorInvoke["RLM_RightTurnLampFault"] = ms;
            _dicErrorStartTs["RLM_RightTurnLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadRightPositionLampFault(int ms)
        {
            RLM_RightPositionLampFault = string.Empty;
            _dicErrorInvoke["RLM_RightPositionLampFault"] = ms;
            _dicErrorStartTs["RLM_RightPositionLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadRightStopLampFault(int ms)
        {
            RLM_RightStopLampFault = string.Empty;
            _dicErrorInvoke["RLM_RightStopLampFault"] = ms;
            _dicErrorStartTs["RLM_RightStopLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadRightFogLampFault(int ms)
        {
            RLM_RightRearfogLampFault = string.Empty;
            _dicErrorInvoke["RLM_RightRearfogLampFault"] = ms;
            _dicErrorStartTs["RLM_RightRearfogLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadFbBackUp(int ms)
        {
            RLM_ReversingLampFault = string.Empty;
            _dicErrorInvoke["RLM_ReversingLampFault"] = ms;
            _dicErrorStartTs["RLM_ReversingLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadRLM_MidApositionLampFault(int ms)
        {
            RLM_MidApositionLampFault = string.Empty;
            _dicErrorInvoke["RLM_MidApositionLampFault"] = ms;
            _dicErrorStartTs["RLM_MidApositionLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadRLM_MidLeftTurnLampFault(int ms)
        {
            RLM_MidLeftTurnLampFault = string.Empty;
            _dicErrorInvoke["RLM_MidLeftTurnLampFault"] = ms;
            _dicErrorStartTs["RLM_MidLeftTurnLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadRLM_MidLeftStopLampFault(int ms)
        {
            RLM_MidLeftStopLampFault = string.Empty;
            _dicErrorInvoke["RLM_MidLeftStopLampFault"] = ms;
            _dicErrorStartTs["RLM_MidLeftStopLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadRLM_LicenseLampFault(int ms)
        {
            RLM_LicenseLampFault = string.Empty;
            _dicErrorInvoke["RLM_LicenseLampFault"] = ms;
            _dicErrorStartTs["RLM_LicenseLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadRLM_HighStopLampFault(int ms)
        {
            RLM_HighStopLampFault = string.Empty;
            _dicErrorInvoke["RLM_HighStopLampFault"] = ms;
            _dicErrorStartTs["RLM_HighStopLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadRLM_MidRightTurnLampFault(int ms)
        {
            RLM_MidRightTurnLampFault = string.Empty;
            _dicErrorInvoke["RLM_MidRightTurnLampFault"] = ms;
            _dicErrorStartTs["RLM_MidRightTurnLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        public void ReadRLM_AdasBlueLampFault(int ms)
        {
            RLM_AdasBlueLampFault = string.Empty;
            _dicErrorInvoke["RLM_AdasBlueLampFault"] = ms;
            _dicErrorStartTs["RLM_AdasBlueLampFault"] = HighPrecisionTimer.GetTimestamp();
        }

        #endregion

        #region 动画uartcan报文监控

        public MySerialPort UartCan1BaudRate1M;
        public MySerialPort UartCan2BaudRate1M;

        private readonly Dictionary<byte, List<string>> _expectedUartCan1HeaderLog = new Dictionary<byte, List<string>>();
        private readonly Dictionary<byte, List<string>> _expectedUartCan1TailLog = new Dictionary<byte, List<string>>();
        private readonly Dictionary<byte, List<string>> _expectedUartCan2HeaderLog = new Dictionary<byte, List<string>>();
        private readonly Dictionary<byte, List<string>> _expectedUartCan2TailLog = new Dictionary<byte, List<string>>();

        private readonly List<string> _expectedParkingAnimationUartCan1HeaderLog = new List<string>();
        private readonly List<string> _expectedParkingAnimationUartCan2HeaderLog = new List<string>();

        private void InitExpectedUartCanLog()
        {
            for (var i = 2; i < 0x0B; i++)
            {
                _expectedUartCan1HeaderLog.Add((byte)i, new List<string>());
                _expectedUartCan1TailLog.Add((byte)i, new List<string>());

                _expectedUartCan2HeaderLog.Add((byte)i, new List<string>());
                _expectedUartCan2TailLog.Add((byte)i, new List<string>());
            }

            #region 动画模式3报文

            _expectedUartCan1HeaderLog[3].Add("B0200000000000000000");
            _expectedUartCan1HeaderLog[3].Add("B1200000000000000000");
            _expectedUartCan1HeaderLog[3].Add("B2200000000000000000");
            _expectedUartCan1HeaderLog[3].Add("B3200000000000000000");
            _expectedUartCan1HeaderLog[3].Add("B4200000000000000000");
            _expectedUartCan1HeaderLog[3].Add("B5200000000000000000");
            _expectedUartCan1HeaderLog[3].Add("B6200000000000000000");
            _expectedUartCan1HeaderLog[3].Add("B8200000000000000000");
            _expectedUartCan1HeaderLog[3].Add("B9200000000000000000");
            _expectedUartCan1HeaderLog[3].Add("BA200000000000000000");
            _expectedUartCan1HeaderLog[3].Add("A02800000000");
            _expectedUartCan1HeaderLog[3].Add("A12800000000");
            _expectedUartCan1HeaderLog[3].Add("A22800000000");
            _expectedUartCan1HeaderLog[3].Add("A32800000000");
            _expectedUartCan1HeaderLog[3].Add("A42800004040");
            _expectedUartCan1HeaderLog[3].Add("A52800000000");
            _expectedUartCan1HeaderLog[3].Add("A62800000000");
            _expectedUartCan1HeaderLog[3].Add("A82800000000");
            _expectedUartCan1HeaderLog[3].Add("A92800000000");
            _expectedUartCan1HeaderLog[3].Add("AA2800000000");

            _expectedUartCan1TailLog[3].Add("B0203333333340404040");
            _expectedUartCan1TailLog[3].Add("B1208080808080808080");
            //_expectedUartCan1TailLog[3].Add("B2200000000000000000");
            //_expectedUartCan1TailLog[3].Add("B3200000000000000000");
            _expectedUartCan1TailLog[3].Add("B4204040404040404040");
            //_expectedUartCan1TailLog[3].Add("B5200000000000000000");
            //_expectedUartCan1TailLog[3].Add("B6200000000000000000");
            _expectedUartCan1TailLog[3].Add("B8200000333333333333");
            _expectedUartCan1TailLog[3].Add("B9203333333333333333");
            _expectedUartCan1TailLog[3].Add("BA203333333333333333");
            _expectedUartCan1TailLog[3].Add("A02840404040");
            _expectedUartCan1TailLog[3].Add("A12880803333");
            _expectedUartCan1TailLog[3].Add("A22800000000");
            _expectedUartCan1TailLog[3].Add("A32800000000");
            _expectedUartCan1TailLog[3].Add("A4280000B3B3");
            _expectedUartCan1TailLog[3].Add("A52800000000");
            _expectedUartCan1TailLog[3].Add("A62800000000");
            _expectedUartCan1TailLog[3].Add("A82833333333");
            _expectedUartCan1TailLog[3].Add("A92880808080");
            _expectedUartCan1TailLog[3].Add("AA2880808080");

            _expectedUartCan2HeaderLog[3].Add("B0200000000000000000");
            _expectedUartCan2HeaderLog[3].Add("B1200000000000000000");
            _expectedUartCan2HeaderLog[3].Add("B2200000000000000000");
            _expectedUartCan2HeaderLog[3].Add("B3200000000000000000");
            _expectedUartCan2HeaderLog[3].Add("B4200000000000000000");
            _expectedUartCan2HeaderLog[3].Add("B5200000000000000000");
            _expectedUartCan2HeaderLog[3].Add("B6200000000000000000");
            _expectedUartCan2HeaderLog[3].Add("B7200000000000000000");
            _expectedUartCan2HeaderLog[3].Add("B8200000000000000000");
            _expectedUartCan2HeaderLog[3].Add("B9200000000000000000");
            _expectedUartCan2HeaderLog[3].Add("BA200000000000000000");
            _expectedUartCan2HeaderLog[3].Add("A02800000000");
            _expectedUartCan2HeaderLog[3].Add("A12800000000");
            _expectedUartCan2HeaderLog[3].Add("A22800000000");
            _expectedUartCan2HeaderLog[3].Add("A32800000000");
            _expectedUartCan2HeaderLog[3].Add("A42800000000");
            _expectedUartCan2HeaderLog[3].Add("A52800000000");
            _expectedUartCan2HeaderLog[3].Add("A62800000000");
            _expectedUartCan2HeaderLog[3].Add("A72800000000");
            _expectedUartCan2HeaderLog[3].Add("A82800000000");
            _expectedUartCan2HeaderLog[3].Add("A92800000000");
            _expectedUartCan2HeaderLog[3].Add("AA2800000000");

            _expectedUartCan2TailLog[3].Add("B0203333333340404040");
            _expectedUartCan2TailLog[3].Add("B1208080808080808080");
            //_expectedUartCan2TailLog[3].Add("B220000000000000000");
            //_expectedUartCan2TailLog[3].Add("B320000000000000000");
            _expectedUartCan2TailLog[3].Add("B4204040404040404040");
            //_expectedUartCan2TailLog[3].Add("B520000000000000000");
            //_expectedUartCan2TailLog[3].Add("B620000000000000000");
            _expectedUartCan2TailLog[3].Add("B720B3B3B3B3B3B3000");
            _expectedUartCan2TailLog[3].Add("B8200000333333333333");
            _expectedUartCan2TailLog[3].Add("B9203333333333333333");
            _expectedUartCan2TailLog[3].Add("BA203333333333333333");
            _expectedUartCan2TailLog[3].Add("A02840404040");
            _expectedUartCan2TailLog[3].Add("A12880803333");
            //_expectedUartCan2TailLog[3].Add("A22800000000");
            //_expectedUartCan2TailLog[3].Add("A32800000000");
            _expectedUartCan2TailLog[3].Add("A428B3B30000");
            _expectedUartCan2TailLog[3].Add("A52800000000");
            //_expectedUartCan2TailLog[3].Add("A62800000000");
            //_expectedUartCan2TailLog[3].Add("A72800000000");
            _expectedUartCan2TailLog[3].Add("A82833333333");
            _expectedUartCan2TailLog[3].Add("A92880808080");
            _expectedUartCan2TailLog[3].Add("AA2880808080");

            #endregion

            #region 驻车动画报文

            _expectedParkingAnimationUartCan1HeaderLog.Add("B020333333330D0D0D0D");
            _expectedParkingAnimationUartCan1HeaderLog.Add("B1200D0D0D0D0D0D0D0D");
            //_expectedParkingAnimationUartCan1HeaderLog.Add("B2200000000000000000");
            //_expectedParkingAnimationUartCan1HeaderLog.Add("B3200000000000000000");
            _expectedParkingAnimationUartCan1HeaderLog.Add("B4200D0D0D0D0D0D0D0D");
            //_expectedParkingAnimationUartCan1HeaderLog.Add("B5200000000000000000");
            //_expectedParkingAnimationUartCan1HeaderLog.Add("B6200000000000000000");
            _expectedParkingAnimationUartCan1HeaderLog.Add("B8200000333333333333");
            _expectedParkingAnimationUartCan1HeaderLog.Add("B9203333333333333333");
            _expectedParkingAnimationUartCan1HeaderLog.Add("BA203333333333333333");
            _expectedParkingAnimationUartCan1HeaderLog.Add("A0280D0D0D0D");
            _expectedParkingAnimationUartCan1HeaderLog.Add("A1280D0D3333");
            //_expectedParkingAnimationUartCan1HeaderLog.Add("A22800000000");
            //_expectedParkingAnimationUartCan1HeaderLog.Add("A32800000000");
            //_expectedParkingAnimationUartCan1HeaderLog.Add("A42800000000");
            //_expectedParkingAnimationUartCan1HeaderLog.Add("A52800000000");
            //_expectedParkingAnimationUartCan1HeaderLog.Add("A62800000000");
            _expectedParkingAnimationUartCan1HeaderLog.Add("A82833333333");
            _expectedParkingAnimationUartCan1HeaderLog.Add("A9280D0D0D0D");
            _expectedParkingAnimationUartCan1HeaderLog.Add("AA280D0D0D0D");

            _expectedParkingAnimationUartCan2HeaderLog.Add("B020333333330D0D0D0D");
            _expectedParkingAnimationUartCan2HeaderLog.Add("B1200D0D0D0D0D0D0D0D");
            //_expectedParkingAnimationUartCan2HeaderLog.Add("B2200000000000000000");
            //_expectedParkingAnimationUartCan2HeaderLog.Add("B3200000000000000000");
            _expectedParkingAnimationUartCan2HeaderLog.Add("B4200D0D0D0D0D0D0D0D");
            //_expectedParkingAnimationUartCan2HeaderLog.Add("B5200000000000000000");
            //_expectedParkingAnimationUartCan2HeaderLog.Add("B6200000000000000000");
            //_expectedParkingAnimationUartCan2HeaderLog.Add("B7200000000000000000");
            _expectedParkingAnimationUartCan2HeaderLog.Add("B8200000333333333333");
            _expectedParkingAnimationUartCan2HeaderLog.Add("B9203333333333333333");
            _expectedParkingAnimationUartCan2HeaderLog.Add("BA203333333333333333");
            _expectedParkingAnimationUartCan2HeaderLog.Add("A0280D0D0D0D");
            _expectedParkingAnimationUartCan2HeaderLog.Add("A1280D0D3333");
            //_expectedParkingAnimationUartCan2HeaderLog.Add("A22800000000");
            //_expectedParkingAnimationUartCan2HeaderLog.Add("A32800000000");
            //_expectedParkingAnimationUartCan2HeaderLog.Add("A42800000000");
            //_expectedParkingAnimationUartCan2HeaderLog.Add("A52800000000");
            //_expectedParkingAnimationUartCan2HeaderLog.Add("A62800000000");
            //_expectedParkingAnimationUartCan2HeaderLog.Add("A72800000000");
            _expectedParkingAnimationUartCan2HeaderLog.Add("A82833333333");
            _expectedParkingAnimationUartCan2HeaderLog.Add("A9280D0D0D0D");
            _expectedParkingAnimationUartCan2HeaderLog.Add("AA280D0D0D0D");

            #endregion
        }

        private bool _bInWelComeShow;
        private readonly List<string> _actualUartCan1Log = new List<string>();
        private readonly List<string> _actualUartCan2Log = new List<string>();

        private bool _bInParkingAnimationShow;
        private readonly List<string> _actualUartCan1LogInParkingAnimationShow = new List<string>();
        private readonly List<string> _actualUartCan2LogInParkingAnimationShow = new List<string>();

        [Description("R,检测动画时UartCan1的数据结果")]
        public string CheckWelcomeShowUartCan1DataResult = string.Empty;
        [Description("R,检测动画时UartCan2的数据结果")]
        public string CheckWelcomeShowUartCan2DataResult = string.Empty;

        [Description("R,检测驻车动画时UartCan1的数据结果")]
        public string CheckParkingShowUartCan1DataResult = string.Empty;
        [Description("R,检测驻车动画时UartCan2的数据结果")]
        public string CheckParkingShowUartCan2DataResult = string.Empty;

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            var dataStr = ValueHelper.GetHextStr(datas).Replace(" ", "");

            if (_bInWelComeShow && BCM_WelcomeLightReq >= 2 && BCM_WelcomeLightReq <= 0x0B)
            {
                if (UartCan1BaudRate1M != null && UartCan1BaudRate1M.Name == name)
                {
                    Console.WriteLine("uart_can1 recv data when welcome{0} showing: {1}", BCM_WelcomeLightReq, ValueHelper.GetHextStr(datas));

                    {
                        var headers = _expectedUartCan1HeaderLog[BCM_WelcomeLightReq];
                        foreach (var header in headers.Where(header => dataStr.Contains(header) && !_actualUartCan1Log.Contains(dataStr)))
                            _actualUartCan1Log.Add(dataStr);
                    }

                    {
                        var tails = _expectedUartCan1TailLog[BCM_WelcomeLightReq];
                        foreach (var tail in tails.Where(tail => dataStr.Contains(tail) && !_actualUartCan1Log.Contains(dataStr)))
                            _actualUartCan1Log.Add(dataStr);
                    }
                }

                if (UartCan2BaudRate1M != null && UartCan2BaudRate1M.Name == name)
                {
                    Console.WriteLine("uart_can2 recv data when parking{0} showing: {1}", BCM_WelcomeLightReq, ValueHelper.GetHextStr(datas));

                    {
                        var headers = _expectedUartCan2HeaderLog[BCM_WelcomeLightReq];
                        foreach (var header in headers.Where(header => dataStr.Contains(header) && !_actualUartCan2Log.Contains(dataStr)))
                            _actualUartCan2Log.Add(dataStr);
                    }

                    {
                        var tails = _expectedUartCan2TailLog[BCM_WelcomeLightReq];
                        foreach (var tail in tails.Where(tail => dataStr.Contains(tail) && !_actualUartCan2Log.Contains(dataStr)))
                            _actualUartCan2Log.Add(dataStr);
                    }
                }
            }

            if (_bInParkingAnimationShow && BCM_ParkLightReq == 0x02)
            {
                if (UartCan1BaudRate1M != null && UartCan1BaudRate1M.Name == name)
                {
                    Console.WriteLine("uart_can1 recv data when parking{0} showing: {1}", BCM_ParkLightReq, ValueHelper.GetHextStr(datas));

                    foreach (var header in _expectedParkingAnimationUartCan1HeaderLog.Where(header => dataStr.Contains(header) && !_actualUartCan1LogInParkingAnimationShow.Contains(dataStr)))
                        _actualUartCan1LogInParkingAnimationShow.Add(dataStr);
                }

                if (UartCan2BaudRate1M != null && UartCan2BaudRate1M.Name == name)
                {
                    Console.WriteLine("uart_can2 recv data when welcome{0} showing: {1}", BCM_ParkLightReq, ValueHelper.GetHextStr(datas));

                    foreach (var header in _expectedParkingAnimationUartCan2HeaderLog.Where(header => dataStr.Contains(header) && !_actualUartCan2LogInParkingAnimationShow.Contains(dataStr)))
                        _actualUartCan2LogInParkingAnimationShow.Add(dataStr);
                }
            }
        }

        #endregion
    }
}
