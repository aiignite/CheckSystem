using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,SGM458后灯")]
    public sealed class Sgm458RearLamp : ControllerBase
    {
        public LinBus LinWithBaudRate10417;

        public Sgm458RearLamp(string name)
            : base(name)
        {
            if (_mainWorkThread != null)
            {
                _mainWorkThread.Abort();
                _mainWorkThread.Join();
            }
            _mainWorkThread = new Thread(CyNormalCyclicTimer);
            _mainWorkThread.Start();
        }

        ~Sgm458RearLamp()
        {
            Dispose();
        }

        private readonly Thread _mainWorkThread;
        private bool _isSleep = true;
        private readonly LinCommunicationMatrix.IntelMatrix _motorolaMatrix0X00 =
           new LinCommunicationMatrix.IntelMatrix(0x00, 3);
        private readonly object _lockLin = new object();

        [Description("LIN唤醒")]
        public void LampAwake()
        {
            _isSleep = false;
        }

        [Description("LIN休眠")]
        public void LampSleep()
        {
            _isSleep = true;
        }

        [Description("Tail-NoAction-灭")]
        public void TailNoAction()
        {
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 0));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 0));
        }

        [Description("Tail-NormalOn-低亮")]
        public void TailNormalOn()
        {
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 1));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 1));
        }

        //[Description("Tail-NormalOff")]
        //public void TailNormalOff()
        //{
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 2));
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 2));
        //}

        //[Description("Tail-RampOn")]
        //public void TailRampOn()
        //{
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 3));
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 3));
        //}

        //[Description("Tail-RampOff")]
        //public void TailRampOff()
        //{
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 4));
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 4));
        //}

        //[Description("Tail-SwipeOn")]
        //public void TailSwipeOn()
        //{
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 5));
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 5));
        //}

        //[Description("Tail-SwipeOff")]
        //public void TailSwipeOff()
        //{
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 6));
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 6));
        //}

        [Description("Tail-HighlightOn-高亮")]
        public void TailHighlightOn()
        {
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 7));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 7));
        }

        //[Description("Tail-HighlightOff")]
        //public void TailHighlightOff()
        //{
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 8));
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 8));
        //}

        [Description("Turn-NotActive-灭")]
        public void TurnNotActive()
        {
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 0));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 0));
        }

        ////[Description("Turn-Active2")]
        ////public void TurnActive2()
        ////{
        ////    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 5));
        ////    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 5));
        ////}

        [Description("Turn-NormalFlash")]
        public void TurnNormalFlash()
        {
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 2));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 2));
        }

        [Description("Turn-SwipeTurn-低亮")]
        public void TurnSwipeTurn()
        {
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 1));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 1));
        }

        [Description("Turn-Active1-高亮")]
        public void TurnActive1()
        {
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 4));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 4));
        }

        //[Description("Turn-HoldOnLight")]
        //public void TurnHoldOnLight()
        //{
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 3));
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 3));
        //}

        [Description("简单解锁")]
        public void SimpleLeavingHome()
        {
            TailNoAction();
            TurnNotActive();
            LampShowAbort();
            Thread.Sleep(100);

            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 3));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, 2));
        }

        [Description("简单闭锁")]
        public void SimpleComingHome()
        {
            TailNoAction();
            TurnNotActive();
            LampShowAbort();
            Thread.Sleep(100);

            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 5));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, 2));
        }

        [Description("复杂解锁")]
        public void ComplexLeavingHome()
        {
            TailNoAction();
            TurnNotActive();
            LampShowAbort();
            Thread.Sleep(100);

            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 3));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, 3));
        }

        [Description("复杂闭锁")]
        public void ComplexComingHome()
        {
            TailNoAction();
            TurnNotActive();
            LampShowAbort();
            Thread.Sleep(100);

            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 5));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, 3));
        }

        [Description("Follow Me Home")]
        public void FollowMeHome()
        {
            TailNoAction();
            TurnNotActive();
            LampShowAbort();
            Thread.Sleep(100);

            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 8));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, 2));
        }

        [Description("打断动画")]
        public void LampShowAbort()
        {
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 0));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, 0));
        }

        private void CyNormalCyclicTimer()
        {
            var list = new List<byte> { 0x0E, 0x3A, 0x0F, 0x3B };
            //var list = new List<byte> { 0x3A, 0x3A, 0x3A, 0x3A };
            var i = 0;

            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                try
                {
                    if (LinWithBaudRate10417 == null)
                        continue;

                    lock (_lockLin)
                    {
                        _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(15, 1, 1));
                        _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(23, 1, 1));

                        if (!_isSleep)
                        {
                            LinWithBaudRate10417.SendMasterLin(0x39, new byte[] { 0xFF, 0xFF, 0xF0, 0xFF });
                            LinWithBaudRate10417.SendMasterLin(_motorolaMatrix0X00.MasterLinId, _motorolaMatrix0X00.MatrixData);

                            //continue;

                            byte[] echo;
                            LinWithBaudRate10417.SendSlaveLin(list[i], out echo, 200);

                            if (echo != null && echo.Length == 4)
                            {
                                var str = string.Empty;
                                foreach (var t in echo)
                                {
                                    str += ValueHelper.GetHextStrWithOx(t) + " ";
                                }

                                var data = new LinCommunicationMatrix.IntelMatrix(0x00, 8);

                                Console.WriteLine("FeedBack: {0}, {1}", ValueHelper.GetHextStr(list[i]), str);

                                if (list[i] == 0x0E) //左A
                                {
                                    Array.Copy(echo, data.MatrixData, 8);

                                }
                                else if (list[i] == 0x0F) //左B
                                {
                                    Array.Copy(echo, data.MatrixData, 8);

                                }
                                if (list[i] == 0x3A) //右A
                                {
                                    Array.Copy(echo, data.MatrixData, 8);

                                }
                                else if (list[i] == 0x3B) //右B
                                {
                                    Array.Copy(echo, data.MatrixData, 8);
                                }
                            }

                            i++;
                            if (i == 4)
                                i = 0;

                            //LinWithBaudRate10417.SendMasterLin(_motorolaMatrix0X00.MasterLinId, new byte[] { 0x00, 0x98, 0x98 });
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        #region Version

        [Description("R,HASCO从节点软件版本号")]
        public string HASCO从节点软件版本号 = string.Empty;

        [Description("读HASCO从节点软件版本号")]
        public void ReadHASCO从节点软件版本号(string nad)
        {
            HASCO从节点软件版本号 = string.Empty;
            HASCO从节点软件版本号 = ReadDid(20, 0xF1, 0x94, Convert.ToByte(nad, 16), "ASCII");
        }

        private string ReadDid(int dataLen, byte didHi, byte didLo, byte nad, string dataType)
        {
            if (LinWithBaudRate10417 == null)
            {
                return string.Empty;
            }

            var readValueDataLen = dataLen;
            var data0 = nad;

            //Console.WriteLine("config: " + infoName);
            //Console.WriteLine("config: " + standard);

            try
            {
                lock (_lockLin)
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

                        var getStr = string.Empty;
                        if (string.Equals(dataType, "ASCII", StringComparison.CurrentCultureIgnoreCase))
                            getStr = Encoding.ASCII.GetString(resultBs.ToArray());
                        else if (string.Equals(dataType, "Hex", StringComparison.CurrentCultureIgnoreCase))
                            getStr = resultBs.Aggregate(getStr, (current, t) => current + ValueHelper.GetHextStr(t));
                        return getStr;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        #endregion
    }
}
