using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;

namespace Controller
{
    [Description("LIN-Product,ALCM音乐氛围灯")]
    public sealed class AlcmAtmosphereLampModule : ControllerBase
    {
        public LinBus Lin19200;

        public LinBus LedLin;

        [Description("R/W,模式控制-音乐模式=5-非音乐模式=1")]
        public int AmbtLgtZn1AnmIdxCmd;
        [Description("R/W,颜色控制-颜色控制0~121")]
        public int AmbtLgtZn1ColrIdxCmd;
        [Description("R/W,亮度控制-亮度调节0~100")]
        public int AmmbtLgtZn1ColrIntstyCmd;

        public AlcmAtmosphereLampModule(string name)
            : base(name)
        {
            LinBus.PushLinMsg += LinBus_PushLinMsg;
            _mainWorkThread = new Thread(MainWork);
            _mainWorkThread.Start();
        }

        ~AlcmAtmosphereLampModule()
        {
            Dispose();
        }

        private void LinBus_PushLinMsg(string name, LinBus.LinDataPackage data)
        {
            if (LedLin != null && LedLin.Name == name)
            {
                if (_isCheckLedLin)
                {
                    if (data != null && data.LinData != null)
                    {
                        _ledLinMsgCount++;
                    }
                }

                if (_isCheckAmbtLgtSoftwareVersionCmd)
                {
                    if (data != null && data.LinData != null && data.LinId == LinBus.ConvertLinId(0x3A) && data.LinDataLen >= 4)
                    {
                        var cmd = ValueHelper.GetHextStrWithOx(data.LinData[4]);
                        if (cmd.ToLower() == "0xCC".ToLower())
                        {
                            if (_get0xCCCount <= 5)
                            {
                                _get0xCCCount++;
                            }
                            else
                            {
                                AmbtLgtSoftwareVersionCmd = ValueHelper.GetHextStrWithOx(data.LinData[4]);
                            }
                        }
                        else
                        {
                            AmbtLgtSoftwareVersionCmd = ValueHelper.GetHextStrWithOx(data.LinData[4]);
                        }


                        //Console.WriteLine("ALCM={0},0x3A_Data={1}", Name, ValueHelper.GetHextStr(data.LinData));
                    }
                }
            }

            if (Lin19200 != null && Lin19200.Name == name)
            {
                if (_isCheckBcmLin)
                {
                    if (data != null &&
                        data.LinData != null &&
                        data.LinId == LinBus.ConvertLinId(0x0C) &&
                        data.LinDataLen > 0)
                    {
                        _bcmLinMsgCount++;
                    }
                }
            }
        }

        private bool _isCheckBcmLin;
        private int _bcmLinMsgCount;
        public string CheckBcmLinMsgResult = string.Empty;

        public async void CheckBcmLinMsg()
        {
            CheckBcmLinMsgResult = string.Empty;

            if (Lin19200 == null)
                return;

            _bcmLinMsgCount = 0;
            _isCheckBcmLin = true;
            await Task.Delay(500);
            _isCheckBcmLin = false;

            CheckBcmLinMsgResult = _bcmLinMsgCount > 0 ? "OK" : "NG";
        }

        private bool _isCheckAmbtLgtSoftwareVersionCmd;
        public string AmbtLgtSoftwareVersionCmd = string.Empty;
        private int _get0xCCCount = 0;

        public async void CheckAmbtLgtSoftwareVersionCmd()
        {
            AmbtLgtSoftwareVersionCmd = string.Empty;
            _get0xCCCount = 0;

            if (Lin19200 == null)
                return;

            _isCheckAmbtLgtSoftwareVersionCmd = true;
            await Task.Delay(450);
            _isCheckAmbtLgtSoftwareVersionCmd = false;
            _get0xCCCount = 0;
        }

        private bool _isCheckLedLin;
        private int _ledLinMsgCount;
        public string CheckLedLinMsgResult = string.Empty;

