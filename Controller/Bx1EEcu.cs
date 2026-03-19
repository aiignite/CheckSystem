using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,BX1E-ECU")]
    public sealed class Bx1EEcu : ControllerBase
    {
        public CanBus Can;

        public Bx1EEcu(string name)
            : base(name)
        {
            if (_mainWorkThread != null)
            {
                _mainWorkThread.Abort();
                _mainWorkThread.Join();
            }
            _mainWorkThread = new Thread(Network);
            _mainWorkThread.Start();
        }

        ~Bx1EEcu()
        {
            Dispose();
        }

        private void Network()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                if (Can == null)
                    continue;

                Thread.Sleep(100);

                try
                {
                    if (!_isSleep)
                    {
                        lock (_lockSender)
                        {
                            var listPackage = new List<CanBus.CanDataPackage>
                            {
                                new CanBus.CanDataPackage(0x53F, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data, new byte[] {0x3F, 0x40, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF}),
                                new CanBus.CanDataPackage(0x169, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data,
                                    new byte[]{0xFF, 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00, _isUsbIn ? (byte) 0x02 : (byte) 0x07})
                            };

                            //new CanBus.CanDataPackage(0x20B, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //        CanBus.CanFormat.Data, new byte[] {0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00}),
                            //    new CanBus.CanDataPackage(0x18, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //        CanBus.CanFormat.Data, new byte[] {0x11, 0x00, 0x00, 0xC8, 0x00, 0x48, 0xC8, 0xC8})

                            switch (_shadowIndex)
                            {
                                case 0:
                                    listPackage.Add(new CanBus.CanDataPackage(0x20B, CanBus.CanProtocol.Can,
                                        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                        new byte[] { 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                    listPackage.Add(new CanBus.CanDataPackage(0x18, CanBus.CanProtocol.Can,
                                        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                        new byte[] { 0x11, 0x00, 0x00, 0xC8, 0x00, 0x48, 0xC8, 0xC8 }));
                                    break;

                                case 1:
                                    listPackage.Add(new CanBus.CanDataPackage(0x20B, CanBus.CanProtocol.Can,
                                        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                        new byte[] { 0x00, 0x00, 0x49, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                    listPackage.Add(new CanBus.CanDataPackage(0x18, CanBus.CanProtocol.Can,
                                        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                        new byte[] { 0x11, 0x00, 0x00, 0xC8, 0x00, 0x48, 0xC8, 0xC8 }));
                                    break;

                                case 2:
                                    listPackage.Add(new CanBus.CanDataPackage(0x20B, CanBus.CanProtocol.Can,
                                        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                        new byte[] { 0x00, 0x00, 45, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                    listPackage.Add(new CanBus.CanDataPackage(0x18, CanBus.CanProtocol.Can,
                                        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                        new byte[] { 0x11, 0x00, 0x00, 0xC8, 0x00, 0x48, 0xC8, 0xC8 }));
                                    break;

                                case 3:
                                    listPackage.Add(new CanBus.CanDataPackage(0x20B, CanBus.CanProtocol.Can,
                                        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                        new byte[] { 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                    listPackage.Add(new CanBus.CanDataPackage(0x18, CanBus.CanProtocol.Can,
                                        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                        new byte[] { 0x11, 0x20, 0x00, 0xC8, 0x00, 0x48, 0xC8, 0xC8 }));
                                    break;
                            }

                            Can.SendCanDatas(listPackage.ToArray());
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private const uint CanDiagnosisRequestPhyCanId = 0x7BD;
        //private const uint CanDiagnosisRequestFunCanId = 0x7DF;
        private const uint CanDiagnosisResponseCanId = 0x6BD;
        private readonly Thread _mainWorkThread;
        private bool _isSleep = true;
        private bool _isInExtendedSession;
        private readonly object _lockSender = new object();

        #region public method

        [Description("CAN唤醒")]
        public void CanAwake()
        {
            _isSleep = false;
        }

        [Description("CAN休眠")]
        public void CanSleep()
        {
            _isSleep = true;
        }

        [Description("R,零件号")]
        public string PartNo;
        [Description("R,零件号(Geely)")]
        public string GeelyNo;
        [Description("R,硬件版本号")]
        public string HardwareVer;
        [Description("R,软件版本号")]
        public string SoftwareVer;
        [Description("R,序列号")]
        public string SerialNo;
        [Description("R,写入并读取生产序列号")]
        public string WriteAndReadSerialNoResult;

        [Description("R,读取软件版本号(Boot 1)")]
        public string Boot1Ver;
        [Description("R,读取软件版本号(Boot 2)")]
        public string Boot2Ver;
        [Description("R,读取软件版本号(DBC Version)")]
        public string DbcVer;

        [Description("R/W,服务器IP地址")]
        public string ServerIp = "127.0.0.1";

        [Description("R/W,服务器数据库名称")]
        public string ServerDataBase = "IPMS";

        [Description("R/W,服务器用户名")]
        public string ServerUid = "sa";

        [Description("R/W,服务器用密码")]
        public string ServerPwd = "123456";

        [Description("读零件号")]
        public void ReadPartNo()
        {
            PartNo = string.Empty;
            PartNo = ReadDidViaCan(0xF1, 0xF7).GetStringByAsciiBytes(false);
        }

        [Description("零件号(Geely)")]
        public void ReadGeelyNo()
        {
            GeelyNo = string.Empty;
            GeelyNo = ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0xAE)).Replace(" ", "");
        }

        [Description("硬件版本号")]
        public void ReadHardwareVer()
        {
            HardwareVer = string.Empty;
            HardwareVer = ReadDidViaCan(0xF1, 0xF6).GetStringByAsciiBytes(false);
        }

        [Description("读取软件版本号(APP)")]
        public void ReadSoftwareVer()
        {
            SoftwareVer = string.Empty;
            SoftwareVer = ReadDidViaCan(0xF1, 0xF0).GetStringByAsciiBytes(false);
        }

        [Description("读取软件版本号Boot")]
        public void ReadBootVer()
        {
            Boot1Ver = string.Empty;
            Boot2Ver = string.Empty;

            Boot1Ver = ReadDidViaCan(0xF1, 0xF1).GetStringByAsciiBytes(false);
            Boot2Ver = ReadDidViaCan(0xF1, 0xF2).GetStringByAsciiBytes(false);
        }

        [Description("读取软件版本号(DBC Version)")]
        public void ReadDbcVer()
        {
            DbcVer = string.Empty;
            DbcVer = ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0xA0)).Replace(" ", "");
        }

        [Description("写入并读取生产序列号")]
        public void WriteAndReadSerialNo()
        {
            WriteAndReadSerialNoResult = string.Empty;

            var write = WriteSerialNo();
            var read = ReadSerialNo();

            if (write == "FFFFFFFF")
            {
                WriteAndReadSerialNoResult = string.Format("NG 写入：{0}，读取{1}", write, read);
            }
            else
            {
                if (write == read)
                {
                    WriteAndReadSerialNoResult = string.Format("OK 写入：{0}，读取{1}", write, read);
                }
                else
                {
                    WriteAndReadSerialNoResult = string.Format("NG 写入：{0}，读取{1}", write, read);
                }
            }
        }

        //[Description("读序列号")]
        private string ReadSerialNo()
        {
            SerialNo = string.Empty;
            SerialNo = ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0x8C)).Replace(" ", "");
            return SerialNo;
        }

        //[Description("测试写序列号")]
        private string WriteSerialNo()
        {
            var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
              ServerUid, ServerPwd);

            using (var conn = new SqlConnection(sqlConnectiong))
            {
                try
                {
                    var cmd = new SqlCommand("GetCheckNo", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@Productno", "BX1E");  //给输入参数赋值
                    var parOutputDate = cmd.Parameters.Add("@date", SqlDbType.DateTime);  //定义输出参数 
                    parOutputDate.Direction = ParameterDirection.Output;  //参数类型为Output 
                    var returnSerialNo = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnSerialNo.Direction = ParameterDirection.ReturnValue;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    var serialNo = returnSerialNo.Value.ToString().PadLeft(3, '0');
                    var date = parOutputDate.Value.ToString();

                    var year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
                    var day = DateTime.Parse(date).DayOfYear.ToString().PadLeft(3, '0');

                    var str = string.Format("{0}{1}{2}", year, day, serialNo);
                    var bs = new List<byte>();
                    for (var i = 0; i < str.Length; i = i + 2)
                        bs.Add(Convert.ToByte(str.Substring(i, 2), 16));

                    lock (_lockSender)
                    {
                        if (Can != null)
                        {
                            Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x8C, bs.ToArray());
                        }
                    }

                    return str;
                }
                catch (Exception)
                {
                    return "FFFFFFFF";
                }
            }
        }

        private byte[] ReadDidViaCan(byte didHi, byte didLo)
        {
            if (Can == null)
                return new List<byte>().ToArray();

            lock (_lockSender)
            {
                byte[] echoBytes;
                if (Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes, 0x00))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    return new List<byte>().ToArray();

                    //Thread.Sleep(250);
                    //if (!Can.CanBusWithUds.TryReadData(
                    //    CanDiagnosisRequestPhyCanId,
                    //    CanDiagnosisResponseCanId,
                    //    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    //    didHi, didLo, out echoBytes))
                    //    return new List<byte>().ToArray();
                    //return echoBytes ?? new List<byte>().ToArray();
                }

                //return new List<byte>().ToArray();

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                return new List<byte>().ToArray();

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();
                return echoBytes ?? new List<byte>().ToArray();
            }
        }
        #endregion

        #region session选择

        [Description("R,安全访问0506结果")]
        public string SecurityAccess0506Result = string.Empty;

        [Description("进入正常模式")]
        public void EnterDefaultSession()
        {
            if (Can == null)
                return;

            lock (_lockSender)
            {
                _isInExtendedSession = false;

                if (Can.CanBusWithUds.TryEnterDefaultSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard))
                    return;

                Thread.Sleep(500);

                Can.CanBusWithUds.TryEnterDefaultSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard);
            }
        }

        [Description("进入拓展模式")]
        public void EnterExtendedSession()
        {
            if (Can == null)
                return;

            lock (_lockSender)
            {
                if (Can.CanBusWithUds.TryEnterExtendedSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (Can.CanBusWithUds.TryEnterExtendedSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                }
            }
        }

        [Description("进入编程模式")]
        public void EnterProgramSession()
        {
            if (Can == null)
                return;

            _isInExtendedSession = false;

            lock (_lockSender)
            {
                if (Can.CanBusWithUds.TryEnterProgrammingSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    return;
                }

                Thread.Sleep(500);
                Can.CanBusWithUds.TryEnterProgrammingSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Standard);
            }
        }

        [Description("关闭正常通信")]
        public void DisableRxAndTxCommunication()
        {
            Can.CanBusWithUds.TryCommunicationControl(
                CanDiagnosisRequestPhyCanId,
                CanDiagnosisResponseCanId,
                CanBus.CanType.Standard,
                Uds14229Helper.CommunicationControl.DisableRxAndTx);
        }

        [Description("安全访问0D0E")]
        public void SecurityAccess0506()
        {
            SecurityAccess0506Result = string.Empty;

            byte[] seedBytes;
            if (Can.CanBusWithUds.TryRequestSeed(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                CanBus.CanType.Standard, 0x05, out seedBytes))
            {
                var keyBytes = GetKey(seedBytes, 5);

                if (Can.CanBusWithUds.TrySendKey(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, 0x06, keyBytes))
                {
                    SecurityAccess0506Result = "OK";
                }
            }
        }

        public static IEnumerable<byte> GetKey(byte[] seedBytes, int securetyLevel)
        {
            var keyBytes = new byte[8];
            int keyLen;

            GenerateKeyEx(
                seedBytes,
                (short)seedBytes.Length,
                securetyLevel,
                null,
                keyBytes,
                keyBytes.Length,
                out keyLen);

            var returnKey = new byte[keyLen];
            Array.Copy(keyBytes, returnKey, keyLen);
            return returnKey;
        }

        [DllImport(@"\DllImport\BX1E_GPFL_SeednKey", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GenerateKeyEx(
                byte[] iSeedArray, /* Array for the seed [in] */
                short iSeedArraySize, /* Length of the array for the seed [in] */
                int iSecurityLevel, /* Security level [in] */
                byte[] iVariant, /* Name of the active variant [in] */
                byte[] ioKeyArray, /* Array for the key [in, out] */
                int iKeyArraySize, /* Maximum length of the array for the key [in] */
                out int oSize /* Length of the key [out] */);

        #endregion

        #region usb

        private bool _isUsbIn;

        [Description("USB挂载")]
        public void UsbIn()
        {
            UsbInOut(true);
        }

        [Description("USB卸载")]
        public void UsbOut()
        {
            UsbInOut(false);
        }

        private void UsbInOut(bool isIn)
        {
            //if (Can != null)
            //{
            //    Can.SendStandardCanData(0x169, new byte[] { 0xFF, 0x00, 0x00, 0x0FF, 0x00, 0x00, 0x00, isIn ? (byte)0x02 : (byte)0x00 });
            //}
            _isUsbIn = isIn;
        }

        #endregion

        #region LED

        [Description("LCD显示ON")]
        public void LcdOn()
        {
            LcdOnOff(true);
        }

        [Description("LCD显示OFF")]
        public void LcdOff()
        {
            LcdOnOff(false);
        }

        private void LcdOnOff(bool isOn)
        {
            if (Can != null)
                Can.SendStandardCanData(0x167, new byte[] { 0x00, 0xFF, isOn ? (byte)0xC0 : (byte)0xE0, 0x00, 0x00, 0x00, 0x00, 0x00 });
        }

        private int _shadowIndex;

        [Description("功能测试1On")]
        public void Shadow1On()
        {
            _shadowIndex = 1;
        }

        [Description("功能测试2On")]
        public void Shadow2On()
        {
            _shadowIndex = 2;
        }

        [Description("功能测试3On")]
        public void Shadow3On()
        {
            _shadowIndex = 3;
        }

        [Description("功能测试Off")]
        public void ShadowOff()
        {
            _shadowIndex = 0;
        }

        #endregion

        #region 清除错误

        [Description("R,清除错误结果")]
        public string ClearDtcResult;

        [Description("清除错误")]
        public void ClearDtc()
        {
            ClearDtcResult = string.Empty;

            if (Can == null)
                return;
            lock (_lockSender)
                ClearDtcResult = Can.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(CanDiagnosisRequestPhyCanId,
                         CanDiagnosisResponseCanId, CanBus.CanType.Standard) ? "OK" : "NG";
        }

        #endregion
    }
}
