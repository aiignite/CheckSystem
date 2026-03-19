using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class JK2516S : ControllerBase
    {
        public JK2516S(string name) : base(name) { }

        private SerialPort _serialPort;
        private MyAsyncSocketClient _tcpClient;
        private bool _bIsSerialPort;
        /// <summary>
        /// 读取电阻状态判断
        /// </summary>
        private bool _bInReadRes;
        /// <summary>
        /// 读取电阻量程状态判断
        /// </summary>
        private bool _bInReadRange;

        public void ConnectCom(string protocolValue)
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
                    _bIsSerialPort = true;
                }
                else
                {
                    var split = protocolValue.Split(':');
                    var ipAddressStr = split[0];
                    var port = Convert.ToInt32(split[1]);

                    _tcpClient.InitSocket(ipAddressStr, port);
                    _tcpClient.OnPushSocketsToTcpClient += _tcpClient_OnPushSocketsToTcpClient;
                }
            }
            catch (Exception ex)
            {
                OnPushControllerMsg(ex.Message);
            }
        }

        private void _tcpClient_OnPushSocketsToTcpClient(BaseSocket sockets)
        {
            var buff = new byte[sockets.Offset];
            Array.Copy(sockets.RecBuffer, 0, buff, 0, buff.Length);

            FormatReadResData(buff);
            FormatReadRangeData(buff);
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = sender as SerialPort;
            if (serialPort == null)
                return;

            Thread.Sleep(100);

            var len = serialPort.BytesToRead;
            var buff = new byte[len];
            serialPort.Read(buff, 0, len);
            FormatReadResData(buff);
            FormatReadRangeData(buff);
        }

        #region RES读取结果

        [Description("R,R1")] public double R1 = double.MinValue;
        [Description("R,R2")] public double R2 = double.MinValue;
        [Description("R,R3")] public double R3 = double.MinValue;
        [Description("R,R4")] public double R4 = double.MinValue;
        [Description("R,R5")] public double R5 = double.MinValue;
        [Description("R,R6")] public double R6 = double.MinValue;
        [Description("R,R7")] public double R7 = double.MinValue;
        [Description("R,R8")] public double R8 = double.MinValue;
        [Description("R,R9")] public double R9 = double.MinValue;
        [Description("R,R10")] public double R10 = double.MinValue;
        [Description("R,R11")] public double R11 = double.MinValue;
        [Description("R,R12")] public double R12 = double.MinValue;
        [Description("R,R13")] public double R13 = double.MinValue;
        [Description("R,R14")] public double R14 = double.MinValue;
        [Description("R,R15")] public double R15 = double.MinValue;
        [Description("R,R16")] public double R16 = double.MinValue;
        [Description("R,R17")] public double R17 = double.MinValue;
        [Description("R,R18")] public double R18 = double.MinValue;
        [Description("R,R19")] public double R19 = double.MinValue;
        [Description("R,R20")] public double R20 = double.MinValue;
        [Description("R,R21")] public double R21 = double.MinValue;
        [Description("R,R22")] public double R22 = double.MinValue;
        [Description("R,R23")] public double R23 = double.MinValue;
        [Description("R,R24")] public double R24 = double.MinValue;
        [Description("R,R25")] public double R25 = double.MinValue;
        [Description("R,R26")] public double R26 = double.MinValue;
        [Description("R,R27")] public double R27 = double.MinValue;
        [Description("R,R28")] public double R28 = double.MinValue;
        [Description("R,R29")] public double R29 = double.MinValue;
        [Description("R,R30")] public double R30 = double.MinValue;
        [Description("R,R31")] public double R31 = double.MinValue;
        [Description("R,R32")] public double R32 = double.MinValue;
        [Description("R,R33")] public double R33 = double.MinValue;
        [Description("R,R34")] public double R34 = double.MinValue;
        [Description("R,R35")] public double R35 = double.MinValue;
        [Description("R,R36")] public double R36 = double.MinValue;
        [Description("R,R37")] public double R37 = double.MinValue;
        [Description("R,R38")] public double R38 = double.MinValue;
        [Description("R,R39")] public double R39 = double.MinValue;
        [Description("R,R40")] public double R40 = double.MinValue;
        [Description("R,R41")] public double R41 = double.MinValue;
        [Description("R,R42")] public double R42 = double.MinValue;
        [Description("R,R43")] public double R43 = double.MinValue;
        [Description("R,R44")] public double R44 = double.MinValue;
        [Description("R,R45")] public double R45 = double.MinValue;
        [Description("R,R46")] public double R46 = double.MinValue;
        [Description("R,R47")] public double R47 = double.MinValue;
        [Description("R,R48")] public double R48 = double.MinValue;
        [Description("R,R49")] public double R49 = double.MinValue;
        [Description("R,R50")] public double R50 = double.MinValue;
        [Description("R,R51")] public double R51 = double.MinValue;
        [Description("R,R52")] public double R52 = double.MinValue;
        [Description("R,R53")] public double R53 = double.MinValue;
        [Description("R,R54")] public double R54 = double.MinValue;
        [Description("R,R55")] public double R55 = double.MinValue;
        [Description("R,R56")] public double R56 = double.MinValue;
        [Description("R,R57")] public double R57 = double.MinValue;
        [Description("R,R58")] public double R58 = double.MinValue;
        [Description("R,R59")] public double R59 = double.MinValue;
        [Description("R,R60")] public double R60 = double.MinValue;
        [Description("R,R61")] public double R61 = double.MinValue;
        [Description("R,R62")] public double R62 = double.MinValue;
        [Description("R,R63")] public double R63 = double.MinValue;
        [Description("R,R64")] public double R64 = double.MinValue;
        [Description("R,R65")] public double R65 = double.MinValue;
        [Description("R,R66")] public double R66 = double.MinValue;
        [Description("R,R67")] public double R67 = double.MinValue;
        [Description("R,R68")] public double R68 = double.MinValue;
        [Description("R,R69")] public double R69 = double.MinValue;
        [Description("R,R70")] public double R70 = double.MinValue;
        [Description("R,R71")] public double R71 = double.MinValue;
        [Description("R,R72")] public double R72 = double.MinValue;
        [Description("R,R73")] public double R73 = double.MinValue;
        [Description("R,R74")] public double R74 = double.MinValue;
        [Description("R,R75")] public double R75 = double.MinValue;
        [Description("R,R76")] public double R76 = double.MinValue;
        [Description("R,R77")] public double R77 = double.MinValue;
        [Description("R,R78")] public double R78 = double.MinValue;
        [Description("R,R79")] public double R79 = double.MinValue;
        [Description("R,R80")] public double R80 = double.MinValue;
        [Description("R,R81")] public double R81 = double.MinValue;
        [Description("R,R82")] public double R82 = double.MinValue;
        [Description("R,R83")] public double R83 = double.MinValue;
        [Description("R,R84")] public double R84 = double.MinValue;
        [Description("R,R85")] public double R85 = double.MinValue;
        [Description("R,R86")] public double R86 = double.MinValue;
        [Description("R,R87")] public double R87 = double.MinValue;
        [Description("R,R88")] public double R88 = double.MinValue;
        [Description("R,R89")] public double R89 = double.MinValue;
        [Description("R,R90")] public double R90 = double.MinValue;

        #endregion

        [Description("读电阻")]
        public void ReadRes()
        {
            #region 复位读取数据

            R1 = double.MinValue;
            R2 = double.MinValue;
            R3 = double.MinValue;
            R4 = double.MinValue;
            R5 = double.MinValue;
            R6 = double.MinValue;
            R7 = double.MinValue;
            R8 = double.MinValue;
            R9 = double.MinValue;
            R10 = double.MinValue;
            R11 = double.MinValue;
            R12 = double.MinValue;
            R13 = double.MinValue;
            R14 = double.MinValue;
            R15 = double.MinValue;
            R16 = double.MinValue;
            R17 = double.MinValue;
            R18 = double.MinValue;
            R19 = double.MinValue;
            R20 = double.MinValue;
            R21 = double.MinValue;
            R22 = double.MinValue;
            R23 = double.MinValue;
            R24 = double.MinValue;
            R25 = double.MinValue;
            R26 = double.MinValue;
            R27 = double.MinValue;
            R28 = double.MinValue;
            R29 = double.MinValue;
            R30 = double.MinValue;
            R31 = double.MinValue;
            R32 = double.MinValue;
            R33 = double.MinValue;
            R34 = double.MinValue;
            R35 = double.MinValue;
            R36 = double.MinValue;
            R37 = double.MinValue;
            R38 = double.MinValue;
            R39 = double.MinValue;
            R40 = double.MinValue;
            R41 = double.MinValue;
            R42 = double.MinValue;
            R43 = double.MinValue;
            R44 = double.MinValue;
            R45 = double.MinValue;
            R46 = double.MinValue;
            R47 = double.MinValue;
            R48 = double.MinValue;
            R49 = double.MinValue;
            R50 = double.MinValue;
            R51 = double.MinValue;
            R52 = double.MinValue;
            R53 = double.MinValue;
            R54 = double.MinValue;
            R55 = double.MinValue;
            R56 = double.MinValue;
            R57 = double.MinValue;
            R58 = double.MinValue;
            R59 = double.MinValue;
            R60 = double.MinValue;
            R61 = double.MinValue;
            R62 = double.MinValue;
            R63 = double.MinValue;
            R64 = double.MinValue;
            R65 = double.MinValue;
            R66 = double.MinValue;
            R67 = double.MinValue;
            R68 = double.MinValue;
            R69 = double.MinValue;
            R70 = double.MinValue;
            R71 = double.MinValue;
            R72 = double.MinValue;
            R73 = double.MinValue;
            R74 = double.MinValue;
            R75 = double.MinValue;
            R76 = double.MinValue;
            R77 = double.MinValue;
            R78 = double.MinValue;
            R79 = double.MinValue;
            R80 = double.MinValue;
            R81 = double.MinValue;
            R82 = double.MinValue;
            R83 = double.MinValue;
            R84 = double.MinValue;
            R85 = double.MinValue;
            R86 = double.MinValue;
            R87 = double.MinValue;
            R88 = double.MinValue;
            R89 = double.MinValue;
            R90 = double.MinValue;

            #endregion

            #region 测试方法
            //测试段

            //{
            //    string test = "01 03 E0 00 00 00 00 00 1E 84 80 00 00 04 72 00 1E 84 80 00 00 04 72 00 1E 84 80 00 02 49 D5 00 1E 84 80 00 00 04 4E 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 01 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 04 D2 00 00 00 01 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 07 D0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 E3 A3";

            //    test = test.Replace(" ", "");

            //    byte[] testbyte = new byte[test.Length / 2];
            //    for (int i = 0; i < test.Length; i += 2)
            //    {
            //        testbyte[i / 2] = Convert.ToByte(test.Substring(i, 2), 16);
            //    }
            //    _bInReadRes = true;
            //    FormatReadResData(testbyte);



            //    string test2 = "01 03 30 1E 8E 8E 6E 81 11 11 11 11 11 11 11 11 11 11 11 21 11 11 11 11 11 11 11 11 11 11 11 11 11 11 11 11 11 55 55 00 00 55 55 55 55 55 55 55 55 55 55 DA D6";

            //    test2 = test2.Replace(" ", "");

            //    byte[] testbyte2 = new byte[test2.Length / 2];
            //    for (int i = 0; i < test2.Length; i += 2)
            //    {
            //        testbyte2[i / 2] = Convert.ToByte(test2.Substring(i, 2), 16);
            //    }
            //    _bInReadRange = true;
            //    FormatReadRangeData(testbyte2);
            //}

            //////////////////////////////////////////////////////////////
            #endregion


            if (_serialPort is null && _tcpClient is null)
                return;

            var startBs = new byte[] { 0x01, 0x06, 0x01, 0x68, 0x00, 0x01, 0xC8, 0x2A };
            if (_bIsSerialPort)
                _serialPort?.Write(startBs, 0, startBs.Length);
            else
                _tcpClient?.SendData(startBs);

            Thread.Sleep(2500);

            //读电阻值
            {
                _bInReadRes = true;
                if (!SendReadResData())
                    _bInReadRes = false;

                var startTs = HighPrecisionTimer.GetTimestamp();

                while (_bInReadRes)
                {
                    var nowTs = HighPrecisionTimer.GetTimestamp();
                    var ms = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                    if (ms >= 2500)
                        break;
                }

                _bInReadRes = false;
            }

            //读取量程
            {
                _bInReadRange = true;
                if (!SendReadRangeData())//添加读取量程的报文
                    _bInReadRange = false;

                var startTs = HighPrecisionTimer.GetTimestamp();
                while (_bInReadRange)//增加读取量程延时
                {
                    var nowTs = HighPrecisionTimer.GetTimestamp();
                    var ms = HighPrecisionTimer.GetTimestampIntervalMs(startTs, nowTs);
                    if (ms >= 1000)
                        break;
                }
                _bInReadRange = false;
            }

            Thread.Sleep(500);

            var endBs = new byte[] { 0x01, 0x06, 0x01, 0x68, 0x00, 0x00, 0x09, 0xEA };  // 修改
            if (_bIsSerialPort)
                _serialPort?.Write(endBs, 0, endBs.Length); // 修改
            else
                _tcpClient?.SendData(endBs); // 修改
        }

        private const byte DevId = 0x01;
        private const byte Func = 0x03;
        private const byte StartAddrHi = 0x00;
        private const byte StartAddrLo = 0x00;
        private const byte DataLenHi = 0x00;
        private const byte DataLenLow = 0x70; // 修改

        private const byte StartRangeAddrHi = 0x01;
        private const byte StartRangeAddrLo = 0x00;
        private const byte DataRangeLenHi = 0x00;
        private const byte DataRangeLenLow = 0x18;


        private bool SendReadResData()//读取电阻阻值的方法
        {
            var toCheckCrcData = new byte[] { DevId, Func, StartAddrHi, StartAddrLo, DataLenHi, DataLenLow };
            var crc = ValueHelper.Crc16(toCheckCrcData).ToArray();
            var bs = new List<byte>();
            bs.AddRange(toCheckCrcData);
            bs.AddRange(crc);

            if (_bIsSerialPort)
            {
                if (_serialPort is null)
                    return false;
                _serialPort.Write(bs.ToArray(), 0, bs.Count);

            }
            else
            {
                if (_tcpClient is null)
                    return false;
                _tcpClient.SendData(bs.ToArray());
            }

            return true;
        }

        private bool SendReadRangeData()//读取电阻量程的方法
        {

            var toCheckCrcData = new byte[] { DevId, Func, StartRangeAddrHi, StartRangeAddrLo, DataLenHi, 0x18 };//读取24通道量程
            var crc = ValueHelper.Crc16(toCheckCrcData).ToArray();
            var bs = new List<byte>();
            bs.AddRange(toCheckCrcData);
            bs.AddRange(crc);

            if (_bIsSerialPort)
            {
                if (_serialPort is null)
                    return false;
                _serialPort.Write(bs.ToArray(), 0, bs.Count);

            }
            else
            {
                if (_tcpClient is null)
                    return false;
                _tcpClient.SendData(bs.ToArray());
            }

            return true;
        }

        /// <summary>
        /// 将串口接收到的报文组合成电阻阻值
        /// </summary>
        /// <param name="data">包含电阻阻值的接收数据</param>
        private void FormatReadResData(byte[] data)
        {
            if (_bInReadRes)
            {
                if (data is null)
                    return;

                if (data.Length == 1 + 1 + 1 + ((DataLenHi * 256) + DataLenLow) * 2 + 2) //筛选电阻值报文
                {
                    var devId = data[0];
                    var func = data[1];
                    var dataLen = data[2];
                    var crcHi = data[data.Length - 1 - 1];
                    var crcLo = data[data.Length - 1];
                    var toCheckCrc = new byte[data.Length - 2];
                    Array.Copy(data, toCheckCrc, toCheckCrc.Length);
                    var checkCrc = ValueHelper.Crc16(toCheckCrc).ToArray();
                    if (devId == DevId && func == Func && dataLen == (DataLenHi * 256 + DataLenLow) * 2 && checkCrc[0] == crcHi && checkCrc[1] == crcLo)//判断是否是电阻的回读报文
                    {
                        var resData = new byte[(DataLenHi * 256 + DataLenLow) * 2];
                        Array.Copy(data, 3, resData, 0, resData.Length);
                        var res = new List<double>();

                        for (var i = 4; i < resData.Length - 1; i = i + 4) // 修改 
                            res.Add(resData[i] * 256 * 256 * 256 + resData[i + 1] * 256 * 256 + resData[i + 2] * 256 + resData[i + 3]); //电阻读数为4个字节

                        for (var i = 0; i < res.Count; i++)
                        {
                            var resIndex = i + 1;
                            var fieldName = "R" + resIndex;
                            var field = GetType().GetField(fieldName);
                            if (field != null)
                                field.SetValue(this, res[i]);//读取到的数值，需要结合档位计算
                        }

                        _bInReadRes = false;
                    }
                }
            }
        }


        /// <summary>
        /// 将串口接收到的报文组合成电阻档位
        /// </summary>
        /// <param name="data">包含电阻档位的接收数据</param>
        private void FormatReadRangeData(byte[] data)
        {
            if (_bInReadRange)
            {
                if (data is null)
                    return;

                if (data.Length == 1 + 1 + 1 + ((DataRangeLenHi * 256) + DataRangeLenLow) * 2 + 2) //id+功能号+长度位+数据段+两位CRC
                {
                    var devId = data[0];
                    var func = data[1];
                    var dataLen = data[2];
                    var crcHi = data[data.Length - 1 - 1];
                    var crcLo = data[data.Length - 1];
                    var toCheckCrc = new byte[data.Length - 2];
                    Array.Copy(data, toCheckCrc, toCheckCrc.Length);
                    var checkCrc = ValueHelper.Crc16(toCheckCrc).ToArray();
                    if (devId == DevId && func == Func && dataLen == (DataRangeLenHi * 256 + DataRangeLenLow) * 2 && checkCrc[0] == crcHi && checkCrc[1] == crcLo)
                    {
                        var RangeData = new byte[(DataRangeLenHi * 256 + DataRangeLenLow) * 2];
                        Array.Copy(data, 3, RangeData, 0, RangeData.Length);
                        var Range = new List<byte>();

                        for (var i = 0; i < RangeData.Length; i++)
                        {
                            if (i == 0)
                            {
                                Range.Add(RangeData[i].GetByteLowOrder());
                            }
                            else
                            {
                                Range.Add(RangeData[i].GetByteHighOrder());
                                Range.Add(RangeData[i].GetByteLowOrder());
                            }
                        }

                        for (var i = 0; i < Range.Count; i++)
                        {
                            var resIndex = i + 1;
                            var fieldName = "R" + resIndex;
                            var field = GetType().GetField(fieldName);
                            if (field != null)
                            {
                                var fieldValue = (double)field.GetValue(this);

                                /*
                                 *0量程:温度 **.**（正）
                                 * 10量程：温度 **.**（负）
                                 * 1量程:20.000 mΩ, 共5位数字,    **.***
                                 * 2量程:200.00 mΩ,共5位数字     **.**
                                 * 3量程:2000.0 mΩ,共5位数字     ****.*
                                 * 4量程:20.000Ω,共5位数字       ***.***
                                 * 5量程:200.00Ω,共5位数字       ***.**
                                 * 6量程:1000.00Ω,共6位数字      ****.**
                                 * 7量程:100.000 KΩ,共6位数字    ***.***
                                 * 8量程:1000.00KΩ,共6位数字    ****.**
                                 * 9量程:***.* KΩ               **.*
                                 * 11量程 *** KΩ                ***
                                 * 14量程  ----
                                 */

                                // 转成对应的阻值,单位为Ω
                                switch (Range[i])
                                {
                                    case 1:
                                        fieldValue = (fieldValue * 0.001) * 0.001;
                                        break;

                                    case 2:
                                        fieldValue = (fieldValue * 0.01) * 0.001;
                                        break;

                                    case 3:
                                        fieldValue = (fieldValue * 0.1) * 0.001;
                                        break;

                                    case 4:
                                        fieldValue = fieldValue * 0.001;
                                        break;

                                    case 5:
                                        fieldValue = fieldValue * 0.01;
                                        break;

                                    case 6:
                                        fieldValue = fieldValue * 0.01;
                                        break;

                                    case 7:
                                        fieldValue = (fieldValue * 0.001) * 1000;
                                        break;

                                    case 8:
                                        fieldValue = (fieldValue * 0.01) * 1000;
                                        break;

                                    case 9:
                                        fieldValue = (fieldValue * 0.1) * 1000;
                                        break;

                                    case 10:
                                        fieldValue = fieldValue * 1000;
                                        break;
                                }

                                field.SetValue(this, fieldValue);
                            }
                        }

                        _bInReadRange = false;
                    }
                }
            }
        }
    }
}
