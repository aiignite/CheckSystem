using OpenCvSharp;
using OpenCvSharp.Extensions;
using Sunny.UI;
using System;
using System.Windows.Forms;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace CheckSystem.VisionDetection.Calibration
{
    public partial class FrmDynamicVisionCali : UIForm
    {
        public FrmDynamicVisionCali()
        {
            InitializeComponent();

            //var imagePath = @"C:\Users\B1438\Pictures\Camera Roll\Image18.bmp";
            //using (var src = Cv2.ImRead(imagePath, ImreadModes.Color))
            //{
            //    var gray = new Mat();
            //    Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            //    Cv2.ImShow("gray", gray);
            //    Cv2.WaitKey();
            //    Cv2.DestroyAllWindows();

            //    Cv2.GaussianBlur(gray, gray, new Size(3, 3), 1, 1);
            //    Cv2.ImShow("GaussianBlur", gray);
            //    Cv2.WaitKey();
            //    Cv2.DestroyAllWindows();

            //    var binary = new Mat();
            //    Cv2.Threshold(gray, binary, 200, 220, ThresholdTypes.Binary);

            //    Cv2.ImShow("binary", binary);
            //    Cv2.WaitKey();
            //    Cv2.DestroyAllWindows();

            //    Point[][] contours;
            //    HierarchyIndex[] hierarchy;
            //    Cv2.FindContours(binary, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            //    using (var contoursImg = new Mat(src.Size(), MatType.CV_8UC3))
            //    {
            //        // 绘制所有轮廓
            //        for (var i = 0; i < contours.Length; i++)
            //            Cv2.DrawContours(contoursImg, contours, i, Scalar.Red, 2, LineTypes.Link8, hierarchy, 0);

            //        Cv2.ImShow("Contours", contoursImg);
            //        Cv2.WaitKey();
            //        Cv2.DestroyAllWindows();

            //        foreach (var contour in contours)
            //        {
            //            double area = Cv2.ContourArea(contour);
            //            if (area > 5) // 设定一个最小面积阈值
            //            {
            //                // 可选：计算轮廓的包围盒
            //                var rect = Cv2.BoundingRect(contour);
            //                Cv2.Rectangle(src, rect, Scalar.Blue, 2);

            //                // 或者计算最小封闭圆的半径和中心
            //                Cv2.MinEnclosingCircle(contour, out Point2f center, out float radius);
            //                Cv2.Circle(src, (int)center.X, (int)center.Y, (int)radius, Scalar.Green, 2);
            //            }
            //        }

            //        Cv2.ImShow("src Circle", src);
            //        Cv2.WaitKey();
            //        Cv2.DestroyAllWindows();
            //    }
            //}
        }

        private Mat _srcMat;

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (_srcMat != null && !_srcMat.Empty())
                        _srcMat.Dispose();

                    _srcMat = new Mat(ofd.FileName, ImreadModes.AnyColor);
                    pictureBox1.Image = _srcMat.ToBitmap();
                }
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (_srcMat == null || _srcMat.Empty())
                return;

            using (var src = _srcMat.Clone())
            {
                InputArray kernelRY = InputArray.Create<int>(new int[3, 3] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 2 } });
                Mat dstY = new Mat();
                Cv2.Filter2D(src, dstY, -1, kernelRY, new Point(-1, -1), 0, 0);
                Cv2.ImShow("SobelY", dstY);
                Cv2.WaitKey();
                Cv2.DestroyAllWindows();

                var gray = new Mat();
                if (dstY.Channels() != 1)
                    Cv2.CvtColor(dstY, gray, ColorConversionCodes.BGR2GRAY);
                else
                    gray = dstY.Clone();
                Cv2.ImShow("gray", gray);
                Cv2.WaitKey();
                Cv2.DestroyAllWindows();

                Cv2.GaussianBlur(gray, gray, new Size(3, 3), 1, 1);
                Cv2.ImShow("GaussianBlur", gray);
                Cv2.WaitKey();
                Cv2.DestroyAllWindows();

                var binary = new Mat();
                Cv2.Threshold(gray, binary, ThresholdTxtMin.IntValue, ThresholdTxtMax.IntValue, ThresholdTypes.Binary);

                Cv2.ImShow("binary", binary);
                Cv2.WaitKey();
                Cv2.DestroyAllWindows();

                Point[][] contours;
                HierarchyIndex[] hierarchy;
                Cv2.FindContours(binary, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

                using (var contoursImg = new Mat(src.Size(), MatType.CV_8UC3))
                {
                    // 绘制所有轮廓
                    for (var i = 0; i < contours.Length; i++)
                        Cv2.DrawContours(contoursImg, contours, i, Scalar.Red, 2, LineTypes.Link8, hierarchy, 0);

                    Cv2.ImShow("Contours", contoursImg);
                    Cv2.WaitKey();
                    Cv2.DestroyAllWindows();

                    foreach (var contour in contours)
                    {
                        double area = Cv2.ContourArea(contour);
                        Console.WriteLine("area = {0}", area);
                        if (area >= ContourAreaTxtMin.IntValue && area <= ContourAreaTxtMax.IntValue) // 设定一个最小面积阈值
                        {
                            // 可选：计算轮廓的包围盒
                            var rect = Cv2.BoundingRect(contour);
                            Cv2.Rectangle(src, rect, Scalar.Blue, 2);

                            // 或者计算最小封闭圆的半径和中心
                            Point2f center;
                            float radius;
                            Cv2.MinEnclosingCircle(contour, out center, out radius);
                            Cv2.Circle(src, (int)center.X, (int)center.Y, (int)radius, Scalar.Green, 2);
                        }
                    }

                    Cv2.ImShow("src Circle", src);
                    Cv2.WaitKey();
                    Cv2.DestroyAllWindows();

                    pictureBox2.Image = src.ToBitmap();
                }
            }
        }
    }
}
