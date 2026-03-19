using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CheckSystem.VisionDetection.Vision;
using Sunny.UI;

namespace CheckSystem.VisionDetection
{
    public partial class FrmDetectionParaConfig : UIForm
    {
        public VisionConfigPara Para { get; set; }
        private bool IsNew { get; set; }

        //public FrmDetectionParaConfig(
        //    string name, string type, string group, string leftOrRight, string unit, string relays, string methodBefore, string delay, string methodAfter, string binding)
        public FrmDetectionParaConfig(VisionConfigPara para)
        {
            InitializeComponent();
            Style = UIStyle.LayuiGreen;

            dgvGroupDetail.Style = UIStyle.Gray;
            dgvGroupDetail.ReadOnly = false;
            dgvGroupDetail.RowHeadersVisible = false;
            dgvGroupDetail.AllowUserToAddRows = false;
            dgvGroupDetail.AllowUserToResizeRows = false;
            dgvGroupDetail.AllowUserToResizeColumns = true;
            dgvGroupDetail.MultiSelect = true;
            dgvGroupDetail.RowHeadersVisible = false;
            dgvGroupDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGroupDetail.CellContentClick += dgvGroupDetail_CellContentClick;

            _cmbType.SelectedIndexChanged += _cmbType_SelectedIndexChanged;
            _cmbType.SelectedIndex = 0;
            _cmbLeftRightType.SelectedIndex = 0;

            var voltNode = new TreeNode("电压");
            for (var i = 0; i < 18; i++)
                voltNode.Nodes.Add(new TreeNode("电压" + (i + 1)));

            var currNode = new TreeNode("电流");
            for (var i = 0; i < 12; i++)
                currNode.Nodes.Add(new TreeNode("电流" + (i + 1)));
            cmbBinding.Nodes.Add(voltNode);
            cmbBinding.Nodes.Add(currNode);

            if (para == null)
            {
                Para = new VisionConfigPara
                {
                    ParaGroups = new VisionConfigParaParaGroup[1],
                    ParaDelayMs = "50",
                    ParaType = "电性能",
                    ParaLeftOrRight = @"L&R",
                    ParaMethods = new VisionConfigParaParaMethods
                    {
                        ParaMethodsAfter = string.Empty,
                        ParaMethodsBefore = string.Empty
                    },
                    ParaReleysList = new VisionConfigParaParaReleysList
                    {
                        ParaReleysOnList = string.Empty,
                        ParaReleysOffList = string.Empty
                    },
                    PowerPara = "电压1=13.5V，电流1=10A，电压2=0V，电流2=0A，电压3=5V，电流3=1A，串并联模式=无",
                };

                Para.ParaGroups[0] = new VisionConfigParaParaGroup
                {
                    ParaGroupK = "1",
                    ParaGroupB = "0"
                };
                IsNew = true;
            }
            else
            {
                Para = para;

                if (string.IsNullOrEmpty(Para.ParaType))
                    Para.ParaType = "电性能";
                if (string.IsNullOrEmpty(Para.ParaDelayMs))
                    Para.ParaType = "50";
                if (string.IsNullOrEmpty(Para.ParaLeftOrRight))
                    Para.ParaType = @"L&R";

                if (Para.ParaMethods == null)
                {
                    Para.ParaMethods = new VisionConfigParaParaMethods
                    {
                        ParaMethodsAfter = string.Empty,
                        ParaMethodsBefore = string.Empty
                    };
                }
                if (string.IsNullOrEmpty(Para.ParaMethods.ParaMethodsAfter))
                    Para.ParaMethods.ParaMethodsAfter = string.Empty;
                if (string.IsNullOrEmpty(Para.ParaMethods.ParaMethodsBefore))
                    Para.ParaMethods.ParaMethodsBefore = string.Empty;

                if (Para.ParaGroups == null)
                {
                    Para.ParaGroups = new VisionConfigParaParaGroup[1];
                    Para.ParaGroups[0] = new VisionConfigParaParaGroup { ParaGroupB = "0", ParaGroupK = "1" };
                }
                if (Para.ParaReleysList == null)
                {
                    Para.ParaReleysList = new VisionConfigParaParaReleysList
                    {
                        ParaReleysOnList = string.Empty,
                        ParaReleysOffList = string.Empty
                    };
                }

                if (string.IsNullOrEmpty(Para.ParaReleysList.ParaReleysOnList))
                    Para.ParaReleysList.ParaReleysOnList = string.Empty;
                if (string.IsNullOrEmpty(Para.ParaReleysList.ParaReleysOffList))
                    Para.ParaReleysList.ParaReleysOffList = string.Empty;
                IsNew = false;
            }

            for (var i = 0; i < 12 * 3; i++)
            {
                var rlName = "继电器" + (i + 1);

                var relayOnNode = new TreeNode(rlName);
                if (Para.ParaReleysList.ParaReleysOnList.Contains(rlName + ";"))
                    relayOnNode.Checked = true;
                _txtRelaysOn.Nodes.Add(relayOnNode);


                var relayOffNode = new TreeNode(rlName);
                if (Para.ParaReleysList.ParaReleysOffList.Contains(rlName))
                    relayOffNode.Checked = true;
                _txtRelaysOff.Nodes.Add(relayOffNode);
            }

            _txtName.Text = Para.ParaName;
            _txtUnit.Text = Para.ParaUnit;

            if (!string.IsNullOrEmpty(Para.ParaReleysList.ParaReleysOnList) && Para.ParaReleysList.ParaReleysOnList.Split(",").Any())
                foreach (var t in Para.ParaReleysList.ParaReleysOnList.Split(","))
                    _txtRelaysOn.Text += t + @",";

            if (!string.IsNullOrEmpty(Para.ParaReleysList.ParaReleysOffList) && Para.ParaReleysList.ParaReleysOffList.Split(",").Any())
                foreach (var t in Para.ParaReleysList.ParaReleysOffList.Split(","))
                    _txtRelaysOff.Text += t + @",";

            _txtRelaysOn.Text = _txtRelaysOn.Text.TrimEnd(',');
            _txtRelaysOff.Text = _txtRelaysOff.Text.TrimEnd(',');
            _txtDelay.Text = Para.ParaDelayMs;
            //_txtMethodAfter.Text = Para.ParaMethods.ParaMethodsAfter.Trim();
            //_txtMethodBefore.Text = Para.ParaMethods.ParaMethodsBefore.Trim();
            cmbBinding.Text = Para.ParaBinding;

            for (var i = 0; i < _cmbType.Items.Count; i++)
            {
                var t = _cmbType.Items[i];
                if (t.ToString() != Para.ParaType)
                    continue;
                _cmbType.SelectedIndex = i;
                break;
            }

            for (var i = 0; i < _cmbLeftRightType.Items.Count; i++)
            {
                var t = _cmbLeftRightType.Items[i];
                if (t.ToString() != Para.ParaLeftOrRight)
                    continue;
                _cmbLeftRightType.SelectedIndex = i;
                break;
            }

            if (Para.ParaType.Contains("信息读取") || Para.ParaType.Contains("电性能") || para.ParaType.Contains("电阻"))
            {
                for (var i = 0; i < Para.ParaGroups.Length; i++)
                {
                    dgvGroupDetail.Rows[i].Cells[0].Value = Para.ParaGroups[i].ParaGroupName;
                    dgvGroupDetail.Rows[i].Cells[1].Value = Para.ParaGroups[i].ParaGroupMin;
                    dgvGroupDetail.Rows[i].Cells[2].Value = Para.ParaGroups[i].ParaGroupMax;
                    dgvGroupDetail.Rows[i].Cells[3].Value = Para.ParaGroups[i].ParaGroupValue;
                    dgvGroupDetail.Rows[i].Cells[5].Value = Para.ParaGroups[i].ParaGroupK;
                    dgvGroupDetail.Rows[i].Cells[6].Value = Para.ParaGroups[i].ParaGroupB;
                }
            }

            _txtGroupCount.ValueChanged += txtGroupCount_ValueChanged;
            _txtGroupCount.Value = Para.ParaGroups.Length + 1;
            _txtGroupCount.Value = Para.ParaGroups.Length;
        }

