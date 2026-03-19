using CommonUtility;
using CommonUtility.FileOperator;
using CommonUtility.HikSdk;
using Controller;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using StateMachine;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImageProcessing = CheckSystem.VisionDetection.Vision.ImageProcessing;

namespace CheckSystem
{
    public partial class FormStaticVisionCali : UIForm
    {
        private List<string> ListStep { get; set; }
        private VisionImage ToCalibrateImg { get; set; }

        private State _state;
        private readonly List<Shape> _calibateRegions = new List<Shape>();
        private readonly List<Shape> _interestedRois = new List<Shape>();
        private readonly ImageViewer _mainImageViewer = new ImageViewer { Dock = DockStyle.Fill };
        private string _nowBuffBase64 = string.Empty;
        private string _nowEditCameraSn = string.Empty;
        private string _nowEditBuffName = string.Empty;
        private int _nowStep;
        private DeviceConfigPara _editPara;

        public FormStaticVisionCali(State state, DeviceConfigPara editPara)
        {
            InitializeComponent();
            _state = state;
            _editPara = editPara;
            uiSplitContainerCalibration.Panel1.Controls.Add(_mainImageViewer);
            Style = UIStyle.Gray;
            Load += FormStaticVisionCali_Load;
        }

        private void FormStaticVisionCali_Load(object sender, EventArgs e)
        {
            Text += Text + "_['" + _editPara.ProcessNo + "_" + _editPara.Name + "']";
            InitImageViewer(_mainImageViewer);
            LoadCamerabBuff();
            InitStep();

            if (_state.DeviceConfig.Rois != null)
            {
                _interestedRois.AddRange(_state.DeviceConfig.Rois?.ToList().FindAll(f => f.Name.ToLower() == _editPara.OkFormat?.ToLower()).Select(item => ImageProcessing.GetRectByString(string.Format("{0},{1},{2},{3}", item.RectX, item.RectY, item.RectWidth, item.RectHeight))));
            }
        }

        private void LoadCamerabBuff()
        {
            var cameraTreeNode = new TreeNode { Text = @"图像缓存列表" };

            if (_state != null)
            {
                var visions = _state.LstControllers.Where(f => (f != null && f.GetType() == typeof(VisionAnalysis))).Select(t => t as VisionAnalysis);
                foreach (var vision in visions)
                {
                    var keys = vision.StaticImageBuff.Keys.ToList();
                    if (keys.Any())
                    {
                        var eachCameraNode = new TreeNode(string.Format("{0}_[{1}]", vision.Name, vision.CameraSn)) { Tag = vision.CameraSn };

                        for (var i = 0; i < keys.Count; i++)
                        {
                            var key = keys[i];
                            var keyNode = new TreeNode(string.Format("缓存名称_[{0}]", key)) { Tag = key };
                            for (var j = 0; j < vision.StaticImageBuff[key].Length; j++)
                            {
                                var imageNode = new TreeNode("图像缓存" + j);
                                imageNode.Tag = j;
                                keyNode.Nodes.Add(imageNode);
                            }

                            eachCameraNode.Nodes.Add(keyNode);
                        }

                        cameraTreeNode.Nodes.Add(eachCameraNode);
                    }
                }
            }

            treeViewCamerasList.Nodes.Add(cameraTreeNode);
            treeViewCamerasList.ExpandAll();
            treeViewCamerasList.BeforeCollapse += (ns1, ne1) => { ne1.Cancel = true; };
            treeViewCamerasList.AfterSelect += (afs, afe) =>
            {
                var nodeLevel = afe.Node.Level;
                if (nodeLevel == 3)
                {
                    var parentBuffNameNode = afe.Node.Parent;
                    var parentCameraNode = afe.Node.Parent.Parent;

                    var buffIndex = (int)afe.Node.Tag;
                    var buffName = parentBuffNameNode.Tag.ToString();
                    var cameraSn = parentCameraNode.Tag.ToString();

                    var vision = _state.LstControllers.Where(f => (f != null && f.GetType() == typeof(VisionAnalysis))).First(t => ((VisionAnalysis)t).CameraSn == cameraSn) as VisionAnalysis;
                    _nowBuffBase64 = vision?.StaticImageBuff[buffName][buffIndex].Item2;
                    _nowEditCameraSn = cameraSn;
                    _nowEditBuffName = buffName;
                }
                else
                {
                    _nowBuffBase64 = string.Empty;
                    _nowEditCameraSn = string.Empty;
                    _nowEditBuffName = string.Empty;
                }

                CopySrcToShow();
            };
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
                    imgMat = new Mat(768, 1024, MatType.CV_8UC1, Scalar.Black);
                    var tpb = BitmapConverter.ToBitmap(imgMat);
                    _nowBuffBase64 = MyCamera.BitmapToBase64String(tpb);
                    tpb.Dispose();
                }
            }
            else
            {
                imgMat?.Dispose();
                imgMat = null;
                imgMat = new Mat(768, 1024, MatType.CV_8UC1, Scalar.Black);
                var tpb = BitmapConverter.ToBitmap(imgMat);
                _nowBuffBase64 = MyCamera.BitmapToBase64String(tpb);
                tpb.Dispose();
            }

