using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,DC1E后灯")]
    public sealed class Dc1ERearLamp : ControllerBase
    {
        public CanBus Can;

        [Description("R,A灯左位置动灯故障")]
        public string StsOfLedPosnLampLe1;

        [Description("R,A灯右位置动灯故障")]
        public string StsOfLedPosnLampRi1;

        [Description("R,B灯左位置动灯故障")]
        public string StsOfLedPosnLampLe2;

        [Description("R,B灯右位置动灯故障")]
        public string StsOfLedPosnLampRi2;

        [Description("R,A灯左制动灯故障")]
        public string StsOfLedStopLampLe1;

        [Description("R,A灯右制动灯故障")]
        public string StsOfLedStopLampRi1;

        [Description("R,B灯左制动灯故障")]
        public string StsOfLedStopLampLe2;

        [Description("R,B灯右制动灯故障")]
        public string StsOfLedStopLampRi2;

        [Description("R,A灯左转灯故障")]
        public string StsOfLedTurnIndcrLe1;

        [Description("R,A灯右转灯故障")]
        public string StsOfLedTurnIndcrRi1;

        [Description("R,B灯左转灯故障")]
        public string StsOfLedTurnIndcrLe2;

        [Description("R,B灯右转灯故障")]
        public string StsOfLedTurnIndcrRi2;

        [Description("R,A灯左倒车灯故障")]
        public string StsOfLedRvsgLampLe2;

        [Description("R,A灯右倒车灯故障")]
        public string StsOfLedRvsgLampRi2;

        [Description("R,A灯左雾灯故障")]
        public string StsOfLedReFogLampLe2;

        [Description("R,A灯右雾灯故障")]
        public string StsOfLedReFogLampRi2;

        private readonly object _lockError = new object();
        private DateTime _lastErrorDateTimeRcml;
        private DateTime _lastErrorDateTimeRcmr;
        private DateTime _lastErrorDateTimeRcmm;

        public Dc1ERearLamp(string name)
            : base(name)
        {
            CanBus.PushCanMsg += CanBus_PushCanMsg;
            //StartReadMsg();

            if (MainWorkThread != null)
            {
                MainWorkThread.Abort();
                MainWorkThread.Join();
            }

            MainWorkThread = new Thread(
                MainWork)
            { IsBackground = true };
            MainWorkThread.Start();

            _errorMsg.Add("RCML", new CanCommunicationMatrix.MotorolaMatrix(0x250, 8));
            _errorMsg.Add("RCMR", new CanCommunicationMatrix.MotorolaMatrix(0x260, 8));
            _errorMsg.Add("RCMM", new CanCommunicationMatrix.MotorolaMatrix(0x254, 8));
        }

        private readonly Dictionary<string, CanCommunicationMatrix> _errorMsg = new Dictionary<string, CanCommunicationMatrix>();

        private void CanBus_PushCanMsg(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            #region 故障

            if (data.CanId == 0x250) // A左
            {
                lock (_lockError)
                {
                    _errorMsg["RMML"].MatrixData = data.CanData;
                    StsOfLedPosnLampLe1 = _errorMsg["RMML"].GetMatrixData(8, 2).ToString();
                    StsOfLedStopLampLe1 = _errorMsg["RMML"].GetMatrixData(20, 2).ToString();
                    StsOfLedTurnIndcrLe1 = _errorMsg["RMML"].GetMatrixData(28, 2).ToString();

                    _lastErrorDateTimeRcml = DateTime.Now;
                }
            }
            else if (data.CanId == 0x260) // A右
            {
                lock (_lockError)
                {
                    _errorMsg["RCMR"].MatrixData = data.CanData;
                    StsOfLedPosnLampRi1 = _errorMsg["RCMR"].GetMatrixData(8, 2).ToString();
                    StsOfLedStopLampRi1 = _errorMsg["RCMR"].GetMatrixData(22, 2).ToString();
                    StsOfLedTurnIndcrRi1 = _errorMsg["RCMR"].GetMatrixData(24, 2).ToString();

                    _lastErrorDateTimeRcmr = DateTime.Now;
                }
            }
            else if (data.CanId == 0x254) // B
            {
                lock (_lockError)
                {
                    _errorMsg["RCMM"].MatrixData = data.CanData;

                    StsOfLedPosnLampLe2 = _errorMsg["RCMM"].GetMatrixData(8, 2).ToString();
                    StsOfLedPosnLampRi2 = _errorMsg["RCMM"].GetMatrixData(10, 2).ToString();

                    StsOfLedStopLampLe2 = _errorMsg["RCMM"].GetMatrixData(24, 2).ToString();
                    StsOfLedStopLampRi2 = _errorMsg["RCMM"].GetMatrixData(26, 2).ToString();

                    StsOfLedTurnIndcrLe2 = _errorMsg["RCMM"].GetMatrixData(28, 2).ToString();
                    StsOfLedTurnIndcrRi2 = _errorMsg["RCMM"].GetMatrixData(30, 2).ToString();

                    StsOfLedRvsgLampLe2 = _errorMsg["RCMM"].GetMatrixData(20, 2).ToString();
                    StsOfLedRvsgLampRi2 = _errorMsg["RCMM"].GetMatrixData(22, 2).ToString();

                    StsOfLedReFogLampLe2 = _errorMsg["RCMM"].GetMatrixData(12, 2).ToString();
                    StsOfLedReFogLampRi2 = _errorMsg["RCMM"].GetMatrixData(14, 2).ToString();

                    _lastErrorDateTimeRcmm = DateTime.Now;
                }
            }

            #endregion

            if (IsReadMsg)
            {
                //public string Read0x250Msg = string.Empty;
                //public string Read0x260Msg = string.Empty;
                //public string Read0x240Msg = string.Empty;

                if (onPushCanDataType == CanBus.OnPushCanDataType.NotFilterRx || onPushCanDataType == CanBus.OnPushCanDataType.FilterRx)
                {
                    if (data.CanData != null)
                    {
                        if (data.CanId == 0x250)
                        {
                            Read0X250Msg = "0x250:";
                            foreach (var t in data.CanData)
                                Read0X250Msg += ValueHelper.GetHextStr(t);
                        }
                        else if (data.CanId == 0x260)
                        {
                            Read0X260Msg = "0x260:";
                            foreach (var t in data.CanData)
                                Read0X260Msg += ValueHelper.GetHextStr(t);
                        }
                        else if (data.CanId == 0x254)
                        {
                            Read0X254Msg = "0x254:";
                            foreach (var t in data.CanData)
                                Read0X254Msg += ValueHelper.GetHextStr(t);
                        }
                        else if (data.CanId == 0x533)
                        {
                            Read0X533Msg = "0x533:";
                            foreach (var t in data.CanData)
                                Read0X533Msg += ValueHelper.GetHextStr(t);
                        }
                        else if (data.CanId == 0x534)
                        {
                            Read0X534Msg = "0x534:";
                            foreach (var t in data.CanData)
                                Read0X534Msg += ValueHelper.GetHextStr(t);
                        }
                        else if (data.CanId == 0x535)
                        {
                            Read0X535Msg = "0x535:";
                            foreach (var t in data.CanData)
                                Read0X535Msg += ValueHelper.GetHextStr(t);
                        }
                    }
                }
            }
        }

        ~Dc1ERearLamp()
        {
            Dispose();
        }

        private Thread MainWorkThread { get; set; }
        private bool IsSleep { get; set; }
        private LampType ThisLampType { get; set; }
        private ushort TurnKeepTime { get; set; }
        private bool IsTurnHoldingOn { get; set; }
        private bool IsTurnRunningOn { get; set; }
        private bool IsTurnFlickerOn { get; set; }
        private bool IsTailOn { get; set; }
        private bool IsHdOn { get; set; }
        private bool IsStopOn { get; set; }
        private bool IsFogOn { get; set; }
        private bool IsBulOn { get; set; }
        private bool IsLogoOn { get; set; }
        private bool IsWelcomeAnimationOn { get; set; }
        private bool IsGoodByeAnimationOn { get; set; }

        #region 测试用
        public string Read0X250Msg = string.Empty;
        public string Read0X260Msg = string.Empty;
        public string Read0X254Msg = string.Empty;
        public string Read0X533Msg = string.Empty;
        public string Read0X534Msg = string.Empty;
        public string Read0X535Msg = string.Empty;
        private bool IsReadMsg { get; set; }

        private bool Is52AOn { get; set; }
        private bool IsOtherMsgOn { get; set; }
        private bool IsNmPniOn { get; set; }
        private bool IsPcn18On { get; set; }
        #endregion

        private readonly object _lampLocker = new object();

        /// <summary>
        /// 0x20A 
        /// cyclic-50ms
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X20A =
            new CanCommunicationMatrix.MotorolaMatrix(0x20A, 8);

        /// <summary>
        /// 0x21A
        /// cyclic-20ms
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X21A =
            new CanCommunicationMatrix.MotorolaMatrix(0x21A, 8);

        /// <summary>
        /// 0x220
        /// cyclic-60ms
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X220 =
            new CanCommunicationMatrix.MotorolaMatrix(0x220, 8);

        /// <summary>
        /// 0x264
        /// cyclic-65ms
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X264 =
            new CanCommunicationMatrix.MotorolaMatrix(0x264, 8);

        /// <summary>
        /// 0x78
        /// cyclic-20ms
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X78 =
            new CanCommunicationMatrix.MotorolaMatrix(0x78, 8);

        /// <summary>
        /// 0x52A
        /// cyclic-50ms
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _motrMotorolaMatrix0X52A =
            new CanCommunicationMatrix.MotorolaMatrix(0x52A, 8);

        private void MainWork()
        {
            IsSleep = true;

            var sendCount = 0;
            var actnOfLedStopLampCntr = (byte)0x00;
            var actnOfIndcrIndcrOutCntr = (byte)0x00;

            var actvnOfIndcrIndcrOut = (byte)0x00;
            var indcrSts = (byte)0x00;
            var emgyBrkLiIndcrTurn = (byte)0x00;

            var lastTurnState = 0;
            var lastTurnKeepTime = 0;

            #region UBSET
            _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(39, 1, 1));
            _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(21, 1, 1));
            _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(41, 1, 1));
            _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(43, 1, 1));
            _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(55, 1, 1));
            _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(59, 1, 1));
            _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(61, 1, 1)); // eu 2023-01-12

            _motorolaMatrix0X21A.UpdateData(new MatrixValDefinition(38, 1, 1));
            _motorolaMatrix0X21A.UpdateData(new MatrixValDefinition(53, 1, 1));
            _motorolaMatrix0X21A.UpdateData(new MatrixValDefinition(50, 1, 1));

            _motorolaMatrix0X78.UpdateData(new MatrixValDefinition(39, 1, 1));
            _motorolaMatrix0X264.UpdateData(new MatrixValDefinition(2, 1, 1));
            _motorolaMatrix0X264.UpdateData(new MatrixValDefinition(46, 1, 1));

            //_motorolaMatrix0X220.UpdateData(new MatrixValDefinition(35, 1, 1));
            //_motorolaMatrix0X220.UpdateData(new MatrixValDefinition(39, 1, 1));
            //_motorolaMatrix0X220.UpdateData(new MatrixValDefinition(48, 1, 1));
            _motorolaMatrix0X220.UpdateData(new MatrixValDefinition(50, 1, 1));
            #endregion

            #region 52A_SET
            _motrMotorolaMatrix0X52A.MatrixData = new byte[] { 0x2A, 0x40, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            #endregion

            #region 主程序
            while (MainWorkThread.IsAlive)
            {
                if (!MainWorkThread.IsAlive)
                    break;

                if (Can == null)
                    continue;

                Thread.Sleep(10);

                lock (_lockError)
                {
                    var endTime = DateTime.Now;
                    if (ValueHelper.GetTimeSpanMs(_lastErrorDateTimeRcml, endTime) >= 2500)
                    {
                        StsOfLedPosnLampLe1 = string.Empty;
                        StsOfLedStopLampLe1 = string.Empty;
                        StsOfLedTurnIndcrLe1 = string.Empty;
                    }

                    if (ValueHelper.GetTimeSpanMs(_lastErrorDateTimeRcmr, endTime) >= 2500)
                    {
                        StsOfLedPosnLampRi1 = string.Empty;
                        StsOfLedStopLampRi1 = string.Empty;
                        StsOfLedTurnIndcrRi1 = string.Empty;
                    }

                    if (ValueHelper.GetTimeSpanMs(_lastErrorDateTimeRcmm, endTime) >= 2500)
                    {
                        StsOfLedPosnLampLe2 = string.Empty;
                        StsOfLedPosnLampRi2 = string.Empty;

                        StsOfLedStopLampLe2 = string.Empty;
                        StsOfLedStopLampRi2 = string.Empty;

                        StsOfLedTurnIndcrLe2 = string.Empty;
                        StsOfLedTurnIndcrRi2 = string.Empty;

                        StsOfLedRvsgLampLe2 = string.Empty;
                        StsOfLedRvsgLampLe2 = string.Empty;

                        StsOfLedReFogLampLe2 = string.Empty;
                        StsOfLedReFogLampRi2 = string.Empty;
                    }
                }

                lock (_lampLocker)
                {
                    if (IsSleep)
                    {
                        sendCount = 0;
                        actnOfLedStopLampCntr = 0x00;
                        actnOfIndcrIndcrOutCntr = 0x00;
                        lastTurnState = 0;
                        lastTurnKeepTime = 0;

                        actvnOfIndcrIndcrOut = 0x00;
                        indcrSts = 0x00;
                        emgyBrkLiIndcrTurn = 0x00;

                        TurnKeepTime = 0;
                        IsTurnHoldingOn = false;
                        IsTurnRunningOn = false;
                        IsTurnFlickerOn = false;
                        IsTailOn = false;
                        IsHdOn = false;
                        IsStopOn = false;
                        IsFogOn = false;
                        IsBulOn = false;
                        IsLogoOn = false;
                        IsWelcomeAnimationOn = false;
                        IsGoodByeAnimationOn = false;

                        _motorolaMatrix0X20A.MatrixData = new byte[8];
                        _motorolaMatrix0X21A.MatrixData = new byte[8];
                        _motorolaMatrix0X220.MatrixData = new byte[8];

                        #region UBSET
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(39, 1, 1));
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(21, 1, 1));
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(41, 1, 1));
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(43, 1, 1));
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(55, 1, 1));
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(59, 1, 1));
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(61, 1, 1)); // eu 2023-01-12

                        _motorolaMatrix0X21A.UpdateData(new MatrixValDefinition(38, 1, 1));
                        _motorolaMatrix0X21A.UpdateData(new MatrixValDefinition(53, 1, 1));
                        _motorolaMatrix0X21A.UpdateData(new MatrixValDefinition(50, 1, 1));

                        _motorolaMatrix0X78.UpdateData(new MatrixValDefinition(39, 1, 1));

                        _motorolaMatrix0X264.UpdateData(new MatrixValDefinition(2, 1, 1));
                        _motorolaMatrix0X264.UpdateData(new MatrixValDefinition(46, 1, 1));

                        _motorolaMatrix0X220.UpdateData(new MatrixValDefinition(50, 1, 1));
                        #endregion
                        continue;
                    }

                    sendCount++;

                    var sendPackage = new List<CanBus.CanDataPackage>();

                    if (sendCount * 10 % 20 == 0) // 20ms
                    {
                        // logo灯
                        _motorolaMatrix0X78.UpdateData(new MatrixValDefinition(7, 1, IsLogoOn ? (byte)1 : (byte)0));
                        _motorolaMatrix0X264.UpdateData(new MatrixValDefinition(3, 1, IsLogoOn ? (byte)1 : (byte)0));

                        #region turn
                        if (IsTurnRunningOn && !IsTurnFlickerOn && !IsTurnHoldingOn)
                        {
                            lastTurnKeepTime += 20;

                            if (sendCount * 10 % 400 == 0)
                            {
                                if (lastTurnState == 0)
                                {
                                    actvnOfIndcrIndcrOut = 0x03;
                                    indcrSts = 0x03;
                                    emgyBrkLiIndcrTurn = 0x01;
                                   lastTurnState = 1;
                                }
                                else
                                {
                                    if (lastTurnKeepTime >= TurnKeepTime)
                                    {
                                        lastTurnKeepTime = 0;
                                        actvnOfIndcrIndcrOut = 0x00;
                                        indcrSts = 0x00;
                                        emgyBrkLiIndcrTurn = 0x01;
                                        lastTurnState = 0;
                                    }
                                    else
                                    {
                                        actvnOfIndcrIndcrOut = 0x03;
                                        indcrSts = 0x03;
                                        emgyBrkLiIndcrTurn = 0x01;
                                    }
                                }
                            }
                        }
                        else if (!IsTurnRunningOn && IsTurnFlickerOn && !IsTurnHoldingOn)
                        {
                            lastTurnKeepTime += 20;

                            if (sendCount * 10 % 400 == 0)
                            {
                                if (lastTurnState == 0)
                                {
                                    actvnOfIndcrIndcrOut = 0x00;
                                    indcrSts = 0x00;
                                    emgyBrkLiIndcrTurn = 0x02;
                                    lastTurnState = 1;
                                }
                                else
                                {
                                    if (lastTurnKeepTime >= TurnKeepTime)
                                    {
                                        lastTurnKeepTime = 0;
                                        actvnOfIndcrIndcrOut = 0x00;
                                        indcrSts = 0x00;
                                        emgyBrkLiIndcrTurn = 0x01;
                                        lastTurnState = 0;
                                    }
                                    else
                                    {
                                        actvnOfIndcrIndcrOut = 0x03;
                                        indcrSts = 0x03;
                                        emgyBrkLiIndcrTurn = 0x01;
                                    }
                                }
                            }
                        }
                        else if (!IsTurnRunningOn && !IsTurnFlickerOn && IsTurnHoldingOn)
                        {
                            if (TurnKeepTime == 65535)
                            {
                              
                                if (sendCount * 10 % 400 == 0)
                                {
                                    if (lastTurnState == 0)
                                    {
                                        actvnOfIndcrIndcrOut = 0x00;
                                        indcrSts = 0x00;
                                        emgyBrkLiIndcrTurn = 0x02;
                                        lastTurnState = 1;
                                    }
                                    else
                                    {
                                        actvnOfIndcrIndcrOut = 0x03;
                                        indcrSts = 0x03;
                                        emgyBrkLiIndcrTurn = 0x02;
                                    }
                                }
                            }
                            //else if (TurnKeepTime == 65530)
                            //{
                            //    TurnKeepTime++;
                            //}
                            //else if (TurnKeepTime == 65534)
                            //{
                            //    actvnOfIndcrIndcrOut = 0x03;
                            //    indcrSts = 0x03;
                            //    emgyBrkLiIndcrTurn = 0x01;
                            //}
                            else
                            {
                                actvnOfIndcrIndcrOut = 0x03;
                                indcrSts = 0x03;
                                emgyBrkLiIndcrTurn = 0x01;
                                lastTurnState = 0;
                            }
                            //actvnOfIndcrIndcrOut = 0x03;
                            //indcrSts = 0x03;
                            //emgyBrkLiIndcrTurn = 0x01;
                        }
                        else if (!IsTurnRunningOn && !IsTurnFlickerOn && !IsTurnHoldingOn)
                        {
                            actvnOfIndcrIndcrOut = 0x00;
                            indcrSts = 0x00;
                            emgyBrkLiIndcrTurn = 0x00;
                            lastTurnState = 0;
                        }

                        _motorolaMatrix0X21A.UpdateData(new MatrixValDefinition(36, 2, actvnOfIndcrIndcrOut));
                        _motorolaMatrix0X21A.UpdateData(new MatrixValDefinition(51, 2, indcrSts));
                        _motorolaMatrix0X21A.UpdateData(new MatrixValDefinition(48, 2, emgyBrkLiIndcrTurn));

                        _motorolaMatrix0X21A.UpdateData(new MatrixValDefinition(32, 4, actnOfIndcrIndcrOutCntr));
                        
                        var actnOfIndcrIndcrOutChks = CalculateCrc8(158, actnOfIndcrIndcrOutCntr, actvnOfIndcrIndcrOut);
                        _motorolaMatrix0X21A.UpdateData(new MatrixValDefinition(40, 8, actnOfIndcrIndcrOutChks));

                        actnOfIndcrIndcrOutCntr = (byte)(actnOfIndcrIndcrOutCntr + 0x01);
                        if (actnOfIndcrIndcrOutCntr == 0x0F)
                            actnOfIndcrIndcrOutCntr = 0x00;
                        if (IsOtherMsgOn)
                        {
                            sendPackage.Add(new CanBus.CanDataPackage(_motorolaMatrix0X21A.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X21A.MatrixData));
                        }
                        #endregion

                        if (IsOtherMsgOn)
                        {
                            sendPackage.Add(new CanBus.CanDataPackage(_motorolaMatrix0X78.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X78.MatrixData));
                            sendPackage.Add(new CanBus.CanDataPackage(_motorolaMatrix0X264.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X264.MatrixData));
                        }
                    }
                    if (sendCount * 10 % 50 == 0) // 50ms
                    {
                        // stop 制动灯
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(20, 1, IsStopOn ? (byte)1 : (byte)0));
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(16, 4, actnOfLedStopLampCntr));
                        var actnOfLedStopLampChks = CalculateCrc8(1091, actnOfLedStopLampCntr, IsStopOn ? (byte)1 : (byte)0);
                        actnOfLedStopLampCntr = (byte)(actnOfLedStopLampCntr + 0x01);
                        if (actnOfLedStopLampCntr == 0x0F)
                            actnOfLedStopLampCntr = 0x00;
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(24, 8, actnOfLedStopLampChks));
                        // bul 倒车灯
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(42, 1, IsBulOn ? (byte)1 : (byte)0));
                        // fog 雾灯
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(40, 1, IsFogOn ? (byte)1 : (byte)0));
                        // welconme 回家动画
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(58, 1, IsWelcomeAnimationOn ? (byte)1 : (byte)0));
                        // goodbye 离家动画
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(54, 1, IsGoodByeAnimationOn ? (byte)1 : (byte)0));
                        // tail 位置灯
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(38, 1, IsTailOn ? (byte)1 : (byte)0));
                        _motorolaMatrix0X20A.UpdateData(new MatrixValDefinition(60, 1, IsTailOn ? (byte)1 : (byte)0)); // eu 2023-01-12

                        if (IsOtherMsgOn)
                        {
                            sendPackage.Add(new CanBus.CanDataPackage(_motorolaMatrix0X20A.CanId, CanBus.CanProtocol.Can,
                               CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X20A.MatrixData));
                        }

                        if (Is52AOn)
                        {
                            _motrMotorolaMatrix0X52A.UpdateData(new MatrixValDefinition(14, 1,
                                IsNmPniOn ? (byte)1 : (byte)0));
                            _motrMotorolaMatrix0X52A.UpdateData(new MatrixValDefinition(18, 1,
                                IsPcn18On ? (byte)1 : (byte)0));

                            sendPackage.Add(new CanBus.CanDataPackage(_motrMotorolaMatrix0X52A.CanId,
                                CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motrMotorolaMatrix0X52A.MatrixData));
                        }
                    }
                    if (sendCount * 10 % 60 == 0) // 60ms
                    {
                        if (IsHdOn)
                        {
                            _motorolaMatrix0X220.UpdateData(new MatrixValDefinition(51, 2, 1));
                            if (IsOtherMsgOn)
                            {
                                sendPackage.Add(new CanBus.CanDataPackage(_motorolaMatrix0X220.CanId,
                                    CanBus.CanProtocol.Can,
                                    CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X220.MatrixData));
                            }
                        }
                        else
                        {
                            _motorolaMatrix0X220.UpdateData(new MatrixValDefinition(51, 2, 2));
                            if (IsOtherMsgOn)
                            {
                                sendPackage.Add(new CanBus.CanDataPackage(_motorolaMatrix0X220.CanId,
                                    CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                    _motorolaMatrix0X220.MatrixData));
                            }
                        }
                    }

                    if (sendCount > 4000)
                        sendCount = 0;
                    Can.SendCanDatas(sendPackage.ToArray());
                }
            }
            #endregion
        }

        private static byte CalculateCrc8(
            int dataId, byte counter, byte lamp)
        {
            var crc = (byte)0x00;

            //  current CRC8 XOR data in
            crc ^= (byte)(dataId % 256);
            for (var j = 0; j < 8; j++)
            {
                // check if MSB is 1
                if ((crc & 0x80) > 0x0)
                {
                    crc <<= 0x01;
                    crc ^= 0x1D;
                }
                else
                {
                    crc <<= 0x01;
                }
            }

            crc ^= (byte)(dataId / 256);
            for (var j = 0; j < 8; j++)
            {
                // check if MSB is 1
                if ((crc & 0x80) > 0x0)
                {
                    crc <<= 0x01;
                    crc ^= 0x1D;
                }
                else
                {
                    crc <<= 0x01;
                }
            }

            crc ^= counter;
            for (var j = 0; j < 8; j++)
            {
                // check if MSB is 1
                if ((crc & 0x80) > 0x0)
                {
                    crc <<= 1;
                    crc ^= 0x1D;
                }
                else
                {
                    crc <<= 1;
                }
            }

            crc ^= lamp;
            for (var j = 0; j < 8; j++)
            {
                /*check if MSB is 1*/
                if ((crc & 0x80) > 0x0)
                {
                    crc <<= 1;
                    crc ^= 0x1D;
                }
                else
                {
                    crc <<= 1;
                }
            }

            crc ^= 0x00;

            return crc;
        }

        public void Set52AUp()
        {
            _motrMotorolaMatrix0X52A.MatrixData = new byte[] { 0x2A, 0x40, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
        }

        public void Set52AZero()
        {
            _motrMotorolaMatrix0X52A.MatrixData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }

        [Description("控制版唤醒")]
        public void LampAwake()
        {
            lock (_lampLocker)
            {
                //_motrMotorolaMatrix0X52A.MatrixData = new byte[] { 0x2A, 0x40, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                NmPniOn();
                Pnc18On();
                IsOtherMsgOn = true;
                Is52AOn = true;
                IsSleep = false;
            }
        }

        [Description("控制板休眠")]
        public void LampSleep()
        {
            lock (_lampLocker)
            {
                IsOtherMsgOn = false;
                Is52AOn = false;
                IsSleep = true;
            }
        }

        #region 测试用

        [Description("控制版唤醒且不发送52A")]
        public void LampAwakeWithout52A()
        {
            lock (_lampLocker)
            {
                //_motrMotorolaMatrix0X52A.MatrixData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                Is52AOn = false;
                IsOtherMsgOn = true;
                IsSleep = false;
            }
        }

        [Description("控制版唤醒且只发送52A")]
        public void LampAwakeJust52A()
        {
            lock (_lampLocker)
            {
                //NmPniOff();
                //Pnc18Off();
                //_motrMotorolaMatrix0X52A.MatrixData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                Is52AOn = true;
                IsOtherMsgOn = false;
                IsSleep = false;
            }
        }

        [Description("发送52A报文")]
        public void Msg52AOn()
        {
            lock (_lampLocker)
            {
                Is52AOn = true;
            }
        }

        [Description("停止52A报文")]
        public void Msg52AOff()
        {
            lock (_lampLocker)
            {
                Is52AOn = false;
            }
        }

        [Description("NM_PNI=1")]
        public void NmPniOn()
        {
            lock (_lampLocker)
            {
                IsNmPniOn = true;
            }
        }

        [Description("NM_PNI=0")]
        public void NmPniOff()
        {
            lock (_lampLocker)
            {
                IsNmPniOn = false;
            }
        }

        [Description("PNC18=1")]
        public void Pnc18On()
        {
            lock (_lampLocker)
            {
                IsPcn18On = true;
            }
        }

        [Description("PNC18=0")]
        public void Pnc18Off()
        {
            lock (_lampLocker)
            {
                IsPcn18On = false;
            }
        }

        public void StartReadMsg()
        {
            Read0X250Msg = "NULL";
            Read0X260Msg = "NULL";
            Read0X254Msg = "NULL";
            Read0X533Msg = "NULL";
            Read0X534Msg = "NULL";
            Read0X535Msg = "NULL";
            IsReadMsg = true;
        }

        public void StopReadMsg()
        {
            IsReadMsg = false;
        }

        #endregion

        #region STOP 制动灯
        /// <summary>
        /// 制动灯ON
        /// </summary>
        [Description("制动灯ON")]
        public void StopOn()
        {
            lock (_lampLocker)
            {
                IsStopOn = true;
            }
        }

        /// <summary>
        /// 制动灯OFF
        /// </summary>
        [Description("制动灯OFF")]
        public void StopOff()
        {
            lock (_lampLocker)
            {
                IsStopOn = false;
            }
        }
        #endregion

        #region TAIL 位置灯
        /// <summary>
        /// 位置灯ON
        /// </summary>
        [Description("位置灯ON")]
        public void TailOn()
        {
            TurnOff();

            lock (_lampLocker)
            {
                IsTailOn = true;
                IsHdOn = false;
            }
        }

        /// <summary>
        /// 位置灯高亮ON
        /// </summary>
        [Description("位置灯高亮ON")]
        public void TailHdOn()
        {
            TurnOff();

            lock (_lampLocker)
            {
                IsTailOn = true;
                IsHdOn = true;
            }
        }

        /// <summary>
        /// 位置灯OFF
        /// </summary>
        [Description("位置灯OFF")]
        public void TailOff()
        {
            TurnOff();

            lock (_lampLocker)
            {
                IsTailOn = false;
                IsHdOn = false;
            }
        }
        #endregion

        #region BUL 倒车灯
        /// <summary>
        /// 倒车灯ON
        /// </summary>
        [Description("倒车灯ON")]
        public void BulOn()
        {
            lock (_lampLocker)
            {
                IsBulOn = true;
            }
        }

        /// <summary>
        /// 倒车灯OFF
        /// </summary>
        [Description("倒车灯OFF")]
        public void BulOff()
        {
            lock (_lampLocker)
            {
                IsBulOn = false;
            }
        }
        #endregion

        #region FOG 雾灯
        /// <summary>
        /// 雾灯ON
        /// </summary>
        [Description("雾灯ON")]
        public void FogOn()
        {
            lock (_lampLocker)
            {
                IsFogOn = true;
            }
        }

        /// <summary>
        /// 雾灯OFF
        /// </summary>
        [Description("雾灯OFF")]
        public void FogOff()
        {
            lock (_lampLocker)
            {
                IsFogOn = false;
            }
        }
        #endregion

        #region LOGO

        [Description("LOGO灯ON")]
        public void LogoOn()
        {
            lock (_lampLocker)
            {
                IsLogoOn = true;
            }
        }

        [Description("LOGO灯OFF")]
        public void LogoOff()
        {
            lock (_lampLocker)
            {
                IsLogoOn = false;
            }
        }

        #endregion

        #region 转向灯-信号可区分左右-目前程序不区分左右

        /// <summary>
        /// 转向灯正常亮度时序点亮
        /// </summary>
        [Description("转向灯正常亮度时序点亮")]
        public void TurnRunningOn(string keepTime)
        {
            lock (_lampLocker)
            {
                try
                {
                    TurnKeepTime = ushort.Parse(keepTime);
                }
                catch (Exception)
                {
                    TurnKeepTime = 1000;
                }

                _motorolaMatrix0X264.UpdateData(new MatrixValDefinition(47, 1, 0x00));
                IsTurnHoldingOn = false;
                IsTurnRunningOn = true;
                IsTurnFlickerOn = false;
                IsHdOn = false;
            }
        }

        /// <summary>
        /// 转向灯正常亮度闪烁点亮
        /// </summary>
        [Description("转向灯正常亮度闪烁点亮")]
        public void TurnFlickerOn(string keepTime)
        {
            lock (_lampLocker)
            {
                try
                {
                    TurnKeepTime = ushort.Parse(keepTime);
                }
                catch (Exception)
                {
                    TurnKeepTime = 1000;
                }

                _motorolaMatrix0X264.UpdateData(new MatrixValDefinition(47, 1, 0x01));
                IsTurnHoldingOn = false;
                IsTurnRunningOn = false;
                IsTurnFlickerOn = true;
                IsHdOn = false;
            }
        }

        /// <summary>
        /// 转向灯时序高亮点亮
        /// </summary>
        [Description("转向灯时序高亮点亮")]
        public void TurnRunningHdOn(string keepTime)
        {
            lock (_lampLocker)
            {
                try
                {
                    TurnKeepTime = ushort.Parse(keepTime);
                }
                catch (Exception)
                {
                    TurnKeepTime = 1000;
                }

                _motorolaMatrix0X264.UpdateData(new MatrixValDefinition(47, 1, 0x00));
                IsTurnHoldingOn = false;
                IsTurnRunningOn = true;
                IsTurnFlickerOn = false;
                IsHdOn = true;
            }
        }

        /// <summary>
        /// 转向灯闪烁高亮点亮
        /// </summary>
        [Description("转向灯闪烁高亮点亮")]
        public void TurnFlickerHdOn(string keepTime)
        {
            lock (_lampLocker)
            {
                try
                {
                    TurnKeepTime = ushort.Parse(keepTime);
                }
                catch (Exception)
                {
                    TurnKeepTime = 1000;
                }

                _motorolaMatrix0X264.UpdateData(new MatrixValDefinition(47, 1, 0x01));
                IsTurnHoldingOn = false;
                IsTurnRunningOn = false;
                IsTurnFlickerOn = true;
                IsHdOn = true;
            }
        }

        /// <summary>
        /// 转向灯时序后常亮
        /// </summary>
        [Description("转向灯时序后常亮")]
        public void TurnHoldingOn()
        {
            lock (_lampLocker)
            {
                _motorolaMatrix0X264.UpdateData(new MatrixValDefinition(47, 1, 0x00));
                IsTurnHoldingOn = true;
                IsTurnRunningOn = false;
                IsTurnFlickerOn = false;
                IsHdOn = false;
            }
        }

        /// <summary>
        /// 转向灯闪烁后常亮
        /// </summary>
        [Description("转向灯闪烁后常亮")]
        public void TurnFickerHoldingOn()
        {
            lock (_lampLocker)
            {
                _motorolaMatrix0X264.UpdateData(new MatrixValDefinition(47, 1, 0x01));
                TurnKeepTime = 65535;
                IsTurnHoldingOn = true;
                IsTurnRunningOn = false;
                IsTurnFlickerOn = false;
                IsHdOn = true;
            }
        }

        /// <summary>
        /// 转向灯高亮常亮
        /// </summary>
        [Description("转向灯高亮常亮")]
        public void TurnHoldingHdOn()
        {
            lock (_lampLocker)
            {
                _motorolaMatrix0X264.UpdateData(new MatrixValDefinition(47, 1, 0x01));
                IsTurnHoldingOn = true;
                IsTurnRunningOn = false;
                IsTurnFlickerOn = false;
                IsHdOn = true;
            }
        }

        /// <summary>
        /// 转向灯关闭
        /// </summary>
        [Description("转向灯关闭")]
        public void TurnOff()
        {
            lock (_lampLocker)
            {
                TurnKeepTime = 0;
                IsTurnHoldingOn = false;
                IsTurnRunningOn = false;
                IsTurnFlickerOn = false;
                IsHdOn = false;
            }
        }
        #endregion

        #region 动画
        /// <summary>
        /// CHO
        /// 回家动画ON
        /// </summary>
        [Description("回家动画ON")]
        public void WelcomeAnimationOn()
        {
            TurnOff();
            TailOff();
            StopOff();
            FogOff();
            BulOff();
            LogoOff();

            lock (_lampLocker)
            {
                IsGoodByeAnimationOn = false;
                IsWelcomeAnimationOn = true;
            }
        }

        /// <summary>
        /// CHO
        /// 回家动画OFF
        /// </summary>
        [Description("回家动画OFF")]
        public void WelcomeAnimationOff()
        {
            TurnOff();
            TailOff();
            StopOff();
            FogOff();
            BulOff();
            LogoOff();

            lock (_lampLocker)
            {
                IsGoodByeAnimationOn = false;
                IsWelcomeAnimationOn = false;
            }
        }

        /// <summary>
        /// LHO
        /// 离家动画ON
        /// </summary>
        [Description("离家动画ON")]
        public void GoodByeAnimationOn()
        {
            TurnOff();
            TailOff();
            StopOff();
            FogOff();
            BulOff();

            lock (_lampLocker)
            {
                IsWelcomeAnimationOn = false;
                IsGoodByeAnimationOn = true;
            }
        }

        /// <summary>
        /// LHO
        /// 离家动画OFF
        /// </summary>
        [Description("离家动画OFF")]
        public void GoodByeAnimationOff()
        {
            TurnOff();
            TailOff();
            StopOff();
            FogOff();
            BulOff();

            lock (_lampLocker)
            {
                IsWelcomeAnimationOn = false;
                IsGoodByeAnimationOn = false;
            }
        }
        #endregion

        #region 版本信息

        private uint _requestCanId = 0x7b2;
        private uint _responseCanId = 0x6b2;

        [Description("R,EcuDeliveryAssemblyPartNumber")]
        public string EcuDeliveryAssemblyPartNumber = string.Empty;

        [Description("R,EcuCoreAssemblyPartNumber")]
        public string EcuCoreAssemblyPartNumber = string.Empty;

        [Description("R,PblPrimaryBootloaderSoftwarePartNumber")]
        public string PblPrimaryBootloaderSoftwarePartNumber = string.Empty;

        [Description("R,PrimaryBootloaderDiagnosticDatabasePartNumber")]
        public string PrimaryBootloaderDiagnosticDatabasePartNumber = string.Empty;

        [Description("R,LoadModule")]
        public string LoadModule = string.Empty;

        [Description("R,ApplicationDiagnosticDatabasePartNumber")]
        public string ApplicationDiagnosticDatabasePartNumber = string.Empty;

        [Description("R,HascoApplicationSoftwareVersionNumber")]
        public string HascoApplicationSoftwareVersionNumber = string.Empty;

        [Description("R,HascoPrimaryBootloaderSoftwareVersionNumber")]
        public string HascoPrimaryBootloaderSoftwareVersionNumber = string.Empty;

        [Description("R,HascoSecondaryBootloaderSoftwareVersionNumber")]
        public string HascoSecondaryBootloaderSoftwareVersionNumber = string.Empty;

        [Description("R,HascoApplicationSoftwarePartNumber")]
        public string HascoApplicationSoftwarePartNumber = string.Empty;

        [Description("R,HascoPrimaryBootloaderSoftwarePartNumber")]
        public string HascoPrimaryBootloaderSoftwarePartNumber = string.Empty;

        [Description("R,HascoSupplierSecondaryBootloaderSoftwarePartNumber")]
        public string HascoSupplierSecondaryBootloaderSoftwarePartNumber = string.Empty;

        [Description("R,HascoHardwareVersionNumber")]
        public string HascoHardwareVersionNumber = string.Empty;

        [Description("R,HascoHardwarePartNumber")]
        public string HascoHardwarePartNumber = string.Empty;

        [Description("Read EcuDeliveryAssemblyPartNumberOFF")]
        public void ReadEcuDeliveryAssemblyPartNumber()
        {
            EcuDeliveryAssemblyPartNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xAB)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read EcuCoreAssemblyPartNumber")]
        public void ReadEcuCoreAssemblyPartNumber()
        {
            EcuCoreAssemblyPartNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xAA)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read PblPrimaryBootloaderSoftwarePartNumber")]
        public void ReadPblPrimaryBootloaderSoftwarePartNumber()
        {
            PblPrimaryBootloaderSoftwarePartNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xA5)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read PrimaryBootloaderDiagnosticDatabasePartNumber")]
        public void ReadPrimaryBootloaderDiagnosticDatabasePartNumber()
        {
            PrimaryBootloaderDiagnosticDatabasePartNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xA1)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read LoadModule")]
        public void ReadLoadModule()
        {
            LoadModule =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xAE)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read ApplicationDiagnosticDatabasePartNumber")]
        public void ReadApplicationDiagnosticDatabasePartNumber()
        {
            ApplicationDiagnosticDatabasePartNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xA0)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read HascoApplicationSoftwareVersionNumber")]
        public void ReadHascoApplicationSoftwareVersionNumber()
        {
            HascoApplicationSoftwareVersionNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xF0)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read HascoPrimaryBootloaderSoftwareVersionNumber")]
        public void ReadHascoPrimaryBootloaderSoftwareVersionNumber()
        {
            HascoPrimaryBootloaderSoftwareVersionNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xF1)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read HascoSecondaryBootloaderSoftwareVersionNumber")]
        public void ReadHascoSecondaryBootloaderSoftwareVersionNumber()
        {
            HascoSecondaryBootloaderSoftwareVersionNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xF2)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read HascoApplicationSoftwarePartNumber")]
        public void ReadHascoApplicationSoftwarePartNumber()
        {
            HascoApplicationSoftwarePartNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xF3)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read HascoPrimaryBootloaderSoftwarePartNumber")]
        public void ReadHascoPrimaryBootloaderSoftwarePartNumber()
        {
            HascoPrimaryBootloaderSoftwarePartNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xF4)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read HascoSupplierSecondaryBootloaderSoftwarePartNumber")]
        public void ReadHascoSupplierSecondaryBootloaderSoftwarePartNumber()
        {
            HascoSupplierSecondaryBootloaderSoftwarePartNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xF5)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read HascoHardwareVersionNumber")]
        public void ReadHascoHardwareVersionNumber()
        {
            HascoHardwareVersionNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xF6)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read HascoHardwarePartNumber")]
        public void ReadHascoHardwarePartNumber()
        {
            HascoHardwarePartNumber =
                CanReadDid(_requestCanId, _responseCanId, 0xF1, 0xF7)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        private IEnumerable<byte> CanReadDid(
            uint requestCanId, uint responseCanId, byte didHi, byte didLo)
        {
            if (Can == null)
                return new byte[0];

            lock (_lampLocker)
            {
                Can.AddDoNotFilterCanId(responseCanId);

                byte[] readBytes;
                if (Can.CanBusWithUds.TryReadData(
                    requestCanId, responseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi,
                    didLo, out readBytes))
                {
                    Can.RemoveDoNotFilterCanId(responseCanId);
                    return readBytes ?? new byte[0];
                }

                Thread.Sleep(500);

                if (!Can.CanBusWithUds.TryReadData(
                    requestCanId, responseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi,
                    didLo, out readBytes))
                {
                    Can.RemoveDoNotFilterCanId(responseCanId);
                    return new byte[0];
                }

                Can.RemoveDoNotFilterCanId(responseCanId);
                return readBytes ?? new byte[0];
            }
        }

        [Description("R,诊断-读取DTC结果")]
        public string ReadDtcResult = string.Empty;
        [Description("R,清除DTC错误结果")]
        public string ClearFaultResult;

        [Description("读DTC")]
        public void ReadDtc()
        {
            byte[] echo;
            if (Can.CanBusWithUds.TryReadDtcInfomation(
                    _requestCanId, _responseCanId,
                    CanBus.CanType.Standard, 0x02, 0x09,
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

                            ReadDtcResult += string.Format("[{0}];", dtcData.Remark);
                        }

                        if (string.IsNullOrEmpty(ReadDtcResult))
                            ReadDtcResult = "NoError";
                    }
                    else
                        ReadDtcResult = "ReadDtcResLenError";
                }
            }
            else
                ReadDtcResult = "NoRead";
        }

        [Description("清除DTC错误")]
        public void ClearRFault()
        {
            ClearFaultResult = string.Empty;

            if (Can == null)
                return;

            if (Can.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(
                    _requestCanId, _responseCanId,
                    CanBus.CanType.Standard))
                ClearFaultResult = @"OK";
        }

        #endregion

        #region 切换灯种

        [Description("切换为B灯")]
        public void ChangeToRcmm()
        {
            ThisLampType = LampType.Rcmm;
            _requestCanId = 0x7b7;
            _responseCanId = 0x6b7;
        }

        [Description("切换为A灯(L)")]
        public void ChangeToRcml()
        {
            ThisLampType = LampType.Rcml;
            _requestCanId = 0x7b5;
            _responseCanId = 0x6b5;
        }

        [Description("切换为A灯(R)")]
        public void ChangeToRcmR()
        {
            ThisLampType = LampType.Rcmr;
            _requestCanId = 0x7b6;
            _responseCanId = 0x6b6;
        }

        private enum LampType
        {
            Rcmm,

            Rcml,

            Rcmr
        }

        #endregion
    }
}
