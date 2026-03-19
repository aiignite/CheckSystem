using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonUtility;
using CommonUtility.BusLoader;
using HZH_Controls.IconFont;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.LIN
{
    public partial class LinDataViewForm : UIForm
    {
        private LinBus Lin { get; set; }
        private Form CurrentOpenedForm { get; set; }
        private LinDeviceMgrForm ControllerMgrForm { get; set; }

        public LinDataViewForm()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);
            CheckForIllegalCrossThreadCalls = false;
            WindowState = FormWindowState.Maximized;
            btnTablePanel.BackColor = Color.DarkGoldenrod;
            OpenCanDevice();
            Load += LinDataViewForm_Load;
            Closed += LinDataViewForm_Closed;
        }

        public void LinDataViewForm_Closed(object sender, EventArgs e)
        {
            if (ControllerMgrForm != null)
                ControllerMgrForm.Dispose();
        }

        private void LinDataViewForm_Load(object sender, EventArgs e)
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

        public LinDataViewForm(LinBus lin, string showTitle)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);
            CheckForIllegalCrossThreadCalls = false;
            Lin = lin;
            WindowState = FormWindowState.Maximized;
            LinBus.PushLinMsg += LinBus_PushLinMsg;
            btnTablePanel.BackColor = Color.Green;
            Text = showTitle;
            Load += LinDataViewForm_Load;
            Closed += LinDataViewForm_Closed;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void OpenCanDevice()
        {
            if (Lin == null)
            {
                using (ControllerMgrForm = new LinDeviceMgrForm())
                {
                    if (ControllerMgrForm.ShowDialog() == DialogResult.OK && ControllerMgrForm.InitLin != null)
                    {
                        MessageBox.Show(@"打开设备成功！");
                        Lin = ControllerMgrForm.InitLin;
                        LinBus.PushLinMsg += LinBus_PushLinMsg;
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

        private void LinBus_PushLinMsg(string name, LinBus.LinDataPackage data)
        {
            if (!InvokeRequired)
                return;

            try
            {
                Invoke(new Action(() =>
                {
                    if (Lin.Name != name)
                        return;

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

                        //if (LinListView.Items.Count > int.Parse(lblCmbMaxLinCount.comboBox.Text))
                        //    LinListView.Items.Clear();

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
            catch (Exception)
            {
                // ignored
            }
        }

        private void ucBtnFillet1_BtnClick(object sender, EventArgs e)
        {
            OpenCanDevice();
        }

        private void ucBtnNormalSendData_BtnClick(object sender, EventArgs e)
        {
            if (Lin == null)
            {
                MessageBox.Show(@"请先打开设备！");
            }
            else
            {
                const string formName = "NormalSendForm";

                if (CurrentOpenedForm == null)
                {
                    CurrentOpenedForm = new LinDataNormalSendForm(Lin) { Name = formName };
                    CurrentOpenedForm.Show();
                }
                else
                {
                    if (CurrentOpenedForm.Name == formName)
                    {
                        if (CurrentOpenedForm.IsDisposed)
                        {
                            CurrentOpenedForm = new LinDataNormalSendForm(Lin) { Name = formName };
                            CurrentOpenedForm.Show();
                        }
                        CurrentOpenedForm.WindowState = FormWindowState.Normal;
                        CurrentOpenedForm.Focus();
                    }
                    else
                    {
                        CurrentOpenedForm.Close();
                        CurrentOpenedForm.Dispose();

                        CurrentOpenedForm = new LinDataNormalSendForm(Lin) { Name = formName };
                        CurrentOpenedForm.Show();
                    }
                }
            }
        }

        private void ucBtnUdsSendData_BtnClick(object sender, EventArgs e)
        {
            //if (Lin == null)
            //{
            //    MessageBox.Show(@"请先打开设备！");
            //}
            //else
            //{
            //    const string formName = "UDSSendForm";

            //    if (CurrentOpenedForm == null)
            //    {
            //        CurrentOpenedForm = new CanDataUdsSendForm { Name = formName };
            //        CurrentOpenedForm.Show();
            //    }
            //    else
            //    {
            //        if (CurrentOpenedForm.Name == formName)
            //        {
            //            if (CurrentOpenedForm.IsDisposed)
            //            {
            //                CurrentOpenedForm = new CanDataUdsSendForm { Name = formName };
            //                CurrentOpenedForm.Show();
            //            }
            //            CurrentOpenedForm.WindowState = FormWindowState.Normal;
            //            CurrentOpenedForm.Focus();
            //        }
            //        else
            //        {
            //            CurrentOpenedForm.Close();
            //            CurrentOpenedForm.Dispose();

            //            CurrentOpenedForm = new CanDataUdsSendForm { Name = formName };
            //            CurrentOpenedForm.Show();
            //        }
            //    }
            //}
        }

        private void ucBtnMotorolaMatrix_BtnClick(object sender, EventArgs e)
        {

        }

        private void ucBtnIntelMatrix_BtnClick(object sender, EventArgs e)
        {

        }

        private void ucBtnProduct_BtnClick(object sender, EventArgs e)
        {
            if (Lin == null)
            {
                MessageBox.Show(@"请先打开设备！");
            }
            else
            {
                const string formName = "ProductSendForm";

                if (CurrentOpenedForm == null)
                {
                    CurrentOpenedForm = new LinDataProductForm(Lin) { Name = formName };
                    CurrentOpenedForm.Show();
                }
                else
                {
                    if (CurrentOpenedForm.Name == formName)
                    {
                        if (CurrentOpenedForm.IsDisposed)
                        {
                            CurrentOpenedForm = new LinDataProductForm(Lin) { Name = formName };
                            CurrentOpenedForm.Show();
                        }
                        CurrentOpenedForm.WindowState = FormWindowState.Normal;
                        CurrentOpenedForm.Focus();
                    }
                    else
                    {
                        CurrentOpenedForm.Close();
                        CurrentOpenedForm.Dispose();

                        CurrentOpenedForm = new LinDataProductForm(Lin) { Name = formName };
                        CurrentOpenedForm.Show();
                    }
                }
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

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnClearLinData_Click(null, null);
        }
    }
}
