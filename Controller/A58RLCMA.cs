using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,A58_RLCMA")]
    public sealed class A58Rlcma : ControllerBase
    {
        public CanBus Can;

        private VectorDbcEmulator VectorEmulator { get; set; }

        [Description("R/W,PL_PWM")]
        public string PlPwm = 0.ToString();

        [Description("R/W,TL_PWM")]
        public string TlPwm = 0.ToString();

        [Description("R,左右识别")]
        public bool IsLeft;

        [Description("R,应用程序版本号")]
        public string AppVer;

        [Description("R,引导程序版本号")]
        public string FblVer;

        [Description("R,配置程序版本号")]
        public string CfgVer;

        public A58Rlcma(string name)
            : base(name)
        {
            SysConfig(Directory.GetCurrentDirectory() + @"\ControllerConfig\RLCMACAN_6.dbc");
        }

        ~A58Rlcma()
        {
            Dispose();
        }

        private uint _canDiagnosisRequestPhyCanId = 0x75B;
        //private const uint CanDiagnosisRequestFunCanId = 0x7DF;
        private uint _canDiagnosisResponseCanId = 0x7DB;

        [Description("设置为左节点")]
        public void SetLeft()
        {
            IsLeft = true;
        }

        [Description("设置为右节点")]
        public void SetRight()
        {
            IsLeft = false;
        }

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

                    Thread.Sleep(250);
                    if (!Can.CanBusWithUds.TryReadData(
                        _canDiagnosisRequestPhyCanId,
                        _canDiagnosisResponseCanId,
                        CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                        didHi, didLo, out echoBytes))
                        return new List<byte>().ToArray();
                    return echoBytes ?? new List<byte>().ToArray();
                }

                return new List<byte>().ToArray();

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
            VectorEmulator.InitMessageVariable("GW_ACU_10_L", "msg_35A");
            VectorEmulator.InitMessageVariable("Nm_LCU", "msg_54F");
            VectorEmulator.InitMessageVariable("LCU_MASTER_1", "msg_1E9");

            VectorEmulator.InitMessageVariable("LftRrPlCmd1", "LftRrPlCmd1");
            VectorEmulator.InitMessageVariable("LftRrPlCmd2", "LftRrPlCmd2");
            VectorEmulator.InitMessageVariable("LftRrPlCmd3", "LftRrPlCmd3");
            VectorEmulator.InitMessageVariable("LftRrPlCmd4", "LftRrPlCmd4");
            VectorEmulator.InitMessageVariable("RghtRrPlCmd1", "RghtRrPlCmd1");
            VectorEmulator.InitMessageVariable("RghtRrPlCmd2", "RghtRrPlCmd2");
            VectorEmulator.InitMessageVariable("RghtRrPlCmd3", "RghtRrPlCmd3");
            VectorEmulator.InitMessageVariable("RghtRrPlCmd4", "RghtRrPlCmd4");

            VectorEmulator.InitMessageVariable("LftRrTrnCmd1", "LftRrTrnCmd1");
            VectorEmulator.InitMessageVariable("RghtRrTrnCmd1", "RghtRrTrnCmd1");
        }

        private void Prestart()
        {
            VectorEmulator.SetMessageVariableSignalValue("msg_1E9", "LCU_LftTrnSts", 0x01);
            VectorEmulator.SetMessageVariableSignalValue("msg_1E9", "LCU_RghtTrnSts", 0x01);
            VectorEmulator.SetMessageVariableSignalValue("msg_1E9", "LCU_LftRrPlSts", 0x01);
            VectorEmulator.SetMessageVariableSignalValue("msg_1E9", "LCU_RghtRrPlSts", 0x01);
        }

        private void Start()
        {
            VectorEmulator.SetTimer(MsgTimer20Ms(), 50);
            VectorEmulator.SetTimer(MsgTimer20MsTl(), 50);
            VectorEmulator.SetTimer(MsgTimer200Ms(), 200);
            VectorEmulator.SetTimer(MsgTimer500Ms(), 500);
        }

        private Action MsgTimer20Ms()
        {
            return () =>
            {
                byte plPwm;
                if (!string.IsNullOrEmpty(PlPwm) && byte.TryParse(PlPwm, out plPwm))
                {
                    if (plPwm > 0x64)
                    {
                        plPwm = 0x64;
                    }
                }
                else
                {
                    plPwm = 0x64;
                }

                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd1", "LCU_LftRrAPL01Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd1", "LCU_LftRrAPL02Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd1", "LCU_LftRrAPL03Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd1", "LCU_LftRrAPL04Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd1", "LCU_LftRrAPL05Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd1", "LCU_LftRrAPL06Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd1", "LCU_LftRrAPL07Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd1", "LCU_LftRrAPL08Cmd", plPwm);

                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd2", "LCU_LftRrAPL09Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd2", "LCU_LftRrAPL10Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd2", "LCU_LftRrAPL11Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd2", "LCU_LftRrAPL12Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd2", "LCU_LftRrAPL13Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd2", "LCU_LftRrAPL14Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd2", "LCU_LftRrAPL15Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd2", "LCU_LftRrAPL16Cmd", plPwm);

                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd3", "LCU_LftRrAPL17Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd3", "LCU_LftRrAPL18Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd3", "LCU_LftRrAPL19Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd3", "LCU_LftRrAPL20Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd3", "LCU_LftRrAPL21Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd3", "LCU_LftRrAPL22Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd3", "LCU_LftRrAPL23Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd3", "LCU_LftRrAPL24Cmd", plPwm);

                VectorEmulator.SetMessageVariableSignalValue("LftRrPlCmd4", "LCU_LftRrAPL25Cmd", plPwm);

                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd1", "LCU_RghtRrAPL01Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd1", "LCU_RghtRrAPL02Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd1", "LCU_RghtRrAPL03Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd1", "LCU_RghtRrAPL04Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd1", "LCU_RghtRrAPL05Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd1", "LCU_RghtRrAPL06Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd1", "LCU_RghtRrAPL07Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd1", "LCU_RghtRrAPL08Cmd", plPwm);

                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd2", "LCU_RghtRrAPL09Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd2", "LCU_RghtRrAPL10Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd2", "LCU_RghtRrAPL11Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd2", "LCU_RghtRrAPL12Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd2", "LCU_RghtRrAPL13Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd2", "LCU_RghtRrAPL14Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd2", "LCU_RghtRrAPL15Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd2", "LCU_RghtRrAPL16Cmd", plPwm);

                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd3", "LCU_RghtRrAPL17Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd3", "LCU_RghtRrAPL18Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd3", "LCU_RghtRrAPL19Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd3", "LCU_RghtRrAPL20Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd3", "LCU_RghtRrAPL21Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd3", "LCU_RghtRrAPL22Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd3", "LCU_RghtRrAPL23Cmd", plPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd3", "LCU_RghtRrAPL24Cmd", plPwm);

                VectorEmulator.SetMessageVariableSignalValue("RghtRrPlCmd4", "LCU_RghtRrAPL25Cmd", plPwm);

                VectorEmulator.OutPut("msg_1E9", Can);

                if (IsLeft)
                {
                    VectorEmulator.OutPut(new[] { "LftRrPlCmd1", "LftRrPlCmd2", "LftRrPlCmd3", "LftRrPlCmd4" }, Can);

                    //VectorEmulator.OutPut("", Can);
                    //VectorEmulator.OutPut("", Can);
                    //VectorEmulator.OutPut("", Can);
                    //VectorEmulator.OutPut("", Can);
                }
                else
                {
                    VectorEmulator.OutPut(new[] { "RghtRrPlCmd1", "RghtRrPlCmd2", "RghtRrPlCmd3", "RghtRrPlCmd4" }, Can);

                    //VectorEmulator.OutPut("", Can);
                    //VectorEmulator.OutPut("", Can);
                    //VectorEmulator.OutPut("", Can);
                    //VectorEmulator.OutPut("", Can);
                }
            };
        }

        private Action MsgTimer20MsTl()
        {
            return () =>
            {
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

                VectorEmulator.SetMessageVariableSignalValue("LftRrTrnCmd1", "LCU_LftRrATL01Cmd", tlPwm);
                VectorEmulator.SetMessageVariableSignalValue("RghtRrTrnCmd1", "LCU_RghtRrATL01Cmd", tlPwm);

                if (IsLeft)
                {
                    VectorEmulator.OutPut("LftRrTrnCmd1", Can);
                }
                else
                {
                    VectorEmulator.OutPut("RghtRrTrnCmd1", Can);
                }
            };
        }

        private Action MsgTimer200Ms()
        {
            return () =>
            {
                VectorEmulator.OutPut("msg_54F", Can);
            };
        }

        private Action MsgTimer500Ms()
        {
            return () =>
            {
                VectorEmulator.OutPut("msg_35A", Can);
            };
        }

        #endregion
    }
}
