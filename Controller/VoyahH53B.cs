using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Controller
{
    [Description("CAN-Product,东风岚图H53B")]
    public sealed class VoyahH53B : ControllerBase
    {
        public CanBus Can;
        private VectorDbcEmulator VectorEmulator { get; set; }
        private bool IsCanStarted { get; set; }

        public VoyahH53B(string name) : base(name)
        {
            SysConfig(Directory.GetCurrentDirectory() + @"\ControllerConfig\H77_Matrix_ExLighting_CAN_v3.0.2_20250328.dbc");
        }

        ~VoyahH53B()
        {
            Dispose();
        }

        [Description("开启CAN消息")]
        public void StartCanMsg()
        {
            IsCanStarted = true;
        }

        [Description("关闭CAN消息")]
        public void StopCanMsg()
        {
            IsCanStarted = false;
        }

        private void SysConfig(string dbcFilePath)
        {
            VectorEmulator =
                new VectorDbcEmulator(new[] { dbcFilePath });
            Start();
        }

        private void Start()
        {
            VectorEmulator.SetTimer(Tmr_Refresh20Ms(), 20);
            VectorEmulator.SetTimer(Tmr_Refresh10Ms(), 10);
            VectorEmulator.SetTimer(Tmr_Refresh100Ms(), 100);
            VectorEmulator.SetTimer(Tmr_Refresh400Ms(), 400);
        }

        private Action Tmr_Refresh20Ms()
        {
            return () =>
            {
                var toCheckCrc = new List<byte>();
                toCheckCrc.Add(0x02);
                toCheckCrc.Add(0xA1);

                var matrixData = new CanCommunicationMatrix.MotorolaMatrix(0x2A1, 8);
                matrixData.UpdateData(new MatrixValDefinition(0, 2, BCM_RMPosLampReq));
                matrixData.UpdateData(new MatrixValDefinition(2, 2, BCM_FLdrlReq));
                matrixData.UpdateData(new MatrixValDefinition(9, 3, BCM_EXLHeightAdjtReq));
                matrixData.UpdateData(new MatrixValDefinition(12, 2, BCM_ADBModeReq));
                matrixData.UpdateData(new MatrixValDefinition(14, 2, BCM_FLPosLampReq));
                matrixData.UpdateData(new MatrixValDefinition(16, 2, BCM_FRdrlReq));
                matrixData.UpdateData(new MatrixValDefinition(18, 2, BCM_FRPosLampReq));
                matrixData.UpdateData(new MatrixValDefinition(20, 2, BCM_LowBeamReq));
                matrixData.UpdateData(new MatrixValDefinition(22, 2, BCM_HighBeamReq));
                matrixData.UpdateData(new MatrixValDefinition(26, 2, BCM_RearfoglampReq));
                if (BCM_LeftTurnLampReq == 2)
                {
                    if (_turn400msSignal == 1)
                    {
                        matrixData.UpdateData(new MatrixValDefinition(28, 2, BCM_LeftTurnLampReq));
                    }
                    else if (_turn400msSignal == 2)
                    {
                        matrixData.UpdateData(new MatrixValDefinition(28, 2, 0x01));
                    }
                    else
                    {
                        matrixData.UpdateData(new MatrixValDefinition(28, 2, BCM_LeftTurnLampReq));
                    }
                }
                else
                {
                    matrixData.UpdateData(new MatrixValDefinition(28, 2, BCM_LeftTurnLampReq));
                }

                if (BCM_RightTurnLampReq == 2)
                {
                    if (_turn400msSignal == 1)
                    {
                        matrixData.UpdateData(new MatrixValDefinition(30, 2, BCM_RightTurnLampReq));
                    }
                    else if (_turn400msSignal == 2)
                    {
                        matrixData.UpdateData(new MatrixValDefinition(30, 2, 0x01));
                    }
                    else
                    {
                        matrixData.UpdateData(new MatrixValDefinition(30, 2, BCM_RightTurnLampReq));
                    }
                }
                else
                {
                    matrixData.UpdateData(new MatrixValDefinition(30, 2, BCM_RightTurnLampReq));
                }

                matrixData.UpdateData(new MatrixValDefinition(32, 2, BCM_RLPosLampReq));
                matrixData.UpdateData(new MatrixValDefinition(34, 2, BCM_RRPosLampReq));
                matrixData.UpdateData(new MatrixValDefinition(36, 2, BCM_turnLampFlickerReq));
                matrixData.UpdateData(new MatrixValDefinition(38, 2, BCM_turnLampSerialReq));
                matrixData.UpdateData(new MatrixValDefinition(40, 2, BCM_ReversinglampReq));
                matrixData.UpdateData(new MatrixValDefinition(42, 2, BCM_StoplampReq));
                matrixData.UpdateData(new MatrixValDefinition(44, 2, BCM_LicenseLampReq));
                matrixData.UpdateData(new MatrixValDefinition(48, 4, RollingCounter2A1));

                var tpToCheckData = new byte[7];
                Array.Copy(matrixData.MatrixData, 0, tpToCheckData, 0, 7);
                toCheckCrc.AddRange(tpToCheckData);

                Checksum2A1 = crc_8find(toCheckCrc.ToArray(), toCheckCrc.Count);
                matrixData.UpdateData(new MatrixValDefinition(56, 8, Checksum2A1));

                RollingCounter2A1++;
                if (RollingCounter2A1 > 15)
                    RollingCounter2A1 = 0;

                if (Can != null && IsCanStarted)
                    Can.SendStandardCanData(matrixData.CanId, matrixData.MatrixData);
            };
        }

        private Action Tmr_Refresh10Ms()
        {
            return () =>
            {
                var matrixData = new CanCommunicationMatrix.MotorolaMatrix(0x62, 8);
                matrixData.UpdateData(new MatrixValDefinition(0, 2, BCM_AdasBlueLampReq));

                if (Can != null && IsCanStarted)
                    Can.SendStandardCanData(matrixData.CanId, matrixData.MatrixData);
            };
        }

        private Action Tmr_Refresh100Ms()
        {
            return () =>
            {
                var matrixData = new CanCommunicationMatrix.MotorolaMatrix(0x3C0, 8);
                matrixData.UpdateData(new MatrixValDefinition(34, 4, BCM_WelcomeLightReq));

                var matrixData2 = new CanCommunicationMatrix.MotorolaMatrix(0x201, 8);
                matrixData2.UpdateData(new MatrixValDefinition(30, 3, BCM_RearLogoReq));

                if (Can != null && IsCanStarted)
                {
                    Can.SendStandardCanData(matrixData.CanId, matrixData.MatrixData);
                    Can.SendStandardCanData(matrixData2.CanId, matrixData2.MatrixData);
                }
            };
        }

        private Action Tmr_Refresh400Ms()
        {
            return () =>
            {
                if (_turn400msSignal == 1)
                {
                    _turn400msSignal = 2;
                }
                else if (_turn400msSignal == 2)
                {
                    _turn400msSignal = 1;
                }
            };
        }

        [Description("R/W,BCM_RMPosLampReq")]
        public byte BCM_RMPosLampReq = 1;
        [Description("R/W,BCM_FLdrlReq")]
        public byte BCM_FLdrlReq;
        [Description("R/W,BCM_EXLHeightAdjtReq")]
        public byte BCM_EXLHeightAdjtReq;
        [Description("R/W,BCM_ADBModeReq")]
        public byte BCM_ADBModeReq;
        [Description("R/W,BCM_FLPosLampReq")]
        public byte BCM_FLPosLampReq;
        [Description("R/W,BCM_FRdrlReq")]
        public byte BCM_FRdrlReq;
        [Description("R/W,BCM_FRPosLampReq")]
        public byte BCM_FRPosLampReq;
        [Description("R/W,BCM_LowBeamReq")]
        public byte BCM_LowBeamReq;
        [Description("R/W,BCM_HighBeamReq")]
        public byte BCM_HighBeamReq;
        [Description("R/W,BCM_RearfoglampReq")]
        public byte BCM_RearfoglampReq;
        [Description("R/W,BCM_LeftTurnLampReq")]
        public byte BCM_LeftTurnLampReq = 1;
        [Description("R/W,BCM_RightTurnLampReq")]
        public byte BCM_RightTurnLampReq = 1;
        [Description("R/W,BCM_RLPosLampReq")]
        public byte BCM_RLPosLampReq = 1;
        [Description("R/W,BCM_RRPosLampReq")]
        public byte BCM_RRPosLampReq = 1;
        [Description("R/W,BCM_turnLampFlickerReq")]
        public byte BCM_turnLampFlickerReq;
        [Description("R/W,BCM_turnLampSerialReq")]
        public byte BCM_turnLampSerialReq;
        [Description("R/W,BCM_ReversinglampReq")]
        public byte BCM_ReversinglampReq;
        [Description("R/W,BCM_StoplampReq")]
        public byte BCM_StoplampReq = 1;
        [Description("R/W,BCM_LicenseLampReq")]
        public byte BCM_LicenseLampReq;
        private byte RollingCounter2A1;
        private byte Checksum2A1;

        public byte BCM_AdasBlueLampReq = 1;
        public byte BCM_RearLogoReq = 1;
        public byte BCM_WelcomeLightReq = 1;

        private byte[] crc_table = new byte[]{
            0x00, 0x1D, 0x3A, 0x27, 0x74, 0x69, 0x4E, 0x53,
            0xE8, 0xF5, 0xD2, 0xCF, 0x9C, 0x81, 0xA6, 0xBB,
            0xCD, 0xD0, 0xF7, 0xEA, 0xB9, 0xA4, 0x83, 0x9E,
            0x25, 0x38, 0x1F, 0x02, 0x51, 0x4C, 0x6B, 0x76,
            0x87, 0x9A, 0xBD, 0xA0, 0xF3, 0xEE, 0xC9, 0xD4,
            0x6F, 0x72, 0x55, 0x48, 0x1B, 0x06, 0x21, 0x3C,
            0x4A, 0x57, 0x70, 0x6D, 0x3E, 0x23, 0x04, 0x19,
            0xA2, 0xBF, 0x98, 0x85, 0xD6, 0xCB, 0xEC, 0xF1,
            0x13, 0x0E, 0x29, 0x34, 0x67, 0x7A, 0x5D, 0x40,
            0xFB, 0xE6, 0xC1, 0xDC, 0x8F, 0x92, 0xB5, 0xA8,
            0xDE, 0xC3, 0xE4, 0xF9, 0xAA, 0xB7, 0x90, 0x8D,
            0x36, 0x2B, 0x0C, 0x11, 0x42, 0x5F, 0x78, 0x65,
            0x94, 0x89, 0xAE, 0xB3, 0xE0, 0xFD, 0xDA, 0xC7,
            0x7C, 0x61, 0x46, 0x5B, 0x08, 0x15, 0x32, 0x2F,
            0x59, 0x44, 0x63, 0x7E, 0x2D, 0x30, 0x17, 0x0A,
            0xB1, 0xAC, 0x8B, 0x96, 0xC5, 0xD8, 0xFF, 0xE2,
            0x26, 0x3B, 0x1C, 0x01, 0x52, 0x4F, 0x68, 0x75,
            0xCE, 0xD3, 0xF4, 0xE9, 0xBA, 0xA7, 0x80, 0x9D,
            0xEB, 0xF6, 0xD1, 0xCC, 0x9F, 0x82, 0xA5, 0xB8,
            0x03, 0x1E, 0x39, 0x24, 0x77, 0x6A, 0x4D, 0x50,
            0xA1, 0xBC, 0x9B, 0x86, 0xD5, 0xC8, 0xEF, 0xF2,
            0x49, 0x54, 0x73, 0x6E, 0x3D, 0x20, 0x07, 0x1A,
            0x6C, 0x71, 0x56, 0x4B, 0x18, 0x05, 0x22, 0x3F,
            0x84, 0x99, 0xBE, 0xA3, 0xF0, 0xED, 0xCA, 0xD7,
            0x35, 0x28, 0x0F, 0x12, 0x41, 0x5C, 0x7B, 0x66,
            0xDD, 0xC0, 0xE7, 0xFA, 0xA9, 0xB4, 0x93, 0x8E,
            0xF8, 0xE5, 0xC2, 0xDF, 0x8C, 0x91, 0xB6, 0xAB,
            0x10, 0x0D, 0x2A, 0x37, 0x64, 0x79, 0x5E, 0x43,
            0xB2, 0xAF, 0x88, 0x95, 0xC6, 0xDB, 0xFC, 0xE1,
            0x5A, 0x47, 0x60, 0x7D, 0x2E, 0x33, 0x14, 0x09,
            0x7F, 0x62, 0x45, 0x58, 0x0B, 0x16, 0x31, 0x2C,
            0x97, 0x8A, 0xAD, 0xB0, 0xE3, 0xFE, 0xD9, 0xC4
        };

        private byte crc_8find(byte[] data, int len)
        {
            byte crc8 = 0x00;
            var dIndex = 0;
            while (len-- > 0)
            {
                crc8 = crc_table[crc8 ^ data[dIndex]];
                dIndex++;
            }
            return crc8;
        }

        #region 尾灯

        [Description("B灯位置灯开")]
        public void RMRearLampOn() 
        {
            BCM_RMPosLampReq = 2;
        }

        [Description("B灯位置灯关")]
        public void RMRearLampOff() 
        {
            BCM_RMPosLampReq = 1;
        }

        [Description("左尾灯开")]
        public void LeftRearLampOn()
        {
            BCM_RLPosLampReq = 2;
        }

        [Description("左尾灯关")]
        public void LeftRearLampOff()
        {
            BCM_RLPosLampReq = 1;
        }

        [Description("右尾灯开")]
        public void RightRearLampOn()
        {
            BCM_RRPosLampReq = 2;
        }

        [Description("右尾灯关")]
        public void RightRearLampOff()
        {
            BCM_RRPosLampReq = 1;
        }

        [Description("制动灯开")]
        public void StopOn()
        {
            BCM_StoplampReq = 2;
        }

        [Description("制动灯关")]
        public void StopOff()
        {
            BCM_StoplampReq = 1;
        }

        [Description("左转开")]
        public void LeftTurnOn()
        {
            BCM_LeftTurnLampReq = 2;
        }

        [Description("左转关")]
        public void LeftTurnOff()
        {
            BCM_LeftTurnLampReq = 1;
        }

        [Description("右转开")]
        public void RightTurnOn()
        {
            BCM_RightTurnLampReq = 2;
        }

        [Description("右转关")]
        public void RightTurnOff()
        {
            BCM_RightTurnLampReq = 1;
        }

        [Description("转向流水使能")]
        public void TurnLampSerialOn()
        {
            BCM_turnLampSerialReq = 1;
        }

        [Description("转向流水失能")]
        public void TurnLampSerialOff()
        {
            BCM_turnLampSerialReq = 0;
        }

        [Description("logoDynamic开")]
        public void LogoDynamicOn()
        {
            BCM_RearLogoReq = 2;
        }

        [Description("logoStatic开")]
        public void LogoStaticOn()
        {
            BCM_RearLogoReq = 3;
        }

        [Description("logo关")]
        public void LogoOff()
        {
            BCM_RearLogoReq = 1;
        }

        [Description("小蓝灯开")]
        public void BlueLampOn()
        {
            BCM_AdasBlueLampReq = 2;
        }

        [Description("小蓝灯关")]
        public void BlueLampOff()
        {
            BCM_AdasBlueLampReq = 1;
        }

        [Description("welcome打开")]
        public void WelcomeOn(string mode)
        {
            byte value;
            if (byte.TryParse(mode, out value))
            {
                if (value >= 2 && value <= 0x0B)
                {
                    BCM_WelcomeLightReq = value;
                }
            }
        }

        [Description("welcome关闭")]
        public void WelcomeOff()
        {
            BCM_WelcomeLightReq = 1;
        }

        #endregion

        #region turn 400ms 使能

        private int _turn400msSignal;

        [Description("转向400毫秒使能")]
        public void Turn400MsOn()
        {
            _turn400msSignal = 1;
        }

        [Description("转向400毫秒失能")]
        public void Turn400MsOff()
        {
            _turn400msSignal = 0;
        }

        #endregion
    }
}
