using CommonUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class InfineonTld7002 : ControllerBase
    {
        public MySerialPort MySerialPort;

        [Description("R,读取OPT状态-OTP时0x03为OK-仿真时0x01为OK")]
        public string OtpStatus = string.Empty;

        [Description("R,读取OPT与写入配置比对-一致为OK")]
        public string CompareReadedOtp = string.Empty;

        [Description("R/W,OtpSleepTime1")]
        public string OtpSleepTime1 = 1.ToString();
        [Description("R/W,OtpSleepTime2")]
        public string OtpSleepTime2 = 1.ToString();
        [Description("R/W,是否打印debug日志")]
        public string IsPrintDebugLog = 1.ToString();

        [Description("R,OTP_PWM_DC_GPIN0")]
        public string OtpPwmDcGpin0 = string.Empty;

        [Description("R,读OTP的前32Words-起始地址0x83")]
        public string TheFirst32Words = string.Empty;
        [Description("R,读OTP的后8Word-起始地址0xA3")]
        public string TheLast8Words = string.Empty;

        //[Description("R,读寄存器结果")]
        //public string ReadRegResult = string.Empty;

        //[Description("R/W,OTP配置文件-ocfg格式")]
        // public string OtpConfigFilePath = @"C:\Projs\2022\芯片相关\Infineon-TLD7002\VW326-LOG\测试烧录文件\ADDRESS_1_Customer_GPIN_SEEYAO_B_TURN1_ad6.ocfg";

        //[Description("R/W,OTP配置文件-ocfg格式")]
        //public string OtpConfigFilePath = @"C:\Projs\2022\芯片相关\Infineon-TLD7002\VW326-LOG\20221216\SLAVE 4.ocfg";

        [Description("R/W,OTP配置文件-ocfg格式")]
        public string OtpConfigFilePath = string.Empty;
        //@"C:\Projs\2022\芯片相关\Infineon-TLD7002\SGM358-2\358-2 T0前地址B灯TAIL1TAIL3TURN1 20221205\358-2 T0前地址B灯TAIL1TAIL3TURN1 20221205\地址4(TAIL1 AB档位)1206.ocfg";
        //@"E:\Projects-2022\点灯&芯片相关\E001前后灯-TLD7002\软件程序\1-0.ocfg";
        //@"C:\Users\B765\Desktop\2-0-20230128.ocfg";

        public InfineonTld7002(string name)
            : base(name)
        {
            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;

            _mrcList.Add(0, "01");
            _mrcList.Add(1, "10");
            _mrcList.Add(2, "11");
            _mrcList.Add(3, "00");

            _dlcCodeList.Add(1, "001");
            _dlcCodeList.Add(2, "010");
            _dlcCodeList.Add(4, "011");
            _dlcCodeList.Add(8, "100");
            _dlcCodeList.Add(12, "101");
            _dlcCodeList.Add(16, "110");
            _dlcCodeList.Add(32, "111");

            _ledControlThread = new Thread(LedControlWork) { IsBackground = true };
            _ledControlThread.Start();
        }

        ~InfineonTld7002()
        {
            Dispose();
        }

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (MySerialPort == null || MySerialPort.Name != name)
                return;
            var str = ValueHelper.GetHextStr(datas);
            //datas.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            Console.WriteLine(str);

            if (IsPrintDebugLog == 1.ToString())
                Console.WriteLine(str);

            if (_isRead)
            {
                try
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = str.Replace(" ", "");

                        if (_isRead && !string.IsNullOrEmpty(_readCmd) && _readCount > 0 && _readCount <= 32)
                        {
                            var len = _readCount * 2 + 3 + 4;
                            if (datas.Length == len && str.StartsWith(_readCmd))
                            {
                                var isCrc3Correct = CheckSlaveResCrc3(datas[datas.Length - 2], datas[datas.Length - 1]);
                                if (isCrc3Correct)
                                {
                                    var tempData = string.Empty;
                                    var baseIndex = 4;
                                    for (var i = 0; i < _readCount * 2; i++)
                                    {
                                        tempData += ValueHelper.GetHextStr(datas[baseIndex]);
                                        baseIndex++;
                                    }

                                    baseIndex = 4;
                                    var toCrc8Data = new List<byte> { datas[3] };
                                    for (var i = 0; i < _readCount * 2; i++)
                                    {
                                        toCrc8Data.Add(datas[baseIndex]);
                                        baseIndex++;
                                    }
                                    var crc8 = GetCrc8(toCrc8Data.ToArray(), (byte)toCrc8Data.Count);
                                    if (crc8 == datas[datas.Length - 3])
                                    {
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
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public void ResetMrcCount()
        {
            lock (_lockSend)
                _mrcCount = 0;
        }

        [Description("OTP-断电后保持-一次性编程")]
        public void WriteConfiguration()
        {
            OtpWrite(false);
        }

        [Description("仿真OTP-断电后恢复-可重复写入")]
        public void EmulateConfiguration()
        {
            OtpWrite(true);
        }

        private void OtpWrite(bool isEmulation)
        {
            OtpStatus = string.Empty;
            CompareReadedOtp = string.Empty;

            if (MySerialPort == null)
            {
                OtpStatus = "NG UART未初始化";
                CompareReadedOtp = "NG UART未初始化";
                return;
            }

            if (string.IsNullOrEmpty(OtpConfigFilePath))
            {
                OtpStatus = "NG 没有配置文件";
                CompareReadedOtp = "NG 没有配置文件";
                return;
            }

            lock (LockCfgFile)
            {
                if (!OtpConfigFilePath.ToLower().EndsWith(".ocfg"))
                {
                    OtpStatus = "NG 配置文件格式不正确 必须为ocfg格式";
                    CompareReadedOtp = "NG UART未初始化 必须为ocfg格式";
                    return;
                }
            }

            if (!File.Exists(OtpConfigFilePath))
            {
                OtpStatus = "NG 配置文件不存在";
                CompareReadedOtp = "NG 配置文件不存在";
                return;
            }

            var st = new Stopwatch();
            st.Start();

            Ocfgobject ocfgobject;
            lock (LockCfgFile)
            {
                try
                {
                    var lines = File.ReadAllLines(OtpConfigFilePath).ToList();
                    var str = lines.Aggregate(string.Empty, (current, l) => current + l);

                    ocfgobject = JsonConvert.DeserializeObject<Ocfgobject>(str);
                }
                catch (Exception)
                {
                    ocfgobject = null;
                }
            }

            if (ocfgobject == null)
            {
                OtpStatus = "NG 配置文件读取失败";
                CompareReadedOtp = "NG 配置文件读取失败";
                return;
            }

            ResetMrcCount();

            for (var i = 0; i < 3; i++)
                BroadcastSync();

            // PM_CHANGE_INIT
            for (var i = 0; i < 2; i++)
                PM_CHANGE(PmChangeCmd.InitMode);

            // PM_CHANGE_OTP;
            PM_CHANGE(PmChangeCmd.OtpMode);

            if (isEmulation)
                WriteReg(0x00, 0x80, 1, new byte[] { 0x3B, 0xD2 }); // 0x80,仿真3BD2
            else
                WriteReg(0x00, 0x81, 1, new byte[] { 0xA4, 0x7B }); // 0x81,实际A47B

            var writefirst32WordsDatas = new Dictionary<byte, ushort>();
            for (var i = 0x83; i < 0xA3; i++)
                writefirst32WordsDatas.Add((byte)i, 0);
            var writeLast8WordsDatas = new Dictionary<byte, ushort>();
            for (var i = 0xA3; i < 0xAB; i++)
                writeLast8WordsDatas.Add((byte)i, 0);

            if (!UpdateOtpRegValueFromOcfg(writefirst32WordsDatas, ocfgobject))
                return;

            if (!UpdateOtpRegValueFromOcfg(writeLast8WordsDatas, ocfgobject))
                return;

            var data1 = new List<byte>();
            foreach (var key in writefirst32WordsDatas.Keys)
            {
                var value = writefirst32WordsDatas[key];
                var bits = Convert.ToString(value, 2).PadLeft(16, '0').ToCharArray().Reverse().ToArray();

                var str1 = string.Empty;
                for (var i = 7; i >= 0; i--)
                    str1 += bits[i];

                var str2 = string.Empty;
                for (var i = 15; i >= 8; i--)
                    str2 += bits[i];

                data1.Add(Convert.ToByte(str2, 2));
                data1.Add(Convert.ToByte(str1, 2));
            }

            var data2 = new List<byte>();
            foreach (var key in writeLast8WordsDatas.Keys)
            {
                var value = writeLast8WordsDatas[key];
                var bits = Convert.ToString(value, 2).PadLeft(16, '0').ToCharArray().Reverse().ToArray();

                var str1 = string.Empty;
                for (var i = 7; i >= 0; i--)
                    str1 += bits[i];

                var str2 = string.Empty;
                for (var i = 15; i >= 8; i--)
                    str2 += bits[i];

                data2.Add(Convert.ToByte(str2, 2));
                data2.Add(Convert.ToByte(str1, 2));
            }

            var toCrc16 = new List<byte>();
            toCrc16.AddRange(data1);
            toCrc16.Add(data2[0]);
            toCrc16.Add(data2[1]);
            toCrc16.Add(data2[2]);
            toCrc16.Add(data2[3]);
            toCrc16.Add(data2[4]);
            toCrc16.Add(data2[5]);

            var crc16Info = new CrcHelper();
            crc16Info.CRC_Table_Init(crc16Info.Crc16CcittFalse);
            var crc16 = BitConverter.GetBytes(crc16Info.CALC_CRC(crc16Info.Crc16CcittFalse, toCrc16.ToArray()));
            data2[6] = crc16[1];
            data2[7] = crc16[0];

            var data1Str = ValueHelper.GetHextStr(data1.ToArray()).Replace(" ", "");
            var data2Str = ValueHelper.GetHextStr(data2.ToArray()).Replace(" ", "");

            WriteReg(0x00, 0x83, 32, data1.ToArray());
            if (!isEmulation)
                Thread.Sleep(int.Parse(OtpSleepTime1));
            WriteReg(0x00, 0xA3, 8, data2.ToArray());
            if (!isEmulation)
                Thread.Sleep(int.Parse(OtpSleepTime1));

            for (var i = 0; i < 2; i++)
                PM_CHANGE(PmChangeCmd.InitMode);
            HWCR_ALL(byte.Parse(ocfgobject.OTP_SLAVE_ID.ToString()));
            Thread.Sleep(10);
            PM_CHANGE(PmChangeCmd.InitMode);

            st.Stop();
            Console.WriteLine(@"7002 cost: " + st.ElapsedMilliseconds);

            {
                // read 0x82
                string recv;
                if (ReadReg(byte.Parse(ocfgobject.OTP_SLAVE_ID.ToString()), 0x82, 0x01, out recv))
                {
                    try
                    {
                        OtpStatus = "0x" + recv.Substring(2, 2);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }

            {
                // read 0xAB
                string recv;
                if (ReadReg(byte.Parse(ocfgobject.OTP_SLAVE_ID.ToString()), 0xAB, 0x04, out recv))
                {
                    if (IsPrintDebugLog == 1.ToString())
                        Console.WriteLine(@"Read OTP_LOG_WORD From 0xAB: " + recv);
                }
            }

            {
                string recv1;
                var isRead32WordsOk = ReadReg(byte.Parse(ocfgobject.OTP_SLAVE_ID.ToString()), 0x83, 32, out recv1);

                string recv2;
                var isRead8WordsOk = ReadReg(byte.Parse(ocfgobject.OTP_SLAVE_ID.ToString()), 0xA3, 8, out recv2);

                if (isRead32WordsOk && isRead8WordsOk)
                {
                    if (!string.IsNullOrEmpty(recv1) && !string.IsNullOrEmpty(recv2) &&
                        string.Equals(data1Str, recv1, StringComparison.CurrentCultureIgnoreCase) &&
                        string.Equals(data2Str, recv2, StringComparison.CurrentCultureIgnoreCase))
                    {
                        CompareReadedOtp = "OK";
                        return;
                    }
                }
            }

            CompareReadedOtp = "NG";
        }

        [Description("读OTP参数-并与配置文件对比")]
        public void OtpRead()
        {
            CompareReadedOtp = string.Empty;

            if (MySerialPort == null)
            {
                OtpStatus = "NG UART未初始化";
                CompareReadedOtp = "NG UART未初始化";
                return;
            }

            if (string.IsNullOrEmpty(OtpConfigFilePath))
            {
                OtpStatus = "NG 没有配置文件";
                CompareReadedOtp = "NG 没有配置文件";
                return;
            }

            if (!OtpConfigFilePath.ToLower().EndsWith(".ocfg"))
            {
                OtpStatus = "NG 配置文件格式不正确 必须为ocfg格式";
                CompareReadedOtp = "NG UART未初始化 必须为ocfg格式";
                return;
            }

            if (!File.Exists(OtpConfigFilePath))
            {
                OtpStatus = "NG 配置文件不存在";
                CompareReadedOtp = "NG 配置文件不存在";
                return;
            }

            Ocfgobject ocfgobject;
            lock (LockCfgFile)
            {
                try
                {
                    var lines = File.ReadAllLines(OtpConfigFilePath).ToList();
                    var str = lines.Aggregate(string.Empty, (current, l) => current + l);

                    ocfgobject = JsonConvert.DeserializeObject<Ocfgobject>(str);
                }
                catch (Exception)
                {
                    ocfgobject = null;
                }
            }

            if (ocfgobject == null)
            {
                OtpStatus = "NG 配置文件读取失败";
                CompareReadedOtp = "NG 配置文件读取失败";
                return;
            }

            ResetMrcCount();

            for (var i = 0; i < 3; i++)
                BroadcastSync();

            var writefirst32WordsDatas = new Dictionary<byte, ushort>();
            for (var i = 0x83; i < 0xA3; i++)
                writefirst32WordsDatas.Add((byte)i, 0);
            var writeLast8WordsDatas = new Dictionary<byte, ushort>();
            for (var i = 0xA3; i < 0xAB; i++)
                writeLast8WordsDatas.Add((byte)i, 0);

            if (!UpdateOtpRegValueFromOcfg(writefirst32WordsDatas, ocfgobject))
                return;

            if (!UpdateOtpRegValueFromOcfg(writeLast8WordsDatas, ocfgobject))
                return;

            var data1 = new List<byte>();
            foreach (var key in writefirst32WordsDatas.Keys)
            {
                var value = writefirst32WordsDatas[key];
                var bits = Convert.ToString(value, 2).PadLeft(16, '0').ToCharArray().Reverse().ToArray();

                var str1 = string.Empty;
                for (var i = 7; i >= 0; i--)
                    str1 += bits[i];

                var str2 = string.Empty;
                for (var i = 15; i >= 8; i--)
                    str2 += bits[i];

                data1.Add(Convert.ToByte(str2, 2));
                data1.Add(Convert.ToByte(str1, 2));
            }

            var data2 = new List<byte>();
            foreach (var key in writeLast8WordsDatas.Keys)
            {
                var value = writeLast8WordsDatas[key];
                var bits = Convert.ToString(value, 2).PadLeft(16, '0').ToCharArray().Reverse().ToArray();

                var str1 = string.Empty;
                for (var i = 7; i >= 0; i--)
                    str1 += bits[i];

                var str2 = string.Empty;
                for (var i = 15; i >= 8; i--)
                    str2 += bits[i];

                data2.Add(Convert.ToByte(str2, 2));
                data2.Add(Convert.ToByte(str1, 2));
            }

            var toCrc16 = new List<byte>();
            toCrc16.AddRange(data1);
            toCrc16.Add(data2[0]);
            toCrc16.Add(data2[1]);
            toCrc16.Add(data2[2]);
            toCrc16.Add(data2[3]);
            toCrc16.Add(data2[4]);
            toCrc16.Add(data2[5]);

            var crc16Info = new CrcHelper();
            crc16Info.CRC_Table_Init(crc16Info.Crc16CcittFalse);
            var crc16 = BitConverter.GetBytes(crc16Info.CALC_CRC(crc16Info.Crc16CcittFalse, toCrc16.ToArray()));
            data2[6] = crc16[1];
            data2[7] = crc16[0];

            var data1Str = ValueHelper.GetHextStr(data1.ToArray()).Replace(" ", "");
            var data2Str = ValueHelper.GetHextStr(data2.ToArray()).Replace(" ", "");

            {
                string recv1;
                var isRead32WordsOk = ReadReg(byte.Parse(ocfgobject.OTP_SLAVE_ID.ToString()), 0x83, 32, out recv1);

                string recv2;
                var isRead8WordsOk = ReadReg(byte.Parse(ocfgobject.OTP_SLAVE_ID.ToString()), 0xA3, 8, out recv2);

                if (isRead32WordsOk && isRead8WordsOk)
                {
                    if (!string.IsNullOrEmpty(recv1) && !string.IsNullOrEmpty(recv2) &&
                        string.Equals(data1Str, recv1, StringComparison.CurrentCultureIgnoreCase) &&
                        string.Equals(data2Str, recv2, StringComparison.CurrentCultureIgnoreCase))
                    {
                        CompareReadedOtp = "OK";
                        return;
                    }
                }
            }

            CompareReadedOtp = "NG";
        }

        private bool UpdateOtpRegValueFromOcfg(
            Dictionary<byte, ushort> dic, Ocfgobject ocfgobject)
        {
            try
            {
                var listKeys = dic.Keys.ToList();
                foreach (var key in listKeys)
                {
                    var isFindKey = false;
                    foreach (var property in ocfgobject.GetType().GetProperties())
                    {
                        if (property.Name.Contains("DEBUG_DATA")) continue;
                        var ocfgValue = property.GetValue(ocfgobject);
                        if (ocfgValue == null) continue;
                        ushort ocfgUshortValue;
                        //if (ocfgValue.ToString() == "-1")
                        //{
                        //    ocfgUshortValue = 0;
                        //}
                        //else
                        {
                            if (!ushort.TryParse(ocfgValue.ToString(), out ocfgUshortValue))
                            {
                                continue;
                            }
                        }
                        if (Attribute.GetCustomAttribute(property, typeof(OtpDefinition)) == null) continue;
                        var otpDefinition =
                            (OtpDefinition)Attribute.GetCustomAttribute(property, typeof(OtpDefinition));
                        if (otpDefinition.RegAddr != key)
                            continue;
                        isFindKey = true;

                        var bits = Convert.ToString(dic[key], 2).PadLeft(16, '0').ToCharArray().Reverse().ToArray();

                        var bitsFromOcfgFile = new List<char>();
                        var baseIndex = 0;
                        for (var i = otpDefinition.StartBit; i <= otpDefinition.EndBit; i++)
                        {
                            bitsFromOcfgFile.Add(Convert.ToString(ocfgUshortValue, 2).PadLeft(16, '0').ToCharArray().Reverse().ToArray()[baseIndex]);
                            baseIndex++;
                        }
                        Array.Copy(bitsFromOcfgFile.ToArray(), 0, bits, otpDefinition.StartBit, otpDefinition.EndBit - otpDefinition.StartBit + 1);

                        var str = string.Empty;
                        for (var i = 15; i >= 0; i--)
                            str += bits[i];
                        dic[key] = Convert.ToUInt16(str, 2);
                    }

                    if (isFindKey || key == 0xA6)
                        continue;
                    OtpStatus = string.Format("NG 配置文件读取失败 未找到地址{0}的OTP数据", ValueHelper.GetHextStrWithOx(key));
                    CompareReadedOtp = string.Format("NG 配置文件读取失败 未找到地址{0}的OTP数据", ValueHelper.GetHextStrWithOx(key));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                OtpStatus = string.Format("NG {0}", ex.Message);
                CompareReadedOtp = string.Format("NG {0}", ex.Message);
                return false;
            }
        }

        //[Description("测试仿真OTP")]
        //public void TestOtpWrite(string id)
        //{
        //    BroadcastSync();
        //    BroadcastSync();
        //    BroadcastSync();

        //    // PM_CHANGE
        //    for (var i = 0; i < 2; i++)
        //    {
        //        PM_CHANGE(PmChangeCmd.InitMode);
        //    }

        //    //Thread.Sleep(5);
        //    PM_CHANGE(PmChangeCmd.OtpMode);
        //    //Thread.Sleep(5);
        //    WriteReg(0x00, 0x80, 1, new byte[] { 0x3B, 0xD2 }); // 实际A47B,仿真3BD2
        //    //Thread.Sleep(50);
        //    //WriteReg(0x00, 0xa5, 1, new byte[] { 0x0, byte.Parse(id) });

        //    var data1Str =
        //        "80 80 80 80 80 80 80 80 80 80 80 80 80 80 80 80 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 08 20 08 20 08 20 08 20 08 20 08 20 08 20 18 20 FF FF 86 F3 09 D0 FF FF 20 20 FF FF 00 00"
        //            .Replace(" ", "");
        //    //var data2Str = "00 00 00 07 00 04 F1 03 00 00 00 00 00 00 00 00".Replace(" ", "");
        //    var data2Str = ("00 00 00 07 00 " + ValueHelper.GetHextStr(byte.Parse(id)) + " F1 03 00 00 00 00 00 00 00 00").Replace(" ", "");
        //    var data1 = new List<byte>();
        //    for (var i = 0; i < data1Str.Length; i = i + 2)
        //    {
        //        data1.Add(Convert.ToByte(data1Str.Substring(i, 2), 16));
        //    }
        //    var data2 = new List<byte>();
        //    for (var i = 0; i < data2Str.Length; i = i + 2)
        //    {
        //        data2.Add(Convert.ToByte(data2Str.Substring(i, 2), 16));
        //    }

        //    var toCrc16 = new List<byte>();
        //    toCrc16.AddRange(data1);
        //    toCrc16.Add(data2[0]);
        //    toCrc16.Add(data2[1]);
        //    toCrc16.Add(data2[2]);
        //    toCrc16.Add(data2[3]);
        //    toCrc16.Add(data2[4]);
        //    toCrc16.Add(data2[5]);

        //    var crc16Info = new CrcHelper();
        //    crc16Info.CRC_Table_Init(crc16Info.Crc16CcittFalse);
        //    var crc16 = BitConverter.GetBytes(crc16Info.CALC_CRC(crc16Info.Crc16CcittFalse, toCrc16.ToArray()));
        //    data2[6] = crc16[1];
        //    data2[7] = crc16[0];

        //    WriteReg(0x00, 0x83, 32, data1.ToArray());
        //    WriteReg(0x00, 0xA3, 8, data2.ToArray());

        //    //Thread.Sleep(50);
        //    for (var i = 0; i < 2; i++)
        //    {
        //        PM_CHANGE(PmChangeCmd.InitMode);
        //    }

        //    {
        //        string recv;
        //        if (ReadReg(byte.Parse(id), 0x82, 0x01, out recv))
        //        {
        //            Console.WriteLine("读OTP_status: " + recv);
        //        }
        //    }

        //    Read_OTP_PWM_DC_GPIN0(id);
        //}

        //[Description("读OTP_PWM_DC_GPIN1")]
        //public void Read_OTP_PWM_DC_GPIN1()
        //{
        //    string recv;
        //    if (ReadReg(0x01, 0x83, 0x08, out recv))
        //    {
        //        //Console.WriteLine("读OTP_PWM_DC_GPIN1: " + recv);
        //    }
        //}

        [Description("读OTP_PWM_DC_GPIN0")]
        public void Read_OTP_PWM_DC_GPIN0(string devAddr)
        {
            OtpPwmDcGpin0 = string.Empty;

            byte slaveId;
            if (byte.TryParse(devAddr, out slaveId))
            {
                if (slaveId >= 1 && slaveId <= 31)
                {
                    string recv;
                    if (ReadReg(byte.Parse(devAddr), 0x8B, 0x08, out recv))
                    {
                        OtpPwmDcGpin0 = recv;
                        Console.WriteLine("读OTP_PWM_DC_GPIN0: " + recv);
                    }
                }
            }
        }

        [Description("读OTP的40个Words")]
        public void ReadOtpWords(string devAddr)
        {
            TheFirst32Words = string.Empty;
            TheLast8Words = string.Empty;

            byte slaveId;
            if (byte.TryParse(devAddr, out slaveId))
            {
                if (slaveId >= 1 && slaveId <= 31)
                {
                    string recv1;
                    var isRead32WordsOk = ReadReg(slaveId, 0x83, 32, out recv1);

                    string recv2;
                    var isRead8WordsOk = ReadReg(slaveId, 0xA3, 8, out recv2);

                    if (isRead32WordsOk && isRead8WordsOk)
                    {
                        TheFirst32Words = recv1.Replace(" ", "");
                        TheLast8Words = recv2.Replace(" ", "");
                    }
                }
            }
        }

        private void BroadcastSync()
        {
            lock (_lockSend)
            {
                var broadcastSyncBytes = new List<byte> { SyncByte };
                var mrcBits = _mrcList[_mrcCount];
                var mrcByte = Convert.ToByte("000000" + mrcBits, 2);
                _mrcCount++;
                if (_mrcCount > 3)
                    _mrcCount = 0;

                const byte slaveAddr = 0x00;

                const string dlcBits = "000"; //Convert.ToString(dlc, 2).PadLeft(8, '0').Substring(5, 3);
                var dlcBytes = Convert.ToByte("00000" + dlcBits, 2);
                var slaveAddrBits = Convert.ToString(slaveAddr, 2).PadLeft(8, '0').Substring(3, 5);
                var funcByte = Convert.ToByte("00000" + BroadcastDutyCycleSynchronization, 2);

                var crc3 = GetMasterReqCrc3(slaveAddr, mrcByte, dlcBytes, funcByte);
                var crc3Bits = Convert.ToString(crc3, 2).PadLeft(8, '0').Substring(5, 3);

                broadcastSyncBytes.Add(Convert.ToByte(string.Format("{0}{1}", crc3Bits, slaveAddrBits), 2));
                broadcastSyncBytes.Add(Convert.ToByte(string.Format("{0}{1}{2}", mrcBits, dlcBits, BroadcastDutyCycleSynchronization), 2));

                MySerialPort.SendCommand(broadcastSyncBytes.ToArray());
            }
        }

        private void PM_CHANGE(PmChangeCmd pmChangeCmd)
        {
            lock (_lockSend)
            {
                var pmChangeBytes = new List<byte> { SyncByte };
                var mrcBits = _mrcList[_mrcCount];
                var mrcByte = Convert.ToByte("000000" + mrcBits, 2);
                _mrcCount++;
                if (_mrcCount > 3)
                    _mrcCount = 0;

                const byte numOfWords = 0x01;
                const byte slaveAddr = 0x00;

                var dlcBits = _dlcCodeList[numOfWords];//Convert.ToString(dlc, 2).PadLeft(8, '0').Substring(5, 3);
                var dlcBytes = Convert.ToByte("00000" + dlcBits, 2);
                var slaveAddrBits = Convert.ToString(slaveAddr, 2).PadLeft(8, '0').Substring(3, 5);
                var funcByte = Convert.ToByte("00000" + PowerModeChange, 2);

                var crc3 = GetMasterReqCrc3(slaveAddr, mrcByte, dlcBytes, funcByte);
                var crc3Bits = Convert.ToString(crc3, 2).PadLeft(8, '0').Substring(5, 3);

                pmChangeBytes.Add(Convert.ToByte(string.Format("{0}{1}", crc3Bits, slaveAddrBits), 2));
                pmChangeBytes.Add(Convert.ToByte(string.Format("{0}{1}{2}", mrcBits, dlcBits, PowerModeChange), 2));

                var cmd = new byte[] { (byte)pmChangeCmd, 0x00 };
                pmChangeBytes.AddRange(cmd);
                pmChangeBytes.Add(GetCrc8(cmd, 2));

                MySerialPort.SendCommand(pmChangeBytes.ToArray());
            }
        }

        private void HWCR_ALL(byte slaveAddr)
        {
            lock (_lockSend)
            {
                var hwcrBytes = new List<byte> { SyncByte };
                var mrcBits = _mrcList[_mrcCount];
                var mrcByte = Convert.ToByte("000000" + mrcBits, 2);
                _mrcCount++;
                if (_mrcCount > 3)
                    _mrcCount = 0;

                const byte numOfWords = 0x04;

                var dlcBits = _dlcCodeList[numOfWords];//Convert.ToString(dlc, 2).PadLeft(8, '0').Substring(5, 3);
                var dlcBytes = Convert.ToByte("00000" + dlcBits, 2);
                var slaveAddrBits = Convert.ToString(slaveAddr, 2).PadLeft(8, '0').Substring(3, 5);
                var funcByte = Convert.ToByte("00000" + HardwareControlFrame, 2);

                var crc3 = GetMasterReqCrc3(slaveAddr, mrcByte, dlcBytes, funcByte);
                var crc3Bits = Convert.ToString(crc3, 2).PadLeft(8, '0').Substring(5, 3);

                hwcrBytes.Add(Convert.ToByte(string.Format("{0}{1}", crc3Bits, slaveAddrBits), 2));
                hwcrBytes.Add(Convert.ToByte(string.Format("{0}{1}{2}", mrcBits, dlcBits, HardwareControlFrame), 2));

                var cmd = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0xFF };
                hwcrBytes.AddRange(cmd);
                hwcrBytes.Add(GetCrc8(cmd, 8));

                MySerialPort.SendCommand(hwcrBytes.ToArray());
            }
        }

        private bool ReadReg(byte slaveAddr, byte startRegAddr, byte numOfWords, out string recv)
        {
            recv = string.Empty;

            if (MySerialPort == null)
                return false;

            lock (_lockSend)
            {
                if (numOfWords == 1 || numOfWords == 2 ||
                    numOfWords == 4 || numOfWords == 8 ||
                    numOfWords == 12 || numOfWords == 16 || numOfWords == 32)
                {
                    var readBytes = new List<byte> { SyncByte };

                    var mrcBits = _mrcList[_mrcCount];
                    var mrcByte = Convert.ToByte("000000" + mrcBits, 2);
                    _mrcCount++;
                    if (_mrcCount > 3)
                        _mrcCount = 0;

                    var dlcBits = _dlcCodeList[numOfWords];
                    var dlcBytes = Convert.ToByte("00000" + dlcBits, 2);
                    var slaveAddrBits = Convert.ToString(slaveAddr, 2).PadLeft(8, '0').Substring(3, 5);
                    var funcByte = Convert.ToByte("00000" + ReadRegister, 2);

                    var crc3 = GetMasterReqCrc3(slaveAddr, mrcByte, dlcBytes, funcByte);
                    var crc3Bits = Convert.ToString(crc3, 2).PadLeft(8, '0').Substring(5, 3);

                    readBytes.Add(Convert.ToByte(string.Format("{0}{1}", crc3Bits, slaveAddrBits), 2));
                    readBytes.Add(Convert.ToByte(string.Format("{0}{1}{2}", mrcBits, dlcBits, ReadRegister), 2));
                    readBytes.Add(startRegAddr);

                    _readCount = numOfWords;
                    _isRead = true;
                    foreach (var t in readBytes)
                        _readCmd += ValueHelper.GetHextStr(t);

                    MySerialPort.SendCommand(readBytes.ToArray());

                    if (!_waitReadHandle.WaitOne(100))
                    {
                        _readCmd = string.Empty;
                        _isRead = false;
                        return false;
                    }

                    recv = _readRecv;
                    return true;
                }
            }

            return false;
        }

        private void WriteReg(
            byte slaveAddr, byte startRegAddr, byte numOfWords, IReadOnlyCollection<byte> datas)
        {
            if (MySerialPort == null)
                return;

            lock (_lockSend)
            {
                if (numOfWords == 1 || numOfWords == 2 ||
                    numOfWords == 4 || numOfWords == 8 ||
                    numOfWords == 12 || numOfWords == 16 || numOfWords == 32)
                {
                    if (datas != null && datas.Count == numOfWords * 2)
                    {
                        var writeBytes = new List<byte> { SyncByte };

                        var mrcBits = _mrcList[_mrcCount];
                        var mrcByte = Convert.ToByte("000000" + mrcBits, 2);
                        _mrcCount++;
                        if (_mrcCount > 3)
                            _mrcCount = 0;

                        var dlcBits = _dlcCodeList[numOfWords];
                        var dlcBytes = Convert.ToByte("00000" + dlcBits, 2);
                        var slaveAddrBits = Convert.ToString(slaveAddr, 2).PadLeft(8, '0').Substring(3, 5);
                        var funcByte = Convert.ToByte("00000" + WriteRegister, 2);

                        var crc3 = GetMasterReqCrc3(slaveAddr, mrcByte, dlcBytes, funcByte);
                        var crc3Bits = Convert.ToString(crc3, 2).PadLeft(8, '0').Substring(5, 3);

                        writeBytes.Add(Convert.ToByte(string.Format("{0}{1}", crc3Bits, slaveAddrBits), 2));
                        writeBytes.Add(Convert.ToByte(string.Format("{0}{1}{2}", mrcBits, dlcBits, WriteRegister), 2));

                        var toCrc8Data = new List<byte> { startRegAddr };
                        toCrc8Data.AddRange(datas);
                        var crc8 = GetCrc8(toCrc8Data.ToArray(), (byte)toCrc8Data.Count);

                        writeBytes.AddRange(toCrc8Data);
                        writeBytes.Add(crc8);
                        MySerialPort.SendCommand(writeBytes.ToArray());
                    }
                }
            }
        }

        private void GenerateHeader()
        {

        }

        #region common value

        private static readonly object LockCfgFile = new object();
        private readonly object _lockSend = new object();
        private readonly EventWaitHandle _waitReadHandle = new AutoResetEvent(false);

        private bool _isRead;
        private string _readCmd = string.Empty;
        private int _readCount;
        private string _readRecv = string.Empty;

        private int _mrcCount;
        private readonly Dictionary<int, string> _mrcList = new Dictionary<int, string>();

        private readonly Dictionary<int, string> _dlcCodeList = new Dictionary<int, string>();

        private const byte SyncByte = 0x55;

        private const string BroadcastDutyCycleSynchronization = "000"; // 55 20 40
        private const string DutyCycleShadowRegisterUpdate = "001";
        private const string RequestDiagnostics = "010";
        private const string HardwareControlFrame = "011";
        private const string WriteRegister = "100";
        private const string ReadRegister = "101";
        private const string PowerModeChange = "110";

        internal enum PmChangeCmd : byte
        {
            InitMode = 0x00,
            Res1 = 0x01,
            FailSafeMode = 0x02,
            OtpMode = 0x03
        }

        internal class Ocfgobject
        {
            public string DEVICE { get; set; }
            public string CFG_NAME { get; set; }

            [OtpDefinition(0xA5, "4:0")]
            public int OTP_SLAVE_ID { get; set; }

            [OtpDefinition(0x83, "7:0")]
            public int OTP_PWM_DC_GPIN1_0UT00 { get; set; }

            [OtpDefinition(0x83, "15:8")]
            public int OTP_PWM_DC_GPIN1_0UT01 { get; set; }

            [OtpDefinition(0x84, "7:0")]
            public int OTP_PWM_DC_GPIN1_0UT02 { get; set; }

            [OtpDefinition(0x84, "15:8")]
            public int OTP_PWM_DC_GPIN1_0UT03 { get; set; }

            [OtpDefinition(0x85, "7:0")]
            public int OTP_PWM_DC_GPIN1_0UT04 { get; set; }

            [OtpDefinition(0x85, "15:8")]
            public int OTP_PWM_DC_GPIN1_0UT05 { get; set; }

            [OtpDefinition(0x86, "7:0")]
            public int OTP_PWM_DC_GPIN1_0UT06 { get; set; }

            [OtpDefinition(0x86, "15:8")]
            public int OTP_PWM_DC_GPIN1_0UT07 { get; set; }

            [OtpDefinition(0x87, "7:0")]
            public int OTP_PWM_DC_GPIN1_0UT08 { get; set; }

            [OtpDefinition(0x87, "15:8")]
            public int OTP_PWM_DC_GPIN1_0UT09 { get; set; }

            [OtpDefinition(0x88, "7:0")]
            public int OTP_PWM_DC_GPIN1_0UT10 { get; set; }

            [OtpDefinition(0x88, "15:8")]
            public int OTP_PWM_DC_GPIN1_0UT11 { get; set; }

            [OtpDefinition(0x89, "7:0")]
            public int OTP_PWM_DC_GPIN1_0UT12 { get; set; }

            [OtpDefinition(0x89, "15:8")]
            public int OTP_PWM_DC_GPIN1_0UT13 { get; set; }

            [OtpDefinition(0x8A, "7:0")]
            public int OTP_PWM_DC_GPIN1_0UT14 { get; set; }

            [OtpDefinition(0x8A, "15:8")]
            public int OTP_PWM_DC_GPIN1_0UT15 { get; set; }

            [OtpDefinition(0x8B, "7:0")]
            public int OTP_PWM_DC_GPIN0_0UT00 { get; set; }

            [OtpDefinition(0x8B, "15:8")]
            public int OTP_PWM_DC_GPIN0_0UT01 { get; set; }

            [OtpDefinition(0x8C, "7:0")]
            public int OTP_PWM_DC_GPIN0_0UT02 { get; set; }

            [OtpDefinition(0x8C, "15:8")]
            public int OTP_PWM_DC_GPIN0_0UT03 { get; set; }

            [OtpDefinition(0x8D, "7:0")]
            public int OTP_PWM_DC_GPIN0_0UT04 { get; set; }

            [OtpDefinition(0x8D, "15:8")]
            public int OTP_PWM_DC_GPIN0_0UT05 { get; set; }

            [OtpDefinition(0x8E, "7:0")]
            public int OTP_PWM_DC_GPIN0_0UT06 { get; set; }

            [OtpDefinition(0x8E, "15:8")]
            public int OTP_PWM_DC_GPIN0_0UT07 { get; set; }

            [OtpDefinition(0x8F, "7:0")]
            public int OTP_PWM_DC_GPIN0_0UT08 { get; set; }

            [OtpDefinition(0x8F, "15:8")]
            public int OTP_PWM_DC_GPIN0_0UT09 { get; set; }

            [OtpDefinition(0x90, "7:0")]
            public int OTP_PWM_DC_GPIN0_0UT10 { get; set; }

            [OtpDefinition(0x90, "15:8")]
            public int OTP_PWM_DC_GPIN0_0UT11 { get; set; }

            [OtpDefinition(0x91, "7:0")]
            public int OTP_PWM_DC_GPIN0_0UT12 { get; set; }

            [OtpDefinition(0x91, "15:8")]
            public int OTP_PWM_DC_GPIN0_0UT13 { get; set; }

            [OtpDefinition(0x92, "7:0")]
            public int OTP_PWM_DC_GPIN0_0UT14 { get; set; }

            [OtpDefinition(0x92, "15:8")]
            public int OTP_PWM_DC_GPIN0_0UT15 { get; set; }

            [OtpDefinition(0x93, "15:0")]
            public int OTP_CH_SAFE_STATE { get; set; }

            [OtpDefinition(0x94, "5:0")]
            public int OTP_CH_ISET_OUT00 { get; set; }

            [OtpDefinition(0x94, "11:6")]
            public int OTP_CH_ISET_OUT01 { get; set; }

            [OtpDefinition(0x95, "5:0")]
            public int OTP_CH_ISET_OUT02 { get; set; }

            [OtpDefinition(0x95, "11:6")]
            public int OTP_CH_ISET_OUT03 { get; set; }

            [OtpDefinition(0x96, "5:0")]
            public int OTP_CH_ISET_OUT04 { get; set; }

            [OtpDefinition(0x96, "11:6")]
            public int OTP_CH_ISET_OUT05 { get; set; }

            [OtpDefinition(0x97, "5:0")]
            public int OTP_CH_ISET_OUT06 { get; set; }

            [OtpDefinition(0x97, "11:6")]
            public int OTP_CH_ISET_OUT07 { get; set; }

            [OtpDefinition(0x98, "5:0")]
            public int OTP_CH_ISET_OUT08 { get; set; }

            [OtpDefinition(0x98, "11:6")]
            public int OTP_CH_ISET_OUT09 { get; set; }

            [OtpDefinition(0x99, "5:0")]
            public int OTP_CH_ISET_OUT10 { get; set; }

            [OtpDefinition(0x99, "11:6")]
            public int OTP_CH_ISET_OUT11 { get; set; }

            [OtpDefinition(0x9A, "5:0")]
            public int OTP_CH_ISET_OUT12 { get; set; }

            [OtpDefinition(0x9A, "11:6")]
            public int OTP_CH_ISET_OUT13 { get; set; }

            [OtpDefinition(0x9B, "5:0")]
            public int OTP_CH_ISET_OUT14 { get; set; }

            [OtpDefinition(0x9B, "11:6")]
            public int OTP_CH_ISET_OUT15 { get; set; }

            [OtpDefinition(0x9C, "15:0")]
            public int OTP_PWM_PHASE_EN { get; set; }

            [OtpDefinition(0x9B, "13:12")]
            public int OTP_DIAG_DEBOUNCE { get; set; }

            [OtpDefinition(0x9B, "14:14")]
            public int OTP_LOW_PW_INIT { get; set; }

            [OtpDefinition(0x9D, "3:0")]
            public int OTP_PWM_FREQ { get; set; }

            [OtpDefinition(0x9D, "8:4")]
            public int OTP_PWM_PHASE_SHIFT { get; set; }

            [OtpDefinition(0x9D, "9:9")]
            public int OTP_GPIN0_DC_DEC_EN { get; set; }

            [OtpDefinition(0x9D, "10:10")]
            public int OTP_GPIN1_DC_DEC_EN { get; set; }

            [OtpDefinition(0x9D, "11:11")]
            public int OTP_CH_DCDC_OUT0_EN { get; set; }

            [OtpDefinition(0x9D, "12:12")]
            public int OTP_GPIN1_INV_EN { get; set; }

            [OtpDefinition(0x9D, "13:13")]
            public int OTP_GPIN0_Func_OE { get; set; }

            [OtpDefinition(0x9D, "14:14")]
            public int OTP_GPIN0_HiZ_EN { get; set; }

            [OtpDefinition(0x9D, "15:15")]
            public int OTP_GPIN1_HiZ_EN { get; set; }

            [OtpDefinition(0x9E, "4:0")]
            public int OTP_DIAG_VDEN_VS { get; set; }

            [OtpDefinition(0x9E, "9:5")]
            public int OTP_DIAG_VDEN_VLED { get; set; }

            [OtpDefinition(0x9E, "12:10")]
            public int OTP_DIAG_TDELAY { get; set; }

            [OtpDefinition(0x9E, "15:13")]
            public int OTP_DIAG_OUT15_ERRn_EN { get; set; }

            [OtpDefinition(0x9F, "15:0")]
            public int OTP_SHORT_WRN_EN { get; set; }

            [OtpDefinition(0xA0, "7:0")]
            public int OTP_SLS_TH0 { get; set; }

            [OtpDefinition(0xA0, "15:8")]
            public int OTP_SLS_TH1 { get; set; }

            [OtpDefinition(0xA1, "15:0")]
            public int OTP_DIAG_OUT_GROUP { get; set; }

            [OtpDefinition(0xA2, "15:0")]
            public int OTP_GPIN0_MAP { get; set; }

            [OtpDefinition(0xA3, "15:0")]
            public int OTP_GPIN1_MAP { get; set; }

            [OtpDefinition(0xA4, "2:0")]
            public int OTP_DIAG_WDT_SET { get; set; }

            [OtpDefinition(0xA4, "6:3")]
            public int OTP_VFWD_VLED_TH { get; set; }

            [OtpDefinition(0xA4, "10:7")]
            public int OTP_VFWD_VS_TH { get; set; }

            [OtpDefinition(0xA4, "11:11")]
            public int OTP_DIAG_mgnt_SET { get; set; }

            [OtpDefinition(0xA4, "12:12")]
            public int OTP_DIAG_OUT_SW_OFF_DC100 { get; set; }

            [OtpDefinition(0xA4, "13:13")]
            public int OTP_PWR_LOAD_EN { get; set; }

            [OtpDefinition(0xA4, "14:14")]
            public int OTP_DIAG_SLS_LO_CK { get; set; }

            [OtpDefinition(0xA4, "15:15")]
            public int OTP_CURR_WRN_REPORT { get; set; }

            [OtpDefinition(0xA7, "0:0")]
            public int OTP_CH_TH_DER_EN { get; set; }

            [OtpDefinition(0xA7, "3:1")]
            public int OTP_CH_TH_DER_TH { get; set; }

            [OtpDefinition(0xA7, "4:4")]
            public int OTP_PWR_OFF_LOAD_CH_SET00 { get; set; }

            [OtpDefinition(0xA7, "5:5")]
            public int OTP_PWR_OFF_LOAD_CH_SET02 { get; set; }

            [OtpDefinition(0xA7, "6:6")]
            public int OTP_PWR_OFF_LOAD_CH_SET08 { get; set; }

            [OtpDefinition(0xA7, "7:7")]
            public int OTP_PWR_OFF_LOAD_CH_SET10 { get; set; }

            [OtpDefinition(0xA7, "9:8")]
            public int OTP_PWR_OFF_LOAD_TH_CH01_SET { get; set; }

            [OtpDefinition(0xA7, "11:10")]
            public int OTP_PWR_OFF_LOAD_TH_CH23_SET { get; set; }

            [OtpDefinition(0xA7, "13:12")]
            public int OTP_PWR_OFF_LOAD_TH_CH89_SET { get; set; }

            [OtpDefinition(0xA7, "15:14")]
            public int OTP_PWR_OFF_LOAD_TH_CH1011_SET { get; set; }

            [OtpDefinition(0xA8, "1:0")]
            public int OTP_HSLI_T_BITSMPL { get; set; }

            [OtpDefinition(0xA8, "3:2")]
            public int OTP_HSLI_T_SYNC_BREAK { get; set; }

            [OtpDefinition(0xA8, "6:4")]
            public int OTP_HSLI_T_FRAME_DLY { get; set; }

            [OtpDefinition(0xA8, "7:7")]
            public int OTP_RAMP_EN { get; set; }

            [OtpDefinition(0xA9, "4:0")]
            public int OTP_VGPIN0_TJSTART { get; set; }

            [OtpDefinition(0xA9, "9:5")]
            public int OTP_VGPIN0_TJSTOP { get; set; }

            [OtpDefinition(0xA9, "10:10")]
            public int OTP_CH_TH_DER_G_PIN { get; set; }

            [OtpDefinition(0xAA, "15:0")]
            public int OTP_GP_WORD { get; set; }

            public int OTP_STATUS { get; set; }

            public int OTP_CUST_SGN { get; set; }

            [OtpDefinition(0xAB, "15:0")]
            public int OTP_LOG_WORD0 { get; set; }

            [OtpDefinition(0xAC, "15:0")]
            public int OTP_LOG_WORD1 { get; set; }

            [OtpDefinition(0xAD, "15:0")]
            public int OTP_LOG_WORD2 { get; set; }

            [OtpDefinition(0xAE, "15:0")]
            public int OTP_LOG_WORD3 { get; set; }

            public DEBUG_DATA DEBUG_DATA { get; set; }
        }

        internal class DEBUG_DATA
        {
            public string INFO { get; set; }
            public string HEX_DATA_16BIT { get; set; }
            public string ADDRESS_START { get; set; }
            public string ADDRESS_END { get; set; }
        }

        internal class OtpDefinition : Attribute
        {
            public byte RegAddr;
            public int StartBit;
            public int EndBit;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="regAddr"></param>
            /// <param name="bits">e.g 4:0</param>
            public OtpDefinition(byte regAddr, string bits)
            {
                RegAddr = regAddr;
                StartBit = int.Parse(bits.Split(':')[1]);
                EndBit = int.Parse(bits.Split(':')[0]);
            }
        }

        #endregion

        #region CRC

        private static byte GetCrc8(IReadOnlyList<byte> dataArray, byte dataLen)
        {
            var crc = Tld7002Crc8CalcSeed;

            for (var index = 0; index < dataLen; index++)
            {
                /* use data bytes */
                crc ^= dataArray[index];

                for (var bit = 0; bit < 8; bit++)
                {
                    if ((crc & 0x80) != 0x00)
                    {
                        crc <<= 0x01;
                        crc ^= 0x1D;
                    }
                    else
                    {
                        crc <<= 0x01;
                    }
                }
            }

            crc = (byte)(crc ^ 0xFF);
            return crc;
        }

        private byte GetMasterReqCrc3(byte addr, byte mrc, byte dlc, byte func)
        {
            // 55 60 8E 03 00 6A
            // 55 A0 CE 00 00 BE
            const byte tld7002CrcAddressAddressMsk = 0x1F;
            const byte tld7002MrcDlcFunMrcMsk = 0xC0;
            const byte tld7002MrcDlcFunFunMsk = 0x07;
            const byte tld7002MrcDlcFunDlcMsk = 0x38;

            var crc = Tld7002Crc3CalcSeed;
            var crcAddressByte = (addr << 0x00) & tld7002CrcAddressAddressMsk;
            var mrcDlcFunByte = (byte)0x00;
            mrcDlcFunByte |= (byte)(((mrc << 0x06) & tld7002MrcDlcFunMrcMsk) |
                                ((dlc << 0x03) & tld7002MrcDlcFunDlcMsk) |
                                ((func << 0x00) & tld7002MrcDlcFunFunMsk));
            crc = _tld7002LookupCrc35Bit[crc ^ (crcAddressByte & 0x1FU)];
            crc = _tld7002MirrorMidCrc3[crc];
            crc = _tld7002LookupCrc38Bit[crc ^ mrcDlcFunByte];
            //Console.WriteLine(crc);

            return crc;
        }

        private bool CheckSlaveResCrc3(byte outputStatusByte, byte ack)
        {
            byte ostByte = 0;
            byte ackByte = 0;
            var crc = Tld7002Crc3CalcSeed;

            var outputStatusBits = Convert.ToString(outputStatusByte, 2).PadLeft(8, '0');
            var ackBits = Convert.ToString(ack, 2).PadLeft(8, '0');

            ostByte |= (byte)(((byte)(outputStatusBits.Substring(7, 1) == "1" ? 0x01 : 0x00) << 0x00) |
                    ((byte)(outputStatusBits.Substring(6, 1) == "1" ? 0x01 : 0x00) << 0x01) |
                    ((byte)(outputStatusBits.Substring(5, 1) == "1" ? 0x01 : 0x00) << 0x02) |
                    ((byte)(outputStatusBits.Substring(4, 1) == "1" ? 0x01 : 0x00) << 0x03) |
                    ((byte)(outputStatusBits.Substring(3, 1) == "1" ? 0x01 : 0x00) << 0x04) |
                    ((byte)(outputStatusBits.Substring(2, 1) == "1" ? 0x01 : 0x00) << 0x05) |
                    ((byte)(outputStatusBits.Substring(1, 1) == "1" ? 0x01 : 0x00) << 0x06) |
                    ((byte)(outputStatusBits.Substring(0, 1) == "1" ? 0x01 : 0x00) << 0x07));

            ackByte |= (byte)(((byte)(ackBits.Substring(7, 1) == "1" ? 0x01 : 0x00) << 0x00) |
                                (Convert.ToByte(ackBits.Substring(5, 2), 2) << 0x01) |
                                (Convert.ToByte(ackBits.Substring(3, 2), 2) << 0x03));

            crc = _tld7002LookupCrc38Bit[crc ^ ostByte];
            crc = _tld7002MirrorMidCrc3[crc];
            crc = _tld7002LookupCrc35Bit[crc ^ (ackByte & 0x1FU)];
            var crcCorrect = crc == Convert.ToByte(ackBits.Substring(0, 3), 2);

            return crcCorrect;
        }

        private const byte Tld7002Crc3CalcSeed = 0x05;
        private const byte Tld7002Crc8CalcSeed = 0xFF;

        private readonly byte[] _tld7002LookupCrc35Bit =
        {
            0, 1, 5, 4, 7, 6, 2, 3, 6, 7, 3, 2, 1, 0, 4, 5,
            3, 2, 6, 7, 4, 5, 1, 0, 5, 4, 0, 1, 2, 3, 7, 6
        };

        private readonly byte[] _tld7002MirrorMidCrc3 = { 0, 4, 2, 6, 1, 5, 3, 7 };

        private readonly byte[] _tld7002LookupCrc38Bit =
        {
            0, 3, 4, 7, 2, 1, 6, 5, 1, 2, 5, 6, 3, 0, 7, 4,
            5, 6, 1, 2, 7, 4, 3, 0, 4, 7, 0, 3, 6, 5, 2, 1,
            7, 4, 3, 0, 5, 6, 1, 2, 6, 5, 2, 1, 4, 7, 0, 3,
            2, 1, 6, 5, 0, 3, 4, 7, 3, 0, 7, 4, 1, 2, 5, 6,
            6, 5, 2, 1, 4, 7, 0, 3, 7, 4, 3, 0, 5, 6, 1, 2,
            3, 0, 7, 4, 1, 2, 5, 6, 2, 1, 6, 5, 0, 3, 4, 7,
            1, 2, 5, 6, 3, 0, 7, 4, 0, 3, 4, 7, 2, 1, 6, 5,
            4, 7, 0, 3, 6, 5, 2, 1, 5, 6, 1, 2, 7, 4, 3, 0,
            3, 0, 7, 4, 1, 2, 5, 6, 2, 1, 6, 5, 0, 3, 4, 7,
            6, 5, 2, 1, 4, 7, 0, 3, 7, 4, 3, 0, 5, 6, 1, 2,
            4, 7, 0, 3, 6, 5, 2, 1, 5, 6, 1, 2, 7, 4, 3, 0,
            1, 2, 5, 6, 3, 0, 7, 4, 0, 3, 4, 7, 2, 1, 6, 5,
            5, 6, 1, 2, 7, 4, 3, 0, 4, 7, 0, 3, 6, 5, 2, 1,
            0, 3, 4, 7, 2, 1, 6, 5, 1, 2, 5, 6, 3, 0, 7, 4,
            2, 1, 6, 5, 0, 3, 4, 7, 3, 0, 7, 4, 1, 2, 5, 6,
            7, 4, 3, 0, 5, 6, 1, 2, 6, 5, 2, 1, 4, 7, 0, 3
        };

        #endregion

        #region LED控制

        private readonly Dictionary<int, Dictionary<int, ushort>> _slaveLedStatus =
            new Dictionary<int, Dictionary<int, ushort>>();

        private readonly Thread _ledControlThread;
        private bool _isLedControl;

        [Description("开启LED控制")]
        public void StartLedControl()
        {
            Tld7002_Init();
            _isLedControl = true;
        }

        [Description("关闭LED控制")]
        public void StopLedControl()
        {
            _isLedControl = false;
        }

        [Description("添加SLAVE")]
        public void AddSlave(int addr)
        {
            if (addr < 1 || addr > 31)
                return;

            lock (_slaveLedStatus)
            {
                if (_slaveLedStatus.ContainsKey(addr))
                    return;

                _slaveLedStatus.Add(addr, new Dictionary<int, ushort>());
                for (var i = 1; i <= 16; i++)
                    _slaveLedStatus[addr].Add(i, 0);
            }
        }

        [Description("移除SLAVE")]
        public void RemoveSlave(int addr)
        {
            lock (_slaveLedStatus)
            {
                if (_slaveLedStatus.ContainsKey(addr))
                    _slaveLedStatus.Remove(addr);
            }
        }

        [Description("打开目标地址所有LED")]
        public void OpenSlave(string devAddr)
        {
            byte slaveId;
            if (!byte.TryParse(devAddr, out slaveId))
                return;

            if (slaveId < 1 || slaveId > 31)
                return;

            lock (_slaveLedStatus)
            {
                if (!_slaveLedStatus.ContainsKey(slaveId))
                    return;

                foreach (var t in _slaveLedStatus[slaveId].ToList())
                    _slaveLedStatus[slaveId][t.Key] = CalculateDc14Bit(256);
            }
        }

        [Description("关闭目标地址所有LED")]
        public void CloseSlave(string devAddr)
        {
            byte slaveId;
            if (!byte.TryParse(devAddr, out slaveId))
                return;

            if (slaveId < 1 || slaveId > 31)
                return;

            lock (_slaveLedStatus)
            {
                if (!_slaveLedStatus.ContainsKey(slaveId))
                    return;

                foreach (var t in _slaveLedStatus[slaveId].ToList())
                    _slaveLedStatus[slaveId][t.Key] = CalculateDc14Bit(0);
            }
        }

        [Description("设置目标地址的单个LED的DC")]
        public void OpenSlaveSingleLed(string devAddr, string ledAddr, string ledDc)
        {
            byte slaveId, ledId;
            int dc;
            if (!byte.TryParse(devAddr, out slaveId) ||
                !byte.TryParse(ledAddr, out ledId) ||
                !int.TryParse(ledDc, out dc))
                return;

            if (slaveId < 1 || slaveId > 31 || ledId < 1 || ledId > 16 || dc < 0 || dc > 256)
                return;

            lock (_slaveLedStatus)
            {
                if (_slaveLedStatus.ContainsKey(slaveId))
                    _slaveLedStatus[slaveId][ledId] = CalculateDc14Bit(dc);
            }
        }

        private void Tld7002_Init()
        {
            ResetMrcCount();

            for (var i = 0; i < 5; i++)
                BroadcastSync();

            HWCR_ALL(byte.Parse(0.ToString()));
            Thread.Sleep(10);

            // PM_CHANGE_INIT
            for (var i = 0; i < 2; i++)
                PM_CHANGE(PmChangeCmd.InitMode);
        }

        private void LedControlWork()
        {
            while (_ledControlThread.IsAlive)
            {
                if (!_ledControlThread.IsAlive)
                    break;

                Thread.Sleep(10);

                if (!_isLedControl)
                    continue;

                if (MySerialPort == null)
                    continue;

                lock (_slaveLedStatus)
                {
                    foreach (var t in _slaveLedStatus.Keys)
                        DC_UPDATE_14Bits((byte)t);
                    BroadcastSync();
                }
            }
        }

        private void DC_UPDATE_14Bits(byte address)
        {
            lock (_lockSend)
            {
                var dcUpdateSyncBytes = new List<byte> { SyncByte };

                var mrcBits = _mrcList[_mrcCount];
                var mrcByte = Convert.ToByte("000000" + mrcBits, 2);
                _mrcCount++;
                if (_mrcCount > 3)
                    _mrcCount = 0;

                var dlcBits = _dlcCodeList[16];
                var dlcBytes = Convert.ToByte("00000" + dlcBits, 2);
                var slaveAddrBits = Convert.ToString(address, 2).PadLeft(8, '0').Substring(3, 5);
                var funcByte = Convert.ToByte("00000" + DutyCycleShadowRegisterUpdate, 2);

                var crc3 = GetMasterReqCrc3(address, mrcByte, dlcBytes, funcByte);
                var crc3Bits = Convert.ToString(crc3, 2).PadLeft(8, '0').Substring(5, 3);

                var datas = new List<byte>();
                lock (_slaveLedStatus)
                {
                    if (!_slaveLedStatus.ContainsKey(address))
                        return;

                    var bits = new List<string>();

                    foreach (var valueBits in _slaveLedStatus[address].Keys
                                 .Select(tKey => _slaveLedStatus[address][tKey]).Select(value =>
                                     Convert.ToString(value, 2).PadLeft(16, '0')))
                    {
                        for (var i = 15; i >= 2; i--)
                            bits.Add(valueBits[i].ToString());
                        bits.Add("0");
                        bits.Add("0");
                    }

                    for (var i = 0; i < bits.Count; i = i + 8)
                    {
                        var tempBits = string.Empty;
                        for (var j = i + 7; j >= i; j--)
                            tempBits += bits[j];
                        datas.Add(Convert.ToByte(tempBits, 2));
                    }
                }

                dcUpdateSyncBytes.Add(Convert.ToByte(string.Format("{0}{1}", crc3Bits, slaveAddrBits), 2));
                dcUpdateSyncBytes.Add(Convert.ToByte(string.Format("{0}{1}{2}", mrcBits, dlcBits, DutyCycleShadowRegisterUpdate), 2));

                var toCrc8Data = new List<byte>();
                toCrc8Data.AddRange(datas);
                var crc8 = GetCrc8(toCrc8Data.ToArray(), (byte)toCrc8Data.Count);

                dcUpdateSyncBytes.AddRange(toCrc8Data);
                dcUpdateSyncBytes.Add(crc8);
                MySerialPort.SendCommand(dcUpdateSyncBytes.ToArray());
            }
        }

        private static ushort CalculateDc14Bit(int dc8Bit)
        {
            const double gamma = 0.4545f;
            return (ushort)Math.Round(16383 * Math.Pow(dc8Bit / 255f, 1 / gamma), 0, MidpointRounding.AwayFromZero);
        }

        #endregion
    }
}
