using CheckSystem.YfasLogo;
using CommonUtility;
using Controller;
using Newtonsoft.Json;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using StateMachine;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using CommonUtility.FileOperator;
using HZH_Controls.IconFont;
using FontImages = HZH_Controls.IconFont.FontImages;
using Point = OpenCvSharp.Point;

namespace CheckSystem.AlcmAutoDevice
{
    public partial class FrmAlcmMain : Form
    {
        public static string CcdConfigPath = string.Format(@"{0}\{1}\{2}", Program.SysDir, @"图像检测配置文件\音乐氛围灯模块", "音乐氛围灯模块.xml");
        private LedCheckPara LedCheckPara { get; set; }
        private AlcmDevice AlcmDevice { get; set; }
        private State St { get; set; }
        private Thread Th { get; set; }
        private BackgroundWorker RefreshBackgroundWorker { get; set; }

        private readonly string _xmlFileName = string.Format(@"{0}\{1}\{2}", Program.SysDir, @"流程配置文件", "音乐氛围灯模块自动线终检-20240115.xml");

        private readonly Dictionary<int, DateTime> _dateTimeList = new Dictionary<int, DateTime>();

        private const string LedCheckName = "负载版音乐律动";
        private const string A2BCheckName = "A2B主从板指示灯";

        private string _currentPrName = string.Empty;
        private string _currentPrNo = string.Empty;
        private string _currentDeviceNo = string.Empty;

        private CheckStep _currentCheckStep = CheckStep.WaitStart;
        private bool _isCheckA2BLedOk;
        private bool _isCheckLedAllOnOk;
        private bool _isCheckLedJumpOk;

        private int _jumpCount;
        private string _lastJumpStates;
        private readonly Stopwatch _jumpTimeOut = new Stopwatch();

        public FrmAlcmMain()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Icon = FontImages.GetIcon(
                FontIcons.A_fa_music, 32,
                Color.DodgerBlue);
            InitCamera();
            Load += FrmAlcmMain_Load;
            Closed += FrmAlcmMain_Closed;

            SetDgv(mainDgv1);
        }

        private void FrmAlcmMain_Closed(object sender, EventArgs e)
        {
            if (RefreshBackgroundWorker != null)
                RefreshBackgroundWorker.CancelAsync();
            StopGrab();
            CloseCamera();
            Environment.Exit(0);
        }

        private void FrmAlcmMain_Load(object sender, EventArgs e)
        {
            LedCheckPara = XmlHelper.Deserialize<LedCheckPara>(CcdConfigPath);

            var option = new UIEditOption { AutoLabelWidth = true, Text = @"选择产品" };
            var prMode = new[] { "D2UC", "C1YB", @"NDLB" };
            option.AddCombobox("Product", "产品：", prMode, 0, true, true);
            option.AddSwitch("IsNeedRefresh", "是否需要刷新文件", false);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                //this.ShowErrorTip("请重启程序并选择需要生产的产品");
                return;
            }

            toolStrip1.Enabled = true;

            var prIndex = (int)frm["Product"];
            var isNeedRefresh = (bool)frm["IsNeedRefresh"];

            _currentPrName = prMode[prIndex];

            if (prIndex == 0)
                _currentPrNo = "PR4101000906";
            else if (prIndex == 1)
                _currentPrNo = "PR4101000914";
            else if (prIndex == 2)
                _currentPrNo = "PR4101000903";

            _currentDeviceNo = "IN4304000176";

            uiMarkLabel2.BackColor = Color.DarkBlue;
            uiMarkLabel2.Text = _currentPrName;
            if (isNeedRefresh)
                uiMarkLabel2.Text += @"(带LIN刷新文件)";

            var baseToRefreshPath = string.Format(@"{0}\{1}\{2}", Program.SysDir, @"图像检测配置文件\音乐氛围灯模块\刷新文件", _currentPrName);
            var drv = baseToRefreshPath + @"\DRV";
            var app = baseToRefreshPath + @"\APP";
            var cali = baseToRefreshPath + @"\CAL";

            if (Directory.Exists(drv) && Directory.GetFiles(drv).Length == 1)
                drv = Directory.GetFiles(drv)[0];
            if (Directory.Exists(app) && Directory.GetFiles(app).Length == 1)
                app = Directory.GetFiles(app)[0];
            if (Directory.Exists(cali) && Directory.GetFiles(cali).Length == 1)
                cali = Directory.GetFiles(cali)[0];

            OpenCamera();
            SetContinuesMode();
            StatGrap();
            SetGainExposureTime(0, 5000);

