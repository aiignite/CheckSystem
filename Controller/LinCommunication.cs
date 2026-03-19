using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;
using DBUtility;

namespace Controller
{
    public sealed class LinCommunication : ControllerBase
    {
        public LinBus Lin;
        private byte _masterLinId;
        private byte _slaveLinId;

        private readonly Dictionary<string, SRecordFileHelper.SRecordLineData[]> _downloadDataList =
            new Dictionary<string, SRecordFileHelper.SRecordLineData[]>();

        /// <summary>
        /// 条码内容
        /// </summary>
        public string BarcodeContent;

        /// <summary>
        /// 下载结果
        /// </summary>
        public string DownLoadResult;

        /// <summary>
        /// 总成零件号
        /// </summary>
        public string HwPn;

        /// <summary>
        /// 生产序列号
        /// </summary>
        public string SerialNum;

        /// <summary>
        /// 生产日期
        /// </summary>
        public string ManufactureDate;

        /// <summary>
        /// FBL零件号
        /// </summary>
        public string FblSwPn;

        /// <summary>
        /// 应用程序零件号
        /// </summary>
        public string AppSwPn;

        /// <summary>
        /// 配置文件零件号
        /// </summary>
        public string CfgPn;

        /// <summary>
        /// FBL版本号
        /// </summary>
        public string FblVer;

        /// <summary>
        /// 应用程序版本号
        /// </summary>
        public string AppSwVer;

        /// <summary>
        /// 配置文件版本号
        /// </summary>
        public string CfgVer;

        /// <summary>
        /// 自定义读取内容
        /// </summary>
        public string CustomRead;

        public LinCommunication(string name)
            : base(name) { }

        public void InitMasterLinId(string masterLinId)
        {
            _masterLinId = Convert.ToByte(masterLinId, 16);
        }

        public void InitSlavaLinId(string slaveLinId)
        {
            _slaveLinId = Convert.ToByte(slaveLinId, 16);
        }

        public void ReadDownLoadFile(string filePath)
        {
            if (!filePath.EndsWith(".sx") &&
                !filePath.EndsWith(".s19") &&
                !filePath.EndsWith(".s2") &&
                !filePath.EndsWith(".s1"))
                return;

            if (!File.Exists(filePath))
                return;

            var fileInfo = new FileInfo(filePath);

            if (_downloadDataList.ContainsKey(fileInfo.Name))
                return;

            var sRecordLines = SRecordFileHelper.GetSRecordLineData(filePath);
            var downloadData =
                sRecordLines.Where(
                    t =>
                        t.Type == SRecordFileHelper.SRecordType.S1 ||
                        t.Type == SRecordFileHelper.SRecordType.S2 ||
                        t.Type == SRecordFileHelper.SRecordType.S3).ToList();
            _downloadDataList.Add(fileInfo.Name, downloadData.ToArray());
        }

