using CommonUtility.HikSdk;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;
using PointF = OpenCvSharp.Point2f;

namespace Controller
{
    public sealed class FogVisionAnalysis : ControllerBase
    {
        [Description("R,FOG分析结果")] public string AnalysisResultFog = string.Empty;

        public string AnalysisResultBase64String = string.Empty;
        public double EMax = double.MinValue;
        public double V_5U = double.MinValue;
        public double V_5B = double.MinValue;
        public double H_10L = double.MinValue;
        public double H_10R = double.MinValue;

        [Description("R/W,当前使用相机SN号")] public string CameraSn = string.Empty;

        [Description("R/W,当前使用相机的镜头大小-通常采用的4mm即4000um")]
        public double Ratio = 8000;

        [Description("R/W,当前使用相机像元大小-通常采用的是2.2um")]
        public double PixelScale = 2.2;

        [Description("R/W,相机物距")] public double CameraDistanceCm = 41;

        [Description("R/W,菲涅尔透镜物距")] public double FresnelLensDistanceCm = 64;

        public FogVisionAnalysis(string name) : base(name)
        {
            Images = new Dictionary<string, Mat>();
            //AnalyzeFog("FOG光型");
        }

        ~FogVisionAnalysis() => Dispose();

        public Dictionary<string, Mat> Images { get; set; }

        public void AnalyzeFog(string fogSrcName)
        {
            AnalysisResultFog = string.Empty;
            AnalysisResultBase64String = string.Empty;
            EMax = double.MinValue;
            V_5U = double.MinValue;
            V_5B = double.MinValue;
            H_10L = double.MinValue;
            H_10R = double.MinValue;

            //AppendImage("FOG光型", Cv2.ImRead(@"D:\资料\Projects\3UG\3UG雾灯\光型\Image_20260316103748936_41CM_6000曝光.bmp"));

            if (!Images.ContainsKey(fogSrcName) || Images[fogSrcName] == null || Images[fogSrcName].Empty())
            {
                AnalysisResultFog = "FAILED FOG图像不存在";
                return;
            }

            Point hbHv = default;

            try
            {
                using (var imageFogSrc = Images[fogSrcName].Clone())
                {
                    var srcWidth = imageFogSrc.Width;
                    var srcHeight = imageFogSrc.Height;

                    Mat imageSrcGray;
                    Mat imageSrcClahe;
                    Mat imageSrcGaussianBlur;
                    Mat imageSrcOtsu;
                    double imageSrcOtsuThresholdValue;
                    Mat imageSrcClose;
                    Mat imageSrcCannyEdge;
                    ImageDeNoise(imageFogSrc, out imageSrcGray, out imageSrcClahe, out imageSrcGaussianBlur,
                        out imageSrcOtsu, out imageSrcOtsuThresholdValue, out imageSrcClose, out imageSrcCannyEdge);

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

                            using (var toGetLx = imageFogSrc.Clone(maxContourRect))
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

                                    using (var systemMat = imageFogSrc.Clone())
                                    {
                                        var roiRect = GetGrayRoiByDegree(hbHv, 0, 0, 11, 6);

                                        if (roiRect.X >= 0 && roiRect.Y >= 0 &&
                                            roiRect.X + roiRect.Width <= systemMat.Width &&
                                            roiRect.Y <= systemMat.Height)
                                        {
                                            // 计算HV点的像素坐标,并根据HV点建立坐标系
                                            {
                                                var pH0V0 = hbHv;
                                                Cv2.Line(systemMat, new Point(0, pH0V0.Y), new Point(srcWidth, pH0V0.Y),
                                                    Scalar.Yellow);
                                                Cv2.Line(systemMat, new Point(pH0V0.X, 0),
                                                    new Point(pH0V0.X, srcHeight),
                                                    Scalar.Yellow);

                                                // 画出HV点和拐点位置
                                                Cv2.Circle(systemMat, pH0V0, 1, Scalar.Green, -1);
                                            }

                                            var toCheckHbPoint = new Dictionary<string, Point2f>
                                            {
                                                { "EMax", new Point2f(0, 0) },
                                                { "V-5U", new Point2f(0f, 5f) },
                                                { "V-5B", new Point2f(0f, -5f) },
                                                { "H-10L", new Point2f(10f, 0f) },
                                                { "H-10R", new Point2f(-10f, 0f) },
                                            };

                                            // 计算各法规点的灰度值
                                            var dicGray = new Dictionary<string, double>();
                                            {
                                                var listPurplePoints = new List<Point>();
                                                foreach (var item in toCheckHbPoint)
                                                {
                                                    var pixelPoint = DegreeToPixelPoint(hbHv, item.Value.X,
                                                        item.Value.Y,
                                                        true);
                                                    listPurplePoints.Add(pixelPoint);

                                                    var thisPointRoi = GetGrayRoiByDegree(hbHv, item.Value.X,
                                                        item.Value.Y,
                                                        0.1, 0.1, true);
                                                    var lxRoi = new Mat(imageFogSrc, thisPointRoi);
                                                    var lx = GetGray(lxRoi);
                                                    dicGray.Add(item.Key, lx);
                                                    lxRoi.Dispose();

                                                    Cv2.Rectangle(systemMat, thisPointRoi, Scalar.Blue);
                                                    Cv2.Circle(systemMat, pixelPoint, 1, Scalar.Blue, -1);
                                                    Cv2.PutText(systemMat, string.Format("{0}: '{1}'", item.Key, lx),
                                                        new Point(thisPointRoi.X + thisPointRoi.Width + 1,
                                                            thisPointRoi.Y + 2), HersheyFonts.HersheySimplex, 0.4,
                                                        Scalar.Blue);
                                                }

                                                foreach (var p in listPurplePoints)
                                                    Cv2.Circle(systemMat, p, 1, Scalar.Purple, -1);
                                            }

                                            Cv2.Rectangle(systemMat, roiRect, Scalar.Blue);

                                            using (var dstSystemMat = systemMat.Clone(roiRect))
                                            {
                                                // 分析完成,输出结果
                                                {
                                                    AnalysisResultFog = "Finish";
                                                    //AnalysisResultHbBase64String = MyCamera.BitmapToBase64String(dstSystemMat.ToBitmap());

                                                    EMax = dicGray["EMax"];
                                                    V_5U = dicGray["V-5U"];
                                                    V_5B = dicGray["V-5B"];
                                                    H_10L = dicGray["H-10L"];
                                                    H_10R = dicGray["H-10R"];
                                                }

                                                Cv2.Resize(dstSystemMat, dstSystemMat, new Size(800, 600));
                                                //dstSystemMat.ImWrite(@"D:\资料\Projects\3UG\3UG雾灯\光型\RESULT.BMP");

                                                AppendImage("以FOG最亮点作为HV建立坐标系", dstSystemMat);
                                                dstSystemMat.Dispose();
                                            }
                                        }
                                        else
                                        {
                                            AnalysisResultFog =
                                                string.Format("FAILED FOG检测'H:-{0}°~{1}°','V:-{2}°~{3}°'范围超限", 12, 12,
                                                    6,
                                                    6);
                                            AnalysisResultBase64String =
                                                MyCamera.BitmapToBase64String(imageFogSrc.ToBitmap());
                                        }

                                        systemMat.Dispose();
                                    }
                                }
                                else
                                {
                                    AnalysisResultFog = "FAILED FOG最亮点未找到";
                                }


                                toGetLx.Dispose();
                            }
                        }
                        else
                        {
                            AnalysisResultFog = "FAILED FOG图像轮廓未找到";
                        }

