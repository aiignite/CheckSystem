using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility;
using Controller;
using HZH_Controls.IconFont;

namespace CheckSystem.HelperForms.Tps929120
{
    public partial class TpsFormDataViewer : Form
    {
        public readonly Tps929120Ti Tps929120Ti = new Tps929120Ti("TPS929129TI");
        public readonly SyControllerWith56Pin ControllerWith56Pin = new SyControllerWith56Pin("Controller");
        private readonly Dictionary<int, bool> _onlineList = new Dictionary<int, bool>();
        private readonly Dictionary<string, int[]> _otherParasPosion = new Dictionary<string, int[]>();

        public TpsFormDataViewer(SyControllerWith56Pin controllerWith56Pin)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Icon = FontImages.GetIcon(FontIcons.A_fa_plug, 32,
                Color.DodgerBlue);
            WindowState = FormWindowState.Maximized;

            if (controllerWith56Pin != null)
            {
                ControllerWith56Pin = controllerWith56Pin;

                Closed += FormDataViewer_Closed;
                Load += FormDataViewer_Load;

                //ControllerWith56Pin.InitRemoteIpAddress("192.168.1.28:8088");
                Tps929120Ti.MySerialPort = ControllerWith56Pin.GatewaySci2;
                Tps929120Ti.PushLogSciMsg += Tps929120Ti_PushLogSciMsg;

                btnOnOffLine.BackColor = Color.DarkRed;
                btnOnOffLine.Text = @"offline".ToUpper();

                cmbAddrList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                for (var i = 0; i < 16; i++)
                {
                    // Addr:00h
                    var bits = Convert.ToString((byte)i, 2).PadLeft(8, '0');
                    var str = string.Format("Addr: {0}h,{1}{2}{3}{4}b", ValueHelper.GetHextStr((byte)i), bits[4], bits[5],
                        bits[6], bits[7]);
                    cmbAddrList.comboBox.Items.Add(str);
                    _onlineList.Add(i, false);
                }
                cmbAddrList.comboBox.SelectedIndex = 0;
                cmbAddrList.comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;

                #region EEPI&&EEPP Paras
                dgvIoutPwm.label.Text = @"EEPI&&EEPP Paras";
                dgvIoutPwm.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Address" });
                dgvIoutPwm.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Acronym" });
                dgvIoutPwm.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Register Name" });
                dgvIoutPwm.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Default" });
                dgvIoutPwm.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Value(HEX)" });
                dgvIoutPwm.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Value(DEC)" });
                dgvIoutPwm.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "DC(MA)/PWM D%" });

                dgvIoutPwm.dataGridView.AllowUserToAddRows = false;
                dgvIoutPwm.dataGridView.AllowUserToDeleteRows = false;
                dgvIoutPwm.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvIoutPwm.dataGridView.AllowUserToResizeColumns = true;
                dgvIoutPwm.dataGridView.AllowUserToResizeRows = false;
                dgvIoutPwm.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvIoutPwm.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
                dgvIoutPwm.dataGridView.ReadOnly = true;
                dgvIoutPwm.dataGridView.RowHeadersVisible = false;
                for (var i = 0; i < dgvIoutPwm.dataGridView.Columns.Count; i++)
                    dgvIoutPwm.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                for (var i = 0; i < 12; i++)
                {
                    const int baseAddr = 128;
                    var acronym = string.Format("EEPI{0}", i);
                    var registerName = string.Format("EEPIOUT{0}", i);

                    var rowIndex = dgvIoutPwm.dataGridView.Rows.Add();
                    var row = dgvIoutPwm.dataGridView.Rows[rowIndex];
                    row.Cells[0].Value = string.Format("{0}h", ValueHelper.GetHextStr((byte)(baseAddr + i)));
                    row.Cells[1].Value = acronym;
                    row.Cells[2].Value = registerName;
                    row.Cells[3].Value = "3Fh";
                }

                for (var i = 0; i < 12; i++)
                {
                    const int baseAddr = 160;
                    var acronym = string.Format("EEPP{0}", i);
                    var registerName = string.Format("EEP_PWMOUT{0}", i);

                    var rowIndex = dgvIoutPwm.dataGridView.Rows.Add();
                    var row = dgvIoutPwm.dataGridView.Rows[rowIndex];
                    row.Cells[0].Value = string.Format("{0}h", ValueHelper.GetHextStr((byte)(baseAddr + i)));
                    row.Cells[1].Value = acronym;
                    row.Cells[2].Value = registerName;
                    row.Cells[3].Value = "3Fh";
                }
                #endregion

                #region EEP_FS0&&EEP_FS1&&EEP_DIAGEN
                dgvChConfig.label.Text = @"EEP_FS0&&EEP_FS1&&EEP_DIAGEN";
                dgvChConfig.label.Height = 30;

                dgvChConfig.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvChConfig.dataGridView.MultiSelect = false;
                dgvChConfig.dataGridView.ColumnHeadersVisible = false;
                dgvChConfig.dataGridView.RowHeadersVisible = false;
                dgvChConfig.dataGridView.Columns.Add(new DataGridViewButtonColumn());
                dgvChConfig.dataGridView.Columns.Add(new DataGridViewButtonColumn());
                dgvChConfig.dataGridView.Columns.Add(new DataGridViewButtonColumn());
                dgvChConfig.dataGridView.Columns.Add(new DataGridViewButtonColumn());

                dgvChConfig.dataGridView.AllowUserToAddRows = false;
                dgvChConfig.dataGridView.AllowUserToDeleteRows = false;
                dgvChConfig.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvChConfig.dataGridView.AllowUserToResizeColumns = false;
                dgvChConfig.dataGridView.AllowUserToResizeRows = false;
                dgvChConfig.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvChConfig.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
                dgvChConfig.dataGridView.RowHeadersVisible = false;
                for (var i = 0; i < dgvChConfig.dataGridView.Columns.Count; i++)
                    dgvChConfig.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvChConfig.dataGridView.SelectionChanged += dgvChConfigDataGridView_SelectionChanged;
                dgvChConfig.dataGridView.CellContentClick += ledPara_dataGridView_CellContentClick;

