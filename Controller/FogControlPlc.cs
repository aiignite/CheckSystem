using CommonUtility;
using HslCommunication.ModBus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    public sealed class FogControlPlc : ControllerBase
    {
        [Description("R/W,40001检测完成")]
        public bool PcWriteCheckEnd;
        [Description("R/W,40002检测结果OK")]
        public bool PcWriteCheckOk;
        [Description("R/W,40003检测结果NG")]
        public bool PcWriteCheckNg;
        [Description("R/W,40004检测模组同步")]
        public bool PcWriteServoRun;
        [Description("R/W,40005扫码完成")]
        public bool PcWriteBarcodeScanEnd;
        [Description("R/W,40006当前产品编号")]
        public int PcWriteProductType;
        [Description("R/W,40007~40008检测模组X位置")]
        public float PcWriteServoXPos;
        [Description("R/W,40009~40010检测模组Z位置")]
        public float PcWriteServoZPos;

        [Description("R,400121检测启动")]
        public bool PcReadCheckStart;
        [Description("R,400122检测模组到位")]
        public bool PcReadServoRunEnd;
        [Description("R,400123扫码启动")]
        public bool PcReadBarcodeScanStart;
        [Description("R,400125~400126照度计寄存器")]
        public float PcReadIlluminometer;

        public bool PreStart;

        public FogControlPlc(string name) :
            base(name)
        {
        }

        ~FogControlPlc() => Dispose();

        private ModbusTcpNet _modbusTcpNet;
        private Thread _daemonTh;

        public void InitRemoteIpAddress(string ipport)
        {
            //连接到siemens PLC的socket server上
            var split = ipport.Split(':');
            var ip = split[0];
            var port = Convert.ToInt32(split[1]);
            _modbusTcpNet = new ModbusTcpNet(ip, port) { ReceiveTimeOut = 250 };
        }

        public void CycleUpdate()
        {
            if (_daemonTh != null)
            {
                _daemonTh.Abort();
                _daemonTh.Join();
            }

            _daemonTh = new Thread(Daemon) { IsBackground = true };
            _daemonTh.Start();
        }

        private void Daemon()
        {
            _modbusTcpNet.ConnectServer();

            while (_daemonTh.IsAlive)
            {
                if (!_daemonTh.IsAlive)
                    break;

                Thread.Sleep(10);

                var writeBs = new List<byte>
                {
                    0x00,
                    PcWriteCheckEnd ? (byte)0x01 : (byte)0x00,

                    0x00,
                    PcWriteCheckOk ? (byte)0x01 : (byte)0x00,

                    0x00,
                    PcWriteCheckNg ? (byte)0x01 : (byte)0x00,

                    0x00,
                    PcWriteServoRun ? (byte)0x01 : (byte)0x00,

                    0x00,
                    PcWriteBarcodeScanEnd ? (byte)0x01 : (byte)0x00,

                    0x00,
                    (byte)PcWriteProductType
                };

                var servoXPos = BitConverter.GetBytes(PcWriteServoXPos).Reverse().ToList();
                writeBs.Add(servoXPos[2]);
                writeBs.Add(servoXPos[3]);
                writeBs.Add(servoXPos[0]);
                writeBs.Add(servoXPos[1]);

                var servoZPos = BitConverter.GetBytes(PcWriteServoZPos < 70 ? 70 : PcWriteServoZPos).Reverse().ToList();
                writeBs.Add(servoZPos[2]);
                writeBs.Add(servoZPos[3]);
                writeBs.Add(servoZPos[0]);
                writeBs.Add(servoZPos[1]);

                _modbusTcpNet.Write("0", writeBs.ToArray());

                Thread.Sleep(10);

                var readBs = _modbusTcpNet.Read("120", 7);

                if (readBs.IsSuccess)
                {
                    PcReadCheckStart = readBs.Content[1] == 0x01;
                    PcReadServoRunEnd = readBs.Content[3] == 0x01;
                    PcReadBarcodeScanStart = readBs.Content[5] == 0x01;

                    PcReadIlluminometer = (readBs.Content[8] * 256f + readBs.Content[9]) * 65536f + readBs.Content[10] * 256f + readBs.Content[11];

                    PreStart = readBs.Content[13] == 0x01;
                }
            }
        }

        #region 扫码

        public string BarcodeResult = string.Empty;
        public string BarcodeTriggerCmd = @"T";
        public bool IsReadingBarcode;
        private MyAsyncSocketClient BarcodeScaner { get; set; }
        private readonly EventWaitHandle _barcodeReadWait = new AutoResetEvent(false);
        private string KeyAndKeyindexAndLen { get; set; }

        private readonly Queue<string> _barcodeScanList = new Queue<string>();

        public void ConnectBarcode(string ipPort)
        {
            var split = ipPort.Split(':');
            var ipAddressStr = split[0];
            var port = Convert.ToInt32(split[1]);
            BarcodeScaner = new MyAsyncSocketClient();
            BarcodeScaner.InitSocket(ipAddressStr, port);
            BarcodeScaner.OnPushSocketsToTcpClient += _barcodeScaner_OnPushSocketsToTcpClient;
        }

        private void _barcodeScaner_OnPushSocketsToTcpClient(BaseSocket sockets)
        {
            if (sockets == null || sockets.RecBuffer == null)
                return;

            if (sockets.RecBuffer.Length == 0)
                return;

            if (!IsReadingBarcode)
                return;

            if (string.IsNullOrEmpty(KeyAndKeyindexAndLen))
                return;

            try
            {
                var buffer = new byte[sockets.Offset];
                Array.Copy(sockets.RecBuffer, 0, buffer, 0, sockets.Offset);

                var sp = KeyAndKeyindexAndLen.Split(':');
                var key = sp[0];
                var keyIndex = Convert.ToInt32(sp[1]);
                var len = Convert.ToInt32(sp[2]);

                var temp = Encoding.ASCII.GetString(buffer, 0, buffer.Length);

                if (string.IsNullOrEmpty(temp))
                    return;

                var findKeyIndex = temp.LastIndexOf(key, StringComparison.Ordinal);
                if (findKeyIndex == -1)
                    return;

                var barcodeResult = temp.Substring(findKeyIndex - keyIndex, len);
                _barcodeScanList.Enqueue(barcodeResult);
                IsReadingBarcode = false;
                _barcodeReadWait.Set();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void DequeueBarocde()
        {
            BarcodeResult = string.Empty;

            if (_barcodeScanList.Any())
                BarcodeResult = _barcodeScanList.Dequeue();
        }

        private Thread TriggerTh { get; set; }

        public void ReadBarcode(string format)
        {
            BarcodeResult = string.Empty;
            KeyAndKeyindexAndLen = format;
            IsReadingBarcode = true;

            if (TriggerTh != null)
            {
                TriggerTh.Abort();
                TriggerTh.Join();
            }

            if (BarcodeScaner == null)
                return;

            TriggerTh = new Thread(() =>
            {
                while (true)
                {
                    if (!IsReadingBarcode)
                        break;
                    BarcodeScaner.SendData(Encoding.ASCII.GetBytes(BarcodeTriggerCmd));
                    _barcodeReadWait.WaitOne(1000);
                }
            });
            TriggerTh.Start();
        }


        #endregion
    }
}
