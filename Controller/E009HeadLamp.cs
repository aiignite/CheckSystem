using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using CommonUtility.FileOperator;

namespace Controller
{
    [Description("CAN-Product,红旗E009头灯")]
    public sealed class E009HeadLamp : ControllerBase
    {
        public CanBus Can;

        public E009HeadLamp(string name)
            : base(name)
        {
            ChangeToLeftNode();
            CanBus.PushCanMsg += CanBus_PushCanMsg;

            Th = new Thread(MainWork) { IsBackground = true };
            Th.Start();

            Th1003 = new Thread(MainWork1003) { IsBackground = true };
            Th1003.Start();
        }

        private void CanBus_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (Can != null && Can.Name == name && onPushCanDataType != CanBus.OnPushCanDataType.Tx)
            {
                if (_isReadNode)
                {
                    if (data.CanId == 0x70D || data.CanId == 0x1f6)
                    {
                        ReadNodeValue = 1;
                    }
                    else if (data.CanId == 0x7BB || data.CanId == 0x1f5)
                    {
                        ReadNodeValue = 2;
                    }
                }
            }
        }

        ~E009HeadLamp()
        {
            Dispose();
        }

        private bool _isSleep = true;
        private bool _isInExtendedSession;
        private Thread Th { get; set; }
        private Thread Th1003 { get; set; }

        #region 信号列表

