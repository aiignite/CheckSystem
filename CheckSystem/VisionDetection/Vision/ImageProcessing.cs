using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;

namespace CheckSystem.VisionDetection.Vision
{
    /// <summary>
    /// 图像处理
    /// </summary>
    public static class ImageProcessing
    {
        public delegate void PushVisionImgEventHandle(VisionImage visionImage);
        public static event PushVisionImgEventHandle PushVisionImgMsg;

        /// <summary>
        /// 在VisionImage上标记出轮廓索引
        /// </summary>
        /// <param name="visionImage"></param>
        /// <param name="rois"></param>
        public static void DrawContourCountInOverlay(VisionImage visionImage, ViewerRoi rois)
        {
            var textOptions = new OverlayTextOptions("Arial", 20) { TextDecoration = { Bold = false } };
            visionImage.Overlays.Default.Clear();

            for (var i = 0; i < rois.Count; i++)
            {
                var shape = rois[i].Shape;

                var shapeType = shape.GetType();
                PointContour temp = null;
                if (shapeType == typeof(PolygonContour))
                    temp = new PointContour(((PolygonContour)shape).Points[0].X, ((PolygonContour)shape).Points[0].Y - 14);
                else if (shapeType == typeof(RectangleContour))
                    temp = new PointContour(((RectangleContour)shape).Left, ((RectangleContour)shape).Top);
                else if (shapeType == typeof(RotatedRectangleContour))
                    temp = new PointContour(((RotatedRectangleContour)shape).Center.X - ((RotatedRectangleContour)shape).Width / 2, ((RotatedRectangleContour)shape).Center.Y - ((RotatedRectangleContour)shape).Height / 2 - 14);
                visionImage.Overlays.Default.AddText((i + 1).ToString(), temp, Rgb32Value.RedColor, textOptions);
            }
        }

        /// <summary>
        /// 在VisionImage上标记出轮廓索引
        /// </summary>
        /// <param name="visionImage"></param>
        /// <param name="shape"></param>
        /// <param name="text"></param>
        public static void DrawContourCountInOverlay(VisionImage visionImage, Shape shape, string text)
        {
            var textOptions = new OverlayTextOptions("Arial", 10) { TextDecoration = { Bold = false } };
            //visionImage.Overlays.Default.Clear();

            var shapeType = shape.GetType();
            PointContour temp = null;
            if (shapeType == typeof(PolygonContour))
                temp = new PointContour(((PolygonContour)shape).Points[0].X, ((PolygonContour)shape).Points[0].Y - 14);
            else if (shapeType == typeof(RectangleContour))
                temp = new PointContour(((RectangleContour)shape).Left, ((RectangleContour)shape).Top);
            visionImage.Overlays.Default.AddText(text, temp, Rgb32Value.RedColor, textOptions);
        }

        /// <summary>
        /// 图片色素提取
        /// </summary>
        /// <param name="imageSource">原图</param>
        /// <param name="colorPlaneExtractionType">色素提取类型</param>
        /// <returns></returns>
        public static VisionImage ColorPlaneExtraction(
            VisionImage imageSource, ColorPlaneExtractionType colorPlaneExtractionType)
        {
            var image = new VisionImage(ImageType.U8, 7);
            Algorithms.Copy(imageSource, image);

            if (imageSource.Type == ImageType.U8)
                return image;

            try
            {
                // Extract the blue color plane and copy it to the main image.
                using (var plane = new VisionImage(ImageType.U8, 7))
                {
                    switch (colorPlaneExtractionType)
                    {
                        case ColorPlaneExtractionType.Default:
                            return image;

                        case ColorPlaneExtractionType.Red:
                            Algorithms.ExtractColorPlanes(image, ColorMode.Rgb, plane, null, null);
                            break;

                        case ColorPlaneExtractionType.Green:
                            Algorithms.ExtractColorPlanes(image, ColorMode.Rgb, null, plane, null);
                            break;

                        case ColorPlaneExtractionType.Blue:
                            Algorithms.ExtractColorPlanes(image, ColorMode.Rgb, null, null, plane);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("colorPlaneExtractionType", colorPlaneExtractionType, null);
                    }

                    Algorithms.Copy(plane, image);
                    plane.Dispose();
                }
            }
            catch
            {
                return null;
            }

            // Lookup Table: Math Lookup
            // Transforms an image by applying a transfer function to the value of each pixel.
            //Algorithms.MathLookup(image, image, MathLookupOperator.Exp, 1.5);

            return image;
        }