        public void CheckLedLinMsg()
        {
            CheckLedLinMsgResult = string.Empty;

            if (LedLin == null)
                return;

            _ledLinMsgCount = 0;
            _isCheckLedLin = true;
            Thread.Sleep(500);
            _isCheckLedLin = false;

            CheckLedLinMsgResult = _ledLinMsgCount > 0 ? "OK" : "NG";

            _ledLinMsgCount = 0;
        }

        #region 功能控制

        private readonly object _lockSend = new object();
        private readonly Thread _mainWorkThread;
        private bool _isAwake;

        private readonly LinCommunicationMatrix.IntelMatrix _bcmLin15ControlBroadcast1CmdMsg =
            new LinCommunicationMatrix.IntelMatrix(0x39, 8);
        private readonly LinCommunicationMatrix.IntelMatrix _bcmLin15ControlBroadcast2CmdMsg =
            new LinCommunicationMatrix.IntelMatrix(0x3A, 8);

        [Description("模块唤醒")]
        public void ModuleAwake()
        {
            _isAwake = true;
        }

        [Description("模块休眠")]
        public void ModuleSleep()
        {
            _isAwake = false;
        }

        private void MainWork()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(25);

                if (Lin19200 == null)
                    continue;

                if (!_isAwake)
                    continue;

