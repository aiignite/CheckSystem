using System;
using System.Windows.Forms;
using CommonUtility;
using Controller;
using Sunny.UI;

namespace CheckSystem.UART
{
    public partial class UartDeviceMgrForm : UIForm
    {
        public MySerialPort MySerialPort { get; set; }
        private ControllerBase Controller { get; set; }

        public UartDeviceMgrForm()
        {
            InitializeComponent();

            ucmbDevicesList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmbDevicesList.comboBox.Items.Add(typeof(SyControllerWith56Pin).Name);
            //ucmbDevicesList.comboBox.Items.Add(typeof(ZlgUsbCanFd200U).Name);
            //ucmbDevicesList.comboBox.Items.Add(typeof(ToomossUsb2XxxCanLin).Name);
            //ucmbDevicesList.comboBox.Items.Add(typeof(ControllerWithGateway).Name);
            //ucmbDevicesList.comboBox.Items.Add(typeof(SyControllerMaster).Name);

            ucmbDevicesList.comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            if (ucmbDevicesList.comboBox.Items.Count > 0)
                ucmbDevicesList.comboBox.SelectedIndex = 0;

            for (var i = 28; i < 50; i++)
                ucmbIpAddrList.comboBox.Items.Add(string.Format("192.168.1.{0}:8088", i));
            ucmbIpAddrList.comboBox.Items.Add("127.0.0.1:502");
            if (ucmbIpAddrList.comboBox.Items.Count > 0)
                ucmbIpAddrList.comboBox.SelectedIndex = 0;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ucmbDevicesList.comboBox.Text == typeof(ZlgUsbCanFd200U).Name ||
                ucmbDevicesList.comboBox.Text == typeof(ToomossUsb2XxxCanLin).Name)
            {
                ucmbIpAddrList.Enabled = false;
            }
            else
            {
                ucmbIpAddrList.Enabled = true;
            }
        }

        public new void Dispose()
        {
            if (Controller != null)
            {
                Controller.Dispose();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (ucmbDevicesList.comboBox.Text == typeof(SyControllerWith56Pin).Name)
                {
                    var gw = new SyControllerWith56Pin("SyControllerWith56Pin");
                    gw.InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);
                    var propertyInfo = GetType().GetProperty("MySerialPort");
                    if (propertyInfo != null)
                        propertyInfo.SetValue(this, gw.GetType().GetField("GatewaySci2").GetValue(gw));
                    Controller = gw;

                    //var matrix92664 = new MatrixTps92664("90664");
                    //matrix92664.MySerialPort = gw.GatewaySci2;
                    //matrix92664.AddDev("2");
                    //matrix92664.AddDev("3");
                    //matrix92664.AddDev("4");
                    //matrix92664.SysConfig();
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
