using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,H77尾灯")]
    public sealed class H77RearLamp : ControllerBase
    {
        public LinBus LinWithBaudRate10417;

        public H77RearLamp(string name) : base(name)
        {
            _mainWorkThread = new Thread(CyNormalCyclicTimer);
            _mainWorkThread.Start();
        }

        ~H77RearLamp()
        {
            Dispose();
        }

        #region 点灯

        private readonly Thread _mainWorkThread;
        private bool _isSleep = true;
        private readonly object _lockLin = new object();
        private readonly LinCommunicationMatrix.IntelMatrix _intelMatrix0X00 =
            new LinCommunicationMatrix.IntelMatrix(0x00, 4);
        private byte _emcMode = 1;

        private void CyNormalCyclicTimer()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(25);

                lock (_lockLin)
                {
                    try
                    {
                        if (LinWithBaudRate10417 == null)
                            continue;

                        if (_emcMode != 0x05)
                        {
                            //_intelMatrix0X00.UpdateData(new MatrixValDefinition(15, 1, 1));
                            //_intelMatrix0X00.UpdateData(new MatrixValDefinition(23, 1, 1));

                            if (!_isSleep)
                            {
                                LinWithBaudRate10417.SendMasterLin(0x39, new byte[] { _emcMode, 0x00, 0x00, 0x00 });
                                LinWithBaudRate10417.SendMasterLin(_intelMatrix0X00.MasterLinId, _intelMatrix0X00.MatrixData);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        [Description("打开LIN发送")]
        public void LinStartScheduler()
        {
            _isSleep = false;
        }

        [Description("关闭LIN发送")]
        public void LinStopScheduler()
        {
            _isSleep = true;
        }

        [Description("EMC_MODE")]
        public void EmcMode(string modeIndex)
        {
            byte index;
            if (byte.TryParse(modeIndex, out index))
                if (index >= 1 && index <= 8 && index != 5)
                    _emcMode = index;
        }

        [Description("EMC_MODE5_SLEEP")]
        public void EmcSleep()
        {
            _emcMode = 0x05;
            if (LinWithBaudRate10417 == null)
                return;
            lock (_lockLin)
            {
                for (var i = 0; i < 20; i++)
                {
                    LinWithBaudRate10417.SendMasterLin(0x39, new byte[] { _emcMode, 0x00, 0x00, 0x00 });
                    LinWithBaudRate10417.SendMasterLin(_intelMatrix0X00.MasterLinId, _intelMatrix0X00.MatrixData);
                    Thread.Sleep(5);
                }

                LinWithBaudRate10417.SendMasterLin(0x3C, new byte[] { 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
            }
        }

        [Description("L_位置灯OFF")]
        public void LeftParkLampOff()
        {
            UpdateParkLampSignal(true, 0);
        }

        [Description("L_位置灯根据信号打开")]
        public void LeftParkLampOn(int value)
        {
            if (value >= 0 && value <= 8)
                UpdateParkLampSignal(true, (byte)value);
        }

        [Description("L_位置灯信号1")]
        public void LeftParkLamp1()
        {
            UpdateParkLampSignal(true, 1);
        }

        [Description("L_位置灯信号7")]
        public void LeftParkLamp7()
        {
            UpdateParkLampSignal(true, 7);
        }

        [Description("R_位置灯OFF")]
        public void RightParkLampOff()
        {
            UpdateParkLampSignal(false, 0);
        }

        [Description("R_位置灯根据信号打开")]
        public void RightParkLampOn(int value)
        {
            if (value >= 0 && value <= 8)
                UpdateParkLampSignal(false, (byte)value);
        }

        [Description("R_位置灯信号1")]
        public void RightParkLamp1()
        {
            UpdateParkLampSignal(false, 1);
        }

        [Description("R_位置灯信号7")]
        public void RightParkLamp7()
        {
            UpdateParkLampSignal(false, 7);
        }

        private void UpdateParkLampSignal(bool isLeft, byte lampIndex)
        {
            _intelMatrix0X00.UpdateData(isLeft
                ? new MatrixValDefinition(16, 4, lampIndex)
                : new MatrixValDefinition(8, 4, lampIndex));
        }

        [Description("L_转向灯OFF")]
        public void LeftTurnOff()
        {
            UpdateTurnLampSignal(true, 0);
        }

        [Description("L_转向灯根据信号打开")]
        public void LeftTurnOn(int value)
        {
            if (value >= 0 && value <= 8)
                UpdateTurnLampSignal(true, (byte)value);
        }

        [Description("L_转向灯信号1")]
        public void LeftTurnLamp1()
        {
            UpdateTurnLampSignal(true, 1);
        }

        [Description("L_转向灯信号2")]
        public void LeftTurnLamp2()
        {
            UpdateTurnLampSignal(true, 2);
        }

        [Description("L_转向灯信号3")]
        public void LeftTurnLamp3()
        {
            UpdateTurnLampSignal(true, 3);
        }

        [Description("L_转向灯信号4")]
        public void LeftTurnLamp4()
        {
            UpdateTurnLampSignal(true, 4);
        }

        [Description("R_转向灯OFF")]
        public void RightTurnOff()
        {
            UpdateTurnLampSignal(false, 0);
        }

        [Description("R_转向灯根据信号打开")]
        public void RightTurnOn(int value)
        {
            if (value >= 0 && value <= 8)
                UpdateTurnLampSignal(false, (byte)value);
        }

        [Description("R_转向灯信号1")]
        public void RightTurnLamp1()
        {
            UpdateTurnLampSignal(false, 1);
        }

        [Description("R_转向灯信号2")]
        public void RightTurnLamp2()
        {
            UpdateTurnLampSignal(false, 2);
        }

        [Description("R_转向灯信号3")]
        public void RightTurnLamp3()
        {
            UpdateTurnLampSignal(false, 3);
        }

        [Description("R_转向灯信号4")]
        public void RightTurnLamp4()
        {
            UpdateTurnLampSignal(false, 4);
        }

        [Description("RrLmpShwMod_64_5")]
        public void RrLmpShwMod_64_5(string mod)
        {
            byte mode;
            if (!byte.TryParse(mod, out mode))
                return;
            if (mode <= 31)
                _intelMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, mode));
        }

        [Description("RrLmpShwTyp_64_5")]
        public void RrLmpShwTyp_64_5(string typ)
        {
            byte type;
            if (!byte.TryParse(typ, out type))
                return;
            if (type <= 7)
                _intelMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, type));
        }

        private void UpdateTurnLampSignal(bool isLeft, byte lampIndex)
        {
            _intelMatrix0X00.UpdateData(isLeft
                ? new MatrixValDefinition(20, 3, lampIndex)
                : new MatrixValDefinition(12, 3, lampIndex));
        }

        [Description("L_制动灯按信号打开")]
        public void LeftStopOn(int value)
        {
            if (value >= 0 && value <= 1)
            {
                _intelMatrix0X00.UpdateData(new MatrixValDefinition(23, 1, (byte)value));
            }
        }

        [Description("R_制动灯按信号打开")]
        public void RightStopOn(int value)
        {
            if (value >= 0 && value <= 1)
            {
                _intelMatrix0X00.UpdateData(new MatrixValDefinition(24, 1, (byte)value));
            }
        }

        [Description("LOGO灯按信号打开")]
        public void LogoOn(int value)
        {
            if (value >= 0 && value <= 2)
            {
                _intelMatrix0X00.UpdateData(new MatrixValDefinition(25, 2, (byte)value));
            }
        }

        #endregion

        #region 3C&3D指令

        [Description("R,LeftBoot")]
        public string LeftBoot = string.Empty;
        [Description("R,RightBoot")]
        public string RightBoot = string.Empty;
        [Description("R,LeftApp")]
        public string LeftApp = string.Empty;
        [Description("R,RightApp")]
        public string RightApp = string.Empty;

        public void SendCorrectSleepCmd()
        {
            Send3CCmd(1);
        }

        public void SendIncorrectSleepCmd()
        {
            Send3CCmd(2);
        }

        public void LeftReset()
        {
            Send3CCmd(3);
        }

        public void RightReset()
        {
            Send3CCmd(4);
        }

        [Description("读左BOOT")]
        public void ReadLeftBoot()
        {
            LeftBoot = string.Empty;
            LeftBoot = Send3CCmd(5).GetStringByAsciiBytes(false);
        }

        [Description("读左APP")]
        public void ReadLeftApp()
        {
            LeftBoot = string.Empty;
            LeftBoot = Send3CCmd(6).GetStringByAsciiBytes(false);
        }

        [Description("读右BOOT")]
        public void ReadRightBoot()
        {
            RightBoot = string.Empty;
            RightBoot = Send3CCmd(7).GetStringByAsciiBytes(false);
        }

        [Description("读右APP")]
        public void ReadRightApp()
        {
            RightBoot = string.Empty;
            RightBoot = Send3CCmd(8).GetStringByAsciiBytes(false);
        }

        private byte[] Send3CCmd(int g3CType)
        {
            if (LinWithBaudRate10417 != null)
            {
                const byte reqLinId = 0x3C;
                const byte recvLinId = 0x3D;
                const byte leftNad = 0x64;
                const byte rightNad = 0x66;

                switch (g3CType)
                {
                    case 1:
                        lock (_lockLin)
                            LinWithBaudRate10417.SendMasterLin(reqLinId,
                                new byte[] { 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
                        break;

                    case 2:
                        lock (_lockLin)
                            LinWithBaudRate10417.SendMasterLin(reqLinId,
                                new byte[] { 0x05, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
                        break;

                    case 3:
                        lock (_lockLin)
                            LinWithBaudRate10417.SendMasterLin(reqLinId,
                                new byte[] { leftNad, 0x01, 0xB5, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
                        break;

                    case 4:
                        lock (_lockLin)
                            LinWithBaudRate10417.SendMasterLin(reqLinId,
                                new byte[] { rightNad, 0x01, 0xB5, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
                        break;

                    case 5:
                        return ReadVer(reqLinId, recvLinId, leftNad, true);

                    case 6:
                        return ReadVer(reqLinId, recvLinId, leftNad, false);

                    case 7:
                        return ReadVer(reqLinId, recvLinId, rightNad, true);

                    case 8:
                        return ReadVer(reqLinId, recvLinId, rightNad, false);
                }
            }

            return new byte[0];
        }

        private byte[] ReadVer(byte reqLinId, byte recvLinId, byte nad, bool isBoot)
        {
            const byte didHi = 0xFC;
            var didLo = (byte)(isBoot ? 0x03 : 0x06);

            lock (_lockLin)
            {
                var sendBytes = new byte[] { nad, 0x03, 0x22, didHi, didLo, 0x00, 0x00, 0x00 };
                LinWithBaudRate10417.SendMasterLin(0x3C, sendBytes);
                var resultBs = new List<byte>();

                Thread.Sleep(100);
                byte[] echo1;
                var isReadFirstFrameSucceed = LinWithBaudRate10417.SendSlaveLin(0x3D, out echo1);
                if (echo1 != null && echo1.Length == 8 &&
                    echo1[0] == nad && echo1[1] == 0x10 &&
                    echo1[3] == 0x62 && echo1[4] == didHi && echo1[5] == didLo)
                {
                    byte[] echo2;
                    var isSucceed = LinWithBaudRate10417.SendSlaveLin(0x3D, out echo2);
                    if (isSucceed && echo2 != null && echo2.Length == 8 && echo2[0] == nad && echo2[1] == 0x20)
                    {
                        resultBs.Add(echo1[6]);
                        resultBs.Add(echo1[7]);
                        for (var j = 2; j < 8; j++)
                            if (echo2[j] != 0x00 && echo2[j] != 0x55 && echo2[j] != 0xAA && echo2[j] != 0xFF)
                                resultBs.Add(echo2[j]);

                        return resultBs.ToArray();
                    }
                }
            }

            return new byte[0];
        }

        #endregion
    }
}
