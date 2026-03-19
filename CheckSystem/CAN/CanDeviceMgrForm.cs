using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CommonUtility.BusLoader;
using Controller;
using HZH_Controls.IconFont;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.CAN
{
    public partial class CanDeviceMgrForm : UIForm
    {
        public CanBus InitCan { get; set; }
        private ControllerBase Controller { get; set; }

        public CanDeviceMgrForm()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(
                FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);

            ucmbCANList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            ucmbDevicesList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmbDevicesList.comboBox.Items.Add(typeof(SyControllerWith56Pin).Name);
            ucmbDevicesList.comboBox.Items.Add(typeof(ZlgUsbCanFd200U).Name);
            ucmbDevicesList.comboBox.Items.Add(typeof(ToomossUsb2XxxCanLin).Name);
            ucmbDevicesList.comboBox.Items.Add(typeof(ControllerWithGateway).Name);
            ucmbDevicesList.comboBox.Items.Add(typeof(SyControllerMaster).Name);
            ucmbDevicesList.comboBox.Items.Add(typeof(SyRenesasMcuControllerMaster).Name);

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
            ucmbCANList.comboBox.Items.Clear();

            if (ucmbDevicesList.comboBox.Text == typeof(ControllerWithGateway).Name)
                foreach (var f in typeof(ControllerWithGateway).GetFields().Where(f => f.FieldType == typeof(CanBus)))
                    ucmbCANList.comboBox.Items.Add(f.Name);
            else if (ucmbDevicesList.comboBox.Text == typeof(SyControllerWith56Pin).Name)
                foreach (var f in typeof(SyControllerWith56Pin).GetFields().Where(f => f.FieldType == typeof(CanBus)))
                    ucmbCANList.comboBox.Items.Add(f.Name);
            else if (ucmbDevicesList.comboBox.Text == typeof(SyControllerMaster).Name)
                foreach (var f in typeof(SyControllerMaster).GetFields().Where(f => f.FieldType == typeof(CanBus)))
                    ucmbCANList.comboBox.Items.Add(f.Name);
            else if (ucmbDevicesList.comboBox.Text == typeof(ZlgUsbCanFd200U).Name)
                foreach (var f in typeof(ZlgUsbCanFd200U).GetFields().Where(f => f.FieldType == typeof(CanBus)))
                    ucmbCANList.comboBox.Items.Add(f.Name);
            else if (ucmbDevicesList.comboBox.Text == typeof(ToomossUsb2XxxCanLin).Name)
                foreach (var f in typeof(ToomossUsb2XxxCanLin).GetFields().Where(f => f.FieldType == typeof(CanBus)))
                    ucmbCANList.comboBox.Items.Add(f.Name);
            else if (ucmbDevicesList.comboBox.Text == typeof(SyRenesasMcuControllerMaster).Name)
                foreach (var f in typeof(SyRenesasMcuControllerMaster).GetFields().Where(f => f.FieldType == typeof(CanBus)))
                    ucmbCANList.comboBox.Items.Add(f.Name);

            if (ucmbCANList.comboBox.Items.Count > 0)
                ucmbCANList.comboBox.SelectedIndex = 0;

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

        private void CanDeviceMgrForm_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_BtnClick(object sender, EventArgs e)
        {
            try
            {
                if (ucmbDevicesList.comboBox.Text == typeof(ControllerWithGateway).Name)
                {
                    var gw = new ControllerWithGateway("ControllerWithGateway");
                    gw.InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);
                    var propertyInfo = GetType().GetProperty("InitCan");
                    if (propertyInfo != null)
                        propertyInfo.SetValue(this, gw.GetType().GetField(ucmbCANList.comboBox.Text).GetValue(gw));
                    Controller = gw;
                }
                else if (ucmbDevicesList.comboBox.Text == typeof(SyControllerWith56Pin).Name)
                {
                    var gw = new SyControllerWith56Pin("SyControllerWith56Pin");
                    gw.InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);
                    var propertyInfo = GetType().GetProperty("InitCan");
                    if (propertyInfo != null)
                        propertyInfo.SetValue(this, gw.GetType().GetField(ucmbCANList.comboBox.Text).GetValue(gw));
                    Controller = gw;
                }
                else if (ucmbDevicesList.comboBox.Text == typeof(SyControllerMaster).Name)
                {
                    var gw = new SyControllerMaster("SyControllerMaster");
                    gw.InitRemoteIpAddr(ucmbIpAddrList.comboBox.Text);
                    var propertyInfo = GetType().GetProperty("InitCan");
                    if (propertyInfo != null)
                        propertyInfo.SetValue(this, gw.GetType().GetField(ucmbCANList.comboBox.Text).GetValue(gw));
                    Controller = gw;
                }
                else if (ucmbDevicesList.comboBox.Text == typeof(ZlgUsbCanFd200U).Name)
                {
                    var gw = new ZlgUsbCanFd200U("ZlgUsbCanFd200U");
                    //gw.InitRemoteIpAddr(ucmbIpAddrList.comboBox.Text);
                    var propertyInfo = GetType().GetProperty("InitCan");
                    if (propertyInfo != null)
                        propertyInfo.SetValue(this, gw.GetType().GetField(ucmbCANList.comboBox.Text).GetValue(gw));
                    Controller = gw;
                }
                else if (ucmbDevicesList.comboBox.Text == typeof(ToomossUsb2XxxCanLin).Name)
                {
                    var gw = new ToomossUsb2XxxCanLin("ToomossUsb2XxxCanLin");
                    //gw.InitRemoteIpAddr(ucmbIpAddrList.comboBox.Text);
                    var propertyInfo = GetType().GetProperty("InitCan");
                    if (propertyInfo != null)
                        propertyInfo.SetValue(this, gw.GetType().GetField(ucmbCANList.comboBox.Text).GetValue(gw));
                    Controller = gw;
                }
                else if (ucmbDevicesList.comboBox.Text == typeof(SyRenesasMcuControllerMaster).Name)
                {
                    var gw = new SyRenesasMcuControllerMaster("SyRenesasMcuControllerMaster");
                    gw.InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);
                    var propertyInfo = GetType().GetProperty("InitCan");
                    if (propertyInfo != null)
                        propertyInfo.SetValue(this, gw.GetType().GetField(ucmbCANList.comboBox.Text).GetValue(gw));
                    Controller = gw;
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public new void Dispose()
        {
            if (Controller != null)
            {
                Controller.Dispose();
            }
        }
    }
}