        public static VisionImage LookupTable(VisionImage imageSource, LookupTableType lookupTableType)
        {
            var image = new VisionImage(ImageType.U8, 7);
            Algorithms.Copy(imageSource, image);

            try
            {
                // Lookup Table: Math Lookup
                // Transforms an image by applying a transfer function to the value of each pixel.

                switch (lookupTableType)
                {
                    case LookupTableType.ImageSource:
                        return image;

                    case LookupTableType.Logarithmic:
                        Algorithms.MathLookup(imageSource, image, MathLookupOperator.Log);
                        break;

                    case LookupTableType.Exponential:
                        Algorithms.MathLookup(imageSource, image, MathLookupOperator.Exp);
                        break;

                    case LookupTableType.Square:
                        Algorithms.MathLookup(imageSource, image, MathLookupOperator.Square);
                        break;

                    case LookupTableType.SquareRoot:
                        Algorithms.MathLookup(imageSource, image, MathLookupOperator.SquareRoot);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("lookupTableType", lookupTableType, null);
                }



            }
            catch (Exception)
            {
                return null;
            }

            return image;
        }

        public static VisionImage GetImageByThreshold(VisionImage imageSource, Range range)
        {
            var image = new VisionImage(ImageType.U8, 7);
            Algorithms.Copy(imageSource, image);

            Algorithms.Threshold(imageSource, image, range, true, 1);
            return image;
        }

        public static void ImageRotationImage(VisionImage image, double angle, bool isMaintainSize)
        {
            // Geometry: Rotation
            //PixelValue vaFillValue = Functions.IVA_SetPixelValue(image.Type, 0, 0, 0, 0);
            PixelValue vaFillValue;
            switch (image.Type)
            {
                case ImageType.U8:
                case ImageType.I16:
                case ImageType.U16:
                case ImageType.Single:
                    vaFillValue = new PixelValue(0);
                    break;
                case ImageType.Complex:
                    vaFillValue = new PixelValue(new Complex());
                    break;
                case ImageType.Rgb32:
                    vaFillValue = new PixelValue(new Rgb32Value(0, 0, 0));
                    break;
                case ImageType.RgbU64:
                    vaFillValue = new PixelValue(new RgbU64Value(0, 0, 0));
                    break;
                case ImageType.Hsl32:
                    vaFillValue = new PixelValue(new Hsl32Value());
                    break;
                default:
                    vaFillValue = new PixelValue();
                    break;
            }

            Algorithms.Rotate(image, image, angle, vaFillValue, InterpolationMethod.Bilinear, isMaintainSize);
        }

        public static List<CircleReport> FindCircle(
            VisionImage imageSource, Range radiusRange)
        {
            using (var circleImg = new VisionImage(ImageType.U8, 7))
            {
                Algorithms.Copy(imageSource, circleImg);

                // Automatic Threshold
                Algorithms.AutoThreshold(circleImg, circleImg, 2, ThresholdMethod.Metric);

                //var img = GetImageByThreshold(circleImg, thresholdRange);

                var circleResults = Algorithms.FindCircles(circleImg, circleImg, radiusRange);
                var returnList = circleResults.ToList();

                circleImg.Dispose();

                return returnList;
            }
        }

