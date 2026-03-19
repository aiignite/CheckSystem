using CommonUtility;
using CommonUtility.BusLoader;
using Go;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,427C-SUV格栅灯")]
    public sealed class Audi427CSuvGrilleLamp : ControllerBase
    {
        public CanBus CanFd;

        public Audi427CSuvGrilleLamp(string name) : base(name)
        {
            _motorolaMatrix.Add(new CanCommunicationMatrix.MotorolaMatrix(0x289, 64));
            _motorolaMatrix.Add(new CanCommunicationMatrix.MotorolaMatrix(0x28A, 64));
            _motorolaMatrix.Add(new CanCommunicationMatrix.MotorolaMatrix(0x28B, 64));
            _motorolaMatrix.Add(new CanCommunicationMatrix.MotorolaMatrix(0x28C, 64));
            _motorolaMatrix.Add(new CanCommunicationMatrix.MotorolaMatrix(0x2B3, 64));
            _motorolaMatrix.Add(new CanCommunicationMatrix.MotorolaMatrix(0x28E, 64));
            _motorolaMatrix.Add(new CanCommunicationMatrix.MotorolaMatrix(0x28F, 64));
            _motorolaMatrix.Add(new CanCommunicationMatrix.MotorolaMatrix(0x290, 64));
            _motorolaMatrix.Add(new CanCommunicationMatrix.MotorolaMatrix(0x291, 64));
            _motorolaMatrix.Add(new CanCommunicationMatrix.MotorolaMatrix(0x292, 64));

            foreach (var t in _motorolaMatrix)
            {
                _leds.Add(t.CanId, new List<MatrixValDefinition>());
                _leds[t.CanId].Add(new MatrixValDefinition(2, 6, 0)); //1
                _leds[t.CanId].Add(new MatrixValDefinition(12, 6, 0)); //2
                _leds[t.CanId].Add(new MatrixValDefinition(22, 6, 0)); //3
                _leds[t.CanId].Add(new MatrixValDefinition(16, 6, 0)); //4
                _leds[t.CanId].Add(new MatrixValDefinition(26, 6, 0)); //5
                _leds[t.CanId].Add(new MatrixValDefinition(36, 6, 0)); //6       
                _leds[t.CanId].Add(new MatrixValDefinition(46, 6, 0)); //7
                _leds[t.CanId].Add(new MatrixValDefinition(40, 6, 0)); //8
                _leds[t.CanId].Add(new MatrixValDefinition(50, 6, 0)); //9
                _leds[t.CanId].Add(new MatrixValDefinition(60, 6, 0)); //10            
                _leds[t.CanId].Add(new MatrixValDefinition(70, 6, 0)); //11
                _leds[t.CanId].Add(new MatrixValDefinition(64, 6, 0)); //12           
                _leds[t.CanId].Add(new MatrixValDefinition(74, 6, 0)); //13
                if (t.CanId == 0x28A)
                    continue;
                _leds[t.CanId].Add(new MatrixValDefinition(84, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(94, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(88, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(98, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(108, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(118, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(112, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(122, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(132, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(142, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(136, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(146, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(156, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(166, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(160, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(170, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(180, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(190, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(184, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(194, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(204, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(214, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(208, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(218, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(228, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(238, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(232, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(242, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(252, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(262, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(256, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(266, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(276, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(286, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(280, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(290, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(300, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(310, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(304, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(314, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(324, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(334, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(328, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(338, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(348, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(358, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(352, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(362, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(372, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(382, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(376, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(386, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(396, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(406, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(400, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(410, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(420, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(430, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(424, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(434, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(444, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(454, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(448, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(458, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(468, 6, 0));
                _leds[t.CanId].Add(new MatrixValDefinition(478, 6, 0));
            }

            MainWork();
            SchedulerAsync();
        }

        ~Audi427CSuvGrilleLamp() => Dispose();

        private object _lockCanSend = new object();
        private bool _isSleep = true;
        private CanCommunicationMatrix.MotorolaMatrix _ledRequestMsg0x2B0 = new CanCommunicationMatrix.MotorolaMatrix(0x2B0, 32);
        //private Dictionary<uint, byte[]> _leds = new Dictionary<uint, byte[]>();

        private int _mode;
        private List<CanCommunicationMatrix.MotorolaMatrix> _motorolaMatrix = new List<CanCommunicationMatrix.MotorolaMatrix>();
        private Dictionary<uint, List<MatrixValDefinition>> _leds = new Dictionary<uint, List<MatrixValDefinition>>();

        private void MainWork()
        {
            CanBus.PushCanMsg += CanBus_PushCanMsg;
            ModeCtrl50ms();
            PixelCtrl40ms();
        }

        private void ModeCtrl50ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    if (CanFd == null)
                        return;
                    if (_isSleep)
                        return;

                    if (!_isRunMode)
                    {
                        _ledRequestMsg0x2B0.UpdateData(new MatrixValDefinition(40, 2, (byte)_mode));
                        lock (_lockCanSend)
                            CanFd.SendStandardCanFdData(_ledRequestMsg0x2B0.CanId, _ledRequestMsg0x2B0.MatrixData);
                    }
                    else
                    {
                        _ledRequestMsg0x2B0.UpdateData(new MatrixValDefinition(40, 2, 2));
                        lock (_lockCanSend)
                            CanFd.SendStandardCanFdData(_ledRequestMsg0x2B0.CanId, _ledRequestMsg0x2B0.MatrixData);
                    }
                },
                Interval = 50
            });
        }

        private bool _ledFlag = true;
        private double _pwmFactor = 1.6d;
        private double _pwmMin = 0d;
        private double _pwmMax = 100.8d;

        private void PixelCtrl40ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    if (CanFd == null)
                        return;

                    if (_isSleep)
                        return;

                    if (_isRunMode)
                    {
                        var isOn = false;

                        foreach (var item in _motorolaMatrix)
                        {
                            for (var i = 0; i < _leds[item.CanId].Count; i++)
                            {
                                var pwm = _runningPwm;//100.8;
                                if (_leds[item.CanId][i].Value != (byte)(Math.Round(pwm / 1.6d, 0, MidpointRounding.AwayFromZero)))
                                {
                                    _leds[item.CanId][i].Value = (byte)(Math.Round(pwm / 1.6d, 0, MidpointRounding.AwayFromZero));
                                    isOn = true;
                                    break;
                                }
                            }

                            if (isOn)
                                break;
                        }

                        for (var i = 0; i < _motorolaMatrix.Count; i++)
                        {
                            var item = _motorolaMatrix[i];
                            foreach (var mvd in _leds[item.CanId])
                                item.UpdateData(mvd);

                            if (_ledFlag)
                            {
                                if (i % 2 == 0)
                                    lock (_lockCanSend)
                                        CanFd.SendStandardCanFdData(item.CanId, item.MatrixData);
                            }
                            else
                            {
                                if (i % 2 != 0)
                                    lock (_lockCanSend)
                                        CanFd.SendStandardCanFdData(item.CanId, item.MatrixData);
                            }
                        }

                        _ledFlag = !_ledFlag;

                        if (!isOn)
                            AllPixelClose();
                    }
                    else
                    {
                        if (_mode != 2)
                            return;

                        for (var i = 0; i < _motorolaMatrix.Count; i++)
                        {
                            var item = _motorolaMatrix[i];
                            foreach (var mvd in _leds[item.CanId])
                                item.UpdateData(mvd);

                            if (_ledFlag)
                            {
                                if (i % 2 == 0)
                                    lock (_lockCanSend)
                                        CanFd.SendStandardCanFdData(item.CanId, item.MatrixData);
                            }
                            else
                            {
                                if (i % 2 != 0)
                                    lock (_lockCanSend)
                                        CanFd.SendStandardCanFdData(item.CanId, item.MatrixData);
                            }
                        }

                        _ledFlag = !_ledFlag;
                    }
                },
                Interval = 40
            });
        }

        [Description("打开CAN")]
        public void StartCan()
        {
            _isSleep = false;
        }

        [Description("关闭CAN")]
        public void StopCan()
        {
            _isSleep = true;
        }

        [Description("静态点亮")]
        public void FroSigLampOn()
        {
            _mode = 1;
        }

        [Description("静态关闭")]
        public void FroSigLampOff()
        {
            _mode = 0;
        }

        [Description("动态点亮")]
        public void FroSigLampPixel()
        {
            _mode = 2;
        }

        [Description("打开所有像素")]
        public void AllPixelOpen(double pwm)
        {
            if (pwm < _pwmMin || pwm > _pwmMax)
                return;

            foreach (var item in _motorolaMatrix)
            {
                for (var i = 0; i < _leds[item.CanId].Count; i++)
                {
                    _leds[item.CanId][i].Value = (byte)(Math.Round(pwm / _pwmFactor, 0, MidpointRounding.AwayFromZero));
                }
            }
        }

        [Description("关闭所有灯像素")]
        public void AllPixelClose()
        {
            AllPixelOpen(0);
        }

        [Description("打开奇数像素")]
        public void OddPixelOpen(double pwm)
        {
            if (pwm < _pwmMin || pwm > _pwmMax)
                return;

            var index = 0;

            foreach (var item in _motorolaMatrix)
            {
                for (var i = 0; i < _leds[item.CanId].Count; i++)
                {
                    if (index % 2 != 0)
                        _leds[item.CanId][i].Value = (byte)(Math.Round(pwm / _pwmFactor, 0, MidpointRounding.AwayFromZero));
                    index++;
                }
            }
        }

        [Description("关闭奇数像素")]
        public void OddPixelClose()
        {
            OddPixelOpen(0);
        }

        [Description("打开偶数像素")]
        public void EvenPixelOpen(double pwm)
        {
            if (pwm < _pwmMin || pwm > _pwmMax)
                return;

            var index = 0;

            foreach (var item in _motorolaMatrix)
            {
                for (var i = 0; i < _leds[item.CanId].Count; i++)
                {
                    if (index % 2 == 0)
                        _leds[item.CanId][i].Value = (byte)(Math.Round(pwm / _pwmFactor, 0, MidpointRounding.AwayFromZero));
                    index++;
                }
            }
        }

        [Description("关闭偶数像素")]
        public void EvenPixelClose()
        {
            EvenPixelOpen(0);
        }

        private bool _isRunMode;
        private double _runningPwm;

        [Description("打开跑马灯")]
        public void OpenRunMode(double pwm)
        {
            if (pwm <= _pwmMin || pwm > _pwmMax)
                _runningPwm = _pwmMax;
            else
                _runningPwm = pwm;
            AllPixelClose();
            _isRunMode = true;
        }

        [Description("关闭跑马灯")]
        public void CloseRunMode()
        {
            AllPixelClose();
            _isRunMode = false;
        }

        //#region 版本信息

        //[Description("R,软件版本号F18A")]
        //public string SwVer = string.Empty;
        //[Description("R,APP软件版本号F189")]
        //public string AppVer = string.Empty;
        //[Description("R,APP软件零件本号F187")]
        //public string AppPartNo = string.Empty;
        //[Description("R,Boot软件版本号F180")]
        //public string BootVer = string.Empty;
        //[Description("R,Boot软件零件本号F181")]
        //public string BootPartNo = string.Empty;

        //[Description("Read软件版本号F18A")]
        //public void ReadSwVer()
        //{
        //    SwVer = string.Empty;
        //    SwVer = Encoding.ASCII.GetString(ReadDid(0xF1, 0x8A));
        //}

        //[Description("ReadAPP软件版本号F189")]
        //public void ReadAppVer()
        //{
        //    AppVer = string.Empty;
        //    AppVer = Encoding.ASCII.GetString(ReadDid(0xF1, 0x89));
        //}

        //[Description("ReadAPP软件零件本号F187")]
        //public void ReadAppPartNo()
        //{
        //    AppPartNo = string.Empty;
        //    AppPartNo = Encoding.ASCII.GetString(ReadDid(0xF1, 0x87));
        //}

        //[Description("ReadBoot软件版本号F180")]
        //public void ReadBootVer()
        //{
        //    BootVer = string.Empty;
        //    BootVer = Encoding.ASCII.GetString(ReadDid(0xF1, 0x80));
        //}

        //[Description("ReadBoot软件零件本号F181")]
        //public void ReadBootPartNo()
        //{
        //    BootPartNo = string.Empty;
        //    BootPartNo = Encoding.ASCII.GetString(ReadDid(0xF1, 0x81));
        //}

        //private byte[] ReadDid(byte didHi, byte didLo)
        //{
        //    if (CanFd == null)
        //        return new byte[0];

        //    uint CanDiagnosisRequestFunCanId = 0x7DF;
        //    uint _canDiagnosisResponseCanId = 0x7BB;

        //    byte[] echo;
        //    lock (_lockCanSend)
        //    {
        //        if (CanFd.CanBusWithUds.TryReadData(CanDiagnosisRequestFunCanId, _canDiagnosisResponseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, didHi, didLo, out echo))
        //        {
        //            return echo;
        //        }
        //        else
        //        {
        //            Thread.Sleep(250);
        //            if (CanFd.CanBusWithUds.TryReadData(CanDiagnosisRequestFunCanId, _canDiagnosisResponseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, didHi, didLo, out echo))
        //            {
        //                return echo;
        //            }
        //        }
        //    }

        //    return new byte[0];
        //}

        //#endregion

        #region 版本信息_20260317

        [Description("R,软件版本号F18A")]
        public string SwVer = string.Empty;
        [Description("R,APP软件版本号F189")]
        public string AppVer = string.Empty;
        [Description("R,APP软件零件本号F187")]
        public string AppPartNo = string.Empty;
        [Description("R,Boot软件版本号F180")]
        public string BootVer = string.Empty;
        [Description("R,Boot软件零件本号F181")]
        public string BootPartNo = string.Empty;

        [Description("Read软件版本号F18A")]
        public void ReadSwVer()
        {
            SwVer = string.Empty;
            SwVer = Encoding.ASCII.GetString(ReadDid(0xF1, 0x8A));
        }

        [Description("ReadAPP软件版本号F189")]
        public void ReadAppVer()
        {
            AppVer = string.Empty;
            AppVer = Encoding.ASCII.GetString(ReadDid(0xF1, 0x89));
        }

        [Description("ReadAPP软件零件本号F187")]
        public void ReadAppPartNo()
        {
            AppPartNo = string.Empty;
            AppPartNo = Encoding.ASCII.GetString(ReadDid(0xF1, 0x87));
        }

        [Description("ReadBoot软件版本号F180")]
        public void ReadBootVer()
        {
            BootVer = string.Empty;
            BootVer = Encoding.ASCII.GetString(ReadDid(0xF1, 0x80));
        }

        [Description("ReadBoot软件零件本号F181")]
        public void ReadBootPartNo()
        {
            BootPartNo = string.Empty;
            BootPartNo = Encoding.ASCII.GetString(ReadDid(0xF1, 0x81));
        }

        private void CanBus_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (CanFd is null)
                return;

            if (CanFd.Name != name)
                return;

            if (data is null || data.CanData.Length < 8)
                return;

            if (data.CanId == DiagnosticReqCanId || data.CanId == DiagnosticRecvCanId)
            {
                if ((data.CanData[3] == 0xF1 && data.CanData[4] == 0x89))
                {

                }
                else if (data.CanData[2] == 0xF1 && data.CanData[3] == 0x89)
                {

                }
                else
                {
                    Console.WriteLine(ValueHelper.GetHextStrWithOx(data.CanData));
                }
            }

            if (data.CanId == DiagnosticRecvCanId)
            {
                //Console.WriteLine("rx 0x14DAF1BE: " + ValueHelper.GetHextStr(data.CanData));

                if (_bReadDid)
                {
                    _uds22Buffer.AddRange(data.CanData);
                    _bReadDidWaitHandle.Set();
                }
            }
        }

        private byte[] ReadDid(byte didHi, byte didLo)
        {
            if (CanFd is null)
                return new byte[0];
            lock (_lockCanSender)
            {
                byte[] echo;
                if (CanFdUds22(didHi, didLo, out echo))
                {
                    Thread.Sleep(1);
                    return echo;
                }
            }

            Thread.Sleep(100);

            lock (_lockCanSender)
            {
                byte[] echo;
                if (CanFdUds22(didHi, didLo, out echo))
                {
                    Thread.Sleep(1);
                    return echo;
                }
            }


            return new byte[0];
        }

        private bool CanFdUds22(byte didHi, byte didLo, out byte[] echo)
        {
            _uds22Buffer.Clear();

            if (CanFd is null)
            {
                echo = new byte[0];
                return false;
            }

            _bReadDidWaitHandle.Reset();
            _bReadDid = true;

            var firstSend = new byte[] { 0x03, 0x22, didHi, didLo, 0x00, 0x00, 0x00, 0x00 };

            var t1 = HighPrecisionTimer.GetTimestamp();

            //使用 ThreadPool 异步发送，减少 Task 创建开销
            ThreadPool.QueueUserWorkItem(_ => CanFd.SendStandardCanFdData(DiagnosticReqCanId, firstSend));

            // 同步等待响应
            var isFirstRecvOk = _bReadDidWaitHandle.Wait(250);

            _bReadDid = false;
            _bReadDidWaitHandle.Reset();

            var t2 = HighPrecisionTimer.GetTimestamp();
            Console.WriteLine("read did cost: {0}/ms", HighPrecisionTimer.GetTimestampIntervalMs(t1, t2));

            if (isFirstRecvOk)
            {
                var bufferArray = _uds22Buffer.ToArray();
                var count = bufferArray.Length;

                if (count >= 4)
                {
                    var b1 = bufferArray[0];
                    var b2 = bufferArray[1];
                    var b3 = bufferArray[2];
                    var b4 = bufferArray[3];

                    if (count == 8 && b1.GetByteHighOrder() == 0x00 && b2 == 0x62 && b3 == didHi && b4 == didLo)
                    {
                        var datalen = b1.GetByteLowOrder();
                        if (datalen >= 3 && datalen <= 7)
                        {
                            echo = new byte[datalen - 3];
                            Array.Copy(bufferArray, 4, echo, 0, datalen - 3);
                            return true;
                        }
                    }
                    else if (count > 8 && b1.GetByteHighOrder() == 0x00)
                    {
                        var datalen = b2;
                        if (datalen >= 3 && datalen <= 62 && count >= datalen + 2)
                        {
                            echo = new byte[datalen - 3];
                            Array.Copy(bufferArray, 5, echo, 0, datalen - 3);
                            return true;
                        }
                    }
                }
            }

            echo = new byte[0];
            return false;
        }

        private readonly object _lockCanSender = new object();
        private List<byte> _uds22Buffer = new List<byte>();
        private bool _bReadDid;
        private bool _bReadSecodnFrame;
        private ManualResetEventSlim _bReadDidWaitHandle = new ManualResetEventSlim(false);
        private uint DiagnosticReqCanId = 0x6EC;
        private uint DiagnosticRecvCanId = 0x6ED;

        #endregion
    }
}
