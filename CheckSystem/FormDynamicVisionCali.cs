using CheckSystem.VisionDetection.Vision;
using CommonUtility;
using CommonUtility.FileOperator;
using CommonUtility.HikSdk;
using Controller;
using NationalInstruments.UI;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using StateMachine;
using Sunny.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using ImageProcessing = CheckSystem.VisionDetection.Vision.ImageProcessing;
using Range = NationalInstruments.Vision.Range;

namespace CheckSystem
{
    public partial class FormDynamicVisionCali : UIForm
    {
        private readonly ImageViewer _mainImageViewer = new ImageViewer { Dock = DockStyle.Fill };
        private State _state;
        private DeviceConfigPara _editPara;
        private string _nowBuffBase64;
        private string _nowBuffName;
        private string _nowCameraSn;

        public FormDynamicVisionCali(State state, DeviceConfigPara editPara)
        {
            InitializeComponent();
            _editPara = editPara;
            _state = state;
            Style = UIStyle.Gray;
            Load += FormDynamicVisionCali_Load;
        }

        private void FormDynamicVisionCali_Load(object sender, EventArgs e)
        {
            Text += Text + "_['" + _editPara.ProcessNo + "_" + _editPara.Name + "']";
            panel1.Controls.Add(_mainImageViewer);
            InitImageViewer(_mainImageViewer);
            LoadCameraBuff();
        }

        private void LoadCameraBuff()
        {
            {
                // 获取与OkFormat匹配的所有Roi
                var matchedRois = _state.DeviceConfig.Rois
                    .Where(f => f.Name != null && f.Name.ToLower() == _editPara.OkFormat.ToLower())
                    .ToList();

                // 按Group分组
                var groupedRois = matchedRois
                    .GroupBy(g => g.Group)
                    .ToList();

                // 找到数量最多的分组
                var maxGroup = groupedRois
                    .OrderByDescending(grp => grp.Count())
                    .FirstOrDefault();

                // 生成该组的数组
                DeviceConfigRoi[] tpRoi = maxGroup != null ? maxGroup.ToArray() : new DeviceConfigRoi[0];
                for (var i = 0; i < tpRoi.Length; i++)
                {
                    var rect = new RectangleContour();
                    rect.Left = double.Parse(tpRoi[i].RectX);
                    rect.Top = double.Parse(tpRoi[i].RectY);
                    rect.Width = double.Parse(tpRoi[i].RectWidth);
                    rect.Height = double.Parse(tpRoi[i].RectHeight);
                    _mainImageViewer.Roi.Add(rect);
                }
            }

            var cameraTreeNode = new TreeNode { Text = @"图像缓存列表" };

            if (_state != null)
            {
                var visions = _state.LstControllers.Where(f => (f != null && f.GetType() == typeof(VisionAnalysis))).Select(t => t as VisionAnalysis);
                foreach (var vision in visions)
                {
                    var keys = vision.DynamicImageBuff.Keys.ToList();
                    if (keys.Any())
                    {
                        var eachCameraNode = new TreeNode(string.Format("{0}_[{1}]", vision.Name, vision.CameraSn)) { Tag = vision.CameraSn };
                        for (var i = 0; i < keys.Count; i++)
                        {
                            var key = keys[i];
                            var keyNode = new TreeNode(string.Format("缓存名称_[{0}]", key)) { Tag = key };
                            eachCameraNode.Nodes.Add(keyNode);
                        }
                        cameraTreeNode.Nodes.Add(eachCameraNode);
                    }
                }
            }

            treeViewCamerasList.Nodes.Add(cameraTreeNode);
            treeViewCamerasList.ExpandAll();
            treeViewCamerasList.BeforeCollapse += (ns1, ne1) => { ne1.Cancel = true; };
            treeViewCamerasList.BeforeSelect += (bs, be) =>
            {
                var nodeLevel = be.Node.Level;
                if (nodeLevel != 2)
                    return;

                if (!UIMessageBox.ShowAsk("是否选择该动画"))
                {
                    UIMessageTip.Show("取消选择");
                    be.Cancel = true;
                    return;
                }

                _nowBuffName = string.Empty;
                _nowCameraSn = string.Empty;
                _nowBuffBase64 = string.Empty;

                toolStripCmbImgList.Items.Clear();
                cmbGroup.Items.Clear();

                var parentBuffNameNode = be.Node.Parent;

                var buffName = be.Node.Tag.ToString();
                var cameraSn = parentBuffNameNode.Tag.ToString();
                _nowBuffName = buffName;
                _nowCameraSn = cameraSn;

                var vision = _state.LstControllers.Where(f => (f != null && f.GetType() == typeof(VisionAnalysis))).First(t => ((VisionAnalysis)t).CameraSn == cameraSn) as VisionAnalysis;
                for (var j = 0; j < vision.DynamicImageBuff[buffName].Length; j++)
                    toolStripCmbImgList.Items.Add(("图像缓存" + j));
                cmbGroup.SelectedIndex = -1;
                //toolStripCmbImgList.SelectedIndex = toolStripCmbImgList.Items.Count > 0 ? 0 : -1;
                toolStripCmbImgList.SelectedIndex = -1;
                CopySrcToShow();
            };

            toolStripCmbImgList.SelectedIndexChanged += ToolStripCmbImgList_SelectedIndexChanged;
            cmbGroup.SelectedIndexChanged += CmbGroup_SelectedIndexChanged;
        }