        /// <summary>
        /// 获取轮廓在图片中的灰度值
        /// </summary>
        /// <param name="imageSource">源图</param>
        /// <param name="shape">轮廓</param>
        /// <returns></returns>
        public static double GetGrayValue(VisionImage imageSource, Shape shape)
        {
            try
            {

                shape.ConvertToRoi();
                //using (var roi = new Roi { shape })
                {
                    QuantifyReport vaQuantifyReport;

                    using (var imageMask = new VisionImage(ImageType.U8, 7))
                    {
                        // Creates the mask based on the region of interest.
                        Algorithms.RoiToMask(imageMask, shape.ConvertToRoi(), new PixelValue(255), imageSource);
                        // Calculates statistical parameters on an image.
                        vaQuantifyReport = Algorithms.Quantify(imageSource, imageMask);
                        imageMask.Dispose();
                    }
                    //roi.Dispose();
                    return Math.Round(vaQuantifyReport.Global.Mean, 2, MidpointRounding.AwayFromZero);
                }
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// 获取模块匹配结果
        /// </summary>
        /// <param name="image"></param>
        /// <param name="rect"></param>
        /// <param name="templateFilePath"></param>
        /// <param name="result"></param>
        /// <param name="columnStepSize"></param>
        /// <param name="extractionMode"></param>
        /// <param name="filterSize"></param>
        /// <param name="maximumEndPointGap"></param>
        /// <param name="minimumLength"></param>
        /// <param name="rowStepSize"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static bool GetGeometricMatchingResult(VisionImage image, RectangleContour rect, string templateFilePath,
            out GeometricMatchingResultStruct result, int columnStepSize = 15, ExtractionMode extractionMode = ExtractionMode.NormalImage,
            EdgeFilterSize filterSize = EdgeFilterSize.Fine, int maximumEndPointGap = 10, int minimumLength = 25,
            int rowStepSize = 15, int threshold = 75)
        {
            var vaCurveOptions = new CurveOptions
            {
                ColumnStepSize = columnStepSize,
                ExtractionMode = extractionMode,
                FilterSize = filterSize,
                MaximumEndPointGap = maximumEndPointGap,
                MinimumLength = minimumLength,
                RowStepSize = rowStepSize,
                Threshold = threshold
            };

            var matchGpmOptions = new MatchGeometricPatternEdgeBasedOptions
            {
                Advanced =
                {
                    ContrastMode = ContrastMode.Original,
                    MatchStrategy = GeometricMatchingSearchStrategy.Balanced
                },
                MinimumMatchScore = 700,
                Mode = GeometricMatchModes.RotationInvariant | GeometricMatchModes.OcclusionInvariant,
                NumberOfMatchesRequested = 1
            };
            double[] vaRangesMin = { 0, 0, 50, 0 };
            double[] vaRangesMax = { 360, 0, 200, 25 };
            matchGpmOptions.OcclusionRange = new Range(vaRangesMin[3], vaRangesMax[3]);
            matchGpmOptions.RotationAngleRanges.Add(new Range(vaRangesMin[0], vaRangesMax[0]));
            matchGpmOptions.RotationAngleRanges.Add(new Range(vaRangesMin[1], vaRangesMax[1]));
            matchGpmOptions.ScaleRange = new Range(vaRangesMin[2], vaRangesMax[2]);
            matchGpmOptions.SubpixelAccuracy = false;

            // Creates the image template.
            using (var imageTemplate = new VisionImage(ImageType.U8, 7))
            {
                // Read the image template.
                imageTemplate.ReadFile(templateFilePath);
                Algorithms.LearnGeometricPatternEdgeBased(imageTemplate);

                var roi = new Roi();
                //var vaRect = new RectangleContour(rect.Left, rect.Top, rect.Width, rect.Height);
                var vaRect = new RectangleContour(0, 0, image.Width, image.Height);
                roi.Add(vaRect);

                var gpmResults = Algorithms.MatchGeometricPatternEdgeBased(image, imageTemplate, vaCurveOptions, matchGpmOptions, roi);
                roi.Dispose();

                if (gpmResults.Any())
                {
                    result = new GeometricMatchingResultStruct
                    {
                        Position = new PointF((float)gpmResults[0].Position.X, (float)gpmResults[0].Position.Y),
                        Rotation = gpmResults[0].Rotation
                    };
                    //new[] { gpmResults[0].Position.X, gpmResults[0].Position.Y, gpmResults[0].Rotation };
                    return true;
                }

                result = new GeometricMatchingResultStruct
                {
                    Position = new PointF(-9999.0f, -9999.0f),
                    Rotation = -9999.0
                };
                return false;
            }
        }

        public static RotatedRectangleContour GetRotatedRecvByString(string rect)
        {
            var sp = rect.Split(',');
            return new RotatedRectangleContour(new PointContour(double.Parse(sp[0]), double.Parse(sp[1])),
                double.Parse(sp[2]), double.Parse(sp[3]), double.Parse(sp[4]));
        }

        /// <summary>
        /// 根据字符串获取矩形
        /// 格式0,0,0,0
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static RectangleContour GetRectByString(string rect)
        {
            var sp = rect.Split(',');
            return new RectangleContour(
                (int)double.Parse(sp[0]),
                (int)double.Parse(sp[1]),
                (int)double.Parse(sp[2]),
                (int)double.Parse(sp[3]));
        }

        /// <summary>
        /// 根据字符串获取带有4个点的多边形
        /// 格式0,0,1,1,2,2,3,3
        /// </summary>
        /// <param name="polygonContour"></param>
        /// <returns></returns>
        public static PolygonContour GetPolygonContourByString(string polygonContour)
        {
            var sp = polygonContour.Split(',');
            var p1 = new PointContour((int)double.Parse(sp[0]), (int)double.Parse(sp[1]));
            var p2 = new PointContour((int)double.Parse(sp[2]), (int)double.Parse(sp[3]));
            var p3 = new PointContour((int)double.Parse(sp[4]), (int)double.Parse(sp[5]));
            var p4 = new PointContour((int)double.Parse(sp[6]), (int)double.Parse(sp[7]));

            return GetPolygonContour((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, (int)p3.X, (int)p3.Y, (int)p4.X, (int)p4.Y);
        }

        /// <summary>
        /// 根据字符串获取在二维平面中定义一个点
        /// 格式0,0
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point GetPointByString(string point)
        {
            var sp = point.Split(',');
            return new Point(int.Parse(sp[0]), int.Parse(sp[1]));
        }

        public static string GetStringRect(RectangleContour rect)
        {
            return string.Format(@"{0},{1},{2},{3}", rect.Left, rect.Top, rect.Width, rect.Height);
        }

        public static string GetStringPolygonContour(PolygonContour polygonContour)
        {
            var returnStr = polygonContour.Points.Aggregate(string.Empty, (current, p) => current + (p.X + "," + p.Y) + ",");
            returnStr = returnStr.Trim(',');

            return returnStr;
        }

        public static string GetStringRotatedRect(RotatedRectangleContour rect)
        {
            return string.Format(@"{0},{1},{2},{3},{4}", rect.Center.X, rect.Center.Y, rect.Width, rect.Height, rect.Angle);
        }

        /// <summary>
        /// 获取对角线焦点
        /// </summary>
        /// <param name="contourObj"></param>
        /// <returns></returns>
        private static PointContour GetPointContour(object contourObj)
        {
            var shapeType = contourObj.GetType();
            if (shapeType == typeof(PolygonContour))
                return GetPolygonDiagonalPoint((PolygonContour)contourObj);
            else if (shapeType == typeof(RectangleContour))
                return GetRectangleDiagonalPoint((RectangleContour)contourObj);
            else
                return null;
        }

        /// <summary>
        /// 获取多边形对角线交点
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        private static PointContour GetPolygonDiagonalPoint(OpenClosedContour polygon)
        {
            var p1 = polygon.Points[0];
            var p2 = polygon.Points[1];
            var p3 = polygon.Points[2];
            var p4 = polygon.Points[3];
            var dX = ((p3.X - p1.X) * (p4.X - p2.X) * (p2.Y - p1.Y) + p1.X * (p3.Y - p1.Y) * (p4.X - p2.X) -
                      p2.X * (p4.Y - p2.Y) * (p3.X - p1.X))
                     / ((p3.Y - p1.Y) * (p4.X - p2.X) - (p4.Y - p2.Y) * (p3.X - p1.X));
            var dY = (p3.Y - p1.Y) * ((p4.X - p2.X) * (p2.Y - p1.Y) + (p1.X - p2.X) * (p4.Y - p2.Y))
                     / ((p3.Y - p1.Y) * (p4.X - p2.X) - (p4.Y - p2.Y) * (p3.X - p1.X)) + p1.Y;

            var point = new PointContour(dX - 15, dY + 20);
            return point;

            //return new PointContour(polygon.Points[0].X, polygon.Points[0].Y - 14);
        }

        /// <summary>
        /// 获取矩形对角线交点
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private static PointContour GetRectangleDiagonalPoint(RectangleContour rect)
        {
            var dX = rect.Left + rect.Width / 2;
            var dY = rect.Top + rect.Height / 2;
            var point = new PointContour(dX - 15, dY + 20);
            return point;

            //return new PointContour(rect.Left, rect.Top);
        }

        /// <summary>
        /// 获取多边形
        /// 默认形状是200*100的矩形
        /// </summary>
        /// <param name="leftTopX"></param>
        /// <param name="leftTopY"></param>
        /// <param name="rightTopX"></param>
        /// <param name="rightTopY"></param>
        /// <param name="rightBottomX"></param>
        /// <param name="rightBottomY"></param>
        /// <param name="leftBottomX"></param>
        /// <param name="leftBottomY"></param>
        /// <returns></returns>
        public static PolygonContour GetPolygonContour(
            int leftTopX = 0,
            int leftTopY = 0,
            int rightTopX = 200,
            int rightTopY = 0,
            int rightBottomX = 200,
            int rightBottomY = 100,
            int leftBottomX = 0,
            int leftBottomY = 100)
        {
            IList<PointContour> points = new List<PointContour>();
            points.Add(new PointContour(new Point(leftTopX, leftTopY))); // 左上
            points.Add(new PointContour(new Point(rightTopX, rightTopY))); // 右上
            points.Add(new PointContour(new Point(rightBottomX, rightBottomY))); // 右下
            points.Add(new PointContour(new Point(leftBottomX, leftBottomY))); // 左下
            return new PolygonContour(points);
        }

        public static void CreateNiTemplateImg(string filePath)
        {
            using (var screenImgBit = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height))
            {
                var g = Graphics.FromImage(screenImgBit);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.AllScreens[0].Bounds.Size);
                //for (var x = 0; x < screenImgBit.Width; x++)
                //{
                //    for (var y = 0; y < screenImgBit.Height; y++)
                //    {
                //        var pixel = screenImgBit.GetPixel(x, y);
                //        var val = (pixel.R + pixel.G + pixel.B)/3;
                //        screenImgBit.SetPixel(x, y, Color.FromArgb(val, val, val));
                //    }
                //}

                //if (!File.Exists(tempImgFile))
                //    File.Create(tempImgFile);
                screenImgBit.Save(filePath);
                g.Dispose();

                var fileInfo =
                    Algorithms.GetFileInformation(filePath);
                using (var img = new VisionImage(fileInfo.ImageType))
                {
                    img.ReadFile(filePath);

                    var cImg =
                        ColorPlaneExtraction(img, ColorPlaneExtractionType.Red);
                    Algorithms.Copy(cImg, img);
                    cImg.Dispose();

                    img.WriteBmpFile(filePath);
                }
            }
        }

