using CommonUtility.HikSdk;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.XFeatures2D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using static CommonUtility.MyTaskScheduler;
using Point = OpenCvSharp.Point;
using PointF = OpenCvSharp.Point2f;

namespace Controller
{
    /*近光LOW法规点定义
        B 50 L：[-3.43°, 0.57°]
        75R：[1.15°, -0.57°]
        75L：[-3.43°, -0.57°]
        50L：[-3.43°, -0.86°]
        50R：[1.72°, -0.86°]
        50V：[0°, -0.86°]
        25L：[-9°, -1.72°]
        25R：[9°, -1.72°]
        HV：[0°, 0°]
        Z III：
        Z IV：
        P1：[-8°, 4°]
        P2：[0°, 4°]
        P3：[8°, 4°]
        P4：[-4°, 2°]
        P5：[0°, 2°]
        P6：[4°, 2°]
        P7：[-8°, 0°]
        P8：[-4°, 0°]
        grad H 5L~5R：[±5°, -0.2°]
        grad 2.5L：[±2.5°, ±1.5°]
        grad 1R：[1°, ±2°]
        grad 2R：[2°, ±2°]
        grad 3R：[3°, ±2°]
        Linearity：
    */

    /*远光HIGH法规点定义
        EMax：
        H2：[-2.58°, 0°]
        H3：[2.58°, 0°]
        H1：[-5.14°, 0°]
        H4：[5.14°, 0°]
    */

    public sealed class HeadlampAnalysisByGb4599 : ControllerBase
    {
        [Description("R/W,当前使用相机SN号")]
        public string CameraSn = string.Empty;

        [Description("R/W,当前使用相机的镜头大小-通常采用的4mm即4000um")]
        public double Ratio = 8000;

        [Description("R/W,当前使用相机像元大小-通常采用的是2.2um")]
        public double PixelScale = 2.4;

        [Description("R/W,相机物距")]
        public double CameraDistanceCm = 37;

        [Description("R/W,菲涅尔透镜物距")]
        public double FresnelLensDistanceCm = 55;

        [Description("R/W,截止线扫描X轴起始像素")]
        public int ScanStartX = 1010;

        [Description("R/W,截止线扫描X轴结束像素")]
        public int ScanEndX = 1500;

        [Description("R/W,最大检测区域H")]
        public int ScanDegreeH = 15;

        [Description("R/W,最大检测区域V")]
        public int ScanDegreeV = 5;

        public HeadlampAnalysisByGb4599(string name) : base(name)
        {
            Images = new Dictionary<string, Mat>();
            //_lbTestPosDegree.Add("B_50_L", new Point2f(-3.43f, 0.57f));
            //_lbTestPosDegree.Add("75R", new Point2f(1.15f, -0.57f));
            //_lbTestPosDegree.Add("75L", new Point2f(-3.43f, -0.57f));
            //_lbTestPosDegree.Add("50L", new Point2f(-3.43f, -0.86f));
            //_lbTestPosDegree.Add("50R", new Point2f(1.72f, -0.86f));
            //_lbTestPosDegree.Add("50V", new Point2f(0f, -0.86f));
            //_lbTestPosDegree.Add("25V", new Point2f(0f, -1.72f));
            //_lbTestPosDegree.Add("25L", new Point2f(-9f, -1.72f));
            //_lbTestPosDegree.Add("25R", new Point2f(9f, -1.72f));
            //_lbTestPosDegree.Add("HV", new Point2f(0f, 0f));
            //_lbTestPosDegree.Add("P1", new Point2f(-8f, 4f));
            //_lbTestPosDegree.Add("P2", new Point2f(0f, 4f));
            //_lbTestPosDegree.Add("P3", new Point2f(8f, 4f));
            //_lbTestPosDegree.Add("P4", new Point2f(-4f, 2f));
            //_lbTestPosDegree.Add("P5", new Point2f(0f, 2f));
            //_lbTestPosDegree.Add("P6", new Point2f(4f, 2f));
            //_lbTestPosDegree.Add("P7", new Point2f(-8f, 0f));
            //_lbTestPosDegree.Add("P8", new Point2f(-4f, 0f));

            _lbTestPosDegree.Add("B50L", new Point2f(-3.43f, 0.57f));
            _lbTestPosDegree.Add("BR", new Point2f(2.5f, 1f));
            _lbTestLine.Add("BLL", Tuple.Create(new Point2f(-20f, 0.57f), new Point2f(-8f, 0.57f)));
            _lbTestPosDegree.Add("P", new Point2f(-7f, 0f));
            _lbTestPosDegree2.Add(new Dictionary<string, PointF>
                   {
                       { "S50", new PointF(-8f, 4f) },
                       { "S50LL", new PointF(0f, 4f) },
                       { "S50RR", new PointF(8f, 4f) }
                   });
            _lbTestPosDegree2.Add(new Dictionary<string, PointF>
                   {
                       { "S100", new PointF(-4f, 2f) },
                       { "S100LL", new PointF(0f, 2f) },
                       { "S100RR", new PointF(4f, 2f) }
                   });
            _lbTestPosDegree.Add("HV", new Point2f(0f, 0f));
            _lbTestPosDegree.Add("75R", new Point2f(1.15f, -0.57f));
            _lbTestPosDegree.Add("50R", new Point2f(1.72f, -0.86f));
            _lbTestPosDegree.Add("50V", new Point2f(0f, -0.86f));
            _lbTestPosDegree.Add("50L", new Point2f(-3.43f, -0.86f));
            _lbTestLine.Add("LINE50", Tuple.Create(new Point2f(-6.84f, -0.86f), new Point2f(6.84f, -0.86f)));
            _lbTestPosDegree.Add("40R", new Point2f(9, -1.07f));
            _lbTestPosDegree.Add("40L", new Point2f(-9, -1.07f));
            _lbTestLine.Add("LINE40RR", Tuple.Create(new Point2f(9f, -1.07f), new Point2f(14f, -1.07f)));
            _lbTestLine.Add("LINE40LL", Tuple.Create(new Point2f(-14f, -1.07f), new Point2f(-9f, -1.07f)));
            _lbTestLine.Add("LINE25R", Tuple.Create(new Point2f(9f, -1.72f), new Point2f(16f, -1.72f)));
            _lbTestLine.Add("LINE25", Tuple.Create(new Point2f(-9f, -1.72f), new Point2f(9f, -1.72f)));
            _lbTestPosDegree.Add("25V", new Point2f(0f, -1.72f));
            _lbTestLine.Add("LINE25L", Tuple.Create(new Point2f(-16f, -1.72f), new Point2f(-9f, -1.72f)));
            _lbTestLine.Add("LINE15", Tuple.Create(new Point2f(-20f, -2.86f), new Point2f(20f, -2.86f)));
            _lbTestLine.Add("LINE10", Tuple.Create(new Point2f(-4.5f, -4f), new Point2f(2f, -4f)));

            _lbTestPoly.Add("Area3A", new Point2f[] { new Point2f(-8f, 1f), new Point2f(-8f, 4f), new Point2f(8f, 4f), new Point2f(8f, 2f), new Point2f(6f, 1.5f), new Point2f(1.5f, 1.5f), new Point2f(0f, 0f), new Point2f(-4f, 0f), });
            _lbTestPoly.Add("AreaLine10Bottom", new Point2f[] { new Point2f(-4.5f, -4f), new Point2f(2f, -4f), new Point2f(2f, -4.95f), new Point2f(-4.5f, -4.95f), });
        }

        ~HeadlampAnalysisByGb4599()
        {
            Dispose();
        }

        #region 图像获取相关

        private readonly CameraControl _cameraControl = new CameraControl();

        public Dictionary<string, Mat> Images { get; set; }

        public void CaptureImage(string name, uint exposeTime, int captureCount)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (string.IsNullOrEmpty(CameraSn))
                return;

            if (exposeTime == 0)
                return;

            _cameraControl.DeviceListAcq();
            var device = _cameraControl.CameraList.Find(f =>
                string.Equals(f.GigeInfo.chSerialNumber, CameraSn, StringComparison.CurrentCultureIgnoreCase));

            if (device != null)
            {
                device.OpenCamera();
                device.SetExposureTime((int)exposeTime);
                Thread.Sleep(250);
                if (device.Capture((uint)captureCount))
                {
                    int row, col;
                    var mat0 = device.GetImageFromBuff(0, out row, out col);
                    var size = mat0.Size();
                    var type = mat0.Type();

                    var averagedFrame = new Mat(size, type, Scalar.All(0));
                    for (var i = 0; i < captureCount; i++)
                    {
                        var frame = device.GetImageFromBuff(i, out row, out col);
                        Cv2.AddWeighted(averagedFrame, 1.0, frame, 1.0 / captureCount, 0, averagedFrame);
                    }
                    AppendImage(name, averagedFrame);
                }

                device.ClearBuffer();
                device.CloseCamera();
            }
        }

