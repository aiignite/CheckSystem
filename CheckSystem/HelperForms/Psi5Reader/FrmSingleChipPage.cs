using Controller;
using Sunny.UI;
using System;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.Psi5Reader
{
    public partial class FrmSingleChipPage : UIPage
    {
        private readonly SyPsi5InterfaceWithE52140 _psi5Interface =
            new SyPsi5InterfaceWithE52140(Guid.NewGuid().ToString());

        public FrmSingleChipPage()
        {
            InitializeComponent();
            InitChannels();
            DisableChannelControls();
            btnConfigChip.Enabled = false;
        }

        private void InitChannels()
        {
            var ucControl1 = new UcSingleChannel(1, _psi5Interface) { Dock = DockStyle.Fill };
            uiTableLayoutPanel1.SetColumn(ucControl1, 0);
            uiTableLayoutPanel1.SetRow(ucControl1, 1);
            uiTableLayoutPanel1.Controls.Add(ucControl1);

            var ucControl2 = new UcSingleChannel(2, _psi5Interface) { Dock = DockStyle.Fill };
            uiTableLayoutPanel1.SetColumn(ucControl2, 1);
            uiTableLayoutPanel1.SetRow(ucControl2, 1);
            uiTableLayoutPanel1.Controls.Add(ucControl2);

            var ucControl3 = new UcSingleChannel(3, _psi5Interface) { Dock = DockStyle.Fill };
            uiTableLayoutPanel1.SetColumn(ucControl3, 0);
            uiTableLayoutPanel1.SetRow(ucControl3, 2);
            uiTableLayoutPanel1.Controls.Add(ucControl3);

            var ucControl4 = new UcSingleChannel(4, _psi5Interface) { Dock = DockStyle.Fill };
            uiTableLayoutPanel1.SetColumn(ucControl4, 1);
            uiTableLayoutPanel1.SetRow(ucControl4, 2);
            uiTableLayoutPanel1.Controls.Add(ucControl4);
        }

        private void DisableChannelControls()
        {
            SwitchChannelControls(false);
        }

        private void EnableChannelControls()
        {
            SwitchChannelControls(true);
        }

        private void SwitchChannelControls(bool isEnable)
        {
            foreach (var t in uiTableLayoutPanel1.Controls)
            {
                if (t.GetType() != typeof(UcSingleChannel))
                    continue;

                var c = t as UcSingleChannel;
                c.Enabled = isEnable;
                //if (t is UcSingleChannel c)
                //{
                //    c.Enabled = isEnable;
                //}
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //_psi5Interface.ConnectInterface(txtIp.Value + ":502");

            _psi5Interface.ConnectInterface(txtIp.Value + ":502");
            btnConnect.Enabled = false;
            btnConfigChip.Enabled = true;
            txtIp.Enabled = false;
        }

        private void btnConfigChip_Click(object sender, System.EventArgs e)
        {
            _psi5Interface.SetVBus();
            _psi5Interface.SetAllCh189Kps();
            _psi5Interface.EnableSyncPulseChargePumpAndAllCh();

            EnableChannelControls();
        }
    }
}
