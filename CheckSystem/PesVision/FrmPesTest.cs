using CommonUtility.HikSdk;
using Controller;
using MathNet.Numerics;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using OpenCvSharp;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Control = System.Windows.Forms.Control;
using Point = OpenCvSharp.Point;
using Point2d = OpenCvSharp.Point2d;
using Size = OpenCvSharp.Size;

namespace CheckSystem.PesVision
{
    public partial class FrmPesTest : UIForm
    {
        private readonly ImageViewer _mainImageViewer = new ImageViewer { Dock = DockStyle.Fill };
        private readonly HeadlampAnalysisByGb4599 _headlampAnalysisByGb2591 = new HeadlampAnalysisByGb4599("前照灯光型检测GB4599");
        //private readonly HeadlampAnalysisByGb4599 _headlampAnalysisByGb4599 = new HeadlampAnalysisByGb4599("前照灯光型检测GB4599");

        public FrmPesTest()
        {
            InitializeComponent();
            ImagePanel.Controls.Add(_mainImageViewer);
            _hikcamera.DeviceListAcq();
            Load += FrmPesTest_Load;
        }

        private void FrmPesTest_Load(object sender, EventArgs e)
        {
            _x = Width;//获取窗体的宽度
            _y = Height;//获取窗体的高度
            SetTag(funcPanel);//调用方法
            Resize += FrmPesTest_Resize;
            cmbImageList.SelectedIndexChanged += CmbImageList_SelectedIndexChanged;
            BindDictionaryToComboBox();
            InitImageViewer(_mainImageViewer);
            btnCaptureImage.Click += BtnCaptureImage_Click;
            btnReadImage.Click += BtnReadImage_Click;
            btnClearAllImage.Click += BtnClearAllImage_Click;
            btnSetRoi.Click += BtnSetRoi_Click;
            btnDeNoise.Click += BtnDeNoise_Click;
            btnFindCutOffLine.Click += BtnFindCutOffLine_Click;
            btnAnalysis.Click += BtnAnalysis_Click;
            btnAnalysisHb.Click += BtnAnalysisHb_Click;

            WindowState = FormWindowState.Maximized;
        }

        private void CmbImageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbImageList.SelectedItem == null)
                return;

            // 获取选中的键和值
            var selectedItem = (KeyValuePair<string, Mat>)cmbImageList.SelectedItem;
            var key = selectedItem.Key;
            var value = selectedItem.Value;

