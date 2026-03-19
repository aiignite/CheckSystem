using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,LDM12B_CANFD_Bus_Simulation")]
    public sealed class Ldm12BCanFdBusSimulation : ControllerBase
    {
        public CanBus Can;

        public Ldm12BCanFdBusSimulation(string name)
            : base(name)
        {
            if (Th != null)
            {
                Th.Abort();
                Th.Join();
            }
            Th = new Thread(MainWork);
            Th.Start();
        }

        ~Ldm12BCanFdBusSimulation()
        {
            Dispose();
        }

        [Description("模块唤醒")]
        public void ModuleAwake()
        {
            _isAwake = true;
        }

        [Description("模块休眠")]
        public void ModuleSleep()
        {
            _isAwake = false;
        }

        [Description("近光开")]
        public void LowBeamOn()
        {
            _headLampLoFlOnBStat1 = 0x01;
            _headLampLoFrOnBStat1 = 0x01;
        }

        [Description("近光关")]
        public void LowBeamOff()
        {
            _headLampLoFlOnBStat1 = 0x00;
            _headLampLoFrOnBStat1 = 0x00;
        }

        [Description("远近光开")]
        public void HighBeamOn()
        {
            _headLghtHiDRq = 0x02;
        }

        [Description("远近光关")]
        public void HighBeamOff()
        {
            _headLghtHiDRq = 0x00;
        }

        [Description("打开DRL")]
        public void DrlOn()
        {
            _drLandPosActivationControl = 0x02;
        }

        [Description("打开PL")]
        public void PlOn()
        {
            _drLandPosActivationControl = 0x01;
        }

        [Description("关闭DRL/PL")]
        public void DrlPlOff()
        {
            _drLandPosActivationControl = 0x00;
        }

        [Description("转向灯常亮")]
        public void TurnOn()
        {
            _turnLghtLeftDRq2 = 0x02;
            _turnLghtRightDRq2 = 0x02;

        }

        [Description("转向灯流水")]
        public void TurnSeq()
        {
            _turnLghtLeftDRq2 = 0x03;
            _turnLghtRightDRq2 = 0x03;
        }

        [Description("关闭转向灯")]
        public void TurnOff()
        {
            _turnLghtLeftDRq2 = 0x00;
            _turnLghtRightDRq2 = 0x00;
        }

        [Description("按百分比亮度打开SBL")]
        public void SblOn(string percent)
        {
            int per;
            if (!int.TryParse(percent, out per))
                return;
            if (per <= 0 || per > 100)
                return;
            _slght1BrghtLeftbRPcRq8 = (byte)(int.Parse(percent) * 2);
            _slght1BrghtRightPcRq8 = (byte)(int.Parse(percent) * 2);
        }

        [Description("关闭SBL")]
        public void SblOff()
        {
            _slght1BrghtLeftbRPcRq8 = 0;
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

        [Description("ResetAnimation")]
        public void ResetAnimation()
        {
            _wfSuperstateDStat2 = 0x000;
            _wfSubstateDStat3 = 0x00;
            _ignitionStatus4 = 0x04;
        }

        private Thread Th { get; set; }
        private bool _isAwake;
        private byte _turnLghtLeftDRq2 = 0x01;
        private byte _turnLghtRightDRq2 = 0x01;
        private byte _headLampLoFlOnBStat1;
        private byte _headLampLoFrOnBStat1;
        private byte _ignitionStatus4 = 0x04;
        private byte _pwPckTqDStat2;
        private byte _elPwDStat3;
        private byte _hcmSleepyTime1;
        private byte _headLghtHiDRq;
        private byte HeadLghtHi_T_Rq = 0x03;
        private byte _wfSuperstateDStat2;
        private byte _wfSubstateDStat3;
        private byte _drLandPosActivationControl;
        private byte _slght1BrghtLeftbRPcRq8;
        private byte _slght1BrghtRightPcRq8;

        /// <summary>
        /// VehicleOperatingModes
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _ldm12VehicleOperatingModes =
            new CanCommunicationMatrix.MotorolaMatrix(0x55, 8);

        /// <summary>
        /// HighBeamMaster
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _ldm12HiBeamRq =
            new CanCommunicationMatrix.MotorolaMatrix(0x66, 64);

        /// <summary>
        /// LvlCmd
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _ldm12LvlCmd =
            new CanCommunicationMatrix.MotorolaMatrix(0x76, 8);

        /// <summary>
        /// AFSRq
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _ldm12AfsRq =
            new CanCommunicationMatrix.MotorolaMatrix(0x62, 8);

        /// <summary>
        /// SsblLeftHl
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _ldm12SsblLeftHl =
            new CanCommunicationMatrix.MotorolaMatrix(0x70, 8);

        /// <summary>
        /// SsblRightHl
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _ldm12SsblRightHl =
            new CanCommunicationMatrix.MotorolaMatrix(0x71, 8);

        /// <summary>
        /// E2E_BCMtoLDCM
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _ldm12E2EBcMtoLdcm =
            new CanCommunicationMatrix.MotorolaMatrix(0x320, 8);

        /// <summary>
        /// BaseFeaturesActvRq
        /// </summary>
        private readonly CanCommunicationMatrix.MotorolaMatrix _ldm12BaseFeaturesActvRq =
           new CanCommunicationMatrix.MotorolaMatrix(0x50, 7);

        private void MainWork()
        {
            var sendCount = 0;
            const int period = 10;
            var u8Mes320Bz = 0;

            while (Th.IsAlive)
            {
                if (!Th.IsAlive)
                    break;

                Thread.Sleep(period);

                if (Can == null || !_isAwake)
                {
                    sendCount = 0;
                    continue;
                }

                try
                {
                    var sendList = new List<CanBus.CanDataPackage>();

                    if (sendCount % (30 / period) == 0) // T_30ms_Event
                    {
                        _ldm12SsblLeftHl.UpdateData(new MatrixValDefinition(0, 8, _slght1BrghtLeftbRPcRq8));
                        _ldm12SsblLeftHl.UpdateData(new MatrixValDefinition(20, 8, _slght1BrghtLeftbRPcRq8));
                        _ldm12SsblLeftHl.UpdateData(new MatrixValDefinition(24, 8, _slght1BrghtLeftbRPcRq8));
                        _ldm12SsblLeftHl.UpdateData(new MatrixValDefinition(44, 8, _slght1BrghtLeftbRPcRq8));
                        _ldm12SsblLeftHl.UpdateData(new MatrixValDefinition(48, 8, _slght1BrghtLeftbRPcRq8));

                        _ldm12SsblRightHl.UpdateData(new MatrixValDefinition(0, 8, _slght1BrghtRightPcRq8));
                        _ldm12SsblRightHl.UpdateData(new MatrixValDefinition(20, 8, _slght1BrghtRightPcRq8));
                        _ldm12SsblRightHl.UpdateData(new MatrixValDefinition(24, 8, _slght1BrghtRightPcRq8));
                        _ldm12SsblRightHl.UpdateData(new MatrixValDefinition(44, 8, _slght1BrghtRightPcRq8));
                        _ldm12SsblRightHl.UpdateData(new MatrixValDefinition(48, 8, _slght1BrghtRightPcRq8));

                        sendList.AddRange(
                            new[]
                            {
                                new CanBus.CanDataPackage(_ldm12LvlCmd.CanId, CanBus.CanProtocol.CanFd,
                                    CanBus.CanType.Standard, CanBus.CanFormat.Data, _ldm12LvlCmd.MatrixData),
                                new CanBus.CanDataPackage(_ldm12AfsRq.CanId, CanBus.CanProtocol.CanFd,
                                    CanBus.CanType.Standard, CanBus.CanFormat.Data, _ldm12AfsRq.MatrixData),
                                new CanBus.CanDataPackage(_ldm12SsblLeftHl.CanId, CanBus.CanProtocol.CanFd,
                                    CanBus.CanType.Standard, CanBus.CanFormat.Data, _ldm12SsblLeftHl.MatrixData),
                                new CanBus.CanDataPackage(_ldm12SsblRightHl.CanId, CanBus.CanProtocol.CanFd,
                                    CanBus.CanType.Standard, CanBus.CanFormat.Data, _ldm12SsblRightHl.MatrixData)
                            });
                    }

                    if (sendCount % (10 / period) == 0) // T_10ms_Event
                    {
                        _ldm12VehicleOperatingModes.UpdateData(new MatrixValDefinition(2, 2, _pwPckTqDStat2));
                        _ldm12VehicleOperatingModes.UpdateData(new MatrixValDefinition(4, 4, _ignitionStatus4));
                        _ldm12VehicleOperatingModes.UpdateData(new MatrixValDefinition(14, 1, _hcmSleepyTime1));
                        _ldm12VehicleOperatingModes.UpdateData(new MatrixValDefinition(15, 3, _elPwDStat3));

                        sendList.AddRange(
                           new[]
                            {
                                new CanBus.CanDataPackage(_ldm12VehicleOperatingModes.CanId, CanBus.CanProtocol.CanFd,
                                    CanBus.CanType.Standard, CanBus.CanFormat.Data, _ldm12VehicleOperatingModes.MatrixData)
                            });
                    }

                    if (sendCount % (20 / period) == 0) // T_20ms_Event
                    {
                        _ldm12HiBeamRq.UpdateData(new MatrixValDefinition(5, 3, _headLghtHiDRq));
                        _ldm12HiBeamRq.UpdateData(new MatrixValDefinition(1, 4, HeadLghtHi_T_Rq));

                        _ldm12BaseFeaturesActvRq.UpdateData(new MatrixValDefinition(27, 3, _wfSubstateDStat3));
                        _ldm12BaseFeaturesActvRq.UpdateData(new MatrixValDefinition(30, 2, _wfSuperstateDStat2));

                        #region DRL PL
                        _ldm12BaseFeaturesActvRq.UpdateData(new MatrixValDefinition(14, 2, _drLandPosActivationControl));
                        _ldm12BaseFeaturesActvRq.UpdateData(new MatrixValDefinition(12, 2, _drLandPosActivationControl));
                        _ldm12BaseFeaturesActvRq.UpdateData(new MatrixValDefinition(10, 2, _drLandPosActivationControl));
                        _ldm12BaseFeaturesActvRq.UpdateData(new MatrixValDefinition(8, 2, _drLandPosActivationControl));

                        _ldm12BaseFeaturesActvRq.UpdateData(new MatrixValDefinition(38, 2, _drLandPosActivationControl));
                        _ldm12BaseFeaturesActvRq.UpdateData(new MatrixValDefinition(36, 2, _drLandPosActivationControl));
                        _ldm12BaseFeaturesActvRq.UpdateData(new MatrixValDefinition(34, 2, _drLandPosActivationControl));
                        _ldm12BaseFeaturesActvRq.UpdateData(new MatrixValDefinition(32, 2, _drLandPosActivationControl));
                        #endregion

                        sendList.AddRange(
                            new[]
                            {
                                new CanBus.CanDataPackage(_ldm12HiBeamRq.CanId, CanBus.CanProtocol.CanFd,
                                    CanBus.CanType.Standard, CanBus.CanFormat.Data, _ldm12HiBeamRq.MatrixData),
                                new CanBus.CanDataPackage(_ldm12BaseFeaturesActvRq.CanId, CanBus.CanProtocol.CanFd,
                                    CanBus.CanType.Standard, CanBus.CanFormat.Data, _ldm12BaseFeaturesActvRq.MatrixData)
                            });
                    }

                    if (sendCount % (100 / period) == 0) // T_100ms_Event
                    {
                        if (u8Mes320Bz >= 14)
                            u8Mes320Bz = 0;
                        else
                            u8Mes320Bz++;

                        var sumCrc = Crc_CalculateCRC8(new[] { 0x20, 0x03 }, 2, 0xFF);

                        var message320Data = new int[7];
                        message320Data[0] = u8Mes320Bz;
                        message320Data[1] =
                                (_turnLghtLeftDRq2 << 6) | (_turnLghtRightDRq2 << 4) |
                                (_headLampLoFlOnBStat1 << 3) | (_headLampLoFrOnBStat1 << 2);

                        sumCrc = Crc_CalculateCRC8(message320Data, 7, sumCrc);
                        sumCrc = (byte)(sumCrc ^ 0xFF);

                        int u8Mes320Crc = sumCrc;

                        _ldm12E2EBcMtoLdcm.UpdateData(new MatrixValDefinition(8, 4, (byte)u8Mes320Bz));
                        _ldm12E2EBcMtoLdcm.UpdateData(new MatrixValDefinition(0, 8, (byte)u8Mes320Crc));

                        _ldm12E2EBcMtoLdcm.UpdateData(new MatrixValDefinition(18, 1, _headLampLoFrOnBStat1));
                        _ldm12E2EBcMtoLdcm.UpdateData(new MatrixValDefinition(19, 1, _headLampLoFlOnBStat1));

                        _ldm12E2EBcMtoLdcm.UpdateData(new MatrixValDefinition(20, 2, _turnLghtLeftDRq2));
                        _ldm12E2EBcMtoLdcm.UpdateData(new MatrixValDefinition(22, 2, _turnLghtRightDRq2));

                        sendList.AddRange(
                            new[]
                            {
                                new CanBus.CanDataPackage(_ldm12E2EBcMtoLdcm.CanId, CanBus.CanProtocol.CanFd,
                                    CanBus.CanType.Standard, CanBus.CanFormat.Data, _ldm12E2EBcMtoLdcm.MatrixData)
                            });
                    }

                    Can.SendCanDatas(sendList.ToArray());

                    sendCount++;
                    if (sendCount > 9999)
                        sendCount = 0;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private byte Crc_CalculateCRC8(IReadOnlyList<int> crcDataPtr, byte crcLength, byte crcStartValue8)
        {
            const byte crcFinalXorCrc8 = 0xFF;

            byte crcLoopCounter;
            var crcValue = (byte)(crcFinalXorCrc8 ^ crcStartValue8);
            for (crcLoopCounter = 0; crcLoopCounter < crcLength; crcLoopCounter++)
                crcValue = (byte)_crcTable8[crcValue ^ crcDataPtr[crcLoopCounter]];
            return (byte)(crcValue ^ crcFinalXorCrc8);
        }

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
    }
}
