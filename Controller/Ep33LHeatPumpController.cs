using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,EP33L热泵控制器")]
    public sealed class Ep33LHeatPumpController : ControllerBase
    {
        public LinBus Lin;
        public CanBus Can;

        private Thread MainWorkThread { get; set; }
        private readonly object _controllerSenderLocker = new object();
        private const byte MasterLinId = 0x3C;
        private const byte SlaveLinId = 0x3D;
        private bool _isInExtendedSession;

        public Ep33LHeatPumpController(string name)
            : base(name)
        {
            if (MainWorkThread != null)
            {
                MainWorkThread.Abort();
                MainWorkThread.Join();
            }

            MainWorkThread = new Thread(
                MainWork) { IsBackground = true };
            MainWorkThread.Start();
        }

        ~Ep33LHeatPumpController()
        {
            Dispose();
        }

        private void MainWork()
        {
            while (MainWorkThread.IsAlive)
            {
                if (!MainWorkThread.IsAlive)
                    break;

                if (Lin == null)
                    continue;

                Thread.Sleep(1000);

                if (!_isInExtendedSession)
                    continue;

                var result = SendRecvMsg(MasterLinId, SlaveLinId,
                    new byte[] { 0x04, 0x02, 0x3E, 0x00, 0xAA, 0xAA, 0xAA, 0xAA }, 50);
                if (result != null && result.Length == 8 && result[2] == 0x7E)
                {

                }
                else
                    _isInExtendedSession = false;
            }
        }

        public string CheckCanMessage = string.Empty;

        public void CheckCanMsg(string delayMs)
        {
            CheckCanMessage = "NG";

            if (Can == null)
                return;

            var findCanIdList = new List<uint>
            {
                0x00,
                0x01,
                0x02,
                0x03,
                0x04,
                0x05,
                0x06,
                0x07,
                0x08,
                0x09,
                0x0A,
                0x0B,
                0x0C,
                0x0D,
                0x0E,
                0x0F,
                0x10,
                0x11,
                0x36,
                0x37,
                0x38,
                0x39,
                0x3A,
                0x2E,
                0x2F,
                0x30,
                0x3B
            };

            foreach (var canId in findCanIdList)
                Can.AddDoNotFilterCanId(canId);

            Can.CanRecvDataPackages.Clear();
            Thread.Sleep(int.Parse(delayMs));

            var isOk =
                findCanIdList.All(canId => Can.CanRecvDataPackages.FindAll(f => f.CanId == canId).Any());

            if (isOk)
                CheckCanMessage = @"OK";

            foreach (var canId in findCanIdList)
                Can.RemoveDoNotFilterCanId(canId);
        }

        #region 读取版本信息

        [Description("R,PartNumber")]
        public string PartNumber;

        [Description("R,HardwareNumber")]
        public string HardwareNumber;

        [Description("R,SoftwareNumber")]
        public string SoftwareNumber;

        [Description("R,NCFNumber")]
        public string NcfNumber;

        [Description("Read PartNumber")]
        public void ReadPartNumber()
        {
            PartNumber = SendRecvMsg(
                MasterLinId,
                SlaveLinId,
                new byte[] { 0x04, 0x01, 0xA0, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA }, 50)
                .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read HardwareNumber")]
        public void ReadHardwareNumber()
        {
            HardwareNumber = SendRecvMsg(
                MasterLinId,
                SlaveLinId,
                new byte[] { 0x04, 0x01, 0xA1, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA }, 100)
                .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read SoftwareNumber")]
        public void ReadSoftwareNumber()
        {
            SoftwareNumber = SendRecvMsg(
                MasterLinId,
                SlaveLinId,
                new byte[] { 0x04, 0x01, 0xA2, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA }, 50)
                .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("Read NCFNumber")]
        public void ReadNcfNumber()
        {
            NcfNumber = SendRecvMsg(
                MasterLinId,
                SlaveLinId,
                new byte[] { 0x04, 0x01, 0xA3, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA }, 50)
                .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        #endregion

        #region 拓展模式功能

        [Description("进入拓展模式")]
        public void EnterExtendedSession()
        {
            var result = SendRecvMsg(MasterLinId, SlaveLinId,
                new byte[] { 0x04, 0x02, 0x10, 0x03, 0xAA, 0xAA, 0xAA, 0xAA }, 50);
            if (result != null && result.Length == 8 && result[2] == 0x50 && result[3] == 0x03)
                _isInExtendedSession = true;
        }

        [Description("进入正常模式")]
        public void EnterNomalSeesion()
        {
            _isInExtendedSession = false;
            SendRecvMsg(
                MasterLinId, SlaveLinId, new byte[] { 0x04, 0x02, 0x10, 0x01, 0xAA, 0xAA, 0xAA, 0xAA }, 50);
        }

        [Description("安全解锁")]
        public void SecurityAccess()
        {
            var r = SendRecvMsg(MasterLinId, SlaveLinId,
                new byte[] { 0x04, 0x02, 0x27, 0x01, 0xAA, 0xAA, 0xAA, 0xAA }, 50);

            if (r == null || r.Length != 8 || r[2] != 0x67 || r[3] != 0x01)
                return;

            var seedBytes = new byte[4];
            Array.Copy(r, 4, seedBytes, 0, 4);
            Array.Reverse(seedBytes);

            var keyBytes =
                BitConverter.GetBytes(ComputeKey(BitConverter.ToUInt32(seedBytes, 0))).Reverse().ToArray();
            SendRecvMsg(MasterLinId, SlaveLinId,
                new byte[] { 0x04, 0x06, 0x27, 0x02, keyBytes[0], keyBytes[1], keyBytes[2], keyBytes[3] }, 50);
        }

        public void TestWriteErrprom1() { }

        public void TestReadErrprom1() { }

        public void TestWriteErrprom2() { }

        public void TestReadErrprom2() { }

        [Description("模块休眠")]
        public void SendSleepCmd()
        {
            _isInExtendedSession = false;
            Thread.Sleep(650);
            SendRecvMsg(MasterLinId, SlaveLinId, new byte[] { 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 50);
        }

        [Description("模块唤醒")]
        public void SendAwakeCmd()
        {
            for (var i = 0; i < 50; i++)
                SendRecvMsg(MasterLinId, SlaveLinId, new byte[] { 0x04, 0x01, 0xA0, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA }, 50);
        }

        public static uint ComputeKey(uint seed)
        {
            const uint safetyconstant = 0xDA89E08F;

            uint wTemp;
            uint wTop31Bits;

            var wLastSeed = seed;
            const uint temp = (safetyconstant & 0x00000800) >> 10 | ((safetyconstant & 0x00200000) >> 21);
            switch (temp)
            {
                case 0:
                    wTemp = (byte)((seed & 0xff000000) >> 24);
                    break;

                case 1:
                    wTemp = (byte)((seed & 0x00ff0000) >> 16);
                    break;

                case 2:
                    wTemp = (byte)((seed & 0x0000ff00) >> 8);
                    break;

                case 3:
                    wTemp = (byte)(seed & 0x000000ff);
                    break;

                default:
                    break;
            }

            const byte sb1 = (byte)((safetyconstant & 0x000003FC) >> 2);
            const byte sb2 = (byte)(((safetyconstant & 0x7F800000) >> 23) ^ 0xA5);
            const byte sb3 = (byte)(((safetyconstant & 0x001FE000) >> 13) ^ 0x5A);
            /* SB2 and SB3 determine the maximum number of passes through the loop.
            Size of SB2 and SB3 can be limited to fewer bits, to minimize the maximum number of passes through the algorithm
            The iterations calculation; where seedItByte, SB1, SB2 and SB3 are generated from fixed security constant;
             */
            var iterations = (byte)(((wTemp ^ sb1) & sb2) + sb3);
            for (var jj = 0; jj < iterations; jj++)
            {
                wTemp = ((wLastSeed & 0x40000000) / 0x40000000) ^ ((wLastSeed & 0x01000000) / 0x01000000)
                             ^ ((wLastSeed & 0x1000) / 0x1000) ^ ((wLastSeed & 0x04) / 0x04);
                var wLsBit = wTemp & 0x00000001;
                wLastSeed = wLastSeed << 1;
                wTop31Bits = wLastSeed & 0xFFFFFFFE;
                wLastSeed = wTop31Bits | wLsBit;
            }

            wTop31Bits = ((wLastSeed & 0x00FF0000) >> 16) | /*KEY[0] = Last_Seed[1]*/
                         ((wLastSeed & 0xFF000000) >> 8) |  /*KEY[1] = Last_Seed[0]*/
                         ((wLastSeed & 0x000000FF) << 8) |  /*KEY[2] = Last_Seed[3]*/
                         ((wLastSeed & 0x0000FF00) << 16);  /*KEY[3] = Last_Seed[2]*/
            wTop31Bits = wTop31Bits ^ safetyconstant;

            var key = wTop31Bits;
            return key;
        }

        #endregion

        #region 电源电压采集

        [Description("R,电源电压KL30_1")]
        public float Kl30Volt1;

        [Description("R,电源电压KL30_2")]
        public float Kl30Volt2;

        [Description("R,内部系统5V")]
        public float Sys5VSupply;

        [Description("R,传感器5V")]
        public float Sensor5VSupply;

        [Description("读取电源电压KL30_1")]
        public void ReadKl30Volt1()
        {
            Kl30Volt1 = ReadVolt(0xFD, 0x00);
        }

        [Description("读取电源电压KL30_2")]
        public void ReadKl30Volt2()
        {
            Kl30Volt2 = ReadVolt(0xFD, 0x01);
        }

        [Description("读取内部系统5V")]
        public void ReadSys5VSupply()
        {
            Sys5VSupply = ReadVolt(0xFD, 0x02);
        }

        [Description("读取传感器5V")]
        public void ReadSensor5VSupply()
        {
            Sensor5VSupply = ReadVolt(0xFD, 0x03);
        }

        #endregion

        #region 传感器电路采集

        [Description("R,传感器-1-PTCTemSnsr-FR-SIG")]
        public float PtcTemSnsr1Volt;

        [Description("R,传感器-2-PTCTemSnsr-FL-SIG")]
        public float PtcTemSnsr2Volt;

        [Description("R,传感器-3-IndrHeatExcgrInltTemSnsr-SIG")]
        public float IndrHeatExcgrInltTemSnsr3Volt;

        [Description("R,传感器-4-InltHeatExcgrOtltTemSnsr-SIG")]
        public float InltHeatExcgrOtltTemSnsr4Volt;

        [Description("R,传感器-5-OtdrHeatExcgrOtltTemSnsr-SIG")]
        public float OtdrHeatExcgrOtltTemSnsr5Volt;

        [Description("R,传感器-6-IntkComprTemSnsr-SIG")]
        public float IntkComprTemSnsr6Volt;

        [Description("R,传感器-7-IndrHeatEhgOtltPrsSnsr-SIG")]
        public float IndrHeatEhgOtltPrsSnsr7Volt;

        [Description("R,传感器-8-IntkComprPrsSnsr-SIG")]
        public float IntkComprPrsSnsr8Volt;

        [Description("R,传感器-9-OutsideCondensor outside pressure sensor")]
        public float OutsideSnsr9Volt;

        [Description("R,传感器-10-CON2-PT")]
        public float Con2Snsr10Volt;

        [Description("读传感器-1-PTCTemSnsr-FR-SIG")]
        public void ReadPtcTemSnsr1Volt()
        {
            PtcTemSnsr1Volt = ReadVolt(0xFD, 0x04);
        }

        [Description("读传感器-2-PTCTemSnsr-FL-SIG")]
        public void ReadPtcTemSnsr2Volt()
        {
            PtcTemSnsr2Volt = ReadVolt(0xFD, 0x05);
        }

        [Description("读传感器-3-IndrHeatExcgrInltTemSnsr-SIG")]
        public void ReadIndrHeatExcgrInltTemSnsr3Volt()
        {
            IndrHeatExcgrInltTemSnsr3Volt = ReadVolt(0xFD, 0x06);
        }

        [Description("读传感器-4-InltHeatExcgrOtltTemSnsr-SIG")]
        public void ReadInltHeatExcgrOtltTemSnsr4Volt()
        {
            InltHeatExcgrOtltTemSnsr4Volt = ReadVolt(0xFD, 0x07);
        }

        [Description("读传感器-5-OtdrHeatExcgrOtltTemSnsr-SIG")]
        public void ReadOtdrHeatExcgrOtltTemSnsr5Volt()
        {
            OtdrHeatExcgrOtltTemSnsr5Volt = ReadVolt(0xFD, 0x08);
        }

        [Description("读传感器-6-IntkComprTemSnsr-SIG")]
        public void ReadIntkComprTemSnsr6Volt()
        {
            IntkComprTemSnsr6Volt = ReadVolt(0xFD, 0x09);
        }

        [Description("读传感器-7-IndrHeatEhgOtltPrsSnsr-SIG")]
        public void ReadIndrHeatEhgOtltPrsSnsr7Volt()
        {
            IndrHeatEhgOtltPrsSnsr7Volt = ReadVolt(0xFD, 0x0A);
        }

        [Description("读传感器-8-IntkComprPrsSnsr-SIG")]
        public void ReadIntkComprPrsSnsr8Volt()
        {
            IntkComprPrsSnsr8Volt = ReadVolt(0xFD, 0x0B);
        }

        [Description("读传感器-9-OutsideCondensor-outside-pressure-sensor")]
        public void ReadOutsideSnsr9Volt()
        {
            OutsideSnsr9Volt = ReadVolt(0xFD, 0x0C);
        }

        [Description("读传感器-10-CON2-PT")]
        public void ReadCon2Snsr10Volt()
        {
            Con2Snsr10Volt = ReadVolt(0xFD, 0x0D);
        }

        #endregion

        #region VN7050输出 IO Control

        [Description("打开VN7050高边输出-1-HtngVlv-HSD")]
        public void OpenHtngVlvHsd1()
        {
            SwitchHsd(0x01, 0xFD, 0x30);
        }

        [Description("关闭VN7050高边输出-1-HtngVlv-HSD")]
        public void CloseHtngVlvHsd1()
        {
            SwitchHsd(0x00, 0xFD, 0x30);
        }

        [Description("打开VN7050高边输出-2-CoolngVlv-HSD")]
        public void OpenCoolngVlvHsd2()
        {
            SwitchHsd(0x01, 0xFD, 0x31);
        }

        [Description("关闭VN7050高边输出-2-CoolngVlv-HSD")]
        public void CloseCoolngVlvHsd2()
        {
            SwitchHsd(0x00, 0xFD, 0x31);
        }

        [Description("打开VN7050高边输出-3-BypsVlv-HSD")]
        public void OpenBypsVlvHsd3()
        {
            SwitchHsd(0x01, 0xFD, 0x32);
        }

        [Description("关闭VN7050高边输出-3-BypsVlv-HSD")]
        public void CloseBypsVlvHsd3()
        {
            SwitchHsd(0x00, 0xFD, 0x32);
        }

        [Description("打开VN7050高边输出-4-DhmdyVlv-HSD")]
        public void OpenDhmdyVlvHsd4()
        {
            SwitchHsd(0x01, 0xFD, 0x33);
        }

        [Description("关闭VN7050高边输出-4-DhmdyVlv-HSD")]
        public void CloseDhmdyVlvHsd4()
        {
            SwitchHsd(0x00, 0xFD, 0x33);
        }

        [Description("打开VN7050高边输出-5-CabinTXV-HSD")]
        public void OpenCabinTxvHsd5()
        {
            SwitchHsd(0x01, 0xFD, 0x34);
        }

        [Description("关闭VN7050高边输出-5-CabinTXV-HSD")]
        public void CloseCabinTxvHsd5()
        {
            SwitchHsd(0x00, 0xFD, 0x34);
        }

        private void SwitchHsd(byte isOn, byte didHi, byte didLo)
        {
            SendRecvMsg(
                MasterLinId,
                SlaveLinId,
                new byte[] { 0x04, 0x05, 0x2F, didHi, didLo, 0x03, isOn, 0xAA },
                50);
        }

        [Description("R,高边输出-1-CoolngVlv-HSD—状态查询")]
        public string HtngVlvHsd1State;

        [Description("R,高边输出-2-HtngVlv-HSD—状态查询")]
        public string CoolngVlvHsd2State;

        [Description("R,高边输出-3-BypsVlv-HSD—状态查询")]
        public string BypsVlvHsd3State;

        [Description("R,高边输出-4-DhmdyVlv-HSD—状态查询")]
        public string DhmdyVlvHsd4State;

        [Description("R,高边输出-5-CabinTxv-HSD—状态查询")]
        public string CabinTxvHsd5State;

        [Description("高边输出-1-CoolngVlv-HSD—状态查询")]
        public void ReadHtngVlvHsd1State()
        {
            HtngVlvHsd1State = string.Empty;
            HtngVlvHsd1State = ReadDid(MasterLinId, SlaveLinId, 0xFD, 0x30)
                .Aggregate(HtngVlvHsd1State, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("高边输出-2-HtngVlv-HSD—状态查询")]
        public void ReadCoolngVlvHsd2State()
        {
            CoolngVlvHsd2State = string.Empty;
            CoolngVlvHsd2State = ReadDid(MasterLinId, SlaveLinId, 0xFD, 0x31)
                .Aggregate(CoolngVlvHsd2State, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("高边输出-3-BypsVlv-HSD—状态查询")]
        public void ReadBypsVlvHsd3State()
        {
            BypsVlvHsd3State = string.Empty;
            BypsVlvHsd3State = ReadDid(MasterLinId, SlaveLinId, 0xFD, 0x32)
                .Aggregate(BypsVlvHsd3State, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("高边输出-4-DhmdyVlv-HSD—状态查询")]
        public void ReadDhmdyVlvHsd4State()
        {
            DhmdyVlvHsd4State = string.Empty;
            DhmdyVlvHsd4State = ReadDid(MasterLinId, SlaveLinId, 0xFD, 0x33)
                .Aggregate(DhmdyVlvHsd4State, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("高边输出-5-CabinTxv-HSD—状态查询")]
        public void ReadCabinTxvHsd5State()
        {
            CabinTxvHsd5State = string.Empty;
            CabinTxvHsd5State = ReadDid(MasterLinId, SlaveLinId, 0xFD, 0x34)
               .Aggregate(CabinTxvHsd5State, (current, t) => current + ValueHelper.GetHextStr(t));
        }

        [Description("高边输出-1-CoolngVlv-HSD—退出控制")]
        public void ExitHtngVlvHsd1Control()
        {
            ExitHsdControl(0xFD, 0x30);
        }

        [Description("高边输出-2-HtngVlv-HSD—退出控制")]
        public void ExitCoolngVlvHsd2Control()
        {
            ExitHsdControl(0xFD, 0x31);
        }

        [Description("高边输出-3-BypsVlv-HSD—退出控制")]
        public void ExitVlvHsd3Control()
        {
            ExitHsdControl(0xFD, 0x32);
        }

        [Description("高边输出-4-DhmdyVlv-HSD—退出控制")]
        public void ExitDhmdyVlvHsd4Control()
        {
            ExitHsdControl(0xFD, 0x33);
        }

        [Description("高边输出-5-CabinTxv-HSD—退出控制")]
        public void ExitCabinTxvHsd5Control()
        {
            ExitHsdControl(0xFD, 0x34);
        }

        private void ExitHsdControl(byte didHi, byte didLo)
        {
            if (Lin == null)
                return;

            SendRecvMsg(MasterLinId, SlaveLinId, new byte[] { 0x04, 0x04, 0x2F, didHi, didLo, 0x00, 0xAA, 0xAA }, 50);
        }

        #endregion

        #region VN7050 Multisense模拟量输入

        [Description("R,Multisense模拟量输入检测-输出反馈电压1")]
        public float MultisenseAdVolt1;

        [Description("R,Multisense模拟量输入检测-输出反馈电压2")]
        public float MultisenseAdVolt2;

        [Description("R,Multisense模拟量输入检测-输出反馈电压3")]
        public float MultisenseAdVolt3;

        [Description("R,Multisense模拟量输入检测-输出反馈电压4")]
        public float MultisenseAdVolt4;

        [Description("R,Multisense模拟量输入检测-输出反馈电压5")]
        public float MultisenseAdVolt5;

        [Description("读取Multisense模拟量输入检测-输出反馈电压1")]
        public void ReadMultisenseAdVolt1()
        {
            MultisenseAdVolt1 = ReadVolt(0xFD, 0x35);
        }

        [Description("读取Multisense模拟量输入检测-输出反馈电压2")]
        public void ReadMultisenseAdVolt2()
        {
            MultisenseAdVolt2 = ReadVolt(0xFD, 0x36);
        }

        [Description("读取Multisense模拟量输入检测-输出反馈电压3")]
        public void ReadMultisenseAdVolt3()
        {
            MultisenseAdVolt3 = ReadVolt(0xFD, 0x37);
        }

        [Description("读取Multisense模拟量输入检测-输出反馈电压4")]
        public void ReadMultisenseAdVolt4()
        {
            MultisenseAdVolt4 = ReadVolt(0xFD, 0x38);
        }

        [Description("读取Multisense模拟量输入检测-输出反馈电压5")]
        public void ReadMultisenseAdVolt5()
        {
            MultisenseAdVolt5 = ReadVolt(0xFD, 0x39);
        }

        #endregion

        #region ECCV A4980输出

        [Description("R,ECCV1 control当前步数")]
        public float Eccv1ControlStep;

        [Description("R,ECCV2 control当前步数")]
        public float Eccv2ControlStep;

        [Description("R,ECCV3 control当前步数")]
        public float Eccv3ControlStep;

        [Description("写入ECCV1 control目标步数")]
        public void WriteEccv1ControlStep(string step)
        {
            if (string.IsNullOrEmpty(step))
                return;

            ushort stepValue;
            if (!ushort.TryParse(step, out stepValue))
                return;

            var stepBytes = BitConverter.GetBytes(stepValue);
            SendRecvMsg(MasterLinId, SlaveLinId,
                new byte[] { 0x04, 0x06, 0x2F, 0xFD, 0x41, 0x03, stepBytes[1], stepBytes[0] }, 50);
        }

        [Description("读取ECCV1 control当前步数")]
        public void ReadEccv1ControlStep()
        {
            Eccv1ControlStep = ReadStep(0xFD, 0x41);
        }

        [Description("写入ECCV2 Clockwise目标步数")]
        public void WriteEccv2ControlStep(string step)
        {
            if (string.IsNullOrEmpty(step))
                return;

            ushort stepValue;
            if (!ushort.TryParse(step, out stepValue))
                return;

            var stepBytes = BitConverter.GetBytes(stepValue);
            SendRecvMsg(MasterLinId, SlaveLinId,
                new byte[] { 0x04, 0x06, 0x2F, 0xFD, 0x43, 0x03, stepBytes[1], stepBytes[0] }, 50);
        }

        [Description("读取ECCV2 Clockwise当前步数")]
        public void ReadEccv2ControlStep()
        {
            Eccv2ControlStep = ReadStep(0xFD, 0x43);
        }

        [Description("写入ECCV3 Clockwise目标步数")]
        public void WriteEccv3ControlStep(string step)
        {
            if (string.IsNullOrEmpty(step))
                return;

            ushort stepValue;
            if (!ushort.TryParse(step, out stepValue))
                return;

            var stepBytes = BitConverter.GetBytes(stepValue);
            SendRecvMsg(MasterLinId, SlaveLinId,
                new byte[] { 0x04, 0x06, 0x2F, 0xFD, 0x45, 0x03, stepBytes[1], stepBytes[0] }, 50);
        }

        [Description("读取ECCV3 Clockwise当前步数")]
        public void ReadEccv3ControlStep()
        {
            Eccv3ControlStep = ReadStep(0xFD, 0x45);
        }

        [Description("退出ECCV1 control")]
        public void ExitEccv1Control()
        {
            ExitEccvControl(0xFD, 0x41);
        }

        [Description("退出ECCV2 control")]
        public void ExitEccv2Control()
        {
            ExitEccvControl(0xFD, 0x43);
        }

        [Description("退出ECCV3 control")]
        public void ExitEccv3Control()
        {
            ExitEccvControl(0xFD, 0x45);
        }

        private void ExitEccvControl(byte didHi, byte didLo)
        {
            if (Lin == null)
                return;

            SendRecvMsg(MasterLinId, SlaveLinId,
                new byte[] { 0x04, 0x04, 0x2F, didHi, didLo, 0x00, 0xAA, 0xAA }, 50);
        }

        #endregion

        #region 类方法
        private byte[] SendRecvMsg(
            byte masterLinId,
            byte slaveLinId,
            byte[] masterLinData,
            int delayMs)
        {
            if (Lin == null)
                return new byte[0];

            lock (_controllerSenderLocker)
            {
                byte[] echo;
                if (Lin.SendMasterLinAndRecvSingleSlaveLin(
                    masterLinId, slaveLinId, masterLinData, out echo, delayMs) && echo != null)
                    return echo;

                return new byte[0];
            }
        }

        private byte[] ReadDid(
            byte masterLinId,
            byte slaveLinId,
            byte didHi,
            byte didLo)
        {
            if (Lin == null)
                return new byte[0];

            var readFunc = new Func<byte[]>(() =>
            {
                var result = SendRecvMsg(
                    masterLinId, slaveLinId, new byte[] { 0x04, 0x03, 0x22, didHi, didLo, 0xAA, 0xAA, 0xAA }, 50);

                if (result != null && result.Length == 8 && result[1] < 0x10)
                {
                    if (result[2] == 0x62 && result[3] == didHi && result[4] == didLo)
                    {
                        var newBs = new byte[result[1] - 3];
                        Array.Copy(result, 5, newBs, 0, newBs.Length);
                        return newBs;
                    }
                }

                return new byte[0];
            });

            var r1 = readFunc.Invoke();
            if (r1.Any())
                return r1;

            Thread.Sleep(500);
            return readFunc.Invoke();
        }

        private float ReadVolt(byte didHi, byte didLo)
        {
            var result = ReadDid(MasterLinId, SlaveLinId, didHi, didLo);
            if (result.Any())
                return ValueHelper.GetDecimal(result) * 0.001f;

            return -9999f;
        }

        private float ReadStep(byte didHi, byte didLo)
        {
            if (Lin == null)
                return -9999f;

            var readFunc = new Func<byte[]>(() => ReadDid(MasterLinId, SlaveLinId, didHi, didLo));

            var r1 = readFunc.Invoke();
            if (r1.Any())
                return ValueHelper.GetDecimal(r1);

            return -9999f;
        }

        #endregion
    }
}