            var visionImg = MyCamera.MatToVisionImage(value);
            Algorithms.Copy(visionImg, _mainImageViewer.Image);
            visionImg.Dispose();
            GC.Collect();
        }

        private void BindDictionaryToComboBox()
        {
            if (!_headlampAnalysisByGb2591.Images.Any())
            {
                // 清除当前绑定数据
                cmbImageList.DataSource = null;
                cmbImageList.Items.Clear();

                var emptyImage = new VisionImage();
                Algorithms.Copy(emptyImage, _mainImageViewer.Image);
                emptyImage.Dispose();
                return;
            }

            cmbImageList.DataSource = new BindingSource(_headlampAnalysisByGb2591.Images, null);
            cmbImageList.DisplayMember = "Key"; // 显示的文本
            cmbImageList.ValueMember = "Key"; // 实际绑定的值

            cmbImageList.SelectedIndex = cmbImageList.Items.Count - 1;
        }

        #region func 控件缩放

        private float _x;//当前窗体的宽度
        private float _y;//当前窗体的高度

        private void FrmPesTest_Resize(object sender, EventArgs e)
        {
            var newX = Width / _x; //窗体宽度缩放比例
            var newY = Height / _y;//窗体高度缩放比例
            SetControls(newX, newY, funcPanel);//随窗体改变控件大小
        }

        /// <summary>
        /// 将控件的宽，高，左边距，顶边距和字体大小暂存到tag属性中
        /// </summary>
        /// <param name="cons">递归控件中的控件</param>
        private static void SetTag(Control cons)
        {
            foreach (Control con in cons.Controls)//循环窗体中的控件
            {
                //if (con.GetType() != typeof(UIButton))
                {
                    con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                    if (con.Controls.Count > 0)
                        SetTag(con);
                }
            }
        }

        /// <summary>
        /// 根据窗体大小调整控件大小
        /// </summary>
        /// <param name="newx">窗体宽度缩放比例</param>
        /// <param name="newy">窗体高度缩放比例</param>
        /// <param name="cons">随窗体改变控件大小</param>
        private static void SetControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //if (con.GetType() != typeof(UIButton))
                {
                    var mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                    var a = Convert.ToSingle(mytag[0]) * newx;//根据窗体缩放比例确定控件的值，宽度
                    con.Width = (int)a;//宽度
                    a = Convert.ToSingle(mytag[1]) * newy;//高度
                    con.Height = (int)(a);
                    a = Convert.ToSingle(mytag[2]) * newx;//左边距离
                    con.Left = (int)(a);
                    a = Convert.ToSingle(mytag[3]) * newy;//上边缘距离
                    con.Top = (int)(a);
                    var currentSize = Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                        SetControls(newx, newy, con);
                }
            }
        }

        #endregion

        #region ImageViewer

        private void InitImageViewer(ImageViewer imageViewer)
        {
            ImageShowTool(imageViewer);
            imageViewer.SizeChanged += ImageViewer_SizeChanged;
            //imageViewer.RoiChanged += imageViewer_RoiChanged;
        }

        private void ImageViewer_SizeChanged(object sender, EventArgs e)
        {
            var imageViewer = sender as ImageViewer;
            if (imageViewer != null)
                ImageShowTool(imageViewer);
        }

        private static void ImageShowTool(ImageViewer imageViewer)
        {
            imageViewer.ToolsShown = ViewerTools.ZoomIn |
                                     ViewerTools.ZoomOut |
                                     ViewerTools.Pan |
                                     ViewerTools.Selection;
            //ViewerTools.Rectangle;
            //ViewerTools.All;
            imageViewer.ActiveTool = ViewerTools.Selection;
            imageViewer.ZoomToFit = true;
            imageViewer.ShowToolbar = true;
            imageViewer.ShowScrollbars = true;
            imageViewer.ShowImageInfo = true;

            //imageViewer.AutoDelete = false;
        }

        #endregion

        #region Tool Stript
        private void BtnCaptureImage_Click(object sender, EventArgs e)
        {
        }

        private void BtnReadImage_Click(object sender, EventArgs e)
        {
            //toolStripButton1.Text = string.Empty;

            var openFi = new OpenFileDialog
            {
                Site = null,
                Tag = null,
                AddExtension = false,
                CheckPathExists = false,
                DefaultExt = null,
                DereferenceLinks = false,
                FileName = null,
                Filter = null,
                FilterIndex = 0,
                InitialDirectory = null,
                RestoreDirectory = false,
                ShowHelp = false,
                SupportMultiDottedExtensions = false,
                Title = null,
                ValidateNames = false,
                AutoUpgradeEnabled = false,
                CheckFileExists = false,
                Multiselect = false,
                ReadOnlyChecked = false,
                ShowReadOnly = false
            };
            openFi.Filter = "图像文件(JPeg, Gif, Bmp, etc.)|*.jpg;*.jpeg;*.gif;*.bmp;*.tif; *.tiff; *.png| JPeg 图像文件(*.jpg;*.jpeg)"
                            + "|*.jpg;*.jpeg |GIF 图像文件(*.gif)|*.gif |BMP图像文件(*.bmp)|*.bmp|Tiff图像文件(*.tif;*.tiff)|*.tif;*.tiff|Png图像文件(*.png)"
                            + "| *.png |所有文件(*.*)|*.*";
            if (openFi.ShowDialog() == DialogResult.OK)
            {
                btnClearAllImage.PerformClick();
                _headlampAnalysisByGb2591.ReadImage("近光原图", openFi.FileName);
                toolStripButton1.Text = @"“" + openFi.FileName + @"“";
                BindDictionaryToComboBox();
            }
        }

        private void BtnClearAllImage_Click(object sender, EventArgs e)
        {
            _headlampAnalysisByGb2591.ClearAllImage();
            BindDictionaryToComboBox();
        }

        #endregion

        #region Func

        private void BtnSetRoi_Click(object sender, EventArgs e)
        {
            //if ((_headlampAnalysisByGb2591.Images.ContainsKey("近光原图") &&
            //     _headlampAnalysisByGb2591.Images["近光原图"] != null &&
            //     !_headlampAnalysisByGb2591.Images["近光原图"].Empty()))
            //{
            //    var uiFrm = new UIForm { Text = @"SET ROI" };
            //    var imageViewer = new ImageViewer { Dock = DockStyle.Fill };
            //    var btn = new UIButton { Text = @"确定", Dock = DockStyle.Bottom };
            //    uiFrm.Controls.Add(imageViewer);
            //    uiFrm.Controls.Add(btn);

            //    EventHandler btnHandler = ((btnSender, btnE) =>
            //    {
            //        if (imageViewer.Roi.Count == 0)
            //        {
            //            ShowErrorTip("请先设置ROI");
            //        }
            //        else
            //        {
            //            uiFrm.DialogResult = DialogResult.OK;
            //        }
            //    });
            //    btn.Click += btnHandler;

            //    ImageShowTool(imageViewer);
            //    EventHandler<ContoursChangedEventArgs> roiChangedHandle = (imageViewerSender, imageViewerE) =>
            //    {
            //        if (imageViewer.Roi.Count == 0)
            //            imageViewer.Roi.Add(new RectangleContour(1, 1, imageViewer.Image.Width - 2, imageViewer.Image.Height - 2));
            //    };
            //    imageViewer.SizeChanged += ImageViewer_SizeChanged;
            //    imageViewer.RoiChanged += roiChangedHandle;

            //    var visionImg = MyCamera.MatToVisionImage(_headlampAnalysisByGb2591.Images["近光原图"]);
            //    Algorithms.Copy(visionImg, imageViewer.Image);
            //    visionImg.Dispose();
            //    GC.Collect();
            //    imageViewer.Roi.Add(new RectangleContour(0, 0, imageViewer.Image.Width, imageViewer.Image.Height));

            //    var result = uiFrm.ShowDialog();
            //    if (result == DialogResult.OK)
            //    {
            //        var shape = imageViewer.Roi[0].Shape as RectangleContour;

            //        if (shape != null)
            //        {
            //            var left = (int)shape.Left;
            //            var top = (int)shape.Top;
            //            var width = (int)shape.Width;
            //            var height = (int)shape.Height;

            //            _headlampAnalysisByGb2591.LowBeamRoi("近光原图", top, left, width, height, "近光提取ROI");
            //            BindDictionaryToComboBox();
            //        }
            //    }
            //    imageViewer.SizeChanged -= ImageViewer_SizeChanged;
            //    imageViewer.RoiChanged -= roiChangedHandle;
            //    btn.Click -= btnHandler;
            //}
            //else
            //{
            //    ShowErrorTip("请先加载图片");
            //}

            //var option = new UIEditOption { AutoLabelWidth = true };
            //option.AddInteger("Top", "TopLeft", 200);
            //option.AddInteger("Left", "Left", 1000);
            //option.AddInteger("Width", "Width", 2200);
            //option.AddInteger("Height", "Height", 1000);

            //var frm = new UIEditForm(option);
            //frm.Render();
            //frm.ShowDialog();

            //if (frm.IsOK)
            //{
            //    var top = (int)frm["Top"];
            //    var left = (int)frm["Left"];
            //    var width = (int)frm["Width"];
            //    var height = (int)frm["Height"];

            //    _headlampAnalysisByGb2591.LowBeamRoi("近光原图", left, top, width, height, "近光提取ROI");
            //    BindDictionaryToComboBox();
            //}
        }

        private void BtnDeNoise_Click(object sender, EventArgs e)
        {
            //_headlampAnalysisByGb2591.LowBeamDeNoise("近光提取ROI", "近光降噪预处理");
            //BindDictionaryToComboBox();
        }

        private void BtnFindCutOffLine_Click(object sender, EventArgs e)
        {
            //_headlampAnalysisByGb2591.FindCutOffLine("近光降噪预处理", "寻找截止线");
            //BindDictionaryToComboBox();
        }

        private void BtnAnalysis_Click(object sender, EventArgs e)
        {
            if ((_headlampAnalysisByGb2591.Images.ContainsKey("近光原图") &&
                 _headlampAnalysisByGb2591.Images["近光原图"] != null &&
                 !_headlampAnalysisByGb2591.Images["近光原图"].Empty()))
            {
                var uiOption = new UIEditOption();
                var type = new string[] { "557", "NDLB" };
                uiOption.AddCombobox("type", "类型", type, 0);

                var frm = new UIEditForm(uiOption);
                frm.Render();
                frm.ShowDialog();

                if (frm.IsOK)
                {
                    var typeIndex = (int)frm["type"];

                    if (typeIndex == 0)
                    {
                        // 557参数
                        {
                            _headlampAnalysisByGb2591.Ratio = 8000;
                            _headlampAnalysisByGb2591.PixelScale = 2.4;
                            _headlampAnalysisByGb2591.CameraDistanceCm = 37;
                            _headlampAnalysisByGb2591.FresnelLensDistanceCm = 55;

                            _headlampAnalysisByGb2591.ScanStartX = 1010;
                            _headlampAnalysisByGb2591.ScanEndX = 1500;

                            _headlampAnalysisByGb2591.ScanDegreeH = 10;
                            _headlampAnalysisByGb2591.ScanDegreeV = 5;
                        }
                    }
                    else if (typeIndex == 1)
                    {
                        // NDLB参数
                        {
                            _headlampAnalysisByGb2591.Ratio = 8000;
                            _headlampAnalysisByGb2591.PixelScale = 3.45;
                            _headlampAnalysisByGb2591.CameraDistanceCm = 35;
                            _headlampAnalysisByGb2591.FresnelLensDistanceCm = 47;
                            _headlampAnalysisByGb2591.ScanStartX = 1940;
                            _headlampAnalysisByGb2591.ScanEndX = 2270;
                            _headlampAnalysisByGb2591.ScanDegreeH = 10;
                            _headlampAnalysisByGb2591.ScanDegreeV = 5;
                        }
                    }
                    _headlampAnalysisByGb2591.AnalyzeImage("近光原图");
                    BindDictionaryToComboBox();
                }
            }
        }

        private void BtnAnalysisHb_Click(object sender, EventArgs e)
        {
            _headlampAnalysisByGb2591.Ratio = 8000;
            _headlampAnalysisByGb2591.PixelScale = 2.4;
            _headlampAnalysisByGb2591.CameraDistanceCm = 37;
            _headlampAnalysisByGb2591.FresnelLensDistanceCm = 55;

            _headlampAnalysisByGb2591.ScanStartX = 1010;
            _headlampAnalysisByGb2591.ScanEndX = 1500;

            _headlampAnalysisByGb2591.ScanDegreeH = 10;
            _headlampAnalysisByGb2591.ScanDegreeV = 5;

            _headlampAnalysisByGb2591.AnalyzeHbAlb("远光原图", "ALB原图");
            BindDictionaryToComboBox();
        }

        #endregion

        private void 保存当前图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_mainImageViewer.Image != null)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    var result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        var path = Path.Combine(fbd.SelectedPath, string.Format("export_on_{0}_{1}.bmp",
                            DateTime.Now.ToString("yyyyMMdd-hhmmss"), Guid.NewGuid().ToString().Substring(24, 12)));
                        _mainImageViewer.Image.WriteBmpFile(path);
                        ShowSuccessTip("导出成功：" + path);
                    }
                }
            }
            else
            {
                ShowErrorTip("图像为空无法保存");
            }
        }

        private readonly CameraControl _hikcamera = new CameraControl();

        private void btnCaptureLb_Click(object sender, EventArgs e)
        {
            if (_hikcamera.CameraList.Any())
            {
                var option = new UIEditOption { AutoLabelWidth = true };
                var str = new string[] { "#1", "#2", "#3", "#4", "#5", "#6", "#7", "#8", "#9", "#10" };
                option.AddCombobox("index", "index", str, 0);

                var frm = new UIEditForm(option);
                frm.Render();
                frm.ShowDialog();

                if (frm.IsOK)
                {
                    var index = (int)frm["index"];

                    var folder = string.Format(@"E:\MVS_SAVE_DATA\{0}\LB\", str[index]);

                    var listTime = new List<int> { 4500, 7000, 25000, 50000 };

                    var c = _hikcamera.CameraList[0];

                    foreach (var time in listTime)
                    {
                        c.OpenCamera();
                        c.SetExposureTime(time);
                        if (c.Capture(1))
                        {
                            var mat = c.GetImageFromBuff(0, out _, out _);
                            mat.ImWrite(folder + string.Format("LB_{0}_{1}.bmp", str[index], time));
                        }
                        c.ClearBuffer();
                    }

                    c.CloseCamera();

                    ShowSuccessTip("保存成功");
                }
                else
                {
                    ShowInfoTip("取消保存");
                }
            }
            else
            {
                ShowErrorTip("相机未连接");
            }
        }

        private void btnCaptureHb_Click(object sender, EventArgs e)
        {
            if (_hikcamera.CameraList.Any())
            {
                var option = new UIEditOption { AutoLabelWidth = true };
                var str = new string[] { "#1", "#2", "#3", "#4", "#5", "#6", "#7", "#8", "#9", "#10" };
                option.AddCombobox("index", "index", str, 0);

                var frm = new UIEditForm(option);
                frm.Render();
                frm.ShowDialog();

                if (frm.IsOK)
                {
                    var index = (int)frm["index"];

                    var folder = string.Format(@"E:\MVS_SAVE_DATA\{0}\HB\", str[index]);

                    var listTime = new List<int> { 2000, 2500, 3000, 3500, 4000, 4500 };

                    var c = _hikcamera.CameraList[0];

                    foreach (var time in listTime)
                    {
                        c.OpenCamera();
                        c.SetExposureTime(time);
                        if (c.Capture(1))
                        {
                            var mat = c.GetImageFromBuff(0, out _, out _);
                            mat.ImWrite(folder + string.Format("HB_{0}_{1}.bmp", str[index], time));
                        }
                        c.ClearBuffer();
                    }

                    c.CloseCamera();

                    ShowSuccessTip("保存成功");
                }
                else
                {
                    ShowInfoTip("取消保存");
                }
            }
            else
            {
                ShowErrorTip("相机未连接");
            }
        }

        private void uiButton14_Click(object sender, EventArgs e)
        {
            _headlampAnalysisByGb2591.ReadImage("近光原图", @"E:\Projects\557_PES\图像-20250213\近光\export_on_20250214-024823_c6815530c3b7.bmp");
            //_headlampAnalysisByGb2591.ReadImage("近光原图", @"E:\Projects\557_PES\图像-20250213\近光\export_on_20250217-100747_c5932172a291.bmp");

            using (var srcMat = _headlampAnalysisByGb2591.Images["近光原图"].Clone())
            {
                using (var dstMat = srcMat.Clone())
                {
                    if (dstMat.Channels() == 1)
                        Cv2.CvtColor(dstMat, dstMat, ColorConversionCodes.GRAY2BGR);

                    using (var grayMat = srcMat.Clone())
                    {
                        if (grayMat.Channels() == 3)
                            Cv2.CvtColor(grayMat, grayMat, ColorConversionCodes.BGR2GRAY);
                        _headlampAnalysisByGb2591.AppendImage("近光灰度图", grayMat);

                        // 3. 获取极值
                        double minVal, maxVal;
                        Point minLoc, maxLoc;
                        Cv2.MinMaxLoc(grayMat, out minVal, out maxVal, out minLoc, out maxLoc);
                        Cv2.Circle(dstMat, maxLoc, 1, Scalar.Red, -1);

                        DrawContours(dstMat, grayMat, maxVal * 0.9f, Scalar.Red);
                        DrawContours(dstMat, grayMat, maxVal * 0.8f, Scalar.DarkBlue);
                        DrawContours(dstMat, grayMat, maxVal * 0.7f, Scalar.Yellow);
                        DrawContours(dstMat, grayMat, maxVal * 0.6f, Scalar.Green);
                        DrawContours(dstMat, grayMat, maxVal * 0.5f, Scalar.DeepSkyBlue);
                        DrawContours(dstMat, grayMat, maxVal * 0.4f, Scalar.DeepPink);
                        DrawContours(dstMat, grayMat, maxVal * 0.3f, Scalar.IndianRed);
                        DrawContours(dstMat, grayMat, maxVal * 0.2f, Scalar.WhiteSmoke);
                        DrawContours(dstMat, grayMat, maxVal * 0.1f, Scalar.Blue);

                        using (var edgeTestMat = grayMat.Clone())
                        {
                            var clahe = Cv2.CreateCLAHE(clipLimit: 2.0, tileGridSize: new Size(8, 8));
                            clahe.Apply(edgeTestMat, edgeTestMat);
                            Cv2.GaussianBlur(edgeTestMat, edgeTestMat, new Size(5, 5), sigmaX: 1.5);
                            Cv2.Threshold(edgeTestMat, edgeTestMat, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
                            // 形态学闭操作（连接断裂边缘）
                            var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(21, 3));
                            Cv2.MorphologyEx(edgeTestMat, edgeTestMat, MorphTypes.Close, kernel);
                            Cv2.Canny(edgeTestMat, edgeTestMat, 50, 150);

                            //var scanStartX = 1250 - 325;
                            //var scanEndX = 1250 + 495;
                            //var scanStartX = maxLoc.X - 325;
                            //var scanEndX = maxLoc.X + 495;

                            var scanStartX = maxLoc.X - (297+24);
                            var scanEndX = maxLoc.X + (520 -24);
                            var listEdgeLinePoints = new List<Point>();

                            for (var x = scanStartX; x <= scanEndX; x += 1)
                            //for (var x = scanStartX; x <= scanEndX; x++)
                            {
                                for (var y = 0; y < edgeTestMat.Height; y++)
                                {
                                    var pixelValue = edgeTestMat.At<byte>(y, x);
                                    if (pixelValue != 255)
                                        continue;

                                    listEdgeLinePoints.Add(new Point(x, y));
                                    break;
                                }
                            }

                            using (var roiEdgeLine = new Mat(srcMat.Size(), MatType.CV_8UC1, Scalar.Black))
                            {
                                //var cleanedData = PolyHelper.PreprocessData(listEdgeLinePoints);
                                //listEdgeLinePoints.Clear();
                                //listEdgeLinePoints.AddRange(cleanedData);

                                Cv2.CvtColor(roiEdgeLine, roiEdgeLine, ColorConversionCodes.GRAY2BGR);
                                foreach (var p in listEdgeLinePoints)
                                    roiEdgeLine.Set(p.Y, p.X, new Vec3b(255, 255, 255));

                                var xMin = listEdgeLinePoints.Min(f => f.X);
                                var xMax = listEdgeLinePoints.Max(f => f.X);
                                var yMin = listEdgeLinePoints.Min(f => f.Y);
                                var yMax = listEdgeLinePoints.Max(f => f.Y);
                                var strMinMaxRange = string.Format("X:{0}-{1}, Y:{2}-{3}", xMin, xMax, yMin, yMax);

                                {
                                    using (var showFindPointsOnSrc = srcMat.Clone())
                                    {
                                        try
                                        {
                                            // debug
                                            {
                                                //var debugMat = srcMat.Clone();
                                                //if (debugMat.Channels() == 3)
                                                //    Cv2.CvtColor(debugMat, debugMat, ColorConversionCodes.BGR2GRAY);
                                                //// 高斯滤波
                                                //Cv2.GaussianBlur(debugMat, debugMat, new Size(5, 5), 1.5);

                                                //// Sobel边缘检测
                                                //var sobelX = new Mat();
                                                //var sobelY = new Mat();
                                                //Cv2.Sobel(debugMat, sobelX, MatType.CV_64F, 1, 0);
                                                //Cv2.Sobel(debugMat, sobelY, MatType.CV_64F, 0, 1);
                                                //var gradMagnitude = new Mat();
                                                //Cv2.Magnitude(sobelX, sobelY, gradMagnitude);


                                                //sobelX.ImWrite(@"E:\Projects\557_PES\图像-20250213\近光\sobelX.bmp");
                                                //sobelY.ImWrite(@"E:\Projects\557_PES\图像-20250213\近光\sobelY.bmp");
                                                //gradMagnitude.ImWrite(@"E:\Projects\557_PES\图像-20250213\近光\gradMagnitude.bmp");
                                                //// 提取边缘点集
                                                //var edgePoints = new List<Point2d>();
                                                ////for (int xxx = 0; xxx < sobelX.Cols; xxx++)
                                                ////{
                                                ////    // 查找每列最大梯度点
                                                ////    var maxY = Enumerable.Range(0, sobelX.Rows)
                                                ////        .Select(yyyy => new { Y = yyyy, G = sobelX.At<double>(yyyy, xxx) })
                                                ////        .OrderByDescending(p => Math.Abs(p.G))
                                                ////        .First().Y;

                                                ////    edgePoints.Add(new Point2d(xxx, maxY));
                                                ////}


                                                ////for (int xxx = 1; xxx < gradMagnitude.Cols - 1; xxx++)
                                                //for (int xxx = scanStartX; xxx <= scanEndX; xxx++)
                                                //{
                                                //    var colData = Enumerable.Range(0, gradMagnitude.Rows)
                                                //        .Select(yyyy => gradMagnitude.At<double>(yyyy, xxx)).ToArray();

                                                //    // 寻找局部极大值
                                                //    var peaks = new List<int>();
                                                //    for (int yyyy = 1; yyyy < colData.Length - 1; yyyy++)
                                                //    {
                                                //        if (colData[yyyy] > colData[yyyy - 1] &&
                                                //            colData[yyyy] > colData[yyyy + 1] &&
                                                //            colData[yyyy] > 30) // 梯度阈值
                                                //        {
                                                //            peaks.Add(yyyy);
                                                //        }
                                                //    }

                                                //    // 选择最强峰值
                                                //    if (peaks.Any())
                                                //    {
                                                //        var maxY = peaks.OrderByDescending(yyy => colData[yyy]).First();
                                                //        edgePoints.Add(new Point2d(xxx, maxY));
                                                //    }
                                                //}

                                                //// 三次样条插值
                                                //var xData = edgePoints.Select(p => (double)p.X).ToArray();
                                                //var yData = edgePoints.Select(p => (double)p.Y).ToArray();
                                                //var spline = CubicSpline.InterpolateAkimaSorted(xData, yData);

                                                //// 二阶导数计算
                                                //var derivatives = new List<double>();
                                                //double h = 1.0; // 采样步长
                                                //for (var i = 1; i < xData.Length - 1; i++)
                                                //{
                                                //    double d2 = (spline.Interpolate(xData[i + 1]) - 2 * spline.Interpolate(xData[i])
                                                //                 + spline.Interpolate(xData[i - 1])) / (h * h);
                                                //    derivatives.Add(d2);
                                                //}

                                                //// 拐点检测
                                                //double threshold = derivatives.Max(d => Math.Abs(d)) * 0.15;
                                                //var inflectionPoints = new List<Point2d>();

                                                //for (int i = 1; i < derivatives.Count - 1; i++)
                                                //{
                                                //    if (Math.Abs(derivatives[i]) > threshold &&
                                                //        derivatives[i] * derivatives[i + 1] < 0)
                                                //    {
                                                //        // 亚像素优化
                                                //        double subX = xData[i] - h / 2 * (derivatives[i + 1] - derivatives[i - 1]) /
                                                //            (derivatives[i + 1] - 2 * derivatives[i] + derivatives[i - 1]);

                                                //        inflectionPoints.Add(new Point2d(subX, spline.Interpolate(subX)));
                                                //    }
                                                //}

                                                //var lastinflectionPoints = inflectionPoints.Where(p => !inflectionPoints.Any(q => q != p && Math.Abs(q.X - p.X) < 5)).ToArray();

                                                //foreach (var t in lastinflectionPoints)
                                                //{
                                                //    //roiEdgeLine.Set((int)t.Y, (int)t.X, new Vec3b(0, 255, 0));
                                                //    Cv2.Circle(roiEdgeLine, new Point((int)t.Y, (int)t.X), 1, Scalar.Green, -1);
                                                //}
                                            }

                                            var approx = Cv2.ApproxPolyDP(listEdgeLinePoints, 9.009, false);
                                            foreach (var t in approx)
                                            {
                                                Cv2.Circle(roiEdgeLine, t, 5, Scalar.Green, 1);
                                            }


                                            var points = listEdgeLinePoints.Select(t => Tuple.Create((double)t.X, (double)t.Y)).ToList();
                                            var sortedPoints = points.OrderBy(p => p.Item1).ToList();
                                            var normalizeDataResult = PolyHelper.NormalizeData(sortedPoints);
                                            var x = normalizeDataResult.Item1;
                                            var y = normalizeDataResult.Item2;
                                            var xMean = normalizeDataResult.Item3;
                                            var xStd = normalizeDataResult.Item4;
                                            double[] coefficients = PolyHelper.RidgePolynomialFit(x, y, 5);
                                            double[] firstDeriv = PolyHelper.CalculateDerivativeCoefficients(coefficients);
                                            double[] secondDeriv = PolyHelper.CalculateDerivativeCoefficients(firstDeriv);
                                            var findZero = PolyHelper.FindNumericalInflectionPoints(x, y);
                                            var extrema = PolyHelper.FindExtrema(sortedPoints, coefficients, firstDeriv, secondDeriv, xMean, xStd);
                                            foreach (var t in sortedPoints)
                                            {
                                                var xval = t.Item1;
                                                var yval = PolyHelper.EvaluatePolynomial(coefficients, xval);
                                                roiEdgeLine.Set((int)yval, (int)xval, new Vec3b(0, 0, 255));
                                            }
                                            //foreach (var t in extrema)
                                            //{
                                            //    var xval = t.XActual;
                                            //    var yval = PolyHelper.EvaluatePolynomial(coefficients, xval);
                                            //    //roiEdgeLine.Set((int)yval, (int)xval, new Vec3b(0, 255, 0));
                                            //    Cv2.Circle(roiEdgeLine, new Point((int)xval, (int)yval), 5, Scalar.Green, 1);
                                            //}

                                            //// 1. 创建测试数据（含异常点）
                                            //var rawData = new List<Point2d>
                                            //{
                                            //    new Point2d { X = 0.0, Y = 2.0 },   // 正常点
                                            //    new Point2d { X = 1.0, Y = 3.1 },   // 正常点
                                            //    new Point2d { X = 3.0, Y = 5.0 },   // 正常点
                                            //    new Point2d { X = 4.0, Y = 6.1 },   // 正常点
                                            //    new Point2d { X = 6.0, Y = 8.0 },   // 正常点
                                            //    new Point2d { X = 7.0, Y = 9.0 },   // 正常点
                                            //};
                                            //var processedPoints2d = rawData.Select(t => (Point2d)t).ToList();

                                            //var processor = new CurveProcessorDeepSeek();
                                            //var points = listEdgeLinePoints.Select(t => new CurveProcessorDeepSeek.Point(t.X, t.Y)).ToList();
                                            //var cleaned = processor.Preprocess(points);

                                            //var polyOrder = 15;
                                            //if (cleaned.Count >= polyOrder + 1)
                                            //{
                                            //    // 多项式拟合
                                            //    var poly = processor.FitPolynomial(cleaned, polyOrder);
                                            //    //var firstDeriv = processor.Derivative(poly);
                                            //    //var secondDeriv = processor.Derivative(firstDeriv);

                                            //    Console.WriteLine($"多项式系数: [{string.Join(", ", poly.Coefficients)}]");


                                            //    foreach (var t in listEdgeLinePoints)
                                            //    {
                                            //        var xval = t.X;
                                            //        var yval = EvaluatePolynomial(xval, poly.Coefficients);
                                            //        roiEdgeLine.Set((int)yval, xval, new Vec3b(0, 0, 255));
                                            //    }

                                            //    // 3. 求导计算
                                            //    var firstDeriv = processor.FirstDerivative(poly.Coefficients);
                                            //    var secondDeriv = processor.SecondDerivative(poly.Coefficients);
                                            //    var inflectionX = processor.FindInflectionPoint(secondDeriv);

                                            //    var vertexX = processor.FindSecondDerivativeVertex(secondDeriv.Coefficients);
                                            //}

                                            //var points = listEdgeLinePoints.Select(p => new Point2d(p.X, p.Y)).ToList();
                                            //var xValues = points.Select(p => (double)p.X).ToArray();
                                            //var yValues = points.Select(p => (double)p.Y).ToArray();
                                            //var coefficients = Fit.Polynomial(xValues, yValues, 5);
                                            //// 一阶导数系数: [e, 2d, 3c, 4b, 5a]
                                            //var firstDeriv = new double[5];
                                            //for (var i = 0; i < 5; i++)
                                            //    firstDeriv[i] = (i + 1) * coefficients[i + 1];
                                            //// 二阶导数系数: [2d, 6c, 12b, 20a]
                                            //var secondDeriv = new double[4];
                                            //for (var i = 0; i < 4; i++)
                                            //    secondDeriv[i] = (i + 1) * (i + 2) * coefficients[i + 2];

                                            //var rawPoints = listEdgeLinePoints.Select(t => new CurveAnalyzer.Point(t.X, t.Y)).ToList();

                                            //// 步骤1：数据预处理
                                            //var validPoints = CurveAnalyzer.PreprocessPoints(rawPoints);
                                            //Console.WriteLine("有效分界点数量: " + validPoints.Count);

                                            //// 步骤2：多项式拟合
                                            //double[] coefficients;
                                            //if (!CurveAnalyzer.TryPolynomialFit(validPoints, out coefficients))
                                            //{
                                            //    Console.WriteLine("拟合失败");
                                            //    return;
                                            //}
                                            //Console.WriteLine($"拟合系数: a={coefficients[0]:N4}, b={coefficients[1]:N4}, c={coefficients[2]:N4}, d={coefficients[3]:N4}, e={coefficients[4]:N4}, f={coefficients[5]:N4}");

                                            //// 步骤3：导数计算
                                            //Derivatives derivatives;
                                            //CurveAnalyzer.CalculateDerivatives(coefficients, out derivatives);

                                            //// 步骤4：关键点分析
                                            //List<CurveAnalyzer.Point> extrema;
                                            //List<InflectionPoint> inflectionPoints;
                                            //FindCriticalPoints(derivatives.SecondDerivCoeffs, coefficients, out extrema, out inflectionPoints);

                                            //Console.WriteLine("\n二次导数极值点：");
                                            //foreach (var p in extrema)
                                            //{
                                            //    Cv2.Circle(showFindPointsOnSrc, new Point(p.X, p.Y), 1, Scalar.Red, -1);
                                            //    Cv2.Circle(roiEdgeLine, new Point(p.X, p.Y), 1, Scalar.Red, -1);
                                            //    Console.WriteLine($"({p.X:N2}, {p.Y:N2})");
                                            //}

                                            //Console.WriteLine("\n拐点：");
                                            //foreach (var ip in inflectionPoints)
                                            //{
                                            //    Cv2.Circle(showFindPointsOnSrc, new Point(ip.Location.X, ip.Location.Y), 3, Scalar.Purple, 1);
                                            //    Cv2.Circle(roiEdgeLine, new Point(ip.Location.X, ip.Location.Y), 3, Scalar.Purple, 1);
                                            //    Console.WriteLine($"{ip.ConcavityType} ({ip.Location.X:N2}, {ip.Location.Y:N2})");
                                            //}

                                            //// 完整处理流程
                                            ////var testData = listEdgeLinePoints.Select(p => new PointProcessor.Point(p.X, p.Y)).ToList();

                                            //// 测试数据生成
                                            //var testData = Enumerable.Range(0, 1000)
                                            //    .Select(i => new PointProcessor.Point(i * 0.1,
                                            //        Math.Pow(i * 0.1, 5) - 2 * Math.Pow(i * 0.1, 3) + i * 0.1))
                                            //    .ToList();

                                            //var result = PointProcessor.ProcessData(testData);

                                            //Console.WriteLine($"上凹点: {result.FirstUpConcave}");
                                            //Console.WriteLine($"下凹点: {result.FirstDownConcave}");
                                            //Console.WriteLine($"拐点: {result.Inflection}");


                                            //// 数据预处理流程
                                            //var filteredData = PolynomialAnalyzer.ApplyGaussianFilter(
                                            //    dataPoints, windowSize: 7, sigma: 1.5);

                                            //var sampledData = PolynomialAnalyzer.DouglasPeuckerReduction(
                                            //    filteredData, epsilon: 0.1);

                                            //foreach (var p in sampledData)
                                            //{
                                            //    Cv2.Circle(roiEdgeLine, new Point(p.Item1, p.Item2), 1, Scalar.Green, -1);
                                            //}

                                            //double[] coefficients = PolynomialAnalyzer.FitFifthOrder(sampledData);

                                            //// 查找极值点
                                            //var extrema = PolynomialAnalyzer.FindExtrema(coefficients);
                                            //Console.WriteLine("发现极值点：");
                                            //foreach (var p in extrema)
                                            //{
                                            //    Cv2.Circle(showFindPointsOnSrc, new Point(p.Item1, p.Item2), 1, Scalar.Red, -1);
                                            //    Cv2.Circle(roiEdgeLine, new Point(p.Item1, p.Item2), 1, Scalar.Red, -1);
                                            //    Console.WriteLine($"({p.Item1:F4}, {p.Item2:F4})");
                                            //}

                                            //// 查找拐点
                                            //var inflectionPoints = PolynomialAnalyzer.FindInflectionPoints(coefficients);
                                            //Console.WriteLine("\n发现拐点：");
                                            //foreach (var p in inflectionPoints)
                                            //{
                                            //    Cv2.Circle(showFindPointsOnSrc, new Point(p.Item1, p.Item2), 1, Scalar.Purple, -1);
                                            //    Cv2.Circle(roiEdgeLine, new Point(p.Item1, p.Item2), 1, Scalar.Purple, -1);
                                            //    Console.WriteLine($"({p.Item1:F4}, {p.Item2:F4})");
                                            //}
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"错误：{ex.Message}");
                                        }

                                        _headlampAnalysisByGb2591.AppendImage("原图输出二阶导计算结果", showFindPointsOnSrc);
                                    }
                                }

                                //var config = new CurveAnalysisConfig
                                //{
                                //    SmoothingSigma = 2.0,
                                //    NoiseThreshold = 0.1,
                                //    ExtremumThreshold = 0.15
                                //};

                                //var rawData = listEdgeLinePoints.Select(p => new Point2d(p.X, p.Y)).ToList();
                                //var result = CurveAnalyzer.AnalyzeCurve(rawData, config);
                                //foreach (var t in result.ExtremumPoints)
                                //    Cv2.Circle(roiEdgeLine, (Point)t, 1, Scalar.Red, -1);
                                //foreach (var t in result.InflectionPoints)
                                //    Cv2.Circle(roiEdgeLine, (Point)t, 5, Scalar.Purple, 1);


                                _headlampAnalysisByGb2591.AppendImage("二阶导输出图像", roiEdgeLine);
                            }

                            _headlampAnalysisByGb2591.AppendImage("CannyEdge", edgeTestMat);
                        }
                    }

                    _headlampAnalysisByGb2591.AppendImage("输出结果图像", dstMat);
                }
            }

            BindDictionaryToComboBox();
        }

        private void DrawContours(Mat toDrawMat, Mat toBinaryMat, double minGray, Scalar scalar)
        {
            using (var binaryMat = toBinaryMat.Clone())
            {
                if (binaryMat.Channels() == 3)
                    Cv2.CvtColor(binaryMat, binaryMat, ColorConversionCodes.BGR2GRAY);

                Cv2.Threshold(binaryMat, binaryMat, minGray, 255, ThresholdTypes.Binary);
                //Cv2.ImShow("Threshold", binaryMat);
                //Cv2.WaitKey();
                //Cv2.DestroyAllWindows();

                Cv2.Canny(binaryMat, binaryMat, 50, 150);
                //Cv2.ImShow("Canny", binaryMat);
                //Cv2.WaitKey();
                //Cv2.DestroyAllWindows();

                // 形态学闭操作（连接断裂边缘）
                var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(21, 3));
                Cv2.MorphologyEx(binaryMat, binaryMat, MorphTypes.Close, kernel);
                //Cv2.ImShow("MorphologyEx", binaryMat);
                //Cv2.WaitKey();
                //Cv2.DestroyAllWindows();

                _headlampAnalysisByGb2591.AppendImage(string.Format("二值化图像{0}~255", minGray), binaryMat);

                Point[][] contours;
                int maxIndex;
                if (MyCamera.TryGetMaxContourOuter(binaryMat, out contours, out maxIndex, RetrievalModes.Tree, ContourApproximationModes.ApproxNone))
                {
                    Cv2.DrawContours(toDrawMat, contours, maxIndex, scalar);
                }
            }
        }

        public class PointInfo
        {
            public double X { get; set; }
            public double Y { get; set; }
            public string Type { get; set; }
        }

        private List<Point> PreprocessPoints(List<Point> rawPoints)
        {
            if (rawPoints.Count < 3) return rawPoints;

            var points = rawPoints.OrderBy(p => p.X).ToList();
            bool modified;

            do
            {
                modified = false;
                var removeIndices = new HashSet<int>();

                for (int i = 0; i <= points.Count - 3; i++)
                {
                    var y0 = points[i].Y;
                    var y1 = points[i + 1].Y;
                    var y2 = points[i + 2].Y;

                    var monotonic = (y0 <= y1 && y1 <= y2) || (y0 >= y1 && y1 >= y2);
                    if (!monotonic) removeIndices.Add(i + 1);
                }

                // 反向删除保持索引有效
                foreach (var idx in removeIndices.OrderByDescending(x => x))
                {
                    points.RemoveAt(idx);
                    modified = true;
                }
            } while (modified && points.Count >= 3);

            return points;

            //var sorted = points.OrderBy(p => p.X).ToList();
            //var toRemove = new HashSet<int>();

            //for (int i = 0; i < sorted.Count - 2; i++)
            //{
            //    double y0 = sorted[i].Y;
            //    double y1 = sorted[i + 1].Y;
            //    double y2 = sorted[i + 2].Y;

            //    bool isIncreasing = (y0 <= y1) && (y1 <= y2);
            //    bool isDecreasing = (y0 >= y1) && (y1 >= y2);

            //    if (!(isIncreasing || isDecreasing))
            //    {
            //        toRemove.Add(i + 1);
            //    }
            //}

            //return sorted.Where((p, idx) => !toRemove.Contains(idx)).ToList();
        }

        // 多项式拟合核心算法
        public static Tuple<double[], double> FitPolynomial(List<Point2d> points, int order)
        {
            if (points.Count < order + 1)
                throw new ArgumentException("Insufficient points for polynomial fitting");

            var x = points.Select(p => p.X).ToArray();
            var y = points.Select(p => p.Y).ToArray();

            return Tuple.Create(
                Fit.Polynomial(x, y, order),  // 多项式系数
                GoodnessOfFit.RSquared(x.Select(xi => EvaluatePolynomial(xi, Fit.Polynomial(x, y, order))), y) // R平方值
            );
        }

        private List<double> SolveQuadratic(double a, double b, double c)
        {
            List<double> roots = new List<double>();

            if (a == 0)
            {
                if (b == 0) return roots;
                roots.Add(-c / b);
                return roots;
            }

            double discriminant = b * b - 4 * a * c;
            if (discriminant < 0) return roots;

            if (discriminant == 0)
            {
                roots.Add(-b / (2 * a));
            }
            else
            {
                roots.Add((-b + Math.Sqrt(discriminant)) / (2 * a));
                roots.Add((-b - Math.Sqrt(discriminant)) / (2 * a));
            }

            return roots;
        }

        private List<double> SolveLinear(double a, double b)
        {
            List<double> roots = new List<double>();
            if (a == 0) return roots;
            roots.Add(-b / a);
            return roots;
        }

        // 多项式求值（霍纳法则优化）
        public static double EvaluatePolynomial(double x, double[] coefficients)
        {
            if (coefficients == null || coefficients.Length == 0)
                return double.NaN;

            double result = coefficients[coefficients.Length - 1];
            for (int i = coefficients.Length - 2; i >= 0; i--)
                result = result * x + coefficients[i];

            return result;
        }

        // 导数计算模块
        public static double[] GetDerivativeCoefficients(double[] coefficients, int derivativeOrder)
        {
            if (derivativeOrder == 0) return coefficients;

            var result = new double[coefficients.Length - derivativeOrder];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = coefficients[i + derivativeOrder];
                for (int j = 0; j < derivativeOrder; j++)
                    result[i] *= (i + derivativeOrder - j);
            }
            return result;
        }

        public class AnalysisResult
        {
            public double X { get; set; }
            public double Y { get; set; }
            public string PointType { get; set; }
            public string Concavity { get; set; }
        }

        // 特征点分析主方法
        public static List<AnalysisResult> AnalyzeCurve(double[] coefficients, double minX, double maxX)
        {
            var results = new List<AnalysisResult>();

            // 计算各阶导数
            var firstDeriv = GetDerivativeCoefficients(coefficients, 1);
            var secondDeriv = GetDerivativeCoefficients(coefficients, 2);
            var thirdDeriv = GetDerivativeCoefficients(coefficients, 3);

            double relativeEpsilon = 1e-4 * (maxX - minX); // 动态调整误差范围

            // 查找特征点
            FindCriticalPoints(firstDeriv, minX, maxX).ForEach(x =>
            {
                var y = EvaluatePolynomial(x, coefficients);
                var secondDerivVal = EvaluatePolynomial(x, secondDeriv);
                results.Add(new AnalysisResult
                {
                    X = x,
                    Y = y,
                    PointType = Math.Abs(secondDerivVal) < relativeEpsilon ? "Stationary" : (secondDerivVal > 0 ? "Minimum" : "Maximum"),
                    Concavity = secondDerivVal > 0 ? "Convex" : "Concave"
                });
            });

            // 查找拐点
            FindCriticalPoints(secondDeriv, minX, maxX).ForEach(x =>
            {
                //var epsilon = 1e-5;
                var left = EvaluatePolynomial(x - relativeEpsilon, secondDeriv);
                var right = EvaluatePolynomial(x + relativeEpsilon, secondDeriv);

                if (Math.Sign(left) != Math.Sign(right))
                {
                    results.Add(new AnalysisResult
                    {
                        X = x,
                        Y = EvaluatePolynomial(x, coefficients),
                        PointType = "Inflection",
                        Concavity = left < right ? "Concave to Convex" : "Convex to Concave"
                    });
                }
            });

            return results.OrderBy(r => r.X).ToList();
        }

        // 数值方法求根（使用MathNet内置算法）
        private static List<double> FindCriticalPoints(double[] coefficients, double minX, double maxX)
        {
            try
            {
                return FindRoots.Polynomial(coefficients.Reverse().ToArray())
                    .Where(r => r.Imaginary == 0 && r.Real >= minX && r.Real <= maxX)
                    .Select(r => r.Real)
                    .Distinct()
                    .ToList();
            }
            catch
            {
                return new List<double>();
            }
        }

        //private double EvaluatePolynomial(double[] coeffs, double x)
        //{
        //    // y=ax^5+bx^4+cx^3+dx^2+ex+f
        //    double result = 0;
        //    int degree = coeffs.Length - 1;
        //    for (int i = 0; i < coeffs.Length; i++)
        //    {
        //        result += coeffs[i] * Math.Pow(x, degree - i);
        //    }
        //    return result;
        //}
    }
}
