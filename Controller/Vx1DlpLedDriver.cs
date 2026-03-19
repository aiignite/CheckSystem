using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility.FileOperator;

namespace Controller
{
    [Description("CAN-Product,VX1-DLP-LED-Driver")]
    public class Vx1DlpLedDriver : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public CanBus Can;

        /// <summary>
        /// 
        /// </summary>
        public string ProductData;

        /// <summary>
        /// 
        /// </summary>
        public string ProductSerialNumber;

        [Description("R,HardwareVersion")]
        public string HardwareVersion;

        [Description("R,PartNo")]
        public string PartNo;

        [Description("R,FblVersion")]
        public string FblVersion;

        [Description("R,AppVersion")]
        public string AppVersion;

        [Description("R,AppPartNo")]
        public string AppPartNo;

        /// <summary>
        /// 
        /// </summary>
        public string BarcodeBuff;

        private const uint RequestCanId = 0x78C;
        private const uint ResponseCanId = 0x784;
        private readonly Thread _mainThread;
        private bool _isSleep = true;
        private string _currentBarcode = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public Vx1DlpLedDriver(string name)
            : base(name)
        {
            CanBus.PushCanMsg += CanBus_PushCanMsg;

            if (_mainThread != null)
            {
                _mainThread.Abort();
                _mainThread.Join();
            }

            _mainThread = new Thread(Network) { IsBackground = true };
            _mainThread.Start();
        }

        private readonly byte[] _leftNetworkBytes = new byte[8];
        private readonly byte[] _rightNetworkBytes = new byte[8];
        private readonly object _canSendLocker = new object();

        private void Network()
        {
            while (_mainThread.IsAlive)
            {
                if (!_mainThread.IsAlive)
                    break;

                Thread.Sleep(40);

                if (_isSleep || Can == null)
                    continue;

                lock (_canSendLocker)
                {
                    var sendPackageList = new List<CanBus.CanDataPackage>
                    {
                        new CanBus.CanDataPackage(0x30, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data, _leftNetworkBytes),
                        new CanBus.CanDataPackage(0x40, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data, _rightNetworkBytes)
                    };

                    Can.SendCanDatas(sendPackageList.ToArray());
                }
            }
        }

        [Description("打开LED")]
        public void LedOn(string pwm)
        {
            var hex = Convert.ToByte(pwm);
            _leftNetworkBytes[1] = hex;
            _leftNetworkBytes[2] = hex;
            _rightNetworkBytes[1] = hex;
            _rightNetworkBytes[2] = hex;
            _isSleep = false;
        }

