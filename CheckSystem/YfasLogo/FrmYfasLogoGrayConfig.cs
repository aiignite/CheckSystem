using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility;
using Go;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using Newtonsoft.Json;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Sunny.UI;
using Point = System.Drawing.Point;
using Size = OpenCvSharp.Size;

namespace CheckSystem.YfasLogo
{
    public partial class FrmYfasLogoGrayConfig : UIForm
    {
        private generator _timeAction;

        private readonly List<YfasLogoSqlHelper.LogoConfigModel> _configModels = new List<YfasLogoSqlHelper.LogoConfigModel>();

        private Mat SrcMat { get; set; }

        public FrmYfasLogoGrayConfig()
        {
            InitializeComponent();
            SetMainViewerStyle();
            //MainImageViewer.SizeChanged += MainImageViewer_SizeChanged;
            Load += FrmYfasLogoGrayConfig_Load;
            Closed += FrmYfasLogoGrayConfig_Closed;
        }

        private void FrmYfasLogoGrayConfig_Closed(object sender, EventArgs e)
        {
            if (_timeAction != null)
            {
                _timeAction.stop();
            }
        }

        private void FrmYfasLogoGrayConfig_Load(object sender, EventArgs e)
        {
            _configModels.Clear();
            _configModels.AddRange(YfasLogoSqlHelper.GetConfigModels());
            _timeAction = generator.tgo(FormSelection.MainStrand, TimeAction);
        }

        private void MainImageViewer_SizeChanged(object sender, EventArgs e)
        {
            SetMainViewerStyle();
        }

        private void SetMainViewerStyle()
        {
            //MainImageViewer.ToolsShown = ViewerTools.Selection | ViewerTools.ZoomIn | ViewerTools.ZoomOut | ViewerTools.Pan;

            //MainImageViewer.ActiveTool = ViewerTools.Pan;
            //MainImageViewer.ZoomToFit = true;
            //MainImageViewer.ShowToolbar = true;
            //MainImageViewer.ShowScrollbars = true;
            //MainImageViewer.ShowImageInfo = true;
        }

        private async Task TimeAction()
        {
            while (true)
            {
                await generator.sleep(1);
                textBox_Action.Text = DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff");
            }
        }

        private void tsbtnAddPic_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var st = new Stopwatch();
                        st.Start();

                        SrcMat = new Mat(ofd.FileName, ImreadModes.AnyColor);
                        //Cv2.CvtColor(SrcMat, SrcMat, ColorConversionCodes.RGB2GRAY);

                        pictureBox1.Image = SrcMat.ToBitmap();

                        //MainImageViewer.Roi.Clear();
                        //var visionImg = MatToVisionImage(matImage);
                        //Algorithms.Copy(visionImg, MainImageViewer.Image);
                        //visionImg.Dispose();

                        //var config = YfasLogoSqlHelper.GetConfigModels();

                        //foreach (var c in config)
                        //{
                        //    if (c.Type == "Oval")
                        //    {
                        //        var shape = JsonConvert.DeserializeObject<OvalContour>(c.Object);
                        //        MainImageViewer.Roi.Add(shape);
                        //    }
                        //    else if (c.Type == "Line")
                        //    {
                        //        var shape = JsonConvert.DeserializeObject<LineContour>(c.Object);
                        //        MainImageViewer.Roi.Add(shape);
                        //    }
                        //}

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

        private void tsbtnCalculate_Click(object sender, EventArgs e)
        {
            _pGrays.Clear();

            var st = new Stopwatch();
            st.Start();

            MainWorker();

            st.Stop();
            Console.WriteLine(@"耗时=" + st.ElapsedMilliseconds);

            //var newPGrays = _pGrays.OrderBy(f => f.Gray).ToList();
            //Console.WriteLine(@"min = {0}", newPGrays[0].Gray);
            //Console.WriteLine(@"max = {0}", newPGrays[newPGrays.Count - 1].Gray);
        }

        private readonly List<PGray> _pGrays = new List<PGray>();

        private unsafe PGray Operation(Vec3b* pixel, int* position)
        {
            // Get or set pixel value.
            //short intencity = *pixel;
            //var row = position[0];
            //var col = position[1];

            Vec3b intencity = *pixel;
            var row = position[0]; // y
            var col = position[1]; // x

            var b = intencity[0];
            var g = intencity[1];
            var r = intencity[2];
            var pixelValue = (byte)(r * 0.299 + g * 0.587 + b * 0.114);

            //_grays.Add(pixelValue);

            var x = col;
            var y = row;

            return new PGray
            {
                Gray = pixelValue,
                P = new Point(x, y),
                V = intencity
            };
        }

