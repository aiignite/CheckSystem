using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CommonUtility;
using CommonUtility.BusLoader;
using HZH_Controls.IconFont;

namespace CheckSystem
{
    public partial class FormLinMsgMonitor : Form
    {
        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public FormLinMsgMonitor()
        {
            InitializeComponent();
            Text = @"LIN消息监控界面";
            Icon = FontImages.GetIcon(FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);
            Load += FormLinMsgMonitor_Load;
            Closed += FormLinMsgMonitor_Closed;
            btnLinStartMonitor.Click+=btnLinStartMonitor_Click;
            btnClearLinData.Click+=btnClearLinData_Click;
            btnSaveLinMsg.Click+=btnSaveLinMsg_Click;
            btnLinIdRange.Click+=btnLinIdRange_Click;
        }

        private void FormLinMsgMonitor_Load(object sender, EventArgs e)
        {
            LinBus.PushLinMsg += LinBus_PushLinMsg;
        }

        private void FormLinMsgMonitor_Closed(object sender, EventArgs e)
        {
            LinBus.PushLinMsg -= LinBus_PushLinMsg;
        }

        #region LIN MESSAGE
        private void LinBus_PushLinMsg(string name, LinBus.LinDataPackage data)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    if (LinListView.Columns.Count == 0)
                    {
                        var hearerList = new List<string>
                        {
                            "接收时间",
                            "时间戳",
                            "源通道",
                            "LIN ID/PID",
                            "数据"
                        };

                        LinListView.View = View.Details;
                        LinListView.GridLines = true;
                        LinListView.Font = new Font("微软雅黑", 12, FontStyle.Regular);

                        var width = Screen.PrimaryScreen.WorkingArea.Width / hearerList.Count;

                        for (var i = 0; i < hearerList.Count; i++)
                        {
                            LinListView.Columns.Add(hearerList[i]);
                            LinListView.Columns[i].Width = width;
                        }

                        lblCmbLinChannel.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        lblCmbLinChannel.comboBox.Items.Add(@"All");
                        lblCmbLinChannel.comboBox.SelectedIndex = 0;

                        lblCmbMaxLinCount.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        lblCmbMaxLinCount.comboBox.Items.Add("500");
                        lblCmbMaxLinCount.comboBox.Items.Add("100");
                        lblCmbMaxLinCount.comboBox.Items.Add("1000");
                        lblCmbMaxLinCount.comboBox.Items.Add("1500");
                        lblCmbMaxLinCount.comboBox.Items.Add("2000");
                        lblCmbMaxLinCount.comboBox.Items.Add("3500");
                        lblCmbMaxLinCount.comboBox.Items.Add("5000");
                        lblCmbMaxLinCount.comboBox.Items.Add("9999");
                        lblCmbMaxLinCount.comboBox.SelectedIndex = 0;
                    }

                    if (!lblCmbLinChannel.comboBox.Items.Contains(name))
                        lblCmbLinChannel.comboBox.Items.Add(name);

                    if (btnLinStartMonitor.Text == @"开启监控")
                        return;

                    var minLinId = byte.MinValue;
                    var maxLinId = byte.MaxValue;

