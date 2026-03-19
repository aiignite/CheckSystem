using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Controller;
using HZH_Controls.Controls.Btn;
using HZH_Controls.IconFont;
using UserControls;

namespace CheckSystem.HelperForms.BD18331
{
    public partial class Bd18331FormLedControl : Form
    {
        private Bd18331Euv Bd18331Euv { get; set; }
        protected ControllerBase Gw1 { get; set; }

        private readonly Dictionary<string, UserDataGrid> _userDataGrids = new Dictionary<string, UserDataGrid>();
        private readonly List<string> _listPwm = new List<string>();
        private readonly List<string> _listDimSet = new List<string>();
        private readonly Color _chOnColor = Color.LightSeaGreen;
        private readonly Color _chOffColor = Color.LightGoldenrodYellow;
        private Thread _readStatusTh;
        private string _currentDev;

        public Bd18331FormLedControl(IReadOnlyList<string> devs, Bd18331Euv bd18331Euv,
            SyControllerWith56Pin controller)
        {
            InitializeComponent();
            BdInit(devs, bd18331Euv, controller);
            // Icon = FontImages.GetIcon(FontIcons.A_fa_plug, 32,
            //     Color.DodgerBlue);
            // Text = @"BD18331EUV-点灯控制程序";
            // Bd18331Euv = bd18331Euv;
            // Gw1 = controller;
            // Closed += Bd18331FormLedControl_Closed;
            // mainTab.SelectedIndexChanged += mainTab_SelectedIndexChanged;
            //
            // for (var i = 100; i > 0; i--)
            //     _listPwm.Add(i.ToString());
            //
            // foreach (var d in devs)
            // {
            //     var tablePanel = new TableLayoutPanel { Dock = DockStyle.Fill };
            //     for (var i = 0; i < 2; i++)
            //         tablePanel.ColumnStyles.Add(
            //             new ColumnStyle(SizeType.Percent, 50));
            //
            //     tablePanel.RowStyles.Add(
            //         new RowStyle(SizeType.Percent, 100));
            //     tablePanel.RowStyles.Add(
            //         new RowStyle(SizeType.Absolute, 100));
            //
            //     var btnOpen = new UCBtnExt
            //     {
            //         Dock = DockStyle.Fill,
            //         BtnText = "一键全开,通道1~24",
            //         FillColor = _chOnColor,
            //         Margin = new Padding(5)
            //     };
            //
            //     var btnClose = new UCBtnExt
            //     {
            //         Dock = DockStyle.Fill,
            //         BtnText = "一键全关,通道1~24",
            //         FillColor = _chOffColor,
            //         Margin = new Padding(5)
            //     };
            //
            //     btnOpen.BtnClick += btnOpen_Click;
            //     btnClose.BtnClick += btnClose_Click;
            //     tablePanel.Controls.Add(btnOpen, 0, 1);
            //     tablePanel.Controls.Add(btnClose, 1, 1);
            //
            //     for (var k = 0; k < 2; k++)
            //     {
            //         var str = string.Format(k == 0 ? "芯片：{0} 通道1~12" : "芯片：{0} 通道13~24", d);
            //         var userDgv = new UserDataGrid
            //         {
            //             Dock = DockStyle.Fill,
            //             label = { Text = str, Height = 15 },
            //             dataGridView =
            //             {
            //                 ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
            //                 AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            //                 DefaultCellStyle = { WrapMode = DataGridViewTriState.True },
            //                 AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
            //                 AllowUserToResizeColumns = false,
            //                 AllowUserToResizeRows = false,
            //                 AllowUserToAddRows = false,
            //                 AllowUserToDeleteRows = false,
            //                 ReadOnly = false,
            //                 RowHeadersVisible = false,
            //                 MultiSelect = false,
            //                 RowsDefaultCellStyle = { Font = new Font("微软雅黑", 9.5f, FontStyle.Regular) },
            //                 ColumnHeadersDefaultCellStyle = { Font = new Font("微软雅黑", 8, FontStyle.Regular) },
            //             }
            //         };
            //
            //         userDgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "通道" });
            //         userDgv.dataGridView.Columns.Add(new DataGridViewComboBoxColumn { Name = "占空比数值/%" });
            //         userDgv.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "设置占空比" });
            //         userDgv.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "切换开关" });
            //         userDgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShortError" });
            //         userDgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "OpenError" });
            //
            //         for (var i = 0; i < 12; i++)
            //         {
            //             if (k == 0)
            //             {
            //                 userDgv.dataGridView.Rows.Add(string.Format("通道{0}", i + 1));
            //             }
            //             else
            //             {
            //                 userDgv.dataGridView.Rows.Add(string.Format("通道{0}", i + 1 + 12));
            //             }
            //
            //             var comboxDelay = userDgv.dataGridView.Rows[i].Cells[1] as DataGridViewComboBoxCell;
            //             var dataGridViewCellStyleDelay = new DataGridViewCellStyle { NullValue = _listPwm[0] };
            //             if (comboxDelay != null)
            //             {
            //                 comboxDelay.Style = dataGridViewCellStyleDelay;
            //                 comboxDelay.DataSource = _listPwm;
            //                 comboxDelay.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            //             }
            //
            //             userDgv.dataGridView.Rows[i].Cells[2].Value = "设置PWM";
            //             userDgv.dataGridView.Rows[i].Cells[3].Value = "切换";
            //
            //             userDgv.dataGridView.Rows[i].DefaultCellStyle.BackColor = _chOffColor;
            //         }
            //
            //         userDgv.dataGridView.CellContentClick += dataGridView_CellContentClick;
            //         tablePanel.Controls.Add(userDgv, k, 0);
            //         _userDataGrids.Add(str, userDgv);
            //     }
            //
            //     var newTab = new TabPage
            //     {
            //         Text = string.Format("芯片：{0}", d),
            //         Font = new Font("微软雅黑", 16, FontStyle.Regular),
            //         Dock = DockStyle.Fill
            //     };
            //     newTab.Controls.Add(tablePanel);
            //     mainTab.Controls.Add(newTab);
            // }
            //
            // // 0dec, 0x00h, 000000b
            // _currentDev = string.Format("芯片：{0}", devs[0]);
            // if (_readStatusTh != null)
            // {
            //     _readStatusTh.Abort();
            //     _readStatusTh.Join();
            // }
            //
            // _readStatusTh = new Thread(ReadStatus);
            // _readStatusTh.Start();
        }

