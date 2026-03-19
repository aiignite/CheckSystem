using MvCodeReaderSDKNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace CommonUtility
{
    public class HikScanerClass
    {
        //public delegate void PushImgEventHandle(
        //    Image img, int index, string deviceName,
        //    MvCodeReader.MV_CODEREADER_IMAGE_OUT_INFO_EX2 stFrameInfoEx2,
        //    MvCodeReader.MV_CODEREADER_RESULT_BCR_EX2 stBcrResultEx2,
        //    MvCodeReader.MV_CODEREADER_WAYBILL_LIST stWayList,
        //    MvCodeReader.MV_CODEREADER_OCR_INFO_LIST stOcrInfo);
        public static event PushImgEventHandle PushImage;
        public delegate void PushImgEventHandle(Image img, string deviceSn, List<BarcodeStruct> barcodeScanResult);

        public static List<MyHikScanerDevice> HikScanerDevices = new List<MyHikScanerDevice>();
        public static Image MBitMap1;

        /// <summary>
        /// 设备列表信息
        /// </summary>
        private MvCodeReader.MV_CODEREADER_DEVICE_INFO_LIST _mPstDeviceList;

        /// <summary>
        /// 设备是否正在采集
        /// </summary>
        private bool IsDevicesGrabbing { get; set; }

        /// <summary>
        /// 设备是否打开
        /// </summary>
        private bool IsDevicesOpened { get; set; }

        /// <summary>
        /// ch:在线设备数量
        /// en:Online Device Number
        /// </summary>
        private int _mNDevNum;

        /// <summary>
        /// ch:设备使用数量
        /// en:Used Device Number
        /// </summary>
        private int _mNCanOpenDeviceNum;

        public HikScanerClass()
        {
            _mPstDeviceList = new MvCodeReader.MV_CODEREADER_DEVICE_INFO_LIST();
            IsDevicesGrabbing = false;
            IsDevicesOpened = false;
            _mNCanOpenDeviceNum = 0;
            _mNDevNum = 0;
            DeviceListAcq();
        }

        ~HikScanerClass()
        {
            try
            {
                if (IsDevicesGrabbing)
                {
                    StopGrabbing();
                }
                if (IsDevicesOpened)
                {
                    CloseDevices();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 打开相机
        /// </summary>
        public bool OpenDevices()
        {
            if (HikScanerDevices.Count <= 0)
                return false;

            if (IsDevicesOpened)
                return false;

            IsDevicesOpened = false;
            var nCameraUsingNum = _mNDevNum;

            for (int i = 0, j = 0; j < _mNDevNum; j++)
            {
                //ch:获取选择的设备信息 | en:Get Selected Device Information
                var stDevInfo =
                    (MvCodeReader.MV_CODEREADER_DEVICE_INFO)Marshal.PtrToStructure(_mPstDeviceList.pDeviceInfo[j], typeof(MvCodeReader.MV_CODEREADER_DEVICE_INFO));

                //ch:打开设备
                //en:Open Device
                if (null == HikScanerDevices[i].MPcDevice)
                {
                    HikScanerDevices[i].MPcDevice = new MvCodeReader();
                    if (null == HikScanerDevices[i].MPcDevice)
                    {
                        return false;
                    }
                }

                var nRet = HikScanerDevices[i].MPcDevice.MV_CODEREADER_CreateHandle_NET(ref stDevInfo);
                if (MvCodeReader.MV_CODEREADER_OK != nRet)
                {
                    // richTextBox.Text += String.Format("Create Handle[{0}] failed! nRet=0x{1}\r\n", StrTemp, nRet.ToString("X"));
                    return false;
                }

                nRet = HikScanerDevices[i].MPcDevice.MV_CODEREADER_OpenDevice_NET();
                if (MvCodeReader.MV_CODEREADER_OK != nRet)
                {
                    //richTextBox.Text += String.Format("Open Device[{0}] failed! nRet=0x{1}\r\n", StrTemp, nRet.ToString("X"));
                    continue;
                }
                else
                {
                    //richTextBox.Text += String.Format("Open Device[{0}] success!\r\n", StrTemp);

                    _mNCanOpenDeviceNum++;
                    HikScanerDevices[i].MPstDeviceInfo = stDevInfo;

                    if (stDevInfo.nTLayerType == MvCodeReader.MV_CODEREADER_GIGE_DEVICE)
                    {
                        var nPacketSize = HikScanerDevices[i].MPcDevice.MV_CODEREADER_GetOptimalPacketSize_NET();
                        if (nPacketSize > 0)
                        {
                            nRet = HikScanerDevices[i].MPcDevice.MV_CODEREADER_SetIntValue_NET("GevSCPSPacketSize", nPacketSize);
                            if (nRet != MvCodeReader.MV_CODEREADER_OK)
                            {
                                //richTextBox.Text += String.Format("Set Packet Size failed! nRet=0x{0}\r\n", nRet.ToString("X"));
                            }
                        }
                        else
                        {
                            //richTextBox.Text += String.Format("Get Packet Size failed! nPacketSize=0x{0}\r\n", nPacketSize.ToString("X"));
                        }
                    }

                    // MV_CODEREADER_CODE_TYPE
                    HikScanerDevices[i].MPcDevice.MV_CODEREADER_SetEnumValue_NET("TriggerMode", (uint)MvCodeReader.MV_CODEREADER_TRIGGER_MODE.MV_CODEREADER_TRIGGER_MODE_OFF);

                    IsDevicesOpened = true;
                    if (_mNCanOpenDeviceNum == nCameraUsingNum)
                    {
                        break;
                    }
                    i++;
                }
            }

            for (var i = 0; i < HikScanerDevices.Count; i++)
            {
                //if (i == 0 || i == 1)
                {
                    HikScanerDevices[i].MPcDevice.MV_CODEREADER_RegisterImageCallBackEx2_NET(HikScanerDevices[i].MCbImageOutput, (IntPtr)i);
                }
            }

            return true;
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        public void CloseDevices()
        {
            if (IsDevicesGrabbing)
            {
                StopGrabbing();
            }

            if (IsDevicesOpened)
            {
                for (var i = 0; i < _mNCanOpenDeviceNum; ++i)
                {
                    var nRet = HikScanerDevices[i].MPcDevice.MV_CODEREADER_CloseDevice_NET();
                    if (MvCodeReader.MV_CODEREADER_OK != nRet)
                    {
                        return;
                    }

                    //nRet = HikScanerDevices[i].MPcDevice.MV_CODEREADER_DestroyHandle_NET();
                    //if (MvCodeReader.MV_CODEREADER_OK != nRet)
                    //{
                    //    return;
                    //}
                }
            }

            // ch:取流标志位清零 
            // en:Zero setting grabbing flag bit
            IsDevicesGrabbing = false;
            IsDevicesOpened = false;

            // ch:重置成员变量 
            // en:Reset member variable
            ResetMember();
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        public bool StartGrabbing()
        {
            if (!IsDevicesGrabbing && IsDevicesOpened)
            {
                // ch:开始采集 
                // en:Start Grabbing
                for (var i = 0; i < _mNCanOpenDeviceNum; i++)
                {
                    if (!HikScanerDevices[i].IsTriggerSoftware)
                        SetContinuesMode(HikScanerDevices[i].DeviceSn); // 连续模式
                    else
                        SetTriggerMode(HikScanerDevices[i].DeviceSn); // 软触发模式

                    var nRet = HikScanerDevices[i].MPcDevice.MV_CODEREADER_StartGrabbing_NET();
                    if (MvCodeReader.MV_CODEREADER_OK != nRet)
                    {
                        return false;
                        //richTextBox.Text += String.Format("No.{0} Start Grabbing failed! nRet=0x{1}\r\n", (i + 1).ToString(), nRet.ToString("X"));
                    }
                }

                // ch:标志位置位true 
                // en:Set Position Bit true
                IsDevicesGrabbing = true;
            }

            return true;
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        public void StopGrabbing()
        {
            if (IsDevicesGrabbing && IsDevicesOpened)
            {
                for (var i = 0; i < _mNCanOpenDeviceNum; ++i)
                    HikScanerDevices[i].MPcDevice.MV_CODEREADER_StopGrabbing_NET();
                //ch:标志位设为false  
                // en:Set Flag Bit false
                IsDevicesGrabbing = false;
            }
        }

        /// <summary>
        /// 软触发一次
        /// </summary>
        public bool TriggerSoftware(string deviceSn, out string triggerResults, int delayMs)
        {
            triggerResults = string.Empty;
            if (HikScanerDevices.Any() && IsDevicesOpened && IsDevicesGrabbing)
            {
                var findDevice = HikScanerDevices.Find(f => f.DeviceSn == deviceSn);
                {
                    if (findDevice != null)
                    {
                        if (!findDevice.IsTriggerSoftware)
                            SetTriggerMode(findDevice.DeviceSn); // 软触发模式

                        findDevice.TriggerBuff = string.Empty;
                        findDevice.IsTriggerIng = true;

                        var nRet = findDevice.MPcDevice.MV_CODEREADER_SetCommandValue_NET("TriggerSoftware");
                        if (MvCodeReader.MV_CODEREADER_OK != nRet)
                        {
                            findDevice.IsTriggerIng = false;
                            //ShowErrorMsg("Trigger Software Fail!", nRet);
                            return false;
                        }

                        Thread.Sleep(delayMs);
                        triggerResults = findDevice.TriggerBuff;
                        findDevice.IsTriggerIng = false;

                        if (!findDevice.IsTriggerSoftware)
                            SetContinuesMode(findDevice.DeviceSn); // 连续模式
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 设置为连续模式
        /// </summary>
        /// <param name="deviceSn"></param>
        /// <returns></returns>
        public bool SetContinuesMode(string deviceSn)
        {
            var isDevicesGrabbing = IsDevicesGrabbing;
            if (HikScanerDevices.Any() && IsDevicesOpened)
            {

                if (isDevicesGrabbing)
                    StopGrabbing();
                var findDevice = HikScanerDevices.Find(f => f.DeviceSn == deviceSn);
                if (findDevice != null)
                {
                    findDevice.IsTriggerSoftware = false;
                    var nRet = findDevice.MPcDevice.MV_CODEREADER_SetEnumValue_NET("TriggerMode", (uint)MvCodeReader.MV_CODEREADER_TRIGGER_MODE.MV_CODEREADER_TRIGGER_MODE_OFF);
                    if (isDevicesGrabbing)
                        StartGrabbing();
                    return MvCodeReader.MV_CODEREADER_OK == nRet;
                }
            }

            if (isDevicesGrabbing)
                StartGrabbing();
            return false;
        }

        /// <summary>
        /// 设置为软触发模式
        /// </summary>
        /// <param name="deviceSn"></param>
        /// <returns></returns>
        public bool SetTriggerMode(string deviceSn)
        {
            var isDevicesGrabbing = IsDevicesGrabbing;
            if (HikScanerDevices.Any() && IsDevicesOpened)
            {
                if (isDevicesGrabbing)
                    StopGrabbing();
                var findDevice = HikScanerDevices.Find(f => f.DeviceSn == deviceSn);
                if (findDevice != null)
                {
                    findDevice.IsTriggerSoftware = true;
                    var nRet = findDevice.MPcDevice.MV_CODEREADER_SetEnumValue_NET("TriggerMode",
                        (uint)MvCodeReader.MV_CODEREADER_TRIGGER_MODE.MV_CODEREADER_TRIGGER_MODE_ON);
                    if (MvCodeReader.MV_CODEREADER_OK != nRet)
                    {
                        if (isDevicesGrabbing)
                            StartGrabbing();
                        //ShowErrorMsg("Set TriggerMode On Fail!", nRet);
                        //bnContinuesMode.Checked = true;
                        return false;
                    }

                    nRet = findDevice.MPcDevice.MV_CODEREADER_SetEnumValue_NET("TriggerSource",
                        (uint)MvCodeReader.MV_CODEREADER_TRIGGER_SOURCE.MV_CODEREADER_TRIGGER_SOURCE_SOFTWARE);
                    if (MvCodeReader.MV_CODEREADER_OK != nRet)
                    {
                        if (isDevicesGrabbing)
                            StartGrabbing();
                        //ShowErrorMsg("Set TriggerMode Source SoftWare Fail!", nRet);
                        return false;
                    }

                    if (isDevicesGrabbing)
                        StartGrabbing();
                    return true;
                }
            }

            if (isDevicesGrabbing)
                StartGrabbing();
            return false;
        }

        /// <summary>
        /// ch:枚举设备
        /// en:Create Device List
        /// </summary>
        public void DeviceListAcq()
        {
            if (!IsDevicesOpened)
            {
                GC.Collect();
                var nRet = MvCodeReader.MV_CODEREADER_EnumDevices_NET(
                    ref _mPstDeviceList, MvCodeReader.MV_CODEREADER_GIGE_DEVICE);
                if (MvCodeReader.MV_CODEREADER_OK != nRet)
                {
                    //richTextBox.Text += "Enumerate devices fail!\r\n";
                    return;
                }

                _mNDevNum = (int)_mPstDeviceList.nDeviceNum;
                for (var i = 0; i < _mNDevNum; i++)
                {
                    var device = new MyHikScanerDevice { DeviceIndex = i };
                    HikScanerDevices.Add(device);

                    //ch:获取选择的设备信息 | en:Get Selected Device Information
                    var stDevInfo =
                        (MvCodeReader.MV_CODEREADER_DEVICE_INFO)Marshal.PtrToStructure(_mPstDeviceList.pDeviceInfo[i], typeof(MvCodeReader.MV_CODEREADER_DEVICE_INFO));

                    var strTemp = string.Empty;
                    if (stDevInfo.nTLayerType == MvCodeReader.MV_CODEREADER_GIGE_DEVICE)
                    {
                        var buffer = Marshal.UnsafeAddrOfPinnedArrayElement(stDevInfo.SpecialInfo.stGigEInfo, 0);
                        var stGigeInfo = (MvCodeReader.MV_CODEREADER_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MvCodeReader.MV_CODEREADER_GIGE_DEVICE_INFO));
                        if (!string.IsNullOrEmpty(stGigeInfo.chUserDefinedName))
                        {
                            var chUserDefinedName = Encoding.GetEncoding("GB2312").GetBytes(stGigeInfo.chUserDefinedName);
                            var strUserDefinedName = Encoding.UTF8.GetString(chUserDefinedName);
                            strTemp = "GEV: " + strUserDefinedName + " (" + stGigeInfo.chSerialNumber + ")";
                        }
                        else
                        {
                            strTemp = "GEV: " + stGigeInfo.chManufacturerName + " " + stGigeInfo.chModelName + " (" + stGigeInfo.chSerialNumber + ")";
                        }

                        HikScanerDevices[i].DeviceSn = !string.IsNullOrEmpty(stGigeInfo.chSerialNumber)
                            ? stGigeInfo.chSerialNumber
                            : string.Empty;
                    }

                    HikScanerDevices[i].DeviceName = !string.IsNullOrEmpty(strTemp) ? strTemp : string.Empty;

                }
                //_mPcDevice = new MvCodeReader[_mNDevNum];
                //_mPstDeviceInfo = new MvCodeReader.MV_CODEREADER_DEVICE_INFO[_mNDevNum];
            }
        }

        /// <summary>
        /// 重置变量
        /// </summary>
        private void ResetMember()
        {
            if (!IsDevicesOpened)
            {
                _mPstDeviceList = new MvCodeReader.MV_CODEREADER_DEVICE_INFO_LIST();
                IsDevicesGrabbing = false;
                _mNCanOpenDeviceNum = 0;
                _mNDevNum = 0;

                HikScanerDevices.Clear();
                DeviceListAcq();
            }
        }

        /// <summary>
        /// 获取条码类型
        /// </summary>
        /// <param name="nBarType"></param>
        /// <returns></returns>
        public static string GetBarType(MvCodeReader.MV_CODEREADER_CODE_TYPE nBarType)
        {
            switch (nBarType)
            {
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_TDCR_DM:
                    return "DM码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_TDCR_QR:
                    return "QR码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_EAN8:
                    return "EAN8码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_UPCE:
                    return "UPCE码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_UPCA:
                    return "UPCA码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_EAN13:
                    return "EAN13码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_ISBN13:
                    return "ISBN13码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_CODABAR:
                    return "库德巴码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_ITF25:
                    return "交叉25码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_CODE39:
                    return "Code39码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_CODE93:
                    return "Code93码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_CODE128:
                    return "Code128码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_TDCR_PDF417:
                    return "PDF417码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_MATRIX25:
                    return "MATRIX25码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_MSI:
                    return "MSI码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_CODE11:
                    return "Code11码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_INDUSTRIAL25:
                    return "industria125码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_CHINAPOST:
                    return "中国邮政码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_BCR_ITF14:
                    return "交叉14码";
                case MvCodeReader.MV_CODEREADER_CODE_TYPE.MV_CODEREADER_TDCR_ECC140:
                    return "ECC140码";
                default:
                    return "/";
            }
        }

        public class MyHikScanerDevice
        {
            public int DeviceIndex;
            public MvCodeReader MPcDevice;
            public MvCodeReader.MV_CODEREADER_DEVICE_INFO MPstDeviceInfo;
            public string DeviceSn;
            public string DeviceName;
            public bool IsTriggerSoftware;
            public MvCodeReader.cbOutputEx2delegate MCbImageOutput;

            private readonly byte[] _mBufForDriver1 = new byte[1024 * 1024 * 20];
            public bool IsShowImg;
            //public Bitmap MBitMap1;

            public bool IsTriggerIng;
            public string TriggerBuff = string.Empty;

            public List<BarcodeScanResult> ContinuesModeScanLog = new List<BarcodeScanResult>();

            public MyHikScanerDevice()
            {
                IsTriggerSoftware = false;
                MCbImageOutput = ImageCallBack;
            }

            private void ImageCallBack(IntPtr pData, IntPtr pstFrameInfoEx2, IntPtr pUser)
            {
                var stFrameInfoEx2 =
                    (MvCodeReader.MV_CODEREADER_IMAGE_OUT_INFO_EX2)
                        Marshal.PtrToStructure(pstFrameInfoEx2, typeof(MvCodeReader.MV_CODEREADER_IMAGE_OUT_INFO_EX2));

                if (0 >= stFrameInfoEx2.nFrameLen)
                    return;

                var stBcrResultEx2 =
                        (MvCodeReader.MV_CODEREADER_RESULT_BCR_EX2)
                            Marshal.PtrToStructure(stFrameInfoEx2.UnparsedBcrList.pstCodeListEx2,
                                typeof(MvCodeReader.MV_CODEREADER_RESULT_BCR_EX2));

                if (IsTriggerIng)
                {
                    Marshal.Copy(pData, _mBufForDriver1, 0, (int)stFrameInfoEx2.nFrameLen);
                    BarcodeScanResult triggerBuff;

                    if (!string.IsNullOrEmpty(TriggerBuff))
                    {
                        triggerBuff = JsonConvert.DeserializeObject<BarcodeScanResult>(TriggerBuff);
                    }
                    else
                    {
                        triggerBuff = new BarcodeScanResult
                        {
                            Width = stFrameInfoEx2.nWidth,
                            Height = stFrameInfoEx2.nHeight
                        };
                    }

                    for (var i = 0; i < stBcrResultEx2.nCodeNum; ++i)
                    {
                        var strCode = Encoding.ASCII.GetString(stBcrResultEx2.stBcrInfoEx2[i].chCode).TrimEnd('\0');

                        if (!string.IsNullOrEmpty(strCode))
                        {
                            if (triggerBuff.BarcodeStructs.FindIndex(f => f.Barcode == strCode) == -1)
                            {
                                var newBarocodeStruct = new BarcodeStruct
                                {
                                    Barcode = string.IsNullOrEmpty(strCode) ? "NoRead" : strCode,
                                    TotalProcCost = stBcrResultEx2.stBcrInfoEx2[i].nTotalProcCost,
                                    AlgoCost = stBcrResultEx2.stBcrInfoEx2[i].sAlgoCost,
                                    Ppm = stBcrResultEx2.stBcrInfoEx2[i].sPPM,
                                    BarType =
                                        GetBarType(
                                            (MvCodeReader.MV_CODEREADER_CODE_TYPE)
                                                stBcrResultEx2.stBcrInfoEx2[i].nBarType),
                                    OverQuality = stBcrResultEx2.stBcrInfoEx2[i].stCodeQuality.nOverQuality,
                                    IdrScore = stBcrResultEx2.stBcrInfoEx2[i].nIDRScore
                                };

                                for (var j = 0; j < 4; ++j)
                                {
                                    newBarocodeStruct.Position[j].X = stBcrResultEx2.stBcrInfoEx2[i].pt[j].x;
                                    newBarocodeStruct.Position[j].Y = stBcrResultEx2.stBcrInfoEx2[i].pt[j].y;
                                }

                                triggerBuff.BarcodeStructs.Add(newBarocodeStruct);
                            }
                        }
                    }

                    TriggerBuff = JsonConvert.SerializeObject(triggerBuff);

                    if (IsShowImg)
                    {
                        if (stFrameInfoEx2.enPixelType ==
                        MvCodeReader.MvCodeReaderGvspPixelType.PixelType_CodeReader_Gvsp_Mono8)
                        {
                            var pImage = Marshal.UnsafeAddrOfPinnedArrayElement(_mBufForDriver1, 0);
                            var mBitMap1 = new Bitmap(stFrameInfoEx2.nWidth, stFrameInfoEx2.nHeight, stFrameInfoEx2.nWidth,
                                PixelFormat.Format8bppIndexed, pImage);
                            var cp = mBitMap1.Palette;
                            for (var i = 0; i < 256; i++)
                            {
                                cp.Entries[i] = Color.FromArgb(i, i, i);
                            }
                            mBitMap1.Palette = cp;
                            MBitMap1 = mBitMap1;
                        }
                        else if (stFrameInfoEx2.enPixelType ==
                                 MvCodeReader.MvCodeReaderGvspPixelType.PixelType_CodeReader_Gvsp_Jpeg)
                        {
                            GC.Collect();
                            var ms = new MemoryStream();
                            ms.Write(_mBufForDriver1, 0, (int)stFrameInfoEx2.nFrameLen);
                            MBitMap1 = Image.FromStream(ms);
                        }
                    }
                }
                else
                {
                    if (!IsTriggerSoftware)
                    {
                        if (PushImage != null)
                        {
                            Marshal.Copy(pData, _mBufForDriver1, 0, (int)stFrameInfoEx2.nFrameLen);

                            if (IsShowImg)
                            {
                                if (stFrameInfoEx2.enPixelType ==
                                MvCodeReader.MvCodeReaderGvspPixelType.PixelType_CodeReader_Gvsp_Mono8)
                                {
                                    var pImage = Marshal.UnsafeAddrOfPinnedArrayElement(_mBufForDriver1, 0);
                                    var mBitMap1 = new Bitmap(stFrameInfoEx2.nWidth, stFrameInfoEx2.nHeight, stFrameInfoEx2.nWidth,
                                        PixelFormat.Format8bppIndexed, pImage);
                                    var cp = mBitMap1.Palette;
                                    for (var i = 0; i < 256; i++)
                                    {
                                        cp.Entries[i] = Color.FromArgb(i, i, i);
                                    }
                                    mBitMap1.Palette = cp;
                                    MBitMap1 = mBitMap1;
                                }
                                else if (stFrameInfoEx2.enPixelType ==
                                         MvCodeReader.MvCodeReaderGvspPixelType.PixelType_CodeReader_Gvsp_Jpeg)
                                {
                                    GC.Collect();
                                    var ms = new MemoryStream();
                                    ms.Write(_mBufForDriver1, 0, (int)stFrameInfoEx2.nFrameLen);
                                    MBitMap1 = Image.FromStream(ms);

                                    //if (PushVisionImage != null) PushVisionImage(_mBufForDriver1, MBitMap1.Width, MBitMap1.Height, DeviceSn, 0);
                                }
                            }

                            var pushDatas = new List<BarcodeStruct>();
                            for (var i = 0; i < stBcrResultEx2.nCodeNum; ++i)
                            {
                                var strCode = Encoding.ASCII.GetString(stBcrResultEx2.stBcrInfoEx2[i].chCode).TrimEnd('\0');

                                if (!string.IsNullOrEmpty(strCode))
                                {
                                    var newBarocodeStruct = new BarcodeStruct
                                    {
                                        Barcode = string.IsNullOrEmpty(strCode) ? "NoRead" : strCode,
                                        TotalProcCost = stBcrResultEx2.stBcrInfoEx2[i].nTotalProcCost,
                                        AlgoCost = stBcrResultEx2.stBcrInfoEx2[i].sAlgoCost,
                                        Ppm = stBcrResultEx2.stBcrInfoEx2[i].sPPM,
                                        BarType =
                                            GetBarType(
                                                (MvCodeReader.MV_CODEREADER_CODE_TYPE)
                                                    stBcrResultEx2.stBcrInfoEx2[i].nBarType),
                                        OverQuality = stBcrResultEx2.stBcrInfoEx2[i].stCodeQuality.nOverQuality,
                                        IdrScore = stBcrResultEx2.stBcrInfoEx2[i].nIDRScore
                                    };

                                    for (var j = 0; j < 4; ++j)
                                    {
                                        newBarocodeStruct.Position[j].X = stBcrResultEx2.stBcrInfoEx2[i].pt[j].x;
                                        newBarocodeStruct.Position[j].Y = stBcrResultEx2.stBcrInfoEx2[i].pt[j].y;
                                    }

                                    pushDatas.Add(newBarocodeStruct);
                                }
                            }

                            //PushImage(MBitMap1, DeviceSn, pushDatas);
                            PushImage(null, DeviceSn, pushDatas);
                        }
                    }
                }
            }
        }

        public class BarcodeScanResult
        {
            public List<BarcodeStruct> BarcodeStructs = new List<BarcodeStruct>();
            //public Image Img;
            public ushort Width;
            public ushort Height;
        }

        public class BarcodeStruct
        {
            public BarcodeStruct()
            {
                Position = new Point[4];
            }

            /// <summary>
            /// 字符串
            /// </summary>
            public string Barcode { get; set; }

            /// <summary>
            /// 位置
            /// </summary>
            public Point[] Position { get; set; }

            /// <summary>
            /// 从触发开始到APP输出时间统计(ms)
            /// </summary>
            public uint TotalProcCost { get; set; }

            /// <summary>
            /// 算法耗时
            /// </summary>
            public ushort AlgoCost { get; set; }

            /// <summary>
            /// PPM(10倍)
            /// </summary>
            public ushort Ppm { get; set; }

            /// <summary>
            /// 条码类型
            /// </summary>
            public string BarType { get; set; }

            /// <summary>
            /// 总体质量评分（1D/2D公用）
            /// </summary>
            public int OverQuality { get; set; }

            /// <summary>
            /// 读码评分
            /// </summary>
            public uint IdrScore { get; set; }
        }
    }
}
