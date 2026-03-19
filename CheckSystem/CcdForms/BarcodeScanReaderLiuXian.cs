using CommonUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace CheckSystem.CcdForms
{
    public sealed class BarcodeScanReaderLiuXian : Controller.ControllerBase
    {
        private readonly object _locker = new object();
        private MySerialPort _mySerialPort;
        public bool IsInUsing;
        public string GetBarcodeStr = string.Empty;
        public bool LockBarcode;
        public int GetBarcodeLength = 0;

        public string Grade1Str = string.Empty;
        public string Grade2Str = string.Empty;
        public string Grade3Str = string.Empty;
        public string Grade4Str = string.Empty;
        public string Grade5Str = string.Empty;




        public string GW1SSC = string.Empty;
        public string GW1XSC = string.Empty;
        public string GW2SC = string.Empty;
        public string GW3SC = string.Empty;
        public string GW4SC = string.Empty;
        public string GW5SC = string.Empty;

        public int ngcount = 0;


        public bool connected = false;

        public bool isgetbar = false;


        public BarcodeScanReaderLiuXian(string name)
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
                bool exit = false;
                Task.Factory.StartNew(() =>
                {
                    while (!exit)
                    {
                        //connected = false;
                        try
                        {
                            if (connected)
                            {
                                //发送心跳
                                _mySerialPort.SendCommand("-");
                                Task.Delay(500).Wait();
                            }
                            else
                            {
                                connected = false;

                                var split = protocolValue.Split(':');
                                var ipAddressStr = split[0];
                                var port = Convert.ToInt32(split[1]);
                                Stopwatch stopwatch = new Stopwatch();
                                stopwatch.Start();
                                _mySerialPort = new MySerialPort(ipAddressStr, port);
                                stopwatch.Stop();
                                if (stopwatch.ElapsedMilliseconds < 1500)
                                {

                                    connected = true;
                                }
                                else
                                {
                                    connected = false;
                                }

                                Task.Delay(1000).Wait();
                            }
                        }
                        catch (Exception)
                        {
                            connected = false;

                            // client.Disconnect();
                        }
                    }

                }, TaskCreationOptions.LongRunning);


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
                GetBarcodeLength = 0;
                GetBarcodeStr = string.Empty;
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
                    GetBarcodeLength = GetBarcodeStr.Length;
                }
            }
        }


        private class ReturnMes
        {
            public string RES { get; set; }
        }



        public void ClearBuffer()
        {
            _mySerialPort.ClearBuff();
        }




        public void ReadBarcode()
        {
            lock (_locker)
            {
                GetBarcodeStr = string.Empty;
                GetBarcodeLength = 0;
                if (_mySerialPort == null)
                    return;
                _mySerialPort.ClearBuff();

                var tempList = new List<string>();
                _mySerialPort.SendCommand("T");


                var temp = ReadBuffer();

                foreach (var str in temp)
                    GetBarcodeStr += str;

                GetBarcodeLength = GetBarcodeStr.Length;



                for (int i = 0; i < 6; i++)
                //for (; ; )
                {
                    Thread.Sleep(50);
                    if (GetBarcodeLength == 0)
                    {
                        isgetbar = false;
                        GetBarcodeStr = string.Empty;
                        GetBarcodeLength = 0;
                        if (_mySerialPort == null)
                            return;
                        _mySerialPort.ClearBuff();

                        tempList = new List<string>();
                        _mySerialPort.SendCommand("T");


                        temp = ReadBuffer();

                        foreach (var str in temp)
                            GetBarcodeStr += str;
                        GetBarcodeLength = GetBarcodeStr.Length;

                    }

                    if (GetBarcodeLength != 0)
                    {
                        isgetbar = true;
                    }


                    if (isgetbar)
                    {
                        break;
                    }

                    //else
                    //{
                    //    break;
                    //}

                }
                if (GetBarcodeStr.Contains(";"))
                {
                    var GetBarcodeStr1 = GetBarcodeStr.Split(';');
                    GetBarcodeStr = GetBarcodeStr1[0];

                }
                //if (GetBarcodeLength == 0)
                //{
                //    MessageBox.Show("扫码失败，请确认");
                //    return;
                //}




            }
        }

        public void GetGrade1(string indexAndLen)
        {
            try
            {
                Grade1Str = string.Empty;
                if (string.IsNullOrEmpty(GetBarcodeStr))
                    return;
                var index = int.Parse(indexAndLen.Split(':')[0]);
                var len = int.Parse(indexAndLen.Split(':')[1]);
                Grade1Str = GetBarcodeStr.Substring(index, len);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void GetGrade2(string indexAndLen)
        {
            try
            {
                Grade2Str = string.Empty;
                if (string.IsNullOrEmpty(GetBarcodeStr))
                    return;
                var index = int.Parse(indexAndLen.Split(':')[0]);
                var len = int.Parse(indexAndLen.Split(':')[1]);
                Grade2Str = GetBarcodeStr.Substring(index, len);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void GetGrade3(string indexAndLen)
        {
            try
            {
                Grade3Str = string.Empty;
                if (string.IsNullOrEmpty(GetBarcodeStr))
                    return;
                var index = int.Parse(indexAndLen.Split(':')[0]);
                var len = int.Parse(indexAndLen.Split(':')[1]);
                Grade3Str = GetBarcodeStr.Substring(index, len);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void GetGrade4(string indexAndLen)
        {
            try
            {
                Grade4Str = string.Empty;
                if (string.IsNullOrEmpty(GetBarcodeStr))
                    return;
                var index = int.Parse(indexAndLen.Split(':')[0]);
                var len = int.Parse(indexAndLen.Split(':')[1]);
                Grade4Str = GetBarcodeStr.Substring(index, len);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void GetGrade5(string indexAndLen)
        {
            try
            {
                Grade5Str = string.Empty;
                if (string.IsNullOrEmpty(GetBarcodeStr))
                    return;
                var index = int.Parse(indexAndLen.Split(':')[0]);
                var len = int.Parse(indexAndLen.Split(':')[1]);
                Grade5Str = GetBarcodeStr.Substring(index, len);
            }
            catch (Exception)
            {
                // ignored
            }
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