        public Bd18331FormLedControl(IReadOnlyList<string> devs, Bd18331Euv bd18331Euv,
            SyRenesasMcuControllerMaster controller)
        {
            InitializeComponent();
            BdInit(devs, bd18331Euv, controller);
            // Icon = FontImages.GetIcon(FontIcons.A_fa_plug, 32,
            //     Color.DodgerBlue);
            // Text = @"BD18331EUV-点灯控制程序";
            // Bd18331Euv = bd18331Euv;
            // Gw1 = controller;
            // Closed += Bd18331FormLedControl_Closed;
            // mainTab.SelectedIndexChanged += mainTab_SelectedIndexChanged;
            //
            // for (var i = 100; i > 0; i--)
            //     _listPwm.Add(i.ToString());
            //
            // foreach (var d in devs)
            // {
            //     var tablePanel = new TableLayoutPanel { Dock = DockStyle.Fill };
            //     for (var i = 0; i < 2; i++)
            //         tablePanel.ColumnStyles.Add(
            //             new ColumnStyle(SizeType.Percent, 50));
            //
            //     tablePanel.RowStyles.Add(
            //         new RowStyle(SizeType.Percent, 100));
            //     tablePanel.RowStyles.Add(
            //         new RowStyle(SizeType.Absolute, 100));
            //
            //     var btnOpen = new UCBtnExt
            //     {
            //         Dock = DockStyle.Fill,
            //         BtnText = "一键全开,通道1~24",
            //         FillColor = _chOnColor,
            //         Margin = new Padding(5)
            //     };
            //
            //     var btnClose = new UCBtnExt
            //     {
            //         Dock = DockStyle.Fill,
            //         BtnText = "一键全关,通道1~24",
            //         FillColor = _chOffColor,
            //         Margin = new Padding(5)
            //     };
            //
            //     btnOpen.BtnClick += btnOpen_Click;
            //     btnClose.BtnClick += btnClose_Click;
            //     tablePanel.Controls.Add(btnOpen, 0, 1);
            //     tablePanel.Controls.Add(btnClose, 1, 1);
            //
            //     for (var k = 0; k < 2; k++)
            //     {
            //         var str = string.Format(k == 0 ? "芯片：{0} 通道1~12" : "芯片：{0} 通道13~24", d);
            //         var userDgv = new UserDataGrid
            //         {
            //             Dock = DockStyle.Fill,
            //             label = { Text = str, Height = 15 },
            //             dataGridView =
            //             {
            //                 ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
            //                 AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            //                 DefaultCellStyle = { WrapMode = DataGridViewTriState.True },
            //                 AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
            //                 AllowUserToResizeColumns = false,
            //                 AllowUserToResizeRows = false,
            //                 AllowUserToAddRows = false,
            //                 AllowUserToDeleteRows = false,
            //                 ReadOnly = false,
            //                 RowHeadersVisible = false,
            //                 MultiSelect = false,
            //                 RowsDefaultCellStyle = { Font = new Font("微软雅黑", 9.5f, FontStyle.Regular) },
            //                 ColumnHeadersDefaultCellStyle = { Font = new Font("微软雅黑", 8, FontStyle.Regular) },
            //             }
            //         };
            //
            //         userDgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "通道" });
            //         userDgv.dataGridView.Columns.Add(new DataGridViewComboBoxColumn { Name = "占空比数值/%" });
            //         userDgv.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "设置占空比" });
            //         userDgv.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "切换开关" });
            //         userDgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShortError" });
            //         userDgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "OpenError" });
            //
            //         for (var i = 0; i < 12; i++)
            //         {
            //             if (k == 0)
            //             {
            //                 userDgv.dataGridView.Rows.Add(string.Format("通道{0}", i + 1));
            //             }
            //             else
            //             {
            //                 userDgv.dataGridView.Rows.Add(string.Format("通道{0}", i + 1 + 12));
            //             }
            //
            //             var comboxDelay = userDgv.dataGridView.Rows[i].Cells[1] as DataGridViewComboBoxCell;
            //             var dataGridViewCellStyleDelay = new DataGridViewCellStyle { NullValue = _listPwm[0] };
            //             if (comboxDelay != null)
            //             {
            //                 comboxDelay.Style = dataGridViewCellStyleDelay;
            //                 comboxDelay.DataSource = _listPwm;
            //                 comboxDelay.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            //             }
            //
            //             userDgv.dataGridView.Rows[i].Cells[2].Value = "设置PWM";
            //             userDgv.dataGridView.Rows[i].Cells[3].Value = "切换";
            //
            //             userDgv.dataGridView.Rows[i].DefaultCellStyle.BackColor = _chOffColor;
            //         }
            //
            //         userDgv.dataGridView.CellContentClick += dataGridView_CellContentClick;
            //         tablePanel.Controls.Add(userDgv, k, 0);
            //         _userDataGrids.Add(str, userDgv);
            //     }
            //
            //     var newTab = new TabPage
            //     {
            //         Text = string.Format("芯片：{0}", d),
            //         Font = new Font("微软雅黑", 16, FontStyle.Regular),
            //         Dock = DockStyle.Fill
            //     };
            //     newTab.Controls.Add(tablePanel);
            //     mainTab.Controls.Add(newTab);
            // }
            //
            // // 0dec, 0x00h, 000000b
            // _currentDev = string.Format("芯片：{0}", devs[0]);
            // if (_readStatusTh != null)
            // {
            //     _readStatusTh.Abort();
            //     _readStatusTh.Join();
            // }
            //
            // _readStatusTh = new Thread(ReadStatus);
            // _readStatusTh.Start();
        }

