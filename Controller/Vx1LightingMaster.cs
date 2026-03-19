using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    public sealed class Vx1LightingMaster : ControllerBase
    {
        public Vx1LightingMaster(string name) :
            base(name)
        {
            _keepNetworkThread = new Thread(KeepNetwork) { IsBackground = true };
            _keepNetworkThread.Start();

            _keepExtendedSessionThread = new Thread(KeepExtendedSession) { IsBackground = true };
            _keepExtendedSessionThread.Start();
        }

        private readonly Thread _keepExtendedSessionThread;
        private readonly Thread _keepNetworkThread;
        private bool _isInExtendedSession;
        private bool _isSleep = true;
        private uint _diagnosisRequestPhyCanId = 0x715;
        private uint _diagnosisRequestFunCanId = 0x7DF;
        private uint _diagnosisResponseCanId = 0x775;

        public CanBus BodyCan;
        public CanBus SubCan;

        public string BodyCanIsHaveMsgCheckResult = string.Empty;
        public string SubCanIsHaveMsgCheckResult = string.Empty;
        public string BodyCanIsNotHaveMsgCheckResult = string.Empty;
        public string SubCanIsNotHaveMsgCheckResult = string.Empty;
        public string EthernetCheckResult = string.Empty;

        [Description("R,HardwarePartNo")]
        public string HardwarePartNo;
        [Description("R,HardwareVersion")]
        public string HardwareVersion;
        [Description("R,SoftwarePartNo")]
        public string SoftwarePartNo;
        [Description("R,SoftwareVersion")]
        public string SoftwareVersion;
        public string ProductionDate;
        public string SerialNo;
        public double SystemTemperature;
        public double SystemVoltage;
        public string ClearDiagnosticInfomationResult;
        public string DiagnosticInfomation;
        public string DlpLightingMasterErrRsp;
        public string DlpCameraErrRsp;
        public string Barcode = string.Empty;
        public string FpdLinkErrorInfo;
        public string ConfirmBarcodeResult = string.Empty;

        public void ConfirmBarcode()
        {
            if (string.IsNullOrEmpty(ConfirmBarcodeResult))
            {
                ConfirmBarcodeResult = "NG";
                return;
            }

            if (string.IsNullOrEmpty(SerialNo))
            {
                ConfirmBarcodeResult = "NG";
                return;
            }

            var barcode1 = SerialNo.Replace("OK ", "");
            //GetBarcode(Barcode);
            var barcode2 = ConfirmBarcodeResult.Replace("\r", "").Replace("\n", "");
            //GetBarcode(ConfirmBarcodeResult);
            if (!string.IsNullOrEmpty(barcode2) && !string.IsNullOrEmpty(barcode1))
            {
                if (barcode2.EndsWith(barcode1))
                {
                    ConfirmBarcodeResult = "OK " + barcode2;
                    return;
                }
                else
                {

                    ConfirmBarcodeResult = "NG " + barcode2;
                    return;
                }
            }

            ConfirmBarcodeResult = "NG";
        }

        private void KeepNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                if (BodyCan == null)
                    continue;

                if (!_isSleep)
                {
                    BodyCan.SendStandardCanData(0x224, new byte[8]);
                    BodyCan.SendStandardCanData(0x2CC, new byte[] { 0xAE, 0x06, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00 });
                }
            }
        }

        [Description("唤醒")]
        public void Awake()
        {
            _isSleep = false;
        }

        [Description("休眠")]
        public void Sleep()
        {
            _isSleep = true;
        }

        private void KeepExtendedSession()
        {
            while (_keepExtendedSessionThread.IsAlive)
            {
                if (!_keepExtendedSessionThread.IsAlive)
                    break;

                Thread.Sleep(1200);

                if (BodyCan == null)
                    continue;

                if (_isSleep)
                    continue;

                if (_isInExtendedSession)
                {
                    BodyCan.SendStandardCanData(_diagnosisRequestPhyCanId, new byte[] { 0x02, 0x3e, 0x00, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA });
                }
            }
        }

        public void ExecuteBodyCanIsHaveMsgCheck()
        {
            BodyCanIsHaveMsgCheckResult = @"NG";

            if (BodyCan == null)
                return;

            var findCnaIdList = new List<uint> { 0x2e2, 0x2e3 };

            foreach (var canId in findCnaIdList)
                BodyCan.AddDoNotFilterCanId(canId);

            BodyCan.CanRecvDataPackages.Clear();
            Thread.Sleep(1000);

            var isOk = findCnaIdList.All(canId => BodyCan.CanRecvDataPackages.FindAll(f => f.CanId == canId).Any());

            if (isOk)
                BodyCanIsHaveMsgCheckResult = @"OK";

            foreach (var canId in findCnaIdList)
                BodyCan.RemoveDoNotFilterCanId(canId);
        }

        public void ExecuteSubCanIsHaveMsgCheck()
        {
            SubCanIsHaveMsgCheckResult = @"NG";

            if (SubCan == null)
                return;

            var findCnaIdList = new List<uint> { 0x20 };

            foreach (var canId in findCnaIdList)
                SubCan.AddDoNotFilterCanId(canId);

            SubCan.CanRecvDataPackages.Clear();
            Thread.Sleep(1000);

            var isOk = findCnaIdList.All(canId => SubCan.CanRecvDataPackages.FindAll(f => f.CanId == canId).Any());

            if (isOk)
                SubCanIsHaveMsgCheckResult = @"OK";

            foreach (var canId in findCnaIdList)
                SubCan.RemoveDoNotFilterCanId(canId);
        }

        public void ExecuteBodyCanIsNotHaveMsgCheck()
        {
            BodyCanIsNotHaveMsgCheckResult = @"NG";

            if (BodyCan == null)
                return;

            var findCnaIdList = new List<uint> { 0x2e2, 0x2e3 };

            foreach (var canId in findCnaIdList)
                BodyCan.AddDoNotFilterCanId(canId);

            BodyCan.CanRecvDataPackages.Clear();
            Thread.Sleep(1000);

            var isOk = findCnaIdList.All(canId => !BodyCan.CanRecvDataPackages.FindAll(f => f.CanId == canId).Any());

            if (isOk)
                BodyCanIsNotHaveMsgCheckResult = @"OK";
        }

        public void ExecuteSubCanIsNotHaveMsgCheck()
        {
            SubCanIsNotHaveMsgCheckResult = @"NG";

            if (SubCan == null)
                return;

            var findCnaIdList = new List<uint> { 0x20 };

            foreach (var canId in findCnaIdList)
                SubCan.AddDoNotFilterCanId(canId);

            SubCan.CanRecvDataPackages.Clear();
            Thread.Sleep(1000);

            var isOk = findCnaIdList.All(canId => !SubCan.CanRecvDataPackages.FindAll(f => f.CanId == canId).Any());

            if (isOk)
                SubCanIsNotHaveMsgCheckResult = @"OK";
        }

        public void ExecuteEthernetCheck()
        {
            EthernetCheckResult = @"NG";
            //return;

            if (BodyCan == null)
                return;

            BodyCan.AddDoNotFilterCanId(_diagnosisResponseCanId);
            EnterExtendedSession();
            Thread.Sleep(25);
            SecurityAccess();
            Thread.Sleep(25);

            //Thread.Sleep(50);

            //Task.Factory.StartNew(() =>
            //{
            //    // ping 192.168.0.136
            //    // 远程有响应就是用远程SERVER
            //    // 远程无响应就是用本地SERVER
            //    var p1 = new Ping();
            //    try
            //    {
            //        var reply = p1.Send("192.168.2.14"); //发送主机名或Ip地址

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //});

            BodyCan.AddDoNotFilterCanId(_diagnosisResponseCanId);
            byte[] startEcho;
            BodyCan.CanBusWithUds.TesterTryRequest(
                     _diagnosisRequestPhyCanId,
                     _diagnosisResponseCanId,
                     new byte[] { 0x31, 0x01, 0xAF, 0x14, 0x0f }, out startEcho,
                     CanBus.CanType.Standard, timeoutFromMilliseconds: 55 * 1000, pendingByte: 0xCC);

            //Thread.Sleep(55 * 1000);

            byte[] resultEcho;
            BodyCan.CanBusWithUds.TesterTryRequest(
               _diagnosisRequestPhyCanId,
               _diagnosisResponseCanId,
               new byte[] { 0x31, 0x03, 0xAF, 0x14 }, out resultEcho,
               CanBus.CanType.Standard, timeoutFromMilliseconds: 5 * 1000, pendingByte: 0xCC);

            EthernetCheckResult = string.Empty;
            if (resultEcho != null)
            {
                foreach (var t in resultEcho)
                    EthernetCheckResult += ValueHelper.GetHextStr(t);

                //EthernetCheckResult = "7103AF1400";
            }

            EnterDefaultSession();

            BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
        }

        public void ReadFeedbackMessage()
        {
            DlpCameraErrRsp = string.Empty;
            DlpLightingMasterErrRsp = string.Empty;

            var findCnaIdList = new List<uint> { 0x2e2 };

            foreach (var canId in findCnaIdList)
                BodyCan.AddDoNotFilterCanId(canId);

            BodyCan.CanRecvDataPackages.Clear();
            Thread.Sleep(1000);

            var find = BodyCan.CanRecvDataPackages.FindLast(f => f.CanId == findCnaIdList[0]);

            if (find != null && find.CanData != null && find.CanDataLen == 8)
            {
                var bitList = new List<string>();
                foreach (var t in find.CanData)
                {
                    var temp = Convert.ToString(t, 2).PadLeft(8, '0');

                    for (var i = temp.Length - 1; i >= 0; i--)
                        bitList.Add(temp[i].ToString());
                }

                DlpLightingMasterErrRsp = bitList[37];
                DlpCameraErrRsp = bitList[38];
            }

            foreach (var canId in findCnaIdList)
                BodyCan.RemoveDoNotFilterCanId(canId);
        }

        [Description("ReadHardwarePartNo")]
        public void ReadHardwarePartNo()
        {
            HardwarePartNo = string.Empty;

            if (BodyCan == null)
                return;

            var result =
                BodyCanReadDid(_diagnosisRequestPhyCanId, _diagnosisResponseCanId, 0xf1, 0x91);
            HardwarePartNo = Encoding.ASCII.GetString(result, 0, result.Length);
        }

        [Description("ReadHardwareVersion")]
        public void ReadHardwareVersion()
        {
            HardwareVersion = string.Empty;

            if (BodyCan == null)
                return;

            var result =
                BodyCanReadDid(_diagnosisRequestPhyCanId, _diagnosisResponseCanId, 0xf1, 0x93);
            foreach (var t in result)
                HardwareVersion += ValueHelper.GetHextStr(t);
        }

        [Description("ReadSoftwarePartNo")]
        public void ReadSoftwarePartNo()
        {
            SoftwarePartNo = string.Empty;

            if (BodyCan == null)
                return;

            var result =
                BodyCanReadDid(_diagnosisRequestPhyCanId, _diagnosisResponseCanId, 0xf1, 0x88);
            SoftwarePartNo = Encoding.ASCII.GetString(result, 0, result.Length);
        }

        [Description("ReadSoftwareVersion")]
        public void ReadSoftwareVersion()
        {
            SoftwareVersion = string.Empty;

            if (BodyCan == null)
                return;

            var result =
                BodyCanReadDid(_diagnosisRequestPhyCanId, _diagnosisResponseCanId, 0xf1, 0x95);
            foreach (var t in result)
                SoftwareVersion += ValueHelper.GetHextStr(t);
        }

        public void WriteProductionDate()
        {
            if (BodyCan == null)
                return;

            if (string.IsNullOrEmpty(Barcode))
                return;

            var barcode = GetBarcode(Barcode);

            if (string.IsNullOrEmpty(barcode))
                return;

            try
            {
                var year = barcode.Substring(9, 1);
                var day = barcode.Substring(10, 3);

                switch (year)
                {
                    case "W":
                        year = "2020";
                        break;

                    case "X":
                        year = "2021";
                        break;

                    case "Y":
                        year = "2022";
                        break;

                    case "Z":
                        year = "2023";
                        break;

                    case "A":
                        year = "2024";
                        break;

                    case "B":
                        year = "2025";
                        break;

                    case "C":
                        year = "2026";
                        break;

                    default:
                        return;
                }

                var dt = DateTime.Parse(string.Format("{0}/01/01", year));
                dt = dt.AddDays(int.Parse(day) - 1);

                var month = dt.Month.ToString().PadLeft(2, '0');
                var dayOfMonth = dt.Day.ToString().PadLeft(2, '0');

                var sendBytesStr = string.Format("{0}{1}{2}", year, month, dayOfMonth);
                var sendBytes = new List<byte>();

                for (var i = 0; i < sendBytesStr.Length; i = i + 2)
                {
                    var temp = sendBytesStr[i].ToString() + sendBytesStr[i + 1];
                    sendBytes.Add(Convert.ToByte(temp, 16));
                }

                BodyCan.AddDoNotFilterCanId(_diagnosisResponseCanId);
                //EnterExtendedSession();
                //SecurityAccess();

                if (!BodyCan.CanBusWithUds.TryWriteData(_diagnosisRequestPhyCanId, _diagnosisResponseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x8B, sendBytes, 0xCC))
                {
                    Thread.Sleep(500);
                    BodyCan.CanBusWithUds.TryWriteData(_diagnosisRequestPhyCanId, _diagnosisResponseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x8B, sendBytes, 0xCC);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            //EnterDefaultSession();
            BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
        }

        public void WriteSerialNo()
        {
            if (BodyCan == null)
                return;

            if (string.IsNullOrEmpty(Barcode))
                return;

            var barcode = GetBarcode(Barcode);

            if (string.IsNullOrEmpty(barcode))
                return;

            try
            {
                var year = barcode.Substring(9, 1);
                var day = barcode.Substring(10, 3);
                var searialNo = barcode.Substring(13, 4);

                switch (year)
                {
                    case "W":
                        year = "2020";
                        break;

                    case "X":
                        year = "2021";
                        break;

                    case "Y":
                        year = "2022";
                        break;

                    case "Z":
                        year = "2023";
                        break;

                    case "A":
                        year = "2024";
                        break;

                    case "B":
                        year = "2025";
                        break;

                    case "C":
                        year = "2026";
                        break;

                    default:
                        return;
                }

                var dt = DateTime.Parse(string.Format("{0}/01/01", year));
                dt = dt.AddDays(int.Parse(day) - 1);

                var month = dt.Month.ToString().PadLeft(2, '0');
                var dayOfMonth = dt.Day.ToString().PadLeft(2, '0');

                var sendBytes = Encoding.ASCII.GetBytes(string.Format("11{0}{1}{2}{3}1", year.Substring(2, 2), month, dayOfMonth, searialNo.PadLeft(9, '0')));

                BodyCan.AddDoNotFilterCanId(_diagnosisResponseCanId);
                //EnterExtendedSession();
                //SecurityAccess();

                if (!BodyCan.CanBusWithUds.TryWriteData(
                    _diagnosisRequestPhyCanId, _diagnosisResponseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x8C, sendBytes, 0xCC))
                {
                    Thread.Sleep(500);
                    BodyCan.CanBusWithUds.TryWriteData(
                        _diagnosisRequestPhyCanId, _diagnosisResponseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x8C, sendBytes, 0xCC);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            //EnterDefaultSession();
            BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
        }

        public void ReadProductionDate()
        {
            ProductionDate = string.Empty;

            if (BodyCan == null)
                return;

            var result =
                BodyCanReadDid(_diagnosisRequestPhyCanId, _diagnosisResponseCanId, 0xf1, 0x8B);
            var tempHex = string.Empty;
            if (result != null)
            {
                foreach (var t in result)
                    tempHex += ValueHelper.GetHextStr(t);
            }

            if (!string.IsNullOrEmpty(Barcode))
            {
                var barcode = GetBarcode(Barcode);

                if (!string.IsNullOrEmpty(barcode))
                {
                    var year = string.Empty;
                    var month = string.Empty;
                    var day = string.Empty;
                    var searialNo = string.Empty;

                    try
                    {
                        year = barcode.Substring(9, 1);
                        day = barcode.Substring(10, 3);
                        searialNo = barcode.Substring(13, 4);

                        switch (year)
                        {
                            case "W":
                                year = "2020";
                                break;

                            case "X":
                                year = "2021";
                                break;

                            case "Y":
                                year = "2022";
                                break;

                            case "Z":
                                year = "2023";
                                break;

                            case "A":
                                year = "2024";
                                break;

                            case "B":
                                year = "2025";
                                break;

                            case "C":
                                year = "2026";
                                break;

                            default:
                                return;
                        }

                        var dt = DateTime.Parse(string.Format("{0}/01/01", year));
                        dt = dt.AddDays(int.Parse(day) - 1);

                        month = dt.Month.ToString().PadLeft(2, '0');
                        var dayOfMonth = dt.Day.ToString().PadLeft(2, '0');

                        var sendBytesStr = string.Format("{0}{1}{2}", year, month, dayOfMonth);

                        if (sendBytesStr == tempHex)
                        {
                            ProductionDate = "OK " + tempHex;
                        }
                        else
                        {
                            ProductionDate = "NG " + tempHex;
                        }
                    }
                    catch (Exception ex)
                    {
                        ProductionDate = "NG " + ex.Message;
                    }
                }
            }
            //ProductionDate = Encoding.ASCII.GetString(result, 0, result.Length);
        }

        public void ReadSerialNo()
        {
            SerialNo = string.Empty;

            if (BodyCan == null)
                return;

            var result =
                BodyCanReadDid(_diagnosisRequestPhyCanId, _diagnosisResponseCanId, 0xf1, 0x8c);

            if (result != null)
            {
                var tempStr = Encoding.ASCII.GetString(result, 0, result.Length);

                if (!string.IsNullOrEmpty(Barcode))
                {
                    var barcode = GetBarcode(Barcode);

                    if (!string.IsNullOrEmpty(barcode))
                    {
                        var year = string.Empty;
                        var month = string.Empty;
                        var day = string.Empty;
                        var searialNo = string.Empty;

                        try
                        {
                            year = barcode.Substring(9, 1);
                            day = barcode.Substring(10, 3);
                            searialNo = barcode.Substring(13, 4);

                            switch (year)
                            {
                                case "W":
                                    year = "2020";
                                    break;

                                case "X":
                                    year = "2021";
                                    break;

                                case "Y":
                                    year = "2022";
                                    break;

                                case "Z":
                                    year = "2023";
                                    break;

                                case "A":
                                    year = "2024";
                                    break;

                                case "B":
                                    year = "2025";
                                    break;

                                case "C":
                                    year = "2026";
                                    break;

                                default:
                                    return;
                            }

                            var dt = DateTime.Parse(string.Format("{0}/01/01", year));
                            dt = dt.AddDays(int.Parse(day) - 1);

                            month = dt.Month.ToString().PadLeft(2, '0');
                            var dayOfMonth = dt.Day.ToString().PadLeft(2, '0');

                            var sendBytesStr = string.Format("11{0}{1}{2}{3}1", year.Substring(2, 2), month, dayOfMonth, searialNo.PadLeft(9, '0'));

                            if (tempStr == sendBytesStr)
                            {
                                SerialNo = "OK " + tempStr;
                            }
                            else
                            {
                                SerialNo = "NG " + tempStr;
                            }
                        }
                        catch (Exception ex)
                        {
                            SerialNo = "NG " + ex.Message;
                        }
                    }
                }
            }

            //SerialNo = Encoding.ASCII.GetString(result, 0, result.Length);
        }

        public void ClearBarcode()
        {
            Barcode = string.Empty;
        }

        private static string GetBarcode(string str)
        {
            var getBarcodeStr = string.Empty;

            try
            {
                var keyAndKeyindexAndLen = "LM:0:17";
                var sp = keyAndKeyindexAndLen.Split(':');
                var key = sp[0];
                var keyIndex = Convert.ToInt32(sp[1]);
                var len = Convert.ToInt32(sp[2]);

                var temp = str;
                //"000ABCP001EFG000";
                var findKeyIndex = temp.LastIndexOf(key, StringComparison.Ordinal);
                if (findKeyIndex == -1)
                    return getBarcodeStr;
                getBarcodeStr = temp.Substring(findKeyIndex - keyIndex, len);
            }
            catch (Exception exception)
            {
                // ignored
                getBarcodeStr = string.Empty;
            }

            return getBarcodeStr;
        }

        public void ReadSystemVoltage()
        {
            SystemVoltage = -9999.9;
            if (BodyCan == null)
                return;

            var result =
                BodyCanReadDid(_diagnosisRequestPhyCanId, _diagnosisResponseCanId, 0xcf, 0x00);

            SystemVoltage = ValueHelper.GetDecimal(result);
        }

        public void ReadSystemTemperature()
        {
            SystemTemperature = -9999.9;
            if (BodyCan == null)
                return;

            var result =
                BodyCanReadDid(_diagnosisRequestPhyCanId, _diagnosisResponseCanId, 0x10, 0x01);
            SystemTemperature = ValueHelper.GetDecimal(result);
        }

        public void ClearDiagnosticInfomation()
        {
            ClearDiagnosticInfomationResult = @"NG";

            if (BodyCan == null)
                return;

            BodyCan.AddDoNotFilterCanId(_diagnosisResponseCanId);

            if (BodyCan.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(_diagnosisRequestPhyCanId,
                _diagnosisResponseCanId, CanBus.CanType.Standard, pendingByte: 0xCC))
            {
                BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
                ClearDiagnosticInfomationResult = @"OK";
                return;
            }
            Thread.Sleep(500);

            if (BodyCan.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(_diagnosisRequestPhyCanId,
                _diagnosisResponseCanId, CanBus.CanType.Standard, pendingByte: 0xCC))
            {
                BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
                ClearDiagnosticInfomationResult = @"OK";
                return;
            }

            BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
        }

        public void ReadDiagnosticInfomation()
        {
            DiagnosticInfomation = string.Empty;

            if (BodyCan == null)
                return;

            DiagnosticInfomation = BodyCanIsHaveMsgCheckResult;

            //BodyCan.AddDoNotFilterCanId(_diagnosisResponseCanId);

            //byte[] echo;
            //if (BodyCan.CanBusWithUds.TryReadDtcInfomation(_diagnosisRequestPhyCanId,
            //    _diagnosisResponseCanId, CanBus.CanType.Standard, 0x02, 0x08,
            //    out echo))
            //{
            //    BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);

            //    if (echo == null || echo.Length == 0)
            //        return;
            //    DiagnosticInfomation = "OK";
            //    foreach (var t in echo)
            //    {
            //        DiagnosticInfomation += ValueOperater.GetHextStr(t);
            //    }                    
            //}
            //else
            //{
            //    Thread.Sleep(500);
            //    if (!BodyCan.CanBusWithUds.TryReadDtcInfomation(_diagnosisRequestPhyCanId,
            //        _diagnosisResponseCanId, CanBus.CanType.Standard, 0x02, 0x08,
            //        out echo))
            //    {
            //        BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
            //        return;
            //    }

            //    BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);

            //    if (echo == null || echo.Length == 0)
            //        return;
            //    DiagnosticInfomation = "OK";
            //    foreach (var t in echo)
            //    {
            //        DiagnosticInfomation += ValueOperater.GetHextStr(t);
            //    }
            //}
        }

        public void HsdOn(string delayMs)
        {
            FpdLinkErrorInfo = string.Empty;

            if (BodyCan == null)
                return;

            if (SubCan == null)
                return;

            BodyCan.AddDoNotFilterCanId(_diagnosisResponseCanId);

            EnterExtendedSession();
            SecurityAccess();

            BodyCan.CanBusWithUds.TryInputOutputControl(_diagnosisRequestPhyCanId, _diagnosisResponseCanId,
                CanBus.CanType.Standard, 0x10, 0x1B,
                Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x01 }, pendingByte: 0xCC);

            SubCan.AddDoNotFilterCanId(0x31);
            SubCan.AddDoNotFilterCanId(0x41);
            SubCan.CanRecvDataPackages.Clear();

            Thread.Sleep(int.Parse(delayMs));

            SubCan.RemoveDoNotFilterCanId(0x31);
            SubCan.RemoveDoNotFilterCanId(0x41);

            var find0X41 = SubCan.CanRecvDataPackages.FindAll(f => f.CanId == 0x41);
            var isFindError = false;
            foreach (var f in find0X41)
            {
                if (f.CanData != null && f.CanDataLen == 8)
                {
                    var bitList = new List<string>();
                    foreach (var temp in f.CanData.Select(t => Convert.ToString(t, 2).PadLeft(8, '0')))
                        for (var i = temp.Length - 1; i >= 0; i--)
                            bitList.Add(temp[i].ToString());
                    FpdLinkErrorInfo = bitList[25];

                    if (FpdLinkErrorInfo == "1")
                    {
                        isFindError = true;
                        break;
                    }
                }

                if (isFindError)
                    break;
            }

            //if (find0X41 != null && find0X41.RecvData != null && find0X41.RecvData.Length == 8)
            //{
            //    var bitList = new List<string>();
            //    foreach (var temp in find0X41.RecvData.Select(t => Convert.ToString(t, 2).PadLeft(8, '0')))
            //        for (var i = temp.Length - 1; i >= 0; i--)
            //            bitList.Add(temp[i].ToString());
            //    FpdLinkErrorInfo = bitList[25];
            //}

            //BodyCan.CanBusWithUds.TryInputOutputControl(_diagnosisRequestPhyCanId, _diagnosisResponseCanId,
            //    CanBus.CanType.Standard, 0x10, 0x1B,
            //    Uds14229Operater.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x00 });

            //EnterDefaultSession();

            //BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
        }

        public void EnterExtendedSession()
        {
            BodyCan.AddDoNotFilterCanId(_diagnosisResponseCanId);

            if (BodyCan.CanBusWithUds.TryEnterExtendedSession(
                _diagnosisRequestPhyCanId,
                _diagnosisResponseCanId,
                CanBus.CanType.Standard, pendingByte: 0xCC))
            {
                BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
                _isInExtendedSession = true;
                return;
            }

            Thread.Sleep(500);
            if (BodyCan.CanBusWithUds.TryEnterExtendedSession(
                _diagnosisRequestPhyCanId,
                _diagnosisResponseCanId,
                CanBus.CanType.Standard, pendingByte: 0xCC))
            {
                BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
                _isInExtendedSession = true;
                return;
            }
        }

        public void EnterDefaultSession()
        {
            BodyCan.AddDoNotFilterCanId(_diagnosisResponseCanId);
            _isInExtendedSession = false;

            if (BodyCan.CanBusWithUds.TryEnterDefaultSession(
                _diagnosisRequestPhyCanId,
                _diagnosisResponseCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xCC))
            {
                BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
                return;
            }

            Thread.Sleep(500);
            BodyCan.CanBusWithUds.TryEnterDefaultSession(
                _diagnosisRequestPhyCanId,
                _diagnosisResponseCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xCC);
            BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
        }

        public void SecurityAccess()
        {
            if (BodyCan == null)
                return;

            BodyCan.AddDoNotFilterCanId(_diagnosisResponseCanId);

            byte[] seedBytes;
            if (!BodyCan.CanBusWithUds.TryRequestSeed(
                _diagnosisRequestPhyCanId, _diagnosisResponseCanId,
                CanBus.CanType.Standard, 0x01, out seedBytes, pendingByte: 0xCC))
                return;

            if (seedBytes == null || seedBytes.Length != 4)
            {
                BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
                return;
            }

            const uint learnMask = 0x973F80E6;
            var securityKey = (uint)0x00;

            Array.Reverse(seedBytes);
            var securitySeed = BitConverter.ToUInt32(seedBytes, 0);

            if (securitySeed != 0)
            {
                for (var i = 0; i < 35; i++)
                {
                    if ((securitySeed & 0x80000000) != 0)
                    {
                        securitySeed = securitySeed << 1;
                        securitySeed = securitySeed ^ learnMask;
                    }
                    else
                    {
                        securitySeed = securitySeed << 1;
                    }
                }

                securityKey = securitySeed;
            }

            var keyBytes = BitConverter.GetBytes(securityKey);
            Array.Reverse(keyBytes);

            BodyCan.CanBusWithUds.TrySendKey(_diagnosisRequestPhyCanId, _diagnosisResponseCanId,
                CanBus.CanType.Standard, 0x02, keyBytes, pendingByte: 0xCC);

            BodyCan.RemoveDoNotFilterCanId(_diagnosisResponseCanId);
        }

        private byte[] BodyCanReadDid(uint requestCanId, uint responseCanId, byte didHi, byte didLo)
        {
            if (BodyCan == null)
                return new byte[0];

            BodyCan.AddDoNotFilterCanId(responseCanId);

            byte[] readBytes;
            if (BodyCan.CanBusWithUds.TryReadData(
                requestCanId, responseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi,
                didLo, out readBytes, 0xCC))
            {
                BodyCan.RemoveDoNotFilterCanId(responseCanId);
                return readBytes ?? new byte[0];
            }

            Thread.Sleep(500);

            if (!BodyCan.CanBusWithUds.TryReadData(
                requestCanId, responseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi,
                didLo, out readBytes, 0xCC))
            {
                BodyCan.RemoveDoNotFilterCanId(responseCanId);
                return new byte[0];
            }

            BodyCan.RemoveDoNotFilterCanId(responseCanId);
            return readBytes ?? new byte[0];
        }
    }
}
