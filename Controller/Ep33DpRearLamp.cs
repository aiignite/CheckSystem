using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,EP33低配组合后灯")]
    public sealed class Ep33DpRearLamp : ControllerBase
    {
        public LinBus Lin;

        public Ep33DpRearLamp(string name)
            : base(name)
        {
            _matrixValDefinitions.Add(AuxMainBeamLghtReq_l, new MatrixValDefinition(0, 1, 0));
            _matrixValDefinitions.Add(BndngLghtOnReq_l, new MatrixValDefinition(1, 2, 0));
            _matrixValDefinitions.Add(BrkLghtOnReq_l, new MatrixValDefinition(3, 1, 0));
            _matrixValDefinitions.Add(DISucsLghtEnb_l, new MatrixValDefinition(4, 1, 0));
            _matrixValDefinitions.Add(DayTimeRunningLghtOnReq_l, new MatrixValDefinition(5, 1, 0));
            _matrixValDefinitions.Add(DipdBeamLghtOnReq_l, new MatrixValDefinition(6, 1, 0));
            _matrixValDefinitions.Add(ExtrLghtFlwUpMdReq_l, new MatrixValDefinition(7, 1, 0));
            _matrixValDefinitions.Add(FrtFogLghtOnReq_l, new MatrixValDefinition(8, 2, 0));
            _matrixValDefinitions.Add(WthrLghtOnReq_l, new MatrixValDefinition(10, 2, 0));
            _matrixValDefinitions.Add(WlcmLghtReq_l, new MatrixValDefinition(12, 1, 0));
            _matrixValDefinitions.Add(PerfForWlcmLghtngMdSeln_l, new MatrixValDefinition(18, 4, 0));
            _matrixValDefinitions.Add(PerfForFrwlLghtSeln_l, new MatrixValDefinition(24, 3, 0));
            _matrixValDefinitions.Add(LdspcClsLghtReq_l, new MatrixValDefinition(27, 1, 0));
            _matrixValDefinitions.Add(LghtngShwMdReq_l, new MatrixValDefinition(30, 1, 0));
            _matrixValDefinitions.Add(PerfForLghtngShwMdSeln_l, new MatrixValDefinition(32, 3, 0));
            _matrixValDefinitions.Add(StaOfBMSPackForChrgng_l, new MatrixValDefinition(36, 4, 0));
            _matrixValDefinitions.Add(DimLvlReq_l, new MatrixValDefinition(40, 5, 0));
            _matrixValDefinitions.Add(BntClsLghtReq_l, new MatrixValDefinition(45, 1, 0));
            _matrixValDefinitions.Add(DircnLghtOnReq_l, new MatrixValDefinition(48, 2, 0));
            _matrixValDefinitions.Add(FrwlLghtReq_l, new MatrixValDefinition(50, 1, 0));
            _matrixValDefinitions.Add(LdspcOpenSts_l, new MatrixValDefinition(51, 2, 0));
            _matrixValDefinitions.Add(MainBeamLghtOnReq_l, new MatrixValDefinition(53, 1, 0));
            _matrixValDefinitions.Add(RevsLghtOnReq_l, new MatrixValDefinition(54, 1, 0));
            _matrixValDefinitions.Add(RrFogLghtOnReq_l, new MatrixValDefinition(55, 1, 0));
            _matrixValDefinitions.Add(SideLghtOnReq_l, new MatrixValDefinition(56, 2, 0));

            _mainWorkThread = new Thread(MainWork);
            _mainWorkThread.Start();
        }

        ~Ep33DpRearLamp()
        {
            Dispose();
        }

        private void MainWork()
        {
            var list = new List<byte> { 0x17, 0x18, 0x1a, 0x19 };
            var i = 0;

            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(50);

                if (Lin == null)
                    continue;

                if (!_isAwake)
                    continue;

                lock (_lockSend)
                {
                    foreach (var key in _matrixValDefinitions.Keys)
                        _motorolaMatrix.UpdateData(_matrixValDefinitions[key]);
                    Lin.SendMasterLin(
                        _motorolaMatrix.MasterLinId, _motorolaMatrix.MatrixData);

                    byte[] echo;
                    Lin.SendSlaveLin(list[i], out echo, 200);

                    if (echo != null && echo.Length == 8)
                    {
                        var str = string.Empty;
                        foreach (var t in echo)
                        {
                            str += ValueHelper.GetHextStrWithOx(t) + " ";
                        }

                        Console.WriteLine("FeedBack: {0}, {1}", ValueHelper.GetHextStr(list[i]), str);

                        if (list[i] == 0x17) //左A
                        {
                            var data = new LinCommunicationMatrix.IntelMatrix(0x17, 8);
                            Array.Copy(echo, data.MatrixData, 8);

                            // BrkLampF4_l 	制动灯故障信号
                            // 制动灯故障信号 = data.GetMatrixData(30, 1);
                            // ....
                        }
                        else if (list[i] == 0x18) //左B
                        {
                            var data = new LinCommunicationMatrix.IntelMatrix(0x18, 8);
                            Array.Copy(echo, data.MatrixData, 8);

                            // BrkLampF6_l 
                            //  制动灯故障信号 = data.GetMatrixData(30, 1);
                            // ....
                        }
                        // else if 
                        // else if 
                    }

                    i++;
                    if (i == 4)
                        i = 0;
                }
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

        //[Description("制动灯ON")]
        //public void StopOn()
        //{
        //    _matrixValDefinitions[BrkLghtOnReq_l].Value = 0x01;
        //}

        //[Description("制动灯OFF")]
        //public void StopOff()
        //{
        //    _matrixValDefinitions[BrkLghtOnReq_l].Value = 0x00;
        //}

        [Description("位置灯低亮ON")]
        public void TailOn()
        {
            _matrixValDefinitions[SideLghtOnReq_l].Value = 0x03;
            _matrixValDefinitions[LdspcClsLghtReq_l].Value = 0x00;
            _matrixValDefinitions[LdspcOpenSts_l].Value = 0x00;
        }

        [Description("位置灯高亮ON")]
        public void TailHsdOn()
        {
            _matrixValDefinitions[SideLghtOnReq_l].Value = 0x03;
            _matrixValDefinitions[LdspcClsLghtReq_l].Value = 0x01;
            _matrixValDefinitions[LdspcOpenSts_l].Value = 0x01;
        }

        [Description("位置灯OFF")]
        public void TailOff()
        {
            _matrixValDefinitions[SideLghtOnReq_l].Value = 0x00;
            _matrixValDefinitions[LdspcClsLghtReq_l].Value = 0x00;
            _matrixValDefinitions[LdspcOpenSts_l].Value = 0x00;
        }

        [Description("尾门ON")]
        public void WeimenOn()
        {
            _matrixValDefinitions[LdspcOpenSts_l].Value = 0x01;
        }

        [Description("尾门OFF")]
        public void WeimenOff()
        {
            _matrixValDefinitions[LdspcOpenSts_l].Value = 0x00;
        }

        [Description("转向灯正常点亮")]
        public void TurnOn()
        {
            _matrixValDefinitions[DISucsLghtEnb_l].Value = 0x00;
        }

        [Description("转向灯流水点亮")]
        public void TurnRunningOn()
        {
            _matrixValDefinitions[DISucsLghtEnb_l].Value = 0x01;
        }

        [Description("欢迎动画")]
        public void Welcome()
        {
            _matrixValDefinitions[WlcmLghtReq_l].Value = 0x01;
            _matrixValDefinitions[PerfForWlcmLghtngMdSeln_l].Value = 0x01;
        }

        [Description("欢送动画")]
        public void Farewell()
        {
            _matrixValDefinitions[PerfForWlcmLghtngMdSeln_l].Value = 0x01;
            _matrixValDefinitions[PerfForFrwlLghtSeln_l].Value = 0x01;
        }

        [Description("关闭动画")]
        public void AbortWelcomeFarewell()
        {
            _matrixValDefinitions[WlcmLghtReq_l].Value = 0x00;
            _matrixValDefinitions[PerfForWlcmLghtngMdSeln_l].Value = 0x00;
            _matrixValDefinitions[PerfForFrwlLghtSeln_l].Value = 0x00;
        }

        //[Description("转向灯ON")]
        //public void TurnOn()
        //{
        //    _matrixValDefinitions[DircnLghtOnReq_l].Value = 0x03;
        //    _matrixValDefinitions[LdspcClsLghtReq_l].Value = 0x01;
        //    _matrixValDefinitions[LdspcOpenSts_l].Value = 0x01;
        //}

        //[Description("转向灯OFF")]
        //public void TurnOff()
        //{
        //    _matrixValDefinitions[DircnLghtOnReq_l].Value = 0x00;
        //    _matrixValDefinitions[LdspcClsLghtReq_l].Value = 0x00;
        //    _matrixValDefinitions[LdspcOpenSts_l].Value = 0x00;
        //}

        //[Description("离家动画ON")]
        //public void FarewellOn()
        //{
        //    _matrixValDefinitions[FrwlLghtReq_l].Value = 0x01;
        //    _matrixValDefinitions[PerfForFrwlLghtSeln_l].Value = 0x01;
        //}

        //[Description("离家动画OFF")]
        //public void FarewellOff()
        //{
        //    _matrixValDefinitions[FrwlLghtReq_l].Value = 0x00;
        //    _matrixValDefinitions[PerfForFrwlLghtSeln_l].Value = 0x00;
        //}

        private readonly Thread _mainWorkThread;
        private bool _isAwake;
        private readonly LinCommunicationMatrix.IntelMatrix _motorolaMatrix =
            new LinCommunicationMatrix.IntelMatrix(0x16, 8);
        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
            new Dictionary<string, MatrixValDefinition>();
        private readonly object _lockSend = new object();

        private string AuxMainBeamLghtReq_l = "AuxMainBeamLghtReq_l";
        private string BndngLghtOnReq_l = "BndngLghtOnReq_l";
        private string BrkLghtOnReq_l = "BrkLghtOnReq_l";
        private string DISucsLghtEnb_l = "DISucsLghtEnb_l";
        private string DayTimeRunningLghtOnReq_l = "DayTimeRunningLghtOnReq_l";
        private string DipdBeamLghtOnReq_l = "DipdBeamLghtOnReq_l";
        private string ExtrLghtFlwUpMdReq_l = "ExtrLghtFlwUpMdReq_l";
        private string FrtFogLghtOnReq_l = "FrtFogLghtOnReq_l";
        private string WthrLghtOnReq_l = "WthrLghtOnReq_l";
        private string WlcmLghtReq_l = "WlcmLghtReq_l";
        private string PerfForWlcmLghtngMdSeln_l = "PerfForWlcmLghtngMdSeln_l";
        private string PerfForFrwlLghtSeln_l = "PerfForFrwlLghtSeln_l";
        private string LdspcClsLghtReq_l = "LdspcClsLghtReq_l";
        private string LghtngShwMdReq_l = "LghtngShwMdReq_l";
        private string PerfForLghtngShwMdSeln_l = "PerfForLghtngShwMdSeln_l";
        private string StaOfBMSPackForChrgng_l = "StaOfBMSPackForChrgng_l";
        private string DimLvlReq_l = "DimLvlReq_l";
        private string BntClsLghtReq_l = "BntClsLghtReq_l";
        private string DircnLghtOnReq_l = "DircnLghtOnReq_l";
        private string FrwlLghtReq_l = "FrwlLghtReq_l";
        private string LdspcOpenSts_l = "LdspcOpenSts_l";
        private string MainBeamLghtOnReq_l = "MainBeamLghtOnReq_l";
        private string RevsLghtOnReq_l = "RevsLghtOnReq_l";
        private string RrFogLghtOnReq_l = "RrFogLghtOnReq_l";
        private string SideLghtOnReq_l = "SideLghtOnReq_l";

        #region 版本信息

        [Description("R,读取HASCO总软件版本号")]
        public string HascoSoftwareVersion = string.Empty;

        [Description("R,读取HASCO应用程序软件版本号")]
        public string HascoAppVersion = string.Empty;

        [Description("R,读取HASCO应用程序小软件版本号")]
        public string HascoAppVersionFlag = string.Empty;

        [Description("R,读取HASCO应用程序软件零件号")]
        public string HascoAppPartNo = string.Empty;

        [Description("R,读取HASCO引导程序软件零件号")]
        public string HascoFblPartNo = string.Empty;

        [Description("R,读取HASCO标定程序软件零件号")]
        public string HascoCaliPartNo = string.Empty;

        private LampType _currentLampType;
        private byte _nad;

        [Description("设置为Left RCLA")]
        public void SetLeftRcla()
        {
            _currentLampType = LampType.LeftRcla;
            _nad = 0x01;
        }

        [Description("设置为RCLB")]
        public void SetRightRcla()
        {
            _currentLampType = LampType.Rclb;
            _nad = 0x03;
        }

        [Description("设置为Right RCLA")]
        public void SetRclb()
        {
            _currentLampType = LampType.RightRcla;
            _nad = 0x02;
        }

        [Description("读取HASCO总软件版本号")]
        public void ReadHascoSoftwareVersion(string nad)
        {
            HascoSoftwareVersion = string.Empty;
            //HascoSoftwareVersion = Read2LenEcho(0xF1, 0xF0);
            try
            {
                _nad = Convert.ToByte(nad, 16);
                HascoSoftwareVersion = Read2LenEcho(0xF1, 0x95);
            }
            catch (Exception)
            {
            }
        }

        [Description("读取HASCO应用程序软件版本号")]
        public void ReadHascoAppVersion()
        {
            HascoAppVersion = string.Empty;
            HascoAppVersion = Read2LenEcho(0xF1, 0xF1);
        }

        [Description("读取HASCO应用程序小软件版本号")]
        public void ReadHascoAppVersionFlag()
        {
            HascoAppVersionFlag = string.Empty;
            HascoAppVersionFlag = Read1LenEcho(0xF1, 0xF8);
        }

        [Description("读取HASCO应用程序软件零件号")]
        public void ReadHascoAppPartNo()
        {
            HascoAppPartNo = string.Empty;
            HascoAppPartNo = Read3LenEcho(0xF1, 0xF9);
        }

        [Description("读取HASCO引导程序软件零件号")]
        public void ReadHascoFblPartNo()
        {
            HascoFblPartNo = string.Empty;
            HascoFblPartNo = Read3LenEcho(0xF1, 0xFA);
        }

        [Description("读取HASCO标定程序软件零件号")]
        public void ReadHascoCaliPartNo()
        {
            HascoCaliPartNo = string.Empty;
            HascoCaliPartNo = Read3LenEcho(0xF1, 0xFC);
        }

        private string Read3LenEcho(byte didHi, byte didLo)
        {
            if (Lin == null)
                return string.Empty;

            lock (_lockSend)
            {
                try
                {
                    byte[] echo;
                    if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, new byte[] { _nad, 0x03, 0x22, didHi, didLo, 0xFF, 0xFF, 0xFF }, out echo))
                    {
                        if (echo != null && echo.Length == 8)
                        {
                            var len = echo[2] - 3;
                            var recv = new List<byte> { echo[6], echo[7] };

                            byte[] echo1;
                            if (Lin.SendSlaveLin(0x3D, out echo1))
                            {
                                if (echo1 != null && echo1.Length == 8)
                                {
                                    for (var i = 2; i < 8; i++)
                                    {
                                        recv.Add(echo1[i]);
                                    }

                                    byte[] echo2;
                                    if (Lin.SendSlaveLin(0x3D, out echo2))
                                    {
                                        if (echo2 != null && echo2.Length == 8)
                                        {
                                            for (var i = 2; i < 2 + (len - 2 - 6); i++)
                                            {
                                                recv.Add(echo2[i]);
                                            }

                                            return recv.ToArray().GetStringByAsciiBytes(false);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        private string Read2LenEcho(byte didHi, byte didLo)
        {
            if (Lin == null)
                return string.Empty;

            lock (_lockSend)
            {
                try
                {
                    byte[] echo;
                    if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, new byte[] { _nad, 0x03, 0x22, didHi, didLo, 0xFF, 0xFF, 0xFF }, out echo))
                    {
                        if (echo != null && echo.Length == 8)
                        {
                            var len = echo[2] - 3;
                            var recv = new List<byte> { echo[6], echo[7] };

                            byte[] echo1;
                            if (Lin.SendSlaveLin(0x3D, out echo1))
                            {
                                if (echo1 != null && echo1.Length == 8)
                                {
                                    for (var i = 2; i < 2 + (len - 2); i++)
                                    {
                                        recv.Add(echo1[i]);
                                    }


                                    return recv.ToArray().GetStringByAsciiBytes(false);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        private string Read1LenEcho(byte didHi, byte didLo)
        {
            if (Lin == null)
                return string.Empty;

            lock (_lockSend)
            {
                try
                {
                    byte[] echo;
                    if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, new byte[] { _nad, 0x03, 0x22, didHi, didLo, 0xFF, 0xFF, 0xFF }, out echo))
                    {
                        if (echo != null && echo.Length == 8)
                        {
                            var len = echo[1] - 3;
                            var recv = new List<byte>();
                            for (var i = 5; i < 5 + len; i++)
                            {
                                recv.Add(echo[i]);
                            }

                            return recv.ToArray().GetStringByAsciiBytes(false);
                        }
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        internal enum LampType
        {
            LeftRcla,

            Rclb,

            RightRcla
        }

        #endregion

        #region S11L解锁APP

        [Description("S11L解锁APP")]
        public void WriteAppFlag()
        {
            if (Lin != null)
            {
                lock (_lockSend)
                {
                    for (var i = 0; i < 5; i++)
                    {
                        Lin.SendMasterLin(0x3C, new byte[0]);
                    }

                    //Lin.SendMasterLin(0x3C, new byte[] { 0x65, 0x02, 0x10, 0x03, 0xFF, 0xFF, 0xFF, 0xFF });
                    Thread.Sleep(50);

                    _nad = 0X03;

                    byte[] echo;
                    if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D,
                        new byte[] { _nad, 0x02, 0x10, 0x03, 0xFF, 0xFF, 0xFF, 0xFF }, out echo, 250))
                    {
                        if (echo != null && echo.Length == 8 && echo[0] == _nad && echo[2] == 0x50 && echo[3] == 0x03)
                        {
                            byte[] echo1;
                            if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D,
                                new byte[] { _nad, 0x02, 0x27, 0x01, 0xFF, 0xFF, 0xFF, 0xFF }, out echo1, 100))
                            {
                                if (echo1 != null && echo1.Length == 8 &&
                                    ValueHelper.GetHextStr(echo1)
                                        .Replace(" ", "")
                                        .StartsWith(ValueHelper.GetHextStr(_nad) + "06670100000000"))
                                {
                                    byte[] echo2;
                                    if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D,
                                        new byte[] { _nad, 0x06, 0x27, 0x02, 0x00, 0x00, 0x00, 0x00 }, out echo2, 100))
                                    {
                                        if (echo2 != null && echo2.Length == 8 &&
                                            ValueHelper.GetHextStr(echo2)
                                                .Replace(" ", "")
                                                .StartsWith(ValueHelper.GetHextStr(_nad) + "026702"))
                                        {
                                            byte[] echo3;
                                            Lin.SendMasterLin(0x3C, new byte[] { _nad, 0x10, 0x07, 0x2E, 0x01, 0x01, 0x3C, 0xDE });
                                            Thread.Sleep(25);
                                            if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D,
                                                new byte[] { _nad, 0x21, 0x68, 0x5A, 0xFF, 0xFF, 0xFF, 0xFF }, out echo3, 100))
                                            {
                                                if (echo3 != null && echo3.Length == 8 &&
                                                    ValueHelper.GetHextStr(echo3)
                                                        .Replace(" ", "")
                                                        .StartsWith(ValueHelper.GetHextStr(_nad) + "036E0101"))
                                                {
                                                    Lin.SendMasterLin(0x3C, new byte[] { _nad, 0x02, 0x11, 0x01, 0xFF, 0xFF, 0xFF, 0xFF });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }








                }
            }
        }

        #endregion
    }
}
