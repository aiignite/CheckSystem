using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,CDX707后灯")]
    public sealed class Cdx707RearLamp : ControllerBase
    {
        public CanBus Can;
        private VectorDbcEmulator VectorEmulator { get; set; }

        public Cdx707RearLamp(string name)
            : base(name)
        {
            SysConfig(
                Directory.GetCurrentDirectory() + @"\ControllerConfig\Y2023_FNV3_FDCAN_HCM_v22.11.dbc");

            //_headLghtSwtchDStat2 = 1;
            _turnLghtLeftDRq2 = 1;
            _turnLghtRightDRq2 = 1;
            _ignitionStatus4 = 4;
        }

        ~Cdx707RearLamp()
        {
            Dispose();
        }

        [Description("控制板唤醒")]
        public void ModuleAwake()
        {
            _switchOnOff = true;
        }

        [Description("控制板休眠")]
        public void ModuleSleep()
        {
            _switchOnOff = false;
        }

        [Description("TailOn")]
        public void TailOn()
        {
            _drlPos3ActvLfRq2 = 1;
            _drlPos3ActvRfRq2 = 1;
        }

        [Description("TailOff")]
        public void TailOff()
        {
            _drlPos3ActvLfRq2 = 0;
            _drlPos3ActvRfRq2 = 0;
        }

        [Description("LeftTailOn")]
        public void LeftTailOn()
        {
            _drlPos3ActvLfRq2 = 1;
        }

        [Description("LeftTailOff")]
        public void LeftTailOff()
        {
            _drlPos3ActvLfRq2 = 0;
        }

        [Description("RightTailOn")]
        public void RightTailOn()
        {
            _drlPos3ActvRfRq2 = 1;
        }

        [Description("RightTailOff")]
        public void RightTailOff()
        {
            _drlPos3ActvRfRq2 = 0;
        }

        [Description("尾门控制ON")]
        public void TailGateOn()
        {
            _drStatInnrTgateBActl = 0x01;
        }

        [Description("尾门控制OFF")]
        public void TailGateOff()
        {
            _drStatInnrTgateBActl = 0x00;
        }

        [Description("Logo信号开")]
        public void LogoSignalOn()
        {
            _vehVActlEng = 0x01;
        }

        [Description("Logo信号关")]
        public void LogoSignalOff()
        {
            _vehVActlEng = 0x00;
        }

        [Description("Welcome")]
        public void Welcome()
        {
            _ignitionStatus4 = 0x01;
            _wfSuperstateDStat2 = 0x01;
            _wfSubstateDStat3 = 0x01;

        }

        [Description("Firewell")]
        public void Firewell()
        {
            _ignitionStatus4 = 0x01;
            _wfSuperstateDStat2 = 0x01;
            _wfSubstateDStat3 = 0x00;
        }

        [Description("ResetWelcomeFirewell")]
        public void ResetWelcomeFirewell()
        {
            _ignitionStatus4 = 0x04;
            _wfSuperstateDStat2 = 0x00;
            _wfSubstateDStat3 = 0x00;
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
            VectorEmulator.InitMessageVariable("BodyInfo_3_HCM", "BodyInfo_3_HCM");
            VectorEmulator.InitMessageVariable("VehicleOperatingModes", "LDM12_VehicleOperatingModes");
            VectorEmulator.InitMessageVariable("HighBeamMaster", "LDM12_HiBeamRq");
            VectorEmulator.InitMessageVariable("LvlCmd", "LDM12_LvlCmd");
            VectorEmulator.InitMessageVariable("AFSRq", "LDM12_AFSRq");
            VectorEmulator.InitMessageVariable("SsblLeftHl", "LDM12_SsblLeftHl");
            VectorEmulator.InitMessageVariable("SsblRightHl", "LDM12_SsblRightHl");
            VectorEmulator.InitMessageVariable("E2E_BCMtoLDCM", "LDM12_E2E_BCMtoLDCM");
            VectorEmulator.InitMessageVariable("BaseFeaturesActvRq", "LDM12_BaseFeaturesActvRq");

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

                lock (_lampLocker)
                {
                    VectorEmulator.OutPut("BodyInfo_3_HCM", Can, canProtocol: CanBus.CanProtocol.CanFd);
                    VectorEmulator.OutPut("LDM12_LvlCmd", Can, canProtocol: CanBus.CanProtocol.CanFd);
                    VectorEmulator.OutPut("LDM12_AFSRq", Can, canProtocol: CanBus.CanProtocol.CanFd);
                    VectorEmulator.OutPut("LDM12_SsblLeftHl", Can, canProtocol: CanBus.CanProtocol.CanFd);
                    VectorEmulator.OutPut("LDM12_SsblRightHl", Can, canProtocol: CanBus.CanProtocol.CanFd);
                }
            };
        }

        private Action T_20ms_Event()
        {
            return () =>
            {
                if (!_switchOnOff)
                    return;

                lock (_lampLocker)
                {
                    VectorEmulator.OutPut("LDM12_HiBeamRq", Can, canProtocol: CanBus.CanProtocol.CanFd);
                    VectorEmulator.OutPut("LDM12_BaseFeaturesActvRq", Can, canProtocol: CanBus.CanProtocol.CanFd);
                }
            };
        }

        private Action T_10ms_Event()
        {
            return () =>
            {
                if (!_switchOnOff)
                    return;

                lock (_lampLocker)
                {
                    VectorEmulator.OutPut("LDM12_VehicleOperatingModes", Can, canProtocol: CanBus.CanProtocol.CanFd);
                }
            };
        }

        private Action T_1ms_update_data()
        {
            return () =>
            {
                if (!_switchOnOff)
                    return;

                lock (_lampLocker)
                {
                    // 新增尾门开关，0x3B3
                    VectorEmulator.SetMessageVariableSignalValue("BodyInfo_3_HCM", "DrStatInnrTgate_B_Actl", _drStatInnrTgateBActl);

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
                    VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "HeadLghtHi_D_Rq", _headLghtHiDRq);
                    VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "HeadLghtHi_T_Rq", 0x01);

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

                    // 新增Veh_V_ActlEng，0x55
                    VectorEmulator.SetMessageVariableSignalValue("LDM12_VehicleOperatingModes", "Veh_V_ActlEng", _vehVActlEng);

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
                }
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

        [Description("R/W,DrStatInnrTgate_B_Actl")]
        private byte _drStatInnrTgateBActl;

        [Description("R/W,Veh_V_ActlEng")]
        private byte _vehVActlEng;

        [Description("R/W,HeadLghtLvlLeftRef_D_Rq_3")]
        private byte _headLghtLvlLeftRefDRq3;

        [Description("R/W,HeadLghtLvlRightRef_D_Rq_3")]
        private byte _headLghtLvlRightRefDRq3;

        [Description("R/W,HeadLghtLvlLeft_No_Rq_12")]
        private byte _headLghtLvlLeftNoRq12;

        [Description("R/W,HeadLghtLvlRight_No_Rq_12")]
        private byte _headLghtLvlRightNoRq12;

        [Description("R/W,Traffic_Style_Rq_1")]
        private byte _trafficStyleRq1;

        [Description("R/W,LowBeamLightClassL_D_Rq_4")]
        private byte _lowBeamLightClassLdRq4;

        [Description("R/W,LowBeamLightClassR_D_Rq_4")]
        private byte _lowBeamLightClassRdRq4;

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

        #region 版本信息

        private readonly object _lampLocker = new object();
        private uint _requestCanId = 0x7b5;
        private uint _responseCanId = 0x6b5;

        [Description("R,读取HASCO从节点软件版本号")]
        public string AppVer;

        [Description("R,读取HASCO从节点软件零件号")]
        public string AppPartNo;

        [Description("读取HASCO从节点软件版本号")]
        public void ReadAppVer()
        {
            AppVer = string.Empty;

            if (Can == null)
                return;

            try
            {
                var requestCanId = _requestCanId;
                var responseCanId = _responseCanId;

                Can.AddDoNotFilterCanId(responseCanId);
                Can.CanRecvDataPackages.Clear();
                for (var i = 0; i < 5; i++)
                {
                    Can.SendStandardCanData(requestCanId, new byte[] { 0x03, 0x22, 0xF1, 0x94, 0xFF, 0xFF, 0xFF, 0xFF });
                    Thread.Sleep(250);
                }
                Can.RemoveDoNotFilterCanId(responseCanId);
                if (Can.CanRecvDataPackages.Count >= 4)
                {
                    var findRecv = Can.CanRecvDataPackages.ToList().FindAll(f => f.CanId == responseCanId);

                    var find0 =
                        findRecv.FindIndex(f => f.CanId == responseCanId && f.CanDataLen == 8 && f.CanData[0] == 0x10);
                    var find1 =
                       findRecv.FindIndex(f => f.CanId == responseCanId && f.CanDataLen == 8 && f.CanData[0] == 0x21);
                    var find2 =
                       findRecv.FindIndex(f => f.CanId == responseCanId && f.CanDataLen == 8 && f.CanData[0] == 0x22);
                    var find3 =
                       findRecv.FindIndex(f => f.CanId == responseCanId && f.CanDataLen == 8 && f.CanData[0] == 0x23);

                    if (find0 != -1 && find1 != -1 && find2 != -1 && find3 != -3)
                    {
                        var bs = new List<byte>
                            {
                                findRecv[find0].CanData[5],
                                findRecv[find0].CanData[6],
                                findRecv[find0].CanData[7]
                            };

                        for (var i = 1; i < 8; i++)
                            bs.Add(findRecv[find1].CanData[i]);

                        for (var i = 1; i < 8; i++)
                            bs.Add(findRecv[find2].CanData[i]);

                        bs.Add(findRecv[find3].CanData[1]);
                        bs.Add(findRecv[find3].CanData[2]);
                        bs.Add(findRecv[find3].CanData[3]);

                        AppVer = Encoding.ASCII.GetString(bs.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                AppVer = string.Empty;
            }
        }

        [Description("读取HASCO从节点软件零件号")]
        public void ReadAppPartNo()
        {
            AppPartNo = string.Empty;

            if (Can == null)
                return;

            try
            {
                //string reqCanId = "0x7b5";
                //string recvCanId = "0x6b5";

                var requestCanId = _requestCanId;
                var responseCanId = _responseCanId;

                Can.AddDoNotFilterCanId(responseCanId);
                Can.CanRecvDataPackages.Clear();
                for (var i = 0; i < 5; i++)
                {
                    Can.SendStandardCanData(requestCanId, new byte[] { 0x03, 0x22, 0xF1, 0x95, 0xFF, 0xFF, 0xFF, 0xFF });
                    Thread.Sleep(250);
                }
                Can.RemoveDoNotFilterCanId(responseCanId);
                if (Can.CanRecvDataPackages.Count >= 4)
                {
                    var findRecv = Can.CanRecvDataPackages.ToList().FindAll(f => f.CanId == responseCanId);

                    var find0 =
                        findRecv.FindIndex(f => f.CanId == responseCanId && f.CanDataLen == 8 && f.CanData[0] == 0x10);
                    var find1 =
                       findRecv.FindIndex(f => f.CanId == responseCanId && f.CanDataLen == 8 && f.CanData[0] == 0x21);

                    if (find0 != -1 && find1 != -1)
                    {
                        var bs = new List<byte>
                            {
                                findRecv[find0].CanData[5],
                                findRecv[find0].CanData[6],
                                findRecv[find0].CanData[7]
                            };

                        for (var i = 1; i < 7; i++)
                            bs.Add(findRecv[find1].CanData[i]);

                        AppPartNo = Encoding.ASCII.GetString(bs.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                AppPartNo = string.Empty;
            }
        }

        #endregion

        #region 切换灯种

        private LampType ThisLampType { get; set; }

        //[Description("切换为B灯")]
        //public void ChangeToRcmm()
        //{
        //    ThisLampType = LampType.Rcmm;
        //    _requestCanId = 0x7b7;
        //    _responseCanId = 0x6b7;
        //}

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
