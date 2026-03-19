using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CommonUtility.BusLoader
{
    public class UdsTrans
    {
        public enum AddressingModes
        {
            PhysicalAddressing,
            FunctionalAddressing,
        }

        private int id;

        /// <summary>
        /// UDS默认填充字节
        /// </summary>
        public byte FillByte = 0x55;

        public int TxId;
        public int RxId;
        public int TestId;

        /// <summary>
        /// Frame types arranged in numericalorder for efficient switch statement jump tables.
        /// </summary>
        private enum FrameType
        {
            /// <summary>
            /// Single Frame
            /// </summary>
            TxSf = 0,

            /// <summary>
            /// First Frame
            /// </summary>
            TxFf = 1,

            /// <summary>
            /// Consecutive Frame
            /// </summary>
            TxCf = 2,

            /// <summary>
            /// Flow Control Frame
            /// </summary>
            TxFc = 3
        };

        /*
        ** Masks for the PCI(Protcol ControlInformation) byte.
        ** The MS bit contains the frame type.
        ** The LS bit is mapped differently,depending on frame type, as follows:
        ** SF: DL (number of diagnostic bytes NOT including the PCI byte; only the
        **      3 LS bits are used).
        ** FF: XDL (extended data length; always be 0.)
        ** CF: Sequence number,4 bits, max value:15.
        ** FC: Flow Status. The value of FS shall be set to zero that means that
        **     the tester is ready to receive a maximum number of CF.
        */
        private enum Pci /* Don't change thesevalues, these must be  */
        {
            /// <summary>
            /// MS bits -  Frame Type
            /// </summary>
            FrameTypeMask = 0xF0,

            /// <summary>
            /// Single frame
            /// </summary>
            SfTpdu = 0x00,

            /// <summary>
            /// First frame 
            /// </summary>
            FfTpdu = 0x10,

            /// <summary>
            /// Consecutive frame
            /// </summary>
            CfTpdu = 0x20,

            /// <summary>
            /// Flow control frame 
            /// </summary>
            FcTpdu = 0x30,

            /// <summary>
            /// Flow control frame
            /// </summary>
            FcOvflPdu = 0x32,

            /* LS bits - SF_DL */
            /// <summary>
            /// SF Max Data Length
            /// </summary>
            SfDlMaxBytes = 0x07,

            /// <summary>
            /// number diagnostic data bytes 
            /// </summary>
            SfDlMask = 0x07,

            /// <summary>
            /// change to check the 4 bits for testing,number diagnostic data bytes
            /// </summary>
            SfDlMaskLong = 0x0F,

            /* LS bits - FF_DL */
            /// <summary>
            /// Extended data length
            /// </summary>
            FfExDlMask = 0x0F,

            /* LS bits - CF_SN */
            /// <summary>
            /// Sequence number mask
            /// </summary>
            CfSnMask = 0x0F,

            /// <summary>
            /// Max value of sequence number
            /// </summary>
            CfSnMaxValue = 0x0F,

            /* LS bits - FC Saatus */
            /// <summary>
            /// Flow control frame, CONTINUE
            /// </summary>
            FcStatusContinue = 0x00,

            /// <summary>
            /// Flow control frame, WAIT
            /// </summary>
            FcStatusWait = 0x01,

            /// <summary>
            /// Flow control frame, OVERFLOW
            /// </summary>
            FcStatusOverflow = 0x02,

            FcStatusMask = 0x0F,
        };

        private int _nAs = 25;
        private int _nAr = 25;
        private int _nBs = 75;
        private int _nBr;
        private int _nCr = 150;
        private int _nCs;

        private const int FcBsMaxValue = 0;
        private const int FcStMinValue = 5;
        private const int CfSnMaxValue = 15;
        private const int SfDlMaxBytes = 7;

        /// <summary>
        /// ime to wait for the tester to senda FC frame in response to a FF(wait for flow control frametime out).
        /// N_As + N_Bs = 25 +75 = 100ms
        /// </summary>
        private readonly int _fcWaitTimeout; //N_As +N_Bs + 50;

        /// <summary>
        /// wait for Consecutive frame time out
        /// N_Cr <!--<--> 150ms
        /// </summary>
        private readonly int _cfWaitTimeout; // N_Cr; //(N_Cr- 10))

        private const int RxMaxTpBytes = 0xFFF;

        public UdsTrans()
        {
            _canRxInfo.Frame = new byte[8];
            _canTxInfo.Frame = new byte[8];

            _fcWaitTimeout = _nAs + _nBs + 50; /* N_As + N_Bs + 50 */
            _cfWaitTimeout = _nCr; /* (N_Cr - 10)) */
        }

        private readonly int beginning_seq_number = 1;
        private readonly int TPCI_Byte = 0;
        private readonly int DL_Byte = 1;
        private readonly int BS_Byte = 1;
        private readonly int STminByte = 2;

        private class TxInfo
        {
            public bool TxRxIdle = false;
            public bool TxFcTpdu;
            public bool TxLastFrameError;
            public bool TxWaitFc;
            public bool TxInProgress;

            /// <summary>
            /// BS(Block Size) in a flow ControlFrame
            /// </summary>
            public int TxBlockSize;

            public int TxStminTime = 20;

            /// <summary>
            /// STmin Time in Flow Control Frame
            /// </summary>
            public int TxCfStminWaitTime = 20;

            /// <summary>
            /// Wait for FC when has sentFF
            /// </summary>
            public int TxFcWaitTime;

            public int Lenght;
            public int Offset;
            public int NextSeqNum;
            public byte[] Buffer;
            public byte[] Frame;
        }

        private class RxInfo
        {
            public bool RxInProgress;

            /// <summary>
            /// if the message has neverbeen received to be used by application level software
            /// </summary>
            public bool RxMsgRcvd;

            public bool TxAborted;

            public int RxCfWaitTime;

            public bool RxFcWaitTimeoutDisable;

            public bool rx_overflow = false;

            public int Lenght;
            public int Offset;
            public int NextSeqNum;

            public byte[] Buffer;
            public byte[] Frame;
        }

        private readonly TxInfo _canTxInfo = new TxInfo();
        private readonly RxInfo _canRxInfo = new RxInfo();

        #region Event

        public class FarmsEventArgs : EventArgs
        {
            public int Id;
            public int Dlc;
            public byte[] Dat = new byte[8];
            public long Time;

            public override string ToString()
            {
                Time %= 1000000;
                return Id.ToString("X3") + " " + Dlc.ToString("X1") + " " + HexToStrings(Dat) + " " + Time / 1000 + "." + (Time % 1000).ToString("d3");
                // return Id.ToString("X3") + " " + Dlc.ToString("X1") + " " + Dat.HexToStrings("") + " " + (Time / 1000) + "." + (Time % 1000).ToString("d3");
            }
        }

        public class RxMsgEventArgs : EventArgs
        {
            public int Id;
            public byte[] Dat;
            public long Time;

            public RxMsgEventArgs(int lenght)
            {
                Dat = new byte[lenght];
            }

            public override string ToString()
            {
                Time %= 1000000;
                return Id.ToString("X3") + " " + HexToStrings(Dat) + " " + Time / 1000 + "." + (Time % 1000).ToString("d3");
                // return Id.ToString("X3") + " "+ Dat.HexToStrings(" ") + " "+ Time / 1000 + "." + (Time % 1000).ToString("d3");
            }
        }

        public class ErrorEventArgs : EventArgs
        {
            public string Error;

            public override string ToString()
            {
                return Error;
            }
        }

        /// <summary>
        /// UDS 传输层发送一帧事件
        /// </summary>
        public event EventHandler EventTxFarms;

        /// <summary>
        /// UDS 传输层接收一帧事件
        /// </summary>
        public event EventHandler EventRxFarms;

        /// <summary>
        /// UDS 传输层接收完成事件
        /// </summary>
        public event EventHandler EventRxMsgs;

        /// <summary>
        /// UDS 传输层错误事件
        /// </summary>
        public event EventHandler EventError;

        private void TxFarmsEvent(int txid, byte[] dat, int dlc, long time)
        {
            if (EventTxFarms == null)
                return;

            var eArgs = new FarmsEventArgs
            {
                Id = txid,
                Dlc = dlc,
                Time = time
            };

            Array.Copy(dat, eArgs.Dat, dlc);
            EventTxFarms(this, eArgs);
        }

        private void RxFarmsEvent(int rxid, byte[] dat, int dlc, long time)
        {
            if (EventRxFarms == null)
                return;

            var eArgs = new FarmsEventArgs
            {
                Id = rxid,
                Dlc = dlc,
                Time = time
            };

            Array.Copy(dat, eArgs.Dat, dlc);
            EventRxFarms(this, eArgs);
        }

        private void RxMsgEvent(int rxid, byte[] dat)
        {
            if (EventRxMsgs == null)
                return;
            var lenght = dat.Length;
            var eRxMsgArgs = new RxMsgEventArgs(lenght) { Id = rxid };

            Array.Copy(dat, eRxMsgArgs.Dat, lenght);
            EventRxMsgs(this, eRxMsgArgs);
        }

        private void RrrorEvent(string strings)
        {
            if (EventError == null)
                return;

            var eArgs = new ErrorEventArgs { Error = strings };
            EventError(this, eArgs);
        }

        public delegate bool CanWriteData(int id, byte[] dat, int dlc, out long time);
        public delegate bool CanReadData(out int id, ref byte[] dat, out int dlc, out long time);

        /// <summary>
        /// 利用委托发送一帧数据
        /// </summary>
        public CanWriteData WriteData;

        /// <summary>
        /// 利用委托接收一帧数据
        /// </summary>
        public CanReadData ReadData;

        #endregion

        #region Trans Thread

        private Thread _testerPresentThread;

        private void testerPresent_Thread()
        {
            while (true)
            {
                long time;

                if (WriteData != null &&
                    WriteData(
                    TestId, new byte[] { 0x02, 0x3E, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00 }, 8, out time))
                    Thread.Sleep(3000);
            }
        }

        private bool _tester;

        /// <summary>
        /// 诊断保持
        /// </summary>
        public bool TesterPresentCheckd
        {
            set
            {
                _tester = value;
                if (_tester)
                {
                    _testerPresentThread = new Thread(testerPresent_Thread);
                    _testerPresentThread.Start();
                }
                else
                {
                    if (_testerPresentThread != null && _testerPresentThread.IsAlive)
                        _testerPresentThread.Abort();
                }
            }

            get
            {
                return _tester;
            }
        }

        private Thread _transThread;

        private void CanTrans_Thread()
        {
            var oldTime = DateTime.Now.Ticks;
            while (true)
            {
                long cnt;

                var dat = new byte[8];

                while (true)
                {
                    var rxFrame = false;

                    long time;
                    int dlc;
                    int privateId;
                    while (ReadData != null && ReadData(out privateId, ref dat, out dlc, out time))
                    {
                        if (privateId != RxId || dlc != 8) 
                            continue;
                        if (_canRxInfo.RxInProgress && dat[0] == 0x02 && dat[1] == 0x7F && dat[3] == 0x78)
                        {
                            _canRxInfo.RxCfWaitTime = 5000;
                            break;
                        }

                        Array.Copy(dat, _canRxInfo.Frame, 8);
                        RxFarmsEvent(privateId, dat, dlc, time);
                        rxFrame = true;
                        break;
                    }

                    var nowTime = DateTime.Now.Ticks;
                    cnt = nowTime - oldTime;

                    if (cnt > 10000 || rxFrame)
                    {
                        oldTime = nowTime;
                        break;
                    }
                    if (!_canTxInfo.TxInProgress && !_canRxInfo.RxInProgress) // UDS空闲，释放进程
                        Thread.Sleep(1);
                }

                CanTrans_Manage((int)(cnt + 5000) / 10000);
            }
        }

        /// <summary>
        /// UDS 传输层开启
        /// </summary>
        public void Start()
        {
            _transThread = new Thread(CanTrans_Thread) { Priority = ThreadPriority.Highest };
            _transThread.Start();
        }

        /// <summary>
        /// UDS 传输层关闭
        /// </summary>
        public void Stop()
        {
            if (_transThread != null && _transThread.IsAlive)
                _transThread.Abort();

            TesterPresentCheckd = false;
        }

        #endregion

        private byte[] _txMsg = new byte[0];

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <returns></returns>
        public bool CanTrans_TxMsg(AddressingModes mode, byte[] msg)
        {
            if (msg.Length == 0)
            {
                RrrorEvent("-->Error:Tx Msg Length Is Zero");
                return false;
            }

            if (msg.Length > RxMaxTpBytes - 2)
            {
                RrrorEvent("-->Error:Tx Msg Length > RX_MAX_TP_BYTES");
                return false;
            }

            if (_txMsg.Length != 0)
            {
                RrrorEvent("-->Error:Tx Msg ing");
                return false;
            }

            id = mode == AddressingModes.PhysicalAddressing ? TxId : TestId;

            _txMsg = msg;
            _txMsg = new byte[msg.Length];
            Array.Copy(msg, _txMsg, msg.Length);

            return true;
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <returns></returns>
        public bool CanTrans_TxMsg(AddressingModes mode, string strings)
        {
            return CanTrans_TxMsg(mode, StringToHex(strings));
            // return CanTrans_TxMsg(mode, strings.StringToHex());
        }

        private void CanTrans_TxMsg()
        {
            if (_txMsg.Length == 0)
                return;

            /*
            ** Set the tx_in_progress bit...itwill be cleared when TX is done.
            */
            _canTxInfo.TxInProgress = true;
            _canTxInfo.TxLastFrameError = false;

            /*
            ** Assign fields in the controlstructure to initiate TX, then TX the
            ** appropriate frame type.
            */
            _canTxInfo.Offset = 0;
            _canTxInfo.Lenght = _txMsg.Length;
            _canTxInfo.Buffer = new byte[_txMsg.Length];

            Array.Copy(_txMsg, _canTxInfo.Buffer, _canTxInfo.Lenght);
            _canTxInfo.Offset = 0;
            _txMsg = new byte[0];

            CanTrans_TxFrame(_canTxInfo.Lenght <= SfDlMaxBytes ? FrameType.TxSf : FrameType.TxFf);
        }

        private void CanTrans_TxFrame(FrameType frameType)
        {
            var txFarmeIndex = 0;

            if (_canTxInfo.TxLastFrameError == false)
            {
                _canTxInfo.Frame = new[] { FillByte, FillByte, FillByte, FillByte, FillByte, FillByte, FillByte, FillByte };

                /*
                ** Place control bytes into theframe.
                */
                int txDataBytes;
                switch (frameType)
                {
                    case FrameType.TxSf: /*single frame */
                        _canTxInfo.Frame[TPCI_Byte] = (byte)((byte)Pci.SfTpdu | _canTxInfo.Lenght);
                        txDataBytes = _canTxInfo.Lenght;
                        txFarmeIndex = 1;
                        break;

                    case FrameType.TxFf: /* first frame */
                        _canTxInfo.Frame[TPCI_Byte] = (byte)((byte)Pci.FfTpdu | (_canTxInfo.Lenght >> 8) & 0x0F);
                        _canTxInfo.Frame[DL_Byte] = (byte)(_canTxInfo.Lenght & 0xFF);
                        txDataBytes = SfDlMaxBytes - 1;
                        txFarmeIndex = 2;
                        _canTxInfo.NextSeqNum = beginning_seq_number;
                        _canRxInfo.RxFcWaitTimeoutDisable = false;
                        break;

                    case FrameType.TxCf: /*conscutive frame */
                        _canTxInfo.Frame[TPCI_Byte] = (byte)((byte)Pci.CfTpdu | _canTxInfo.NextSeqNum);
                        txFarmeIndex = 1;
                        txDataBytes = _canTxInfo.Lenght - _canTxInfo.Offset;
                        if (txDataBytes > SfDlMaxBytes)
                        {
                            txDataBytes = SfDlMaxBytes;
                        }
                        _canTxInfo.NextSeqNum = (_canTxInfo.NextSeqNum + 1) % (CfSnMaxValue + 1);
                        break;

                    case FrameType.TxFc: /*single frame */
                        if (_canRxInfo.rx_overflow)
                        {
                            _canTxInfo.Frame[TPCI_Byte] = (byte)Pci.FcOvflPdu;
                        }
                        else
                        {
                            _canTxInfo.Frame[TPCI_Byte] = (byte)Pci.FcTpdu;
                        }

                        _canTxInfo.Frame[BS_Byte] = FcBsMaxValue;
                        _canTxInfo.Frame[STminByte] = FcStMinValue;
                        txDataBytes = 0;
                        break;

                    default:
                        return;
                }

                while (txDataBytes != 0)
                {
                    _canTxInfo.Frame[txFarmeIndex++] = _canTxInfo.Buffer[_canTxInfo.Offset++];
                    txDataBytes--;
                }
            }

            long time;

            if (WriteData != null && WriteData(id, _canTxInfo.Frame, 8, out time))
            {
                TxFarmsEvent(id, _canTxInfo.Frame, 8, time);
                _canTxInfo.TxLastFrameError = false;
                _canRxInfo.Frame[TPCI_Byte] = 0;

                /*
                ** Verify if the data has beencompletely transfered. If not, set flag to
                ** transfer CF frames. (For FCframes, s_cantp_tx_info is not used and there
                ** should not be a CF frameafter a FC frame.)
                */
                if (_canTxInfo.Lenght > _canTxInfo.Offset && frameType != FrameType.TxFc)
                {
                    _canTxInfo.TxInProgress = true;

                    if (frameType == FrameType.TxFf)
                    {
                        _canTxInfo.TxWaitFc = true;
                        _canTxInfo.TxFcWaitTime = _fcWaitTimeout; /* start flow controlwait timer */
                    }
                }
                else
                {
                    _canTxInfo.TxInProgress = false;
                }
            }
            else
            {
                /* user specific action incasetransmission request is not successful */
                _canTxInfo.TxLastFrameError = true;
            }
        }

        private void CanTrans_Manage(int tick)
        {
            CanTrans_TxMsg();
            CanTrans_Counter(tick);

            /*
            ** If new message has beenreceived, process it.
            */
            if (_canRxInfo.Frame[TPCI_Byte] != 0)
            {
                CanTrans_RxStateAnalyse();

                /*clear first rx frame byte tocheck a new frame next time*/
                _canRxInfo.Frame[TPCI_Byte] = 0;
            }

            if (_canTxInfo.TxInProgress && !_canTxInfo.TxWaitFc)
            {
                if (0x00 == _canTxInfo.TxBlockSize)
                {
                    /* st_min time, receivedfrom tester*/
                    if (0x00 == _canTxInfo.TxStminTime)
                    {
                        CanTrans_TxFrame(FrameType.TxCf);
                    }
                    else
                    {
                        /* st_min time,received from tester is not 0 */
                        if (0x00 == _canTxInfo.TxCfStminWaitTime)
                        {
                            CanTrans_TxFrame(FrameType.TxCf);
                            _canTxInfo.TxCfStminWaitTime = _canTxInfo.TxStminTime;
                        }
                    }
                }
                else if (_canTxInfo.TxBlockSize > 1)
                {
                    if (0x00 == _canTxInfo.TxStminTime)
                    {
                        CanTrans_TxFrame(FrameType.TxCf);
                        if (!_canTxInfo.TxLastFrameError)
                        {
                            _canTxInfo.TxBlockSize--;
                        }
                    }
                    else
                    {
                        if (0x00 == _canTxInfo.TxCfStminWaitTime)
                        {
                            CanTrans_TxFrame(FrameType.TxCf);
                            if (!_canTxInfo.TxLastFrameError)
                            {
                                _canTxInfo.TxBlockSize--;
                            }
                            /* start stmintime,interval of consecutive frame */
                            _canTxInfo.TxCfStminWaitTime = _canTxInfo.TxStminTime;
                        }
                    }

                    if (_canTxInfo.TxBlockSize <= 1)
                    {
                        _canTxInfo.TxWaitFc = true;

                        /* start flow controlwait timer */
                        _canTxInfo.TxFcWaitTime = _fcWaitTimeout;
                    }
                }
            }
            else if (_canTxInfo.TxFcTpdu)
            {
                CanTrans_TxFrame(FrameType.TxFc);
                _canTxInfo.TxFcTpdu = false;

                /*start to counter the CF waittime*/
                _canRxInfo.RxCfWaitTime = _cfWaitTimeout;
            }

            if (_canTxInfo.TxInProgress && _canTxInfo.TxWaitFc)
            {
                /* wait for flow control frametime out! */
                if (_canTxInfo.TxFcWaitTime == 0)
                {
                    _canTxInfo.TxInProgress = false;
                    _canTxInfo.TxWaitFc = false;
                    _canTxInfo.TxLastFrameError = false;
                    RrrorEvent("-->Error: Wait For FlowControl Frame Time Out");
                }
            }

            if (_canRxInfo.RxInProgress && !_canTxInfo.TxFcTpdu)
            {
                if (0x00 == _canRxInfo.RxCfWaitTime)
                {
                    _canRxInfo.TxAborted = true;

                    /*
                    ** wait for consecutiveframe Time out,abort Rx.
                    */
                    _canRxInfo.RxInProgress = false;

                    /*
                    ** When Time out occurs,ECU has to send negative
                    ** resp(71) for the firstframe.First frame is already copied in to
                    ** g_cantp_can_rx_info.msgbuffer but message length is not yet copied.
                    ** So assign data length asFirst Frame length and set RX_MSG_RCVD
                    ** flag.This flag indicatesto a new message has come.
                    */

                    _canRxInfo.Lenght = SfDlMaxBytes - 1;
                    _canRxInfo.RxMsgRcvd = true;

                    RrrorEvent("-->Error: Ecu Tx Aborted");
                }
            }

            if (_canRxInfo.RxMsgRcvd)
            {
                _canRxInfo.RxMsgRcvd = false;

                if (_canRxInfo.TxAborted == false)
                {
                    RxMsgEvent(RxId, _canRxInfo.Buffer);
                }
                _canRxInfo.TxAborted = false;
            }
        }

        private void CanTrans_Counter(int tick)
        {
            /* interval of consecutive frame,STmin = 10ms, separation time */
            if (_canTxInfo.TxCfStminWaitTime > 0)
                if (_canTxInfo.TxCfStminWaitTime > tick)
                    _canTxInfo.TxCfStminWaitTime -= tick;
                else
                    _canTxInfo.TxCfStminWaitTime = 0;

            /* N_Bs, flow control frame waittime out, 75ms*/
            if (_canTxInfo.TxFcWaitTime > 0)
            {
                if (_canTxInfo.TxFcWaitTime > tick)
                    _canTxInfo.TxFcWaitTime -= tick;
                else
                {
                    _canTxInfo.TxFcWaitTime = 0;
                    _canRxInfo.RxFcWaitTimeoutDisable = true;
                }
            }

            /* N_Cr,consecutive frame wait timeout, 75ms*/
            if (_canRxInfo.RxCfWaitTime > tick)
                _canRxInfo.RxCfWaitTime -= tick;
            else
                _canRxInfo.RxCfWaitTime = 0;
        }

        private void CanTrans_RxStateAnalyse()
        {
            int dataLength;

            /* single frame */
            if ((_canRxInfo.Frame[TPCI_Byte] & (byte)Pci.FrameTypeMask) == (byte)Pci.SfTpdu)
            {
                /* As per 15765-2 network layerspec when SF_DL is 0 or greater
                ** than 7, just ignore it.
                */
                dataLength = _canRxInfo.Frame[TPCI_Byte] & (byte)Pci.SfDlMaskLong;
                _canTxInfo.TxInProgress = false;
                _canTxInfo.TxWaitFc = false;
                _canRxInfo.RxInProgress = false;
                _canTxInfo.TxLastFrameError = false;

                if ((dataLength == 0) || (dataLength > SfDlMaxBytes))
                {
                    return;
                }

                _canRxInfo.Lenght = dataLength;
                _canRxInfo.Buffer = new byte[_canRxInfo.Lenght];

                /*
                ** Copy the frame to the RXbuffer. Clear the RX_IN_PROGRESS bit
                ** (SF frame) will abortmulti-frame transfer.
                */

                Array.Copy(_canRxInfo.Frame, 1, _canRxInfo.Buffer, 0, _canRxInfo.Lenght);
                _canRxInfo.RxMsgRcvd = true;
            }
            /* first frame */
            else if ((_canRxInfo.Frame[TPCI_Byte] & (byte)Pci.FrameTypeMask) == (byte)Pci.FfTpdu)
            {
                dataLength = ((_canRxInfo.Frame[TPCI_Byte] & (byte)Pci.FfExDlMask) << 8)
                                         + _canRxInfo.Frame[DL_Byte];
                _canRxInfo.RxFcWaitTimeoutDisable = false;
                _canRxInfo.Lenght = dataLength;
                _canRxInfo.Buffer = new byte[_canRxInfo.Lenght];

                /*
                ** Clear the RX buffer, copyfirst frame to RX buffer and initiate RX.
                */
                Array.Copy(_canRxInfo.Frame, 2, _canRxInfo.Buffer, 0, SfDlMaxBytes - 1);
                _canRxInfo.NextSeqNum = beginning_seq_number;
                _canRxInfo.Offset = SfDlMaxBytes - 1;

                _canTxInfo.TxInProgress = false;
                _canTxInfo.TxWaitFc = false;
                _canRxInfo.RxInProgress = true;

                /* set flag to send flowcontrol frame */
                _canTxInfo.TxFcTpdu = true;
            }
            /* Consecutive Frame */
            else if (((_canRxInfo.Frame[TPCI_Byte] & (byte)Pci.FrameTypeMask) == (byte)Pci.CfTpdu)
                /* Don't accept consecutiveframe until flow control frame sent by ECU */
                     && !_canTxInfo.TxFcTpdu
                /* Don't accept consecutiveframe if we are sending CF*/
                     && !_canTxInfo.TxInProgress)
            {
                /*
                ** Ignore frame unless RX inprogress.
                */
                if (_canRxInfo.RxInProgress)
                {
                    /*
                    ** Verify the sequencenumber is as expected.
                    */
                    if ((_canRxInfo.Frame[TPCI_Byte] & (byte)Pci.CfSnMask) == _canRxInfo.NextSeqNum)
                    {
                        dataLength = _canRxInfo.Lenght - _canRxInfo.Offset;

                        /*
                        **  Last frame in message?
                        */

                        if (dataLength <= SfDlMaxBytes)
                        {
                            Array.Copy(_canRxInfo.Frame, 1, _canRxInfo.Buffer, _canRxInfo.Offset, dataLength);
                            _canRxInfo.RxInProgress = false;
                            _canRxInfo.RxMsgRcvd = true;
                        }
                        else
                        {
                            /*
                            ** not the lastframe,copy bytes to RX buffer and
                            ** continue RXing.
                            */

                            Array.Copy(_canRxInfo.Frame, 1, _canRxInfo.Buffer, _canRxInfo.Offset, SfDlMaxBytes);
                            _canRxInfo.NextSeqNum = (_canRxInfo.NextSeqNum + 1) % (CfSnMaxValue + 1);
                            _canRxInfo.Offset += SfDlMaxBytes;
                            _canRxInfo.RxCfWaitTime = _cfWaitTimeout;
                        }
                    }
                    else
                    {
                        /*
                        ** Invalid sequence number...abort Rx.Asa diagnostic measure,
                        ** consideration wasgiven to send an FC frame here, but not done.
                        */
                        _canRxInfo.RxInProgress = false;

                        /*
                        ** When Invalidsequence number is received, ECU has to send
                        ** negative resp forthe first frame.so set RX_MSG_RCVD flag.
                        ** This flag indicatesto DiagManager as new message has come.
                        */

                        _canRxInfo.TxAborted = true;
                        _canRxInfo.RxMsgRcvd = true;
                        RrrorEvent("-->Error: Ecu Invalid Sequence Number");
                    }
                }
            }
            /* flow control frame */
            else if ((_canRxInfo.Frame[TPCI_Byte] & (byte)Pci.FrameTypeMask) == (byte)Pci.FcTpdu)
            {
                if (_canTxInfo.TxWaitFc)
                {
                    /*                    
                       ** Receive Flow Status(FS) forTransmiting the CF Frames.
                       ** The value of FS shall be setto zero that means that the
                       ** tester is ready to receive amaximum number of CF.
                       */

                    var flowControlSts = Pci.FcStatusContinue;

                    if ((_canRxInfo.Frame[TPCI_Byte] & (byte)Pci.FcStatusMask) != 0x00)
                    {
                        /* Flow Status(FS)
                           ** 0: Continue to send(CTS)
                           ** 1: wait(WT)
                           ** 2: Overflow(OVFLW)
                           */

                        flowControlSts = (Pci)(_canRxInfo.Frame[TPCI_Byte] & (byte)Pci.FcStatusMask);
                    }

                    /*
                    ** Receive the BS and STmin time for Transmiting the CF Frames.
                    */
                    if (_canRxInfo.Frame[BS_Byte] != 0x00)
                    {
                        _canTxInfo.TxBlockSize = _canRxInfo.Frame[BS_Byte] + 1;
                    }
                    else
                    {
                        _canTxInfo.TxBlockSize = 0x00;
                    }

                    if ((_canRxInfo.Frame[STminByte] & 0x7F) != 0x00)
                    {
                        /*
                        ** Valid Range forSTMin timeout is 0 - 127ms.
                        */

                        _canTxInfo.TxStminTime = (_canRxInfo.Frame[STminByte] & 0x7F) + 5;   /* extend the delay time */
                    }
                    else
                    {
                        _canTxInfo.TxStminTime = 20;
                    }

                    if ((flowControlSts == Pci.FcStatusContinue) && (_canRxInfo.RxFcWaitTimeoutDisable == false))
                    {
                        _canTxInfo.TxWaitFc = false;
                        _canTxInfo.TxFcWaitTime = 0;
                    }
                    else if (flowControlSts == Pci.FcStatusWait)
                    {
                        _canTxInfo.TxFcWaitTime = _fcWaitTimeout;  /* if wait, we will wait another time */
                    }
                    else if (flowControlSts == Pci.FcStatusOverflow)
                    {
                        /* do nothing here, ifover flow, we will stop sending
                           any message until wegot new cmd */
                        _canTxInfo.TxFcWaitTime = 1;   /* exit after 10ms */
                        RrrorEvent("-->Error: Ecu Buff Over Flow");
                    }
                    else
                    {
                        /* do nothing here, ifover flow, we will stop sending                        
                           any message until wegot new cmd */
                        _canTxInfo.TxFcWaitTime = 1;  /* exit after 10ms */
                    }
                }
            }
        }

        private static byte[] StringToHex(string value)
        {
            var temp = value.Replace(" ", "");
            if (temp.Length % 2 != 0)
                return new byte[0];

            var returnBs = new List<byte>();
            for (var i = 0; i < temp.Length; i = i + 2)
                returnBs.Add(Convert.ToByte(string.Format("{0}{1}", temp[i], temp[i + 1]), 16));
            return returnBs.ToArray();
        }

        private static string HexToStrings(byte[] value)
        {
            return value == null
                ? string.Empty
                : value.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
        }
    }
}
