using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Timers;

namespace Controller
{
    [Description("LIN-Product,S59前灯")]
    public sealed class S59HeadLamp : ControllerBase
    {
        public LinBus LinWithBaudRate19200;

        [Description("R,近光是否打开")]
        public bool IsLbOn;
        [Description("R,远光是否打开")]
        public bool IsHbOn;
        [Description("R,转向是否打开")]
        public bool IsTurnOn;
        [Description("R,转向闪烁是否打开")]
        public bool IsTurnFlash;
        [Description("R,DRL是否打开")]
        public bool IsDrlOn;
        [Description("R,PL是否打开")]
        public bool IsPlOn;
        [Description("R,当前模式")]
        public string CurrentLightMode = LightMode.Normal.ToString();

        [Description("R,当前动画")]
        public string CurrentAnimation = string.Empty;
        [Description("R,当前动画剩余/S")]
        public string CurrentAnimationDuration = string.Empty;

        private int _turnFlashCount;
        private int _modeCount;
        private readonly System.Timers.Timer _modeTimer;
        private int _intCount;

        public S59HeadLamp(string name) : base(name)
        {
            _funcMatrixValDefinitions.Add(VIUL_FLPLSt, new Signal(VIUL_FLPLSt));
            _funcMatrixValDefinitions.Add(VIUL_FLGLPLSt, new Signal(VIUL_FLGLPLSt));
            _funcMatrixValDefinitions.Add(VIUL_FLGLDRLSt, new Signal(VIUL_FLGLDRLSt));
            _funcMatrixValDefinitions.Add(VIUL_RLPLOpenSt, new Signal(VIUL_RLPLOpenSt));
            _funcMatrixValDefinitions.Add(VIUL_TISt, new Signal(VIUL_TISt));
            _funcMatrixValDefinitions.Add(VIUL_LowBeamSt, new Signal(VIUL_LowBeamSt));
            _funcMatrixValDefinitions.Add(VIUL_HighBeamSt, new Signal(VIUL_HighBeamSt));
            _funcMatrixValDefinitions.Add(VIUL_DayTimeRunningLamp, new Signal(VIUL_DayTimeRunningLamp));
            _funcMatrixValDefinitions.Add(VIUL_StopSt, new Signal(VIUL_StopSt));
            _funcMatrixValDefinitions.Add(VIUL_ReverseSt, new Signal(VIUL_ReverseSt));
            _funcMatrixValDefinitions.Add(VIUL_FGLLPLSt, new Signal(VIUL_FGLLPLSt));
            _funcMatrixValDefinitions.Add(VIUL_FGLLDRLSt, new Signal(VIUL_FGLLDRLSt));
            _funcMatrixValDefinitions.Add(VIUL_CILRSt, new Signal(VIUL_CILRSt));
            _funcMatrixValDefinitions.Add(VIUL_UnLockWelcomeLampReq, new Signal(VIUL_UnLockWelcomeLampReq));
            _funcMatrixValDefinitions.Add(VIUL_LockWelcomeLampReq, new Signal(VIUL_LockWelcomeLampReq));
            _funcMatrixValDefinitions.Add(VIUL_ChargingReq, new Signal(VIUL_ChargingReq));
            _funcMatrixValDefinitions.Add(VIUL_SOCReq, new Signal(VIUL_SOCReq));
            _funcMatrixValDefinitions.Add(VIUL_ErrorReq, new Signal(VIUL_ErrorReq));
            _funcMatrixValDefinitions.Add(VIUL_CampingReq, new Signal(VIUL_CampingReq));
            _funcMatrixValDefinitions.Add(VIUL_MusicLampReq, new Signal(VIUL_MusicLampReq));
            _funcMatrixValDefinitions.Add(VIUL_SayhiLampModeSetReq, new Signal(VIUL_SayhiLampModeSetReq));
            _funcMatrixValDefinitions.Add(VIUL_ExhibitReq, new Signal(VIUL_ExhibitReq));

            SetTimer(new MyTaskScheduler.TaskInfo { Action = LinNetWork(), Interval = 25 });
            SchedulerAsync();

            _modeTimer = new System.Timers.Timer();
            _modeTimer.Elapsed += _modeTimer_Elapsed;
            _modeTimer.Interval = 100;
            _modeTimer.AutoReset = true;
            _modeTimer.Enabled = false;
            _modeTimer.Start();
        }

