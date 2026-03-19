using System;
using System.ComponentModel;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,DC1E后灯-canoe转换代码测试")]
    public sealed class TestDc1ERearLamp : ControllerBase
    {
        public CanBus Can;

        private VectorDbcEmulator VectorEmulator { get; set; }

        public TestDc1ERearLamp(string name)
            : base(name)
        {
            SysConfig(@"D:\Projects\DC1E\后灯\调试资料-20210602\SDB20019_DC1E_BodyExposedCAN_201211.dbc");
        }

        ~TestDc1ERearLamp()
        {
            Dispose();
        }

        [Description("控制版唤醒")]
        public void LampAwake()
        {
            Cem52A = true;
            CemSwitch = true;
        }

        [Description("控制板休眠")]
        public void LampSleep()
        {
            Cem52A = false;
            CemSwitch = false;
        }

        #region STOP 制动灯
        /// <summary>
        /// 制动灯ON
        /// </summary>
        [Description("制动灯ON")]
        public void StopOn()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActnOfLedStopLampActnOfLedStopLamp", 1);
        }

        /// <summary>
        /// 制动灯OFF
        /// </summary>
        [Description("制动灯OFF")]
        public void StopOff()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActnOfLedStopLampActnOfLedStopLamp", 0);
        }
        #endregion

        #region TAIL 位置灯
        /// <summary>
        /// 位置灯ON
        /// </summary>
        [Description("位置灯ON")]
        public void TailOn()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActnOfLedPosnLamp", 1);
        }

        /// <summary>
        /// 位置灯OFF
        /// </summary>
        [Description("位置灯OFF")]
        public void TailOff()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActnOfLedPosnLamp", 0);
        }

        /// <summary>
        /// 位置灯高亮ON
        /// </summary>
        [Description("位置灯高亮ON")]
        public void TailHdOn()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_220", "TrSts", 1);
        }

        /// <summary>
        /// 位置灯高亮ON
        /// </summary>
        [Description("位置灯高亮OFF")]
        public void TailHdOff()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_220", "TrSts", 2);
        }
        #endregion

        #region BUL 倒车灯
        /// <summary>
        /// 倒车灯ON
        /// </summary>
        [Description("倒车灯ON")]
        public void BulOn()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActnOfLedRvsgLamp", 1);
        }

        /// <summary>
        /// 倒车灯OFF
        /// </summary>
        [Description("倒车灯OFF")]
        public void BulOff()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActnOfLedRvsgLamp", 0);
        }
        #endregion

        #region FOG 雾灯
        /// <summary>
        /// 雾灯ON
        /// </summary>
        [Description("雾灯ON")]
        public void FogOn()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActnOfLedReFogLamp", 1);
        }

        /// <summary>
        /// 雾灯OFF
        /// </summary>
        [Description("雾灯OFF")]
        public void FogOff()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActnOfLedReFogLamp", 0);
        }
        #endregion

        #region LOGO

        [Description("LOGO灯ON")]
        public void LogoOn()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_78", "ActnOfLedReFogLamp", 1);
        }

        [Description("LOGO灯OFF")]
        public void LogoOff()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_78", "ActnOfLedReFogLamp", 0);
        }

        #endregion

        #region 动画
        /// <summary>
        /// CHO
        /// 回家动画ON
        /// </summary>
        [Description("回家动画ON")]
        public void WelcomeAnimationOn()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActvnOfWelcomeLi", 1);
        }

        /// <summary>
        /// CHO
        /// 回家动画OFF
        /// </summary>
        [Description("回家动画OFF")]
        public void WelcomeAnimationOff()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActvnOfWelcomeLi", 0);
        }

        /// <summary>
        /// LHO
        /// 离家动画ON
        /// </summary>
        [Description("离家动画ON")]
        public void GoodByeAnimationOn()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActvnOfGoodByeLi", 1);
        }

        /// <summary>
        /// LHO
        /// 离家动画OFF
        /// </summary>
        [Description("离家动画OFF")]
        public void GoodByeAnimationOff()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActvnOfGoodByeLi", 0);
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

        private void InitVariables()
        {
            VectorEmulator.InitMessageVariable("CemBodyExpoCommonFr21", "msg_40");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr50", "msg_20A");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr51", "msg_21A");
            VectorEmulator.InitMessageVariable("CEMBodyExpoCommonFr07", "msg_220");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr25", "msg_78");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr01", "msg_1C");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr02", "msg_StopRi2");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr03", "msg_FogRi2");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr04", "msg_PosnRi2");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr05", "msg_TurnRi2");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr06", "msg_MkrLe1");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr08", "msg_MkrLe2");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr09", "msg_MkrRi1");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr10", "msg_StopLe1");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr11", "msg_StopLe2");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr12", "msg_StopRi1");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr13", "msg_FogLe1");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr14", "msg_FogLe2");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr15", "msg_FogRi1");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr16", "msg_PosnLe1");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr17", "msg_PosnLe2");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr18", "msg_PosnRi1");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr19", "msg_TurnLe1");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr20", "msg_TurnLe2");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr21", "msg_TurnRi1");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr22", "msg_RvsgLe1");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr23", "msg_RvsgLe2");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr24", "msg_RvsgRi1");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr48", "msg_MkrRi2");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr49", "msg_RvsgRi2");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr52", "msg_Logo");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr53", "msg_PosnLe3");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr54", "msg_PosnLe4");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr55", "msg_PosnLe5");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr56", "msg_PosnRi3");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr57", "msg_PosnRi4");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr58", "msg_PosnRi5");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr62", "msg_314");
            VectorEmulator.InitMessageVariable("CemBodyExpoFr63", "msg_315");
            VectorEmulator.InitMessageVariable("CemBodyCanExposedNMFr", "msg_52A");
        }

        private void Prestart()
        {
            UBset(1);
            DHU_UBset(1);
            msg_52A_Set();

            VectorEmulator.SetMessageVariableSignalValue("msg_40", "VehModMngtGlbSafe1_UB", 0x01);
            VectorEmulator.SetMessageVariableSignalValue("msg_40", "VehModMngtGlbSafe1UsgModSts", 0x0B);
        }

        private void Start()
        {
            //Cem52A = true;
            //CemSwitch = true;
            TurnMode = 1;

            VectorEmulator.SetTimer(MsgTimer(), 50);
            VectorEmulator.SetTimer(TurnTimer(), 400);
            VectorEmulator.SetTimer(MsgTimer60(), 60);
            VectorEmulator.SetTimer(NodeTimer(), 500);
            VectorEmulator.SetTimer(MsgTimer20(), 20);
        }

        private void UBset(byte val)
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActnOfLedPosnLamp_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActnOfLedStopLamp_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActnOfLedRvsgLamp_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActnOfLedReFogLamp_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_21A", "ActvnOfIndcr_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_21A", "IndcrSts_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_21A", "EmgyBrkLiIndcrTurn_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_220", "TrSts_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActvnOfGoodByeLi_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_20A", "ActvnOfWelcomeLi_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_78", "ActvnOfReLogoLamp_UB", val);
        }

        private void DHU_UBset(byte val)
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_314", "DHUcontrolLedMkrLampLe1_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_314", "DHUcontrolLedMkrLampLe2_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_314", "DHUcontrolLedMkrLampRi1_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_314", "DHUcontrolLedMkrLampRi2_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_314", "DHUcontrolLedStopLampLe1_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_314", "DHUcontrolLedStopLampLe2_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_314", "DHUcontrolLedStopLampRi1_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_314", "DHUcontrolLedStopLampRi2_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_314", "DHUcontrolmiddlebrakelight_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_314", "DHUcontrolmiddlecorneringlamp_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_314", "DHUcontrolmiddlepositionlight_UB", val);

            VectorEmulator.SetMessageVariableSignalValue("msg_315", "DHUcontrolCrossreversinglight_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_315", "DHUcontrolleftbrakelightLamp_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_315", "DHUcontrolleftcorneringlamp_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_315", "DHUcontrolleftpositionlamp_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_315", "DHUcontrolrightbrakelight_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_315", "DHUcontrolrightcorneringlamp_UB", val);
            VectorEmulator.SetMessageVariableSignalValue("msg_315", "DHUcontrolrightpositionLamp_UB", val);
        }

        private void msg_52A_Set()
        {
            VectorEmulator.SetMessageVariableByteValue("msg_52A", 0, 0x2A);
            VectorEmulator.SetMessageVariableByteValue("msg_52A", 1, 0x40);
            VectorEmulator.SetMessageVariableByteValue("msg_52A", 2, 0xFF);
            VectorEmulator.SetMessageVariableByteValue("msg_52A", 3, 0xFF);
            VectorEmulator.SetMessageVariableByteValue("msg_52A", 4, 0xFF);
            VectorEmulator.SetMessageVariableByteValue("msg_52A", 5, 0xFF);
            VectorEmulator.SetMessageVariableByteValue("msg_52A", 6, 0xFF);
            VectorEmulator.SetMessageVariableByteValue("msg_52A", 7, 0xFF);
        }

        #region System timer
        private Action MsgTimer()
        {
            return () =>
            {
                if (Cem52A)
                {
                    VectorEmulator.OutPut("msg_52A", Can);
                }

                if (CemSwitch)
                {
                    VectorEmulator.OutPut("msg_40", Can);
                    VectorEmulator.OutPut("msg_78", Can);

                    var actnOfLedStopLampCntr =
                        VectorEmulator.GetMessageVariableSignalValue(
                        "msg_20A", "ActnOfLedStopLampCntr");
                    if (actnOfLedStopLampCntr == 0x0F)
                    {
                        actnOfLedStopLampCntr = 0x00;
                        VectorEmulator.SetMessageVariableSignalValue(
                           "msg_20A", "ActnOfLedStopLampCntr", actnOfLedStopLampCntr);
                    }

                    var actnOfLedStopLampChks = (byte)0x00;
                    var actnOfLedStopLampActnOfLedStopLamp =
                        VectorEmulator.GetMessageVariableSignalValue("msg_20A", "ActnOfLedStopLampActnOfLedStopLamp");
                    switch (StopErr)
                    {
                        case 0:
                            actnOfLedStopLampChks = CalculateCrc8(
                                1091, actnOfLedStopLampCntr, actnOfLedStopLampActnOfLedStopLamp);
                            break;

                        case 1:
                            break;

                        case 2:
                            actnOfLedStopLampChks = 0;
                            break;

                        case 3:
                            actnOfLedStopLampChks = CalculateCrc8(
                                1091, actnOfLedStopLampCntr, actnOfLedStopLampActnOfLedStopLamp);
                            break;

                        case 4:
                            actnOfLedStopLampChks = CalculateCrc8(
                                1091, actnOfLedStopLampCntr, actnOfLedStopLampActnOfLedStopLamp);
                            break;
                    }
                    VectorEmulator.SetMessageVariableSignalValue(
                           "msg_20A", "ActnOfLedStopLampChks", actnOfLedStopLampChks);
                    VectorEmulator.OutPut("msg_20A", Can);

                    switch (StopErr)
                    {
                        case 0:
                            actnOfLedStopLampCntr++;
                            break;

                        case 1:
                            actnOfLedStopLampCntr++;
                            break;

                        case 2:
                            break;

                        case 3:
                            break;

                        case 4:
                            actnOfLedStopLampCntr++;
                            break;
                    }
                    VectorEmulator.SetMessageVariableSignalValue(
                           "msg_20A", "ActnOfLedStopLampCntr", actnOfLedStopLampCntr);
                }
            };
        }

        private Action TurnTimer()
        {
            return () =>
            {
                if (TurnSign == 1)
                {
                    VectorEmulator.SetMessageVariableSignalValue(
                        "msg_21A", "ActvnOfIndcrIndcrOut", (byte)TurnMode);
                    VectorEmulator.SetMessageVariableSignalValue(
                        "msg_21A", "IndcrSts", (byte)TurnMode);

                    VectorEmulator.SetMessageVariableSignalValue(
                        "msg_21A", "EmgyBrkLiIndcrTurn", EmgyMode ? (byte)0x01 : (byte)0x02);
                }
                else
                {
                    VectorEmulator.SetMessageVariableSignalValue(
                        "msg_21A", "ActvnOfIndcrIndcrOut", 0x00);
                    VectorEmulator.SetMessageVariableSignalValue(
                        "msg_21A", "IndcrSts", 0x00);

                    VectorEmulator.SetMessageVariableSignalValue(
                       "msg_21A", "EmgyBrkLiIndcrTurn", EmgyMode ? (byte)0x02 : (byte)0x01);
                }
                TurnSign = 1 - TurnSign;
            };
        }

        private Action MsgTimer60()
        {
            return () =>
            {
                if (CemSwitch)
                    VectorEmulator.OutPut("msg_220", Can);
            };
        }

        private Action NodeTimer()
        {
            return () =>
            {

            };
        }

        private Action MsgTimer20()
        {
            return () =>
            {
                var actvnOfIndcrIndcrOutCntr = VectorEmulator.GetMessageVariableSignalValue(
                    "msg_21A", "ActvnOfIndcrIndcrOutCntr");
                if (actvnOfIndcrIndcrOutCntr == 0x0F)
                {
                    actvnOfIndcrIndcrOutCntr = 0x00;
                    VectorEmulator.SetMessageVariableSignalValue(
                        "msg_21A", "ActvnOfIndcrIndcrOutCntr", actvnOfIndcrIndcrOutCntr);
                }

                var actvnOfIndcrIndcrOutChks = (byte)0x00;
                var actvnOfIndcrIndcrOut =
                    VectorEmulator.GetMessageVariableSignalValue("msg_21A", "ActvnOfIndcrIndcrOut");

                switch (TurnErr)
                {
                    case 0:
                        actvnOfIndcrIndcrOutChks =
                            CalculateCrc8(158, actvnOfIndcrIndcrOutCntr, actvnOfIndcrIndcrOut);
                        break;

                    case 1:
                        break;

                    case 2:
                        actvnOfIndcrIndcrOutChks = 0x00;
                        break;

                    case 3:
                        actvnOfIndcrIndcrOutChks =
                            CalculateCrc8(158, actvnOfIndcrIndcrOutCntr, actvnOfIndcrIndcrOut);
                        break;

                    case 4:
                        actvnOfIndcrIndcrOutChks =
                            CalculateCrc8(158, actvnOfIndcrIndcrOutCntr, actvnOfIndcrIndcrOut);
                        break;
                }

                VectorEmulator.SetMessageVariableSignalValue(
                    "msg_21A", "ActvnOfIndcrIndcrOutChks", actvnOfIndcrIndcrOutChks);

                if (CemSwitch)
                    VectorEmulator.OutPut("msg_21A", Can);

                switch (TurnErr)
                {
                    case 0:
                        actvnOfIndcrIndcrOutCntr++;
                        break;

                    case 1:
                        actvnOfIndcrIndcrOutCntr++;
                        break;

                    case 2:
                        break;

                    case 3:
                        break;

                    case 4:
                        actvnOfIndcrIndcrOutCntr++;
                        break;
                }

                VectorEmulator.SetMessageVariableSignalValue(
                       "msg_21A", "ActvnOfIndcrIndcrOutCntr", actvnOfIndcrIndcrOutCntr);
            };
        }
        #endregion

        #region System variable
        private bool Cem52A { get; set; }
        private bool CemSwitch { get; set; }
        private int StopErr { get; set; }
        private int TurnErr { get; set; }
        private int TurnSign { get; set; }
        private int TurnMode { get; set; }
        private bool EmgyMode { get; set; }
        #endregion

        private static byte CalculateCrc8(
            int dataId, byte counter, byte lamp)
        {
            var crc = (byte)0x00;

            //  current CRC8 XOR data in
            crc ^= (byte)(dataId % 256);
            for (var j = 0; j < 8; j++)
            {
                // check if MSB is 1
                if ((crc & 0x80) > 0x0)
                {
                    crc <<= 0x01;
                    crc ^= 0x1D;
                }
                else
                {
                    crc <<= 0x01;
                }
            }

            crc ^= (byte)(dataId / 256);
            for (var j = 0; j < 8; j++)
            {
                // check if MSB is 1
                if ((crc & 0x80) > 0x0)
                {
                    crc <<= 0x01;
                    crc ^= 0x1D;
                }
                else
                {
                    crc <<= 0x01;
                }
            }

            crc ^= counter;
            for (var j = 0; j < 8; j++)
            {
                // check if MSB is 1
                if ((crc & 0x80) > 0x0)
                {
                    crc <<= 1;
                    crc ^= 0x1D;
                }
                else
                {
                    crc <<= 1;
                }
            }

            crc ^= lamp;
            for (var j = 0; j < 8; j++)
            {
                /*check if MSB is 1*/
                if ((crc & 0x80) > 0x0)
                {
                    crc <<= 1;
                    crc ^= 0x1D;
                }
                else
                {
                    crc <<= 1;
                }
            }

            crc ^= 0x00;

            return crc;
        }
        #endregion
    }
}
