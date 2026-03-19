using CommonUtility;
using Go;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HZH_Controls.IconFont;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.UART
{
    public partial class UartDataViewForm : UIForm
    {
        private MySerialPort MySerialPort { get; set; }
        private Form CurrentOpenedForm { get; set; }
        private UartDeviceMgrForm ControllerMgrForm { get; set; }

        private readonly List<Data> _datas = new List<Data>();

        internal class Data
        {
            public string RecvTime { get; set; }

            public string SourceChannel { get; set; }

            public string UartData { get; set; }
        }

        public UartDataViewForm()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);
            WindowState = FormWindowState.Maximized;
            btnTablePanel.BackColor = Color.DarkGoldenrod;
            OpenCanDevice();
            Load += UartDataViewForm_Load;
            Closed += UartDataViewForm_Closed;
        }

        private void UartDataViewForm_Load(object sender, System.EventArgs e)
        {
            uiDataGridView1.ShowCellToolTips = false;
            uiDataGridView1.ReadOnly = true;
            uiDataGridView1.RowHeadersVisible = false;
            uiDataGridView1.AllowUserToAddRows = false;
            uiDataGridView1.AllowUserToResizeRows = false;
            uiDataGridView1.MultiSelect = true;
            uiDataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            uiDataGridView1.AddColumn("RecvTime", "RecvTime");
            uiDataGridView1.AddColumn("SourceChannel", "SourceChannel");
            uiDataGridView1.AddColumn("UartData", "UartData");
            uiDataGridView1.VirtualMode = true;
            uiDataGridView1.CellValueNeeded += uiDataGridView1_CellValueNeeded;
            uiDataGridView1.DataError += uiDataGridView1_DataError;

            lblCmbCanChannel.DropDownStyle = ComboBoxStyle.DropDownList;
            lblCmbCanChannel.Items.Add(@"All");
            lblCmbCanChannel.SelectedIndex = 0;

            lblCmbMaxCount.DropDownStyle = ComboBoxStyle.DropDownList;
            lblCmbMaxCount.Items.Add("500");
            lblCmbMaxCount.Items.Add("100");
            lblCmbMaxCount.Items.Add("1000");
            lblCmbMaxCount.Items.Add("1500");
            lblCmbMaxCount.Items.Add("2000");
            lblCmbMaxCount.Items.Add("3500");
            lblCmbMaxCount.Items.Add("5000");
            lblCmbMaxCount.Items.Add("9999");
            lblCmbMaxCount.Items.Add("99999");
            lblCmbMaxCount.Items.Add("999999");
            lblCmbMaxCount.SelectedIndex = 0;

            _action = generator.tgo(FormSelection.MainStrand, async delegate ()
            {
                while (true)
                {
                    await generator.sleep(30);
                    txtTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff");
                    //(system_tick.get_tick_ms() % 1000 / 10).ToString();
                }
            });
        }

        private void UartDataViewForm_Closed(object sender, EventArgs e)
        {
            if (ControllerMgrForm != null)
                ControllerMgrForm.Dispose();

            _action.stop();
        }

        private void uiDataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (_datas.Count > e.RowIndex)
            {
                var cellHeader = uiDataGridView1.Columns[e.ColumnIndex].HeaderText;
                var d = _datas[e.RowIndex];
                var propertyInfo = d.GetType().GetProperty(cellHeader);
                if (propertyInfo != null)
                {
                    var property = propertyInfo.GetValue(d);
                    e.Value = property;
                }
            }
        }

        private static void uiDataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void OpenCanDevice()
        {
            if (MySerialPort == null)
            {
                using (ControllerMgrForm = new UartDeviceMgrForm())
                {
                    if (ControllerMgrForm.ShowDialog() == DialogResult.OK && ControllerMgrForm.MySerialPort != null)
                    {
                        MessageBox.Show(@"打开设备成功！");
                        MySerialPort = ControllerMgrForm.MySerialPort;
                        MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;
                        btnTablePanel.BackColor = Color.Green;
                    }
                    else
                    {
                        btnTablePanel.BackColor = Color.DarkGoldenrod;
                        MessageBox.Show(@"打开设备失败！");
                    }
                }
            }
            else
            {
                MessageBox.Show(@"设备已经打开！");
            }
        }

        private generator _action;

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            SetListView(name, datas);
        }

        private delegate void SetListViewDelegate(string name, byte[] datas);

        private void SetListView(
           string name, byte[] datas)
        {
            try
            {
                if (InvokeRequired)
                {
                    SetListViewDelegate setListViewDelegate = SetListView;
                    Invoke(setListViewDelegate, name, datas);
                }
                else
                {
                    if (MySerialPort.Name != name)
                        return;

                    if (!lblCmbCanChannel.Items.Contains(name))
                        lblCmbCanChannel.Items.Add(name);

                    if (btnStartMonitor.Text == @"开启监控")
                        return;

                    if (name == lblCmbCanChannel.Text ||
                        lblCmbCanChannel.Text == @"All")
                    {
                        var newData = new Data
                        {
                            RecvTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"),
                            SourceChannel = name,
                            UartData = ValueHelper.GetHextStr(datas)
                        };

                        lock (_datas)
                        {
                            if (_datas.Count > int.Parse(lblCmbMaxCount.Text))
                            {
                                _datas.Clear();
                            }
                            _datas.Add(newData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void btnStartMonitor_Click(object sender, EventArgs e)
        {
            if (btnStartMonitor.Text == @"开启监控")
            {
                btnStartMonitor.Text = @"停止监控";
                btnSaveCanMsg.Enabled = false;
            }
            else if (btnStartMonitor.Text == @"停止监控")
            {
                btnStartMonitor.Text = @"开启监控";
                btnSaveCanMsg.Enabled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lock (_datas)
            {
                _datas.Clear();
                uiDataGridView1.ClearRows();
                uiDataGridView1.RowCount = _datas.Count;
                uiDataGridView1.Refresh();
            }
        }

        private void btnSaveCanMsg_Click(object sender, EventArgs e)
        {
            if (btnStartMonitor.Text == @"开启监控" && _datas.Count > 0)
            {
                lock (_datas)
                {
                    var fileDialog = new SaveFileDialog { Filter = @"文本文件|*.txt" };
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var filePath = fileDialog.FileName;
                        if (filePath.EndsWith(".txt"))
                        {
                            var sb = new StringBuilder();
                            for (var i = 0; i < _datas.Count; i++)
                            {
                                var str = string.Format("{0};{1};{2}", _datas[i].RecvTime, _datas[i].SourceChannel,
                                    _datas[i].UartData);

                                sb.Append(str + "\r\n");
                            }

                            try
                            {
                                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                {
                                    var bs = Encoding.ASCII.GetBytes(sb.ToString());
                                    fs.Write(bs, 0, bs.Length);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private void uiMillisecondTimer1_Tick(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                if (btnStartMonitor.Text == @"开启监控")
                    return;

                uiDataGridView1.Invoke(new Action(() =>
                {
                    lock (_datas)
                    {
                        try
                        {
                            if (uiDataGridView1.RowCount > 0 && uiDataGridView1.RowCount == _datas.Count)
                            {
                                if (string.Equals(
                                    uiDataGridView1.Rows[uiDataGridView1.RowCount - 1].Cells[0].Value.ToString(),
                                    _datas[_datas.Count - 1].RecvTime, StringComparison.CurrentCultureIgnoreCase))
                                    return;
                            }

                            if (_datas.Count <= 0)
                                return;
                            uiDataGridView1.ClearRows();
                            uiDataGridView1.RowCount = _datas.Count;
                            uiDataGridView1.Refresh();
                            uiDataGridView1.FirstDisplayedScrollingRowIndex = uiDataGridView1.RowCount - 1;
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                    }
                }));
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var str = txtSendData.Text.Replace(" ", "");
                var bs = new List<byte>();
                for (var i = 0; i < str.Length; i = i + 2)
                    bs.Add(Convert.ToByte(str.Substring(i, 2), 16));

                MySerialPort.SendCommand(bs.ToArray());
                //MySerialPort.SendBreakSyncCmd(bs.ToArray());
            }
            catch (Exception exception)
            {
                // ignored
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            txtSendData.Text = "64 8A 02 31";
            toolStripButton1.PerformClick();
            Thread.Sleep(10);
            txtSendData.Text = "64 0B";
            toolStripButton1.PerformClick();
            Thread.Sleep(10);

            //txtSendData.Text = "64 8A 02 31";
            //toolStripButton1.PerformClick();
            //Thread.Sleep(10);
            //txtSendData.Text = "64 0B 03";
            //toolStripButton1.PerformClick();
            //Thread.Sleep(10);
        }
    }
}