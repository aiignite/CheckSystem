using CommonUtility;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    public sealed class HalHac3980Programmer : ControllerBase
    {
        #region public fields

        [Description("R,FirmwareVersion")]
        public string FirmwareVersion = string.Empty;
        [Description("R,SwitchMspToOperationModeB")]
        public string SwitchMspToOperationModeB = string.Empty;
        [Description("R,SetBitTimeInMicrosecond")]
        public string SetBitTimeInMicrosecond = string.Empty;
        [Description("R,SupplyVoltageOn")]
        public string SupplyVoltageOn = string.Empty;
        [Description("R,SupplyVoltageOff")]
        public string SupplyVoltageOff = string.Empty;
        [Description("R,ListeningMode0x22A2")]
        public string ListeningMode0X22A2 = string.Empty;
        [Description("R,ProgrammingMode0x2EAE")]
        public string ProgrammingMode0X2Eae = string.Empty;
        [Description("R,ReadAddr0x75")]
        public string ReadAddr0x75Result = string.Empty;
        [Description("R,GenerateReset0x0006")]
        public string GenerateReset0X0006 = string.Empty;
        [Description("R,SelectIoChannel")]
        public string IoChannel = string.Empty;

        [Description("R,将参数文件存入EEPROM的结果")]
        public string StoreEepromResult = string.Empty;
        [Description("R,标定结果")]
        public string CalibrationResult = string.Empty;

        [Description("R/W,EEPROM参数文件路径")]
        public string EepromParaFilePath =
            //@"E:\Projects\557_PSI5\558 CPS HAL 3980 PSI5\量产烧录器\HAL3980程序烧写软件即配置\PS15 eeprom配置\S1 S2 S3 S4-44us(14bit 16bit formal).txt";
            @"C:\Users\B1438\Desktop\MIC_ID_0251DAB9_2024-08-28_13-02-43_EEPROM_DUMP.txt";

        #endregion

        /// <summary>
        /// 38400,even,8bit,1bit
        /// </summary>
        private SerialPort _serialPort;
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);
        private readonly object _buffLocker = new object();
        private readonly List<byte> _buff = new List<byte>();
        private bool _bSendCmd;
        private string _sRecvCmd = string.Empty;

        public delegate void PushInfoMsgEventHandle(string info);
        public event PushInfoMsgEventHandle PushInfoMsg;

        public void OnPushInfoMsg(string info, bool isRecv)
        {
            if (PushInfoMsg == null)
                return;
            var dateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            PushInfoMsg(string.Format("{0}_{1}: {2}", dateTime, isRecv ? "Recv" : "Write", info));
        }

        public HalHac3980Programmer(string name) :
            base(name)
        {
            StartRecvTimer();
        }

        ~HalHac3980Programmer()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            Dispose();
        }

        private void StartRecvTimer()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = new Action(() =>
                {
                    lock (_buffLocker)
                    {
                        if (_buff.Count >= 2)
                        {
                            var str = ValueHelper.GetHextStr(_buff.ToArray()).Replace(" ", "");

                            var isGet = false;
                            for (var k = 0; k < 16; k++)
                            {
                                var startStr =
                                    ValueHelper.GetHextStr(Encoding.ASCII.GetBytes(string.Format("{0}:", k))).Replace(" ", "");
                                var endStr = "0D0A";

                                if (str.StartsWith(startStr))
                                {
                                    isGet = true;
                                    var findEndIndex = str.IndexOf(endStr, StringComparison.Ordinal);
                                    if (findEndIndex != -1)
                                    {
                                        if (findEndIndex % 2 == 0)
                                        {
                                            //var startIndex = 0;
                                            var endIndex = findEndIndex / 2 + 1;
                                            var len = endIndex - 0 + 1;

                                            var bs = new byte[len];
                                            Array.Copy(_buff.ToArray(), 0, bs, 0, len);
                                            for (var i = 0; i < len; i++)
                                                _buff.RemoveAt(0);

                                            if (_bSendCmd)
                                            {
                                                _sRecvCmd = Encoding.ASCII.GetString(bs, 0, bs.Length).Trim();
                                                OnPushInfoMsg(_sRecvCmd, true);
                                                _bSendCmd = false;
                                                _waitHandle.Set();
                                            }
                                        }
                                        else
                                        {
                                            _buff.Clear();
                                        }
                                    }
                                }
                            }

                            if (!isGet)
                            {
                                _buff.Clear();
                            }
                        }
                    }
                }),
                Interval = 5
            });

            SchedulerAsync();
        }

        private ushort Crc16_CCITT(IReadOnlyList<ushort> data, byte length)
        {
            const ushort checksumPoly = 0x1021; // x^16+x^12+x^5+1
            byte i, bit;
            ushort tempByteSwap;
            var crc = (ushort)0xFFFFU;
            for (i = 0; i < length; i++)
            {
                tempByteSwap = (ushort)(((data[i] >> 8) & 0xFFU) + ((data[i] & 0xFFU) << 8));
                crc = (ushort)(crc ^ tempByteSwap);
                for (bit = 0; bit < 16; bit++)
                {
                    if ((crc & 0x8000U) != 0)
                    {
                        crc = (ushort)(crc << 1);
                        crc = (ushort)(crc ^ checksumPoly);
                    }
                    else
                        crc = (ushort)(crc << 1);
                }
            }
            tempByteSwap = (ushort)(((crc >> 8) & 0xFFU) + ((crc & 0xFFU) << 8));
            return tempByteSwap;
        }

        private int CalcCrc(int data, int size)
        {
            const byte crcPoly = 0x1D; // X^8+X^4+X^3+X^2+
            int i;
            byte crc = 0xFF;
            for (i = 0; i < size; i++)
            {
                if ((crc >> 7 & 0x1) != (data >> (size - 1 - i) & 1))
                {
                    crc = (byte)(crc << 1);
                    crc ^= crcPoly;
                    crc &= 0xFF;
                }
                else
                    crc <<= 1;
            }
            return crc = (byte)((~crc) & 0xFF);
        }

        private static byte Crc8(byte[] data, byte length)
        {
            const byte crcPoly = 0x1D; // x^8+x^4+x^3+x^2+

            var crc = (byte)0xFF;
            for (var i = 0; i < length; i++)
            {
                crc = (byte)(crc ^ data[i]);
                for (var bit = 0; bit < 8; bit++)
                {
                    if ((crc & 0x80) != 0)
                    {
                        crc = (byte)(crc << 1);
                        crc = (byte)(crc ^ crcPoly);
                    }
                    else
                        crc = (byte)(crc << 1);
                }
            }
            return (byte)~crc;
        }

        public void ConnectCom(string com)
        {
            try
            {
                _serialPort = new SerialPort(com, 38400, Parity.Even, 8, StopBits.One);
                _serialPort.DataReceived += _serialPort_DataReceived;
                _serialPort.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = sender as SerialPort;
            if (serialPort == null)
                return;

            //Thread.Sleep(40);

            var len = serialPort.BytesToRead;
            var buff = new byte[len];
            serialPort.Read(buff, 0, len);

            lock (_buffLocker)
                _buff.AddRange(buff);
        }

        private bool SendCommand(string value, out string response)
        {
            _sRecvCmd = string.Empty;
            _bSendCmd = true;
            var isSuccess = false;

            if (_serialPort == null || !_serialPort.IsOpen)
            {
                response = string.Empty;
                return false;
            }

            var t1 = Task.Factory.StartNew(() =>
            {
                lock (_buffLocker)
                {
                    if (value.StartsWith("xxw"))
                    {
                        var addr = Convert.ToByte(value.Substring(3, 2), 16);
                        var addrBits = Convert.ToString(addr, 2).PadLeft(8, '0').Substring(1, 7) + "0";
                        addr = Convert.ToByte(addrBits, 2);
                        var data0 = Convert.ToByte(value.Substring(5, 2), 16);
                        var data1 = Convert.ToByte(value.Substring(7, 2), 16);
                        var data = (ushort)(data0 * 256 + data1);
                        var crc = Crc8(new[] { addr, data0, data1 }, 3);
                        //Console.WriteLine("CRC:" + crc);
                        value += ValueHelper.GetHextStr(crc);
                    }

                    var bs = Encoding.ASCII.GetBytes(value).ToList();
                    bs.Add(0x0A);
                    if (_serialPort != null && _serialPort.IsOpen)
                    {
                        _serialPort.Write(bs.ToArray(), 0, bs.Count);
                        OnPushInfoMsg(value, false);
                    }
                }
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                isSuccess = _waitHandle.WaitOne(500);
            });

            Task.WaitAll(t1, t2);

            if (isSuccess)
            {
                if (string.IsNullOrEmpty(_sRecvCmd))
                {
                    isSuccess = false;
                    response = string.Empty;
                }
                else
                {
                    response = _sRecvCmd;
                }
            }
            else
            {
                _bSendCmd = false;
                response = string.Empty;
            }

            return isSuccess;
        }

        [Description("Get firmware version")]
        public void GetFirmwareVersion()
        {
            FirmwareVersion = string.Empty;
            SendCommand("?V", out FirmwareVersion);
        }

        [Description("Switch MSP to operation Mode B")]
        public void SwitchModeB()
        {
            SwitchMspToOperationModeB = string.Empty;
            SendCommand("smB", out SwitchMspToOperationModeB);
        }

        [Description("Set bit time")]
        public void SetBitTime()
        {
            SetBitTimeInMicrosecond = string.Empty;
            SendCommand("sbt0064", out SetBitTimeInMicrosecond);
        }

        [Description("Supply voltage on")]
        public void VsupOn()
        {
            SupplyVoltageOn = string.Empty;
            SendCommand("vho1", out SupplyVoltageOn);
        }

        [Description("Supply voltage off")]
        public void VsupOff()
        {
            SupplyVoltageOff = string.Empty;
            SendCommand("vho0", out SupplyVoltageOff);
        }

        [Description("Start listening mode (0x22A2)")]
        public void StartListeningMode()
        {
            ListeningMode0X22A2 = string.Empty;
            SendCommand("xxw7522A2", out ListeningMode0X22A2); // xxw7522A218
        }

        [Description("Start Programming Mode (0x2EAE)")]
        public void StartProgrammingMode()
        {
            ProgrammingMode0X2Eae = string.Empty;
            SendCommand("xxw752EAE", out ProgrammingMode0X2Eae); // xxw752EAEF3
        }

        [Description("ReadAddr0x75")]
        public void ReadAddr0X75()
        {
            ReadAddr0x75Result = string.Empty;
            ReadOneRegisterFromEEPROM(0x75, out ReadAddr0x75Result);
        }

        [Description("Generate Reset (0x0006)")]
        public void GenerateReset()
        {
            GenerateReset0X0006 = string.Empty;
            SendCommand("xxw750006", out GenerateReset0X0006); // xxw75000620
        }

        [Description("Select I/O Channel1/2")]
        public void SelectIoChannel(int ch)
        {
            IoChannel = string.Empty;
            if (ch != 1 && ch != 2)
                return;
            IoChannel = string.Empty;
            SendCommand(string.Format("ftses{0}", ch), out IoChannel); // ftses1/2
        }

        public void WriteEeprom(byte addr, string value)
        {
            string recv;
            SendCommand(string.Format("xxw{0}{1}", ValueHelper.GetHextStr(addr), value), out recv);
        }

        public bool ReadOneRegisterFromEEPROM(byte addr, out string value)
        {
            value = string.Empty;

            string recv;
            if (SendCommand(string.Format("xxr{0}", ValueHelper.GetHextStr(addr)), out recv))
            {
                if (!string.IsNullOrEmpty(recv) && recv.StartsWith("0:") && recv.Length == 2 + 6)
                {
                    try
                    {
                        var data1 = Convert.ToByte(recv.Substring(2, 2), 16);
                        var data2 = Convert.ToByte(recv.Substring(4, 2), 16);
                        var data3 = Convert.ToByte(recv.Substring(6, 2), 16);

                        value = recv.Substring(2, 4);
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public bool ReadOneRegisterFromEEPROMExt(byte addr, out string value)
        {
            value = string.Empty;

            //string recv;
            //SendCommand(string.Format("xxr{0}", ValueHelper.GetHextStr(addr)), out recv);

            var addr75 = (ushort)(0xD070 + addr);
            string recv75;
            var cmd75Bits = Convert.ToString(addr75, 2).PadLeft(16, '0');
            var cmd75Byte0 = Convert.ToByte(cmd75Bits.Substring(0, 8), 2);
            var cmd75Byte1 = Convert.ToByte(cmd75Bits.Substring(8, 8), 2);
            var cmd75 = string.Format("xxw{0}{1}{2}", ValueHelper.GetHextStr(0x75), ValueHelper.GetHextStr(cmd75Byte0), ValueHelper.GetHextStr(cmd75Byte1));

            SendCommand(cmd75, out recv75);
            if (!string.IsNullOrEmpty(recv75) && !string.Equals(recv75, "0:000000", StringComparison.CurrentCultureIgnoreCase))
                return false;

            string recv;
            if (SendCommand(string.Format("xxr{0}", ValueHelper.GetHextStr(0x75)), out recv))
            {
                if (!string.IsNullOrEmpty(recv) && recv.StartsWith("0:") && recv.Length == 2 + 6)
                {
                    if (recv.ToLower().StartsWith("0:00A8".ToLower()))
                    {
                        string recv6E;
                        if (SendCommand(string.Format("xxr{0}", ValueHelper.GetHextStr(0x6E)), out recv6E))
                        {
                            if (!string.IsNullOrEmpty(recv6E) && recv6E.StartsWith("0:") && recv6E.Length == 2 + 6)
                            {
                                var data1 = Convert.ToByte(recv6E.Substring(2, 2), 16);
                                var data2 = Convert.ToByte(recv6E.Substring(4, 2), 16);
                                var data3 = Convert.ToByte(recv6E.Substring(6, 2), 16);

                                value = recv6E.Substring(2, 4);
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool StoringOneRegisterToEEPROM(byte addr, out string errorMsg)
        {
            errorMsg = string.Empty;

            var addrBits = Convert.ToString(addr, 2).PadLeft(8, '0').Substring(1, 7);
            var pmCmd = string.Format("01{0}{1}", addrBits, addrBits);
            var pmCmd0 = Convert.ToByte(pmCmd.Substring(0, 8), 2);
            var pmCmd1 = Convert.ToByte(pmCmd.Substring(8, 8), 2);

            var cmd = string.Format("xxw{0}{1}", ValueHelper.GetHextStr(0x75), string.Format("{0}{1}", ValueHelper.GetHextStr(pmCmd0), ValueHelper.GetHextStr(pmCmd1)));
            string recv;
            SendCommand(cmd, out recv);

            if (!string.IsNullOrEmpty(recv) && !string.Equals(recv, "0:000000", StringComparison.CurrentCultureIgnoreCase))
            {
                errorMsg = string.Format("'{0}' Failed: {1}", cmd, recv);
                return false;
            }

            return true;
        }

        private bool WriteAndStoreRegisterToEeprom(byte addr, string value, out string errorMsg)
        {
            string recv;
            var cmd = string.Format("xxw{0}{1}", ValueHelper.GetHextStr(addr), value);
            SendCommand(cmd, out recv);

            if (!string.IsNullOrEmpty(recv) && !string.Equals(recv, "0:000000", StringComparison.CurrentCultureIgnoreCase))
            {
                errorMsg = string.Format("'{0}' Failed: {1}", cmd, recv);
                return false;
            }

            return StoringOneRegisterToEEPROM(addr, out errorMsg);
        }

        public bool WriteAndStoreRegisterToEepromExt(byte addr, string value, out string errorMsg)
        {
            errorMsg = string.Empty;

            // Prepare DATA: Addr=0x6E, DATA=data ==> xxw6E000043
            // Write ADDR: ADDR=0x75, DATA=0xF070+addr ==> xxw75F070D5
            // Read ADDR: ADDR=0x75, STS = DATA,==> xxr75
            // Store: ==>xxw75685075
            // xxw6E000043==>xxw75F070D5==>xxr75==>xxw75685075==>...==>xxw6E3E78DC==>xxw75F07F6E==>xxr75==>xxw756FDF11

            // Prepare DATA: Addr=0x6E, DATA=data ==> xxw6E000043
            string recv6E;
            var cmd6E = string.Format("xxw{0}{1}", ValueHelper.GetHextStr(0x6E), value);
            SendCommand(cmd6E, out recv6E);
            if (!string.IsNullOrEmpty(recv6E) && !string.Equals(recv6E, "0:000000", StringComparison.CurrentCultureIgnoreCase))
            {
                errorMsg = string.Format("'{0}' Failed: {1}", cmd6E, recv6E);
                return false;
            }

            // Write ADDR: ADDR=0x75, DATA=0xF070+addr ==> xxw75F070D5
            var addr75 = (ushort)(0xF070 + addr);
            string recv75;
            var cmd75Bits = Convert.ToString(addr75, 2).PadLeft(16, '0');
            var cmd75Byte0 = Convert.ToByte(cmd75Bits.Substring(0, 8), 2);
            var cmd75Byte1 = Convert.ToByte(cmd75Bits.Substring(8, 8), 2);
            var cmd75 = string.Format("xxw{0}{1}{2}", ValueHelper.GetHextStr(0x75), ValueHelper.GetHextStr(cmd75Byte0), ValueHelper.GetHextStr(cmd75Byte1));
            SendCommand(cmd75, out recv75);
            if (!string.IsNullOrEmpty(recv75) && !string.Equals(recv75, "0:000000", StringComparison.CurrentCultureIgnoreCase))
            {
                errorMsg = string.Format("'{0}' Failed: {1}", cmd75, recv75);
                return false;
            }

            // Read ADDR: ADDR=0x75, STS = DATA,==> xxr75
            string xxr75Recv;
            if (ReadOneRegisterFromEEPROM(0x75, out xxr75Recv))
            {
                if (string.Equals(xxr75Recv, "00A8", StringComparison.CurrentCultureIgnoreCase))
                {
                    // Store: ==>xxw75685075
                    return StoringOneRegisterToEEPROM((byte)(0x50 + addr), out errorMsg);
                }
            }

            return false;
        }

        private string TransferDecimalToHexString(decimal value)
        {
            // 8192=0x2000
            // -20374=0xB06A  
            // -9882=0xD965

            if (value < 0)
            {
                var data = (uint)(0 - value);
                var dataBits = Convert.ToString(data, 2).PadLeft(16, '0');
                var dataBits2 = string.Empty;

                if (data == 0)
                    return "0000";

                foreach (var t in dataBits)
                {
                    if (t.ToString() == "1")
                        dataBits2 += "0";
                    else
                        dataBits2 += "1";
                }

                var data2 = Convert.ToUInt16(dataBits2, 2) + 1;
                var data2Bytes = BitConverter.GetBytes(data2);

                var str = string.Format("{0}{1}", ValueHelper.GetHextStr(data2Bytes[1]), ValueHelper.GetHextStr(data2Bytes[0]));
                return str;
            }
            else
            {
                var data = (uint)(value);
                var dataBits = Convert.ToString(data, 2).PadLeft(16, '0');
                var dataBits2 = dataBits;

                var data2 = Convert.ToUInt16(dataBits2, 2);
                var data2Bytes = BitConverter.GetBytes(data2);

                var str = string.Format("{0}{1}", ValueHelper.GetHextStr(data2Bytes[1]), ValueHelper.GetHextStr(data2Bytes[0]));
                return str;
            }
        }

        private decimal TransferHexToDecimal(string hexValue)
        {
            var toFormatValue = hexValue.Replace("0x", "");

            var b0 = Convert.ToByte(toFormatValue.Substring(0, 2), 16);
            var b1 = Convert.ToByte(toFormatValue.Substring(2, 2), 16);

            var bits0 = Convert.ToString(b0, 2).PadLeft(8, '0');
            var bits1 = Convert.ToString(b1, 2).PadLeft(8, '0');
            var data16Bit = bits0 + bits1;

            var datasBits2 = string.Empty;
            if (data16Bit.StartsWith("1"))
            {
                foreach (var t in data16Bit)
                {
                    if (t.ToString() == "1")
                        datasBits2 += "0";
                    else
                        datasBits2 += "1";
                }
            }
            else
            {
                datasBits2 = data16Bit;
            }

            decimal output;
            if (data16Bit.StartsWith("1"))
                output = (0 - Convert.ToUInt16(datasBits2, 2)) - 1;
            else
                output = Convert.ToUInt16(datasBits2, 2);

            return output;
        }

        private readonly object _lockFile = new object();

        [Description("将参数文件存入EEPROM")]
        public void StoreEepromFromParaFile()
        {
            /*The EXT EEPROM register numbers follow directly on the EEPROM register
                numbers(total register number is 96).The EEPROM register numbers start at
            0x00 and end at 0x4F.The EXT EEPROM register numbers start at 0x50 and
                end at 0x5F*/

            StoreEepromResult = string.Empty;

            if (string.IsNullOrEmpty(EepromParaFilePath))
            {
                StoreEepromResult = "NG 未指定参数文件";
                return;
            }

            if (!File.Exists(EepromParaFilePath))
            {
                StoreEepromResult = "NG 参数文件不存在";
                return;
            }

            List<dynamic> rows;

            lock (_lockFile)
            {
                try
                {
                    rows = MiniExcel.Query(EepromParaFilePath, excelType: ExcelType.CSV).ToList();
                }
                catch (Exception e)
                {
                    StoreEepromResult = "NG 读取参数文件失败：" + e.Message;
                    return;
                }
            }

            if (!rows.Any())
            {
                StoreEepromResult = "NG 读取参数文件失败，参数未空";
                return;
            }

            var infoStartIndex = rows.FindIndex(f => f.A.ToString() == @"EEPROM:");

            // EEPROM:
            // AddrNr.	Value	Value (EXT)
            if (infoStartIndex == 0 && rows.Count >= 130 &&
                rows[1].A.ToString() == @"AddrNr." &&
                rows[1].B.ToString() == @"Value" &&
                rows[1].C.ToString() == @"Value (EXT)")
            {
                var dicParas = new Dictionary<int, decimal[]>();
                for (var i = 0; i < 128; i++)
                    dicParas.Add(i, new decimal[2]);

                var isCheckParasOk = true;
                var currentAddr = 0;
                var checkFileErrorMsg = string.Empty;
                for (var i = 2; i < 130; i++)
                {
                    var iRowA = rows[i].A.ToString();
                    var iRowB = rows[i].B.ToString();
                    var iRowC = rows[i].C.ToString();

                    var tempAddr = 0;
                    var tempValue = 0;
                    var tempValueExt = 0;
                    if (int.TryParse(iRowA, out tempAddr) && int.TryParse(iRowB, out tempValue) && int.TryParse(iRowC, out tempValueExt))
                    {
                        if (currentAddr == tempAddr && (tempValue >= -65535 && tempValue <= 65535) && (tempValueExt >= -65535 && tempValueExt <= 65535))
                        {
                            dicParas[currentAddr][0] = tempValue;
                            dicParas[currentAddr][1] = tempValueExt;
                            currentAddr++;
                        }
                        else
                        {
                            isCheckParasOk = false;
                            checkFileErrorMsg += string.Format(@"Row[{0}] Error", i);
                            break;
                        }
                    }
                    else
                    {
                        isCheckParasOk = false;
                        checkFileErrorMsg += string.Format(@"Row[{0}] Error", i);
                        break;
                    }
                }

                if (!isCheckParasOk)
                {
                    StoreEepromResult = "NG " + checkFileErrorMsg;
                    return;
                }

                for (var i = 0; i <= 0x4F; i++)
                {
                    var addr = i;
                    var value = TransferDecimalToHexString(dicParas[i][0]);                   
                    string errorMsg;
                    if (WriteAndStoreRegisterToEeprom((byte)i, value, out errorMsg))
                        continue;
                    StoreEepromResult = "NG " + errorMsg;
                    return;
                }

                for (var i = 0; i < 16; i++)
                {
                    var addr = i;
                    var value = TransferDecimalToHexString(dicParas[i][1]);
                    string errorMsg;
                    if (WriteAndStoreRegisterToEepromExt((byte)i, value, out errorMsg))
                        continue;
                    StoreEepromResult = "NG " + errorMsg;
                    return;
                }

                GenerateReset();
                if (!string.Equals(GenerateReset0X0006, "0:000000"))
                {
                    StoreEepromResult = "NG GenerateReset Fail " + GenerateReset0X0006;
                    return;
                }

                Thread.Sleep(150);
                StartListeningMode();
                if (!string.Equals(ListeningMode0X22A2, "0:000000") && !string.Equals(ListeningMode0X22A2, "1:000000"))
                {
                    StoreEepromResult = "NG Start Listening Mode Fail " + ListeningMode0X22A2;
                    return;
                }

                Thread.Sleep(25);
                StartProgrammingMode();
                if (!string.Equals(ProgrammingMode0X2Eae, "0:000000"))
                {
                    StoreEepromResult = "NG Start Programming Mode Fail " + ProgrammingMode0X2Eae;
                    return;
                }

                SelectIoChannel(1);
                Thread.Sleep(25);

                Thread.Sleep(50);
                for (var i = 0; i <= 0x4F; i++)
                {
                    var expectedValue = TransferDecimalToHexString(dicParas[i][0]);
                    var addr = (byte)i;
                    string readResult;
                    if (ReadOneRegisterFromEEPROM(addr, out readResult))
                    {
                        if (string.Equals(readResult, expectedValue, StringComparison.CurrentCultureIgnoreCase))
                            continue;

                        StoreEepromResult =
                            string.Format("NG READ ADDR '{0}' Faild, Actual={1}, Expected={2}",
                                ValueHelper.GetHextStrWithOx(addr), readResult, expectedValue);
                        return;
                    }
                    else
                    {
                        StoreEepromResult =
                            string.Format("NG READ ADDR '{0}' Faild", ValueHelper.GetHextStrWithOx(addr));
                        return;
                    }
                }

                for (var i = 0; i < 16; i++)
                {
                    var expectedValue = TransferDecimalToHexString(dicParas[i][1]);
                    var addr = (byte)i;
                    string readResult;
                    if (ReadOneRegisterFromEEPROMExt(addr, out readResult))
                    {
                        if (string.Equals(readResult, expectedValue, StringComparison.CurrentCultureIgnoreCase))
                            continue;

                        StoreEepromResult =
                            string.Format("NG READ ADDR(EXT) '{0}' Faild, Actual={1}, Expected={2}",
                                ValueHelper.GetHextStrWithOx(addr), readResult, expectedValue);
                        return;
                    }
                    else
                    {
                        StoreEepromResult =
                            string.Format("NG READ ADDR(EXT) '{0}' Faild", ValueHelper.GetHextStrWithOx(addr));
                        return;
                    }
                }

                StoreEepromResult = @"OK";
                return;
            }
            else
            {
                StoreEepromResult = "NG 读取参数文件失败，ROW0或ROW1格式不正确";
                return;
            }
        }

        public void ReadCalibrationPoint1()
        {
            _calibrationPoint1List.Clear();
            //_calibrationPoint1 = -9857;

            for (var i = 0; i < 20; i++)
            {
                string toFormatValue;
                if (ReadOneRegisterFromEEPROM(0x59, out toFormatValue))
                {
                    var b0 = Convert.ToByte(toFormatValue.Substring(0, 2), 16);
                    var b1 = Convert.ToByte(toFormatValue.Substring(2, 2), 16);

                    var bits0 = Convert.ToString(b0, 2).PadLeft(8, '0');
                    var bits1 = Convert.ToString(b1, 2).PadLeft(8, '0');
                    var data16Bit = bits0 + bits1;

                    var datasBits2 = string.Empty;
                    if (data16Bit.StartsWith("1"))
                    {
                        foreach (var t in data16Bit)
                        {
                            if (t.ToString() == "1")
                                datasBits2 += "0";
                            else
                                datasBits2 += "1";
                        }
                    }
                    else
                    {
                        datasBits2 = data16Bit;
                    }

                    float output;
                    if (data16Bit.StartsWith("1"))
                        output = (0f - Convert.ToUInt16(datasBits2, 2)) + 1;
                    else
                        output = Convert.ToUInt16(datasBits2, 2);

                    _calibrationPoint1List.Add((decimal)output);
                }
            }
        }

        public void ReadCalibrationPoint2()
        {
            //_calibrationPoint2 = -29948;
            _calibrationPoint2List.Clear();

            for (var i = 0; i < 20; i++)
            {
                string toFormatValue;
                if (ReadOneRegisterFromEEPROM(0x59, out toFormatValue))
                {
                    var b0 = Convert.ToByte(toFormatValue.Substring(0, 2), 16);
                    var b1 = Convert.ToByte(toFormatValue.Substring(2, 2), 16);

                    var bits0 = Convert.ToString(b0, 2).PadLeft(8, '0');
                    var bits1 = Convert.ToString(b1, 2).PadLeft(8, '0');
                    var data16Bit = bits0 + bits1;

                    var datasBits2 = string.Empty;
                    if (data16Bit.StartsWith("1"))
                    {
                        foreach (var t in data16Bit)
                        {
                            if (t.ToString() == "1")
                                datasBits2 += "0";
                            else
                                datasBits2 += "1";
                        }
                    }
                    else
                    {
                        datasBits2 = data16Bit;
                    }

                    float output;
                    if (data16Bit.StartsWith("1"))
                        output = (0f - Convert.ToUInt16(datasBits2, 2)) + 1;
                    else
                        output = Convert.ToUInt16(datasBits2, 2);

                    _calibrationPoint2List.Add((decimal)output);
                }
            }
        }

        //private decimal _calibrationPoint1;
        private readonly List<decimal> _calibrationPoint1List = new List<decimal>();
        //private decimal _calibrationPoint2;
        private readonly List<decimal> _calibrationPoint2List = new List<decimal>();

        [Description("R/W,CP1设置")]
        public float SpCp1 = -25000f;
        [Description("R/W,CP1设置")]
        public float SpCp2 = 25000f;
        [Description("R/W,TargetValue1设置")]
        public float TargetValue1 = -29491f;
        [Description("R/W,TargetValue2设置")]
        public float TargetValue2 = 29491f;

        [Description("是否是Decreasing")]
        public bool IsDecreasing = true;

        [Description("计算传感器标定值")]
        public void CalculateSensorThenCalibrationEeprom()
        {
            CalibrationResult = string.Empty;

            if (_calibrationPoint1List.Count != 20)
            {
                CalibrationResult = "NG 第一个标定点未采集满20组数据";
                return;
            }

            if (_calibrationPoint2List.Count != 20)
            {
                CalibrationResult = "NG 第二个标定点未采集满20组数据";
                return;
            }

            var angleOut1Pos1 = (_calibrationPoint1List.Min() + _calibrationPoint1List.Max()) / 2;
            var angleOut1Pos2 = (_calibrationPoint2List.Min() + _calibrationPoint2List.Max()) / 2;

            var angleOut1Range = IsDecreasing ? -(angleOut1Pos2 - angleOut1Pos1) : +(angleOut1Pos2 - angleOut1Pos1);
            if (angleOut1Range < 0)
                angleOut1Range += (decimal)65536f;

            var angleOut1RangeHalf = IsDecreasing ? -(angleOut1Range / 2) : +(angleOut1Range / 2);
            var refAngle0Ch1 = 65536f - ((double)angleOut1Pos1 + (double)angleOut1RangeHalf);
            if (refAngle0Ch1 > 32767)
                refAngle0Ch1 -= 65536f;
            var refAngleCh1 = refAngle0Ch1 + 0 + 0;

            var refAngleOut1Pos1 = ExcelMod((double)angleOut1Pos1 + refAngleCh1 + Math.Pow(2, 15), Math.Pow(2, 16)) - Math.Pow(2, 15);
            var refAngleOut1Pos2 = ExcelMod((double)angleOut1Pos2 + refAngleCh1 + Math.Pow(2, 15), Math.Pow(2, 16)) - Math.Pow(2, 15);

            var spGain = (SpCp2 - SpCp1) / (refAngleOut1Pos2 - refAngleOut1Pos1);

            var nmult1 = 0;
            var absSpGain = Math.Abs(spGain);
            if (absSpGain > 1)
            {
                var logSpGain = Math.Log(absSpGain, 2);
                var roundUpSpGain = Math.Ceiling(logSpGain);
                nmult1 = (int)roundUpSpGain;
            }

            if (nmult1 < 0 || nmult1 > 7)
            {
                CalibrationResult = @"NG nmult1=" + nmult1;
                return;
            }

            var spGainCh1 = 32768f * (spGain / Math.Pow(2, nmult1));

            //var spOffsetCh1 = 0f;//sp_cp1 - (double)(ANGLE_OUT_1pos1 * (decimal)SP_Gain);
            var spOffsetCh1 = SpCp1 - refAngleOut1Pos1 * spGain;

            var outGainCh1 = 16384f * ((TargetValue2 - TargetValue1) / (SpCp2 - SpCp1));
            var outOffsetCh1 = TargetValue1 - ((outGainCh1 / 16384F) * SpCp1);

            var strRefAngle0Ch1 = TransferDecimalToHexString((decimal)refAngle0Ch1);
            var strNmult1 = TransferDecimalToHexString(nmult1);
            var strSpGainCh1 = TransferDecimalToHexString((decimal)spGainCh1);
            var strSpOffsetCh1 = TransferDecimalToHexString((decimal)spOffsetCh1);
            var strOutGainCh1 = TransferDecimalToHexString((decimal)outGainCh1);
            var strOutOffetCh1 = TransferDecimalToHexString((decimal)outOffsetCh1);
            var strSetupDatapathRegister = string.Format("00{0}", ValueHelper.GetHextStr(Convert.ToByte(string.Format("{0}00000", Convert.ToString(nmult1, 2).PadLeft(3, '0')), 2)));

            var caliWriteFunc = new Func<bool>(() =>
            {
                SelectIoChannel(1);
                Thread.Sleep(25);

                string recv00;
                if (!ReadOneRegisterFromEEPROM(0x00, out recv00))
                {
                    CalibrationResult = "NG xxr00 Failed";
                    return false;
                }

                Thread.Sleep(25);
                string recv48Error;
                if (!WriteAndStoreRegisterToEeprom(0x48, strOutGainCh1, out recv48Error))
                {
                    CalibrationResult = "NG " + recv48Error;
                    return false;
                }

                Thread.Sleep(25);
                string recv47Error;
                if (!WriteAndStoreRegisterToEeprom(0x47, strOutOffetCh1, out recv47Error))
                {
                    CalibrationResult = "NG " + recv47Error;
                    return false;
                }

                Thread.Sleep(25);
                string recv00Error;
                if (!WriteAndStoreRegisterToEeprom(0x00, strSetupDatapathRegister, out recv00Error))
                {
                    CalibrationResult = "NG " + recv00Error;
                    return false;
                }

                Thread.Sleep(25);
                string recv19Error;
                if (!WriteAndStoreRegisterToEeprom(0x19, strSpGainCh1, out recv19Error))
                {
                    CalibrationResult = "NG " + recv19Error;
                    return false;
                }

                Thread.Sleep(25);
                string recv1EError;
                if (!WriteAndStoreRegisterToEeprom(0x1E, strRefAngle0Ch1, out recv1EError))
                {
                    CalibrationResult = "NG " + recv1EError;
                    return false;
                }

                Thread.Sleep(25);
                string recv18Error;
                if (!WriteAndStoreRegisterToEeprom(0x18, strSpOffsetCh1, out recv18Error))
                {
                    CalibrationResult = "NG " + recv18Error;
                    return false;
                }

                return true;
            });

            var isCaliWriteOk = caliWriteFunc.Invoke();

            if (!isCaliWriteOk)
            {
                GenerateReset();
                Thread.Sleep(150);
                StartListeningMode();
                Thread.Sleep(25);
                StartProgrammingMode();
            }
            else
            {
                GenerateReset();
                if (!string.Equals(GenerateReset0X0006, "0:000000"))
                {
                    CalibrationResult = "NG GenerateReset Fail " + GenerateReset0X0006;
                    return;
                }

                Thread.Sleep(150);
                StartListeningMode();
                if (!string.Equals(ListeningMode0X22A2, "0:000000") && !string.Equals(ListeningMode0X22A2, "1:000000"))
                {
                    CalibrationResult = "NG Start Listening Mode Fail " + ListeningMode0X22A2;
                    return;
                }

                Thread.Sleep(25);
                StartProgrammingMode();
                if (!string.Equals(ProgrammingMode0X2Eae, "0:000000"))
                {
                    CalibrationResult = "NG Start Programming Mode Fail " + ProgrammingMode0X2Eae;
                    return;
                }

                string xxr75, xxr48, xxr47, xxr00, xxr19, xxr1E, xxr18;
                if (ReadOneRegisterFromEEPROM(0x75, out xxr75) && string.Equals(xxr75, "00A8", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (ReadOneRegisterFromEEPROM(0x48, out xxr48) && string.Equals(xxr48, strOutGainCh1, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (ReadOneRegisterFromEEPROM(0x47, out xxr47) && string.Equals(xxr47, strOutOffetCh1, StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (ReadOneRegisterFromEEPROM(0x00, out xxr00) && string.Equals(xxr00, strSetupDatapathRegister, StringComparison.CurrentCultureIgnoreCase))
                            {
                                if (ReadOneRegisterFromEEPROM(0x19, out xxr19) && string.Equals(xxr19, strSpGainCh1, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    if (ReadOneRegisterFromEEPROM(0x1E, out xxr1E) && string.Equals(xxr1E, strRefAngle0Ch1, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        if (ReadOneRegisterFromEEPROM(0x18, out xxr18) && string.Equals(xxr18, strSpOffsetCh1, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            CalibrationResult = "OK";
                                            return;
                                        }
                                        else
                                        {
                                            CalibrationResult = string.Format("NG READ ADDR '{0}' Faild, Actual={1}, Expected={2}", ValueHelper.GetHextStrWithOx(0x18), xxr1E, strSpOffsetCh1);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        CalibrationResult = string.Format("NG READ ADDR '{0}' Faild, Actual={1}, Expected={2}", ValueHelper.GetHextStrWithOx(0x1E), xxr1E, strRefAngle0Ch1);
                                        return;
                                    }
                                }
                                else
                                {
                                    CalibrationResult = string.Format("NG READ ADDR '{0}' Faild, Actual={1}, Expected={2}", ValueHelper.GetHextStrWithOx(0x19), xxr19, strSpGainCh1);
                                    return;
                                }
                            }
                            else
                            {
                                CalibrationResult = string.Format("NG READ ADDR '{0}' Faild, Actual={1}, Expected={2}",
                                    ValueHelper.GetHextStrWithOx(0x00), xxr00, strSetupDatapathRegister);
                                return;
                            }
                        }
                        else
                        {
                            CalibrationResult = string.Format("NG READ ADDR '{0}' Faild, Actual={1}, Expected={2}",
                                ValueHelper.GetHextStrWithOx(0x47), xxr47, strOutOffetCh1);
                            return;
                        }
                    }
                    else
                    {
                        CalibrationResult = string.Format("NG READ ADDR '{0}' Faild, Actual={1}, Expected={2}",
                            ValueHelper.GetHextStrWithOx(0x48), xxr48, strOutGainCh1);
                        return;
                    }
                }
                else
                {
                    CalibrationResult = string.Format("NG READ ADDR '{0}' Faild, Actual={1}, Expected={2}",
                        ValueHelper.GetHextStrWithOx(0x75), xxr75, "00A8");
                    return;
                }
            }
        }

        private static double ExcelMod(double number, double divisor)
        {
            var result = number % divisor;

            if (result < 0)
                result += divisor;

            return result;
        }
    }
}
