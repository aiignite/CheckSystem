using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    public sealed class Ep33LightingMaster : ControllerBase
    {
        public CanBus BodyCan;
        public CanBus SubCan;

        public CanBus IsdRlCanFd1;
        public CanBus IsdRrCanFd2;
        public CanBus IsdFdCanFd3;
        public CanBus IsdRmCanFd4;
        public CanBus IsdF1CanFd5;

        public Ep33LightingMaster(string name)
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

        ~Ep33LightingMaster()
        {
            Dispose();
        }

        private bool _isInExtendedSession;
        private bool _isSleep = true;
        private readonly object _lockSend = new object();
        private readonly Thread _keepExtendedSessionThread;
        private readonly Thread _keepNetworkThread;
        private const uint BodyCanDiagnosisRequestPhyCanId = 0x6C1;
        private const uint BodyCanDiagnosisRequestFunCanId = 0x7DF;
        private const uint BodyCanDiagnosisResponseCanId = 0x6C9;
        private const uint SubCanDiagnosisRequestPhyCanId = 0x646;
        private const uint SubCanDiagnosisResponseCanId = 0x647;

        private void KeepNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                if (BodyCan == null)
                    continue;

                if (_isSleep)
                    continue;
                lock (_lockSend)
                {
                    BodyCan.SendStandardCanData(
                       0x480, new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                }
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

        private void KeepExtendedSession()
        {
            while (_keepExtendedSessionThread.IsAlive)
            {
                if (!_keepExtendedSessionThread.IsAlive)
                    break;

                Thread.Sleep(1200);

                if (BodyCan == null)
                    continue;

                if (_isSleep)
                    continue;

                if (!_isInExtendedSession)
                    continue;
                lock (_lockSend)
                    BodyCan.SendStandardCanData(BodyCanDiagnosisRequestPhyCanId, new byte[] { 0x02, 0x3e, 0x00, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA });
            }
        }

        [Description("进入正常模式")]
        public void EnterDefaultSession()
        {
            if (BodyCan == null)
                return;

            lock (_lockSend)
            {
                if (BodyCan.CanBusWithUds.TryEnterExtendedSession(
                BodyCanDiagnosisRequestPhyCanId,
                BodyCanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (BodyCan.CanBusWithUds.TryEnterExtendedSession(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId, CanBus.CanType.Standard))
                    _isInExtendedSession = true;
            }
        }

        [Description("进入拓展模式")]
        public void EnterExtendedSession()
        {
            if (BodyCan == null)
                return;

            lock (_lockSend)
            {
                if (BodyCan.CanBusWithUds.TryEnterExtendedSession(
                    BodyCanDiagnosisRequestPhyCanId, BodyCanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (BodyCan.CanBusWithUds.TryEnterExtendedSession(
                    BodyCanDiagnosisRequestPhyCanId, BodyCanDiagnosisResponseCanId, CanBus.CanType.Standard))
                    _isInExtendedSession = true;
            }
        }

        [Description("解锁SeedKey")]
        public void SecurityAccess()
        {
            if (BodyCan == null)
                return;

            lock (_lockSend)
            {
                byte[] seedBytes;
                if (!BodyCan.CanBusWithUds.TryRequestSeed(
                    BodyCanDiagnosisRequestPhyCanId, BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, 0x01, out seedBytes))
                    return;

                if (seedBytes == null || seedBytes.Length != 4)
                    return;
                var keyBytes = BodyCanCalcKey(seedBytes.ToArray(), 1).ToArray();

                BodyCan.CanBusWithUds.TrySendKey(
                    BodyCanDiagnosisRequestPhyCanId, BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, 0x02, keyBytes);
            }
        }

        [Description("R,BodyCan消息检测")]
        public string BodyCanIsHaveMsgCheckResult = string.Empty;

        [Description("R,SubCan消息检测")]
        public string SubCanIsHaveMsgCheckResult = string.Empty;

        [Description("R,反馈消息读取0x21d")]
        public string DlpCamrFitSts = string.Empty;

        [Description("R,故障DTC读取")]
        public string DiagnosticInfomation = string.Empty;

        [Description("R,故障DTC清除")]
        public string ClearDiagnosticInfomationResult = string.Empty;

        [Description("BodyCan消息检测")]
        public void ExecuteBodyCanIsHaveMsgCheck()
        {
            BodyCanIsHaveMsgCheckResult = @"NG";
            DlpCamrFitSts = string.Empty;

            if (BodyCan == null)
                return;

            var findCnaIdList = new List<uint> { 0x21D, 0x21E, 0x3B4, 0x34E, 0x34B };

            foreach (var canId in findCnaIdList)
                lock (_lockSend)
                    BodyCan.AddDoNotFilterCanId(canId);

            BodyCan.CanRecvDataPackages.Clear();
            Thread.Sleep(2550);

            var isOk = findCnaIdList.All(canId => BodyCan.CanRecvDataPackages.FindAll(f => f.CanId == canId).Any());

            if (isOk)
                BodyCanIsHaveMsgCheckResult = @"OK";

            var find21D = BodyCan.CanRecvDataPackages.FindLast(f => f.CanId == 0x21D);
            if (find21D != null)
            {
                var matrixData = new CanCommunicationMatrix.MotorolaMatrix(0x21D, 8);
                Array.Copy(find21D.CanData, 0, matrixData.MatrixData, 0, find21D.CanDataLen);
                matrixData.GetMatrixData(0, 3);

                DlpCamrFitSts = matrixData.GetMatrixData(3, 2).ToString();
            }

            foreach (var canId in findCnaIdList)
                lock (_lockSend)
                    BodyCan.RemoveDoNotFilterCanId(canId);
        }

        [Description("SubCan消息检测")]
        public void ExecuteSubCanIsHaveMsgCheck()
        {
            SubCanIsHaveMsgCheckResult = @"NG";

            if (SubCan == null)
                return;

            var findCnaIdList = new List<uint> { 0x35, 0x36, 0x5A, 0x335, 0x51, 0x52, 0x2A, 0x2B };

            foreach (var canId in findCnaIdList)
                SubCan.AddDoNotFilterCanId(canId);

            SubCan.CanRecvDataPackages.Clear();
            Thread.Sleep(4550);

            var isOk = findCnaIdList.All(canId => SubCan.CanRecvDataPackages.FindAll(f => f.CanId == canId).Any());

            if (isOk)
                SubCanIsHaveMsgCheckResult = @"OK";

            foreach (var canId in findCnaIdList)
                SubCan.RemoveDoNotFilterCanId(canId);
        }

        [Description("读取DTC")]
        public void ReadDtc()
        {
            DiagnosticInfomation = string.Empty;

            if (BodyCan == null)
                return;

            lock (_lockSend)
            {
                byte[] echo;
                if (BodyCan.CanBusWithUds.TryReadDtcInfomation(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, 0x02, 0x7F,
                    out echo))
                {
                    if (echo == null)
                        return;
                    DiagnosticInfomation = "OK ";
                    foreach (var t in echo)
                        DiagnosticInfomation += ValueHelper.GetHextStr(t);
                }
                else
                {
                    Thread.Sleep(500);
                    if (!BodyCan.CanBusWithUds.TryReadDtcInfomation(
                        BodyCanDiagnosisRequestPhyCanId,
                        BodyCanDiagnosisResponseCanId,
                        CanBus.CanType.Standard, 0x02, 0x7F,
                        out echo))
                        return;

                    if (echo == null)
                        return;
                    DiagnosticInfomation = "OK ";
                    foreach (var t in echo)
                        DiagnosticInfomation += ValueHelper.GetHextStr(t);
                }
            }
        }

        [Description("清除DTC")]
        public void ClearDtc()
        {
            ClearDiagnosticInfomationResult = @"NG";
            if (BodyCan == null)
                return;
            lock (_lockSend)
            {
                if (BodyCan == null)
                    return;

                if (BodyCan.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard))
                {
                    ClearDiagnosticInfomationResult = @"OK";
                    return;
                }
                Thread.Sleep(500);

                if (BodyCan.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard))
                {
                    ClearDiagnosticInfomationResult = @"OK";
                }
            }
        }

        [Description("禁用DTC")]
        public void DisableDtc()
        {
            if (BodyCan == null) return;
            lock (_lockSend)
            {
                BodyCan.SendStandardCanData(BodyCanDiagnosisRequestPhyCanId,
                     new byte[] { 0x04, 0x2E, 0xB0, 0x19, 0x05, 0x00, 0x00, 0x00 });
            }
        }

        [Description("R,ISC灯光秀测试结果")]
        public string IscLightShowTest;

        [Description("启动ISC灯光秀测试")]
        public void StartIscLightShowTest()
        {
            IscLightShowTest = "NG";

            if (SubCan == null)
                return;

            const uint sendCanId = (uint)0x20;
            SubCan.CanRecvDataPackages.Clear();
            var recvCanId = (uint)0x35;
            SubCan.AddDoNotFilterCanId(recvCanId);

            var sendPackage = new List<CanBus.CanDataPackage>();
            for (var i = 0; i < 10; i++)
            {
                sendPackage.Add(
                    new CanBus.CanDataPackage(sendCanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[]
                        {
                            0x00, 0x00, 0x00, 0x12,
                            0x00, 0x00, 0x00, 0x00
                        }));
            }

            for (var i = 0; i < 350; i++)
            {
                SubCan.SendCanDatas(sendPackage.ToArray());
                Thread.Sleep(20);
            }

            var findCanDataPackage =
                SubCan.CanRecvDataPackages.FindLast(f => f.CanId == recvCanId);

            if (findCanDataPackage != null &&
                findCanDataPackage.CanData != null &&
                findCanDataPackage.CanData.Length == 8)
                IscLightShowTest = ValueHelper.GetHextStrWithOx(new[] { findCanDataPackage.CanData[0] });

            SubCan.RemoveDoNotFilterCanId(recvCanId);
            SubCan.CanRecvDataPackages.Clear();
        }

        [Description("R,故障信息")]
        public string IscErrorInfo;

        [Description("读取故障信息")]
        public void ReadrIscErrorInfo()
        {
            IscErrorInfo = string.Empty;

            if (SubCan == null)
                return;

            const uint sendCanId = (uint)0x20;
            SubCan.CanRecvDataPackages.Clear();
            var recvCanId = (uint)0x35;
            SubCan.AddDoNotFilterCanId(recvCanId);

            var sendPackage = new List<CanBus.CanDataPackage>();
            for (var i = 0; i < 10; i++)
            {
                sendPackage.Add(
                    new CanBus.CanDataPackage(sendCanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[]
                        {
                            0x00, 0x00, 0x00, 0x01,
                            0x00, 0x00, 0x00, 0x00
                        }));
            }

            for (var i = 0; i < 150; i++)
            {
                SubCan.SendCanDatas(sendPackage.ToArray());
                Thread.Sleep(20);
            }

            //Thread.Sleep(2000);

            //var termPackage = new CanBus.CanDataPackage[SubCan.CanRecvDataPackages.Count];
            //Array.Copy(SubCan.CanRecvDataPackages.ToArray(), termPackage, termPackage.Length);

            //foreach (var f in termPackage)
            //{
            //    if (f.CanId != recvCanId || f.CanData == null || f.CanData.Length != 8 || f.CanData[0] != 0x02 ||
            //        f.CanData[1] != 0x00)
            //        continue;
            //    var bs = new List<byte[]>();

            //    for (var i = 0; i < 8; i = i + 2)
            //        bs.Add(
            //            new[] { f.CanData[i], f.CanData[i + 1] });

            //    IscErrorInfo = ValueHelper.GetHextStr(bs[2]) + ValueHelper.GetHextStr(bs[3]);
            //}
            var findCanDataPackage =
                SubCan.CanRecvDataPackages.FindLast(
                    f =>
                        f.CanId == recvCanId && f.CanData != null && f.CanData.Length == 8 && f.CanData[1] == 0x00 &&
                        f.CanData[2] == 0x00);

            if (findCanDataPackage != null &&
                findCanDataPackage.CanData != null &&
                findCanDataPackage.CanData.Length == 8)
            {
                //var bs = new List<byte[]>();

                //for (var i = 0; i < 8; i = i + 2)
                //    bs.Add(
                //            new[] { findCanDataPackage.CanData[i], findCanDataPackage.CanData[i + 1] });

                IscErrorInfo = ValueHelper.GetHextStr(findCanDataPackage.CanData[2]) + ValueHelper.GetHextStr(findCanDataPackage.CanData[3]);
            }

            SubCan.RemoveDoNotFilterCanId(recvCanId);
            SubCan.CanRecvDataPackages.Clear();
        }

        [Description("R,RL_CAN消息读取")]
        public string CanFd1FrameTest;

        [Description("R,RR_CAN消息读取")]
        public string CanFd2FrameTest;

        [Description("R,FD_CAN消息读取")]
        public string CanFd3FrameTest;

        [Description("R,RM_CAN消息读取")]
        public string CanFd4FrameTest;

        [Description("R,F1_CAN消息读取")]
        public string CanFd5FrameTest;

        [Description("读取CANFD消息帧")]
        public void ReadCanFdFrames()
        {
            for (var i = 1; i < 6; i++)
            {
                var field = GetType().GetField(string.Format("CanFd{0}FrameTest", i));
                field.SetValue(this, "NG");
            }

            var canFdCount = 5;

            for (var i = 1; i < canFdCount + 1; i++)
            {
                //var memberInfo = GetType().GetField("CanFd" + i);

                FieldInfo memberInfo = null;
                if (i == 1)
                    memberInfo = GetType().GetField("IsdRlCanFd1");
                else if (i == 2)
                    memberInfo = GetType().GetField("IsdRrCanFd2");
                else if (i == 3)
                    memberInfo = GetType().GetField("IsdFdCanFd3");
                else if (i == 4)
                    memberInfo = GetType().GetField("IsdRmCanFd4");
                else if (i == 5)
                    memberInfo = GetType().GetField("IsdF1CanFd5");

                if (memberInfo == null)
                    continue;
                var canFd = memberInfo.GetValue(this) as CanFdWithGateway.GatewayCanFd;
                if (canFd == null)
                    continue;

                //var lstCanIdMemberInfo = GetType().GetProperty(string.Format(@"_periodicCanFd{0}Id", i));
                //if (lstCanIdMemberInfo == null)
                //    continue;
                var listCanId = GetPeriodicCanFdId(i);
                if (listCanId == null || listCanId.Count == 0)
                    continue;

                for (var j = 0; j < 5; j++)
                {
                    canFd.SelectCan();
                    Thread.Sleep(50);
                }
                Thread.Sleep(50);

                foreach (var t in listCanId)
                    canFd.AddDoNotFilterCanId(t);
                canFd.CanRecvDataPackages.Clear();
                Thread.Sleep(5000);

                var receivedDataPackage = canFd.CanRecvDataPackages;
                var isOk = listCanId.Select(t => receivedDataPackage.FindAll(f => f.CanId == t))
                    .All(canDataPackages => canDataPackages.Any());
                //receivedDataPackage.All(t => listCanId.Contains(t.RecvCanId));

                foreach (var t in listCanId)
                    canFd.RemoveDoNotFilterCanId(t);
                canFd.CanRecvDataPackages.Clear();

                var field = GetType().GetField(string.Format("CanFd{0}FrameTest", i));
                field.SetValue(this, isOk ? "OK" : "NG");

                if (!isOk)
                    break;
            }
        }

        private static List<uint> GetPeriodicCanFdId(int index)
        {
            if (index == 1)
            {
                var temp = new List<uint> { 0xff };

                for (var i = (uint)0x300; i <= 0x305; i++)
                    temp.Add(i);

                return temp;
            }

            if (index == 2)
            {
                var temp = new List<uint> { 0xff };

                for (var i = (uint)0x500; i <= 0x505; i++)
                    temp.Add(i);

                return temp;
            }

            if (index == 3)
            {
                var temp = new List<uint> { 0xff };

                for (var i = (uint)0x140; i <= 0x145; i++)
                    temp.Add(i);
                for (var i = (uint)0x240; i <= 0x245; i++)
                    temp.Add(i);
                for (var i = (uint)0x1C0; i <= 0x1CD; i++)
                    temp.Add(i);
                for (var i = (uint)0x2C0; i <= 0x2CD; i++)
                    temp.Add(i);

                return temp;
            }

            if (index == 4)
            {
                var temp = new List<uint> { 0xff };

                for (var i = (uint)0x400; i <= 0x427; i++)
                    temp.Add(i);

                return temp;
            }

            if (index == 5)
            {
                var temp = new List<uint> { 0xFF };

                //for (var i = (uint)0x300; i <= 0x302; i++)
                //    temp.Add(i);

                for (var i = (uint)0x100; i <= 0x113; i++)
                    temp.Add(i);
                for (var i = (uint)0x200; i <= 0x213; i++)
                    temp.Add(i);
                for (var i = (uint)0x180; i <= 0x188; i++)
                    temp.Add(i);
                for (var i = (uint)0x280; i <= 0x288; i++)
                    temp.Add(i);

                return temp;
            }

            return new List<uint>();
        }

        [DllImport(@"\DllImport\PH-SAT-QC-018-2020(20200204)).dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GenerateKeyExOpt(
            byte[] ipSeedArray, // const unsigned char*  ipSeedArray,            // Array for the seed [in]
            int iSeedArraySize, // unsigned int          iSeedArraySize,         // Length of the array for the seed [in]
            int iSecurityLevel, // const unsigned int    iSecurityLevel,         // Security level [in]
            byte[] ipVariant, // const char*           ipVariant,              // Name of the active variant [in]
            byte[] ipOptions, // const char*           ipOptions,              // Optional parameter which might be used for OEM specific information [in]
            byte[] iopKeyArray, // unsigned char*        iopKeyArray,            // Array for the key [in, out]
            int iMaxKeyArraySize, // unsigned int          iMaxKeyArraySize,       // Maximum length of the array for the key [in]
            out int oActualKeyArraySize // unsigned int&         oActualKeyArraySize)    // Length of the key [out]
            );

        private static IEnumerable<byte> BodyCanCalcKey(byte[] seedBytes, int securetyLevel)
        {
            var keyBytes = new byte[4];
            int keyLen;

            GenerateKeyExOpt(
                seedBytes, seedBytes.Length,
                securetyLevel,
                null, null,
                keyBytes, keyBytes.Length, out keyLen);

            return keyBytes;
        }

        #region BobyCan读写版本号

        [Description("R,LightingMaster的零件号")]
        public string LightingMasterPartNo;

        [Description("R,LightingMaster的硬件零件号")]
        public string LightingMasterHardwarePartNo;

        [Description("R,LightingMaster的软件零件号")]
        public string LightingMasterSoftwarePartNo;

        [Description("R,LightingMaster的硬件版本号")]
        public string LightingMasterHardwareVersion;

        [Description("R,MPU的软件零件号")]
        public string MpuSoftwarePartNo;

        [Description("R,MPU的软件版本号")]
        public string MpuSoftwareVersion;

        [Description("R,系统电压")]
        public double SysVolt;

        [Description("读LightingMaster的零件号")]
        public void ReadLightingMasterPartNo()
        {
            LightingMasterPartNo = string.Empty;
            foreach (var t in ReadDidViaBodyCan(0xF1, 0x87))
                LightingMasterPartNo += ValueHelper.GetHextStr(t);
        }

        [Description("读LightingMaster的硬件零件号")]
        public void ReadLightingMasterHardwarePartNo()
        {
            LightingMasterHardwarePartNo = string.Empty;
            foreach (var t in ReadDidViaBodyCan(0xF1, 0x91))
                LightingMasterHardwarePartNo += ValueHelper.GetHextStr(t);
        }

        [Description("读LightingMaster的软件零件号")]
        public void ReadLightingMasterSoftwarePartNo()
        {
            LightingMasterSoftwarePartNo = string.Empty;
            foreach (var t in ReadDidViaBodyCan(0xF1, 0xA0))
                LightingMasterSoftwarePartNo += ValueHelper.GetHextStr(t);
        }

        [Description("读LightingMaster的硬件版本号")]
        public void ReadLightingMasterHardwareVersion()
        {
            LightingMasterHardwareVersion = string.Empty;
            LightingMasterHardwareVersion = ReadDidViaBodyCan(0xF1, 0x92).GetStringByAsciiBytes(false);
        }

        [Description("读MPU模块的软件零件号")]
        public void ReadMpuSoftwarePartNo()
        {
            MpuSoftwarePartNo = string.Empty;
            foreach (var t in ReadDidViaBodyCan(0xF1, 0xB5))
                MpuSoftwarePartNo += ValueHelper.GetHextStr(t);
        }

        [Description("读MPU模块的软件版本号")]
        public void ReadMpuSoftwareVersion()
        {
            MpuSoftwareVersion = string.Empty;
            MpuSoftwareVersion = ReadDidViaBodyCan(0xF1, 0x94).GetStringByAsciiBytes(false);
        }

        [Description("读系统电压")]
        public void ReadSysVolt()
        {
            SysVolt = -9999;
            var result = ReadDidViaBodyCan(0xB0, 0x10);
            if (result.Length == 2)
                SysVolt = result[0] * 256 + result[1];
        }

        #endregion

        #region SubCan读版本号

        [Description("R,ISC模块的软件零件号")]
        public string IscSoftwarePartNo;

        [Description("R,ISC模块的软件版本号")]
        public string IscSoftwareVersion;

        [Description("R,ISC模块的配置文件零件号")]
        public string IscConfigPartNo;

        [Description("R,ISC模块的配置文件版本号")]
        public string IscConfigVersion;

        [Description("R,Camera模块的MPU软件版本号")]
        public string CameraMpuSoftwareVersion;

        [Description("R,Camera模块的MCU软件版本号")]
        public string CameraMcuSoftwareVersion;

        [Description("检查ISC模块的软件版本号")]
        public void ReadIscVersionInfo()
        {
            IscSoftwarePartNo = string.Empty;
            IscSoftwareVersion = string.Empty;
            IscConfigPartNo = string.Empty;
            IscConfigVersion = string.Empty;

            if (SubCan == null)
                return;

            SubCan.AddDoNotFilterCanId(0x335);
            Thread.Sleep(1000);

            var findData =
                SubCan.CanRecvDataPackages.FindLast(
                f => f.CanId == 0x335 && f.CanData != null && f.CanDataLen == 8);
            if (findData == null)
                return;

            IscSoftwarePartNo =
                (findData.CanData[0] * 256 + findData.CanData[1]).ToString().PadLeft(5, '0');
            IscSoftwareVersion =
                (findData.CanData[2] * 256 + findData.CanData[3]).ToString();
            IscConfigPartNo =
                (findData.CanData[5] * 256 + findData.CanData[6]).ToString().PadLeft(5, '0');
            IscConfigVersion =
                findData.CanData[7].ToString();

            SubCan.RemoveDoNotFilterCanId(0x335);
        }

        [Description("读Camera模块的软件版本号")]
        public void ReadCameraSoftwareVersion()
        {
            CameraMpuSoftwareVersion = string.Empty;
            CameraMcuSoftwareVersion = string.Empty;

            SubCan.AddDoNotFilterCanId(0x2E0);
            SubCan.CanRecvDataPackages.Clear();
            Thread.Sleep(1000);

            var findData =
               SubCan.CanRecvDataPackages.FindLast(
               f => f.CanId == 0x2E0 && f.CanData != null && f.CanDataLen == 8);
            if (findData == null)
                return;

            CameraMpuSoftwareVersion = ValueHelper.GetHextStrWithOx(findData.CanData[4]);
            CameraMcuSoftwareVersion = ValueHelper.GetHextStrWithOx(findData.CanData[5]);
        }

        [Description("R,摄像头控制线路板总成零件号F187")]
        public string CameraPartNumber;

        [Description("R,系统供应商标识号F18A")]
        public string SystemSupplierIdentificationNo;

        [Description("R,电控单元序列号F18C")]
        public string ElectricalControllerSerialNo;

        [Description("R,系统供应商硬件版本号F193")]
        public string SystemSupplierHardwareVersion;

        [Description("R,系统供应商软件版本号F195")]
        public string SystemSupplierApplicationVersion;

        [Description("R,电控单元CAN矩阵版本F1A2")]
        public string ElectricalControllerCanMatrixVersion;

        [Description("R,CV22配套PMUpowergood_FA03")]
        public string Cv22PmuPowerGood;
        [Description("R,一级5VDCDCPowergood_FA03")]
        public string FirstGrade5VdcdcPowerGood;
        [Description("R,CV22关键电源PWR_CV_1V579_FA03")]
        public string Cv22CriticalVoltagePwrCv1V579;
        [Description("R,DEBOUNCE_GPIO_FA03")]
        public string DebounceGpio;

        [Description("R,KL30电压_FA04")]
        public double Kl30Voltage;

        [Description("R,板卡温度_高温状态_FA05")]
        public string PlateCardHighTemperatureState;
        [Description("R,板卡温度_极低温状态_FA05")]
        public string PlateCardLowTemperatureState;

        [Description("R,PWR_CVSYS_1V8电压状态_FA06")]
        public string PwrCvsys1V8;
        [Description("R,PWR_DDR_1V1电压状态_FA06")]
        public string PwrDdr1V1;
        [Description("R,PWR_CV_VDD_0V75电压状态_FA06")]
        public string PwrCvVdd0V75;
        [Description("R,PWR_CVSYS_3V3电压状态_FA06")]
        public string PwrCvsys3V3;
        [Description("R,PWR_CV_VDDA_0V8电压状态_FA06")]
        public string PwrCvVdda0V8;
        [Description("R,PMIC_VDDIO_3V 电压状态_FA06")]
        public string PmicVddio3V;
        [Description("R,PWR_DDR_1V8 电压状态_FA06")]
        public string PwrDdr1V8;
        [Description("R,PWR_eMMC_1V8 电压状态_FA06")]
        public string PwrEmmc1V8;
        [Description("R,PWR_CV_1V579 电压状态_FA06")]
        public string PwrCv1V579;
        [Description("R,ETH_VDDO_1V0 电压状态_FA06")]
        public string EthVddo1V0;

        [Description("R,前视摄像头高边开关输出电压_FA0E")]
        public double FrontCameraHighSideVolt;
        [Description("R,一级5VDCDC输出电压_FA0F")]
        public double First5VDcdcVolt;

        [Description("R,ADB-camerabuck-boost输出电压检测_FA01")]
        public double AdbCameraBuckBoostVolt;

        [Description("R,休眠唤醒信号线电压_FA02")]
        public double SignalVolt;

        [Description("R,前视摄像头视频_FA00")]
        public string FrontCameraVideo;
        [Description("R,前视摄像头buck-boostpowerGood_FA00")]
        public string FrontCameraBuckBoostPowerGood;
        [Description("R,前视摄像头高边开关状态_FA00")]
        public string FrontCameraHighSideState;
        [Description("R,DMS视频_FA00")]
        public string DmsVideo;
        [Description("R,以太网状态_FA00")]
        public string EthernetState;
        [Description("R,CV22分区检测_FA00")]
        public string Cvv2ZoneState;
        [Description("R,BoardID_FA00")]
        public string BoardId;

        [Description("R,CV22烧写信息_FB01")]
        public string Cv22ProgramState;
        [Description("R,MCU信息_FB01")]
        public string McuInfo;

        [Description("读取摄像头控制线路板总成零件号")]
        public void ReadCameraPartNumber()
        {
            CameraPartNumber = string.Empty;
            CameraPartNumber = ReadDidViaSubCan(0xF1, 0x87).GetStringByAsciiBytes(false);
        }

        [Description("读系统供应商标识号")]
        public void ReadSystemSupplierIdentificationNo()
        {
            SystemSupplierIdentificationNo = string.Empty;
            SystemSupplierIdentificationNo = ReadDidViaSubCan(0xF1, 0x8A).GetStringByAsciiBytes(false);
        }

        [Description("读电控单元序列号")]
        public void ReadElectricalControlSerialNo()
        {
            ElectricalControllerSerialNo = string.Empty;
            foreach (var t in ReadDidViaSubCan(0xFA, 0x01))
                ElectricalControllerSerialNo += ValueHelper.GetHextStr(t);
        }

        [Description("读系统供应商硬件版本号")]
        public void ReadSystemSupplierHardwareVersion()
        {
            SystemSupplierHardwareVersion = string.Empty;
            SystemSupplierHardwareVersion = ReadDidViaSubCan(0xF1, 0x93).GetStringByAsciiBytes(false);
        }

        [Description("读系统供应商软件版本号")]
        public void ReadSystemSupplierApplicationVersion()
        {
            SystemSupplierApplicationVersion = string.Empty;
            SystemSupplierApplicationVersion = ReadDidViaSubCan(0xF1, 0x95).GetStringByAsciiBytes(false);
        }

        [Description("读电控单元CAN矩阵版本")]
        public void ReadElectricalControllerCanMatrixVersion()
        {
            ElectricalControllerCanMatrixVersion = string.Empty;
            foreach (var t in ReadDidViaSubCan(0xFA, 0xA2))
                ElectricalControllerCanMatrixVersion += ValueHelper.GetHextStr(t);
        }

        [Description("读MPU信息")]
        public void ReadMpuInfo(string delayTime)
        {
            FrontCameraVideo = string.Empty;
            FrontCameraBuckBoostPowerGood = string.Empty;
            FrontCameraHighSideState = string.Empty;
            DmsVideo = string.Empty;
            EthernetState = string.Empty;
            Cvv2ZoneState = string.Empty;
            BoardId = string.Empty;

            if (SubCan == null || string.IsNullOrEmpty(delayTime))
                return;

            int delayTimeI;
            if (!int.TryParse(delayTime, out delayTimeI))
                return;

            if (delayTimeI / 1000 <= 0)
                return;

            if (!SubCan.CanBusWithUds.TryEnterExtendedSession(
                SubCanDiagnosisRequestPhyCanId,
                SubCanDiagnosisResponseCanId,
                CanBus.CanType.Standard,
                CanBus.CanProtocol.Can))
            {
                Thread.Sleep(500);
                if (!SubCan.CanBusWithUds.TryEnterExtendedSession(
                    SubCanDiagnosisRequestPhyCanId,
                    SubCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard,
                    CanBus.CanProtocol.Can))
                    return;
            }

            byte[] seedEcho;
            if (SubCan.CanBusWithUds.TryRequestSeed(
                SubCanDiagnosisRequestPhyCanId,
                SubCanDiagnosisResponseCanId,
                CanBus.CanType.Standard, 0x01, out seedEcho))
            {
                if (seedEcho != null && seedEcho.Length == 4)
                {
                    var key = SubCanCalcKey(seedEcho);

                    if (SubCan.CanBusWithUds.TrySendKey(
                        SubCanDiagnosisRequestPhyCanId,
                        SubCanDiagnosisResponseCanId,
                        CanBus.CanType.Standard, 0x02, key))
                    {
                        for (var i = 0; i < delayTimeI / 1000; i++)
                        {
                            SubCan.SendStandardCanData(
                                SubCanDiagnosisRequestPhyCanId,
                                new byte[] { 0x02, 0x3e, 0x00, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA });
                            Thread.Sleep(1000);
                        }

                        var result = ReadDidViaSubCan(0xFA, 0x00);
                        if (result.Length == 5)
                        {
                            var bitStrList = new List<string>();

                            foreach (var r in result)
                            {
                                var bitStr = new string[8];
                                for (var i = 0; i < 8; i++)
                                {
                                    var temp = Convert.ToString(r, 2).PadLeft(8, '0');
                                    bitStr[i] = temp[i].ToString();
                                }
                                Array.Reverse(bitStr);
                                bitStrList.AddRange(bitStr);
                            }

                            FrontCameraVideo = bitStrList[0];
                            FrontCameraBuckBoostPowerGood = bitStrList[1];
                            FrontCameraHighSideState = bitStrList[2];
                            DmsVideo = bitStrList[8];
                            EthernetState = bitStrList[15];
                            Cvv2ZoneState = bitStrList[24];
                            BoardId = string.Format("{0}{1}{2}", bitStrList[32], bitStrList[33], bitStrList[34]);
                        }
                    }
                }
            }
        }

        [Description("读ADB-camera-buck-boost-输出电压检测")]
        public void ReadAdbCameraBuckBoostVolt()
        {
            AdbCameraBuckBoostVolt = -9999;
            AdbCameraBuckBoostVolt = ValueHelper.GetDecimal(ReadDidViaSubCan(0xFA, 0x01));
        }

        [Description("读休眠唤醒信号线电压")]
        public void ReadSignalVolt()
        {
            SignalVolt = -9999;
            SignalVolt = ValueHelper.GetDecimal(ReadDidViaSubCan(0xFA, 0x02));
        }

        [Description("读电压状态")]
        public void ReadVoltState()
        {
            Cv22PmuPowerGood = string.Empty;
            FirstGrade5VdcdcPowerGood = string.Empty;
            Cv22CriticalVoltagePwrCv1V579 = string.Empty;
            DebounceGpio = string.Empty;

            var result = ReadDidViaSubCan(0xFA, 0x03);
            if (result.Length == 1)
            {
                var bitStrList = new List<string>();

                foreach (var r in result)
                {
                    var bitStr = new string[8];
                    for (var i = 0; i < 8; i++)
                    {
                        var temp = Convert.ToString(r, 2).PadLeft(8, '0');
                        bitStr[i] = temp[i].ToString();
                    }
                    Array.Reverse(bitStr);
                    bitStrList.AddRange(bitStr);
                }

                Cv22PmuPowerGood = bitStrList[0];
                FirstGrade5VdcdcPowerGood = bitStrList[1];
                Cv22CriticalVoltagePwrCv1V579 = bitStrList[2];
                DebounceGpio = bitStrList[3];
            }
        }

        [Description("读KL30电压")]
        public void ReadKl30Voltage()
        {
            Kl30Voltage = -9999;
            Kl30Voltage = ValueHelper.GetDecimal(ReadDidViaSubCan(0xFA, 0x04));
        }

        [Description("读板卡温度状态")]
        public void ReadPlateCardTemplateState()
        {
            PlateCardHighTemperatureState = string.Empty;
            PlateCardLowTemperatureState = string.Empty;

            var result = ReadDidViaSubCan(0xFA, 0x05);
            if (result.Length == 1)
            {
                var bitStrList = new List<string>();

                foreach (var r in result)
                {
                    var bitStr = new string[8];
                    for (var i = 0; i < 8; i++)
                    {
                        var temp = Convert.ToString(r, 2).PadLeft(8, '0');
                        bitStr[i] = temp[i].ToString();
                    }
                    Array.Reverse(bitStr);
                    bitStrList.AddRange(bitStr);
                }

                PlateCardHighTemperatureState = bitStrList[0];
                PlateCardLowTemperatureState = bitStrList[1];
            }
        }

        [Description("读取系统关键电压状态和电压")]
        public void ReadSystemInternalVolt()
        {
            PwrCvsys1V8 = string.Empty;
            PwrDdr1V1 = string.Empty;
            PwrCvVdd0V75 = string.Empty;
            PwrCvsys3V3 = string.Empty;
            PwrCvVdda0V8 = string.Empty;
            PmicVddio3V = string.Empty;
            PwrDdr1V8 = string.Empty;
            PwrEmmc1V8 = string.Empty;
            PwrCv1V579 = string.Empty;
            EthVddo1V0 = string.Empty;

            var result = ReadDidViaSubCan(0xFA, 0x06);
            if (result.Length == 2)
            {
                var bitStrList = new List<string>();

                foreach (var r in result)
                {
                    var bitStr = new string[8];
                    for (var i = 0; i < 8; i++)
                    {
                        var temp = Convert.ToString(r, 2).PadLeft(8, '0');
                        bitStr[i] = temp[i].ToString();
                    }
                    Array.Reverse(bitStr);
                    bitStrList.AddRange(bitStr);
                }

                PwrCvsys1V8 = bitStrList[0];
                PwrDdr1V1 = bitStrList[1];
                PwrCvVdd0V75 = bitStrList[2];
                PwrCvsys3V3 = bitStrList[3];
                PwrCvVdda0V8 = bitStrList[4];
                PmicVddio3V = bitStrList[5];
                PwrDdr1V8 = bitStrList[6];
                PwrEmmc1V8 = bitStrList[7];
                PwrCv1V579 = bitStrList[8];
                EthVddo1V0 = bitStrList[9];
            }
        }

        [Description("读取固件烧写信息")]
        public void ReadZoneProgramInfo()
        {
            Cv22ProgramState = string.Empty;
            McuInfo = string.Empty;

            var result = ReadDidViaSubCan(0xFB, 0x01);
            if (result.Length == 1)
            {
                var bitStrList = new List<string>();

                foreach (var r in result)
                {
                    var bitStr = new string[8];
                    for (var i = 0; i < 8; i++)
                    {
                        var temp = Convert.ToString(r, 2).PadLeft(8, '0');
                        bitStr[i] = temp[i].ToString();
                    }
                    Array.Reverse(bitStr);
                    bitStrList.AddRange(bitStr);
                }

                Cv22ProgramState = bitStrList[0];
                McuInfo = bitStrList[1];
            }
        }

        [Description("读前视摄像头高边开关输出电压")]
        public void ReadFrontCameraHighSideVolt()
        {
            FrontCameraHighSideVolt = -9999;
            FrontCameraHighSideVolt = ValueHelper.GetDecimal(ReadDidViaSubCan(0xFA, 0x0E));
        }

        [Description("一级5VDCDC输出电压")]
        public void ReadFirst5VDcdcVolt()
        {
            First5VDcdcVolt = -9999;
            First5VDcdcVolt = ValueHelper.GetDecimal(ReadDidViaSubCan(0xFA, 0x0F));
        }

        private static IEnumerable<byte> SubCanCalcKey(byte[] seedBytes)
        {
            const uint learnmask = 0x56494D53U;
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

            return BitConverter.GetBytes(securityKey).Reverse().ToArray();
        }

        #endregion

        private byte[] ReadDidViaBodyCan(byte didHi, byte didLo)
        {
            if (BodyCan == null)
                return new List<byte>().ToArray();

            lock (_lockSend)
            {
                byte[] echoBytes;
                if (BodyCan.CanBusWithUds.TryReadData(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    Thread.Sleep(250);
                    if (!BodyCan.CanBusWithUds.TryReadData(
                        BodyCanDiagnosisRequestPhyCanId,
                        BodyCanDiagnosisResponseCanId,
                        CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                        didHi, didLo, out echoBytes))
                        return new List<byte>().ToArray();
                    return echoBytes ?? new List<byte>().ToArray();
                }

                Thread.Sleep(250);
                if (!BodyCan.CanBusWithUds.TryReadData(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!BodyCan.CanBusWithUds.TryReadData(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();
                return echoBytes ?? new List<byte>().ToArray();
            }
        }

        private byte[] ReadDidViaSubCan(byte didHi, byte didLo)
        {
            if (SubCan == null)
                return new List<byte>().ToArray();

            lock (SubCan)
            {
                byte[] echoBytes;
                if (SubCan.CanBusWithUds.TryReadData(
                    SubCanDiagnosisRequestPhyCanId,
                    SubCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    Thread.Sleep(250);
                    if (!SubCan.CanBusWithUds.TryReadData(
                        SubCanDiagnosisRequestPhyCanId,
                        SubCanDiagnosisResponseCanId,
                        CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                        didHi, didLo, out echoBytes))
                        return new List<byte>().ToArray();
                    return echoBytes ?? new List<byte>().ToArray();
                }

                Thread.Sleep(250);
                if (!SubCan.CanBusWithUds.TryReadData(
                    SubCanDiagnosisRequestPhyCanId,
                    SubCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!SubCan.CanBusWithUds.TryReadData(
                    SubCanDiagnosisRequestPhyCanId,
                    SubCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!SubCan.CanBusWithUds.TryReadData(
                    SubCanDiagnosisRequestPhyCanId,
                    SubCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();
                return echoBytes ?? new List<byte>().ToArray();
            }
        }
    }
}
