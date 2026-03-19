using System;
using System.Drawing;
using System.Windows.Forms;
using Controller;
using HZH_Controls.IconFont;

namespace CheckSystem.RobotForms
{
    public partial class RobotControllerDeviceMgrForm : Form
    {
        public RobotControllerDeviceMgrForm()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_icon_table, 32,
               Color.DodgerBlue);

            ucmbDevicesList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmbDevicesList.comboBox.Items.Add(typeof(RobotControllerPpMode).Name);
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

            ucmbLocalIpAddrList.comboBox.Items.Add("192.168.1.50:8088");
            ucmbLocalIpAddrList.comboBox.Items.Add("192.168.1.50:5028");
            ucmbLocalIpAddrList.comboBox.Items.Add("192.168.1.50:5029");
            ucmbLocalIpAddrList.comboBox.Items.Add("192.168.1.50:5030");
            ucmbLocalIpAddrList.comboBox.Items.Add("192.168.1.50:5031");
            ucmbLocalIpAddrList.comboBox.Items.Add("192.168.1.50:5032");
            ucmbLocalIpAddrList.comboBox.Items.Add("192.168.1.150:8089");
            ucmbLocalIpAddrList.comboBox.Items.Add("127.0.0.1:8089");

            ucmbIpAddrList.comboBox.SelectedIndex = 0;
            ucmbLocalIpAddrList.comboBox.SelectedIndex = 0;
        }

        private void RobotControllerDeviceMgrForm_Load(object sender, EventArgs e)
        {
        }

        private void btnConnect_BtnClick(object sender, EventArgs e)
        {
            try
            {
                using (var robotController = new RobotControllerPpMode(""))
                {
                    robotController.InitLocalLocalIpAddress(ucmbLocalIpAddrList.comboBox.Text);
                    robotController.InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);
                    if (robotController.RtexUdpClient != null)
                    {
                        var frm = new RobotControllerMainForm(robotController);
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
