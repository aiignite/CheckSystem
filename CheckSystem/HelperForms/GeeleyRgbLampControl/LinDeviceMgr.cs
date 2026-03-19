using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CommonUtility.BusLoader;
using Controller;
using HZH_Controls.IconFont;

namespace CheckSystem.HelperForms.GeeleyRgbLampControl
{
    public partial class LinDeviceMgr : Form
    {
        public LinBus InitLin { get; set; }
        public ControllerBase Controller { get; set; }

        public LinDeviceMgr()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);
            ucmbLINList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            ucmbDevicesList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmbDevicesList.comboBox.Items.Add(typeof(SyControllerWith56Pin).Name);
            ucmbDevicesList.comboBox.Items.Add(typeof(ToomossUsb2XxxCanLin).Name);
            ucmbDevicesList.comboBox.Items.Add(typeof(ControllerWithGateway).Name);
            ucmbDevicesList.comboBox.Items.Add(typeof(SyControllerMaster).Name);
            
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
            ucmbLINList.comboBox.Items.Clear();

            if (ucmbDevicesList.comboBox.Text == typeof(ControllerWithGateway).Name)
                foreach (var f in typeof(ControllerWithGateway).GetFields().Where(f => f.FieldType == typeof(LinBus)))
                    ucmbLINList.comboBox.Items.Add(f.Name);
            else if (ucmbDevicesList.comboBox.Text == typeof(SyControllerWith56Pin).Name)
                foreach (var f in typeof(SyControllerWith56Pin).GetFields().Where(f => f.FieldType == typeof(LinBus)))
                    ucmbLINList.comboBox.Items.Add(f.Name);
            else if (ucmbDevicesList.comboBox.Text == typeof(SyControllerMaster).Name)
                foreach (var f in typeof(SyControllerMaster).GetFields().Where(f => f.FieldType == typeof(LinBus)))
                    ucmbLINList.comboBox.Items.Add(f.Name);
            else if (ucmbDevicesList.comboBox.Text == typeof(ToomossUsb2XxxCanLin).Name)
                foreach (var f in typeof(ToomossUsb2XxxCanLin).GetFields().Where(f => f.FieldType == typeof(LinBus)))
                    ucmbLINList.comboBox.Items.Add(f.Name);

            if (ucmbLINList.comboBox.Items.Count > 0)
                ucmbLINList.comboBox.SelectedIndex = 0;

            ucmbIpAddrList.Enabled = ucmbDevicesList.comboBox.Text != typeof(ToomossUsb2XxxCanLin).Name;
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
                    var propertyInfo = GetType().GetProperty("InitLin");
                    if (propertyInfo != null)
                        propertyInfo.SetValue(this, gw.GetType().GetField(ucmbLINList.comboBox.Text).GetValue(gw));
                    Controller = gw;
                }
                else if (ucmbDevicesList.comboBox.Text == typeof(SyControllerWith56Pin).Name)
                {
                    var gw = new SyControllerWith56Pin("SyControllerWith56Pin");
                    gw.InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);
                    var propertyInfo = GetType().GetProperty("InitLin");
                    if (propertyInfo != null)
                        propertyInfo.SetValue(this, gw.GetType().GetField(ucmbLINList.comboBox.Text).GetValue(gw));
                    Controller = gw;
                }
                else if (ucmbDevicesList.comboBox.Text == typeof(SyControllerMaster).Name)
                {
                    var gw = new SyControllerMaster("SyControllerMaster");
                    gw.InitRemoteIpAddr(ucmbIpAddrList.comboBox.Text);
                    var propertyInfo = GetType().GetProperty("InitLin");
                    if (propertyInfo != null)
                        propertyInfo.SetValue(this, gw.GetType().GetField(ucmbLINList.comboBox.Text).GetValue(gw));
                    Controller = gw;
                }
                else if (ucmbDevicesList.comboBox.Text == typeof(ToomossUsb2XxxCanLin).Name)
                {
                    var gw = new ToomossUsb2XxxCanLin("ToomossUsb2XxxCanLin");
                    //gw.InitRemoteIpAddr(ucmbIpAddrList.comboBox.Text);
                    var propertyInfo = GetType().GetProperty("InitLin");
                    if (propertyInfo != null)
                        propertyInfo.SetValue(this, gw.GetType().GetField(ucmbLINList.comboBox.Text).GetValue(gw));
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
