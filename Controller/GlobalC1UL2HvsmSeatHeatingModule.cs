using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,GlobalC1UL2HVSM座椅加热模块")]
    public sealed class GlobalC1UL2HvsmSeatHeatingModule : ControllerBase
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

        [Description("R,读取故障码")]
        public string ErrorCode = string.Empty;

        [Description("R,软件APP版本号")]
        public string AppVer = string.Empty;
        [Description("R,软件BOOT版本号")]
        public string BootVer = string.Empty;

        [Description("R,CAL版本号")]
        public string CalVer = string.Empty;
        [Description("R,模块总成号")]
        public string PartNo = string.Empty;
        [Description("R,供应商零件号")]
        public string CustomSupplyNo = string.Empty;
        [Description("R,生产序列号")]
        public string ProduceSerialNo = string.Empty;

        public GlobalC1UL2HvsmSeatHeatingModule(string name) : base(name)
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

        ~GlobalC1UL2HvsmSeatHeatingModule()
        {
            Dispose();
        }

        private bool _isStopLin = true;
        private readonly Thread _mainWorkThread;
        private readonly object _lockSend = new object();
        private byte _nad = (byte)0x66;
        private byte[] _msg0x26 = new byte[2];
        private byte[] _msg0x27 = new byte[4];

        private const double PwmFactor = 0.392157d;

        private byte _leftBackrestHeatingPwm;
        private byte _rightBackrestHeatingPwm;
        private byte _leftSeatCushioHeatingPwm;
        private byte _rightSeatCushioHeatingPwm;

        private byte _leftVentilationPwm;
        private byte _rightVentilationPwm;

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
                    var msg = new byte[] { _leftBackrestHeatingPwm, _leftSeatCushioHeatingPwm, _rightBackrestHeatingPwm, _rightSeatCushioHeatingPwm, _leftVentilationPwm, _rightVentilationPwm, 0xC0 };

                    if (sendCount == 0) // 5ms
                    {
                        // send _msg0x26
                        // send 0x11/0x19
                        //_msg0x26[1] = _faultModeOpenTime;
                        //LinWithBaudRate10417.SendMasterLin(0x02, _msg0x26);
                        //var msg = new byte[] { _leftBackrestHeatingPwm, 0x00, _leftSeatCushioHeatingPwm, 0x00, _leftVentilationPwm, _rightVentilationPwm, 0xC0 };                       
                        LinWithBaudRate10417.SendMasterLin(0x19, msg);
                    }
                    else if (sendCount == 1) // 10ms
                    {
                        // send _msg0x27
                        // 0x25
                        //LinWithBaudRate10417.SendMasterLin(0x27, _msg0x27);
                        //LinWithBaudRate10417.SendMasterLin(0x25, new byte[0]);
                        LinWithBaudRate10417.SendMasterLin(0x19, msg);
                    }
                    else if (sendCount == 2) // 15ms
                    {
                        // send _msg0x26
                        // 0x35
                        //LinWithBaudRate10417.SendMasterLin(0x02, _msg0x26);
                        //LinWithBaudRate10417.SendMasterLin(0x35, new byte[0]);
                        LinWithBaudRate10417.SendMasterLin(0x19, msg);
                    }
                    else if (sendCount == 3) // 20ms
                    {
                        // send _msg0x27
                        // send 0x10/0x18
                        //LinWithBaudRate10417.SendMasterLin(0x27, _msg0x27);                        
                        LinWithBaudRate10417.SendMasterLin(0x19, msg);
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

        [Description("DriverCushionHsdPwm")]
        public void DriverCushionHsdPwm(double pwm)
        {
            if (pwm < 0 || pwm > 100)
                return;

            _leftSeatCushioHeatingPwm = (byte)Math.Round(pwm / PwmFactor, 0, MidpointRounding.AwayFromZero);
            _leftSeatCushioHeatingPwm = pwm == 0 ? (byte)0x00 : (byte)0xE6;
        }

        [Description("DriverBackHsdPwm")]
        public void DriverBackHsdPwm(double pwm)
        {
            if (pwm < 0 || pwm > 100)
                return;

            _leftBackrestHeatingPwm = (byte)Math.Round(pwm / PwmFactor, 0, MidpointRounding.AwayFromZero);
            _leftBackrestHeatingPwm = pwm == 0 ? (byte)0x00 : (byte)0xE6;
        }

        [Description("CoDriverCushionHsdPwm")]
        public void CoDriverCushionHsdPwm(double pwm)
        {
            if (pwm < 0 || pwm > 100)
                return;

            _rightSeatCushioHeatingPwm = (byte)Math.Round(pwm / PwmFactor, 0, MidpointRounding.AwayFromZero);
            _rightSeatCushioHeatingPwm = pwm == 0 ? (byte)0x00 : (byte)0xE6;
        }

        [Description("CoDriverBackHsdPwm")]
        public void CoDriverBackHsdPwm(double pwm)
        {
            if (pwm < 0 || pwm > 100)
                return;

            _rightBackrestHeatingPwm = (byte)Math.Round(pwm / PwmFactor, 0, MidpointRounding.AwayFromZero);
            _rightBackrestHeatingPwm = pwm == 0 ? (byte)0x00 : (byte)0xE6;
        }

        #endregion

        #region 诊断

        public void SetFrontRow() { }

        public void SetBakRow() { }

        [Description("读取故障码")]
        public void ReadError()
        {
            ErrorCode = string.Empty;
            if (LinWithBaudRate10417 != null)
            {
                var tp = _isStopLin;
                StopLin();
                Thread.Sleep(500);

                byte[] echo;
                if (LinWithBaudRate10417.SendSlaveLin(0x1A, out echo, timeOutMs: 200))
                {
                    if (echo.Length >= 6)
                    {
                        var newbs = new byte[6];
                        Array.Copy(echo, 0, newbs, 0, 6);
                        ErrorCode = ValueHelper.GetHextStr(newbs);
                    }
                }

                _isStopLin = tp;
            }
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

        [Description("读CAL版本号")]
        public void ReadCalVer()
        {
            CalVer = string.Empty;
            var result = ReadDid(27, 0xF1, 0xC2, _nad);
            CalVer = Encoding.ASCII.GetString(result, 0, result.Length);
        }

        [Description("读模块总成号")]
        public void ReadPartNo()
        {
            PartNo = string.Empty;

            if (LinWithBaudRate10417 is null)
                return;

            lock (_lockSend)
            {
                var b1 = new byte[] { _nad, 0x02, 0xB2, 0x24, 0xFF, 0xFF, 0xFF, 0xFF };
                var b2 = new byte[] { _nad, 0x02, 0xB2, 0x25, 0xFF, 0xFF, 0xFF, 0xFF };

                byte[] echo1;
                byte[] echo2;
                var result = new List<byte>();
                if (LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, b1, out echo1) && echo1 != null && echo1.Length == 8)
                {
                    if (ValueHelper.GetHextStr(echo1).Replace(" ", "").ToUpper().StartsWith(ValueHelper.GetHextStr(_nad).ToUpper() + "05F2"))
                    {
                        if (LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, b2, out echo2) && echo2 != null && echo2.Length == 8)
                        {
                            if (ValueHelper.GetHextStr(echo1).Replace(" ", "").ToUpper().StartsWith(ValueHelper.GetHextStr(_nad).ToUpper() + "05F2"))
                            {
                                result.Add(echo1[3]);
                                result.Add(echo1[4]);
                                result.Add(echo1[5]);
                                result.Add(echo1[6]);

                                result.Add(echo2[3]);
                                result.Add(echo2[4]);
                                result.Add(echo2[5]);
                                result.Add(echo2[6]);

                                PartNo = Encoding.ASCII.GetString(result.ToArray(), 0, result.Count);
                            }
                        }
                    }
                }
            }
        }

        [Description("读供应商零件号")]
        public void ReadCustomSupplyNo()
        {
            CustomSupplyNo = string.Empty;

            if (LinWithBaudRate10417 is null)
                return;

            lock (_lockSend)
            {
                var b1 = new byte[] { _nad, 0x02, 0xB2, 0x26, 0xFF, 0xFF, 0xFF, 0xFF };
                var b2 = new byte[] { _nad, 0x02, 0xB2, 0x27, 0xFF, 0xFF, 0xFF, 0xFF };

                byte[] echo1;
                byte[] echo2;
                var result = new List<byte>();
                if (LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, b1, out echo1) && echo1 != null && echo1.Length == 8)
                {
                    if (ValueHelper.GetHextStr(echo1).Replace(" ", "").ToUpper().StartsWith(ValueHelper.GetHextStr(_nad).ToUpper() + "05F2"))
                    {
                        if (LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, b2, out echo2) && echo2 != null && echo2.Length == 8)
                        {
                            if (ValueHelper.GetHextStr(echo1).Replace(" ", "").ToUpper().StartsWith(ValueHelper.GetHextStr(_nad).ToUpper() + "05F2"))
                            {
                                result.Add(echo1[3]);
                                result.Add(echo1[4]);
                                result.Add(echo1[5]);
                                result.Add(echo1[6]);

                                result.Add(echo2[3]);
                                result.Add(echo2[4]);
                                result.Add(echo2[5]);
                                result.Add(echo2[6]);

                                CustomSupplyNo = Encoding.ASCII.GetString(result.ToArray(), 0, result.Count);
                            }
                        }
                    }
                }
            }
        }

        [Description("写入并读取生产序列号")]
        public void WriteAndReadMtc()
        {
            ProduceSerialNo = string.Empty;
            if (LinWithBaudRate10417 is null)
                return;

            if (string.IsNullOrEmpty(Barcode))
            {
                ProduceSerialNo = "NG 获取数据失败，请检查网络连接";
                return;
            }

            lock (_lockSend)
            {
                //if (!SyProductionSaveCheckData.TryGetSySequenceDailyData(prNo, out string outDate, out int outSerialNo))
                //{
                //    ProduceSerialNo = "NG 获取数据失败，请检查网络连接";
                //    return;
                //}

                var mtc = string.Empty;
                var outDate = string.Empty;
                var outSerialNo = string.Empty;

                try
                {
                    var sp1 = Barcode.Split(new[] { Encoding.ASCII.GetString(new byte[] { 0x1D }) }, StringSplitOptions.RemoveEmptyEntries);
                    mtc = (sp1[4].TrimStart('T')).TrimEnd((char)0x1E, (char)0x04);
                    // 1A25298A2B4C0004

                    var mtcYear = "20" + mtc.Substring(2, 2);
                    var mtcDay = int.Parse(mtc.Substring(4, 3));
                    var mtcSerialNo = int.Parse(mtc.Substring(12, 4));

                    var mtcDateTime = DateTime.Parse(string.Format("{0}/01/01 00:00:00", mtcYear)).AddDays(mtcDay - 1);

                    outDate = mtcDateTime.ToString("yyyyMMdd");
                    outSerialNo = mtcSerialNo.ToString().PadLeft(4, '0');
                }
                catch (Exception)
                {
                    mtc = string.Empty;
                    outDate = string.Empty;
                    outSerialNo = string.Empty;
                }

                if (string.IsNullOrEmpty(outDate) || string.IsNullOrEmpty(outSerialNo))
                {
                    ProduceSerialNo = "NG 获取数据失败，请检查网络连接";
                    return;
                }

                var date = outDate.Substring(2, 6);
                var serialNo = outSerialNo;
                var toWriteStr = date + serialNo;
                var dateBs = Encoding.ASCII.GetBytes(toWriteStr);

                var bsLst = new List<byte[]>
                {
                    new byte[] { _nad, 0x10, 0x13, 0x2E, 0xFE, 0x04, dateBs[0], dateBs[1] },
                    new byte[] { _nad, 0x21, dateBs[2], dateBs[3], dateBs[4], dateBs[5], dateBs[6], dateBs[7] },
                    new byte[] { _nad, 0x22, dateBs[8], dateBs[9], 0xFF, 0xFF, 0xFF, 0xFF },
                    new byte[] { _nad, 0x23, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }
                };

                foreach (var item in bsLst)
                {
                    LinWithBaudRate10417.SendMasterLin(0x3C, item);
                    Thread.Sleep(10);
                    LinWithBaudRate10417.SendMasterLin(0x3D, new byte[0]);
                    Thread.Sleep(10);
                    //byte[] tempECHO;
                    //if (LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, item, out tempECHO) && tempECHO != null && tempECHO.Length == 8)
                    //{
                    //    if (ValueHelper.GetHextStr(tempECHO).Replace(" ", "").ToUpper().StartsWith(ValueHelper.GetHextStr(_nad).ToUpper() + "036E"))
                    //    {
                    //        continue;
                    //    }
                    //    else
                    //    {
                    //        break;
                    //    }
                    //}
                }

                var b1 = new byte[] { _nad, 0x02, 0xB2, 0x20, 0xFF, 0xFF, 0xFF, 0xFF };
                var b2 = new byte[] { _nad, 0x02, 0xB2, 0x21, 0xFF, 0xFF, 0xFF, 0xFF };
                var b3 = new byte[] { _nad, 0x02, 0xB2, 0x22, 0xFF, 0xFF, 0xFF, 0xFF };
                var b4 = new byte[] { _nad, 0x02, 0xB2, 0x23, 0xFF, 0xFF, 0xFF, 0xFF };

                byte[] echo1;
                byte[] echo2;
                byte[] echo3;
                byte[] echo4;
                var result = new List<byte>();

                if (LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, b1, out echo1) && echo1 != null && echo1.Length == 8)
                {
                    if (ValueHelper.GetHextStr(echo1).Replace(" ", "").ToUpper().StartsWith(ValueHelper.GetHextStr(_nad).ToUpper() + "05F2"))
                    {
                        if (LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, b2, out echo2) && echo2 != null && echo2.Length == 8)
                        {
                            if (ValueHelper.GetHextStr(echo2).Replace(" ", "").ToUpper().StartsWith(ValueHelper.GetHextStr(_nad).ToUpper() + "05F2"))
                            {
                                if (LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, b3, out echo3) && echo3 != null && echo3.Length == 8)
                                {
                                    if (ValueHelper.GetHextStr(echo3).Replace(" ", "").ToUpper().StartsWith(ValueHelper.GetHextStr(_nad).ToUpper() + "05F2"))
                                    {
                                        if (LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, b4, out echo4) && echo4 != null && echo4.Length == 8)
                                        {
                                            if (ValueHelper.GetHextStr(echo4).Replace(" ", "").ToUpper().StartsWith(ValueHelper.GetHextStr(_nad).ToUpper() + "05F2"))
                                            {
                                                result.Add(echo1[3]);
                                                result.Add(echo1[4]);
                                                result.Add(echo1[5]);
                                                result.Add(echo1[6]);

                                                result.Add(echo2[3]);
                                                result.Add(echo2[4]);
                                                result.Add(echo2[5]);
                                                result.Add(echo2[6]);

                                                result.Add(echo3[3]);
                                                result.Add(echo3[4]);

                                                var resdStr = ValueHelper.GetStringByAsciiBytes(result.ToArray(), false);
                                                ProduceSerialNo = resdStr == toWriteStr ? "OK " + resdStr : string.Format("NG 需写入:{0} 实际读取:{1}", toWriteStr, resdStr);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ProduceSerialNo = "NG 读取数据通信失败";
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

        public void ReadDiscreteInput()
        {
            DiscreteInput = string.Empty;
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

                    Thread.Sleep(25);
                    byte[] recv;
                    var isReadFirstFrameSucceed = LinWithBaudRate10417.SendSlaveLin(0x3D, out recv, timeOutMs: 200);
                    if (isReadFirstFrameSucceed && recv != null && recv.Length == 8)
                    {
                        var tpReadLen = 0;

                        if (recv[0] == data0)
                        {
                            if (recv[1] >= 0x10) // 多帧
                            {
                                if (recv[3] == 0x62 && recv[4] == didHi && recv[5] == didLo)
                                {
                                    resultBs.Add(recv[6]);
                                    resultBs.Add(recv[7]);
                                    var len = (recv[1] - 0x10) * 256 + recv[2];
                                    tpReadLen = len - 3;

                                    int count;
                                    if ((len - 5) % 6 == 0)
                                        count = (len - 5) / 6;
                                    else
                                        count = (len - 5) / 6 + 1;

                                    for (var i = 0; i < count; i++)
                                    {
                                        Thread.Sleep(25);
                                        byte[] recvBytesRest;
                                        var isSucceed = LinWithBaudRate10417.SendSlaveLin(0x3D, out recvBytesRest, timeOutMs: 200);
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
                                    tpReadLen = recv[1] - 3;
                                    for (var i = 5; i < 5 + dataLen; i++)
                                    {
                                        resultBs.Add(recv[i]);
                                    }
                                }
                            }
                        }

                        //if (resultBs.Any() && resultBs.Count >= readValueDataLen)
                        if (resultBs.Any() && resultBs.Count >= tpReadLen && tpReadLen > 0)
                        {
                            var temp3333 = new byte[tpReadLen];
                            Array.Copy(resultBs.ToArray(), 0, temp3333, 0, tpReadLen);
                            resultBs.Clear();
                            resultBs.AddRange(temp3333);
                        }
                        else
                        {
                            resultBs.Clear();
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

            if (!SyProductionSaveCheckData.TryGetSySequenceDailyData(generalPartNo, out string outDate, out int outSerialNo))
            {
                ProduceSerialNo = "NG 获取数据失败，请检查网络连接";
                return string.Empty;
            }

            //string outDate;
            //string outSerialNo;
            //if (!GetDateAndSerialNumber(false, generalPartNo, out outDate, out outSerialNo))
            //    return string.Empty;

            outDate = string.Format("{0}/{1}/{2} 00:00:00", outDate.Substring(0, 4), outDate.Substring(4, 2), outDate.Substring(6, 2));

            var tracInfo = string.Format("1A{0}{1}{2}{3}{4}{5}", DateTime.Parse(outDate).Year.ToString().Substring(2, 2),
                DateTime.Parse(outDate).DayOfYear.ToString().PadLeft(3, '0'), track1, track2, track3,
                outSerialNo.ToString().PadLeft(4, '0'));

            matrixBytes.AddRange(Encoding.ASCII.GetBytes("T" + tracInfo)); //  追溯码
            matrixBytes.AddRange(new byte[] { 0x1E, 0x04 }); // trailer <RS>

            var generatedBarcode = matrixBytes.ToArray().GetStringByAsciiBytes(false);

            return generatedBarcode;
        }

        #endregion

        #region LIN刷写

        [Description("R,App下载结果")]
        public string AppDownloadResult = string.Empty;
        [Description("R,APP下载耗时-秒")]
        public float AppDownloadCostTime = -9999;

        [Description("R,Cal1下载结果")]
        public string Cal1DownloadResult = string.Empty;
        [Description("R,Cal1下载耗时-秒")]
        public float Cal1DownloadCostTime = -9999;

        [Description("R,Cal2下载结果")]
        public string Cal2DownloadResult = string.Empty;
        [Description("R,Cal2下载耗时-秒")]
        public float Cal2DownloadCostTime = -9999;

        [Description("R/W,APP文件路径")]
        public string AppFilePath = @"E:\Projects\HVSM\刷写\HVSM产线刷写调试材料\刷写文件\Global_HVSM_APP_V0.21.0_20251128_Test.mot";
        [Description("R/W,CAL1文件路径")]
        public string Calibration1FilePath = @"E:\Projects\HVSM\刷写\HVSM产线刷写调试材料\刷写文件\Global_HVSM_E2LB-2_Front_AVENIR_CAL1_V0.9.0_20251128_Test.mot";
        [Description("R/W,CAL2文件路径")]
        public string Calibration2FilePath = @"E:\Projects\HVSM\刷写\HVSM产线刷写调试材料\刷写文件\Global_HVSM_E2LB-2_Rear_AVENIR_CAL2_V0.6.0_20251128_Test.mot";

        private byte _diagnosisMasterLinId = 0x3C;
        private byte _diagnosisSlaveLinId = 0x3D;
        private static readonly object FileLocker = new object();

        [Description("刷新APP文件")]
        public void AppDownload()
        {
            AppDownloadResult = string.Empty;
            AppDownloadCostTime = -9999;
            var st = new Stopwatch();
            st.Start();
            DataDownload(AppFilePath, ref AppDownloadResult, false);
            ReqDevRestart();
            st.Stop();
            AppDownloadCostTime = st.ElapsedMilliseconds / 1000f;
        }

        [Description("刷新Cal1文件")]
        public void Cai1Download()
        {
            Cal1DownloadResult = string.Empty;
            Cal1DownloadCostTime = -9999;
            var st = new Stopwatch();
            st.Start();
            DataDownload(Calibration1FilePath, ref Cal1DownloadResult, true);
            ReqDevRestart();
            st.Stop();
            Cal1DownloadCostTime = st.ElapsedMilliseconds / 1000f;
        }

        [Description("刷新Cal2文件")]
        public void Cai2Download()
        {
            Cal2DownloadResult = string.Empty;
            Cal2DownloadCostTime = -9999;
            var st = new Stopwatch();
            st.Start();
            DataDownload(Calibration2FilePath, ref Cal2DownloadResult, true);
            ReqDevRestart();
            st.Stop();
            Cal2DownloadCostTime = st.ElapsedMilliseconds / 1000f;
        }

        private void DataDownload(string filePath, ref string resultMsg, bool isCal)
        {
            if (!File.Exists(filePath))
            {
                resultMsg = "NG 待刷新文件不存在";
                return;
            }

            List<List<SRecordFileHelper.SRecordLineData>> dataBlocks;
            lock (FileLocker)
            {
                var sRecordApp = SRecordFileHelper.GetSRecordLineData(filePath);
                dataBlocks = SRecordFileHelper.GetBlocks(sRecordApp); // Block集合
            }

            if (LinWithBaudRate10417 == null)
            {
                resultMsg = "NG LIN未初始化";
                return;
            }

            try
            {
                var entendedModeReq = GetLinRecvMsg(new byte[] { _nad, 0x02, 0x10, 0x03, 0x00, 0x00, 0x00, 0x00 }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
                if (!entendedModeReq.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x50, 0x03 }).Replace(" ", "")))
                {
                    //resultMsg = "NG 10 03失败：" + entendedModeReq;
                    //return;
                }

                var seedReq1 = GetLinRecvMsg(new byte[] { _nad, 0x02, 0x27, 0x01, 0x00, 0x00, 0x00, 0x00 }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
                if (!seedReq1.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x06, 0x67, 0x01 }).Replace(" ", "")) || seedReq1.Length != 8 * 2)
                {
                    //resultMsg = "NG 27 01失败：" + seedReq1;
                    //return;
                }

                var seed11 = Convert.ToByte(seedReq1.Substring(8, 2), 16);
                var seed12 = Convert.ToByte(seedReq1.Substring(10, 2), 16);
                var seed13 = Convert.ToByte(seedReq1.Substring(12, 2), 16);
                var seed14 = Convert.ToByte(seedReq1.Substring(14, 2), 16);

                var keys1 = GenerateKeys(GenerateSeed(new[] { seed11, seed12, seed13, seed14 }));
                var keyReq1 = GetLinRecvMsg(new byte[] { _nad, 0x06, 0x27, 0x02, keys1[0], keys1[1], keys1[2], keys1[3] },
                    _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
                if (!keyReq1.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x67, 0x02 }).Replace(" ", "")))
                {
                    //resultMsg = "NG 27 02失败：" + keyReq1;
                    //return;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            var programModeReq = GetLinRecvMsg(new byte[] { _nad, 0x02, 0x10, 0x02, 0x00, 0x00, 0x00, 0x00 }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!programModeReq.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x50, 0x02 }).Replace(" ", "")))
            {
                resultMsg = "NG 10 02失败：" + programModeReq;
                return;
            }

            Thread.Sleep(1050);

            var seedReq2 = GetLinRecvMsg(new byte[] { _nad, 0x02, 0x27, 0x01, 0x00, 0x00, 0x00, 0x00 }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!seedReq2.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x06, 0x67, 0x01 }).Replace(" ", "")) || seedReq2.Length != 8 * 2)
            {
                resultMsg = "NG 27 01失败：" + seedReq2;
                return;
            }

            var seed21 = Convert.ToByte(seedReq2.Substring(8, 2), 16);
            var seed22 = Convert.ToByte(seedReq2.Substring(10, 2), 16);
            var seed23 = Convert.ToByte(seedReq2.Substring(12, 2), 16);
            var seed24 = Convert.ToByte(seedReq2.Substring(14, 2), 16);

            var keys2 = GenerateKeys(GenerateSeed(new[] { seed21, seed22, seed23, seed24 }));

            var keyReq2 = GetLinRecvMsg(
                new byte[] { _nad, 0x06, 0x27, 0x02, keys2[0], keys2[1], keys2[2], keys2[3] },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);

            if (!keyReq2.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x67, 0x02 }).Replace(" ", "")))
            {
                resultMsg = "NG 27 02失败：" + keyReq2;
                return;
            }

            var dataRecord = new List<byte>();
            var dataStartAddr = dataBlocks[0][0].Address; /*!isCal ? (uint)0x08020000 : 0x08050000;*/
            var dataLen = 0;
            foreach (var t in dataBlocks[0])
            {
                dataRecord.AddRange(t.Data);
                dataLen += t.DataLen;
            }

            LinWithBaudRate10417.SendMasterLin(_diagnosisMasterLinId, new byte[] { _nad, 0x10, 0x0d, 0x31, 0x01, 0xFF, 0x00, 0x44 });
            var reqFlsStartAddr = BitConverter.GetBytes(dataStartAddr).Reverse().ToArray();
            if (isCal)
            {
                reqFlsStartAddr[0] = 0x10;
            }
            var reqFlsLen = BitConverter.GetBytes(dataLen).Reverse().ToArray();

            var addrBytes = new byte[4];
            if (isCal)
            {
                Array.Copy(reqFlsStartAddr, addrBytes, 4);
            }
            else
            {
                addrBytes = new byte[] { 0x00, 0x00, 0x40, 0x00 };
            }

            Thread.Sleep(25);
            LinWithBaudRate10417.SendMasterLin(_diagnosisMasterLinId, new byte[] { _nad, 0x21, addrBytes[0], addrBytes[1], addrBytes[2], addrBytes[3], reqFlsLen[0], reqFlsLen[1] });
            Thread.Sleep(25);
            LinWithBaudRate10417.SendMasterLin(_diagnosisMasterLinId, new byte[] { _nad, 0x22, reqFlsLen[2], reqFlsLen[3], 0x00, 0x00, 0x00, 0x00 });

            var eraseReq1 = string.Empty;
            var errorCount = 0;
            var eraseTs1 = HighPrecisionTimer.GetTimestamp();

            while (true)
            {
                byte[] echoBytes;
                if (!LinWithBaudRate10417.SendSlaveLin(_diagnosisSlaveLinId, out echoBytes, 50))
                {
                    var eraseTs2 = HighPrecisionTimer.GetTimestamp();
                    var eraseTs = HighPrecisionTimer.GetTimestampIntervalMs(eraseTs1, eraseTs2);

                    errorCount++;
                    if (errorCount > 10 && eraseTs > 3500)
                        break;
                }
                else if (ValueHelper.GetHextStr(echoBytes).Replace(" ", "") == ValueHelper.GetHextStr(new byte[8]).Replace(" ", ""))
                {
                    var eraseTs2 = HighPrecisionTimer.GetTimestamp();
                    var eraseTs = HighPrecisionTimer.GetTimestampIntervalMs(eraseTs1, eraseTs2);

                    errorCount++;
                    if (errorCount > 10 && eraseTs > 3500)
                        break;
                }

                if (ValueHelper.GetHextStr(echoBytes)
                        .Replace(" ", "")
                        .StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x03, 0x7F, 0x31, 0x78 }).Replace(" ", "")))
                {
                    Thread.Sleep(50);
                    continue;
                }

                if (!string.IsNullOrEmpty(ValueHelper.GetHextStr(echoBytes)) &&
                    ValueHelper.GetHextStr(echoBytes).Replace(" ", "") != ValueHelper.GetHextStr(new byte[8]).Replace(" ", ""))
                {
                    eraseReq1 = ValueHelper.GetHextStr(echoBytes).Replace(" ", "");
                    break;
                }
            }

            if (!eraseReq1.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x71, 0x01, 0xFF, 0x00 }).Replace(" ", "")))
            {
                resultMsg = "NG 请求擦除失败：" + eraseReq1;
                return;
            }

            var eraseReq2 = GetLinRecvMsg(new byte[] { _nad, 0x04, 0x31, 0x03, 0xFF, 0x00, 0x00, 0x00 }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!eraseReq2.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x71, 0x03 }).Replace(" ", "")))
            {
                resultMsg = "NG 读取擦除状态失败：" + eraseReq2;
                return;
            }

            LinWithBaudRate10417.SendMasterLin(_diagnosisMasterLinId, new byte[] { _nad, 0x10, 0x0b, 0x34, 0x00, 0x44, reqFlsStartAddr[0], reqFlsStartAddr[1] });
            Thread.Sleep(25);
            var data34Req = GetLinRecvMsg(new byte[] { _nad, 0x21, reqFlsStartAddr[2], reqFlsStartAddr[3], reqFlsLen[0], reqFlsLen[1], reqFlsLen[2], reqFlsLen[3] }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!data34Req.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x74 }).Replace(" ", "")))
            {
                resultMsg = "NG 34请求下载失败：" + data34Req;
                return;
            }

            if (!DataTransfer(dataRecord, ref resultMsg))
                return;

            var exitDataTransfer2 = GetLinRecvMsg(new byte[] { _nad, 0x01, 0x37, 0x00, 0x00, 0x00, 0x00, 0x00 }, _diagnosisMasterLinId, _diagnosisSlaveLinId, 200, false);
            if (!exitDataTransfer2.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x01, 0x77 }).Replace(" ", "")))
            {
                resultMsg = "NG 37结束下载失败：" + exitDataTransfer2;
                return;
            }

            LinWithBaudRate10417.SendMasterLin(_diagnosisMasterLinId, new byte[] { _nad, 0x10, 0x0d, 0x31, 0x01, 0xFF, 0x01, 0x44 });
            Thread.Sleep(25);
            LinWithBaudRate10417.SendMasterLin(_diagnosisMasterLinId, new byte[] { _nad, 0x21, reqFlsStartAddr[0], reqFlsStartAddr[1], reqFlsStartAddr[2], reqFlsStartAddr[3], reqFlsLen[0], reqFlsLen[1] });
            Thread.Sleep(25);
            var routineControl3101Ff01Req = GetLinRecvMsg(new byte[] { _nad, 0x22, reqFlsLen[2], reqFlsLen[3], 0x00, 0x00, 0x00, 0x00 }, _diagnosisMasterLinId, _diagnosisSlaveLinId, 200, false);
            if (!routineControl3101Ff01Req.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x71, 0x01, 0xFF, 0x01 }).Replace(" ", "")))
            {
                resultMsg = "NG 3101FF01失败：" + routineControl3101Ff01Req;
                return;
            }

            var routineControl3103Ff01Req = GetLinRecvMsg(new byte[] { _nad, 0x04, 0x31, 0x03, 0xFF, 0x01, 0x00, 0x00 },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!routineControl3103Ff01Req.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x71, 0x03, 0xFF, 0x01 }).Replace(" ", "")))
            {
                resultMsg = "NG 3103FF01失败：" + routineControl3103Ff01Req;
                return;
            }

            resultMsg = "OK";
        }

        private void ReqDevRestart()
        {
            LinWithBaudRate10417.SendMasterLin(_diagnosisMasterLinId, new byte[] { _nad, 0x02, 0x10, 0x81, 0x00, 0x00, 0x00, 0x00 });
        }

        private bool DataTransfer(IReadOnlyList<byte> dataBytes, ref string msg)
        {
            var dataIndex = 0;
            var countOf36 = dataBytes.Count / 128;
            var groupCount = (byte)1;
            {
                for (var i = 0; i < countOf36; i++)
                {
                    var rollingCount = 0x21;
                    var firstFrame = new byte[] { _nad, 0x10, 0x82, 0x36, groupCount, dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++] };
                    LinWithBaudRate10417.SendMasterLin(_diagnosisMasterLinId, firstFrame);
                    //Console.WriteLine("data transfer: " + ValueHelper.GetHextStr(firstFrame));

                    for (var j = 0; j < 125 / 6; j++)
                    {
                        var continueFrame = new[] { _nad, (byte)rollingCount, dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++] };
                        LinWithBaudRate10417.SendMasterLin(_diagnosisMasterLinId, continueFrame);
                        //Console.WriteLine("data transfer: " + ValueHelper.GetHextStr(continueFrame));

                        rollingCount++;
                        if (rollingCount > 0x2F)
                            rollingCount = 0x20;
                    }

                    // Console.WriteLine("data transfer: " + ValueHelper.GetHextStr(new byte[] { _nad, (byte)rollingCount, dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++], (byte)0xFF }));
                    var lastFrameRequest = GetLinRecvMsg(new[]
                    {
                        _nad, (byte) rollingCount,
                        dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++],
                        dataBytes[dataIndex++], dataBytes[dataIndex++], (byte)0xFF
                    }, _diagnosisMasterLinId, _diagnosisSlaveLinId, 250);
                    if (!lastFrameRequest.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x76, groupCount }).Replace(" ", "")))
                    {
                        msg = "NG 36传输失败：" + lastFrameRequest;
                        return false;
                    }

                    groupCount = (byte)(groupCount + 0x01);
                }
            }

            if (dataBytes.Count % 128 != 0)
            {
                var restCount = dataBytes.Count % 128;

                var rollingCount = 0x21;
                var firstFrame = new byte[] { _nad, 0x10, (byte)(restCount + 2), 0x36, groupCount, dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++] };
                LinWithBaudRate10417.SendMasterLin(_diagnosisMasterLinId, firstFrame);
                //Console.WriteLine("data transfer: " + ValueHelper.GetHextStr(firstFrame));

                var rest36CountinueCount = restCount / 6;
                var isCountinueRest = (restCount - 3) % 6 != 0;
                if (isCountinueRest)
                    rest36CountinueCount++;
                for (var i = 0; i < rest36CountinueCount; i++)
                {
                    if (i == rest36CountinueCount - 1)
                    {
                        var lastBytes = new byte[8];
                        lastBytes[0] = _nad;
                        lastBytes[1] = (byte)rollingCount;
                        if (!isCountinueRest)
                        {
                            lastBytes[2] = dataBytes[dataIndex++];
                            lastBytes[3] = dataBytes[dataIndex++];
                            lastBytes[4] = dataBytes[dataIndex++];
                            lastBytes[5] = dataBytes[dataIndex++];
                            lastBytes[6] = dataBytes[dataIndex++];
                            lastBytes[7] = dataBytes[dataIndex++];
                        }
                        else
                        {
                            var usedCount = dataBytes.Count - dataIndex;
                            var bUsedIndex = 2;
                            for (var kk = 0; kk < usedCount; kk++)
                                lastBytes[bUsedIndex++] = dataBytes[dataIndex++];

                            //Console.WriteLine("data transfer: " + ValueHelper.GetHextStr(lastBytes));
                            var lastFrameRequest = GetLinRecvMsg(lastBytes, _diagnosisMasterLinId, _diagnosisSlaveLinId, 250);
                            if (!lastFrameRequest.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x76, groupCount }).Replace(" ", "")))
                            {
                                msg = "NG 36传输失败：" + lastFrameRequest;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        var continueFrame = new[] { _nad, (byte)rollingCount, dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++] };
                        LinWithBaudRate10417.SendMasterLin(_diagnosisMasterLinId, continueFrame);
                        //Console.WriteLine("data transfer: " + ValueHelper.GetHextStr(continueFrame));

                        rollingCount++;
                        if (rollingCount > 0x2F)
                            rollingCount = 0x20;
                    }
                }
            }

            return true;
        }

        private static uint GenerateSeed(IReadOnlyList<byte> seedBytes)
        {
            uint seedNumber = seedBytes[0];
            seedNumber = (seedNumber << 8) + seedBytes[1];
            seedNumber = (seedNumber << 8) + seedBytes[2];
            seedNumber = (seedNumber << 8) + seedBytes[3];

            return seedNumber;
        }

        private static byte[] GenerateKeys(uint seed)
        {
            var wKey = 0xFFFFFFFF - seed;

            var key4 = wKey >> 24 & 0xff;
            var key5 = wKey >> 16 & 0xff;
            var key6 = wKey >> 8 & 0xff;
            var key7 = wKey & 0xff;

            return new[] { (byte)key4, (byte)key5, (byte)key6, (byte)key7 };
        }

        private string GetLinRecvMsg(
            byte[] sendBytes, byte masterLinId, byte slaveLinId, int delayMs = 50, bool isNeedRetry = true)
        {
            byte[] resultBytes;
            if (LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(
                masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
            {
                if (resultBytes != null && resultBytes.Length == 8)
                    return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));

                if (isNeedRetry)
                {
                    Thread.Sleep(500);
                    if (!LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(
                        masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
                        return string.Empty;

                    if (resultBytes != null && resultBytes.Length == 8)
                        return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));
                }
            }
            else if (isNeedRetry)
            {
                Thread.Sleep(500);
                if (!LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(
                    masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
                    return string.Empty;

                if (resultBytes != null && resultBytes.Length == 8)
                    return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));
            }

            return string.Empty;
        }

        #endregion
    }
}