            StartState(prIndex, isNeedRefresh);
        }

        private void StartState(int productType, bool isNeedRefresh)
        {
            St = new State();
            St.Init<ControllerBase>(_xmlFileName, "Controller.dll");

            St.PushEnter += St_PushEnter;
            St.PushDisplay += St_PushDisplay;
            St.PushEndResult += St_PushEndResult;

            var alcm = St.LstControllers.Find(f => f is AlcmDevice);
            if (alcm != null)
            {
                AlcmDevice = alcm as AlcmDevice;
                if (AlcmDevice != null)
                {
                    AlcmDevice.ProductType = productType;
                    AlcmDevice.IsNeedRefresh = isNeedRefresh;
                }
            }

            foreach (var checkApp in
                     St.LstControllers.OfType<ControllerBase>().Where(c => c.Name.Equals("检测程序")).OfType<CheckApp>())
                checkApp.IsByPass = true;

            TaskCheck();
        }

        private void St_PushEnter(string wsName, string enterStaus, string[] enterAction)
        {
            if (wsName == "主程序")
            {
                BeginInvoke(new Action(() =>
                {
                    uiMarkLabel1.Text = enterStaus;
                }));
            }
        }

        private void St_PushEndResult(string processNo, List<CheckData> checkValues)
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

                if (lblLedResult.BackColor != Color.Green && lblLedResult.BackColor != Color.Red)
                    lblLedResult.BackColor = Color.Red;
                if (lblA2bLedResult.BackColor != Color.Green && lblA2bLedResult.BackColor != Color.Red)
                    lblA2bLedResult.BackColor = Color.Red;

                tempCheckValues.Add(new CheckData
                {
                    Format = "/",
                    ParaName = LedCheckName,
                    ProcessNo = key,
                    Range = "OK",
                    Result = lblLedResult.BackColor == Color.Green ? "True" : "False",
                    Type = "图像检测",
                    Unit = "/",
                    Value = lblLedResult.BackColor == Color.Green ? "True" : "False",
                });

                tempCheckValues.Add(new CheckData
                {
                    Format = "=",
                    ParaName = A2BCheckName,
                    ProcessNo = key,
                    Range = "OK",
                    Result = lblA2bLedResult.BackColor == Color.Green ? "True" : "False",
                    Type = "图像检测",
                    Unit = "/",
                    Value = lblA2bLedResult.BackColor == Color.Green ? "True" : "False",
                });

                var isNg =
                    tempCheckValues.FindAll(
                        f => string.Equals(f.Result, false.ToString(), StringComparison.CurrentCultureIgnoreCase)).Any();
                var okCount = 0;
                var totalCount = 0;
                var taskId = Guid.NewGuid().ToString();

                #region 数据存储
                var listBarcodeStrs = new List<string>();

                try
                {
                    var modelList = new List<Model.manufactureCheckData>();
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

                            var model = new Model.manufactureCheckData
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
                        var model = new Model.manufactureCheckData
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
                    foreach (var t in tempCheckValues)
                    {
                        if (t.Type.ToLower() != "barcode" && t.Type.ToLower() != "barcodegroup")
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

                        okCount = bll.GetCountByTaskNo(string.Format(
                            "processNo = '{0}' and checkResult = '{1}' and DATEDIFF(DD,createTime,GETDATE()) = 0 and creater = '{2}'",
                            model.processNo, "0001", model.creater));

                        totalCount =
                            bll.GetCountByTaskNo(
                                string.Format(
                                    "processNo = '{0}' and DATEDIFF(DD,createTime,GETDATE()) = 0 and creater = '{1}'",
                                    model.processNo, model.creater));
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

                mainDgv1.ClearSelection();

                foreach (var checkValue in tempCheckValues)
                {
                    // 查找参数表对应的控件
                    var para =
                        St.DeviceConfig.Paras.ToList()
                            .Find(t => t.ProcessNo == checkValue.ProcessNo && t.Name == checkValue.ParaName);
                    if (para == null)
                        continue;

                    // 查找参数表对应界面控件
                    var cellIndex = -1;

                    if (para.ControlName.Contains("工位1"))
                        cellIndex = 2;
                    else if (para.ControlName.Contains("工位2"))
                        cellIndex = 3;
                    else if (para.ControlName.Contains("工位3"))
                        cellIndex = 4;
                    else if (para.ControlName.Contains("工位4"))
                        cellIndex = 5;

                    var costTime = -1.0;
                    if (_dateTimeList.ContainsKey(cellIndex))
                        costTime =
                            Math.Round(ValueHelper.GetTimeSpanMs(_dateTimeList[cellIndex], DateTime.Now) / (float)1000, 1,
                                MidpointRounding.AwayFromZero);

                    if (cellIndex != -1 && mainDgv1.RowCount >= 4)
                    {
                        if (para.ControlName.Contains("工位1"))
                        {
                            AlcmDevice.IsWs1Ok = !isNg ? (ushort)1 : (ushort)0;
                            AlcmDevice.IsWs1Complete = 1;
                        }
                        else if (para.ControlName.Contains("工位2"))
                        {
                            AlcmDevice.IsWs2Ok = !isNg ? (ushort)1 : (ushort)0;
                            AlcmDevice.IsWs2Complete = 1;
                        }
                        else if (para.ControlName.Contains("工位3"))
                        {
                            AlcmDevice.IsWs3Ok = !isNg ? (ushort)1 : (ushort)0;
                            AlcmDevice.IsWs3Complete = 1;
                        }
                        else if (para.ControlName.Contains("工位4"))
                        {
                            AlcmDevice.IsWs4Ok = !isNg ? (ushort)1 : (ushort)0;
                            AlcmDevice.IsWs4Complete = 1;
                        }

                        mainDgv1.Rows[0].Cells[cellIndex].Value = isNg ? "NG" : "OK";
                        mainDgv1.Rows[0].Cells[cellIndex].Style.BackColor = isNg ? Color.Red : Color.Green;
                        mainDgv1.Rows[1].Cells[cellIndex].Value = DateTime.Now.ToShortTimeString();
                        mainDgv1.Rows[2].Cells[cellIndex].Value = costTime + "秒";
                        mainDgv1.Rows[3].Cells[cellIndex].Value = "/";

                        //if (checkValues.FindAll(f => f.ParaName == "样件检测结果").Any())
                        //    continue;
                        mainDgv1.Rows[3].Cells[cellIndex].Value = string.Format("OK={0},NG={1}/总:{2}", okCount, totalCount - okCount, totalCount);
                        break;
                        //var tempResult = ((UserDataGrid)control).label.Text;
                        //((UserDataGrid)control).label.Text = string.Format("{0} [OK={1},NG={2}/总: {3}]", tempResult, okCount, totalCount - okCount, totalCount);
                    }
                }
            }
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
                        var cellIndex = -1;
                        if (para.ControlName.Contains("工位1"))
                            cellIndex = 2;
                        else if (para.ControlName.Contains("工位2"))
                            cellIndex = 3;
                        else if (para.ControlName.Contains("工位3"))
                            cellIndex = 4;
                        else if (para.ControlName.Contains("工位4"))
                            cellIndex = 5;

                        if (cellIndex != -1 && mainDgv1.Rows.Count >= 4)
                        {
                            if (mainDgv1.Rows[0].Cells[cellIndex].Style.BackColor != mainDgv1.Rows[0].Cells[0].Style.BackColor)
                            {
                                for (var i = 0; i < mainDgv1.Rows.Count; i++)
                                {
                                    mainDgv1.Rows[i].Cells[cellIndex].Style.BackColor =
                                        mainDgv1.Rows[i].Cells[0].Style.BackColor;
                                    mainDgv1.Rows[i].Cells[cellIndex].Value = string.Empty;
                                }

                                if (_dateTimeList.ContainsKey(cellIndex))
                                {
                                    _dateTimeList[cellIndex] = DateTime.Now;
                                }

                                txtJumpInfo.Text = string.Empty;

                                lblA2bLedResult.BackColor = Color.DarkGoldenrod;
                                lblLedResult.BackColor = Color.DarkGoldenrod;
                                lblLedResult.Text = @"负载灯板音乐律动";
                            }

                            mainDgv1.Rows[0].Cells[cellIndex].Value = @"正在检测";

                            var name = v1.ParaName;
                            var range = v1.Range;
                            var checkValue = v.Value;
                            var checkResult = v1.Result.ToLower();

                            if (!string.IsNullOrEmpty(v1.Unit))
                            {
                                checkValue += @"/" + v1.Unit;
                                range += @"/" + v1.Unit;
                            }

                            var isHave = -1;
                            for (var i = 4; i < mainDgv1.RowCount; i++)
                            {
                                if (mainDgv1.Rows[i].Cells[0].Value.ToString() != name)
                                    continue;

                                isHave = i;
                                break;
                            }

                            if (isHave >= 4)
                            {
                                mainDgv1.Rows[isHave].Cells[cellIndex].Value = checkValue;
                                mainDgv1.Rows[isHave].Cells[cellIndex].Style.BackColor =
                                    checkResult == "false" || checkResult == "0"
                                        ? Color.Red
                                        : mainDgv1.Rows[isHave].Cells[cellIndex].Style.BackColor;
                            }
                            else
                            {
                                var index = mainDgv1.Rows.Add();
                                mainDgv1.Rows[index].Cells[0].Value = name;
                                mainDgv1.Rows[index].Cells[1].Value = range;
                                mainDgv1.Rows[index].Cells[cellIndex].Value = checkValue;
                                mainDgv1.Rows[index].Cells[cellIndex].Style.BackColor =
                                    checkResult == "false" || checkResult == "0"
                                        ? Color.Red
                                        : mainDgv1.Rows[index].Cells[cellIndex].Style.BackColor;
                            }

                            mainDgv1.FirstDisplayedScrollingRowIndex = mainDgv1.RowCount - 1;
                        }
                    }
                }));
            }
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

            RefreshBackgroundWorker = new BackgroundWorker();
            RefreshBackgroundWorker.DoWork += RefreshBackgroundWorker_DoWork;
            RefreshBackgroundWorker.WorkerReportsProgress = true;
            RefreshBackgroundWorker.WorkerSupportsCancellation = true;
            RefreshBackgroundWorker.RunWorkerAsync();
        }

        private void RefreshBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            while (!worker.CancellationPending)
            {
                Thread.Sleep(50);

                if (AlcmDevice == null)
                    continue;

                if (!AlcmDevice.Execute)
                    continue;

                var ngCount = 0;

                // 测试过程
                {
                    BeginInvoke(new Action(() =>
                    {
                        txtJumpInfo.Text = string.Empty;

                        lblA2bLedResult.BackColor = Color.DarkGoldenrod;
                        lblLedResult.BackColor = Color.DarkGoldenrod;
                        lblLedResult.Text = @"负载灯板音乐律动";
                    }));

                    AlcmDevice.IsWs1Ok = 0;
                    AlcmDevice.IsWs2Ok = 0;
                    AlcmDevice.IsWs3Ok = 0;
                    AlcmDevice.IsWs4Ok = 0;

                    AlcmDevice.IsWs1Complete = 0;
                    AlcmDevice.IsWs2Complete = 0;
                    AlcmDevice.IsWs3Complete = 0;
                    AlcmDevice.IsWs4Complete = 0;

                    //// 每次启动，先保存一张图片，用于下次标定
                    //var fileName = string.Format(@"{0}\{1}\{2}.bmp", Program.SysDir, @"图像检测配置文件\音乐氛围灯模块", _currentPrName);
                    //bnSaveBmp_Click(fileName);

                    _isCheckA2BLedOk = false;
                    _isCheckLedAllOnOk = false;
                    _isCheckLedJumpOk = false;

                    _jumpCount = 0;
                    _lastJumpStates = string.Empty;
                    _jumpTimeOut.Stop();
                    _jumpTimeOut.Reset();

                    _currentCheckStep = CheckStep.LedAllOn;

                    while (true)
                    {
                        if (_currentCheckStep == CheckStep.WaitStart)
                        {
                            _jumpTimeOut.Stop();
                            _jumpTimeOut.Reset();
                            break;
                        }

                        if (_currentCheckStep == CheckStep.PlayMusic)
                        {
                            _jumpTimeOut.Reset();
                            _jumpTimeOut.Start();
                            // 播放音乐
                            AlcmDevice.PlayMusic();
                            _currentCheckStep = CheckStep.LedJump;
                        }
                        else if (_currentCheckStep == CheckStep.ChangeMusic)
                        {
                            // 播放音乐
                            AlcmDevice.StopMusic();
                            Thread.Sleep(4000);
                            _jumpTimeOut.Reset();
                            _jumpTimeOut.Start();
                            AlcmDevice.PlayMusic();
                            _currentCheckStep = CheckStep.LedJumpReCheck;
                        }
                    }

                    // 停止音乐
                    AlcmDevice.StopMusic();

                    // 显示结果
                    if (!_isCheckLedAllOnOk)
                    {
                        ngCount++;
                        BeginInvoke(new Action(() =>
                        {
                            lblLedResult.BackColor = Color.Red;
                            lblLedResult.Text = @"负载灯板音乐律动:LED未全亮";
                        }));
                    }
                    else
                    {
                        if (_isCheckA2BLedOk && _isCheckLedJumpOk)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                lblLedResult.BackColor = Color.Green;
                                lblLedResult.Text = @"负载灯板音乐律动";

                                lblA2bLedResult.BackColor = Color.Green;
                            }));
                        }
                        else
                        {
                            ngCount++;
                            if (!_isCheckA2BLedOk)
                            {
                                BeginInvoke(new Action(() =>
                                {
                                    lblA2bLedResult.BackColor = Color.Red;
                                }));
                            }
                            else if (!_isCheckLedJumpOk)
                            {
                                lblA2bLedResult.BackColor = Color.Green;

                                BeginInvoke(new Action(() =>
                                {
                                    lblLedResult.BackColor = Color.Red;
                                    lblLedResult.Text = @"负载灯板音乐律动:LED未律动";
                                }));
                            }
                        }
                    }
                }

                //if (ngCount > 0)
                //{
                //    AlcmDevice.IsWs1Ok = 0;
                //    AlcmDevice.IsWs2Ok = 0;
                //    AlcmDevice.IsWs3Ok = 0;
                //    AlcmDevice.IsWs4Ok = 0;
                //}
                //else
                //{
                //    if (tsbtnSampleCheck.Text == @"正常模式")
                //    {
                //        AlcmDevice.IsWs1Ok = 0;
                //        AlcmDevice.IsWs2Ok = 0;
                //        AlcmDevice.IsWs3Ok = 0;
                //        AlcmDevice.IsWs4Ok = 0;
                //    }
                //    else
                //    {
                //        AlcmDevice.IsWs1Ok = 1;
                //        AlcmDevice.IsWs2Ok = 1;
                //        AlcmDevice.IsWs3Ok = 1;
                //        AlcmDevice.IsWs4Ok = 1;
                //    }
                //}

                AlcmDevice.Complete = true;

                while (true)
                {
                    if (AlcmDevice.Complete)
                        continue;
                    AlcmDevice.Execute = false;
                    break;
                }
            }
        }

        private void SetDgv(UIDataGridView mainDgv)
        {
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
            mainDgv.AddColumn("工位1", "工位1");
            mainDgv.AddColumn("工位2", "工位2");
            mainDgv.AddColumn("工位3", "工位4");
            mainDgv.AddColumn("工位4", "工位4");

            mainDgv.AddRow("检测结果", "OK", "", "", "", "");
            mainDgv.AddRow("检测时间", "/", "", "", "", "");
            mainDgv.AddRow("检测耗时", "/", "", "", "", "");
            mainDgv.AddRow("检测计数", "/", "", "", "", "");

            mainDgv.AutoResizeColumns();

            _dateTimeList.Clear();
            _dateTimeList.Add(2, DateTime.Now);
            _dateTimeList.Add(3, DateTime.Now);
            _dateTimeList.Add(4, DateTime.Now);
            _dateTimeList.Add(5, DateTime.Now);
        }

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public uint[] m_nSaveImageBufSize = new uint[4] { 0, 0, 0, 0 };
        public IntPtr[] m_pSaveImageBuf = new IntPtr[4] { IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero };
        private object[] m_BufForSaveImageLock = new object[4];
        MyCamera.MV_FRAME_OUT_INFO_EX[] m_stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX[4];

        MyCamera.cbOutputExdelegate cbImage;
        MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList;
        private MyCamera[] m_pMyCamera;
        MyCamera.MV_CC_DEVICE_INFO[] m_pDeviceInfo;
        bool m_bGrabbing;
        int m_nCanOpenDeviceNum;        // ch:设备使用数量 | en:Used Device Number
        int m_nDevNum;        // ch:在线设备数量 | en:Online Device Number
        int[] m_nFrames;      // ch:帧数 | en:Frame Number
        bool m_bTimerFlag;     // ch:定时器开始计时标志位 | en:Timer Start Timing Flag Bit
        IntPtr[] m_hDisplayHandle;

        private void InitCamera()
        {
            m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            m_bGrabbing = false;
            m_nCanOpenDeviceNum = 0;
            m_nDevNum = 0;
            DeviceListAcq();
            m_pMyCamera = new MyCamera[4];
            m_pDeviceInfo = new MyCamera.MV_CC_DEVICE_INFO[4];
            m_nFrames = new int[4];
            cbImage = ImageCallBack;
            for (var i = 0; i < 4; ++i)
                m_BufForSaveImageLock[i] = new object();

            m_bTimerFlag = false;
            m_hDisplayHandle = new IntPtr[4];
        }

        /// <summary>
        /// ch:初始化、打开相机 | en:Initialization and open devices
        /// </summary>
        private void OpenCamera()
        {
            var bOpened = false;

            // ch:获取使用设备的数量 | en:Get Used Device Number
            var nCameraUsingNum = 1;
            // ch:参数检测 | en:Parameters inspection
            if (nCameraUsingNum <= 0)
            {
                nCameraUsingNum = 1;
            }
            if (nCameraUsingNum > 4)
            {
                nCameraUsingNum = 4;
            }

            for (int i = 0, j = 0; j < m_nDevNum; j++)
            {
                //ch:获取选择的设备信息 | en:Get Selected Device Information
                var device =
                    (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[j], typeof(MyCamera.MV_CC_DEVICE_INFO));

                var StrTemp = "";
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    var gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                    if (gigeInfo.chUserDefinedName != "")
                    {
                        StrTemp = "GEV: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")";
                    }
                    else
                    {
                        StrTemp = "GEV: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")";
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    var usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (usbInfo.chUserDefinedName != "")
                    {
                        StrTemp = "U3V: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")";
                    }
                    else
                    {
                        StrTemp = "U3V: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")";
                    }
                }

                //ch:打开设备 | en:Open Device
                if (null == m_pMyCamera[i])
                {
                    m_pMyCamera[i] = new MyCamera();
                    if (null == m_pMyCamera[i])
                    {
                        return;
                    }
                }

                var nRet = m_pMyCamera[i].MV_CC_CreateDevice_NET(ref device);
                if (MyCamera.MV_OK != nRet)
                {
                    return;
                }

                nRet = m_pMyCamera[i].MV_CC_OpenDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    //richTextBox.Text += string.Format("Open Device[{0}] failed! nRet=0x{1}\r\n", StrTemp, nRet.ToString("X"));
                    continue;
                }
                else
                {
                    //richTextBox.Text += string.Format("Open Device[{0}] success!\r\n", StrTemp);

                    m_nCanOpenDeviceNum++;
                    m_pDeviceInfo[i] = device;
                    // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                    if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                    {
                        var nPacketSize = m_pMyCamera[i].MV_CC_GetOptimalPacketSize_NET();
                        if (nPacketSize > 0)
                        {
                            nRet = m_pMyCamera[i].MV_CC_SetIntValueEx_NET("GevSCPSPacketSize", nPacketSize);
                            if (nRet != MyCamera.MV_OK)
                            {
                                //richTextBox.Text += string.Format("Set Packet Size failed! nRet=0x{0}\r\n", nRet.ToString("X"));
                            }
                        }
                        else
                        {
                            //richTextBox.Text += string.Format("Get Packet Size failed! nRet=0x{0}\r\n", nPacketSize.ToString("X"));
                        }
                    }

                    m_pMyCamera[i].MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                    m_pMyCamera[i].MV_CC_RegisterImageCallBackEx_NET(cbImage, (IntPtr)i);
                    bOpened = true;
                    if (m_nCanOpenDeviceNum == nCameraUsingNum)
                    {
                        break;
                    }
                    i++;
                }
            }

            // ch:只要有一台设备成功打开 | en:As long as there is a device successfully opened
            if (bOpened)
            {
                //tbUseNum.Text = m_nCanOpenDeviceNum.ToString();
                //SetCtrlWhenOpen();
            }
        }

        /// <summary>
        /// ch:关闭相机 | en:Close Device
        /// </summary>
        private void CloseCamera()
        {
            for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                int nRet;

                nRet = m_pMyCamera[i].MV_CC_CloseDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    return;
                }

                //nRet = m_pMyCamera[i].MV_CC_DestroyDevice_NET();
                //if (MyCamera.MV_OK != nRet)
                //{
                //    return;
                //}
            }

            //控件操作 ch: | en:Control Operation
            //SetCtrlWhenClose();
            // ch:取流标志位清零 | en:Zero setting grabbing flag bit
            m_bGrabbing = false;
            // ch:重置成员变量 | en:Reset member variable
            ResetMember();
        }

        /// <summary>
        /// ch:枚举设备 | en:Create Device List
        /// </summary>
        private void DeviceListAcq()
        {
            GC.Collect();
            var nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                //richTextBox.Text += "Enumerate devices fail!\r\n";
                return;
            }

            m_nDevNum = (int)m_pDeviceList.nDeviceNum;
            //tbDevNum.Text = m_nDevNum.ToString("d");
        }

        /// <summary>
        /// ch:打开触发模式 | en:Open Trigger Mode
        /// </summary>
        private void SetTriggerMode()
        {
            for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                m_pMyCamera[i].MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);

                // ch:触发源选择:0 - Line0; | en:Trigger source select:0 - Line0;
                //           1 - Line1;
                //           2 - Line2;
                //           3 - Line3;
                //           4 - Counter;
                //           7 - Software;
                m_pMyCamera[i].MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
            }

            // ch:触发源设为软触发 | en:Set Trigger Source As Software
            for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                m_pMyCamera[i].MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
            }
        }

        /// <summary>
        /// // ch:连续采集 | en:
        /// </summary>
        private void SetContinuesMode()
        {
            for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                m_pMyCamera[i].MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                //cbSoftTrigger.Enabled = false;
                //bnTriggerExec.Enabled = false;
                //bnSaveBmp.Enabled = true;
            }
        }

        private void SetGainExposureTime(int gain, int exposuireTime)
        {
            int nRet;
            for (int i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                bool bSuccess = true;
                m_pMyCamera[i].MV_CC_SetEnumValue_NET("ExposureAuto", 0);

                nRet = m_pMyCamera[i].MV_CC_SetFloatValue_NET("ExposureTime", float.Parse(exposuireTime.ToString()));
                if (nRet != MyCamera.MV_OK)
                {
                    //Updatetxt(string.Format("Set Exposure Time Failed! nRet=0x{0}", nRet.ToString("X")));
                    bSuccess = false;
                }

                m_pMyCamera[i].MV_CC_SetEnumValue_NET("GainAuto", 0);
                nRet = m_pMyCamera[i].MV_CC_SetFloatValue_NET("Gain", float.Parse(gain.ToString()));
                if (nRet != MyCamera.MV_OK)
                {
                    //Updatetxt(string.Format("Set Gain Failed! nRet=0x{0}", nRet.ToString("X")));
                    bSuccess = false;
                }

                if (bSuccess)
                {
                    //Updatetxt(string.Format("Set Parameters Succeed! Gaint={0}, ExposureTime={1}", gain, exposuireTime));
                }
            }
        }

        /// <summary>
        /// ch:开始采集 | en:Stop Grabbing
        /// </summary>
        private void StatGrap()
        {
            {
                int nRet;
                //m_hDisplayHandle[0] = pictureBox1.Handle;
                //m_hDisplayHandle[1] = pictureBox2.Handle;
                //m_hDisplayHandle[2] = pictureBox3.Handle;
                //m_hDisplayHandle[3] = pictureBox4.Handle;

                // ch:开始采集 | en:Start Grabbing
                for (var i = 0; i < m_nCanOpenDeviceNum; i++)
                {
                    m_nFrames[i] = 0;
                    m_stFrameInfo[i].nFrameLen = 0;//取流之前先清除帧长度
                    m_stFrameInfo[i].enPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Undefined;
                    nRet = m_pMyCamera[i].MV_CC_StartGrabbing_NET();
                    if (MyCamera.MV_OK != nRet)
                    {
                        //richTextBox.Text += string.Format("No.{0} Start Grabbing failed! nRet=0x{1}\r\n", (i + 1).ToString(), nRet.ToString("X"));
                    }
                }

                //ch:开始计时  | en:Start Timing
                m_bTimerFlag = true;
                // ch:控件操作 | en:Control Operation
                //SetCtrlWhenStartGrab();
                // ch:标志位置位true | en:Set Position Bit true
                m_bGrabbing = true;
            }
        }

        /// <summary>
        /// ch: 停止采集 | en:Stop Grabbing
        /// </summary>
        private void StopGrab()
        {
            {
                for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
                {
                    m_pMyCamera[i].MV_CC_StopGrabbing_NET();
                }
                //ch:标志位设为false  | en:Set Flag Bit false
                m_bGrabbing = false;
                // ch:停止计时 | en:Stop Timing
                m_bTimerFlag = false;

                // ch:控件操作 | en:Control Operation
                //SetCtrlWhenStopGrab();
            }
        }

        private void ResetMember()
        {
            m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            m_bGrabbing = false;
            m_nCanOpenDeviceNum = 0;
            m_nDevNum = 0;
            DeviceListAcq();
            m_nFrames = new int[4];
            cbImage = new MyCamera.cbOutputExdelegate(ImageCallBack);
            m_bTimerFlag = false;
            m_hDisplayHandle = new IntPtr[4];
            m_pDeviceInfo = new MyCamera.MV_CC_DEVICE_INFO[4];
        }

        private void bnSaveBmp_Click(string filepath)
        {
            var stSaveParam = new MyCamera.MV_SAVE_IMG_TO_FILE_PARAM();
            lock (m_BufForSaveImageLock[0])
            {
                if (m_stFrameInfo[0].nFrameLen == 0)
                {
                    //richTextBox.Text += "save image failed! No data!\r\n";
                    //this.ShowErrorTip("保存失败");
                    return;
                }

                try
                {
                    stSaveParam.enImageType = MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Bmp;
                    stSaveParam.enPixelType = m_stFrameInfo[0].enPixelType;
                    stSaveParam.pData = m_pSaveImageBuf[0];
                    stSaveParam.nDataLen = m_stFrameInfo[0].nFrameLen;
                    stSaveParam.nHeight = m_stFrameInfo[0].nHeight;
                    stSaveParam.nWidth = m_stFrameInfo[0].nWidth;

                    stSaveParam.pImagePath = filepath;

                    if (File.Exists(filepath))
                        File.Delete(filepath);

                    var nRet = m_pMyCamera[0].MV_CC_SaveImageToFile_NET(ref stSaveParam);

                    //this.ShowSuccessTip(nRet == 0 ? "保存成功" : "保存失败");
                }
                catch (Exception ex)
                {
                    //this.ShowErrorTip("保存失败：" + ex.Message);
                }
            }
        }

        /// <summary>
        /// ch:取流回调函数 | en:Aquisition Callback Function
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="pFrameInfo"></param>
        /// <param name="pUser"></param>
        private void ImageCallBack(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            var nIndex = (int)pUser;

            // ch:抓取的帧数 | en:Aquired Frame Number
            ++m_nFrames[nIndex];

            lock (m_BufForSaveImageLock[nIndex])
            {
                if (m_pSaveImageBuf[nIndex] == IntPtr.Zero || pFrameInfo.nFrameLen > m_nSaveImageBufSize[nIndex])
                {
                    if (m_pSaveImageBuf[nIndex] != IntPtr.Zero)
                    {
                        Marshal.Release(m_pSaveImageBuf[nIndex]);
                        m_pSaveImageBuf[nIndex] = IntPtr.Zero;
                    }

                    m_pSaveImageBuf[nIndex] = Marshal.AllocHGlobal((int)pFrameInfo.nFrameLen);
                    if (m_pSaveImageBuf[nIndex] == IntPtr.Zero)
                        return;
                    m_nSaveImageBufSize[nIndex] = pFrameInfo.nFrameLen;
                }

                m_stFrameInfo[nIndex] = pFrameInfo;
                CopyMemory(m_pSaveImageBuf[nIndex], pData, pFrameInfo.nFrameLen);
            }

            //var stDisplayInfo = new MyCamera.MV_DISPLAY_FRAME_INFO();
            //stDisplayInfo.hWnd = m_hDisplayHandle[nIndex];
            //stDisplayInfo.pData = pData;
            //stDisplayInfo.nDataLen = pFrameInfo.nFrameLen;
            //stDisplayInfo.nWidth = pFrameInfo.nWidth;
            //stDisplayInfo.nHeight = pFrameInfo.nHeight;
            //stDisplayInfo.enPixelType = pFrameInfo.enPixelType;

            //m_pMyCamera[nIndex].MV_CC_DisplayOneFrame_NET(ref stDisplayInfo);

            {
                try
                {
                    var listPutTxtAction = new List<Action<Mat>>();

                    const double minGrayOffet = 0.7;
                    var mat = new Mat(pFrameInfo.nHeight, pFrameInfo.nWidth, MatType.CV_8UC1, pData);

                    var listA2BLedRectsWithGray = new List<RectWithGray>();
                    var findA2BLedFunc = LedCheckPara.VisionFuncs.VisionFunc.ToList().Find(f => f.FuncName == string.Format("{0}/{1}", A2BCheckName, _currentPrName));
                    if (findA2BLedFunc != null && findA2BLedFunc.VisionPara != null &&
                        findA2BLedFunc.VisionPara.ShapesGroups.Any())
                    {
                        if (findA2BLedFunc.VisionPara.ShapesGroups[0].Shapes != null)
                        {
                            listA2BLedRectsWithGray.AddRange(
                                from item in findA2BLedFunc.VisionPara.ShapesGroups[0].Shapes
                                let tRect = item.Contour.Rect.Split(',')
                                where item.Contour.Type == "RectangleContour"
                                let newRect =
                                    new Rect(int.Parse(tRect[0]), int.Parse(tRect[1]), int.Parse(tRect[2]),
                                        int.Parse(tRect[3]))
                                select new RectWithGray
                                {
                                    Rect = newRect,
                                    GrayStandard = (int)item.Value,
                                    GrayMin = (int)item.Min,
                                    GrayMax = (int)item.Max,
                                    GrayCurrent = (int)Cv2.Mean(mat[newRect])[0]
                                });
                        }
                    }

                    //Cv2.PutText(mat, string.Format("{0}", _currentCheckStep), new Point(mat.Width * 0.45, mat.Height * 0.92), HersheyFonts.HersheyComplex, 0.4, Scalar.WhiteSmoke);
                    listPutTxtAction.Add(m =>
                    {
                        Cv2.PutText(m, string.Format("{0}", _currentCheckStep), new Point(m.Width * 0.45, m.Height * 0.92),
                            HersheyFonts.HersheyComplex, 0.4, Scalar.WhiteSmoke);
                    });

                    var listLedRectsWithGray = new List<RectWithGray>();
                    var findLedFunc = LedCheckPara.VisionFuncs.VisionFunc.ToList().Find(f => f.FuncName == string.Format("{0}/{1}", LedCheckName, _currentPrName));
                    if (findLedFunc != null && findLedFunc.VisionPara != null &&
                        findLedFunc.VisionPara.ShapesGroups.Any())
                    {
                        if (findLedFunc.VisionPara.ShapesGroups[0].Shapes != null)
                        {
                            listLedRectsWithGray.AddRange(
                                from item in findLedFunc.VisionPara.ShapesGroups[0].Shapes
                                let tRect = item.Contour.Rect.Split(',')
                                where item.Contour.Type == "RectangleContour"
                                let newRect =
                                    new Rect(int.Parse(tRect[0]), int.Parse(tRect[1]), int.Parse(tRect[2]),
                                        int.Parse(tRect[3]))
                                select new RectWithGray
                                {
                                    Rect = newRect,
                                    GrayMin = (int)item.Min,
                                    GrayMax = (int)item.Max,
                                    GrayCurrent = (int)Cv2.Mean(mat[newRect])[0]
                                });
                        }
                    }

                    if (_currentCheckStep == CheckStep.A2BLed)
                    {
                        if (!listA2BLedRectsWithGray.Any())
                        {
                            _isCheckA2BLedOk = false;
                            _currentCheckStep = CheckStep.WaitStart;
                        }
                        else
                        {
                            var ngCount = 0;
                            for (var i = 0; i < listA2BLedRectsWithGray.Count; i++)
                            {
                                if (listA2BLedRectsWithGray[i].GrayCurrent < listA2BLedRectsWithGray[i].GrayMin)
                                {
                                    ngCount++;
                                }
                            }

                            if (ngCount == 0)
                            {
                                // 每次启动，先保存一张图片，用于下次标定
                                {
                                    var fileName = string.Format(@"{0}\{1}\{2}_A2B指示灯.bmp", Program.SysDir, @"图像检测配置文件\音乐氛围灯模块", _currentPrName);
                                    bnSaveBmp_Click(fileName);
                                }

                                _isCheckA2BLedOk = true;
                                _currentCheckStep = CheckStep.PlayMusic;
                            }
                            else
                            {
                                //Cv2.PutText(mat,
                                //    string.Format("watch A2B timeout(max={1}ms): {0}ms",
                                //        _jumpTimeOut.ElapsedMilliseconds, 30 * 1000),
                                //    new Point(mat.Width * 0.45, mat.Height * 0.94), HersheyFonts.HersheyComplex, 0.4,
                                //    Scalar.WhiteSmoke);

                                listPutTxtAction.Add(m =>
                                {
                                    Cv2.PutText(m,
                                       string.Format("watch A2B timeout(max={1}ms): {0}ms",
                                           _jumpTimeOut.ElapsedMilliseconds, 30 * 1000),
                                       new Point(m.Width * 0.45, m.Height * 0.94), HersheyFonts.HersheyComplex, 0.4,
                                       Scalar.WhiteSmoke);
                                });

                                if (_jumpTimeOut.ElapsedMilliseconds > 30 * 1000)
                                {
                                    // 每次启动，先保存一张图片，用于下次标定
                                    {
                                        var fileName = string.Format(@"{0}\{1}\{2}_A2B指示灯.bmp", Program.SysDir, @"图像检测配置文件\音乐氛围灯模块", _currentPrName);
                                        bnSaveBmp_Click(fileName);
                                    }

                                    _isCheckA2BLedOk = false;
                                    _currentCheckStep = CheckStep.WaitStart;
                                }
                            }
                        }
                    }
                    else if (_currentCheckStep == CheckStep.LedAllOn)
                    {
                        if (!listLedRectsWithGray.Any())
                        {
                            _isCheckLedAllOnOk = false;
                            _currentCheckStep = CheckStep.WaitStart;
                        }
                        else
                        {
                            // 每次启动，先保存一张图片，用于下次标定
                            {
                                var fileName = string.Format(@"{0}\{1}\{2}_LED全亮.bmp", Program.SysDir, @"图像检测配置文件\音乐氛围灯模块", _currentPrName);
                                bnSaveBmp_Click(fileName);
                            }

                            var ngCount = 0;
                            for (var i = 0; i < listLedRectsWithGray.Count; i++)
                            {
                                if (listLedRectsWithGray[i].GrayCurrent < listLedRectsWithGray[i].GrayMin)
                                    ngCount++;
                            }

                            if (ngCount == 0)
                            {
                                for (var i = 0; i < listLedRectsWithGray.Count; i++)
                                    _lastJumpStates += "1";

                                _jumpTimeOut.Reset();
                                _jumpTimeOut.Start();
                                _isCheckLedAllOnOk = true;
                                _currentCheckStep = CheckStep.A2BLed;
                            }
                            else
                            {
                                //_isCheckLedAllOnOk = false;
                                //_currentCheckStep = CheckStep.WaitStart;

                                for (var i = 0; i < listLedRectsWithGray.Count; i++)
                                {
                                    if (listLedRectsWithGray[i].GrayCurrent < listLedRectsWithGray[i].GrayMin)
                                        _lastJumpStates += "0";
                                    else
                                        _lastJumpStates += "1";
                                }

                                _jumpTimeOut.Reset();
                                _jumpTimeOut.Start();
                                _isCheckLedAllOnOk = true;
                                _currentCheckStep = CheckStep.A2BLed;
                            }
                        }
                    }
                    else if (_currentCheckStep == CheckStep.LedJump || _currentCheckStep == CheckStep.LedJumpReCheck)
                    {
                        if (!listLedRectsWithGray.Any())
                        {
                            _isCheckLedJumpOk = false;
                            _currentCheckStep = CheckStep.WaitStart;
                        }
                        else
                        {
                            var tempCharArray = _lastJumpStates.ToCharArray();
                            var tempState = string.Empty;

                            for (var i = 0; i < listLedRectsWithGray.Count; i++)
                            {
                                //if (listLedRectsWithGray[i].GrayCurrent < listLedRectsWithGray[i].GrayMin * minGrayOffet)
                                //{
                                //    tempCharArray[i] = '0';
                                //}
                                //else if (listLedRectsWithGray[i].GrayCurrent >= listLedRectsWithGray[i].GrayMin)
                                //{
                                //    tempCharArray[i] = '1';
                                //}

                                if ((double)listLedRectsWithGray[i].GrayCurrent / listLedRectsWithGray[i].GrayStandard < minGrayOffet)
                                {
                                    tempCharArray[i] = '0';
                                }
                                else if ((double)listLedRectsWithGray[i].GrayCurrent / listLedRectsWithGray[i].GrayStandard >= minGrayOffet)
                                {
                                    tempCharArray[i] = '1';
                                }
                            }

                            foreach (var item in tempCharArray)
                            {
                                tempState += item.ToString();
                            }

                            if (tempState.Length == _lastJumpStates.Length)
                            {
                                if (tempState != _lastJumpStates)
                                {
                                    _lastJumpStates = tempState;
                                    UpdateTxtBox(txtJumpInfo, string.Format("jump info: {0}{1}", _lastJumpStates, Environment.NewLine));
                                    _jumpCount++;
                                }
                            }

                            var maxTimeOut = 30 * 1000;

                            //Cv2.PutText(mat, string.Format("watch jump timeout(max={1}ms): {0}ms", _jumpTimeOut.ElapsedMilliseconds, maxTimeOut), new Point(mat.Width * 0.45, mat.Height * 0.94), HersheyFonts.HersheyComplex, 0.4, Scalar.WhiteSmoke);
                            //Cv2.PutText(mat, string.Format("watch jump count: {0}", _jumpCount), new Point(mat.Width * 0.6, mat.Height * 0.97), HersheyFonts.HersheyComplex, 0.4, Scalar.WhiteSmoke);

                            listPutTxtAction.Add(m =>
                            {
                                Cv2.PutText(m,
                                    string.Format("watch jump timeout(max={1}ms): {0}ms",
                                        _jumpTimeOut.ElapsedMilliseconds, maxTimeOut),
                                    new Point(m.Width * 0.45, m.Height * 0.94), HersheyFonts.HersheyComplex, 0.4,
                                    Scalar.WhiteSmoke);
                            });
                            listPutTxtAction.Add(m =>
                            {
                                Cv2.PutText(m, string.Format("watch jump count: {0}", _jumpCount),
                                    new Point(m.Width * 0.6, m.Height * 0.97), HersheyFonts.HersheyComplex, 0.4,
                                    Scalar.WhiteSmoke);
                            });

                            if (_jumpCount > 30)
                            {
                                _isCheckLedJumpOk = true;
                                _currentCheckStep = CheckStep.WaitStart;
                            }
                            else
                            {
                                if (_jumpTimeOut.ElapsedMilliseconds > maxTimeOut)
                                {
                                    if (_currentCheckStep == CheckStep.LedJump)
                                    {
                                        _currentCheckStep = CheckStep.ChangeMusic;
                                    }
                                    else
                                    {
                                        _isCheckLedJumpOk = false;
                                        _currentCheckStep = CheckStep.WaitStart;
                                    }
                                }
                            }
                        }
                    }

                    var grayImg = new Mat();
                    Cv2.CvtColor(mat, grayImg, ColorConversionCodes.BayerRG2RGB);

                    var matImage = new Mat(pFrameInfo.nHeight, pFrameInfo.nWidth, MatType.CV_8UC3, grayImg.Data);
                    DrawRectWithGray(listA2BLedRectsWithGray, matImage, 1.0, "A2B");
                    DrawRectWithGray(listLedRectsWithGray, matImage, minGrayOffet);

                    foreach (var t in listPutTxtAction)
                        t.Invoke(matImage);

                    UpdatePicturebox(pictureBox1, matImage.ToBitmap());
                    matImage.Dispose();
                    mat.Dispose();
                    grayImg.Dispose();
                    GC.Collect();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private static void DrawRectWithGray(IReadOnlyList<RectWithGray> listRectWithGrays, Mat mat, double minOffset, string title = "")
        {
            for (var i = 0; i < listRectWithGrays.Count; i++)
            {
                var rect = listRectWithGrays[i].Rect;
                Cv2.PutText(mat,
                    string.IsNullOrEmpty(title)
                        ? listRectWithGrays[i].GrayCurrent.ToString(CultureInfo.InvariantCulture)
                        : string.Format("{0}:{1}", title,
                            listRectWithGrays[i].GrayCurrent.ToString(CultureInfo.InvariantCulture)),
                    new Point(rect.X, rect.Y), HersheyFonts.HersheySimplex, 0.5,
                    listRectWithGrays[i].GrayCurrent >= listRectWithGrays[i].GrayMin * minOffset
                        ? Scalar.WhiteSmoke
                        : Scalar.Red);
                //Cv2.Rectangle(mat, rect, listRectWithGrays[i].GrayCurrent >= listRectWithGrays[i].GrayMin * minOffset ? Scalar.Green : Scalar.Red);

                Cv2.Rectangle(mat, rect, (double)listRectWithGrays[i].GrayCurrent / listRectWithGrays[i].GrayStandard > minOffset ? Scalar.Green : Scalar.Red);
            }
        }

        private delegate void UpdateTxtBoxDelegate(ISynchronizeInvoke control, string value);
        private void UpdateTxtBox(ISynchronizeInvoke control, string value)
        {
            var updateDelegate = new UpdateTxtBoxDelegate(UpdateTxtBox);

            if (control.InvokeRequired)
            {
                Invoke(updateDelegate, control, value);
            }
            else
            {
                //var txtBox = control as UITextBox;
                //if (txtBox == null)
                //    return;
                //if (txtBox.Text.Length > 10 * 1000)
                //{
                //    txtBox.Text = string.Empty;
                //}

                //txtBox.Text += value;
                //Application.DoEvents();
            }
        }

        private struct RectWithGray
        {
            public Rect Rect;
            public int GrayStandard;
            public int GrayMin;
            public int GrayMax;
            public int GrayCurrent;
        }

        private delegate void UpdatePictureboxDelegate(ISynchronizeInvoke control, Bitmap value);

        /// <summary>
        /// 多线程设置PictureBox的图像
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        private void UpdatePicturebox(ISynchronizeInvoke control, Bitmap value)
        {
            //try
            //{
            //    control.Invoke(new Action<PictureBox, Bitmap>((ct, v) => { ct.Image = v; }), new object[] { control, value });
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}

            var updatePictureboxDelegate = new UpdatePictureboxDelegate(UpdatePicturebox);

            if (control.InvokeRequired)
            {
                Invoke(updatePictureboxDelegate, control, value);
            }
            else
            {
                var pictureBox = control as PictureBox;
                if (pictureBox == null)
                    return;
                pictureBox.Image = value;
                Application.DoEvents();
            }
        }

        private void tsbtnLedParaSet_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmAlcmLedParaSet(
                CcdConfigPath, string.Format("{0}/{1}", LedCheckName, _currentPrName)))
            {
                frm.ShowDialog();
            }
        }

        private void tsbtnA2BLedParaSet_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmAlcmLedParaSet(
                CcdConfigPath, string.Format("{0}/{1}", A2BCheckName, _currentPrName)))
            {
                frm.ShowDialog();
            }
        }

        private void tsbtnStateConfig_Click(object sender, EventArgs e)
        {
            if (!File.Exists(_xmlFileName))
                return;

            try
            {
                var dirPath = Program.SysDir;
                var xmlPath = _xmlFileName;
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

        private enum CheckStep
        {
            WaitStart,

            A2BLed,

            LedAllOn,

            PlayMusic,

            LedJump,

            ChangeMusic,

            LedJumpReCheck
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
    }
}