                for (var i = 0; i < 9; i++)
                {
                    var rowHeader = dgvChConfig.dataGridView.Rows[dgvChConfig.dataGridView.Rows.Add()];
                    rowHeader.DefaultCellStyle.ForeColor = Color.White;
                    var rowValue = dgvChConfig.dataGridView.Rows[dgvChConfig.dataGridView.Rows.Add()];
                    var rowEnd = dgvChConfig.dataGridView.Rows[dgvChConfig.dataGridView.Rows.Add()];

                    if ((i + 1) % 3 == 0)
                    {
                        for (var j = 0; j < 4; j++)
                        {
                            rowEnd.Cells[j].Value = "===========================";
                            //rowEnd.Cells[j].Style.BackColor = Color.DarkGray;

                            var btn = rowEnd.Cells[j] as DataGridViewButtonCell;
                            if (btn != null)
                            {
                                btn.FlatStyle = FlatStyle.Flat;
                                btn.Style.BackColor = Color.Black;
                            }
                        }
                    }

                    rowHeader.ReadOnly = true;

                    for (var j = 0; j < 4; j++)
                    {
                        var index = i * 4 + j;
                        var strName = string.Empty;
                        var strValue = string.Empty;
                        var colorName = Color.Blue;
                        var colorValue = Color.DarkRed;
                        Color titleColor;

                        if (index >= 0 && index <= 11)
                        {
                            strName = string.Format("EEP_FS0CH{0}", index);
                            strValue = "Disable";
                            titleColor = colorName = Color.Blue;
                            colorValue = Color.DarkRed;
                        }
                        else if (index >= 12 && index <= 23)
                        {
                            strName = string.Format("EEP_FS1CH{0}", index - 12);
                            strValue = "Enable";
                            titleColor = colorName = Color.DarkSlateBlue;
                            colorValue = Color.DarkGreen;
                        }
                        else if (index >= 24 && index <= 35)
                        {
                            strName = string.Format("EEP_DIAGENCH{0}", index - 24);
                            strValue = "Enable";
                            titleColor = colorName = Color.DarkGoldenrod;
                            colorValue = Color.DarkGreen;
                        }

                        var btnName = rowHeader.Cells[j] as DataGridViewButtonCell;
                        if (btnName != null)
                        {
                            btnName.FlatStyle = FlatStyle.Flat;
                            btnName.Style.BackColor = colorName;
                        }

                        var btnValue = rowValue.Cells[j] as DataGridViewButtonCell;
                        if (btnValue != null)
                        {
                            btnValue.FlatStyle = FlatStyle.Flat;
                            btnValue.Style.BackColor = colorValue;
                        }

                        if ((i + 1) % 3 != 0)
                        {
                            var btn = rowEnd.Cells[j] as DataGridViewButtonCell;
                            if (btn != null)
                            {
                                titleColor = Color.White;
                                btn.FlatStyle = FlatStyle.Flat;
                                btn.Style.BackColor = titleColor;
                            }
                        }

                        rowHeader.Cells[j].Value = strName;
                        rowHeader.Cells[j].Style.Font = new Font("黑体", 10, FontStyle.Bold, GraphicsUnit.Point, 120);
                        rowValue.Cells[j].Value = strValue;
                    }
                }
                #endregion

                #region 其它
                dgvOtherConfig.label.Text = @"Ohters";
                dgvOtherConfig.label.Height = 30;

                dgvOtherConfig.dataGridView.ColumnHeadersVisible = true;
                dgvOtherConfig.dataGridView.RowHeadersVisible = false;
                dgvOtherConfig.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name" });
                dgvOtherConfig.dataGridView.Columns.Add(new DataGridViewComboBoxColumn { Name = "Value" });
                dgvOtherConfig.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description" });
                dgvOtherConfig.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name" });
                dgvOtherConfig.dataGridView.Columns.Add(new DataGridViewComboBoxColumn { Name = "Value" });
                dgvOtherConfig.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description" });

                dgvOtherConfig.dataGridView.AllowUserToAddRows = false;
                dgvOtherConfig.dataGridView.AllowUserToDeleteRows = false;
                dgvOtherConfig.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvOtherConfig.dataGridView.AllowUserToResizeColumns = false;
                dgvOtherConfig.dataGridView.AllowUserToResizeRows = false;
                dgvOtherConfig.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvOtherConfig.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
                dgvOtherConfig.dataGridView.RowHeadersVisible = false;
                for (var i = 0; i < dgvOtherConfig.dataGridView.Columns.Count; i++)
                    dgvOtherConfig.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                dgvOtherConfig.dataGridView.Columns[0].ReadOnly = dgvOtherConfig.dataGridView.Columns[2].ReadOnly =
                    dgvOtherConfig.dataGridView.Columns[3].ReadOnly = dgvOtherConfig.dataGridView.Columns[5].ReadOnly = true;

                var listRow = new List<DataGridViewRow>();
                for (var i = 0; i < 7; i++)
                {
                    var row1 = dgvOtherConfig.dataGridView.Rows[dgvOtherConfig.dataGridView.Rows.Add()];
                    listRow.Add(row1);
                }

                for (var i = 0; i < 256; i++)
                {
                    var str = Convert.ToString((byte)i, 2).PadLeft(8, '0');
                    if (i < 16)
                        _eepDevaddr.Add(string.Format("{0}h,{1}{2}{3}{4}b", ValueHelper.GetHextStr((byte)i), str[4], str[5],
                            str[6], str[7]));
                    _eepAdcshortth.Add(string.Format("{0}b", str));
                }

                listRow[0].Cells[0].Value = Ldo;
                SetCmbInDataGridView(Ldo, dgvOtherConfig.dataGridView, 0, 1, "5.0 V", _eepLdo, "LDO output voltage setting.");
                listRow[0].Cells[3].Value = Expen;
                SetCmbInDataGridView(Expen, dgvOtherConfig.dataGridView, 0, 4, "Disable", _eepExpen, "PWM generator exponetinal dimmng enable register.");

                listRow[1].Cells[0].Value = Devaddr;
                SetCmbInDataGridView(Devaddr, dgvOtherConfig.dataGridView, 1, 1, _eepDevaddr[0], _eepDevaddr, "Device slave address EEPROM register.");
                listRow[1].Cells[3].Value = Pwnfreq;
                SetCmbInDataGridView(Pwnfreq, dgvOtherConfig.dataGridView, 1, 4, "2 kHz", _eepPwmfreq, "PWM frequency selection EEPROM register.");

                listRow[2].Cells[0].Value = Intaddr;
                SetCmbInDataGridView(Intaddr, dgvOtherConfig.dataGridView, 2, 1, "Disable", _eepIntaddr, "Slave address selection bit.");
                listRow[2].Cells[3].Value = Ofaf;
                SetCmbInDataGridView(Ofaf, dgvOtherConfig.dataGridView, 2, 4, _eepOfaf[1], _eepOfaf, "Output failure state setting.");

                listRow[3].Cells[0].Value = Refrange;
                SetCmbInDataGridView(Refrange, dgvOtherConfig.dataGridView, 3, 1, "512", _eepRefRrange, "Reference current ratio setting EEPROM register.");
                listRow[3].Cells[3].Value = Fltimeout;
                SetCmbInDataGridView(Fltimeout, dgvOtherConfig.dataGridView, 3, 4, "1 ms", _eepFltimeout, "FlexWire timeout timer setting EEPROM register.");

                listRow[4].Cells[0].Value = Adclowsupth;
                SetCmbInDataGridView(Adclowsupth, dgvOtherConfig.dataGridView, 4, 1, "8 V", _eepAdclowsupth, "ADC Supply monitor threshold setting EEPROM register.");
                listRow[4].Cells[3].Value = Odiout;
                SetCmbInDataGridView(Odiout, dgvOtherConfig.dataGridView, 4, 4, "0", _eepOdiout, "On-demand diagnostics output current setting EEPROM register.");

                listRow[5].Cells[0].Value = Odpw;
                SetCmbInDataGridView(Odpw, dgvOtherConfig.dataGridView, 5, 1, "100 µs", _eepOdpw, "On-demand diagnostics pulse-width setting EEPROM register.");
                listRow[5].Cells[3].Value = Wdtimer;
                SetCmbInDataGridView(Wdtimer, dgvOtherConfig.dataGridView, 5, 4, "Direct to FS", _eepWdtimer, "Watchdog timer setting EEPROM register.");

                listRow[6].Cells[0].Value = Inittimer;
                SetCmbInDataGridView(Inittimer, dgvOtherConfig.dataGridView, 6, 1, "0 ms", _eepInittimer, "Initialization timer setting EEPROM register.");
                listRow[6].Cells[3].Value = Adcshortth;
                SetCmbInDataGridView(Adcshortth, dgvOtherConfig.dataGridView, 6, 4, _eepAdcshortth[0], _eepAdcshortth, "ADC short detection threshold setting EEPROM register.");
                #endregion

