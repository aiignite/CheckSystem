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
    [Description("LIN-Product,AS33-LedAppDownload")]
    public sealed class As33LedAppDownload : ControllerBase
    {
        public LinBus Lin;

        [Description("R,下载结果")]
        public string DownloadResult = string.Empty;

        [Description("R,下载耗时-秒")]
        public float DownloadCostTime;

        [Description("R/W,APP文件路径")]
        public string AppFilePath =
             //string.Empty;
             //@"C:\Projs\2022\点灯相关\AS33\AS33下载上位机-20220531\日志-20220531\AS33_DRL_APP_V1.2.05_20220518_Release.sx";
             @"E:\桌面\新建文件夹\T010_350mA.sx";

        private byte _commandLinId = 0x32;
        private byte _responseLinId = 0x33;
        private static readonly object AppFileLocker = new object();

        public As33LedAppDownload(string name)
            : base(name) { }

        ~As33LedAppDownload()
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

        [Description("R,擦除APP结果")]
        public string EraseAppResult;

        [Description("擦除APP")]
        public void EraseApp()
        {
            EraseAppResult = "NG";

            if (Lin == null)
                return;

            if (!ConnectStatusCheck(50, _commandLinId, _responseLinId))
                return;

            // Flash Erase Request
            var sendData = new[]
            {
                (byte) 0x03, (byte) 0xFF, (byte) 0xA0, (byte) 0x00, (byte) 0x00, (byte) 0x50, (byte) 0x00, (byte) 0x39
            };
            var flashEraseRequest = GetLinRecvMsg(sendData, _commandLinId, _responseLinId, 2500);
            if (!flashEraseRequest.StartsWith("03FF"))
                return;

            EraseAppResult = "OK";
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

            lock (AppFileLocker)
            {
                if (string.IsNullOrEmpty(AppFilePath))
                    return;

                if (!File.Exists(AppFilePath))
                    return;

                s19LineDatas = GetS19LineData(AppFilePath).ToArray();
            }

            if (s19LineDatas.Length <= 0)
            {
                DownloadResult += " 未选择下载APP文件";
                return;
            }

            if (LinAppDownLoadVersion2(s19LineDatas))
                DownloadResult = "OK";
        }

        private bool LinAppDownLoadVersion2(
            IEnumerable<S19LineData> listLineData)
        {
            if (!ConnectStatusCheck(50, _commandLinId, _responseLinId))
                return false;

            var lineDatas = listLineData as S19LineData[] ?? listLineData.ToArray();
            //var s19LineDatas = listLineData as S19LineData[] ?? lineDatas.ToArray();
            //var downloadData =
            //    s19LineDatas.Where(t => t.Type == S19Type.S1 || t.Type == S19Type.S2 || t.Type == S19Type.S3).ToList();

            // Flash Erase Request
            var sendData = new[]
            {
                (byte) 0x03, (byte) 0xFF, (byte) 0xA0, (byte) 0x00, (byte) 0x00, (byte) 0x50, (byte) 0x00, (byte) 0x39
            };
            var flashEraseRequest = GetLinRecvMsg(sendData, _commandLinId, _responseLinId, 2500, false);
            if (!flashEraseRequest.StartsWith("03FF"))
                return false;

            var blocks = GetBlocks(lineDatas);
            if (blocks.Count != 1)
                return false;

            try
            {
                var last1DataCount = blocks[0][blocks[0].Count - 1].DataLen;
                var last2DataCount = blocks[0][blocks[0].Count - 2].DataLen;

                var tempList = new List<byte>();
                tempList.AddRange(blocks[0][blocks[0].Count - 1].Data);

                for (var i = 0; i < last2DataCount - last1DataCount; i++)
                    tempList.Add(0xFF);

                blocks[0][blocks[0].Count - 1].DataLen = (byte)tempList.Count;
                blocks[0][blocks[0].Count - 1].Data = new byte[tempList.Count];
                Array.Copy(tempList.ToArray(), blocks[0][blocks[0].Count - 1].Data, tempList.Count);

            }
            catch (Exception)
            {
                return false;
            }

            var count = blocks.SelectMany(b => b).Aggregate(0, (current, t) => current + t.DataLen);

            sendData = new[]
            {
                (byte) 0x04,
                BitConverter.GetBytes(blocks[0][0].Address).Reverse().ToList()[1],
                BitConverter.GetBytes(blocks[0][0].Address).Reverse().ToList()[2],
                BitConverter.GetBytes(blocks[0][0].Address).Reverse().ToList()[3],
                (byte) (count >> 16), (byte) (count >> 8), (byte) (count & 0x0000FF),
                (byte) 0x3A
            };

            var flashProInfoResult = GetLinRecvMsg(sendData, _commandLinId, _responseLinId, 1500, false);
            if (!flashProInfoResult.StartsWith("04FF"))
                return false;

            var blockCount = count % 128 == 0 ? count / 128 : count / 128 + 1;
            var blockBytes = new List<List<byte>>();

            var allData = new List<byte>();
            foreach (var b in blocks.SelectMany(block => block))
                allData.AddRange(b.Data);

            if (count % 128 == 0)
            {
                var index = 0;
                for (var i = 0; i < blockCount; i++)
                {
                    blockBytes.Add(new List<byte>());
                    for (var j = 0; j < 128; j++)
                    {
                        blockBytes[i].Add(allData[index]);
                        index++;
                    }
                    blockBytes[i].AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                }
            }
            else
            {
                var index = 0;
                for (var i = 0; i < blockCount - 1; i++)
                {
                    blockBytes.Add(new List<byte>());
                    for (var j = 0; j < 128; j++)
                    {
                        blockBytes[i].Add(allData[index]);
                        index++;
                    }
                    blockBytes[i].AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                }

                blockBytes.Add(new List<byte>());
                for (var i = index; i < allData.Count; i++)
                    blockBytes[blockCount - 1].Add(allData[i]);

                var restDataCount = allData.Count - index;
                if (restDataCount % 6 != 0)
                {
                    //var restBytes=new List<byte>();
                    //for (var i = 0; i < 6 - restDataCount % 6; i++)
                    //    restBytes.Add(0xFF);
                    blockBytes[blockCount - 1].AddRange(new byte[6 - restDataCount % 6]);
                }
            }

            foreach (var b in blockBytes)
            {
                var seq = -1;
                for (var i = 0; i < b.Count; i = i + 6)
                {
                    seq++;
                    sendData = new byte[] { 0x05, (byte)seq, b[i], b[i + 1], b[i + 2], b[i + 3], b[i + 4], b[i + 5] };

                    if (seq == 0xFF)
                        seq = -1;

                    if (i == b.Count - 6)
                    {
                        var flashProgReq = GetLinRecvMsg(sendData, _commandLinId, _responseLinId, 500);
                        if (!flashProgReq.StartsWith("05FF"))
                            return false;
                    }
                    else
                    {
                        Lin.SendMasterLin(_commandLinId, sendData);
                        Thread.Sleep(1);
                    }
                }
            }

            sendData = new byte[] { 0x02, 0x35, 0x36, 0x37, 0x38, 0x00, 0x00, 0x00 };
            var exitBootResult = GetLinRecvMsg(sendData, _commandLinId, _responseLinId, 100);
            return exitBootResult.StartsWith("02FF");
        }

        public bool ConnectStatusCheck(int tryCount, byte masterLinId, byte slaveLinId)
        {
            // bebug
            //return true;

            while (true)
            {
                for (var i = 0; i < tryCount / 5; i++)
                {
                    Lin.SendMasterLin(masterLinId, new byte[] { 0x01, 0x31, 0x32, 0x33, 0x34, 0x00, 0x00, 0x00 });

                    var connectStatusCheckBytes = new byte[] { 0x01, 0x31, 0x32, 0x33, 0x34, 0x00, 0x00, 0x00 };
                    //SetLinMsgDataGridView(new GatewayLin.LinRecvDataPackage(masterLinId, connectStatusCheckBytes, 0x55,
                    //    0x55));

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
    }
}