                        toGetMaxRectSrc.Dispose();
                    }

                    imageSrcGray.Dispose();
                    imageSrcClahe.Dispose();
                    imageSrcGaussianBlur.Dispose();
                    imageSrcOtsu.Dispose();
                    imageSrcClose.Dispose();
                    imageSrcCannyEdge.Dispose();

                    imageFogSrc.Dispose();
                }
            }
            catch (Exception e)
            {
                AnalysisResultFog =
                    string.Format("NG 图像处理异常：{0}", e.Message);
                AnalysisResultBase64String = string.Empty;
            }
        }

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
            Mat imageSrc, out Mat imageGray, out Mat imageClahe, out Mat imageGaussianBlur, out Mat imageOtsu,
            out double otsuThresholdValue, out Mat imageClose, out Mat imageCannyEdge)
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

                otsuThresholdValue = Cv2.Threshold(imageGaussianBlur, imageOtsu, 0, 255,
                    ThresholdTypes.Otsu | ThresholdTypes.Binary);

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

                using (var tp = mask.Clone())
                {
                    Cv2.CvtColor(tp, tp, ColorConversionCodes.BGR2GRAY);
                    var mean = Cv2.Mean(tp);
                    lx = Math.Round(mean.Val0, 0, MidpointRounding.AwayFromZero);
                }

                //var mean = Cv2.Mean(mask);
                //lx = MyCamera.GetLxByRgb(mean);

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

            var leftTop = DegreeToPixelPoint(inflectionPoint, degreeH - offsetDegreeH, degreeV + offsetDegreeV,
                isInflectionPointHv);
            var rightBottom = DegreeToPixelPoint(inflectionPoint, degreeH + offsetDegreeH, degreeV - offsetDegreeV,
                isInflectionPointHv);

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

        private Point DegreeToPixelPoint(Point inflectionPoint, double degreeH, double degreeV,
            bool isInflectionPointHv = false)
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

        private readonly CameraControl _cameraControl = new CameraControl();

        public void OpenCamera(uint exposeTime)
        {
            if (exposeTime == 0)
                return;

            _cameraControl.DeviceListAcq();
            var device = _cameraControl.CameraList.Find(f =>
                string.Equals(f.GigeInfo.chSerialNumber, CameraSn, StringComparison.CurrentCultureIgnoreCase));

            if (device is null)
                return;

            device.OpenCamera();
            device.SetExposureTime((int)exposeTime);
            Thread.Sleep(250);
        }

        public void CaptureImage(string name, int captureCount)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (string.IsNullOrEmpty(CameraSn))
                return;

            var device = _cameraControl.CameraList.Find(f =>
                string.Equals(f.GigeInfo.chSerialNumber, CameraSn, StringComparison.CurrentCultureIgnoreCase));

            if (device != null)
            {
                //device.OpenCamera();
                //device.SetExposureTime((int)exposeTime);
                //Thread.Sleep(250);
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
            }
        }

        public void CloseCamera()
        {
            _cameraControl.DeviceListAcq();
            var device = _cameraControl.CameraList.Find(f =>
                string.Equals(f.GigeInfo.chSerialNumber, CameraSn, StringComparison.CurrentCultureIgnoreCase));

            if (device is null)
                return;

            device.ClearBuffer();
            device.CloseCamera();
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
    }
}
