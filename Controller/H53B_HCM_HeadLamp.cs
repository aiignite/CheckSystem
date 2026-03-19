using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,H53B")]
    public sealed class H53B_HCM_HeadLamp : ControllerBase
    {
        #region Files

        public CanBus Can;
        public static object _sendLock = new object();
        public readonly Thread _mainWorkThread;
        public bool _isSleep = true;
        [Description("R/W, 右转向灯请求信号")] public byte BCM_RightTurnLampReq = 0x00;
        [Description("R/W, 右前昼间行驶灯请求信号")] public byte BCM_FRdrlReq = 0x00;
        [Description("R/W, 左前昼间行驶灯请求信号")] public byte BCM_FLdrlReq = 0x00;
        [Description("R/W, 大灯高度手动调节请求")] public byte BCM_EXLHeightAdjtReq = 0x00;
        [Description("R/W, ADB模式请求")] public byte BCM_ADBModeReq = 0x00;
        [Description("R/W, 左前位置灯请求信号")] public byte BCM_FLPosLampReq = 0x00;
        [Description("R/W, 右前位置灯请求信号")] public byte BCM_FRPosLampReq = 0x00;
        [Description("R/W, 近光灯请求信号")] public byte BCM_LowBeamReq = 0x00;
        [Description("R/W, 远光灯请求信号")] public byte BCM_HighBeamReq = 0x00;
        [Description("R/W, 左转向灯请求信号")] public byte BCM_LeftTurnLampReq = 0x00;
        [Description("R/W, 转向灯闪烁频率请求")] public byte BCM_turnLampFlickerReq = 0x00;
        [Description("R/W, 转向灯流水请求")] public byte BCM_turnLampSerialReq = 0x00;

        [Description("R/W, every message increments the counter")]
        public byte RollingCounter2A1 = 0x00;

        [Description("R/W, CRC8, checksum.")] public byte Checksum2A1 = 0x00;

        public readonly CanCommunicationMatrix.MotorolaMatrix _data0X2A1 =
            new CanCommunicationMatrix.MotorolaMatrix(0x2A1, 8);

        private readonly Dictionary<HH53BOnOrOffType, MatrixValDefinition> _lampOperaterDic =
            new Dictionary<HH53BOnOrOffType, MatrixValDefinition>();

        private bool LeftTurnFlicker = false;
        private bool RightTurnFlicker = false;
        private bool MotorState = false;

        #endregion

        public H53B_HCM_HeadLamp(string name) : base(name)
        {
            foreach (var temp in Enum.GetValues(typeof(HH53BOnOrOffType)).Cast<HH53BOnOrOffType>())
                _lampOperaterDic.Add(temp, temp.GetCustomAttribute<MatrixValDefinition>());
            _mainWorkThread = new Thread(CyNormalCyclicTimer);
            _mainWorkThread.Start();
        }

        ~H53B_HCM_HeadLamp() => Dispose();

        private void CyNormalCyclicTimer()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(25);

                lock (_sendLock)
                {
                    try
                    {
                        if (Can == null)
                            continue;

                        if (!_isSleep)
                        {
                            _data0X2A1.UpdateData(new MatrixValDefinition(30, 2, BCM_RightTurnLampReq));
                            _data0X2A1.UpdateData(new MatrixValDefinition(16, 2, BCM_FRdrlReq));
                            _data0X2A1.UpdateData(new MatrixValDefinition(2, 2, BCM_FLdrlReq));
                            _data0X2A1.UpdateData(new MatrixValDefinition(9, 3, BCM_EXLHeightAdjtReq));
                            _data0X2A1.UpdateData(new MatrixValDefinition(12, 2, BCM_ADBModeReq));
                            _data0X2A1.UpdateData(new MatrixValDefinition(14, 2, BCM_FLPosLampReq));
                            _data0X2A1.UpdateData(new MatrixValDefinition(18, 2, BCM_FRPosLampReq));
                            _data0X2A1.UpdateData(new MatrixValDefinition(20, 2, BCM_LowBeamReq));
                            _data0X2A1.UpdateData(new MatrixValDefinition(22, 2, BCM_HighBeamReq));
                            _data0X2A1.UpdateData(new MatrixValDefinition(28, 2, BCM_LeftTurnLampReq));
                            _data0X2A1.UpdateData(new MatrixValDefinition(36, 2, BCM_turnLampFlickerReq));
                            _data0X2A1.UpdateData(new MatrixValDefinition(38, 2, BCM_turnLampSerialReq));
                            _data0X2A1.UpdateData(new MatrixValDefinition(48, 4, RollingCounter2A1));

                            var toCheckCrc = new List<byte>();
                            toCheckCrc.Add(0x02);
                            toCheckCrc.Add(0xA1);
                            var tpToCheckData = new byte[7];
                            Array.Copy(_data0X2A1.MatrixData, 0, tpToCheckData, 0, 7);
                            toCheckCrc.AddRange(tpToCheckData);
                            var checksum2A1 = crc_8find(toCheckCrc.ToArray(), toCheckCrc.Count);
                            _data0X2A1.UpdateData(new MatrixValDefinition(56, 8, checksum2A1));

                            RollingCounter2A1++;
                            if (RollingCounter2A1 > 15)
                                RollingCounter2A1 = 0;

                            Can.SendStandardCanData(_data0X2A1.CanId, _data0X2A1.MatrixData);
                            Can.SendStandardCanData(0x666,
                                new byte[] { _logoPwm, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                            Thread.Sleep(50);
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        [Description("打开Can发送")]
        public void LinStartScheduler() => _isSleep = false;

        [Description("关闭Can发送")]
        public void LinStopScheduler() => _isSleep = true;

        [Description("TurnLOn")]
        public void TurnLOn() => BCM_LeftTurnLampReq = 0x02;

        [Description("TurnLOff")]
        public void TurnLOff() => BCM_LeftTurnLampReq = 0x01;

        [Description("TurnROn")]
        public void TurnROn() => BCM_RightTurnLampReq = 0x02;

        [Description("TurnROff")]
        public void TurnROff() => BCM_RightTurnLampReq = 0x01;

        [Description("DrlROn")]
        public void DrlROn() => BCM_FRdrlReq = 0x02;

        [Description("DrlROff")]
        public void DrlROff() => BCM_FRdrlReq = 0x01;

        [Description("DrlLOn")]
        public void DrlLOn() => BCM_FLdrlReq = 0x02;

        [Description("DrlLOff")]
        public void DrlLOff() => BCM_FLdrlReq = 0x01;

        [Description("TailLOn")]
        public void TailLOn() => BCM_FLPosLampReq = 0x02;

        [Description("TailLOff")]
        public void TailLOff() => BCM_FLPosLampReq = 0x01;

        [Description("TailROn")]
        public void TailROn() => BCM_FRPosLampReq = 0x02;

        [Description("TailROff")]
        public void TailROff() => BCM_FRPosLampReq = 0x01;

        [Description("BCM_LowBeamReqOn")]
        public void BCM_LowBeamReqOn() => BCM_LowBeamReq = 0x02;

        [Description("BCM_LowBeamReqOff")]
        public void BCM_LowBeamReqOff() => BCM_LowBeamReq = 0x01;

        [Description("BCM_HighBeamReqOn")]
        public void BCM_HighBeamReqOn() => BCM_HighBeamReq = 0x02;

        [Description("BCM_HighBeamReqOff")]
        public void BCM_HighBeamReqOff() => BCM_HighBeamReq = 0x01;

        [Description("BCM_LeftTurnLampReqOn")]
        public void BCM_LeftTurnLampReqOn() => BCM_LeftTurnLampReq = 0x02;

        [Description("BCM_LeftTurnLampReqOff")]
        public void BCM_LeftTurnLampReqOff() => BCM_LeftTurnLampReq = 0x01;

        [Description("BCM_turnLampFlickerReqOn")]
        public void BCM_turnLampFlickerReqOn(string value)
        {
            var values = Convert.ToByte(value, 16);
            BCM_turnLampFlickerReq = values;
        }

        [Description("BCM_turnLampSerialReqOn")]
        public void BCM_turnLampSerialReqOn() => BCM_turnLampSerialReq = 0x01;

        [Description("BCM_turnLampSerialReqOff")]
        public void BCM_turnLampSerialReqOff() => BCM_turnLampSerialReq = 0x00;

        #region 左转向闪烁

        [Description("左转向灯闪烁开")]
        public void LeftTurnLampFlickerOn()
        {
            if (LeftTurnFlicker)
                return;
            LeftTurnFlicker = true;
            new Action(() => LeftFilicker()).BeginInvoke(LeftIsCont, null);
        }

        [Description("左转向灯闪烁关")]
        public void LeftTurnLampFlickerOff() => LeftTurnFlicker = false;

        private void LeftIsCont(IAsyncResult asyncResult)
        {
            if (!LeftTurnFlicker)
            {
                BCM_LeftTurnLampReq = 0x01;
                return;
            }

            new Action(() => LeftFilicker()).BeginInvoke(LeftIsCont, null);
        }

        private void LeftFilicker()
        {
            lock (_sendLock)
            {
                if (BCM_LeftTurnLampReq == 0x02)
                    BCM_LeftTurnLampReq = 0x01;
                else
                    BCM_LeftTurnLampReq = 0x02;
                Console.WriteLine("转向灯请求信号");
                Console.WriteLine(BCM_LeftTurnLampReq);
            }

            Thread.Sleep(350);
        }

        #endregion

        #region 右转向闪烁

        [Description("右转向灯闪烁开")]
        public void RightTurnLampFlickerOn()
        {
            if (RightTurnFlicker)
                return;
            RightTurnFlicker = true;
            new Action(() => RightFilicker()).BeginInvoke(RightIsCont, null);
        }

        [Description("右转向灯闪烁关")]
        public void RightTurnLampFlickerOff() => RightTurnFlicker = false;

        private void RightIsCont(IAsyncResult asyncResult)
        {
            if (!RightTurnFlicker)
            {
                BCM_RightTurnLampReq = 0x01;
                return;
            }

            new Action(() => RightFilicker()).BeginInvoke(RightIsCont, null);
        }

        private void RightFilicker()
        {
            lock (_sendLock)
            {
                if (BCM_RightTurnLampReq == 0x02)
                    BCM_RightTurnLampReq = 0x01;
                else
                    BCM_RightTurnLampReq = 0x02;
                Console.WriteLine("转向灯请求信号");
                Console.WriteLine(BCM_RightTurnLampReq);
            }

            Thread.Sleep(400);
        }

        #endregion

        #region 马达反复动作

        [Description("马达往返运动开始")]
        public void MotorOn()
        {
            if (MotorState)
                return;
            MotorState = true;
            new Action(() => Motor()).BeginInvoke(motorIsCont, null);
        }

        [Description("马达往返运动停止")]
        public void MotorOff() => MotorState = false;

        private void motorIsCont(IAsyncResult asyncResult)
        {
            if (!MotorState)
                return;
            new Action(() => Motor()).BeginInvoke(motorIsCont, null);
        }

        private void Motor()
        {
            lock (_sendLock)
            {
                if (BCM_EXLHeightAdjtReq == 0x01)
                    BCM_EXLHeightAdjtReq = 0x05;
                else
                    BCM_EXLHeightAdjtReq = 0x01;
                Console.WriteLine("马达请求信号");
                Console.WriteLine(BCM_EXLHeightAdjtReq);
            }

            Thread.Sleep(4000);
        }

        #endregion

        private enum HH53BOnOrOffType
        {
            /// <summary>
            /// 右转向灯亮
            /// </summary>
            [MatrixValDefinition(30, 2, 2)] [Description("0x2A1")]
            BCM_RightTurnLampReqOn,

            /// <summary>
            /// 右转向灯灭
            /// </summary>
            [MatrixValDefinition(30, 2, 1)] [Description("0x2A1")]
            BCM_RightTurnLampReqOff,
        }

        public void sfe()
        {
            _data0X2A1.UpdateData(new MatrixValDefinition(30, 2, BCM_RightTurnLampReq));
            _data0X2A1.UpdateData(new MatrixValDefinition(16, 2, BCM_FRdrlReq));
            _data0X2A1.UpdateData(new MatrixValDefinition(2, 2, BCM_FLdrlReq));
            _data0X2A1.UpdateData(new MatrixValDefinition(9, 3, BCM_EXLHeightAdjtReq));
            _data0X2A1.UpdateData(new MatrixValDefinition(12, 2, BCM_ADBModeReq));
            _data0X2A1.UpdateData(new MatrixValDefinition(14, 2, BCM_FLPosLampReq));
            _data0X2A1.UpdateData(new MatrixValDefinition(18, 2, BCM_FRPosLampReq));
            _data0X2A1.UpdateData(new MatrixValDefinition(20, 2, BCM_LowBeamReq));
            _data0X2A1.UpdateData(new MatrixValDefinition(22, 2, BCM_HighBeamReq));
            _data0X2A1.UpdateData(new MatrixValDefinition(28, 2, BCM_LeftTurnLampReq));
            _data0X2A1.UpdateData(new MatrixValDefinition(36, 2, BCM_turnLampFlickerReq));
            _data0X2A1.UpdateData(new MatrixValDefinition(38, 2, BCM_turnLampSerialReq));
            _data0X2A1.UpdateData(new MatrixValDefinition(56, 8, Checksum2A1));
        }

        private byte[] crc_table = new byte[]
        {
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

        private byte _logoPwm = 0x00;

        [Description("LOGO以占空比打开")]
        public void LogoOn(string pwm)
        {
            if (!byte.TryParse(pwm, out var value)) return;
            if (value <= 100)
                _logoPwm = value;
        }

        [Description("LOGO关")]
        public void LogoOff() => _logoPwm = 0;
    }
}