            var img = MyCamera.MatToVisionImage(imgMat);
            imgMat?.Dispose();
            imgMat = null;
            Algorithms.Copy(img, _mainImageViewer.Image);
            img.Dispose();

            if (ToCalibrateImg != null)
                ToCalibrateImg.Dispose();
            ToCalibrateImg = new VisionImage(_mainImageViewer.Image.Type);
            Algorithms.Copy(_mainImageViewer.Image, ToCalibrateImg);
        }

        private void InitStep()
        {
            stepBar.EnabledChanged += stepBar_EnabledChanged;
            stepBar.ItemIndexChanged += stepBar_ItemIndexChanged;
            cmbLookupTable.EnabledChanged += cmbLookupTable_EnabledChanged;

            stepBar.Style = UIStyle.LayuiGreen;
            ListStep = new List<string> { "步骤1：选择相机并获取图片", "步骤2：增强对比度", "步骤3：设置识别区域", "步骤4：标定图像", "步骤5：标定灰度值范围", "步骤6：保存参数" };
            stepBar.ItemWidth = Size.Width / ListStep.Count;
            stepBar.Interval = 10;
            stepBar.Items.Clear();
            stepBar.Items.Add(ListStep[0]);
            stepBar.Items.Add(ListStep[1]);
            stepBar.ItemIndex = _nowStep;

            cmbLookupTable.DropDownStyle = UIDropDownStyle.DropDownList;
            cmbLookupTable.DataSource =
                EnumOperater.GetEnumValueList<ImageProcessing.LookupTableType>();
            cmbLookupTable.SelectedIndexChanged += cmbLookupTable_SelectedIndexChanged;
        }

        private void stepBar_EnabledChanged(object sender, EventArgs e)
        {
            stepBar.Style = stepBar.Enabled ? UIStyle.LayuiGreen : UIStyle.Gray;
        }

        private void stepBar_ItemIndexChanged(object sender, int value)
        {
            if (stepBar.ItemIndex < ListStep.Count - 1)
            {
                if (stepBar.ItemIndex == stepBar.Items.Count - 1)
                {
                    stepBar.Items.Add(ListStep[stepBar.ItemIndex + 1]);

                    if (stepBar.ItemIndex == 3)
                    {
                        dgvGrays.ClearRows();
                        _mainImageViewer.Roi.Clear();

                        foreach (var t in _interestedRois)
                            _mainImageViewer.Roi.Add(t);
                    }
                }
                else
                {
                    var index = stepBar.ItemIndex;

                    for (var i = ListStep.Count - 1; i >= index + 2; i--)
                    {
                        if (stepBar.Items.Count - 1 >= i)
                            stepBar.Items.RemoveAt(i);
                    }
                }
            }
            else // 最后一步，即保存
            {
                if (dgvGrays.RowCount == 0)
                {
                    ShowErrorTip("请先确定灰度值范围");
                    stepBar.ItemIndex = 4;
                    return;
                }

                if (!ShowAskDialog("确认保存", "是否需要保存标定参数？", UIStyle.Red, true))
                {
                    stepBar.ItemIndex = 3;
                    return;
                }

                if (_state.DeviceConfig.Rois == null)
                    _state.DeviceConfig.Rois = new DeviceConfigRoi[0];
                var tpRoi = _state.DeviceConfig.Rois.ToList();
                tpRoi.RemoveAll(f => f.Name.ToLower() == _editPara.OkFormat.ToLower());

                var roiCount = _mainImageViewer.Roi.Count;
                var roiRegion = new DeviceConfigRoi[roiCount];
                for (var i = 0; i < roiCount; i++)
                {
                    var shape = _mainImageViewer.Roi[i].Shape;
                    var rectangleContour = (RectangleContour)shape;

                    roiRegion[i] = new DeviceConfigRoi
                    {
                        Name = _editPara.OkFormat,
                        Group = "0",
                        RectX = Math.Round(rectangleContour.Left, 0, MidpointRounding.AwayFromZero).ToString(),
                        RectY = Math.Round(rectangleContour.Top, 0, MidpointRounding.AwayFromZero).ToString(),
                        RectWidth = Math.Round(rectangleContour.Width, 0, MidpointRounding.AwayFromZero).ToString(),
                        RectHeight = Math.Round(rectangleContour.Height, 0, MidpointRounding.AwayFromZero).ToString(),
                        Min = dgvGrays.Rows[i].Cells[2].Value.ToString(),
                        Max = dgvGrays.Rows[i].Cells[3].Value.ToString()
                    };

                    tpRoi.Add(roiRegion[i]);
                }

                _state.DeviceConfig.Rois = tpRoi.ToArray();
                var toSavePara = XmlHelper.Deserialize<DeviceConfig>(_state.XmlFilePath);
                toSavePara.Rois = tpRoi.ToArray();
                XmlHelper.SerializeToFile(toSavePara, _state.XmlFilePath, Encoding.UTF8);
                UIMessageTip.Show("保存成功");
            }

            if (stepBar.ItemIndex == 0) // 选择相机并获取图片
            {
                CopySrcToShow();

                treeViewCamerasList.Enabled = true;
                visionGroupBox.Enabled = false;

                dgvGrays.ClearRows();
                _mainImageViewer.Roi.Clear();
            }
            else if (stepBar.ItemIndex == 1) // 增强对比度
            {
                if (_mainImageViewer.Image.Width != 0 && _mainImageViewer.Image.Height != 0)
                {
                    //if (!string.IsNullOrEmpty(lblCameraName.Text))
                    if (true)
                    {
                        treeViewCamerasList.Enabled = false;
                        visionGroupBox.Enabled = true;

                        cmbLookupTable.Enabled = true;
                        btnAddRoi.Enabled = false;
                        btnAddRect.Enabled = false;
                        btnAddShape.Enabled = false;
                        btnAddFromCopy.Enabled = false;
                        btnFindCircle.Enabled = false;
                        btnRefreshGray.Enabled = false;
                        btnClearRoi.Enabled = false;

                        dgvGrays.ClearRows();
                        _mainImageViewer.Roi.Clear();

                        #region bug,从后往前回退时会有问题

                        if (ToCalibrateImg != null)
                            ToCalibrateImg.Dispose();
                        ToCalibrateImg = new VisionImage(_mainImageViewer.Image.Type);
                        Algorithms.Copy(_mainImageViewer.Image, ToCalibrateImg);

                        #endregion

                        var cImg = ImageProcessing.ColorPlaneExtraction(ToCalibrateImg, ImageProcessing.ColorPlaneExtractionType.Red);
                        if (cImg != null)
                        {
                            Algorithms.Copy(cImg, ToCalibrateImg);
                            cImg.Dispose();
                        }

                        cmbLookupTable_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        ShowErrorDialog("请先选择一个相机");
                        stepBar.ItemIndex = 0;
                    }
                }
                else
                {
                    ShowErrorDialog("请先加载一张图片");
                    stepBar.ItemIndex = 0;
                }
            }
            else if (stepBar.ItemIndex == 2) // 设置识别区域
            {
                cmbLookupTable.Enabled = false;
                btnAddRoi.Enabled = true;
                btnAddRect.Enabled = false;
                btnAddShape.Enabled = false;
                btnAddFromCopy.Enabled = false;
                btnFindCircle.Enabled = false;
                btnRefreshGray.Enabled = false;
                btnClearRoi.Enabled = false;

                dgvGrays.ClearRows();
                _mainImageViewer.Roi.Clear();

                for (var i = 0; i < _calibateRegions.Count; i++)
                {
                    var t = _calibateRegions[i];
                    _mainImageViewer.Roi.Add(t);
                    _mainImageViewer.Roi[i].Color = Rgb32Value.YellowColor;
                }
            }
            else if (stepBar.ItemIndex == 3) // 标定图像
            {
                cmbLookupTable.Enabled = false;
                btnAddRoi.Enabled = false;
                btnAddRect.Enabled = true;
                btnAddShape.Enabled = true;
                btnAddFromCopy.Enabled = true;
                btnFindCircle.Enabled = true;
                btnRefreshGray.Enabled = false;
                btnClearRoi.Enabled = true;

                dgvGrays.ClearRows();
                _mainImageViewer.Roi.Clear();

                for (var i = 0; i < _interestedRois.Count; i++)
                {
                    var t = _interestedRois[i];
                    _mainImageViewer.Roi.Add(t);
                }
            }
            else if (stepBar.ItemIndex == 4) // 设置灰度值范围
            {
                cmbLookupTable.Enabled = false;
                btnAddRoi.Enabled = false;
                btnAddRect.Enabled = false;
                btnAddShape.Enabled = false;
                btnAddFromCopy.Enabled = false;
                btnFindCircle.Enabled = false;
                btnRefreshGray.Enabled = true;
                btnClearRoi.Enabled = false;
            }
        }

        private void cmbLookupTable_EnabledChanged(object sender, EventArgs e)
        {
            cmbLookupTable.Style = cmbLookupTable.Enabled ? UIStyle.Green : UIStyle.Gray;
        }

        private void cmbLookupTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stepBar.ItemIndex == 1)
            {
                using (var srcImg = new VisionImage(ToCalibrateImg.Type))
                {
                    Algorithms.Copy(ToCalibrateImg, srcImg);

                    var typeToLookup =
                           EnumOperater.GetEnumByValue<ImageProcessing.LookupTableType>(
                               cmbLookupTable.Text);
                    var lookupImg = ImageProcessing.LookupTable(srcImg, typeToLookup);
                    Algorithms.Copy(lookupImg, srcImg);
                    lookupImg.Dispose();

                    Algorithms.Copy(srcImg, _mainImageViewer.Image);
                }
            }
        }

        private void InitImageViewer(ImageViewer imageViewer)
        {
            ImageShowTool(imageViewer);
            imageViewer.SizeChanged += imageViewer_SizeChanged;
            imageViewer.RoiChanged += imageViewer_RoiChanged;

            dgvGrays.Style = UIStyle.Gray;
            dgvGrays.ReadOnly = true;
            dgvGrays.RowHeadersVisible = false;
            dgvGrays.AllowUserToAddRows = false;
            dgvGrays.AllowUserToResizeRows = false;
            dgvGrays.MultiSelect = false;
            dgvGrays.RowHeadersVisible = true;
            dgvGrays.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGrays.CellClick += dgvGrays_CellClick;

            dgvGrays.AddColumn("Id", "Dd");
            dgvGrays.AddColumn("Value", "Value");
            dgvGrays.AddColumn("Min", "Min");
            dgvGrays.AddColumn("Max", "Max");
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

        private void imageViewer_RoiChanged(object sender, ContoursChangedEventArgs e)
        {
            if (stepBar.ItemIndex == 2)
            {
                _calibateRegions.Clear();
                foreach (var t in _mainImageViewer.Roi)
                {
                    t.Color = Rgb32Value.YellowColor;
                    _calibateRegions.Add(t.Shape);
                }
            }
            else if (stepBar.ItemIndex == 3)
            {
                _interestedRois.Clear();

                foreach (var t in _mainImageViewer.Roi)
                    _interestedRois.Add(t.Shape);
            }

            dgvGrays.ClearRows();
            ImageProcessing.DrawContourCountInOverlay(_mainImageViewer.Image, _mainImageViewer.Roi);
        }

        private void dgvGrays_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            for (var i = 0; i < _mainImageViewer.Roi.Count; i++)
            {
                _mainImageViewer.Roi[i].Color = Rgb32Value.GreenColor;
            }

            if (dgvGrays.RowCount > 0)
            {
                if (e.ColumnIndex == -1)
                {
                    var index = int.Parse(dgvGrays.Rows[e.RowIndex].Cells[0].Value.ToString());
                    _mainImageViewer.Roi[index - 1].Color = Rgb32Value.RedColor;
                }
            }
        }

        private void btnAddFromCopy_Click(object sender, EventArgs e)
        {
            var needRoiParas = _state.DeviceConfig.Paras.ToList().FindAll(f => f.DataType.ToLower() == "roi" && !(f.ProcessNo == _editPara.ProcessNo && f.Name.ToLower() == _editPara.Name.ToLower()));//new List<DeviceConfigPara>();

            if (!needRoiParas.Any())
            {
                UIMessageTip.Show("没有其他光型可以复制");
                return;
            }
            //var rois = new List<DeviceConfigRoi[]>();
            //foreach (var item in needRoiParas)
            //{
            //    var tpRois = _state.DeviceConfig.Rois.ToList().FindAll(f => f.Name.ToLower() == item.OkFormat.ToLower());
            //    rois.Add(tpRois.ToArray());
            //}

            var option = new UIEditOption { AutoLabelWidth = true, Text = @"光型复制", ValueWidth = 800 };
            var isCLear = new[] { "清除", "保留" };

            var showRoiName = needRoiParas.Select(f => f.Name).ToArray();
            option.AddCombobox("IsClear", "是否清除当前标定光型:", isCLear, 0, true, true);
            option.AddCombobox("Rois", "名称:", showRoiName.ToArray(), 0, true, true);

            var copyFrm = new UIEditForm(option);
            copyFrm.Render();
            copyFrm.ShowDialog();

            if (!copyFrm.IsOK)
            {
                ShowInfoTip("用户取消了操作");
            }
            else
            {
                var isClear = isCLear[(int)copyFrm["IsClear"]] == "清除";
                if (isClear)
                {
                    _interestedRois.Clear();
                    _mainImageViewer.Roi.Clear();
                    dgvGrays.ClearRows();
                }

                var selectedParaName = showRoiName[(int)copyFrm["Rois"]];
                var selectedParaIndex = (int)copyFrm["Rois"];
                var tpRois = _state.DeviceConfig.Rois.ToList().FindAll(f => f.Name.ToLower() == needRoiParas[selectedParaIndex].OkFormat.ToLower());
                if (!tpRois.Any())
                {
                    UIMessageTip.ShowError(string.Format("选择的光型：[{0}]中未标定图像，没有光型可以复制"));
                    return;
                }
                else
                {
                    foreach (var tpRoi in tpRois)
                    {
                        if (double.Parse(tpRoi.RectX) > _mainImageViewer.Image.Width || double.Parse(tpRoi.RectX) + double.Parse(tpRoi.RectWidth) > _mainImageViewer.Image.Width)
                            continue;

                        if (double.Parse(tpRoi.RectY) > _mainImageViewer.Image.Height || double.Parse(tpRoi.RectY) + double.Parse(tpRoi.RectHeight) > _mainImageViewer.Image.Height)
                            continue;

                        _interestedRois.Add(new RectangleContour() { Left = double.Parse(tpRoi.RectX), Top = double.Parse(tpRoi.RectY), Width = double.Parse(tpRoi.RectWidth), Height = double.Parse(tpRoi.RectHeight) });
                        _mainImageViewer.Roi.Add(_interestedRois[_interestedRois.Count - 1]);
                    }
                }
            }
        }

        private void btnClearRoi_Click(object sender, EventArgs e)
        {
            _interestedRois.Clear();
            _mainImageViewer.Roi.Clear();
            dgvGrays.ClearRows();
        }

        private void btnRefreshGray_Click(object sender, EventArgs e)
        {
            dgvGrays.ClearRows();

            var offset = 0.15;
            if (_mainImageViewer.Image.Width < 1000 &&
                _mainImageViewer.Image.Height < 1000)
                offset = 0.2;

            var option = new UIEditOption
            {
                AutoLabelWidth = true,
                Text = "确认"
            };

            var list = new List<string>();
            var selectIndex = 0;
            var baseIndex = 0;
            for (var i = 5; i <= 35; i = i + 5)
            {
                list.Add(string.Format("±{0}%", i));

                if (Math.Abs(offset * 100 - i) == 0)
                    selectIndex = baseIndex;
                baseIndex++;
            }

            option.AddCombobox("Percent", "±百分比", list.ToArray(), selectIndex, true, true);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                ShowInfoTip("操作取消");
                return;
            }

            offset = double.Parse(list[(int)frm["Percent"]].TrimStart('±').TrimEnd('%')) / 100;

            using (var srcBitmap = MyCamera.Base64StringToBitmap(_nowBuffBase64))
            using (var srcMat = BitmapConverter.ToMat(srcBitmap))
            {
                if (srcMat.Channels() != 1)
                    Cv2.CvtColor(srcMat, srcMat, ColorConversionCodes.BGR2GRAY);

                for (var i = 0; i < _mainImageViewer.Roi.Count; i++)
                {
                    var shape = _mainImageViewer.Roi[i].Shape as RectangleContour;
                    var rectOnImage = new Rect((int)shape.Left, (int)shape.Top, (int)shape.Width, (int)shape.Height);
                    var roiMat = new Mat(srcMat, MyCamera.GetRectInMat(srcMat.Width, srcMat.Height, rectOnImage));
                    var gray = Cv2.Mean(roiMat);
                    var grayPer = Math.Round(gray.Val0, 2, MidpointRounding.AwayFromZero);//Math.Round(ImageProcessing.GetGrayValue(_mainImageViewer.Image, shape), 2);
                    var grayMinPer = Math.Round(grayPer * (1 - offset) < 0 ? 0 : (grayPer * (1 - 0)) <= 20 ? 0 : grayPer * (1 - offset), 2);
                    var grayMaxPer = Math.Round(grayMinPer < 1 ? 20 : grayPer * (1 + offset) > 255 ? 255 : grayPer * (1 + offset), 2);

                    roiMat.Dispose();

                    dgvGrays.AddRow(i + 1, grayPer, grayMinPer, grayMaxPer);
                }

                srcBitmap.Dispose();
                srcMat.Dispose();
            }



            dgvGrays.AutoResizeColumns();
        }

        private void btnAddRect_Click(object sender, EventArgs e)
        {
            if (_mainImageViewer.Image.Width != 0 && _mainImageViewer.Image.Height != 0)
            {
                var shape = new RectangleContour(0, 0, 200, 200);
                _mainImageViewer.Roi.Add(shape);
                _interestedRois.Add(shape);
            }
        }

        private void tsBtnFindCircle_Click(object sender, EventArgs e)
        {
            if (_mainImageViewer.Image.Width != 0 && _mainImageViewer.Image.Height != 0)
            {
                _interestedRois.Clear();
                _mainImageViewer.Roi.Clear();
                dgvGrays.ClearRows();

                var circles = new List<CircleReport>();
                var rectList = new List<RectangleContour>();
                int matchRange = 20 - 15;
                int radutsExpandPixel = 20 - 0;
                if (_mainImageViewer.Image.Width <= 1000 && _mainImageViewer.Image.Height <= 1000)
                {
                    circles = ImageProcessing
                        .FindCircle(_mainImageViewer.Image, new NationalInstruments.Vision.Range(2, 100))
                        .OrderBy(f => f.Center.X).ToList();

                    matchRange = 0;
                    radutsExpandPixel = 2;
                }
                else
                {
                    circles = ImageProcessing
                        .FindCircle(_mainImageViewer.Image, new NationalInstruments.Vision.Range(5, 100))
                        .OrderBy(f => f.Center.X).ToList();
                }

                foreach (var t in from t in circles
                                  let find = rectList.Find(
                                      f =>
                                          Math.Abs(f.Left - t.Center.X) <= matchRange &&
                                          Math.Abs(f.Top - t.Center.Y) <= matchRange)
                                  where find == null
                                  select t)
                {
                    // _calibateRegions

                    var targetRect = new Rect();
                    if (_calibateRegions.Any())
                    {
                        var rect = _calibateRegions[0] as RectangleContour;
                        targetRect = new Rect((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
                    }
                    else
                    {
                        targetRect = new Rect(0, 0, _mainImageViewer.Image.Width, _mainImageViewer.Image.Height);
                    }

                    if (t.Center.X >= targetRect.X && t.Center.X <= targetRect.X + targetRect.Width && t.Center.Y >= targetRect.Top && t.Center.Y <= targetRect.Top + targetRect.Height)
                        rectList.Add(new RectangleContour(t.Center.X - (t.Radius + radutsExpandPixel),
                               t.Center.Y - (t.Radius + radutsExpandPixel), (t.Radius + radutsExpandPixel) * 2,
                               (t.Radius + radutsExpandPixel) * 2));
                }

                foreach (var t in rectList)
                {
                    _interestedRois.Add(t);
                    _mainImageViewer.Roi.Add(t);
                }
            }
        }

        private void btnAddRoi_Click(object sender, EventArgs e)
        {
            if (_mainImageViewer.Image.Width != 0 && _mainImageViewer.Image.Height != 0)
                _mainImageViewer.Roi.Add(new RectangleContour(0, 0, 200, 200));

            foreach (var t in _mainImageViewer.Roi)
                t.Color = Rgb32Value.YellowColor;
        }
    }
}
