using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,LCH-多像素")]
    public sealed class LchWithMultiplePixel : ControllerBase
    {
        public CanBus Can;
        private VectorDbcEmulator VectorEmulator { get; set; }
        private string CanDb1 { get; set; }
        private string CanDb2 { get; set; }

        public LchWithMultiplePixel(string name)
            : base(name)
        {
            var files = new[]
            {
                Directory.GetCurrentDirectory() + @"\ControllerConfig\AFSCAN_KMatrix_V1.11F_20150528_MA.dbc",
                Directory.GetCurrentDirectory() + @"\ControllerConfig\AFSCAN_KMatrix_V1.10F_20150417_DW_EEK4_skoito.dbc",
            };

            //var files = new[]
            //{
            //    @"D:\Projects\点灯设备及LED检测\科博达相关\LCH-32pixel - 20200515\dbc\AFSCAN_KMatrix_V1.11F_20150528_MA.dbc",
            //    @"D:\Projects\点灯设备及LED检测\科博达相关\LCH-32pixel - 20200515\dbc\AFSCAN_KMatrix_V1.10F_20150417_DW_EEK4_skoito.dbc"
            //};

            CanDb1 =
                new FileInfo(
                    files[0]).Name.Remove(new FileInfo(files[0]).Name.Length - 4, 4);
            CanDb2 =
                new FileInfo(
                    files[1]).Name.Remove(new FileInfo(files[1]).Name.Length - 4, 4);

            SysConfig(files);
        }

        [Description("角灯ON")]
        public void CornerOn()
        {
            EnvLCorninglight = 1;
            EnvRCorninglight = 1;
        }

        [Description("角灯OFF")]
        public void CornerOff()
        {
            EnvLCorninglight = 0;
            EnvRCorninglight = 0;
        }

        [Description("转向灯ON")]
        public void TurnOn()
        {
            EnvLcmBlinkerL = 1;
            EnvLcmBlinkerCycleL = 1;
            EnvLcmBlinkerR = 1;
            EnvLcmBlinkerCycleR = 1;
        }

        [Description("转向灯OFF")]
        public void TurnOff()
        {
            EnvLcmBlinkerL = 0;
            EnvLcmBlinkerCycleL = 0;
            EnvLcmBlinkerR = 0;
            EnvLcmBlinkerCycleR = 0;
        }

        [Description("日行灯ON")]
        public void DrlOn()
        {
            EnvLcmTflL = 1;
            EnvLcmTflR = 1;
            EnvLcmPoL = 0;
            EnvLcmPoR = 0;
        }

        [Description("日行灯OFF")]
        public void DrlOff()
        {
            EnvLcmTflL = 0;
            EnvLcmTflR = 0;
            EnvLcmPoL = 0;
            EnvLcmPoR = 0;
        }

        [Description("位置灯ON")]
        public void PlOn()
        {
            EnvLcmTflL = 0;
            EnvLcmTflR = 0;
            EnvLcmPoL = 1;
            EnvLcmPoR = 1;
        }

        [Description("位置灯OFF")]
        public void PlOff()
        {
            EnvLcmTflL = 0;
            EnvLcmTflR = 0;
            EnvLcmPoL = 0;
            EnvLcmPoR = 0;
        }

        //[Description("MotorLeftLimit")]
        //public void MotorLeftLimit()
        //{
        //    VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 0, 0x3E);
        //    VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 1, 0x04);
        //    VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 3, 0x3E);
        //    VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 4, 0x04);
        //}

        //[Description("MotorRightLimit")]
        //public void MotorRightLimit()
        //{
        //    VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 0, 0xA0);
        //    VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 1, 0x0F);
        //    VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 3, 0xA0);
        //    VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 4, 0x0F);
        //}

        //[Description("MotorHome")]
        //public void MotorHome()
        //{
        //    VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 0, 0xFE);
        //    VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 1, 0x07);
        //    VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 3, 0xFE);
        //    VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 4, 0x07);
        //}

        //private bool _isAfsInit;

        //[Description("MotorInit")]
        //public void MotorInit()
        //{
        //    if (!_isAfsInit)
        //    {
        //        VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 0, 0xFE);
        //        VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 1, 0x27);
        //        VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 3, 0xFE);
        //        VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 4, 0x27);
        //        _isAfsInit = true;
        //    }
        //    else
        //    {
        //        VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 0, 0x00);
        //        VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 1, 0x00);
        //        VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 3, 0x00);
        //        VectorEmulator.SetMessageVariableByteValue("AfsSollFkt_msg", 4, 0x00);
        //        _isAfsInit = false;
        //    }
        //}

        ~LchWithMultiplePixel()
        {
            Dispose();
        }

        protected override void Dispose(bool isDisposeing)
        {
            if (!isDisposeing)
                return;
            if (VectorEmulator == null)
                return;
            VectorEmulator.Dispose();
        }

        #region Code from CANoe and DBC

        public void SysConfig(string[] dbcFilePath)
        {
            VectorEmulator =
                new VectorDbcEmulator(dbcFilePath);
            InitVariables();
            Start();
        }

        private void InitVariables()
        {
            VectorEmulator.InitMessageVariable("LED_Soll_Fkt0", "m_LED_Soll_Fkt0", CanDb1);
            VectorEmulator.InitMessageVariable("LED_Soll_Fkt2", "m_LED_Soll_Fkt2", CanDb1);

            VectorEmulator.InitMessageVariable("MXB_Soll_Fkt0", "m_MXB_Soll_Fkt0", CanDb1);
            VectorEmulator.InitMessageVariable("MXB_Soll_Fkt1", "m_MXB_Soll_Fkt1", CanDb1);
            VectorEmulator.InitMessageVariable("MXB_Soll_Fkt2", "m_MXB_Soll_Fkt2", CanDb1);
            VectorEmulator.InitMessageVariable("MXB_Soll_Fkt3", "m_MXB_Soll_Fkt3", CanDb1);
            VectorEmulator.InitMessageVariable("MXB_Soll_Fkt4", "m_MXB_Soll_Fkt4", CanDb1);

            VectorEmulator.InitMessageVariable("MXB_Soll_Fkt9", "m_MXB_Soll_Fkt9", CanDb1);
            VectorEmulator.InitMessageVariable("MXB_Soll_Fkt10", "m_MXB_Soll_Fkt10", CanDb1);
            VectorEmulator.InitMessageVariable("MXB_Soll_Fkt11", "m_MXB_Soll_Fkt11", CanDb1);
            VectorEmulator.InitMessageVariable("MXB_Soll_Fkt12", "m_MXB_Soll_Fkt12", CanDb1);
            VectorEmulator.InitMessageVariable("MXB_Soll_Fkt13", "m_MXB_Soll_Fkt13", CanDb1);

            VectorEmulator.InitMessageVariable("Motor_14", "m_Motor_14", CanDb1);
            VectorEmulator.InitMessageVariable("Klemmen_Status_01", "m_Klemmen_Status_01", CanDb1);
            VectorEmulator.InitMessageVariable("Airbag_01", "m_Airbag_01", CanDb1);
            VectorEmulator.InitMessageVariable("Licht_Anf_01", "my_licht_Anf_01", CanDb2);
            VectorEmulator.InitMessageVariable("ESP_21", "m_ESP21", CanDb1);

            VectorEmulator.InitMessageVariable("LWR_Soll_Fkt", "LwrSollFkt_msg", CanDb1);
            VectorEmulator.InitMessageVariable("AFS_Soll_Fkt", "AfsSollFkt_msg", CanDb1);

            VectorEmulator.InitMessageVariable("LED_Soll_Fkt1", "m_LED_Soll_Fkt1", CanDb1);
            VectorEmulator.InitMessageVariable("LCM_Soll_Fkt", "my_LCM_Soll_Fkt", CanDb1);
        }

        private void Start()
        {
            VectorEmulator.SetTimer(timer_0x15F_20ms(), 20);
            VectorEmulator.SetTimer(Timer20Ms(), 20);
            VectorEmulator.SetTimer(Timer200Ms(), 200);
            VectorEmulator.SetTimer(Licht_Anf_01_100ms_Timer(), 200);
            VectorEmulator.SetTimer(msTimer_LwrSollFkt(), 10);
            VectorEmulator.SetTimer(msTimer_AfsSollFkt(), 10);
            VectorEmulator.SetTimer(Klemmen_timer(), 50);
            VectorEmulator.SetTimer(Esp21Timer(), 100);
            VectorEmulator.SetTimer(LCM_Soll_Fkt_100ms_Timer(), 100);
        }

        #region System timer

        private Action timer_0x15F_20ms()
        {
            return () =>
            {
                VectorEmulator.OutPut("m_LED_Soll_Fkt2", Can);
            };
        }

        private Action Timer20Ms()
        {
            return () =>
            {
                if (GLeftTxEn || GRightTxEn)
                {
                    if (GLeftTxEn)
                        VectorEmulator.SetMessageVariableSignalValue("m_LED_Soll_Fkt1", "LED_L_Abbiegel_Anf", EnvLCorninglight);
                    if (GRightTxEn)
                        VectorEmulator.SetMessageVariableSignalValue("m_LED_Soll_Fkt1", "LED_R_Abbiegel_Anf", EnvRCorninglight);

                    VectorEmulator.SetMessageVariableSignalValue("m_LED_Soll_Fkt0", "LED_L_Fernl_LH_Anf", EnvLHighbeam);
                    VectorEmulator.SetMessageVariableSignalValue("m_LED_Soll_Fkt0", "LED_R_Fernl_LH_Anf", EnvRHighbeam);

                    VectorEmulator.OutPut("m_LED_Soll_Fkt1", Can, CanDb1);
                    VectorEmulator.OutPut("m_LED_Soll_Fkt0", Can, CanDb1);
                }

                //if (GLeftTxEn)
                //{
                //    VectorEmulator.SetMessageVariableSignalValue("m_LED_Soll_Fkt1", "LED_L_Abbiegel_Anf", EnvLCorninglight);
                //    VectorEmulator.SetMessageVariableSignalValue("m_LED_Soll_Fkt0", "LED_L_Fernl_LH_Anf", EnvLHighbeam);
                //    VectorEmulator.SetMessageVariableSignalValue("m_LED_Soll_Fkt0", "LED_R_Fernl_LH_Anf", EnvRHighbeam);

                //    VectorEmulator.OutPut("m_LED_Soll_Fkt1", Can, CanDb1);
                //    VectorEmulator.OutPut("m_LED_Soll_Fkt0", Can, CanDb1);
                //}

                //if (GRightTxEn)
                //{
                //    VectorEmulator.SetMessageVariableSignalValue("m_LED_Soll_Fkt1", "LED_R_Abbiegel_Anf", EnvRCorninglight);
                //    VectorEmulator.SetMessageVariableSignalValue("m_LED_Soll_Fkt0", "LED_L_Fernl_LH_Anf", EnvLHighbeam);
                //    VectorEmulator.SetMessageVariableSignalValue("m_LED_Soll_Fkt0", "LED_R_Fernl_LH_Anf", EnvRHighbeam);

                //    VectorEmulator.OutPut("m_LED_Soll_Fkt1", Can, CanDb1);
                //    VectorEmulator.OutPut("m_LED_Soll_Fkt0", Can, CanDb1);
                //}
            };
        }

        private Action Timer200Ms()
        {
            return () =>
            {
                LUMI_LED();

                VectorEmulator.OutPut("m_MXB_Soll_Fkt0", Can);
                VectorEmulator.OutPut("m_MXB_Soll_Fkt1", Can);
                VectorEmulator.OutPut("m_MXB_Soll_Fkt2", Can);
                VectorEmulator.OutPut("m_MXB_Soll_Fkt3", Can);
                VectorEmulator.OutPut("m_MXB_Soll_Fkt4", Can);

                VectorEmulator.OutPut("m_MXB_Soll_Fkt9", Can);
                VectorEmulator.OutPut("m_MXB_Soll_Fkt10", Can);
                VectorEmulator.OutPut("m_MXB_Soll_Fkt11", Can);
                VectorEmulator.OutPut("m_MXB_Soll_Fkt12", Can);
                VectorEmulator.OutPut("m_MXB_Soll_Fkt13", Can);
            };
        }

        private Action Licht_Anf_01_100ms_Timer()
        {
            byte czMxb = 0;
            byte checkSum;
            byte index;

            return () =>
            {
                czMxb++;
                if (czMxb > 15)
                    czMxb = 0;

                VectorEmulator.SetMessageVariableSignalValue("my_licht_Anf_01", "Licht_Anf_01_bz", czMxb, CanDb2);
                VectorEmulator.SetMessageVariableSignalValue("my_licht_Anf_01", "Licht_Anf_01_Abblendlicht", EnvLlpLLowbeam, CanDb2);

                checkSum = 0xFF;
                for (index = 1; index < 8; index++)
                {
                    var temp = VectorEmulator.GetMessageVariableSignalValue("my_licht_Anf_01", index, CanDb2);
                    checkSum = (byte)(_crc8LookupTable[checkSum ^ temp] & 0xFF);
                }

                checkSum = (byte)(_crc8LookupTable[checkSum ^ _lcmSollBzTable2[czMxb]] & 0xFF);
                checkSum ^= 0xFF;

                VectorEmulator.SetMessageVariableByteValue("my_licht_Anf_01", 0, checkSum, CanDb2);

                VectorEmulator.OutPut("my_licht_Anf_01", Can, CanDb2);
            };
        }

        private Action Klemmen_timer()
        {
            byte bz = 0x00;
            byte index;
            byte checkSum;

            return () =>
            {
                bz++;
                if (bz > 15)
                    bz = 0;

                VectorEmulator.SetMessageVariableSignalValue("m_Klemmen_Status_01", "Klemmen_bz", bz);
                VectorEmulator.SetMessageVariableByteValue("m_Klemmen_Status_01", 2, 0xff);

                checkSum = 0xFF;
                for (index = 1; index < 4; index++)
                {
                    var temp = VectorEmulator.GetMessageVariableSignalValue("m_Klemmen_Status_01", index);
                    checkSum = (byte)(_crc8LookupTable[checkSum ^ temp] & 0xFF);
                }
                checkSum ^= _klemmenTable[bz];
                checkSum = (byte)(_crc8LookupTable[checkSum] & 0xff);
                checkSum ^= 0xFF;

                VectorEmulator.SetMessageVariableByteValue("m_Klemmen_Status_01", 0, checkSum);

                VectorEmulator.OutPut("m_Klemmen_Status_01", Can);

                Debug.WriteLine(string.Format("3c0: {0}", ValueHelper.GetHextStrWithOx(bz)));
            };
        }

        private Action Esp21Timer()
        {
            byte bz = 0x00;
            byte index;
            byte checkSum;

            return () =>
            {
                checkSum = 0xff;

                bz++;
                if (bz > 15)
                    bz = 0;

                VectorEmulator.SetMessageVariableSignalValue("m_ESP21", "ESP_21_BZ", bz);

                for (index = 1; index < 8; index++)
                {
                    var temp = VectorEmulator.GetMessageVariableSignalValue("m_ESP21", index);
                    checkSum = _crc8LookupTable[checkSum ^ temp];
                }

                checkSum ^= _esp21TAble[bz];
                checkSum = (byte)(_crc8LookupTable[checkSum] & 0xff);
                checkSum ^= 0xff;

                VectorEmulator.SetMessageVariableSignalValue("m_ESP21", "ESP_21_CRC", checkSum);

                VectorEmulator.OutPut("m_ESP21", Can);
            };
        }

        private Action LCM_Soll_Fkt_100ms_Timer()
        {
            byte bz = 0x00;
            byte index;
            byte checkSum;

            return () =>
            {
                VectorEmulator.SetMessageVariableSignalValue("my_LCM_Soll_Fkt", "LCM_TFL_L_Anf", EnvLcmTflL);
                VectorEmulator.SetMessageVariableSignalValue("my_LCM_Soll_Fkt", "LCM_TFL_R_Anf", EnvLcmTflR);
                VectorEmulator.SetMessageVariableSignalValue("my_LCM_Soll_Fkt", "LCM_Blinker_L_Anf", EnvLcmBlinkerL);
                VectorEmulator.SetMessageVariableSignalValue("my_LCM_Soll_Fkt", "LCM_Blinker_R_Anf", EnvLcmBlinkerR);
                VectorEmulator.SetMessageVariableSignalValue("my_LCM_Soll_Fkt", "LCM_Blinkerzyklus_L", EnvLcmBlinkerCycleL);
                VectorEmulator.SetMessageVariableSignalValue("my_LCM_Soll_Fkt", "LCM_Blinkerzyklus_R", EnvLcmBlinkerCycleR);
                VectorEmulator.SetMessageVariableSignalValue("my_LCM_Soll_Fkt", "LCM_PO_L_Anf", EnvLcmPoL);
                VectorEmulator.SetMessageVariableSignalValue("my_LCM_Soll_Fkt", "LCM_PO_R_Anf", EnvLcmPoR);

                bz++;
                if (bz > 15)
                    bz = 0;
                VectorEmulator.SetMessageVariableSignalValue("my_LCM_Soll_Fkt", "LCM_Soll_BZ", bz);

                checkSum = 0xFF;

                for (index = 1; index < 8; index++)
                {
                    var temp = VectorEmulator.GetMessageVariableSignalValue("my_LCM_Soll_Fkt", index);
                    checkSum = (byte)(_crc8LookupTable[checkSum ^ temp] & 0xFF);
                }

                checkSum ^= _lcmSollBzTable[bz];
                checkSum = (byte)(_crc8LookupTable[checkSum] & 0xff);
                checkSum ^= 0xFF;

                VectorEmulator.SetMessageVariableByteValue("my_LCM_Soll_Fkt", 0, checkSum);
                VectorEmulator.OutPut("my_LCM_Soll_Fkt", Can);
            };
        }

        [Description("R/W,myLeftAfsSetPos")]
        private byte myLeftAfsSetPos = 0;

        [Description("R/W,myRightAfsSetPos")]
        private byte myRightAfsSetPos = 0;

        [Description("R/W,mifc_Afs_RefRun_Links")]
        private byte mifc_Afs_RefRun_Links = 0;

        [Description("R/W,mifc_Afs_RefRun_Rechts")]
        private byte mifc_Afs_RefRun_Rechts = 0;

        //[Description("R/W,mifc_SOA_Variant")]
        //public byte mifc_SOA_Variant = 224;

        //[Description("R/W,mifc_SOA_Variant2")]
        //public byte mifc_SOA_Variant2 = 185;

        [Description("R/W,mifc_SOA_Variant")]
        public byte mifc_SOA_Variant = 0;

        [Description("R/W,mifc_SOA_Variant2")]
        public byte mifc_SOA_Variant2 = 0;

        //[Description("R/W,mifc_SOA_Variant")]
        //public byte mifc_SOA_Variant = 0;

        //[Description("R/W,mifc_SOA_Variant2")]
        //public byte mifc_SOA_Variant2 = 0;

        private Action msTimer_AfsSollFkt()
        {
            return () =>
            {
                //VectorEmulator.SetMessageVariableSignalValue("AfsSollFkt_msg", "SOA_SollPos_L", myLeftAfsSetPos);
                //VectorEmulator.SetMessageVariableSignalValue("AfsSollFkt_msg", "SOA_SollPos_R", myRightAfsSetPos);

                //if (myLeftAfsSetPos == 4091)
                //{
                //    myLeftAfsSetPos = 1;
                //}
                //else
                //{
                //    myLeftAfsSetPos = 4091;
                //}

                //if (myRightAfsSetPos == 4091)
                //{
                //    myRightAfsSetPos = 1;
                //}
                //else
                //{
                //    myRightAfsSetPos = 4091;
                //}

                //VectorEmulator.SetMessageVariableSignalValue("","",);

                VectorEmulator.SetMessageVariableSignalValue("AfsSollFkt_msg", "SOA_RefLauf_L", mifc_Afs_RefRun_Links);
                VectorEmulator.SetMessageVariableSignalValue("AfsSollFkt_msg", "SOA_FehlPos_R", mifc_Afs_RefRun_Rechts);

                VectorEmulator.SetMessageVariableSignalValue("AfsSollFkt_msg", "SOA_FehlPos_L", 0);
                VectorEmulator.SetMessageVariableSignalValue("AfsSollFkt_msg", "SOA_FehlPos_R", 0);

                VectorEmulator.SetMessageVariableSignalValue("AfsSollFkt_msg", "SOA_Variante", mifc_SOA_Variant);
                VectorEmulator.SetMessageVariableSignalValue("AfsSollFkt_msg", "SOA_Variante_2", mifc_SOA_Variant2);

                VectorEmulator.SetMessageVariableSignalValue("AfsSollFkt_msg", "SOA_DimAbLicht_L", 0x00);
                VectorEmulator.SetMessageVariableSignalValue("AfsSollFkt_msg", "SOA_DimAbLicht_R", 0x00);

                VectorEmulator.SetMessageVariableSignalValue("AfsSollFkt_msg", "dlc", 8);

                VectorEmulator.OutPut("AfsSollFkt_msg", Can);
            };
        }

        private Action msTimer_LwrSollFkt()
        {
            return () =>
            {
                VectorEmulator.OutPut("LwrSollFkt_msg", Can);
            };
        }

        #endregion

        private void LUMI_LED()
        {
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt0", "MXB_LED_01_Anf", EnvLlpMxbL1);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt0", "MXB_LED_02_Anf", EnvLlpMxbL2);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt0", "MXB_LED_03_Anf", EnvLlpMxbL3);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt0", "MXB_LED_04_Anf", EnvLlpMxbL4);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt0", "MXB_LED_05_Anf", EnvLlpMxbL5);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt0", "MXB_LED_06_Anf", EnvLlpMxbL6);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt0", "MXB_LED_07_Anf", EnvLlpMxbL7);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt0", "MXB_LED_08_Anf", EnvLlpMxbL8);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt0", "MXB_LED_09_Anf", EnvLlpMxbL9);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt0", "MXB_LED_10_Anf", EnvLlpMxbL10);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt1", "MXB_LED_11_Anf", EnvLlpMxbL11);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt1", "MXB_LED_12_Anf", EnvLlpMxbL12);

            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt2", "MXB_LED_25_Anf", EnvLlpMxbL22);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt2", "MXB_LED_26_Anf", EnvLlpMxbL21);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt2", "MXB_LED_27_Anf", EnvLlpMxbL20);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt2", "MXB_LED_28_Anf", EnvLlpMxbL19);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt2", "MXB_LED_29_Anf", EnvLlpMxbL18);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt2", "MXB_LED_30_Anf", EnvLlpMxbL17);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt3", "MXB_LED_31_Anf", EnvLlpMxbL16);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt3", "MXB_LED_32_Anf", EnvLlpMxbL15);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt3", "MXB_LED_33_Anf", EnvLlpMxbL14);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt3", "MXB_LED_34_Anf", EnvLlpMxbL13);

            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt3", "MXB_LED_35_Anf", EnvLlpMxbL32);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt3", "MXB_LED_36_Anf", EnvLlpMxbL31);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt3", "MXB_LED_37_Anf", EnvLlpMxbL30);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt3", "MXB_LED_38_Anf", EnvLlpMxbL29);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt3", "MXB_LED_39_Anf", EnvLlpMxbL28);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt3", "MXB_LED_40_Anf", EnvLlpMxbL27);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt4", "MXB_LED_41_Anf", EnvLlpMxbL26);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt4", "MXB_LED_42_Anf", EnvLlpMxbL25);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt4", "MXB_LED_43_Anf", EnvLlpMxbL24);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt4", "MXB_LED_44_Anf", EnvLlpMxbL23);

            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt9", "MXB_LED_91_Anf", EnvLlpMxbR1);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt9", "MXB_LED_92_Anf", EnvLlpMxbR2);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt9", "MXB_LED_93_Anf", EnvLlpMxbR3);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt9", "MXB_LED_94_Anf", EnvLlpMxbR4);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt9", "MXB_LED_95_Anf", EnvLlpMxbR5);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt9", "MXB_LED_96_Anf", EnvLlpMxbR6);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt9", "MXB_LED_97_Anf", EnvLlpMxbR7);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt9", "MXB_LED_98_Anf", EnvLlpMxbR8);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt9", "MXB_LED_99_Anf", EnvLlpMxbR9);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt9", "MXB_LED_100_Anf", EnvLlpMxbR10);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt10", "MXB_LED_101_Anf", EnvLlpMxbR11);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt10", "MXB_LED_102_Anf", EnvLlpMxbR12);

            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt11", "MXB_LED_115_Anf", EnvLlpMxbR22);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt11", "MXB_LED_116_Anf", EnvLlpMxbR21);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt11", "MXB_LED_117_Anf", EnvLlpMxbR20);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt11", "MXB_LED_118_Anf", EnvLlpMxbR19);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt11", "MXB_LED_119_Anf", EnvLlpMxbR18);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt11", "MXB_LED_120_Anf", EnvLlpMxbR17);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt12", "MXB_LED_121_Anf", EnvLlpMxbR16);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt12", "MXB_LED_122_Anf", EnvLlpMxbR15);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt12", "MXB_LED_123_Anf", EnvLlpMxbR14);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt12", "MXB_LED_124_Anf", EnvLlpMxbR13);

            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt12", "MXB_LED_125_Anf", EnvLlpMxbR32);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt12", "MXB_LED_126_Anf", EnvLlpMxbR31);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt12", "MXB_LED_127_Anf", EnvLlpMxbR30);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt12", "MXB_LED_128_Anf", EnvLlpMxbR29);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt12", "MXB_LED_129_Anf", EnvLlpMxbR28);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt12", "MXB_LED_130_Anf", EnvLlpMxbR27);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt13", "MXB_LED_131_Anf", EnvLlpMxbR26);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt13", "MXB_LED_132_Anf", EnvLlpMxbR25);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt13", "MXB_LED_133_Anf", EnvLlpMxbR24);
            VectorEmulator.SetMessageVariableSignalValue("m_MXB_Soll_Fkt13", "MXB_LED_134_Anf", EnvLlpMxbR23);
        }

        private double[] _ledCurrent =
        {
            0, 20, 20, 27, 39, 56, 63, 60.5, 52, 39, 33, //L1~L10
            25, 0, 0, 0, 0, 0, //L11~L16
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, //L17~L32
            20, 20, 27, 39, 56, 63, 60.5, 52, 39, 33, //R1~R10
            25, 0, 0, 0, 0, 0, //R11~R16
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 //R17~R18
        }; // LUMILED

        private readonly byte[] _crc8LookupTable =
        {
            0, 47, 94, 113, 188, 147, 226, 205, 87, 120, 9, 38, 235, 196, 181, 154, 174, 129, 240, 223, 18, 61, 76, 99,
            249, 214, 167, 136, 69, 106,
            27, 52, 115, 92, 45, 2, 207, 224, 145, 190, 36, 11, 122, 85, 152, 183, 198, 233, 221, 242, 131, 172, 97, 78,
            63, 16, 138, 165, 212, 251,
            54, 25, 104, 71, 230, 201, 184, 151, 90, 117, 4, 43, 177, 158, 239, 192, 13, 34, 83, 124, 72, 103, 22, 57,
            244, 219, 170, 133, 31, 48,
            65, 110, 163, 140, 253, 210, 149, 186, 203, 228, 41, 6, 119, 88, 194, 237, 156, 179, 126, 81, 32, 15, 59, 20,
            101, 74, 135, 168, 217, 246,
            108, 67, 50, 29, 208, 255, 142, 161, 227, 204, 189, 146, 95, 112, 1, 46, 180, 155, 234, 197, 8, 39, 86, 121,
            77, 98, 19, 60, 241, 222,
            175, 128, 26, 53, 68, 107, 166, 137, 248, 215, 144, 191, 206, 225, 44, 3, 114, 93, 199, 232, 153, 182, 123,
            84, 37, 10, 62, 17, 96, 79,
            130, 173, 220, 243, 105, 70, 55, 24, 213, 250, 139, 164, 5, 42, 91, 116, 185, 150, 231, 200, 82, 125, 12, 35,
            238, 193, 176, 159, 171, 132,
            245, 218, 23, 56, 73, 102, 252, 211, 162, 141, 64, 111, 30, 49, 118, 89, 40, 7, 202, 229, 148, 187, 33, 14,
            127, 80, 157, 178, 195,
            236, 216, 247, 134, 169, 100, 75, 58, 21, 143, 160, 209, 254, 51, 28, 109, 66
        };

        private readonly byte[] _lcmSollBzTable =
        {
            0xBD, 0x44, 0x90,
            0x8D, 0x74, 0xFA,
            0x91, 0x89, 0x41,
            0x4A, 0xBE, 0x82,
            0xB3, 0x05, 0x84, 0xCF
        };

        private readonly byte[] _lcmSollBzTable2 =
        {
            0xc5, 0x39, 0xc7, 0xf9, 
            0x92, 0xdb, 0x24, 0xce,
            0xf1, 0xb5, 0x7a, 0xc4, 
            0xbc, 0x60, 0xe3, 0xd1
        };

        private readonly byte[] _klemmenTable =
        {
            0xc3, 0xc3, 0xc3, 0xc3, 
            0xc3, 0xc3, 0xc3, 0xc3,
            0xc3, 0xc3, 0xc3, 0xc3, 
            0xc3, 0xc3, 0xc3, 0xc3
        };

        private readonly byte[] _esp21TAble =
        {
            0xb4, 0xef, 0xf8, 0x49, 
            0x1e, 0xe5, 0xc2, 0xc0,
            0x97, 0x19, 0x3c, 0xc9, 
            0xf1, 0x98, 0xd6, 0x61
        };

        private byte[] _motor =
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };

        [Description("R/W,GLeftTxEn")]
        public bool GLeftTxEn = false;

        [Description("R/W,GRightTxEn")]
        public bool GRightTxEn = false;

        [Description("R/W,EnvLCorninglight")]
        private byte EnvLCorninglight;

        [Description("R/W,EnvRCorninglight")]
        private byte EnvRCorninglight;

        [Description("R/W,EnvLHighbeam")]
        private byte EnvLHighbeam;

        [Description("R/W,EnvRHighbeam")]
        private byte EnvRHighbeam;

        [Description("R/W,EnvLlpLLowbeam")]
        public byte EnvLlpLLowbeam;

        [Description("R/W,EnvLcmTflL")]
        private byte EnvLcmTflL = 0;

        [Description("R/W,EnvLcmTflR")]
        private byte EnvLcmTflR = 0;

        [Description("R/W,EnvLcmBlinkerL")]
        private byte EnvLcmBlinkerL = 0;

        [Description("R/W,EnvLcmBlinkerR")]
        private byte EnvLcmBlinkerR = 0;

        [Description("R/W,EnvLcmBlinkerCycleL")]
        private byte EnvLcmBlinkerCycleL = 0;

        [Description("R/W,EnvLcmBlinkerCycleR")]
        private byte EnvLcmBlinkerCycleR = 0;

        [Description("R/W,EnvLcmPoL")]
        private byte EnvLcmPoL = 0;

        [Description("R/W,EnvLcmPoR")]
        private byte EnvLcmPoR = 0;

        [Description("R/W,EnvLlpMxbL1")]
        public byte EnvLlpMxbL1;//D12

        [Description("R/W,EnvLlpMxbL2")]
        public byte EnvLlpMxbL2;//D11

        [Description("R/W,EnvLlpMxbL3")]
        public byte EnvLlpMxbL3;//D10

        [Description("R/W,EnvLlpMxbL4")]
        public byte EnvLlpMxbL4;//D09

        [Description("R/W,EnvLlpMxbL5")]
        public byte EnvLlpMxbL5;//D08

        [Description("R/W,EnvLlpMxbL6")]
        public byte EnvLlpMxbL6;//D07

        [Description("R/W,EnvLlpMxbL7")]
        public byte EnvLlpMxbL7;//D06

        [Description("R/W,EnvLlpMxbL8")]
        public byte EnvLlpMxbL8;//D05

        [Description("R/W,EnvLlpMxbL9")]
        public byte EnvLlpMxbL9;//D04

        [Description("R/W,EnvLlpMxbL10")]
        public byte EnvLlpMxbL10;//D03

        [Description("R/W,EnvLlpMxbL11")]
        public byte EnvLlpMxbL11;//D02

        [Description("R/W,EnvLlpMxbL12")]
        public byte EnvLlpMxbL12;//D01

        [Description("R/W,EnvLlpMxbL22")]
        public byte EnvLlpMxbL22;//D22

        [Description("R/W,EnvLlpMxbL21")]
        public byte EnvLlpMxbL21;//D21

        [Description("R/W,EnvLlpMxbL20")]
        public byte EnvLlpMxbL20;//D20

        [Description("R/W,EnvLlpMxbL19")]
        public byte EnvLlpMxbL19;//D19

        [Description("R/W,EnvLlpMxbL18")]
        public byte EnvLlpMxbL18;//D18

        [Description("R/W,EnvLlpMxbL17")]
        public byte EnvLlpMxbL17;//D17

        [Description("R/W,EnvLlpMxbL16")]
        public byte EnvLlpMxbL16;//D16

        [Description("R/W,EnvLlpMxbL15")]
        public byte EnvLlpMxbL15;//D15

        [Description("R/W,EnvLlpMxbL14")]
        public byte EnvLlpMxbL14;//D14

        [Description("R/W,EnvLlpMxbL13")]
        public byte EnvLlpMxbL13;//D13

        [Description("R/W,EnvLlpMxbL32")]
        public byte EnvLlpMxbL32;//D32

        [Description("R/W,EnvLlpMxbL31")]
        public byte EnvLlpMxbL31;//D31

        [Description("R/W,EnvLlpMxbL30")]
        public byte EnvLlpMxbL30;//D30

        [Description("R/W,EnvLlpMxbL29")]
        public byte EnvLlpMxbL29;//D29

        [Description("R/W,EnvLlpMxbL28")]
        public byte EnvLlpMxbL28;//D28

        [Description("R/W,EnvLlpMxbL27")]
        public byte EnvLlpMxbL27;//D27

        [Description("R/W,EnvLlpMxbL26")]
        public byte EnvLlpMxbL26;//D26

        [Description("R/W,EnvLlpMxbL25")]
        public byte EnvLlpMxbL25;//D25

        [Description("R/W,EnvLlpMxbL24")]
        public byte EnvLlpMxbL24;//D24

        [Description("R/W,EnvLlpMxbL23")]
        public byte EnvLlpMxbL23;//D23

        [Description("R/W,EnvLlpMxbR1")]
        public byte EnvLlpMxbR1;//D12

        [Description("R/W,EnvLlpMxbR2")]
        public byte EnvLlpMxbR2;//D11

        [Description("R/W,EnvLlpMxbR3")]
        public byte EnvLlpMxbR3;//D10

        [Description("R/W,EnvLlpMxbR4")]
        public byte EnvLlpMxbR4;//D09

        [Description("R/W,EnvLlpMxbR5")]
        public byte EnvLlpMxbR5;//D8

        [Description("R/W,EnvLlpMxbR6")]
        public byte EnvLlpMxbR6;//D7

        [Description("R/W,EnvLlpMxbR7")]
        public byte EnvLlpMxbR7;//D6

        [Description("R/W,EnvLlpMxbR8")]
        public byte EnvLlpMxbR8;//D5

        [Description("R/W,EnvLlpMxbR9")]
        public byte EnvLlpMxbR9;//D4

        [Description("R/W,EnvLlpMxbR10")]
        public byte EnvLlpMxbR10;//D3

        [Description("R/W,EnvLlpMxbR11")]
        public byte EnvLlpMxbR11;//D2

        [Description("R/W,EnvLlpMxbR12")]
        public byte EnvLlpMxbR12;//D1

        [Description("R/W,EnvLlpMxbR22")]
        public byte EnvLlpMxbR22;//D22

        [Description("R/W,EnvLlpMxbR21")]
        public byte EnvLlpMxbR21;//D21

        [Description("R/W,EnvLlpMxbR20")]
        public byte EnvLlpMxbR20;//D20

        [Description("R/W,EnvLlpMxbR19")]
        public byte EnvLlpMxbR19;//D19

        [Description("R/W,EnvLlpMxbR18")]
        public byte EnvLlpMxbR18;//D18

        [Description("R/W,EnvLlpMxbR17")]
        public byte EnvLlpMxbR17;//D17

        [Description("R/W,EnvLlpMxbR16")]
        public byte EnvLlpMxbR16;//D16

        [Description("R/W,EnvLlpMxbR15")]
        public byte EnvLlpMxbR15;//D15

        [Description("R/W,EnvLlpMxbR14")]
        public byte EnvLlpMxbR14;//D14

        [Description("R/W,EnvLlpMxbR13")]
        public byte EnvLlpMxbR13;//D13

        [Description("R/W,EnvLlpMxbR32")]
        public byte EnvLlpMxbR32;//32

        [Description("R/W,EnvLlpMxbR31")]
        public byte EnvLlpMxbR31;//31

        [Description("R/W,EnvLlpMxbR30")]
        public byte EnvLlpMxbR30;//30

        [Description("R/W,EnvLlpMxbR29")]
        public byte EnvLlpMxbR29;//29

        [Description("R/W,EnvLlpMxbR28")]
        public byte EnvLlpMxbR28;//28

        [Description("R/W,EnvLlpMxbR27")]
        public byte EnvLlpMxbR27;//27

        [Description("R/W,EnvLlpMxbR26")]
        public byte EnvLlpMxbR26;//26

        [Description("R/W,EnvLlpMxbR25")]
        public byte EnvLlpMxbR25;//25

        [Description("R/W,EnvLlpMxbR24")]
        public byte EnvLlpMxbR24;//24

        [Description("R/W,EnvLlpMxbR23")]
        public byte EnvLlpMxbR23;//23

        #endregion
    }
}
