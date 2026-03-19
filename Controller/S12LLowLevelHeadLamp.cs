using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;

namespace Controller
{
    [Description("LIN-Product,S12L低配前灯")]
    public sealed class S12LLowLevelHeadLamp : ControllerBase
    {
        public LinBus BcmLin5;
        public LinBus IccLin6;

        [Description("R/W,转向流水使能")]
        public bool TurnFlowEnable;

        public S12LLowLevelHeadLamp(string name)
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
        }

        ~S12LLowLevelHeadLamp()
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
        public void PlReq()
        {
            if (_matrixValDefinitions[SideLghtOnReq_lL5_BCM_LIN5].Value == 0x00)
            {
                _matrixValDefinitions[DimLvlReq_lL5_BCM_LIN5].Value = 0x1f;
                _matrixValDefinitions[SideLghtOnReq_lL5_BCM_LIN5].Value = 0x03;
            }
            else if (_matrixValDefinitions[SideLghtOnReq_lL5_BCM_LIN5].Value == 0x03)
            {
                _matrixValDefinitions[DimLvlReq_lL5_BCM_LIN5].Value = 0x00;
                _matrixValDefinitions[SideLghtOnReq_lL5_BCM_LIN5].Value = 0x00;
            }
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
        public void WlcMode2(string lvl)
        {
            if (lvl == "1" || lvl == "2" || lvl == "3")
            {
                _matrixValDefinitions[PerfForWlcmLghtngMdSeln_lL5_BCM_LIN5].Value = byte.Parse(lvl); // 0x02; // 1~3
            }
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

        #region 版本信息

        [Description("R,软件版本号")]
        public string AppVer = string.Empty;
        [Description("R,硬件版本号")]
        public string HardwareVer = string.Empty;

        [Description("Read版本号")]
        public void ReadVer()
        {
            AppVer = string.Empty;
            HardwareVer = string.Empty;

            if (BcmLin5 == null)
                return;

            lock (_lockSend)
            {
                BcmLin5.SendMasterLin(0x3C, new byte[] { 0x47, 0x03, 0x22, 0xF1, 0x89, 0xFF, 0xFF, 0xFF });
                Thread.Sleep(25);

                byte[] echo1;
                if (BcmLin5.SendSlaveLin(0x3D, out echo1))
                {
                    if (echo1 != null && echo1.Length == 8)
                    {
                        if (echo1[0] == 0x47 && echo1[1] == 0x10 && echo1[2] == 0x0B && echo1[3] == 0x62 && echo1[4] == 0xf1 && echo1[5] == 0x89)
                        {
                            byte[] echo2;
                            if (BcmLin5.SendSlaveLin(0x3D, out echo2))
                            {
                                if (echo2 != null && echo2.Length == 8)
                                {
                                    if (echo2[0] == 0x47 && echo2[1] == 0x21)
                                    {
                                        var bs = new List<byte>
                                        {
                                            echo1[6],
                                            echo1[7],
                                            echo2[2],
                                            echo2[3],
                                            echo2[4],
                                            echo2[5],
                                            echo2[6],
                                            echo2[7]
                                        };

                                        AppVer = bs.ToArray().GetStringByAsciiBytes(false);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            lock (_lockSend)
            {
                BcmLin5.SendMasterLin(0x3C, new byte[] { 0x47, 0x03, 0x22, 0xF0, 0x89, 0xFF, 0xFF, 0xFF });
                Thread.Sleep(25);

                byte[] echo1;
                if (BcmLin5.SendSlaveLin(0x3D, out echo1))
                {
                    if (echo1 != null && echo1.Length == 8)
                    {
                        if (echo1[0] == 0x47 && echo1[1] == 0x10 && echo1[2] == 0x0B && echo1[3] == 0x62 && echo1[4] == 0xf0 && echo1[5] == 0x89)
                        {
                            byte[] echo2;
                            if (BcmLin5.SendSlaveLin(0x3D, out echo2))
                            {
                                if (echo2 != null && echo2.Length == 8)
                                {
                                    if (echo2[0] == 0x47 && echo2[1] == 0x21)
                                    {
                                        var bs = new List<byte>
                                        {
                                            echo1[6],
                                            echo1[7],
                                            echo2[2],
                                            echo2[3],
                                            echo2[4],
                                            echo2[5],
                                            echo2[6],
                                            echo2[7]
                                        };

                                        HardwareVer = bs.ToArray().GetStringByAsciiBytes(false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
        private bool _isAwake;
        private readonly LinCommunicationMatrix.IntelMatrix _motorolaMatrix =
            new LinCommunicationMatrix.IntelMatrix(0x35, 8);
        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
            new Dictionary<string, MatrixValDefinition>();
        private byte _hdlmpLvlngReq;
        private readonly object _lockSend = new object();

        private void MainWork()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(20);

                lock (_lockSend)
                {
                    #region LED LIN5
                    if (BcmLin5 != null && _isAwake)
                    {
                        // TurnFlowEnable
                        _matrixValDefinitions[DISucsLghtEnb_lL5_BCM_LIN5].Value = TurnFlowEnable ? (byte)0x01 : (byte)0x00;

                        foreach (var key in _matrixValDefinitions.Keys)
                            _motorolaMatrix.UpdateData(_matrixValDefinitions[key]);

                        for (int i = 0x31; i <= 0x36; i++)
                        {
                            BcmLin5.SendMasterLin(
                                _motorolaMatrix.MasterLinId, _motorolaMatrix.MatrixData);

                            //Thread.Sleep(25);
                            byte[] echo1;
                            if (BcmLin5.SendSlaveLin((byte)i, out echo1, 50))
                                Console.WriteLine("{0}: {1}", ValueHelper.GetHextStrWithOx((byte)i), ValueHelper.GetHextStr(echo1));
                        }

                        //BcmLin5.SendMasterLin(
                        //    _motorolaMatrix.MasterLinId, _motorolaMatrix.MatrixData);

                        //Thread.Sleep(25);
                        //byte[] echo1;
                        //if (BcmLin5.SendSlaveLin(0x31, out echo1, timeOutMs: 100))
                        //    Console.WriteLine("0x31: {0}", ValueHelper.GetHextStr(echo1));

                        //BcmLin5.SendMasterLin(
                        //  _motorolaMatrix.MasterLinId, _motorolaMatrix.MatrixData);
                        //Thread.Sleep(25);
                        //byte[] echo2;
                        //if (BcmLin5.SendSlaveLin(0x36, out echo2, timeOutMs: 100))
                        //    Console.WriteLine("0x36: {0}", ValueHelper.GetHextStr(echo2));

                    }
                    #endregion
                }

                Thread.Sleep(20);

                lock (_lockSend)
                {
                    #region Motor LIN6
                    if (IccLin6 != null && _isAwake)
                    {
                        var intelMatrix = new LinCommunicationMatrix.IntelMatrix(0x08, 8);
                        var md = new MatrixValDefinition(0, 3, _hdlmpLvlngReq);
                        intelMatrix.UpdateData(md);
                        IccLin6.SendMasterLin(intelMatrix.MasterLinId, intelMatrix.MatrixData);
                    }
                    #endregion
                }
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

        #region APP刷新

        [Description("R,App下载结果")]
        public string AppDownloadResult = string.Empty;
        [Description("R,APP下载耗时-秒")]
        public float AppDownloadCostTime = -9999;

        [Description("R/W,FlashDrv文件路径")]
        public string FlashDriverFilePath = @"E:\Projects-2022\点灯&芯片相关\S12L\前灯模块\S12L低配刷写\Flashdriver.s19";
        [Description("R/W,APP文件路径")]
        public string AppFielPath = @"E:\Projects-2022\点灯&芯片相关\S12L\前灯模块\S12L低配刷写\S12L_App_L_V09.s19";

        private byte _nad = 0x47;
        private byte _diagnosisMasterLinId = 0x3C;
        private byte _diagnosisSlaveLinId = 0x3D;
        private static readonly object FileLocker = new object();

        [Description("刷新APP文件")]
        public void AppDownload()
        {
            AppDownloadResult = string.Empty;
            AppDownloadCostTime = -9999;
            var st = new Stopwatch();
            st.Start();
            DataDownload(AppFielPath, ref AppDownloadResult, false);
            GetLinRecvMsg(new byte[] { _nad, 0x02, 0x11, 0x03, 0xFF, 0xFF, 0xFF, 0xFF }, _diagnosisMasterLinId,
               _diagnosisSlaveLinId);

            GetLinRecvMsg(new byte[] { _nad, 0x02, 0x10, 0x03, 0xFF, 0xFF, 0xFF, 0xFF }, _diagnosisMasterLinId,
              _diagnosisSlaveLinId, isNeedRetry: false);
            GetLinRecvMsg(new byte[] { 0x7E, 0x03, 0x28, 0x80, 0x03, 0xFF, 0xFF, 0xFF }, _diagnosisMasterLinId,
               _diagnosisSlaveLinId, isNeedRetry: false);
            GetLinRecvMsg(new byte[] { 0x7E, 0x02, 0x85, 0x81, 0xFF, 0xFF, 0xFF, 0xFF }, _diagnosisMasterLinId,
             _diagnosisSlaveLinId, isNeedRetry: false);
            GetLinRecvMsg(new byte[] { 0x7E, 0x02, 0x10, 0x81, 0xFF, 0xFF, 0xFF, 0xFF }, _diagnosisMasterLinId,
               _diagnosisSlaveLinId, isNeedRetry: false);

            st.Stop();
            AppDownloadCostTime = st.ElapsedMilliseconds / 1000f;
        }

        private void DataDownload(string filePath, ref string resultMsg, bool isCal)
        {
            if (string.IsNullOrEmpty(FlashDriverFilePath))
            {
                resultMsg = "NG FlashDrv文件不存在";
                return;
            }

            if (!File.Exists(filePath))
            {
                resultMsg = "NG 待刷新文件不存在";
                return;
            }

            List<List<SRecordFileHelper.SRecordLineData>> flashDrvBlocks;
            List<List<SRecordFileHelper.SRecordLineData>> dataBlocks;
            lock (FileLocker)
            {
                var sRecordDrv = SRecordFileHelper.GetSRecordLineData(FlashDriverFilePath);
                flashDrvBlocks = SRecordFileHelper.GetBlocks(sRecordDrv); // Block集合

                var sRecordApp = SRecordFileHelper.GetSRecordLineData(filePath);
                dataBlocks = SRecordFileHelper.GetBlocks(sRecordApp); // Block集合
            }

            if (BcmLin5 == null)
            {
                resultMsg = "NG LIN未初始化";
                return;
            }

            for (var i = 0; i < 20; i++)
            {
                GetLinRecvMsg(new byte[] { 0x7E, 0x02, 0x3E, 0x80, 0xFF, 0xFF, 0xFF, 0xFF }, _diagnosisMasterLinId,
                    _diagnosisSlaveLinId, 25, false);

                byte[] tmepEcho;
                BcmLin5.SendSlaveLin(_diagnosisSlaveLinId, out tmepEcho, 50);
                //BcmLin5.SendMasterLin(_diagnosisMasterLinId, new byte[] { 0x7E, 0x02, 0x3E, 0x80, 0xFF, 0xFF, 0xFF, 0xFF });
            }

            try
            {
                var entendedModeReq = GetLinRecvMsg(
                new byte[] { _nad, 0x02, 0x10, 0x03, 0xff, 0xff, 0xff, 0xff }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
                if (!entendedModeReq.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x50, 0x03 }).Replace(" ", "")))
                {
                    //resultMsg = "NG 10 03失败：" + entendedModeReq;
                    //return;
                }

                var seedReq1 = GetLinRecvMsg(new byte[] { _nad, 0x02, 0x27, 0x01, 0xff, 0xff, 0xff, 0xff }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
                if (!seedReq1.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x06, 0x67, 0x01 }).Replace(" ", "")) || seedReq1.Length != 8 * 2)
                {
                    //resultMsg = "NG 27 01失败：" + seedReq1;
                    //return;
                }

                var seed11 = Convert.ToByte(seedReq1.Substring(8, 2), 16);
                var seed12 = Convert.ToByte(seedReq1.Substring(10, 2), 16);
                var seed13 = Convert.ToByte(seedReq1.Substring(12, 2), 16);
                var seed14 = Convert.ToByte(seedReq1.Substring(14, 2), 16);

                var keys1 = GenerateKey(new[] { seed11, seed12, seed13, seed14 });
                var keyReq1 = GetLinRecvMsg(
                    new byte[] { _nad, 0x06, 0x27, 0x02, keys1[0], keys1[1], keys1[2], keys1[3] },
                    _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
                if (!keyReq1.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x67, 0x02 }).Replace(" ", "")))
                {
                    //resultMsg = "NG 27 02失败：" + keyReq1;
                    //return;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            var req31010203 = GetLinRecvMsg(new byte[] { _nad, 0x04, 0x31, 0x01, 0x02, 0x03, 0xFF, 0xFF },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, 250, false);

            if (!req31010203.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x71 }).Replace(" ", "")) &&
                !req31010203.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x05, 0x71 }).Replace(" ", "")))
            {
                resultMsg = "NG 31 01 02 03失败：" + req31010203;
                return;
            }

            GetLinRecvMsg(new byte[] { 0x7E, 0x02, 0x85, 0x82, 0xFF, 0xFF, 0xFF, 0xFF }, _diagnosisMasterLinId,
                _diagnosisSlaveLinId, isNeedRetry: false);
            GetLinRecvMsg(new byte[] { 0x7E, 0x03, 0x28, 0x81, 0x03, 0xFF, 0xFF, 0xFF }, _diagnosisMasterLinId,
               _diagnosisSlaveLinId, isNeedRetry: false);

            Thread.Sleep(1000);

            BcmLin5.SendMasterLin(_diagnosisMasterLinId, new byte[] { _nad, 0x02, 0x10, 0x02, 0xff, 0xff, 0xff, 0xff });
            Thread.Sleep(2500);

            var programModeReq = GetLinRecvMsg(
                new byte[] { _nad, 0x02, 0x10, 0x02, 0xff, 0xff, 0xff, 0xff }, _diagnosisMasterLinId, _diagnosisSlaveLinId, 200, false);
            if (!programModeReq.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x06, 0x50, 0x02 }).Replace(" ", "")))
            {
                resultMsg = "NG 10 02失败：" + programModeReq;
                return;
            }

            var seedReq2 = GetLinRecvMsg(new byte[] { _nad, 0x02, 0x27, 0x01, 0xff, 0xff, 0xff, 0xff }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!seedReq2.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x06, 0x67, 0x01 }).Replace(" ", "")) || seedReq2.Length != 8 * 2)
            {
                resultMsg = "NG 27 01失败：" + seedReq2;
                return;
            }

            var seed21 = Convert.ToByte(seedReq2.Substring(8, 2), 16);
            var seed22 = Convert.ToByte(seedReq2.Substring(10, 2), 16);
            var seed23 = Convert.ToByte(seedReq2.Substring(12, 2), 16);
            var seed24 = Convert.ToByte(seedReq2.Substring(14, 2), 16);

            var keys2 = GenerateKey(new[] { seed21, seed22, seed23, seed24 });

            var keyReq2 = GetLinRecvMsg(
                new byte[] { _nad, 0x06, 0x27, 0x02, keys2[0], keys2[1], keys2[2], keys2[3] },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);

            if (!keyReq2.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x67, 0x02 }).Replace(" ", "")))
            {
                resultMsg = "NG 27 02失败：" + keyReq2;
                return;
            }

