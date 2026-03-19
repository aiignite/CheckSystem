using Controller;
using HZH_Controls.IconFont;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.AnalogMax25608
{
    public partial class AnalogMax25608DeviceMgrcs : Form
    {
        public AnalogMax25608DeviceMgrcs()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_icon_table, 32,
               Color.DodgerBlue);

            ucmbDevicesList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmbDevicesList.comboBox.Items.Add(typeof(SyControllerWith56Pin).Name);
            if (ucmbDevicesList.comboBox.Items.Count > 0)
                ucmbDevicesList.comboBox.SelectedIndex = 0;

            ucmbIpAddrList.comboBox.Items.Add("192.168.1.30:8088");
            ucmbIpAddrList.comboBox.Items.Add("192.168.1.28:8088");
            ucmbIpAddrList.comboBox.Items.Add("192.168.1.29:8088");
            ucmbIpAddrList.comboBox.Items.Add("192.168.1.30:8088");
            ucmbIpAddrList.comboBox.Items.Add("192.168.1.31:8088");
            ucmbIpAddrList.comboBox.Items.Add("192.168.1.32:8088");
            ucmbIpAddrList.comboBox.Items.Add("192.168.1.150:8088");
            ucmbIpAddrList.comboBox.Items.Add("127.0.0.1:8088");

            ucmbIpAddrList.comboBox.SelectedIndex = 0;
        }

        private void btnConnect_BtnClick(object sender, EventArgs e)
        {
            try
            {
                using (var controller = new SyControllerWith56Pin("ControllerWith56Pin"))
                {
                    controller.InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);
                    if (controller.IsConnected)
                    {
                        var frm = new AnalogMax25608LedControl(controller);
                        frm.ShowDialog();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show(@"连接失败");
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
