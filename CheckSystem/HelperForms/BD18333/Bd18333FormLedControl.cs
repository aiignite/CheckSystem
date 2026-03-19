using CommonUtility;
using Controller;
using HZH_Controls.Controls.Checkbox;
using HZH_Controls.IconFont;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.HelperForms.BD18333
{
    public partial class Bd18333FormLedControl : UIForm
    {
        private readonly List<UCCheckBox> _checkedAddr = new List<UCCheckBox>();

        public Bd18333FormLedControl()
        {
            InitializeComponent();
            Load += Bd18333FormLedControl_Load;
        }

        private void Bd18333FormLedControl_Load(object sender, EventArgs e)
        {
            Icon = FontImages.GetIcon(FontIcons.E_icon_table, 32,
                Color.DodgerBlue);

            ucmbDevicesList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmbDevicesList.comboBox.Items.Add(typeof(SyControllerWith56Pin).Name);
            ucmbDevicesList.comboBox.Items.Add(typeof(SyRenesasMcuControllerMaster).Name);
            if (ucmbDevicesList.comboBox.Items.Count > 0)
                ucmbDevicesList.comboBox.SelectedIndex = 0;

            //ucmbIpAddrList.comboBox.Items.Add("192.168.1.30:8088");
            ucmbIpAddrList.comboBox.Items.Add("192.168.1.28:8088");
            ucmbIpAddrList.comboBox.Items.Add("192.168.1.29:8088");
            ucmbIpAddrList.comboBox.Items.Add("192.168.1.30:8088");
            ucmbIpAddrList.comboBox.Items.Add("192.168.1.31:8088");
            ucmbIpAddrList.comboBox.Items.Add("192.168.1.32:8088");
            ucmbIpAddrList.comboBox.Items.Add("192.168.1.150:8088");
            ucmbIpAddrList.comboBox.Items.Add("127.0.0.1:8088");

            ucmbIpAddrList.comboBox.SelectedIndex = 0;

            for (var i = 0; i < 15; i++)
            {
                var checkBox = new UCCheckBox
                {
                    Name = i.ToString(),
                    TextValue =
                        string.Format("{0}dec, {1}h, {2}b", i, ValueHelper.GetHextStrWithOx((byte)i),
                            Convert.ToString((byte)i, 2).PadLeft(8, '0').Substring(2, 6)),
                    //Dock = DockStyle.Top,
                    //TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("微软雅黑", 12, FontStyle.Regular),
                    Width = 250
                };
                _checkedAddr.Add(checkBox);
            }

            flowLayoutPanel1.WrapContents = true;
            foreach (var c in _checkedAddr)
            {
                flowLayoutPanel1.Controls.Add(c);
            }
        }

        private void btnConnect_BtnClick(object sender, EventArgs e)
        {
            var devs = (from c in _checkedAddr where c.Checked select c.Name).ToList();
            var devsStr = (from c in _checkedAddr where c.Checked select c.TextValue).ToList();

            if (!devs.Any())
            {
                MessageBox.Show(@"连接失败,请选择至少一个地址");
                return;
            }

            try
            {
                if (ucmbDevicesList.comboBox.SelectedIndex == 0)
                {
                    using (var controller = new SyControllerWith56Pin("56pin"))
                    {
                        controller.InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);
                        if (controller.IsConnected)
                        {
                            var bd18331Euv = new Bd18333Euv("BD18331Euv") { UartCan = controller.GatewaySci2 };
                            foreach (var dev in devs)
                                bd18331Euv.AddDev(dev);

                            var frm = new Bd18333CanDeviceMgrForm(devsStr, bd18331Euv, controller);
                            frm.ShowDialog();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show(@"连接失败");
                        }
                    }
                }
                else
                {
                    using (var controller = new SyRenesasMcuControllerMaster("syr"))
                    {
                        controller.InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);
                        if (controller.IsConnected)
                        {
                            var bd18331Euv = new Bd18333Euv("BD18331Euv") { UartCan = controller.UartCan };
                            foreach (var dev in devs)
                                bd18331Euv.AddDev(dev);

                            var frm = new Bd18333CanDeviceMgrForm(devsStr, bd18331Euv, controller);
                            frm.ShowDialog();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show(@"连接失败");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"连接失败:" + ex.Message);
            }
        }
    }
}
