using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,A88前灯")]
    public sealed class A88FrontLamp : ControllerBase
    {
        public CanBus Can;

        [Description("R/W,DrlPL_PWM")]
        public string DrlPlPwm = 100.ToString();

        [Description("R/W,TL_PWM")]
        public string TlPwm = 100.ToString();

        [Description("R/W,Hb_PWM")]
        public string HbPwm = 100.ToString();

        [Description("R/W,Lb_PWM")]
        public string LbPwm = 100.ToString();

        [Description("R,应用程序版本号")]
        public string AppVer;

        [Description("R,引导程序版本号")]
        public string FblVer;

        [Description("R,配置程序版本号")]
        public string CfgVer;

        public A88FrontLamp(string name)
            : base(name)
        {
            SysConfig(
                Directory.GetCurrentDirectory() + @"\ControllerConfig\A88Top_20211018.dbc");
        }

        ~A88FrontLamp()
        {
            Dispose();
        }

        #region 版本号相关

        private uint _canDiagnosisRequestPhyCanId = 0x732;
        //private const uint CanDiagnosisRequestFunCanId = 0x7DF;
        private uint _canDiagnosisResponseCanId = 0x7B2;

        [Description("设置CANID")]
        public void SetCanId(string reqCanId, string recvCanId)
        {
            try
            {
                _canDiagnosisRequestPhyCanId = Convert.ToUInt32(reqCanId, 16);
                _canDiagnosisResponseCanId = Convert.ToUInt32(recvCanId, 16);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("Read应用程序版本号")]
        public void ReadAppVer()
        {
            AppVer = string.Empty;
            AppVer = ReadDidViaCan(0x2E, 0x02).GetStringByAsciiBytes(false);
        }

        [Description("Read引导程序版本号")]
        public void ReadFblVer()
        {
            FblVer = string.Empty;
            FblVer = ReadDidViaCan(0x2E, 0x04).GetStringByAsciiBytes(false);
        }

        [Description("Read配置程序版本号")]
        public void ReadCfgVer()
        {
            CfgVer = string.Empty;
            CfgVer = ReadDidViaCan(0x2E, 0x06).GetStringByAsciiBytes(false);
        }

        private byte[] ReadDidViaCan(byte didHi, byte didLo)
        {
            if (Can == null)
                return new List<byte>().ToArray();

            lock (Can)
            {
                byte[] echoBytes;
                if (Can.CanBusWithUds.TryReadData(
                    _canDiagnosisRequestPhyCanId,
                    _canDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    return new List<byte>().ToArray();
                }

                //return new List<byte>().ToArray();

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    _canDiagnosisRequestPhyCanId,
                    _canDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    return new List<byte>().ToArray();
                }

                return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    _canDiagnosisRequestPhyCanId,
                    _canDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    _canDiagnosisRequestPhyCanId,
                    _canDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();
                return echoBytes ?? new List<byte>().ToArray();
            }
        }
        #endregion

        #region LED相关
        [Description("近光开")]
        public void LbOn()
        {
            _isLbOn = true;
            VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LightShwSts", 0x00);

            LbPwm = _lbPwmTemp;
            HbPwm = _hbPwmTemp;
            TlPwm = _tlPwmTemp;
            _isDrlOnSpecial = false;
        }

        [Description("近光关")]
        public void LbOff()
        {
            _isLbOn = false;
            VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LightShwSts", 0x00);

            LbPwm = _lbPwmTemp;
            HbPwm = _hbPwmTemp;
            TlPwm = _tlPwmTemp;
            _isDrlOnSpecial = false;
        }

        [Description("远光开")]
        public void HbOn()
        {
            _isHbOn = true;
            VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LightShwSts", 0x00);

            LbPwm = _lbPwmTemp;
            HbPwm = _hbPwmTemp;
            TlPwm = _tlPwmTemp;
            _isDrlOnSpecial = false;
        }

        [Description("远光关")]
        public void HbOff()
        {
            _isHbOn = false;
            VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LightShwSts", 0x00);

            LbPwm = _lbPwmTemp;
            HbPwm = _hbPwmTemp;
            TlPwm = _tlPwmTemp;
            _isDrlOnSpecial = false;
        }

        [Description("DRL开")]
        public void DrlOn()
        {
            for (var i = 0; i < _drlSingleOnList.Count; i++)
                _drlSingleOnList[i] = 1;
            _isDrlOn = true;
            VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LightShwSts", 0x00);

            LbPwm = _lbPwmTemp;
            HbPwm = _hbPwmTemp;
            TlPwm = _tlPwmTemp;
            _isDrlOnSpecial = false;
        }

        [Description("DRL关")]
        public void DrlOff()
        {
            for (var i = 0; i < _drlSingleOnList.Count; i++)
                _drlSingleOnList[i] = 0;
            _isDrlOn = false;
            VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LightShwSts", 0x00);

            LbPwm = _lbPwmTemp;
            HbPwm = _hbPwmTemp;
            TlPwm = _tlPwmTemp;
            _isDrlOnSpecial = false;
        }

        [Description("DRL单颗开")]
        public void DrlSingleOn(string drlIndex)
        {
            int index;
            if (int.TryParse(drlIndex, out index))
            {
                if (index >= 1 && index <= 57)
                {
                    if (!_isDrlOnSpecial)
                    {
                        _lbPwmTemp = LbPwm;
                        _hbPwmTemp = HbPwm;
                        _tlPwmTemp = TlPwm;
                        LbPwm = 0.ToString();
                        HbPwm = 0.ToString();
                        TlPwm = 0.ToString();
                        Thread.Sleep(250);
                        _isDrlOnSpecial = true;
                    }

                    VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LightShwSts", 0x01);
                    _drlSingleOnList[index - 1] = 1;
                    _isDrlOn = true;
                }
            }
        }

        [Description("DRL单颗关")]
        public void DrlSingleOff(string drlIndex)
        {
            int index;
            if (int.TryParse(drlIndex, out index))
            {
                if (index >= 1 && index <= 57 && _isDrlOn)
                {
                    if (!_isDrlOnSpecial)
                    {
                        _lbPwmTemp = LbPwm;
                        _hbPwmTemp = HbPwm;
                        _tlPwmTemp = TlPwm;
                        LbPwm = 0.ToString();
                        HbPwm = 0.ToString();
                        TlPwm = 0.ToString();
                        Thread.Sleep(250);
                        _isDrlOnSpecial = true;
                    }

                    VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LightShwSts", 0x01);
                    _drlSingleOnList[index - 1] = 0;
                }
            }
        }

        [Description("PL开")]
        public void PlOn()
        {
            for (var i = 0; i < _drlSingleOnList.Count; i++)
                _drlSingleOnList[i] = 1;
            _isPlOn = true;
            VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LightShwSts", 0x00);

            LbPwm = _lbPwmTemp;
            HbPwm = _hbPwmTemp;
            TlPwm = _tlPwmTemp;
            _isDrlOnSpecial = false;
        }

        [Description("PL关")]
        public void PlOff()
        {
            for (var i = 0; i < _drlSingleOnList.Count; i++)
                _drlSingleOnList[i] = 0;
            _isPlOn = false;
            VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LightShwSts", 0x00);

            LbPwm = _lbPwmTemp;
            HbPwm = _hbPwmTemp;
            TlPwm = _tlPwmTemp;
            _isDrlOnSpecial = false;
        }

        [Description("转向开")]
        public void TurnOn()
        {
            _isTurnOn = true;
            VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LightShwSts", 0x00);

            LbPwm = _lbPwmTemp;
            HbPwm = _hbPwmTemp;
            TlPwm = _tlPwmTemp;
            _isDrlOnSpecial = false;
        }

        [Description("转向关")]
        public void TurnOff()
        {
            _isTurnOn = false;
            VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LightShwSts", 0x00);

            LbPwm = _lbPwmTemp;
            HbPwm = _hbPwmTemp;
            TlPwm = _tlPwmTemp;
            _isDrlOnSpecial = false;
        }

        #endregion

        #region 马达相关

        [Description("马达-找机械零位")]
        public void MotorInit()
        {
            //LcuAls1
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 0, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 1, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 2, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 3, 0x05);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 4, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 5, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 6, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 7, 0x00);

            Thread.Sleep(2500);

            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 0, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 1, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 2, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 3, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 4, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 5, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 6, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 7, 0x00);
        }

        [Description("马达-到光学零位")]
        public void MotorToOpticalZeroPos()
        {
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 0, 0x06);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 1, 0xC0);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 2, 0x1B);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 3, 0x0A);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 4, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 5, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 6, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 7, 0x00);
        }

        [Description("马达-到下极限")]
        public void MotorToLimitDownPos()
        {
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 0, 0x28);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 1, 0x80);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 2, 0xA2);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 3, 0x0A);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 4, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 5, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 6, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 7, 0x00);
        }

        [Description("马达-到上极限")]
        public void MotorToLimitUpPos()
        {
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 0, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 1, 0xC0);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 2, 0x03);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 3, 0x0A);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 4, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 5, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 6, 0x00);
            VectorEmulator.SetMessageVariableByteValue(LcuAls1, 7, 0x00);
        }

        #endregion

        #region Code from CANoe and DBC

        private void SysConfig(string dbcFilePath)
        {
            VectorEmulator =
                new VectorDbcEmulator(new[] { dbcFilePath });
            InitVariables();
            Prestart();
            Start();
        }

        private VectorDbcEmulator VectorEmulator { get; set; }

        private bool _isHbOn;
        private bool _isLbOn;
        private bool _isDrlOn;
        private bool _isPlOn;
        private bool _isTurnOn;
        private readonly List<int> _drlSingleOnList = new List<int>();
        private bool _isDrlOnSpecial;
        private string _lbPwmTemp = 100.ToString();
        private string _hbPwmTemp = 100.ToString();
        private string _tlPwmTemp = 100.ToString();

        private const string LcuAls1 = "LCU_ALS_1";

        private const string LcuFlDrlPl1 = "LCU_FLDrlPl_1";
        private const string LcuFlDrlPl2 = "LCU_FLDrlPl_2";
        private const string LcuFlDrlPl3 = "LCU_FLDrlPl_3";
        private const string LcuFlDrlPl4 = "LCU_FLDrlPl_4";
        private const string LcuFlDrlPl5 = "LCU_FLDrlPl_5";
        private const string LcuFlDrlPl6 = "LCU_FLDrlPl_6";
        private const string LcuFlDrlPl7 = "LCU_FLDrlPl_7";
        private const string LcuFlDrlPl8 = "LCU_FLDrlPl_8";

        private const string LcuFlHb1 = "LCU_FLHb_1";
        private const string LcuFlHb2 = "LCU_FLHb_2";
        private const string LcuFlLb1 = "LCU_FLLb_1";
        private const string LcuFlTl1 = "LCU_FLTl_1";

        private const string LcuFrDrlPl1 = "LCU_FRDrlPl_1";
        private const string LcuFrDrlPl2 = "LCU_FRDrlPl_2";
        private const string LcuFrDrlPl3 = "LCU_FRDrlPl_3";
        private const string LcuFrDrlPl4 = "LCU_FRDrlPl_4";
        private const string LcuFrDrlPl5 = "LCU_FRDrlPl_5";
        private const string LcuFrDrlPl6 = "LCU_FRDrlPl_6";
        private const string LcuFrDrlPl7 = "LCU_FRDrlPl_7";
        private const string LcuFrDrlPl8 = "LCU_FRDrlPl_8";

        private const string LcuFrHb1 = "LCU_FRHb_1";
        private const string LcuFrHb2 = "LCU_FRHb_2";
        private const string LcuFrLb1 = "LCU_FRLb_1";
        private const string LcuFrTl1 = "LCU_FRTl_1";

        private const string LcuMaster1 = "LCU_MASTER_1";
        private const string LcuMaster2 = "LCU_MASTER_2";

        private const string NmLcu = "Nm_LCU";

        private const string HbControl = "LCU_HbSts";
        private const string LbControl = "LCU_LbSts";

        private const string LftDrlControl = "LCU_LftDrlSts";
        private const string LftPlControl = "LCU_LftFrntPlSts";
        private const string LftTurnControl = "LCU_LftTrnSts";

        private const string RightDrlControl = "LCU_RghtDrlSts";
        private const string RightPlControl = "LCU_RghtFrntPlSts";
        private const string RightTurnControl = "LCU_RghtTrnSts";

        private void InitVariables()
        {
            VectorEmulator.InitMessageVariable(LcuAls1, LcuAls1);

            VectorEmulator.InitMessageVariable(LcuFlDrlPl1, LcuFlDrlPl1);
            VectorEmulator.InitMessageVariable(LcuFlDrlPl2, LcuFlDrlPl2);
            VectorEmulator.InitMessageVariable(LcuFlDrlPl3, LcuFlDrlPl3);
            VectorEmulator.InitMessageVariable(LcuFlDrlPl4, LcuFlDrlPl4);
            VectorEmulator.InitMessageVariable(LcuFlDrlPl5, LcuFlDrlPl5);
            VectorEmulator.InitMessageVariable(LcuFlDrlPl6, LcuFlDrlPl6);
            VectorEmulator.InitMessageVariable(LcuFlDrlPl7, LcuFlDrlPl7);
            VectorEmulator.InitMessageVariable(LcuFlDrlPl8, LcuFlDrlPl8);

            VectorEmulator.InitMessageVariable(LcuFlHb1, LcuFlHb1);
            VectorEmulator.InitMessageVariable(LcuFlHb2, LcuFlHb2);

            VectorEmulator.InitMessageVariable(LcuFlLb1, LcuFlLb1);
            VectorEmulator.InitMessageVariable(LcuFlTl1, LcuFlTl1);

            VectorEmulator.InitMessageVariable(LcuFrDrlPl1, LcuFrDrlPl1);
            VectorEmulator.InitMessageVariable(LcuFrDrlPl2, LcuFrDrlPl2);
            VectorEmulator.InitMessageVariable(LcuFrDrlPl3, LcuFrDrlPl3);
            VectorEmulator.InitMessageVariable(LcuFrDrlPl4, LcuFrDrlPl4);
            VectorEmulator.InitMessageVariable(LcuFrDrlPl5, LcuFrDrlPl5);
            VectorEmulator.InitMessageVariable(LcuFrDrlPl6, LcuFrDrlPl6);
            VectorEmulator.InitMessageVariable(LcuFrDrlPl7, LcuFrDrlPl7);
            VectorEmulator.InitMessageVariable(LcuFrDrlPl8, LcuFrDrlPl8);

            VectorEmulator.InitMessageVariable(LcuFrHb1, LcuFrHb1);
            VectorEmulator.InitMessageVariable(LcuFrHb2, LcuFrHb2);

            VectorEmulator.InitMessageVariable(LcuFrLb1, LcuFrLb1);
            VectorEmulator.InitMessageVariable(LcuFrTl1, LcuFrTl1);

            VectorEmulator.InitMessageVariable(LcuMaster1, LcuMaster1);
            VectorEmulator.InitMessageVariable(LcuMaster2, LcuMaster2);

            VectorEmulator.InitMessageVariable(NmLcu, NmLcu);
        }

        private void Prestart()
        {
            //VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_HbSts", 0x01);
            //VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, "LCU_LbSts", 0x01);

            for (var i = 0; i < 57; i++)
                _drlSingleOnList.Add(0);

            VectorEmulator.SetMessageVariableByteValue(LcuMaster2, 0, 0x15);
            VectorEmulator.SetMessageVariableByteValue(LcuMaster2, 1, 0x10);
            VectorEmulator.SetMessageVariableByteValue(LcuMaster2, 2, 0x80);
            VectorEmulator.SetMessageVariableByteValue(NmLcu, 0, 0x4F);
        }

        private void Start()
        {
            VectorEmulator.SetTimer(MsgTimer20Ms(), 20);
            VectorEmulator.SetTimer(MsgTimer200Ms(), 200);
            VectorEmulator.SetTimer(MsgTimer40Ms(), 40);
        }

        private Action MsgTimer20Ms()
        {
            return () =>
            {
                byte drlPlPwm;
                if (!string.IsNullOrEmpty(DrlPlPwm) && byte.TryParse(DrlPlPwm, out drlPlPwm))
                {
                    if (drlPlPwm > 0x64)
                    {
                        drlPlPwm = 0x64;
                    }
                }
                else
                {
                    drlPlPwm = 0x64;
                }

                byte tlPwm;
                if (!string.IsNullOrEmpty(TlPwm) && byte.TryParse(TlPwm, out tlPwm))
                {
                    if (tlPwm > 0x64)
                    {
                        tlPwm = 0x64;
                    }
                }
                else
                {
                    tlPwm = 0x64;
                }

                byte lbPwm;
                if (!string.IsNullOrEmpty(LbPwm) && byte.TryParse(LbPwm, out lbPwm))
                {
                    if (lbPwm > 0x64)
                    {
                        lbPwm = 0x64;
                    }
                }
                else
                {
                    lbPwm = 0x64;
                }

                byte hbPwm;
                if (!string.IsNullOrEmpty(HbPwm) && byte.TryParse(HbPwm, out hbPwm))
                {
                    if (hbPwm > 0x64)
                    {
                        hbPwm = 0x64;
                    }
                }
                else
                {
                    hbPwm = 0x64;
                }

                #region DRL PL

                for (var k = 1; k <= 7; k++)
                {
                    for (var i = 1; i <= 8; i++)
                    {
                        {
                            var msgName = string.Format("LCU_FLDrlPl_{0}", k);
                            var drlIndex = i + (k - 1) * 8;
                            var signalName = string.Format("LCU_LftFrntDrlPl{0}Cmd", drlIndex.ToString().PadLeft(2, '0'));

                            VectorEmulator.SetMessageVariableSignalValue(msgName, signalName,
                                _drlSingleOnList[drlIndex - 1] == 1 ? drlPlPwm : (byte)0x00);
                        }

                        {
                            var msgName = string.Format("LCU_FRDrlPl_{0}", k);
                            var drlIndex = i + (k - 1) * 8;
                            var signalName = string.Format("LCU_RghtFrntDrlPl{0}Cmd", drlIndex.ToString().PadLeft(2, '0'));

                            VectorEmulator.SetMessageVariableSignalValue(msgName, signalName,
                                _drlSingleOnList[drlIndex - 1] == 1 ? drlPlPwm : (byte)0x00);
                        }
                    }
                }

                VectorEmulator.SetMessageVariableSignalValue(LcuFlDrlPl8, "LCU_LftFrntDrlPl57Cmd", _drlSingleOnList[56] == 1 ? drlPlPwm : (byte)0x00);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrDrlPl8, "LCU_RghtFrntDrlPl57Cmd", _drlSingleOnList[56] == 1 ? drlPlPwm : (byte)0x00);

                #endregion

                #region LB HB
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb1, "LCU_LftHb01Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb1, "LCU_LftHb02Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb1, "LCU_LftHb03Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb1, "LCU_LftHb04Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb1, "LCU_LftHb05Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb1, "LCU_LftHb06Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb1, "LCU_LftHb07Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb1, "LCU_LftHb08Cmd", hbPwm);

                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb2, "LCU_LftHb09Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb2, "LCU_LftHb10Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb2, "LCU_LftHb11Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb2, "LCU_LftHb12Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb2, "LCU_LftHb13Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb2, "LCU_LftHb14Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb2, "LCU_LftHb15Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlHb2, "LCU_LftHb16Cmd", hbPwm);

                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb1, "LCU_RghtHb01Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb1, "LCU_RghtHb02Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb1, "LCU_RghtHb03Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb1, "LCU_RghtHb04Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb1, "LCU_RghtHb05Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb1, "LCU_RghtHb06Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb1, "LCU_RghtHb07Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb1, "LCU_RghtHb08Cmd", hbPwm);

                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb2, "LCU_RghtHb09Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb2, "LCU_RghtHb10Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb2, "LCU_RghtHb11Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb2, "LCU_RghtHb12Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb2, "LCU_RghtHb13Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb2, "LCU_RghtHb14Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb2, "LCU_RghtHb15Cmd", hbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrHb2, "LCU_RghtHb16Cmd", hbPwm);

                VectorEmulator.SetMessageVariableSignalValue(LcuFlLb1, "LCU_LftLb01Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlLb1, "LCU_LftLb02Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlLb1, "LCU_LftLb03Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlLb1, "LCU_LftLb04Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlLb1, "LCU_LftLb05Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlLb1, "LCU_LftLb06Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlLb1, "LCU_LftLb07Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlLb1, "LCU_LftLb08Cmd", lbPwm);

                VectorEmulator.SetMessageVariableSignalValue(LcuFrLb1, "LCU_RightLb01Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrLb1, "LCU_RightLb02Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrLb1, "LCU_RightLb03Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrLb1, "LCU_RightLb04Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrLb1, "LCU_RightLb05Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrLb1, "LCU_RightLb06Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrLb1, "LCU_RightLb07Cmd", lbPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrLb1, "LCU_RightLb08Cmd", lbPwm);

                #endregion

                #region Turn

                VectorEmulator.SetMessageVariableSignalValue(LcuFlTl1, "LCU_LftFrntTL01Cmd", tlPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFlTl1, "LCU_LftFrntTL02Cmd", tlPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrTl1, "LCU_RghtFrntTL01Cmd", tlPwm);
                VectorEmulator.SetMessageVariableSignalValue(LcuFrTl1, "LCU_RghtFrntTL02Cmd", tlPwm);

                #endregion

                VectorEmulator.OutPut(LcuAls1, Can);

                VectorEmulator.OutPut(LcuFlDrlPl1, Can);
                VectorEmulator.OutPut(LcuFlDrlPl2, Can);
                VectorEmulator.OutPut(LcuFlDrlPl3, Can);
                VectorEmulator.OutPut(LcuFlDrlPl4, Can);
                VectorEmulator.OutPut(LcuFlDrlPl5, Can);
                VectorEmulator.OutPut(LcuFlDrlPl6, Can);
                VectorEmulator.OutPut(LcuFlDrlPl7, Can);
                VectorEmulator.OutPut(LcuFlDrlPl8, Can);

                VectorEmulator.OutPut(LcuFrDrlPl1, Can);
                VectorEmulator.OutPut(LcuFrDrlPl2, Can);
                VectorEmulator.OutPut(LcuFrDrlPl3, Can);
                VectorEmulator.OutPut(LcuFrDrlPl4, Can);
                VectorEmulator.OutPut(LcuFrDrlPl5, Can);
                VectorEmulator.OutPut(LcuFrDrlPl6, Can);
                VectorEmulator.OutPut(LcuFrDrlPl7, Can);
                VectorEmulator.OutPut(LcuFrDrlPl8, Can);

                VectorEmulator.OutPut(LcuFlHb1, Can);
                VectorEmulator.OutPut(LcuFlHb2, Can);
                VectorEmulator.OutPut(LcuFrHb1, Can);
                VectorEmulator.OutPut(LcuFrHb2, Can);

                VectorEmulator.OutPut(LcuFlLb1, Can);
                VectorEmulator.OutPut(LcuFrLb1, Can);

                VectorEmulator.OutPut(LcuFrTl1, Can);
                VectorEmulator.OutPut(LcuFlTl1, Can);
            };
        }

        private Action MsgTimer40Ms()
        {
            return () =>
            {
                VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, LbControl, _isLbOn ? (byte)0x01 : (byte)0x00);
                VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, HbControl, _isHbOn ? (byte)0x01 : (byte)0x00);

                VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, LftDrlControl, _isDrlOn ? (byte)0x01 : (byte)0x00);
                VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, RightDrlControl, _isDrlOn ? (byte)0x01 : (byte)0x00);

                VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, LftPlControl, _isPlOn ? (byte)0x01 : (byte)0x00);
                VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, RightPlControl, _isPlOn ? (byte)0x01 : (byte)0x00);

                VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, LftTurnControl, _isTurnOn ? (byte)0x01 : (byte)0x00);
                VectorEmulator.SetMessageVariableSignalValue(LcuMaster1, RightTurnControl, _isTurnOn ? (byte)0x01 : (byte)0x00);

                VectorEmulator.OutPut(LcuMaster1, Can);
                VectorEmulator.OutPut(LcuMaster2, Can);
            };
        }

        private Action MsgTimer200Ms()
        {
            return () => { VectorEmulator.OutPut(NmLcu, Can); };
        }

        #endregion
    }
}
