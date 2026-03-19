using CheckSystem.VisionDetection.Vision;
using CommonUtility;
using MiniExcelLibs;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Sunny.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageProcessing = CheckSystem.VisionDetection.Vision.ImageProcessing;

namespace CheckSystem.VisionDetection.Control
{
    public partial class DynamicImageViewer : UserControl
    {
        private VisionImage[] _visionImages = new VisionImage[0];
        //private readonly string _tempFilePath = string.Format(@"{0}\{1}\animationTemp.jpg", Program.SysDir, "图像检测配置文件");
        private bool _isChecking;
        private string _enterTimeTs = string.Empty;

        public DynamicImageViewer()
        {
            InitializeComponent();
            dgvData.Style = UIStyle.Gray;
            dgvData.ReadOnly = true;
            dgvData.RowHeadersVisible = false;
            dgvData.AllowUserToAddRows = false;
            dgvData.AllowUserToResizeRows = false;
            dgvData.MultiSelect = true;
            dgvData.RowHeadersVisible = false;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _enterTimeTs = HighPrecisionTimer.GetTimestamp().ToString();
            //InitImageViewer(ImageView1);
            //ImageView1.SizeChanged += ImageView_SizeChanged;
        }

        public void ImageView_SizeChanged(object sender, EventArgs e)
        {
            var imgView = sender as ImageViewer;
            if (imgView != null)
                InitImageViewer(imgView);
        }

        private static void InitImageViewer(ImageViewer imgViewer)
        {
            imgViewer.ToolsShown = ViewerTools.ZoomIn |
                                   ViewerTools.ZoomOut |
                                   ViewerTools.Pan;
            imgViewer.ActiveTool = ViewerTools.Pan;
            imgViewer.ZoomToFit = false;
            imgViewer.ShowToolbar = true;
            imgViewer.ShowScrollbars = true;
            imgViewer.ShowImageInfo = true;
        }

        public void Reset()
        {
            dgvData.ClearAll();
            foreach (var t in _visionImages)
                t.Dispose();
            _visionImages = new VisionImage[0];
            //Reset(ImageView1);

            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            pictureBox1.Image = null;

            GC.Collect();
        }

        private static void Reset(ImageViewer imgViewer)
        {
            imgViewer.Roi.Clear();

            var img = new VisionImage(ImageType.U8);
            Algorithms.Copy(img, imgViewer.Image);
            img.Dispose();
            GC.Collect();
        }

        public void ReleaseImg()
        {
            foreach (var t in _visionImages)
                t.Dispose();
            _visionImages = new VisionImage[0];
            GC.Collect();
        }

