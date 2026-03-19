using CommonUtility;
using CommonUtility.HikSdk;
using Controller;
using Emgu.CV;
using Emgu.CV.Structure;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using InputArray = OpenCvSharp.InputArray;
using Mat = OpenCvSharp.Mat;
using OutputArray = OpenCvSharp.OutputArray;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace CheckSystem.OpenCvSharp
{
    public partial class FrmOsramTest : UIForm
    {
        private readonly ImageViewer _mainImageViewer = new ImageViewer { Dock = DockStyle.Fill };
        public Dictionary<string, Mat> Images = new Dictionary<string, Mat>();

        private readonly KebodaLdmSeries _kebodaLdm = new KebodaLdmSeries("KeBoDa");
        private readonly SyRenesasMcuControllerMaster _controllerMaster = new SyRenesasMcuControllerMaster("IP28");
        private readonly OsramEviyosVisionAnalysis _osramEviyosVisionAnalysis = new OsramEviyosVisionAnalysis("OsramEviyosVisionAnalysis");

        public FrmOsramTest()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            ImagePanel.Controls.Add(_mainImageViewer);
            Load += FrmOsramTest_Load;
        }

        private void FrmOsramTest_Load(object sender, EventArgs e)
        {
            try
            {
                _controllerMaster.InitRemoteIpAddress("192.168.1.28:8088");
                _kebodaLdm.Can = _controllerMaster.GatewayCan1;
                _kebodaLdm.ModuleAwake();
                _kebodaLdm.LeavingHomeActive();
            }
            catch (Exception exception)
            {
                ShowErrorTip(exception.Message);
            }

            _x = Width;//获取窗体的宽度
            _y = Height;//获取窗体的高度
            SetTag(funcPanel);//调用方法
            Resize += FrmPesTest_Resize;
            cmbImageList.SelectedIndexChanged += CmbImageList_SelectedIndexChanged;
            InitImageViewer(_mainImageViewer);
        }

        private void AppendImage(string name, Mat mat)
        {
            if (Images.ContainsKey(name))
            {
                try
                {
                    if (Images[name] != null)
                    {
                        Images[name].Dispose();
                        Images[name] = null;
                    }
                }
                catch (Exception)
                {
                    Images[name] = null;
                }
            }
            else
            {
                Images.Add(name, null);
            }

            Images[name] = mat.Clone();
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
            if (!Images.Any())
            {
                // 清除当前绑定数据
                cmbImageList.DataSource = null;
                cmbImageList.Items.Clear();

                var emptyImage = new VisionImage();
                Algorithms.Copy(emptyImage, _mainImageViewer.Image);
                emptyImage.Dispose();
                return;
            }

            cmbImageList.DataSource = new BindingSource(Images, null);
            cmbImageList.DisplayMember = "Key"; // 显示的文本
            cmbImageList.ValueMember = "Key"; // 实际绑定的值

            cmbImageList.SelectedIndex = cmbImageList.Items.Count - 1;
        }

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

        public void ReadImage(string name, string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath) && !string.IsNullOrEmpty(name))
            {
                var mat = Cv2.ImRead(filePath);
                AppendImage(name, mat);
                mat.Dispose();
            }
        }

        private Mat GetImage(string name)
        {
            if (Images.ContainsKey(name) && Images[name] != null && !Images[name].Empty())
            {
                return Images[name].Clone();
            }
            ShowErrorTip(string.Format(@"'{0}'不存在", name));
            return null;
        }

        private void btnReadImage_Click(object sender, EventArgs e)
        {
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
                ReadImage("原图", openFi.FileName);
                BindDictionaryToComboBox();
            }
        }

        private void btnOtsu_Click(object sender, EventArgs e)
        {
            var src = GetImage("原图");

            if (src != null)
            {
                using (var otsuMat = new Mat())
                {
                    Cv2.CvtColor(src, otsuMat, ColorConversionCodes.RGB2GRAY);
                    // 滤波
                    Cv2.Blur(otsuMat, otsuMat, new Size(3, 3));
                    // otsu
                    Cv2.Threshold(otsuMat, otsuMat, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                    AppendImage("OTSU", otsuMat);
                    BindDictionaryToComboBox();
                    otsuMat.Dispose();
                }
            }
        }

        private void btnOuterRect_Click(object sender, EventArgs e)
        {
            var src = GetImage("OTSU");
            var srcColor = GetImage("原图");

            if (src != null && srcColor != null)
            {
                var maxRect = new Rect();
                var maxSize = 0d;
                var angle = 0f;

                var option = new UIEditOption { AutoLabelWidth = true };
                var retrievalModes = EnumOperater.GetEnumValueList<RetrievalModes>().Select(t => t.ToString()).ToList();
                var contourApproximationModes = EnumOperater.GetEnumValueList<ContourApproximationModes>().Select(t => t.ToString()).ToList();
                option.AddCombobox("retrievalModes", "retrievalModes", retrievalModes.ToArray(), 0);
                option.AddCombobox("contourApproximationModes", "contourApproximationModes", contourApproximationModes.ToArray(), 0);

                var frm = new UIEditForm(option);
                frm.Render();
                frm.ShowDialog();

                if (frm.IsOK)
                {
                    var retrievalMode = EnumOperater.GetEnumByValue<RetrievalModes>(frm["retrievalModes"].ToString());
                    var contourApproximationMode = EnumOperater.GetEnumByValue<ContourApproximationModes>(frm["contourApproximationModes"].ToString());

                    using (var srcGray = src.Clone())
                    {
                        // 1、 先取出最大外接矩形
                        Point[][] contours;
                        HierarchyIndex[] hierarchy;
                        Cv2.FindContours(srcGray, out contours, out hierarchy, retrievalMode, contourApproximationMode);

                        for (var i = 0; i < hierarchy.Length; i++)
                        {
                            var temp = contours[i];
                            var tempSize = Cv2.ContourArea(temp);

                            if (!(tempSize > maxSize))
                                continue;
                            var areaRect = Cv2.MinAreaRect(temp);
                            angle = areaRect.Angle;
                            maxRect = areaRect.BoundingRect();
                            maxSize = tempSize;
                        }

                        if (maxSize > 0d && maxRect.Left + maxRect.Width <= src.Width && maxRect.Top + maxRect.Height <= src.Height)
                        {
                            using (var showImg = srcColor.Clone(maxRect))
                            {
                                //showImg.ImWrite(@"E:\Projects\万级像素\自动线终检\有一条线的不良件\测试图像3\Image_456_RESULT.bmp");

                                var newOstu = showImg.Clone();

                                Cv2.CvtColor(newOstu, newOstu, ColorConversionCodes.BGR2GRAY);
                                Cv2.MorphologyEx(newOstu, newOstu, MorphTypes.Open, new Mat());
                                Cv2.Threshold(newOstu, newOstu, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
                                Cv2.ImShow("newOstu", newOstu);
                                Cv2.WaitKey();
                                Cv2.DestroyAllWindows();
                                newOstu.Dispose();

                                AppendImage("最大外接矩形", showImg);
                                BindDictionaryToComboBox();
                                showImg.Dispose();
                            }
                        }
                        else
                        {
                            using (var showImg = srcColor.Clone())
                            {
                                AppendImage("最大外接矩形", showImg);
                                BindDictionaryToComboBox();
                                showImg.Dispose();
                            }
                        }

                        srcGray.Dispose();
                    }
                }
            }
        }

        private void btnClearAllImage_Click(object sender, EventArgs e)
        {
            var keys = Images.Keys.ToList();
            foreach (var name in keys)
            {
                try
                {
                    if (Images[name] != null)
                    {
                        Images[name].Dispose();
                        Images[name] = null;
                    }
                }
                catch (Exception)
                {
                    Images[name] = null;
                }
            }
            Images.Clear();
            BindDictionaryToComboBox();
        }

        private static bool AnalysisRectLx(Mat mat, bool isHorizontal, double standardLx, ref List<RectWithResult> rectWithResults, out string ngMsg)
        {
            ngMsg = string.Empty;

            var width = mat.Width;
            var height = mat.Height;

            var iMax = isHorizontal ? 20 : 2;
            var jMax = isHorizontal ? 2 : 20;

            var perWidth = width / (float)iMax;//isHorizontal ? width / 20f : width / 2f;
            var perHeight = height / (float)jMax;//isHorizontal ? height / 2f : height / 20f;

            var toDrawRect = new List<RectWithResult>();
            var ngCount = 0;

            for (var i = 0; i < iMax; i++)
            {
                for (var j = 0; j < jMax; j++)
                {
                    var rX = i * perWidth;
                    var rY = j * perHeight;
                    var r = MyCamera.GetRectInMat(width, height, new Rect((int)rX, (int)rY, (int)Math.Ceiling(perWidth), (int)Math.Ceiling(perHeight)));

                    using (var rMask = new Mat(mat, r))
                    {
                        var rMean = Cv2.Mean(rMask);
                        var lx = MyCamera.GetLxByRgb(rMean);
                        Console.WriteLine(@"i={0}, j={1}, lx={2}, standard={3}", i, j, lx, standardLx);

                        var roiNgCount = 0;

                        {
                            var roiXMax = isHorizontal ? 2 : 5;
                            var roiYMax = isHorizontal ? 5 : 2;

                            var roiInRMaskWidth = rMask.Width / (float)roiXMax;//rMask.Width / 2f;
                            var roiInRMaskHeight = rMask.Height / (float)roiYMax;//rMask.Height / 5f;

                            for (var k = 0; k < roiXMax; k++)
                            {
                                for (var l = 0; l < roiYMax; l++)
                                {
                                    var roiX = k * roiInRMaskWidth;
                                    var roiY = l * roiInRMaskHeight;

                                    var roiInRMask = MyCamera.GetRectInMat(rMask.Width, rMask.Height, new Rect((int)roiX, (int)roiY, (int)Math.Ceiling(roiInRMaskWidth), (int)Math.Ceiling(roiInRMaskHeight)));

                                    using (var roiMask = new Mat(rMask, roiInRMask))
                                    {
                                        //roiMask.ImWrite(string.Format(@"E:\Projects\万级像素\自动线终检\有一条线的不良件\测试图像3\RoiMask\{0}_{1}.bmp", k, l));
                                        var roiLx = MyCamera.GetLxByRgb(roiMask.Mean());

                                        var isRoiLxNg = false;
                                        if (roiLx < standardLx)
                                        {
                                            isRoiLxNg = true;
                                            roiNgCount++;
                                            ngMsg += string.Format(
                                                @"[i={0}, j={1}, k={2}, l={3}]: roi lx={4}, standard={5}, {6};", i, j, k,
                                                l, roiLx, standardLx, "NG");
                                        }

                                        Console.WriteLine(@"[i={0}, j={1}, k={2}, l={3}]: roi lx={4}, standard={5}, {6}", i, j, k, l, roiLx, standardLx, isRoiLxNg ? "NG" : "OK");

                                        roiMask.Dispose();
                                    }
                                }
                            }
                        }

                        //var isNg = lx < standardLx;
                        var isNg = roiNgCount > 0;
                        toDrawRect.Add(new RectWithResult { I = i, J = j, IsOk = !isNg, R = r });
                        if (isNg)
                            ngCount++;

                        rMask.Dispose();
                    }
                }
            }

            rectWithResults.AddRange(toDrawRect);

            return ngCount <= 0;
        }

        internal class RectWithResult
        {
            public Rect R;
            public bool IsOk;
            public int I;
            public int J;

            public Point[][] contours;
            public HierarchyIndex[] hierarchy;
            public List<int> needDrawContoursIndex = new List<int>();
        }

        private bool IsCv2ShowImage;
        public int StardandMin = 40;
        public int StardandMax = 10000;

        private void btnCalcLx_Click(object sender, EventArgs e)
        {
            //Analysis("");

            var src = GetImage("原图");
            if (src != null)
            {
                _osramEviyosVisionAnalysis.IsCv2ShowImage = false;
                string bitmap;
                string error;
                _osramEviyosVisionAnalysis.StardandMin = (int)numericUpDown1.Value;
                _osramEviyosVisionAnalysis.IsCv2ShowImage = uiCheckBox1.Checked;
                var isOk = _osramEviyosVisionAnalysis.VisionAnalysis(src, out bitmap, out error);

                if (!string.IsNullOrEmpty(bitmap))
                {
                    // 从Base64字符串转换回字节数组
                    var convertedBytes = Convert.FromBase64String(bitmap);

                    // 从字节数组转换回Bitmap
                    using (var ms = new MemoryStream(convertedBytes))
                    {
                        var convertedBitmap = new Bitmap(ms);
                        // 保存或使用转换后的Bitmap

                        var mat = convertedBitmap.ToMat();
                        AppendImage("处理结果", mat);
                        BindDictionaryToComboBox();
                        mat.Dispose();
                    }
                }

                if (isOk)
                {
                    ShowSuccessTip("判定PASS");
                }
                else
                {
                    ShowErrorTip("判定FAILED: " + error);
                }
            }
        }

        private bool Analysis(string path)
        {
            var isOk = false;

            if (!string.IsNullOrEmpty(path))
            {
                ReadImage("原图", path);
            }

            var src = GetImage("原图");
            if (src != null)
            {
                IsCv2ShowImage = uiCheckBox1.Checked;


                var error = string.Empty;
                var resultBitmap = string.Empty;

                using (var srcMat = src.Clone())
                {
                    Cv2ShowImage("原始图像", srcMat);

                    var lightArea = srcMat.Clone();

                    using (var toAnalysisMat = lightArea.Clone())
                    {
                        Point[][] contours = { };
                        HierarchyIndex[] hierarchy = { };

                        using (var toAnalysisGrayMat = toAnalysisMat.Clone())
                        {
                            // 转换成灰度图像
                            if (toAnalysisGrayMat.Channels() == 3)
                                Cv2.CvtColor(toAnalysisGrayMat, toAnalysisGrayMat, ColorConversionCodes.BGR2GRAY);
                            Cv2ShowImage("原始灰度图像", toAnalysisGrayMat);

                            // 高斯模糊降噪
                            Cv2.GaussianBlur(toAnalysisGrayMat, toAnalysisGrayMat, new Size(3, 3), 0);
                            Cv2ShowImage("高斯模糊降噪", toAnalysisGrayMat);

                            // 自适应二值化
                            Cv2.AdaptiveThreshold(toAnalysisGrayMat, toAnalysisGrayMat, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 5, 2);
                            Cv2ShowImage("自适应二值化", toAnalysisGrayMat);

                            // 形态学操作（断开细连接）
                            Cv2.Erode(toAnalysisGrayMat, toAnalysisGrayMat, Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3)), iterations: 2);
                            Cv2ShowImage("形态学操作（断开细连接）", toAnalysisGrayMat);

                            // Erode后取最大内部轮廓
                            Rect maxErodeRect;
                            if (MyCamera.TryGetMaxContourOuterRect(toAnalysisGrayMat, out maxErodeRect))
                            {
                                var borderOffet = 2;
                                maxErodeRect = new Rect(maxErodeRect.X + borderOffet, maxErodeRect.Y + borderOffet, maxErodeRect.Width - borderOffet * 2, maxErodeRect.Height - borderOffet * 2);

                                using (var dstRoi = toAnalysisGrayMat.Clone(maxErodeRect))
                                {
                                    var top = maxErodeRect.Y;
                                    var left = maxErodeRect.X;
                                    var bottom = toAnalysisGrayMat.Height - (maxErodeRect.Y + maxErodeRect.Height);
                                    var right = toAnalysisGrayMat.Width - (maxErodeRect.X + maxErodeRect.Width);

                                    Cv2.CopyMakeBorder(dstRoi, dstRoi, top, bottom, left, right, BorderTypes.Constant, Scalar.White);
                                    Cv2ShowImage("去边界", dstRoi);

                                    // 去除小面积连通域
                                    var contours1 = Cv2.FindContoursAsArray(
                                        dstRoi, RetrievalModes.List, ContourApproximationModes.ApproxSimple
                                    );
                                    foreach (var contour in contours1)
                                    {
                                        if (Cv2.ContourArea(contour) < StardandMin) // 面积阈值
                                        {
                                            Cv2.DrawContours(dstRoi, new[] { contour }, -1, Scalar.White, -1);
                                        }
                                        else
                                        {
                                            var tp = contour.ToList();
                                            var xMin = tp.Min(x => x.X);
                                            var xMax = tp.Max(x => x.X);
                                            var yMin = tp.Min(x => x.Y);
                                            var yMax = tp.Max(x => x.Y);

                                            var filterMax = 3;
                                            if (Math.Abs(xMin - xMax) <= filterMax || Math.Abs(yMin - yMax) <= filterMax) // 再过滤大面积时左右或上下差值filterMax个像素的线段
                                            {
                                                //Cv2.Rectangle(dstRoi, Cv2.MinAreaRect(contour).BoundingRect(),Scalar.Red);
                                                Cv2.DrawContours(dstRoi, new[] { contour }, -1, Scalar.White, -1);
                                            }
                                            //else
                                            //{
                                            //    var rotatedRect = Cv2.MinAreaRect(contour);
                                            //    var angle = rotatedRect.Angle;
                                            //    var size = rotatedRect.Size;

                                            //    // 统一角度到0-180度范围
                                            //    if (size.Width < size.Height)
                                            //    {
                                            //        angle += 90;
                                            //        var temp = new Tuple<float, float>(size.Width, size.Height);
                                            //        size.Width = temp.Item2;
                                            //        size.Height = temp.Item1;

                                            //        var AngleThreshold = 1;
                                            //        var SizeRatioThreshold = 0.1;

                                            //        var lineType = 0;

                                            //        // 水平线判断
                                            //        if (Math.Abs(angle) <= AngleThreshold &&
                                            //            size.Height / size.Width < SizeRatioThreshold)
                                            //        {
                                            //            lineType = 1;
                                            //        }
                                            //        // 垂直线判断
                                            //        if (Math.Abs(angle - 90) <= AngleThreshold &&
                                            //            size.Width / size.Height < SizeRatioThreshold)
                                            //        {
                                            //            lineType = 2;
                                            //        }

                                            //        if (lineType > 0)
                                            //        {
                                            //            Cv2.DrawContours(dstRoi, new[] { contour }, -1, Scalar.White, -1);
                                            //        }
                                            //    }
                                            //}
                                        }
                                    }

                                    Cv2ShowImage("去除小面积连通域", dstRoi);

                                    // 轮廓扫描
                                    Cv2.FindContours(dstRoi, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

                                    var ngContourCount = 0;
                                    StardandMin = (int)numericUpDown1.Value;

                                    var maxSize = 0d;
                                    var maxIndex = -1;
                                    var minSize = (double)(src.Width * src.Height);
                                    var minIndex = -1;

                                    for (var i = 0; i < hierarchy.Length; i++)
                                    {
                                        var len = Cv2.ContourArea(contours[i]);
                                        //Console.WriteLine(@"SIZE = {0}", len);

                                        if (len > StardandMin && len < StardandMax)
                                        {
                                            Console.WriteLine(@"NG SIZE = {0}", len);
                                            //Cv2.DrawContours(toAnalysisMat, contours, i, Scalar.Red);
                                            Cv2.Rectangle(toAnalysisMat, Cv2.MinAreaRect(contours[i]).BoundingRect(), Scalar.Red);
                                            ngContourCount++;
                                        }

                                        if (len > maxSize && len < 0.9 * (src.Width * src.Height))
                                        {
                                            maxIndex = i;
                                            maxSize = len;
                                        }

                                        if (len < minSize && len > 40 && len < 0.9 * (src.Width * src.Height))
                                        {
                                            minSize = len;
                                            minIndex = i;
                                        }
                                    }

                                    //Cv2.DrawContours(toAnalysisMat, contours, -1, Scalar.Green);

                                    if (maxIndex > -1 && maxSize >= 0)
                                    {
                                        //Cv2.DrawContours(toAnalysisMat, contours, maxIndex, Scalar.Yellow);
                                        Console.WriteLine(@"max size = " + maxSize);
                                    }

                                    if (minIndex > -1 && minSize >= 0)
                                    {
                                        //Cv2.DrawContours(toAnalysisMat, contours, minIndex, Scalar.Purple);
                                        Console.WriteLine(@"min size = " + minSize);
                                    }

                                    var rb = toAnalysisMat.ToBitmap();
                                    resultBitmap = BitmapToBase64String(rb);
                                    rb.Dispose();

                                    if (ngContourCount == 0)
                                        isOk = true;
                                }
                            }

                            toAnalysisGrayMat.Dispose();
                        }

                        toAnalysisMat.Dispose();
                    }

                    lightArea.Dispose();
                    srcMat.Dispose();

                    var mat = Base64StringToBitmap(resultBitmap).ToMat();
                    AppendImage("处理结果", mat);
                    BindDictionaryToComboBox();
                    mat.Dispose();

                    if (isOk)
                    {
                        ShowSuccessTip("判定PASS");
                    }
                    else
                    {
                        ShowErrorTip("判定FAILED: " + error);
                    }
                }
            }

            return isOk;
        }

        // 更高效的实现方式
        static void ApplyGammaCorrection(Mat input, Mat output, double gamma)
        {
            using (Mat lookUpTable = new Mat(1, 256, MatType.CV_8UC1))
            {
                var lutData = new byte[256];
                for (int i = 0; i < 256; i++)
                {
                    lutData[i] = (byte)Math.Min(255, Math.Round(Math.Pow(i / 255.0, gamma) * 255.0));
                }
                lookUpTable.SetArray(0, 0, lutData);
                Cv2.LUT(input, lookUpTable, output);
            }
        }

        private void Cv2ShowImage(string name, Mat mat)
        {
            if (IsCv2ShowImage)
            {
                if (mat != null && !mat.Empty())
                {
                    Cv2.ImShow(name, mat);
                    Cv2.WaitKey();
                    Cv2.DestroyAllWindows();
                }
            }
        }

        private static Bitmap Base64StringToBitmap(string base64String)
        {
            // 从Base64字符串转换回字节数组
            var convertedBytes = Convert.FromBase64String(base64String);

            // 从字节数组转换回Bitmap
            using (var ms = new MemoryStream(convertedBytes))
            {
                var convertedBitmap = new Bitmap(ms);
                // 保存或使用转换后的Bitmap
                return convertedBitmap;
            }
        }

        private static string BitmapToBase64String(Bitmap bitmap)
        {
            // 将Bitmap转换为字节数组
            byte[] bitmapBytes;
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                bitmapBytes = ms.ToArray();
            }

            // 将字节数组转换为Base64字符串
            var base64String = Convert.ToBase64String(bitmapBytes);
            return base64String;
        }

        private void btnDrawContour_Click(object sender, EventArgs e)
        {
            var option = new UIEditOption { AutoLabelWidth = true };
            var retrievalModes = EnumOperater.GetEnumValueList<RetrievalModes>().Select(t => t.ToString()).ToList();
            var contourApproximationModes = EnumOperater.GetEnumValueList<ContourApproximationModes>().Select(t => t.ToString()).ToList();
            var listStr = new string[] { "原图", "抠图结果", "先将原图进行抠图" };
            option.AddCombobox("source", "source", listStr, 0);
            option.AddCombobox("retrievalModes", "retrievalModes", retrievalModes.ToArray(), 0);
            option.AddCombobox("contourApproximationModes", "contourApproximationModes", contourApproximationModes.ToArray(), 0);
            option.AddInteger("minArea", "minArea", 100);
            option.AddSwitch("isShowRect", "isShowRect", false);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (frm.IsOK)
            {
                var retrievalMode = EnumOperater.GetEnumByValue<RetrievalModes>(frm["retrievalModes"].ToString());
                var contourApproximationMode = EnumOperater.GetEnumByValue<ContourApproximationModes>(frm["contourApproximationModes"].ToString());
                var minArea = (int)frm["minArea"];
                var isShowRect = (bool)frm["isShowRect"];

                var src = string.Empty;

                if ((int)frm["source"] == 2)
                {
                    var srcImgae = GetImage("原图");
                    if (srcImgae != null)
                    {
                        Mat result;
                        Positioning(srcImgae, out result, frm["retrievalModes"].ToString(), frm["contourApproximationModes"].ToString());
                        if (result != null && !result.Empty())
                        {
                            src = "抠图结果";
                            AppendImage("抠图结果", result);
                            BindDictionaryToComboBox();
                            result.Dispose();
                        }
                    }
                }
                else
                {
                    src = listStr[(int)frm["source"]];
                }

                if (string.IsNullOrEmpty(src))
                    return;

                var srcColor = GetImage(src);

                if (srcColor != null)
                {
                    using (var srcGray = srcColor.Clone())
                    {
                        //Cv2.MedianBlur(srcGray, srcGray, 11);
                        //var diff = new Mat();
                        //var img2 = srcColor.Clone();
                        //Cv2.Absdiff(srcGray, img2, diff);

                        //if (diff.Channels() == 3)
                        //    Cv2.CvtColor(diff, diff, ColorConversionCodes.BGR2GRAY);
                        ////Cv2.BitwiseNot(diff, diff);

                        //Cv2.ImShow("Absdiff", diff);
                        //Cv2.WaitKey();
                        //Cv2.DestroyAllWindows();

                        //var thres = new Mat();
                        //Cv2.Threshold(diff, thres, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);

                        //// 1、 先取出最大外接矩形
                        //Point[][] contours = { };
                        //HierarchyIndex[] hierarchy = { };

                        //try
                        //{
                        //    Cv2.FindContours(thres, out contours, out hierarchy, retrievalMode, contourApproximationMode);
                        //}
                        //catch (Exception exception)
                        //{
                        //    //Console.WriteLine(exception);
                        //    ShowErrorTip("取轮廓异常：" + exception);
                        //}

                        //img2.Dispose();
                        //diff.Dispose();
                        //thres.Dispose();

                        // 灰度转换 +自适应二值化
                        if (srcGray.Channels() == 3)
                            Cv2.CvtColor(srcGray, srcGray, ColorConversionCodes.BGR2GRAY);
                        Cv2.AdaptiveThreshold(srcGray, srcGray, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 11, 2);
                        Cv2.ImShow("AdaptiveThreshold", srcGray);
                        Cv2.WaitKey();
                        Cv2.DestroyAllWindows();

                        //Mat element5 = new Mat(5, 5, MatType.CV_8U, new Scalar(1));
                        //Cv2.MorphologyEx(srcGray, srcGray, MorphTypes.Close, element5);
                        //Cv2.ImShow("Close", srcGray);
                        //Cv2.WaitKey();
                        //Cv2.DestroyAllWindows();
                        //Cv2.MorphologyEx(srcGray, srcGray, MorphTypes.Open, element5);
                        //Cv2.ImShow("Open", srcGray);
                        //Cv2.WaitKey();
                        //Cv2.DestroyAllWindows();
                        //element5.Dispose();

                        Point[][] contours = { };
                        HierarchyIndex[] hierarchy = { };

                        try
                        {
                            //Cv2.FindContours(srcGray, out contours, out hierarchy, retrievalMode, contourApproximationMode);
                            Cv2.FindContours(srcGray, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
                        }
                        catch (Exception exception)
                        {
                            //Console.WriteLine(exception);
                            ShowErrorTip("取轮廓异常：" + exception);
                        }

                        using (var showImg = srcColor.Clone())
                        {
                            var maxRect = new Rect();
                            var maxSize = 0d;

                            for (var i = 0; i < hierarchy.Length; i++)
                            {
                                var tpArea = Cv2.ContourArea(contours[i]);

                                if (tpArea >= maxSize)
                                {
                                    var areaRect = Cv2.MinAreaRect(contours[i]);
                                    maxRect = areaRect.BoundingRect();
                                    maxSize = maxRect.Width * maxRect.Height;
                                }
                            }

                            var ngContourCount = 0;

                            for (var i = 0; i < hierarchy.Length; i++)
                            {
                                foreach (var p in contours[i])
                                {
                                    if (maxRect.Contains(p))
                                    {

                                    }
                                }

                                //var len = Cv2.ContourArea(contours[i]);
                                var len = Cv2.ArcLength(contours[i], true);
                                Console.WriteLine($"len:{len}, maxSize:{maxSize}");

                                //Cv2.ConnectedComponentsWithStats()

                                //if (len > minArea && len < minArea * 2)
                                if (len > minArea && len < 120000)
                                {
                                    Cv2.DrawContours(showImg, contours, i, Scalar.Red);
                                    ngContourCount++;

                                    if (isShowRect)
                                    {
                                        var areaRect = Cv2.MinAreaRect(contours[i]);
                                        maxRect = areaRect.BoundingRect();
                                        Cv2.Rectangle(showImg, maxRect, Scalar.Yellow);
                                    }
                                }

                                //if (!(len > maxSize))
                                //    continue;

                                //var areaRect = Cv2.MinAreaRect(contours[i]);
                                //maxRect = areaRect.BoundingRect();
                                //maxSize = len;

                                //Cv2.DrawContours(showImg, contours, i, Scalar.Red);
                            }

                            if (ngContourCount > 0)
                            {
                                ShowErrorTip("判定FAIL: " + ngContourCount);
                            }
                            else
                            {
                                ShowSuccessTip("判定PASS");
                            }

                            AppendImage("取轮廓", showImg);
                            BindDictionaryToComboBox();
                            showImg.Dispose();
                        }

                        srcGray.Dispose();

                        Cv2.WaitKey();
                        Cv2.DestroyAllWindows();
                    }
                }
            }
        }

        private void btnPositioning_Click(object sender, EventArgs e)
        {
            var srcColor = GetImage("原图");
            if (srcColor != null)
            {
                Mat result;
                Positioning(srcColor, out result);
                if (result != null && !result.Empty())
                {
                    AppendImage("抠图结果", result);
                    BindDictionaryToComboBox();
                    result.Dispose();
                }
            }
        }

        [Description("R/W, XY比例最小值")]
        public float XyRatioMin = 3.8f;
        [Description("R/W, XY比例最大值")]
        public float XyRatioMax = 4.2f;

        private void Positioning(Mat mat, out Mat resultMat, string retrievalModeStr = "", string contourApproximationModeStr = "")
        {
            resultMat = null;

            if (string.IsNullOrEmpty(retrievalModeStr) || string.IsNullOrEmpty(contourApproximationModeStr))
            {
                var option = new UIEditOption { AutoLabelWidth = true };
                var retrievalModes = EnumOperater.GetEnumValueList<RetrievalModes>().Select(t => t.ToString()).ToList();
                var contourApproximationModes = EnumOperater.GetEnumValueList<ContourApproximationModes>().Select(t => t.ToString()).ToList();
                option.AddCombobox("retrievalModes", "retrievalModes", retrievalModes.ToArray(), 0);
                option.AddCombobox("contourApproximationModes", "contourApproximationModes", contourApproximationModes.ToArray(), 0);

                var frm = new UIEditForm(option);
                frm.Render();
                frm.ShowDialog();

                if (frm.IsOK)
                {
                    retrievalModeStr = frm["retrievalModes"].ToString();
                    contourApproximationModeStr = frm["contourApproximationModes"].ToString();
                }
            }

            if (!string.IsNullOrEmpty(retrievalModeStr) && !string.IsNullOrEmpty(contourApproximationModeStr))
            {
                var retrievalMode = EnumOperater.GetEnumByValue<RetrievalModes>(retrievalModeStr);
                var contourApproximationMode = EnumOperater.GetEnumByValue<ContourApproximationModes>(contourApproximationModeStr);

                using (var srcMat = mat.Clone())
                {
                    Rect r1;
                    double angle1;
                    if (GetMaxOuterRect(srcMat, retrievalMode, contourApproximationMode, out r1, out angle1))
                    {
                        RotateImage(srcMat, angle1);

                        Rect r3;
                        double angle3;
                        if (GetMaxOuterRect(srcMat, retrievalMode, contourApproximationMode, out r3, out angle3))
                        {
                            using (var newMat = srcMat.Clone(r3))
                            {
                                Rect r2;
                                double angle2;

                                using (var lastMat = newMat.Clone())
                                {
                                    var isHorizontal = 0; // 0=error,1=horizontal,2=vertical

                                    if ((float)lastMat.Width / lastMat.Height >= XyRatioMin && (float)lastMat.Width / lastMat.Height <= XyRatioMax)
                                        isHorizontal = 1;
                                    else if ((float)lastMat.Height / lastMat.Width >= XyRatioMin && (float)lastMat.Height / lastMat.Width <= XyRatioMax)
                                        isHorizontal = 2;

                                    if (isHorizontal == 1 || isHorizontal == 2)
                                    {
                                        if (isHorizontal == 1)
                                        {
                                            resultMat = lastMat.Clone();
                                        }
                                        else
                                        {
                                            var rb = lastMat.ToBitmap();
                                            rb.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                            resultMat = rb.ToMat();
                                        }
                                    }
                                    else
                                    {
                                        ShowErrorTip(string.Format("图像纠正后宽高比不在{0}~{1}之间,width={2},height={3}", XyRatioMin, XyRatioMax, lastMat.Width, lastMat.Height));
                                    }

                                    lastMat.Dispose();
                                }

                                newMat.Dispose();
                            }
                        }
                        else
                        {
                            ShowErrorTip("图像纠正后外轮廓定位失败");
                        }
                    }
                    else
                    {
                        ShowErrorTip("原始图像外轮廓定位失败");
                    }

                    srcMat.Dispose();
                }
            }
        }

        private bool GetMaxOuterRect(
            Mat srcMat, RetrievalModes retrievalMode, ContourApproximationModes contourApproximationMode, out Rect outRect, out double outAngle)
        {
            outRect = new Rect();
            outAngle = 0;

            var maxRect = new Rect();
            var maxSize = 0d;
            var angle = 0f;

            // 先讲图像预处理
            // 转化为灰度图
            using (var srcGray = new Mat())
            {
                Cv2.CvtColor(srcMat, srcGray, ColorConversionCodes.RGB2GRAY);
                // 滤波
                Cv2.Blur(srcGray, srcGray, new Size(3, 3));
                // otsu
                Cv2.Threshold(srcGray, srcGray, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                // 1、 先取出最大外接矩形
                Point[][] contours;
                HierarchyIndex[] hierarchy;
                Cv2.FindContours(srcGray, out contours, out hierarchy, retrievalMode, contourApproximationMode);

                for (var i = 0; i < hierarchy.Length; i++)
                {
                    var temp = contours[i];
                    var tempSize = Cv2.ContourArea(temp);

                    if (!(tempSize > maxSize))
                        continue;
                    var areaRect = Cv2.MinAreaRect(temp);
                    angle = areaRect.Angle;
                    maxRect = areaRect.BoundingRect();
                    maxSize = tempSize;
                }

                srcGray.Dispose();
            }

            if (maxSize == 0d)
                return false;

            outRect = maxRect;
            outAngle = angle;
            return true;
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

        private readonly CameraControl _cameraControl = new CameraControl();

        private void btnCaptureImage_Click(object sender, EventArgs e)
        {
            var uiFrm = new UIForm { Text = @"SET ROI" };
            var imageViewer = new ImageViewer { Dock = DockStyle.Fill };
            var btn = new UIButton { Text = @"确定", Dock = DockStyle.Bottom };

            var gainTxt = new NumericUpDown { Dock = DockStyle.Bottom, Minimum = 0, Maximum = 25, Value = 1 };
            var exposeTimeTxt = new NumericUpDown { Dock = DockStyle.Bottom, Minimum = 1, Maximum = 1000000, Value = 100000 };

            uiFrm.Controls.Add(imageViewer);
            uiFrm.Controls.Add(gainTxt);
            uiFrm.Controls.Add(exposeTimeTxt);
            uiFrm.Controls.Add(btn);

            MyCamera.PushMatEventHandle matHandle = delegate (MvCameraSdk.MV_GIGE_DEVICE_INFO info, Mat mat)
            {
                if (_cameraControl.CameraList.Any() &&
                    _cameraControl.CameraList[0].GigeInfo.chSerialNumber == info.chSerialNumber)
                {
                    var visionImg = MyCamera.MatToVisionImage(mat);
                    Algorithms.Copy(visionImg, imageViewer.Image);
                    visionImg.Dispose();
                    mat.Dispose();
                    GC.Collect();
                }
            };

            uiFrm.Load += ((sender1, e1) =>
            {
                btn.Click += ((btnSender, btnE) =>
                {
                    if (_cameraControl.CameraList.Any() &&
                        imageViewer.Image != null &&
                        imageViewer.Image.Width == _cameraControl.CameraList[0].PayloadWidth &&
                        imageViewer.Image.Height == _cameraControl.CameraList[0].PayloadHeight)
                    {
                        btnClearAllImage.PerformClick();
                        var mat = MyCamera.VisionImageToMat(imageViewer.Image);
                        AppendImage("原图", mat);
                        BindDictionaryToComboBox();

                        uiFrm.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        ShowErrorTip("图像采集异常");
                    }
                });
                ImageShowTool(imageViewer);
                imageViewer.SizeChanged += ImageViewer_SizeChanged;
                _cameraControl.DeviceListAcq();
                if (_cameraControl.CameraList.Any())
                {
                    _cameraControl.CameraList[0].OpenCamera();
                    _cameraControl.CameraList[0].SetGain((int)gainTxt.Value);
                    _cameraControl.CameraList[0].SetExposureTime((int)exposeTimeTxt.Value);
                    _cameraControl.CameraList[0].StartGrab();

                    gainTxt.ValueChanged += (gainTxtO, gainTxtArgs) =>
                    {
                        _cameraControl.CameraList[0].SetGain((int)gainTxt.Value);
                    };

                    exposeTimeTxt.ValueChanged += (exposeTimeTxtO, exposeTimeTxtArgs) =>
                    {
                        _cameraControl.CameraList[0].SetExposureTime((int)exposeTimeTxt.Value);
                    };
                }

                MyCamera.PushMat += matHandle;
            });

            uiFrm.Closed += ((sender1, e1) =>
            {
                MyCamera.PushMat -= matHandle;
                imageViewer.SizeChanged -= ImageViewer_SizeChanged;
                _cameraControl.CloseAllCamera();
            });

            uiFrm.WindowState = FormWindowState.Maximized;
            var result = uiFrm.ShowDialog();
        }

        private void btnSaveImageBmp_Click(object sender, EventArgs e)
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

        private void btnSaveImageJpg_Click(object sender, EventArgs e)
        {
            if (_mainImageViewer.Image != null)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    var result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        var path = Path.Combine(fbd.SelectedPath, string.Format("export_on_{0}_{1}.jpg",
                            DateTime.Now.ToString("yyyyMMdd-hhmmss"), Guid.NewGuid().ToString().Substring(24, 12)));
                        _mainImageViewer.Image.WriteJpegFile(path);
                        ShowSuccessTip("导出成功：" + path);
                    }
                }
            }
            else
            {
                ShowErrorTip("图像为空无法保存");
            }
        }

        /// <summary>
        /// 获取分割点
        /// </summary>
        /// <param name="contours"></param>
        /// <param name="contourCount"></param>
        /// <param name="arcLength"></param>
        /// <param name="farDistance"></param>
        /// <returns></returns>
        public List<Point> GetSplitPoints(Point[][] contours, List<int> contourCount, int arcLength, int farDistance)
        {
            #region 凸包检测
            List<double> lArc = new List<double>();
            //Mat src = srcImage.Clone();
            List<Point[]> lpContours = new List<Point[]>();
            List<int> hulls = new List<int>();
            Point lastP = new Point();
            Point firstP = new Point();
            Point farLastP = new Point();
            List<Point> lps = new List<Point>();
            int dot = 1;
            List<int> depth = new List<int>();
            for (int i = 0; i < contourCount.Count; i++)
            {
                InputArray inputArray = InputArray.Create<Point>(contours[contourCount[i]]);
                OutputArray outputArray = OutputArray.Create(hulls);
                Cv2.ConvexHull(inputArray, outputArray, false, false);
                if (Cv2.ArcLength(inputArray, true) < arcLength)
                {
                    //lArc.Add(Cv2.ArcLength(inputArray, true));
                    continue;
                }
                //前三个值得含义分别为：凸缺陷的起始点，凸缺陷的终点，凸缺陷的最深点（即边缘点到凸包距离最大点）。
                var defects = Cv2.ConvexityDefects(contours[contourCount[i]], hulls);
                for (int j = 0; j < defects.Length; j++)
                {
                    var start = contours[contourCount[i]][defects[j].Item0];
                    var end = contours[contourCount[i]][defects[j].Item1];
                    var far = contours[contourCount[i]][defects[j].Item2];
                    //OpenCvSharp.Point fart = contours[contourCount[i]][defects[j].Item3];
                    // if (defects[j].Item3 > farDistance) //(4500 < defects[j].Item3 && defects[j].Item3 < 300000)
                    {
                        lps.Add(contours[contourCount[i]][defects[j].Item2]);
                        depth.Add(defects[j].Item3);
                    }
                }
            }
            #endregion
            return lps;
        }

        public void ToBinImage(Mat srcGray)
        {
            var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(19, 19));
            // 1、灰度图及二值化
            if (srcGray.Channels() == 3)
                Cv2.CvtColor(srcGray, srcGray, ColorConversionCodes.BGR2GRAY);
            Cv2.ImShow("grayImage", srcGray);
            Cv2.Threshold(srcGray, srcGray, 0, 255, ThresholdTypes.Otsu);
            Cv2.ImShow("binImage", srcGray);

            // 2、对灰度图底帽处理
            kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(31, 31));
            Cv2.MorphologyEx(srcGray, srcGray, MorphTypes.BlackHat, kernel, new Point(-1, -1), 1);
            Cv2.ImShow("底帽运算", srcGray);
            var th = Cv2.Threshold(srcGray, srcGray, 0, 255, ThresholdTypes.Otsu);
            Cv2.ImShow("底帽处理后的二值化图像", srcGray);

            // 3、二值图闭运算处理
            kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.MorphologyEx(srcGray, srcGray, MorphTypes.Close, kernel, new Point(-1, -1));
            Cv2.ImShow("闭运算处理后的二值化图像", srcGray);

            {
                var tl = th * 0.5;
                Cv2.Canny(srcGray, srcGray, th, 255, L2gradient: true);
                Cv2.ImShow("canny", srcGray);
            }
        }

        private static void AnalysisContour(Mat mat, double tl, double th)
        {
            using (var blackMat = mat.Clone())
            {
                var width = blackMat.Width;
                var height = blackMat.Height;

                var iMax = 20;
                var jMax = 2;

                var perWidth = width / (float)iMax;
                var perHeight = height / (float)jMax;

                var toDrawRect = new List<RectWithResult>();
                var ngCount = 0;

                for (var i = 0; i < iMax; i++)
                {
                    for (var j = 0; j < jMax; j++)
                    {
                        var rX = i * perWidth;
                        var rY = j * perHeight;
                        var r = MyCamera.GetRectInMat(width, height, new Rect((int)rX, (int)rY, (int)Math.Ceiling(perWidth), (int)Math.Ceiling(perHeight)));

                        using (var rMask = new Mat(blackMat, r))
                        {
                            Cv2.Canny(rMask, rMask, tl, th);
                            Cv2.ImShow(string.Format("rMask{0}_{1}", i, j), rMask);

                            rMask.Dispose();
                        }
                    }
                }

                blackMat.Dispose();
            }
        }

        private void btnFindBrightSpot_Click(object sender, EventArgs e)
        {
            var src = GetImage("原图");
            if (src != null)
            {
                _osramEviyosVisionAnalysis.IsCv2ShowImage = false;
                string bitmap;
                string error;
                var isOk = _osramEviyosVisionAnalysis.FindBrightSpot(src, out bitmap, out error);

                if (!string.IsNullOrEmpty(bitmap))
                {
                    // 从Base64字符串转换回字节数组
                    var convertedBytes = Convert.FromBase64String(bitmap);

                    // 从字节数组转换回Bitmap
                    using (var ms = new MemoryStream(convertedBytes))
                    {
                        var convertedBitmap = new Bitmap(ms);
                        // 保存或使用转换后的Bitmap

                        var mat = convertedBitmap.ToMat();
                        AppendImage("处理结果", mat);
                        BindDictionaryToComboBox();
                        mat.Dispose();
                    }
                }

                if (isOk)
                {
                    ShowSuccessTip("判定PASS");
                }
                else
                {
                    ShowErrorTip("判定FAILED: " + error);
                }
            }
        }

        private List<FileInfo> _listFiles = new List<FileInfo>();
        private int _currentIndex = -1;

        private void btnOpenOKFiles_Click(object sender, EventArgs e)
        {
            _listFiles.Clear();
            var folder = @"E:\Projects\万级像素\自动线终检\图像测试\20250218\OK_ImageType1_From_20250201";
            var files = Directory.GetFiles(folder);

            foreach (var t in files)
            {
                var fileInfo = new FileInfo(t);
                _listFiles.Add(fileInfo);
            }
        }

        private void btnAnalysisNext_Click(object sender, EventArgs e)
        {
            if (!_listFiles.Any())
            {
                ShowInfoTip("没有图像");
                return;
            }

            _currentIndex++;
            if (_currentIndex < _listFiles.Count)
            {
                Analysis(_listFiles[_currentIndex].FullName);
                ShowInfoTip("当前第" + (_currentIndex + 1) + "张；" + "共" + (_listFiles.Count) + "张；");
            }
            else
            {
                ShowInfoTip("已经到做后一张了");
                _currentIndex = -1;
            }
        }

        private void btnAnalysisAll_Click(object sender, EventArgs e)
        {
            if (!_listFiles.Any())
            {
                ShowInfoTip("没有图像");
                return;
            }


            var listNgIndex = new List<int>();
            for (var i = 0; i < _listFiles.Count; i++)
            {
                uiCheckBox1.Checked = false;

                var isOk = Analysis(_listFiles[i].FullName);
                Console.WriteLine(@"正在分析：{0}/{1}，结果：{2}", i + 1, _listFiles.Count, isOk ? "OK" : "NG");
                if (!isOk)
                {
                    listNgIndex.Add(i);
                    File.Copy(_listFiles[i].FullName, string.Format(@"E:\Projects\万级像素\自动线终检\图像测试\20250218\OK_ImageType1_From_20250201_测试异常\{0}", _listFiles[i].Name));
                }
            }

            for (var i = 0; i < listNgIndex.Count; i++)
            {
                Console.WriteLine(@"OK图像测试异常，第{0}张，文件名：{1}", i, _listFiles[i].Name);
            }
        }
    }
}
