using CommonUtility.MyCameraSdk.Cameralibs.HKCamera;
using NationalInstruments.Vision;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace CommonUtility.HikSdk
{
    public sealed class MyCamera
    {
        public delegate void PushMatEventHandle(
            MvCameraSdk.MV_GIGE_DEVICE_INFO gigeInfo, Mat mat);
        public static event PushMatEventHandle PushMat;

        private void OnPushMat(Mat mat)
        {
            if (PushMat != null)
                PushMat(GigeInfo, mat);
            else
                mat.Dispose();
        }

        public IntPtr PUser { get; private set; }
        public MvCameraSdk.MV_GIGE_DEVICE_INFO GigeInfo { get; private set; }
        public int CameraIndex { get; private set; }

        private MvCameraSdk.MV_CC_DEVICE_INFO _mPDeviceInfo;
        private MvCameraSdk.MV_FRAME_OUT_INFO_EX _mStFrameInfo = new MvCameraSdk.MV_FRAME_OUT_INFO_EX();
        private MvCameraSdk.MV_IMAGE_BASIC_INFO pstInfo;
        private MvCameraSdk.MVCC_INTVALUE pstValue;
        //private uint _mNSaveImageBufSize = 0;
        private IntPtr _mPSaveImageBuf = IntPtr.Zero;
        private IntPtr _grabBuff = IntPtr.Zero;
        public readonly MvCameraSdk _mvCameraSdk = new MvCameraSdk();

        private readonly object _mBufForSaveImageLock = new object();
        private readonly MvCameraSdk.cbOutputExdelegate _cbImage;

        private bool _isOpened;
        private bool _isGrabbing;
        private bool _isCapturing;
        private uint _captureCount;
        private int _captureRow = -1;
        private int _captureCol = -1;
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);
        public Mat[,] MatBuffer = new Mat[10, 200];
        public uint PayloadSize;
        public uint PayloadWidth;
        public uint PayloadHeight;

        private uint _roiOffsetXStep = 2;
        private uint _payloadMinWidth = 32;
        private uint _roiOffsetYStep = 2;
        private uint _payloadMinHeight = 32;

        private uint _buffSizeForSaveImage = 3072 * 2048 * (16 * 3 + 4) + 2048;
        private byte[] _bufForSaveImage;

        public MyCamera(IntPtr pUser, MvCameraSdk.MV_GIGE_DEVICE_INFO gigeInfo)
        {
            PUser = pUser;
            CameraIndex = (int)pUser;
            GigeInfo = gigeInfo;

            _bufForSaveImage = new byte[_buffSizeForSaveImage];
            _cbImage = ImageCallBack; //ImageCallBack;
        }

        ~MyCamera() { }

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        private void ProcessAndSaveImage(IntPtr pData, MvCameraSdk.MV_FRAME_OUT_INFO_EX pFrameInfo, ulong ts)
        {
            try
            {
                #region 分步骤转换
                //var independentMat = new Mat(pFrameInfo.nHeight, pFrameInfo.nWidth, MatType.CV_8UC1, pData); // OPENCV直接copy内存
                var independentMat = new Mat(pFrameInfo.nHeight, pFrameInfo.nWidth, MatType.CV_8UC1); // 先实例化mat
                CopyMemory(independentMat.Data, pData, pFrameInfo.nFrameLen); // 再copy内存

                if (pFrameInfo.enPixelType == MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerRG8)
                    Cv2.CvtColor(independentMat, independentMat, ColorConversionCodes.BayerRG2RGB);
                else if (pFrameInfo.enPixelType == MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGR8)
                    Cv2.CvtColor(independentMat, independentMat, ColorConversionCodes.BayerGR2RGB);
                else
                    Cv2.CvtColor(independentMat, independentMat, ColorConversionCodes.BayerRG2BGR);

                #endregion

                // 设置字体、文本内容、字体大小和颜色
                const HersheyFonts fontFace = HersheyFonts.HersheySimplex;
                const double fontScale = 0.45;
                int baseline;
                var showTxt = string.Format("{0}/[{1},{2}],DevTS={3},HostTS={4}", pFrameInfo.nFrameNum, _captureRow, _captureCol, ts, pFrameInfo.nHostTimeStamp);
                // 获取文字的尺寸
                var textSize = Cv2.GetTextSize(showTxt, fontFace, fontScale, 1, out baseline);

                var frameIndex = pFrameInfo.nFrameNum;
                //Cv2.PutText(matImage, showTxt, new Point(1, 15), fontFace, fontScale, Scalar.WhiteSmoke);
                //Cv2.PutText(independentMat, showTxt, new Point(1, pFrameInfo.nHeight - textSize.Height), fontFace, fontScale, Scalar.WhiteSmoke);
                //Console.WriteLine(showTxt);

                if (_isGrabbing && !_isCapturing && _isPushMat)
                {
                    //Cv2.ImWrite(string.Format(@"E:\测试grab图像\GrabImg{0}_{1}.jpg", _grabIndex.ToString().PadLeft(8, '0'),
                    //    HighPrecisionTimer.GetTimestamp()), independentMat);
                    //_grabIndex++;
                    var toPushMat = independentMat.Clone();
                    OnPushMat(toPushMat);
                }

                if (_isCapturing && _captureCount > 0)
                {
                    lock (_lockGetBuff)
                    {
                        MatBuffer[_captureRow, _captureCol] = independentMat.Clone();
                        _matPosList.Add(new MatPos { Row = _captureRow, Col = _captureCol, TimeStamp = ts, FrameIndex = (int)frameIndex, PixelType = pFrameInfo.enPixelType });
                    }

                    _captureCol++;
                    if (_captureCol == MatBuffer.GetLength(1))
                    {
                        _captureCol = 0;
                        _captureRow++;
                    }

                    _captureCount--;
                    if (_listCaptureInptr.Any() && _targetGetCount >= 0 && _listCaptureInptr.Count > _targetGetCount)
                    {
                        Marshal.FreeHGlobal(_listCaptureInptr[_targetGetCount]);
                        _listCaptureInptr[_targetGetCount] = IntPtr.Zero;
                    }
                    _targetGetCount++;

                    //Console.WriteLine("grab callback cost: " + _st.ElapsedMilliseconds);                   

                    if (_captureCount == 0)
                    {
                        //_st.Stop();
                        //Console.WriteLine("grab cost: " + _st.ElapsedMilliseconds);

                        _waitHandle.Set();
                    }
                }

                independentMat.Dispose();
                independentMat = null;
                //Marshal.FreeHGlobal(pData);
                //GC.Collect();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private Bitmap ParseRawImageDatacallback(IntPtr pData, MvCameraSdk.MV_FRAME_OUT_INFO_EX stFrameInfo)
        {
            Bitmap output = null;

            MvCameraSdk.MvGvspPixelType enDstPixelType;
            if (IsMonoData(stFrameInfo.enPixelType))
            {
                enDstPixelType = MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Mono8;
            }
            else if (IsColorData(stFrameInfo.enPixelType))
            {
                enDstPixelType = MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed;
            }
            else
            {
                throw new NotSupportedException("Can not support such pixel type currently");
            }

            IntPtr pImage = Marshal.UnsafeAddrOfPinnedArrayElement(_bufForSaveImage, 0);

            MvCameraSdk.MV_PIXEL_CONVERT_PARAM stConverPixelParam = new MvCameraSdk.MV_PIXEL_CONVERT_PARAM
            {
                nWidth = stFrameInfo.nWidth,
                nHeight = stFrameInfo.nHeight,
                pSrcData = pData,
                nSrcDataLen = stFrameInfo.nFrameLen,
                enSrcPixelType = stFrameInfo.enPixelType,
                enDstPixelType = enDstPixelType,
                pDstBuffer = pImage,
                nDstBufferSize = _buffSizeForSaveImage
            };

            int nRet = _mvCameraSdk.MV_CC_ConvertPixelType_NET(ref stConverPixelParam);
            if (MvCameraSdk.MV_OK != nRet)
            {
                //throw new InvalidOperationException("Unable to convert pixel type");
                return null;
            }

            if (enDstPixelType == MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Mono8)
            {
                //************************Mono8 转 Bitmap*******************************
                output = new Bitmap(stFrameInfo.nWidth, stFrameInfo.nHeight, stFrameInfo.nWidth * 1,
                    PixelFormat.Format8bppIndexed, pImage);

                ColorPalette cp = output.Palette;
                // init palette
                for (int i = 0; i < 256; i++)
                {
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                }

                output.Palette = cp;
            }
            else
            {
                //*********************RGB8 转 Bitmap**************************
                for (int i = 0; i < stFrameInfo.nHeight; i++)
                {
                    for (int j = 0; j < stFrameInfo.nWidth; j++)
                    {
                        byte chRed = _bufForSaveImage[i * stFrameInfo.nWidth * 3 + j * 3];
                        _bufForSaveImage[i * stFrameInfo.nWidth * 3 + j * 3] =
                            _bufForSaveImage[i * stFrameInfo.nWidth * 3 + j * 3 + 2];
                        _bufForSaveImage[i * stFrameInfo.nWidth * 3 + j * 3 + 2] = chRed;
                    }
                }

                output = new Bitmap(stFrameInfo.nWidth, stFrameInfo.nHeight, stFrameInfo.nWidth * 3,
                    PixelFormat.Format24bppRgb, pImage);
            }

            return output;
        }

        private bool IsColorData(MvCameraSdk.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_YUV422_Packed:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_YCBCR411_8_CBYYCRYY:
                    return true;

                default:
                    return false;
            }
        }

        private bool IsMonoData(MvCameraSdk.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                    return true;

                default:
                    return false;
            }
        }

        public void OpenCamera()
        {
            if (PUser == IntPtr.Zero)
                return;

            if (_isOpened)
                return;

            //ch:获取选择的设备信息 | en:Get Selected Device Information
            var device =
                (MvCameraSdk.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(PUser, typeof(MvCameraSdk.MV_CC_DEVICE_INFO));

            var strTemp = string.Empty;
            if (device.nTLayerType == MvCameraSdk.MV_GIGE_DEVICE)
            {
                var gigeInfo = (MvCameraSdk.MV_GIGE_DEVICE_INFO)MvCameraSdk.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MvCameraSdk.MV_GIGE_DEVICE_INFO));

                if (!string.IsNullOrEmpty(gigeInfo.chUserDefinedName))
                    strTemp = "GEV: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")";
                else
                    strTemp = "GEV: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")";
            }
            else if (device.nTLayerType == MvCameraSdk.MV_USB_DEVICE)
            {
                var usbInfo =
                    (MvCameraSdk.MV_USB3_DEVICE_INFO)MvCameraSdk.ByteToStruct(device.SpecialInfo.stUsb3VInfo,
                        typeof(MvCameraSdk.MV_USB3_DEVICE_INFO));

                if (!string.IsNullOrEmpty(usbInfo.chUserDefinedName))
                    strTemp = "U3V: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")" + "\r\n";
                else
                    strTemp = "U3V: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")" + "\r\n";
            }

            // ch:打开设备 | en:Open Device
            var nRet = _mvCameraSdk.MV_CC_CreateDevice_NET(ref device);
            if (MvCameraSdk.MV_OK != nRet)
                return;
            nRet = _mvCameraSdk.MV_CC_OpenDevice_NET();
            if (MvCameraSdk.MV_OK != nRet)
            {
                strTemp = string.Format("Open Device[{0}] failed! nRet=0x{1}\r\n", strTemp, nRet.ToString("X"));
                return;
            }
            else
            {
                strTemp = string.Format("Open Device[{0}] success!\r\n", strTemp);
                _mPDeviceInfo = device;
                var stParam = new MvCameraSdk.MVCC_INTVALUE();

                var stParamRetPayloadWidth = _mvCameraSdk.MV_CC_GetIntValue_NET("SensorWidth", ref stParam);
                if (stParamRetPayloadWidth == MvCameraSdk.MV_OK)
                {
                    var nPayloadWidth = stParam.nCurValue;
                    PayloadWidth = nPayloadWidth;
                }
                else
                {
                    return;
                }

                var stParamRetPayloadHeight = _mvCameraSdk.MV_CC_GetIntValue_NET("SensorHeight", ref stParam);
                if (stParamRetPayloadHeight == MvCameraSdk.MV_OK)
                {
                    var nPayloadHeight = stParam.nCurValue;
                    PayloadHeight = nPayloadHeight;
                }
                else
                {
                    return;
                }

                _mvCameraSdk.MV_CC_SetWidth_NET(PayloadWidth);
                _mvCameraSdk.MV_CC_SetHeight_NET(PayloadHeight);
                _mvCameraSdk.MV_CC_SetAOIoffsetX_NET(0);
                _mvCameraSdk.MV_CC_SetAOIoffsetY_NET(0);
                _mvCameraSdk.MV_CC_SetWidth_NET(PayloadWidth);
                _mvCameraSdk.MV_CC_SetHeight_NET(PayloadHeight);
                _mvCameraSdk.MV_CC_SetHeartBeatTimeout_NET(1000);

                {
                    var stParamOffsetXRet = _mvCameraSdk.MV_CC_GetIntValue_NET("Width", ref stParam);
                    if (stParamOffsetXRet != MvCameraSdk.MV_OK)
                        return;
                    _roiOffsetXStep = stParam.nInc;
                    _payloadMinWidth = stParam.nMin;

                    var stParamOffsetYRet = _mvCameraSdk.MV_CC_GetIntValue_NET("Height", ref stParam);
                    if (stParamOffsetYRet != MvCameraSdk.MV_OK)
                        return;
                    _roiOffsetYStep = stParam.nInc;
                    _payloadMinHeight = stParam.nMin;
                }

                var stParamRetPayloadSize = _mvCameraSdk.MV_CC_GetIntValue_NET("PayloadSize", ref stParam);
                if (stParamRetPayloadSize == MvCameraSdk.MV_OK)
                {
                    var nPayloadSize = stParam.nCurValue;
                    PayloadSize = nPayloadSize;
                }

                if (_grabBuff != IntPtr.Zero)
                    Marshal.FreeHGlobal(_grabBuff);
                PayloadSize = PayloadWidth * PayloadHeight * 4;
                _grabBuff = Marshal.AllocHGlobal((int)PayloadSize);

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (device.nTLayerType == MvCameraSdk.MV_GIGE_DEVICE)
                {
                    //var nPacketSize = _mvCameraSdk.MV_CC_GetOptimalPacketSize_NET();
                    var nPacketSize = 1200;
                    if (nPacketSize > 0)
                    {
                        nRet = _mvCameraSdk.MV_CC_SetIntValueEx_NET("GevSCPSPacketSize", nPacketSize);
                        if (nRet != MvCameraSdk.MV_OK)
                            strTemp += string.Format("Set Packet Size failed! nRet=0x{0}\r\n", nRet.ToString("X"));

                        nRet = _mvCameraSdk.MV_CC_SetIntValueEx_NET("GevSCPD", 1000);
                        if (nRet != MvCameraSdk.MV_OK)
                            strTemp += string.Format("Set SCPD failed! nRet=0x{0}\r\n", nRet.ToString("X"));
                    }
                    else
                        strTemp += string.Format("Get Packet Size failed! nRet=0x{0}\r\n", nPacketSize.ToString("X"));
                }

                var isSetMvCcSetGrabStrategyNetOk = _mvCameraSdk.MV_CC_SetGrabStrategy_NET(MvCameraSdk.MV_GRAB_STRATEGY.MV_GrabStrategy_OneByOne);
                Console.WriteLine("set MV_CC_SetGrabStrategy_NET result = {0}", isSetMvCcSetGrabStrategyNetOk);
                Console.WriteLine("set resend result = {0}", _mvCameraSdk.MV_GIGE_SetResend_NET(0, 0, 0));
                _mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MvCameraSdk.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                //_mvCameraSdk.MV_CC_SetEnumValue_NET("PixelFormat", (uint)MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed);
                _mvCameraSdk.MV_CC_RegisterImageCallBackEx_NET(_cbImage, PUser);

                _isOpened = true;
            }
        }

        public void CloseCamera()
        {
            if (!_isOpened)
                return;

            if (_isGrabbing)
                StopGrab();

            try
            {
                _mvCameraSdk.MV_CC_CloseDevice_NET();
            }
            catch (Exception)
            {
            }
            _isOpened = false;
        }

        public void SetExposureTime(int exposureTime)
        {
            _mvCameraSdk.MV_CC_SetEnumValue_NET("ExposureAuto", 0);
            _mvCameraSdk.MV_CC_SetFloatValue_NET("ExposureTime", exposureTime);
        }

        public void SetGain(int gain)
        {
            _mvCameraSdk.MV_CC_SetFloatValue_NET("Gain", gain);
        }

        private long _grabTs = HighPrecisionTimer.GetTimestamp();
        private int _grabIndex = 0;
        private bool _isPushMat;

        public int LastCaptureRoiOffsetX;
        public int LastCaptureRoiOffsetY;
        public int LastCaptureRoiWidth;
        public int LastCaptureRoiHeight;

        public void StartGrab(bool isPushMat = true, Rect roiRect = new Rect())
        {
            if (_isOpened && !_isGrabbing)
            {
                //_grabBuff = Marshal.AllocHGlobal((int)PayloadSize);

                if (roiRect.X == 0 && roiRect.Y == 0 && roiRect.Width == 0 && roiRect.Height == 0)
                {
                    _mvCameraSdk.MV_CC_SetWidth_NET(PayloadWidth);
                    _mvCameraSdk.MV_CC_SetHeight_NET(PayloadHeight);
                    _mvCameraSdk.MV_CC_SetAOIoffsetX_NET(0);
                    _mvCameraSdk.MV_CC_SetAOIoffsetY_NET(0);
                    _mvCameraSdk.MV_CC_SetWidth_NET(PayloadWidth);
                    _mvCameraSdk.MV_CC_SetHeight_NET(PayloadHeight);

                    //var setWidthRet = _mvCameraSdk.MV_CC_SetWidth_NET((uint)416);
                    //var setHeightRet = _mvCameraSdk.MV_CC_SetHeight_NET((uint)312);
                    //var setAoiXRet = _mvCameraSdk.MV_CC_SetAOIoffsetX_NET((uint)208);
                    //var setAoiYRet = _mvCameraSdk.MV_CC_SetAOIoffsetY_NET((uint)198);
                }
                else
                {
                    var aoiX = roiRect.X;
                    var aoiY = roiRect.Y;
                    var aoiWidth = roiRect.Width;
                    var aoiHeight = roiRect.Height;
                    aoiWidth = aoiWidth < 32 ? 32 : aoiWidth;
                    aoiHeight = aoiHeight < 32 ? 32 : aoiHeight;

                    //_mvCameraSdk.MV_CC_SetWidth_NET(32);
                    //_mvCameraSdk.MV_CC_SetHeight_NET(32);
                    //_mvCameraSdk.MV_CC_SetAOIoffsetX_NET(0);
                    //_mvCameraSdk.MV_CC_SetAOIoffsetY_NET(0);

                    var setWidthRet = _mvCameraSdk.MV_CC_SetWidth_NET((uint)aoiWidth);
                    var setHeightRet = _mvCameraSdk.MV_CC_SetHeight_NET((uint)aoiHeight);
                    var setAoiXRet = _mvCameraSdk.MV_CC_SetAOIoffsetX_NET((uint)aoiX);
                    var setAoiYRet = _mvCameraSdk.MV_CC_SetAOIoffsetY_NET((uint)aoiY);

                    LastCaptureRoiOffsetX = setAoiXRet == MvCameraSdk.MV_OK ? aoiX : aoiX;
                    LastCaptureRoiOffsetY = setAoiYRet == MvCameraSdk.MV_OK ? aoiY : aoiY;
                    LastCaptureRoiWidth = setWidthRet == MvCameraSdk.MV_OK ? aoiWidth : aoiWidth;
                    LastCaptureRoiHeight = setHeightRet == MvCameraSdk.MV_OK ? aoiHeight : aoiHeight;

                    //_mvCameraSdk.MV_CC_SetWidth_NET(PayloadWidth);
                    //_mvCameraSdk.MV_CC_SetHeight_NET(PayloadHeight);
                    //_mvCameraSdk.MV_CC_SetAOIoffsetX_NET((uint)144);
                    //_mvCameraSdk.MV_CC_SetAOIoffsetY_NET((uint)120);
                    //_mvCameraSdk.MV_CC_SetWidth_NET((uint)416);
                    //_mvCameraSdk.MV_CC_SetHeight_NET((uint)354);
                }

                // 开始取流
                _mStFrameInfo.nFrameLen = 0;
                _mStFrameInfo.enPixelType = MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Undefined;
                _mvCameraSdk.MV_CC_StartGrabbing_NET();

                // 连续采集
                _mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerMode",
                    (uint)MvCameraSdk.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);

                _grabIndex = 0;
                _grabTs = HighPrecisionTimer.GetTimestamp();
                _isGrabbing = true;
                _isPushMat = isPushMat;
            }
        }

        public void StopGrab()
        {
            if (!_isGrabbing)
                return;

            _isGrabbing = false;

            //if (_grabBuff != IntPtr.Zero)
            //{
            //    Marshal.FreeHGlobal(_grabBuff);
            //}

            var nRet = _mvCameraSdk.MV_CC_StopGrabbing_NET();

            _mvCameraSdk.MV_CC_SetWidth_NET(PayloadWidth);
            _mvCameraSdk.MV_CC_SetHeight_NET(PayloadHeight);
            _mvCameraSdk.MV_CC_SetAOIoffsetX_NET(0);
            _mvCameraSdk.MV_CC_SetAOIoffsetY_NET(0);
            _mvCameraSdk.MV_CC_SetWidth_NET(PayloadWidth);
            _mvCameraSdk.MV_CC_SetHeight_NET(PayloadHeight);

            if (MvCameraSdk.MV_OK != nRet)
                return;

            return;

            //await Task.Run(() =>
            //{
            //    var nRet = _mvCameraSdk.MV_CC_StopGrabbing_NET();

            //    _mvCameraSdk.MV_CC_SetWidth_NET(PayloadWidth);
            //    _mvCameraSdk.MV_CC_SetHeight_NET(PayloadHeight);
            //    _mvCameraSdk.MV_CC_SetAOIoffsetX_NET(0);
            //    _mvCameraSdk.MV_CC_SetAOIoffsetY_NET(0);
            //    _mvCameraSdk.MV_CC_SetWidth_NET(PayloadWidth);
            //    _mvCameraSdk.MV_CC_SetHeight_NET(PayloadHeight);

            //    if (MvCameraSdk.MV_OK != nRet)
            //        return;

            //    return;
            //});
        }

        //private Stopwatch _st = new Stopwatch();

        private readonly List<IntPtr> _listCaptureInptr = new List<IntPtr>();
        private int _targetGetCount = -1;
        private int _targetGetTotalCount;

        public bool Capture(uint count = 1, int delayMs = 3000, Rect roiRect = new Rect())
        {
            ClearBuffer();

            if (count == 0)
                return false;

            if (!_isOpened)
                return false;

            if (count > 10 * 200)
                return false;

            //_st.Start();            

            //if (count == 1)
            //{
            //    if (_isGrabbing)
            //        StopGrab();

            //    var isSuccess = false;

            //    var captureAction = new Action(() =>
            //    {
            //        _captureRow = 0;
            //        _captureCol = 0;

            //        #region 测试用

            //        //// 设置软触发
            //        ////_mvCameraSdk.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
            //        //_mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MvCameraSdk.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
            //        //_mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MvCameraSdk.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
            //        //_mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MvCameraSdk.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);

            //        //// 开始取流
            //        //_mStFrameInfo.nFrameLen = 0;
            //        //_mStFrameInfo.enPixelType = MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Undefined;
            //        //_mvCameraSdk.MV_CC_StartGrabbing_NET();

            //        //var triggerRet = _mvCameraSdk.MV_CC_SetCommandValue_NET("TriggerSoftware");
            //        //if (triggerRet == 0)
            //        //{
            //        //    var stFrameInfo = new MvCameraSdk.MV_FRAME_OUT();
            //        //    var getBuffRet = _mvCameraSdk.MV_CC_GetImageBuffer_NET(ref stFrameInfo, 1000);
            //        //    if (getBuffRet == 0)
            //        //    {
            //        //        isSuccess = true;
            //        //    }
            //        //}

            //        //_mvCameraSdk.MV_CC_StopGrabbing_NET();

            //        #endregion

            //        #region 老方法，软触发模式

            //        _captureCount = count;
            //        _isCapturing = true;

            //        // 设置软触发
            //        _mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MvCameraSdk.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
            //        _mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MvCameraSdk.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
            //        _mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MvCameraSdk.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);

            //        // 开始取流
            //        _mStFrameInfo.nFrameLen = 0;
            //        _mStFrameInfo.enPixelType = MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Undefined;
            //        _mvCameraSdk.MV_CC_StartGrabbing_NET();

            //        var taskWait = Task.Factory.StartNew(() =>
            //        {
            //            isSuccess = _waitHandle.WaitOne(delayMs);
            //        });

            //        var taskTrigger = Task.Factory.StartNew(() =>
            //        {
            //            // 软触发一次
            //            _mvCameraSdk.MV_CC_SetCommandValue_NET("TriggerSoftware");
            //        });

            //        Task.WaitAll(taskTrigger, taskWait);

            //        #endregion
            //    });

            //    captureAction.Invoke();

            //    _captureCount = 0;
            //    _isCapturing = false;

            //    if (!isSuccess)
            //    {
            //        captureAction.Invoke();

            //        if (!isSuccess)
            //        {
            //            ClearBuffer();
            //            var blackImage = new Mat(3009, 3009, MatType.CV_8UC3, new Scalar(0, 0, 0));
            //            MatBuffer[0, 0] = blackImage.Clone();
            //            blackImage.Dispose();
            //            GC.Collect();
            //        }
            //    }

            //    _mvCameraSdk.MV_CC_StopGrabbing_NET();
            //    //_mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MvCameraSdk.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);

            //    return isSuccess;
            //}
            //else
            //{
            //    _captureRow = 0;
            //    _captureCol = 0;

            //    _captureCount = 0;
            //    _isCapturing = true;

            //    if (!_isGrabbing)
            //        StartGrab();

            //    _captureCount = count;
            //    var isSuccess = _waitHandle.WaitOne(delayMs);

            //    _captureRow = -1;
            //    _captureCol = -1;
            //    _captureCount = 0;
            //    _isCapturing = false;

            //    if (!isSuccess)
            //    {
            //        ClearBuffer();

            //        var tempCount = count;

            //        var row = 0;
            //        var col = 0;

            //        for (var i = 0; i < tempCount; i++)
            //        {
            //            if (MatBuffer[row, col] == null ||
            //                MatBuffer[row, col].Empty())
            //            {
            //                var blackImage = new Mat(908, 908, MatType.CV_8UC1, new Scalar(0, 0, 0));
            //                MatBuffer[row, col] = blackImage.Clone();
            //                blackImage.Dispose();

            //                lock (_lockGetBuff)
            //                    _matPosList.Add(new MatPos { Col = col, Row = row, TimeStamp = (ulong)HighPrecisionTimer.GetTimestamp() });
            //            }

            //            col++;
            //            if (col != MatBuffer.GetLength(1))
            //                continue;
            //            col = 0;
            //            row++;
            //        }

            //        GC.Collect();
            //    }

            //    //_matPosList = _matPosList.OrderBy(f => f.TimeStamp).ToList();
            //    lock (_lockGetBuff)
            //        _matPosList = _matPosList.OrderBy(f => f.FrameIndex).ToList();
            //    StopGrab();
            //    return isSuccess;
            //}

            _captureRow = 0;
            _captureCol = 0;
            _captureCount = 0;

            for (var i = 0; i < _listCaptureInptr.Count; i++)
            {
                if (_listCaptureInptr[i] == IntPtr.Zero)
                    continue;
                Marshal.FreeHGlobal(_listCaptureInptr[i]);
                _listCaptureInptr[i] = IntPtr.Zero;
            }
            _listCaptureInptr.Clear();

            _targetGetCount = 0;
            _targetGetTotalCount = (int)count;
            for (var i = 0; i < count; i++)
                _listCaptureInptr.Add(Marshal.AllocHGlobal((int)PayloadSize));

            var isSuccess = false;
            GC.Collect();
            var captureTask = Task.Factory.StartNew(() =>
            {
                if (!_isGrabbing)
                    StartGrab(false, roiRect);

                _captureCount = count;
                Thread.Sleep(5);
                _isCapturing = true;
            });
            var waitTask = Task.Factory.StartNew(() =>
            {
                isSuccess = _waitHandle.WaitOne(delayMs);
            });

            Task.WaitAll(captureTask, waitTask);

            _captureRow = -1;
            _captureCol = -1;
            _captureCount = 0;

            _isCapturing = false;

            if (!isSuccess)
            {
                ClearBuffer();

                var tempCount = count;

                var row = 0;
                var col = 0;

                for (var i = 0; i < tempCount; i++)
                {
                    if (MatBuffer[row, col] == null ||
                        MatBuffer[row, col].Empty())
                    {
                        var blackImage = new Mat((int)PayloadHeight, (int)PayloadWidth, MatType.CV_8UC3, new Scalar(0, 0, 0));
                        MatBuffer[row, col] = blackImage.Clone();
                        blackImage.Dispose();

                        lock (_lockGetBuff)
                            _matPosList.Add(new MatPos { Col = col, Row = row, TimeStamp = (ulong)HighPrecisionTimer.GetTimestamp() });
                    }

                    col++;
                    if (col != MatBuffer.GetLength(1))
                        continue;
                    col = 0;
                    row++;
                }

                GC.Collect();
            }

            StopGrab();
            //_matPosList = _matPosList.OrderBy(f => f.TimeStamp).ToList();
            lock (_lockGetBuff)
                _matPosList = _matPosList.OrderBy(f => f.FrameIndex).ToList();

            for (var i = 0; i < _listCaptureInptr.Count; i++)
            {
                if (_listCaptureInptr[i] == IntPtr.Zero)
                    continue;
                Marshal.FreeHGlobal(_listCaptureInptr[i]);
                _listCaptureInptr[i] = IntPtr.Zero;
            }
            _listCaptureInptr.Clear();
            _targetGetCount = -1;
            _targetGetTotalCount = 0;
            return isSuccess;
        }

        public void ClearBuffer()
        {
            lock (_lockGetBuff)
                _matPosList.Clear();

            for (var i = 0; i < _listCaptureInptr.Count; i++)
            {
                if (_listCaptureInptr[i] == IntPtr.Zero)
                    continue;
                Marshal.FreeHGlobal(_listCaptureInptr[i]);
                _listCaptureInptr[i] = IntPtr.Zero;
            }
            _listCaptureInptr.Clear();

            _mvCameraSdk.MV_CC_ClearImageBuffer_NET();
            _lastFrameIndex = -1;
            _lastHostTs = 0;
            _lastTs = 0;
            _firstFrameLen = 0;

            var row = MatBuffer.GetLength(0);
            var col = MatBuffer.GetLength(1);

            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    if (MatBuffer[i, j] == null || MatBuffer[i, j].Empty())
                    {
                        MatBuffer[i, j] = null;
                        continue;
                    }
                    MatBuffer[i, j].Dispose();
                    MatBuffer[i, j] = null;
                }
            }

            GC.Collect();
            MatBuffer = new Mat[row, col];
        }

        private List<MatPos> _matPosList = new List<MatPos>();
        private int _lastFrameIndex;
        private ulong _lastTs;
        private long _lastHostTs;
        private uint _firstFrameLen;

        private readonly object _lockGetBuff = new object();

        public Mat GetImageFromBuff(int index, out int row, out int col)
        {
            lock (_lockGetBuff)
            {
                row = -1;
                col = -1;

                if (index >= 0 && index < _matPosList.Count && _matPosList.Any())
                {
                    var matPos = _matPosList[index];
                    row = matPos.Row;
                    col = matPos.Col;

                    if (row == -1 || col == -1)
                        return null;

                    return MatBuffer[row, col];
                }

                return null;
            }
        }

        public void SaveAllImg()
        {

        }

        private void ImageCallBack(
            IntPtr pData, ref MvCameraSdk.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            // 800*600的高速相机，3000曝光值，图像回传一次大约6ms~8ms，即可以采集到16s的数据
            // 800*600的高速相机，10000曝光值，可以采集到20s的数据
            // 800*600的高速相机，1500曝光值，可以采集到16s的数据

            var nIndex = (int)pUser;

            if (nIndex != CameraIndex)
                return;
            if (pData == IntPtr.Zero)
                return;

            //var systemTs = HighPrecisionTimer.GetTimestamp();
            //var cameraHostTs = pFrameInfo.nHostTimeStamp;
            //var cameraTs = ((ulong)pFrameInfo.nDevTimeStampHigh << 32) | pFrameInfo.nDevTimeStampLow;
            //Console.WriteLine("systemTs={0}; cameraHostTs={1}; cameraTs={2}", systemTs, cameraHostTs, cameraTs);

            if (_isCapturing)
            {
                var imageCallBackTs = HighPrecisionTimer.GetTimestamp();
                var grabTsCost = HighPrecisionTimer.GetTimestampIntervalMs(_grabTs, imageCallBackTs);
                if (grabTsCost < 2 && _targetGetTotalCount > 1)
                    return;

                if (_targetGetCount != -1 && _targetGetCount < _listCaptureInptr.Count && _listCaptureInptr[_targetGetCount] != IntPtr.Zero)
                {
                    CopyMemory(_listCaptureInptr[_targetGetCount], pData, pFrameInfo.nFrameLen);
                }

                _grabTs = imageCallBackTs;

                var ts = ((ulong)pFrameInfo.nDevTimeStampHigh << 32) | pFrameInfo.nDevTimeStampLow;

                if (pFrameInfo.nFrameNum <= _lastFrameIndex ||
                    pFrameInfo.nHostTimeStamp <= _lastHostTs ||
                    ts <= _lastTs ||
                    pFrameInfo.nHostTimeStamp <= 0)
                    return;

                if (_lastFrameIndex == -1)
                    _firstFrameLen = pFrameInfo.nFrameLen;
                else
                {
                    if (pFrameInfo.nFrameLen >= _firstFrameLen)
                        _firstFrameLen = pFrameInfo.nFrameLen;
                    else
                        return;
                }

                _lastFrameIndex = (int)pFrameInfo.nFrameNum;
                _lastHostTs = pFrameInfo.nHostTimeStamp;
                _lastTs = ts;

                lock (_mBufForSaveImageLock)
                {
                    if (_targetGetCount < _listCaptureInptr.Count && _targetGetCount != -1 && _listCaptureInptr[_targetGetCount] != IntPtr.Zero)
                    {
                        //// 将图像数据和信息加入队列
                        //var newQueue = new QueueImageClass(_listCaptureInptr[_targetGetCount], pFrameInfo, ts, true);
                        //_imageQueue.Enqueue(newQueue);
                        ProcessAndSaveImage(_listCaptureInptr[_targetGetCount], pFrameInfo, ts);
                    }
                }
            }
            else
            {
                CopyMemory(_grabBuff, pData, pFrameInfo.nFrameLen);
                //// 将图像数据和信息加入队列
                //var newQueue = new QueueImageClass(_grabBuff, pFrameInfo, (ulong)HighPrecisionTimer.GetTimestamp());
                //_imageQueue.Enqueue(newQueue);
                ProcessAndSaveImage(_grabBuff, pFrameInfo, (ulong)HighPrecisionTimer.GetTimestamp());
            }
        }

        private struct MatPos
        {
            public ulong TimeStamp;
            public int Row;
            public int Col;
            public int FrameIndex;
            public MvCameraSdk.MvGvspPixelType PixelType;
        }

        private class QueueImageClass
        {
            public IntPtr Data;
            public MvCameraSdk.MV_FRAME_OUT_INFO_EX Info;
            public ulong Ts;

            public QueueImageClass(IntPtr data, MvCameraSdk.MV_FRAME_OUT_INFO_EX info, ulong ts)

            {
                //if (Data != IntPtr.Zero)
                //{
                //    Marshal.Release(Data);
                //    Data = IntPtr.Zero;
                //}

                Data = data;

                //if (Data != IntPtr.Zero)
                //{
                //    Marshal.Release(Data);
                //    Data = IntPtr.Zero;
                //}
                //Data = Marshal.AllocHGlobal((int)info.nFrameLen);
                //CopyMemory(Data, data, info.nFrameLen);

                Info = info;
                Ts = ts;
            }
        }

        public Rect GetOuterMaxRect(List<Rect> rects)
        {
            var xStep = (int)_roiOffsetXStep;
            var yStep = (int)_roiOffsetYStep;

            var minX = (int)_payloadMinWidth;
            var mxY = (int)_payloadMinHeight;

            var left = rects.Min(f => f.X);
            var right = rects.Max(f => f.X + f.Width);
            var top = rects.Min(f => f.Y);
            var bottom = rects.Max(f => f.Y + f.Height);

            if (left % xStep != 0)
                left -= left % xStep;
            left = left - minX < 0 ? 0 : left - minX;

            if (right % xStep != 0)
                right += (xStep - right % xStep);
            right = right + minX > PayloadWidth ? (int)PayloadWidth : right + minX;

            if (top % yStep != 0)
                top -= top % yStep;
            top = top - mxY < 0 ? 0 : top - mxY;

            if (bottom % yStep != 0)
                bottom += (yStep - bottom % yStep);
            bottom = bottom + mxY > PayloadHeight ? (int)PayloadHeight : bottom + mxY;

            var roiRect = new Rect(new Point(left, top), new Size(right - left, bottom - top));

            if (right - left < _payloadMinWidth || bottom - top < _payloadMinHeight)
                return new Rect(0, 0, (int)PayloadWidth, (int)PayloadHeight);

            //var x = roiRect.X - roiRect.X % (int)PayloadWidth;
            //var y= roiRect.Y - roiRect.Y % (int)PayloadHeight;
            //var width= roiRect.Width - roiRect.Width % (int)PayloadWidth;
            //var height = roiRect.Height - roiRect.Height % (int)PayloadWidth;
            //width= width<32?32:width;
            //height = height < 32 ? 32 : height;

            return roiRect;
        }

        #region 常用方法

        public static VisionImage MatToVisionImage(Mat matImage)
        {
            var channel = matImage.Channels();

            var bytes = new byte[matImage.Total() * matImage.ElemSize()];// 创建与图像大小相匹配的字节数组

            Marshal.Copy(matImage.Data, bytes, 0, bytes.Length);// 将Mat对象的数据复制到字节数组中

            var visionImg = ImageProcessing.ConvertBytesToVisionImg(bytes, matImage.Cols, matImage.Rows, channel > 1);

            return visionImg;
        }

        public static Mat VisionImageToMat(VisionImage visionImg)
        {
            var st = new Stopwatch();
            st.Start();

            var pixelValue = visionImg.ImageToArray();

            var matType = new MatType();
            Mat image = null;

            // 并行遍历二维数组，并设置 Mat 矩阵的每个元素的值
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount // 设置最大线程数
            };

            if (visionImg.Type == ImageType.U8)
            {
                // 构造一个 size 为 imageWidth x imageHeight、通道数为 1 (表示 RGB) 的 Mat
                matType = MatType.CV_8UC1;
                image = new Mat(visionImg.Height, visionImg.Width, matType);

                var u8 = pixelValue.U8;

                Parallel.ForEach(Partitioner.Create(0, u8.GetLength(0)), parallelOptions, range =>
                {
                    for (var y = range.Item1; y < range.Item2; y++)
                    {
                        for (var x = 0; x < u8.GetLength(1); x++)
                        {
                            var color = new Vec3b(u8[y, x], 0, 0);
                            image.Set(y, x, color);
                        }
                    }
                });
            }
            else if (visionImg.Type == ImageType.Rgb32)
            {
                // 构造一个 size 为 imageWidth x imageHeight、通道数为 3 (表示 RGB) 的 Mat
                matType = MatType.CV_8UC3;
                image = new Mat(visionImg.Height, visionImg.Width, matType);
                var rgb32 = pixelValue.Rgb32;

                Parallel.ForEach(Partitioner.Create(0, rgb32.GetLength(0)), parallelOptions, range =>
                {
                    for (var y = range.Item1; y < range.Item2; y++)
                    {
                        for (var x = 0; x < rgb32.GetLength(1); x++)
                        {
                            try
                            {
                                var color = new Vec3b(rgb32[y, x].Blue, rgb32[y, x].Green, rgb32[y, x].Red);
                                image.Set(y, x, color);
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                        }
                    }
                });
            }

            // 保存图像到文件中，也可以进一步处理
            st.Stop();

            var title = string.Format("VisionImageToMat get {0} image cost = {1}ms", matType, st.ElapsedMilliseconds);
            Console.WriteLine(title);

            //Cv2.ImShow(title, image);

            //image.SaveImage("output.jpg");

            return image;
            //return GammaCorrection(image, 2.2f);
        }

        public static double GetLxByRgb(Scalar meanVal)
        {
            // double r, double g, double b
            var r = meanVal.Val2;
            var g = meanVal.Val1;
            var b = meanVal.Val0;

            var varR = r / 255;
            var varG = g / 255;
            var varB = b / 255;

            if (varR > 0.04045)
                varR = Math.Pow(((varR + 0.055) / 1.055), 2.4);
            else
                varR /= 12.92;

            if (varG > 0.04045)
                varG = Math.Pow(((varG + 0.055) / 1.055), 2.4);
            else
                varG /= 12.92;

            if (varB > 0.04045)
                varB = Math.Pow(((varB + 0.055) / 1.055), 2.4);
            else
                varB /= 12.92;

            varR *= 100;
            varG *= 100;
            varB *= 100;

            var x = varR * 0.4124 + varG * 0.3576 + varB * 0.1805;
            var y = varR * 0.2126 + varG * 0.7152 + varB * 0.0722;
            var z = varR * 0.0193 + varG * 0.1192 + varB * 0.9505;

            return Math.Round(y, 2, MidpointRounding.AwayFromZero);

            //var gamma = 2.2;
            //// 首先，对RGB颜色值应用Gamma校正
            //var rGamma = Math.Pow(r / 255.0, gamma);
            //var gGamma = Math.Pow(g / 255.0, gamma);
            //var bGamma = Math.Pow(b / 255.0, gamma); // 接着，计算加权灰度值，这里假设Gamma校正后也使用相同的加权系数
            //var gray = 0.299 * rGamma + 0.587 * gGamma + 0.114 * bGamma;

            ////将得到的值重新映射回 [0,255] 范围内
            //var grayValue = gray * 255.0;

            //// 返回值前确保在 [0,255] 范围内，并转换为灰度值
            //if (grayValue > 255)
            //{
            //    return 255;
            //}

            //if (grayValue <= 0)
            //{
            //    return 0;
            //}

            //return (byte)grayValue;
        }

        public static string BitmapToBase64String(Bitmap bitmap)
        {
            var st = new Stopwatch();
            st.Start();

            try
            {
                // 将Bitmap转换为字节数组
                byte[] bitmapBytes;
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    bitmapBytes = ms.ToArray();
                }

                // 将字节数组转换为Base64字符串
                var base64String = Convert.ToBase64String(bitmapBytes);
                return base64String;
            }
            catch (Exception)
            {
                return string.Empty;
            }
            finally
            {
                st.Stop();
                //Console.WriteLine("BitmapToBase64String执行一次耗时: {0}/ms", st.ElapsedMilliseconds);
            }
        }

        public static Bitmap Base64StringToBitmap(string base64String)
        {
            var st = new Stopwatch();
            st.Start();

            try
            {
                // 从Base64字符串转换回字节数组
                var convertedBytes = Convert.FromBase64String(base64String);

                // 从字节数组转换回Bitmap
                using (var ms = new MemoryStream(convertedBytes))
                {
                    var convertedBitmap = new Bitmap(ms);
                    // 保存或使用转换后的Bitmap
                    return convertedBitmap;
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                st.Stop();
                //Console.WriteLine("BitmapToBase64String执行一次耗时: {0}/ms", st.ElapsedMilliseconds);
            }
        }

        public static Rect GetRectInMat(int matWidth, int matHeight, Rect originRect)
        {
            var left = originRect.Left;
            if (left < 0)
                left = 0;

            var top = originRect.Top;
            if (top < 0)
                top = 0;

            var width = originRect.Width;
            if (left + width > matWidth)
                width -= left + width - matWidth;

            var height = originRect.Height;
            if (top + height > matHeight)
                height -= top + height - matHeight;

            return new Rect(left, top, width, height);
        }

        /// <summary>
        /// 找到图像中最大的轮廓的外接矩形
        /// </summary>
        /// <param name="matSrc">图像</param>
        /// <param name="outerRect">找到的最大轮廓</param>
        /// <param name="retrievalModes">RetrievalModes</param>
        /// <param name="contourApproximationModes">ContourApproximationModes</param>
        /// <param name="isFindTypeByArea">筛选类型,是否通过面积筛选,false则为周长</param>
        /// <returns></returns>
        public static bool TryGetMaxContourOuterRect(
            Mat matSrc, out Rect outerRect, RetrievalModes retrievalModes = RetrievalModes.External, ContourApproximationModes contourApproximationModes = ContourApproximationModes.ApproxSimple, bool isFindTypeByArea = true)
        {
            outerRect = new Rect();

            var maxSize = 0d;
            var maxRect = new Rect();
            var counterLen = 0d;
            var contourIndex = -1;

            using (var grayMat = matSrc.Clone())
            {
                if (grayMat.Channels() == 3)
                    Cv2.CvtColor(grayMat, grayMat, ColorConversionCodes.BGR2GRAY);

                // 找到轮廓
                Point[][] contours;
                HierarchyIndex[] hierarchy;
                Cv2.FindContours(grayMat, out contours, out hierarchy, retrievalModes, contourApproximationModes);

                for (var i = 0; i < hierarchy.Length; i++)
                {
                    var temp = contours[i];
                    var tempSize = isFindTypeByArea ? Cv2.ContourArea(temp) : Cv2.ArcLength(temp, true);
                    //var tempLen = Cv2.ArcLength(temp, true);

                    if (!(tempSize > maxSize))
                        continue;

                    var areaRect = Cv2.MinAreaRect(temp);
                    var tpRect = areaRect.BoundingRect();
                    var tpNewRect = GetRectInMat(grayMat.Width, grayMat.Height, tpRect);
                    {
                        maxRect = tpNewRect;
                        maxSize = tempSize;
                        //counterLen = tempSize;
                        //contourIndex = i;
                    }
                }

                grayMat.Dispose();
            }

            if (!(maxSize > 0))
                return false;

            outerRect = maxRect;
            return true;
        }

        /// <summary>
        /// 找到图像中最大的轮廓的
        /// </summary>
        /// <param name="matSrc">图像</param>
        /// <param name="outerContour">找到的最大轮廓</param>
        /// <param name="retrievalModes">RetrievalModes</param>
        /// <param name="contourApproximationModes">ContourApproximationModes</param>
        /// <param name="isFindTypeByArea">筛选类型,是否通过面积筛选,false则为周长</param>
        /// <returns></returns>
        public static bool TryGetMaxContourOuter(
            Mat matSrc, out Point[][] outerContour, out int maxIndex, RetrievalModes retrievalModes = RetrievalModes.External, ContourApproximationModes contourApproximationModes = ContourApproximationModes.ApproxSimple, bool isFindTypeByArea = true)
        {
            var maxSize = 0d;
            var maxRect = new Rect();
            maxIndex = -1;

            //Point[][] contours;
            HierarchyIndex[] hierarchy;

            using (var grayMat = matSrc.Clone())
            {
                if (grayMat.Channels() == 3)
                    Cv2.CvtColor(grayMat, grayMat, ColorConversionCodes.BGR2GRAY);

                // 找到轮廓
                Cv2.FindContours(grayMat, out outerContour, out hierarchy, retrievalModes, contourApproximationModes);

                for (var i = 0; i < hierarchy.Length; i++)
                {
                    var temp = outerContour[i];
                    var tempSize = isFindTypeByArea ? Cv2.ContourArea(temp) : Cv2.ArcLength(temp, true);
                    //var tempLen = Cv2.ArcLength(temp, true);

                    if (!(tempSize > maxSize))
                        continue;

                    var areaRect = Cv2.MinAreaRect(temp);
                    var tpRect = areaRect.BoundingRect();
                    var tpNewRect = GetRectInMat(grayMat.Width, grayMat.Height, tpRect);
                    {
                        maxRect = tpNewRect;
                        maxSize = tempSize;
                        maxIndex = i;
                    }
                }

                grayMat.Dispose();
            }

            if (!(maxSize > 0))
                return false;

            return true;
        }

        static Mat GammaCorrection(Mat image, float gamma)
        {
            // 创建一个与输入图像相同大小的矩阵来存储校正后的图像
            Mat correctedImage = new Mat(image.Size(), image.Type());

            // 归一化像素值到 0 和 1 之间
            image.ConvertTo(correctedImage, MatType.CV_32F, 1.0 / 255.0);

            // 应用伽马校正
            Cv2.Pow(correctedImage, gamma, correctedImage);

            // 反归一化回原来的范围
            correctedImage.ConvertTo(correctedImage, MatType.CV_8U, 255.0);

            return correctedImage;
        }

        public static Mat FormatMat(string[] base64s)
        {
            var totalToDraw = (int)Math.Ceiling(Math.Sqrt(base64s.Length));
            var firstBitmap = Base64StringToBitmap(base64s[0]);
            var totalWidth = firstBitmap.Width * base64s.Length;
            var totalHeight = firstBitmap.Height * base64s.Length;
            var firstMat = BitmapConverter.ToMat(firstBitmap);
            var avgWidth = (int)Math.Ceiling(totalWidth / (double)base64s.Length);
            var avgHeight = (int)Math.Ceiling(totalHeight / (double)base64s.Length);
            var canvas = new Mat(new Size((avgWidth + 1) * totalToDraw, (avgHeight + 1) * totalToDraw), firstMat.Type(), Scalar.Black);
            firstBitmap.Dispose();
            firstMat.Dispose();

            var imgX = 0;
            var imgY = 0;

            for (var i = 0; i < base64s.Length; i++)
            {
                if (string.IsNullOrEmpty((base64s[i])))
                    continue;
                var resource = Base64StringToBitmap(base64s[i]);
                if (resource == null)
                    continue;

                using (var mat = BitmapConverter.ToMat(resource))
                {
                    var posX = imgX * avgWidth;
                    var posY = imgY * avgHeight;

                    imgX++;
                    if (imgX >= totalToDraw)
                    {
                        imgX = 0;
                        imgY++;
                    }

                    var scaleX = (double)avgWidth / mat.Width;
                    var scaleY = (double)avgHeight / mat.Height;

                    using (var scaleMat = mat.Resize(new Size(), scaleX, scaleY))
                    using (var roi = new Mat(canvas, new Rect((int)posX, (int)posY, avgWidth, avgHeight)))
                    {
                        scaleMat.CopyTo(roi);
                        scaleMat.Dispose();
                        roi.Dispose();
                    }

                    resource.Dispose();
                    mat.Dispose();
                }
            }

            const int MaxWidth = 4096;
            const int MaxHeight = 2160;
            int originalWidth = canvas.Width;
            int originalHeight = canvas.Height;
            if (originalWidth > MaxWidth || originalHeight > MaxHeight)
            {
                double widthRatio = (double)MaxWidth / originalWidth;
                double heightRatio = (double)MaxHeight / originalHeight;
                double scaleRatio = Math.Min(widthRatio, heightRatio); // 取较小比例以同时满足宽高限制

                // 4. 计算目标尺寸
                int targetWidth = (int)(originalWidth * scaleRatio);
                int targetHeight = (int)(originalHeight * scaleRatio);

                // 5. 执行缩放（推荐高质量插值算法）
                Cv2.Resize(
                    src: canvas,
                    dst: canvas,
                    dsize: new Size(targetWidth, targetHeight),
                    interpolation: InterpolationFlags.Lanczos4 // 或InterpolationFlags.Cubic
                );
            }

            return canvas;
        }

        public static Mat FormatMatRaw(Mat[] mats)
        {
            var totalToDraw = (int)Math.Ceiling(Math.Sqrt(mats.Length));
            var totalWidth = mats.Sum(f => f.Width);
            var totalHeight = mats.Sum(f => f.Height);
            var avgWidth = (int)Math.Ceiling(totalWidth / (double)mats.Length);
            var avgHeight = (int)Math.Ceiling(totalHeight / (double)mats.Length);
            var canvas = new Mat(new Size((avgWidth + 1) * totalToDraw, (avgHeight + 1) * totalToDraw), MatType.CV_8UC3, Scalar.Black);

            var imgX = 0;
            var imgY = 0;

            for (var i = 0; i < mats.Length; i++)
            {
                var mat = mats[i];

                var posX = imgX * avgWidth;
                var posY = imgY * avgHeight;

                imgX++;
                if (imgX >= totalToDraw)
                {
                    imgX = 0;
                    imgY++;
                }

                var scaleX = (double)avgWidth / mat.Width;
                var scaleY = (double)avgHeight / mat.Height;

                using (var scaleMat = mat.Resize(new Size(), scaleX, scaleY))
                using (var roi = new Mat(canvas, new Rect((int)posX, (int)posY, avgWidth, avgHeight)))
                {
                    scaleMat.CopyTo(roi);
                    scaleMat.Dispose();
                    roi.Dispose();
                }
            }

            const int MaxWidth = 4096;
            const int MaxHeight = 2160;
            int originalWidth = canvas.Width;
            int originalHeight = canvas.Height;
            if (originalWidth > MaxWidth || originalHeight > MaxHeight)
            {
                double widthRatio = (double)MaxWidth / originalWidth;
                double heightRatio = (double)MaxHeight / originalHeight;
                double scaleRatio = Math.Min(widthRatio, heightRatio); // 取较小比例以同时满足宽高限制

                // 4. 计算目标尺寸
                int targetWidth = (int)(originalWidth * scaleRatio);
                int targetHeight = (int)(originalHeight * scaleRatio);

                // 5. 执行缩放（推荐高质量插值算法）
                Cv2.Resize(
                    src: canvas,
                    dst: canvas,
                    dsize: new Size(targetWidth, targetHeight),
                    interpolation: InterpolationFlags.Lanczos4 // 或InterpolationFlags.Cubic
                );
            }

            return canvas;
        }

        #endregion
    }
}
