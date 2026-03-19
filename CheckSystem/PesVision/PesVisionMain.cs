using CheckSystem.VisionDetection;
using CommonUtility;
using CommonUtility.FileOperator;
using CommonUtility.HikSdk;
using Controller;
using HZH_Controls.IconFont;
using Model;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using Newtonsoft.Json;
using OpenCvSharp;
using StateMachine;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Windows.Forms;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.PesVision
{
    public partial class PesVisionMain : UIForm
    {
        public readonly IniFileHelper IniFile =
            new IniFileHelper(Program.SysDir + @"\PesConfig" + @"\PesVisionConfig.ini");

        private readonly ImageViewer _mainImageViewer = new ImageViewer { Dock = DockStyle.Fill };
        private readonly Dictionary<int, DateTime> _dateTimeList = new Dictionary<int, DateTime>();
        private readonly Mat _defaultBlackMat = new Mat(300, 300, MatType.CV_8UC3, new Scalar(0, 0, 0));
        private VisionImage _defaultBlackVisionImage;

        private readonly Dictionary<string, ImageViewer> _imageViewers = new Dictionary<string, ImageViewer>();
        private readonly object _lockImageViewer = new object();

        public PesVisionMain()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Load += PesVisionMain_Load;
            Closed += PesVisionMain_Closed;
        }

        private void PesVisionMain_Load(object sender, EventArgs e)
        {
            Icon = FontImages.GetIcon(FontIcons.E_icon_lightbulb, 32,
                Color.DodgerBlue);

            var xml = IniFile.IniReadValue("System", "Xml") + ".xml";
            var cameraSn = IniFile.IniReadValue("System", "CameraSn");

            St = new State();
            St.Init<ControllerBase>(string.Format(@"{0}\{1}\{2}", Program.SysDir, "流程配置文件", xml), "Controller.dll");
            lblTitle.Text = St.DeviceConfig.DeviceInfo.DeviceName;

            if (St.BarcodeGroupList.Count != 0 || St.LedGroupList.Count != 0)
            {
                if (new FormGroupSelect(St).ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show(@"请重启程序选择档位！");
                    return;
                }
            }

            InitUi();
            Start();
        }

        private void PesVisionMain_Closed(object sender, EventArgs e)
        {
            if (PesVisionAnalysis != null)
                PesVisionAnalysis.Dispose();
            Environment.Exit(0);
        }

        private void InitUi()
        {
            _defaultBlackVisionImage = CommonUtility.HikSdk.MyCamera.MatToVisionImage(_defaultBlackMat);
            uiPanel1.Controls.Add(_mainImageViewer);
            InitImageViewer(_mainImageViewer);
            InitMainDgv();

            uiRichTextBox1.Clear();
            Algorithms.Copy(_defaultBlackVisionImage, _mainImageViewer.Image);
        }

        private void InitMainDgv()
        {
            //uiMarkLabel1.Text = string.Format(@"测试时间：等待开始{0}测试结果：等待开始{1}测试耗时：等待开始", Environment.NewLine, Environment.NewLine);
            //uiLedBulb1.Color = Color.DarkGoldenrod;

            mainDgv.Style = UIStyle.Gray;
            mainDgv.ReadOnly = true;
            mainDgv.RowHeadersVisible = false;
            mainDgv.AllowUserToAddRows = false;
            mainDgv.AllowUserToResizeRows = false;
            mainDgv.MultiSelect = true;
            mainDgv.RowHeadersVisible = false;
            mainDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            mainDgv.ClearRows();
            mainDgv.ClearColumns();
            mainDgv.AddColumn("名称", "名称");
            mainDgv.AddColumn("标准", "标准");
            mainDgv.AddColumn("结果", "结果");

            mainDgv.AddRow("检测结果", "OK", "", "", "", "");
            mainDgv.AddRow("检测时间", "/", "", "", "", "");
            mainDgv.AddRow("检测耗时", "/", "", "", "", "");
            mainDgv.AddRow("检测计数", "/", "", "", "", "");

            //mainDgv.AutoResizeColumns();

            _dateTimeList.Clear();
            _dateTimeList.Add(2, DateTime.Now);
        }

        private void InitImageViewer(ImageViewer imageViewer)
        {
            ImageShowTool(imageViewer, true);
            imageViewer.SizeChanged += imageViewer_SizeChanged;
        }

        private void ImageShowTool(ImageViewer imageViewer, bool isNew = false)
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
            imageViewer.AutoDelete = true;
        }

        private void imageViewer_SizeChanged(object sender, EventArgs e)
        {
            var imageViewer = sender as ImageViewer;
            if (imageViewer != null)
                ImageShowTool(imageViewer);
        }

        #region 流程

        private State St { get; set; }
        private Thread Th { get; set; }
        private PesVisionAnalysisNDLB PesVisionAnalysis { get; set; }

        private void Start()
        {
            var pesVisionAnalysis = St.LstControllers.Find(f => f is PesVisionAnalysisNDLB);
            if (pesVisionAnalysis != null)
            {
                PesVisionAnalysis = pesVisionAnalysis as PesVisionAnalysisNDLB;

                if (PesVisionAnalysis != null)
                {
                    //PesVisionAnalysis.RoiStr = IniFile.IniReadValue("CutOffLine", "ROI");
                    //PesVisionAnalysis.SetInterestedRoi();

                    //PesVisionAnalysis.DenoiseSigma = double.Parse(IniFile.IniReadValue("CutOffLine", "Denoise"));
                    //PesVisionAnalysis.ScanPixelStep = int.Parse(IniFile.IniReadValue("CutOffLine", "PixelStep"));
                    //PesVisionAnalysis.PerDegreePixel = double.Parse(IniFile.IniReadValue("CutOffLine", "PerDegreePixel"));

                    //PesVisionAnalysis.LbStandardStr = IniFile.IniReadValue("LowBeam", "Points");
                    //PesVisionAnalysis.LowBeamGradient = double.Parse(IniFile.IniReadValue("LowBeam", "Gradient"));
                    //PesVisionAnalysis.SetLowBeamStandard();

                    //PesVisionAnalysis.HbStandardStr = IniFile.IniReadValue("HighBeam", "Points");
                    //PesVisionAnalysis.HighBeamH0V0EmaxRatio = double.Parse(IniFile.IniReadValue("HighBeam", "H0V0EmaxRatio"));
                    //PesVisionAnalysis.SetHighBeamStandard();

                    //lock (_lockImageViewer)
                    //{
                    //    foreach (var t in PesVisionAnalysis.ImgMats)
                    //    {
                    //        uiComboBox1.Items.Add(t.Key);
                    //        _imageViewers.Add(t.Key, new ImageViewer { Dock = DockStyle.Fill });
                    //    }
                    //}

                    //uiComboBox1.SelectedIndex = 0;
                }
            }

            //St.PushEnter += St_PushEnter;
            St.PushDisplay += St_PushDisplay;
            St.PushEndResult += St_PushEndResult;

            TaskCheck();
        }

        private void BindDictionaryToComboBox()
        {
            if (!PesVisionAnalysis.Images.Any())
            {
                // 清除当前绑定数据
                cmbImageList.DataSource = null;
                cmbImageList.Items.Clear();

                var emptyImage = new VisionImage();
                Algorithms.Copy(emptyImage, _mainImageViewer.Image);
                emptyImage.Dispose();
                return;
            }

            cmbImageList.DataSource = new BindingSource(PesVisionAnalysis.Images, null);
            cmbImageList.DisplayMember = "Key"; // 显示的文本
            cmbImageList.ValueMember = "Key"; // 实际绑定的值

            cmbImageList.SelectedIndex = cmbImageList.Items.Count - 1;
        }

        private void TaskCheck()
        {
            if (Th != null)
            {
                Th.Abort();
                Th.Join();
            }

            Th = new Thread(() => { St.TaskCheckStatus(); }) { IsBackground = true };
            Th.Start();
        }

        private void St_PushDisplay(List<CheckData> value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    foreach (var v in value)
                    {
                        var v1 = v;

                        var para =
                            St.DeviceConfig.Paras.ToList()
                                .Find(t => t.ProcessNo == v1.ProcessNo && t.Name == v1.ParaName);

                        if (para == null)
                            continue;

                        // 查找参数表对应界面控件
                        //UIDataGridView dgv = mainDgv1;
                        //UITitlePanel panel = ws1Panel;
                        var cellIndex = 2;

                        if (mainDgv.Rows.Count >= 4)
                        {
                            if (mainDgv.Rows[0].Cells[cellIndex].Style.BackColor != mainDgv.Rows[0].Cells[0].Style.BackColor)
                            {
                                for (var i = 0; i < mainDgv.Rows.Count; i++)
                                {
                                    mainDgv.Rows[i].Cells[cellIndex].Style.BackColor =
                                        mainDgv.Rows[i].Cells[0].Style.BackColor;
                                    mainDgv.Rows[i].Cells[cellIndex].Value = string.Empty;
                                }

                                if (_dateTimeList.ContainsKey(cellIndex))
                                {
                                    _dateTimeList[cellIndex] = DateTime.Now;
                                }

                                lblBarcodeScan.Text = @"二维码扫描：";
                                lblBarcodeScan.BackColor = Color.LightGray;
                                //uiRichTextBox1.Clear();
                                //lock (_lockImageViewer)
                                //{
                                //    Algorithms.Copy(_defaultBlackVisionImage, _mainImageViewer.Image);

                                //    foreach (var t in _imageViewers)
                                //        Algorithms.Copy(_defaultBlackVisionImage, t.Value.Image);
                                //}
                                BindDictionaryToComboBox();
                            }

                            mainDgv.Rows[0].Cells[cellIndex].Value = @"正在检测";

                            var name = v1.ParaName;
                            var range = v1.Range;
                            var checkValue = v.Value;
                            var checkResult = v1.Result.ToLower();

                            if (para.DataType.ToLower() == "barcode")
                                lblBarcodeScan.Text = @"二维码扫描：" +
                                                      Encoding.ASCII.GetBytes(checkValue).GetStringByAsciiBytes(true);

                            if (!string.IsNullOrEmpty(v1.Unit))
                            {
                                checkValue += @"/" + v1.Unit;
                                range += @"/" + v1.Unit;
                            }

                            var isHave = -1;
                            for (var i = 4; i < mainDgv.RowCount; i++)
                            {
                                if (mainDgv.Rows[i].Cells[0].Value.ToString() != name)
                                    continue;

                                isHave = i;
                                break;
                            }

                            if (isHave >= 4)
                            {
                                mainDgv.Rows[isHave].Cells[cellIndex].Value = checkValue;
                                mainDgv.Rows[isHave].Cells[cellIndex].Style.BackColor =
                                    checkResult == "false" || checkResult == "0"
                                        ? Color.Red
                                        : mainDgv.Rows[isHave].Cells[cellIndex].Style.BackColor;
                            }
                            else
                            {
                                var index = mainDgv.Rows.Add();
                                mainDgv.Rows[index].Cells[0].Value = name;
                                mainDgv.Rows[index].Cells[1].Value = range;
                                mainDgv.Rows[index].Cells[cellIndex].Value = checkValue;
                                mainDgv.Rows[index].Cells[cellIndex].Style.BackColor =
                                    checkResult == "false" || checkResult == "0"
                                        ? Color.Red
                                        : mainDgv.Rows[index].Cells[cellIndex].Style.BackColor;
                            }

                            mainDgv.FirstDisplayedScrollingRowIndex = mainDgv.RowCount - 1;
                        }
                    }
                }));
            }
        }

        private readonly string _currentPrName = @"NDLB_PES_远近光模组总成";
        private readonly string _currentPrNo = @"PR4104007152";
        private readonly string _currentDeviceNo = @"IN4304000220";

        private void St_PushEndResult(string processNo, List<CheckData> checkValues)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    var gp = checkValues.GroupBy(g => g.ProcessNo).ToDictionary(p => p.Key, p => p.ToList());

                    foreach (var key in gp.Keys)
                    {
                        var tempCheckValues =
                            gp[key].FindAll(
                                f =>
                                    f.ParaName != "样件检测结果" && f.ParaName != "检测结果OK" && f.ParaName != "检测结果NG");

                        if (!tempCheckValues.Any())
                            continue;

                        var isNg =
                            tempCheckValues.FindAll(
                                f => string.Equals(f.Result, false.ToString(), StringComparison.CurrentCultureIgnoreCase)).Any();
                        var okCount = 0;
                        var totalCount = 0;
                        var taskId = Guid.NewGuid().ToString();
                        //var process = processNo.Substring(0, processNo.Length - 3);

                        BindDictionaryToComboBox();

                        //uiRichTextBox1.Clear();
                        //lock (_lockImageViewer)
                        //{
                        //    Algorithms.Copy(_defaultBlackVisionImage, _mainImageViewer.Image);

                        //    foreach (var t in PesVisionAnalysis.ImgMats)
                        //    {
                        //        if (t.Value != null && !t.Value.Empty())
                        //        {
                        //            MatCopyToImageViewer(t.Value, _imageViewers[t.Key]);
                        //        }
                        //        //Algorithms.Copy(_defaultBlackVisionImage, t.Value.Image);
                        //    }
                        //}

                        //uiRichTextBox1.AppendText(PesVisionAnalysis.VisionAnalysisMsg);

                        //var showImgIndex0 = -1;
                        //for (var i = 0; i < cmbImageList.Items.Count; i++)
                        //{
                        //    var imgKey = cmbImageList.Items[i].ToString();
                        //    if (PesVisionAnalysis.ImgMats[imgKey] != null)
                        //    {
                        //        showImgIndex0 = i;
                        //        //showImgIndex = imgKey;
                        //    }
                        //}

                        //PesVisionAnalysis.Release();
                        //var showImgIndex = string.Empty;

                        //if (showImgIndex0 != -1)
                        //{
                        //    lock (_lockImageViewer)
                        //    {
                        //        var imgKey = cmbImageList.Items[showImgIndex0].ToString();
                        //        if (!string.IsNullOrEmpty(imgKey))
                        //        {
                        //            //MatCopyToImageViewer(PesVisionAnalysis.ImgMats[imgKey], _mainImageViewer);
                        //            Algorithms.Copy(_imageViewers[imgKey].Image, _mainImageViewer.Image);
                        //        }
                        //    }

                        //    cmbImageList.SelectedIndex = showImgIndex0;
                        //}
                        //uiTabControl1.SelectedIndex = 0;

                        #region 数据存储
                        var listBarcodeStrs = new List<string>();

                        try
                        {
                            var modelList = new List<manufactureCheckData>();
                            var bll = new BLL.manufactureCheckData();

                            var barcodeList = tempCheckValues.FindAll(f => f.Type != null && f.Type.ToLower().Equals("barcodegroup"));
                            if (tempCheckValues.FindAll(f => f.Type != null && f.Type.ToLower().Equals("barcode")).Any())
                                barcodeList.AddRange(tempCheckValues.FindAll(f => f.Type.ToLower().Equals("barcode")));

                            var pcbaCodePara = tempCheckValues.Find(f => f.ParaName.ToLower().Contains("PCBA二维码".ToLower()));
                            var pcbaCode = string.Empty;
                            if (pcbaCodePara != null)
                                pcbaCode = pcbaCodePara.Value;

                            if (barcodeList.Count > 0)
                            {
                                foreach (var t1 in barcodeList)
                                {
                                    listBarcodeStrs.Add(t1.Value);

                                    var model = new manufactureCheckData
                                    {
                                        taskNo = taskId,
                                        productBarcode = t1.Value,
                                        productUid = t1.Value,
                                        checkStaffNo = "admin",
                                        productNo = _currentPrNo,
                                        processNo = string.Format("{0}_{1}", _currentPrNo, key.Substring(key.Length - 3, 3)),
                                        checkResult = isNg ? "0002" : "0001",
                                        checkDate = DateTime.Now,
                                        createTime = DateTime.Now,
                                        creater = string.Format("{0}_{1}_Mac:{2}", St.DeviceConfig.DeviceInfo.DeviceName,
                                            key.Substring(key.Length - 3, 3),
                                            MacAddressHelper.GetMacByIpConfig().Replace("-", string.Empty)),
                                        pcbaBarcode = pcbaCode,
                                    };

                                    var listCheckNameAndData =
                                        tempCheckValues.FindAll(
                                                f =>
                                                    f.Type.ToLower() != "barcode" && f.Type.ToLower() != "barcodegroup" &&
                                                    f.Type.ToLower() != "vision")
                                            .Select(t => new FormCheck.CheckNameAndData { Name = t.ParaName, Data = t.Value })
                                            .ToList();

                                    model.checkData = JsonConvert.SerializeObject(listCheckNameAndData);
                                    if (model.checkData.Length > 4000)
                                        model.checkData = model.checkData.Substring(0, 4000);
                                    modelList.Add(model);

                                    //if (!checkValues.FindAll(f => f.ParaName == "样件检测结果").Any())
                                    bll.Add(model);
                                }
                            }
                            else
                            {
                                var model = new manufactureCheckData
                                {
                                    taskNo = taskId,
                                    productBarcode = string.Empty,
                                    productUid = string.Empty,
                                    checkStaffNo = "admin",
                                    productNo = _currentPrNo,
                                    processNo = string.Format("{0}_{1}", _currentPrNo, key.Substring(key.Length - 3, 3)),
                                    checkResult = isNg ? "0002" : "0001",
                                    checkDate = DateTime.Now,
                                    createTime = DateTime.Now,
                                    creater = string.Format("{0}_{1}_Mac:{2}", St.DeviceConfig.DeviceInfo.DeviceName,
                                        key.Substring(key.Length - 3, 3),
                                        MacAddressHelper.GetMacByIpConfig().Replace("-", string.Empty)),
                                    pcbaBarcode = pcbaCode,
                                };

                                var listCheckNameAndData =
                                    tempCheckValues.FindAll(
                                            f =>
                                                f.Type.ToLower() != "barcode" && f.Type.ToLower() != "barcodegroup" &&
                                                f.Type.ToLower() != "vision")
                                        .Select(t => new FormCheck.CheckNameAndData { Name = t.ParaName, Data = t.Value })
                                        .ToList();

                                model.checkData = JsonConvert.SerializeObject(listCheckNameAndData);
                                if (model.checkData.Length > 4000)
                                    model.checkData = model.checkData.Substring(0, 4000);
                                modelList.Add(model);

                                //if (!checkValues.FindAll(f => f.ParaName == "样件检测结果").Any())
                                bll.Add(model);
                            }

                            var listCheckDetail = new List<SyProductionSaveCheckData.CheckDataDetail>();
                            foreach (var t in tempCheckValues.Where(t => t.Type.ToLower() != "barcode" && t.Type.ToLower() != "barcodegroup"))
                            {
                                if (t.Type.ToLower() == "vision")
                                {
                                    listCheckDetail.Add(new SyProductionSaveCheckData.CheckDataDetail
                                    {
                                        Type = t.Type,
                                        ParaName = t.ParaName,
                                        Range = "/",
                                        Result = t.Result,
                                        Value = t.Result
                                    });
                                }
                                else
                                {
                                    listCheckDetail.Add(new SyProductionSaveCheckData.CheckDataDetail
                                    {
                                        Type = t.Type,
                                        ParaName = t.ParaName,
                                        Range = t.Range.TrimStart('~'),
                                        Result = t.Result,
                                        Value = t.Value
                                    });
                                }
                            }

                            string syCreator;
                            try
                            {
                                syCreator = _currentPrNo + key.Substring(key.Length - 3, 3);
                            }
                            catch (Exception)
                            {
                                syCreator = string.Empty;
                            }

                            if (!string.IsNullOrEmpty(pcbaCode))
                            {
                                listBarcodeStrs.Add(pcbaCode);
                            }

                            if (modelList.Any())
                            {
                                var model = modelList[0];

                                //okCount = bll.GetCountByTaskNo(string.Format(
                                //    "processNo = '{0}' and checkResult = '{1}' and DATEDIFF(DD,createTime,GETDATE()) = 0 and creater = '{2}'",
                                //    model.processNo, "0001", model.creater));

                                //totalCount =
                                //    bll.GetCountByTaskNo(
                                //        string.Format(
                                //            "processNo = '{0}' and DATEDIFF(DD,createTime,GETDATE()) = 0 and creater = '{1}'",
                                //model.processNo, model.creater));

                                GetCount(St.DeviceConfig.DeviceInfo.DeviceName, string.Format("{0}_{1}", _currentPrNo, processNo.Substring(processNo.Length - 3, 3)), ref totalCount, ref okCount);

                                foreach (var m in modelList)
                                {
                                    SaveDataLocal(m, !isNg, checkValues.FindAll(f => f.ParaName == "样件检测结果").Any(), St.DeviceConfig.DeviceInfo.DeviceName, string.Format("{0}_{1}", _currentPrNo, processNo.Substring(processNo.Length - 3, 3)));
                                }
                            }

                            SyProductionSaveCheckData.SaveData(
                              !isNg,
                              false,
                              _currentDeviceNo, _currentPrName,
                              _currentPrNo,
                              listBarcodeStrs, listCheckDetail, syCreator, totalCount);


                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                        #endregion

                        mainDgv.ClearSelection();

                        foreach (var checkValue in tempCheckValues)
                        {
                            // 查找参数表对应的控件
                            var para =
                                St.DeviceConfig.Paras.ToList()
                                    .Find(t => t.ProcessNo == checkValue.ProcessNo && t.Name == checkValue.ParaName);
                            if (para == null)
                                continue;

                            // 查找参数表对应界面控件
                            var cellIndex = 2;

                            var costTime = -1.0;
                            if (_dateTimeList.ContainsKey(cellIndex))
                                costTime =
                                    Math.Round(ValueHelper.GetTimeSpanMs(_dateTimeList[cellIndex], DateTime.Now) / (float)1000, 1,
                                        MidpointRounding.AwayFromZero);

                            if (mainDgv.RowCount >= 4)
                            {
                                mainDgv.Rows[0].Cells[cellIndex].Value = isNg ? "NG" : "OK";
                                mainDgv.Rows[0].Cells[cellIndex].Style.BackColor = isNg ? Color.Red : Color.Green;
                                mainDgv.Rows[1].Cells[cellIndex].Value = DateTime.Now.ToShortTimeString();
                                mainDgv.Rows[2].Cells[cellIndex].Value = costTime + "秒";
                                mainDgv.Rows[3].Cells[cellIndex].Value = "/";

                                lblBarcodeScan.BackColor = isNg ? Color.Red : Color.Green;

                                mainDgv.Rows[3].Cells[cellIndex].Value = string.Format("OK={0},NG={1}/总:{2}", okCount, totalCount - okCount, totalCount);
                                break;
                            }
                        }
                    }
                }));
            }
        }

        private void SaveDataLocal(Model.manufactureCheckData model, bool isOk, bool isSampleTest, string title, string controlName)
        {
            //var st = new Stopwatch();
            //st.Start();

            var taskid = model.taskNo;
            var productNo = model.productNo;
            var uid = model.productUid;
            var checkData = model.checkData;

            var saveLocalData = new LocalDbHelper.manufactureCheckData
            {
                taskNo = taskid,
                productNo = productNo,
                productUid = uid,
                pcbaNo = string.Empty,
                pcbaBarcode = uid,
                productBarcode = uid,
                packageBarcode = string.Empty,
                processNo = productNo + "_001",
                checkData = JsonConvert.SerializeObject(checkData),
                checkDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                checkStaffNo = "admin",
                checkResult = isSampleTest ? (isOk ? "9001" : "9002") : (isOk ? "0001" : "0002"),
                creater = Program.DeviceNo + ":" + title + "_" + controlName,
                createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                checkDataText = string.Empty
            };

            LocalDbHelper.InsertData(new LocalDbHelper.manufactureCheckData[] { saveLocalData });

            //if (LocalDbHelper.LocalSqlite != null)
            //{
            //    try
            //    {
            //        var insert =
            //            string.Format(
            //                "insert into manufactureCheckData (taskNo,productNo,productUid,pcbaNo,pcbaBarcode,productBarcode,packageBarcode,processNo,checkData,checkDate,checkStaffNo,checkResult,creater,createTime,checkDataText) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')",
            //                taskid, productNo, uid, string.Empty, uid, uid, string.Empty, productNo + "_001", JsonConvert.SerializeObject(checkData),
            //                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "admin", isSampleTest ? (isOk ? "9001" : "9002") : (isOk ? "0001" : "0002"), Program.DeviceNo + ":" + title + "_" + controlName,
            //                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), JsonConvert.SerializeObject(checkData));

            //        LocalDbHelper.LocalSqlite.Query(insert);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }
            //}

            //st.Stop();
            //Console.WriteLine("SaveDataLocal耗时：" + st.ElapsedMilliseconds);
        }

        private void GetCount(string title, string controlName, ref int totalCount, ref int okCount)
        {
            if (okCount < 0)
                throw new ArgumentOutOfRangeException(nameof(okCount));
            //var st = new Stopwatch();
            //st.Start();

            {
                try
                {
                    var sql =
                        string.Format(
                            "select taskNo from manufactureCheckData where creater = '{0}' and (checkResult = '0001' or checkResult = '0002') and createTime between datetime('now','start of day','+1 seconds') and  datetime('now','start of day','+1 days','-1 seconds') group by taskNo",
                           Program.DeviceNo + ":" + title + "_" + controlName);

                    totalCount = LocalDbHelper.QueryBySqlString(sql).Count;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    totalCount = 0;
                }
            }

            {


                try
                {
                    var sqlOk = string.Format("select taskNo from manufactureCheckData where creater = '{0}' and createTime between datetime('now','start of day','+1 seconds') and  datetime('now','start of day','+1 days','-1 seconds') and checkResult = '0001' group by taskNo", Program.DeviceNo + ":" + title + "_" + controlName);
                    var getData = LocalDbHelper.QueryBySqlString(sqlOk);
                    okCount = getData.Count;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    okCount = 0;
                }
            }

            //st.Stop();
            //Console.WriteLine("GetCount耗时：" + st.ElapsedMilliseconds);
        }

        private static void MatCopyToImageViewer(Mat mat, ImageViewer imageViewer)
        {
            var visionImg = CommonUtility.HikSdk.MyCamera.MatToVisionImage(mat);
            Algorithms.Copy(visionImg, imageViewer.Image);
            visionImg.Dispose();
        }

        #endregion

        private void pesConfigBtn_Click(object sender, EventArgs e)
        {
            //if (PesVisionAnalysis != null)
            //{
            //    if (!PesVisionAnalysis.IsInAutoRun)
            //    {
            //        PesVisionAnalysis.IsInCali = true;

            //        var frm = new PesVisionConfig(PesVisionAnalysis, IniFile);
            //        frm.ShowDialog();

            //        PesVisionAnalysis.IsInCali = false;
            //    }
            //    else
            //    {
            //        ShowErrorTip("正在自动运行中");
            //    }
            //}
        }

        private void tsbtnStateConfig_Click(object sender, EventArgs e)
        {
            if (!File.Exists(St.XmlFilePath))
                return;

            try
            {
                var dirPath = Program.SysDir;
                var xmlPath = St.XmlFilePath;
                var controllerPath = dirPath + @"\Controller.dll";
                var userControlPath = dirPath + @"\UserControls.dll";
                using (var form = new DeviceDesign.FormMain(xmlPath, controllerPath, userControlPath))
                {
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"打开失败：" + ex.Message);
            }
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (_lockImageViewer)
            {
                //var imgKey = cmbImageList.Items[cmbImageList.SelectedIndex].ToString();
                //if (!string.IsNullOrEmpty(imgKey))
                //{
                //    //MatCopyToImageViewer(PesVisionAnalysis.ImgMats[imgKey], _mainImageViewer);
                //    Algorithms.Copy(_imageViewers[imgKey].Image, _mainImageViewer.Image);
                //}

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

        }

        private Form _debugForm;

        private void tsbtnControllerOperation_Click(object sender, EventArgs e)
        {
            try
            {
                if (_debugForm == null)
                {
                    _debugForm = new FormControllerDebuger(St);
                    _debugForm.Show();
                }
                else
                {
                    if (_debugForm.IsDisposed)
                    {
                        _debugForm = new FormControllerDebuger(St);
                    }
                    _debugForm.Show();
                    _debugForm.WindowState = FormWindowState.Normal;
                    _debugForm.Focus();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void tsbtnStateWatch_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new FormStateMonitor(St))
                    form.ShowDialog();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private async void toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (var checkApp in
                     St.LstControllers.OfType<ControllerBase>().Where(c => c.Name.Equals("检测程序")).OfType<CheckApp>())
                checkApp.Bbool1 = true;

            //// 操作...
            //Console.WriteLine("打开电源");
            //// 操作...
            //Console.WriteLine("打开继电器");
            //// 操作...
            //Console.WriteLine("打开灯");
            //await Task.Delay(1000);

            //// 操作...
            //// 操作...
            //// 操作...

            //// 操作...
            //Console.WriteLine("关闭电源");
            //// 操作...
            //Console.WriteLine("关闭继电器");
            //// 操作...
            //Console.WriteLine("关闭灯");
            //await Task.Delay(1000);
        }

        private void btnServoTeach_Click(object sender, EventArgs e)
        {
            var pesVisionAnalysis = St.LstControllers.Find(f => f is PesVisionAnalysisNDLB);
            var pesPlcController = St.LstControllers.Find(f => f is PesControlPlc);

            if (pesVisionAnalysis != null && pesPlcController != null)
            {
                var frm = new PesServoCali(pesVisionAnalysis as PesVisionAnalysisNDLB, pesPlcController as PesControlPlc);
                frm.ShowDialog();
            }
        }

        private void btnLookLocalData_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmVisionCheckDataHistory())
            {
                frm.ShowDialog();
            }
        }
    }
}
