using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,PSI5读取")]
    public sealed class SyPsi5InterfaceWithE52140 : ControllerBase
    {
        private readonly SerialPort _serialPort = new SerialPort();
        private readonly MyAsyncSocketClient _tcpClient = new MyAsyncSocketClient();
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);
        private SpiFunc _currentFunc;
        private bool _isSend;
        private readonly List<byte> _sendBuff = new List<byte>();
        private readonly List<byte> _recvBuff = new List<byte>();

        public SyPsi5InterfaceWithE52140(string name)
            : base(name)
        {
            _cicAwakeThread = new Thread(CicAwakeWork) { IsBackground = true };
            _cicAwakeThread.Start();
        }

        ~SyPsi5InterfaceWithE52140()
        {
            Dispose();
        }

        private static byte Crc3(byte[] datas)
        {
            byte poly = 0x0D;
            poly = (byte)(poly << 5);

            var len = datas.Length;

            byte crc8 = 0x07;

            for (var i = 0; i < len; i++)
            {
                crc8 ^= datas[i];

                for (var j = 0; j < 8; j++)
                {
                    if ((crc8 & 0x80) != 0)
                    {
                        crc8 = (byte)((crc8 << 1) ^ poly);
                    }
                    else
                    {
                        crc8 <<= 1;
                    }
                }
            }

            crc8 = (byte)(crc8 >> 5);

            return crc8;
        }

        public void ConnectInterface(string protocolValue)
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

        public void ConnectSerialPort(string comPort)
        {
            try
            {
                var sp = comPort.Split(':');
                //_serialPort = new SerialPort("COM1", 19200);
                _serialPort.PortName = sp[0];
                _serialPort.BaudRate = int.Parse(sp[1]);
                _serialPort.Parity = Parity.None;
                _serialPort.StopBits = StopBits.One;
                _serialPort.DataBits = 8;
                _serialPort.Open();
                _serialPort.DataReceived += _serialPort_DataReceived;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_serialPort == null || !_serialPort.IsOpen)
                return;

            Thread.Sleep(50);
            var len = _serialPort.BytesToRead;
            var buff = new byte[len];
            _serialPort.Read(buff, 0, len);

            var buffStr = ValueHelper.GetHextStr(buff).Replace(" ", "");
            var sendStr = ValueHelper.GetHextStr(_sendBuff.ToArray()).Replace(" ", "");

            Console.WriteLine(string.Format(@"serial port recv data {0}: {1}", _currentFunc, ValueHelper.GetHextStr(buff)));

            if (buffStr.StartsWith(sendStr) && buffStr.Length > sendStr.Length)
            {
                try
                {
                    var newBuffStr = buffStr.Substring(sendStr.Length, buffStr.Length - sendStr.Length);
                    var newBuff = new List<byte>();
                    for (var i = 0; i < newBuffStr.Length; i = i + 2)
                    {
                        var b = Convert.ToByte(newBuffStr.Substring(i, 2), 16);
                        newBuff.Add(b);
                    }

                    DataRecv(newBuff.ToArray());
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
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
            DataRecv(buffer);
        }

        private void DataRecv(byte[] buffer)
        {
            if (buffer == null || buffer.Length < 4)
                return;

            if (buffer[0] == 0x55 && buffer[1] == 0xAA && buffer[buffer.Length - 1] == 0x5A && buffer.Length >= 7)
            {
                var actualLen = buffer[2];
                if (actualLen + 3 == buffer.Length)
                {
                    //Console.WriteLine(ValueHelper.GetHextStr(buffer));

                    var func = EnumOperater.GetEnumByValue<SpiFunc>(buffer[4]);
                    Console.WriteLine(string.Format(@"spi recv func {0}: {1}", func, ValueHelper.GetHextStr(buffer)));

                    if (_isSend)
                    {
                        switch (_currentFunc)
                        {
                            case SpiFunc.Nss:
                                _waitHandle.Set();
                                break;

                            case SpiFunc.ShortSyncPulse:
                                _waitHandle.Set();
                                break;

                            case SpiFunc.SpiSend:
                                if (func == SpiFunc.SpiRecv && buffer[5] == buffer.Length - 8)
                                {
                                    var dataLen = buffer[5];
                                    var data = new byte[dataLen];
                                    Array.Copy(buffer, 6, data, 0, dataLen);
                                    _recvBuff.AddRange(data);
                                    _waitHandle.Set();
                                }
                                break;
                        }
                    }
                }
            }
        }

        private bool FormatAndSendData(IReadOnlyList<byte> buf, out byte[] echo)
        {
            var sendBuf = new byte[255]; // 定义一个足够长的数组！
            sendBuf[0] = 0x55;
            sendBuf[1] = 0xAA;
            var actualLen = (byte)(buf.Count + 6 - 3);
            sendBuf[2] = actualLen; // 报文总长度，55 AA不计入总长度
            sendBuf[actualLen + 3 - 1] = 0x5A; // 最有一位结束符 5A
            for (var i = 0; i < buf.Count; i++) // 从第四位开始填充有效数据
                sendBuf[4 + i] = buf[i];

            sendBuf[3] = 0;
            sendBuf[actualLen + 3 - 2] = 0x00;

            _currentFunc = EnumOperater.GetEnumByValue<SpiFunc>(buf[0]);
            _recvBuff.Clear();
            _sendBuff.Clear();

            _isSend = true;

            var tbs = new byte[actualLen + 3];
            Array.Copy(sendBuf, tbs, actualLen + 3);
            _sendBuff.AddRange(tbs);

            if (_serialPort.IsOpen)
                _serialPort.Write(tbs, 0, tbs.Length);
            else
                _tcpClient.SendData(tbs);

            Console.WriteLine(string.Format(@"spi send func {0}: {1}", _currentFunc, ValueHelper.GetHextStr(tbs)));

            var isOk = _waitHandle.WaitOne(200);

            _isSend = false;
            _sendBuff.Clear();

            if (isOk)
            {
                echo = new byte[_recvBuff.Count];
                Array.Copy(_recvBuff.ToArray(), 0, echo, 0, _recvBuff.Count);
            }
            else
            {
                echo = new byte[] { };
            }

            return isOk;
        }

        private bool NssLow()
        {
            //var datas = new List<byte> { (byte)SpiFunc.Nss, 0x00, 0x00 };
            //byte[] echo;
            //return FormatAndSendData(datas, out echo);

            return true;
        }

        private bool NssHigh()
        {
            //var datas = new List<byte> { (byte)SpiFunc.Nss, 0x00, 0x01 };
            //byte[] echo;
            //return FormatAndSendData(datas, out echo);

            return true;
        }

        private bool SendSpiData(IReadOnlyCollection<byte> value, out byte[] values)
        {
            if (value == null)
            {
                values = new byte[] { };
                return false;
            }

            if (NssLow())
            {
                var datas1 = new List<byte> { (byte)SpiFunc.SpiSend, (byte)value.Count };
                datas1.AddRange(value);

                byte[] echoBytes1;
                if (FormatAndSendData(datas1, out echoBytes1))
                {
                    if (NssHigh())
                    {
                        values = new byte[echoBytes1.Length];
                        Array.Copy(echoBytes1.ToArray(), 0, values, 0, echoBytes1.Length);

                        return true;
                    }
                }
            }

            values = new byte[] { };
            return false;
        }

        /// <summary>
        /// VBUS=5.15V；
        /// Yes, ok but to be on the very safe side higher voltage is better!
        /// </summary>
        [Description("设置VBUS=5.15V")]
        public void SetVBus()
        {
            var bs = E52040_Spi_Write_Register_Cmd(0x00, 0x0001); // write addr 0x01, set vbus=5.15V

            byte[] echo;
            SendSpiData(bs, out echo);

            //for (var i = 0; i < bs.Length; i = i + 2)
            //{
            //    byte[] echo;
            //    SendSpiData(new byte[] { bs[i], bs[i + 1] }, out echo);
            //}
        }

        /// <summary>
        ///  CH1~4 =189kbps；
        /// </summary>
        [Description("设置CH1~4=189kbps")]
        public void SetAllCh189Kps()
        {
            var bs = (E52040_Spi_Write_Register_Cmd(0x01, 0x00F0)); // write addr 0x01, set PSI5_BIT_TIME_CH1~4=189kbps

            byte[] echo;
            SendSpiData(bs, out echo);

            //for (var i = 0; i < bs.Length; i = i + 2)
            //{
            //    byte[] echo;
            //    SendSpiData(new byte[] { bs[i], bs[i + 1] }, out echo);
            //}
        }

        /// <summary>
        /// Enable Sync Pulse Charge Pump; Enable Channel 1~4
        /// </summary>
        [Description("打开CH1~4")]
        public void EnableSyncPulseChargePumpAndAllCh()
        {
            var bs = E52040_Spi_Write_Register_Cmd(0x02, 0x004F); // write addr 0x02, set EN_CP_SYNC=ENABLED, EC_CH1~4=ENABLED

            byte[] echo;
            SendSpiData(bs, out echo);

            //for (var i = 0; i < bs.Length; i = i + 2)
            //{
            //    byte[] echo;
            //    SendSpiData(new byte[] { bs[i], bs[i + 1] }, out echo);
            //}
        }

        /// <summary>
        /// CH timeslot1 configured to be CRC and enable Parity/CRC,
        /// set frame length and set timeslot length
        /// </summary>
        /// <param name="ch">1~4</param>
        /// <param name="frameLen">11~33</param>
        [Description("设置每个通道的数据长度")]
        public void ConfigChTimeslot(int ch, uint frameLen)
        {
            if (ch < 1 || ch > 4 || frameLen < 11 || frameLen > 33)
                return;

            var addr = (byte)(0x03 + (ch - 1) * 7);

            var tsxFlen = frameLen - 10;
            var tsxFlenBits = Convert.ToString(tsxFlen, 2).PadLeft(5, '0');

            var bits = string.Format("000011{0}0100", tsxFlenBits);
            var value = Convert.ToUInt16(bits, 2);

            var bs = E52040_Spi_Write_Register_Cmd(addr, value);

            byte[] echo;
            SendSpiData(bs, out echo);

            //for (var i = 0; i < bs.Length; i = i + 2)
            //{
            //    byte[] echo;
            //    SendSpiData(new byte[] { bs[i], bs[i + 1] }, out echo);
            //}
        }

        /// <summary>
        /// 设置每个通道的buffer长度
        /// </summary>
        /// <param name="ch">1~4</param>
        /// <param name="bitLen">48、32、24、16</param>
        [Description("设置每个通道的buffer长度")]
        public void ConfigSpiBuffer(int ch, uint bitLen)
        {
            if (ch < 1 || ch > 4)
                return;

            var addr = (byte)(0x09 + (ch - 1) * 7);

            string spiBufferCnfgBits;
            switch (bitLen)
            {
                case 48:
                    spiBufferCnfgBits = "00";
                    break;

                case 32:
                    spiBufferCnfgBits = "01";
                    break;

                case 24:
                    spiBufferCnfgBits = "10";
                    break;

                case 16:
                    spiBufferCnfgBits = "11";
                    break;

                default:
                    return;
            }

            var bits = string.Format("0000{0}0000000000", spiBufferCnfgBits);
            var value = Convert.ToUInt16(bits, 2);

            var bs = E52040_Spi_Write_Register_Cmd(addr, value);

            byte[] echo;
            SendSpiData(bs, out echo);

            //for (var i = 0; i < bs.Length; i = i + 2)
            //{
            //    byte[] echo;
            //    SendSpiData(new byte[] { bs[i], bs[i + 1] }, out echo);
            //}
        }

        public void ReadSensorData24Bit(int ch, int shortSyncPulseCount)
        {
            if (ShortSyncPulseCh(ch, shortSyncPulseCount))
            {
                var results = new List<byte>();

                for (var i = 0; i < 3; i++)
                {
                    byte[] echoAll;
                    var bs = new List<byte>();
                    bs.AddRange(E52040_Spi_Get_Data_24b_Cmd(ch, i));
                    bs.AddRange(new byte[] { 0x00, 0x00 });
                    bs.AddRange(new byte[] { 0x00, 0x00 });
                    if (SendSpiData(bs, out echoAll))
                        results.AddRange(echoAll);
                    else
                        return;

                    //byte[] echo1;
                    //if (SendSpiData(E52040_Spi_Get_Data_24b_Cmd(ch, i), out echo1))
                    //    results.AddRange(echo1);
                    //else
                    //    return;

                    //byte[] echo2;
                    //if (SendSpiData(new byte[] { 0x00, 0x00 }, out echo2))
                    //    results.AddRange(echo2);
                    //else
                    //    return;

                    //byte[] echo3;
                    //if (SendSpiData(new byte[] { 0x00, 0x00 }, out echo3))
                    //    results.AddRange(echo3);
                    //else
                    //    return;
                }

                //if (results.Count == 18 && results[0] == 0x31)
                if (results.Count == 18)
                {
                    var listBits = new List<string>();

                    var str1 = Convert.ToString(results[7], 2).PadLeft(8, '0');
                    for (var i = 7; i >= 0; i--)
                        listBits.Add(str1[i].ToString());

                    var str2 = Convert.ToString(results[6], 2).PadLeft(8, '0');
                    for (var i = 7; i >= 0; i--)
                        listBits.Add(str2[i].ToString());

                    var str3 = Convert.ToString(results[5], 2).PadLeft(8, '0');
                    for (var i = 7; i >= 0; i--)
                        listBits.Add(str3[i].ToString());

                    var str4 = Convert.ToString(results[4], 2).PadLeft(8, '0');
                    for (var i = 7; i >= 0; i--)
                        listBits.Add(str4[i].ToString());

                    var str5 = Convert.ToString(results[3], 2).PadLeft(8, '0');
                    for (var i = 7; i >= 0; i--)
                        listBits.Add(str5[i].ToString());

                    var datasBits = string.Empty;
                    for (var i = 31; i >= 18; i--)
                        datasBits += listBits[i];

                    var psi5DataPackage =
                        new Psi5DataPackage(datasBits, listBits[16], listBits[15], listBits[14], listBits[17]);
                    SetResult(ch, psi5DataPackage);
                }

                //Console.WriteLine(ValueHelper.GetHextStr(results.ToArray()));
            }
        }

        public void ReadSensorData32Bit(int ch, int shortSyncPulseCount)
        {
            var readAction = new Action(() =>
            {
                if (ShortSyncPulseCh(ch, shortSyncPulseCount))
                {
                    var results = new List<byte>();

                    for (var i = 0; i < 3; i++)
                    {
                        byte[] echoAll;
                        var bs = new List<byte>();
                        bs.AddRange(E52040_Spi_Get_Data_32b_Cmd(ch, i));
                        bs.AddRange(new byte[] { 0x00, 0x00 });
                        bs.AddRange(new byte[] { 0x00, 0x00 });
                        if (SendSpiData(bs, out echoAll))
                            results.AddRange(echoAll);
                        else
                            return;
                    }

                    //if (results.Count == 18 && results[0] == 0x31)
                    if (results.Count == 18)
                    {
                        var listBits = new List<string>();

                        var str1 = Convert.ToString(results[7], 2).PadLeft(8, '0');
                        for (var i = 7; i >= 0; i--)
                            listBits.Add(str1[i].ToString());

                        var str2 = Convert.ToString(results[6], 2).PadLeft(8, '0');
                        for (var i = 7; i >= 0; i--)
                            listBits.Add(str2[i].ToString());

                        var str3 = Convert.ToString(results[5], 2).PadLeft(8, '0');
                        for (var i = 7; i >= 0; i--)
                            listBits.Add(str3[i].ToString());

                        var str4 = Convert.ToString(results[4], 2).PadLeft(8, '0');
                        for (var i = 7; i >= 0; i--)
                            listBits.Add(str4[i].ToString());

                        var str5 = Convert.ToString(results[3], 2).PadLeft(8, '0');
                        for (var i = 7; i >= 0; i--)
                            listBits.Add(str5[i].ToString());

                        var datasBits = string.Empty;
                        for (var i = 23; i >= 10; i--)
                            datasBits += listBits[i];

                        var crcBits = string.Format("{0}{1}{2}", listBits[28], listBits[27], listBits[26]);
                        var zeroBits = string.Format("{0}{1}", listBits[25], listBits[24]);

                        var psi5DataPackage =
                            new Psi5DataPackage(datasBits, listBits[8], listBits[7], listBits[6], listBits[9],
                                zerobits: zeroBits, crcBits: crcBits);
                        SetResult(ch, psi5DataPackage);
                    }
                }
            });

            readAction.Invoke();
            var stautsBitsFieldValue =
                GetType().GetField(string.Format("Channel{0}Psi5StatusBit", ch)).GetValue(this);
            if (stautsBitsFieldValue == null || stautsBitsFieldValue.ToString() == "1")
                readAction.Invoke();
        }

        private void SetResult(int ch, Psi5DataPackage psi5DataPackage)
        {
            GetType().GetField(string.Format("Channel{0}Psi5RollingCounter", ch))
                .SetValue(this, psi5DataPackage.Psi5RollingCount);

            GetType().GetField(string.Format("Channel{0}Psi5StatusBit", ch))
                .SetValue(this, psi5DataPackage.Psi5StatusBit);

            GetType().GetField(string.Format("Channel{0}Psi5Data14Bit", ch))
                .SetValue(this, psi5DataPackage.Psi5Data14Bit);

            GetType().GetField(string.Format("Channel{0}Psi5OutputData", ch))
                .SetValue(this, psi5DataPackage.Psi5OutputData);

            GetType().GetField(string.Format("Channel{0}OutPutPercent", ch))
                .SetValue(this, psi5DataPackage.OutPutPercent);

            GetType().GetField(string.Format("Channel{0}Psi5RollingCounterBits", ch))
                .SetValue(this, psi5DataPackage.RollingCountBits);

            GetType().GetField(string.Format("Channel{0}Psi5CrcBits", ch))
                .SetValue(this, psi5DataPackage.CrcBits);
        }

        [Description("R,CH1-PSI5帧RollingCounter")]
        public double Channel1Psi5RollingCounter = -9999;
        [Description("R,CH1-PSI5帧StatusBit")]
        public string Channel1Psi5StatusBit = string.Empty;
        [Description("R,CH1-PSI5帧14Bit")]
        public string Channel1Psi5Data14Bit = string.Empty;
        [Description("R,CH1-PSI5帧输出")]
        public double Channel1Psi5OutputData = -9999;
        [Description("R,CH1-输出占比")]
        public double Channel1OutPutPercent = -9999;
        [Description("R,CH1-PSI5帧RoiiingCounterBits")]
        public string Channel1Psi5RollingCounterBits = string.Empty;
        [Description("R,CH1-PSI5帧CrcBits")]
        public string Channel1Psi5CrcBits = string.Empty;

        [Description("R,CH2-PSI5帧RollingCounter")]
        public double Channel2Psi5RollingCounter = -9999;
        [Description("R,CH2-PSI5帧StatusBit")]
        public string Channel2Psi5StatusBit = string.Empty;
        [Description("R,CH2-PSI5帧24Bit")]
        public string Channel2Psi5Data14Bit = string.Empty;
        [Description("R,CH2-PSI5帧输出")]
        public double Channel2Psi5OutputData = -9999;
        [Description("R,CH2-输出占比")]
        public double Channel2OutPutPercent = -9999;
        [Description("R,CH2-PSI5帧RoiiingCounterBits")]
        public string Channel2Psi5RollingCounterBits = string.Empty;
        [Description("R,CH2-PSI5帧CrcBits")]
        public string Channel2Psi5CrcBits = string.Empty;

        [Description("R,CH3-PSI5帧RollingCounter")]
        public double Channel3Psi5RollingCounter = -9999;
        [Description("R,CH3-PSI5帧StatusBit")]
        public string Channel3Psi5StatusBit = string.Empty;
        [Description("R,CH3-PSI5帧24Bit")]
        public string Channel3Psi5Data14Bit = string.Empty;
        [Description("R,CH3-PSI5帧输出")]
        public double Channel3Psi5OutputData = -9999;
        [Description("R,CH3-输出占比")]
        public double Channel3OutPutPercent = -9999;
        [Description("R,CH3-PSI5帧RoiiingCounterBits")]
        public string Channel3Psi5RollingCounterBits = string.Empty;
        [Description("R,CH3-PSI5帧CrcBits")]
        public string Channel3Psi5CrcBits = string.Empty;

        [Description("R,CH4-PSI5帧RollingCounter")]
        public double Channel4Psi5RollingCounter = -9999;
        [Description("R,CH4-PSI5帧StatusBit")]
        public string Channel4Psi5StatusBit = string.Empty;
        [Description("R,CH4-PSI5帧24Bit")]
        public string Channel4Psi5Data14Bit = string.Empty;
        [Description("R,CH4-PSI5帧输出")]
        public double Channel4Psi5OutputData = -9999;
        [Description("R,CH4-输出占比")]
        public double Channel4OutPutPercent = -9999;
        [Description("R,CH4-PSI5帧RoiiingCounterBits")]
        public string Channel4Psi5RollingCounterBits = string.Empty;
        [Description("R,CH4-PSI5帧CrcBits")]
        public string Channel4Psi5CrcBits = string.Empty;

        public bool ShortSyncPulseCh(int ch, int count)
        {
            if (ch < 1 || ch > 4)
                return false;

            if (count < 1 || count > 50)
                return false;

            var chByte = (byte)0x01;

            if (ch == 1)
                chByte = (byte)0x01;
            else if (ch == 2)
                chByte = (byte)0x02;
            else if (ch == 3)
                chByte = (byte)0x04;
            else
                chByte = (byte)0x08;

            var datas = new List<byte> { (byte)SpiFunc.ShortSyncPulse, chByte, (byte)count };
            byte[] echo;
            return FormatAndSendData(datas, out echo);

            //byte[] echo;
            //return SendSpiData(E52040_Spi_ShortSyncPulseCh_Cmd(ch), out echo);
        }

        private static byte[] E52040_Spi_ShortSyncPulseCh_Cmd(int ch)
        {
            if (ch == 1)
                return new byte[] { 0x31, 0x00 };
            if (ch == 2)
                return new byte[] { 0x32, 0x00 };
            if (ch == 3)
                return new byte[] { 0x34, 0x00 };
            if (ch == 4)
                return new byte[] { 0x38, 0x00 };

            return null;
        }

        private byte[] E52040_Spi_Get_Data_24b_Cmd(int ch, int bufferId)
        {
            if (ch >= 1 && ch <= 4 && bufferId >= 0 && bufferId <= 5)
            {
                var listBits = new List<string> { "0", "1", "0", "1" };

                var chBits = Convert.ToString(ch, 2).PadLeft(3, '0');
                listBits.Add(chBits[0].ToString());
                listBits.Add(chBits[1].ToString());
                listBits.Add(chBits[2].ToString());

                var buffIdBits = Convert.ToString(bufferId, 2).PadLeft(3, '0');
                listBits.Add(buffIdBits[0].ToString());
                listBits.Add(buffIdBits[1].ToString());
                listBits.Add(buffIdBits[2].ToString());

                for (var i = 0; i < 6; i++)
                    listBits.Add("0");

                var str = listBits.Aggregate(string.Empty, (current, t) => current + t);
                var returnByte = new List<byte>();

                for (var i = 0; i < str.Length; i = i + 8)
                {
                    var tempStr = string.Empty;

                    for (var j = 0; j < 8; j++)
                        tempStr += str[i + j];

                    returnByte.Add(Convert.ToByte(tempStr, 2));
                }

                //returnByte.Add(0x00);
                //returnByte.Add(0x00);
                //returnByte.Add(0x00);
                //returnByte.Add(0x00);

                return returnByte.ToArray();
            }

            return null;
        }

        private byte[] E52040_Spi_Get_Data_32b_Cmd(int ch, int bufferId)
        {
            if (ch >= 1 && ch <= 4 && bufferId >= 0 && bufferId <= 5)
            {
                var listBits = new List<string> { "0", "1", "1", "1" };

                var chBits = Convert.ToString(ch, 2).PadLeft(3, '0');
                listBits.Add(chBits[0].ToString());
                listBits.Add(chBits[1].ToString());
                listBits.Add(chBits[2].ToString());

                var buffIdBits = Convert.ToString(bufferId, 2).PadLeft(3, '0');
                listBits.Add(buffIdBits[0].ToString());
                listBits.Add(buffIdBits[1].ToString());
                listBits.Add(buffIdBits[2].ToString());

                for (var i = 0; i < 6; i++)
                    listBits.Add("0");

                var str = listBits.Aggregate(string.Empty, (current, t) => current + t);
                var returnByte = new List<byte>();

                for (var i = 0; i < str.Length; i = i + 8)
                {
                    var tempStr = string.Empty;

                    for (var j = 0; j < 8; j++)
                        tempStr += str[i + j];

                    returnByte.Add(Convert.ToByte(tempStr, 2));
                }

                //returnByte.Add(0x00);
                //returnByte.Add(0x00);
                //returnByte.Add(0x00);
                //returnByte.Add(0x00);

                return returnByte.ToArray();
            }

            return null;
        }

        private static byte[] E52040_Spi_Write_Register_Cmd(byte addr, ushort value)
        {
            if (addr > 0x3F)
                return null;

            var listBits = new List<string> { "0", "0", "0", "1" };

            var addrbits = Convert.ToString(addr, 2).PadLeft(8, '0');
            for (var i = 2; i < addrbits.Length; i++)
                listBits.Add(addrbits[i].ToString());

            var valueBits = Convert.ToString(value, 2).PadLeft(16, '0');
            listBits.AddRange(valueBits.Select(t => t.ToString()));

            for (var i = 0; i < 6; i++)
                listBits.Add("0");

            var str = listBits.Aggregate(string.Empty, (current, t) => current + t);
            var returnByte = new List<byte>();

            for (var i = 0; i < str.Length; i = i + 8)
            {
                var tempStr = string.Empty;

                for (var j = 0; j < 8; j++)
                    tempStr += str[i + j];

                returnByte.Add(Convert.ToByte(tempStr, 2));
            }

            return returnByte.ToArray();
        }

        private byte[] E52040_Spi_Read_Register_Cmd(byte addr)
        {
            if (addr > 0x3F)
                return null;

            var listBits = new List<string> { "0", "0", "1", "0" };

            var addrbits = Convert.ToString(addr, 2).PadLeft(8, '0');
            for (var i = 2; i < addrbits.Length; i++)
                listBits.Add(addrbits[i].ToString());

            for (var i = 0; i < 22; i++)
                listBits.Add("0");

            var str = listBits.Aggregate(string.Empty, (current, t) => current + t);
            var returnByte = new List<byte>();

            for (var i = 0; i < str.Length; i = i + 8)
            {
                var tempStr = string.Empty;

                for (var j = 0; j < 8; j++)
                    tempStr += str[i + j];

                returnByte.Add(Convert.ToByte(tempStr, 2));
            }

            return returnByte.ToArray();
        }

        private static byte[] E52040_Spi_Nop_Cmd()
        {
            return new byte[] { 0xE0, 0x00 };
        }

        private enum SpiFunc : byte
        {
            Nss = 0x03,

            SpiSend = 0x04,

            SpiRecv = 0x05,

            ShortSyncPulse = 0x10
        }

        public class Psi5DataPackage
        {
            public double Psi5OutputData { get; set; }
            public string Psi5Data14Bit { get; set; }
            public double Psi5RollingCount { get; set; }
            public string Psi5StatusBit { get; set; }
            public double OutPutPercent { get; set; }
            public string RollingCountBits { get; set; }
            public string CrcBits { get; set; }

            public Psi5DataPackage(
                string data14Bit, string rollingCountBit2, string rollingCountBit1, string rollingCountBit0, string statusBit, string zerobits = "", string crcBits = "")
            {
                var rollingCountBits = string.Empty;
                rollingCountBits += rollingCountBit2;
                rollingCountBits += rollingCountBit1;
                rollingCountBits += rollingCountBit0;

                RollingCountBits = string.Format("{0}{1}{2}", rollingCountBit2, rollingCountBit1, rollingCountBit0);

                if (!string.IsNullOrEmpty(crcBits))
                    CrcBits = crcBits;

                Psi5Data14Bit = data14Bit;

                var datasBits2 = string.Empty;
                if (data14Bit.StartsWith("1"))
                {
                    foreach (var t in data14Bit)
                    {
                        if (t.ToString() == "1")
                            datasBits2 += "0";
                        else
                            datasBits2 += "1";
                    }
                }
                else
                {
                    datasBits2 = data14Bit;
                }

                float psi5Output;
                if (data14Bit.StartsWith("1"))
                    psi5Output = 0f - Convert.ToUInt32(datasBits2, 2);
                else
                    psi5Output = Convert.ToUInt32(datasBits2, 2);

                var percent = (psi5Output + 7680f) / 15360f * 100f;
                var rollingCounter = Convert.ToByte(rollingCountBits, 2);

                OutPutPercent = Math.Round(percent, 2, MidpointRounding.AwayFromZero);
                Psi5RollingCount = rollingCounter;
                Psi5StatusBit = statusBit;
                Psi5OutputData = psi5Output;
                Psi5Data14Bit = data14Bit;

                if (!string.IsNullOrEmpty(zerobits))
                    Psi5Data14Bit += zerobits;
            }
        }

        public static Dictionary<string, string> Psi5ErrorReporting = new Dictionary<string, string>
        {
            {"01111101001000","Overtemperature"},
            {"01111101000100","Undervoltage"},
            {"01111101000010","Overvoltage"},
            {"01111101000001","Magnet lost"},
            {"01111101000000","Clipping of signal/emory error"},

            {"0111110100100000","Overtemperature"},
            {"0111110100010000","Undervoltage"},
            {"0111110100001000","Overvoltage"},
            {"0111110100000100","Magnet lost"},
            {"0111110100000010","Clipping of signal"},
            {"0111110100000001","Memory error"},
        };

        #region CIC模块，CAN诊断读取

        public CanBus CicCan;

        private readonly object _cicLockSend = new object();
        private readonly Thread _cicAwakeThread;

        private void CicAwakeWork()
        {
            while (_cicAwakeThread.IsAlive)
            {
                if (!_cicAwakeThread.IsAlive)
                    break;

                Thread.Sleep(50);

                if (CicCan != null)
                {
                    lock (_cicLockSend)
                    {
                        CicCan.SendStandardCanFdData(0x621, new byte[8]);
                    }
                }
            }
        }

        private int _lfrfErrorCount = 0;
        private int _lrrrErrorCount = 0;
        public bool IsReadLfRf = true;
        public bool IsReadLrRr = false;

        [Description("CIC模块读取PSI5输出")]
        public void ReadSensorData32BitByCicCan()
        {
            const uint reqCanId = 0x14DA7DF1;
            const uint recvCanId = 0x14DAF17D;

            if (CicCan != null)
            {
                lock (_cicLockSend)
                {
                    //byte[] ecuResetEcho;
                    //CicCan.CanBusWithUds
                    //    .TesterTryRequest(reqCanId, recvCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho,
                    //        CanBus.CanType.Extended, CanBus.CanProtocol.CanFd, timeoutFromMilliseconds: 100);
                    //Thread.Sleep(200);

                    CicCan.CanBusWithUds.TryEnterExtendedSession(reqCanId, recvCanId, CanBus.CanType.Extended,
                        CanBus.CanProtocol.CanFd, timeOut: 100);
                }

                lock (_cicLockSend)
                {
                    if (IsReadLfRf)
                    {
                        byte[] echoLfRf;
                        if (CicCan.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Extended,
                                CanBus.CanProtocol.CanFd, 0x40, 0xE8, out echoLfRf, timeoutFromMilliseconds: 100) && echoLfRf.Length >= 4)
                        {
                            //Channel1Psi5Data14Bit = ValueHelper.GetHextStrWithOx(new byte[] { echoLfRf[0], echoLfRf[1] });
                            //Channel2Psi5Data14Bit = ValueHelper.GetHextStrWithOx(new byte[] { echoLfRf[2], echoLfRf[3] });

                            _lfrfErrorCount = 0;

                            var b1 = Convert.ToString(echoLfRf[0], 2).PadLeft(8, '0');
                            var b2 = Convert.ToString(echoLfRf[1], 2).PadLeft(8, '0');
                            var b3 = Convert.ToString(echoLfRf[2], 2).PadLeft(8, '0');
                            var b4 = Convert.ToString(echoLfRf[3], 2).PadLeft(8, '0');

                            var channel1Psi5Data14Bit = string.Format("{0}{1}", b1, b2.Substring(0, 6));
                            var channel2Psi5Data14Bit = string.Format("{0}{1}", b3, b4.Substring(0, 6));

                            var psi5DataPackageCh1 =
                                new Psi5DataPackage(channel1Psi5Data14Bit, "0", "0", "0", "0",
                                    zerobits: "00", crcBits: "000");
                            SetResult(1, psi5DataPackageCh1);

                            var psi5DataPackageCh2 =
                                new Psi5DataPackage(channel2Psi5Data14Bit, "0", "0", "0", "0",
                                    zerobits: "00", crcBits: "000");
                            SetResult(2, psi5DataPackageCh2);
                        }
                        else
                        {
                            if (_lfrfErrorCount > 5)
                            {
                                _lfrfErrorCount = 0;

                                Channel1Psi5OutputData = -9999;
                                Channel1Psi5StatusBit = "1";

                                Channel2Psi5OutputData = -9999;
                                Channel2Psi5StatusBit = "1";
                            }
                            else
                            {
                                _lfrfErrorCount++;
                            }
                        }
                    }
                }

                lock (_cicLockSend)
                {
                    if (IsReadLrRr)
                    {
                        byte[] echoLrRr;
                        if (CicCan.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Extended,
                                CanBus.CanProtocol.CanFd, 0x40, 0xEA, out echoLrRr, timeoutFromMilliseconds: 100) && echoLrRr.Length >= 4)
                        {
                            //Channel3Psi5Data14Bit = ValueHelper.GetHextStrWithOx(new byte[] { echoLrRr[0], echoLrRr[1] });
                            //Channel4Psi5Data14Bit = ValueHelper.GetHextStrWithOx(new byte[] { echoLrRr[2], echoLrRr[3] });

                            _lrrrErrorCount = 0;

                            var b1 = Convert.ToString(echoLrRr[0], 2).PadLeft(8, '0');
                            var b2 = Convert.ToString(echoLrRr[1], 2).PadLeft(8, '0');
                            var b3 = Convert.ToString(echoLrRr[2], 2).PadLeft(8, '0');
                            var b4 = Convert.ToString(echoLrRr[3], 2).PadLeft(8, '0');

                            Channel3Psi5Data14Bit = string.Format("{0}{1}", b1, b2.Substring(0, 6));
                            Channel4Psi5Data14Bit = string.Format("{0}{1}", b3, b4.Substring(0, 6));

                            var psi5DataPackageCh3 =
                                new Psi5DataPackage(Channel3Psi5Data14Bit, "0", "0", "0", "0",
                                    zerobits: "00", crcBits: "000");
                            SetResult(3, psi5DataPackageCh3);

                            var psi5DataPackageCh4 =
                                new Psi5DataPackage(Channel4Psi5Data14Bit, "0", "0", "0", "0",
                                    zerobits: "00", crcBits: "000");
                            SetResult(4, psi5DataPackageCh4);
                        }
                        else
                        {
                            if (_lrrrErrorCount > 5)
                            {
                                _lrrrErrorCount = 0;

                                Channel3Psi5OutputData = -9999;
                                Channel3Psi5StatusBit = "1";

                                Channel4Psi5OutputData = -9999;
                                Channel4Psi5StatusBit = "1";
                            }
                            else
                            {
                                _lrrrErrorCount++;
                            }
                        }
                    }
                }
            }
        }

        [Description("CIC-RESET")]
        public void CicReset()
        {
            if (CicCan == null)
                return;

            const uint reqCanId = 0x14DA7DF1;
            const uint recvCanId = 0x14DAF17D;

            byte[] ecuResetEcho;
            CicCan.CanBusWithUds
                .TesterTryRequest(reqCanId, recvCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho,
                    CanBus.CanType.Extended, CanBus.CanProtocol.CanFd, timeoutFromMilliseconds: 100);
        }

        #endregion
    }
}
