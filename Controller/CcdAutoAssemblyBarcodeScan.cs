using CommonUtility;
using System;
using System.Text;
using System.Threading;

namespace Controller
{
    public sealed class CcdAutoAssemblyBarcodeScan : ControllerBase
    {
        public CcdAutoAssemblyBarcodeScan
            (string name) : base(name)
        {
        }

        ~CcdAutoAssemblyBarcodeScan() { }

        public string LoadPlateBarcode = string.Empty;
        public string BarcodeTriggerCmd = @"T";
        public bool IsReadingBarcode;
        public bool ReadingBarcodeNG;
        private MyAsyncSocketClient BarcodeScaner { get; set; }
        private readonly EventWaitHandle _barcodeReadWait = new AutoResetEvent(false);
        private string KeyAndKeyindexAndLen { get; set; }

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

                LoadPlateBarcode = temp.Trim();

                IsReadingBarcode = false;
                _barcodeReadWait.Set();
            }
            catch (Exception)
            {

            }
        }

        private Thread TriggerTh { get; set; }

        public void ReadBarcode(string format)
        {
            LoadPlateBarcode = string.Empty;
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
                for (var i = 0; i < 3; i++)
                {
                    if (!IsReadingBarcode)
                        break;
                    BarcodeScaner.SendData(Encoding.ASCII.GetBytes(BarcodeTriggerCmd));
                    _barcodeReadWait.WaitOne(1000);
                }
            });
            TriggerTh.Start();
        }
    }
}
