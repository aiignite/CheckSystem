using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,小鹏E29尾灯")]
    internal class XiaoPengE29RearLamp : ControllerBase
    {
        public CanBus Can;

        public XiaoPengE29RearLamp(string name) : base(name)
        {
            for (var i = 0; i < 96; i++)
                _ledDictionary.Add(i, 0x00);
            MainWork();
        }

        ~XiaoPengE29RearLamp()
        {
            Dispose();
        }

        private readonly object _lock = new object();
        private bool _isSleep = true;
        private readonly CanCommunicationMatrix.MotorolaMatrix _bcm0X269 = new CanCommunicationMatrix.MotorolaMatrix(0x269, 8);
        private readonly CanCommunicationMatrix.MotorolaMatrix _bcm0X22B = new CanCommunicationMatrix.MotorolaMatrix(0x22B, 8);
        private readonly CanCommunicationMatrix.MotorolaMatrix _bcm0X1E4 = new CanCommunicationMatrix.MotorolaMatrix(0x1E4, 64);

        private bool _isTailOn;
        private bool _isTailHdOn;
        private bool _isStopOn;
        private byte _isTurnLeftOn;
        private byte _isTurnRightOn;
        private bool _isLampSignalMode;
        private readonly Dictionary<int, byte> _ledDictionary = new Dictionary<int, byte>();
        private bool _isRunningMode;
        private byte _runningModeValue;
        private int _runningModeCount;

        private void MainWork()
        {
            var sendCount = 0;

            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    if (Can == null)
                    {
                        sendCount = 0;
                        return;
                    }

                    if (_isSleep)
                    {
                        sendCount = 0;
                        return;
                    }

                    _bcm0X269.UpdateData(new MatrixValDefinition(6, 2, _isTurnLeftOn));
                    _bcm0X269.UpdateData(new MatrixValDefinition(4, 2, _isTurnRightOn));
                    _bcm0X269.UpdateData(new MatrixValDefinition(39, 1, _isTailOn ? (byte)0x01 : (byte)0x00));
                    _bcm0X269.UpdateData(new MatrixValDefinition(44, 1, _isTailHdOn ? (byte)0x01 : (byte)0x00));
                    lock (_lock)
                    {
                        Can.SendStandardCanFdData(0X1b5, new byte[] { 0x00, _isLampSignalMode ? (byte)0x02 : (byte)0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                        if (_isLampSignalMode)
                        {
                            if (_isRunningMode)
                            {
                                if (sendCount % 3 == 0)
                                {
                                    if (_runningModeCount == 96)
                                    {
                                        _runningModeCount = 0;
                                        for (var i = 95; i >= 0; i--)
                                            _ledDictionary[i] = 0x00;
                                    }
                                    else
                                    {
                                        _ledDictionary[_runningModeCount] = _runningModeValue;
                                        _runningModeCount++;
                                    }

                                    for (var i = 0; i < 96; i++)
                                    {
                                        if (i % 2 == 0)
                                        {
                                            var md = new MatrixValDefinition(i * 4 + 4, 4, _ledDictionary[i]);
                                            _bcm0X1E4.UpdateData(md);
                                        }
                                        else
                                        {
                                            var md = new MatrixValDefinition((i - 1) * 4 + 0, 4, _ledDictionary[i]);
                                            _bcm0X1E4.UpdateData(md);
                                        }
                                    }
                                }

                                Can.SendStandardCanFdData(_bcm0X1E4.CanId, _bcm0X1E4.MatrixData);
                            }
                            else
                            {
                                for (var i = 0; i < 96; i++)
                                {
                                    if (i % 2 == 0)
                                    {
                                        var md = new MatrixValDefinition(i * 4 + 4, 4, _ledDictionary[i]);
                                        _bcm0X1E4.UpdateData(md);
                                    }
                                    else
                                    {
                                        var md = new MatrixValDefinition((i - 1) * 4 + 0, 4, _ledDictionary[i]);
                                        _bcm0X1E4.UpdateData(md);
                                    }
                                }

                                Can.SendStandardCanFdData(_bcm0X1E4.CanId, _bcm0X1E4.MatrixData);
                            }
                        }
                        else
                        {
                            Can.SendStandardCanFdData(_bcm0X269.CanId, _bcm0X269.MatrixData);
                        }

                        sendCount++;
                        if (sendCount > 12)
                            sendCount = 0;
                    }
                },
                Interval = 40
            });

            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    if (Can == null)
                        return;

                    if (_isSleep)
                        return;

                    _bcm0X22B.UpdateData(new MatrixValDefinition(26, 1, _isStopOn ? (byte)0x01 : (byte)0x00));
                    lock (_lock)
                        Can.SendStandardCanFdData(_bcm0X22B.CanId, _bcm0X22B.MatrixData);
                },
                Interval = 50
            });

            SchedulerAsync();
        }

        [Description("打开CAN消息")]
        public void StartCan()
        {
            _isSleep = false;
        }

        [Description("关闭CAN消息")]
        public void StopCan()
        {
            _isSleep = true;
        }

        [Description("TAIL ON")]
        public void TailOn()
        {
            _isTailOn = true;
            _isTailHdOn = false;
        }

        [Description("TAIL HD ON")]
        public void TailHdOn()
        {
            _isTailOn = true;
            _isTailHdOn = true;
        }

        [Description("TAIL OFF")]
        public void TailOff()
        {
            _isTailOn = false;
            _isTailHdOn = false;
        }

        [Description("TURN LEFT ON")]
        public void TurnLeftOn()
        {
            _isTurnLeftOn = 0x01;
        }

        [Description("TURN LEFT LINE ON")]
        public void TurnLeftLineOn()
        {
            _isTurnLeftOn = 0x02;
        }

        [Description("TURN LEFT OFF")]
        public void TurnLeftOff()
        {
            _isTurnLeftOn = 0x00;
        }

        [Description("TURN RIGHT ON")]
        public void TurnRightOn()
        {
            _isTurnRightOn = 0x01;
        }

        [Description("TURN RIGHT LINE ON")]
        public void TurnRightLineOn()
        {
            _isTurnRightOn = 0x02;
        }

        [Description("TURN RIGHT OFF")]
        public void TurnRightOff()
        {
            _isTurnRightOn = 0x00;
        }

        [Description("STOP ON")]
        public void StopOn()
        {
            _isStopOn = true;
        }

        [Description("STOP OFF")]
        public void StopOff()
        {
            _isStopOn = false;
        }

        [Description("打开灯语模式")]
        public void OpenLampSignalMode()
        {
            _isRunningMode = false;
            _runningModeValue = 0x00;
            _runningModeCount = 0;
            for (var i = 0; i < 96; i++)
                _ledDictionary[i] = 0x00;
            _isLampSignalMode = true;
        }

        [Description("关闭灯语模式")]
        public void CloseLampSignalMode()
        {
            _isRunningMode = false;
            _runningModeValue = 0x00;
            _runningModeCount = 0;
            for (var i = 0; i < 96; i++)
                _ledDictionary[i] = 0x00;
            _isLampSignalMode = false;
        }

        [Description("打开单颗LED")]
        public void OpenSingleLed(int index, byte value)
        {
            if (_isRunningMode)
                return;
            SetLedValue(index, value);
        }

        [Description("关闭单颗LED")]
        public void CloseSingleLed(int index)
        {
            if (_isRunningMode)
                return;
            SetLedValue(index, 0x00);
        }

        [Description("打开所有LED")]
        public void OpenAllLed(byte value)
        {
            if (_isRunningMode)
                return;
            for (var i = 0; i < 96; i++)
                SetLedValue(i, value);
        }

        [Description("打开奇数LED")]
        public void OpenOddLed(byte value)
        {
            if (_isRunningMode)
                return;
            for (var i = 0; i < 96; i++)
            {
                if (i % 2 != 0)
                {
                    SetLedValue(i, value);
                }
            }
        }

        [Description("打开偶数LED")]
        public void OpenEvenLed(byte value)
        {
            if (_isRunningMode)
                return;
            for (var i = 0; i < 96; i++)
            {
                if (i % 2 == 0)
                {
                    SetLedValue(i, value);
                }
            }
        }

        [Description("关闭所有LED")]
        public void CloseAllLed()
        {
            if (_isRunningMode)
                return;
            for (var i = 0; i < 96; i++)
                SetLedValue(i, 0x00);
        }

        [Description("打开跑马效果")]
        public void StartRunning(byte value)
        {
            if (_isRunningMode)
                return;

            if (value >= 0x01 && value <= 0x0A)
            {
                _runningModeCount = 0;

                for (var i = 0; i < 96; i++)
                    _ledDictionary[i] = 0x00;

                _isRunningMode = true;
                _runningModeValue = value;
            }
        }

        [Description("关闭跑马效果")]
        public void StopRunning()
        {
            _isRunningMode = false;
            _runningModeValue = 0x00;
            _runningModeCount = 0;

            for (var i = 0; i < 96; i++)
                _ledDictionary[i] = 0x00;
        }

        private void SetLedValue(int index, byte value)
        {
            if (index >= 0 && index <= 95 && value <= 0x0A)
            {
                _ledDictionary[index] = value;
            }
        }

        #region 故障信息&版本信息

        /// <summary>
        /// 左后位置灯输出状态信号
        /// </summary>
        public string MRL_RLParkinglightOutputSt = string.Empty;
        /// <summary>
        /// 左后位置灯故障信号
        /// </summary>
        public string MRL_RLParkinglightFailureSt = string.Empty;
        /// <summary>
        /// 左后转向灯故障信号
        /// </summary>
        public string MRL_RLTurnLampFailureSt = string.Empty;
        /// <summary>
        /// 左后转向灯输出显示状态信号
        /// </summary>
        public string MRL_RLTurnLampOutputSt = string.Empty;
        /// <summary>
        /// 右后位置灯输出状态信号
        /// </summary>
        public string MRL_RRParkinglightOutputSt = string.Empty;
        /// <summary>
        /// 右后位置灯故障信号
        /// </summary>
        public string MRL_RRParkinglightFailureSt = string.Empty;
        /// <summary>
        /// 右后转向灯输出显示状态信号
        /// </summary>
        public string MRL_RRTurnLampOutputSt = string.Empty;
        /// <summary>
        /// 右后转向灯故障信号
        /// </summary>
        public string MRL_RRTurnLampFailureSt = string.Empty;

        private readonly List<uint> _canFdResponseIdList = new List<uint>();

        [Description("R,故障信息读取")]
        public string FaultRead = string.Empty;

        /// <summary>
        /// 读取故障信息
        /// </summary>
        [Description("读取故障信息")]
        public void FaultDetect()
        {
            MRL_RLParkinglightOutputSt = string.Empty;
            MRL_RLParkinglightFailureSt = string.Empty;
            MRL_RLTurnLampFailureSt = string.Empty;
            MRL_RLTurnLampOutputSt = string.Empty;
            MRL_RRParkinglightOutputSt = string.Empty;
            MRL_RRParkinglightFailureSt = string.Empty;
            MRL_RRTurnLampOutputSt = string.Empty;
            MRL_RRTurnLampFailureSt = string.Empty;
            _canFdResponseIdList.Clear();
            _canFdResponseIdList.Add(0x3E4);

            FaultRead = string.Empty;
            if (Can == null)
                return;

            foreach (var t in _canFdResponseIdList)
                Can.AddDoNotFilterCanId(t);

            Can.CanRecvDataPackages.Clear();
            Thread.Sleep(2000);

            if (Can.CanRecvDataPackages.Any())
            {
                var temp = Can.CanRecvDataPackages.ToArray();

                try
                {
                    var datas = new List<byte>();
                    foreach (var find in _canFdResponseIdList.Select(item => temp.ToList().Find(f => f.CanId == item)))
                    {
                        if (find == null)
                        {
                            FaultRead = string.Empty;
                            break;
                        }
                        datas.AddRange(find.CanData);
                        FaultRead += ValueHelper.GetHextStr(datas.ToArray());
                        FaultRead += " ";
                    }
                    FaultRead = FaultRead.TrimEnd(' ');
                    var value = new CanCommunicationMatrix.IntelMatrix(0x3E4, 12);
                    Array.Copy(datas.ToArray(), value.MatrixData, 12);
                    MRL_RLParkinglightOutputSt = ConvertState(value.GetMatrixData(65, 1));
                    MRL_RLParkinglightFailureSt = ConvertState(value.GetMatrixData(79, 1));
                    MRL_RLTurnLampFailureSt = ConvertState(value.GetMatrixData(75, 1));
                    MRL_RLTurnLampOutputSt = ConvertState(value.GetMatrixData(77, 1));
                    MRL_RRParkinglightOutputSt = ConvertState(value.GetMatrixData(64, 1));
                    MRL_RRParkinglightFailureSt = ConvertState(value.GetMatrixData(78, 1));
                    MRL_RRTurnLampOutputSt = ConvertState(value.GetMatrixData(76, 1));
                    MRL_RRTurnLampFailureSt = ConvertState(value.GetMatrixData(74, 1));
                }
                catch (Exception)
                {
                    FaultRead = string.Empty;
                }
            }

            foreach (var t in _canFdResponseIdList)
                Can.RemoveDoNotFilterCanId(t);
        }

        private string ConvertState(ulong i)
        {
            string state = string.Empty;
            switch (i)
            {
                case 0:
                    state = "Notactive";
                    break;
                case 1:
                    state = "active";
                    break;
            }
            return state;
        }


        [Description("R,SoftwareVer")]
        public string SoftwareVer = string.Empty;

        /// <summary>
        /// 读取故障信息
        /// </summary>
        [Description("读取软件版本信息")]
        public void SoftwareVerRead()
        {
            Can.SendStandardCanData(
                              0X751, new byte[] { 0x03, 0x22, 0xF1, 0x89, 0xCC, 0xCC, 0xCC, 0xCC });

            _canFdResponseIdList.Clear();
            _canFdResponseIdList.Add(0x7D1);

            SoftwareVer = string.Empty;
            if (Can == null)
                return;

            foreach (var t in _canFdResponseIdList)
                Can.AddDoNotFilterCanId(t);

            Can.CanRecvDataPackages.Clear();
            Thread.Sleep(200);

            if (Can.CanRecvDataPackages.Any())
            {
                var temp = Can.CanRecvDataPackages.ToArray();

                try
                {
                    foreach (var find in _canFdResponseIdList.Select(item => temp.ToList().Find(f => f.CanId == item)))
                    {
                        if (find == null)
                        {
                            SoftwareVer = string.Empty;
                            break;
                        }
                        var datas = new List<byte>();
                        datas.AddRange(find.CanData);
                        SoftwareVer += ValueHelper.GetHextStr(datas.ToArray());
                        SoftwareVer += " ";
                    }

                    SoftwareVer = SoftwareVer.TrimEnd(' ');
                }
                catch (Exception)
                {
                    SoftwareVer = string.Empty;
                }
            }

            foreach (var t in _canFdResponseIdList)
                Can.RemoveDoNotFilterCanId(t);
        }

        [Description("R,HardwareVer")]
        public string HardwareVer = string.Empty;

        /// <summary>
        /// 读取故障信息
        /// </summary>
        [Description("读取硬件版本信息")]
        public void HardwareVerRead()
        {
            Can.SendStandardCanData(
                              0X751, new byte[] { 0x03, 0x22, 0xF1, 0x7F, 0xCC, 0xCC, 0xCC, 0xCC });

            _canFdResponseIdList.Clear();
            _canFdResponseIdList.Add(0x7D1);

            HardwareVer = string.Empty;
            if (Can == null)
                return;

            foreach (var t in _canFdResponseIdList)
                Can.AddDoNotFilterCanId(t);

            Can.CanRecvDataPackages.Clear();
            Thread.Sleep(200);

            if (Can.CanRecvDataPackages.Any())
            {
                var temp = Can.CanRecvDataPackages.ToArray();

                try
                {
                    foreach (var find in _canFdResponseIdList.Select(item => temp.ToList().Find(f => f.CanId == item)))
                    {
                        if (find == null)
                        {
                            HardwareVer = string.Empty;
                            break;
                        }
                        var datas = new List<byte>();
                        datas.AddRange(find.CanData);
                        HardwareVer += ValueHelper.GetHextStr(datas.ToArray());
                        HardwareVer += " ";
                    }

                    HardwareVer = HardwareVer.TrimEnd(' ');
                }
                catch (Exception)
                {
                    HardwareVer = string.Empty;
                }
            }

            foreach (var t in _canFdResponseIdList)
                Can.RemoveDoNotFilterCanId(t);
        }

        [Description("R,TailQingChu")]
        public string TailQingChu = string.Empty;

        /// <summary>
        /// 位置灯请求报文
        /// </summary>
        [Description("位置灯清除报文")]
        public void TailQingChuRead()
        {
            _canFdResponseIdList.Clear();
            _canFdResponseIdList.Add(0x7D1);

            TailQingChu = string.Empty;
            if (Can == null)
                return;

            foreach (var t in _canFdResponseIdList)
                Can.AddDoNotFilterCanId(t);

            Can.CanRecvDataPackages.Clear();

            Can.SendStandardCanData(
                            0X751, new byte[] { 0x04, 0x14, 0xFF, 0xFF, 0xFF, 0xCC, 0xCC, 0xCC });
            Thread.Sleep(200);

            if (Can.CanRecvDataPackages.Any())
            {
                var temp = Can.CanRecvDataPackages.ToArray();

                try
                {
                    foreach (var find in _canFdResponseIdList.Select(item => temp.ToList().Find(f => f.CanId == item)))
                    {
                        if (find == null)
                        {
                            TailQingChu = string.Empty;
                            break;
                        }
                        var datas = new List<byte>();
                        datas.AddRange(find.CanData);
                        TailQingChu += ValueHelper.GetHextStr(datas.ToArray());
                        TailQingChu += " ";
                    }

                    TailQingChu = TailQingChu.TrimEnd(' ');
                }
                catch (Exception)
                {
                    TailQingChu = string.Empty;
                }
            }

            foreach (var t in _canFdResponseIdList)
                Can.RemoveDoNotFilterCanId(t);
        }

        [Description("R,TailQingQiu")]
        public string TailQingQiu = string.Empty;

        /// <summary>
        /// 位置灯请求报文
        /// </summary>
        [Description("位置灯请求报文")]
        public void TailQingQiuRead()
        {
            _canFdResponseIdList.Clear();
            _canFdResponseIdList.Add(0x7D1);

            TailQingQiu = string.Empty;
            if (Can == null)
                return;

            foreach (var t in _canFdResponseIdList)
                Can.AddDoNotFilterCanId(t);

            Can.CanRecvDataPackages.Clear();

            Can.SendStandardCanData(
                              0X751, new byte[] { 0x02, 0x10, 0x03, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC });
            Thread.Sleep(200);

            if (Can.CanRecvDataPackages.Any())
            {
                var temp = Can.CanRecvDataPackages.ToArray();

                try
                {
                    foreach (var find in _canFdResponseIdList.Select(item => temp.ToList().Find(f => f.CanId == item)))
                    {
                        if (find == null)
                        {
                            TailQingQiu = string.Empty;
                            break;
                        }
                        var datas = new List<byte>();
                        datas.AddRange(find.CanData);
                        TailQingQiu += ValueHelper.GetHextStr(datas.ToArray());
                        TailQingQiu += " ";
                    }

                    TailQingQiu = TailQingQiu.TrimEnd(' ');
                }
                catch (Exception)
                {
                    TailQingQiu = string.Empty;
                }
            }

            foreach (var t in _canFdResponseIdList)
                Can.RemoveDoNotFilterCanId(t);
        }

        [Description("R,StopQingChu")]
        public string StopQingChu = string.Empty;

        /// <summary>
        /// 位置灯请求报文
        /// </summary>
        [Description("制动灯清除报文")]
        public void StopQingChuRead()
        {
            _canFdResponseIdList.Clear();
            _canFdResponseIdList.Add(0x7D1);

            StopQingChu = string.Empty;
            if (Can == null)
                return;

            foreach (var t in _canFdResponseIdList)
                Can.AddDoNotFilterCanId(t);

            Can.CanRecvDataPackages.Clear();

            Can.SendStandardCanData(
                             0X751, new byte[] { 0x04, 0x14, 0xFF, 0xFF, 0xFF, 0xCC, 0xCC, 0xCC });

            Thread.Sleep(200);

            if (Can.CanRecvDataPackages.Any())
            {
                var temp = Can.CanRecvDataPackages.ToArray();

                try
                {
                    foreach (var find in _canFdResponseIdList.Select(item => temp.ToList().Find(f => f.CanId == item)))
                    {
                        if (find == null)
                        {
                            StopQingChu = string.Empty;
                            break;
                        }
                        var datas = new List<byte>();
                        datas.AddRange(find.CanData);
                        StopQingChu += ValueHelper.GetHextStr(datas.ToArray());
                        StopQingChu += " ";
                    }

                    StopQingChu = StopQingChu.TrimEnd(' ');
                }
                catch (Exception)
                {
                    StopQingChu = string.Empty;
                }
            }

            foreach (var t in _canFdResponseIdList)
                Can.RemoveDoNotFilterCanId(t);
        }

        [Description("R,StopQingQiu")]
        public string StopQingQiu = string.Empty;

        /// <summary>
        /// 位置灯请求报文
        /// </summary>
        [Description("制动灯请求报文")]
        public void StopQingQiuRead()
        {
            _canFdResponseIdList.Clear();
            _canFdResponseIdList.Add(0x7D1);

            StopQingQiu = string.Empty;
            if (Can == null)
                return;

            foreach (var t in _canFdResponseIdList)
                Can.AddDoNotFilterCanId(t);

            Can.CanRecvDataPackages.Clear();

            Can.SendStandardCanData(
                              0X751, new byte[] { 0x02, 0x10, 0x03, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC });
            Thread.Sleep(200);

            if (Can.CanRecvDataPackages.Any())
            {
                var temp = Can.CanRecvDataPackages.ToArray();

                try
                {
                    foreach (var find in _canFdResponseIdList.Select(item => temp.ToList().Find(f => f.CanId == item)))
                    {
                        if (find == null)
                        {
                            StopQingQiu = string.Empty;
                            break;
                        }
                        var datas = new List<byte>();
                        datas.AddRange(find.CanData);
                        StopQingQiu += ValueHelper.GetHextStr(datas.ToArray());
                        StopQingQiu += " ";
                    }

                    StopQingQiu = StopQingQiu.TrimEnd(' ');
                }
                catch (Exception)
                {
                    StopQingQiu = string.Empty;
                }
            }

            foreach (var t in _canFdResponseIdList)
                Can.RemoveDoNotFilterCanId(t);
        }

        #endregion
    }
}