        private void MainWorker()
        {
            var ovals = new List<OvalContour>();
            var lines = new List<LineContour>();

            foreach (var c in _configModels)
            {
                if (c.Type == "Oval")
                {
                    var shape = JsonConvert.DeserializeObject<OvalContour>(c.Object);
                    ovals.Add(shape);

                    var oval = shape;

                    var centerX = (int)(oval.Left + oval.Width / 2);
                    var centerY = (int)(oval.Top + oval.Height / 2);

                    var center = new global::OpenCvSharp.Point(centerX, centerY);
                    var size = new Size(oval.Width/2, oval.Height/2);

                    var axes = new Size2f(oval.Width, oval.Height);

                    // 获取椭圆的所有边界点坐标
                    var points = Cv2.Ellipse2Poly(center, size, 0, 0, 360, 1);

                    Mat image = new Mat(SrcMat.Width, SrcMat.Height, SrcMat.Type(), Scalar.White);  // 创建一个空白图像
                    //var points = Cv2.Ellipse2Poly(center, size, 0, 0, 360, 5);

                    
                    foreach (var t in points)
                    {
                        //_pGrays.Add(new PGray
                        //{
                        //    Gray = 100,
                        //    P = new Point(t.X, t.Y),
                        //    V = new Vec3b(255, 0, 255)
                        //});

                        //image.Circle(t, 5, Scalar.Red, 2);

                        //var radius = 5;
                        //Rect roiRectangle = new Rect(t.X - radius, t.Y - radius, radius * 2, radius * 2);
                        //Mat roiC = new Mat(SrcMat, roiRectangle);

                        //////显示图像
                        ////pictureBox1.Image = roiC.ToBitmap();
                        ////Cv2.ImShow("Ellipse", roiC);
                        ////Cv2.WaitKey(0);

                        //Scalar mean, stddev;
                        //Cv2.MeanStdDev(roiC, out mean, out stddev); // 计算整个图像的均值和标准差

                        //var grayValue = Cv2.Mean(roiC);

                        //var b = grayValue.Val0;
                        //var g = grayValue.Val1;
                        //var r = grayValue.Val2;
                        //var pixelValue = (byte)(r * 0.299 + g * 0.587 + b * 0.114);
                        
                        //Console.WriteLine(pixelValue);

                        ////image.Circle(t, radius, grayValue, 2);
                        ////SrcMat.Circle(t, radius, Scalar.Blue, 5);
                        //// Console.WriteLine(grayValue.Val0);
                    }

                    //// 显示图像
                    //pictureBox1.Image = SrcMat.ToBitmap();
                    //Cv2.ImShow("Ellipse", SrcMat);
                    //Cv2.WaitKey(0);

                }
                else if (c.Type == "Line")
                {
                    var shape = JsonConvert.DeserializeObject<LineContour>(c.Object);
                    lines.Add(shape);

                    // 创建一条直线的起点和终点
                    var l = shape;
                    var startPoint = new global::OpenCvSharp.Point(l.Start.X, l.Start.Y);
                    var endPoint = new global::OpenCvSharp.Point(l.End.X, l.End.Y);

                    // 创建一个 LineIterator 对象
                    LineIterator lineIterator = new LineIterator(SrcMat, startPoint, endPoint);

                    foreach (var t in lineIterator)
                    {
                        //var radius = 25;
                        //Rect roiRectangle = new Rect(t.Pos.X - radius, t.Pos.Y - radius, radius * 2, radius * 2);
                        //Mat roiC = new Mat(SrcMat, roiRectangle);

                        //////显示图像
                        ////pictureBox1.Image = roiC.ToBitmap();
                        ////Cv2.ImShow("Ellipse", roiC);
                        ////Cv2.WaitKey(0);

                        //Scalar mean, stddev;
                        //Cv2.MeanStdDev(roiC, out mean, out stddev); // 计算整个图像的均值和标准差

                        //var grayValue = Cv2.Mean(roiC);

                        //var b = grayValue.Val0;
                        //var g = grayValue.Val1;
                        //var r = grayValue.Val2;
                        //var pixelValue = (byte)(r * 0.299 + g * 0.587 + b * 0.114);

                        //Console.WriteLine(pixelValue);

                        ////image.Circle(t, radius, grayValue, 2);
                        //SrcMat.Circle(t.Pos, radius, Scalar.Blue, 5);
                        // Console.WriteLine(grayValue.Val0);

                        //_pGrays.Add(new PGray
                        //{
                        //    Gray = 100,
                        //    P = new Point(t.Pos.X, t.Pos.Y),
                        //    V = new Vec3b(0, 0, 255)
                        //});
                    }

                    //// 显示图像
                    //pictureBox1.Image = SrcMat.ToBitmap();
                    //Cv2.ImShow("Ellipse", SrcMat);
                    //Cv2.WaitKey(0);
                }
            }

            return;

            var matImage = SrcMat.Clone();

            unsafe
            {
                matImage.ForEachAsVec3b((c, p) =>
                {
                    var pGray = Operation(c, p);

                    //if (ovals.Any(oval => GetPointIsInOval(oval, pGray.P.X, pGray.P.Y)))
                    //    _pGrays.Add(pGray);
                    //else if (lines.Any(line => GetPointIsInLine(line, new Point(pGray.P.X, pGray.P.Y), 1)))
                    //    _pGrays.Add(pGray);
                    if (lines.Any(line => GetPointIsInLine(line, new Point(pGray.P.X, pGray.P.Y), 1)))
                        _pGrays.Add(pGray);

                    //foreach (var shape in shapes)
                    //{
                    //    if (shape is OvalContour)
                    //    {
                    //        var ovalContour = shape as OvalContour;

                    //        if (GetPointIsInOval(ovalContour, pGray.P.X, pGray.P.Y))
                    //        {
                    //            _pGrays.Add(pGray);
                    //            break;
                    //        }
                    //    }

                    //    if (shape is LineContour)
                    //    {
                    //        var lineContour = shape as LineContour;
                    //        var pf = new Point(pGray.P.X, pGray.P.Y);

                    //        if (GetPointIsInLine(lineContour, pf, 2))
                    //        {
                    //            _pGrays.Add(pGray);
                    //            break;
                    //        }
                    //    }
                    //}
                });
            }
        }

