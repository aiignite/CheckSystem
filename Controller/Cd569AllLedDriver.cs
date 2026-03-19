using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;

namespace Controller
{
    [Description("LIN-Product,CD569全LED驱动模块")]
    public sealed class Cd569AllLedDriver : ControllerBase
    {
        public LinBus Lin;

        private byte _masterLinId = 0x10;
        private byte _slaveLinId = 0x1A;

        public Cd569AllLedDriver(string name)
            : base(name) { }

        /// <summary>
        /// 条码内容
        /// </summary>
        public string BarcodeContent;

        /// <summary>
        /// 总成零件号
        /// </summary>
        [Description("R,总成零件号")]
        public string HwPn;

        /// <summary>
        /// 生产序列号
        /// </summary>
        [Description("R,生产序列号")]
        public string SerialNum;

        /// <summary>
        /// 生产日期
        /// </summary>
        [Description("R,生产日期")]
        public string ManufactureDate;

        /// <summary>
        /// FBL零件号
        /// </summary>
        [Description("R,FBL零件号")]
        public string FblSwPn;

        /// <summary>
        /// 应用程序零件号
        /// </summary>
        [Description("R,应用程序零件号")]
        public string AppSwPn;

        /// <summary>
        /// 配置文件零件号
        /// </summary>
        [Description("R,配置文件零件号")]
        public string CfgPn;

        /// <summary>
        /// FBL版本号
        /// </summary>
        [Description("R,FBL版本号")]
        public string FblVer;

        /// <summary>
        /// 应用程序版本号
        /// </summary>
        [Description("R,应用程序版本号")]
        public string AppSwVer;

        /// <summary>
        /// 配置文件版本号
        /// </summary>
        [Description("R,配置文件版本号")]
        public string CfgVer;

        [Description("R,MEC值读取")]
        public string MecValue;

        [Description("R,MEC清零")]
        public string MecClearResult;

        [Description("R,解锁SeedKey")]
        public string SeedKeyResult;

        public void InitMasterLinId(string masterLinId)
        {
            _masterLinId = Convert.ToByte(masterLinId, 16);
        }

        public void InitSlavaLinId(string slaveLinId)
        {
            _slaveLinId = Convert.ToByte(slaveLinId, 16);
        }

        private string GetLinRecvMsg(string sendMasterLinValue)
        {
            var sendBytes = new List<byte>();
            for (var i = 0; i < sendMasterLinValue.Length; i = i + 2)
            {
                var temp = sendMasterLinValue[i].ToString() + sendMasterLinValue[i + 1];
                sendBytes.Add(Convert.ToByte(temp, 16));
            }

            byte[] resultBytes;
            if (Lin.SendMasterLinAndRecvSingleSlaveLin(
                _masterLinId, _slaveLinId, sendBytes.ToArray(), out resultBytes))
            {
                if (resultBytes != null)
                    return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));

                Thread.Sleep(500);
                if (!Lin.SendMasterLinAndRecvSingleSlaveLin(
                    _masterLinId, _slaveLinId, sendBytes.ToArray(), out resultBytes))
                    return string.Empty;

