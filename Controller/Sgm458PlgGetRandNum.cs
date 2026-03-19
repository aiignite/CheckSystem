using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,SGM458PLG随机数获取")]
    public sealed class Sgm458PlgGetRandNum : ControllerBase
    {
        public CanBus Can;

        [Description("R,获取到的随机数-16进制格式")]
        public string GetRandNumHex;

        [Description("R,获取到的随机数-ASCII格式")]
        public string GetRandNumAscii;

        public Sgm458PlgGetRandNum(string name)
            : base(name)
        {
            if (_keepNetworkThread != null)
            {
                _keepNetworkThread.Abort();
                _keepNetworkThread.Join();
            }

            _keepNetworkThread = new Thread(KeepNetwork) { IsBackground = true };
            _keepNetworkThread.Start();
        }

        ~Sgm458PlgGetRandNum()
        {
            Dispose();
        }

        private readonly Thread _keepNetworkThread;
        private readonly object _lockSend = new object();
        private bool _isSleep = true;

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
                       0x621, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
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

        [Description("获取随机数")]
        public void GetRandNum()
        {
            GetRandNumAscii = string.Empty;
            GetRandNumHex = string.Empty;

            var getBytes = GetRandNumInternal();
            if (getBytes != null && getBytes.Count == 32)
            {
                GetRandNumHex = ValueHelper.GetHextStr(getBytes.ToArray()).Replace(" ", "");
                GetRandNumAscii = Encoding.ASCII.GetString(getBytes.ToArray());
            }
        }

        private List<byte> GetRandNumInternal()
        {
            if (Can == null)
                return new List<byte>();

            const uint reqCanId = (uint)0x14DAA4F1;
            const uint recvCanId = (uint)0x14DAF1A4;

            var randNumBytes = new List<byte>();

            for (var i = 0; i < 3; i++)
            {
                Thread.Sleep(50);
                if (Can.CanBusWithUds.TryEnterExtendedSession(reqCanId, recvCanId, CanBus.CanType.Extended))
                {
                    Thread.Sleep(50);
                    byte[] echoBytes;
                    if (Can.CanBusWithUds.TryRequestSeed(reqCanId, recvCanId, CanBus.CanType.Extended, 0x01, out echoBytes))
                    {
                        if (echoBytes != null && echoBytes.Length == 31)
                        {
                            for (var j = 16; j < echoBytes.Length; j++)
                            {
                                randNumBytes.Add(echoBytes[j]);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            if (randNumBytes.Count != 15 * 3)
                return new List<byte>();

            var returnBytes = new byte[32];
            Array.Copy(randNumBytes.ToArray(), 0, returnBytes, 0, returnBytes.Length);

            return returnBytes.ToList();
        }
    }
}