        private readonly CanCommunicationMatrix.IntelMatrix _bcm11 =
            new CanCommunicationMatrix.IntelMatrix(0x230, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _bcm19 =
            new CanCommunicationMatrix.IntelMatrix(0x237, 8);

        private void MainWork()
        {
            while (Th.IsAlive)
            {
                if (!Th.IsAlive)
                    break;

                Thread.Sleep(20);

                try
                {
                    if (Can == null)
                        continue;

                    if (_isSleep)
                        continue;

                    var lstPages = new List<CanBus.CanDataPackage>
                    {
                        new CanBus.CanDataPackage(
                            _bcm19.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data,
                            _bcm19.MatrixData),
                        new CanBus.CanDataPackage(
                            _bcm11.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data,
                            _bcm11.MatrixData),
                    };

                    Can.SendCanDatas(lstPages.ToArray());
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void MainWork1003()
        {
            while (Th.IsAlive)
            {
                if (!Th.IsAlive)
                    break;

                Thread.Sleep(500);

                try
                {
                    if (Can == null)
                        continue;

                    if (_isSleep)
                        continue;

                    if (_isInExtendedSession)
                    {
                        Can.SendStandardCanData(CanDiagnosisRequestFunCanId, CanBus.MyUds.KeepExtendedSessionBytes(pendingBytes: 0xAA));
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        #endregion

        #region 诊断

        [Description("R,ECU复位结果")]
        public string EcuResetResult = string.Empty;

        [Description("R,Programming Date-F199")]
        public string ProgrammingDate = string.Empty;
        [Description("R,Tester Serial Number-F198")]
        public string TesterSerialNumber = string.Empty;

        [Description("R,SupplySwVersion-F195")]
        public string SupplySwVersion = string.Empty;
        [Description("R,SupplyHwVersion-F193")]
        public string SupplyHwVersion = string.Empty;
        [Description("R,SoftwareVersion-F1A0")]
        public string SoftwareVersion = string.Empty;

        [Description("R,LampBoardSoftwareVersion-F189")]
        public string LampBoardSoftwareVersion = string.Empty;
        [Description("R,LampBoardHardwareVersion-F191")]
        public string LampBoardHardwareVersion = string.Empty;
        [Description("R,LampBoardPartNumber-F187")]
        public string LampBoardPartNumber = string.Empty;

        private uint _canDiagnosisRequestPhyCanId = 0x7B3;
        private const uint CanDiagnosisRequestFunCanId = 0x7DF;
        private uint _canDiagnosisResponseCanId = 0x7BB;

        private uint _nodeReqCanId = 0x6F1;
        private uint _nodeRecvCanId = 0x6F9;

        //private uint _canDiagnosisRequestPhyCanId = 0x6f0;
        //private const uint CanDiagnosisRequestFunCanId = 0x7DF;
        //private uint _canDiagnosisResponseCanId = 0x6f8;

        private uint _level1SecurityAccessMask = 0x83B755C8u;
        private uint _level2SecurityAccessMask = 0x3AF2B6DCu;

        private readonly object _can1Locker = new object();

        private bool _isReadNode;
        public int ReadNodeValue;

        [Description("休眠")]
        public void ControllerSleep()
        {
            _isSleep = true;
        }

        [Description("唤醒")]
        public void ControllerAwake()
        {
            _isSleep = false;
        }

        [Description("读取节点信息")]
        public void ReadNode()
        {
            if (Can == null)
                return;

            ReadNodeValue = 0;
            _isReadNode = true;

            Can.SendStandardCanData(CanDiagnosisRequestFunCanId, new byte[] { 0x02, 0x11, 0x01, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa });
            Thread.Sleep(500);

            _isReadNode = false;

            if (ReadNodeValue == 1)
                ChangeToLeftNode();
            else if (ReadNodeValue == 2)
                ChangeToRightNode();

            //_readNodeValue = 0;
        }

        [Description("切换成左节点")]
        public void ChangeToLeftNode()
        {
            // 0x6f0 ==> FL
            // 0x6f8 ==> FL
            // 0x1f6 ==> FL
            _canDiagnosisRequestPhyCanId = 0x705;
            _canDiagnosisResponseCanId = 0x70D;

            _nodeReqCanId = 0x6F0;
            _nodeRecvCanId = 0x6F8;

            _level1SecurityAccessMask = 0x83A655B7u;
            _level2SecurityAccessMask = 0x3AC1B7D7u;

            //_datasetFilePath = DatasetFilePathFr;
            _datasetFilePath = DatasetFilePathFl;
        }

        [Description("切换成右节点")]
        public void ChangeToRightNode()
        {
            // 0x6f1 ==> FR
            // 0x6f9 ==> FR
            // 0x1f5 ==> FR
            _canDiagnosisRequestPhyCanId = 0x7B3;
            _canDiagnosisResponseCanId = 0x7BB;

            _nodeReqCanId = 0x6F1;
            _nodeRecvCanId = 0x6F9;

            _level1SecurityAccessMask = 0x83B755C8u;
            _level2SecurityAccessMask = 0x3AF2B6DCu;

            _datasetFilePath = DatasetFilePathFr;
        }

        [Description("ECU复位")]
        public string EcuReset()
        {
            EcuResetResult = string.Empty;

            byte[] ecuResetEcho;
            if (!Can.CanBusWithUds
                    .TesterTryRequest(CanDiagnosisRequestFunCanId, _canDiagnosisResponseCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho, CanBus.CanType.Standard))
            {
                Thread.Sleep(200);
                Can.CanBusWithUds
                    .TesterTryRequest(CanDiagnosisRequestFunCanId, _canDiagnosisResponseCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho, CanBus.CanType.Standard);
            }

            EcuResetResult = ValueHelper.GetHextStr(ecuResetEcho).Replace(" ", "");
            return EcuResetResult; //ValueHelper.GetHextStr(ecuResetEcho).Replace(" ", "");
        }

        [Description("进入正常模式")]
        public void EnterDefaultSession()
        {
            if (Can == null)
                return;

            lock (_can1Locker)
            {
                _isInExtendedSession = false;

                if (Can.CanBusWithUds.TryEnterDefaultSession(
                        CanDiagnosisRequestFunCanId,
                        _canDiagnosisResponseCanId, CanBus.CanType.Standard))
                    return;

                Thread.Sleep(500);

                Can.CanBusWithUds.TryEnterDefaultSession(
                    CanDiagnosisRequestFunCanId,
                    _canDiagnosisResponseCanId, CanBus.CanType.Standard);
            }
        }

        [Description("进入拓展模式")]
        public void EnterExtendedSession()
        {
            if (Can == null)
                return;

            lock (_can1Locker)
            {
                if (Can.CanBusWithUds.TryEnterExtendedSession(
                        CanDiagnosisRequestFunCanId,
                        _canDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (Can.CanBusWithUds.TryEnterExtendedSession(
                        CanDiagnosisRequestFunCanId,
                        _canDiagnosisResponseCanId, CanBus.CanType.Standard))
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

            //_isInExtendedSession = false;

            lock (_can1Locker)
            {
                if (Can.CanBusWithUds.TryEnterProgrammingSession(
                        CanDiagnosisRequestFunCanId,
                        _canDiagnosisResponseCanId, CanBus.CanType.Standard))
                {
                    return;
                }

                Thread.Sleep(500);
                Can.CanBusWithUds.TryEnterProgrammingSession(
                    CanDiagnosisRequestFunCanId,
                    _canDiagnosisResponseCanId, CanBus.CanType.Standard);
            }
        }

        //[Description("Request Seed")]
        //public void RequestSeed()
        //{
        //    byte[] seedBytes;
        //    if (!Can.CanBusWithUds.TryRequestSeed(
        //            _canDiagnosisRequestPhyCanId,
        //            _canDiagnosisResponseCanId,
        //            CanBus.CanType.Standard, 0x03, out seedBytes))
        //    {

        //    }
        //}

        [Description("ReadProgrammingDate")]
        public void ReadProgrammingDate(bool isNodeReq)
        {
            ProgrammingDate = string.Empty;
            ProgrammingDate = ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0x99, isNodeReq)).Replace(" ", "");
        }

        [Description("ReadTesterSerialNumber")]
        public void ReadTesterSerialNumber(bool isNodeReq)
        {
            TesterSerialNumber = string.Empty;
            TesterSerialNumber = ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0x98, isNodeReq)).Replace(" ", "");
        }

        [Description("ReadSupplySwVersion")]
        public void ReadSupplySwVersion(bool isNodeReq)
        {
            SupplySwVersion = string.Empty;
            SupplySwVersion = ReadDidViaCan(0xF1, 0x95, isNodeReq).GetStringByAsciiBytes(false);//ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0x95)).Replace(" ", "");
        }

        [Description("ReadSupplyHwVersion")]
        public void ReadSupplyHwVersion(bool isNodeReq)
        {
            SupplyHwVersion = string.Empty;
            SupplyHwVersion = ReadDidViaCan(0xF1, 0x93, isNodeReq).GetStringByAsciiBytes(false); //ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0x93)).Replace(" ", "");
        }

        [Description("ReadSoftwareVersion")]
        public void
            ReadSoftwareVersion(bool isNodeReq)
        {
            SoftwareVersion = string.Empty;
            SoftwareVersion = ReadDidViaCan(0xF1, 0xA0, isNodeReq).GetStringByAsciiBytes(false); //ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0xA0)).Replace(" ", "");
        }

        [Description("ReadLampBoardSoftwareVersion")]
        public void ReadLampBoardSoftwareVersion(bool isNodeReq)
        {
            LampBoardSoftwareVersion = string.Empty;
            LampBoardSoftwareVersion = ReadDidViaCan(0xF1, 0x89, isNodeReq).GetStringByAsciiBytes(false); //ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0x91)).Replace(" ", "");
        }

        [Description("ReadLampBoardHardwareVersion")]
        public void ReadLampBoardHardwareVersion(bool isNodeReq)
        {
            LampBoardHardwareVersion = string.Empty;
            LampBoardHardwareVersion = ReadDidViaCan(0xF1, 0x91, isNodeReq).GetStringByAsciiBytes(false);// ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0x89)).Replace(" ", "");
        }

        [Description("ReadLampBoardPartNumber")]
        public void ReadLampBoardPartNumber(bool isNodeReq)
        {
            LampBoardPartNumber = string.Empty;
            LampBoardPartNumber = ReadDidViaCan(0xF1, 0x87, isNodeReq).GetStringByAsciiBytes(false); //ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0x87)).Replace(" ", "");
        }

        [Description("读取灯控信息")]
        public void ReadLampControlVersion()
        {
            ReadProgrammingDate(false);
            ReadTesterSerialNumber(false);

            ReadSupplySwVersion(false);
            ReadSupplyHwVersion(false);
            ReadSoftwareVersion(false);

            ReadLampBoardHardwareVersion(false);
            ReadLampBoardSoftwareVersion(false);
            ReadLampBoardPartNumber(false);
        }

        [Description("读取灯具信息")]
        public void ReadLampVersion()
        {
            ReadProgrammingDate(true);
            ReadTesterSerialNumber(true);

            ReadSupplySwVersion(true);
            ReadSupplyHwVersion(true);
            ReadSoftwareVersion(true);

            ReadLampBoardHardwareVersion(true);
            ReadLampBoardSoftwareVersion(true);
            ReadLampBoardPartNumber(true);
        }

        private byte[] ReadDidViaCan(byte didHi, byte didLo, bool isNodeReq)
        {
            if (Can == null)
                return new List<byte>().ToArray();

            lock (_can1Locker)
            {
                byte[] echoBytes;
                if (Can.CanBusWithUds.TryReadData(
                        isNodeReq ? _nodeReqCanId : _canDiagnosisRequestPhyCanId,
                        isNodeReq ? _nodeRecvCanId : _canDiagnosisResponseCanId,
                        CanBus.CanType.Standard,
                        CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))

                {
                    if (echoBytes != null)
                        return echoBytes;

                    return new List<byte>().ToArray();

                    Thread.Sleep(250);
                    if (Can.CanBusWithUds.TryReadData(
                            isNodeReq ? _nodeReqCanId : _canDiagnosisRequestPhyCanId,
                            isNodeReq ? _nodeRecvCanId : _canDiagnosisResponseCanId,
                            CanBus.CanType.Standard,
                            CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                        return new List<byte>().ToArray();
                    return echoBytes ?? new List<byte>().ToArray();
                }

                Thread.Sleep(250);
                if (Can.CanBusWithUds.TryReadData(
                        isNodeReq ? _nodeReqCanId : _canDiagnosisRequestPhyCanId,
                        isNodeReq ? _nodeRecvCanId : _canDiagnosisResponseCanId,
                        CanBus.CanType.Standard,
                        CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                return new List<byte>().ToArray();

                Thread.Sleep(250);
                if (Can.CanBusWithUds.TryReadData(
                        isNodeReq ? _nodeReqCanId : _canDiagnosisRequestPhyCanId,
                        isNodeReq ? _nodeRecvCanId : _canDiagnosisResponseCanId,
                        CanBus.CanType.Standard,
                        CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (Can.CanBusWithUds.TryReadData(
                        isNodeReq ? _nodeReqCanId : _canDiagnosisRequestPhyCanId,
                        isNodeReq ? _nodeRecvCanId : _canDiagnosisResponseCanId,
                        CanBus.CanType.Standard,
                        CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                return echoBytes ?? new List<byte>().ToArray();
            }
        }

        #endregion

        #region APP刷新

        [Description("重复下载测试")]
        public void RepeatDownload(int repeatCount)
        {
            var okCount = 0;
            var ngCount = 0;

            for (var i = 0; i < repeatCount; i++)
            {
                var st = new Stopwatch();
                st.Start();

                ControllerAwake();
                Thread.Sleep(100);
                ControllerSleep();
                Thread.Sleep(100);
                EcuReset();
                Thread.Sleep(1000);
                DownLoadFile();
                Thread.Sleep(250);

                st.Stop();

                if (DownloadResult == "OK")
                {
                    okCount++;
                }
                else
                {
                    ngCount++;
                }

                Console.WriteLine(@"第{0}次下载结果：{1}；耗时：{2}ms", i + 1, DownloadResult, st.ElapsedMilliseconds);
            }

            Console.WriteLine(@"下载测试结果，ok：{0}次，ng：{1}，共：{2}", okCount, ngCount, repeatCount);
        }

        [Description("R,下载结果")]
        public string DownloadResult = string.Empty;

        [Description("R,下载耗时")]
        public string DownloadCostTime = string.Empty;

        public string DrvFlashFilePath = @"E:\Projects\红旗E009\BIN_File\E001_FlashDrvBin.s19";
        public string AppFilePath = @"E:\Projects\红旗E009\BIN_File\20240716\LDME007_App_Update_TAG7009.s19";
        public string DatasetFilePathFl = @"E:\Projects\红旗E009\BIN_File\20240716\E009FLGCHV03.s19";
        public string DatasetFilePathFr = @"E:\Projects\红旗E009\BIN_File\20240716\E009FRGCHV03.s19";

        private string _datasetFilePath = string.Empty;

        private static readonly object LockFile = new object();

        [Description("下载")]
        public void DownLoadFile()
        {
            DownloadResult = string.Empty;
            DownloadCostTime = string.Empty;

            if (Can == null)
            {
                DownloadResult = "NG CAN未初始化";
                return;
            }

            var fileListApp = new List<string>();
            var fileListCali = new List<string>();

            lock (LockFile)
            {
                if (!string.IsNullOrEmpty(AppFilePath))
                {
                    if (!File.Exists(AppFilePath))
                    {
                        DownloadResult = "NG APP文件不存在";
                        return;
                    }
                    else
                        fileListApp.Add(AppFilePath);
                }
                else
                {
                    DownloadResult = "NG APP文件不存在";
                    return;
                }

                if (!string.IsNullOrEmpty(_datasetFilePath))
                {
                    if (!File.Exists(_datasetFilePath))
                    {
                        DownloadResult = "NG DATASET文件不存在";
                        return;
                    }
                    else
                        fileListCali.Add(_datasetFilePath);
                }
                else
                {
                    DownloadResult = "NG DATASET文件不存在";
                    return;
                }
            }

            if (!fileListApp.Any())
            {
                DownloadResult = "NG 未指定APP下载文件";
                return;
            }

            if (!fileListCali.Any())
            {
                DownloadResult = "NG 未指定DATASET下载文件";
                return;
            }

            var downloadAction = new Action(() =>
            {
                #region APP

                if (!PreProgramming(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, ref DownloadResult))
                    return;

                if (!WriteProgramDate(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, ref DownloadResult))
                    return;

                if (!DownloadDrvFlash(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, ref DownloadResult))
                {
                    DownloadResult = "NG APP下载DrvFlash下载失败 " + DownloadResult;
                    return;
                }

                if (!Can.CanBusWithUds.TryStartRoutineControl(
                        _canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Standard,
                        Uds14229Helper.RoutineControl.EraseMemory))
                {
                    DownloadResult = "NG APP下载擦除内存3101FF00失败失败 " + DownloadResult;
                    return;
                }

                if (
                    fileListApp.Select(SRecordFileHelper.GetSRecordLineData)
                    .Select(SRecordFileHelper.GetBlocks)
                    .Any(blocks => !Can.CanBusWithUds.TransferData(
                        _canDiagnosisRequestPhyCanId,
                        _canDiagnosisResponseCanId,
                        CanBus.CanType.Standard,
                        blocks, false, ref DownloadResult, isCrcCheck: true)))
                {
                    DownloadResult = "NG APP下载Block失败";
                    return;
                }

                if (!Can.CanBusWithUds.TryStartRoutineControl(
                        _canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId,
                        CanBus.CanType.Standard, Uds14229Helper.RoutineControl.CheckProgrammingDependencies))
                {
                    DownloadResult = "NG APP下载检测程序一致完整性3101FF01失败";
                    return;
                }

                if (!EndProgramming(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, ref DownloadResult))
                    return;

                #endregion

                #region DATASET

                if (!PreProgramming(_nodeReqCanId, _nodeRecvCanId, ref DownloadResult))
                    return;

                if (!WriteProgramDate(_nodeReqCanId, _nodeRecvCanId, ref DownloadResult))
                    return;

                if (!DownloadDrvFlash(_nodeReqCanId, _nodeRecvCanId, ref DownloadResult))
                {
                    DownloadResult = "NG DATASET下载DrvFlash下载失败 " + DownloadResult;
                    return;
                }

                if (!Can.CanBusWithUds.TryStartRoutineControl(
                        _nodeReqCanId, _nodeRecvCanId, CanBus.CanType.Standard,
                        Uds14229Helper.RoutineControl.EraseMemory))
                {
                    DownloadResult = "NG DATASET下载擦除内存3101FF00失败失败 " + DownloadResult;
                    return;
                }

                if (
                    fileListCali.Select(SRecordFileHelper.GetSRecordLineData)
                    .Select(SRecordFileHelper.GetBlocks)
                    .Any(blocks => !Can.CanBusWithUds.TransferData(
                        _nodeReqCanId, _nodeRecvCanId,
                        CanBus.CanType.Standard,
                        blocks, false, ref DownloadResult, isCrcCheck: true)))
                {
                    DownloadResult = "NG DATASET下载Block失败";
                    return;
                }

                if (!Can.CanBusWithUds.TryStartRoutineControl(
                        _nodeReqCanId, _nodeRecvCanId,
                        CanBus.CanType.Standard, Uds14229Helper.RoutineControl.CheckProgrammingDependencies))
                {
                    DownloadResult = "NG DATASET下载检测程序一致完整性3101FF01失败";
                    return;
                }

                if (!EndProgramming(_nodeReqCanId, _nodeRecvCanId, ref DownloadResult))
                    return;

                #endregion

                DownloadResult = "OK";
            });

            var st = new Stopwatch();
            st.Start();
            downloadAction.Invoke();

            if (DownloadResult != "OK")
            {
                EcuReset();
                Thread.Sleep(1000);
                ControllerAwake();
                Thread.Sleep(500);
                ControllerSleep();
                Thread.Sleep(50);
                downloadAction.Invoke();
            }

            st.Stop();
            DownloadCostTime = Math.Round(st.ElapsedMilliseconds / 1000f, 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
        }

        //[Description("APP文件下载")]
        //public void DownLoadAppFile()
        //{
        //}

        //[Description("DATASET文件下载")]
        //public void DownLoadDatasetFile()
        //{
        //}

        /// <summary>
        /// 下载DrvFlash
        /// </summary>
        /// <param name="recvCanId"></param>
        /// <param name="result"></param>
        /// <param name="reqCanId"></param>
        /// <returns></returns>
        private bool DownloadDrvFlash(uint reqCanId, uint recvCanId, ref string result)
        {
            if (!string.IsNullOrEmpty(DrvFlashFilePath))
            {
                if (!File.Exists(DrvFlashFilePath))
                {
                    result = "NG DrvFlash文件不存在";
                    return false;
                }
            }
            else
            {
                result = "NG 未指定DrvFlash文件";
                return false;
            }

            var sRecord = SRecordFileHelper.GetSRecordLineData(DrvFlashFilePath);
            var drvFlashBlocks = SRecordFileHelper.GetBlocks(sRecord); // Block集合

            return Can.CanBusWithUds.TransferData(
                reqCanId,
                recvCanId,
                CanBus.CanType.Standard,
                drvFlashBlocks,
                false,
                ref result, isCrcCheck: true);
        }

        /// <summary>
        /// 预编程
        /// </summary>
        /// <param name="recvCanId"></param>
        /// <param name="result"></param>
        /// <param name="reqCanId"></param>
        /// <returns></returns>
        private bool PreProgramming(uint reqCanId, uint recvCanId, ref string result)
        {
            //ControllerAwake();
            //Thread.Sleep(100);

            if (!Can.CanBusWithUds.TryEnterExtendedSession(
                    reqCanId, recvCanId, CanBus.CanType.Standard))
            {
                result = "NG 进入拓展模式1003失败";
                return false;
            }

            if (!Can.CanBusWithUds.TryControlDtcSetting(
                    reqCanId, recvCanId,
                    CanBus.CanType.Standard,
                    Uds14229Helper.ControlDtcSetting.Disable))

            {
                result = "NG 禁止网络中所有ECU进行DTC设置8502失败";
                return false;
            }

            if (!Can.CanBusWithUds.TryCommunicationControl(
                    reqCanId, recvCanId, CanBus.CanType.Standard, Uds14229Helper.CommunicationControl.DisableRxAndTx))
            {
                result = "NG 禁止网络中所有ECU进行常规报文通信280303失败";
                return false;
            }

            if (!Can.CanBusWithUds.TryEnterProgrammingSession(
                    reqCanId, recvCanId, CanBus.CanType.Standard, timeOut: 1000))
            {
                result = "NG 进入编程模式1002失败";
                return false;
            }

            byte[] seedBytes;
            if (!Can.CanBusWithUds.TryRequestSeed(
                    reqCanId, recvCanId, CanBus.CanType.Standard, 0x03, out seedBytes, pendingByte: 0x55))
            {
                result = "NG 请求seed2703失败";
                return false;
            }

            // key2 = CalcKey(seed, 0x544D738A)
            var keyBytes = CalcKey(seedBytes, _level2SecurityAccessMask);

            if (!Can.CanBusWithUds.TrySendKey(
                    reqCanId, recvCanId, CanBus.CanType.Standard, 0x04, keyBytes))
            {
                result = "NG 写入key2704失败";
                return false;
            }

            Thread.Sleep(25);

            return true;
        }

        private bool WriteProgramDate(uint reqCanId, uint recvCanId, ref string result)
        {
            var dateTime = DateTime.Now;
            var str = string.Format("{0}{1}{2}", dateTime.Year, dateTime.Month.ToString().PadLeft(2, '0'),
                dateTime.Day.ToString().PadLeft(2, '0'));
            str += dateTime.ToString("HHmmssffff");
            str += "00";

            if (str.Length != 10 * 2)
            {
                result = "NG 生成编程日期参数异常";
                return false;
            }

            var bs = new List<byte>();
            for (var i = 0; i < str.Length; i = i + 2)
                bs.Add(Convert.ToByte(str.Substring(i, 2), 16));

            //if (!Can.CanBusWithUds.TryWriteData(reqCanId, recvCanId,
            //        CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x98, bs, timeoutFromMilliseconds: 1000))
            //{
            //    result = "NG 2E F1 98失败";
            //    return false;
            //}

            if (!Can.CanBusWithUds.TryWriteData(reqCanId, recvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x98, new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A }, timeoutFromMilliseconds: 1000))
            {
                result = "NG 2E F1 98失败";
                return false;
            }

            Thread.Sleep(200);

            //if (!Can.CanBusWithUds.TryWriteData(reqCanId, recvCanId,
            //        CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x99, bs.GetRange(0, 4), timeoutFromMilliseconds: 1000))
            //{
            //    result = "NG 2E F1 99失败";
            //    return false;
            //}

            if (!Can.CanBusWithUds.TryWriteData(reqCanId, recvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x99, new byte[] { 0x23, 0x07, 0x13, 0x14 }, timeoutFromMilliseconds: 1000))
            {
                result = "NG 2E F1 99失败";
                return false;
            }

            Thread.Sleep(200);

            return true;
        }

        private bool EndProgramming(uint reqCanId, uint recvCanId, ref string result)
        {
            EcuReset();

            if (EcuResetResult != "5101")
            {
                result = "NG ECU RESET失败";
                return false;
            }

            Thread.Sleep(1000);

            if (!Can.CanBusWithUds.TryEnterExtendedSession(
                    reqCanId, recvCanId, CanBus.CanType.Standard))
            {
                result = "NG 进入拓展模式1003失败";
                return false;
            }

            if (!Can.CanBusWithUds.TryCommunicationControl(
                    reqCanId, recvCanId, CanBus.CanType.Standard, Uds14229Helper.CommunicationControl.EnableRxAndTx))
            {
                result = "NG 打开网络中所有ECU进行常规报文通信280003失败";
                return false;
            }

            if (!Can.CanBusWithUds.TryControlDtcSetting(
                    reqCanId, recvCanId, CanBus.CanType.Standard,
                    Uds14229Helper.ControlDtcSetting.Enablle))
            {
                result = "NG 打开网络中所有ECU进行DTC设置8501失败";
                return false;
            }

            if (!Can.CanBusWithUds.TryEnterDefaultSession(
                    reqCanId, recvCanId, CanBus.CanType.Standard))
            {
                result = "NG 进入拓展模式1001失败";
                return false;
            }

            if (!Can.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(CanDiagnosisRequestFunCanId, _canDiagnosisResponseCanId, CanBus.CanType.Standard))
            {
                result = "NG ClearDTC失败";
                return false;
            }

            return true;
        }

        //public IEnumerable<byte> CalcKey(byte[] seedBytes, int securetyLevel)
        //{
        //    return new byte[] { 0xAE, 0x95, 0x27, 0x62 };
        //}

        private static IEnumerable<byte> CalcKey(uint wSeed, uint mask)
        {
            uint wTemp;
            uint wTop31Bits;
            uint jj;
            var wLastSeed = wSeed;
            var temp = (byte)((byte)((mask & 0x00000800) >> 10) | ((mask & 0x00200000) >> 21));

            if (temp == 0)
            {
                wTemp = (uint)((wSeed & 0xff000000) >> 24);
            }
            else if (temp == 1)
            {
                wTemp = (uint)((wSeed & 0x00ff0000) >> 16);
            }
            else if (temp == 2)
            {
                wTemp = (uint)((wSeed & 0x0000ff00) >> 8);
            }
            else
            {
                wTemp = (uint)(wSeed & 0x000000ff);
            }
            var sb1 = (uint)((mask & 0x000003FC) >> 2);
            var sb2 = (uint)(((mask & 0x7F800000) >> 23) ^ 0xA5);
            var sb3 = (uint)(((mask & 0x001FE000) >> 13) ^ 0x5A);

            var iterations = (uint)(((wTemp ^ sb1) & sb2) + sb3);

            for (jj = 0; jj < iterations; jj++)
            {
                wTemp = ((wLastSeed & 0x40000000) / 0x40000000) ^ ((wLastSeed & 0x01000000) / 0x01000000) ^ ((wLastSeed & 0x1000) / 0x1000) ^ ((wLastSeed & 0x04) / 0x04);
                var wLsBit = (wTemp & 0x00000001);
                wLastSeed = (uint)(wLastSeed << 1); /* Left Shift the bits */
                wTop31Bits = (uint)(wLastSeed & 0xFFFFFFFE);
                wLastSeed = (uint)(wTop31Bits | wLsBit);
            }

            if ((mask & 0x00000001) != 0)
            {
                wTop31Bits = ((wLastSeed & 0x00FF0000) >> 16) |
                             ((wLastSeed & 0xFF000000) >> 8) |
                             ((wLastSeed & 0x000000FF) << 8) |
                             ((wLastSeed & 0x0000FF00) << 16);
            }
            else
            {
                wTop31Bits = wLastSeed;
            }

            wTop31Bits = wTop31Bits ^ mask;

            var returnBs = BitConverter.GetBytes(wTop31Bits).Reverse();

            return returnBs;
        }

        private static IEnumerable<byte> CalcKey(byte[] wSeeds, uint mask)
        {
            Array.Reverse(wSeeds);
            var wSeed = BitConverter.ToUInt32(wSeeds, 0);
            return CalcKey(wSeed, mask);
        }

        #endregion
    }
}