        private void CmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGroup.Items.Count > 0 && cmbGroup.SelectedIndex != -1)
            {
                if (_imgIndexInGroup.Count > 0 && cmbGroup.SelectedIndex < _imgIndexInGroup.Count)
                {
                    var imgIndex = _imgIndexInGroup[cmbGroup.SelectedIndex];

                    if (toolStripCmbImgList.Items.Count > 0 && toolStripCmbImgList.Items.Count > imgIndex)
                    {
                        toolStripCmbImgList.SelectedIndex = imgIndex;
                        UIMessageTip.Show("已选择第" + imgIndex + "张图像");
                        return;
                    }
                }
            }

            UIMessageTip.Show("未选择图像");
        }

        private void ToolStripCmbImgList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripCmbImgList.Items.Count > 0 && toolStripCmbImgList.SelectedIndex != -1)
            {
                if (!string.IsNullOrEmpty(_nowCameraSn) && !string.IsNullOrEmpty(_nowBuffName))
                {
                    var cameraSn = _nowCameraSn;
                    var buffName = _nowBuffName;
                    var vision = _state.LstControllers.Where(f => (f != null && f.GetType() == typeof(VisionAnalysis))).First(t => ((VisionAnalysis)t).CameraSn == cameraSn) as VisionAnalysis;
                    if (vision.DynamicImageBuff.ContainsKey(buffName) && toolStripCmbImgList.SelectedIndex < vision.DynamicImageBuff[buffName].Length)
                        _nowBuffBase64 = vision.DynamicImageBuff[buffName][toolStripCmbImgList.SelectedIndex];
                    else
                        _nowBuffBase64 = string.Empty;

                }
            }
            else if (toolStripCmbImgList.SelectedIndex == -1)
            {
                _nowBuffBase64 = string.Empty;
            }

            CopySrcToShow();
        }

        private void CopySrcToShow()
        {
            var imgMat = new Mat();
            if (!string.IsNullOrEmpty(_nowBuffBase64))
            {
                var imgBitMap = MyCamera.Base64StringToBitmap(_nowBuffBase64);

                if (imgBitMap != null)
                {
                    imgMat?.Dispose();
                    imgMat = null;
                    imgMat = BitmapConverter.ToMat(imgBitMap);
                    imgBitMap?.Dispose();
                }
                else
                {
                    imgMat?.Dispose();
                    imgMat = null;
                    imgMat = new Mat(800, 1024, MatType.CV_8UC1, Scalar.Black);
                    var tpb = BitmapConverter.ToBitmap(imgMat);
                    _nowBuffBase64 = MyCamera.BitmapToBase64String(tpb);
                    tpb.Dispose();
                }
            }
            else
            {
                imgMat?.Dispose();
                imgMat = null;
                imgMat = new Mat(800, 1024, MatType.CV_8UC1, Scalar.Black);
                var tpb = BitmapConverter.ToBitmap(imgMat);
                _nowBuffBase64 = MyCamera.BitmapToBase64String(tpb);
                tpb.Dispose();
            }

            var img = MyCamera.MatToVisionImage(imgMat);
            imgMat?.Dispose();
            imgMat = null;
            Algorithms.Copy(img, _mainImageViewer.Image);
            img.Dispose();

            var tpShapes = new List<RectangleContour>();

            for (var i = 0; i < _mainImageViewer.Roi.Count; i++)
            {
                if (_mainImageViewer.Roi[i].Shape is RectangleContour shape)
                    tpShapes.Add(shape);
            }

            _mainImageViewer.Roi.Clear();
            foreach (var sp in tpShapes)
            {
                var left = sp.Left;
                if (left < 0 || left > _mainImageViewer.Image.Width || (left + sp.Width > _mainImageViewer.Image.Width && (left + sp.Width - _mainImageViewer.Image.Width) > sp.Width * 0.6))
                    continue;

                var top = sp.Top;
                if (top < 0 || top > _mainImageViewer.Image.Height || (top + sp.Height > _mainImageViewer.Image.Height && (top + sp.Height - _mainImageViewer.Image.Height) > sp.Height * 0.6))
                    top = 0;

                _mainImageViewer.Roi.Add(sp);
            }

            //if (ToCalibrateImg != null)
            //    ToCalibrateImg.Dispose();
            //ToCalibrateImg = new VisionImage(_mainImageViewer.Image.Type);
            //Algorithms.Copy(_mainImageViewer.Image, ToCalibrateImg);
        }

        private void InitImageViewer(ImageViewer imageViewer)
        {
            ImageShowTool(imageViewer);
            imageViewer.SizeChanged += imageViewer_SizeChanged;
            //imageViewer.RoiChanged += imageViewer_RoiChanged;
        }

        private static void ImageShowTool(ImageViewer imageViewer)
        {
            imageViewer.ToolsShown = ViewerTools.ZoomIn |
                                     ViewerTools.ZoomOut |
                                     ViewerTools.Pan |
                                     ViewerTools.Selection;
            imageViewer.ActiveTool = ViewerTools.Selection;
            imageViewer.ZoomToFit = true;
            imageViewer.ShowToolbar = true;
            imageViewer.ShowScrollbars = true;
            imageViewer.ShowImageInfo = true;
        }

        private static void imageViewer_SizeChanged(object sender, EventArgs e)
        {
            var imageViewer = sender as ImageViewer;
            if (imageViewer != null)
                ImageShowTool(imageViewer);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_toSave.Any() || _toSave.Count == 1)
            {
                UIMessageTip.ShowError("没有可保存的结果，请先执行标定学习");
                return;
            }

            if (!UIMessageBox.ShowAsk("确定要保存吗？"))
            {
                UIMessageTip.Show("取消保存");
                return;
            }

            var tpRoi = _state.DeviceConfig.Rois.ToList();
            tpRoi.RemoveAll(f => f.Name.ToLower() == _editPara.OkFormat.ToLower());

            for (var groupIndex = 0; groupIndex < _toSave.Count; groupIndex++)
            {
                for (var i = 0; i < _toSave[groupIndex].Length; i++)
                {
                    var caliRoi = _toSave[groupIndex][i];
                    var rect = ImageProcessing.GetRectByString(caliRoi.Rect);
                    var max = Math.Round(double.Parse(caliRoi.Value.TrimEnd('%')), 2, MidpointRounding.AwayFromZero);
                    var min = Math.Round((double.Parse(caliRoi.Min.TrimEnd('%'))) * max, 2, MidpointRounding.AwayFromZero);

                    var roi = new DeviceConfigRoi
                    {
                        Name = _editPara.OkFormat,
                        Group = groupIndex.ToString(),
                        RectX = Math.Round(rect.Left, 0, MidpointRounding.AwayFromZero).ToString(),
                        RectY = Math.Round(rect.Top, 0, MidpointRounding.AwayFromZero).ToString(),
                        RectWidth = Math.Round(rect.Width, 0, MidpointRounding.AwayFromZero).ToString(),
                        RectHeight = Math.Round(rect.Height, 0, MidpointRounding.AwayFromZero).ToString(),
                        Min = min.ToString(),
                        Max = max.ToString()
                    };
                    tpRoi.Add(roi);
                }
            }

            _state.DeviceConfig.Rois = tpRoi.ToArray();
            var toSavePara = XmlHelper.Deserialize<DeviceConfig>(_state.XmlFilePath);
            toSavePara.Rois = tpRoi.ToArray();
            XmlHelper.SerializeToFile(toSavePara, _state.XmlFilePath, Encoding.UTF8);
            UIMessageTip.Show("保存成功");
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnFindCircle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_nowBuffBase64))
            {
                ShowErrorTip("请先选择一张图片");
                return;
            }

            if (!UIMessageBox.ShowAsk("识别LED将会清空当前所有LED，请确认是否执行"))
            {
                ShowInfoTip("取消识别");
                return;
            }

            {
                var srcBitmap = MyCamera.Base64StringToBitmap(_nowBuffBase64);
                var srcMat = BitmapConverter.ToMat(srcBitmap);
                var srcImg = MyCamera.MatToVisionImage(srcMat);
                srcBitmap.Dispose();
                srcMat.Dispose();

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

                ToolStripCmbImgList_SelectedIndexChanged(null, null);
            }
        }

        private void btnAddRect_Click(object sender, EventArgs e)
        {
            if (_mainImageViewer.Image.Width != 0 && _mainImageViewer.Image.Height != 0)
            {
                int spWidth = 50, spHeight = 50;
                var spX = (_mainImageViewer.Image.Width - spWidth) / 2;
                var spY = (_mainImageViewer.Image.Height - spHeight) / 2;

                var shape = new RectangleContour(spX, spY, spWidth, spHeight);
                _mainImageViewer.Roi.Add(shape);
            }
        }

        private readonly List<VisionConfigVisionFuncCameraAnalysisShapesShape[]> _toSave = new List<VisionConfigVisionFuncCameraAnalysisShapesShape[]>();
        private List<int> _imgIndexInGroup = new List<int>();

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

        private void btnLearn_Click(object sender, EventArgs e)
        {
            #region debug 测试

            if (toolStripCmbImgList.Items.Count <= 0 || string.IsNullOrEmpty(_nowBuffName))
            {
                UIMessageTip.ShowError("请先选择需要标定的图像缓存");
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

            var ImgCount = toolStripCmbImgList.Items.Count;

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
                _imgIndexInGroup.Clear();
                dgvData.ClearAll();

                var tempStandard = new List<LedState>();

                var grayClass = new VisionConfigVisionFuncCameraAnalysisShapesShape[_mainImageViewer.Roi.Count];
                _mainImageViewer.Image.Overlays.Default.Clear();

                var vision = _state.LstControllers.Where(f => (f != null && f.GetType() == typeof(VisionAnalysis))).First(t => ((VisionAnalysis)t).CameraSn == _nowCameraSn) as VisionAnalysis;
                var standardImgBuffBase64 = vision.DynamicImageBuff[_nowBuffName][allOnIndex];
                var standardImgBitmap = MyCamera.Base64StringToBitmap(standardImgBuffBase64);
                var standardImgMat = BitmapConverter.ToMat(standardImgBitmap);
                standardImgBitmap?.Dispose();

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

                    var rect = ImageProcessing.GetRectByString(grayClass[ri].Rect);
                    var originRect = new Rect((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
                    var matWidth = standardImgMat.Width;
                    var matHeight = standardImgMat.Height;
                    var roiMat = new Mat(standardImgMat, CommonUtility.HikSdk.MyCamera.GetRectInMat(matWidth, matHeight, originRect));
                    var meanVal = Cv2.Mean(roiMat);

                    var grayPer = Math.Round(meanVal.Val0, 1, MidpointRounding.AwayFromZero);
                    grayPer = CommonUtility.HikSdk.MyCamera.GetLxByRgb(meanVal);
                    grayClass[ri].Value = grayPer + "%";
                    grayClass[ri].Max = "100%";
                    grayClass[ri].Min = onOffThreshold + "%";

                    tempStandard.Add(new LedState { Gray = double.Parse(grayClass[ri].Value.TrimEnd('%')), Min = double.Parse(grayClass[ri].Min.TrimEnd('%')), Max = double.Parse(grayClass[ri].Max.TrimEnd('%')) });
                }

                //standardImgMat.Dispose();
                //standardImgMat = null;

                //var imgLedStates = new List<ImgLedState>();
                //for (var i = 0; i < endIndex; i++)
                //{
                //    imgLedStates.Add(new ImgLedState { ImgIndex = i });

                //    for (var j = 0; j < _mainImageViewer.Roi.Count; j++)
                //        imgLedStates[i].LedState.Add(new LedState { Gray = -9999, Max = -9999, Min = -9999 });
                //}
                //var listStr = new List<string>();
                //var groupIndex = 0;
                //{
                //    var index1 = groupIndex;

                //    var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
                //    Parallel.For((long)startIndex, endIndex, options, i =>
                //    {
                //        var g = grayClass;

                //        var items = new ConcurrentBag<VisionConfigVisionFuncCameraAnalysisShapesShape>(g);
                //        var imgIndex = i;

                //        var tpBase64 = vision.DynamicImageBuff[_nowBuffName][imgIndex];
                //        var tpImgBitmap = MyCamera.Base64StringToBitmap(tpBase64);
                //        var tpImgMat = BitmapConverter.ToMat(tpImgBitmap);
                //        tpImgBitmap?.Dispose();

                //        Parallel.ForEach(items, (shape, state, index) =>
                //        {
                //            var rect = ImageProcessing.GetRectByString(shape.Rect);
                //            int mRow, mCol;
                //            var originRect = new Rect((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
                //            var matWidth = tpImgMat.Width;
                //            var matHeight = tpImgMat.Height;
                //            var roiMat = new Mat(tpImgMat, MyCamera.GetRectInMat(matWidth, matHeight, originRect));
                //            var meanVal = Cv2.Mean(roiMat);

                //            var grayPer = Math.Round(meanVal.Val0, 1, MidpointRounding.AwayFromZero);
                //            grayPer = MyCamera.GetLxByRgb(meanVal);

                //            imgLedStates[(int)imgIndex].LedState[(int)index].Gray = grayPer;
                //        });

                //        tpImgMat.Dispose();
                //        tpImgMat = null;
                //    });
                //}

                //dgvData.AddColumn("ImgIndex", "ImgIndex");
                //for (var i = 0; i < tempStandard.Count; i++)
                //    dgvData.AddColumn("Roi" + i, "Roi" + i);

                //for (var i = 0; i < imgLedStates.Count; i++)
                //{
                //    var objs = new object[tempStandard.Count + 1];
                //    objs[0] = i;

                //    for (var j = 0; j < imgLedStates[i].LedState.Count; j++)
                //        objs[j + 1] =
                //            string.Format(
                //                "{0}/{1}={2}", imgLedStates[i].LedState[j].Gray, tempStandard[j].Gray, Math.Round(
                //                    imgLedStates[i].LedState[j].Gray / tempStandard[j].Gray, 1,
                //                    MidpointRounding.AwayFromZero));//Math.Round(imgLedStates[i].LedState[j].Gray / tempStandard[j].Gray, 1, MidpointRounding.AwayFromZero);

                //    dgvData.AddRow(objs);
                //}

                //foreach (var t in imgLedStates)
                //{
                //    var str = string.Empty;

                //    var isPass = false;
                //    for (var i = 0; i < t.LedState.Count; i++)
                //    {
                //        var thisGray = t.LedState[i].Gray;
                //        var standardGray = tempStandard[i].Gray;
                //        var unif = thisGray / standardGray;

                //        if (unif <= 0.15)
                //        {
                //            str += "0";
                //        }
                //        else if (unif > 0.15 && unif <= onOffThreshold)
                //        {
                //            isPass = true;
                //            continue;
                //        }
                //        else
                //        {
                //            str += "1";
                //        }
                //    }

                //    if (str.Length == t.LedState.Count)
                //    {
                //        if (!isPass)
                //        {
                //            if (!listStr.Any())
                //            {
                //                cmbGroup.Items.Add(t.ImgIndex);
                //                _imgIndexInGroup.Add(t.ImgIndex);

                //                listStr.Add(string.Format("第{0}张:{1}", t.ImgIndex.ToString().PadLeft(4, '0'), str));

                //                var tempGrayClass =
                //                    new VisionConfigVisionFuncCameraAnalysisShapesShape[grayClass.Length];
                //                for (var i = 0; i < t.LedState.Count; i++)
                //                {
                //                    tempGrayClass[i] = new VisionConfigVisionFuncCameraAnalysisShapesShape
                //                    {
                //                        Rect = grayClass[i].Rect,
                //                        Type = grayClass[i].Type,
                //                        Min = onOffThreshold + "%",
                //                        Max = str[i].ToString() == "0" ? "0%" : "100%",
                //                        Value = imgLedStates[(int)t.ImgIndex].LedState[(int)i].Gray + "%"
                //                    };
                //                }

                //                _toSave.Add(tempGrayClass);
                //            }
                //            else
                //            {
                //                if (listStr[listStr.Count - 1].Split(':')[1] != str)
                //                {
                //                    cmbGroup.Items.Add(t.ImgIndex);
                //                    _imgIndexInGroup.Add(t.ImgIndex);
                //                    listStr.Add(string.Format("第{0}张:{1}", t.ImgIndex.ToString().PadLeft(4, '0'), str));

                //                    var tempGrayClass =
                //                        new VisionConfigVisionFuncCameraAnalysisShapesShape[grayClass.Length];
                //                    for (var i = 0; i < t.LedState.Count; i++)
                //                    {
                //                        tempGrayClass[i] = new VisionConfigVisionFuncCameraAnalysisShapesShape
                //                        {
                //                            Rect = grayClass[i].Rect,
                //                            Type = grayClass[i].Type,
                //                            Min = onOffThreshold + "%",
                //                            Max = str[i].ToString() == "0" ? "0%" : "100%",
                //                            Value = imgLedStates[(int)t.ImgIndex].LedState[(int)i].Gray + "%"
                //                        };
                //                    }

                //                    _toSave.Add(tempGrayClass);
                //                }
                //            }
                //        }
                //    }
                //}

                //foreach (var t in listStr)
                //    txtLearnView.AppendText(t + "\r\n");

                // 差帧测试
                {
                    var listLedState = new List<string>();

                    Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
                    var tpShapes = new List<RectangleContour>();

                    for (var i = 0; i < _mainImageViewer.Roi.Count; i++)
                    {
                        if (_mainImageViewer.Roi[i].Shape is RectangleContour shape)
                            tpShapes.Add(shape);
                    }

                    using (var standartGray = standardImgMat.Clone())
                    {
                        if (standartGray.Channels() != 0)
                            Cv2.CvtColor(standartGray, standartGray, ColorConversionCodes.BGR2GRAY);
                        Cv2.GaussianBlur(standartGray, standartGray, new Size(5, 5), 0);

                        for (var imgIndex = 0; imgIndex < ImgCount; imgIndex++)
                        {
                            var lstOriginGray = new List<double>();
                            var lstNewGray = new List<double>();
                            var standardGrayAvg = 0d;

                            {
                                var grayShow = standartGray.Clone();
                                Cv2.CvtColor(grayShow, grayShow, ColorConversionCodes.GRAY2BGR);

                                for (int i = 0; i < tpShapes.Count; i++)
                                {
                                    var shape = tpShapes[i];
                                    var rect = shape;
                                    int mRow, mCol;
                                    var originRect = new Rect((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
                                    var matWidth = grayShow.Width;
                                    var matHeight = grayShow.Height;
                                    var roiMat = new Mat(grayShow, MyCamera.GetRectInMat(matWidth, matHeight, originRect));
                                    var meanVal = Cv2.Mean(roiMat);

                                    var grayPer = Math.Round(meanVal.Val0, 1, MidpointRounding.AwayFromZero);
                                    standardGrayAvg += grayPer;
                                    Cv2.Rectangle(grayShow, originRect, Scalar.Green);
                                    Cv2.PutText(grayShow, grayPer.ToString(), new Point(originRect.X, originRect.Y - 1), HersheyFonts.HersheySimplex, 0.3, Scalar.Green);
                                }
                                standardGrayAvg = standardGrayAvg / tpShapes.Count;
                                //Cv2.ImShow(string.Format("standartGray"), grayShow);
                                //Cv2.WaitKey();
                            }

                            var tpBase64 = vision.DynamicImageBuff[_nowBuffName][imgIndex];
                            var tpImgBitmap = MyCamera.Base64StringToBitmap(tpBase64);
                            var tpImgMat = BitmapConverter.ToMat(tpImgBitmap);

                            if (tpImgMat.Channels() != 0)
                                Cv2.CvtColor(tpImgMat, tpImgMat, ColorConversionCodes.BGR2GRAY);
                            Cv2.GaussianBlur(tpImgMat, tpImgMat, new Size(5, 5), 0);

                            {
                                var grayShow = tpImgMat.Clone();
                                Cv2.CvtColor(grayShow, grayShow, ColorConversionCodes.GRAY2BGR);

                                for (int i = 0; i < tpShapes.Count; i++)
                                {
                                    var shape = tpShapes[i];
                                    var rect = shape;
                                    int mRow, mCol;
                                    var originRect = new Rect((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
                                    var matWidth = grayShow.Width;
                                    var matHeight = grayShow.Height;
                                    var roiMat = new Mat(grayShow, MyCamera.GetRectInMat(matWidth, matHeight, originRect));
                                    var meanVal = Cv2.Mean(roiMat);

                                    var grayPer = Math.Round(meanVal.Val0, 1, MidpointRounding.AwayFromZero);
                                    Cv2.Rectangle(grayShow, originRect, Scalar.Green);
                                    Cv2.PutText(grayShow, grayPer.ToString(), new Point(originRect.X, originRect.Y - 1), HersheyFonts.HersheySimplex, 0.3, Scalar.Green);

                                    lstOriginGray.Add(grayPer);
                                }

                                //Cv2.ImShow("gray image at " + imgIndex, grayShow);
                                //Cv2.WaitKey();
                            }

                            using (var subMat = new Mat())
                            {
                                Cv2.Absdiff(standartGray, tpImgMat, subMat); // 计算绝对值差分
                                //Cv2.ImShow(string.Format("standard-img{0}-Absdiff", imgIndex), subMat);
                                Cv2.Threshold(subMat, subMat, standardGrayAvg, 255, ThresholdTypes.Binary); // 过滤微小变化
                                //Cv2.ImShow(string.Format("standard-img{0}-Threshold", imgIndex), subMat);
                                //Cv2.Subtract(standartGray, tpImgMat, subMat);
                                //Cv2.ImShow(string.Format("standard-img{0}", imgIndex), subMat);
                                //Cv2.WaitKey();

                                using (var erodeMat = new Mat())
                                {
                                    // 腐蚀操作
                                    Cv2.Erode(subMat, erodeMat, element);
                                    //Cv2.ImShow("erode image at " + imgIndex, erodeMat);
                                    // 再膨胀
                                    //Cv2.Dilate(erodeMat, erodeMat, element);
                                    //Cv2.ImShow("Dilate image at " + imgIndex, erodeMat);

                                    var strStateStr = string.Empty;

                                    {
                                        var grayShow = erodeMat.Clone();
                                        Cv2.CvtColor(grayShow, grayShow, ColorConversionCodes.GRAY2BGR);
                                        for (var i = 0; i < tpShapes.Count; i++)
                                        {
                                            var shape = tpShapes[i];
                                            var rect = shape;
                                            int mRow, mCol;
                                            var originRect = new Rect((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
                                            var matWidth = grayShow.Width;
                                            var matHeight = grayShow.Height;
                                            var roiMat = new Mat(grayShow, MyCamera.GetRectInMat(matWidth, matHeight, originRect));
                                            var meanVal = Cv2.Mean(roiMat);

                                            var grayPer = Math.Round(meanVal.Val0, 1, MidpointRounding.AwayFromZero);
                                            Cv2.Rectangle(grayShow, originRect, Scalar.Green);
                                            Cv2.PutText(grayShow, grayPer.ToString(), new Point(originRect.X, originRect.Y - 1), HersheyFonts.HersheySimplex, 0.3, Scalar.Green);

                                            lstNewGray.Add(grayPer);

                                            if (imgIndex == 6 && (i >= 9 && i <= 11))
                                            {
                                                Console.WriteLine("breat at " + imgIndex);
                                            }

                                            strStateStr += (lstNewGray[i] / lstOriginGray[i] <= 0.6 ? "1" : "0");
                                        }

                                        //if (listLedState.Any())
                                        //{
                                        //    var last = listLedState.Last();
                                        //    if (last != strStateStr)
                                        //    {
                                        //        listLedState.Add(strStateStr);
                                        //        cmbGroup.Items.Add(imgIndex);
                                        //        _imgIndexInGroup.Add(imgIndex);
                                        //        Cv2.ImShow(string.Format("img{0}-standard-erode", imgIndex), grayShow);
                                        //        Cv2.WaitKey();
                                        //    }
                                        //    else
                                        //    {
                                        //        //if (imgIndex == 6)
                                        //        //{
                                        //        //    Console.WriteLine("breat at " + imgIndex);
                                        //        //    Cv2.ImShow(string.Format("img{0}-standard-erode", imgIndex), grayShow);
                                        //        //    Cv2.WaitKey();
                                        //        //}
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    cmbGroup.Items.Add(imgIndex);
                                        //    _imgIndexInGroup.Add(imgIndex);
                                        //    listLedState.Add(strStateStr);
                                        //    Cv2.ImShow(string.Format("img{0}-standard-erode", imgIndex), grayShow);
                                        //    Cv2.WaitKey();
                                        //}

                                        Cv2.ImShow(string.Format("img{0}-standard-erode", imgIndex), grayShow);
                                        Cv2.WaitKey();
                                    }
                                }
                            }

                            Cv2.DestroyAllWindows();

                            tpImgBitmap?.Dispose();
                            tpImgMat?.Dispose();
                        }
                    }
                }


            }

            st.Stop();
            this.ShowSuccessTip(string.Format(@"学习结束，耗时：{0}", st.ElapsedMilliseconds));

            Enabled = true;
        }

        #endregion
    }
}