        private void BdInit(IReadOnlyList<string> devs, Bd18331Euv bd18331Euv, ControllerBase controller)
        {
            Icon = FontImages.GetIcon(FontIcons.A_fa_plug, 32,
                Color.DodgerBlue);
            Text = @"BD18331EUV-点灯控制程序";
            Bd18331Euv = bd18331Euv;
            Gw1 = controller;
            Closed += Bd18331FormLedControl_Closed;
            mainTab.SelectedIndexChanged += mainTab_SelectedIndexChanged;

            for (var i = 100; i > 0; i--)
                _listPwm.Add(i.ToString());

            _listDimSet.Add("3.75");
            _listDimSet.Add("7.50");
            _listDimSet.Add("11.25");
            _listDimSet.Add("15.00");
            _listDimSet.Add("18.75");
            _listDimSet.Add("22.50");
            _listDimSet.Add("26.25");
            _listDimSet.Add("30.00");
            _listDimSet.Add("33.75");
            _listDimSet.Add("37.50");
            _listDimSet.Add("41.25");
            _listDimSet.Add("45.00");
            _listDimSet.Add("48.75");
            _listDimSet.Add("52.50");
            _listDimSet.Add("56.25");
            _listDimSet.Add("60.00");

            foreach (var d in devs)
            {
                var tablePanel = new TableLayoutPanel { Dock = DockStyle.Fill };
                for (var i = 0; i < 2; i++)
                    tablePanel.ColumnStyles.Add(
                        new ColumnStyle(SizeType.Percent, 50));

                tablePanel.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 100));
                tablePanel.RowStyles.Add(
                    new RowStyle(SizeType.Absolute, 100));

