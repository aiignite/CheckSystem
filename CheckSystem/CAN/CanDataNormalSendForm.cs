using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility.BusLoader;
using HZH_Controls.IconFont;

namespace CheckSystem.CAN
{
    public partial class CanDataNormalSendForm : Form
    {
        public CanBus Can { get; set; }
        private string LastTxtCanId { get; set; }
        private string LastTxtCanData { get; set; }

        private readonly string[] _charList =
        {
            "0", "1", "2", "3",
            "4", "5", "6", "7",
            "8", "9", "A", "B",
            "C", "D", "E", "F"
        };

        public CanDataNormalSendForm(CanBus can)
        {
            InitializeComponent();
            Can = can;
            Icon = FontImages.GetIcon(FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);
            txtCanType.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            txtCanType.comboBox.Items.Add("标准帧");
            txtCanType.comboBox.Items.Add("扩展帧");
            txtCanType.comboBox.SelectedIndex = 0;

            txtCanId.textBox.TextChanged += txtCanId_TextChanged;
            txtCanData.textBox.TextChanged += txtCanData_TextChanged;
        }

        private void txtCanData_TextChanged(object sender, EventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox == null || string.IsNullOrEmpty(txtBox.Text))
                return;
            foreach (var c in txtBox.Text)
            {
                if (!_charList.Contains(c.ToString().ToUpper()))
                {
                    txtBox.Text = LastTxtCanData;
                    return;
                }

                if (txtBox.Text.Length > 16)
                {
                    txtBox.Text = LastTxtCanData;
                    return;
                }
            }

            LastTxtCanData = txtBox.Text;
        }

        private void txtCanId_TextChanged(object sender, EventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox == null || string.IsNullOrEmpty(txtBox.Text))
                return;
            foreach (var c in txtBox.Text)
            {
                if (!_charList.Contains(c.ToString().ToUpper()))
                {
                    txtBox.Text = LastTxtCanId;
                    return;
                }

                if (txtBox.Text.Length > 8)
                {
                    txtBox.Text = LastTxtCanId;
                    return;
                }
            }

            LastTxtCanId = txtBox.Text;
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCanId.textBox.Text))
                return;

            int sendTimes;
            if (string.IsNullOrEmpty(txtSendTimes.textBox.Text) ||
                !int.TryParse(txtSendTimes.textBox.Text, out sendTimes))
                sendTimes = 1;

            int interval;
            if (string.IsNullOrEmpty(txtSendInterval.textBox.Text) ||
                !int.TryParse(txtSendInterval.textBox.Text, out interval))
                interval = 1;

            var canId = Convert.ToUInt32(txtCanId.textBox.Text, 16);

            var canType = CanBus.CanType.Standard;
            if (txtCanType.comboBox.Text == @"扩展帧")
                canType = CanBus.CanType.Extended;

            var datas = new List<byte>();
            var dataStr = txtCanData.textBox.Text;

            if (!string.IsNullOrEmpty(dataStr))
            {
                if (dataStr.Length % 2 != 0)
                {
                    dataStr += "0";
                    txtCanData.textBox.Text = dataStr;
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

            SendData(canId, canType, datas.ToArray(), sendTimes, interval);
        }

        private async void SendData(
            uint canId, CanBus.CanType canType, byte[] datas, int count, int interval)
        {
            Enabled = false;


            await Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        var frmLoading = new CanDataNormalSendCountForm();
                        frmLoading.SendMaxValue(count);

                        var isBreak = false;

                        frmLoading.BackgroundWorkAction = delegate
                        {
                            for (var i = 0; i < count; i++)
                            {
                                try
                                {
                                    Can.SendCanDatas(
                                        new[]
                                        {
                                            new CanBus.CanDataPackage(canId, CanBus.CanProtocol.Can, canType,
                                                CanBus.CanFormat.Data, datas.ToArray())
                                        });
                                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(i + 1, (i + 1).ToString());
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }

                                Thread.Sleep(interval);

                                if (isBreak)
                                    break;
                            }
                        };

                        frmLoading.ShowDialog();
                        isBreak = true;
                    }));
                }
            });

            Enabled = true;
        }
    }
}
