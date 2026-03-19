using CheckSystem.VisionDetection.Vision;
using CommonUtility;
using CommonUtility.HikSdk;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using OpenCvSharp;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using ImageProcessing = CheckSystem.VisionDetection.Vision.ImageProcessing;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace CheckSystem.VisionDetection.Control
{
    public partial class StaticImageViewer : UserControl
    {
        public StaticImageViewer()
        {
            InitializeComponent();
            InitGrayDgv();

            MainImageViewer.ToolsShown = ViewerTools.ZoomIn |
                                         ViewerTools.ZoomOut |
                                         ViewerTools.Pan;
            MainImageViewer.ActiveTool = ViewerTools.Pan;
            MainImageViewer.ZoomToFit = true;
            MainImageViewer.ShowToolbar = true;
            MainImageViewer.ShowScrollbars = true;
            MainImageViewer.ShowImageInfo = true;
            MainImageViewer.SizeChanged += MainImageViewer_SizeChanged;
        }

        private void InitGrayDgv()
        {
            grayDgv.Style = UIStyle.Gray;
            grayDgv.ReadOnly = true;
            grayDgv.RowHeadersVisible = false;
            grayDgv.AllowUserToAddRows = false;
            grayDgv.AllowUserToResizeRows = false;
            grayDgv.MultiSelect = true;
            grayDgv.RowHeadersVisible = false;
            grayDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            grayDgv.AddColumn("Min", "Min");
            grayDgv.AddColumn("Max", "Max");
            grayDgv.AddColumn("Value", "Value");
            grayDgv.AddColumn("Result", "Result");
        }

        private void MainImageViewer_SizeChanged(object sender, EventArgs e)
        {
            MainImageViewer.ToolsShown = ViewerTools.ZoomIn |
                                         ViewerTools.ZoomOut |
                                         ViewerTools.Pan;

            MainImageViewer.ActiveTool = ViewerTools.Pan;
            MainImageViewer.ZoomToFit = true;
            MainImageViewer.ShowToolbar = true;
            MainImageViewer.ShowScrollbars = true;
            MainImageViewer.ShowImageInfo = true;
        }

        public void Reset()
        {
            MainImageViewer.Roi.Clear();
            grayDgv.ClearRows();

            var img = new VisionImage(ImageType.U8);
            Algorithms.Copy(img, MainImageViewer.Image);
            img.Dispose();
        }

        public bool DoWork(VisionConfigVisionFunc func, bool isLeft)
        {
            if (func == null)
                return false;

            var temp = isLeft ? func.VisionFuncDetailL[0] : func.VisionFuncDetailR[0];

            var device =
                VisionCommon.NiImaqd.CameraList.Find(
                    f => f.GigeInfo.chSerialNumber == temp.UserId);
            if (device == null)
                return false;

            #region ROI

            var p1List = new List<RotatedRectangleContour>();
            //var p1LightOnList = new List<RotatedRectangleContour>();

            for (var i = 0; i < temp.Analysis.ShapesGroups[0].Length; i++)
            {
                var shape = temp.Analysis.ShapesGroups[0][i];

                if (shape.Type == "RectangleContour")
                {
                    var rect = ImageProcessing.GetRectByString(shape.Rect);
                    if (double.Parse(shape.Value.Replace("%", "")) >= 25)
                    {
                        //p1LightOnList.Add(
                        //    new RotatedRectangleContour(
                        //        new PointContour(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2), rect.Width,
                        //        rect.Height, 0));
                    }

                    p1List.Add(
                        new RotatedRectangleContour(
                            new PointContour(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2), rect.Width,
                            rect.Height, 0));
                }
                else if (shape.Type == "RotatedRectangleContour")
                {
                    if (double.Parse(shape.Value.Replace("%", "")) >= 25)
                    {
                        //p1LightOnList.Add(ImageProcessing.GetRotatedRecvByString(shape.Rect));
                    }

                    p1List.Add(ImageProcessing.GetRotatedRecvByString(shape.Rect));
                }
            }

            var toSearchRoi = new List<RotatedRectangleContour>();
            toSearchRoi.AddRange(p1List);

            var listRects = toSearchRoi.Select(shape => new Rect((int)(shape.Center.X - shape.Width / 2), (int)(shape.Center.Y - shape.Height / 2), (int)shape.Width, (int)shape.Height)).ToList();
            var captureRoi = device.GetOuterMaxRect(listRects);

            #endregion

            device.OpenCamera();
            var allOther =
                VisionCommon.NiImaqd.CameraList.FindAll(
                    f => f.GigeInfo.chSerialNumber != device.GigeInfo.chSerialNumber);
            //foreach (var other in allOther)
            //    other.CloseCamera();

            int[] ngCount = { 0 };
            device.SetExposureTime(int.Parse(temp.Shutter));
            Thread.Sleep(200);
            var captureCount = 5;
            device.Capture(count: (uint)captureCount, roiRect: captureRoi);

            var isOk = false;
            for (var i = 0; i < captureCount; i++)
            {
                int row;
                int col;
                using (var toCheckMat = device.GetImageFromBuff(i, out row, out col).Clone())
                {
                    isOk = InvokeVisionAnalysis(toCheckMat, temp, toSearchRoi, device.LastCaptureRoiOffsetX, device.LastCaptureRoiOffsetY, p1List);
                    toCheckMat.Dispose();
                }

                if (isOk)
                    break;
            }
            device.ClearBuffer();
            return isOk;

            #region 旧方法备份，只拍一张

            //var visionImg = MyCamera.MatToVisionImage(device.MatBuffer[0, 0]);
            //Algorithms.Copy(visionImg, MainImageViewer.Image);
            //visionImg.Dispose();
            //device.ClearBuffer();
            ////return false;

            //var cImg = ImageProcessing.ColorPlaneExtraction(MainImageViewer.Image,
            //    ImageProcessing.ColorPlaneExtractionType.Red);
            //if (cImg != null)
            //{
            //    Algorithms.Copy(cImg, MainImageViewer.Image);
            //    cImg.Dispose();
            //}

            //List<CircleReport> list;
            //using (var srcImg = new VisionImage(MainImageViewer.Image.Type))
            //{
            //    Algorithms.Copy(MainImageViewer.Image, srcImg);

            //    var typeToLookup =
            //        EnumOperater.GetEnumByValue<ImageProcessing.LookupTableType>(
            //            temp.Analysis.LookupTable);
            //    var lookupImg = ImageProcessing.LookupTable(srcImg, typeToLookup);
            //    Algorithms.Copy(lookupImg, srcImg);
            //    lookupImg.Dispose();
            //    Algorithms.Copy(srcImg, MainImageViewer.Image);

            //    Algorithms.AutoThreshold(srcImg, srcImg, 2, ThresholdMethod.Metric);
            //    //list = ImageProcessing.FindCircle(srcImg, new NationalInstruments.Vision.Range(5, 100)).OrderBy(f => f.Center.X).ToList();
            //    srcImg.Dispose();
            //}

            //var isCali = true;

            //var checkAction = new Action<bool>(pIsCali =>
            //{
            //    var toDrawRects = new List<Rect>();
            //    var grayPerList = new List<double>();

            //    foreach (var t in toSearchRoi)
            //    {
            //        grayPerList.Add(-9999);
            //        var rect = new Rect((int)(t.Center.X - t.Width / 2) - device.LastCaputureRoiOffsetX, (int)(t.Center.Y - t.Height / 2) - device.LastCaputureRoiOffsetY, (int)t.Width, (int)t.Height);
            //        toDrawRects.Add(rect);
            //    }

            //    // 获取 VisionImage 的像素数据
            //    var pixelData = new byte[MainImageViewer.Image.Width * MainImageViewer.Image.Height]; // 针对灰度图像，每个像素1字节
            //    var ddd = MainImageViewer.Image.ImageToArray();
            //    // 创建 OpenCvSharp 的 Mat 对象
            //    var mat = new Mat(MainImageViewer.Image.Height, MainImageViewer.Image.Width, MatType.CV_8UC1);
            //    mat.SetArray(0, 0, ddd.U8);

            //    //var items = new ConcurrentBag<RotatedRectangleContour>(toSearchRoi);

            //    Parallel.ForEach(toSearchRoi, (shape, state, index) =>
            //    {
            //        var newX = (int)(shape.Center.X - shape.Width / 2) - device.LastCaputureRoiOffsetX;
            //        newX = newX < 0 ? 0 : newX;
            //        newX = newX > mat.Width ? mat.Width : newX;

            //        var newY = (int)(shape.Center.Y - shape.Height / 2) - device.LastCaputureRoiOffsetY;
            //        newY = newY < 0 ? 0 : newY;
            //        newY = newY > mat.Height ? mat.Height : newY;

            //        var newWidth = (int)shape.Width;
            //        newWidth = newX + newWidth > mat.Width ? mat.Width - newX : newWidth;

            //        var newHeight = (int)shape.Height;
            //        newHeight = newY + newHeight > mat.Height ? mat.Height - newY : newHeight;

            //        var rectOnImage = new Rect(newX, newY, newWidth, newHeight);

            //        if (pIsCali)
            //        {
            //            int centerX;
            //            int centerY;
            //            int offsetX;
            //            int offsetY;
            //            var tMat = new Mat(mat, rectOnImage);

            //            try
            //            {
            //                double minVal;
            //                double maxVal;
            //                Point minLocl;
            //                Point maxLoc;

            //                Cv2.MinMaxLoc(tMat, out minVal, out maxVal, out minLocl, out maxLoc);

            //                // 找到亮点的最中心处
            //                var binary = new Mat();
            //                Cv2.Threshold(tMat, binary, maxVal - 1, 255, ThresholdTypes.Binary);
            //                var moments = Cv2.Moments(binary, true);
            //                centerX = (int)(moments.M10 / moments.M00);
            //                centerY = (int)(moments.M01 / moments.M00);

            //                // 调整中心点坐标为原图坐标
            //                centerX += rectOnImage.X;
            //                centerY += rectOnImage.Y;
            //                offsetX = Math.Abs(centerX - rectOnImage.X);
            //                offsetY = Math.Abs(centerY - rectOnImage.Y);
            //            }
            //            catch (Exception)
            //            {
            //                centerX = 0;
            //                centerY = 0;
            //                centerX += rectOnImage.X;
            //                centerY += rectOnImage.Y;
            //                offsetX = Math.Abs(centerX - rectOnImage.X);
            //                offsetY = Math.Abs(centerY - rectOnImage.Y);
            //            }
            //            finally
            //            {
            //                tMat.Dispose();
            //            }

            //            if (offsetX > rectOnImage.Width || offsetY > rectOnImage.Height)
            //            {
            //                toDrawRects[(int)index] = rectOnImage;

            //                var roiMat = new Mat(mat, MyCamera.GetRectInMat(mat.Width, mat.Height, rectOnImage));
            //                var gray = Cv2.Mean(roiMat);
            //                var grayPerMat =
            //                    Math.Round(gray.Val0 / 255, 4);
            //                roiMat.Dispose();
            //                grayPerList[(int)index] = grayPerMat;
            //            }
            //            else
            //            {
            //                var newRect = new Rect(new Point(centerX - rectOnImage.Width / 2, centerY - rectOnImage.Height / 2), new Size(rectOnImage.Width, rectOnImage.Height));
            //                toDrawRects[(int)index] = newRect;

            //                var roiMat = new Mat(mat, MyCamera.GetRectInMat(mat.Width, mat.Height, newRect));
            //                var gray = Cv2.Mean(roiMat);
            //                var grayPerMat =
            //                    Math.Round(gray.Val0 / 255, 4);
            //                roiMat.Dispose();
            //                grayPerList[(int)index] = grayPerMat;
            //            }
            //        }
            //        else
            //        {
            //            var roiMat = new Mat(mat, MyCamera.GetRectInMat(mat.Width, mat.Height, rectOnImage));
            //            var gray = Cv2.Mean(roiMat);
            //            var grayPerMat =
            //                Math.Round(gray.Val0 / 255, 4);
            //            roiMat.Dispose();
            //            grayPerList[(int)index] = grayPerMat;
            //        }
            //    });

            //    mat.Dispose();

            //    //if (pIsCali)
            //    {
            //        MainImageViewer.Roi.Clear();

            //        foreach (var rect in toDrawRects)
            //            MainImageViewer.Roi.Add(new RotatedRectangleContour(
            //                new PointContour(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2), rect.Width,
            //                rect.Height, 0));
            //        ImageProcessing.DrawContourCountInOverlay(MainImageViewer.Image, MainImageViewer.Roi);
            //    }

            //    for (var i = 0; i < grayPerList.Count; i++)
            //    {
            //        var grayPer = grayPerList[i];
            //        var grayMinPer =
            //            temp.Analysis.ShapesGroups[0][i].Min;
            //        var grayMaxPer =
            //            temp.Analysis.ShapesGroups[0][i].Max;
            //        grayDgv.AddRow(grayMinPer, grayMaxPer, grayPer * 100 + "%");

            //        if (grayPer * 100 >= double.Parse(grayMinPer.TrimEnd('%')) &&
            //            grayPer * 100 <= double.Parse(grayMaxPer.TrimEnd('%')))
            //        {
            //            grayDgv.Rows[grayDgv.RowCount - 1].Cells[3].Value = "OK";
            //        }
            //        else
            //        {
            //            grayDgv.Rows[grayDgv.RowCount - 1].Cells[3].Value = "NG";
            //            grayDgv.Rows[grayDgv.RowCount - 1].DefaultCellStyle.BackColor =
            //                Color.Red;
            //            MainImageViewer.Roi[i].Color = Rgb32Value.RedColor;
            //            ngCount[0]++;
            //        }
            //    }

            //    grayDgv.ClearSelection();
            //});

            //Invoke(checkAction, isCali);
            ////checkAction(isCali);
            //if (isCali && ngCount[0] > 0)
            //{
            //    isCali = false;
            //    Invoke(new Action(() =>
            //    {
            //        MainImageViewer.Roi.Clear();
            //        grayDgv.ClearRows();
            //    }));
            //    ngCount[0] = 0;
            //    toSearchRoi.Clear();
            //    toSearchRoi.AddRange(p1List);

            //    Invoke(checkAction, isCali);
            //    //checkAction(isCali);
            //}

            ////foreach (var other in allOther)
            ////    other.OpenCamera();

            //return ngCount[0] <= 0;

            #endregion
        }

        private bool InvokeVisionAnalysis(
            Mat toCheckMat, VisionConfigVisionFuncCamera temp, List<RotatedRectangleContour> toSearchRoi, int LastCaputureRoiOffsetX, int LastCaputureRoiOffsetY, List<RotatedRectangleContour> p1List)
        {
            Invoke(new Action(Reset));

            int[] ngCount = { 0 };

            var visionImg = MyCamera.MatToVisionImage(toCheckMat);
            Algorithms.Copy(visionImg, MainImageViewer.Image);
            visionImg.Dispose();
            //return false;

            var cImg = ImageProcessing.ColorPlaneExtraction(MainImageViewer.Image,
                ImageProcessing.ColorPlaneExtractionType.Red);
            if (cImg != null)
            {
                Algorithms.Copy(cImg, MainImageViewer.Image);
                cImg.Dispose();
            }

            List<CircleReport> list;
            using (var srcImg = new VisionImage(MainImageViewer.Image.Type))
            {
                Algorithms.Copy(MainImageViewer.Image, srcImg);

                var typeToLookup =
                    EnumOperater.GetEnumByValue<ImageProcessing.LookupTableType>(
                        temp.Analysis.LookupTable);
                var lookupImg = ImageProcessing.LookupTable(srcImg, typeToLookup);
                Algorithms.Copy(lookupImg, srcImg);
                lookupImg.Dispose();
                Algorithms.Copy(srcImg, MainImageViewer.Image);

                Algorithms.AutoThreshold(srcImg, srcImg, 2, ThresholdMethod.Metric);
                //list = ImageProcessing.FindCircle(srcImg, new NationalInstruments.Vision.Range(5, 100)).OrderBy(f => f.Center.X).ToList();
                srcImg.Dispose();
            }

            var isCali = false;

            var checkAction = new Action<bool>(pIsCali =>
            {
                var toDrawRects = new List<Rect>();
                var grayPerList = new List<double>();

                foreach (var t in toSearchRoi)
                {
                    grayPerList.Add(-9999);
                    var rect = new Rect((int)(t.Center.X - t.Width / 2) - LastCaputureRoiOffsetX, (int)(t.Center.Y - t.Height / 2) - LastCaputureRoiOffsetY, (int)t.Width, (int)t.Height);
                    toDrawRects.Add(rect);
                }

                // 获取 VisionImage 的像素数据
                var pixelData = new byte[MainImageViewer.Image.Width * MainImageViewer.Image.Height]; // 针对灰度图像，每个像素1字节
                var ddd = MainImageViewer.Image.ImageToArray();
                // 创建 OpenCvSharp 的 Mat 对象
                var mat = new Mat(MainImageViewer.Image.Height, MainImageViewer.Image.Width, MatType.CV_8UC1);
                mat.SetArray(0, 0, ddd.U8);

                //var items = new ConcurrentBag<RotatedRectangleContour>(toSearchRoi);

                Parallel.ForEach(toSearchRoi, (shape, state, index) =>
                {
                    var newX = (int)(shape.Center.X - shape.Width / 2) - LastCaputureRoiOffsetX;
                    newX = newX < 0 ? 0 : newX;
                    newX = newX > mat.Width ? mat.Width : newX;

                    var newY = (int)(shape.Center.Y - shape.Height / 2) - LastCaputureRoiOffsetY;
                    newY = newY < 0 ? 0 : newY;
                    newY = newY > mat.Height ? mat.Height : newY;

                    var newWidth = (int)shape.Width;
                    newWidth = newX + newWidth > mat.Width ? mat.Width - newX : newWidth;

                    var newHeight = (int)shape.Height;
                    newHeight = newY + newHeight > mat.Height ? mat.Height - newY : newHeight;

                    var rectOnImage = new Rect(newX, newY, newWidth, newHeight);

                    if (pIsCali)
                    {
                        int centerX;
                        int centerY;
                        int offsetX;
                        int offsetY;
                        var tMat = new Mat(mat, rectOnImage);

                        try
                        {
                            double minVal;
                            double maxVal;
                            Point minLocl;
                            Point maxLoc;

                            Cv2.MinMaxLoc(tMat, out minVal, out maxVal, out minLocl, out maxLoc);

                            // 找到亮点的最中心处
                            var binary = new Mat();
                            Cv2.Threshold(tMat, binary, maxVal - 1, 255, ThresholdTypes.Binary);
                            var moments = Cv2.Moments(binary, true);
                            centerX = (int)(moments.M10 / moments.M00);
                            centerY = (int)(moments.M01 / moments.M00);

                            // 调整中心点坐标为原图坐标
                            centerX += rectOnImage.X;
                            centerY += rectOnImage.Y;
                            offsetX = Math.Abs(centerX - rectOnImage.X);
                            offsetY = Math.Abs(centerY - rectOnImage.Y);
                        }
                        catch (Exception)
                        {
                            centerX = 0;
                            centerY = 0;
                            centerX += rectOnImage.X;
                            centerY += rectOnImage.Y;
                            offsetX = Math.Abs(centerX - rectOnImage.X);
                            offsetY = Math.Abs(centerY - rectOnImage.Y);
                        }
                        finally
                        {
                            tMat.Dispose();
                        }

                        if (offsetX > rectOnImage.Width / 2 || offsetY > rectOnImage.Height / 2)
                        {
                            toDrawRects[(int)index] = rectOnImage;

                            var roiMat = new Mat(mat, MyCamera.GetRectInMat(mat.Width, mat.Height, rectOnImage));
                            var gray = Cv2.Mean(roiMat);
                            var grayPerMat =
                                Math.Round(gray.Val0 / 255, 4);
                            roiMat.Dispose();
                            grayPerList[(int)index] = grayPerMat;
                        }
                        else
                        {
                            var newRect = new Rect(new Point(centerX - rectOnImage.Width / 2, centerY - rectOnImage.Height / 2), new Size(rectOnImage.Width, rectOnImage.Height));
                            toDrawRects[(int)index] = newRect;

                            var roiMat = new Mat(mat, MyCamera.GetRectInMat(mat.Width, mat.Height, newRect));
                            var gray = Cv2.Mean(roiMat);
                            var grayPerMat =
                                Math.Round(gray.Val0 / 255, 4);
                            roiMat.Dispose();
                            grayPerList[(int)index] = grayPerMat;
                        }
                    }
                    else
                    {
                        var roiMat = new Mat(mat, MyCamera.GetRectInMat(mat.Width, mat.Height, rectOnImage));
                        var gray = Cv2.Mean(roiMat);
                        var grayPerMat =
                            Math.Round(gray.Val0 / 255, 4);
                        roiMat.Dispose();
                        grayPerList[(int)index] = grayPerMat;
                    }
                });

                mat.Dispose();

                //if (pIsCali)
                {
                    MainImageViewer.Roi.Clear();

                    foreach (var rect in toDrawRects)
                        MainImageViewer.Roi.Add(new RotatedRectangleContour(
                            new PointContour(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2), rect.Width,
                            rect.Height, 0));
                    ImageProcessing.DrawContourCountInOverlay(MainImageViewer.Image, MainImageViewer.Roi);
                }

                for (var i = 0; i < grayPerList.Count; i++)
                {
                    var grayPer = grayPerList[i];
                    var grayMinPer =
                        temp.Analysis.ShapesGroups[0][i].Min;
                    var grayMaxPer =
                        temp.Analysis.ShapesGroups[0][i].Max;
                    grayDgv.AddRow(grayMinPer, grayMaxPer, grayPer * 100 + "%");

                    if (grayPer * 100 >= double.Parse(grayMinPer.TrimEnd('%')) &&
                        grayPer * 100 <= double.Parse(grayMaxPer.TrimEnd('%')))
                    {
                        grayDgv.Rows[grayDgv.RowCount - 1].Cells[3].Value = "OK";
                    }
                    else
                    {
                        grayDgv.Rows[grayDgv.RowCount - 1].Cells[3].Value = "NG";
                        grayDgv.Rows[grayDgv.RowCount - 1].DefaultCellStyle.BackColor =
                            Color.Red;
                        MainImageViewer.Roi[i].Color = Rgb32Value.RedColor;
                        ngCount[0]++;
                    }
                }

                grayDgv.ClearSelection();
            });

            Invoke(checkAction, isCali);
            //checkAction(isCali);
            if (!isCali && ngCount[0] > 0)
            {
                isCali = true;
                Invoke(new Action(() =>
                {
                    MainImageViewer.Roi.Clear();
                    grayDgv.ClearRows();
                }));
                ngCount[0] = 0;
                toSearchRoi.Clear();
                toSearchRoi.AddRange(p1List);

                Invoke(checkAction, isCali);
                //checkAction(isCali);
            }

            //foreach (var other in allOther)
            //    other.OpenCamera();

            return ngCount[0] <= 0;
        }

        private class ImgLedState
        {
            public int ImgIndex;

            public readonly List<LedState> LedState = new List<LedState>();
        }

        private class LedState
        {
            public double Gray;
            public double Min;
            public double Max;
        }
    }
}
