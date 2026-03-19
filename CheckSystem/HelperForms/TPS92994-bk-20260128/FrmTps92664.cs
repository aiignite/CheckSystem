using CheckSystem.HelperForms.Tps92662;
using CommonUtility;
using Controller;
using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.TPS92994
{
    public partial class FrmTps92664 : UIForm
    {
        private readonly SyRenesasMcuControllerMaster _controllerMaster = new SyRenesasMcuControllerMaster("master");
        //private readonly SyControllerWith56Pin _controllerMaster = new SyControllerWith56Pin("master");
        private readonly Tps92664Box _matrixTps = new Tps92664Box("TPS92664/92665");
        private readonly List<Tps92664WidthCtrl> _ledCtrl = new List<Tps92664WidthCtrl>();
        private ushort _nowDev = 0xFF;

        private Tps92664ConfigData _configData;
        private readonly string _configFilePath;

        public FrmTps92664()
        {
            InitializeComponent();

            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;

            // 配置文件路径
            var configDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TpsConfig");
            if (!Directory.Exists(configDir))
                Directory.CreateDirectory(configDir);
            _configFilePath = Path.Combine(configDir, "Tps92664Config.json");
            LoadConfigFromFile(false);

            for (var i = 0; i <= 7; i++)
                cmbAddrList.Items.Add(string.Format("DEC='{0}'; HEX={1}; BIN ='{2}';", i, ValueHelper.GetHextStrWithOx((byte)i), Convert.ToString(i, 2).PadLeft(3, '0')));
            cmbAddrList.SelectedIndex = 0;

            //foreach (var item in _configData.HighBeam.Channels)
            //{
            //    var ledCtrl = new Tps92664WidthCtrl(item.ChannelName, 0, int.Parse(item.Channel.ToString())) { Enabled = false };
            //    ledCtrl.Visible = false;
            //    ledCtrl.WidthValueChanged += LedCtrl_WidthValueChanged;
            //    _ledCtrl.Add(ledCtrl);
            //    gpLeds.Controls.Add(ledCtrl);
            //}

            for (var i = 0; i < 16; i++)
            {
                var item = _configData.HighBeam.GetChannel(i);
                var ledCtrl = new Tps92664WidthCtrl(item.ChannelName, 0, int.Parse(item.Channel.ToString()), item.PWM) { Enabled = false };
                ledCtrl.Visible = !(i == 0 || i == 1);
                ledCtrl.WidthValueChanged += LedCtrl_WidthValueChanged;
                _ledCtrl.Add(ledCtrl);
                gpLeds.Controls.Add(ledCtrl);
            }
            SizeChanged += FrmTps92664_SizeChanged;
            btnStart.Click += BtnStart_Click;
            btnStop.Click += BtnStop_Click;
            btnConfig.Click += BtnConfig_Click;
            btnAll.Click += BtnConfig_Click1;
            btnAllOff.Click += BtnAllOff_Click;
            btnBifurcation.Click += BtnBifurcation_Click;
            FormClosing += FrmTps92664_FormClosing;
            FormClosed += FrmTps92664_FormClosed;

            Load += FrmTps92664_Load;
        }

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (_matrixTps != null && _matrixTps.MySerialPort != null && _matrixTps.MySerialPort.Name == name)
            {
                var value = ValueHelper.GetHextStr(datas);
                Console.WriteLine("uart data: {0}", value);

                if (_nowDev != 0xFF && datas.Length == 54 / 2 && datas[0] == 0x7A && datas[1] == 0x20 + _nowDev && datas[2] == 0x29)
                {
                    var lstBytes = new List<byte>();
                    for (var i = 5; i < 5 + 20; i++)
                        lstBytes.Add(datas[i]);

                    var width04To01LBits = Convert.ToString(lstBytes[16], 2).PadLeft(8, '0');
                    var width08To05LBits = Convert.ToString(lstBytes[17], 2).PadLeft(8, '0');
                    var width12To09LBits = Convert.ToString(lstBytes[18], 2).PadLeft(8, '0');
                    var width16To13LBits = Convert.ToString(lstBytes[19], 2).PadLeft(8, '0');

                    var lstWidth = new List<ushort>
                    {
                        Convert.ToUInt16(Convert.ToString(lstBytes[0], 2).PadLeft(8, '0') + width04To01LBits.Substring(6, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[1], 2).PadLeft(8, '0') + width04To01LBits.Substring(4, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[2], 2).PadLeft(8, '0') + width04To01LBits.Substring(2, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[3], 2).PadLeft(8, '0') + width04To01LBits.Substring(0, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[4], 2).PadLeft(8, '0') + width08To05LBits.Substring(6, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[5], 2).PadLeft(8, '0') + width08To05LBits.Substring(4, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[6], 2).PadLeft(8, '0') + width08To05LBits.Substring(2, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[7], 2).PadLeft(8, '0') + width08To05LBits.Substring(0, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[8], 2).PadLeft(8, '0') + width12To09LBits.Substring(6, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[9], 2).PadLeft(8, '0') + width12To09LBits.Substring(4, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[10], 2).PadLeft(8, '0') + width12To09LBits.Substring(2, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[11], 2).PadLeft(8, '0') + width12To09LBits.Substring(0, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[12], 2).PadLeft(8, '0') + width16To13LBits.Substring(6, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[13], 2).PadLeft(8, '0') + width16To13LBits.Substring(4, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[14], 2).PadLeft(8, '0') + width16To13LBits.Substring(2, 2), 2),
                        Convert.ToUInt16(Convert.ToString(lstBytes[15], 2).PadLeft(8, '0') + width16To13LBits.Substring(0, 2), 2)
                    };

                    for (var i = 0; i < 16; i++)
                        _ledCtrl[i].SetPer(lstWidth[i]);
                }
            }
        }

        private void BtnAllOff_Click(object sender, EventArgs e)
        {
            _isAllOn = true;

            for (var i = 0; i < 16; i++)
            {
                _ledCtrl[i].LedOff();
                _matrixTps.TargetDevSingleChWidth(_nowDev.ToString(), (i + 1).ToString(), 0);
            }
            _matrixTps.SendCmdWidth();

            _isAllOn = false;
        }

        private void BtnConfig_Click1(object sender, EventArgs e)
        {
            var str = new List<string>();
            for (int i = 100; i >= 80; i--)
                str.Add(string.Format("Base * {0}%", i));

            var option = new UIEditOption();
            option.AddCombobox("per", "点亮效果", str.ToArray(), 0);

            var uiOption = new UIEditForm(option);
            uiOption.Render();
            uiOption.ShowDialog();
            var uiResult = uiOption.IsOK;
            if (!uiResult)
            {
                ShowInfoTip("操作取消");
                return;
            }

            var per = 100 - (int)uiOption["per"];

            _isAllOn = true;

            for (var i = 0; i < 16; i++)
            {
                _ledCtrl[i].WidthVal = (int)(_configData.HighBeam.GetChannel(i).PWM * per * 0.01);
                _matrixTps.TargetDevSingleChWidth(_nowDev.ToString(), (i + 1).ToString(), (int)(_configData.HighBeam.GetChannel(i).PWM * per * 0.01));
            }
            _matrixTps.SendCmdWidth();

            _isAllOn = false;
        }

        private async void BtnBifurcation_Click(object sender, EventArgs e)
        {
            btnAllOff.PerformClick();

            btnAll.Enabled = btnAllOff.Enabled = btnConfig.Enabled = false;
            btnStop.Enabled = false;

            btnBifurcation.Enabled = false;
            btnBifurcation.Style = UIStyle.Green;

            Thread.Sleep(50);
            await Bifurcation();

            btnAll.Enabled = btnAllOff.Enabled = btnConfig.Enabled = true;
            btnStop.Enabled = true;

            btnBifurcation.Enabled = true;
            btnBifurcation.Style = UIStyle.Orange;
        }

        private Task Bifurcation()
        {
            return Task.Run(async () =>
            {
                var tEnter = HighPrecisionTimer.GetTimestamp();
                for (var i = 1; i < 15; i++)
                {
                    var t1 = HighPrecisionTimer.GetTimestamp();
                    var chName = string.Format("CH" + (i));
                    var ctrl = _ledCtrl.Find(f => f.ShowName == chName);
                    if (ctrl is null)
                        continue;

                    if (InvokeRequired)
                    {
                        ctrl.Invoke(new Action(() =>
                        {
                            ctrl.WidthVal = (int)(_configData.HighBeam.GetChannel(ctrl.LedIndex).PWM * 100 * 0.01);
                        }));
                    }
                    else
                    {
                        ctrl.WidthVal = (int)(_configData.HighBeam.GetChannel(ctrl.LedIndex).PWM * 100 * 0.01);
                    }

                    _matrixTps.TargetDevSingleChWidth(_nowDev.ToString(), (ctrl.LedIndex + 1).ToString(), (int)(_configData.HighBeam.GetChannel(ctrl.LedIndex).PWM * 100 * 0.01));
                    _matrixTps.SendCmdWidth();

                    await Task.Run(async () =>
                    {
                        while (true)
                        {
                            await Task.Delay(1);
                            var t2 = HighPrecisionTimer.GetTimestamp();
                            if (HighPrecisionTimer.GetTimestampIntervalMs(t1, t2) >= _configData.HighBeam.GetChannel(ctrl.LedIndex).Ts1)
                                break;
                        }
                    });
                }

                var keepTs1 = HighPrecisionTimer.GetTimestamp();
                Console.WriteLine("从第一颗点亮到全部点亮，耗时：" + HighPrecisionTimer.GetTimestampIntervalMs(tEnter, keepTs1));

                // 点亮后保持到1.0s
                await Task.Run(async () =>
                {
                    while (true)
                    {
                        await Task.Delay(1);
                        var t2 = HighPrecisionTimer.GetTimestamp();
                        if (HighPrecisionTimer.GetTimestampIntervalMs(tEnter, t2) >= _configData.KEEPONTS1)
                            break;
                    }
                });

                var keepTs2 = HighPrecisionTimer.GetTimestamp();
                Console.WriteLine("从第一颗点亮到全部点亮并保持1.0s，耗时：" + HighPrecisionTimer.GetTimestampIntervalMs(keepTs1, keepTs2));

                // 1.0s起全部熄灭至1.5s
                if (InvokeRequired)
                {
                    btnAllOff.Invoke(new Action(() =>
                    {
                        btnAllOff.Enabled = true;
                        btnAllOff.PerformClick();
                        btnAllOff.Enabled = false;
                    }));
                }
                else
                {
                    btnAllOff.Enabled = true;
                    btnAllOff.PerformClick();
                    btnAllOff.Enabled = false;
                }
                await Task.Run(async () =>
                {
                    while (true)
                    {
                        await Task.Delay(1);
                        var t2 = HighPrecisionTimer.GetTimestamp();
                        if (HighPrecisionTimer.GetTimestampIntervalMs(tEnter, t2) >= _configData.KEEPOFFTS1)
                            break;
                    }
                });

                for (var i = 14; i >= 1; i--)
                {
                    var t1 = HighPrecisionTimer.GetTimestamp();
                    var chName = string.Format("CH" + (i));
                    var ctrl = _ledCtrl.Find(f => f.ShowName == chName);
                    if (ctrl is null)
                        continue;

                    if (InvokeRequired)
                    {
                        ctrl.Invoke(new Action(() =>
                        {
                            ctrl.WidthVal = (int)(_configData.HighBeam.GetChannel(ctrl.LedIndex).PWM * 100 * 0.01);
                        }));
                    }
                    else
                    {
                        ctrl.WidthVal = (int)(_configData.HighBeam.GetChannel(ctrl.LedIndex).PWM * 100 * 0.01);
                    }

                    _matrixTps.TargetDevSingleChWidth(_nowDev.ToString(), (ctrl.LedIndex + 1).ToString(), (int)(_configData.HighBeam.GetChannel(ctrl.LedIndex).PWM * 100 * 0.01));
                    _matrixTps.SendCmdWidth();

                    await Task.Run(async () =>
                    {
                        while (true)
                        {
                            await Task.Delay(1);
                            var t2 = HighPrecisionTimer.GetTimestamp();
                            if (HighPrecisionTimer.GetTimestampIntervalMs(t1, t2) >= _configData.HighBeam.GetChannel(ctrl.LedIndex).Ts2)
                                break;
                        }
                    });
                }

                // 点亮后保持到2.5s
                await Task.Run(async () =>
                {
                    while (true)
                    {
                        await Task.Delay(1);
                        var t2 = HighPrecisionTimer.GetTimestamp();
                        if (HighPrecisionTimer.GetTimestampIntervalMs(tEnter, t2) >= _configData.KEEPONTS2)
                            break;
                    }
                });

                // 2.5s起全部熄灭，动画结束
                if (InvokeRequired)
                {
                    btnAllOff.Invoke(new Action(() =>
                    {
                        btnAllOff.Enabled = true;
                        btnAllOff.PerformClick();
                        btnAllOff.Enabled = false;
                    }));
                }
                else
                {
                    btnAllOff.Enabled = true;
                    btnAllOff.PerformClick();
                    btnAllOff.Enabled = false;
                }
            });
        }

        private bool _isAllOn = false;

        /// <summary>
        /// 从文件加载配置
        /// </summary>
        /// <param name="showMessage">是否显示加载成功消息</param>
        private void LoadConfigFromFile(bool showMessage = true)
        {
            try
            {
                if (File.Exists(_configFilePath))
                {
                    var json = File.ReadAllText(_configFilePath);
                    _configData = JsonConvert.DeserializeObject<Tps92664ConfigData>(json);
                    if (_configData == null)
                        _configData = Tps92664ConfigData.CreateDefault();

                    // 确保有HighBeam配置
                    if (_configData.HighBeam == null)
                        _configData.HighBeam = BeamConfig.CreateDefault();

                    // 确保有16个通道
                    if (_configData.HighBeam.Channels == null || _configData.HighBeam.Channels.Count != 16)
                        _configData.HighBeam = BeamConfig.CreateDefault();

                    if (showMessage)
                        UIMessageBox.ShowSuccess("配置已加载");
                }
                else
                {
                    _configData = Tps92664ConfigData.CreateDefault();
                }
            }
            catch (Exception ex)
            {
                if (showMessage)
                    UIMessageBox.ShowError($"加载失败: {ex.Message}");

                _configData = Tps92664ConfigData.CreateDefault();
            }
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
                _matrixTps.MySerialPort = _controllerMaster.UartCan;
                //_matrixTps.MySerialPort = _controllerMaster.GatewaySci2;
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

        private async void LedCtrl_WidthValueChanged(object sender, LedControlWithPwm.WidthValueChangeActionEventArgs e)
        {
            if (_isAllOn)
                return;

            var index = e.Index;
            var value = e.Value;
            _matrixTps?.TargetDevSingleChWidth(_nowDev.ToString(), (index + 1).ToString(), value);
            //_matrixTps?.SendCmdWidth();
            await Task.Run(() => { _matrixTps?.SendCmdWidth(); });
        }

        private void FrmTps92664_SizeChanged(object sender, EventArgs e)
        {
            Width = (int)(Screen.PrimaryScreen.WorkingArea.Width * 1.0);
            Height = Screen.PrimaryScreen.WorkingArea.Height;

            var color1 = Color.FromArgb(255, 255, 255);
            var color2 = Color.FromArgb(235, 243, 255);

            gpLeds.Location = new Point(0, btnStop.Location.Y + btnStop.Height + 5);
            //gpLeds.Size = new Size(Width - (lblAddr.Location.X * 2 + 10), Height - gpLeds.Location.Y - 20);
            gpLeds.Size = new Size(Width - 3, Height - gpLeds.Location.Y - 2);

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
            btnAll.Enabled = btnAllOff.Enabled = true;
            btnBifurcation.Enabled = true;

            // 启用所有LED控件，并根据PWM Width值自动设置开关状态
            foreach (var item in _ledCtrl)
            {
                item.Enabled = true;
                item.LedOff();
            }

            _nowDev = (ushort)cmbAddrList.SelectedIndex;
            _matrixTps.AddDev(_nowDev.ToString());

            for (var i = 0; i < 16; i++)
            {
                var cfg = _configData.HighBeam.GetChannel(i);
                _matrixTps.TargetDevSingleChPhase(_nowDev.ToString(), i.ToString(), cfg.Phase);
            }

            _matrixTps.SysConfig();

            timer1.Enabled = true;
            timer1.Start();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            foreach (var item in _ledCtrl)
            {
                item.btnOnOff.Active = false; item.LedOff();
            }
            _matrixTps.SysNotActive();
            _matrixTps.Remove(_nowDev.ToString());
            _nowDev = 0xFF;
            cmbAddrList.Enabled = true;
            btnStart.Style = UIStyle.Blue;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnAll.Enabled = btnAllOff.Enabled = false;
            btnBifurcation.Enabled = false;

            timer1.Enabled = false;
            timer1.Stop();
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            try
            {
                // 打开配置窗口
                var configForm = new FrmTps92664Config(_configFilePath);
                configForm.ShowDialog();
                LoadConfigFromFile(false);
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError($"打开配置窗口失败: {ex.Message}");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_nowDev == 0xFF)
                return;

            _matrixTps.SendCmdReadWidth();
        }
    }
}
