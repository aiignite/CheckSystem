using CommonUtility;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CommonUtility.FileOperator;
using Mat = OpenCvSharp.Mat;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace CheckSystem.PesVision
{
    public static class PesVisionUtility
    {
        public static IniFileHelper IniFile =
            new IniFileHelper(Program.SysDir + @"\PesConfig" + @"\PesVisionConfig.ini");

        public static Rect Roi;
        public static double Denoise;
        public static double PixelStep;
        public static double PerDegreePixel;
        public static List<PointWithGray> LbStandard = new List<PointWithGray>();
        public static double LowBeamGradient;
        public static List<PointWithGray> HbStandard = new List<PointWithGray>();
        public static double HighBeamH0V0EmaxRatio;


        public static void ReadSetup()
        {
            var roiStr = IniFile.IniReadValue("CutOffLine", "ROI");
            Roi = new Rect(int.Parse(roiStr.Split(',')[0]), int.Parse(roiStr.Split(',')[1]), int.Parse(roiStr.Split(',')[2]), int.Parse(roiStr.Split(',')[3]));
            Denoise = double.Parse(IniFile.IniReadValue("CutOffLine", "Denoise"));
            PixelStep = double.Parse(IniFile.IniReadValue("CutOffLine", "PixelStep"));
            PerDegreePixel = double.Parse(IniFile.IniReadValue("CutOffLine", "PerDegreePixel"));

            var lbPointsStr = IniFile.IniReadValue("LowBeam", "Points");
            foreach (var p in lbPointsStr.Split(';'))
            {
                var ps = p.Trim(new char[] { '[', ']' }).Split(',');
                LbStandard.Add(new PointWithGray { Gray = byte.Parse(ps[2]), Point = new Point(int.Parse(ps[0]), int.Parse(ps[1])) });
            }
            LowBeamGradient = double.Parse(IniFile.IniReadValue("LowBeam", "Gradient"));

            var hbPointsStr = IniFile.IniReadValue("HighBeam", "Points");
            foreach (var p in hbPointsStr.Split(';'))
            {
                var ps = p.Trim(new char[] { '[', ']' }).Split(',');
                HbStandard.Add(new PointWithGray { Gray = byte.Parse(ps[2]), Point = new Point(int.Parse(ps[0]), int.Parse(ps[1])) });
            }
            HighBeamH0V0EmaxRatio = double.Parse(IniFile.IniReadValue("HighBeam", "H0V0EmaxRatio"));
        }

        private static long _startTs;

        public static int VisionAnalysis(
            Mat lowBeamCutOffLine, double denoiseSigma, Rect cutOffLineRoi, out Mat denoiseImg, Point pCenterNegative4, out double perDegreePixel,
            int maxYDiff, int scanBrightestStep,
            out Point cutOffLineP1, out Point cutOffLineP2, out Point cutOffLineP3, out Point cutOffLineP4, out Point pCutOffLineCenter, out Point pH0V0, ref List<Point> validCorners,
            out Mat cutOffLineContourImg, out Mat inflectionPointImg, out Mat coordinateSystemImg,
            Mat lowBeamOrigin, out Mat lowBeamWithGrayResult, Mat highBeamOrigin, out Mat highBeamWithGrayResult,
            ref string msg, PesVisionAnalysisStep targetStep = 0)
        {
            _startTs = HighPrecisionTimer.GetTimestamp();

            denoiseImg = null;
            cutOffLineContourImg = null;
            inflectionPointImg = null;
            coordinateSystemImg = null;
            lowBeamWithGrayResult = null;
            highBeamWithGrayResult = null;
            perDegreePixel = -1;
            cutOffLineP1 = new Point(0, 0);
            cutOffLineP2 = new Point(0, 0);
            cutOffLineP3 = new Point(0, 0);
            cutOffLineP4 = new Point(0, 0);
            pCutOffLineCenter = new Point(0, 0);
            pH0V0 = new Point(0, 0);

            if (lowBeamCutOffLine == null || lowBeamCutOffLine.Empty())
            {
                msg += @"请先导入待识别截止线的近光图像";
                return -1;
            }

            // 先将图像降噪
            denoiseImg = ImgDenoise(lowBeamCutOffLine, denoiseSigma);
            msg += FormatMsg("降噪完成");

            if (targetStep == PesVisionAnalysisStep.Denoise)
                return (int)targetStep;

            // 寻找并收集最亮点
            for (var x = cutOffLineRoi.X; x < cutOffLineRoi.X + cutOffLineRoi.Width; x += scanBrightestStep)
            {
                double maxValue = 0;
                var maxPoint = new Point(x, 0);

                for (var y = cutOffLineRoi.Y; y < cutOffLineRoi.Y + cutOffLineRoi.Height; y++)
                {
                    double pixelValue = denoiseImg.At<byte>(y, x);
                    if (!(pixelValue > maxValue))
                        continue;
                    maxValue = pixelValue;
                    maxPoint = new Point(x, y);
                }

                validCorners.Add(maxPoint);
            }
            cutOffLineContourImg = lowBeamCutOffLine.Clone();
            foreach (var p in validCorners)
                Cv2.Circle(cutOffLineContourImg, p, 1, Scalar.Red, -1);
            msg += FormatMsg("寻找并收集最亮点完成");

            if (validCorners.Count == 0)
            {
                msg += FormatMsg("寻找最亮点失败");
                return (int)PesVisionAnalysisStep.Denoise;
            }

            if (targetStep == PesVisionAnalysisStep.CutOffLineContour)
                return (int)targetStep;

            // 根据Y轴的偏差,分析拐点坐标
            var minXPoint = validCorners.OrderBy(p => p.X).First();
            //获取所有与最左侧点近似水平的点
            var horizontalPoints =
                validCorners.Where(p => Math.Abs(p.Y - minXPoint.Y) <= maxYDiff).ToList();
            // 剩余的点，不包括近似水平线上的点
            var remainingPoints = validCorners.Except(horizontalPoints).ToList();
            // 拟合并绘制第一条线
            FitLine(horizontalPoints, denoiseImg.Width, out cutOffLineP1, out cutOffLineP2);
            // 拟合并绘制第二条线
            FitLine(remainingPoints, denoiseImg.Width, out cutOffLineP3, out cutOffLineP4);
            // 两条直线交汇点
            pCutOffLineCenter = GetIntersection(cutOffLineP1, cutOffLineP2, cutOffLineP3, cutOffLineP4);
            // 分析拐点结束
            msg += FormatMsg("分析拐点坐标完成");

            var pixel = Math.Abs(pCutOffLineCenter.X - pCenterNegative4.X);
            perDegreePixel = Math.Round(pixel / (double)4, 6, MidpointRounding.AwayFromZero);
            pH0V0 = new Point((double)pCutOffLineCenter.X, (double)pCutOffLineCenter.Y - perDegreePixel * 0.57);

            var perPixelDegree = Math.Round((double)4 / pixel, 6, MidpointRounding.AwayFromZero);

            // 判断拐点是否在ROI中
            var isInsideRoi = cutOffLineRoi.Contains(pCutOffLineCenter);
            if (!isInsideRoi)
            {
                msg += FormatMsg("寻找拐点失败,拐点坐标不在ROI区域内");
                return (int)PesVisionAnalysisStep.CutOffLineContour;
            }

            // 判断[H0,V0]点是否在图像内
            if (pH0V0.X <= 0 || pH0V0.X >= lowBeamCutOffLine.Width || pH0V0.Y <= 0 || pH0V0.Y >= lowBeamCutOffLine.Height)
            {
                msg += FormatMsg("计算[H0,V0]点失败,超出图像范围");
                return (int)PesVisionAnalysisStep.CutOffLineContour;
            }

            if (targetStep == PesVisionAnalysisStep.InflectionPoint)
                return (int)targetStep;

            inflectionPointImg = lowBeamCutOffLine.Clone();
            Cv2.Line(inflectionPointImg, cutOffLineP1, cutOffLineP2, Scalar.Red);
            Cv2.Line(inflectionPointImg, cutOffLineP3, cutOffLineP4, Scalar.Blue);
            Cv2.Circle(inflectionPointImg, pCenterNegative4, 1, Scalar.Green, -1);
            Cv2.Circle(inflectionPointImg, pH0V0, 1, Scalar.Green, -1);
            Cv2.Circle(inflectionPointImg, pCutOffLineCenter, 1, Scalar.Green, -1);
            msg += FormatMsg("绘制拐点结束");

            coordinateSystemImg = lowBeamCutOffLine.Clone();
            DrawCoordinateSystem(coordinateSystemImg, pH0V0, pCutOffLineCenter);

            var toCheckLineX = (double)pH0V0.X - perDegreePixel * 2.5;
            var pHn2d5V2 = new Point(toCheckLineX, (double)pH0V0.Y - perDegreePixel * 2);
            var pHn2d5Vn2 = new Point(toCheckLineX, (double)pH0V0.Y + perDegreePixel * 2);
            DrawLowBeamGradientLine(coordinateSystemImg, pHn2d5V2, pHn2d5Vn2);

            // LB上方区域9点 + 下方区域1点
            //var listCiclesLb = new List<CircleWithGray>
            //{
            //    DrawPointCircleText(coordinateSystemImg, pH0V0, "0", "0", perDegreePixel), // LowBeam九点测试
            //    DrawPointCircleText(coordinateSystemImg, pH0V0, "0", "2", perDegreePixel), // LowBeam九点测试
            //    DrawPointCircleText(coordinateSystemImg, pH0V0, "0", "4", perDegreePixel), // LowBeam九点测试
            //    DrawPointCircleText(coordinateSystemImg, pH0V0, "-8", "0", perDegreePixel), // LowBeam九点测试
            //    DrawPointCircleText(coordinateSystemImg, pH0V0, "-8", "4", perDegreePixel), // LowBeam九点测试
            //    DrawPointCircleText(coordinateSystemImg, pH0V0, "-4", "2", perDegreePixel), // LowBeam九点测试
            //    DrawPointCircleText(coordinateSystemImg, pH0V0, "-4", "0", perDegreePixel), // LowBeam九点测试
            //    DrawPointCircleText(coordinateSystemImg, pH0V0, "4", "2", perDegreePixel), // LowBeam九点测试
            //    DrawPointCircleText(coordinateSystemImg, pH0V0, "8", "4", perDegreePixel), // LowBeam九点测试
            //    DrawPointCircleText(coordinateSystemImg, pH0V0, "0", "-11", perDegreePixel) // LowBeam三区测试_[HV,(0,-11)]
            //};
            var listCiclesLb = new List<CircleWithGray>();
            foreach (var lp in LbStandard)
                listCiclesLb.Add(DrawPointCircleText(coordinateSystemImg, pH0V0, lp.Point.X.ToString(), lp.Point.Y.ToString(),
                    perDegreePixel));

            msg += FormatMsg("绘制坐标系结束");

            if (targetStep == PesVisionAnalysisStep.SetCoordinateSystem)
                return (int)targetStep;

            if (lowBeamOrigin == null || lowBeamOrigin.Empty())
            {
                msg += FormatMsg("未导入近光图像");
                return (int)PesVisionAnalysisStep.SetCoordinateSystem;
            }

            lowBeamWithGrayResult = lowBeamOrigin.Clone();
            DrawCoordinateSystem(lowBeamWithGrayResult, pH0V0, pCutOffLineCenter);

            foreach (var c in listCiclesLb)
            {
                var grayC = GetCircularRoiMeanGray(lowBeamOrigin, c);
                c.Gray = grayC;
                msg += FormatMsg(string.Format("计算LB_{0}灰度值='{1}'结束", c.Name, c.Gray));
                DrawPointCircleText(lowBeamWithGrayResult, pH0V0, c.DegreeH, c.DegreeV, perDegreePixel, true, c.Gray);
            }

            var listPoint = new List<PointWithGray>();
            var slope = CalculateLineGradientSlope(lowBeamOrigin, pHn2d5V2, pHn2d5Vn2, ref listPoint);
            msg += FormatMsg(string.Format("计算LB_[HV(-2.5,±2)]的灰度值斜率='{0}'结束", slope));
            //DrawLowBeamGradientLine(lowBeamWithGrayResult, pHn2d5V2, pHn2d5Vn2);
            foreach (var p in listPoint)
                Cv2.Circle(lowBeamWithGrayResult, p.Point, 1, Scalar.Red, -1);

            //var maxBrightestCenterLb = GetBrightestCircularRoi(lowBeamOrigin, (int)(0.4 * perDegreePixel));
            //var maxBrghtestCenterLbDegreeH = Math.Round((maxBrightestCenterLb.X - pH0V0.X) * perPixelDegree, 1, MidpointRounding.AwayFromZero);
            //var maxBrghtestCenterLbDegreeV = Math.Round((0 - (maxBrightestCenterLb.Y - pH0V0.Y)) * perPixelDegree, 1, MidpointRounding.AwayFromZero);
            //var pEMaxLb = DrawPointCircleText(
            //    lowBeamWithGrayResult, pH0V0, maxBrghtestCenterLbDegreeH.ToString(CultureInfo.InvariantCulture), maxBrghtestCenterLbDegreeV.ToString(CultureInfo.InvariantCulture), perDegreePixel); // LowBeam EMax
            //var pEMaxGrayLb = GetCircularRoiMeanGray(lowBeamOrigin, pEMaxLb);
            //pEMaxLb.Gray = pEMaxGrayLb;
            //DrawPointCircleText(lowBeamWithGrayResult, pH0V0, pEMaxLb.DegreeH, pEMaxLb.DegreeV, perDegreePixel, true, pEMaxLb.Gray);
            //msg += FormatMsg(string.Format("计算LB_EMax结束，EMax在{0}，灰度值为{1}", pEMaxLb.Name, pEMaxLb.Gray));

            if (highBeamOrigin == null || highBeamOrigin.Empty())
            {
                msg += FormatMsg("未导入远光图像");
                return (int)PesVisionAnalysisStep.LowBeamAnalysis;
            }

            highBeamWithGrayResult = highBeamOrigin.Clone();

            //var listCiclesHb = new List<CircleWithGray>
            //{
            //    DrawPointCircleText(highBeamWithGrayResult, pH0V0, "-11", "0", perDegreePixel), // HighBeam四点测试
            //    DrawPointCircleText(highBeamWithGrayResult, pH0V0, "11", "0", perDegreePixel), // HighBeam四点测试
            //    DrawPointCircleText(highBeamWithGrayResult, pH0V0, "-20", "0", perDegreePixel), // HighBeam四点测试
            //    DrawPointCircleText(highBeamWithGrayResult, pH0V0, "20", "0", perDegreePixel), // HighBeam四点测试
            //    DrawPointCircleText(highBeamWithGrayResult, pH0V0, "0", "0", perDegreePixel), // HighBeam HV(0,0)点亮度
            //};
            var listCiclesHb = new List<CircleWithGray>();
            foreach (var hp in HbStandard)
                listCiclesHb.Add(DrawPointCircleText(highBeamWithGrayResult, pH0V0, hp.Point.X.ToString(), hp.Point.Y.ToString(), perDegreePixel));
            listCiclesHb.Add(DrawPointCircleText(highBeamWithGrayResult, pH0V0, "0", "0", perDegreePixel)); // HighBeam HV(0,0)点亮度

            foreach (var c in listCiclesHb)
            {
                var grayC = GetCircularRoiMeanGray(highBeamOrigin, c);
                c.Gray = grayC;
                msg += FormatMsg(string.Format("计算HB_{0}灰度值='{1}'结束", c.Name, c.Gray));
                DrawPointCircleText(highBeamWithGrayResult, pH0V0, c.DegreeH, c.DegreeV, perDegreePixel, true, c.Gray);
            }

            var maxBrightestCenterHb = GetBrightestCircularRoi(highBeamOrigin, (int)(0.1 * perDegreePixel));
            var maxBrghtestCenterHbDegreeH = Math.Round((maxBrightestCenterHb.X - pH0V0.X) * perPixelDegree, 1, MidpointRounding.AwayFromZero);
            var maxBrghtestCenterHbDegreeV = Math.Round((0 - (maxBrightestCenterHb.Y - pH0V0.Y)) * perPixelDegree, 1, MidpointRounding.AwayFromZero);
            var pEMaxHb = DrawPointCircleText(
                highBeamWithGrayResult, pH0V0, maxBrghtestCenterHbDegreeH.ToString(CultureInfo.InvariantCulture), maxBrghtestCenterHbDegreeV.ToString(CultureInfo.InvariantCulture), perDegreePixel); // LowBeam EMax
            var pEMaxGrayHb = GetCircularRoiMeanGray(highBeamOrigin, pEMaxHb);
            pEMaxHb.Gray = pEMaxGrayHb;
            DrawPointCircleText(highBeamWithGrayResult, pH0V0, pEMaxHb.DegreeH, pEMaxHb.DegreeV, perDegreePixel, true, pEMaxHb.Gray);
            msg += FormatMsg(string.Format("计算Hb_EMax结束，EMax在{0}，灰度值为{1}", pEMaxHb.Name, pEMaxHb.Gray));

            DrawCoordinateSystem(highBeamWithGrayResult, pH0V0, pCutOffLineCenter);

            return (int)targetStep;
        }

        private static void DrawCoordinateSystem(Mat src, Point pH0V0, Point? cutOffLineCenter = null)
        {
            Cv2.Line(src, new Point(0, pH0V0.Y), new Point(src.Width, pH0V0.Y), Scalar.Yellow, 2);
            Cv2.Line(src, new Point(pH0V0.X, 0), new Point(pH0V0.X, src.Height), Scalar.Yellow, 2);
            Cv2.Circle(src, pH0V0, 1, Scalar.Green, -1);

            if (cutOffLineCenter == null)
                return;
            var pCutOffLineCenter = (Point)cutOffLineCenter;
            Cv2.Circle(src, pCutOffLineCenter, 1, Scalar.Green, -1);
        }

        private static void DrawLowBeamGradientLine(Mat src, Point p1, Point p2)
        {
            Cv2.Line(src, p1, p2, Scalar.Green);
            Cv2.PutText(src, "[HV(-2.5,2)]", new Point(p1.X + 1, p1.Y - 1), HersheyFonts.HersheySimplex, 0.45, Scalar.Green);
            Cv2.PutText(src, "[HV(-2.5,-2)]", new Point(p2.X + 1, p2.Y - 1), HersheyFonts.HersheySimplex, 0.45, Scalar.Green);
        }

        private static Mat ImgDenoise(Mat toGaussianMat, double sigma)
        {
            var gaussian = toGaussianMat.Clone();
            Cv2.GaussianBlur(toGaussianMat, gaussian, new Size(sigma, sigma), 0);

            var s = Cv2.Split(gaussian);
            gaussian.Dispose();
            gaussian = s[2].Clone();

            return gaussian;
        }

        private static void CutOffLineAnalysis(
           Mat toAnalysisGrayMat, Rect roi, int scanBrightestStep, int maxYDiff,
           ref List<Point> validCorners,
           out Point p1, out Point p2, out Point p3, out Point p4, out Point pCenter, ref string msg)
        {
            var grayImage = toAnalysisGrayMat.Clone();
            //var verticalChanges =
            //    CalculateVerticalGradients(grayImage, roi.X, roi.X + roi.Width, roi.Y, roi.Y + roi.Height);
            var brightestPoints = new List<Point>();

            // 第一步：寻找并收集最亮点
            //for (var x = 0; x < grayImage.Width; x += scanBrightestStep)
            //{
            //    double maxValue = 0;
            //    var maxPoint = new Point(x, 0);

            //    for (var y = 0; y < grayImage.Height; y++)
            //    {
            //        double pixelValue = grayImage.At<byte>(y, x);
            //        if (!(pixelValue > maxValue))
            //            continue;
            //        maxValue = pixelValue;
            //        maxPoint = new Point(x, y);
            //    }

            //    brightestPoints.Add(maxPoint);
            //}
            for (var x = roi.X; x < roi.X + roi.Width; x += scanBrightestStep)
            {
                double maxValue = 0;
                var maxPoint = new Point(x, 0);

                for (var y = roi.Y; y < roi.Y + roi.Height; y++)
                {
                    double pixelValue = grayImage.At<byte>(y, x);
                    if (!(pixelValue > maxValue))
                        continue;
                    maxValue = pixelValue;
                    maxPoint = new Point(x, y);
                }

                brightestPoints.Add(maxPoint);
            }
            msg += FormatMsg("寻找并收集最亮点完成");

            //brightestPoints.Clear();
            //for (var i = roi.X; i < roi.X + roi.Width; i++)
            //{
            //    double minVal;
            //    double maxVal;
            //    Point minLocl;
            //    Point maxLoc;
            //    var rectOnImage = new Rect(i, roi.Y, 1, roi.Height);
            //    var tMat = new Mat(grayImage, rectOnImage);
            //    Cv2.MinMaxLoc(tMat, out minVal, out maxVal, out minLocl, out maxLoc);

            //    // 找到亮点的最中心处
            //    var binary = new Mat();
            //    Cv2.Threshold(tMat, binary, maxVal - 1, 255, ThresholdTypes.Binary);
            //    var moments = Cv2.Moments(binary, true);
            //    var centerX = (int)(moments.M10 / moments.M00);
            //    var centerY = (int)(moments.M01 / moments.M00);

            //    // 调整中心点坐标为原图坐标
            //    centerX += rectOnImage.X;
            //    centerY += rectOnImage.Y;
            //    var offsetX = Math.Abs(centerX - rectOnImage.X);
            //    var offsetY = Math.Abs(centerY - rectOnImage.Y);

            //    tMat.Dispose();
            //    brightestPoints.Add(new Point(centerX, centerY));
            //}

            grayImage.Dispose();
            GC.Collect();

            // 第二步：将找到ROI区域内的最亮点坐标
            //for (var i = 0; i < brightestPoints.Count - 1; i++)
            //{
            //    var rect = roi;

            //    //if (x >= 1480 && x <= 1670 && y >= 760 && y <= 960)
            //    if (brightestPoints[i].X >= (rect.X) &&
            //        brightestPoints[i].X <= rect.X + (rect.Width) &&
            //        brightestPoints[i].Y >= rect.Y &&
            //        brightestPoints[i].Y <= rect.Y + (rect.Height) &&
            //        brightestPoints[i + 1].X >= (rect.X) &&
            //        brightestPoints[i + 1].X <= rect.X + (rect.Width) &&
            //        brightestPoints[i + 1].Y >= rect.Y &&
            //        brightestPoints[i + 1].Y <= rect.Y + (rect.Height))
            //    {
            //        validCorners.Add(brightestPoints[i]);
            //        //validCorners.Add(brightestPoints[i + 1]);
            //    }
            //}
            validCorners.AddRange(brightestPoints);
            msg += FormatMsg("将找到ROI区域内的最亮点坐标完成");

            // 根据Y轴的偏差,分析拐点坐标
            var minXPoint = validCorners.OrderBy(p => p.X).First();
            //获取所有与最左侧点近似水平的点
            var horizontalPoints =
                validCorners.Where(p => Math.Abs(p.Y - minXPoint.Y) <= maxYDiff).ToList();
            // 剩余的点，不包括近似水平线上的点
            var remainingPoints = validCorners.Except(horizontalPoints).ToList();
            // 拟合并绘制第一条线
            FitLine(horizontalPoints, toAnalysisGrayMat.Width, out p1, out p2);
            // 拟合并绘制第二条线
            FitLine(remainingPoints, toAnalysisGrayMat.Width, out p3, out p4);
            // 两条直线交汇点
            pCenter = GetIntersection(p1, p2, p3, p4);

            msg += FormatMsg("分析拐点坐标完成");
        }

        public static List<int>[] CalculateVerticalGradients(Mat grayImage, int startX, int endX, int startY, int endY)
        {
            //var width = grayImage.Width;
            //var height = grayImage.Height;
            //var verticalChanges = new List<int>[width];

            //for (var x = 0; x < width; x = x + 1)
            //{
            //    verticalChanges[x] = new List<int>();
            //    for (var y = 0; y < height - 1; y++) // 注意避免越界
            //    {
            //        int currentPixel = grayImage.At<byte>(y, x);
            //        int nextPixel = grayImage.At<byte>(y + 1, x);
            //        var difference = Math.Abs(nextPixel - currentPixel);

            //        // 可以设定一个阈值来判断是否为有效的边界
            //        if (difference > 5) // 这个值可以根据实际情况调整
            //            verticalChanges[x].Add(y);
            //    }
            //}

            var width = endX - startX;
            var height = endY - startY;
            var verticalChanges = new List<int>[width];

            for (var x = 0; x < width; x = x + 1)
            {
                verticalChanges[x] = new List<int>();
                for (var y = 0; y < height - 1; y++) // 注意避免越界
                {
                    int currentPixel = grayImage.At<byte>(startY + y, startX + x);
                    int nextPixel = grayImage.At<byte>(startY + y + 1, startX + x);
                    var difference = Math.Abs(nextPixel - currentPixel);

                    // 可以设定一个阈值来判断是否为有效的边界
                    if (difference > 5) // 这个值可以根据实际情况调整
                        verticalChanges[x].Add(y);
                }
            }

            return verticalChanges;
        }

        private static void FitLine(IReadOnlyCollection<Point> points, int imgWidth, out Point p1, out Point p2)
        {
            p1 = new Point(0, 0);
            p2 = new Point(0, 0);

            if (points.Count < 2)
                return;

            // 使用 FitLine 获得线参数
            var lineParameters = Cv2.FitLine(points, DistanceTypes.L2, 0, 0.01, 0.01);

            var vx = lineParameters.Vx;
            var vy = lineParameters.Vy;
            var x0 = lineParameters.X1;
            var y0 = lineParameters.Y1;

            // 计算两个端点
            // y = y0 + (x - x0) * vy / vx
            var drawX1 = 0;
            var drawY1 = (int)(y0 + (drawX1 - x0) * vy / vx);
            var drawX2 = imgWidth;
            var drawY2 = (int)(y0 + (drawX2 - x0) * vy / vx);

            p1 = new Point(drawX1, drawY1);
            p2 = new Point(drawX2, drawY2);

            // 绘制直线
            //Cv2.Line(image, new Point(drawX1, drawY1), new Point(drawX2, drawY2), color, 2);
        }

        private static Point GetIntersection(Point a1, Point a2, Point b1, Point b2)
        {
            double deltaA = a2.X - a1.X;
            double deltaB = b2.X - b1.X;

            if (deltaA == 0 || deltaB == 0) // Check if either line is vertical
            {
                // If both lines are vertical, they are parallel or coincide
                if (Math.Abs(deltaA - deltaB) <= 0)
                    //return null;
                    return new Point(0, 0);
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
                //return null;
                return new Point(0, 0);

            // Calculate intersection point using Cramer's Rule
            var x = (b2Line * c1Line - a2Line * c2Line) / det;
            var y = (a1Line * c2Line - b1Line * c1Line) / det;

            return new Point((int)x, (int)y);
        }

        private static string FormatMsg(string content)
        {
            var msg = string.Format(@"{0},耗时:{1}ms", content,
                         HighPrecisionTimer.GetTimestampIntervalMs(_startTs, HighPrecisionTimer.GetTimestamp())) +
                     Environment.NewLine;
            _startTs = HighPrecisionTimer.GetTimestamp();
            return msg;
        }

        private static Point FormatPoint(Point pH0V0, double degreeH, double degreeV, double perPixelDegree)
        {
            var x = pH0V0.X + (degreeH * perPixelDegree);
            var y = pH0V0.Y - (degreeV * perPixelDegree);

            return new Point(x, y);
        }

        private static CircleWithGray DrawPointCircleText(Mat mat, Point pH0V0, string degreeH, string degreeV,
            double perDegreePixel, bool isShowGray = false, byte gray = 0)
        {
            var h = double.Parse(degreeH);
            var v = double.Parse(degreeV);
            var radius = (int)(0.1 * perDegreePixel);

            var pToDraw0 = FormatPoint(pH0V0, h, v, perDegreePixel);
            var pName = string.Format("[HV({0},{1})]", degreeH, degreeV);
            if (isShowGray)
                pName += string.Format("='{0}'", gray);
            Cv2.Circle(mat, pToDraw0, 1, Scalar.Red, -1);
            Cv2.PutText(mat, pName, new Point(pToDraw0.X + 1, pToDraw0.Y - radius - 10), HersheyFonts.HersheySimplex, 0.45, Scalar.Red);
            Cv2.Circle(mat, pToDraw0, radius, Scalar.Red);

            var circle = new CircleWithGray
            {
                Circle = new CircleSegment(pToDraw0, radius),
                Name = pName,
                DegreeH = degreeH,
                DegreeV = degreeV
            };
            return circle;
        }

        private static byte GetCircularRoiMeanGray(Mat src, CircleWithGray circle)
        {
            // 确保输入图像是灰度图
            var grayImage = new Mat();
            if (src.Channels() == 3)
                Cv2.CvtColor(src, grayImage, ColorConversionCodes.BGR2GRAY);
            else
                grayImage = src.Clone();

            // 创建一个与原始图像相同大小的掩膜
            var mask = new Mat(grayImage.Size(), MatType.CV_8UC1, Scalar.Black);
            // 在掩膜上绘制白色圆形
            Cv2.Circle(mask, new Point((int)circle.Circle.Center.X, (int)circle.Circle.Center.Y), (int)circle.Circle.Radius, Scalar.White, -1);
            // 计算圆形区域的平均值
            var meanVal = Cv2.Mean(grayImage, mask);

            grayImage.Dispose();
            mask.Dispose();
            GC.Collect();

            // 返回平均灰度值
            return (byte)Math.Round(meanVal.Val0, 0, MidpointRounding.AwayFromZero);
        }

        private static double CalculateLineGradientSlope(Mat image, Point start, Point end, ref List<PointWithGray> resultsPointWithGrays)
        {
            if (resultsPointWithGrays == null)
                throw new ArgumentNullException(nameof(resultsPointWithGrays));

            // 确保图像是灰度图
            var grayImage = new Mat();
            if (image.Channels() == 3)
                Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);
            else
                grayImage = image.Clone();

            // 获取直线上的所有点
            var linePoints = GetLinePoints(start, end);
            foreach (var point in linePoints)
            {
                var dummy = grayImage.At<byte>(point.Y, point.X);
                Console.WriteLine("{0},{1}", point.Y, dummy);
            }

            grayImage.Dispose();
            GC.Collect();

            resultsPointWithGrays = AnalyzeVerticalLine(image, start.X, start.Y, end.Y);

            // 获取直线上的灰度值
            //var grayValues = linePoints.Select(point => grayImage.At<byte>(point.Y, point.X)).Select(dummy => (int)dummy).ToList();
            var grayValues = resultsPointWithGrays.Select(point => point.Gray).ToList();

            // 计算梯度
            var gradients = new List<double>();
            for (var i = 1; i < grayValues.Count; i++)
            {
                //Console.WriteLine("计算梯度: " + grayValues[i]);
                //Console.WriteLine("计算梯度: " + grayValues[i - 1]);
                gradients.Add(grayValues[i] - grayValues[i - 1]);
            }

            // 计算梯度的斜率（使用最小二乘法）
            return CalculateSlope(gradients);
        }

        private static List<PointWithGray> AnalyzeVerticalLine(Mat src, int pX, int pStartY, int pEndY, int smoothingKernelSize = 5, int brightnessThreshold = 20)
        {
            {
                // 应用高斯模糊以平滑图像
                var smoothed = new Mat();
                Cv2.GaussianBlur(src, smoothed, new Size(smoothingKernelSize, smoothingKernelSize), 0);

                //pStartY = 568;
                //pEndY = 582;

                var midX = pX;
                var grayValues = new List<int>();
                var pointLists = new List<Point>();
                var maxBrightness = 0;
                var maxBrightnessIndex = -1;
                var minBrightness = 255;

                // 从上到下遍历像素
                for (var y = pStartY; y < pEndY; y++)
                {
                    int grayValue = smoothed.At<byte>(y, midX);
                    grayValues.Add(grayValue);
                    pointLists.Add(new Point(midX, y));

                    if (grayValue > maxBrightness)
                    {
                        maxBrightness = grayValue;
                        maxBrightnessIndex = y;
                    }

                    if (grayValue < minBrightness)
                    {
                        minBrightness = grayValue;
                    }
                }

                smoothed.Dispose();
                GC.Collect();

                // 计算亮度范围
                var brightnessRange = maxBrightness - minBrightness;

                // 找到微亮点的索引
                var slightlyBrightIndex = grayValues.FindIndex(v => v >= minBrightness + brightnessThreshold);

                var listPoint = new List<PointWithGray>();

                // 计算并输出梯度
                for (var i = slightlyBrightIndex + 1; i < grayValues.Count; i++)
                {
                    var gradient = i > slightlyBrightIndex ? grayValues[i] - grayValues[i - 1] : 0;

                    listPoint.Add(new PointWithGray
                    {
                        Point = pointLists[i],
                        Gray = (byte)grayValues[i]
                    });

                    if (i == maxBrightnessIndex)
                        break;
                }

                return listPoint;
            }
        }

        private static IEnumerable<Point> GetLinePoints(Point start, Point end)
        {
            var points = new List<Point>();
            var dx = Math.Abs(end.X - start.X);
            var dy = Math.Abs(end.Y - start.Y);
            var sx = start.X < end.X ? 1 : -1;
            var sy = start.Y < end.Y ? 1 : -1;
            var err = dx - dy;

            while (true)
            {
                points.Add(new Point(start.X, start.Y));
                if (start.X == end.X && start.Y == end.Y) break;
                var e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    start.X += sx;
                }

                if (e2 >= dx)
                    continue;
                err += dx;
                start.Y += sy;
            }
            return points;
        }

        private static double CalculateSlope(IReadOnlyCollection<double> gradients)
        {
            var n = gradients.Count;
            double sumX = Enumerable.Range(0, n).Sum();
            var sumY = gradients.Sum();
            var sumXy = Enumerable.Range(0, n).Zip(gradients, (x, y) => x * y).Sum();
            double sumX2 = Enumerable.Range(0, n).Select(x => x * x).Sum();

            // 计算斜率 (y = mx + b 中的 m)
            var slope = (n * sumXy - sumX * sumY) / (n * sumX2 - sumX * sumX);
            return Math.Round(slope, 4, MidpointRounding.AwayFromZero);
        }

        private static Point GetBrightestCircularRoi(Mat src, int radius)
        {
            // 确保输入图像是灰度图
            var grayImage = new Mat();
            if (src.Channels() == 3)
                Cv2.CvtColor(src, grayImage, ColorConversionCodes.BGR2GRAY);
            else
                grayImage = src.Clone();

            var rows = grayImage.Rows;
            var cols = grayImage.Cols;

            double minVal, maxVal;
            Point minLoc, maxLoc;

            // 找到最小和最大像素值及其位置
            Cv2.MinMaxLoc(grayImage, out minVal, out maxVal, out minLoc, out maxLoc);

            grayImage.Dispose();
            GC.Collect();

            return maxLoc;
        }

        public enum PesVisionAnalysisStep
        {
            Denoise,

            CutOffLineContour,

            InflectionPoint,

            SetCoordinateSystem,

            LowBeamAnalysis,

            HighBeamAnalysis
        }

        public class PointWithGray
        {
            public Point Point;
            public byte Gray;
        }

        internal class CircleWithGray
        {
            public string Name;
            public byte Gray;
            public CircleSegment Circle;
            public string DegreeH;
            public string DegreeV;
        }
    }
}
