using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,P12L低配前灯")]
    public sealed class P12LLowLevelHeadLamp : ControllerBase
    {
        public LinBus BcmLin5;
        public LinBus IccLin6;

        [Description("R,转向灯流水使能")]
        public bool DiSucsLghtEnb;

        public P12LLowLevelHeadLamp(string name)
            : base(name)
        {
            _matrixValDefinitions.Add(AuxMainBeamLghtReq_lL5_BCM_LIN5, new MatrixValDefinition(9, 1, 0));
            _matrixValDefinitions.Add(BndngLghtOnReq_lL5_BCM_LIN5, new MatrixValDefinition(19, 2, 0));
            _matrixValDefinitions.Add(BntClsLghtReq_lL5_BCM_LIN5, new MatrixValDefinition(39, 1, 0));
            _matrixValDefinitions.Add(BrkLghtOnReq_lL5_BCM_LIN5, new MatrixValDefinition(18, 1, 0));
            _matrixValDefinitions.Add(ChrgLghtReq_lL5_BCM_LIN5, new MatrixValDefinition(62, 1, 0));
            _matrixValDefinitions.Add(DayTimeRunningLghtAndSideLghtMd_lL5_BCM_LIN5, new MatrixValDefinition(58, 4, 0));
            _matrixValDefinitions.Add(DayTimeRunningLghtOnReq_lL5_BCM_LIN5, new MatrixValDefinition(12, 1, 0));
            _matrixValDefinitions.Add(DimLvlReq_lL5_BCM_LIN5, new MatrixValDefinition(40, 5, 0));
            _matrixValDefinitions.Add(DipdBeamLghtOnReq_lL5_BCM_LIN5, new MatrixValDefinition(3, 1, 0));
            _matrixValDefinitions.Add(DircnLghtOnReq_lL5_BCM_LIN5, new MatrixValDefinition(10, 2, 0));
            _matrixValDefinitions.Add(DISucsLghtEnb_lL5_BCM_LIN5, new MatrixValDefinition(38, 1, 0));
            _matrixValDefinitions.Add(ExtrLghtFlwUpMdReq_lL5_BCM_LIN5, new MatrixValDefinition(36, 1, 0));
            _matrixValDefinitions.Add(FrtFogLghtOnReq_lL5_BCM_LIN5, new MatrixValDefinition(13, 2, 0));
            _matrixValDefinitions.Add(FrwlLghtReq_lL5_BCM_LIN5, new MatrixValDefinition(23, 1, 0));
            _matrixValDefinitions.Add(LdspcClsLghtReq_lL5_BCM_LIN5, new MatrixValDefinition(31, 1, 0));
            _matrixValDefinitions.Add(LdspcOpenSts_lL5_BCM_LIN5, new MatrixValDefinition(56, 2, 0));
            _matrixValDefinitions.Add(LghtngShwMdReq_lL5_BCM_LIN5, new MatrixValDefinition(32, 1, 0));
            _matrixValDefinitions.Add(MainBeamLghtOnReq_lL5_BCM_LIN5, new MatrixValDefinition(8, 1, 0));
            _matrixValDefinitions.Add(PerfForFrwlLghtSeln_lL5_BCM_LIN5, new MatrixValDefinition(28, 3, 0));
            _matrixValDefinitions.Add(PerfForLghtngShwMdSeln_lL5_BCM_LIN5, new MatrixValDefinition(33, 3, 0));
            _matrixValDefinitions.Add(PerfForWlcmLghtngMdSeln_lL5_BCM_LIN5, new MatrixValDefinition(24, 4, 0));
            _matrixValDefinitions.Add(RevsLghtOnReq_lL5_BCM_LIN5, new MatrixValDefinition(15, 1, 0));
            _matrixValDefinitions.Add(RrFogLghtOnReq_lL5_BCM_LIN5, new MatrixValDefinition(21, 1, 0));
            _matrixValDefinitions.Add(SideLghtOnReq_lL5_BCM_LIN5, new MatrixValDefinition(4, 2, 0));
            _matrixValDefinitions.Add(StaOfBMSPackForChrgng_lL5_BCM_LIN5, new MatrixValDefinition(48, 4, 0));
            _matrixValDefinitions.Add(WlcmLghtReq_lL5_BCM_LIN5, new MatrixValDefinition(22, 1, 0));
            _matrixValDefinitions.Add(WthrLghtOnReq_lL5_BCM_LIN5, new MatrixValDefinition(16, 2, 0));

            //_matrixValDefinitions[MainBeamLghtOnReq_lL5_BCM_LIN5].Value = 0x01;
            //_matrixValDefinitions[DISucsLghtEnb_lL5_BCM_LIN5].Value = 0x01;
            //_matrixValDefinitions[DipdBeamLghtOnReq_lL5_BCM_LIN5].Value = 0x01;

            if (_mainWorkThread != null)
            {
                _mainWorkThread.Abort();
                _mainWorkThread.Join();
            }
            _mainWorkThread = new Thread(MainWork);
            _mainWorkThread.Start();

            if (_motorWorkThread != null)
            {
                _motorWorkThread.Abort();
                _motorWorkThread.Join();
            }
            _motorWorkThread = new Thread(MotorWork);
            _motorWorkThread.Start();
        }

        ~P12LLowLevelHeadLamp()
        {
            Dispose();
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

        #region LED Control

        [Description("HighBeamReq")]
        public void HighBeamReq()
        {
            if (_matrixValDefinitions[MainBeamLghtOnReq_lL5_BCM_LIN5].Value == 0x00)
                _matrixValDefinitions[MainBeamLghtOnReq_lL5_BCM_LIN5].Value = 0x01;
            else if (_matrixValDefinitions[MainBeamLghtOnReq_lL5_BCM_LIN5].Value == 0x01)
                _matrixValDefinitions[MainBeamLghtOnReq_lL5_BCM_LIN5].Value = 0x00;
        }

        [Description("LowBeamReq")]
        public void LowBeamReq()
        {
            if (_matrixValDefinitions[DipdBeamLghtOnReq_lL5_BCM_LIN5].Value == 0x00)
                _matrixValDefinitions[DipdBeamLghtOnReq_lL5_BCM_LIN5].Value = 0x01;
            else if (_matrixValDefinitions[DipdBeamLghtOnReq_lL5_BCM_LIN5].Value == 0x01)
                _matrixValDefinitions[DipdBeamLghtOnReq_lL5_BCM_LIN5].Value = 0x00;
        }

        [Description("DrlReq")]
        public void DrlReq()
        {
            if (_matrixValDefinitions[DayTimeRunningLghtOnReq_lL5_BCM_LIN5].Value == 0x00)
            {
                _matrixValDefinitions[DayTimeRunningLghtOnReq_lL5_BCM_LIN5].Value = 0x01;
            }
            else if (_matrixValDefinitions[DayTimeRunningLghtOnReq_lL5_BCM_LIN5].Value == 0x01)
            {
                _matrixValDefinitions[DayTimeRunningLghtOnReq_lL5_BCM_LIN5].Value = 0x00;
            }
        }

        [Description("PlReq")]
        public void PlReq(string value)
        {
            byte dimLvlReq;
            if (byte.TryParse(value, out dimLvlReq))
            {
                if (dimLvlReq <= 31)
                {
                    if (dimLvlReq == 0)
                    {
                        _matrixValDefinitions[DimLvlReq_lL5_BCM_LIN5].Value = 0x00;
                        _matrixValDefinitions[SideLghtOnReq_lL5_BCM_LIN5].Value = 0x00;
                    }
                    else
                    {
                        _matrixValDefinitions[DimLvlReq_lL5_BCM_LIN5].Value = dimLvlReq;
                        _matrixValDefinitions[SideLghtOnReq_lL5_BCM_LIN5].Value = 0x03;
                    }
                }
            }

            //if (_matrixValDefinitions[SideLghtOnReq_lL5_BCM_LIN5].Value == 0x00)
            //{
            //    _matrixValDefinitions[DimLvlReq_lL5_BCM_LIN5].Value = 0x1f;
            //    _matrixValDefinitions[SideLghtOnReq_lL5_BCM_LIN5].Value = 0x03;
            //}
            //else if (_matrixValDefinitions[SideLghtOnReq_lL5_BCM_LIN5].Value == 0x03)
            //{
            //    _matrixValDefinitions[DimLvlReq_lL5_BCM_LIN5].Value = 0x00;
            //    _matrixValDefinitions[SideLghtOnReq_lL5_BCM_LIN5].Value = 0x00;
            //}
        }

        [Description("开关Turn")]
        public void Turn()
        {
            if (_matrixValDefinitions[DircnLghtOnReq_lL5_BCM_LIN5].Value == 0x00)
                _matrixValDefinitions[DircnLghtOnReq_lL5_BCM_LIN5].Value = 0x03;
            else if (_matrixValDefinitions[DircnLghtOnReq_lL5_BCM_LIN5].Value == 0x03)
                _matrixValDefinitions[DircnLghtOnReq_lL5_BCM_LIN5].Value = 0x00;
        }

        //[Description("日间行车灯的点亮模式0")]
        //public void DrlMode1()
        //{
        //    _matrixValDefinitions[DayTimeRunningLghtAndSideLghtMd_lL5_BCM_LIN5].Value = 0x00;
        //}

        //[Description("日间行车灯的点亮模式1")]
        //public void DrlMode2()
        //{
        //    _matrixValDefinitions[DayTimeRunningLghtAndSideLghtMd_lL5_BCM_LIN5].Value = 0x01;
        //}

        //[Description("日间行车灯的点亮模式2")]
        //public void DrlMode3()
        //{
        //    _matrixValDefinitions[DayTimeRunningLghtAndSideLghtMd_lL5_BCM_LIN5].Value = 0x02;
        //}

        //[Description("日间行车灯的点亮模式3")]
        //public void DrlMode4()
        //{
        //    _matrixValDefinitions[DayTimeRunningLghtAndSideLghtMd_lL5_BCM_LIN5].Value = 0x03;
        //}

        //[Description("日间行车灯的点亮模式4")]
        //public void DrlMode5()
        //{
        //    _matrixValDefinitions[DayTimeRunningLghtAndSideLghtMd_lL5_BCM_LIN5].Value = 0x04;
        //}

        //[Description("日间行车灯的点亮模式5")]
        //public void DrlMode6()
        //{
        //    _matrixValDefinitions[DayTimeRunningLghtAndSideLghtMd_lL5_BCM_LIN5].Value = 0x05;
        //}

        //[Description("日间行车灯的点亮模式6")]
        //public void Dm7()
        //{
        //    _matrixValDefinitions[DayTimeRunningLghtAndSideLghtMd_lL5_BCM_LIN5].Value = 0x06;
        //}

        [Description("Welcome Light Request迎宾请求")]
        public void WlcReq()
        {
            if (_matrixValDefinitions[WlcmLghtReq_lL5_BCM_LIN5].Value == 0x00)
            {
                //_matrixValDefinitions[DircnLghtOnReq_lL5_BCM_LIN5].Value = 0x03;
                //Thread.Sleep(50);

                _matrixValDefinitions[WlcmLghtReq_lL5_BCM_LIN5].Value = 0x01;
                //Thread.Sleep(50);
                //_matrixValDefinitions[DircnLghtOnReq_lL5_BCM_LIN5].Value = 0x00;
            }
            else if (_matrixValDefinitions[WlcmLghtReq_lL5_BCM_LIN5].Value == 0x01)
            {
                _matrixValDefinitions[WlcmLghtReq_lL5_BCM_LIN5].Value = 0x00;
                //_matrixValDefinitions[DircnLghtOnReq_lL5_BCM_LIN5].Value = 0x00;
            }
        }

        [Description("迎宾模式关")]
        public void WlcMode1()
        {
            _matrixValDefinitions[PerfForWlcmLghtngMdSeln_lL5_BCM_LIN5].Value = 0x00;
        }

        [Description("迎宾模式开")]
        public void WlcMode2()
        {
            _matrixValDefinitions[PerfForWlcmLghtngMdSeln_lL5_BCM_LIN5].Value = 0x01;
        }

        //[Description("充电灯")]
        //public void Chg()
        //{
        //    if (_matrixValDefinitions[ChrgLghtReq_lL5_BCM_LIN5].Value == 0x00)
        //        _matrixValDefinitions[ChrgLghtReq_lL5_BCM_LIN5].Value = 0x01;
        //    else if (_matrixValDefinitions[ChrgLghtReq_lL5_BCM_LIN5].Value == 0x01)
        //        _matrixValDefinitions[ChrgLghtReq_lL5_BCM_LIN5].Value = 0x00;
        //}

        [Description("Farewell Light Request欢送请求")]
        public void Fwr()
        {
            if (_matrixValDefinitions[FrwlLghtReq_lL5_BCM_LIN5].Value == 0x00)
            {
                //_matrixValDefinitions[DircnLghtOnReq_lL5_BCM_LIN5].Value = 0x03;
                //Thread.Sleep(50);

                _matrixValDefinitions[FrwlLghtReq_lL5_BCM_LIN5].Value = 0x01;
                //Thread.Sleep(50);
                //_matrixValDefinitions[DircnLghtOnReq_lL5_BCM_LIN5].Value = 0x00;
            }

            else if (_matrixValDefinitions[FrwlLghtReq_lL5_BCM_LIN5].Value == 0x01)
            {
                _matrixValDefinitions[FrwlLghtReq_lL5_BCM_LIN5].Value = 0x00;
                //_matrixValDefinitions[DircnLghtOnReq_lL5_BCM_LIN5].Value = 0x00;
            }
        }

        #endregion

        #region Motor Control

        [Description("HdlmpLvlngReq")]
        public void HdlmpLvlngReq(string value)
        {
            byte hdlmpLvlngReq;
            if (!byte.TryParse(value, out hdlmpLvlngReq))
                return;
            if (hdlmpLvlngReq <= 0x07)
                _hdlmpLvlngReq = hdlmpLvlngReq;
        }

        #endregion

        //[Description("LghtngShwMdReq_lL5_BCM_LIN5")]
        //public void LghtngShwReq()
        //{
        //    if (_matrixValDefinitions[LghtngShwMdReq_lL5_BCM_LIN5].Value == 0x00)
        //        _matrixValDefinitions[LghtngShwMdReq_lL5_BCM_LIN5].Value = 0x01;
        //    else if (_matrixValDefinitions[LghtngShwMdReq_lL5_BCM_LIN5].Value == 0x01)
        //        _matrixValDefinitions[LghtngShwMdReq_lL5_BCM_LIN5].Value = 0x00;
        //}

        //[Description("改信号")]
        //public void SetSingal(string name, string value)
        //{
        //    try
        //    {
        //        _matrixValDefinitions[name].Value = Convert.ToByte(value, 16);
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }
        //}

        #region 内部方法
        private readonly Thread _mainWorkThread;
        private readonly Thread _motorWorkThread;
        private bool _isAwake;
        private readonly LinCommunicationMatrix.IntelMatrix _motorolaMatrix =
            new LinCommunicationMatrix.IntelMatrix(0x35, 8);
        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
            new Dictionary<string, MatrixValDefinition>();
        private byte _hdlmpLvlngReq;

        private void MainWork()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(10);

                #region LED LIN5
                if (BcmLin5 != null && _isAwake)
                {
                    _matrixValDefinitions[DISucsLghtEnb_lL5_BCM_LIN5].Value = (byte)(DiSucsLghtEnb ? 0x01 : 0x00);

                    foreach (var key in _matrixValDefinitions.Keys)
                        _motorolaMatrix.UpdateData(_matrixValDefinitions[key]);

                    BcmLin5.SendMasterLin(
                        _motorolaMatrix.MasterLinId, _motorolaMatrix.MatrixData);

                    Thread.Sleep(10);
                    BcmLin5.SendMasterLin(0x31, new byte[0]);
                    //byte[] echo1;
                    //if (BcmLin5.SendSlaveLin(0x31, out echo1, 25))
                    //    Console.WriteLine(ValueHelper.GetHextStr(echo1));

                    BcmLin5.SendMasterLin(
                        _motorolaMatrix.MasterLinId, _motorolaMatrix.MatrixData);

                    Thread.Sleep(10);
                    BcmLin5.SendMasterLin(0x36, new byte[0]);
                    //byte[] echo2;
                    //if (BcmLin5.SendSlaveLin(0x36, out echo2, 25))
                    //    Console.WriteLine(ValueHelper.GetHextStr(echo2));

                    //BcmLin5.SendMasterLin(
                    //  _motorolaMatrix.MasterLinId, _motorolaMatrix.MatrixData);
                    //Thread.Sleep(25);
                    //byte[] echo2;
                    //if (BcmLin5.SendSlaveLin(0x36, out echo2))
                    //    Console.WriteLine(ValueHelper.GetHextStr(echo2));

                }
                #endregion
            }
        }

        private void MotorWork()
        {
            while (_motorWorkThread.IsAlive)
            {
                if (!_motorWorkThread.IsAlive)
                    break;

                Thread.Sleep(20);

                #region Motor LIN6
                if (IccLin6 != null && _isAwake)
                {
                    var intelMatrix = new LinCommunicationMatrix.IntelMatrix(0x08, 8);
                    intelMatrix.MatrixData[0] = 0xF8;
                    intelMatrix.MatrixData[1] = 0xFF;
                    intelMatrix.MatrixData[2] = 0xFF;
                    intelMatrix.MatrixData[3] = 0xFF;
                    intelMatrix.MatrixData[4] = 0xFF;
                    intelMatrix.MatrixData[5] = 0xFF;
                    intelMatrix.MatrixData[6] = 0xFF;
                    intelMatrix.MatrixData[7] = 0xFF;

                    var md = new MatrixValDefinition(0, 3, _hdlmpLvlngReq);
                    intelMatrix.UpdateData(md);
                    IccLin6.SendMasterLin(intelMatrix.MasterLinId, intelMatrix.MatrixData);
                }
                #endregion
            }
        }

        private string AuxMainBeamLghtReq_lL5_BCM_LIN5 = "AuxMainBeamLghtReq_lL5_BCM_LIN5";
        private string BndngLghtOnReq_lL5_BCM_LIN5 = "BndngLghtOnReq_lL5_BCM_LIN5";
        private string BntClsLghtReq_lL5_BCM_LIN5 = "BntClsLghtReq_lL5_BCM_LIN5";
        private string BrkLghtOnReq_lL5_BCM_LIN5 = "BrkLghtOnReq_lL5_BCM_LIN5";
        private string ChrgLghtReq_lL5_BCM_LIN5 = "ChrgLghtReq_lL5_BCM_LIN5";
        private string DayTimeRunningLghtAndSideLghtMd_lL5_BCM_LIN5 = "DayTimeRunningLghtAndSideLghtMd_lL5_BCM_LIN5";
        private string DayTimeRunningLghtOnReq_lL5_BCM_LIN5 = "DayTimeRunningLghtOnReq_lL5_BCM_LIN5";
        private string DimLvlReq_lL5_BCM_LIN5 = "DimLvlReq_lL5_BCM_LIN5";
        private string DipdBeamLghtOnReq_lL5_BCM_LIN5 = "DipdBeamLghtOnReq_lL5_BCM_LIN5";
        private string DircnLghtOnReq_lL5_BCM_LIN5 = "DircnLghtOnReq_lL5_BCM_LIN5";
        private string DISucsLghtEnb_lL5_BCM_LIN5 = "DISucsLghtEnb_lL5_BCM_LIN5";
        private string ExtrLghtFlwUpMdReq_lL5_BCM_LIN5 = "ExtrLghtFlwUpMdReq_lL5_BCM_LIN5";
        private string FrtFogLghtOnReq_lL5_BCM_LIN5 = "FrtFogLghtOnReq_lL5_BCM_LIN5";
        private string FrwlLghtReq_lL5_BCM_LIN5 = "FrwlLghtReq_lL5_BCM_LIN5";
        private string LdspcClsLghtReq_lL5_BCM_LIN5 = "LdspcClsLghtReq_lL5_BCM_LIN5";
        private string LdspcOpenSts_lL5_BCM_LIN5 = "LdspcOpenSts_lL5_BCM_LIN5";
        private string LghtngShwMdReq_lL5_BCM_LIN5 = "LghtngShwMdReq_lL5_BCM_LIN5";
        private string MainBeamLghtOnReq_lL5_BCM_LIN5 = "MainBeamLghtOnReq_lL5_BCM_LIN5";
        private string PerfForFrwlLghtSeln_lL5_BCM_LIN5 = "PerfForFrwlLghtSeln_lL5_BCM_LIN5";
        private string PerfForLghtngShwMdSeln_lL5_BCM_LIN5 = "PerfForLghtngShwMdSeln_lL5_BCM_LIN5";
        private string PerfForWlcmLghtngMdSeln_lL5_BCM_LIN5 = "PerfForWlcmLghtngMdSeln_lL5_BCM_LIN5";
        private string RevsLghtOnReq_lL5_BCM_LIN5 = "RevsLghtOnReq_lL5_BCM_LIN5";
        private string RrFogLghtOnReq_lL5_BCM_LIN5 = "RrFogLghtOnReq_lL5_BCM_LIN5";
        private string SideLghtOnReq_lL5_BCM_LIN5 = "SideLghtOnReq_lL5_BCM_LIN5";
        private string StaOfBMSPackForChrgng_lL5_BCM_LIN5 = "StaOfBMSPackForChrgng_lL5_BCM_LIN5";
        private string WlcmLghtReq_lL5_BCM_LIN5 = "WlcmLghtReq_lL5_BCM_LIN5";
        private string WthrLghtOnReq_lL5_BCM_LIN5 = "WthrLghtOnReq_lL5_BCM_LIN5";
        #endregion
    }
}
