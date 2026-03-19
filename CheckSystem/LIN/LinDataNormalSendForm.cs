using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility.BusLoader;
using HZH_Controls.IconFont;

namespace CheckSystem.LIN
{
    public partial class LinDataNormalSendForm : Form
    {
        public LinBus Lin { get; set; }
        private string LastTxtMasterLinId { get; set; }
        private string LastTxtSlaveLinId { get; set; }
        private string LastTxtLinData { get; set; }

        private readonly string[] _charList =
        {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E",
            "F"
        };

        public LinDataNormalSendForm(LinBus lin)
        {
            InitializeComponent();
            Lin = lin;
            Icon = FontImages.GetIcon(FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);
            txtMasterLinId.textBox.TextChanged += txtMaxterLinId_TextChanged;
            txtSlaveLinId.textBox.TextChanged += txtSlaveLinId_TextChanged;
            txtLinData.textBox.TextChanged += txtLinData_TextChanged;

            cmbMasterSlaveDelayMs.label.Font = new Font("微软雅黑", 9);
            cmbMasterSlaveDelayMs.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            for (var i = 1; i < 101; i++)
                cmbMasterSlaveDelayMs.comboBox.Items.Add(i * 50);
            cmbMasterSlaveDelayMs.comboBox.SelectedIndex = 0;
        }

        private void txtLinData_TextChanged(object sender, EventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox == null || string.IsNullOrEmpty(txtBox.Text))
                return;
            foreach (var c in txtBox.Text)
            {
                if (!_charList.Contains(c.ToString().ToUpper()))
                {
                    txtBox.Text = LastTxtLinData;
                    return;
                }

                if (txtBox.Text.Length > 16)
                {
                    txtBox.Text = LastTxtLinData;
                    return;
                }
            }

            LastTxtLinData = txtBox.Text;
        }

        private void txtMaxterLinId_TextChanged(object sender, EventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox == null || string.IsNullOrEmpty(txtBox.Text))
                return;
            foreach (var c in txtBox.Text)
            {
                if (!_charList.Contains(c.ToString().ToUpper()))
                {
                    txtBox.Text = LastTxtMasterLinId;
                    return;
                }

                if (txtBox.Text.Length > 2)
                {
                    txtBox.Text = LastTxtMasterLinId;
                    return;
                }
            }

            LastTxtMasterLinId = txtBox.Text;
        }

        private void txtSlaveLinId_TextChanged(object sender, EventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox == null || string.IsNullOrEmpty(txtBox.Text))
                return;
            foreach (var c in txtBox.Text)
            {
                if (!_charList.Contains(c.ToString().ToUpper()))
                {
                    txtBox.Text = LastTxtSlaveLinId;
                    return;
                }

                if (txtBox.Text.Length <= 2) 
                    continue;
                txtBox.Text = LastTxtSlaveLinId;
                return;
            }

            LastTxtSlaveLinId = txtBox.Text;
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMasterLinId.textBox.Text))
                return;

            if (rbSendSlaveLin.Checked && !rbSendMasterLin.Checked &&
                string.IsNullOrEmpty(txtSlaveLinId.textBox.Text))
                return;

            int sendTimes;
            if (string.IsNullOrEmpty(txtSendTimes.textBox.Text) ||
                !int.TryParse(txtSendTimes.textBox.Text, out sendTimes))
                sendTimes = 1;

            int interval;
            if (string.IsNullOrEmpty(txtSendInterval.textBox.Text) ||
                !int.TryParse(txtSendInterval.textBox.Text, out interval))
                interval = 0;

            var masterLinId = Convert.ToByte(txtMasterLinId.textBox.Text, 16);
            var slaveLinId = string.IsNullOrEmpty(txtSlaveLinId.textBox.Text)
                ? (byte)0x00
                : Convert.ToByte(txtSlaveLinId.textBox.Text, 16);

            var datas = new List<byte>();
            var dataStr = txtLinData.textBox.Text;

            if (!string.IsNullOrEmpty(dataStr))
            {
                if (dataStr.Length % 2 != 0)
                {
                    dataStr += "0";
                    txtLinData.textBox.Text = dataStr;
                }
            }

            if (!string.IsNullOrEmpty(dataStr))
            {
                for (var i = 0; i < dataStr.Length; i = i + 2)
                {
                    var br = string.Format("{0}{1}", dataStr[i], dataStr[i + 1]);
                    datas.Add(Convert.ToByte(br, 16));
                }
            }

            SendData(masterLinId, slaveLinId, datas.ToArray(), sendTimes, interval);
            //countForm.ShowDialog();
            //countForm.IsStop = true;
            //Thread.Sleep(50);
        }

        private async void SendData(
            byte masterLinId, byte slaveLinId, byte[] datas, int count, int interval)
        {
            Enabled = false;

            await Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        for (var i = 0; i < count; i++)
                        {
                            if (!rbSendSlaveLin.Checked && rbSendMasterLin.Checked)
                                Lin.SendMasterLin(masterLinId, datas);
                            else if (rbSendSlaveLin.Checked && !rbSendMasterLin.Checked)
                            {
                                byte[] echo;
                                Lin.SendMasterLinAndRecvSingleSlaveLin(
                                    masterLinId, slaveLinId, datas, out echo, interval == 0 ? 0 : int.Parse(cmbMasterSlaveDelayMs.comboBox.Text));
                            }

                            if (interval != 0)
                                Thread.Sleep(interval);

                        }
                    }));
                }
            });

            Enabled = true;
        }
    }
}
