using CommonUtility;
using CommonUtility.FileOperator;
using Controller;
using CsvHelper.Configuration.Attributes;
using Sunny.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace CheckSystem.HelperForms.GeeleyDx1h
{
    public partial class FrmLoadBox : UIForm
    {
        private ToomossUsb2XxxCanLin _toomossUsb2XxxCan = new ToomossUsb2XxxCanLin("toomoss");
        private const int DutCount = 1;
        private BindingList<Dx1hData> _dataSource = new BindingList<Dx1hData>();
        private DX1H_V3_EMC[] _dx1hModules = new DX1H_V3_EMC[DutCount];
        private CsvFileHelper<Dx1hData>[] _logWriter = new CsvFileHelper<Dx1hData>[DutCount];

        private bool[] _isUpdateNgRed = new bool[DutCount];
        private bool[] _isUpdateMotorNgRed = new bool[DutCount];
        private int[] _dutNgCount = new int[DutCount];
        private int[] _dutMotorNgCount = new int[DutCount];

        private bool _isRunning;
        private Thread _backgroundTh;
        private int _step;
        private MotorMode[] _moduleRunMode = new MotorMode[DutCount];
        private long[] _moduleRunModeStartTs = new long[DutCount];

        private string _configFile = string.Format(@"{0}\{1}\{2}", Program.SysDir, "ControllerConfig", "DX1H_LoadBox.xml");
        private Dx1hEmcConfig.DeviceConfig _dx1HEmcConfig;

        public FrmLoadBox()
        {
            InitializeComponent();
            if (File.Exists(_configFile))
            {
                _dx1HEmcConfig = XmlHelper.Deserialize<Dx1hEmcConfig.DeviceConfig>(_configFile);
                if (_dx1HEmcConfig != null)
                {
                    Load += FrmLoadBox_Load; ;
                    FormClosed += FrmLoadBox_FormClosed;

                    //if (_toomossUsb2XxxCan.Can1 != null)
                    //{
                    //    Load += FrmLoadBox_Load; ;
                    //    FormClosed += FrmLoadBox_FormClosed;
                    //}
                    //else
                    //{
                    //    UIMessageBox.ShowError("can通讯板卡连接失败");
                    //}
                }
                else
                {
                    UIMessageBox.ShowError("配置文件解析失败");
                }
            }
            else
            {
                UIMessageBox.ShowError("配置文件不存在");
            }
        }

        private void FrmLoadBox_FormClosed(object sender, FormClosedEventArgs e)
        {
            _adWorker.CancelAsync();
            Stop();

            if (_backgroundTh != null)
            {
                _backgroundTh.Abort();
                _backgroundTh.Join();
            }
        }

        private void FrmLoadBox_Load(object sender, EventArgs e)
        {
            InitController();
            InitMainDgv();
            InitTimer();
            Resize += (rs, re) => ResizeUI();
            WindowState = FormWindowState.Maximized;
        }

        private SyRenesasMcuControllerMaster _cIP29 = new SyRenesasMcuControllerMaster("IP29");
        private SyRenesasMcuControllerMaster _cIP28 = new SyRenesasMcuControllerMaster("IP28");
        private BackgroundWorker _adWorker = new BackgroundWorker();

        private void InitController()
        {
            var isLogFolderOk = false;

            var driveLetter = string.Format(@"{0}\{1}", Path.GetPathRoot(Program.SysDir), "DX1H_实验箱");
            try
            {
                if (!Directory.Exists(driveLetter))
                    Directory.CreateDirectory(driveLetter);

                for (int i = 0; i < DutCount; i++)
                {
                    var file = string.Format(@"{0}\{1}", driveLetter, "产品" + (i + 1).ToString());
                    if (!Directory.Exists(file))
                        Directory.CreateDirectory(file);
                }
            }
            catch (Exception)
            {
                isLogFolderOk = false;
            }

            for (var i = 0; i < DutCount; i++)
                _dx1hModules[i] = new DX1H_V3_EMC("dx1h" + i);

            _cIP28.InitRemoteIpAddress("192.168.1.28:8088");
            _cIP29.InitRemoteIpAddress("192.168.1.29:8088");

            if (_dx1hModules.Length > 0)
            {
                //_dx1hModules[0].Can = _cIP29.GatewayCan2;
                _dx1hModules[0].Can = _toomossUsb2XxxCan.Can1;
            }

            if (_dx1hModules.Length > 1)
                _dx1hModules[1].Can = _cIP28.GatewayCan2;

            if (_dx1hModules.Length > 2)
                _dx1hModules[2].Can = _cIP28.GatewayCan3;

            for (var i = 0; i < DutCount; i++)
            {
                _logWriter[i] = new CsvFileHelper<Dx1hData>(string.Format(@"{0}\{1}", driveLetter, "产品" + (i + 1).ToString()));
                _logWriter[i].Start();
            }

            _adWorker.WorkerSupportsCancellation = true;
            _adWorker.DoWork += _adWorker_DoWork;
            _adWorker.RunWorkerAsync();
        }

        private double[,] _duty = new double[DutCount, 2];

        private void _adWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            var lastAd = HighPrecisionTimer.GetTimestamp();

            // Keep acquiring until we error or cancel
            while (!worker.CancellationPending)
            {
                try
                {
                    var ns = HighPrecisionTimer.GetTimestamp();

                    if (HighPrecisionTimer.GetTimestampIntervalMs(lastAd, ns) > 1000)
                    {
                        lastAd = HighPrecisionTimer.GetTimestamp();
                        _cIP29.MasterReadDIsAndDuty();

                        //lock (_lockAdDuty)
                        {
                            try
                            {
                                if (DutCount >= 1)
                                {
                                    _duty[0, 0] = _cIP29.Duty1;
                                    _duty[0, 1] = _cIP29.Duty2;
                                }

                                if (DutCount >= 2)
                                {
                                    _duty[1, 0] = _cIP29.Duty3;
                                    _duty[1, 1] = _cIP29.Duty4;
                                }

                                if (DutCount >= 3)
                                {
                                    _duty[2, 0] = _cIP29.Duty5;
                                    _duty[2, 1] = _cIP29.Duty6;
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void InitMainDgv()
        {
            {
                // 配置DataGridView（虚拟模式+无滚动条+全窗口适配）
                dataGridView.ReadOnly = true;
                dataGridView.RowHeadersVisible = false;
                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToResizeRows = false;
                dataGridView.MultiSelect = true;
                dataGridView.RowHeadersVisible = false;
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dataGridView.ClearRows();
                dataGridView.ClearColumns();

                dataGridView.VirtualMode = true;

                // 绑定虚拟模式事件                
                dataGridView.CellValueNeeded += DataGridView_CellValueNeeded;
                dataGridView.CellFormatting += DataGridView_CellFormatting;
                dataGridView.RowsAdded += (rs, re) => ResizeUI();

                // 初始化数据源（6产品）
                for (int i = 0; i < DutCount; i++)
                    _dataSource.Add(new Dx1hData());
                // 添加15列（数据类型）
                dataGridView.AddColumn("DUT", "DUT");
                var tp = new Dx1hData();
                for (int i = 0; i < tp.GetType().GetProperties().Length; i++)
                {
                    var p = tp.GetType().GetProperties()[i];
                    if (p.PropertyType == typeof(double))
                    {
                        var pName =
                              ((NameAttribute)Attribute.GetCustomAttribute(p, typeof(NameAttribute)))
                                  .Names[0];
                        dataGridView.AddColumn(pName, p.Name);
                    }
                    else if (p.PropertyType == typeof(string))
                    {
                        var pName =
                             ((NameAttribute)Attribute.GetCustomAttribute(p, typeof(NameAttribute)))
                                 .Names[0];
                        if (pName == "CAN通信")
                            dataGridView.AddColumn(pName, p.Name);
                    }
                }

                dataGridView.RowCount = DutCount;
            }
        }

        /// <summary>
        /// 虚拟模式：动态提供单元格数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _dataSource.Count)
            {
                try
                {
                    if (e.ColumnIndex == 0 && (e.Value == null || e.Value.ToString() != string.Format("DUT{0}", e.RowIndex + 1)))
                        e.Value = string.Format("DUT{0}", e.RowIndex + 1);
                    else if (e.ColumnIndex > 0)
                    {
                        //e.Value = _dataSource[e.RowIndex].Values[e.ColumnIndex - 1].ToString("F2");
                        //e.Value = _dataSource[e.RowIndex].GetType().GetProperty()

                        try
                        {
                            var p = _dataSource[e.RowIndex].GetType().GetProperties()[e.ColumnIndex];
                            var pVlaue = p.GetValue(_dataSource[e.RowIndex]);

                            if (p.PropertyType == typeof(double))
                            {
                                e.Value = (pVlaue) is double.MinValue ? "NA" : Math.Round((double)pVlaue, 2, MidpointRounding.AwayFromZero).ToString();
                            }
                            else if (p.PropertyType == typeof(string))
                            {
                                e.Value = string.IsNullOrEmpty(pVlaue as string) ? "NA" : pVlaue;
                            }
                        }
                        catch (Exception)
                        {
                            e.Value = string.Empty;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 单元格样式：奇偶行背景色+数据范围警示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var dgv = sender as DataGridView;

            if (e.RowIndex >= 0 && e.RowIndex < _dataSource.Count && dgv.RowCount > 0 && e.RowIndex < dgv.RowCount)
            {
                if (e.ColumnIndex == 0)
                    e.CellStyle.BackColor = Color.LightGray;
                // 检查数据是否超范围（仅对数据列生效）
                else if (e.ColumnIndex > 1 && dgv.ColumnCount > 0 && e.ColumnIndex < dgv.ColumnCount)
                {
                    try
                    {
                        if (e.Value != null && e.Value.ToString() != "NA" && !string.IsNullOrEmpty(e.Value.ToString()))
                        {
                            var dutIndex = e.RowIndex;
                            if (_isRunning)
                            {
                                var paraName = dgv.Columns[e.ColumnIndex].Name;

                                var value = (double)_dataSource[e.RowIndex].GetType().GetProperties()[e.ColumnIndex].GetValue(_dataSource[e.RowIndex]);

                                if (_dx1HEmcConfig.Paras.First(f => f.Name.ToLower() == paraName.ToLower()) != null)
                                {
                                    if (IsNg(paraName, e.Value.ToString()) && _isUpdateNgRed[dutIndex])
                                    {
                                        e.CellStyle.BackColor = Color.Red;
                                        e.CellStyle.ForeColor = Color.White;
                                    }
                                }
                                else
                                {
                                    if (IsMotorNg(paraName, e.Value.ToString()) && _isUpdateMotorNgRed[dutIndex])
                                    {
                                        e.CellStyle.BackColor = Color.Red;
                                        e.CellStyle.ForeColor = Color.White;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// 均分列宽（铺满窗口）
        /// </summary>
        private void ResizeUI()
        {
            try
            {
                var topBtnCount = toolStripTop.Items.OfType<ToolStripButton>().Count() + 1;
                var pertWidth = (int)(Screen.PrimaryScreen.WorkingArea.Width * 1.0) / topBtnCount;
                foreach (var control in toolStripTop.Items)
                    if (control is ToolStripButton button)
                        button.Size = new Size(pertWidth, 35);

                if (dataGridView.Columns.Count == 0)
                    return;
                int columnWidth = (dataGridView.ClientSize.Width / dataGridView.Columns.Count) - 1;
                int rowHeight = ((dataGridView.ClientSize.Height) / (dataGridView.Rows.Count + 1));
                rowHeight = rowHeight <= 10 ? 10 : rowHeight - 1;
                foreach (DataGridViewColumn column in dataGridView.Columns)
                    column.Width = columnWidth;
                foreach (DataGridViewRow row in dataGridView.Rows)
                    row.Height = rowHeight;
                dataGridView.SetColumnHeadersHeight(50);
            }
            catch (Exception)
            {
            }
        }

        private void InitTimer()
        {
            saveTimer.Interval = 100;
            saveTimer.Tick += new EventHandler(saveTimer_Tick);
            saveTimer.Start();

            if (_backgroundTh != null)
            {
                _backgroundTh.Abort();
                _backgroundTh.Join();
            }
            _backgroundTh = new Thread(BackgroundWorker_DoWork) { IsBackground = true };
            _backgroundTh.Start();
        }

        private void saveTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                lblTimeTs.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss:fff");
                lblRunState.Text = _isRunning ? "运行中" : "停止中";
                lblRunState.ForeColor = _isRunning ? Color.Green : Color.DarkGoldenrod;
            }
            catch (Exception)
            {

            }
            UpdateData();
        }

        /// <summary>
        /// 数据更新（实际采集逻辑）
        /// </summary>
        private void UpdateData()
        {
            try
            {
                for (int i = 0; i < DutCount; i++)
                {
                    var isRun = _isRunning;
                    _dataSource[i].Ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                    if (isRun)
                    {
                        if (_dx1hModules[i].IsCanLoss)
                        {
                            SetValueNA(i);
                            _isUpdateNgRed[i] = false;
                            _isUpdateMotorNgRed[i] = false;
                            _dutNgCount[i] = 0;
                            _dutMotorNgCount[i] = 0;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_dx1hModules[i].Msg120) &&
                               !string.IsNullOrEmpty(_dx1hModules[i].Msg149) &&
                               !string.IsNullOrEmpty(_dx1hModules[i].Msg171))
                            {
                                _dataSource[i].KL30_VBAT1 = _dx1hModules[i].KL30_1Voltage;
                                _dataSource[i].KL30_VBAT2 = _dx1hModules[i].KL30_2Voltage;
                                _dataSource[i].KL30_VBAT3 = _dx1hModules[i].KL30_3Voltage;
                                _dataSource[i].NTC1 = _dx1hModules[i].NTC1Temperature;
                                _dataSource[i].NTC2 = _dx1hModules[i].NTC2Temperature;
                                _dataSource[i].SecRIHotCurrent = _dx1hModules[i].SecRIHotCurrent;
                                _dataSource[i].BackrestSeatHotCurrent = _dx1hModules[i].BackrestSeatHotCurrent;
                                _dataSource[i].BackrestSeatHotPWM = _dx1hModules[i].BackrestSeatHotPWM;
                                _dataSource[i].SecRIHotPWM = _dx1hModules[i].SecRIHotPWM;

                                _dataSource[i].Mot2Current = _dx1hModules[i].Mot2Current;
                                _dataSource[i].Mot4Current = _dx1hModules[i].Mot4Current;
                                _dataSource[i].Mot6Current = _dx1hModules[i].Mot6Current;
                                _dataSource[i].Mot8Current = _dx1hModules[i].Mot8Current;
                                _dataSource[i].Mot2Hall = _dx1hModules[i].Mot2Hall;
                                _dataSource[i].Mot4Hall = _dx1hModules[i].Mot4Hall;
                                _dataSource[i].Mot6Hall = _dx1hModules[i].Mot6Hall;
                                _dataSource[i].Mot8Hall = _dx1hModules[i].Mot8Hall;

                                _dataSource[i].Msg171 = _dx1hModules[i].Msg171;
                                _dataSource[i].Msg149 = _dx1hModules[i].Msg149;
                                _dataSource[i].Msg120 = _dx1hModules[i].Msg120;

                                _dataSource[i].Fan1PWM = _duty[i, 0];
                                _dataSource[i].Fan2PWM = _duty[i, 1];

                                _dataSource[i].CanState = "True";

                                var minVolt = 9;
                                var maxVolt = 16;

                                if (_dataSource[i].KL30_VBAT1 < minVolt || _dataSource[i].KL30_VBAT1 > maxVolt ||
                                    _dataSource[i].KL30_VBAT2 < minVolt || _dataSource[i].KL30_VBAT2 > maxVolt ||
                                    _dataSource[i].KL30_VBAT3 < minVolt || _dataSource[i].KL30_VBAT3 > maxVolt)
                                {
                                    _isUpdateNgRed[i] = false;
                                    _isUpdateMotorNgRed[i] = false;
                                    _dutNgCount[i] = 0;
                                    _dutMotorNgCount[i] = 0;
                                }
                                else
                                {
                                    var nowTs = HighPrecisionTimer.GetTimestamp();
                                    var ts = HighPrecisionTimer.GetTimestampIntervalMs(_moduleRunModeStartTs[i], nowTs);

                                    var ngCount = 0;
                                    var motorNgCount = 0;

                                    for (int m = 0; m < _dataSource[i].GetType().GetProperties().Length; m++)
                                    {
                                        var p = _dataSource[i].GetType().GetProperties()[m];
                                        if (p.PropertyType == typeof(double))
                                        {
                                            var pName =
                                                  ((NameAttribute)Attribute.GetCustomAttribute(p, typeof(NameAttribute)))
                                                      .Names[0];

                                            var paraName = pName;
                                            var value = (double)p.GetValue(_dataSource[i]);

                                            if (_dx1HEmcConfig.Paras.ToList().Find(f => f.Name.ToLower() == paraName.ToLower()) != null)
                                            {
                                                ngCount += IsNg(paraName, value.ToString()) ? 1 : 0;
                                            }
                                            else
                                            {
                                                motorNgCount += IsMotorNg(paraName, value.ToString()) ? 1 : 0;
                                            }
                                        }
                                    }

                                    if (ts > 2000)
                                    {
                                        var isNg = false;
                                        var isMotorNg = false;

                                        if (ngCount == 0 && motorNgCount == 0)
                                        {
                                            _isUpdateNgRed[i] = false;
                                            _isUpdateMotorNgRed[i] = false;
                                            _dutNgCount[i] = 0;
                                            _dutMotorNgCount[i] = 0;

                                            _dataSource[i].Result = true;
                                        }
                                        else
                                        {
                                            if (motorNgCount > 0)
                                            {
                                                if (_moduleRunMode[i] == MotorMode.Off)
                                                {
                                                    _isUpdateMotorNgRed[i] = false;
                                                    _dutMotorNgCount[i] = 0;
                                                }
                                                else
                                                {
                                                    _dutMotorNgCount[i]++;
                                                    if (_dutMotorNgCount[i] > 15)
                                                    {
                                                        _isUpdateMotorNgRed[i] = true;
                                                        isMotorNg = true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                _isUpdateMotorNgRed[i] = false;
                                                _dutMotorNgCount[i] = 0;
                                            }

                                            if (ngCount > 0)
                                            {
                                                _dutNgCount[i]++;
                                                if (_dutNgCount[i] > 15)
                                                {
                                                    _isUpdateNgRed[i] = true;
                                                    isNg = true;
                                                }
                                                else
                                                {
                                                    _isUpdateNgRed[i] = false;
                                                }
                                            }
                                            else
                                            {
                                                _isUpdateNgRed[i] = false;
                                                _dutNgCount[i] = 0;
                                            }
                                        }

                                        _dataSource[i].Result = !isNg && !isMotorNg;
                                        _logWriter[i].EnqueueData(_dataSource[i]);
                                    }
                                    else
                                    {
                                        //_dutNgCount[i] = 0;
                                        //_isUpdateNgRed[i] = false;
                                        _isUpdateNgRed[i] = false;
                                        _isUpdateMotorNgRed[i] = false;
                                        _dutNgCount[i] = 0;
                                        _dutMotorNgCount[i] = 0;
                                        _dataSource[i].Result = true;
                                        _logWriter[i].EnqueueData(_dataSource[i]);
                                    }
                                }
                            }
                            else
                            {
                                _isUpdateNgRed[i] = false;
                                _isUpdateMotorNgRed[i] = false;
                            }
                        }
                    }
                    else
                    {
                        SetValueNA(i);
                        _isUpdateNgRed[i] = false;
                        _isUpdateMotorNgRed[i] = false;
                        _dutNgCount[i] = 0;
                        _dutMotorNgCount[i] = 0;
                    }
                }

                dataGridView.Invalidate(); // 触发重绘
                dataGridView.ClearSelection();
            }
            catch (Exception)
            {
            }
        }

        private bool IsNg(string paraName, string douleValue)
        {
            try
            {
                var min = double.MinValue;
                var max = double.MaxValue;

                min = _dx1HEmcConfig.Paras.First(f => f.Name.ToLower() == paraName.ToLower()).Min;
                max = _dx1HEmcConfig.Paras.First(f => f.Name.ToLower() == paraName.ToLower()).Max;
                double value;
                if (double.TryParse(douleValue.ToString(), out value))
                {
                    if (value < min || value > max)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        private bool IsMotorNg(string paraName, string douleValue)
        {
            try
            {
                var min = double.MinValue;
                var max = double.MaxValue;

                min = _dx1HEmcConfig.MotorParas.First(f => f.Name.ToLower() == paraName.ToLower()).Min;
                max = _dx1HEmcConfig.MotorParas.First(f => f.Name.ToLower() == paraName.ToLower()).Max;
                double value;
                if (double.TryParse(douleValue.ToString(), out value))
                {
                    if (value < min || value > max)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        private void SetValueNA(int index)
        {
            try
            {
                for (int j = 0; j < _dataSource[index].GetType().GetProperties().Length; j++)
                {
                    var p = _dataSource[index].GetType().GetProperties()[j];
                    if (p.PropertyType == typeof(double))
                        p.SetValue(_dataSource[index], double.MinValue);
                    else if (p.PropertyType == typeof(string))
                    {
                        var pName =
                                ((NameAttribute)Attribute.GetCustomAttribute(p, typeof(NameAttribute)))
                                    .Names[0];
                        if (pName == "CAN通信")
                            p.SetValue(_dataSource[index], string.Empty);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private int _nowOnMortorIndex = 0;
        private long[] _sleepTs = new long[DutCount];
        private bool _bStart;

        private void BackgroundWorker_DoWork()
        {
            //Run();
            //_isRunning = true;

            for (int i = 0; i < DutCount; i++)
                _sleepTs[i] = 0;

            while (_backgroundTh.IsAlive)
            {
                try
                {
                    if (_step == 1)
                    {
                        Run();
                        _isRunning = true;
                        _step = 2;
                    }
                    else if (_step == 2)
                    {
                        var nowTs = HighPrecisionTimer.GetTimestamp();
                        if (_moduleRunMode[_nowOnMortorIndex] == MotorMode.On)
                        {
                            if (HighPrecisionTimer.GetTimestampIntervalMs(_moduleRunModeStartTs[_nowOnMortorIndex], nowTs) >= 10 * 1000)
                            {
                                {
                                    var _emcModule = _dx1hModules[_nowOnMortorIndex];
                                    //_emcModule.Group1Motro2_BackrestMotOff();
                                    //_emcModule.Group2Motro4_LenMotOff();
                                    //_emcModule.Group3Motro6_BackrestMotOff();
                                    //_emcModule.Group4Motro8_LenMotOff();
                                    _emcModule.Heat50OnMotorOff();
                                    _moduleRunMode[_nowOnMortorIndex] = MotorMode.Off;
                                    _moduleRunModeStartTs[_nowOnMortorIndex] = nowTs;
                                }

                                _nowOnMortorIndex++;
                                if (_nowOnMortorIndex == DutCount)
                                    _nowOnMortorIndex = 0;
                                continue;
                            }
                        }
                        else
                        {
                            var sleepTs = HighPrecisionTimer.GetTimestampIntervalMs(_sleepTs[_nowOnMortorIndex], nowTs);
                            if (sleepTs >= 50 * 1000 || _sleepTs[_nowOnMortorIndex] == 0)
                            {
                                Run();
                                continue;
                            }
                        }
                    }
                    else if (_step == 3)
                    {
                        var nowTs = HighPrecisionTimer.GetTimestamp();

                        if (HighPrecisionTimer.GetTimestampIntervalMs(_moduleRunModeStartTs[_nowOnMortorIndex], nowTs) >= 60 * 1000)
                        {

                        }
                    }
                    else if (_step == 4)
                    {
                        _step = 0;
                        _isRunning = false;
                        _nowOnMortorIndex = 0;
                        for (int i = 0; i < DutCount; i++)
                            _sleepTs[i] = 0;
                        Stop();
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void Run()
        {
            try
            {
                var ts = HighPrecisionTimer.GetTimestamp();

                for (var i = 0; i < DutCount; i++)
                {
                    var _emcModule = _dx1hModules[i];
                    //_emcModule.SeatHeatgCmdBackSecRightOn("100");
                    //_emcModule.SeatHeatgCmdCushSecRightOn("100");
                    //_emcModule.SeatVentnCmdBackSecRightOn("100");
                    //_emcModule.SeatVentnCmdCushSecRightOn("100");

                    if (_nowOnMortorIndex == i)
                    {
                        //_emcModule.Group1Motro2Start_BackrestMotOn();
                        //_emcModule.Group2Motro4Start_LenMotOn();
                        //_emcModule.Group3Motro6Start_BackrestMotOn();
                        //_emcModule.Group4Motro8Start_LenMotOn();

                        _emcModule.Heat50OnMotorOn();

                        _moduleRunMode[i] = MotorMode.On;
                        _sleepTs[i] = HighPrecisionTimer.GetTimestamp();
                    }
                    else
                    {
                        //_emcModule.Group1Motro2_BackrestMotOff();
                        //_emcModule.Group2Motro4_LenMotOff();
                        //_emcModule.Group3Motro6_BackrestMotOff();
                        //_emcModule.Group4Motro8_LenMotOff();

                        _emcModule.Heat50OnMotorOff();

                        _moduleRunMode[i] = MotorMode.Off;
                    }

                    _moduleRunModeStartTs[i] = ts;
                    _emcModule.StartScheduler();
                }
            }
            catch (Exception)
            {

            }
        }

        private void Stop()
        {
            try
            {
                var ts = HighPrecisionTimer.GetTimestamp();

                for (var i = 0; i < DutCount; i++)
                {
                    var _emcModule = _dx1hModules[i];
                    //_emcModule.Group1Motro2_BackrestMotOff();
                    //_emcModule.Group2Motro4_LenMotOff();
                    //_emcModule.Group3Motro6_BackrestMotOff();
                    //_emcModule.Group4Motro8_LenMotOff();

                    //_emcModule.SeatVentnCmdCushSecRightOff();
                    //_emcModule.SeatVentnCmdBackSecRightOff();
                    //_emcModule.SeatHeatgCmdBackSecRightOff();
                    //_emcModule.SeatHeatgCmdCushSecRightOff();

                    _emcModule.AllOff();

                    _moduleRunMode[i] = MotorMode.Off;
                    _moduleRunModeStartTs[i] = ts;
                }

                Thread.Sleep(1000);

                for (var i = 0; i < DutCount; i++)
                {
                    var _emcModule = _dx1hModules[i];
                    _emcModule.StopScheduler();
                }
            }
            catch (Exception)
            {
            }
        }

        public class Dx1hData
        {
            [Name("时间戳")]
            public string Ts { get; set; }

            [Name("KL30电压1")]
            public double KL30_VBAT1 { get; set; }

            [Name("KL30电压2")]
            public double KL30_VBAT2 { get; set; }

            [Name("KL30电压3")]
            public double KL30_VBAT3 { get; set; }

            [Name("NTC1")]
            public double NTC1 { get; set; }

            [Name("NTC2")]
            public double NTC2 { get; set; }

            [Name("坐垫加热电流值")]
            public double SecRIHotCurrent { get; set; }

            [Name("靠背加热电流值")]
            public double BackrestSeatHotCurrent { get; set; }

            [Name("靠背加热PWM")]
            public double BackrestSeatHotPWM { get; set; }

            [Name("坐垫加热PWM")]
            public double SecRIHotPWM { get; set; }

            [Name("靠背风扇PWM")]
            public double Fan1PWM { get; set; }

            [Name("坐垫风扇PWM")]
            public double Fan2PWM { get; set; }

            [Name("靠背电流值")]
            public double Mot2Current { get; set; }

            [Name("滑轨电流值")]
            public double Mot4Current { get; set; }

            [Name("腿托垂直电流值")]
            public double Mot6Current { get; set; }

            [Name("腿托水平电流值")]
            public double Mot8Current { get; set; }

            [Name("霍尔值")]
            public double Mot2Hall { get; set; }

            [Name("霍尔值")]
            public double Mot4Hall { get; set; }

            [Name("霍尔值")]
            public double Mot6Hall { get; set; }

            [Name("霍尔值")]
            public double Mot8Hall { get; set; }

            [Name("CAN通信")]
            public string CanState { get; set; }

            public bool Result;

            [Name("0x171数据")]
            public string Msg171 { get; set; }

            [Name("0x120数据")]
            public string Msg120 { get; set; }

            [Name("0x149数据")]
            public string Msg149 { get; set; }

            public Dx1hData()
            {
                Ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
            }
        }

        internal enum MotorMode
        {
            Off,

            On
        }

        private void btnAllStart_Click(object sender, EventArgs e)
        {
            _step = 1;
            UIMessageTip.ShowOk("已开始");
        }

        private void btnAllStop_Click(object sender, EventArgs e)
        {
            _step = 4;
            UIMessageTip.ShowWarning("已停止");
        }

        private void tsbtnDetectionPara_Click(object sender, EventArgs e)
        {
            var toUpdate1 = new Dx1hEmcConfig.DeviceConfigPara[_dx1HEmcConfig.Paras.Length];
            for (var i = 0; i < _dx1HEmcConfig.Paras.Length; i++)
            {
                toUpdate1[i] = new Dx1hEmcConfig.DeviceConfigPara();
                toUpdate1[i].Name = _dx1HEmcConfig.Paras[i].Name;
                toUpdate1[i].Min = _dx1HEmcConfig.Paras[i].Min;
                toUpdate1[i].Max = _dx1HEmcConfig.Paras[i].Max;
            }

            var toUpdate2 = new Dx1hEmcConfig.DeviceConfigMotorPara[_dx1HEmcConfig.MotorParas.Length];
            for (var i = 0; i < _dx1HEmcConfig.MotorParas.Length; i++)
            {
                toUpdate2[i] = new Dx1hEmcConfig.DeviceConfigMotorPara();
                toUpdate2[i].Name = _dx1HEmcConfig.MotorParas[i].Name;
                toUpdate2[i].Min = _dx1HEmcConfig.MotorParas[i].Min;
                toUpdate2[i].Max = _dx1HEmcConfig.MotorParas[i].Max;
            }

            using (var frm = new FrmParaSet(toUpdate1, toUpdate2))
            {
                var result = frm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    _dx1HEmcConfig.Paras = frm._p1;
                    _dx1HEmcConfig.MotorParas = frm._p2;
                    XmlHelper.SerializeToFile(_dx1HEmcConfig, _configFile, Encoding.UTF8);
                    this.ShowSuccessTip("参数已更新");
                }
                else
                {
                    this.ShowInfoTip("操作取消，未更新参数");
                }
            }
        }

        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        private void btnOpenLogFolder_Click(object sender, EventArgs e)
        {
            var driveLetter = string.Format(@"{0}{1}", Path.GetPathRoot(Program.SysDir), "DX1H_实验箱");

            if (Directory.Exists(driveLetter))
            {
                try
                {
                    ShellExecute(IntPtr.Zero, "explore", driveLetter, null, null, 1); // 1表示正常窗口
                }
                catch (Exception ex)
                {
                    this.ShowErrorTip("打开文件夹失败：" + ex.Message);
                }
            }
            else
            {
                this.ShowErrorTip("路径不存在");
            }
        }
    }
}
