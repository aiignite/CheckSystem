using CommonUtility;
using CommonUtility.BusLoader;
using HZH_Controls.IconFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CheckSystem
{
    public partial class FormCanMsgMonitor : Form
    {
        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public FormCanMsgMonitor()
        {
            InitializeComponent();
            Text = @"CAN消息监控界面";
            Icon = FontImages.GetIcon(FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);
            Load += FormCanMsgMonitor_Load;
            Closed += FormCanMsgMonitor_Closed;
            btnCanStartMonitor.Click += btnStartCanMonitor_Click;
            btnClearCanData.Click += btnClearCanData_Click;
            btnCanIdRange.Click += btnCanIdRange_Click_1;
            btnSaveCanMsg.Click += btnSaveCanMsg_Click;
        }

        private void FormCanMsgMonitor_Load(object sender, EventArgs e)
        {
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

            lblCmbCanChannel.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            lblCmbCanChannel.comboBox.Items.Add(@"All");
            lblCmbCanChannel.comboBox.SelectedIndex = 0;

            lblCmbMaxCanCount.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            lblCmbMaxCanCount.comboBox.Items.Add("500");
            lblCmbMaxCanCount.comboBox.Items.Add("100");
            lblCmbMaxCanCount.comboBox.Items.Add("1000");
            lblCmbMaxCanCount.comboBox.Items.Add("1500");
            lblCmbMaxCanCount.comboBox.Items.Add("2000");
            lblCmbMaxCanCount.comboBox.Items.Add("3500");
            lblCmbMaxCanCount.comboBox.Items.Add("5000");
            lblCmbMaxCanCount.comboBox.Items.Add("9999");
            lblCmbMaxCanCount.comboBox.SelectedIndex = 0;

            CanBus.PushCanMsg += CanBus_PushCanMsg;
        }


        private void FormCanMsgMonitor_Closed(object sender, EventArgs e)
        {
            CanBus.PushCanMsg -= CanBus_PushCanMsg;
            uiMillisecondTimer1.Enabled = false;
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

        #region CAN MESSAGE

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

        private void CanBus_PushCanMsg(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (!lblCmbCanChannel.comboBox.Items.Contains(name))
                lblCmbCanChannel.comboBox.Items.Add(name);

            if (btnCanStartMonitor.Text == @"开启监控")
                return;

            var minCanId = uint.MinValue;
            var maxCanId = uint.MaxValue;

            if (lblCmbCanIdMin.Enabled == false && lblCmbCanIdMax.Enabled == false &&
                !string.IsNullOrEmpty(lblCmbCanIdMin.comboBox.Text) &&
                !string.IsNullOrEmpty(lblCmbCanIdMax.comboBox.Text))
            {
                try
                {
                    minCanId = Convert.ToUInt32(lblCmbCanIdMin.comboBox.Text, 16);
                    maxCanId = Convert.ToUInt32(lblCmbCanIdMax.comboBox.Text, 16);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            if ((name == lblCmbCanChannel.comboBox.Text ||
                lblCmbCanChannel.comboBox.Text == @"All") &&
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
                    if (_datas.Count > int.Parse(lblCmbMaxCanCount.comboBox.Text))
                    {
                        _datas.Clear();
                    }
                    _datas.Add(newData);
                }
            }
        }

        private void btnStartCanMonitor_Click(object sender, EventArgs e)
        {
            if (btnCanStartMonitor.Text == @"开启监控")
            {
                btnCanStartMonitor.Text = @"停止监控";
                btnSaveCanMsg.Enabled = false;
            }
            else if (btnCanStartMonitor.Text == @"停止监控")
            {
                btnCanStartMonitor.Text = @"开启监控";
                btnSaveCanMsg.Enabled = true;
            }
        }

        private void btnClearCanData_Click(object sender, EventArgs e)
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
                if (string.IsNullOrEmpty(lblCmbCanIdMin.comboBox.Text) && string.IsNullOrEmpty(lblCmbCanIdMax.comboBox.Text))
                {
                    lblCmbCanIdMin.Enabled = true;
                    lblCmbCanIdMax.Enabled = true;
                }
                else if (!string.IsNullOrEmpty(lblCmbCanIdMin.comboBox.Text) && !string.IsNullOrEmpty(lblCmbCanIdMax.comboBox.Text))
                {
                    lblCmbCanIdMin.comboBox.Text = string.Empty;
                    lblCmbCanIdMax.comboBox.Text = string.Empty;
                }
            }
            else if (lblCmbCanIdMin.Enabled && lblCmbCanIdMax.Enabled)
            {
                if (string.IsNullOrEmpty(lblCmbCanIdMin.comboBox.Text) && string.IsNullOrEmpty(lblCmbCanIdMax.comboBox.Text))
                {
                    lblCmbCanIdMin.comboBox.Text = string.Empty;
                    lblCmbCanIdMax.comboBox.Text = string.Empty;
                    lblCmbCanIdMin.Enabled = false;
                    lblCmbCanIdMax.Enabled = false;
                }
                else if (!string.IsNullOrEmpty(lblCmbCanIdMin.comboBox.Text) && !string.IsNullOrEmpty(lblCmbCanIdMax.comboBox.Text))
                {
                    var isConvertOk = true;

                    try
                    {
                        Convert.ToUInt32(lblCmbCanIdMin.comboBox.Text, 16);
                        Convert.ToUInt32(lblCmbCanIdMax.comboBox.Text, 16);
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
            //if (btnCanStartMonitor.Text == @"开启监控" && CanListView.Items.Count > 0)
            //{
            //    var fileDialog = new SaveFileDialog { Filter = @"文本文件|*.txt" };
            //    if (fileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        var filePath = fileDialog.FileName;
            //        if (filePath.EndsWith(".txt"))
            //        {
            //            var sb = new StringBuilder();
            //            for (var i = 0; i < CanListView.Items.Count; i++)
            //            {
            //                var str = string.Empty;
            //                for (var j = 0; j < CanListView.Columns.Count; j++)
            //                    str += CanListView.Items[i].SubItems[j].Text + "; ";
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
        #endregion

        private void uiMillisecondTimer1_Tick(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                if (btnCanStartMonitor.Text == @"开启监控")
                    return;

                try
                {
                    uiDataGridView1.Invoke(new Action(() =>
                    {
                        lock (_datas)
                        {
                            if (_datas.Count > 0)
                            {
                                uiDataGridView1.ClearRows();
                                uiDataGridView1.RowCount = _datas.Count;
                                uiDataGridView1.Refresh();
                                uiDataGridView1.FirstDisplayedScrollingRowIndex = uiDataGridView1.RowCount - 1;
                            }
                        }
                    }));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
