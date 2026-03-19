using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;
using DBUtility;

namespace Controller
{
    [Description("CAN-Product,MIFA-EPMCU")]
    public sealed class MifaEpMcu : ControllerBase
    {
        public CanBus Can;

        public MifaEpMcu(string name)
            : base(name)
        {
            if (_keepNetworkThread != null)
            {
                _keepNetworkThread.Abort();
                _keepNetworkThread.Join();
            }

            _keepNetworkThread = new Thread(KeepNetwork) { IsBackground = true };
            _keepNetworkThread.Start();

            if (_keepExtendedSessionThread != null)
            {
                _keepExtendedSessionThread.Abort();
                _keepExtendedSessionThread.Join();
            }

            _keepExtendedSessionThread = new Thread(KeepExtendedSession) { IsBackground = true };
            _keepExtendedSessionThread.Start();
        }

        ~MifaEpMcu()
        {
            Dispose();
        }

        private readonly Thread _keepExtendedSessionThread;
        private readonly Thread _keepNetworkThread;
        private readonly object _lockSend = new object();
        private bool _isInExtendedSession;
        private bool _isSleep = true;
        private const uint CanDiagnosisRequestPhyCanId = 0x775;
        //private const uint CanDiagnosisRequestFunCanId = 0x7DF;
        private const uint CanDiagnosisResponseCanId = 0x77D;

        private const uint MaskLevel1 = 0x453b5050u;
        private const uint MaskLevel2 = 0x50bb0545u;

        private void KeepNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                if (Can == null)
                    continue;

                if (_isSleep)
                    continue;
                lock (_lockSend)
                {
                    Can.SendStandardCanData(
                       0x401, new byte[] { 0x81, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                }
            }
        }

        private void KeepExtendedSession()
        {
            while (_keepExtendedSessionThread.IsAlive)
            {
                if (!_keepExtendedSessionThread.IsAlive)
                    break;

                Thread.Sleep(1200);

                if (Can == null)
                    continue;

                if (!_isInExtendedSession)
                    continue;
                lock (_lockSend)
                    Can.SendStandardCanData(CanDiagnosisRequestPhyCanId, new byte[] { 0x02, 0x3e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            }
        }

        [Description("唤醒")]
        public void Awake()
        {
            _isSleep = false;
        }

        [Description("休眠")]
        public void Sleep()
        {
            _isSleep = true;
        }

        [Description("进入正常模式")]
        public void EnterDefaultSession()
        {
            if (Can == null)
                return;

            lock (_lockSend)
            {
                _isInExtendedSession = false;

                if (Can.CanBusWithUds.TryEnterDefaultSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can))
                    return;

                Thread.Sleep(500);

                Can.CanBusWithUds.TryEnterDefaultSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can);
            }
        }

        [Description("进入拓展模式")]
        public void EnterExtendedSession()
        {
            if (Can == null)
                return;

            lock (_lockSend)
            {
                if (Can.CanBusWithUds.TryEnterExtendedSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (Can.CanBusWithUds.TryEnterExtendedSession(
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
            if (Can == null)
                return;

            _isInExtendedSession = false;

            lock (_lockSend)
            {
                if (Can.CanBusWithUds.TryEnterProgrammingSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    return;
                }

                Thread.Sleep(500);
                Can.CanBusWithUds.TryEnterProgrammingSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard);
            }
        }

        [Description("解锁SeedKey")]
        public bool SecurityAccess(string subFunc)
        {
            if (Can == null)
                return false;

            if (subFunc != "1" && subFunc != "5")
                return false;

            var subFunctoion = byte.Parse(subFunc);

            lock (_lockSend)
            {
                byte[] seedBytes;
                if (!Can.CanBusWithUds.TryRequestSeed(
                    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, subFunctoion, out seedBytes))
                    return false;

                if (seedBytes == null || seedBytes.Length != 4)
                    return false;
                var keyBytes = CalcKey(seedBytes.ToArray(), subFunc == "1" ? MaskLevel1 : MaskLevel2).ToArray();

                return Can.CanBusWithUds.TrySendKey(
                     CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                     CanBus.CanType.Standard, (byte)(subFunctoion + 0x01), keyBytes);
            }
        }

        private static IEnumerable<byte> CalcKey(byte[] seedBytes, uint learnmask)
        {
            Array.Reverse(seedBytes);
            var securitySeed = BitConverter.ToUInt32(seedBytes, 0);
            uint securityKey;

            if (securitySeed != 0)
            {
                for (var i = 0; i < 35; i++)
                {
                    if ((securitySeed & 0x80000000) > 0)
                    {
                        securitySeed = securitySeed << 1;
                        securitySeed = securitySeed ^ learnmask;
                    }
                    else
                    {
                        securitySeed = securitySeed << 1;
                    }
                }
                securityKey = securitySeed;
            }
            else
            {
                securityKey = 0;
            }

            var keyBytes = BitConverter.GetBytes(securityKey).Reverse().ToArray();
            return keyBytes;
        }

        #region 版本信息

        [Description("R,生产序列号-F18C")]
        public string ProductSerialNo;

        [Description("R,总成零件号-F187")]
        public string PartNo;

        [Description("R,引导程序零件号-F183")]
        public string FblPartNo;

        [Description("R,应用程序零件号-F1A0")]
        public string AppPartNo;

        [Description("R,供应商软件参考号-F194")]
        public string SupplySwNo;

        [Description("R,标定程序零件号-F1A1")]
        public string CaliPartNo;

        [Description("ReadF18C-生产序列号")]
        public void ReadProductSerialNo()
        {
            ProductSerialNo = string.Empty;
            ProductSerialNo = ReadDidViaCan(0xF1, 0x8C).GetStringByAsciiBytes(false);
        }

        [Description("ReadF187-总成零件号")]
        public void ReadPartNo()
        {
            PartNo = string.Empty;
            PartNo = ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0x87)).Replace(" ", "");
        }

        [Description("ReadF183-引导程序零件号")]
        public void ReadFblPartNo()
        {
            FblPartNo = string.Empty;
            FblPartNo = ReadDidViaCan(0xF1, 0x83).GetStringByAsciiBytes(false);
        }

        [Description("ReadF1A0-应用程序零件号")]
        public void ReadAppPartNo()
        {
            AppPartNo = string.Empty;
            AppPartNo = ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0xA0)).Replace(" ", "");
        }

