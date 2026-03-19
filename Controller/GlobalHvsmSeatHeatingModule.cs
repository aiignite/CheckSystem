using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,GlobalHVSM座椅加热模块")]
    public sealed class GlobalHvsmSeatHeatingModule : ControllerBase
    {
        public LinBus LinWithBaudRate10417;

        [Description("R,静态电流")]
        public double QuiescentCurrent;
        [Description("R,外壳二维码")]
        public string Barcode = string.Empty;
        [Description("R,外壳二维码-Modbus通讯格式")]
        public string ModbusBarcode = string.Empty;
        [Description("R,PCBA二维码")]
        public string PcbaBarcode = string.Empty;

        [Description("R,VBAT1采样")]
        public float VBAT1 = float.MinValue;
        [Description("R,VBAT2采样")]
        public float VBAT2 = float.MinValue;

        [Description("R,NTC信号状态-DriverBackSignal信号检测")]
        public float DriverBackSignalAdc = float.MinValue;
        [Description("R,NTC信号状态-Co-DriverBackSignal信号检测")]
        public float CoDriverBackSignalAdc = float.MinValue;
        [Description("R,NTC信号状态-DriverCushionSignal信号检测")]
        public float DriverCushionSignalAdc = float.MinValue;
        [Description("R,NTC信号状态-Co-DriverCushionSignal信号检测")]
        public float CoDriverCushionSignalAdc = float.MinValue;

        [Description("R,DiscreteInput信号检测")]
        public string DiscreteInput = string.Empty;

        [Description("R,DriverBack电流检测")]
        public float DriverBackCurr = float.MinValue;
        [Description("R,Co-DriverBack电流检测")]
        public float CoDriverBackCurr = float.MinValue;
        [Description("R,DriverCushion电流检测")]
        public float DriverCushionCurr = float.MinValue;
        [Description("R,Co-DriverCushion电流检测")]
        public float CoDriverCushionCurr = float.MinValue;

        [Description("R,小电流电路检测Left")]
        public float LowCurrentCircuitLeft = float.MinValue;
        [Description("R,小电流电路检测Right")]
        public float LowCurrentCircuitRight = float.MinValue;

        [Description("R,软件APP版本号")]
        public string AppVer = string.Empty;
        [Description("R,软件BOOT版本号")]
        public string BootVer = string.Empty;

        public GlobalHvsmSeatHeatingModule(string name) : base(name)
        {
            LinBus.PushLinMsg += LinBus_PushLinMsg;
            _mainWorkThread = new Thread(LinNetWork) { IsBackground = true };
            _mainWorkThread.Start();
        }

        private void LinBus_PushLinMsg(string name, LinBus.LinDataPackage data)
        {
            if (LinWithBaudRate10417 != null && !string.IsNullOrEmpty(name) && LinWithBaudRate10417.Name == name)
            {
                if ((data.LinId == LinBus.ConvertLinId(0x25) || data.LinId == 0x25) && data.LinData.Length == 4)
                {
                    var d1 = data.LinData[0];
                    var d2 = data.LinData[1];
                    var d3 = data.LinData[2];
                    var d4 = data.LinData[3];
                }
            }
        }

        ~GlobalHvsmSeatHeatingModule()
        {
            Dispose();
        }

        private bool _isStopLin = true;
        private readonly Thread _mainWorkThread;
        private readonly object _lockSend = new object();
        private byte _nad = (byte)Row.Back;
        private byte[] _msg0x26 = new byte[2];
        private byte[] _msg0x27 = new byte[4];

        private const double PwmFactor = 0.392157d;

        private byte _leftBackrestHeatingPwm;
        private byte _rightBackrestHeatingPwm;
        private byte _leftSeatCushioHeatingPwm;
        private byte _rightSeatCushioHeatingPwm;

        private byte _leftVentilationPwm;
        private byte _rightVentilationPwm;

        private Row _cRow = Row.Back;

        internal enum Row : byte
        {
            /// <summary>
            /// 前排
            /// DiscreteInput需悬空
            /// </summary>
            Front = 0x64,

            /// <summary>
            /// 后排
            /// DiscreteInput需接gnd
            /// </summary>
            Back = 0x66
        }

        #region 周期报文

        private void LinNetWork()
        {
            var sendCount = 0;
            var sendPeriod = 5;

            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    return;

                Thread.Sleep(sendPeriod);

                if (LinWithBaudRate10417 == null)
                    continue;

                if (_isStopLin)
                {
                    sendCount = 0;
                    continue;
                }

                lock (_lockSend)
                {
                    if (sendCount == 0) // 5ms
                    {
                        // send _msg0x26
                        // send 0x11/0x19
                        //_msg0x26[1] = _faultModeOpenTime;
                        LinWithBaudRate10417.SendMasterLin(0x26, _msg0x26);
                        var msg = new byte[] { _leftVentilationPwm, _rightVentilationPwm, 0xFF, 0xFC };
                        LinWithBaudRate10417.SendMasterLin(_cRow == Row.Front ? (byte)0x11 : (byte)0x19, msg);
                    }
                    else if (sendCount == 1) // 10ms
                    {
                        // send _msg0x27
                        // 0x25
                        LinWithBaudRate10417.SendMasterLin(0x27, _msg0x27);
                        LinWithBaudRate10417.SendMasterLin(0x25, new byte[0]);
                    }
                    else if (sendCount == 2) // 15ms
                    {
                        // send _msg0x26
                        // 0x35
                        LinWithBaudRate10417.SendMasterLin(0x26, _msg0x26);
                        LinWithBaudRate10417.SendMasterLin(0x35, new byte[0]);
                    }
                    else if (sendCount == 3) // 20ms
                    {
                        // send _msg0x27
                        // send 0x10/0x18
                        LinWithBaudRate10417.SendMasterLin(0x27, _msg0x27);
                        var msg = new byte[] { _leftBackrestHeatingPwm, _rightBackrestHeatingPwm, _leftSeatCushioHeatingPwm, _rightSeatCushioHeatingPwm };
                        LinWithBaudRate10417.SendMasterLin(_cRow == Row.Front ? (byte)0x10 : (byte)0x18, msg);
                    }
                }

                sendCount++;
                if (sendCount > 3)
                    sendCount = 0;
            }
        }

        [Description("打开LIN")]
        public void StartLin()
        {
            _isStopLin = false;
        }

        [Description("关闭LIN")]
        public void StopLin()
        {
            _isStopLin = true;
        }

        [Description("正常模式")]
        public void NormalMode()
        {
            _msg0x26[0] = 0x00;
        }

        [Description("温度模式")]
        public void TempMode()
        {
            _msg0x26[0] = 0x01;
        }

        [Description("PWM模式")]
        public void PwmMode()
        {
            _msg0x26[0] = 0x02;
        }

        [Description("DriverFanPwm")]
        public void DriverFanPwm(double pwm)
        {
            if (pwm < 0 || pwm > 100)
                return;

            _leftVentilationPwm = (byte)Math.Round(pwm / PwmFactor, 0, MidpointRounding.AwayFromZero);
        }

        [Description("CoDriverFanPwm")]
        public void CoDriverFanPwm(double pwm)
        {
            if (pwm < 0 || pwm > 100)
                return;

            _rightVentilationPwm = (byte)Math.Round(pwm / PwmFactor, 0, MidpointRounding.AwayFromZero);
        }

        [Description("DriverBackHsdPwm")]
        public void DriverBackHsdPwm(double pwm)
        {
            if (pwm < 0 || pwm > 100)
                return;

            _rightBackrestHeatingPwm = (byte)Math.Round(pwm / PwmFactor, 0, MidpointRounding.AwayFromZero);
        }

        [Description("CoDriverBackHsdPwm")]
        public void CoDriverBackHsdPwm(double pwm)
        {
            if (pwm < 0 || pwm > 100)
                return;

            _rightSeatCushioHeatingPwm = (byte)Math.Round(pwm / PwmFactor, 0, MidpointRounding.AwayFromZero);
        }

        [Description("DriverCushionHsdPwm")]
        public void DriverCushionHsdPwm(double pwm)
        {
            if (pwm < 0 || pwm > 100)
                return;

            _leftBackrestHeatingPwm = (byte)Math.Round(pwm / PwmFactor, 0, MidpointRounding.AwayFromZero);
        }

        [Description("CoDriverCushionHsdPwm")]
        public void CoDriverCushionHsdPwm(double pwm)
        {
            if (pwm < 0 || pwm > 100)
                return;

            _leftSeatCushioHeatingPwm = (byte)Math.Round(pwm / PwmFactor, 0, MidpointRounding.AwayFromZero);
        }

        #endregion

        #region 诊断

        [Description("设置成前排-DiscreteInput需悬空")]
        public void SetFrontRow()
        {
            _cRow = Row.Front;
            _nad = (byte)_cRow;
        }

        [Description("设置成后排-DiscreteInput需接GND")]
        public void SetBakRow()
        {
            _cRow = Row.Back;
            _nad = (byte)_cRow;
        }

        [Description("读软件APP版本号")]
        public void ReadAppVer()
        {
            AppVer = string.Empty;
            var result = ReadDid(6, 0xFC, 0x06, _nad);
            AppVer = Encoding.ASCII.GetString(result, 0, result.Length);
        }

        [Description("读软件Boot版本号")]
        public void ReadBootVer()
        {
            BootVer = string.Empty;
            var result = ReadDid(6, 0xFC, 0x03, _nad);
            BootVer = Encoding.ASCII.GetString(result, 0, result.Length);
        }

        [Description("读VBAT1")]
        public void ReadVBAT1()
        {
            VBAT1 = float.MinValue;
            var result = ReadDid(2, 0xFF, 0x02, _nad);
            if (result.Length == 2)
                VBAT1 = (result[0] * 256 + result[1]) * 0.001f;
        }

        [Description("读VBAT2")]
        public void ReadVBAT2()
        {
            VBAT2 = float.MinValue;
            var result = ReadDid(2, 0xFF, 0x03, _nad);
            if (result.Length == 2)
                VBAT2 = (result[0] * 256 + result[1]) * 0.001f;
        }

        [Description("读DriverBackSignal信号检测")]
        public void ReadDriverBackSignalAdc()
        {
            DriverBackSignalAdc = float.MinValue;
            var result = ReadDid(2, 0xFF, 0x04, _nad);
            if (result.Length == 2)
                DriverBackSignalAdc = (float)Math.Round((result[0] * 256 + result[1]) * 5000d / 4096d * 0.001d, 2, MidpointRounding.AwayFromZero);
        }

        [Description("读Co-DriverBackSignal信号检测")]
        public void ReadCoDriverBackSignalAdc()
        {
            CoDriverBackSignalAdc = float.MinValue;
            var result = ReadDid(2, 0xFF, 0x05, _nad);
            if (result.Length == 2)
                CoDriverBackSignalAdc = (float)Math.Round((result[0] * 256 + result[1]) * 5000d / 4096d * 0.001d, 2, MidpointRounding.AwayFromZero);
        }

        [Description("读DriverCushionSignal信号检测")]
        public void ReadDriverCushionSignalAdc()
        {
            DriverCushionSignalAdc = float.MinValue;
            var result = ReadDid(2, 0xFF, 0x06, _nad);
            if (result.Length == 2)
                DriverCushionSignalAdc = (float)Math.Round((result[0] * 256 + result[1]) * 5000d / 4096d * 0.001d, 2, MidpointRounding.AwayFromZero);
        }

        [Description("读Co-DriverCushionSignal信号检测")]
        public void ReadCoDriverCushionSignalAdc()
        {
            CoDriverCushionSignalAdc = float.MinValue;
            var result = ReadDid(2, 0xFF, 0x07, _nad);
            if (result.Length == 2)
                CoDriverCushionSignalAdc = (float)Math.Round((result[0] * 256 + result[1]) * 5000d / 4096d * 0.001d, 2, MidpointRounding.AwayFromZero);
        }

        [Description("读DiscreteInput信号检测")]
        public void ReadDiscreteInput()
        {
            DiscreteInput = string.Empty;
            var result = ReadDid(2, 0xFF, 0x08, _nad);
            if (result.Length == 2)
                DiscreteInput = (result[0] * 256 + result[1]).ToString();
        }

        [Description("读小电流电路检测-Left")]
        public void ReadLowCurrentCircuitLeft()
        {
            LowCurrentCircuitLeft = float.MinValue;
            var result = ReadDid(2, 0xFF, 0x09, _nad);
            if (result.Length == 2)
                LowCurrentCircuitLeft = (result[0] * 256 + result[1]) * 0.001f;
        }

        [Description("读小电流电路检测-Right")]
        public void ReadLowCurrentCircuitRight()
        {
            LowCurrentCircuitRight = float.MinValue;
            var result = ReadDid(2, 0xFF, 0x0E, _nad);
            if (result.Length == 2)
                LowCurrentCircuitRight = (result[0] * 256 + result[1]) * 0.001f;
        }

        [Description("读DriverBack电流检测")]
        public void ReadDriverBackCurr()
        {
            DriverBackCurr = float.MinValue;
            var result = ReadDid(2, 0xFF, 0x0A, _nad);
            if (result.Length == 2)
                DriverBackCurr = (result[0] * 256 + result[1]) * 0.001f;
        }

        [Description("读Co-DriverBack电流检测")]
        public void ReadCoDriverBackCurr()
        {
            CoDriverBackCurr = float.MinValue;
            var result = ReadDid(2, 0xFF, 0x0B, _nad);
            if (result.Length == 2)
                CoDriverBackCurr = (result[0] * 256 + result[1]) * 0.001f;
        }

        [Description("读DriverCushion电流检测")]
        public void ReadDriverCushionCurr()
        {
            DriverCushionCurr = float.MinValue;
            var result = ReadDid(2, 0xFF, 0x0C, _nad);
            if (result.Length == 2)
                DriverCushionCurr = (result[0] * 256 + result[1]) * 0.001f;
        }

        [Description("读Co-DriverCushion电流检测")]
        public void ReadCoDriverCushionCurr()
        {
            CoDriverCushionCurr = float.MinValue;
            var result = ReadDid(2, 0xFF, 0x0D, _nad);
            if (result.Length == 2)
                CoDriverCushionCurr = (result[0] * 256 + result[1]) * 0.001f;
        }

        private byte[] ReadDid(int dataLen, byte didHi, byte didLo, byte nad)
        {
            if (LinWithBaudRate10417 == null)
                return new byte[0];

            var readValueDataLen = dataLen;
            var data0 = nad;

            //Console.WriteLine("config: " + infoName);
            //Console.WriteLine("config: " + standard);

            lock (_lockSend)
            {
                try
                {
                    var sendBytes = new byte[] { data0, 0x03, 0x22, didHi, didLo, 0xFF, 0xFF, 0xFF };
                    LinWithBaudRate10417.SendMasterLin(0x3C, sendBytes);

                    var resultBs = new List<byte>();

                    Thread.Sleep(100);
                    byte[] recv;
                    var isReadFirstFrameSucceed = LinWithBaudRate10417.SendSlaveLin(0x3D, out recv);
                    if (isReadFirstFrameSucceed && recv != null && recv.Length == 8)
                    {
                        if (recv[0] == data0)
                        {
                            if (recv[1] >= 0x10) // 多帧
                            {
                                if (recv[3] == 0x62 && recv[4] == didHi && recv[5] == didLo)
                                {
                                    resultBs.Add(recv[6]);
                                    resultBs.Add(recv[7]);
                                    var len = (recv[1] - 0x10) * 256 + recv[2];

                                    int count;
                                    if ((len - 5) % 6 == 0)
                                        count = (len - 5) / 6;
                                    else
                                        count = (len - 5) / 6 + 1;

                                    for (var i = 0; i < count; i++)
                                    {
                                        byte[] recvBytesRest;
                                        var isSucceed = LinWithBaudRate10417.SendSlaveLin(0x3D, out recvBytesRest);
                                        if (isSucceed && recvBytesRest != null && recvBytesRest.Length == 8)
                                        {
                                            for (var j = 2; j < 8; j++)
                                                resultBs.Add(recvBytesRest[j]);
                                        }
                                    }
                                }
                            }
                            else // 单帧
                            {
                                if (recv[2] == 0x62 && recv[3] == didHi && recv[4] == didLo)
                                {
                                    for (var i = 5; i < 5 + dataLen; i++)
                                    {
                                        resultBs.Add(recv[i]);
                                    }
                                }
                            }
                        }

                        if (resultBs.Any() && resultBs.Count >= readValueDataLen)
                        {
                            var temp3333 = new byte[readValueDataLen];
                            Array.Copy(resultBs.ToArray(), 0, temp3333, 0, readValueDataLen);
                            resultBs.Clear();
                            resultBs.AddRange(temp3333);
                        }

                        return resultBs.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    return new byte[0];
                }

                return new byte[0];
            }
        }

        #endregion

        #region 生成二维码

        [Description("生成二维码")]
        public void GenerateBarcode(string generalPartNo, string generalVpps, string seeyaoDuns, string track1, string track2, string track3)
        {
            Barcode = string.Empty;
            ModbusBarcode = string.Empty;

            try
            {
                Barcode = Generate1Barcode(generalPartNo, generalVpps, seeyaoDuns, track1, track2, track3);
                var sp1 = Barcode.Split(new[] { Encoding.ASCII.GetString(new byte[] { 0x1D }) }, StringSplitOptions.RemoveEmptyEntries);
                ModbusBarcode = string.Format("{0},{1},{2},{3}", sp1[1].TrimStart('Y'), sp1[2].TrimStart('P'), sp1[3].TrimStart('1', '2', 'V'), sp1[4].TrimStart('T')).TrimEnd((char)0x1E, (char)0x04);
            }
            catch (Exception)
            {
                Barcode = string.Empty;
                ModbusBarcode = string.Empty;
            }
        }

        private string Generate1Barcode(string generalPartNo, string generalVpps, string seeyaoDuns, string track1, string track2, string track3)
        {
            if (string.IsNullOrEmpty(generalPartNo) || generalPartNo.Length != 8)
                return string.Empty;

            if (string.IsNullOrEmpty(generalVpps))
                return string.Empty;

            if (string.IsNullOrEmpty(seeyaoDuns) || seeyaoDuns.Length != 9)
                return string.Empty;

            if (string.IsNullOrEmpty(track1) || track1.Length != 1)
                return string.Empty;

            if (string.IsNullOrEmpty(track2) || track2.Length != 2)
                return string.Empty;

            if (string.IsNullOrEmpty(track3) || track3.Length != 2)
                return string.Empty;

            //if (string.IsNullOrEmpty(TrackInfo) || !TrackInfo.StartsWith("OK "))
            //    return string.Empty;

            //var tracInfo = TrackInfo.Replace("OK ", "");

            var matrixBytes = new List<byte>();
            matrixBytes.AddRange(new byte[] { 0x5B, 0x29, 0x3E, 0x1E, 0x30, 0x36, 0x1D, 0x59 }); // header:[)><RS>06<GS>Y
            matrixBytes.AddRange(Encoding.ASCII.GetBytes(generalVpps)); // 通用VPPS号
            matrixBytes.Add(0x1D); //<GS>
            matrixBytes.AddRange(Encoding.ASCII.GetBytes("P" + generalPartNo)); //  P+通用零件号
            matrixBytes.Add(0x1D); //<GS>
            matrixBytes.AddRange(Encoding.ASCII.GetBytes("12V" + seeyaoDuns)); //  12V+DUNS邓氏码
            matrixBytes.Add(0x1D); //<GS>

            string outDate;
            string outSerialNo;
            if (!GetDateAndSerialNumber(false, generalPartNo, out outDate, out outSerialNo))
                return string.Empty;
            var tracInfo = string.Format("1A{0}{1}{2}{3}{4}{5}", DateTime.Parse(outDate).Year.ToString().Substring(2, 2),
                DateTime.Parse(outDate).DayOfYear.ToString().PadLeft(3, '0'), track1, track2, track3,
                outSerialNo.PadLeft(4, '0'));

            matrixBytes.AddRange(Encoding.ASCII.GetBytes("T" + tracInfo)); //  追溯码
            matrixBytes.AddRange(new byte[] { 0x1E, 0x04 }); // trailer <RS>

            var generatedBarcode = matrixBytes.ToArray().GetStringByAsciiBytes(false);

            return generatedBarcode;
        }

        #endregion
    }
}