                #region Buttons
                var btnFont = new Font("黑体", 10, FontStyle.Bold, GraphicsUnit.Point, 120);
                const int btnWidth = 120;
                const int btnHeight = 50;
                var backColor = Color.DarkGray;
                var refreshBtn = new Button
                {
                    Text = @"获取在线芯片连接",
                    Font = btnFont,
                    Width = btnWidth,
                    Height = btnHeight,
                    BackColor = backColor
                };
                var foreachReadEepromBtn = new Button
                {
                    Text = @"遍历读取芯片参数",
                    Font = btnFont,
                    Width = btnWidth,
                    Height = btnHeight,
                    BackColor = backColor
                };
                var singleReadEepromBtn = new Button
                {
                    Text = @"读取单芯片参数",
                    Font = btnFont,
                    Width = btnWidth,
                    Height = btnHeight,
                    BackColor = backColor
                };
                var ledControllerBtn = new Button
                {
                    Text = @"LED控制",
                    Font = btnFont,
                    Width = btnWidth,
                    Height = btnHeight,
                    BackColor = backColor
                };
                var foreachWriteBtn = new Button
                {
                    Text = @"遍历刷写芯片参数",
                    Font = btnFont,
                    Width = btnWidth,
                    Height = btnHeight,
                    BackColor = backColor
                };
                var singleWriteBtn = new Button
                {
                    Text = @"刷写单芯片参数",
                    Font = btnFont,
                    Width = btnWidth,
                    Height = btnHeight,
                    BackColor = backColor
                };

                btnFlowPanel.Controls.Add(refreshBtn);
                btnFlowPanel.Controls.Add(foreachReadEepromBtn);
                btnFlowPanel.Controls.Add(singleReadEepromBtn);
                btnFlowPanel.Controls.Add(foreachWriteBtn);
                btnFlowPanel.Controls.Add(singleWriteBtn);
                btnFlowPanel.Controls.Add(ledControllerBtn);

