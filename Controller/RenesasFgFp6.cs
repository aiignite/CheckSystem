using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class RenesasFgFp6 : ControllerBase
    {
        #region public fields

        [Description("R,烧写结果")]
        public string ProgResult;

        [Description("R,FlashMemory")]
        public string FlashMemory;

        [Description("R,OptionBytes")]
        public string OptionBytes;

        [Description("R,ICU-S")]
        public string IcuSStatus;

        [Description("R,写ID-Code结果")]
        public string SetIdCodeResult;

        [Description("R/W,需要写入的ID-Code")]
        public string IdCodeToSet;

        [Description("R,读取的ID-Code")]
        public string IdCodeFromGet;

        [Description("R,读取与写入的ID-Code对比")]
        public string IdCodeCompare;

        [Description("R/W,Authentication ID")]
        public string AuthId;

        [Description("R/W,UniqueCode起始地址")]
        public string UniqueCodeStartAddr;

        [Description("R/W,UniqueCode16进制参数")]
        public string UniqueCodeHexPattern;

        #endregion

        private readonly object _lock = new object();
        private MySerialPort _mySerialPort;
        public RenesasFgFp6(string name)
            : base(name)
        {

        }

        public void ConnectFp6(string portBaudTate)
        {
            var port = portBaudTate.Split(':')[0];
            var baudTate = portBaudTate.Split(':')[1];
            _mySerialPort = new MySerialPort(port.ToLower(), Convert.ToInt32(baudTate), Parity.None, 8, StopBits.One);
            _mySerialPort.MyOpen();
        }

        [Description("切换烧写区域")]
        public void ConnectDevice()
        {

            if (_mySerialPort == null)
                return;

            try
            {
                _mySerialPort.ReadDataStr();
                _mySerialPort.ClearBuff();
                _mySerialPort.Write("con\r\n");
                var readStr = _mySerialPort.ReadDataStr();
                if (readStr != null)
                {
                    Console.WriteLine(readStr);
                }
                Thread.Sleep(50);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("切换烧写区域")]
        public void ChangeProgArea(string index)
        {
            if (index != "0" &&
                index != "1" &&
                index != "2" &&
                index != "3" &&
                index != "4" &&
                index != "5" &&
                index != "6" &&
                index != "7")
                return;

            if (_mySerialPort == null)
                return;

            try
            {
                _mySerialPort.ReadDataStr();
                _mySerialPort.ClearBuff();
                _mySerialPort.Write(string.Format("progarea {0}\r\n", int.Parse(index)));
                var readStr = _mySerialPort.ReadDataStr();
                if (readStr != null)
                {
                    Console.WriteLine(readStr);
                }
                Thread.Sleep(50);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("SetUniqueCode")]
        public void SetUniqueCode()
        {
            if (_mySerialPort == null || string.IsNullOrEmpty(UniqueCodeStartAddr) || string.IsNullOrEmpty(UniqueCodeHexPattern))
                return;

            try
            {
                var str = string.Format("serno {0} {1}\r\n", UniqueCodeStartAddr, UniqueCodeHexPattern);
                _mySerialPort.ReadDataStr();
                _mySerialPort.ClearBuff();
                _mySerialPort.Write(str);
                var readStr = _mySerialPort.ReadDataStr();
                if (readStr != null)
                {
                    Console.WriteLine(readStr);
                }
                Thread.Sleep(50);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("擦除程序后烧写")]
        public void EraseAndProgram(string timeOutMs)
        {
            ProgResult = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - 下载中";

            if (_mySerialPort == null)
                return;

            try
            {
                _mySerialPort.ReadDataStr();
                _mySerialPort.ClearBuff();
                _mySerialPort.Write("ep\r\n");
                var tempStr = string.Empty;
                const int timePeriod = 50;
                int timeOutValue;
                if (!string.IsNullOrEmpty(timeOutMs) && int.TryParse(timeOutMs, out timeOutValue))
                {

                }
                else
                {
                    timeOutValue = 10 * 1000;
                }

                //var count = 0;
                var timeStart = DateTime.Now;
                string pmResult;

                while (true)
                {
                    Thread.Sleep(timePeriod);

                    var readStr = _mySerialPort.ReadDataStr();
                    tempStr += readStr;

                    if (tempStr.EndsWith(">"))
                    {
                        pmResult = tempStr;
                        break;
                    }

                    var timeEnd = DateTime.Now;
                    var timeMs = ValueHelper.GetTimeSpanMs(timeStart, timeEnd);

                    if (timeMs > timeOutValue)
                    {
                        ProgResult = "NG";
                        return;
                    }
                }

                pmResult = pmResult.Replace("\r\n", "");
                if (pmResult.EndsWith("Erase,Program operation finished.>") && !pmResult.Contains("ERROR"))
                {
                    ProgResult = "OK";
                }
                else
                {
                    var findIndexStart = pmResult.IndexOf("ERROR", StringComparison.Ordinal);
                    var findIndexEnd = pmResult.IndexOf(">", StringComparison.Ordinal);
                    ProgResult = "NG " + pmResult.Substring(findIndexStart, findIndexEnd - findIndexStart);
                }
            }
            catch (Exception ex)
            {
                ProgResult = "NG " + ex.Message;
            }

            Thread.Sleep(250);
        }

        [Description("SetFlashOption")]
        public void SetFlashOption()
        {
            if (_mySerialPort == null)
                return;

            try
            {
                const string str = "pfo\r\n";
                _mySerialPort.ReadDataStr();
                _mySerialPort.ClearBuff();
                _mySerialPort.Write(str);
                var readStr = _mySerialPort.ReadDataStr();
                if (readStr != null)
                {
                    Console.WriteLine(readStr);
                }
                Thread.Sleep(50);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("GetOptionBytes")]
        public void GetOptionBytes()
        {
            OptionBytes = string.Empty;

            if (_mySerialPort == null)
                return;

            try
            {
                var timeOutMs = 5000.ToString();
                _mySerialPort.ReadDataStr();
                _mySerialPort.ClearBuff();
                _mySerialPort.Write("gob\r\n");
                var tempStr = string.Empty;
                const int timePeriod = 50;
                int timeOutValue;
                if (!string.IsNullOrEmpty(timeOutMs) && int.TryParse(timeOutMs, out timeOutValue))
                {

                }
                else
                {
                    timeOutValue = 10 * 1000;
                }

                //var count = 0;
                var timeStart = DateTime.Now;
                string pmResult;

                while (true)
                {
                    Thread.Sleep(timePeriod);

                    var readStr = _mySerialPort.ReadDataStr();
                    tempStr += readStr;

                    if (tempStr.EndsWith(">"))
                    {
                        pmResult = tempStr;
                        break;
                    }

                    var timeEnd = DateTime.Now;
                    var timeMs = ValueHelper.GetTimeSpanMs(timeStart, timeEnd);

                    if (timeMs > timeOutValue)
                    {
                        return;
                    }
                }

                pmResult = pmResult.Replace("\r\n", "");

                var startIndex = pmResult.IndexOf("Option Bytes :", StringComparison.Ordinal);
                var endIndex = pmResult.IndexOf("PASS", StringComparison.Ordinal);
                OptionBytes = pmResult.Substring(startIndex, endIndex - startIndex);
            }
            catch (Exception ex)
            {
                OptionBytes = ex.Message;
            }
        }

        [Description("根据UniqueCode读FlashMemory")]
        public void ReadFlashMemoryViaUniqueCode()
        {
            FlashMemory = string.Empty;

            if (_mySerialPort == null || string.IsNullOrEmpty(UniqueCodeStartAddr) || string.IsNullOrEmpty(UniqueCodeHexPattern))
                return;

            var startAddress = UniqueCodeStartAddr;
            var endAddress = string.Empty;

            var dds = new List<byte>();
            for (var i = 0; i < startAddress.Length; i = i + 2)
                dds.Add(Convert.ToByte(startAddress.Substring(i, 2), 16));
            var dex = (uint)(ValueHelper.GetDecimal(dds.ToArray()));

            var dex2 = (uint)(dex + UniqueCodeHexPattern.Length / 2 - 1);
            endAddress = BitConverter.GetBytes(dex2).Reverse().ToArray().Aggregate(endAddress, (current, t) => current + ValueHelper.GetHextStr(t));

            try
            {
                var timeOutMs = 5000.ToString();
                _mySerialPort.ReadDataStr();
                _mySerialPort.ClearBuff();
                var cmd = string.Format("read {0} {1}\r\n", startAddress, endAddress);
                _mySerialPort.Write(cmd);
                var tempStr = string.Empty;
                const int timePeriod = 50;
                int timeOutValue;
                if (!string.IsNullOrEmpty(timeOutMs) && int.TryParse(timeOutMs, out timeOutValue))
                {

                }
                else
                {
                    timeOutValue = 10 * 1000;
                }

                //var count = 0;
                var timeStart = DateTime.Now;
                string pmResult;

                while (true)
                {
                    Thread.Sleep(timePeriod);

                    var readStr = _mySerialPort.ReadDataStr();
                    tempStr += readStr;

                    if (tempStr.EndsWith(">"))
                    {
                        pmResult = tempStr;
                        break;
                    }

                    var timeEnd = DateTime.Now;
                    var timeMs = ValueHelper.GetTimeSpanMs(timeStart, timeEnd);

                    if (timeMs > timeOutValue)
                    {
                        return;
                    }
                }

                pmResult = pmResult.Replace("\r\n", "");

                var startIndex = pmResult.IndexOf(startAddress + ":", StringComparison.Ordinal);
                var endIndex = pmResult.IndexOf("PASS", StringComparison.Ordinal);
                var flashMemory = pmResult.Substring(startIndex, endIndex - startIndex).Substring(10).Replace(" ", "");
                if (UniqueCodeHexPattern == flashMemory)
                {
                    FlashMemory = "OK " + flashMemory;
                }
                else
                {
                    FlashMemory = "NG " + flashMemory;
                }
            }
            catch (Exception ex)
            {
                FlashMemory = "NG " + ex.Message;
            }
        }

        [Description("ReadFlashMemory")]
        public void ReadFlashMemory(string startAddress, string endAddress)
        {
            FlashMemory = string.Empty;

            if (_mySerialPort == null)
                return;

            try
            {
                var timeOutMs = 5000.ToString();
                _mySerialPort.ReadDataStr();
                _mySerialPort.ClearBuff();
                var cmd = string.Format("read {0} {1}\r\n", startAddress, endAddress);
                _mySerialPort.Write(cmd);
                var tempStr = string.Empty;
                const int timePeriod = 50;
                int timeOutValue;
                if (!string.IsNullOrEmpty(timeOutMs) && int.TryParse(timeOutMs, out timeOutValue))
                {

                }
                else
                {
                    timeOutValue = 10 * 1000;
                }

                //var count = 0;
                var timeStart = DateTime.Now;
                string pmResult;

                while (true)
                {
                    Thread.Sleep(timePeriod);

                    var readStr = _mySerialPort.ReadDataStr();
                    tempStr += readStr;

                    if (tempStr.EndsWith(">"))
                    {
                        pmResult = tempStr;
                        break;
                    }

                    var timeEnd = DateTime.Now;
                    var timeMs = ValueHelper.GetTimeSpanMs(timeStart, timeEnd);

                    if (timeMs > timeOutValue)
                    {
                        return;
                    }
                }

                pmResult = pmResult.Replace("\r\n", "");

                var startIndex = pmResult.IndexOf(startAddress + ":", StringComparison.Ordinal);
                var endIndex = pmResult.IndexOf("PASS", StringComparison.Ordinal);
                FlashMemory = pmResult.Substring(startIndex, endIndex - startIndex);
            }
            catch (Exception ex)
            {
                FlashMemory = ex.Message;
            }
        }

        [Description("写ID-Code")]
        public void SetIdCode()
        {
            SetIdCodeResult = string.Empty;

            if (_mySerialPort == null || string.IsNullOrEmpty(IdCodeToSet) || IdCodeToSet.Length % 2 != 0)
                return;

            try
            {
                var timeOutMs = 5000.ToString();

                _mySerialPort.ReadDataStr();
                _mySerialPort.ClearBuff();
                _mySerialPort.Write("scf\r\n");
                Thread.Sleep(200);

                _mySerialPort.ReadDataStr();
                _mySerialPort.ClearBuff();
                var str = string.Format("sid {0}\r\n", IdCodeToSet);
                _mySerialPort.Write(str);

                var tempStr = string.Empty;
                const int timePeriod = 50;
                int timeOutValue;
                if (!string.IsNullOrEmpty(timeOutMs) && int.TryParse(timeOutMs, out timeOutValue))
                {

                }
                else
                {
                    timeOutValue = 10 * 1000;
                }

                //var count = 0;
                var timeStart = DateTime.Now;
                string pmResult;

                while (true)
                {
                    Thread.Sleep(timePeriod);

                    var readStr = _mySerialPort.ReadDataStr();
                    tempStr += readStr;

                    if (tempStr.EndsWith(">"))
                    {
                        pmResult = tempStr;
                        break;
                    }

                    var timeEnd = DateTime.Now;
                    var timeMs = ValueHelper.GetTimeSpanMs(timeStart, timeEnd);

                    if (timeMs > timeOutValue)
                    {
                        return;
                    }
                }

                pmResult = pmResult.Replace("\r\n", "");

                if (pmResult.EndsWith(string.Format("sid {0}Set Serial Programming ID PASSSerial Progr. ID operation finished.>", IdCodeToSet)))
                {
                    SetIdCodeResult = "OK";
                }
                else
                {
                    SetIdCodeResult = "NG";
                }
            }
            catch (Exception)
            {
                SetIdCodeResult = "NG";
            }
        }

        ////[Description("读ID-Code并与写入值相比较")]
        ////public void GetIdCode()
        ////{
        ////    IdCodeFromGet = string.Empty;
        ////    IdCodeCompare = string.Empty;

        ////    if (_mySerialPort == null)
        ////        return;

        ////    try
        ////    {
        ////        const string str = "gid\r\n";
        ////        _mySerialPort.ReadDataStr();
        ////        _mySerialPort.ClearBuff();
        ////        _mySerialPort.Write(str);
        ////        Thread.Sleep(250);
        ////        var readStr = _mySerialPort.ReadDataStr();

        ////        //var readStr =
        ////        //    "gid\r\nID code : FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF\r\nPASS\r\nID code operation finished.\r\n>";
        ////        if (!string.IsNullOrEmpty(readStr) && readStr.StartsWith("gid") && readStr.EndsWith(">"))
        ////        {
        ////            var sp = readStr.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        ////            if (sp.Length == 5 && sp[1].StartsWith("ID code : "))
        ////            {
        ////                var temp = sp[1].Replace("ID code : ", "");
        ////                IdCodeFromGet = temp;
        ////            }
        ////        }

        ////        if (string.IsNullOrEmpty(IdCodeToSet))
        ////            IdCodeCompare = "NG";
        ////        else
        ////            IdCodeCompare = IdCodeFromGet == IdCodeToSet ? "OK" : "NG";
        ////    }
        ////    catch (Exception)
        ////    {
        ////        IdCodeFromGet = string.Empty;
        ////        IdCodeCompare = string.Empty;
        ////    }
        ////}

        [Description("ChangeAuthenticationIdCode")]
        public void ChangeAuthId()
        {
            if (_mySerialPort == null || string.IsNullOrEmpty(AuthId) || AuthId.Length % 2 != 0)
                return;

            try
            {
                var str = string.Format("set_auth_id id {0}\r\n", AuthId);
                _mySerialPort.Write(str);
                Thread.Sleep(250);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("Read ICU-S")]
        public void ReadIcuS()
        {
            IcuSStatus = string.Empty;

            if (_mySerialPort == null)
                return;

            try
            {
                var timeOutMs = 5000.ToString();
                _mySerialPort.ReadDataStr();
                _mySerialPort.ClearBuff();
                _mySerialPort.Write("gos\r\n");
                var tempStr = string.Empty;
                const int timePeriod = 50;
                int timeOutValue;
                if (!string.IsNullOrEmpty(timeOutMs) && int.TryParse(timeOutMs, out timeOutValue))
                {

                }
                else
                {
                    timeOutValue = 10 * 1000;
                }

                //var count = 0;
                var timeStart = DateTime.Now;
                string pmResult;

                while (true)
                {
                    Thread.Sleep(timePeriod);

                    var readStr = _mySerialPort.ReadDataStr();
                    tempStr += readStr;

                    if (tempStr.EndsWith(">"))
                    {
                        pmResult = tempStr;
                        break;
                    }

                    var timeEnd = DateTime.Now;
                    var timeMs = ValueHelper.GetTimeSpanMs(timeStart, timeEnd);

                    if (timeMs > timeOutValue)
                    {
                        return;
                    }
                }

                var sp = pmResult.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (!sp.Any())
                    return;
                if (sp.ToList().FindIndex(f => f.StartsWith("PASS")) == -1)
                    return;
                var icuSIndex = sp.ToList().FindIndex(f => f.StartsWith("ICU-S"));
                if (icuSIndex != -1)
                {
                    var icusSp = sp[icuSIndex].Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    IcuSStatus = icusSp[1].Trim();
                }
            }
            catch (Exception ex)
            {
                IcuSStatus = ex.Message;
            }
        }

        #region 上传文件

        /// <summary>
        /// 参数文件路径,需全英文路径
        /// </summary>
        [Description("R,参数文件路径,需全英文路径")]
        public string PrmFile = @"G:\PgFp6\PLG458_Merge\R7F701581.pr6";
        [Description("R,设置文件路径,需全英文路径")]
        public string SetFile = @"G:\PgFp6\PLG458_Merge\PLG458_Merge.es6";
        [Description("R,烧写文件路径,需全英文路径")]
        public string LodFile = @"G:\PgFp6\358l_PLG_48V_25_56_AC_20230725.mot";
        [Description("R,烧写结果")]
        public string LoadFileResult = "NG";

        private readonly SerialPort _teraTermComPort = new SerialPort();
        private string _teraTermCom = string.Empty;

        public void InitTeraTermComPort(string com)
        {
            if (com.StartsWith("COM"))
            {
                _teraTermCom = com;
                _teraTermComPort.PortName = _teraTermCom;
            }
            else
            {
                OnPushControllerMsg(string.Format("瑞萨RENESAS烧写器串口格式错误,当前为{0},应为例如:COM1", com));
            }
        }

        public void UploadFile(string index)
        {
            if (index != "0" &&
                index != "1" &&
                index != "2" &&
                index != "3" &&
                index != "4" &&
                index != "5" &&
                index != "6" &&
                index != "7")
            {
                LoadFileResult = "NG:请输入正确的ProgrammingArea索引";
                return;
            }

            //查找下载文件的进程是否打开，如果打开了直接关闭
            var myProcesses = Process.GetProcesses();
            foreach (var myProcess in myProcesses.Where(myProcess => myProcess.ProcessName.Contains("ttermpro") || myProcess.ProcessName.Contains("ttpmacro")))
            {
                myProcess.Kill();
            }

            LoadFileResult = "NG";
            if (!File.Exists(@"teraterm\loadFileBase.ttl"))
            {
                LoadFileResult = "NG:脚本基础文件不存在";
                return;
            }

            var loadCommand = File.ReadAllText(@"teraterm\loadFileBase.ttl");
            var com = _teraTermCom.Replace("COM", "");
            var setFileSplitArr = SetFile.Split(new[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
            string setName;
            if (setFileSplitArr.Length > 2)
            {
                setName = setFileSplitArr[setFileSplitArr.Length - 1];
            }
            else
            {
                LoadFileResult = "NG:setFile路径异常";
                return;
            }

            var lodFileSplitArr = LodFile.Split(new[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
            var lodName = string.Empty;
            if (lodFileSplitArr.Length > 2)
            {
                lodName = lodFileSplitArr[lodFileSplitArr.Length - 1];
            }
            else
            {
                LoadFileResult = "NG:lodFile路径异常";
            }
            loadCommand = loadCommand.Replace("#COM#", com);
            loadCommand = loadCommand.Replace("#prmFile#", PrmFile);
            loadCommand = loadCommand.Replace("#setName#", setName);
            loadCommand = loadCommand.Replace("#setFile#", SetFile);
            loadCommand = loadCommand.Replace("#time#", DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
            loadCommand = loadCommand.Replace("#lodName#", lodName);
            loadCommand = loadCommand.Replace("#lodFile#", LodFile);

            File.WriteAllText(@"teraterm\loadFileTemp.ttl", loadCommand);

            if (!_teraTermComPort.IsOpen)
            {
                try
                {
                    _teraTermComPort.Open();
                }
                catch (Exception ex)
                {
                    LoadFileResult = "NG:串口打开失败," + ex.Message;
                    return;
                    //OnPushControllerMsg(string.Format("串口打开失败：{0}",  ex.Message));
                }
            }

            var fileInfo = new FileInfo(LodFile);

            var checkLoadFileName = fileInfo.Name.ToLower().Replace(".mot", "").Replace(".s19", "").Replace(".hex", "");
            if (checkLoadFileName.Length > 30)
                checkLoadFileName = checkLoadFileName.Substring(0, 30);

            var checkLoadName = string.Format("{0}  {1}", index, checkLoadFileName);

            _teraTermComPort.Write("files\r\n");
            Thread.Sleep(50);
            var fileMsg = _teraTermComPort.ReadExisting();
            if (fileMsg.ToLower().Contains(checkLoadName))
            {
                LoadFileResult = "OK";
                _teraTermComPort.Close();
                Thread.Sleep(25);
                return;
            }

            _teraTermComPort.Write(string.Format("progarea {0}\r\n", int.Parse(index)));
            Thread.Sleep(850);
            var readPmArea = _teraTermComPort.ReadExisting();
            if (!readPmArea.ToLower().Contains(string.Format("Active Program Area: {0}", index).ToLower()))
            {
                LoadFileResult = "NG:ProgrammingArea切换失败，无法上传";
                _teraTermComPort.Close();
                return;
            }

            _teraTermComPort.Close();
            Thread.Sleep(25);
            var cmdResult = MacAddressHelper.Cmd.RunCmd(string.Format(@"{0}\teraterm\ttpmacro.exe {1}\teraterm\loadFileTemp.ttl OFF", Environment.CurrentDirectory, Environment.CurrentDirectory));

            myProcesses = Process.GetProcesses();
            foreach (var myProcess in myProcesses.Where(myProcess => myProcess.ProcessName.Contains("ttermpro") || myProcess.ProcessName.Contains("ttpmacro")))
            {
                try
                {
                    myProcess.Kill();
                }
                catch (Exception)
                {
                    //ignor
                }
            }
            Thread.Sleep(1500);
            _teraTermComPort.Open();
            _teraTermComPort.Write("files\r\n");
            Thread.Sleep(20);
            fileMsg = _teraTermComPort.ReadExisting();
            _teraTermComPort.Close();
            Thread.Sleep(25);

            if (fileMsg.ToLower().Contains(checkLoadName))
            {
                LoadFileResult = "OK";
            }
        }

        #endregion
    }
}
