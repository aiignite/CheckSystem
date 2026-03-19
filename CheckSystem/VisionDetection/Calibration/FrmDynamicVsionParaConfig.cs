using CheckSystem.VisionDetection.Vision;
using CommonUtility;
using CommonUtility.FileOperator;
using CommonUtility.HikSdk;
using MiniExcelLibs;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using OpenCvSharp;
using Sunny.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageProcessing = CheckSystem.VisionDetection.Vision.ImageProcessing;
using Point = System.Drawing.Point;
using Range = NationalInstruments.Vision.Range;
using Size = System.Drawing.Size;

namespace CheckSystem.VisionDetection.Calibration
{
    public sealed partial class FrmDynamicVsionParaConfig : UIForm
    {
        private readonly NationalInstruments.Vision.WindowsForms.ImageViewer _mainImageViewer = new ImageViewer
        { Dock = DockStyle.Fill };

        private readonly List<VisionConfigVisionFuncCameraAnalysisShapesShape[]> _toSave =
            new List<VisionConfigVisionFuncCameraAnalysisShapesShape[]>();

        public FrmDynamicVsionParaConfig(string funcName, bool isLeft)
        {
            InitializeComponent();
            panel1.Controls.Add(_mainImageViewer);

            dgvData.Style = UIStyle.Gray;
            dgvData.ReadOnly = true;
            dgvData.RowHeadersVisible = false;
            dgvData.AllowUserToAddRows = false;
            dgvData.AllowUserToResizeRows = false;
            dgvData.MultiSelect = true;
            dgvData.RowHeadersVisible = false;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LoadCamera();
            Closed += FrmDynamicVsionParaConfig_Closed;

            var func = VisionCommon.VisionConfig.VisionInfo.ToList().Find(f => f.VisionFuncName == funcName);
            if (func != null)
            {
                Func = func;

                if (isLeft)
                {
                    if (Func.VisionFuncDetailL == null || !Func.VisionFuncDetailL.Any())
                    {
                        Func.VisionFuncDetailL = new VisionConfigVisionFuncCamera[1];
                        Func.VisionFuncDetailL[0] = new VisionConfigVisionFuncCamera
                        {
                            Shutter = "10000",
                            UserId = VisionCommon.NiImaqd != null && VisionCommon.NiImaqd.CameraList.Count > 0
                                ? VisionCommon.NiImaqd.CameraList[0].GigeInfo.chSerialNumber
                                : string.Empty,
                            FrameCount = "50",
                            SequenceType = "先点亮后抓拍",
                            SequenceDelay = "50",
                            Analysis = new VisionConfigVisionFuncCameraAnalysis
                            {
                                CaliRegion = new string[0],
                                LookupTable = CommonUtility.ImageProcessing.LookupTableType.ImageSource.ToString(),
                                ShapesGroups = new VisionConfigVisionFuncCameraAnalysisShapesShape[1][]
                            }
                        };
                        Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0] = new VisionConfigVisionFuncCameraAnalysisShapesShape[0];
                    }
                }
                else
                {
                    if (Func.VisionFuncDetailR == null || !Func.VisionFuncDetailR.Any())
                    {
                        Func.VisionFuncDetailR = new VisionConfigVisionFuncCamera[1];
                        Func.VisionFuncDetailR[0] = new VisionConfigVisionFuncCamera
                        {
                            Shutter = "10000",
                            UserId = VisionCommon.NiImaqd != null && VisionCommon.NiImaqd.CameraList.Count > 0
                                ? VisionCommon.NiImaqd.CameraList[0].GigeInfo.chSerialNumber
                                : string.Empty,
                            FrameCount = "50",
                            SequenceType = "先点亮后抓拍",
                            SequenceDelay = "50",
                            Analysis = new VisionConfigVisionFuncCameraAnalysis
                            {
                                CaliRegion = new string[0],
                                LookupTable = CommonUtility.ImageProcessing.LookupTableType.ImageSource.ToString(),
                                ShapesGroups = new VisionConfigVisionFuncCameraAnalysisShapesShape[1][]
                            }
                        };
                        Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0] = new VisionConfigVisionFuncCameraAnalysisShapesShape[0];
                    }
                }
            }
            else
            {
                Func = new VisionConfigVisionFunc
                {
                    VisionFuncName = funcName,
                    VisionFuncDetailL = new VisionConfigVisionFuncCamera[1],
                    VisionFuncDetailR = new VisionConfigVisionFuncCamera[1]
                };

                var tempFunc = new VisionConfigVisionFuncCamera
                {
                    Shutter = "10000",
                    UserId = VisionCommon.NiImaqd != null && VisionCommon.NiImaqd.CameraList.Count > 0
                         ? VisionCommon.NiImaqd.CameraList[0].GigeInfo.chSerialNumber
                         : string.Empty,
                    FrameCount = "50",
                    SequenceType = "先点亮后抓拍",
                    SequenceDelay = "50",
                    Analysis = new VisionConfigVisionFuncCameraAnalysis
                    {
                        CaliRegion = new string[0],
                        LookupTable = CommonUtility.ImageProcessing.LookupTableType.ImageSource.ToString(),
                        ShapesGroups = new VisionConfigVisionFuncCameraAnalysisShapesShape[1][]
                    }
                };

                if (isLeft)
                {
                    Func.VisionFuncDetailL[0] = tempFunc;
                    Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0] = new VisionConfigVisionFuncCameraAnalysisShapesShape[0];
                }
                else
                {
                    Func.VisionFuncDetailR[0] = tempFunc;
                    Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0] = new VisionConfigVisionFuncCameraAnalysisShapesShape[0];
                }
            }

            if (isLeft)
            {
                if (Func.VisionFuncDetailL[0].SequenceType == "先点亮后抓拍")
                    cmbSequenceType.SelectedIndex = 0;
                else if (Func.VisionFuncDetailL[0].SequenceType == "先抓拍后点亮")
                    cmbSequenceType.SelectedIndex = 1;
                else
                {
                    Func.VisionFuncDetailL[0].SequenceType = "先点亮后抓拍";
                    cmbSequenceType.SelectedIndex = 1;
                }

                if (string.IsNullOrEmpty(Func.VisionFuncDetailL[0].Shutter))
                    Func.VisionFuncDetailL[0].Shutter = "10000";
                if (string.IsNullOrEmpty(Func.VisionFuncDetailL[0].FrameCount))
                    Func.VisionFuncDetailL[0].FrameCount = "50";
                if (string.IsNullOrEmpty(Func.VisionFuncDetailL[0].SequenceDelay))
                    Func.VisionFuncDetailL[0].SequenceDelay = "50";
                txtShutter.Value = int.Parse(Func.VisionFuncDetailL[0].Shutter);
                txtFrameCount.Value = int.Parse(Func.VisionFuncDetailL[0].FrameCount);
                txtSequenceDelay.Value = int.Parse(Func.VisionFuncDetailL[0].SequenceDelay);

                for (var i = 0; i < cmbCameras.Items.Count; i++)
                {
                    if (cmbCameras.Items[i].ToString() != Func.VisionFuncDetailL[0].UserId)
                        continue;
                    cmbCameras.SelectedIndex = i;
                    break;
                }
            }
            else
            {
                if (Func.VisionFuncDetailR[0].SequenceType == "先点亮后抓拍")
                    cmbSequenceType.SelectedIndex = 0;
                else if (Func.VisionFuncDetailR[0].SequenceType == "先抓拍后点亮")
                    cmbSequenceType.SelectedIndex = 1;

                if (string.IsNullOrEmpty(Func.VisionFuncDetailR[0].Shutter))
                    Func.VisionFuncDetailR[0].Shutter = "10000";
                if (string.IsNullOrEmpty(Func.VisionFuncDetailR[0].FrameCount))
                    Func.VisionFuncDetailR[0].FrameCount = "50";
                if (string.IsNullOrEmpty(Func.VisionFuncDetailR[0].SequenceDelay))
                    Func.VisionFuncDetailR[0].SequenceDelay = "50";

                txtShutter.Text = Func.VisionFuncDetailR[0].Shutter;
                txtFrameCount.Text = Func.VisionFuncDetailR[0].FrameCount;
                txtSequenceDelay.Text = Func.VisionFuncDetailR[0].SequenceDelay;

                for (var i = 0; i < cmbCameras.Items.Count; i++)
                {
                    if (cmbCameras.Items[i].ToString() != Func.VisionFuncDetailR[0].UserId)
                        continue;
                    cmbCameras.SelectedIndex = i;
                    break;
                }
            }

            IsLeft = isLeft;
            Text += @" (" + Func.VisionFuncName + @")";

            //UIStyles.InitColorful(Color.FromArgb(80, 126, 164), Color.White);
            Size =
                MinimumSize =
                    MaximumSize =
                        new Size(Screen.GetWorkingArea(this).Width - 50, Screen.GetWorkingArea(this).Height - 50);
            StartPosition = FormStartPosition.CenterScreen;
            Location = new Point(25, 25);

            _mainImageViewer.ToolsShown = ViewerTools.ZoomIn |
                                          ViewerTools.ZoomOut |
                                          ViewerTools.Pan |
                                          ViewerTools.Selection;
            _mainImageViewer.ActiveTool = ViewerTools.Pan;
            _mainImageViewer.ZoomToFit = true;
            _mainImageViewer.ShowToolbar = true;
            _mainImageViewer.ShowScrollbars = true;
            _mainImageViewer.ShowImageInfo = true;
            _mainImageViewer.SizeChanged += MainImageViewer_SizeChanged;
            _mainImageViewer.RoiChanged += _mainImageViewer_RoiChanged;

            toolStripCmbImgList.SelectedIndexChanged += toolStripCmbImgList_SelectedIndexChanged;
            cmbCameras.SelectedIndexChanged += CmbCameras_SelectedIndexChanged;
        }

        private void CmbCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var t in VisionCommon.NiImaqd.CameraList.Where(t => t != null))
                t.ClearBuffer();

            ImgCount = 0;
            toolStripCmbImgList.Items.Clear();
            cmbGroup.Items.Clear();

            // 设定图像的宽度和高度
            var width = 640;
            var height = 480;

            // 创建一个黑色的3通道图像
            // MatType.CV_8UC3 表示8位无符号的3通道图像
            var blackImage = new Mat(height, width, MatType.CV_8UC3, Scalar.Black);

            var visionImg = CommonUtility.HikSdk.MyCamera.MatToVisionImage(blackImage);
            Algorithms.Copy(visionImg, _mainImageViewer.Image);
            visionImg.Dispose();
            blackImage.Dispose();
            GC.Collect();
        }

        private void LoadCamera()
        {
            //var cameraTreeNode = new TreeNode { Text = @"相机列表" };

            if (VisionCommon.NiImaqd != null)
            {
                foreach (var t in VisionCommon.NiImaqd.CameraList)
                    cmbCameras.Items.Add(t.GigeInfo.chSerialNumber);

                if (VisionCommon.NiImaqd.CameraList.Any())
                    cmbCameras.SelectedIndex = 0;
            }
        }

        private void FrmDynamicVsionParaConfig_Closed(object sender, EventArgs e)
        {
            foreach (var device in VisionCommon.NiImaqd.CameraList.Where(device => device != null))
                device.ClearBuffer();
        }

        private void MainImageViewer_SizeChanged(object sender, EventArgs e)
        {
            _mainImageViewer.ToolsShown = ViewerTools.ZoomIn |
                                          ViewerTools.ZoomOut |
                                          ViewerTools.Pan |
                                          ViewerTools.Selection;

            _mainImageViewer.ActiveTool = ViewerTools.Pan;
            _mainImageViewer.ZoomToFit = true;
            _mainImageViewer.ShowToolbar = true;
            _mainImageViewer.ShowScrollbars = true;
            _mainImageViewer.ShowImageInfo = true;
        }

        private void _mainImageViewer_RoiChanged(object sender, ContoursChangedEventArgs e)
        {
            ImageProcessing.DrawContourCountInOverlay(_mainImageViewer.Image, _mainImageViewer.Roi);
        }

        private VisionConfigVisionFunc Func { get; set; }
        private int ImgCount { get; set; }
        private bool IsLeft { get; set; }
        private void btnAddRect_Click(object sender, EventArgs e)
        {
            if (_mainImageViewer.Image.Width != 0 && _mainImageViewer.Image.Height != 0)
            {
                var shape = new RectangleContour(0, 0, 200, 200);
                _mainImageViewer.Roi.Add(shape);
            }
        }

        private void btnFindCircle_Click(object sender, EventArgs e)
        {
            var device = VisionCommon.NiImaqd.CameraList.Find(f => f.GigeInfo.chSerialNumber == cmbCameras.Text);
            if (device == null)
            {
                this.ShowErrorTip("未找到相机");
                return;
            }

            if (toolStripCmbImgList.SelectedIndex >= 0 && toolStripCmbImgList.Items.Count > 0)
            {
                var rowIndex = -1;
                var colIndex = -1;
                var index = -1;

                for (var i = 0; i < device.MatBuffer.GetLength(0); i++)
                {
                    for (var j = 0; j < device.MatBuffer.GetLength(1); j++)
                    {
                        if (device.MatBuffer[i, j] != null && !string.IsNullOrEmpty(device.MatBuffer[i, j]))
                        {
                            index++;
                            if (index == toolStripCmbImgList.SelectedIndex)
                            {
                                rowIndex = i;
                                colIndex = j;
                                break;
                            }
                        }
                    }
                }

                if (index == -1 || rowIndex == -1 || colIndex == -1)
                {
                    this.ShowErrorTip("未找到图片");
                    return;
                }

                var srcImg = CommonUtility.HikSdk.MyCamera.MatToVisionImage(CommonUtility.HikSdk.MyCamera.Base64StringToMat(device.MatBuffer[rowIndex, colIndex]));

                var option = new UIEditOption
                {
                    AutoLabelWidth = true,
                    Text = "确认"
                };

                var isLowPixel = srcImg.Width <= 1000 && srcImg.Height <= 1000;

                option.AddInteger("RadiusMin", "最小半径", isLowPixel ? 2 : 5);
                option.AddInteger("RadiusMax", "最大半径", 100);
                option.AddInteger("MatchRange", "过滤间隔", isLowPixel ? 1 : 10);
                option.AddInteger("RectExpand", "以圆心扩大多找画框", isLowPixel ? 1 : 2);

                var frm = new UIEditForm(option);
                frm.Render();
                frm.ShowDialog();

                if (!frm.IsOK)
                {
                    this.ShowInfoTip("操作取消");
                    return;
                }

                var matchRange = (int)frm["MatchRange"];//20 - 15;
                var radiusExpandPixel = (int)frm["RectExpand"];//10 - 0;
                var radiusMin = (int)frm["RadiusMin"];
                var radiusMax = (int)frm["RadiusMax"];

                if (matchRange < 0 || radiusExpandPixel < 0 || radiusMin < 0 || radiusMax < 0)
                {
                    this.ShowErrorTip("参数填写错误，不能<0");
                    return;
                }

                _mainImageViewer.Roi.Clear();

                var cImg = ImageProcessing.ColorPlaneExtraction(srcImg, ImageProcessing.ColorPlaneExtractionType.Red);
                if (cImg != null)
                {
                    Algorithms.Copy(cImg, srcImg);
                    cImg.Dispose();
                }

                var typeToLookup =
                    EnumOperater.GetEnumByValue<ImageProcessing.LookupTableType>(ImageProcessing.LookupTableType.Exponential);
                var lookupImg = ImageProcessing.LookupTable(srcImg, typeToLookup);
                Algorithms.Copy(lookupImg, srcImg);
                lookupImg.Dispose();

                List<CircleReport> circles;
                var rectList = new List<RectangleContour>();


                circles = ImageProcessing.FindCircle(srcImg, new Range(radiusMin, radiusMax)).OrderBy(f => f.Center.X).ToList();
                var rAll = circles.Sum(c => c.Radius);

                if (radiusExpandPixel == 9999)
                    radiusExpandPixel = rAll / circles.Count;

                //if (srcImg.Width <= 1000 && srcImg.Height <= 1000)
                //{
                //    circles = ImageProcessing.FindCircle(srcImg, new Range(2, 100)).OrderBy(f => f.Center.X).ToList();
                //    var rAll = circles.Sum(c => c.Radius);
                //    //matchRange = 0;
                //    radutsExpandPixel = rAll / circles.Count;
                //    //radutsExpandPixel = 0;
                //}
                //else
                //{
                //    circles = ImageProcessing.FindCircle(srcImg, new Range(5, 100)).OrderBy(f => f.Center.X).ToList();
                //}

                foreach (var t in from t in circles
                                  let find = rectList.Find(
                                      f =>
                                          Math.Abs(f.Left - t.Center.X) <= matchRange &&
                                          Math.Abs(f.Top - t.Center.Y) <= matchRange)
                                  where find == null
                                  select t)
                    rectList.Add(new RectangleContour(t.Center.X - (t.Radius + radiusExpandPixel),
                        t.Center.Y - (t.Radius + radiusExpandPixel), (t.Radius + radiusExpandPixel) * 2,
                        (t.Radius + radiusExpandPixel) * 2));

                foreach (var t in rectList)
                    _mainImageViewer.Roi.Add(t);

                srcImg.Dispose();

                toolStripCmbImgList_SelectedIndexChanged(null, null);
            }
        }

        private void btnLearn_Click(object sender, EventArgs e)
        {
            #region debug 测试

            var device = VisionCommon.NiImaqd.CameraList.Find(f => f.GigeInfo.chSerialNumber == cmbCameras.Text);
            if (device == null)
            {
                this.ShowErrorTip("未找到相机");
                return;
            }

            var option = new UIEditOption
            {
                AutoLabelWidth = true,
                Text = "确认"
            };
            option.AddInteger("Start", "从第几张图像开始", 0);
            option.AddInteger("End", "至第几张图像结束", 0);
            option.AddInteger("AllOn", "LED全亮在第几张图像", 0);
            option.AddDouble("OnOffThreshold", "分割阈值", 0.45);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
                return;

            var startIndex = (int)frm["Start"];
            var endIndex = (int)frm["End"];
            var allOnIndex = (int)frm["AllOn"];
            var onOffThreshold = Math.Round((double)frm["OnOffThreshold"], 3, MidpointRounding.AwayFromZero);

            if (startIndex < 0 || startIndex > ImgCount - 1)
            {
                this.ShowErrorTip("开始位置输入错误");
                return;
            }

            if (endIndex < 0 || endIndex > ImgCount - 1)
            {
                this.ShowErrorTip("结束位置输入错误");
                return;
            }

            if (allOnIndex < 0 || allOnIndex > ImgCount - 1)
            {
                this.ShowErrorTip("全亮位置输入错误");
                return;
            }

            if (endIndex < startIndex)
            {
                this.ShowErrorTip("结束位置不能小于开始位置");
                return;
            }

            if (allOnIndex < startIndex)
            {
                this.ShowErrorTip("全亮位置不能小于开始位置");
                return;
            }

            if (onOffThreshold <= 0.15 || onOffThreshold >= 1)
            {
                this.ShowErrorTip("分割阈值必须在0.15~1之间");
                return;
            }

            var st = new Stopwatch();
            st.Start();

            if (_mainImageViewer.Roi.Count > 0)
            {
                Enabled = false;
                _toSave.Clear();
                txtLearnView.Clear();
                cmbGroup.Items.Clear();
                dgvData.ClearAll();

                //var indexPairs = new List<KeyValuePair<int, int>>();
                //for (var i = 0; i < device.MatBuffer.GetLength(0); i++)
                //    for (var j = 0; j < device.MatBuffer.GetLength(1); j++)
                //        indexPairs.Add(new KeyValuePair<int, int>(i, j));

                var tempStandard = new List<LedState>();

                var grayClass = new VisionConfigVisionFuncCameraAnalysisShapesShape[_mainImageViewer.Roi.Count];
                _mainImageViewer.Image.Overlays.Default.Clear();

                for (var ri = 0; ri < _mainImageViewer.Roi.Count; ri++)
                {
                    grayClass[ri] = new VisionConfigVisionFuncCameraAnalysisShapesShape();

                    var shape = _mainImageViewer.Roi[ri].Shape;
                    if (shape is PolygonContour) // 多边形
                    {
                        var polygonContour = (PolygonContour)shape;

                        grayClass[ri].Type = typeof(PolygonContour).Name;
                        grayClass[ri].Rect =
                            ImageProcessing.GetStringPolygonContour(polygonContour);
                    }
                    else if (shape is RectangleContour) // 矩形
                    {
                        var rectangleContour = (RectangleContour)shape;

                        grayClass[ri].Type = typeof(RectangleContour).Name;
                        grayClass[ri].Rect =
                            ImageProcessing.GetStringRect(rectangleContour);
                    }
                    else if (shape is RotatedRectangleContour) // 矩形
                    {
                        var rotatedRectangleContour = (RotatedRectangleContour)shape;

                        grayClass[ri].Type = typeof(RotatedRectangleContour).Name;
                        grayClass[ri].Rect =
                            ImageProcessing.GetStringRotatedRect(rotatedRectangleContour);
                    }

                    var rowIndex = -1;
                    var colIndex = -1;
                    var index = -1;

                    for (var i = 0; i < device.MatBuffer.GetLength(0); i++)
                    {
                        for (var j = 0; j < device.MatBuffer.GetLength(1); j++)
                        {
                            if (device.MatBuffer[i, j] != null && !string.IsNullOrEmpty(device.MatBuffer[i, j]))
                            {
                                index++;
                                if (index == allOnIndex)
                                {
                                    rowIndex = i;
                                    colIndex = j;
                                    break;
                                }
                            }
                        }
                    }

                    if (index == -1 || rowIndex == -1 || colIndex == -1)
                    {
                        this.ShowErrorTip("未找到图片");
                        return;
                    }

                    var rect = ImageProcessing.GetRectByString(grayClass[ri].Rect);

                    var originRect = new Rect((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
                    var matWidth = device.MatWidth[rowIndex, colIndex];
                    var matHeight = device.MatHeight[rowIndex, colIndex];
                    //var roiMat = new Mat(device.MatBuffer[rowIndex, colIndex], CommonUtility.HikSdk.MyCamera.GetRectInMat(matWidth, matHeight, originRect));
                    var roiMat = new Mat(MyCamera.Base64StringToMat(device.MatBuffer[rowIndex, colIndex]), MyCamera.GetRectInMat(matWidth, matHeight, originRect));
                    var meanVal = Cv2.Mean(roiMat);

                    //roiMat.Dispose();
                    //GC.Collect();

                    var grayPer = Math.Round(meanVal.Val0, 1, MidpointRounding.AwayFromZero);
                    grayPer = CommonUtility.HikSdk.MyCamera.GetLxByRgb(meanVal);
                    grayClass[ri].Value = grayPer + "%";
                    grayClass[ri].Max = "100%";
                    grayClass[ri].Min = onOffThreshold + "%";

                    tempStandard.Add(new LedState { Gray = double.Parse(grayClass[ri].Value.TrimEnd('%')), Min = double.Parse(grayClass[ri].Min.TrimEnd('%')), Max = double.Parse(grayClass[ri].Max.TrimEnd('%')) });
                }
                var imgLedStates = new List<ImgLedState>();
                for (var i = 0; i < endIndex; i++)
                {
                    imgLedStates.Add(new ImgLedState { ImgIndex = i });

                    for (var j = 0; j < _mainImageViewer.Roi.Count; j++)
                        imgLedStates[i].LedState.Add(new LedState { Gray = -9999, Max = -9999, Min = -9999 });
                }
                var listStr = new List<string>();
                var groupIndex = 0;
                {
                    var index1 = groupIndex;

                    var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
                    Parallel.For((long)startIndex, endIndex, options, i =>
                    {
                        var g = grayClass;

                        var items = new ConcurrentBag<VisionConfigVisionFuncCameraAnalysisShapesShape>(g);
                        var imgIndex = i;

                        Parallel.ForEach(items, (shape, state, index) =>
                        {
                            var rect = ImageProcessing.GetRectByString(shape.Rect);
                            int mRow, mCol;
                            var originRect = new Rect((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
                            var matWidth = device.GetImageFromBuff((int)imgIndex, out mRow, out mCol).Width;
                            var matHeight = device.GetImageFromBuff((int)imgIndex, out mRow, out mCol).Height;
                            var roiMat = new Mat(device.GetImageFromBuff((int)imgIndex, out mRow, out mCol), CommonUtility.HikSdk.MyCamera.GetRectInMat(matWidth, matHeight, originRect));
                            var meanVal = Cv2.Mean(roiMat);

                            var grayPer = Math.Round(meanVal.Val0, 1, MidpointRounding.AwayFromZero);
                            //grayPer = Math.Round(grayPer / 255, 1) * 100;
                            grayPer = CommonUtility.HikSdk.MyCamera.GetLxByRgb(meanVal);

                            imgLedStates[(int)imgIndex].LedState[(int)index].Gray = grayPer;
                        });
                    });

                    //Parallel.ForEach(indexPairs, pair =>
                    //{
                    //    var mRow = pair.Key;
                    //    var mCol = pair.Value;

                    //    var imgIndex = mRow * device.MatBuffer.GetLength(1) + mCol;

                    //    if (imgIndex < endIndex)
                    //    {
                    //        var g = grayClass;

                    //        var items = new ConcurrentBag<VisionConfigVisionFuncCameraAnalysisShapesShape>(g);

                    //        Parallel.ForEach(items, (shape, state, index) =>
                    //        {
                    //            var rect = ImageProcessing.GetRectByString(shape.Rect);
                    //            var roiMat = new Mat(device.MatBuffer[mRow, mCol], new Rect((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height));
                    //            var meanVal = Cv2.Mean(roiMat);

                    //            var grayPer = Math.Round(meanVal.Val0, 1, MidpointRounding.AwayFromZero);
                    //            //grayPer = Math.Round(grayPer / 255, 1) * 100;

                    //            imgLedStates[imgIndex].LedState[(int)index].Gray = grayPer;
                    //        });
                    //    }
                    //});
                }

                dgvData.AddColumn("ImgIndex", "ImgIndex");
                for (var i = 0; i < tempStandard.Count; i++)
                    dgvData.AddColumn("Roi" + i, "Roi" + i);

                for (var i = 0; i < imgLedStates.Count; i++)
                {
                    var objs = new object[tempStandard.Count + 1];
                    objs[0] = i;

                    for (var j = 0; j < imgLedStates[i].LedState.Count; j++)
                        objs[j + 1] =
                            string.Format(
                                "{0}/{1}={2}", imgLedStates[i].LedState[j].Gray, tempStandard[j].Gray, Math.Round(
                                    imgLedStates[i].LedState[j].Gray / tempStandard[j].Gray, 1,
                                    MidpointRounding.AwayFromZero));//Math.Round(imgLedStates[i].LedState[j].Gray / tempStandard[j].Gray, 1, MidpointRounding.AwayFromZero);

                    dgvData.AddRow(objs);
                }

                foreach (var t in imgLedStates)
                {
                    var str = string.Empty;

                    var isPass = false;
                    for (var i = 0; i < t.LedState.Count; i++)
                    {
                        var thisGray = t.LedState[i].Gray;
                        var standardGray = tempStandard[i].Gray;
                        var unif = thisGray / standardGray;

                        if (unif <= 0.15)
                        {
                            str += "0";
                        }
                        else if (unif > 0.15 && unif <= onOffThreshold)
                        {
                            isPass = true;
                            continue;
                        }
                        else
                        {
                            str += "1";
                        }
                    }

                    if (str.Length == t.LedState.Count)
                    {
                        if (!isPass)
                        {
                            if (!listStr.Any())
                            {
                                cmbGroup.Items.Add(t.ImgIndex);

                                listStr.Add(string.Format("第{0}张:{1}", t.ImgIndex.ToString().PadLeft(4, '0'), str));

                                var tempGrayClass =
                                    new VisionConfigVisionFuncCameraAnalysisShapesShape[grayClass.Length];
                                for (var i = 0; i < t.LedState.Count; i++)
                                {
                                    tempGrayClass[i] = new VisionConfigVisionFuncCameraAnalysisShapesShape
                                    {
                                        Rect = grayClass[i].Rect,
                                        Type = grayClass[i].Type,
                                        Min = onOffThreshold + "%",
                                        Max = str[i].ToString() == "0" ? "0%" : "100%",
                                        Value = tempStandard[i].Gray + "%"
                                    };
                                }

                                _toSave.Add(tempGrayClass);
                            }
                            else
                            {
                                if (listStr[listStr.Count - 1].Split(':')[1] != str)
                                {
                                    cmbGroup.Items.Add(t.ImgIndex);
                                    listStr.Add(string.Format("第{0}张:{1}", t.ImgIndex.ToString().PadLeft(4, '0'), str));

                                    var tempGrayClass =
                                        new VisionConfigVisionFuncCameraAnalysisShapesShape[grayClass.Length];
                                    for (var i = 0; i < t.LedState.Count; i++)
                                    {
                                        tempGrayClass[i] = new VisionConfigVisionFuncCameraAnalysisShapesShape
                                        {
                                            Rect = grayClass[i].Rect,
                                            Type = grayClass[i].Type,
                                            Min = onOffThreshold + "%",
                                            Max = str[i].ToString() == "0" ? "0%" : "100%",
                                            Value = tempStandard[i].Gray + "%"
                                        };
                                    }

                                    _toSave.Add(tempGrayClass);
                                }
                            }
                        }
                    }
                }

                foreach (var t in listStr)
                    txtLearnView.AppendText(t + "\r\n");

                st.Stop();
                this.ShowSuccessTip(string.Format(@"学习结束，耗时：{0}", st.ElapsedMilliseconds));

                Enabled = true;
            }

            #endregion
        }

        private void btnLightAndSnap_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbCameras.Text) && VisionCommon.NiImaqd != null)
            {
                var st = new Stopwatch();
                st.Start();

                var cameraStartOn = (long)0;
                var cameraEndOn = (long)0;
                var ledStartOn = (long)0;
                var ledEndOn = (long)0;

                cmbGroup.Items.Clear();
                toolStripCmbImgList.Items.Clear();
                dgvData.ClearAll();

                var device = VisionCommon.NiImaqd.CameraList.Find(f => f.GigeInfo.chSerialNumber == cmbCameras.Text);
                if (device != null)
                {
                    var allOther =
                        VisionCommon.NiImaqd.CameraList.FindAll(
                            f => f.GigeInfo.chSerialNumber != device.GigeInfo.chSerialNumber);
                    foreach (var other in allOther)
                        other.CloseCamera();

                    ImgCount = txtFrameCount.Value;
                    device.SetExposureTime(txtShutter.Value);
                    Thread.Sleep(100);

                    var perMs = 8;
                    if (txtShutter.Value / 1000 >= 8)
                        perMs = txtShutter.Value / 1000;
                    var timeOut = perMs * (uint)ImgCount + 5000;

                    var t = VisionCommon.VisionConfig.ParaInfo.ToList().Find(f => f.ParaName == Func.VisionFuncName);
                    if (t != null)
                    {
                        var thisStepPowerPara = string.IsNullOrEmpty(t.PowerPara)
                               ? string.Format("电压1={0}V，电流1={1}A，电压2={2}V，电流2={3}A，电压3={4}V，电流3={5}A，串并联模式={6}", 13.5,
                                   10, 0, 0, 5, 1, "无")
                               : t.PowerPara;

                        VisionCommon.PowerSet(thisStepPowerPara, true);
                    }

                    var temp = IsLeft ? Func.VisionFuncDetailL[0] : Func.VisionFuncDetailR[0];

                    //if (temp.SequenceType == "先点亮后抓拍")
                    if (cmbSequenceType.SelectedIndex == 0)
                    {
                        var t1End = false;
                        var t2End = false;

                        var cameraTask = Task.Factory.StartNew(() =>
                        {
                            var stThis = new Stopwatch();
                            stThis.Start();
                            while (true)
                            {
                                if (stThis.ElapsedMilliseconds >= txtSequenceDelay.Value)
                                    break;
                                Thread.Sleep(1);
                            }

                            stThis.Stop();

                            //Task.Delay(txtSequenceDelay.Value).Wait();
                            //Thread.Sleep(txtSequenceDelay.Value);
                            cameraStartOn = st.ElapsedMilliseconds;
                            device.Capture((uint)ImgCount, (int)timeOut);

                            t1End = true;
                        });

                        var ledTask = Task.Factory.StartNew(() =>
                        {
                            ledStartOn = st.ElapsedMilliseconds;
                            if (t != null)
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

                                if (t.ParaMethods != null)
                                    VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsBefore);
                            }
                            ledEndOn = st.ElapsedMilliseconds;

                            t2End = true;
                        });

                        // wait....,,
                        while (true)
                        {
                            if (t1End && t2End)
                                break;

                            Thread.Sleep(1);
                        }
                    }
                    //else if (temp.SequenceType == "先抓拍后点亮")
                    else if (cmbSequenceType.SelectedIndex == 1)
                    {
                        var t1End = false;
                        var t2End = false;

                        var cameraTask = Task.Factory.StartNew(() =>
                        {
                            cameraStartOn = st.ElapsedMilliseconds;
                            device.Capture((uint)ImgCount, (int)timeOut);
                            cameraEndOn = st.ElapsedMilliseconds;

                            t1End = true;
                        });

                        var ledTask = Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(txtSequenceDelay.Value);
                            ledStartOn = st.ElapsedMilliseconds;
                            if (t != null)
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

                                if (t.ParaMethods != null)
                                    VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsBefore);
                            }
                            ledEndOn = st.ElapsedMilliseconds;

                            t2End = true;
                        });

                        // wait....,,
                        while (true)
                        {
                            if (t1End && t2End)
                                break;

                            Thread.Sleep(1);
                        }
                    }

                    for (var i = 0; i < ImgCount; i++)
                        toolStripCmbImgList.Items.Add(i);


                    //var visionImg = CommonUtility.HikSdk.MyCamera.MatToVisionImage(device.MatBuffer[0, 0]);
                    int mRow, mCol;
                    var visionImg = CommonUtility.HikSdk.MyCamera.MatToVisionImage(device.GetImageFromBuff(0, out mRow, out mCol));
                    Algorithms.Copy(visionImg, _mainImageViewer.Image);
                    visionImg.Dispose();
                    GC.Collect();

                    if (t != null)
                    {
                        VisionCommon.InvokeRelays(t.ParaReleysList, true);

                        if (t.ParaMethods != null)
                            VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsAfter);
                    }

                    foreach (var other in allOther)
                        other.OpenCamera();
                }

                st.Stop();
                this.ShowSuccessDialog(
                    string.Format("点亮并抓拍结束，总耗时:{0}ms；\r\n相机在第{1}ms触发，在第{2}ms结束；\r\n动画在第{3}ms触发，在第{4}ms结束",
                        st.ElapsedMilliseconds.ToString(),
                        cameraStartOn, cameraEndOn, ledStartOn, ledEndOn));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ShowAskDialog("确定保存？"))
            {
                var detail = new VisionConfigVisionFuncCamera
                {
                    Shutter = txtShutter.Value.ToString(),
                    UserId = cmbCameras.Text,
                    FrameCount = txtFrameCount.Value.ToString(),
                    SequenceType = cmbSequenceType.Items[cmbSequenceType.SelectedIndex].ToString(),
                    SequenceDelay = txtSequenceDelay.Value.ToString(),
                    Analysis = new VisionConfigVisionFuncCameraAnalysis
                    {
                        CaliRegion = new string[0],
                        LookupTable = CommonUtility.ImageProcessing.LookupTableType.Exponential.ToString(),
                        ShapesGroups = _toSave.ToArray()
                        //ShapesGroups = new VisionConfigVisionFuncCameraAnalysisShapesShape[1][]
                    }
                };

                if (IsLeft)
                    Func.VisionFuncDetailL[0] = detail;
                else
                    Func.VisionFuncDetailR[0] = detail;

                var fucIndex =
                       VisionCommon.VisionConfig.VisionInfo.ToList()
                           .FindIndex(f => f.VisionFuncName == Func.VisionFuncName);
                if (fucIndex == -1)
                {
                    var temp = VisionCommon.VisionConfig.VisionInfo.ToList();
                    temp.Add(Func);
                    VisionCommon.VisionConfig.VisionInfo = temp.ToArray();
                }
                else
                {
                    VisionCommon.VisionConfig.VisionInfo[fucIndex] = Func;
                }

                VisionCommon.VisionConfig.DeviceInfo.DeviceUpdateTime =
                    DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                XmlHelper.SerializeToFile(VisionCommon.VisionConfig, VisionCommon.FilePath, Encoding.UTF8);
            }
        }

        private void toolStripCmbImgList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var device = VisionCommon.NiImaqd.CameraList.Find(f => f.GigeInfo.chSerialNumber == cmbCameras.Text);

            if (device != null && toolStripCmbImgList.SelectedIndex >= 0)
            {

                var imgIndex = toolStripCmbImgList.SelectedIndex;
                int mRow, mCol;
                var visionImg = CommonUtility.HikSdk.MyCamera.MatToVisionImage(device.GetImageFromBuff(imgIndex, out mRow, out mCol));
                Algorithms.Copy(visionImg, _mainImageViewer.Image);
                visionImg.Dispose();
            }
        }

        private class ImgLedState
        {
            public readonly List<LedState> LedState = new List<LedState>();
            public int ImgIndex;
        }

        private class LedState
        {
            public double Gray;
            public double Max;
            public double Min;
        }

        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGroup.Items.Count > 0 && cmbGroup.SelectedIndex >= 0)
            {
                var value = int.Parse(cmbGroup.Items[cmbGroup.SelectedIndex].ToString());

                if (toolStripCmbImgList.Items.Count > 0 && toolStripCmbImgList.Items.Count > value)
                {
                    toolStripCmbImgList.SelectedIndex = value;
                }
            }

        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (toolStripCmbImgList.Items.Count <= 0)
            {
                this.ShowErrorTip("请先采集图像");
                return;
            }

            var device = VisionCommon.NiImaqd.CameraList.Find(f => f.GigeInfo.chSerialNumber == cmbCameras.Text);
            if (device == null)
            {
                this.ShowErrorTip("未找到相机");
                return;
            }

            // 选择路径
            string path;
            using (var folder = new FolderBrowserDialog())
            {
                var result = folder.ShowDialog();
                if (result != DialogResult.OK)
                    return;
                path = folder.SelectedPath + string.Format(@"\{0}_Img_{1}_{2}_shutter_{3}",
                    DateTime.Now.ToString(CultureInfo.InvariantCulture)
                        .Replace(":", string.Empty)
                        .Replace("/", string.Empty)
                        .Replace(" ", string.Empty),
                    Func.VisionFuncName,
                    Guid.NewGuid().ToString(), txtShutter.Value);

                Directory.CreateDirectory(path);

                var saveCount = 0;
                for (var j = 0; j < device.MatBuffer.GetLength(0); j++)
                {
                    for (var k = 0; k < device.MatBuffer.GetLength(1); k++)
                    {
                        if (device.MatBuffer[j, k] == null || !string.IsNullOrEmpty(device.MatBuffer[j, k]))
                            continue;

                        MyCamera.Base64StringToMat(device.MatBuffer[j, k]).ImWrite(string.Format(@"{0}\image{1}_{2}.jpg", path,
                             saveCount.ToString().PadLeft(5, '0'), cmbCameras.Text));
                        saveCount++;
                    }
                }

                this.ShowSuccessTip(string.Format("保存成功\r\n{0}", path));
            }
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvData.RowCount <= 0)
            {
                this.ShowErrorTip("没有数据无法导出");
                return;
            }

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
                    this.ShowSuccessTip(@"数据已成功导出到 Excel 文件中。");
                }
                catch (Exception ex)
                {
                    this.ShowErrorTip(@"导出过程中发生错误：" + ex.Message);
                }
            }
        }
    }
}