        /// <summary>
        /// 色素提取类型
        /// </summary>
        public enum ColorPlaneExtractionType
        {
            /// <summary>
            /// 默认，不提取色素
            /// </summary>
            Default,

            /// <summary>
            /// 提取红色
            /// </summary>
            Red,

            /// <summary>
            /// 提取绿色
            /// </summary>
            Green,

            /// <summary>
            /// 提取蓝色
            /// </summary>
            Blue
        }

        /// <summary>
        /// 对一幅图像应用查找表以改善对比度与亮度
        /// </summary>
        public enum LookupTableType
        {
            /// <summary>
            /// 原始图像
            /// </summary>
            ImageSource,

            /// <summary>
            /// 均衡图像
            /// </summary>
            //Equalize,

            /// <summary>
            /// 翻转图像
            /// 翻转像素值
            /// 显示原始图像的底片
            /// </summary>
            //Reverse,

            /// <summary>
            /// 对图像应用对数变换
            /// 以增强暗区的亮度与对比度
            /// </summary>
            Logarithmic,

            /// <summary>
            /// 对图像应用指数变换
            /// 以减弱亮区亮度从而提高亮区对比度
            /// </summary>
            Exponential,

            /// <summary>
            /// 平方
            /// 减少暗区对比度
            /// 类似于指数但有更平缓的效果
            /// </summary>
            Square,

