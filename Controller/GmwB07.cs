using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Controller
{
    [Description("CAN-Product,GMWB07")]
    public sealed class GmwB07 : ControllerBase
    {
        public CanBus Can;

        public GmwB07(string name)
            : base(name) => SysConfig(Directory.GetCurrentDirectory() + @"\ControllerConfig\GWM.dbc");

        ~GmwB07() => Dispose();

        #region LED相关

        [Description("唤醒")]
        public void LampAwake() => _isSleep = false;

        [Description("休眠")]
        public void LampSleep()
        {
            _isSleep = true;
            _rollingcounterBcm10 = 0x00;
        }

        [Description("近光开")]
        public void LbOn()
        {
            _isHbOn = false;
            _isLbOn = true;
        }

        [Description("近光关")]
        public void LbOff()
        {
            _isHbOn = false;
            _isLbOn = false;
        }

        [Description("远光开")]
        public void HbOn()
        {
            _isLbOn = true;
            _isHbOn = true;
        }

        [Description("远光关")]
        public void HbOff()=> _isHbOn = false;

        [Description("DRL开")]
        public void DrlOn()
        {
            _isPlOn = false;
            _isDrlOn = true;
        }

        [Description("DRL关")]
        public void DrlOff()=> _isDrlOn = false;

        [Description("PL开")]
        public void PlOn()
        {
            _isDrlOn = false;
            _isPlOn = true;
        }

        [Description("PL关")]
        public void PlOff() => _isPlOn = false;

        [Description("转向开")]
        public void TurnOn() => _isTurnOn = true;

        [Description("转向关")]
        public void TurnOff() => _isTurnOn = false;

        #endregion

        #region Code from CANoe and DBC

        private void SysConfig(string dbcFilePath)
        {
            VectorEmulator =
                new VectorDbcEmulator(new[] { dbcFilePath });
            InitVariables();
            PreStart();
            Start();
        }

        private VectorDbcEmulator VectorEmulator { get; set; }

        private bool _isSleep = true;
        private bool _isHbOn;
        private bool _isLbOn;
        private bool _isDrlOn;
        private bool _isPlOn;
        private bool _isTurnOn;

        public const string CemNm = "CEM_NM";
        public const string Bcm10 = "BCM10";
        public const string Bcm17 = "BCM17";
        public const string Bcm8 = "BCM8";
        public const string Eem1 = "EEM1";
        public const string Peps1 = "PEPS1";
        public const string Peps2 = "PEPS2";

        private byte _rfPosnLmpWorkCmd;
        private byte _lfPosnLmpWorkCmd;
        private byte _hiBeamWorkCmd;
        private byte _lowBeamWorkCmd;
        private byte _rTurnLmpWorkCmd;
        private byte _lTurnLampWorkCmd;

        private byte _rDayRunngLmpWorkCmd;
        private byte _lDayRunngLmpWorkCmd;

        private byte _rollingcounterBcm10;
        private byte _checksumBcm10;

        private void InitVariables()
        {
            VectorEmulator.InitMessageVariable(CemNm, CemNm);

            VectorEmulator.InitMessageVariable(Bcm10, Bcm10);
            VectorEmulator.InitMessageVariable(Bcm17, Bcm17);
            VectorEmulator.InitMessageVariable(Bcm8, Bcm8);

            VectorEmulator.InitMessageVariable(Eem1, Eem1);
            VectorEmulator.InitMessageVariable(Peps1, Peps1);
            VectorEmulator.InitMessageVariable(Peps2, Peps2);
        }

        private void PreStart()
        {
            VectorEmulator.SetMessageVariableByteValue(CemNm, 0, 0x4F);
            VectorEmulator.SetMessageVariableByteValue(CemNm, 1, 0x01);
        }

        private void Start()
        {
            VectorEmulator.SetTimer(MsgTimer20Ms(), 20);
            VectorEmulator.SetTimer(MsgTimer50Ms(), 50);
            VectorEmulator.SetTimer(MsgTimer100Ms(), 100);
            VectorEmulator.SetTimer(MsgTimer200Ms(), 250);
        }

        /// <summary>
        /// NM 0x501
        /// </summary>
        /// <returns></returns>
        private Action MsgTimer20Ms()
        {
            return () =>
            {
                if (_isSleep)
                    return;
                VectorEmulator.OutPut(CemNm, Can);
            };
        }

        /// <summary>
        /// BCM17 0x23E
        /// PEPS2 0x295
        /// </summary>
        /// <returns></returns>
        private Action MsgTimer50Ms()
        {
            return () =>
            {
                if (_isSleep)
                    return;
                VectorEmulator.OutPut(new[] { Bcm17, Peps2 }, Can);
            };
        }

        /// <summary>
        /// BCM8 0x29F
        /// EEM1 0x2A8
        /// </summary>
        /// <returns></returns>
        private Action MsgTimer100Ms()
        {
            return () =>
            {
                if (_isSleep)
                    return;
                VectorEmulator.OutPut(new[] { Bcm8, Eem1 }, Can);
            };
        }

        /// <summary>
        /// BCM10 0x2E7
        /// </summary>
        /// <returns></returns>
        private Action MsgTimer200Ms()
        {
            return () =>
            {
                if (_isSleep)
                    return;

                _rfPosnLmpWorkCmd = _isPlOn ? (byte)1 : (byte)0;
                _lfPosnLmpWorkCmd = _isPlOn ? (byte)1 : (byte)0;
                _hiBeamWorkCmd = _isHbOn ? (byte)1 : (byte)0;
                _lowBeamWorkCmd = _isLbOn ? (byte)1 : (byte)0;
                _rTurnLmpWorkCmd = _isTurnOn ? (byte)1 : (byte)0;
                _lTurnLampWorkCmd = _isTurnOn ? (byte)1 : (byte)0;

                _rDayRunngLmpWorkCmd = _isDrlOn ? (byte)1 : (byte)0;
                _lDayRunngLmpWorkCmd = _isDrlOn ? (byte)1 : (byte)0;

                var a = new byte[9];
                a[0] = 0xE7;
                a[1] = 0x02;

                var byte2 = (_rfPosnLmpWorkCmd << 7) + (_lfPosnLmpWorkCmd << 4) +
                            (_hiBeamWorkCmd << 3) + (_lowBeamWorkCmd << 2) +
                            (_rTurnLmpWorkCmd << 1) + _lTurnLampWorkCmd;
                var byte3 = (_rDayRunngLmpWorkCmd << 5) + (_lDayRunngLmpWorkCmd << 4);
                a[2] = (byte)byte2;
                a[3] = (byte)byte3;
                a[8] = _rollingcounterBcm10;

                _checksumBcm10 = Crc(a);

                _rollingcounterBcm10 = (byte)(_rollingcounterBcm10 + 1);
                if (_rollingcounterBcm10 > 0x0F)
                    _rollingcounterBcm10 = 0;

                VectorEmulator.SetMessageVariableSignalValue(Bcm10, "Checksum_BCM10", _checksumBcm10);
                VectorEmulator.SetMessageVariableSignalValue(Bcm10, "Rollingcounter_BCM10", _rollingcounterBcm10);

                // pl
                VectorEmulator.SetMessageVariableSignalValue(Bcm10, "LFPosnLmpWorkCmd", _lfPosnLmpWorkCmd);
                VectorEmulator.SetMessageVariableSignalValue(Bcm10, "RFPosnLmpWorkCmd", _rfPosnLmpWorkCmd);

                // hi beam
                VectorEmulator.SetMessageVariableSignalValue(Bcm10, "HiBeamWorkCmd", _hiBeamWorkCmd);

                // low beam
                VectorEmulator.SetMessageVariableSignalValue(Bcm10, "LowBeamWorkCmd", _lowBeamWorkCmd);

                // turn
                VectorEmulator.SetMessageVariableSignalValue(Bcm10, "LTurnLampWorkCmd", _lTurnLampWorkCmd);
                VectorEmulator.SetMessageVariableSignalValue(Bcm10, "RTurnLmpWorkCmd", _rTurnLmpWorkCmd);

                // drl
                VectorEmulator.SetMessageVariableSignalValue(Bcm10, "LDayRunngLmpWorkCmd", _lDayRunngLmpWorkCmd);
                VectorEmulator.SetMessageVariableSignalValue(Bcm10, "RDayRunngLmpWorkCmd", _rDayRunngLmpWorkCmd);

                VectorEmulator.OutPut(Bcm10, Can);
            };
        }

        private static byte Crc(IReadOnlyList<byte> a)
        {
            const byte poly = 0x1D;
            int byteIndex;
            var crc = 0;
            for (byteIndex = 0; byteIndex < 9; byteIndex++)
            {
                crc ^= a[byteIndex];
                int bitIndex;
                for (bitIndex = 0; bitIndex < 8; bitIndex++)
                {
                    if ((crc & 0x80) != 0)
                    {
                        crc = (crc << 1) ^ poly;
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }

            crc ^= 0x00;

            return (byte)crc;
        }

        #endregion
    }
}