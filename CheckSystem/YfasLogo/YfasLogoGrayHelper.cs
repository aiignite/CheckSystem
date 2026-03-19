using NationalInstruments.Vision;
using Newtonsoft.Json;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace CheckSystem.YfasLogo
{
    public static class YfasLogoGrayHelper
    {
        public static GrayResult OvalGray(Mat srcMat, string ovalObject)
        {
            try
            {
                var oval = JsonConvert.DeserializeObject<OvalContour>(ovalObject);

                var centerX = (int)(oval.Left + oval.Width / 2);
                var centerY = (int)(oval.Top + oval.Height / 2);

                var center = new Point(centerX, centerY);
                var size = new Size(oval.Width / 2, oval.Height / 2);

                var grays = new List<byte>();

                // 椭圆参数
                double angle = 0;

                var points = new List<Point>();

                var a = size.Width;
                var bb = size.Height;
                var x0 = centerX;
                var y0 = centerY;
                for (double thetaDegrees = 0; thetaDegrees < 360; thetaDegrees += 0.1)
                {
                    var thetaRadians = thetaDegrees * (Math.PI / 180); // 转换为弧度

                    var x = a * Math.Cos(thetaRadians) * Math.Cos(0)
                        - bb * Math.Sin(thetaRadians) * Math.Sin(0) + x0;
                    var y = a * Math.Cos(thetaRadians) * Math.Sin(0)
                            + bb * Math.Sin(thetaRadians) * Math.Cos(0) + y0;

                    var point = new Point((int)Math.Round(x), (int)Math.Round(y));
                    points.Add(point);
                }

                foreach (var p in points)
                {
                    if (p.Y >= srcMat.Height || p.X >= srcMat.Width || p.Y < 0 || p.X < 0)
                    {
                        grays.Add(0);
                    }
                    else
                    {
                        var intencity = srcMat.Get<Vec3b>(p.Y, p.X);
                        var b = intencity[0];
                        var g = intencity[1];
                        var r = intencity[2];
                        var gray = RgbToGammaCorrectedGray(r, g, b);
                        grays.Add(gray);
                    }
                }

                var gp =
                           grays.GroupBy(b => b)
                               .Select(group => new { Value = group.Key, Count = group.Count() }) // 使用匿名类型来保存键和计数
                               .OrderBy(grp => grp.Value) // 按 Count 属性降序排序
                               .ToDictionary(grp => grp.Value, grp => grp.Count); // 将结果转成字典
                var min = 0f;
                var max = 0f;
                var average = 0f;
                if (gp.Keys.ToList().Any())
                {
                    min = gp.Keys.ToList()[0];
                    max = gp.Keys.ToList()[0];
                    if (gp.Keys.ToList().Count > 1)
                    {
                        min = gp.Keys.ToList()[0];
                        max = gp.Keys.ToList()[gp.Keys.ToList().Count - 1];
                    }
                    average = (float)grays.Average(b => (double)b);
                }

                var result = new GrayResult(min, max, average);
                result.CacUnif();

                // 画椭圆
                //Cv2.Ellipse(srcMat, center, size, angle, 0, 360, Scalar.Green, 2);

                points.Clear();
                grays.Clear();
                gp.Clear();

                return result;
            }
            catch (Exception)
            {
                var result0 = new GrayResult(0, 0, 0);
                result0.CacUnif();

                return result0;
            }
        }

        public static GrayResult LineGray(Mat srcMat, string lineObject)
        {
            try
            {
                var line = JsonConvert.DeserializeObject<LineContour>(lineObject);

                var lineIterator = new LineIterator(srcMat, new Point(line.Start.X, line.Start.Y), new Point(line.End.X, line.End.Y));
                var grays = (from t in lineIterator select t.GetValue<Vec3b>() into vec3B let b = vec3B[0] let g = vec3B[1] let r = vec3B[2] select RgbToGammaCorrectedGray(r, g, b)).ToList();

                var gp =
                   grays.GroupBy(b => b)
                       .Select(group => new { Value = group.Key, Count = group.Count() }) // 使用匿名类型来保存键和计数
                       .OrderBy(grp => grp.Value) // 按 Count 属性降序排序
                       .ToDictionary(grp => grp.Value, grp => grp.Count); // 将结果转成字典


                var min = 0f;
                var max = 0f;
                var average = 0f;
                if (gp.Keys.ToList().Any())
                {
                    min = gp.Keys.ToList()[0];
                    max = gp.Keys.ToList()[0];
                    if (gp.Keys.ToList().Count > 1)
                    {
                        min = gp.Keys.ToList()[0];
                        max = gp.Keys.ToList()[gp.Keys.ToList().Count - 1];
                    }
                    average = (float)grays.Average(b => (double)b);
                }

                var result = new GrayResult(min, max, average);
                result.CacUnif();

                // 画线
                //Cv2.Line(srcMat, new Point(line.Start.X, line.Start.Y), new Point(line.End.X, line.End.Y), Scalar.Green, 2);

                grays.Clear();
                gp.Clear();

                return result;
            }
            catch (Exception)
            {
                var result0 = new GrayResult(0, 0, 0);
                result0.CacUnif();

                return result0;
            }
        }

        public static void OvalsGray(Mat srcMat, string[] ovalObjects, out GrayResult[] grayResults, out double unifAll, out double averageAll)
        {
            grayResults = new GrayResult[ovalObjects.Length];

            try
            {
                for (var i = 0; i < ovalObjects.Length; i++)
                {
                    var obj = ovalObjects[i];
                    var oval = JsonConvert.DeserializeObject<OvalContour>(obj);

                    var centerX = (int)(oval.Left + oval.Width / 2);
                    var centerY = (int)(oval.Top + oval.Height / 2);

                    var center = new Point(centerX, centerY);
                    var size = new Size(oval.Width / 2, oval.Height / 2);

                    // 创建mask和绘制椭圆
                    var mask = new Mat(srcMat.Size(), MatType.CV_8UC1, Scalar.All(0));
                    Cv2.Ellipse(mask, center, size, 0, 0, 360, Scalar.All(255), -1);

                    // 确定边界矩形
                    var boundingRect = Cv2.BoundingRect(mask);
                    var roiMask = new Mat(mask, boundingRect); // 感兴趣区域的掩膜

                    // 应用mask
                    var dst = new Mat(srcMat, boundingRect);
                    var roiDst = new Mat(); // 创建ROI的目标图像
                    dst.CopyTo(roiDst, roiMask);

                    // 遍历ROI，提取非零像素
                    var grays = new List<byte>();
                    // 遍历边界矩形内的像素
                    for (var y = 0; y < roiDst.Rows; y++)
                    {
                        for (var x = 0; x < roiDst.Cols; x++)
                        {
                            if (roiMask.At<byte>(y, x) > 0) // 确保仅处理椭圆内的像素
                            {
                                var pixel = roiDst.At<Vec3b>(y, x);
                                var gray = RgbToGammaCorrectedGray(pixel.Item2, pixel.Item1, pixel.Item0);
                                grays.Add(gray);
                            }
                        }
                    }

                    var gp =
                          grays.GroupBy(b => b)
                              .Select(group => new { Value = group.Key, Count = group.Count() }) // 使用匿名类型来保存键和计数
                              .OrderBy(grp => grp.Value) // 按 Count 属性降序排序
                              .ToDictionary(grp => grp.Value, grp => grp.Count); // 将结果转成字典
                    var min = 0f;
                    var max = 0f;
                    var average = 0f;
                    if (gp.Keys.ToList().Any())
                    {
                        min = gp.Keys.ToList()[0];
                        max = gp.Keys.ToList()[0];
                        if (gp.Keys.ToList().Count > 1)
                        {
                            min = gp.Keys.ToList()[0];
                            max = gp.Keys.ToList()[gp.Keys.ToList().Count - 1];
                        }
                        average = (float)grays.Average(b => (double)b);
                    }

                    var result = new GrayResult(min, max, average);
                    result.CacUnif();

                    grayResults[i] = result;
                }

                var minInAll = grayResults.Min(f => f.Min);
                var maxInAll = grayResults.Max(f => f.Max);
                averageAll = Math.Round(grayResults.Average(f => f.Average), 0, MidpointRounding.AwayFromZero);
                unifAll = Math.Round(minInAll / maxInAll * 100, 2, MidpointRounding.AwayFromZero);
                if (double.IsNaN(unifAll))
                    unifAll = 0;
            }
            catch (Exception)
            {
                for (var i = 0; i < grayResults.Length; i++)
                {
                    grayResults[i] = new GrayResult(0, 0, 0);
                    grayResults[i].CacUnif();
                }

                unifAll = 0;
                averageAll = 0;
            }
        }

        public static void LinesGray(Mat srcMat, string[] lineObjects, out GrayResult[] grayResults, out double unifAll, out double averageAll)
        {
            grayResults = new GrayResult[lineObjects.Length];

            try
            {
                for (var i = 0; i < lineObjects.Length; i++)
                {
                    var lineObject = lineObjects[i];

                    var line = JsonConvert.DeserializeObject<LineContour>(lineObject);

                    var lineIterator = new LineIterator(srcMat, new Point(line.Start.X, line.Start.Y), new Point(line.End.X, line.End.Y));
                    var grays = (from t in lineIterator select t.GetValue<Vec3b>() into vec3B let b = vec3B[0] let g = vec3B[1] let r = vec3B[2] select RgbToGammaCorrectedGray(r, g, b)).ToList();

                    var gp =
                       grays.GroupBy(b => b)
                           .Select(group => new { Value = group.Key, Count = group.Count() }) // 使用匿名类型来保存键和计数
                           .OrderBy(grp => grp.Value) // 按 Count 属性降序排序
                           .ToDictionary(grp => grp.Value, grp => grp.Count); // 将结果转成字典

                    var min = 0f;
                    var max = 0f;
                    var average = 0f;
                    if (gp.Keys.ToList().Any())
                    {
                        min = gp.Keys.ToList()[0];
                        max = gp.Keys.ToList()[0];
                        if (gp.Keys.ToList().Count > 1)
                        {
                            min = gp.Keys.ToList()[0];
                            max = gp.Keys.ToList()[gp.Keys.ToList().Count - 1];
                        }
                        average = (float)grays.Average(b => (double)b);
                    }

                    var result = new GrayResult(min, max, average);
                    result.CacUnif();
                    grayResults[i] = result;
                }

                var minInAll = grayResults.Min(f => f.Min);
                var maxInAll = grayResults.Max(f => f.Max);
                averageAll = Math.Round(grayResults.Average(f => f.Average), 0, MidpointRounding.AwayFromZero);

                //unifAll = Math.Round((1f - (maxInAll - minInAll) / (maxInAll + minInAll)) * 100, 2, MidpointRounding.AwayFromZero);
                unifAll = Math.Round(minInAll / maxInAll * 100, 2, MidpointRounding.AwayFromZero);
                if (double.IsNaN(unifAll))
                    unifAll = 0;
            }
            catch (Exception)
            {
                for (var i = 0; i < grayResults.Length; i++)
                {
                    grayResults[i] = new GrayResult(0, 0, 0);
                    grayResults[i].CacUnif();
                }

                unifAll = 0;
                averageAll = 0;
            }
        }

        public static bool GetPointIsInOval(OvalContour drawOvalContour, int xp, int yp)
        {
            var shape = drawOvalContour;

            var centerX = (int)(drawOvalContour.Left + drawOvalContour.Width / 2);
            var centerY = (int)(drawOvalContour.Top + drawOvalContour.Height / 2);

            var x0 = centerX;//(shape.Left + shape.Width / 2);
            var y0 = centerY;//(shape.Top + shape.Height / 2);

            double a, b;

            if (shape.Width >= shape.Height)
            {
                a = shape.Width;
                b = shape.Height;
            }
            else
            {
                a = shape.Height;
                b = shape.Width;
            }

            a = a / 2;
            b = b / 2;

            // 判断点是否在椭圆边线上
            var result = Math.Pow((xp - x0) / a, 2) + Math.Pow((yp - y0) / b, 2);
            return Math.Abs(result - 1) < 0.001;
        }

        public static byte RgbToGammaCorrectedGray(byte r, byte g, byte b, double gamma = 2.2)
        {
            // 首先，对RGB颜色值应用Gamma校正
            var rGamma = Math.Pow(r / 255.0, gamma);
            var gGamma = Math.Pow(g / 255.0, gamma);
            var bGamma = Math.Pow(b / 255.0, gamma); // 接着，计算加权灰度值，这里假设Gamma校正后也使用相同的加权系数
            var gray = 0.299 * rGamma + 0.587 * gGamma + 0.114 * bGamma;

            //将得到的值重新映射回 [0,255] 范围内
            var grayValue = gray * 255.0;

            // 返回值前确保在 [0,255] 范围内，并转换为灰度值
            if (grayValue > 255)
            {
                return 255;
            }

            if (grayValue <= 0)
            {
                return 0;
            }

            return (byte)grayValue;
        }

        public static bool AutoRotate(double baseAngle, double maxSize, ref Mat src)
        {
            return false;

            var outImage1 = src.Clone();

            unsafe
            {
                outImage1.ForEachAsVec3b((c, p) =>
                {
                    Vec3b intencity = *c;
                    var row = p[0]; // y
                    var col = p[1]; // x

                    var b = intencity[0];
                    var g = intencity[1];
                    var r = intencity[2];

                    if (r > 50)
                    {
                        outImage1.Set(row, col, new Vec3b(0, 0, 255));
                    }
                });
            }

            //Cv2.ImShow("Red", outImage1);
            //Cv2.WaitKey();
            //Cv2.DestroyAllWindows();

            // 先转换成8CU1
            Cv2.CvtColor(outImage1, outImage1, ColorConversionCodes.BGR2GRAY);
            // 再自适应阈值分割
            Cv2.AdaptiveThreshold(outImage1, outImage1, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 31, 10);
            // 再边缘探测
            var dst = new Mat();
            var absDst = new Mat();
            Cv2.Laplacian(outImage1, dst, MatType.CV_16S, 3, 1, 0, BorderTypes.Default);
            Cv2.ConvertScaleAbs(dst, absDst);

            outImage1.Dispose();
            GC.Collect();

            // 再找出边界
            Point[][] contours2;
            HierarchyIndex[] hierarchy2;
            Cv2.FindContours(absDst, out contours2, out hierarchy2, RetrievalModes.External, ContourApproximationModes.ApproxNone);

            var newRect = new Rect();

            if (contours2.Any())
            {
                var sizes = new List<double>();

                foreach (var temp in contours2)
                {
                    var tempSize = Cv2.ArcLength(temp, true);
                    sizes.Add(tempSize);
                    if (tempSize >= maxSize)
                    {
                        var rect = Cv2.BoundingRect(temp);

                        var offset = 20;

                        if (rect.Width + offset > src.Width || rect.Height + offset > src.Height)
                        {
                            if (src.Height - rect.Height > src.Width - rect.Width)
                            {
                                var newOffset = src.Height - rect.Height;
                                if (rect.Width + newOffset > src.Width || rect.Height + newOffset > src.Height)
                                    offset = 0;
                                else
                                    offset = newOffset;

                            }
                            else
                            {
                                var newOffset = src.Width - rect.Width;
                                if (rect.Width + newOffset > src.Width || rect.Width + newOffset > src.Width)
                                    offset = 0;
                                else
                                    offset = newOffset;
                            }
                        }

                        newRect = new Rect(rect.X - offset / 2, rect.Y - offset / 2, rect.Width + offset, rect.Height + offset);

                        absDst = absDst.Clone(new Rect(newRect.Left, newRect.Top, newRect.Width / 2 + offset, newRect.Height));
                        //absDst.ImWrite(@"D:\Projects\文档\延锋-logo灯\图像采集\后灯\absDst.jpg");
                        break;
                    }
                }

                Console.WriteLine(@"sizes.Max():" + sizes.Max());
                if (sizes.Max() < maxSize)
                {
                    absDst.Dispose();
                    GC.Collect();
                    return false;
                }

                Size size;
                size.Width = 3;
                size.Height = 3;

                Cv2.Threshold(absDst, absDst, 1, 255, ThresholdTypes.BinaryInv);
                // 对图像进行按位非操作，将黑色改成白色，白色改成黑色
                Cv2.BitwiseNot(absDst, absDst);
                Cv2.GaussianBlur(absDst, absDst, new Size(5, 5), 0, 0); // 高斯滤波

                absDst = absDst.MorphologyEx(MorphTypes.Dilate, Cv2.GetStructuringElement(MorphShapes.Rect, size));

                //absDst.ImWrite(@"C:\Users\B1438\Desktop\sda.jpg");
                //Cv2.WaitKey();

                //Cv2.ImShow("形态学膨胀效果", src);
                /*
                *  HoughLinesP:使用概率霍夫变换查找二进制图像中的线段。
                *  参数：
                *      1； image: 输入图像 （只能输入单通道图像）
                *      2； rho:   累加器的距离分辨率(以像素为单位) 生成极坐标时候的像素扫描步长
                *      3； theta: 累加器的角度分辨率(以弧度为单位)生成极坐标时候的角度步长，一般取值CV_PI/180 ==1度
                *      4； threshold: 累加器阈值参数。只有那些足够的行才会返回 投票(>阈值)；设置认为几个像素连载一起才能被看做是直线。
                *      5； minLineLength: 最小线长度，设置最小线段是有几个像素组成。
                *      6；maxLineGap: 同一条线上的点之间连接它们的最大允许间隙。(默认情况下是0）：设置你认为像素之间间隔多少个间隙也能认为是直线
                *      返回结果:
                *      输出线。每条线由一个4元向量(x1, y1, x2，y2)
                */
                var linepiont = Cv2.HoughLinesP(absDst, 1, Cv2.PI / 180, 300, 0, 0);

                if (linepiont.Length == 0)
                    linepiont = Cv2.HoughLinesP(absDst, 1, Cv2.PI / 180, 200, 0, 0);
                if (linepiont.Length == 0)
                    linepiont = Cv2.HoughLinesP(absDst, 1, Cv2.PI / 180, 150, 0, 0);
                if (linepiont.Length == 0)
                    return false;

                src = src.Clone(newRect);

                var lenArr = new double[linepiont.Length];
                for (var i = 0; i < linepiont.Length; i++)
                {
                    var p1 = linepiont[i].P1;
                    var p2 = linepiont[i].P2;
                    lenArr[i] = GetTwoPointDistance(p1, p2);
                }

                var max = lenArr.Max();
                var j = lenArr.ToList().IndexOf(max);//用最长的那条直线来计算旋转角度
                var pp1 = linepiont[j].P1;
                var pp2 = linepiont[j].P2;

                absDst.Dispose();
                GC.Collect();
                //src.Line(pp1, pp2, Scalar.Green, 3);

                var angle = GetAngel(pp1, pp2);
                angle = angle + baseAngle;
                Console.WriteLine(@"Angle=" + angle);
                RotateImage(src, angle);
                return true;
            }

            return false;
        }

        private static double GetTwoPointDistance(Point p1, Point p2)
        {
            var dx = Math.Abs(p1.X - p2.X);
            var dy = Math.Abs(p1.Y - p2.Y);
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private static double GetAngel(Point p1, Point p2)
        {
            double angel;
            var dx = Math.Abs(p1.X - p2.X);
            var dy = Math.Abs(p1.Y - p2.Y);
            var atan = Math.Atan(dy / (double)dx);
            if (p1.Y < p2.Y)
            {
                angel = atan * 180 / Cv2.PI;
            }
            else
            {
                angel = atan * 180 / Cv2.PI;
                angel = 0 - angel;
            }
            return angel;
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

        public static Mat DrawChineseTextOnImage(Mat image, string text, System.Drawing.Point location, Font font, Color color)
        {
            // 将OpenCV的Mat对象转换为Bitmap
            var bitmap = image.ToBitmap();

            // 绘制中文文本
            using (var graphics = Graphics.FromImage(bitmap))
            {
                using (Brush brush = new SolidBrush(color))
                {
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    graphics.DrawString(text, font, brush, location);
                }
            }

            // 将 Bitmap 转回 Mat 对象
            image.Dispose();
            GC.Collect();
            image = BitmapConverter.ToMat(bitmap).Clone();

            // 释放 Bitmap 资源
            bitmap.Dispose();

            return image;
        }

        public class GrayResult
        {
            public double Min;
            public double Max;
            public double Average;
            public double Unif1;
            public double Unif2;
            public double Unif3;
            public double Unif4;

            public GrayResult(float min, float max, float average)
            {
                Min = Math.Round(min, 0, MidpointRounding.AwayFromZero);
                Max = Math.Round(max, 0, MidpointRounding.AwayFromZero);
                Average = Math.Round(average, 0, MidpointRounding.AwayFromZero);
            }

            public void CacUnif()
            {
                Unif1 = Math.Round((1f - (Max - Min) / (Max + Min)) * 100, 2, MidpointRounding.AwayFromZero);
                Unif2 = Math.Round(Min / ((Max + Min) / 2f) * 100, 2, MidpointRounding.AwayFromZero);
                Unif3 = Math.Round(Min / Average * 100, 2, MidpointRounding.AwayFromZero);
                Unif4 = Math.Round(Min / Max * 100, 2, MidpointRounding.AwayFromZero);

                if (double.IsNaN(Unif1))
                    Unif1 = 0;

                if (double.IsNaN(Unif2))
                    Unif2 = 0;

                if (double.IsNaN(Unif3))
                    Unif3 = 0;

                if (double.IsNaN(Unif4))
                    Unif4 = 0;
            }
        }
    }
}
