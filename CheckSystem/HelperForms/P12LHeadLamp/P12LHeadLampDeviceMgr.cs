using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CommonUtility.BusLoader;
using Controller;
using HZH_Controls.IconFont;

namespace CheckSystem.HelperForms.P12LHeadLamp
{
    public partial class P12LHeadLampDeviceMgr : Form
    {
        public CanBus InitCan1 { get; set; }
        public CanBus InitCan2 { get; set; }
        public LinBus InitLin5 { get; set; }
        public LinBus InitLin6 { get; set; }
        private ControllerBase Controller { get; set; }

        public P12LHeadLampDeviceMgr()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(
                FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);

            ucmLin5List.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmLin6List.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmbCAN1List.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmbCAN2List.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            ucmbDevicesList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmbDevicesList.comboBox.Items.Add(typeof(ZlgUsbCanFd200U).Name);
            ucmbDevicesList.comboBox.Items.Add(typeof(ToomossUsb2XxxCanLin).Name);

            ucmbDevicesList.comboBox.SelectedIndexChanged += ucmbDevicesListComboBox_SelectedIndexChanged;
            if (ucmbDevicesList.comboBox.Items.Count > 0)
                ucmbDevicesList.comboBox.SelectedIndex = 0;

            ucmbProductLevel.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmbProductLevel.comboBox.SelectedIndexChanged += prodcutLevelComboBox_SelectedIndexChanged;
            ucmbProductLevel.comboBox.Items.Add("高配");
            ucmbProductLevel.comboBox.Items.Add("低配");
            ucmbProductLevel.comboBox.Items.Add("融合方案");
            ucmbProductLevel.comboBox.SelectedIndex = 0;
        }

        private void CanDeviceMgrForm_Load(object sender, EventArgs e)
        {

        }

