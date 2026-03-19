using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,SGM358-2后灯")]
    public sealed class Sgm3582RearLamp : ControllerBase
    {
        public LinBus LinWithBaudRate10417;

        public Sgm3582RearLamp(string name)
            : base(name)
        {
            LinBus.PushLinMsg += LinBus_PushLinMsg;
            ErrorThread();
            _mainWorkThread = new Thread(CyNormalCyclicTimer);
            _mainWorkThread.Start();
        }

        ~Sgm3582RearLamp()
        {
            Dispose();
        }

        private readonly Thread _mainWorkThread;
        private bool _isSleep = true;
        private readonly LinCommunicationMatrix.IntelMatrix _motorolaMatrix0X00 =
           new LinCommunicationMatrix.IntelMatrix(0x00, 3);

        private readonly object _lockLin = new object();
        private bool _isLeft = true;

        private bool _isTurnOn;
        private int _turnOnState;
        private bool _isTurnSwitchEnable;

        [Description("R/W,L&R左右同时控制")]
        public bool IsAllControl;

        [Description("设置为左灯")]
        public void SetLeft()
        {
            _isLeft = true;
        }

        [Description("设置为右灯")]
        public void SetRight()
        {
            _isLeft = false;
        }

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

        #region 点灯
        [Description("位置灯高亮")]
        public void TailNormalOn()
        {
            if (IsAllControl)
            {
                _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 1));
                _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 1));
            }
            else
            {
                if (_isLeft)
                {
                    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 1));
                }
                else
                {
                    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 1));
                }
            }
        }

        [Description("位置灯高亮,复用位置灯低亮")]
        public void TailHighLightOn()
        {
            if (IsAllControl)
            {
                _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 7));
                _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 7));
            }
            else
            {
                if (_isLeft)
                {
                    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 7));
                }
                else
                {
                    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 7));
                }
            }
        }

        [Description("位置灯灭")]
        public void TailNoAction()
        {
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(8, 4, 0));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(16, 4, 0));
        }

        [Description("转向灯A灯低亮B灯流水")]
        public void TurnSwipeTurn()
        {
            if (IsAllControl)
            {
                _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 1));
                _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 1));
            }
            else
            {
                if (_isLeft)
                {
                    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 1));
                }
                else
                {
                    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 1));
                }
            }

            _isTurnOn = true;
            _turnOnState = 1;
        }

        [Description("转向灯A灯高亮B灯流水")]
        public void TurnHighLightOn()
        {
            if (IsAllControl)
            {
                _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 4));
                _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 4));
            }
            else
            {
                if (_isLeft)
                {
                    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 4));
                }
                else
                {
                    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 4));
                }
            }

            _isTurnOn = true;
            _turnOnState = 2;
        }

        [Description("转向灯A灯低亮B灯高亮")]
        public void TurnHoldOnLight()
        {
            if (IsAllControl)
            {
                _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 3));
                _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 3));
            }
            else
            {
                if (_isLeft)
                {
                    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 3));
                }
                else
                {
                    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 3));
                }
            }

            _isTurnOn = true;
            _turnOnState = 3;
        }

        [Description("转向灯灭")]
        public void TurnNotActive()
        {
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(12, 3, 0));
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(20, 3, 0));
            _isTurnOn = false;
        }

        [Description("开启转向灯400ms亮灭")]
        public void StartSwitchTurn()
        {
            _isTurnSwitchEnable = true;
        }

        [Description("关闭转向灯400ms亮灭")]
        public void StopSwitchTurn()
        {
            _isTurnSwitchEnable = false;

            if (!_isTurnOn)
            {
                switch (_turnOnState)
                {
                    case 1:
                        TurnSwipeTurn();
                        break;

                    case 2:
                        TurnHighLightOn();
                        break;

                    case 3:
                        TurnHoldOnLight();
                        break;
                }
            }
        }

        [Description("简单解锁")]
        public void SimpleLeavingHome()
        {
            TailNoAction();
            //TurnNotActive();
            LampShowAbort();
            LampShowReset();

            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 3)); // RrLmpShwMod_64_5
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, 2)); // RrLmpShwTyp_64_5
        }

        [Description("简单闭锁")]
        public void SimpleComingHome()
        {
            TailNoAction();
            //TurnNotActive();
            LampShowAbort();
            LampShowReset();

            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 5)); // RrLmpShwMod_64_5
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, 2)); // RrLmpShwTyp_64_5
        }

        [Description("复杂解锁")]
        public void ComplexLeavingHome()
        {
            TailNoAction();
            //TurnNotActive();
            LampShowAbort();
            LampShowReset();

            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 3)); // RrLmpShwMod_64_5
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, 3)); // RrLmpShwTyp_64_5
        }

        [Description("复杂闭锁")]
        public void ComplexComingHome()
        {
            TailNoAction();
            //TurnNotActive();
            LampShowAbort();
            LampShowReset();

            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 5)); // RrLmpShwMod_64_5
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, 3)); // RrLmpShwTyp_64_5
        }

        //[Description("Follow Me Home")]
        //public void FollowMeHome()
        //{
        //    TailNoAction();
        //    TurnNotActive();
        //    LampShowAbort();
        //    LampShowReset();

        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 8)); // RrLmpShwMod_64_5
        //    _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, 2)); // RrLmpShwTyp_64_5
        //}

        //[Description("开启动画")]
        //public void LampShow(string type, string mode)
        //{
        //    if (type == "2" || type == "3")
        //    {
        //        if (mode == "1" || mode == "3" || mode == "4" || mode == "5" || mode == "6" || mode == "7" || mode == "8")
        //        {
        //            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, Convert.ToByte(mode))); // RrLmpShwMod_64_5
        //            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, Convert.ToByte(type))); // RrLmpShwTyp_64_5
        //        }
        //    }
        //}

        [Description("打断动画")]
        public void LampShowAbort()
        {
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 2)); // RrLmpShwMod_64_5
            //Thread.Sleep(100);
        }

        //[Description("复位动画信号")]
        private void LampShowReset()
        {
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(3, 5, 0)); // RrLmpShwMod_64_5
            _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(0, 3, 0)); // RrLmpShwTyp_64_5
        }

        private void CyNormalCyclicTimer()
        {
            var sendCount = 0;

            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(1);

                lock (_lockLin)
                {
                    try
                    {
                        if (LinWithBaudRate10417 == null)
                            continue;

                        _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(15, 1, 1));
                        _motorolaMatrix0X00.UpdateData(new MatrixValDefinition(23, 1, 1));

                        if (!_isSleep)
                        {
                            sendCount++;

                            //SendReadErrorMsg(0x0E);

                            switch (sendCount)
                            {
                                case 5:
                                    break;
                                case 10:
                                    SendCtrlMsg();
                                    SendReadErrorMsg(0x0E);
                                    break;

                                case 15:
                                    break;
                                case 20:
                                    SendCtrlMsg();
                                    SendReadErrorMsg(0x0F);
                                    break;

                                case 25:
                                    SwitchTurn();
                                    break;

                                case 30:
                                    SendCtrlMsg();
                                    SendReadErrorMsg(0x3A);
                                    break;

                                case 35:
                                    break;
                                case 40:
                                    SendCtrlMsg();
                                    SendReadErrorMsg(0x3B);
                                    break;

                                case 50:
                                    SwitchTurn();
                                    sendCount = 0;
                                    break;
                            }
                        }
                        else
                        {
                            sendCount = 0;
                            左侧A灯转向灯 = string.Empty;
                            左侧A灯制动灯 = string.Empty;
                            左侧A灯位置灯 = string.Empty;

                            右侧A灯转向灯 = string.Empty;
                            右侧A灯制动灯 = string.Empty;
                            右侧A灯位置灯 = string.Empty;

                            左侧B灯转向灯 = string.Empty;
                            左侧B灯位置灯 = string.Empty;

                            右侧B灯转向灯 = string.Empty;
                            右侧B灯位置灯 = string.Empty;
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        private void SwitchTurn()
        {
            if (_isTurnSwitchEnable)
            {
                if (_isTurnOn)
                {
                    TurnNotActive();
                }
                else
                {
                    switch (_turnOnState)
                    {
                        case 1:
                            TurnSwipeTurn();
                            break;

                        case 2:
                            TurnHighLightOn();
                            break;

                        case 3:
                            TurnHoldOnLight();
                            break;
                    }
                }
            }
        }

        private void SendCtrlMsg()
        {
            LinWithBaudRate10417.SendMasterLin(_motorolaMatrix0X00.MasterLinId,
                _motorolaMatrix0X00.MatrixData);
        }

        private void SendReadErrorMsg(byte linId)
        {
            LinWithBaudRate10417.SendMasterLin(linId, new byte[0]);
        }

        private void ReadErrorLa(int prkLmpFltA, int trnLmpFltA, int brkLmpFltA)
        {
            //byte[] echoLeftA;
            //if (LinWithBaudRate10417.SendSlaveLin(0x0E, out echoLeftA, timeOutMs: 50) &&
            //    echoLeftA != null && echoLeftA.Length >= 4)
            {
                //var prkLmpFltA = GetIntelValue(echoLeftA, 12, 3);
                //var trnLmpFltA = GetIntelValue(echoLeftA, 16, 3);
                //var brkLmpFltA = GetIntelValue(echoLeftA, 19, 3);
                switch (trnLmpFltA)
                {
                    case 0:
                        左侧A灯转向灯 = string.Format("{0}（No Fault）", trnLmpFltA);
                        break;
                    case 1:
                        左侧A灯转向灯 = string.Format("{0}（Short to Battery）", trnLmpFltA);
                        break;
                    case 2:
                        左侧A灯转向灯 = string.Format("{0}（short to Gnd）", trnLmpFltA);
                        break;
                    case 3:
                        左侧A灯转向灯 = string.Format("{0}（Open Load）", trnLmpFltA);
                        break;
                    case 4:
                        左侧A灯转向灯 = string.Format("{0}（Over temperature）", trnLmpFltA);
                        break;
                    case 5:
                        左侧A灯转向灯 = string.Format("{0}（Over voltage）", trnLmpFltA);
                        break;
                    case 6:
                        左侧A灯转向灯 = string.Format("{0}（Low Voltage）", trnLmpFltA);
                        break;
                    case 7:
                        左侧A灯转向灯 = string.Format("{0}（Not used）", trnLmpFltA);
                        break;
                    default:
                        左侧A灯转向灯 = string.Format("{0}（Unkonw）", trnLmpFltA);
                        break;
                }
                switch (brkLmpFltA)
                {
                    case 0:
                        左侧A灯制动灯 = string.Format("{0}（No Fault）", brkLmpFltA);
                        break;
                    case 1:
                        左侧A灯制动灯 = string.Format("{0}（Short to Battery）", brkLmpFltA);
                        break;
                    case 2:
                        左侧A灯制动灯 = string.Format("{0}（short to Gnd）", brkLmpFltA);
                        break;
                    case 3:
                        左侧A灯制动灯 = string.Format("{0}（Open Load）", brkLmpFltA);
                        break;
                    case 4:
                        左侧A灯制动灯 = string.Format("{0}（Over temperature）", brkLmpFltA);
                        break;
                    case 5:
                        左侧A灯制动灯 = string.Format("{0}（Over voltage）", brkLmpFltA);
                        break;
                    case 6:
                        左侧A灯制动灯 = string.Format("{0}（Low Voltage）", brkLmpFltA);
                        break;
                    case 7:
                        左侧A灯制动灯 = string.Format("{0}（Not used）", brkLmpFltA);
                        break;
                    default:
                        左侧A灯制动灯 = string.Format("{0}（Unkonw）", brkLmpFltA);
                        break;
                }
                switch (prkLmpFltA)
                {
                    case 0:
                        左侧A灯位置灯 = string.Format("{0}（No Fault）", prkLmpFltA);
                        break;
                    case 1:
                        左侧A灯位置灯 = string.Format("{0}（Short to Battery）", prkLmpFltA);
                        break;
                    case 2:
                        左侧A灯位置灯 = string.Format("{0}（short to Gnd）", prkLmpFltA);
                        break;
                    case 3:
                        左侧A灯位置灯 = string.Format("{0}（Open Load）", prkLmpFltA);
                        break;
                    case 4:
                        左侧A灯位置灯 = string.Format("{0}（Over temperature）", prkLmpFltA);
                        break;
                    case 5:
                        左侧A灯位置灯 = string.Format("{0}（Over voltage）", prkLmpFltA);
                        break;
                    case 6:
                        左侧A灯位置灯 = string.Format("{0}（Low Voltage）", prkLmpFltA);
                        break;
                    case 7:
                        左侧A灯位置灯 = string.Format("{0}（Not used）", prkLmpFltA);
                        break;
                    default:
                        左侧A灯位置灯 = string.Format("{0}（Unkonw）", prkLmpFltA);
                        break;
                }
            }
            //else
            //{
            //    左侧A灯转向灯 = string.Empty;
            //    左侧A灯制动灯 = string.Empty;
            //    左侧A灯位置灯 = string.Empty;
            //}
        }

        private void ReadErrorRa(int prkLmpFltA, int trnLmpFltA, int brkLmpFltA)
        {
            //byte[] echoRightA;
            //if (LinWithBaudRate10417.SendSlaveLin(0x3A, out echoRightA, timeOutMs: 50) &&
            //    echoRightA != null && echoRightA.Length >= 4)
            {
                //var prkLmpFltA = GetIntelValue(echoRightA, 12, 3);
                //var trnLmpFltA = GetIntelValue(echoRightA, 16, 3);
                //var brkLmpFltA = GetIntelValue(echoRightA, 19, 3);
                switch (trnLmpFltA)
                {
                    case 0:
                        右侧A灯转向灯 = string.Format("{0}（No Fault）", trnLmpFltA);
                        break;
                    case 1:
                        右侧A灯转向灯 = string.Format("{0}（Short to Battery）", trnLmpFltA);
                        break;
                    case 2:
                        右侧A灯转向灯 = string.Format("{0}（short to Gnd）", trnLmpFltA);
                        break;
                    case 3:
                        右侧A灯转向灯 = string.Format("{0}（Open Load）", trnLmpFltA);
                        break;
                    case 4:
                        右侧A灯转向灯 = string.Format("{0}（Over temperature）", trnLmpFltA);
                        break;
                    case 5:
                        右侧A灯转向灯 = string.Format("{0}（Over voltage）", trnLmpFltA);
                        break;
                    case 6:
                        右侧A灯转向灯 = string.Format("{0}（Low Voltage）", trnLmpFltA);
                        break;
                    case 7:
                        右侧A灯转向灯 = string.Format("{0}（Not used）", trnLmpFltA);
                        break;
                    default:
                        右侧A灯转向灯 = string.Format("{0}（Unkonw）", trnLmpFltA);
                        break;
                }
                switch (brkLmpFltA)
                {
                    case 0:
                        右侧A灯制动灯 = string.Format("{0}（No Fault）", brkLmpFltA);
                        break;
                    case 1:
                        右侧A灯制动灯 = string.Format("{0}（Short to Battery）", brkLmpFltA);
                        break;
                    case 2:
                        右侧A灯制动灯 = string.Format("{0}（short to Gnd）", brkLmpFltA);
                        break;
                    case 3:
                        右侧A灯制动灯 = string.Format("{0}（Open Load）", brkLmpFltA);
                        break;
                    case 4:
                        右侧A灯制动灯 = string.Format("{0}（Over temperature）", brkLmpFltA);
                        break;
                    case 5:
                        右侧A灯制动灯 = string.Format("{0}（Over voltage）", brkLmpFltA);
                        break;
                    case 6:
                        右侧A灯制动灯 = string.Format("{0}（Low Voltage）", brkLmpFltA);
                        break;
                    case 7:
                        右侧A灯制动灯 = string.Format("{0}（Not used）", brkLmpFltA);
                        break;
                    default:
                        右侧A灯制动灯 = string.Format("{0}（Unkonw）", brkLmpFltA);
                        break;
                }
                switch (prkLmpFltA)
                {
                    case 0:
                        右侧A灯位置灯 = string.Format("{0}（No Fault）", prkLmpFltA);
                        break;
                    case 1:
                        右侧A灯位置灯 = string.Format("{0}（Short to Battery）", prkLmpFltA);
                        break;
                    case 2:
                        右侧A灯位置灯 = string.Format("{0}（short to Gnd）", prkLmpFltA);
                        break;
                    case 3:
                        右侧A灯位置灯 = string.Format("{0}（Open Load）", prkLmpFltA);
                        break;
                    case 4:
                        右侧A灯位置灯 = string.Format("{0}（Over temperature）", prkLmpFltA);
                        break;
                    case 5:
                        右侧A灯位置灯 = string.Format("{0}（Over voltage）", prkLmpFltA);
                        break;
                    case 6:
                        右侧A灯位置灯 = string.Format("{0}（Low Voltage）", prkLmpFltA);
                        break;
                    case 7:
                        右侧A灯位置灯 = string.Format("{0}（Not used）", prkLmpFltA);
                        break;
                    default:
                        右侧A灯位置灯 = string.Format("{0}（Unkonw）", prkLmpFltA);
                        break;
                }
            }
            //else
            //{
            //    右侧A灯转向灯 = string.Empty;
            //    右侧A灯制动灯 = string.Empty;
            //    右侧A灯位置灯 = string.Empty;
            //}
        }

        private void ReadErrorLb(int prkLmpFltA, int trnLmpFltA)
        {
            //byte[] echoLeftB;
            //if (LinWithBaudRate10417.SendSlaveLin(0x0F, out echoLeftB, timeOutMs: 50) &&
            //    echoLeftB != null && echoLeftB.Length >= 4)
            {
                //var prkLmpFltA = GetIntelValue(echoLeftB, 12, 3);
                //var trnLmpFltA = GetIntelValue(echoLeftB, 16, 3);
                //var brkLmpFltA = GetIntelValue(echoLeftB, 19, 3);
                switch (trnLmpFltA)
                {
                    case 0:
                        左侧B灯转向灯 = string.Format("{0}（No Fault）", trnLmpFltA);
                        break;
                    case 1:
                        左侧B灯转向灯 = string.Format("{0}（Short to Battery）", trnLmpFltA);
                        break;
                    case 2:
                        左侧B灯转向灯 = string.Format("{0}（short to Gnd）", trnLmpFltA);
                        break;
                    case 3:
                        左侧B灯转向灯 = string.Format("{0}（Open Load）", trnLmpFltA);
                        break;
                    case 4:
                        左侧B灯转向灯 = string.Format("{0}（Over temperature）", trnLmpFltA);
                        break;
                    case 5:
                        左侧B灯转向灯 = string.Format("{0}（Over voltage）", trnLmpFltA);
                        break;
                    case 6:
                        左侧B灯转向灯 = string.Format("{0}（Low Voltage）", trnLmpFltA);
                        break;
                    case 7:
                        左侧B灯转向灯 = string.Format("{0}（Not used）", trnLmpFltA);
                        break;
                    default:
                        左侧B灯转向灯 = string.Format("{0}（Unkonw）", trnLmpFltA);
                        break;
                }
                switch (prkLmpFltA)
                {
                    case 0:
                        左侧B灯位置灯 = string.Format("{0}（No Fault）", prkLmpFltA);
                        break;
                    case 1:
                        左侧B灯位置灯 = string.Format("{0}（Short to Battery）", prkLmpFltA);
                        break;
                    case 2:
                        左侧B灯位置灯 = string.Format("{0}（short to Gnd）", prkLmpFltA);
                        break;
                    case 3:
                        左侧B灯位置灯 = string.Format("{0}（Open Load）", prkLmpFltA);
                        break;
                    case 4:
                        左侧B灯位置灯 = string.Format("{0}（Over temperature）", prkLmpFltA);
                        break;
                    case 5:
                        左侧B灯位置灯 = string.Format("{0}（Over voltage）", prkLmpFltA);
                        break;
                    case 6:
                        左侧B灯位置灯 = string.Format("{0}（Low Voltage）", prkLmpFltA);
                        break;
                    case 7:
                        左侧B灯位置灯 = string.Format("{0}（Not used）", prkLmpFltA);
                        break;
                    default:
                        左侧B灯位置灯 = string.Format("{0}（Unkonw）", prkLmpFltA);
                        break;
                }
            }
            //else
            //{
            //    左侧B灯转向灯 = string.Empty;
            //    左侧B灯位置灯 = string.Empty;
            //}
        }

        private void ReadErrorRb(int prkLmpFltA, int trnLmpFltA)
        {
            //byte[] echoRightB;
            //if (LinWithBaudRate10417.SendSlaveLin(0x3B, out echoRightB, timeOutMs: 50) &&
            //    echoRightB != null && echoRightB.Length >= 4)
            {
                //var prkLmpFltA = GetIntelValue(echoRightB, 12, 3);
                //var trnLmpFltA = GetIntelValue(echoRightB, 16, 3);
                //var brkLmpFltA = GetIntelValue(echoRightB, 19, 3);
                switch (trnLmpFltA)
                {
                    case 0:
                        右侧B灯转向灯 = string.Format("{0}（No Fault）", trnLmpFltA);
                        break;
                    case 1:
                        右侧B灯转向灯 = string.Format("{0}（Short to Battery）", trnLmpFltA);
                        break;
                    case 2:
                        右侧B灯转向灯 = string.Format("{0}（short to Gnd）", trnLmpFltA);
                        break;
                    case 3:
                        右侧B灯转向灯 = string.Format("{0}（Open Load）", trnLmpFltA);
                        break;
                    case 4:
                        右侧B灯转向灯 = string.Format("{0}（Over temperature）", trnLmpFltA);
                        break;
                    case 5:
                        右侧B灯转向灯 = string.Format("{0}（Over voltage）", trnLmpFltA);
                        break;
                    case 6:
                        右侧B灯转向灯 = string.Format("{0}（Low Voltage）", trnLmpFltA);
                        break;
                    case 7:
                        右侧B灯转向灯 = string.Format("{0}（Not used）", trnLmpFltA);
                        break;
                    default:
                        右侧B灯转向灯 = string.Format("{0}（Unkonw）", trnLmpFltA);
                        break;
                }
                switch (prkLmpFltA)
                {
                    case 0:
                        右侧B灯位置灯 = string.Format("{0}（No Fault）", prkLmpFltA);
                        break;
                    case 1:
                        右侧B灯位置灯 = string.Format("{0}（Short to Battery）", prkLmpFltA);
                        break;
                    case 2:
                        右侧B灯位置灯 = string.Format("{0}（short to Gnd）", prkLmpFltA);
                        break;
                    case 3:
                        右侧B灯位置灯 = string.Format("{0}（Open Load）", prkLmpFltA);
                        break;
                    case 4:
                        右侧B灯位置灯 = string.Format("{0}（Over temperature）", prkLmpFltA);
                        break;
                    case 5:
                        右侧B灯位置灯 = string.Format("{0}（Over voltage）", prkLmpFltA);
                        break;
                    case 6:
                        右侧B灯位置灯 = string.Format("{0}（Low Voltage）", prkLmpFltA);
                        break;
                    case 7:
                        右侧B灯位置灯 = string.Format("{0}（Not used）", prkLmpFltA);
                        break;
                    default:
                        右侧B灯位置灯 = string.Format("{0}（Unkonw）", prkLmpFltA);
                        break;
                }
            }
            //else
            //{
            //    右侧B灯转向灯 = string.Empty;
            //    右侧B灯位置灯 = string.Empty;
            //}
        }

        #endregion

        #region 版本信息

        [Description("R,HASCO总软件版本号")]
        public string HASCO总软件版本号 = string.Empty;
        [Description("R,HASCO应用程序软件版本号")]
        public string HASCO应用程序软件版本号 = string.Empty;
        [Description("R,HASCO应用程序软件小版本号")]
        public string HASCO应用程序软件小版本号 = string.Empty;
        [Description("R,HASCO引导程序软件版本号")]
        public string HASCO引导程序软件版本号 = string.Empty;
        [Description("R,HASCO引导程序软件小版本号")]
        public string HASCO引导程序软件小版本号 = string.Empty;
        [Description("R,HASCO标定版本号")]
        public string HASCO标定版本号 = string.Empty;
        [Description("R,HASCO标定小版本号")]
        public string HASCO标定小版本号 = string.Empty;
        [Description("R,HASCO应用程序软件零件号")]
        public string HASCO应用程序软件零件号 = string.Empty;
        [Description("R,HASCO引导程序软件零件号")]
        public string HASCO引导程序软件零件号 = string.Empty;
        [Description("R,HASCO标定零件号")]
        public string HASCO标定零件号 = string.Empty;
        [Description("R,信耀引导程序软件内部版本号")]
        public string 信耀引导程序软件内部版本号 = string.Empty;
        [Description("R,信耀应用程序内部版本号")]
        public string 信耀应用程序内部版本号 = string.Empty;
        [Description("R,泛亚标定零件号")]
        public string 泛亚标定零件号 = string.Empty;

        [Description("ReadVer")]
        public void ReadVer()
        {
            HASCO总软件版本号 = string.Empty;
            HASCO应用程序软件版本号 = string.Empty;
            HASCO应用程序软件小版本号 = string.Empty;
            HASCO引导程序软件版本号 = string.Empty;
            HASCO引导程序软件小版本号 = string.Empty;
            HASCO标定版本号 = string.Empty;
            HASCO标定小版本号 = string.Empty;
            HASCO应用程序软件零件号 = string.Empty;
            HASCO引导程序软件零件号 = string.Empty;
            HASCO标定零件号 = string.Empty;
            信耀引导程序软件内部版本号 = string.Empty;
            信耀应用程序内部版本号 = string.Empty;
            泛亚标定零件号 = string.Empty;

            byte nad;
            if (_isLeft)
            {
                nad = 0x64;
            }
            else
            {
                nad = 0x66;
            }

            HASCO总软件版本号 = ReadDid(4, 0xF1, 0xF0, nad, "ASCII");
            HASCO应用程序软件版本号 = ReadDid(4, 0xF1, 0xF1, nad, "ASCII");
            HASCO应用程序软件小版本号 = ReadDid(1, 0xF1, 0xF2, nad, "ASCII");
            HASCO引导程序软件版本号 = ReadDid(4, 0xF1, 0xF3, nad, "ASCII");
            HASCO引导程序软件小版本号 = ReadDid(1, 0xF1, 0xF4, nad, "ASCII");
            HASCO标定版本号 = ReadDid(4, 0xF1, 0xF7, nad, "ASCII");
            HASCO标定小版本号 = ReadDid(1, 0xF1, 0xF8, nad, "ASCII");
            HASCO应用程序软件零件号 = ReadDid(9, 0xF1, 0xF9, nad, "ASCII");
            HASCO引导程序软件零件号 = ReadDid(9, 0xF1, 0xFA, nad, "ASCII");
            HASCO标定零件号 = ReadDid(9, 0xF1, 0xFC, nad, "ASCII");
            信耀引导程序软件内部版本号 = ReadDid(7, 0xFC, 0x03, nad, "ASCII");
            信耀应用程序内部版本号 = ReadDid(7, 0xFC, 0x06, nad, "ASCII");
            泛亚标定零件号 = ReadDid(4, 0xF1, 0xC2, nad, "ASCII");
        }

        private string ReadDid(int dataLen, byte didHi, byte didLo, byte nad, string dataType)
        {
            if (LinWithBaudRate10417 == null)
                return string.Empty;

            var readValueDataLen = dataLen;
            var data0 = nad;

            //Console.WriteLine("config: " + infoName);
            //Console.WriteLine("config: " + standard);

            lock (_lockLin)
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

                        var getStr = string.Empty;
                        if (string.Equals(dataType, "ASCII", StringComparison.CurrentCultureIgnoreCase))
                            getStr = Encoding.ASCII.GetString(resultBs.ToArray());
                        else if (string.Equals(dataType, "Hex", StringComparison.CurrentCultureIgnoreCase))
                            getStr = resultBs.Aggregate(getStr, (current, t) => current + ValueHelper.GetHextStr(t));
                        return getStr;
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

                return string.Empty;
            }
        }

        #endregion

        #region 故障信息

        //public string 左侧A灯转向灯故障 = string.Empty;
        //public string 左侧B灯转向灯故障 = string.Empty;
        //public string 右侧A灯转向灯故障 = string.Empty;
        //public string 右侧B灯转向灯故障 = string.Empty;
        //public string 左侧A灯制动灯故障 = string.Empty;
        //public string 右侧A灯制动灯故障 = string.Empty;
        //public string 右侧A灯位置灯故障 = string.Empty;
        //public string 右侧B灯位置灯故障 = string.Empty;
        //public string 左侧A灯位置灯故障 = string.Empty;
        //public string 左侧B灯位置灯故障 = string.Empty;
        //public string 左侧A灯低压故障 = string.Empty;
        //public string 左侧B灯低压故障 = string.Empty;
        //public string 右侧A灯低压故障 = string.Empty;
        //public string 右侧B灯低压故障 = string.Empty;
        //public string 左侧A灯高压故障 = string.Empty;
        //public string 左侧B灯高压故障 = string.Empty;
        //public string 右侧A灯高压故障 = string.Empty;
        //public string 右侧B灯高压故障 = string.Empty;

        public string 左侧A灯转向灯 = string.Empty;
        public string 左侧A灯制动灯 = string.Empty;
        public string 左侧A灯位置灯 = string.Empty;

        public string 右侧A灯转向灯 = string.Empty;
        public string 右侧A灯制动灯 = string.Empty;
        public string 右侧A灯位置灯 = string.Empty;

        public string 左侧B灯转向灯 = string.Empty;
        public string 左侧B灯位置灯 = string.Empty;

        public string 右侧B灯转向灯 = string.Empty;
        public string 右侧B灯位置灯 = string.Empty;

        public int GetIntelValue(byte[] data, int startBit, int bitLen)
        {
            try
            {
                var bitData = new BitArray(data);

                var listBitStr = new List<string>();
                for (var i = 0; i < bitLen; i++)
                {
                    listBitStr.Add(bitData[startBit + i] ? "1" : "0");
                }

                var str = string.Empty;
                for (var i = listBitStr.Count - 1; i >= 0; i--)
                    str += listBitStr[i];

                var value = Convert.ToInt32(str, 2);

                return value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private long _errorLaTs = 0;
        private long _errorRaTs = 0;
        private long _errorLbTs = 0;
        private long _errorRbTs = 0;

        private readonly object _lockErrorLa = new object();
        private readonly object _lockErrorRa = new object();
        private readonly object _lockErrorLb = new object();
        private readonly object _lockErrorRb = new object();

        private void LinBus_PushLinMsg(string name, LinBus.LinDataPackage data)
        {
            if (LinWithBaudRate10417 != null && LinWithBaudRate10417.Name == name && data != null && data.LinDataLen >= 4)
            {
                var prkLmpFltA = GetIntelValue(data.LinData, 12, 3);
                var trnLmpFltA = GetIntelValue(data.LinData, 16, 3);
                var brkLmpFltA = GetIntelValue(data.LinData, 19, 3);

                if (data.LinId == 0x0E || data.LinId == LinBus.ConvertLinId(0x0E)) // left a
                {
                    lock (_lockErrorLa)
                    {
                        _errorLaTs = HighPrecisionTimer.GetTimestamp();
                        ReadErrorLa(prkLmpFltA, trnLmpFltA, brkLmpFltA);
                    }
                }
                else if (data.LinId == 0x3A || data.LinId == LinBus.ConvertLinId(0x3A)) // right a
                {
                    lock (_lockErrorRa)
                    {
                        _errorRaTs = HighPrecisionTimer.GetTimestamp();
                        ReadErrorRa(prkLmpFltA, trnLmpFltA, brkLmpFltA);
                    }
                }
                else if (data.LinId == 0x0F || data.LinId == LinBus.ConvertLinId(0x0F)) // left b
                {
                    lock (_lockErrorLb)
                    {
                        _errorLbTs = HighPrecisionTimer.GetTimestamp();
                        ReadErrorLb(prkLmpFltA, trnLmpFltA);
                    }
                }
                else if (data.LinId == 0x3B || data.LinId == LinBus.ConvertLinId(0x3B)) // right b
                {
                    lock (_lockErrorRb)
                    {
                        _errorRbTs = HighPrecisionTimer.GetTimestamp();
                        ReadErrorRb(prkLmpFltA, trnLmpFltA);
                    }
                }
            }
        }

        private void ErrorThread()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = new Action(() =>
                {
                    var nowTs = HighPrecisionTimer.GetTimestamp();

                    lock (_lockErrorLa)
                    {
                        var tsLa = HighPrecisionTimer.GetTimestampIntervalMs(_errorLaTs, nowTs);
                        if (tsLa > 1500)
                        {
                            左侧A灯转向灯 = string.Empty;
                            左侧A灯制动灯 = string.Empty;
                            左侧A灯位置灯 = string.Empty;
                        }
                    }

                    lock (_lockErrorRa)
                    {
                        var tsRa = HighPrecisionTimer.GetTimestampIntervalMs(_errorRaTs, nowTs);
                        if (tsRa > 1500)
                        {
                            右侧A灯转向灯 = string.Empty;
                            右侧A灯制动灯 = string.Empty;
                            右侧A灯位置灯 = string.Empty;
                            Console.WriteLine(@"Right RCLA ERROR TIME OUT: {0}ms", tsRa);
                        }
                    }

                    lock (_lockErrorLb)
                    {
                        var tsLb = HighPrecisionTimer.GetTimestampIntervalMs(_errorLbTs, nowTs);
                        if (tsLb > 1500)
                        {
                            左侧B灯转向灯 = string.Empty;
                            左侧B灯位置灯 = string.Empty;
                        }
                    }

                    lock (_lockErrorRb)
                    {
                        var tsRb = HighPrecisionTimer.GetTimestampIntervalMs(_errorRbTs, nowTs);
                        if (tsRb > 1500)
                        {
                            右侧B灯转向灯 = string.Empty;
                            右侧B灯位置灯 = string.Empty;
                            Console.WriteLine(@"Right RCLB ERROR TIME OUT: {0}ms", tsRb);
                        }
                    }
                }),
                Interval = 10
            });

            SchedulerAsync();
        }

        #endregion
    }
}