        [Description("ReadF194-供应商软件参考号")]
        public void ReadSupplySwNo()
        {
            SupplySwNo = string.Empty;
            SupplySwNo = ReadDidViaCan(0xF1, 0x94).GetStringByAsciiBytes(false);
        }

        [Description("ReadF1A1-标定程序零件号")]
        public void ReadCaliPartNo()
        {
            CaliPartNo = string.Empty;
            CaliPartNo = ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0xA1)).Replace(" ", "");
        }

        [Description("WriteF187-总成零件号")]
        public void WritePartNo()
        {
            if (Can != null)
            {
                Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x87,
                    new List<byte> { 0x43, 0x00, 37, 0x19, 0x27, 0x00 });
            }
        }

        [Description("WriteF18C-生产序列号")]
        public void WriteSerialNo()
        {
            if (Can != null)
            {
                var debugStr = "LS22801A888888888888";

                Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x8C,
                    Encoding.ASCII.GetBytes(debugStr));
            }
        }

        private byte[] ReadDidViaCan(byte didHi, byte didLo)
        {
            if (Can == null)
                return new List<byte>().ToArray();

            lock (Can)
            {
                byte[] echoBytes;
                if (Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    return new List<byte>().ToArray();
                }

                Thread.Sleep(200);

                if (Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    return new List<byte>().ToArray();
                }

                return new List<byte>().ToArray();
            }
        }

        #endregion

        #region APP下载

        [Description("R,下载结果")]
        public string DownloadResult = string.Empty;

        [Description("R/W,APP文件路径")]
        public string AppFilePath = @"C:\Projs\2022\MIFA驻车控制器\E2MIFA\MIFA_EPMCU_APP_V2.2.05_20220413_Release.sx";

        private static readonly object FileLocker = new object();

        [Description("下载")]
        public void DownloadFile()
        {
            // DownloadResult = string.Empty;
            DownloadResult = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - 下载中";

            if (Can == null)
                DownloadResult = "NG CAN未初始化";

            var fileList = new List<string>();

            lock (FileLocker)
            {
                if (!string.IsNullOrEmpty(AppFilePath))
                    if (!File.Exists(AppFilePath))
                        DownloadResult = "NG APP文件不存在";
                    else
                        fileList.Add(AppFilePath);
            }

            if (!fileList.Any())
                DownloadResult = "NG 未指定下载文件";

            var downloadAction = new Action(() =>
            {
                if (!PreProgramming(ref DownloadResult))
                    return;

                if (
                    fileList.Select(SRecordFileHelper.GetSRecordLineData)
                        .Select(SRecordFileHelper.GetBlocks)
                        .Any(blocks => !Can.CanBusWithUds.TransferData(
                            CanDiagnosisRequestPhyCanId,
                            CanDiagnosisResponseCanId,
                            CanBus.CanType.Standard,
                            blocks, false, ref DownloadResult)))
                {
                    DownloadResult = "NG 下载Block失败：" + DownloadResult;
                    return;
                }

                if (
                    !Can.CanBusWithUds.TryStartRoutineControl(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Standard, Uds14229Helper.RoutineControl.CheckProgrammingIntegrity))
                {
                    DownloadResult = "NG 3101DFFF失败：" + DownloadResult;
                    return;
                }

                if (
                    !Can.CanBusWithUds.TryStartRoutineControl(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Standard, Uds14229Helper.RoutineControl.CheckProgrammingDependencies))
                {
                    DownloadResult = "NG 3101FF01失败：" + DownloadResult;
                    return;
                }

                if (
                   !Can.CanBusWithUds.TryStartRoutineControl(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                       CanBus.CanType.Standard, Uds14229Helper.RoutineControl.CalculateSoftwareVerificationNumber, new[] { (byte)0x01 }))
                {
                    DownloadResult = "NG 3101DFFE01失败：" + DownloadResult;
                    return;
                }

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
        }

        /// <summary>
        /// 预编程
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool PreProgramming(ref string result)
        {
            if (!Can.CanBusWithUds.TryEnterProgrammingSession(
                CanDiagnosisRequestPhyCanId,
                CanDiagnosisResponseCanId,
                CanBus.CanType.Standard))
            {
                result = "NG 进入编程模式1002失败";
                return false;
            }

            if (!SecurityAccess("5"))
            {
                result = "NG SeedKey请求失败";
                return false;
            }

            return true;
        }

        #endregion
    }
}
