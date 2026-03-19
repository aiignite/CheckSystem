using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,E2LB前灯高配-P00133228")]
    public sealed class E2LbGpFrontLamp : ControllerBase
    {
        public CanBus Can;

        public E2LbGpFrontLamp(string name)
            : base(name)
        {
            const string str = "0x102,0x8c,0xcb,0x54,0x31,0x70,0x9f,0x7c,0xaf,0x5f,0x9e,0x91,0x66,0x79,0x23,0xd8,0xe5\r\n" +
                               "0x103,0x9d,0x6f,0x0e,0x62,0x56,0x80,0x0d,0x14,0x5c,0xd4,0x81,0x43,0x56,0xf1,0xc9,0xb4\r\n" +
                               "0x104,0x66,0x92,0xfe,0xbb,0xad,0x8b,0x1a,0xa3,0x17,0xae,0x67,0x9f,0xa8,0x22,0x88,0x84\r\n" +
                               "0x105,0xd7,0x47,0xf7,0x50,0x3f,0xd2,0xea,0xd9,0x64,0x4f,0x1d,0x41,0xdf,0x5c,0xbb,0xa3\r\n" +
                               "0x106,0x02,0x4c,0x63,0x04,0xfc,0x21,0xfb,0xe6,0xe1,0xa7,0xae,0x2f,0x37,0xb6,0x47,0xec\r\n" +
                               "0x107,0x7e,0x59,0xf4,0x98,0x7c,0xac,0xe1,0x6d,0x05,0x6e,0xf7,0x6d,0xf2,0x7e,0x91,0x14\r\n" +
                               "0x108,0x85,0xff,0xa4,0x86,0xb5,0xd3,0x53,0xcc,0xe0,0x3b,0x46,0x07,0x8b,0xd3,0x49,0xa1\r\n" +
                               "0x109,0x80,0xc8,0x7e,0xa2,0x52,0x84,0xa3,0xbd,0x10,0x77,0xac,0x7e,0x17,0x20,0xae,0xe0\r\n" +
                               "0x10A,0x6b,0x1f,0xfd,0xcc,0xd8,0x09,0xbb,0xb0,0xbf,0xc8,0xf2,0xbf,0xb9,0x72,0x7e,0xf1\r\n" +
                               "0x10B,0x15,0xa4,0x60,0xcd,0x24,0xf7,0x2e,0x69,0x82,0xd6,0xa1,0x3c,0x4a,0x53,0xc2,0x2b\r\n" +
                               "0x10C,0xb8,0x05,0x50,0x29,0x66,0x7a,0x49,0x3f,0x2d,0xd7,0xf5,0xc0,0x9a,0x84,0xfb,0x85\r\n" +
                               "0x10D,0xec,0x18,0x2b,0x58,0xe0,0x5e,0x3b,0x8f,0x12,0x87,0x2e,0xfa,0xbd,0x47,0xd6,0xa6\r\n" +
                               "0x10E,0xb9,0x06,0x51,0x30,0x67,0x7b,0x4a,0x40,0x2e,0xd8,0xf6,0xc1,0x9b,0x85,0xfc,0x86\r\n" +
                               "0x10F,0xed,0x19,0x2c,0x59,0xe1,0x5f,0x3c,0x90,0x13,0x88,0x2f,0xfb,0xbe,0x48,0xd7,0xa7\r\n" +
                               "0x110,0xba,0x07,0x52,0x31,0x68,0x7c,0x4b,0x41,0x2f,0xd9,0xf7,0xc2,0x9c,0x86,0xfd,0x87\r\n" +
                               "0x111,0xee,0x1a,0x2d,0x5a,0xe2,0x60,0x3d,0x91,0x14,0x89,0x30,0xfc,0xbf,0x49,0xd8,0xa8\r\n" +
                               "0x112,0xbb,0x08,0x53,0x32,0x69,0x7d,0x4c,0x42,0x30,0xda,0xf8,0xc3,0x9d,0x87,0xfe,0x88\r\n" +
                               "0x113,0xef,0x1b,0x2e,0x5b,0xe3,0x61,0x3e,0x92,0x15,0x8a,0x31,0xfd,0xc0,0x4a,0xd9,0xa9\r\n" +
                               "0x114,0xbc,0x09,0x54,0x33,0x6a,0x7e,0x4d,0x43,0x31,0xdb,0xf9,0xc4,0x9e,0x88,0xff,0x89\r\n" +
                               "0x115,0xf0,0x1c,0x2f,0x5c,0xe4,0x62,0x3f,0x93,0x16,0x8b,0x32,0xfe,0xc1,0x4b,0xda,0xaa\r\n" +
                               "0x116,0xbd,0x0a,0x55,0x34,0x6b,0x7f,0x4e,0x44,0x32,0xdc,0xfa,0xc5,0x9f,0x89,0x01,0x8a\r\n" +
                               "0x117,0xf1,0x1d,0x30,0x5d,0xe5,0x63,0x40,0x94,0x17,0x8c,0x33,0xff,0xc2,0x4c,0xdb,0xab\r\n" +
                               "0x118,0xbe,0x0b,0x56,0x35,0x6c,0x80,0x4f,0x45,0x33,0xdd,0xfb,0xc6,0xa0,0x8a,0x02,0x8b\r\n" +
                               "0x119,0xf2,0x1e,0x31,0x5e,0xe6,0x64,0x41,0x95,0x18,0x8d,0x34,0x01,0xc3,0x4d,0xdc,0xac\r\n" +
                               "0x201,0x15,0x69,0x9b,0x91,0xea,0x75,0x3c,0xf7,0x62,0x37,0xf6,0x73,0x86,0xe9,0x65,0x16\r\n" +
                               "0x203,0x88,0x2a,0xa4,0x3b,0x4b,0x85,0x2d,0x16,0xc5,0x4c,0x87,0x91,0xb5,0x11,0x31,0xc1\r\n" +
                               "0x20D,0x9b,0x0c,0xb3,0x79,0xe2,0xca,0x83,0x86,0x87,0x20,0x68,0x37,0x32,0xc8,0xdf,0x43\r\n" +
                               "0x400,0xb0,0x9b,0xa3,0x78,0x20,0x46,0x9c,0x45,0xdd,0xa3,0x86,0x82,0x8a,0xf1,0x9f,0xc5\r\n" +
                               "0x401,0x5e,0x5f,0x01,0x90,0x95,0x72,0xe4,0x28,0x4c,0xd8,0x88,0xfb,0xb1,0x36,0x13,0x82\r\n" +
                               "0x402,0xf9,0xa4,0x7b,0x67,0x70,0xa0,0xc1,0x0f,0xd6,0x01,0xed,0xdd,0x3a,0xe6,0xd4,0x1d\r\n" +
                               "0x403,0xdf,0xe8,0x42,0x32,0x48,0x99,0x09,0x35,0x4d,0x73,0xbb,0x3d,0x8d,0x6b,0x96,0x06\r\n" +
                               "0x404,0x40,0xb5,0xf4,0x96,0xa0,0x3b,0xd4,0xa8,0x81,0xc9,0xf1,0xee,0x19,0x08,0x0c,0xe4\r\n" +
                               "0x405,0xb3,0xca,0x8a,0x4e,0x64,0xf9,0xd8,0x97,0x3c,0x17,0xac,0x1a,0x6c,0x18,0x33,0x8f\r\n" +
                               "0x406,0xda,0xe3,0x1a,0x49,0xe4,0x22,0xb3,0xe2,0xaf,0x90,0xec,0xf1,0x17,0x3b,0xed,0xaf\r\n" +
                               "0x407,0xf9,0x47,0xf3,0x09,0xd0,0xa0,0x7a,0x84,0xee,0xf2,0xd3,0x8d,0xe6,0x85,0xea,0x66\r\n" +
                               "0x40A,0x8b,0x23,0x1a,0x35,0x69,0x98,0x3f,0x92,0x67,0x8d,0x6e,0xdf,0xca,0xe6,0x70,0x72\r\n" +
                               "0x40B,0xd0,0x3c,0x00,0x95,0x78,0x1d,0x8b,0xd6,0xad,0xea,0xa8,0xcc,0xaf,0xaa,0x83,0xe0\r\n" +
                               "0x410,0xcf,0x0e,0xb3,0x81,0x95,0xdd,0x1e,0x45,0x76,0x1c,0xf1,0x08,0xc2,0xb3,0x1a,0x19\r\n" +
                               "0x411,0x62,0x96,0x04,0x17,0x46,0x9b,0x61,0xe2,0xb4,0xa5,0x5d,0x59,0x41,0xe2,0x1f,0x9c\r\n" +
                               "0x412,0x0d,0x9f,0x00,0x4c,0x8d,0x00,0xd2,0x34,0x8a,0xf5,0xed,0x97,0x71,0x69,0xd4,0xa7\r\n" +
                               "0x413,0xfb,0xb9,0xc7,0xb9,0xaf,0xa3,0x88,0xb8,0x5b,0x26,0x3c,0x1b,0x30,0x61,0xbc,0x34\r\n" +
                               "0x414,0x3e,0xed,0xc6,0x3d,0x42,0xe2,0x37,0x76,0xf3,0x14,0xf5,0xdb,0x4a,0x14,0xe7,0x26\r\n" +
                               "0x415,0xbe,0x95,0xc4,0x24,0x33,0xb0,0x24,0x92,0xc9,0xcf,0x50,0x5d,0xea,0x4f,0x4c,0xac";

            var line = str.Split(new[] { "\r\n" }, StringSplitOptions.None);

            foreach (var t in line)
            {
                var sp = t.Split(',');

                var canId = Convert.ToUInt32(sp[0], 16);
                _dataIdList.Add(canId, new Dictionary<byte, byte>());
                for (var i = 0; i <= 15; i++)
                    _dataIdList[canId].Add((byte)i, Convert.ToByte(sp[i + 1], 16));
            }

            _matrixValDefinitions.Add(CheckSum0X201, new MatrixValDefinition(0, 8, 0));
            _matrixValDefinitions.Add(Msgcnt0X201, new MatrixValDefinition(8, 4, 0));
            _matrixValDefinitions.Add(CheckSum0X203, new MatrixValDefinition(0, 8, 0));
            _matrixValDefinitions.Add(Msgcnt0X203, new MatrixValDefinition(8, 4, 0));
            _matrixValDefinitions.Add(CheckSum0X20D, new MatrixValDefinition(0, 8, 0));
            _matrixValDefinitions.Add(Msgcnt0X20D, new MatrixValDefinition(8, 4, 0));

            _matrixValDefinitions.Add(LftBndCmd, new MatrixValDefinition(0, 8, 0));
            _matrixValDefinitions.Add(RghtBndCmd, new MatrixValDefinition(8, 8, 0));

            if (_mainWorkThread != null)
            {
                _mainWorkThread.Abort();
                _mainWorkThread.Join();
            }
            _mainWorkThread = new Thread(MainWork);
            _mainWorkThread.Start();
        }

        ~E2LbGpFrontLamp()
        {
            Dispose();
        }

        private readonly Thread _mainWorkThread;
        private bool _isAwake;

        private void MainWork()
        {
            var sendCount = 0;
            const int period = 5;
            var extendedPeriodCount = 0;

            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(period);

                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(period);

                if (_isInExtendedSession)
                {
                    if (extendedPeriodCount % 50 == 0) // Period = 1000ms
                        if (Can != null)
                            Can.SendStandardCanData(
                                _currentReqCanId, new byte[] { 0x02, 0x3E, 0x00, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA });

                    extendedPeriodCount++;
                }
                else
                {
                    extendedPeriodCount = 0;
                }

                if (Can == null || !_isAwake)
                {
                    foreach (var key in _matrixValDefinitions.Keys)
                        _matrixValDefinitions[key].Value = 0x00;

                    sendCount = 0;
                    continue;
                }

                var sendList = new List<CanBus.CanDataPackage>();

                if (sendCount % (400 / period) == 0)
                {
                    sendList.Add(
                        new CanBus.CanDataPackage(
                            0x172,
                            CanBus.CanProtocol.Can,
                            CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            new byte[8]));
                }

                if (sendCount % (20 / period) == 0)
                {
                    GetCheckSum(Msgcnt0X201, CheckSum0X201, _intel0X201Matrix);
                    GetCheckSum(Msgcnt0X203, CheckSum0X203, _intel0X203Matrix);

                    sendList.Add(new CanBus.CanDataPackage(_intel0X201Matrix.CanId, CanBus.CanProtocol.Can,
                        CanBus.CanType.Standard, CanBus.CanFormat.Data, _intel0X201Matrix.MatrixData));
                    sendList.Add(new CanBus.CanDataPackage(_intel0X203Matrix.CanId, CanBus.CanProtocol.Can,
                        CanBus.CanType.Standard, CanBus.CanFormat.Data, _intel0X203Matrix.MatrixData));

                    _motorola0X204Matrix.UpdateData(_matrixValDefinitions[LftBndCmd]);
                    _motorola0X204Matrix.UpdateData(_matrixValDefinitions[RghtBndCmd]);
                    sendList.Add(new CanBus.CanDataPackage(_motorola0X204Matrix.CanId, CanBus.CanProtocol.Can,
                        CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorola0X204Matrix.MatrixData));
                }

                if (sendCount % (100 / period) == 0)
                {
                    GetCheckSum(Msgcnt0X20D, CheckSum0X20D, _intel0X20DMatrix);
                    sendList.Add(new CanBus.CanDataPackage(_intel0X20DMatrix.CanId, CanBus.CanProtocol.Can,
                        CanBus.CanType.Standard, CanBus.CanFormat.Data, _intel0X20DMatrix.MatrixData));
                }

                if (sendList.Any())
                    Can.SendCanDatas(sendList.ToArray());

                sendCount++;
                if (sendCount > 9999)
                    sendCount = 0;
            }
        }

        #region 诊断相关公共函数

        /// <summary>
        /// 读取模式配置
        /// </summary>
        [Description("R,读取模式配置")]
        public string ModeStr;

        /// <summary>
        /// 读LDMConfig
        /// </summary>
        [Description("R,LDMConfig")]
        public string LdmConfig;

        /// <summary>
        /// 读取节点配置
        /// </summary>
        [Description("R,读取节点配置")]
        public string LrdfStr;

        ///// <summary>
        ///// 读取远光备份线状态
        ///// </summary>
        //[Description("R,读取远光备份线状态")]
        //public string HbHwCmdSts;

        ///// <summary>
        ///// 读取近光备份线状态
        ///// </summary>
        //[Description("R,读取近光备份线状态")]
        //public string LbHwCmdSts;

        /// <summary>
        /// 总成零件号
        /// </summary>
        [Description("R,总成零件号")]
        public string HwPn;

        /// <summary>
        /// 引导程序零件号
        /// </summary>
        [Description("R,引导程序零件号")]
        public string BootAppSwPn;

        /// <summary>
        /// 引导程序版本号
        /// </summary>
        [Description("R,引导程序版本号")]
        public string BootAppSwVer;

        /// <summary>
        /// 应用程序零件号
        /// </summary>
        [Description("R,应用程序零件号")]
        public string AppSwPn;

        /// <summary>
        /// 配置文件零件号
        /// </summary>
        [Description("R,配置文件零件号")]
        public string CfgPn;

        /// <summary>
        /// 应用程序版本号
        /// </summary>
        [Description("R,应用程序版本号")]
        public string AppSwVer;

        /// <summary>
        /// 配置文件版本号
        /// </summary>
        [Description("R,配置文件版本号")]
        public string CfgVer;

        /// <summary>
        /// 平台零件信息
        /// </summary>
        [Description("R,平台零件信息")]
        public string PlatformPartsInformation;

        /// <summary>
        /// 生产追溯信息
        /// </summary>
        [Description("R,生产追溯信息")]
        public string ProductiontInfo;

        private uint _currentReqCanId = 0x715;
        private uint _currentRecvCanId = 0x795;
        private bool _isInExtendedSession;

        [Description("配置为普通模式")]
        public void WriteNormalMode()
        {
            if (Can == null)
                return;

            if (Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xC0, 0x00,
                new byte[] { 0x55 }))
                return;
            Thread.Sleep(500);
            Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xC0, 0x00,
                new byte[] { 0x55 });
        }

        [Description("配置为测试模式")]
        public void WriteTestMode()
        {
            if (Can == null)
                return;

            if (Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xC0, 0x00,
                new byte[] { 0x00 }))
                return;
            Thread.Sleep(500);
            Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xC0, 0x00,
                new byte[] { 0x00 });
        }

        [Description("进入拓展")]
        public void EnterExtendedSession()
        {
            if (Can == null)
                return;

            if (
                !Can.CanBusWithUds.TryEnterExtendedSession(_currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard))
            {
                Thread.Sleep(500);
                if (Can.CanBusWithUds.TryEnterExtendedSession(_currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard))
                    _isInExtendedSession = true;
            }
            else
                _isInExtendedSession = true;
        }

        [Description("退出拓展")]
        public void ExitExtendedSession()
        {
            if (Can == null)
                return;

            _isInExtendedSession = false;

            if (Can.CanBusWithUds.TryEnterDefaultSession(_currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can))
                return;

            Thread.Sleep(500);
            Can.CanBusWithUds.TryEnterDefaultSession(_currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can);
        }

        [Description("读取模式配置")]
        public string ReadCurrMode()
        {
            ModeStr = string.Empty;

            if (Can == null)
                return string.Empty;

            try
            {
                var readResult = ReadDid(_currentReqCanId, _currentRecvCanId, 0xC0, 0x00);
                if (readResult.Any())
                {
                    if (readResult[0] == 0x00)
                        ModeStr = "测试模式 " + ValueHelper.GetHextStrWithOx(readResult[0]);
                    else
                        ModeStr = "正常模式 " + ValueHelper.GetHextStrWithOx(readResult[0]);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return ModeStr;
        }

        [Description("读取LDMConfig")]
        public void ReadLdmConfig()
        {
            LdmConfig = string.Empty;

            if (Can == null)
                return;

            try
            {
                var readResult = ReadDid(_currentReqCanId, _currentRecvCanId, 0xC0, 0x03);
                if (readResult.Any())
                {
                    LdmConfig = ValueHelper.GetHextStr(readResult);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        
        [Description("读取节点配置")]
        public void ReadLrdf()
        {
            LrdfStr = string.Empty;

            if (Can == null)
                return;

            Can.AddDoNotFilterCanId(0x400);
            Can.AddDoNotFilterCanId(0x401);

            Can.CanRecvDataPackages.Clear();
            Thread.Sleep(500);

            if (Can.CanRecvDataPackages.FindAll(f => f.CanId == 0x400).Any())
            {
                LrdfStr += "左节点";
                _currentReqCanId = 0x715;
                _currentRecvCanId = 0x795;
            }
            else if (Can.CanRecvDataPackages.FindAll(f => f.CanId == 0x401).Any())
            {
                LrdfStr += "右节点";
                _currentReqCanId = 0x716;
                _currentRecvCanId = 0x796;
            }

            Can.RemoveDoNotFilterCanId(0x400);
            Can.RemoveDoNotFilterCanId(0x401);
        }

        [Description("配置为左节点")]
        public void SetLeftNode()
        {
            _currentReqCanId = 0x715;
            _currentRecvCanId = 0x795;
        }

        [Description("配置为右节点")]
        public void SetRightNode()
        {
            _currentReqCanId = 0x716;
            _currentRecvCanId = 0x796;
        }

        /// <summary>
        /// 读总成零件号
        /// </summary>
        [Description("读总成零件号")]
        public void ReadHwPn()
        {
            HwPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xC0, 0x37);
            if (result != null && result.Length == 6)
            {
                HwPn =
                    "P" +
                    ValueHelper.GetHextStr(new[] { result[0], result[1], result[2], result[3] }) +
                    "." +
                    new[] { result[4], result[5] }.GetStringByAsciiBytes(false); // （P）00133228
            }
        }

        /// <summary>
        /// 读引导程序零件号
        /// </summary>
        [Description("读引导程序零件号")]
        public void ReadBootAppSwPn()
        {
            BootAppSwPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xC0, 0x30);
            BootAppSwPn = "SW" + ValueHelper.GetHextStr(result);
        }

        /// <summary>
        /// 读引导程序版本号
        /// </summary>
        [Description("读引导程序版本号")]
        public void ReadBootAppSwVer()
        {
            BootAppSwVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xC0, 0x33);
            BootAppSwVer = result.GetStringByAsciiBytes(false);
        }

        /// <summary>
        /// 读应用程序零件号
        /// </summary>
        [Description("读应用程序零件号")]
        public void ReadAppSwPn()
        {
            AppSwPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xC0, 0x31);
            AppSwPn = "SW" + ValueHelper.GetHextStr(result);
        }

        /// <summary>
        /// 读配置文件零件号
        /// </summary>
        [Description("读配置文件零件号")]
        public void ReadCfgPn()
        {
            CfgPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xC0, 0x32);
            CfgPn = "SW" + ValueHelper.GetHextStr(result);
        }

        /// <summary>
        /// 读应用程序版本号
        /// </summary>
        [Description("读应用程序版本号")]
        public void ReadAppSwVer()
        {
            AppSwVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xC0, 0x34);
            AppSwVer = result.GetStringByAsciiBytes(false);
        }

        /// <summary>
        /// 配置文件版本号
        /// </summary>
        [Description("配置文件版本号")]
        public void ReadCfgVer()
        {
            CfgVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xC0, 0x35);
            CfgVer = result.GetStringByAsciiBytes(false);
        }

        [Description("读取平台零件信息")]
        public void ReadPlatformPartsInformation()
        {
            PlatformPartsInformation = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xC0, 0x36);
            if (result != null && result.Length == 6)
            {
                HwPn =
                    "S" +
                    ValueHelper.GetHextStr(new[] { result[0], result[1], result[2], result[3] }) +
                    "." +
                    new[] { result[4], result[5] }.GetStringByAsciiBytes(false); // （P）00133228
            }
        }

        [Description("读取生产追溯信息")]
        public void ReadProductiontInfo()
        {
            ProductiontInfo = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xF0, 0xB4);
            if (result != null)
                ProductiontInfo = "OK " + result.GetStringByAsciiBytes(false);
        }

        [Description("安全访问")]
        public void SecurityAccess()
        {
            byte[] seedBytes;
            if (!Can.CanBusWithUds.TryRequestSeed(
                _currentReqCanId,
                _currentRecvCanId,
                CanBus.CanType.Standard, 0x09, out seedBytes))
            {
                return;
            }

            var keyBytes = new byte[]
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            if (!Can.CanBusWithUds.TrySendKey(
                _currentReqCanId,
                _currentRecvCanId,
                CanBus.CanType.Standard, 0x0A, keyBytes))
            {
                return;
            }
        }

        private byte[] ReadDid(uint reqCanId, uint recvCanId, byte didHi, byte didLo)
        {
            if (Can == null)
                return new byte[0];

            byte[] echo;
            if (Can.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                CanBus.CanProtocol.Can, didHi, didLo, out echo))
                return echo;
            Thread.Sleep(500);
            return Can.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                CanBus.CanProtocol.Can, didHi, didLo, out echo) ? echo : new byte[0];
        }

        #endregion

        #region LED相关公共函数

        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
            new Dictionary<string, MatrixValDefinition>();
        private readonly CanCommunicationMatrix.IntelMatrix _intel0X201Matrix =
            new CanCommunicationMatrix.IntelMatrix(0x201, 5);
        private readonly CanCommunicationMatrix.IntelMatrix _intel0X203Matrix =
            new CanCommunicationMatrix.IntelMatrix(0x203, 5);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorola0X204Matrix =
            new CanCommunicationMatrix.MotorolaMatrix(0x204, 2);
        private readonly CanCommunicationMatrix.IntelMatrix _intel0X20DMatrix =
            new CanCommunicationMatrix.IntelMatrix(0x20D, 6);
        private readonly Dictionary<uint, Dictionary<byte, byte>> _dataIdList = new Dictionary<uint, Dictionary<byte, byte>>();

        private const string CheckSum0X201 = "CheckSum0X201";
        private const string Msgcnt0X201 = "Msgcnt0X201";
        private const string CheckSum0X203 = "CheckSum0X203";
        private const string Msgcnt0X203 = "Msgcnt0X203";
        private const string CheckSum0X20D = "CheckSum0X20D";
        private const string Msgcnt0X20D = "Msgcnt0X20D";
        private const string LftBndCmd = "LftBndCmd";
        private const string RghtBndCmd = "RghtBndCmd";

        public void GetCheckSum(
            string msgCntMatrixName,
            string msgCheckSumMatrixName,
            CanCommunicationMatrix.IntelMatrix intelMatrix)
        {
            if (!_matrixValDefinitions.ContainsKey(msgCntMatrixName) ||
                !_matrixValDefinitions.ContainsKey(msgCheckSumMatrixName))
                return;

            var value = _matrixValDefinitions[msgCntMatrixName].GetIntelSignalValue(intelMatrix.MatrixData);
            if (value == 0xFF)
                _matrixValDefinitions[msgCntMatrixName].Value = 0x00;
            else
                _matrixValDefinitions[msgCntMatrixName].Value = (byte)(value + 0x01);

            intelMatrix.UpdateData(_matrixValDefinitions[msgCntMatrixName]);
            var crc = GetMsgCheckSum(
                intelMatrix.MatrixData, intelMatrix.CanId, _matrixValDefinitions[msgCntMatrixName].Value);
            _matrixValDefinitions[msgCheckSumMatrixName].Value = crc;
            intelMatrix.UpdateData(_matrixValDefinitions[msgCheckSumMatrixName]);
        }

        private byte GetMsgCheckSum(IReadOnlyList<byte> data, uint msgId, byte msgCnt)
        {
            const byte e2ECrcInitValue = (byte)0xFF;
            //const byte e2ECrcPolyNomial = (byte)0x2F;
            const byte e2ECrcXorValue = (byte)0xFF;

            try
            {
                var len = data.Count - 1;
                var checkBuf = new byte[8];
                var crc = e2ECrcInitValue;

                for (var i = 1; i < data.Count; i++)
                    checkBuf[i - 1] = data[i];
                checkBuf[len] = GetDataId(msgId, msgCnt);

                for (var i = 0; i < len + 1; i++)
                {
                    var checkSum = (byte)(checkBuf[i] ^ crc);
                    crc = _crcTable[checkSum];

                    //for (var j = 0; j < 8; j++)
                    //{
                    //    if (checkSum >= 0x80)
                    //        checkSum = (byte)((checkSum << 1) ^ e2ECrcPolyNomial);
                    //    else
                    //        checkSum = (byte)(checkSum << i);
                    //}
                    //crc = checkSum;
                }

                return (byte)(crc ^ e2ECrcXorValue);
            }
            catch (Exception)
            {
                return 0x00;
            }
        }

        private byte GetDataId(uint msgId, byte msgCnt)
        {
            try
            {
                var dataId = _dataIdList[msgId][msgCnt];
                return dataId;
            }
            catch (Exception)
            {
                return 0xFF;
            }
        }

        [Description("唤醒")]
        public void LampAwake()
        {
            _isAwake = true;
        }

        [Description("休眠")]
        public void LampSleep()
        {
            _isAwake = false;
        }

        [Description("打开左角灯")]
        public void LftBndOn()
        {
            SwitchBnd(true, true);
        }

        [Description("关闭左角灯")]
        public void LefBndOff()
        {
            SwitchBnd(true, false);
        }

        [Description("打开右角灯")]
        public void RghtBndCmdOn()
        {
            SwitchBnd(false, true);
        }

        [Description("关闭右角灯")]
        public void RghtBndCmdOff()
        {
            SwitchBnd(false, false);
        }

        private void SwitchBnd(bool isLeft, bool isOn)
        {
            if (isLeft)
                _matrixValDefinitions[LftBndCmd].Value = isOn ? (byte)0xC8 : (byte)0x00;
            else
                _matrixValDefinitions[RghtBndCmd].Value = isOn ? (byte)0xC8 : (byte)0x00;
        }

        #endregion

        #region 单颗点亮

        public string Lbpes1Control;
        public string Lbpes2Control;
        public string Lbpes3Control;
        public string ALbControl;

        [Description("测试单颗LB")]
        public void DebugIoControl()
        {
            if (Can != null)
            {
                //byte b;
                //if (byte.TryParse(value, out b))
                //{
                //    Can.CanBusWithUds.TryInputOutputControl(
                //        _currentReqCanId,
                //        _currentRecvCanId,
                //        CanBus.CanType.Standard,
                //        0xFD, 0x03,
                //        Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { b, 0xff, 0xff });
                //}

                //Can.CanBusWithUds.TryInputOutputControl(
                //    _currentReqCanId,
                //    _currentRecvCanId,
                //    CanBus.CanType.Standard,
                //    0xFD, 0x03,
                //    Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x00, 0x00, 0x00, 0x00 });

                Can.CanBusWithUds.TryInputOutputControl(
                   _currentReqCanId,
                   _currentRecvCanId,
                   CanBus.CanType.Standard,
                   0xFD, 0x03,
                   Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x00, 0x00, 0x00, 0x01 });

                //Can.SendCanDatas(new[]
                //{
                //    new CanBus.CanDataPackage(_currentReqCanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                //        CanBus.CanFormat.Data, new byte[] {0x06, 0x2F, 0xFD, 0x04, 0x03,  0x3F, 0x07, 0xAA})
                //});
            }
        }

        #endregion

        private readonly byte[] _crcTable =
        {
            0x00, 0x2F, 0x5E, 0x71, 0xBC, 0x93, 0xE2, 0xCD, 0x57, 0x78, 0x09, 0x26, 0xEB, 0xC4, 0xB5, 0x9A,
            0xAE, 0x81, 0xF0, 0xDF, 0x12, 0x3D, 0x4C, 0x63, 0xF9, 0xD6, 0xA7, 0x88, 0x45, 0x6A, 0x1B, 0x34,
            0x73, 0x5C, 0x2D, 0x02, 0xCF, 0xE0, 0x91, 0xBE, 0x24, 0x0B, 0x7A, 0x55, 0x98, 0xB7, 0xC6, 0xE9,
            0xDD, 0xF2, 0x83, 0xAC, 0x61, 0x4E, 0x3F, 0x10, 0x8A, 0xA5, 0xD4, 0xFB, 0x36, 0x19, 0x68, 0x47,
            0xE6, 0xC9, 0xB8, 0x97, 0x5A, 0x75, 0x04, 0x2B, 0xB1, 0x9E, 0xEF, 0xC0, 0x0D, 0x22, 0x53, 0x7C,
            0x48, 0x67, 0x16, 0x39, 0xF4, 0xDB, 0xAA, 0x85, 0x1F, 0x30, 0x41, 0x6E, 0xA3, 0x8C, 0xFD, 0xD2,
            0x95, 0xBA, 0xCB, 0xE4, 0x29, 0x06, 0x77, 0x58, 0xC2, 0xED, 0x9C, 0xB3, 0x7E, 0x51, 0x20, 0x0F,
            0x3B, 0x14, 0x65, 0x4A, 0x87, 0xA8, 0xD9, 0xF6, 0x6C, 0x43, 0x32, 0x1D, 0xD0, 0xFF, 0x8E, 0xA1,
            0xE3, 0xCC, 0xBD, 0x92, 0x5F, 0x70, 0x01, 0x2E, 0xB4, 0x9B, 0xEA, 0xC5, 0x08, 0x27, 0x56, 0x79,
            0x4D, 0x62, 0x13, 0x3C, 0xF1, 0xDE, 0xAF, 0x80, 0x1A, 0x35, 0x44, 0x6B, 0xA6, 0x89, 0xF8, 0xD7,
            0x90, 0xBF, 0xCE, 0xE1, 0x2C, 0x03, 0x72, 0x5D, 0xC7, 0xE8, 0x99, 0xB6, 0x7B, 0x54, 0x25, 0x0A,
            0x3E, 0x11, 0x60, 0x4F, 0x82, 0xAD, 0xDC, 0xF3, 0x69, 0x46, 0x37, 0x18, 0xD5, 0xFA, 0x8B, 0xA4,
            0x05, 0x2A, 0x5B, 0x74, 0xB9, 0x96, 0xE7, 0xC8, 0x52, 0x7D, 0x0C, 0x23, 0xEE, 0xC1, 0xB0, 0x9F,
            0xAB, 0x84, 0xF5, 0xDA, 0x17, 0x38, 0x49, 0x66, 0xFC, 0xD3, 0xA2, 0x8D, 0x40, 0x6F, 0x1E, 0x31,
            0x76, 0x59, 0x28, 0x07, 0xCA, 0xE5, 0x94, 0xBB, 0x21, 0x0E, 0x7F, 0x50, 0x9D, 0xB2, 0xC3, 0xEC,
            0xD8, 0xF7, 0x86, 0xA9, 0x64, 0x4B, 0x3A, 0x15, 0x8F, 0xA0, 0xD1, 0xFE, 0x33, 0x1C, 0x6D, 0x42
        };
    }
}
