using System;
using System.Drawing;
using System.Windows.Forms;
using CommonUtility.FileOperator;
using Controller;
using HZH_Controls.IconFont;

namespace CheckSystem.HelperForms.Tld7002
{
    public partial class Tld7002DeviceMgr : Form
    {
        public Tld7002DeviceMgr()
        {
            InitializeComponent();

            Icon = FontImages.GetIcon(FontIcons.E_icon_table, 32,
               Color.DodgerBlue);

            ucmbDevicesList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ucmbDevicesList.comboBox.Items.Add(typeof(SyControllerWith56Pin).Name);
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
        }

        private void btnConnect_BtnClick(object sender, EventArgs e)
        {
            try
            {
                using (var controller = new SyControllerWith56Pin(""))
                {
                    controller.InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);
                    if (controller.IsConnected)
                    {
                        var frm = new Tld7002OtpForm(controller);
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

        private void btnReadCfgAndConnect_BtnClick(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = @"文本文件|*.tld7002",
                Multiselect = false,
            };
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            Tld7002Config obj = null;
            try
            {
                obj = XmlHelper.Deserialize<Tld7002Config>(fileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"打开配置文件失败，" + ex.Message);
            }

            if (obj != null)
            {
                try
                {
                    using (var controller = new SyControllerWith56Pin(""))
                    {
                        controller.InitRemoteIpAddress(ucmbIpAddrList.comboBox.Text);
                        if (controller.IsConnected)
                        {
                            var frm = new Tld7002OtpForm(controller, obj, fileDialog.FileName);
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
            else
            {
                MessageBox.Show(@"打开配置文件失败");
            }
        }
    }
}
