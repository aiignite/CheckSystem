using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,CD764高配前灯")]
    public sealed class Cd764GpFrontLamp : ControllerBase
    {
        public CanBus Can;
        private VectorDbcEmulator VectorEmulator { get; set; }

        public Cd764GpFrontLamp(string name)
            : base(name)
        {
            SysConfig(
                 Directory.GetCurrentDirectory() + @"\ControllerConfig\Y2022_H2_HSCAN_HCM_v21.02_Working.dbc");

            _turnLghtLeftDRq2 = 1;
            _turnLghtRightDRq2 = 1;
            _ignitionStatus4 = 4;

            //_ignitionStatus4 = 1;
            //_wfSubstateDStat3 = 1;
            //_wfSuperstateDStat2 = 1;
        }

        ~Cd764GpFrontLamp()
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

        [Description("SpotEnOn")]
        public void SpotEnOn()
        {
            _headLampLoFlOnBStat1 = 0x01;
            _headLampLoFrOnBStat1 = 0x01;

            _hbhlBeamDRq3 = 0x03;
            _hiBeamBrghtRightDRq5 = 0x1C;
            _hiBeamBrghtLeftDRq5 = 0x1C;
        }

        [Description("SpotEnOff")]
        public void SpotEnOff()
        {
            _headLampLoFlOnBStat1 = 0x00;
            _headLampLoFrOnBStat1 = 0x00;

            _hbhlBeamDRq3 = 0x00;
            _hiBeamBrghtRightDRq5 = 0x00;
            _hiBeamBrghtLeftDRq5 = 0x00;
        }

        private int _ledIndex;
        private int _sliceIndex;

        [Description("打开单颗LED")]
        public void LedOn(string index)
        {
            int value;
            if (!int.TryParse(index, out value))
                return;

            _sliceIndex = -1;
            _ledIndex = value;

            //if (value >= 1 || value <= 12)
            //{
            //    var degrees = (float)0.0;

            //    switch (value)
            //    {
            //        case 1:
            //            degrees = 7;
            //            break;

            //        case 2:
            //            degrees = 5;
            //            break;

            //        case 3:
            //            degrees = 3;
            //            break;

            //        case 4:
            //            degrees = 1;
            //            break;

            //        case 5:
            //            degrees = -1;
            //            break;

            //        case 6:
            //            degrees = -4;
            //            break;

            //        case 7:
            //            degrees = -4;
            //            break;

            //        case 8:
            //            degrees = -5;
            //            break;

            //        case 9:
            //            degrees = -7;
            //            break;

            //        case 10:
            //            degrees = -9;
            //            break;

            //        case 11:
            //            degrees = -11;
            //            break;

            //        case 12:
            //            degrees = -17;
            //            break;
            //    }

            //    spot1 = (int)((degrees + 35) / 0.04);
            //}
        }

        [Description("关闭单颗LED")]
        public void LedOff()
        {
            _ledIndex = -1;
        }

        [Description("打开SLICE")]
        public void SliceOn(string index)
        {
            int value;
            if (!int.TryParse(index, out value))
                return;

            _sliceIndex = value;
            _ledIndex = -1;
        }

        [Description("关闭SLICE")]
        public void SliceOff()
        {
            _sliceIndex = -1;
        }

        //[Description("打开左近光")]
        //public void LeftLowBeamOn()
        //{
        //    SwitchLowBeam(true, true);
        //}

        //[Description("关闭左近光")]
        //public void LeftLowBemOff()
        //{
        //    SwitchLowBeam(true, false);
        //}

        //[Description("打开右近光")]
        //public void RightLowBeamOn()
        //{
        //    SwitchLowBeam(false, true);
        //}

        //[Description("关闭右近光")]
        //public void RightLowBeamOff()
        //{
        //    SwitchLowBeam(false, false);
        //}

        //private void SwitchLowBeam(bool isLeft, bool isOn)
        //{
        //    if (isLeft)
        //        _headLampLoFlOnBStat1 = isOn ? (byte)0x01 : (byte)0x00;
        //    else
        //        _headLampLoFrOnBStat1 = isOn ? (byte)0x01 : (byte)0x00;
        //}

        [Description("打开远光-Flash to pass")]
        public void HighBeamFlashToPass()
        {
            SliceOff();
            LedOff();
            _hbhlBeamDRq3 = 0x01;
        }

        //[Description("打开远光-Manual high beam")]
        //public void HighBeamManualHighBeam()
        //{
        //    _hbhlBeamDRq3 = 0x02;
        //}

        //[Description("打开远光-Auto high beam")]
        //public void HgihBeamAutoHighBeam()
        //{
        //    _hbhlBeamDRq3 = 0x03;
        //}

        //[Description("打开远光-Wig Wag")]
        //public void HighBeamWigWag()
        //{
        //    _hbhlBeamDRq3 = 0x04;
        //}

        //[Description("打开远光-Placeholder")]
        //public void HighBeamPlaceholder()
        //{
        //    _hbhlBeamDRq3 = 0x05;
        //}

        [Description("关闭远光")]
        public void HighBeamOff()
        {
            _hbhlBeamDRq3 = 0x00;
            SliceOff();
            LedOff();
        }

        [Description("打开左DRL")]
        public void LeftDrlOn()
        {
            SwitchDrlPl(true, true, true);
        }

        [Description("打开左PL")]
        public void LeftPlOn()
        {
            SwitchDrlPl(true, true, false);
        }

        [Description("关闭左DRL/PL")]
        public void LeftDrlPlOff()
        {
            SwitchDrlPl(true, false, false);
        }

        [Description("打开右DRL")]
        public void RightDrlOn()
        {
            SwitchDrlPl(false, true, true);
        }

        [Description("打开右PL")]
        public void RightPlOn()
        {
            SwitchDrlPl(false, true, false);
        }

        [Description("关闭右DRL/PL")]
        public void RightDrlPlOff()
        {
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
            _turnLghtLeftDRq2 = 0;
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
            _wfSuperstateDStat2 = 0x000;
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
                    _headLghtSwtchDStat2 = 0x00;
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
                        _headLghtSwtchDStat2 = 0x01;
                    }
                    else
                    {
                        _drlPos1ActvLfRq2 = 0x01;
                        _drlPos2ActvLfRq2 = 0x01;
                        _drlPos3ActvLfRq2 = 0x01;
                        _drlPos4ActvLfRq2 = 0x01;

                        _drlPosActvFadeLfRq4 = 0x00;
                        _headLghtSwtchDStat2 = 0x01;
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
                    _headLghtSwtchDStat2 = 0x00;
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
                        _headLghtSwtchDStat2 = 0x01;
                    }
                    else
                    {
                        _drlPos1ActvRfRq2 = 0x01;
                        _drlPos2ActvRfRq2 = 0x01;
                        _drlPos3ActvRfRq2 = 0x01;
                        _drlPos4ActvRfRq2 = 0x01;

                        _drlPosActvFadeRfRq4 = 0x00;
                        _headLghtSwtchDStat2 = 0x01;
                    }
                }
            }
        }

        #region Code from CANoe and DBC

        private void SysConfig(string dbcFilePath)
        {
            VectorEmulator =
                new VectorDbcEmulator(new[] { dbcFilePath });
            InitVariables();
            Start();
        }

        private void InitVariables()
        {
            VectorEmulator.InitMessageVariable("VehicleOperatingModes", "LDM12_VehicleOperatingModes", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("Spot1LeftHl", "LDM12_Spot1LeftHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("Spot1RightHl", "LDM12_Spot1RightHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("Spot2LeftHl", "LDM12_Spot2LeftHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("Spot2RightHl", "LDM12_Spot2RightHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("Spot3LeftHl", "LDM12_Spot3LeftHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("Spot3RightHl", "LDM12_Spot3RightHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("HiBeamRq", "LDM12_HiBeamRq", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("LvlCmd", "LDM12_LvlCmd", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("AFSRq", "LDM12_AFSRq", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("SsblLeftHl", "LDM12_SsblLeftHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("SsblRightHl", "LDM12_SsblRightHl", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("E2E_BCMtoLDCM", "LDM12_E2E_BCMtoLDCM", "Y2022_H2_HSCAN_HCM_v21.02_Working");
            VectorEmulator.InitMessageVariable("BaseFeaturesActvRq", "LDM12_BaseFeaturesActvRq", "Y2022_H2_HSCAN_HCM_v21.02_Working");
        }

        private void Start()
        {
            VectorEmulator.SetTimer(T_30ms_Event(), 30);
            VectorEmulator.SetTimer(T_20ms_Event(), 20);
            VectorEmulator.SetTimer(T_10ms_Event(), 10);
            VectorEmulator.SetTimer(T_100ms_Event(), 100);
            VectorEmulator.SetTimer(T_1ms_update_data(), 100);
            //VectorEmulator.SetTimer(T_2s_Event(), 2000);
        }

        #region System timer

        private Action T_30ms_Event()
        {
            return () =>
            {
                if (!_switchOnOff)
                    return;

                //VectorEmulator.OutPut("LDM12_LvlCmd", Can);
                //VectorEmulator.OutPut("LDM12_AFSRq", Can);
                //VectorEmulator.OutPut("LDM12_SsblLeftHl", Can);
                //VectorEmulator.OutPut("LDM12_SsblRightHl", Can);

                VectorEmulator.OutPut(
                    new[] { "LDM12_LvlCmd", "LDM12_AFSRq", "LDM12_SsblLeftHl", "LDM12_SsblRightHl" }, Can);
            };
        }

        private Action T_20ms_Event()
        {
            return () =>
            {
                if (!_switchOnOff)
                    return;

                if (Can != null)
                {
                    if (_ledIndex != -1 && _sliceIndex == -1)
                    {
                        switch (_ledIndex)
                        {
                            case 1:
                                Can.SendStandardCanData(0x60, new byte[] { 0x80, 0x14, 0xB6, 0x0D, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x80, 0x14, 0xB6, 0x0D, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0xC0, 0x14, 0x52, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0xC0, 0x14, 0x52, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 2:
                                Can.SendStandardCanData(0x60, new byte[] { 0x80, 0x14, 0xB5, 0xF4, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x80, 0x14, 0xB5, 0xF4, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0xC0, 0x11, 0xFA, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0xC0, 0x11, 0xFA, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 3:
                                Can.SendStandardCanData(0x60, new byte[] { 0x80, 0x14, 0xB5, 0xDB, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x80, 0x14, 0xB5, 0xDB, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0xC0, 0x11, 0x32, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0xC0, 0x11, 0x32, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 4:
                                Can.SendStandardCanData(0x60, new byte[] { 0x80, 0x14, 0xB5, 0xc2, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x80, 0x14, 0xB5, 0xc2, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0xC0, 0x10, 0x6a, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0xC0, 0x10, 0x6a, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 5:
                                Can.SendStandardCanData(0x60, new byte[] { 0x80, 0x14, 0xB5, 0xA9, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x80, 0x14, 0xB5, 0xA9, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0xC0, 0x0f, 0xa2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0xC0, 0x0f, 0xa2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 6:
                                Can.SendStandardCanData(0x60, new byte[] { 0x80, 0x14, 0xB5, 0x83, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x80, 0x14, 0xB5, 0x83, 0xC0, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0xC0, 0x0E, 0xDA, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0xC0, 0x0E, 0xDA, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 7:
                                Can.SendStandardCanData(0x60, new byte[] { 0x80, 0x14, 0xB5, 0x83, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x80, 0x14, 0xB5, 0x83, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0xC0, 0x0F, 0x3E, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0xC0, 0x0F, 0x3E, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 8:
                                Can.SendStandardCanData(0x60, new byte[] { 0x80, 0x14, 0xB5, 0x77, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x80, 0x14, 0xB5, 0x77, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0xC0, 0x0E, 0x12, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0xC0, 0x0E, 0x12, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 9:
                                Can.SendStandardCanData(0x60, new byte[] { 0x80, 0x14, 0xB5, 0x5E, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x80, 0x14, 0xB5, 0x5E, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0xC0, 0x0d, 0x4a, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0xC0, 0x0d, 0x4a, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 10:
                                Can.SendStandardCanData(0x60, new byte[] { 0x80, 0x14, 0xB5, 0x45, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x80, 0x14, 0xB5, 0x45, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0xC0, 0x0c, 0x82, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0xC0, 0x0c, 0x82, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 11:
                                Can.SendStandardCanData(0x60, new byte[] { 0x80, 0x14, 0xB5, 0x2c, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x80, 0x14, 0xB5, 0x2c, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0xC0, 0x0b, 0xba, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0xC0, 0x0b, 0xba, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 12:
                                Can.SendStandardCanData(0x60, new byte[] { 0x80, 0x14, 0xB4, 0xE1, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x80, 0x14, 0xB4, 0xE1, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0xC0, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0xC0, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;
                        }
                    }
                    else if (_ledIndex == -1 && _sliceIndex != -1)
                    {
                        switch (_sliceIndex)
                        {
                            case 1:
                                Can.SendStandardCanData(0x60, new byte[] { 0x40, 0x0C, 0x1D, 0x9C, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x40, 0x0C, 0x1D, 0x9C, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0x00, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0x00, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 2:
                                Can.SendStandardCanData(0x60, new byte[] { 0x40, 0x0C, 0xE5, 0xA9, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x40, 0x0C, 0xE5, 0xA9, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0x00, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0x00, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 3:
                                Can.SendStandardCanData(0x60, new byte[] { 0x40, 0x0C, 0x49, 0xAE, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x40, 0x0C, 0x49, 0xAE, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0x00, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0x00, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 4:
                                Can.SendStandardCanData(0x60, new byte[] { 0x40, 0x0C, 0xAD, 0xCE, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x40, 0x0C, 0xAD, 0xCE, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0x00, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0x00, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 5:
                                Can.SendStandardCanData(0x60, new byte[] { 0x40, 0x0C, 0x49, 0xB5, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x40, 0x0C, 0x49, 0xB5, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0x00, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0x00, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;

                            case 6:
                                Can.SendStandardCanData(0x60, new byte[] { 0x40, 0x0C, 0x49, 0xB5, 0x40, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x61, new byte[] { 0x40, 0x0C, 0x49, 0xB5, 0x40, 0x16, 0x40, 0x00 });

                                Can.SendStandardCanData(0x64, new byte[] { 0x00, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                Can.SendStandardCanData(0x65, new byte[] { 0x00, 0x0A, 0xF2, 0x96, 0xC0, 0x16, 0x40, 0x00 });
                                break;
                        }
                    }
                    else if (_ledIndex == 0 && _sliceIndex == 0)
                    {
                        //VectorEmulator.OutPut("LDM12_Spot1LeftHl", Can);
                        //VectorEmulator.OutPut("LDM12_Spot1RightHl", Can);
                        //VectorEmulator.OutPut("LDM12_Spot2LeftHl", Can);
                        //VectorEmulator.OutPut("LDM12_Spot2RightHl", Can);
                        //VectorEmulator.OutPut("LDM12_Spot3LeftHl", Can);
                        //VectorEmulator.OutPut("LDM12_Spot3RightHl", Can);

                        VectorEmulator.OutPut(new string[] { "LDM12_Spot1LeftHl", "LDM12_Spot1RightHl", "LDM12_Spot2LeftHl", "LDM12_Spot2RightHl", "LDM12_Spot3LeftHl", "LDM12_Spot3RightHl" }, Can);
                    }
                }

                //VectorEmulator.OutPut("LDM12_HiBeamRq", Can);
                //VectorEmulator.OutPut("LDM12_BaseFeaturesActvRq", Can);

                VectorEmulator.OutPut(new string[] { "LDM12_HiBeamRq", "LDM12_BaseFeaturesActvRq" }, Can);
            };
        }

        private Action T_10ms_Event()
        {
            return () =>
            {
                if (_switchOnOff)
                    VectorEmulator.OutPut("LDM12_VehicleOperatingModes", Can);
            };
        }

        private Action T_1ms_update_data()
        {
            return () =>
            {
                if (!_switchOnOff)
                    return;

                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_LvlCmd", "HeadLghtLvlLeftRef_D_Rq", _headLghtLvlLeftRefDRq3);
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_LvlCmd", "HeadLghtLvlRghtRef_D_Rq", _headLghtLvlRightRefDRq3);
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_LvlCmd", "HeadLghtLvlLeft_No_Rq", _headLghtLvlLeftNoRq12);
                VectorEmulator.SetMessageVariableSignalValue(
                    "LDM12_LvlCmd", "HeadLghtLvlRight_No_Rq", _headLghtLvlRightNoRq12);
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

                VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "HBHL_Beam_D_Rq", _hbhlBeamDRq3);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "HiBeamHWmde_SED_Rq", _hiBeamHWmdeSedRq2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "HiBeamBrghtRight_D_Rq", _hiBeamBrghtRightDRq5);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "HiBeamFadeVRight_T_Rq", _hiBeamFadeVRightTRq5);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "HiBeamBrghtLeft_D_Rq", _hiBeamBrghtLeftDRq5);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_HiBeamRq", "HiBeamFadeVLeft_T_Rq", _hiBeamFadeVLeftTRq5);

                VectorEmulator.SetMessageVariableSignalValue("LDM12_VehicleOperatingModes", "Ignition_Status", _ignitionStatus4);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_VehicleOperatingModes", "PwPckTq_D_Stat", _pwPckTqDStat2);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_VehicleOperatingModes", "ElPw_D_Stat", _elPwDStat3);
                VectorEmulator.SetMessageVariableSignalValue("LDM12_VehicleOperatingModes", "HCMSleepyTime_B_Stat", _hcmSleepyTime1);

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
            };
        }

        private Action T_100ms_Event()
        {
            return () =>
            {
                if (_switchOnOff)
                    VectorEmulator.OutPut("LDM12_E2E_BCMtoLDCM", Can);
            };
        }

        private Action T_2s_Event()
        {
            return () => { };
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

        [Description("R/W,HBHL_Beam_D_Rq_3")]
        private byte _hbhlBeamDRq3;

        [Description("R/W,HiBeamHWmde_SED_Rq_2")]
        private byte _hiBeamHWmdeSedRq2;

        [Description("R/W,HiBeamBrghtRight_D_Rq_5")]
        private byte _hiBeamBrghtRightDRq5;

        [Description("R/W,HiBeamFadeVRight_T_Rq_5")]
        private byte _hiBeamFadeVRightTRq5;

        [Description("R/W,HiBeamBrghtLeft_D_Rq_5")]
        private byte _hiBeamBrghtLeftDRq5;

        [Description("R/W,HiBeamFadeVLeft_T_Rq_5")]
        private byte _hiBeamFadeVLeftTRq5;

        [Description("R/W,Ignition_Status_4")]
        private byte _ignitionStatus4;

        [Description("R/W,PwPckTq_D_Stat_2")]
        private byte _pwPckTqDStat2;

        [Description("R/W,ElPw_D_Stat_3")]
        private byte _elPwDStat3;

        [Description("R/W,HCMSleepyTime_1")]
        private byte _hcmSleepyTime1;

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

        #endregion

        #endregion

        #region 版本信息

        [Description("R,读取HASCO从节点软件版本号")]
        public string AppVer;

        [Description("R,读取HASCO从节点软件零件号")]
        public string AppPartNo;

        [Description("读取HASCO从节点软件版本号")]
        public void ReadAppVer(string reqCanId, string recvCanId)
        {
            AppVer = string.Empty;

            if (Can == null)
                return;

            try
            {
                var requestCanId = Convert.ToUInt32(reqCanId, 16);
                var responseCanId = Convert.ToUInt32(recvCanId, 16);

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
        public void ReadAppPartNo(string reqCanId, string recvCanId)
        {
            AppPartNo = string.Empty;

            if (Can == null)
                return;

            try
            {
                //string reqCanId = "0x7b5";
                //string recvCanId = "0x6b5";

                var requestCanId = Convert.ToUInt32(reqCanId, 16);
                var responseCanId = Convert.ToUInt32(recvCanId, 16);

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

                    if (find0 != -1 && find1 != -1 )
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
    }
}
