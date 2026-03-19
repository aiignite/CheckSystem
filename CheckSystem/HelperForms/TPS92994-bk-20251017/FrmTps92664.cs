using CheckSystem.HelperForms.Tps92662;
using CommonUtility;
using Controller;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.TPS92994
{
    public partial class FrmTps92664 : UIForm
    {
        //private readonly SyRenesasMcuControllerMaster _controllerMaster = new SyRenesasMcuControllerMaster("master");
        private readonly SyControllerWith56Pin _controllerMaster = new SyControllerWith56Pin("master");
        private readonly MatrixTps92664 _matrixTps = new MatrixTps92664("TPS92664/92665");
        private readonly List<LedControlWithPwm> _ledCtrl = new List<LedControlWithPwm>();
        private ushort _nowDev = 0xFF;

        public FrmTps92664()
        {
            InitializeComponent();

            for (var i = 0; i <= 7; i++)
                cmbAddrList.Items.Add(string.Format("DEC='{0}'; HEX={1}; BIN ='{2}';", i, ValueHelper.GetHextStrWithOx((byte)i), Convert.ToString(i, 2).PadLeft(3, '0')));
            cmbAddrList.SelectedIndex = 0;

            for (var i = 0; i < 16; i++)
            {
                var ledCtrl = new LedControlWithPwm(i, "OUT" + i, 1023, 1023) { Enabled = false };
                ledCtrl.PhaseValueChanged += LedCtrl_PhaseValueChanged;
                ledCtrl.WidthValueChanged += LedCtrl_WidthValueChanged;
                _ledCtrl.Add(ledCtrl);
                gpLeds.Controls.Add(ledCtrl);
            }
            SizeChanged += FrmTps92664_SizeChanged;
            btnStart.Click += BtnStart_Click;
            btnStop.Click += BtnStop_Click;
            FormClosing += FrmTps92664_FormClosing;
            FormClosed += FrmTps92664_FormClosed;

            Load += FrmTps92664_Load;
        }

        private void FrmTps92664_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UIMessageBox.ShowAsk("是否关闭窗口？"))
                return;

            e.Cancel = true;
        }

        private void FrmTps92664_Load(object sender, EventArgs e)
        {
            try
            {
                _controllerMaster.InitRemoteIpAddress("192.168.1.28:8088");
                //_matrixTps.MySerialPort = _controllerMaster.UartCan;
                _matrixTps.MySerialPort = _controllerMaster.GatewaySci2;
            }
            catch (Exception)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = false;
                cmbAddrList.Enabled = false;
                Style = UIStyle.Red;
                UIMessageBox.ShowError("控制器初始化失败");
            }
        }

        private void FrmTps92664_FormClosed(object sender, FormClosedEventArgs e)
        {
            _matrixTps?.SysNotActive();
            _matrixTps?.Dispose();
        }

        private void LedCtrl_WidthValueChanged(object sender, LedControlWithPwm.WidthValueChangeActionEventArgs e)
        {
            var index = e.Index;
            var value = e.Value;
            _matrixTps?.TargetDevSingleChWidth(_nowDev.ToString(), (index + 1).ToString(), value);
        }

        private void LedCtrl_PhaseValueChanged(object sender, LedControlWithPwm.PhaseValueChangeActionEventArgs e)
        {
            var index = e.Index;
            var value = e.Value;
            _matrixTps?.TargetDevSingleChPhase(_nowDev.ToString(), (index + 1).ToString(), value);
        }

        private void FrmTps92664_SizeChanged(object sender, EventArgs e)
        {
            Width = (int)(Screen.PrimaryScreen.WorkingArea.Width * 1.0);
            Height = Screen.PrimaryScreen.WorkingArea.Height;

            var color1 = Color.FromArgb(255, 255, 255);
            var color2 = Color.FromArgb(235, 243, 255);

            gpLeds.Location = new Point(lblAddr.Location.X, lblAddr.Location.Y + 30);
            gpLeds.Size = new Size(Width - (lblAddr.Location.X * 2 + 10), Height - gpLeds.Location.Y - 20);

            for (var i = 0; i < _ledCtrl.Count / 2; i++)
            {
                var perWidth = gpLeds.Size.Width / (_ledCtrl.Count / 2);
                var ledCtrl = _ledCtrl[i];
                ledCtrl.Location = new Point(perWidth * i + 5, 0 + 30);
                ledCtrl.Width = perWidth - 10;
                ledCtrl.Height = (gpLeds.Size.Height - 50) / 2;

                ledCtrl.groupBox1.BackColor = i % 2 == 0 ? color1 : color2;
            }

            for (var i = _ledCtrl.Count / 2; i < _ledCtrl.Count; i++)
            {
                var upLedCtrl = _ledCtrl[i - _ledCtrl.Count / 2];
                var ledCtrl = _ledCtrl[i];

                ledCtrl.Location = new Point(upLedCtrl.Location.X, upLedCtrl.Location.Y + upLedCtrl.Height + 15);
                ledCtrl.Width = upLedCtrl.Width;
                ledCtrl.Height = upLedCtrl.Height;

                ledCtrl.groupBox1.BackColor = i % 2 != 0 ? color1 : color2;
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            cmbAddrList.Enabled = false;
            btnStart.Style = UIStyle.Green;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            foreach (var item in _ledCtrl)
                item.Enabled = true;
            _nowDev = (ushort)cmbAddrList.SelectedIndex;
            _matrixTps.AddDev(_nowDev.ToString());
            _matrixTps.SysConfig();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            foreach (var item in _ledCtrl)
                item.btnOnOff.Active = false;
            _matrixTps.SysNotActive();
            _matrixTps.Remove(_nowDev.ToString());
            _nowDev = 0xFF;
            cmbAddrList.Enabled = true;
            btnStart.Style = UIStyle.Blue;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            foreach (var item in _ledCtrl)
                item.Enabled = false;
        }
    }
}
