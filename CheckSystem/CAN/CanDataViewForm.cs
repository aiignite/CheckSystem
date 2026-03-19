using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CommonUtility;
using CommonUtility.BusLoader;
using HZH_Controls.IconFont;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.CAN
{
    public partial class CanDataViewForm : UIForm
    {
        private CanBus Can { get; set; }
        private Form CurrentOpenedForm { get; set; }
        private CanDeviceMgrForm ControllerMgrForm { get; set; }

        public CanDataViewForm()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);
            WindowState = FormWindowState.Maximized;
            btnTablePanel.BackColor = Color.DarkGoldenrod;
            OpenCanDevice();
            Load += CanDataViewForm_Load;
            Closed += CanDataViewForm_Closed;
        }

        public void CanDataViewForm_Closed(object sender, EventArgs e)
        {
            if (ControllerMgrForm != null)
                ControllerMgrForm.Dispose();
        }

        private void CanDataViewForm_Load(object sender, EventArgs e)
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
            uiDataGridView1.AddColumn("Direction", "Direction");
            uiDataGridView1.AddColumn("CanProtocol", "CanProtocol");
            uiDataGridView1.AddColumn("CanDateType", "CanDateType");
            uiDataGridView1.AddColumn("CanId", "CanId");
            uiDataGridView1.AddColumn("CanData", "CanData");
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
            lblCmbMaxCount.SelectedIndex = 0;
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

        public CanDataViewForm(CanBus can, string showTitle)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_social_googledrive, 32,
               Color.DodgerBlue);
            Can = can;
            WindowState = FormWindowState.Maximized;
            CanBus.PushCanMsg += CanBus_PushCanMsg;
            btnTablePanel.BackColor = Color.Green;
            Text = showTitle;
            Load += CanDataViewForm_Load;
            Closed += CanDataViewForm_Closed;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void OpenCanDevice()
        {
            if (Can == null)
            {
                using (ControllerMgrForm = new CanDeviceMgrForm())
                {
                    if (ControllerMgrForm.ShowDialog() == DialogResult.OK && ControllerMgrForm.InitCan != null)
                    {
                        MessageBox.Show(@"打开设备成功！");
                        Can = ControllerMgrForm.InitCan;
                        CanBus.PushCanMsg += CanBus_PushCanMsg;
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

        private void CanBus_PushCanMsg(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            SetListView(name, data, onPushCanDataType);
        }

        private delegate void SetListViewDelegate(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType);

        private readonly List<Data> _datas = new List<Data>();

        internal class Data
        {
            public string RecvTime { get; set; }

            public string SourceChannel { get; set; }

            public string Direction { get; set; }

            public string CanProtocol { get; set; }

            public string CanDateType { get; set; }

            public string CanId { get; set; }

            public string CanData { get; set; }
        }

        private void SetListView(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            try
            {
                if (InvokeRequired)
                {
                    SetListViewDelegate setListViewDelegate = SetListView;
                    Invoke(setListViewDelegate, name, data, onPushCanDataType);
                }
                else
                {
                    if (Can.Name != name)
                        return;

                    if (!lblCmbCanChannel.Items.Contains(name))
                        lblCmbCanChannel.Items.Add(name);

                    if (btnStartMonitor.Text == @"开启监控")
                        return;

                    var minCanId = uint.MinValue;
                    var maxCanId = uint.MaxValue;

                    if (lblCmbCanIdMin.Enabled == false && lblCmbCanIdMax.Enabled == false &&
                        !string.IsNullOrEmpty(lblCmbCanIdMin.Text) &&
                        !string.IsNullOrEmpty(lblCmbCanIdMax.Text))
                    {
                        try
                        {
                            minCanId = Convert.ToUInt32(lblCmbCanIdMin.Text, 16);
                            maxCanId = Convert.ToUInt32(lblCmbCanIdMax.Text, 16);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }

                    if ((name == lblCmbCanChannel.Text ||
                        lblCmbCanChannel.Text == @"All") &&
                        data.CanId >= minCanId && data.CanId <= maxCanId)
                    {
                        var newData = new Data
                        {
                            RecvTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"),
                            SourceChannel = name,
                            Direction = onPushCanDataType.ToString(),
                            CanProtocol = data.CanProtocol.ToString(),
                            CanDateType = data.CanType.ToString(),
                            CanId = string.Format("0x{0}", string.Format("{0:X}", data.CanId).PadLeft(8, '0')),
                            CanData = ValueHelper.GetHextStr(data.CanData)
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

        private void ucBtnFillet1_BtnClick(object sender, EventArgs e)
        {
            OpenCanDevice();
        }

        private void ucBtnNormalSendData_BtnClick(object sender, EventArgs e)
        {
            if (Can == null)
            {
                MessageBox.Show(@"请先打开设备！");
            }
            else
            {
                const string formName = "NormalSendForm";

                if (CurrentOpenedForm == null)
                {
                    CurrentOpenedForm = new CanDataNormalSendForm(Can) { Name = formName };
                    CurrentOpenedForm.Show();
                }
                else
                {
                    if (CurrentOpenedForm.Name == formName)
                    {
                        if (CurrentOpenedForm.IsDisposed)
                        {
                            CurrentOpenedForm = new CanDataNormalSendForm(Can) { Name = formName };
                            CurrentOpenedForm.Show();
                        }
                        CurrentOpenedForm.WindowState = FormWindowState.Normal;
                        CurrentOpenedForm.Focus();
                    }
                    else
                    {
                        CurrentOpenedForm.Close();
                        CurrentOpenedForm.Dispose();

                        CurrentOpenedForm = new CanDataNormalSendForm(Can) { Name = formName };
                        CurrentOpenedForm.Show();
                    }
                }
            }
        }

        private void ucBtnUdsSendData_BtnClick(object sender, EventArgs e)
        {
            if (Can == null)
            {
                MessageBox.Show(@"请先打开设备！");
            }
            else
            {
                const string formName = "UDSSendForm";

                if (CurrentOpenedForm == null)
                {
                    CurrentOpenedForm = new CanDataUdsSendForm(Can) { Name = formName };
                    CurrentOpenedForm.Show();
                }
                else
                {
                    if (CurrentOpenedForm.Name == formName)
                    {
                        if (CurrentOpenedForm.IsDisposed)
                        {
                            CurrentOpenedForm = new CanDataUdsSendForm(Can) { Name = formName };
                            CurrentOpenedForm.Show();
                        }
                        CurrentOpenedForm.WindowState = FormWindowState.Normal;
                        CurrentOpenedForm.Focus();
                    }
                    else
                    {
                        CurrentOpenedForm.Close();
                        CurrentOpenedForm.Dispose();

                        CurrentOpenedForm = new CanDataUdsSendForm(Can) { Name = formName };
                        CurrentOpenedForm.Show();
                    }
                }
            }
        }

        private void ucBtnMotorolaMatrix_BtnClick(object sender, EventArgs e)
        {

        }

        private void ucBtnIntelMatrix_BtnClick(object sender, EventArgs e)
        {

        }

        private void ucBtnProduct_BtnClick(object sender, EventArgs e)
        {
            if (Can == null)
            {
                MessageBox.Show(@"请先打开设备！");
            }
            else
            {
                const string formName = "ProductSendForm";

                if (CurrentOpenedForm == null)
                {
                    CurrentOpenedForm = new CanDataProductFrom(Can) { Name = formName };
                    CurrentOpenedForm.Show();
                }
                else
                {
                    if (CurrentOpenedForm.Name == formName)
                    {
                        if (CurrentOpenedForm.IsDisposed)
                        {
                            CurrentOpenedForm = new CanDataProductFrom(Can) { Name = formName };
                            CurrentOpenedForm.Show();
                        }
                        CurrentOpenedForm.WindowState = FormWindowState.Normal;
                        CurrentOpenedForm.Focus();
                    }
                    else
                    {
                        CurrentOpenedForm.Close();
                        CurrentOpenedForm.Dispose();

                        CurrentOpenedForm = new CanDataProductFrom(Can) { Name = formName };
                        CurrentOpenedForm.Show();
                    }
                }
            }
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (_datas)
            {
                _datas.Clear();
                uiDataGridView1.ClearRows();
                uiDataGridView1.RowCount = _datas.Count;
                uiDataGridView1.Refresh();
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

        private void btnCanIdRange_Click_1(object sender, EventArgs e)
        {
            if (lblCmbCanIdMin.Enabled == false && lblCmbCanIdMax.Enabled == false)
            {
                if (string.IsNullOrEmpty(lblCmbCanIdMin.Text) && string.IsNullOrEmpty(lblCmbCanIdMax.Text))
                {
                    lblCmbCanIdMin.Enabled = true;
                    lblCmbCanIdMax.Enabled = true;
                }
                else if (!string.IsNullOrEmpty(lblCmbCanIdMin.Text) && !string.IsNullOrEmpty(lblCmbCanIdMax.Text))
                {
                    lblCmbCanIdMin.Text = string.Empty;
                    lblCmbCanIdMax.Text = string.Empty;
                }
            }
            else if (lblCmbCanIdMin.Enabled && lblCmbCanIdMax.Enabled)
            {
                if (string.IsNullOrEmpty(lblCmbCanIdMin.Text) && string.IsNullOrEmpty(lblCmbCanIdMax.Text))
                {
                    lblCmbCanIdMin.Text = string.Empty;
                    lblCmbCanIdMax.Text = string.Empty;
                    lblCmbCanIdMin.Enabled = false;
                    lblCmbCanIdMax.Enabled = false;
                }
                else if (!string.IsNullOrEmpty(lblCmbCanIdMin.Text) && !string.IsNullOrEmpty(lblCmbCanIdMax.Text))
                {
                    var isConvertOk = true;

                    try
                    {
                        Convert.ToUInt32(lblCmbCanIdMax.Text, 16);
                        Convert.ToUInt32(lblCmbCanIdMax.Text, 16);
                    }
                    catch (Exception)
                    {
                        isConvertOk = false;
                    }

                    if (isConvertOk)
                    {
                        lblCmbCanIdMin.Enabled = false;
                        lblCmbCanIdMax.Enabled = false;
                    }
                }
            }
        }

        private void btnSaveCanMsg_Click(object sender, EventArgs e)
        {
            //if (btnStartMonitor.Text == @"开启监控" && listView1.Items.Count > 0)
            //{
            //    var fileDialog = new SaveFileDialog { Filter = @"文本文件|*.txt" };
            //    if (fileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        var filePath = fileDialog.FileName;
            //        if (filePath.EndsWith(".txt"))
            //        {
            //            var sb = new StringBuilder();
            //            for (var i = 0; i < listView1.Items.Count; i++)
            //            {
            //                var str = string.Empty;
            //                for (var j = 0; j < listView1.Columns.Count; j++)
            //                    str += listView1.Items[i].SubItems[j].Text + "; ";
            //                sb.Append(str + "\r\n");
            //            }

            //            try
            //            {
            //                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            //                {
            //                    var bs = Encoding.ASCII.GetBytes(sb.ToString());
            //                    fs.Write(bs, 0, bs.Length);
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show(ex.Message);
            //            }
            //        }
            //    }
            //}
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
    }
}
