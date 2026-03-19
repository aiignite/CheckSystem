using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,CDX707前灯")]
    public sealed class Cdx707FrontLamp : ControllerBase
    {
        public CanBus Can;
        private VectorDbcEmulator VectorEmulator { get; set; }
        private readonly Dictionary<string, byte[]> _matrixDatas = new Dictionary<string, byte[]>();
        private string _matrixMode = "Default";

        public Cdx707FrontLamp(string name)
            : base(name)
        {
            SysConfig(
                 Directory.GetCurrentDirectory() + @"\ControllerConfig\Y2023_FNV3_FDCAN_HCM_v21.06.dbc");

            _turnLghtLeftDRq2 = 1;
            _turnLghtRightDRq2 = 1;
            _ignitionStatus4 = 4;

            //_headLampLoFlOnBStat1 = 1;
            //_headLampLoFrOnBStat1 = 1;
        }

        ~Cdx707FrontLamp()
        {
            Dispose();
        }

        [Description("模块唤醒")]
        public void ModuleAwake()
        {
            _switchOnOff = true;
        }

        [Description("模块休眠")]
        public void ModuleSleep()
        {
            _switchOnOff = false;
        }

        //[Description("熄火")]
        //public void IgnOff()
        //{
        //    _ignitionStatus4 = 1;
        //}

        //[Description("点火")]
        //public void IgnOn()
        //{
        //    _ignitionStatus4 = 4;
        //}

        [Description("CLASS选择")]
        public void ClassMode(string modeIndex)
        {
            byte index;
            if (!byte.TryParse(modeIndex, out index)) return;
            if (index > 13) return;
            _lowBeamLightClassLdRq4 = index;
            _lowBeamLightClassRdRq4 = index;
        }

        [Description("LowBeamOn")]
        public void LbOn()
        {
            //HeadLampLoFlOn_B_Stat
            //HeadLampLoFrOn_B_Stat
            //_wfSuperstateDStat2 = 0x03;

            _headLampLoFlOnBStat1 = 1;
            _headLampLoFrOnBStat1 = 1;
        }

        [Description("LowBeamOff")]
        public void LbOff()
        {
            //_wfSuperstateDStat2 = 0x00;
            _headLampLoFlOnBStat1 = 0;
            _headLampLoFrOnBStat1 = 0;
        }

        [Description("打开远光全亮-FlashToPass")]
        public void HighBeamFlashToPass()
        {
            //SliceOff();
            //LedOff();
            //_lowBeamLightClassLdRq4 = 0x00;
            //_lowBeamLightClassRdRq4 = 0x00;

            _headLghtHiDRq = 0x01;
            _matrixMode = "Default";
        }

        //[Description("打开远光-Manual")]
        //public void HighBeamManual()
        //{
        //    //SliceOff();
        //    //LedOff();
        //    _headLghtHiDRq = 0x02;
        //}

        [Description("打开远光单颗或ADB控制-OnAuto")]
        public void HighBeamOnAuto()
        {
            //SliceOff();
            //LedOff();

            _headLghtHiDRq = 0x03;
            _matrixMode = "Default";
        }

        [Description("左模块线路板A单颗")]
        public void LeftHbASingleOn(string ledIndex)
        {
            int index;
            if (!int.TryParse(ledIndex, out index)) return;
            if (index >= 1 && index <= 30)
                _matrixMode = string.Format("左A-{0}", index);
        }

        [Description("左模块线路板B单颗")]
        public void LeftHbBSingleOn(string ledIndex)
        {
            int index;
            if (!int.TryParse(ledIndex, out index)) return;
            if (index >= 1 && index <= 30)
                _matrixMode = string.Format("左B-{0}", index);
        }

        [Description("左模块线路板ADB")]
        public void LeftHbAdbOn(string adbIndex)
        {
            int index;
            if (!int.TryParse(adbIndex, out index))
                return;
            if (index >= 1 && index <= 6)
                _matrixMode = string.Format("左ADB-{0}", index);
        }

        [Description("右模块线路板A单颗")]
        public void RightHbASingleOn(string ledIndex)
        {
            int index;
            if (!int.TryParse(ledIndex, out index))
                return;
            if (index >= 1 && index <= 30)
                _matrixMode = string.Format("右A-{0}", index);
        }

        [Description("右模块线路板B单颗")]
        public void RightHbBSingleOn(string ledIndex)
        {
            int index;
            if (!int.TryParse(ledIndex, out index)) return;
            if (index >= 1 && index <= 30)
                _matrixMode = string.Format("右B-{0}", index);
        }

        [Description("右模块线路板ADB")]
        public void RightHbAdbOn(string adbIndex)
        {
            int index;
            if (!int.TryParse(adbIndex, out index)) return;
            if (index >= 1 && index <= 6)
                _matrixMode = string.Format("右ADB-{0}", index);
        }

        [Description("关闭远光")]
        public void HighBeamOff()
        {
            _headLghtHiDRq = 0x00;
            //_matrixMode = "Default";
            //SliceOff();
            //LedOff();
        }

        [Description("打开DRL")]
        public void DrlOn()
        {
            SwitchDrlPl(true, true, true);
            SwitchDrlPl(false, true, true);
        }

        [Description("打开PL")]
        public void PlOn()
        {
            SwitchDrlPl(true, true, false);
            SwitchDrlPl(false, true, false);
        }

        [Description("关闭DRL/PL")]
        public void DrlPlOff()
        {
            SwitchDrlPl(true, false, false);
            SwitchDrlPl(false, false, false);
        }

        [Description("转向灯常亮")]
        public void TurnHoldingOn()
        {
            _tiAutoSwitchOffOn = false;
            _tiAutoSwitchOffSeq = false;
            _tiStateGapTime = 0;
            _turnLghtLeftDRq2 = 2;
            _turnLghtRightDRq2 = 2;
        }

        [Description("转向灯闪烁点亮")]
        public void TurnRunningOn(string keepMs)
        {
            int ms;
            if (!int.TryParse(keepMs, out ms))
                return;
            _tiAutoSwitchOffOn = true;
            _tiAutoSwitchOffSeq = false;
            _tiStateGapTime = ms / 100;
            _turnLghtLeftDRq2 = 2;
            _turnLghtRightDRq2 = 2;
        }

        [Description("转向灯时序点亮")]
        public void TurnFlickerOn(string keepMs)
        {
            int ms;
            if (!int.TryParse(keepMs, out ms))
                return;
            _tiAutoSwitchOffOn = false;
            _tiAutoSwitchOffSeq = true;
            _tiStateGapTime = ms / 100;
            _turnLghtLeftDRq2 = 3;
            _turnLghtRightDRq2 = 3;
        }

        [Description("关闭转向灯")]
        public void TurnOff()
        {
            _tiAutoSwitchOffOn = false;
            _tiAutoSwitchOffSeq = false;
            _tiStateGapTime = 0;
            _turnLghtLeftDRq2 = 1;
            _turnLghtRightDRq2 = 1;
        }

        [Description("按百分比亮度打开SBL")]
        public void SblOn(string percent)
        {
            int per;
            if (!int.TryParse(percent, out per))
                return;
            if (per <= 0 || per > 100)
                return;
            _slght1BrghtLeftPcRq8 = (byte)(int.Parse(percent) * 2);
            _slght1BrghtRightPcRq8 = (byte)(int.Parse(percent) * 2);
        }

        [Description("关闭SBL")]
        public void SblOff()
        {
            _slght1BrghtLeftPcRq8 = 0;
            _slght1BrghtRightPcRq8 = 0;
        }

        [Description("Welcome")]
        public void Welcome()
        {
            _wfSuperstateDStat2 = 0x01;
            _wfSubstateDStat3 = 0x01;
            _ignitionStatus4 = 0x01;
        }

        [Description("Firewell")]
        public void Firewell()
        {
            _wfSuperstateDStat2 = 0x01;
            _wfSubstateDStat3 = 0x00;
            _ignitionStatus4 = 0x01;
        }

        [Description("ResetWelcomeFirewell")]
        public void ResetWelcomeFirewell()
        {
            _wfSuperstateDStat2 = 0x00;
            _wfSubstateDStat3 = 0x00;
            _ignitionStatus4 = 0x04;
        }

        private void SwitchDrlPl(bool isLeft, bool isOn, bool isDrl)
        {
            if (isLeft)
            {
                if (!isOn)
                {
                    _drlPos1ActvLfRq2 = 0x00;
                    _drlPos2ActvLfRq2 = 0x00;
                    _drlPos3ActvLfRq2 = 0x00;
                    _drlPos4ActvLfRq2 = 0x00;

                    _drlPosActvFadeLfRq4 = 0x00;
                }
                else
                {
                    if (isDrl)
                    {
                        _drlPos1ActvLfRq2 = 0x02;
                        _drlPos2ActvLfRq2 = 0x02;
                        _drlPos3ActvLfRq2 = 0x02;
                        _drlPos4ActvLfRq2 = 0x02;

                        _drlPosActvFadeLfRq4 = 0x00;
                    }
                    else
                    {
                        _drlPos1ActvLfRq2 = 0x01;
                        _drlPos2ActvLfRq2 = 0x01;
                        _drlPos3ActvLfRq2 = 0x01;
                        _drlPos4ActvLfRq2 = 0x01;

                        _drlPosActvFadeLfRq4 = 0x00;
                    }
                }
            }
            else
            {
                if (!isOn)
                {
                    _drlPos1ActvRfRq2 = 0x00;
                    _drlPos2ActvRfRq2 = 0x00;
                    _drlPos3ActvRfRq2 = 0x00;
                    _drlPos4ActvRfRq2 = 0x00;

                    _drlPosActvFadeRfRq4 = 0x00;
                }
                else
                {
                    if (isDrl)
                    {
                        _drlPos1ActvRfRq2 = 0x02;
                        _drlPos2ActvRfRq2 = 0x02;
                        _drlPos3ActvRfRq2 = 0x02;
                        _drlPos4ActvRfRq2 = 0x02;

                        _drlPosActvFadeRfRq4 = 0x00;
                    }
                    else
                    {
                        _drlPos1ActvRfRq2 = 0x01;
                        _drlPos2ActvRfRq2 = 0x01;
                        _drlPos3ActvRfRq2 = 0x01;
                        _drlPos4ActvRfRq2 = 0x01;

                        _drlPosActvFadeRfRq4 = 0x00;
                    }
                }
            }
        }

        #region DBC

        private void SysConfig(string dbcFilePath)
        {
            VectorEmulator =
                new VectorDbcEmulator(new[] { dbcFilePath });
            InitVariables();
            Start();
        }

        private void InitVariables()
        {
            VectorEmulator.InitMessageVariable("VehicleOperatingModes", "LDM12_VehicleOperatingModes", "Y2023_FNV3_FDCAN_HCM_v21.06");
            VectorEmulator.InitMessageVariable("HighBeamMaster", "LDM12_HiBeamRq", "Y2023_FNV3_FDCAN_HCM_v21.06");
            VectorEmulator.InitMessageVariable("LvlCmd", "LDM12_LvlCmd", "Y2023_FNV3_FDCAN_HCM_v21.06");
            VectorEmulator.InitMessageVariable("AFSRq", "LDM12_AFSRq", "Y2023_FNV3_FDCAN_HCM_v21.06");
            VectorEmulator.InitMessageVariable("SsblLeftHl", "LDM12_SsblLeftHl", "Y2023_FNV3_FDCAN_HCM_v21.06");
            VectorEmulator.InitMessageVariable("SsblRightHl", "LDM12_SsblRightHl", "Y2023_FNV3_FDCAN_HCM_v21.06");
            VectorEmulator.InitMessageVariable("E2E_BCMtoLDCM", "LDM12_E2E_BCMtoLDCM", "Y2023_FNV3_FDCAN_HCM_v21.06");
            VectorEmulator.InitMessageVariable("BaseFeaturesActvRq", "LDM12_BaseFeaturesActvRq", "Y2023_FNV3_FDCAN_HCM_v21.06");

            //VectorEmulator.InitMessageVariable("VehicleOperatingModes", "LDM12_VehicleOperatingModes", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("Spot1LeftHl", "LDM12_Spot1LeftHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("Spot1RightHl", "LDM12_Spot1RightHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("Spot2LeftHl", "LDM12_Spot2LeftHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("Spot2RightHl", "LDM12_Spot2RightHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("Spot3LeftHl", "LDM12_Spot3LeftHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("Spot3RightHl", "LDM12_Spot3RightHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("HiBeamRq", "LDM12_HiBeamRq", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("LvlCmd", "LDM12_LvlCmd", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("AFSRq", "LDM12_AFSRq", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("SsblLeftHl", "LDM12_SsblLeftHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("SsblRightHl", "LDM12_SsblRightHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("E2E_BCMtoLDCM", "LDM12_E2E_BCMtoLDCM", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            //VectorEmulator.InitMessageVariable("BaseFeaturesActvRq", "LDM12_BaseFeaturesActvRq", "Y2022_H2_HSCAN_HCM_v21.02_Working");

            // add matrix datas
            ReadMatrixTxt(Directory.GetCurrentDirectory() + @"\ControllerConfig\707矩阵\707左单颗.txt");
            ReadMatrixTxt(Directory.GetCurrentDirectory() + @"\ControllerConfig\707矩阵\707右单颗.txt");
            ReadMatrixTxt(Directory.GetCurrentDirectory() + @"\ControllerConfig\707矩阵\707左ADB.txt");
            ReadMatrixTxt(Directory.GetCurrentDirectory() + @"\ControllerConfig\707矩阵\707右ADB.txt");

            _matrixDatas.Add(_matrixMode, new byte[64]);
            _matrixDatas[_matrixMode][0] = 0x60;
        }

        private void ReadMatrixTxt(string file)
        {
            var lines = File.ReadAllLines(file).ToList();

            foreach (var l in lines)
            {
                try
                {
                    if (string.IsNullOrEmpty(l))
                        continue;
                    var sp = l.Split(';');
                    var name = sp[0];
                    var value = sp[1].Replace(" ", "");

                    var bs = new List<byte>();
                    for (var i = 0; i < value.Length; i = i + 2)
                        bs.Add(Convert.ToByte(value.Substring(i, 2), 16));
                    _matrixDatas.Add(name, bs.ToArray());
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void Start()
        {
            VectorEmulator.SetTimer(T_30ms_Event(), 35);
            VectorEmulator.SetTimer(T_20ms_Event(), 25);
            VectorEmulator.SetTimer(T_10ms_Event(), 20);
            VectorEmulator.SetTimer(T_1ms_update_data(), 250);
            VectorEmulator.SetTimer(T_100ms_Event(), 60);

            //VectorEmulator.SetTimer(T_2s_Event(), 2000);
        }

        #region Systeim Timer

        private Action T_30ms_Event()
        {
            return () =>
            {
                if (!_switchOnOff)
                    return;

                VectorEmulator.OutPut("LDM12_LvlCmd", Can, canProtocol: CanBus.CanProtocol.CanFd);

                VectorEmulator.SetMessageVariableByteValue("LDM12_AFSRq", 2, 0x1B);
                VectorEmulator.SetMessageVariableByteValue("LDM12_AFSRq", 3, 0x5B);
                VectorEmulator.SetMessageVariableByteValue("LDM12_AFSRq", 4, 0x6B);
                VectorEmulator.SetMessageVariableByteValue("LDM12_AFSRq", 5, 0x80);
                VectorEmulator.OutPut("LDM12_AFSRq", Can, canProtocol: CanBus.CanProtocol.CanFd);

                VectorEmulator.OutPut("LDM12_SsblLeftHl", Can, canProtocol: CanBus.CanProtocol.CanFd);
                VectorEmulator.OutPut("LDM12_SsblRightHl", Can, canProtocol: CanBus.CanProtocol.CanFd);
            };
        }

        private Action T_20ms_Event()
        {
            return () =>
            {
                if (!_switchOnOff)
                    return;

                VectorEmulator.OutPut("LDM12_BaseFeaturesActvRq", Can, canProtocol: CanBus.CanProtocol.CanFd);
            };
        }

        private Action T_10ms_Event()
        {
            return () =>
            {
                if (!_switchOnOff)
                    return;

                VectorEmulator.OutPut("LDM12_VehicleOperatingModes", Can, canProtocol: CanBus.CanProtocol.CanFd);
            };
        }

        private Action T_1ms_update_data()
        {
            return () =>
            {
                if (!_switchOnOff)
                    return;

                /*Message0x76 LvlCmd */
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_LvlCmd", "HeadLghtLvlLeftRef_D_Rq", _headLghtLvlLeftRefDRq3);
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_LvlCmd", "HeadLghtLvlRghtRef_D_Rq", _headLghtLvlRightRefDRq3);
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_LvlCmd", "HeadLghtLvlLeft_No_Rq", _headLghtLvlLeftNoRq12);
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_LvlCmd", "HeadLghtLvlRight_No_Rq", _headLghtLvlRightNoRq12);

                /*Message0x62 AFSRq */
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_AFSRq", "Traffic_Style_Rq", _trafficStyleRq1);
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_AFSRq", "LowBeamLightClassL_D_Rq", _lowBeamLightClassLdRq4);
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_AFSRq", "LowBeamLightClassR_D_Rq", _lowBeamLightClassRdRq4);
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_AFSRq", "LowBeamLightClassL_T_Rq", _lowBeamLightClassLtRq5);
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_AFSRq", "LowBeamLightClassR_T_Rq", _lowBeamLightClassRtRq5);
                //VectorEmulator.SetMessageVariableSignalValue(
                //    "LDM12_AFSRq", "SwvlTrgtLeft_An_Rq", _lowBeamLightClassRtRq5);
                //VectorEmulator.SetMessageVariableSignalValue(
                //    "LDM12_AFSRq", "SwvlTrgtRight_An_Rq", _lowBeamLightClassRtRq5);

                //get current angle from control
                //tmp_SwvlTrgtLeft_An_Rq_11 = getValue(SwvlTrgtLeft_An_Rq_11);
                //tmp_SwvlTrgtRight_An_Rq_11 = getValue(SwvlTrgtRight_An_Rq_11);

                VectorEmulator.SetMessageVariableSignalValue("LDM12_E2E_BCMtoLDCM", "TurnLghtLeft_D_Rq", _turnLghtLeftDRq2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_E2E_BCMtoLDCM", "TurnLghtRight_D_Rq", _turnLghtRightDRq2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_E2E_BCMtoLDCM", "HeadLampLoFlOn_B_Stat", _headLampLoFlOnBStat1);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_E2E_BCMtoLDCM", "HeadLampLoFrOn_B_Stat", _headLampLoFrOnBStat1);

                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "SideMarker_Actv_Rq", _sideMarkerActvRq1);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "Front_Fog_Actv_Rq", _frontFogActvRq1);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "DrlPos1_Actv_Lf_Rq", _drlPos1ActvLfRq2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "DrlPos1_Actv_Rf_Rq", _drlPos1ActvRfRq2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "DrlPos2_Actv_Lf_Rq", _drlPos2ActvLfRq2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "DrlPos2_Actv_Rf_Rq", _drlPos2ActvRfRq2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "DrlPos3_Actv_Lf_Rq", _drlPos3ActvLfRq2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "DrlPos3_Actv_Rf_Rq", _drlPos3ActvRfRq2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "DrlPos4_Actv_Lf_Rq", _drlPos4ActvLfRq2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "DrlPos4_Actv_Rf_Rq", _drlPos4ActvRfRq2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "Drl_Pos_Actv_Fade_Lf_Rq", _drlPosActvFadeLfRq4);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "Drl_Pos_Actv_Fade_Rf_Rq", _drlPosActvFadeRfRq4);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "WfSuperstate_D_Stat", _wfSuperstateDStat2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "WfSubstate_D_Stat", _wfSubstateDStat3);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "ExtLghtFront_D_RqBody", _extLghtFrontDRqBody2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "HeadLghtSwtch_D_Stat", _headLghtSwtchDStat2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_BaseFeaturesActvRq", "ExtLghtAnmtn_D_Rq", _exLghtAnmtnDRq);

                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblLeftHl", "Slght1BrghtLeft_Pc_Rq", _slght1BrghtLeftPcRq8);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblLeftHl", "Slght1VLeft_T_Rq", _slght1VLeftTRq4);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblLeftHl", "Slght2BrghtLeft_Pc_Rq", _slght2BrghtLeftPcRq8);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblLeftHl", "Slght2VLeft_T_Rq", _slght2VLeftTRq4);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblLeftHl", "Slght3BrghtLeft_Pc_Rq", _slght3BrghtLeftPcRq8);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblLeftHl", "Slght3VLeft_T_Rq", _slght3VLeftTRq4);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblLeftHl", "Slght4BrghtLeft_Pc_Rq", _slght4BrghtLeftPcRq8);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblLeftHl", "Slght4VLeft_T_Rq", _slght4VLeftTRq4);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblLeftHl", "Slght5BrghtLeft_Pc_Rq", _slght5BrghtLeftPcRq8);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblLeftHl", "Slght5VLeft_T_Rq", _slght5VLeftTRq4);

                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblRightHl", "Slght1BrghtRight_Pc_Rq", _slght1BrghtRightPcRq8);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblRightHl", "Slght1VRight_T_Rq", _slght1VRightTRq4);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblRightHl", "Slght2BrghtRight_Pc_Rq", _slght2BrghtRightPcRq8);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblRightHl", "Slght2VRight_T_Rq", _slght2VRightTRq4);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblRightHl", "Slght3BrghtRight_Pc_Rq", _slght3BrghtRightPcRq8);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblRightHl", "Slght3VRight_T_Rq", _slght3VRightTRq4);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblRightHl", "Slght4BrghtRight_Pc_Rq", _slght4BrghtRightPcRq8);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblRightHl", "Slght4VRight_T_Rq", _slght4VRightTRq4);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblRightHl", "Slght5BrghtRight_Pc_Rq", _slght5BrghtRightPcRq8);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_SsblRightHl", "Slght5VRight_T_Rq", _slght5VRightTRq4);

                /*0x66 (HighBeamMaster)*/
                if (_headLghtHiDRq == 0x01 || _headLghtHiDRq == 0x00)
                {
                    VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "HeadLghtHi_D_Rq", _headLghtHiDRq);
                    VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "HeadLghtHi_T_Rq", 0x01);

                    VectorEmulator.OutPut("LDM12_HiBeamRq", Can, canProtocol: CanBus.CanProtocol.CanFd);
                }
                else
                {
                    Can.SendCanDatas(new[]
                    {
                        new CanBus.CanDataPackage(0x66, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data, _matrixDatas[_matrixMode])
                    });
                }

                ///*0x66 (HighBeamMaster1)*/
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm1Mde_D_Rq", Hbm1Mde_D_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm1Ramping_T_Rq", Hbm1Ramping_T_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm1LeftLghtLeft_An_Rq", Hbm1LeftLghtLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm1BrdrBottom_An_Rq", Hbm1BrdrBottom_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm1RightLghtLeft_An_Rq", Hbm1RightLghtLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm1TrnstnLeft_An_Rq", Hbm1TrnstnLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm1TrnstnRight_An_Rq", Hbm1TrnstnRight_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm1LeftLghtRight_An_Rq", Hbm1LeftLghtRight_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm1BrdrTop_An_Rq", Hbm1BrdrTop_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm1RightLghtRigh_An_Rq", Hbm1RightLghtRigh_An_Rq);

                ///*0x66 (HighBeamMaster2)*/
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm2Mde_D_Rq", Hbm2Mde_D_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm2Ramping_T_Rq", Hbm2Ramping_T_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm2LeftLghtLeft_An_Rq", Hbm2LeftLghtLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm2BrdrBottom_An_Rq", Hbm2BrdrBottom_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm2RightLghtLeft_An_Rq", Hbm2RightLghtLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm2TrnstnLeft_An_Rq", Hbm2TrnstnLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm2TrnstnRight_An_Rq", Hbm2TrnstnRight_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm2LeftLghtRight_An_Rq", Hbm2LeftLghtRight_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm2BrdrTop_An_Rq", Hbm2BrdrTop_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm2RightLghtRigh_An_Rq", Hbm2RightLghtRigh_An_Rq);

                ///*0x66 (HighBeamMaster3)*/
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm3Mde_D_Rq", Hbm3Mde_D_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm3Ramping_T_Rq", Hbm3Ramping_T_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm3LeftLghtLeft_An_Rq", Hbm3LeftLghtLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm3BrdrBottom_An_Rq", Hbm3BrdrBottom_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm3RightLghtLeft_An_Rq", Hbm3RightLghtLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm3TrnstnLeft_An_Rq", Hbm3TrnstnLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm3TrnstnRight_An_Rq", Hbm3TrnstnRight_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm3LeftLghtRight_An_Rq", Hbm3LeftLghtRight_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm3BrdrTop_An_Rq", Hbm3BrdrTop_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm3RightLghtRigh_An_Rq", Hbm3RightLghtRigh_An_Rq);

                ///*0x66 (HighBeamMaster4)*/
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4Mde_D_Rq", Hbm4Mde_D_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4Ramping_T_Rq", Hbm4Ramping_T_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4LeftLghtLeft_An_Rq", Hbm4LeftLghtLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4BrdrBottom_An_Rq", Hbm4BrdrBottom_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4RightLghtLeft_An_Rq", Hbm4RightLghtLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4TrnstnLeft_An_Rq", Hbm4TrnstnLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4TrnstnRight_An_Rq", Hbm4TrnstnRight_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4LeftLghtRight_An_Rq", Hbm4LeftLghtRight_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4BrdrTop_An_Rq", Hbm4BrdrTop_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4RightLghtRigh_An_Rq", Hbm4RightLghtRigh_An_Rq);

                ///*0x66 (HighBeamMaster5)*/
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4Mde_D_Rq", Hbm4Mde_D_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4Ramping_T_Rq", Hbm4Ramping_T_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4LeftLghtLeft_An_Rq", Hbm4LeftLghtLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4BrdrBottom_An_Rq", Hbm4BrdrBottom_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4RightLghtLeft_An_Rq", Hbm4RightLghtLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4TrnstnLeft_An_Rq", Hbm4TrnstnLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4TrnstnRight_An_Rq", Hbm4TrnstnRight_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4LeftLghtRight_An_Rq", Hbm4LeftLghtRight_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4BrdrTop_An_Rq", Hbm4BrdrTop_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm4RightLghtRigh_An_Rq", Hbm4RightLghtRigh_An_Rq);

                ///*0x66 (HighBeamMaster6)*/
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm6Mde_D_Rq", Hbm6Mde_D_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm6Ramping_T_Rq", Hbm6Ramping_T_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm6LeftLghtLeft_An_Rq", Hbm6LeftLghtLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm6BrdrBottom_An_Rq", Hbm6BrdrBottom_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm6RightLghtLeft_An_Rq", Hbm6RightLghtLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm6TrnstnLeft_An_Rq", Hbm6TrnstnLeft_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm6TrnstnRight_An_Rq", Hbm6TrnstnRight_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm6LeftLghtRight_An_Rq", Hbm6LeftLghtRight_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm6BrdrTop_An_Rq", Hbm6BrdrTop_An_Rq);
                //VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "Hbm6RightLghtRigh_An_Rq", Hbm6RightLghtRigh_An_Rq);

                VectorEmulator.SetMessageVariableSignalValue("LDM12_VehicleOperatingModes", "Ignition_Status", _ignitionStatus4);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_VehicleOperatingModes", "PwPckTq_D_Stat", _pwPckTqDStat2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_VehicleOperatingModes", "ElPw_D_Stat", _elPwDStat3);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_VehicleOperatingModes", "HCMSleepyTime_B_Stat", _hcmSleepyTime1);

                #region 转向灯功能

                // Simulate TI off /on cycle state
                if (
                    _tiAutoSwitchOffOn && (0 != _tiStateGapTime) && true != _tiAutoSwitchNullOn &&
                    (true != _tiAutoSwitchNullSeq) && (true != _tiAutoSwitchOffSeq))
                {
                    _targetCount = _tiStateGapTime;
                    if (_targetCount == _count)
                    {
                        if (_flag)
                        {
                            _turnLghtLeftDRq2 = 1;
                            _turnLghtRightDRq2 = 1;
                        }
                        else
                        {
                            _turnLghtLeftDRq2 = 2;
                            _turnLghtRightDRq2 = 2;
                        }

                        _count = 0;
                        _flag = !_flag;
                    }
                    else
                    {
                        _count++;
                        if (_count > 10000)
                            _count = 0;
                    }
                }
                else
                    _count = 0;

                // Simulate TI Null/on state
                if (
                    _tiAutoSwitchNullOn &&
                    (0 != _tiStateGapTime) &&
                    true != _tiAutoSwitchOffOn &&
                    (true != _tiAutoSwitchNullSeq) &&
                    (true != _tiAutoSwitchOffSeq))
                {
                    _targetCount1 = _tiStateGapTime;
                    if (_targetCount1 == _count1)
                    {
                        if (_flag1)
                        {
                            _turnLghtLeftDRq2 = 0;
                            _turnLghtRightDRq2 = 0;
                        }
                        else
                        {
                            _turnLghtLeftDRq2 = 2;
                            _turnLghtRightDRq2 = 2;
                        }
                        _count1 = 0;
                        _flag1 = !_flag1;
                    }
                    else
                    {
                        _count1++;
                        if (_count1 > 10000)
                            _count1 = 0;
                    }
                }
                else
                    _count1 = 0;

                // Simulate TI Off/SEQ state
                if (
                    _tiAutoSwitchOffSeq &&
                    (0 != _tiStateGapTime) &&
                    true != _tiAutoSwitchOffOn &&
                    (true != _tiAutoSwitchNullSeq) &&
                    (true != _tiAutoSwitchNullOn))
                {
                    _targetCount2 = _tiStateGapTime;
                    if (_targetCount2 == _count2)
                    {
                        if (_flag2)
                        {
                            _turnLghtLeftDRq2 = 1;
                            _turnLghtRightDRq2 = 1;
                        }
                        else
                        {
                            _turnLghtLeftDRq2 = 3;
                            _turnLghtRightDRq2 = 3;
                        }
                        _count2 = 0;
                        _flag2 = !_flag2;
                    }
                    else
                    {
                        _count2++;
                        if (_count2 > 10000)
                            _count2 = 0;
                    }
                }
                else
                    _count2 = 0;

                // Simulate TI Null/SEQ state
                if (
                    _tiAutoSwitchNullSeq &&
                    (0 != _tiStateGapTime) &&
                    true != _tiAutoSwitchOffOn &&
                    (true != _tiAutoSwitchOffSeq) &&
                    (true != _tiAutoSwitchNullOn))
                {
                    _targetCount3 = _tiStateGapTime;
                    if (_targetCount3 == _count3)
                    {
                        if (_flag3)
                        {
                            _turnLghtLeftDRq2 = 0;
                            _turnLghtRightDRq2 = 0;
                        }
                        else
                        {
                            _turnLghtLeftDRq2 = 3;
                            _turnLghtRightDRq2 = 3;
                        }
                        _count3 = 0;
                        _flag3 = !_flag3;
                    }
                    else
                    {
                        _count3++;
                        if (_count3 > 10000)
                            _count3 = 0;
                    }
                }
                else
                    _count3 = 0;

                #endregion
            };
        }

        private int _u8Mes320Bz;
        private int _u8Mes320Crc;

        private Action T_100ms_Event()
        {
            return () =>
            {
                if (!_switchOnOff)
                    return;

                if (_u8Mes320Bz >= 14)
                    _u8Mes320Bz = 0;
                else
                    _u8Mes320Bz++;

                //_u8Mes320Bz = 1;

                var message320 = new int[8];

                var sumCrc = Crc_CalculateCRC8(new[] { 0x20, 0x03 }, 2, 0xFF);

                message320[0] = (byte)_u8Mes320Bz;
                message320[1] =
                    (_turnLghtLeftDRq2 << 6) |
                    (_turnLghtRightDRq2 << 4) |
                    (_headLampLoFlOnBStat1 << 3) |
                    (_headLampLoFrOnBStat1 << 2);

                sumCrc = Crc_CalculateCRC8(message320, 7, sumCrc);
                sumCrc = (byte)(sumCrc ^ 0xFF);
                _u8Mes320Crc = sumCrc;

                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_E2E_BCMtoLDCM", "CanMsg320_No_Cnt", (byte)_u8Mes320Bz);
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_E2E_BCMtoLDCM", "CanMsg320_No_Crc", (byte)_u8Mes320Crc);

                VectorEmulator.OutPut("LDM12_E2E_BCMtoLDCM", Can, canProtocol: CanBus.CanProtocol.CanFd);
            };
        }

        /// <summary>
        /// Motor_AutoMove
        /// </summary>
        /// <returns></returns>
        private Action T_2s_Event()
        {
            return () =>
            {

            };
        }

        #endregion

        #region System variable

        [Description("R/W,SwitchOnOff")]
        private bool _switchOnOff;

        [Description("R/W,HeadLghtLvlLeftRef_D_Rq_3")]
        private byte _headLghtLvlLeftRefDRq3;

        [Description("R/W,HeadLghtLvlRightRef_D_Rq_3")]
        private byte _headLghtLvlRightRefDRq3;

        [Description("R/W,HeadLghtLvlLeft_No_Rq_12")]
        private byte _headLghtLvlLeftNoRq12;

        [Description("R/W,HeadLghtLvlRight_No_Rq_12")]
        private byte _headLghtLvlRightNoRq12;

        [Description("R/W,Traffic_Style_Rq_1")]
        public byte _trafficStyleRq1;

        [Description("R/W,LowBeamLightClassL_D_Rq_4")]
        public byte _lowBeamLightClassLdRq4;

        [Description("R/W,LowBeamLightClassR_D_Rq_4")]
        public byte _lowBeamLightClassRdRq4;

        [Description("R/W,LowBeamLightClassL_T_Rq_5")]
        private byte _lowBeamLightClassLtRq5;

        [Description("R/W,LowBeamLightClassR_T_Rq_5")]
        private byte _lowBeamLightClassRtRq5;

        [Description("R/W,TurnLghtLeft_D_Rq_2")]
        private byte _turnLghtLeftDRq2;

        [Description("R/W,TurnLghtRight_D_Rq_2")]
        private byte _turnLghtRightDRq2;

        [Description("R/W,HeadLampLoFlOn_B_Stat_1")]
        private byte _headLampLoFlOnBStat1;

        [Description("R/W,HeadLampLoFrOn_B_Stat_1")]
        private byte _headLampLoFrOnBStat1;

        [Description("R/W,SideMarker_Actv_Rq_1")]
        private byte _sideMarkerActvRq1;

        [Description("R/W,Front_Fog_Actv_Rq_1")]
        private byte _frontFogActvRq1;

        [Description("R/W,Turn_Deactv_Brght_Rq_8")]
        private byte _turnDeactvBrghtRq8;

        [Description("R/W,DrlPos1_Actv_Lf_Rq_2")]
        private byte _drlPos1ActvLfRq2;

        [Description("R/W,DrlPos1_Actv_Rf_Rq_2")]
        private byte _drlPos1ActvRfRq2;

        [Description("R/W,DrlPos2_Actv_Lf_Rq_2")]
        private byte _drlPos2ActvLfRq2;

        [Description("R/W,DrlPos2_Actv_Rf_Rq_2")]
        private byte _drlPos2ActvRfRq2;

        [Description("R/W,DrlPos3_Actv_Lf_Rq_2")]
        private byte _drlPos3ActvLfRq2;

        [Description("R/W,DrlPos3_Actv_Rf_Rq_2")]
        private byte _drlPos3ActvRfRq2;

        [Description("R/W,DrlPos4_Actv_Lf_Rq_2")]
        private byte _drlPos4ActvLfRq2;

        [Description("R/W,DrlPos4_Actv_Rf_Rq_2")]
        private byte _drlPos4ActvRfRq2;

        [Description("R/W,Drl_Pos_Actv_Fade_Lf_Rq_4")]
        private byte _drlPosActvFadeLfRq4;

        [Description("R/W,Drl_Pos_Actv_Fade_Rf_Rq_4")]
        private byte _drlPosActvFadeRfRq4;

        [Description("R/W,WfSuperstate_D_Stat_2")]
        private byte _wfSuperstateDStat2;

        [Description("R/W,WfSubstate_D_Stat_3")]
        private byte _wfSubstateDStat3;

        [Description("R/W,ExtLghtFront_D_RqBody_2")]
        private byte _extLghtFrontDRqBody2;

        [Description("R/W,HeadLghtSwtch_D_Stat_2")]
        private byte _headLghtSwtchDStat2;

        [Description("R/W,Slght1BrghtLeft_Pc_Rq_8")]
        private byte _slght1BrghtLeftPcRq8;

        [Description("R/W,Slght1VLeft_T_Rq_4")]
        private byte _slght1VLeftTRq4;

        [Description("R/W,Slght2BrghtLeft_Pc_Rq_8")]
        private byte _slght2BrghtLeftPcRq8;

        [Description("R/W,Slght2VLeft_T_Rq_4")]
        private byte _slght2VLeftTRq4;

        [Description("R/W,Slght3BrghtLeft_Pc_Rq_8")]
        private byte _slght3BrghtLeftPcRq8;

        [Description("R/W,Slght3VLeft_T_Rq_4")]
        private byte _slght3VLeftTRq4;

        [Description("R/W,Slght4BrghtLeft_Pc_Rq_8")]
        private byte _slght4BrghtLeftPcRq8;

        [Description("R/W,Slght4VLeft_T_Rq_4")]
        private byte _slght4VLeftTRq4;

        [Description("R/W,Slght5BrghtLeft_Pc_Rq_8")]
        private byte _slght5BrghtLeftPcRq8;

        [Description("R/W,Slght5VLeft_T_Rq_4")]
        private byte _slght5VLeftTRq4;

        [Description("R/W,Slght1BrghtRight_Pc_Rq_8")]
        private byte _slght1BrghtRightPcRq8;

        [Description("R/W,Slght1VRight_T_Rq_4")]
        private byte _slght1VRightTRq4;

        [Description("R/W,Slght2BrghtRight_Pc_Rq_8")]
        private byte _slght2BrghtRightPcRq8;

        [Description("R/W,Slght2VRight_T_Rq_4")]
        private byte _slght2VRightTRq4;

        [Description("R/W,Slght3BrghtRight_Pc_Rq_8")]
        private byte _slght3BrghtRightPcRq8;

        [Description("R/W,Slght3VRight_T_Rq_4")]
        private byte _slght3VRightTRq4;

        [Description("R/W,Slght4BrghtRight_Pc_Rq_8")]
        private byte _slght4BrghtRightPcRq8;

        [Description("R/W,Slght4VRight_T_Rq_4")]
        private byte _slght4VRightTRq4;

        [Description("R/W,Slght5BrghtRight_Pc_Rq_8")]
        private byte _slght5BrghtRightPcRq8;

        [Description("R/W,Slght5VRight_T_Rq_4")]
        private byte _slght5VRightTRq4;

        [Description("R/W,HeadLghtHi_D_Rq")]
        private byte _headLghtHiDRq;

        [Description("R/W,ExLghtAnmtn_D_Rq")]
        private byte _exLghtAnmtnDRq;

        [Description("R/W,Ignition_Status_4")]
        public byte _ignitionStatus4;

        [Description("R/W,PwPckTq_D_Stat_2")]
        private byte _pwPckTqDStat2;

        [Description("R/W,ElPw_D_Stat_3")]
        private byte _elPwDStat3;

        [Description("R/W,HCMSleepyTime_1")]
        private byte _hcmSleepyTime1;

        #endregion

        #region Func

        private bool _tiAutoSwitchOffSeq; // 时序
        private bool _tiAutoSwitchOffOn; // 闪烁
        private bool _tiAutoSwitchNullOn;
        private bool _tiAutoSwitchNullSeq;

        private int _tiStateGapTime;
        private int _targetCount;
        private int _count;
        private bool _flag;
        private int _targetCount1;
        private int _count1;
        private bool _flag1;
        private int _targetCount2;
        private int _count2;
        private bool _flag2;
        private int _targetCount3;
        private int _count3;
        private bool _flag3;

        private const byte CrcFinalXorCrc8 = 0xff;

        private readonly int[] _crcTable8 =
        {
            0x00, 0x1d, 0x3a, 0x27,
            0x74, 0x69, 0x4e, 0x53,
            0xe8, 0xf5, 0xd2, 0xcf,
            0x9c, 0x81, 0xa6, 0xbb,
            0xcd, 0xd0, 0xf7, 0xea,
            0xb9, 0xa4, 0x83, 0x9e,
            0x25, 0x38, 0x1f, 0x02,
            0x51, 0x4c, 0x6b, 0x76,
            0x87, 0x9a, 0xbd, 0xa0,
            0xf3, 0xee, 0xc9, 0xd4,
            0x6f, 0x72, 0x55, 0x48,
            0x1b, 0x06, 0x21, 0x3c,
            0x4a, 0x57, 0x70, 0x6d,
            0x3e, 0x23, 0x04, 0x19,
            0xa2, 0xbf, 0x98, 0x85,
            0xd6, 0xcb, 0xec, 0xf1,
            0x13, 0x0e, 0x29, 0x34,
            0x67, 0x7a, 0x5d, 0x40,
            0xfb, 0xe6, 0xc1, 0xdc,
            0x8f, 0x92, 0xb5, 0xa8,
            0xde, 0xc3, 0xe4, 0xf9,
            0xaa, 0xb7, 0x90, 0x8d,
            0x36, 0x2b, 0x0c, 0x11,
            0x42, 0x5f, 0x78, 0x65,
            0x94, 0x89, 0xae, 0xb3,
            0xe0, 0xfd, 0xda, 0xc7,
            0x7c, 0x61, 0x46, 0x5b,
            0x08, 0x15, 0x32, 0x2f,
            0x59, 0x44, 0x63, 0x7e,
            0x2d, 0x30, 0x17, 0x0a,
            0xb1, 0xac, 0x8b, 0x96,
            0xc5, 0xd8, 0xff, 0xe2,
            0x26, 0x3b, 0x1c, 0x01,
            0x52, 0x4f, 0x68, 0x75,
            0xce, 0xd3, 0xf4, 0xe9,
            0xba, 0xa7, 0x80, 0x9d,
            0xeb, 0xf6, 0xd1, 0xcc,
            0x9f, 0x82, 0xa5, 0xb8,
            0x03, 0x1e, 0x39, 0x24,
            0x77, 0x6a, 0x4d, 0x50,
            0xa1, 0xbc, 0x9b, 0x86,
            0xd5, 0xc8, 0xef, 0xf2,
            0x49, 0x54, 0x73, 0x6e,
            0x3d, 0x20, 0x07, 0x1a,
            0x6c, 0x71, 0x56, 0x4b,
            0x18, 0x05, 0x22, 0x3f,
            0x84, 0x99, 0xbe, 0xa3,
            0xf0, 0xed, 0xca, 0xd7,
            0x35, 0x28, 0x0f, 0x12,
            0x41, 0x5c, 0x7b, 0x66,
            0xdd, 0xc0, 0xe7, 0xfa,
            0xa9, 0xb4, 0x93, 0x8e,
            0xf8, 0xe5, 0xc2, 0xdf,
            0x8c, 0x91, 0xb6, 0xab,
            0x10, 0x0d, 0x2a, 0x37,
            0x64, 0x79, 0x5e, 0x43,
            0xb2, 0xaf, 0x88, 0x95,
            0xc6, 0xdb, 0xfc, 0xe1,
            0x5a, 0x47, 0x60, 0x7d,
            0x2e, 0x33, 0x14, 0x09,
            0x7f, 0x62, 0x45, 0x58,
            0x0b, 0x16, 0x31, 0x2c,
            0x97, 0x8a, 0xad, 0xb0,
            0xe3, 0xfe, 0xd9, 0xc4
        };

        private byte Crc_CalculateCRC8(IReadOnlyList<int> crcDataPtr, byte crcLength, byte crcStartValue8)
        {
            byte crcLoopCounter;
            var crcValue = (byte)(CrcFinalXorCrc8 ^ crcStartValue8);
            for (crcLoopCounter = 0; crcLoopCounter < crcLength; crcLoopCounter++)
                crcValue = (byte)_crcTable8[crcValue ^ crcDataPtr[crcLoopCounter]];
            return (byte)(crcValue ^ CrcFinalXorCrc8);
        }

        #endregion

        #endregion
    }
}