                    if (lblCmbLinIdMin.Enabled == false && lblCmbLinIdMax.Enabled == false &&
                        !string.IsNullOrEmpty(lblCmbLinIdMin.comboBox.Text) && !string.IsNullOrEmpty(lblCmbLinIdMax.comboBox.Text))
                    {
                        try
                        {
                            minLinId = Convert.ToByte(lblCmbLinIdMin.comboBox.Text, 16);
                            maxLinId = Convert.ToByte(lblCmbLinIdMax.comboBox.Text, 16);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }

                    if ((name == lblCmbLinChannel.comboBox.Text || lblCmbLinChannel.comboBox.Text == @"All") &&
                        data.LinId >= minLinId && data.LinId <= maxLinId)
                    {
                        LinListView.BeginUpdate();

                        if (LinListView.Items.Count > int.Parse(lblCmbMaxLinCount.comboBox.Text))
                            LinListView.Items.Clear();

                        var item = new ListViewItem { Text = data.DateTime.ToString("yyyy-MM-dd HH:mm:ss:ffff") };
                        item.SubItems.Add(data.DateTime.Ticks.ToString());
                        item.SubItems.Add(name);
                        item.SubItems.Add(string.Format("0x{0}", string.Format("{0:X}", data.LinId).PadLeft(2, '0')));
                        item.SubItems.Add(ValueHelper.GetHextStr(data.LinData));

                        LinListView.Items.Add(item);
                        LinListView.EndUpdate();
                    }
                }));
            }
        }

        private void btnLinStartMonitor_Click(object sender, EventArgs e)
        {
            if (btnLinStartMonitor.Text == @"开启监控")
            {
                btnLinStartMonitor.Text = @"停止监控";
                btnSaveLinMsg.Enabled = false;
            }
            else if (btnLinStartMonitor.Text == @"停止监控")
            {
                btnLinStartMonitor.Text = @"开启监控";
                btnSaveLinMsg.Enabled = true;
            }
        }

        private void btnClearLinData_Click(object sender, EventArgs e)
        {
            LinListView.BeginUpdate();
            LinListView.Items.Clear();
            LinListView.EndUpdate();
        }

        private void btnSaveLinMsg_Click(object sender, EventArgs e)
        {
            if (btnLinStartMonitor.Text == @"开启监控" && LinListView.Items.Count > 0)
            {
                var fileDialog = new SaveFileDialog { Filter = @"文本文件|*.txt" };
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = fileDialog.FileName;
                    if (filePath.EndsWith(".txt"))
                    {
                        var sb = new StringBuilder();
                        for (var i = 0; i < LinListView.Items.Count; i++)
                        {
                            var str = string.Empty;
                            for (var j = 0; j < LinListView.Columns.Count; j++)
                                str += LinListView.Items[i].SubItems[j].Text + "; ";
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

        private void btnLinIdRange_Click(object sender, EventArgs e)
        {
            if (lblCmbLinIdMin.Enabled == false && lblCmbLinIdMax.Enabled == false)
            {
                if (string.IsNullOrEmpty(lblCmbLinIdMin.comboBox.Text) && string.IsNullOrEmpty(lblCmbLinIdMax.comboBox.Text))
                {
                    lblCmbLinIdMin.Enabled = true;
                    lblCmbLinIdMax.Enabled = true;
                }
                else if (!string.IsNullOrEmpty(lblCmbLinIdMin.comboBox.Text) && !string.IsNullOrEmpty(lblCmbLinIdMax.comboBox.Text))
                {
                    lblCmbLinIdMin.comboBox.Text = string.Empty;
                    lblCmbLinIdMax.comboBox.Text = string.Empty;
                }
            }
            else if (lblCmbLinIdMin.Enabled && lblCmbLinIdMax.Enabled)
            {
                if (string.IsNullOrEmpty(lblCmbLinIdMin.comboBox.Text) && string.IsNullOrEmpty(lblCmbLinIdMax.comboBox.Text))
                {
                    lblCmbLinIdMin.comboBox.Text = string.Empty;
                    lblCmbLinIdMax.comboBox.Text = string.Empty;
                    lblCmbLinIdMin.Enabled = false;
                    lblCmbLinIdMax.Enabled = false;
                }
                else if (!string.IsNullOrEmpty(lblCmbLinIdMin.comboBox.Text) && !string.IsNullOrEmpty(lblCmbLinIdMax.comboBox.Text))
                {
                    var isConvertOk = true;

                    try
                    {
                        Convert.ToUInt32(lblCmbLinIdMin.comboBox.Text, 16);
                        Convert.ToUInt32(lblCmbLinIdMax.comboBox.Text, 16);
                    }
                    catch (Exception)
                    {
                        isConvertOk = false;
                    }

                    if (isConvertOk)
                    {
                        lblCmbLinIdMin.Enabled = false;
                        lblCmbLinIdMax.Enabled = false;
                    }
                }
            }
        }
        #endregion
        
    }
}
