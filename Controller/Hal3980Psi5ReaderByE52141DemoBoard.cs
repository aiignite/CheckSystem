using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    public sealed class Hal3980Psi5ReaderByE52141DemoBoard : ControllerBase
    {
        [Description("R,控制器初始化是否成功")]
        public string IsDemoBoardConfingOk = string.Empty;

        [Description("R,通道1-PSI5帧输出(14bit)16进制数")]
        public double Channel1Psi5OutPut14Bits = -9999;
        [Description("R,通道1-PSI5帧输出")]
        public double Channel1Psi5OutPutData = -9999;
        [Description("R,通道1-PSI5帧RollingCounter")]
        public double Channel1Psi5RollingCounter = -9999;
        [Description("R,通道1-PSI5帧StatusBit")]
        public string Channel1Psi5StatusBit = string.Empty;
        [Description("R,通道1-输出占比")]
        public double Channel1OutPutPercent = -9999;

        [Description("R,通道2-PSI5帧输出(14bit)16进制数")]
        public double Channel2Psi5OutPut14Bits = -9999;
        [Description("R,通道2-PSI5帧输出")]
        public double Channel2Psi5OutPutData = -9999;
        [Description("R,通道2-PSI5帧RollingCounter")]
        public double Channel2Psi5RollingCounter = -9999;
        [Description("R,通道2-PSI5帧StatusBit")]
        public string Channel2Psi5StatusBit = string.Empty;
        [Description("R,通道2-输出占比")]
        public double Channel2OutPutPercent = -9999;

        [Description("R,通道3-PSI5帧输出(14bit)16进制数")]
        public double Channel3Psi5OutPut14Bits = -9999;
        [Description("R,通道3-PSI5帧输出")]
        public double Channel3Psi5OutPutData = -9999;
        [Description("R,通道3-PSI5帧RollingCounter")]
        public double Channel3Psi5RollingCounter = -9999;
        [Description("R,通道3-PSI5帧StatusBit")]
        public string Channel3Psi5StatusBit = string.Empty;
        [Description("R,通道3-输出占比")]
        public double Channel3OutPutPercent = -9999;

        [Description("R,通道4-PSI5帧输出(14bit)16进制数")]
        public double Channel4Psi5OutPut14Bits = -9999;
        [Description("R,通道4-PSI5帧输出")]
        public double Channel4Psi5OutPutData = -9999;
        [Description("R,通道4-PSI5帧RollingCounter")]
        public double Channel4Psi5RollingCounter = -9999;
        [Description("R,通道4-PSI5帧StatusBit")]
        public string Channel4Psi5StatusBit = string.Empty;
        [Description("R,通道4-输出占比")]
        public double Channel4OutPutPercent = -9999;

        public Hal3980Psi5ReaderByE52141DemoBoard(string name)
            : base(name)
        {
            // 先上电
            // 再配置demo板寄存器参数
            // 再读取
            // 目前只用CH1有效，其他通道暂时不清楚为什么没有用
        }

        ~Hal3980Psi5ReaderByE52141DemoBoard()
        {
            try
            {
                if (_serialPort != null)
                {
                    _serialPort.Close();
                    _serialPort.Dispose();
                }
            }
            catch (Exception)
            {
                // ignored
            }

            Dispose();
        }

        private SerialPort _serialPort;
        private bool _isReadPsi5Read;
        private string _psi5DataReadBuff = string.Empty;
        private int _readChannel;
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);

        public void ConnectBarcodeScanner(string protocolValue)
        {
            try
            {
                if (protocolValue.StartsWith("COM"))
                {
                    var port = protocolValue.Split(':')[0];
                    var baudTate = protocolValue.Split(':')[1];
                    _serialPort =
                        new SerialPort(port, Convert.ToInt32(baudTate), Parity.None, 8, StopBits.One);
                    _serialPort.Open();
                    _serialPort.DataReceived += _serialPort_DataReceived;
                }
            }
            catch (Exception ex)
            {
                OnPushControllerMsg(ex.Message);
            }
        }

        [Description("通道上电")]
        public void PowerOn()
        {
            if (_serialPort == null)
                return;

            try
            {
                _serialPort.Write("ON\r\n");
                Thread.Sleep(250);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("Demo板寄存器配置")]
        public void DemoBoardConfig()
        {
            IsDemoBoardConfingOk = string.Empty;

            if (_serialPort == null)
                return;

            var initFunc = new Func<bool>(() =>
            {
                _serialPort.Write("SPI_RCONF 00\r\n");
                Thread.Sleep(250);
                _serialPort.Write("SPI_WCONF 00 0001\r\n");
                Thread.Sleep(250);
                _serialPort.Write("SPI_WCONF 01 0010\r\n");
                Thread.Sleep(250);
                _serialPort.Write("SPI_WCONF 02 0041\r\n");
                Thread.Sleep(250);
                _serialPort.Write("SPI_WCONF 03 06D4\r\n");
                Thread.Sleep(250);
                _serialPort.Write("SPI_BUF_CONF 1 2\r\n");
                Thread.Sleep(250);

                var isOk = false;
                for (var i = 0; i < 10; i++)
                {
                    ReadPsi5Data("1");
                    if (!string.IsNullOrEmpty(Channel1Psi5StatusBit))
                    {
                        isOk = true;
                        break;
                    }
                    Thread.Sleep(250);
                }

                return isOk;
            });

            try
            {
                if (!initFunc())
                {
                    if (!initFunc())
                    {
                        IsDemoBoardConfingOk = "NG";
                    }
                    else
                    {
                        IsDemoBoardConfingOk = "OK";
                    }
                }
                else
                {
                    IsDemoBoardConfingOk = "OK";
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("通道断电")]
        public void PowerOff()
        {
            if (_serialPort == null)
                return;

            try
            {
                _serialPort.Write("OFF\r\n");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("读PSI5帧输出")]
        public void ReadPsi5Data(string channel)
        {
            int ch;
            if (int.TryParse(channel, out ch))
            {
                if (ch >= 1 && ch <= 4)
                {
                    if (ch == 1)
                    {
                        Channel1Psi5OutPut14Bits = -9999;
                        Channel1Psi5OutPutData = -9999;
                        Channel1OutPutPercent = -9999;
                        Channel1Psi5RollingCounter = -9999;
                        Channel1Psi5StatusBit = string.Empty;
                    }
                    else if (ch == 2)
                    {
                        Channel2Psi5OutPut14Bits = -9999;
                        Channel2Psi5OutPutData = -9999;
                        Channel2OutPutPercent = -9999;
                        Channel2Psi5RollingCounter = -9999;
                        Channel2Psi5StatusBit = string.Empty;
                    }
                    else if (ch == 3)
                    {
                        Channel3Psi5OutPut14Bits = -9999;
                        Channel3Psi5OutPutData = -9999;
                        Channel3OutPutPercent = -9999;
                        Channel3Psi5RollingCounter = -9999;
                        Channel3Psi5StatusBit = string.Empty;
                    }
                    else if (ch == 4)
                    {
                        Channel4Psi5OutPut14Bits = -9999;
                        Channel4Psi5OutPutData = -9999;
                        Channel4OutPutPercent = -9999;
                        Channel4Psi5RollingCounter = -9999;
                        Channel4Psi5StatusBit = string.Empty;
                    }

                    if (_serialPort == null)
                        return;

                    if (_isReadPsi5Read)
                        return;

                    try
                    {
                        _psi5DataReadBuff = string.Empty;
                        _isReadPsi5Read = true;
                        _readChannel = ch;
                        _waitHandle.Reset();

                        var tSend = new Task(() =>
                        {
                            _serialPort.Write(string.Format("SPI_TR24CHAN {0} 005\r\n", ch));
                        });

                        var tWait = new Task(() =>
                        {
                            if (_waitHandle.WaitOne(1000))
                            {
                                if (ch == 1)
                                {
                                    GetPsi5_14BitData(_psi5DataReadBuff, 1);
                                }
                                else if (ch == 2)
                                {
                                    GetPsi5_14BitData(_psi5DataReadBuff, 2);
                                }
                                else if (ch == 3)
                                {
                                    GetPsi5_14BitData(_psi5DataReadBuff, 3);
                                }
                                else if (ch == 4)
                                {
                                    GetPsi5_14BitData(_psi5DataReadBuff, 4);
                                }
                            }
                        });

                        tWait.Start();
                        tSend.Start();
                        Task.WaitAll(tSend, tWait);

                        _psi5DataReadBuff = string.Empty;
                        _isReadPsi5Read = false;
                        _readChannel = 0;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        private void GetPsi5_14BitData(string value, int ch)
        {
            var str = value.Replace(string.Format("#SPI_TR24CHAN {0} 005 ", ch), "").TrimEnd(':').TrimEnd();
            var sp = str.Split(' ');

            //if (sp.Length == 30 && sp[0] == "0" && sp[2] == "1" && sp[4] == "2" && sp[1] != "7FFFF" && sp[3] == "7FFFF" && sp[5] == "7FFFF")
            if (sp.Length == 30 &&
                sp[0] == "0" && sp[2] == "1" && sp[4] == "2" &&
                sp[6] == "0" && sp[8] == "1" && sp[10] == "2" &&
                sp[12] == "0" && sp[14] == "1" && sp[16] == "2" &&
                sp[18] == "0" && sp[20] == "1" && sp[22] == "2")
            {
                if (sp[0] != "7FFFF" && sp[6] != "7FFFF" && sp[12] != "7FFFF" && sp[18] != "7FFFF" && sp[24] != "7FFFF")
                {
                    if (sp[3] == "7FFFF" && sp[5] == "7FFFF"
                        && sp[9] == "7FFFF" && sp[11] == "7FFFF"
                        && sp[15] == "7FFFF" && sp[17] == "7FFFF")
                    {
                        var rollingCounter1 = Convert.ToByte(Convert.ToString(Convert.ToUInt32(sp[1], 16), 2).PadLeft(32, '0').Substring(29, 3), 2);
                        var rollingCounter2 = Convert.ToByte(Convert.ToString(Convert.ToUInt32(sp[7], 16), 2).PadLeft(32, '0').Substring(29, 3), 2);
                        var rollingCounter3 = Convert.ToByte(Convert.ToString(Convert.ToUInt32(sp[13], 16), 2).PadLeft(32, '0').Substring(29, 3), 2);
                        var rollingCounter4 = Convert.ToByte(Convert.ToString(Convert.ToUInt32(sp[19], 16), 2).PadLeft(32, '0').Substring(29, 3), 2);
                        var rollingCounter5 = Convert.ToByte(Convert.ToString(Convert.ToUInt32(sp[25], 16), 2).PadLeft(32, '0').Substring(29, 3), 2);

                        if (rollingCounter1 + 1 == rollingCounter2 ||
                            rollingCounter2 + 1 == rollingCounter3 ||
                            rollingCounter3 + 1 == rollingCounter4 ||
                            rollingCounter4 + 1 == rollingCounter5)
                        {
                            var psi5Data = sp[25];

                            var uintValue = Convert.ToUInt32(psi5Data, 16);
                            var bits = Convert.ToString(uintValue, 2).PadLeft(32, '0');

                            var rollingCounter = Convert.ToByte(bits.Substring(29, 3), 2);
                            var statusBit = bits.Substring(28, 1);
                            var datasBits = bits.Substring(14, 14);
                            var psi5_14BitsData = Convert.ToUInt32(datasBits, 2);

                            var datasBits2 = string.Empty;
                            if (datasBits.StartsWith("1"))
                            {
                                foreach (var t in datasBits)
                                {
                                    if (t.ToString() == "1")
                                        datasBits2 += "0";
                                    else
                                        datasBits2 += "1";
                                }
                            }
                            else
                            {
                                datasBits2 = datasBits;
                            }

                            float psi5_output;
                            if (datasBits.StartsWith("1"))
                            {
                                psi5_output = 0f - Convert.ToUInt32(datasBits2, 2);
                            }
                            else
                            {
                                psi5_output = Convert.ToUInt32(datasBits2, 2);
                            }
                            var percent = (psi5_output + 7680f) / 15360f * 100f;

                            if (ch == 1)
                            {
                                Channel1Psi5OutPut14Bits = psi5_14BitsData;
                                Channel1Psi5OutPutData = psi5_output;
                                Channel1OutPutPercent = percent;
                                Channel1Psi5RollingCounter = rollingCounter;
                                Channel1Psi5StatusBit = statusBit;
                            }
                            else if (ch == 2)
                            {
                                Channel2Psi5OutPut14Bits = psi5_14BitsData;
                                Channel2Psi5OutPutData = psi5_output;
                                Channel2OutPutPercent = percent;
                                Channel2Psi5RollingCounter = rollingCounter;
                                Channel2Psi5StatusBit = statusBit;
                            }
                            else if (ch == 3)
                            {
                                Channel3Psi5OutPut14Bits = psi5_14BitsData;
                                Channel3Psi5OutPutData = psi5_output;
                                Channel3OutPutPercent = percent;
                                Channel3Psi5RollingCounter = rollingCounter;
                                Channel3Psi5StatusBit = statusBit;
                            }
                            else if (ch == 4)
                            {
                                Channel4Psi5OutPut14Bits = psi5_14BitsData;
                                Channel4Psi5OutPutData = psi5_output;
                                Channel4OutPutPercent = percent;
                                Channel4Psi5RollingCounter = rollingCounter;
                                Channel4Psi5StatusBit = statusBit;
                            }
                        }
                    }
                }
            }
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = sender as SerialPort;
            if (serialPort == null)
                return;
            Thread.Sleep(50);

            var len = serialPort.BytesToRead;
            var buff = new byte[len];
            serialPort.Read(buff, 0, len);

            if (!_isReadPsi5Read || _readChannel <= 0)
            {
                Console.WriteLine(Encoding.ASCII.GetString(buff).Replace(Encoding.ASCII.GetString(new byte[] { 0x0D }), " "));
                return;
            }

            //var buffHex = ValueHelper.GetHextStr(buff);
            //Console.WriteLine(buffHex);

            var buffString = Encoding.ASCII.GetString(buff).Replace(Encoding.ASCII.GetString(new byte[] { 0x0D }), " ");

            if (!string.IsNullOrEmpty(buffString) && buffString.StartsWith(string.Format("#SPI_TR24CHAN {0} 005 ", _readChannel)) && buffString.EndsWith(":"))
            {
                _psi5DataReadBuff = buffString;
                Console.WriteLine("PSI5 Read Buff: " + _psi5DataReadBuff);
                _waitHandle.Set();
            }
        }
    }
}
