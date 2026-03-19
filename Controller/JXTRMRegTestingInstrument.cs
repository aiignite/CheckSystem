using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class JXTRMRegTestingInstrument : ControllerBase
    {
        [Description("R/W,State为busy时的等待时间")]
        public int BusyWaitingMs = 700;
        [Description("R/W,通讯超时时间")]
        public int CommunicationTimeoutMs = 3000;

        [Description("R/W,当前最大读取电阻数量")]
        public int CurrentMaxReadRegCount = 30;

        [Description("R,DEV1-R1")] public double R1 = double.MinValue;
        [Description("R,DEV1-R2")] public double R2 = double.MinValue;
        [Description("R,DEV1-R3")] public double R3 = double.MinValue;
        [Description("R,DEV1-R4")] public double R4 = double.MinValue;
        [Description("R,DEV1-R5")] public double R5 = double.MinValue;
        [Description("R,DEV1-R6")] public double R6 = double.MinValue;
        [Description("R,DEV1-R7")] public double R7 = double.MinValue;
        [Description("R,DEV1-R8")] public double R8 = double.MinValue;
        [Description("R,DEV1-R9")] public double R9 = double.MinValue;
        [Description("R,DEV1-R10")] public double R10 = double.MinValue;
        [Description("R,DEV1-R11")] public double R11 = double.MinValue;
        [Description("R,DEV1-R12")] public double R12 = double.MinValue;
        [Description("R,DEV1-R13")] public double R13 = double.MinValue;
        [Description("R,DEV1-R14")] public double R14 = double.MinValue;
        [Description("R,DEV1-R15")] public double R15 = double.MinValue;
        [Description("R,DEV1-R16")] public double R16 = double.MinValue;
        [Description("R,DEV1-R17")] public double R17 = double.MinValue;
        [Description("R,DEV1-R18")] public double R18 = double.MinValue;
        [Description("R,DEV1-R19")] public double R19 = double.MinValue;
        [Description("R,DEV1-R20")] public double R20 = double.MinValue;
        [Description("R,DEV1-R21")] public double R21 = double.MinValue;
        [Description("R,DEV1-R22")] public double R22 = double.MinValue;
        [Description("R,DEV1-R23")] public double R23 = double.MinValue;
        [Description("R,DEV1-R24")] public double R24 = double.MinValue;
        [Description("R,DEV1-R25")] public double R25 = double.MinValue;
        [Description("R,DEV1-R26")] public double R26 = double.MinValue;
        [Description("R,DEV1-R27")] public double R27 = double.MinValue;
        [Description("R,DEV1-R28")] public double R28 = double.MinValue;
        [Description("R,DEV1-R29")] public double R29 = double.MinValue;
        [Description("R,DEV1-R30")] public double R30 = double.MinValue;

        [Description("R,DEV2-RR1")] public double RR1 = double.MinValue;
        [Description("R,DEV2-RR2")] public double RR2 = double.MinValue;
        [Description("R,DEV2-RR3")] public double RR3 = double.MinValue;
        [Description("R,DEV2-RR4")] public double RR4 = double.MinValue;
        [Description("R,DEV2-RR5")] public double RR5 = double.MinValue;
        [Description("R,DEV2-RR6")] public double RR6 = double.MinValue;
        [Description("R,DEV2-RR7")] public double RR7 = double.MinValue;
        [Description("R,DEV2-RR8")] public double RR8 = double.MinValue;
        [Description("R,DEV2-RR9")] public double RR9 = double.MinValue;
        [Description("R,DEV2-RR10")] public double RR10 = double.MinValue;
        [Description("R,DEV2-RR11")] public double RR11 = double.MinValue;
        [Description("R,DEV2-RR12")] public double RR12 = double.MinValue;
        [Description("R,DEV2-RR13")] public double RR13 = double.MinValue;
        [Description("R,DEV2-RR14")] public double RR14 = double.MinValue;
        [Description("R,DEV2-RR15")] public double RR15 = double.MinValue;
        [Description("R,DEV2-RR16")] public double RR16 = double.MinValue;
        [Description("R,DEV2-RR17")] public double RR17 = double.MinValue;
        [Description("R,DEV2-RR18")] public double RR18 = double.MinValue;
        [Description("R,DEV2-RR19")] public double RR19 = double.MinValue;
        [Description("R,DEV2-RR20")] public double RR20 = double.MinValue;
        [Description("R,DEV2-RR21")] public double RR21 = double.MinValue;
        [Description("R,DEV2-RR22")] public double RR22 = double.MinValue;
        [Description("R,DEV2-RR23")] public double RR23 = double.MinValue;
        [Description("R,DEV2-RR24")] public double RR24 = double.MinValue;
        [Description("R,DEV2-RR25")] public double RR25 = double.MinValue;
        [Description("R,DEV2-RR26")] public double RR26 = double.MinValue;
        [Description("R,DEV2-RR27")] public double RR27 = double.MinValue;
        [Description("R,DEV2-RR28")] public double RR28 = double.MinValue;
        [Description("R,DEV2-RR29")] public double RR29 = double.MinValue;
        [Description("R,DEV2-RR30")] public double RR30 = double.MinValue;

        private MyAsyncSocketClient _tcpClient;
        private readonly Dictionary<long, byte[]> _recvBuff = new Dictionary<long, byte[]>();

        public JXTRMRegTestingInstrument(string name) : base(name)
        {
            _tcpClient = new MyAsyncSocketClient();
            _tcpClient.OnPushSocketsToTcpClient += _tcpClient_OnPushSocketsToTcpClient;
        }

        private void _tcpClient_OnPushSocketsToTcpClient(BaseSocket sockets)
        {
            if (sockets == null || sockets.RecBuffer == null)
                return;

            if (sockets.RecBuffer.Length == 0)
                return;

            if (sockets.Offset == 0)
                return;

            var buffer = new byte[sockets.Offset];
            Array.Copy(sockets.RecBuffer, 0, buffer, 0, sockets.Offset);

            if ((buffer[1] == ReadFunc || buffer[1] == WriteFunc))
            {
                var crc1 = buffer[buffer.Length - 2];
                var crc2 = buffer[buffer.Length - 1];
                var toCheckCrcData = new byte[buffer.Length - 2];
                Array.Copy(buffer, toCheckCrcData, toCheckCrcData.Length);
                var checkCrc = ValueHelper.Crc16(toCheckCrcData).ToArray();
                if (checkCrc[0] == crc1 && checkCrc[1] == crc2)
                {
                    var ts = HighPrecisionTimer.GetTimestamp();
                    if (_recvBuff.ContainsKey(ts))
                        _recvBuff.Remove(ts);

                    if (_recvBuff.Count > 256)
                    {
                        var removeKeys = _recvBuff.Where(x => HighPrecisionTimer.GetTimestampIntervalMs(x.Key, ts) >= 10 * 1000).Select(x => x.Key).ToList();
                        foreach (var key in removeKeys)
                            _recvBuff.Remove(key);
                    }

                    _recvBuff.Add(ts, buffer);

                    if (_isWaitingReadAck && buffer[0] == _readingDev)
                    {
                        if (buffer.Length == 1 + 1 + 1 + 2 + _readingLength * 2 && buffer[1] == ReadFunc && buffer[2] == _readingLength * 2)
                            _readWaitHandle.Set();
                    }

                    if (_isWaitingWriteAck && buffer[0] == _writingDev)
                    {
                        if (buffer.Length == 8 && buffer[1] == WriteFunc && buffer[2] * 256 + buffer[3] == _writingStartAddr && buffer[4] == _wrtingContent[0] && buffer[5] == _wrtingContent[1])
                            _writeWaitHandle.Set();
                    }
                }
            }
        }

        ~JXTRMRegTestingInstrument()
        {
            _tcpClient?.Stop();
            Dispose();
        }

        public void ConnectJXTRM(string ipPort)
        {
            var split = ipPort.Split(':');
            var ipAddressStr = split[0];
            var port = Convert.ToInt32(split[1]);

            _tcpClient?.InitSocket(ipAddressStr, port);
        }

        private const byte Dev1 = 0x01;
        private const byte Dev2 = 0x02;
        private const byte ReadFunc = 0x03;
        private const byte WriteFunc = 0x06;
        private readonly EventWaitHandle _readWaitHandle = new AutoResetEvent(false);
        private bool _isWaitingReadAck;
        private byte _readingDev;
        private ushort _readingStartAddr;
        private ushort _readingLength;

        private readonly EventWaitHandle _writeWaitHandle = new AutoResetEvent(false);
        private bool _isWaitingWriteAck;
        private byte _writingDev;
        private ushort _writingStartAddr;
        private byte[] _wrtingContent;

        [Description("读DEV1的R1到R30")]
        public void ReadRegFromR1ToR30() => ReadRegArea(Dev1);

        [Description("读DEV2的RR1到RR30")]
        public void ReadRegFromRR1ToRR30() => ReadRegArea(Dev2);

        private void ResetRegArea(byte dev)
        {
            for (var baseRIndex = 1; baseRIndex <= 30; baseRIndex++)
            {
                var fieldR = GetType().GetField(dev == Dev2 ? string.Format("RR{0}", baseRIndex) : string.Format("R{0}", baseRIndex));
                fieldR?.SetValue(this, double.MinValue);
            }
        }

        private void ReadRegArea(byte dev)
        {
            ResetRegArea(dev);

            var stateRes = ReadState(dev);
            if (stateRes == -1)
                return;

            if (stateRes == 0)
            {
                ExecDataCollection(dev);
            }
            else if (stateRes == 2 || stateRes == 3)
            {
                var delayCount = 0;

                for (var i = 0; i < 200; i++)
                {
                    if (!SwitchEnable(dev, false))
                        return;

                    stateRes = ReadState(dev);
                    if (stateRes == -1)
                        return;

                    if (stateRes == 0)
                        break;

                    if (stateRes == 1)
                    {
                        if (delayCount == 5)
                            return;

                        Thread.Sleep(BusyWaitingMs > 0 && BusyWaitingMs <= 10 * 1000 ? BusyWaitingMs : 1000);
                        delayCount++;
                    }

                    if (stateRes == 3 || stateRes == 4)
                        break;

                    Thread.Sleep(50);
                }

                ExecDataCollection(dev);
            }
        }

        private void ExecDataCollection(byte dev)
        {
            if (!SwitchEnable(dev, true))
                return;

            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(BusyWaitingMs > 0 && BusyWaitingMs <= 10 * 1000 ? BusyWaitingMs : 1000);
                var stateRes = ReadState(dev);
                if (stateRes == 0 || stateRes == -1)
                    return;

                if (stateRes == 1)
                    continue;

                break;
            }

            var count = 30;
            if (CurrentMaxReadRegCount < 1)
                count = 1;
            else if (CurrentMaxReadRegCount > 30)
                count = 30;
            else
                count = CurrentMaxReadRegCount;

            ushort[] readRegContents;
            if (ReadReg(dev, 6001, (ushort)(count * 2), out readRegContents))
            {
                var baseRIndex = 1;
                for (var i = 0; i < readRegContents.Length; i = i + 2)
                {
                    var fieldR = GetType().GetField(dev == Dev1 ? string.Format("R{0}", baseRIndex++) : string.Format("RR{0}", baseRIndex++));
                    fieldR?.SetValue(this, readRegContents[i + 1]);
                }
            }
        }

        private bool SwitchEnable(byte dev, bool isEnable)
        {
            return WriteReg(dev, 1001, isEnable ? new byte[] { 0x00, 0x01 } : new byte[] { 0x00, 0x00 });
        }

        private int ReadState(byte dev)
        {
            return ReadReg(dev, 1000, 1, out ushort[] stateContent) ? (stateContent[0]) : -1;
        }

        private bool ReadReg(byte dev, ushort startAddr, ushort length, out ushort[] contents)
        {
            contents = new ushort[length];
            if (_isWaitingReadAck)
                return false;

            var readBytes = new List<byte> { ReadFunc };
            readBytes.AddRange(BitConverter.GetBytes(startAddr).Reverse());
            readBytes.AddRange(BitConverter.GetBytes(length).Reverse());

            _readingDev = dev;
            _readingLength = length;
            _readingStartAddr = startAddr;
            _isWaitingReadAck = true;
            var sendTs = HighPrecisionTimer.GetTimestamp();
            SendData(dev, readBytes.ToArray());

            var isOk = _readWaitHandle.WaitOne(CommunicationTimeoutMs > 0 && CommunicationTimeoutMs <= 10 * 1000 ? CommunicationTimeoutMs : 3000);
            _isWaitingReadAck = false;

            if (!isOk)
                return false;

            var nowTs = HighPrecisionTimer.GetTimestamp();
            var keys = _recvBuff.Keys.ToList();
            var findKeyIndex = keys.FindIndex(
                f =>
                HighPrecisionTimer.GetTimestampIntervalMs(sendTs, f) >= 0 &&
                HighPrecisionTimer.GetTimestampIntervalMs(sendTs, f) <= 1000 &&
                 _recvBuff[f] != null && _recvBuff[f].Length == 1 + 1 + 1 + 2 * length + 2 && _recvBuff[f][1] == ReadFunc && _recvBuff[f][2] == length * 2 && _recvBuff[f][0] == dev);
            if (findKeyIndex == -1)
                return false;

            var getBuff = _recvBuff[keys[findKeyIndex]];
            var contendIndex = 0;
            for (var i = 3; i < getBuff.Length - 2; i += 2)
            {
                contents[contendIndex] = (ushort)(getBuff[i] * 256 + getBuff[i + 1]);
                contendIndex++;
            }
            return true;
        }

        private bool WriteReg(byte dev, ushort startAddr, byte[] contents)
        {
            if (contents == null || contents.Length != 1 * 2)
                return false;

            if (_isWaitingWriteAck)
                return false;

            var writeBytes = new List<byte> { WriteFunc };
            writeBytes.AddRange(BitConverter.GetBytes(startAddr).Reverse());
            writeBytes.AddRange(contents);

            _wrtingContent = new byte[contents.Length];
            Array.Copy(contents, _wrtingContent, contents.Length);
            _writingDev = dev;
            _writingStartAddr = startAddr;
            _isWaitingWriteAck = true;
            var sendTs = HighPrecisionTimer.GetTimestamp();
            SendData(dev, writeBytes.ToArray());

            var isOk = _writeWaitHandle.WaitOne(CommunicationTimeoutMs > 0 && CommunicationTimeoutMs <= 10 * 1000 ? CommunicationTimeoutMs : 3000);
            _isWaitingWriteAck = false;

            if (!isOk)
                return false;

            var nowTs = HighPrecisionTimer.GetTimestamp();
            var keys = _recvBuff.Keys.ToList();
            return keys.FindIndex(
                f =>
                HighPrecisionTimer.GetTimestampIntervalMs(sendTs, f) >= 0 &&
                HighPrecisionTimer.GetTimestampIntervalMs(sendTs, f) <= 1000 &&
                _recvBuff[f] != null && _recvBuff[f].Length == 8 && _recvBuff[f][1] == WriteFunc && (ushort)(_recvBuff[f][2] * 256 + _recvBuff[f][3]) == startAddr && _recvBuff[f][4] == contents[0] && _recvBuff[f][5] == contents[1] && _recvBuff[f][0] == dev) != -1;
        }

        private void SendData(byte dev, byte[] sendBytes)
        {
            var bs = new List<byte> { dev };
            bs.AddRange(sendBytes);
            var crc = ValueHelper.Crc16(bs);
            bs.AddRange(crc);
            _tcpClient?.SendData(bs.ToArray());
        }

        #region 20251025 分步骤

        public bool IsJxPrepared;

        public void JXPrepareDev1()
        {
            var dev = Dev1;

            IsJxPrepared = false;

            ResetRegArea(dev);

            var stateRes = ReadState(dev);
            if (stateRes == -1)
            {
                IsJxPrepared = true;
                return;
            }

            if (stateRes == 0)
            {
                //ExecDataCollection(dev);
                IsJxPrepared = true;
                return;
            }
            else if (stateRes == 2 || stateRes == 3)
            {
                var delayCount = 0;

                for (var i = 0; i < 200; i++)
                {
                    if (!SwitchEnable(dev, false))
                    {
                        IsJxPrepared = true;
                        return;
                    }

                    stateRes = ReadState(dev);
                    if (stateRes == -1)
                    {
                        IsJxPrepared = true;
                        return;
                    }

                    if (stateRes == 0)
                        break;

                    if (stateRes == 1)
                    {
                        if (delayCount == 5)
                        {
                            IsJxPrepared = true;
                            return;
                        }

                        Thread.Sleep(BusyWaitingMs > 0 && BusyWaitingMs <= 10 * 1000 ? BusyWaitingMs : 1000);
                        delayCount++;
                    }

                    if (stateRes == 3 || stateRes == 4)
                        break;

                    Thread.Sleep(50);
                }

                IsJxPrepared = true;
                return;
            }

            IsJxPrepared = true;
            return;
        }

        public void JXReadDataDev1()
        {
            var dev = Dev1;
            ResetRegArea(dev);
            ExecDataCollection(dev);
        }

        #endregion
    }
}
