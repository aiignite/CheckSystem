using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    public sealed class HaikangBarcodeHVSM : ControllerBase
    {

        public HaikangBarcodeHVSM(string name)
            : base(name) { }


        private readonly MyAsyncSocketClient _tcpClient = new MyAsyncSocketClient();
        public List<byte> _readPerBuff = new List<byte>();
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);

        //需要条码数量
        public int BarcodeCount = 0;

        [Description("R,切换参数结果")]
        public string ChangeResult;

        [Description("R/W,是否获取到条码")]
        public bool IsGetBar;

        [Description("R,条码1")]
        public string Barcode1 = string.Empty;
        [Description("R,条码2")]
        public string Barcode2 = string.Empty;
        [Description("R,条码3")]
        public string Barcode3 = string.Empty;
        [Description("R,条码4")]
        public string Barcode4 = string.Empty;
        [Description("R,条码5")]
        public string Barcode5 = string.Empty;
        [Description("R,条码6")]
        public string Barcode6 = string.Empty;
        [Description("R,条码7")]
        public string Barcode7 = string.Empty;
        [Description("R,条码8")]
        public string Barcode8 = string.Empty;
        [Description("R,条码9")]
        public string Barcode9 = string.Empty;
        [Description("R,条码10")]
        public string Barcode10 = string.Empty;
        [Description("R,条码11")]
        public string Barcode11 = string.Empty;
        [Description("R,条码12")]
        public string Barcode12 = string.Empty;
        [Description("R,条码13")]
        public string Barcode13 = string.Empty;
        [Description("R,条码14")]
        public string Barcode14 = string.Empty;
        [Description("R,条码15")]
        public string Barcode15 = string.Empty;
        [Description("R,条码16")]
        public string Barcode16 = string.Empty;


        private bool isconnect;
        /// <summary>
        /// 初始化客户端
        /// </summary>
        /// <param name="ipstress"></param>
        [Description("连接")]
        public void InitTcpClient(string ipstress)
        {
            if (isconnect)
            {
                return;
            }

            isconnect = true;

            Task.Run(() =>
            {
                var sp = ipstress.Split(':');
                while (true)
                {
                    if (!isconnect)
                    {
                        _tcpClient.Stop();
                     
                        break;
                    }
                    //if (_tcpClient._client == null || !_tcpClient._client.Connected)
                    //{
                    //    _tcpClient.InitSocket(sp[0], Convert.ToInt32(sp[1]));
                    //    //_tcpClient.Start();
                    //    _tcpClient.OnPushSocketsToTcpClient += _tcpClient_OnPushSocketsToTcpClient;

                    //}

                    Thread.Sleep(800);
                }

            });
        }
        public void InitTcpClientChangeByDate0109(string ipPort)
        {
            try
            {
                var ipAddr = ipPort.Split(':')[0];
                var port = ipPort.Split(':')[1];             
                _tcpClient.InitSocket(ipAddr, int.Parse(port));
                _tcpClient.OnPushSocketsToTcpClient += _tcpClient_OnPushSocketsToTcpClient;
                isconnect = true;
            }
            catch (Exception ex)
            {
                OnPushControllerMsg(ex.Message);
            }
        }

        public void TcpClientClose()
        {
            isconnect = false;
        }

        /// <summary>
        /// 客户端发送
        /// </summary>
        /// <param name="msg"></param>
        [Description("ClientWrite")]
        public void ClientWrite(string msg)
        {
            //if (_tcpClient._client == null || _tcpClient._client.Connected)
            {
                _tcpClient.SendData(Encoding.Default.GetBytes(msg));
            }
        }

        /// <summary>
        /// 客户端接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tcpClient_OnPushSocketsToTcpClient(BaseSocket sockets)
        {
            try
            {
                if (sockets == null || sockets.RecBuffer == null)
                    return;

                if (sockets.Offset == 0)
                    return;

                var buffer = new byte[sockets.Offset];
                Array.Copy(sockets.RecBuffer, 0, buffer, 0, sockets.Offset);
                var values = Encoding.ASCII.GetString(buffer).Split(';');
                if (buffer[0] == 0x3C && buffer[buffer.Length -1] == 0x3E)
                {
                    ChangeResult = Encoding.ASCII.GetString(buffer);
                    _waitHandle.Set();
                }
                else
                {
                    if (!IsGetBar)
                    {
                        if (values.Length > 0) Barcode1 = values[0];
                        if (values.Length > 1) Barcode2 = values[1];
                        if (values.Length > 2) Barcode3 = values[2];
                        if (values.Length > 3) Barcode4 = values[3];
                        if (values.Length > 4) Barcode5 = values[4];
                        if (values.Length > 5) Barcode6 = values[5];
                        if (values.Length > 6) Barcode7 = values[6];
                        if (values.Length > 7) Barcode8 = values[7];
                        if (values.Length > 8) Barcode9 = values[8];
                        if (values.Length > 9) Barcode10 = values[9];
                        if (values.Length > 10) Barcode11 = values[10];
                        if (values.Length > 11) Barcode12 = values[11];
                        if (values.Length > 12) Barcode13 = values[12];
                        if (values.Length > 13) Barcode14 = values[13];
                        if (values.Length > 14) Barcode15 = values[14];
                        if (values.Length > 15) Barcode16 = values[15];
                    }

                    if (values.Length >= BarcodeCount && !Barcode1.Contains("NoRead"))
                    {
                        IsGetBar = true;
                    }
                }
     
            }
            catch (Exception) { }

        }

        [Description("切换参数")]
        public void ChangeParameter(int idx)
        {
            ChangeResult = "";

            //if (!_tcpClient._client.Connected)
            //{
            //    ChangeResult = "NG，TCP未连接";
            //    return;
            //}

            ClientWrite("<Set,Acq,0>");
            _waitHandle.WaitOne(2000);

            if (!ChangeResult.Equals("<Set,Acq,OK>"))
            {
                ChangeResult = "NG，关闭采集失败_ " + ChangeResult;
                return;
            }

            Thread.Sleep(100);
            ChangeResult = "";
            ClientWrite("<Set,UserCur,"+idx + ">");
            _waitHandle.WaitOne(2000);

            if (!ChangeResult.Equals("<Set,UserCur,OK>"))
            {
                ChangeResult = "NG，切换参数失败_ " + ChangeResult;
                return;
            }

            Thread.Sleep(100);
            ChangeResult = "";
            ClientWrite("<Set,Acq,1>");
            _waitHandle.WaitOne(2000);

            if (!ChangeResult.Equals("<Set,Acq,OK>"))
            {
                ChangeResult = "NG，打开采集失败_ " + ChangeResult;
                return;
            }

            ChangeResult = "OK";
        }

        public void ClearBarcode()
        {
            IsGetBar = false;
            Barcode1 = "0";
            Barcode2 = "0";
            Barcode3 = "0";
            Barcode4 = "0";
            Barcode5 = "0";
            Barcode6 = "0";
            Barcode7 = "0";
            Barcode8 = "0";
            Barcode9 = "0";
            Barcode10 = "0";
            Barcode11 = "0";
            Barcode12 = "0";
            Barcode13 = "0";
            Barcode14 = "0";
            Barcode15 = "0";
            Barcode16 = "0";
        }


    }
}