                foreach (var b in btnFlowPanel.Controls.OfType<Button>())
                    b.Click += func_Click;
                #endregion
            }
        }

        private bool _isShowMsg;

        private void Tps929120Ti_PushLogSciMsg(byte[] datas)
        {
            if (_isShowMsg)
            {
                uiRichTextBox1.Invoke(new Action(() =>
                {
                    uiRichTextBox1.AppendText(ValueHelper.GetHextStrWithOx(datas) + Environment.NewLine);
                }));
            }
        }

        private void dgvChConfigDataGridView_SelectionChanged(
            object sender, EventArgs e)
        {
            dgvChConfig.dataGridView.ClearSelection();
        }

        private void FormDataViewer_Load(object sender, EventArgs e)
        {
            DataGridViewClearSelection();
        }

        private void FormDataViewer_Closed(object sender, EventArgs e)
        {
            if (Tps929120Ti != null)
                Tps929120Ti.Dispose();
            if (ControllerWith56Pin != null)
                ControllerWith56Pin.Dispose();
        }

        private void DataGridViewClearSelection()
        {
            dgvChConfig.dataGridView.ClearSelection();
            dgvIoutPwm.dataGridView.ClearSelection();
            dgvOtherConfig.dataGridView.ClearSelection();

            dgvChConfig.dataGridView.EndEdit();
            dgvIoutPwm.dataGridView.EndEdit();
            dgvOtherConfig.dataGridView.EndEdit();
        }

        private static void ledPara_dataGridView_CellContentClick(
            object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null)
                return;

            dgv.ClearSelection();

            var btn = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
            if (btn == null)
                return;

            if (btn.FormattedValue != null && !string.IsNullOrEmpty(btn.FormattedValue.ToString()))
            {
                var str = btn.FormattedValue.ToString();
                if (str.StartsWith("Disable"))
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Enable";
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.Style.BackColor = Color.DarkGreen;
                }
                else if (str.StartsWith("Enable"))
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Disable";
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.Style.BackColor = Color.DarkRed;
                }
            }
        }

        private async void func_Click(object sender, EventArgs e)
        {
            mainPanel.Enabled = false;
            var btn = sender as Button;
            if (btn != null)
            {
                await Task.Run(() =>
                {
                    if (btn.Text == @"获取在线芯片连接")
                    {
                        RefreshTps();
                        ShowRegDefaultValue();
                    }
                    else if (btn.Text == @"遍历读取芯片参数")
                    {
                        RefreshTps();

                        var keys = _onlineList.Keys.ToList();

                        uiRichTextBox1.Clear();
                        _isShowMsg = true;

                        foreach (var key in keys.Where(key => _onlineList[key]))
                        {
                            Tps929120Ti.ReadSingleDevReg(key.ToString(), "80");
                            Tps929120Ti.ReadSingleDevReg(key.ToString(), "88");

                            Tps929120Ti.ReadSingleDevReg(key.ToString(), "A0");
                            Tps929120Ti.ReadSingleDevReg(key.ToString(), "A8");

                            Tps929120Ti.ReadSingleDevReg(key.ToString(), "C0");
                            Tps929120Ti.ReadSingleDevReg(key.ToString(), "C8");
                        }

                        ShowRegValues();

                        _isShowMsg = false;
                    }
                    else if (btn.Text == @"读取单芯片参数")
                    {
                        if (btnOnOffLine.BackColor == Color.DarkGreen)
                        {
                            var key = cmbAddrList.comboBox.SelectedIndex;

                            //Tps929120Ti.ReadSingleDevReg(ValueHelper.GetHextStr((byte)key), "80");
                            //Tps929120Ti.ReadSingleDevReg(ValueHelper.GetHextStr((byte)key), "88");

                            //Tps929120Ti.ReadSingleDevReg(ValueHelper.GetHextStr((byte)key), "A0");
                            //Tps929120Ti.ReadSingleDevReg(ValueHelper.GetHextStr((byte)key), "A8");

                            //Tps929120Ti.ReadSingleDevReg(ValueHelper.GetHextStr((byte)key), "C0");
                            //Tps929120Ti.ReadSingleDevReg(ValueHelper.GetHextStr((byte)key), "C8");

                            Tps929120Ti.ReadSingleDevReg(key.ToString(), "80");
                            Tps929120Ti.ReadSingleDevReg(key.ToString(), "88");

                            Tps929120Ti.ReadSingleDevReg(key.ToString(), "A0");
                            Tps929120Ti.ReadSingleDevReg(key.ToString(), "A8");

                            Tps929120Ti.ReadSingleDevReg(key.ToString(), "C0");
                            Tps929120Ti.ReadSingleDevReg(key.ToString(), "C8");

                            RefreshTps(key);
                            ShowRegValues();
                        }
                        else
                        {
                            MessageBox.Show(@"当前地址芯片未连接");
                        }
                    }
                    else if (btn.Text == @"LED控制")
                        LedControl();
                });
            }

            mainPanel.Enabled = true;
            DataGridViewClearSelection();
        }

        private void ledBtn_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            var str = btn.Text;

            var fi = Tps929120Ti.GetType().GetField(str);
            if (fi == null) return;
            var fiValue = fi.GetValue(Tps929120Ti);
            if (fiValue == null) return;
            var isOn = (bool)fiValue;
            fi.SetValue(Tps929120Ti, !isOn);
            btn.BackColor = isOn ? Color.DarkGoldenrod : Color.DarkGreen;
        }

        private void SetCmbInDataGridView(
            string namme,
            DataGridView dgv, int rowIndex, int cellIndex,
            string nullValue, List<string> dataSource, string note = "")
        {
            _otherParasPosion.Add(namme, new[] { rowIndex, cellIndex });

            var comboxDelay = dgv.Rows[rowIndex].Cells[cellIndex] as DataGridViewComboBoxCell;
            var dataGridViewCellStyleDelay = new DataGridViewCellStyle { NullValue = nullValue };
            if (comboxDelay == null)
                return;
            comboxDelay.Style = dataGridViewCellStyleDelay;
            comboxDelay.DataSource = dataSource;
            comboxDelay.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;

            dgv.Rows[rowIndex].Cells[cellIndex - 1].Style.Font = new Font("黑体", 10, FontStyle.Bold, GraphicsUnit.Point, 120);

            dgv.Rows[rowIndex].Cells[cellIndex + 1].Value = note;
            dgv.Rows[rowIndex].Cells[cellIndex + 1].Style.Font = new Font("宋体", 7, FontStyle.Regular);
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cmb = sender as ComboBox;
            if (cmb != null)
            {
                var currentIndex = cmb.SelectedIndex;
                if (_onlineList.ContainsKey(currentIndex))
                {
                    if (_onlineList[currentIndex])
                    {
                        btnOnOffLine.BackColor = Color.DarkGreen;
                        btnOnOffLine.Text = @"online".ToUpper();
                    }
                    else
                    {
                        btnOnOffLine.BackColor = Color.DarkRed;
                        btnOnOffLine.Text = @"offline".ToUpper();
                    }
                }
                else
                {
                    btnOnOffLine.BackColor = Color.DarkRed;
                    btnOnOffLine.Text = @"offline".ToUpper();
                }

                ShowRegValues(false);
            }
        }

        #region TPS929120TI Func

        private void RefreshTps(int index = -1)
        {
            var keys = _onlineList.Keys.ToList();

            if (index == -1)
            {
                foreach (var key in keys)
                {
                    _onlineList[key] = false;

                    Tps929120Ti.ReadSingleDevReg(key.ToString(), "CF");

                    var field =
                        Tps929120Ti.GetType()
                            .GetField(string.Format("DevAddr{0}hRegAddrCFhEepm15",
                                ValueHelper.GetHextStr((byte)key).ToUpper()));
                    if (field == null)
                        continue;
                    var fieldValue = field.GetValue(Tps929120Ti);
                    if (fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString()))
                        _onlineList[key] = true;
                }
            }
            else
            {
                _onlineList[index] = false;

                Tps929120Ti.ReadSingleDevReg(index.ToString(), "CF");

                var field =
                    Tps929120Ti.GetType()
                        .GetField(string.Format("DevAddr{0}hRegAddrCFhEepm15",
                            ValueHelper.GetHextStr((byte)index).ToUpper()));
                if (field != null)
                {
                    var fieldValue = field.GetValue(Tps929120Ti);
                    if (fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString()))
                        _onlineList[index] = true;
                }

            }

            var currentIndex = cmbAddrList.comboBox.SelectedIndex;
            if (_onlineList.ContainsKey(currentIndex))
            {
                if (_onlineList[currentIndex])
                {
                    btnOnOffLine.BackColor = Color.DarkGreen;
                    btnOnOffLine.Text = @"online".ToUpper();
                }
                else
                {
                    btnOnOffLine.BackColor = Color.DarkRed;
                    btnOnOffLine.Text = @"offline".ToUpper();
                }
                // btnOnOffLine
            }
            else
            {
                btnOnOffLine.BackColor = Color.DarkRed;
                btnOnOffLine.Text = @"offline".ToUpper();
            }
        }

        private void ShowRegValues(bool isAutoLose = true)
        {
            if (btnOnOffLine.BackColor == Color.DarkGreen)
            {
                var devAddr = cmbAddrList.comboBox.SelectedIndex;
                var isConnectionLoss = false;

                #region PWM&&IOUT
                for (var i = 0; i < dgvIoutPwm.dataGridView.RowCount; i++)
                {
                    var row = dgvIoutPwm.dataGridView.Rows[i];

                    row.Cells[4].Value = string.Empty;
                    row.Cells[5].Value = string.Empty;

                    var regAddr = row.Cells[0].Value.ToString().Substring(0, 2);

                    var findFieldName = string.Format("DevAddr{0}hRegAddr{1}h",
                        ValueHelper.GetHextStr((byte)devAddr),
                        ValueHelper.GetHextStr(Convert.ToByte(regAddr, 16)));

                    foreach (
                        var fv in
                            from f in Tps929120Ti.GetType().GetFields()
                            where f.Name.StartsWith(findFieldName)
                            select f.GetValue(Tps929120Ti))
                    {
                        if (fv != null && !string.IsNullOrEmpty(fv.ToString()))
                        {
                            row.Cells[4].Value = ValueHelper.GetHextStr(Convert.ToByte(fv.ToString(), 16)) + "h";
                            row.Cells[5].Value = Convert.ToByte(fv.ToString(), 16).ToString();
                        }
                        else if (fv == null || string.IsNullOrEmpty(fv.ToString()))
                            isConnectionLoss = true;

                        break;
                    }
                }
                #endregion

                #region LED CH
                var bits = new List<string>();
                for (var ii = 192; ii <= 197; ii++)
                {
                    var regAddr = ValueHelper.GetHextStr((byte)ii);

                    var findFieldName = string.Format("DevAddr{0}hRegAddr{1}h",
                        ValueHelper.GetHextStr((byte)devAddr),
                        ValueHelper.GetHextStr(Convert.ToByte(regAddr, 16)));

                    foreach (
                        var fv in
                            from f in Tps929120Ti.GetType().GetFields()
                            where f.Name.StartsWith(findFieldName)
                            select f.GetValue(Tps929120Ti))
                    {
                        if (fv != null && !string.IsNullOrEmpty(fv.ToString()))
                        {
                            var bitStr = Convert.ToString(Convert.ToByte(fv.ToString(), 16), 2).PadLeft(8, '0');
                            for (var j = 7; j >= 0; j--)
                                bits.Add(bitStr[j].ToString());
                        }
                        else if (fv == null || string.IsNullOrEmpty(fv.ToString()))
                            isConnectionLoss = true;

                        break;
                    }
                }

                if (bits.Count == 48)
                {
                    var rowIndex = -2;
                    for (var ii = 0; ii < 3; ii++)
                    {
                        for (var j = 0; j < 16; j++)
                        {
                            if (j <= 11)
                            {
                                if (j == 0)
                                    rowIndex = rowIndex + 3;
                                else if (j == 4)
                                    rowIndex = rowIndex + 3;
                                else if (j == 8)
                                    rowIndex = rowIndex + 3;

                                var cellIndex = 0;
                                if (j == 0 || j == 4 || j == 8)
                                    cellIndex = 0;
                                else if (j == 1 || j == 5 || j == 9)
                                    cellIndex = 1;
                                else if (j == 2 || j == 6 || j == 10)
                                    cellIndex = 2;
                                else if (j == 3 || j == 7 || j == 11)
                                    cellIndex = 3;

                                dgvChConfig.dataGridView.Rows[rowIndex].Cells[cellIndex].Value = bits[ii * 16 + j] == "1"
                                    ? "Enable"
                                    : "Disable";
                                var btnValue =
                                    dgvChConfig.dataGridView.Rows[rowIndex].Cells[cellIndex] as DataGridViewButtonCell;
                                if (btnValue == null)
                                    continue;
                                btnValue.FlatStyle = FlatStyle.Flat;
                                btnValue.Style.BackColor = bits[ii * 16 + j] == "1" ? Color.DarkGreen : Color.DarkRed;
                            }
                        }
                    }
                }
                else
                {
                    isConnectionLoss = true;
                }
                #endregion

                #region 其它
                var c6 = GatRegStr(devAddr, "C6");
                var c7 = GatRegStr(devAddr, "C7");
                var c8 = GatRegStr(devAddr, "C8");
                var c9 = GatRegStr(devAddr, "C9");
                var ca = GatRegStr(devAddr, "CA");
                var cb = GatRegStr(devAddr, "CB");
                if (!string.IsNullOrEmpty(c6) && !string.IsNullOrEmpty(c7) && !string.IsNullOrEmpty(c8) &&
                    !string.IsNullOrEmpty(c9) && !string.IsNullOrEmpty(ca) && !string.IsNullOrEmpty(cb))
                {
                    var c6Bits =
                        Convert.ToString(Convert.ToByte(c6, 16), 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
                    var c7Bits =
                        Convert.ToString(Convert.ToByte(c7, 16), 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
                    var c8Bits =
                        Convert.ToString(Convert.ToByte(c8, 16), 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
                    var c9Bits =
                        Convert.ToString(Convert.ToByte(c9, 16), 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
                    var caBits =
                        Convert.ToString(Convert.ToByte(ca, 16), 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
                    var cbBits =
                        Convert.ToString(Convert.ToByte(cb, 16), 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();

                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Devaddr][0]].Cells[_otherParasPosion[Devaddr][1]].Value =
                        _eepDevaddr[
                            Convert.ToByte(string.Format("0000{0}{1}{2}{3}", c6Bits[3], c6Bits[2], c6Bits[1], c6Bits[0]), 2)
                            ];
                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Expen][0]].Cells[_otherParasPosion[Expen][1]]
                        .Value =
                        _eepExpen[
                            Convert.ToByte(string.Format("0000000{0}", c6Bits[4]), 2)
                            ];
                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Ldo][0]].Cells[_otherParasPosion[Ldo][1]].Value =
                        _eepLdo[
                            Convert.ToByte(string.Format("0000000{0}", c6Bits[6]), 2)
                            ];

                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Refrange][0]].Cells[_otherParasPosion[Refrange][1]].Value =
                        _eepRefRrange[
                            Convert.ToByte(string.Format("000000{0}{1}", c7Bits[1], c7Bits[0]), 2)
                            ];
                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Ofaf][0]].Cells[_otherParasPosion[Ofaf][1]].Value =
                        _eepOfaf[
                            Convert.ToByte(string.Format("0000000{0}", c7Bits[2]), 2)
                            ];
                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Intaddr][0]].Cells[_otherParasPosion[Intaddr][1]].Value =
                        _eepIntaddr[
                            Convert.ToByte(string.Format("0000000{0}", c7Bits[3]), 2)
                            ];
                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Pwnfreq][0]].Cells[_otherParasPosion[Pwnfreq][1]].Value =
                        _eepPwmfreq[
                            Convert.ToByte(string.Format("0000{0}{1}{2}{3}", c7Bits[7], c7Bits[6], c7Bits[5], c7Bits[4]), 2)
                            ];

                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Adclowsupth][0]].Cells[_otherParasPosion[Adclowsupth][1]].Value =
                        _eepAdclowsupth[
                            Convert.ToByte(string.Format("0000{0}{1}{2}{3}", c8Bits[3], c8Bits[2], c8Bits[1], c8Bits[0]), 2)
                            ];
                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Fltimeout][0]].Cells[_otherParasPosion[Fltimeout][1]].Value =
                        _eepFltimeout[
                            Convert.ToByte(string.Format("00000{0}{1}{2}", c8Bits[6], c8Bits[5], c8Bits[4]), 2)
                            ];

                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Odpw][0]].Cells[_otherParasPosion[Odpw][1]].Value =
                        _eepOdpw[
                            Convert.ToByte(string.Format("0000{0}{1}{2}{3}", c9Bits[3], c9Bits[2], c9Bits[1], c9Bits[0]), 2)
                            ];
                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Odiout][0]].Cells[_otherParasPosion[Odiout][1]].Value =
                        _eepOdiout[
                            Convert.ToByte(string.Format("0000{0}{1}{2}{3}", c9Bits[7], c9Bits[6], c9Bits[5], c9Bits[4]), 2)
                            ];

                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Inittimer][0]].Cells[_otherParasPosion[Inittimer][1]].Value =
                        _eepInittimer[
                            Convert.ToByte(string.Format("0000{0}{1}{2}{3}", caBits[3], caBits[2], caBits[1], caBits[0]), 2)
                            ];
                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Wdtimer][0]].Cells[_otherParasPosion[Wdtimer][1]].Value =
                        _eepWdtimer[
                            Convert.ToByte(string.Format("0000{0}{1}{2}{3}", caBits[7], caBits[6], caBits[5], caBits[4]), 2)
                            ];

                    dgvOtherConfig.dataGridView.Rows[_otherParasPosion[Adcshortth][0]].Cells[
                        _otherParasPosion[Adcshortth][1]].Value =
                        _eepAdcshortth[
                            Convert.ToByte(
                                string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", cbBits[7], cbBits[6], cbBits[5], cbBits[4],
                                    cbBits[3], cbBits[2], cbBits[1], cbBits[0]), 2)
                            ];
                }
                else
                {
                    isConnectionLoss = true;
                }
                #endregion

                if (isAutoLose)
                    if (isConnectionLoss)
                        ShowRegDefaultValue();
            }
            else
            {
                ShowRegDefaultValue();
            }
        }

        private void ShowRegDefaultValue()
        {
            for (var i = 0; i < dgvIoutPwm.dataGridView.RowCount; i++)
            {
                var row = dgvIoutPwm.dataGridView.Rows[i];

                row.Cells[4].Value = string.Empty;
                row.Cells[5].Value = string.Empty;
            }

            for (var i = 0; i < dgvChConfig.dataGridView.RowCount; i = i + 3)
            {
                var btnRowIndex = i + 1;

                for (var j = 0; j < 4; j++)
                {
                    var btnValue = dgvChConfig.dataGridView.Rows[btnRowIndex].Cells[j] as DataGridViewButtonCell;

                    if (btnValue == null)
                        continue;
                    if (i <= 8)
                    {
                        btnValue.FlatStyle = FlatStyle.Flat;
                        btnValue.Style.BackColor = Color.DarkRed;
                        dgvChConfig.dataGridView.Rows[btnRowIndex].Cells[j].Value = "Disable";
                    }
                    else
                    {
                        btnValue.FlatStyle = FlatStyle.Flat;
                        btnValue.Style.BackColor = Color.DarkGreen;
                        dgvChConfig.dataGridView.Rows[btnRowIndex].Cells[j].Value = "Enable";
                    }
                }
            }
        }

        private string GatRegStr(int devAddr, string regAddr)
        {
            var returnStr = string.Empty;

            var findFieldName = string.Format("DevAddr{0}hRegAddr{1}h",
                        ValueHelper.GetHextStr((byte)devAddr),
                        ValueHelper.GetHextStr(Convert.ToByte(regAddr, 16)));

            foreach (
                var fv in
                    from f in Tps929120Ti.GetType().GetFields()
                    where f.Name.StartsWith(findFieldName)
                    select f.GetValue(Tps929120Ti))
            {
                if (fv != null && !string.IsNullOrEmpty(fv.ToString()))
                    returnStr = fv.ToString();

                break;
            }

            return returnStr;
        }

        private void LedControl()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    using (
                        var ledForm = new Form
                        {
                            Width = 600,
                            Height = 600,
                            StartPosition = FormStartPosition.CenterScreen,
                            Text = @"LED Control"
                        })
                    {
                        Tps929120Ti.StartLedControl();

                        ledForm.MaximizeBox = false;
                        ledForm.Icon = FontImages.GetIcon(FontIcons.A_fa_lightbulb_o, 32,
                            Color.DodgerBlue);

                        var table = new TableLayoutPanel { Dock = DockStyle.Fill };
                        for (var i = 0; i < 1; i++)
                            table.ColumnStyles.Add(
                                new ColumnStyle(SizeType.Percent, 100));

                        var ledPanel = new TableLayoutPanel { Dock = DockStyle.Fill };
                        for (var i = 0; i < 2; i++)
                            ledPanel.ColumnStyles.Add(
                                new ColumnStyle(SizeType.Percent, 50));
                        for (var i = 0; i < 6; i++)
                            ledPanel.RowStyles.Add(
                                new RowStyle(SizeType.Percent, 16));

                        for (var i = 0; i < 12; i++)
                        {
                            var ledBtn = new Button
                            {
                                Dock = DockStyle.Fill,
                                BackColor = Color.DarkGoldenrod,
                                Text = string.Format("Led" + (i + 1)),
                                Font = new Font("微软雅黑", 12, FontStyle.Regular),
                                Margin = new Padding(1, 1, 1, 1)
                            };
                            ledBtn.Click += ledBtn_Click;

                            var col = i % 2;
                            var row = i / 2;

                            ledPanel.Controls.Add(ledBtn, col, row);
                        }

                        table.Controls.Add(ledPanel, 0, 0);
                        ledForm.Controls.Add(table);
                        ledForm.ShowDialog();

                        Tps929120Ti.StopLedControl();
                    }
                }));
            }
        }

        #endregion

        #region Para list
        private const string Ldo = "EPP_LDO";
        private const string Devaddr = "EPP_DEVADDR";
        private const string Expen = "EEP_EXPEN";
        private const string Pwnfreq = "EPP_PWMFREQ";
        private const string Intaddr = "EPP_INTADDR";
        private const string Ofaf = "EPP_OFAF";
        private const string Refrange = "EPP_REFRANGE";
        private const string Fltimeout = "EPP_FLTEMEOUT";
        private const string Adclowsupth = "EPP_ADCLOWSUPTH";
        private const string Odiout = "EPP_ODIOUT";
        private const string Odpw = "EPP_ODPW";
        private const string Wdtimer = "EPP_WDTIMER";
        private const string Inittimer = "EPP_INITTIMER";
        private const string Adcshortth = "EPP_ADCSHORTTH";

        private readonly List<string> _eepLdo = new List<string> { "5.0 V", "4.4 V" };
        private readonly List<string> _eepExpen = new List<string> { "Disable", "Enable" };
        private readonly List<string> _eepDevaddr = new List<string>();
        private readonly List<string> _eepPwmfreq = new List<string>
        {
            "200 Hz",
            "250 Hz",
            "300 Hz",
            "350 Hz",
            "400 Hz",
            "500 Hz",
            "600 Hz",
            "800 Hz",
            "1000 Hz",
            "1200 Hz",
            "2 kHz",
            "4 kHz",
            "5.9 kHz",
            "7.8 kHz",
            "9.6 kHz",
            "20.8 kHz"
        };
        private readonly List<string> _eepIntaddr = new List<string> { "Disable", "Enable" };
        private readonly List<string> _eepOfaf = new List<string> { "One-fails-others-on", "One-fails-all-fail" };
        private readonly List<string> _eepRefRrange = new List<string> { "64", "128", "256", "512", };
        private readonly List<string> _eepFltimeout = new List<string>
        {
            "1 ms",
            "125 µs",
            "250 µs",
            "500 µs",
            "1.25 ms",
            "2.5 ms",
            "5 ms",
            "10 ms"
        };
        private readonly List<string> _eepAdclowsupth = new List<string>
        {
            "5 V",
            "6 V",
            "7 V",
            "8 V",
            "9 V",
            "10 V",
            "11 V",
            "12 V",
            "13 V",
            "14 V",
            "15 V",
            "16 V",
            "17 V",
            "18 V",
            "19 V",
            "20 V",
        };
        private readonly List<string> _eepOdiout = new List<string>
        {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"
        };
        private readonly List<string> _eepOdpw = new List<string>
        {
            "100 µs",
            "20 µs",
            "30 µs",
            "50 µs",
            "80 µs",
            "150 µs",
            "200 µs",
            "300 µs",
            "500 µs",
            "800 µs",
            "1 ms",
            "1.2 ms",
            "1.5 ms",
            "2 ms",
            "3 ms",
            "5 ms"
        };
        private readonly List<string> _eepWdtimer = new List<string>
        {
            "Disabled",
            "200 µs",
            "500 µs",
            "1 ms",
            "2 ms",
            "5 ms",
            "10 ms",
            "20 ms",
            "50 ms",
            "100 ms",
            "200 ms",
            "500 ms",
            "Direct to FS",
            "Direct to FS",
            "Direct to FS",
            "Direct to FS"
        };
        private readonly List<string> _eepInittimer = new List<string>
        {
            "0 ms",
            "50 ms",
            "20 ms",
            "10 ms",
            "5 ms",
            "2 ms",
            "1 ms",
            "500 µs",
            "200 µs",
            "100 µs",
            "50 µs",
            "50 µs",
            "50 µs",
            "50 µs",
            "50 µs",
            "50 µs"
        };
        private readonly List<string> _eepAdcshortth = new List<string>();
        #endregion

        private void btnSaveParas_BtnClick(object sender, EventArgs e)
        {
            var fileDialog = new SaveFileDialog { Filter = @"文本文件|*.tps" };
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            var isCanSave = !(btnOnOffLine.BackColor != Color.DarkGreen);
            var str = string.Empty;

            for (var i = 0; i < dgvIoutPwm.dataGridView.RowCount; i++)
            {
                var row = dgvIoutPwm.dataGridView.Rows[i];

                if (row.Cells[4].Value == null)
                {
                    isCanSave = false;
                    break;
                }

                if (string.IsNullOrEmpty(row.Cells[4].Value.ToString()))
                {
                    isCanSave = false;
                    break;
                }

                str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32(row.Cells[0].Value.ToString().Substring(0, 2), 16),
                    Convert.ToInt32(row.Cells[4].Value.ToString().Substring(0, 2), 16));
            }

            var dgv = dgvChConfig.dataGridView;
            var c0Bits = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                GetIsEnable(dgv, 4, 3), GetIsEnable(dgv, 4, 2),
                GetIsEnable(dgv, 4, 1), GetIsEnable(dgv, 4, 0),
                GetIsEnable(dgv, 1, 3), GetIsEnable(dgv, 1, 2),
                GetIsEnable(dgv, 1, 1), GetIsEnable(dgv, 1, 0));
            var c1Bits = string.Format("0000{0}{1}{2}{3}",
                GetIsEnable(dgv, 7, 3), GetIsEnable(dgv, 7, 2),
                GetIsEnable(dgv, 7, 1), GetIsEnable(dgv, 7, 0));
            str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32("C0h".Substring(0, 2), 16),
                    Convert.ToInt32(c0Bits, 2));
            str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32("C1h".Substring(0, 2), 16),
                    Convert.ToInt32(c1Bits, 2));

            var c2Bits = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                GetIsEnable(dgv, 4 + 9, 3), GetIsEnable(dgv, 4 + 9, 2),
                GetIsEnable(dgv, 4 + 9, 1), GetIsEnable(dgv, 4 + 9, 0),
                GetIsEnable(dgv, 1 + 9, 3), GetIsEnable(dgv, 1 + 9, 2),
                GetIsEnable(dgv, 1 + 9, 1), GetIsEnable(dgv, 1 + 9, 0));
            var c3Bits = string.Format("0000{0}{1}{2}{3}",
                GetIsEnable(dgv, 7 + 9, 3), GetIsEnable(dgv, 7 + 9, 2),
                GetIsEnable(dgv, 7 + 9, 1), GetIsEnable(dgv, 7 + 9, 0));
            str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32("C2h".Substring(0, 2), 16),
                    Convert.ToInt32(c2Bits, 2));
            str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32("C3h".Substring(0, 2), 16),
                    Convert.ToInt32(c3Bits, 2));

            var c4Bits = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                GetIsEnable(dgv, 4 + 9 + 9, 3), GetIsEnable(dgv, 4 + 9 + 9, 2),
                GetIsEnable(dgv, 4 + 9 + 9, 1), GetIsEnable(dgv, 4 + 9 + 9, 0),
                GetIsEnable(dgv, 1 + 9 + 9, 3), GetIsEnable(dgv, 1 + 9 + 9, 2),
                GetIsEnable(dgv, 1 + 9 + 9, 1), GetIsEnable(dgv, 1 + 9 + 9, 0));
            var c5Bits = string.Format("0000{0}{1}{2}{3}",
                GetIsEnable(dgv, 7 + 9 + 9, 3), GetIsEnable(dgv, 7 + 9 + 9, 2),
                GetIsEnable(dgv, 7 + 9 + 9, 1), GetIsEnable(dgv, 7 + 9 + 9, 0));
            str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32("C4h".Substring(0, 2), 16),
                    Convert.ToInt32(c4Bits, 2));
            str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32("C5h".Substring(0, 2), 16),
                    Convert.ToInt32(c5Bits, 2));

            var dgv2 = dgvOtherConfig.dataGridView;
            var c6Bits = string.Format("0{0}0{1}{2}",
                GetCmbIndex(dgv2, _otherParasPosion[Ldo], _eepLdo, 1),
                GetCmbIndex(dgv2, _otherParasPosion[Expen], _eepExpen, 1),
                GetCmbIndex(dgv2, _otherParasPosion[Devaddr], _eepDevaddr, 4));

            var c7Bits = string.Format("{0}{1}{2}{3}",
                GetCmbIndex(dgv2, _otherParasPosion[Pwnfreq], _eepPwmfreq, 4),
                GetCmbIndex(dgv2, _otherParasPosion[Intaddr], _eepIntaddr, 1),
                GetCmbIndex(dgv2, _otherParasPosion[Ofaf], _eepOfaf, 1),
                GetCmbIndex(dgv2, _otherParasPosion[Refrange], _eepRefRrange, 2));

            var c8Bits = string.Format("0{0}{1}",
                GetCmbIndex(dgv2, _otherParasPosion[Fltimeout], _eepFltimeout, 3),
                GetCmbIndex(dgv2, _otherParasPosion[Adclowsupth], _eepAdclowsupth, 4));

            var c9Bits = string.Format("{0}{1}",
                GetCmbIndex(dgv2, _otherParasPosion[Odiout], _eepOdiout, 4),
                GetCmbIndex(dgv2, _otherParasPosion[Odpw], _eepOdpw, 4));

            var caBits = string.Format("{0}{1}",
                GetCmbIndex(dgv2, _otherParasPosion[Wdtimer], _eepWdtimer, 4),
                GetCmbIndex(dgv2, _otherParasPosion[Inittimer], _eepInittimer, 4));

            var cbBits = string.Format("{0}",
                GetCmbIndex(dgv2, _otherParasPosion[Adcshortth], _eepAdcshortth, 8));

            str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32("C6h".Substring(0, 2), 16),
                    Convert.ToInt32(c6Bits, 2));
            str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32("C7h".Substring(0, 2), 16),
                    Convert.ToInt32(c7Bits, 2));
            str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32("C8h".Substring(0, 2), 16),
                    Convert.ToInt32(c8Bits, 2));
            str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32("C9h".Substring(0, 2), 16),
                    Convert.ToInt32(c9Bits, 2));
            str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32("CAh".Substring(0, 2), 16),
                    Convert.ToInt32(caBits, 2));
            str += string.Format("{0}:{1}\r\n",
                    Convert.ToInt32("CBh".Substring(0, 2), 16),
                    Convert.ToInt32(cbBits, 2));

            str += "204:0\r\n205:0\r\n206:0";

            try
            {
                if (isCanSave)
                {
                    var filePath = fileDialog.FileName;

                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    WriteTxt(filePath, str);
                    MessageBox.Show(@"保存成功");
                }
                else
                {
                    MessageBox.Show(@"保存失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"保存失败：" + ex.Message);
            }
        }

        private static string GetIsEnable(
            DataGridView dgv, int rowIndex, int cellIndex)
        {
            var str = string.Empty;

            var btnValue = dgv.Rows[rowIndex].Cells[cellIndex] as DataGridViewButtonCell;
            if (btnValue != null)
                str = btnValue.Style.BackColor == Color.DarkGreen ? 1.ToString() : 0.ToString();

            return str;
        }

        private static string GetCmbIndex(
            DataGridView dgv, IReadOnlyList<int> pos, List<string> dataList, int len)
        {
            var str = string.Empty;
            for (var i = 0; i < len; i++)
                str += "0";

            var cmb = dgv.Rows[pos[0]].Cells[pos[1]] as DataGridViewComboBoxCell;
            if (cmb == null || cmb.FormattedValue == null ||
                string.IsNullOrEmpty(cmb.FormattedValueType.ToString()))
                return str;
            var value = cmb.FormattedValue.ToString();

            var findIndex = dataList.FindIndex(f => f == value);
            if (findIndex != -1)
                str = Convert.ToString(findIndex, 2).PadLeft(len, '0');

            return str;
        }

        private static void WriteTxt(string filePath, string value)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    var bs = Encoding.ASCII.GetBytes(value);
                    fs.Write(bs, 0, bs.Length);
                }

                //Thread.Sleep(25);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnWriteThisTps_BtnClick(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"确认？", @"请确认", MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            DataGridViewClearSelection();
            mainPanel.Enabled = false;


            await Task.Run(() =>
            {
                var isCanSave = !(btnOnOffLine.BackColor != Color.DarkGreen);
                var str = string.Empty;

                for (var i = 0; i < dgvIoutPwm.dataGridView.RowCount; i++)
                {
                    var row = dgvIoutPwm.dataGridView.Rows[i];

                    if (row.Cells[4].Value == null)
                    {
                        isCanSave = false;
                        break;
                    }

                    if (string.IsNullOrEmpty(row.Cells[4].Value.ToString()))
                    {
                        isCanSave = false;
                        break;
                    }

                    str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32(row.Cells[0].Value.ToString().Substring(0, 2), 16),
                        Convert.ToInt32(row.Cells[4].Value.ToString().Substring(0, 2), 16));
                }

                var dgv = dgvChConfig.dataGridView;
                var c0Bits = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                    GetIsEnable(dgv, 4, 3), GetIsEnable(dgv, 4, 2),
                    GetIsEnable(dgv, 4, 1), GetIsEnable(dgv, 4, 0),
                    GetIsEnable(dgv, 1, 3), GetIsEnable(dgv, 1, 2),
                    GetIsEnable(dgv, 1, 1), GetIsEnable(dgv, 1, 0));
                var c1Bits = string.Format("0000{0}{1}{2}{3}",
                    GetIsEnable(dgv, 7, 3), GetIsEnable(dgv, 7, 2),
                    GetIsEnable(dgv, 7, 1), GetIsEnable(dgv, 7, 0));
                str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32("C0h".Substring(0, 2), 16),
                        Convert.ToInt32(c0Bits, 2));
                str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32("C1h".Substring(0, 2), 16),
                        Convert.ToInt32(c1Bits, 2));

                var c2Bits = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                    GetIsEnable(dgv, 4 + 9, 3), GetIsEnable(dgv, 4 + 9, 2),
                    GetIsEnable(dgv, 4 + 9, 1), GetIsEnable(dgv, 4 + 9, 0),
                    GetIsEnable(dgv, 1 + 9, 3), GetIsEnable(dgv, 1 + 9, 2),
                    GetIsEnable(dgv, 1 + 9, 1), GetIsEnable(dgv, 1 + 9, 0));
                var c3Bits = string.Format("0000{0}{1}{2}{3}",
                    GetIsEnable(dgv, 7 + 9, 3), GetIsEnable(dgv, 7 + 9, 2),
                    GetIsEnable(dgv, 7 + 9, 1), GetIsEnable(dgv, 7 + 9, 0));
                str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32("C2h".Substring(0, 2), 16),
                        Convert.ToInt32(c2Bits, 2));
                str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32("C3h".Substring(0, 2), 16),
                        Convert.ToInt32(c3Bits, 2));

                var c4Bits = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                    GetIsEnable(dgv, 4 + 9 + 9, 3), GetIsEnable(dgv, 4 + 9 + 9, 2),
                    GetIsEnable(dgv, 4 + 9 + 9, 1), GetIsEnable(dgv, 4 + 9 + 9, 0),
                    GetIsEnable(dgv, 1 + 9 + 9, 3), GetIsEnable(dgv, 1 + 9 + 9, 2),
                    GetIsEnable(dgv, 1 + 9 + 9, 1), GetIsEnable(dgv, 1 + 9 + 9, 0));
                var c5Bits = string.Format("0000{0}{1}{2}{3}",
                    GetIsEnable(dgv, 7 + 9 + 9, 3), GetIsEnable(dgv, 7 + 9 + 9, 2),
                    GetIsEnable(dgv, 7 + 9 + 9, 1), GetIsEnable(dgv, 7 + 9 + 9, 0));
                str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32("C4h".Substring(0, 2), 16),
                        Convert.ToInt32(c4Bits, 2));
                str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32("C5h".Substring(0, 2), 16),
                        Convert.ToInt32(c5Bits, 2));

                var dgv2 = dgvOtherConfig.dataGridView;
                var c6Bits = string.Format("0{0}0{1}{2}",
                    GetCmbIndex(dgv2, _otherParasPosion[Ldo], _eepLdo, 1),
                    GetCmbIndex(dgv2, _otherParasPosion[Expen], _eepExpen, 1),
                    GetCmbIndex(dgv2, _otherParasPosion[Devaddr], _eepDevaddr, 4));

                var c7Bits = string.Format("{0}{1}{2}{3}",
                    GetCmbIndex(dgv2, _otherParasPosion[Pwnfreq], _eepPwmfreq, 4),
                    GetCmbIndex(dgv2, _otherParasPosion[Intaddr], _eepIntaddr, 1),
                    GetCmbIndex(dgv2, _otherParasPosion[Ofaf], _eepOfaf, 1),
                    GetCmbIndex(dgv2, _otherParasPosion[Refrange], _eepRefRrange, 2));

                var c8Bits = string.Format("0{0}{1}",
                    GetCmbIndex(dgv2, _otherParasPosion[Fltimeout], _eepFltimeout, 3),
                    GetCmbIndex(dgv2, _otherParasPosion[Adclowsupth], _eepAdclowsupth, 4));

                var c9Bits = string.Format("{0}{1}",
                    GetCmbIndex(dgv2, _otherParasPosion[Odiout], _eepOdiout, 4),
                    GetCmbIndex(dgv2, _otherParasPosion[Odpw], _eepOdpw, 4));

                var caBits = string.Format("{0}{1}",
                    GetCmbIndex(dgv2, _otherParasPosion[Wdtimer], _eepWdtimer, 4),
                    GetCmbIndex(dgv2, _otherParasPosion[Inittimer], _eepInittimer, 4));

                var cbBits = string.Format("{0}",
                    GetCmbIndex(dgv2, _otherParasPosion[Adcshortth], _eepAdcshortth, 8));

                str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32("C6h".Substring(0, 2), 16),
                        Convert.ToInt32(c6Bits, 2));
                str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32("C7h".Substring(0, 2), 16),
                        Convert.ToInt32(c7Bits, 2));
                str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32("C8h".Substring(0, 2), 16),
                        Convert.ToInt32(c8Bits, 2));
                str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32("C9h".Substring(0, 2), 16),
                        Convert.ToInt32(c9Bits, 2));
                str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32("CAh".Substring(0, 2), 16),
                        Convert.ToInt32(caBits, 2));
                str += string.Format("{0}:{1}\r\n",
                        Convert.ToInt32("CBh".Substring(0, 2), 16),
                        Convert.ToInt32(cbBits, 2));

                str += "204:0\r\n205:0\r\n206:0";

                var originFile = Tps929120Ti.DataFilePath;
                var tempFile = string.Format(@"{0}.tps", Guid.NewGuid().ToString().Replace("-", ""));

                try
                {
                    if (isCanSave)
                    {
                        var filePath = tempFile;

                        if (File.Exists(filePath))
                            File.Delete(filePath);
                        WriteTxt(filePath, str);

                        Tps929120Ti.DataFilePath = tempFile;
                        Tps929120Ti.SingleErgodicProgramTpsFromDataFile(cmbAddrList.comboBox.SelectedIndex.ToString());

                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        if (Tps929120Ti.ErgodicProgramTpsCount == 1 &&
                            Tps929120Ti.ErgodicProgramTpsDevAddrs ==
                            cmbAddrList.comboBox.SelectedIndex.ToString().PadLeft(2, '0'))
                        {
                            ShowRegValues();
                            MessageBox.Show(string.Format(@"更新成功，耗时：{0}", Tps929120Ti.ErgodicProgramTpsCostMs));
                        }
                        else
                            MessageBox.Show(@"更新失败");
                    }
                    else
                    {
                        MessageBox.Show(@"更新失败");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"更新失败：" + ex.Message);
                }
                finally
                {
                    Tps929120Ti.DataFilePath = originFile;
                }
            });

            mainPanel.Enabled = true;
            DataGridViewClearSelection();
        }
    }
}
