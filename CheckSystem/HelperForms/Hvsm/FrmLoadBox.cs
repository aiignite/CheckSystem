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

namespace CheckSystem.HelperForms.Hvsm
{
    public partial class FrmLoadBox : UIForm
    {
        public class HvsmData
        {
            [Name("时间戳")]
            public string Ts { get; set; }

            [Name("模式")]
            public string Mode { get; set; }

            [Name("VBAT1(mV)")]
            public double VBat1 { get; set; }

            [Name("VBAT2(mV)")]
            public double VBat2 { get; set; }

            [Name("HSD1_电流(mA)")]
            public double HSD1_Current { get; set; }

            [Name("HSD2_电流(mA)")]
            public double HSD2_Current { get; set; }

            [Name("HSD3_电流(mA)")]
            public double HSD3_Current { get; set; }

            [Name("HSD4_电流(mA)")]
            public double HSD4_Current { get; set; }

            [Name("LIN通信")]
            public string LinCommunicatingState { get; set; }

            [Name("NTC1")]
            public double NTC1 { get; set; }

            [Name("NTC2")]
            public double NTC2 { get; set; }

            [Name("NTC3")]
            public double NTC3 { get; set; }

            [Name("NTC4")]
            public double NTC4 { get; set; }

            [Name("INPUT")]
            public string Input { get; set; }

            [Name("风扇1占空比")]
            public double Fan1Duty { get; set; }

            [Name("风扇2占空比")]
            public double Fan2Duty { get; set; }

            [Name("循环次数")]
            public int CycleCount { get; set; }

            [Name("0x10数据")]
            public string Msg0X10 { get; set; }

            [Name("0x11数据")]
            public string Msg0X11 { get; set; }

            [Name("0x12数据")]
            public string Msg0X12 { get; set; }

            [Name("0x13数据")]
            public string Msg0X13 { get; set; }

            [Name("RESULT")]
            public bool Result { get; set; }

            public HvsmData()
            {
                Ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
            }
        }

        private BindingList<HvsmData> _dataSource = new BindingList<HvsmData>();
        private HvsmEmcConfig.DeviceConfig _hvsmConfig;

