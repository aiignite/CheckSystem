using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Controller
{
    public sealed class Psi5TransferWithElmosE52140 : ControllerBase
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

        public Psi5TransferWithElmosE52140(string name)
            : base(name)
        {
            // 先上电
            // 再配置demo板寄存器参数
            // 再读取
            // 目前只用CH1有效
        }

        ~Psi5TransferWithElmosE52140()
        {
            Dispose();
        }

        private readonly MyAsyncSocketClient _tcpClient = new MyAsyncSocketClient();
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);
        private readonly List<byte> _readPerBuff = new List<byte>();
        private bool _isReadingPer;
        private bool _isSend;
        private string _txbData = string.Empty;
        private bool _isReadPsi5Read;

        public void ConnectController(string protocolValue)
        {
            try
            {
                var sp = protocolValue.Split(':');
                var ip = sp[0];
                var port = int.Parse(sp[1]);

                _tcpClient.InitSocket(ip, port);
                _tcpClient.OnPushSocketsToTcpClient += _tcpClient_OnPushSocketsToTcpClient;
            }
            catch (Exception ex)
            {
                OnPushControllerMsg(ex.Message);
            }
        }

        private void _tcpClient_OnPushSocketsToTcpClient(BaseSocket sockets)
        {
            if (sockets == null || sockets.RecBuffer == null)
                return;

            if (sockets.Offset == 0)
                return;

            var buffer = new byte[sockets.Offset];
            Array.Copy(sockets.RecBuffer, 0, buffer, 0, sockets.Offset);

            if (buffer[0] == 0x55 && buffer[1] == 0xAA && buffer[buffer.Length - 1] == 0x5A && buffer.Length >= 7)
            {
                Console.WriteLine(ValueHelper.GetHextStr(buffer));
                var actualLen = buffer[2];
                if (actualLen + 3 == buffer.Length)
                {
                    if (_isSend)
                    {
                        // 进行显示
                        var str = new StringBuilder();
                        foreach (var t in buffer)
                            str.Append(t.ToString("X").PadLeft(2, '0') + " ");
                        //txb_DataShow.AppendText("接收数据为: " + str + "\r\n");
                        //Console.WriteLine("接收数据为: " + str + "\r\n");

                        if (buffer[4] == 0x05)
                        {
                            if (buffer[5] == buffer.Length - 8 && _isReadingPer)
                            {
                                if (_isReadingPer)
                                {
                                    var dataLen = buffer[5];
                                    var data = new byte[dataLen];
                                    Array.Copy(buffer, 6, data, 0, dataLen);
                                    _readPerBuff.AddRange(data);
                                }
                            }
                        }

                        _waitHandle.Set();
                    }
                }
            }
        }

        private void btn_creatAndSend()
        {
            if (_tcpClient != null)
            {
                var sendBuf = new byte[255]; // 定义一个足够长的数组！
                byte actualLen; // 最后要生成数据的长度！
                sendBuf[0] = 0x55;
                sendBuf[1] = 0xAA;
                var strDis = new StringBuilder();

                try
                {
                    // 解析数据，顺带解析长度，使用正则表达式！
                    var inputData = _txbData;
                    var str = Regex.Replace(inputData, @"[0-9a-fA-F][0-9a-fA-F]{0,1}", "$0" + " ");
                    var mc = Regex.Matches(str, @"\b[0-9a-fA-F]+\b");
                    var buf = (from Match m in mc select byte.Parse(m.Value, NumberStyles.HexNumber)).ToList();
                    actualLen = (byte)(buf.Count + 6 - 3); // 总长度！
                    //lb_len.Text = @"报文总长度：" + actualLen;
                    sendBuf[2] = actualLen; // 报文总长度，55 AA不计入总长度
                    sendBuf[actualLen + 3 - 1] = 0x5A; // 最有一位结束符 5A
                    for (var i = 0; i < buf.Count; i++) // 从第四位开始填充有效数据
                    {
                        sendBuf[4 + i] = buf[i];
                    }

                    //// 第三位是否验证校验
                    //if (chb_check.Checked)
                    //{
                    //    sendBuf[3] = 1;
                    //    CaculateXorAndLen(sendBuf, (byte)(actualLen + 3));
                    //    lb_checkValue.Text = @"校验值：0x" + sendBuf[actualLen + 3 - 3].ToString("X");
                    //}
                    //else
                    {
                        sendBuf[3] = 0;
                        sendBuf[actualLen + 3 - 2] = 0x00;
                        //lb_checkValue.Text = @"校验值：-";
                    }
                    {
                        for (var i = 0; i < actualLen + 3; i++)
                        {
                            strDis.Append(sendBuf[i].ToString("X").PadLeft(2, '0') + " ");
                        }
                        //if (chb_showSend.Checked)
                        //{
                        //    txb_DataShow.AppendText("生成数据为: " + strDis + "\r\n");
                        //}
                    }
                }
                catch
                {
                    //txb_DataShow.AppendText("长度错误" + "数据出错，请检查");
                    return;
                }

                try
                {
                    _isSend = true;

                    var tbs = new byte[actualLen + 3];
                    Array.Copy(sendBuf, tbs, actualLen + 3);
                    _tcpClient.SendData(tbs);

                    _waitHandle.WaitOne(200);

                    _isSend = false;
                }
                catch
                {
                    //txb_DataShow.AppendText("串口发送失败，请检查串口\r\n");
                }
            }
        }

        private void Psi5PerformClick(string byte0, string byte1)
        {
            // NSS 低电平
            _txbData = @"03 00 00";
            //Thread.Sleep(25);
            btn_creatAndSend();
            //Thread.Sleep(25);

            _txbData = string.Format(@"04 02 {0} {1}", byte0, byte1);//@"04 02 52 80";
            //Thread.Sleep(25);
            btn_creatAndSend();
            //Thread.Sleep(25);

            // NSS 高电平
            _txbData = @"03 00 01";
            //Thread.Sleep(25);
            btn_creatAndSend();
            //Thread.Sleep(25);
        }

        private DataPackage SPI_TR24CHAN_1(int delayMs = 100)
        {
            _readPerBuff.Clear();

            _isReadingPer = true;

            // 短触发
            // 3100
            Psi5PerformClick("31", "00");

            // SPI_TR24CHAN 1 005
            // 5200 0000 0000          
            Psi5PerformClick("52", "00");
            Psi5PerformClick("00", "00");
            Psi5PerformClick("00", "00");

            // 5240 0000 0000
            Psi5PerformClick("52", "40");
            Psi5PerformClick("00", "00");
            Psi5PerformClick("00", "00");

            // 5280 0000 0000
            Psi5PerformClick("52", "80");
            Psi5PerformClick("00", "00");
            Psi5PerformClick("00", "00");

            // E0 00
            Psi5PerformClick("E0", "00");

            if (delayMs == 0)
            {
                _isReadingPer = false;
                return null;
            }

            Thread.Sleep(delayMs);
            _isReadingPer = false;

            try
            {
                var listBits = new List<string>();

                Console.WriteLine(ValueHelper.GetHextStr(_readPerBuff.ToArray()));

                var str1 = Convert.ToString(_readPerBuff[9], 2).PadLeft(8, '0');
                for (var i = 7; i >= 0; i--)
                    listBits.Add(str1[i].ToString());

                var str2 = Convert.ToString(_readPerBuff[8], 2).PadLeft(8, '0');
                for (var i = 7; i >= 0; i--)
                    listBits.Add(str2[i].ToString());

                var str3 = Convert.ToString(_readPerBuff[7], 2).PadLeft(8, '0');
                for (var i = 7; i >= 0; i--)
                    listBits.Add(str3[i].ToString());

                var str4 = Convert.ToString(_readPerBuff[6], 2).PadLeft(8, '0');
                for (var i = 7; i >= 0; i--)
                    listBits.Add(str4[i].ToString());

                var str5 = Convert.ToString(_readPerBuff[5], 2).PadLeft(8, '0');
                for (var i = 7; i >= 0; i--)
                    listBits.Add(str5[i].ToString());

                var rollingCountBits = string.Empty;
                rollingCountBits += listBits[16];
                rollingCountBits += listBits[15];
                rollingCountBits += listBits[14];

                var statusBit = listBits[17];

                var datasBits = string.Empty;
                for (var i = 31; i >= 18; i--)
                    datasBits += listBits[i];

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
                float psi5Output;
                if (datasBits.StartsWith("1"))
                {
                    psi5Output = 0f - Convert.ToUInt32(datasBits2, 2);
                }
                else
                {
                    psi5Output = Convert.ToUInt32(datasBits2, 2);
                }
                var percent = (psi5Output + 7680f) / 15360f * 100f;
                var rollingCounter = Convert.ToByte(rollingCountBits, 2);

                var channel1OutPutPercent = Math.Round(percent, 2, MidpointRounding.AwayFromZero);
                var channel1Psi5RollingCounter = rollingCounter;
                var channel1Psi5StatusBit = statusBit;

                var dataPackage = new DataPackage
                {
                    OutPutPercent = channel1OutPutPercent,
                    Psi5RollingCounter = channel1Psi5RollingCounter,
                    Channel1Psi5StatusBit = channel1Psi5StatusBit,
                };
                return dataPackage;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private class DataPackage
        {
            [Description("PSI5帧输出(14bit)16进制数")]
            public double Psi5OutPut14Bits = -9999;
            [Description("PSI5帧输出")]
            public double Psi5OutPutData = -9999;
            [Description("PSI5帧RollingCounter")]
            public double Psi5RollingCounter = -9999;
            [Description("PSI5帧StatusBit")]
            public string Channel1Psi5StatusBit = string.Empty;
            [Description("输出占比")]
            public double OutPutPercent = -9999;
        }

        [Description("通道上电")]
        public void PowerOn()
        {
            if (_tcpClient == null)
                return;
        }

        [Description("通道断电")]
        public void PowerOff()
        {
            if (_tcpClient == null)
                return;
        }

        [Description("Demo板寄存器配置")]
        public void DemoBoardConfig()
        {
            IsDemoBoardConfingOk = string.Empty;

            if (_tcpClient == null)
                return;

            var initFunc = new Func<bool>(() =>
            {
                // SPI_RCONF 00
                // 2000 0000 E000
                //Psi5PerformClick("20", "00");
                //Psi5PerformClick("00", "00");
                //Psi5PerformClick("E0", "00");

                // SPI_WCONF 00 0001
                // 1000 0040 E000 
                // VBUS=5.15V
                // ASIC_CNFG_1(0X00)=0x001
                Psi5PerformClick("10", "00");
                Psi5PerformClick("00", "40");
                Psi5PerformClick("E0", "00");

                // SPI_WCONF 01 0010
                // 1040 0400 E000
                // PSI5_BIT_TIME_CH1=189kbps
                // ASIC_CNFG_2(0X01)=0x0010
                Psi5PerformClick("10", "40");
                Psi5PerformClick("04", "00");
                Psi5PerformClick("E0", "00");

                // SPI_WCONF 02 0041
                // 1080 1040 E000
                // enable sync pulse charge pump
                // en_ch1
                // ASIC_CNFG_3(0X02)=0X41
                Psi5PerformClick("10", "80");
                Psi5PerformClick("10", "40");
                Psi5PerformClick("E0", "00");

                // SPI_WCONF 03 06D4
                // 10C1 B500 E000
                // CH1_CFG1(0X03)=0x06D4
                // CH1 timeslot1 configured to be CRC and enable Parity/CRC, set frame length and set timeslot length
                Psi5PerformClick("10", "C1");
                Psi5PerformClick("B5", "00");
                Psi5PerformClick("E0", "00");

                // SPI_BUF_CONF 1 2
                // 2240 0000 E000     
                //Psi5PerformClick("22", "40");
                //Psi5PerformClick("00", "00");
                //Psi5PerformClick("E0", "00");

                // 1242 0000 E000
                // CH1_CFG7(0X09)=0X0800
                Psi5PerformClick("12", "42");
                Psi5PerformClick("00", "00");
                Psi5PerformClick("E0", "00");

                for (var i = 0; i < 35; i++)
                {
                    SPI_TR24CHAN_1(0);
                }

                ReadPsi5Data("1");
                var rollingCount1 = (int)Channel1Psi5RollingCounter;
                ReadPsi5Data("1");
                var rollingCount2 = (int)Channel1Psi5RollingCounter;
                ReadPsi5Data("1");
                var rollingCount3 = (int)Channel1Psi5RollingCounter;

                if (rollingCount1 == 7 && rollingCount2 == 0 && rollingCount3 == 1)
                {
                    return true;
                }
                if (rollingCount1 == 6 && rollingCount2 == 7 && rollingCount3 == 0)
                {
                    return true;
                }
                if (rollingCount1 == 5 && rollingCount2 == 6 && rollingCount3 == 7)
                {
                    return true;
                }
                if (rollingCount1 + 1 == rollingCount2 && rollingCount2 + 1 == rollingCount3)
                {
                    return true;
                }

                return false;
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

                    if (_tcpClient == null)
                        return;

                    if (_isReadPsi5Read)
                        return;

                    try
                    {
                        //var st=new Stopwatch();
                        //st.Start();

                        _isReadPsi5Read = true;

                        var dataPackage = SPI_TR24CHAN_1(1);
                        if (dataPackage != null)
                        {
                            if (ch == 1)
                            {
                                GetPsi5_14BitData(dataPackage, 1);
                            }
                            else if (ch == 2)
                            {
                                GetPsi5_14BitData(dataPackage, 2);
                            }
                            else if (ch == 3)
                            {
                                GetPsi5_14BitData(dataPackage, 3);
                            }
                            else if (ch == 4)
                            {
                                GetPsi5_14BitData(dataPackage, 4);
                            }
                        }

                        _isReadPsi5Read = false;

                        //st.Stop();
                        //Console.WriteLine("Read one psi5 data cost "+ st.ElapsedMilliseconds);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        private void GetPsi5_14BitData(DataPackage dataPackage, int ch)
        {
            if (ch == 1)
            {
                Channel1Psi5OutPut14Bits = dataPackage.Psi5OutPut14Bits;
                Channel1Psi5OutPutData = dataPackage.Psi5OutPutData;
                Channel1OutPutPercent = dataPackage.OutPutPercent;
                Channel1Psi5RollingCounter = dataPackage.Psi5RollingCounter;
                Channel1Psi5StatusBit = dataPackage.Channel1Psi5StatusBit;
            }
            else if (ch == 2)
            {
                Channel2Psi5OutPut14Bits = dataPackage.Psi5OutPut14Bits;
                Channel2Psi5OutPutData = dataPackage.Psi5OutPutData;
                Channel2OutPutPercent = dataPackage.OutPutPercent;
                Channel2Psi5RollingCounter = dataPackage.Psi5RollingCounter;
                Channel2Psi5StatusBit = dataPackage.Channel1Psi5StatusBit;
            }
            else if (ch == 3)
            {
                Channel3Psi5OutPut14Bits = dataPackage.Psi5OutPut14Bits;
                Channel3Psi5OutPutData = dataPackage.Psi5OutPutData;
                Channel3OutPutPercent = dataPackage.OutPutPercent;
                Channel3Psi5RollingCounter = dataPackage.Psi5RollingCounter;
                Channel3Psi5StatusBit = dataPackage.Channel1Psi5StatusBit;
            }
            else if (ch == 4)
            {
                Channel4Psi5OutPut14Bits = dataPackage.Psi5OutPut14Bits;
                Channel4Psi5OutPutData = dataPackage.Psi5OutPutData;
                Channel4OutPutPercent = dataPackage.OutPutPercent;
                Channel4Psi5RollingCounter = dataPackage.Psi5RollingCounter;
                Channel4Psi5StatusBit = dataPackage.Channel1Psi5StatusBit;
            }
        }
    }
}