        public void DownloadByVersion1(string fileName)
        {
            DownLoadResult = string.Empty;

            if (!_downloadDataList.ContainsKey(fileName))
            {
                DownLoadResult = "NG";
                return;
            }

            if (!ConnectStatusCheck(1500, _masterLinId, _slaveLinId))
            {
                DownLoadResult = "NG";
            }
            else
            {
                Thread.Sleep(50);

                var downloadData = _downloadDataList[fileName].ToList();

                // EEPROM Erase Request(0x1B/0x08/0x10/0x11/0x12/0x13/dtat[5]/0x0/0x0)
                // Data[5]中是解析为S19文件后S2行总行数，>=100ms
                var erasingEepromRequestResult =
                    GetLinRecvMsg(new byte[] { 0x08, 0x10, 0x11, 0x12, 0x13, (byte)downloadData.Count, 0x00, 0x00 },
                        _masterLinId, _slaveLinId, 500);

                if (string.IsNullOrEmpty(erasingEepromRequestResult) ||
                    !erasingEepromRequestResult.StartsWith("08FF"))
                {
                    DownLoadResult = "NG EPROM Erase Request";
                    return;
                }

                Thread.Sleep(200);

                foreach (var t in downloadData)
                {
                    Thread.Sleep(100);

                    // Data[1]是解析为S19文件后每一个S2行首地址的16～23位， 
                    // data[2]是首地址的8～15位， 
                    // data[3]是首地址的0～7位，
                    // data[4]是这个S2的数据长度， 
                    // data[5]是这个S2行的校验和
                    var addrBytes = BitConverter.GetBytes(t.Address);
                    var dataSum = t.Data.Aggregate(0, (current, d) => current + d);
                    var programmingEepromResult =
                        GetLinRecvMsg(
                            new byte[] { 0x09, addrBytes[2], addrBytes[1], addrBytes[0], t.DataLen, (byte)dataSum, 0x00, 0x00 },
                            _masterLinId, _slaveLinId, 200);

                    if (string.IsNullOrEmpty(programmingEepromResult) ||
                        !programmingEepromResult.StartsWith("09FF"))
                    {
                        DownLoadResult = "NG EEPROM Program Information";
                        return;
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
                        var thisFrameDatas = new List<byte> { 0x11, (byte)i };
                        for (var j = 0; j < 6; j++)
                            thisFrameDatas.Add(sendDatas[6 * i + j]);
                        Lin.SendMasterLin(_masterLinId, thisFrameDatas.ToArray());

                        Thread.Sleep(50);
                    }

                    Thread.Sleep(120);

                    byte[] eepromInformationRequestResult;
                    if (!Lin.SendSlaveLin(_slaveLinId, out eepromInformationRequestResult) ||
                        eepromInformationRequestResult == null ||
                        eepromInformationRequestResult.Length != 8)
                    {
                        DownLoadResult = "NG EEPROM Program";
                        return;
                    }

                    if (eepromInformationRequestResult[0] == 0x11 &&
                        eepromInformationRequestResult[1] == 0xFF)
                    {

                    }
                    else
                    {
                        DownLoadResult = "NG EEPROM Program";
                        return;
                    }
                }

                var eepromWorkFlagRequestResult =
                    GetLinRecvMsg(new byte[] { 0x12, 0x02, 0x03, 0x04, 0x05, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);

                if (string.IsNullOrEmpty(eepromWorkFlagRequestResult) ||
                    !eepromWorkFlagRequestResult.StartsWith("12FF"))
                {
                    DownLoadResult = "NG EEPROM work flag Request";
                    return;
                }

                var integrityDetectRequestResult =
                    GetLinRecvMsg(new byte[] { 0x02, 0x18, 0x19, 0x20, 0x21, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);

                if (string.IsNullOrEmpty(integrityDetectRequestResult) ||
                    !integrityDetectRequestResult.StartsWith("02FF"))
                {
                    DownLoadResult = "NG Integrity detect Request";
                    return;
                }

                DownLoadResult = "OK";
            }
        }

        public void DownloadByVersion2(string fileName)
        {
            DownLoadResult = string.Empty;

            if (!_downloadDataList.ContainsKey(fileName))
            {
                DownLoadResult = "NG";
                return;
            }

            if (!ConnectStatusCheck(1500, _masterLinId, _slaveLinId))
            {
                DownLoadResult = "NG";
            }
            else
            {
                var downloadData = _downloadDataList[fileName].ToList();

                foreach (var t in downloadData)
                {
                    Thread.Sleep(100);

                    // Data[1]是解析为S19文件后每一个S2行首地址的24～31位， 
                    // data[2]是首地址的16～23位， data[3]是首地址的8～15位， 
                    // data[4]是首地址的0～7位， 
                    // data[5]是这个S2的数据长度，
                    // data[6是这个S2行的数据和,
                    // data[7]是这个S2行的总行数
                    var addrBytes = BitConverter.GetBytes(t.Address);
                    var dataSum = t.Data.Aggregate(0, (current, d) => current + d);
                    var programmingEepromResult =
                        GetLinRecvMsg(
                            new byte[]
                        {
                            0x09, addrBytes[3], addrBytes[2], addrBytes[1], addrBytes[0], t.DataLen, (byte) dataSum,
                            (byte) downloadData.Count
                        }, _masterLinId, _slaveLinId, 100);

                    if (string.IsNullOrEmpty(programmingEepromResult) ||
                        !programmingEepromResult.StartsWith("09FF"))
                    {
                        DownLoadResult = "NG EEPROM Program Information";
                        return;
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
                        var thisFrameDatas = new List<byte> { 0x10, (byte)i };
                        for (var j = 0; j < 6; j++)
                            thisFrameDatas.Add(sendDatas[6 * i + j]);
                        Lin.SendMasterLin(_masterLinId, thisFrameDatas.ToArray());
                        Thread.Sleep(100);
                    }

                    Thread.Sleep(500);

                    byte[] eepromInformationRequestResult;
                    if (!Lin.SendSlaveLin(_slaveLinId, out eepromInformationRequestResult) ||
                        eepromInformationRequestResult == null ||
                        eepromInformationRequestResult.Length != 8)
                    {
                        DownLoadResult = "NG EEPROM Program";
                        return;
                    }

                    if (eepromInformationRequestResult[0] == 0x10 &&
                        eepromInformationRequestResult[1] == 0xFF)
                    {

                    }
                    else
                    {
                        DownLoadResult = "NG EEPROM Program";
                        return;
                    }
                }

                DownLoadResult = "OK";
            }
        }

        public bool ConnectStatusCheck(int tryCount, byte masterLinId, byte slaveLinId)
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
                        GetLinRecvMsg(connectStatusCheckBytes, masterLinId, slaveLinId);
                    if (!string.IsNullOrEmpty(result) &&
                        result.Length == 8 * 2 &&
                        result.StartsWith("01FF"))
                        return true;

                    Thread.Sleep(50);
                }

                return false;
            }
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
            byte[] sendBytes, byte masterLinId, byte slaveLinId, int delayMs = 50)
        {
            byte[] resultBytes;
            if (Lin.SendMasterLinAndRecvSingleSlaveLin(
                masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
            {
                if (resultBytes != null && resultBytes.Length == 8)
                    return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));

                Thread.Sleep(500);
                if (!Lin.SendMasterLinAndRecvSingleSlaveLin(
                    masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
                    return string.Empty;

                if (resultBytes != null && resultBytes.Length == 8)
                    return resultBytes.Aggregate(string.Empty,
                        (current, t) => current + ValueHelper.GetHextStr(t));
            }
            else
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

        /// <summary>
        /// 写总成零件号
        /// </summary>
        public void WriteHwPn(string sendByteVal)
        {
            GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 写生产序列号
        /// </summary>
        public void WriteSerialNum(string sendByteVal)
        {
            if (string.IsNullOrEmpty(BarcodeContent))
                return;

            if (!sendByteVal.Contains("????"))
                return;

            if (string.IsNullOrEmpty(BarcodeContent) ||
               BarcodeContent.Contains("888888"))
                return;

            try
            {
                var serialNum = Convert.ToUInt16(BarcodeContent.Substring(35, 4));
                var serialNumBytes = BitConverter.GetBytes(serialNum);
                Array.Reverse(serialNumBytes);

                var temp = serialNumBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                if (temp.Length != 4)
                    return;

                sendByteVal = sendByteVal.Replace("????", temp);
                GetLinRecvMsg(sendByteVal);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 写生产日期
        /// </summary>
        public void WriteManufactureDate(string sendByteVal)
        {
            if (string.IsNullOrEmpty(BarcodeContent))
                return;

            if (!sendByteVal.Contains("??????????"))
                return;


            if (string.IsNullOrEmpty(BarcodeContent) ||
                BarcodeContent.Contains("888888"))
                return;

            try
            {
                //var day =
                //    DateTime.Parse(string.Format(@"20{0}/{1}/{2}", BarcodeContent.Substring(12, 2),
                //        BarcodeContent.Substring(14, 2), BarcodeContent.Substring(16, 2)))
                //        .DayOfYear.ToString()
                //        .PadLeft(3, '0');
                var day = BarcodeContent.Substring(32, 3);
                var dayBytes = Encoding.ASCII.GetBytes(day);
                if (BarcodeContent.Substring(31, 1) == "W")
                {
                    var yearBytes = Encoding.ASCII.GetBytes(20.ToString());
                    var temp = dayBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                    temp = yearBytes.Aggregate(temp, (current, t) => current + ValueHelper.GetHextStr(t));

                    if (temp.Length != 10)
                        return;

                    sendByteVal = sendByteVal.Replace("??????????", temp);
                    GetLinRecvMsg(sendByteVal);
                }
                else if (BarcodeContent.Substring(31, 1) == "X")
                {
                    var yearBytes = Encoding.ASCII.GetBytes(21.ToString());
                    var temp = dayBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                    temp = yearBytes.Aggregate(temp, (current, t) => current + ValueHelper.GetHextStr(t));

                    if (temp.Length != 10)
                        return;

                    sendByteVal = sendByteVal.Replace("??????????", temp);
                    GetLinRecvMsg(sendByteVal);
                }
                else if (BarcodeContent.Substring(31, 1) == "Y")
                {
                    var yearBytes = Encoding.ASCII.GetBytes(22.ToString());
                    var temp = dayBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                    temp = yearBytes.Aggregate(temp, (current, t) => current + ValueHelper.GetHextStr(t));

                    if (temp.Length != 10)
                        return;

                    sendByteVal = sendByteVal.Replace("??????????", temp);
                    GetLinRecvMsg(sendByteVal);
                }
                else if (BarcodeContent.Substring(31, 1) == "Z")
                {
                    var yearBytes = Encoding.ASCII.GetBytes(23.ToString());
                    var temp = dayBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                    temp = yearBytes.Aggregate(temp, (current, t) => current + ValueHelper.GetHextStr(t));

                    if (temp.Length != 10)
                        return;

                    sendByteVal = sendByteVal.Replace("??????????", temp);
                    GetLinRecvMsg(sendByteVal);
                }

            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 读总成零件号
        /// </summary>
        public void ReadHwPn(string sendByteVal)
        {
            HwPn = string.Empty;
            HwPn = GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 读生产序列号
        /// </summary>
        public void ReadSerialNum(string sendByteVal)
        {
            SerialNum = string.Empty;

            if (BarcodeContent.Contains("888888"))
            {
                SerialNum = "OK";
                return;
            }

            try
            {
                var temp = GetLinRecvMsg(sendByteVal);
                if (!temp.Substring(0, 4).Equals(sendByteVal.Substring(0, 4)) || temp.Substring(2, 2).Equals("FF"))
                    SerialNum = string.Format("NG 回复数据不正确 {0}", temp);
                else
                {
                    var b1 = Convert.ToByte(temp.Substring(4, 2), 16);
                    var b2 = Convert.ToByte(temp.Substring(6, 2), 16);
                    var b = b1 * 256 + b2;
                    var serialNum = Convert.ToUInt16(BarcodeContent.Substring(35, 4));

                    SerialNum = string.Format(b == serialNum ? "OK 写入{0} 读取{1} 收到数据{2}" : "NG 写入{0} 读取{1} 收到数据{2}", serialNum, b, temp);
                }
            }
            catch (Exception ex)
            {
                SerialNum = string.Format("NG 读取异常 {0}", ex.Message);
            }
        }

        /// <summary>
        /// 读生产日期
        /// </summary>
        public void ReadManufactureData(string sendByteVal)
        {
            ManufactureDate = string.Empty;

            if (BarcodeContent.Contains("888888"))
            {
                SerialNum = "OK";
                return;
            }

            try
            {
                var temp = GetLinRecvMsg(sendByteVal);
                if (!temp.Substring(0, 4).Equals(sendByteVal.Substring(0, 4)) || temp.Substring(2, 2).Equals("FF"))
                    ManufactureDate = string.Format("NG 回复数据不正确 {0}", temp);
                else
                {
                    var b1 = Convert.ToByte(temp.Substring(4, 2), 16);
                    var b2 = Convert.ToByte(temp.Substring(6, 2), 16);
                    var b3 = Convert.ToByte(temp.Substring(8, 2), 16);
                    var b4 = Convert.ToByte(temp.Substring(10, 2), 16);
                    var b5 = Convert.ToByte(temp.Substring(12, 2), 16);

                    var bDay = Encoding.ASCII.GetString(new[] { b1, b2, b3 });
                    var bYear = Encoding.ASCII.GetString(new[] { b4, b5 });

                    string day;
                    string year;

                    if (BarcodeContent.Substring(12, 2) == "88" ||
                        BarcodeContent.Substring(14, 2) == "88" ||
                        BarcodeContent.Substring(16, 2) == "88")
                    {
                        year = "88";
                        day = "888";
                    }
                    else
                    {
                        day = BarcodeContent.Substring(32, 3);
                        //DateTime.Parse(string.Format(@"20{0}/{1}/{2}", BarcodeContent.Substring(31, 1) == "W" ? 20 : 99,
                        //    BarcodeContent.Substring(14, 2), BarcodeContent.Substring(16, 2)))
                        //    .DayOfYear.ToString()
                        //    .PadLeft(3, '0');

                        year = 99.ToString();
                        //BarcodeContent.Substring(33, 1) == "W" ? 20.ToString() : 99.ToString();

                        if (BarcodeContent.Substring(31, 1) == "W")
                            year = 20.ToString();
                        else if (BarcodeContent.Substring(31, 1) == "X")
                            year = 21.ToString();
                        else if (BarcodeContent.Substring(31, 1) == "Y")
                            year = 22.ToString();
                        else if (BarcodeContent.Substring(31, 1) == "Z")
                            year = 23.ToString();
                    }

                    if (day.Equals(bDay) && year.Equals(bYear))
                        ManufactureDate =
                               string.Format("OK 写入{0}年第{1}天 读取第{2}年底{3}天 收到数据{4}", year, day, bYear, bDay, temp);
                    else
                        ManufactureDate =
                               string.Format("NG 写入{0}年第{1}天 读取第{2}年底{3}天 收到数据{4}", year, day, bYear, bDay, temp);
                }
            }
            catch (Exception ex)
            {
                ManufactureDate = string.Format("NG 读取异常 {0}", ex.Message);
            }
        }

        /// <summary>
        /// 读FBL零件号
        /// </summary>
        public void ReadFblSwPn(string sendByteVal)
        {
            FblSwPn = string.Empty;
            FblSwPn = GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 读应用程序零件号
        /// </summary>
        public void ReadAppSwPn(string sendByteVal)
        {
            AppSwPn = string.Empty;
            AppSwPn = GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 读配置文件零件号
        /// </summary>
        public void ReadCfgPn(string sendByteVal)
        {
            CfgPn = string.Empty;
            CfgPn = GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 读FBL版本号
        /// </summary>
        public void ReadFblVer(string sendByteVal)
        {
            FblVer = string.Empty;
            FblVer = GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 读应用程序版本号
        /// </summary>
        public void ReadAppSwVer(string sendByteVal)
        {
            AppSwVer = string.Empty;
            AppSwVer = GetLinRecvMsg(sendByteVal);
        }

        /// <summary>
        /// 配置文件版本号
        /// </summary>
        public void ReadCfgVer(string sendByteVal)
        {
            CfgVer = string.Empty;
            CfgVer = GetLinRecvMsg(sendByteVal);
        }

        public void WriteCustomCmd(string sendByteVal)
        {
            GetLinRecvMsg(sendByteVal);
        }

        public void ReadCustom(string sendByteVal)
        {
            CustomRead = string.Empty;
            CustomRead = GetLinRecvMsg(sendByteVal);
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

        public void GenerateBarcode(string value)
        {
            BarcodeContent = string.Empty;

            if (string.IsNullOrEmpty(value))
                return;

            var sp = value.Split('/');
            if (sp.Length != 3)
                return;

            var partNo = sp[0];
            var hardwarePartNo = sp[1];
            var softwarePartNo = sp[2];

            if (string.IsNullOrEmpty(partNo) || partNo.Length != 9)
                return;

            if (string.IsNullOrEmpty(hardwarePartNo) || hardwarePartNo.Length != 4 || !hardwarePartNo.StartsWith("H"))
                return;

            if (string.IsNullOrEmpty(softwarePartNo) || softwarePartNo.Length != 4 || !softwarePartNo.StartsWith("S"))
                return;

            try
            {
                using (var conn = new SqlConnection(PubConstant.ConnectionString))
                //using (var conn = new SqlConnection("server=.;database=IPMS;uid=sa;pwd=123456"))
                {
                    var cmd = new SqlCommand("GetCheckNo", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@Productno", partNo);  //给输入参数赋值
                    //cmd.Parameters.AddWithValue("@checkStaff", "admin");  //给输入参数赋值
                    //cmd.Parameters.AddWithValue("@creater", "admin");  //给输入参数赋值
                    //var parOutputSerialNo = cmd.Parameters.Add("@serialNumber", SqlDbType.Int);  //定义输出参数 
                    //parOutputSerialNo.Direction = ParameterDirection.Output;  //参数类型为Output  
                    var parOutputDate = cmd.Parameters.Add("@date", SqlDbType.DateTime);  //定义输出参数 
                    parOutputDate.Direction = ParameterDirection.Output;  //参数类型为Output 
                    var returnSerialNo = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnSerialNo.Direction = ParameterDirection.ReturnValue;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    var serialNo = returnSerialNo.Value.ToString();
                    var date = parOutputDate.Value.ToString();
                    //serialNp = parOutputSerialNo.Value.ToString();
                    //date = parOutputDate.Value.ToString();
                    //MessageBox.Show(parOutputSerialNo.Value.ToString());   //显示输出参数的值  
                    //MessageBox.Show(parOutputDate.Value.ToString());  //显示返回值  

                    if (string.IsNullOrEmpty(serialNo) || string.IsNullOrEmpty(date))
                        return;

                    var currentDateTime = TimeSpan.Parse(DateTime.Parse(date).ToShortTimeString());
                    string shift;

                    if (currentDateTime > TimeSpan.Parse("00:00") && currentDateTime <= TimeSpan.Parse("12:30"))
                        shift = "M";
                    else if (currentDateTime > TimeSpan.Parse("12:30") && currentDateTime <= TimeSpan.Parse("17:00"))
                        shift = "A";
                    else
                        shift = "N";

                    var currentYear = DateTime.Parse(date).Year;
                    var year = string.Empty;
                    if (currentYear.Equals(2018))
                        year = "U";
                    else if (currentYear.Equals(2019))
                        year = "V";
                    else if (currentYear.Equals(2020))
                        year = "W";
                    else if (currentYear.Equals(2021))
                        year = "X";
                    else if (currentYear.Equals(2022))
                        year = "Y";
                    else if (currentYear.Equals(2023))
                        year = "Z";
                    else if (currentYear.Equals(2024))
                        year = "A";
                    else if (currentYear.Equals(2025))
                        year = "B";
                    else if (currentYear.Equals(2026))
                        year = "C";
                    else if (currentYear.Equals(2027))
                        year = "D";
                    else if (currentYear.Equals(2028))
                        year = "E";
                    else if (currentYear.Equals(2029))
                        year = "F";
                    else if (currentYear.Equals(2030))
                        year = "G";
                    else if (currentYear.Equals(2031))
                        year = "H";

                    var day = DateTime.Parse(date).DayOfYear.ToString().PadLeft(3, '0');
                    var serialNumber = (int.Parse(serialNo) + 7000).ToString().PadLeft(4, '0');

                    var listBytes = new List<byte>();
                    listBytes.AddRange(new byte[] { 0x5B, 0x29, 0x3E, 0x1E, 0x30, 0x36, 0x1D });
                    listBytes.AddRange(Encoding.ASCII.GetBytes(partNo));
                    listBytes.AddRange(new byte[] { 0x1D, 0x31, 0x32, 0x56 });
                    listBytes.AddRange(Encoding.ASCII.GetBytes(hardwarePartNo));
                    listBytes.AddRange(Encoding.ASCII.GetBytes(softwarePartNo));
                    listBytes.AddRange(new byte[] { 0x1D });
                    listBytes.AddRange(new byte[] { 0x4E });
                    listBytes.AddRange(Encoding.ASCII.GetBytes(shift));
                    listBytes.AddRange(Encoding.ASCII.GetBytes(year));
                    listBytes.AddRange(Encoding.ASCII.GetBytes(day));
                    listBytes.AddRange(Encoding.ASCII.GetBytes(serialNumber));
                    listBytes.AddRange(new byte[] { 0x1E, 0x04 });

                    BarcodeContent = Encoding.ASCII.GetString(listBytes.ToArray());
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
