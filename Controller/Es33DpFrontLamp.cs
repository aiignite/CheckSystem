using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,ES33-DP前灯")]
    public sealed class Es33DpFrontLamp : ControllerBase
    {
        public LinBus Lin;

        public Es33DpFrontLamp(string name)
            : base(name)
        {
            _matrixValDefinitions.Add(DipdBeamLghtOnReq, new MatrixValDefinition(6, 1, 0));
            _matrixValDefinitions.Add(SideLghtOnReq, new MatrixValDefinition(2, 2, 0));
            _matrixValDefinitions.Add(MainBeamLghtOnReq, new MatrixValDefinition(5, 1, 0));
            _matrixValDefinitions.Add(AuxMainBeamLghtReq, new MatrixValDefinition(4, 1, 0));
            _matrixValDefinitions.Add(DircnLghtOnReq, new MatrixValDefinition(0, 2, 0));
            _matrixValDefinitions.Add(DayTimeRunningLghtOnReq, new MatrixValDefinition(7, 1, 0));
            _matrixValDefinitions.Add(FrtFogLghtOnReq, new MatrixValDefinition(8, 2, 0));
            _matrixValDefinitions.Add(WthrLghtOnReq, new MatrixValDefinition(10, 2, 0));
            _matrixValDefinitions.Add(RevsLghtOnReq, new MatrixValDefinition(12, 1, 0));
            _matrixValDefinitions.Add(BrkLghtOnReq, new MatrixValDefinition(13, 1, 0));
            _matrixValDefinitions.Add(BndngLghtOnReq, new MatrixValDefinition(14, 2, 0));
            _matrixValDefinitions.Add(RrFogLghtOnReq, new MatrixValDefinition(16, 1, 0));
            _matrixValDefinitions.Add(WlcmLghtReq, new MatrixValDefinition(17, 1, 0));
            _matrixValDefinitions.Add(PerfForWlcmLghtngMdSeln, new MatrixValDefinition(18, 4, 2));
            _matrixValDefinitions.Add(FrwlLghtReq, new MatrixValDefinition(22, 1, 0));
            _matrixValDefinitions.Add(PerfForFrwlLghtSeln, new MatrixValDefinition(24, 3, 0));
            _matrixValDefinitions.Add(LdspcOpenSts, new MatrixValDefinition(27, 2, 0));
            _matrixValDefinitions.Add(ExtrLghtFlwUpMdReq, new MatrixValDefinition(23, 1, 0));
            _matrixValDefinitions.Add(DimLvlReq, new MatrixValDefinition(32, 5, 1));
            _matrixValDefinitions.Add(DiSucsLghtEnb, new MatrixValDefinition(29, 1, 1));
            _matrixValDefinitions.Add(StaOfBmsPackForChrgng, new MatrixValDefinition(40, 4, 0));
            _matrixValDefinitions.Add(BntClsLghtReq, new MatrixValDefinition(30, 1, 0));
            _matrixValDefinitions.Add(HelloRMdLghtngLagMdSeln, new MatrixValDefinition(44, 4, 0));
            _matrixValDefinitions.Add(HelloRReqLghtngLagReq, new MatrixValDefinition(31, 1, 0));
            _matrixValDefinitions.Add(FdMCrMd, new MatrixValDefinition(37, 2, 0));
            _matrixValDefinitions.Add(FdMCrReq, new MatrixValDefinition(39, 1, 0));
            _matrixValDefinitions.Add(ChrgLghtReq, new MatrixValDefinition(48, 1, 0));
            _matrixValDefinitions.Add(DayTimeRunningLghtMd, new MatrixValDefinition(49, 4, 0));
            _matrixValDefinitions.Add(DisChrgLghtReq, new MatrixValDefinition(56, 1, 0));
            _matrixValDefinitions.Add(VehDrvngMd, new MatrixValDefinition(53, 3, 0));

            if (_mainWorkThread != null)
            {
                _mainWorkThread.Abort();
                _mainWorkThread.Join();
            }
            _mainWorkThread = new Thread(MainWork);
            _mainWorkThread.Start();
        }

        ~Es33DpFrontLamp()
        {
            Dispose();
        }

        private void MainWork()
        {
            _matrixValDefinitions[DircnLghtOnReq].Value = 0x03;

            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(50);

                if (Lin == null)
                    continue;

                if (!_isAwake)
                    continue;

                foreach (var key in _matrixValDefinitions.Keys)
                {
                    //var field = GetType().GetField(key);
                    //if (field != null)
                    //{
                    //    var value = field.GetValue(this);
                    //    if (value != null)
                    //    {
                    //        _matrixValDefinitions[key].Value = (byte)value;
                    //    }
                    //}

                    _motorolaMatrix.UpdateData(_matrixValDefinitions[key]);
                }
                Lin.SendMasterLin(
                    _motorolaMatrix.MasterLinId, _motorolaMatrix.MatrixData);
            }
        }

        [Description("唤醒")]
        public void LampAwake()
        {
            _isAwake = true;
        }

        [Description("休眠")]
        public void LampSleep()
        {
            _isAwake = false;
        }

        [Description("HighBeamReq")]
        public void HighBeamReq()
        {
            if (_matrixValDefinitions[MainBeamLghtOnReq].Value == 0x00)
                _matrixValDefinitions[MainBeamLghtOnReq].Value = 0x01;
            else if (_matrixValDefinitions[MainBeamLghtOnReq].Value == 0x01)
                _matrixValDefinitions[MainBeamLghtOnReq].Value = 0x00;

            if (_matrixValDefinitions[AuxMainBeamLghtReq].Value == 0x00)
                _matrixValDefinitions[AuxMainBeamLghtReq].Value = 0x01;
            else if (_matrixValDefinitions[AuxMainBeamLghtReq].Value == 0x01)
                _matrixValDefinitions[AuxMainBeamLghtReq].Value = 0x00;
        }

        [Description("AuxHighBeamReq")]
        public void AuxHighBeamReq()
        {
            _matrixValDefinitions[MainBeamLghtOnReq].Value = 0x00;

            if (_matrixValDefinitions[AuxMainBeamLghtReq].Value == 0x00)
                _matrixValDefinitions[AuxMainBeamLghtReq].Value = 0x01;
            else if (_matrixValDefinitions[AuxMainBeamLghtReq].Value == 0x01)
                _matrixValDefinitions[AuxMainBeamLghtReq].Value = 0x00;
        }

        [Description("DrlReq")]
        public void DrlReq()
        {
            if (_matrixValDefinitions[DayTimeRunningLghtOnReq].Value == 0x00)
                _matrixValDefinitions[DayTimeRunningLghtOnReq].Value = 0x01;
            else if (_matrixValDefinitions[DayTimeRunningLghtOnReq].Value == 0x01)
                _matrixValDefinitions[DayTimeRunningLghtOnReq].Value = 0x00;

            if (_matrixValDefinitions[ChrgLghtReq].Value == 0x00)
                _matrixValDefinitions[ChrgLghtReq].Value = 0x01;
            else if (_matrixValDefinitions[ChrgLghtReq].Value == 0x01)
                _matrixValDefinitions[ChrgLghtReq].Value = 0x00;
        }

        [Description("PlReq")]
        public void PlReq()
        {
            if (_matrixValDefinitions[SideLghtOnReq].Value == 0x00)
                _matrixValDefinitions[SideLghtOnReq].Value = 0x01;
            else if (_matrixValDefinitions[SideLghtOnReq].Value == 0x01)
                _matrixValDefinitions[SideLghtOnReq].Value = 0x00;
        }

        [Description("日间行车灯的点亮模式0")]
        public void DrlMode0()
        {
            _matrixValDefinitions[DayTimeRunningLghtMd].Value = 0x00;
        }

        [Description("日间行车灯的点亮模式1")]
        public void DrlMode1()
        {
            _matrixValDefinitions[DayTimeRunningLghtMd].Value = 0x01;
        }

        [Description("日间行车灯的点亮模式2")]
        public void DrlMode2()
        {
            _matrixValDefinitions[DayTimeRunningLghtMd].Value = 0x02;
        }

        [Description("日间行车灯的点亮模式3")]
        public void DrlMode3()
        {
            _matrixValDefinitions[DayTimeRunningLghtMd].Value = 0x03;
        }

        [Description("日间行车灯的点亮模式4")]
        public void DrlMode4()
        {
            _matrixValDefinitions[DayTimeRunningLghtMd].Value = 0x04;
        }

        [Description("日间行车灯的点亮模式5")]
        public void DrlMode5()
        {
            _matrixValDefinitions[DayTimeRunningLghtMd].Value = 0x05;
        }

        [Description("日间行车灯的点亮模式6")]
        public void DrlMode6()
        {
            _matrixValDefinitions[DayTimeRunningLghtMd].Value = 0x06;
        }

        [Description("DipdBeamLghtOnReq")]
        public void DipdBeamReq()
        {
            if (_matrixValDefinitions[DipdBeamLghtOnReq].Value == 0x00)
            {
                //_matrixValDefinitions[PerfForWlcmLghtngMdSeln].Value = 0x05;
                _matrixValDefinitions[DipdBeamLghtOnReq].Value = 0x01;
            }
            else if (_matrixValDefinitions[DipdBeamLghtOnReq].Value == 0x01)
            {
                //_matrixValDefinitions[PerfForWlcmLghtngMdSeln].Value = 0X00;
                _matrixValDefinitions[DipdBeamLghtOnReq].Value = 0x00;
            }
        }

        //private int baseIndex = 0;

        [Description("WlcmLight")]
        public void WlcmLightReq()
        {
            if (_matrixValDefinitions[WlcmLghtReq].Value == 0x00)
            {
                _matrixValDefinitions[PerfForWlcmLghtngMdSeln].Value = 0x05;
                Thread.Sleep(250);
                _matrixValDefinitions[WlcmLghtReq].Value = 0x01;
            }
            else if (_matrixValDefinitions[WlcmLghtReq].Value == 0x01)
            {
                _matrixValDefinitions[PerfForWlcmLghtngMdSeln].Value = 0X00;
                _matrixValDefinitions[WlcmLghtReq].Value = 0x00;
            }
            //if (_matrixValDefinitions[WlcmLghtReq].Value == 0x00)
            //    _matrixValDefinitions[WlcmLghtReq].Value = 0x01;
            //else if (_matrixValDefinitions[WlcmLghtReq].Value == 0x01)
            //    _matrixValDefinitions[WlcmLghtReq].Value = 0x00;
        }

        [Description("FrwlLghtReq")]
        public void FrwlLightReq()
        {
            //WlcmLghtReq
            if (_matrixValDefinitions[FrwlLghtReq].Value == 0x00)
            {
                _matrixValDefinitions[PerfForFrwlLghtSeln].Value = 0x07;
                Thread.Sleep(250);
                _matrixValDefinitions[FrwlLghtReq].Value = 0x01;
            }
            else if (_matrixValDefinitions[FrwlLghtReq].Value == 0x01)
            {
                _matrixValDefinitions[PerfForFrwlLghtSeln].Value = 0x00;
                _matrixValDefinitions[FrwlLghtReq].Value = 0x00;
            }
        }

        [Description("充电灯-3块单白光有动画")]
        public void ChrgLght()
        {
            //WlcmLghtReq
            if (_matrixValDefinitions[ChrgLghtReq].Value == 0x00)
            {
                //_matrixValDefinitions[PerfForFrwlLghtSeln].Value = 0x03;
                _matrixValDefinitions[ChrgLghtReq].Value = 0x01;
            }
            else if (_matrixValDefinitions[ChrgLghtReq].Value == 0x01)
            {
                //_matrixValDefinitions[PerfForFrwlLghtSeln].Value = 0x00;
                _matrixValDefinitions[ChrgLghtReq].Value = 0x00;
            }
        }

        [Description("测试")]
        public void DebugSingal()
        {
            //WlcmLghtReq
            if (_matrixValDefinitions[ChrgLghtReq].Value == 0x00)
            {
                //_matrixValDefinitions[PerfForFrwlLghtSeln].Value = 0x03;
                _matrixValDefinitions[ChrgLghtReq].Value = 0x01;
            }
            else if (_matrixValDefinitions[ChrgLghtReq].Value == 0x01)
            {
                //_matrixValDefinitions[PerfForFrwlLghtSeln].Value = 0x00;
                _matrixValDefinitions[ChrgLghtReq].Value = 0x00;
            }
        }

        private readonly Thread _mainWorkThread;
        private bool _isAwake;
        private readonly LinCommunicationMatrix.IntelMatrix _motorolaMatrix =
            new LinCommunicationMatrix.IntelMatrix(0x1A, 8);
        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
            new Dictionary<string, MatrixValDefinition>();

        private const string DipdBeamLghtOnReq = "DipdBeamLghtOnReq_l";
        private const string SideLghtOnReq = "SideLghtOnReq_l";
        private const string MainBeamLghtOnReq = "MainBeamLghtOnReq_l";
        private const string AuxMainBeamLghtReq = "AuxMainBeamLghtReq_l";
        private const string DircnLghtOnReq = "DircnLghtOnReq_l";
        private const string DayTimeRunningLghtOnReq = "DayTimeRunningLghtOnReq_l";
        private const string FrtFogLghtOnReq = "FrtFogLghtOnReq_l";
        private const string WthrLghtOnReq = "WthrLghtOnReq_l";
        private const string RevsLghtOnReq = "RevsLghtOnReq_l";
        private const string BrkLghtOnReq = "BrkLghtOnReq_l";
        private const string BndngLghtOnReq = "BndngLghtOnReq_l";
        private const string RrFogLghtOnReq = "RrFogLghtOnReq_l";
        private const string WlcmLghtReq = "WlcmLghtReq_l";
        private const string PerfForWlcmLghtngMdSeln = "PerfForWlcmLghtngMdSeln_l";
        private const string FrwlLghtReq = "	FrwlLghtReq_l";
        private const string PerfForFrwlLghtSeln = "PerfForFrwlLghtSeln_l";
        private const string LdspcOpenSts = "LdspcOpenSts_l	";
        private const string ExtrLghtFlwUpMdReq = "ExtrLghtFlwUpMdReq_l";
        private const string DimLvlReq = "DimLvlReq_l	";
        private const string DiSucsLghtEnb = "DISucsLghtEnb_l";
        private const string StaOfBmsPackForChrgng = "StaOfBMSPackForChrgng_l";
        private const string BntClsLghtReq = "BntClsLghtReq_l";
        private const string HelloRMdLghtngLagMdSeln = "HelloRMdLghtngLagMdSeln_l	";
        private const string HelloRReqLghtngLagReq = "HelloRReqLghtngLagReq_l";
        private const string FdMCrMd = "FdMCrMd_l";
        private const string FdMCrReq = "FdMCrReq_l";
        private const string ChrgLghtReq = "ChrgLghtReq_l";
        private const string DayTimeRunningLghtMd = "DayTimeRunningLghtMd_l";
        private const string DisChrgLghtReq = "DisChrgLghtReq_l";
        private const string VehDrvngMd = "VehDrvngMd_l";
    }
}
