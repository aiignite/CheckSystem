using System;
using System.Windows.Forms;
using Controller;
using Sunny.UI;

namespace CheckSystem.SyController
{
    public partial class SyControllerDeviceMgrForm : UIForm
    {
        public object InitController { get; set; }

        public SyControllerDeviceMgrForm(string typeName)
        {
            InitializeComponent();

            ucmbDevicesList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmbDevicesList.comboBox.Items.Add(typeName);
            if (ucmbDevicesList.comboBox.Items.Count > 0)
                ucmbDevicesList.comboBox.SelectedIndex = 0;

            for (var i = 28; i < 50; i++)
                ucmbIpAddrList.comboBox.Items.Add(string.Format("192.168.1.{0}:8088", i));
            ucmbIpAddrList.comboBox.Items.Add("127.0.0.1:502");
            if (ucmbIpAddrList.comboBox.Items.Count > 0)
                ucmbIpAddrList.comboBox.SelectedIndex = 0;
        }

        private void CanDeviceMgrForm_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_BtnClick(object sender, EventArgs e)
        {
            try
            {
                if (ucmbDevicesList.comboBox.Text == typeof(ControllerWithGateway).Name)
                {
                    InitController = new ControllerWithGateway("ControllerWithGateway");
                    ((ControllerWithGateway)InitController).InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);

                }
                else if (ucmbDevicesList.comboBox.Text == typeof(SyControllerWith56Pin).Name)
                {
                    InitController = new SyControllerWith56Pin("SyControllerWith56Pin");
                    ((SyControllerWith56Pin)InitController).InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);
                }
                else if (ucmbDevicesList.comboBox.Text == typeof(SyControllerMaster).Name)
                {
                    InitController = new SyControllerMaster("SyControllerMaster");
                    ((SyControllerMaster)InitController).InitRemoteIpAddr(ucmbIpAddrList.comboBox.Text);
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
