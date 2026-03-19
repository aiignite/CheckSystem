using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CommonUtility.BusLoader;
using Controller;

namespace CheckSystem.HelperForms.GeeleyRgbLampControl
{
    public partial class GeeleyRgbLampControlForm : Form
    {
        private LinDeviceMgr ControllerMgrForm { get; set; }
        private LinBus Lin { get; set; }
        private readonly GeeleyAutoRgbLamp _geeleyAutoRgbLamp = new GeeleyAutoRgbLamp("GeeleyRgb");
        private bool _isLedOn;
        private bool _isAuto;
        private readonly Thread _autoThread;
        private int _keepCount;
        private int _currentIndex;

        public GeeleyRgbLampControlForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Closed += GeeleyRgbLampControlForm_Closed;

            using (ControllerMgrForm = new LinDeviceMgr())
            {
                if (ControllerMgrForm.ShowDialog() == DialogResult.OK && ControllerMgrForm.InitLin != null)
                {
                    MessageBox.Show(@"打开设备成功！");
                    Lin = ControllerMgrForm.InitLin;

                    cmbDelayTime.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbDelayTime.Items.Add(1000);
                    cmbDelayTime.Items.Add(500);
                    cmbDelayTime.Items.Add(1500);
                    cmbDelayTime.Items.Add(2000);
                    cmbDelayTime.Items.Add(2500);
                    cmbDelayTime.Items.Add(3000);
                    cmbDelayTime.SelectedIndex = 0;

                    listBox1.Items.Add(string.Format("{0},{1},{2}",
                        Color.Red.R, Color.Red.G, Color.Red.B));
                    listBox1.Items.Add(string.Format("{0},{1},{2}",
                        Color.Green.R, Color.Green.G, Color.Green.B));
                    listBox1.Items.Add(string.Format("{0},{1},{2}",
                        Color.Blue.R, Color.Blue.G, Color.Blue.B));
                    listBox1.Items.Add(string.Format("{0},{1},{2}",
                        Color.Aqua.R, Color.Aqua.G, Color.Aqua.B));
                    listBox1.Items.Add(string.Format("{0},{1},{2}",
                        Color.Yellow.R, Color.Yellow.G, Color.Yellow.B));
                    listBox1.Items.Add(string.Format("{0},{1},{2}",
                        Color.Chartreuse.R, Color.Chartreuse.G, Color.Chartreuse.B));
                    listBox1.Items.Add(string.Format("{0},{1},{2}",
                        Color.SteelBlue.R, Color.SteelBlue.G, Color.SteelBlue.B));

                    tBarRed.Minimum = 0;
                    tBarRed.Maximum = 255;

                    tBarGreen.Minimum = 0;
                    tBarGreen.Maximum = 255;

                    tBarBlue.Minimum = 0;
                    tBarBlue.Maximum = 255;

                    btnCurrentColor.BackColor = Color.Gray;
                    tBarRed.Value = btnCurrentColor.BackColor.R;
                    tBarGreen.Value = btnCurrentColor.BackColor.G;
                    tBarBlue.Value = btnCurrentColor.BackColor.B;
                    tBarRed.ValueChanged += tBar_ValueChanged;
                    tBarGreen.ValueChanged += tBar_ValueChanged;
                    tBarBlue.ValueChanged += tBar_ValueChanged;

                    _geeleyAutoRgbLamp.Lin = Lin;
                    //_syControllerWith56Pin.InitRemoteIpAddress("192.168.1.28:8088");
                    //_geeleyAutoRgbLamp.Lin = _syControllerWith56Pin.GatewayLin;
                    _geeleyAutoRgbLamp.LampAwake();

                    if (_autoThread != null)
                    {
                        _autoThread.Abort();
                        _autoThread.Join();
                    }

                    _autoThread = new Thread(MainWork) { IsBackground = true };
                    _autoThread.Start();
                }
                else
                {
                    MessageBox.Show(@"打开设备失败！");

                    tBarRed.Enabled = false;
                    tBarGreen.Enabled = false;
                    tBarRed.Enabled = false;
                    panel1.Enabled = false;
                }
            }
        }

        private void MainWork()
        {
            while (_autoThread.IsAlive)
            {
                Thread.Sleep(5);

                if (_isAuto)
                {
                    if (listBox1.Items.Count == 0)
                        continue;

                    //for (var i = 0; i < listBox1.Items.Count; i++)
                    {
                        var t = listBox1.Items[_currentIndex];
                        listBox1.SelectedIndex = _currentIndex;

                        var sp = t.ToString().Split(',');

                        btnCurrentColor.BackColor = Color.FromArgb(int.Parse(sp[0]), int.Parse(sp[1]), int.Parse(sp[2]));

                        tBarRed.Value = int.Parse(sp[0]);
                        tBarGreen.Value = int.Parse(sp[1]);
                        tBarBlue.Value = int.Parse(sp[2]);

                        //tBarRed.Value = btnCurrentColor.BackColor.R;
                        //tBarGreen.Value = btnCurrentColor.BackColor.G;
                        //tBarBlue.Value = btnCurrentColor.BackColor.B;

                        SetRgb();
                        _keepCount++;
                        if (_keepCount * 5 >= int.Parse(cmbDelayTime.Text))
                        {
                            _currentIndex++;
                            _keepCount = 0;
                        }
                        if (_currentIndex == listBox1.Items.Count)
                        {
                            _currentIndex = 0;
                        }
                        //Thread.Sleep(int.Parse(cmbDelayTime.Text));
                    }
                }
                else
                {
                    if (_isLedOn)
                    {
                        SetRgb();
                    }
                    else
                    {
                        _geeleyAutoRgbLamp.SwitchRgd("0", "0", "0");
                    }
                }
            }
        }

