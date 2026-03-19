using CommonUtility;
using Go;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using OpenCvSharp;
using Sunny.UI;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageProcessing = CommonUtility.ImageProcessing;
using Mat = OpenCvSharp.Mat;

namespace CheckSystem.OpenCvSharp
{
    public partial class FrmTestPaint : UIForm
    {
        generator _timeAction;

        private OvalContour _drawOvalContour = new OvalContour(82, 112, 426, 426);
        private RectangleContour _drawRectangleContour;

        private RectangleContour _grayContour = new RectangleContour(481, 444, 14, 14);

        public FrmTestPaint()
        {
            InitializeComponent();
            Load += FrmTestPaint_Load;

            MainImageViewer.ToolsShown = ViewerTools.All;
            MainImageViewer.ZoomToFit = true;
            MainImageViewer.ShowToolbar = true;
            MainImageViewer.ShowScrollbars = true;
            MainImageViewer.ShowImageInfo = true;
            MainImageViewer.SizeChanged += MainImageViewer_SizeChanged;
            MainImageViewer.RoiChanged += MainImageViewer_RoiChanged;
        }

        void MainImageViewer_RoiChanged(object sender, ContoursChangedEventArgs e)
        {
            //if (e.NewItems.Any() && e.NewItems[0].Shape is OvalContour)
            //{
            //    var shape = (OvalContour)e.NewItems[0].Shape;

            //    var centerX = (shape.Left + shape.Width / 2);
            //    var centerY = (shape.Top + shape.Height / 2);

            //    double a, b;

            //    if (shape.Width >= shape.Height)
            //    {
            //        a = shape.Width;
            //        b = shape.Height;
            //    }
            //    else
            //    {
            //        a = shape.Height;
            //        b = shape.Width;
            //    }

            //    //MainImageViewer.Roi.Clear();
            //    //MainImageViewer.Roi.Add(shape);
            //    //MainImageViewer.Roi.Add(new RectangleContour(shape.Left, shape.Top, shape.Width, shape.Height));

            //    Console.WriteLine(@"椭圆的中心点=({0},{1}),长轴={2},短轴={3}", centerX, centerY, a, b);
            //}
        }

        void FrmTestPaint_Load(object sender, EventArgs e)
        {
            _timeAction = generator.tgo(FormSelection.MainStrand, TimeAction);
        }

        private async Task TimeAction()
        {
            while (true)
            {
                await generator.sleep(1);
                textBox_Action.Text = DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff");
            }
        }

