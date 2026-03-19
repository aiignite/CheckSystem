using CommonUtility;
using CommonUtility.HikSdk;
using MathNet.Numerics;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Point = OpenCvSharp.Point;
using PointF = OpenCvSharp.Point2f;
using Size = OpenCvSharp.Size;

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

    public sealed class PesVisionAnalysisNDLB : ControllerBase
    {
        [Description("R/W,当前使用相机SN号")]
        public string CameraSn = string.Empty;

        [Description("R/W,当前使用相机的镜头大小-通常采用的4mm即4000um")]
        public double Ratio = 6000;

        [Description("R/W,当前使用相机像元大小-通常采用的是2.2um")]
        public double PixelScale = 2.4;

        [Description("R/W,相机物距")]
        public double CameraDistanceCm = 57.7;

        [Description("R/W,菲涅尔透镜物距")]
        public double FresnelLensDistanceCm = 64;

        [Description("R/W,模板匹配路徑")]
        public string MatchTempFilePath = @"E:\Projects\MyOpenCv\MyOpenCv\template.bmp";

        public PesVisionAnalysisNDLB(string name) : base(name)
        {
            Images = new Dictionary<string, Mat>();

            _lbTestPosDegree.Add("B50L", new Point2f(-3.43f, 0.57f));
            _lbTestPosDegree.Add("BR", new Point2f(2.5f, 1f));
            _lbTestLine.Add("BLL", Tuple.Create(new Point2f(-16f, 0.57f), new Point2f(-8f, 0.57f)));
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
            _lbTestLine.Add("LINE15", Tuple.Create(new Point2f(-16f, -2.86f), new Point2f(16f, -2.86f)));
            _lbTestLine.Add("LINE10", Tuple.Create(new Point2f(-4.5f, -4f), new Point2f(2f, -4f)));

            _lbTestPoly.Add("Area3A", new Point2f[] { new Point2f(-8f, 1f), new Point2f(-8f, 4f), new Point2f(8f, 4f), new Point2f(8f, 2f), new Point2f(6f, 1.5f), new Point2f(1.5f, 1.5f), new Point2f(0f, 0f), new Point2f(-4f, 0f), });
            _lbTestPoly.Add("AreaLine10Bottom", new Point2f[] { new Point2f(-4.5f, -4f), new Point2f(2f, -4f), new Point2f(2f, -4.95f), new Point2f(-4.5f, -4.95f), });
        }

        ~PesVisionAnalysisNDLB()
        {
            _cameraControl.Dispose();
            Dispose();
        }

        public string ToSaveImgBarcode = string.Empty;
        private readonly long _enterTs = HighPrecisionTimer.GetTimestamp();

        public void SaveImg()
        {
            try
            {
                var ts = HighPrecisionTimer.GetTimestamp().ToString();
                string name;
                if (!string.IsNullOrEmpty(Name))
                    name = Name;
                else
                    name = _enterTs + "_" + Name;
                name += "_保存图像";
                var driveLetter = Path.GetPathRoot(Directory.GetCurrentDirectory());
                var date = DateTime.Now.ToString("yyyyMMdd");
                var folder = string.Format(@"{0}/{1}", driveLetter, name);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileCount = Directory.GetDirectories(folder);
                if (fileCount.Length > 250)
                {
                    var di = new DirectoryInfo(folder);
                    var files = di.GetDirectories();
                    foreach (var file in files)
                        file.Delete(true);
                }

                string barcode;
                try
                {
                    barcode = ToSaveImgBarcode.Substring(39, 16);
                    barcode = barcode.Replace("?", "X");
                }
                catch (Exception e)
                {
                    barcode = string.Empty;
                }

                var folder1 = string.Format(@"{0}/{1}/{2}", driveLetter, name, date + (string.IsNullOrEmpty(barcode) ? _enterTs.ToString() : "_" + barcode));

                if (!Directory.Exists(folder1))
                    Directory.CreateDirectory(folder1);

                var folder2 = string.Format(@"{0}/{1}", folder1, ts);
                if (!Directory.Exists(folder2))
                    Directory.CreateDirectory(folder2);

                var index = 0;
                foreach (var t in Images.Where(t => t.Value != null && !t.Value.Empty()))
                {
                    var jpg = string.Format(@"{0}/{1}_{2}_{3}.jpeg", folder2, index++, t.Key, string.IsNullOrEmpty(barcode) ? _enterTs.ToString() : barcode);
                    if (!t.Value.ImWrite(jpg))
                    {
                        jpg = string.Format(@"{0}/{1}_{2}.jpeg", folder2, index++, t.Key);
                        t.Value.ImWrite(jpg);
                    }
                }

                ToSaveImgBarcode = string.Empty;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #region 图像获取相关

        private readonly CameraControl _cameraControl = new CameraControl();
        public Dictionary<string, Mat> Images { get; set; }

        public void StartGrab()
        {
            //if (SelectedCamera == null)
            //    return;
            //SelectedCamera.SetExposureTime(LowBeamShutter1);
            //Thread.Sleep(400);
            //SelectedCamera.StartGrab();

            _cameraControl.DeviceListAcq();
            var device = _cameraControl.CameraList.Find(f =>
                string.Equals(f.GigeInfo.chSerialNumber, CameraSn, StringComparison.CurrentCultureIgnoreCase));

            if (device != null)
            {
                device.OpenCamera();
                device.SetExposureTime((int)2000);
                device.StartGrab();
            }
        }

        public void StopGrab()
        {
            _cameraControl.DeviceListAcq();
            var device = _cameraControl.CameraList.Find(f =>
                string.Equals(f.GigeInfo.chSerialNumber, CameraSn, StringComparison.CurrentCultureIgnoreCase));

            if (device != null)
            {
                device.StopGrab();
                device.ClearBuffer();
                device.CloseCamera();
            }
        }

        public void CaptureImage(string name, uint exposeTime)
        {
            //Thread.Sleep(50);
            //if (name == "近光原图")
            //{
            //    using (var mat = Cv2.ImRead(@"E:\Projects\NDLB_PES\测试结果-20250529\#9\Image_20250529132514397-3000.bmp"))
            //    {
            //        AppendImage("近光原图", mat);
            //    }
            //    return;
            //}
            //else if (name == "远光原图")
            //{
            //    using (var mat = Cv2.ImRead(@"E:\Projects\NDLB_PES\测试结果-20250529\#9\Image_20250529132536934-2000.bmp"))
            //    {
            //        AppendImage("远光原图", mat);
            //    }
            //    return;
            //}

            if (string.IsNullOrEmpty(name))
                return;

            if (string.IsNullOrEmpty(CameraSn))
                return;

            if (exposeTime == 0)
                return;

            var captureCount = 1;

            _cameraControl.DeviceListAcq();
            var device = _cameraControl.CameraList.Find(f =>
                string.Equals(f.GigeInfo.chSerialNumber, CameraSn, StringComparison.CurrentCultureIgnoreCase));

            if (device != null)
            {
                device.OpenCamera();
                device.SetExposureTime((int)exposeTime);
                Thread.Sleep(500);
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
                else
                {
                    Thread.Sleep(500);
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

                device.ClearBuffer();
                //device.CloseCamera();
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

        private readonly Dictionary<string, Point2f> _lbTestPosDegree = new Dictionary<string, Point2f>();
        private readonly List<Dictionary<string, Point2f>> _lbTestPosDegree2 = new List<Dictionary<string, PointF>>();
        private readonly Dictionary<string, Tuple<Point2f, Point2f>> _lbTestLine = new Dictionary<string, Tuple<Point2f, Point2f>>();
        private readonly Dictionary<string, Point2f[]> _lbTestPoly = new Dictionary<string, PointF[]>();

        #region 分析过程GB4599,以LB为基准

        public int InflectionPixelPointX = int.MinValue;
        public int InflectionPixelPointY = int.MinValue;

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

            _inp = default(Point);

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

        public void AnalyzeImage(string srcLbName, string srcHbName)
        {
            ClearAnalysisResult();

            if (string.IsNullOrEmpty(srcLbName) || !Images.ContainsKey(srcLbName) || Images[srcLbName] == null || Images[srcLbName].Empty())
            {
                AnalysisResult = @"FAILED 近光图像不存在";
                return;
            }

            var pInflection = default(Point);

            using (var src = Images[srcLbName].Clone())
            {
                using (var grayMat = src.Clone())
                {
                    if (grayMat.Channels() != 1)
                        Cv2.CvtColor(grayMat, grayMat, ColorConversionCodes.BGR2GRAY);

                    using (var matchTemp = Cv2.ImRead(MatchTempFilePath))
                    {
                        if (matchTemp.Channels() != 1)
                            Cv2.CvtColor(matchTemp, matchTemp, ColorConversionCodes.BGR2GRAY);

                        //执行匹配
                        using (var result = new Mat())
                        {
                            var minValue = 0d;
                            var maxValue = 0d;
                            var minLocation = new Point();
                            var maxLocation = new Point();

                            Cv2.MatchTemplate(grayMat, matchTemp, result, TemplateMatchModes.CCoeffNormed);
                            Cv2.MinMaxLoc(result, out minValue, out maxValue, out minLocation, out maxLocation);

                            if (maxValue < 0.95)
                            {
                                AnalysisResult = @"FAILED 模板匹配失败，请检查模板是否正确";
                                return;
                            }

                            var matchLocation = new Point(maxLocation.X, maxLocation.Y);
                            var str = $"MatchResultMin:{minValue:F2}, MatchResultMax:{maxValue:F2}; X:{matchLocation.X}, Y:{matchLocation.Y}, Width:{matchTemp.Width}, Height:{matchTemp.Height}";
                            var matchRect = new Rect(matchLocation.X, matchLocation.Y, (int)matchTemp.Width, (int)matchTemp.Height);

                            // 显示模版匹配结果
                            using (var matchMat = src.Clone())
                            {
                                if (matchMat.Channels() == 1)
                                    Cv2.CvtColor(matchMat, matchMat, ColorConversionCodes.GRAY2BGR);
                                matchMat.Rectangle(matchRect, Scalar.Green);
                                matchMat.PutText(str, new Point(matchLocation.X + matchRect.Width + 5, matchLocation.Y + matchRect.Height + 5), HersheyFonts.HersheySimplex, 0.45, Scalar.Green);
                                AppendImage(@"近光匹配结果", matchMat);
                            }

                            using (var medianBlurMat = grayMat.Clone())
                            {
                                Cv2.MedianBlur(medianBlurMat, medianBlurMat, 5); // 使用5*5的中值滤波器
                                AppendImage(@"近光中值滤波", medianBlurMat);

                                using (var firstOtsu = medianBlurMat.Clone()) // 第一次otsu，取光型区域
                                {
                                    Cv2.Threshold(firstOtsu, firstOtsu, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
                                    AppendImage(@"第一次otsu取光型区域", firstOtsu);

                                    // 找到轮廓
                                    Point[][] contoursMax;
                                    HierarchyIndex[] hierarchyMax;
                                    Cv2.FindContours(firstOtsu, out contoursMax, out hierarchyMax, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                                    var maxMaxArea = 0d;
                                    var maxMaxIndex = -1;

                                    // 计算并绘制每个轮廓的几何中心
                                    var tempMaxcontourIndex = 0;
                                    foreach (var contour in contoursMax)
                                    {
                                        var area = Cv2.ContourArea(contour);

                                        if (area > maxMaxArea)
                                        {
                                            maxMaxArea = area;
                                            maxMaxIndex = tempMaxcontourIndex;
                                        }

                                        tempMaxcontourIndex++;
                                    }

                                    if (maxMaxIndex == -1)
                                    {
                                        AnalysisResult = @"FAILED 未找到光型区域";
                                        return;
                                    }

                                    var maxRect = Cv2.BoundingRect(contoursMax[maxMaxIndex]);

                                    if (maxRect.Top <= matchRect.Top)
                                    {
                                        AnalysisResult = @"FAILED 找到的光型区域异常";
                                        return;
                                    }

                                    var topAdd = maxRect.Top - matchRect.Top;

                                    if (maxRect.Top - topAdd < 0 || maxRect.Bottom + topAdd > firstOtsu.Rows)
                                    {
                                        AnalysisResult = @"FAILED 找到的光型区域异常";
                                        return;
                                    }

                                    maxRect = new Rect(maxRect.Left, maxRect.Top - topAdd, maxRect.Width, maxRect.Height + topAdd);

                                    var offsetX = maxRect.X;
                                    var offsetY = maxRect.Y;

                                    // 显示最大轮廓
                                    using (var matchMat = src.Clone())
                                    {
                                        if (matchMat.Channels() == 1)
                                            Cv2.CvtColor(matchMat, matchMat, ColorConversionCodes.GRAY2BGR);
                                        matchMat.Rectangle(maxRect, Scalar.Green);
                                        AppendImage("近光待测光型轮廓", matchMat);
                                    }

                                    using (var roiMat = medianBlurMat[maxRect])
                                    {
                                        AppendImage("近光待测光型区域提取", roiMat);

                                        using (var secondOtsu = roiMat.Clone()) // 第一次otsu，将待测光型区域二值化
                                        {
                                            Cv2.Threshold(secondOtsu, secondOtsu, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
                                            AppendImage("第二次otsu二值化待测光型区域", secondOtsu);

                                            var listEdgePoints = new List<Point>();

                                            for (var i = matchRect.X - offsetX; i <= matchRect.X + matchRect.Width - offsetX; i++)
                                            {
                                                for (var j = matchRect.Y - offsetY; j <= matchRect.Y + matchRect.Height - offsetY; j++)
                                                {
                                                    var gray = secondOtsu.At<byte>(j, i);
                                                    if (gray == 255)
                                                    {
                                                        listEdgePoints.Add(new Point(i, j));
                                                        break;
                                                    }
                                                }
                                            }

                                            if (!listEdgePoints.Any())
                                            {
                                                AnalysisResult = @"FAILED 截止线寻找失败";
                                                return;
                                            }

                                            using (var edgeMat = new Mat(src.Size(), MatType.CV_8UC1, Scalar.Black))
                                            {
                                                foreach (var point in listEdgePoints)
                                                    edgeMat.Set<byte>(point.Y + offsetY, point.X + offsetX, 255);

                                                AppendImage("近光截止线展示", edgeMat);

                                                // ...
                                                // 曲线拟合
                                                using (var polyMat = edgeMat.Clone())
                                                {
                                                    Cv2.CvtColor(polyMat, polyMat, ColorConversionCodes.GRAY2BGR);

                                                    var xs = listEdgePoints.Select(t => (double)t.X).ToArray();
                                                    var ys = listEdgePoints.Select(t => (double)t.Y).ToArray();
                                                    //y = Smoothing(y,3);

                                                    var _order = Math.Min(5, xs.Length - 1); //曲线阶数
                                                    var ret = Fit.Polynomial(xs, ys, _order);

                                                    var yTest = listEdgePoints.Select(point => GetPolynomialValue(_order, point.X, ret)).ToArray();
                                                    var _rSquared = GoodnessOfFit.RSquared(ys, yTest);

                                                    var poly = new PolynomialDerivative(ret[5], ret[4], ret[3], ret[2], ret[1], ret[0]);
                                                    var inflectionPoints = poly.FindInflectionPoints();

                                                    for (int i = 0; i < listEdgePoints.Count; i++)
                                                        polyMat.Set((int)yTest[i] + offsetY, listEdgePoints[i].X + offsetX, new Vec3b(0, 0, 255));

                                                    if (inflectionPoints.Item1 != null)
                                                    {
                                                        pInflection = new Point((int)inflectionPoints.Item1 + offsetX, GetPolynomialValue(_order, (double)inflectionPoints.Item1, ret) + offsetY);
                                                        _inp = pInflection;
                                                        Cv2.Circle(polyMat, pInflection, 1, Scalar.Green, -1);
                                                    }

                                                    AppendImage("近光截止线曲线拟合结果", polyMat);

                                                    if (pInflection == default(Point))
                                                    {
                                                        AnalysisResult = @"FAILED 二阶求导计算拐点失败";
                                                        return;
                                                    }

                                                    using (var showInflectionPointOnSrc = src.Clone())
                                                    {
                                                        if (showInflectionPointOnSrc.Channels() == 1)
                                                            Cv2.CvtColor(showInflectionPointOnSrc, showInflectionPointOnSrc, ColorConversionCodes.GRAY2BGR);

                                                        for (int i = 0; i < listEdgePoints.Count; i++)
                                                        {
                                                            showInflectionPointOnSrc.Set((int)listEdgePoints[i].Y + offsetY, listEdgePoints[i].X + offsetX, new Vec3b(0, 255, 255));
                                                            showInflectionPointOnSrc.Set((int)yTest[i] + offsetY, listEdgePoints[i].X + offsetX, new Vec3b(0, 0, 255));
                                                        }

                                                        if (inflectionPoints.Item1 != null)
                                                            Cv2.Circle(showInflectionPointOnSrc, pInflection, 1, Scalar.Green, -1);

                                                        AppendImage("近光找拐点结果", showInflectionPointOnSrc);
                                                    }

                                                    using (var gammaMat = medianBlurMat.Clone())
                                                    //using (var gammaMat = MyCvHelper.GammaCorrection(medianBlurMat, 0.45))
                                                    {
                                                        using (var showGB4599ResultMat = gammaMat.Clone())
                                                        {
                                                            if (showGB4599ResultMat.Channels() == 1)
                                                                Cv2.CvtColor(showGB4599ResultMat, showGB4599ResultMat, ColorConversionCodes.GRAY2BGR);

                                                            // 计算HV点的像素坐标,并根据HV点建立坐标系
                                                            {
                                                                var pH0V0 = DegreeToPixelPoint(pInflection, 0, 0);
                                                                Cv2.Line(showGB4599ResultMat, new Point(0, pH0V0.Y), new Point(showGB4599ResultMat.Width, pH0V0.Y), Scalar.Yellow);
                                                                Cv2.Line(showGB4599ResultMat, new Point(pH0V0.X, 0), new Point(pH0V0.X, showGB4599ResultMat.Height), Scalar.Yellow);

                                                                //Cv2.Line(showGB4599ResultMat, new Point(0, pInflection.Y), new Point(showGB4599ResultMat.Width, pInflection.Y), Scalar.Yellow);

                                                                // 画出HV点和拐点位置
                                                                Cv2.Circle(showGB4599ResultMat, pH0V0, 1, Scalar.Green, -1);
                                                                Cv2.Circle(showGB4599ResultMat, pInflection, 1, Scalar.Green, -1);
                                                            }

                                                            // 计算各法规点的灰度值
                                                            var dicGray = new Dictionary<string, double>();

                                                            // 最亮点
                                                            var pEMaxDegreeH = double.MinValue;
                                                            var pEMaxDegreeV = double.MinValue;
                                                            Point pEMax = default;
                                                            using (var maxBrightnessMat = gammaMat.Clone())
                                                            {
                                                                Cv2.MinMaxLoc(maxBrightnessMat, out _, out var maxVal, out _, out _);
                                                                Cv2.Threshold(maxBrightnessMat, maxBrightnessMat, maxVal * 0.80, 255, ThresholdTypes.Binary);
                                                                AppendImage("近光最亮点区域轮廓", maxBrightnessMat);

                                                                // 找到轮廓
                                                                Point[][] contoursEMax;
                                                                HierarchyIndex[] hierarchyEMax;
                                                                Cv2.FindContours(maxBrightnessMat, out contoursEMax, out hierarchyEMax, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

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

                                                                if (maxEMaxIndex > -1)
                                                                {
                                                                    // 计算矩
                                                                    var moments = Cv2.Moments(contoursEMax[maxEMaxIndex]);
                                                                    // 计算中心坐标
                                                                    var cx = moments.M10 / moments.M00;
                                                                    var cy = moments.M01 / moments.M00;
                                                                    pEMax = new Point(cx, cy);
                                                                    Cv2.Circle(showGB4599ResultMat, pEMax, 1, Scalar.Purple, -1);

                                                                    var pEMaxDegree = PixelPointToDegree(pInflection, pEMax);
                                                                    pEMaxDegreeH = pEMaxDegree.X;
                                                                    pEMaxDegreeV = pEMaxDegree.Y;
                                                                    var toCheckEMaxRoi = GetGrayRoiByDegree(pInflection, pEMaxDegree.X, pEMaxDegree.Y, 0.1, 0.1);
                                                                    var lx = GetGray(new Mat(gammaMat, toCheckEMaxRoi));
                                                                    dicGray.Add("EMax", lx);
                                                                    Cv2.Circle(showGB4599ResultMat, pEMax, 1, Scalar.Purple, -1);
                                                                    Cv2.Rectangle(showGB4599ResultMat, toCheckEMaxRoi, Scalar.Purple);
                                                                    //Cv2.PutText(systemMat, string.Format("EMax[{0},{1}]: '{2}'", pEMaxDegree.X, pEMaxDegree.Y, lx), new Point(toCheckEMaxRoi.X + toCheckEMaxRoi.Width + 1, toCheckEMaxRoi.Y + 2), HersheyFonts.HersheySimplex,
                                                                    //    0.4, Scalar.Purple);
                                                                    Cv2.PutText(showGB4599ResultMat, string.Format("EMax[{0},{1}]", pEMaxDegree.X, pEMaxDegree.Y), new Point(toCheckEMaxRoi.X + toCheckEMaxRoi.Width + 1, toCheckEMaxRoi.Y + 2), HersheyFonts.HersheySimplex,
                                                                       0.4, Scalar.Purple);
                                                                }
                                                                else
                                                                {
                                                                    dicGray.Add("EMax", double.MinValue);
                                                                }
                                                            }

                                                            {
                                                                // 多边形
                                                                foreach (var item in _lbTestPoly)
                                                                {
                                                                    var name = item.Key;

                                                                    Point[] points = new Point[item.Value.Length];

                                                                    for (int i = 0; i < item.Value.Length; i++)
                                                                        points[i] = DegreeToPixelPoint(pInflection, item.Value[i].X, item.Value[i].Y);

                                                                    using (Mat mask = new Mat(src.Size(), MatType.CV_8UC1, new Scalar(0)))
                                                                    {
                                                                        Cv2.FillPoly(mask, new Point[][] { points }, new Scalar(255));

                                                                        double maxVal = double.MinValue;
                                                                        Point maxPoint = new Point();

                                                                        for (int y = 0; y < src.Height; y++)
                                                                        {
                                                                            for (int x = 0; x < src.Width; x++)
                                                                            {
                                                                                // 检查是否在多边形内部
                                                                                if (mask.At<byte>(y, x) == 255)
                                                                                {
                                                                                    var thisPointRoi = new Rect(x - 1, y - 1, 3, 3);
                                                                                    var lxRoi = new Mat(gammaMat, thisPointRoi);
                                                                                    var lx = GetGray(lxRoi);
                                                                                    lxRoi.Dispose();

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
                                                                        Cv2.Circle(showGB4599ResultMat, maxPoint, 1, Scalar.Red, -1);
                                                                        Cv2.PutText(showGB4599ResultMat, string.Format("{0}: '{1}'", item.Key, maxVal), new Point(maxPoint.X + 1, maxPoint.Y + 2), HersheyFonts.HersheySimplex, 0.4, Scalar.Red);
                                                                        Cv2.Polylines(showGB4599ResultMat, new Point[][] { points }, isClosed: true, Scalar.Blue, thickness: 1);
                                                                        mask.Dispose();
                                                                    }
                                                                }

                                                                var listPurplePoints = new List<Point>();
                                                                foreach (var item in _lbTestPosDegree)
                                                                {
                                                                    var pixelPoint = DegreeToPixelPoint(pInflection, item.Value.X, item.Value.Y);
                                                                    listPurplePoints.Add(pixelPoint);

                                                                    var thisPointRoi = GetGrayRoiByDegree(pInflection, item.Value.X, item.Value.Y, 0.1, 0.1);
                                                                    var lxRoi = new Mat(gammaMat, thisPointRoi);
                                                                    var lx = GetGray(lxRoi);
                                                                    dicGray.Add(item.Key, lx);
                                                                    lxRoi.Dispose();

                                                                    Cv2.Rectangle(showGB4599ResultMat, thisPointRoi, Scalar.Red);
                                                                    Cv2.Circle(showGB4599ResultMat, pixelPoint, 1, Scalar.Red, -1);
                                                                    Cv2.PutText(showGB4599ResultMat, string.Format("{0}: '{1}'", item.Key, lx), new Point(thisPointRoi.X + thisPointRoi.Width + 1, thisPointRoi.Y + 2), HersheyFonts.HersheySimplex, 0.4, Scalar.Red);
                                                                }

                                                                foreach (var p in _lbTestPosDegree2)
                                                                {
                                                                    var maxLx = double.MinValue;
                                                                    str = string.Empty;

                                                                    foreach (var item in p)
                                                                    {
                                                                        var pixelPoint = DegreeToPixelPoint(pInflection, item.Value.X, item.Value.Y);
                                                                        listPurplePoints.Add(pixelPoint);

                                                                        var thisPointRoi = GetGrayRoiByDegree(pInflection, item.Value.X, item.Value.Y, 0.1, 0.1);//  new Rect(pixelPoint.X - 1, pixelPoint.Y - 1, 3, 3);
                                                                        var lxRoi = new Mat(gammaMat, thisPointRoi);
                                                                        var lx = GetGray(lxRoi);

                                                                        str += item.Key + "+";

                                                                        if (double.MinValue == maxLx)
                                                                        {
                                                                            maxLx = lx;
                                                                        }
                                                                        else
                                                                        {
                                                                            maxLx += lx;
                                                                        }

                                                                        lxRoi.Dispose();

                                                                        Cv2.Rectangle(showGB4599ResultMat, thisPointRoi, Scalar.Red);
                                                                        Cv2.Circle(showGB4599ResultMat, pixelPoint, 1, Scalar.Red, -1);
                                                                        Cv2.PutText(showGB4599ResultMat, string.Format("{0}: '{1}'", item.Key, lx), new Point(thisPointRoi.X + thisPointRoi.Width + 1, thisPointRoi.Y + 2), HersheyFonts.HersheySimplex, 0.4, Scalar.Red);
                                                                    }

                                                                    str = str.TrimEnd('+');
                                                                    dicGray.Add(str, maxLx);
                                                                }

                                                                foreach (var p in listPurplePoints)
                                                                    Cv2.Circle(showGB4599ResultMat, p, 1, Scalar.Purple, -1);

                                                                // 线段
                                                                foreach (var item in _lbTestLine)
                                                                {
                                                                    var name = item.Key;
                                                                    var startPixel = DegreeToPixelPoint(pInflection, item.Value.Item1.X, item.Value.Item1.Y);
                                                                    var endPixel = DegreeToPixelPoint(pInflection, item.Value.Item2.X, item.Value.Item2.Y);

                                                                    var minLx = double.MinValue;
                                                                    var pointMaxP = default(Point);

                                                                    var showTxtPoint = new Point((startPixel.X + endPixel.X) / 2f + 1, endPixel.Y + 15);
                                                                    if (startPixel.X < 1)
                                                                        showTxtPoint.X = 1;
                                                                    else if (endPixel.X > src.Width - 1)
                                                                        showTxtPoint.X = src.Width - 1;

                                                                    for (var i = item.Value.Item1.X; i <= item.Value.Item2.X; i = i + 0.01f)
                                                                    {
                                                                        var testH = i;
                                                                        var testV = item.Value.Item1.Y;

                                                                        var thisPointRoi = GetGrayRoiByDegree(pInflection, testH, testV, 0.1, 0.1);//  new Rect(pixelPoint.X - 1, pixelPoint.Y - 1, 3, 3);
                                                                        if (thisPointRoi.X < 0 || thisPointRoi.Y < 0 ||
                                                                            thisPointRoi.X + thisPointRoi.Width > src.Width ||
                                                                            thisPointRoi.Y + thisPointRoi.Height > src.Height)
                                                                            continue;
                                                                        var lxRoi = new Mat(gammaMat, thisPointRoi);
                                                                        var lx = GetGray(lxRoi);

                                                                        if (minLx == double.MinValue)
                                                                        {
                                                                            minLx = lx;
                                                                            pointMaxP = DegreeToPixelPoint(pInflection, testH, testV);
                                                                        }
                                                                        else
                                                                        {
                                                                            if (lx < minLx)
                                                                            {
                                                                                minLx = lx;
                                                                                pointMaxP = DegreeToPixelPoint(pInflection, testH, testV);
                                                                            }
                                                                        }
                                                                    }

                                                                    minLx = Math.Round(minLx, 2, MidpointRounding.AwayFromZero);
                                                                    dicGray.Add(name, minLx);

                                                                    Cv2.Circle(showGB4599ResultMat, pointMaxP, 1, Scalar.Red, -1);
                                                                    Cv2.Line(showGB4599ResultMat, startPixel, endPixel, Scalar.Purple, 1);
                                                                    Cv2.Circle(showGB4599ResultMat, startPixel, 1, Scalar.Blue, -1);
                                                                    Cv2.Circle(showGB4599ResultMat, endPixel, 1, Scalar.Blue, -1);
                                                                    //Cv2.PutText(systemMat, string.Format("{0}: '{1}'", item.Key, lx), new Point(endPixel.X + 1, endPixel.Y + 1), HersheyFonts.HersheySimplex, 0.4, Scalar.Red);
                                                                    Cv2.PutText(showGB4599ResultMat, string.Format("{0}: {1}", name, minLx), showTxtPoint, HersheyFonts.HersheySimplex, 0.4, Scalar.Purple);
                                                                }
                                                            }

                                                            // 测梯度
                                                            var maxG = double.MinValue;
                                                            {
                                                                Point maxGp1 = default;
                                                                Point maxGp2 = default;

                                                                var gp1 = DegreeToPixelPoint(pInflection, -2.5, -1.5);
                                                                var gp2 = DegreeToPixelPoint(pInflection, -2.5, 0);
                                                                Cv2.Line(showGB4599ResultMat, gp1, gp2, Scalar.Yellow);

                                                                var listGrays = new List<Tuple<Point, double, Point, double, double>>();

                                                                for (var v = -1.5d; v <= 0d; v += 0.05)
                                                                {
                                                                    //if (v + 0.1 > 5)
                                                                    //    break;

                                                                    var p1 = DegreeToPixelPoint(pInflection, -2.5, v);
                                                                    var r1 = GetGrayRoiByDegree(pInflection, -2.5, v, 0.1);
                                                                    //r1 = new Rect(new Point(p1.X - 1, p1.Y - 1), new Size(3, 3));
                                                                    //var lx1 = GetGray(new Mat(Images[srcName], r1));
                                                                    r1 = new Rect(new Point(p1.X, p1.Y), new Size(1, 1));
                                                                    var lx1 = GetGray(new Mat(gammaMat, r1));

                                                                    var p2 = DegreeToPixelPoint(pInflection, -2.5, v + 0.1);
                                                                    var r2 = GetGrayRoiByDegree(pInflection, -2.5, v + 0.1, 0.1);
                                                                    //r2 = new Rect(new Point(p2.X - 1, p2.Y - 1), new Size(3, 3));
                                                                    //var lx2 = GetGray(new Mat(Images[srcName], r2));
                                                                    r2 = new Rect(new Point(p2.X, p2.Y), new Size(1, 1));
                                                                    var lx2 = GetGray(new Mat(gammaMat, r2));

                                                                    var g = Math.Round(Math.Log10(lx1) - Math.Log10(lx2), 2, MidpointRounding.AwayFromZero);

                                                                    listGrays.Add(new Tuple<Point, double, Point, double, double>(p1, lx1, p2, lx2, g));

                                                                    if (!(g >= maxG))
                                                                        continue;

                                                                    maxG = g;
                                                                    maxGp1 = p1;
                                                                    maxGp2 = p2;
                                                                }

                                                                if (Math.Abs(maxG - double.MinValue) > 1 && maxGp1 != default && maxGp1 != default)
                                                                {
                                                                    Cv2.Line(showGB4599ResultMat, maxGp1, maxGp2, Scalar.Blue);
                                                                    Cv2.PutText(showGB4599ResultMat, string.Format("Grad_2.5L: '{0}'", maxG),
                                                                        new Point(maxGp2.X + 1, maxGp2.Y + 1), HersheyFonts.HersheySimplex, 0.4, Scalar.Blue);
                                                                    Cv2.Circle(showGB4599ResultMat, maxGp1, 1, Scalar.Purple);
                                                                    Cv2.Circle(showGB4599ResultMat, maxGp2, 1, Scalar.Purple);
                                                                }
                                                            }

                                                            // 分析完成,输出结果
                                                            {
                                                                AnalysisResult = "Finish";

                                                                InflectionPixelPointX = pInflection.X;
                                                                InflectionPixelPointY = pInflection.Y;

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

                                                            AppendImage("近光检测结果", showGB4599ResultMat);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(srcHbName))
            {
                if (!Images.ContainsKey(srcHbName) || Images[srcHbName] == null || Images[srcHbName].Empty())
                {
                    AnalysisResult = @"FAILED 远光图像不存在";
                    return;
                }

                ClearAnalysisResultHbAlb();
                AnalyzeHb(srcHbName, DegreeToPixelPoint(pInflection, 0d, 0d), pInflection);
            }
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

        public double CalculateFresnelLensDistancePixel()
        {
            var scaleRatio = CalculateScaleRatio(PixelScale, Ratio / 1000f, CameraDistanceCm * 0.01f);
            var perMm = 1 / scaleRatio;
            var distance = perMm * FresnelLensDistanceCm * 10f;

            distance = Math.Round(distance, 2, MidpointRounding.AwayFromZero);

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

        private static double GetDegrees(double rad)
        {
            return Math.Round(rad * (180 / Math.PI), 2, MidpointRounding.AwayFromZero);
        }

        private static double GetRad(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        private static double GetGray(Mat mask)
        {
            double lx;
            //Cv2.CvtColor(mask, mask, ColorConversionCodes.BGR2GRAY);
            var mean = Cv2.Mean(mask);
            lx = Math.Round(mean.Val0, 0, MidpointRounding.AwayFromZero);

            return lx;
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

            var top = new Point(leftTop.X - 1, leftTop.Y - 1);
            //var width = rightBottom.X - leftTop.X;
            //var height = rightBottom.Y - leftTop.Y;

            //if (width < 3)
            //{
            //    width = 3;
            //}

            //if (height < 3)
            //{
            //    height = 3;
            //}

            var roi = new Rect(top, new Size(3, 3));
            return roi;
        }

        /// <summary>
        /// 获取多项式的值
        /// </summary>
        /// <param name="order">阶数</param>
        /// <param name="x">x坐标</param>
        /// <param name="polynomialArray">多项式系数数组</param>
        /// <returns>Y坐标</returns>
        private static double GetPolynomialValue(int order, double x, double[] polynomialArray) =>
            Enumerable.Range(0, order + 1)
                .Select(t => polynomialArray[t] * Math.Pow(x, t))
                .Sum();

        public class PolynomialDerivative
        {
            // 五次函数系数
            public double a, b, c, d, e, f;

            public PolynomialDerivative(double a, double b, double c, double d, double e, double f)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.d = d;
                this.e = e;
                this.f = f;
            }

            /// <summary>
            /// 计算二阶导数的极值点（三阶导数的根）
            /// </summary>
            /// <returns></returns>
            //public (double? x1, double? x2) FindInflectionPoints()
            public Tuple<double?, double?> FindInflectionPoints()
            {
                // 三阶导数方程：60a x² + 24b x + 6c = 0
                double A = 60 * a;
                double B = 24 * b;
                double C = 6 * c;

                double discriminant = B * B - 4 * A * C;

                if (discriminant < 0)
                    return new Tuple<double?, double?>(null, null);//(null, null); // 无实根

                double sqrtDiscriminant = Math.Sqrt(discriminant);
                double x1 = (-B + sqrtDiscriminant) / (2 * A);
                double x2 = (-B - sqrtDiscriminant) / (2 * A);

                var isX1InflectionPoint = IsInflectionPoint(x1);
                var isX2InflectionPoint = IsInflectionPoint(x2);

                return new Tuple<double?, double?>(x1, x2);//(x1, x2);
            }
            private bool IsInflectionPoint(double x)
            {
                double epsilon = 1e-6; // 数值容差
                double ddfLeft = SecondDerivative(x - epsilon);
                double ddfRight = SecondDerivative(x + epsilon);

                // 符号变化判断
                return (ddfLeft * ddfRight) <= 0;
            }

            public double SecondDerivative(double x) =>
                20 * a * Math.Pow(x, 3) + 12 * b * Math.Pow(x, 2) + 6 * c * x + 2 * d;
        }

        #endregion

        #region 分析过程GB4599，以LB的拐点位基准

        public string AnalysisResultHb = string.Empty;

        public double GrayEMaxHb = double.MinValue;

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

        private void AnalyzeHb(string hbSrcName, Point hv, Point ip)
        {
            if (!Images.ContainsKey(hbSrcName) || Images[hbSrcName] == null || Images[hbSrcName].Empty())
            {
                AnalysisResultHb = "FAILED HB图像不存在";
                return;
            }

            Point hbHv = hv;

            using (var imageHbSrc = Images[hbSrcName].Clone())
            {
                using (var grayMat = imageHbSrc.Clone())
                {
                    if (grayMat.Channels() != 1)
                        Cv2.CvtColor(grayMat, grayMat, ColorConversionCodes.BGR2GRAY);
                    Cv2.MedianBlur(grayMat, grayMat, 5); // 使用5*5的中值滤波器

                    Rect maxContourRect;
                    if (MyCamera.TryGetMaxContourOuterRect(grayMat, out maxContourRect))
                    {
                        var offsetX = maxContourRect.X;
                        var offsetY = maxContourRect.Y;

                        using (var toGetLx = grayMat.Clone(maxContourRect))
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
                                hbHv = hv;
                                if (toGetLx.Channels() == 1)
                                    Cv2.CvtColor(toGetLx, toGetLx, ColorConversionCodes.GRAY2BGR);
                                Cv2.Circle(toGetLx, maxLxPoint, 1, Scalar.Purple, -1);
                                AppendImage("HB_EMax", toGetLx);

                                using (var systemMat = imageHbSrc.Clone())
                                {
                                    if (systemMat.Channels() == 1)
                                        Cv2.CvtColor(systemMat, systemMat, ColorConversionCodes.GRAY2BGR);
                                    var roiRect = GetGrayRoiByDegree(hbHv, 0, 0, 12.5, 6);

                                    if (roiRect.X >= 0 && roiRect.Y >= 0 && roiRect.X + roiRect.Width <= systemMat.Width && roiRect.Y <= systemMat.Height)
                                    {
                                        // 计算HV点的像素坐标,并根据HV点建立坐标系
                                        {
                                            var pH0V0 = hbHv;
                                            Cv2.Line(systemMat, new Point(0, pH0V0.Y), new Point(systemMat.Width, pH0V0.Y), Scalar.Yellow);
                                            Cv2.Line(systemMat, new Point(pH0V0.X, 0), new Point(pH0V0.X, systemMat.Height), Scalar.Yellow);

                                            // 画出HV点和拐点位置
                                            Cv2.Circle(systemMat, pH0V0, 1, Scalar.Green, -1);
                                        }

                                        var eMaxDeg = PixelPointToDegree(ip, new Point(maxLxPoint.X + maxContourRect.X, maxLxPoint.Y + maxContourRect.Y));

                                        var toCheckHbPoint = new Dictionary<string, Point2f>
                                        {
                                            { "EMax", new Point2f(eMaxDeg.X,eMaxDeg.Y) },

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

                                        using (var dstSystemMat = systemMat.Clone())
                                        {
                                            // 分析完成,输出结果
                                            {
                                                AnalysisResultHb = "Finish";

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

                                            AppendImage("HB结果", dstSystemMat);
                                            dstSystemMat.Dispose();
                                        }
                                    }
                                    else
                                    {
                                        AnalysisResultHb = string.Format("FAILED HB检测'H:-{0}°~{1}°','V:-{2}°~{3}°'范围超限", 12, 12, 6, 6);
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

                    grayMat.Dispose();
                }

                imageHbSrc.Dispose();
            }
        }

        #endregion

        #region 其他

        public bool IsLbOn;
        public bool IsHbOn;
        public bool IsInCali;
        public bool IsInAutoRun;
        public bool IsInServoCali;

        public bool IsTransferLbOk;
        public bool IsTransferHbOk;

        public double OutPutTransferServoXk = 0.16;
        public double OutPutTransferServoXb = 15.89;

        public double OutPutTransferServoZk = 0.16;
        public double OutPutTransferServoZb = 166.39 - 65;

        public double OutPutTransferServoXMax = 400;

        public double OutPutTransferServoZMax = 270;
        public double OutPutTransferServoZMin = 75;

        public float OutputLb25VColor;
        public float OutputHbHVColor;

        public float OutputTransferLb25VX;
        public float OutputTransferLb25VZ;

        public float OutputTransferHbHVX;
        public float OutputTransferHbHVZ;

        private Point _inp;

        public void ResetTransferServoPosition()
        {
            IsTransferLbOk = false;
            IsTransferHbOk = false;

            // 近光
            {
                OutputLb25VColor = -9999;

                OutputTransferLb25VX = -9999;
                OutputTransferLb25VZ = -9999;
            }

            // 远光
            {
                OutputHbHVColor = -9999;

                OutputTransferHbHVX = -9999;
                OutputTransferHbHVZ = -9999;
            }
        }

        /// <summary>
        /// 近光像素结果坐标转换为模组坐标
        /// </summary>
        public void TransferServoPosistionLb()
        {
            // LB 25V
            {
                var p25v = DegreeToPixelPoint(_inp, 0f, -1.72f);
                OutputTransferLb25VX = CalculateServoPoint(p25v).X;
                OutputTransferLb25VZ = CalculateServoPoint(p25v).Y;

                if (OutputTransferLb25VX > OutPutTransferServoXMax ||
                    OutputTransferLb25VZ > OutPutTransferServoZMax ||
                    OutputTransferLb25VZ < OutPutTransferServoZMin)
                    return;
            }

            IsTransferLbOk = true;
        }

        /// <summary>
        /// 远光光像素结果坐标转换为模组坐标
        /// </summary>
        public void TransferServoPosistionHb()
        {
            // HB HV
            {
                var pHv = DegreeToPixelPoint(_inp, 0f, 0f);
                OutputTransferHbHVX = CalculateServoPoint(pHv).X;
                OutputTransferHbHVZ = CalculateServoPoint(pHv).Y;

                if (OutputTransferHbHVX > OutPutTransferServoXMax ||
                    OutputTransferHbHVZ > OutPutTransferServoZMax ||
                    OutputTransferHbHVZ < OutPutTransferServoZMin)
                    return;
            }

            IsTransferHbOk = true;
        }

        private Point2f CalculateServoPoint(Point imgPoint)
        {
            const int offsetX = 0; //InterestedRoi.X;
            const int offsetY = 0; //InterestedRoi.Y;

            var x = (imgPoint.X + offsetX) * OutPutTransferServoXk + OutPutTransferServoXb;
            var z = (imgPoint.Y + offsetY) * OutPutTransferServoZk + OutPutTransferServoZb;

            //z = z - 67;

            //var x = ((imgPoint.X - 69F) / 51) * 10f;
            //var z = ((imgPoint.Y + 167f) / 37.5f) * 10f;

            return new Point2f((float)x, (float)z);
        }

        #endregion
    }
}
