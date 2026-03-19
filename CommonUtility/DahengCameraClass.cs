using GxIAPINET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CommonUtility
{
    public class DahengCameraClass
    {
        public delegate void PushImgEventHandle(byte[] imgBytes, int width, int heigh, bool iscolor);
        public static event PushImgEventHandle PushImageBytes;

        public class CCamerInfo
        {
            /// <summary>
            /// 判断是否为彩色相机
            /// </summary>
            public bool MbIsColorFilter { get; set; }

            /// <summary>
            /// 相机已打开标志
            /// </summary>
            public bool MbIsOpen { get; set; }

            /// <summary>
            /// 相机正在采集标志
            /// </summary>
            public bool MbIsSnap { get; set; }

            /// <summary>
            /// 标识是否支持白平衡
            /// </summary>
            public bool MbWhiteAuto { get; set; }

            /// <summary>
            /// 相机是否掉线
            /// </summary>

            public bool MbIsOffLine { get; set; }

            /// <summary>
            /// 采集速度级别是否支持
            /// </summary>
            public bool MbAcqSpeedLevel { get; set; }

            /// <summary>
            /// 白平衡列表框转换标志
            /// </summary>
            public bool MbWhiteAutoSelectedIndex { get; set; }

            /// <summary>
            /// 帧率
            /// </summary>
            public double MdFps { get; set; }

            /// <summary>
            /// 设备对像
            /// </summary>
            public IGXDevice MObjIgxDevice;

            /// <summary>
            /// 流对像
            /// </summary>
            public IGXStream MObjIgxStream;

            /// <summary>
            /// 远端设备属性控制器对像
            /// </summary>
            public IGXFeatureControl MObjIgxFeatureControl;

            public GX_DEVICE_OFFLINE_CALLBACK_HANDLE MhOfflineCallback;

            /// <summary>
            /// 设备显示名称
            /// </summary>
            public string MStrDisplayName { get; set; }

            /// <summary>
            /// 序列号
            /// </summary>
            public string MStrSn { get; set; }

            /// <summary>
            /// 设备类型
            /// </summary>
            public GX_DEVICE_CLASS_LIST MEmDeviceType { get; set; }

            /// <summary>
            /// bitmap对象,仅供存储图像使用
            /// </summary>
            public Bitmap MBitmapForSave { get; set; }

            /// <summary>
            /// 图像数据大小
            /// </summary>
            public int MnPayloadSize { get; set; }

            /// <summary>
            /// 图像宽度
            /// </summary>
            public int MnWidth { get; set; }

            /// <summary>
            /// 图像高度
            /// </summary>
            public int MnHeigh { get; set; }

            /// <summary>
            /// 是否支持彩色相机
            /// </summary>
            public bool MbIsColor { get; set; }

            /// <summary>
            /// 黑白相机buffer
            /// </summary>
            public byte[] MByMonoBuffer { get; set; }

            /// <summary>
            /// 彩色相机buffer
            /// </summary>
            public byte[] MByColorBuffer { get; set; }

            ///// <summary>
            ///// 用于与当前的数据格式进行与运算得到当前的数据位数
            ///// </summary>
            //public const uint PixelFormateBit = 0x00FF0000;

            ///// <summary>
            ///// 8位数据图像格式
            ///// </summary>
            //public const uint GxPixel8Bit = 0x00080000;

            public bool IsTriggerExecute { get; set; }
            public int TriggerCount { get; set; }
            public object TriggerLocker = new object();
            public List<byte[]> TriggerImageBytes = new List<byte[]>();
            public AutoResetEvent TriggerWaitEvent = new AutoResetEvent(false);
        }

        ///// <summary>
        ///// 自动白平衡当前的值
        ///// </summary>
        //public string MStrBalanceWhiteAutoValue = "Off";

        private readonly Dictionary<int, string> _mCbEnumDevice = new Dictionary<int, string>();

        /// <summary>
        /// 相机参数状态列表
        /// </summary>
        public static List<CCamerInfo> MListCCamerInfo = new List<CCamerInfo>();

        /// <summary>
        /// Factory对像
        /// </summary>
        private readonly IGXFactory _mObjIgxFactory;

        /// <summary>
        /// 设备信息列表
        /// </summary>
        private readonly List<IGXDeviceInfo> _mListIgxDeviceInfo = new List<IGXDeviceInfo>();

        /// <summary>
        /// 初始化相机数目
        /// </summary>
        private int _mNCamNum;

        public DahengCameraClass()
        {
            _mObjIgxFactory = IGXFactory.GetInstance();
            _mObjIgxFactory.Init();
            EnumDevice();
        }

        public void OpenDevices()
        {
            // 判断当前连接设备个数
            if (_mListIgxDeviceInfo.Count <= 0)
            {
                //MessageBox.Show("未发现设备!");
                return;
            }

            foreach (var t in MListCCamerInfo.Where(t => !t.MbIsOpen))
            {
                CloseStream(ref t.MObjIgxStream);
                CloseDevice(ref t.MObjIgxDevice);

                //打开设备用的序列号
                var strSn = t.MStrSn;

                //打开列表选中的设备
                t.MObjIgxDevice = _mObjIgxFactory.OpenDeviceBySN(strSn, GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE);
                t.MObjIgxFeatureControl = t.MObjIgxDevice.GetRemoteFeatureControl();
                t.MObjIgxStream = t.MObjIgxDevice.OpenStream(0);

                InitParam(ref t.MObjIgxFeatureControl);

                // 更新设备打开标识
                t.MbIsOpen = true;
                //m_objGxBitmap = new GxBitmap(m_objIGXDevice, m_pic_ShowImage);

                try
                {
                    if (t.MObjIgxFeatureControl != null)
                    {
                        if (t.MObjIgxFeatureControl.GetEnumFeature("PixelColorFilter").GetValue() != "None")
                            t.MbIsColor = true;

                        t.MnPayloadSize = (int)t.MObjIgxFeatureControl.GetIntFeature("PayloadSize").GetValue();
                        t.MnWidth = (int)t.MObjIgxFeatureControl.GetIntFeature("Width").GetValue();
                        t.MnHeigh = (int)t.MObjIgxFeatureControl.GetIntFeature("Height").GetValue();

                        t.MObjIgxFeatureControl.GetFloatFeature("ExposureTime").SetValue(10000);
                        t.MObjIgxFeatureControl.GetIntFeature("BlockTimeout").SetValue(200);

                        var objDeviceClass = t.MObjIgxDevice.GetDeviceInfo().GetDeviceClass();
                        if (objDeviceClass == GX_DEVICE_CLASS_LIST.GX_DEVICE_CLASS_GEV)
                        {
                            // 判断设备是否支持流通道数据包功能
                            if (t.MObjIgxFeatureControl.IsImplemented("GevSCPSPacketSize"))
                            {
                                // 获取当前网络环境的最优包长值
                                var nPacketSize = t.MObjIgxStream.GetOptimalPacketSize();
                                // 将最优包长值设置为当前设备的流通道包长值
                                t.MObjIgxFeatureControl.GetIntFeature("GevSCPSPacketSize").SetValue(nPacketSize);
                            }
                        }

                        t.MObjIgxStream.SetAcqusitionBufferNumber(50);

                        t.MhOfflineCallback =
                            t.MObjIgxDevice.RegisterDeviceOfflineCallback(this, OnDeviceOfflineCallbackFun);
                    }

                    //if (t.MObjIgxStream != null)
                    //{
                    //    t.MObjIgxStream.RegisterCaptureCallback(t, CaptureCallbackPro);
                    //    t.MObjIgxStream.StartGrab();
                    //}

                    //if (t.MObjIgxFeatureControl != null)
                    //    t.MObjIgxFeatureControl.GetCommandFeature("AcquisitionStart").Execute();

                    //t.MbIsSnap = true;
                }
                catch (Exception)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private static byte[] ConvertToBytes(CCamerInfo cc, IBaseData objIBaseData)
        {
            byte[] imageBuffer;

            try
            {
                IntPtr pBuffer;

                var width = cc.MnWidth;
                var height = cc.MnHeigh;

                if (cc.MbIsColor)
                {
                    imageBuffer = new byte[width * 3 * height];
                    var emValidBits = GetBestValudBit(objIBaseData.GetPixelFormat());
                    pBuffer = objIBaseData.ConvertToRGB24(
                        emValidBits, GX_BAYER_CONVERT_TYPE_LIST.GX_RAW2RGB_NEIGHBOUR, false);
                    Marshal.Copy(pBuffer, imageBuffer, 0, cc.MnWidth * 3 * cc.MnHeigh);
                }
                else
                {
                    imageBuffer = new byte[width * height];
                    pBuffer = objIBaseData.GetBuffer();
                    Marshal.Copy(pBuffer, imageBuffer, 0, width * height);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return imageBuffer;
        }

        private static void OnDeviceOfflineCallbackFun(object pUserParam)
        {
            var cCamerInfo = pUserParam as CCamerInfo;
            try
            {
                if (cCamerInfo != null)
                    cCamerInfo.MbIsOffLine = true;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void CloseDevices()
        {
            foreach (var t in MListCCamerInfo)
            {
                if (t.MbIsSnap)
                {
                    // 先停止采集
                    if (t.MObjIgxFeatureControl != null)
                    {
                        t.MObjIgxFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                        t.MObjIgxFeatureControl = null;
                    }

                    t.MbIsSnap = false;
                }

                CloseStream(ref t.MObjIgxStream);
                CloseDevice(ref t.MObjIgxDevice);

                t.MbIsOpen = false;
            }
        }

        public CCamerInfo GetDevice(string sn)
        {
            var findCameraIndex = MListCCamerInfo.FindIndex(f => f.MStrSn == sn);
            return findCameraIndex == -1 ? null : MListCCamerInfo[findCameraIndex];
        }

        /// <summary>
        /// 设置快门
        /// </summary>
        public static void SetShutter(string sn, int shutter)
        {
            var findCameraIndex = MListCCamerInfo.FindIndex(f => f.MStrSn == sn);
            if (findCameraIndex == -1)
                return;

            var cc = MListCCamerInfo[findCameraIndex];
            cc.MObjIgxFeatureControl.GetFloatFeature("ExposureTime").SetValue(shutter);
        }

        /// <summary>
        /// 打开帧率模式
        /// </summary>
        /// <param name="sn">相机SN</param>
        /// <param name="frameRate">帧率</param>
        public static void OpenFrameRateMode(string sn, double frameRate)
        {
            SetFrameRateMode(sn, true);
            SetFramgeRate(sn, frameRate);
        }

        /// <summary>
        /// 关闭帧率模式
        /// </summary>
        /// <param name="sn">相机SN</param>
        public static void CloseFrameRateMode(string sn)
        {
            SetFrameRateMode(sn, false);
        }

        /// <summary>
        /// 采集帧率调节模式
        /// </summary>
        private static void SetFrameRateMode(string sn, bool isOpen)
        {
            var findCameraIndex = MListCCamerInfo.FindIndex(f => f.MStrSn == sn);
            if (findCameraIndex == -1)
                return;

            var cc = MListCCamerInfo[findCameraIndex];
            cc.MObjIgxFeatureControl.GetEnumFeature("AcquisitionFrameRateMode").SetValue(isOpen ? "On" : "Off");
        }

        /// <summary>
        /// 设置采集帧率
        /// </summary>

        private static void SetFramgeRate(string sn, double frameRate)
        {
            var findCameraIndex = MListCCamerInfo.FindIndex(f => f.MStrSn == sn);
            if (findCameraIndex == -1)
                return;

            var cc = MListCCamerInfo[findCameraIndex];
            cc.MObjIgxFeatureControl.GetFloatFeature("AcquisitionFrameRate").SetValue(frameRate);
        }

        //public byte[] TriggerSoftware(string sn)
        //{
        //    var findCameraIndex = MListCCamerInfo.FindIndex(f => f.MStrSn == sn);
        //    if (findCameraIndex == -1)
        //        return null;

        //    IImageData objIImageData = null;
        //    const uint nTimeout = 3000;

        //    // 每次发送触发命令之前清空采集输出队列
        //    // 防止库内部缓存帧，造成本次GXGetImage得到的图像是上次发送触发得到的图
        //    if (MListCCamerInfo[findCameraIndex].MObjIgxStream != null)
        //        MListCCamerInfo[findCameraIndex].MObjIgxStream.FlushQueue();

        //    // 发送软触发命令
        //    if (MListCCamerInfo[findCameraIndex].MObjIgxFeatureControl != null)
        //        MListCCamerInfo[findCameraIndex].MObjIgxFeatureControl.GetCommandFeature("TriggerSoftware").Execute();

        //    // 获取图像
        //    if (MListCCamerInfo[findCameraIndex].MObjIgxStream != null)
        //        objIImageData = MListCCamerInfo[findCameraIndex].MObjIgxStream.GetImage(nTimeout);

        //    // 获得图像原始数据大小、宽度、高度等
        //    var mObjIgxFeatureControl = MListCCamerInfo[findCameraIndex].MObjIgxFeatureControl;
        //    if (mObjIgxFeatureControl != null)
        //    {
        //        MListCCamerInfo[findCameraIndex].MnPayloadSize = (int)mObjIgxFeatureControl.GetIntFeature("PayloadSize").GetValue();

        //        MListCCamerInfo[findCameraIndex].MnWidth = (int)mObjIgxFeatureControl.GetIntFeature("Width").GetValue();
        //        MListCCamerInfo[findCameraIndex].MnHeigh = (int)mObjIgxFeatureControl.GetIntFeature("Height").GetValue();
        //    }

        //    // 获取是否为彩色相机
        //    var objIgxFeatureControl = MListCCamerInfo[findCameraIndex].MObjIgxFeatureControl;
        //    if (objIgxFeatureControl != null && objIgxFeatureControl.IsImplemented("PixelColorFilter"))
        //    {
        //        var igxFeatureControl = MListCCamerInfo[findCameraIndex].MObjIgxFeatureControl;
        //        if (igxFeatureControl != null)
        //        {
        //            var strValue = igxFeatureControl.GetEnumFeature("PixelColorFilter").GetValue();

        //            if (strValue != "None")
        //                MListCCamerInfo[findCameraIndex].MbIsColor = true;
        //        }
        //    }

        //    byte[] imageBuffer;

        //    try
        //    {
        //        IntPtr pBuffer;
        //        if (MListCCamerInfo[findCameraIndex].MbIsColor)
        //        {
        //            imageBuffer = new byte[MListCCamerInfo[findCameraIndex].MnWidth * 3 * MListCCamerInfo[findCameraIndex].MnHeigh];
        //            var emValidBits = GetBestValudBit(objIImageData.GetPixelFormat());
        //            pBuffer = objIImageData.ConvertToRGB24(emValidBits, GX_BAYER_CONVERT_TYPE_LIST.GX_RAW2RGB_NEIGHBOUR, false);
        //            Marshal.Copy(pBuffer, imageBuffer, 0, MListCCamerInfo[findCameraIndex].MnWidth * 3 * MListCCamerInfo[findCameraIndex].MnHeigh);
        //        }
        //        else
        //        {
        //            imageBuffer = new byte[MListCCamerInfo[findCameraIndex].MnWidth * MListCCamerInfo[findCameraIndex].MnHeigh];
        //            pBuffer = objIImageData.GetBuffer();
        //            Marshal.Copy(pBuffer, imageBuffer, 0, MListCCamerInfo[findCameraIndex].MnWidth * MListCCamerInfo[findCameraIndex].MnHeigh);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        imageBuffer = null;
        //    }

        //    if (null != objIImageData)
        //        //用完之后释放资源
        //        objIImageData.Destroy();

        //    return imageBuffer;
        //}

        /// <summary>
        /// 软触发模式
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="triggerCount"></param>
        /// <param name="triggerByteses"></param>
        /// <param name="outTime"></param>
        /// <returns></returns>
        public bool TriggerSoftware(
            string sn, int triggerCount, out List<byte[]> triggerByteses, int outTime = 10000)
        {
            var findCameraIndex = MListCCamerInfo.FindIndex(f => f.MStrSn == sn);
            if (findCameraIndex == -1)
            {
                triggerByteses = new List<byte[]>();
                return false;
            }

            var cc = MListCCamerInfo[findCameraIndex];

            // 开始采集
            if (cc.MObjIgxStream != null)
            {
                cc.MObjIgxStream.RegisterCaptureCallback(cc, CaptureCallbackPro);
                cc.MObjIgxStream.StartGrab();
            }
            if (cc.MObjIgxFeatureControl != null)
                cc.MObjIgxFeatureControl.GetCommandFeature("AcquisitionStart").Execute();

            cc.MbIsSnap = true;

            lock (cc.TriggerLocker)
            {
                cc.TriggerImageBytes.Clear();
                cc.TriggerCount = triggerCount;
                cc.IsTriggerExecute = true;
            }

            var isSucceed = cc.TriggerWaitEvent.WaitOne(outTime);
            if (!isSucceed || cc.TriggerImageBytes == null)
            {
                cc.MbIsSnap = false;

                // 停止采集
                if (cc.MObjIgxFeatureControl != null)
                    cc.MObjIgxFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                if (cc.MObjIgxStream != null)
                {
                    cc.MObjIgxStream.StopGrab();
                    cc.MObjIgxStream.UnregisterCaptureCallback();
                }

                triggerByteses = new List<byte[]>();
                return false;
            }

            lock (cc.TriggerLocker)
            {
                triggerByteses = new List<byte[]>();
                triggerByteses.AddRange(cc.TriggerImageBytes.ToList());
                cc.TriggerImageBytes.Clear();
            }

            // 停止采集
            if (cc.MObjIgxFeatureControl != null)
                cc.MObjIgxFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
            if (cc.MObjIgxStream != null)
            {
                cc.MObjIgxStream.StopGrab();
                cc.MObjIgxStream.UnregisterCaptureCallback();
            }

            cc.MbIsSnap = false;
            return true;
        }

        public void AcquisitionStart(string sn)
        {
            var findCameraIndex = MListCCamerInfo.FindIndex(f => f.MStrSn == sn);
            if (findCameraIndex == -1)
                return;

            var cc = MListCCamerInfo[findCameraIndex];

            if (cc.MbIsSnap)
                return;

            // 开始采集
            if (cc.MObjIgxStream != null)
            {
                cc.MObjIgxStream.RegisterCaptureCallback(cc, CaptureCallbackPro);
                cc.MObjIgxStream.StartGrab();
            }
            if (cc.MObjIgxFeatureControl != null)
                cc.MObjIgxFeatureControl.GetCommandFeature("AcquisitionStart").Execute();

            cc.MbIsSnap = true;
        }

        public void AcquisitionStop(string sn)
        {
            var findCameraIndex = MListCCamerInfo.FindIndex(f => f.MStrSn == sn);
            if (findCameraIndex == -1)
                return;

            var cc = MListCCamerInfo[findCameraIndex];

            if (!cc.MbIsSnap)
                return;

            // 停止采集
            if (cc.MObjIgxFeatureControl != null)
                cc.MObjIgxFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
            if (cc.MObjIgxStream != null)
            {
                cc.MObjIgxStream.StopGrab();
                cc.MObjIgxStream.UnregisterCaptureCallback();
            }

            cc.MbIsSnap = false;
        }

        private static void CaptureCallbackPro(object objUserParam, IFrameData objIFrameData)
        {
            try
            {
                var cc = objUserParam as CCamerInfo;

                if (cc == null)
                    return;

                if (!cc.MbIsSnap)
                    return;

                var bs = ConvertToBytes(cc, objIFrameData);
                if (PushImageBytes != null)
                    PushImageBytes(ConvertToBytes(cc, objIFrameData), cc.MnWidth, cc.MnHeigh, cc.MbIsColor);

                lock (cc.TriggerLocker)
                {
                    if (!cc.IsTriggerExecute)
                        return;

                    if (cc.TriggerImageBytes.Count == cc.TriggerCount)
                    {
                        cc.IsTriggerExecute = false;
                        cc.TriggerCount = 0;
                        cc.TriggerWaitEvent.Set();

                        return;
                    }
                    cc.TriggerImageBytes.Add(bs);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 枚举设备
        /// </summary>
        private async void EnumDevice()
        {
            await Task.Run(() =>
            {
                _mListIgxDeviceInfo.Clear();
                if (_mObjIgxFactory != null)
                {
                    _mObjIgxFactory.UpdateDeviceList(200, _mListIgxDeviceInfo);
                }

                // 判断当前连接设备个数
                if (_mListIgxDeviceInfo.Count <= 0)
                {
                    //MessageBox.Show("未检测到设备,请确保设备正常连接然后重启程序!");
                    return;
                }

                _mNCamNum = GetMin(_mListIgxDeviceInfo.Count);
                _mCbEnumDevice.Clear();

                for (var i = 0; i < _mNCamNum; i++)
                {
                    _mCbEnumDevice.Add(i, _mListIgxDeviceInfo[i].GetDisplayName());
                    var objCCamerInfo = new CCamerInfo
                    {
                        MStrDisplayName = _mListIgxDeviceInfo[i].GetDisplayName(),
                        MStrSn = _mListIgxDeviceInfo[i].GetSN(),
                        MEmDeviceType = _mListIgxDeviceInfo[i].GetDeviceClass()
                    };
                    var findIndex = MListCCamerInfo.FindIndex(f => f.MStrSn == objCCamerInfo.MStrSn);
                    if (findIndex == -1)
                        MListCCamerInfo.Add(objCCamerInfo);
                }
            });
        }

        /// <summary>
        /// 当枚举的相机个数超过4个，按4个显示，小于等于4个按实际值显示
        /// 当枚举的相机个数小于0按0返回
        /// </summary>
        /// <param name="nDeviceCount">实际枚举到的个数</param>
        /// <returns>显示的最小个数</returns>
        private static int GetMin(int nDeviceCount)
        {
            if (nDeviceCount < 0)
                return 0;

            return nDeviceCount;
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        private static void InitParam(ref IGXFeatureControl c)
        {
            if (c == null)
                return;

            //设置采集模式连续采集
            c.GetEnumFeature("AcquisitionMode").SetValue("Continuous");

            //设置触发模式为关
            c.GetEnumFeature("TriggerMode").SetValue("Off");

            // 选择触发源为软触发
            c.GetEnumFeature("TriggerSource").SetValue("Software");

            if (c.IsImplemented("PixelColorFilter"))
            {
                var strValue = c.GetEnumFeature("PixelColorFilter").GetValue();

                if (strValue != "None")
                {
                    c.GetEnumFeature("BalanceWhiteAuto").SetValue("Off");
                    c.GetEnumFeature("BalanceRatioSelector").SetValue("Red");
                    c.GetFloatFeature("BalanceRatio").SetValue(1);
                    c.GetEnumFeature("BalanceRatioSelector").SetValue("Green");
                    c.GetFloatFeature("BalanceRatio").SetValue(1);
                    c.GetEnumFeature("BalanceRatioSelector").SetValue("Blue");
                    c.GetFloatFeature("BalanceRatio").SetValue(1);
                }
            }

            //设置图像区域为默认最大值
            var widthMax = c.GetIntFeature("WidthMax").GetValue();
            var heightMax = c.GetIntFeature("HeightMax").GetValue();
            c.GetIntFeature("OffsetX").SetValue(0);
            c.GetIntFeature("OffsetY").SetValue(0);
            c.GetIntFeature("Width").SetValue(widthMax);
            c.GetIntFeature("Height").SetValue(heightMax);
        }

        private static void UpdateBitmap(CCamerInfo c)
        {
            //给BitmapData加锁
            var bmpData = c.MBitmapForSave.LockBits(new Rectangle(0, 0, c.MnWidth, c.MnHeigh), ImageLockMode.ReadWrite,
                c.MBitmapForSave.PixelFormat);

            //得到一个指向Bitmap的buffer指针
            var ptrBmp = bmpData.Scan0;
            int nImageStride;
            if (c.MbIsColor)
                nImageStride = c.MnWidth * 3;
            else
                nImageStride = c.MnWidth;
            //图像宽能够被4整除直接copy
            if (nImageStride == bmpData.Stride)
                Marshal.Copy(c.MbIsColor ? c.MByColorBuffer : c.MByMonoBuffer, 0, ptrBmp,
                    bmpData.Stride * c.MBitmapForSave.Height);
            else//图像宽不能够被4整除按照行copy
            {
                for (var i = 0; i < c.MBitmapForSave.Height; ++i)
                    Marshal.Copy(c.MbIsColor ? c.MByColorBuffer : c.MByMonoBuffer, i * nImageStride,
                        new IntPtr(ptrBmp.ToInt64() + i * bmpData.Stride), c.MnWidth);
            }
            //BitmapData解锁
            c.MBitmapForSave.UnlockBits(bmpData);
        }

        /// <summary>
        /// 判断PixelFormat是否为8位
        /// </summary>
        /// <param name="emPixelFormatEntry">图像数据格式</param>
        /// <returns>true为8为数据，false为非8位数据</returns>
        private static bool IsPixelFormat8(GX_PIXEL_FORMAT_ENTRY emPixelFormatEntry)
        {
            var bIsPixelFormat8 = false;
            var uiPixelFormatEntry = (uint)emPixelFormatEntry;
            if ((uiPixelFormatEntry & 0x00FF0000) == 0x00080000)
                bIsPixelFormat8 = true;
            return bIsPixelFormat8;
        }

        /// <summary>
        /// 通过GX_PIXEL_FORMAT_ENTRY获取最优Bit位
        /// </summary>
        /// <param name="emPixelFormatEntry">图像数据格式</param>
        /// <returns>最优Bit位</returns>
        private static GX_VALID_BIT_LIST GetBestValudBit(GX_PIXEL_FORMAT_ENTRY emPixelFormatEntry)
        {
            var emValidBits = GX_VALID_BIT_LIST.GX_BIT_0_7;

            switch (emPixelFormatEntry)
            {
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO8:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR8:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG8:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB8:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG8:
                    {
                        emValidBits = GX_VALID_BIT_LIST.GX_BIT_0_7;
                        break;
                    }

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO10:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR10:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG10:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB10:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG10:
                    {
                        emValidBits = GX_VALID_BIT_LIST.GX_BIT_2_9;
                        break;
                    }

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO12:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR12:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG12:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB12:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG12:
                    {
                        emValidBits = GX_VALID_BIT_LIST.GX_BIT_4_11;
                        break;
                    }

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO14:
                    {
                        //暂时没有这样的数据格式待升级
                        break;
                    }

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO16:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR16:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG16:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB16:

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG16:
                    {
                        //暂时没有这样的数据格式待升级
                        break;
                    }

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_RGB16_PLANAR:
                    break;

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_RGB12_PLANAR:
                    break;

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_RGB10_PLANAR:
                    break;

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_RGB8_PLANAR:
                    break;

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO8_SIGNED:
                    break;

                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_UNDEFINED:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("emPixelFormatEntry", emPixelFormatEntry, null);
            }

            return emValidBits;
        }

        /// <summary>
        /// 关闭流
        /// </summary>
        /// <param name="igxStream"></param>
        private static void CloseStream(ref IGXStream igxStream)
        {
            try
            {
                //关闭流
                if (null == igxStream) return;
                igxStream.Close();
                igxStream.UnregisterCaptureCallback();
                igxStream = null;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <param name="igxDevice"></param>
        private static void CloseDevice(ref IGXDevice igxDevice)
        {
            try
            {
                //关闭设备
                if (null == igxDevice)
                    return;

                igxDevice.Close();
                igxDevice = null;
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
