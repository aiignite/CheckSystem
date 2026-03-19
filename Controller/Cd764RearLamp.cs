using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,CD764组合后灯-lin波特率10417")]
    public sealed class Cd764RearLamp : ControllerBase
    {
        public LinBus Lin;

        public Cd764RearLamp(string name)
            : base(name)
        {
            _matrixValDefinitions.Add(WfSuperstate, new MatrixValDefinition(0, 2, 0));
            _matrixValDefinitions.Add(WfSubstate, new MatrixValDefinition(2, 3, 0));
            _matrixValDefinitions.Add(IgnitionStatus, new MatrixValDefinition(8, 4, 0));
            _matrixValDefinitions.Add(RearAnimationType, new MatrixValDefinition(12, 3, 0));
            _matrixValDefinitions.Add(VehSpeed, new MatrixValDefinition(16, 16, 0));
            _matrixValDefinitions.Add(DecklidAjar, new MatrixValDefinition(32, 1, 0));
            _matrixValDefinitions.Add(DriverSideDrSta, new MatrixValDefinition(33, 1, 0));
            _matrixValDefinitions.Add(PsngrSideDrSta, new MatrixValDefinition(34, 1, 0));
            _matrixValDefinitions.Add(RrSideDrSta, new MatrixValDefinition(35, 1, 0));
            _matrixValDefinitions.Add(RlSideDrSta, new MatrixValDefinition(36, 1, 0));
            _matrixValDefinitions.Add(EngDStat, new MatrixValDefinition(40, 2, 0));

            Sup(0x07, 0x03);
            Ign = 1;
            _matrixValDefinitions[RearAnimationType].Value = 0x01;
            _matrixValDefinitions[EngDStat].Value = 0x03;

            _mainWorkThread = new Thread(MainWork);
            _mainWorkThread.Start();
        }

        ~Cd764RearLamp()
        {
            Dispose();
        }

        [Description("唤醒")]
        public void LampAwake()
        {
            Reset();
            _isAwake = true;
        }

        [Description("休眠")]
        public void LampSleep()
        {
            _isAwake = false;
            Reset();
        }

        [Description("Reset")]
        public void Reset()
        {
            WelcomeMode = 4;
            FarewellMode = 7;
            Sup(0x07, 0x03);
        }

        [Description("LogoOn")]
        public void LogoOn()
        {
            Weimen = 1;
        }

        [Description("LogoOff")]
        public void LogoOff()
        {
            Weimen = 0;
        }

        [Description("迎宾动画")]
        public void Welcome(string modeIndex)
        {
            int index;
            if (!int.TryParse(modeIndex, out index))
                return;
            if (index >= 0 && index <= 3)
                WelcomeMode = index;
        }

        [Description("伴我回家动画")]
        public void Farewell(string modeIndex)
        {
            int index;
            if (!int.TryParse(modeIndex, out index))
                return;
            if (index >= 0 && index <= 6)
                FarewellMode = index;
        }

        private void MainWork()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(20);

                if (Lin == null)
                    continue;

                if (!_isAwake)
                    continue;

                foreach (var key in _matrixValDefinitions.Keys)
                    _motorolaMatrix.UpdateData(_matrixValDefinitions[key]);

                Lin.SendMasterLin(
                    _motorolaMatrix.MasterLinId, _motorolaMatrix.MatrixData);
            }
        }

        private void Sup(byte wfSubstateValue, byte wfSuperstateValue)
        {
            _matrixValDefinitions[WfSubstate].Value = wfSubstateValue;
            _matrixValDefinitions[WfSuperstate].Value = wfSuperstateValue;
            _motorolaMatrix.UpdateData(_matrixValDefinitions[WfSubstate]);
            _motorolaMatrix.UpdateData(_matrixValDefinitions[WfSuperstate]);
        }

        private readonly Thread _mainWorkThread;
        private bool _isAwake;
        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix =
            new LinCommunicationMatrix.MotorolaMatrix(0x15, 6);
        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
            new Dictionary<string, MatrixValDefinition>();

        private const string WfSuperstate = "WFSuperstate";
        private const string WfSubstate = "WFSubstate";
        private const string IgnitionStatus = "Ignition_Status";
        private const string RearAnimationType = "RearAnimation_Type";
        private const string VehSpeed = "Veh_Speed";
        private const string DecklidAjar = "Decklid_Ajar";
        private const string DriverSideDrSta = "Driver_Side_Dr_Sta";
        private const string PsngrSideDrSta = "Psngr_Side_Dr_Sta";
        private const string RrSideDrSta = "RR_Side_Dr_Sta";
        private const string RlSideDrSta = "RL_Side_Dr_Sta";
        private const string EngDStat = "Eng_D_Stat";

        private int _ign;
        private int _vspeed;
        private int _animation;
        private int _weimen;
        private int _driver;
        private int _psngr;
        private int _rlSide;
        private int _rrSide;
        private int _farewellMode;
        private int _welcomeMode;

        private int Ign
        {
            get { return _ign; }
            set
            {
                _ign = value;
                switch (value)
                {
                    case 0:
                        _matrixValDefinitions[IgnitionStatus].Value = 0x00;
                        break;

                    case 1:
                        _matrixValDefinitions[IgnitionStatus].Value = 0x01;
                        break;

                    case 2:
                        _matrixValDefinitions[IgnitionStatus].Value = 0x02;
                        break;

                    case 3:
                        _matrixValDefinitions[IgnitionStatus].Value = 0x04;
                        break;

                    case 4:
                        _matrixValDefinitions[IgnitionStatus].Value = 0x08;
                        break;

                    case 5:
                        _matrixValDefinitions[IgnitionStatus].Value = 0x0F;
                        break;
                }
            }
        }

        private int Vspeed
        {
            get { return _vspeed; }
            set { _vspeed = value; }
        }

        private int Animation
        {
            get { return _animation; }
            set { _animation = value; }
        }

        private int Weimen
        {
            get { return _weimen; }
            set
            {
                _weimen = value;

                _matrixValDefinitions[DecklidAjar].Value = value == 1 ? (byte)1 : (byte)0;
                //_matrixValDefinitions[DriverSideDrSta].Value = value == 1 ? (byte)1 : (byte)0;
                //_matrixValDefinitions[PsngrSideDrSta].Value = value == 1 ? (byte)1 : (byte)0;
                //_matrixValDefinitions[RrSideDrSta].Value = value == 1 ? (byte)1 : (byte)0;
                //_matrixValDefinitions[RlSideDrSta].Value = value == 1 ? (byte)1 : (byte)0;
            }
        }

        private int Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }

        private int Psngr
        {
            get { return _psngr; }
            set { _psngr = value; }
        }

        private int RlSide
        {
            get { return _rlSide; }
            set { _rlSide = value; }
        }

        private int RrSide
        {
            get { return _rrSide; }
            set { _rrSide = value; }
        }

        private int WelcomeMode
        {
            get { return _welcomeMode; }
            set
            {
                _welcomeMode = value;
                switch (value)
                {
                    case 0:
                        Sup(0x00, 0x00);
                        Thread.Sleep(500);
                        Sup(0x01, 0x01);
                        break;

                    case 1:
                        Sup(0x00, 0x00);
                        Thread.Sleep(500);
                        Sup(0x02, 0x01);
                        break;

                    case 2:
                        Sup(0x01, 0x01);
                        Thread.Sleep(500);
                        Sup(0x01, 0x01);
                        break;

                    case 3:
                        Sup(0x01, 0x01);
                        Thread.Sleep(500);
                        Sup(0x02, 0x01);
                        break;
                }
            }
        }

        private int FarewellMode
        {
            get { return _farewellMode; }
            set
            {
                _farewellMode = value;
                switch (value)
                {
                    case 0:
                        Sup(0x01, 0x01);
                        Thread.Sleep(500);
                        Sup(0x00, 0x00);
                        break;

                    case 1:
                        Sup(0x02, 0x01);
                        Thread.Sleep(500);
                        Sup(0x00, 0x00);
                        break;

                    case 2:
                        Sup(0x04, 0x02);
                        Thread.Sleep(500);
                        Sup(0x00, 0x00);
                        break;

                    case 3:
                        Sup(0x06, 0x01);
                        Thread.Sleep(500);
                        Sup(0x00, 0x00);
                        break;

                    case 4:
                        Sup(0x06, 0x02);
                        Thread.Sleep(500);
                        Sup(0x00, 0x00);
                        break;

                    case 5:
                        Sup(0x07, 0x02);
                        Thread.Sleep(500);
                        Sup(0x00, 0x00);
                        break;

                    case 6:
                        Sup(0x04, 0x01);
                        Thread.Sleep(500);
                        Sup(0x00, 0x00);
                        break;
                }
            }
        }

        #region 版本信息

        [Description("R,读取HASCO软件版本号")]
        public string HascoAppVersion = string.Empty;

        [Description("R,读取HASCO软件零件号")]
        public string HascoAppPartNo = string.Empty;

        private byte _nad;
        private readonly object _lockSend = new object();

        [Description("设置为Left RCLA")]
        public void SetLeftRcla()
        {
            _nad = 0x36;
        }

        [Description("设置为Right RCLA")]
        public void SetRightRcla()
        {
            _nad = 0x36;
        }

        [Description("设置为Left RCLB")]
        public void SetLeftRclb()
        {
            _nad = 0x36;
        }

        [Description("设置为Right RCLB")]
        public void SetRightRclb()
        {
            _nad = 0x37;
        }

        [Description("读取HASCO从节点软件版本号")]
        public void ReadHascoSoftwareVersion()
        {
            HascoAppVersion = string.Empty;
            //HascoSoftwareVersion = Read2LenEcho(0xF1, 0xF0);
            try
            {
                //_nad = Convert.ToByte(nad, 16);
                HascoAppVersion = Read4LenEcho(0xF1, 0x94);
            }
            catch (Exception)
            {
            }
        }

        [Description("读取HASCO应用程序软件零件号")]
        public void ReadHascoAppPartNo()
        {
            HascoAppPartNo = string.Empty;
            HascoAppPartNo = Read3LenEcho(0xF1, 0x95);
        }

        private string Read4LenEcho(byte didHi, byte didLo)
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
                                        for (var i = 2; i < 8; i++)
                                        {
                                            recv.Add(echo2[i]);
                                        }

                                        byte[] echo3;
                                        if (Lin.SendSlaveLin(0x3D, out echo3))
                                        {
                                            if (echo3 != null && echo3.Length == 8)
                                            {
                                                for (var i = 2; i < 2 + (len - 2 - 6 - 6); i++)
                                                {
                                                    recv.Add(echo3[i]);
                                                }

                                                return recv.ToArray().GetStringByAsciiBytes(false);
                                            }
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
    }
}
