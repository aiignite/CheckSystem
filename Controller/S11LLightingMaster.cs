using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    public sealed class S11LLightingMaster : ControllerBase
    {
        public CanBus BodyCan;
        public CanBus SubCan;

        public CanBus IsdRlCanFd1;
        public CanBus IsdRrCanFd2;
        public CanBus IsdFdCanFd3;
        public CanBus IsdRmCanFd4;
        public CanBus IsdF1CanFd5;

        public S11LLightingMaster(string name)
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

        ~S11LLightingMaster()
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

                    BodyCan.SendStandardCanData(
                       0x083, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

                    BodyCan.SendStandardCanData(
                       0x178, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
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
                _isInExtendedSession = false;

                if (BodyCan.CanBusWithUds.TryEnterExtendedSession(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId, CanBus.CanType.Standard))
                    return;

                Thread.Sleep(500);
                if (BodyCan.CanBusWithUds.TryEnterExtendedSession(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                }
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

        [Description("进入编程模式")]
        public void EnterProgramSession()
        {
            if (BodyCan == null)
                return;

            lock (_lockSend)
            {
                //_isInExtendedSession = false;
                if (BodyCan.CanBusWithUds.TryEnterProgrammingSession(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (BodyCan.CanBusWithUds.TryEnterProgrammingSession(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                    return;
                }
            }
        }

        [Description("解锁SeedKey")]
        public void SecurityAccess(string subfunc)
        {
            if (BodyCan == null)
                return;
            if (subfunc.Length != 4)
                return;

            byte seedSubFuc;
            byte keySubFuc;
            try
            {
                seedSubFuc = Convert.ToByte(subfunc.Substring(0, 2));
                keySubFuc = Convert.ToByte(subfunc.Substring(2, 2));
            }
            catch (Exception)
            {
                return;
            }

            lock (_lockSend)
            {
                byte[] seedBytes;
                if (!BodyCan.CanBusWithUds.TryRequestSeed(
                    BodyCanDiagnosisRequestPhyCanId, BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, seedSubFuc, out seedBytes))
                    return;

                if (seedBytes == null || seedBytes.Length != 4)
                    return;
                var keyBytes = GetKey(seedBytes.ToArray(), seedSubFuc).ToArray();
                Thread.Sleep(100);
                if (!BodyCan.CanBusWithUds.TrySendKey(
                    BodyCanDiagnosisRequestPhyCanId, BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, keySubFuc, keyBytes))
                {
                    BodyCan.CanBusWithUds.TrySendKey(
                    BodyCanDiagnosisRequestPhyCanId, BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, keySubFuc, keyBytes);
                }
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
            DlpCamrFit78Sts = string.Empty;

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

        [Description("SubCan消息检测-S11L高配")]
        public void ExecuteS11LHighSubCanIsHaveMsgCheck()
        {
            SubCanIsHaveMsgCheckResult = @"NG";

            if (SubCan == null)
                return;

            var findCnaIdList = new List<uint> { 0x35, 0x36, 0x6A, 0x5A, 0x335, 0x51, 0x52, 0x61, 0x62, 0x2A, 0x2B };
            //var findCnaIdList = new List<uint> { 0x35, 0x36, 0x5A, 0x335, 0x51, 0x2A, 0x2B };

            foreach (var canId in findCnaIdList)
                SubCan.AddDoNotFilterCanId(canId);

            SubCan.CanRecvDataPackages.Clear();
            Thread.Sleep(3500);

            var isOk = true;
            var notFindCanId = string.Empty;
            foreach (var item in findCnaIdList)
            {
                var findCanId = SubCan.CanRecvDataPackages.Find(f => f.CanId == item);
                if (findCanId == null)
                {
                    isOk = false;
                    notFindCanId += item + " ";
                    break;
                }
            }

            if (isOk)
                SubCanIsHaveMsgCheckResult = @"OK";
            else
                SubCanIsHaveMsgCheckResult += notFindCanId;

            foreach (var canId in findCnaIdList)
                SubCan.RemoveDoNotFilterCanId(canId);
        }

        [Description("SubCan消息检测-S11L低配")]
        public void ExecuteS11LLowSubCanIsHaveMsgCheck()
        {
            SubCanIsHaveMsgCheckResult = @"NG";

            if (SubCan == null)
                return;

            var findCnaIdList = new List<uint> { 0x35, 0x36, 0x6A, 0x5A, 0x335, 0x51, 0x52, 0x61, 0x62 };
            //var findCnaIdList = new List<uint> { 0x35, 0x36, 0x5A, 0x335, 0x51,};

            foreach (var canId in findCnaIdList)
                SubCan.AddDoNotFilterCanId(canId);

            SubCan.CanRecvDataPackages.Clear();
            Thread.Sleep(3500);

            var isOk = true;
            var notFindCanId = string.Empty;
            foreach (var item in findCnaIdList)
            {
                var findCanId = SubCan.CanRecvDataPackages.Find(f => f.CanId == item);
                if (findCanId == null)
                {
                    isOk = false;
                    notFindCanId += item + " ";
                    break;
                }
            }

            if (isOk)
                SubCanIsHaveMsgCheckResult = @"OK";
            else
                SubCanIsHaveMsgCheckResult += notFindCanId;

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
                    CanBus.CanType.Standard, 0x02, 0x2F,
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
                //new byte[] { 0x04, 0x2E, 0xB0, 0x19, 0x04 });
            }
        }

        [Description("关闭正常通信")]
        public void DisableCommunication()
        {
            if (BodyCan == null) return;
            lock (_lockSend)
            {
                BodyCan.SendStandardCanData(BodyCanDiagnosisRequestPhyCanId,
                     new byte[] { 0x03, 0x28, 0x03, 0x01, 0x00, 0x00, 0x00, 0x00 });
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

            Console.WriteLine("开始读取isc driver故障");

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

            var isStart = true;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (!isStart)
                        break;
                    SubCan.SendCanDatas(sendPackage.ToArray());
                    Thread.Sleep(20);
                }
            });

            Thread.Sleep(1000);

            var findCanDataPackage =
                SubCan.CanRecvDataPackages.FindLast(
                    f =>
                        f.CanId == recvCanId && f.CanData != null && f.CanData.Length == 8);

            if (findCanDataPackage != null &&
                findCanDataPackage.CanData != null &&
                findCanDataPackage.CanData.Length == 8)
            {
                // ValueHelper.GetHextStr(findCanDataPackage.CanData[1]) + ValueHelper.GetHextStr(findCanDataPackage.CanData[2])
                IscErrorInfo = string.Format("{0}{1}",
                    ValueHelper.GetHextStr(findCanDataPackage.CanData[1]),
                    ValueHelper.GetHextStr(findCanDataPackage.CanData[2]));
            }

            isStart = false;
            SubCan.RemoveDoNotFilterCanId(recvCanId);
            SubCan.CanRecvDataPackages.Clear();

            Console.WriteLine("结束读取isc driver故障");
        }

        [Description("S11L低配读取故障信息")]
        public void S11LLowReadrIscErrorInfo()
        {
            IscErrorInfo = string.Empty;

            if (SubCan == null)
                return;

            Console.WriteLine("开始读取isc driver故障");

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
                            0x10, 0x00, 0x00, 0x01,
                            0x00, 0x00, 0x00, 0x00
                        }));
            }

            var isStart = true;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (!isStart)
                        break;
                    SubCan.SendCanDatas(sendPackage.ToArray());
                    Thread.Sleep(20);
                }
            });

            Thread.Sleep(1000);

            var findCanDataPackage =
                SubCan.CanRecvDataPackages.FindLast(
                    f =>
                        f.CanId == recvCanId && f.CanData != null && f.CanData.Length == 8);

            if (findCanDataPackage != null &&
                findCanDataPackage.CanData != null &&
                findCanDataPackage.CanData.Length == 8)
            {
                // ValueHelper.GetHextStr(findCanDataPackage.CanData[1]) + ValueHelper.GetHextStr(findCanDataPackage.CanData[2])
                IscErrorInfo = string.Format("{0}{1}",
                    ValueHelper.GetHextStr(findCanDataPackage.CanData[1]),
                    ValueHelper.GetHextStr(findCanDataPackage.CanData[2]));
            }

            isStart = false;
            SubCan.RemoveDoNotFilterCanId(recvCanId);
            SubCan.CanRecvDataPackages.Clear();

            Console.WriteLine("结束读取isc driver故障");
        }

        [Description("S11L高配读取故障信息")]
        public void S11LHighReadrIscErrorInfo()
        {
            IscErrorInfo = string.Empty;

            if (SubCan == null)
                return;

            Console.WriteLine("开始读取isc driver故障");

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
                            0xAA, 0xAA, 0x00, 0x00
                        }));
            }

            var isStart = true;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (!isStart)
                        break;
                    SubCan.SendCanDatas(sendPackage.ToArray());
                    Thread.Sleep(20);
                }
            });

            Thread.Sleep(1000);

            var findCanDataPackage =
                SubCan.CanRecvDataPackages.FindLast(
                    f =>
                        f.CanId == recvCanId && f.CanData != null && f.CanData.Length == 8);

            if (findCanDataPackage != null &&
                findCanDataPackage.CanData != null &&
                findCanDataPackage.CanData.Length == 8)
            {
                // ValueHelper.GetHextStr(findCanDataPackage.CanData[1]) + ValueHelper.GetHextStr(findCanDataPackage.CanData[2])
                IscErrorInfo = string.Format("{0}{1}",
                    ValueHelper.GetHextStr(findCanDataPackage.CanData[1]),
                    ValueHelper.GetHextStr(findCanDataPackage.CanData[2]));
            }

            isStart = false;
            SubCan.RemoveDoNotFilterCanId(recvCanId);
            SubCan.CanRecvDataPackages.Clear();

            Console.WriteLine("结束读取isc driver故障");
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

        [Description("R,以太网连接测试结果")]
        public string EthernetCheckResult = string.Empty;

        [Description("R,接收测试CANRR通道第6个字节的Bit1")]
        public string RrCan0X35Byte5Bit1;

        [Description("R,接收测试CANRM通道第6个字节的Bit0")]
        public string RmCan0X35Byte5Bit0;

        [Description("R,接收测试CANRL通道第5个字节的Bit7")]
        public string RlCan0X35Byte4Bit7;

        [Description("R,接收测试CANFD通道第5个字节的Bit0")]
        public string FdCan0X35Byte4Bit0;
        [Description("R,接收测试CANFD通道第5个字节的Bit2")]
        public string FdCan0X35Byte4Bit2;
        [Description("R,接收测试CANFD通道第5个字节的Bit4")]
        public string FdCan0X35Byte4Bit4;
        [Description("R,接收测试CANFD通道第5个字节的Bit6")]
        public string FdCan0X35Byte4Bit6;

        [Description("R,接收测试CANF1通道第4个字节的Bit7")]
        public string F1Can0X35Byte3Bit7;
        [Description("R,接收测试CANF1通道第5个字节的Bit1")]
        public string F1Can0X35Byte4Bit1;
        [Description("R,接收测试CANF1通道第5个字节的Bit3")]
        public string F1Can0X35Byte4Bit3;
        [Description("R,接收测试CANF1通道第5个字节的Bit5")]
        public string F1Can0X35Byte4Bit5;

        [Description("R,MPU启动标志位")]
        public string MpuRunFlag;

        [Description("读取CANFD消息帧")]
        public void ReadCanFdFrames(string timeOutMs)
        {
            for (var i = 1; i < 6; i++)
            {
                var field = GetType().GetField(string.Format("CanFd{0}FrameTest", i));
                field.SetValue(this, "NG");
            }

            const int maxTimeOut = 10 * 1000;
            int timeOut;
            if (int.TryParse(timeOutMs, out timeOut))
            {
                if (timeOut <= 0 || timeOut > maxTimeOut)
                {
                    timeOut = maxTimeOut;
                }
            }
            else
            {
                timeOut = maxTimeOut;
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
                var canTpyeName = memberInfo.GetValue(this).GetType().Name;
                var canFd = memberInfo.GetValue(this) as CanBus;
                if (canFd == null)
                    continue;

                var listCanId = GetPeriodicCanFdId(i);
                if (listCanId == null || listCanId.Count == 0)
                    continue;

                if (canTpyeName == typeof(CanFdWithGateway.GatewayCanFd).Name)
                {
                    for (var j = 0; j < 5; j++)
                    {

                        ((CanFdWithGateway.GatewayCanFd)canFd).SelectCan();
                        Thread.Sleep(50);
                    }
                }
                Thread.Sleep(50);

                foreach (var t in listCanId)
                    canFd.AddDoNotFilterCanId(t);
                canFd.CanRecvDataPackages.Clear();

                if (i != 5)
                {
                    Thread.Sleep(timeOut);

                    var receivedDataPackage = new List<CanBus.CanDataPackage>();
                    //var receivedDataPackage = canFd.CanRecvDataPackages;
                    receivedDataPackage.AddRange(canFd.CanRecvDataPackages);
                    var isOk = listCanId.Select(t => receivedDataPackage.FindAll(f => f != null && f.CanId == t))
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
                else
                {
                    Thread.Sleep(timeOut);

                    var receivedDataPackage = new List<CanBus.CanDataPackage>();
                    if ((canFd.CanRecvDataPackages.Count > 0))
                    {
                        receivedDataPackage.Add(canFd.CanRecvDataPackages[0]);
                    }
                    //receivedDataPackage.AddRange(canFd.CanRecvDataPackages);
                    var isOk = receivedDataPackage.Any();

                    var field = GetType().GetField(string.Format("CanFd{0}FrameTest", i));
                    field.SetValue(this, isOk ? "OK" : "NG");

                    foreach (var t in listCanId)
                        canFd.RemoveDoNotFilterCanId(t);
                    canFd.CanRecvDataPackages.Clear();

                    if (!isOk)
                        break;
                }
            }
        }

        [Description("接收测试")]
        public void SendRecvTest(string timeOutMs)
        {
            F1Can0X35Byte3Bit7 = string.Empty;
            F1Can0X35Byte4Bit1 = string.Empty;
            F1Can0X35Byte4Bit3 = string.Empty;
            F1Can0X35Byte4Bit5 = string.Empty;

            FdCan0X35Byte4Bit0 = string.Empty;
            FdCan0X35Byte4Bit2 = string.Empty;
            FdCan0X35Byte4Bit4 = string.Empty;
            FdCan0X35Byte4Bit6 = string.Empty;

            RlCan0X35Byte4Bit7 = string.Empty;

            RmCan0X35Byte5Bit0 = string.Empty;
            RrCan0X35Byte5Bit1 = string.Empty;

            MpuRunFlag = string.Empty;

            if (SubCan == null)
                return;

            var maxTimeOut = 5 * 1000;
            int timeOut;
            if (int.TryParse(timeOutMs, out timeOut))
            {
                if (timeOut <= 0 || timeOut > maxTimeOut)
                {
                    timeOut = maxTimeOut;
                }
            }
            else
            {
                timeOut = maxTimeOut;
            }

            SubCan.AddDoNotFilterCanId(0x20);
            SubCan.CanRecvDataPackages.Clear();
            Thread.Sleep(250);
            var find0X20 = SubCan.CanRecvDataPackages.FindLast(f => f.CanId == 0x20);
            if (find0X20 != null && find0X20.CanData != null && find0X20.CanData.Length > 5)
            {
                MpuRunFlag = ValueHelper.GetHextStr(find0X20.CanData[3]);
            }
            SubCan.RemoveDoNotFilterCanId(0x20);

            var canFdCount = 5;

            for (var i = 1; i < canFdCount + 1; i++)
            {
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
                var canTpyeName = memberInfo.GetValue(this).GetType().Name;
                var canFd = memberInfo.GetValue(this) as CanBus;
                if (canFd == null)
                    continue;

                var listCanId = GetPeriodicCanFdId(i);
                if (listCanId == null || listCanId.Count == 0)
                    continue;

                if (canTpyeName == typeof(CanFdWithGateway.GatewayCanFd).Name)
                {
                    for (var j = 0; j < 5; j++)
                    {
                        ((CanFdWithGateway.GatewayCanFd)canFd).SelectCan();
                        Thread.Sleep(50);
                    }
                }
                Thread.Sleep(10);

                SubCan.AddDoNotFilterCanId(0x35);
                SubCan.CanRecvDataPackages.Clear();

                if (i != 5)
                {
                    var sendCanIdList = new List<uint>();
                    switch (i)
                    {
                        case 1:
                            sendCanIdList.Add(0x690);
                            break;

                        case 2:
                            sendCanIdList.Add(0x6B0);
                            break;

                        case 3:
                            sendCanIdList.Add(0x630);
                            sendCanIdList.Add(0x640);
                            sendCanIdList.Add(0x670);
                            sendCanIdList.Add(0x680);
                            break;

                        case 4:
                            sendCanIdList.Add(0x6A0);
                            break;
                    }

                    var isBreak = false;

                    var th = new Thread(() =>
                    {
                        while (true)
                        {
                            if (isBreak)
                                break;
                            Thread.Sleep(50);

                            foreach (var t in sendCanIdList)
                            {
                                canFd.SendCanDatas(new[]
                                {
                                    new CanBus.CanDataPackage(t, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard,
                                        CanBus.CanFormat.Data, new byte[64])
                                });
                            }
                        }
                    });
                    th.Start();

                    Thread.Sleep(timeOut);

                    var find0X35 = SubCan.CanRecvDataPackages.FindLast(f => f.CanId == 0x35);
                    if (find0X35 != null && find0X35.CanData != null && find0X35.CanData.Length > 6)
                    {
                        if (i == 1)
                        {
                            var findB = find0X35.CanData[4];

                            var bitStrList = new List<string>();

                            var bitStr = new string[8];
                            for (var j = 0; j < 8; j++)
                            {
                                var temp = Convert.ToString(findB, 2).PadLeft(8, '0');
                                bitStr[j] = temp[j].ToString();
                            }
                            Array.Reverse(bitStr);
                            bitStrList.AddRange(bitStr);

                            RlCan0X35Byte4Bit7 = bitStrList[7];
                        }
                        else if (i == 2)
                        {
                            var findB = find0X35.CanData[5];

                            var bitStrList = new List<string>();

                            var bitStr = new string[8];
                            for (var j = 0; j < 8; j++)
                            {
                                var temp = Convert.ToString(findB, 2).PadLeft(8, '0');
                                bitStr[j] = temp[j].ToString();
                            }
                            Array.Reverse(bitStr);
                            bitStrList.AddRange(bitStr);

                            RrCan0X35Byte5Bit1 = bitStrList[1];
                        }
                        else if (i == 3)
                        {
                            var findB = find0X35.CanData[4];

                            var bitStrList = new List<string>();

                            var bitStr = new string[8];
                            for (var j = 0; j < 8; j++)
                            {
                                var temp = Convert.ToString(findB, 2).PadLeft(8, '0');
                                bitStr[j] = temp[j].ToString();
                            }
                            Array.Reverse(bitStr);
                            bitStrList.AddRange(bitStr);

                            FdCan0X35Byte4Bit0 = bitStrList[0];
                            FdCan0X35Byte4Bit2 = bitStrList[2];
                            FdCan0X35Byte4Bit4 = bitStrList[4];
                            FdCan0X35Byte4Bit6 = bitStrList[6];
                        }
                        else if (i == 4)
                        {
                            var findB = find0X35.CanData[5];

                            var bitStrList = new List<string>();

                            var bitStr = new string[8];
                            for (var j = 0; j < 8; j++)
                            {
                                var temp = Convert.ToString(findB, 2).PadLeft(8, '0');
                                bitStr[j] = temp[j].ToString();
                            }
                            Array.Reverse(bitStr);
                            bitStrList.AddRange(bitStr);

                            RmCan0X35Byte5Bit0 = bitStrList[0];
                        }
                    }

                    isBreak = true;
                    th.Abort();
                    th.Join();

                    SubCan.RemoveDoNotFilterCanId(0x20);
                    SubCan.RemoveDoNotFilterCanId(0x35);
                    SubCan.CanRecvDataPackages.Clear();
                }
                else
                {
                    var sendCanIdList = new List<uint> { 0x610, 0x620, 0x650, 0x660 };

                    var isBreak = false;

                    var th = new Thread(() =>
                    {
                        while (true)
                        {
                            if (isBreak)
                                break;
                            Thread.Sleep(50);

                            foreach (var t in sendCanIdList)
                            {
                                canFd.SendCanDatas(new[]
                                {
                                    new CanBus.CanDataPackage(t, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                        CanBus.CanFormat.Data, new byte[8])
                                });
                                Thread.Sleep(5);
                            }
                        }
                    });
                    th.Start();

                    Thread.Sleep(timeOut);

                    var find0X35 = SubCan.CanRecvDataPackages.FindLast(f => f.CanId == 0x35);
                    if (find0X35 != null && find0X35.CanData != null && find0X35.CanData.Length > 6)
                    {
                        {
                            var findB = find0X35.CanData[3];

                            var bitStrList = new List<string>();

                            var bitStr = new string[8];
                            for (var j = 0; j < 8; j++)
                            {
                                var temp = Convert.ToString(findB, 2).PadLeft(8, '0');
                                bitStr[j] = temp[j].ToString();
                            }
                            Array.Reverse(bitStr);
                            bitStrList.AddRange(bitStr);

                            F1Can0X35Byte3Bit7 = bitStrList[7];
                        }

                        {
                            var findB = find0X35.CanData[4];

                            var bitStrList = new List<string>();

                            var bitStr = new string[8];
                            for (var j = 0; j < 8; j++)
                            {
                                var temp = Convert.ToString(findB, 2).PadLeft(8, '0');
                                bitStr[j] = temp[j].ToString();
                            }
                            Array.Reverse(bitStr);
                            bitStrList.AddRange(bitStr);

                            F1Can0X35Byte4Bit1 = bitStrList[1];
                            F1Can0X35Byte4Bit3 = bitStrList[3];
                            F1Can0X35Byte4Bit5 = bitStrList[5];
                        }
                    }

                    isBreak = true;
                    th.Abort();
                    th.Join();
                    SubCan.RemoveDoNotFilterCanId(0x20);
                    SubCan.RemoveDoNotFilterCanId(0x35);
                    SubCan.CanRecvDataPackages.Clear();
                }
            }
        }

        [Description("以太网连接测试")]
        public void ExecuteEthernetCheck()
        {
            EthernetCheckResult = @"NG";

            if (BodyCan == null)
                return;

            byte[] startEcho;
            BodyCan.CanBusWithUds.TesterTryRequest(
                BodyCanDiagnosisRequestPhyCanId,
                BodyCanDiagnosisResponseCanId,
                new byte[] { 0x31, 0x01, 0xAA, 0x03, 0x01 }, out startEcho,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, timeoutFromMilliseconds: 45 * 1000);

            byte[] resultEcho;
            BodyCan.CanBusWithUds.TesterTryRequest(
                BodyCanDiagnosisRequestPhyCanId,
                BodyCanDiagnosisResponseCanId,
                new byte[] { 0x31, 0x03, 0xAA, 0x03 }, out resultEcho,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, timeoutFromMilliseconds: 5 * 1000);

            EthernetCheckResult = string.Empty;
            if (resultEcho != null)
            {
                foreach (var t in resultEcho)
                    EthernetCheckResult += ValueHelper.GetHextStr(t);

                //EthernetCheckResult = "7103AF1400";
            }

            Console.WriteLine("{0}_以太网测试:{1}.", DateTime.Now, EthernetCheckResult);
        }

        public string LeftDlpcHsdError = string.Empty;
        public string RightDlpcHsdError = string.Empty;
        public string FpdLinkExecute = string.Empty;

        public void HsdOn(string delayMs)
        {
            LeftDlpcHsdError = string.Empty;
            RightDlpcHsdError = string.Empty;
            FpdLinkExecute = string.Empty;

            if (BodyCan == null)
                return;

            byte[] resultEcho;
            BodyCan.CanBusWithUds.TesterTryRequest(
                BodyCanDiagnosisRequestPhyCanId,
                BodyCanDiagnosisResponseCanId,
                new byte[] { 0x2f, 0xd0, 0x11, 0x03, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, out resultEcho,
               CanBus.CanType.Standard, CanBus.CanProtocol.Can, timeoutFromMilliseconds: 10 * 1000);

            if (SubCan != null)
            {
                SubCan.AddDoNotFilterCanId(0x31);
                SubCan.AddDoNotFilterCanId(0x41);
                SubCan.AddDoNotFilterCanId(0x20);
                SubCan.CanRecvDataPackages.Clear();
            }

            int ms;
            if (int.TryParse(delayMs, out ms))
                Thread.Sleep(ms);

            if (SubCan != null)
            {
                var tempCanPackages = SubCan.CanRecvDataPackages;
                var findLeft0X31 = tempCanPackages.FindLast(f => f.CanId == 0x31);
                if (findLeft0X31 != null && findLeft0X31.CanData.Length > 4)
                {
                    var bitStrList = new List<string>();

                    var bitStr = new string[8];
                    for (var i = 0; i < 8; i++)
                    {
                        var temp = Convert.ToString(findLeft0X31.CanData[3], 2).PadLeft(8, '0');
                        bitStr[i] = temp[i].ToString();
                    }
                    Array.Reverse(bitStr);
                    bitStrList.AddRange(bitStr);

                    LeftDlpcHsdError = bitStr[1];
                }

                var findLeft0X41 = tempCanPackages.FindLast(f => f.CanId == 0x41);
                if (findLeft0X41 != null && findLeft0X41.CanData.Length > 4)
                {
                    var bitStrList = new List<string>();

                    var bitStr = new string[8];
                    for (var i = 0; i < 8; i++)
                    {
                        var temp = Convert.ToString(findLeft0X41.CanData[3], 2).PadLeft(8, '0');
                        bitStr[i] = temp[i].ToString();
                    }
                    Array.Reverse(bitStr);
                    bitStrList.AddRange(bitStr);

                    RightDlpcHsdError = bitStr[1];
                }

                var find0X20 = tempCanPackages.FindLast(f => f.CanId == 0x20);
                if (find0X20 != null && find0X20.CanData.Length > 3)
                    FpdLinkExecute =
                        ValueHelper.GetHextStr(find0X20.CanData[0]) +
                        ValueHelper.GetHextStr(find0X20.CanData[1]) +
                        ValueHelper.GetHextStr(find0X20.CanData[2]);

                SubCan.RemoveDoNotFilterCanId(0x31);
                SubCan.RemoveDoNotFilterCanId(0x41);
                SubCan.RemoveDoNotFilterCanId(0x20);
            }
        }

        #region 读写追溯信息

        public string ProductionDate;
        public string SerialNo;
        public string Barcode = string.Empty;

        public void WriteProductionDate()
        {
            if (BodyCan == null)
                return;

            if (string.IsNullOrEmpty(Barcode))
                return;

            var barcode = GetBarcode(Barcode);

            if (string.IsNullOrEmpty(barcode))
                return;

            try
            {
                var result = ReadDidViaBodyCan(0xf1, 0x8B);
                var tempHex = string.Empty;
                if (result != null)
                {
                    tempHex = result.Aggregate(tempHex, (current, t) => current + ValueHelper.GetHextStr(t));

                    //if (tempHex == "303030")
                    if (true)
                    {
                        var year = barcode.Substring(9, 1);
                        var day = barcode.Substring(10, 3);

                        switch (year)
                        {
                            case "W":
                                year = "2020";
                                break;

                            case "X":
                                year = "2021";
                                break;

                            case "Y":
                                year = "2022";
                                break;

                            case "Z":
                                year = "2023";
                                break;

                            case "A":
                                year = "2024";
                                break;

                            case "B":
                                year = "2025";
                                break;

                            case "C":
                                year = "2026";
                                break;

                            default:
                                return;
                        }

                        var dt = DateTime.Parse(string.Format("{0}/01/01", year));
                        dt = dt.AddDays(int.Parse(day) - 1);

                        var month = dt.Month.ToString().PadLeft(2, '0');
                        var dayOfMonth = dt.Day.ToString().PadLeft(2, '0');

                        var sendBytesStr = string.Format("{0}{1}{2}", year.Substring(2, 2), month, dayOfMonth);
                        var sendBytes = new List<byte>();

                        for (var i = 0; i < sendBytesStr.Length; i = i + 2)
                        {
                            var temp = sendBytesStr[i].ToString() + sendBytesStr[i + 1];
                            sendBytes.Add(Convert.ToByte(temp, 16));
                        }

                        if (!BodyCan.CanBusWithUds.TryWriteData(
                            BodyCanDiagnosisRequestPhyCanId,
                            BodyCanDiagnosisResponseCanId,
                            CanBus.CanType.Standard,
                            CanBus.CanProtocol.Can,
                            0xF1, 0x8B, sendBytes))
                        {
                            Thread.Sleep(500);
                            BodyCan.CanBusWithUds.TryWriteData(
                                BodyCanDiagnosisRequestPhyCanId,
                                BodyCanDiagnosisResponseCanId,
                                CanBus.CanType.Standard,
                                CanBus.CanProtocol.Can,
                                0xF1, 0x8B, sendBytes);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void WriteSerialNo()
        {
            if (BodyCan == null)
                return;

            if (string.IsNullOrEmpty(Barcode))
                return;

            var barcode = GetBarcode(Barcode);

            if (string.IsNullOrEmpty(barcode))
                return;

            try
            {
                var result = ReadDidViaBodyCan(0xf1, 0x8C);
                var tempHex = string.Empty;
                if (result != null)
                {
                    tempHex = result.Aggregate(tempHex, (current, t) => current + ValueHelper.GetHextStr(t));

                    //if (tempHex == "30303030303030303030303030303030")
                    if (true)
                    {
                        var year = barcode.Substring(9, 1);
                        var day = barcode.Substring(10, 3);
                        var searialNo = barcode.Substring(13, 4);

                        switch (year)
                        {
                            case "W":
                                year = "2020";
                                break;

                            case "X":
                                year = "2021";
                                break;

                            case "Y":
                                year = "2022";
                                break;

                            case "Z":
                                year = "2023";
                                break;

                            case "A":
                                year = "2024";
                                break;

                            case "B":
                                year = "2025";
                                break;

                            case "C":
                                year = "2026";
                                break;

                            default:
                                return;
                        }

                        var dt = DateTime.Parse(string.Format("{0}/01/01", year));
                        dt = dt.AddDays(int.Parse(day) - 1);

                        var month = dt.Month.ToString().PadLeft(2, '0');
                        var dayOfMonth = dt.Day.ToString().PadLeft(2, '0');

                        var sendBytes =
                            Encoding.ASCII.GetBytes(string.Format("{0}{1}{2}{3}", barcode.Substring(0, 9), barcode.Substring(9, 1), barcode.Substring(10, 3), barcode.Substring(14, 3)));
                        //Encoding.ASCII.GetBytes(string.Format("LM1402QNM{0}{1}{2}", barcode.Substring(9, 1), barcode.Substring(10, 3), barcode.Substring(14, 3)));

                        if (!BodyCan.CanBusWithUds.TryWriteData(
                            BodyCanDiagnosisRequestPhyCanId,
                            BodyCanDiagnosisResponseCanId,
                            CanBus.CanType.Standard,
                            CanBus.CanProtocol.Can,
                            0xF1, 0x8C, sendBytes))
                        {
                            Thread.Sleep(500);
                            BodyCan.CanBusWithUds.TryWriteData(
                                BodyCanDiagnosisRequestPhyCanId,
                                BodyCanDiagnosisResponseCanId,
                                CanBus.CanType.Standard,
                                CanBus.CanProtocol.Can,
                                0xF1, 0x8C,
                                sendBytes);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void ReadProductionDate()
        {
            ProductionDate = string.Empty;

            if (BodyCan == null)
                return;

            var result =
                ReadDidViaBodyCan(0xf1, 0x8B);
            var tempHex = string.Empty;
            if (result != null)
                tempHex = result.Aggregate(tempHex, (current, t) => current + ValueHelper.GetHextStr(t));

            if (!string.IsNullOrEmpty(Barcode))
            {
                var barcode = GetBarcode(Barcode);

                if (!string.IsNullOrEmpty(barcode))
                {
                    var year = string.Empty;
                    var month = string.Empty;
                    var day = string.Empty;
                    var searialNo = string.Empty;

                    try
                    {
                        year = barcode.Substring(9, 1);
                        day = barcode.Substring(10, 3);
                        searialNo = barcode.Substring(13, 4);

                        switch (year)
                        {
                            case "W":
                                year = "2020";
                                break;

                            case "X":
                                year = "2021";
                                break;

                            case "Y":
                                year = "2022";
                                break;

                            case "Z":
                                year = "2023";
                                break;

                            case "A":
                                year = "2024";
                                break;

                            case "B":
                                year = "2025";
                                break;

                            case "C":
                                year = "2026";
                                break;

                            default:
                                return;
                        }

                        var dt = DateTime.Parse(string.Format("{0}/01/01", year));
                        dt = dt.AddDays(int.Parse(day) - 1);

                        month = dt.Month.ToString().PadLeft(2, '0');
                        var dayOfMonth = dt.Day.ToString().PadLeft(2, '0');

                        var sendBytesStr = string.Format("{0}{1}{2}", year.Substring(2, 2), month, dayOfMonth);

                        if (sendBytesStr == tempHex)
                            ProductionDate = "OK " + tempHex;
                        else
                            ProductionDate = "NG " + tempHex;
                    }
                    catch (Exception ex)
                    {
                        ProductionDate = "NG " + ex.Message;
                    }
                }
            }
        }

        public void ReadSerialNo()
        {
            SerialNo = string.Empty;

            if (BodyCan == null)
                return;

            var result =
                ReadDidViaBodyCan(0xf1, 0x8c);

            if (result != null)
            {
                var tempStr = Encoding.ASCII.GetString(result, 0, result.Length);

                if (!string.IsNullOrEmpty(Barcode))
                {
                    var barcode = GetBarcode(Barcode);

                    if (!string.IsNullOrEmpty(barcode))
                    {
                        var year = string.Empty;
                        var month = string.Empty;
                        var day = string.Empty;
                        var searialNo = string.Empty;

                        try
                        {
                            year = barcode.Substring(9, 1);
                            day = barcode.Substring(10, 3);
                            searialNo = barcode.Substring(13, 4);

                            switch (year)
                            {
                                case "W":
                                    year = "2020";
                                    break;

                                case "X":
                                    year = "2021";
                                    break;

                                case "Y":
                                    year = "2022";
                                    break;

                                case "Z":
                                    year = "2023";
                                    break;

                                case "A":
                                    year = "2024";
                                    break;

                                case "B":
                                    year = "2025";
                                    break;

                                case "C":
                                    year = "2026";
                                    break;

                                default:
                                    return;
                            }

                            var dt = DateTime.Parse(string.Format("{0}/01/01", year));
                            dt = dt.AddDays(int.Parse(day) - 1);

                            month = dt.Month.ToString().PadLeft(2, '0');
                            var dayOfMonth = dt.Day.ToString().PadLeft(2, '0');

                            var sendBytesStr = string.Format("{0}{1}{2}{3}", barcode.Substring(0, 9), barcode.Substring(9, 1), barcode.Substring(10, 3), barcode.Substring(14, 3));

                            if (tempStr == sendBytesStr)
                                SerialNo = "OK " + tempStr;
                            else
                                SerialNo = "NG " + tempStr;
                        }
                        catch (Exception ex)
                        {
                            SerialNo = "NG " + ex.Message;
                        }
                    }
                }
            }
        }


        [Description("R,读取物流配置")]
        public string EcuProgrammingProcessFileNumber;

        public void WriteEcuProgrammingProcessFileNumber(string value)
        {
            var bs = new List<byte>();
            if (value.Length % 2 != 0)
                return;

            try
            {
                for (var i = 0; i < value.Length; i = i + 2)
                    bs.Add(Convert.ToByte(string.Format("{0}{1}", value[i], value[i + 1]), 16));

                if (BodyCan.CanBusWithUds.TryWriteData(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard,
                    CanBus.CanProtocol.Can,
                    0xF1, 0xAA, bs.ToArray()))
                    return;
                Thread.Sleep(500);
                BodyCan.CanBusWithUds.TryWriteData(
                    BodyCanDiagnosisRequestPhyCanId,
                    BodyCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard,
                    CanBus.CanProtocol.Can,
                    0xF1, 0xAA,
                    bs.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("读取物流配置")]
        public void ReadEcuProgrammingProcessFileNumber()
        {
            EcuProgrammingProcessFileNumber = string.Empty;

            var result =
                ReadDidViaBodyCan(0xf1, 0xAA);

            if (result != null)
                EcuProgrammingProcessFileNumber = ValueHelper.GetHextStr(result);
        }

        private static string GetBarcode(string str)
        {
            var getBarcodeStr = string.Empty;

            try
            {
                var keyAndKeyindexAndLen = "LM:0:17";
                var sp = keyAndKeyindexAndLen.Split(':');
                var key = sp[0];
                var keyIndex = Convert.ToInt32(sp[1]);
                var len = Convert.ToInt32(sp[2]);

                var temp = str;
                //"000ABCP001EFG000";
                var findKeyIndex = temp.LastIndexOf(key, StringComparison.Ordinal);
                if (findKeyIndex == -1)
                    return getBarcodeStr;
                getBarcodeStr = temp.Substring(findKeyIndex - keyIndex, len);
            }
            catch (Exception exception)
            {
                // ignored
                getBarcodeStr = string.Empty;
            }

            return getBarcodeStr;
        }
        #endregion

        private static List<uint> GetPeriodicCanFdId(int index)
        {
            if (index == 1)
            {
                var temp = new List<uint> { 0xff };

                for (var i = (uint)0x300; i <= 0x301; i++)
                    temp.Add(i);

                return temp;
            }

            if (index == 2)
            {
                var temp = new List<uint> { 0xff };

                for (var i = (uint)0x500; i <= 0x501; i++)
                    temp.Add(i);

                return temp;
            }

            if (index == 3)
            {
                var temp = new List<uint> { 0xff };

                //for (var i = (uint)0x140; i <= 0x145; i++)
                //    temp.Add(i);
                //for (var i = (uint)0x240; i <= 0x245; i++)
                //    temp.Add(i);
                for (var i = (uint)0x1C0; i <= 0x1C7; i++)
                    temp.Add(i);
                for (var i = (uint)0x2C0; i <= 0x2C7; i++)
                    temp.Add(i);

                return temp;
            }

            if (index == 4)
            {
                var temp = new List<uint> { 0xff };

                for (var i = (uint)0x400; i <= 0x41C; i++)
                    temp.Add(i);

                return temp;
            }

            if (index == 5)
            {
                var temp = new List<uint> { 0xFF };

                //for (var i = (uint)0x300; i <= 0x302; i++)
                //    temp.Add(i);

                for (var i = (uint)0x100; i <= 0x110; i++)
                    temp.Add(i);
                for (var i = (uint)0x200; i <= 0x210; i++)
                    temp.Add(i);
                for (var i = (uint)0x180; i <= 0x188; i++)
                    temp.Add(i);
                for (var i = (uint)0x280; i <= 0x288; i++)
                    temp.Add(i);

                //for (var i = (uint)0x100; i <= 0x100; i++)
                //    temp.Add(i);
                //for (var i = (uint)0x200; i <= 0x200; i++)
                //    temp.Add(i);
                //for (var i = (uint)0x180; i <= 0x180; i++)
                //    temp.Add(i);
                //for (var i = (uint)0x280; i <= 0x280; i++)
                //    temp.Add(i);

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

        private static IEnumerable<byte> GetKey(byte[] seedBytes, int securetyLevel)
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
            var r = ReadDidViaBodyCan(0xF1, 0x87);
            foreach (var t in r)
                LightingMasterPartNo += ValueHelper.GetHextStr(t);
        }

        [Description("读LightingMaster的硬件零件号")]
        public void ReadLightingMasterHardwarePartNo()
        {
            LightingMasterHardwarePartNo = string.Empty;
            var r = ReadDidViaBodyCan(0xF1, 0x91);
            foreach (var t in r)
                LightingMasterHardwarePartNo += ValueHelper.GetHextStr(t);
        }


        [Description("读LightingMaster的软件零件号")]
        public void ReadLightingMasterSoftwarePartNo()
        {
            LightingMasterSoftwarePartNo = string.Empty;
            var r = ReadDidViaBodyCan(0xF1, 0xA0);
            foreach (var t in r)
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
            var r = ReadDidViaBodyCan(0xF1, 0xB5);
            foreach (var t in r)
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
        [Description("R,PMIC_VDDIO_3V电压状态_FA06")]
        public string PmicVddio3V;
        [Description("R,PWR_DDR_1V8电压状态_FA06")]
        public string PwrDdr1V8;
        [Description("R,PWR_eMMC_1V8电压状态_FA06")]
        public string PwrEmmc1V8;
        [Description("R,PWR_CV_1V579电压状态_FA06")]
        public string PwrCv1V579;
        [Description("R,ETH_VDDO_1V0电压状态_FA06")]
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
            var r = ReadDidViaSubCan(0xF1, 0x8C);
            foreach (var t in r)
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
            var r = ReadDidViaSubCan(0xF1, 0xA2);
            foreach (var t in r)
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

            var readMpuFunc = new Func<bool>(() =>
            {
                Thread.Sleep(500);
                if (!SubCan.CanBusWithUds.TryEnterExtendedSession(
                SubCanDiagnosisRequestPhyCanId,
                SubCanDiagnosisResponseCanId,
                CanBus.CanType.Standard))
                {
                    Thread.Sleep(500);
                    if (!SubCan.CanBusWithUds.TryEnterExtendedSession(
                        SubCanDiagnosisRequestPhyCanId,
                        SubCanDiagnosisResponseCanId,
                        CanBus.CanType.Standard))
                    {
                        FrontCameraVideo = "进入拓展模式失败";
                        FrontCameraBuckBoostPowerGood = "进入拓展模式失败";
                        FrontCameraHighSideState = "进入拓展模式失败";
                        DmsVideo = "进入拓展模式失败";
                        EthernetState = "进入拓展模式失败";
                        Cvv2ZoneState = "进入拓展模式失败";
                        BoardId = "进入拓展模式失败";
                        return false;
                    }
                }

                Thread.Sleep(500);
                SubCan.SendStandardCanData(SubCanDiagnosisRequestPhyCanId,
                    new byte[] { 0x03, 0x28, 0x03, 0x01, 0x00, 0x00, 0x00, 0x00 });

                Thread.Sleep(500);
                byte[] seedEcho;
                if (SubCan.CanBusWithUds.TryRequestSeed(
                    SubCanDiagnosisRequestPhyCanId,
                    SubCanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, 0x01, out seedEcho))
                {
                    if (seedEcho != null && seedEcho.Length == 4)
                    {
                        var key = SubCanCalcKey(seedEcho);

                        Thread.Sleep(500);
                        SubCan.CanBusWithUds.TrySendKey(
                                SubCanDiagnosisRequestPhyCanId,
                                SubCanDiagnosisResponseCanId,
                                CanBus.CanType.Standard, 0x02, key);

                        Thread.Sleep(500);

                        SubCan.CanBusWithUds.TryStartRoutineControl(
                            SubCanDiagnosisRequestPhyCanId,
                            SubCanDiagnosisResponseCanId,
                            CanBus.CanType.Standard, Uds14229Helper.RoutineControl.MpuMode);

                        for (var i = 0; i < delayTimeI / 1000; i++)
                        {
                            SubCan.SendStandardCanData(
                                SubCanDiagnosisRequestPhyCanId,
                                new byte[] { 0x02, 0x3e, 0x00, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA });
                            Thread.Sleep(1000);
                        }

                        Thread.Sleep(500);
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
                            EthernetState = bitStrList[16];
                            Cvv2ZoneState = bitStrList[24];
                            BoardId = string.Format("{0}{1}{2}", bitStrList[34], bitStrList[33], bitStrList[32]);

                            return true;
                        }
                        else
                        {
                            FrontCameraVideo = "读取FA00失败";
                            FrontCameraBuckBoostPowerGood = "读取FA00失败";
                            FrontCameraHighSideState = "读取FA00失败";
                            DmsVideo = "读取FA00失败";
                            EthernetState = "读取FA00失败";
                            Cvv2ZoneState = "读取FA00失败";
                            BoardId = "读取FA00失败";
                            return false;
                        }
                    }
                }
                else
                {
                    FrontCameraVideo = "请求Seed失败";
                    FrontCameraBuckBoostPowerGood = "请求Seed失败";
                    FrontCameraHighSideState = "请求Seed失败";
                    DmsVideo = "请求Seed失败";
                    EthernetState = "请求Seed失败";
                    Cvv2ZoneState = "请求Seed失败";
                    BoardId = "请求Seed失败";
                    return false;
                }

                return false;
            });

            if (readMpuFunc() == false)
            {
                //Thread.Sleep(250);
                //SubCan.CanBusWithUds.TryEnterDefaultSession(SubCanDiagnosisRequestPhyCanId,
                //    SubCanDiagnosisResponseCanId,
                //    CanBus.CanType.Standard,
                //    CanBus.CanProtocol.Can);
                //readMpuFunc();
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
                for (int i = 0; i < 5; i++)
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
                    }
                }

                return new List<byte>().ToArray();
            }


            //lock (_lockSend)
            //{
            //    byte[] echoBytes;
            //    if (BodyCan.CanBusWithUds.TryReadData(
            //        BodyCanDiagnosisRequestPhyCanId,
            //        BodyCanDiagnosisResponseCanId,
            //        CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
            //    {
            //        if (echoBytes != null)
            //            return echoBytes;

            //        Thread.Sleep(250);
            //        if (!BodyCan.CanBusWithUds.TryReadData(
            //            BodyCanDiagnosisRequestPhyCanId,
            //            BodyCanDiagnosisResponseCanId,
            //            CanBus.CanType.Standard, CanBus.CanProtocol.Can,
            //            didHi, didLo, out echoBytes))
            //            return new List<byte>().ToArray();
            //        return echoBytes ?? new List<byte>().ToArray();
            //    }

            //    Thread.Sleep(250);
            //    if (!BodyCan.CanBusWithUds.TryReadData(
            //        BodyCanDiagnosisRequestPhyCanId,
            //        BodyCanDiagnosisResponseCanId,
            //        CanBus.CanType.Standard, CanBus.CanProtocol.Can,
            //        didHi, didLo, out echoBytes))
            //        return new List<byte>().ToArray();

            //    if (echoBytes != null)
            //        return echoBytes;

            //    Thread.Sleep(250);
            //    if (!BodyCan.CanBusWithUds.TryReadData(
            //        BodyCanDiagnosisRequestPhyCanId,
            //        BodyCanDiagnosisResponseCanId,
            //        CanBus.CanType.Standard, CanBus.CanProtocol.Can,
            //        didHi, didLo, out echoBytes))
            //        return new List<byte>().ToArray();
            //    return echoBytes ?? new List<byte>().ToArray();
            //}
        }

        private byte[] ReadDidViaSubCan(byte didHi, byte didLo)
        {
            if (SubCan == null)
                return new List<byte>().ToArray();

            for (int i = 0; i < 10; i++)
            {
                byte[] echoBytes;
                if (SubCan.CanBusWithUds.TryReadData(
                   SubCanDiagnosisRequestPhyCanId,
                   SubCanDiagnosisResponseCanId,
                   CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    Thread.Sleep(500);
                }
            }

            return new List<byte>().ToArray();

            //lock (SubCan)
            //{
            //    byte[] echoBytes;
            //    if (SubCan.CanBusWithUds.TryReadData(
            //        SubCanDiagnosisRequestPhyCanId,
            //        SubCanDiagnosisResponseCanId,
            //        CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes, 1000))
            //    {
            //        if (echoBytes != null)
            //            return echoBytes;

            //        Thread.Sleep(250);
            //        if (!SubCan.CanBusWithUds.TryReadData(
            //            SubCanDiagnosisRequestPhyCanId,
            //            SubCanDiagnosisResponseCanId,
            //            CanBus.CanType.Standard, CanBus.CanProtocol.Can,
            //            didHi, didLo, out echoBytes, 1000))
            //            return new List<byte>().ToArray();
            //        return echoBytes ?? new List<byte>().ToArray();
            //    }

            //    Thread.Sleep(250);
            //    if (SubCan.CanBusWithUds.TryReadData(
            //        SubCanDiagnosisRequestPhyCanId,
            //        SubCanDiagnosisResponseCanId,
            //        CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes, 1000))
            //    {
            //        if (echoBytes != null)
            //            return echoBytes;

            //        Thread.Sleep(250);
            //        if (!SubCan.CanBusWithUds.TryReadData(
            //            SubCanDiagnosisRequestPhyCanId,
            //            SubCanDiagnosisResponseCanId,
            //            CanBus.CanType.Standard, CanBus.CanProtocol.Can,
            //            didHi, didLo, out echoBytes, 1000))
            //            return new List<byte>().ToArray();
            //        return echoBytes ?? new List<byte>().ToArray();
            //    }

            //    Thread.Sleep(250);
            //    if (!SubCan.CanBusWithUds.TryReadData(
            //        SubCanDiagnosisRequestPhyCanId,
            //        SubCanDiagnosisResponseCanId,
            //        CanBus.CanType.Standard, CanBus.CanProtocol.Can,
            //        didHi, didLo, out echoBytes, 1000))
            //        return new List<byte>().ToArray();
            //    return echoBytes ?? new List<byte>().ToArray();
            //}
        }

        public string DlpCamrFit78Sts { get; set; }
    }
}
