using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility;
using Go;
using HZH_Controls.IconFont;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using Newtonsoft.Json;
using OpenCvSharp;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.YfasLogo
{
    public partial class FrmYfasLogoContourConfig : UIForm
    {
        private generator _timeAction;

        /// <summary>
        /// 0=外圈，1=V，2=W
        /// </summary>
        private int _thisType;

        private Mat _srcMat;

        private readonly string _productName;

        private YfasLogoSqlHelper.ProductType _product;

        private readonly List<YfasLogoSqlHelper.LogoConfigModel> _config = YfasLogoSqlHelper.GetConfigModels();

        public FrmYfasLogoContourConfig(YfasLogoSqlHelper.ProductType product)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Icon = FontImages.GetIcon(
                FontIcons.A_fa_file_code_o, 32, Color.DodgerBlue);

            _product = product;
            _productName = _product.GetCustomAttribute<DescriptionAttribute>().Description;
            InitConfigs();

            Load += FrmYfasLogoConfig_Load;
            Closed += FrmYfasLogoConfig_Closed;
            tscmbRoiType.SelectedIndexChanged += tscmbRoiType_SelectedIndexChanged;
        }

        private void InitConfigs()
        {
            var findOval = _config.Find(f => f.Position == "外圈" && f.Type == "Oval" && f.ProductionName == _productName);
            if (findOval == null)
            {
                findOval = new YfasLogoSqlHelper.LogoConfigModel
                {
                    MaxGray = 255,
                    MinGray = 0,
                    MaxHomo = 100,
                    MinHomo = 0,
                    Object = JsonConvert.SerializeObject(new OvalContour(0, 0, 350, 350)),
                    Position = "外圈",
                    ProductionName = _productName,
                    Type = "Oval",
                    MinAverage = 0,
                    MaxAverage = 255
                };
                YfasLogoSqlHelper.AddConfig(findOval);
            }

            var findVs = _config.FindAll(f => f.Position.StartsWith("V") && f.Type == "Line" && f.ProductionName == _productName);
            if (findVs.Count != 3)
            {
                for (var i = 0; i < 3; i++)
                {
                    var position = "V-" + (i + 1);
                    YfasLogoSqlHelper.ClearAllConfigData(string.Format("Position = '{0}' and ProductName = '{1}' and Type = '{2}'", position, _productName, "Line"));

                    var newV = new YfasLogoSqlHelper.LogoConfigModel
                    {
                        MaxGray = 255,
                        MinGray = 0,
                        MaxHomo = 100,
                        MinHomo = 0,
                        Object = JsonConvert.SerializeObject(new LineContour(new PointContour(0, 0), new PointContour(100, 100))),
                        Position = position,
                        ProductionName = _productName,
                        Type = "Line",
                        MinAverage = 0,
                        MaxAverage = 255
                    };
                    YfasLogoSqlHelper.AddConfig(newV);
                }
            }

            var findWs = _config.FindAll(f => f.Position.StartsWith("W") && f.Type == "Line" && f.ProductionName == _productName);
            if (findWs.Count != 5)
            {
                for (var i = 0; i < 5; i++)
                {
                    var position = "W-" + (i + 1);
                    YfasLogoSqlHelper.ClearAllConfigData(string.Format("Position = '{0}' and ProductName = '{1}' and Type = '{2}'", position, _productName, "Line"));

                    var newW = new YfasLogoSqlHelper.LogoConfigModel
                    {
                        MaxGray = 255,
                        MinGray = 0,
                        MaxHomo = 100,
                        MinHomo = 0,
                        Object = JsonConvert.SerializeObject(new LineContour(new PointContour(0, 0), new PointContour(100, 100))),
                        Position = position,
                        ProductionName = _productName,
                        Type = "Line",
                        MinAverage = 0,
                        MaxAverage = 255
                    };
                    YfasLogoSqlHelper.AddConfig(newW);
                }
            }

            var find16 =
                _config.FindAll(f => f.Position.StartsWith("圈-") && f.Type == "Oval" && f.ProductionName == _productName);
            if (find16.Count != 16)
            {
                for (var i = 0; i < 16; i++)
                {
                    var position = "圈-" + (i + 1);
                    YfasLogoSqlHelper.ClearAllConfigData(string.Format("Position = '{0}' and ProductName = '{1}' and Type = '{2}'", position, _productName, "Line"));

                    var newW = new YfasLogoSqlHelper.LogoConfigModel
                    {
                        MaxGray = 255,
                        MinGray = 0,
                        MaxHomo = 100,
                        MinHomo = 0,
                        Object = JsonConvert.SerializeObject(new OvalContour(0, 0, 350, 350)),
                        Position = position,
                        ProductionName = _productName,
                        Type = "Oval",
                        MinAverage = 0,
                        MaxAverage = 255
                    };
                    YfasLogoSqlHelper.AddConfig(newW);
                }
            }

            _config.Clear();
            _config.AddRange(YfasLogoSqlHelper.GetConfigModels().FindAll(f => f.ProductionName == _productName));
        }

        private void tscmbRoiType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _thisType = tscmbRoiType.SelectedIndex;

            switch (_thisType)
            {
                case 0:
                    Text = string.Format("[{0}]:{1}(外圈ROI标定)", _productName, "LOGO灯检测位置标定");
                    break;

                case 1:
                    Text = string.Format("[{0}]:{1}(“V”ROI标定)", _productName, "LOGO灯检测位置标定");
                    break;

                case 2:
                    Text = string.Format("[{0}]:{1}(“W”ROI标定)", _productName, "LOGO灯检测位置标定");
                    break;

                case 3:
                    Text = string.Format("[{0}]:{1}(“16点”ROI标定)", _productName, "LOGO灯检测位置标定");
                    break;
            }

            MainImageViewer.Roi.Clear();

            if (_thisType == 0)
            {
                var findOval = _config.Find(f => f.Position == "外圈" && f.Type == "Oval" && f.ProductionName == _productName);
                if (findOval != null)
                {
                    var shape = JsonConvert.DeserializeObject<OvalContour>(findOval.Object);

                    if (_srcMat != null && !_srcMat.Empty())
                        MainImageViewer.Roi.Add(shape);
                }
                else
                {
                    var drawOvalContour = new OvalContour(0, 0, 350, 350);
                    if (_srcMat != null && !_srcMat.Empty())
                        MainImageViewer.Roi.Add(drawOvalContour);
                }
            }
            else if (_thisType == 1)
            {
                var findVs = _config.FindAll(f => f.Position.StartsWith("V") && f.Type == "Line" && f.ProductionName == _productName);

                if (findVs.Count == 3)
                {
                    foreach (var v in findVs)
                    {
                        var shape = JsonConvert.DeserializeObject<LineContour>(v.Object);
                        if (_srcMat != null && !_srcMat.Empty()) MainImageViewer.Roi.Add(shape);
                    }
                }
                else
                {
                    for (var i = 0; i < 3; i++)
                    {
                        var drawLineContour = new LineContour(new PointContour(0, 0), new PointContour(100, 100));
                        if (_srcMat != null && !_srcMat.Empty()) MainImageViewer.Roi.Add(drawLineContour);
                    }
                }
            }
            else if (_thisType == 2)
            {
                var findWs = _config.FindAll(f => f.Position.StartsWith("W") && f.Type == "Line" && f.ProductionName == _productName);

                if (findWs.Count == 5)
                {
                    foreach (var v in findWs)
                    {
                        var shape = JsonConvert.DeserializeObject<LineContour>(v.Object);
                        if (_srcMat != null && !_srcMat.Empty()) MainImageViewer.Roi.Add(shape);
                    }
                }
                else
                {
                    for (var i = 0; i < 5; i++)
                    {
                        var drawLineContour = new LineContour(new PointContour(0, 0), new PointContour(100, 100));
                        if (_srcMat != null && !_srcMat.Empty()) MainImageViewer.Roi.Add(drawLineContour);
                    }
                }
            }
            else if (_thisType == 3)
            {
                var find16 =
                    _config.FindAll(
                        f => f.Position.StartsWith("圈-") && f.Type == "Oval" && f.ProductionName == _productName);

                if (find16.Count == 16)
                {
                    for (var i = 0; i < find16.Count; i++)
                    {
                        var shape = JsonConvert.DeserializeObject<OvalContour>(find16[i].Object);

                        if (_srcMat != null && !_srcMat.Empty())
                            MainImageViewer.Roi.Add(shape);
                    }
                }
            }

            if (_srcMat != null && !_srcMat.Empty())
                DrawContourCountInOverlay(MainImageViewer.Image, MainImageViewer.Roi);
        }

        private void FrmYfasLogoConfig_Closed(object sender, EventArgs e)
        {
            if (_timeAction != null)
                _timeAction.stop();
            if (_srcMat != null && !_srcMat.Empty())
                _srcMat.Dispose();
            if (MainImageViewer.Image != null)
                MainImageViewer.Image.Dispose();
        }

        private void FrmYfasLogoConfig_Load(object sender, EventArgs e)
        {
            tscmbRoiType.SelectedIndex = 0;
            SetMainViewerStyle();
            SetMainDgvStyle();
            MainImageViewer.SizeChanged += MainImageViewer_SizeChanged;
            MainImageViewer.RoiChanged += MainImageViewer_RoiChanged;
        }

        private void MainImageViewer_SizeChanged(object sender, EventArgs e)
        {
            SetMainViewerStyle();
        }

        private void MainImageViewer_RoiChanged(object sender, ContoursChangedEventArgs e)
        {
            DrawContourCountInOverlay(MainImageViewer.Image, MainImageViewer.Roi);
        }

        private void DrawContourCountInOverlay(VisionImage visionImage, Roi rois)
        {
            var textOptions = new OverlayTextOptions("Arial", 100) { TextDecoration = { Bold = true } };
            visionImage.Overlays.Default.Clear();

            for (var i = 0; i < rois.Count; i++)
            {
                var shape = rois[i].Shape;

                var shapeType = shape.GetType();
                PointContour temp;
                if (shapeType == typeof(OvalContour) && _thisType == 0)
                {
                    temp = new PointContour(((OvalContour)shape).Left + 100, ((OvalContour)shape).Top + 100);
                    visionImage.Overlays.Default.AddText("外圈", temp, Rgb32Value.GreenColor, textOptions);
                }
                else if (shapeType == typeof(LineContour) && _thisType == 1)
                {
                    temp = ((LineContour)shape).Start;
                    visionImage.Overlays.Default.AddText("V-" + (i + 1), temp, Rgb32Value.GreenColor, textOptions);
                }
                else if (shapeType == typeof(LineContour) && _thisType == 2)
                {
                    temp = ((LineContour)shape).Start;
                    visionImage.Overlays.Default.AddText("W-" + (i + 1), temp, Rgb32Value.GreenColor, textOptions);
                }
                else if (shapeType == typeof(OvalContour) && _thisType == 3)
                {
                    temp = new PointContour(((OvalContour)shape).Left + 100, ((OvalContour)shape).Top + 100);
                    visionImage.Overlays.Default.AddText("圈-" + (i + 1), temp, Rgb32Value.GreenColor, textOptions);
                }
            }
        }

        private void SetMainViewerStyle()
        {
            MainImageViewer.ToolsShown = ViewerTools.Selection | ViewerTools.ZoomIn | ViewerTools.ZoomOut | ViewerTools.Pan;

            MainImageViewer.ActiveTool = ViewerTools.Pan;
            MainImageViewer.ZoomToFit = true;
            MainImageViewer.ShowToolbar = true;
            MainImageViewer.ShowScrollbars = true;
            MainImageViewer.ShowImageInfo = true;
        }

        private void SetMainDgvStyle()
        {
            mainDgv.Style = UIStyle.Blue;
            //mainDgv.ReadOnly = true;
            mainDgv.RowHeadersVisible = false;
            mainDgv.AllowUserToAddRows = false;
            mainDgv.AllowUserToResizeRows = false;
            mainDgv.MultiSelect = true;
            mainDgv.RowHeadersVisible = false;
            mainDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            mainDgv.ClearRows();
            mainDgv.ClearColumns();
            mainDgv.AddColumn("名称", "名称");
            mainDgv.AddColumn("ROI类型", "ROI类型");
            mainDgv.AddColumn("ROI位置", "ROI位置");
            mainDgv.AddColumn("下限", "下限", readOnly: false);
            mainDgv.AddColumn("上限", "上限", readOnly: false);
            mainDgv.AddButtonColumn("更新", "更新");

            foreach (var enumValue in EnumOperater.GetEnumValueList<YfasLogoSqlHelper.CheckType>())
            {
                var des = enumValue.GetCustomAttribute<DescriptionAttribute>().Description;
                // var rowIndex = mainDgv.AddRow(des, "0", "255", "更新");

                double min;
                var max = double.PositiveInfinity;
                string type;
                string position;

                switch (enumValue)
                {
                    case YfasLogoSqlHelper.CheckType.Curr:
                        continue;
                    case YfasLogoSqlHelper.CheckType.Volt:
                        continue;

                    case YfasLogoSqlHelper.CheckType.OuterOvalAverage:
                        type = "Oval";
                        position = "外圈";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.OuterOvalUnif:
                        type = "Oval";
                        position = "外圈";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinHomo;
                        //max = _config.Find(f => f.Type == type && f.Position == position).MaxHomo;
                        break;

                    case YfasLogoSqlHelper.CheckType.V1Average:
                        type = "Line";
                        position = "V-1";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.V2Average:
                        type = "Line";
                        position = "V-2";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.V3Average:
                        type = "Line";
                        position = "V-3";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.VUnif:
                        type = "Line";
                        position = "V-1~3";
                        min = _config.Find(f => f.Type == type && f.Position.StartsWith("V-")).MinHomo;
                        break;

                    case YfasLogoSqlHelper.CheckType.VAvarage:
                        type = "Line";
                        position = "V-1~3";
                        min = _config.Find(f => f.Type == type && f.Position.StartsWith("V-")).MinAllAverage;
                        max = _config.Find(f => f.Type == type && f.Position.StartsWith("V-")).MaxAllAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner1OvalAverage:
                        type = "Oval";
                        position = "圈-1";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner2OvalAverage:
                        type = "Oval";
                        position = "圈-2";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner3OvalAverage:
                        type = "Oval";
                        position = "圈-3";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner4OvalAverage:
                        type = "Oval";
                        position = "圈-4";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner5OvalAverage:
                        type = "Oval";
                        position = "圈-5";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner6OvalAverage:
                        type = "Oval";
                        position = "圈-6";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner7OvalAverage:
                        type = "Oval";
                        position = "圈-7";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner8OvalAverage:
                        type = "Oval";
                        position = "圈-8";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner9OvalAverage:
                        type = "Oval";
                        position = "圈-9";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner10OvalAverage:
                        type = "Oval";
                        position = "圈-10";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner11OvalAverage:
                        type = "Oval";
                        position = "圈-11";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner12OvalAverage:
                        type = "Oval";
                        position = "圈-12";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner13OvalAverage:
                        type = "Oval";
                        position = "圈-13";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;


                    case YfasLogoSqlHelper.CheckType.Inner14OvalAverage:
                        type = "Oval";
                        position = "圈-14";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner15OvalAverage:
                        type = "Oval";
                        position = "圈-15";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.Inner16OvalAverage:
                        type = "Oval";
                        position = "圈-16";
                        min = _config.Find(f => f.Type == type && f.Position == position).MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == position).MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.InnerUnif:
                        type = "Oval";
                        position = "圈-1~16";
                        min = _config.Find(f => f.Type == type && f.Position.StartsWith("圈-")).MinHomo;
                        break;

                    case YfasLogoSqlHelper.CheckType.InnerAvarage:
                        type = "Oval";
                        position = "圈-1~16";
                        min = _config.Find(f => f.Type == type && f.Position.StartsWith("圈-")).MinAllAverage;
                        max = _config.Find(f => f.Type == type && f.Position.StartsWith("圈-")).MaxAllAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.W1Average:
                        type = "Line";
                        position = "W-1";
                        min = _config.Find(f => f.Type == type && f.Position == "W-1").MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == "W-1").MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.W2Average:
                        type = "Line";
                        position = "W-2";
                        min = _config.Find(f => f.Type == type && f.Position == "W-2").MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == "W-2").MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.W3Average:
                        type = "Line";
                        position = "W-3";
                        min = _config.Find(f => f.Type == type && f.Position == "W-3").MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == "W-3").MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.W4Average:
                        type = "Line";
                        position = "W-4";
                        min = _config.Find(f => f.Type == type && f.Position == "W-4").MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == "W-4").MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.W5Average:
                        type = "Line";
                        position = "W-5";
                        min = _config.Find(f => f.Type == type && f.Position == "W-5").MinAverage;
                        max = _config.Find(f => f.Type == type && f.Position == "W-5").MaxAverage;
                        break;

                    case YfasLogoSqlHelper.CheckType.WUnif:
                        type = "Line";
                        position = "W-1~5";
                        min = _config.Find(f => f.Type == type && f.Position.StartsWith("W-")).MinHomo;
                        break;

                    case YfasLogoSqlHelper.CheckType.WAverage:
                        type = "Line";
                        position = "W-1~5";
                        min = _config.Find(f => f.Type == type && f.Position.StartsWith("W-")).MinAllAverage;
                        max = _config.Find(f => f.Type == type && f.Position.StartsWith("W-")).MaxAllAverage;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (double.IsNegativeInfinity(min) && double.IsPositiveInfinity(max))
                    continue;

                var newRowIndex = mainDgv.AddRow(des, type, position, min, max, "更新当前行");
                if (double.IsNegativeInfinity(min))
                    mainDgv.Rows[newRowIndex].Cells[3].ReadOnly = true;
                if (double.IsPositiveInfinity(max))
                    mainDgv.Rows[newRowIndex].Cells[4].ReadOnly = true;

                _dicValues.Add(newRowIndex, new[] { min, max });
            }

            if (mainDgv.RowCount <= 0)
                return;
            mainDgv.AutoResizeColumns();
            mainDgv.AutoResizeRows();
            mainDgv.CellEndEdit += mainDgv_CellEndEdit;
            mainDgv.CellContentClick += mainDgv_CellContentClick;
        }

        private void mainDgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == mainDgv.ColumnCount - 1)
            {
                //this.ShowInfoTip(mainDgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                var min = double.Parse(mainDgv.Rows[e.RowIndex].Cells[3].Value.ToString());
                var max = double.Parse(mainDgv.Rows[e.RowIndex].Cells[4].Value.ToString());

                var showMsg = string.Format("确认信息：{0}名称：{1}{2}范围：{3}~{4}",
                    Environment.NewLine,
                    mainDgv.Rows[e.RowIndex].Cells[0].Value, Environment.NewLine,
                    min, max);

                if (this.ShowAskDialog(showMsg))
                {
                    var des = mainDgv.Rows[e.RowIndex].Cells[0].Value.ToString();

                    foreach (var enumValue in EnumOperater.GetEnumValueList<YfasLogoSqlHelper.CheckType>())
                    {
                        if (!string.IsNullOrEmpty(enumValue.GetCustomAttribute<DescriptionAttribute>().Description) &&
                            des == enumValue.GetCustomAttribute<DescriptionAttribute>().Description)
                        {
                            switch (enumValue)
                            {
                                case YfasLogoSqlHelper.CheckType.Curr:
                                    return;
                                case YfasLogoSqlHelper.CheckType.Volt:
                                    return;

                                case YfasLogoSqlHelper.CheckType.OuterOvalAverage:
                                    var outerOvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "外圈");
                                    outerOvalAverage.MinAverage = min;
                                    outerOvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(outerOvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.OuterOvalUnif:
                                    var outerOvalUnif = _config.Find(f => f.Type == "Oval" && f.Position == "外圈");
                                    outerOvalUnif.MinHomo = min;
                                    //outerOvalUnif.MaxHomo = max;
                                    YfasLogoSqlHelper.UpdateConfig(outerOvalUnif);
                                    break;

                                case YfasLogoSqlHelper.CheckType.V1Average:
                                    var v1Average = _config.Find(f => f.Type == "Line" && f.Position == "V-1");
                                    v1Average.MinAverage = min;
                                    v1Average.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(v1Average);
                                    break;

                                case YfasLogoSqlHelper.CheckType.V2Average:
                                    var v2Average = _config.Find(f => f.Type == "Line" && f.Position == "V-2");
                                    v2Average.MinAverage = min;
                                    v2Average.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(v2Average);
                                    break;

                                case YfasLogoSqlHelper.CheckType.V3Average:
                                    var v3Average = _config.Find(f => f.Type == "Line" && f.Position == "V-3");
                                    v3Average.MinAverage = min;
                                    v3Average.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(v3Average);
                                    break;

                                case YfasLogoSqlHelper.CheckType.VUnif:
                                    for (var i = 0; i < 3; i++)
                                    {
                                        var pi = i + 1;

                                        var vUnif = _config.Find(f => f.Type == "Line" && f.Position == "V-" + pi);
                                        vUnif.MinHomo = min;
                                        YfasLogoSqlHelper.UpdateConfig(vUnif);
                                    }
                                    break;

                                case YfasLogoSqlHelper.CheckType.VAvarage:
                                    for (var i = 0; i < 3; i++)
                                    {
                                        var pi = i + 1;

                                        var vAvarage = _config.Find(f => f.Type == "Line" && f.Position == "V-" + pi);
                                        vAvarage.MinAllAverage = min;
                                        vAvarage.MaxAllAverage = max;
                                        YfasLogoSqlHelper.UpdateConfig(vAvarage);
                                    }
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner1OvalAverage:
                                    var inner1OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-1");
                                    inner1OvalAverage.MinAverage = min;
                                    inner1OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner1OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner2OvalAverage:
                                    var inner2OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-2");
                                    inner2OvalAverage.MinAverage = min;
                                    inner2OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner2OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner3OvalAverage:
                                    var inner3OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-3");
                                    inner3OvalAverage.MinAverage = min;
                                    inner3OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner3OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner4OvalAverage:
                                    var inner4OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-4");
                                    inner4OvalAverage.MinAverage = min;
                                    inner4OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner4OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner5OvalAverage:
                                    var inner5OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-5");
                                    inner5OvalAverage.MinAverage = min;
                                    inner5OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner5OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner6OvalAverage:
                                    var inner6OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-6");
                                    inner6OvalAverage.MinAverage = min;
                                    inner6OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner6OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner7OvalAverage:
                                    var inner7OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-7");
                                    inner7OvalAverage.MinAverage = min;
                                    inner7OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner7OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner8OvalAverage:
                                    var inner8OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-8");
                                    inner8OvalAverage.MinAverage = min;
                                    inner8OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner8OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner9OvalAverage:
                                    var inner9OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-9");
                                    inner9OvalAverage.MinAverage = min;
                                    inner9OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner9OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner10OvalAverage:
                                    var inner10OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-10");
                                    inner10OvalAverage.MinAverage = min;
                                    inner10OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner10OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner11OvalAverage:
                                    var inner11OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-11");
                                    inner11OvalAverage.MinAverage = min;
                                    inner11OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner11OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner12OvalAverage:
                                    var inner12OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-12");
                                    inner12OvalAverage.MinAverage = min;
                                    inner12OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner12OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner13OvalAverage:
                                    var inner13OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-13");
                                    inner13OvalAverage.MinAverage = min;
                                    inner13OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner13OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner14OvalAverage:
                                    var inner14OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-14");
                                    inner14OvalAverage.MinAverage = min;
                                    inner14OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner14OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner15OvalAverage:
                                    var inner15OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-15");
                                    inner15OvalAverage.MinAverage = min;
                                    inner15OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner15OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.Inner16OvalAverage:
                                    var inner16OvalAverage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-16");
                                    inner16OvalAverage.MinAverage = min;
                                    inner16OvalAverage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(inner16OvalAverage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.InnerUnif:
                                    for (var i = 0; i < 16; i++)
                                    {
                                        var pi = i + 1;

                                        var innerUnif = _config.Find(f => f.Type == "Oval" && f.Position == "圈-" + pi);
                                        innerUnif.MinHomo = min;
                                        YfasLogoSqlHelper.UpdateConfig(innerUnif);
                                    }
                                    break;

                                case YfasLogoSqlHelper.CheckType.InnerAvarage:
                                    for (var i = 0; i < 16; i++)
                                    {
                                        var pi = i + 1;

                                        var innerAvarage = _config.Find(f => f.Type == "Oval" && f.Position == "圈-" + pi);
                                        innerAvarage.MinAllAverage = min;
                                        innerAvarage.MaxAllAverage = max;
                                        YfasLogoSqlHelper.UpdateConfig(innerAvarage);
                                    }
                                    break;

                                case YfasLogoSqlHelper.CheckType.W1Average:
                                    var w1AWerage = _config.Find(f => f.Type == "Line" && f.Position == "W-1");
                                    w1AWerage.MinAverage = min;
                                    w1AWerage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(w1AWerage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.W2Average:
                                    var w2AWerage = _config.Find(f => f.Type == "Line" && f.Position == "W-2");
                                    w2AWerage.MinAverage = min;
                                    w2AWerage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(w2AWerage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.W3Average:
                                    var w3AWerage = _config.Find(f => f.Type == "Line" && f.Position == "W-3");
                                    w3AWerage.MinAverage = min;
                                    w3AWerage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(w3AWerage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.W4Average:
                                    var w4AWerage = _config.Find(f => f.Type == "Line" && f.Position == "W-4");
                                    w4AWerage.MinAverage = min;
                                    w4AWerage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(w4AWerage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.W5Average:
                                    var w5AWerage = _config.Find(f => f.Type == "Line" && f.Position == "W-5");
                                    w5AWerage.MinAverage = min;
                                    w5AWerage.MaxAverage = max;
                                    YfasLogoSqlHelper.UpdateConfig(w5AWerage);
                                    break;

                                case YfasLogoSqlHelper.CheckType.WUnif:
                                    for (var i = 0; i < 5; i++)
                                    {
                                        var pi = i + 1;

                                        var wUnif = _config.Find(f => f.Type == "Line" && f.Position == "W-" + pi);
                                        wUnif.MinHomo = min;
                                        YfasLogoSqlHelper.UpdateConfig(wUnif);
                                    }
                                    break;

                                case YfasLogoSqlHelper.CheckType.WAverage:
                                    for (var i = 0; i < 5; i++)
                                    {
                                        var pi = i + 1;

                                        var wAvarage = _config.Find(f => f.Type == "Line" && f.Position == "W-" + pi);
                                        wAvarage.MinAllAverage = min;
                                        wAvarage.MaxAllAverage = max;
                                        YfasLogoSqlHelper.UpdateConfig(wAvarage);
                                    }
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                    }

                    _config.Clear();
                    _config.AddRange(YfasLogoSqlHelper.GetConfigModels().FindAll(f => f.ProductionName == _productName));
                    this.ShowSuccessTip("更新成功");
                }
                else
                {
                    this.ShowInfoTip("取消更新");
                }
            }
        }

        private readonly Dictionary<int, double[]> _dicValues = new Dictionary<int, double[]>();

        private void mainDgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == 3 || e.ColumnIndex == 4))
            {
                var value = mainDgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                double b;
                if (double.TryParse(value, out b))
                {
                    var ib = Math.Round(b, 0, MidpointRounding.AwayFromZero);
                    if (e.ColumnIndex == 3)
                    {
                        _dicValues[e.RowIndex][0] = ib;
                    }
                    else if (e.ColumnIndex == 4)
                    {
                        _dicValues[e.RowIndex][1] = ib;
                    }
                    mainDgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ib;
                }
                else
                {
                    if (e.ColumnIndex == 3)
                    {
                        mainDgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _dicValues[e.RowIndex][0];
                    }
                    else if (e.ColumnIndex == 4)
                    {
                        mainDgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _dicValues[e.RowIndex][1];
                    }
                    this.ShowErrorTip("请填写数字");
                }
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

                        if (_srcMat != null && !_srcMat.Empty())
                            _srcMat.Dispose();

                        _srcMat = new Mat(ofd.FileName, ImreadModes.AnyColor);

                        var baseAngle = FrmYfasLogoCheckMain.RearBaseAngle;
                        var maxSize = FrmYfasLogoCheckMain.RearMaxSize;

                        if (_product == YfasLogoSqlHelper.ProductType.FrontLamp)
                        {
                            baseAngle = FrmYfasLogoCheckMain.FrontBaseAngle;
                            maxSize = FrmYfasLogoCheckMain.FrontMaxSize;
                        }

                        YfasLogoGrayHelper.AutoRotate(baseAngle, maxSize, ref _srcMat);

                        MainImageViewer.Roi.Clear();
                        var visionImg = MatToVisionImage(_srcMat);

                        Algorithms.Copy(visionImg, MainImageViewer.Image);
                        visionImg.Dispose();

                        tscmbRoiType_SelectedIndexChanged(null, null);
                        st.Stop();
                        //Console.WriteLine(@"处理耗时：{0}ms", st.ElapsedMilliseconds);

                        listBox1.Items.Add(string.Format("加载图片='{0}', cost={1}ms", ofd.FileName, st.ElapsedMilliseconds));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            if (!this.ShowAskDialog("是否确定更新？"))
            {
                this.ShowInfoTip("操作已取消");
                return;
            }

            if (_thisType == 0)
            {
                if (MainImageViewer.Roi.Count == 0)
                {
                    this.ShowErrorTip("请先添加标定区域！");
                    return;
                }

                var oval = _config.Find(f => f.Position == "外圈" && f.Type == "Oval" && f.ProductionName == _productName);

                if (oval != null)
                {
                    var shape = MainImageViewer.Roi[0].Shape;
                    oval.Object = JsonConvert.SerializeObject(shape);
                    YfasLogoSqlHelper.UpdateConfig(oval);
                }
                else
                {
                    oval = new YfasLogoSqlHelper.LogoConfigModel
                    {
                        MaxGray = 255,
                        MinGray = 0,
                        MaxHomo = 100,
                        MinHomo = 0,
                        Object = JsonConvert.SerializeObject(new OvalContour(0, 0, 350, 350)),
                        Position = "外圈",
                        ProductionName = _productName,
                        Type = "Oval",
                        MinAverage = 0,
                        MaxAverage = 255
                    };
                    YfasLogoSqlHelper.AddConfig(oval);
                }

                _config.Clear();
                _config.AddRange(YfasLogoSqlHelper.GetConfigModels());
            }
            else if (_thisType == 1 || _thisType == 2)
            {
                if ((MainImageViewer.Roi.Count != 3 && _thisType == 1) || (MainImageViewer.Roi.Count != 5 && _thisType == 2))
                {
                    this.ShowErrorTip("请先添加标定区域！");
                    return;
                }

                for (var i = 0; i < MainImageViewer.Roi.Count; i++)
                {
                    var roi = MainImageViewer.Roi[i];

                    var type = "Line";

                    var position = string.Empty;
                    if (_thisType == 1)
                        position = "V-" + (i + 1);
                    else if (_thisType == 2)
                        position = "W-" + (i + 1);

                    var line = _config.Find(f => f.Position == position && f.Type == type && f.ProductionName == _productName);
                    if (line != null)
                    {
                        var shape = roi.Shape;
                        line.Object = JsonConvert.SerializeObject(shape);
                        YfasLogoSqlHelper.UpdateConfig(line);
                    }
                    else
                    {
                        line = new YfasLogoSqlHelper.LogoConfigModel
                        {
                            MaxGray = 255,
                            MinGray = 0,
                            MaxHomo = 100,
                            MinHomo = 0,
                            Object = JsonConvert.SerializeObject(new LineContour(new PointContour(0, 0), new PointContour(100, 100))),
                            Position = position,
                            ProductionName = _productName,
                            Type = "Line",
                            MinAverage = 0,
                            MaxAverage = 255
                        };
                        YfasLogoSqlHelper.AddConfig(line);
                    }
                }

                _config.Clear();
                _config.AddRange(YfasLogoSqlHelper.GetConfigModels());
            }
            else
            {
                if (MainImageViewer.Roi.Count != 16)
                {
                    this.ShowErrorTip("请先添加标定区域！");
                    return;
                }

                for (var i = 0; i < MainImageViewer.Roi.Count; i++)
                {
                    var roi = MainImageViewer.Roi[i];

                    var type = "Oval";
                    var position = "圈-" + (i + 1);

                    var oval = _config.Find(f => f.Position == position && f.Type == type && f.ProductionName == _productName);
                    if (oval != null)
                    {
                        var shape = roi.Shape;
                        oval.Object = JsonConvert.SerializeObject(shape);
                        YfasLogoSqlHelper.UpdateConfig(oval);
                    }
                    else
                    {
                        oval = new YfasLogoSqlHelper.LogoConfigModel
                        {
                            MaxGray = 255,
                            MinGray = 0,
                            MaxHomo = 100,
                            MinHomo = 0,
                            Object = JsonConvert.SerializeObject(new OvalContour(0, 0, 350, 350)),
                            Position = "外圈",
                            ProductionName = _productName,
                            Type = "Oval",
                            MinAverage = 0,
                            MaxAverage = 255
                        };
                        YfasLogoSqlHelper.AddConfig(oval);
                    }
                }

                _config.Clear();
                _config.AddRange(YfasLogoSqlHelper.GetConfigModels());
            }
            this.ShowSuccessTip("更新成功");
        }

        public Mat VisionImageToMat(VisionImage visionImg)
        {
            var pixelValue = visionImg.ImageToArray();

            var st = new Stopwatch();
            st.Start();

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

        public VisionImage MatToVisionImage(Mat matImage)
        {
            var channel = matImage.Channels();

            var bytes = new byte[matImage.Total() * matImage.ElemSize()]; // 创建与图像大小相匹配的字节数组

            Marshal.Copy(matImage.Data, bytes, 0, bytes.Length); // 将Mat对象的数据复制到字节数组中

            return ImageProcessing.ConvertBytesToVisionImg(bytes, matImage.Cols, matImage.Rows, channel > 1);
        }

        private void tsbtnCalculate_Click(object sender, EventArgs e)
        {
            if (_srcMat == null || _srcMat.Empty())
                return;

            //if (_srcMat != null && !_srcMat.Empty())
            //{
            //    _srcMat.Dispose();
            //}

            //_srcMat = new Mat(@"D:\Projects\文档\延锋-logo灯\测试图片-20240105\实际检测图片.bmp", ImreadModes.AnyColor);

            //var config = YfasLogoSqlHelper.GetConfigModels();

            if (_thisType == 0)
            {
                var findOval = _config.Find(f => f.Position == "外圈" && f.Type == "Oval" && f.ProductionName == _productName);
                if (findOval == null)
                {
                    this.ShowErrorTip("计算失败");
                    return;
                }
                var st = new Stopwatch();
                st.Start();

                var result = YfasLogoGrayHelper.OvalGray(_srcMat, findOval.Object);
                listBox1.Items.Add(string.Format("[“外圈”计算结果]: Min={0}, Max={1}, Average={2}, Unif={3}%, cost={4}ms", result.Min, result.Max, result.Average, result.Unif4, st.ElapsedMilliseconds));

                st.Stop();
            }
            else if (_thisType == 1 || _thisType == 2)
            {
                var pos = _thisType == 1 ? "V-" : "W-";

                var findRects = _config.FindAll(f => f.Position.StartsWith(pos) && f.Type == "Line" && f.ProductionName == _productName);

                if (!findRects.Any())
                {
                    this.ShowErrorTip("计算失败");
                    return;
                }
                var st = new Stopwatch();
                st.Start();

                var objs = findRects.Select(t => t.Object).ToList();

                YfasLogoGrayHelper.GrayResult[] grayResults;
                double unifAll;
                double averageAll;
                YfasLogoGrayHelper.LinesGray(_srcMat, objs.ToArray(), out grayResults, out unifAll, out averageAll);

                for (var i = 0; i < grayResults.Length; i++)
                    listBox1.Items.Add(string.Format("[“{0}”计算结果]: pos={1}, Min={2}, Max={3}, Average={4}, cost={5}ms", _thisType == 1 ? "V" : "W", pos + (i + 1), grayResults[i].Min, grayResults[i].Max, grayResults[i].Average, st.ElapsedMilliseconds));

                listBox1.Items.Add(string.Format("[“{0}”整体计算结果]: Average={1}, Unif={2}%, cost={3}ms", _thisType == 1 ? "V" : "W", averageAll, unifAll, st.ElapsedMilliseconds));

                st.Stop();
            }
            else
            {
                var pos = "圈-";

                var findRects = _config.FindAll(f => f.Position.StartsWith(pos) && f.Type == "Oval" && f.ProductionName == _productName);

                if (!findRects.Any())
                {
                    this.ShowErrorTip("计算失败");
                    return;
                }
                var st = new Stopwatch();
                st.Start();

                YfasLogoGrayHelper.GrayResult[] grayResults;
                double unifAll;
                double averageAll;
                YfasLogoGrayHelper.OvalsGray(_srcMat, findRects.Select(t => t.Object).ToArray(), out grayResults, out unifAll, out averageAll);

                var avgGrays = new List<double>();

                for (var i = 0; i < grayResults.Length; i++)
                {
                    listBox1.Items.Add(string.Format("[“{0}”计算结果]: pos={1}, Min={2}, Max={3}, Average={4}", "16点",
                        pos + (i + 1), grayResults[i].Min, grayResults[i].Max, grayResults[i].Average));
                    avgGrays.Add(grayResults[i].Average);
                }

                var avgGrayMin = avgGrays.Min();
                var avgGrayMax = avgGrays.Max();

                var unifByAllAvg= Math.Round(avgGrayMin / avgGrayMax * 100, 2, MidpointRounding.AwayFromZero);

                listBox1.Items.Add(string.Format("[“{0}”整体计算结果]: Average={1}, Unif={2}%, cost={3}ms", "16点", averageAll, unifByAllAvg, st.ElapsedMilliseconds));

                st.Stop();
            }
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            this.ShowInfoTip("数据已清空");
        }
    }
}
