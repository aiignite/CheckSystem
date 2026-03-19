using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;
using HZH_Controls.IconFont;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.HelperForms.AnalogMax25608
{
    public sealed partial class AnalogMax25608LedControl : UIForm
    {
        private readonly UITableLayoutPanel _uiTableLayoutPanel = new UITableLayoutPanel { Dock = DockStyle.Fill };
        private readonly Controller.AnalogMax25608 _analogMax25608 = new Controller.AnalogMax25608("AnalogMax25608");
        public SyControllerWith56Pin ControllerWith56Pin { get; set; }

        public AnalogMax25608LedControl(SyControllerWith56Pin controllerWith56Pin)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.A_fa_plug, 32,
               Color.DodgerBlue);
            Text = @"ANALOG MAX25608";
            //_uiTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            if (controllerWith56Pin != null)
            {
                ControllerWith56Pin = controllerWith56Pin;
                _analogMax25608.MySerialPort = ControllerWith56Pin.GatewaySci2;

                Closed += AnalogMax25608LedControl_Closed;
                Load += AnalogMax25608LedControl_Load;
            }
        }

        private void AnalogMax25608LedControl_Closed(object sender, EventArgs e)
        {
            if (_analogMax25608 != null)
                _analogMax25608.Dispose();
            if (ControllerWith56Pin != null)
                ControllerWith56Pin.Dispose();
        }

        private async void AnalogMax25608LedControl_Load(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        for (var i = 0; i < 13; i++)
                            _uiTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 7));
                        for (var i = 0; i < 16; i++)
                            _uiTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 6));

                        for (var i = 0; i < 16; i++)
                        {
                            for (var j = 0; j < 13; j++)
                            {
                                var dev = @"Dev" + i;

                                if (j == 0)
                                {
                                    var devCheckBox = new UICheckBox
                                    {
                                        Text = dev,
                                        Name = dev,
                                        Dock = DockStyle.Fill
                                    };
                                    devCheckBox.CheckedChanged += devCheckBox_CheckedChanged;
                                    _uiTableLayoutPanel.Controls.Add(devCheckBox, j, i);
                                }
                                else
                                {
                                    var ledCheckBox = new UICheckBox
                                    {
                                        Text = @"CH" + j,
                                        Name = string.Format("{0}_CH{1}", dev, j),
                                        Dock = DockStyle.Fill,
                                        Enabled = false
                                    };

                                    ledCheckBox.CheckedChanged += ledCheckBox_CheckedChanged;
                                    _uiTableLayoutPanel.Controls.Add(ledCheckBox, j, i);
                                }
                            }
                        }

                        Controls.Add(_uiTableLayoutPanel);
                    }));
                }
            });
        }

        public async void ledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        var checkBox = sender as UICheckBox;
                        if (checkBox == null) return;
                        var name = checkBox.Name.Split('_');
                        var ch = name[1];
                        var dev = name[0];

                        var findDevCheckBox = _uiTableLayoutPanel.Controls.Find(dev, false);
                        if (findDevCheckBox.Length <= 0)
                            return;
                        var devCheckBox = findDevCheckBox[0] as UICheckBox;
                        if (devCheckBox == null) 
                            return;
                        if (!devCheckBox.Checked)
                            return;
                        if (checkBox.Checked)
                            _analogMax25608.TargetDevSingleChOn(dev.Replace("Dev", string.Empty),
                                    ch.Replace("CH", string.Empty));
                        else
                            _analogMax25608.TargetDevSingleChOff(dev.Replace("Dev", string.Empty),
                                   ch.Replace("CH", string.Empty));
                    }));
                }
            });
        }

        private async void devCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        var checkBox = sender as UICheckBox;
                        if (checkBox == null)
                            return;
                        var dev = checkBox.Name;

                        for (var i = 1; i < 13; i++)
                        {
                            var ledCheckBoxName = string.Format("{0}_CH{1}", dev, i);
                            var findLedCheckBox = _uiTableLayoutPanel.Controls.Find(ledCheckBoxName, false);

                            if (findLedCheckBox.Length <= 0)
                                continue;
                            var ledCheckBox = findLedCheckBox[0] as UICheckBox;
                            if (ledCheckBox == null)
                                continue;
                            ledCheckBox.Checked = false;
                            if (checkBox.Checked)
                            {
                                ledCheckBox.Enabled = true;
                                _analogMax25608.AddDev(dev.Replace("Dev", string.Empty));
                            }
                            else
                            {
                                ledCheckBox.Enabled = false;
                                _analogMax25608.Remove(dev.Replace("Dev", string.Empty));
                            }
                        }
                    }));
                }
            });
        }
    }
}
