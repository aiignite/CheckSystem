using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,SolenoidValueAppDownload")]
    public sealed class SolenoidValueAppDownload : ControllerBase
    {
        public LinBus Lin;

        #region 刷新

        [Description("R,下载结果")]
        public string DownloadResult = string.Empty;

        [Description("R,下载耗时-秒")]
        public float DownloadCostTime;

        [Description("R/W,APP文件路径")]
        public string AppFilePath =
            //string.Empty;
            @"E:\Projects\电磁阀\T010_350mA.sx";

        [Description("R/W,Cal文件路径")]
        public string CalFilePath =
            //string.Empty;
            @"E:\Projects\电磁阀\cal.sx";

        private byte _commandLinId = 0x1B;
        private byte _responseLinId = 0x1C;
        private static readonly object AppFileLocker = new object();

        public SolenoidValueAppDownload(string name)
            : base(name) { }

        ~SolenoidValueAppDownload()
        {
            Dispose();
        }

        [Description("设置命令帧LinID")]
        public void SetCommandLinId(string lindId)
        {
            try
            {
                _commandLinId = Convert.ToByte(lindId, 16);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("设置响应帧LinID")]
        public void SetResponseLinId(string lindId)
        {
            try
            {
                _responseLinId = Convert.ToByte(lindId, 16);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("开始APP下载")]
        public void StartAppDownload()
        {
            DownloadCostTime = 0;
            var st = new Stopwatch();
            st.Start();

            FlashHandler();
            st.Stop();

            DownloadCostTime = st.ElapsedMilliseconds / 1000f;
        }

        private void FlashHandler()
        {
            DownloadResult = "NG";

            if (Lin == null)
                return;

            S19LineData[] s19LineDatas;
            S19LineData[] calS19LineDatas;

            lock (AppFileLocker)
            {
                if (string.IsNullOrEmpty(AppFilePath))
                    return;

                if (!File.Exists(AppFilePath))
                    return;

                s19LineDatas = GetS19LineData(AppFilePath).ToArray();
            }

            lock (AppFileLocker)
            {
                if (string.IsNullOrEmpty(CalFilePath))
                    return;

                if (!File.Exists(CalFilePath))
                    return;

                calS19LineDatas = GetS19LineData(CalFilePath).ToArray();
            }

            if (s19LineDatas.Length <= 0)
            {
                DownloadResult += " 未选择下载APP文件";
                return;
            }
            if (calS19LineDatas.Length <= 0)
            {
                DownloadResult += " 未选择下载Cal文件";
                return;
            }

            if (LinAppDownLoadVersion2(s19LineDatas))
            {
                if (LinCalDownLoad(calS19LineDatas))
                {
                    var exitBootResult = GetLinRecvMsg(new byte[] { 0x02, 0x18, 0x19, 0x20, 0x21, 0x00, 0x00, 0x00 },
                        _commandLinId, _responseLinId, 100, isNeedRetry: false);
                    if (!exitBootResult.StartsWith("02FF"))
                        DownloadResult += " EXIT BOOT FAILED";
                    else
                        DownloadResult = "OK";
                }
                else
                    DownloadResult += " CAL文件下载失败";
            }
            else
                DownloadResult += " APP文件下载失败";
        }

        private bool LinAppDownLoadVersion2(
            IEnumerable<S19LineData> listLineData)
        {
            if (!ConnectStatusCheck(10, _commandLinId, _responseLinId))
                return false;

            var lineDatas = listLineData as S19LineData[] ?? listLineData.ToArray();
            var blocks = GetBlocks(lineDatas);
            if (blocks.Count != 1)
                return false;

            var count = blocks.SelectMany(b => b).Count();
            byte numberHigh;
            byte numberLow;
            if (count >= 255)
            {
                numberHigh = (byte)((count & 0xff00) >> 8);
                numberLow = (byte)(count & 0xff);
            }
            else
            {
                numberHigh = (byte)count;
                numberLow = 0x00;
            }

            // Flash Erase Request
            var sendData = new byte[]
            {
                0x03, 0x26, 0x27, 0x28, 0x29, numberHigh, numberLow, 0x00
            };
            var flashEraseRequest = GetLinRecvMsg(sendData, _commandLinId, _responseLinId, 2500, false);
            if (!flashEraseRequest.StartsWith("03FF"))
                return false;

            foreach (var b in blocks)
            {
                foreach (var itemB in b)
                {
                    var tempXorcs = itemB.Data.Aggregate((byte)0x00, (current, data) => (byte)(current + data));
                    var mChksum = (byte)(tempXorcs & 0xFF);

                    var byte0 = BitConverter.GetBytes(itemB.Address).Reverse().ToList()[1];
                    var byte1 = BitConverter.GetBytes(itemB.Address).Reverse().ToList()[2];
                    var byte2 = BitConverter.GetBytes(itemB.Address).Reverse().ToList()[3];
                    var byte4 = itemB.DataLen;

                    // 0x04
                    var result04 = GetLinRecvMsg(new byte[] { 0x04, byte0, byte1, byte2, byte4, mChksum, 0x00, 0x00 }, _commandLinId, _responseLinId, isNeedRetry: false);
                    if (!result04.StartsWith("04FF"))
                        return false;

                    var rest06Datas = itemB.DataLen % 6;
                    var toSendData = new List<byte>();
                    toSendData.AddRange(itemB.Data);
                    if (rest06Datas > 0)
                        for (var i = 0; i < 6 - rest06Datas; i++)
                            toSendData.Add(0x00);

                    var seq = (byte)0x00;
                    for (var i = 0; i < toSendData.Count; i += 6)
                    {
                        Lin.SendMasterLin(_commandLinId, new byte[] { 0x06, seq, toSendData[i], toSendData[i + 1], toSendData[i + 2], toSendData[i + 3], toSendData[i + 4], toSendData[i + 5] });
                        seq++;
                    }

                    byte[] result06Bytes;
                    Lin.SendSlaveLin(_responseLinId, out result06Bytes, timeOutMs: 1500);
                    var result06 = ValueHelper.GetHextStr(result06Bytes).Replace(" ", "");
                    if (!result06.StartsWith("06FF"))
                        return false;
                }
            }

            var result07 = GetLinRecvMsg(new byte[] { 0x07, 0x50, 0x51, 0x52, 0x53, 0x00, 0x00, 0x00 }, _commandLinId, _responseLinId, isNeedRetry: false);
            if (!result07.StartsWith("07FF"))
                return false;

            return true;
        }

        private bool LinCalDownLoad(IEnumerable<S19LineData> listLineData)
        {
            var lineDatas = listLineData as S19LineData[] ?? listLineData.ToArray();
            var blocks = GetBlocks(lineDatas);
            var count = blocks.SelectMany(b => b).Count();
            byte numberHigh;
            byte numberLow;
            if (count >= 255)
            {
                numberHigh = (byte)((count & 0xff00) >> 8);
                numberLow = (byte)(count & 0xff);
            }
            else
            {
                numberHigh = (byte)count;
                numberLow = 0x00;
            }

            var result08 = GetLinRecvMsg(new byte[] { 0x08, 0x10, 0x11, 0x12, 0x13, numberHigh, numberLow, 0x00 },
                _commandLinId, _responseLinId, isNeedRetry: false);

            if (!result08.StartsWith("08FF"))
                return false;

            foreach (var b in blocks)
            {
                foreach (var itemB in b)
                {
                    var tempXorcs = itemB.Data.Aggregate((byte)0x00, (current, data) => (byte)(current + data));
                    var mChksum = (byte)(tempXorcs & 0xFF);

                    var byte0 = BitConverter.GetBytes(itemB.Address).Reverse().ToList()[1];
                    var byte1 = BitConverter.GetBytes(itemB.Address).Reverse().ToList()[2];
                    var byte2 = BitConverter.GetBytes(itemB.Address).Reverse().ToList()[3];
                    var byte4 = itemB.DataLen;

                    // 0x09
                    var result09 = GetLinRecvMsg(new byte[] { 0x09, byte0, byte1, byte2, byte4, mChksum, 0x00, 0x00 }, _commandLinId, _responseLinId, isNeedRetry: false);
                    if (!result09.StartsWith("09FF"))
                        return false;

                    var rest11Datas = itemB.DataLen % 6;
                    var toSendData = new List<byte>();
                    toSendData.AddRange(itemB.Data);
                    if (rest11Datas > 0)
                        for (var i = 0; i < 6 - rest11Datas; i++)
                            toSendData.Add(0x00);

                    var seq = (byte)0x00;
                    for (var i = 0; i < toSendData.Count; i += 6)
                    {
                        Lin.SendMasterLin(_commandLinId, new byte[] { 0x11, seq, toSendData[i], toSendData[i + 1], toSendData[i + 2], toSendData[i + 3], toSendData[i + 4], toSendData[i + 5] });
                        seq++;
                    }

                    byte[] result11Bytes;
                    Lin.SendSlaveLin(_responseLinId, out result11Bytes, timeOutMs: 1500);
                    var result11 = ValueHelper.GetHextStr(result11Bytes).Replace(" ", "");
                    if (!result11.StartsWith("11FF"))
                        return false;
                }
            }

            var result12 = GetLinRecvMsg(new byte[] { 0x12, 0x02, 0x03, 0x04, 0x05, 0x00, 0x00, 0x00 }, _commandLinId, _responseLinId, isNeedRetry: false);
            if (!result12.StartsWith("12FF"))
                return false;

            return true;
        }

        public bool ConnectStatusCheck(int tryCount, byte masterLinId, byte slaveLinId)
        {
            while (true)
            {
                for (var i = 0; i < tryCount; i++)
                {
                    var connectStatusCheckBytes = new byte[] { 0x01, 0x31, 0x32, 0x33, 0x34, 0x00, 0x00, 0x00 };

                    var result =
                        GetLinRecvMsg(connectStatusCheckBytes, masterLinId, slaveLinId, isNeedRetry: false, delayMs: 5);

                    if (!string.IsNullOrEmpty(result) &&
                        result.Length == 8 * 2 &&
                        result.StartsWith("01FF"))
                        return true;

                    Thread.Sleep(5);
                }

                return false;
            }
        }

        private string GetLinRecvMsg(
            byte[] sendBytes, byte masterLinId, byte slaveLinId, int delayMs = 50, bool isNeedRetry = true)
        {
            byte[] resultBytes;
            if (Lin.SendMasterLinAndRecvSingleSlaveLin(
                    masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
            {
                if (resultBytes != null && resultBytes.Length == 8)
                    return resultBytes.Aggregate(string.Empty,
                        (current, t) => current + ValueHelper.GetHextStr(t));

                if (isNeedRetry)
                {
                    Thread.Sleep(500);
                    if (!Lin.SendMasterLinAndRecvSingleSlaveLin(
                            masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
                        return string.Empty;

                    if (resultBytes != null && resultBytes.Length == 8)
                        return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));
                }
            }
            else
            {
                if (isNeedRetry)
                {
                    Thread.Sleep(500);
                    if (!Lin.SendMasterLinAndRecvSingleSlaveLin(
                            masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
                        return string.Empty;

                    if (resultBytes != null && resultBytes.Length == 8)
                        return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));
                }
            }

            return string.Empty;
        }

        private static IEnumerable<S19LineData> GetS19LineData(string filePath)
        {
            var temp = new List<string>();
            using (var sr = new StreamReader(filePath, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        var str = line.Trim(' ');
                        if (!string.IsNullOrEmpty(str))
                            temp.Add(str);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }

            var listLineData = new List<S19LineData>();

            foreach (var t in temp)
            {
                var thisLineData = new S19LineData();
                var tempCheckSum = (byte)0;

                var stype = t.Substring(0, 2);

                // 获取Type
                for (var i = 0; i < (int)S19Type.Smax; i++)
                {
                    if (stype != S19TypeTable[i])
                        continue;
                    thisLineData.Type = EnumOperater.GetEnum<S19Type>(S19TypeTable[i]);
                    break;
                }

                // 获取Count
                thisLineData.Count = Convert.ToByte(t.Substring(2, 2), 16);
                tempCheckSum += thisLineData.Count;
                if (thisLineData.Count != t.Length / 2 - 2)
                {
                    // error
                    return new List<S19LineData>();
                }

                // 获取Address
                if (thisLineData.Type == S19Type.S0)
                {
                    thisLineData.Address = 0x00;
                }
                else if (thisLineData.Type == S19Type.S1 || thisLineData.Type == S19Type.S5 || thisLineData.Type == S19Type.S9)
                {
                    thisLineData.Address = Convert.ToUInt32(t.Substring(4, 4), 16);

                    tempCheckSum += (byte)(thisLineData.Address >> 8 & 0x0FF);
                    tempCheckSum += (byte)(thisLineData.Address & 0x0FF);

                    var dataLen = t.Length / 2 - 1 - 1 - 1 - 2;
                    thisLineData.Data = new byte[dataLen];
                    for (var i = 0; i < thisLineData.Count - 3; i++)
                    {
                        var data = Convert.ToByte(t.Substring(2 * i + 8, 2), 16);
                        thisLineData.Data[i] = data;
                        tempCheckSum += data;
                    }

                    thisLineData.CheckSum = Convert.ToByte(t.Substring(t.Length - 2, 2), 16);
                    tempCheckSum = (byte)(0xFF - tempCheckSum);
                    if (thisLineData.CheckSum != tempCheckSum)
                    {
                        // checksum error
                        return new List<S19LineData>();
                    }
                }
                else if (thisLineData.Type == S19Type.S2 || thisLineData.Type == S19Type.S8)
                {
                    thisLineData.Address = Convert.ToUInt32(t.Substring(4, 6), 16);

                    tempCheckSum += (byte)(thisLineData.Address >> 16 & 0xFF);
                    tempCheckSum += (byte)(thisLineData.Address >> 8 & 0xFF);
                    tempCheckSum += (byte)(thisLineData.Address & 0xFF);

                    var dataLen = t.Length / 2 - 1 - 1 - 1 - 3;
                    thisLineData.Data = new byte[dataLen];
                    for (var i = 0; i < thisLineData.Count - 4; i++)
                    {
                        var data = Convert.ToByte(t.Substring(2 * i + 10, 2), 16);
                        thisLineData.Data[i] = data;
                        tempCheckSum += data;
                    }

                    thisLineData.CheckSum = Convert.ToByte(t.Substring(t.Length - 2, 2), 16);
                    tempCheckSum = (byte)(0xFF - tempCheckSum);
                    if (thisLineData.CheckSum != tempCheckSum)
                    {
                        // checksum error
                        return new List<S19LineData>();
                    }
                }
                else if (thisLineData.Type == S19Type.S3 || thisLineData.Type == S19Type.S7)
                {
                    thisLineData.Address = Convert.ToUInt32(t.Substring(4, 8), 16);

                    tempCheckSum += (byte)(thisLineData.Address >> 24 & 0xFF);
                    tempCheckSum += (byte)(thisLineData.Address >> 16 & 0xFF);
                    tempCheckSum += (byte)(thisLineData.Address >> 8 & 0xFF);
                    tempCheckSum += (byte)(thisLineData.Address & 0xFF);

                    var dataLen = t.Length / 2 - 1 - 1 - 1 - 4;
                    thisLineData.Data = new byte[dataLen];
                    for (var i = 0; i < thisLineData.Count - 5; i++)
                    {
                        var data = Convert.ToByte(t.Substring(2 * i + 12, 2), 16);
                        thisLineData.Data[i] = data;
                        tempCheckSum += data;
                    }

                    thisLineData.CheckSum = Convert.ToByte(t.Substring(t.Length - 2, 2), 16);
                    tempCheckSum = (byte)(0xFF - tempCheckSum);
                    if (thisLineData.CheckSum != tempCheckSum)
                    {
                        // checksum error
                        return new List<S19LineData>();
                    }
                }

                // 获取DataLen
                if (thisLineData.Type == S19Type.S1)
                    thisLineData.DataLen = (byte)(thisLineData.Count - 0x03);
                else if (thisLineData.Type == S19Type.S2)
                    thisLineData.DataLen = (byte)(thisLineData.Count - 0x04);
                else if (thisLineData.Type == S19Type.S3)
                    thisLineData.DataLen = (byte)(thisLineData.Count - 0x05);
                else
                    thisLineData.DataLen = 0x00;

                listLineData.Add(thisLineData);
            }

            return listLineData;
        }

        private static List<List<S19LineData>> GetBlocks(IEnumerable<S19LineData> s19Lines)
        {
            var dataLines = s19Lines.Where(
                    t => t.Type == S19Type.S1 || t.Type == S19Type.S2 || t.Type == S19Type.S3)
                .ToList().OrderBy(s => s.Address);

            var blockList = new List<List<S19LineData>>();

            var addrBase = (uint)0;
            foreach (var t in dataLines)
            {
                if (addrBase != t.Address)
                    blockList.Add(new List<S19LineData>());

                blockList[blockList.Count - 1].Add(t);

                addrBase = t.Address + t.DataLen;
            }

            return blockList;
        }

        internal class S19LineData
        {
            public S19Type Type;

            public byte Count;

            public uint Address;

            public byte[] Data;

            public byte CheckSum;

            public byte DataLen;
        }

        internal enum S19Type
        {
            S0 = 0,

            S1,

            S2,

            S3,

            S5,

            S7,

            S8,

            S9,

            Smax
        }

        private static readonly string[] S19TypeTable =
        {
            "S0","S1","S2","S3","S5","S7","S8","S9",
        };

        #endregion

        #region 标定

        [Description("写入第1个点-基于当前输出占空比+1")]
        public void WriteFirstPointUp()
        {
            if (Lin != null)
            {
                Lin.SendMasterLin(0x30, new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            }
        }

        [Description("写入第1个点-基于当前输出占空比-1")]
        public void WriteFirstPointDown()
        {
            Lin.SendMasterLin(0x30, new byte[] { 0x02 ,0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
        }

        [Description("写入第2个点-基于当前输出占空比+1")]
        public void WriteSecondPointUp()
        {
            if (Lin != null)
            {
                Lin.SendMasterLin(0x31, new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            }
        }

        [Description("写入第2个点-基于当前输出占空比-1")]
        public void WriteSecondPointDown()
        {
            Lin.SendMasterLin(0x31, new byte[] { 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
        }

        [Description("R,读取第1个点写入标志")]
        public string FirstPointWriteFlag = string.Empty;
        [Description("R,读取第1个点输出占空比")]
        public string FirstPointOutputDuty = string.Empty;

        [Description("读取第1个点")]
        public void ReadFirstPoint()
        {
            FirstPointWriteFlag = string.Empty;
            FirstPointOutputDuty = string.Empty;

            if (Lin != null)
            {
                byte[] echo;
                if (Lin.SendSlaveLin(0x32, out echo))
                {
                    if (echo.Length >= 3)
                    {
                        FirstPointWriteFlag = echo[2] == 0x01 ? bool.TrueString : bool.FalseString;
                        FirstPointOutputDuty = (echo[0] * 256 + echo[1]).ToString();
                    }
                }
            }
        }

        [Description("R,读取第2个点写入标志")]
        public string SecondPointWriteFlag = string.Empty;
        [Description("R,读取第2个点输出占空比")]
        public string SecondPointOutputDuty = string.Empty;

        [Description("读取第2个点")]
        public void ReadSecondPoint()
        {
            SecondPointWriteFlag = string.Empty;
            SecondPointOutputDuty = string.Empty;

            if (Lin != null)
            {
                byte[] echo;
                if (Lin.SendSlaveLin(0x33, out echo))
                {
                    if (echo.Length >= 3)
                    {
                        SecondPointWriteFlag = echo[2] == 0x01 ? bool.TrueString : bool.FalseString;
                        SecondPointOutputDuty = (echo[0] * 256 + echo[1]).ToString();
                    }
                }
            }
        }

        #endregion
    }
}