                lock (_lockSend)
                {
                    if (AmbtLgtZn1AnmIdxCmd >= 0 && AmbtLgtZn1AnmIdxCmd <= 31)
                        _bcmLin15ControlBroadcast1CmdMsg.UpdateData(new MatrixValDefinition(29, 5, (byte)AmbtLgtZn1AnmIdxCmd));
                    if (AmbtLgtZn1ColrIdxCmd >= 0 && AmbtLgtZn1ColrIdxCmd <= 121)
                        _bcmLin15ControlBroadcast1CmdMsg.UpdateData(new MatrixValDefinition(1, 7, (byte)AmbtLgtZn1ColrIdxCmd));
                    if (AmmbtLgtZn1ColrIntstyCmd >= 0 && AmmbtLgtZn1ColrIntstyCmd <= 100)
                        _bcmLin15ControlBroadcast2CmdMsg.UpdateData(new MatrixValDefinition(0, 8, (byte)AmmbtLgtZn1ColrIntstyCmd));

                    Lin19200.SendMasterLin(_bcmLin15ControlBroadcast1CmdMsg.MasterLinId,
                        _bcmLin15ControlBroadcast1CmdMsg.MatrixData);
                    Lin19200.SendMasterLin(_bcmLin15ControlBroadcast2CmdMsg.MasterLinId,
                        _bcmLin15ControlBroadcast2CmdMsg.MatrixData);

                    byte[] echo;
                    Lin19200.SendSlaveLin(0x0C, out echo, 50);
                }
            }
        }

        #endregion

        #region 版本信息

        [Description("R,App软件信息")]
        public string AppVersion = string.Empty;
        [Description("R,CAL内部版本号")]
        public string CalInternalVersion = string.Empty;
        [Description("R,零件号")]
        public string PartNo = string.Empty;
        [Description("R,内部版本号")]
        public string InternalVersion = string.Empty;

        [Description("R,版本号F1F1")]
        public string VerF1F1 = string.Empty;
        [Description("R,标定F1F7")]
        public string CalF1F7 = string.Empty;
        [Description("R,引导程序软件内部版本号FC03")]
        public string InternalFblVerFc03 = string.Empty;

        [Description("读App软件信息")]
        public void ReadAppVersion()
        {
            AppVersion = string.Empty;
            AppVersion = ReadDid(2, 0xF1, 0xF1, 0x66, "ASCII");
        }

        [Description("读cal内部版本号")]
        public void ReadCalInternalVersion()
        {
            CalInternalVersion = string.Empty;
            CalInternalVersion = ReadDid(4, 0xF2, 0xF1, 0x66, "ASCII");
        }

        [Description("读零件号")]
        public void ReadPartNo()
        {
            PartNo = string.Empty;
            PartNo = ReadDid(4, 0xF1, 0xF9, 0x66, "Dec");
        }

        [Description("读内部版本号")]
        public void ReadInternalVersion()
        {
            InternalVersion = string.Empty;
            InternalVersion = ReadDid(7, 0xFC, 0x06, 0x66, "ASCII");
        }

        [Description("读所有版本信息")]
        public void ReadAllVersionInfo()
        {
            ReadAppVersion();
            ReadPartNo();
            ReadInternalVersion();
            ReadCalInternalVersion();

            VerF1F1 = string.Empty;
            CalF1F7 = string.Empty;
            InternalFblVerFc03 = string.Empty;

            VerF1F1 = ReadDid(4, 0xF1, 0xF1, 0x66, "ASCII");
            CalF1F7 = ReadDid(4, 0xF1, 0xF7, 0x66, "Hex");
            InternalFblVerFc03 = ReadDid(7, 0xFC, 0x03, 0x66, "ASCII");
        }

        private string ReadDid(int dataLen, byte didHi, byte didLo, byte nad, string dataType)
        {
            if (Lin19200 == null)
            {
                return string.Empty;
            }

            var readValueDataLen = dataLen;
            var data0 = nad;

            lock (_lockSend)
            {
                try
                {
                    var sendBytes = new byte[] { data0, 0x03, 0x22, didHi, didLo, 0xFF, 0xFF, 0xFF };
                    Lin19200.SendMasterLin(0x3C, sendBytes);

                    var resultBs = new List<byte>();

                    Thread.Sleep(100);
                    byte[] recv;
                    var isReadFirstFrameSucceed = Lin19200.SendSlaveLin(0x3D, out recv);
                    if (isReadFirstFrameSucceed && recv != null && recv.Length == 8)
                    {
                        if (recv[0] == data0)
                        {
                            if (recv[1] >= 0x10) // 多帧
                            {
                                if (recv[3] == 0x62 && recv[4] == didHi && recv[5] == didLo)
                                {
                                    resultBs.Add(recv[6]);
                                    resultBs.Add(recv[7]);
                                    var len = (recv[1] - 0x10) * 256 + recv[2];

                                    int count;
                                    if ((len - 5) % 6 == 0)
                                        count = (len - 5) / 6;
                                    else
                                        count = (len - 5) / 6 + 1;

                                    for (var i = 0; i < count; i++)
                                    {
                                        byte[] recvBytesRest;
                                        var isSucceed = Lin19200.SendSlaveLin(0x3D, out recvBytesRest);
                                        if (isSucceed && recvBytesRest != null && recvBytesRest.Length == 8)
                                        {
                                            for (var j = 2; j < 8; j++)
                                                resultBs.Add(recvBytesRest[j]);
                                        }
                                    }
                                }
                            }
                            else // 单帧
                            {
                                if (recv[2] == 0x62 && recv[3] == didHi && recv[4] == didLo)
                                {
                                    for (var i = 5; i < 5 + dataLen; i++)
                                    {
                                        resultBs.Add(recv[i]);
                                    }
                                }
                            }
                        }

                        if (resultBs.Any() && resultBs.Count >= readValueDataLen)
                        {
                            var temp3333 = new byte[readValueDataLen];
                            Array.Copy(resultBs.ToArray(), 0, temp3333, 0, readValueDataLen);
                            resultBs.Clear();
                            resultBs.AddRange(temp3333);
                        }

                        var getStr = string.Empty;
                        if (string.Equals(dataType, "ASCII", StringComparison.CurrentCultureIgnoreCase))
                            getStr = Encoding.ASCII.GetString(resultBs.ToArray());
                        else if (string.Equals(dataType, "Hex", StringComparison.CurrentCultureIgnoreCase))
                            getStr = resultBs.Aggregate(getStr, (current, t) => current + ValueHelper.GetHextStr(t));
                        else if (string.Equals(dataType, "Dec", StringComparison.CurrentCultureIgnoreCase))
                            getStr = ValueHelper.GetDecimal(resultBs.ToArray()).ToString();
                        return getStr;
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

                return string.Empty;
            }
        }

        #endregion

        #region LIN刷写

        [Description("R,App下载结果")]
        public string AppDownloadResult = string.Empty;
        [Description("R,APP下载耗时-秒")]
        public float AppDownloadCostTime = -9999;

        [Description("R,Cal下载结果")]
        public string CalDownloadResult = string.Empty;
        [Description("R,APP下载耗时-秒")]
        public float CalDownloadCostTime = -9999;

        [Description("R/W,FlashDrv文件路径")]
        public string FlashDriverFilePath = @"E:\Projects-2022\音乐氛围灯\C1YB_ALCM_APP_V1.0.02_FBL_V1.0.00_CAL_V0.0.02_20230816_Release\执行文件\DRV\FLASHDRV.mot";
        [Description("R/W,APP文件路径")]
        public string AppFielPath = @"E:\Projects-2022\音乐氛围灯\D2UC_ALCM_APP_V1.0.07_FBL_V1.0.00_CAL_V0.0.04_20231215_Release\执行文件\APP\D2UC_C1YB_ALCM_APP_V1.0.07_20231212_Release.mot";
        [Description("R/W,CAL文件路径")]
        public string CalibrationFilePath = @"E:\Projects-2022\音乐氛围灯\D2UC_ALCM_APP_V1.0.07_FBL_V1.0.00_CAL_V0.0.04_20231215_Release\执行文件\APP\D2UC_ALCM_CAL_V0.0.04_20231212_Release.bin";

        private byte _nad = 0x66;
        private byte _diagnosisMasterLinId = 0x3C;
        private byte _diagnosisSlaveLinId = 0x3D;
        private static readonly object FileLocker = new object();

        [Description("刷新APP文件")]
        public void AppDownload()
        {
            AppDownloadResult = string.Empty;
            AppDownloadCostTime = -9999;
            var st = new Stopwatch();
            st.Start();
            DataDownload(AppFielPath, ref AppDownloadResult, false);
            GetLinRecvMsg(new byte[] { _nad, 0x02, 0x11, 0x01, 0xFF, 0xFF, 0xFF, 0xFF }, _diagnosisMasterLinId,
               _diagnosisSlaveLinId);
            st.Stop();
            AppDownloadCostTime = st.ElapsedMilliseconds / 1000f;
        }

        [Description("刷新Cal文件")]
        public void CaiDownload()
        {
            CalDownloadResult = string.Empty;
            CalDownloadCostTime = -9999;
            var st = new Stopwatch();
            st.Start();
            DataDownload(CalibrationFilePath, ref CalDownloadResult, true);
            GetLinRecvMsg(new byte[] { _nad, 0x02, 0x11, 0x01, 0xFF, 0xFF, 0xFF, 0xFF }, _diagnosisMasterLinId,
               _diagnosisSlaveLinId);
            st.Stop();
            CalDownloadCostTime = st.ElapsedMilliseconds / 1000f;
        }

        private void DataDownload(string filePath, ref string resultMsg, bool isCal)
        {
            lock (FileLocker)
            {
                if (string.IsNullOrEmpty(FlashDriverFilePath))
                {
                    resultMsg = "NG FlashDrv文件不存在";
                    return;
                }
            }

            if (!File.Exists(filePath))
            {
                resultMsg = "NG 待刷新文件不存在";
                return;
            }

            List<List<SRecordFileHelper.SRecordLineData>> flashDrvBlocks;
            List<List<SRecordFileHelper.SRecordLineData>> dataBlocks;
            lock (FileLocker)
            {
                var sRecordDrv = SRecordFileHelper.GetSRecordLineData(FlashDriverFilePath);
                flashDrvBlocks = SRecordFileHelper.GetBlocks(sRecordDrv); // Block集合

                var sRecordApp = SRecordFileHelper.GetSRecordLineData(filePath);
                dataBlocks = SRecordFileHelper.GetBlocks(sRecordApp); // Block集合
            }

            if (Lin19200 == null)
            {
                resultMsg = "NG LIN未初始化";
                return;
            }

            try
            {
                var entendedModeReq = GetLinRecvMsg(
                new byte[] { _nad, 0x02, 0x10, 0x03, 0xff, 0xff, 0xff, 0xff }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
                if (!entendedModeReq.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x50, 0x03 }).Replace(" ", "")))
                {
                    //resultMsg = "NG 10 03失败：" + entendedModeReq;
                    //return;
                }

                var seedReq1 = GetLinRecvMsg(new byte[] { _nad, 0x02, 0x27, 0x01, 0xff, 0xff, 0xff, 0xff }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
                if (!seedReq1.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x06, 0x67, 0x01 }).Replace(" ", "")) || seedReq1.Length != 8 * 2)
                {
                    //resultMsg = "NG 27 01失败：" + seedReq1;
                    //return;
                }

                var seed11 = Convert.ToByte(seedReq1.Substring(8, 2), 16);
                var seed12 = Convert.ToByte(seedReq1.Substring(10, 2), 16);
                var seed13 = Convert.ToByte(seedReq1.Substring(12, 2), 16);
                var seed14 = Convert.ToByte(seedReq1.Substring(14, 2), 16);

                var keys1 = GenerateKeys(GenerateSeed(new[] { seed11, seed12, seed13, seed14 }));
                var keyReq1 = GetLinRecvMsg(
                    new byte[] { _nad, 0x06, 0x27, 0x02, keys1[0], keys1[1], keys1[2], keys1[3] },
                    _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
                if (!keyReq1.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x67, 0x02 }).Replace(" ", "")))
                {
                    //resultMsg = "NG 27 02失败：" + keyReq1;
                    //return;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            var programModeReq = GetLinRecvMsg(
                new byte[] { _nad, 0x02, 0x10, 0x02, 0xff, 0xff, 0xff, 0xff }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!programModeReq.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x50, 0x02 }).Replace(" ", "")))
            {
                resultMsg = "NG 10 02失败：" + programModeReq;
                return;
            }

            var seedReq2 = GetLinRecvMsg(new byte[] { _nad, 0x02, 0x27, 0x01, 0xff, 0xff, 0xff, 0xff }, _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!seedReq2.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x06, 0x67, 0x01 }).Replace(" ", "")) || seedReq2.Length != 8 * 2)
            {
                resultMsg = "NG 27 01失败：" + seedReq2;
                return;
            }

            var seed21 = Convert.ToByte(seedReq2.Substring(8, 2), 16);
            var seed22 = Convert.ToByte(seedReq2.Substring(10, 2), 16);
            var seed23 = Convert.ToByte(seedReq2.Substring(12, 2), 16);
            var seed24 = Convert.ToByte(seedReq2.Substring(14, 2), 16);

            var keys2 = GenerateKeys(GenerateSeed(new[] { seed21, seed22, seed23, seed24 }));

            var keyReq2 = GetLinRecvMsg(
                new byte[] { _nad, 0x06, 0x27, 0x02, keys2[0], keys2[1], keys2[2], keys2[3] },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);

            if (!keyReq2.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x67, 0x02 }).Replace(" ", "")))
            {
                resultMsg = "NG 27 02失败：" + keyReq2;
                return;
            }

            var flashTotalBytes = new List<byte>();
            var flashDrvStartAddr = flashDrvBlocks[0][0].Address;
            var flashDrvDataLen = 0;
            foreach (var t in flashDrvBlocks[0])
            {
                flashTotalBytes.AddRange(t.Data);
                flashDrvDataLen += t.DataLen;
            }

            var dataRecord = new List<byte>();
            var dataStartAddr = !isCal ? (uint)0x08020000 : 0x08050000;
            var dataLen = 0;
            foreach (var t in dataBlocks[0])
            {
                dataRecord.AddRange(t.Data);
                dataLen += t.DataLen;
            }

            Lin19200.SendMasterLin(_diagnosisMasterLinId,
                new byte[]
                {
                    _nad, 0x10, 0x0b, 0x34, 0x01, 0x44,
                    BitConverter.GetBytes(flashDrvStartAddr).Reverse().ToArray()[0],
                    BitConverter.GetBytes(flashDrvStartAddr).Reverse().ToArray()[1]
                });
            Thread.Sleep(25);
            var flashDrv34Req = GetLinRecvMsg(
                new byte[]
                {
                    _nad,
                    0x21,
                    BitConverter.GetBytes(flashDrvStartAddr).Reverse().ToArray()[2],
                    BitConverter.GetBytes(flashDrvStartAddr).Reverse().ToArray()[3],
                    BitConverter.GetBytes(flashDrvDataLen).Reverse().ToArray()[0],
                    BitConverter.GetBytes(flashDrvDataLen).Reverse().ToArray()[1],
                    BitConverter.GetBytes(flashDrvDataLen).Reverse().ToArray()[2],
                    BitConverter.GetBytes(flashDrvDataLen).Reverse().ToArray()[3]
                }, _diagnosisMasterLinId,
                _diagnosisSlaveLinId,
                isNeedRetry: false);

            if (!flashDrv34Req.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x74 }).Replace(" ", "")))
            {
                resultMsg = "NG FlashDrv 34失败：" + flashDrv34Req;
                return;
            }

            var maxNumberOfBlockLength1 = Convert.ToByte(flashDrv34Req.Substring(8, 2), 16) * 256 +
                                          Convert.ToByte(flashDrv34Req.Substring(10, 2), 16);

            if (!DataTransfer(flashTotalBytes, ref resultMsg))
                return;

            var exitDataTransfer1 = GetLinRecvMsg(new byte[] { _nad, 0x01, 0x37, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, 200, false);
            if (!exitDataTransfer1.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x01, 0x77 }).Replace(" ", "")))
            {
                resultMsg = "NG FlashDrv 37失败：" + exitDataTransfer1;
                return;
            }

            Lin19200.SendMasterLin(_diagnosisMasterLinId,
                new byte[]
                {
                    _nad, 0x10, 0x0d, 0x31, 0x01, 0xFF, 0x00, 0x44
                });
            Thread.Sleep(25);
            Lin19200.SendMasterLin(_diagnosisMasterLinId,
                new byte[]
                {
                    _nad, 0x21,
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[0],
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[1],
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[2],
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[3],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[0],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[1]
                });
            Thread.Sleep(25);
            Lin19200.SendMasterLin(_diagnosisMasterLinId, new byte[]
            {
                _nad, 0x22,
                BitConverter.GetBytes(dataLen).Reverse().ToArray()[2],
                BitConverter.GetBytes(dataLen).Reverse().ToArray()[3],
                0xFF, 0xFF, 0xFF, 0xFF
            });

            var eraseReq1 = string.Empty;
            var errorCount = 0;
            while (true)
            {
                byte[] echoBytes;
                if (!Lin19200.SendSlaveLin(_diagnosisSlaveLinId, out echoBytes, 50))
                {
                    errorCount++;
                    if (errorCount > 10)
                        break;
                }
                if (ValueHelper.GetHextStr(echoBytes)
                        .Replace(" ", "")
                        .StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x03, 0x7F, 0x31, 0x78 }).Replace(" ", "")))
                {
                    Thread.Sleep(50);
                    continue;
                }

                if (!string.IsNullOrEmpty(ValueHelper.GetHextStr(echoBytes)))
                {
                    eraseReq1 = ValueHelper.GetHextStr(echoBytes).Replace(" ", "");
                    break;
                }
            }
            if (!eraseReq1.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x71, 0x01, 0xFF, 0x00 }).Replace(" ", "")))
            {
                resultMsg = "NG 擦除失败1：" + eraseReq1;
                return;
            }

            var eraseReq2 = GetLinRecvMsg(new byte[] { _nad, 0x04, 0x31, 0x03, 0xFF, 0x00, 0xFF, 0xFF },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!eraseReq2.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x71, 0x03 }).Replace(" ", "")))
            {
                resultMsg = "NG 擦除失败2：" + eraseReq2;
                return;
            }

            Lin19200.SendMasterLin(_diagnosisMasterLinId,
                new byte[]
                {
                    _nad, 0x10, 0x0b, 0x34, 0x00, 0x44,
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[0],
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[1]
                });
            Thread.Sleep(25);
            var data34Req = GetLinRecvMsg(
                new byte[]
                {
                    _nad,
                    0x21,
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[2],
                    BitConverter.GetBytes(dataStartAddr).Reverse().ToArray()[3],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[0],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[1],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[2],
                    BitConverter.GetBytes(dataLen).Reverse().ToArray()[3]
                }, _diagnosisMasterLinId,
                _diagnosisSlaveLinId,
                isNeedRetry: false);

            if (!data34Req.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x74 }).Replace(" ", "")))
            {
                resultMsg = "NG data 34失败：" + data34Req;
                return;
            }

            if (!DataTransfer(dataRecord, ref resultMsg))
                return;

            var exitDataTransfer2 = GetLinRecvMsg(new byte[] { _nad, 0x01, 0x37, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF },
               _diagnosisMasterLinId, _diagnosisSlaveLinId, 200, false);
            if (!exitDataTransfer2.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x01, 0x77 }).Replace(" ", "")))
            {
                resultMsg = "NG data 37失败：" + flashDrv34Req;
                return;
            }

            Lin19200.SendMasterLin(_diagnosisMasterLinId,
                new byte[]
                {
                    _nad, 0x10, 0x0d, 0x31, 0x01, 0xFF, 0x01, 0x44
                });
            Thread.Sleep(25);
            Lin19200.SendMasterLin(_diagnosisMasterLinId,
                new byte[]
                {
                    _nad, 0x21,
                    0x00, 0xFF, 0xDC, 0x00, 0x00, 0x00
                });
            Thread.Sleep(25);
            var routineControl3101Ff01Req =
                GetLinRecvMsg(
                    new byte[]
                    {
                        _nad, 0x22,
                        0x1C, 0x00,
                        0xFF, 0xFF, 0xFF, 0xFF
                    },
                    _diagnosisMasterLinId,
                    _diagnosisSlaveLinId, 200, false);
            if (!routineControl3101Ff01Req.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x71, 0x01, 0xFF, 0x01 }).Replace(" ", "")))
            {
                resultMsg = "NG 31 01 ff 01失败：" + routineControl3101Ff01Req;
                return;
            }

            var routineControl3103Ff01Req = GetLinRecvMsg(new byte[] { _nad, 0x04, 0x31, 0x03, 0xFF, 0x01, 0xFF, 0xFF },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, isNeedRetry: false);
            if (!routineControl3103Ff01Req.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x04, 0x71, 0x03, 0xFF, 0x01 }).Replace(" ", "")))
            {
                resultMsg = "NG 31 03 ff 01失败：" + routineControl3103Ff01Req;
                return;
            }

            resultMsg = "OK";
        }

        private bool DataTransfer(IReadOnlyList<byte> dataBytes, ref string msg)
        {
            if (dataBytes.Count % 128 != 0)
            {
                msg = "NG 数据非128的整数倍";
                return false;
            }

            var dataIndex = 0;
            var countOf36 = dataBytes.Count / 128;

            for (var i = 0; i < countOf36; i++)
            {
                var rollingCount = 0x21;
                var firstFrame = new byte[] { _nad, 0x10, 0x82, 0x36, (byte)(i + 1), dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++] };
                Lin19200.SendMasterLin(_diagnosisMasterLinId, firstFrame);

                for (var j = 0; j < 125 / 6; j++)
                {
                    var continueFrame = new[]
                    {
                        _nad,
                        (byte) rollingCount,
                        dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++],
                        dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++]
                    };

                    Lin19200.SendMasterLin(_diagnosisMasterLinId, continueFrame);

                    rollingCount++;
                    if (rollingCount > 0x2F)
                        rollingCount = 0x20;
                }

                var lastFrameRequest = GetLinRecvMsg(new[]
                {
                    _nad, (byte) rollingCount,
                    dataBytes[dataIndex++], dataBytes[dataIndex++], dataBytes[dataIndex++],
                    dataBytes[dataIndex++], dataBytes[dataIndex++], (byte)0xFF
                },
                _diagnosisMasterLinId, _diagnosisSlaveLinId, 250);

                if (!lastFrameRequest.StartsWith(ValueHelper.GetHextStr(new byte[] { _nad, 0x02, 0x76, (byte)(i + 1) }).Replace(" ", "")))
                {
                    msg = "NG 36传输失败：" + lastFrameRequest;
                    return false;
                }
            }

            return true;
        }

        private static uint GenerateSeed(IReadOnlyList<byte> seedBytes)
        {
            uint seedNumber = seedBytes[0];
            seedNumber = (seedNumber << 8) + seedBytes[1];
            seedNumber = (seedNumber << 8) + seedBytes[2];
            seedNumber = (seedNumber << 8) + seedBytes[3];

            return seedNumber;
        }

        private static byte[] GenerateKeys(uint seed)
        {
            var wKey = 0xFFFFFFFF - seed;

            var key4 = wKey >> 24 & 0xff;
            var key5 = wKey >> 16 & 0xff;
            var key6 = wKey >> 8 & 0xff;
            var key7 = wKey & 0xff;

            return new[] { (byte)key4, (byte)key5, (byte)key6, (byte)key7 };
        }

        private string GetLinRecvMsg(
            byte[] sendBytes, byte masterLinId, byte slaveLinId, int delayMs = 50, bool isNeedRetry = true)
        {
            byte[] resultBytes;
            if (Lin19200.SendMasterLinAndRecvSingleSlaveLin(
                masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
            {
                if (resultBytes != null && resultBytes.Length == 8)
                    return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));

                if (isNeedRetry)
                {
                    Thread.Sleep(500);
                    if (!Lin19200.SendMasterLinAndRecvSingleSlaveLin(
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
                if (!Lin19200.SendMasterLinAndRecvSingleSlaveLin(
                    masterLinId, slaveLinId, sendBytes.ToArray(), out resultBytes, delayMs))
                    return string.Empty;

                if (resultBytes != null && resultBytes.Length == 8)
                    return resultBytes.Aggregate(string.Empty,
                            (current, t) => current + ValueHelper.GetHextStr(t));
            }

            return string.Empty;
        }

        #endregion

        [Description("R,重复CAL下载测试结果")]
        public string RepeatCalDownloadResult = string.Empty;

        [Description("重复CAL下载测试")]
        public void RepeatCalDownload(int repeatCount)
        {
            RepeatCalDownloadResult = string.Empty;

            var okCount = 0;
            var ngCount = 0;

            for (var i = 0; i < repeatCount; i++)
            {
                var st = new Stopwatch();
                st.Start();

                ModuleAwake();
                Thread.Sleep(100);
                ModuleSleep();
                Thread.Sleep(100);
                CaiDownload();
                Thread.Sleep(250);

                st.Stop();

                if (CalDownloadResult == "OK")
                    okCount++;
                else
                    ngCount++;

                RepeatCalDownloadResult += string.Format("第{0}次CAL下载结果：{1}；耗时：{2}ms\r\n", i + 1, CalDownloadResult,
                    st.ElapsedMilliseconds);
                //Console.WriteLine(@"第{0}次下载结果：{1}；耗时：{2}ms", i + 1, CalDownloadResult, st.ElapsedMilliseconds);
            }

            RepeatCalDownloadResult += string.Format("重复CAL下载测试结果，ok：{0}次，ng：{1}，共：{2}\r\n", okCount, ngCount, repeatCount);
            //Console.WriteLine(@"下载测试结果，ok：{0}次，ng：{1}，共：{2}", okCount, ngCount, repeatCount);
        }
    }
}