        ~S59HeadLamp()
        {
            Dispose();
        }

        private Action LinNetWork()
        {
            return () =>
            {
                if (LinWithBaudRate19200 == null)
                {
                    _periodCount = 0;
                    return;
                }

                if (!_isAwake)
                {
                    _periodCount = 0;
                    return;
                }

                lock (_linLocker)
                {
                    var mode = EnumOperater.GetEnumByValue<LightMode>(CurrentLightMode);
                    switch (mode)
                    {
                        case LightMode.Normal:
                            _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = IsLbOn ? (byte)0x01 : (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = IsHbOn ? (byte)0x01 : (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = IsDrlOn ? (byte)0x01 : (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = IsPlOn ? (byte)0x01 : (byte)0x00; ;

                            if (!IsTurnFlash)
                            {
                                _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = IsTurnOn ? (byte)0x03 : (byte)0x00;
                            }
                            else
                            {
                                TurnLogic();
                            }
                            break;

                        case LightMode.Mode1:
                            _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x01;
                            _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x01;
                            _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x01;

                            TurnLogic();
                            break;

                        case LightMode.Mode2:
                            if (_modeCount == 0)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x01;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 1)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x02;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 2)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x01;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 3)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x02;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 4)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x03;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 5)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x04;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 6)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x05;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            break;

                        case LightMode.Mode3:
                            _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x01;
                            _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                            break;

                        case LightMode.Mode4:
                            _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                            break;

                        case LightMode.Mode5:
                            _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                            break;

                        case LightMode.Mode6:
                            if (_modeCount == 0)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x01;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 1)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x01;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            break;

                        case LightMode.Mode7:
                            if (_modeCount == 0)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x02;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 1)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x02;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            break;

