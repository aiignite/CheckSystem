using CheckSystem.VisionDetection.Vision;
using CommonUtility;
using CommonUtility.FileOperator;
using CommonUtility.HikSdk;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using OpenCvSharp;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ImageProcessing = CheckSystem.VisionDetection.Vision.ImageProcessing;

namespace CheckSystem.VisionDetection.Calibration
{
    public sealed partial class FrmStaticVisionParaConfig : UIForm
    {
        private List<string> ListStep { get; set; }
        private VisionImage ToCalibrateImg { get; set; }

        private readonly List<Shape> _calibateRegions = new List<Shape>();
        private readonly List<Shape> _interestedRois = new List<Shape>();
        private readonly ImageViewer _mainImageViewer = new ImageViewer { Dock = DockStyle.Fill };

        private VisionConfigVisionFunc Func { get; set; }

        private bool IsLeft { get; set; }

        public FrmStaticVisionParaConfig(string funcName, bool isLeft)
        {
            InitializeComponent();
            uiSplitContainerCalibration.Panel1.Controls.Add(_mainImageViewer);
            Style = UIStyle.Gray;

            var func = VisionCommon.VisionConfig.VisionInfo.ToList().Find(f => f.VisionFuncName == funcName);
            if (func != null)
            {
                Func = func;

                if (isLeft)
                {
                    if (Func.VisionFuncDetailL == null || Func.VisionFuncDetailL.Length == 0)
                    {
                        Func.VisionFuncDetailL = new VisionConfigVisionFuncCamera[1];

                        Func.VisionFuncDetailL[0] = new VisionConfigVisionFuncCamera
                        {
                            Shutter = "10000",
                            UserId = VisionCommon.NiImaqd != null && VisionCommon.NiImaqd.CameraList.Count > 0
                                ? VisionCommon.NiImaqd.CameraList[0].GigeInfo.chSerialNumber
                                : string.Empty,
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
                    if (Func.VisionFuncDetailR == null || Func.VisionFuncDetailR.Length == 0)
                    {
                        Func.VisionFuncDetailR = new VisionConfigVisionFuncCamera[1];

                        Func.VisionFuncDetailR[0] = new VisionConfigVisionFuncCamera
                        {
                            Shutter = "10000",
                            UserId = VisionCommon.NiImaqd != null && VisionCommon.NiImaqd.CameraList.Count > 0
                             ? VisionCommon.NiImaqd.CameraList[0].GigeInfo.chUserDefinedName
                             : string.Empty,
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
                if (isLeft)
                {
                    Func.VisionFuncDetailL[0] = new VisionConfigVisionFuncCamera
                    {
                        Shutter = "10000",
                        UserId = VisionCommon.NiImaqd != null && VisionCommon.NiImaqd.CameraList.Count > 0
                            ? VisionCommon.NiImaqd.CameraList[0].GigeInfo.chSerialNumber
                            : string.Empty,
                        Analysis = new VisionConfigVisionFuncCameraAnalysis
                        {
                            CaliRegion = new string[0],
                            LookupTable = CommonUtility.ImageProcessing.LookupTableType.ImageSource.ToString(),
                            ShapesGroups = new VisionConfigVisionFuncCameraAnalysisShapesShape[1][]
                        }
                    };

                    Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0] = new VisionConfigVisionFuncCameraAnalysisShapesShape[0];
                }
                else
                {
                    Func.VisionFuncDetailR[0] = new VisionConfigVisionFuncCamera
                    {
                        Shutter = "10000",
                        UserId = VisionCommon.NiImaqd != null && VisionCommon.NiImaqd.CameraList.Count > 0
                            ? VisionCommon.NiImaqd.CameraList[0].GigeInfo.chSerialNumber
                            : string.Empty,
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

            IsLeft = isLeft;
            Text += @" 【" + Func.VisionFuncName + @")】";

            //UIStyles.InitColorful(Color.FromArgb(80, 126, 164), Color.White);
            Size =
                MinimumSize =
                    MaximumSize =
                        new System.Drawing.Size(Screen.GetWorkingArea(this).Width - 50,
                            Screen.GetWorkingArea(this).Height - 50);
            StartPosition = FormStartPosition.CenterScreen;
            Location = new System.Drawing.Point(25, 25);
            tablePanelCameraControl.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            Load += FrmCddParaConfig_Load;
            Closed += FrmStaticVisionParaConfig_Closed;
        }

        private void FrmCddParaConfig_Load(object sender, EventArgs e)
        {
            MyCamera.PushMat += MyCamera_PushMat;
            InitImageViewer(_mainImageViewer);
            InitStep();
            LoadCamera();
            InitParas();
        }

        private void MyCamera_PushMat(MvCameraSdk.MV_GIGE_DEVICE_INFO gigeInfo, Mat mat)
        {
            if (!string.IsNullOrEmpty(lblCameraName.Text) && gigeInfo.chSerialNumber == lblCameraName.Text && !btnStartGrab.Enabled)
            {
                var device = VisionCommon.NiImaqd.CameraList.Find(f => f.GigeInfo.chSerialNumber == lblCameraName.Text);
                if (device != null)
                {
                    try
                    {
                        var visionImg = MyCamera.MatToVisionImage(mat);
                        Algorithms.Copy(visionImg, _mainImageViewer.Image);
                        visionImg.Dispose();
                        mat.Dispose();
                        GC.Collect();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private static void FrmStaticVisionParaConfig_Closed(object sender, EventArgs e)
        {
            if (VisionCommon.NiImaqd == null)
                return;
            foreach (var t in VisionCommon.NiImaqd.CameraList)
                t.StopGrab();
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
            //uiBreadcrumb1.Items.Add("步骤3：设置识别区域");
            //uiBreadcrumb1.Items.Add("步骤4：标定图像");
            //uiBreadcrumb1.Items.Add("步骤5：保存参数");
            stepBar.ItemIndex = 0;

            cmbLookupTable.DropDownStyle = UIDropDownStyle.DropDownList;
            cmbLookupTable.DataSource =
                EnumOperater.GetEnumValueList<ImageProcessing.LookupTableType>();
            cmbLookupTable.SelectedIndexChanged += cmbLookupTable_SelectedIndexChanged;
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

                if (IsLeft)
                {
                    Func.VisionFuncDetailL[0].UserId = lblCameraName.Text;
                    Func.VisionFuncDetailL[0].Shutter = txtShutter.Value.ToString();
                    Func.VisionFuncDetailL[0].Analysis.LookupTable =
                        cmbLookupTable.Items[cmbLookupTable.SelectedIndex].ToString();

                    Func.VisionFuncDetailL[0].Analysis.CaliRegion = new string[_calibateRegions.Count];
                    for (var i = 0; i < _calibateRegions.Count; i++)
                    {
                        var rect = (RectangleContour)_calibateRegions[i];
                        Func.VisionFuncDetailL[0].Analysis.CaliRegion[i] = ImageProcessing.GetStringRect(rect);
                    }

                    Func.VisionFuncDetailL[0].Analysis.ShapesGroups = new VisionConfigVisionFuncCameraAnalysisShapesShape[1][];

                    var roiCount = _mainImageViewer.Roi.Count;
                    Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0] = new VisionConfigVisionFuncCameraAnalysisShapesShape[roiCount];
                    for (var i = 0; i < roiCount; i++)
                    {
                        Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0][i] = new VisionConfigVisionFuncCameraAnalysisShapesShape();
                        var shape = _mainImageViewer.Roi[i].Shape;

                        if (shape is PolygonContour) // 多边形
                        {
                            var polygonContour = (PolygonContour)shape;

                            Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0][i].Type = typeof(PolygonContour).Name;
                            Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0][i].Rect =
                                ImageProcessing.GetStringPolygonContour(polygonContour);
                        }
                        else if (shape is RectangleContour) // 矩形
                        {
                            var rectangleContour = (RectangleContour)shape;

                            Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0][i].Type = typeof(RectangleContour).Name;
                            Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0][i].Rect =
                                ImageProcessing.GetStringRect(rectangleContour);
                        }
                        else if (shape is RotatedRectangleContour) // 矩形
                        {
                            var rotatedRectangleContour = (RotatedRectangleContour)shape;

                            Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0][i].Type = typeof(RotatedRectangleContour).Name;
                            Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0][i].Rect =
                                ImageProcessing.GetStringRotatedRect(rotatedRectangleContour);
                        }

                        Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0][i].Value =
                            dgvGrays.Rows[i].Cells[1].Value.ToString();
                        Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0][i].Min = dgvGrays.Rows[i].Cells[2].Value.ToString();
                        Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0][i].Max = dgvGrays.Rows[i].Cells[3].Value.ToString();
                    }
                }
                else
                {
                    Func.VisionFuncDetailR[0].UserId = lblCameraName.Text;
                    Func.VisionFuncDetailR[0].Shutter = txtShutter.Value.ToString();
                    Func.VisionFuncDetailR[0].Analysis.LookupTable =
                         cmbLookupTable.Items[cmbLookupTable.SelectedIndex].ToString();

                    Func.VisionFuncDetailR[0].Analysis.CaliRegion = new string[_calibateRegions.Count];
                    for (var i = 0; i < _calibateRegions.Count; i++)
                    {
                        var rect = (RectangleContour)_calibateRegions[i];
                        Func.VisionFuncDetailR[0].Analysis.CaliRegion[i] = ImageProcessing.GetStringRect(rect);
                    }

                    Func.VisionFuncDetailR[0].Analysis.ShapesGroups = new VisionConfigVisionFuncCameraAnalysisShapesShape[1][];

                    var roiCount = _mainImageViewer.Roi.Count;
                    Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0] = new VisionConfigVisionFuncCameraAnalysisShapesShape[roiCount];
                    for (var i = 0; i < roiCount; i++)
                    {
                        Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0][i] = new VisionConfigVisionFuncCameraAnalysisShapesShape();
                        var shape = _mainImageViewer.Roi[i].Shape;

                        if (shape is PolygonContour) // 多边形
                        {
                            var polygonContour = (PolygonContour)shape;

                            Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0][i].Type = typeof(PolygonContour).Name;
                            Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0][i].Rect =
                                 ImageProcessing.GetStringPolygonContour(polygonContour);
                        }
                        else if (shape is RectangleContour) // 矩形
                        {
                            var rectangleContour = (RectangleContour)shape;

                            Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0][i].Type = typeof(RectangleContour).Name;
                            Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0][i].Rect =
                                 ImageProcessing.GetStringRect(rectangleContour);
                        }
                        else if (shape is RotatedRectangleContour) // 矩形
                        {
                            var rotatedRectangleContour = (RotatedRectangleContour)shape;

                            Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0][i].Type = typeof(RotatedRectangleContour).Name;
                            Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0][i].Rect =
                                 ImageProcessing.GetStringRotatedRect(rotatedRectangleContour);
                        }

                        Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0][i].Value =
                             dgvGrays.Rows[i].Cells[1].Value.ToString();
                        Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0][i].Min = dgvGrays.Rows[i].Cells[2].Value.ToString();
                        Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0][i].Max = dgvGrays.Rows[i].Cells[3].Value.ToString();
                    }
                }

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
                ShowSuccessTip("保存成功");
            }

            if (stepBar.ItemIndex == 0) // 选择相机并获取图片
            {
                var img = new VisionImage(ImageType.U8);
                Algorithms.Copy(img, _mainImageViewer.Image);
                img.Dispose();

                treeViewCamerasList.Enabled = true;
                cameraGroupBox.Enabled = true;
                visionGroupBox.Enabled = false;

                dgvGrays.ClearRows();
                _mainImageViewer.Roi.Clear();
            }
            else if (stepBar.ItemIndex == 1) // 增强对比度
            {
                if (_mainImageViewer.Image.Width != 0 && _mainImageViewer.Image.Height != 0)
                {
                    if (!string.IsNullOrEmpty(lblCameraName.Text))
                    {
                        treeViewCamerasList.Enabled = false;
                        cameraGroupBox.Enabled = false;
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

        private void stepBar_EnabledChanged(object sender, EventArgs e)
        {
            stepBar.Style = stepBar.Enabled ? UIStyle.LayuiGreen : UIStyle.Gray;
        }

        private void cmbLookupTable_EnabledChanged(object sender, EventArgs e)
        {
            cmbLookupTable.Style = cmbLookupTable.Enabled ? UIStyle.Green : UIStyle.Gray;
        }

        private void LoadCamera()
        {
            var cameraTreeNode = new TreeNode { Text = @"相机列表" };

            if (VisionCommon.NiImaqd != null)
            {
                for (var i = 0; i < VisionCommon.NiImaqd.CameraList.Count; i++)
                    cameraTreeNode.Nodes.Add(new TreeNode { Text = string.Format("相机{0}：{1}", i + 1, VisionCommon.NiImaqd.CameraList[i].GigeInfo.chSerialNumber) });

                treeViewCamerasList.Nodes.Add(cameraTreeNode);
                treeViewCamerasList.ExpandAll();
                treeViewCamerasList.BeforeSelect += treeViewCamerasList_BeforeSelect;
                txtShutter.ValueChanged += txtShutter_ValueChanged;

                if (VisionCommon.NiImaqd.CameraList.Any())
                    treeViewCamerasList.SelectedNode = cameraTreeNode.Nodes[0];
            }
        }

        private void InitParas()
        {
            if (VisionCommon.NiImaqd != null)
            {
                if (IsLeft)
                {
                    for (var i = 0; i < treeViewCamerasList.Nodes[0].Nodes.Count; i++)
                    {
                        if (Func.VisionFuncDetailL[0].UserId == null ||
                            !treeViewCamerasList.Nodes[0].Nodes[i].Text.EndsWith(
                            Func.VisionFuncDetailL[0].UserId))
                            continue;
                        treeViewCamerasList.SelectedNode = treeViewCamerasList.Nodes[0].Nodes[i];
                        txtShutter.Value = int.Parse(Func.VisionFuncDetailL[0].Shutter);
                        break;
                    }

                    for (var i = 0; i < cmbLookupTable.Items.Count; i++)
                    {
                        if (cmbLookupTable.Items[i].ToString() == Func.VisionFuncDetailL[0].Analysis.LookupTable)
                        {
                            cmbLookupTable.SelectedIndex = i;
                            break;
                        }
                    }

                    if (Func.VisionFuncDetailL[0].Analysis.CaliRegion != null)
                    {
                        foreach (var sp in from t in Func.VisionFuncDetailL[0].Analysis.CaliRegion where !string.IsNullOrEmpty(t) select t.Split(','))
                            _calibateRegions.Add(new RectangleContour(double.Parse(sp[0]), double.Parse(sp[1]), double.Parse(sp[2]), double.Parse(sp[3])));
                    }

                    foreach (var t in Func.VisionFuncDetailL[0].Analysis.ShapesGroups[0])
                    {
                        if (t.Type == "PolygonContour")
                            _interestedRois.Add(ImageProcessing.GetPolygonContourByString(t.Rect));
                        else if (t.Type == "RectangleContour")
                            _interestedRois.Add(ImageProcessing.GetRectByString(t.Rect));
                        else if (t.Type == "RotatedRectangleContour")
                            _interestedRois.Add(ImageProcessing.GetRotatedRecvByString(t.Rect));
                    }
                }
                else
                {
                    for (var i = 0; i < treeViewCamerasList.Nodes[0].Nodes.Count; i++)
                    {
                        if (Func.VisionFuncDetailR[0].UserId == null ||
                            !treeViewCamerasList.Nodes[0].Nodes[i].Text.EndsWith(
                            Func.VisionFuncDetailR[0].UserId))
                            continue;
                        treeViewCamerasList.SelectedNode = treeViewCamerasList.Nodes[0].Nodes[i];
                        txtShutter.Value = int.Parse(Func.VisionFuncDetailR[0].Shutter);
                        break;
                    }

                    for (var i = 0; i < cmbLookupTable.Items.Count; i++)
                    {
                        if (cmbLookupTable.Items[i].ToString() == Func.VisionFuncDetailR[0].Analysis.LookupTable)
                        {
                            cmbLookupTable.SelectedIndex = i;
                            break;
                        }
                    }

                    if (Func.VisionFuncDetailR[0].Analysis.CaliRegion != null)
                    {
                        foreach (var sp in from t in Func.VisionFuncDetailR[0].Analysis.CaliRegion where !string.IsNullOrEmpty(t) select t.Split(','))
                            _calibateRegions.Add(new RectangleContour(double.Parse(sp[0]), double.Parse(sp[1]), double.Parse(sp[2]), double.Parse(sp[3])));
                    }

                    foreach (var t in Func.VisionFuncDetailR[0].Analysis.ShapesGroups[0])
                    {
                        if (t.Type == "PolygonContour")
                            _interestedRois.Add(ImageProcessing.GetPolygonContourByString(t.Rect));
                        else if (t.Type == "RectangleContour")
                            _interestedRois.Add(ImageProcessing.GetRectByString(t.Rect));
                        else if (t.Type == "RotatedRectangleContour")
                            _interestedRois.Add(ImageProcessing.GetRotatedRecvByString(t.Rect));
                    }
                }
            }
        }

        private void treeViewCamerasList_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (!e.Node.Text.Contains("："))
                return;

            if (lblCameraName.Text != e.Node.Text.Split("：")[1])
            {
                if (btnStartGrab.Enabled == false)
                    btnStopGrab.PerformClick();

                var img = new VisionImage(ImageType.U8);
                Algorithms.Copy(img, _mainImageViewer.Image);
                img.Dispose();

                lblCameraName.Text = e.Node.Text.Split("：")[1];
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
            //ViewerTools.Rectangle;
            //ViewerTools.All;
            imageViewer.ActiveTool = ViewerTools.Selection;
            imageViewer.ZoomToFit = true;
            imageViewer.ShowToolbar = true;
            imageViewer.ShowScrollbars = true;
            imageViewer.ShowImageInfo = true;

            //imageViewer.AutoDelete = false;
        }

        private static void imageViewer_SizeChanged(object sender, EventArgs e)
        {
            var imageViewer = sender as ImageViewer;
            if (imageViewer != null)
                ImageShowTool(imageViewer);
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

        private void btnStartGrab_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblCameraName.Text))
            {
                var device = VisionCommon.NiImaqd.CameraList.Find(f => f.GigeInfo.chSerialNumber == lblCameraName.Text);
                if (device != null)
                {
                    device.StartGrab();

                    {
                        var t =
                            VisionCommon.VisionConfig.ParaInfo.ToList()
                                .Find(f => f.ParaName == Func.VisionFuncName && f.ParaType.EndsWith("静态图像"));
                        if (t != null)
                        {
                            if (VisionCommon.VisionConfig.TestFlowInfo != null && VisionCommon.VisionConfig.TestFlowInfo.Any())
                            {
                                var lvl1Index = -1;

                                for (var i = 0; i < VisionCommon.VisionConfig.TestFlowInfo.Length; i++)
                                {
                                    if (VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow != null && VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow.Any())
                                    {
                                        var index = VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow.ToList()
                                             .FindIndex(
                                                 f =>
                                                     f.TestFlowValue.StartsWith("检测：") &&
                                                     f.TestFlowValue.Contains(t.ParaName));

                                        if (index != -1)
                                        {
                                            lvl1Index = i;
                                            break;
                                        }
                                    }
                                }

                                if (lvl1Index != -1)
                                {
                                    var level1 = VisionCommon.VisionConfig.TestFlowInfo[lvl1Index];
                                    VisionCommon.PowerSet(level1.TestFlowValue.Split('：')[1], true);
                                    var isBreak = false;

                                    foreach (var level2 in level1.TestFlow)
                                    {
                                        if (string.IsNullOrEmpty(level2.TestFlowValue))
                                            continue;
                                        if (isBreak)
                                            break;

                                        if (level2.TestFlowValue.StartsWith("检测："))
                                        {
                                            var checkList = level2.TestFlowValue.Split("：")[1].Split("，");

                                            if (checkList.Any())
                                            {
                                                foreach (var ct in checkList)
                                                {
                                                    if (ct == t.ParaName)
                                                    {
                                                        VisionCommon.InvokeRelays(t.ParaReleysList);
                                                        if (t.ParaMethods != null)
                                                            VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsBefore);

                                                        isBreak = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (level2.TestFlowValue.StartsWith("继电器"))
                                            {
                                                var sp = level2.TestFlowValue.Split("：");
                                                var key = sp[0];
                                                var value = string.Equals(sp[1], "ON", StringComparison.CurrentCultureIgnoreCase);
                                                if (!VisionCommon.Relays.ContainsKey(key))
                                                    continue;
                                                VisionCommon.Relays[key].Invoke(value);

                                                //foreach (var ioT in VisionCommon.IoControllers)
                                                //    ioT.Value.SetOutputs();
                                            }
                                            else if (level2.TestFlowValue.StartsWith("延时："))
                                            {
                                                var sp = level2.TestFlowValue.Split("：");
                                                Thread.Sleep(int.Parse(sp[1]));
                                            }
                                            else if (level2.TestFlowValue.StartsWith("函数："))
                                            {
                                                var sp = level2.TestFlowValue.Split("：");
                                                VisionCommon.InvokeCustomCmd(sp[1]);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var thisStepPowerPara = string.IsNullOrEmpty(t.PowerPara)
                                  ? string.Format("电压1={0}V，电流1={1}A，电压2={2}V，电流2={3}A，电压3={4}V，电流3={5}A，串并联模式={6}", 13.5,
                                      10, 0, 0, 5, 1, "无")
                                  : t.PowerPara;
                                VisionCommon.PowerSet(thisStepPowerPara, true);

                                VisionCommon.InvokeRelays(t.ParaReleysList);
                                if (t.ParaMethods != null)
                                    VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsBefore);
                            }
                        }
                    }

                    stepBar.Enabled = false;
                    btnStartGrab.Enabled = false;
                    btnSnap.Enabled = false;
                    btnLoadImg.Enabled = false;
                    btnStopGrab.Enabled = true;
                }
            }
            else
            {
                ShowErrorDialog("请先选择相机");
            }
        }

        private void btnStopGrab_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblCameraName.Text))
            {
                var device = VisionCommon.NiImaqd.CameraList.Find(f => f.GigeInfo.chSerialNumber == lblCameraName.Text);
                if (device != null)
                {
                    device.StopGrab();

                    var t =
                        VisionCommon.VisionConfig.ParaInfo.ToList().Find(f => f.ParaName == Func.VisionFuncName && f.ParaType.EndsWith("静态图像"));
                    if (t != null)
                    {
                        if (VisionCommon.VisionConfig.TestFlowInfo != null && VisionCommon.VisionConfig.TestFlowInfo.Any())
                        {
                            var lvl1Index = -1;

                            for (var i = 0; i < VisionCommon.VisionConfig.TestFlowInfo.Length; i++)
                            {
                                if (VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow != null && VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow.Any())
                                {
                                    var index = VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow.ToList()
                                         .FindIndex(
                                             f =>
                                                 f.TestFlowValue.StartsWith("检测：") &&
                                                 f.TestFlowValue.Contains(t.ParaName));

                                    if (index != -1)
                                    {
                                        lvl1Index = i;
                                        break;
                                    }
                                }
                            }

                            if (lvl1Index != -1)
                            {
                                var level1 = VisionCommon.VisionConfig.TestFlowInfo[lvl1Index];
                                var isBreak = false;

                                foreach (var level2 in level1.TestFlow)
                                {
                                    if (string.IsNullOrEmpty(level2.TestFlowValue))
                                        continue;

                                    if (level2.TestFlowValue.StartsWith("检测："))
                                    {
                                        var checkList = level2.TestFlowValue.Split("：")[1].Split("，");

                                        if (checkList.Any())
                                        {
                                            foreach (var ct in checkList)
                                            {
                                                if (ct == t.ParaName)
                                                {
                                                    VisionCommon.InvokeRelays(t.ParaReleysList, true);

                                                    if (t.ParaMethods != null)
                                                        VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsAfter);

                                                    isBreak = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (level2.TestFlowValue.StartsWith("继电器"))
                                        {
                                            if (isBreak)
                                            {
                                                var sp = level2.TestFlowValue.Split("：");
                                                var key = sp[0];
                                                var value = string.Equals(sp[1], "ON", StringComparison.CurrentCultureIgnoreCase);
                                                if (!VisionCommon.Relays.ContainsKey(key))
                                                    continue;
                                                VisionCommon.Relays[key].Invoke(value);

                                                //foreach (var ioT in VisionCommon.IoControllers)
                                                //    ioT.Value.SetOutputs();
                                            }
                                        }
                                        else if (level2.TestFlowValue.StartsWith("延时："))
                                        {
                                            if (isBreak)
                                            {
                                                var sp = level2.TestFlowValue.Split("：");
                                                Thread.Sleep(int.Parse(sp[1]));
                                            }

                                        }
                                        else if (level2.TestFlowValue.StartsWith("函数："))
                                        {
                                            if (isBreak)
                                            {
                                                var sp = level2.TestFlowValue.Split("：");
                                                VisionCommon.InvokeCustomCmd(sp[1]);
                                            }
                                        }
                                    }
                                }

                                foreach (var power in VisionCommon.ListCcdPowers)
                                {
                                    VisionCommon.IsPowerOn = false;
                                    power.Value.PowerOff();
                                }
                            }
                        }
                        else
                        {
                            VisionCommon.InvokeRelays(t.ParaReleysList, true);

                            if (t.ParaMethods != null)
                                VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsAfter);

                            foreach (var pp in VisionCommon.ListCcdPowers)
                                pp.Value.PowerOff();
                            VisionCommon.IsPowerOn = false;
                        }
                    }

                    stepBar.Enabled = true;
                    btnStartGrab.Enabled = true;
                    btnSnap.Enabled = true;
                    btnLoadImg.Enabled = true;
                    btnStopGrab.Enabled = false;
                }
            }
        }

        private void txtShutter_ValueChanged(object sender, int value)
        {
            if (!string.IsNullOrEmpty(lblCameraName.Text))
            {
                var device = VisionCommon.NiImaqd.CameraList.Find(f => f.GigeInfo.chSerialNumber == lblCameraName.Text);
                if (device != null)
                    device.SetExposureTime(value);
            }
        }

        private void btnSnap_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblCameraName.Text))
            {
                var device = VisionCommon.NiImaqd.CameraList.Find(f => f.GigeInfo.chSerialNumber == lblCameraName.Text);
                if (device != null)
                {
                    device.SetExposureTime(txtShutter.Value);
                    Thread.Sleep(200);

                    var t =
                        VisionCommon.VisionConfig.ParaInfo.ToList().Find(f =>
                            f.ParaName == Func.VisionFuncName && f.ParaType.EndsWith("静态图像"));
                    if (t != null)
                    {
                        if (VisionCommon.VisionConfig.TestFlowInfo != null && VisionCommon.VisionConfig.TestFlowInfo.Any())
                        {
                            var lvl1Index = -1;

                            for (var i = 0; i < VisionCommon.VisionConfig.TestFlowInfo.Length; i++)
                            {
                                if (VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow != null && VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow.Any())
                                {
                                    var index = VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow.ToList()
                                         .FindIndex(
                                             f =>
                                                 f.TestFlowValue.StartsWith("检测：") &&
                                                 f.TestFlowValue.Contains(t.ParaName));

                                    if (index != -1)
                                    {
                                        lvl1Index = i;
                                        break;
                                    }
                                }
                            }

                            if (lvl1Index != -1)
                            {
                                var level1 = VisionCommon.VisionConfig.TestFlowInfo[lvl1Index];
                                VisionCommon.PowerSet(level1.TestFlowValue.Split('：')[1], true);

                                foreach (var level2 in level1.TestFlow)
                                {
                                    if (string.IsNullOrEmpty(level2.TestFlowValue))
                                        continue;

                                    if (level2.TestFlowValue.StartsWith("检测："))
                                    {
                                        var checkList = level2.TestFlowValue.Split("：")[1].Split("，");

                                        if (checkList.Any())
                                        {
                                            foreach (var ct in checkList)
                                            {
                                                if (ct == t.ParaName)
                                                {
                                                    VisionCommon.InvokeRelays(t.ParaReleysList);
                                                    if (t.ParaMethods != null)
                                                        VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsBefore);

                                                    if (!string.IsNullOrEmpty(t.ParaDelayMs) || t.ParaDelayMs != "/" || int.Parse(t.ParaDelayMs) > 0)
                                                        Thread.Sleep(int.Parse(t.ParaDelayMs));

                                                    device.Capture();
                                                    var visionImg = MyCamera.MatToVisionImage(MyCamera.Base64StringToMat(device.MatBuffer[0, 0]));
                                                    Algorithms.Copy(visionImg, _mainImageViewer.Image);
                                                    visionImg.Dispose();

                                                    VisionCommon.InvokeRelays(t.ParaReleysList, true);

                                                    if (t.ParaMethods != null)
                                                        VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsAfter);

                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (level2.TestFlowValue.StartsWith("继电器"))
                                        {
                                            var sp = level2.TestFlowValue.Split("：");
                                            var key = sp[0];
                                            var value = string.Equals(sp[1], "ON", StringComparison.CurrentCultureIgnoreCase);
                                            if (!VisionCommon.Relays.ContainsKey(key))
                                                continue;
                                            VisionCommon.Relays[key].Invoke(value);

                                            //foreach (var ioT in VisionCommon.IoControllers)
                                            //    ioT.Value.SetOutputs();
                                        }
                                        else if (level2.TestFlowValue.StartsWith("延时："))
                                        {
                                            var sp = level2.TestFlowValue.Split("：");
                                            Thread.Sleep(int.Parse(sp[1]));
                                        }
                                        else if (level2.TestFlowValue.StartsWith("函数："))
                                        {
                                            var sp = level2.TestFlowValue.Split("：");
                                            VisionCommon.InvokeCustomCmd(sp[1]);
                                        }
                                    }
                                }

                                foreach (var power in VisionCommon.ListCcdPowers)
                                {
                                    VisionCommon.IsPowerOn = false;
                                    power.Value.PowerOff();
                                }
                            }
                        }
                        else
                        {
                            var thisStepPowerPara = string.IsNullOrEmpty(t.PowerPara)
                                 ? string.Format("电压1={0}V，电流1={1}A，电压2={2}V，电流2={3}A，电压3={4}V，电流3={5}A，串并联模式={6}",
                                     13.5, 10, 0, 0, 5, 1, "无")
                                 : t.PowerPara;

                            VisionCommon.PowerSet(thisStepPowerPara, true);
                            VisionCommon.InvokeRelays(t.ParaReleysList);
                            if (t.ParaMethods != null)
                                VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsBefore);

                            if (!string.IsNullOrEmpty(t.ParaDelayMs) || t.ParaDelayMs != "/" || int.Parse(t.ParaDelayMs) > 0)
                                Thread.Sleep(int.Parse(t.ParaDelayMs));

                            //var st = new Stopwatch();
                            //st.Start();

                            device.Capture();
                            var visionImg = MyCamera.MatToVisionImage(MyCamera.Base64StringToMat(device.MatBuffer[0, 0]));
                            Algorithms.Copy(visionImg, _mainImageViewer.Image);
                            visionImg.Dispose();

                            VisionCommon.InvokeRelays(t.ParaReleysList, true);

                            if (t.ParaMethods != null)
                                VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsAfter);

                            foreach (var pp in VisionCommon.ListCcdPowers)
                                pp.Value.PowerOff();
                            VisionCommon.IsPowerOn = false;

                            //st.Stop();
                            //ShowSuccessTip(string.Format(@"抓拍结束，耗时：{0}ms", st.ElapsedMilliseconds));
                        }
                    }
                }
            }
            else
            {
                ShowErrorDialog("请先选择相机");
            }
        }

        private void btnSaveImg_Click(object sender, EventArgs e)
        {
        }

        private void tsBtnAddRoi_Click(object sender, EventArgs e)
        {
            if (_mainImageViewer.Image.Width != 0 && _mainImageViewer.Image.Height != 0)
                _mainImageViewer.Roi.Add(new RectangleContour(0, 0, 200, 200));

            foreach (var t in _mainImageViewer.Roi)
                t.Color = Rgb32Value.YellowColor;
        }

        private void btnLoadImg_Click(object sender, EventArgs e)
        {
            var imageDialog = new ImagePreviewFileDialog();

            if (imageDialog.ShowDialog() == DialogResult.OK)
            {
                //var fileinfo = Algorithms.GetFileInformation(imageDialog.FileName);
                _mainImageViewer.Image.ReadFile(imageDialog.FileName);
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
                int radutsExpandPixel = 10 - 0;
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
                    rectList.Add(new RectangleContour(t.Center.X - (t.Radius + radutsExpandPixel),
                        t.Center.Y - (t.Radius + radutsExpandPixel), (t.Radius + radutsExpandPixel) * 2,
                        (t.Radius + radutsExpandPixel) * 2));

                foreach (var t in rectList)
                {
                    _interestedRois.Add(t);
                    _mainImageViewer.Roi.Add(t);
                }

                //foreach (var t in circles.OrderBy(f => f.Center.X))
                //{
                //    var roi = new RotatedRectangleContour(
                //        new PointContour { X = t.Center.X, Y = t.Center.Y }, t.Radius * 2 + 20, t.Radius * 2 + 20, 0);

                //    MainImageViewer.Roi.Add(roi);
                //}

                //foreach (var t in MainImageViewer.Roi)
                //{
                //    t.Color = Rgb32Value.YellowColor;
                //}
            }
        }

        private void btnClearRoi_Click(object sender, EventArgs e)
        {
            _interestedRois.Clear();
            _mainImageViewer.Roi.Clear();
            dgvGrays.ClearRows();
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

        private void btnAddShape_Click(object sender, EventArgs e)
        {
            return;

            if (_mainImageViewer.Image.Width != 0 && _mainImageViewer.Image.Height != 0)
            {
                var newPolygonContour = ImageProcessing.GetPolygonContour();
                _mainImageViewer.Roi.Add(newPolygonContour);
                _interestedRois.Add(newPolygonContour);
            }
        }

        private void imageViewer_RoiChanged(object sender, ContoursChangedEventArgs e)
        {
            //dgvGrays.ClearRows();

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

                //var temp = new List<Shape>();

                foreach (var t in _mainImageViewer.Roi)
                    _interestedRois.Add(t.Shape);
                //temp=temp.OrderBy(f=>f.ConvertToRoi())
            }

            dgvGrays.ClearRows();
            ImageProcessing.DrawContourCountInOverlay(_mainImageViewer.Image, _mainImageViewer.Roi);
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

            //var mat = VisionImageToMat(MainImageViewer.Image);
            ////Cv2.ImShow("ImgOrigin", mat);

            //for (var i = 0; i < MainImageViewer.Roi.Count; i++)
            //{
            //    var shape = MainImageViewer.Roi[i].Shape as RectangleContour;

            //    var gray = 0d;

            //    if (shape != null)
            //    {
            //        var rect = new Rect((int)shape.Left, (int)shape.Top, (int)shape.Width, (int)shape.Height);

            //        if (mat.Channels() == 1)
            //        {
            //            // 在灰度图像上裁剪出矩形区域
            //            var roi = new Mat(mat, rect);

            //            // 计算ROI的平均灰度值
            //            var avgGray = Cv2.Mean(roi);
            //            gray = Math.Round(avgGray[0], 0, MidpointRounding.AwayFromZero);

            //            //var grayMinPer =
            //            //    avgY * 0.85 < 0 ? 0 : Math.Round(avgY * 0.85, 0, MidpointRounding.AwayFromZero);
            //            //var grayMaxPer = avgY * 1.15 > 255
            //            //    ? 255
            //            //    : Math.Round(avgY * 1.15, 0, MidpointRounding.AwayFromZero);

            //            //dgvGrays.AddRow(i + 1, avgY, grayMinPer, grayMaxPer);

            //            roi.Dispose();
            //            GC.Collect();
            //        }
            //        else
            //        {
            //            // 创建一个Mat对象以储存转换到XYZ颜色空间后的图像
            //            var imgXyz = new Mat();

            //            // 将图像从BGR转换到CIE XYZ颜色空间
            //            Cv2.CvtColor(mat, imgXyz, ColorConversionCodes.BGR2XYZ);

            //            // 获取该矩形区域内的子图像
            //            var regionXyz = imgXyz.SubMat(rect);

            //            // 计算区域内的平均XYZ值
            //            var avgXyz = Cv2.Mean(regionXyz);
            //            gray = Math.Round(avgXyz[1], 0, MidpointRounding.AwayFromZero); //Y值，同时也代表了亮度信息

            //            //var grayMinPer =
            //            //    avgY * 0.85 < 0 ? 0 : Math.Round(avgY * 0.85, 0, MidpointRounding.AwayFromZero);
            //            //var grayMaxPer = avgY * 1.15 > 255
            //            //    ? 255
            //            //    : Math.Round(avgY * 1.15, 0, MidpointRounding.AwayFromZero);

            //            //dgvGrays.AddRow(i + 1, avgY, grayMinPer, grayMaxPer);

            //            imgXyz.Dispose();
            //            GC.Collect();
            //        }

            //        var grayPer = Math.Round(gray / 255, 2);
            //        var grayMinPer = Math.Round(grayPer - offset < 0 ? 0 : grayPer - offset, 2);
            //        var grayMaxPer = Math.Round(grayPer + offset > 1 ? 1 : grayPer + offset, 2);

            //        dgvGrays.AddRow(i + 1, grayPer * 100 + "%", grayMinPer * 100 + "%", grayMaxPer * 100 + "%");
            //    }
            //}

            //mat.Dispose();
            //GC.Collect();

            for (var i = 0; i < _mainImageViewer.Roi.Count; i++)
            {
                var shape = _mainImageViewer.Roi[i].Shape;
                var grayPer = Math.Round(ImageProcessing.GetGrayValue(_mainImageViewer.Image, shape) / 255, 2);
                var grayMinPer = Math.Round(grayPer - offset < 0 ? 0 : grayPer - offset, 2);
                var grayMaxPer = Math.Round(grayPer + offset > 1 ? 1 : grayPer + offset, 2);

                //if (MainImageViewer.Roi[i].Shape is RectangleContour)
                //{
                //    var roi = (RectangleContour)MainImageViewer.Roi[i].Shape;

                //    var mat = MyCamera.VisionImageToMat(MainImageViewer.Image);

                //    var rect = new Rect((int)roi.Left, (int)roi.Top, (int)roi.Width, (int)roi.Height);

                //    // 使用SubMat方法获取这个矩形区域的子图像
                //    Mat subImage = mat.SubMat(rect);

                //    // 计算这个区域的均值
                //    Scalar meanValue = Cv2.Mean(subImage);

                //    mat.Dispose();
                //    subImage.Dispose();
                //    GC.Collect();
                //}

                dgvGrays.AddRow(i + 1, grayPer * 100 + "%", grayMinPer * 100 + "%", grayMaxPer * 100 + "%");
            }

            dgvGrays.AutoResizeColumns();
        }

        private void btnAddFromCopy_Click(object sender, EventArgs e)
        {
            var option = new UIEditOption { AutoLabelWidth = true, Text = @"光型复制" };
            var rois = VisionCommon.VisionConfig.ParaInfo.ToList().FindAll(f => f.ParaType.Contains("静态图像"))
                .Select(t => t.ParaName).ToList();
            var isCLear = new[] { "清除", "保留" };

            option.AddCombobox("IsClear", "是否清除当前标定光型:", isCLear, 0, true, true);
            option.AddCombobox("Rois", "名称:", rois.ToArray(), 0, true, true);

            var copyFrm = new UIEditForm(option);
            copyFrm.Render();
            copyFrm.ShowDialog();

            if (!copyFrm.IsOK)
            {
                ShowInfoTip("用户取消了操作");
            }
            else
            {
                var funcName = rois[(int)copyFrm["Rois"]];
                var isClear = isCLear[(int)copyFrm["IsClear"]] == "清除";
                if (isClear)
                {
                    _interestedRois.Clear();
                    _mainImageViewer.Roi.Clear();
                    dgvGrays.ClearRows();
                }

                var func = VisionCommon.VisionConfig.VisionInfo.ToList().Find(f => f.VisionFuncName == funcName);
                if (func != null)
                {
                    if (IsLeft)
                    {
                        foreach (var t in func.VisionFuncDetailL[0].Analysis.ShapesGroups[0])
                        {

                            if (t.Type == "PolygonContour")
                                _interestedRois.Add(ImageProcessing.GetPolygonContourByString(t.Rect));
                            else if (t.Type == "RectangleContour")
                                _interestedRois.Add(ImageProcessing.GetRectByString(t.Rect));
                            else if (t.Type == "RotatedRectangleContour")
                                _interestedRois.Add(ImageProcessing.GetRotatedRecvByString(t.Rect));

                            _mainImageViewer.Roi.Add(_interestedRois[_interestedRois.Count - 1]);
                        }
                    }
                    else
                    {
                        foreach (var t in func.VisionFuncDetailR[0].Analysis.ShapesGroups[0])
                        {

                            if (t.Type == "PolygonContour")
                                _interestedRois.Add(ImageProcessing.GetPolygonContourByString(t.Rect));
                            else if (t.Type == "RectangleContour")
                                _interestedRois.Add(ImageProcessing.GetRectByString(t.Rect));
                            else if (t.Type == "RotatedRectangleContour")
                                _interestedRois.Add(ImageProcessing.GetRotatedRecvByString(t.Rect));

                            _mainImageViewer.Roi.Add(_interestedRois[_interestedRois.Count - 1]);
                        }
                    }
                }
            }
        }
    }
}
