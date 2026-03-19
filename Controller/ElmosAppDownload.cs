using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,ELMOSAppDownload")]
    public sealed class ElmosAppDownLoad : ControllerBase
    {
        public LinBus Lin;

        [Description("R,刷新结果")]
        public string DatDownloadResult = string.Empty;

        [Description("R,读取生产日期")]
        public string ReadProductData = string.Empty;

        [Description("R,读取生产序列号")]
        public string ReadProductSerialNo = string.Empty;

        [Description("R,读取硬件版本号")]
        public string ReadHardwareNo = string.Empty;

        [Description("R,读取供货零件号")]
        public string ReadProductNo = string.Empty;

        [Description("R,读取节点配置")]
        public string ReadSlaveConfig = string.Empty;

        [Description("R/W,基础序列号")]
        public int BaseSerialNoIndex = 0;

        //[Description("R/W,Dat文件路径")]
        //public string DatFilePath = string.Empty;

        //[Description("R/W,BmmLinCmdId")]
        //public string BmmLinCmdId = string.Empty;

        //[Description("R/W,BmmLinAnsId")]
        //public string BmmLinAnsId = string.Empty;

        //[Description("R/W,BmmLinInitialNad")]
        //public string BmmLinInitialNad = string.Empty;

        //[Description("R/W,硬件版本号")]
        //public string HardWareVersion = string.Empty;

        //[Description("R/W,供货零件号")]
        //public string ProductNo = string.Empty;

        [Description("R/W,Dat文件路径")]
        public string DatFilePath =
            //@"D:\Projects\EP33系列\调光执行器\S00020886_EP33L233_SW0000946_A001\S00020886_EP33L233_SW0000946_A001.dat";
            //@"D:\Projects\EP33系列\调光执行器\file\E2LB\E2LB_V0.8(SVN_26).dat";
            //@"C:\Projs\2022\ES33调光执行器\ES33_SMC_APP_V1.1.01_20220826_Test.dat";
            @"E:\Projects-2022\S11L马达支架\S11L_SMC_APP_V1.0.02_20221031_Test.dat";

        [Description("R/W,BmmLinCmdId")]
        public string BmmLinCmdId = "0x38";

        [Description("R/W,BmmLinAnsId")]
        public string BmmLinAnsId = "0x3a";

        [Description("R/W,BmmLinInitialNad")]
        public string BmmLinInitialNad = "0x04";

        [Description("R/W,硬件版本号")]
        public string HardWareVersion = "48000153";

        [Description("R/W,供货零件号")]
        public string ProductNo = "S00023050";

        [Description("R/W,节点配置Hex")]
        public string NodeInfoHex = "0x97";

        //public string NodeConfig=""

        public ElmosAppDownLoad(string name)
            : base(name) { }

        private int _sequenceNumber = 1;

        [Description("ELMOSAppDownload")]
        public void DatFileDownload()
        {
            DatDownloadResult = string.Empty;
            ReadProductData = string.Empty;
            ReadProductSerialNo = string.Empty;
            ReadHardwareNo = string.Empty;
            ReadProductNo = string.Empty;
            ReadSlaveConfig = string.Empty;

            if (Lin == null)
            {
                DatDownloadResult = "NG 未找到LIN端口";
                return;
            }

            if (string.IsNullOrEmpty(BmmLinCmdId) || string.IsNullOrEmpty(BmmLinAnsId))
            {
                DatDownloadResult = "NG 未定义正确的LinId";
                return;
            }

            if (string.IsNullOrEmpty(BmmLinInitialNad))
            {
                DatDownloadResult = "NG 未定义正确的NAD";
                return;
            }

            //if (string.IsNullOrEmpty(HardWareVersion) ||
            //    string.IsNullOrEmpty(ProductNo) ||
            //    !HardWareVersion.StartsWith("H") ||
            //    !ProductNo.StartsWith("S") ||
            //    ProductNo.Length != 9)
            //{
            //    DatDownloadResult = "NG 未定义正确的硬件版本号或供货零件号";
            //    return;
            //}

            if (string.IsNullOrEmpty(DatFilePath))
            {
                DatDownloadResult = "NG 未定义dat文件路径";
                return;
            }

            if (!DatFilePath.EndsWith(@".dat"))
            {
                DatDownloadResult = "NG 选择的文件非dat文件";
                return;
            }

            if (!File.Exists(DatFilePath))
            {
                DatDownloadResult = "NG dat文件不存在";
                return;
            }

            var temp = new List<string>();
            using (var sr = new StreamReader(DatFilePath, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        line = line.Trim(' ');
                        if (line.Length != 6)
                        {
                            DatDownloadResult = "NG dat文件解析错误";
                            return;
                        }

                        var str = line.Trim(' ');
                        if (!string.IsNullOrEmpty(str))
                            temp.Add(str);
                    }
                    catch (Exception ex)
                    {
                        DatDownloadResult = "NG dat文件解析错误 " + ex.Message;
                        return;
                    }
                }
            }

            var dataList = new List<byte>();
            foreach (var t in temp)
            {
                try
                {
                    var b1 = t.Substring(4, 2);
                    var b2 = t.Substring(2, 2);
                    dataList.Add(Convert.ToByte(b1, 16));
                    dataList.Add(Convert.ToByte(b2, 16));
                }
                catch (Exception ex)
                {
                    DatDownloadResult = "NG dat文件解析错误 " + ex.Message;
                    return;
                }
            }

            _sequenceNumber = 1;
            var bmmLinCmdId = Convert.ToByte(BmmLinCmdId, 16);
            var bmmLinAnsId = Convert.ToByte(BmmLinAnsId, 16);
            var bmmLinInitialNad = Convert.ToByte(BmmLinInitialNad, 16);

            Awake(bmmLinCmdId);

            DatDownloadResult = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - 握手中";
            if (!ConnectCheck(bmmLinCmdId, bmmLinAnsId, bmmLinInitialNad, 100))
            {
                DatDownloadResult = "NG 握手失败";
                return;
            }

            DatDownloadResult = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - 下载中";
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            Thread.Sleep(10);
            if (!FlashErase(bmmLinCmdId, bmmLinAnsId, bmmLinInitialNad))
            {
                stopWatch.Stop();
                DatDownloadResult = "NG Flash擦除失败 " + stopWatch.ElapsedMilliseconds + "ms";
                return;
            }

            Thread.Sleep(10);
            if (!FlashTransfer(bmmLinCmdId, bmmLinAnsId, bmmLinInitialNad, dataList))
            {
                stopWatch.Stop();
                DatDownloadResult = "NG Flash传输失败" + stopWatch.ElapsedMilliseconds + "ms";
                return;
            }

            Thread.Sleep(10);
            WriteAndReadEpprom(bmmLinCmdId, bmmLinAnsId, bmmLinInitialNad);

            Thread.Sleep(50);
            Lin.SendMasterLin(bmmLinCmdId, new byte[] { 0x7f, (byte)_sequenceNumber, 0xf0, 0xff, 0xff, 0xff, 0xff, 0xff });
            AddSequenceNumber();

            stopWatch.Stop();
            DatDownloadResult = "OK " + stopWatch.ElapsedMilliseconds + "ms";
        }

        [Description("读EPPROM")]
        public void ReadEpprom()
        {
            var bmmLinCmdId = Convert.ToByte(BmmLinCmdId, 16);
            var bmmLinAnsId = Convert.ToByte(BmmLinAnsId, 16);
            var bmmLinInitialNad = Convert.ToByte(BmmLinInitialNad, 16);

            ReadProductData = string.Empty;
            ReadProductSerialNo = string.Empty;
            ReadHardwareNo = string.Empty;
            ReadProductNo = string.Empty;
            ReadSlaveConfig = string.Empty;

            Awake(bmmLinCmdId);

            if (!ConnectCheck(bmmLinCmdId, bmmLinAnsId, bmmLinInitialNad, 200))
            {
                ReadProductData = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - 握手失败";
                ReadProductSerialNo = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - 握手失败";
                ReadHardwareNo = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - 握手失败";
                ReadProductNo = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - 握手失败";
                ReadSlaveConfig = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - 握手失败";
                return;
            }

            ReadProductData = string.Empty;
            ReadProductSerialNo = string.Empty;
            ReadHardwareNo = string.Empty;
            ReadProductNo = string.Empty;
            ReadSlaveConfig = string.Empty;

            #region 读EPPROM
            //if (ConnectCheck(bmmLinCmdid, bmmLinAnsId, bmmLinInitialNad, 20))
            if (true)
            {


                var readProductDataBytes = ReadTrackInfo(0, bmmLinCmdId, bmmLinAnsId, bmmLinInitialNad);
                var productDataBytes = readProductDataBytes as byte[] ?? readProductDataBytes.ToArray();
                if (productDataBytes.Length == 5)
                {
                    var temp = Encoding.ASCII.GetString(productDataBytes);
                    //if (temp == Encoding.ASCII.GetString(productData))
                    //    ReadProductData = "OK " + temp;
                    //else
                    //    ReadProductData = "NG " + temp;
                    ReadProductData = temp;
                }

                var readProductSerialNoBytes = ReadTrackInfo(5, bmmLinCmdId, bmmLinAnsId, bmmLinInitialNad);
                var productSerialNoBytes = readProductSerialNoBytes as byte[] ?? readProductSerialNoBytes.ToArray();
                if (productSerialNoBytes.Length == 5)
                {
                    var temp = string.Empty;
                    for (var i = 0; i < 2; i++)
                        temp += ValueHelper.GetHextStr(productSerialNoBytes.ToList()[i]);

                    //if (temp == productSerialNo)
                    //    ReadProductSerialNo = "OK " + temp;
                    //else
                    //    ReadProductSerialNo = "NG " + temp;

                    ReadProductSerialNo = temp;
                }

                var readProductHardwareVersionBytes = ReadTrackInfo(10, bmmLinCmdId, bmmLinAnsId, bmmLinInitialNad);
                var productHardwareVersionBytes = readProductHardwareVersionBytes as byte[] ?? readProductHardwareVersionBytes.ToArray();
                if (productHardwareVersionBytes.Length == 5)
                {
                    var temp = string.Empty;
                    temp += ValueHelper.GetHextStr(productHardwareVersionBytes.ToArray()[0]);
                    temp += ValueHelper.GetHextStr(productHardwareVersionBytes.ToArray()[1]);
                    temp += ValueHelper.GetHextStr(productHardwareVersionBytes.ToArray()[2]);
                    temp += ValueHelper.GetHextStr(productHardwareVersionBytes.ToArray()[3]);
                    //if (temp == HardWareVersion)
                    //    ReadHardwareNo = "OK " + temp;
                    //else
                    //    ReadHardwareNo = "NG " + temp;

                    ReadHardwareNo = temp;
                }

                var readProductNoBytes = ReadTrackInfo(15, bmmLinCmdId, bmmLinAnsId, bmmLinInitialNad);
                var productNoBytes = readProductNoBytes as byte[] ?? readProductNoBytes.ToArray();
                if (productNoBytes.Length == 5)
                {
                    var temp = string.Empty;
                    temp += "S";

                    var hex = string.Empty;
                    for (var i = 0; i < 3; i++)
                        hex += ValueHelper.GetHextStr(productNoBytes.ToArray()[i]);
                    hex = hex.PadLeft(8, '0');
                    temp += hex;

                    //if (temp == ProductNo)
                    //    ReadProductNo = "OK " + temp;
                    //else
                    //    ReadProductNo = "NG " + temp;

                    ReadProductNo = temp;
                }

                var readSlaveConfigBytes = ReadTrackInfo(0x38, bmmLinCmdId, bmmLinAnsId, bmmLinInitialNad);
                var slaveConfigBytes = readSlaveConfigBytes as byte[] ?? readSlaveConfigBytes.ToArray();
                if (slaveConfigBytes.Length == 5)
                {
                    var temp = string.Empty;

                    var hex = string.Empty;
                    for (var i = 0; i < 3; i++)
                        hex += ValueHelper.GetHextStr(slaveConfigBytes.ToArray()[i]);
                    temp += hex;

                    ReadSlaveConfig = temp;
                }
            }
            #endregion
        }

        /// <summary>
        /// 通过LIN唤醒
        /// </summary>
        /// <param name="bmmLinCmdId"></param>
        private void Awake(byte bmmLinCmdId)
        {
            for (var i = 0; i < 4; i++)
            {
                Lin.SendMasterLin(
                    bmmLinCmdId,
                    new byte[] { 0x7f, 0x01, 0xff, 0x02, 0x01, 0x34, 0x01, 0xff });
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 产品握手
        /// </summary>
        /// <param name="bmmLinCmdid"></param>
        /// <param name="bmmLinAnsId"></param>
        /// <param name="bmmLinInitialNad"></param>
        /// <param name="maxTryCount"></param>
        /// <returns></returns>
        private bool ConnectCheck(byte bmmLinCmdid, byte bmmLinAnsId, byte bmmLinInitialNad, int maxTryCount)
        {
            // Li 38 Tx  8  7f d7 ff ff ff 34 ff 00  checksum = fb
            // Li 38 Tx  8  04 d8 fe 00 00 00 00 00  checksum = ab
            // Li 3a Rx  8  04 d8 03 00 ff 00 ff ff  checksum = 65 

            for (var i = 0; i < maxTryCount; i++)
            {
                Lin.SendMasterLin(bmmLinCmdid, new byte[] { 0x7F, (byte)_sequenceNumber, 0xff, 0xff, 0xff, 0x34, 0xff, 0x00 });
                //Lin.SendMasterLin(bmmLinCmdid, new byte[] { 0x7F, (byte)_sequenceNumber, 0xff, 0x02, 0x01, 0x34, 0x01, 0xff });
                AddSequenceNumber();
                Thread.Sleep(10);

                var tempSequenceNumber = (byte)_sequenceNumber;
                Lin.SendMasterLin(bmmLinCmdid, new byte[]
                {
                    bmmLinInitialNad, tempSequenceNumber, 0xfe, 0x00, 0x00, 0x00, 0x00, 0x00
                });
                AddSequenceNumber();
                Thread.Sleep(10);

                byte[] echoBytes;
                if (!Lin.SendSlaveLin(bmmLinAnsId, out echoBytes, 50))
                    continue;

                if (echoBytes.Length == 8 &&
                    echoBytes[0] == bmmLinInitialNad &&
                    echoBytes[1] == tempSequenceNumber &&
                    echoBytes[4] == 0xFF)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Flash擦除
        /// </summary>
        /// <returns></returns>
        private bool FlashErase(byte bmmLinCmdid, byte bmmLinAnsId, byte bmmLinInitialNad)
        {
            //Li 38 Tx  8  7f d9 80 91 3b ff ff ff  checksum = e0 ==> 请求擦除flash命令
            //Li 38 Tx  8  7f da 7f 03 84 ac ff ff  checksum = 79 ==> 擦除flash命令
            //Li 38 Tx  8  04 db fe 00 00 00 00 00  checksum = a8 ==> 从节点擦除命令执行完擦除 FLASH 命令之后，需等待一段时间（至少 30ms）
            //Li 3a Rx  8  04 db 06 00 ff 00 ff ff  checksum = 5f ==> FLASH 擦除完成后，可以发送从节点状态查询命令，检查当前从节点的状态，若错误码为 0xFF，则表明成功执行了上一条命令，即可认为 FLASH 擦除成功

            Lin.SendMasterLin(bmmLinCmdid, new byte[] { 0x7f, (byte)_sequenceNumber, 0x80, 0x91, 0x3b, 0xff, 0xff, 0xff });
            AddSequenceNumber();
            Thread.Sleep(10);

            Lin.SendMasterLin(bmmLinCmdid, new byte[] { 0x7f, (byte)_sequenceNumber, 0x7f, 0x03, 0x84, 0xac, 0xff, 0xff });
            AddSequenceNumber();
            Thread.Sleep(50);

            var tempSequenceNumber = (byte)_sequenceNumber;
            Lin.SendMasterLin(bmmLinCmdid, new byte[]
            {
                bmmLinInitialNad, tempSequenceNumber, 0xfe, 0x00, 0x00, 0x00, 0x00, 0x00
            });
            AddSequenceNumber();

            byte[] echoBytes;
            if (!Lin.SendSlaveLin(bmmLinAnsId, out echoBytes, 250))
                return false;

            return echoBytes.Length == 8 &&
                   echoBytes[0] == bmmLinInitialNad &&
                   echoBytes[1] == tempSequenceNumber &&
                   echoBytes[4] == 0xFF;
        }

        /// <summary>
        /// FLASH传输
        /// </summary>
        /// <param name="bmmLinCmdid"></param>
        /// <param name="bmmLinAnsId"></param>
        /// <param name="bmmLinInitialNad"></param>
        /// <param name="dataList"></param>
        private bool FlashTransfer(
            byte bmmLinCmdid, byte bmmLinAnsId, byte bmmLinInitialNad, IList<byte> dataList)
        {
            //Li 38 Tx  8  7f dc 40 91 3b ff ff 84  checksum = 99
            //Li 38 Tx  8  7f dd bf 20 00 00 00 ac  checksum = 9d

            Lin.SendMasterLin(bmmLinCmdid, new byte[] { 0x7f, (byte)_sequenceNumber, 0x40, 0x91, 0x3b, 0xff, 0xff, 0x84 });
            AddSequenceNumber();
            Thread.Sleep(10);

            Lin.SendMasterLin(bmmLinCmdid, new byte[] { 0x7f, (byte)_sequenceNumber, 0xbf, 0x20, 0x00, 0x00, 0x00, 0xac });
            AddSequenceNumber();
            Thread.Sleep(10);

            ushort crcNew = 0;
            ushort crcOld = 0xFFFF;

            foreach (var t in dataList)
            {
                var cval = t;
                crcNew = BmmAddToCrc(crcOld, cval);
                crcOld = crcNew;
            }

            var crcByte0 = BitConverter.GetBytes(crcNew)[1];
            var crcByte1 = BitConverter.GetBytes(crcNew)[0];

            var rest = dataList.Count % 5;
            if (rest != 0)
                for (var i = 0; i < 5 - rest; i++)
                    dataList.Add(0xFF);

            var baseIndex = 0;
            for (var i = 0; i < dataList.Count / 5; i++)
            {
                Lin.SendMasterLin(
                    bmmLinCmdid,
                    new byte[]
                    {
                        0x7F, (byte) _sequenceNumber, 0xbc, dataList[baseIndex], dataList[baseIndex + 1],
                        dataList[baseIndex + 2], dataList[baseIndex + 3], dataList[baseIndex + 4]
                    });
                AddSequenceNumber();
                baseIndex = baseIndex + 5;
                Thread.Sleep(10);
            }

            Thread.Sleep(50);
            var tempSequenceNumber = (byte)_sequenceNumber;
            Lin.SendMasterLin(bmmLinCmdid, new byte[]
            {
                bmmLinInitialNad, tempSequenceNumber, 0x42, 0xff, 0xff, 0xff, 0xff, 0xff
            });
            AddSequenceNumber();

            bool isOk;
            Thread.Sleep(20);

            byte[] echoBytes;
            if (!Lin.SendSlaveLin(bmmLinAnsId, out echoBytes, 500))
            {
                isOk = false;
            }
            else
            {
                isOk = echoBytes.Length == 8 &&
                       echoBytes[0] == bmmLinInitialNad &&
                       echoBytes[1] == tempSequenceNumber &&
                       echoBytes[2] == 0xFF &&
                       echoBytes[3] == crcByte0 &&
                       echoBytes[4] == crcByte1;
            }

            return isOk;
        }

        private void WriteAndReadEpprom(byte bmmLinCmdid, byte bmmLinAnsId, byte bmmLinInitialNad)
        {
            #region 写EPPROM

            var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
               ServerUid, ServerPwd);
            var serialNo = string.Empty;
            var date = string.Empty;
            using (var conn = new SqlConnection(sqlConnectiong))
            {
                try
                {
                    var cmd = new SqlCommand("GetCheckNo", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@Productno", ProductNo);  //给输入参数赋值
                    var parOutputDate = cmd.Parameters.Add("@date", SqlDbType.DateTime);  //定义输出参数 
                    parOutputDate.Direction = ParameterDirection.Output;  //参数类型为Output 
                    var returnSerialNo = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnSerialNo.Direction = ParameterDirection.ReturnValue;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    serialNo = (int.Parse(returnSerialNo.Value.ToString()) + BaseSerialNoIndex).ToString();
                    date = parOutputDate.Value.ToString();
                }
                catch (Exception)
                {
                    return;
                }
            }

            if (string.IsNullOrEmpty(serialNo) || string.IsNullOrEmpty(date))
                return;

            var productData = Encoding.ASCII.GetBytes(string.Format("{0}{1}", DateTime.Parse(date).Year.ToString().Substring(2, 2), DateTime.Parse(date).DayOfYear.ToString().PadLeft(3, '0')));//Encoding.ASCII.GetBytes("19156"); // 21年231天，待修改，根据实际生成。
            var productSerialNo = serialNo.PadLeft(4, '0');//"1352"; // 第196个，待修改，根据实际生成

            //var productData = Encoding.ASCII.GetBytes("14335"); // 21年231天，待修改，根据实际生成。
            //var productSerialNo = "0004"; // 第196个，待修改，根据实际生成
            //HardWareVersion = "48000653";
            //ProductNo = "S00013476";

            var writeEppromAction = new Action(() =>
            {
                var writeDataList = new List<byte>
                {
                    productData[0],
                    productData[1],
                    productData[2],
                    productData[3],
                    productData[4],

                    Convert.ToByte(productSerialNo.Substring(0, 2), 16),
                    Convert.ToByte(productSerialNo.Substring(2, 2), 16),
                    0x00,
                    0x00,
                    0x00,

                    Convert.ToByte(HardWareVersion.Substring(0, 2), 16),
                    Convert.ToByte(HardWareVersion.Substring(2, 2), 16),
                    Convert.ToByte(HardWareVersion.Substring(4, 2), 16),
                    Convert.ToByte(HardWareVersion.Substring(6, 2), 16),
                    0x00,

                    Convert.ToByte(ProductNo.Substring(3, 2), 16),
                    Convert.ToByte(ProductNo.Substring(5, 2), 16),
                    Convert.ToByte(ProductNo.Substring(7, 2), 16),
                };

                ushort crcNew = 0;
                ushort crcOld = 0xFFFF;

                foreach (var t in writeDataList)
                {
                    var cval = t;
                    crcNew = BmmAddToCrc(crcOld, cval);
                    crcOld = crcNew;
                }

                var crcByte0 = BitConverter.GetBytes(crcNew)[1];
                var crcByte1 = BitConverter.GetBytes(crcNew)[0];

                // 写EPPROM请求
                Lin.SendMasterLin(bmmLinCmdid,
                    new byte[] { 0x7f, (byte)_sequenceNumber, 0x10, 0x12, 0x00, crcByte0, crcByte1, 0xC2 });
                AddSequenceNumber();
                Thread.Sleep(10);

                // 写生产日期
                Lin.SendMasterLin(bmmLinCmdid,
                    new byte[]
                    {
                        0x7f, (byte) _sequenceNumber, 0xef,
                        writeDataList[0],
                        writeDataList[1],
                        writeDataList[2],
                        writeDataList[3],
                        writeDataList[4],
                    });
                AddSequenceNumber();
                Thread.Sleep(10);

                // 写生产序列号
                Lin.SendMasterLin(bmmLinCmdid,
                    new byte[]
                    {
                        0x7f,
                        (byte) _sequenceNumber,
                        0xef,
                        writeDataList[5],
                        writeDataList[6],
                        writeDataList[7],
                        writeDataList[8],
                        writeDataList[9],
                    });
                AddSequenceNumber();
                Thread.Sleep(10);

                // 写硬件版本号
                Lin.SendMasterLin(bmmLinCmdid,
                    new byte[]
                    {
                        0x7f,
                        (byte) _sequenceNumber,
                        0xef,
                        writeDataList[10],
                        writeDataList[11],
                        writeDataList[12],
                        writeDataList[13],
                        writeDataList[14],
                    });
                AddSequenceNumber();
                Thread.Sleep(10);

                // 写供货零件号
                Lin.SendMasterLin(bmmLinCmdid,
                    new byte[]
                    {
                        0x7f,
                        (byte) _sequenceNumber,
                        0xef,
                        writeDataList[15],
                        writeDataList[16],
                        writeDataList[17],
                        0x00,
                        0x00
                    });
                AddSequenceNumber();
                Thread.Sleep(10);
            });
            writeEppromAction.Invoke();

            Thread.Sleep(500);

            var slaveConfigAction = new Action(() =>
            {
                var writeDataList = new List<byte>
                {
                    Convert.ToByte(NodeInfoHex,16)
                };

                ushort crcNew = 0;
                ushort crcOld = 0xFFFF;

                foreach (var t in writeDataList)
                {
                    var cval = t;
                    crcNew = BmmAddToCrc(crcOld, cval);
                    crcOld = crcNew;
                }

                var crcByte0 = BitConverter.GetBytes(crcNew)[1];
                var crcByte1 = BitConverter.GetBytes(crcNew)[0];

                // 写EPPROM请求
                Lin.SendMasterLin(bmmLinCmdid,
                    new byte[] { 0x7f, (byte)_sequenceNumber, 0x10, 0x01, 0x38, crcByte0, crcByte1, 0xC2 });
                AddSequenceNumber();
                Thread.Sleep(10);

                // 节点配置
                Lin.SendMasterLin(bmmLinCmdid,
                    new byte[]
                    {
                        0x7f, (byte) _sequenceNumber, 0xef,
                        writeDataList[0],
                        0xFF, 0xFF, 0xFF, 0xFF
                    });
                AddSequenceNumber();
                Thread.Sleep(10);
            });

            slaveConfigAction.Invoke();
            #endregion

            Thread.Sleep(500);

            //var tempSequenceNumber = (byte)_sequenceNumber;
            //Lin.SendMasterLin(bmmLinCmdid, new byte[]
            //{
            //    bmmLinInitialNad, tempSequenceNumber, 0x12, 0xff, 0xff, 0xff, 0xff, 0xff
            //});
            //AddSequenceNumber();
            //byte[] echoBytes;
            //if (Lin.SendSlaveLin(bmmLinAnsId, out echoBytes, 250))
            //{
            //}

            #region 读EPPROM
            //if (ConnectCheck(bmmLinCmdid, bmmLinAnsId, bmmLinInitialNad, 20))
            if (true)
            {
                var readProductDataBytes = ReadTrackInfo(0, bmmLinCmdid, bmmLinAnsId, bmmLinInitialNad);
                var productDataBytes = readProductDataBytes as byte[] ?? readProductDataBytes.ToArray();
                if (productDataBytes.Length == 5)
                {
                    var temp = Encoding.ASCII.GetString(productDataBytes);
                    if (temp == Encoding.ASCII.GetString(productData))
                        ReadProductData = "OK " + temp;
                    else
                        ReadProductData = "NG " + temp;
                }

                var readProductSerialNoBytes = ReadTrackInfo(5, bmmLinCmdid, bmmLinAnsId, bmmLinInitialNad);
                var productSerialNoBytes = readProductSerialNoBytes as byte[] ?? readProductSerialNoBytes.ToArray();
                if (productSerialNoBytes.Length == 5)
                {
                    var temp = string.Empty;
                    for (var i = 0; i < 2; i++)
                        temp += ValueHelper.GetHextStr(productSerialNoBytes.ToList()[i]);

                    if (temp == productSerialNo)
                        ReadProductSerialNo = "OK " + temp;
                    else
                        ReadProductSerialNo = "NG " + temp;
                }

                var readProductHardwareVersionBytes = ReadTrackInfo(10, bmmLinCmdid, bmmLinAnsId, bmmLinInitialNad);
                var productHardwareVersionBytes = readProductHardwareVersionBytes as byte[] ?? readProductHardwareVersionBytes.ToArray();
                if (productHardwareVersionBytes.Length == 5)
                {
                    var temp = string.Empty;
                    temp += ValueHelper.GetHextStr(productHardwareVersionBytes.ToArray()[0]);
                    temp += ValueHelper.GetHextStr(productHardwareVersionBytes.ToArray()[1]);
                    temp += ValueHelper.GetHextStr(productHardwareVersionBytes.ToArray()[2]);
                    temp += ValueHelper.GetHextStr(productHardwareVersionBytes.ToArray()[3]);
                    if (temp == HardWareVersion)
                        ReadHardwareNo = "OK " + temp;
                    else
                        ReadHardwareNo = "NG " + temp;
                }

                var readProductNoBytes = ReadTrackInfo(15, bmmLinCmdid, bmmLinAnsId, bmmLinInitialNad);
                var productNoBytes = readProductNoBytes as byte[] ?? readProductNoBytes.ToArray();
                if (productNoBytes.Length == 5)
                {
                    var temp = string.Empty;
                    temp += "S";

                    var hex = string.Empty;
                    for (var i = 0; i < 3; i++)
                        hex += ValueHelper.GetHextStr(productNoBytes.ToArray()[i]);
                    hex = hex.PadLeft(8, '0');
                    temp += hex;

                    if (temp == ProductNo)
                        ReadProductNo = "OK " + temp;
                    else
                        ReadProductNo = "NG " + temp;
                }

                var readSlaveConfigBytes = ReadTrackInfo(0x38, bmmLinCmdid, bmmLinAnsId, bmmLinInitialNad);
                var slaveConfigBytes = readSlaveConfigBytes as byte[] ?? readSlaveConfigBytes.ToArray();
                if (slaveConfigBytes.Length == 5)
                {
                    var temp = string.Empty;

                    var hex = string.Empty;
                    for (var i = 0; i < 3; i++)
                        hex += ValueHelper.GetHextStr(slaveConfigBytes.ToArray()[i]);
                    temp += hex;

                    ReadSlaveConfig = temp;
                }
            }
            #endregion
        }

        private IEnumerable<byte> ReadTrackInfo(
            int addrOffset, byte bmmLinCmdid, byte bmmLinAnsId, byte bmmLinInitialNad)
        {
            var tempSequenceNumber = (byte)_sequenceNumber;
            Lin.SendMasterLin(bmmLinCmdid, new byte[]
            {
                bmmLinInitialNad, tempSequenceNumber, 0xC4, 0x00, (byte)addrOffset, 0x05, 0x01, 0xff
            });
            AddSequenceNumber();
            byte[] echoBytes;
            if (Lin.SendSlaveLin(bmmLinAnsId, out echoBytes, 250))
            {
                if (echoBytes.Length == 8 &&
                    echoBytes[0] == bmmLinInitialNad &&
                    echoBytes[1] == tempSequenceNumber &&
                    echoBytes[2] == 0xFF)
                {
                    return new[] { echoBytes[3], echoBytes[4], echoBytes[5], echoBytes[6], echoBytes[7] };
                }
            }

            return new byte[0];
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddSequenceNumber()
        {
            _sequenceNumber++;
            if (_sequenceNumber > byte.MaxValue)
                _sequenceNumber = 0;
        }

        [Description("R,软件零件号")]
        public string SoftPart = string.Empty;

        [Description("R,软件版本号")]
        public string SoftVersion = string.Empty;

        [Description("读取版本信息")]
        public void ReadSoftInfo()
        {
            SoftPart = string.Empty;
            SoftVersion = string.Empty;

            try
            {
                //var bmmLinCmdId = Convert.ToByte(BmmLinCmdId, 16);
                //Awake(bmmLinCmdId);

                //for (var i = 0; i < 5; i++)
                //{
                //    //Lin.SendMasterLin(0x30, new byte[8]);
                //    //byte[] echo1;
                //    //Lin.SendSlaveLin(0x34, out echo1);

                //    Lin.SendMasterLin(0x30, new byte[8]);
                //    byte[] echo2;
                //    Lin.SendSlaveLin(0x34, out echo2);

                //    //Lin.SendMasterLin(0x30, new byte[8]);
                //    //byte[] echo3;
                //    //Lin.SendSlaveLin(0x36, out echo3);

                //    //Lin.SendMasterLin(0x30, new byte[8]);
                //    //byte[] echo4;
                //    //Lin.SendSlaveLin(0x37, out echo4);

                //    //Lin.SendMasterLin(0x30, new byte[8]);
                //    //byte[] echo5;
                //    //Lin.SendSlaveLin(0x38, out echo5);

                //    //Lin.SendMasterLin(0x30, new byte[8]);
                //    //byte[] echo6;
                //    //Lin.SendSlaveLin(0x39, out echo6);

                //    //Lin.SendMasterLin(0x30, new byte[8]);
                //    //byte[] echo7;
                //    //Lin.SendSlaveLin(0x3A, out echo7);

                //    //Lin.SendMasterLin(0x30, new byte[8]);
                //    //byte[] echo8;
                //    //Lin.SendSlaveLin(0x3B, out echo8);
                //}

                byte cmd = 0x38;
                var send = new byte[2];
                var value = new byte[8];
                send[0] = 0x05;
                int j = 0;
                do
                {
                    Lin.SendMasterLinAndRecvSingleSlaveLin(0x09, cmd, send, out value);
                    //return;
                    if (value.Length >= 5)
                    {
                        var temp = value[2].ToString("x") + value[3].ToString("x") + value[4].ToString("x");
                        var no = Convert.ToInt32(temp, 16);
                        SoftPart = "SW" + no.ToString().PadLeft(7, '0');
                        Thread.Sleep(50);
                    }
                } while (value.FirstOrDefault() == 0x00 && j++ < 5);

                send[0] = 0x06; j = 0;
                do
                {
                    Lin.SendMasterLinAndRecvSingleSlaveLin(0x09, cmd, send, out value);
                    if (value.Length >= 2)
                    {
                        SoftVersion = Encoding.ASCII.GetString(new[] { value[0] }) + value[1].ToString("00") + value[2].ToString("x");
                        Thread.Sleep(50);
                    }
                } while (value.FirstOrDefault() == 0x00 && j++ < 5);

            }
            catch (Exception err)
            {
                Console.WriteLine("读取信息有误：" + err.Message);
            }
        }

        private static ushort BmmAddToCrc(ushort pCrc, byte cVal)
        {
            ushort i;
            for (i = 0; i < 8; i++)
            {
                ushort cXor = (byte)((byte)((pCrc) >> 8) ^ cVal);
                cVal <<= 1;
                pCrc <<= 1;
                if ((cXor & 0x80) != 0)
                    pCrc ^= 0x1021;
            }

            return pCrc;
        }

        #region

        [Description("R/W,服务器IP地址")]
        public string ServerIp = "127.0.0.1";

        [Description("R/W,服务器数据库名称")]
        public string ServerDataBase = "IPMS";

        [Description("R/W,服务器用户名")]
        public string ServerUid = "sa";

        [Description("R/W,服务器用密码")]
        public string ServerPwd = "123456";

        #endregion
    }
}
