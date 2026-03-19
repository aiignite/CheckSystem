using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,P00141531_EP33辅助远近光大灯")]
    public class Ep33AdmFrontLamp : ControllerBase
    {
        public CanBus Can;

        public Ep33AdmFrontLamp(string name)
            : base(name)
        {
            _matrixValDefinitions.Add(LftAdmLbCmd, new MatrixValDefinition(0, 8, 0));
            _matrixValDefinitions.Add(LftAdmhb1Cmd, new MatrixValDefinition(8, 8, 0));
            _matrixValDefinitions.Add(LftAdmhb2Cmd, new MatrixValDefinition(16, 8, 0));
            _matrixValDefinitions.Add(LftAdmhb3Cmd, new MatrixValDefinition(24, 8, 0));
            _matrixValDefinitions.Add(LftAdmhb4Cmd, new MatrixValDefinition(32, 8, 0));
            _matrixValDefinitions.Add(LftAdmhb5Cmd, new MatrixValDefinition(40, 8, 0));

            _matrixValDefinitions.Add(RghtAdmLbCmd, new MatrixValDefinition(0, 8, 0));
            _matrixValDefinitions.Add(RghtAdmhb1Cmd, new MatrixValDefinition(8, 8, 0));
            _matrixValDefinitions.Add(RghtAdmhb2Cmd, new MatrixValDefinition(16, 8, 0));
            _matrixValDefinitions.Add(RghtAdmhb3Cmd, new MatrixValDefinition(24, 8, 0));
            _matrixValDefinitions.Add(RghtAdmhb4Cmd, new MatrixValDefinition(32, 8, 0));
            _matrixValDefinitions.Add(RghtAdmhb5Cmd, new MatrixValDefinition(40, 8, 0));

            _matrixValDefinitions.Add(DhlDirConf, new MatrixValDefinition(1, 2, 0));
            _matrixValDefinitions.Add(LdhlRefCmd, new MatrixValDefinition(0, 1, 0));
            _matrixValDefinitions.Add(LdhlConfPos, new MatrixValDefinition(6, 10, 0));
            _matrixValDefinitions.Add(RdhlRefCmd, new MatrixValDefinition(48, 1, 0));
            _matrixValDefinitions.Add(RdhlConfPos, new MatrixValDefinition(54, 10, 0));

            if (_mainWorkThread != null)
            {
                _mainWorkThread.Abort();
                _mainWorkThread.Join();
            }
            _mainWorkThread = new Thread(MainWork);
            _mainWorkThread.Start();
        }

        ~Ep33AdmFrontLamp()
        {
            Dispose();
        }

        private readonly Thread _mainWorkThread;
        private bool _isAwake;
        private readonly CanCommunicationMatrix.IntelMatrix _intelMatrix0X50 =
            new CanCommunicationMatrix.IntelMatrix(0x50, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _intelMatrix0X60 =
            new CanCommunicationMatrix.IntelMatrix(0x60, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _intelMatrix0X300 =
            new CanCommunicationMatrix.IntelMatrix(0x300, 8);
        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
            new Dictionary<string, MatrixValDefinition>();

        private void MainWork()
        {
            var sendCount = 0;
            var period = 5;
            var extendedPeriodCount = 0;

            while (_mainWorkThread.IsAlive)
            {
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

                if (sendCount % 5 == 0)
                {
                    _intelMatrix0X50.UpdateData(_matrixValDefinitions[LftAdmLbCmd]);
                    _intelMatrix0X50.UpdateData(_matrixValDefinitions[LftAdmhb1Cmd]);
                    _intelMatrix0X50.UpdateData(_matrixValDefinitions[LftAdmhb2Cmd]);
                    _intelMatrix0X50.UpdateData(_matrixValDefinitions[LftAdmhb3Cmd]);
                    _intelMatrix0X50.UpdateData(_matrixValDefinitions[LftAdmhb4Cmd]);
                    _intelMatrix0X50.UpdateData(_matrixValDefinitions[LftAdmhb5Cmd]);

                    _intelMatrix0X60.UpdateData(_matrixValDefinitions[RghtAdmLbCmd]);
                    _intelMatrix0X60.UpdateData(_matrixValDefinitions[RghtAdmhb1Cmd]);
                    _intelMatrix0X60.UpdateData(_matrixValDefinitions[RghtAdmhb2Cmd]);
                    _intelMatrix0X60.UpdateData(_matrixValDefinitions[RghtAdmhb3Cmd]);
                    _intelMatrix0X60.UpdateData(_matrixValDefinitions[RghtAdmhb4Cmd]);
                    _intelMatrix0X60.UpdateData(_matrixValDefinitions[RghtAdmhb5Cmd]);

                    //Can.SendCanDatas(new[]
                    //{
                    //    new CanBus.CanDataPackage(_intelMatrix0x50.CanId, CanBus.CanProtocol.Can,
                    //        CanBus.CanType.Standard, CanBus.CanFormat.Data, _intelMatrix0x50.MatrixData),
                    //    //new CanBus.CanDataPackage(_intelMatrix0x60.CanId, CanBus.CanProtocol.Can,
                    //    //    CanBus.CanType.Standard, CanBus.CanFormat.Data, _intelMatrix0x60.MatrixData)
                    //});

                    sendList.AddRange(
                        new[]
                        {
                            new CanBus.CanDataPackage(_intelMatrix0X50.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _intelMatrix0X50.MatrixData),
                            new CanBus.CanDataPackage(_intelMatrix0X60.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _intelMatrix0X60.MatrixData)
                        });
                }

                if (sendCount % 20 == 3)
                {
                    _intelMatrix0X300.UpdateData(_matrixValDefinitions[DhlDirConf]);
                    _intelMatrix0X300.UpdateData(_matrixValDefinitions[LdhlRefCmd]);
                    _intelMatrix0X300.UpdateData(_matrixValDefinitions[LdhlConfPos]);
                    _intelMatrix0X300.UpdateData(_matrixValDefinitions[RdhlRefCmd]);
                    _intelMatrix0X300.UpdateData(_matrixValDefinitions[RdhlConfPos]);

                    sendList.AddRange(
                        new[]
                        {
                            new CanBus.CanDataPackage(_intelMatrix0X300.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _intelMatrix0X300.MatrixData)
                        });
                }

                Can.SendCanDatas(sendList.ToArray());

                sendCount++;
                if (sendCount > 9999)
                    sendCount = 0;
            }
        }

        #region LED相关公共函数

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

        [Description("打开辅近光")]
        public void LbOn()
        {
            _matrixValDefinitions[LftAdmLbCmd].Value = 0x64;
            _matrixValDefinitions[RghtAdmLbCmd].Value = 0x64;
        }

        [Description("关闭辅近光")]
        public void LbOff()
        {
            _matrixValDefinitions[LftAdmLbCmd].Value = 0x00;
            _matrixValDefinitions[RghtAdmLbCmd].Value = 0x00;
        }

        [Description("打开单颗辅远光")]
        public void HbOn(string index)
        {
            switch (index)
            {
                case "1":
                    _matrixValDefinitions[LftAdmhb1Cmd].Value = 0x64;
                    _matrixValDefinitions[RghtAdmhb1Cmd].Value = 0x64;
                    break;

                case "2":
                    _matrixValDefinitions[LftAdmhb2Cmd].Value = 0x64;
                    _matrixValDefinitions[RghtAdmhb2Cmd].Value = 0x64;
                    break;

                case "3":
                    _matrixValDefinitions[LftAdmhb3Cmd].Value = 0x64;
                    _matrixValDefinitions[RghtAdmhb3Cmd].Value = 0x64;
                    break;

                case "4":
                    _matrixValDefinitions[LftAdmhb4Cmd].Value = 0x64;
                    _matrixValDefinitions[RghtAdmhb4Cmd].Value = 0x64;
                    break;

                case "5":
                    _matrixValDefinitions[LftAdmhb5Cmd].Value = 0x64;
                    _matrixValDefinitions[RghtAdmhb5Cmd].Value = 0x64;
                    break;
            }
        }

        [Description("关闭单颗辅远光")]
        public void HbOff(string index)
        {
            switch (index)
            {
                case "1":
                    _matrixValDefinitions[LftAdmhb1Cmd].Value = 0x00;
                    _matrixValDefinitions[RghtAdmhb1Cmd].Value = 0x00;
                    break;

                case "2":
                    _matrixValDefinitions[LftAdmhb2Cmd].Value = 0x00;
                    _matrixValDefinitions[RghtAdmhb2Cmd].Value = 0x00;
                    break;

                case "3":
                    _matrixValDefinitions[LftAdmhb3Cmd].Value = 0x00;
                    _matrixValDefinitions[RghtAdmhb3Cmd].Value = 0x00;
                    break;

                case "4":
                    _matrixValDefinitions[LftAdmhb4Cmd].Value = 0x00;
                    _matrixValDefinitions[RghtAdmhb4Cmd].Value = 0x00;
                    break;

                case "5":
                    _matrixValDefinitions[LftAdmhb5Cmd].Value = 0x00;
                    _matrixValDefinitions[RghtAdmhb5Cmd].Value = 0x00;
                    break;
            }
        }

        #endregion

        #region 诊断相关公共函数

        /// <summary>
        /// 读取模式配置
        /// </summary>
        [Description("R,读取模式配置")]
        public string ModeStr;

        /// <summary>
        /// 读取节点配置
        /// </summary>
        [Description("R,读取节点配置")]
        public string LrdfStr;

        /// <summary>
        /// 读取远光备份线状态
        /// </summary>
        [Description("R,读取远光备份线状态")]
        public string HbHwCmdSts;

        /// <summary>
        /// 读取近光备份线状态
        /// </summary>
        [Description("R,读取近光备份线状态")]
        public string LbHwCmdSts;

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

        private uint _currentReqCanId = 0x1f6;
        private uint _currentRecvCanId = 0x1f7;
        private bool _isInExtendedSession;

        [Description("配置为普通模式")]
        public void WriteNormalMode()
        {
            if (Can == null)
                return;

            if (Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xD0, 0x2A,
                new byte[] { 0x01 }))
                return;
            Thread.Sleep(500);
            Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xD0, 0x2A,
                new byte[] { 0x01 });
        }

        [Description("配置为测试模式")]
        public void WriteTestMode()
        {
            if (Can == null)
                return;

            if (Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xD0, 0x2A,
                new byte[] { 0x00 }))
                return;
            Thread.Sleep(500);
            Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xD0, 0x2A,
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
                var readResult = ReadDid(_currentReqCanId, _currentRecvCanId, 0xD0, 0x2A);
                if (readResult.Any() && (readResult[0] == 0x0F || readResult[0] == 0x00))
                    ModeStr = "测试模式 " + ValueHelper.GetHextStrWithOx(readResult[0]);
                else
                    ModeStr = "正常模式 " + ValueHelper.GetHextStrWithOx(readResult[0]);
            }
            catch (Exception)
            {
                // ignored
            }

            return ModeStr;
        }

        [Description("读取节点配置")]
        public void ReadLrdf()
        {
            LrdfStr = string.Empty;

            if (Can == null)
                return;

            Can.AddDoNotFilterCanId(0x34);
            Can.AddDoNotFilterCanId(0x44);

            Can.CanRecvDataPackages.Clear();
            Thread.Sleep(500);

            if (Can.CanRecvDataPackages.FindAll(f => f.CanId == 0x34).Any())
            {
                LrdfStr += "左节点";
                _currentReqCanId = 0x1F6;
                _currentRecvCanId = 0x1F7;
            }
            else if (Can.CanRecvDataPackages.FindAll(f => f.CanId == 0x44).Any())
            {
                LrdfStr += "右节点";
                _currentReqCanId = 0x1F8;
                _currentRecvCanId = 0x1F9;
            }

            Can.RemoveDoNotFilterCanId(0x34);
            Can.RemoveDoNotFilterCanId(0x44);
        }

        /// <summary>
        /// 备份线状态检测
        /// </summary>
        [Description("备份线状态检测")]
        public void ReadHwCmdSts()
        {
            HbHwCmdSts = string.Empty;
            LbHwCmdSts = string.Empty;

            if (Can == null)
                return;

            Can.AddDoNotFilterCanId(0x116);
            Can.AddDoNotFilterCanId(0x126);

            Can.CanRecvDataPackages.Clear();
            Thread.Sleep(500);

            if (Can.CanRecvDataPackages.FindLast(f => f.CanId == 0x116) != null)
            {
                var b = Can.CanRecvDataPackages.FindLast(f => f.CanId == 0x116).CanData[0];

                LbHwCmdSts = Convert.ToString(b, 2).PadLeft(8, '0')[1].ToString() == "1" ? "ON" : "OFF";
                HbHwCmdSts = Convert.ToString(b, 2).PadLeft(8, '0')[3].ToString() == "1" ? "ON" : "OFF";
            }
            else if (Can.CanRecvDataPackages.FindLast(f => f.CanId == 0x126) != null)
            {
                var b = Can.CanRecvDataPackages.FindLast(f => f.CanId == 0x116).CanData[0];

                LbHwCmdSts = Convert.ToString(b, 2).PadLeft(8, '0')[1].ToString() == "1" ? "ON" : "OFF";
                HbHwCmdSts = Convert.ToString(b, 2).PadLeft(8, '0')[3].ToString() == "1" ? "ON" : "OFF";
            }

            Can.RemoveDoNotFilterCanId(0x116);
            Can.RemoveDoNotFilterCanId(0x126);
        }

        /// <summary>
        /// 读总成零件号
        /// </summary>
        [Description("读总成零件号")]
        public void ReadHwPn()
        {
            HwPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xD0, 0x0F);
            if (result != null)
                HwPn = result.GetStringByAsciiBytes(false); // P00141531
        }

        /// <summary>
        /// 读引导程序零件号
        /// </summary>
        [Description("读引导程序零件号")]
        public void ReadBootAppSwPn()
        {
            BootAppSwPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xC0);
            if (result.Length != 5) return;
            var str1 = Encoding.ASCII.GetString(new[] { result[0], result[1] });
            var str2 = ValueHelper.GetDecimal(new[] { result[2], result[3], result[4] }).ToString().PadLeft(7, '0');
            BootAppSwPn = str1 + str2;
        }

        /// <summary>
        /// 读引导程序版本号
        /// </summary>
        [Description("读引导程序版本号")]
        public void ReadBootAppSwVer()
        {
            BootAppSwVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0XD0);
            BootAppSwVer = result.GetStringByAsciiBytes(false); // B003
        }

        /// <summary>
        /// 读应用程序零件号
        /// </summary>
        [Description("读应用程序零件号")]
        public void ReadAppSwPn()
        {
            AppSwPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xC1);
            AppSwPn = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0');
        }

        /// <summary>
        /// 读配置文件零件号
        /// </summary>
        [Description("读配置文件零件号")]
        public void ReadCfgPn()
        {
            CfgPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xC2);
            CfgPn = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0');
        }

        /// <summary>
        /// 读应用程序版本号
        /// </summary>
        [Description("读应用程序版本号")]
        public void ReadAppSwVer()
        {
            AppSwVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xD1);
            AppSwVer = result.GetStringByAsciiBytes(false); // A013
        }

        /// <summary>
        /// 配置文件版本号
        /// </summary>
        [Description("配置文件版本号")]
        public void ReadCfgVer()
        {
            CfgVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xD2);
            CfgVer = result.GetStringByAsciiBytes(false); // C013
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

        private const string LftAdmLbCmd = "LftADMLBCmd";
        private const string LftAdmhb1Cmd = "LftADMHB1Cmd";
        private const string LftAdmhb2Cmd = "LftADMHB2Cmd";
        private const string LftAdmhb3Cmd = "LftADMHB3Cmd";
        private const string LftAdmhb4Cmd = "LftADMHB4Cmd";
        private const string LftAdmhb5Cmd = "LftADMHB5Cmd";

        private const string RghtAdmLbCmd = "RghtADMLBCmd";
        private const string RghtAdmhb1Cmd = "RghtADMHB1Cmd";
        private const string RghtAdmhb2Cmd = "RghtADMHB2Cmd";
        private const string RghtAdmhb3Cmd = "RghtADMHB3Cmd";
        private const string RghtAdmhb4Cmd = "RghtADMHB4Cmd";
        private const string RghtAdmhb5Cmd = "RghtADMHB5Cmd";

        private const string DhlDirConf = "DHLDirConf";
        private const string LdhlRefCmd = "LDHLRefCmd";
        private const string LdhlConfPos = "LDHLConfPos";
        private const string RdhlRefCmd = "RDHLRefCmd";
        private const string RdhlConfPos = "RDHLConfPos";
    }
}