        public bool DoWork(VisionConfigVisionFunc func, VisionConfigPara t, bool isLeft)
        {
            if (func == null || t == null)
                return false;

            var detail = isLeft ? func.VisionFuncDetailL[0] : func.VisionFuncDetailR[0];

            var listRoiRect = new List<Rect>();

            var listStrBase = new List<string>();
            for (var i = 0; i < detail.Analysis.ShapesGroups.Length; i++)
            {
                var str = string.Empty;
                foreach (var shape in detail.Analysis.ShapesGroups[i])
                {
                    var gray = double.Parse(shape.Max.Replace("%", ""));
                    if (gray <= 10)
                        str += "0";
                    else
                        str += "1";
                }

                listStrBase.Add(str);
            }

            var driveLetter = Path.GetPathRoot(Program.SysDir) + @"\AnimationTemp" + @"\" + func.VisionFuncName + @"_" + _enterTimeTs;
            try
            {
                if (!Directory.Exists(driveLetter))
                    Directory.CreateDirectory(driveLetter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //var _tempFilePath = string.Format(@"{0}\{1}_animationTemp.jpg", driveLetter, HighPrecisionTimer.GetTimestamp());
            //var _tempFilePath = string.Format(@"{0}\{1}\{2}_{3}_animationTemp.jpg", Program.SysDir, "图像检测配置文件", Guid.NewGuid().ToString(), func.VisionFuncName);

            const int maxTurnCount = 4;
            //var isTurn = func.VisionFuncName.ToLower().Contains("turn") || func.VisionFuncName.Contains("转向");
            var dicTurnState = new List<List<string>>();
            var dicTurnFindIndex = new Dictionary<int, int>();
            for (var i = 0; i < maxTurnCount; i++)
                dicTurnFindIndex.Add(i, -1);

            var testFunc = new Func<bool>(() =>
            {
                _isChecking = true;
                BeginInvoke(new Action(() =>
                {
                    dgvData.ClearAll();
                }));
                //var st = new Stopwatch();
                //st.Start();

                if (isLeft)
                {
                    if (func.VisionFuncDetailL == null)
                    {
                        func.VisionFuncDetailL = new VisionConfigVisionFuncCamera[1];
                        func.VisionFuncDetailL[0] = new VisionConfigVisionFuncCamera();
                    }
                }
                else
                {
                    if (func.VisionFuncDetailR == null)
                    {
                        func.VisionFuncDetailR = new VisionConfigVisionFuncCamera[1];
                        func.VisionFuncDetailR[0] = new VisionConfigVisionFuncCamera();
                    }
                }

                int count;
                if (!int.TryParse(detail.FrameCount, out count))
                    return false;

                var listStrNew = new List<string>();
                var groupIndex = 0;

                var imgLedStates = new List<ImgLedState>();

                for (var i = 0; i < count; i++)
                {
                    imgLedStates.Add(new ImgLedState { ImgIndex = i });

                    for (var j = 0; j < detail.Analysis.ShapesGroups[groupIndex].Length; j++)
                    {
                        var shape = detail.Analysis.ShapesGroups[groupIndex][j];
                        var min = double.Parse(shape.Min.Replace("%", ""));
                        var max = double.Parse(shape.Max.Replace("%", ""));

                        imgLedStates[i].LedState.Add(new LedState { Min = min, Max = max, Gray = 9999 });
                    }
                }

                for (var i = 0; i < detail.Analysis.ShapesGroups[0].Length; i++)
                {
                    var sp = ImageProcessing.GetRectByString(detail.Analysis.ShapesGroups[0][i].Rect);
                    listRoiRect.Add(new Rect((int)sp.Left, (int)sp.Top, (int)sp.Width, (int)sp.Height));
                }

                var device =
                    VisionCommon.NiImaqd.CameraList.Find(
                        f => f.GigeInfo.chSerialNumber == detail.UserId);
                if (device == null)
                    return false;
                device.ClearBuffer();
                device.OpenCamera();

                var allOther =
                    VisionCommon.NiImaqd.CameraList.FindAll(
                        f => f.GigeInfo.chSerialNumber != device.GigeInfo.chSerialNumber);
                foreach (var other in allOther)
                    other.CloseCamera();

                //var ngCount = 0;

                var captureRoi = device.GetOuterMaxRect(listRoiRect);
                device.SetExposureTime(int.Parse(detail.Shutter));
                Thread.Sleep(200);

                var perMs = 8;
                if (int.Parse(detail.Shutter) / 1000 >= 8)
                    perMs = int.Parse(detail.Shutter) / 1000;

                if (string.IsNullOrEmpty(detail.SequenceType) ||
                    detail.SequenceType == "先点亮后抓拍")
                {
                    var t1End = false;
                    var t2End = false;

                    var cameraTask = Task.Factory.StartNew(() =>
                    {
                        int ms;
                        if (int.TryParse(detail.SequenceDelay, out ms))
                        {
                            if (ms > 0)
                            {
                                var stThis = new Stopwatch();
                                stThis.Start();
                                //Task.Delay(ms).Wait();
                                while (true)
                                {
                                    if (stThis.ElapsedMilliseconds >= ms)
                                        break;
                                    Thread.Sleep(1);
                                }

                                stThis.Stop();
                            }
                        }

                        var timeOut = perMs * (uint)count + 5000;
                        device.Capture((uint)count, (int)timeOut, roiRect: captureRoi);
                        t1End = true;
                    });

                    var ledTask = Task.Factory.StartNew(() =>
                    {
                        var stThis = new Stopwatch();
                        stThis.Start();
                        VisionCommon.InvokeRelays(t.ParaReleysList);
                        //Thread.Sleep(50);
                        while (true)
                        {
                            if (stThis.ElapsedMilliseconds >= 50)
                                break;
                            Thread.Sleep(1);
                        }

                        if (t.ParaMethods != null)
                            VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsBefore);
                        t2End = true;
                    });

                    //Task.WaitAll(cameraTask, ledTask);
                    while (true)
                    {
                        if (t1End && t2End)
                            break;
                        Thread.Sleep(1);
                    }
                }
                else if (detail.SequenceType == "先抓拍后点亮")
                {
                    var t1End = false;
                    var t2End = false;

                    var cameraTask = Task.Factory.StartNew(() =>
                    {
                        var timeOut = perMs * (uint)count + 5000;
                        device.Capture((uint)count, (int)timeOut, roiRect: captureRoi);

                        t1End = true;
                    });

                    var ledTask = Task.Factory.StartNew(() =>
                    {
                        int ms;
                        if (int.TryParse(detail.SequenceDelay, out ms))
                        {
                            if (ms > 0)
                            {
                                var stThis = new Stopwatch();
                                stThis.Start();
                                while (true)
                                {
                                    if (stThis.ElapsedMilliseconds >= ms)
                                        break;
                                    Thread.Sleep(1);
                                }

                                stThis.Stop();
                            }
                        }

                        {
                            VisionCommon.InvokeRelays(t.ParaReleysList);
                            var stThis = new Stopwatch();
                            stThis.Start();
                            while (true)
                            {
                                if (stThis.ElapsedMilliseconds >= 50)
                                    break;
                                Thread.Sleep(1);
                            }

                            stThis.Stop();
                        }

                        if (t.ParaMethods != null)
                            VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsBefore);

                        t2End = true;
                    });

                    while (true)
                    {
                        if (t1End && t2End)
                            break;

                        Thread.Sleep(1);
                    }
                }

                //device.CloseCamera();
                var isOpenOtherCamera = false;
                Task.Factory.StartNew(() =>
                {
                    VisionCommon.InvokeRelays(t.ParaReleysList, true);
                    Thread.Sleep(25);

                    if (t.ParaMethods != null)
                        VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsAfter);

                    isOpenOtherCamera = true;
                });

                // 定义新的尺寸

                var totalToDrawTemp = Math.Sqrt(count).ToString(CultureInfo.InvariantCulture);
                int totalToDraw;
                if (!int.TryParse(totalToDrawTemp, out totalToDraw))
                    totalToDraw = (int)Math.Sqrt(count) + 1;

                const int newWidth = 400;
                const int newHeight = 400;
                //var canvas = new Mat(new Size(8000, 8000), MatType.CV_8UC3, Scalar.Black); // 使用白色背景
                var canvas = new Mat(new Size(newWidth * totalToDraw, newHeight * totalToDraw), MatType.CV_8UC3, Scalar.Black); // 使用白色背景
                var totalImageCount = totalToDraw * totalToDraw;

                var index1 = groupIndex;
                var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
                Parallel.For((long)0, count, options, i =>
                {
                    var imgIndex = i;
                    var g = detail.Analysis.ShapesGroups[index1].ToList();
                    var items = new ConcurrentBag<VisionConfigVisionFuncCameraAnalysisShapesShape>(g);

                    if (imgIndex < count)
                    {
                        if (imgIndex < totalImageCount)
                        {
                            // 使用Cv2.Resize函数进行非等比例缩放
                            int mRow, mCol;
                            var toResizedImage =
                                device.GetImageFromBuff((int)imgIndex, out mRow, out mCol)
                                    .Clone(); //device.MatBuffer[mRow, mCol].Clone();

                            Parallel.ForEach(items, (shape, state, index) =>
                            {
                                //var ledIndex = g.IndexOf(shape);

                                var rect = ImageProcessing.GetRectByString(shape.Rect);
                                var rectOnImage = new Rect((int)rect.Left - device.LastCaptureRoiOffsetX,
                                    (int)rect.Top - device.LastCaptureRoiOffsetY, (int)rect.Width, (int)rect.Height);
                                Cv2.Rectangle(toResizedImage, rectOnImage, Scalar.White);

                                var roiMat = new Mat(
                                    device.GetImageFromBuff((int)imgIndex, out mRow, out mCol),
                                    CommonUtility.HikSdk.MyCamera.GetRectInMat(
                                        device.GetImageFromBuff((int)imgIndex, out mRow, out mCol).Width,
                                        device.GetImageFromBuff((int)imgIndex, out mRow, out mCol).Height,
                                        rectOnImage)); //new Mat(device.MatBuffer[mRow, mCol], rectOnImage);
                                var meanVal = Cv2.Mean(roiMat);
                                roiMat.Dispose();
                                var grayPer = Math.Round(meanVal.Val0, 1, MidpointRounding.AwayFromZero);
                                grayPer = CommonUtility.HikSdk.MyCamera.GetLxByRgb(meanVal);

                                imgLedStates[(int)imgIndex].LedState[(int)index].Gray = grayPer;
                            });

                            var srcWidth = toResizedImage.Width;
                            var srcHeight = toResizedImage.Height;

                            // 计算原始图像的宽高比
                            var aspectRatio = (double)srcWidth / srcHeight;

                            // 计算新的宽度和高度，保持宽高比并且不超过目标尺寸
                            int newResizeWidth, newResizeHeight;
                            if (aspectRatio > 1.0)
                            {
                                // 宽度大于高度
                                newResizeWidth = Math.Min(newWidth, (int)(newHeight * aspectRatio));
                                newResizeHeight = (int)(newResizeWidth / aspectRatio);
                            }
                            else
                            {
                                // 高度大于或等于宽度
                                newResizeHeight = Math.Min(newWidth, (int)(newHeight / aspectRatio));
                                newResizeWidth = (int)(newResizeHeight * aspectRatio);
                            }

                            // 创建一个新的Mat对象来保存缩放后的图片
                            var resizedImage = new Mat(newWidth, newHeight, MatType.CV_8UC3, Scalar.Black);
                            Cv2.PutText(resizedImage, string.Format("[{0},{1}],img={2}", mRow, mCol, imgIndex), new Point(1, 30), HersheyFonts.HersheyComplex, 0.45, Scalar.WhiteSmoke);

                            // 计算放置新图像的位置
                            var x = (newWidth - newResizeWidth) / 2;
                            var y = (newHeight - newResizeHeight) / 2;

                            //Cv2.Resize(device.MatBuffer[mRow, mCol], resizedImage, new Size(newWidth, newHeight), 0, 0, InterpolationFlags.Linear);
                            Mat tempResizedImage = new Mat();
                            Cv2.Resize(toResizedImage, tempResizedImage, new Size(newResizeWidth, newResizeHeight));

                            // 将调整后的图像复制到中心位置
                            tempResizedImage.CopyTo(resizedImage[new Rect(x, y, newResizeWidth, newResizeHeight)]);

                            tempResizedImage.Dispose();
                            toResizedImage.Dispose();
                            // 给缩放后的图像加上1像素的边框
                            var thickness = 1;
                            Cv2.Rectangle(resizedImage, new Point(0, 0), new Point(resizedImage.Width - thickness, resizedImage.Height - thickness), Scalar.White);
                            // 定义缩放图像在空白图像中的位置
                            var newRowIndex = imgIndex / totalToDraw;
                            var newColIndex = imgIndex % totalToDraw;
                            //Console.WriteLine("newRowIndex={0},newColIndex={1}", newRowIndex, newColIndex);
                            var offsetX = newColIndex * newWidth; // 缩放图像的左上角x坐标
                            var offsetY = newRowIndex * newHeight; // 缩放图像的左上角y坐标
                            //Console.WriteLine("offsetX={0},offsetY={1}", offsetX, offsetY);
                            // 在空白图像上定义缩放图像的ROI
                            using (var roi = new Mat(canvas, new Rect((int)offsetX, (int)offsetY, resizedImage.Width, resizedImage.Height)))
                            {
                                // 将缩放后的图像复制到空白图像的指定位置
                                resizedImage.CopyTo(roi);
                                // 释放资源
                                resizedImage.Dispose();
                                roi.Dispose();
                            }
                        }
                    }
                });

                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }
                pictureBox1.Image = canvas.ToBitmap();

                BeginInvoke(new Action(() =>
                {
                    dgvData.AddColumn("ImgIndex", "ImgIndex");
                    for (var i = 0; i < detail.Analysis.ShapesGroups[0].Length; i++)
                        dgvData.AddColumn("Roi" + i, "Roi" + i);

                    for (var i = 0; i < imgLedStates.Count; i++)
                    {
                        var objs = new object[detail.Analysis.ShapesGroups[0].Length + 1];
                        objs[0] = i;

                        for (var j = 0; j < imgLedStates[i].LedState.Count; j++)
                            objs[j + 1] = string.Format(
                                "{0}/{1}={2}", imgLedStates[i].LedState[j].Gray,
                                double.Parse(detail.Analysis.ShapesGroups[0][j].Value.TrimEnd('%')),
                                Math.Round(
                                    imgLedStates[i].LedState[j].Gray /
                                    double.Parse(detail.Analysis.ShapesGroups[0][j].Value.TrimEnd('%')), 1,
                                    MidpointRounding.AwayFromZero));

                        dgvData.AddRow(objs);
                    }
                }));

                for (var v = 0; v < imgLedStates.Count; v++)
                {
                    var result = imgLedStates[v];
                    var isPass = false;
                    var str = string.Empty;

                    for (var qq = 0; qq < result.LedState.Count; qq++)
                    {
                        var eachLed = result.LedState[qq];
                        var standardGray = double.Parse(detail.Analysis.ShapesGroups[0][qq].Value.TrimEnd('%'));
                        var standardUnif = double.Parse(detail.Analysis.ShapesGroups[0][qq].Min.TrimEnd('%'));

                        var thisGray = eachLed.Gray;
                        var unif = thisGray / standardGray;

                        if (unif <= 0.15)
                        {
                            str += "0";
                        }
                        else if (unif > 0.15 && unif <= standardUnif)
                        {
                            isPass = true;
                            continue;
                        }
                        else //if (unif > 0.3 && unif <= 0.7)
                        {
                            str += "1";
                        }
                    }

                    if (!isPass && str.Length == result.LedState.Count)
                    {
                        if (!listStrNew.Any())
                        {
                            if (str == listStrBase[0])
                            {
                                //groupIndex++;
                                listStrNew.Add(string.Format("{0}", str));
                            }
                        }
                        else
                        {
                            if (listStrNew[listStrNew.Count - 1] != str)
                            {
                                //groupIndex++;
                                listStrNew.Add(string.Format("{0}", str));
                            }
                        }
                    }

                    if (listStrNew.Count == listStrBase.Count && (func.VisionFuncName.ToLower().Contains("turn") || func.VisionFuncName.Contains("转向")))
                        break;

                    //if ((func.VisionFuncName.ToLower().Contains("turn") || func.VisionFuncName.Contains("转向")))
                    //    break;
                }

                dicTurnState.Add(listStrNew);

                BeginInvoke(new Action(() =>
                {
                    tabControl1.SelectedIndex = 0;

                    uiRichTextBox1.Clear();
                    uiRichTextBox2.Clear();

                    foreach (var str in listStrBase)
                        uiRichTextBox1.AppendText(str + Environment.NewLine);

                    foreach (var str in listStrNew)
                        uiRichTextBox2.AppendText(str + Environment.NewLine);
                }));

                _isChecking = false;

                //var fileName = string.Format(@"E:\Projects\VW491\动画NG-记录\{0}_{1}.jpg", func.VisionFuncName,
                //    Guid.NewGuid().ToString());

                var returnFunc = new Func<bool, bool>(pIsOk =>
                {
                    if (canvas != null && !canvas.Empty())
                        canvas.Dispose();

                    device.ClearBuffer();

                    while (true)
                    {
                        if (isOpenOtherCamera)
                            break;
                        Thread.Sleep(1);
                    }

                    GC.Collect();

                    return pIsOk;
                });

                #region 20241219新改，每组最多丢弃3张

                if (listStrBase.Count <= listStrNew.Count &&
                    listStrNew.Count > 2 &&
                    listStrBase.Count > 2)
                {
                    var searchNewIndex = 0;
                    var searchBaseIndex = 0;

                    var matchStartIndex = new Dictionary<int, List<string>>();

                    for (var i = 0; i < listStrNew.Count; i++)
                    {
                        if (listStrNew[i] != listStrBase[0] || listStrNew.Count - i + 1 < listStrBase.Count)
                            continue;

                        matchStartIndex.Add(i, new List<string>());
                        for (var j = i; j < listStrNew.Count; j++)
                            matchStartIndex[i].Add(listStrNew[j]);
                    }

                    if (!matchStartIndex.Any())
                        return returnFunc.Invoke(false);

                    var keys = matchStartIndex.Keys.ToList();
                    foreach (var key in keys)
                    {
                        var notFindIndex = new List<int>();

                        var toMatchListStr = matchStartIndex[key];
                        var thisGroupRetryCount = 0;
                        var currentMatchedIndex = 0;

                        if (toMatchListStr.Count < listStrBase.Count)
                            continue;

                        foreach (var t1 in toMatchListStr)
                        {
                            if (t1 == listStrBase[currentMatchedIndex])
                            {
                                currentMatchedIndex++;
                                thisGroupRetryCount = 0;
                            }
                            else
                            {
                                if (thisGroupRetryCount < 3)
                                {
                                    thisGroupRetryCount++;
                                    continue;
                                }

                                currentMatchedIndex++;
                                thisGroupRetryCount = 0;

                                notFindIndex.Add(currentMatchedIndex);
                                continue;
                            }

                            if (currentMatchedIndex != listStrBase.Count)
                                continue;

                            if (!notFindIndex.Any())
                                return returnFunc(true);

                            var findOtherCount = 0;
                            for (var i = 0; i < notFindIndex.Count; i++)
                            {
                                if (listStrNew.Any(str => str == listStrBase[i]))
                                {
                                    findOtherCount++;
                                    break;
                                }
                            }

                            if (findOtherCount == notFindIndex.Count)
                                return returnFunc(true);

                            return returnFunc(false);
                        }
                    }

                    return returnFunc.Invoke(false);
                }

                #endregion

                #region 在用的方法

                //if (listStrBase.Count <= listStrNew.Count &&
                //    listStrNew.Count > 2 &&
                //    listStrBase.Count > 2)
                //{
                //    var searchNewIndex = 0;
                //    var searchBaseIndex = 0;

                //    for (var i = 0; i < listStrNew.Count; i++)
                //    {
                //        if (i != searchNewIndex)
                //            return returnFunc(false);

                //        if (listStrNew[searchNewIndex] != listStrBase[searchBaseIndex])
                //        {
                //            if (searchNewIndex >= listStrNew.Count - 1)
                //                return returnFunc(false);

                //            if (listStrNew[searchNewIndex + 1] == listStrBase[searchBaseIndex])
                //            {
                //                searchNewIndex++;
                //                continue;
                //            }
                //            return returnFunc(false);
                //        }

                //        searchNewIndex++;
                //        searchBaseIndex++;

                //        if (searchBaseIndex == listStrBase.Count)
                //            break;
                //    }

                //    return returnFunc(true);
                //}

                #endregion

                return returnFunc.Invoke(false);
            });