                var btnOpen = new UCBtnExt
                {
                    Dock = DockStyle.Fill,
                    BtnText = "一键全开,通道1~24",
                    FillColor = _chOnColor,
                    Margin = new Padding(5)
                };

                var btnClose = new UCBtnExt
                {
                    Dock = DockStyle.Fill,
                    BtnText = "一键全关,通道1~24",
                    FillColor = _chOffColor,
                    Margin = new Padding(5)
                };

                btnOpen.BtnClick += btnOpen_Click;
                btnClose.BtnClick += btnClose_Click;
                tablePanel.Controls.Add(btnOpen, 0, 1);
                tablePanel.Controls.Add(btnClose, 1, 1);

                for (var k = 0; k < 2; k++)
                {
                    var str = string.Format(k == 0 ? "芯片：{0} 通道1~12" : "芯片：{0} 通道13~24", d);
                    var userDgv = new UserDataGrid
                    {
                        Dock = DockStyle.Fill,
                        label = { Text = str, Height = 15 },
                        dataGridView =
                        {
                            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                            DefaultCellStyle = { WrapMode = DataGridViewTriState.True },
                            //AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                            AllowUserToResizeColumns = true,
                            AllowUserToResizeRows = false,
                            AllowUserToAddRows = false,
                            AllowUserToDeleteRows = false,
                            ReadOnly = false,
                            RowHeadersVisible = false,
                            MultiSelect = false,
                            RowsDefaultCellStyle = { Font = new Font("微软雅黑", 9.5f, FontStyle.Regular) },
                            ColumnHeadersDefaultCellStyle = { Font = new Font("微软雅黑", 8, FontStyle.Regular) },
                        }
                    };

                    userDgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "通道", ReadOnly = true, Width = 10 });
                    userDgv.dataGridView.Columns.Add(new DataGridViewComboBoxColumn { Name = "占空比数值/%" });
                    userDgv.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "设置占空比" });

                    userDgv.dataGridView.Columns.Add(new DataGridViewComboBoxColumn { Name = "DC电流" });
                    userDgv.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "设置DC电流" });

                    userDgv.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "切换开关" });
                    userDgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShortError", ReadOnly = true });
                    userDgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "OpenError", ReadOnly = true });

                    for (var i = 0; i < 12; i++)
                    {
                        if (k == 0)
                        {
                            userDgv.dataGridView.Rows.Add(string.Format("通道{0}", i + 1));
                        }
                        else
                        {
                            userDgv.dataGridView.Rows.Add(string.Format("通道{0}", i + 1 + 12));
                        }

                        {
                            var comboxDelay = userDgv.dataGridView.Rows[i].Cells[1] as DataGridViewComboBoxCell;
                            var dataGridViewCellStyleDelay = new DataGridViewCellStyle { NullValue = _listPwm[0] };
                            if (comboxDelay != null)
                            {
                                comboxDelay.Style = dataGridViewCellStyleDelay;
                                comboxDelay.DataSource = _listPwm;
                                comboxDelay.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                            }
                        }

                        {
                            var comboxDelay = userDgv.dataGridView.Rows[i].Cells[3] as DataGridViewComboBoxCell;
                            var dataGridViewCellStyleDelay = new DataGridViewCellStyle { NullValue = _listDimSet[0] };
                            if (comboxDelay != null)
                            {
                                comboxDelay.Style = dataGridViewCellStyleDelay;
                                comboxDelay.DataSource = _listDimSet;
                                comboxDelay.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                            }
                        }

                        userDgv.dataGridView.Rows[i].Cells[2].Value = "设置PWM";
                        userDgv.dataGridView.Rows[i].Cells[4].Value = "设置DC电流";
                        userDgv.dataGridView.Rows[i].Cells[5].Value = "切换";

                        userDgv.dataGridView.Rows[i].DefaultCellStyle.BackColor = _chOffColor;
                    }

                    userDgv.dataGridView.CellContentClick += dataGridView_CellContentClick;
                    tablePanel.Controls.Add(userDgv, k, 0);
                    _userDataGrids.Add(str, userDgv);
                }

                var newTab = new TabPage
                {
                    Text = string.Format("芯片：{0}", d),
                    Font = new Font("微软雅黑", 16, FontStyle.Regular),
                    Dock = DockStyle.Fill
                };
                newTab.Controls.Add(tablePanel);
                mainTab.Controls.Add(newTab);
            }

            // 0dec, 0x00h, 000000b
            _currentDev = string.Format("芯片：{0}", devs[0]);
            if (_readStatusTh != null)
            {
                _readStatusTh.Abort();
                _readStatusTh.Join();
            }

            _readStatusTh = new Thread(ReadStatus);
            _readStatusTh.Start();
        }

        private void mainTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tabPage = sender as TabControl;
            if (tabPage == null)
                return;
            var str = tabPage.SelectedTab.Text;
            _currentDev = str;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            var btn = sender as UCBtnExt;
            if (btn != null)
            {
                var str = btn.Parent.Parent.Text;
                DevChControlAll(false, str);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var btn = sender as UCBtnExt;
            if (btn != null)
            {
                var str = btn.Parent.Parent.Text;
                DevChControlAll(true, str);
            }
        }

        private void DevChControlAll(bool isOn, string devName)
        {
            for (var k = 0; k < 2; k++)
            {
                var str = string.Format(k == 0 ? "{0} 通道1~12" : "{0} 通道13~24", devName);

                var dgv = _userDataGrids[str];

                for (var i = 0; i < dgv.dataGridView.RowCount; i++)
                {
                    dgv.dataGridView.Rows[i].DefaultCellStyle.BackColor = isOn ? _chOnColor : _chOffColor;
                    var chIndex = dgv.dataGridView.Rows[i].Cells[0].Value.ToString().Replace("通道", "");
                    var devAddr = dgv.Parent.Parent.Text.Replace("芯片：", "").Split(',')[0].TrimEnd('d', 'e', 'c');
                    if (isOn)
                    {
                        Bd18331Euv.ChannelOn(devAddr, chIndex);
                    }
                    else
                    {
                        Bd18331Euv.ChannelOff(devAddr, chIndex);
                    }
                }
            }
        }

        private void Bd18331FormLedControl_Closed(object sender, EventArgs e)
        {
            if (Gw1 != null)
                Gw1.Dispose();

            if (Bd18331Euv != null)
                Bd18331Euv.Dispose();

            if (_readStatusTh == null) return;
            _readStatusTh.Abort();
            _readStatusTh.Join();
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv != null)
            {
                var key = ((UserDataGrid)dgv.Parent).label.Text;
                var devAddr = dgv.Parent.Parent.Parent.Text.Replace("芯片：", "").Split(',')[0].TrimEnd('d', 'e', 'c');
                var chIndex = dgv.Rows[e.RowIndex].Cells[0].Value.ToString().Replace("通道", "");

                if (dgv.Columns[e.ColumnIndex].Name == "设置占空比" || dgv.Columns[e.ColumnIndex].Name == "设置DC电流" || dgv.Columns[e.ColumnIndex].Name == "切换开关")
                {
                    _userDataGrids[key].dataGridView.EndEdit();

                    if (dgv.Columns[e.ColumnIndex].Name == "设置占空比")
                    {
                        var formattedValue = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].FormattedValue;
                        if (formattedValue != null)
                        {
                            var pwm = formattedValue.ToString();
                            Bd18331Euv.SetPwmDuty(devAddr, chIndex, pwm);
                        }
                    }
                    else if (dgv.Columns[e.ColumnIndex].Name == "设置DC电流")
                    {
                        var formattedValue = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].FormattedValue;
                        if (formattedValue != null)
                        {
                            var pwm = formattedValue.ToString();
                            Bd18331Euv.DimSet(devAddr, chIndex, pwm);
                        }
                    }
                    else if (dgv.Columns[e.ColumnIndex].Name == "切换开关")
                    {
                        var rowBackColor = dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor;
                        if (rowBackColor == _chOnColor)
                        {
                            dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = _chOffColor;
                            Bd18331Euv.ChannelOff(devAddr, chIndex);
                        }
                        else
                        {
                            dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = _chOnColor;
                            Bd18331Euv.ChannelOn(devAddr, chIndex);
                        }
                    }
                }
            }
        }

        private void ReadStatus()
        {
            //while (_readStatusTh.IsAlive)
            //{
            //    if (!_readStatusTh.IsAlive)
            //        return;

            //    Thread.Sleep(250);

            //    var dev = _currentDev.Replace("芯片：", "").Split(',')[0].TrimEnd('d', 'e', 'c');
            //    Bd18331Euv.ReadStatus(dev);

            //    var shortError = Bd18331Euv.GetType().GetField(string.Format("Dev{0}Led1To24ShortErrorStatus", dev)).GetValue(Bd18331Euv).ToString();
            //    var openError = Bd18331Euv.GetType().GetField(string.Format("Dev{0}Led1To24OpenErrorStatus", dev)).GetValue(Bd18331Euv).ToString();

            //    if (!string.IsNullOrEmpty(shortError) && !string.IsNullOrEmpty(openError))
            //    {
            //        // 芯片：0dec, 0x00h, 000000b 通道1~12
            //        var baseStrIndex = 0;
            //        var key1 = _currentDev + " 通道1~12";
            //        var dgv1 = _userDataGrids[key1].dataGridView;
            //        for (var i = 0; i < 12; i++)
            //        {
            //            dgv1.Rows[i].Cells[4].Value = shortError[baseStrIndex].ToString();
            //            dgv1.Rows[i].Cells[5].Value = openError[baseStrIndex].ToString();
            //            baseStrIndex++;
            //        }

            //        // 芯片：0dec, 0x00h, 000000b 通道13~24
            //        var key2 = _currentDev + " 通道13~24";
            //        var dgv2 = _userDataGrids[key2].dataGridView;
            //        for (var i = 0; i < 12; i++)
            //        {
            //            dgv2.Rows[i].Cells[4].Value = shortError[baseStrIndex].ToString();
            //            dgv2.Rows[i].Cells[5].Value = openError[baseStrIndex].ToString();
            //            baseStrIndex++;
            //        }
            //    }
            //}
        }

        private void btnPwmMode_Click(object sender, EventArgs e)
        {
            btnDcMode.Enabled = true;
            btnPwmMode.Enabled = false;
            lblMode.Text = @"当前模式：PWM模式";
            Bd18331Euv.IsDcMode = false;
        }

        private void btnDcMode_Click(object sender, EventArgs e)
        {
            btnDcMode.Enabled = false;
            btnPwmMode.Enabled = true;
            lblMode.Text = @"当前模式：DC模式";
            Bd18331Euv.IsDcMode = true;
        }
    }
}