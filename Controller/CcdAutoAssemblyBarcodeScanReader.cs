using CommonUtility;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class CcdAutoAssemblyBarcodeScanReader : ControllerBase
    {
        private readonly object _locker = new object();
        private MySerialPort _mySerialPort;
        public bool IsInUsing;

        [Description("R,当前读码数据")]
        public string GetBarcodeStr = string.Empty;
        public bool LockBarcode;
        [Description("R,当前读码数据缓存")]
        public int GetBarcodeLength = 0;
        [Description("R,缓存中取得的条码")]
        public string DequeueBuffBarcode = string.Empty;

        public int ngcount = 0;
        public bool connected = false;
        public bool isgetbar = false;

        public CcdAutoAssemblyBarcodeScanReader(string name)
            : base(name) { }

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
                //bool exit = false;
                //Task.Factory.StartNew(() =>
                //{
                //    while (!exit)
                //    {
                //        //connected = false;
                //        try
                //        {
                //            if (connected)
                //            {
                //                //发送心跳
                //                _mySerialPort.SendCommand("-");
                //                Task.Delay(500).Wait();
                //            }
                //            else
                //            {
                //                connected = false;

                //                var split = protocolValue.Split(':');
                //                var ipAddressStr = split[0];
                //                var port = Convert.ToInt32(split[1]);
                //                Stopwatch stopwatch = new Stopwatch();
                //                stopwatch.Start();
                //                _mySerialPort = new MySerialPort(ipAddressStr, port);
                //                stopwatch.Stop();
                //                if (stopwatch.ElapsedMilliseconds < 1500)
                //                {

                //                    connected = true;
                //                }
                //                else
                //                {
                //                    connected = false;
                //                }

                //                Task.Delay(1000).Wait();
                //            }
                //        }
                //        catch (Exception)
                //        {
                //            connected = false;

                //            // client.Disconnect();
                //        }
                //    }
                //}, TaskCreationOptions.LongRunning);
            }
            catch (Exception ex)
            {
                OnPushControllerMsg(ex.Message);
            }
        }

        [Description("读码")]
        public void ReadBarcode()
        {
            lock (_locker)
            {
                isgetbar = false;
                GetBarcodeStr = string.Empty;
                GetBarcodeLength = 0;
                if (_mySerialPort == null)
                    return;
                _mySerialPort.ClearBuff();
                var tempList = new List<string>();
                _mySerialPort.SendCommand("T");
                Thread.Sleep(1000);
                var temp = ReadBuffer();
                temp = temp.TrimEnd();

                if (temp.Length > 0)
                {
                    lock (_lockBuff)
                    {
                        foreach (var str in temp)
                            GetBarcodeStr += str;

                        GetBarcodeLength = GetBarcodeStr.Length;
                        EnqueueBarcode(GetBarcodeStr);
                    }
                }

                ngcount = 0;
            }
        }

        private string ReadBuffer()
        {
            return _mySerialPort.ReadDataStr();
        }

        private Queue<string> _bufferQueue = new Queue<string>();
        private readonly object _lockBuff = new object();

        [Description("取队列缓存")]
        public void DequeueBarcode()
        {
            DequeueBuffBarcode = DequeuBarcode();
        }

        private void EnqueueBarcode(string barocode)
        {
            lock (_lockBuff)
            {
                if (_bufferQueue.Count > 5000)
                    _bufferQueue.Dequeue();
                _bufferQueue.Enqueue(barocode);
            }
        }

        private string DequeuBarcode()
        {
            lock (_lockBuff)
                return _bufferQueue.Any() ? _bufferQueue.Dequeue() : string.Empty;
        }
    }
}
