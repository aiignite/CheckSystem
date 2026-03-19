using Controller;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.BD18333
{
    public partial class Bd18333CanDeviceMgrForm : UIForm
    {
        private Bd18333Euv _bd18333Euv;
        private ControllerBase _gw;
        private readonly List<string> _dev = new List<string>();

        public Bd18333CanDeviceMgrForm(IReadOnlyList<string> devs, Bd18333Euv bd18333Euv,
            SyRenesasMcuControllerMaster controller)
        {
            InitializeComponent();
            InitBd(devs, bd18333Euv, controller);
        }

        public Bd18333CanDeviceMgrForm(IReadOnlyList<string> devs, Bd18333Euv bd18333Euv,
            SyControllerWith56Pin controller)
        {
            InitializeComponent();
            InitBd(devs, bd18333Euv, controller);
        }

        private void InitBd(IReadOnlyList<string> devs, Bd18333Euv bd18333Euv, ControllerBase controller)
        {
            _bd18333Euv = bd18333Euv;
            _gw = controller;
            _dev.AddRange(devs);
            Load += Bd18333CanDeviceMgrForm_Load;
            FormClosing += Bd18333CanDeviceMgrForm_FormClosing;
        }

        private void Bd18333CanDeviceMgrForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!UIMessageBox.ShowAsk("是否要退出？"))
            {
                e.Cancel = true;
                UIMessageTip.ShowWarning("取消");
                return;
            }

            _bd18333Euv?.Dispose();
            _gw?.Dispose();
        }

        private void uiRadioButton1_Click(object sender, EventArgs e)
        {
            if (uiRadioButton1.Checked && !uiRadioButton2.Checked)
                _bd18333Euv.IsDcMode = false;
            else if (!uiRadioButton1.Checked && uiRadioButton2.Checked)
                _bd18333Euv.IsDcMode = true;

            foreach (var item in _dimsetTxt.Keys.ToList().SelectMany(key => _dimsetTxt[key]))
                item.Enabled = _bd18333Euv.IsDcMode;

            foreach (var item in _pwmDutyTxt.Keys.ToList().SelectMany(key => _pwmDutyTxt[key]))
                item.Enabled = !_bd18333Euv.IsDcMode;

            foreach (var item in _dcdimCmb.Keys.ToList().SelectMany(key => _dcdimCmb[key]))
                item.Enabled = !_bd18333Euv.IsDcMode;
        }

        private int _pageIndex = -1;
        private readonly List<TabPage> _pages = new List<TabPage>();
        private readonly Dictionary<string, List<UIMarkLabel>> _chLbl = new Dictionary<string, List<UIMarkLabel>>();
        private readonly Dictionary<string, List<NumericUpDown>> _dimsetTxt = new Dictionary<string, List<NumericUpDown>>();
        private readonly Dictionary<string, List<NumericUpDown>> _pwmDutyTxt = new Dictionary<string, List<NumericUpDown>>();
        private readonly Dictionary<string, List<UIComboBox>> _dcdimCmb = new Dictionary<string, List<UIComboBox>>();
        private readonly Dictionary<string, List<UISwitch>> _onoffSw = new Dictionary<string, List<UISwitch>>();

        private readonly Dictionary<string, List<Label>> _dcCurrUnit = new Dictionary<string, List<Label>>();
        private readonly Dictionary<string, List<Label>> _pwmPer = new Dictionary<string, List<Label>>();

        private void Bd18333CanDeviceMgrForm_Load(object sender, EventArgs e)
        {
            if (_dev.Any())
            {
                foreach (var d in _dev)
                {
                    var addrDec = d.Split(',')[0].Replace("dec", string.Empty);
                    var tpage = new TabPage { Name = d, Text = @"DEV" + addrDec, Tag = addrDec };
                    uiTabControl1.Controls.Add(tpage);
                    _pages.Add(tpage);

                    _chLbl.Add(addrDec, new List<UIMarkLabel>());
                    _dimsetTxt.Add(addrDec, new List<NumericUpDown>());
                    _dcCurrUnit.Add(addrDec, new List<Label>());
                    _pwmDutyTxt.Add(addrDec, new List<NumericUpDown>());
                    _pwmPer.Add(addrDec, new List<Label>());
                    _dcdimCmb.Add(addrDec, new List<UIComboBox>());
                    _onoffSw.Add(addrDec, new List<UISwitch>());

                    for (var i = 0; i < 24; i++)
                    {
                        var title = new UIMarkLabel
                        {
                            MarkPos = UIMarkLabel.UIMarkPos.Bottom,
                            Text = @"CH" + (i + 1),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Width = 75,
                            Font = new Font("微软雅黑", 8)
                        };

                        var mA = new Label
                        {
                            Text = @"/mA",
                            Width = 35,
                            TextAlign = ContentAlignment.MiddleLeft,
                            Font = new Font("微软雅黑", 8)
                        };

                        var per = new Label
                        {
                            Text = @"/%",
                            Width = 35,
                            TextAlign = ContentAlignment.MiddleLeft,
                            Font = new Font("微软雅黑", 8)
                        };

                        var dimset = new NumericUpDown { DecimalPlaces = 2, Font = new Font("微软雅黑", 9), Minimum = (decimal)0.23, Maximum = 60, Value = (decimal)0.23, Tag = (i + 1).ToString(), Width = 55, Enabled = false };
                        var pwmduty = new NumericUpDown { Minimum = 0, Font = new Font("微软雅黑", 9), Maximum = 100, Value = 100, Tag = (i + 1).ToString(), Width = 55, Enabled = true };
                        var dcdim = new UIComboBox
                        {
                            DropDownStyle = UIDropDownStyle.DropDownList,
                            Tag = (i + 1).ToString(),
                            Width = 90,
                            Enabled = true,
                            Font = new Font("微软雅黑", 9)
                        };
                        dcdim.Items.Add("3.75mA");
                        dcdim.Items.Add("7.50mA");
                        dcdim.Items.Add("11.25mA");
                        dcdim.Items.Add("15.00mA");
                        dcdim.Items.Add("18.75mA");
                        dcdim.Items.Add("22.50mA");
                        dcdim.Items.Add("26.25mA");
                        dcdim.Items.Add("30.00mA");
                        dcdim.Items.Add("33.75mA");
                        dcdim.Items.Add("37.50mA");
                        dcdim.Items.Add("41.25mA");
                        dcdim.Items.Add("45.00mA");
                        dcdim.Items.Add("48.75mA");
                        dcdim.Items.Add("52.50mA");
                        dcdim.Items.Add("56.25mA");
                        dcdim.Items.Add("60.00mA");
                        dcdim.SelectedIndex = dcdim.Items.Count - 1;

                        var sw = new UISwitch { Tag = (i + 1).ToString() };

                        _dcCurrUnit[addrDec].Add(mA);
                        _chLbl[addrDec].Add(title);
                        _dimsetTxt[addrDec].Add(dimset);
                        _pwmDutyTxt[addrDec].Add(pwmduty);
                        _pwmPer[addrDec].Add(per);
                        _dcdimCmb[addrDec].Add(dcdim);
                        _onoffSw[addrDec].Add(sw);

                        dimset.ValueChanged += Dimset_Click;
                        pwmduty.ValueChanged += Pwmduty_ValueChanged;
                        dcdim.SelectedIndexChanged += Dcdim_SelectedIndexChanged;
                        sw.ActiveChanged += Sw_ActiveChanged;

                        tpage.Controls.Add(title);
                        tpage.Controls.Add(dimset);
                        tpage.Controls.Add(mA);
                        tpage.Controls.Add(pwmduty);
                        tpage.Controls.Add(per);
                        tpage.Controls.Add(dcdim);
                        tpage.Controls.Add(sw);
                    }
                }

                SizeChanged += Bd18333CanDeviceMgrForm_SizeChanged;
                uiTabControl1.SelectedIndexChanged += UiTabControl1_SelectedIndexChanged;
                _pageIndex = uiTabControl1.TabIndex = 0;
                ResizeUi(_pageIndex);
            }

            allDimSet.DecimalPlaces = 2;
            allDimSet.Font = new Font("微软雅黑", 9);
            allDimSet.Minimum = (decimal)0.23;
            allDimSet.Maximum = 60;
            allDimSet.Value = (decimal)0.23;
            allDimSet.ValueChanged += AllDimSet_ValueChanged;

            allPwm.DecimalPlaces = 0;
            allPwm.Font = new Font("微软雅黑", 9);
            allPwm.Minimum = 0;
            allPwm.Maximum = 100;
            allPwm.Value = 100;
            allPwm.ValueChanged += AllPwmSet_ValueChanged;

            cmbPwmCurr.DropDownStyle = UIDropDownStyle.DropDownList;
            cmbPwmCurr.Items.Add("3.75mA");
            cmbPwmCurr.Items.Add("7.50mA");
            cmbPwmCurr.Items.Add("11.25mA");
            cmbPwmCurr.Items.Add("15.00mA");
            cmbPwmCurr.Items.Add("18.75mA");
            cmbPwmCurr.Items.Add("22.50mA");
            cmbPwmCurr.Items.Add("26.25mA");
            cmbPwmCurr.Items.Add("30.00mA");
            cmbPwmCurr.Items.Add("33.75mA");
            cmbPwmCurr.Items.Add("37.50mA");
            cmbPwmCurr.Items.Add("41.25mA");
            cmbPwmCurr.Items.Add("45.00mA");
            cmbPwmCurr.Items.Add("48.75mA");
            cmbPwmCurr.Items.Add("52.50mA");
            cmbPwmCurr.Items.Add("56.25mA");
            cmbPwmCurr.Items.Add("60.00mA");
            cmbPwmCurr.SelectedIndex = cmbPwmCurr.Items.Count - 1;
            cmbPwmCurr.SelectedIndexChanged += CmbPwmCurr_SelectedIndexChanged;

            allSw.ActiveChanged += AllSw_ActiveChanged;
        }

        private void AllSw_ActiveChanged(object sender, EventArgs e)
        {
            foreach (var item in _onoffSw.Keys.ToList().SelectMany(key => _onoffSw[key]))
            {
                item.Active = ((UISwitch)sender).Active;

                if (item.Active)
                    _bd18333Euv?.ChannelOn(GetDevAddr(item), GetChAddr(item));
                else
                    _bd18333Euv?.ChannelOff(GetDevAddr(item), GetChAddr(item));
            }
        }

        private void CmbPwmCurr_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var item in _dcdimCmb.Keys.ToList().SelectMany(key => _dcdimCmb[key]))
            {
                item.SelectedIndex = ((UIComboBox)sender).SelectedIndex;
                _bd18333Euv?.SetPwmCurr(GetDevAddr(item), GetChAddr(item), item.SelectedIndex.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void AllPwmSet_ValueChanged(object sender, EventArgs e)
        {
            foreach (var item in _pwmDutyTxt.Keys.ToList().SelectMany(key => _pwmDutyTxt[key]))
            {
                item.Value = ((NumericUpDown)sender).Value;
                _bd18333Euv?.SetPwmDuty(GetDevAddr(item), GetChAddr(item), item.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void AllDimSet_ValueChanged(object sender, EventArgs e)
        {
            foreach (var item in _dimsetTxt.Keys.ToList().SelectMany(key => _dimsetTxt[key]))
            {
                item.Value = ((NumericUpDown)sender).Value;
                var mA = item.Value;
                var dimset = (int)Math.Round(((mA / (decimal)60f) * 256) - 1, 0, MidpointRounding.AwayFromZero);
                _bd18333Euv?.DimSet(GetDevAddr(item), GetChAddr(item), dimset.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void Dcdim_SelectedIndexChanged(object sender, EventArgs e)
            => _bd18333Euv?.SetPwmCurr(GetDevAddr((UIComboBox)sender), GetChAddr((UIComboBox)sender),
                ((UIComboBox)sender).SelectedIndex.ToString(CultureInfo.InvariantCulture));

        private void UiTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _pageIndex = uiTabControl1.SelectedIndex;
            ResizeUi(_pageIndex);
        }

        private void Sw_ActiveChanged(object sender, EventArgs e)
        {
            if (((UISwitch)sender).Active)
                _bd18333Euv?.ChannelOn(GetDevAddr((UISwitch)sender), GetChAddr((UISwitch)sender));
            else
                _bd18333Euv?.ChannelOff(GetDevAddr((UISwitch)sender), GetChAddr((UISwitch)sender));
        }

        private void Pwmduty_ValueChanged(object sender, EventArgs e)
            => _bd18333Euv?.SetPwmDuty(GetDevAddr((NumericUpDown)sender), GetChAddr((NumericUpDown)sender), ((NumericUpDown)sender).Value.ToString(CultureInfo.InvariantCulture));

        private void Dimset_Click(object sender, EventArgs e)
        {
            var mA = ((NumericUpDown)sender).Value;
            var dimset = (int)Math.Round(((mA / (decimal)60f) * 256) - 1, 0, MidpointRounding.AwayFromZero);
            _bd18333Euv?.DimSet(GetDevAddr((NumericUpDown)sender), GetChAddr((NumericUpDown)sender), dimset.ToString(CultureInfo.InvariantCulture));
        }

        private static string GetDevAddr(Control ctrl) => ctrl.Parent.Tag.ToString();

        private static string GetChAddr(Control ctrl) => ctrl.Tag.ToString();

        private void Bd18333CanDeviceMgrForm_SizeChanged(object sender, EventArgs e) => ResizeUi(_pageIndex);

        private void ResizeUi(int index)
        {
            if (index >= 0 && _pages.Any() && index <= _pages.Count - 1)
            {
                var page = _pages[index];
                var dev = page.Tag.ToString();

                var width = page.ClientSize.Width;

                const int intervalX = 5;
                const int intervalY = 15;
                var baseX = 10;
                var baseY = 10;

                var titleWidth = (width - (50 * 2) - (55 * 2 * 2) - (80 * 2) - (70 * 2) - (35 * 2 * 2) - intervalX * 10) / 2;

                for (var i = 0; i < 12; i++)
                {
                    _chLbl[dev][i].Width = titleWidth;
                    _chLbl[dev][i].Location = new Point(baseX, baseY);
                    _dimsetTxt[dev][i].Location = new Point(_chLbl[dev][i].Location.X + _chLbl[dev][i].Width + intervalX, baseY);
                    _dcCurrUnit[dev][i].Location = new Point(_dimsetTxt[dev][i].Location.X + _dimsetTxt[dev][i].Width, baseY);
                    _pwmDutyTxt[dev][i].Location = new Point(_dcCurrUnit[dev][i].Location.X + _dcCurrUnit[dev][i].Width + intervalX, baseY);
                    _pwmPer[dev][i].Location = new Point(_pwmDutyTxt[dev][i].Location.X + _pwmDutyTxt[dev][i].Width, baseY);
                    _dcdimCmb[dev][i].Location = new Point(_pwmPer[dev][i].Location.X + _pwmPer[dev][i].Width + intervalX, baseY);
                    _onoffSw[dev][i].Location = new Point(_dcdimCmb[dev][i].Location.X + _dcdimCmb[dev][i].Width + intervalX, baseY);

                    baseY += _chLbl[dev][i].Height + intervalY;
                }

                baseX = _onoffSw[dev][11].Location.X + _onoffSw[dev][11].Width + intervalX * 5;
                baseY = 10;

                for (var i = 12; i < 24; i++)
                {
                    _chLbl[dev][i].Width = titleWidth;
                    _chLbl[dev][i].Location = new Point(baseX, baseY);
                    _dimsetTxt[dev][i].Location = new Point(_chLbl[dev][i].Location.X + _chLbl[dev][i].Width + intervalX, baseY);
                    _dcCurrUnit[dev][i].Location = new Point(_dimsetTxt[dev][i].Location.X + _dimsetTxt[dev][i].Width, baseY);
                    _pwmDutyTxt[dev][i].Location = new Point(_dcCurrUnit[dev][i].Location.X + _dcCurrUnit[dev][i].Width + intervalX, baseY);
                    _pwmPer[dev][i].Location = new Point(_pwmDutyTxt[dev][i].Location.X + _pwmDutyTxt[dev][i].Width, baseY);
                    _dcdimCmb[dev][i].Location = new Point(_pwmPer[dev][i].Location.X + _pwmPer[dev][i].Width + intervalX, baseY);
                    _onoffSw[dev][i].Location = new Point(_dcdimCmb[dev][i].Location.X + _dcdimCmb[dev][i].Width + intervalX, baseY);

                    baseY += _chLbl[dev][i].Height + intervalY;
                }
            }
        }
    }
}