        public FrmLoadBox()
        {
            InitializeComponent();

            if (File.Exists(_configFile))
            {
                _hvsmConfig = XmlHelper.Deserialize<HvsmEmcConfig.DeviceConfig>(_configFile);
                if (_hvsmConfig != null)
                {
                    Load += FrmLoadBox_Load; ;
                    FormClosed += FrmLoadBox_FormClosed;
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

        private const int DutCount = 9;
        private string _configFile = string.Format(@"{0}\{1}\{2}", Program.SysDir, "ControllerConfig", "HVSM_LoadBox.xml");//@"D:\Projects\CheckSystem\CheckSystem\bin\Debug\ControllerConfig\HVSM_LoadBox.xml";

        private HvsmEmcModule[] _hvsmEmcModules = new HvsmEmcModule[DutCount];
        private SyRenesasMcuControllerMaster[] _controllers = new SyRenesasMcuControllerMaster[DutCount];
        private SyRenesasMcuControllerSlaveWith8RLs SyRenesasMcuControllerSlaveWith8RLs_30_201 = new SyRenesasMcuControllerSlaveWith8RLs("30_201_RL");
        private CsvFileHelper<HvsmData>[] _logWriter = new CsvFileHelper<HvsmData>[DutCount];
        private BackgroundWorker _adWorker = new BackgroundWorker();
        private FrmSingleControl[] _frmSingleControls = new FrmSingleControl[DutCount];

        private bool[] _isUpdateNgRed = new bool[DutCount];
        private int[] _dutNgCount = new int[DutCount];

        private HvsmEmcModule _monitorTempModule = new HvsmEmcModule("温度监控控制器");
        private SyRenesasMcuControllerMaster _monitorTempController = new SyRenesasMcuControllerMaster("温度监控控制器板卡");
        private BackgroundWorker _tempWorker = new BackgroundWorker();

        private void FrmLoadBox_Load(object sender, EventArgs e)
        {
            InitController();
            InitMainDgv();
            InitTimer();
            Resize += (rs, re) => ResizeUI();
            WindowState = FormWindowState.Maximized;
        }

        private void FrmLoadBox_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveTimer.Stop();
            saveTimer.Enabled = false;

            foreach (var item in _frmSingleControls)
            {
                if (item == null)
                    continue;

                item.DisposeTh();
            }

            foreach (var hvsm in _hvsmEmcModules)
                hvsm.Dispose();

            _adWorker.CancelAsync();
            _tempWorker.CancelAsync();
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
                    var isRun = _frmSingleControls[i].IsRunning;

                    _dataSource[i].Ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                    if (isRun)
                    {
                        if (_hvsmEmcModules[i].IsLinLoss)
                        {
                            SetValueNA(i);
                            //if (frmSingleControls[i].Mode == 0)
                            _dataSource[i].Mode = _frmSingleControls[i].NowMode;
                            _isUpdateNgRed[i] = false;
                            _dutNgCount[i] = 0;
                        }
                        else
                        {
                            if (_frmSingleControls[i].NowMode.ToLower().Contains("sleep"))
                            {
                                SetValueNA(i);
                                //if (frmSingleControls[i].Mode == 0)
                                _dataSource[i].Mode = _frmSingleControls[i].NowMode;
                                _isUpdateNgRed[i] = false;
                                _dutNgCount[i] = 0;
                            }
                            else if (!string.IsNullOrEmpty(_hvsmEmcModules[i].Msg0x10) &&
                                !string.IsNullOrEmpty(_hvsmEmcModules[i].Msg0x11) &&
                                !string.IsNullOrEmpty(_hvsmEmcModules[i].Msg0x12) &&
                                !string.IsNullOrEmpty(_hvsmEmcModules[i].Msg0x13) &&
                                !string.IsNullOrEmpty(_frmSingleControls[i].NowMode))
                            {
                                _dataSource[i].VBat1 = _hvsmEmcModules[i].Vbat1;
                                _dataSource[i].VBat2 = _hvsmEmcModules[i].Vbat2;
                                if (_hvsmEmcModules[i].HSD1_Current <= 1)
                                    _dataSource[i].HSD1_Current = _hvsmEmcModules[i].HSD1_Current;
                                else
                                    _dataSource[i].HSD1_Current = Math.Round(_hvsmConfig.OffsetPara[i][0].K * _hvsmEmcModules[i].HSD1_Current + _hvsmConfig.OffsetPara[i][0].B, 2, MidpointRounding.AwayFromZero);

                                if (_hvsmEmcModules[i].HSD2_Current <= 1)
                                    _dataSource[i].HSD2_Current = _hvsmEmcModules[i].HSD2_Current;
                                else
                                    _dataSource[i].HSD2_Current = Math.Round(_hvsmConfig.OffsetPara[i][1].K * _hvsmEmcModules[i].HSD2_Current + _hvsmConfig.OffsetPara[i][1].B, 2, MidpointRounding.AwayFromZero);

                                if (_hvsmEmcModules[i].HSD3_Current <= 1)
                                    _dataSource[i].HSD3_Current = _hvsmEmcModules[i].HSD3_Current;
                                else
                                    _dataSource[i].HSD3_Current = Math.Round(_hvsmConfig.OffsetPara[i][2].K * _hvsmEmcModules[i].HSD3_Current + _hvsmConfig.OffsetPara[i][2].B, 2, MidpointRounding.AwayFromZero);

                                if (_hvsmEmcModules[i].HSD4_Current <= 1)
                                    _dataSource[i].HSD4_Current = _hvsmEmcModules[i].HSD4_Current;
                                else
                                    _dataSource[i].HSD4_Current = Math.Round(_hvsmConfig.OffsetPara[i][3].K * _hvsmEmcModules[i].HSD4_Current + _hvsmConfig.OffsetPara[i][3].B, 2, MidpointRounding.AwayFromZero);

                                if (bool.Parse(_hvsmConfig.OffsetPara[i][0].IsEnable) && _dataSource[i].HSD1_Current > _hvsmConfig.OffsetPara[i][0].Threshold)
                                    _dataSource[i].HSD1_Current = _hvsmConfig.OffsetPara[i][0].ShowValue;
                                if (bool.Parse(_hvsmConfig.OffsetPara[i][1].IsEnable) && _dataSource[i].HSD2_Current > _hvsmConfig.OffsetPara[i][1].Threshold)
                                    _dataSource[i].HSD2_Current = _hvsmConfig.OffsetPara[i][1].ShowValue;
                                if (bool.Parse(_hvsmConfig.OffsetPara[i][2].IsEnable) && _dataSource[i].HSD3_Current > _hvsmConfig.OffsetPara[i][2].Threshold)
                                    _dataSource[i].HSD3_Current = _hvsmConfig.OffsetPara[i][2].ShowValue;
                                if (bool.Parse(_hvsmConfig.OffsetPara[i][3].IsEnable) && _dataSource[i].HSD4_Current > _hvsmConfig.OffsetPara[i][3].Threshold)
                                    _dataSource[i].HSD4_Current = _hvsmConfig.OffsetPara[i][3].ShowValue;

                                _dataSource[i].LinCommunicatingState = "True";
                                _dataSource[i].Input = _hvsmEmcModules[i].Input;
                                _dataSource[i].Mode = _frmSingleControls[i].NowMode;
                                _dataSource[i].Msg0X10 = _hvsmEmcModules[i].Msg0x10;
                                _dataSource[i].Msg0X11 = _hvsmEmcModules[i].Msg0x11;
                                _dataSource[i].Msg0X12 = _hvsmEmcModules[i].Msg0x12;
                                _dataSource[i].Msg0X13 = _hvsmEmcModules[i].Msg0x13;
                                _dataSource[i].NTC1 = _hvsmEmcModules[i].Ntc1;
                                _dataSource[i].NTC2 = _hvsmEmcModules[i].Ntc2;
                                _dataSource[i].NTC3 = _hvsmEmcModules[i].Ntc3;
                                _dataSource[i].NTC4 = _hvsmEmcModules[i].Ntc4;

                                //lock (_lockAdDuty)
                                {
                                    _dataSource[i].Fan1Duty = _duty[i, 0];
                                    _dataSource[i].Fan2Duty = _duty[i, 1];
                                }

                                var nowTs = HighPrecisionTimer.GetTimestamp();
                                var ts = HighPrecisionTimer.GetTimestampIntervalMs(_frmSingleControls[i].ModeStartTs, nowTs);

                                var ngCount = 0;
                                for (int k = 2; k <= 15; k++)
                                {
                                    if (k != 8 && k != 13)
                                    {
                                        var paraName = GetParaName(k);
                                        var value = (double)_dataSource[i].GetType().GetProperties()[k].GetValue(_dataSource[i]);
                                        ngCount += IsNg(paraName, value.ToString()) ? 1 : 0;
                                    }
                                }

                                if (ts > 3500)
                                {
                                    if (ngCount == 0)
                                    {
                                        _isUpdateNgRed[i] = false;
                                        _dutNgCount[i] = 0;
                                        _dataSource[i].Result = true;
                                        _logWriter[i].EnqueueData(_dataSource[i]);
                                    }
                                    else
                                    {
                                        _dutNgCount[i]++;
                                        if (_dutNgCount[i] > 5)
                                        {
                                            _isUpdateNgRed[i] = true;
                                            _dataSource[i].Result = false;
                                            _logWriter[i].EnqueueData(_dataSource[i]);
                                        }
                                        else
                                        {
                                            _isUpdateNgRed[i] = false;
                                            _dataSource[i].Result = true;
                                            _logWriter[i].EnqueueData(_dataSource[i]);
                                        }
                                    }
                                }
                                else
                                {
                                    _dutNgCount[i] = 0;
                                    _isUpdateNgRed[i] = false;
                                    _dataSource[i].Result = true;
                                    _logWriter[i].EnqueueData(_dataSource[i]);
                                }

                            }
                        }
                    }
                    else
                    {
                        SetValueNA(i);
                        _isUpdateNgRed[i] = false;
                        _dutNgCount[i] = 0;
                    }
                }

                dataGridView.Invalidate(); // 触发重绘
                dataGridView.ClearSelection();
            }
            catch (Exception)
            {
            }
        }

