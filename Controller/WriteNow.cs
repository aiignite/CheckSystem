using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    public sealed class WriteNow : ControllerBase
    {
        [Description("R,通道1烧写结果")]
        public string Site1DownloadResult;
        [Description("R,通道2烧写结果")]
        public string Site2DownloadResult;
        [Description("R,通道3烧写结果")]
        public string Site3DownloadResult;
        [Description("R,通道4烧写结果")]
        public string Site4DownloadResult;
        [Description("R,通道5烧写结果")]
        public string Site5DownloadResult;
        [Description("R,通道6烧写结果")]
        public string Site6DownloadResult;
        [Description("R,通道7烧写结果")]
        public string Site7DownloadResult;
        [Description("R,通道8烧写结果")]
        public string Site8DownloadResult;

        private SerialPort ThisSerialPort { get; set; }
        private MyAsyncSocketClient ThisLan { get; set; }
        private string SelectedProjName { get; set; }
        private bool IsRead { get; set; }
        private readonly List<string> _readBuffer = new List<string>();
        private readonly AutoResetEvent _dataReceivedEvent = new AutoResetEvent(false);

        public WriteNow(string name)
            : base(name)
        {

        }

        /// <summary>
        /// 初始化串口
        /// </summary>
        /// <param name="comtBaudTate"></param>
        public void InitComBaute(string comtBaudTate)
        {
            try
            {
                var com = comtBaudTate.Split(':')[0];
                var baudTate = comtBaudTate.Split(':')[1];
                ThisSerialPort = new SerialPort(com, int.Parse(baudTate));
                ThisSerialPort.Open();
                ThisSerialPort.DataReceived += _thiSerialPort_DataReceived;
            }
            catch (Exception ex)
            {
                OnPushControllerMsg(ex.Message);
            }
        }

        /// <summary>
        /// 初始化网口
        /// </summary>
        /// <param name="ipPort"></param>
        public void InitLan(string ipPort)
        {
            try
            {
                var ipAddr = ipPort.Split(':')[0];
                var port = ipPort.Split(':')[1];
                ThisLan = new MyAsyncSocketClient();
                ThisLan.InitSocket(ipAddr, int.Parse(port));
                ThisLan.OnPushSocketsToTcpClient += ThisLan_OnPushSocketsToTcpClient;
            }
            catch (Exception ex)
            {
                OnPushControllerMsg(ex.Message);
            }
        }

        public void CloseLan()
        {
            if (ThisLan != null)
            {
                ThisLan.Stop();
                ThisLan.OnPushSocketsToTcpClient -= ThisLan_OnPushSocketsToTcpClient;
            }

        }

        private void ThisLan_OnPushSocketsToTcpClient(BaseSocket sockets)
        {
            if (sockets == null || sockets.RecBuffer == null)
                return;

            if (sockets.RecBuffer.Length == 0)
                return;

            var buffer = new byte[sockets.Offset];
            Array.Copy(sockets.RecBuffer, 0, buffer, 0, sockets.Offset);

            DataReadSet(buffer);
        }

        private void _thiSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = sender as SerialPort;
            if (serialPort == null)
                return;
            var len = serialPort.BytesToRead;
            var buff = new byte[len];
            serialPort.Read(buff, 0, len);

            DataReadSet(buff);
        }

        private void DataReadSet(IReadOnlyCollection<byte> buff)
        {
            if (!IsRead)
                return;

            if (buff.Count == 0)
                return;

            lock (_readBuffer)
            {
                foreach (var t in buff)
                {
                    if (t != 0x0A && !_readBuffer.Any())
                        _readBuffer.Add(Encoding.ASCII.GetString(new[] { t }));
                    else if (t != 0x0A && _readBuffer.Any())
                        _readBuffer[_readBuffer.Count - 1] += Encoding.ASCII.GetString(new[] { t });
                    else if (t == 0x0A)
                    {
                        OnPushControllerMsg(_readBuffer[_readBuffer.Count - 1]);
                        _readBuffer.Add(string.Empty);
                    }

                    _dataReceivedEvent.Set();
                }
            }
        }

        /// <summary>
        /// 选项目名称
        /// </summary>
        /// <param name="projectName"></param>
        [Description("选项目名称")]
        public void SelectProjectName(string projectName)
        {
            SelectedProjName = projectName;
        }

        /// <summary>
        /// RUN单通道
        /// </summary>
        /// <param name="siteIndex">通道号</param>
        [Description("RUN单通道")]
        public void RunSingleSite(string siteIndex)
        {
            //Site1DownloadResult = "OK";
            //return;

            var i = int.Parse(siteIndex);
            var selectedIndex = string.Empty;

            if (i == 1)
                selectedIndex = "01";
            else if (i == 2)
                selectedIndex = "02";
            else if (i == 3)
                selectedIndex = "04";
            else if (i == 4)
                selectedIndex = "08";
            else if (i == 5)
                selectedIndex = "10";
            else if (i == 6)
                selectedIndex = "20";
            else if (i == 7)
                selectedIndex = "40";
            else if (i == 8)
                selectedIndex = "80";

            var resultStr = "NG";
            var resultField = GetType().GetField(string.Format("Site{0}DownloadResult", siteIndex));
            resultField.SetValue(this, resultStr);

            if ((ThisSerialPort == null || !ThisSerialPort.IsOpen) && ThisLan == null)
            {
                resultStr += @" 串口或网口异常";
                resultField.SetValue(this, resultStr);
                return;
            }

            lock (_readBuffer)
            {
                _readBuffer.Clear();
            }
            IsRead = true;

            bool isOk;

            var str = string.Format(@"#exec -o prj -f \projects\{0}.wnp -s h{1}", SelectedProjName, selectedIndex);
            str += "\r\n";
            if (ThisSerialPort != null)
                ThisSerialPort.Write(str);
            else if (ThisLan != null)
                ThisLan.SendData(Encoding.ASCII.GetBytes(str));

            while (true)
            {
                //if (!_dataReceivedEvent.WaitOne(2500))
                if (!_dataReceivedEvent.WaitOne(75 * 1000))
                {
                    isOk = false;
                    break;
                }

                lock (_readBuffer)
                {
                    if (_readBuffer.Any(f => !f.Equals("*") && !f.Equals(">") && !string.IsNullOrEmpty(f)))
                    {
                        isOk = false;
                        break;
                    }
                }

                lock (_readBuffer)
                {
                    if (!_readBuffer.Any(f => f.Equals(">")))
                        continue;

                    isOk = true;
                    break;
                }
            }

            resultStr = isOk ? "OK" : "NG";
            resultField.SetValue(this, resultStr);

            IsRead = false;
            lock (_readBuffer)
            {
                _readBuffer.Clear();
            }
        }

        /// <summary>
        /// RUN多通道
        /// </summary>
        /// <param name="startEnd">例1~4</param>
        public void RunRangeSite(string startEnd)
        {
            var sp = startEnd.Split('~');
            var start = int.Parse(sp[0]) - 1;
            var end = int.Parse(sp[1]) - 1;

            var siteListStr = new[]
            {
                0.ToString(), 0.ToString(), 0.ToString(), 0.ToString(),
                0.ToString(), 0.ToString(), 0.ToString(), 0.ToString()
            };

            for (var i = start; i <= end; i++)
            {
                siteListStr[i] = 1.ToString();

                var resultStr = "NG";
                var resultField = GetType().GetField(string.Format("Site{0}DownloadResult", i + 1));
                resultField.SetValue(this, resultStr);

                if ((ThisSerialPort != null && ThisSerialPort.IsOpen) || (ThisLan != null))
                    continue;
                resultStr += @" 串口或网口异常";
                resultField.SetValue(this, resultStr);
            }

            if ((ThisSerialPort == null || !ThisSerialPort.IsOpen) && ThisLan == null)
                return;

            var str = string.Empty;
            for (var i = siteListStr.Length - 1; i >= 0; i--)
                str += siteListStr[i];
            var siteHex = ValueHelper.GetHextStr(Convert.ToByte(str, 2));

            lock (_readBuffer)
            {
                _readBuffer.Clear();
            }

            //var cmd1 = @"#data -o set -c in -t file -f \images\WriteNowOsramEviyos.wni";
            //cmd1 += "\r\n";
            //var cmd2 = @"#data -o set -c out -t file -f \images\dump.bin";
            //cmd2 += "\r\n";

            //if (ThisSerialPort != null)
            //{
            //    ThisSerialPort.Write(cmd1);
            //    ThisSerialPort.Write(cmd2);
            //}
            //else if (ThisLan != null)
            //{
            //    ThisLan.SendData(Encoding.ASCII.GetBytes(cmd1));
            //    ThisLan.SendData(Encoding.ASCII.GetBytes(cmd2));
            //}
            IsRead = true;

            var sendStr = string.Format(@"#exec -o prj -f \projects\{0}.wnp -s h{1}", SelectedProjName, siteHex);
            sendStr += "\r\n";
            if (ThisSerialPort != null)
                ThisSerialPort.Write(sendStr);
            else if (ThisLan != null)
                ThisLan.SendData(Encoding.ASCII.GetBytes(sendStr));

            while (true)
            {
                if (!_dataReceivedEvent.WaitOne(2500))
                {
                    for (var i = start; i <= end; i++)
                    {
                        siteListStr[i] = 1.ToString();

                        const string resultStr = "NG";
                        var resultField = GetType().GetField(string.Format("Site{0}DownloadResult", i + 1));
                        resultField.SetValue(this, resultStr);
                    }
                    break;
                }

                //_readBuffer.Add("*");
                //_readBuffer.Add("*");
                //_readBuffer.Add("h20000003!");

                lock (_readBuffer)
                {
                    if (_readBuffer.Any(f => !f.Equals("*") && !f.Equals(">") && !string.IsNullOrEmpty(f)))
                    {
                        var find =
                            _readBuffer.FindIndex(
                                f =>
                                    !string.IsNullOrEmpty(f) && f.StartsWith("h", StringComparison.OrdinalIgnoreCase) &&
                                    f.EndsWith("!") && f.Length == 8 + 2);

                        if (find != -1)
                        {
                            var hexStr = Convert.ToByte("0x" + _readBuffer[find].Substring(7, 2), 16);
                            var bitStr = Convert.ToString(hexStr, 2).PadLeft(8, '0');

                            var tempBitList = new List<string>();
                            for (var i = bitStr.Length - 1; i >= 0; i--)
                                tempBitList.Add(bitStr[i].ToString());

                            for (var i = 0; i < tempBitList.Count; i++)
                            {
                                if (i < start || i > end)
                                    continue;
                                var resultStr = tempBitList[i] == "1" ? "NG" : "OK";

                                var resultField = GetType().GetField(string.Format("Site{0}DownloadResult", i + 1));
                                resultField.SetValue(this, resultStr);
                            }
                            break;
                        }
                    }
                }

                lock (_readBuffer)
                {
                    if (!_readBuffer.Any(f => f.Equals(">")))
                        continue;

                    for (var i = start; i <= end; i++)
                    {
                        siteListStr[i] = 1.ToString();

                        const string resultStr = "OK";
                        var resultField = GetType().GetField(string.Format("Site{0}DownloadResult", i + 1));
                        resultField.SetValue(this, resultStr);
                    }

                    break;
                }
            }

            IsRead = false;
            lock (_readBuffer)
            {
                _readBuffer.Clear();
            }
        }
    }
}
