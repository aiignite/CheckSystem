using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,红旗E001尾灯")]
    public sealed class E001RearLamp : ControllerBase
    {
        public CanBus Can;

        public E001RearLamp(string name) :
            base(name)
        {
            Th = new Thread(MainWork) { IsBackground = true };
            Th.Start();
        }

        private bool _isSleep = true;
        private readonly object _lockCanSend = new object();
        private Thread Th { get; set; }

        ~E001RearLamp()
        {
            Dispose();
        }

        #region 信号列表

        private readonly CanCommunicationMatrix.IntelMatrix _bcm11 = new CanCommunicationMatrix.IntelMatrix(0x230, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _bcm12 = new CanCommunicationMatrix.IntelMatrix(0x232, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _bcm19 = new CanCommunicationMatrix.IntelMatrix(0x237, 8);

        private void MainWork()
        {
            var count = 0;

            var brakeOffDatasStr = new List<string>
            {
                "9000000000000000",
                "5D00000000000010",
                "1700000000000020",
                "DA00000000000030",
                "8300000000000040",
                "4E00000000000050",
                "0400000000000060",
                "C900000000000070",
                "B600000000000080",
                "7B00000000000090",
                "31000000000000A0",
                "FC000000000000B0",
                "A5000000000000C0",
                "68000000000000D0",
                "A5000000000000E0"
            };

            var brakeOnDatasStr = new List<string>
            {
                "F904000000000000",
                "3404000000000010",
                "7E04000000000020",
                "B304000000000030",
                "EA04000000000040",
                "2704000000000050",
                "6D04000000000060",
                "A004000000000070",
                "DF04000000000080",
                "1204000000000090",
                "58040000000000A0",
                "95040000000000B0",
                "CC040000000000C0",
                "01040000000000D0",
                "4B040000000000E0"
            };

            var brakeOffDatas = new List<byte[]>();
            var brakeOnDatas = new List<byte[]>();
            var dataIndexOfBrake = 0;

            foreach (var off in brakeOffDatasStr)
            {
                var temp = new List<byte>();
                for (var i = 0; i < off.Length; i = i + 2)
                {
                    var b = Convert.ToByte(off.Substring(i, 2), 16);
                    temp.Add(b);
                }
                brakeOffDatas.Add(temp.ToArray());
            }

            foreach (var off in brakeOnDatasStr)
            {
                var temp = new List<byte>();
                for (var i = 0; i < off.Length; i = i + 2)
                {
                    var b = Convert.ToByte(off.Substring(i, 2), 16);
                    temp.Add(b);
                }
                brakeOnDatas.Add(temp.ToArray());
            }

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
                        dataIndexOfBrake = 0;
                        continue;
                    }

                    lock (_lockCanSend)
                    {
                        var lstPages = new List<CanBus.CanDataPackage>();

                        _bcm12.UpdateData(new MatrixValDefinition(9, 1, _reverseLightCmd ? (byte)0x01 : (byte)0x00));
                        lstPages.Add(new CanBus.CanDataPackage(_bcm12.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data, _bcm12.MatrixData));

                        _bcm11.UpdateData(new MatrixValDefinition(18, 1, _turningLightCmdLeft ? (byte)0x01 : (byte)0x00));
                        _bcm11.UpdateData(new MatrixValDefinition(44, 1, _flowTurningLightCmdLeft ? (byte)0x01 : (byte)0x00));

                        _bcm11.UpdateData(new MatrixValDefinition(19, 1, _turningLightCmdRight ? (byte)0x01 : (byte)0x00));
                        _bcm11.UpdateData(new MatrixValDefinition(45, 1, _flowTurningLightCmdRight ? (byte)0x01 : (byte)0x00));

                        _bcm11.UpdateData(new MatrixValDefinition(58, 1, _fogCmd ? (byte)0x01 : (byte)0x00));

                        lstPages.Add(new CanBus.CanDataPackage(_bcm11.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data, _bcm11.MatrixData));

                        lstPages.Add(new CanBus.CanDataPackage(0x1F1, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            _brakeLightCmd ? brakeOnDatas[dataIndexOfBrake] : brakeOffDatas[dataIndexOfBrake]));
                        dataIndexOfBrake++;
                        if (dataIndexOfBrake == 15)
                            dataIndexOfBrake = 0;

                        _bcm19.UpdateData(new MatrixValDefinition(29, 1, _positionLightCmdLeft ? (byte)0x01 : (byte)0x00));
                        _bcm19.UpdateData(new MatrixValDefinition(30, 1, _positionLightCmdRight ? (byte)0x01 : (byte)0x00));

                        _bcm19.UpdateData(new MatrixValDefinition(57, 1, _turningLightCmdLeft ? (byte)0x01 : (byte)0x00));
                        _bcm19.UpdateData(new MatrixValDefinition(58, 1, _turningLightCmdRight ? (byte)0x01 : (byte)0x00));

                        lstPages.Add(new CanBus.CanDataPackage(_bcm19.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data, _bcm19.MatrixData));

                        lstPages.Add(new CanBus.CanDataPackage(
                            0x5A0, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x14, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00 }));

                        lstPages.Add(new CanBus.CanDataPackage(
                            0x2B0, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00 }));

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

        [Description("唤醒")]
        public void ModuleAwake()
        {
            _isSleep = false;
        }

        [Description("休眠")]
        public void ModuleSleep()
        {
            _isSleep = true;
        }

        private bool _fogCmd; // fog
        private bool _brakeLightCmd; // stop
        private bool _reverseLightCmd; // bul

        [Description("制动打开")]
        public void StopOn()
        {
            _brakeLightCmd = true;
        }

        [Description("制动关闭")]
        public void StopOff()
        {
            _brakeLightCmd = false;
        }

        [Description("倒车灯开")]
        public void BullOn()
        {
            _reverseLightCmd = true;
        }

        [Description("倒车灯关")]
        public void BullOff()
        {
            _reverseLightCmd = false;
        }

        [Description("雾灯打开")]
        public void FogOn()
        {
            _fogCmd = true;
        }

        [Description("雾灯关闭")]
        public void FogOff()
        {
            _fogCmd = false;
        }

        private bool _turningLightCmdLeft; // turn
        private bool _flowTurningLightCmdLeft; // turn
        private bool _positionLightCmdLeft; // tail

        [Description("左尾灯打开")]
        public void LeftTailOn()
        {
            _positionLightCmdLeft = true;
        }

        [Description("左尾灯关闭")]
        public void LeftTailOff()
        {
            _positionLightCmdLeft = false;
        }

        [Description("左转向打开")]
        public void LeftTurnOn()
        {
            _flowTurningLightCmdLeft = true;
            _turningLightCmdLeft = true;
        }

        [Description("左转向关闭")]
        public void LeftTurnOff()
        {
            _flowTurningLightCmdLeft = false;
            _turningLightCmdLeft = false;
        }

        private bool _turningLightCmdRight; // turn
        private bool _flowTurningLightCmdRight; // turn
        private bool _positionLightCmdRight; // tail

        [Description("右尾灯打开")]
        public void RightTailOn()
        {
            _positionLightCmdRight = true;
        }

        [Description("右尾灯关闭")]
        public void RightTailOff()
        {
            _positionLightCmdRight = false;
        }

        [Description("右转向打开")]
        public void RightTurnOn()
        {
            _flowTurningLightCmdRight = true;
            _turningLightCmdRight = true;
        }

        [Description("右转向关闭")]
        public void RightTurnOff()
        {
            _flowTurningLightCmdRight = false;
            _turningLightCmdRight = false;
        }

        #endregion

        #region DTC相关

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
                        0x706, 0x70E, CanBus.CanType.Standard, 0x02, 0xFF,
                        out echo))
                {
                    if (echo != null)
                    {
                        // DTC CODE: B1DEF, (10)01110111101111
                        // CODE: 9DEF, 1001110111101111
                        // Failure Type: 0x13, dtc low byte

                        //ReadDtcResult = ValueHelper.GetHextStr(echo);

                        if (echo.Length % 4 == 0)
                        {
                            for (var i = 0; i < echo.Length; i = i + 4)
                            {
                                var dtcData = new Uds14229Helper.DtcData(echo[i], echo[i + 1], echo[i + 2], echo[i + 3]);
                                //Console.WriteLine(dtcData.Remark);

                                ReadDtcLResult += string.Format("[{0}];", dtcData.Remark);
                            }

                            if (string.IsNullOrEmpty(ReadDtcLResult))
                            {
                                ReadDtcLResult = "NoError";
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
                        0x706, 0x70E, CanBus.CanType.Standard))
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
                        0x730, 0x738, CanBus.CanType.Standard, 0x02, 0xFF, out echo))
                {
                    if (echo != null)
                    {
                        // DTC CODE: B1DEF, (10)01110111101111
                        // CODE: 9DEF, 1001110111101111
                        // Failure Type: 0x13, dtc low byte

                        //ReadDtcResult = ValueHelper.GetHextStr(echo);

                        if (echo.Length % 4 == 0)
                        {
                            for (var i = 0; i < echo.Length; i = i + 4)
                            {
                                var dtcData = new Uds14229Helper.DtcData(echo[i], echo[i + 1], echo[i + 2], echo[i + 3]);
                                //Console.WriteLine(dtcData.Remark);

                                ReadDtcRResult += string.Format("[{0}];", dtcData.Remark);
                            }

                            if (string.IsNullOrEmpty(ReadDtcRResult))
                            {
                                ReadDtcRResult = "NoError";
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
                        0x730, 0x738, CanBus.CanType.Standard))
                {
                    ClearFaultRResult = @"OK";
                }
            }
        }

        #endregion
    }
}