        private void MainImageViewer_SizeChanged(object sender, EventArgs e)
        {
            MainImageViewer.ToolsShown = ViewerTools.All;

            MainImageViewer.ActiveTool = ViewerTools.Pan;
            MainImageViewer.ZoomToFit = true;
            MainImageViewer.ShowToolbar = true;
            MainImageViewer.ShowScrollbars = true;
            MainImageViewer.ShowImageInfo = true;
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var st = new Stopwatch();
                        st.Start();

                        {
                            //var matImg = new Mat(700, 700, MatType.CV_8UC3);

                            //for (var i = 0; i < matImg.Cols; i++)
                            //{
                            //    for (int j = 0; j < matImg.Rows; j++)
                            //    {
                            //        var color = new Vec3b(0, 0, 255);
                            //        matImg.Set(i, j, color);

                            //        var centerX = (int)(_drawOvalContour.Left + _drawOvalContour.Width) / 2;
                            //        var centerY = (int)(_drawOvalContour.Top + _drawOvalContour.Height) / 2;
                            //        if (i == centerX && j == centerY)
                            //        {
                            //            var color = new Vec3b(0, 0, 255);
                            //            matImg.Set(i, j, color);
                            //        }
                            //    }
                            //}

                            //var visionImg = MatToVisionImage(matImg);
                            //Algorithms.Copy(visionImg, MainImageViewer.Image);
                            //visionImg.Dispose();
                        }

                        {
                            var matImage = new Mat(ofd.FileName, ImreadModes.AnyColor);
                            Scalar color = Scalar.Green;

                            var centerX = (int)(_drawOvalContour.Left + _drawOvalContour.Width / 2);
                            var centerY = (int)(_drawOvalContour.Top + _drawOvalContour.Height / 2);

                            ////matImage.Set(centerY, centerX, new Vec3b(0, 255, 0));
                            //Cv2.Rectangle(matImage, new Point(centerX, centerY), new Point(centerX, centerY), color, 2);

                            //// 椭圆参数
                            //Point center = new Point(centerX, centerY);
                            //Size axes = new Size(_drawOvalContour.Width / 2, _drawOvalContour.Height / 2);
                            //double angle = 0;


                            //// 画椭圆
                            //Cv2.Ellipse(matImage, center, axes, angle, 0, 360, color, 2);

                            //for (var i = 0; i < matImage.Cols; i++)
                            //{
                            //    for (int j = 0; j < matImage.Rows; j++)
                            //    {
                            //        //var color = new Vec3b(0, 0, 255);
                            //        //matImage.Set(i, j, color);

                            //        if (IsInOval(centerX, centerY, i, j))
                            //        {
                            //            matImage.Set(j, i, new Vec3b(0, 255, 0));
                            //        }
                            //    }
                            //}

                            var visionImg = MatToVisionImage(matImage);

                            #region 图像预处理

                            var cImg = ImageProcessing.ColorPlaneExtraction(visionImg,
                                ImageProcessing.ColorPlaneExtractionType.Red);
                            if (cImg != null)
                            {
                                Algorithms.Copy(cImg, visionImg);
                                cImg.Dispose();
                            }

                            using (var srcImg = new VisionImage(visionImg.Type))
                            {
                                Algorithms.Copy(visionImg, srcImg);

                                var typeToLookup =
                                    EnumOperater.GetEnumByValue<ImageProcessing.LookupTableType>(
                                        ImageProcessing.LookupTableType.Exponential);
                                var lookupImg = ImageProcessing.LookupTable(srcImg, typeToLookup);
                                Algorithms.Copy(lookupImg, srcImg);
                                lookupImg.Dispose();
                                Algorithms.Copy(srcImg, visionImg);

                                Algorithms.AutoThreshold(srcImg, srcImg, 2, ThresholdMethod.Metric);
                                //list = ImageProcessing.FindCircle(srcImg, new NationalInstruments.Vision.Range(5, 100)).OrderBy(f => f.Center.X).ToList();
                                srcImg.Dispose();

                                //var rectOnImage = new Rect((int)_grayContour.Left, (int)_grayContour.Top, (int)_grayContour.Width, (int)_grayContour.Height);
                                //var ddd = visionImg.ImageToArray();
                                //var mat = new Mat(visionImg.Height, visionImg.Width, MatType.CV_8UC1);
                                //mat.SetArray(0, 0, ddd.U8);
                                //var roiMat = new Mat(mat, rectOnImage);
                                //var gray = Cv2.Mean(roiMat);
                                //var grayPer = Math.Round(gray.Val0 / 255, 4);
                            }

                            #endregion

                            #region 图像不预处理，gamma矫正后的灰度值

                            var mat = CommonUtility.HikSdk.MyCamera.VisionImageToMat(visionImg);
                            var rectOnImage = new Rect((int)_grayContour.Left, (int)_grayContour.Top, (int)_grayContour.Width, (int)_grayContour.Height);
                            // 在掩膜上绘制白色圆形
                            var mask = new Mat(mat.Size(), MatType.CV_8UC1, Scalar.Black);
                            Cv2.Rectangle(mask, rectOnImage, Scalar.White, -1);
                            var meanVal = Cv2.Mean(mat, mask);
                            var gray = GetLxByRGB(meanVal.Val2, meanVal.Val1, meanVal.Val0);
                            var grayPer = Math.Round(gray / 255, 4);
                            #endregion


                            Algorithms.Copy(visionImg, MainImageViewer.Image);
                            visionImg.Dispose();
                            MainImageViewer.Roi.Clear();
                            MainImageViewer.Roi.Add(_grayContour);
                        }

                        {
                            //var src = VisionImageToMat(MainImageViewer.Image);
                            ////VisionImageToMat(MainImageViewer.Image);

                            //src = src.Canny(50, 200, 3);
                            //Cv2.CvtColor(src, src, ColorConversionCodes.GRAY2BGR);

                            //var visionImg = MatToVisionImage(src);
                            //Algorithms.Copy(visionImg, MainImageViewer.Image);
                            //visionImg.Dispose();
                        }

                        st.Stop();
                        Console.WriteLine(@"处理耗时：{0}ms", st.ElapsedMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private double GetLxByRGB(double r, double g, double b)
        {
            var var_r = r / 255;
            var var_g = g / 255;
            var var_b = b / 255;

            if (var_r > 0.04045)
                var_r = Math.Pow(((var_r + 0.055) / 1.055), 2.4);
            else
                var_r /= 12.92;

            if (var_g > 0.04045)
                var_g = Math.Pow(((var_g + 0.055) / 1.055), 2.4);
            else
                var_g /= 12.92;

            if (var_b > 0.04045)
                var_b = Math.Pow(((var_b + 0.055) / 1.055), 2.4);
            else
                var_b /= 12.92;

            var_r *= 100;
            var_g *= 100;
            var_b *= 100;

            var x = var_r * 0.4124 + var_g * 0.3576 + var_b * 0.1805;
            var y = var_r * 0.2126 + var_g * 0.7152 + var_b * 0.0722;
            var z = var_r * 0.0193 + var_g * 0.1192 + var_b * 0.9505;

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

        public Mat VisionImageToMat(VisionImage visionImg)
        {
            var pixelValue = visionImg.ImageToArray();

            var st = new Stopwatch();
            st.Start();

            MatType matType = new MatType();
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
        }

        public VisionImage MatToVisionImage(Mat matImage)
        {
            var channel = matImage.Channels();

            var bytes = new byte[matImage.Total() * matImage.ElemSize()];// 创建与图像大小相匹配的字节数组

            Marshal.Copy(matImage.Data, bytes, 0, bytes.Length);// 将Mat对象的数据复制到字节数组中

            var visionImg = ImageProcessing.ConvertBytesToVisionImg(bytes, matImage.Cols, matImage.Rows, channel > 1);

            return visionImg;

        }

        private void toolStripButtonDrawCircle_Click(object sender, EventArgs e)
        {
            _drawOvalContour = new OvalContour(82, 112, 426, 426);
            _drawRectangleContour = new RectangleContour(82, 112, 426, 426);

            MainImageViewer.Roi.Add(_drawOvalContour);

            MainImageViewer.Roi.Add(_drawRectangleContour);
        }

        private bool IsInOval(int centerX, int centerY, int xp, int yp)
        {
            var shape = _drawOvalContour;

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
            double result = Math.Pow((xp - x0) / a, 2) + Math.Pow((yp - y0) / b, 2);
            if (Math.Abs(result - 1) < 0.01)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
