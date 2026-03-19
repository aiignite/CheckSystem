using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,SGM358ECU")]
    public sealed class Sgm358Ecu : ControllerBase
    {
        public CanBus MainCan;
        public CanBus PrivateCan;

        [Description("R/W,服务器IP地址")]
        public string ServerIp = "127.0.0.1";

        [Description("R/W,服务器数据库名称")]
        public string ServerDataBase = "IPMS";

        [Description("R/W,服务器用户名")]
        public string ServerUid = "sa";

        [Description("R/W,服务器用密码")]
        public string ServerPwd = "123456";

        public Sgm358Ecu(string name)
            : base(name)
        {
            if (_mainWorkThread != null)
            {
                _mainWorkThread.Abort();
                _mainWorkThread.Join();
            }
            _mainWorkThread = new Thread(AwakenTimer);
            _mainWorkThread.Start();
        }

        ~Sgm358Ecu()
        {
            Dispose();
        }

        private readonly object _lockCan = new object();
        private int _periodCount;
        private bool _isSleeping = true;
        private bool _isInExtendedSession;
        private readonly Thread _mainWorkThread;
        private const uint ReqCanId = 0x14DA72F1;
        private const uint RecvCanId = 0x14DAF172;

        private void AwakenTimer()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                _periodCount++;

                if (_periodCount > 150)
                    _periodCount = 0;

                try
                {
                    if (MainCan == null)
                        return;

                    lock (_lockCan)
                    {
                        if (!_isSleeping)
                        {
                            MainCan.SendStandardCanData(
                                0x625, new byte[] { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 });
                            MainCan.SendStandardCanData(
                                0x1F1, new byte[] { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 });
                        }

                        if (_isInExtendedSession && _periodCount == 50)
                            MainCan.SendStandardCanData(ReqCanId,
                                new byte[] { 0x02, 0x3E, 0x00, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA });
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        #region 主要控制

        [Description("唤醒")]
        public void AhlActivate()
        {
            _isSleeping = false;
        }

        [Description("休眠")]
        public void AhlNotActivate()
        {
            _isSleeping = true;
        }

        [Description("进入正常模式")]
        public void EnterDefaultSession()
        {
            if (MainCan == null)
                return;

            lock (_lockCan)
            {
                _isInExtendedSession = false;

                if (MainCan.CanBusWithUds.TryEnterDefaultSession(
                    ReqCanId,
                    RecvCanId, CanBus.CanType.Extended))
                    return;

                Thread.Sleep(500);

                MainCan.CanBusWithUds.TryEnterDefaultSession(
                    ReqCanId,
                    RecvCanId, CanBus.CanType.Extended);
            }
        }

        [Description("进入拓展模式")]
        public void EnterExtendedSession()
        {
            if (MainCan == null)
                return;

            lock (_lockCan)
            {
                if (MainCan.CanBusWithUds.TryEnterExtendedSession(
                    ReqCanId,
                    RecvCanId, CanBus.CanType.Extended))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (MainCan.CanBusWithUds.TryEnterExtendedSession(
                    ReqCanId,
                    RecvCanId, CanBus.CanType.Extended))
                {
                    _isInExtendedSession = true;
                }
            }
        }

        [Description("进入编程模式")]
        public void EnterProgramSession()
        {
            if (MainCan == null)
                return;

            _isInExtendedSession = false;

            lock (_lockCan)
            {
                if (MainCan.CanBusWithUds.TryEnterProgrammingSession(
                    ReqCanId,
                    RecvCanId, CanBus.CanType.Extended))
                {
                    return;
                }

                Thread.Sleep(500);
                MainCan.CanBusWithUds.TryEnterProgrammingSession(
                    ReqCanId,
                    RecvCanId, CanBus.CanType.Extended);
            }
        }

        [Description("解锁SeedKey")]
        public bool SecurityAccess(string subFunc)
        {
            if (MainCan == null)
                return false;

            byte securitySubFunc;
            if (!byte.TryParse(subFunc, out securitySubFunc))
                return false;

            if (securitySubFunc != 0x01)
                return false;

            var subFunctoion = securitySubFunc;

            lock (_lockCan)
            {
                byte[] seedBytes;
                if (!MainCan.CanBusWithUds.TryRequestSeed(
                    ReqCanId, RecvCanId,
                    CanBus.CanType.Extended, subFunctoion, out seedBytes))
                    return false;

                if (seedBytes == null || seedBytes.Length != 5)
                    return false;

                return true;
                //var keyBytes = new byte[] { 0xa1, 0xb2, 0xc3, 0xd4, 0xe5 };

                //return MainCan.CanBusWithUds.TrySendKey(
                //    ReqCanId, RecvCanId,
                //    CanBus.CanType.Extended, (byte)(subFunctoion + 0x01), keyBytes);
            }
        }

        #endregion

        #region 版本信息

        [Description("R,读ECU_ID-01F3")]
        public string EcuId;
        [Description("R,读引导程序零件号-01C0")]
        public string BootLoaderPartNumber;
        [Description("R,读应用程序零件号-01C1")]
        public string ApplicationSoftwarePartNumber;
        [Description("R,读配置文件零件号-01C2")]
        public string CalibrationSoftwarePartNumber;
        [Description("R,读当前零件号-01CB")]
        public string EndModulePartNumber;
        [Description("R,读初始零件号-F191")]
        public string BaseModulePartNumber;
        [Description("R,读引导程序版本号-01D0")]
        public string BootLoaderAlphaCode;
        [Description("R,读应用程序版本号-01D1")]
        public string ApplicationSoftwareAlphaCode;
        [Description("R,读配置文件版本号-01D2")]
        public string CalibrationSoftwareAlphaCode;
        [Description("R,读当前版本号-01DB")]
        public string EndModuleAlphaCode;
        [Description("R,读初始版本号-01DC")]
        public string BaseModuleAlphaCode;
        [Description("R,读MEC值-01A0")]
        public double Mec = -9999;
        [Description("R,读DDI值-019A")]
        public string Ddi;
        [Description("R,读ECU供电电压-F088")]
        public double EcuVolt = -9999;
        [Description("R,读传感器电源回路-8240")]
        public double SensorPowerCircuit;
        [Description("R,读前传感器信号回路-832D")]
        public double FrontSensorSignalLoop = -9999;
        [Description("R,读后传感器信号回路-832E")]
        public double RearSensorSignalLoop = -9999;
        [Description("R,清除错误结果")]
        public string ClearFaultMemoryResult;
        [Description("R,读取错误结果-190209")]
        public string ReadrFaultMemoryResult;
        [Description("R,读取VPPS-01AB")]
        public string Vpps;
        [Description("R,读取DUNS-01B3")]
        public string Duns;

        /// <summary>
        /// 读ECU_ID
        /// 0x01F3
        /// </summary>
        [Description("读ECU_ID-01F3")]
        public void ReadEcuId()
        {
            EcuId = "NG";
            var result = MainCanReadDid(0x01, 0xF3);
            if (result == null)
                return;

            try
            {
                var temp = result.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                if (!string.IsNullOrEmpty("temp"))
                {
                    EcuId = "OK " + temp;
                }
                else
                {
                    EcuId = "NG " + temp;
                }
            }
            catch (Exception ex)
            {
                EcuId = "NG " + ex.Message;
            }
        }

        //[Description("写ECU_ID-D004")]
        //public void WriteEcuId()
        //{
        //    string partNo = "358";
        //    string baseModulePartNumber = "26381122";
        //    string fna = "9770A";
        //    string upc1 = "1";
        //    string upc2 = "2C";
        //    string baseModuleAlphaCode = "AA";
        //    string endModulePartNumber = "26381122";
        //    string endModuleAlphaCode = "AA";

        //    if (MainCan == null)
        //        return;

        //    var sqlConnectiong =
        //        string.Format(@"server={0};database={1};uid={2};pwd={3}",
        //        ServerIp, ServerDataBase, ServerUid, ServerPwd);

        //    string serialNo;
        //    string date;
        //    using (var conn = new SqlConnection(sqlConnectiong))
        //    {
        //        try
        //        {
        //            var cmd = new SqlCommand("GetCheckNo", conn) { CommandType = CommandType.StoredProcedure };
        //            cmd.Parameters.AddWithValue("@Productno", partNo);  //给输入参数赋值
        //            var parOutputDate = cmd.Parameters.Add("@date", SqlDbType.DateTime);  //定义输出参数 
        //            parOutputDate.Direction = ParameterDirection.Output;  //参数类型为Output 
        //            var returnSerialNo = cmd.Parameters.Add("@return_value", SqlDbType.Int);
        //            returnSerialNo.Direction = ParameterDirection.ReturnValue;
        //            conn.Open();
        //            cmd.ExecuteNonQuery();

        //            serialNo = returnSerialNo.Value.ToString().PadLeft(8, '0');
        //            date = parOutputDate.Value.ToString();
        //        }
        //        catch (Exception)
        //        {
        //            Date = "NG 生成生产日期及序列号失败";
        //            SerialNo = "NG 生成生产日期及序列号失败";
        //            return;
        //        }
        //    }

        //    var listBytes = new List<byte>();

        //     add Sequence Number
        //    for (var i = 0; i < serialNo.Length; i = i + 2)
        //    {
        //        var b = Convert.ToByte(serialNo.Substring(i, 2), 16);
        //        listBytes.Add(b);
        //    }

        //     add Base Module Part Number
        //    listBytes.AddRange(BitConverter.GetBytes(uint.Parse(baseModulePartNumber)).Reverse());

        //     add FNA
        //    listBytes.AddRange(Encoding.ASCII.GetBytes(fna));

        //     add UPC_1
        //    listBytes.Add(Convert.ToByte(upc1.PadLeft(2, '0'), 16));

        //     add UPC_2
        //    listBytes.AddRange(Encoding.ASCII.GetBytes(upc2));

        //     add Base Module Part Number
        //    listBytes.AddRange(BitConverter.GetBytes(uint.Parse(baseModulePartNumber)).Reverse());

        //     add Base Module Alpha Code
        //    listBytes.AddRange(Encoding.ASCII.GetBytes(baseModuleAlphaCode));

        //     add End Module Part Number
        //    listBytes.AddRange(BitConverter.GetBytes(uint.Parse(endModulePartNumber)).Reverse());

        //     add End Module Alpha Code
        //    listBytes.AddRange(Encoding.ASCII.GetBytes(endModuleAlphaCode));

        //    lock (_lockCan)
        //    {
        //        MainCan.CanBusWithUds.TryWriteData(ReqCanId, RecvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can,
        //            0xD0, 0x04, listBytes);

        //        ReadEcuId();
        //        if (!string.IsNullOrEmpty(EcuId) && EcuId.StartsWith("OK "))
        //        {
        //            var tempEcuId = EcuId.Replace("OK ", "");
        //            var temp = string.Empty;

        //            for (var i = 0; i < 16; i++)
        //            {
        //                temp += ValueHelper.GetHextStr(listBytes[i]);
        //            }

        //            if (tempEcuId == temp)
        //            {
        //                EcuId = "OK " + tempEcuId;
        //            }
        //            else
        //            {
        //                EcuId = string.Format("NG 写入：{0}，读取：{1}", temp, tempEcuId);
        //            }
        //        }
        //    }
        //}


        [Description("读ECU_ID-D004")]
        public void WriteEcuId(
            string partNo,
            string baseModulePartNumber,
            string fna, string upc1, string upc2,
            string baseModuleAlphaCode, string endModulePartNumber, string endModuleAlphaCode)
        {
            if (MainCan == null)
                return;

            EcuId = string.Empty;

            var sqlConnectiong =
                string.Format(@"server={0};database={1};uid={2};pwd={3}",
                ServerIp, ServerDataBase, ServerUid, ServerPwd);

            var serialIdentifier = string.Empty;
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

                    var serialNo = returnSerialNo.Value.ToString().PadLeft(4, '0');
                    var date = parOutputDate.Value.ToString();

                    if (int.Parse(serialNo) > 9999)
                        return;

                    var year = DateTime.Parse(date).Year;
                    var dayOfYear = DateTime.Parse(date).DayOfYear;

                    if (year >= 2020 && year <= 2029)
                    {
                        year = 2029 - year;

                        serialIdentifier =
                            string.Format("{0}{1}{2}", year,
                                dayOfYear.ToString().PadLeft(3, '0'), serialNo).PadLeft(8, '0');
                    }
                }
                catch (Exception)
                {
                    EcuId = "NG 为从服务器申请到可用的生产日期及序列号";
                    //Date = "NG 生成生产日期及序列号失败";
                    //SerialNo = "NG 生成生产日期及序列号失败";
                    return;
                }
            }

            if (string.IsNullOrEmpty(serialIdentifier))
                return;

            var listBytes = new List<byte>();

            // add Sequence Number


            for (var i = 0; i < serialIdentifier.Length; i = i + 2)
            {
                var b = Convert.ToByte(serialIdentifier.Substring(i, 2), 16);
                listBytes.Add(b);
            }

            // add Base Module Part Number
            listBytes.AddRange(BitConverter.GetBytes(uint.Parse(baseModulePartNumber)).Reverse());

            // add FNA
            listBytes.AddRange(Encoding.ASCII.GetBytes(fna));

            // add UPC_1
            listBytes.Add(Convert.ToByte(upc1.PadLeft(2, '0'), 16));

            // add UPC_2
            listBytes.AddRange(Encoding.ASCII.GetBytes(upc2));

            // add Base Module Part Number
            listBytes.AddRange(BitConverter.GetBytes(uint.Parse(baseModulePartNumber)).Reverse());

            // add Base Module Alpha Code
            listBytes.AddRange(Encoding.ASCII.GetBytes(baseModuleAlphaCode));

            // add End Module Part Number
            listBytes.AddRange(BitConverter.GetBytes(uint.Parse(endModulePartNumber)).Reverse());

            // add End Module Alpha Code
            listBytes.AddRange(Encoding.ASCII.GetBytes(endModuleAlphaCode));

            lock (_lockCan)
            {
                MainCan.CanBusWithUds.TryWriteData(ReqCanId, RecvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can,
                    0xD0, 0x04, listBytes);

                ReadEcuId();
                if (!string.IsNullOrEmpty(EcuId) && EcuId.StartsWith("OK "))
                {
                    var tempEcuId = EcuId.Replace("OK ", "");
                    var temp = string.Empty;

                    for (var i = 0; i < 16; i++)
                    {
                        temp += ValueHelper.GetHextStr(listBytes[i]);
                    }

                    if (tempEcuId == temp)
                    {
                        EcuId = "OK " + tempEcuId;
                    }
                    else
                    {
                        EcuId = string.Format("NG 写入：{0}，读取：{1}", temp, tempEcuId);
                    }
                }
            }
        }

        /// <summary>
        /// 读引导程序零件号
        /// 0x01C0
        /// </summary>
        [Description("读引导程序零件号-01C0")]
        public void ReadBootLoaderPartNumber()
        {
            BootLoaderPartNumber = string.Empty;
            var result = MainCanReadDid(0x01, 0xC0);
            //var result = Read(0xF1, 0xC0).Invoke();
            try
            {
                if (result != null)
                    BootLoaderPartNumber = ValueHelper.GetDecimal(result).ToString();
            }
            catch (Exception ex)
            {
                BootLoaderPartNumber = ex.Message;
            }
        }

        /// <summary>
        /// 读引导程序版本号
        /// 0x01D0
        /// </summary>
        [Description("读引导程序版本号-01D0")]
        public void ReadBootLoaderAlphaCode()
        {
            BootLoaderAlphaCode = string.Empty;
            var result = MainCanReadDid(0x01, 0xD0);
            //var result = Read(0xF1, 0xD0).Invoke();
            try
            {
                if (result == null)
                    return;
                BootLoaderAlphaCode = result.GetStringByAsciiBytes(false);
            }
            catch (Exception ex)
            {
                BootLoaderAlphaCode = ex.Message;
            }
        }

        /// <summary>
        /// 读初始零件号
        /// 0xF191
        /// </summary>
        [Description("读初始零件号-F191")]
        public void ReadBaseModulePartNumber()
        {
            BaseModulePartNumber = string.Empty;
            try
            {
                var result = MainCanReadDid(0xF1, 0x91);
                if (result == null)
                    return;
                BaseModulePartNumber = ValueHelper.GetDecimal(result).ToString();
            }
            catch (Exception ex)
            {
                BaseModulePartNumber = ex.Message;
            }
        }

        /// <summary>
        /// 读初始版本号
        /// 0x01DC
        /// </summary>
        [Description("读初始版本号-01DC")]
        public void ReadBaseModuleAlphaCode()
        {
            BaseModuleAlphaCode = string.Empty;
            var result = MainCanReadDid(0x01, 0xDC);
            try
            {
                if (result == null)
                    return;
                BaseModuleAlphaCode = result.GetStringByAsciiBytes(false);
            }
            catch (Exception ex)
            {
                BaseModuleAlphaCode = ex.Message;
            }
        }

        /// <summary>
        /// 读当前零件号
        /// 0x01CB
        /// </summary>
        [Description("读当前零件号-01CB")]
        public void ReadEndModulePartNumber()
        {
            EndModulePartNumber = string.Empty;
            var result = MainCanReadDid(0x01, 0xCB);
            try
            {
                if (result == null)
                    return;
                EndModulePartNumber = ValueHelper.GetDecimal(result).ToString();
            }
            catch (Exception ex)
            {
                EndModulePartNumber = ex.Message;
            }
        }

        /// <summary>
        /// 读当前版本号
        /// 0x01DB
        /// </summary>
        [Description("读当前版本号-01DB")]
        public void ReadEndModuleAlphaCode()
        {
            EndModuleAlphaCode = string.Empty;
            var result = MainCanReadDid(0x01, 0xDB);
            try
            {
                if (result == null)
                    return;
                EndModuleAlphaCode = result.GetStringByAsciiBytes(false);
            }
            catch (Exception ex)
            {
                EndModuleAlphaCode = ex.Message;
            }
        }

        /// <summary>
        /// 读应用程序零件号
        /// 0x01C1
        /// </summary>
        [Description("读应用程序零件号-01C1")]
        public void ReadApplicationSoftwarePartNumber()
        {
            ApplicationSoftwarePartNumber = string.Empty;
            var result = MainCanReadDid(0x01, 0xC1);
            try
            {
                if (result == null)
                    return;
                ApplicationSoftwarePartNumber =
                    ValueHelper.GetDecimal(result).ToString();
            }
            catch (Exception ex)
            {
                ApplicationSoftwarePartNumber = ex.Message;
            }
        }

        /// <summary>
        /// 读应用程序版本号
        /// 0x01D1
        /// </summary>
        [Description("读应用程序版本号-01D1")]
        public void ReadApplicationSoftwareAlphaCode()
        {
            ApplicationSoftwareAlphaCode = string.Empty;
            var result = MainCanReadDid(0x01, 0xD1);
            try
            {
                if (result == null)
                    return;
                ApplicationSoftwareAlphaCode = result.GetStringByAsciiBytes(false);
            }
            catch (Exception ex)
            {
                ApplicationSoftwareAlphaCode = ex.Message;
            }
        }

        /// <summary>
        /// 读配置文件零件号
        /// 0x01C2
        /// </summary>
        [Description("读配置文件零件号-01C2")]
        public void ReadCalibrationSoftwarePartNumber()
        {
            CalibrationSoftwarePartNumber = string.Empty;
            var result = MainCanReadDid(0x01, 0xC2);
            try
            {
                if (result == null)
                    return;
                CalibrationSoftwarePartNumber = ValueHelper.GetDecimal(result).ToString();
            }
            catch (Exception ex)
            {
                CalibrationSoftwarePartNumber = ex.Message;
            }
        }

        /// <summary>
        /// 读配置文件版本号
        /// 0x01D2
        /// </summary>
        [Description("读配置文件版本号-01D2")]
        public void ReadCalibrationSoftwareAlphaCode()
        {
            CalibrationSoftwareAlphaCode = string.Empty;
            var result = MainCanReadDid(0x01, 0xD2);
            try
            {
                if (result == null)
                    return;
                CalibrationSoftwareAlphaCode = result.GetStringByAsciiBytes(false);
            }
            catch (Exception ex)
            {
                CalibrationSoftwareAlphaCode = ex.Message;
            }
        }

        private void WriteMec(string hex)
        {
            if (MainCan == null)
                return;

            var bytes = new List<byte> { Convert.ToByte(hex, 16) };
            if (MainCan.CanBusWithUds.TryWriteData(
                ReqCanId, RecvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0x01, 0xA0,
                bytes))
                return;

            Thread.Sleep(500);
            if (MainCan.CanBusWithUds.TryWriteData(ReqCanId, RecvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0x01, 0xA0,
                bytes))
                return;

            Thread.Sleep(500);
            MainCan.CanBusWithUds.TryWriteData(ReqCanId, RecvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0x01, 0xA0,
                bytes);
        }

        /// <summary>
        /// 读MEC值
        /// 0x01A0
        /// </summary>
        [Description("读MEC值-01A0")]
        public void ReadMec()
        {
            Mec = -9999;
            var result = MainCanReadDid(0x01, 0xA0);
            if (result == null)
                return;
            try
            {
                Mec = result[0];
            }
            catch (Exception)
            {
                Mec = -9999;
            }
        }

        /// <summary>
        /// 读DDI值
        /// 0x019A
        /// </summary>
        [Description("读DDI值-019A")]
        public void ReadDdi()
        {
            Ddi = string.Empty;
            var result = MainCanReadDid(0x01, 0x9A);
            if (result == null)
                return;
            foreach (var t in result)
                Ddi += ValueHelper.GetHextStrWithOx(t) + " ";
            Ddi = Ddi.Trim();
        }

        /// <summary>
        /// 读ECU供电电压
        /// 0xF088
        /// </summary>
        [Description("读ECU供电电压-F088")]
        public void ReadEcuVolt()
        {
            EcuVolt = -9999;
            var result = MainCanReadDid(0xF0, 0x88);
            if (result == null)
                return;
            try
            {
                EcuVolt = BitConverter.ToUInt16(result, 0) * 0.1 + 3;
            }
            catch (Exception)
            {
                EcuVolt = -9999;
            }
        }

        /// <summary>
        /// 读传感器电源回路
        /// 0x8240
        /// </summary>
        [Description("读传感器电源回路-8240")]
        public void ReadSensorPowerCircuit()
        {
            SensorPowerCircuit = -9999;
            var result = MainCanReadDid(0x82, 0x40);
            if (result == null)
                return;
            try
            {
                var x1 = (double)result[0];
                var x2 = (double)20;
                SensorPowerCircuit = x1 / x2;
            }
            catch (Exception)
            {
                SensorPowerCircuit = -9999;
            }
        }

        /// <summary>
        /// 读前传感器信号回路
        /// 0x832D
        /// </summary>
        [Description("读前传感器信号回路-832D")]
        public void ReadFrontSensorSignalLoop()
        {
            FrontSensorSignalLoop = -9999;
            var result = MainCanReadDid(0x83, 0x2D);
            if (result == null || result.Length != 2)
                return;
            try
            {
                var x1 = (double)(result[0] * 256 + result[1]);
                var x2 = (double)5;
                var x3 = (double)1024;
                FrontSensorSignalLoop = x1 * x2 / x3;
            }
            catch (Exception)
            {
                FrontSensorSignalLoop = -9999;
            }
        }

        /// <summary>
        /// 读后传感器信号回路
        /// 0x832E
        /// </summary>
        [Description("读后传感器信号回路-832E")]
        public void ReadRearSensorSignalLoop()
        {
            RearSensorSignalLoop = -9999;
            var result = MainCanReadDid(0x83, 0x2E);
            if (result == null || result.Length != 2)
                return;
            try
            {
                var x1 = (double)(result[0] * 256 + result[1]);
                var x2 = (double)5;
                var x3 = (double)1024;
                RearSensorSignalLoop = x1 * x2 / x3;
            }
            catch (Exception)
            {
                RearSensorSignalLoop = -9999;
            }
        }

        /// <summary>
        /// 读取VPPS
        /// 0x01AB
        /// </summary>
        [Description("读取VPPS-01AB")]
        public void ReadVpps()
        {
            Vpps = string.Empty;
            var result = MainCanReadDid(0x01, 0xAB);
            if (result == null)
                return;
            Vpps = result.GetStringByAsciiBytes(false);
        }

        /// <summary>
        /// 读取DUNS
        /// 0x01B3
        /// </summary>
        [Description("读取DUNS-01B3")]
        public void ReadDuns()
        {
            Duns = string.Empty;
            var result = MainCanReadDid(0x01, 0xB3);
            if (result == null)
                return;
            Duns = result.GetStringByAsciiBytes(false);
        }

        /// <summary>
        /// 读取追溯信息
        /// 0x01B4
        /// </summary>
        //public void ReadTrackInfo()
        //{
        //    TraskInfo = "NG";
        //    var result = Read(0x01, 0xB4).Invoke();
        //    if (result == null)
        //        return;
        //    try
        //    {
        //        var lineId = string.Format("[产线号_{0}]", Encoding.ASCII.GetString(result, 0, 1));
        //        var shiftId = string.Format("[班次_{0}]", Encoding.ASCII.GetString(result, 1, 1));
        //        var year = string.Format("[年_{0}]", Encoding.ASCII.GetString(result, 2, 2));
        //        var day = string.Format("[天_{0}]", Encoding.ASCII.GetString(result, 4, 3));
        //        var a = string.Format("[追溯_{0}]", Encoding.ASCII.GetString(result, 7, 1));
        //        var str2B = string.Format("[工程更改记录_{0}]", Encoding.ASCII.GetString(result, 8, 2));
        //        var randomNo = string.Format("[随机数_{0}]", Encoding.ASCII.GetString(result, 10, 6));
        //        TraskInfo = "OK " + lineId + shiftId + year + day + a + str2B + randomNo;
        //    }
        //    catch (Exception)
        //    {
        //        var temp =
        //            result.Aggregate(string.Empty, (current, t) => current + t.GetHextStr() + " ");
        //        temp = temp.Trim();
        //        TraskInfo += " " + temp;
        //    }
        //}

        /// <summary>
        /// 读取故障
        /// 0x19
        /// </summary>\
        [Description("读取故障-190209")]
        public void ReadFaultMemory()
        {
            ReadrFaultMemoryResult = string.Empty;

            var dataList = new byte[] { 0x19, 0x02, 0x09 };
            byte[] outBytes;
            if (
                MainCan.CanBusWithUds.TesterTryRequest(ReqCanId, RecvCanId, dataList.ToList(), out outBytes,
                    CanBus.CanType.Extended))
            {
                if (outBytes == null)
                    return;
                ReadrFaultMemoryResult = ValueHelper.GetHextStr(outBytes);
            }
            else
            {
                if (
                    !MainCan.CanBusWithUds.TesterTryRequest(ReqCanId, RecvCanId, dataList.ToList(), out outBytes,
                        CanBus.CanType.Extended))
                    return;
                if (outBytes == null)
                    return;
                ReadrFaultMemoryResult = ValueHelper.GetHextStr(outBytes);
            }
        }

        /// <summary>
        /// 清除故障
        /// 0x04
        /// </summary>
        [Description("清除故障")]
        public void ClearFaultMemory()
        {
            ClearFaultMemoryResult = "NG";
            var clearFaultBytes = new byte[] { 0x14, 0xFF, 0xFF, 0xFF };
            byte[] outBytes;
            if (MainCan.CanBusWithUds.TesterTryRequest(ReqCanId, RecvCanId, clearFaultBytes, out outBytes, CanBus.CanType.Extended))
            {
                if (outBytes != null && outBytes.Length >= 1 && outBytes[0] == 0x54)
                {
                    ClearFaultMemoryResult = "OK";
                    return;
                }
            }
            else
            {
                Thread.Sleep(500);

                if (MainCan.CanBusWithUds.TesterTryRequest(ReqCanId, RecvCanId, clearFaultBytes, out outBytes, CanBus.CanType.Extended))
                {
                    if (outBytes != null && outBytes.Length >= 1 && outBytes[0] == 0x54)
                    {
                        ClearFaultMemoryResult = "OK";
                        return;
                    }
                }
            }

            ClearFaultMemoryResult = "NG";
        }

        #endregion

        private byte[] MainCanReadDid(byte didHi, byte didLo)
        {
            if (MainCan == null)
                return new List<byte>().ToArray();

            lock (MainCan)
            {
                byte[] echoBytes;
                if (MainCan.CanBusWithUds.TryReadData(
                    ReqCanId,
                    RecvCanId,
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    return new List<byte>().ToArray();
                }

                Thread.Sleep(200);

                if (MainCan.CanBusWithUds.TryReadData(
                    ReqCanId,
                    RecvCanId,
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    return new List<byte>().ToArray();
                }

                return new List<byte>().ToArray();
            }
        }
    }
}
