using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    [Description("CAN-Product,S12L热泵控制器")]
    public sealed class S12LHeatPumpController : ControllerBase
    {
        public CanBus Can1;
        public CanBus Can2;

        public LinBus Lin1;
        public LinBus Lin2;
        public LinBus Lin3;
        public LinBus Lin4;
        public LinBus Lin5;
        public LinBus Lin6;

        private const uint CanDiagnosisRequestPhyCanId = 0x750;
        //private const uint CanDiagnosisRequestFunCanId = 0x7DF;
        private const uint CanDiagnosisResponseCanId = 0x758;

        [Description("R/W,是否存储CAN日志")]
        public bool IsSaveCanLog;

        private bool _isStartCanLog;
        private readonly List<CanBus.CanDataPackage> _canLogs = new List<CanBus.CanDataPackage>();

        public S12LHeatPumpController(string name)
            : base(name)
        {
            CanBus.PushCanMsg += CanBus_PushCanMsg;
            LinBus.PushLinMsg += LinBus_PushLinMsg;
            //SetTimer(Can1NetWork(), 20);

            if (_networkThread != null)
            {
                _networkThread.Abort();
                _networkThread.Join();
            }
            _networkThread = new Thread(Can1NetWork);
            _networkThread.Start();
        }

        private void CanBus_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (Can1 != null && Can1.Name == name && IsSaveCanLog && (data.CanId == CanDiagnosisRequestPhyCanId || data.CanId == CanDiagnosisResponseCanId))
            {
                if (data.CanId == CanDiagnosisRequestPhyCanId &&
                    ValueHelper.GetHextStr(data.CanData).StartsWith("023E80"))
                    return;
                if (data.CanId == CanDiagnosisResponseCanId &&
                    (ValueHelper.GetHextStr(data.CanData).StartsWith("017E") || ValueHelper.GetHextStr(data.CanData).StartsWith("027E") || ValueHelper.GetHextStr(data.CanData).StartsWith("037E")))
                    return;

                if (_isStartCanLog)
                {
                    if (_canLogs.Count > 9999)
                        _canLogs.Clear();
                    _canLogs.Add(data);
                }
            }

            if (Can1 != null &&
                Can1.Name == name &&
                (data.CanId != CanDiagnosisRequestPhyCanId && data.CanId != CanDiagnosisResponseCanId) &&
                _can1Buff.FindAll(f => f.CanId == data.CanId).Count == 0 &&
                _isCheckCan1)
            {
                _can1Buff.Add(data);
            }

            if (Can2 != null &&
                Can2.Name == name &&
                _can2Buff.FindAll(f => f.CanId == data.CanId).Count == 0 &&
                _isCheckCan2)
            {
                _can2Buff.Add(data);
            }
        }

        [Description("开启接收CAN日志")]
        public void StartCanLog()
        {
            if (IsSaveCanLog)
            {
                _canLogs.Clear();
                _isStartCanLog = true;
            }
        }

        [Description("关闭并存储CAN日志")]
        public void StopCanLog()
        {
            if (IsSaveCanLog)
            {
                if (Can1 != null)
                {
                    try
                    {
                        var folder = Directory.GetCurrentDirectory() + @"\S12L_CAN_DataRecord";

                        if (!Directory.Exists(folder))
                            Directory.CreateDirectory(folder);

                        var file = string.Format(@"{0}\{1}_{2}_{3}.txt",
                            folder,
                            Can1.Name.Replace(":", "_").Replace("/", "_").Replace(" ", "_").Replace(".", "_"),
                            DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace(":", string.Empty).Replace("/", string.Empty).Replace(" ", string.Empty),
                            Guid.NewGuid());

                        //var code = value;

                        //var lines = File.ReadAllLines(filePath);
                        var list = _canLogs.Select(t => string.Format(@"{0};{1};{2} ", t.DateTime.ToString("yyyy-MM-dd HH:mm:ss ffff"), string.Format("0x{0}", string.Format("{0:X}", t.CanId).PadLeft(8, '0')), ValueHelper.GetHextStr(t.CanData))).ToList();

                        File.WriteAllLines(file, list);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    _isStartCanLog = false;
                    _canLogs.Clear();
                }
            }
        }

        ~S12LHeatPumpController()
        {
            Dispose();
        }

        private readonly object _can1Locker = new object();
        private int _periodCount;
        private bool _isSleeping = true;
        private bool _isInExtendedSession;
        private readonly Thread _networkThread;

        private void Can1NetWork()
        {
            while (_networkThread.IsAlive)
            {
                if (!_networkThread.IsAlive)
                    break;

                Thread.Sleep(20);

                _periodCount++;

                if (_periodCount > 50)
                    _periodCount = 0;

                try
                {
                    if (Can1 == null)
                        continue;

                    lock (_can1Locker)
                    {
                        if (!_isSleeping)
                        {
                            var lstPages = new List<CanBus.CanDataPackage>
                            {
                                new CanBus.CanDataPackage(
                                    0x4C3,
                                    CanBus.CanProtocol.Can,
                                    CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data,
                                    new byte[8])
                            };

                            Can1.SendCanDatas(lstPages.ToArray());
                        }

                        if (_isInExtendedSession && _periodCount == 50)
                            Can1.SendStandardCanData(0x7DF,
                                new byte[] { 0x02, 0x3E, 0x80, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA });
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        #region 主要控制

        [Description("R,发送休眠指令结果")]
        public string SendSleepCmdResult = string.Empty;

        [Description("发送休眠指令")]
        public void SendSleepCmd()
        {
            SendSleepCmdResult = string.Empty;

            if (Can1 != null)
            {
                ControllerSleep();
                Thread.Sleep(250);
                EnterDefaultSession();
                byte[] resultEcho;
                if (Can1.CanBusWithUds.TesterTryRequest(
                    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, new byte[] { 0x10, 0x03 }, out resultEcho, CanBus.CanType.Standard))
                {
                    Thread.Sleep(250);
                    SecurityAccess("01");
                    Thread.Sleep(500);

                    if (Can1.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xB0, 0x75, new byte[] { 0x02 }))
                    {
                        SendSleepCmdResult = "OK";
                    }
                }
            }
        }

        [Description("休眠")]
        public void ControllerSleep()
        {
            _isSleeping = true;
        }

        [Description("唤醒")]
        public void ControllerAwake()
        {
            _isSleeping = false;
        }

        [Description("进入正常模式")]
        public void EnterDefaultSession()
        {
            if (Can1 == null)
                return;

            lock (_can1Locker)
            {
                _isInExtendedSession = false;

                if (Can1.CanBusWithUds.TryEnterDefaultSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard))
                    return;

                Thread.Sleep(500);

                Can1.CanBusWithUds.TryEnterDefaultSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard);
            }
        }

        [Description("进入拓展模式")]
        public void EnterExtendedSession()
        {
            if (Can1 == null)
                return;

            lock (_can1Locker)
            {
                if (Can1.CanBusWithUds.TryEnterExtendedSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (Can1.CanBusWithUds.TryEnterExtendedSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                }
            }
        }

        [Description("进入编程模式")]
        public void EnterProgramSession()
        {
            if (Can1 == null)
                return;

            _isInExtendedSession = false;

            lock (_can1Locker)
            {
                if (Can1.CanBusWithUds.TryEnterProgrammingSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    return;
                }

                Thread.Sleep(500);
                Can1.CanBusWithUds.TryEnterProgrammingSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard);
            }
        }

        [Description("R,安全访问结果")]
        public string SecurityAccessResult = string.Empty;

        [Description("解锁SeedKey-拓展模式01或编程模式05")]
        public bool SecurityAccess(string subFunc)
        {
            SecurityAccessResult = string.Empty;

            if (Can1 == null)
                return false;

            byte securitySubFunc;
            if (!byte.TryParse(subFunc, out securitySubFunc))
                return false;

            if (securitySubFunc != 0x01 && securitySubFunc != 0x05)
                return false;

            var subFunctoion = securitySubFunc;

            lock (_can1Locker)
            {
                byte[] seedBytes;
                if (!Can1.CanBusWithUds.TryRequestSeed(
                    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, subFunctoion, out seedBytes))
                    return false;

                if (seedBytes == null || seedBytes.Length != 4)
                    return false;

                var cmd = new Process();
                var startInfo = new ProcessStartInfo
                {
                    FileName =
                        Directory.GetCurrentDirectory() +
                        @"\DllImport\SeedKeyDebug\SeedKeyDebug\bin\Debug\SeedKeyDebug.exe",
                    Arguments = string.Empty,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                cmd.StartInfo = startInfo;

                var keyStr = string.Empty;
                if (cmd.Start())
                {
                    var seed = BitConverter.ToInt32(seedBytes.Reverse().ToArray(), 0);
                    cmd.StandardInput.WriteLine("{0},{1}", seed, subFunctoion);

                    while (true)
                    {
                        var log = cmd.StandardOutput.ReadLine();
                        if (log == null)
                            break;
                        if (!log.StartsWith("Key:"))
                            continue;
                        keyStr = log;
                        break;
                    }
                    cmd.StandardInput.WriteLine("\n");
                }

                if (string.IsNullOrEmpty(keyStr))
                    return false;
                var key = keyStr.Replace("Key:", "");
                var keyBytes = BitConverter.GetBytes(int.Parse(key)).Reverse().ToArray();

                if (Can1.CanBusWithUds.TrySendKey(
                    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, (byte)(subFunctoion + 0x01), keyBytes))
                {
                    SecurityAccessResult = "OK";
                    return true;
                }
                else
                {
                    SecurityAccessResult = "NG";
                    return false;
                }

                //return Can1.CanBusWithUds.TrySendKey(
                //    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                //    CanBus.CanType.Standard, (byte)(subFunctoion + 0x01), keyBytes);
            }
        }

        [Description("切换分区")]
        public void ChangeZone()
        {
            ChangeZoneResult = string.Empty; // OK 结果71 01 DF FD

            if (Can1 == null)
                return;

            lock (_can1Locker)
            {
                byte[] resultEcho;
                Can1.CanBusWithUds.TesterTryRequest(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    new byte[] { 0x31, 0x01, 0xDF, 0xFD, 0x01 }, out resultEcho, CanBus.CanType.Standard);

                if (resultEcho != null && ValueHelper.GetHextStr(resultEcho).Replace(" ", "") == "7101DFFD")
                {
                    //foreach (var t in resultEcho)
                    //    ChangeZoneResult += ValueHelper.GetHextStr(t);

                    //EthernetCheckResult = "7103AF1400";

                    Thread.Sleep(500);

                    byte[] resultEcho2;
                    Can1.CanBusWithUds.TesterTryRequest(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        new byte[] { 0x31, 0x03, 0xDF, 0xFD }, out resultEcho2, CanBus.CanType.Standard);

                    if (resultEcho2 != null)
                    {
                        foreach (var t in resultEcho2)
                            ChangeZoneResult += ValueHelper.GetHextStr(t);

                        if (ChangeZoneResult == "7103DFFD0001000001")
                        {
                            var str = EcuReset();

                            if (str == "5101")
                            {
                                ChangeZoneResult = "OK";
                            }
                            else
                            {
                                ChangeZoneResult = "NG";
                            }
                        }
                    }
                }
            }
        }

        [Description("ECU复位")]
        public string EcuReset()
        {
            EcuResetResult = string.Empty;

            byte[] ecuResetEcho;
            if (!Can1.CanBusWithUds
                .TesterTryRequest(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho, CanBus.CanType.Standard))
            {
                Thread.Sleep(200);
                Can1.CanBusWithUds
               .TesterTryRequest(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho, CanBus.CanType.Standard);
            }

            EcuResetResult = ValueHelper.GetHextStr(ecuResetEcho).Replace(" ", "");
            return EcuResetResult; //ValueHelper.GetHextStr(ecuResetEcho).Replace(" ", "");
        }

        #endregion

        #region 普通模式/工厂模式

        [Description("R,ECU复位结果")]
        public string EcuResetResult = string.Empty;

        [Description("R,切换分区结果")]
        public string ChangeZoneResult = string.Empty;

        [Description("R,模式读取")]
        public string Mode = string.Empty;

        [Description("读取当前模式")]
        public void ReadMode()
        {
            Mode = string.Empty;
            Mode = ValueHelper.GetHextStr(ReadDidViaCan1(0xB0, 0x75)).Replace(" ", "");
        }

        [Description("写入普通模式")]
        public void WriteNormalMode()
        {
            WriteModex(0x00);
        }

        [Description("写入工厂模式")]
        public void WriteFactoryModex()
        {
            WriteModex(0x01);
        }

        private void WriteModex(byte modexIndex)
        {
            if (Can1 == null)
                return;

            lock (_can1Locker)
            {
                Can1.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xB0, 0x75, new[] { modexIndex });
            }
        }

        #endregion

        #region 读零件版本号

        [Description("读所有版本信息")]
        public void ReadAllVer()
        {
            ReadEcuBootloaderSoftwareReferenceNumber();
            Thread.Sleep(500);
            ReadEcuPartNumber();
            Thread.Sleep(500);
            ReadEcuHardwareNumber();
            Thread.Sleep(500);
            ReadEcuSoftwareNumber();
            Thread.Sleep(500);
            ReadEcuCalibrationSoftwareNumber();
            Thread.Sleep(500);
            ReadNcfNumber();
            Thread.Sleep(500);
            ReadEcuConfigurationFileNumber();
            Thread.Sleep(500);
            ReadEcuProgrammingProcessFileNumber();
            Thread.Sleep(500);
            ReadSsbPartNumber();
            Thread.Sleep(500);
            ReadSsbHardwareNumber();
            Thread.Sleep(500);
            ReadSsbSoftwareNumber();
            Thread.Sleep(500);
            Read初始功能配置码();
        }

        //[ReadOnlyField("测试")]
        //public string Test = "ss";

        [Description("R,EcuSerialNumber-F18C")]
        public string EcuSerialNumber = string.Empty;

        [Description("R,EcuBootloaderSoftwareReferenceNumber-F183")]
        public string EcuBootloaderSoftwareReferenceNumber = string.Empty;

        [Description("R,ECUPartNumber-F187")]
        public string EcuPartNumber = string.Empty;

        [Description("R,ECUHardwareNumber-F191")]
        public string EcuHardwareNumber = string.Empty;

        [Description("R,ECUSoftwareNumber-F1A0")]
        public string EcuSoftwareNumber = string.Empty;

        [Description("R,EcuCalibrationSoftwareNumber-F1A1")]
        public string EcuCalibrationSoftwareNumber = string.Empty;

        [Description("R,SsbHardwareNumber-F192")]
        public string SsbHardwareNumber = string.Empty;

        [Description("R,SsbSoftwareNumber-F194")]
        public string SsbSoftwareNumber = string.Empty;

        [Description("R,ECUNCFNumber-F1A2")]
        public string EcuNcfNumber = string.Empty;

        [Description("R,EcuConfigurationFileNumber-F1A9")]
        public string EcuConfigurationFileNumber = string.Empty;

        [Description("R,EcuProgrammingProcessFileNumber-F1AA")]
        public string EcuProgrammingProcessFileNumber = string.Empty;

        [Description("R,SsbPartNumber-FD01")]
        public string SsbPartNumber = string.Empty;

        [Description("R,初始功能配置码-C000")]
        public string 初始功能配置码 = string.Empty;

        [Description("R,VBAT_Vol-0112")]
        public double VbatVol = -9999;

        [Description("R,Sensor5V-B020")]
        public double Sensor5V = -9999;

        [Description("R,烧写标志位-AFF0")]
        public string ProgramFlag = string.Empty;

        [Description("R,软件有效性标志位-AFFF")]
        public string SoftwareValidFlag = string.Empty;

        [Description("R,软件兼容性状态SoftwareCompatibilityStatus-AFFE")]
        public string SoftwareCompatibilityStatus = string.Empty;

        [Description("R,软件完整性状态SoftwareIntegrityStatus-AFFD")]
        public string SoftwareIntegrityStatus = string.Empty;

        [Description("R,刷新计数ProgrammingCounter-AFFC")]
        public string ProgrammingCounter = string.Empty;

        //[Description("写ECUPartNumber-F187")]
        //public void WrtieEcuPartNumber(string value)
        //{
        //    if (Can1 == null)
        //    {
        //        return;
        //    }

        //    var bs = new List<byte>();
        //    try
        //    {
        //        for (var i = 0; i < value.Length; i = i + 2)
        //        {
        //            bs.Add(Convert.ToByte(value.Substring(i, 2), 16));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return;
        //    }

        //    if (bs.Count != 5)
        //    {
        //        return;
        //    }

        //    lock (_can1Locker)
        //    {
        //        byte[] echo;
        //        if (Can1.CanBusWithUds.TryWriteData(
        //            CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x87, bs.ToArray()))
        //        {

        //        }
        //    }
        //}

        [Description("读EcuSerialNumber-F18C")]
        public void ReadEcuSerialNumber()
        {
            EcuSerialNumber = string.Empty;
            EcuSerialNumber = ReadDidViaCan1(0xF1, 0x8C).GetStringByAsciiBytes(false);
        }

        [Description("读EcuBootloaderSoftwareReferenceNumber-F183")]
        public void ReadEcuBootloaderSoftwareReferenceNumber()
        {
            EcuBootloaderSoftwareReferenceNumber = string.Empty;
            EcuBootloaderSoftwareReferenceNumber = ReadDidViaCan1(0xF1, 0x83).GetStringByAsciiBytes(false);
        }

        [Description("读ECUPartNumber-F187")]
        public void ReadEcuPartNumber()
        {
            EcuPartNumber = string.Empty;
            EcuPartNumber = ValueHelper.GetHextStr(ReadDidViaCan1(0xF1, 0x87)).Replace(" ", "");
        }

        [Description("读ECUHardwareNumber-F191")]
        public void ReadEcuHardwareNumber()
        {
            EcuHardwareNumber = string.Empty;
            EcuHardwareNumber = ValueHelper.GetHextStr(ReadDidViaCan1(0xF1, 0x91)).Replace(" ", "");
        }

        [Description("读ECUSoftwareNumber-F1A0")]
        public void ReadEcuSoftwareNumber()
        {
            EcuSoftwareNumber = string.Empty;
            EcuSoftwareNumber = ValueHelper.GetHextStr(ReadDidViaCan1(0xF1, 0xA0)).Replace(" ", "");
        }

        [Description("读EcuCalibrationSoftwareNumber-F1A1")]
        public void ReadEcuCalibrationSoftwareNumber()
        {
            EcuCalibrationSoftwareNumber = string.Empty;
            EcuCalibrationSoftwareNumber = ValueHelper.GetHextStr(ReadDidViaCan1(0xF1, 0xA1)).Replace(" ", "");
        }

        [Description("读SsbHardwareNumber-F192")]
        public void ReadSsbHardwareNumber()
        {
            SsbHardwareNumber = string.Empty;
            SsbHardwareNumber = ReadDidViaCan1(0xF1, 0x92).GetStringByAsciiBytes(false);
        }
        [Description("读SsbSoftwareNumber-F194")]
        public void ReadSsbSoftwareNumber()
        {
            SsbSoftwareNumber = string.Empty;
            SsbSoftwareNumber = ReadDidViaCan1(0xF1, 0x94).GetStringByAsciiBytes(false);
        }

        [Description("读ECUNCFNumber-F1A2")]
        public void ReadNcfNumber()
        {
            EcuNcfNumber = string.Empty;
            EcuNcfNumber = ValueHelper.GetHextStr(ReadDidViaCan1(0xF1, 0xA2)).Replace(" ", "");
        }

        [Description("读EcuConfigurationFileNumber-F1A9")]
        public void ReadEcuConfigurationFileNumber()
        {
            EcuConfigurationFileNumber = string.Empty;
            EcuConfigurationFileNumber = ValueHelper.GetHextStr(ReadDidViaCan1(0xF1, 0xA9)).Replace(" ", "");
        }

        [Description("读EcuProgrammingProcessFileNumber-F1AA")]
        public void ReadEcuProgrammingProcessFileNumber()
        {
            EcuProgrammingProcessFileNumber = string.Empty;
            EcuProgrammingProcessFileNumber = ValueHelper.GetHextStr(ReadDidViaCan1(0xF1, 0xAA)).Replace(" ", "");
        }

        [Description("读SsbPartNumber-FD01")]
        public void ReadSsbPartNumber()
        {
            SsbPartNumber = string.Empty;
            SsbPartNumber = ReadDidViaCan1(0xFD, 0x01).GetStringByAsciiBytes(false).Replace(" ", "");
        }

        [Description("读初始功能配置码-C000")]
        public void Read初始功能配置码()
        {
            初始功能配置码 = string.Empty;
            初始功能配置码 = ValueHelper.GetHextStr(ReadDidViaCan1(0xC0, 0x0)).Replace(" ", "");
        }

        [Description("读KL30-0112")]
        public void ReadVbatVol()
        {
            VbatVol = -9999;
            var read = ReadDidViaCan1(0x01, 0x12);

            if (read != null && read.Length == 1)
                VbatVol = (read[0]) * 0.1;
            //VbatVol = (read[0]) / 1024f * 5 / 0.248;
        }

        [Description("读传感器5V-B020")]
        public void ReadSensor5V()
        {
            Sensor5V = -9999;
            var read = ReadDidViaCan1(0xB0, 0x20);

            if (read != null && read.Length >= 2)
                Sensor5V = (read[0] * 256 + read[1]) * 133f / 20460f;
        }

        [Description("读烧写标志位-AFF0")]
        public void ReadProgramFlag()
        {
            ProgramFlag = string.Empty;
            ProgramFlag = ValueHelper.GetHextStr(ReadDidViaCan1(0xAF, 0xF0)).Replace(" ", "");
        }

        [Description("读软件有效性标志位-AFFF")]
        public void ReadProgramSoftwareValidFlag()
        {
            SoftwareValidFlag = string.Empty;
            SoftwareValidFlag = ValueHelper.GetHextStr(ReadDidViaCan1(0xAF, 0xFF)).Replace(" ", "");
        }

        [Description("读SoftwareCompatibilityStatus-AFFE")]
        public void ReadSoftwareCompatibilityStatus()
        {
            SoftwareCompatibilityStatus = string.Empty;
            SoftwareCompatibilityStatus = ValueHelper.GetHextStr(ReadDidViaCan1(0xAF, 0xFE)).Replace(" ", "");
        }

        [Description("读SoftwareIntegrityStatus-AFFD")]
        public void ReadSoftwareIntegrityStatus()
        {
            SoftwareIntegrityStatus = string.Empty;
            SoftwareIntegrityStatus = ValueHelper.GetHextStr(ReadDidViaCan1(0xAF, 0xFD)).Replace(" ", "");
        }

        [Description("读ProgrammingCounter-AFFC")]
        public void ReadProgrammingCounter()
        {
            ProgrammingCounter = string.Empty;
            ProgrammingCounter = ValueHelper.GetHextStr(ReadDidViaCan1(0xAF, 0xFC)).Replace(" ", "");
        }

        #endregion

        #region 输入SensorAD值测试

        [Description("R,J1-1-蒸发器温度传感器-B01A-EvaporatorSensorAD")]
        public double EvaporatorSensorAd = -9999;
        [Description("R,J1-2-左区吹面出风口温度传感器-B01B-FaceDischargeAirTemperatureSensor_FrontLeftAD")]
        public double FaceDischargeAirTemperatureSensorFrontLeftAd = -9999;
        [Description("R,J1-3-右区吹面出风口温度传感器-B01D-FaceDischargeAirTemperatureSensor_FrontRightAD")]
        public double FaceDischargeAirTemperatureSensorFrontRightAd = -9999;
        [Description("R,J1-5-右区吹脚出风口温度传感器-B01E-FootDischargeAirTemperatureSensor_FrontRightAD")]
        public double FootDischargeAirTemperatureSensorFrontRightAd = -9999;
        [Description("R,J1-6-除霜出风口温度传感器-B05F-DefrostDischargeAirTemperatureSensorAD")]
        public double DefrostDischargeAirTemperatureSensorAd = -9999;
        [Description("R,J1-7-左区吹脚出风口温度传感器-B01C-FootDischargeAirTemperatureSensor_FrontLeftSensorAD")]
        public double FootDischargeAirTemperatureSensorFrontLeftSensorAd = -9999;
        [Description("R,J2-1-外温传感器-B019-AmbientSensorAD")]
        public double AmbientSensorAd = -9999;
        [Description("R,J2-27-室外换热器出口温度传感器-B066-Oulet_OHXTemperatureSensorAD")]
        public double OuletOhxTemperatureSensorAd = -9999;
        [Description("R,J2-28-压缩机吸气温度传感器-B060-Inlet_CompressorTemperatureSensorAD")]
        public double InletCompressorTemperatureSensorAd = -9999;
        [Description("R,J2-43-压缩机排气温度传感器-B061-OutletCompressorTemperatureSensorAD")]
        public double OutletCompressorTemperatureSensorAd = -9999;
        [Description("R,J2-44-室内冷凝器出口压力传感器-B063-OutletWaterCoolingCondenserPressureSensorAD")]
        public double OutletWaterCoolingCondenserPressureSensorAd = -9999;
        [Description("R,J2-45-室内冷凝器出口温度传感器-B062-OutletWaterCoolingCondenserTemperatureSensorAD")]
        public double OutletWaterCoolingCondenserTemperatureSensorAd = -9999;
        [Description("R,J2-46-蒸发器出口压力传感器-B065-OutletevaporatorPressureSensorAD")]
        public double OutletevaporatorPressureSensorAd = -9999;
        [Description("R,J2-47-蒸发器出口温度传感器-B064-OutletevaporatorTemperatureSensorAD")]
        public double OutletevaporatorTemperatureSensorAd = -9999;
        [Description("R,J3-7-Heatcore入水温度传感器-B069-HeatCoreInletCoolantTemperatureSensorAD")]
        public double HeatCoreInletCoolantTemperatureSensorAd = -9999;
        [Description("R,J3-8-WCDS入水温度传感器-B06A-WaterCoolingCondenserInletCoolantTemperatureSensorAD")]
        public double WaterCoolingCondenserInletCoolantTemperatureSensorAd = -9999;
        [Description("R,J3-9-Chiller入水温度传感器-B068-ChillerInletCoolantTemperatureSensorAD")]
        public double ChillerInletCoolantTemperatureSensorAd = -9999;
        [Description("R,J3-18-电机进水温度传感器-B067-EDUCoolantTemperatureSensorAD")]
        public double EduCoolantTemperatureSensorAd = -9999;

        [Description("读输入SensorAD值")]
        public void ReadSensorAd()
        {
            var fi = GetType().GetFields();

            foreach (var fieldInfo in fi)
            {
                if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) == null)
                    continue;

                var des =
                    ((DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)))
                        .Description;

                var sp = des.Split(',');

                var name = fieldInfo.Name;
                var type = fieldInfo.FieldType.ToString();
                var inputOutputType = sp[0];
                var fieldNote = sp[1];

                if (inputOutputType == "R")
                {
                    if (des.StartsWith("R,J"))
                    {
                        var findB0 = des.IndexOf("-B0", StringComparison.Ordinal);
                        if (findB0 != -1)
                        {
                            fieldInfo.SetValue(this, -9999);

                            var did = des.Substring(findB0 + 1, 4);

                            var didHi = Convert.ToByte(did.Substring(0, 2), 16);
                            var didLo = Convert.ToByte(did.Substring(2, 2), 16);

                            var read = ReadDidViaCan1(didHi, didLo);
                            if (read != null && read.Length >= 2)
                            {
                                var ad = read[0] * 256 + read[1];
                                var volt = ad / 1024f * 5;
                                fieldInfo.SetValue(this, volt);
                            }

                            Thread.Sleep(20);
                        }
                    }
                }
            }
        }

        #endregion

        #region DC Motor Routine Results

        [Description("R,左区模式风门电机标定结果")]
        public string FrontModeMotorCaliResult = string.Empty;
        [Description("R,后区模式风门电机标定结果")]
        public string RearModeMotorCaliResult = string.Empty;
        [Description("R,除霜风门电机标定结果")]
        public string DefrostModeMotorCaliResult = string.Empty;
        [Description("R,内外循环风门电机标定结果")]
        public string IntakeModeMotorCaliResult = string.Empty;
        [Description("R,HEPA风门电机标定结果")]
        public string ServoHepaMotorCaliResult = string.Empty;

        [Description("读取电机标定结果")]
        public void ReadDcMotorRoutineResults()
        {
            FrontModeMotorCaliResult = string.Empty;
            RearModeMotorCaliResult = string.Empty;
            DefrostModeMotorCaliResult = string.Empty;
            IntakeModeMotorCaliResult = string.Empty;
            ServoHepaMotorCaliResult = string.Empty;

            var result = ReadDidViaCan1(0xFD, 0x02);

            if (result.Length == 2)
            {
                var str1 = Convert.ToString(result[0], 2).PadLeft(8, '0');
                var str2 = Convert.ToString(result[1], 2).PadLeft(8, '0');

                FrontModeMotorCaliResult = Convert.ToByte(str1.Substring(6, 2), 2).ToString();
                RearModeMotorCaliResult = Convert.ToByte(str1.Substring(4, 2), 2).ToString();
                DefrostModeMotorCaliResult = Convert.ToByte(str1.Substring(2, 2), 2).ToString();
                IntakeModeMotorCaliResult = Convert.ToByte(str1.Substring(0, 2), 2).ToString();

                ServoHepaMotorCaliResult = Convert.ToByte(str2.Substring(6, 2), 2).ToString();
            }
        }

        #endregion

        #region 2F服务

        private void IoControl(byte didHi, byte didLo, IEnumerable<byte> optionBytes)
        {
            if (Can1 == null)
                return;

            lock (_can1Locker)
            {
                Can1.CanBusWithUds.TryInputOutputControl(
                    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Standard, didHi, didLo,
                    Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, optionBytes);
            }
        }

        #endregion

        #region 输入PWM 信号

        [Description("R,AQS信号输入PWM2-0xE019")]
        public string Aqs信号输入pwm2 = string.Empty;

        [Description("Read-AQS信号输入PWM2-0xE019")]
        public void ReadAqs信号输入pwm2()
        {
            Aqs信号输入pwm2 = string.Empty;
            Aqs信号输入pwm2 = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x19)).ToString();
        }

        //[Description("AQS信号输入PWM2")]
        //public void Aqs信号输入Pwm2(string pwm)
        //{
        //    ushort pwmValue;
        //    if (ushort.TryParse(pwm, out pwmValue))
        //    {
        //        var pwmBytes = BitConverter.GetBytes(pwmValue).Reverse().ToArray();
        //        IoControl(0xE0, 0x19, pwmBytes);
        //    }
        //}

        #endregion

        #region 输出PWM,LSD,HSD

        [Description("R,冷却风扇PWM-0xE060")]
        public string 冷却风扇pwm = string.Empty;

        [Description("Read-冷却风扇PWM-0xE060")]
        public void Read冷却风扇Pwm()
        {
            冷却风扇pwm = string.Empty;
            冷却风扇pwm = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x60)).ToString();
        }

        [Description("冷却风扇PWM")]
        public void 冷却风扇Pwm(string pwm)
        {
            ushort pwmValue;
            if (ushort.TryParse(pwm, out pwmValue))
            {
                var pwmBytes = BitConverter.GetBytes(pwmValue).Reverse().ToArray();
                IoControl(0xE0, 0x60, pwmBytes);
            }
        }

        [Description("R,内冷阀Hsd-0xE04F")]
        public string 内冷阀HsdReadValue = string.Empty;

        [Description("Read-内冷阀Hsd-0xE04F")]
        public void Read内冷阀Hsd()
        {
            内冷阀HsdReadValue = string.Empty;
            内冷阀HsdReadValue = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x4F)).ToString();
        }

        [Description("内冷阀HSD")]
        public void 内冷阀Hsd(string isOn)
        {
            var onOff = isOn == "1";
            IoControl(0xE0, 0x4F, new[] { onOff ? (byte)0x01 : (byte)0x00 });
        }

        [Description("R,制热阀Hsd-0xE050")]
        public string 制热阀HsdReadValue = string.Empty;

        [Description("Read-制热阀Hsd-0xE050")]
        public void Read制热阀Hsd()
        {
            制热阀HsdReadValue = string.Empty;
            制热阀HsdReadValue = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x50)).ToString();
        }

        [Description("制热阀HSD")]
        public void 制热阀Hsd(string isOn)
        {
            var onOff = isOn == "1";
            IoControl(0xE0, 0x50, new[] { onOff ? (byte)0x01 : (byte)0x00 });
        }

        [Description("R,制冷阀Hsd-0xE051")]
        public string 制冷阀HsdReadValue = string.Empty;

        [Description("Read-制冷阀Hsd-0xE051")]
        public void Read制冷阀Hsd()
        {
            制冷阀HsdReadValue = string.Empty;
            制冷阀HsdReadValue = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x51)).ToString();
        }

        [Description("制冷阀HSD")]
        public void 制冷阀Hsd(string isOn)
        {
            var onOff = isOn == "1";
            IoControl(0xE0, 0x51, new[] { onOff ? (byte)0x01 : (byte)0x00 });
        }

        [Description("R,前风窗加热丝-0xE072")]
        public string 前风窗加热丝ReadValue = string.Empty;

        [Description("Read-前风窗加热丝-0xE072")]
        public void Read前风窗加热丝()
        {
            前风窗加热丝ReadValue = string.Empty;
            前风窗加热丝ReadValue = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x72)).ToString();
        }

        [Description("R,前风窗加热丝")]
        public void 前风窗加热丝(string isOn)
        {
            var onOff = isOn == "1";
            IoControl(0xE0, 0x72, new[] { onOff ? (byte)0x01 : (byte)0x00 });
        }

        [Description("R,后风窗加热驱动-0xE03C")]
        public string 后风窗加热驱动ReadValue = string.Empty;

        [Description("Read-后风窗加热驱动-0xE03C")]
        public void Read后风窗加热驱动()
        {
            后风窗加热驱动ReadValue = string.Empty;
            后风窗加热驱动ReadValue = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x3C)).ToString();
        }

        [Description("后风窗加热驱动")]
        public void 后风窗加热驱动(string isOn)
        {
            var onOff = isOn == "1";
            IoControl(0xE0, 0x3C, new[] { onOff ? (byte)0x01 : (byte)0x00 });
        }

        #endregion

        #region 膨胀阀输出
        [Description("R,Chiller电子膨胀阀-0xE041")]
        public string Chiller电子膨胀阀ReadValue = string.Empty;

        [Description("Read-Chiller电子膨胀阀-0xE041")]
        public void ReadChiller电子膨胀阀()
        {
            Chiller电子膨胀阀ReadValue = string.Empty;
            Chiller电子膨胀阀ReadValue = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x41)).ToString();
        }

        [Description("Wrtie-Chiller电子膨胀阀")]
        public void Chiller电子膨胀阀(string value)
        {
            ushort ushortValue;
            if (ushort.TryParse(value, out ushortValue))
            {
                var valueBytes = BitConverter.GetBytes(ushortValue).Reverse().ToArray();
                IoControl(0xE0, 0x41, valueBytes);
            }
        }

        [Description("R,制热电子膨胀阀-0xE056")]
        public string 制热电子膨胀阀ReadValue = string.Empty;

        [Description("Read-制热电子膨胀阀-0xE056")]
        public void Read制热电子膨胀阀()
        {
            制热电子膨胀阀ReadValue = string.Empty;
            制热电子膨胀阀ReadValue = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x56)).ToString();
        }

        [Description("Wrtie-制热电子膨胀阀")]
        public void 制热电子膨胀阀(string value)
        {
            ushort ushortValue;
            if (ushort.TryParse(value, out ushortValue))
            {
                var valueBytes = BitConverter.GetBytes(ushortValue).Reverse().ToArray();
                IoControl(0xE0, 0x56, valueBytes);
            }
        }

        [Description("R,前蒸发器电子膨胀阀-0xE058")]
        public string 前蒸发器电子膨胀阀ReadValue = string.Empty;

        [Description("Read-前蒸发器电子膨胀阀-0xE058")]
        public void Read前蒸发器电子膨胀阀()
        {
            前蒸发器电子膨胀阀ReadValue = string.Empty;
            前蒸发器电子膨胀阀ReadValue = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x58)).ToString();
        }

        [Description("Wrtie-前蒸发器电子膨胀阀")]
        public void 前蒸发器电子膨胀阀(string value)
        {
            ushort ushortValue;
            if (ushort.TryParse(value, out ushortValue))
            {
                var valueBytes = BitConverter.GetBytes(ushortValue).Reverse().ToArray();
                IoControl(0xE0, 0x58, valueBytes);
            }
        }
        #endregion

        #region DC电机输出

        [Description("R,左区模式风门电机位置反馈-0xE001")]
        public string 左区模式风门电机位置反馈 = string.Empty;

        [Description("Read-左区模式风门电机位置反馈-0xE001")]
        public void Read左区模式风门电机位置()
        {
            左区模式风门电机位置反馈 = string.Empty;
            左区模式风门电机位置反馈 = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x01)).ToString();
        }

        [Description("Wrtie-左区模式风门电机位置")]
        public void Write左区模式风门电机位置(string value)
        {
            byte byteValue;
            if (byte.TryParse(value, out byteValue))
            {
                IoControl(0xE0, 0x01, new[] { byteValue });
            }
        }

        [Description("R,后区模式风门电机位置反馈-0xE003")]
        public string 后区模式风门电机位置反馈 = string.Empty;

        [Description("Read-后区模式风门电机位置反馈-0xE003")]
        public void Read后区模式风门电机位置()
        {
            后区模式风门电机位置反馈 = string.Empty;
            后区模式风门电机位置反馈 = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x03)).ToString();
        }

        [Description("Wrtie-后区模式风门电机位置")]
        public void Write后区模式风门电机位置(string value)
        {
            byte byteValue;
            if (byte.TryParse(value, out byteValue))
            {
                IoControl(0xE0, 0x03, new[] { byteValue });
            }
        }

        [Description("R,内外循环风门电机位置反馈-0xE002")]
        public string 内外循环风门电机位置反馈 = string.Empty;

        [Description("Read-内外循环风门电机位置反馈-0xE002")]
        public void Read内外循环风门电机位置()
        {
            内外循环风门电机位置反馈 = string.Empty;
            内外循环风门电机位置反馈 = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x02)).ToString();
        }

        [Description("Wrtie-内外循环风门电机位置")]
        public void Write内外循环风门电机位置(string value)
        {
            byte byteValue;
            if (byte.TryParse(value, out byteValue))
            {
                IoControl(0xE0, 0x02, new[] { byteValue });
            }
        }

        [Description("R,除霜风门电机位置反馈-0xE006")]
        public string 除霜风门电机位置反馈 = string.Empty;

        [Description("Read-除霜风门电机位置反馈-0xE006")]
        public void Read除霜风门电机位置()
        {
            除霜风门电机位置反馈 = string.Empty;
            除霜风门电机位置反馈 = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x06)).ToString();
        }

        [Description("Wrtie-除霜风门电机位置")]
        public void Write除霜风门电机位置(string value)
        {
            byte byteValue;
            if (byte.TryParse(value, out byteValue))
            {
                IoControl(0xE0, 0x06, new[] { byteValue });
            }
        }

        [Description("R,HEPA风门电机位置反馈-0xE061")]
        public string Hepa风门电机位置反馈 = string.Empty;

        [Description("Read-HEPA风门电机位置反馈-0xE061")]
        public void ReadHepa风门电机位置()
        {
            Hepa风门电机位置反馈 = string.Empty;
            Hepa风门电机位置反馈 = ValueHelper.GetDecimal(ReadDidViaCan1(0xE0, 0x61)).ToString();
        }

        [Description("Wrtie-HEPA风门电机位置")]
        public void WriteHepa风门电机位置(string value)
        {
            byte byteValue;
            if (byte.TryParse(value, out byteValue))
            {
                IoControl(0xE0, 0x61, new[] { byteValue });
            }
        }

        #endregion

        private byte[] ReadDidViaCan1(byte didHi, byte didLo)
        {
            if (Can1 == null)
                return new List<byte>().ToArray();

            lock (_can1Locker)
            {
                byte[] echoBytes;
                if (Can1.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    return new List<byte>().ToArray();

                    Thread.Sleep(250);
                    if (!Can1.CanBusWithUds.TryReadData(
                        CanDiagnosisRequestPhyCanId,
                        CanDiagnosisResponseCanId,
                        CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                        didHi, didLo, out echoBytes))
                        return new List<byte>().ToArray();
                    return echoBytes ?? new List<byte>().ToArray();
                }


                Thread.Sleep(250);
                if (!Can1.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                return new List<byte>().ToArray();

                Thread.Sleep(250);
                if (!Can1.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!Can1.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();
                return echoBytes ?? new List<byte>().ToArray();
            }
        }

        #region APP下载

        [Description("R,下载结果")]
        public string DownloadResult = string.Empty;

        //[Description("R/W,APP文件路径")]
        //public string AppFilePath = @"G:\Proj-2021\华域三电-S12L控制器\hex-20221008\S12L_EP2_X170_X022IS0_B022IS0_APP(1).s19";

        //[Description("R/W,CAL文件路径")]
        //public string CaliFailPath = @"G:\Proj-2021\华域三电-S12L控制器\hex-20221008\S12L_EP2_X170_X022IS0_B022IS0_CAL(1).s19";

        //[Description("R/W,APP文件路径")]
        //public string AppFilePath = @"G:\Proj-2021\华域三电-S12L控制器\hex-20221110\S12L_EP2_X023IS0_B023IS0_APP.s19";

        //[Description("R/W,CAL文件路径")]
        //public string CaliFailPath = @"G:\Proj-2021\华域三电-S12L控制器\hex-20221110\S12L_EP2_X023IS0_B023IS0_CAL.s19";

        //[Description("R/W,APP文件路径")]
        //public string AppFilePath = @"G:\Proj-2021\华域三电-S12L控制器\hex-20221129\IS1\S12L_X023_IS1_APP-V0.1.s19";

        //[Description("R/W,CAL文件路径")]
        //public string CaliFailPath = @"G:\Proj-2021\华域三电-S12L控制器\hex-20221129\IS1\S12L_X023_IS1_CAL-V0.1.s19";

        //[Description("R/W,APP文件路径")]
        //public string AppFilePath = @"G:\Proj-2021\华域三电-S12L控制器\hex-20221129\IS2\S12L_X023IS2_B023IS2_APP.s19";

        //[Description("R/W,CAL文件路径")]
        //public string CaliFailPath = @"G:\Proj-2021\华域三电-S12L控制器\hex-20221129\IS2\S12L_X023IS2_B023IS2_CAL.s19";

        [Description("R/W,APP文件路径")]
        public string AppFilePath =
            @"X:\Proj-2021\华域三电-S12L-X1控制器\Version\20240723\S12L_X1_PPV_X302_IS2_APP_Signed.s19";

        [Description("R/W,CAL文件路径")]
        public string CaliFailPath = @"X:\Proj-2021\华域三电-S12L-X1控制器\Version\20240723\S12L_X1_PPV_X302_IS2_CAL_Signed.s19";

        private static readonly object FileLocker = new object();

        [Description("下载")]
        public void DownloadFile()
        {
            var dlAction = new Action(() =>
            {
                // DownloadResult = string.Empty;
                DownloadResult = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - 下载中";

                if (Can1 == null)
                {
                    DownloadResult = "NG CAN未初始化";
                    return;
                }

                ControllerAwake();
                Thread.Sleep(2500);
                ControllerSleep();
                Thread.Sleep(100);

                var fileList = new List<string>();

                lock (FileLocker)
                {
                    if (!string.IsNullOrEmpty(AppFilePath))
                    {
                        if (!File.Exists(AppFilePath))
                        {
                            DownloadResult = "NG APP文件不存在";
                            return;
                        }
                        else
                        {
                            fileList.Add(AppFilePath);
                        }
                    }

                    if (!string.IsNullOrEmpty(CaliFailPath))
                    {
                        if (!File.Exists(CaliFailPath))
                        {
                            DownloadResult = "NG CFG文件不存在";
                            return;
                        }
                        else
                        {
                            fileList.Add(CaliFailPath);
                        }
                    }
                }

                if (!fileList.Any())
                {
                    DownloadResult = "NG 未指定下载文件";
                    return;
                }

                var downloadAction = new Action(() =>
                {
                    if (!PreProgramming(ref DownloadResult))
                        return;

                    if (
                        fileList.Select(SRecordFileHelper.GetSRecordLineData)
                            .Select(SRecordFileHelper.GetBlocks)
                            .Any(blocks => !Can1.CanBusWithUds.TransferData(
                                CanDiagnosisRequestPhyCanId,
                                CanDiagnosisResponseCanId,
                                CanBus.CanType.Standard,
                                blocks, false, ref DownloadResult, is37RequireCrc: 1)))
                    {
                        DownloadResult = "NG 下载Block失败：" + DownloadResult;
                        return;
                    }

                    if (
                        !Can1.CanBusWithUds.TryStartRoutineControl(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                            CanBus.CanType.Standard, Uds14229Helper.RoutineControl.CheckProgrammingIntegrity))
                    {
                        DownloadResult = "NG 3101DFFF失败：" + DownloadResult;
                        return;
                    }

                    if (
                        !Can1.CanBusWithUds.TryStartRoutineControl(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                            CanBus.CanType.Standard, Uds14229Helper.RoutineControl.CheckProgrammingDependencies))
                    {
                        DownloadResult = "NG 3101FF01失败：" + DownloadResult;
                        return;
                    }

                    byte[] ecuResetEcho;
                    Can1.CanBusWithUds
                        .TesterTryRequest(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho, CanBus.CanType.Standard);

                    //if (
                    //   !Can1.CanBusWithUds.TryStartRoutineControl(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    //       CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknowm5, new[] { (byte)0x01 }))
                    //{
                    //    DownloadResult = "NG 3101DFFE01失败：" + DownloadResult;
                    //    return;
                    //}

                    //Can.SendCanDatas(new[]
                    //{
                    //    new CanBus.CanDataPackage(CanDiagnosisRequestPhyCanId, CanBus.CanProtocol.Can,
                    //        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                    //        new byte[] {0x04, 0x31, 0x01, 0xDF, 0xFF, 0xAA, 0xAA, 0xAA})
                    //});
                    //Thread.Sleep(2000);

                    DownloadResult = "OK";
                });

                var st = new Stopwatch();
                st.Start();
                downloadAction.Invoke();
                st.Stop();
                DownloadResult += " " + st.ElapsedMilliseconds / 1000 + "s";
            });

            dlAction.Invoke();

            //if (!DownloadResult.Contains("OK"))
            //{
            //    EcuReset();
            //    Thread.Sleep(1000);
            //    dlAction.Invoke();
            //}
        }

        /// <summary>
        /// 预编程
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool PreProgramming(ref string result)
        {
            if (!Can1.CanBusWithUds.TryEnterExtendedSession(
              CanDiagnosisRequestPhyCanId,
              CanDiagnosisResponseCanId,
              CanBus.CanType.Standard))
            {
                //result = "NG 进入编程模式1003失败";
                //return false;
            }

            Thread.Sleep(550);

            if (!Can1.CanBusWithUds.TryEnterProgrammingSession(
                CanDiagnosisRequestPhyCanId,
                CanDiagnosisResponseCanId,
                CanBus.CanType.Standard))
            {
                result = "NG 进入编程模式1002失败";
                return false;
            }

            //Thread.Sleep(1550);
            Thread.Sleep(550);

            if (!SecurityAccess("5"))
            {
                result = "NG SeedKey请求失败";
                return false;
            }

            //Thread.Sleep(550);
            Thread.Sleep(250);

            return true;
        }

        #endregion

        #region LIN测试

        [Description("R,LIN1测试")]
        public string Lin1MsgTest;
        [Description("R,LIN2测试")]
        public string Lin2MsgTest;
        [Description("R,LIN3测试")]
        public string Lin3MsgTest;
        [Description("R,LIN4测试")]
        public string Lin4MsgTest;
        [Description("R,LIN5测试")]
        public string Lin5MsgTest;
        [Description("R,LIN6测试")]
        public string Lin6MsgTest;

        private int _currentLinTestChannel;

        private void LinBus_PushLinMsg(string name, LinBus.LinDataPackage data)
        {
            if (_currentLinTestChannel != -1)
            {
                var linBusFieldName = "Lin" + _currentLinTestChannel;
                var linBusField = GetType().GetField(linBusFieldName);
                if (linBusField != null)
                {
                    var linBus = (LinBus)linBusField.GetValue(this);

                    if (linBus != null)
                    {
                        if (name == linBus.Name)
                        {
                            var fieldName = string.Format("Lin{0}MsgTest", _currentLinTestChannel);
                            var field = GetType().GetField(fieldName);
                            if (field != null)
                            {
                                var str = field.GetValue(this).ToString();
                                str += ValueHelper.GetHextStr(data.LinId) + " ";
                                field.SetValue(this, str);
                            }
                        }
                    }
                }
            }
        }

        [Description("LIN测试")]
        public void LinMsgTest(string linChannel)
        {
            int ch;
            if (int.TryParse(linChannel, out ch))
            {
                if (ch >= 1 && ch <= 6)
                {
                    var fieldName = string.Format("Lin{0}MsgTest", ch);
                    var field = GetType().GetField(fieldName);
                    if (field != null)
                    {
                        field.SetValue(this, string.Empty);
                        _currentLinTestChannel = ch;
                        Thread.Sleep(250);
                        _currentLinTestChannel = -1;

                        var str = field.GetValue(this).ToString();
                        if (string.IsNullOrEmpty(str))
                        {
                            field.SetValue(this, "NG");
                        }
                        else
                        {
                            field.SetValue(this, "OK ");
                        }
                    }
                }
            }
        }

        #endregion

        #region CAN测试

        [Description("R,CAN1测试")]
        public string Can1MsgTest;
        [Description("R,CAN2测试")]
        public string Can2MsgTest;

        private bool _isCheckCan1;
        private bool _isCheckCan2;

        private List<CanBus.CanDataPackage> _can1Buff = new List<CanBus.CanDataPackage>();
        private List<CanBus.CanDataPackage> _can2Buff = new List<CanBus.CanDataPackage>();

        public void CanMsgTest(string canChannel)
        {
            int ch;
            if (int.TryParse(canChannel, out ch))
            {
                if (ch == 1)
                {
                    Can1MsgTest = string.Empty;
                    _can1Buff.Clear();

                    if (Can1 == null)
                    {
                        Can1MsgTest = "NG CAN1未初始化";
                    }
                    else
                    {
                        _isCheckCan1 = true;
                        var listCanId = new List<uint>
                        {
                            0x363,
                            0x388,
                            0x37c,
                            0x3a8,
                            0x38c,
                            0x3a7,
                            0x33d,
                            0x373,
                            0x2bd,
                            0x365,
                            0x272,
                            0x2f4
                        };

                        //foreach (var t in listCanId)
                        //    Can1.AddDoNotFilterCanId(t);
                        //Can1.CanRecvDataPackages.Clear();
                        Thread.Sleep(1000);

                        _isCheckCan1 = false;
                        var isOk = listCanId.All(canId => _can1Buff.FindAll(f => f.CanId == canId).Any());
                        _can1Buff.Clear();
                        Can1MsgTest = isOk ? @"OK" : @"NG";
                    }
                }
                else if (ch == 2)
                {
                    Can2MsgTest = string.Empty;
                    _can2Buff.Clear();

                    if (Can2 == null)
                    {
                        Can2MsgTest = "NG CAN2未初始化";
                    }
                    else
                    {
                        _isCheckCan2 = true;
                        var listCanId = new List<uint>
                        {
                            0xa,
                            0xb,
                            0xc,
                            0xd,
                            0xe,
                            0xf,
                            0x1,
                            0x2,
                            0x3,
                            0x4,
                            0x5,
                            0x6,
                            0x7,
                            0x8,
                            0x9,
                        };

                        //foreach (var t in listCanId)
                        //    Can2.AddDoNotFilterCanId(t);
                        //Can2.CanRecvDataPackages.Clear();
                        Thread.Sleep(1000);
                        _isCheckCan2 = false;
                        var isOk = listCanId.All(canId => _can2Buff.FindAll(f => f.CanId == canId).Any());
                        _can2Buff.Clear();
                        Can2MsgTest = isOk ? @"OK" : @"NG";
                    }
                }
            }
        }

        public void TestCan1AndCan2Cycle()
        {
            var t1 = new Task(() =>
            {
                CanMsgTest(1.ToString());
            });

            var t2 = new Task(() =>
            {
                CanMsgTest(2.ToString());
            });

            t1.Start();
            t2.Start();
            Task.WaitAll(t1, t2);
        }

        #endregion

        #region 读写MTC

        public string ToWriteBarcode = string.Empty;

        public string WtireMtcResult = string.Empty;

        public void ClearBarcode()
        {
            ToWriteBarcode = string.Empty;
        }

        public void WriteAndReadMtc()
        {
            WtireMtcResult = string.Empty;

            if (Can1 == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(ToWriteBarcode))
            {
                WtireMtcResult = "NG 条码为空";
                return;
            }

            if (ToWriteBarcode == "样件模式")
            {
                ReadEcuSerialNumber();
                WtireMtcResult = "OK 样件模式不写入追溯信息 读取的追溯信息为：" + EcuSerialNumber;
                return;
            }

            if (ToWriteBarcode.Length != 47)
            {
                WtireMtcResult = "NG 条码长度不对";
                return;
            }

            try
            {
                var toWriteStr = ToWriteBarcode.Substring(ToWriteBarcode.Length - 16, 16);

                var toWriteBytes = Encoding.ASCII.GetBytes(toWriteStr);
                lock (_can1Locker)
                    Can1.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                            CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x8C, toWriteBytes);

                Thread.Sleep(1000);
                ReadEcuSerialNumber();
                var readStr = EcuSerialNumber;

                if (readStr.Length == 16 && readStr == toWriteStr)
                {
                    WtireMtcResult = "OK ";
                }
                else
                {
                    WtireMtcResult = "NG ";
                }

                WtireMtcResult += string.Format("写入：{0}，读取{1}。", toWriteStr, readStr);
            }
            catch (Exception ex)
            {
                WtireMtcResult = "NG " + ex.Message;
            }
        }

        public void Write初始功能配置码(string value)
        {
            if (Can1 == null)
            {
                return;
            }

            try
            {
                // 09 00 80 80 00 03 00 00
                var bs = new List<byte>();
                for (var i = 0; i < value.Length; i = i + 2)
                {
                    var b = Convert.ToByte(value.Substring(i, 2), 16);
                    bs.Add(b);
                }

                lock (_can1Locker)
                    Can1.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                            CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xC0, 0x00, bs);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion
    }
}