        [Description("关闭LED并停止周期帧")]
        public void LedOff()
        {
            _isSleep = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetBarcode(string keyAndKeyindexAndLen)
        {
            _currentBarcode = string.Empty;

            if (string.IsNullOrEmpty(BarcodeBuff))
                return;

            try
            {
                //const string key = "P00120613";
                //const int keyIndex = 7;
                //const int len = 45;
                var sp = keyAndKeyindexAndLen.Split(':');
                var key = sp[0];
                var keyIndex = Convert.ToInt32(sp[1]);
                var len = Convert.ToInt32(sp[2]);

                var temp = BarcodeBuff;
                //"000ABCP001EFG000";
                var findKeyIndex = temp.LastIndexOf(key, StringComparison.Ordinal);
                if (findKeyIndex == -1)
                    return;
                _currentBarcode = temp.Substring(findKeyIndex - keyIndex, len);
            }
            catch (Exception exception)
            {
                // ignored
                _currentBarcode = exception.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearBarcode()
        {
        }

        [Description("SecurityAccess")]
        public void SecurityAccess(string key)
        {
            if (Can == null)
                return;

            if (string.IsNullOrEmpty(key))
                key = "021122334401";

            var keyBytes = new List<byte>();

            for (var i = 0; i < key.Length; i = i + 2)
                keyBytes.Add(Convert.ToByte(string.Format(@"{0}{1}", key[i], key[i + 1]), 16));

            var sendBytes = new List<byte> { 0x27, 0x02 };
            sendBytes.AddRange(keyBytes);
            Can.SendStandardCanData(RequestCanId, sendBytes);
        }

        [Description("进入扩展模式")]
        public void EnterExtendedMode()
        {
            var sendBytes = new List<byte> { 0x10, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Can.SendStandardCanData(RequestCanId, sendBytes);
        }

        [Description("ECU重置")]
        public void EcuReset()
        {
            var sendBytes = new List<byte> { 0x11, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Can.SendStandardCanData(RequestCanId, sendBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        public void WriteHardwareVersion(string hardwareVersion)
        {
            var b = Convert.ToByte(hardwareVersion, 16);
            var sendBytes = new byte[] { 0x2E, 0xF1, 0x89, 0x48, 0x00, b, 0x00, 0x00 };

            if (Can != null)
                Can.SendStandardCanData(RequestCanId, sendBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        public void WriteProductData()
        {
            if (Can == null)
                return;

            if (string.IsNullOrEmpty(_currentBarcode))
                return;

            try
            {
                var year = _currentBarcode.Substring(31, 1);
                var day = _currentBarcode.Substring(32, 3);
                var serialNo = _currentBarcode.Substring(35, 4);

                switch (year)
                {
                    case "U":
                        year = "18";
                        break;

                    case "V":
                        year = "19";
                        break;

                    case "W":
                        year = "20";
                        break;

                    case "X":
                        year = "21";
                        break;

                    case "Y":
                        year = "22";
                        break;

                    case "Z":
                        year = "23";
                        break;

                    case "A":
                        year = "24";
                        break;

                    default:
                        return;
                }

                var bs = new List<byte> { 0x2E, 0xF1, 0x84 };
                bs.AddRange(Encoding.ASCII.GetBytes(string.Format(@"{0}{1}", year, day)));

                Can.SendStandardCanData(RequestCanId, bs);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void WriteProductSerialNumber()
        {
            if (Can == null)
                return;

            if (string.IsNullOrEmpty(_currentBarcode))
                return;

            try
            {
                var year = _currentBarcode.Substring(31, 1);
                var day = _currentBarcode.Substring(32, 3);
                var serialNo = _currentBarcode.Substring(35, 4);

                var bs = new List<byte> { 0x2E, 0xF1, 0x87 };
                for (var i = 0; i < serialNo.Length; i = i + 2)
                    bs.Add(Convert.ToByte(string.Format(@"{0}{1}", serialNo[i], serialNo[i + 1]), 16));
                bs.Add(0x00);
                bs.Add(0x00);
                bs.Add(0x00);

                Can.SendStandardCanData(RequestCanId, bs);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("读硬件版本号")]
        public void ReadHardwareVersion()
        {
            HardwareVersion = string.Empty;
            foreach (var t in CanReadDid(RequestCanId, ResponseCanId, 0xF1, 0x89))
                HardwareVersion += ValueHelper.GetHextStr(t);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReadProductData()
        {
            ProductData = string.Empty;

            if (Can == null)
                return;

            if (string.IsNullOrEmpty(_currentBarcode))
                return;

            try
            {
                var year = _currentBarcode.Substring(31, 1);
                var day = _currentBarcode.Substring(32, 3);
                var serialNo = _currentBarcode.Substring(35, 4);

                switch (year)
                {
                    case "U":
                        year = "18";
                        break;

                    case "V":
                        year = "19";
                        break;

                    case "W":
                        year = "20";
                        break;

                    case "X":
                        year = "21";
                        break;

                    case "Y":
                        year = "22";
                        break;

                    case "Z":
                        year = "23";
                        break;

                    case "A":
                        year = "24";
                        break;

                    default:
                        return;
                }

                var expectedBs = new List<byte> { 0x62, 0xF1, 0x84 };
                expectedBs.AddRange(Encoding.ASCII.GetBytes(string.Format(@"{0}{1}", year, day)));

                var expectedValue = expectedBs.Aggregate(string.Empty,
                    (current, t) => current + ValueHelper.GetHextStr(t));

                var actualValue = CanReadDid(RequestCanId, ResponseCanId, 0xF1, 0x84)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));

                ProductData = expectedValue == actualValue ? "OK " + actualValue : "NG " + actualValue;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReadProductSerialNumber()
        {
            ProductSerialNumber = string.Empty;

            if (Can == null)
                return;

            if (string.IsNullOrEmpty(_currentBarcode))
                return;

            try
            {
                var year = _currentBarcode.Substring(31, 1);
                var day = _currentBarcode.Substring(32, 3);
                var serialNo = _currentBarcode.Substring(35, 4);

                var expectedBs = new List<byte> { 0x62, 0xF1, 0x87 };
                for (var i = 0; i < serialNo.Length; i = i + 2)
                    expectedBs.Add(Convert.ToByte(string.Format(@"{0}{1}", serialNo[i], serialNo[i + 1]), 16));
                //expectedBs.Add(0x00);
                //expectedBs.Add(0x00);
                //expectedBs.Add(0x00);

                var expectedValue = expectedBs.Aggregate(string.Empty,
                    (current, t) => current + ValueHelper.GetHextStr(t));

                var actualValue = CanReadDid(RequestCanId, ResponseCanId, 0xF1, 0x87)
                    .Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));

                ProductSerialNumber = expectedValue == actualValue ? "OK " + actualValue : "NG " + actualValue;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("读总成零件号")]
        public void ReadPartNo()
        {
            PartNo = string.Empty;
            foreach (var t in CanReadDid(RequestCanId, ResponseCanId, 0xF1, 0x8A))
                PartNo += ValueHelper.GetHextStr(t);
        }

        [Description("读FBL版本号")]
        public void ReadFblVersion()
        {
            FblVersion = string.Empty;
            foreach (var t in CanReadDid(RequestCanId, ResponseCanId, 0xF1, 0x80))
                FblVersion += ValueHelper.GetHextStr(t);
        }

        [Description("读APP版本号")]
        public void ReadAppVersion()
        {
            AppVersion = string.Empty;
            foreach (var t in CanReadDid(RequestCanId, ResponseCanId, 0xF1, 0xA1))
                AppVersion += ValueHelper.GetHextStr(t);
        }

        [Description("读APP零件号")]
        public void ReadAppPartNo()
        {
            AppPartNo = string.Empty;
            foreach (var t in CanReadDid(RequestCanId, ResponseCanId, 0xF1, 0x8C))
                AppPartNo += ValueHelper.GetHextStr(t);
        }

        private IEnumerable<byte> CanReadDid(uint requestCanId, uint responseCanId, byte didHi, byte didLo)
        {
            if (Can == null)
                return new byte[0];

            Can.AddDoNotFilterCanId(responseCanId);
            Can.CanRecvDataPackages.Clear();

            lock (_canSendLocker)
            {
                var sendBytes = new byte[] { 0x22, didHi, didLo, 0x00, 0x00, 0x00, 0x00, 0x00 };
                Can.SendStandardCanData(requestCanId, sendBytes);
                Thread.Sleep(250);
                var findPackage =
                    Can.CanRecvDataPackages.Find(
                        f =>
                            f.CanId == responseCanId && f.CanData[0] ==
                            0x62 && f.CanData[1] == didHi && f.CanData[2] == didLo);
                if (findPackage != null)
                    return findPackage.CanData;

                Thread.Sleep(250);
                Can.CanRecvDataPackages.Clear();
                Can.SendStandardCanData(requestCanId, sendBytes);
                Thread.Sleep(250);
                findPackage =
                    Can.CanRecvDataPackages.Find(
                        f =>
                            f.CanId == responseCanId && f.CanDataLen == 8 && f.CanData[0] ==
                            0x62 && f.CanData[1] == didHi && f.CanData[2] == didLo);

                return findPackage != null ? findPackage.CanData : new byte[0];
            }
        }

        #region 程序下载相关

        [Description("R,APP下载结果")]
        public string DownloadResult = string.Empty;

        [Description("R,APP下载时间")]
        public string DownloadTimeS = string.Empty;

        [Description("R/W,APP文件路径")]
        public string AppFilePath =
            @"G:\Proj-2021\VX1系列\VX1模块系列\2.VX1高配前组合灯ADLPLED驱动模块 （新产品）\返工\DLP模块样品组返工程序S012(A013 B003)\DLP_LED_Drviver_A013_20220623.s19";

        private static readonly object FileLocker = new object();

        [Description("APP下载")]
        public void AppDownload()
        {
            DownloadResult = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - 下载中"; //string.Empty;
            DownloadTimeS = string.Empty;

            if (Can == null)
            {
                DownloadResult = "NG CAN未初始化";
                return;
            }

            var downloadAction = new Action(() =>
            {
                List<List<SRecordFileHelper.SRecordLineData>> blocks;

                lock (FileLocker)
                {
                    if (string.IsNullOrEmpty(AppFilePath) || !File.Exists(AppFilePath) ||
                        !AppFilePath.ToLower().EndsWith(".s19"))
                    {
                        DownloadResult = "NG APP文件不存在";
                        return;
                    }

                    var sRecord = SRecordFileHelper.GetSRecordLineData(AppFilePath);
                    blocks = SRecordFileHelper.GetBlocks(sRecord); // Block集合
                }

                if (blocks == null)
                {
                    DownloadResult = "NG APP文件错误";
                    return;
                }

                for (var i = 0; i < 20; i++)
                {
                    Can.SendStandardCanData(RequestCanId, new byte[8]);
                }

                if (!CanRequest(new List<byte> { 0x27, 0x02, 0x02, 0x11, 0x22, 0x33, 0x44, 0x01 }, "670202"))
                {
                    DownloadResult = "NG 2702";
                    return;
                }

                if (!CanRequest(new List<byte> { 0x10, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, "500200"))
                {
                    DownloadResult = "NG 2702";
                    return;
                }

                var checkSum = 0;
                var checkSumList = new List<byte>();
                var dataRecord = new List<byte>();
                foreach (var block in blocks)
                {
                    var flashTotalBytes = new List<byte>();
                    var dataLen = 0;
                    var startAddr = block[0].Address;

                    foreach (var t in block)
                    {
                        flashTotalBytes.AddRange(t.Data);
                        dataRecord.AddRange(t.Data);
                        dataLen += t.DataLen;
                        checkSum = t.Data.Aggregate(checkSum, (current, d) => current + d);
                        checkSumList.AddRange(t.Data);
                    }

                    var addrBytes = BitConverter.GetBytes(startAddr).Reverse().ToArray();
                    var lenBytes = BitConverter.GetBytes(dataLen).Reverse().ToArray();
                    if (!CanRequest(
                        new List<byte>
                        {
                            0x34,
                            addrBytes[0],
                            addrBytes[1],
                            addrBytes[2],
                            addrBytes[3],
                            lenBytes[2],
                            lenBytes[3],
                            0x00
                        }, "740400"))
                    {
                        DownloadResult = "NG 34请求";
                        return;
                    }


                    var baseDataIndex = 0;
                    var restBytes = new List<byte>();
                    if (flashTotalBytes.Count % 1024 != 0)
                    {
                        for (
                            var i = flashTotalBytes.Count / 1024 * 1024;
                            i < flashTotalBytes.Count;
                            i++)
                        {
                            restBytes.Add(flashTotalBytes[i]);
                        }
                    }

                    var count76 = 1;
                    for (var i = 0; i < flashTotalBytes.Count / 1024; i++)
                    {
                        for (var j = 1; j < 171; j++)
                        {
                            if (!CanRequest(
                                new List<byte>
                                {
                                    0x36,
                                    (byte) j,
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
                                }, "76" + ValueHelper.GetHextStr((byte)j)))
                            {
                                DownloadResult = "NG 36请求";
                                return;
                            }

                            baseDataIndex = baseDataIndex + 6;
                        }

                        if (!CanRequest(
                            new List<byte>
                            {
                                0x36,
                                0xAB,
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
                                0xFF,
                                0xFF
                            }, "76" + ValueHelper.GetHextStr((byte)count76)))
                        {
                            DownloadResult = "NG 36请求";
                            return;
                        }
                        count76++;
                        baseDataIndex = baseDataIndex + 4;
                    }

                    if (restBytes.Any())
                    {
                        for (var i = 0; i < restBytes.Count % 6; i++)
                            restBytes.Add(0xFF);

                        var baseCount = 1;
                        for (var i = 0; i < restBytes.Count; i = i + 6)
                        {
                            if (i != restBytes.Count - 6)
                            {
                                if (!CanRequest(
                                    new List<byte>
                                    {
                                        0x36,
                                        (byte) baseCount,
                                        restBytes[i + 0],
                                        restBytes[i + 1],
                                        restBytes[i + 2],
                                        restBytes[i + 3],
                                        restBytes[i + 4],
                                        restBytes[i + 5],
                                    }, "76" + ValueHelper.GetHextStr((byte)baseCount)))
                                {
                                    DownloadResult = "NG 36请求";
                                    return;
                                }
                                baseCount++;
                            }
                            else
                            {
                                if (!CanRequest(
                                    new List<byte>
                                    {
                                        0x36,
                                        (byte) baseCount,
                                        restBytes[i + 0],
                                        restBytes[i + 1],
                                        restBytes[i + 2],
                                        restBytes[i + 3],
                                        restBytes[i + 4],
                                        restBytes[i + 5],
                                    }, "76" + ValueHelper.GetHextStr((byte)count76)))
                                {
                                    DownloadResult = "NG 36请求";
                                    return;
                                }
                            }
                        }
                    }

                    if (!CanRequest(new byte[] { 0x37, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, "77"))
                    {
                        DownloadResult = "NG 37请求";
                        return;
                    }
                }

                var crc = BitConverter.GetBytes((ushort)FuncCalCrc16(checkSumList.ToArray())).Reverse().ToArray();
                if (!CanRequest(new byte[] { 0x31, 0x02, 0x02, crc[0], crc[1], 0x00, 0x00, 0x00 }, "710202"))
                {
                    DownloadResult = "NG 31请求";
                    return;
                }

                EcuReset();
                DownloadResult = "OK";
            });

            var st = new Stopwatch();
            st.Start();
            downloadAction.Invoke();
            st.Stop();
            DownloadTimeS = st.ElapsedMilliseconds / 1000 + "s";
        }

        private bool CanRequest(
            IEnumerable<byte> datBytes, string recvStartValue, int timeOut = 500)
        {
            _isWatiRecv = true;
            _waitRecvStartValue = recvStartValue;

            //var sendAction = new Task(() =>
            //{

            //});

            //sendAction.Start();
            Can.SendStandardCanData(RequestCanId, datBytes);
            var isSuccess = _waitHandle.WaitOne(timeOut);
            //Thread.Sleep(5);

            return isSuccess;
        }

        private uint FuncCalCrc16(byte[] bytes)
        {
            uint crcIdx;

            uint crcVal = 0x0000;
            for (crcIdx = 0; crcIdx < bytes.Length; crcIdx++)
            {
                var tmpData = bytes[crcIdx];
                crcVal = _crcTable[(tmpData ^ crcVal) & 0x0F] ^ (crcVal >> 4);
                crcVal = _crcTable[((tmpData >> 4) ^ crcVal) & 0x0F] ^ (crcVal >> 4);
            }
            return crcVal;
        }

        private readonly uint[] _crcTable =
        {
            0x0000, 0xCC01, 0xD801, 0x1400, 0xF001, 0x3C00, 0x2800, 0xE401,
            0xA001, 0x6C00, 0x7800, 0xB401, 0x5000, 0x9C01, 0x8801, 0x4400
        };

        private bool _isWatiRecv;
        private string _waitRecvStartValue;
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);

        private void CanBus_PushCanMsg(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (Can == null)
                return;

            if (Can.Name != name)
                return;

            if (!_isWatiRecv || data.CanId != ResponseCanId)
                return;

            if (string.IsNullOrEmpty(_waitRecvStartValue))
            {
                _isWatiRecv = false;
            }
            else
            {
                var dataStr = ValueHelper.GetHextStr(data.CanData).Replace(" ", "");
                if (!dataStr.StartsWith(_waitRecvStartValue))
                    return;
                _isWatiRecv = false;
                _waitRecvStartValue = string.Empty;
                _waitHandle.Set();
            }
        }

        #endregion
    }
}