        private void prodcutLevelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ucmbProductLevel.comboBox.SelectedIndex == 1) // 低配
            {
                ucmbDevicesList.comboBox.Items.Clear();
                ucmbDevicesList.comboBox.Items.Add(typeof(ToomossUsb2XxxCanLin).Name);
            }
            else if (ucmbProductLevel.comboBox.SelectedIndex == 0)// 高配
            {
                ucmbDevicesList.comboBox.Items.Clear();
                ucmbDevicesList.comboBox.Items.Add(typeof(ZlgUsbCanFd200U).Name);
                ucmbDevicesList.comboBox.Items.Add(typeof(ToomossUsb2XxxCanLin).Name);
            }
            else if (ucmbProductLevel.comboBox.SelectedIndex == 2)// 融合
            {
                ucmbDevicesList.comboBox.Items.Clear();
                ucmbDevicesList.comboBox.Items.Add(typeof(ZlgUsbCanFd200U).Name);
                ucmbDevicesList.comboBox.Items.Add(typeof(ToomossUsb2XxxCanLin).Name);
            }

            ucmbDevicesList.comboBox.SelectedIndex = 0;
        }

        private void ucmbDevicesListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ucmbCAN1List.comboBox.Items.Clear();
            ucmbCAN2List.comboBox.Items.Clear();
            ucmLin5List.comboBox.Items.Clear();
            ucmLin6List.comboBox.Items.Clear();

            if (ucmbProductLevel.comboBox.SelectedIndex == 0) // 高配
            {
                ucmbCAN1List.label.Text = @"信号灯CAN1通道：";
                ucmbCAN2List.label.Text = @"远近光CAN2通道：";
                ucmbCAN1List.Visible = true;
                ucmbCAN2List.Visible = true;

                if (ucmbDevicesList.comboBox.Text == typeof(ZlgUsbCanFd200U).Name)
                {
                    ucmLin5List.Visible = false;
                    ucmLin6List.Visible = false;

                    foreach (var f in typeof(ZlgUsbCanFd200U).GetFields().Where(f => f.FieldType == typeof(CanBus)))
                    {
                        ucmbCAN1List.comboBox.Items.Add(f.Name);
                        ucmbCAN2List.comboBox.Items.Add(f.Name);
                    }
                }
                else if (ucmbDevicesList.comboBox.Text == typeof(ToomossUsb2XxxCanLin).Name)
                {
                    ucmLin5List.Visible = false;
                    ucmLin6List.Visible = true;

                    foreach (var f in typeof(ToomossUsb2XxxCanLin).GetFields().Where(f => f.FieldType == typeof(CanBus)))
                    {
                        ucmbCAN1List.comboBox.Items.Add(f.Name);
                        ucmbCAN2List.comboBox.Items.Add(f.Name);
                    }

                    foreach (var f in typeof(ToomossUsb2XxxCanLin).GetFields().Where(f => f.FieldType == typeof(LinBus)))
                    {
                        ucmLin6List.comboBox.Items.Add(f.Name);
                    }
                }
            }
            else if (ucmbProductLevel.comboBox.SelectedIndex == 1)// 低配
            {
                ucmbCAN1List.Visible = false;
                ucmbCAN2List.Visible = false;
                ucmLin5List.Visible = true;
                ucmLin6List.Visible = true;

                if (ucmbDevicesList.comboBox.Text == typeof(ToomossUsb2XxxCanLin).Name)
                {
                    foreach (var f in typeof(ToomossUsb2XxxCanLin).GetFields().Where(f => f.FieldType == typeof(LinBus)))
                    {
                        ucmLin5List.comboBox.Items.Add(f.Name);
                        ucmLin6List.comboBox.Items.Add(f.Name);
                    }
                }
            }
            else if (ucmbProductLevel.comboBox.SelectedIndex == 2)// 融合
            {
                ucmbCAN1List.label.Text = @"CAN通道：";
                ucmbCAN1List.Visible = true;
                ucmbCAN2List.Visible = false;

                if (ucmbDevicesList.comboBox.Text == typeof(ZlgUsbCanFd200U).Name)
                {
                    ucmLin5List.Visible = false;
                    ucmLin6List.Visible = false;

                    foreach (var f in typeof(ZlgUsbCanFd200U).GetFields().Where(f => f.FieldType == typeof(CanBus)))
                    {
                        ucmbCAN1List.comboBox.Items.Add(f.Name);
                        ucmbCAN2List.comboBox.Items.Add(f.Name);
                    }
                }
                else if (ucmbDevicesList.comboBox.Text == typeof(ToomossUsb2XxxCanLin).Name)
                {
                    ucmLin5List.Visible = false;
                    ucmLin6List.Visible = false;

                    foreach (var f in typeof(ToomossUsb2XxxCanLin).GetFields().Where(f => f.FieldType == typeof(CanBus)))
                    {
                        ucmbCAN1List.comboBox.Items.Add(f.Name);
                        ucmbCAN2List.comboBox.Items.Add(f.Name);
                    }

                    foreach (var f in typeof(ToomossUsb2XxxCanLin).GetFields().Where(f => f.FieldType == typeof(LinBus)))
                    {
                        ucmLin6List.comboBox.Items.Add(f.Name);
                    }
                }
            }

            if (ucmLin6List.comboBox.Items.Count > 0)
                ucmLin6List.comboBox.SelectedIndex = 0;

            if (ucmLin5List.comboBox.Items.Count > 0)
                ucmLin5List.comboBox.SelectedIndex = 0;

            if (ucmLin5List.comboBox.Items.Count > 1)
                ucmLin5List.comboBox.SelectedIndex = 1;

            if (ucmbCAN1List.comboBox.Items.Count > 0)
                ucmbCAN1List.comboBox.SelectedIndex = 0;

            if (ucmbCAN2List.comboBox.Items.Count > 0)
                ucmbCAN2List.comboBox.SelectedIndex = 0;

            if (ucmbCAN2List.comboBox.Items.Count > 1)
                ucmbCAN2List.comboBox.SelectedIndex = 1;
        }

        private void btnConnect_BtnClick(object sender, EventArgs e)
        {
            try
            {
                if (ucmbProductLevel.comboBox.SelectedIndex == 0) // 高配
                {
                    if (ucmbDevicesList.comboBox.Text == typeof(ZlgUsbCanFd200U).Name)
                    {
                        var gw = new ZlgUsbCanFd200U("ZlgUsbCanFd200U");

                        var propertyInfo1 = GetType().GetProperty("InitCan1");
                        if (propertyInfo1 != null)
                            propertyInfo1.SetValue(this, gw.GetType().GetField(ucmbCAN1List.comboBox.Text).GetValue(gw));

                        var propertyInfo2 = GetType().GetProperty("InitCan2");
                        if (propertyInfo2 != null)
                            propertyInfo2.SetValue(this, gw.GetType().GetField(ucmbCAN2List.comboBox.Text).GetValue(gw));

                        Controller = gw;
                    }
                    else if (ucmbDevicesList.comboBox.Text == typeof(ToomossUsb2XxxCanLin).Name)
                    {
                        var gw = new ToomossUsb2XxxCanLin("ToomossUsb2XxxCanLin");
                        //gw.InitRemoteIpAddr(ucmbIpAddrList.comboBox.Text);
                        var propertyInfo1 = GetType().GetProperty("InitCan1");
                        if (propertyInfo1 != null)
                            propertyInfo1.SetValue(this, gw.GetType().GetField(ucmbCAN1List.comboBox.Text).GetValue(gw));

                        var propertyInfo2 = GetType().GetProperty("InitCan2");
                        if (propertyInfo2 != null)
                            propertyInfo2.SetValue(this, gw.GetType().GetField(ucmbCAN2List.comboBox.Text).GetValue(gw));

                        var propertyInfo3 = GetType().GetProperty("InitLin6");
                        if (propertyInfo3 != null)
                            propertyInfo3.SetValue(this, gw.GetType().GetField(ucmLin6List.comboBox.Text).GetValue(gw));

                        Controller = gw;
                    }

                    using (var frm = new P12LHighLevelHeadLamp(InitCan1, InitCan2, InitLin6, true))
                        frm.ShowDialog();
                    Close();
                }
                else if (ucmbProductLevel.comboBox.SelectedIndex == 1)// 低配
                {
                    if (ucmbDevicesList.comboBox.Text == typeof(ToomossUsb2XxxCanLin).Name)
                    {
                        var gw = new ToomossUsb2XxxCanLin("ToomossUsb2XxxCanLin");

                        var propertyInfo3 = GetType().GetProperty("InitLin6");
                        if (propertyInfo3 != null)
                            propertyInfo3.SetValue(this, gw.GetType().GetField(ucmLin6List.comboBox.Text).GetValue(gw));

                        var propertyInfo4 = GetType().GetProperty("InitLin5");
                        if (propertyInfo4 != null)
                            propertyInfo4.SetValue(this, gw.GetType().GetField(ucmLin5List.comboBox.Text).GetValue(gw));

                        Controller = gw;
                    }

                    using (var frm = new P12LLowLevelHeadLamp(InitLin6, InitLin5))
                        frm.ShowDialog();
                    Close();
                }
                else if (ucmbProductLevel.comboBox.SelectedIndex == 2) // 融合
                {
                    if (ucmbDevicesList.comboBox.Text == typeof(ZlgUsbCanFd200U).Name)
                    {
                        var gw = new ZlgUsbCanFd200U("ZlgUsbCanFd200U");

                        var propertyInfo1 = GetType().GetProperty("InitCan1");
                        if (propertyInfo1 != null)
                            propertyInfo1.SetValue(this, gw.GetType().GetField(ucmbCAN1List.comboBox.Text).GetValue(gw));

                        Controller = gw;
                    }
                    else if (ucmbDevicesList.comboBox.Text == typeof(ToomossUsb2XxxCanLin).Name)
                    {
                        var gw = new ToomossUsb2XxxCanLin("ToomossUsb2XxxCanLin");
                        //gw.InitRemoteIpAddr(ucmbIpAddrList.comboBox.Text);
                        var propertyInfo1 = GetType().GetProperty("InitCan1");
                        if (propertyInfo1 != null)
                            propertyInfo1.SetValue(this, gw.GetType().GetField(ucmbCAN1List.comboBox.Text).GetValue(gw));

                        Controller = gw;
                    }

                    using (var frm = new P12LHighLevelHeadLamp(InitCan1, null, null, false))
                        frm.ShowDialog();
                    Close();
                }
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