        public void ReadImage(string name, string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath) && !string.IsNullOrEmpty(name))
            {
                var mat = Cv2.ImRead(filePath);
                AppendImage(name, mat);
                mat.Dispose();
            }
        }

        public void ClearAllImage()
        {
            var keys = Images.Keys.ToList();
            foreach (var name in keys)
            {
                try
                {
                    if (Images[name] != null)
                    {
                        Images[name].Dispose();
                        Images[name] = null;
                    }
                }
                catch (Exception)
                {
                    Images[name] = null;
                }
            }

            Images.Clear();
        }

        public void AppendImage(string name, Mat mat)
        {
            if (Images.ContainsKey(name))
            {
                try
                {
                    if (Images[name] != null)
                    {
                        Images[name].Dispose();
                        Images[name] = null;
                    }
                }
                catch (Exception)
                {
                    Images[name] = null;
                }
            }
            else
            {
                Images.Add(name, null);
            }

            Images[name] = mat.Clone();
        }

        #endregion

        #region °与像素之间的转换

        /// <summary>
        /// 根据°计算像素
        /// </summary>
        /// <param name="degree">需要转换的°</param>
        /// <param name="ratio">ratio指的是相机镜头大小，通常采用的4mm即4000um</param>
        /// <param name="pixelScale">pixelScale指的是相机像元大小，通常采用的是2.2um</param>
        /// <returns></returns>
        public int DegreeToPixel(double degree)
        {
            var distance = CalculateFresnelLensDistancePixel();
            var offsetPixel = (int)(Math.Tan(Math.Abs(GetRad(degree))) * distance);

            return offsetPixel;

            //var rad = degree * (Math.PI / 180);
            //var tan = Math.Tan(rad);
            //var pixel = (int)Math.Abs(tan * ratio / pixelScale);
            //return pixel;
        }

        ///// <summary>
        ///// 根据像素计算°
        ///// </summary>
        ///// <param name="pixel">需要转换的像素，是指像素的长度值</param>
        ///// <param name="ratio">ratio指的是相机镜头大小，通常采用的4mm即4000um</param>
        ///// <param name="pixelScale">pixelScale指的是相机像元大小，通常采用的是2.2um</param>
        ///// <returns></returns>
        //public double PixelToDegree(int pixel, double ratio, double pixelScale)
        //{
        //    var tan = pixel * pixelScale / ratio;
        //    var rad = Math.Atan(tan);
        //    var degree = Math.Round(rad * (180 / Math.PI), 2, MidpointRounding.AwayFromZero);

        //    return degree;
        //}

        #endregion

        #region 分析过程新,以LB为基准

        public int InflectionPixelPointX = int.MinValue;
        public int InflectionPixelPointY = int.MinValue;

        //public double Gray75L = double.MinValue;        
        //public double Gray25L = double.MinValue;       
        //public double Gray25R = double.MinValue;
        //public double GrayP1 = double.MinValue;
        //public double GrayP2 = double.MinValue;
        //public double GrayP3 = double.MinValue;
        //public double GrayP4 = double.MinValue;
        //public double GrayP5 = double.MinValue;
        //public double GrayP6 = double.MinValue;
        //public double GraySumP1P2P3 = double.MinValue;
        //public double GraySumP4P5P6 = double.MinValue;
        //public double GrayP7 = double.MinValue;
        //public double GrayP8 = double.MinValue;

        public double Grad2d5L = double.MinValue;

        public string AnalysisResult = string.Empty;
        public string AnalysisResultBase64String = string.Empty;

        /*
         * 新法规GB4599
         */

        public double InflectionDegree = double.MinValue;

        public double GrayHv = double.MinValue;

        public double EMaxPointDegreeH = double.MinValue;
        public double EMaxPointDegreeV = double.MinValue;
        public double GrayEMax = double.MinValue;

        public double GrayB50L = double.MinValue;
        public double GrayBR = double.MinValue;
        public double GrayLineBLL = double.MinValue;
        public double GrayP = double.MinValue;
        public double GrayArea3A = double.MinValue;
        public double GrayS50S50LLS50RR = double.MinValue;
        public double GrayS100S100LLS100RR = double.MinValue;
        public double Gray75R = double.MinValue;
        public double Gray50R = double.MinValue;
        public double Gray50V = double.MinValue;
        public double Gray50L = double.MinValue;
        public double GrayLine50 = double.MinValue;
        public double Gray40R = double.MinValue;
        public double Gray40L = double.MinValue;
        public double GrayLine40RR = double.MinValue;
        public double GrayLine40LL = double.MinValue;
        public double GrayLine25R = double.MinValue;
        public double GrayLine25 = double.MinValue;
        public double Gray25V = double.MinValue;
        public double GrayLine25L = double.MinValue;
        public double GrayLine15 = double.MinValue;
        public double GrayLine10 = double.MinValue;
        public double GrayLine10Bottom = double.MinValue;

        public void ClearAnalysisResult()
        {
            AnalysisResult = string.Empty;
            AnalysisResultBase64String = string.Empty;

            InflectionDegree = double.MinValue;

            InflectionPixelPointX = int.MinValue;
            InflectionPixelPointY = int.MinValue;

            GrayHv = double.MinValue;

            EMaxPointDegreeH = double.MinValue;
            EMaxPointDegreeV = double.MinValue;
            GrayEMax = double.MinValue;

            GrayB50L = double.MinValue;
            GrayBR = double.MinValue;
            GrayLineBLL = double.MinValue;
            GrayP = double.MinValue;
            GrayArea3A = double.MinValue;
            GrayS50S50LLS50RR = double.MinValue;
            GrayS100S100LLS100RR = double.MinValue;
            Gray75R = double.MinValue;
            Gray50R = double.MinValue;
            Gray50V = double.MinValue;
            Gray50L = double.MinValue;
            GrayLine50 = double.MinValue;
            Gray40R = double.MinValue;
            Gray40L = double.MinValue;
            GrayLine40RR = double.MinValue;
            GrayLine40LL = double.MinValue;
            GrayLine25R = double.MinValue;
            GrayLine25 = double.MinValue;
            Gray25V = double.MinValue;
            GrayLine25L = double.MinValue;
            GrayLine15 = double.MinValue;
            GrayLine10 = double.MinValue;
            GrayLine10Bottom = double.MinValue;

            Grad2d5L = double.MinValue;
        }

        private readonly Dictionary<string, Point2f> _lbTestPosDegree = new Dictionary<string, Point2f>();
        private readonly List<Dictionary<string, Point2f>> _lbTestPosDegree2 = new List<Dictionary<string, PointF>>();
        private readonly Dictionary<string, Tuple<Point2f, Point2f>> _lbTestLine = new Dictionary<string, Tuple<Point2f, Point2f>>();
        private readonly Dictionary<string, Point2f[]> _lbTestPoly = new Dictionary<string, PointF[]>();

        private readonly Dictionary<string, Point2f> _hbTestPosDegree2 = new Dictionary<string, Point2f>();

        public void AnalyzeImage(string srcName)
        {
            ClearAnalysisResult();

            //ReadImage("近光原图", @"E:\Projects\557_PES\Image_20250211155530648.bmp");

            if (!Images.ContainsKey(srcName) || Images[srcName] == null || Images[srcName].Empty())
            {
                AnalysisResult = "FAILED 图像不存在";
                return;
            }

            // 取原始图像
            using (var imageSrc = Images[srcName].Clone())
            {
                var srcWidth = imageSrc.Width;
                var srcHeight = imageSrc.Height;
                var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(21, 3));

                Mat imageSrcGray;
                Mat imageSrcClahe;
                Mat imageSrcGaussianBlur;
                Mat imageSrcOtsu;
                double imageSrcOtsuThresholdValue;
                Mat imageSrcClose;
                Mat imageSrcCannyEdge;
                ImageDeNoise(imageSrc, out imageSrcGray, out imageSrcClahe, out imageSrcGaussianBlur, out imageSrcOtsu, out imageSrcOtsuThresholdValue, out imageSrcClose, out imageSrcCannyEdge);

                AppendImage("原图灰度图像", imageSrcGray);
                AppendImage("原图自适应直方图均衡化（CLAHE）", imageSrcClahe);
                AppendImage("原图高斯滤波降噪", imageSrcGaussianBlur);
                AppendImage("原图OTSU二值化图像", imageSrcOtsu);
                AppendImage("原图形态学闭操作（连接断裂边缘）", imageSrcClose);
                AppendImage("原图边缘检测", imageSrcCannyEdge);

                // 通过降噪出来后取出OTSU二值化图像
                using (var toCheckSrc = imageSrcOtsu.Clone())
                {
                    imageSrcGray.Dispose();
                    imageSrcClahe.Dispose();
                    imageSrcGaussianBlur.Dispose();
                    imageSrcOtsu.Dispose();
                    imageSrcClose.Dispose();
                    imageSrcCannyEdge.Dispose();

                    Rect maxContourRect;
                    if (MyCamera.TryGetMaxContourOuterRect(toCheckSrc, out maxContourRect))
                    {
                        var offsetX = maxContourRect.X;
                        var offsetY = maxContourRect.Y;

                        // 取得最大的外接轮廓作为待测光型ROI
                        using (var roi = imageSrc.Clone(maxContourRect))
                        {
                            AppendImage("待测光型ROI", roi);

                            Mat imageRoiGray;
                            Mat imageRoiClahe;
                            Mat imageRoiGaussianBlur;
                            Mat imageRoiOtsu;
                            double imageRoiOtsuThresholdValue;
                            Mat imageRoiClose;
                            Mat imageRoiCannyEdge;
                            // 重新处理ROI图像
                            ImageDeNoise(roi, out imageRoiGray, out imageRoiClahe, out imageRoiGaussianBlur, out imageRoiOtsu, out imageRoiOtsuThresholdValue, out imageRoiClose, out imageRoiCannyEdge);

                            AppendImage("待测光型ROI灰度图像", imageRoiGray);
                            AppendImage("待测光型ROI自适应直方图均衡化（CLAHE）", imageRoiClahe);
                            AppendImage("待测光型ROI高斯滤波降噪", imageRoiGaussianBlur);
                            AppendImage("待测光型ROI OTSU二值化图像", imageRoiOtsu);
                            AppendImage("待测光型ROI形态学闭操作（连接断裂边缘）", imageRoiClose);
                            AppendImage("待测光型ROI边缘", imageRoiCannyEdge);

                            // 取得ROI图像的最大外接轮廓
                            Rect edgeMaxRect;
                            if (MyCamera.TryGetMaxContourOuterRect(imageRoiCannyEdge, out edgeMaxRect, RetrievalModes.List, ContourApproximationModes.ApproxNone))
                            {
                                using (var roiContoursMat = imageRoiCannyEdge.Clone())
                                {
                                    Cv2.Rectangle(roiContoursMat, edgeMaxRect, Scalar.Green);
                                    AppendImage("待测光型ROI轮廓", roiContoursMat);
                                    roiContoursMat.Dispose();
                                }

                                // 先获取获取最亮点位置
                                Point pEMax = default;
                                {
                                    // 先取得最亮点灰度值及坐标位置
                                    double minVal, maxVal;
                                    Point minLoc, maxLoc;
                                    Cv2.MinMaxLoc(imageRoiGray, out minVal, out maxVal, out minLoc, out maxLoc);

                                    using (var toCheckEMaxMask = imageRoiGray.Clone())
                                    {
                                        Cv2.Threshold(toCheckEMaxMask, toCheckEMaxMask, maxVal * 0.90, 255, ThresholdTypes.Binary);
                                        Cv2.MorphologyEx(toCheckEMaxMask, toCheckEMaxMask, MorphTypes.Close, kernel);

                                        // 找到轮廓
                                        Point[][] contoursEMax;
                                        HierarchyIndex[] hierarchyEMax;
                                        Cv2.FindContours(toCheckEMaxMask, out contoursEMax, out hierarchyEMax, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                                        var maxEMaxArea = 0d;
                                        var maxEMaxIndex = -1;

                                        // 计算并绘制每个轮廓的几何中心
                                        var tempEMaxcontourIndex = 0;
                                        foreach (var contour in contoursEMax)
                                        {
                                            var area = Cv2.ContourArea(contour);

                                            if (area > maxEMaxArea)
                                            {
                                                maxEMaxArea = area;
                                                maxEMaxIndex = tempEMaxcontourIndex;
                                            }

                                            tempEMaxcontourIndex++;
                                        }

                                        Cv2.CvtColor(toCheckEMaxMask, toCheckEMaxMask, ColorConversionCodes.GRAY2BGR);

                                        if (maxEMaxIndex > -1)
                                        {
                                            toCheckEMaxMask.DrawContours(contoursEMax, maxEMaxIndex, Scalar.Red);

                                            // 计算矩
                                            var moments = Cv2.Moments(contoursEMax[maxEMaxIndex]);

                                            // 计算中心坐标
                                            var cx = moments.M10 / moments.M00;
                                            var cy = moments.M01 / moments.M00;
                                            Cv2.Circle(toCheckEMaxMask, new Point(cx, cy), 1, Scalar.Purple, -1);

                                            pEMax = new Point(cx + offsetX, cy + offsetY);
                                        }

                                        AppendImage("EMax区域轮廓及EMax点位置", toCheckEMaxMask);
                                        toCheckEMaxMask.Dispose();
                                    }
                                }

                                if (pEMax != default)
                                {
                                    var scanLeftLimitation = DegreeToPixel(3.43) + DegreeToPixel(0.28647);
                                    var scanRightLimitation = DegreeToPixel(6) - DegreeToPixel(0.28647);

                                    // 自上而下寻找Y轴第一个点
                                    var listEdgeLinePoints = new List<Point>();
                                    var listGradPoints = new List<Point>();
                                    //for (var x = ScanStartX - offsetX; x <= ScanEndX - offsetX; x++)
                                    //for (var x = 0; x <= imageRoiCannyEdge.Width; x++)

                                    for (var x = (pEMax.X - scanLeftLimitation) - offsetX; x <= (pEMax.X + scanRightLimitation) - offsetX; x++)
                                    {
                                        for (var y = 0; y < imageRoiCannyEdge.Height; y++)
                                        {
                                            // 获取当前像素的灰度值
                                            var pixelValue = imageRoiCannyEdge.At<byte>(y, x);
                                            if (pixelValue != 255)
                                                continue;

                                            listEdgeLinePoints.Add(new Point(x, y));
                                            break;
                                        }
                                    }
                                    using (var roiEdgeLine = new Mat(roi.Size(), MatType.CV_8UC1, Scalar.Black))
                                    {
                                        foreach (var p in listEdgeLinePoints)
                                            roiEdgeLine.Set(p.Y, p.X, new Vec3b(255, 255, 255));
                                        AppendImage("绘制明暗截止线", roiEdgeLine);

                                        //var approxTest = Cv2.ApproxPolyDP(listEdgeLinePoints, 1 / CalculateScaleRatio(PixelScale, Ratio / 1000f, CameraDistanceCm * 0.01f), false);

                                        Cv2.MorphologyEx(roiEdgeLine, roiEdgeLine, MorphTypes.Close, kernel);
                                        AppendImage("明暗截止线形态学闭操作（连接断裂边缘）", roiEdgeLine);

                                        // 找到轮廓
                                        Point[][] edgeLineContours;
                                        HierarchyIndex[] edgeLineHierarchy;
                                        Cv2.FindContours(roiEdgeLine, out edgeLineContours, out edgeLineHierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                                        // 拐点检测
                                        if (edgeLineHierarchy.Any())
                                        {
                                            var maxPointCount = 0;
                                            var maxPointCountIndex = -1;

                                            for (var a = 0; a < edgeLineHierarchy.Length; a++)
                                            {
                                                //if (edgeLineContours[a].Length < 5)
                                                //    continue;

                                                if (edgeLineContours[a].Length > maxPointCount)
                                                {
                                                    maxPointCount = edgeLineContours[a].Length;
                                                    maxPointCountIndex = a;
                                                }
                                            }

                                            if (maxPointCountIndex != -1 && maxPointCount >= 4)
                                            {
                                                var approx = Cv2.ApproxPolyDP(edgeLineContours[maxPointCountIndex], 3, true);
                                                approx = Cv2.ApproxPolyDP(listEdgeLinePoints, 1 / CalculateScaleRatio(PixelScale, Ratio / 1000f, CameraDistanceCm * 0.01f), false);

                                                var newPs = new List<Point>();
                                                foreach (var p in approx)
                                                {
                                                    if (newPs.Contains(p))
                                                        continue;
                                                    var filterLen = 10; // 这里10个像素长度需改成按照毫米为单位
                                                    var p1 = p;
                                                    var findAny = newPs.FindAll(f => GetPointDistance(f, p1) <= filterLen);
                                                    if (!findAny.Any())
                                                        newPs.Add(p);
                                                }

                                                newPs = newPs.OrderBy(f => f.X).ToList();

                                                if (newPs.Count >= 4)
                                                {
                                                    // 已找到角点,开始分析拐点
                                                    using (var dst = imageSrc.Clone(maxContourRect))
                                                    {
                                                        if (dst.Channels() == 1)
                                                            Cv2.CvtColor(dst, dst, ColorConversionCodes.GRAY2BGR);

                                                        foreach (var t in newPs)
                                                            Cv2.Circle(dst, t, 1, Scalar.Red, -1);

                                                        Point inflectionPoint = default;
                                                        double hhLineRotA = 0;

                                                        // 曲率分析
                                                        var breakAt1 = -1;
                                                        var breakAt2 = -1;
                                                        var listSlope = new List<double>();

                                                        for (var i = 1; i < newPs.Count - 1; i++)
                                                        {
                                                            var p1 = newPs[0];
                                                            var p2 = newPs[i];

                                                            // 计算出的斜率是顺时针方向的
                                                            var slope1 = (float)(p2.Y - p1.Y) / (p2.X - p1.X);
                                                            listSlope.Add(slope1);

                                                            //var p1 = newPs[i - 1];
                                                            //var p2 = newPs[i];
                                                            //var p3 = newPs[i + 1];

                                                            //var len1 = GetPointDistance(p1, p2);
                                                            //var len2 = GetPointDistance(p2, p3);

                                                            //if (Math.Abs(p1.Y - p2.Y) < 35 && p2.Y > p3.Y) // // 这里35个像素需改成按照毫米为单位
                                                            //{
                                                            //    var slope1 = (float)(p2.Y - p1.Y) / (p2.X - p1.X);
                                                            //    var slope2 = (float)(p3.Y - p2.Y) / (p3.X - p2.X);

                                                            //    var angle1 = Math.Atan(slope1) * (180 / Math.PI);
                                                            //    var angle2 = Math.Atan(slope2) * (180 / Math.PI);

                                                            //    if (angle1 >= -10 && angle1 <= 10 && angle2 > -60 && angle2 < -10)
                                                            //    {
                                                            //        var startP = newPs[i - 1];
                                                            //        var breakP = newPs[i];
                                                            //        var stopP = newPs[i + 1];

                                                            //        var leftPs = new List<Point>();
                                                            //        var rightPs = new List<Point>();

                                                            //        foreach (var edge in listEdgeLinePoints)
                                                            //        {
                                                            //            if (edge.X >= startP.X && edge.X < breakP.X)
                                                            //                leftPs.Add(edge);

                                                            //            if (edge.X > breakP.X && edge.X <= stopP.X)
                                                            //                rightPs.Add(edge);
                                                            //        }

                                                            //        var line1 = GetLine(leftPs, roiEdgeLine.Cols);
                                                            //        var line2 = GetLine(rightPs, roiEdgeLine.Cols);
                                                            //        Cv2.Line(dst, line1.P1, line1.P2, Scalar.Yellow);
                                                            //        Cv2.Line(dst, line2.P1, line2.P2, Scalar.Yellow);

                                                            //        inflectionPoint = GetIntersection(line1.P1, line1.P2, line2.P1, line2.P2);

                                                            //        Cv2.Circle(dst, inflectionPoint, 1, Scalar.Green, -1);
                                                            //        break;
                                                            //    }
                                                            //}
                                                        }

                                                        var minSlope = listSlope.Min();
                                                        var maxSlope = listSlope.Max();
                                                        for (var i = 0; i < listSlope.Count; i++)
                                                        {
                                                            var slope = listSlope[i];
                                                            if (Math.Abs(slope - minSlope) < 0.001)
                                                                breakAt2 = i + 1;
                                                            if (Math.Abs(slope - maxSlope) < 0.001)
                                                                breakAt1 = i + 1;
                                                        }

                                                        if (breakAt1 != -1 && breakAt2 != -1 && breakAt1 != breakAt2 && breakAt2 > breakAt1)
                                                        {
                                                            var startLp = newPs[0];
                                                            var stopLp = newPs[breakAt1];

                                                            var startRp = newPs[breakAt1];
                                                            var stopRp = newPs[breakAt2];

                                                            var leftPs = new List<Point>();
                                                            var rightPs = new List<Point>();

                                                            foreach (var edge in listEdgeLinePoints)
                                                            {
                                                                if (edge.X >= startLp.X && edge.X <= stopLp.X)
                                                                    leftPs.Add(edge);

                                                                if (edge.X > startRp.X && edge.X <= stopRp.X)
                                                                    rightPs.Add(edge);
                                                            }

                                                            if (leftPs.Count >= 4 && rightPs.Count >= 4)
                                                            {
                                                                var line1 = GetLine(leftPs, roiEdgeLine.Cols);
                                                                var line2 = GetLine(rightPs, roiEdgeLine.Cols);
                                                                Cv2.Line(dst, line1.P1, line1.P2, Scalar.Yellow);
                                                                Cv2.Line(dst, line2.P1, line2.P2, Scalar.Yellow);

                                                                inflectionPoint = GetIntersection(line1.P1, line1.P2, line2.P1, line2.P2);

                                                                //// debug
                                                                //inflectionPoint = new Point(inflectionPoint.X + 20, inflectionPoint.Y +10);

                                                                hhLineRotA = CalculateRotAngle(line1.P1, line1.P2);

                                                                var agree1 = CalculateRotAngle(line1.P1, line1.P2);
                                                                var agree2 = CalculateRotAngle(line2.P1, line2.P2);
                                                                InflectionDegree = agree1 - agree2;

                                                                Cv2.Circle(dst, inflectionPoint, 1, Scalar.Green, -1);
                                                            }
                                                        }

                                                        AppendImage("显示找寻拐点结果", dst);
                                                        dst.Dispose();

                                                        // 已经找到拐点
                                                        // 建立坐标系
                                                        if (inflectionPoint != default)
                                                        {
                                                            inflectionPoint = new Point(inflectionPoint.X + offsetX, inflectionPoint.Y + offsetY);

                                                            using (var systemMat = imageSrc.Clone())
                                                            {
                                                                //Mat M = Cv2.GetRotationMatrix2D(inflectionPoint, hhLineRotA, 1.0);

                                                                //double cos = M.Get<double>(0, 0);
                                                                //double sin = M.Get<double>(0, 1);
                                                                //int nw = (int)(cos * systemMat.Width + sin * systemMat.Height);
                                                                //int nh = (int)(sin * systemMat.Width + cos * systemMat.Height);

                                                                //M.Set<double>(0, 2, M.Get<double>(0, 2) + (nw / 2 - systemMat.Width / 2));
                                                                //M.Set<double>(1, 2, M.Get<double>(1, 2) + (nh / 2 - systemMat.Height / 2));

                                                                //Cv2.WarpAffine(systemMat, systemMat, M, new Size(nw, nh), InterpolationFlags.Linear, BorderTypes.Constant, new Scalar(255, 255, 0));

                                                                //M.Dispose();

                                                                //{
                                                                //    var ttt=systemMat.Clone();
                                                                //    Cv2.Circle(ttt, inflectionPoint, 1, Scalar.Green, -1);
                                                                //    ttt.ImWrite(@"E:\Projects\557_PES\ttt.bmp");
                                                                //}

                                                                var roiRect = GetGrayRoiByDegree(inflectionPoint, 0, 0, ScanDegreeH, ScanDegreeV); // 这里需要判断scanROI的区域是否超范围

                                                                if (systemMat.Channels() == 1)
                                                                    Cv2.CvtColor(systemMat, systemMat, ColorConversionCodes.GRAY2BGR);

                                                                // 计算HV点的像素坐标,并根据HV点建立坐标系
                                                                {
                                                                    var pH0V0 = DegreeToPixelPoint(inflectionPoint, 0, 0);
                                                                    Cv2.Line(systemMat, new Point(0, pH0V0.Y), new Point(srcWidth, pH0V0.Y), Scalar.Yellow);
                                                                    Cv2.Line(systemMat, new Point(pH0V0.X, 0), new Point(pH0V0.X, srcHeight), Scalar.Yellow);

                                                                    Cv2.Line(systemMat, new Point(0, inflectionPoint.Y), new Point(srcWidth, inflectionPoint.Y), Scalar.Yellow);

                                                                    // 画出HV点和拐点位置
                                                                    Cv2.Circle(systemMat, pH0V0, 1, Scalar.Green, -1);
                                                                    Cv2.Circle(systemMat, inflectionPoint, 1, Scalar.Green, -1);
                                                                }

                                                                //var grad = DegreeToPixelPoint(inflectionPoint, -2.5, 0);
                                                                //Cv2.Line(systemMat, new Point(grad.X, 0), new Point(grad.X, systemMat.Height), Scalar.Blue);

                                                                // 垂直扫描V-V线2.5L处的截止线，确定最大梯度G
                                                                // 扫描步长0.05°
                                                                var maxG = double.MinValue;

                                                                // 方法1
                                                                //{
                                                                //    var pixelCutOffLinen2d5L = DegreeToPixelPoint(inflectionPoint, -2.5, 0);
                                                                //    var findPointIndex = listGradPoints.FindIndex(f => Math.Abs((f.X + offsetX) - pixelCutOffLinen2d5L.X) <= 1);
                                                                //    if (findPointIndex != -1)
                                                                //    {
                                                                //        var findPoint = new Point(listGradPoints[findPointIndex].X + offsetX, listGradPoints[findPointIndex].Y + offsetY);
                                                                //        var findPointDegree = PixelPointToDegree(inflectionPoint, findPoint);
                                                                //        var p1 = DegreeToPixelPoint(inflectionPoint, findPointDegree.X, findPointDegree.Y);
                                                                //        var r1 = GetGrayRoiByDegree(inflectionPoint, findPointDegree.X, findPointDegree.Y, 0.1);
                                                                //        var lx1 = GetGray(new Mat(Images[srcName], r1));

                                                                //        var p2 = DegreeToPixelPoint(inflectionPoint, findPointDegree.X, findPointDegree.Y + 0.1);
                                                                //        var r2 = GetGrayRoiByDegree(inflectionPoint, findPointDegree.X, findPointDegree.Y + 0.1, 0.1);
                                                                //        var lx2 = GetGray(new Mat(Images[srcName], r2));

                                                                //        maxG = Math.Round(Math.Log10(lx1) - Math.Log10(lx2), 2, MidpointRounding.AwayFromZero);

                                                                //        var maxGP1 = p1;
                                                                //        var maxGP2 = p2;
                                                                //        Cv2.Line(systemMat, maxGP1, maxGP2, Scalar.Blue);
                                                                //        Cv2.PutText(systemMat, string.Format("Grad_2.5L: '{0}'", maxG),
                                                                //            new Point(maxGP2.X + 1, maxGP2.Y + 1), HersheyFonts.HersheySimplex, 0.4, Scalar.Blue);
                                                                //        Cv2.Circle(systemMat, maxGP1, 1, Scalar.Purple);
                                                                //        Cv2.Circle(systemMat, maxGP2, 1, Scalar.Purple);
                                                                //    }
                                                                //}

                                                                // 方法2
                                                                {
                                                                    Point maxGp1 = default;
                                                                    Point maxGp2 = default;

                                                                    var gp1 = DegreeToPixelPoint(inflectionPoint, -2.5, -1.5);
                                                                    var gp2 = DegreeToPixelPoint(inflectionPoint, -2.5, 0);
                                                                    Cv2.Line(systemMat, gp1, gp2, Scalar.Yellow);

                                                                    var listGrays = new List<Tuple<Point, double, Point, double, double>>();

                                                                    for (var v = -1.5d; v <= 0d; v += 0.05)
                                                                    {
                                                                        //if (v + 0.1 > 5)
                                                                        //    break;

                                                                        var p1 = DegreeToPixelPoint(inflectionPoint, -2.5, v);
                                                                        var r1 = GetGrayRoiByDegree(inflectionPoint, -2.5, v, 0.1);
                                                                        //r1 = new Rect(new Point(p1.X - 1, p1.Y - 1), new Size(3, 3));
                                                                        //var lx1 = GetGray(new Mat(Images[srcName], r1));
                                                                        r1 = new Rect(new Point(p1.X, p1.Y), new Size(1, 1));
                                                                        var lx1 = GetGray(new Mat(Images[srcName], r1));

                                                                        var p2 = DegreeToPixelPoint(inflectionPoint, -2.5, v + 0.1);
                                                                        var r2 = GetGrayRoiByDegree(inflectionPoint, -2.5, v + 0.1, 0.1);
                                                                        //r2 = new Rect(new Point(p2.X - 1, p2.Y - 1), new Size(3, 3));
                                                                        //var lx2 = GetGray(new Mat(Images[srcName], r2));
                                                                        r2 = new Rect(new Point(p2.X, p2.Y), new Size(1, 1));
                                                                        var lx2 = GetGray(new Mat(Images[srcName], r2));

                                                                        var g = Math.Round(Math.Log10(lx1) - Math.Log10(lx2), 2, MidpointRounding.AwayFromZero);

                                                                        listGrays.Add(new Tuple<Point, double, Point, double, double>(p1, lx1, p2, lx2, g));

                                                                        if (!(g >= maxG))
                                                                            continue;

                                                                        maxG = g;
                                                                        maxGp1 = p1;
                                                                        maxGp2 = p2;
                                                                    }

                                                                    //foreach (var g in listGrays)
                                                                    //{
                                                                    //    Console.WriteLine($@"P1={g.Item1}:{g.Item2}; P2={g.Item3}:{g.Item4}; G={g.Item5}");
                                                                    //}

                                                                    //Console.WriteLine($@"最大梯度G：{maxG}");

                                                                    if (Math.Abs(maxG - double.MinValue) > 1 && maxGp1 != default && maxGp1 != default)
                                                                    {
                                                                        Cv2.Line(systemMat, maxGp1, maxGp2, Scalar.Blue);
                                                                        Cv2.PutText(systemMat, string.Format("Grad_2.5L: '{0}'", maxG),
                                                                            new Point(maxGp2.X + 1, maxGp2.Y + 1), HersheyFonts.HersheySimplex, 0.4, Scalar.Blue);
                                                                        Cv2.Circle(systemMat, maxGp1, 1, Scalar.Purple);
                                                                        Cv2.Circle(systemMat, maxGp2, 1, Scalar.Purple);
                                                                    }
                                                                }

                                                                // 计算各法规点的灰度值
                                                                var dicGray = new Dictionary<string, double>();
                                                                {
                                                                    // 多边形
                                                                    foreach (var item in _lbTestPoly)
                                                                    {
                                                                        var name = item.Key;

                                                                        var polyMat = imageSrc.Clone();
                                                                        if (polyMat.Channels() == 3)
                                                                        {
                                                                            Cv2.CvtColor(polyMat, polyMat, ColorConversionCodes.BGRA2GRAY);
                                                                        }

                                                                        Point[] points = new Point[item.Value.Length];

                                                                        for (int i = 0; i < item.Value.Length; i++)
                                                                        {
                                                                            points[i] = DegreeToPixelPoint(inflectionPoint, item.Value[i].X, item.Value[i].Y);
                                                                        }

                                                                        using (Mat mask = new Mat(imageSrc.Size(), MatType.CV_8UC1, new Scalar(0)))
                                                                        {
                                                                            Cv2.FillPoly(mask, new Point[][] { points }, new Scalar(255));

                                                                            double maxVal = double.MinValue;
                                                                            Point maxPoint = new Point();

                                                                            for (int y = 0; y < imageSrc.Height; y++)
                                                                            {
                                                                                for (int x = 0; x < imageSrc.Width; x++)
                                                                                {
                                                                                    // 检查是否在多边形内部
                                                                                    if (mask.At<byte>(y, x) == 255)
                                                                                    {
                                                                                        var thisPointRoi = new Rect(x - 1, y - 1, 3, 3);
                                                                                        var lxRoi = new Mat(imageSrc, thisPointRoi);
                                                                                        var lx = GetGray(lxRoi);
                                                                                        lxRoi.Dispose();

                                                                                        //if (name == "AreaLine10Bottom")
                                                                                        //{
                                                                                        //    Console.WriteLine(string.Format("AreaLine10Bottom Point: {0},{1}={2}", x, y, lx));
                                                                                        //}

                                                                                        if (maxVal == double.MinValue)
                                                                                        {
                                                                                            maxVal = lx;
                                                                                            maxPoint = new Point(x, y);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (lx > maxVal)
                                                                                            {
                                                                                                maxVal = lx;
                                                                                                maxPoint = new Point(x, y);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                            dicGray.Add(name, maxVal);
                                                                            Cv2.Circle(systemMat, maxPoint, 1, Scalar.Red, -1);
                                                                            Cv2.PutText(systemMat, string.Format("{0}: '{1}'", item.Key, maxVal), new Point(maxPoint.X + 1, maxPoint.Y + 2), HersheyFonts.HersheySimplex, 0.4, Scalar.Red);
                                                                            Cv2.Polylines(systemMat, new Point[][] { points }, isClosed: true, Scalar.Blue, thickness: 1);
                                                                            mask.Dispose();
                                                                        }
                                                                    }

                                                                    var listPurplePoints = new List<Point>();
                                                                    foreach (var item in _lbTestPosDegree)
                                                                    {
                                                                        var pixelPoint = DegreeToPixelPoint(inflectionPoint, item.Value.X, item.Value.Y);
                                                                        listPurplePoints.Add(pixelPoint);

                                                                        var thisPointRoi = GetGrayRoiByDegree(inflectionPoint, item.Value.X, item.Value.Y, 0.1, 0.1);//  new Rect(pixelPoint.X - 1, pixelPoint.Y - 1, 3, 3);
                                                                        var lxRoi = new Mat(imageSrc, thisPointRoi);
                                                                        var lx = GetGray(lxRoi);
                                                                        dicGray.Add(item.Key, lx);
                                                                        lxRoi.Dispose();

                                                                        Cv2.Rectangle(systemMat, thisPointRoi, Scalar.Red);
                                                                        Cv2.Circle(systemMat, pixelPoint, 1, Scalar.Red, -1);
                                                                        Cv2.PutText(systemMat, string.Format("{0}: '{1}'", item.Key, lx), new Point(thisPointRoi.X + thisPointRoi.Width + 1, thisPointRoi.Y + 2), HersheyFonts.HersheySimplex, 0.4, Scalar.Red);
                                                                    }

                                                                    foreach (var p in _lbTestPosDegree2)
                                                                    {
                                                                        var maxLx = double.MinValue;
                                                                        var str = string.Empty;

                                                                        foreach (var item in p)
                                                                        {
                                                                            var pixelPoint = DegreeToPixelPoint(inflectionPoint, item.Value.X, item.Value.Y);
                                                                            listPurplePoints.Add(pixelPoint);

                                                                            var thisPointRoi = GetGrayRoiByDegree(inflectionPoint, item.Value.X, item.Value.Y, 0.1, 0.1);//  new Rect(pixelPoint.X - 1, pixelPoint.Y - 1, 3, 3);
                                                                            var lxRoi = new Mat(imageSrc, thisPointRoi);
                                                                            var lx = GetGray(lxRoi);

                                                                            str += item.Key + "+";

                                                                            if (double.MinValue == maxLx)
                                                                            {
                                                                                maxLx = lx;
                                                                            }
                                                                            else
                                                                            {
                                                                                maxLx += lx;
                                                                                //if (lx < minLx)
                                                                                //{
                                                                                //    minLx = lx;
                                                                                //}
                                                                            }

                                                                            lxRoi.Dispose();

                                                                            Cv2.Rectangle(systemMat, thisPointRoi, Scalar.Red);
                                                                            Cv2.Circle(systemMat, pixelPoint, 1, Scalar.Red, -1);
                                                                            Cv2.PutText(systemMat, string.Format("{0}: '{1}'", item.Key, lx), new Point(thisPointRoi.X + thisPointRoi.Width + 1, thisPointRoi.Y + 2), HersheyFonts.HersheySimplex, 0.4, Scalar.Red);
                                                                        }

                                                                        str = str.TrimEnd('+');
                                                                        dicGray.Add(str, maxLx);
                                                                    }

                                                                    foreach (var p in listPurplePoints)
                                                                        Cv2.Circle(systemMat, p, 1, Scalar.Purple, -1);

                                                                    // 线段
                                                                    foreach (var item in _lbTestLine)
                                                                    {
                                                                        var name = item.Key;
                                                                        var startPixel = DegreeToPixelPoint(inflectionPoint, item.Value.Item1.X, item.Value.Item1.Y);
                                                                        var endPixel = DegreeToPixelPoint(inflectionPoint, item.Value.Item2.X, item.Value.Item2.Y);

                                                                        var minLx = double.MinValue;
                                                                        var pointMaxP = default(Point);

                                                                        var showTxtPoint = new Point((startPixel.X + endPixel.X) / 2f + 1, endPixel.Y + 15);
                                                                        if (startPixel.X < roiRect.X)
                                                                            showTxtPoint.X = (int)((roiRect.X + endPixel.X) / 2f);
                                                                        else if (endPixel.X > roiRect.X + roiRect.Width)
                                                                            showTxtPoint.X = (int)((startPixel.X + roiRect.X + roiRect.Width) / 2f) - 50;

                                                                        for (var i = item.Value.Item1.X; i <= item.Value.Item2.X; i = i + 0.01f)
                                                                        {
                                                                            var testH = i;
                                                                            var testV = item.Value.Item1.Y;

                                                                            var thisPointRoi = GetGrayRoiByDegree(inflectionPoint, testH, testV, 0.1, 0.1);//  new Rect(pixelPoint.X - 1, pixelPoint.Y - 1, 3, 3);
                                                                            if (thisPointRoi.X < 0 || thisPointRoi.Y < 0 ||
                                                                                thisPointRoi.X + thisPointRoi.Width > imageSrc.Width ||
                                                                                thisPointRoi.Y + thisPointRoi.Height > imageSrc.Height)
                                                                                continue;
                                                                            var lxRoi = new Mat(imageSrc, thisPointRoi);
                                                                            var lx = GetGray(lxRoi);

                                                                            //if (name == "LINE10")
                                                                            //{
                                                                            //    Console.WriteLine(string.Format("LINE10 Point: {0},{1}={2}", DegreeToPixelPoint(inflectionPoint, testH, testV).X, DegreeToPixelPoint(inflectionPoint, testH, testV).Y, lx));
                                                                            //}

                                                                            if (minLx == double.MinValue)
                                                                            {
                                                                                minLx = lx;
                                                                                pointMaxP = DegreeToPixelPoint(inflectionPoint, testH, testV);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (lx < minLx)
                                                                                {
                                                                                    minLx = lx;
                                                                                    pointMaxP = DegreeToPixelPoint(inflectionPoint, testH, testV);
                                                                                }
                                                                            }
                                                                        }

                                                                        minLx = Math.Round(minLx, 2, MidpointRounding.AwayFromZero);
                                                                        dicGray.Add(name, minLx);

                                                                        Cv2.Circle(systemMat, pointMaxP, 1, Scalar.Red, -1);
                                                                        Cv2.Line(systemMat, startPixel, endPixel, Scalar.Purple, 1);
                                                                        Cv2.Circle(systemMat, startPixel, 1, Scalar.Blue, -1);
                                                                        Cv2.Circle(systemMat, endPixel, 1, Scalar.Blue, -1);
                                                                        //Cv2.PutText(systemMat, string.Format("{0}: '{1}'", item.Key, lx), new Point(endPixel.X + 1, endPixel.Y + 1), HersheyFonts.HersheySimplex, 0.4, Scalar.Red);
                                                                        Cv2.PutText(systemMat, string.Format("{0}: {1}", name, minLx), showTxtPoint, HersheyFonts.HersheySimplex, 0.4, Scalar.Purple);
                                                                    }
                                                                }

                                                                // 以50R的灰度值为二值化阈值下限扫描最亮点
                                                                var pEMaxDegreeH = double.MinValue;
                                                                var pEMaxDegreeV = double.MinValue;
                                                                {
                                                                    if (pEMax != default)
                                                                    {
                                                                        var pEMaxDegree = PixelPointToDegree(inflectionPoint, pEMax);
                                                                        pEMaxDegreeH = pEMaxDegree.X;
                                                                        pEMaxDegreeV = pEMaxDegree.Y;
                                                                        var toCheckEMaxRoi = GetGrayRoiByDegree(inflectionPoint, pEMaxDegree.X, pEMaxDegree.Y, 0.1, 0.1);
                                                                        var lx = GetGray(new Mat(Images[srcName], toCheckEMaxRoi));
                                                                        dicGray.Add("EMax", lx);
                                                                        Cv2.Circle(systemMat, pEMax, 1, Scalar.Purple, -1);
                                                                        Cv2.Rectangle(systemMat, toCheckEMaxRoi, Scalar.Purple);
                                                                        //Cv2.PutText(systemMat, string.Format("EMax[{0},{1}]: '{2}'", pEMaxDegree.X, pEMaxDegree.Y, lx), new Point(toCheckEMaxRoi.X + toCheckEMaxRoi.Width + 1, toCheckEMaxRoi.Y + 2), HersheyFonts.HersheySimplex,
                                                                        //    0.4, Scalar.Purple);
                                                                        Cv2.PutText(systemMat, string.Format("EMax[{0},{1}]", pEMaxDegree.X, pEMaxDegree.Y), new Point(toCheckEMaxRoi.X + toCheckEMaxRoi.Width + 1, toCheckEMaxRoi.Y + 2), HersheyFonts.HersheySimplex,
                                                                           0.4, Scalar.Purple);
                                                                    }

                                                                    //using (var toCheckEMaxMask = imageSrc.Clone(maxContourRect))
                                                                    //{
                                                                    //    var degree50R = _lbTestPosDegree["50R"];
                                                                    //    var threRoi = new Mat(imageSrc, GetGrayRoiByDegree(inflectionPoint, degree50R.X, degree50R.Y, 0.1, 0.1));
                                                                    //    var gray50R = GetGray(threRoi);
                                                                    //    threRoi.Dispose();

                                                                    //    if (toCheckEMaxMask.Channels() == 3)
                                                                    //        Cv2.CvtColor(toCheckEMaxMask, toCheckEMaxMask, ColorConversionCodes.BGR2GRAY);
                                                                    //    Cv2.Threshold(toCheckEMaxMask, toCheckEMaxMask, Math.Abs(gray50R - 255) < 0.1 ? 254 : gray50R, 255, ThresholdTypes.Binary);
                                                                    //    Cv2.MorphologyEx(toCheckEMaxMask, toCheckEMaxMask, MorphTypes.Close, kernel);

                                                                    //    // 找到轮廓
                                                                    //    Point[][] contoursEMax;
                                                                    //    HierarchyIndex[] hierarchyEMax;
                                                                    //    Cv2.FindContours(toCheckEMaxMask, out contoursEMax, out hierarchyEMax, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                                                                    //    var maxEMaxArea = 0d;
                                                                    //    var maxEMaxIndex = -1;

                                                                    //    // 计算并绘制每个轮廓的几何中心
                                                                    //    var tempEMaxcontourIndex = 0;
                                                                    //    foreach (var contour in contoursEMax)
                                                                    //    {
                                                                    //        var area = Cv2.ContourArea(contour);

                                                                    //        if (area > maxEMaxArea)
                                                                    //        {
                                                                    //            maxEMaxArea = area;
                                                                    //            maxEMaxIndex = tempEMaxcontourIndex;
                                                                    //        }

                                                                    //        tempEMaxcontourIndex++;
                                                                    //    }

                                                                    //    Cv2.CvtColor(toCheckEMaxMask, toCheckEMaxMask, ColorConversionCodes.GRAY2BGR);

                                                                    //    if (maxEMaxIndex > -1)
                                                                    //    {
                                                                    //        toCheckEMaxMask.DrawContours(contoursEMax, maxEMaxIndex, Scalar.Red);

                                                                    //        // 计算矩
                                                                    //        var moments = Cv2.Moments(contoursEMax[maxEMaxIndex]);

                                                                    //        // 计算中心坐标
                                                                    //        var cx = moments.M10 / moments.M00;
                                                                    //        var cy = moments.M01 / moments.M00;

                                                                    //        var pEMax = new Point(cx + offsetX, cy + offsetY);
                                                                    //        if (pEMax != default)
                                                                    //        {
                                                                    //            var pEMaxDegree = PixelPointToDegree(inflectionPoint, pEMax);
                                                                    //            pEMaxDegreeH = pEMaxDegree.X;
                                                                    //            pEMaxDegreeV = pEMaxDegree.Y;
                                                                    //            var toCheckEMaxRoi = GetGrayRoiByDegree(inflectionPoint, pEMaxDegree.X, pEMaxDegree.Y, 0.1, 0.1);
                                                                    //            var lx = GetGray(new Mat(Images[srcName], toCheckEMaxRoi));
                                                                    //            dicGray.Add("EMax", lx);
                                                                    //            Cv2.Circle(systemMat, pEMax, 1, Scalar.Purple, -1);
                                                                    //            Cv2.Rectangle(systemMat, toCheckEMaxRoi, Scalar.Purple);
                                                                    //            Cv2.PutText(systemMat, string.Format("EMax[{0},{1}]: '{2}'", pEMaxDegree.X, pEMaxDegree.Y, lx), new Point(toCheckEMaxRoi.X + toCheckEMaxRoi.Width + 1, toCheckEMaxRoi.Y + 2), HersheyFonts.HersheySimplex,
                                                                    //                0.4, Scalar.Purple);
                                                                    //        }
                                                                    //    }

                                                                    //    AppendImage("EMax区域轮廓", toCheckEMaxMask);
                                                                    //    toCheckEMaxMask.Dispose();
                                                                    //}
                                                                }

                                                                Cv2.Rectangle(systemMat, roiRect, Scalar.Red);

                                                                using (var dstSystemImage = systemMat.Clone(roiRect))
                                                                {
                                                                    // 分析完成,输出结果
                                                                    {
                                                                        AnalysisResult = "Finish";
                                                                        AnalysisResultBase64String = MyCamera.BitmapToBase64String(dstSystemImage.ToBitmap());

                                                                        InflectionPixelPointX = inflectionPoint.X;
                                                                        InflectionPixelPointY = inflectionPoint.Y;

                                                                        GrayHv = dicGray["HV"];

                                                                        EMaxPointDegreeH = pEMaxDegreeH;
                                                                        EMaxPointDegreeV = pEMaxDegreeV;
                                                                        GrayEMax = dicGray["EMax"];

                                                                        Grad2d5L = maxG;

                                                                        GrayB50L = dicGray["B50L"];
                                                                        GrayBR = dicGray["BR"];
                                                                        GrayLineBLL = dicGray["BLL"];
                                                                        GrayP = dicGray["P"];
                                                                        GrayArea3A = dicGray["Area3A"];
                                                                        GrayS50S50LLS50RR = dicGray["S50+S50LL+S50RR"];
                                                                        GrayS100S100LLS100RR = dicGray["S100+S100LL+S100RR"];
                                                                        Gray75R = dicGray["75R"];
                                                                        Gray50R = dicGray["50R"];
                                                                        Gray50V = dicGray["50V"];
                                                                        Gray50L = dicGray["50L"];
                                                                        GrayLine50 = dicGray["LINE50"];
                                                                        Gray40R = dicGray["40R"];
                                                                        Gray40L = dicGray["40L"];
                                                                        GrayLine40RR = dicGray["LINE40RR"];
                                                                        GrayLine40LL = dicGray["LINE40LL"];
                                                                        GrayLine25R = dicGray["LINE25R"];
                                                                        GrayLine25 = dicGray["LINE25"];
                                                                        Gray25V = dicGray["25V"];
                                                                        GrayLine25L = dicGray["LINE25L"];
                                                                        GrayLine15 = dicGray["LINE15"];
                                                                        GrayLine10 = dicGray["LINE10"];
                                                                        GrayLine10Bottom = dicGray["AreaLine10Bottom"];
                                                                    }

                                                                    AppendImage("根据拐点建立坐标系", dstSystemImage);
                                                                    dstSystemImage.ImWrite(@"E:\Projects\557_PES\图像-20250418\dstSystemImage.bmp");
                                                                    dstSystemImage.Dispose();
                                                                }

                                                                systemMat.Dispose();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            AnalysisResult = "FAILED 拐点判定失败";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    AnalysisResult = "FAILED 截止线角点扫描失败,角点个数:" + newPs.Count;
                                                }
                                            }
                                            else
                                            {
                                                AnalysisResult = "FAILED 截止线角点扫描失败";
                                            }
                                        }
                                        else
                                        {
                                            AnalysisResult = "FAILED 截止线边缘轮廓扫描失败";
                                        }

                                        roiEdgeLine.Dispose();
                                    }
                                }
                                else
                                {
                                    AnalysisResult = "FAILED 找最亮点失败";
                                }
                            }
                            else
                            {
                                AnalysisResult = "FAILED ROI轮廓扫描失败";
                            }

                            imageRoiGray.Dispose();
                            imageRoiClahe.Dispose();
                            imageRoiGaussianBlur.Dispose();
                            imageRoiOtsu.Dispose();
                            imageRoiClose.Dispose();
                            imageRoiCannyEdge.Dispose();

                            roi.Dispose();
                        }
                    }
                    else
                    {
                        AnalysisResult = "FAILED 最大轮廓扫描失败";
                    }

                    toCheckSrc.Dispose();
                }

                imageSrc.Dispose();
            }
        }

        #endregion

        #region 分析过程新，以HB最亮点为基准

        public string AnalysisResultHb = string.Empty;
        public string AnalysisResultAlb = string.Empty;

        public string AnalysisResultHbBase64String = string.Empty;

        public double GrayEMaxHb = double.MinValue;
        //public double GrayH1Hb = double.MinValue;
        //public double GrayH2Hb = double.MinValue;
        //public double GrayH3Hb = double.MinValue;
        //public double GrayH4Hb = double.MinValue;
        //public double GrayH5Hb = double.MinValue;

        public double Gray12LH = double.MinValue;
        public double Gray9LH = double.MinValue;
        public double Gra6LH = double.MinValue;
        public double Gray3LH = double.MinValue;
        public double Gray3RH = double.MinValue;
        public double Gray6RH = double.MinValue;
        public double Gray9RH = double.MinValue;
        public double Gray12RH = double.MinValue;
        public double GrayV2U = double.MinValue;

        public double GrayAlb = double.MinValue;

        public void ClearAnalysisResultHbAlb()
        {
            AnalysisResultHb = string.Empty;
            AnalysisResultAlb = string.Empty;

            AnalysisResultHbBase64String = string.Empty;

            GrayEMaxHb = double.MinValue;

            Gray12LH = double.MinValue;
            Gray9LH = double.MinValue;
            Gra6LH = double.MinValue;
            Gray3LH = double.MinValue;
            Gray3RH = double.MinValue;
            Gray6RH = double.MinValue;
            Gray9RH = double.MinValue;
            Gray12RH = double.MinValue;
            GrayV2U = double.MinValue;

            GrayAlb = double.MinValue;
        }

        public void AnalyzeHbAlb(string hbSrcName, string albSrcName)
        {
            //ReadImage("远光原图", @"E:\Projects\557_PES\图像-20250418\557光型测试图片-20250418\远光\R\亮度OK\3000.bmp");
            //ReadImage("ALB原图", @"E:\Projects\557_PES\图像-20250418\557光型测试图片-20250418\ALB\R\亮度OK\3500.bmp");

            //ReadImage("远光原图", @"E:\Projects\557_PES\图像-20250418\557光型测试图片-20250418\远光\R\亮度下限\3000.bmp");
            //ReadImage("ALB原图", @"E:\Projects\557_PES\图像-20250418\557光型测试图片-20250418\ALB\R\亮度下限\3500.bmp");

            if (!Images.ContainsKey(hbSrcName) || Images[hbSrcName] == null || Images[hbSrcName].Empty())
            {
                AnalysisResultHb = "FAILED HB图像不存在";
                return;
            }

            Point hbHv = default;

            using (var imageHbSrc = Images[hbSrcName].Clone())
            {
                var srcWidth = imageHbSrc.Width;
                var srcHeight = imageHbSrc.Height;

                Mat imageSrcGray;
                Mat imageSrcClahe;
                Mat imageSrcGaussianBlur;
                Mat imageSrcOtsu;
                double imageSrcOtsuThresholdValue;
                Mat imageSrcClose;
                Mat imageSrcCannyEdge;
                ImageDeNoise(imageHbSrc, out imageSrcGray, out imageSrcClahe, out imageSrcGaussianBlur, out imageSrcOtsu, out imageSrcOtsuThresholdValue, out imageSrcClose, out imageSrcCannyEdge);

                AppendImage("原图灰度图像", imageSrcGray);
                AppendImage("原图自适应直方图均衡化（CLAHE）", imageSrcClahe);
                AppendImage("原图高斯滤波降噪", imageSrcGaussianBlur);
                AppendImage("原图OTSU二值化图像", imageSrcOtsu);
                AppendImage("原图形态学闭操作（连接断裂边缘）", imageSrcClose);
                AppendImage("原图边缘检测", imageSrcCannyEdge);

                using (var toGetMaxRectSrc = imageSrcOtsu.Clone())
                {
                    Rect maxContourRect;
                    if (MyCamera.TryGetMaxContourOuterRect(toGetMaxRectSrc, out maxContourRect))
                    {
                        var offsetX = maxContourRect.X;
                        var offsetY = maxContourRect.Y;

                        using (var toGetLx = imageHbSrc.Clone(maxContourRect))
                        {
                            var maxLx = double.MinValue;
                            Point maxLxPoint = default;

                            for (var x = 0; x <= toGetLx.Width; x += 3)
                            {
                                for (var y = 0; y <= toGetLx.Height; y += 3)
                                {
                                    if (x <= toGetLx.Width && y <= toGetLx.Height &&
                                        x + 3 <= toGetLx.Width && y + 3 <= toGetLx.Height)
                                    {
                                        var rect = new Rect(new Point(x, y), new Size(3, 3));
                                        var mask = new Mat(toGetLx, rect);
                                        var lx = GetGray(mask);

                                        if (lx >= maxLx)
                                        {
                                            maxLx = lx;
                                            maxLxPoint = new Point(x, y);
                                        }
                                    }
                                }
                            }

                            if (maxLxPoint != default)
                            {
                                hbHv = new Point(maxLxPoint.X + offsetX, maxLxPoint.Y + offsetY);
                                if (toGetLx.Channels() == 1)
                                    Cv2.CvtColor(toGetLx, toGetLx, ColorConversionCodes.GRAY2BGR);
                                Cv2.Circle(toGetLx, maxLxPoint, 1, Scalar.Purple, -1);
                                AppendImage("EMax", toGetLx);

                                using (var systemMat = imageHbSrc.Clone())
                                {
                                    var roiRect = GetGrayRoiByDegree(hbHv, 0, 0, 12.5, 6);

                                    if (roiRect.X >= 0 && roiRect.Y >= 0 && roiRect.X + roiRect.Width <= systemMat.Width && roiRect.Y <= systemMat.Height)
                                    {
                                        // 计算HV点的像素坐标,并根据HV点建立坐标系
                                        {
                                            var pH0V0 = hbHv;
                                            Cv2.Line(systemMat, new Point(0, pH0V0.Y), new Point(srcWidth, pH0V0.Y), Scalar.Yellow);
                                            Cv2.Line(systemMat, new Point(pH0V0.X, 0), new Point(pH0V0.X, srcHeight), Scalar.Yellow);

                                            // 画出HV点和拐点位置
                                            Cv2.Circle(systemMat, pH0V0, 1, Scalar.Green, -1);
                                        }

                                        var toCheckHbPoint = new Dictionary<string, Point2f>
                                        {
                                            { "EMax", new Point2f(0, 0) },
                                            //{ "H1", new Point2f(-11, 0) },
                                            //{ "H2", new Point2f(-5.14f, 0) },
                                            //{ "H3", new Point2f(5.14f, 0) },
                                            //{ "H4", new Point2f(11, 0) },
                                            //{ "H5", new Point2f(0, 5.5f) },

                                            { "12L-H", new Point2f(-12f, 0f) },
                                            { "9L-H", new Point2f(-9f, 0f) },
                                            { "6L-H", new Point2f(-6f,0f) },
                                            { "3L-H", new Point2f(-3f,0f) },
                                            { "3R-H", new Point2f(3f, 0f) },
                                            { "6R-H", new Point2f(6f, 0f) },
                                            { "9R-H", new Point2f(9f, 0f) },
                                            { "12R-H", new Point2f(12f, 0f) },
                                            { "V-2U", new Point2f(0f, 2f) },
                                        };

                                        // 计算各法规点的灰度值
                                        var dicGray = new Dictionary<string, double>();
                                        {
                                            var listPurplePoints = new List<Point>();
                                            foreach (var item in toCheckHbPoint)
                                            {
                                                var pixelPoint = DegreeToPixelPoint(hbHv, item.Value.X, item.Value.Y, true);
                                                listPurplePoints.Add(pixelPoint);

                                                var thisPointRoi = GetGrayRoiByDegree(hbHv, item.Value.X, item.Value.Y, 0.1, 0.1, true);
                                                var lxRoi = new Mat(imageHbSrc, thisPointRoi);
                                                var lx = GetGray(lxRoi);
                                                dicGray.Add(item.Key, lx);
                                                lxRoi.Dispose();

                                                Cv2.Rectangle(systemMat, thisPointRoi, Scalar.Red);
                                                Cv2.Circle(systemMat, pixelPoint, 1, Scalar.Red, -1);
                                                Cv2.PutText(systemMat, string.Format("{0}: '{1}'", item.Key, lx), new Point(thisPointRoi.X + thisPointRoi.Width + 1, thisPointRoi.Y + 2), HersheyFonts.HersheySimplex, 0.4, Scalar.Red);
                                            }
                                            foreach (var p in listPurplePoints)
                                                Cv2.Circle(systemMat, p, 1, Scalar.Purple, -1);
                                        }

                                        Cv2.Rectangle(systemMat, roiRect, Scalar.Red);

                                        using (var dstSystemMat = systemMat.Clone(roiRect))
                                        {
                                            // 分析完成,输出结果
                                            {
                                                AnalysisResultHb = "Finish";
                                                AnalysisResultHbBase64String = MyCamera.BitmapToBase64String(dstSystemMat.ToBitmap());

                                                GrayEMaxHb = dicGray["EMax"];

                                                Gray12LH = dicGray["12L-H"];
                                                Gray9LH = dicGray["9L-H"];
                                                Gra6LH = dicGray["6L-H"];
                                                Gray3LH = dicGray["3L-H"];
                                                Gray3RH = dicGray["3R-H"];
                                                Gray6RH = dicGray["6R-H"];
                                                Gray9RH = dicGray["9R-H"];
                                                Gray12RH = dicGray["12R-H"];
                                                GrayV2U = dicGray["V-2U"];
                                            }

                                            AppendImage("以HB最亮点作为HV建立坐标系", dstSystemMat);
                                            dstSystemMat.Dispose();
                                        }
                                    }
                                    else
                                    {
                                        AnalysisResultHb = string.Format("FAILED HB检测'H:-{0}°~{1}°','V:-{2}°~{3}°'范围超限", 12, 12, 6, 6);
                                        AnalysisResultHbBase64String = MyCamera.BitmapToBase64String(imageHbSrc.ToBitmap());
                                    }

                                    systemMat.Dispose();
                                }
                            }
                            else
                            {
                                AnalysisResultHb = "FAILED HB最亮点未找到";
                            }


                            toGetLx.Dispose();
                        }
                    }
                    else
                    {
                        AnalysisResultHb = "FAILED HB图像轮廓未找到";
                    }

                    toGetMaxRectSrc.Dispose();
                }

                imageSrcGray.Dispose();
                imageSrcClahe.Dispose();
                imageSrcGaussianBlur.Dispose();
                imageSrcOtsu.Dispose();
                imageSrcClose.Dispose();
                imageSrcCannyEdge.Dispose();

                imageHbSrc.Dispose();
            }

            if (!Images.ContainsKey(albSrcName) || Images[albSrcName] == null || Images[albSrcName].Empty())
            {
                AnalysisResultAlb = "FAILED ALB图像不存在";
                return;
            }

            if (hbHv != default)
            {
                using (var imageAlbSrc = Images[albSrcName].Clone())
                {
                    //var roiRect = GetGrayRoiByDegree(hbHv, 0, 0, 12, 5);

                    var p1 = DegreeToPixelPoint(hbHv, -12, 6, true);
                    var p2 = DegreeToPixelPoint(hbHv, 12, 6, true);
                    var p3 = DegreeToPixelPoint(hbHv, -12, 0, true);
                    //var p4 = DegreeToPixelPoint(hbHv, -12, 0, true);
                    var roiRect = new Rect(p1, new Size(p2.X - p1.X, p3.Y - p1.Y));

                    if (roiRect.X >= 0 && roiRect.Y >= 0 && roiRect.X + roiRect.Width <= imageAlbSrc.Width &&
                        roiRect.Y <= imageAlbSrc.Height)
                    {
                        var maxLx = double.MinValue;
                        Point maxLxPoint = default;

                        for (var x = roiRect.X; x <= imageAlbSrc.Width; x += 3)
                        {
                            for (var y = roiRect.Y; y <= imageAlbSrc.Height; y += 3)
                            {
                                if (x <= imageAlbSrc.Width && y <= imageAlbSrc.Height &&
                                    x + 3 <= imageAlbSrc.Width && y + 3 <= imageAlbSrc.Height)
                                {
                                    var rect = new Rect(new Point(x, y), new Size(3, 3));
                                    var mask = new Mat(imageAlbSrc, rect);
                                    var lx = GetGray(mask);

                                    if (lx >= maxLx)
                                    {
                                        maxLx = lx;
                                        maxLxPoint = new Point(x, y);
                                    }
                                }
                            }
                        }

                        if (maxLxPoint != default)
                        {
                            using (var systemMat = imageAlbSrc.Clone())
                            {
                                // 计算HV点的像素坐标,并根据HV点建立坐标系
                                {
                                    var pH0V0 = hbHv;
                                    Cv2.Line(systemMat, new Point(0, pH0V0.Y), new Point(systemMat.Width, pH0V0.Y), Scalar.Yellow);
                                    Cv2.Line(systemMat, new Point(pH0V0.X, 0), new Point(pH0V0.X, systemMat.Height), Scalar.Yellow);

                                    // 画出HV点和拐点位置
                                    Cv2.Circle(systemMat, pH0V0, 1, Scalar.Green, -1);
                                    Cv2.Circle(systemMat, maxLxPoint, 10, Scalar.Purple);
                                }

                                using (var dstSystemMat = systemMat.Clone(roiRect))
                                {
                                    // 分析完成,输出结果
                                    {
                                        AnalysisResultAlb = "Finish";
                                        GrayAlb = maxLx;
                                    }

                                    AppendImage("以HB最亮点作为HV建立坐标系ALB图像", dstSystemMat);
                                    dstSystemMat.Dispose();
                                }

                                systemMat.Dispose();
                            }
                        }
                        else
                        {
                            AnalysisResultAlb = "FAILED 未找到ALB最亮点";
                        }
                    }
                    else
                    {
                        AnalysisResultAlb = string.Format("FAILED ALB检测'H:-{0}°~{1}°','V:-{2}°~{3}°'范围超限", 12, 12, 0, 6);
                    }
                }
            }
            else
            {
                AnalysisResultAlb = "FAILED 未通过HB图像找到最亮点作为HV，无法检测ALB";
                return;
            }
        }

        #endregion

        #region 通用计算方法

        /// <summary>
        /// 将图像预处理
        /// </summary>
        /// <param name="imageSrc">原始图像</param>
        /// <param name="imageGray">灰度图像</param>
        /// <param name="imageClahe">自适应直方图均衡化图像</param>
        /// <param name="imageGaussianBlur">高斯滤波降噪图像</param>
        /// <param name="imageOtsu">OTSU图像</param>
        /// <param name="otsuThresholdValue">OTSU阈值</param>
        /// <param name="imageClose">形态学闭操作（连接断裂边缘）图像</param>
        /// <param name="imageCannyEdge">边缘处理图像</param>
        public void ImageDeNoise(
            Mat imageSrc, out Mat imageGray, out Mat imageClahe, out Mat imageGaussianBlur, out Mat imageOtsu, out double otsuThresholdValue, out Mat imageClose, out Mat imageCannyEdge)
        {
            imageGray = new Mat();
            imageClahe = new Mat();
            imageGaussianBlur = new Mat();
            imageOtsu = new Mat();
            imageClose = new Mat();
            imageCannyEdge = new Mat();

            using (var roi = imageSrc.Clone())
            {
                if (roi.Channels() == 3)
                    Cv2.CvtColor(roi, imageGray, ColorConversionCodes.BGR2GRAY);

                // 自适应直方图均衡化（CLAHE）
                var clahe = Cv2.CreateCLAHE(clipLimit: 2.0, tileGridSize: new Size(8, 8));
                clahe.Apply(imageGray, imageClahe);

                // 高斯滤波降噪
                Cv2.GaussianBlur(imageClahe, imageGaussianBlur, new Size(5, 5), sigmaX: 1.5);

                // 备份：中值滤波+膨胀+腐蚀
                {
                    // 定义结构元素，这里使用5x5大小的矩形结构元素
                    //var element = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5), new Point(-1, -1));
                    // 中值滤波，去除噪声
                    //Cv2.MedianBlur(roi, roi, ksize: 5); // ksize必须是大于1的奇数
                    // 膨胀操作
                    //Cv2.Dilate(roi, roi, element, iterations: 1);
                    // 腐蚀操作
                    //Cv2.Erode(roi, roi, element, iterations: 1);
                }

                otsuThresholdValue = Cv2.Threshold(imageGaussianBlur, imageOtsu, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);

                // 形态学闭操作（连接断裂边缘）
                var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(21, 3));
                Cv2.MorphologyEx(imageOtsu, imageClose, MorphTypes.Close, kernel);

                //Cv2.Canny(roi, roiEdge, roiOtsu / 2, roiOtsu);
                Cv2.Canny(imageClose, imageCannyEdge, 50, 150);
            }
        }

        private static double GetGray(Mat mask)
        {
            double lx;
            if (mask.Channels() == 3)
            {
                //// 分离HSV通道
                //var hsvChannels = Cv2.Split(mask);
                //var mean = Cv2.Mean(hsvChannels[2]);
                //lx = Math.Round(mean.Val0, 0, MidpointRounding.AwayFromZero);

                //Cv2.CvtColor(mask, mask, ColorConversionCodes.BGR2GRAY);
                //var mean = Cv2.Mean(mask);
                //lx = Math.Round(mean.Val0, 0, MidpointRounding.AwayFromZero);

                var mean = Cv2.Mean(mask);
                lx = MyCamera.GetLxByRgb(mean);

                //var b = mean.Val0;
                //var g = mean.Val1;
                //var r = mean.Val2;
                //var k = 0.547373141f;
                //lx = Math.Round(Math.Pow(Math.Pow(r / 255.0f, 2.2f) + Math.Pow(g / 170.0f, 2.2f) + Math.Pow(b / 425.0f, 2.2f), 1 / 2.2f) * k, 2);
            }
            else
            {
                Cv2.CvtColor(mask, mask, ColorConversionCodes.BGR2GRAY);
                var mean = Cv2.Mean(mask);
                lx = Math.Round(mean.Val0, 0, MidpointRounding.AwayFromZero);
            }

            return lx;
        }

        private static LineSegmentPoint GetLine(List<Point> points, int cols)
        {
            var lineParams = Cv2.FitLine(points, DistanceTypes.L2, 0, 0.01, 0.01);
            var vx = lineParams.Vx;
            var vy = lineParams.Vy;
            var x = lineParams.X1;
            var y = lineParams.Y1;
            var leftY = (int)(-x * vy / vx + y);
            var rightY = (int)((cols - x) * vy / vx + y);

            var line = new LineSegmentPoint(new Point(0, leftY), new Point(cols - 1, rightY));
            return line;
        }

        private static Point GetIntersection(Point a1, Point a2, Point b1, Point b2)
        {
            double deltaA = a2.X - a1.X;
            double deltaB = b2.X - b1.X;

            if (deltaA == 0 || deltaB == 0) // Check if either line is vertical
            {
                // If both lines are vertical, they are parallel or coincide
                if (Math.Abs(deltaA - deltaB) <= 0)
                    return default;
            }

            // Line A represented as a1x + a2y = c1
            double a1Line = a2.Y - a1.Y;
            double a2Line = a1.X - a2.X;
            var c1Line = a1Line * a1.X + a2Line * a1.Y;

            // Line B represented as b1x + b2y = c2
            double b1Line = b2.Y - b1.Y;
            double b2Line = b1.X - b2.X;
            var c2Line = b1Line * b1.X + b2Line * b1.Y;

            // Calculate the determinant of the coefficient matrix
            var det = a1Line * b2Line - b1Line * a2Line;

            if (det == 0) // Lines are parallel
                return default;

            // Calculate intersection point using Cramer's Rule
            var x = (b2Line * c1Line - a2Line * c2Line) / det;
            var y = (a1Line * c2Line - b1Line * c1Line) / det;

            return new Point((int)x, (int)y);
        }

        private static double GetRad(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        private static double GetDegrees(double rad)
        {
            return Math.Round(rad * (180 / Math.PI), 2, MidpointRounding.AwayFromZero);
        }

        private Rect GetGrayRoiByDegree(Point inflectionPoint,
            double degreeH,
            double degreeV,
            double offsetDegreeH = 0.02d,
            double offsetDegreeV = 0.02d,
            bool isInflectionPointHv = false)
        {
            if (offsetDegreeH <= 0.1 || offsetDegreeV <= 0.1)
            {
                var center = DegreeToPixelPoint(inflectionPoint, degreeH, degreeV, isInflectionPointHv);
                return new Rect(center.X - 1, center.Y - 1, 3, 3);
            }

            var leftTop = DegreeToPixelPoint(inflectionPoint, degreeH - offsetDegreeH, degreeV + offsetDegreeV, isInflectionPointHv);
            var rightBottom = DegreeToPixelPoint(inflectionPoint, degreeH + offsetDegreeH, degreeV - offsetDegreeV, isInflectionPointHv);

            var top = new Point(leftTop.X, leftTop.Y);
            var width = rightBottom.X - leftTop.X;
            var height = rightBottom.Y - leftTop.Y;

            if (width < 3)
            {
                width = 3;
            }

            if (height < 3)
            {
                height = 3;
            }

            var roi = new Rect(top, new Size(width, height));
            return roi;
        }

        private Point DegreeToPixelPoint(Point inflectionPoint, double degreeH, double degreeV, bool isInflectionPointHv = false)
        {
            var distance = CalculateFresnelLensDistancePixel();

            var pH0V0 = inflectionPoint;
            if (!isInflectionPointHv)
            {
                var pixel0d57 = (int)(Math.Tan(Math.Abs(GetRad(0.57d))) * distance);
                pH0V0 = new Point(inflectionPoint.X, inflectionPoint.Y - pixel0d57);
            }

            var offsetHPixel = (int)(Math.Tan(Math.Abs(GetRad(degreeH))) * distance);
            var offsetVPixel = (int)(Math.Tan(Math.Abs(GetRad(degreeV))) * distance);

            var returnP = pH0V0;

            if (degreeH > 0)
                returnP.X = pH0V0.X + offsetHPixel;
            else if (degreeH < 0)
                returnP.X = pH0V0.X - offsetHPixel;

            if (degreeV > 0)
                returnP.Y = pH0V0.Y - offsetVPixel;
            else if (degreeV < 0)
                returnP.Y = pH0V0.Y + offsetVPixel;

            return returnP;
        }

        private PointF PixelPointToDegree(Point inflectionPoint, Point pixelPoint, bool isInflectionPointHv = false)
        {
            var distance = CalculateFresnelLensDistancePixel();

            var pH0V0 = inflectionPoint;
            if (!isInflectionPointHv)
            {
                var pixel0d57 = (int)(Math.Tan(Math.Abs(GetRad(0.57d))) * distance);
                pH0V0 = new Point(inflectionPoint.X, inflectionPoint.Y - pixel0d57);
            }

            var offsetPixelH = Math.Abs(pixelPoint.X - pH0V0.X);
            var offsetPixelV = Math.Abs(pixelPoint.Y - pH0V0.Y);

            var offsetHDegree = GetDegrees(Math.Asin(offsetPixelH / distance));
            var offsetVDegree = GetDegrees(Math.Asin(offsetPixelV / distance));

            var returnPf = new PointF(0, 0);

            if (pixelPoint.X > pH0V0.X)
                returnPf.X += (float)Math.Round(offsetHDegree, 2, MidpointRounding.AwayFromZero);
            else if (pixelPoint.X < pH0V0.X)
                returnPf.X -= (float)Math.Round(offsetHDegree, 2, MidpointRounding.AwayFromZero);

            if (pixelPoint.Y > pH0V0.Y)
                returnPf.Y -= (float)Math.Round(offsetVDegree, 2, MidpointRounding.AwayFromZero);
            else if (pixelPoint.Y < pH0V0.Y)
                returnPf.Y += (float)Math.Round(offsetVDegree, 2, MidpointRounding.AwayFromZero);

            return returnPf;
        }

        private double GetPointDistance(Point p1, Point p2)
        {
            // 定义两个点的坐标
            float x1 = p1.X;
            float y1 = p1.Y;
            float x2 = p2.X;
            float y2 = p2.Y;

            // 手动计算距离
            double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            return distance;
        }

        /// <summary>
        /// 计算像素-实际长度比例系数
        /// </summary>
        /// <param name="pixelSizeMicrons">像元尺寸（微米）</param>
        /// <param name="focalLengthMm">焦距（毫米）</param>
        /// <param name="objectDistanceM">物距（米）</param>
        /// <returns>比例系数（毫米/像素）</returns>
        public static double CalculateScaleRatio(double pixelSizeMicrons, double focalLengthMm, double objectDistanceM)
        {
            // 单位转换
            var s = pixelSizeMicrons * 1e-3; // 微米 -> 毫米
            var f = focalLengthMm;
            var u = objectDistanceM * 1000; // 米 -> 毫米

            return s * u / f;
        }

        public double CalculateFresnelLensDistancePixel()
        {
            var scaleRatio = CalculateScaleRatio(PixelScale, Ratio / 1000f, CameraDistanceCm * 0.01f);
            var perMm = 1 / scaleRatio;
            var distance = perMm * FresnelLensDistanceCm * 10f;

            distance = Math.Round(distance, 2, MidpointRounding.AwayFromZero);

            return distance;
        }

        public static double CalculateRotAngle(Point pointA, Point pointB)
        {
            double dx = pointB.X - pointA.X;
            double dy = pointB.Y - pointA.Y;

            double angleRadians = Math.Atan2(dy, dx);
            double angleDegrees = angleRadians * (180.0 / Math.PI);

            if (angleDegrees < 0)
            {
                angleDegrees += 360;
            }

            if (angleDegrees > 180)
            {
                angleDegrees -= 360;
            }

            return angleDegrees;
        }

        #endregion

        #region 查前道追溯

        public string ToCheckBarcode = string.Empty;

        public string TrackInfo = string.Empty;

        public string ServerIp = "192.168.2.108";
        public string ServerDataBase = "IPMS";
        public string ServerUid = "sa";
        public string ServerPwd = "123456";

        public void ReadTrack()
        {
            TrackInfo = string.Empty;
            if (string.IsNullOrEmpty(ToCheckBarcode))
                return;

            var existSql = string.Format("select count(1) from manufactureCheckData where checkData like '%{0}%'", ToCheckBarcode);

            if (Exists(existSql.ToString(), null))
            {
                TrackInfo = "OK ";
            }
            else
            {
                TrackInfo = "NG";
            }
        }

        private bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            var obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if (Equals(obj, null) || Equals(obj, DBNull.Value))
                cmdresult = 0;
            else
                cmdresult = int.Parse(obj.ToString());
            return cmdresult != 0;
        }

        public object GetSingle(string sqlString, params SqlParameter[] cmdParms)
        {
            var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
                ServerUid, ServerPwd);

            using (var connection = new SqlConnection(sqlConnectiong))
            {
                using (var cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        var obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if (Equals(obj, null) || Equals(obj, DBNull.Value))
                            return null;
                        return obj;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        private static void PrepareCommand(
           SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms == null)
                return;
            foreach (var parameter in cmdParms)
            {
                if ((parameter.Direction == ParameterDirection.InputOutput ||
                     parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                    parameter.Value = DBNull.Value;
                cmd.Parameters.Add(parameter);
            }
        }

        private DataSet Query(string sqlString)
        {
            var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
                ServerUid, ServerPwd);

            using (var connection = new SqlConnection(sqlConnectiong))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(sqlString, connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        #endregion
    }
}
