using OpenCvSharp;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    public sealed class HikMultipleBarcodeScanner : ControllerBase
    {
        [Description("R,是否取得条码")]
        public bool IsGetBar;

        [Description("R,最大扫描数量")]
        public int MaxScanCodeCount = 1;

        public HikMultipleBarcodeScanner(string name) : base(name)
        {
            for (var i = 1; i <= 20; i++)
            {
                _barcodePosInfo.Add(i, Tuple.Create<Point, double, double>(new Point(0, 0), 0, 0));
                _bufferQueue.Add(i, new Queue<string>());
            }
        }

        public string ClientReceive = string.Empty;

        private SimpleTcpClient client = new SimpleTcpClient();
        bool connected = false;
        private string _nhkip;

        /// <summary>
        /// 初始化客户端
        /// </summary>
        /// <param name="ipstress"></param>
        public void InitTcpClient(string ipstress)
        {
            _nhkip = ipstress;

            //设置编码格式，默认是UTF8
            client.StringEncoder = Encoding.UTF8;
            //设置分隔符，默认是0x13
            client.Delimiter = Encoding.UTF8.GetBytes("\r")[0];
            var sp = ipstress.Split(':');
            client.Connect(sp[0], int.Parse(sp[1]));
            //收到数据的事件
            bool exit = false;
            Task.Factory.StartNew(() =>
            {
                while (!exit)
                {
                    try
                    {
                        if (connected)
                        {
                            //发送心跳
                            client.Write("-");
                            Task.Delay(10000).Wait();
                        }
                        else
                        {
                            //断线重连
                            client.Connect(sp[0], int.Parse(sp[1]));
                            connected = true;
                            Task.Delay(1000).Wait();
                        }
                    }
                    catch (Exception)
                    {
                        connected = false;
                        client.Disconnect();
                    }
                }

            }, TaskCreationOptions.LongRunning);
            client.DataReceived += Client_DataReceived;
        }

        public void ClientWriteHex(string msg)
        {
            List<byte> senddata = new List<byte>();
            for (int i = 0; i < msg.Length; i += 2)
                senddata.Add(Convert.ToByte(msg.Substring(i, 2), 16));
            msg = Encoding.ASCII.GetString(senddata.ToArray());

            ClientWriteString(msg);
        }

        public void ClientWriteString(string msg)
        {
            try
            {
                if (connected)
                {
                    client.Write(msg);

                }
            }
            catch (Exception ex)
            {

            }
        }

        [Description("R,当前扫描的条码1")]
        public string CurrentReadBarcode1 = string.Empty;
        [Description("R,当前扫描的条码2")]
        public string CurrentReadBarcode2 = string.Empty;
        [Description("R,当前扫描的条码3")]
        public string CurrentReadBarcode3 = string.Empty;
        [Description("R,当前扫描的条码4")]
        public string CurrentReadBarcode4 = string.Empty;
        [Description("R,当前扫描的条码5")]
        public string CurrentReadBarcode5 = string.Empty;
        [Description("R,当前扫描的条码6")]
        public string CurrentReadBarcode6 = string.Empty;
        [Description("R,当前扫描的条码7")]
        public string CurrentReadBarcode7 = string.Empty;
        [Description("R,当前扫描的条码8")]
        public string CurrentReadBarcode8 = string.Empty;
        [Description("R,当前扫描的条码9")]
        public string CurrentReadBarcode9 = string.Empty;
        [Description("R,当前扫描的条码10")]
        public string CurrentReadBarcode10 = string.Empty;
        [Description("R,当前扫描的条码11")]
        public string CurrentReadBarcode11 = string.Empty;
        [Description("R,当前扫描的条码12")]
        public string CurrentReadBarcode12 = string.Empty;
        [Description("R,当前扫描的条码13")]
        public string CurrentReadBarcode13 = string.Empty;
        [Description("R,当前扫描的条码14")]
        public string CurrentReadBarcode14 = string.Empty;
        [Description("R,当前扫描的条码15")]
        public string CurrentReadBarcode15 = string.Empty;
        [Description("R,当前扫描的条码16")]
        public string CurrentReadBarcode16 = string.Empty;
        [Description("R,当前扫描的条码17")]
        public string CurrentReadBarcode17 = string.Empty;
        [Description("R,当前扫描的条码18")]
        public string CurrentReadBarcode18 = string.Empty;
        [Description("R,当前扫描的条码19")]
        public string CurrentReadBarcode19 = string.Empty;
        [Description("R,当前扫描的条码20")]
        public string CurrentReadBarcode20 = string.Empty;

        [Description("R,缓存中取得的条码1")]
        public string DequeueBuffBarcode1 = string.Empty;
        [Description("R,缓存中取得的条码2")]
        public string DequeueBuffBarcode2 = string.Empty;
        [Description("R,缓存中取得的条码3")]
        public string DequeueBuffBarcode3 = string.Empty;
        [Description("R,缓存中取得的条码4")]
        public string DequeueBuffBarcode4 = string.Empty;
        [Description("R,缓存中取得的条码5")]
        public string DequeueBuffBarcode5 = string.Empty;
        [Description("R,缓存中取得的条码6")]
        public string DequeueBuffBarcode6 = string.Empty;
        [Description("R,缓存中取得的条码7")]
        public string DequeueBuffBarcode7 = string.Empty;
        [Description("R,缓存中取得的条码8")]
        public string DequeueBuffBarcode8 = string.Empty;
        [Description("R,缓存中取得的条码9")]
        public string DequeueBuffBarcode9 = string.Empty;
        [Description("R,缓存中取得的条码10")]
        public string DequeueBuffBarcode10 = string.Empty;
        [Description("R,缓存中取得的条码11")]
        public string DequeueBuffBarcode11 = string.Empty;
        [Description("R,缓存中取得的条码12")]
        public string DequeueBuffBarcode12 = string.Empty;
        [Description("R,缓存中取得的条码13")]
        public string DequeueBuffBarcode13 = string.Empty;
        [Description("R,缓存中取得的条码14")]
        public string DequeueBuffBarcode14 = string.Empty;
        [Description("R,缓存中取得的条码15")]
        public string DequeueBuffBarcode15 = string.Empty;
        [Description("R,缓存中取得的条码16")]
        public string DequeueBuffBarcode16 = string.Empty;
        [Description("R,缓存中取得的条码17")]
        public string DequeueBuffBarcode17 = string.Empty;
        [Description("R,缓存中取得的条码18")]
        public string DequeueBuffBarcode18 = string.Empty;
        [Description("R,缓存中取得的条码19")]
        public string DequeueBuffBarcode19 = string.Empty;
        [Description("R,缓存中取得的条码20")]
        public string DequeueBuffBarcode20 = string.Empty;

        private Dictionary<int, Queue<string>> _bufferQueue = new Dictionary<int, Queue<string>>();

        /// <summary>
        /// 客户端接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            try
            {
                string str = e.MessageString;

                if (str[0].Equals('<') && str[str.Length - 1].Equals('>'))
                {
                    ClientReceive = str;
                    return;
                }

                if (!_bExeScanBarcode)
                    return;

                if (!e.MessageString.Contains("NoRead"))
                {
                    var splitBarcodeList = new List<string>();
                    splitBarcodeList = e.MessageString.Split(';').ToList();
                    splitBarcodeList = e.MessageString.Replace("(", "").Replace(")", "").Split(';').ToList();

                    //var maxCount = MaxScanCodeCount >= 1 && MaxScanCodeCount <= 20 ? MaxScanCodeCount : 1;
                    var keys = _tempbarcodeBuffList.Keys.ToList();

                    foreach (var item in splitBarcodeList)
                    {
                        var spInfo = item.Split(',');
                        var x = Convert.ToInt32(spInfo[0]);
                        var y = Convert.ToInt32(spInfo[1]);
                        var barcode = spInfo[2];

                        Console.WriteLine("hik get code: ({0}, {1}), {2}, code len={3}", x, y, barcode, barcode.Length);


                        foreach (var index in keys)
                        {
                            var xMin = _barcodePosInfo[index].Item1.X - _barcodePosInfo[index].Item2;
                            var xMax = _barcodePosInfo[index].Item1.X + _barcodePosInfo[index].Item2;
                            var yMin = _barcodePosInfo[index].Item1.Y - _barcodePosInfo[index].Item3;
                            var yMax = _barcodePosInfo[index].Item1.Y + _barcodePosInfo[index].Item3;

                            if (x >= xMin && x <= xMax && y >= yMin && y <= yMax)
                            {
                                if (_tempbarcodeBuffList[index] != barcode)
                                    _tempbarcodeBuffList[index] = barcode;
                            }
                        }
                    }

                    if (!_tempbarcodeBuffList.Values.ToList().FindAll(f => string.IsNullOrEmpty(f)).Any())
                    {
                        foreach (var index in keys)
                        {
                            var code = _tempbarcodeBuffList[index];
                            EnqueueBarcode(index, code);
                            var fieldName = string.Format("CurrentReadBarcode{0}", index);
                            var field = GetType().GetField(fieldName);
                            field?.SetValue(this, code);
                        }

                        _bExeScanBarcode = false;
                        IsGetBar = true;
                        ClientWriteString("stop");
                    }
                }

                else if (e.MessageString.Contains("NoRead"))
                {
                    IsGetBar = false;
                }
            }

            catch (Exception ex)
            {
                OnPushControllerMsg(ex.Message);
            }
        }

        private readonly Dictionary<int, Tuple<Point, double, double>> _barcodePosInfo = new Dictionary<int, Tuple<Point, double, double>>();
        private Dictionary<int, string> _tempbarcodeBuffList = new Dictionary<int, string>();
        private bool _bExeScanBarcode;
        private readonly object _lockBuff = new object();

        [Description("执行读码")]
        public void ExecuteBarcodeScan(int start, int end)
        {
            if (_bExeScanBarcode)
                return;

            IsGetBar = false;
            _tempbarcodeBuffList.Clear();
            var maxCount = MaxScanCodeCount >= 1 && MaxScanCodeCount <= 20 ? MaxScanCodeCount : 1;

            if (start < 1 || end < 1)
                return;
            if (end < start)
                return;
            if (start > maxCount || end > maxCount)
                return;

            for (var index = start; index <= end; index++)
            {
                _tempbarcodeBuffList.Add(index, string.Empty);
                var code = string.Empty;
                var fieldName = string.Format("CurrentReadBarcode{0}", index);
                var field = GetType().GetField(fieldName);
                field?.SetValue(this, code);
            }

            _bExeScanBarcode = true;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (_bExeScanBarcode == false || IsGetBar)
                    {
                        break;
                    }

                    ClientWriteString("start");
                    Thread.Sleep(500);
                }
            });

            //_bExeScanBarcode = true;
        }

        [Description("取队列缓存")]
        public void DequeueBarcode()
        {
            var maxCount = MaxScanCodeCount >= 1 && MaxScanCodeCount <= 20 ? MaxScanCodeCount : 1;
            var keys = _bufferQueue.Keys.ToList().FindAll(f => f <= maxCount);
            foreach (var index in keys)
            {
                var code = DequeuBarcode(index);
                var fieldName = string.Format("DequeueBuffBarcode{0}", index);
                var field = GetType().GetField(fieldName);
                field?.SetValue(this, code);
            }
        }

        [Description("设置二维码坐标")]
        public void SetBarcodePosition(int index, int xPos, int yPos, int xRange, int yRange)
        {
            if (!_barcodePosInfo.ContainsKey(index))
                return;
            _barcodePosInfo[index] = new Tuple<Point, double, double>(new Point(xPos, yPos), xRange, yRange);
        }

        private void EnqueueBarcode(int index, string barocode)
        {
            lock (_lockBuff)
            {
                if (_bufferQueue[index].Count>5000)
                {
                    _bufferQueue[index].Dequeue();
                }
                _bufferQueue[index].Enqueue(barocode);
            }
        }

        private string DequeuBarcode(int index)
        {
            lock (_lockBuff)
                return _bufferQueue[index].Any() ? _bufferQueue[index].Dequeue() : string.Empty;
        }
    }
}