            /// <summary>
            /// 平方根
            /// 减少亮度对比度
            /// 类似于指数但有更平缓的效果
            /// </summary>
            SquareRoot
        }

        /// <summary>
        /// Geometric Matching Result
        /// </summary>
        public struct GeometricMatchingResultStruct
        {
            /// <summary>
            /// Geometric Matching Position Result
            /// </summary>
            public PointF Position { get; set; }

            /// <summary>
            /// /// <summary>
            /// Geometric Matching Angle Result
            /// </summary>
            /// </summary>
            public double Rotation { get; set; }
        }

        public static VisionImage ConvertBytesToVisionImg(
           byte[] source, int width, int heigh, bool iscolor)
        {
            VisionImage visionImage;
            try
            {
                if (iscolor)
                {
                    visionImage = new VisionImage(ImageType.Rgb32, 0);
                    visionImage.SetSize(width, heigh);
                    var source1 = new byte[width * heigh * 4];
                    var num1 = 0;
                    var num2 = 0;
                    while (num2 < source.Length)
                    {
                        var numArray1 = source1;
                        var index1 = num1;
                        var num3 = 1;
                        var num4 = index1 + num3;
                        var numArray2 = source;
                        var index2 = num2;
                        var num5 = 1;
                        var num6 = index2 + num5;
                        var num7 = (int)numArray2[index2];
                        numArray1[index1] = (byte)num7;
                        var numArray3 = source1;
                        var index3 = num4;
                        var num8 = 1;
                        var num9 = index3 + num8;
                        var numArray4 = source;
                        var index4 = num6;
                        var num10 = 1;
                        var num11 = index4 + num10;
                        var num12 = (int)numArray4[index4];
                        numArray3[index3] = (byte)num12;
                        var numArray5 = source1;
                        var index5 = num9;
                        var num13 = 1;
                        var num14 = index5 + num13;
                        var numArray6 = source;
                        var index6 = num11;
                        var num15 = 1;
                        num2 = index6 + num15;
                        var num16 = (int)numArray6[index6];
                        numArray5[index5] = (byte)num16;
                        var numArray7 = source1;
                        var index7 = num14;
                        var num17 = 1;
                        num1 = index7 + num17;
                        var num18 = (int)byte.MaxValue;
                        numArray7[index7] = (byte)num18;
                    }
                    var num19 = width * 4;
                    var num20 = Convert.ToInt32(visionImage.LineWidthInBytes);
                    var num21 = num20 - num19;
                    if (num19 == num20)
                    {
                        Marshal.Copy(source1, 0, visionImage.StartPtr, source1.Length);
                    }
                    else
                    {
                        var source2 = new byte[num20 * heigh];
                        var num3 = 0;
                        for (var index = 0; index < source2.Length; ++index)
                        {
                            if (index % num20 == 0 && index > num21)
                                num3 += num21;
                            if (index - num3 < source1.Length)
                                source2[index] = source1[index - num3];
                        }
                        Marshal.Copy(source2, 0, visionImage.StartPtr, source2.Length);
                    }
                }
                else
                {
                    visionImage = new VisionImage(ImageType.U8, 0);
                    visionImage.SetSize(width, heigh);
                    var num1 = width;
                    var num2 = Convert.ToInt32(visionImage.LineWidthInBytes);
                    var num3 = num2 - num1;
                    if (num1 == num2)
                    {
                        Marshal.Copy(source, 0, visionImage.StartPtr, source.Length);
                    }
                    else
                    {
                        var source1 = new byte[num2 * heigh];
                        var num4 = 0;
                        for (var index = 0; index < source1.Length; ++index)
                        {
                            if (index % num2 == 0 && index > num3)
                                num4 += num3;
                            if (index - num4 < source.Length)
                                source1[index] = source[index - num4];
                        }
                        Marshal.Copy(source1, 0, visionImage.StartPtr, source1.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return visionImage;
        }

        public static void OnPushVisionImgMsg(VisionImage visionimage)
        {
            var handler = PushVisionImgMsg;
            if (handler != null) handler(visionimage);
        }
    }
}
