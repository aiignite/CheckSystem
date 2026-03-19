using CommonUtility.FileOperator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace CommonUtility.BusLoader
{
    public abstract class CanBus
    {
        public delegate void PushCanMsgEventHandle(
            string name, CanDataPackage data, OnPushCanDataType onPushCanDataType);
        public static event PushCanMsgEventHandle PushCanMsg;

        //public readonly object _canLocker = new object();

        protected CanBus(string name, int maxFrameCount = 1)
        {
            Name = name;
            MaxFrameCount = maxFrameCount;
            CanBusWithUds = new MyUds(this);
        }

        protected void RegisterSendAction(
            Action<CanDataPackage[]> sendCanPackagesAction)
        {
            _sendCanPackagesAction = sendCanPackagesAction;
        }

        public string Name { get; set; }

        public int MaxFrameCount { get; set; }

        /// <summary>
        /// UDS ISO 14229
        /// </summary>
        public readonly MyUds CanBusWithUds;

        /// <summary>
        /// CAN通道接收的消息包列表
        /// </summary>
        public readonly List<CanDataPackage> CanRecvDataPackages =
            new List<CanDataPackage>();

        /// <summary>
        /// 缓冲区最大存储时间
        /// </summary>
        private const int MaxCatchTimeMs = 1000 * 5;

        /// <summary>
        /// 不需要过滤的CAN ID列表
        /// 只接收在此列表里的CAN ID的消息
        /// </summary>
        private readonly List<uint> _doNotFilterCanIds = new List<uint>();

        /// <summary>
        /// 发送消息函数
        /// </summary>
        private Action<CanDataPackage[]> _sendCanPackagesAction;

        /// <summary>
        /// 接收过滤
        /// </summary>
        private Expression<Func<CanDataPackage, bool>> _recvFilter;

        /// <summary>
        /// 期望接收的数据包数量
        /// </summary>
        private int _expectedRecvPackageCount;

        /// <summary>
        /// 实际接收的CAN数据消息包
        /// </summary>
        private readonly List<CanDataPackage> _actualRecvCanDataPackages = new List<CanDataPackage>();

        /// <summary>
        /// 超时信号量
        /// </summary>
        private readonly EventWaitHandle _recvWaitHandle = new AutoResetEvent(false);

        /// <summary>
        /// 添加不需要过滤的CAN ID的消息
        /// 默认不接收任何CAN ID的消息
        /// </summary>
        /// <param name="canId">不需要过滤的CAN ID</param>
        public void AddDoNotFilterCanId(uint canId)
        {
            if (!_doNotFilterCanIds.Contains(canId))
                _doNotFilterCanIds.Add(canId);
        }

        /// <summary>
        /// 移除不需要过滤的CAN ID的消息
        /// </summary>
        /// <param name="canId">需要移除的CAN ID</param>
        public void RemoveDoNotFilterCanId(uint canId)
        {
            if (_doNotFilterCanIds.Contains(canId))
                _doNotFilterCanIds.Remove(canId);
        }

        /// <summary>
        /// 发送CAN消息
        /// </summary>
        /// <param name="canDataPackages"></param>
        public void SendCanDatas(CanDataPackage[] canDataPackages)
        {
            try
            {
                if (_sendCanPackagesAction == null)
                    return;
                foreach (var p in canDataPackages)
                    OnPushCanMsg(Name, p, OnPushCanDataType.Tx);
                _sendCanPackagesAction(canDataPackages);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 发送CAN标准帧
        /// </summary>
        /// <param name="canId"></param>
        /// <param name="values"></param>
        public void SendStandardCanData(
            uint canId, IEnumerable<byte> values)
        {
            SendCanDatas(
                new[] { new CanDataPackage(canId, CanProtocol.Can, CanType.Standard, CanFormat.Data, values.ToArray()) });
        }

        /// <summary>
        /// 发送CAN拓展帧
        /// </summary>
        /// <param name="canId"></param>
        /// <param name="values"></param>
        public void SendExtendedCanData(
            uint canId, IEnumerable<byte> values)
        {
            SendCanDatas(
                new[] { new CanDataPackage(canId, CanProtocol.Can, CanType.Extended, CanFormat.Data, values.ToArray()) });
        }

        /// <summary>
        /// 发送CANFD标准帧
        /// </summary>
        /// <param name="canId"></param>
        /// <param name="values"></param>
        public void SendStandardCanFdData(uint canId, IEnumerable<byte> values)
        {
            SendCanDatas(
               new[] { new CanDataPackage(canId, CanProtocol.CanFd, CanType.Standard, CanFormat.Data, values.ToArray()) });
        }

        /// <summary>
        /// 发送CANFD拓展帧
        /// </summary>
        /// <param name="canId"></param>
        /// <param name="values"></param>
        public void SendExtendedCanFdData(uint canId, IEnumerable<byte> values)
        {
            SendCanDatas(
                new[] { new CanDataPackage(canId, CanProtocol.CanFd, CanType.Extended, CanFormat.Data, values.ToArray()) });
        }

        public void ReceiveCanDatas(CanDataPackage[] canDataPackages)
        {
            if (canDataPackages == null)
                return;

            foreach (var recvPackage in canDataPackages)
            {
                var dt = DateTime.Now;

                if (_doNotFilterCanIds.Contains(recvPackage.CanId))
                {
                    OnPushCanMsg(Name, recvPackage, OnPushCanDataType.NotFilterRx);

                    lock (CanRecvDataPackages)
                    {
                        CanRecvDataPackages.RemoveAll(
                            f => ValueHelper.GetTimeSpanMs(f.DateTime, dt) > MaxCatchTimeMs);
                        CanRecvDataPackages.Add(recvPackage);
                    }

                    if (_recvFilter == null)
                        continue;
                    lock (_recvFilter)
                    {
                        if (_recvFilter.Compile().Invoke(recvPackage))
                        {
                            lock (_actualRecvCanDataPackages)
                            {
                                _actualRecvCanDataPackages.Add(recvPackage);
                                if (_actualRecvCanDataPackages.Count == _expectedRecvPackageCount)
                                    _recvWaitHandle.Set();
                            }
                        }
                    }
                }
                else
                {
                    OnPushCanMsg(Name, recvPackage, OnPushCanDataType.FilterRx);
                }
            }
        }

        //private static async void OnPushCanMsg(
        //    string name, CanDataPackage data, OnPushCanDataType onPushCanDataType)
        //{
        //    await Task.Run(() =>
        //    {
        //        //if (PushCanMsg != null && (data.CanId == 0x716 || data.CanId == 0x796))
        //        //    PushCanMsg.Invoke(name, data, onPushCanDataType);
        //        if (PushCanMsg != null)
        //            PushCanMsg.Invoke(name, data, onPushCanDataType);
        //    });
        //}

        private static void OnPushCanMsg(
           string name, CanDataPackage data, OnPushCanDataType onPushCanDataType)
        {
            if (PushCanMsg != null)
            {
                //PushCanMsg.BeginInvoke(name, data, onPushCanDataType, null, null);
                PushCanMsg.Invoke(name, data, onPushCanDataType);
            }
        }

        protected CanDataPackage[] SendAndRecvCanMsg(
            CanDataPackage[] datas,
            Expression<Func<CanDataPackage, bool>> filter,
            int expectedRecvPackageCount,
            int recvTimeOutMs = 500)
        {
            _recvWaitHandle.Reset();

            lock (_actualRecvCanDataPackages)
                _actualRecvCanDataPackages.Clear();

            if (_recvFilter == null)
            {
                _expectedRecvPackageCount = expectedRecvPackageCount;
                _recvFilter = filter;
            }
            else
            {
                lock (filter)
                {
                    _expectedRecvPackageCount = expectedRecvPackageCount;
                    _recvFilter = filter;
                }
            }

            var sendAction = new Action(() => { SendCanDatas(datas); });
            sendAction.BeginInvoke(null, null);
            var isWaitOk = _recvWaitHandle.WaitOne(recvTimeOutMs);

            if (!isWaitOk)
                return new CanDataPackage[0];

            var getResults = new CanDataPackage[expectedRecvPackageCount];
            lock (_actualRecvCanDataPackages)
            {
                Array.Copy(_actualRecvCanDataPackages.ToArray(), getResults, getResults.Length);
                _actualRecvCanDataPackages.Clear();
            }

            return getResults;
        }

        /// <summary>
        /// 根据CanId判断是否为拓展帧
        /// </summary>
        /// <param name="canId"></param>
        /// <returns></returns>
        public static bool IsExtendedCan(uint canId)
        {
            //return (canId & 0x80000000) != 0;

            return canId > 0x7FF;
        }

        public class MyUds
        {
            private readonly CanBus _canBus;
            private readonly UdsTrans _udsTrans;

            public MyUds(CanBus canBus)
            {
                _canBus = canBus;
                _udsTrans = new UdsTrans { WriteData = WriteData };
            }

            private bool WriteData(int id, byte[] dat, int dlc, out long time)
            {
                time = 1;
                _canBus.SendCanDatas(
                    new[]
                    {
                        new CanDataPackage((uint)id, CanProtocol.Can, CanType.Standard, CanFormat.Data, dat)
                    });
                return true;
            }

            public static byte[] KeepExtendedSessionBytes(byte data = 0x80, byte pendingBytes = 0x00)
            {
                return new byte[] { 0x02, 0x3e, data, pendingBytes, pendingBytes, pendingBytes, pendingBytes, pendingBytes };
            }

            /// <summary>
            /// 传输下载34、36、37
            /// </summary>
            /// <param name="reqCanId"></param>
            /// <param name="recvCanId"></param>
            /// <param name="drvFlashBlocks"></param>
            /// <param name="canType"></param>
            /// <param name="errorMsg"></param>
            /// <param name="isEraseFlash"></param>
            /// <param name="preMaxNumberOfBlockLength"></param>
            /// <param name="canProtocol"></param>
            /// <param name="pendingByte"></param>
            /// <param name="isCrcCheck"></param>
            /// <param name="is37RequireCrc">
            /// -1是仅37；
            /// 0是37 00，即不要求在肯定应答报文中返回该数据块的CRC校验值；
            /// 1是37 01，即要求在肯定应答报文中返回该数据块的CRC16校验值
            /// </param>
            /// <param name="isCrcCheckByAppSelf"></param>
            /// <returns></returns>
            public bool TransferData(
                uint reqCanId,
                uint recvCanId,
                CanType canType,
                List<List<SRecordFileHelper.SRecordLineData>> drvFlashBlocks,
                bool isEraseFlash,
                ref string errorMsg,
                int? preMaxNumberOfBlockLength = null,
                CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA, bool isCrcCheck = false, int is37RequireCrc = -1, bool isCrcCheckByAppSelf = true, uint baseAddr = 0x00000000, int deley36ms=0)
            {
                if (!drvFlashBlocks.Any())
                    return true;

                var isErrased = false;
                foreach (var block in drvFlashBlocks)
                {
                    var flashTotalBytes = new List<byte>();

                    if (block.Any())
                    {
                        var dataRecord = new List<byte>();

                        var startAddr = block[0].Address + baseAddr;
                        var dataLen = 0;

                        foreach (var t in block)
                        {
                            flashTotalBytes.AddRange(t.Data);
                            dataRecord.AddRange(t.Data);
                            dataLen += t.DataLen;
                        }

                        var routineControlOperationBytes = new List<byte> { 0x44 };
                        routineControlOperationBytes.AddRange(
                            BitConverter.GetBytes(startAddr).Reverse().ToArray());
                        routineControlOperationBytes.AddRange(
                            BitConverter.GetBytes(dataLen).Reverse().ToArray());

                        if (isEraseFlash)
                        {
                            if (!isErrased)
                            {
                                if (!_canBus.CanBusWithUds.TryStartRoutineControl(
                                    reqCanId, recvCanId, CanType.Standard,
                                    Uds14229Helper.RoutineControl.EraseMemory,
                                    routineControlOperationBytes.ToArray(), pendingByte: pendingByte))
                                {
                                    errorMsg = "擦除内存3101FF00失败";
                                    return false;
                                }

                                isErrased = true;
                            }
                        }

                        int maxNumberOfBlockLength;
                        if (
                            !_canBus.CanBusWithUds.TryRequestDownload(
                                reqCanId,
                                recvCanId,
                                CanType.Standard,
                                startAddr,
                                (uint)dataLen,
                                out maxNumberOfBlockLength, pendingByte: pendingByte))
                        {
                            errorMsg = "请求下载34失败";
                            return false;
                        }

                        //Thread.Sleep(15);

                        if (preMaxNumberOfBlockLength != null)
                            maxNumberOfBlockLength = (int)preMaxNumberOfBlockLength;

                        int blockSequenceCount;
                        if (dataRecord.Count % (maxNumberOfBlockLength - 2) == 0)
                            blockSequenceCount = dataRecord.Count / (maxNumberOfBlockLength - 2);
                        else
                            blockSequenceCount = dataRecord.Count / (maxNumberOfBlockLength - 2) + 1;

                        if (blockSequenceCount == 1 && dataRecord.Count <= 4)
                        {
                            // ignore
                        }
                        else if (blockSequenceCount == 1 && dataRecord.Count <= maxNumberOfBlockLength - 2)
                        {
                            // 一次36
                            var len = dataRecord.Count;
                            var transferRequestParameterRecord = new byte[len];
                            Array.Copy(dataRecord.ToArray(), 0, transferRequestParameterRecord, 0, len);
                            dataRecord.RemoveRange(0, len);

                            if (!_canBus.CanBusWithUds.TryTransferData(
                                reqCanId,
                                recvCanId,
                                CanType.Standard, 1, transferRequestParameterRecord, pendingByte: pendingByte))
                            {
                                errorMsg = string.Format("数据传输36失败 在第{0}次36服务", 1);
                                return false;
                            }
                        }
                        else // 多次36
                        {
                            for (var i = 1; i < blockSequenceCount + 1; i++)
                            {
                                byte[] transferRequestParameterRecord;

                                if (dataRecord.ToArray().Length >= maxNumberOfBlockLength - 2)
                                {
                                    var len = maxNumberOfBlockLength - 2;
                                    transferRequestParameterRecord = new byte[len];
                                    Array.Copy(dataRecord.ToArray(), 0, transferRequestParameterRecord, 0, len);
                                    dataRecord.RemoveRange(0, len);
                                }
                                else
                                {
                                    var len = dataRecord.Count;
                                    transferRequestParameterRecord = new byte[len];
                                    Array.Copy(dataRecord.ToArray(), 0, transferRequestParameterRecord, 0, len);
                                    dataRecord.RemoveRange(0, len);
                                }

                                if (!_canBus.CanBusWithUds.TryTransferData(
                                    reqCanId,
                                    recvCanId,
                                    CanType.Standard,
                                    (byte)i,
                                    transferRequestParameterRecord, pendingByte: pendingByte))
                                {
                                    errorMsg = string.Format("数据传输36失败 在第{0}次36服务 共{1}次36服务", i, blockSequenceCount);
                                    return false;
                                }

                                if (deley36ms>0)
                                    Thread.Sleep(deley36ms);
                            }
                        }

                        if (is37RequireCrc != -1)
                        {
                            _canBus.CanBusWithUds.TryRequestTransferExit(
                                reqCanId,
                                recvCanId,
                                CanType.Standard, pendingByte: pendingByte, isRequireCrcType: is37RequireCrc == 1);
                        }
                        else
                        {
                            _canBus.CanBusWithUds.TryRequestTransferExitWithoutRequireCrc(
                                reqCanId,
                                recvCanId,
                                CanType.Standard, pendingByte: pendingByte);
                        }

                        //Thread.Sleep(1500);

                        if (isCrcCheck)
                        {
                            if (isCrcCheckByAppSelf)
                            {
                                var flashCrc32Bytes = ValueHelper.Crc32(flashTotalBytes.ToArray()).ToArray();
                                var crc = new byte[0];
                                if (!_canBus.CanBusWithUds.TryStartRoutineControl(
                                        reqCanId, recvCanId, CanType.Standard, Uds14229Helper.RoutineControl.PreCheck,
                                        crc, pendingByte: pendingByte))
                                {
                                    errorMsg = "NG APP下载 启动内存检查例程效验CRC值31010202失败";
                                    return false;
                                }
                            }
                            else
                            {
                                var flashCrc32Bytes = ValueHelper.Crc32(flashTotalBytes.ToArray()).ToArray();
                                var crc = new[] { flashCrc32Bytes[3], flashCrc32Bytes[2], flashCrc32Bytes[1], flashCrc32Bytes[0] };
                                if (!_canBus.CanBusWithUds.TryStartRoutineControl(
                                        reqCanId, recvCanId, CanType.Standard, Uds14229Helper.RoutineControl.PreCheck,
                                        crc, pendingByte: pendingByte))
                                {
                                    errorMsg = "NG APP下载 启动内存检查例程效验CRC值31010202失败";
                                    return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }

            /// <summary>
            /// 按标识符读取数据
            /// 0x22
            /// </summary>
            public bool TryReadData(
                uint requestCanId,
                uint responseCanId,
                CanType canType,
                CanProtocol canProtocol,
                byte didHi,
                byte didLo,
                out byte[] readResultData,
                byte pendingByte = 0xAA,
                int timeoutFromMilliseconds = 500)
            {
                readResultData = null;
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.ReadDataByIdentifier,
                    didHi,
                    didLo,
                };
                byte[] echo;
                var isSuccess = TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, canProtocol,
                    timeoutFromMilliseconds: timeoutFromMilliseconds, pendingByte: pendingByte);
                if (!isSuccess)
                    return false;
                if (echo == null || echo.Length <= 3)
                    return false;

                if (!echo[1].Equals(didHi) || !echo[2].Equals(didLo))
                    return false;

                readResultData = new byte[echo.Length - 3];
                Array.Copy(echo, 3, readResultData, 0, readResultData.Length);
                return true;
            }

            /// <summary>
            /// 按标识符写数据
            /// 0x2E
            /// </summary>
            public bool TryWriteData(
                uint requestCanId,
                uint responseCanId,
                CanType canType,
                CanProtocol canProtocol,
                byte didHi,
                byte didLo,
                IEnumerable<byte> writeContent,
                byte pendingByte = 0xAA, int timeoutFromMilliseconds = 500)
            {
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.WriteDataByIdentifier,
                    didHi, didLo
                };
                sendBytes.AddRange(writeContent);
                byte[] echo;
                var isSuccess = TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo,
                    canType, canProtocol, pendingByte: pendingByte, timeoutFromMilliseconds: timeoutFromMilliseconds);

                if (!isSuccess)
                    return false;

                if (echo == null || echo.Length < 3)
                    return false;

                return echo[1].Equals(didHi) && echo[2].Equals(didLo);
            }

            /// <summary>
            /// 进入默认模式
            /// 10 01
            /// </summary>
            /// <param name="requestCanId"></param>
            /// <param name="responseCanId"></param>
            /// <param name="canType"></param>
            /// <param name="canProtocol"></param>
            /// <param name="pendingByte"></param>
            /// <returns></returns>
            public bool TryEnterDefaultSession(
                uint requestCanId,
                uint responseCanId,
                CanType canType, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA)
            {
                return TryDiagnosticSessionControl(requestCanId, responseCanId, canType, canProtocol,
                    Uds14229Helper.DiagnosticSession.Default);
            }

            /// <summary>
            /// 进入编程模式
            /// 10 02
            /// </summary>
            /// <param name="requestCanId"></param>
            /// <param name="responseCanId"></param>
            /// <param name="canType"></param>
            /// <param name="canProtocol"></param>
            /// <param name="pendingByte"></param>
            /// <param name="timeOut"></param>
            /// <returns></returns>
            public bool TryEnterProgrammingSession(
                uint requestCanId,
                uint responseCanId,
                CanType canType, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA, int timeOut = 500)
            {
                return TryDiagnosticSessionControl(requestCanId, responseCanId, canType, canProtocol,
                    Uds14229Helper.DiagnosticSession.Programming, timeOut: timeOut);
            }

            /// <summary>
            /// 进入扩展模式
            /// 10 03
            /// </summary>
            /// <param name="requestCanId"></param>
            /// <param name="responseCanId"></param>
            /// <param name="canType"></param>
            /// <param name="canProtocol"></param>
            /// <param name="pendingByte"></param>
            /// <param name="timeOut"></param>
            /// <returns></returns>
            public bool TryEnterExtendedSession(
                uint requestCanId,
                uint responseCanId,
                CanType canType, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA, int timeOut = 500)
            {
                //$3E待机握手

                //$3E服务用于向服务器指示诊断仪仍然连接在网络上，之前已经激活的诊断服务功能可以仍然保持激活状态。

                //例子：02 3E 80 00 00 00 00 00，发送一个3E服务的报文，保持非默认会话状态。80表示无需回复。

                return TryDiagnosticSessionControl(requestCanId, responseCanId, canType, canProtocol,
                    Uds14229Helper.DiagnosticSession.Extedned, pendingByte, timeOut);
            }

            /// <summary>
            /// 诊断会话控制
            /// 0x10
            /// </summary>
            /// <param name="requestCanId"></param>
            /// <param name="responseCanId"></param>
            /// <param name="canType"></param>
            /// <param name="canProtocol"></param>
            /// <param name="session"></param>
            /// <param name="pendingByte"></param>
            /// <param name="timeOut"></param>
            /// <returns></returns>
            private bool TryDiagnosticSessionControl(
                uint requestCanId,
                uint responseCanId,
                CanType canType, CanProtocol canProtocol,
                Uds14229Helper.DiagnosticSession session,
                byte pendingByte = 0xAA, int timeOut = 500)
            {
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.DiagnosticSessionControl,
                    (byte) session
                };
                byte[] echo;
                var isSuccess = TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, canProtocol, pendingByte: pendingByte, timeoutFromMilliseconds: timeOut);

                if (!isSuccess)
                    return false;

                return echo != null && echo.Length >= 2 && echo[1].Equals((byte)session);
            }

            /// <summary>
            /// 安全访问，请求Seed
            /// 0x27
            /// </summary>
            /// <param name="requestCanId"></param>
            /// <param name="responseCanId"></param>
            /// <param name="canType"></param>
            /// <param name="requesetSeedSubFunc"></param>
            /// <param name="seedBytes"></param>
            /// <param name="canProtocol"></param>
            /// <param name="pendingByte"></param>
            /// <param name="timeOut"></param>
            /// <returns></returns>
            public bool TryRequestSeed(
                uint requestCanId,
                uint responseCanId,
                CanType canType,
                byte requesetSeedSubFunc,
                out byte[] seedBytes, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA, int timeOut = 1000)
            {
                seedBytes = null;
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.SecurityAccess,
                    requesetSeedSubFunc
                };
                byte[] echo;
                var isSuccess = TesterTryRequest(
                    requestCanId, responseCanId,
                    sendBytes, out echo, canType, canProtocol,
                    pendingByte: pendingByte, timeoutFromMilliseconds: timeOut);

                if (!isSuccess)
                    return false;

                if (echo == null || echo.Length < 2 || !echo[1].Equals(requesetSeedSubFunc))
                    return false;

                seedBytes = new byte[echo.Length - 2];
                Array.Copy(echo, 2, seedBytes, 0, seedBytes.Length);
                return true;
            }

            /// <summary>
            /// 安全访问，发送Key
            /// 0x27
            /// </summary>
            /// <param name="requestCanId"></param>
            /// <param name="responseCanId"></param>
            /// <param name="canType"></param>
            /// <param name="sendKeySubunc"></param>
            /// <param name="keyBytes"></param>
            /// <param name="canProtocol"></param>
            /// <param name="pendingByte"></param>
            /// <param name="timeoutFromMilliseconds"></param>
            /// <returns></returns>
            public bool TrySendKey(
                uint requestCanId,
                uint responseCanId,
                CanType canType,
                byte sendKeySubunc,
                IEnumerable<byte> keyBytes, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA, int timeoutFromMilliseconds = 1500)
            {
                var sendBytes = new List<byte> { (byte)Uds14229Helper.ServiceType.SecurityAccess, sendKeySubunc };
                sendBytes.AddRange(keyBytes);

                byte[] echo;
                var isSuccess = TesterTryRequest(
                    requestCanId, responseCanId, sendBytes, out echo, canType, canProtocol,
                    pendingByte: pendingByte, timeoutFromMilliseconds: timeoutFromMilliseconds);

                if (!isSuccess)
                    return false;

                return echo != null && echo.Length >= 2 && echo[1].Equals(sendKeySubunc);
            }

            /// <summary>
            /// 通讯控制
            /// 0x28
            /// </summary>
            /// <param name="requestCanId"></param>
            /// <param name="responseCanId"></param>
            /// <param name="canType"></param>
            /// <param name="communicationControl"></param>
            /// <param name="canProtocol"></param>
            /// <param name="pendingByte"></param>
            /// <param name="communicationType">表明这条诊断请求要对哪种报文进行控制，长度为1个byte
            /// bit0-1,0x1=正常应用报文，0x2=网络管理报文，0x3=应用与网管报文 </param>
            /// <returns></returns>
            public bool TryCommunicationControl(
                uint requestCanId,
                uint responseCanId,
                CanType canType,
                Uds14229Helper.CommunicationControl communicationControl,
                CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA, byte communicationType = 0x03)
            {
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.CommunicationControl,
                    (byte) communicationControl,
                    communicationType // 表明这条诊断请求要对哪种报文进行控制，长度为1个byte // bit0-1,0x1=正常应用报文，0x2=网络管理报文，0x3=应用与网管报文
                };

                byte[] echo;
                var isSuccess =
                    TesterTryRequest(
                    requestCanId,
                    responseCanId,
                    sendBytes,
                    out echo,
                    canType, canProtocol, pendingByte: pendingByte);

                if (!isSuccess)
                    return false;

                return echo != null && echo.Length >= 2 && echo[1].Equals((byte)communicationControl);
            }

            /// <summary>
            /// 0x19
            /// 故障DTC读取
            /// </summary>
            public bool TryReadDtcInfomation(
                uint requestCanId,
                uint responseCanId,
                CanType canType,
                byte subFunc,
                byte dtcStatusMask,
                out byte[] readResultData, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA)
            {
                //DTC（diagnostic trouble code）：如果系统检测到了一个错误，它将存储为DTC。
                //DTC可表现为：一个显而易见的故障；通讯信号的丢失（不会使故障灯亮起）；排放相关的故障；安全相关的错误等。DTC可以揭示错误的位置和错误类型。
                //通常DTC占用3个字节，OBD II占用两个字节
                //$19拥有28个子服务（Sub - Function）。常用的子服务有02（通过DTC状态掩码读取DTC），04（读取快照信息），06（读取扩展信息），0A（读ECU支持的所有DTC数据）。
                //刚才提到，一个DTC除了它自己的3个字节，还有一个字节专门用于表达DTC的状态。这个状态字节每个位的含义可以查询ISO 14229 - 1。
                //注意，并不是所有的DTC状态都是支持的。下图是Request / Response。括号标识循环，可以读出很多DTC。

                /*
                 * sub-function = 0x02 （reportDTCByStatusMask）
                 * sub-function = 0x02用于读取符合特定条件的DTC列表，此时parameter仍然为一个byte的Mask，用于与DTC的Status进行“与”运算，而ECU返回的则是"与"运算之后结果不为0的DTC列表。
                 * 比如19 02 01这个命令的用途，就是读取所有状态为active的DTC的数量。此时ECU返回的格式应该是59 02 01 XX XX XX 01 YY YY YY 09......。返回的DTC列表中的每个条目为4个字节，前三个字节用于标识DTC，比如 XX XX XX，最后一个字节用于标识DTC状态，比如01，表示DTC是active的，09表示DTC是active且confirm的。
                 * sub-function = 0x06 （reportDTCExtDataRecordByDTCNumber）sub-function = 0x06用于读取某个DTC及其相关的环境数据，此时parameter为4个byte，前三个byte用于标识我们要读取的DTC，第四个byte用于标识要读取的环境数据的范围，UDS规定使用FF来表示读取所有的环境数据，各厂家可以要根据自己的需求定义其他的值来代表要读取的环境数据的范围。环境数据包括DTC状态，优先级，发生次数，老化计数器，时间戳，里程等，厂家还可以根据自己的需求定义一些此DTC产生时的测量数据。
                 * 比如 19 06 XX XX XX FF就表示读取 XX XX XX这个DTC的所有环境数据，ECU的返回值应该是59 06 XX XX XX AA BB CC DD.....，其中AA BB CC DD...代表的就是XX XX XX这个DTC产生时所一起存储的环境数据。sub-function = 0x0E（reportMostRecentConfirmedDTC）
                 * sub-function = 0x0E时，不需要parameter。0x0E表示，要求ECU上报最近的一条被置为confirm的DTC。我在《统一诊断服务 (Unified diagnostic services ， UDS) （三）》一文中介绍过0x86服务，sub-function = 0x0E的19服务通常被作为参数传递给86指令，要求ECU在发生DTC存储的时候进行自动上报，即19 0E这两个字节的指令被嵌入到86服务的命令中。这条命令在开发阶段会用到，比如验证某个故障路径是否生效。
                 */

                readResultData = null;

                var sendBytes = new List<byte>
                    {
                        (byte) Uds14229Helper.ServiceType.ReadDtcInfomation,
                        subFunc,
                        dtcStatusMask
                    };

                byte[] echo;
                var isSuccess = TesterTryRequest(
                    requestCanId, responseCanId, sendBytes, out echo, canType, canProtocol, pendingByte: pendingByte);

                if (!isSuccess)
                    return false;

                if (echo == null || echo.Length <= 2)
                    return false;

                if (!echo[1].Equals(subFunc))
                    return false;

                readResultData = new byte[echo.Length - 3];
                Array.Copy(echo, 3, readResultData, 0, readResultData.Length);
                return true;
            }

            /// <summary>
            /// 0x14
            /// 故障DTC清除
            /// </summary>
            /// <returns></returns>
            public bool TryClearAllGroupsDiagnosticInformation(
                uint requestCanId,
                uint responseCanId,
                CanType canType, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA)
            {
                //清除（复位）DTC格式，它可以改变DTC的状态。3个FF代表清除所有DTC
                //Request：14 + FF + FF + FF
                //Response：54
                var sendBytes = new List<byte> { (byte)Uds14229Helper.ServiceType.ClearDiagnosticInformation };
                sendBytes.AddRange(new byte[] { 0xff, 0xff, 0xff }); // 3个FF代表清除所有DTC

                byte[] echo;
                return TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, canProtocol,
                    pendingByte: pendingByte, timeoutFromMilliseconds: 2000);
            }

            /// <summary>
            /// RoutineControl
            /// 0x31
            /// </summary>
            /// <param name="requestCanId"></param>
            /// <param name="responseCanId"></param>
            /// <param name="canType"></param>
            /// <param name="routineControl"></param>
            /// <param name="operationBytes"></param>
            /// <param name="canProtocol"></param>
            /// <param name="pendingByte"></param>
            /// <param name="timeoutFromMilliseconds"></param>
            /// <returns></returns>
            public bool TryStartRoutineControl(
                uint requestCanId,
                uint responseCanId,
                CanType canType,
                Uds14229Helper.RoutineControl routineControl,
                byte[] operationBytes = null,
                CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA,
                int timeoutFromMilliseconds = 5000)
            {
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.RoutineControl,
                    0x01, // 01=start, 02=stop, 03=?
                };
                sendBytes.AddRange(BitConverter.GetBytes((ushort)routineControl).ToArray()
                    .Reverse());
                if (operationBytes != null)
                    sendBytes.AddRange(operationBytes);

                byte[] echo;
                var isSuccess = TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, canProtocol,
                    timeoutFromMilliseconds: timeoutFromMilliseconds, pendingByte: pendingByte);

                if (!isSuccess)
                    return false;

                if (echo == null || echo.Length < 4 || !echo[1].Equals(0x01))
                    return false;

                var temp = new byte[2];
                Array.Copy(echo, 2, temp, 0, 2);
                return BitConverter.ToUInt16(temp.Reverse().ToArray(), 0) == (ushort)routineControl;
            }

            /// <summary>
            /// RoutineControl
            /// 0x31
            /// </summary>
            /// <param name="requestCanId"></param>
            /// <param name="responseCanId"></param>
            /// <param name="canType"></param>
            /// <param name="routineControlHi"></param>
            /// <param name="routineControlLo"></param>
            /// <param name="operationBytes"></param>
            /// <param name="canProtocol"></param>
            /// <param name="pendingByte"></param>
            /// <param name="timeoutFromMilliseconds"></param>
            /// <returns></returns>
            public bool TryStartRoutineControl(
                uint requestCanId,
                uint responseCanId,
                CanType canType,
                byte routineControlHi, byte routineControlLo,
                byte[] operationBytes = null,
                CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA,
                int timeoutFromMilliseconds = 5000)
            {
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.RoutineControl,
                    0x01, // 01=start, 02=stop, 03=?
                };
                sendBytes.AddRange(new[] { routineControlHi, routineControlLo });
                if (operationBytes != null)
                    sendBytes.AddRange(operationBytes);

                byte[] echo;
                var isSuccess = TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, canProtocol,
                    timeoutFromMilliseconds: timeoutFromMilliseconds, pendingByte: pendingByte);

                if (!isSuccess)
                    return false;

                if (echo == null || echo.Length < 4 || !echo[1].Equals(0x01))
                    return false;

                var temp = new byte[2];
                Array.Copy(echo, 2, temp, 0, 2);
                return BitConverter.ToUInt16(temp.Reverse().ToArray(), 0) ==
                       (ushort)(routineControlHi * 256 + routineControlLo);
            }

            public bool TryStartRoutineControl(
               uint requestCanId,
               uint responseCanId,
               CanType canType,
               byte routineControlHi, byte routineControlLo,
                out byte[] echoBytes,
               byte[] operationBytes = null,
               CanProtocol canProtocol = CanProtocol.Can,
               byte pendingByte = 0xAA, int timeout = 5000)
            {
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.RoutineControl,
                    0x01, // 01=start, 02=stop, 03=?
                };
                sendBytes.AddRange(new[] { routineControlHi, routineControlLo });
                if (operationBytes != null)
                    sendBytes.AddRange(operationBytes);

                byte[] echo;
                var isSuccess = TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, canProtocol,
                    timeoutFromMilliseconds: timeout, pendingByte: pendingByte);

                if (!isSuccess)
                {
                    echoBytes = new byte[] { };
                    return false;
                }

                if (echo == null || echo.Length < 4 || !echo[1].Equals(0x01))
                {
                    echoBytes = new byte[] { };
                    return false;
                }

                var temp = new byte[2];
                Array.Copy(echo, 2, temp, 0, 2);
                var returnBytes = echo.Where((t, i) => i > 3).ToList();
                echoBytes = returnBytes.ToArray();

                return BitConverter.ToUInt16(temp.Reverse().ToArray(), 0) ==
                       (ushort)(routineControlHi * 256 + routineControlLo);
            }

            /// <summary>
            /// 请求下载
            /// 0x34
            /// </summary>
            public bool TryRequestDownload(
                uint requestCanId,
                uint responseCanId,
                CanType canType,
                uint memoryAddress,
                uint memorySize,
                out int maxNumberOfBlockLength, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA)
            {
                //maxNumberOfBlockLength = 0;
                //var temp0X34Response = new byte[] { 0x04, 0x74, 0x20, 0x0f, 0xff, 0xaa, 0xaa, 0xaa };
                //var ss = new byte[temp0X34Response[0] - 1 - 1];
                //Array.Copy(temp0X34Response, 3, ss, 0, ss.Length);
                //for (var i = 0; i < ss.Length; i++)
                //{
                //    var pow = (int)Math.Pow(256, ss.Length - i - 1);
                //    maxNumberOfBlockLength += ss[i] * pow;
                //}

                maxNumberOfBlockLength = 0;
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.RequestDownload,
                    0x00, // 1个byte的dataFormatIdentifier，这里面标识了数据格式相关的信息，比如数据是否有压缩，是否有加密，用的什么算法加密等，应该由主机厂与供应商约定好，用哪个bit来表示压缩、加密等信息
                    0x44 // 1个字节的addressAndLengthFormatIdentifier，用于指示后面两个部分所占用的字节，高4bit表示memorySize所占的字节长度，低4bit表示memoryAddress，这两个值分别设置为n和m
                };
                sendBytes.AddRange(BitConverter.GetBytes(memoryAddress).ToArray().Reverse()); // m个字节的memoryAddress，由addressAndLengthFormatIdentifier中的低4bit指示。含义是要写入数据在ECU中的逻辑地址
                sendBytes.AddRange(BitConverter.GetBytes(memorySize).ToArray().Reverse()); // n个字节的memorySize，由addressAndLengthFormatIdentifier中的高4bit指示。含义是要写入数据的字节数
                byte[] echo;
                var isSuccess = TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, pendingByte: pendingByte);

                if (!isSuccess)
                    return false;

                if (echo == null || echo.Length < 2)
                    return false;

                var ss = new byte[echo.Length - 1 - 1];
                Array.Copy(echo, 2, ss, 0, ss.Length);
                for (var i = 0; i < ss.Length; i++)
                {
                    var pow = (int)Math.Pow(256, ss.Length - i - 1);
                    maxNumberOfBlockLength += ss[i] * pow;
                }

                return true;
            }

            /// <summary>
            /// 数据传输
            /// 0x36
            /// </summary>
            public bool TryTransferData(
                uint requestCanId,
                uint responseCanId,
                CanType canType,
                byte blockSequenceCounter,
                IEnumerable<byte> transferRequestParameterRecord, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA)
            {
                var sendBytes = new List<byte> { (byte)Uds14229Helper.ServiceType.TransferData, blockSequenceCounter };
                sendBytes.AddRange(transferRequestParameterRecord.ToArray());

                byte[] echo;
                return TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, timeoutFromMilliseconds: 5000, pendingByte: pendingByte);
                //return TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, timeoutFromMilliseconds: 5000);
            }

            /// <summary>
            /// 退出下载
            /// 0x37
            /// </summary>
            public bool TryRequestTransferExit(
                uint requestCanId,
                uint responseCanId,
                CanType canType, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA, bool isRequireCrcType = false)
            {
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.RequestTransferExit,
                    isRequireCrcType ? (byte) 0x01 : (byte) 0x00
                };
                byte[] echo;
                return TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, pendingByte: pendingByte, timeoutFromMilliseconds: 2000);
            }

            public bool TryRequestTransferExitWithoutRequireCrc(
                uint requestCanId,
                uint responseCanId,
                CanType canType, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA)
            {
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.RequestTransferExit
                };
                byte[] echo;
                return TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, pendingByte: pendingByte);
            }

            /// <summary>
            /// IO Control
            /// 0x2F
            /// </summary>
            /// <param name="requestCanId"></param>
            /// <param name="responseCanId"></param>
            /// <param name="canType"></param>
            /// <param name="didHi"></param>
            /// <param name="didLo"></param>
            /// <param name="parameter"></param>
            /// <param name="dataBytes"></param>
            /// <param name="canProtocol"></param>
            /// <param name="pendingByte"></param>
            /// <returns></returns>
            public bool TryInputOutputControl(
                uint requestCanId,
                uint responseCanId,
                CanType canType,
                byte didHi,
                byte didLo,
                Uds14229Helper.InputOutputControlParameter parameter,
                IEnumerable<byte> dataBytes, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA, int timeOut = 200)
            {
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.InputOutputControlByIdentifier,
                    didHi,
                    didLo,
                    (byte) parameter
                };
                sendBytes.AddRange(dataBytes);

                byte[] echo;
                var isSuccess = TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, canProtocol, pendingByte: pendingByte, timeoutFromMilliseconds: timeOut);

                if (!isSuccess)
                    return false;

                if (echo == null || echo.Length <= 3)
                    return false;

                if (!echo[1].Equals(didHi) || !echo[2].Equals(didLo))
                    return false;

                return true;
            }

            /// <summary>
            /// DTC Control
            /// 0x85
            /// </summary>
            /// <param name="requestCanId"></param>
            /// <param name="responseCanId"></param>
            /// <param name="canType"></param>
            /// <param name="controlDtcSetting"></param>
            /// <param name="canProtocol"></param>
            /// <param name="pendingByte"></param>
            /// <returns></returns>
            public bool TryControlDtcSetting(
                uint requestCanId,
                uint responseCanId,
                CanType canType,
                Uds14229Helper.ControlDtcSetting controlDtcSetting, CanProtocol canProtocol = CanProtocol.Can,
                byte pendingByte = 0xAA)
            {
                var sendBytes = new List<byte>
                {
                    (byte) Uds14229Helper.ServiceType.ControlDtcSetting,
                    (byte) controlDtcSetting
                };

                // 第三部分是optional的，由各家自己定义，比如，可以用FF FF FF 来表示这条诊断命令针对所有的DTC

                byte[] echo;
                var isSuccess = TesterTryRequest(requestCanId, responseCanId, sendBytes, out echo, canType, canProtocol, pendingByte: pendingByte);

                if (!isSuccess)
                    return false;

                return echo != null && echo.Length >= 2 && echo[1].Equals((byte)controlDtcSetting);
            }

            #region UDS ISO 14229 请求数据打包

            /// <summary>
            /// 请求数据打包
            /// UDS ISO 14229 
            /// </summary>
            /// <param name="requestCanId">请求CANID</param>
            /// <param name="responseCanId">回复CANID</param>
            /// <param name="requestData">请求数据</param>
            /// <param name="responseData">回复的实际数据</param>
            /// <param name="canProtocol">CAN协议</param>
            /// <param name="requestCanType">CAN类型</param>
            /// <param name="canFormat">CAN数据类型</param>
            /// <param name="timeoutFromMilliseconds">超时时间</param>
            /// <param name="pendingByte"></param>
            /// <returns>是否正确响应</returns>
            public bool TesterTryRequest(
                uint requestCanId,
                uint responseCanId,
                IReadOnlyList<byte> requestData,
                out byte[] responseData,
                CanType requestCanType,
                CanProtocol canProtocol = CanProtocol.Can,
                CanFormat canFormat = CanFormat.Data,
                int timeoutFromMilliseconds = 1000,
                byte pendingByte = 0xAA)
            {
                var isContainsReqCanId = false;
                var isContainsRecvCanId = false;

                if (_canBus._doNotFilterCanIds.Contains(requestCanId))
                    isContainsReqCanId = true;
                if (_canBus._doNotFilterCanIds.Contains(responseCanId))
                    isContainsRecvCanId = true;
                _canBus.AddDoNotFilterCanId(requestCanId);
                _canBus.AddDoNotFilterCanId(responseCanId);

                responseData = new byte[0];

                var sid = requestData[0];
                var echoSid = (byte)(sid + 0x40);

                Expression<Func<CanDataPackage, bool>> getFirstFrameResponseExp = w =>
                    w.CanId == responseCanId &&
                    w.CanType == requestCanType &&
                    w.CanFormat == canFormat &&
                    (w.CanDataLen == 8 && w.CanData[0].GetByteHighOrder() == 0x00 && w.CanData[1] == echoSid ||
                     w.CanDataLen == 8 && w.CanData[0].GetByteHighOrder() == 0x01 && w.CanData[2] == echoSid);

                Expression<Func<CanDataPackage, bool>> getConsecutiveFrameResponseExp = w =>
                    w.CanId == responseCanId &&
                    w.CanType == requestCanType &&
                    w.CanFormat == canFormat &&
                    w.CanDataLen == 8 && w.CanData[0].GetByteHighOrder() == 0x02;

                Expression<Func<CanDataPackage, bool>> get0X03ResponseExp = w =>
                    w.CanId == responseCanId &&
                    w.CanType == requestCanType &&
                    w.CanFormat == canFormat &&
                    w.CanDataLen == 8 && w.CanData[0].GetByteHighOrder() == 0x03;

                //Expression<Func<CanDataPackage, bool>> getLastFrameResponseExp = w =>
                //    w.CanId == responseCanId &&
                //    w.CanType == requestCanType &&
                //    w.CanFormat == canFormat &&
                //    w.CanDataLen == 8 && w.CanData[0].GetByteHighOrder() == 0x00 && w.CanData[1] == echoSid;

                Expression<Func<CanDataPackage, bool>> getLastFrameResponseExp = w =>
                    w.CanId == responseCanId &&
                    w.CanType == requestCanType &&
                    w.CanFormat == canFormat &&
                    w.CanDataLen == 8 && w.CanData[0].GetByteHighOrder() == 0x00 && w.CanData[1] == echoSid ||
                    (w.CanDataLen == 8 && w.CanData[0].GetByteHighOrder() == 0x01 && w.CanData[2] == echoSid);

                ushort requestConsecutiveFrameCount;
                var requestFrameLen = (ushort)requestData.Count;

                if ((requestFrameLen - 6) % 7 != 0)
                    requestConsecutiveFrameCount = (ushort)((requestFrameLen - 6) / 7 + 1);
                else
                    requestConsecutiveFrameCount = (ushort)((requestFrameLen - 6) / 7);

                #region 发送单帧
                if (requestData.Count <= 7) // 发送单帧
                {
                    var covering = new List<byte> { ((byte)requestData.Count).GetByteLowOrder() };
                    covering.AddRange(requestData.ToList());
                    for (var i = 0; i < 7 - requestData.Count; i++)
                        covering.Add(pendingByte);

                    var firstFrameResponse = _canBus.SendAndRecvCanMsg(
                        new[]
                        {
                            new CanDataPackage(
                                requestCanId, canProtocol, requestCanType, canFormat, covering.ToArray())
                        },
                        getFirstFrameResponseExp, 1, timeoutFromMilliseconds);

                    if (firstFrameResponse == null || !firstFrameResponse.Any())
                    {
                        if (!isContainsReqCanId)
                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                        if (!isContainsRecvCanId)
                            _canBus.RemoveDoNotFilterCanId(responseCanId);

                        return false;
                    }

                    var echoData = firstFrameResponse[0].CanData;
                    ushort echoFrameLen;
                    ushort echoConsecutiveFrameCount;
                    if (!IsHaveConsecutiveFrame(echoData[0], echoData[1], out echoFrameLen, out echoConsecutiveFrameCount))
                    {
                        if (echoData.Length < echoFrameLen + 1)
                        {
                            if (!isContainsReqCanId)
                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                            if (!isContainsRecvCanId)
                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                            return false;
                        }

                        responseData = new byte[echoFrameLen];
                        Array.Copy(echoData, 1, responseData, 0, echoFrameLen);
                        {
                            if (!isContainsReqCanId)
                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                            if (!isContainsRecvCanId)
                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                            return true;
                        }
                    }

                    return FlowControl(
                        requestCanId, responseCanId, requestCanType, canProtocol, canFormat,
                        isContainsReqCanId, isContainsRecvCanId, pendingByte,
                        echoFrameLen, getConsecutiveFrameResponseExp,
                        echoConsecutiveFrameCount, timeoutFromMilliseconds, echoData, ref responseData);

                    //if (echoData.Length < 3 || !echoData[2].Equals(echoSid))
                    //{
                    //    if (!isContainsReqCanId)
                    //        _canBus.RemoveDoNotFilterCanId(requestCanId);
                    //    if (!isContainsRecvCanId)
                    //        _canBus.RemoveDoNotFilterCanId(responseCanId);
                    //    return false;
                    //}

                    //if (echoData.Length < 8)
                    //{
                    //    if (!isContainsReqCanId)
                    //        _canBus.RemoveDoNotFilterCanId(requestCanId);
                    //    if (!isContainsRecvCanId)
                    //        _canBus.RemoveDoNotFilterCanId(responseCanId);
                    //    return false;
                    //}

                    //var tempDataLst = new List<byte>();
                    //for (var i = 2; i < 8; i++)
                    //    tempDataLst.Add(echoData[i]);

                    ////Thread.Sleep(20);

                    //var testerFlowControlData =
                    //    new byte[] { 0x30, 0x00, 0x00, pendingByte, pendingByte, pendingByte, pendingByte, pendingByte };

                    //var recv2 = _canBus.SendAndRecvCanMsg(
                    //    new[]
                    //    {
                    //        new CanDataPackage(
                    //            requestCanId, canProtocol, requestCanType, canFormat, testerFlowControlData)
                    //    },
                    //    getConsecutiveFrameResponseExp, echoConsecutiveFrameCount, timeoutFromMilliseconds);

                    //if (recv2 == null || recv2.Length < echoConsecutiveFrameCount)
                    //{
                    //    if (!isContainsReqCanId)
                    //        _canBus.RemoveDoNotFilterCanId(requestCanId);
                    //    if (!isContainsRecvCanId)
                    //        _canBus.RemoveDoNotFilterCanId(responseCanId);
                    //    return false;
                    //}

                    //var serialNo = (byte)0x21;

                    //for (var i = 0; i < echoConsecutiveFrameCount; i++)
                    //{
                    //    var r =
                    //        recv2.FirstOrDefault(f => f.CanData[0] == serialNo);
                    //    serialNo++;
                    //    if (r != null)
                    //        for (var j = 1; j < r.CanData.Length; j++)
                    //            tempDataLst.Add(r.CanData[j]);
                    //    else
                    //    {
                    //        if (!isContainsReqCanId)
                    //            _canBus.RemoveDoNotFilterCanId(requestCanId);
                    //        if (!isContainsRecvCanId)
                    //            _canBus.RemoveDoNotFilterCanId(responseCanId);
                    //        return false;
                    //    }
                    //}

                    //if (tempDataLst.Count < echoFrameLen)
                    //{
                    //    if (!isContainsReqCanId)
                    //        _canBus.RemoveDoNotFilterCanId(requestCanId);
                    //    if (!isContainsRecvCanId)
                    //        _canBus.RemoveDoNotFilterCanId(responseCanId);
                    //    return false;
                    //}
                    //responseData = new byte[echoFrameLen];
                    //Array.Copy(tempDataLst.ToArray(), 0, responseData, 0, echoFrameLen);
                    //{
                    //    if (!isContainsReqCanId)
                    //        _canBus.RemoveDoNotFilterCanId(requestCanId);
                    //    if (!isContainsRecvCanId)
                    //        _canBus.RemoveDoNotFilterCanId(responseCanId);
                    //    return true;
                    //}
                }
                #endregion

                #region 发送多帧
                else // 发送多帧
                {
                    var tempRequest = requestData.ToList();
                    var serialNo = (byte)0x21;

                    var transferLenBytes = BitConverter.GetBytes((ushort)(requestFrameLen + 0x1000));

                    var firstFrame = new List<byte>();
                    firstFrame.AddRange(transferLenBytes.ToArray().Reverse());
                    for (var k = 0; k < 6; k++)
                        firstFrame.Add(tempRequest[k]);
                    tempRequest.RemoveRange(0, 6);

                    var pendingSendDatas = new List<byte[]>();
                    for (var j = 0; j < requestConsecutiveFrameCount; j++)
                    {
                        var consecutiveFrame = new List<byte> { serialNo };

                        if (tempRequest.Count <= 7) // 发送的最后一帧数据
                        {
                            consecutiveFrame.AddRange(tempRequest);
                            var complement = new List<byte>();
                            for (var k = 0; k < 7 - tempRequest.Count; k++)
                                complement.Add(pendingByte);
                            consecutiveFrame.AddRange(complement);
                            tempRequest.RemoveRange(0, tempRequest.Count);

                            // pending send
                            pendingSendDatas.Add(consecutiveFrame.ToArray());
                        }
                        else
                        {
                            for (var k = 0; k < 7; k++)
                                consecutiveFrame.Add(tempRequest[k]);
                            tempRequest.RemoveRange(0, 7);
                            // pending send
                            pendingSendDatas.Add(consecutiveFrame.ToArray());
                        }

                        serialNo++;
                        if (serialNo > 0x2f)
                            serialNo = 0x20;
                    }

                    // send first frame
                    var firstFrameResponse = _canBus.SendAndRecvCanMsg(
                        new[]
                        {
                            new CanDataPackage(
                                requestCanId, canProtocol, requestCanType, canFormat, firstFrame.ToArray())
                        },
                        get0X03ResponseExp, 1, timeoutFromMilliseconds);

                    if (firstFrameResponse == null || !firstFrameResponse.Any())
                    {
                        {
                            if (!isContainsReqCanId)
                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                            if (!isContainsRecvCanId)
                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                            return false;
                        }
                    }

                    var blockSize = firstFrameResponse[0].CanData[1];
                    var seperateTime = firstFrameResponse[0].CanData[2];
                    var maxFrameCount = _canBus.MaxFrameCount;

                    #region 多帧

                    if (blockSize == 0x00)
                    {
                        #region 打包后一次性发送
                        if (seperateTime == 0)
                        {
                            int pendingCount;

                            if (pendingSendDatas.Count % maxFrameCount == 0)
                            {
                                pendingCount = pendingSendDatas.Count / maxFrameCount;
                                var listCanDataPackage = new List<CanDataPackage>();
                                for (var i = 0; i < pendingCount - 1; i = i + 1)
                                {
                                    for (var j = 0; j < maxFrameCount; j++)
                                    {
                                        listCanDataPackage.Add(
                                            new CanDataPackage(
                                                requestCanId, canProtocol, requestCanType, canFormat,
                                                pendingSendDatas[j]));
                                    }
                                    pendingSendDatas.RemoveRange(0, maxFrameCount);
                                    _canBus.SendCanDatas(listCanDataPackage.ToArray());
                                    listCanDataPackage.Clear();
                                }

                                // 最后一次数据
                                var lastFrame = new List<CanDataPackage>();
                                for (var j = 0; j < maxFrameCount; j++)
                                {
                                    lastFrame.Add(
                                        new CanDataPackage(
                                            requestCanId, canProtocol, requestCanType, canFormat, pendingSendDatas[j]));
                                }

                                var recv1 = _canBus.SendAndRecvCanMsg(
                                    lastFrame.ToArray(), getLastFrameResponseExp, 1, timeoutFromMilliseconds);

                                if (recv1 == null || recv1.Length != 1)
                                {
                                    if (!isContainsReqCanId)
                                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                                    if (!isContainsRecvCanId)
                                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                                    return false;
                                }

                                ushort recvConsecutiveFrameCount;
                                ushort recvFrameLen;
                                if (IsHaveConsecutiveFrame(recv1[0].CanData[0], recv1[0].CanData[1], out recvFrameLen,
                                    out recvConsecutiveFrameCount))
                                {
                                    if (!isContainsReqCanId)
                                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                                    if (!isContainsRecvCanId)
                                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                                    return false;
                                }
                                if (recv1[0].CanData.Length < recvFrameLen + 1)
                                {
                                    if (!isContainsReqCanId)
                                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                                    if (!isContainsRecvCanId)
                                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                                    return false;
                                }

                                responseData = new byte[recvFrameLen];
                                Array.Copy(recv1[0].CanData, 1, responseData, 0, recvFrameLen);
                                {
                                    if (!isContainsReqCanId)
                                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                                    if (!isContainsRecvCanId)
                                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                                    return true;
                                }
                            }
                            else
                            {
                                pendingCount = pendingSendDatas.Count / maxFrameCount + 1;
                                var listCanDataPackage = new List<CanDataPackage>();
                                for (var i = 0; i < pendingCount - 1; i = i + 1)
                                {
                                    // 调用发送多帧函数
                                    for (var j = 0; j < maxFrameCount; j++)
                                    {
                                        listCanDataPackage.Add(
                                            new CanDataPackage(
                                                requestCanId, canProtocol, requestCanType, canFormat,
                                                pendingSendDatas[j]));
                                    }
                                    pendingSendDatas.RemoveRange(0, maxFrameCount);

                                    _canBus.SendCanDatas(listCanDataPackage.ToArray());
                                    listCanDataPackage.Clear();
                                }

                                // 最后一次数据
                                var lastFrame =
                                    pendingSendDatas.Select(t =>
                                        new CanDataPackage(
                                            requestCanId, canProtocol, requestCanType, canFormat, t)).ToList();

                                // 调用发送多帧函数
                                var recv1 = _canBus.SendAndRecvCanMsg(
                                    lastFrame.ToArray(), getLastFrameResponseExp, 1, timeoutFromMilliseconds);

                                if (recv1 == null || recv1.Length != 1)
                                {
                                    if (!isContainsReqCanId)
                                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                                    if (!isContainsRecvCanId)
                                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                                    return false;
                                }

                                ushort recvConsecutiveFrameCount;
                                ushort recvFrameLen;
                                if (IsHaveConsecutiveFrame(recv1[0].CanData[0], recv1[0].CanData[1], out recvFrameLen,
                                    out recvConsecutiveFrameCount))
                                {
                                    if (!isContainsReqCanId)
                                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                                    if (!isContainsRecvCanId)
                                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                                    return false;
                                }
                                if (recv1[0].CanData.Length < recvFrameLen + 1)
                                {
                                    if (!isContainsReqCanId)
                                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                                    if (!isContainsRecvCanId)
                                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                                    return false;
                                }

                                responseData = new byte[recvFrameLen];
                                Array.Copy(recv1[0].CanData, 1, responseData, 0, recvFrameLen);
                                {
                                    if (!isContainsReqCanId)
                                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                                    if (!isContainsRecvCanId)
                                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                                    return true;
                                }
                            }
                        }
                        #endregion

                        #region 间隔发送

                        for (var i = 0; i < pendingSendDatas.Count; i++)
                        {
                            if (i == pendingSendDatas.Count - 1)
                            {
                                // 调用发送多帧函数
                                var recv1 =
                                    _canBus.SendAndRecvCanMsg(
                                        new[]
                                        {
                                            new CanDataPackage(requestCanId, canProtocol, requestCanType,
                                                canFormat, pendingSendDatas[i])
                                        },
                                        getLastFrameResponseExp, 1, timeoutFromMilliseconds);

                                if (recv1 == null || recv1.Length != 1)
                                {
                                    if (!isContainsReqCanId)
                                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                                    if (!isContainsRecvCanId)
                                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                                    return false;
                                }

                                ushort recvConsecutiveFrameCount;
                                ushort recvFrameLen;
                                if (IsHaveConsecutiveFrame(recv1[0].CanData[0], recv1[0].CanData[1],
                                    out recvFrameLen, out recvConsecutiveFrameCount))
                                {
                                    return FlowControl(
                                        requestCanId, responseCanId, requestCanType, canProtocol, canFormat,
                                        isContainsReqCanId, isContainsRecvCanId, pendingByte,
                                        recvFrameLen, getConsecutiveFrameResponseExp,
                                        recvConsecutiveFrameCount, timeoutFromMilliseconds, recv1[0].CanData,
                                        ref responseData);

                                    //if (!isContainsReqCanId)
                                    //    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                    //if (!isContainsRecvCanId)
                                    //    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                    //return false;
                                }
                                if (recv1[0].CanData.Length < recvFrameLen + 1)
                                {
                                    if (!isContainsReqCanId)
                                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                                    if (!isContainsRecvCanId)
                                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                                    return false;
                                }

                                responseData = new byte[recvFrameLen];
                                Array.Copy(recv1[0].CanData, 1, responseData, 0, recvFrameLen);
                                {
                                    if (!isContainsReqCanId)
                                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                                    if (!isContainsRecvCanId)
                                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                                    return true;
                                }
                            }

                            _canBus.SendCanDatas(new[]
                            {
                                new CanDataPackage(
                                    requestCanId, canProtocol, requestCanType, canFormat,
                                    pendingSendDatas[i])
                            });

                            Thread.Sleep(seperateTime);
                        }

                        #endregion
                    }
                    else
                    {
                        if (pendingSendDatas.Count <= blockSize)
                        {
                            #region 一次发送

                            if (seperateTime == 0)
                            {
                                int pendingCount;

                                if (pendingSendDatas.Count % maxFrameCount == 0)
                                {
                                    pendingCount = pendingSendDatas.Count / maxFrameCount;
                                    var listCanDataPackage = new List<CanDataPackage>();
                                    for (var i = 0; i < pendingCount - 1; i = i + 1)
                                    {
                                        for (var j = 0; j < maxFrameCount; j++)
                                        {
                                            listCanDataPackage.Add(
                                                new CanDataPackage(
                                                    requestCanId, canProtocol, requestCanType, canFormat,
                                                    pendingSendDatas[j]));
                                        }
                                        pendingSendDatas.RemoveRange(0, maxFrameCount);
                                        _canBus.SendCanDatas(listCanDataPackage.ToArray());
                                        listCanDataPackage.Clear();
                                    }

                                    // 最后一次数据
                                    var lastFrame = new List<CanDataPackage>();
                                    for (var j = 0; j < maxFrameCount; j++)
                                    {
                                        lastFrame.Add(
                                            new CanDataPackage(
                                                requestCanId, canProtocol, requestCanType, canFormat,
                                                pendingSendDatas[j]));
                                    }

                                    var recv1 = _canBus.SendAndRecvCanMsg(
                                        lastFrame.ToArray(), getLastFrameResponseExp, 1, timeoutFromMilliseconds);

                                    if (recv1 == null || recv1.Length != 1)
                                    {
                                        if (!isContainsReqCanId)
                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                        if (!isContainsRecvCanId)
                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                        return false;
                                    }

                                    ushort recvConsecutiveFrameCount;
                                    ushort recvFrameLen;
                                    if (IsHaveConsecutiveFrame(recv1[0].CanData[0], recv1[0].CanData[1], out recvFrameLen,
                                        out recvConsecutiveFrameCount))
                                    {
                                        return FlowControl(
                                            requestCanId, responseCanId, requestCanType, canProtocol, canFormat,
                                            isContainsReqCanId, isContainsRecvCanId, pendingByte,
                                            recvFrameLen, getConsecutiveFrameResponseExp,
                                            recvConsecutiveFrameCount, timeoutFromMilliseconds, recv1[0].CanData,
                                            ref responseData);

                                        //if (!isContainsReqCanId)
                                        //    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                        //if (!isContainsRecvCanId)
                                        //    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                        //return false;
                                    }
                                    if (recv1[0].CanData.Length < recvFrameLen + 1)
                                    {
                                        if (!isContainsReqCanId)
                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                        if (!isContainsRecvCanId)
                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                        return false;
                                    }

                                    responseData = new byte[recvFrameLen];
                                    Array.Copy(recv1[0].CanData, 1, responseData, 0, recvFrameLen);
                                    {
                                        if (!isContainsReqCanId)
                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                        if (!isContainsRecvCanId)
                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                        return true;
                                    }
                                }
                                else
                                {
                                    pendingCount = pendingSendDatas.Count / maxFrameCount + 1;
                                    var listCanDataPackage = new List<CanDataPackage>();
                                    for (var i = 0; i < pendingCount - 1; i = i + 1)
                                    {
                                        // 调用发送多帧函数
                                        for (var j = 0; j < maxFrameCount; j++)
                                        {
                                            listCanDataPackage.Add(
                                                new CanDataPackage(
                                                    requestCanId, canProtocol, requestCanType, canFormat,
                                                    pendingSendDatas[j]));
                                        }
                                        pendingSendDatas.RemoveRange(0, maxFrameCount);

                                        _canBus.SendCanDatas(listCanDataPackage.ToArray());
                                        listCanDataPackage.Clear();
                                    }

                                    // 最后一次数据
                                    var lastFrame =
                                        pendingSendDatas.Select(t =>
                                            new CanDataPackage(
                                                requestCanId, canProtocol, requestCanType, canFormat, t)).ToList();

                                    // 调用发送多帧函数
                                    var recv1 = _canBus.SendAndRecvCanMsg(
                                        lastFrame.ToArray(), getLastFrameResponseExp, 1, timeoutFromMilliseconds);

                                    if (recv1 == null || recv1.Length != 1)
                                    {
                                        if (!isContainsReqCanId)
                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                        if (!isContainsRecvCanId)
                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                        return false;
                                    }

                                    ushort recvConsecutiveFrameCount;
                                    ushort recvFrameLen;
                                    if (IsHaveConsecutiveFrame(recv1[0].CanData[0], recv1[0].CanData[1], out recvFrameLen,
                                        out recvConsecutiveFrameCount))
                                    {
                                        return FlowControl(
                                            requestCanId, responseCanId, requestCanType, canProtocol, canFormat,
                                            isContainsReqCanId, isContainsRecvCanId, pendingByte,
                                            recvFrameLen, getConsecutiveFrameResponseExp,
                                            recvConsecutiveFrameCount, timeoutFromMilliseconds, recv1[0].CanData,
                                            ref responseData);
                                        //if (!isContainsReqCanId)
                                        //    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                        //if (!isContainsRecvCanId)
                                        //    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                        //return false;
                                    }
                                    if (recv1[0].CanData.Length < recvFrameLen + 1)
                                    {
                                        if (!isContainsReqCanId)
                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                        if (!isContainsRecvCanId)
                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                        return false;
                                    }

                                    responseData = new byte[recvFrameLen];
                                    Array.Copy(recv1[0].CanData, 1, responseData, 0, recvFrameLen);
                                    {
                                        if (!isContainsReqCanId)
                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                        if (!isContainsRecvCanId)
                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                        return true;
                                    }
                                }
                            }

                            #endregion

                            #region 间隔发送

                            for (var i = 0; i < pendingSendDatas.Count; i++)
                            {
                                if (i == pendingSendDatas.Count - 1)
                                {
                                    // 调用发送多帧函数
                                    var recv1 =
                                        _canBus.SendAndRecvCanMsg(
                                            new[]
                                        {
                                            new CanDataPackage(requestCanId, canProtocol, requestCanType,
                                                canFormat, pendingSendDatas[i])
                                        },
                                            getLastFrameResponseExp, 1, timeoutFromMilliseconds);

                                    if (recv1 == null || recv1.Length != 1)
                                    {
                                        if (!isContainsReqCanId)
                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                        if (!isContainsRecvCanId)
                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                        return false;
                                    }

                                    ushort recvConsecutiveFrameCount;
                                    ushort recvFrameLen;
                                    if (IsHaveConsecutiveFrame(recv1[0].CanData[0], recv1[0].CanData[1], out recvFrameLen,
                                        out recvConsecutiveFrameCount))
                                    {
                                        return FlowControl(
                                            requestCanId, responseCanId, requestCanType, canProtocol, canFormat,
                                            isContainsReqCanId, isContainsRecvCanId, pendingByte,
                                            recvFrameLen, getConsecutiveFrameResponseExp,
                                            recvConsecutiveFrameCount, timeoutFromMilliseconds, recv1[0].CanData,
                                            ref responseData);
                                        //if (!isContainsReqCanId)
                                        //    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                        //if (!isContainsRecvCanId)
                                        //    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                        //return false;
                                    }
                                    if (recv1[0].CanData.Length < recvFrameLen + 1)
                                    {
                                        if (!isContainsReqCanId)
                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                        if (!isContainsRecvCanId)
                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                        return false;
                                    }

                                    responseData = new byte[recvFrameLen];
                                    Array.Copy(recv1[0].CanData, 1, responseData, 0, recvFrameLen);
                                    {
                                        if (!isContainsReqCanId)
                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                        if (!isContainsRecvCanId)
                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                        return true;
                                    }
                                }

                                _canBus.SendCanDatas(new[]
                                {
                                    new CanDataPackage(
                                        requestCanId, canProtocol, requestCanType, canFormat,
                                        pendingSendDatas[i])
                                });

                                Thread.Sleep(seperateTime);
                            }

                            #endregion
                        }

                        if (seperateTime > 0) // 需要间隔发送
                        {
                            #region 间隔发送
                            var baseIndex = 0;

                            var sendGroupCount = pendingSendDatas.Count / blockSize;
                            if (pendingSendDatas.Count % blockSize > 0)
                                sendGroupCount++;

                            for (var i = 0; i < sendGroupCount; i++)
                            {
                                for (var j = 0; j < blockSize;)
                                {
                                    // send can data
                                    if (baseIndex < pendingSendDatas.Count - 1)
                                    {
                                        // 调用发送多帧函数
                                        var recv1 =
                                            _canBus.SendAndRecvCanMsg(
                                                new[]
                                                {
                                                    new CanDataPackage(
                                                        requestCanId, canProtocol, requestCanType, canFormat,
                                                        pendingSendDatas[baseIndex])
                                                },
                                                getLastFrameResponseExp, 1, timeoutFromMilliseconds);

                                        if (recv1 == null || recv1.Length != 1)
                                        {
                                            if (!isContainsReqCanId)
                                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            if (!isContainsRecvCanId)
                                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            return false;
                                        }

                                        ushort recvConsecutiveFrameCount;
                                        ushort recvFrameLen;
                                        if (IsHaveConsecutiveFrame(recv1[0].CanData[0], recv1[0].CanData[1], out recvFrameLen,
                                            out recvConsecutiveFrameCount))
                                        {
                                            return FlowControl(
                                                requestCanId, responseCanId, requestCanType, canProtocol, canFormat,
                                                isContainsReqCanId, isContainsRecvCanId, pendingByte,
                                                recvFrameLen, getConsecutiveFrameResponseExp,
                                                recvConsecutiveFrameCount, timeoutFromMilliseconds, recv1[0].CanData,
                                                ref responseData);
                                            //if (!isContainsReqCanId)
                                            //    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            //if (!isContainsRecvCanId)
                                            //    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            //return false;
                                        }
                                        if (recv1[0].CanData.Length < recvFrameLen + 1)
                                        {
                                            if (!isContainsReqCanId)
                                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            if (!isContainsRecvCanId)
                                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            return false;
                                        }

                                        responseData = new byte[recvFrameLen];
                                        Array.Copy(recv1[0].CanData, 1, responseData, 0, recvFrameLen);
                                        {
                                            if (!isContainsReqCanId)
                                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            if (!isContainsRecvCanId)
                                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        // 调用发送多帧函数
                                        var recv1 =
                                            _canBus.SendAndRecvCanMsg(
                                                new[]
                                                {
                                                    new CanDataPackage(requestCanId, canProtocol, requestCanType,
                                                        canFormat, pendingSendDatas[baseIndex])
                                                },
                                                getLastFrameResponseExp, 1, timeoutFromMilliseconds);

                                        if (recv1 == null || recv1.Length != 1)
                                        {
                                            if (!isContainsReqCanId)
                                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            if (!isContainsRecvCanId)
                                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            return false;
                                        }

                                        ushort recvConsecutiveFrameCount;
                                        ushort recvFrameLen;
                                        if (IsHaveConsecutiveFrame(recv1[0].CanData[0], recv1[0].CanData[1], out recvFrameLen,
                                            out recvConsecutiveFrameCount))
                                        {
                                            return FlowControl(
                                                requestCanId, responseCanId, requestCanType, canProtocol, canFormat,
                                                isContainsReqCanId, isContainsRecvCanId, pendingByte,
                                                recvFrameLen, getConsecutiveFrameResponseExp,
                                                recvConsecutiveFrameCount, timeoutFromMilliseconds, recv1[0].CanData,
                                                ref responseData);
                                            //if (!isContainsReqCanId)
                                            //    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            //if (!isContainsRecvCanId)
                                            //    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            //return false;
                                        }
                                        if (recv1[0].CanData.Length < recvFrameLen + 1)
                                        {
                                            if (!isContainsReqCanId)
                                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            if (!isContainsRecvCanId)
                                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            return false;
                                        }

                                        responseData = new byte[recvFrameLen];
                                        Array.Copy(recv1[0].CanData, 1, responseData, 0, recvFrameLen);
                                        {
                                            if (!isContainsReqCanId)
                                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            if (!isContainsRecvCanId)
                                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            return true;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else // 不需要间隔发送
                        {
                            #region 不间隔发送

                            if (maxFrameCount >= blockSize)
                            {
                                var sendGroupCount = pendingSendDatas.Count / blockSize;
                                if (pendingSendDatas.Count % blockSize > 0)
                                    sendGroupCount++;

                                var baseIndex = 0;

                                for (var i = 0; i < sendGroupCount; i++)
                                {
                                    if (i == sendGroupCount - 1)
                                    {
                                        var tempPackages = new List<CanDataPackage>();

                                        while (true)
                                        {
                                            if (baseIndex == pendingSendDatas.Count)
                                                break;

                                            tempPackages.Add(new CanDataPackage(
                                                 requestCanId, canProtocol, requestCanType, canFormat,
                                                 pendingSendDatas[baseIndex]));
                                            baseIndex++;
                                        }

                                        // 调用发送多帧函数
                                        var recv1 =
                                            _canBus.SendAndRecvCanMsg(tempPackages.ToArray(),
                                                getLastFrameResponseExp, 1, timeoutFromMilliseconds);

                                        if (recv1 == null || recv1.Length != 1)
                                        {
                                            if (!isContainsReqCanId)
                                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            if (!isContainsRecvCanId)
                                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            return false;
                                        }

                                        ushort recvConsecutiveFrameCount;
                                        ushort recvFrameLen;
                                        if (IsHaveConsecutiveFrame(recv1[0].CanData[0], recv1[0].CanData[1], out recvFrameLen,
                                            out recvConsecutiveFrameCount))
                                        {
                                            return FlowControl(
                                                requestCanId, responseCanId, requestCanType, canProtocol, canFormat,
                                                isContainsReqCanId, isContainsRecvCanId, pendingByte,
                                                recvFrameLen, getConsecutiveFrameResponseExp,
                                                recvConsecutiveFrameCount, timeoutFromMilliseconds, recv1[0].CanData,
                                                ref responseData);
                                            //if (!isContainsReqCanId)
                                            //    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            //if (!isContainsRecvCanId)
                                            //    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            //return false;
                                        }
                                        if (recv1[0].CanData.Length < recvFrameLen + 1)
                                        {
                                            if (!isContainsReqCanId)
                                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            if (!isContainsRecvCanId)
                                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            return false;
                                        }

                                        responseData = new byte[recvFrameLen];
                                        Array.Copy(recv1[0].CanData, 1, responseData, 0, recvFrameLen);
                                        {
                                            if (!isContainsReqCanId)
                                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            if (!isContainsRecvCanId)
                                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        var tempPackages = new List<CanDataPackage>();
                                        for (var j = 0; j < blockSize; j++)
                                        {
                                            tempPackages.Add(new CanDataPackage(
                                                requestCanId, canProtocol, requestCanType, canFormat,
                                                pendingSendDatas[baseIndex]));
                                            baseIndex++;
                                        }

                                        // 调用发送多帧函数
                                        var recv1 =
                                            _canBus.SendAndRecvCanMsg(tempPackages.ToArray(),
                                                get0X03ResponseExp, 1, timeoutFromMilliseconds);

                                        if (recv1 == null || !recv1.Any())
                                        {
                                            if (!isContainsReqCanId)
                                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                                            if (!isContainsRecvCanId)
                                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                                            return false;
                                        }
                                    }
                                }
                            }
                            else // 一次发送数量小于模块能接收的包数量
                            {
                                var baseIndex = 0;

                                var sendGroupCount = pendingSendDatas.Count / blockSize;
                                if (pendingSendDatas.Count % blockSize > 0)
                                    sendGroupCount++;

                                for (var i = 0; i < sendGroupCount; i++)
                                {
                                    if (i == sendGroupCount - 1)
                                    {
                                        if (blockSize < maxFrameCount)
                                        {
                                            var tempPackages = new List<CanDataPackage>();

                                            tempPackages.Clear();

                                            for (var k = 0; k < maxFrameCount; k++)
                                            {
                                                if (baseIndex == pendingSendDatas.Count)
                                                    break;
                                                tempPackages.Add(new CanDataPackage(
                                                    requestCanId, canProtocol, requestCanType, canFormat,
                                                    pendingSendDatas[baseIndex]));
                                                baseIndex++;
                                            }

                                            // 调用发送多帧函数
                                            var recv1 =
                                                _canBus.SendAndRecvCanMsg(tempPackages.ToArray(),
                                                    getLastFrameResponseExp, 1, timeoutFromMilliseconds);

                                            if (recv1 == null || recv1.Length != 1)
                                            {
                                                if (!isContainsReqCanId)
                                                    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                if (!isContainsRecvCanId)
                                                    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                return false;
                                            }

                                            ushort recvConsecutiveFrameCount;
                                            ushort recvFrameLen;
                                            if (IsHaveConsecutiveFrame(recv1[0].CanData[0], recv1[0].CanData[1], out recvFrameLen,
                                                out recvConsecutiveFrameCount))
                                            {
                                                return FlowControl(
                                                    requestCanId, responseCanId, requestCanType, canProtocol, canFormat,
                                                    isContainsReqCanId, isContainsRecvCanId, pendingByte,
                                                    recvFrameLen, getConsecutiveFrameResponseExp,
                                                    recvConsecutiveFrameCount, timeoutFromMilliseconds, recv1[0].CanData,
                                                    ref responseData);
                                                //if (!isContainsReqCanId)
                                                //    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                //if (!isContainsRecvCanId)
                                                //    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                //return false;
                                            }
                                            if (recv1[0].CanData.Length < recvFrameLen + 1)
                                            {
                                                if (!isContainsReqCanId)
                                                    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                if (!isContainsRecvCanId)
                                                    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                return false;
                                            }

                                            responseData = new byte[recvFrameLen];
                                            Array.Copy(recv1[0].CanData, 1, responseData, 0, recvFrameLen);
                                            {
                                                if (!isContainsReqCanId)
                                                    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                if (!isContainsRecvCanId)
                                                    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                return true;
                                            }
                                        }
                                        else
                                        {
                                            var tempPackages = new List<CanDataPackage>();

                                            for (var j = 0; j < blockSize / maxFrameCount;)
                                            {
                                                tempPackages.Clear();

                                                for (var k = 0; k < maxFrameCount; k++)
                                                {
                                                    if (baseIndex == pendingSendDatas.Count)
                                                        break;
                                                    tempPackages.Add(new CanDataPackage(
                                                        requestCanId, canProtocol, requestCanType, canFormat,
                                                        pendingSendDatas[baseIndex]));
                                                    baseIndex++;
                                                }

                                                if (baseIndex == pendingSendDatas.Count)
                                                {
                                                    // 调用发送多帧函数
                                                    var recv1 =
                                                        _canBus.SendAndRecvCanMsg(tempPackages.ToArray(),
                                                            getLastFrameResponseExp, 1, timeoutFromMilliseconds);

                                                    if (recv1 == null || recv1.Length != 1)
                                                    {
                                                        if (!isContainsReqCanId)
                                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                        if (!isContainsRecvCanId)
                                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                        return false;
                                                    }

                                                    ushort recvConsecutiveFrameCount;
                                                    ushort recvFrameLen;
                                                    if (IsHaveConsecutiveFrame(recv1[0].CanData[0], recv1[0].CanData[1],
                                                        out recvFrameLen,
                                                        out recvConsecutiveFrameCount))
                                                    {
                                                        return FlowControl(
                                                            requestCanId, responseCanId, requestCanType, canProtocol,
                                                            canFormat,
                                                            isContainsReqCanId, isContainsRecvCanId, pendingByte,
                                                            recvFrameLen, getConsecutiveFrameResponseExp,
                                                            recvConsecutiveFrameCount, timeoutFromMilliseconds,
                                                            recv1[0].CanData, ref responseData);
                                                        //if (!isContainsReqCanId)
                                                        //    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                        //if (!isContainsRecvCanId)
                                                        //    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                        //return false;
                                                    }
                                                    if (recv1[0].CanData.Length < recvFrameLen + 1)
                                                    {
                                                        if (!isContainsReqCanId)
                                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                        if (!isContainsRecvCanId)
                                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                        return false;
                                                    }

                                                    responseData = new byte[recvFrameLen];
                                                    Array.Copy(recv1[0].CanData, 1, responseData, 0, recvFrameLen);
                                                    {
                                                        if (!isContainsReqCanId)
                                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                        if (!isContainsRecvCanId)
                                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                        return true;
                                                    }
                                                }
                                                else
                                                {
                                                    _canBus.SendCanDatas(tempPackages.ToArray());

                                                    tempPackages.Clear();

                                                    for (var k = baseIndex; k < pendingSendDatas.Count - 1; k++)
                                                    {
                                                        _canBus.SendCanDatas(new[]
                                                        {
                                                            new CanDataPackage(
                                                                requestCanId, canProtocol, requestCanType, canFormat,
                                                                pendingSendDatas[baseIndex])
                                                        });
                                                        baseIndex++;
                                                    }

                                                    // 调用发送多帧函数
                                                    var recv1 =
                                                        _canBus.SendAndRecvCanMsg(new[]
                                                        {
                                                            new CanDataPackage(
                                                                requestCanId, canProtocol, requestCanType, canFormat,
                                                                pendingSendDatas[baseIndex])
                                                        }, getLastFrameResponseExp, 1, timeoutFromMilliseconds);

                                                    if (recv1 == null || recv1.Length != 1)
                                                    {
                                                        if (!isContainsReqCanId)
                                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                        if (!isContainsRecvCanId)
                                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                        return false;
                                                    }

                                                    ushort recvConsecutiveFrameCount;
                                                    ushort recvFrameLen;
                                                    if (IsHaveConsecutiveFrame(recv1[0].CanData[0], recv1[0].CanData[1], out recvFrameLen,
                                                        out recvConsecutiveFrameCount))
                                                    {
                                                        return FlowControl(
                                                            requestCanId, responseCanId, requestCanType, canProtocol,
                                                            canFormat,
                                                            isContainsReqCanId, isContainsRecvCanId, pendingByte,
                                                            recvFrameLen, getConsecutiveFrameResponseExp,
                                                            recvConsecutiveFrameCount, timeoutFromMilliseconds,
                                                            recv1[0].CanData, ref responseData);
                                                        //if (!isContainsReqCanId)
                                                        //    _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                        //if (!isContainsRecvCanId)
                                                        //    _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                        //return false;
                                                    }
                                                    if (recv1[0].CanData.Length < recvFrameLen + 1)
                                                    {
                                                        if (!isContainsReqCanId)
                                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                        if (!isContainsRecvCanId)
                                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                        return false;
                                                    }

                                                    responseData = new byte[recvFrameLen];
                                                    Array.Copy(recv1[0].CanData, 1, responseData, 0, recvFrameLen);
                                                    {
                                                        if (!isContainsReqCanId)
                                                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                        if (!isContainsRecvCanId)
                                                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var tempPackages = new List<CanDataPackage>();

                                        if (blockSize <= maxFrameCount)
                                        {

                                        }
                                        else
                                        {
                                            if (blockSize % maxFrameCount == 0)
                                            {
                                                for (var j = 0; j < blockSize / maxFrameCount; j++)
                                                {
                                                    tempPackages.Clear();

                                                    for (var k = 0; k < maxFrameCount; k++)
                                                    {
                                                        tempPackages.Add(new CanDataPackage(
                                                            requestCanId, canProtocol, requestCanType, canFormat,
                                                            pendingSendDatas[baseIndex]));
                                                        baseIndex++;
                                                    }

                                                    if (j == blockSize / maxFrameCount - 1)
                                                    {
                                                        // 调用发送多帧函数
                                                        var recv1 =
                                                            _canBus.SendAndRecvCanMsg(tempPackages.ToArray(),
                                                                get0X03ResponseExp, 1, timeoutFromMilliseconds);

                                                        if (recv1 == null || !recv1.Any())
                                                        {
                                                            if (!isContainsReqCanId)
                                                                _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                            if (!isContainsRecvCanId)
                                                                _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                            return false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        _canBus.SendCanDatas(tempPackages.ToArray());
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (var j = 0; j < blockSize / maxFrameCount; j++)
                                                {
                                                    tempPackages.Clear();

                                                    for (var k = 0; k < maxFrameCount; k++)
                                                    {
                                                        tempPackages.Add(new CanDataPackage(
                                                            requestCanId, canProtocol, requestCanType, canFormat,
                                                            pendingSendDatas[baseIndex]));
                                                        baseIndex++;
                                                    }

                                                    _canBus.SendCanDatas(tempPackages.ToArray());
                                                }

                                                tempPackages.Clear();

                                                for (var j = 0; j < blockSize % maxFrameCount; j++)
                                                {
                                                    tempPackages.Add(new CanDataPackage(
                                                           requestCanId, canProtocol, requestCanType, canFormat,
                                                           pendingSendDatas[baseIndex]));
                                                    baseIndex++;
                                                }

                                                // 调用发送多帧函数
                                                var recv1123 =
                                                    _canBus.SendAndRecvCanMsg(tempPackages.ToArray(), get0X03ResponseExp,
                                                        1, timeoutFromMilliseconds);

                                                if (recv1123 == null || !recv1123.Any())
                                                {
                                                    if (!isContainsReqCanId)
                                                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                                                    if (!isContainsRecvCanId)
                                                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                                                    return false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }

                    #endregion
                }
                #endregion

                return false;
            }

            private static bool IsHaveConsecutiveFrame(
                byte data0, byte data1,
                out ushort frameLen, out ushort consecutiveFrameCount)
            {
                var isSingle = data0.GetByteHighOrder() == 0;

                if (isSingle)
                {
                    frameLen = data0;
                    consecutiveFrameCount = 0;
                    return false;
                }

                frameLen = (ushort)(data0.GetByteLowOrder() * 256 + data1);
                if ((frameLen - 6) % 7 == 0)
                    consecutiveFrameCount = (ushort)((frameLen - 6) / 7);
                else
                    consecutiveFrameCount = (ushort)((frameLen - 6) / 7 + 1);
                return true;
            }

            private bool FlowControl(
                uint requestCanId, uint responseCanId, CanType requestCanType, CanProtocol canProtocol, CanFormat canFormat,
                bool isContainsReqCanId, bool isContainsRecvCanId, byte pendingByte, int echoFrameLen,
                Expression<Func<CanDataPackage, bool>> getConsecutiveFrameResponseExp,
                int echoConsecutiveFrameCount, int timeoutFromMilliseconds,
                IReadOnlyList<byte> echoData, ref byte[] responseData)
            {
                var tempDataLst = new List<byte>();
                for (var i = 2; i < 8; i++)
                    tempDataLst.Add(echoData[i]);

                //Thread.Sleep(20);

                var testerFlowControlData =
                    new byte[] { 0x30, 0x00, 0x00, pendingByte, pendingByte, pendingByte, pendingByte, pendingByte };

                var recv2 = _canBus.SendAndRecvCanMsg(
                    new[]
                    {
                        new CanDataPackage(
                            requestCanId, canProtocol, requestCanType, canFormat, testerFlowControlData)
                    },
                    getConsecutiveFrameResponseExp, echoConsecutiveFrameCount, timeoutFromMilliseconds);

                if (recv2 == null || recv2.Length < echoConsecutiveFrameCount)
                {
                    if (!isContainsReqCanId)
                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                    if (!isContainsRecvCanId)
                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                    return false;
                }

                var serialNo = (byte)0x21;

                for (var i = 0; i < echoConsecutiveFrameCount; i++)
                {
                    var r =
                        recv2.FirstOrDefault(f => f.CanData[0] == serialNo);
                    serialNo++;
                    if (serialNo > 47)
                        serialNo = (byte)0x20;
                    if (r != null)
                        for (var j = 1; j < r.CanData.Length; j++)
                            tempDataLst.Add(r.CanData[j]);
                    else
                    {
                        if (!isContainsReqCanId)
                            _canBus.RemoveDoNotFilterCanId(requestCanId);
                        if (!isContainsRecvCanId)
                            _canBus.RemoveDoNotFilterCanId(responseCanId);
                        return false;
                    }
                }

                if (tempDataLst.Count < echoFrameLen)
                {
                    if (!isContainsReqCanId)
                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                    if (!isContainsRecvCanId)
                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                    return false;
                }
                responseData = new byte[echoFrameLen];
                Array.Copy(tempDataLst.ToArray(), 0, responseData, 0, echoFrameLen);
                {
                    if (!isContainsReqCanId)
                        _canBus.RemoveDoNotFilterCanId(requestCanId);
                    if (!isContainsRecvCanId)
                        _canBus.RemoveDoNotFilterCanId(responseCanId);
                    return true;
                }
            }

            #endregion
        }

        /// <summary>
        /// CAN消息接收包
        /// </summary>
        public class CanDataPackage
        {
            /// <summary>
            /// 消息唯一标识
            /// </summary>
            public string MessageKey { get; private set; }

            /// <summary>
            /// CAN消息的时间
            /// </summary>
            public DateTime DateTime { get; private set; }

            /// <summary>
            /// CAN ID
            /// </summary>
            public uint CanId { get; private set; }

            /// <summary>
            /// CAN协议类型
            /// </summary>
            public CanProtocol CanProtocol { get; private set; }

            /// <summary>
            /// 帧类型
            /// </summary>
            public CanType CanType { get; private set; }

            /// <summary>
            /// 帧格式
            /// </summary>
            public CanFormat CanFormat { get; private set; }

            /// <summary>
            /// CAN消息长度
            /// </summary>
            public int CanDataLen { get; private set; }

            /// <summary>
            /// CAN消息内容
            /// </summary>
            public byte[] CanData { get; private set; }

            /// <summary>
            /// CAN消息包
            /// </summary>
            /// <param name="canId">CAN ID</param>
            /// <param name="canProtocol">CAN协议类型</param>
            /// <param name="canType">帧类型</param>
            /// <param name="canFormat">帧格式</param>
            /// <param name="data">帧数据</param>
            public CanDataPackage(
                uint canId, CanProtocol canProtocol, CanType canType, CanFormat canFormat, byte[] data)
            {
                MessageKey = Guid.NewGuid().ToString();
                DateTime = DateTime.Now;
                CanId = canId;
                CanProtocol = canProtocol;
                CanType = canType;
                CanFormat = canFormat;

                if (data == null)
                {
                    CanData = new byte[0];
                    CanDataLen = 0;
                }
                else
                {
                    CanData = data;
                    CanDataLen = data.Length;
                }
            }
        }

        /// <summary>
        /// 帧类型
        /// </summary>
        public enum CanType : byte
        {
            /// <summary>
            /// 00
            /// 标准帧
            /// </summary>
            Standard = 0x00,

            /// <summary>
            /// 04
            /// 扩展帧
            /// </summary>
            Extended = 0x04,
        }

        /// <summary>
        /// 帧格式
        /// </summary>
        public enum CanFormat : byte
        {
            /// <summary>
            /// 00
            /// 数据帧
            /// </summary>
            Data = 0x00,

            /// <summary>
            /// 01
            /// 远程帧
            /// </summary>
            Remote = 0x01,
        }

        /// <summary>
        /// CAN协议类型
        /// </summary>
        public enum CanProtocol : byte
        {
            /// <summary>
            /// CAN
            /// </summary>
            Can,

            /// <summary>
            /// CAN FD
            /// </summary>
            CanFd,

            /// <summary>
            /// SPEED CAN FD
            /// </summary>
            SpeedCanFd
        }

        public enum OnPushCanDataType
        {
            /// <summary>
            /// 发送
            /// </summary>
            Tx,

            /// <summary>
            /// 接收
            /// 非过滤列表中
            /// </summary>
            FilterRx,

            /// <summary>
            /// 接收
            /// 过滤列表中
            /// </summary>
            NotFilterRx
        }
    }
}
