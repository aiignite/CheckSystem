using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Controller
{
    [Description("LIN-Product,L234 Up_Level ECE/CCC")]
    public sealed class L234Mld : ControllerBase
    {
        public LinBus LinWithBaudRate19200;

        [Description("R/W,SOC动画同时触发")]
        public string BatSocDispAnmSyncTrgrd = "0";

        [Description("R/W,外部动画触发1命令")]
        public string ExtLgtAnmTrgr1Cmd = "0";

        [Description("R/W,外部动画触发2命令")]
        public string ExtLgtAnmTrgr2Cmd = "0";

        [Description("R/W,外部动画触发3命令")]
        public string ExtLgtAnmTrgr3Cmd = "0";

        [Description("R/W,外部动画触发4命令")]
        public string ExtLgtAnmTrgr4Cmd = "0";

        [Description("R/W,外部动画触发5命令")]
        public string ExtLgtAnmTrgr5Cmd = "0";

        [Description("R/W,外部动画触发6命令")]
        public string ExtLgtAnmTrgr6Cmd = "0";

        [Description("R/W,外部动画触发7命令")]
        public string ExtLgtAnmTrgr7Cmd = "0";

        [Description("R/W,外部动画触发8命令")]
        public string ExtLgtAnmTrgr8Cmd = "0";

        public L234Mld(string name)
            : base(name)
        {
            _funcMatrixValDefinitions.Add(DytmRnngLmpsLtCmndOn, new Signal(DytmRnngLmpsLtCmndOn));
            _funcMatrixValDefinitions.Add(DytmRnngLmpsRtCmndOn, new Signal(DytmRnngLmpsRtCmndOn));
            _funcMatrixValDefinitions.Add(DytmRnngLmpsAuxLtCmndOn, new Signal(DytmRnngLmpsAuxLtCmndOn));
            _funcMatrixValDefinitions.Add(DytmRnngLmpsAuxRtCmndOn, new Signal(DytmRnngLmpsAuxRtCmndOn));
            _funcMatrixValDefinitions.Add(EmblmLmpFrtCmndOn, new Signal(EmblmLmpFrtCmndOn));
            _funcMatrixValDefinitions.Add(PrkLmpLtCmndOn, new Signal(PrkLmpLtCmndOn));
            _funcMatrixValDefinitions.Add(PrkLmpRtCmndOn, new Signal(PrkLmpRtCmndOn));
            _funcMatrixValDefinitions.Add(PrkLmpAuxLtCmndOn, new Signal(PrkLmpAuxLtCmndOn));
            _funcMatrixValDefinitions.Add(PrkLmpAuxRtCmndOn, new Signal(PrkLmpAuxRtCmndOn));
            _funcMatrixValDefinitions.Add(SdMrkrLmpLtCmndOn, new Signal(SdMrkrLmpLtCmndOn));
            _funcMatrixValDefinitions.Add(SdMrkrLmpRtCmndOn, new Signal(SdMrkrLmpRtCmndOn));
            _funcMatrixValDefinitions.Add(LoBmLmpLtCmndOn, new Signal(LoBmLmpLtCmndOn));
            _funcMatrixValDefinitions.Add(LoBmLmpRtCmndOn, new Signal(LoBmLmpRtCmndOn));
            _funcMatrixValDefinitions.Add(HiBmLmpLtCmndOn, new Signal(HiBmLmpLtCmndOn));
            _funcMatrixValDefinitions.Add(HiBmLmpRtCmndOn, new Signal(HiBmLmpRtCmndOn));
            _funcMatrixValDefinitions.Add(TrnLmpLfCmndOn, new Signal(TrnLmpLfCmndOn));
            _funcMatrixValDefinitions.Add(TrnLmpRfCmndOn, new Signal(TrnLmpRfCmndOn));
            _funcMatrixValDefinitions.Add(TrnLmpSeqOpAllwd, new Signal(TrnLmpSeqOpAllwd));
            _funcMatrixValDefinitions.Add(FgLmpRrCmndOn, new Signal(FgLmpRrCmndOn));
            _funcMatrixValDefinitions.Add(RvrsLmpCmndOn, new Signal(RvrsLmpCmndOn));
            _funcMatrixValDefinitions.Add(TlLmpLtCmndOn, new Signal(TlLmpLtCmndOn));
            _funcMatrixValDefinitions.Add(TlLmpRtCmndOn, new Signal(TlLmpRtCmndOn));
            _funcMatrixValDefinitions.Add(TlLmpAuxLtCmndOn, new Signal(TlLmpAuxLtCmndOn));
            _funcMatrixValDefinitions.Add(TlLmpAuxRtCmndOn, new Signal(TlLmpAuxRtCmndOn));
            _funcMatrixValDefinitions.Add(TlLmpSecLtCmndOn, new Signal(TlLmpSecLtCmndOn));
            _funcMatrixValDefinitions.Add(TlLmpSecRtCmndOn, new Signal(TlLmpSecRtCmndOn));
            _funcMatrixValDefinitions.Add(BrkLmpLtCmndOn, new Signal(BrkLmpLtCmndOn));
            _funcMatrixValDefinitions.Add(BrkLmpRtCmndOn, new Signal(BrkLmpRtCmndOn));
            _funcMatrixValDefinitions.Add(BrkLmpSecLtCmndOn, new Signal(BrkLmpSecLtCmndOn));
            _funcMatrixValDefinitions.Add(BrkLmpSecRtCmndOn, new Signal(BrkLmpSecRtCmndOn));
            _funcMatrixValDefinitions.Add(TrnLmpLrCmndOn, new Signal(TrnLmpLrCmndOn));
            _funcMatrixValDefinitions.Add(TrnLmpRrCmndOn, new Signal(TrnLmpRrCmndOn));
            _funcMatrixValDefinitions.Add(TrnLmpSecLrCmndOn, new Signal(TrnLmpSecLrCmndOn));
            _funcMatrixValDefinitions.Add(TrnLmpSecRrCmndOn, new Signal(TrnLmpSecRrCmndOn));

            _animationMatrixValDefinitions.Add(BatSocDispVal, new Signal(BatSocDispVal));
            _animationMatrixValDefinitions.Add(BatSocDispSts, new Signal(BatSocDispSts));

            _animationMatrixValDefinitions.Add("BatSocDispAnmSyncTrgrd", new Signal("BatSocDispAnmSyncTrgrd,7,1"));

            _animationMatrixValDefinitions.Add("ExtLgtAnmTrgr1Cmd", new Signal("ExtLgtAnmTrgr1Cmd,11,5"));
            _animationMatrixValDefinitions.Add("ExtLgtAnmTrgr2Cmd", new Signal("ExtLgtAnmTrgr2Cmd,16,5"));
            _animationMatrixValDefinitions.Add("ExtLgtAnmTrgr3Cmd", new Signal("ExtLgtAnmTrgr3Cmd,21,5"));
            _animationMatrixValDefinitions.Add("ExtLgtAnmTrgr4Cmd", new Signal("ExtLgtAnmTrgr4Cmd,26,5"));
            _animationMatrixValDefinitions.Add("ExtLgtAnmTrgr5Cmd", new Signal("ExtLgtAnmTrgr5Cmd,31,5"));
            _animationMatrixValDefinitions.Add("ExtLgtAnmTrgr6Cmd", new Signal("ExtLgtAnmTrgr6Cmd,36,5"));
            _animationMatrixValDefinitions.Add("ExtLgtAnmTrgr7Cmd", new Signal("ExtLgtAnmTrgr7Cmd,41,5"));
            _animationMatrixValDefinitions.Add("ExtLgtAnmTrgr8Cmd", new Signal("ExtLgtAnmTrgr8Cmd,46,5"));

            //SetTimer(LinNetWork(), 10);

            SetTimer(new MyTaskScheduler.TaskInfo { Action = LinNetWork(), Interval = 10 });
            SchedulerAsync();
        }

        ~L234Mld()
        {
            Dispose();
        }

        private readonly object _linLocker = new object();
        private bool _isAwake = false;
        private int _periodCount;

        private readonly LinCommunicationMatrix.IntelMatrix _functionCmd =
            new LinCommunicationMatrix.IntelMatrix(0x39, 8);
        private readonly LinCommunicationMatrix.IntelMatrix _animationCmd =
            new LinCommunicationMatrix.IntelMatrix(0x38, 8);
        private readonly Dictionary<string, Signal> _funcMatrixValDefinitions =
            new Dictionary<string, Signal>();
        private readonly Dictionary<string, Signal> _animationMatrixValDefinitions =
            new Dictionary<string, Signal>();

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
                    {
                        var keys = _funcMatrixValDefinitions.Keys.ToList();
                        foreach (var key in keys)
                            _functionCmd.UpdateData(_funcMatrixValDefinitions[key].MatrixValDefinition);

                        LinWithBaudRate19200.SendMasterLin(_functionCmd.MasterLinId, _functionCmd.MatrixData);
                        _periodCount++;
                    }
                }

                if (_periodCount == 2)
                {
                    {
                        UpdateTrgr();
                        var keys = _animationMatrixValDefinitions.Keys.ToList();
                        foreach (var key in keys)
                            _animationCmd.UpdateData(_animationMatrixValDefinitions[key].MatrixValDefinition);

                        LinWithBaudRate19200.SendMasterLin(_animationCmd.MasterLinId, _animationCmd.MatrixData);
                        _periodCount = 0;
                    }
                }
            };
        }

        #region main control

        [Description("MLD休眠")]
        public void MldSleep()
        {
            _isAwake = false;
        }

        [Description("MLD唤醒")]
        public void MldAwake()
        {
            _isAwake = true;
        }

        [Description("DRL开")]
        public void DrlOn()
        {
            _funcMatrixValDefinitions[DytmRnngLmpsLtCmndOn].MatrixValDefinition.Value = 0x01;
            _funcMatrixValDefinitions[DytmRnngLmpsRtCmndOn].MatrixValDefinition.Value = 0x01;
            //_funcMatrixValDefinitions[DytmRnngLmpsAuxLtCmndOn].MatrixValDefinition.Value = 0x01;
            //_funcMatrixValDefinitions[DytmRnngLmpsAuxRtCmndOn].MatrixValDefinition.Value = 0x01;
        }

        [Description("DRL关")]
        public void DrlOff()
        {
            _funcMatrixValDefinitions[DytmRnngLmpsLtCmndOn].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[DytmRnngLmpsRtCmndOn].MatrixValDefinition.Value = 0x00;
            //_funcMatrixValDefinitions[DytmRnngLmpsAuxLtCmndOn].MatrixValDefinition.Value = 0x00;
            //_funcMatrixValDefinitions[DytmRnngLmpsAuxRtCmndOn].MatrixValDefinition.Value = 0x00;
        }

        [Description("辅助DRL开")]
        public void AuxDrlOn()
        {
            //_funcMatrixValDefinitions[DytmRnngLmpsLtCmndOn].MatrixValDefinition.Value = 0x01;
            //_funcMatrixValDefinitions[DytmRnngLmpsRtCmndOn].MatrixValDefinition.Value = 0x01;
            _funcMatrixValDefinitions[DytmRnngLmpsAuxLtCmndOn].MatrixValDefinition.Value = 0x01;
            _funcMatrixValDefinitions[DytmRnngLmpsAuxRtCmndOn].MatrixValDefinition.Value = 0x01;
        }

        [Description("辅助DRL关")]
        public void AuxDrlOff()
        {
            //_funcMatrixValDefinitions[DytmRnngLmpsLtCmndOn].MatrixValDefinition.Value = 0x00;
            //_funcMatrixValDefinitions[DytmRnngLmpsRtCmndOn].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[DytmRnngLmpsAuxLtCmndOn].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[DytmRnngLmpsAuxRtCmndOn].MatrixValDefinition.Value = 0x00;
        }

        [Description("车标灯开")]
        public void EmblmOn()
        {
            _funcMatrixValDefinitions[EmblmLmpFrtCmndOn].MatrixValDefinition.Value = 0x01;
        }

        [Description("车标灯关")]
        public void EmblmOff()
        {
            _funcMatrixValDefinitions[EmblmLmpFrtCmndOn].MatrixValDefinition.Value = 0x00;
        }

        [Description("PL开")]
        public void PlOn()
        {
            _funcMatrixValDefinitions[PrkLmpLtCmndOn].MatrixValDefinition.Value = 0x01;
            _funcMatrixValDefinitions[PrkLmpRtCmndOn].MatrixValDefinition.Value = 0x01;
            //_funcMatrixValDefinitions[PrkLmpAuxLtCmndOn].MatrixValDefinition.Value = 0x01;
            //_funcMatrixValDefinitions[PrkLmpAuxRtCmndOn].MatrixValDefinition.Value = 0x01;
        }

        [Description("PL关")]
        public void PlOff()
        {
            _funcMatrixValDefinitions[PrkLmpLtCmndOn].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[PrkLmpRtCmndOn].MatrixValDefinition.Value = 0x00;
            //_funcMatrixValDefinitions[PrkLmpAuxLtCmndOn].MatrixValDefinition.Value = 0x00;
            //_funcMatrixValDefinitions[PrkLmpAuxRtCmndOn].MatrixValDefinition.Value = 0x00;
        }

        [Description("辅助PL开")]
        public void AuxPlOn()
        {
            //_funcMatrixValDefinitions[PrkLmpLtCmndOn].MatrixValDefinition.Value = 0x01;
            //_funcMatrixValDefinitions[PrkLmpRtCmndOn].MatrixValDefinition.Value = 0x01;
            _funcMatrixValDefinitions[PrkLmpAuxLtCmndOn].MatrixValDefinition.Value = 0x01;
            _funcMatrixValDefinitions[PrkLmpAuxRtCmndOn].MatrixValDefinition.Value = 0x01;
        }

        [Description("辅助PL关")]
        public void AuxPlOff()
        {
            //_funcMatrixValDefinitions[PrkLmpLtCmndOn].MatrixValDefinition.Value = 0x00;
            //_funcMatrixValDefinitions[PrkLmpRtCmndOn].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[PrkLmpAuxLtCmndOn].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[PrkLmpAuxRtCmndOn].MatrixValDefinition.Value = 0x00;
        }

        [Description("示廓灯开")]
        public void SdMrkrOn()
        {
            _funcMatrixValDefinitions[SdMrkrLmpLtCmndOn].MatrixValDefinition.Value = 0x01;
            _funcMatrixValDefinitions[SdMrkrLmpRtCmndOn].MatrixValDefinition.Value = 0x01;
        }

        [Description("示廓灯关")]
        public void SdMrkrOff()
        {
            _funcMatrixValDefinitions[SdMrkrLmpLtCmndOn].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[SdMrkrLmpRtCmndOn].MatrixValDefinition.Value = 0x00;
        }

        [Description("近光开")]
        public void LbOn()
        {
            _funcMatrixValDefinitions[LoBmLmpLtCmndOn].MatrixValDefinition.Value = 0x01;
            _funcMatrixValDefinitions[LoBmLmpRtCmndOn].MatrixValDefinition.Value = 0x01;
        }

        [Description("近光关")]
        public void LbOff()
        {
            _funcMatrixValDefinitions[LoBmLmpLtCmndOn].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[LoBmLmpRtCmndOn].MatrixValDefinition.Value = 0x00;
        }

        [Description("远光开")]
        public void HbOn()
        {
            _funcMatrixValDefinitions[HiBmLmpLtCmndOn].MatrixValDefinition.Value = 0x01;
            _funcMatrixValDefinitions[HiBmLmpRtCmndOn].MatrixValDefinition.Value = 0x01;
        }

        [Description("远光关")]
        public void HbOff()
        {
            _funcMatrixValDefinitions[HiBmLmpLtCmndOn].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[HiBmLmpRtCmndOn].MatrixValDefinition.Value = 0x00;
        }

        [Description("转向灯开")]
        public void TurnOn()
        {
            //_funcMatrixValDefinitions[TrnLmpSeqOpAllwd].MatrixValDefinition.Value = 0x01;
            _funcMatrixValDefinitions[TrnLmpLfCmndOn].MatrixValDefinition.Value = 0x01;
            _funcMatrixValDefinitions[TrnLmpRfCmndOn].MatrixValDefinition.Value = 0x01;

        }

        [Description("转向灯关")]
        public void TurnOff()
        {
            //_funcMatrixValDefinitions[TrnLmpSeqOpAllwd].MatrixValDefinition.Value = 0x01;
            _funcMatrixValDefinitions[TrnLmpLfCmndOn].MatrixValDefinition.Value = 0x00;
            _funcMatrixValDefinitions[TrnLmpRfCmndOn].MatrixValDefinition.Value = 0x00;
        }

        [Description("转向灯时序使能开")]
        public void TurnSeqOn()
        {
            _funcMatrixValDefinitions[TrnLmpSeqOpAllwd].MatrixValDefinition.Value = 0x01;
        }

        [Description("转向灯时序使能关")]
        public void TurnSeqOff()
        {
            _funcMatrixValDefinitions[TrnLmpSeqOpAllwd].MatrixValDefinition.Value = 0x00;
        }

        [Description("充电显示值")]
        public void BatSocDisplay(string value)
        {
            byte per;
            if (!byte.TryParse(value, out per))
                return;
            if (per > 0 && per < 100)
                _animationMatrixValDefinitions[BatSocDispVal].MatrixValDefinition.Value = per;
        }

        [Description("充电显示状态-Off")]
        public void BatSocDisplay_Off()
        {
            _animationMatrixValDefinitions[BatSocDispSts].MatrixValDefinition.Value = 0x00;
        }

        [Description("充电显示状态-Charging")]
        public void BatSocDisplay_Charging()
        {
            _animationMatrixValDefinitions[BatSocDispSts].MatrixValDefinition.Value = 0x01;
        }

        [Description("充电显示状态-Delayed_Charging")]
        public void BatSocDisplay_Delayed_Charging()
        {
            _animationMatrixValDefinitions[BatSocDispSts].MatrixValDefinition.Value = 0x02;
        }

        [Description("充电显示状态-Charge_Complete")]
        public void BatSocDisplay_Charge_Complete()
        {
            _animationMatrixValDefinitions[BatSocDispSts].MatrixValDefinition.Value = 0x03;
        }

        [Description("充电显示状态-Display_Disabled")]
        public void BatSocDisplay_Display_Disabled()
        {
            _animationMatrixValDefinitions[BatSocDispSts].MatrixValDefinition.Value = 0x04;
        }

        private void UpdateTrgr()
        {
            if (BatSocDispAnmSyncTrgrd == 0.ToString())
            {
                _animationMatrixValDefinitions["BatSocDispAnmSyncTrgrd"].MatrixValDefinition.Value = 0x00;
            }
            else
            {
                _animationMatrixValDefinitions["BatSocDispAnmSyncTrgrd"].MatrixValDefinition.Value = 0x01;
            }

            for (var i = 1; i < 9; i++)
            {
                var str = string.Format("ExtLgtAnmTrgr{0}Cmd", i);
                var field = GetType().GetField(str);
                if (field != null)
                {
                    var fieldValue = field.GetValue(this);
                    if (fieldValue == null)
                    {
                        _animationMatrixValDefinitions[str].MatrixValDefinition.Value = 0x00;
                    }
                    else
                    {
                        if (fieldValue.ToString() == 0.ToString())
                        {
                            _animationMatrixValDefinitions[str].MatrixValDefinition.Value = 0x00;
                        }
                        else
                        {
                            int value;
                            if (int.TryParse(fieldValue.ToString(), out value))
                            {
                                if (value >= 1 && value <= 31)
                                {
                                    _animationMatrixValDefinitions[str].MatrixValDefinition.Value = (byte)value;
                                }
                                else
                                {
                                    _animationMatrixValDefinitions[str].MatrixValDefinition.Value = 0x00;
                                }
                            }
                            else
                            {
                                _animationMatrixValDefinitions[str].MatrixValDefinition.Value = 0x00;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region const strings

        private const string DytmRnngLmpsLtCmndOn = "DytmRnngLmpsLtCmndOn,0,1";
        private const string DytmRnngLmpsRtCmndOn = "DytmRnngLmpsRtCmndOn,1,1";
        private const string DytmRnngLmpsAuxLtCmndOn = "DytmRnngLmpsAuxLtCmndOn,2,1";
        private const string DytmRnngLmpsAuxRtCmndOn = "DytmRnngLmpsAuxRtCmndOn,3,1";
        private const string EmblmLmpFrtCmndOn = "EmblmLmpFrtCmndOn,4,1";
        private const string PrkLmpLtCmndOn = "PrkLmpLtCmndOn,5,1";
        private const string PrkLmpRtCmndOn = "PrkLmpRtCmndOn,6,1";
        private const string PrkLmpAuxLtCmndOn = "PrkLmpAuxLtCmndOn,7,1";
        private const string PrkLmpAuxRtCmndOn = "PrkLmpAuxRtCmndOn,8,1";
        private const string SdMrkrLmpLtCmndOn = "SdMrkrLmpLtCmndOn,9,1";
        private const string SdMrkrLmpRtCmndOn = "SdMrkrLmpRtCmndOn,10,1";
        private const string LoBmLmpLtCmndOn = "LoBmLmpLtCmndOn,11,1";
        private const string LoBmLmpRtCmndOn = "LoBmLmpRtCmndOn,12,1";
        private const string HiBmLmpLtCmndOn = "HiBmLmpLtCmndOn,13,1";
        private const string HiBmLmpRtCmndOn = "HiBmLmpRtCmndOn,14,1";
        private const string TrnLmpLfCmndOn = "TrnLmpLFCmndOn,15,1";
        private const string TrnLmpRfCmndOn = "TrnLmpRFCmndOn,16,1";
        private const string TrnLmpSeqOpAllwd = "TrnLmpSeqOpAllwd,17,1";
        private const string FgLmpRrCmndOn = "FgLmpRrCmndOn,18,1";
        private const string RvrsLmpCmndOn = "RvrsLmpCmndOn,19,1";
        private const string TlLmpLtCmndOn = "TlLmpLtCmndOn,20,1";
        private const string TlLmpRtCmndOn = "TlLmpRtCmndOn,21,1";
        private const string TlLmpAuxLtCmndOn = "TlLmpAuxLtCmndOn,22,1";
        private const string TlLmpAuxRtCmndOn = "TlLmpAuxRtCmndOn,23,1";
        private const string TlLmpSecLtCmndOn = "TlLmpSecLtCmndOn,24,1";
        private const string TlLmpSecRtCmndOn = "TlLmpSecRtCmndOn,25,1";
        private const string BrkLmpLtCmndOn = "BrkLmpLtCmndOn,26,1";
        private const string BrkLmpRtCmndOn = "BrkLmpRtCmndOn,27,1";
        private const string BrkLmpSecLtCmndOn = "BrkLmpSecLtCmndOn,28,1";
        private const string BrkLmpSecRtCmndOn = "BrkLmpSecRtCmndOn,29,1";
        private const string TrnLmpLrCmndOn = "TrnLmpLRCmndOn,30,1";
        private const string TrnLmpRrCmndOn = "TrnLmpRRCmndOn,31,1";
        private const string TrnLmpSecLrCmndOn = "TrnLmpSecLRCmndOn,32,1";
        private const string TrnLmpSecRrCmndOn = "TrnLmpSecRRCmndOn,33,1";

        private const string BatSocDispVal = "BatSOCDispVal,0,7";
        private const string BatSocDispSts = "BatSOCDispSts,8,3";

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

        #endregion
    }
}