        private void tBar_ValueChanged(object sender, EventArgs e)
        {
            btnCurrentColor.BackColor =
                Color.FromArgb(tBarRed.Value, tBarGreen.Value, tBarBlue.Value);
            //if (!_isLedOn)
            //    return;
            //SetRgb();
            //_geeleyAutoRgbLamp.SwitchRgd(
            //   tBarRed.Value.ToString(),
            //   tBarGreen.Value.ToString(),
            //   tBarBlue.Value.ToString());
        }

        private void GeeleyRgbLampControlForm_Closed(object sender, EventArgs e)
        {
            if (_autoThread != null)
            {
                _autoThread.Abort();
                _autoThread.Join();
            }

            if (ControllerMgrForm.Controller != null)
            {
                ControllerMgrForm.Controller.Dispose();
            }

            if (_geeleyAutoRgbLamp != null)
            {
                _geeleyAutoRgbLamp.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _geeleyAutoRgbLamp.BreathModeOff();
        }

        private void btnBreathOn_Click(object sender, EventArgs e)
        {
            _geeleyAutoRgbLamp.BreathModeOn();
        }

        private void btnColorSelection_Click(object sender, EventArgs e)
        {
            if (!_isAuto)
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    btnCurrentColor.BackColor = colorDialog1.Color;

                    tBarRed.Value = colorDialog1.Color.R;
                    tBarGreen.Value = colorDialog1.Color.G;
                    tBarBlue.Value = colorDialog1.Color.B;

                    //if (_isLedOn)
                    //    SetRgb();
                }
            }
            else
            {
                MessageBox.Show(@"正在自动刷新");
            }
        }

        private void SetRgb()
        {
            _geeleyAutoRgbLamp.SwitchRgd(
                btnCurrentColor.BackColor.R.ToString(), 
                btnCurrentColor.BackColor.G.ToString(), 
                btnCurrentColor.BackColor.B.ToString());
        }

        private void btnLedOn_Click(object sender, EventArgs e)
        {
            if (!_isLedOn)
            {
                _isLedOn = true;
            }
            else
            {
                MessageBox.Show(@"Led已打开");
            }
        }

        private void btnLedOff_Click(object sender, EventArgs e)
        {
            if (_isLedOn)
            {
                _isAuto = false;
                _isLedOn = false;
                _keepCount = 0;
                _currentIndex = 0;
            }
            else
            {
                MessageBox.Show(@"Led已关闭");
            }
        }

        private void btnAddToList_Click(object sender, EventArgs e)
        {
            if (!_isAuto)
            {
                var str = string.Format("{0},{1},{2}", btnCurrentColor.BackColor.R, btnCurrentColor.BackColor.G,
                    btnCurrentColor.BackColor.B);

                var isHave = listBox1.Items.Cast<object>().Any(t => t.ToString() == str);

                if (!isHave)
                    listBox1.Items.Add(str);
            }
            else
            {
                MessageBox.Show(@"正在自动刷新，请先关闭自动刷新");
            }
        }

        private void btnAutoOn_Click(object sender, EventArgs e)
        {
            if (!_isAuto)
            {
                if (_isLedOn)
                {
                    if (listBox1.Items.Count > 0)
                    {
                        _keepCount = 0;
                        _currentIndex = 0;
                        listBox1.SelectedIndex = 0;
                        _isAuto = true;
                    }
                    else
                    {
                        MessageBox.Show(@"请现在列表中添加颜色");
                    }
                }
                else
                {
                    MessageBox.Show(@"请先打开LED");
                }
            }
            else
            {
                MessageBox.Show(@"已经打开自动刷新");
            }
        }

        private void btnAutoOff_Click(object sender, EventArgs e)
        {
            if (_isAuto)
            {
                _isAuto = false;
                _keepCount = 0;
                _currentIndex = 0;
            }
            else
            {
                MessageBox.Show(@"自动刷新未打开，无需关闭");
            }
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            if (!_isAuto)
            {
                listBox1.Items.Clear();
            }
            else
            {
                MessageBox.Show(@"正在自动刷新，请先关闭自动刷新");
            }
        }
    }
}