        private void dgvGroupDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (
                !string.Equals(dgvGroupDetail.Columns[e.ColumnIndex].HeaderText, "计算",
                    StringComparison.CurrentCultureIgnoreCase))
                return;
            var value = dgvGroupDetail.Rows[e.RowIndex].Cells[3].Value;
            if (value == null)
            {
                this.ShowErrorTip("输入的数据不正确");
                return;
            }
            double valueDouble;
            if (!double.TryParse(value.ToString(), out valueDouble))
            {
                this.ShowErrorTip("输入的数据不正确");
                return;
            }
            var option = new UIEditOption();
            option.AddDouble("Per", "±%", 10);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
                return;
            var per = (double)frm["Per"];
            if (per > 0 && per < 100)
            {
                var minDouble = Math.Round(valueDouble * (100 - per) * 0.01, 2, MidpointRounding.AwayFromZero);
                var maxDouble = Math.Round(valueDouble * (100 + per) * 0.01, 2, MidpointRounding.AwayFromZero);
                dgvGroupDetail.Rows[e.RowIndex].Cells[1].Value = minDouble;
                dgvGroupDetail.Rows[e.RowIndex].Cells[2].Value = maxDouble;
            }
            else
            {
                this.ShowErrorTip("输入的数据不正确");
            }
        }

