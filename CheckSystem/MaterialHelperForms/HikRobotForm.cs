using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckSystem.MaterialHelperForms.MaterialModels;
using CommonUtility;
using Controller;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using HZH_Controls.IconFont;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.MaterialHelperForms
{
    public partial class HikRobotForm : UIForm
    {
        private readonly HikBarcodeScaner _hikBarcodeScaner = new HikBarcodeScaner("仓库电子料标签生成");
        private readonly SyControllerWith56Pin _controller = new SyControllerWith56Pin("控制器");
        private readonly object _lockHik = new object();

        //private readonly object _lockStockInScaners = new object();
        private bool _isScanExecuted;
        private bool _isGenerating;
        //private bool _isStockIng;
        private string _lastBoxBarcode = string.Empty;
        private string _lastStockInBarcode = string.Empty;

        public static string UserName;
        public static string UserCount;

        public HikRobotForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            InitializeDefaultUi();
            ReadSetup();
            Icon = FontImages.GetIcon(FontIcons.A_fa_camera, 32,
                Color.DodgerBlue);
            CheckForIllegalCrossThreadCalls = false;
            Closed += HikRobotForm_Closed;
            Load += HikRobotForm_Load;
        }

        public void HikRobotForm_Load(object sender, EventArgs e)
        {
            var loginWindow = new FormLogin((userName, password) =>
            {
                if (userName == @"6024")
                {
                    UserName = "6024";
                    UserCount = "6024";
                    return true;
                }

                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                    return false;

                var sql = "select F_Encode,F_Account,F_RealName,F_Password,F_SecretKey from lr_base_user where F_Encode = '" + userName + "'";

                var dt = HikDbHelperSql.GetDataTable(sql);
                if (dt.DefaultView.Count > 0)
                {
                    var encode = dt.DefaultView[0]["F_Encode"] == null ? string.Empty : dt.DefaultView[0]["F_Encode"].ToString();
                    var account = dt.DefaultView[0]["F_Account"] == null ? string.Empty : dt.DefaultView[0]["F_Account"].ToString();
                    //var pwd = dt.DefaultView[0]["F_Password"] == null ? string.Empty : dt.DefaultView[0]["F_Password"].ToString();
                    var key = dt.DefaultView[0]["F_SecretKey"] == null ? string.Empty : dt.DefaultView[0]["F_SecretKey"].ToString();

                    if (account == password)
                    {
                        UserName = dt.DefaultView[0]["F_RealName"] == null ? "无" : dt.DefaultView[0]["F_RealName"].ToString();
                        UserCount = account;
                        return true;
                    }

                    return false;
                }

                return false;
            });

            if (loginWindow.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show(@"登录失败");
                Close();
                return;
            }

            Text += string.Format("【当前登录用户名：{0}；工号：{1}】", UserName, UserCount);
            HikScanerClass.PushImage += HikScanerClass_PushImage;
            cmbStockInNo.SelectedIndexChanged += cmbStockInNocomboBox_SelectedIndexChanged;
            dgvStockInDetail.CellContentClick += dataGridViewStockInDetail_CellContentClick;

            BindData();
            //BindStockInDetals();
            LoadDevice();
        }

        private void HikRobotForm_Closed(object sender, EventArgs e)
        {
            if (_triggerThread != null)
            {
                _triggerThread.Abort();
                _triggerThread.Join();
            }

            if (_validationThead != null)
            {
                _validationThead.Abort();
                _validationThead.Join();
            }

            if (_alwaysTriggerThread != null)
            {
                _alwaysTriggerThread.Abort();
                _alwaysTriggerThread.Join();
            }

            if (_stockInThread != null)
            {
                _stockInThread.Abort();
                _stockInThread.Join();
            }

            if (_hikBarcodeScaner != null)
            {
                _hikBarcodeScaner.StopGrabbing();
                _hikBarcodeScaner.CloseScanner();
            }
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private readonly List<HikScanerClass.BarcodeStruct> _scanBuffer =
            new List<HikScanerClass.BarcodeStruct>();
        private readonly List<string> _expectedCodes = new List<string>();

        private void HikScanerClass_PushImage(
            Image img, string deviceSn, List<HikScanerClass.BarcodeStruct> barcodes)
        {
            if (!string.IsNullOrEmpty(deviceSn))
            {
                if (deviceSn == HikSetup.HiRobotMachineSn)
                {
                    if (barcodes.Any())
                    {
                        if (!_isScanExecuted) return;
                        if (_scanBuffer.Count > 9999)
                            _scanBuffer.Clear();

                        foreach (
                            var t in
                                barcodes.Where(t => _scanBuffer.FindIndex(f => f.Barcode == t.Barcode) == -1)
                                    .Where(t => PringLogs.FindIndex(f => f == t.Barcode) == -1))
                            _scanBuffer.Add(t);
                    }
                }
                else if (deviceSn == HikSetup.HiRobotMachineSn2 && !string.IsNullOrEmpty(textBox2.Text) && !_isTryingToSap)
                {
                    if (barcodes.Any())
                    {
                        if (_currentValidateStep == ValidateStep.PreStart ||
                            _currentValidateStep == ValidateStep.Validate ||
                                _currentValidateStep == ValidateStep.ValidateNg ||
                                _currentValidateStep == ValidateStep.ValidateOk)
                        {
                            var findSeeYaoMaterialBarcode =
                                barcodes.FindAll(
                                    f =>
                                        !string.IsNullOrEmpty(f.Barcode) &&
                                        (f.Barcode.StartsWith("ST") || f.Barcode.StartsWith("PR")));

                            var seeYaoMaterialBarcode = new List<string>();

                            if (findSeeYaoMaterialBarcode.Any())
                            {
                                foreach (var b in from b in findSeeYaoMaterialBarcode let countOfAt = b.Barcode.Count(t => t.ToString() == "@") where countOfAt == 16 select b)
                                {
                                    seeYaoMaterialBarcode.Add(b.Barcode);
                                    //seeYaoMaterialBarcode = b.Barcode;
                                    break;
                                }
                            }

                            if (seeYaoMaterialBarcode.Any())
                            {
                                if (seeYaoMaterialBarcode.Count == 1)
                                {
                                    Console.WriteLine(_currentValidateStep + ":" + barcodes.Count);

                                    if (_currentValidateStep == ValidateStep.PreStart)
                                    {
                                        _currentHistoryBarcodesList.Clear();
                                        _currentScanSeeYaoBarcode = seeYaoMaterialBarcode[0];
                                        _currentValidateStep = ValidateStep.Validate;

                                        if (_currentHistoryBarcodesList.Count > 9999)
                                            _currentHistoryBarcodesList.Clear();

                                        foreach (
                                            var t in
                                                barcodes.Where(t => _currentHistoryBarcodesList.FindIndex(f => f.Barcode == t.Barcode) == -1)
                                                    .Where(t => PringLogs.FindIndex(f => f == t.Barcode) == -1))
                                            _currentHistoryBarcodesList.Add(t);
                                    }
                                    else if (_currentValidateStep == ValidateStep.Validate)
                                    {
                                        //_currentHistoryBarcodesList.AddRange(barcodes);

                                        if (_currentHistoryBarcodesList.Count > 9999)
                                            _currentHistoryBarcodesList.Clear();

                                        foreach (
                                            var t in
                                                barcodes.Where(t => _currentHistoryBarcodesList.FindIndex(f => f.Barcode == t.Barcode) == -1)
                                                    .Where(t => PringLogs.FindIndex(f => f == t.Barcode) == -1))
                                            _currentHistoryBarcodesList.Add(t);
                                    }
                                    //else if (_currentValidateStep == ValidateStep.ValidateOk || _currentValidateStep == ValidateStep.ValidateNg)
                                    else if (_currentValidateStep == ValidateStep.ValidateOk)
                                    {
                                        if (_currentScanSeeYaoBarcode != seeYaoMaterialBarcode[0])
                                        {
                                            _currentHistoryBarcodesList.Clear();
                                            _currentScanSeeYaoBarcode = seeYaoMaterialBarcode[0];
                                            _currentValidateStep = ValidateStep.Validate;

                                            if (_currentHistoryBarcodesList.Count > 9999)
                                                _currentHistoryBarcodesList.Clear();

                                            foreach (
                                                var t in
                                                    barcodes.Where(t => _currentHistoryBarcodesList.FindIndex(f => f.Barcode == t.Barcode) == -1)
                                                        .Where(t => PringLogs.FindIndex(f => f == t.Barcode) == -1))
                                                _currentHistoryBarcodesList.Add(t);
                                        }
                                    }
                                    else if (_currentValidateStep == ValidateStep.ValidateNg)
                                    {
                                        _currentHistoryBarcodesList.Clear();
                                        _currentScanSeeYaoBarcode = seeYaoMaterialBarcode[0];
                                        _currentValidateStep = ValidateStep.Validate;

                                        if (_currentHistoryBarcodesList.Count > 9999)
                                            _currentHistoryBarcodesList.Clear();

                                        foreach (
                                            var t in
                                                barcodes.Where(t => _currentHistoryBarcodesList.FindIndex(f => f.Barcode == t.Barcode) == -1)
                                                    .Where(t => PringLogs.FindIndex(f => f == t.Barcode) == -1))
                                            _currentHistoryBarcodesList.Add(t);
                                    }
                                }
                                else
                                {
                                    _currentHistoryBarcodesList.Clear();
                                    _currentScanSeeYaoBarcode = string.Empty;
                                    _currentValidateStep = ValidateStep.PreStart;

                                    UpdatePanelDataValidation(UIStyle.Blue, @"入库校验:");
                                    tsLblState.Text = string.Format("{0}: 读取到多个打印的二维码", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); ;
                                }
                            }
                        }
                    }
                }
            }
        }

        #region 界面初始化、设备初始化、参数读取

        private void InitializeDefaultUi()
        {
            tsLblState.Text = string.Empty;
            //dgvValidation.Style = UIStyle.Gray;
            //dgvValidation.ReadOnly = true;
            ////dgvValidation.Enabled = false;
            //dgvValidation.RowHeadersVisible = false;
            //dgvValidation.AllowUserToAddRows = false;
            //dgvValidation.AllowUserToResizeRows = false;
            //dgvValidation.AllowUserToDeleteRows = false;
            //dgvValidation.MultiSelect = false;
            //dgvValidation.RowHeadersVisible = false;
            //dgvValidation.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //dgvValidation.AddColumn("条码内容", "条码内容");
            //dgvValidation.AddColumn("是否扫描到", "是否扫描到");

            //获取控件的Type,设置双缓存
            var dgvType = pictureBox1.GetType();
            var properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //为控件的属性设置值
            if (properInfo != null) properInfo.SetValue(pictureBox1, true, null);

            //cmbIssporadic.comboBox.Items.Add("整盘入库Q");
            //cmbIssporadic.comboBox.Items.Add("零星入库L");
            //cmbIssporadic.comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            //cmbIssporadic.comboBox.SelectedIndex = 0;

            //cmbStockInType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStockInType.Items.Add("正常入库");
            cmbStockInType.Items.Add("待下发订单换箱");
            cmbStockInType.Items.Add("SAP补单");
            cmbStockInType.SelectedIndex = 0;

            cmbDetailSearchType.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDetailSearchType.comboBox.Items.Add("查找最近50条记录");
            cmbDetailSearchType.comboBox.Items.Add("查找最近100条记录");
            cmbDetailSearchType.comboBox.Items.Add("查找最近200条记录");
            cmbDetailSearchType.comboBox.Items.Add("按物料号查找");
            cmbDetailSearchType.comboBox.Items.Add("按生产批次查找");
            cmbDetailSearchType.comboBox.SelectedIndex = 0;
            cmbDetailSearchType.comboBox.SelectedIndexChanged += cmbDetailSearchcomboBox_SelectedIndexChanged;
            txtSearchMaterialNo.Visible = false;
            dateStartSearch.Visible = false;
            dateEndSearch.Visible = false;

            dgvStockInDetail.Style = UIStyle.Gray;
            dgvStockInDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStockInDetail.RowTemplate.Height = 50;
            dgvStockInDetail.RowsDefaultCellStyle.Font = new Font("微软雅黑", 9f, FontStyle.Regular);
            dgvStockInDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgvStockInDetail.AllowUserToResizeColumns = true;
            dgvStockInDetail.AllowUserToResizeRows = false;
            dgvStockInDetail.AllowUserToAddRows = false;
            dgvStockInDetail.AllowUserToDeleteRows = false;
            dgvStockInDetail.ReadOnly = true;
            dgvStockInDetail.RowHeadersVisible = false;
            dgvStockInDetail.MultiSelect = false;
            dgvStockInDetail.AllowUserToOrderColumns = false;

            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "id" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "入库单号" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "物料号" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "产品型号" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "供应商代码" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "LOT_NO" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "Count" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "SupplyLedGroup" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "是否整盘入库" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "质量等级" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "特定用途" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "状态" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "托盘" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo" });
            dgvStockInDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "Other" });
            dgvStockInDetail.Columns.Add(new DataGridViewButtonColumn { Name = "标签补打", DefaultCellStyle = { NullValue = "标签补打" } });

            //dgvStockInDetail.Columns[0].Width = 1;
            //dgvStockInDetail.Columns[1].Width = 50;
            //dgvStockInDetail.Columns[2].Width = 45;
            //dgvStockInDetail.Columns[3].Width = 120;
            //dgvStockInDetail.Columns[4].Width = 25;
            ////dgvStockInDetail.Columns[5].Width = 60;
            //dgvStockInDetail.Columns[6].Width = 10;
            //dgvStockInDetail.Columns[7].Width = 50;
            //dgvStockInDetail.Columns[11].Width = 10;
            //dgvStockInDetail.Columns[12].Width = 40;
            //dgvStockInDetail.Columns[13].Width = 10;
            //dgvStockInDetail.Columns[14].Width = 10;
            //dgvStockInDetail.Columns[15].Width = 10;

            dgvGeneratedResult.Style = UIStyle.Gray;
            dgvGeneratedResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGeneratedResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGeneratedResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvGeneratedResult.AllowUserToResizeColumns = true;
            dgvGeneratedResult.AllowUserToResizeRows = false;
            dgvGeneratedResult.AllowUserToAddRows = false;
            dgvGeneratedResult.AllowUserToDeleteRows = false;
            dgvGeneratedResult.ReadOnly = true;
            dgvGeneratedResult.RowHeadersVisible = false;
            dgvGeneratedResult.MultiSelect = true;
            dgvGeneratedResult.AllowUserToOrderColumns = false;

            dgvGeneratedResult.AddColumn("@分隔位", "@分隔位", 30);
            dgvGeneratedResult.AddColumn("内容", "内容");
            dgvGeneratedResult.AddColumn("释义", "释义", 50);

            //dgvGeneratedResult.Columns.Add(new DataGridViewTextBoxColumn { Name = "@分隔位" });
            //dgvGeneratedResult.Columns.Add(new DataGridViewTextBoxColumn { Name = "内容" });
            //dgvGeneratedResult.Columns.Add(new DataGridViewTextBoxColumn { Name = "释义" });

            //dgvGeneratedResult.Columns[0].Width = 5;
            //dgvGeneratedResult.Columns[2].Width = 10;

            for (var i = 0; i < 14; i++)
            {
                var newRowIndex = dgvGeneratedResult.Rows.Add();
                var newRow = dgvGeneratedResult.Rows[newRowIndex];
                newRow.Cells[0].Value = i.ToString();
                newRow.Cells[1].Value = string.Empty;
            }
            dgvGeneratedResult.Rows[0].Cells[2].Value = @"物料号";
            dgvGeneratedResult.Rows[1].Cells[2].Value = @"入库日期";
            dgvGeneratedResult.Rows[2].Cells[2].Value = @"自增序列号";
            dgvGeneratedResult.Rows[3].Cells[2].Value = @"供应商代码";
            dgvGeneratedResult.Rows[4].Cells[2].Value = @"包装规格";
            dgvGeneratedResult.Rows[5].Cells[2].Value = @"入库单号\订单号";
            dgvGeneratedResult.Rows[6].Cells[2].Value = @"特征值*";
            dgvGeneratedResult.Rows[7].Cells[2].Value = @"特征值编号*";
            dgvGeneratedResult.Rows[8].Cells[2].Value = @"产品型号";
            dgvGeneratedResult.Rows[9].Cells[2].Value = @"生产批次";
            dgvGeneratedResult.Rows[10].Cells[2].Value = @"到期日期*";
            dgvGeneratedResult.Rows[11].Cells[2].Value = @"质量等级*";
            dgvGeneratedResult.Rows[12].Cells[2].Value = @"是否零星入库*";
            dgvGeneratedResult.Rows[13].Cells[2].Value = @"特定用途/重要等级*";

            dgvGeneratedResult.SetRowHeight(5);
        }

        private static void ReadSetup()
        {
            HikSetup.ReadSetup();
        }

        private async void LoadDevice()
        {
            //return;
            await Task.Run(() =>
            {
                #region 连接海康读码设备
                if (!HikScanerClass.HikScanerDevices.Any())
                {
                    lblStatus.Text += @"没有检测到摄像头连接;";
                    topTablePanel.Enabled = false;
                    toolStripButton5.Enabled = false;

                    //topTablePanel.Enabled = true;
                    //toolStripButton5.Enabled = true;
                }
                else
                {
                    if (HikScanerClass.HikScanerDevices.FindIndex(f => f.DeviceSn == HikSetup.HiRobotMachineSn) == -1)
                    {
                        lblStatus.Text += @"没有检测系统配置的摄像头序列号：" + HikSetup.HiRobotMachineSn + @"，请重新配置正确的序列号;";
                        topTablePanel.Enabled = false;
                        toolStripButton5.Enabled = false;

                        //topTablePanel.Enabled = true;
                        //toolStripButton5.Enabled = true;
                    }
                    else if (HikScanerClass.HikScanerDevices.FindIndex(f => f.DeviceSn == HikSetup.HiRobotMachineSn2) == -1)
                    {
                        lblStatus.Text += @"没有检测系统配置的摄像头2序列号：" + HikSetup.HiRobotMachineSn + @"，请重新配置正确的序列号;";
                        topTablePanel.Enabled = false;
                        toolStripButton5.Enabled = false;

                        //topTablePanel.Enabled = true;
                        //toolStripButton5.Enabled = true;
                    }
                    else
                    {
                        _controller.InitRemoteIpAddress(HikSetup.TriggerControllerIpPort);

                        if (_controller.IsConnected)
                        {
                            toolStripButton5.Enabled = false;

                            if (_hikBarcodeScaner.OpenScanner() && _hikBarcodeScaner.StartGrabbing())
                            {
                                var sn2Index =
                                    HikScanerClass.HikScanerDevices.FindIndex(
                                        f => f.DeviceSn == HikSetup.HiRobotMachineSn2);
                                if (sn2Index != -1)
                                {
                                    HikScanerClass.HikScanerDevices[sn2Index].IsShowImg = true;
                                }

                                //_hikBarcodeScaner.OpenScanner();
                                //_hikBarcodeScaner.StartGrabbing();
                                toolStripButton5.Text = toolStripButton5 + @"(" + HikSetup.HiRobotMachineSn + @")";
                                toolStripButton5.Enabled = true;

                                //ResetHikScanResultUi();

                                if (_triggerThread != null)
                                {
                                    _triggerThread.Abort();
                                    _triggerThread.Join();
                                }
                                _triggerThread = new Thread(TriggerMainWork) { IsBackground = true };
                                _triggerThread.Start();

                                if (_validationThead != null)
                                {
                                    _validationThead.Abort();
                                    _validationThead.Join();
                                }
                                _validationThead = new Thread(ValidationWork) { IsBackground = true };
                                _validationThead.Start();
                            }
                            else
                            {
                                lblStatus.Text += @"打开摄像头失败;";
                                topTablePanel.Enabled = false;
                                toolStripButton5.Enabled = false;
                            }
                        }
                        else
                        {
                            lblStatus.Text += @"无法连接控制器，IP:" + HikSetup.TriggerControllerIpPort + "";
                            topTablePanel.Enabled = false;
                            toolStripButton5.Enabled = false;
                        }
                    }
                }
                #endregion

                #region 连接打印机和入库读码设备
                try
                {
                    var toSapPort = HikSetup.Setup.IniReadValue("SerialPort", "ToSapPort");
                    if (toSapPort.StartsWith("COM"))
                    {
                        HikSetup.ToSapPort = new MySerialPort(toSapPort.Split(':')[0],
                            int.Parse(toSapPort.Split(':')[1]), Parity.None, 8, StopBits.One);
                        HikSetup.ToSapPort.MyOpen();
                    }
                    else
                    {
                        HikSetup.ToSapPort = new MySerialPort(toSapPort.Split(':')[0], int.Parse(toSapPort.Split(':')[1]));
                    }

                    {
                        int ms;
                        if (int.TryParse(HikSetup.Setup.IniReadValue("SerialPort", "ToSapPortDelayMs"), out ms))
                        {
                            HikSetup.ToSapDelay = ms;
                        }
                    }

                    var printPort = HikSetup.Setup.IniReadValue("SerialPort", "FeedInMaterialBarcodePrinterPort");
                    if (printPort.StartsWith("COM"))
                    {
                        HikSetup.FeedInMaterialBarcodePrinter = new MySerialPort(printPort.Split(':')[0],
                            int.Parse(printPort.Split(':')[1]), Parity.None, 8, StopBits.One);
                        HikSetup.FeedInMaterialBarcodePrinter.MyOpen();
                    }
                    else
                    {
                        HikSetup.FeedInMaterialBarcodePrinter = new MySerialPort(printPort.Split(':')[0], int.Parse(printPort.Split(':')[1]));
                    }

                    //lock (_lockStockInScaners)
                    {
                        int ms;
                        if (int.TryParse(HikSetup.Setup.IniReadValue("SerialPort", "ScanDelayMs"), out ms))
                        {
                            if (ms <= 0 || ms > 5000)
                            {
                                HikSetup.FeedInScanDelayMs = 1000;
                            }
                            else
                            {
                                HikSetup.FeedInScanDelayMs = ms;
                            }
                        }
                        else
                        {
                            HikSetup.FeedInScanDelayMs = 1000;
                        }

                        var feedInMaterialScanerPort = HikSetup.Setup.IniReadValue("SerialPort", "FeedInMaterialPort");
                        if (feedInMaterialScanerPort.StartsWith("COM"))
                        {
                            HikSetup.FeedInMaterialScaner = new MySerialPort(feedInMaterialScanerPort.Split(':')[0],
                                int.Parse(feedInMaterialScanerPort.Split(':')[1]), Parity.None, 8, StopBits.One);
                            HikSetup.FeedInMaterialScaner.MyOpen();
                        }
                        else
                        {
                            HikSetup.FeedInMaterialScaner = new MySerialPort(feedInMaterialScanerPort.Split(':')[0], int.Parse(feedInMaterialScanerPort.Split(':')[1]));
                        }

                        var feedInBoxScannerPort = HikSetup.Setup.IniReadValue("SerialPort", "FeedInBoxScannerPort");
                        HikSetup.IsFeedInBoxScanerTriggerMode =
                            HikSetup.Setup.IniReadValue("SerialPort", "IsFeedInBoxScannerTriggerMode") == 1.ToString();
                        if (feedInBoxScannerPort.StartsWith("COM"))
                        {
                            HikSetup.FeedInBoxScanner = new MySerialPort(feedInBoxScannerPort.Split(':')[0],
                                int.Parse(feedInBoxScannerPort.Split(':')[1]), Parity.None, 8, StopBits.One);
                            HikSetup.FeedInBoxScanner.MyOpen();
                        }
                        else
                        {
                            HikSetup.FeedInBoxScanner = new MySerialPort(feedInBoxScannerPort.Split(':')[0], int.Parse(feedInBoxScannerPort.Split(':')[1]));
                        }

                        btnStockInStatus1.TitleColor = Color.DarkGoldenrod;
                        btnStockInStatus2.TitleColor = Color.DarkGoldenrod;
                    }

                    if (_alwaysTriggerThread != null)
                    {
                        _alwaysTriggerThread.Abort();
                        _alwaysTriggerThread.Join();
                    }
                    _alwaysTriggerThread = new Thread(AlwaysTriggerWork) { IsBackground = true };
                    _alwaysTriggerThread.Start();

                    if (_stockInThread != null)
                    {
                        _stockInThread.Abort();
                        _stockInThread.Join();
                    }
                    _stockInThread = new Thread(StoctInMainWork) { IsBackground = true };
                    _stockInThread.Start();
                }
                catch (Exception)
                {
                    // ignored
                }
                #endregion
            });
        }

        #endregion

        #region 主线程1，触发海康扫码

        private Thread _triggerThread;

        /// <summary>
        /// 主线程1，触发海康扫码
        /// </summary>
        private void TriggerMainWork()
        {
            //var resetCount = 0;
            _controller.StartAutoRefresh();

            while (_triggerThread.IsAlive)
            {
                if (!_triggerThread.IsAlive)
                    break;

                Thread.Sleep(50);

                try
                {
                    if (!btnConfirm.Enabled && btnStockInStatus2.TitleColor == Color.DarkGreen &&
                        !string.IsNullOrEmpty(textBox2.Text) && HikSetup.ToSapPort != null)
                    {
                        var trayNo = textBox2.Text;
                        //_controller.GetCurrentVoltageDi();

                        if (_controller.Di1 == 1.ToString() && !_isGenerating)
                        {
                            var st = new Stopwatch();
                            st.Start();

                            _isGenerating = true;
                            ResetHikScanResultUi();

                            _scanBuffer.Clear();
                            _isScanExecuted = true;

                            UpdateRichGeneratedBarcodeTextBox(Color.Gray, "正在读码");

                            while (true)
                            {
                                if (st.ElapsedMilliseconds < HikSetup.TriggerDelayMs)
                                    continue;

                                _isScanExecuted = false;

                                bnTriggerExec_Click(_scanBuffer, trayNo);
                                _scanBuffer.Clear();
                                //resetCount = 0;
                                break;
                            }

                            while (true)
                            {
                                //_controller.GetCurrentVoltageDi();
                                if (_controller.Di1 == 0.ToString())
                                    break;
                            }

                            st.Stop();

                            UpdateRichGeneratedBarcodeTextBox(Color.Gray,
                                @"耗时：" + st.ElapsedMilliseconds + @"ms", true);
                            _isGenerating = false;
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private Thread _alwaysTriggerThread;

        private void AlwaysTriggerWork()
        {
            while (_alwaysTriggerThread.IsAlive)
            {
                if (!_alwaysTriggerThread.IsAlive)
                    break;

                Thread.Sleep(50);

                HikSetup.FeedInMaterialScaner.ReadDataStr();
                HikSetup.FeedInMaterialScaner.ClearBuff();
                HikSetup.FeedInMaterialScaner.SendCommand("T\r\n");
            }
        }

        #endregion

        #region 主线程2，右下方触发入库扫码,扫描箱体、补单、换箱

        private Thread _stockInThread;

        /// <summary>
        /// 主线程2，右下方触发入库扫码,扫描箱体、补单、换箱
        /// </summary>
        private void StoctInMainWork()
        {
            while (_stockInThread.IsAlive)
            {
                if (!_stockInThread.IsAlive)
                    break;

                Thread.Sleep(50);

                try
                {
                    if (btnStockInStatus2.TitleColor == Color.DarkGoldenrod) // 需要扫描箱体
                    {
                        var result = string.Empty;
                        if (HikSetup.IsFeedInBoxScanerTriggerMode)
                        {
                            HikSetup.FeedInBoxScanner.ReadDataStr();
                            HikSetup.FeedInBoxScanner.ClearBuff();
                            HikSetup.FeedInBoxScanner.SendCommand("T\r\n");
                            Thread.Sleep(HikSetup.FeedInScanDelayMs);

                            var resultList = HikSetup.FeedInBoxScanner.ReadDataStr().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            resultList.RemoveAll(f => f.Contains("NOREAD"));
                            if (resultList.Any())
                                result = resultList[resultList.Count - 1].TrimEnd();
                        }
                        else
                        {
                            while (true)
                            {
                                HikSetup.FeedInBoxScanner.SendCommand("\r\n");
                                var resultList = HikSetup.FeedInBoxScanner.ReadDataStr().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                if (resultList.Any())
                                    result += resultList[resultList.Count - 1].TrimEnd();

                                if (result.Length >= 11)
                                    break;
                            }
                        }

                        if (!string.IsNullOrEmpty(result) && (result.StartsWith("B") ||
                            result.StartsWith("T") && result.Length == 11) && result !=
                            _lastBoxBarcode)
                        {
                            textBox2.Text = result;
                            btnStockInStatus2.TitleColor = Color.DarkGreen;
                            BindingBoxMaterialCount();
                        }
                    }
                    else // 箱体已经扫码，再扫描的应该是物料
                    {
                        var result = string.Empty;
                        if (HikSetup.IsFeedInBoxScanerTriggerMode)
                        {
                            HikSetup.FeedInBoxScanner.ReadDataStr();
                            HikSetup.FeedInBoxScanner.ClearBuff();
                            HikSetup.FeedInBoxScanner.SendCommand("T\r\n");
                            Thread.Sleep(HikSetup.FeedInScanDelayMs);

                            var resultList = HikSetup.FeedInBoxScanner.ReadDataStr().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            resultList.RemoveAll(f => f.Contains("NOREAD"));
                            if (resultList.Any())
                                result = resultList[resultList.Count - 1].TrimEnd();
                        }
                        else
                        {
                            while (true)
                            {
                                var resultList = HikSetup.FeedInBoxScanner.ReadDataStr().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                if (resultList.Any())
                                    result += resultList[resultList.Count - 1].TrimEnd();

                                if (result.EndsWith("@@") && btnStockInStatus2.TitleColor == Color.DarkGreen)
                                    break;

                                if (btnStockInStatus2.TitleColor != Color.DarkGreen)
                                {
                                    result = string.Empty;
                                    break;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(result) &&
                            //result != _lastStockInBarcode &&
                            result.EndsWith("@@") && btnStockInStatus2.TitleColor == Color.DarkGreen &&
                            !string.IsNullOrEmpty(textBox2.Text) && !_isGenerating)
                        {
                            _isGenerating = true;

                            textBox1.Text = result;
                            btnStockInStatus1.TitleColor = Color.DarkGreen;
                            btnStockInStatus3.BackColor = Color.DarkGray;

                            _lastStockInBarcode = result;

                            var trayNo = textBox2.Text;
                            var materialPrintNo = textBox1.Text;

                            if (cmbStockInType.SelectedIndex == 1) // 换箱
                            {
                                btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + "：" + materialPrintNo.Split('@')[2] + "，" + "正在换箱";
                                Thread.Sleep(500);

                                var toChangeModel = HikDbHelperSql.GetNewMaterialStockInDetail(" Status = '待下发' and  MaterialPrintNo = '" + materialPrintNo + "' and TrayNo != '" + trayNo + "'");

                                if (toChangeModel.Any())
                                {
                                    #region 判断当前箱体同一物料号的批次，批次不在同一月不能入库

                                    var toInMaterialNo = toChangeModel[0].MaterialNo;
                                    var toInDcLotNo = toChangeModel[0].DclotNo;

                                    var getMaterialInThisTrayNo =
                                        "select distinct MaterialNo from newMaterialStockInDetail  WHERE IsDelete = '0' AND TrayNo = '" + trayNo + "'"; // 取得当前箱体内所有物料号
                                    var materials = HikDbHelperSql.GetData(getMaterialInThisTrayNo); // 取得当前箱体内所有物料号
                                    if (materials.Contains(toInMaterialNo)) // 取得当前箱体内有这个物料号
                                    {
                                        var getModels =
                                            HikDbHelperSql.GetNewMaterialStockInDetail(
                                                string.Format("IsDelete = '0' AND TrayNo = '" + trayNo + "' and MaterialNo = '" + toInMaterialNo + "'")); // 取得当前箱体内的这个物料号的已入批次

                                        if (getModels.Any())
                                        {
                                            var dcLotNo = getModels[0].DclotNo;
                                            if (dcLotNo.Substring(0, 6) != toInDcLotNo.Substring(0, 6))
                                            {
                                                btnStockInStatus3.BackColor = Color.DarkRed;
                                                btnStockInStatus3.Text = DateTime.Now.ToString() + "：" + materialPrintNo.Split('@')[2] + "，" + @"换箱失败," + string.Format("物料号={0}，箱体={1}，已入箱批次:{2}，当前入箱批次{3}，批次不同无法入同一箱体。", toInMaterialNo, trayNo, dcLotNo, toInDcLotNo);
                                            }
                                            else
                                            {
                                                var sql = "update newMaterialStockInDetail SET TrayNo = '" + trayNo + "',Status = '待下发' WHERE Status = '待下发' and  MaterialPrintNo = '" + materialPrintNo + "' and TrayNo != '" + trayNo + "'";

                                                var update = HikDbHelperSql.Update(sql);

                                                if (update > 0)
                                                {
                                                    btnStockInStatus3.Text = DateTime.Now.ToString() + "：" + materialPrintNo.Split('@')[2] + "，" + @"换箱成功";
                                                    btnStockInStatus3.BackColor = Color.DarkGreen;
                                                }
                                                else
                                                {
                                                    btnStockInStatus3.BackColor = Color.DarkRed;
                                                    btnStockInStatus3.Text = DateTime.Now.ToString() + "：" + materialPrintNo.Split('@')[2] + "，" + @"换箱失败";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            btnStockInStatus3.BackColor = Color.DarkRed;
                                            btnStockInStatus3.Text = DateTime.Now.ToString() + "：" + materialPrintNo.Split('@')[2] + "，" + @"换箱失败";
                                        }
                                    }
                                    else // 取得当前箱体内没有这个物料号，直接换箱
                                    {
                                        var sql = "update newMaterialStockInDetail SET TrayNo = '" + trayNo + "',Status = '待下发' WHERE Status = '待下发' and  MaterialPrintNo = '" + materialPrintNo + "' and TrayNo != '" + trayNo + "'";

                                        var update = HikDbHelperSql.Update(sql);

                                        if (update > 0)
                                        {
                                            btnStockInStatus3.Text = DateTime.Now.ToString() + "：" + materialPrintNo.Split('@')[2] + "，" + @"换箱成功";
                                            btnStockInStatus3.BackColor = Color.DarkGreen;
                                        }
                                        else
                                        {
                                            btnStockInStatus3.BackColor = Color.DarkRed;
                                            btnStockInStatus3.Text = DateTime.Now.ToString() + "：" + materialPrintNo.Split('@')[2] + "，" + @"换箱失败";
                                        }
                                    }

                                    #endregion
                                }
                                else
                                {
                                    btnStockInStatus3.BackColor = Color.DarkRed;
                                    btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + @"：" + materialPrintNo.Split('@')[2] + @"，" + @"换箱失败";
                                }
                            }
                            else if (cmbStockInType.SelectedIndex == 0) // 待下发，继续入库
                            {
                                btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + @"：" + materialPrintNo.Split('@')[2] + @"，" + @"正在入库";
                                Thread.Sleep(500);

                                var toChangeModel = HikDbHelperSql.GetNewMaterialStockInDetail(" Status = '待入库' and IsDelete = '0' and  MaterialPrintNo = '" + materialPrintNo + "'");

                                if (toChangeModel.Any())
                                {
                                    #region 判断当前箱体同一物料号的批次，批次不在同一月不能入库

                                    var toInMaterialNo = toChangeModel[0].MaterialNo;
                                    var toInDcLotNo = toChangeModel[0].DclotNo;

                                    var getMaterialInThisTrayNo =
                                        "select distinct MaterialNo from newMaterialStockInDetail  WHERE IsDelete = '0' AND TrayNo = '" + trayNo + "'";
                                    var materials = HikDbHelperSql.GetData(getMaterialInThisTrayNo);
                                    if (materials.Contains(toInMaterialNo))
                                    {
                                        var getModels =
                                            HikDbHelperSql.GetNewMaterialStockInDetail(
                                                string.Format("IsDelete = '0' AND TrayNo = '" + trayNo + "' and MaterialNo = '" + toInMaterialNo + "'"));

                                        if (getModels.Any())
                                        {
                                            var dcLotNo = getModels[0].DclotNo;
                                            if (dcLotNo.Substring(0, 6) != toInDcLotNo.Substring(0, 6))
                                            {
                                                btnStockInStatus3.BackColor = Color.DarkRed;
                                                btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + @"：" + materialPrintNo.Split('@')[2] + @"，" + @"入库失败," + string.Format("物料号={0}，箱体={1}，已入箱批次:{2}，当前入箱批次{3}，批次不同无法入同一箱体。", toInMaterialNo, trayNo, dcLotNo, toInDcLotNo);
                                            }
                                            else
                                            {
                                                var toSapError = string.Empty;
                                                if (TryToSap(materialPrintNo, trayNo, ref toSapError))
                                                {
                                                    btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + @"：" + materialPrintNo.Split('@')[2] + @"，" + @"入库成功";
                                                    btnStockInStatus3.BackColor = Color.DarkGreen;
                                                }
                                                else
                                                {
                                                    btnStockInStatus3.BackColor = Color.DarkRed;
                                                    btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + @"：" + materialPrintNo.Split('@')[2] + @"，" + toSapError;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            btnStockInStatus3.BackColor = Color.DarkRed;
                                            btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + @"：" + materialPrintNo.Split('@')[2] + @"，" + @"入库失败";
                                        }
                                    }
                                    else
                                    {
                                        var toSapError = string.Empty;
                                        if (TryToSap(materialPrintNo, trayNo, ref toSapError))
                                        {
                                            btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + @"：" + materialPrintNo.Split('@')[2] + @"，" + @"入库成功";
                                            btnStockInStatus3.BackColor = Color.DarkGreen;
                                        }
                                        else
                                        {
                                            btnStockInStatus3.BackColor = Color.DarkRed;
                                            btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + @"：" + materialPrintNo.Split('@')[2] + @"，" + toSapError;
                                        }
                                    }

                                    #endregion
                                }
                                else
                                {
                                    btnStockInStatus3.BackColor = Color.DarkRed;
                                    btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + @"：" + materialPrintNo.Split('@')[2] + @"，" + @"入库失败，查无此数据"; ;
                                }
                            }
                            else // SAP补单
                            {
                                btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + @"：" + materialPrintNo.Split('@')[2] + @"，" + @"SAP补单";
                                Thread.Sleep(500);

                                var toChangeModel = HikDbHelperSql.GetNewMaterialStockInDetail(" Status =  '待下发' and IsDelete = '0' and  MaterialPrintNo = '" + materialPrintNo + "' and Other = '入SAP' and TrayNo = '" + trayNo + "'");

                                if (toChangeModel.Any())
                                {
                                    if (HikSetup.ToSapPort != null)
                                    {
                                        var toSapCmd = toChangeModel[0].Other2;
                                        HikSetup.ToSapPort.ReadDataStr();
                                        HikSetup.ToSapPort.ClearBuff();
                                        HikSetup.ToSapPort.SendCommand(toSapCmd);

                                        btnStockInStatus3.BackColor = Color.DarkGreen;
                                        btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + @"：" + materialPrintNo.Split('@')[2] + @"，" + @"SAP已补单，请确认数据"; ;
                                    }
                                }
                                else
                                {
                                    btnStockInStatus3.BackColor = Color.DarkRed;
                                    btnStockInStatus3.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture) + @"：" + materialPrintNo.Split('@')[2] + @"，" + @"SAP补单失败，查无此数据"; ;
                                }
                            }

                            BindingBoxMaterialCount();
                            _isGenerating = false;
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void btnReStockInScan_Click(object sender, EventArgs e)
        {
            //lock (_lockStockInScaners)
            {
                if (btnStockInStatus2.TitleColor == Color.DarkGreen && !_isGenerating && !_isTryingToSap && _currentValidateStep != ValidateStep.Validate)
                {
                    _lastBoxBarcode = textBox2.Text;

                    btnStockInStatus1.TitleColor = Color.DarkGoldenrod;
                    btnStockInStatus2.TitleColor = Color.DarkGoldenrod;
                    btnStockInStatus3.BackColor = Color.DarkGray;

                    btnStockInStatus3.Text = @"请扫箱体";
                    UpdateCountTxt(txtBoxMaterialCount.Name, string.Empty);

                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;

                    if (HikSetup.FeedInBoxScanner != null)
                        HikSetup.FeedInBoxScanner.MyOpen();
                }
            }
        }

        private void BindingBoxMaterialCount()
        {
            // txtBoxMaterialCount

            var getModels =
                HikDbHelperSql.GetNewMaterialStockInDetail(string.Format("IsDelete = '0' and TrayNo = '{0}'",
                    textBox2.Text));
            UpdateCountTxt(txtBoxMaterialCount.Name, getModels.Count.ToString());
        }

        #endregion

        #region 主线程3，左上方，入库校验

        internal enum ValidateStep
        {
            PreStart,

            Validate,

            Validating,

            ValidateNg,

            ValidateOk
        }

        private Thread _validationThead;
        private ValidateStep _currentValidateStep = ValidateStep.PreStart;
        private string _currentScanSeeYaoBarcode = string.Empty;
        private readonly List<HikScanerClass.BarcodeStruct> _currentHistoryBarcodesList = new List<HikScanerClass.BarcodeStruct>();
        private bool _isTryingToSap;

        private void ValidationWork()
        {
            while (_validationThead.IsAlive)
            {
                if (!_validationThead.IsAlive)
                    break;

                Thread.Sleep(20);

                if (_currentValidateStep == ValidateStep.Validate)
                {
                    UpdatePanelDataValidation(UIStyle.Blue, "入库校验:\r\n" + _currentScanSeeYaoBarcode);

                    tsLblState.Text = string.Format("{0}: 正在读码", DateTime.Now.ToString("HH:mm:ss"));

                    var st = new Stopwatch();
                    st.Start();

                    while (true)
                    {
                        if (st.ElapsedMilliseconds < HikSetup.TriggerDelayMs)
                            continue;
                        break;
                    }

                    st.Stop();
                    _currentValidateStep = ValidateStep.Validating;

                    var historyModel = HikDbHelperSql.GetNewMaterialStockInDetail(string.Format(
                        "IsDelete = '0' and MaterialPrintNo = '{0}'", _currentScanSeeYaoBarcode));

                    if (historyModel.Any())
                    {
                        if (historyModel.Count == 1)
                        {
                            if (historyModel[0].Status == "待入库")
                            {
                                if (bnTriggerExec2_Click(_currentHistoryBarcodesList, _currentScanSeeYaoBarcode, historyModel[0]))
                                {
                                    if (!string.IsNullOrEmpty(textBox2.Text))
                                    {
                                        tsLblState.Text = DateTime.Now.ToString("HH:mm:ss") + @"：" +
                                                             @"正在入SAP";
                                        _isTryingToSap = true;

                                        var toSapError = string.Empty;
                                        var trayNo = textBox2.Text;
                                        var materialPrintNo = _currentScanSeeYaoBarcode;
                                        if (TryToSap(materialPrintNo, trayNo, ref toSapError))
                                        {
                                            BindingBoxMaterialCount();
                                            _currentValidateStep = ValidateStep.ValidateOk;
                                            tsLblState.Text = string.Format("{0}: {1}。",
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "入SAP成功");
                                        }
                                        else
                                        {
                                            _currentValidateStep = ValidateStep.ValidateNg;
                                            tsLblState.Text = string.Format("{0}: 二维码={1}，{2}。",
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), materialPrintNo, toSapError);
                                        }

                                        _isTryingToSap = false;
                                    }
                                    else
                                    {
                                        _currentValidateStep = ValidateStep.ValidateNg;
                                        tsLblState.Text = DateTime.Now.ToString("HH:mm:ss") + @"：" +
                                                              @"校验异常，箱号为空";
                                    }
                                }
                                else
                                {
                                    _currentValidateStep = ValidateStep.ValidateNg;
                                    tsLblState.Text = DateTime.Now.ToString("HH:mm:ss") + @"：" +
                                                          @"校验失败";
                                }
                            }
                            else if (historyModel[0].Status == "待下发")
                            {
                                _currentValidateStep = ValidateStep.ValidateOk;
                                tsLblState.Text = DateTime.Now.ToString("HH:mm:ss") + @"：" +
                                                  @"已校验";
                            }
                            else
                            {
                                _currentValidateStep = ValidateStep.ValidateNg;
                                tsLblState.Text = DateTime.Now.ToString("HH:mm:ss") + @"：" +
                                                  @"校验异常，当前状态为：" + historyModel[0].Status;
                            }
                        }
                        else
                        {
                            _currentValidateStep = ValidateStep.ValidateNg;
                            tsLblState.Text = DateTime.Now.ToString("HH:mm:ss") + @"：" +
                                             @"校验异常，查询数量>1条记录";
                        }
                    }
                    else
                    {
                        _currentValidateStep = ValidateStep.ValidateNg;
                        tsLblState.Text = DateTime.Now.ToString("HH:mm:ss") + @"：" +
                                          @"校验异常，查询数量0条记录";
                    }
                }
                else if (_currentValidateStep == ValidateStep.ValidateOk)
                {
                    UpdatePanelDataValidation(UIStyle.Green, string.Empty);
                }
                else if (_currentValidateStep == ValidateStep.ValidateNg)
                {
                    UpdatePanelDataValidation(UIStyle.Red, string.Empty);
                }
            }
        }

        #endregion

        #region toolStripButton

        private void 海康威视摄像头参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 串口码枪参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 串口打印机参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new HikPrintForm();
            form.ShowDialog();
        }


        private void 重新打开设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 关闭设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _hikBarcodeScaner.StopGrabbing();
            _hikBarcodeScaner.CloseScanner();
        }

        #endregion

        #region 左上角，入库单选择

        private void BindData()
        {
            cmbStockInNo.Items.Clear();
            cmbSupplyNo.Items.Clear();
            cmbMaterialNo.Items.Clear();
            cmbMaterialNo.Enabled = false;

            {
                // 先获取数量，SELECT COUNT(1) FROM [newMaterialStockInInfo] where restNum > 0 and Status = '待入库'  and IsDelete = '0'
                // 再根据数量长度，分批获取数据并放入List中
                // 再将List排序

                const string sql = "select distinct stockInNo from newMaterialStockInInfo where NeedNum > 0 and Status = '待入库' and IsDelete = \'0\' order by stockInNo desc";
                var result = HikDbHelperSql.GetData(sql).ToArray();
                foreach (var t in result)
                    cmbStockInNo.Items.Add(t);
                //cmbStockInNo.Items.AddRange(GetData(sql).ToArray());

                if (cmbStockInNo.Items.Count > 0)
                    cmbStockInNo.SelectedIndex = 0;
            }

            {
                HikMaterialAnalysisHelper.TransDates.Clear();
                const string sql =
                    " SELECT [id],[DateNo],[lotNo],[year],[month],[date],[ymd],[week],[yymmdd] FROM [PLMS].[dbo].[newTransDate]";
                var dt = HikDbHelperSql.GetDataTable(sql);
                for (var i = 0; i < dt.DefaultView.Count; i++)
                {
                    var transDate = new HikMaterialAnalysisHelper.TransDate
                    {
                        Id = dt.DefaultView[i]["id"] == null
                            ? string.Empty
                            : dt.DefaultView[i]["id"].ToString(),
                        DateNo = dt.DefaultView[i]["DateNo"] == null
                            ? string.Empty
                            : dt.DefaultView[i]["DateNo"].ToString(),
                        LotNo = dt.DefaultView[i]["lotNo"] == null
                            ? string.Empty
                            : dt.DefaultView[i]["lotNo"].ToString(),
                        Year = dt.DefaultView[i]["year"] == null
                            ? string.Empty
                            : dt.DefaultView[i]["year"].ToString(),
                        Month = dt.DefaultView[i]["month"] == null
                            ? string.Empty
                            : dt.DefaultView[i]["month"].ToString(),
                        Date = dt.DefaultView[i]["date"] == null
                            ? string.Empty
                            : dt.DefaultView[i]["date"].ToString(),
                        Ymd = dt.DefaultView[i]["ymd"] == null
                            ? string.Empty
                            : dt.DefaultView[i]["ymd"].ToString(),
                        Week = dt.DefaultView[i]["week"] == null
                            ? string.Empty
                            : dt.DefaultView[i]["week"].ToString(),
                        Yymmdd = dt.DefaultView[i]["yymmdd"] == null
                            ? string.Empty
                            : dt.DefaultView[i]["yymmdd"].ToString()
                    };
                    HikMaterialAnalysisHelper.TransDates.Add(transDate);
                }
            }
        }

        //private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    txtsporadicCount.Text = string.Empty;
        //}

        private void cmbStockInNocomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var stockInNo = cmbStockInNo.Text;
            if (!string.IsNullOrEmpty(stockInNo))
            {
                cmbSupplyNo.Items.Clear();
                var sql2 = string.Format("select distinct supplyNo from newMaterialStockInInfo where StockInNo = '{0}' and IsDelete = \'0\'  order by supplyNo desc", stockInNo);
                var result = HikDbHelperSql.GetData(sql2).ToArray();
                foreach (var t in result)
                    cmbSupplyNo.Items.Add(t);
                if (cmbSupplyNo.Items.Count > 0)
                    cmbSupplyNo.SelectedIndex = 0;
            }
        }

        private void cmbDetailSearchcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDetailSearchType.comboBox.SelectedIndex == 3)
            {
                txtSearchMaterialNo.Visible = true;
                dateStartSearch.Visible = false;
                dateEndSearch.Visible = false;
            }
            else if (cmbDetailSearchType.comboBox.SelectedIndex == 4)
            {
                txtSearchMaterialNo.Visible = false;
                dateStartSearch.Visible = true;
                dateEndSearch.Visible = true;
            }
            else
            {
                txtSearchMaterialNo.Visible = false;
                dateStartSearch.Visible = false;
                dateEndSearch.Visible = false;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbStockInNo.Text) ||
                string.IsNullOrEmpty(cmbSupplyNo.Text))
                return;

            cmbMaterialNo.Enabled = true;
            cmbMaterialNo.Items.Add("不选择特定物料号");
            cmbMaterialNo.SelectedIndex = 0;

            cmbStockInNo.Enabled = false;
            cmbSupplyNo.Enabled = false;
            btnConfirm.Enabled = true;
            btnConfirm.Enabled = false;
            btnReConfirm.Enabled = true;
            btnRefresh.Enabled = false;
            btnManualStockIn.Enabled = true;
            //cmbIssporadic.Enabled = false;

            ResetHikScanResultUi();

            _expectedCodes.Clear();
            var getMaterialNoSql =
                "select [MaterialNo] from newMaterialStockInInfo where NeedNum > 0 and Status = '待入库' and IsDelete = \'0\' and StockInNo = '" +
                cmbStockInNo.Text + "' and SupplyNo = '" + cmbSupplyNo.Text + "' order by stockInNo desc";
            var getMaterials = HikDbHelperSql.GetData(getMaterialNoSql);
            if (!getMaterials.Any())
                return;

            foreach (var t in getMaterials)
                cmbMaterialNo.Items.Add(t);

            foreach (
                var dt in
                    getMaterials.Select(
                        t =>
                            "select id,MaterialBarcode,MaterialNo,ModelName,SupplyLedGroup,SupplyLedNo,Count,Barcodetype,BarcodeCount,PartNokey,LotNokey,DcNokey,Qtykey from dbo.newMaterialPrintCorrespond where IsDelete = '0' and Status = '有效' and MaterialNo = '" +
                            t + "'").Select(HikDbHelperSql.GetDataTable))
            {
                for (var i = 0; i < dt.DefaultView.Count; i++)
                {
                    if (dt.DefaultView[i]["MaterialBarcode"] != null)
                    {
                        _expectedCodes.Add(dt.DefaultView[i]["MaterialBarcode"].ToString());
                    }
                }
            }
        }

        private void btnReConfirm_Click(object sender, EventArgs e)
        {
            if (!_isGenerating)
            {
                cmbMaterialNo.Enabled = false;
                cmbMaterialNo.Items.Clear();

                cmbStockInNo.Enabled = true;
                cmbSupplyNo.Enabled = true;
                btnConfirm.Enabled = true;
                btnReConfirm.Enabled = false;
                btnRefresh.Enabled = true;
                btnManualStockIn.Enabled = false;
            }
        }

        private void ResetHikScanResultUi()
        {
            UpdatetxtBarcodeScanResultsRichTextBox(string.Empty);
            //for (var i = 0; i < dgvGeneratedResult.RowCount; i++)
            //    dgvGeneratedResult.Rows[i].Cells[1].Value = string.Empty;
            UpdateDgvGeneratedResult(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty);
            UpdateRichGeneratedBarcodeTextBox(Color.Gray, "等待扫码");
            UpdateCountTxt(txtCurrentStockInCount.Name, string.Empty);
            UpdateCountTxt(txtTotoalStockInCount.Name, string.Empty);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void btnManualStockIn_Click(object sender, EventArgs e)
        {
            if (_isGenerating)
                return;

            if (string.IsNullOrEmpty(cmbStockInNo.Text) ||
                string.IsNullOrEmpty(cmbSupplyNo.Text) ||
                string.IsNullOrEmpty(textBox2.Text))
                return;

            var stockInNo = cmbStockInNo.Text;
            var supplyNo = cmbSupplyNo.Text;

            _isGenerating = true;
            var frm = new HikManualGeneration(stockInNo, supplyNo, textBox2.Text);
            frm.ShowDialog();
            _isGenerating = false;
            BindingBoxMaterialCount();
        }

        #endregion

        #region 左下方，入库明细

        private void dataGridViewStockInDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && dgvStockInDetail.Columns[e.ColumnIndex].HeaderText.Contains("补打"))
            {
                try
                {
                    var id = dgvStockInDetail.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string sql =
                        @"select id,MaterialPrintNo,stockInNo,materialNo,modelName,supplyNo,lotNo,count,supplyLedGroup,issporadic,qualevel,earmarks,status from dbo.newMaterialStockInDetail where id = '" +
                        id + "'";
                    var printMaterialDt = HikDbHelperSql.GetDataTable(sql);
                    if (printMaterialDt.DefaultView.Count > 0)
                    {
                        var pn = "P/N:" + printMaterialDt.DefaultView[0]["modelName"];
                        var dcl = "L/N:" + printMaterialDt.DefaultView[0]["lotNo"];
                        var bin = "BIN:" + printMaterialDt.DefaultView[0]["supplyLedGroup"];
                        var pno = "PNO:" + printMaterialDt.DefaultView[0]["materialNo"];
                        var sup = "SUP:" + printMaterialDt.DefaultView[0]["supplyNo"];
                        var qty = "CTY:" + printMaterialDt.DefaultView[0]["count"];
                        var qualevel = printMaterialDt.DefaultView[0]["qualevel"] == null ? string.Empty : printMaterialDt.DefaultView[0]["qualevel"].ToString();

                        var printBarcode = printMaterialDt.DefaultView[0]["MaterialPrintNo"].ToString();
                        var date = "D/C:" + printBarcode.Split('@')[1].Substring(0, 6);

                        var printOrderNo = printBarcode.Split('@')[2].PadLeft(4, '0');
                        var no = "NO.:" + printOrderNo.Substring(printOrderNo.Length - 4, 4);

                        HikSetup.PrintLabel(pn, dcl, bin, pno, sup, qty, date, no, qualevel, printBarcode);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void btnDetailSearch_Click(object sender, EventArgs e)
        {
            BindStockInDetails();
        }

        private void BindStockInDetails()
        {
            //ShowWaitForm();

            dgvStockInDetail.Rows.Clear();
            //return;
            string sql = string.Empty;

            if (cmbDetailSearchType.comboBox.SelectedIndex == 0)
            {
                sql = @"select top 50 id,stockInNo,materialNo,modelName,supplyNo,lotNo,count,supplyLedGroup,issporadic,qualevel,earmarks,status,orderNo,trayNo,Other from dbo.newMaterialStockInDetail where IsDelete = '0' order by createTime desc";
            }
            else if (cmbDetailSearchType.comboBox.SelectedIndex == 1)
            {
                sql = @"select top 100 id,stockInNo,materialNo,modelName,supplyNo,lotNo,count,supplyLedGroup,issporadic,qualevel,earmarks,status,orderNo,trayNo,Other from dbo.newMaterialStockInDetail where IsDelete = '0' order by createTime desc";
            }
            else if (cmbDetailSearchType.comboBox.SelectedIndex == 2)
            {
                sql = @"select top 150 id,stockInNo,materialNo,modelName,supplyNo,lotNo,count,supplyLedGroup,issporadic,qualevel,earmarks,status,orderNo,trayNo,Other from dbo.newMaterialStockInDetail where IsDelete = '0' order by createTime desc";
            }
            else if (cmbDetailSearchType.comboBox.SelectedIndex == 3)
            {
                sql =
                    @"select top 500 id,stockInNo,materialNo,modelName,supplyNo,lotNo,count,supplyLedGroup,issporadic,qualevel,earmarks,status,orderNo,trayNo,Other from dbo.newMaterialStockInDetail where MaterialNo = '" +
                    txtSearchMaterialNo.txtInput.Text + "' and IsDelete = '0' order by createTime desc";
            }
            else
            {
                if (dateStartSearch.CurrentTime < DateTime.Parse("2010-01-01") || dateStartSearch.CurrentTime > DateTime.Parse("2039-12-31"))
                {
                    return;
                }

                if (dateEndSearch.CurrentTime < DateTime.Parse("2010-01-01") || dateEndSearch.CurrentTime > DateTime.Parse("2039-12-31"))
                {
                    return;
                }

                var startTime = dateStartSearch.CurrentTime.ToString("yyyy-MM-dd");
                var endTime = dateEndSearch.CurrentTime.ToString("yyyy-MM-dd");

                var findStartDateNo = HikMaterialAnalysisHelper.TransDates.FindIndex(f => f.Ymd == startTime);
                var findEndDateNo = HikMaterialAnalysisHelper.TransDates.FindIndex(f => f.Ymd == endTime);

                if (findStartDateNo != -1 && findEndDateNo != -1)
                {
                    var startDateNo = HikMaterialAnalysisHelper.TransDates[findStartDateNo].DateNo;
                    var endDateNo = HikMaterialAnalysisHelper.TransDates[findEndDateNo].DateNo;

                    sql =
                        @"select id,stockInNo,materialNo,modelName,supplyNo,lotNo,count,supplyLedGroup,issporadic,qualevel,earmarks,status,orderNo,trayNo,Other from dbo.newMaterialStockInDetail where DclotNo >= '" +
                        startDateNo + "' and DclotNo <= '" + endDateNo + "' and IsDelete = '0' order by createTime desc";
                }
            }

            try
            {
                var dt = HikDbHelperSql.GetDataTable(sql);

                for (var i = 0; i < dt.DefaultView.Count; i++)
                {
                    var newRow = dgvStockInDetail.Rows[dgvStockInDetail.Rows.Add()];

                    newRow.Cells[0].Value = dt.DefaultView[i]["id"] == null
                                           ? string.Empty
                                           : dt.DefaultView[i]["id"].ToString();

                    newRow.Cells[1].Value = dt.DefaultView[i]["stockInNo"] == null
                                            ? string.Empty
                                            : dt.DefaultView[i]["stockInNo"].ToString();

                    newRow.Cells[2].Value = dt.DefaultView[i]["materialNo"] == null
                                            ? string.Empty
                                            : dt.DefaultView[i]["materialNo"].ToString();

                    newRow.Cells[3].Value = dt.DefaultView[i]["modelName"] == null
                                            ? string.Empty
                                            : dt.DefaultView[i]["modelName"].ToString();

                    newRow.Cells[4].Value = dt.DefaultView[i]["supplyNo"] == null
                                            ? string.Empty
                                            : dt.DefaultView[i]["supplyNo"].ToString();

                    newRow.Cells[5].Value = dt.DefaultView[i]["lotNo"] == null
                                            ? string.Empty
                                            : dt.DefaultView[i]["lotNo"].ToString();

                    newRow.Cells[6].Value = dt.DefaultView[i]["count"] == null
                                            ? string.Empty
                                            : dt.DefaultView[i]["count"].ToString();

                    newRow.Cells[7].Value = dt.DefaultView[i]["supplyLedGroup"] == null
                                            ? string.Empty
                                            : dt.DefaultView[i]["supplyLedGroup"].ToString();

                    newRow.Cells[8].Value = dt.DefaultView[i]["issporadic"] == null
                                            ? string.Empty
                                            : dt.DefaultView[i]["issporadic"].ToString();

                    newRow.Cells[9].Value = dt.DefaultView[i]["qualevel"] == null
                                            ? string.Empty
                                            : dt.DefaultView[i]["qualevel"].ToString();

                    newRow.Cells[10].Value = dt.DefaultView[i]["earmarks"] == null
                                            ? string.Empty
                                            : dt.DefaultView[i]["earmarks"].ToString();

                    newRow.Cells[11].Value = dt.DefaultView[i]["status"] == null
                                            ? string.Empty
                                            : dt.DefaultView[i]["status"].ToString();

                    newRow.Cells[12].Value = dt.DefaultView[i]["TrayNo"] == null
                                            ? string.Empty
                                            : dt.DefaultView[i]["TrayNo"].ToString();

                    newRow.Cells[13].Value = dt.DefaultView[i]["orderNo"] == null
                                           ? string.Empty
                                           : dt.DefaultView[i]["orderNo"].ToString();

                    newRow.Cells[14].Value = dt.DefaultView[i]["Other"] == null
                                          ? string.Empty
                                          : dt.DefaultView[i]["Other"].ToString();
                }

                //dgvStockInDetail.AutoResizeRows();
                dgvStockInDetail.AutoResizeColumns();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion

        #region 扫码与生成

        public static List<string> PringLogs = new List<string>();
        public string LastStockInMaterialNo = string.Empty;

        private Graphics _mObjGc;

        /// <summary>
        /// 画笔颜色
        /// </summary>
        private readonly Pen _pen = new Pen(Color.Green, 3);

        /// <summary>
        /// 条码位置的4个点坐标
        /// </summary>
        private readonly Point[] _stPointList = new Point[4];

        private void bnTriggerExec_Click(
            List<HikScanerClass.BarcodeStruct> readBarcodeResults, string trayNo)
        {
            if (string.IsNullOrEmpty(cmbStockInNo.Text) ||
                string.IsNullOrEmpty(cmbSupplyNo.Text))
                return;

            if (readBarcodeResults == null) throw new ArgumentNullException("readBarcodeResults");
            UpdatetxtBarcodeScanResultsRichTextBox(string.Empty);
            //for (var i = 0; i < dgvGeneratedResult.RowCount; i++)
            //    dgvGeneratedResult.Rows[i].Cells[1].Value = string.Empty;
            UpdateDgvGeneratedResult(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty);
            UpdateRichGeneratedBarcodeTextBox(Color.DarkGoldenrod, "生成中");

            try
            {
                #region 二维码生成过程

                #region 入库单号，供应商代码，零星入库数量
                var stockInNo = cmbStockInNo.Text;
                var supplyNo = cmbSupplyNo.Text;

                var isToSporadicStr = "Q";
                var issporadicCount = 0;
                #endregion

                #region pictureBox和datagridView填充扫码结果

                var scanStr = string.Empty;
                var index = 1;
                foreach (var t in readBarcodeResults)
                {
                    var str = string.Format("码内容{0}：{1}\r\n", index, HikMaterialAnalysisHelper.ReplaceSpecialCharacter(t.Barcode));
                    scanStr += str;
                    index++;
                }

                UpdatetxtBarcodeScanResultsRichTextBox(scanStr);

                if (!readBarcodeResults.Any())
                {
                    UpdateRichGeneratedBarcodeTextBox(Color.Red, "没有扫描到任何条码，生成失败");
                    return;
                }
                #endregion

                #region 根据订单号和供应商代码，获取相对应的物料号

                List<NewMaterialStockInInfo> materialStockInInfo;

                if (cmbMaterialNo.SelectedIndex == 0)
                {
                    materialStockInInfo =
                        HikDbHelperSql.GetNewMaterialStockInInfoModels(
                            "NeedNum > 0 and Status = '待入库' and IsDelete = \'0\' and StockInNo = '" +
                            stockInNo + "' and SupplyNo = '" + supplyNo + "' order by stockInNo desc");
                }
                else
                {
                    materialStockInInfo =
                        HikDbHelperSql.GetNewMaterialStockInInfoModels(
                            "NeedNum > 0 and Status = '待入库' and IsDelete = \'0\' and StockInNo = '" +
                            stockInNo + "' and SupplyNo = '" + supplyNo + "' and MaterialNo = '" + cmbMaterialNo.Text + "' order by stockInNo desc");
                }

                if (!materialStockInInfo.Any())
                {
                    UpdateRichGeneratedBarcodeTextBox(Color.Red, string.Format("根据当前入库单号{0}和供应商代码{1}，未在系统中查询当相关的待入库物料。", stockInNo, supplyNo));
                    return;
                }
                #endregion

                #region 根据取得的物料号，获取打印模块信息

                var toComparePrintModels = new List<NewMaterialPrintCorrespond>();

                toComparePrintModels.AddRange(
                    HikDbHelperSql.GetNewMaterialPrintCorrespondModelsUnionAll(
                        materialStockInInfo.Select(
                            t => string.Format("MaterialNo = '{0}' and IsDelete = '0' and Status = '有效'", t.MaterialNo))
                            .ToArray()));

                if (!toComparePrintModels.Any())
                {
                    UpdateRichGeneratedBarcodeTextBox(Color.Red, string.Format("根据当前入库单号{0}和供应商代码{1}，未在系统中查询到相关物料号的二维码匹配信息。", stockInNo, supplyNo));
                    return;
                }
                #endregion

                #region 解析条码

                HikMaterialAnalysisHelper.StockInfo stockInfoStruck = null;
                var generatedType = -1;

                var scanResultsOfMatrixCode = readBarcodeResults.FindAll(
                    f =>
                        string.Equals(f.BarType, "QR码", StringComparison.CurrentCultureIgnoreCase) ||
                        string.Equals(f.BarType, "PDF417码", StringComparison.CurrentCultureIgnoreCase) ||
                        string.Equals(f.BarType, "DM码", StringComparison.CurrentCultureIgnoreCase));
                var scanResultsOfNotMatrixCode = readBarcodeResults.FindAll(
                    f =>
                        !string.Equals(f.BarType, "QR码", StringComparison.CurrentCultureIgnoreCase) &&
                        !string.Equals(f.BarType, "PDF417码", StringComparison.CurrentCultureIgnoreCase) &&
                        !string.Equals(f.BarType, "DM码", StringComparison.CurrentCultureIgnoreCase));
                var scanResultsOfOsramFormatCode = readBarcodeResults.FindAll(
                    f =>
                        string.Equals(f.BarType, "DM码", StringComparison.CurrentCultureIgnoreCase) &&
                        f.Barcode.StartsWith("[)>@06"));

                // 如有欧司朗格式，则欧司朗格式最优先判单
                if (scanResultsOfOsramFormatCode.Any() && materialStockInInfo.FindAll(f => f.MaterialNo.StartsWith("ST1101")).Any())
                {
                    var ledAnalysisErrorMsg = string.Empty;
                    var isGetLed =
                        scanResultsOfOsramFormatCode.Any(
                            t =>
                                HikMaterialAnalysisHelper.LedOsramBarcodeAnalysis(t.Barcode, toComparePrintModels, materialStockInInfo,
                                    out stockInfoStruck, ref ledAnalysisErrorMsg));

                    if (!isGetLed)
                    {
                        UpdateRichGeneratedBarcodeTextBox(Color.Red,
                            string.Format("扫描到“[)>@06”格式开头的DM码，但无法匹配到相关数据，{0}", ledAnalysisErrorMsg).TrimEnd('，'));
                        return;
                    }

                    generatedType = 0;
                }
                else
                {
                    // 优先判断矩阵码
                    var matrixCodeAnalysisErrorMsg = string.Empty;
                    var barCodeAnalysisErrorMsg = string.Empty;
                    var isCanAnalysisFromBarCode = false;

                    var isCanAnalysisFromDmCode =
                        scanResultsOfMatrixCode.Any(
                            t =>
                                HikMaterialAnalysisHelper.MatrixCodeAnalysis(
                                    t.Barcode, toComparePrintModels, out stockInfoStruck, ref matrixCodeAnalysisErrorMsg));

                    if (!isCanAnalysisFromDmCode)
                    {
                        isCanAnalysisFromBarCode = HikMaterialAnalysisHelper.BarCodeAnalysis(
                            scanResultsOfNotMatrixCode, toComparePrintModels, out stockInfoStruck, ref barCodeAnalysisErrorMsg);

                        if (isCanAnalysisFromBarCode)
                        {
                            generatedType = 2;
                        }
                    }
                    else
                    {
                        generatedType = 1;
                    }

                    if (!isCanAnalysisFromDmCode && !isCanAnalysisFromBarCode)
                    {
                        UpdateRichGeneratedBarcodeTextBox(Color.Red, string.Format("生成失败，{0}，{1}",
                                matrixCodeAnalysisErrorMsg, barCodeAnalysisErrorMsg).TrimEnd('，'));
                        return;
                    }

                    if (stockInfoStruck == null)
                    {
                        UpdateRichGeneratedBarcodeTextBox(Color.Red, "生成信息异常");
                        return;
                    }

                    if (string.IsNullOrEmpty(stockInfoStruck.PartNo) && string.IsNullOrEmpty(stockInfoStruck.ModelName))
                    {
                        UpdateRichGeneratedBarcodeTextBox(Color.Red, "生成信息异常，PartNo或ModelName缺失");
                        return;
                    }

                    if (string.IsNullOrEmpty(stockInfoStruck.LotNo))
                    {
                        UpdateRichGeneratedBarcodeTextBox(Color.Red, "未找到对应的LotNo。");
                        return;
                    }
                    if (string.IsNullOrEmpty(stockInfoStruck.DcNo))
                    {
                        if (!string.IsNullOrEmpty(stockInfoStruck.LotNo))
                            stockInfoStruck.DcNo = stockInfoStruck.LotNo;

                        if (string.IsNullOrEmpty(stockInfoStruck.DcNo))
                        {
                            UpdateRichGeneratedBarcodeTextBox(Color.Red, "未找到对应的DateCode。");
                            return;
                        }
                    }
                }
                #endregion

                #region 根据[newTransDate]表，生成批次号
                var formatDcodeErrorMsg = string.Empty;
                string formattedDcNo;
                if (!HikMaterialAnalysisHelper.FormatDcode(stockInfoStruck.DcNo, out formattedDcNo, ref formatDcodeErrorMsg))
                {
                    UpdateRichGeneratedBarcodeTextBox(Color.Red, string.Format("生成生产批次号失败，{0}", formatDcodeErrorMsg));
                    return;
                }
                stockInfoStruck.DcNo = formattedDcNo;
                #endregion

                #region 在newMaterialStockInInfo中查询Qualevel,Issporadic,Earmarks，并判断入库方式与选择的是否相同
                var searchStockInInfoByBarcode =
                    HikDbHelperSql.GetNewMaterialStockInInfoModels(
                        string.Format(
                            "NeedNum > 0 and Status = '待入库' and IsDelete = '0' and StockInNo = '{0}' and SupplyNo = '{1}' and MaterialNo = '{2}'",
                            stockInNo, supplyNo, stockInfoStruck.MaterialNo));
                if (searchStockInInfoByBarcode.Count == 1)
                {
                    stockInfoStruck.Qualevel = searchStockInInfoByBarcode[0].Qualevel;
                    stockInfoStruck.Issporadic = searchStockInInfoByBarcode[0].Issporadic;
                    stockInfoStruck.Earmarks = searchStockInInfoByBarcode[0].Earmarks;

                    if (string.IsNullOrEmpty(stockInfoStruck.Issporadic))
                        stockInfoStruck.Issporadic = "Q";

                    if (!string.Equals(isToSporadicStr, stockInfoStruck.Issporadic, StringComparison.CurrentCultureIgnoreCase))
                    {
                        UpdateRichGeneratedBarcodeTextBox(Color.Red, string.Format(
                                "根据入库单号：{0}，供应商编码：{1}，物料号：{2}，在入库单表中查找到入库方式为{3}，实际选择的是{4}", stockInNo,
                                supplyNo, stockInfoStruck.MaterialNo, stockInfoStruck.Issporadic, isToSporadicStr));
                        return;
                    }
                }
                else
                {
                    if (searchStockInInfoByBarcode.Count == 0)
                    {
                        UpdateRichGeneratedBarcodeTextBox(Color.Red, string.Format(
                            "根据入库单号：{0}，供应商编码：{1}，物料号：{2}，未在入库单表中查找到对应数据",
                            stockInNo, supplyNo, stockInfoStruck.MaterialNo));
                        return;
                    }

                    UpdateRichGeneratedBarcodeTextBox(Color.Red, string.Format(
                        "根据入库单号：{0}，供应商编码：{1}，物料号：{2}，在入库单表中查找到对应数据不唯一",
                        stockInNo, supplyNo, stockInfoStruck.MaterialNo));
                    return;
                }
                #endregion

                #region 判断当前箱体同一物料号的批次，批次不在同一月不能入库

                {
                    var toInMaterialNo = stockInfoStruck.MaterialNo;
                    //var trayNo = "B2209106470";

                    var toInDcLotNo = stockInfoStruck.DcNo; //"20230315";//"20220302";

                    var getMaterialInThisTrayNo =
                        "select distinct MaterialNo from newMaterialStockInDetail  WHERE IsDelete = '0' AND TrayNo = '" + trayNo + "'";
                    var materials = HikDbHelperSql.GetData(getMaterialInThisTrayNo);
                    if (materials.Contains(toInMaterialNo))
                    {
                        var getModels =
                            HikDbHelperSql.GetNewMaterialStockInDetail(
                                string.Format("IsDelete = '0' AND TrayNo = '" + trayNo + "' and MaterialNo = '" + toInMaterialNo + "'"));

                        if (getModels.Any())
                        {
                            var dcLotNo = getModels[0].DclotNo;
                            if (dcLotNo.Substring(0, 6) != toInDcLotNo.Substring(0, 6))
                            {
                                UpdateRichGeneratedBarcodeTextBox(Color.Red, string.Format("{0}",
                                    string.Format("物料号={0}，箱体={1}，已入箱批次:{2}，当前入箱批次{3}，批次不同无法入同一箱体。", toInMaterialNo, trayNo, dcLotNo, toInDcLotNo)));
                                return;
                            }
                        }
                    }
                }

                #endregion

                #region 整盘入库如果明细表有qty规则，取二维码QTY；明细表无qty规则，取用户输入。零星入库取用户填写数量
                int stockInCount;
                if (isToSporadicStr == "L")
                {
                    stockInCount = issporadicCount;
                }
                else
                {
                    if (!int.TryParse(stockInfoStruck.QtyNo, out stockInCount))
                    {
                        UpdateRichGeneratedBarcodeTextBox(Color.Red, string.Format("QtyNo不正确:{0}。", stockInfoStruck.QtyNo));
                        return;
                    }
                }
                #endregion

                #region 计算当前入库数和总数，并作对比

                int needNum;
                if (!int.TryParse(searchStockInInfoByBarcode[0].NeedNum.ToString(), out needNum))
                {
                    UpdateRichGeneratedBarcodeTextBox(Color.Red, string.Format(
                             "根据入库单号：{0}，供应商编码：{1}，物料号：{2}，未在入库单表中查找到NeedNum数据",
                            stockInNo, supplyNo, stockInfoStruck.MaterialNo));
                    return;
                }

                var totalNum = 0;

                var getHistoryStockInCount =
                    HikDbHelperSql.GetDataTable("select count from newMaterialStockInDetail where StockInNo = '" + stockInNo +
                                                "' and MaterialNo = '" + stockInfoStruck.MaterialNo + "' and SupplyNo = '" +
                                                supplyNo + "' and IsDelete = '0'");
                for (var i = 0; i < getHistoryStockInCount.DefaultView.Count; i++)
                {
                    if (getHistoryStockInCount.DefaultView[i]["count"] == null)
                        continue;
                    int c;
                    if (int.TryParse(getHistoryStockInCount.DefaultView[i]["count"].ToString(), out c))
                        totalNum += c;
                }

                if (totalNum + stockInCount > needNum)
                {
                    UpdateRichGeneratedBarcodeTextBox(Color.Red,
                        string.Format("根据入库单号：{0}，供应商编码：{1}，物料号：{2}，查询到订单数为：{3}，当前已入库订单数为{4}，当前扫描数为{5}，无法入库", stockInNo,
                            supplyNo, stockInfoStruck.MaterialNo, needNum, totalNum, stockInCount));
                    return;
                }

                #endregion

                #region 入库
                // 调用存储过程插入，并返回流水号和入库日期，用于显示
                string[] scanBarcodes = { string.Empty };
                try
                {
                    foreach (var tempStr in
                        readBarcodeResults.Select(t => string.Format("[{0}:{1}];", t.BarType, t.Barcode))
                            .Where(tempStr => scanBarcodes[0].Length + tempStr.Length <= 2000))
                        scanBarcodes[0] += tempStr;

                    if (string.IsNullOrEmpty(scanBarcodes[0]))
                    {
                        UpdateRichGeneratedBarcodeTextBox(Color.Red, "生成失败，无条码扫描结果");
                        return;
                    }
                }
                catch (Exception)
                {
                    UpdateRichGeneratedBarcodeTextBox(Color.Red, "生成失败，无条码扫描结果");
                    return;
                }

                var logPartNo = !string.IsNullOrEmpty(stockInfoStruck.PartNo) ? stockInfoStruck.PartNo : stockInfoStruck.ModelName;

                string stockInDate;
                string orderNo;
                string materialPrintNo;
                if (HikDbHelperSql.TryGetSerialNo(
                    stockInfoStruck.MaterialNo, supplyNo, stockInCount, stockInNo,
                    stockInfoStruck.SupplyLedGroup, stockInfoStruck.SupplyLedNo, logPartNo, stockInfoStruck.DcNo,
                    string.Empty, stockInfoStruck.Qualevel, stockInfoStruck.Issporadic, stockInfoStruck.Earmarks, scanBarcodes[0], stockInfoStruck.LotNo,
                    (totalNum + stockInCount).ToString(), out stockInDate, out orderNo, out materialPrintNo))
                {
                    UpdateDgvGeneratedResult(
                        stockInfoStruck.MaterialNo, //0
                        stockInDate, //1
                        orderNo.PadLeft(4, '0'), //2 // 需要自增，用存储过程做
                        supplyNo, //3
                        stockInCount.ToString(), //4 //qtyNo; // 需和数据库比对是否一致
                        stockInNo, //5
                        stockInfoStruck.SupplyLedGroup, //6
                        stockInfoStruck.SupplyLedNo, //7
                        logPartNo, //8
                        stockInfoStruck.DcNo, //9
                        string.Empty, //10
                        stockInfoStruck.Qualevel, //11
                        stockInfoStruck.Issporadic, //12
                        stockInfoStruck.Earmarks); //12

                    //dgvGeneratedResult.Rows[0].Cells[1].Value = stockInfoStruck.MaterialNo;
                    //dgvGeneratedResult.Rows[1].Cells[1].Value = stockInDate;
                    //dgvGeneratedResult.Rows[2].Cells[1].Value = orderNo.PadLeft(4, '0'); // 需要自增，用存储过程做
                    //dgvGeneratedResult.Rows[3].Cells[1].Value = supplyNo;
                    //dgvGeneratedResult.Rows[4].Cells[1].Value = stockInCount; //qtyNo; // 需和数据库比对是否一致
                    //dgvGeneratedResult.Rows[5].Cells[1].Value = stockInNo;
                    //dgvGeneratedResult.Rows[6].Cells[1].Value = stockInfoStruck.SupplyLedGroup;
                    //dgvGeneratedResult.Rows[7].Cells[1].Value = stockInfoStruck.SupplyLedNo;
                    //dgvGeneratedResult.Rows[8].Cells[1].Value = logPartNo;
                    //dgvGeneratedResult.Rows[9].Cells[1].Value = stockInfoStruck.DcNo;
                    //dgvGeneratedResult.Rows[11].Cells[1].Value = stockInfoStruck.Qualevel;
                    //dgvGeneratedResult.Rows[12].Cells[1].Value = stockInfoStruck.Issporadic;
                    //dgvGeneratedResult.Rows[13].Cells[1].Value = stockInfoStruck.Earmarks;

                    if (!string.IsNullOrEmpty(materialPrintNo))
                    {
                        if (PringLogs.Count > 100)
                            PringLogs.Clear();
                        PringLogs.Add(materialPrintNo);

                        var printOrderNo = orderNo.PadLeft(4, '0');

                        HikSetup.PrintLabel(string.Format("P/N:{0}", logPartNo),
                                                                 string.Format("L/N:{0}", stockInfoStruck.LotNo),
                                                                 string.Format("BIN:{0}", stockInfoStruck.SupplyLedGroup),
                                                                 string.Format("PNO:{0}", stockInfoStruck.MaterialNo),
                                                                 string.Format("SUP:{0}", supplyNo),
                                                                 string.Format("QTY:{0}", stockInCount),
                                                                 string.Format("D/C:{0}", stockInfoStruck.DcNo.Substring(0, 6)),
                                                                 string.Format("NO.:{0}", printOrderNo.Substring(printOrderNo.Length - 4, 4)), stockInfoStruck.Qualevel,
                                                                 materialPrintNo);

                        UpdateCountTxt(txtCurrentStockInCount.Name, (totalNum + stockInCount).ToString());
                        UpdateCountTxt(txtTotoalStockInCount.Name, needNum.ToString());

                        UpdateRichGeneratedBarcodeTextBox(Color.Green, string.Format("{0}\r\n", materialPrintNo));
                    }
                    else
                    {
                        UpdateRichGeneratedBarcodeTextBox(Color.Red, "生成失败，二维码为空。");
                    }
                }
                else
                {
                    UpdateRichGeneratedBarcodeTextBox(Color.Red, "生成失败。");
                }

                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                UpdateRichGeneratedBarcodeTextBox(Color.Red, string.Format("生成失败，过程出现异常，{0}。", ex.Message));
            }
        }

        public bool bnTriggerExec2_Click(
            List<HikScanerClass.BarcodeStruct> readBarcodeResults, string printBarcode, NewMaterialStockInDetail historyModel)
        {
            try
            {
                var printNoSp = printBarcode.Split("@");
                var toValidateMaterialNo = printNoSp[0];
                var toValidateQty = printNoSp[4];
                var toValidateLedGroup = printNoSp[6];
                var toValidateLedNo = printNoSp[7];
                var toValidateModelName = printNoSp[8];
                var toValidateDataCode = printNoSp[1];
                var toValidateSupplyNo = printNoSp[3];

                var detail =
                    HikDbHelperSql.GetNewMaterialStockInDetail(
                        string.Format("MaterialNo = '{0}' and MaterialPrintNo = '{1}' and IsDelete = '0'", toValidateMaterialNo, printBarcode));

                if (detail.Count == 0 || detail.Count > 1)
                {
                    return false;
                }

                var toValidateStockInNo = detail[0].StockInNo;
                var toValidateLotNo = detail[0].LotNo;

                #region 二维码生成过程

                #region 先取得图像
                if (HikScanerClass.MBitMap1 == null)
                {
                    return false;
                }
                #endregion

                #region pictureBox和datagridView填充扫码结果

                var scanResult = new HikScanerClass.BarcodeScanResult
                {
                    Height = (ushort)HikScanerClass.MBitMap1.Height,
                    Width = (ushort)HikScanerClass.MBitMap1.Width
                };

                scanResult.BarcodeStructs.AddRange(readBarcodeResults);

                UpdatePicture(HikScanerClass.MBitMap1, scanResult);

                if (!scanResult.BarcodeStructs.Any())
                {
                    return false;
                }
                #endregion

                #region 根据订单号和供应商代码，获取相对应的物料号

                var materialStockInInfo = HikDbHelperSql.GetNewMaterialStockInInfoModels(
                    "IsDelete = \'0\' and StockInNo = '" +
                    toValidateStockInNo + "' and SupplyNo = '" + toValidateSupplyNo + "' and MaterialNo = '" + toValidateMaterialNo + "' order by stockInNo desc");

                if (!materialStockInInfo.Any())
                {
                    return false;
                }
                #endregion

                #region 根据取得的物料号，获取打印模块信息
                var toComparePrintModels = new List<NewMaterialPrintCorrespond>();
                var toSearchMaterialNo =
                    string.Format(
                        string.IsNullOrEmpty(toValidateLedNo)
                            ? "MaterialNo = '{0}' and SupplyLedGroup = '{1}' and (SupplyLedNo = '{2}' or SupplyLedNo is null) and count = '{3}'"
                            : "MaterialNo = '{0}' and SupplyLedGroup = '{1}' and SupplyLedNo = '{2}' and count = '{3}'",
                        toValidateMaterialNo, toValidateLedGroup, toValidateLedNo, toValidateQty);

                toSearchMaterialNo =
                    string.Format("IsDelete = '0' and Status = '有效' and ({0})", toSearchMaterialNo);
                toComparePrintModels.AddRange(HikDbHelperSql.GetNewMaterialPrintCorrespondModels(toSearchMaterialNo));

                if (!toComparePrintModels.Any())
                {
                    return false;
                }
                #endregion

                #region 解析条码

                HikMaterialAnalysisHelper.StockInfo stockInfoStruck = null;

                var scanResultsOfMatrixCode = readBarcodeResults.FindAll(
                    f =>
                        string.Equals(f.BarType, "QR码", StringComparison.CurrentCultureIgnoreCase) ||
                        string.Equals(f.BarType, "PDF417码", StringComparison.CurrentCultureIgnoreCase) ||
                        string.Equals(f.BarType, "DM码", StringComparison.CurrentCultureIgnoreCase));
                var scanResultsOfNotMatrixCode = readBarcodeResults.FindAll(
                    f =>
                        !string.Equals(f.BarType, "QR码", StringComparison.CurrentCultureIgnoreCase) &&
                        !string.Equals(f.BarType, "PDF417码", StringComparison.CurrentCultureIgnoreCase) &&
                        !string.Equals(f.BarType, "DM码", StringComparison.CurrentCultureIgnoreCase));
                var scanResultsOfOsramFormatCode = readBarcodeResults.FindAll(
                    f =>
                        string.Equals(f.BarType, "DM码", StringComparison.CurrentCultureIgnoreCase) &&
                        f.Barcode.StartsWith("[)>@06"));

                // 如有欧司朗格式，则欧司朗格式最优先判单
                if (scanResultsOfOsramFormatCode.Any() && toValidateMaterialNo.StartsWith("ST1101"))
                {
                    var ledAnalysisErrorMsg = string.Empty;
                    var isGetLed =
                        scanResultsOfOsramFormatCode.Any(
                            t =>
                                HikMaterialAnalysisHelper.LedOsramBarcodeAnalysis(t.Barcode, toComparePrintModels, materialStockInInfo,
                                    out stockInfoStruck, ref ledAnalysisErrorMsg));

                    if (!isGetLed)
                    {
                        return false;
                    }
                }
                else
                {
                    // 优先判断矩阵码
                    var matrixCodeAnalysisErrorMsg = string.Empty;
                    var barCodeAnalysisErrorMsg = string.Empty;
                    var isCanAnalysisFromBarCode = false;

                    var isCanAnalysisFromDmCode =
                        scanResultsOfMatrixCode.Any(
                            t =>
                                HikMaterialAnalysisHelper.MatrixCodeAnalysis(
                                    t.Barcode, toComparePrintModels, out stockInfoStruck, ref matrixCodeAnalysisErrorMsg));

                    if (!isCanAnalysisFromDmCode)
                    {
                        isCanAnalysisFromBarCode = HikMaterialAnalysisHelper.BarCodeAnalysis(
                            scanResultsOfNotMatrixCode, toComparePrintModels, out stockInfoStruck, ref barCodeAnalysisErrorMsg);
                    }

                    if (!isCanAnalysisFromDmCode && !isCanAnalysisFromBarCode)
                    {
                        return false;
                    }

                    if (stockInfoStruck == null)
                    {
                        return false;
                    }

                    if (string.IsNullOrEmpty(stockInfoStruck.PartNo) && string.IsNullOrEmpty(stockInfoStruck.ModelName))
                    {
                        return false;
                    }

                    if (string.IsNullOrEmpty(stockInfoStruck.LotNo))
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(stockInfoStruck.DcNo))
                    {
                        return false;
                    }
                }
                #endregion

                #region 根据[newTransDate]表，生成批次号
                var formatDcodeErrorMsg = string.Empty;
                string formattedDcNo;
                if (!HikMaterialAnalysisHelper.FormatDcode(stockInfoStruck.DcNo, out formattedDcNo, ref formatDcodeErrorMsg))
                {
                    return false;
                }
                stockInfoStruck.DcNo = formattedDcNo;
                #endregion

                #region 判断当前箱体同一物料号的批次，批次不在同一月不能入库

                {
                    var toInMaterialNo = stockInfoStruck.MaterialNo;
                    //var trayNo = "B2209106470";

                    var toInDcLotNo = stockInfoStruck.DcNo; //"20230315";//"20220302";

                    var getMaterialInThisTrayNo =
                        "select distinct MaterialNo from newMaterialStockInDetail  WHERE IsDelete = '0' AND TrayNo = '" + textBox2.Text + "'";
                    var materials = HikDbHelperSql.GetData(getMaterialInThisTrayNo);
                    if (materials.Contains(toInMaterialNo))
                    {
                        var getModels =
                            HikDbHelperSql.GetNewMaterialStockInDetail(
                                string.Format("IsDelete = '0' AND TrayNo = '" + textBox2.Text + "' and MaterialNo = '" + toInMaterialNo + "'"));

                        if (getModels.Any())
                        {
                            var dcLotNo = getModels[0].DclotNo;
                            if (dcLotNo.Substring(0, 6) != toInDcLotNo.Substring(0, 6))
                            {
                                return false;
                            }
                        }
                    }
                }

                #endregion

                var logPartNo = !string.IsNullOrEmpty(stockInfoStruck.PartNo) ? stockInfoStruck.PartNo : stockInfoStruck.ModelName;

                if (stockInfoStruck.MaterialNo == toValidateMaterialNo &&
                    stockInfoStruck.QtyNo == toValidateQty &&
                    stockInfoStruck.SupplyLedGroup == toValidateLedGroup &&
                    stockInfoStruck.SupplyLedNo == toValidateLedNo &&
                    logPartNo == toValidateModelName &&
                    stockInfoStruck.DcNo.Substring(0, 6) + "01" == toValidateDataCode &&
                    stockInfoStruck.LotNo == toValidateLotNo)
                {
                    return true;
                }

                #endregion
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public static bool TryToSap(string materialPrintNo, string trayNo, ref string errorMsg)
        {
            try
            {
                var getToSapModel =
                    HikDbHelperSql.GetNewMaterialStockInDetail(
                    string.Format("MaterialPrintNo='{0}' and Status = '待入库' and IsDelete = '0' and (Other != '入SAP' or Other = '' or Other is null) and (Other2 is not null and LEN(Other2) > 0)", materialPrintNo));

                if (getToSapModel.Count == 1 && getToSapModel[0].Other2.Length > 12)
                {
                    var toSapCmd = getToSapModel[0].Other2;//.Replace(" ", "@");
                    HikSetup.ToSapPort.ReadDataStr();
                    HikSetup.ToSapPort.ClearBuff();
                    HikSetup.ToSapPort.SendCommand(toSapCmd);

                    var toSapResult = string.Empty;
                    var enterTime = DateTime.Now;
                    while (true)
                    {
                        toSapResult += HikSetup.ToSapPort.ReadDataStr();
                        if (toSapResult.Length > 12)
                            break;

                        if (ValueHelper.GetTimeSpanMs(enterTime, DateTime.Now) > HikSetup.ToSapDelay)
                            break;
                    }

                    if (!string.IsNullOrEmpty(toSapResult))
                    {
                        if (toSapResult.ToLower().Contains(toSapCmd.ToLower()))
                        {
                            var updateSql = "update newMaterialStockInDetail set Other = '入SAP', TrayNo = '" + trayNo + "' ,Status = '待下发' where MaterialPrintNo='" + materialPrintNo + "' and IsDelete = '0' and (Other != '入SAP' or Other = '' or Other is null) and (Other2 is not null and LEN(Other2) > 0) and Status = '待入库'";

                            if (HikDbHelperSql.Update(updateSql) > 0)
                            {
                                return true;
                            }

                            errorMsg = string.Format("将二维码={0}从待入库更新成待下发且入SAP时失败，请核对SAP数据并重新入库", materialPrintNo);
                            return false;
                        }

                        errorMsg = @"SAP入库失败，请核对SAP数据并重新入库";
                        return false;
                    }

                    errorMsg = @"SAP入库超时，请核对SAP数据并重新入库";
                    return false;
                }

                errorMsg = string.Format("入SAP前查询二维码={0}且状态为待入库状态的数据不唯一", materialPrintNo);
                return false;
            }
            catch (Exception ex)
            {
                errorMsg = string.Format("二维码={0}，入SAP出现异常，{1}", materialPrintNo, ex.Message);
                return false;
            }
        }

        #endregion

        #region delegate

        private delegate void UpdateGeneratedBarcodeRichTextBoxDelegate(Color color, string text, bool isAppend = false);
        private void UpdateRichGeneratedBarcodeTextBox(Color color, string text, bool isAppend = false)
        {
            var updateRichTextBoxDelegate = new UpdateGeneratedBarcodeRichTextBoxDelegate(UpdateRichGeneratedBarcodeTextBox);
            if (txtGeneratedBarcode.InvokeRequired)
            {
                Invoke(updateRichTextBoxDelegate, color, text, isAppend);
            }
            else
            {
                if (!isAppend)
                {
                    txtGeneratedBarcode.BackColor = color;
                    txtGeneratedBarcode.Text = string.Format("{0}: {1}。",
                     DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text);
                }
                else
                {
                    txtGeneratedBarcode.Text += "\r";
                    txtGeneratedBarcode.Text += text;
                }
            }
        }

        private delegate void UpdatetxtBarcodeScanResultsRichTextBoxDelegate(string text);
        private void UpdatetxtBarcodeScanResultsRichTextBox(string text)
        {
            var updateRichTextBoxDelegate = new UpdatetxtBarcodeScanResultsRichTextBoxDelegate(UpdatetxtBarcodeScanResultsRichTextBox);

            if (txtBarcodeScanResults.InvokeRequired)
            {
                Invoke(updateRichTextBoxDelegate, text);
            }
            else
            {
                txtBarcodeScanResults.Text = text;
            }
        }

        private delegate void UpdateDgvGeneratedResultDelegate(
            string row0, string row1, string row2, string row3, string row4, string row5, string row6, string row7,
            string row8, string row9, string row10, string row11, string row12, string row13);
        private void UpdateDgvGeneratedResult(
            string row0, string row1, string row2, string row3, string row4, string row5, string row6, string row7,
            string row8, string row9, string row10, string row11, string row12, string row13)
        {
            var updateDgvGeneratedResultDelegate = new UpdateDgvGeneratedResultDelegate(UpdateDgvGeneratedResult);

            if (txtBarcodeScanResults.InvokeRequired)
            {
                Invoke(updateDgvGeneratedResultDelegate, row0, row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12, row13);
            }
            else
            {
                dgvGeneratedResult.Rows[0].Cells[1].Value = row0;
                dgvGeneratedResult.Rows[1].Cells[1].Value = row1;
                dgvGeneratedResult.Rows[2].Cells[1].Value = row2; // 需要自增，用存储过程做
                dgvGeneratedResult.Rows[3].Cells[1].Value = row3;
                dgvGeneratedResult.Rows[4].Cells[1].Value = row4; //qtyNo; // 需和数据库比对是否一致
                dgvGeneratedResult.Rows[5].Cells[1].Value = row5;
                dgvGeneratedResult.Rows[6].Cells[1].Value = row6;
                dgvGeneratedResult.Rows[7].Cells[1].Value = row7;
                dgvGeneratedResult.Rows[8].Cells[1].Value = row8;
                dgvGeneratedResult.Rows[9].Cells[1].Value = row9;
                dgvGeneratedResult.Rows[11].Cells[1].Value = row11;
                dgvGeneratedResult.Rows[12].Cells[1].Value = row12;
                dgvGeneratedResult.Rows[13].Cells[1].Value = row13;
            }
        }

        private delegate void PictureUpdateDelegate(Image imgBitmap, HikScanerClass.BarcodeScanResult imgWithBarcodes);
        private void UpdatePicture(Image imgBitmap, HikScanerClass.BarcodeScanResult imgWithBarcodes)
        {
            var pictureUpdateDelegate = new PictureUpdateDelegate(UpdatePicture);

            if (pictureBox1.InvokeRequired)
            {
                Invoke(pictureUpdateDelegate, imgBitmap, imgWithBarcodes);
            }
            else
            {
                #region pictureBox和datagridView填充扫码结果

                pictureBox1.Image = imgBitmap;
                _mObjGc = pictureBox1.CreateGraphics();
                pictureBox1.Refresh();

                foreach (var t in imgWithBarcodes.BarcodeStructs)
                {
                    for (var j = 0; j < 4; ++j)
                    {
                        _stPointList[j].X = (int)(t.Position[j].X * (float)pictureBox1.Size.Width / imgWithBarcodes.Width);
                        _stPointList[j].Y = (int)(t.Position[j].Y * (float)pictureBox1.Size.Height / imgWithBarcodes.Height);
                    }

                    _mObjGc.DrawPolygon(_pen, _stPointList);
                }
                #endregion
            }
        }

        private delegate void UpdatePanelDataValidationDelegate(UIStyle uiStyle, string text);
        private void UpdatePanelDataValidation(UIStyle uiStyle, string text)
        {
            var updatePanelDataValidationDelegate = new UpdatePanelDataValidationDelegate(UpdatePanelDataValidation);
            if (panelDataValidation.InvokeRequired)
            {
                Invoke(updatePanelDataValidationDelegate, uiStyle, text);
            }
            else
            {
                panelDataValidation.Style = uiStyle;
                if (!string.IsNullOrEmpty(text))
                    panelDataValidation.Text = text;
            }
        }

        private delegate void UpdateCountTxtDelegate(string controlName, string text);
        private void UpdateCountTxt(string controlName, string text)
        {
            var txtDelegate = new UpdateCountTxtDelegate(UpdateCountTxt);

            if (controlName == txtCurrentStockInCount.Name)
            {
                if (txtCurrentStockInCount.InvokeRequired)
                    Invoke(txtDelegate, controlName, text);
                else
                    txtCurrentStockInCount.Text = text;
            }
            else if (controlName == txtTotoalStockInCount.Name)
            {
                if (txtTotoalStockInCount.InvokeRequired)
                    Invoke(txtDelegate, controlName, text);
                else
                    txtTotoalStockInCount.Text = text;
            }
            else if (controlName == txtBoxMaterialCount.Name)
            {
                if (txtBoxMaterialCount.InvokeRequired)
                    Invoke(txtDelegate, controlName, text);
                else
                    txtBoxMaterialCount.Text = text;
            }
        }

        #endregion
    }
}