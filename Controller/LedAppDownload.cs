using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,LedAppDownload")]
    public sealed class LedAppDownload : ControllerBase
    {
        public LinBus Lin;

        [Description("R,下载结果")]
        public string DownloadResult = string.Empty;

        [Description("R,下载耗时-秒")]
        public float DownloadCostTime;

        [Description("R/W,APP文件路径")]
        public string AppFilePath =
            //string.Empty;
            //@"D:\Projects\EP33系列\EP33后灯\EP33低配后灯下载\EP33低配程序\EP33_LOW_RCLA_Appl_S005E.sre";
            //@"D:\Projects\EP33系列\EP33后灯\EP33低配后灯下载\EP33低配程序\EP33_LOW_RCLB_Appl_S004E.sre";
            @"E:\桌面\新建文件夹\T010_350mA.sx";

        private const byte CommandReqLinId = 0x2A;
        private const byte CommandRecvLinId = 0x2B;
        private const byte MasterLinId = 0x3C;
        private const byte SlaveLinId = 0x3D;
        private byte _nad = 0x02;

        private readonly List<S19LineData> _slines =
            new List<S19LineData>();

        public LedAppDownload(string name)
            : base(name) { }

        ~LedAppDownload()
        {
            Dispose();
        }

        [Description("切换成后灯A左")]
        public void ChangeToLa()
        {
            _nad = 0x01;
        }

        [Description("切换成后灯B左")]
        public void ChangeToLb()
        {
            _nad = 0x03;
        }

        [Description("切换成后灯A右")]
        public void ChangeToRa()
        {
            _nad = 0x02;
        }

        [Description("切换成后灯B右")]
        public void ChangeToRb()
        {
            _nad = 0x03;
        }

        [Description("读取APP文件")]
        public void GetAppBlocks()
        {
            if (string.IsNullOrEmpty(AppFilePath))
                return;

            if (!File.Exists(AppFilePath))
                return;

            _slines.Clear();
            _slines.AddRange(GetS19LineData(AppFilePath));
        }

        /// <summary>
        /// 唤醒
        /// </summary>
        private void Awake()
        {
            if (Lin == null) return;
            for (var i = 0; i < 1; i++)
            {
                Lin.SendMasterLin(CommandReqLinId, new byte[8]);
                Thread.Sleep(1);
            }

            //byte[] echo;
            //Lin.SendSlaveLin(0x2B, out echo, 50);
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

            if (_slines.Count <= 0)
            {
                DownloadResult += " 未选择下载APP文件";
                return;
            }

            {
                Awake();

                // 10 03
                byte[] echoBytes1;
                Lin.SendMasterLinAndRecvSingleSlaveLin(MasterLinId, SlaveLinId,
                    new byte[] { _nad, 0x02, 0x10, 0x03, 0x00, 0x00, 0x00, 0x00 }, out echoBytes1, 5);

                if (echoBytes1 != null && echoBytes1.Length == 8 && echoBytes1[0] == _nad && echoBytes1[1] == 0x02 &&
                    echoBytes1[2] == 0x50 && echoBytes1[3] == 0x03)
                {
                    // 27 01
                    Thread.Sleep(50);
                    byte[] echoBytes2;
                    Lin.SendMasterLinAndRecvSingleSlaveLin(MasterLinId, SlaveLinId,
                        new byte[] { _nad, 0x02, 0x27, 0x01, 0x00, 0x00, 0x00, 0x00 }, out echoBytes2, 5);
                    if (echoBytes2 != null && echoBytes2.Length == 8 && echoBytes2[0] == _nad && echoBytes2[1] == 0x06 &&
                        echoBytes2[2] == 0x67 && echoBytes2[3] == 0x01)
                    {
                        // 27 02
                        Thread.Sleep(50);
                        byte[] echoBytes3;
                        Lin.SendMasterLinAndRecvSingleSlaveLin(MasterLinId, SlaveLinId,
                            new byte[]
                            {
                                _nad, 0x06, 0x27, 0x02,
                                (byte) ~echoBytes2[4],
                                (byte) ~echoBytes2[5],
                                (byte) ~echoBytes2[6],
                                (byte) ~echoBytes2[7]
                            }, out echoBytes3, 250);
                        if (echoBytes3 != null && echoBytes3.Length == 8 && echoBytes3[0] == _nad && echoBytes3[1] == 0x02 &&
                            echoBytes3[2] == 0x67 && echoBytes3[3] == 0x02)
                        {
                            // 10 02
                            byte[] echoBytes4;
                            Lin.SendMasterLinAndRecvSingleSlaveLin(MasterLinId, SlaveLinId,
                                new byte[] { _nad, 0x02, 0x10, 0x02, 0x00, 0x00, 0x00, 0x00 }, out echoBytes4, 50);
                            if (echoBytes4 != null && echoBytes4.Length == 8 && echoBytes4[0] == _nad &&
                                echoBytes4[1] == 0x02 &&
                                echoBytes4[2] == 0x50 && echoBytes4[3] == 0x02)
                            {
                                Thread.Sleep(15);
                                var error = string.Empty;
                                var isOk = LinAppDownLoadVersion1(GetBlocks(_slines), CommandReqLinId, CommandRecvLinId, ref error);

                                if (isOk)
                                {
                                    DownloadResult = "OK";
                                }
                                else
                                {
                                    DownloadResult += " " + error;
                                }
                            }
                            else
                            {
                                DownloadResult += " 1002失败。";
                            }
                        }
                        else
                        {
                            DownloadResult += " 2702失败。";
                        }
                    }
                    else
                    {
                        DownloadResult += " 2701失败。";
                    }
                }
                else
                {
                    DownloadResult += " 1003失败。";
                }
            }
        }

        private bool LinAppDownLoadVersion1(
            IReadOnlyList<List<S19LineData>> blocks, byte masterLinId, byte slaveLinId, ref string errorMsg)
        {
            if (!ConnectStatusCheck(500, masterLinId, slaveLinId))
            {
                errorMsg = "握手失败";
                return false;
            }

            Thread.Sleep(15);

            // Flash Erase Request(0x1B/0x03/ 0x26/0x27/0x28/0x29/data[5]/dat a[6]/0x0)
            // Data[5]中是解析为S19文件后S2行总 行数高8位，data[6]中是解析为S19文 件后S2行总行数低8位
            // Flash Erase check(0x1C)
            // Feedback correct(0x03/0xff/其它 byte为0)
            var downloadDataCount =
                //BitConverter.GetBytes((ushort)1377);
                BitConverter.GetBytes((ushort)blocks.Aggregate(0, (current, t) => current + t.Count));
            //BitConverter.GetBytes(_slines.Count);
            Array.Reverse(downloadDataCount);
            var flashEraseRequestResult =
                GetLinRecvMsg(
                    new byte[] { 0x03, 0x26, 0x27, 0x28, 0x29, downloadDataCount[0], downloadDataCount[1], 0x00 },
                    masterLinId, slaveLinId, 6000, false);

            if (string.IsNullOrEmpty(flashEraseRequestResult) ||
                !flashEraseRequestResult.StartsWith("03FF"))
            {
                errorMsg = "Flash Erase Request Failed";
                return false;
            }

            // Flash Program information Request(0x1B/0x04/ data[1]/data[2]/data[3]/data[4]/dat a[5]/0x0/0x0)
            // Data[1]是解析为S19文件后每一个S2行 首地址的16～23位，data[2]是首地址的 8～15位，data[3是首地址的0～7位， data[4]是这个S2的数据长度，data[5]是 这个S2行的校验和
            // Flash Program information check(0x1C)
            // Feedback correct(0x04/0xff/其它 byte为0)
            // Flash Program Data Request（0x1B/0x06/ data[1]/data[2]/data[3]/data[4]/data[5]/data [6]/data[7])
            // Data[1]是每发送一次0x10/0x06的计 数加1，当这行S2数据全部发送完 毕，发送查询帧0x1A,计数清 0.data[2]~data[7]是这个S2行的数据
            // Flash Program check （0x1C)
            // Feedback correct(0x06/0xff/其它 byte为0)
            for (var b = 0; b < blocks.Count; b++)
            {
                var block = blocks[b];

                for (var bb = 0; bb < block.Count; bb++)
                {
                    var t = block[bb];

                    // Data[1]是解析为S19文件后每一个S2行首地址的16～23位， 
                    // data[2]是首地址的8～15位， 
                    // data[3]是首地址的0～7位，
                    // data[4]是这个S2的数据长度， 
                    // data[5]是这个S2行的校验和
                    var addrBytes = BitConverter.GetBytes(t.Address);
                    var dataSum = t.Data.Aggregate(0, (current, d) => current + d);
                    var flashProgramInformationRequestResult =
                        GetLinRecvMsg(
                            new byte[]
                            {
                                0x04, addrBytes[3], addrBytes[2], addrBytes[1], addrBytes[0], t.DataLen, (byte) dataSum,
                                0x00
                            },
                            masterLinId, slaveLinId, 50);

                    var str1 = string.Format(@"Line{0}/{1} In Block{2}/{3} Flash Program information Request", bb, block.Count - 1, b, blocks.Count - 1);
                    if (string.IsNullOrEmpty(flashProgramInformationRequestResult) ||
                        !flashProgramInformationRequestResult.StartsWith("04FF"))
                    {
                        errorMsg = str1 + " Failed";
                        return false;
                    }

                    var sendDatas = new List<byte>();
                    sendDatas.AddRange(t.Data);

                    var sendCount = sendDatas.Count / 6;
                    var restCount = sendDatas.Count % 6;
                    if (restCount > 0)
                    {
                        sendCount++;
                        for (var i = 0; i < 6 - restCount; i++)
                            sendDatas.Add(0xFF);
                    }

                    for (var i = 0; i < sendCount; i++)
                    {
                        var thisFrameDatas = new List<byte> { 0x06, (byte)i };
                        for (var j = 0; j < 6; j++)
                            thisFrameDatas.Add(sendDatas[6 * i + j]);
                        Lin.SendMasterLin(masterLinId, thisFrameDatas.ToArray());
                        Thread.Sleep(1);
                    }

                    Thread.Sleep(25);

                    byte[] flashProgramDataRequestResult;
                    var str2 = string.Format(@"Line{0}/{1} In Block{2}/{3} Flash Program", bb, block.Count - 1, b, blocks.Count - 1);

                    if (!Lin.SendSlaveLin(slaveLinId, out flashProgramDataRequestResult) ||
                        flashProgramDataRequestResult == null ||
                        flashProgramDataRequestResult.Length != 8)
                    {
                        errorMsg = str2 + " Failed";
                        return false;
                    }

                    if (flashProgramDataRequestResult[0] == 0x06 &&
                        flashProgramDataRequestResult[1] == 0xFF)
                    {

                    }
                    else
                    {
                        errorMsg = str2 + " Failed";
                        return false;
                    }
                }
            }

            // Flash work flag Request（0x1B/0x07/ 0x50/0x51/0x52/0x53/0x0/0x0/0x0)
            // Flash work flag check(0x1C)
            // Feedback correct(0x07/0xff/其它 byte为0)
            var flashWorkFlahCheckResult =
               GetLinRecvMsg(new byte[] { 0x07, 0x50, 0x51, 0x52, 0x53, 0x00, 0x00, 0x00 }, masterLinId, slaveLinId, 150, false);

            if (string.IsNullOrEmpty(flashWorkFlahCheckResult) ||
                !flashWorkFlahCheckResult.StartsWith("07FF"))
            {
                errorMsg = "Flash work flag Request Faied";
                return false;
            }

            // Integrity detect Request （0x1B/0x02/ 0x18/0x19/0x20/0x21/0x0/0x0/0x0)
            // Integrity detect check(0x1C)
            // Feedback correct (0x02/0xff)
            var integrityDetectRequestResult = GetLinRecvMsg(
                new byte[] { 0x02, 0x18, 0x19, 0x20, 0x21, 0x00, 0x00, 0x00 }, masterLinId, slaveLinId, 150, false);

            if (string.IsNullOrEmpty(integrityDetectRequestResult) ||
                !integrityDetectRequestResult.StartsWith("02FF"))
            {
                errorMsg = "Exit Boot Faied";
                return false;
            }

            return true;
        }

        private bool ConnectStatusCheck(int tryCount, byte masterLinId, byte slaveLinId)
        {
            // bebug
            //return true;

            while (true)
            {
                for (var i = 0; i < tryCount / 50; i++)
                {
                    var connectStatusCheckBytes = new byte[] { 0x01, 0x31, 0x32, 0x33, 0x34, 0x00, 0x00, 0x00 };
                    //SetLinMsgDataGridView(new GatewayLin.LinRecvDataPackage(masterLinId, connectStatusCheckBytes, 0x55,
                    //    0x55));

                    var result =
                        GetLinRecvMsg(connectStatusCheckBytes, masterLinId, slaveLinId, isNeedRetry: false);
                    if (!string.IsNullOrEmpty(result) &&
                        result.Length == 8 * 2 &&
                        result.StartsWith("01FF"))
                        return true;

                    Thread.Sleep(50);
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

                if (!isNeedRetry)
                    return string.Empty;
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
                if (!isNeedRetry)
                    return string.Empty;
                Thread.Sleep(500);
                if (!Lin.SendMasterLinAndRecvSingleSlaveLin(
                    masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
                    return string.Empty;

                if (resultBytes != null && resultBytes.Length == 8)
                    return resultBytes.Aggregate(string.Empty,
                        (current, t) => current + ValueHelper.GetHextStr(t));
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
                        //if (!line.StartsWith("S1") && !line.StartsWith("S2") && !line.StartsWith("S3"))
                        //    continue;

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

        internal struct S19LineData
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