            if (func.VisionFuncName.ToLower().Contains("turn") ||
                func.VisionFuncName.Contains("转向"))
            {
                var listNewTurn = new List<string>();
                var currBase = -1;

                for (var i = 0; i < maxTurnCount; i++)
                {
                    if (testFunc.Invoke())
                        return true;

                    foreach (var eachTurn in dicTurnState)
                    {
                        var baseOnCount = 0;
                        foreach (var findOnCount in eachTurn.Select(str => str.ToCharArray().ToList().FindAll(f => f == '1').Count))
                        {
                            if (findOnCount < baseOnCount)
                                return false;
                            baseOnCount = findOnCount;
                        }
                    }

                    for (var j = 0; j < listStrBase.Count; j++)
                    {
                        if (j <= currBase)
                            continue;

                        var isNotMatch = true;
                        foreach (var eachTurn in dicTurnState)
                        {
                            var findIndex = eachTurn.FindIndex(f => f == listStrBase[j]);
                            if (findIndex != -1 && findIndex > dicTurnFindIndex[i])
                            {
                                currBase = j;
                                dicTurnFindIndex[i] = findIndex;
                                listNewTurn.Add(eachTurn[findIndex]);
                                isNotMatch = false;
                                break;
                            }
                        }

                        if (isNotMatch)
                            break;
                    }

                    if (listNewTurn.Count == listStrBase.Count)
                        break;
                }

                BeginInvoke(new Action(() =>
                {
                    dgvData.ClearAll();

                    tabControl1.SelectedIndex = 0;

                    uiRichTextBox1.Clear();
                    uiRichTextBox2.Clear();

                    foreach (var str in listStrBase)
                        uiRichTextBox1.AppendText(str + Environment.NewLine);

                    uiRichTextBox2.AppendText(Environment.NewLine);
                    for (var i = 0; i < dicTurnState.Count; i++)
                    {
                        uiRichTextBox2.AppendText(i + ":" + Environment.NewLine);

                        foreach (var eachTurn in dicTurnState[i])
                            uiRichTextBox2.AppendText(eachTurn + Environment.NewLine);
                    }

                    uiRichTextBox2.AppendText(Environment.NewLine);
                    uiRichTextBox2.AppendText("Combine:" + Environment.NewLine);
                    foreach (var str in listNewTurn)
                        uiRichTextBox2.AppendText(str + Environment.NewLine);
                }));

                if (listStrBase.Count <= listNewTurn.Count && listNewTurn.Count > 2 && listStrBase.Count > 2)
                {
                    for (var i = 0; i < listNewTurn.Count; i++)
                    {
                        if (listNewTurn[i] != listStrBase[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }

                return false;
            }

            return testFunc.Invoke();
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

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null && !_isChecking)
            {
                // 选择路径
                string path;
                using (var folder = new FolderBrowserDialog())
                {
                    var result = folder.ShowDialog();
                    if (result != DialogResult.OK)
                        return;
                    path = folder.SelectedPath + string.Format(@"\{0}_{1}.jpg",
                        DateTime.Now.ToString(CultureInfo.InvariantCulture)
                            .Replace(":", string.Empty)
                            .Replace("/", string.Empty)
                            .Replace(" ", string.Empty),
                        Guid.NewGuid().ToString());

                    pictureBox1.Image.Save(path, ImageFormat.Jpeg);

                    //File.Open(path, FileMode.Open);
                }
            }
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvData.RowCount <= 0)
                return;

            // 创建一个保存文件对话框
            var saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = @"Excel文件|*.xlsx";
            saveFileDialog1.Title = @"保存为Excel文件";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog1.FileName))
            {
                try
                {
                    // 将 DataGridView 数据转换为 DataTable
                    var dataTable = new DataTable();
                    foreach (DataGridViewColumn column in dgvData.Columns)
                        dataTable.Columns.Add(column.HeaderText, typeof(string));

                    foreach (DataGridViewRow row in dgvData.Rows)
                    {
                        if (row.IsNewRow)
                            continue; // 忽略新行的添加
                        var dataRow = dataTable.NewRow();
                        for (var i = 0; i < row.Cells.Count; i++)
                            dataRow[i] = row.Cells[i].Value;
                        dataTable.Rows.Add(dataRow);
                    }

                    // 使用 MiniExcel 保存数据到 Excel 文件
                    MiniExcel.SaveAs(saveFileDialog1.FileName, dataTable);
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
        }
    }
}
