using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,A2LL ALS Master")]
    public sealed class A2LlAlsMasterEcu : ControllerBase
    {
        public LinBus LinWithBaudRate10417;

        public A2LlAlsMasterEcu(string name) :
            base(name)
        {
            if (_keepNetworkThread != null)
            {
                _keepNetworkThread.Abort();
                _keepNetworkThread.Join();
            }

            _keepNetworkThread = new Thread(KeepNetwork) { IsBackground = true };
            _keepNetworkThread.Start();
        }

        ~A2LlAlsMasterEcu()
        {
            Dispose();
        }

        #region public fields

        [Description("R,引导程序版本号")]
        public string FblVersion;

        [Description("R,硬件版本号")]
        public string HardwareVersion;

        [Description("R,ECU供货零件号")]
        public string PartNo;

        [Description("R,生产日期")]
        public string Date;

        [Description("R,生产序列号")]
        public string SerialNo;

        [Description("R,应用程序版本号")]
        public string AppVersion;

        [Description("R,应用程序零件号")]
        public string AppPartNo;

        [Description("R/W,服务器IP地址")]
        public string ServerIp = "127.0.0.1";

        [Description("R/W,服务器数据库名称")]
        public string ServerDataBase = "IPMS";

        [Description("R/W,服务器用户名")]
        public string ServerUid = "sa";

        [Description("R/W,服务器用密码")]
        public string ServerPwd = "123456";

        [Description("R,模块电源电压")]
        public double ModuleSupplyVolt;

        [Description("R,前传感器信号")]
        public double FrontSensorVolt;

        [Description("R,后传感器信号")]
        public double RearSensorVolt;

        [Description("R,传感器电源电压")]
        public double SensorSupplyVolt;

        [Description("R,马达运动方向配置")]
        public string MotorDirectionConfigResult;

        [Description("R,DHL马达位置信息配置")]
        public string MotorPositionConfigResult;

        [Description("R,控制DHL马达初始化")]
        public string MotorInitResult;

        [Description("R,控制DHL马达运动")]
        public string MotorRunResult;

        [Description("R,读取DHL马达当前实际位置")]
        public string MotorPositionReadResult;

        [Description("R,读取DHL马达开路故障")]
        public string MotorOpenError;

        [Description("R,读取DHL马达短路故障")]
        public string MotorShortError;

        [Description("R,故障清除结果")]
        public string ErrorClearResult;

        #endregion

        private readonly Thread _keepNetworkThread;
        private readonly object _lockSend = new object();
        private bool _isSleep = true;
        private byte _masterLinId = 0x29;
        private byte _slaveLinId = 0x28;

        [Description("ECU休眠")]
        public void ModuleSleep()
        {
            _isSleep = true;
        }

        [Description("ECU唤醒")]
        public void ModuleAwake()
        {
            _isSleep = false;
        }

        private void KeepNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                if (LinWithBaudRate10417 == null)
                    continue;

                if (_isSleep)
                    continue;
                GetLinRecvMsg(new byte[3], 0x00, 0x01);
                //MotorReadError();
            }
        }

        #region 版本信息

        [Description("设置命令帧LIN-ID")]
        public void InitMasterLinId(string masterLinId)
        {
            _masterLinId = Convert.ToByte(masterLinId, 16);
        }

        [Description("设置响应帧LIN-ID")]
        public void InitSlavaLinId(string slaveLinId)
        {
            _slaveLinId = Convert.ToByte(slaveLinId, 16);
        }

        [Description("Read引导程序版本号")]
        public void ReadFblVersion()
        {
            FblVersion = string.Empty;
            FblVersion = GetLinRecvMsg(new byte[] { 0x09, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);
        }

        [Description("Read应用程序版本号")]
        public void ReadAppVersion()
        {
            AppVersion = string.Empty;
            AppVersion = GetLinRecvMsg(new byte[] { 0x09, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);
        }

        [Description("Read应用程序零件号")]
        public void ReadAppPartNo()
        {
            AppPartNo = string.Empty;
            AppPartNo = GetLinRecvMsg(new byte[] { 0x09, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);
        }

        [Description("写追溯")]
        public void WritrTraceInfo(string partNo, string hardwareNo)
        {
            PartNo = string.Empty;
            SerialNo = string.Empty;
            Date = string.Empty;
            HardwareVersion = string.Empty;

            if (LinWithBaudRate10417 == null)
            {
                PartNo = "NG LIN未初始化";
                SerialNo = "NG LIN未初始化";
                Date = "NG LIN未初始化";
                HardwareVersion = "NG LIN未初始化";
                return;
            }

            if (!partNo.StartsWith("P") || partNo.Length != 9)
            {
                PartNo = "NG 需要写入的总成零件号不对";
                return;
            }

            for (var i = 1; i < partNo.Length; i++)
            {
                int x;
                if (int.TryParse(partNo[i].ToString(), out x))
                    continue;
                PartNo = "NG 需要写入的总成零件号不对";
                return;
            }

            if (!hardwareNo.StartsWith("H") || hardwareNo.Length != 4)
            {
                HardwareVersion = "NG 需要写入的硬件版本号不对";
                return;
            }

            for (var i = 1; i < hardwareNo.Length; i++)
            {
                int x;
                if (int.TryParse(hardwareNo[i].ToString(), out x))
                    continue;
                HardwareVersion = "NG 需要写入的硬件版本号不对";
                return;
            }

            var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
              ServerUid, ServerPwd);
            string serialNo;
            string date;
            using (var conn = new SqlConnection(sqlConnectiong))
            {
                try
                {
                    var cmd = new SqlCommand("GetCheckNo", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@Productno", partNo);  //给输入参数赋值
                    var parOutputDate = cmd.Parameters.Add("@date", SqlDbType.DateTime);  //定义输出参数 
                    parOutputDate.Direction = ParameterDirection.Output;  //参数类型为Output 
                    var returnSerialNo = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnSerialNo.Direction = ParameterDirection.ReturnValue;
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    serialNo = returnSerialNo.Value.ToString();
                    date = parOutputDate.Value.ToString();
                }
                catch (Exception)
                {
                    Date = "NG 生成生产日期及序列号失败";
                    SerialNo = "NG 生成生产日期及序列号失败";
                    return;
                }
            }

            if (string.IsNullOrEmpty(serialNo) || string.IsNullOrEmpty(date))
            {
                Date = "NG 生成生产日期及序列号失败";
                SerialNo = "NG 生成生产日期及序列号失败";
                return;
            }

            var partNoBytes = new List<byte>();
            partNoBytes.AddRange(Encoding.ASCII.GetBytes(partNo[0].ToString()));
            for (var i = 1; i < partNo.Length; i = i + 2)
                partNoBytes.Add(Convert.ToByte(partNo.Substring(i, 2), 16));

            var hardwareBytes = new List<byte>();
            hardwareBytes.AddRange(Encoding.ASCII.GetBytes(hardwareNo[0].ToString()));
            for (var i = 0; i < hardwareNo.Substring(1, 3).PadLeft(4, '0').Length; i = i + 2)
                hardwareBytes.Add(Convert.ToByte(hardwareNo.Substring(1, 3).PadLeft(4, '0').Substring(i, 2), 16));

            var dateTime = DateTime.Parse(date);
            var year = dateTime.Year.ToString().Substring(2, 2);
            var day = dateTime.DayOfYear.ToString().PadLeft(3, '0');
            var dateBytes = new List<byte>();
            dateBytes.AddRange(Encoding.ASCII.GetBytes(string.Format("{0}{1}", year, day)));

            var serialNoBytes = new List<byte>();
            for (int i = 0; i < serialNo.PadLeft(4, '0').Length; i = i + 2)
            {
                serialNoBytes.Add(Convert.ToByte(serialNo.PadLeft(4, '0').Substring(i, 2), 16));
            }
            //serialNoBytes.AddRange(Encoding.ASCII.GetBytes(serialNo.PadLeft(4, '0')));

            var dicBytes = new Dictionary<byte, byte[]>
            {
                {0x01, partNoBytes.ToArray()},
                {0x03, serialNoBytes.ToArray()},
                {0x04, hardwareBytes.ToArray()},
                {0x02, dateBytes.ToArray()}
            };

            for (var i = 1; i <= 4; i++)
            {
                var enterFbl = GetLinRecvMsg(new byte[] { 0x01, 0x31, 0x32, 0x33, 0x34, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);
                if (!enterFbl.StartsWith("01FF"))
                {
                    PartNo = "NG 进入FBL模式失败";
                    Date = "NG 进入FBL模式失败";
                    SerialNo = "NG 进入FBL模式失败";
                    HardwareVersion = "NG 进入FBL模式失败";
                    return;
                }

                Thread.Sleep(100);
                var sendBytes = new byte[8];
                sendBytes[0] = 0x08;
                sendBytes[1] = (byte)i;
                Array.Copy(dicBytes[sendBytes[1]], 0, sendBytes, 2, dicBytes[sendBytes[1]].Length);

                var writeResult = GetLinRecvMsg(sendBytes, _masterLinId, _slaveLinId, 100);
                Console.WriteLine("WRTIE RECV:" + writeResult == null ? "" : writeResult);
                //lock (_lockSend)
                //    LinWithBaudRate10417.SendMasterLin(_masterLinId, sendBytes);

                Thread.Sleep(100);
                var exitFbl = GetLinRecvMsg(new byte[] { 0x02, 0x35, 0x36, 0x37, 0x38, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);
                if (!exitFbl.StartsWith("02FF"))
                {
                    PartNo = "NG 退出FBL模式失败";
                    Date = "NG 退出FBL模式失败";
                    SerialNo = "NG 退出FBL模式失败";
                    HardwareVersion = "NG 退出FBL模式失败";
                    return;
                }
            }

            Thread.Sleep(250);

            return;

            #region 追溯信息读取
            for (var i = 1; i <= 4; i++)
            {
                var readBytes = new byte[8];
                readBytes[0] = 0x09;
                readBytes[1] = (byte)i;
                var readStr = GetLinRecvMsg(readBytes, _masterLinId, _slaveLinId);

                if (i == 1 || i == 4)
                {
                    if (i == 1)
                    {
                        PartNo = readStr;
                    }
                    else if (i == 4)
                    {
                        HardwareVersion = readStr;
                    }
                }
                else
                {
                    if (readStr.StartsWith(string.Format("09{0}", ValueHelper.GetHextStr(readBytes[1]))) && readStr.Length == 16)
                    {
                        if (readStr.Substring(4, 12).StartsWith(ValueHelper.GetHextStr(dicBytes[readBytes[1]]).Replace(" ", "")))
                        {
                            switch (i)
                            {
                                case 2:
                                    Date = "OK " + readStr;
                                    break;

                                case 3:
                                    SerialNo = "OK " + readStr;
                                    break;
                            }
                        }
                        else
                        {
                            switch (i)
                            {
                                case 2:
                                    Date = "NG " + readStr;
                                    break;

                                case 3:
                                    SerialNo = "NG " + readStr;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        switch (i)
                        {
                            case 2:
                                Date = "NG " + readStr;
                                break;

                            case 3:
                                SerialNo = "NG " + readStr;
                                break;
                        }
                    }
                }
            }
            #endregion
        }

        [Description("Read供货零件号")]
        public void ReadPartNo()
        {
            PartNo = string.Empty;
            PartNo = GetLinRecvMsg(new byte[] { 0x09, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);
        }

        [Description("Read硬件版本号")]
        public void ReadHardwareVersion()
        {
            HardwareVersion = string.Empty;
            HardwareVersion = GetLinRecvMsg(new byte[] { 0x09, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);
        }

        [Description("Read生产日期")]
        public void ReadDate()
        {
            Date = string.Empty;
            var r = GetLinRecvMsg(new byte[] { 0x09, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);

            if (string.IsNullOrEmpty(r))
            {
                Date = "NG";
            }
            else
            {
                if (r.StartsWith("0902"))
                {
                    if (r.StartsWith("09020000000000"))
                    {
                        Date = "NG " + r;
                    }
                    else
                    {
                        Date = "OK " + r;
                    }
                }
                else
                {
                    Date = "NG " + r;
                }
            }
        }

        [Description("Read生产序列号")]
        public void ReadSerialNo()
        {
            SerialNo = string.Empty;
            var r = GetLinRecvMsg(new byte[] { 0x09, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);

            if (string.IsNullOrEmpty(r))
            {
                SerialNo = "NG";
            }
            else
            {
                if (r.StartsWith("0903"))
                {
                    if (r.StartsWith("09030000000000"))
                    {
                        SerialNo = "NG " + r;
                    }
                    else
                    {
                        SerialNo = "OK " + r;
                    }
                }
                else
                {
                    SerialNo = "NG " + r;
                }
            }
        }

        #endregion

        #region 电压，传感器回路

        [Description("读传感器电源电压")]
        public void ReadSensorSupplyVolt()
        {
            SensorSupplyVolt = -9999;
            var result = GetLinRecvMsg(
                new byte[] { 0x22, 0x12, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                _masterLinId, _slaveLinId);
            if (!result.StartsWith("2212") || result.Length != 16)
                return;
            var dec = (float)Convert.ToByte(result.Substring(4, 2), 16);
            SensorSupplyVolt = dec / 20;
        }

        [Description("读前后传感器信号")]
        public void ReadSensorVolt()
        {
            FrontSensorVolt = -9999;
            RearSensorVolt = -9999;

            var result = GetLinRecvMsg(
                new byte[] { 0x22, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                _masterLinId, _slaveLinId);
            if (!result.StartsWith("2211") || result.Length != 16)
                return;

            var n = (float)(Convert.ToByte(result.Substring(4, 2), 16) * 256 + Convert.ToByte(result.Substring(6, 2), 16));
            var m = (float)(Convert.ToByte(result.Substring(8, 2), 16) * 256 + Convert.ToByte(result.Substring(10, 2), 16));

            FrontSensorVolt = n / 10;
            RearSensorVolt = m / 10;
        }

        [Description("读模块电源电压")]
        public void ReadModuleSupplyVolt()
        {
            ModuleSupplyVolt = -9999;

            var result = GetLinRecvMsg(
                new byte[] { 0x22, 0x15, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                _masterLinId, _slaveLinId);
            if (!result.StartsWith("2215") || result.Length != 16)
                return;

            var dec = (float)Convert.ToByte(result.Substring(4, 2), 16);
            ModuleSupplyVolt = dec / 10 + 3;
        }

        #endregion

        #region 马达相关

        [Description("马达运动方向配置")]
        public void MotorDirectionConfig()
        {
            MotorDirectionConfigResult = string.Empty;
            MotorDirectionConfigResult = GetLinRecvMsg(new byte[] { 0x08, 0x0A, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);
        }

        [Description("DHL马达位置信息配置")]
        public void MotorPositionConfig()
        {
            MotorPositionConfigResult = string.Empty;
            MotorPositionConfigResult = GetLinRecvMsg(new byte[] { 0x08, 0x0B, 0x00, 0x96, 0x01, 0x68, 0x00, 0x96 }, _masterLinId, _slaveLinId);
        }

        [Description("控制DHL马达初始化")]
        public void MotorInit()
        {
            MotorInitResult = string.Empty;
            MotorInitResult = GetLinRecvMsg(new byte[] { 0x31, 0x04, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);
        }

        [Description("控制DHL马达运动")]
        public void MotorRun(string pos)
        {
            MotorRunResult = string.Empty;

            byte targetPos;
            if (byte.TryParse(pos, out targetPos))
                MotorRunResult = GetLinRecvMsg(new byte[] { 0x31, 0x02, 0x03, 0x00, targetPos, 0x00, targetPos, 0x00 }, _masterLinId, _slaveLinId);
        }

        [Description("读取DHL马达当前实际位置")]
        public void MotorPositionRead()
        {
            MotorPositionReadResult = string.Empty;
            MotorPositionReadResult = GetLinRecvMsg(new byte[] { 0x22, 0x13, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);
        }

        [Description("读取DHL马达开路短路故障")]
        public void MotorReadError()
        {
            MotorOpenError = string.Empty;
            MotorShortError = string.Empty;

            var r = GetLinRecvMsg(new byte[] { 0x19, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);

            if (string.IsNullOrEmpty(r) || r.Length != 16)
                return;
            //Console.WriteLine("motor error read: "+r);
            MotorOpenError = r.Substring(2, 2);
            MotorShortError = r.Substring(4, 2);
        }

        [Description("DHL马达运动到下极限确认调光角度")]
        public void MotorMove1()
        {
            var r = GetLinRecvMsg(new byte[] { 0x31, 0x02, 0x03, 0x00, 0x4E, 0x00, 0x4E, 0x00, }, _masterLinId, _slaveLinId);
            Console.WriteLine("运动到下极限确认调光角度 ：" + r);
        }

        [Description("DHL马达回到复位位置-光学零位")]
        public void MotorMove2()
        {
            var r = GetLinRecvMsg(new byte[] { 0x31, 0x02, 0x03, 0x00, 0xd2, 0x00, 0xd2, 0x00 }, _masterLinId, _slaveLinId);
            Console.WriteLine("回到复位位置（光学零位）  ：" + r);
        }

        [Description("故障清除")]
        public void ErrorClear()
        {
            ErrorClearResult = string.Empty;
            ErrorClearResult = GetLinRecvMsg(new byte[] { 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, _masterLinId, _slaveLinId);
        }

        #endregion

        #region 二维码和标签打印

        [Description("R,二维码")]
        public string Barcode;

        [Description("R/W,打印机字体")]
        public string FrontStrToPrint = "FONT001";

        [Description("R/W,打印PartNo")]
        public string HascoPartNoToPrint = "P00074683";

        [Description("R/W,打印硬件号")]
        public string HardwarPartNoToPrint = "H004";

        [Description("R/W,打印邓氏码")]
        public string DunsToPrint = "654514868";

        [Description("R/W,打印boot零件号")]
        public string FblPartNoToPrint = "AA";

        [Description("R/W,打印app零件号")]
        public string AppPartNoToPrint = "AG";

        private MySerialPort BarcodePrinter { get; set; }

        public void ConnectPrinter(string comPort)
        {
            if (comPort.StartsWith("COM"))
            {
                var port = comPort.Split(':')[0];
                var baudTate = comPort.Split(':')[1];
                BarcodePrinter =
                    new MySerialPort(port, Convert.ToInt32(baudTate), Parity.None, 8, StopBits.One);
                BarcodePrinter.MyOpen();
            }
        }

        [Description("生成二维码")]
        public void GenerateBarcode()
        {
            Barcode = string.Empty;
            //SerialNo = "OK 0903123400000000";
            //Date = "OK 0902323233333100";

            if (SerialNo.StartsWith("OK 0903") && Date.StartsWith("OK 0902"))
            {
                try
                {
                    var serialNo = GetSerialNo();
                    var year = GetYear();
                    var day = GetDay();

                    var matrixBytes = new List<byte>();
                    matrixBytes.AddRange(new byte[] { 0x5B, 0x29, 0x3E, 0x1E, 0x30, 0x36, 0x1D }); // header
                    matrixBytes.AddRange(Encoding.ASCII.GetBytes(HascoPartNoToPrint)); // hasco part number
                    matrixBytes.AddRange(new byte[] { 0x1D, 0x31, 0x32, 0x56 });
                    matrixBytes.AddRange(Encoding.ASCII.GetBytes(HardwarPartNoToPrint)); // hardware part number
                    matrixBytes.AddRange(new byte[] { 0x42, 0x56 }); // BV
                    matrixBytes.AddRange(Encoding.ASCII.GetBytes(FblPartNoToPrint)); // AA
                    matrixBytes.AddRange(new byte[] { 0x50, 0x56 }); // PV
                    matrixBytes.AddRange(Encoding.ASCII.GetBytes(AppPartNoToPrint)); // AB
                    matrixBytes.AddRange(new byte[] { 0x1D });
                    matrixBytes.AddRange(new byte[] { 0x4E }); // line new 
                    matrixBytes.AddRange(new byte[] { 0x4D }); // shift morning
                    matrixBytes.AddRange(Encoding.ASCII.GetBytes(year)); // year
                    matrixBytes.AddRange(Encoding.ASCII.GetBytes(day)); // day
                    matrixBytes.AddRange(Encoding.ASCII.GetBytes(serialNo)); // serial number
                    matrixBytes.AddRange(new byte[] { 0x1E, 0x04 }); // trailer

                    Barcode = Encoding.ASCII.GetString(matrixBytes.ToArray(), 0, matrixBytes.Count);
                }
                catch (Exception ex)
                {
                    Barcode = ex.Message;
                }
            }
        }

        [Description("Print Barcode")]
        public void PrintLabel()
        {
            if (BarcodePrinter == null ||
                !BarcodePrinter.IsOpen ||
                string.IsNullOrEmpty(Barcode))
                return;

            if (!SerialNo.StartsWith("OK 0903") || !Date.StartsWith("OK 0902"))
                return;

            try
            {
                var serialNo = GetSerialNo();
                var year = GetYear();
                var day = GetDay();

                var printSb = new StringBuilder();
                printSb.Append("SIZE 39.5 mm,27 mm\r\n");
                printSb.Append("GAP 3 mm,0 mm\r\n");
                printSb.Append("DENSITY 10\r\n");
                printSb.Append("SPEED 2\r\n");
                printSb.Append("CLS\r\n");
                printSb.Append("DIRECTION 1,0\r\n");
                printSb.Append(
                    string.Format("TEXT 30,145,\"{0}\",0,1,1,\"Part Number:{1}\"\r\n", FrontStrToPrint, HascoPartNoToPrint));
                printSb.Append(
                    string.Format("TEXT 30,175,\"{0}\",0,1,1,\"HW NO:{1}\"\r\n", FrontStrToPrint, HardwarPartNoToPrint));
                printSb.Append(
                    string.Format("TEXT 30,205,\"{0}\",0,1,1,\"DUNS:{1}\"\r\n", FrontStrToPrint, DunsToPrint));
                printSb.Append(
                    string.Format("TEXT 30,235,\"{0}\",0,1,1,\"MADE IN CHINA\"\r\n", FrontStrToPrint));
                printSb.Append(
                    string.Format("TEXT 30,265,\"{0}\",0,1,1,\"AFS4683QNM{1}{2}{3}\"\r\n", FrontStrToPrint,
                        year, day, serialNo));
                printSb.Append(
                    string.Format("DMATRIX 335,150,287,287,x5,\"{0}\"\r\n", Barcode));
                printSb.Append("PRINT 1\r\n");

                Console.WriteLine(printSb.ToString());
                var printBs = Encoding.ASCII.GetBytes(printSb.ToString());
                BarcodePrinter.Write(printBs, 0, printBs.Length);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private string GetYear()
        {
            var year =
                Date.Substring(7, 4);

            year = "20" +
                   Encoding.ASCII.GetString(new[] { Convert.ToByte(year.Substring(0, 2), 16), Convert.ToByte(year.Substring(2, 2), 16) });

            //switch (year)
            //{
            //    case "2017":
            //        year = "T";
            //        break;

            //    case "2018":
            //        year = "U";
            //        break;

            //    case "2019":
            //        year = "V";
            //        break;

            //    case "2020":
            //        year = "W";
            //        break;

            //    case "2021":
            //        year = "X";
            //        break;

            //    case "2022":
            //        year = "Y";
            //        break;

            //    case "2023":
            //        year = "Z";
            //        break;

            //    case "2088":
            //        year = "8";
            //        break;
            //}

            return year.Substring(2, 2);
        }

        private string GetDay()
        {
            var day =
                Date.Substring(11, 6);

            return Encoding.ASCII.GetString(new[]
                        {
                            Convert.ToByte(day.Substring(0, 2), 16),
                            Convert.ToByte(day.Substring(2, 2), 16),
                            Convert.ToByte(day.Substring(4, 2), 16)
                        });
        }

        private string GetSerialNo()
        {
            var serialNo =
                        SerialNo.Substring(7, 4);
            return serialNo;
        }

        #endregion

        private string GetLinRecvMsg(
            byte[] sendBytes, byte masterLinId, byte slaveLinId, int delayMs = 50)
        {
            lock (_lockSend)
            {
                if (LinWithBaudRate10417 == null)
                    return string.Empty;

                byte[] resultBytes;
                if (LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(
                    masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
                {
                    Thread.Sleep(25);
                    if (resultBytes != null && resultBytes.Length == 8)
                        return resultBytes.Aggregate(string.Empty,
                                  (current, t) => current + ValueHelper.GetHextStr(t));

                    return string.Empty;
                }

                Thread.Sleep(100);
                if (!LinWithBaudRate10417.SendMasterLinAndRecvSingleSlaveLin(
                    masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
                {
                    Thread.Sleep(25);
                    return string.Empty;
                }

                Thread.Sleep(25);
                if (resultBytes != null && resultBytes.Length == 8)
                    return resultBytes.Aggregate(string.Empty,
                        (current, t) => current + ValueHelper.GetHextStr(t));

                return string.Empty;
            }
        }
    }
}