                        case LightMode.Mode8:
                            if (_modeCount == 0)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x03;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 1)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x04;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 2)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x05;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            break;

                        case LightMode.Mode9:
                            _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x01;
                            _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x01;
                            _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x01;
                            _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                            break;

                        case LightMode.Mode10:
                            _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x01;
                            _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                            break;

                        case LightMode.Mode11:
                            _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x00;
                            TurnLogic();
                            break;

                        case LightMode.Mode12:
                            _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x01;
                            _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x01;
                            _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                            break;

                        case LightMode.Mode13:
                            _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x01;
                            _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x01;
                            _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                            _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x01;
                            _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                            break;

                        case LightMode.Mode14:
                            if (_modeCount == 0)
                            {
                                _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x01;
                                _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                            }
                            if (_modeCount == 1)
                            {
                                _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x01;
                                _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x01;
                                _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                            }
                            if (_modeCount == 2)
                            {
                                _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x01;
                                _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x01;
                                _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x01;
                                _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                            }
                            if (_modeCount == 3)
                            {
                                _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x01;
                                _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x01;
                                _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x01;
                                TurnLogic();
                            }
                            if (_modeCount == 4)
                            {
                                _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x01;
                                _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                            }
                            if (_modeCount == 5)
                            {
                                _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x00;
                                _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                            }
                            else if (_modeCount == 6)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x01;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 7)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x02;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 8)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x01;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 9)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x02;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 10)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x03;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 11)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x04;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            else if (_modeCount == 12)
                            {
                                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x05;
                                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    {
                        var keys = _funcMatrixValDefinitions.Keys.ToList();
                        foreach (var key in keys)
                            _viulReq1.UpdateData(_funcMatrixValDefinitions[key].MatrixValDefinition);

                        LinWithBaudRate19200.SendMasterLin(_viulReq1.MasterLinId, _viulReq1.MatrixData);
                        _periodCount++;

                        if (_periodCount == (((60 * 60) * 1000) / 10))
                            _periodCount = 0;
                    }
                }
            };
        }

        [Description("R/W,TurnFlashMaxCount")]
        public int TurnFlashMaxCount = 10;

        private void TurnLogic()
        {
            var temp = TurnFlashMaxCount;
            if (temp < 5 || temp > 20)
            {
                temp = 10;
            }

            if (_turnFlashCount >= 0 && _turnFlashCount <= temp)
            {
                _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x03;
                _turnFlashCount++;
            }
            else if (_turnFlashCount > temp)
            {
                _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                _turnFlashCount++;

                if (_turnFlashCount >= temp * 2)
                    _turnFlashCount = 0;
            }
        }

        private void _modeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var mode = EnumOperater.GetEnumByValue<LightMode>(CurrentLightMode);

            switch (mode)
            {
                case LightMode.Normal:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode1:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode2:
                    _intCount++;
                    if (_modeCount == 0)
                    {
                        CurrentAnimation = @"迎宾1(8s)";

                        if (_intCount >= (8 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 1)
                    {
                        CurrentAnimation = @"迎宾2(6.4s)";

                        if (_intCount >= (6.4 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 2)
                    {
                        CurrentAnimation = @"闭锁1(2.88s)";

                        if (_intCount >= (2.88 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 3)
                    {
                        CurrentAnimation = @"闭锁2(3s)";

                        if (_intCount >= (3 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 4)
                    {
                        CurrentAnimation = @"SayHi1(61.48s)";

                        if (_intCount >= (61.48 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 5)
                    {
                        CurrentAnimation = @"SayHi2(62.2s)";

                        if (_intCount >= (62.2 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 6)
                    {
                        CurrentAnimation = @"SayHi3(61.48s)";

                        if (_intCount >= (61.48 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount = 0;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    break;

                case LightMode.Mode3:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode4:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode5:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode6:
                    _intCount++;
                    if (_modeCount == 0)
                    {
                        CurrentAnimation = @"迎宾1(8s)";

                        if (_intCount >= (8 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 1)
                    {
                        CurrentAnimation = @"闭锁1(2.88s)";

                        if (_intCount >= (2.88 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount = 0;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    break;

                case LightMode.Mode7:
                    _intCount++;
                    if (_modeCount == 0)
                    {
                        CurrentAnimation = @"迎宾2(6.4s)";

                        if (_intCount >= (6.4 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 1)
                    {
                        CurrentAnimation = @"闭锁2(3s)";

                        if (_intCount >= (3 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount = 0;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    break;

                case LightMode.Mode8:
                    _intCount++;
                    if (_modeCount == 0)
                    {
                        CurrentAnimation = @"SayHi1(61.48s)";

                        if (_intCount >= (61.48 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 1)
                    {
                        CurrentAnimation = @"SayHi2(62.2s)";

                        if (_intCount >= (62.2 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 2)
                    {
                        CurrentAnimation = @"SayHi3(61.48s)";

                        if (_intCount >= (61.48 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount = 0;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    break;

                case LightMode.Mode9:
                    _intCount = 0;
                    return;

                case LightMode.Mode10:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode11:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode12:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode13:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode14:
                    _intCount++;
                    if (_modeCount == 0)
                    {
                        CurrentAnimation = @"PL(10s)";

                        if (_intCount >= (10 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 1)
                    {
                        CurrentAnimation = @"LB&PL(10s)";

                        if (_intCount >= (10 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 2)
                    {
                        CurrentAnimation = @"LB&HB&PL(10s)";

                        if (_intCount >= (10 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 3)
                    {
                        CurrentAnimation = @"LB&HB&PL&TURN(10s)";

                        if (_intCount >= (10 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 4)
                    {
                        CurrentAnimation = @"DRL(10s)";

                        if (_intCount >= (10 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 5)
                    {
                        CurrentAnimation = @"AllOff(1s)";

                        if (_intCount >= (1 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    if (_modeCount == 6)
                    {
                        CurrentAnimation = @"迎宾1(8s)";

                        if (_intCount >= (8 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 7)
                    {
                        CurrentAnimation = @"迎宾2(6.4s)";

                        if (_intCount >= (6.4 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 8)
                    {
                        CurrentAnimation = @"闭锁1(2.88s)";

                        if (_intCount >= (2.88 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 9)
                    {
                        CurrentAnimation = @"闭锁2(3s)";

                        if (_intCount >= (3 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 10)
                    {
                        CurrentAnimation = @"SayHi1(61.48s)";

                        if (_intCount >= (61.48 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 11)
                    {
                        CurrentAnimation = @"SayHi2(62.2s)";

                        if (_intCount >= (62.2 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 12)
                    {
                        CurrentAnimation = @"SayHi3(61.48s)";

                        if (_intCount >= (61.48 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount = 0;
                            _intCount = 0;
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    break;

                default:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;
            }
        }

        [Description("休眠")]
        public void Sleep()
        {
            _isAwake = false;
        }

        [Description("唤醒")]
        public void Awake()
        {
            _isAwake = true;
        }

        [Description("近光开")]
        public void LbOn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsLbOn = true;
        }

        [Description("近光关")]
        public void LbOff()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsLbOn = false;
        }

        [Description("远光开")]
        public void HbOn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsHbOn = true;
        }

        [Description("远光关")]
        public void HbOff()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsHbOn = false;
        }

        [Description("日行灯开")]
        public void DrlOn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsDrlOn = true;
        }

        [Description("日行灯关")]
        public void DrlOff()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsDrlOn = false;
        }

        [Description("位置灯开")]
        public void PlOn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsPlOn = true;
        }

        [Description("位置灯关")]
        public void PlOff()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsPlOn = false;
        }

        [Description("转向灯开")]
        public void TurnOn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsTurnOn = true;
            IsTurnFlash = false;
        }

        [Description("转向灯闪烁开")]
        public void TurnFlashOn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsTurnOn = true;
            IsTurnFlash = true;
            _turnFlashCount = 0;
        }

        [Description("转向灯关")]
        public void TurnOff()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsTurnOn = false;
            IsTurnFlash = false;
            _turnFlashCount = 0;
        }

        [Description("解锁动态迎宾请求1")]
        public void UnLockWelcomeLampReq1()
        {
            _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x01;
        }

        [Description("解锁动态迎宾请求2")]
        public void UnLockWelcomeLampReq2()
        {
            _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x02;
        }

        [Description("解锁动态迎宾请求3")]
        public void UnLockWelcomeLampReq3()
        {
            _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x03;
        }

        [Description("解锁动态迎宾请求4")]
        public void UnLockWelcomeLampReq4()
        {
            _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x04;
        }

        [Description("解锁动态迎宾请求5")]
        public void UnLockWelcomeLampReq5()
        {
            _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x05;
        }

        [Description("闭锁动态迎宾请求1")]
        public void LockWelcomeLampReq1()
        {
            _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x01;
        }

        [Description("闭锁动态迎宾请求2")]
        public void LockWelcomeLampReq2()
        {
            _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x02;
        }

        [Description("关闭迎宾请求")]
        public void ResetWelcomLampReq()
        {
            _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
        }

        [Description("打开特定模式")]
        public void StartSpecialMode(string index)
        {
            int value;
            if (!string.IsNullOrEmpty(index) && int.TryParse(index, out value) && value >= 1 && value <= 14)
            {
                var modeStr = "Mode" + index;
                var mode = EnumOperater.GetEnumByValue<LightMode>(modeStr);

                IsLbOn = false;
                IsHbOn = false;
                IsTurnFlash = false;
                IsTurnOn = false;
                IsDrlOn = false;
                IsPlOn = false;
                _turnFlashCount = 0;
                _modeCount = 0;

                _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x00;
                _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x00;
                _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
                _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x00;
                _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
                _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
                _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;

                CurrentLightMode = mode.ToString();
            }
        }

        [Description("关闭特定模式")]
        public void StopSpecialMode()
        {
            IsLbOn = false;
            IsHbOn = false;
            IsTurnFlash = false;
            IsTurnOn = false;
            IsDrlOn = false;
            IsPlOn = false;
            _turnFlashCount = 0;
            _modeCount = 0;

            _funcMatrixValDefinitions[VIUL_LowBeamSt].MatrixValDefinition.Value = (byte)0x00;
            _funcMatrixValDefinitions[VIUL_HighBeamSt].MatrixValDefinition.Value = (byte)0x00;
            _funcMatrixValDefinitions[VIUL_DayTimeRunningLamp].MatrixValDefinition.Value = (byte)0x00;
            _funcMatrixValDefinitions[VIUL_FLPLSt].MatrixValDefinition.Value = (byte)0x00;
            _funcMatrixValDefinitions[VIUL_TISt].MatrixValDefinition.Value = (byte)0x00;
            _funcMatrixValDefinitions[VIUL_UnLockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[VIUL_LockWelcomeLampReq].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[VIUL_SayhiLampModeSetReq].MatrixValDefinition.Value = 0x00;

            CurrentLightMode = LightMode.Normal.ToString();
        }

        private readonly object _linLocker = new object();
        private bool _isAwake = false;
        private int _periodCount;
        private readonly Dictionary<string, Signal> _funcMatrixValDefinitions =
            new Dictionary<string, Signal>();

        private readonly LinCommunicationMatrix.IntelMatrix _viulReq1 =
            new LinCommunicationMatrix.IntelMatrix(0x13, 8);

        #region const strings

        private const string VIUL_FLPLSt = "VIUL_FLPLSt,0,1";
        private const string VIUL_FLGLPLSt = "VIUL_FLGLPLSt,1,1";
        private const string VIUL_FLGLDRLSt = "VIUL_FLGLDRLSt,2,1";
        private const string VIUL_RLPLOpenSt = "VIUL_RLPLOpenSt,3,1";
        private const string VIUL_TISt = "VIUL_TISt,4,2";
        private const string VIUL_LowBeamSt = "VIUL_LowBeamSt,6,1";
        private const string VIUL_HighBeamSt = "VIUL_HighBeamSt,7,1";
        private const string VIUL_DayTimeRunningLamp = "VIUL_DayTimeRunningLamp,8,1";
        private const string VIUL_StopSt = "VIUL_StopSt,9,1";
        private const string VIUL_ReverseSt = "VIUL_ReverseSt,10,1";
        private const string VIUL_FGLLPLSt = "VIUL_FGLLPLSt,11,1";
        private const string VIUL_FGLLDRLSt = "VIUL_FGLLDRLSt,12,1";
        private const string VIUL_CILRSt = "VIUL_CILRSt,13,1";
        private const string VIUL_UnLockWelcomeLampReq = "VIUL_UnLockWelcomeLampReq,14,3";
        private const string VIUL_LockWelcomeLampReq = "VIUL_LockWelcomeLampReq,17,3";
        private const string VIUL_ChargingReq = "VIUL_ChargingReq,20,5";
        private const string VIUL_SOCReq = "VIUL_SOCReq,25,4";
        private const string VIUL_ErrorReq = "VIUL_ErrorReq,29,4";
        private const string VIUL_CampingReq = "VIUL_CampingReq,33,3";
        private const string VIUL_MusicLampReq = "VIUL_MusicLampReq,36,4";
        private const string VIUL_SayhiLampModeSetReq = "VIUL_SayhiLampModeSetReq,40,1";
        private const string VIUL_ExhibitReq = "VIUL_ExhibitReq,41,1";

        internal class Signal
        {
            public int StartBit;
            public int Length;
            public string Name;
            public MatrixValDefinition MatrixValDefinition;

            public Signal(string initValue)
            {
                var sp = initValue.Split(',');
                Name = sp[0];
                StartBit = int.Parse(sp[1]);
                Length = int.Parse(sp[2]);
                MatrixValDefinition = new MatrixValDefinition(StartBit, Length, 0);
            }
        }

        internal enum LightMode
        {
            Normal,

            Mode1,

            Mode2,

            Mode3,

            Mode4,

            Mode5,

            Mode6,

            Mode7,

            Mode8,

            Mode9,

            Mode10,

            Mode11,

            Mode12,

            Mode13,

            Mode14
        }

        #endregion
    }
}
