using CommonUtility;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace Controller
{
    public sealed class BarcodeScanReaderDlpEol : ControllerBase
    {
        private readonly object _locker = new object();
        private MySerialPort _mySerialPort;
        public bool IsInUsing;
        public string GetBarcodeStr = string.Empty;
        public string GetBarcodeStr1 = string.Empty;

        public bool LockBarcode;
        public int GetBarcodeLength = 0;

        public BarcodeScanReaderDlpEol(string name)
            : base(name) { }

        ~BarcodeScanReaderDlpEol() => Dispose();

        public void ConnectBarcodeScanner(string protocolValue)
        {
            try
            {
                if (protocolValue.StartsWith("COM"))
                {
                    var port = protocolValue.Split(':')[0];
                    var baudTate = protocolValue.Split(':')[1];
                    _mySerialPort =
                        new MySerialPort(port, Convert.ToInt32(baudTate), Parity.None, 8, StopBits.One);
                    _mySerialPort.MyOpen();
                }
                else
                {
                    var split = protocolValue.Split(':');
                    var ipAddressStr = split[0];
                    var port = Convert.ToInt32(split[1]);

                    _mySerialPort = new MySerialPort(ipAddressStr, port);
                }
            }
            catch (Exception ex)
            {
                OnPushControllerMsg(ex.Message);
            }
        }

        public void GetBarcode(string keyAndKeyindexAndLen)
        {
            lock (_locker)
            {
                GetBarcodeStr = string.Empty;
                GetBarcodeLength = 0;

                try
                {
                    var sp = keyAndKeyindexAndLen.Split(':');
                    var key = sp[0];
                    var keyIndex = Convert.ToInt32(sp[1]);
                    var len = Convert.ToInt32(sp[2]);

                    var temp = ReadBuffer();
                    //"000ABCP001EFG000";
                    if (string.IsNullOrEmpty(temp))
                        return;

                    var findKeyIndex = temp.LastIndexOf(key, StringComparison.Ordinal);
                    if (findKeyIndex == -1)
                        return;
                    GetBarcodeStr = temp.Substring(findKeyIndex - keyIndex, len);
                    GetBarcodeLength = GetBarcodeStr.Length;
                }
                catch (Exception exception)
                {
                    // ignored
                    GetBarcodeStr = exception.Message;
                    GetBarcodeLength = 0;
                }
            }
        }

        public int ReadBarcodeTimeoutMs = 60 * 10000;

        public void ReadBarcode(int length)
        {
            _isStopRead = false;

            lock (_locker)
            {
                GetBarcodeStr = string.Empty;
                GetBarcodeLength = 0;

                if (_mySerialPort == null)
                    return;
                _mySerialPort.ClearBuff();

                var tempList = new List<string>();

                var waitCount = 0;

                while (true)
                {
                    if (tempList.Count == length)
                        break;

                    if (waitCount * 1000 > ReadBarcodeTimeoutMs || _isStopRead)
                    {
                        GetBarcodeStr = string.Empty;
                        break;
                    }

                    Thread.Sleep(1);
                    waitCount++;

                    var temp = ReadBuffer().TrimEnd();
                    if (string.IsNullOrEmpty(temp))
                        continue;
                    tempList.Add(temp);
                }

                foreach (var str in tempList)
                    GetBarcodeStr += str.ToString();//TrimEnd();
                //GetBarcodeStr1 = GetBarcodeStr.Substring(20, 4);
                //GetBarcodeLength = GetBarcodeStr.Length;

            }
        }

        private bool _isStopRead;

        public void StopReadBarcode()
        {
            _isStopRead = true;
        }

        private string ReadBuffer()
        {
            return _mySerialPort.ReadDataStr();
        }

        public void LockBarcodeScanReader()
        {
            LockBarcode = true;
        }

        public void UnlockBarcodeScanReader()
        {
            LockBarcode = false;
            GetBarcodeStr = string.Empty;
        }
    }
}