        public VisionImage MatToVisionImage(Mat matImage)
        {
            var channel = matImage.Channels();

            var bytes = new byte[matImage.Total() * matImage.ElemSize()];// 创建与图像大小相匹配的字节数组

            Marshal.Copy(matImage.Data, bytes, 0, bytes.Length);// 将Mat对象的数据复制到字节数组中

            var visionImg = ImageProcessing.ConvertBytesToVisionImg(bytes, matImage.Cols, matImage.Rows, channel > 1);

            return visionImg;
        }

        public Mat VisionImageToMat(VisionImage visionImg)
        {
            var st = new Stopwatch();
            st.Start();

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

            // 保存图像到文件中，也可以进一步处理
            st.Stop();

            var title = string.Format("VisionImageToMat get {0} image cost = {1}ms", matType, st.ElapsedMilliseconds);
            Console.WriteLine(title);

            //Cv2.ImShow(title, image);

            //image.SaveImage("output.jpg");

            return image;
        }

        private static bool GetPointIsInOval(OvalContour drawOvalContour, int xp, int yp)
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
            return Math.Abs(result - 1) < 0.01;
        }

        public static bool GetPointIsInLine(LineContour drawLineContour, Point pf, double range)
        {
            //range 判断的的误差，不需要误差则赋值0

            //点在线段首尾两端之外则return false

            var p1 = new Point((int)drawLineContour.Start.X, (int)drawLineContour.Start.Y);
            var p2 = new Point((int)drawLineContour.End.X, (int)drawLineContour.End.Y);

            double cross = (p2.X - p1.X) * (pf.X - p1.X) + (p2.Y - p1.Y) * (pf.Y - p1.Y);
            if (cross <= 0)
                return false;
            double d2 = (p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y);
            if (cross >= d2)
                return false;

            var r = cross / d2;
            var px = p1.X + (p2.X - p1.X) * r;
            var py = p1.Y + (p2.Y - p1.Y) * r;

            //判断距离是否小于误差
            return Math.Sqrt((pf.X - px) * (pf.X - px) + (py - pf.Y) * (py - pf.Y)) <= range;
        }

        private void tsbtnAddMat_Click(object sender, EventArgs e)
        {
            Mat mat = new Mat(SrcMat.Width, SrcMat.Height, MatType.CV_8UC3, Scalar.White);

            for (var i = 0; i < _pGrays.Count; i++)
            {
                if (_pGrays[i] != null)
                {
                    var color = _pGrays[i].V;// new Vec3b(0, 0, 255);
                    mat.Set(_pGrays[i].P.Y, _pGrays[i].P.X, color);
                }

            }

            //// 调整图像大小
            //Size newSize = new Size(800, 600);
            //Mat resizedMat = new Mat();
            //Cv2.Resize(mat, resizedMat, newSize);

            //Cv2.ImShow("debug", resizedMat);

            pictureBox1.Image = mat.ToBitmap();
        }

        private class PGray
        {
            public Point P { get; set; }
            public Vec3b V { get; set; }
            public byte Gray { get; set; }
        }
    }
}