        private void SetValueNA(int index)
        {
            try
            {
                for (int j = 1; j <= 15; j++)
                {
                    var p = _dataSource[index].GetType().GetProperties()[j];
                    if (p.PropertyType == typeof(double))
                    {
                        p.SetValue(_dataSource[index], double.MinValue);
                    }
                    else if (p.PropertyType == typeof(string))
                    {
                        p.SetValue(_dataSource[index], string.Empty);
                    }
                }
            }
            catch (Exception)
            {
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
                    else if (e.ColumnIndex > 0 && e.ColumnIndex <= 15)
                    {
                        //e.Value = _dataSource[e.RowIndex].Values[e.ColumnIndex - 1].ToString("F2");
                        //e.Value = _dataSource[e.RowIndex].GetType().GetProperty()

                        var p = _dataSource[e.RowIndex].GetType().GetProperties()[e.ColumnIndex];
                        var pVlaue = p.GetValue(_dataSource[e.RowIndex]);
                        //e.Value = p.GetValue(_dataSource[e.RowIndex]);

                        if (p.PropertyType == typeof(double))
                        {
                            e.Value = (pVlaue) is double.MinValue ? "NA" : pVlaue;
                        }
                        else if (p.PropertyType == typeof(string))
                        {
                            e.Value = string.IsNullOrEmpty(pVlaue as string) ? "NA" : pVlaue;
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

            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < _dataSource.Count && dgv.RowCount > 0 && e.RowIndex < dgv.RowCount)
                {
                    // 奇偶行背景色
                    //e.CellStyle.BackColor = (e.RowIndex % 2 == 0) ? Color.WhiteSmoke : Color.LightGray;

                    if (e.ColumnIndex == 0)
                        e.CellStyle.BackColor = Color.LightGray;

                    // 检查数据是否超范围（仅对数据列生效）
                    if (e.ColumnIndex > 1 && e.ColumnIndex <= 15 && dgv.ColumnCount > 0 && e.ColumnIndex < dgv.ColumnCount)
                    {
                        if (e.Value != null && e.Value.ToString() != "NA" && !string.IsNullOrEmpty(e.Value.ToString()))
                        {
                            try
                            {
                                var dutIndex = e.RowIndex;

                                if (e.ColumnIndex > 1 && e.ColumnIndex != 8 && e.ColumnIndex != 13 && _frmSingleControls[dutIndex].IsRunning)
                                {
                                    var paraName = GetParaName(e.ColumnIndex);
                                    if (IsNg(paraName, e.Value.ToString()) && _isUpdateNgRed[dutIndex])
                                    {
                                        e.CellStyle.BackColor = Color.Red;
                                        e.CellStyle.ForeColor = Color.White;
                                    }

                                    //var min = double.MinValue;
                                    //var max = double.MaxValue;
                                    //min = _hvsmConfig.Paras.First(f => f.Name.ToLower() == paraName.ToLower()).Min;
                                    //max = _hvsmConfig.Paras.First(f => f.Name.ToLower() == paraName.ToLower()).Max;
                                    //double value;
                                    //if (double.TryParse(e.Value.ToString(), out value))
                                    //{
                                    //    if (value < min || value > max)
                                    //    {
                                    //        if (_isUpdateNgRed[dutIndex])
                                    //        {
                                    //            e.CellStyle.BackColor = Color.Red;
                                    //            e.CellStyle.ForeColor = Color.White;
                                    //        }
                                    //    }
                                    //}
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private string GetParaName(int index)
        {
            var paraName = string.Empty;

            switch (index)
            {
                case 2:
                    paraName = "VBAT1";
                    break;

                case 3:
                    paraName = "VBAT2";
                    break;

                case 4:
                    paraName = "HSD1_电流";
                    break;

                case 5:
                    paraName = "HSD2_电流";
                    break;

                case 6:
                    paraName = "HSD3_电流";
                    break;

                case 7:
                    paraName = "HSD4_电流";
                    break;

                case 9:
                    paraName = "NTC1";
                    break;

                case 10:
                    paraName = "NTC2";
                    break;

                case 11:
                    paraName = "NTC3";
                    break;

                case 12:
                    paraName = "NTC4";
                    break;

                case 14:
                    paraName = "风扇1占空比";
                    break;

                case 15:
                    paraName = "风扇2占空比";
                    break;
            }

            return paraName;
        }

        private bool IsNg(string paraName, string douleValue)
        {
            try
            {
                var min = double.MinValue;
                var max = double.MaxValue;

                min = _hvsmConfig.Paras.First(f => f.Name.ToLower() == paraName.ToLower()).Min;
                max = _hvsmConfig.Paras.First(f => f.Name.ToLower() == paraName.ToLower()).Max;
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
                dataGridView.SetColumnHeadersHeight(rowHeight);
            }
            catch (Exception)
            {

            }
        }

        private void InitTimer()
        {
            saveTimer.Interval = 80;
            saveTimer.Tick += new EventHandler(saveTimer_Tick);
            saveTimer.Start();
        }

        private void saveTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                lblTimeTs.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss:fff");
                lblTimeTs.ForeColor = _isStartAdTh ? Color.FromArgb(0, 0, 192) : Color.LightGray;
            }
            catch (Exception)
            {

            }
            UpdateData();
        }

        private void InitController()
        {
            var isLogFolderOk = false;

            var driveLetter = string.Format(@"{0}\{1}", Path.GetPathRoot(Program.SysDir), "HVSM_实验箱");
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

            for (int i = 0; i < DutCount; i++)
            {
                if (i <= 4)
                {
                    var ip = string.Format("192.168.1.{0}:8088", 28 + i);
                    _controllers[i] = new SyRenesasMcuControllerMaster("Controller" + i);
                    _controllers[i].InitRemoteIpAddress(ip);
                }

                _hvsmEmcModules[i] = new HvsmEmcModule("HVSM" + i);
                //_hvsmEmcModules[i].StartLin();

                _logWriter[i] = new CsvFileHelper<HvsmData>(string.Format(@"{0}\{1}", driveLetter, "产品" + (i + 1).ToString()));
                _logWriter[i].Start();
            }

            _hvsmEmcModules[0].Lin = _controllers[0].GatewayLin1;
            _hvsmEmcModules[1].Lin = _controllers[0].GatewayLin2;
            _hvsmEmcModules[2].Lin = _controllers[1].GatewayLin1;
            _hvsmEmcModules[3].Lin = _controllers[1].GatewayLin2;
            _hvsmEmcModules[4].Lin = _controllers[2].GatewayLin1;
            _hvsmEmcModules[5].Lin = _controllers[2].GatewayLin2;
            _hvsmEmcModules[6].Lin = _controllers[3].GatewayLin1;
            _hvsmEmcModules[7].Lin = _controllers[3].GatewayLin2;
            _hvsmEmcModules[8].Lin = _controllers[4].GatewayLin1;

            SyRenesasMcuControllerSlaveWith8RLs_30_201.ConnectMaster("192.168.1.30", "0x201");

            _adWorker.WorkerSupportsCancellation = true;
            _adWorker.DoWork += _adWorker_DoWork;
            _adWorker.RunWorkerAsync();

            for (int i = 0; i < DutCount; i++)
            {
                _frmSingleControls[i] = new FrmSingleControl("DUT" + (i + 1).ToString(), _hvsmEmcModules[i]);
                _frmSingleControls[i].SetCycle(_hvsmConfig.CycleParas[i]);
            }

            for (int i = 0; i < DutCount; i++)
                _frmSingleControls[i].SetStandByModePara
                    (_hvsmConfig.StandbyModePara.Hsds[0].Duty, _hvsmConfig.StandbyModePara.Hsds[0].Freq,
                    _hvsmConfig.StandbyModePara.Hsds[1].Duty, _hvsmConfig.StandbyModePara.Hsds[1].Freq,
                    _hvsmConfig.StandbyModePara.Hsds[2].Duty, _hvsmConfig.StandbyModePara.Hsds[2].Freq,
                    _hvsmConfig.StandbyModePara.Hsds[3].Duty, _hvsmConfig.StandbyModePara.Hsds[3].Freq,
                    _hvsmConfig.StandbyModePara.Fans[0].Duty,
                    _hvsmConfig.StandbyModePara.Fans[1].Duty);

            uiTableLayoutPanel1.Controls.Add(_frmSingleControls[0], 0, 0);
            uiTableLayoutPanel1.Controls.Add(_frmSingleControls[1], 1, 0);
            uiTableLayoutPanel1.Controls.Add(_frmSingleControls[2], 2, 0);
            uiTableLayoutPanel1.Controls.Add(_frmSingleControls[3], 0, 1);
            uiTableLayoutPanel1.Controls.Add(_frmSingleControls[4], 1, 1);
            uiTableLayoutPanel1.Controls.Add(_frmSingleControls[5], 2, 1);
            uiTableLayoutPanel1.Controls.Add(_frmSingleControls[6], 0, 2);
            uiTableLayoutPanel1.Controls.Add(_frmSingleControls[7], 1, 2);
            uiTableLayoutPanel1.Controls.Add(_frmSingleControls[8], 2, 2);

            _monitorTempController.InitRemoteIpAddress("192.168.1.100:8088");
            _monitorTempModule.Lin = _monitorTempController.GatewayLin1;
            _monitorTempModule.StartLin();
            _tempWorker.WorkerSupportsCancellation = true;
            _tempWorker.DoWork += _tempWorker_DoWork;
            _tempWorker.RunWorkerAsync();
        }

        private void _tempWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            while (!worker.CancellationPending)
            {
                try
                {
                    var isEnable = bool.Parse(_hvsmConfig.TemperaturePara.IsEnable);

                    if (isEnable)
                    {
                        var ntc = double.MinValue;
                        if (!_monitorTempModule.IsLinLoss)
                        {
                            if (_hvsmConfig.TemperaturePara.NtcChannel == 1)
                                ntc = _monitorTempModule.Ntc1;
                            else if (_hvsmConfig.TemperaturePara.NtcChannel == 2)
                                ntc = _monitorTempModule.Ntc2;
                            else if (_hvsmConfig.TemperaturePara.NtcChannel == 3)
                                ntc = _monitorTempModule.Ntc3;
                            else if (_hvsmConfig.TemperaturePara.NtcChannel == 4)
                                ntc = _monitorTempModule.Ntc4;
                        }

                        if (!_monitorTempModule.IsLinLoss)
                        {
                            for (int i = 0; i < DutCount; i++)
                            {
                                _hvsmEmcModules[i].IsEnableTempMonitor = true;
                                _hvsmEmcModules[i].MonitorSymbol = _hvsmConfig.TemperaturePara.Sysmbol;
                                _hvsmEmcModules[i].MonitorValue = ntc;
                                _hvsmEmcModules[i].MonitorThreshold = _hvsmConfig.TemperaturePara.Value;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < DutCount; i++)
                            {
                                _hvsmEmcModules[i].IsEnableTempMonitor = false;
                                _hvsmEmcModules[i].MonitorSymbol = string.Empty;
                                _hvsmEmcModules[i].MonitorValue = double.MinValue;
                                _hvsmEmcModules[i].MonitorThreshold = double.MinValue;
                            }
                        }

                        Invoke(new Action(() =>
                        {
                            lblIsTempEnable.ForeColor = lblCurrTemp.ForeColor = lblTempThreshold.ForeColor = Color.FromArgb(0, 0, 192);
                            lblIsTempEnable.Text = "温度监控已启用: NTC" + _hvsmConfig.TemperaturePara.NtcChannel;
                            lblCurrTemp.Text = _monitorTempModule.IsLinLoss ? "当前：NA" : "当前：" + ntc.ToString("f2");
                            lblTempThreshold.Text = "判定值：" + _hvsmConfig.TemperaturePara.Sysmbol + _hvsmConfig.TemperaturePara.Value.ToString();
                            lblIsTempMonitorOnline.Text = _monitorTempModule.IsLinLoss ? "监控板离线" : "监控板在线";
                            lblIsTempMonitorOnline.ForeColor = _monitorTempModule.IsLinLoss ? Color.Red : Color.FromArgb(0, 0, 192);
                        }));
                    }
                    else
                    {
                        for (int i = 0; i < DutCount; i++)
                        {
                            _hvsmEmcModules[i].IsEnableTempMonitor = false;
                            _hvsmEmcModules[i].MonitorSymbol = string.Empty;
                            _hvsmEmcModules[i].MonitorValue = double.MinValue;
                            _hvsmEmcModules[i].MonitorThreshold = double.MinValue;
                        }

                        Invoke(new Action(() =>
                        {
                            lblIsTempEnable.Text = "温度监控已禁用";
                            lblIsTempEnable.ForeColor = _monitorTempModule.IsLinLoss ? Color.Gray : Color.FromArgb(0, 0, 192);
                            lblCurrTemp.Text = "NA";
                            lblTempThreshold.Text = "NA";
                            lblIsTempMonitorOnline.Text = "NA";
                            lblCurrTemp.ForeColor = lblTempThreshold.ForeColor = lblIsTempMonitorOnline.ForeColor = Color.Gray;
                        }));
                    }


                }
                catch (Exception)
                {

                }
                finally
                {
                    Thread.Sleep(250);
                }
            }
        }

        private double[,] _duty = new double[DutCount, 2];
        private readonly object _lockAdDuty = new object();
        private bool _isStartAdTh = false;

        private void _adWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            _isStartAdTh = true;

            SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay1 = !true;
            SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay2 = !true;
            SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay3 = !true;
            SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay4 = !true;
            SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay5 = !true;
            SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay6 = !true;
            SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay7 = !true;
            SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay8 = !true;
            SyRenesasMcuControllerSlaveWith8RLs_30_201.SetOutputs();
            var lastSetRls = HighPrecisionTimer.GetTimestamp();
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

                        //var task = new Task[1];
                        for (var i = 0; i < 5; i++)
                            _controllers[i].MasterReadDIsAndDuty();

                        //Task.WaitAll(task);

                        //lock (_lockAdDuty)
                        {
                            try
                            {
                                _duty[0, 0] = _controllers[0].Duty1;
                                _duty[0, 1] = _controllers[0].Duty2;

                                _duty[1, 0] = _controllers[0].Duty3;
                                _duty[1, 1] = _controllers[0].Duty4;

                                _duty[2, 0] = _controllers[1].Duty1;
                                _duty[2, 1] = _controllers[1].Duty2;

                                _duty[3, 0] = _controllers[1].Duty3;
                                _duty[3, 1] = _controllers[1].Duty4;

                                _duty[4, 0] = _controllers[2].Duty1;
                                _duty[4, 1] = _controllers[2].Duty2;

                                _duty[5, 0] = _controllers[2].Duty3;
                                _duty[5, 1] = _controllers[2].Duty4;

                                _duty[6, 0] = _controllers[3].Duty1;
                                _duty[6, 1] = _controllers[3].Duty2;

                                _duty[7, 0] = _controllers[3].Duty3;
                                _duty[7, 1] = _controllers[3].Duty4;

                                _duty[8, 0] = _controllers[4].Duty1;
                                _duty[8, 1] = _controllers[4].Duty2;
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }

                    if (HighPrecisionTimer.GetTimestampIntervalMs(lastSetRls, ns) > 2000)
                    {
                        //SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay1 = !SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay1;
                        //SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay2 = !SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay2;
                        //SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay3 = !SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay3;
                        //SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay4 = !SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay4;
                        //SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay5 = !SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay5;
                        //SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay6 = !SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay6;
                        //SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay7 = !SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay7;
                        //SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay8 = !SyRenesasMcuControllerSlaveWith8RLs_30_201.Relay8;

                        SyRenesasMcuControllerSlaveWith8RLs_30_201.SetOutputs();
                        lastSetRls = HighPrecisionTimer.GetTimestamp();
                    }
                }
                catch (Exception)
                {
                }

                Thread.Sleep(100);
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
                    _dataSource.Add(new HvsmData());
                // 添加15列（数据类型）
                dataGridView.AddColumn("DUT", "DUT");
                for (int i = 1; i <= 15; i++)
                {
                    var tp = new HvsmData();
                    var p = tp.GetType().GetProperties()[i];
                    var pName =
                               ((NameAttribute)Attribute.GetCustomAttribute(p, typeof(NameAttribute)))
                                   .Names[0];

                    dataGridView.AddColumn(pName, p.Name);
                }

                dataGridView.RowCount = DutCount;
            }
        }

        private void btnAllSleepMode_Click(object sender, EventArgs e)
        {
            if (_frmSingleControls.Any(f => f.IsRunning))
            {
                this.ShowErrorDialog("请先停止所有DUT的测试");
                return;
            }

            foreach (var control in _frmSingleControls)
                control.SetMode(0);
            UIMessageTip.Show("已将所有DUT设置为休眠模式");
        }

        private void btnAllStandBy_Click(object sender, EventArgs e)
        {
            if (_frmSingleControls.Any(f => f.IsRunning))
            {
                this.ShowErrorDialog("请先停止所有DUT的测试");
                return;
            }

            foreach (var control in _frmSingleControls)
                control.SetMode(1);
            UIMessageTip.Show("已将所有DUT设置为动作模式");
        }

        private void btnAllCycle_Click(object sender, EventArgs e)
        {
            if (_frmSingleControls.Any(f => f.IsRunning))
            {
                this.ShowErrorDialog("请先停止所有DUT的测试");
                return;
            }

            foreach (var control in _frmSingleControls)
                control.SetMode(2);
            UIMessageTip.Show("已将所有DUT设置为循环模式");
        }

        private void btnAllStart_Click(object sender, EventArgs e)
        {
            if (_frmSingleControls.Any(f => f.IsRunning))
            {
                this.ShowErrorDialog("请先停止所有DUT的测试");
                return;
            }

            for (var i = 0; i < DutCount; i++)
                _frmSingleControls[i].Start();
            UIMessageTip.ShowOk("已全部启动");
        }

        private void btnAllStop_Click(object sender, EventArgs e)
        {
            foreach (var control in _frmSingleControls)
                control.Stop();
            UIMessageTip.ShowWarning("已全部停止");
        }

        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        private void btnOpenLogFolder_Click(object sender, EventArgs e)
        {
            var driveLetter = string.Format(@"{0}{1}", Path.GetPathRoot(Program.SysDir), "HVSM_实验箱");

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

        private void tsbtnDetectionPara_Click(object sender, EventArgs e)
        {
            var pwd = string.Empty;
            if (this.InputPasswordDialog(ref pwd))
            {
                if (pwd.ToLower() != "098765")
                {
                    this.ShowInfoTip("密码输入错误");
                    return;
                }
            }
            else
            {
                this.ShowInfoTip("取消密码输入");
                return;
            }

            var toUpdate = new HvsmEmcConfig.DeviceConfigPara[_hvsmConfig.Paras.Length];//_hvsmConfig.Paras.ToList();
            for (var i = 0; i < _hvsmConfig.Paras.Length; i++)
            {
                toUpdate[i] = new HvsmEmcConfig.DeviceConfigPara();
                toUpdate[i].Name = _hvsmConfig.Paras[i].Name;
                toUpdate[i].Min = _hvsmConfig.Paras[i].Min;
                toUpdate[i].Max = _hvsmConfig.Paras[i].Max;
            }

            using (var frm = new FrmParaSet(_configFile, toUpdate))
            {
                var result = frm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _hvsmConfig.Paras = frm._deviceConfigParas;
                    XmlHelper.SerializeToFile(_hvsmConfig, _configFile, Encoding.UTF8);
                    this.ShowSuccessTip("参数已更新");
                }
                else
                {
                    this.ShowInfoTip("操作取消，未更新参数");
                }
            }
        }

        private void tsbtnCyclePara_Click(object sender, EventArgs e)
        {
            if (_frmSingleControls.Any(f => f.IsRunning))
            {
                this.ShowErrorDialog("请先停止所有DUT的测试");
                return;
            }

            var toUpdateCyclePara = new HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[_hvsmConfig.CycleParas.Length][];
            for (int i = 0; i < _hvsmConfig.CycleParas.Length; i++)
            {
                toUpdateCyclePara[i] = new HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[_hvsmConfig.CycleParas[i].Length];
                for (int j = 0; j < _hvsmConfig.CycleParas[i].Length; j++)
                {
                    toUpdateCyclePara[i][j] = new HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara();
                    toUpdateCyclePara[i][j].Mode = _hvsmConfig.CycleParas[i][j].Mode;
                    toUpdateCyclePara[i][j].Mode = _hvsmConfig.CycleParas[i][j].Mode;
                    toUpdateCyclePara[i][j].Time = _hvsmConfig.CycleParas[i][j].Time;
                    toUpdateCyclePara[i][j].Hsd = new HvsmEmcConfig.DeviceConfigSingleCycleParaCycleParaHsd();
                    toUpdateCyclePara[i][j].Hsd.Duty = _hvsmConfig.CycleParas[i][j].Hsd.Duty;
                    toUpdateCyclePara[i][j].Hsd.Freq = _hvsmConfig.CycleParas[i][j].Hsd.Freq;
                    toUpdateCyclePara[i][j].Fan = new HvsmEmcConfig.DeviceConfigSingleCycleParaCycleParaFan();
                    toUpdateCyclePara[i][j].Fan.Duty = _hvsmConfig.CycleParas[i][j].Fan.Duty;
                }
            }

            using (var frm = new FrmCycleSet(toUpdateCyclePara))
            {
                var result = frm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _hvsmConfig.CycleParas = frm._cyclePara;
                    XmlHelper.SerializeToFile(_hvsmConfig, _configFile, Encoding.UTF8);
                    this.ShowSuccessTip("参数已更新");

                    for (int i = 0; i < DutCount; i++)
                        _frmSingleControls[i].SetCycle(_hvsmConfig.CycleParas[i]);
                }
                else
                {
                    this.ShowInfoTip("操作取消，未更新参数");
                }
            }
        }

        private void btnStandbyModeParas_Click(object sender, EventArgs e)
        {
            if (_frmSingleControls.Any(f => f.IsRunning))
            {
                this.ShowErrorDialog("请先停止所有DUT的测试");
                return;
            }

            var toUpdate = new HvsmEmcConfig.DeviceConfigStandbyModePara();//_hvsmConfig.StandbyModePara;
            toUpdate.Hsds = new HvsmEmcConfig.DeviceConfigStandbyModeParaHsd[_hvsmConfig.StandbyModePara.Hsds.Length];
            for (int i = 0; i < _hvsmConfig.StandbyModePara.Hsds.Length; i++)
            {
                toUpdate.Hsds[i] = new HvsmEmcConfig.DeviceConfigStandbyModeParaHsd();
                toUpdate.Hsds[i].Duty = _hvsmConfig.StandbyModePara.Hsds[i].Duty;
                toUpdate.Hsds[i].Freq = _hvsmConfig.StandbyModePara.Hsds[i].Freq;
            }
            toUpdate.Fans = new HvsmEmcConfig.DeviceConfigStandbyModeParaFan[_hvsmConfig.StandbyModePara.Fans.Length];
            for (int i = 0; i < _hvsmConfig.StandbyModePara.Fans.Length; i++)
            {
                toUpdate.Fans[i] = new HvsmEmcConfig.DeviceConfigStandbyModeParaFan();
                toUpdate.Fans[i].Duty = _hvsmConfig.StandbyModePara.Fans[i].Duty;
            }

            using (var frm = new FrmStandbyParas(toUpdate))
            {
                var result = frm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _hvsmConfig.StandbyModePara = frm._para;
                    XmlHelper.SerializeToFile(_hvsmConfig, _configFile, Encoding.UTF8);
                    this.ShowSuccessTip("参数已更新");

                    //for (int i = 0; i < DutCount; i++)
                    //    _frmSingleControls[i].SetCycle(_hvsmConfig.CycleParas);

                    for (int i = 0; i < DutCount; i++)
                        _frmSingleControls[i].SetStandByModePara
                            (_hvsmConfig.StandbyModePara.Hsds[0].Duty, _hvsmConfig.StandbyModePara.Hsds[0].Freq,
                            _hvsmConfig.StandbyModePara.Hsds[1].Duty, _hvsmConfig.StandbyModePara.Hsds[1].Freq,
                            _hvsmConfig.StandbyModePara.Hsds[2].Duty, _hvsmConfig.StandbyModePara.Hsds[2].Freq,
                            _hvsmConfig.StandbyModePara.Hsds[3].Duty, _hvsmConfig.StandbyModePara.Hsds[3].Freq,
                            _hvsmConfig.StandbyModePara.Fans[0].Duty,
                            _hvsmConfig.StandbyModePara.Fans[1].Duty);
                }
                else
                {
                    this.ShowInfoTip("操作取消，未更新参数");
                }
            }
        }

        private void tsbtTempMonitorPara_Click(object sender, EventArgs e)
        {
            //if (_frmSingleControls.Any(f => f.IsRunning))
            //{
            //    this.ShowErrorDialog("请先停止所有DUT的测试");
            //    return;
            //}

            var pwd = string.Empty;
            if (this.InputPasswordDialog(ref pwd))
            {
                if (pwd.ToLower() != "098765")
                {
                    this.ShowInfoTip("密码输入错误");
                    return;
                }
            }
            else
            {
                this.ShowInfoTip("取消密码输入");
                return;
            }

            var toUpdate = new HvsmEmcConfig.DeviceConfigDutOffsetHsdOffset[_hvsmConfig.OffsetPara.Length][];
            for (var i = 0; i < _hvsmConfig.OffsetPara.Length; i++)
            {
                toUpdate[i] = new HvsmEmcConfig.DeviceConfigDutOffsetHsdOffset[_hvsmConfig.OffsetPara[i].Length];
                for (var j = 0; j < _hvsmConfig.OffsetPara[i].Length; j++)
                {
                    toUpdate[i][j] = new HvsmEmcConfig.DeviceConfigDutOffsetHsdOffset();
                    toUpdate[i][j].K = _hvsmConfig.OffsetPara[i][j].K;
                    toUpdate[i][j].B = _hvsmConfig.OffsetPara[i][j].B;
                    toUpdate[i][j].IsEnable = _hvsmConfig.OffsetPara[i][j].IsEnable;
                    toUpdate[i][j].Threshold = _hvsmConfig.OffsetPara[i][j].Threshold;
                    toUpdate[i][j].ShowValue = _hvsmConfig.OffsetPara[i][j].ShowValue;
                }
            }

            using (var frm = new FrmOffset(toUpdate))
            {
                var result = frm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _hvsmConfig.OffsetPara = frm._paras;
                    XmlHelper.SerializeToFile(_hvsmConfig, _configFile, Encoding.UTF8);
                    this.ShowSuccessTip("参数已更新");
                }
                else
                {
                    this.ShowInfoTip("操作取消，未更新参数");
                }
            }
        }
    }
}