        private void txtGroupCount_ValueChanged(object sender, int value)
        {
            //return;

            for (var i = 0; i < dgvGroupDetail.RowCount; i++)
            {
                dgvGroupDetail.Rows[i].ReadOnly = true;
                dgvGroupDetail.Rows[i].DefaultCellStyle.BackColor = Color.DarkGray;
            }

            for (var i = 0; i < value; i++)
            {
                if (dgvGroupDetail.RowCount >= value)
                {
                    dgvGroupDetail.Rows[i].ReadOnly = false;
                    dgvGroupDetail.Rows[i].DefaultCellStyle.BackColor = Color.DarkGoldenrod;
                }
            }
        }

        private void _cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cmbType.Text.StartsWith("电性能") || _cmbType.Text.StartsWith("信息读取") || _cmbType.Text.StartsWith("电阻"))
            {
                _txtGroupCount.Enabled = true;
                dgvGroupDetail.ClearAll();

                var rowConut = _txtGroupCount.Maximum;// _txtGroupCount.Value;

                dgvGroupDetail.AddColumn("档位名称", "档位名称", readOnly: false);
                dgvGroupDetail.AddColumn("Min", "Min", readOnly: false);
                dgvGroupDetail.AddColumn("Max", "Max", readOnly: false);
                dgvGroupDetail.AddColumn("Value", "Value", readOnly: false);
                dgvGroupDetail.AddButtonColumn("计算", "计算", readOnly: false);
                dgvGroupDetail.AddColumn("*K或分压阻值", "K", readOnly: false);
                dgvGroupDetail.AddColumn("+B或分压电压", "B", readOnly: false);

                for (var i = 0; i < rowConut; i++)
                    dgvGroupDetail.AddRow(i == 0 ? "/" : "", "", "", "", "计算", "1", "0");

                for (var i = 0; i < dgvGroupDetail.RowCount; i++)
                {
                    dgvGroupDetail.Rows[i].ReadOnly = true;
                    dgvGroupDetail.Rows[i].DefaultCellStyle.BackColor = Color.DarkGray;
                }

                for (var i = 0; i < _txtGroupCount.Value; i++)
                {
                    if (dgvGroupDetail.RowCount >= _txtGroupCount.Value)
                    {
                        dgvGroupDetail.Rows[i].ReadOnly = false;
                        dgvGroupDetail.Rows[i].DefaultCellStyle.BackColor = Color.DarkGoldenrod;
                    }
                }

                if (Para != null && (Para != null || Para.ParaGroups != null))
                {
                    for (var i = 0; i < Para.ParaGroups.Length; i++)
                    {
                        dgvGroupDetail.Rows[i].Cells[0].Value = Para.ParaGroups[i].ParaGroupName;
                        dgvGroupDetail.Rows[i].Cells[1].Value = Para.ParaGroups[i].ParaGroupMin;
                        dgvGroupDetail.Rows[i].Cells[2].Value = Para.ParaGroups[i].ParaGroupMax;
                        dgvGroupDetail.Rows[i].Cells[3].Value = Para.ParaGroups[i].ParaGroupValue;
                        dgvGroupDetail.Rows[i].Cells[5].Value = Para.ParaGroups[i].ParaGroupK;
                        dgvGroupDetail.Rows[i].Cells[6].Value = Para.ParaGroups[i].ParaGroupB;
                    }
                }


                dgvGroupDetail.AutoResizeRows();
            }
            else
            {
                _txtGroupCount.Enabled = false;
                dgvGroupDetail.ClearAll();
            }
        }

        internal struct DoubleTypeStruct
        {
            public string Name { get; set; }
            public string Min { get; set; }
            public string Max { get; set; }
            public string K { get; set; }
            public string B { get; set; }

            //public override string ToString()
            //{
            //    DoubleTypeStruct
            //}
        }

        internal struct StringTypeStruct
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            dgvGroupDetail.EndEdit();

            var name = _txtName.Text;
            if (string.IsNullOrEmpty(name))
            {
                this.ShowErrorTip("名称不能为空");
                return;
            }

            if (IsNew)
            {
                if (VisionCommon.VisionConfig.ParaInfo.ToList().FindAll(f => f.ParaName == name).Count > 0)
                {
                    this.ShowErrorTip("名称重复");
                    return;
                }
            }
            else
            {
                foreach (var p in VisionCommon.VisionConfig.ParaInfo)
                {
                    if (p.ParaName != Para.ParaName)
                    {
                        if (p.ParaName == name)
                        {
                            this.ShowErrorTip("名称重复");
                            return;
                        }
                    }
                }
            }

            if (name.Contains(" ") ||
                name.Contains("：") ||
                name.Contains("，") ||
                name.Contains(":") ||
                name.Contains(",") ||
                name.Contains("。") ||
                name.Contains(".") ||
                name.Contains("继电器") ||
                name.Contains("检测") ||
                name.Contains("函数") ||
                name.Contains("延时") ||
                name.Contains("电源模式") ||
                name.Contains("硬件配置") ||
                name.Contains("新增配置"))
            {
                this.ShowErrorTip("名称中不能包含系统关键字！");
                return;
            }

            var type = _cmbType.Text;
            var groupCount = _txtGroupCount.Value;
            var leftOrRight = _cmbLeftRightType.Text;
            var relaysOn = _txtRelaysOn.Text;
            var relaysOff = _txtRelaysOff.Text;
            var binding = cmbBinding.Text;
            var delayMs = _txtDelay.IntValue;

            if (string.IsNullOrEmpty(binding))
                binding = "/";

            if (type.StartsWith("电性能") || type.StartsWith("电阻"))
            {
                if (string.IsNullOrEmpty(binding) || binding == "/")
                {
                    this.ShowErrorTip("绑定字段缺失");
                    return;
                }

                var groups = new VisionConfigParaParaGroup[groupCount];

                for (var i = 0; i < groupCount; i++)
                {
                    var row = dgvGroupDetail.Rows[i];

                    var groupNameValue = row.Cells[0].Value;
                    var groupMinValue = row.Cells[1].Value;
                    var groupMaxValue = row.Cells[2].Value;
                    var groupKValue = row.Cells[5].Value;
                    var groupBValue = row.Cells[6].Value;

                    if (groupNameValue == null ||
                        groupMinValue == null ||
                        groupMaxValue == null ||
                        groupKValue == null ||
                        groupBValue == null)
                    {
                        this.ShowErrorTip("档位信息缺失");
                        return;
                    }

                    if (string.IsNullOrEmpty(groupNameValue.ToString()) ||
                        string.IsNullOrEmpty(groupMinValue.ToString()) ||
                        string.IsNullOrEmpty(groupMaxValue.ToString()) ||
                        string.IsNullOrEmpty(groupKValue.ToString()) ||
                        string.IsNullOrEmpty(groupBValue.ToString()))
                    {
                        this.ShowErrorTip("档位信息缺失");
                        return;
                    }

                    double min, max, k, b;
                    if (!double.TryParse(groupMinValue.ToString(), out min) ||
                        !double.TryParse(groupMaxValue.ToString(), out max) ||
                        !double.TryParse(groupKValue.ToString(), out k) ||
                        !double.TryParse(groupBValue.ToString(), out b))
                    {
                        this.ShowErrorTip("档位信息格式填写错误");
                        return;
                    }

                    groups[i] = new VisionConfigParaParaGroup
                    {
                        ParaGroupName = groupNameValue.ToString(),
                        ParaGroupMin = groupMinValue.ToString(),
                        ParaGroupMax = groupMaxValue.ToString(),
                        ParaGroupK = groupKValue.ToString(),
                        ParaGroupB = groupBValue.ToString(),
                        ParaGroupValue = "/"
                    };
                }

                Para.ParaGroups = groups;
            }
            else if (type.Contains("信息读取"))
            {
                if (string.IsNullOrEmpty(binding) || binding == "/")
                {
                    this.ShowErrorTip("绑定字段缺失");
                    return;
                }

                var groups = new VisionConfigParaParaGroup[groupCount];

                for (var i = 0; i < groupCount; i++)
                {
                    var row = dgvGroupDetail.Rows[i];

                    var groupValue = row.Cells[3].Value;
                    if (groupValue == null)
                    {
                        this.ShowErrorTip("档位信息缺失");
                        return;
                    }
                    if (string.IsNullOrEmpty(groupValue.ToString()))
                    {
                        this.ShowErrorTip("档位信息缺失");
                        return;
                    }

                    var groupName = row.Cells[0].Value;
                    if (groupName == null)
                    {
                        this.ShowErrorTip("档位名称缺失");
                        return;
                    }
                    if (string.IsNullOrEmpty(groupName.ToString()) || groupName.ToString() == "/")
                    {
                        this.ShowErrorTip("档位名称缺失");
                        return;
                    }

                    groups[i] = new VisionConfigParaParaGroup
                    {
                        ParaGroupName = groupName.ToString(),
                        ParaGroupMin = "/",
                        ParaGroupMax = "/",
                        ParaGroupK = "/",
                        ParaGroupB = "/",
                        ParaGroupValue = groupValue.ToString()
                    };
                }

                Para.ParaGroups = groups;
            }

            Para.ParaName = name;
            Para.ParaType = type;
            Para.ParaLeftOrRight = leftOrRight;
            Para.ParaReleysList = new VisionConfigParaParaReleysList
            {
                ParaReleysOnList = relaysOn,
                ParaReleysOffList = relaysOff
            };
            Para.ParaDelayMs = delayMs.ToString();
            //Para.ParaMethods = new VisionConfigParaParaMethods
            //{
            //    ParaMethodsBefore = string.Empty,
            //    ParaMethodsAfter = string.Empty
            //};
            Para.ParaBinding = cmbBinding.Text;
            Para.ParaUnit = _txtUnit.Text;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnPowerPara_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Para.PowerPara))
                Para.PowerPara = "电压1=13.5V，电流1=10A，电压2=0V，电流2=0A，电压3=5V，电流3=1A，串并联模式=无";

            var option = new UIEditOption { AutoLabelWidth = true, Text = @"电源配置" };
            var sp = Para.PowerPara.Split("，");

            var powerMode = new[] { "无", "并联", "串联" };
            foreach (var t in sp)
            {
                var name = t.Split('=')[0];
                var value = t.Split('=')[1];

                if (name.StartsWith("电压"))
                    option.AddDouble("SetV" + name.Replace("电压", ""), name + "/V：",
                        double.Parse(value.TrimEnd('V')));
                else if (name.StartsWith("电流"))
                    option.AddDouble("SetC" + name.Replace("电流", ""), name + "/A：",
                        double.Parse(value.TrimEnd('A')));
                else if (name.StartsWith("串并联模式"))
                    option.AddCombobox("PowerMode", "串并联模式:", powerMode,
                        powerMode.ToList().FindIndex(f => f == value), true, true);
            }

            var frmPowerConfig = new UIEditForm(option);
            frmPowerConfig.Render();
            frmPowerConfig.ShowDialog();

            if (!frmPowerConfig.IsOK)
            {
                this.ShowInfoTip("用户取消了操作");
                return;
            }
            var v1 = frmPowerConfig["SetV1"].ToString();
            var v2 = frmPowerConfig["SetV2"].ToString();
            var v3 = frmPowerConfig["SetV3"].ToString();

            var c1 = frmPowerConfig["SetC1"].ToString();
            var c2 = frmPowerConfig["SetC2"].ToString();
            var c3 = frmPowerConfig["SetC3"].ToString();

            var powerConfig = powerMode[(int)frmPowerConfig["PowerMode"]];

            Para.PowerPara = string.Format("电压1={0}V，电流1={1}A，电压2={2}V，电流2={3}A，电压3={4}V，电流3={5}A，串并联模式={6}", v1,
                c1, v2, c2, v3, c3, powerConfig);
            this.ShowSuccessTip("电源数据已更新，请保存");
        }

        private void _txtMethodBefore_Click(object sender, EventArgs e)
        {
            var frm = new FrmMethodConfig(Para.ParaMethods.ParaMethodsBefore);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                Para.ParaMethods.ParaMethodsBefore = frm.MethodStr;
            }
        }

        private void _txtMethodAfter_Click(object sender, EventArgs e)
        {
            var frm = new FrmMethodConfig(Para.ParaMethods.ParaMethodsAfter);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                Para.ParaMethods.ParaMethodsAfter = frm.MethodStr;
            }
        }
    }
}
