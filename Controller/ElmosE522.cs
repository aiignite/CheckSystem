using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class ElmosE522 : ControllerBase
    {
        public MySerialPort MySerialPort;

        //[Description("R,写入后读取ProgStatus结果")]
        public string ReadProgStatusResult = string.Empty;

        [Description("R,读取ID结果")]
        public string ReadIdResult = string.Empty;

        [Description("R,读寄存器结果")]
        public string ReadRegResult = string.Empty;

        [Description("R,OTP编程结果")]
        public string OtpProgramResult = string.Empty;

        [Description("R,配置文件中准备写入OTP的寄存器内容")]
        public string OtpRegToWriteFromConfig = string.Empty;

        [Description("R,OTP读取内容")]
        public string OtpRegRead = string.Empty;

        [Description("R,OTP读取内容与配置文件比较结果")]
        public string OtpReadAndComprareWithConfigResult = string.Empty;

        [Description("R/W,配置文件路径")]
        public string OtpConfigFilePath =
            @"Y:\Projs\2022\芯片相关\E522.49\e522.49配置文件-3.3V\BA\SW0000903(TAIL_C_ADDR1)-3.3V.cfg";

        public ElmosE522(string name)
            : base(name)
        {
            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;
        }

        ~ElmosE522()
        {
            Dispose();
        }

        private bool _isWrite1;
        private string _write1Cmd = string.Empty;
        private bool _isWrite2;
        private string _write2Cmd = string.Empty;
        private bool _isRead;
        private string _readCmd = string.Empty;
        private int _readCount;
        private string _readRecv = string.Empty;

        private const string OkAck = "FC";
        private readonly EventWaitHandle _waitWrite1Handle = new AutoResetEvent(false);
        private readonly EventWaitHandle _waitWrite2Handle = new AutoResetEvent(false);
        private readonly EventWaitHandle _waitReadHandle = new AutoResetEvent(false);

        private static readonly object ConfigFileLocker = new object();
        private readonly string _otpRegMappingConfigFilePath = Directory.GetCurrentDirectory() +
                                                               @"\ControllerConfig\E52248-49-OTP-RegMappingList.txt";
        private readonly Dictionary<int, Dictionary<string, byte>> _otpRegMappingConfigList =
            new Dictionary<int, Dictionary<string, byte>>();
        //private Dictionary<int, Dictionary<byte, ushort>> _configList = new Dictionary<int, Dictionary<byte, ushort>>();

        /// <summary>
        /// 总线消息推送
        /// </summary>
        /// <param name="name"></param>
        /// <param name="datas"></param>
        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (MySerialPort == null || MySerialPort.Name != name)
                return;

            //var str = datas.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t) + " ");
            //Debug.WriteLine("{0}: {1}", name, str);

            if (_isWrite1 || _isWrite2 || _isRead)
            {
                var str = datas.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t) + " ");
                Debug.WriteLine("{0}: {1}", name, str);
                str = str.Replace(" ", "");

                if (!string.IsNullOrEmpty(str))
                {
                    if (_isWrite1 && !string.IsNullOrEmpty(_write1Cmd))
                    {
                        if (_write1Cmd.Length + OkAck.Length == str.Length && str.EndsWith(OkAck))
                        {
                            _isWrite1 = false;
                            _write1Cmd = string.Empty;
                            _waitWrite1Handle.Set();
                        }
                    }
                    else if (_isWrite2 && !string.IsNullOrEmpty(_write2Cmd))
                    {
                        if (_write2Cmd.Length + OkAck.Length == str.Length && str.EndsWith(OkAck))
                        {
                            _isWrite2 = false;
                            _write2Cmd = string.Empty;
                            _waitWrite2Handle.Set();
                        }
                    }
                    else if (_isRead && !string.IsNullOrEmpty(_readCmd) && _readCount > 0 && _readCount <= 16)
                    {
                        var len = _readCount * 10 % 8 == 0 ? _readCount * 10 / 8 : _readCount * 10 / 8 + 1;

                        if (_readCmd.Length + OkAck.Length + len * 2 + 2 == str.Length)
                        {
                            if (str.Substring(_readCmd.Length, OkAck.Length) == OkAck)
                            {
                                var rcvData = str.Substring(_readCmd.Length + OkAck.Length, len * 2);
                                var revCrc8 = str.Substring(_readCmd.Length + OkAck.Length + len * 2, 2);

                                var datasToCrc8 = new List<byte>();
                                for (var i = 0; i < rcvData.Length; i = i + 2)
                                    datasToCrc8.Add(Convert.ToByte(string.Format("{0}{1}", rcvData[i], rcvData[i + 1]),
                                        16));
                                if (revCrc8 == ValueHelper.GetHextStr(Crc8(datasToCrc8)))
                                {
                                    var tempDataBits = new List<char>();
                                    for (var i = 0; i < rcvData.Length; i = i + 2)
                                        tempDataBits.AddRange(Convert.ToString(
                                                     Convert.ToByte(string.Format("{0}{1}", rcvData[i + 0], rcvData[i + 1]), 16), 2)
                                                     .PadLeft(8, '0').ToCharArray().Reverse());

                                    var tempData = string.Empty;
                                    for (var i = 0; i < _readCount * 10; i = i + 10)
                                    {
                                        var s = string.Format("000000{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                                            tempDataBits[i + 9],
                                            tempDataBits[i + 8],
                                            tempDataBits[i + 7],
                                            tempDataBits[i + 6],
                                            tempDataBits[i + 5],
                                            tempDataBits[i + 4],
                                            tempDataBits[i + 3],
                                            tempDataBits[i + 2],
                                            tempDataBits[i + 1],
                                            tempDataBits[i + 0]);

                                        var ushortValue = BitConverter.GetBytes(Convert.ToUInt16(s, 2)).Reverse().ToList();
                                        tempData += ValueHelper.GetHextStr(ushortValue[0]) +
                                                    ValueHelper.GetHextStr(ushortValue[1]) + " ";
                                    }
                                    tempData = tempData.Trim();

                                    _isRead = false;
                                    _readCmd = string.Empty;
                                    _readCount = 0;
                                    _readRecv = tempData;
                                    _waitReadHandle.Set();
                                }
                            }
                        }
                    }
                }
            }
        }

        [Description("读总线模式下的LED的占空比")]
        public void Read_BUS_PULSE(string devAddressDec)
        {
            ClearReadResult("BusPules");

            byte devAddr;
            if (!byte.TryParse(devAddressDec, out devAddr))
                return;

            if (devAddr > 0x1F)
                return;

            string temp;
            if (!ReadData(Format3HeaderBytes(0x40, devAddr, false, 16), 16, out temp))
                return;
            var sp = temp.Split(' ');
            for (var i = 0; i < 16; i++)
            {
                var ushortValue = Convert.ToByte(sp[i].Substring(0, 2), 16) * 256 +
                                  Convert.ToByte(sp[i].Substring(2, 2), 16);
                var s = 100f / 1023f * ushortValue;
                var x = (int)Math.Round(s, 0);
                SetRegValue("BusPules", i, string.Format("{0}%", x));
            }
        }

        [Description("读总线模式下的LED的电流")]
        public void Read_BUS_CURRENT(string devAddressDec)
        {
            ClearReadResult("BusCurrent");

            byte devAddr;
            if (!byte.TryParse(devAddressDec, out devAddr))
                return;

            if (devAddr > 0x1F)
                return;

            string temp;
            if (!ReadData(Format3HeaderBytes(0x50, devAddr, false, 16), 16, out temp))
                return;
            var sp = temp.Split(' ');
            for (var i = 0; i < 16; i++)
            {
                var ushortValue = Convert.ToByte(sp[i].Substring(0, 2), 16) * 256 +
                                  Convert.ToByte(sp[i].Substring(2, 2), 16);
                var s = 0.1f * ushortValue;
                var x = (int)Math.Round(s, 0);
                SetRegValue("BusCurrent", i, string.Format("{0}mA", x));
            }
        }

        [Description("读LED通道使能")]
        public void Read_LED_ENABLE(string devAddressDec)
        {
            ClearReadResult("LedEnable");

            byte devAddr;
            if (!byte.TryParse(devAddressDec, out devAddr))
                return;

            if (devAddr > 0x1F)
                return;

            string temp;
            if (!ReadData(Format3HeaderBytes(0x62, devAddr, false, 2), 2, out temp))
                return;
            var sp = temp.Split(' ');
            var bits1 =
                Convert.ToString(Convert.ToByte(sp[0].Substring(2, 2), 16), 2).PadLeft(8, '0').Reverse().ToList();
            LedEnable0 = bits1[0].ToString() == "1" ? "On" : "Off";
            LedEnable1 = bits1[1].ToString() == "1" ? "On" : "Off";
            LedEnable2 = bits1[2].ToString() == "1" ? "On" : "Off";
            LedEnable3 = bits1[3].ToString() == "1" ? "On" : "Off";
            LedEnable4 = bits1[4].ToString() == "1" ? "On" : "Off";
            LedEnable5 = bits1[5].ToString() == "1" ? "On" : "Off";
            LedEnable6 = bits1[6].ToString() == "1" ? "On" : "Off";
            LedEnable7 = bits1[7].ToString() == "1" ? "On" : "Off";

            var bits2 =
                Convert.ToString(Convert.ToByte(sp[1].Substring(2, 2), 16), 2).PadLeft(8, '0').Reverse().ToList();
            LedEnable8 = bits2[0].ToString() == "1" ? "On" : "Off";
            LedEnable9 = bits2[1].ToString() == "1" ? "On" : "Off";
            LedEnable10 = bits2[2].ToString() == "1" ? "On" : "Off";
            LedEnable11 = bits2[3].ToString() == "1" ? "On" : "Off";
            LedEnable12 = bits2[4].ToString() == "1" ? "On" : "Off";
            LedEnable13 = bits2[5].ToString() == "1" ? "On" : "Off";
            LedEnable14 = bits2[6].ToString() == "1" ? "On" : "Off";
            LedEnable15 = bits2[7].ToString() == "1" ? "On" : "Off";
        }

        [Description("读通道的LED电压值")]
        public void Read_RESULT_VLED(string devAddressDec)
        {
            ClearReadResult("ResultVled");

            byte devAddr;
            if (!byte.TryParse(devAddressDec, out devAddr))
                return;

            if (devAddr > 0x1F)
                return;

            string temp;
            if (!ReadData(Format3HeaderBytes(0x80, devAddr, false, 16), 16, out temp))
                return;
            var sp = temp.Split(' ');
            for (var i = 0; i < 16; i++)
            {
                var ushortValue = Convert.ToByte(sp[i].Substring(0, 2), 16) * 256 +
                                  Convert.ToByte(sp[i].Substring(2, 2), 16);
                var s = ushortValue / 36f;
                var x = (int)Math.Round(s, 0);
                SetRegValue("ResultVled", i, string.Format("{0}V", x));
            }
        }

        [Description("读供电电压与LED电压差值")]
        public void Read_RESULT_VDIF(string devAddressDec)
        {
            ClearReadResult("ResultVdif");

            byte devAddr;
            if (!byte.TryParse(devAddressDec, out devAddr))
                return;

            if (devAddr > 0x1F)
                return;

            string temp;
            if (!ReadData(Format3HeaderBytes(0x90, devAddr, false, 16), 16, out temp))
                return;
            var sp = temp.Split(' ');
            for (var i = 0; i < 16; i++)
            {
                var ushortValue = Convert.ToByte(sp[i].Substring(0, 2), 16) * 256 +
                                  Convert.ToByte(sp[i].Substring(2, 2), 16);
                var s = ushortValue / 36f;
                var x = (int)Math.Round(s, 0);
                SetRegValue("ResultVdif", i, string.Format("{0}V", x));
            }
        }

        [Description("读LED电流值")]
        public void Read_RESULT_ILED(string devAddressDec)
        {
            ClearReadResult("ResultIled");

            byte devAddr;
            if (!byte.TryParse(devAddressDec, out devAddr))
                return;

            if (devAddr > 0x1F)
                return;

            string temp;
            if (!ReadData(Format3HeaderBytes(0xA0, devAddr, false, 16), 16, out temp))
                return;
            var sp = temp.Split(' ');
            for (var i = 0; i < 16; i++)
            {
                var ushortValue = Convert.ToByte(sp[i].Substring(0, 2), 16) * 256 +
                                  Convert.ToByte(sp[i].Substring(2, 2), 16);
                var s = 0.1f * ushortValue;
                var x = (int)Math.Round(s, 0);
                SetRegValue("ResultIled", i, string.Format("{0}mA", x));
            }
        }

        [Description("读LED开路短路状态")]
        public void Read_LED_OPEN_SHORT(string devAddressDec)
        {
            ClearReadResult("LedOpen");
            ClearReadResult("LedShort");

            byte devAddr;
            if (!byte.TryParse(devAddressDec, out devAddr))
                return;

            if (devAddr > 0x1F)
                return;

            string temp;
            if (!ReadData(Format3HeaderBytes(0xB8, devAddr, false, 4), 4, out temp))
                return;
            var sp = temp.Split(' ');
            var bits1 =
                Convert.ToString(Convert.ToByte(sp[0].Substring(2, 2), 16), 2).PadLeft(8, '0').Reverse().ToList();
            LedOpen0 = bits1[0].ToString() == "1" ? "On" : "Off";
            LedOpen1 = bits1[1].ToString() == "1" ? "On" : "Off";
            LedOpen2 = bits1[2].ToString() == "1" ? "On" : "Off";
            LedOpen3 = bits1[3].ToString() == "1" ? "On" : "Off";
            LedOpen4 = bits1[4].ToString() == "1" ? "On" : "Off";
            LedOpen5 = bits1[5].ToString() == "1" ? "On" : "Off";
            LedOpen6 = bits1[6].ToString() == "1" ? "On" : "Off";
            LedOpen7 = bits1[7].ToString() == "1" ? "On" : "Off";

            var bits2 =
                Convert.ToString(Convert.ToByte(sp[1].Substring(2, 2), 16), 2).PadLeft(8, '0').Reverse().ToList();
            LedOpen8 = bits2[0].ToString() == "1" ? "On" : "Off";
            LedOpen9 = bits2[1].ToString() == "1" ? "On" : "Off";
            LedOpen10 = bits2[2].ToString() == "1" ? "On" : "Off";
            LedOpen11 = bits2[3].ToString() == "1" ? "On" : "Off";
            LedOpen12 = bits2[4].ToString() == "1" ? "On" : "Off";
            LedOpen13 = bits2[5].ToString() == "1" ? "On" : "Off";
            LedOpen14 = bits2[6].ToString() == "1" ? "On" : "Off";
            LedOpen15 = bits2[7].ToString() == "1" ? "On" : "Off";

            var bits3 =
                Convert.ToString(Convert.ToByte(sp[2].Substring(2, 2), 16), 2).PadLeft(8, '0').Reverse().ToList();
            LedShort0 = bits3[0].ToString() == "1" ? "On" : "Off";
            LedShort1 = bits3[1].ToString() == "1" ? "On" : "Off";
            LedShort2 = bits3[2].ToString() == "1" ? "On" : "Off";
            LedShort3 = bits3[3].ToString() == "1" ? "On" : "Off";
            LedShort4 = bits3[4].ToString() == "1" ? "On" : "Off";
            LedShort5 = bits3[5].ToString() == "1" ? "On" : "Off";
            LedShort6 = bits3[6].ToString() == "1" ? "On" : "Off";
            LedShort7 = bits3[7].ToString() == "1" ? "On" : "Off";

            var bits4 =
                Convert.ToString(Convert.ToByte(sp[3].Substring(2, 2), 16), 2).PadLeft(8, '0').Reverse().ToList();
            LedShort8 = bits4[0].ToString() == "1" ? "On" : "Off";
            LedShort9 = bits4[1].ToString() == "1" ? "On" : "Off";
            LedShort10 = bits4[2].ToString() == "1" ? "On" : "Off";
            LedShort11 = bits4[3].ToString() == "1" ? "On" : "Off";
            LedShort12 = bits4[4].ToString() == "1" ? "On" : "Off";
            LedShort13 = bits4[5].ToString() == "1" ? "On" : "Off";
            LedShort14 = bits4[6].ToString() == "1" ? "On" : "Off";
            LedShort15 = bits4[7].ToString() == "1" ? "On" : "Off";
        }

        [Description("清空所有读取结果")]
        public void ClearAllReadResult()
        {
            ClearReadResult("BusPules");
            ClearReadResult("BusCurrent");
            ClearReadResult("LedEnable");
            ClearReadResult("ResultVled");
            ClearReadResult("ResultVdif");
            ClearReadResult("ResultIled");
            ClearReadResult("LedOpen");
            ClearReadResult("LedShort");
        }

        [Description("读寄存器")]
        public void ReadReg(
            string devAddressDec, string memoryAddrAddressHex, string readCount)
        {
            ReadRegResult = string.Empty;

            byte devAddr;
            if (!byte.TryParse(devAddressDec, out devAddr))
                return;

            if (devAddr > 0x1F)
                return;

            try
            {
                //byte memoryAddr;
                //if (!byte.TryParse(memoryAddrAddressHex, out memoryAddr))
                //    return;

                int numWords;
                if (!int.TryParse(readCount, out numWords))
                    return;

                if (numWords <= 0 || numWords > 16)
                    return;
                string temp;
                if (ReadData(Format3HeaderBytes(Convert.ToByte(memoryAddrAddressHex, 16), devAddr, false, numWords), numWords, out temp))
                    ReadRegResult = temp;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        //[Description("测试读寄存器")]
        //public void DebugTestRead()
        //{
        //    //ReadReg(1.ToString(), "B8", 2.ToString());

        //    //WriteData(Format3HeaderBytes(0x62, 0x01, true, 2), new ushort[2]);

        //    //WriteData(Format3HeaderBytes(0x51, 0x01, true, 15), TestMethod().ToArray());

        //    //WriteData(Format3HeaderBytes(0xC0, 0x03, true, 16),
        //    //    new[]
        //    //    {
        //    //        (ushort) 0, (ushort) 0, (ushort) 0, (ushort) 0, (ushort) 0, (ushort) 0, (ushort) 0, (ushort) 0,
        //    //        (ushort) 0, (ushort) 0, (ushort) 0, (ushort) 0, (ushort) 0, (ushort) 0, (ushort) 0, (ushort) 0
        //    //    });

        //    for (var i = 0; i < int.MaxValue; i++)
        //    {
        //        _isRead = false;
        //        ReadReg(0.ToString(), "B9", 1.ToString());
        //        Thread.Sleep(500);
        //    }

        //    // 00 55 41 61 BF 
        //    // FF FF FF FF FF FF 03 00 C0 FF FF FF FF FF FF FF FF 0F 4E 

        //    //MySerialPort.SendCommand(new byte[] { 0x00, 0x55, 0x41, 0x61, 0xBF });
        //    //Thread.Sleep(5);

        //    //MySerialPort.SendCommand(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x03, 0x00, 0xC0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x0F, 0x4E });
        //    //Thread.Sleep(5);

        //    //var str1 = "0055516187";

        //    //var datasToCrc8 = new List<byte>();
        //    //for (var i = 0; i < str1.Length; i = i + 2)
        //    //    datasToCrc8.Add(Convert.ToByte(string.Format("{0}{1}", str1[i], str1[i + 1]),
        //    //        16));
        //    //MySerialPort.SendCommand(datasToCrc8.ToArray());

        //    //var str2 = "3EFC01E8";
        //}

        [Description("写入ID")]
        public void Write_COM_DEV_ADDR(string devIdToWriteDec)
        {
            // 先通过默认地址1Fh地址，即31dec，切换到Paga1
            // 如没有，再从1~31遍历切换到Paga1
            // 再改地址，再读编程状态

            ReadProgStatusResult = string.Empty;

            if (MySerialPort == null)
            {
                ReadProgStatusResult = "设备未连接";
                return;
            }

            byte value;
            if (!byte.TryParse(devIdToWriteDec, out value))
            {
                ReadProgStatusResult = "输入的目标地址不正确";
                return;
            }

            var validateDevAddrFunc =
                new Func<byte, bool>(
                    p =>
                    {
                        if (!ReadProgStatus(p, 1))
                            return false;
                        Read_COM_DEV_ADDR(p.ToString());
                        if (string.IsNullOrEmpty(ReadIdResult) ||
                            ReadIdResult != p.ToString())
                            return false;
                        ReadProgStatusResult = "OK";
                        return true;
                    });

            var baseDevAddr = (byte)0x1F;
            if (OtpPageSelect(baseDevAddr, 1))
            {
                if (WriteData(Format3HeaderBytes(0xFB, baseDevAddr, true, 1), new ushort[] { value }))
                {
                    if (ReadProgStatus(baseDevAddr, 1))
                    {
                        if (validateDevAddrFunc(baseDevAddr))
                            return;
                    }
                }
            }
            else
            {
                if (OtpPageSelect(value, 1))
                {
                    if (validateDevAddrFunc(value))
                        return;
                }
                else
                {
                    for (var i = 1; i < 31; i++)
                    {
                        baseDevAddr = (byte)i;
                        if (!OtpPageSelect(baseDevAddr, 1))
                            continue;
                        Thread.Sleep(50);
                        if (WriteData(Format3HeaderBytes(0xFB, baseDevAddr, true, 1), new ushort[] { value }))
                        {
                            if (ReadProgStatus(baseDevAddr, 1))
                            {
                                if (validateDevAddrFunc(baseDevAddr))
                                    return;
                            }
                        }
                        break;
                    }
                }
            }

            ReadProgStatusResult = "NG";
        }

        [Description("读取ID")]
        public void Read_COM_DEV_ADDR(string devIdDec)
        {
            // 切换到PAGE1
            // 再读

            ReadIdResult = string.Empty;

            if (MySerialPort == null)
                return;

            byte value;
            if (!byte.TryParse(devIdDec, out value))
                return;

            if (!OtpPageSelect(value, 1))
                return;
            string id;
            if (!ReadData(Format3HeaderBytes(0xFB, value, false, 1), 1, out id))
                return;
            var bits =
                Convert.ToString(Convert.ToByte(id.Substring(2, 2), 16), 2)
                    .PadLeft(8, '0')
                    .Reverse()
                    .ToList();
            // 0x03 = '0000 0011'
            ReadIdResult =
                Convert.ToByte(string.Format("0000{0}{1}{2}{3}", bits[3], bits[2], bits[1], bits[0]), 2)
                    .ToString();
        }

        //[Description("LED Open")]
        //public void LedOpen()
        //{
        //    WriteData(Format3HeaderBytes(0x7E, 1, true, 1),
        //        new ushort[] { 601 });

        //    for (int i = 0; i < int.MaxValue; i++)
        //    {
        //        WriteData(Format3HeaderBytes(0x40, 1, true, 16),
        //            new ushort[]
        //            {
        //                510, 212, 99, 118,
        //                510, 212, 99, 118,
        //                510, 212, 99, 118,
        //                510, 212, 99, 118
        //            });
        //        Thread.Sleep(5);

        //        Read_BUS_PULSE(1.ToString());
        //        Thread.Sleep(5);

        //        WriteData(Format3HeaderBytes(0x50, 1, true, 16),
        //            new ushort[16] { 220, 220, 220, 220, 220, 220, 220, 220, 220, 220, 220, 220, 220, 220, 220, 220 });
        //        Thread.Sleep(5);

        //        WriteData(Format3HeaderBytes(0x62, 1, true, 2),
        //            new ushort[2] { 255, 255 });
        //        Thread.Sleep(5);
        //    }
        //}

        [Description("根据配置文件-先反读出OTP不同处-再改写不同的寄存器值")]
        public void OtpDiffFromConfig(string devAddrDec)
        {
            OtpProgramResult = string.Empty;

            byte devAddr;
            if (!byte.TryParse(devAddrDec, out devAddr))
                return;

            if (string.IsNullOrEmpty(OtpConfigFilePath))
                return;

            if (!File.Exists(OtpConfigFilePath) || !OtpConfigFilePath.EndsWith(".cfg"))
                return;

            #region 读配置文件，取出参数
            var configList = new Dictionary<int, Dictionary<byte, ushort>>();
            {
                lock (ConfigFileLocker)
                {
                    _otpRegMappingConfigList.Clear();
                    for (var i = 0; i < 4; i++)
                        _otpRegMappingConfigList.Add(i, new Dictionary<string, byte>());

                    {
                        var lines = File.ReadAllLines(_otpRegMappingConfigFilePath).ToList();

                        foreach (var l in lines)
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(l))
                                    continue;
                                var regAddr = Convert.ToByte(l.Split(':')[0], 16);
                                var regName = l.Split(':')[1];

                                if (regName.StartsWith("OTP_PAGE0_"))
                                    _otpRegMappingConfigList[0].Add(regName, regAddr);
                                else if (regName.StartsWith("OTP_PAGE1_"))
                                    _otpRegMappingConfigList[1].Add(regName, regAddr);
                                else if (regName.StartsWith("OTP_PAGE2_"))
                                    _otpRegMappingConfigList[2].Add(regName, regAddr);
                                else if (regName.StartsWith("OTP_PAGE3_"))
                                    _otpRegMappingConfigList[3].Add(regName, regAddr);
                            }
                            catch (Exception)
                            {
                                return;
                            }
                        }
                    }
                    {
                        var lines = File.ReadAllLines(OtpConfigFilePath).ToList();

                        for (var i = 0; i < 4; i++)
                            configList.Add(i, new Dictionary<byte, ushort>());

                        foreach (var l in lines)
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(l))
                                    continue;

                                if (!l.StartsWith("OTP_PAGE"))
                                    continue;
                                var pageIndex = int.Parse(l.Substring(8, 1));
                                var regName = l.Split('=')[0];
                                var regValue = ushort.Parse(l.Split('=')[1]);
                                var regAddr = _otpRegMappingConfigList[pageIndex][regName];
                                configList[pageIndex].Add(regAddr, regValue);
                            }
                            catch (Exception)
                            {
                                return;
                            }
                        }
                    }
                }
            }
            #endregion

            if (devAddr < 0x01 && devAddr > 0x1F)
                return;
            {
                var st = new Stopwatch();
                st.Start();

                var isPreReadOk = true;

                var dataToWrite = new Dictionary<int, Dictionary<byte, ushort>>();

                #region 预读，找出与配置不同的Reg
                for (var i = 0; i < 4; i++) // page0~3
                {
                    var pageIndex = (byte)i;
                    var baseRegAddr = 192;

                    for (var k = 0; k < 4; k++) // C0,D0,E0,F0
                    {
                        // 选PAGE
                        if (OtpPageSelect(devAddr, pageIndex))
                        {
                            string read1;
                            if (ReadData(Format3HeaderBytes((byte)baseRegAddr, devAddr, false, 16), 16, out read1))
                            {
                                for (var j = 0; j < 16; j++)
                                {
                                    var readUshort1 = (ushort)
                                        (Convert.ToByte(read1.Split(' ')[j].Substring(0, 2), 16) * 256 +
                                         Convert.ToByte(read1.Split(' ')[j].Substring(2, 2), 16));

                                    if (configList[pageIndex].ContainsKey((byte)(baseRegAddr + j)))
                                    {
                                        if (readUshort1 == configList[pageIndex][(byte)(baseRegAddr + j)])
                                            continue;

                                        if (!dataToWrite.ContainsKey(pageIndex))
                                            dataToWrite.Add(pageIndex, new Dictionary<byte, ushort>());

                                        if (!dataToWrite[pageIndex].ContainsKey((byte)(baseRegAddr + j)))
                                            dataToWrite[pageIndex].Add((byte)(baseRegAddr + j), configList[pageIndex][(byte)(baseRegAddr + j)]);
                                    }
                                }

                                baseRegAddr = baseRegAddr + 16;
                            }
                            else
                            {
                                isPreReadOk = false;
                                break;
                            }
                        }
                        else
                        {
                            isPreReadOk = false;
                            break;
                        }
                    }

                    if (!isPreReadOk)
                    {
                        break;
                    }
                }

                if (!isPreReadOk)
                {
                    st.Stop();
                    OtpProgramResult = "NG";
                    OtpProgramResult += " 耗时" + st.ElapsedMilliseconds / 1000f + "s";
                    return;
                }
                #endregion

                #region 把不同的写进去
                var isWrtiteOk = true;

                foreach (var pageId in dataToWrite.Keys)
                {
                    foreach (var regAddr in dataToWrite[pageId].Keys)
                    {
                        var ushortValue = dataToWrite[pageId][regAddr];

                        // 选PAGE
                        if (OtpPageSelect(devAddr, (byte)pageId))
                        {
                            if (WriteData(
                                Format3HeaderBytes(
                                    regAddr, devAddr, true, 1), new List<ushort> { ushortValue }))
                            {

                            }
                            else
                            {
                                isWrtiteOk = false;
                                break;
                            }
                        }
                        else
                        {
                            isWrtiteOk = false;
                            break;
                        }
                    }

                    if (!isWrtiteOk)
                        break;
                }

                st.Stop();
                OtpProgramResult = isWrtiteOk ? "OK" : "NG";
                OtpProgramResult += " 耗时" + st.ElapsedMilliseconds / 1000f + "s";
                #endregion
            }
        }

        [Description("读取OTP值与配置文件比对")]
        public void CompareWithConfig(string devAddrDec)
        {
            CompareWithConfig(devAddrDec, true);
        }

        [Description("读取OTP值与配置文件比对(除page2-0xfd-DEVICE_INFO)")]
        public void CompareWithConfingButNoDeviceInfo(string devAddrDec)
        {
            CompareWithConfig(devAddrDec, false);
        }

        private void CompareWithConfig(string devAddrDec, bool isCompareDeviceInfo)
        {
            OtpRegToWriteFromConfig = string.Empty;
            OtpRegRead = string.Empty;
            OtpReadAndComprareWithConfigResult = string.Empty;

            byte devAddr;
            if (!byte.TryParse(devAddrDec, out devAddr))
                return;

            if (string.IsNullOrEmpty(OtpConfigFilePath))
                return;

            if (!File.Exists(OtpConfigFilePath) || !OtpConfigFilePath.EndsWith(".cfg"))
                return;

            #region 先从配置文件读取
            var configList = new Dictionary<int, Dictionary<byte, ushort>>();
            {
                lock (ConfigFileLocker)
                {
                    _otpRegMappingConfigList.Clear();
                    for (var i = 0; i < 4; i++)
                        _otpRegMappingConfigList.Add(i, new Dictionary<string, byte>());

                    {
                        var lines = File.ReadAllLines(_otpRegMappingConfigFilePath).ToList();

                        foreach (var l in lines)
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(l))
                                    continue;
                                var regAddr = Convert.ToByte(l.Split(':')[0], 16);
                                var regName = l.Split(':')[1];

                                if (regName.StartsWith("OTP_PAGE0_"))
                                    _otpRegMappingConfigList[0].Add(regName, regAddr);
                                else if (regName.StartsWith("OTP_PAGE1_"))
                                    _otpRegMappingConfigList[1].Add(regName, regAddr);
                                else if (regName.StartsWith("OTP_PAGE2_"))
                                    _otpRegMappingConfigList[2].Add(regName, regAddr);
                                else if (regName.StartsWith("OTP_PAGE3_"))
                                    _otpRegMappingConfigList[3].Add(regName, regAddr);
                            }
                            catch (Exception)
                            {
                                return;
                            }
                        }
                    }
                    {
                        var lines = File.ReadAllLines(OtpConfigFilePath).ToList();

                        for (var i = 0; i < 4; i++)
                            configList.Add(i, new Dictionary<byte, ushort>());

                        foreach (var l in lines)
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(l))
                                    continue;

                                if (!l.StartsWith("OTP_PAGE"))
                                    continue;
                                var pageIndex = int.Parse(l.Substring(8, 1));
                                var regName = l.Split('=')[0];
                                var regValue = ushort.Parse(l.Split('=')[1]);
                                var regAddr = _otpRegMappingConfigList[pageIndex][regName];

                                if (regAddr == 253 && l.StartsWith("OTP_PAGE2"))
                                {
                                    if (isCompareDeviceInfo)
                                    {
                                        configList[pageIndex].Add(regAddr, regValue);
                                    }
                                    else
                                    {
                                        configList[pageIndex].Add(regAddr, 0);
                                    }
                                }
                                else
                                {
                                    configList[pageIndex].Add(regAddr, regValue);
                                }

                            }
                            catch (Exception)
                            {
                                return;
                            }
                        }
                    }
                }
            }
            #endregion

            var dataToWriteStr = string.Empty;
            var dataToReadStr = string.Empty;

            for (var i = 0; i < 4; i++)
            {
                if (configList[i].Any())
                {
                    dataToWriteStr += string.Format("[Page{0}]", ValueHelper.GetHextStrWithOx((byte)i));

                    foreach (var key in configList[i].Keys)
                    {
                        var regAddr = key;
                        var ushortValue = configList[i][key];

                        dataToWriteStr +=
                            string.Format("{0}={1}",
                            ValueHelper.GetHextStrWithOx(regAddr),
                            ushortValue.ToString().PadLeft(5, '0'));
                    }
                }
            }

            var isReadOk = true;

            for (var i = 0; i < 4; i++)
            {
                if (configList[i].Any())
                {
                    dataToReadStr += string.Format("[Page{0}]", ValueHelper.GetHextStrWithOx((byte)i));

                    //foreach (var key in configList[i].Keys)
                    {
                        //var regAddr = key;
                        //var regValue = configList[i][key];

                        // 选PAGE
                        if (OtpPageSelect(devAddr, (byte)i))
                        {
                            var baseRegAddr = 192;

                            for (var k = 0; k < 4; k++) // C0,D0,E0,F0
                            {
                                string read1;
                                if (ReadData(Format3HeaderBytes((byte)baseRegAddr, devAddr, false, 16), 16, out read1))
                                {
                                    for (var j = 0; j < 16; j++)
                                    {
                                        var readUshort1 = (ushort)
                                            (Convert.ToByte(read1.Split(' ')[j].Substring(0, 2), 16) * 256 +
                                             Convert.ToByte(read1.Split(' ')[j].Substring(2, 2), 16));

                                        if (k == 3 && j == 13)
                                        {
                                            if (!isCompareDeviceInfo)
                                            {
                                                readUshort1 = 0;
                                            }
                                        }

                                        dataToReadStr += string.Format(
                                            "{0}={1}",
                                            ValueHelper.GetHextStrWithOx((byte)(baseRegAddr + j)),
                                            readUshort1.ToString().PadLeft(5, '0'));
                                    }

                                    baseRegAddr = baseRegAddr + 16;

                                    //var ushortValue = (ushort)
                                    //        (Convert.ToByte(read1.Split(' ')[0].Substring(0, 2), 16) * 256 +
                                    //         Convert.ToByte(read1.Split(' ')[0].Substring(2, 2), 16));

                                    //dataToReadStr +=
                                    //    string.Format("{0}={1}",
                                    //    ValueHelper.GetHextStrWithOx(regAddr),
                                    //    ushortValue.ToString().PadLeft(5, '0'));
                                }
                                else
                                {
                                    isReadOk = false;
                                    break;
                                }
                            }

                            if (!isReadOk)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            OtpRegToWriteFromConfig = dataToWriteStr;
            OtpRegRead = dataToReadStr;
            OtpReadAndComprareWithConfigResult = OtpRegRead == OtpRegToWriteFromConfig ? "OK" : "NG";
        }

        //[Description("OTP-FromConfig")]
        //public void OptFromConfig(string devAddrDec)
        //{
        //    OtpProgramResult = string.Empty;

        //    byte devAddr;
        //    if (!byte.TryParse(devAddrDec, out devAddr))
        //        return;

        //    if (string.IsNullOrEmpty(OtpConfigFilePath))
        //        return;

        //    if (!File.Exists(OtpConfigFilePath) || !OtpConfigFilePath.EndsWith(".cfg"))
        //        return;

        //    #region 读配置文件，取出参数
        //    var configList = new Dictionary<int, Dictionary<byte, ushort>>();
        //    {
        //        lock (ConfigFileLocker)
        //        {
        //            _otpRegMappingConfigList.Clear();
        //            for (var i = 0; i < 4; i++)
        //                _otpRegMappingConfigList.Add(i, new Dictionary<string, byte>());

        //            {
        //                var lines = File.ReadAllLines(_otpRegMappingConfigFilePath).ToList();

        //                foreach (var l in lines)
        //                {
        //                    try
        //                    {
        //                        if (string.IsNullOrEmpty(l))
        //                            continue;
        //                        var regAddr = Convert.ToByte(l.Split(':')[0], 16);
        //                        var regName = l.Split(':')[1];

        //                        if (regName.StartsWith("OTP_PAGE0_"))
        //                            _otpRegMappingConfigList[0].Add(regName, regAddr);
        //                        else if (regName.StartsWith("OTP_PAGE1_"))
        //                            _otpRegMappingConfigList[1].Add(regName, regAddr);
        //                        else if (regName.StartsWith("OTP_PAGE2_"))
        //                            _otpRegMappingConfigList[2].Add(regName, regAddr);
        //                        else if (regName.StartsWith("OTP_PAGE3_"))
        //                            _otpRegMappingConfigList[3].Add(regName, regAddr);
        //                    }
        //                    catch (Exception)
        //                    {
        //                        return;
        //                    }
        //                }
        //            }
        //            {
        //                var lines = File.ReadAllLines(OtpConfigFilePath).ToList();

        //                for (var i = 0; i < 4; i++)
        //                    configList.Add(i, new Dictionary<byte, ushort>());

        //                foreach (var l in lines)
        //                {
        //                    try
        //                    {
        //                        if (string.IsNullOrEmpty(l))
        //                            continue;

        //                        if (!l.StartsWith("OTP_PAGE"))
        //                            continue;
        //                        var pageIndex = int.Parse(l.Substring(8, 1));
        //                        var regName = l.Split('=')[0];
        //                        var regValue = ushort.Parse(l.Split('=')[1]);
        //                        var regAddr = _otpRegMappingConfigList[pageIndex][regName];
        //                        configList[pageIndex].Add(regAddr, regValue);
        //                    }
        //                    catch (Exception)
        //                    {
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    if (devAddr < 0x01 && devAddr > 0x1F)
        //        return;
        //    {
        //        var st = new Stopwatch();
        //        st.Start();

        //        var dataWriteSuccess = true;

        //        for (var i = 0; i < 4; i++) // page0~3
        //        {
        //            var pageIndex = (byte)i;

        //            var baseRegAddr = 192;

        //            for (var k = 0; k < 4; k++) // C0,D0,E0,F0
        //            {
        //                // 选PAGE
        //                if (OtpPageSelect(devAddr, pageIndex))
        //                {
        //                    // 先预读寄存器
        //                    var dataToWrite = new List<ushort>();
        //                    string read1;
        //                    if (ReadData(Format3HeaderBytes((byte)baseRegAddr, devAddr, false, 16), 16, out read1))
        //                    //if (true)
        //                    {
        //                        var preReadCompare = true;
        //                        for (var j = 0; j < 16; j++)
        //                        {
        //                            var readUshort1 = (ushort)
        //                                (Convert.ToByte(read1.Split(' ')[j].Substring(0, 2), 16) * 256 +
        //                                 Convert.ToByte(read1.Split(' ')[j].Substring(2, 2), 16));

        //                            dataToWrite.Add(configList[pageIndex][(byte)(baseRegAddr + j)]);

        //                            if (readUshort1 != configList[pageIndex][(byte)(baseRegAddr + j)])
        //                            {
        //                                preReadCompare = false;
        //                            }
        //                        }

        //                        // 预读和配置文件不一致，则写
        //                        if (!preReadCompare)
        //                        {
        //                            // 选PAGE
        //                            if (OtpPageSelect(devAddr, pageIndex))
        //                            {
        //                                // 写寄存器
        //                                if (WriteData(Format3HeaderBytes((byte)baseRegAddr, devAddr, true, 16),
        //                                    dataToWrite))
        //                                {
        //                                    // 读0xBF
        //                                    ReadProgStatus(devAddr, pageIndex);

        //                                    // 选PAGE
        //                                    if (OtpPageSelect(devAddr, pageIndex))
        //                                    {
        //                                        // 读寄存器
        //                                        string read2;
        //                                        if (ReadData(Format3HeaderBytes((byte)baseRegAddr, devAddr, false, 16), 16, out read2))
        //                                        {
        //                                            for (var j = 0; j < 16; j++)
        //                                            {
        //                                                //var readUshort2 = (ushort)
        //                                                //    (Convert.ToByte(read1.Split(' ')[j].Substring(0, 2), 16) * 256 +
        //                                                //     Convert.ToByte(read1.Split(' ')[j].Substring(2, 2), 16));

        //                                                //if (readUshort2 != configList[k][(byte)(baseRegAddr + j)])
        //                                                //{
        //                                                //    //dataWriteSuccess = false;
        //                                                //    //break;
        //                                                //}
        //                                            }

        //                                            if (!dataWriteSuccess)
        //                                                break;
        //                                        }
        //                                        else
        //                                        {
        //                                            dataWriteSuccess = false;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        dataWriteSuccess = false;
        //                                        break;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    dataWriteSuccess = false;
        //                                    break;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                dataWriteSuccess = false;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        dataWriteSuccess = false;
        //                        break;
        //                    }
        //                }
        //                else
        //                {
        //                    dataWriteSuccess = false;
        //                }

        //                if (!dataWriteSuccess)
        //                    break;

        //                baseRegAddr = baseRegAddr + 16;
        //            }

        //            if (!dataWriteSuccess)
        //                break;
        //        }

        //        st.Stop();
        //        OtpProgramResult = dataWriteSuccess ? "OK" : "NG";
        //        OtpProgramResult += " 耗时" + st.ElapsedMilliseconds / 1000f + "s";
        //    }
        //}

        [Description("OTP-FromConfig-OneByOneWord")]
        public void OptFromConfigOneByOneWord(string devAddrDec)
        {
            OtpProgramResult = string.Empty;

            byte devAddr;
            if (!byte.TryParse(devAddrDec, out devAddr))
                return;

            if (string.IsNullOrEmpty(OtpConfigFilePath))
                return;

            if (!File.Exists(OtpConfigFilePath) || !OtpConfigFilePath.EndsWith(".cfg"))
                return;

            #region 读配置文件，取出参数
            var configList = new Dictionary<int, Dictionary<byte, ushort>>();
            {
                lock (ConfigFileLocker)
                {
                    _otpRegMappingConfigList.Clear();
                    for (var i = 0; i < 4; i++)
                        _otpRegMappingConfigList.Add(i, new Dictionary<string, byte>());

                    {
                        var lines = File.ReadAllLines(_otpRegMappingConfigFilePath).ToList();

                        foreach (var l in lines)
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(l))
                                    continue;
                                var regAddr = Convert.ToByte(l.Split(':')[0], 16);
                                var regName = l.Split(':')[1];

                                if (regName.StartsWith("OTP_PAGE0_"))
                                    _otpRegMappingConfigList[0].Add(regName, regAddr);
                                else if (regName.StartsWith("OTP_PAGE1_"))
                                    _otpRegMappingConfigList[1].Add(regName, regAddr);
                                else if (regName.StartsWith("OTP_PAGE2_"))
                                    _otpRegMappingConfigList[2].Add(regName, regAddr);
                                else if (regName.StartsWith("OTP_PAGE3_"))
                                    _otpRegMappingConfigList[3].Add(regName, regAddr);
                            }
                            catch (Exception)
                            {
                                return;
                            }
                        }
                    }
                    {
                        var lines = File.ReadAllLines(OtpConfigFilePath).ToList();

                        for (var i = 0; i < 4; i++)
                            configList.Add(i, new Dictionary<byte, ushort>());

                        foreach (var l in lines)
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(l))
                                    continue;

                                if (!l.StartsWith("OTP_PAGE"))
                                    continue;
                                var pageIndex = int.Parse(l.Substring(8, 1));
                                var regName = l.Split('=')[0];
                                var regValue = ushort.Parse(l.Split('=')[1]);
                                var regAddr = _otpRegMappingConfigList[pageIndex][regName];
                                configList[pageIndex].Add(regAddr, regValue);
                            }
                            catch (Exception)
                            {
                                return;
                            }
                        }
                    }
                }
            }
            #endregion

            #region 写入和比较
            if (devAddr < 0x01 && devAddr > 0x1F)
                return;
            {
                var st = new Stopwatch();
                st.Start();

                var dataToWriteStr = string.Empty;
                var dataToReadStr = string.Empty;

                var isOk = true;

                for (var i = 0; i < 4; i++)
                {
                    if (configList[i].Any())
                    {
                        dataToWriteStr += string.Format("[Page{0}]\r\n", ValueHelper.GetHextStrWithOx((byte)i));

                        foreach (var key in configList[i].Keys)
                        {
                            var regAddr = key;
                            var ushortValue = configList[i][key];

                            dataToWriteStr +=
                                string.Format("{0}={1}\r\n",
                                ValueHelper.GetHextStrWithOx(regAddr),
                                ushortValue.ToString().PadLeft(5, '0'));

                            // 选PAGE
                            if (OtpPageSelect(devAddr, (byte)i))
                            {
                                WriteData(
                                    Format3HeaderBytes(
                                        regAddr, devAddr, true, 1), new List<ushort> { ushortValue });
                            }
                            else
                            {
                                isOk = false;
                                break;
                            }
                        }

                        if (!isOk)
                        {
                            break;
                        }
                    }
                }

                if (isOk)
                {
                    for (var i = 0; i < 4; i++)
                    {
                        if (configList[i].Any())
                        {
                            dataToReadStr += string.Format("[Page{0}]\r\n", ValueHelper.GetHextStrWithOx((byte)i));

                            foreach (var key in configList[i].Keys)
                            {
                                var regAddr = key;
                                var regValue = configList[i][key];

                                // 选PAGE
                                if (OtpPageSelect(devAddr, (byte)i))
                                {
                                    string read1;
                                    if (ReadData(Format3HeaderBytes(regAddr, devAddr, false, 1), 1, out read1))
                                    {
                                        var ushortValue = (ushort)
                                                (Convert.ToByte(read1.Split(' ')[0].Substring(0, 2), 16) * 256 +
                                                 Convert.ToByte(read1.Split(' ')[0].Substring(2, 2), 16));

                                        dataToReadStr +=
                                            string.Format("{0}={1}\r\n",
                                            ValueHelper.GetHextStrWithOx(regAddr),
                                            ushortValue.ToString().PadLeft(5, '0'));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    dataToReadStr = "NG";
                }

                st.Stop();
                OtpProgramResult = dataToReadStr == dataToWriteStr ? "OK" : "NG";
                OtpProgramResult += " 耗时" + st.ElapsedMilliseconds / 1000f + "s";
            }
            #endregion
        }

        [Description("遍历读取StandaloneAreaPage")]
        public void ForeachReadStandaloneAreaPage(string devAddrDec)
        {
            var result = string.Empty;

            byte devAddr;
            if (!byte.TryParse(devAddrDec, out devAddr))
                return;

            if (devAddr >= 0x01 || devAddr <= 0x1F)
            {
                for (var i = 0; i < 4; i++)
                {
                    result += string.Format("[Page{0}]\r\n", ValueHelper.GetHextStrWithOx((byte)i));

                    var baseRegAddr = 192;

                    for (var k = 0; k < 4; k++)
                    {
                        // 选PAGE
                        if (OtpPageSelect(devAddr, (byte)i))
                        {
                            // 读寄存器
                            var dataToWrite = new List<ushort>();
                            string read1;
                            if (ReadData(Format3HeaderBytes((byte)baseRegAddr, devAddr, false, 16), 16, out read1))
                            {
                                for (var j = 0; j < 16; j++)
                                {
                                    var ushortValue = (ushort)
                                        (Convert.ToByte(read1.Split(' ')[j].Substring(0, 2), 16) * 256 +
                                         Convert.ToByte(read1.Split(' ')[j].Substring(2, 2), 16));

                                    dataToWrite.Add(ushortValue);

                                    var valueHex = string.Format("0x{0}{1}",
                                        ValueHelper.GetHextStr(Convert.ToByte(read1.Split(' ')[j].Substring(0, 2), 16)),
                                        ValueHelper.GetHextStr(Convert.ToByte(read1.Split(' ')[j].Substring(2, 2), 16)));
                                    var regAddr = ValueHelper.GetHextStrWithOx((byte)(baseRegAddr + j));
                                    result += string.Format("{0}={1},{2}\r\n", regAddr, ushortValue.ToString().PadLeft(5, '0'), valueHex);
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }

                        baseRegAddr = baseRegAddr + 16;
                    }
                }
            }

            Debug.WriteLine(result);
        }

        [Description("读取配置文件")]
        public void ReadConfig()
        {
            if (!File.Exists(OtpConfigFilePath) || !OtpConfigFilePath.EndsWith(".cfg"))
                return;

            #region 先从配置文件读取
            var configList = new Dictionary<int, Dictionary<byte, ushort>>();
            {
                lock (ConfigFileLocker)
                {
                    _otpRegMappingConfigList.Clear();
                    for (var i = 0; i < 4; i++)
                        _otpRegMappingConfigList.Add(i, new Dictionary<string, byte>());

                    {
                        var lines = File.ReadAllLines(_otpRegMappingConfigFilePath).ToList();

                        foreach (var l in lines)
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(l))
                                    continue;
                                var regAddr = Convert.ToByte(l.Split(':')[0], 16);
                                var regName = l.Split(':')[1];

                                if (regName.StartsWith("OTP_PAGE0_"))
                                    _otpRegMappingConfigList[0].Add(regName, regAddr);
                                else if (regName.StartsWith("OTP_PAGE1_"))
                                    _otpRegMappingConfigList[1].Add(regName, regAddr);
                                else if (regName.StartsWith("OTP_PAGE2_"))
                                    _otpRegMappingConfigList[2].Add(regName, regAddr);
                                else if (regName.StartsWith("OTP_PAGE3_"))
                                    _otpRegMappingConfigList[3].Add(regName, regAddr);
                            }
                            catch (Exception)
                            {
                                return;
                            }
                        }
                    }
                    {
                        var lines = File.ReadAllLines(OtpConfigFilePath).ToList();

                        for (var i = 0; i < 4; i++)
                            configList.Add(i, new Dictionary<byte, ushort>());

                        foreach (var l in lines)
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(l))
                                    continue;

                                if (!l.StartsWith("OTP_PAGE"))
                                    continue;
                                var pageIndex = int.Parse(l.Substring(8, 1));
                                var regName = l.Split('=')[0];
                                var regValue = ushort.Parse(l.Split('=')[1]);
                                var regAddr = _otpRegMappingConfigList[pageIndex][regName];
                                configList[pageIndex].Add(regAddr, regValue);
                            }
                            catch (Exception)
                            {
                                return;
                            }
                        }
                    }
                }
            }
            #endregion

            var dataToWriteStr = string.Empty;

            for (var i = 0; i < 4; i++)
            {
                if (configList[i].Any())
                {
                    dataToWriteStr += string.Format("[Page{0}]\r\n", ValueHelper.GetHextStrWithOx((byte)i));

                    foreach (var key in configList[i].Keys)
                    {
                        var regAddr = key;
                        var ushortValue = configList[i][key];

                        var ushortValueBytes = BitConverter.GetBytes(ushortValue).Reverse().ToList();
                        var read1 = ValueHelper.GetHextStr(ushortValueBytes[0]) +
                                    ValueHelper.GetHextStr(ushortValueBytes[1]) + " ";

                        var valueHex = string.Format("0x{0}{1}",
                                       ValueHelper.GetHextStr(Convert.ToByte(read1.Substring(0, 2), 16)),
                                       ValueHelper.GetHextStr(Convert.ToByte(read1.Substring(2, 2), 16)));

                        dataToWriteStr +=
                            string.Format("{0}={1},{2}\r\n",
                            ValueHelper.GetHextStrWithOx(regAddr),
                            ushortValue.ToString().PadLeft(5, '0'), valueHex);
                    }
                }
            }

            Console.WriteLine(dataToWriteStr);
        }

        //[Description("测试写单个")]
        //public void WriteSingle(string devIdDec, string pageIdDec, string regAddrDec, string regValueDec)
        //{
        //    OtpPageSelect(byte.Parse(devIdDec), byte.Parse(pageIdDec));
        //    WriteData(
        //        Format3HeaderBytes(
        //            byte.Parse(regAddrDec), byte.Parse(devIdDec), true, 1), new List<ushort> { ushort.Parse(regValueDec) });
        //}

        [Description("R,数据解析")]
        public string DataDataAnalysis = string.Empty;

        [Description("数据解析")]
        public void DataAnalysis(string value)
        {
            // 00 00 00 00 00 00 00 00 00 00 12 00 00 00 15 05 54 51 01 00
            DataDataAnalysis = string.Empty;

            var rcvData = value.Replace(" ", "");
            if (rcvData.Length % 2 != 0)
                return;

            var r = string.Empty;
            try
            {
                var tempDataBits = new List<char>();
                for (var k = 0; k < rcvData.Length; k = k + 2)
                {
                    tempDataBits.AddRange(Convert.ToString(
                        Convert.ToByte(rcvData.Substring(k, 2), 16), 2)
                        .PadLeft(8, '0').ToCharArray().Reverse());
                }

                int readCount;
                if (rcvData.Length * 8 % 10 == 0)
                    readCount = rcvData.Length / 2 * 8 / 10;
                else
                    readCount = rcvData.Length / 2 * 8 / 10 + 1;
                //rcvData.Length * 8 / 10;

                for (var j = 0; j < readCount * 10; j = j + 10)
                {
                    var s = string.Format("000000{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                        tempDataBits[j + 9],
                        tempDataBits[j + 8],
                        tempDataBits[j + 7],
                        tempDataBits[j + 6],
                        tempDataBits[j + 5],
                        tempDataBits[j + 4],
                        tempDataBits[j + 3],
                        tempDataBits[j + 2],
                        tempDataBits[j + 1],
                        tempDataBits[j + 0]);

                    var ushortValue = BitConverter.GetBytes(Convert.ToUInt16(s, 2)).Reverse().ToList();
                    r += ushortValue[0] * 256 + ushortValue[1] + " ";
                }
            }
            catch (Exception)
            {
                // ignored
            }

            DataDataAnalysis = r;
        }

        /// <summary>
        /// OTP地址页面选择-可选0到3页
        /// </summary>
        /// <param name="devId"></param>
        /// <param name="pageId"></param>
        private bool OtpPageSelect(byte devId, ushort pageId)
        {
            if (MySerialPort == null)
                return false;

            if (!WriteData(Format3HeaderBytes(0x70, devId, true, 1), new[] { pageId }))
            {
                //if (!WriteData(Format3HeaderBytes(0x70, devId, true, 1), new[] { pageId }))
                {
                    return false;
                }
            }

            Thread.Sleep(50);
            return true;

            //return MySerialPort != null &&
            //    WriteData(Format3HeaderBytes(0x70, devId, true, 1), new[] { pageId });
        }

        /// <summary>
        /// 读取ProgStatus
        /// </summary>
        /// <returns></returns>
        [Description("读取ProgStatus")]
        public bool ReadProgStatus(byte devId, byte pageIndex)
        {
            ReadProgStatusResult = "NG";

            if (MySerialPort == null)
                return false;

            while (true)
            {
                string progStatus;
                if (ReadData(Format3HeaderBytes(0xBF, devId, false, 1), 1, out progStatus))
                {
                    var bits =
                       Convert.ToString(Convert.ToByte(progStatus.Substring(2, 2), 16), 2)
                           .PadLeft(8, '0')
                           .Reverse()
                           .ToList();
                    var busy = bits[0].ToString() == 1.ToString();
                    var error = bits[1].ToString() == 1.ToString();
                    var baseAddrSel = Convert.ToByte(string.Format("000000{0}{1}", bits[3], bits[2]), 2); // 0x05 = '0000 0101'

                    if (pageIndex == baseAddrSel)
                    {
                        if (!busy)
                        {
                            if (!error)
                            {
                                ReadProgStatusResult = "OK";
                                return true;
                            }

                            return false;
                        }

                        //// 先判断当前选择页是否和读取相同
                        //if (baseAddrSel == 0x01)
                        //{
                        //    // 再判断是否Failed和Busy
                        //    if (!busy)
                        //        return !error;

                        //    if (error)
                        //        return false;
                        //}
                        //else
                        //    return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
        }

        private static byte[] Format3HeaderBytes(
            byte memoryAddr, byte devAddr, bool isWrite, int numWords)
        {
            var memoryAddress = memoryAddr;
            var deviceAddress = devAddr;
            //var writeRead = isWrite ? (byte)0x01 : (byte)0x00;
            var numWordsPlus1 = (byte)(numWords - 1);

            //var memoryAddressBits = Convert.ToString(memoryAddress, 2).PadLeft(8, '0');
            var deviceAddressBits = Convert.ToString(deviceAddress, 2).PadLeft(8, '0');
            var writeReadBit = isWrite ? "1" : "0";
            var numWorksPlus1Bits = Convert.ToString(numWordsPlus1, 2).PadLeft(8, '0');

            var data0 = memoryAddress;
            var data1 =
                Convert.ToByte(
                    string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", numWorksPlus1Bits[6], numWorksPlus1Bits[7],
                        writeReadBit, deviceAddressBits[3], deviceAddressBits[4], deviceAddressBits[5],
                        deviceAddressBits[6], deviceAddressBits[7]), 2);
            var data2 = Convert.ToByte(string.Format("000000{0}{1}", numWorksPlus1Bits[4], numWorksPlus1Bits[5]), 2);

            var crc6 = Crc6(data0, data1, data2);

            return new[] { data0, data1, crc6 };
        }

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="writeBytesHeader"></param>
        /// <param name="writeWordsContent"></param>
        /// <returns></returns>
        private bool WriteData(
            IEnumerable<byte> writeBytesHeader, IReadOnlyCollection<ushort> writeWordsContent)
        {
            var func = new Func<bool>(() =>
            {
                if (MySerialPort == null)
                    return false;
                if (_isRead || _isWrite1 || _isWrite2)
                    return false;
                if (writeWordsContent == null || writeWordsContent.Count == 0)
                    return false;

                if (writeWordsContent.Any(w => w > 1023))
                    return false;

                _isWrite1 = true;
                var sendBytes = new List<byte>();
                sendBytes.AddRange(writeBytesHeader);
                foreach (var t in sendBytes)
                    _write1Cmd += ValueHelper.GetHextStr(t);
                _write1Cmd = "0055" + _write1Cmd;
                MySerialPort.SendBreakSyncCmd(sendBytes.ToArray());

                if (_waitWrite1Handle.WaitOne(200))
                {
                    sendBytes.Clear();

                    var len = writeWordsContent.Count * 10 % 8 == 0
                        ? writeWordsContent.Count * 10 / 8
                        : writeWordsContent.Count * 10 / 8 + 1;
                    var bits = new List<char>();

                    foreach (
                        var a in
                            writeWordsContent.Select(
                                w => Convert.ToString(w, 2).PadLeft(16, '0').ToCharArray().Reverse().ToList()))
                        bits.AddRange(a.GetRange(0, 10));

                    var rest0 = len * 8 - bits.Count;
                    for (var i = 0; i < rest0; i++)
                        bits.Add('0');

                    var bytes = new List<byte>();
                    for (var i = 0; i < bits.Count; i = i + 8)
                        bytes.Add(
                            Convert.ToByte(
                                string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", bits[i + 7], bits[i + 6], bits[i + 5], bits[i + 4],
                                    bits[i + 3], bits[i + 2], bits[i + 1], bits[i + 0]), 2));

                    _isWrite2 = true;
                    sendBytes.AddRange(bytes);
                    sendBytes.Add(Crc8(bytes));
                    foreach (var t in sendBytes)
                        _write2Cmd += ValueHelper.GetHextStr(t);
                    MySerialPort.SendCommand(sendBytes.ToArray());

                    if (_waitWrite2Handle.WaitOne(200))
                    {
                        Thread.Sleep(2);
                        return true;
                    }
                }

                _write1Cmd = string.Empty;
                _write2Cmd = string.Empty;
                _isWrite1 = false;
                _isWrite2 = false;
                return false;
            });

            if (func())
                return true;
            Thread.Sleep(50);

            if (func())
                return true;
            Thread.Sleep(50);

            if (func())
                return true;
            Thread.Sleep(50);

            return func();
        }

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="readBytesHeader"></param>
        /// <param name="numWords"></param>
        /// <param name="recv"></param>
        /// <returns></returns>
        private bool ReadData(
            byte[] readBytesHeader, int numWords, out string recv)
        {
            var result = string.Empty;

            var func = new Func<bool>(() =>
            {
                //recv = string.Empty;

                if (readBytesHeader == null)
                    return false;

                if (MySerialPort == null)
                    return false;

                if (_isRead || _isWrite1 || _isWrite2)
                    return false;

                _isRead = true;
                var sendBytes = new List<byte>();
                sendBytes.AddRange(readBytesHeader);

                _readCount = numWords;
                foreach (var t in sendBytes)
                    _readCmd += ValueHelper.GetHextStr(t);

                _readCmd = "0055" + _readCmd;
                MySerialPort.SendBreakSyncCmd(sendBytes.ToArray());
                //return false;

                if (!_waitReadHandle.WaitOne(200))
                {
                    _readCmd = string.Empty;
                    _isRead = false;
                    return false;
                }

                Thread.Sleep(5);
                result = _readRecv;
                //recv = _readRecv;
                return true;
            });

            if (func.Invoke())
            {
                recv = result;
                return true;
            }

            //Thread.Sleep(5);

            //if (func.Invoke())
            //{
            //    recv = result;
            //    return true;
            //}

            Thread.Sleep(50);

            if (func.Invoke())
            {
                Thread.Sleep(15);
                recv = result;
                return true;
            }

            Thread.Sleep(50);

            if (func.Invoke())
            {
                Thread.Sleep(15);
                recv = result;
                return true;
            }

            Thread.Sleep(50);

            if (func.Invoke())
            {
                Thread.Sleep(15);
                recv = result;
                return true;
            }

            recv = string.Empty;
            return false;
        }

        /// <summary>
        /// The 6 bit CRC is only used for the transmission of 3 byte header frame.
        /// </summary>
        /// <param name="data0"></param>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        private static byte Crc6(byte data0, byte data1, byte data2)
        {
            byte crc = 0x3F;
            var revPoly = CalcRevPoly(0x2C);

            crc = CrcCalc(crc, revPoly, data0);
            crc = CrcCalc(crc, revPoly, data1);
            crc = CrcCalc(crc, revPoly, data2 & 0x03);

            var returnCrc = (byte)((crc << 2) | (data2 & 0x03));
            return returnCrc;
        }

        /// <summary>
        /// The 8 bit CRC is used for transmition of 4 byte header frame and for all data word transmissions.
        /// The polynom depends on the length of the transmission.
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        private static byte Crc8(IReadOnlyCollection<byte> datas)
        {
            var revPoly = CalcRevPoly(datas.Count < 11 ? 0x97 : 0xA6);
            return
                datas.Aggregate<byte, byte>(0xFF, (current, t) => CrcCalc(current, revPoly, t));
        }

        /// <summary>
        /// Build reverse polynom
        /// </summary>
        /// <param name="poly"></param>
        /// <returns></returns>
        private static byte CalcRevPoly(int poly)
        {
            byte recvPoly = 1;
            while (poly > 1)
            {
                recvPoly = (byte)((recvPoly << 1) | (poly & 1));
                poly >>= 1;
            }

            return recvPoly;
        }

        /// <summary>
        /// Calculate CRC for a data byte
        /// </summary>
        /// <returns></returns>
        private static byte CrcCalc(byte crc, byte revPoly, int byteValue)
        {
            var returnCrc = crc;

            for (var i = 0; i < 8; i++)
            {
                if (((returnCrc ^ byteValue) & 1) != 0)
                    returnCrc = (byte)((returnCrc >> 1) ^ revPoly);
                else
                    returnCrc = (byte)(returnCrc >> 1);
                byteValue >>= 1;
            }

            return returnCrc;
        }

        #region Bus Configuration Area 总线配置区 LED参数读取值

        private void SetRegValue(
            string startName, int index, string value)
        {
            var fieldName = string.Format("{0}{1}", startName, index);

            var field = GetType().GetField(fieldName);
            if (field != null)
                field.SetValue(this, value);
        }

        private void ClearReadResult(string startName)
        {
            for (var i = 0; i < 16; i++)
            {
                var fieldName = string.Format("{0}{1}", startName, i);

                var field = GetType().GetField(fieldName);
                if (field != null)
                    field.SetValue(this, string.Empty);
            }
        }

        [Description("R,总线模式下的LED0的占空比")]
        public string BusPules0;
        [Description("R,总线模式下的LED1的占空比")]
        public string BusPules1;
        [Description("R,总线模式下的LED2的占空比")]
        public string BusPules2;
        [Description("R,总线模式下的LED3的占空比")]
        public string BusPules3;
        [Description("R,总线模式下的LED4的占空比")]
        public string BusPules4;
        [Description("R,总线模式下的LED5的占空比")]
        public string BusPules5;
        [Description("R,总线模式下的LED6的占空比")]
        public string BusPules6;
        [Description("R,总线模式下的LED7的占空比")]
        public string BusPules7;
        [Description("R,总线模式下的LED8的占空比")]
        public string BusPules8;
        [Description("R,总线模式下的LED9的占空比")]
        public string BusPules9;
        [Description("R,总线模式下的LED10的占空比")]
        public string BusPules10;
        [Description("R,总线模式下的LED11的占空比")]
        public string BusPules11;
        [Description("R,总线模式下的LED12的占空比")]
        public string BusPules12;
        [Description("R,总线模式下的LED13的占空比")]
        public string BusPules13;
        [Description("R,总线模式下的LED14的占空比")]
        public string BusPules14;
        [Description("R,总线模式下的LED15的占空比")]
        public string BusPules15;

        [Description("R,总线模式下的LED0的电流")]
        public string BusCurrent0;
        [Description("R,总线模式下的LED1的电流")]
        public string BusCurrent1;
        [Description("R,总线模式下的LED2的电流")]
        public string BusCurrent2;
        [Description("R,总线模式下的LED3的电流")]
        public string BusCurrent3;
        [Description("R,总线模式下的LED4的电流")]
        public string BusCurrent4;
        [Description("R,总线模式下的LED5的电流")]
        public string BusCurrent5;
        [Description("R,总线模式下的LED6的电流")]
        public string BusCurrent6;
        [Description("R,总线模式下的LED7的电流")]
        public string BusCurrent7;
        [Description("R,总线模式下的LED8的电流")]
        public string BusCurrent8;
        [Description("R,总线模式下的LED9的电流")]
        public string BusCurrent9;
        [Description("R,总线模式下的LED10的电流")]
        public string BusCurrent10;
        [Description("R,总线模式下的LED11的电流")]
        public string BusCurrent11;
        [Description("R,总线模式下的LED12的电流")]
        public string BusCurrent12;
        [Description("R,总线模式下的LED13的电流")]
        public string BusCurrent13;
        [Description("R,总线模式下的LED14的电流")]
        public string BusCurrent14;
        [Description("R,总线模式下的LED15的电流")]
        public string BusCurrent15;

        [Description("R,LED通道0使能")]
        public string LedEnable0;
        [Description("R,LED通道1使能")]
        public string LedEnable1;
        [Description("R,LED通道2使能")]
        public string LedEnable2;
        [Description("R,LED通道3使能")]
        public string LedEnable3;
        [Description("R,LED通道4使能")]
        public string LedEnable4;
        [Description("R,LED通道5使能")]
        public string LedEnable5;
        [Description("R,LED通道6使能")]
        public string LedEnable6;
        [Description("R,LED通道7使能")]
        public string LedEnable7;
        [Description("R,LED通道8使能")]
        public string LedEnable8;
        [Description("R,LED通道9使能")]
        public string LedEnable9;
        [Description("R,LED通道10使能")]
        public string LedEnable10;
        [Description("R,LED通道11使能")]
        public string LedEnable11;
        [Description("R,LED通道12使能")]
        public string LedEnable12;
        [Description("R,LED通道13使能")]
        public string LedEnable13;
        [Description("R,LED通道14使能")]
        public string LedEnable14;
        [Description("R,LED通道15使能")]
        public string LedEnable15;

        [Description("R,通道0的LED电压值")]
        public string ResultVled0;
        [Description("R,通道1的LED电压值")]
        public string ResultVled1;
        [Description("R,通道2的LED电压值")]
        public string ResultVled2;
        [Description("R,通道3的LED电压值")]
        public string ResultVled3;
        [Description("R,通道4的LED电压值")]
        public string ResultVled4;
        [Description("R,通道5的LED电压值")]
        public string ResultVled5;
        [Description("R,通道6的LED电压值")]
        public string ResultVled6;
        [Description("R,通道7的LED电压值")]
        public string ResultVled7;
        [Description("R,通道8的LED电压值")]
        public string ResultVled8;
        [Description("R,通道9的LED电压值")]
        public string ResultVled9;
        [Description("R,通道10的LED电压值")]
        public string ResultVled10;
        [Description("R,通道11的LED电压值")]
        public string ResultVled11;
        [Description("R,通道12的LED电压值")]
        public string ResultVled12;
        [Description("R,通道13的LED电压值")]
        public string ResultVled13;
        [Description("R,通道14的LED电压值")]
        public string ResultVled14;
        [Description("R,通道15的LED电压值")]
        public string ResultVled15;

        [Description("R,通道0的供电电压与LED电压差值")]
        public string ResultVdif0;
        [Description("R,通道1的供电电压与LED电压差值")]
        public string ResultVdif1;
        [Description("R,通道2的供电电压与LED电压差值")]
        public string ResultVdif2;
        [Description("R,通道3的供电电压与LED电压差值")]
        public string ResultVdif3;
        [Description("R,通道4的供电电压与LED电压差值")]
        public string ResultVdif4;
        [Description("R,通道5的供电电压与LED电压差值")]
        public string ResultVdif5;
        [Description("R,通道6的供电电压与LED电压差值")]
        public string ResultVdif6;
        [Description("R,通道7的供电电压与LED电压差值")]
        public string ResultVdif7;
        [Description("R,通道8的供电电压与LED电压差值")]
        public string ResultVdif8;
        [Description("R,通道9的供电电压与LED电压差值")]
        public string ResultVdif9;
        [Description("R,通道10的供电电压与LED电压差值")]
        public string ResultVdif10;
        [Description("R,通道11的供电电压与LED电压差值")]
        public string ResultVdif11;
        [Description("R,通道12的供电电压与LED电压差值")]
        public string ResultVdif12;
        [Description("R,通道13的供电电压与LED电压差值")]
        public string ResultVdif13;
        [Description("R,通道14的供电电压与LED电压差值")]
        public string ResultVdif14;
        [Description("R,通道15的供电电压与LED电压差值")]
        public string ResultVdif15;

        [Description("R,通道0的LED电流值")]
        public string ResultIled0;
        [Description("R,通道1的LED电流值")]
        public string ResultIled1;
        [Description("R,通道2的LED电流值")]
        public string ResultIled2;
        [Description("R,通道3的LED电流值")]
        public string ResultIled3;
        [Description("R,通道4的LED电流值")]
        public string ResultIled4;
        [Description("R,通道5的LED电流值")]
        public string ResultIled5;
        [Description("R,通道6的LED电流值")]
        public string ResultIled6;
        [Description("R,通道7的LED电流值")]
        public string ResultIled7;
        [Description("R,通道8的LED电流值")]
        public string ResultIled8;
        [Description("R,通道9的LED电流值")]
        public string ResultIled9;
        [Description("R,通道10的LED电流值")]
        public string ResultIled10;
        [Description("R,通道11的LED电流值")]
        public string ResultIled11;
        [Description("R,通道12的LED电流值")]
        public string ResultIled12;
        [Description("R,通道13的LED电流值")]
        public string ResultIled13;
        [Description("R,通道14的LED电流值")]
        public string ResultIled14;
        [Description("R,通道15的LED电流值")]
        public string ResultIled15;

        [Description("R,LED通道0开路状态")]
        public string LedOpen0;
        [Description("R,LED通道1开路状态")]
        public string LedOpen1;
        [Description("R,LED通道2开路状态")]
        public string LedOpen2;
        [Description("R,LED通道3开路状态")]
        public string LedOpen3;
        [Description("R,LED通道4开路状态")]
        public string LedOpen4;
        [Description("R,LED通道5开路状态")]
        public string LedOpen5;
        [Description("R,LED通道6开路状态")]
        public string LedOpen6;
        [Description("R,LED通道7开路状态")]
        public string LedOpen7;
        [Description("R,LED通道8开路状态")]
        public string LedOpen8;
        [Description("R,LED通道9开路状态")]
        public string LedOpen9;
        [Description("R,LED通道10开路状态")]
        public string LedOpen10;
        [Description("R,LED通道11开路状态")]
        public string LedOpen11;
        [Description("R,LED通道12开路状态")]
        public string LedOpen12;
        [Description("R,LED通道13开路状态")]
        public string LedOpen13;
        [Description("R,LED通道14开路状态")]
        public string LedOpen14;
        [Description("R,LED通道15开路状态")]
        public string LedOpen15;

        [Description("R,LED通道0短路状态")]
        public string LedShort0;
        [Description("R,LED通道1短路状态")]
        public string LedShort1;
        [Description("R,LED通道2短路状态")]
        public string LedShort2;
        [Description("R,LED通道3短路状态")]
        public string LedShort3;
        [Description("R,LED通道4短路状态")]
        public string LedShort4;
        [Description("R,LED通道5短路状态")]
        public string LedShort5;
        [Description("R,LED通道6短路状态")]
        public string LedShort6;
        [Description("R,LED通道7短路状态")]
        public string LedShort7;
        [Description("R,LED通道8短路状态")]
        public string LedShort8;
        [Description("R,LED通道9短路状态")]
        public string LedShort9;
        [Description("R,LED通道10短路状态")]
        public string LedShort10;
        [Description("R,LED通道11短路状态")]
        public string LedShort11;
        [Description("R,LED通道12短路状态")]
        public string LedShort12;
        [Description("R,LED通道13短路状态")]
        public string LedShort13;
        [Description("R,LED通道14短路状态")]
        public string LedShort14;
        [Description("R,LED通道15短路状态")]
        public string LedShort15;

        #endregion
    }
}
