using CheckSystem.VisionDetection.Vision;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UserControls;

namespace CheckSystem.VisionDetection
{
    public partial class FrmGroupSelect : UIForm
    {
        private readonly List<UcGroupSelect> _groupLabelComboxes = new List<UcGroupSelect>();
        private readonly Dictionary<string, List<VisionConfigPara>> _groupDictionary = new Dictionary<string, List<VisionConfigPara>>();
        private readonly List<LabelCombox> _barcodeLabelComboxes = new List<LabelCombox>();

        public FrmGroupSelect()
        {
            InitializeComponent();
            //WindowState = FormWindowState.Maximized;
            ////cmbLeftOrRight.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            rbLeft.Checked = true;
            if (VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight != null &&
                string.Equals(VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight, @"不区分", StringComparison.CurrentCultureIgnoreCase))
            {
                rbRight.Visible = false;
                rbLeft.Text = @"当前产品不区分左右";
            }

            for (var i = 0; i < 40; i++)
            {
                {
                    var c = uiGroupBox1.Controls.Find(string.Format("gcmb{0}", i + 1), false);
                    if (c.Any())
                    {
                        var cmb = c[0] as UcGroupSelect;
                        if (cmb != null)
                        {
                            cmb.Visible = false;
                            _groupLabelComboxes.Add(cmb);
                        }
                    }
                }

                {
                    var c = uiGroupBox2.Controls.Find(string.Format("bcmb{0}", i + 1), false);
                    if (c.Any())
                    {
                        var cmb = c[0] as LabelCombox;
                        if (cmb != null)
                        {
                            _barcodeLabelComboxes.Add(cmb);
                        }
                    }
                }
            }

            if (VisionCommon.VisionConfig.ParaInfo != null)
            {
                var groupsPara =
                      VisionCommon.VisionConfig.ParaInfo.ToList()
                          .FindAll(f => f.ParaGroups != null && f.ParaGroups.Length > 1);
                {
                    if (groupsPara.Any())
                    {
                        // 先提取出所有档位并归类
                        foreach (var gp in groupsPara)
                        {
                            var strTemp = string.Empty;
                            var gpTemp = gp.ParaGroups.OrderBy(f => f.ParaGroupName.ToUpper()).ToList();

                            strTemp = gpTemp.Aggregate(strTemp, (current, t) => current + (t.ParaGroupName + "$")).TrimEnd('$');

                            if (!_groupDictionary.ContainsKey(strTemp))
                                _groupDictionary.Add(strTemp, new List<VisionConfigPara>());

                            _groupDictionary[strTemp].Add(gp);
                        }

                        for (var k = 0; k < _groupDictionary.Count; k++)
                        {
                            var key = _groupDictionary.Keys.ToList()[k];
                            var gps = key.Split('$');

                            if (k < _groupLabelComboxes.Count)
                            {
                                _groupLabelComboxes[k].Tag = key;
                                _groupLabelComboxes[k].Visible = true;
                                _groupLabelComboxes[k].cmbName.DropDownStyle = ComboBoxStyle.DropDownList;
                                _groupLabelComboxes[k].comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

                                foreach (var t in _groupDictionary[key])
                                    _groupLabelComboxes[k].cmbName.Items.Add(t.ParaName);
                                _groupLabelComboxes[k].cmbName.SelectedIndex = 0;

                                foreach (var t in gps)
                                    _groupLabelComboxes[k].comboBox.Items.Add(t);
                                _groupLabelComboxes[k].comboBox.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }

            if (VisionCommon.VisionConfig.BarcodeInfo != null)
            {
                for (var u = 0; u < VisionCommon.VisionConfig.BarcodeInfo.Length; u++)
                {
                    var b = VisionCommon.VisionConfig.BarcodeInfo[u];

                    if (u < 16)
                    {
                        if (b.Groups.Length > 0)
                        {
                            _barcodeLabelComboxes[u].label.Text = b.Name;
                            _barcodeLabelComboxes[u].comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

                            foreach (var g in b.Groups)
                            {
                                var str = g.Select(gg => gg.Split("，")).Aggregate(string.Empty, (current, sp) => current + sp[1] + "+");

                                str = str.TrimEnd('+');

                                _barcodeLabelComboxes[u].comboBox.Items.Add(str);
                                _barcodeLabelComboxes[u].comboBox.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }

            //foreach (var t in _groupLabelComboxes.Where(t => string.Equals(t.label.Text, "label", StringComparison.CurrentCultureIgnoreCase)))
            //    t.Visible = false;

            foreach (var t in _groupLabelComboxes.Where(t => t.cmbName.Items.Count == 0 || t.comboBox.Items.Count == 0))
                t.Visible = false;
            foreach (var t in _barcodeLabelComboxes.Where(t => string.Equals(t.label.Text, "label", StringComparison.CurrentCultureIgnoreCase)))
                t.Visible = false;

            rbLeft.CheckedChanged += rbLeft_CheckedChanged;
            rbRight.CheckedChanged += rbRight_CheckedChanged;
            rbLeft_CheckedChanged(null, null);
        }

        private void rbRight_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRight.Checked)
            {
                var groupsPara =
                    VisionCommon.VisionConfig.ParaInfo.ToList()
                        .FindAll(f => f.ParaGroups != null && f.ParaGroups.Length > 1);
                {
                    if (groupsPara.Any())
                    {
                        for (var k = 0; k < groupsPara.Count; k++)
                        {
                            var t = groupsPara[k];

                            for (var i = 0; i < _groupLabelComboxes.Count; i++)
                            {
                                var control = _groupLabelComboxes[i];
                                var cmbName = control.cmbName;
                                var cmb = control.comboBox;
                                var rCount = 0;
                                var isContain = false;

                                foreach (var item in cmbName.Items)
                                {
                                    var pName = item.ToString();

                                    if (pName.ToUpper() == t.ParaName.ToUpper())
                                    {
                                        isContain = true;

                                        if (t.ParaLeftOrRight == "L&R" || t.ParaLeftOrRight == "仅R")
                                        {
                                            rCount++;
                                        }
                                    }
                                }

                                if (isContain)
                                    control.Visible = rCount != 0;

                            }
                        }
                    }
                }
            }
        }

        private void rbLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLeft.Checked)
            {
                var groupsPara =
                    VisionCommon.VisionConfig.ParaInfo.ToList()
                        .FindAll(f => f.ParaGroups != null && f.ParaGroups.Length > 1);
                {
                    if (groupsPara.Any())
                    {
                        for (var k = 0; k < groupsPara.Count; k++)
                        {
                            var t = groupsPara[k];

                            for (var i = 0; i < _groupLabelComboxes.Count; i++)
                            {
                                var control = _groupLabelComboxes[i];
                                var cmbName = control.cmbName;
                                var cmb = control.comboBox;
                                var lCount = 0;
                                var isContain = false;

                                foreach (var item in cmbName.Items)
                                {
                                    var pName = item.ToString();

                                    if (pName.ToUpper() == t.ParaName.ToUpper())
                                    {
                                        isContain = true;

                                        if (t.ParaLeftOrRight == "L&R" || t.ParaLeftOrRight == "仅L")
                                        {
                                            lCount++;
                                        }
                                    }
                                }

                                if (isContain)
                                    control.Visible = lCount != 0;
                            }
                        }
                    }
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            VisionCommon.SelectGroups.Clear();
            VisionCommon.SelectBarcodes.Clear();

            foreach (var t in _groupLabelComboxes)
            {
                var gpUiControl = t;
                var cmbName = t.cmbName;
                var cmb = t.comboBox;

                if (cmb.Visible && cmbName.Items.Count > 0 && cmb.Items.Count > 0)
                {
                    var gpAllName = gpUiControl.Tag.ToString();
                    var gpName = cmb.Items[cmb.SelectedIndex].ToString();
                    var gpValues = _groupDictionary[gpAllName];
                    foreach (var item in cmbName.Items)
                    {
                        var findPara = gpValues.Find(f => string.Equals(f.ParaName, item.ToString(), StringComparison.CurrentCultureIgnoreCase));
                        if (findPara == null)
                            continue;
                        {
                            var findParaGp = findPara.ParaGroups.ToList()
                                .Find(f => string.Equals(f.ParaGroupName, gpName, StringComparison.CurrentCultureIgnoreCase));
                            if (findParaGp != null)
                                VisionCommon.SelectGroups.Add(findPara.ParaName, findParaGp.ParaGroupName);
                        }
                    }
                }
            }

            for (var i = 0; i < _barcodeLabelComboxes.Count; i++)
            {
                var cmb = _barcodeLabelComboxes[i];
                if (cmb.Visible && cmb.label.Text != @"label")
                {
                    VisionCommon.SelectBarcodes.Add(cmb.label.Text, cmb.comboBox.SelectedIndex);
                }
            }

            if (rbLeft.Checked && !rbRight.Checked)
            {
                VisionCommon.IsLeft = true;
            }
            else if (!rbLeft.Checked && rbRight.Checked)
            {
                VisionCommon.IsLeft = false;
            }

            DialogResult = DialogResult.OK;
        }

        private void FrmGroupSelect_Load(object sender, EventArgs e)
        {

        }
    }
}