                if (resultBytes != null)
                    return resultBytes.Aggregate(string.Empty,
                        (current, t) => current + ValueHelper.GetHextStr(t));
            }
            else
            {
                Thread.Sleep(500);
                if (!Lin.SendMasterLinAndRecvSingleSlaveLin(
                    _masterLinId, _slaveLinId, sendBytes.ToArray(), out resultBytes))
                    return string.Empty;

                if (resultBytes != null)
                    return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));
            }

            return string.Empty;
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
            else if (isNeedRetry)
            {
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

        [Description("MEC清零")]
        public void MecClear()
        {
            MecClearResult = string.Empty;
            MecClearResult = GetLinRecvMsg("2E9A000000000000");
        }

        [Description("MEC复位至可点亮")]
        public void MecReset()
        {
            MecClearResult = string.Empty;
            MecClearResult = GetLinRecvMsg("2E9AFF0000000000");
        }

        [Description("MEC值读取")]
        public void ReadMec()
        {
            MecValue = string.Empty;
            MecValue = GetLinRecvMsg("229A000000000000");
        }

        /// <summary>
        /// 写总成零件号
        /// </summary>
        [Description("测试写总成零件号")]
        public void WriteHwPn()
        {
            //GetLinRecvMsg("2E01001267080000");
        }

        /// <summary>
        /// 写生产序列号
        /// </summary>
        [Description("测试写固定值生产序列号")]
        public void WriteSerialNum()
        {
            GetLinRecvMsg("2E03313233350000");
            return;

            //if (string.IsNullOrEmpty(BarcodeContent))
            //    return;

            //if (!sendByteVal.Contains("????"))
            //    return;

            //if (string.IsNullOrEmpty(BarcodeContent) ||
            //   BarcodeContent.Contains("888888"))
            //    return;

            //try
            //{
            //    var serialNum = Convert.ToUInt16(BarcodeContent.Substring(35, 4));
            //    var serialNumBytes = BitConverter.GetBytes(serialNum);
            //    Array.Reverse(serialNumBytes);

            //    var temp = serialNumBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            //    if (temp.Length != 4)
            //        return;

            //    sendByteVal = sendByteVal.Replace("????", temp);
            //    GetLinRecvMsg(sendByteVal);
            //}
            //catch (Exception)
            //{
            //    // ignored
            //}
        }

        /// <summary>
        /// 写生产日期
        /// </summary>
        [Description("测试写固定值生产日期")]
        public void WriteManufactureDate()
        {
            //GetLinRecvMsg("2E02333630313900");
            //return;

            //if (string.IsNullOrEmpty(BarcodeContent))
            //    return;

            //if (!sendByteVal.Contains("??????????"))
            //    return;


            //if (string.IsNullOrEmpty(BarcodeContent) ||
            //    BarcodeContent.Contains("888888"))
            //    return;

            //try
            //{
            //    //var day =
            //    //    DateTime.Parse(string.Format(@"20{0}/{1}/{2}", BarcodeContent.Substring(12, 2),
            //    //        BarcodeContent.Substring(14, 2), BarcodeContent.Substring(16, 2)))
            //    //        .DayOfYear.ToString()
            //    //        .PadLeft(3, '0');
            //    var day = BarcodeContent.Substring(32, 3);
            //    var dayBytes = Encoding.ASCII.GetBytes(day);
            //    if (BarcodeContent.Substring(31, 1) == "W")
            //    {
            //        var yearBytes = Encoding.ASCII.GetBytes(20.ToString());
            //        var temp = dayBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            //        temp = yearBytes.Aggregate(temp, (current, t) => current + ValueHelper.GetHextStr(t));

            //        if (temp.Length != 10)
            //            return;

            //        sendByteVal = sendByteVal.Replace("??????????", temp);
            //        GetLinRecvMsg(sendByteVal);
            //    }
            //    else if (BarcodeContent.Substring(31, 1) == "X")
            //    {
            //        var yearBytes = Encoding.ASCII.GetBytes(21.ToString());
            //        var temp = dayBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            //        temp = yearBytes.Aggregate(temp, (current, t) => current + ValueHelper.GetHextStr(t));

            //        if (temp.Length != 10)
            //            return;

            //        sendByteVal = sendByteVal.Replace("??????????", temp);
            //        GetLinRecvMsg(sendByteVal);
            //    }
            //    else if (BarcodeContent.Substring(31, 1) == "Y")
            //    {
            //        var yearBytes = Encoding.ASCII.GetBytes(22.ToString());
            //        var temp = dayBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            //        temp = yearBytes.Aggregate(temp, (current, t) => current + ValueHelper.GetHextStr(t));

            //        if (temp.Length != 10)
            //            return;

            //        sendByteVal = sendByteVal.Replace("??????????", temp);
            //        GetLinRecvMsg(sendByteVal);
            //    }
            //    else if (BarcodeContent.Substring(31, 1) == "Z")
            //    {
            //        var yearBytes = Encoding.ASCII.GetBytes(23.ToString());
            //        var temp = dayBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            //        temp = yearBytes.Aggregate(temp, (current, t) => current + ValueHelper.GetHextStr(t));

            //        if (temp.Length != 10)
            //            return;

            //        sendByteVal = sendByteVal.Replace("??????????", temp);
            //        GetLinRecvMsg(sendByteVal);
            //    }

            //}
            //catch (Exception)
            //{
            //    // ignored
            //}
        }

        /// <summary>
        /// 读总成零件号
        /// </summary>
        [Description("读总成零件号")]
        public void ReadHwPn()
        {
            const string sendByteVal = "2201000000000000";
            HwPn = string.Empty;
            HwPn = GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 读生产序列号
        /// </summary>
        [Description("读生产序列号")]
        public void ReadSerialNum()
        {
            const string sendByteVal = "2203000000000000";
            SerialNum = string.Empty;
            SerialNum = GetLinRecvMsg(sendByteVal);

            //if (BarcodeContent.Contains("888888"))
            //{
            //    SerialNum = "OK";
            //    return;
            //}

            //try
            //{
            //    var temp = GetLinRecvMsg(sendByteVal);
            //    if (!temp.Substring(0, 4).Equals(sendByteVal.Substring(0, 4)) || temp.Substring(2, 2).Equals("FF"))
            //        SerialNum = string.Format("NG 回复数据不正确 {0}", temp);
            //    else
            //    {
            //        var b1 = Convert.ToByte(temp.Substring(4, 2), 16);
            //        var b2 = Convert.ToByte(temp.Substring(6, 2), 16);
            //        var b = b1 * 256 + b2;
            //        var serialNum = Convert.ToUInt16(BarcodeContent.Substring(35, 4));

            //        SerialNum = string.Format(b == serialNum ? "OK 写入{0} 读取{1} 收到数据{2}" : "NG 写入{0} 读取{1} 收到数据{2}", serialNum, b, temp);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SerialNum = string.Format("NG 读取异常 {0}", ex.Message);
            //}
        }

        /// <summary>
        /// 读生产日期
        /// </summary>
        [Description("读生产日期")]
        public void ReadManufactureData()
        {
            const string sendByteVal = "2202000000000000";
            ManufactureDate = string.Empty;
            ManufactureDate = GetLinRecvMsg(sendByteVal);
            //if (BarcodeContent.Contains("888888"))
            //{
            //    SerialNum = "OK";
            //    return;
            //}

            //try
            //{
            //    var temp = GetLinRecvMsg(sendByteVal);
            //    if (!temp.Substring(0, 4).Equals(sendByteVal.Substring(0, 4)) || temp.Substring(2, 2).Equals("FF"))
            //        ManufactureDate = string.Format("NG 回复数据不正确 {0}", temp);
            //    else
            //    {
            //        var b1 = Convert.ToByte(temp.Substring(4, 2), 16);
            //        var b2 = Convert.ToByte(temp.Substring(6, 2), 16);
            //        var b3 = Convert.ToByte(temp.Substring(8, 2), 16);
            //        var b4 = Convert.ToByte(temp.Substring(10, 2), 16);
            //        var b5 = Convert.ToByte(temp.Substring(12, 2), 16);

            //        var bDay = Encoding.ASCII.GetString(new[] { b1, b2, b3 });
            //        var bYear = Encoding.ASCII.GetString(new[] { b4, b5 });

            //        string day;
            //        string year;

            //        if (BarcodeContent.Substring(12, 2) == "88" ||
            //            BarcodeContent.Substring(14, 2) == "88" ||
            //            BarcodeContent.Substring(16, 2) == "88")
            //        {
            //            year = "88";
            //            day = "888";
            //        }
            //        else
            //        {
            //            day = BarcodeContent.Substring(32, 3);
            //            //DateTime.Parse(string.Format(@"20{0}/{1}/{2}", BarcodeContent.Substring(31, 1) == "W" ? 20 : 99,
            //            //    BarcodeContent.Substring(14, 2), BarcodeContent.Substring(16, 2)))
            //            //    .DayOfYear.ToString()
            //            //    .PadLeft(3, '0');

            //            year = 99.ToString();
            //            //BarcodeContent.Substring(33, 1) == "W" ? 20.ToString() : 99.ToString();

            //            if (BarcodeContent.Substring(31, 1) == "W")
            //                year = 20.ToString();
            //            else if (BarcodeContent.Substring(31, 1) == "X")
            //                year = 21.ToString();
            //            else if (BarcodeContent.Substring(31, 1) == "Y")
            //                year = 22.ToString();
            //            else if (BarcodeContent.Substring(31, 1) == "Z")
            //                year = 23.ToString();
            //        }

            //        if (day.Equals(bDay) && year.Equals(bYear))
            //            ManufactureDate =
            //                   string.Format("OK 写入{0}年第{1}天 读取第{2}年底{3}天 收到数据{4}", year, day, bYear, bDay, temp);
            //        else
            //            ManufactureDate =
            //                   string.Format("NG 写入{0}年第{1}天 读取第{2}年底{3}天 收到数据{4}", year, day, bYear, bDay, temp);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ManufactureDate = string.Format("NG 读取异常 {0}", ex.Message);
            //}
        }

        /// <summary>
        /// 读FBL零件号
        /// </summary>
        [Description("读FBL零件号")]
        public void ReadFblSwPn()
        {
            const string sendByteVal = "2207000000000000";
            FblSwPn = string.Empty;
            FblSwPn = GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 读应用程序零件号
        /// </summary>
        [Description("读应用程序零件号")]
        public void ReadAppSwPn()
        {
            const string sendByteVal = "2205000000000000";
            AppSwPn = string.Empty;
            AppSwPn = GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 读配置文件零件号
        /// </summary>
        [Description("读配置文件零件号")]
        public void ReadCfgPn()
        {
            const string sendByteVal = "2209000000000000";
            CfgPn = string.Empty;
            CfgPn = GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 读FBL版本号
        /// </summary>
        [Description("读FBL版本号")]
        public void ReadFblVer()
        {
            const string sendByteVal = "2208000000000000";
            FblVer = string.Empty;
            FblVer = GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 读应用程序版本号
        /// </summary>
        [Description("读应用程序版本号")]
        public void ReadAppSwVer()
        {
            const string sendByteVal = "2206000000000000";
            AppSwVer = string.Empty;
            AppSwVer = GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 配置文件版本号
        /// </summary>
        [Description("配置文件版本号")]
        public void ReadCfgVer()
        {
            const string sendByteVal = "220A000000000000";
            CfgVer = string.Empty;
            CfgVer = GetLinRecvMsg(sendByteVal);
        }

        public void WriteCustomCmd(string sendByteVal)
        {
            GetLinRecvMsg(sendByteVal);
        }

        public void ReadCustom(string sendByteVal)
        {
            //CustomRead = string.Empty;
            //CustomRead = GetLinRecvMsg(sendByteVal);
        }

        public void WriteCustomMasterCmd(string sendByteVal)
        {
            var sendBytes = new List<byte>();
            for (var i = 0; i < sendByteVal.Length; i = i + 2)
            {
                var temp = sendByteVal[i].ToString() + sendByteVal[i + 1];
                sendBytes.Add(Convert.ToByte(temp, 16));
            }

            byte[] resultBytes;
            Lin.SendMasterLinAndRecvSingleSlaveLin(_masterLinId, _slaveLinId, sendBytes.ToArray(), out resultBytes);
        }

        [Description("解锁SeedKey")]
        public bool UnlockSeedKey()
        {
            SeedKeyResult = string.Empty;

            if (Lin == null)
                return false;

            var sendSeedBytes = new byte[] { 0x27, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var getSeedStr = GetLinRecvMsg(sendSeedBytes, _masterLinId, _slaveLinId);

            if (string.IsNullOrEmpty(getSeedStr) ||
                getSeedStr.Length != 8 * 2 ||
                !getSeedStr.StartsWith("6701"))
                return false;
            var seedB1 = Convert.ToByte(
                string.Format("{0}{1}", getSeedStr[4], getSeedStr[5]), 16);
            var seedB2 = Convert.ToByte(
                string.Format("{0}{1}", getSeedStr[6], getSeedStr[7]), 16);
            var key = CommonGenerateKey(new[] { seedB1, seedB2 }).ToArray();

            var keyB1 = key[0];
            var keyB2 = key[1];

            var sendKeyBytes = new byte[] { 0x27, 0x02, keyB1, keyB2, 0x00, 0x00, 0x00, 0x00 };

            var getKeyStr = GetLinRecvMsg(sendKeyBytes, _masterLinId, _slaveLinId);

            SeedKeyResult = getKeyStr;

            return !string.IsNullOrEmpty(getKeyStr) &&
                   getKeyStr.Length == 8 * 2 &&
                   getKeyStr.StartsWith("6702");
        }

        [Description("清除结果")]
        public void ClearResult()
        {
            HwPn = string.Empty;
            SerialNum = string.Empty;
            ManufactureDate = string.Empty;
            FblSwPn = string.Empty;

            AppSwPn = string.Empty;
            CfgPn = string.Empty;
            FblVer = string.Empty;
            AppSwVer = string.Empty;
            CfgVer = string.Empty;
            MecValue = string.Empty;
            MecClearResult = string.Empty;
            SeedKeyResult = string.Empty;
        }

        private static IEnumerable<byte> CommonGenerateKey(byte[] seed)
        {
            Array.Reverse(seed);
            var securitySeed = BitConverter.ToUInt16(seed, 0);
            var securityKey = (ushort)(securitySeed >> 2);
            securityKey ^= securitySeed;
            securityKey <<= 4;
            securityKey ^= securitySeed;

            //securityKey = securityKey ^ securitySeed;
            //securityKey = securityKey << 4;
            //securityKey = securityKey ^ securitySeed;

            return BitConverter.GetBytes(securityKey).Reverse().ToArray();
        }

        [Description("R/W,芯旺微单片机配置文件路径")]
        public string ChipOniCfgFilePath =
            @"D:\Projects\LIN在线刷写\产品\A39替代料\SW0000747_C002.sx";
        //string.Empty;

        [Description("R,芯旺微单片机配置下载结果")]
        public string ChipOniCfgDownloadResult = string.Empty;

        private static readonly object FileLocker = new object();

        [Description("芯旺微单片机配置文件下载")]
        public void ChipOniCfgDownload()
        {
            ChipOniCfgDownloadResult = @"NG";

            if (Lin == null)
                return;

            if (string.IsNullOrEmpty(ChipOniCfgFilePath))
                return;

            if (!File.Exists(ChipOniCfgFilePath))
                return;

            // 10 03
            var entendedModeReq = GetLinRecvMsg(
                new byte[] { 0x36, 0x02, 0x10, 0x03, 0xff, 0xff, 0xff, 0xff }, 0x3C, 0x3D, isNeedRetry: false);
            if (!string.IsNullOrEmpty(entendedModeReq) &&
                entendedModeReq.StartsWith("36065003"))
            {
                Thread.Sleep(500);

                var isInPmMode = false;

                // 10 02
                Lin.SendMasterLin(0x3C, new byte[] { 0x36, 0x02, 0x10, 0x02, 0xff, 0xff, 0xff, 0xff });
                byte[] echo1002;
                if (Lin.SendSlaveLin(0x3D, out echo1002))
                {
                    if (echo1002 != null && echo1002.Length >= 4 && echo1002[2] == 0x50 && echo1002[3] == 0x02)
                    {
                        isInPmMode = true;
                    }
                }

                if (isInPmMode)
                {
                    // 27 01
                    var seedReq = GetLinRecvMsg(
                        new byte[] { 0x36, 0x02, 0x27, 0x01, 0xff, 0xff, 0xff, 0xff }, 0x3C, 0x3D);
                    if (!string.IsNullOrEmpty(seedReq) &&
                        seedReq.StartsWith("36066701") && seedReq.Length == 8 * 2)
                    {
                        var seed1 = Convert.ToByte(seedReq.Substring(8, 2), 16);
                        var seed2 = Convert.ToByte(seedReq.Substring(10, 2), 16);
                        var seed3 = Convert.ToByte(seedReq.Substring(12, 2), 16);
                        var seed4 = Convert.ToByte(seedReq.Substring(14, 2), 16);

                        var crc = new CrcHelper.CrcInfo
                        {
                            Width = 32,
                            Poly = 0xedb88320,
                            InitReg = 0xffffffff,
                            Refin = true,
                            Refout = true,
                            Xorout = 0xffffffff
                        };

                        var crcHelper = new CrcHelper();
                        crcHelper.CRC_Table_Init(crc);
                        var keys = BitConverter.GetBytes(crcHelper.CALC_CRC(crc, new[] { seed1, seed2, seed3, seed4 }));

                        var keyReq = GetLinRecvMsg(
                            new byte[] { 0x36, 0x06, 0x27, 0x02, keys[0], keys[1], keys[2], keys[3] }, 0x3C, 0x3D);

                        if (!string.IsNullOrEmpty(keyReq) && keyReq.StartsWith("36026702"))
                        {
                            // 写入编程日期
                            var year = Convert.ToByte(DateTime.Now.Year.ToString().Substring(2, 2), 16);
                            var month = Convert.ToByte(DateTime.Now.Month.ToString().PadLeft(2, '0'), 16);
                            var day = Convert.ToByte(DateTime.Now.ToString("dd").PadLeft(2, '0'), 16);
                            var pmDataReq = GetLinRecvMsg(
                                new byte[] { 0x36, 0x06, 0x2E, 0xF1, 0x5A, year, month, day }, 0x3C, 0x3D);
                            if (!string.IsNullOrEmpty(pmDataReq) && pmDataReq.StartsWith("36036EF15A"))
                            {
                                List<List<SRecordFileHelper.SRecordLineData>> blocks;
                                lock (FileLocker)
                                {
                                    var sRecord = SRecordFileHelper.GetSRecordLineData(ChipOniCfgFilePath);
                                    blocks = SRecordFileHelper.GetBlocks(sRecord); // Block集合
                                }

                                if (blocks.Any())
                                {
                                    var dataRecord = new List<byte>();

                                    foreach (var block in blocks)
                                    {
                                        var flashTotalBytes = new List<byte>();

                                        var startAddr = block[0].Address;
                                        var dataLen = 0;

                                        foreach (var t in block)
                                        {
                                            flashTotalBytes.AddRange(t.Data);
                                            dataRecord.AddRange(t.Data);
                                            dataLen += t.DataLen;
                                        }

                                        var routineControlOperationBytes = new List<byte> { 0x44 };
                                        //startAddr = startAddr + 0xf000;
                                        startAddr = 0x0001f000;
                                        routineControlOperationBytes.AddRange(
                                            BitConverter.GetBytes(startAddr).Reverse().ToArray());
                                        routineControlOperationBytes.AddRange(
                                            BitConverter.GetBytes(dataLen).Reverse().ToArray());

                                        Lin.SendMasterLin(0x3C,
                                            new byte[] { 0x36, 0x10, 0x0D, 0x31, 0x01, 0xFF, 0x00, routineControlOperationBytes[0] });
                                        Thread.Sleep(5);
                                        Lin.SendMasterLin(0x3C,
                                            new byte[]
                                            {
                                                0x36, 0x21,
                                                routineControlOperationBytes[1],
                                                routineControlOperationBytes[2],
                                                routineControlOperationBytes[3],
                                                routineControlOperationBytes[4],
                                                routineControlOperationBytes[5],
                                                routineControlOperationBytes[6]
                                            });
                                        Thread.Sleep(5);

                                        var eraseReq = GetLinRecvMsg(
                                            new byte[] { 0x36, 0x22, routineControlOperationBytes[7], routineControlOperationBytes[8], 0xFF, 0xFF, 0xFF, 0xFF },
                                            0x3C, 0x3D, 500);

                                        if (string.IsNullOrEmpty(eraseReq) || !eraseReq.StartsWith("36037F3178"))
                                            return;

                                        //eraseReq = GetLinRecvMsg(
                                        //    new byte[] { 0x36, 0x22, routineControlOperationBytes[7], routineControlOperationBytes[8], 0xFF, 0xFF, 0xFF, 0xFF },
                                        //    0x3C, 0x3D, 500);

                                        byte[] echo;
                                        if (Lin.SendSlaveLin(0x3D, out echo))
                                        {
                                            if (echo != null && echo.Length == 8 && echo[0] == 0x36 && echo[1] == 0x05 && echo[2] == 0x71 && echo[3] == 0x01 && echo[4] == 0xFF)
                                            {
                                                //if (!string.IsNullOrEmpty(eraseReq) && eraseReq.StartsWith("36057101FF00"))
                                                {
                                                    var sendBytes = new List<byte>
                                                    {
                                                        0x34,
                                                        0x00,
                                                        0x44
                                                    };
                                                    sendBytes.AddRange(BitConverter.GetBytes(startAddr).ToArray().Reverse()); // m个字节的memoryAddress，由addressAndLengthFormatIdentifier中的低4bit指示。含义是要写入数据在ECU中的逻辑地址
                                                    sendBytes.AddRange(BitConverter.GetBytes((uint)dataLen).ToArray().Reverse()); // n个字节的memorySize，由addressAndLengthFormatIdentifier中的高4bit指示。含义是要写入数据的字节数

                                                    Lin.SendMasterLin(0x3C,
                                                        new byte[] { 0x36, 0x10, 0x0B, sendBytes[0], sendBytes[1], sendBytes[2], sendBytes[3], sendBytes[4] });

                                                    var dowloadReq =
                                                        GetLinRecvMsg(
                                                            new byte[]
                                                            {
                                                                0x36, 0x21, sendBytes[5], sendBytes[6], sendBytes[7],
                                                                sendBytes[8], sendBytes[9], sendBytes[10]
                                                            },
                                                            0x3C, 0x3D);

                                                    if (!string.IsNullOrEmpty(dowloadReq) && dowloadReq.StartsWith("360474"))
                                                    {
                                                        int total36Count;
                                                        if (flashTotalBytes.Count <= 256)
                                                        {
                                                            total36Count = 1;
                                                        }
                                                        else
                                                        {
                                                            if (flashTotalBytes.Count % 256 == 0)
                                                            {
                                                                total36Count = flashTotalBytes.Count / 256;
                                                            }
                                                            else
                                                            {
                                                                total36Count = flashTotalBytes.Count / 256 + 1;
                                                            }
                                                        }

                                                        var baseDataIndex = 0;
                                                        if (flashTotalBytes.Count >= 256)
                                                        {
                                                            for (var i = 0; i < total36Count; i++)
                                                            {
                                                                Lin.SendMasterLin(0x3C,
                                                                    new byte[]
                                                                {
                                                                    0x36, 0x11, 0x02, 0x36, (byte) (i + 1),
                                                                    flashTotalBytes[0],
                                                                    flashTotalBytes[1], flashTotalBytes[2]
                                                                });

                                                                baseDataIndex = baseDataIndex + 3;

                                                                var byte1 = 0x21;
                                                                for (var j = 0; j < 42; j++)
                                                                {
                                                                    Lin.SendMasterLin(0x3C,
                                                                        new byte[]
                                                                    {
                                                                        0x36, (byte) byte1,
                                                                        baseDataIndex <= flashTotalBytes.Count - 1
                                                                            ? flashTotalBytes[baseDataIndex]
                                                                            : (byte) 0xFF,
                                                                        baseDataIndex + 1 <= flashTotalBytes.Count - 1
                                                                            ? flashTotalBytes[baseDataIndex + 1]
                                                                            : (byte) 0xFF,
                                                                        baseDataIndex + 2 <= flashTotalBytes.Count - 1
                                                                            ? flashTotalBytes[baseDataIndex + 2]
                                                                            : (byte) 0xFF,
                                                                        baseDataIndex + 3 <= flashTotalBytes.Count - 1
                                                                            ? flashTotalBytes[baseDataIndex + 3]
                                                                            : (byte) 0xFF,
                                                                        baseDataIndex + 4 <= flashTotalBytes.Count - 1
                                                                            ? flashTotalBytes[baseDataIndex + 4]
                                                                            : (byte) 0xFF,
                                                                        baseDataIndex + 5 <= flashTotalBytes.Count - 1
                                                                            ? flashTotalBytes[baseDataIndex + 5]
                                                                            : (byte) 0xFF
                                                                    });

                                                                    baseDataIndex = baseDataIndex + 6;

                                                                    byte1 = byte1 + 0x01;
                                                                    if (byte1 > 0x2F)
                                                                        byte1 = 0x20;
                                                                }

                                                                var transferReq = GetLinRecvMsg(
                                                                    new byte[]
                                                                {
                                                                    0x36, (byte) byte1,
                                                                    baseDataIndex <= flashTotalBytes.Count - 1
                                                                        ? flashTotalBytes[baseDataIndex]
                                                                        : (byte) 0xFF,
                                                                    0xFF, 0xFF, 0xFF, 0xFF, 0xFF
                                                                },
                                                                    0x3C, 0x3D);

                                                                if (string.IsNullOrEmpty(transferReq) ||
                                                                    !transferReq.StartsWith("360276"))
                                                                    return;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            int rows;
                                                            if ((flashTotalBytes.Count - 3) % 6 == 0)
                                                                rows = (flashTotalBytes.Count - 3) / 6;
                                                            else
                                                                rows = (flashTotalBytes.Count - 3) / 6 + 1;

                                                            var lenOf36 =
                                                                BitConverter.GetBytes(
                                                                    (ushort)(flashTotalBytes.Count + 2));

                                                            Lin.SendMasterLin(0x3C,
                                                                    new byte[]
                                                                {
                                                                    0x36, (byte)(0x10+lenOf36[1]), lenOf36[0], 0x36, 0x01,
                                                                    flashTotalBytes[0],
                                                                    flashTotalBytes[1], flashTotalBytes[2]
                                                                });

                                                            baseDataIndex = baseDataIndex + 3;

                                                            var byte1 = 0x21;
                                                            for (var i = 0; i < rows - 1; i++)
                                                            {
                                                                Lin.SendMasterLin(0x3C,
                                                                        new byte[]
                                                                    {
                                                                        0x36,
                                                                        (byte) byte1,
                                                                        baseDataIndex <= flashTotalBytes.Count - 1
                                                                            ? flashTotalBytes[baseDataIndex]
                                                                            : (byte) 0xFF,
                                                                        baseDataIndex + 1 <= flashTotalBytes.Count - 1
                                                                            ? flashTotalBytes[baseDataIndex + 1]
                                                                            : (byte) 0xFF,
                                                                        baseDataIndex + 2 <= flashTotalBytes.Count - 1
                                                                            ? flashTotalBytes[baseDataIndex + 2]
                                                                            : (byte) 0xFF,
                                                                        baseDataIndex + 3 <= flashTotalBytes.Count - 1
                                                                            ? flashTotalBytes[baseDataIndex + 3]
                                                                            : (byte) 0xFF,
                                                                        baseDataIndex + 4 <= flashTotalBytes.Count - 1
                                                                            ? flashTotalBytes[baseDataIndex + 4]
                                                                            : (byte) 0xFF,
                                                                        baseDataIndex + 5 <= flashTotalBytes.Count - 1
                                                                            ? flashTotalBytes[baseDataIndex + 5]
                                                                            : (byte) 0xFF
                                                                    });

                                                                baseDataIndex = baseDataIndex + 6;

                                                                byte1 = byte1 + 0x01;
                                                                if (byte1 > 0x2F)
                                                                    byte1 = 0x20;
                                                            }

                                                            var transferReq = GetLinRecvMsg(
                                                                new byte[]
                                                                {
                                                                    0x36,
                                                                    (byte) byte1,
                                                                    baseDataIndex <= flashTotalBytes.Count - 1
                                                                        ? flashTotalBytes[baseDataIndex]
                                                                        : (byte) 0xFF,
                                                                    baseDataIndex + 1 <= flashTotalBytes.Count - 1
                                                                        ? flashTotalBytes[baseDataIndex + 1]
                                                                        : (byte) 0xFF,
                                                                    baseDataIndex + 2 <= flashTotalBytes.Count - 1
                                                                        ? flashTotalBytes[baseDataIndex + 2]
                                                                        : (byte) 0xFF,
                                                                    baseDataIndex + 3 <= flashTotalBytes.Count - 1
                                                                        ? flashTotalBytes[baseDataIndex + 3]
                                                                        : (byte) 0xFF,
                                                                    baseDataIndex + 4 <= flashTotalBytes.Count - 1
                                                                        ? flashTotalBytes[baseDataIndex + 4]
                                                                        : (byte) 0xFF,
                                                                    baseDataIndex + 5 <= flashTotalBytes.Count - 1
                                                                        ? flashTotalBytes[baseDataIndex + 5]
                                                                        : (byte) 0xFF,
                                                                },
                                                                0x3C, 0x3D, isNeedRetry: false);

                                                            if (string.IsNullOrEmpty(transferReq) ||
                                                                !transferReq.StartsWith("360276"))
                                                                return;
                                                        }

                                                        var outTransferReq =
                                                            GetLinRecvMsg(
                                                                new byte[] { 0x36, 0x01, 0x37, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 0x3C,
                                                                0x3D);
                                                        if (!string.IsNullOrEmpty(outTransferReq) && outTransferReq.StartsWith("360177"))
                                                        {
                                                            //var dataSum = flashTotalBytes.Aggregate(0x00, (current, t) => current + t);
                                                            //var crc31 =
                                                            //    BitConverter.GetBytes(crcHelper.CALC_CRC(crc,
                                                            //        BitConverter.GetBytes((uint) dataSum)));

                                                            var checkProgrammingReq =
                                                                GetLinRecvMsg(
                                                                    new byte[]
                                                                    {
                                                                        0x36, 0x04, 0x31, 0x01, 0xFF, 0x01, 0xFF, 0xFF
                                                                    },
                                                                    0x3C, 0x3D);

                                                            if (!string.IsNullOrEmpty(checkProgrammingReq) &&
                                                                checkProgrammingReq.StartsWith("36057101FF01"))
                                                            {
                                                                var ecuResetReq = GetLinRecvMsg(
                                                                    new byte[]
                                                                    {
                                                                        0x36, 0x02, 0x11, 0x01, 0xFF, 0xFF, 0xFF, 0xFF
                                                                    },
                                                                    0x3C, 0x3D);
                                                                ChipOniCfgDownloadResult = @"OK";
                                                                //if (!string.IsNullOrEmpty(ecuResetReq) &&
                                                                //    ecuResetReq.StartsWith("36025101"))
                                                                //{

                                                                //}
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
