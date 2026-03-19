using NationalInstruments.Vision;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace CommonUtility.HikSdk
{
    public sealed class MyCamera : IDisposable
    {
        public delegate void PushMatEventHandle(
            MvCameraSdk.MV_GIGE_DEVICE_INFO gigeInfo, Mat mat);
        public static event PushMatEventHandle PushMat;
        public bool IsGray { get; set; }
        private void OnPushMat(Mat mat)
        {
            if (PushMat != null)
                PushMat(GigeInfo, mat);
            else
                mat.Dispose();
        }

        public bool IsUseCustomSize = true;
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
        private bool _disposed = false; // 添加资源释放标志
        private uint _captureCount;
        private int _captureRow = -1;
        private int _captureCol = -1;
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);
        public string[,] MatBuffer = new string[10, 200];
        public int[,] MatWidth = new int[10, 200];
        public int[,] MatHeight = new int[10, 200];
        public uint PayloadSize;
        public uint PayloadWidth;
        public uint PayloadHeight;

        private uint _roiOffsetXStep = 2;
        private uint _payloadMinWidth = 32;
        private uint _roiOffsetYStep = 2;
        private uint _payloadMinHeight = 32;

        private uint _buffSizeForSaveImage = 3072 * 2048 * (16 * 3 + 4) + 2048;
        private byte[] _bufForSaveImage;
        private string _blackImgBase64 = string.Empty;

        public MyCamera(IntPtr pUser, MvCameraSdk.MV_GIGE_DEVICE_INFO gigeInfo)
        {
            _bufForSaveImage = new byte[_buffSizeForSaveImage];

            PUser = pUser;
            CameraIndex = (int)pUser;
            GigeInfo = gigeInfo;
            _cbImage = ImageCallBack; //ImageCallBack;
        }

        /// <summary>
        /// 实现IDisposable接口
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///  析构函数 - 作为最后的保障确保非托管资源被释放
        /// </summary>
        ~MyCamera() => Dispose(false);

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                try
                {
                    // 先停止相机操作
                    if (_isGrabbing)
                    {
                        try
                        {
                            StopGrab();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"停止采集时发生异常: {ex.Message}");
                        }
                    }

                    // 关闭相机
                    if (_isOpened)
                    {
                        try
                        {
                            CloseCamera();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"关闭相机时发生异常: {ex.Message}");
                        }
                    }

                    if (disposing)
                    {
                        // 释放托管资源
                        _waitHandle?.Dispose();

                        // 清理缓冲区
                        ClearBuffer();

                        // 清理Mat位置列表
                        if (_matPosList != null)
                        {
                            _matPosList.Clear();
                        }
                    }

                    // 释放非托管资源
                    ReleaseUnmanagedResources();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"释放资源时发生异常: {ex.Message}");
                }
                finally
                {
                    _disposed = true;
                }
            }
        }

        private void ReleaseUnmanagedResources()
        {
            try
            {
                // 释放图像缓冲区
                if (_grabBuff != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_grabBuff);
                    _grabBuff = IntPtr.Zero;
                    Console.WriteLine("释放图像缓冲区成功");
                }

                // 释放保存图像缓冲区
                if (_mPSaveImageBuf != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_mPSaveImageBuf);
                    _mPSaveImageBuf = IntPtr.Zero;
                    Console.WriteLine("释放保存图像缓冲区成功");
                }

                // 释放捕获缓冲区
                if (_listCaptureInptr != null)
                {
                    for (var i = 0; i < _listCaptureInptr.Count; i++)
                    {
                        if (_listCaptureInptr[i] != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(_listCaptureInptr[i]);
                            _listCaptureInptr[i] = IntPtr.Zero;
                        }
                    }
                    _listCaptureInptr.Clear();
                    Console.WriteLine($"释放{_listCaptureInptr.Count}个捕获缓冲区成功");
                }

                // 释放SDK资源
                if (_mvCameraSdk != null)
                {
                    try
                    {
                        // 确保设备已关闭
                        if (_isOpened)
                        {
                            _mvCameraSdk.MV_CC_DestroyDevice_NET();
                            _isOpened = false;
                            Console.WriteLine("销毁相机设备句柄成功");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"销毁相机设备句柄时发生异常: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常但不抛出，避免在Dispose中抛出异常
                Console.WriteLine($"释放非托管资源时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查对象是否已释放
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MyCamera), "相机对象已被释放，不能继续使用");
        }

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        //private void ProcessAndSaveImage(IntPtr pData, MvCameraSdk.MV_FRAME_OUT_INFO_EX pFrameInfo, ulong ts)
        //{
        //    try
        //    {
        //        #region 分步骤转换
        //        //var independentMat = new Mat(pFrameInfo.nHeight, pFrameInfo.nWidth, MatType.CV_8UC1, pData); // OPENCV直接copy内存
        //        var independentMat = new Mat(pFrameInfo.nHeight, pFrameInfo.nWidth, MatType.CV_8UC1); // 先实例化mat
        //        CopyMemory(independentMat.Data, pData, pFrameInfo.nFrameLen); // 再copy内存

        //        if (pFrameInfo.enPixelType == MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerRG8)
        //            Cv2.CvtColor(independentMat, independentMat, ColorConversionCodes.BayerRG2RGB);
        //        else if (pFrameInfo.enPixelType == MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGR8)
        //            Cv2.CvtColor(independentMat, independentMat, ColorConversionCodes.BayerGR2RGB);
        //        else
        //            Cv2.CvtColor(independentMat, independentMat, ColorConversionCodes.BayerRG2BGR);

        //        #endregion

        //        // 设置字体、文本内容、字体大小和颜色
        //        const HersheyFonts fontFace = HersheyFonts.HersheySimplex;
        //        const double fontScale = 0.45;
        //        int baseline;
        //        var showTxt = string.Format("{0}/[{1},{2}],DevTS={3},HostTS={4}", pFrameInfo.nFrameNum, _captureRow, _captureCol, ts, pFrameInfo.nHostTimeStamp);
        //        // 获取文字的尺寸
        //        var textSize = Cv2.GetTextSize(showTxt, fontFace, fontScale, 1, out baseline);

        //        var frameIndex = pFrameInfo.nFrameNum;
        //        //Cv2.PutText(matImage, showTxt, new Point(1, 15), fontFace, fontScale, Scalar.WhiteSmoke);
        //        //Cv2.PutText(independentMat, showTxt, new Point(1, pFrameInfo.nHeight - textSize.Height), fontFace, fontScale, Scalar.WhiteSmoke);
        //        //Console.WriteLine(showTxt);

        //        if (_isGrabbing && !_isCapturing && _isPushMat)
        //        {
        //            //Cv2.ImWrite(string.Format(@"E:\测试grab图像\GrabImg{0}_{1}.jpg", _grabIndex.ToString().PadLeft(8, '0'),
        //            //    HighPrecisionTimer.GetTimestamp()), independentMat);
        //            //_grabIndex++;
        //            var toPushMat = independentMat.Clone();
        //            OnPushMat(toPushMat);
        //        }

        //        if (_isCapturing && _captureCount > 0)
        //        {
        //            lock (_lockGetBuff)
        //            {
        //                MatBuffer[_captureRow, _captureCol] = independentMat.Clone();
        //                _matPosList.Add(new MatPos { Row = _captureRow, Col = _captureCol, TimeStamp = ts, FrameIndex = (int)frameIndex, PixelType = pFrameInfo.enPixelType });
        //            }

        //            _captureCol++;
        //            if (_captureCol == MatBuffer.GetLength(1))
        //            {
        //                _captureCol = 0;
        //                _captureRow++;
        //            }

        //            _captureCount--;
        //            if (_listCaptureInptr.Any() && _targetGetCount >= 0 && _listCaptureInptr.Count > _targetGetCount)
        //            {
        //                Marshal.FreeHGlobal(_listCaptureInptr[_targetGetCount]);
        //                _listCaptureInptr[_targetGetCount] = IntPtr.Zero;
        //            }
        //            _targetGetCount++;

        //            //Console.WriteLine("grab callback cost: " + _st.ElapsedMilliseconds);                   

        //            if (_captureCount == 0)
        //            {
        //                //_st.Stop();
        //                //Console.WriteLine("grab cost: " + _st.ElapsedMilliseconds);

        //                _waitHandle.Set();
        //            }
        //        }

        //        independentMat.Dispose();
        //        independentMat = null;
        //        //Marshal.FreeHGlobal(pData);
        //        //GC.Collect();
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }
        //}

        private void ProcessAndSaveImage(IntPtr pData, MvCameraSdk.MV_FRAME_OUT_INFO_EX pFrameInfo, ulong ts)
        {
            try
            {
                // 参数验证
                if (pData == IntPtr.Zero)
                    return;

                // 性能监控 - 仅在需要时记录时间戳
                var processStart = HighPrecisionTimer.GetTimestamp();

                // 解析图像数据
                Mat independentMat = null;
                string independentBase64 = null;

                try
                {
                    independentMat = RunFuncTask(() => { return ParseRawImageDataToMat(pData, pFrameInfo); }).Result;
                    if (independentMat == null || independentMat.Empty())
                    {
                        Console.WriteLine("图像解析失败，返回空图像");
                        return;
                    }

                    // 转换为Base64字符串
                    independentBase64 = MatToBase64String(independentMat);
                    if (string.IsNullOrEmpty(independentBase64))
                    {
                        Console.WriteLine("图像Base64转换失败");
                        return;
                    }

                    independentMat.Dispose();
                    independentMat = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"图像处理异常: {ex.Message}");
                    return;
                }

                var frameIndex = pFrameInfo.nFrameNum;

                // 处理预览模式下的图像推送
                if (_isGrabbing && !_isCapturing && _isPushMat)
                {
                    try
                    {
                        var matForPush = Base64StringToMat(independentBase64);
                        if (matForPush != null && !matForPush.Empty())
                        {
                            OnPushMat(matForPush);
                            matForPush.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"图像推送异常: {ex.Message}");
                    }
                }

                // 处理采集模式下的图像保存
                if (_isCapturing && _captureCount > 0)
                {
                    lock (_lockGetBuff)
                    {
                        try
                        {
                            // 检查缓冲区边界
                            if (_captureRow >= MatBuffer.GetLength(0) || _captureCol >= MatBuffer.GetLength(1))
                            {
                                Console.WriteLine($"图像缓冲区越界: Row={_captureRow}, Col={_captureCol}");
                                return;
                            }

                            // 保存图像信息
                            MatBuffer[_captureRow, _captureCol] = independentBase64;
                            MatWidth[_captureRow, _captureCol] = pFrameInfo.nWidth;
                            MatHeight[_captureRow, _captureCol] = pFrameInfo.nHeight;
                            _matPosList.Add(new MatPos
                            {
                                Row = _captureRow,
                                Col = _captureCol,
                                TimeStamp = ts,
                                FrameIndex = (int)frameIndex,
                                PixelType = pFrameInfo.enPixelType
                            });

                            // 更新缓冲区位置
                            _captureCol++;
                            if (_captureCol >= MatBuffer.GetLength(1))
                            {
                                _captureCol = 0;
                                _captureRow++;
                            }

                            _captureCount--;

                            // 释放已处理的内存缓冲区
                            if (_listCaptureInptr.Any() && _targetGetCount >= 0 &&
                                _targetGetCount < _listCaptureInptr.Count &&
                                _listCaptureInptr[_targetGetCount] != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(_listCaptureInptr[_targetGetCount]);
                                _listCaptureInptr[_targetGetCount] = IntPtr.Zero;
                            }
                            _targetGetCount++;

                            // 检查是否完成采集
                            if (_captureCount == 0)
                            {
                                _waitHandle.Set();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"图像保存异常: {ex.Message}");
                        }
                    }
                }

                // 性能监控 - 输出处理时间
                var processEnd = HighPrecisionTimer.GetTimestamp();
                var processTime = HighPrecisionTimer.GetTimestampIntervalMs(processStart, processEnd);
                if (processTime > 50) // 仅当处理时间较长时输出警告
                {
                    Console.WriteLine($"图像处理耗时: {processTime}ms");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ProcessAndSaveImage异常: {ex.Message}");
            }
        }

        private Mat ParseRawImageDataToMat(IntPtr pData, MvCameraSdk.MV_FRAME_OUT_INFO_EX stFrameInfo)
        {
            try
            {
                // 参数验证
                if (pData == IntPtr.Zero)
                    return null;

                // 确定目标像素类型
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
                    Console.WriteLine($"不支持的像素类型: {stFrameInfo.enPixelType}");
                    return null;
                }

                // 确保转换缓冲区足够大
                if (_bufForSaveImage == null || _bufForSaveImage.Length < _buffSizeForSaveImage)
                {
                    _bufForSaveImage = new byte[_buffSizeForSaveImage];
                }

                IntPtr pImage = Marshal.UnsafeAddrOfPinnedArrayElement(_bufForSaveImage, 0);

                // 设置像素转换参数
                var stConverPixelParam = new MvCameraSdk.MV_PIXEL_CONVERT_PARAM
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

                // 执行像素类型转换
                int nRet = _mvCameraSdk.MV_CC_ConvertPixelType_NET(ref stConverPixelParam);
                if (MvCameraSdk.MV_OK != nRet)
                {
                    Console.WriteLine($"像素类型转换失败，错误码: {nRet}");
                    return null;
                }

                // 创建Mat对象
                Mat mat;
                if (enDstPixelType == MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Mono8)
                {
                    // 单通道图像
                    mat = new Mat(stFrameInfo.nHeight, stFrameInfo.nWidth, MatType.CV_8UC1, pImage);
                }
                else
                {
                    // RGB图像，需要交换B和R通道
                    unsafe
                    {
                        byte* ptr = (byte*)pImage.ToPointer();
                        int totalPixels = stFrameInfo.nWidth * stFrameInfo.nHeight;

                        for (int i = 0; i < totalPixels; i++)
                        {
                            int idx = i * 3;
                            byte temp = ptr[idx];
                            ptr[idx] = ptr[idx + 2];
                            ptr[idx + 2] = temp;
                        }
                    }

                    mat = new Mat(stFrameInfo.nHeight, stFrameInfo.nWidth, MatType.CV_8UC3, pImage);
                }

                // 创建Mat的副本，避免依赖原始缓冲区
                Mat result = mat.Clone();

                if (mat.Width > 2500 || mat.Height > 2500)
                {
                    var scaleX = (double)1920 / mat.Width;
                    var scaleY = (double)1080 / mat.Height;
                    var scale = scaleX >= scaleY ? scaleX : scaleY;
                    result = result.Resize(new Size(), scale, scale);
                }

                mat.Dispose(); // 释放原始Mat，但不释放缓冲区

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ParseRawImageDataToMat异常: {ex.Message}");
                return null;
            }
        }

        private Bitmap ParseRawImageDatacallback(IntPtr pData, MvCameraSdk.MV_FRAME_OUT_INFO_EX stFrameInfo)
        {
            try
            {
                // 参数验证
                if (pData == IntPtr.Zero)
                    return null;

                Bitmap output = null;

                // 确定目标像素类型
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
                    Console.WriteLine($"不支持的像素类型: {stFrameInfo.enPixelType}");
                    return null;
                }

                // 确保转换缓冲区足够大
                if (_bufForSaveImage == null || _bufForSaveImage.Length < _buffSizeForSaveImage)
                {
                    _bufForSaveImage = new byte[_buffSizeForSaveImage];
                }

                IntPtr pImage = Marshal.UnsafeAddrOfPinnedArrayElement(_bufForSaveImage, 0);

                var stConverPixelParam = new MvCameraSdk.MV_PIXEL_CONVERT_PARAM
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

                // 执行像素类型转换
                int nRet = _mvCameraSdk.MV_CC_ConvertPixelType_NET(ref stConverPixelParam);
                if (MvCameraSdk.MV_OK != nRet)
                {
                    Console.WriteLine($"像素类型转换失败，错误码: {nRet}");
                    return null;
                }

                // 根据像素类型创建Bitmap
                if (enDstPixelType == MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Mono8)
                {
                    // Mono8 转 Bitmap
                    output = new Bitmap(stFrameInfo.nWidth, stFrameInfo.nHeight, stFrameInfo.nWidth * 1,
                        PixelFormat.Format8bppIndexed, pImage);

                    // 设置灰度调色板
                    ColorPalette cp = output.Palette;
                    for (int i = 0; i < 256; i++)
                    {
                        cp.Entries[i] = Color.FromArgb(i, i, i);
                    }
                    output.Palette = cp;
                }
                else
                {
                    // RGB8 转 Bitmap - 交换R和B通道
                    unsafe
                    {
                        byte* ptr = (byte*)pImage.ToPointer();
                        int totalPixels = stFrameInfo.nWidth * stFrameInfo.nHeight;

                        for (int i = 0; i < totalPixels; i++)
                        {
                            int idx = i * 3;
                            byte temp = ptr[idx];
                            ptr[idx] = ptr[idx + 2];
                            ptr[idx + 2] = temp;
                        }
                    }

                    output = new Bitmap(stFrameInfo.nWidth, stFrameInfo.nHeight, stFrameInfo.nWidth * 3,
                        PixelFormat.Format24bppRgb, pImage);
                }

                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ParseRawImageDatacallback异常: {ex.Message}");
                return null;
            }
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
            ThrowIfDisposed();

            if (PUser == IntPtr.Zero)
                throw new InvalidOperationException("设备指针为空，无法打开相机");

            if (_isOpened)
                return;

            // 创建设备前先清理可能存在的资源
            CleanupCameraResources();

            var deviceName = string.Empty;
            MvCameraSdk.MV_CC_DEVICE_INFO device = default;

            try
            {
                // 获取设备信息
                device = (MvCameraSdk.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(PUser, typeof(MvCameraSdk.MV_CC_DEVICE_INFO));

                if (device.nTLayerType == MvCameraSdk.MV_GIGE_DEVICE)
                {
                    var gigeInfo = (MvCameraSdk.MV_GIGE_DEVICE_INFO)MvCameraSdk.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MvCameraSdk.MV_GIGE_DEVICE_INFO));
                    deviceName = !string.IsNullOrEmpty(gigeInfo.chUserDefinedName)
                        ? $"GEV: {gigeInfo.chUserDefinedName} ({gigeInfo.chSerialNumber})"
                        : $"GEV: {gigeInfo.chManufacturerName} {gigeInfo.chModelName} ({gigeInfo.chSerialNumber})";
                }
                else if (device.nTLayerType == MvCameraSdk.MV_USB_DEVICE)
                {
                    var usbInfo = (MvCameraSdk.MV_USB3_DEVICE_INFO)MvCameraSdk.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MvCameraSdk.MV_USB3_DEVICE_INFO));
                    deviceName = !string.IsNullOrEmpty(usbInfo.chUserDefinedName)
                        ? $"U3V: {usbInfo.chUserDefinedName} ({usbInfo.chSerialNumber})"
                        : $"U3V: {usbInfo.chManufacturerName} {usbInfo.chModelName} ({usbInfo.chSerialNumber})";
                }

                // 创建设备
                var nRet = _mvCameraSdk.MV_CC_CreateDevice_NET(ref device);
                if (nRet != MvCameraSdk.MV_OK)
                    throw new InvalidOperationException($"创建设备失败，错误代码: 0x{nRet:X}");

                // 打开设备
                nRet = _mvCameraSdk.MV_CC_OpenDevice_NET();
                if (nRet != MvCameraSdk.MV_OK)
                    throw new InvalidOperationException($"打开设备[{deviceName}]失败，错误代码: 0x{nRet:X}");

                Console.WriteLine($"打开设备[{deviceName}]成功!");
                _mPDeviceInfo = device;

                // 获取相机参数
                if (!GetCameraParameters())
                    throw new InvalidOperationException("获取相机参数失败");

                // 分配图像缓冲区
                AllocateImageBuffer();

                // 设置相机参数
                ConfigureCameraSettings(device);

                // 注册图像回调
                RegisterImageCallback();

                // 创建黑色图像Base64
                CreateBlackImageBase64();

                _isOpened = true;
            }
            catch (Exception ex)
            {
                // 清理资源
                CleanupCameraResources();
                _isOpened = false;

                throw new InvalidOperationException($"打开相机失败: {ex.Message}", ex);
            }
        }

        private void CleanupCameraResources()
        {
            try
            {
                // 释放图像缓冲区
                if (_grabBuff != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_grabBuff);
                    _grabBuff = IntPtr.Zero;
                }

                // 关闭和销毁设备
                if (_mvCameraSdk != null)
                {
                    try
                    {
                        _mvCameraSdk.MV_CC_CloseDevice_NET();
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        _mvCameraSdk.MV_CC_DestroyDevice_NET();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"清理相机资源时发生异常: {ex.Message}");
            }
        }

        private bool GetCameraParameters()
        {
            try
            {
                var stParam = new MvCameraSdk.MVCC_INTVALUE();

                // 获取传感器尺寸
                var stParamRetPayloadWidth = _mvCameraSdk.MV_CC_GetIntValue_NET("SensorWidth", ref stParam);
                if (stParamRetPayloadWidth != MvCameraSdk.MV_OK)
                {
                    Console.WriteLine($"获取传感器宽度失败，错误代码: 0x{stParamRetPayloadWidth:X}");
                    return false;
                }
                PayloadWidth = stParam.nCurValue;

                var stParamRetPayloadHeight = _mvCameraSdk.MV_CC_GetIntValue_NET("SensorHeight", ref stParam);
                if (stParamRetPayloadHeight != MvCameraSdk.MV_OK)
                {
                    Console.WriteLine($"获取传感器高度失败，错误代码: 0x{stParamRetPayloadHeight:X}");
                    return false;
                }
                PayloadHeight = stParam.nCurValue;

                // 获取ROI参数
                var stParamOffsetXRet = _mvCameraSdk.MV_CC_GetIntValue_NET("Width", ref stParam);
                if (stParamOffsetXRet == MvCameraSdk.MV_OK)
                {
                    _roiOffsetXStep = stParam.nInc;
                    _payloadMinWidth = stParam.nMin;
                }

                var stParamOffsetYRet = _mvCameraSdk.MV_CC_GetIntValue_NET("Height", ref stParam);
                if (stParamOffsetYRet == MvCameraSdk.MV_OK)
                {
                    _roiOffsetYStep = stParam.nInc;
                    _payloadMinHeight = stParam.nMin;
                }

                var stParamRetPayloadSize = _mvCameraSdk.MV_CC_GetIntValue_NET("PayloadSize", ref stParam);
                if (stParamRetPayloadSize == MvCameraSdk.MV_OK)
                {
                    PayloadSize = stParam.nCurValue;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取相机参数时发生异常: {ex.Message}");
                return false;
            }
        }

        private void AllocateImageBuffer()
        {
            try
            {
                // 释放现有缓冲区
                if (_grabBuff != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_grabBuff);
                    _grabBuff = IntPtr.Zero;
                }

                // 分配新的图像缓冲区
                // 确保PayloadSize有效，并留出一些额外空间
                var bufferSize = Math.Max(PayloadWidth * PayloadHeight * 4, PayloadSize);
                _grabBuff = Marshal.AllocHGlobal((int)bufferSize);

                if (_grabBuff == IntPtr.Zero)
                    throw new OutOfMemoryException("无法分配图像缓冲区");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"分配图像缓冲区失败: {ex.Message}", ex);
            }
        }

        private void ConfigureCameraSettings(MvCameraSdk.MV_CC_DEVICE_INFO device)
        {
            try
            {
                // 设置相机参数
                var widthResult = _mvCameraSdk.MV_CC_SetWidth_NET(PayloadWidth);
                if (widthResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"设置宽度失败，错误代码: 0x{widthResult:X}");

                var heightResult = _mvCameraSdk.MV_CC_SetHeight_NET(PayloadHeight);
                if (heightResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"设置高度失败，错误代码: 0x{heightResult:X}");

                var offsetXResult = _mvCameraSdk.MV_CC_SetAOIoffsetX_NET(0);
                if (offsetXResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"设置X偏移失败，错误代码: 0x{offsetXResult:X}");

                var offsetYResult = _mvCameraSdk.MV_CC_SetAOIoffsetY_NET(0);
                if (offsetYResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"设置Y偏移失败，错误代码: 0x{offsetYResult:X}");

                var heartbeatResult = _mvCameraSdk.MV_CC_SetHeartBeatTimeout_NET(1000);
                if (heartbeatResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"设置心跳超时失败，错误代码: 0x{heartbeatResult:X}");

                // 探测网络最佳包大小(只对GigE相机有效)
                if (device.nTLayerType == MvCameraSdk.MV_GIGE_DEVICE)
                {
                    ConfigureGigESettings();
                }

                // 设置采集策略
                var grabStrategyResult = _mvCameraSdk.MV_CC_SetGrabStrategy_NET(MvCameraSdk.MV_GRAB_STRATEGY.MV_GrabStrategy_OneByOne);
                if (grabStrategyResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"设置采集策略失败，错误代码: 0x{grabStrategyResult:X}");

                var resendResult = _mvCameraSdk.MV_GIGE_SetResend_NET(0, 0, 0);
                Console.WriteLine($"设置重传结果 = {resendResult}");

                var triggerModeResult = _mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MvCameraSdk.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                if (triggerModeResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"设置触发模式失败，错误代码: 0x{triggerModeResult:X}");

                if (IsGray)
                {
                    var setMono8Res = _mvCameraSdk.MV_CC_SetEnumValue_NET("PixelFormat", (uint)MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Mono8);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"配置相机设置时发生异常: {ex.Message}");
                throw;
            }
        }

        private void ConfigureGigESettings()
        {
            try
            {
                //var nPacketSize = _mvCameraSdk.MV_CC_GetOptimalPacketSize_NET();
                //if (nPacketSize > 0)
                //{
                //    var packetSizeResult = _mvCameraSdk.MV_CC_SetIntValueEx_NET("GevSCPSPacketSize", nPacketSize);
                //    if (packetSizeResult != MvCameraSdk.MV_OK)
                //        Console.WriteLine($"设置包大小失败! nRet=0x{packetSizeResult:X}");

                //    var scpdResult = _mvCameraSdk.MV_CC_SetIntValueEx_NET("GevSCPD", 1000);
                //    if (scpdResult != MvCameraSdk.MV_OK)
                //        Console.WriteLine($"设置SCPD失败! nRet=0x{scpdResult:X}");
                //}
                //else
                //    Console.WriteLine($"获取包大小失败! nRet=0x{nPacketSize:X}");

                //var nPacketSize = _mvCameraSdk.MV_CC_GetOptimalPacketSize_NET();
                //if (nPacketSize > 0)
                {
                    var packetSizeResult = _mvCameraSdk.MV_CC_SetIntValueEx_NET("GevSCPSPacketSize", 1500);
                    if (packetSizeResult != MvCameraSdk.MV_OK)
                        Console.WriteLine($"设置包大小失败! nRet=0x{packetSizeResult:X}");

                    var scpdResult = _mvCameraSdk.MV_CC_SetIntValueEx_NET("GevSCPD", 1000);
                    if (scpdResult != MvCameraSdk.MV_OK)
                        Console.WriteLine($"设置SCPD失败! nRet=0x{scpdResult:X}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"配置GigE设置时发生异常: {ex.Message}");
            }
        }

        private void RegisterImageCallback()
        {
            try
            {
                var registerResult = _mvCameraSdk.MV_CC_RegisterImageCallBackEx_NET(_cbImage, PUser);
                if (registerResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"注册图像回调失败，错误代码: 0x{registerResult:X}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"注册图像回调时发生异常: {ex.Message}");
                throw;
            }
        }

        private void CreateBlackImageBase64()
        {
            try
            {
                if (string.IsNullOrEmpty(_blackImgBase64))
                {
                    using (var blackImage = new Mat((int)PayloadHeight, (int)PayloadWidth, MatType.CV_8UC3, new Scalar(0, 0, 0)))
                    using (var blackBitmap = BitmapConverter.ToBitmap(blackImage))
                    {
                        _blackImgBase64 = BitmapToBase64String(blackBitmap);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建黑色图像Base64时发生异常: {ex.Message}");
                // 不抛出异常，因为这不是关键功能
            }
        }

        public void CloseCamera()
        {
            if (!_isOpened)
                return;

            try
            {
                // 停止图像采集
                if (_isGrabbing)
                {
                    StopGrab();
                }

                // 清理相机资源
                CleanupCameraResources();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"关闭相机时发生异常: {ex.Message}");
            }
            finally
            {
                _isOpened = false;
                _mPDeviceInfo = new MvCameraSdk.MV_CC_DEVICE_INFO();

                // 清理其他状态
                _isGrabbing = false;
                _isPushMat = false;
                _grabIndex = 0;

                // 清理缓冲区
                ClearBuffer();
            }
        }

        public void SetExposureTime(int exposureTime)
        {
            ThrowIfDisposed();

            if (!_isOpened)
                throw new InvalidOperationException("相机未打开，无法设置曝光时间");

            if (exposureTime <= 0)
                throw new ArgumentOutOfRangeException(nameof(exposureTime), "曝光时间必须大于0");

            try
            {
                // 关闭自动曝光
                var autoExposureResult = _mvCameraSdk.MV_CC_SetEnumValue_NET("ExposureAuto", 0);
                if (autoExposureResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"关闭自动曝光失败，错误代码: 0x{autoExposureResult:X}");

                // 设置曝光时间
                var result = _mvCameraSdk.MV_CC_SetFloatValue_NET("ExposureTime", exposureTime);
                if (result != MvCameraSdk.MV_OK)
                {
                    // 尝试获取曝光时间范围以提供更好的错误信息
                    try
                    {
                        var floatRange = new MvCameraSdk.MVCC_FLOATVALUE();
                        var rangeResult = _mvCameraSdk.MV_CC_GetFloatValue_NET("ExposureTime", ref floatRange);
                        if (rangeResult == MvCameraSdk.MV_OK)
                        {
                            throw new InvalidOperationException(
                                $"设置曝光时间失败，错误代码: 0x{result:X}。有效范围: {floatRange.fMin} - {floatRange.fMax}");
                        }
                    }
                    catch { }

                    throw new InvalidOperationException($"设置曝光时间失败，错误代码: 0x{result:X}");
                }

                Console.WriteLine($"曝光时间已设置为: {exposureTime} μs");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"设置曝光时间失败: {ex.Message}", ex);
            }
        }

        public void SetGain(int gain)
        {
            ThrowIfDisposed();

            if (!_isOpened)
                throw new InvalidOperationException("相机未打开，无法设置增益");

            if (gain < 0)
                throw new ArgumentOutOfRangeException(nameof(gain), "增益不能为负数");

            try
            {
                // 关闭自动增益
                var autoGainResult = _mvCameraSdk.MV_CC_SetEnumValue_NET("GainAuto", 0);
                if (autoGainResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"关闭自动增益失败，错误代码: 0x{autoGainResult:X}");

                // 设置增益值
                var result = _mvCameraSdk.MV_CC_SetFloatValue_NET("Gain", gain);
                if (result != MvCameraSdk.MV_OK)
                {
                    // 尝试获取增益范围以提供更好的错误信息
                    try
                    {
                        var floatRange = new MvCameraSdk.MVCC_FLOATVALUE();
                        var rangeResult = _mvCameraSdk.MV_CC_GetFloatValue_NET("Gain", ref floatRange);
                        if (rangeResult == MvCameraSdk.MV_OK)
                        {
                            throw new InvalidOperationException(
                                $"设置增益失败，错误代码: 0x{result:X}。有效范围: {floatRange.fMin} - {floatRange.fMax}");
                        }
                    }
                    catch { }

                    throw new InvalidOperationException($"设置增益失败，错误代码: 0x{result:X}");
                }

                Console.WriteLine($"增益已设置为: {gain} dB");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"设置增益失败: {ex.Message}", ex);
            }
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
            ThrowIfDisposed();

            if (!_isOpened)
                throw new InvalidOperationException("相机未打开，无法开始采集");

            if (_isGrabbing)
            {
                Console.WriteLine("相机已在采集中，忽略重复调用");
                return;
            }

            try
            {
                // 设置ROI区域
                ConfigureRoiSettings(roiRect);

                // 初始化帧信息
                _mStFrameInfo.nFrameLen = 0;
                _mStFrameInfo.enPixelType = MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Undefined;

                // 开始取流
                var startGrabResult = _mvCameraSdk.MV_CC_StartGrabbing_NET();
                if (startGrabResult != MvCameraSdk.MV_OK)
                    throw new InvalidOperationException($"开始采集失败，错误代码: 0x{startGrabResult:X}");

                // 设置为连续采集模式
                var triggerModeResult = _mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerMode",
                    (uint)MvCameraSdk.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                if (triggerModeResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"设置触发模式失败，错误代码: 0x{triggerModeResult:X}");

                // 重置采集计数和时间戳
                _grabIndex = 0;
                _grabTs = HighPrecisionTimer.GetTimestamp();
                _isGrabbing = true;
                _isPushMat = isPushMat;

                Console.WriteLine($"相机开始采集，ROI: ({LastCaptureRoiOffsetX},{LastCaptureRoiOffsetY},{LastCaptureRoiWidth},{LastCaptureRoiHeight})");
            }
            catch (Exception ex)
            {
                // 确保在异常情况下状态一致
                _isGrabbing = false;
                throw new InvalidOperationException($"开始采集失败: {ex.Message}", ex);
            }
        }

        private void ConfigureRoiSettings(Rect roiRect)
        {
            try
            {
                // 设置ROI区域
                if (roiRect.X == 0 && roiRect.Y == 0 && roiRect.Width == 0 && roiRect.Height == 0)
                {
                    // 使用全分辨率
                    if (IsUseCustomSize)
                    {
                        var widthResult = _mvCameraSdk.MV_CC_SetWidth_NET(PayloadWidth);
                        var heightResult = _mvCameraSdk.MV_CC_SetHeight_NET(PayloadHeight);
                        var offsetXResult = _mvCameraSdk.MV_CC_SetAOIoffsetX_NET(0);
                        var offsetYResult = _mvCameraSdk.MV_CC_SetAOIoffsetY_NET(0);

                        if (widthResult != MvCameraSdk.MV_OK)
                            Console.WriteLine($"设置全分辨率宽度失败，错误代码: 0x{widthResult:X}");
                        if (heightResult != MvCameraSdk.MV_OK)
                            Console.WriteLine($"设置全分辨率高度失败，错误代码: 0x{heightResult:X}");
                        if (offsetXResult != MvCameraSdk.MV_OK)
                            Console.WriteLine($"设置全分辨率X偏移失败，错误代码: 0x{offsetXResult:X}");
                        if (offsetYResult != MvCameraSdk.MV_OK)
                            Console.WriteLine($"设置全分辨率Y偏移失败，错误代码: 0x{offsetYResult:X}");

                        LastCaptureRoiOffsetX = 0;
                        LastCaptureRoiOffsetY = 0;
                        LastCaptureRoiWidth = (int)PayloadWidth;
                        LastCaptureRoiHeight = (int)PayloadHeight;
                    }
                }
                else
                {
                    // 使用自定义ROI区域
                    var aoiX = roiRect.X;
                    var aoiY = roiRect.Y;
                    var aoiWidth = roiRect.Width;
                    var aoiHeight = roiRect.Height;

                    // 确保ROI尺寸不小于最小值
                    aoiWidth = (int)Math.Max(_payloadMinWidth > 0 ? _payloadMinWidth : 32, aoiWidth);
                    aoiHeight = (int)Math.Max(_payloadMinHeight > 0 ? _payloadMinHeight : 32, aoiHeight);

                    // 确保ROI不超过传感器范围
                    aoiX = Math.Max(0, Math.Min(aoiX, (int)PayloadWidth - aoiWidth));
                    aoiY = Math.Max(0, Math.Min(aoiY, (int)PayloadHeight - aoiHeight));
                    aoiWidth = Math.Min(aoiWidth, (int)PayloadWidth - aoiX);
                    aoiHeight = Math.Min(aoiHeight, (int)PayloadHeight - aoiY);

                    if (IsUseCustomSize)
                    {
                        var setWidthRet = _mvCameraSdk.MV_CC_SetWidth_NET((uint)aoiWidth);
                        var setHeightRet = _mvCameraSdk.MV_CC_SetHeight_NET((uint)aoiHeight);
                        var setAoiXRet = _mvCameraSdk.MV_CC_SetAOIoffsetX_NET((uint)aoiX);
                        var setAoiYRet = _mvCameraSdk.MV_CC_SetAOIoffsetY_NET((uint)aoiY);

                        LastCaptureRoiOffsetX = setAoiXRet == MvCameraSdk.MV_OK ? aoiX : 0;
                        LastCaptureRoiOffsetY = setAoiYRet == MvCameraSdk.MV_OK ? aoiY : 0;
                        LastCaptureRoiWidth = setWidthRet == MvCameraSdk.MV_OK ? aoiWidth : (int)PayloadWidth;
                        LastCaptureRoiHeight = setHeightRet == MvCameraSdk.MV_OK ? aoiHeight : (int)PayloadHeight;

                        if (setWidthRet != MvCameraSdk.MV_OK)
                            Console.WriteLine($"设置ROI宽度失败，错误代码: 0x{setWidthRet:X}");
                        if (setHeightRet != MvCameraSdk.MV_OK)
                            Console.WriteLine($"设置ROI高度失败，错误代码: 0x{setHeightRet:X}");
                        if (setAoiXRet != MvCameraSdk.MV_OK)
                            Console.WriteLine($"设置ROI X偏移失败，错误代码: 0x{setAoiXRet:X}");
                        if (setAoiYRet != MvCameraSdk.MV_OK)
                            Console.WriteLine($"设置ROI Y偏移失败，错误代码: 0x{setAoiYRet:X}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"配置ROI设置时发生异常: {ex.Message}");
                throw;
            }
        }

        public void StopGrab()
        {
            if (!_isGrabbing)
                return;

            try
            {
                _isGrabbing = false;

                // 停止采集
                var stopGrabResult = _mvCameraSdk.MV_CC_StopGrabbing_NET();
                if (stopGrabResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"停止采集失败，错误代码: 0x{stopGrabResult:X}");

                // 恢复全分辨率设置
                RestoreFullResolution();

                Console.WriteLine("相机采集已停止");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"停止采集时发生异常: {ex.Message}");
                // 确保状态正确
                _isGrabbing = false;
            }
        }

        private void RestoreFullResolution()
        {
            try
            {
                if (!IsUseCustomSize || !_isOpened)
                    return;

                // 恢复全分辨率设置
                var widthResult = _mvCameraSdk.MV_CC_SetWidth_NET(PayloadWidth);
                var heightResult = _mvCameraSdk.MV_CC_SetHeight_NET(PayloadHeight);
                var offsetXResult = _mvCameraSdk.MV_CC_SetAOIoffsetX_NET(0);
                var offsetYResult = _mvCameraSdk.MV_CC_SetAOIoffsetY_NET(0);

                if (widthResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"恢复全分辨率宽度失败，错误代码: 0x{widthResult:X}");
                if (heightResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"恢复全分辨率高度失败，错误代码: 0x{heightResult:X}");
                if (offsetXResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"恢复全分辨率X偏移失败，错误代码: 0x{offsetXResult:X}");
                if (offsetYResult != MvCameraSdk.MV_OK)
                    Console.WriteLine($"恢复全分辨率Y偏移失败，错误代码: 0x{offsetYResult:X}");

                // 重置ROI记录
                LastCaptureRoiOffsetX = 0;
                LastCaptureRoiOffsetY = 0;
                LastCaptureRoiWidth = (int)PayloadWidth;
                LastCaptureRoiHeight = (int)PayloadHeight;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"恢复全分辨率时发生异常: {ex.Message}");
            }
        }

        //private Stopwatch _st = new Stopwatch();

        private readonly List<IntPtr> _listCaptureInptr = new List<IntPtr>();
        private int _targetGetCount = -1;
        private int _targetGetTotalCount;
        private long tEnter = HighPrecisionTimer.GetTimestamp();

        public bool Capture(uint count = 1, int delayMs = 3000, Rect roiRect = new Rect())
        {
            return RunFuncTask(() =>
            {
                ThrowIfDisposed();

                // 参数验证
                if (count == 0)
                    return false;

                if (count > 10 * 200)
                    return false;

                if (!_isOpened)
                    throw new InvalidOperationException("相机未打开，无法采集图像");

                if (delayMs <= 0)
                    throw new ArgumentException("延迟时间必须大于0", nameof(delayMs));

                var tEnter = HighPrecisionTimer.GetTimestamp();
                bool isSuccess = false;

                try
                {
                    // 清理缓冲区
                    ClearBuffer();
                    var tclear = HighPrecisionTimer.GetTimestamp();
                    Console.WriteLine("清理缓冲区耗时: " + HighPrecisionTimer.GetTimestampIntervalMs(tEnter, tclear));

                    // 初始化采集参数
                    _captureRow = 0;
                    _captureCol = 0;
                    _captureCount = 0;

                    // 清理之前的缓冲区
                    CleanupCaptureBuffers();

                    var tclearCaptureInptr = HighPrecisionTimer.GetTimestamp();
                    Console.WriteLine("清理采集缓冲区耗时: " + HighPrecisionTimer.GetTimestampIntervalMs(tEnter, tclearCaptureInptr));

                    // 预分配缓冲区
                    _targetGetCount = 0;
                    _targetGetTotalCount = (int)count;
                    AllocateCaptureBuffers(count);

                    var tPreCollect = HighPrecisionTimer.GetTimestamp();
                    Console.WriteLine("预分配缓冲区耗时: " + HighPrecisionTimer.GetTimestampIntervalMs(tEnter, tPreCollect));

                    // 执行采集任务
                    var captureTask = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            if (!_isGrabbing)
                                StartGrab(false, roiRect);

                            _captureCount = count;
                            // 短暂等待确保相机准备就绪
                            Thread.Sleep(5);
                            _isCapturing = true;

                            var tStartGrab = HighPrecisionTimer.GetTimestamp();
                            Console.WriteLine("开始采集耗时: " + HighPrecisionTimer.GetTimestampIntervalMs(tEnter, tStartGrab));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"采集任务异常: {ex.Message}");
                        }
                    });

                    // 等待采集完成
                    var waitTask = Task.Factory.StartNew(() =>
                    {
                        isSuccess = _waitHandle.WaitOne(delayMs);
                    });

                    Task.WaitAll(captureTask, waitTask);

                    var twaitCaptureEnd = HighPrecisionTimer.GetTimestamp();
                    Console.WriteLine("等待采集结束耗时: " + HighPrecisionTimer.GetTimestampIntervalMs(tEnter, twaitCaptureEnd));

                    // 重置采集状态
                    _captureRow = -1;
                    _captureCol = -1;
                    _captureCount = 0;
                    _isCapturing = false;

                    // 处理采集失败的情况
                    if (!isSuccess)
                    {
                        HandleCaptureFailure(count);
                    }

                    // 停止采集并排序结果
                    StopGrab();
                    SortCaptureResults();

                    return isSuccess;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"采集过程异常: {ex.Message}");
                    _isCapturing = false;
                    StopGrab();
                    return false;
                }
                finally
                {
                    // 清理缓冲区
                    CleanupCaptureBuffers();

                    var tstop = HighPrecisionTimer.GetTimestamp();
                    Console.WriteLine("采集总耗时: " + HighPrecisionTimer.GetTimestampIntervalMs(tEnter, tstop));
                }
            }).Result;
        }

        private async void RunActionTask(Action act)
        {
            await Task.Run(() => act());
        }

        private static async Task<T> RunFuncTask<T>(Func<T> func) => await Task.Run(func.Invoke);

        /// <summary>
        /// 清理采集缓冲区
        /// </summary>
        private void CleanupCaptureBuffers()
        {
            try
            {
                var releasedCount = 0;
                for (var i = 0; i < _listCaptureInptr.Count; i++)
                {
                    if (_listCaptureInptr[i] != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(_listCaptureInptr[i]);
                        _listCaptureInptr[i] = IntPtr.Zero;
                        releasedCount++;
                    }
                }
                _listCaptureInptr.Clear();

                if (releasedCount > 0)
                {
                    Console.WriteLine($"成功释放{releasedCount}个采集缓冲区");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"清理采集缓冲区时发生异常: {ex.Message}");
                // 确保列表被清空，即使部分释放失败
                try
                {
                    _listCaptureInptr.Clear();
                }
                catch
                {
                    // 忽略清理列表时的异常
                }
            }
        }

        /// <summary>
        /// 分配采集缓冲区
        /// </summary>
        /// <param name="count">需要分配的缓冲区数量</param>
        private void AllocateCaptureBuffers(uint count)
        {
            try
            {
                for (var i = 0; i < count; i++)
                {
                    _listCaptureInptr.Add(Marshal.AllocHGlobal((int)PayloadSize));
                }
            }
            catch (OutOfMemoryException)
            {
                // 内存不足时清理已分配的缓冲区
                CleanupCaptureBuffers();
                throw new InvalidOperationException($"无法分配{count}个缓冲区，内存不足");
            }
        }

        /// <summary>
        /// 处理采集失败情况
        /// </summary>
        /// <param name="count">期望采集的图像数量</param>
        private void HandleCaptureFailure(uint count)
        {
            ClearBuffer();

            var tempCount = count;
            var row = 0;
            var col = 0;

            lock (_lockGetBuff)
            {
                for (var i = 0; i < tempCount; i++)
                {
                    if (MatBuffer[row, col] == null || string.IsNullOrEmpty(MatBuffer[row, col]))
                    {
                        MatBuffer[row, col] = _blackImgBase64;
                        MatWidth[row, col] = (int)PayloadWidth;
                        MatHeight[row, col] = (int)PayloadHeight;
                        _matPosList.Add(new MatPos
                        {
                            Col = col,
                            Row = row,
                            TimeStamp = (ulong)HighPrecisionTimer.GetTimestamp()
                        });
                    }

                    col++;
                    if (col != MatBuffer.GetLength(1))
                        continue;
                    col = 0;
                    row++;
                }
            }
        }

        /// <summary>
        /// 对采集结果进行排序
        /// </summary>
        private void SortCaptureResults()
        {
            lock (_lockGetBuff)
                _matPosList = _matPosList.OrderBy(f => f.FrameIndex).ToList();
        }

        public void ClearBuffer()
        {
            ThrowIfDisposed();

            try
            {
                // 清理位置列表
                lock (_lockGetBuff)
                    _matPosList.Clear();

                // 清理采集缓冲区
                CleanupCaptureBuffers();

                // 清理相机缓冲区
                if (_isOpened)
                    _mvCameraSdk.MV_CC_ClearImageBuffer_NET();

                // 重置帧信息
                _lastFrameIndex = -1;
                _lastHostTs = 0;
                _lastTs = 0;
                _firstFrameLen = 0;

                // 清理图像缓冲区
                var row = MatBuffer.GetLength(0);
                var col = MatBuffer.GetLength(1);

                for (var i = 0; i < row; i++)
                {
                    for (var j = 0; j < col; j++)
                    {
                        if (!string.IsNullOrEmpty(MatBuffer[i, j]))
                        {
                            MatBuffer[i, j] = string.Empty;
                        }
                    }
                }

                // 重置缓冲区数组（仅在需要时重新创建）
                if (MatWidth.GetLength(0) != 10 || MatWidth.GetLength(1) != 200)
                {
                    MatWidth = new int[10, 200];
                    MatHeight = new int[10, 200];
                }
                else
                {
                    // 仅清零现有数组，避免重新分配内存
                    Array.Clear(MatWidth, 0, MatWidth.Length);
                    Array.Clear(MatHeight, 0, MatHeight.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"清理缓冲区异常: {ex.Message}");
                throw;
            }
        }

        private List<MatPos> _matPosList = new List<MatPos>();
        private int _lastFrameIndex;
        private ulong _lastTs;
        private long _lastHostTs;
        private uint _firstFrameLen;

        private readonly object _lockGetBuff = new object();

        public Mat GetImageFromBuff(int index, out int row, out int col)
        {
            ThrowIfDisposed();

            lock (_lockGetBuff)
            {
                row = -1;
                col = -1;

                // 参数验证
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), "索引不能为负数");

                if (_matPosList == null || _matPosList.Count == 0)
                    return null;

                if (index >= _matPosList.Count)
                    throw new ArgumentOutOfRangeException(nameof(index), $"索引超出范围，最大索引为{_matPosList.Count - 1}");

                var matPos = _matPosList[index];
                row = matPos.Row;
                col = matPos.Col;

                if (row < 0 || col < 0)
                    return null;

                // 检查缓冲区边界
                if (row >= MatBuffer.GetLength(0) || col >= MatBuffer.GetLength(1))
                    return null;

                try
                {
                    return Base64StringToMat(MatBuffer[row, col]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"获取图像异常: {ex.Message}");
                    return null;
                }
            }
        }

        public string GetImageBase64FromBuff(int index, out int row, out int col)
        {
            ThrowIfDisposed();

            lock (_lockGetBuff)
            {
                row = -1;
                col = -1;

                // 参数验证
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), "索引不能为负数");

                if (_matPosList == null || _matPosList.Count == 0)
                    return null;

                if (index >= _matPosList.Count)
                    throw new ArgumentOutOfRangeException(nameof(index), $"索引超出范围，最大索引为{_matPosList.Count - 1}");

                var matPos = _matPosList[index];
                row = matPos.Row;
                col = matPos.Col;

                if (row < 0 || col < 0)
                    return null;

                // 检查缓冲区边界
                if (row >= MatBuffer.GetLength(0) || col >= MatBuffer.GetLength(1))
                    return null;

                return MatBuffer[row, col];
            }
        }

        public void SaveAllImg()
        {

        }

        private void ImageCallBack(
            IntPtr pData, ref MvCameraSdk.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            try
            {
                // 参数验证
                if (pData == IntPtr.Zero)
                    return;

                var nIndex = (int)pUser;
                if (nIndex != CameraIndex)
                    return;

                // 性能监控 - 减少不必要的控制台输出，仅在调试模式下输出
                var imageCallBackTs = HighPrecisionTimer.GetTimestamp();
                var grabTsCost = HighPrecisionTimer.GetTimestampIntervalMs(_grabTs, imageCallBackTs);

                // 高频回调过滤 - 避免处理过于频繁的回调
                if (grabTsCost < 2 && _targetGetTotalCount > 1)
                    return;

                // 计算设备时间戳
                var ts = ((ulong)pFrameInfo.nDevTimeStampHigh << 32) | pFrameInfo.nDevTimeStampLow;

                // 帧有效性检查 - 避免处理重复或无效帧
                if (pFrameInfo.nFrameNum <= _lastFrameIndex ||
                    pFrameInfo.nHostTimeStamp <= _lastHostTs ||
                    ts <= _lastTs ||
                    pFrameInfo.nHostTimeStamp <= 0)
                    return;

                // 帧长度一致性检查
                if (_lastFrameIndex == -1)
                {
                    _firstFrameLen = pFrameInfo.nFrameLen;
                }
                else
                {
                    if (pFrameInfo.nFrameLen >= _firstFrameLen)
                        _firstFrameLen = pFrameInfo.nFrameLen;
                    else
                        return;
                }

                // 更新帧信息
                _lastFrameIndex = (int)pFrameInfo.nFrameNum;
                _lastHostTs = pFrameInfo.nHostTimeStamp;
                _lastTs = ts;
                _grabTs = imageCallBackTs;

                if (_isCapturing)
                {
                    // 采集模式下的处理
                    if (_targetGetCount >= 0 && _targetGetCount < _listCaptureInptr.Count &&
                        _listCaptureInptr[_targetGetCount] != IntPtr.Zero)
                    {
                        // 安全复制内存数据
                        try
                        {
                            CopyMemory(_listCaptureInptr[_targetGetCount], pData, pFrameInfo.nFrameLen);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"内存复制失败: {ex.Message}");
                            return;
                        }

                        // 处理并保存图像
                        lock (_mBufForSaveImageLock)
                        {
                            try
                            {
                                ProcessAndSaveImage(_listCaptureInptr[_targetGetCount], pFrameInfo, ts);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"图像处理失败: {ex.Message}");
                            }
                        }
                    }
                }
                else
                {
                    // 预览模式下的处理
                    try
                    {
                        CopyMemory(_grabBuff, pData, pFrameInfo.nFrameLen);
                        ProcessAndSaveImage(_grabBuff, pFrameInfo, ts);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"预览图像处理失败: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // 捕获回调中的所有异常，防止回调异常导致相机SDK崩溃
                Console.WriteLine($"图像回调异常: {ex.Message}");
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

            return image;
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

        public static string MatToBase64String(Mat mat)
        {
            var str = string.Empty;
            using (var bitmap = BitmapConverter.ToBitmap(mat))
            {
                str = BitmapToBase64String(bitmap);
                bitmap?.Dispose();
            }

            return str;
        }

        public static Mat Base64StringToMat(string base64String)
        {
            var mat = new Mat();
            using (var bitmap = Base64StringToBitmap(base64String))
            {
                mat = BitmapConverter.ToMat(bitmap);
                bitmap?.Dispose();
            }

            return mat;
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
                    ms.Dispose();
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
            try
            {
                // 从Base64字符串转换回字节数组
                var convertedBytes = Convert.FromBase64String(base64String);

                // 从字节数组转换回Bitmap
                using (var ms = new MemoryStream(convertedBytes))
                {
                    var convertedBitmap = new Bitmap(ms);
                    ms.Dispose();
                    // 保存或使用转换后的Bitmap
                    return convertedBitmap;
                }
            }
            catch (Exception)
            {
                return null;
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