            BcmLin5.SendMasterLin(_diagnosisMasterLinId, new byte[] { _nad, 0x10, 0x0A, 0x2E, 0xF1, 0x84, 0x1F, 0x09 });
            Thread.Sleep(25);
            var writeSwThumbPrint = GetLinRecvMsg(new byte[] { _nad, 0x21, 0x1A, 0x00, 0x00, 0x00, 0x00, 0xFF },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);

            if (!writeSwThumbPrint.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x03, 0x6E, 0xF1, 0x84 }).Replace(" ", "")))
            {
                resultMsg = "NG 2E F1 84 1F 09 1A 00 00 00 00失败：" + writeSwThumbPrint;
                return;
            }

            var flashTotalBytes = new List<byte>();
            var flashDrvStartAddr = flashDrvBlocks[0][0].Address;
            var flashDrvDataLen = 0;
            foreach (var t in flashDrvBlocks[0])
            {
                flashTotalBytes.AddRange(t.Data);
                flashDrvDataLen += t.DataLen;
            }

            var dataRecord = new List<byte>();
            var dataStartAddr = !isCal ? (uint)0x00008000 : 0x00008000;
            var dataLen = 0;
            foreach (var t in dataBlocks[0])
            {
                dataRecord.AddRange(t.Data);
                dataLen += t.DataLen;
            }

            BcmLin5.SendMasterLin(_diagnosisMasterLinId,
                new byte[]
                {
                    _nad, 0x10, 0x0b, 0x34, 0x00, 0x44,
                    BitConverter.GetBytes(flashDrvStartAddr).Reverse().ToArray()[0],
                    BitConverter.GetBytes(flashDrvStartAddr).Reverse().ToArray()[1]
                });
            Thread.Sleep(25);
            var flashDrv34Req = GetLinRecvMsg(
                new byte[]
                {
                    _nad,
                    0x21,
                    BitConverter.GetBytes(flashDrvStartAddr).Reverse().ToArray()[2],
                    BitConverter.GetBytes(flashDrvStartAddr).Reverse().ToArray()[3],
                    BitConverter.GetBytes(flashDrvDataLen).Reverse().ToArray()[0],
                    BitConverter.GetBytes(flashDrvDataLen).Reverse().ToArray()[1],
                    BitConverter.GetBytes(flashDrvDataLen).Reverse().ToArray()[2],
                    BitConverter.GetBytes(flashDrvDataLen).Reverse().ToArray()[3]
                }, _diagnosisMasterLinId,
                _diagnosisSlaveLinId,
                isNeedRetry: false);

            if (!flashDrv34Req.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x74 }).Replace(" ", "")))
            {
                resultMsg = "NG FlashDrv 34失败：" + flashDrv34Req;
                return;
            }

            if (!DataTransfer(flashTotalBytes, ref resultMsg))
                return;

            var exitDataTransfer1 = GetLinRecvMsg(new byte[] { _nad, 0x01, 0x37, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, 200, false);
            if (!exitDataTransfer1.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x03, 0x77 }).Replace(" ", "")))
            {
                resultMsg = "NG FlashDrv 37失败：" + flashDrv34Req;
                return;
            }

            BcmLin5.SendMasterLin(_diagnosisMasterLinId, new byte[] { _nad, 0x10, 0x08, 0x31, 0x01, 0x02, 0x02, 0x00 });
            Thread.Sleep(10);
            var req3101020200000000 = GetLinRecvMsg(new byte[] { _nad, 0x21, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!req3101020200000000.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x05, 0x71, 0x01, 0x02, 0x02 }).Replace(" ", "")))
            {
                resultMsg = "NG FlashDrv 31 01 02 02 00 00 00 00失败：" + req3101020200000000;
                return;
            }

            BcmLin5.SendMasterLin(_diagnosisMasterLinId,
                new byte[]
                {
                    _nad, 0x10, 0x0c, 0x31, 0x01, 0xFF, 0x00,  BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[0]
                });
            Thread.Sleep(25);
            BcmLin5.SendMasterLin(_diagnosisMasterLinId,
                new byte[]
                {
                    _nad, 0x21,
                     BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[1],
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[2],
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[3],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[0],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[1],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[2]
                });
            Thread.Sleep(25);
            //BcmLin5.SendMasterLin(_diagnosisMasterLinId, new byte[]
            //{
            //    _nad, 0x22,
            //    BitConverter.GetBytes(dataLen).Reverse().ToArray()[4],
            //    0xFF, 0xFF, 0xFF, 0xFF, 0xFF
            //});

            //var eraseReq1 = string.Empty;
            //var errorCount = 0;
            //while (true)
            //{
            //    byte[] echoBytes;
            //    if (!Lin19200.SendSlaveLin(_diagnosisSlaveLinId, out echoBytes, 50))
            //    {
            //        errorCount++;
            //        if (errorCount > 10)
            //            break;
            //    }
            //    if (ValueHelper.GetHextStr(echoBytes)
            //            .Replace(" ", "")
            //            .StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x03, 0x7F, 0x31, 0x78 }).Replace(" ", "")))
            //    {
            //        Thread.Sleep(50);
            //        continue;
            //    }

            //    if (!string.IsNullOrEmpty(ValueHelper.GetHextStr(echoBytes)))
            //    {
            //        eraseReq1 = ValueHelper.GetHextStr(echoBytes).Replace(" ", "");
            //        break;
            //    }
            //}
            var eraseReq1 = GetLinRecvMsg(new byte[]
            {
                _nad, 0x22,
                BitConverter.GetBytes(dataLen).Reverse().ToArray()[3],
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF
            }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!eraseReq1.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x05, 0x71, 0x01, 0xFF, 0x00 }).Replace(" ", "")))
            {
                resultMsg = "NG 擦除失败1：" + eraseReq1;
                return;
            }

            BcmLin5.SendMasterLin(_diagnosisMasterLinId,
                new byte[]
                {
                    _nad, 0x10, 0x0b, 0x34, 0x00, 0x44,
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[0],
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[1]
                });
            Thread.Sleep(25);
            var data34Req = GetLinRecvMsg(
                new byte[]
                {
                    _nad,
                    0x21,
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[2],
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[3],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[0],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[1],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[2],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[3]
                }, _diagnosisMasterLinId,
                _diagnosisSlaveLinId,
                isNeedRetry: false);

            if (!data34Req.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x74 }).Replace(" ", "")))
            {
                resultMsg = "NG data 34失败：" + data34Req;
                return;
            }

            if (!DataTransfer(dataRecord, ref resultMsg))
                return;

            var exitDataTransfer2 = GetLinRecvMsg(new byte[] { _nad, 0x01, 0x37, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF },
               _diagnosisMasterLinId, _diagnosisSlaveLinId, 200, false);
            if (!exitDataTransfer2.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x03, 0x77 }).Replace(" ", "")))
            {
                resultMsg = "NG data 37失败：" + flashDrv34Req;
                return;
            }

            var req3101FF01 = GetLinRecvMsg(new byte[] { _nad, 0x04, 0x31, 0x01, 0xFF, 0x01, 0xFF, 0xFF },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!req3101FF01.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x05, 0x71, 0x01, 0xFF, 0x01 }).Replace(" ", "")))
            {
                resultMsg = "NG 31 01 FF 01失败：" + req3101FF01;
                return;
            }

            resultMsg = "OK";
        }

        private bool DataTransfer(IReadOnlyList<byte> dataBytes, ref string msg)
        {
            var dataIndex = 0;
            var countOf36 = dataBytes.Count / 1024; // 1189/1024=1
            var rest36 = dataBytes.Count % 1024; // 1189%1024=165

            if (rest36 == 0)
            {

            }
            else if (rest36 <= 3)
            {

            }
            else
            {
                {
                    for (var i = 0; i < countOf36; i++)
                    {
                        var rollingCount = 0x21;
                        var firstFrame = new byte[] { _nad, 0x14, 0x02, 0x36, (byte)(i + 1), dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++] };
                        BcmLin5.SendMasterLin(_diagnosisMasterLinId, firstFrame);

                        for (var j = 0; j < 1021 / 6; j++)
                        {
                            var continueFrame = new[]
                            {
                                _nad,
                                (byte) rollingCount,
                                dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++],
                                dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++]
                            };

                            BcmLin5.SendMasterLin(_diagnosisMasterLinId, continueFrame);

                            rollingCount++;
                            if (rollingCount > 0x2F)
                                rollingCount = 0x20;
                        }

                        var lastFrameRequest = GetLinRecvMsg(new[]
                        {
                            _nad, (byte) rollingCount,
                            dataBytes[dataIndex++], (byte) 0xFF, (byte) 0xFF,
                            (byte) 0xFF, (byte) 0xFF, (byte) 0xFF
                        },
                            _diagnosisMasterLinId, _diagnosisSlaveLinId);

                        if (!lastFrameRequest.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x76, (byte)(i + 1) }).Replace(" ", "")))
                        {
                            msg = "NG 36传输失败：" + lastFrameRequest;
                            return false;
                        }
                    }
                }

                {
                    var rollingCount = 0x21;

                    var len = (ushort)rest36 + 2 + 0x1000;

                    var len0 = BitConverter.GetBytes(len)[1];
                    var len1 = BitConverter.GetBytes(len)[0];
                    //Console.WriteLine(len0 + len1);

                    var firstFrame = new byte[] { _nad, len0, len1, 0x36, (byte)(countOf36 + 1), dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++] };
                    BcmLin5.SendMasterLin(_diagnosisMasterLinId, firstFrame);

                    if ((rest36 - 3) % 6 == 0)
                    {
                        for (var i = 0; i < (rest36 - 3) / 6; i++)
                        {
                            if (i != (rest36 - 3) / 6 - 1)
                            {
                                var continueFrame = new[]
                                {
                                    _nad,
                                    (byte) rollingCount,
                                    dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++],
                                    dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++]
                                };

                                BcmLin5.SendMasterLin(_diagnosisMasterLinId, continueFrame);
                            }
                            else
                            {
                                var lastFrameRequest = GetLinRecvMsg(new[]
                                {
                                    _nad, (byte) rollingCount,
                                    dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++],
                                    dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++]
                                },
                                    _diagnosisMasterLinId, _diagnosisSlaveLinId);

                                if (!lastFrameRequest.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x76, (byte)(countOf36 + 1) }).Replace(" ", "")))
                                {
                                    msg = "NG 36传输失败：" + lastFrameRequest;
                                    return false;
                                }
                            }

                            rollingCount++;
                            if (rollingCount > 0x2F)
                                rollingCount = 0x20;
                        }
                    }
                    else
                    {
                        for (var i = 0; i < (rest36 - 3) / 6; i++)
                        {
                            var continueFrame = new[]
                                {
                                    _nad,
                                    (byte) rollingCount,
                                    dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++],
                                    dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++]
                                };

                            BcmLin5.SendMasterLin(_diagnosisMasterLinId, continueFrame);

                            rollingCount++;
                            if (rollingCount > 0x2F)
                                rollingCount = 0x20;
                        }

                        var lastDataBytes = new byte[6];
                        for (var i = 0; i < lastDataBytes.Length; i++)
                            lastDataBytes[i] = 0xFF;

                        for (var i = 0; i < (rest36 - 3) % 6; i++)
                            lastDataBytes[i] = dataBytes[dataIndex++];

                        var lastFrameRequest = GetLinRecvMsg(new[]
                        {
                            _nad, (byte) rollingCount,
                            lastDataBytes[0], lastDataBytes[1],
                            lastDataBytes[2], lastDataBytes[3],
                            lastDataBytes[4], lastDataBytes[5]
                        },
                            _diagnosisMasterLinId, _diagnosisSlaveLinId);

                        if (!lastFrameRequest.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x76, (byte)(countOf36 + 1) }).Replace(" ", "")))
                        {
                            msg = "NG 36传输失败：" + lastFrameRequest;
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static byte[] GenerateKey(IReadOnlyList<byte> pu8Seed)
        {
            const uint u32AppKeyConst = 0x5E65CEAA;
            uint u32Seed1 = pu8Seed[0];
            u32Seed1 <<= 8;
            u32Seed1 |= pu8Seed[1];
            u32Seed1 <<= 8;
            u32Seed1 |= pu8Seed[2];
            u32Seed1 <<= 8;
            u32Seed1 |= pu8Seed[3];

            uint u32Seed2 = 0;

            for (var u32Loop = 0; u32Loop < 32; u32Loop++)
            {
                if (((u32Seed1 >> u32Loop) & 0x01) != 0)
                    u32Seed2 |= (uint)1 << (31 - u32Loop);
                else
                    u32Seed2 |= 0;
            }

            var u32Key1 = u32Seed1 ^ u32AppKeyConst;
            var u32Key2 = u32Seed2 ^ u32AppKeyConst;
            var u32Key = u32Key1 + u32Key2;

            return new[] { (byte)(u32Key >> 24), (byte)(u32Key >> 16), (byte)(u32Key >> 8), (byte)(u32Key >> 0) };
        }

        private string GetLinRecvMsg(
            byte[] sendBytes, byte masterLinId, byte slaveLinId, int delayMs = 50, bool isNeedRetry = true)
        {
            byte[] resultBytes;
            if (BcmLin5.SendMasterLinAndRecvSingleSlaveLin(
                masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
            {
                if (resultBytes != null && resultBytes.Length == 8)
                    return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));

                if (isNeedRetry)
                {
                    Thread.Sleep(500);
                    if (!BcmLin5.SendMasterLinAndRecvSingleSlaveLin(
                        masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
                        return string.Empty;

                    if (resultBytes != null && resultBytes.Length == 8)
                        return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));
                }
            }
            else if (isNeedRetry)
            {
                Thread.Sleep(500);
                if (!BcmLin5.SendMasterLinAndRecvSingleSlaveLin(
                    masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
                    return string.Empty;

                if (resultBytes != null && resultBytes.Length == 8)
                    return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));
            }

            return string.Empty;
        }

        #endregion
    }
}
