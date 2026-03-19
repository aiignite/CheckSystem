using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Controller
{
    public class V174 : ControllerBase
    {
        private readonly Thread _ledControlTh;
        private bool _isLedControl;
        public CanBus CanBus;

        IniFileHelper iniFile;
        private string _filePath;
        //public byte addr = 0x28;
        List<byte> addrs = new List<byte>();
        private string _0ZONEread;
        private string _0ZONEWrite;
        private string _1ZONEread;
        private string _1ZONEWrite;
        private string _2ZONEread;
        private string _2ZONEWrite;
        private string _CONEread;
        private string _CZONEWrite;
        public string ConfigString = string.Empty;
        public string ReadString = string.Empty;

        public V174(string name)
            : base(name)
        {
            CanBus.PushCanMsg += CanBus_PushCanMsg;
            _ledControlTh = new Thread(LedControl) { IsBackground = true };
            _ledControlTh.Start();
        }

        public void GetV174InifilePath(string filePath)
        {
            _filePath = Directory.GetCurrentDirectory() + @"\FTP配置文件\" + filePath;
            if (!File.Exists(_filePath))
                return;
            iniFile = new IniFileHelper(_filePath);

            _0ZONEread = iniFile.IniReadValue("0ZONE", "Read");
            _0ZONEWrite = iniFile.IniReadValue("0ZONE", "Write");
            _1ZONEread = iniFile.IniReadValue("1ZONE", "Read");
            _1ZONEWrite = iniFile.IniReadValue("1ZONE", "Write");
            _2ZONEread = iniFile.IniReadValue("2ZONE", "Read");
            _2ZONEWrite = iniFile.IniReadValue("2ZONE", "Write");
            _CONEread = iniFile.IniReadValue("CZONE", "Read");
            _CZONEWrite = iniFile.IniReadValue("CZONE", "Write");
            ConfigString = "1,2,C,0ZONEWrite:" + "/" + _1ZONEWrite + "/" + _2ZONEWrite + "/" + _CZONEWrite + "/" + _0ZONEWrite;
        }

        private void CanBus_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (CanBus != null && CanBus.Name == name &&
                (onPushCanDataType == CanBus.OnPushCanDataType.FilterRx || onPushCanDataType == CanBus.OnPushCanDataType.NotFilterRx) &&
                data.CanId != 0x00 && (data.CanDataLen == 16))
            {
                if (_isInFtp)
                    _ftpBuffer.Add(ValueHelper.GetHextStr(data.CanData).Replace(" ", ""));
                Console.WriteLine(@"{0}: {1}", data.CanId, ValueHelper.GetHextStrWithOx(data.CanData));
            }

            if (CanBus != null && CanBus.Name == name &&
                (onPushCanDataType == CanBus.OnPushCanDataType.FilterRx || onPushCanDataType == CanBus.OnPushCanDataType.NotFilterRx) && data.CanId != 0x00)
            {
                //if (_isInFtp)
                //{
                //    _ftpBuffer.Add(ValueHelper.GetHextStr(data.CanData).Replace(" ", ""));
                //}
                Console.WriteLine(@"{0}: {1}", data.CanId, ValueHelper.GetHextStrWithOx(data.CanData));
            }
            //Console.WriteLine("{0}: {1}", data.CanId, ValueHelper.GetHextStrWithOx(data.CanData));
            //throw new NotImplementedException();
        }

        #region 灯板配置

        private static IEnumerable<byte> StrToBytes(string item)
        {
            var str = item.Replace(" ", "");

            var bs = new List<byte>();
            for (var i = 0; i < str.Length; i = i + 2)
            {
                var b = Convert.ToByte(str.Substring(i, 2), 16);
                bs.Add(b);
            }

            return bs;
        }

        private bool _isInFtp;
        private readonly List<string> _ftpBuffer = new List<string>();

        [Description("R,FTP写入")]
        public string FtpConfigWrite;

        [Description("R,FTP配置结果")]
        public string FtpConfigResult;

        [Description("FTP配置-通过硬线拉高CS")]
        public void ChipFTPConfigurationHw(string canId, string isUseCanId)
        {
            ChipFtpConfiguration(canId, isUseCanId, false);
        }

        [Description("FTP配置-通过软件拉高CS")]
        public void ChipFTPConfigurationSw(string canId, string isUseCanId)
        {
            ChipFtpConfiguration(canId, isUseCanId, true);
        }

        private void ChipFtpConfiguration(string canId, string isUseCanId, bool isSwMode)
        {
            if (!File.Exists(_filePath))
            {
                FtpConfigResult = @"配置文件不存在";
                return;
            }
            FtpConfigResult = string.Empty;
            FtpConfigWrite = string.Empty;
            var watchDot = new List<string>
            {
                "19 00 00 00 00",
                "19 00 00 00 01",
            };

            // 解锁
            var unlock = new List<string>
            {
                "AE FF FF FF FF",
                isSwMode ? "18 00 00 00 05" : "18 00 00 00 01",
                "1E 00 00 DC BA",
                "5E 00 00 00 00",
            };

            //FTP地址0
            var ftp0 = new List<string>
            {
                _0ZONEWrite,
                _0ZONEread,

                //iniFile.IniReadValue("0ZONE", "Read"),
                //iniFile.IniReadValue("0ZONE", "Write"),
                // "A0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                //"B0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 "+canId,
                //"A0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
            };

            //FTP地址1
            var ftp1 = new List<string>
            {
                _1ZONEWrite,
                _1ZONEread,

                //iniFile.IniReadValue("1ZONE", "Read"),
                //iniFile.IniReadValue("1ZONE", "Write"),
                // "A1 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                //"B1 00 00 00 00 00 00 00 5B 5B 5B 5B 5B 5B 5B 5B",
                //"A1 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
            };

            //FTP地址2
            var ftp2 = new List<string>
            {
                _2ZONEWrite,
                _2ZONEread,

                //    iniFile.IniReadValue("2ZONE", "Read"),
                //    iniFile.IniReadValue("2ZONE", "Write"),
                ////  "A2 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                // "B2 00 00 00 00 00 00 00 5B 5B 5B 5B 5B 5B 5B 5B",
                // "A2 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
            };

            //FTP地址C
            var ftpc = new List<string>
            {
                _CZONEWrite,
                _CONEread,

                //iniFile.IniReadValue("CZONE", "Write"),
                //iniFile.IniReadValue("CZONE", "Write"),
                // "AC 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                //"BC 00 00 00 00 80 00 21 00 00 00 00 00 00 00 00",
                //"AC 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
            };

            _ftpBuffer.Clear();
            _isInFtp = true;

            foreach (var item in unlock)
            {
                foreach (var item1 in watchDot)
                {
                    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item1));
                    Thread.Sleep(5);
                }
                CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
                Thread.Sleep(5);
            }

            foreach (var item1 in watchDot)
            {
                CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item1));
                Thread.Sleep(5);
            }

            for (var i = 0; i < 2; i++)
            {
                CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(_1ZONEread));
                CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(_2ZONEread));
                CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(_CONEread));
                CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(_0ZONEread));
                //CanBus.SendStandardCanFdData((uint)i, StrToBytes(_0ZONEread));
                Thread.Sleep(2);
            }

            Thread.Sleep(15);

            if (isUseCanId == "0")
            {
                if (_ftpBuffer.Count == 8)
                {
                    if (_ftpBuffer[0] == "00000000000000000000000000000000" &&
                        _ftpBuffer[1] == "00000000000000000000000000000000" &&
                        _ftpBuffer[2] == "00000000000000000000000000000000" &&
                        _ftpBuffer[3] == "00000000000000000000000000000000" &&
                        _ftpBuffer[4] == "00000000000000000000000000000000" &&
                        _ftpBuffer[5] == "00000000000000000000000000000000" &&
                        _ftpBuffer[6] == "00000000000000000000000000000000" &&
                        _ftpBuffer[7] == "00000000000000000000000000000000")
                    {
                        FtpConfigResult = "解锁成功";
                    }
                    else
                    {
                        FtpConfigResult = "芯片错误，非初始状态";
                        //return;
                    }
                }
                else
                {
                    Console.WriteLine(_ftpBuffer.Count.ToString());
                    FtpConfigResult = "解锁失败" + _ftpBuffer.Count;
                    return;
                }
            }
            else
            {
                if (_ftpBuffer.Count == 8)
                {
                    if (_ftpBuffer[0] == "00" + ftp1[0].Replace(" ", "").Substring(2, 30) &&
                        _ftpBuffer[1] == "00" + ftp2[0].Replace(" ", "").Substring(2, 30) &&
                        _ftpBuffer[2] == "00" + ftpc[0].Replace(" ", "").Substring(2, 30) &&
                        _ftpBuffer[3] == "00" + ftp0[0].Replace(" ", "").Substring(2, 30) &&
                        _ftpBuffer[4] == "00" + ftp1[0].Replace(" ", "").Substring(2, 30) &&
                        _ftpBuffer[5] == "00" + ftp2[0].Replace(" ", "").Substring(2, 30) &&
                        _ftpBuffer[6] == "00" + ftpc[0].Replace(" ", "").Substring(2, 30) &&
                        _ftpBuffer[7] == "00" + ftp0[0].Replace(" ", "").Substring(2, 30))
                    {
                        FtpConfigResult = "解锁成功";
                    }
                    else
                    {
                        FtpConfigResult = "芯片错误，非初始状态";
                        //return;
                    }
                }
                else
                {
                    Console.WriteLine(_ftpBuffer.Count.ToString());
                    FtpConfigResult = "解锁失败" + _ftpBuffer.Count;
                    return;
                }
            }

            FtpConfigResult = "NG";
            _ftpBuffer.Clear();
            for (var kk = 0; kk < 3; kk++)
            {
                ReadString = "1,2,C,0ZONERead:";
                _ftpBuffer.Clear();
                Thread.Sleep(10);
                foreach (var item1 in watchDot)
                {
                    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item1));
                    Thread.Sleep(10);
                }
                foreach (var item in ftp1)
                {
                    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
                    Thread.Sleep(10);
                }

                if (_ftpBuffer.Count == 1)
                {
                    ReadString += _ftpBuffer[0];
                    if (_ftpBuffer[0] != "00" + ftp1[0].Replace(" ", "").Substring(2, 30))
                    {
                        FtpConfigResult = "1区刷写失败，请重新刷写";
                        continue;
                    }
                }
                else
                {
                    FtpConfigResult = "1区刷写失败，请重新刷写";
                    continue;
                }
                foreach (var item1 in watchDot)
                {
                    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item1));
                    Thread.Sleep(10);
                }

                foreach (var item in ftp2)
                {
                    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
                    Thread.Sleep(10);
                }

                foreach (var item1 in watchDot)
                {
                    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item1));
                    Thread.Sleep(10);
                }

                if (_ftpBuffer.Count == 2)
                {
                    ReadString += "/" + _ftpBuffer[1];
                    if (_ftpBuffer[1] != "00" + ftp2[0].Replace(" ", "").Substring(2, 30))
                    {
                        FtpConfigResult = "2区刷写失败，请重新刷写";
                        continue;
                    }
                }
                else
                {
                    FtpConfigResult = "2区刷写失败，请重新刷写";
                    continue;
                }
                foreach (var item in ftpc)
                {
                    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
                    Thread.Sleep(10);
                }

                foreach (var item1 in watchDot)
                {
                    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item1));
                    Thread.Sleep(10);
                }

                if (_ftpBuffer.Count == 3)
                {
                    ReadString += "/" + _ftpBuffer[2];
                    if (_ftpBuffer[2] != "00" + ftpc[0].Replace(" ", "").Substring(2, 30))
                    {
                        FtpConfigResult = "C区刷写失败，请重新刷写";
                        continue;
                    }
                }
                else
                {
                    FtpConfigResult = "C区刷写失败，请重新刷写";
                    continue;
                }

                //return;
                foreach (var item in ftp0)
                {
                    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
                    Thread.Sleep(10);
                }

                Thread.Sleep(250);
                _isInFtp = false;

                if (_ftpBuffer.Count == 4)
                {
                    ReadString += "/" + _ftpBuffer[3];
                    if (_ftpBuffer[0] == "00" + ftp1[0].Replace(" ", "").Substring(2, 30) &&
                        _ftpBuffer[1] == "00" + ftp2[0].Replace(" ", "").Substring(2, 30) &&
                        _ftpBuffer[2] == "00" + ftpc[0].Replace(" ", "").Substring(2, 30) &&
                        _ftpBuffer[3] == "00" + ftp0[0].Replace(" ", "").Substring(2, 30))
                    {
                        FtpConfigResult = "OK,";
                        FtpConfigWrite = "OK,";
                        FtpConfigWrite += ConfigString;
                        //FtpConfigResult += " " + ReadString + " " + ConfigString;
                        FtpConfigResult += " " + ReadString;
                        return;
                    }
                }

                foreach (var item1 in watchDot)
                {
                    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item1));
                    Thread.Sleep(10);
                }

                foreach (var item1 in watchDot)
                {
                    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item1));
                    Thread.Sleep(10);
                }

                foreach (var item1 in watchDot)
                {
                    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item1));
                    Thread.Sleep(10);
                }
            }
            FtpConfigResult += " " + ReadString + " " + ConfigString;
            FtpConfigResult = "NG";

            //var ngCount = 0;

            //_ftpBuffer.Clear();
            //_isInFtp = true;
            //foreach (var item in unlock)
            //{
            //    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
            //    Thread.Sleep(10);
            //}
            //foreach (var item in ftp0)
            //{
            //    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
            //    Thread.Sleep(25);
            //}
            //Thread.Sleep(500);
            //_isInFtp = false;
            //if (!_ftpBuffer.Contains("00" + ftp0[1].Replace(" ", "").Substring(2, 30)))
            //    ngCount++;
            //Thread.Sleep(100);

            //_isInFtp = true;
            //_ftpBuffer.Clear();
            //foreach (var item in unlock)
            //{
            //    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
            //    Thread.Sleep(10);
            //}
            //foreach (var item in ftp1)
            //{
            //    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
            //    Thread.Sleep(25);
            //}
            //Thread.Sleep(500);
            //_isInFtp = false;
            //if (!_ftpBuffer.Contains("00" + ftp1[1].Replace(" ", "").Substring(2, 30)))
            //    ngCount++;
            //Thread.Sleep(100);

            //_isInFtp = true;
            //_ftpBuffer.Clear();
            //foreach (var item in unlock)
            //{
            //    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
            //    Thread.Sleep(10);
            //}
            //foreach (var item in ftp2)
            //{
            //    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
            //    Thread.Sleep(50);
            //}
            //Thread.Sleep(500);
            //_isInFtp = false;
            //if (!_ftpBuffer.Contains("00" + ftp2[1].Replace(" ", "").Substring(2, 30)))
            //    ngCount++;
            //Thread.Sleep(100);

            //_isInFtp = true;
            //_ftpBuffer.Clear();
            //foreach (var item in unlock)
            //{
            //    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
            //    Thread.Sleep(10);
            //}
            //foreach (var item in ftpc)
            //{
            //    CanBus.SendStandardCanFdData(isUseCanId == "1" ? Convert.ToByte(canId, 16) : (uint)0x00, StrToBytes(item));
            //    Thread.Sleep(25);
            //}
            //Thread.Sleep(500);
            //_isInFtp = false;
            //if (!_ftpBuffer.Contains("00" + ftpc[1].Replace(" ", "").Substring(2, 30)))
            //    ngCount++;

            //if (ngCount > 0)
            //{
            //    FtpConfigResult = "NG";
            //}
            //else
            //{
            //    FtpConfigResult = "OK";
            //}

            //IsDeblocking = false;
            //if (CanBus == null)
            //    return;
            ////软件配置CS
            //CanBus.SendStandardCanFdData(addrs[0], new byte[] {(byte)0x18, (byte)0x00,(byte)0x00, (byte)0x00, (byte)0x05 } );
            //Thread.Sleep(5);
            //////解锁芯片
            //CanBus.SendStandardCanFdData(addrs[0], new byte[] { (byte)0x1E, (byte)0x00, (byte)0x00, (byte)0xDC, (byte)0xBA });
            //Thread.Sleep(5);
            //CanBus.SendStandardCanFdData(addrs[0], new byte[] { (byte)0x5E, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00 });
            //if(IsDeblocking)
            //{

            //}
            ////配置FTP  
            //CanBus.SendStandardCanFdData(Convert.ToByte(addr, 16), new byte[] { (byte)0xB0, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, Convert.ToByte(addr, 2)});
            //CanBus.SendStandardCanFdData(Convert.ToByte(addr, 16), new byte[] { (byte)0xB1, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x5B, (byte)0x5B, (byte)0x5B, (byte)0x5B, (byte)0x5B, (byte)0x5B, (byte)0x5B, (byte)0x5B });
            //CanBus.SendStandardCanFdData(Convert.ToByte(addr, 16), new byte[] { (byte)0xB2, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x5B, (byte)0x5B, (byte)0x5B, (byte)0x5B, (byte)0x5B, (byte)0x5B, (byte)0x5B, (byte)0x5B });
            //CanBus.SendStandardCanFdData(Convert.ToByte(addr, 16), new byte[] { (byte)0xBC, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x80, (byte)0x00, (byte)0x21, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00 });
        }

        private bool IsInitialState()
        {
            return true;
        }

        private bool IsUnlock()
        {
            return true;
        }

        #endregion

        public void ResetAll()
        {
            var addrs = new List<byte>();
            addrs.Add(0x24);
            addrs.Add(0x20);
            addrs.Add(0x28);
            addrs.Add(0x1C);
            addrs.Add(0x18);
            addrs.Add(0x0C);
            addrs.Add(0x10);
            addrs.Add(0x04);
            addrs.Add(0x08);
            addrs.Add(0x14);

            if (CanBus != null)
            {
                foreach (var canid in addrs) 
                {
                    CanBus.SendStandardCanFdData(canid, StrToBytes("19 00 00 00 00 "));
                    Thread.Sleep(50);
                    CanBus.SendStandardCanFdData(canid, StrToBytes("AE FF FF FF FF "));
                    Thread.Sleep(50);
                    CanBus.SendStandardCanFdData(canid, StrToBytes("19 00 00 00 01 "));
                    Thread.Sleep(50);
                    CanBus.SendStandardCanFdData(canid, StrToBytes("18 00 00 00 05 "));
                    Thread.Sleep(50);
                    CanBus.SendStandardCanFdData(canid, StrToBytes("19 00 00 00 00 "));
                    Thread.Sleep(50);
                    CanBus.SendStandardCanFdData(canid, StrToBytes("1E 00 00 DC BA "));
                    Thread.Sleep(50);
                    CanBus.SendStandardCanFdData(canid, StrToBytes("19 00 00 00 01 "));
                    Thread.Sleep(50);
                    CanBus.SendStandardCanFdData(canid, StrToBytes("5E 00 00 00 00 "));
                    Thread.Sleep(50);
                    CanBus.SendStandardCanFdData(canid, StrToBytes("19 00 00 00 00 "));
                    Thread.Sleep(50);
                    CanBus.SendStandardCanFdData(canid, StrToBytes("B0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 "));
                    Thread.Sleep(50);
                    CanBus.SendStandardCanFdData(canid, StrToBytes("A0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 "));
                    Thread.Sleep(50);
                }
            }
        }

        #region LED控制
        [Description("R/W,LED0")]
        public bool Led0;

        [Description("R/W,LED1")]
        public bool Led1;

        [Description("R/W,LED2")]
        public bool Led2;

        [Description("R/W,LED3")]
        public bool Led3;

        [Description("R/W,LED4")]
        public bool Led4;

        [Description("R/W,LED5")]
        public bool Led5;

        [Description("R/W,LED6")]
        public bool Led6;

        [Description("R/W,LED7")]
        public bool Led7;

        [Description("R/W,LED8")]
        public bool Led8;

        [Description("R/W,LED9")]
        public bool Led9;

        [Description("R/W,LED10")]
        public bool Led10;

        [Description("R/W,LED11")]
        public bool Led11;

        [Description("R/W,LED12")]
        public bool Led12;

        [Description("R/W,LED13")]
        public bool Led13;

        [Description("R/W,LED14")]
        public bool Led14;

        [Description("R/W,LED15")]
        public bool Led15;

        [Description("BOT左打开")]
        public void BOTLeftOn()
        {
            lock (addrs)
            {
                addrs.Add(0x14);
            }
        }

        [Description("BOT左关闭")]
        public void BOTLeftOff()
        {
            lock (addrs)
            {
                addrs.Remove(0x14);
            }
        }

        [Description("BOT右打开")]
        public void BOTRightOn()
        {
            lock (addrs)
            {
                addrs.Add(0x28);
            }
        }

        [Description("BOT右关闭")]
        public void BOTRightOff()
        {
            lock (addrs)
            {
                addrs.Remove(0x28);
            }
        }

        [Description("Master1左打开")]
        public void Master1LeftOn()
        {
            lock (addrs)
            {
                addrs.Add(0x04);
            }
        }

        [Description("Master1左关闭")]
        public void Master1LeftOff()
        {
            lock (addrs)
            {
                addrs.Remove(0x04);
            }
        }

        [Description("Master1右打开")]
        public void Master1RightOn()
        {
            lock (addrs)
            {
                addrs.Add(0x18);
            }
        }

        [Description("Master1右关闭")]
        public void Master1RightOff()
        {
            lock (addrs)
            {
                addrs.Remove(0x18);
            }
        }

        [Description("Master2右打开")]
        public void Master2RightOn()
        {
            lock (addrs)
            {
                addrs.Add(0x1C);
            }
        }

        [Description("Master2右关闭")]
        public void Master2RightOff()
        {
            lock (addrs)
            {
                addrs.Remove(0x1C);
            }
        }

        [Description("Master2左打开")]
        public void Master2LeftOn()
        {
            lock (addrs)
            {
                addrs.Add(0x08);
            }
        }

        [Description("Master2左关闭")]
        public void Master2LeftOff()
        {
            lock (addrs)
            {
                addrs.Remove(0x08);
            }
        }

        [Description("MID右打开")]
        public void MIDRightOn()
        {
            lock (addrs)
            {
                addrs.Add(0x24);
            }
        }

        [Description("MID右关闭")]
        public void MIDRightOff()
        {
            lock (addrs)
            {
                addrs.Remove(0x24);
            }
        }

        [Description("MID左打开")]
        public void MIDLeftOn()
        {
            lock (addrs)
            {
                addrs.Add(0x10);
            }
        }

        [Description("MID左关闭")]
        public void MIDLeftOff()
        {
            lock (addrs)
            {
                addrs.Remove(0x10);
            }
        }

        [Description("TOP右打开")]
        public void TOPRightOn()
        {
            lock (addrs)
            {
                addrs.Add(0x20);
            }
        }

        [Description("TOP右关闭")]
        public void TOPRightOff()
        {
            lock (addrs)
            {
                addrs.Remove(0x20);
            }
        }

        [Description("TOP左打开")]
        public void TOPLeftOn()
        {
            lock (addrs)
            {
                addrs.Add(0x0C);
            }
        }

        [Description("TOP左关闭")]
        public void TOPLeftOff()
        {
            lock (addrs)
            {
                addrs.Remove(0x0C);
            }
        }

        [Description("开启LED控制模式")]
        public void StartLedControl()
        {
            Led0 = false;
            Led1 = false;
            Led2 = false;
            Led3 = false;
            Led4 = false;
            Led5 = false;
            Led6 = false;
            Led7 = false;
            Led8 = false;
            Led9 = false;
            Led10 = false;
            Led11 = false;
            Led12 = false;
            Led13 = false;
            Led14 = false;
            Led15 = false;
            _isLedControl = true;
        }

        [Description("关闭LED控制模式")]
        public void StopLedControl()
        {
            Led0 = false;
            Led1 = false;
            Led2 = false;
            Led3 = false;
            Led4 = false;
            Led5 = false;
            Led6 = false;
            Led7 = false;
            Led8 = false;
            Led9 = false;
            Led10 = false;
            Led11 = false;
            Led12 = false;
            Led13 = false;
            Led14 = false;
            Led15 = false;
            _isLedControl = false;
        }

        [Description("打开所有双数LED")]
        public void LedEvenOn()
        {
            if (!_isLedControl)
                return;
            Led0 = true;
            Led2 = true;
            Led4 = true;
            Led6 = true;
            Led8 = true;
            Led10 = true;
            Led12 = true;
            Led14 = true;
            Led1 = false;
            Led3 = false;
            Led5 = false;
            Led7 = false;
            Led9 = false;
            Led11 = false;
            Led13 = false;
            Led15 = false;
        }

        [Description("地址验证")]
        public void LedEvenOnConfirmation()
        {
            if (!_isLedControl)
                return;
            Led1 = true;
        }

        [Description("添加芯片地址")]
        public void AddAddra(string Addr)
        {
            lock (addrs)
            {
                byte addr = Convert.ToByte(Addr, 16);
                addrs.Add(addr);
            }
        }

        [Description("移除芯片地址")]
        public void DeleteAddra(string Addr)
        {
            lock (addrs)
            {
                var addr = Convert.ToByte(Addr, 16);
                addrs.Remove(addr);
            }
        }

        [Description("打开所有单数LED")]
        public void LedOddOn()
        {
            if (!_isLedControl)
                return;
            Led0 = false;
            Led2 = false;
            Led4 = false;
            Led6 = false;
            Led8 = false;
            Led10 = false;
            Led12 = false;
            Led14 = false;
            Led1 = true;
            Led3 = true;
            Led5 = true;
            Led7 = true;
            Led9 = true;
            Led11 = true;
            Led13 = true;
            Led15 = true;
        }

        [Description("打开所有LED")]
        public void LedAllOn()
        {
            if (!_isLedControl)
                return;
            Led0 = true;
            Led2 = true;
            Led4 = true;
            Led6 = true;
            Led8 = true;
            Led10 = true;
            Led12 = true;
            Led14 = true;
            Led1 = true;
            Led3 = true;
            Led5 = true;
            Led7 = true;
            Led9 = true;
            Led11 = true;
            Led13 = true;
            Led15 = true;
        }

        [Description("关闭所有LED")]
        public void LedAffOff()
        {
            if (!_isLedControl)
                return;
            Led0 = false;
            Led11 = false;
            Led7 = false;
            Led3 = false;
            Led1 = false;
            Led2 = false;
            Led4 = false;
            Led5 = false;
            Led6 = false;
            Led8 = false;
            Led9 = false;
            Led10 = false;
            Led12 = false;
            Led13 = false;
            Led14 = false;
            Led15 = false;
        }


        private bool _IsAwake;

        public void Awake()
        {
            _IsAwake = true;
        }
        public void Sleep()
        {
            _IsAwake = false;
        }

        private void LedControl()
        {
            while (_ledControlTh.IsAlive)
            {
                if (!_ledControlTh.IsAlive)
                    break;

                Thread.Sleep(2);
                //if (!_IsAwake)
                //{
                //    break;
                //}

                if (CanBus == null)
                    continue;
                lock (addrs)
                {
                    foreach (var addr in addrs)
                    {
                        //看门狗重置
                        CanBus.SendStandardCanFdData(addr, new byte[] { 0x19, 0x00, 0x00, 0x00, 0x00 });
                        Thread.Sleep(5);
                        CanBus.SendStandardCanFdData(addr, new byte[] { 0x19, 0x00, 0x00, 0x00, 0x01 });
                        Thread.Sleep(5);

                        if (!_isLedControl)
                            continue;

                        CanBus.SendStandardCanFdData(addr, new byte[] { 0xAE, 0xFF, 0xFF, 0xFF, 0xFF });
                        Thread.Sleep(5);
                        CanBus.SendStandardCanFdData(addr, new byte[] { 0x18, 0x00, 0x00, 0x00, 0x01 });
                        //02~05 LED点灯
                        //Led3 ? "0xFF" : "0x00", Led2 ? "0xFF" : "0x00", Led1 ? "0xFF" : "0x00");
                        CanBus.SendStandardCanFdData(addr, new byte[] { 0x02, (byte)(Led3 ? 0xFF : 0x00), (byte)(Led2 ? 0xFF : 0x00), (byte)(Led1 ? 0xFF : 0x00), (byte)(Led0 ? 0xFF : 0x00) });

                        CanBus.SendStandardCanFdData(addr, new byte[] { 0x03, (byte)(Led7 ? 0xFF : 0x00), (byte)(Led6 ? 0xFF : 0x00), (byte)(Led5 ? 0xFF : 0x00), (byte)(Led4 ? 0xFF : 0x00) });

                        CanBus.SendStandardCanFdData(addr, new byte[] { 0x04, (byte)(Led11 ? 0xFF : 0x00), (byte)(Led10 ? 0xFF : 0x00), (byte)(Led9 ? 0xFF : 0x00), (byte)(Led8 ? 0xFF : 0x00) });

                        CanBus.SendStandardCanFdData(addr, new byte[] { 0x05, (byte)(Led15 ? 0xFF : 0x00), (byte)(Led14 ? 0xFF : 0x00), (byte)(Led13 ? 0xFF : 0x00), (byte)(Led12 ? 0xFF : 0x00) });

                    }
                }
            }
        }
        #endregion
    }
}
