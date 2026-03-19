using CommonUtility.HikSdk;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace Controller
{
    public sealed class OsramEviyosVisionAnalysis : ControllerBase
    {
        [Description("R/W, 摄像头序列号")]
        public string UsingCameraSn;
        [Description("R/W, 最大拍摄张数")]
        public int MaxCaptureCount = 3;
        [Description("R/W, 图像分析结果")]
        public string VisionAnalysisResult = string.Empty;
        [Description("R/W, 图像数据")]
        public string VisionAnalysisBitmapBase64String = string.Empty;
        [Description("R/W, 标准亮度")]
        public int StandStandLx = 20;
        [Description("R/W, XY比例最小值")]
        public float XyRatioMin = 3.8f;
        [Description("R/W, XY比例最大值")]
        public float XyRatioMax = 4.2f;
        [Description("R/W, 是否显示图像")]
        public bool IsCv2ShowImage;

        public int StardandMin = 40;
        public int StardandMax = 10000;

        private readonly CameraControl _cameraControl = new CameraControl();

        public OsramEviyosVisionAnalysis(string name) :
            base(name)
        { }

        ~OsramEviyosVisionAnalysis()
        {
            Dispose();
        }

        private static readonly object LockCamera = new object();

        [Description("抓拍并检测")]
        public void SnapAndAnalysis(int exposureTime)
        {
            lock (LockCamera)
            {
                Console.WriteLine(Name + @"进入采集");

                VisionAnalysisResult = string.Empty;
                VisionAnalysisBitmapBase64String = string.Empty;

                if (string.IsNullOrEmpty(UsingCameraSn))
                {
                    VisionAnalysisResult = @"NG 未定义相机序列号";
                    return;
                }

                _cameraControl.DeviceListAcq();
                var device =
                    _cameraControl.CameraList.Find(f => string.Equals(f.GigeInfo.chSerialNumber, UsingCameraSn, StringComparison.CurrentCultureIgnoreCase));

                if (device == null)
                {
                    VisionAnalysisResult = string.Format(@"NG 相机序列号={0} 未找到设备", UsingCameraSn);
                    return;
                }

                device.OpenCamera();
                device.SetExposureTime(exposureTime);
                Thread.Sleep(200);
                var isCaptureOk = device.Capture((uint)MaxCaptureCount, 15000);
                device.CloseCamera();
                if (!isCaptureOk)
                {
                    device.ClearBuffer();
                    VisionAnalysisResult = @"NG 图像采集失败";
                    return;
                }

                var isOk = false;
                var showNgMsg = string.Empty;

                for (var i = 0; i < MaxCaptureCount; i++)
                {
                    int row;
                    int col;
                    var mat = device.GetImageFromBuff(i, out row, out col).Clone();

                    string resultBitmap;
                    string ngMsg;
                    isOk = VisionAnalysis(mat, out resultBitmap, out ngMsg);
                    VisionAnalysisBitmapBase64String = resultBitmap;
                    if (isOk)
                        break;

                    showNgMsg = ngMsg;
                }

                device.ClearBuffer();
                VisionAnalysisResult = isOk ? @"DetectionPass" : "NG " + showNgMsg; ;
            }
        }

        public void ClearBitmapResult()
        {
            VisionAnalysisBitmapBase64String = string.Empty;
            BrightSpotBitmapBase64String = string.Empty;
        }

        public bool VisionAnalysis(Mat mat, out string resultBitmap, out string errorMsg)
        {
            resultBitmap = string.Empty;
            errorMsg = string.Empty;

            if (mat == null || mat.Empty())
            {
                errorMsg = "图像为空";
                return false;
            }

            var isOk = false;

            using (var srcMat = mat.Clone())
            {
                Rect r1;
                double angle1;
                if (GetMaxOuterRect(srcMat, out r1, out angle1)) // 第一次，先定位到发光区域及外接矩形的旋转角度
                {
                    RotateImage(srcMat, angle1); // 将图像纠正成0°/90°

                    Rect r3;
                    double angle3;
                    if (GetMaxOuterRect(srcMat, out r3, out angle3)) // 第二次，再定位到已经纠正后的图像中的发光区域
                    {
                        Cv2ShowImage("srcMat", srcMat);

                        // 判断发光区域的长宽比是否正确
                        var isHorizontal = 0; // 0=error,1=horizontal,2=vertical
                        if ((float)r3.Width / r3.Height >= XyRatioMin && (float)r3.Width / r3.Height <= XyRatioMax)
                            isHorizontal = 1;
                        else if ((float)r3.Height / r3.Width >= XyRatioMin && (float)r3.Height / r3.Width <= XyRatioMax)
                            isHorizontal = 2;

                        if (isHorizontal == 1 || isHorizontal == 2)
                        {
                            var lightArea = srcMat.Clone(new Rect(r3.X, r3.Y, r3.Width, r3.Height));
                            if (isHorizontal == 2) // 如果发光区域是竖直的，则旋转90°
                            {
                                var rb = lightArea.ToBitmap();
                                rb.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                lightArea = rb.ToMat();
                            }

                            using (var toAnalysisMat = lightArea.Clone())
                            {
                                // 先等分5*2，每块再等分1*4，计算每块的平均亮度，用于判断大面积不良的

                                var listRectWithResults = new List<RectWithResult>();
                                string calcLxMsg = string.Empty;
                                var isLxResult = AnalysisRectLx(toAnalysisMat, true, StandStandLx, ref listRectWithResults, out calcLxMsg);

                                if (!isLxResult)
                                {
                                    foreach (var r in listRectWithResults.Where(r => !r.IsOk))
                                        toAnalysisMat.Rectangle(r.R, Scalar.Red);

                                    errorMsg += calcLxMsg;
                                    var rb = toAnalysisMat.ToBitmap();
                                    resultBitmap = BitmapToBase64String(rb);
                                    rb.Dispose();
                                }
                                else
                                {
                                    Point[][] contours = { };
                                    HierarchyIndex[] hierarchy = { };

                                    using (var toAnalysisGrayMat = toAnalysisMat.Clone())
                                    {
                                        // 转换成灰度图像
                                        if (toAnalysisGrayMat.Channels() == 3)
                                            Cv2.CvtColor(toAnalysisGrayMat, toAnalysisGrayMat, ColorConversionCodes.BGR2GRAY);
                                        Cv2ShowImage("原始灰度图像", toAnalysisGrayMat);

                                        // 高斯模糊降噪
                                        Cv2.GaussianBlur(toAnalysisGrayMat, toAnalysisGrayMat, new Size(3, 3), 0);
                                        Cv2ShowImage("高斯模糊降噪", toAnalysisGrayMat);

                                        // 自适应二值化
                                        Cv2.AdaptiveThreshold(toAnalysisGrayMat, toAnalysisGrayMat, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 5, 2);
                                        Cv2ShowImage("自适应二值化", toAnalysisGrayMat);

                                        // 形态学操作（断开细连接）
                                        Cv2.Erode(toAnalysisGrayMat, toAnalysisGrayMat, Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3)), iterations: 2);
                                        Cv2ShowImage("形态学操作（断开细连接）", toAnalysisGrayMat);

                                        // Erode后取最大内部轮廓
                                        Rect maxErodeRect;
                                        if (MyCamera.TryGetMaxContourOuterRect(toAnalysisGrayMat, out maxErodeRect))
                                        {
                                            var borderOffet = 2;
                                            maxErodeRect = new Rect(maxErodeRect.X + borderOffet, maxErodeRect.Y + borderOffet, maxErodeRect.Width - borderOffet * 2, maxErodeRect.Height - borderOffet * 2);

                                            using (var dstRoi = toAnalysisGrayMat.Clone(maxErodeRect))
                                            {
                                                var top = maxErodeRect.Y;
                                                var left = maxErodeRect.X;
                                                var bottom = toAnalysisGrayMat.Height - (maxErodeRect.Y + maxErodeRect.Height);
                                                var right = toAnalysisGrayMat.Width - (maxErodeRect.X + maxErodeRect.Width);

                                                Cv2.CopyMakeBorder(dstRoi, dstRoi, top, bottom, left, right, BorderTypes.Constant, Scalar.White);
                                                Cv2ShowImage("去边界", dstRoi);

                                                // 去除小面积连通域
                                                var contours1 = Cv2.FindContoursAsArray(
                                                    dstRoi, RetrievalModes.List, ContourApproximationModes.ApproxSimple
                                                );
                                                foreach (var contour in contours1)
                                                {
                                                    if (Cv2.ContourArea(contour) < StardandMin) // 面积阈值
                                                    {
                                                        Cv2.DrawContours(dstRoi, new[] { contour }, -1, Scalar.White, -1);
                                                    }
                                                    else
                                                    {
                                                        var tp = contour.ToList();
                                                        var xMin = tp.Min(x => x.X);
                                                        var xMax = tp.Max(x => x.X);
                                                        var yMin = tp.Min(x => x.Y);
                                                        var yMax = tp.Max(x => x.Y);

                                                        var filterMax = 3;
                                                        if (Math.Abs(xMin - xMax) <= filterMax || Math.Abs(yMin - yMax) <= filterMax) // 再过滤大面积时左右或上下差值filterMax个像素的线段
                                                        {
                                                            Cv2.DrawContours(dstRoi, new[] { contour }, -1, Scalar.White, -1);
                                                        }
                                                    }
                                                }

                                                Cv2ShowImage("去除小面积连通域", dstRoi);

                                                // 轮廓扫描
                                                Cv2.FindContours(dstRoi, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

                                                var ngContourCount = 0;

                                                var maxSize = 0d;
                                                var maxIndex = -1;
                                                var minSize = (double)(toAnalysisMat.Width * toAnalysisMat.Height);
                                                var minIndex = -1;

                                                for (var i = 0; i < hierarchy.Length; i++)
                                                {
                                                    var len = Cv2.ContourArea(contours[i]);
                                                    //Console.WriteLine(@"SIZE = {0}", len);

                                                    if (len > StardandMin && len < StardandMax)
                                                    {
                                                        Console.WriteLine(@"NG SIZE = {0}", len);
                                                        //Cv2.DrawContours(toAnalysisMat, contours, i, Scalar.Red);
                                                        Cv2.Rectangle(toAnalysisMat, Cv2.MinAreaRect(contours[i]).BoundingRect(), Scalar.Red);
                                                        ngContourCount++;
                                                    }
                                                }

                                                var rb = toAnalysisMat.ToBitmap();
                                                resultBitmap = BitmapToBase64String(rb);
                                                rb.Dispose();

                                                if (ngContourCount == 0)
                                                    isOk = true;
                                            }
                                        }
                                        else
                                        {
                                            errorMsg = "灰度图像二值化后无法找到发光区域";
                                        }

                                        toAnalysisGrayMat.Dispose();
                                    }
                                }

                                toAnalysisMat.Dispose();
                            }

                            lightArea.Dispose();
                            //dst.Dispose();
                        }
                        else
                        {
                            errorMsg = string.Format("图像纠正后宽高比不在{0}~{1}之间,width={2},height={3}", XyRatioMin, XyRatioMax, r3.Width, r3.Height);
                        }
                    }
                    else
                    {
                        errorMsg = "图像纠正后外轮廓定位失败";
                    }
                }
                else
                {
                    errorMsg = "原始图像外轮廓定位失败";
                }

                srcMat.Dispose();
                return isOk;
            }
        }

        private static bool AnalysisRectLx(Mat mat, bool isHorizontal, double standardLx, ref List<RectWithResult> rectWithResults, out string ngMsg)
        {
            using (var blackMat = mat.Clone())
            {
                //Mat element5 = new Mat(5, 5, MatType.CV_8U, new Scalar(1));
                //Cv2.MorphologyEx(blackMat, blackMat, MorphTypes.BlackHat, element5);
                //element5.Dispose();

                //if (blackMat.Channels() == 3)
                //{
                //    Cv2.CvtColor(blackMat, blackMat, ColorConversionCodes.BGR2GRAY);
                //}

                ////Cv2.Threshold(blackMat, blackMat, 0, 255, ThresholdTypes.Otsu);
                //Cv2.Threshold(blackMat, blackMat, 10, 255, ThresholdTypes.Binary);//固定阈值分割
                //Cv2.ImShow("blackMat", blackMat);
                //Cv2.WaitKey();
                //Cv2.DestroyAllWindows();

                ngMsg = string.Empty;

                var width = blackMat.Width;
                var height = blackMat.Height;

                var iMax = isHorizontal ? 20 : 2;
                var jMax = isHorizontal ? 2 : 20;

                var perWidth = width / (float)iMax;//isHorizontal ? width / 20f : width / 2f;
                var perHeight = height / (float)jMax;//isHorizontal ? height / 2f : height / 20f;

                var toDrawRect = new List<RectWithResult>();
                var ngCount = 0;

                for (var i = 0; i < iMax; i++)
                {
                    for (var j = 0; j < jMax; j++)
                    {
                        var rX = i * perWidth;
                        var rY = j * perHeight;
                        var r = MyCamera.GetRectInMat(width, height, new Rect((int)rX, (int)rY, (int)Math.Ceiling(perWidth), (int)Math.Ceiling(perHeight)));

                        using (var rMask = new Mat(blackMat, r))
                        {
                            var rMean = Cv2.Mean(rMask);
                            var lx = blackMat.Channels() == 3 ? MyCamera.GetLxByRgb(rMean) : Math.Round(rMean.Val0, 2, MidpointRounding.AwayFromZero);
                            // Console.WriteLine(@"i={0}, j={1}, lx={2}, standard={3}", i, j, lx, standardLx);

                            var roiNgCount = 0;

                            {
                                //var roiXMax = isHorizontal ? 2 : 5;
                                //var roiYMax = isHorizontal ? 5 : 2;

                                var roiXMax = 1;
                                var roiYMax = 1;

                                var roiInRMaskWidth = rMask.Width / (float)roiXMax;//rMask.Width / 2f;
                                var roiInRMaskHeight = rMask.Height / (float)roiYMax;//rMask.Height / 5f;

                                for (var k = 0; k < roiXMax; k++)
                                {
                                    for (var l = 0; l < roiYMax; l++)
                                    {
                                        var roiX = k * roiInRMaskWidth;
                                        var roiY = l * roiInRMaskHeight;

                                        var roiInRMask = MyCamera.GetRectInMat(rMask.Width, rMask.Height, new Rect((int)roiX, (int)roiY, (int)Math.Ceiling(roiInRMaskWidth), (int)Math.Ceiling(roiInRMaskHeight)));

                                        using (var roiMask = new Mat(rMask, roiInRMask))
                                        {
                                            //roiMask.ImWrite(string.Format(@"E:\Projects\万级像素\自动线终检\有一条线的不良件\测试图像3\RoiMask\{0}_{1}.bmp", k, l));
                                            var roiLx = blackMat.Channels() == 3 ? MyCamera.GetLxByRgb(roiMask.Mean()) : Math.Round(roiMask.Mean().Val0, 2, MidpointRounding.AwayFromZero);

                                            var isRoiLxNg = false;
                                            if (roiLx < standardLx)
                                            {
                                                isRoiLxNg = true;
                                                roiNgCount++;
                                                ngMsg += string.Format(
                                                    @"[i={0}, j={1}, k={2}, l={3}]: roi lx={4}, standard={5}, {6};", i, j, k,
                                                    l, roiLx, standardLx, "NG");
                                            }

                                            Console.WriteLine(@"[i={0}, j={1}, k={2}, l={3}]: roi lx={4}, standard={5}, {6}", i, j, k, l, roiLx, standardLx, isRoiLxNg ? "NG" : "OK");

                                            roiMask.Dispose();
                                        }
                                    }
                                }
                            }

                            //var isNg = lx < standardLx;
                            var isNg = roiNgCount > 0;
                            toDrawRect.Add(new RectWithResult { I = i, J = j, IsOk = !isNg, R = r });
                            if (isNg)
                                ngCount++;

                            rMask.Dispose();
                        }
                    }
                }

                rectWithResults.AddRange(toDrawRect);

                blackMat.Dispose();
                return ngCount <= 0;
            }
        }

        private bool GetMaxOuterRect(Mat srcMat, out Rect outRect, out double outAngle)
        {
            outRect = new Rect();
            outAngle = 0;

            var maxRect = new Rect();
            var maxSize = 0d;
            var angle = 0f;

            // 先讲图像预处理
            // 转化为灰度图
            using (var srcGray = new Mat())
            {
                Cv2.CvtColor(srcMat, srcGray, ColorConversionCodes.RGB2GRAY);
                //// 滤波
                //Cv2.Blur(srcGray, srcGray, new Size(3, 3));

                // 滤波
                // 先闭再开
                using (var element5 = new Mat(5, 5, MatType.CV_8U, new Scalar(1)))
                {
                    Cv2.MorphologyEx(srcGray, srcGray, MorphTypes.Close, element5);
                    Cv2.MorphologyEx(srcGray, srcGray, MorphTypes.Open, element5);

                    element5.Dispose();
                }

                // otsu
                Cv2.Threshold(srcGray, srcGray, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                Cv2ShowImage("srcGray", srcGray);

                // 1、 先取出最大外接矩形
                Point[][] contours;
                HierarchyIndex[] hierarchy;
                Cv2.FindContours(srcGray, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

                for (var i = 0; i < hierarchy.Length; i++)
                {
                    var temp = contours[i];
                    var tempSize = Cv2.ContourArea(temp);

                    if (!(tempSize > maxSize))
                        continue;

                    var areaRect = Cv2.MinAreaRect(temp);
                    var tpRect = areaRect.BoundingRect();
                    if (tpRect.Left >= 0 && tpRect.Left <= srcGray.Width && tpRect.Left + tpRect.Width <= srcGray.Width &&
                        tpRect.Top >= 0 && tpRect.Top <= srcGray.Height && tpRect.Top + tpRect.Height <= srcGray.Height)
                    {
                        angle = areaRect.Angle;
                        maxRect = tpRect;
                        maxSize = tempSize;
                    }
                }

                srcGray.Dispose();
            }

            if (maxSize == 0d)
                return false;

            outRect = maxRect;
            outAngle = angle;
            return true;
        }

        private bool AnalysisContourInRect(Mat srcMat, RectWithResult roi, double minArea)
        {
            using (var roiGrayMat = srcMat.Clone(roi.R))
            {
                using (var srcGray = roiGrayMat.Clone())
                {
                    //Cv2.CvtColor(roiGrayMat, srcGray, ColorConversionCodes.RGB2GRAY);
                    //// 滤波
                    //Cv2.Blur(srcGray, srcGray, new Size(3, 3));
                    //// otsu
                    //Cv2.Threshold(srcGray, srcGray, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                    //Cv2ShowImage("roiGrayMat", srcGray);

                    //Point[][] contours;
                    //HierarchyIndex[] hierarchy;
                    Cv2.FindContours(srcGray, out roi.contours, out roi.hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

                    for (var i = 0; i < roi.hierarchy.Length; i++)
                    {
                        var temp = roi.contours[i];
                        var tempSize = Cv2.ContourArea(temp);
                        var minRect = Cv2.MinAreaRect(temp).BoundingRect();

                        if (tempSize >= minArea && !AreRectsEqual(minRect, new Rect(0, 0, roiGrayMat.Width, roiGrayMat.Height)))
                        {
                            roi.needDrawContoursIndex.Add(i);
                        }
                    }

                    srcGray.Dispose();
                }

                roiGrayMat.Dispose();
            }

            return roi.needDrawContoursIndex.Count == 0;
        }

        private static bool AreRectsEqual(Rect rect1, Rect rect2)
        {
            // 比较Rect的X, Y, Width, Height属性
            return Math.Abs(rect1.X - rect2.X) <= 1 && Math.Abs(rect1.Y - rect2.Y) <= 1 && Math.Abs(rect1.Width - rect2.Width) <= 1 && Math.Abs(rect1.Height - rect2.Height) <= 1;
        }

        private void Cv2ShowImage(string name, Mat mat)
        {
            if (IsCv2ShowImage)
            {
                if (mat != null && !mat.Empty())
                {
                    Cv2.ImShow(name, mat);
                    Cv2.WaitKey();
                    Cv2.DestroyAllWindows();
                }
            }
        }

        private static void RotateImage(Mat src, double degree)
        {
            //旋转中心为图像中心    
            var center = new Point2f
            {
                X = (float)(src.Cols / 2.0),
                Y = (float)(src.Rows / 2.0)
            };
            //int length = 0;
            //length = (int)Math.Sqrt(src.Cols * src.Cols + src.Rows * src.Rows);
            //计算二维旋转的仿射变换矩阵  
            var m = Cv2.GetRotationMatrix2D(center, degree, 1);
            Size size;
            size.Width = src.Cols;
            size.Height = src.Rows;
            Cv2.WarpAffine(src, src, m, size, InterpolationFlags.Linear, 0, Scalar.Black);//仿射变换，背景色填充为黑 
        }

        private static string BitmapToBase64String(Bitmap bitmap)
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

        private static Bitmap Base64StringToBitmap(string base64String)
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

        internal class RectWithResult
        {
            public Rect R;
            public bool IsOk;
            public int I;
            public int J;

            public Point[][] contours;
            public HierarchyIndex[] hierarchy;
            public List<int> needDrawContoursIndex = new List<int>();
        }

        #region 找亮斑 20250120

        [Description("R/W, 找亮斑图像数据")]
        public string BrightSpotBitmapBase64String = string.Empty;
        [Description("R/W, 找亮斑图像分析结果")]
        public string BrightSpotAnalysisResult = string.Empty;

        public int SpotMin = 40;
        public int SpotMax = 10000;

        [Description("抓拍并检测")]
        public void SnapAndAnalysisBrightSpot(int exposureTime)
        {
            lock (LockCamera)
            {
                Console.WriteLine(Name + @"进入采集");

                BrightSpotAnalysisResult = string.Empty;
                BrightSpotBitmapBase64String = string.Empty;

                if (string.IsNullOrEmpty(UsingCameraSn))
                {
                    BrightSpotAnalysisResult = @"NG 未定义相机序列号";
                    return;
                }

                _cameraControl.DeviceListAcq();
                var device =
                    _cameraControl.CameraList.Find(f => string.Equals(f.GigeInfo.chSerialNumber, UsingCameraSn, StringComparison.CurrentCultureIgnoreCase));

                if (device == null)
                {
                    BrightSpotAnalysisResult = string.Format(@"NG 相机序列号={0} 未找到设备", UsingCameraSn);
                    return;
                }

                device.OpenCamera();
                device.SetExposureTime(exposureTime);
                Thread.Sleep(200);
                var isCaptureOk = device.Capture((uint)MaxCaptureCount, 15000);
                device.CloseCamera();
                if (!isCaptureOk)
                {
                    device.ClearBuffer();
                    BrightSpotAnalysisResult = @"NG 图像采集失败";
                    return;
                }

                var isOk = false;
                var showNgMsg = string.Empty;

                for (var i = 0; i < MaxCaptureCount; i++)
                {
                    int row;
                    int col;
                    var mat = device.GetImageFromBuff(i, out row, out col).Clone();

                    string resultBitmap;
                    string ngMsg;
                    isOk = FindBrightSpot(mat, out resultBitmap, out ngMsg);
                    BrightSpotBitmapBase64String = resultBitmap;
                    if (isOk)
                        break;

                    showNgMsg = ngMsg;
                }

                device.ClearBuffer();
                BrightSpotAnalysisResult = isOk ? @"DetectionPass" : "NG " + showNgMsg; ;
            }
        }

        public bool FindBrightSpot(Mat mat, out string resultBitmap, out string errorMsg)
        {
            resultBitmap = string.Empty;
            errorMsg = string.Empty;

            if (mat == null || mat.Empty())
            {
                errorMsg = "图像为空";
                return false;
            }

            var isOk = false;

            using (var srcMat = mat.Clone())
            {
                Rect r1;
                double angle1;
                if (GetMaxOuterRect(srcMat, out r1, out angle1)) // 第一次，先定位到发光区域及外接矩形的旋转角度
                {
                    RotateImage(srcMat, angle1); // 将图像纠正成0°/90°

                    Rect r3;
                    double angle3;
                    if (GetMaxOuterRect(srcMat, out r3, out angle3)) // 第二次，再定位到已经纠正后的图像中的发光区域
                    {
                        Cv2ShowImage("srcMat", srcMat);

                        // 判断发光区域的长宽比是否正确
                        var isHorizontal = 0; // 0=error,1=horizontal,2=vertical
                        if ((float)r3.Width / r3.Height >= XyRatioMin && (float)r3.Width / r3.Height <= XyRatioMax)
                            isHorizontal = 1;
                        else if ((float)r3.Height / r3.Width >= XyRatioMin && (float)r3.Height / r3.Width <= XyRatioMax)
                            isHorizontal = 2;

                        if (isHorizontal == 1 || isHorizontal == 2)
                        {
                            var lightArea = srcMat.Clone(new Rect(r3.X, r3.Y, r3.Width, r3.Height));
                            if (isHorizontal == 2) // 如果发光区域是竖直的，则旋转90°
                            {
                                var rb = lightArea.ToBitmap();
                                rb.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                lightArea = rb.ToMat();
                            }

                            using (var toAnalysisMat = lightArea.Clone())
                            {
                                if (toAnalysisMat.Channels() == 3)
                                    Cv2.CvtColor(toAnalysisMat, toAnalysisMat, ColorConversionCodes.BGR2GRAY);
                                var niVisionImage = MyCamera.MatToVisionImage(toAnalysisMat);
                                Algorithms.MathLookup(niVisionImage, niVisionImage, MathLookupOperator.Exp, 1.5);
                                var toAnalysisGrayMat = MyCamera.VisionImageToMat(niVisionImage);
                                niVisionImage.Dispose();

                                Cv2.Canny(toAnalysisGrayMat, toAnalysisGrayMat, 100, 255);
                                //Cv2.ImShow("toAnalysisGrayMat", toAnalysisGrayMat);

                                Point[][] contours;
                                HierarchyIndex[] hierarchy;

                                // 轮廓扫描
                                Cv2.FindContours(toAnalysisGrayMat, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

                                var ngContourCount = 0;

                                for (var i = 0; i < hierarchy.Length; i++)
                                {
                                    var len = Cv2.ContourArea(contours[i]);
                                    //var len = Cv2.ArcLength(contours[i], true);

                                    if (len > SpotMin && len < SpotMax)
                                    {
                                        Console.WriteLine(@"SIZE = {0}", len);
                                        ngContourCount++;
                                        Cv2.DrawContours(toAnalysisMat, contours, i, Scalar.Red);
                                    }
                                }

                                toAnalysisGrayMat.Dispose();

                                var rb = toAnalysisMat.ToBitmap();
                                resultBitmap = BitmapToBase64String(rb);
                                rb.Dispose();

                                if (ngContourCount == 0)
                                    isOk = true;

                                toAnalysisMat.Dispose();
                            }

                            lightArea.Dispose();
                        }
                        else
                        {
                            errorMsg = string.Format("图像纠正后宽高比不在{0}~{1}之间,width={2},height={3}", XyRatioMin, XyRatioMax, r3.Width, r3.Height);
                        }
                    }
                    else
                    {
                        errorMsg = "图像纠正后外轮廓定位失败";
                    }
                }
                else
                {
                    errorMsg = "原始图像外轮廓定位失败";
                }

                srcMat.Dispose();
                return isOk;
            }
        }

        #endregion
    }
}
