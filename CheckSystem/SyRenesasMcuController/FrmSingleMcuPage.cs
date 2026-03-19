using Controller;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace CheckSystem.SyRenesasMcuController
{
    public sealed partial class FrmSingleMcuPage : UIPage
    {
        private SyRenesasMcuControllerMaster ControllerMaster { get; set; }

        public FrmSingleMcuPage(string name)
        {
            InitializeComponent();
            Text = name;
            ControllerMaster = new SyRenesasMcuControllerMaster(name);
            Load += FrmSingleMcuPage_Load;
        }

        private void FrmSingleMcuPage_Load(object sender, EventArgs e)
        {
            InitWriteFlashUi();
        }

        private void InitWriteFlashUi()
        {
            cmbSlaveType.SelectedIndexChanged += CmbSlaveType_SelectedIndexChanged;
            cmbSlaveType.SelectedIndex = 0;
            cmbSlaveCanFuncType.SelectedIndex = 0;
            cmbToUpdateLinCh.SelectedIndex = 0;
        }

        private void CmbSlaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSlaveIDs.Items.Clear();
            if (cmbSlaveType.SelectedIndex == 0)
            {
                cmbSlaveIDs.Items.Add("0x201");
                cmbSlaveIDs.Items.Add("0x202");
                cmbSlaveIDs.Items.Add("0x203");
                cmbSlaveIDs.Items.Add("0x204");
                cmbSlaveIDs.Items.Add("0x205");
                cmbSlaveIDs.Items.Add("0x206");
                cmbSlaveIDs.Items.Add("0x207");
                cmbSlaveIDs.Items.Add("0x208");
                cmbSlaveIDs.Items.Add("0x209");
                cmbSlaveIDs.Items.Add("0x20A");
                cmbSlaveIDs.Items.Add("0x20B");
                cmbSlaveIDs.Items.Add("0x20C");
                cmbSlaveIDs.Items.Add("0x20D");
                cmbSlaveIDs.Items.Add("0x20E");
                cmbSlaveIDs.Items.Add("0x20F");
            }
            else
            {
                cmbSlaveIDs.Items.Add("0x101");
                cmbSlaveIDs.Items.Add("0x102");
                cmbSlaveIDs.Items.Add("0x103");
                cmbSlaveIDs.Items.Add("0x104");
                cmbSlaveIDs.Items.Add("0x105");
                cmbSlaveIDs.Items.Add("0x106");
                cmbSlaveIDs.Items.Add("0x107");
                cmbSlaveIDs.Items.Add("0x108");
                cmbSlaveIDs.Items.Add("0x109");
                cmbSlaveIDs.Items.Add("0x10A");
                cmbSlaveIDs.Items.Add("0x10B");
                cmbSlaveIDs.Items.Add("0x10C");
                cmbSlaveIDs.Items.Add("0x10D");
                cmbSlaveIDs.Items.Add("0x10E");
                cmbSlaveIDs.Items.Add("0x10F");
            }

            cmbSlaveIDs.SelectedIndex = 0;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                ControllerMaster.InitRemoteIpAddress(txtIp.Text + ":8088");

                if (!ControllerMaster.IsConnected)
                    return;

                txtIp.Enabled = false;
                btnConnect.Enabled = false;
                uiTabControl1.Enabled = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void btnMasterReadDIs_Click(object sender, EventArgs e)
        {
            ControllerMaster.MasterReadDIs();

            for (var i = 1; i <= 6; i++)
            {
                var controlName = string.Format("di{0}", i);
                var fieldName = string.Format("Di{0}", i);

                foreach (var c in diTablePanel.Controls)
                {
                    if (c is UILight)
                    {
                        var uiLight = c as UILight;
                        if (uiLight.Name == controlName)
                        {
                            var field = ControllerMaster.GetType().GetField(fieldName);
                            if (field != null)
                            {
                                var fieldValue = field.GetValue(ControllerMaster);
                                if (fieldValue != null)
                                {
                                    if (fieldValue.ToString() == "1")
                                        uiLight.State = UILightState.On;
                                    else if (fieldValue.ToString() == "0")
                                        uiLight.State = UILightState.Off;
                                    else
                                        uiLight.State = UILightState.Blink;
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void btnMasterSetDOs_Click(object sender, EventArgs e)
        {
            ControllerMaster.Do1 = do1.Active;
            ControllerMaster.Do2 = do2.Active;
            ControllerMaster.Do3 = do3.Active;
            ControllerMaster.Do4 = do4.Active;
            ControllerMaster.MasetSetDOs();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var option = new UIEditOption { AutoLabelWidth = true, Text = @"添加RL从站" };

            var lsitSlaveIDs = new List<string>
            {
                "0x201",
                "0x202",
                "0x203",
                "0x204",
                "0x205",
                "0x206",
                "0x207",
                "0x208",
                "0x209",
                "0x20A",
                "0x20B",
                "0x20C",
                "0x20D",
                "0x20E",
                "0x20F",
            };
            option.AddCombobox("RL从站ID", "RL从站ID：", lsitSlaveIDs.ToArray(), 0, true, true);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                this.ShowInfoTip("操作取消");
                return;
            }

            var rlSlaveId = lsitSlaveIDs[(int)frm["RL从站ID"]];

            var rlName = "RL从站_" + rlSlaveId;
            var newRl = new SyRenesasMcuControllerSlaveWith8RLs(rlName);
            if (!newRl.ConnectMaster(txtIp.Text, rlSlaveId))
            {
                this.ShowErrorTip("新增失败:" + rlName);
                return;
            }

            uiFlowLayoutPanel1.Controls.Add(new UcRlControl(newRl, rlSlaveId));
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ControllerMaster.SetSlavesRLs();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //var ucAD = new UcADControl();

            //uiFlowLayoutPanel2.Controls.Add(ucAD);

            var option = new UIEditOption { AutoLabelWidth = true, Text = @"添加RL从站" };

            var lsitSlaveIDs = new List<string>
            {
                "0x101",
                "0x102",
                "0x103",
                "0x104",
                "0x105",
                "0x106",
                "0x107",
                "0x108",
                "0x109",
                "0x10A",
                "0x10B",
                "0x10C",
                "0x10D",
                "0x10E",
                "0x10F",
            };
            option.AddCombobox("AD从站ID", "AD从站ID：", lsitSlaveIDs.ToArray(), 0, true, true);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                this.ShowInfoTip("操作取消");
                return;
            }

            var adSlaveId = lsitSlaveIDs[(int)frm["AD从站ID"]];

            var adName = "AD从站_" + adSlaveId;
            var newAd = new SyRenesasMcuControllerSlaveWith12ADs(adName);
            if (!newAd.ConnectMaster(txtIp.Text, adSlaveId))
            {
                this.ShowErrorTip("新增失败:" + adName);
                return;
            }

            uiFlowLayoutPanel2.Controls.Add(new UcAdControl(newAd, adSlaveId));
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ControllerMaster.ReadSlavesADs();

            foreach (var c in uiFlowLayoutPanel2.FlowLayoutPanel.Controls)
            {
                if (c.GetType() != typeof(UcAdControl))
                    continue;
                var adControl = c as UcAdControl;
                if (adControl == null)
                    continue;
                adControl.RefreshADs();
            }
        }

        private void btnWriteSlaveID_Click(object sender, EventArgs e)
        {
            if (this.ShowAskDialog(string.Format("是否要写入从站ID={0}\r\n写入从站ID时，请确保一次只有一个从站与主站相连!!!", cmbSlaveIDs.Items[cmbSlaveIDs.SelectedIndex].ToString())))
            {
                ControllerMaster.SetSlaveId(cmbSlaveIDs.Items[cmbSlaveIDs.SelectedIndex].ToString());

                this.ShowSuccessTip("写入完成，请重新上电后确认");
                return;
            }

            this.ShowInfoTip("操作已取消");
        }

        private void btnWriteSlaveCanFunc_Click(object sender, EventArgs e)
        {
            if (ShowAskDialog(string.Format("是否要更新从CAN的功能为：{0}!!!", cmbSlaveCanFuncType.Items[cmbSlaveCanFuncType.SelectedIndex].ToString())))
            {
                var type = cmbSlaveCanFuncType.SelectedIndex == 0 ? 0 : 1;

                var bauteRete = (uint)500000;
                bauteRete = cmbSlaveCanFuncType.SelectedIndex == 2 ? bauteRete * 2 : bauteRete;

                ControllerMaster.SetSlaveCanFunc(type.ToString(), bauteRete);

                this.ShowSuccessTip("更新完成，请重新上电后确认");
                return;
            }

            this.ShowInfoTip("操作已取消");
        }

        private void btnWriteUartCanConfig_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdateLinBauteRate_Click(object sender, EventArgs e)
        {
            if (this.ShowAskDialog(string.Format("是否要更新{0},波特率为：{1}!!!", cmbToUpdateLinCh.Items[cmbToUpdateLinCh.SelectedIndex], txtToUpdateLinBauteRate.Text)))
            {
                ControllerMaster.SetLinBauteRate(cmbToUpdateLinCh.SelectedIndex, uint.Parse(txtToUpdateLinBauteRate.Text));

                this.ShowSuccessTip("更新完成，请重新上电后确认");
                return;
            }

            this.ShowInfoTip("操作已取消");
        }
    }
}
