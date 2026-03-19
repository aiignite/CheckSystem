using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,五菱控制器")]
    public sealed class WuLingController : ControllerBase
    {
        public CanBus TesterCan;

        public WuLingController(string name)
            : base(name)
        {
            for (var i = 0x721; i < 0x730; i++)
                _tms_can.Add((uint)i);
            CanBus.PushCanMsg += CanBus_PushCanMsg;

            if (_networkThread != null)
            {
                _networkThread.Abort();
                _networkThread.Join();
            }
            _networkThread = new Thread(Can1NetWork);
            _networkThread.Start();
        }

        ~WuLingController() => Dispose();

        private readonly List<uint> _tms_can = new List<uint>();

        private void CanBus_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (TesterCan != null && TesterCan.Name == name)
            {
                if ((onPushCanDataType is CanBus.OnPushCanDataType.FilterRx || onPushCanDataType is CanBus.OnPushCanDataType.NotFilterRx) &&
                    _tms_can.Contains(data.CanId) && data.CanDataLen >= 8)
                {
                    _recvTs = HighPrecisionTimer.GetTimestamp();

                    switch (data.CanId)
                    {
                        case 0x721:
                            FrEvprTemSens_Value = (int)FormatBits(data.CanData, 13, 11);
                            break;

                        case 0x722:
                            break;

                        case 0x723:
                            break;

                        case 0x724:
                            break;

                        case 0x725:
                            break;

                        case 0x726:
                            break;

                        case 0x727:
                            break;

                        case 0x728:
                            break;

                        case 0x729:
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private readonly object _can1Locker = new object();
        private int _periodCount;
        private bool _isSleeping = true;
        private readonly Thread _networkThread;
        private long _recvTs;
        private const long _timeOutMs = 3000;

        private byte[] Tester_00 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        private void Can1NetWork()
        {
            while (_networkThread.IsAlive)
            {
                if (!_networkThread.IsAlive)
                    break;

                Thread.Sleep(20);

                var ns = HighPrecisionTimer.GetTimestamp();
                IsCanOffline = HighPrecisionTimer.GetTimestampIntervalMs(_recvTs, ns) > _timeOutMs;

                _periodCount++;

                if (_periodCount > 50)
                    _periodCount = 0;

                try
                {
                    if (TesterCan == null)
                        continue;

                    lock (_can1Locker)
                    {
                        if (!_isSleeping)
                        {
                            var lstPages = new List<CanBus.CanDataPackage>
                            {
                                new CanBus.CanDataPackage(
                                    0x701,
                                    CanBus.CanProtocol.Can,
                                    CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data,
                                    Tester_00)
                            };

                            TesterCan.SendCanDatas(lstPages.ToArray());
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        #region 五菱实验箱接收数据

        [Description("R,CAN接收异常")]
        public bool IsCanOffline;

        [Description("R,前蒸温度传感器")]
        public int FrEvprTemSens_Value = int.MinValue;

        private static uint FormatBits(byte[] bytes, int startBit, int len)
        {
            bool[] bits = new bool[len];
            int currentByte = startBit / 8;
            int currentBitInByte = startBit % 8;
            for (int i = 0; i < len; i++)
            {
                bits[i] = ((bytes[currentByte] >> currentBitInByte) & 1) == 1;
                currentBitInByte++;
                if (currentBitInByte >= 8)
                {
                    currentByte--;
                    currentBitInByte = 0;
                }
            }

            Array.Reverse(bits);
            ulong result = 0;

            for (int i = 0; i < len; i++)
            {
                if (bits[i])
                {
                    result |= (1UL << (len - 1 - i));
                }
            }

            return (uint)result;
        }

        #endregion

        #region 五菱实验箱指令发送

        [Description("打开CAN消息发送")]
        public void StartCan() => _isSleeping = false;

        [Description("关闭CAN消息发送")]
        public void StopCan() => _isSleeping = true;

        [Description("EXV1步数请求")]
        public void WriteEXV1Step(string value)
        {
            uint EXV1StepNew = Convert.ToUInt32(value);
            if (EXV1StepNew >= 0 && EXV1StepNew <= 1000)
                FormatBytes(ref Tester_00, 46, 10, EXV1StepNew);
        }

        [Description("EXV2步数请求")]
        public void WriteEXV2Step(string value)
        {
            uint EXV2StepNew = Convert.ToUInt32(value);
            if (EXV2StepNew >= 0 && EXV2StepNew <= 1000)
                FormatBytes(ref Tester_00, 52, 10, EXV2StepNew);
        }

        private static void FormatBytes(ref byte[] refBytes, int startBit, int len, uint value)
        {
            var originData = new byte[refBytes.Length];
            Array.Copy(refBytes, 0, originData, 0, originData.Length);

            try
            {
                const int horizontal = 8;
                var vertical = refBytes.Length;
                var strs = new string[vertical, horizontal];

                for (var i = 0; i < vertical; i++)
                {
                    var bChars =
                        Convert.ToString(refBytes[i], 2).PadLeft(8, '0');
                    for (var j = 7; j >= 0; j--)
                        strs[i, 8 - j - 1] = bChars[j].ToString();
                }

                var valBools =
                    Convert.ToString(value, 2).PadLeft(len, '0').ToCharArray();
                Array.Reverse(valBools);

                var mV = startBit / 8;
                var mH = startBit;
                var mI = 0;
                do
                {
                    strs[mV, mH % 8] =
                        valBools[mI].ToString();
                    mH++;
                    mI++;
                    if (mH % 8 != 0)
                        continue;
                    mH = mH - 16;
                    mV--;
                } while (mI < len);

                for (var i = 0; i < vertical; i++)
                {
                    var tempList = new List<string>();
                    for (var j = 0; j < horizontal; j++)
                        tempList.Add(strs[i, j]);
                    var tempArray = tempList.ToArray();
                    Array.Reverse(tempArray);
                    var bsStr = tempArray.Aggregate(
                        string.Empty, (current, t) => current + t.ToString());
                    refBytes[i] = Convert.ToByte(bsStr, 2);
                }
            }
            catch (Exception)
            {
                Array.Copy(originData, 0, refBytes, 0, originData.Length);
            }
        }

        #endregion
    }
}
