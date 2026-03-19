using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StateMachine;
using UserControls;

namespace DeviceDesign
{
    public partial class FormStatusUnit : Form
    {

        private readonly DeviceConfigStatusUnit _statusUnit;
        private readonly string _workstationName;
        private readonly string _statusUnitName;
        public Point PointStatusUnitInit;

        private readonly UserDataGrid _dgvFunctionEnter = new UserDataGrid();
        private readonly Label _lbFunctionEnter = new Label();
        private readonly UserDataGrid _dgvFunctionDuring = new UserDataGrid();
        private readonly Label _lbFunctionDuring = new Label();

        private readonly LabelText _ltStatusUnitName = new LabelText();

        private readonly LabelCombox _lcbFunctionSelect = new LabelCombox();  //enter or during

        private readonly LabelCombox _lcbControllerFieldL = new LabelCombox();
        private readonly LabelCombox _lcbControllerField2 = new LabelCombox();
        private readonly LabelText _ltValue = new LabelText();

        private readonly LabelCombox _lcbParts = new LabelCombox();
        private readonly LabelCombox _lcbEqualSymbol = new LabelCombox();

        private readonly Button _btnAdd = new Button();

        #region

        private List<string> _listFileds = new List<string>();
        private List<string> _listMethods = new List<string>();

        #endregion

        public FormStatusUnit()
        {
            InitializeComponent();
        }

        public FormStatusUnit(string workstationname, string statusunitname)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Height = SystemInformation.WorkingArea.Height;
            Width = SystemInformation.WorkingArea.Width;
            _workstationName = workstationname;
            var ws = ClassComm.DeviceConfig.WorkStations.ToList().Find(t => t.Name == workstationname);
            if (ws == null)
                return;

            var statusunit =
                ClassComm.DeviceConfig.StatusUnits.ToList()
                    .Find(t => t.Name == statusunitname && t.WorkStationName == workstationname);
            if (statusunit != null)
            {
                _statusUnit = statusunit;
                _statusUnitName = _statusUnit.Name;
                _ltStatusUnitName.Enabled = false;
            }
            else
            {
                _statusUnit = new DeviceConfigStatusUnit();
                _statusUnitName = string.Empty;
            }
        }

        private void FormStatusUnit_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Text = _workstationName;
            //WindowState = FormWindowState.Maximized;
            ControlsInit();
        }

        private async void ControlsInit()
        {
            _dgvFunctionEnter.Dock = DockStyle.Top;
            _dgvFunctionEnter.label.Text = @"进入函数";
            _dgvFunctionEnter.Height = 200;
            _dgvFunctionEnter.dataGridView.AllowUserToAddRows = false;
            _dgvFunctionEnter.dataGridView.Columns.Clear();
            _dgvFunctionEnter.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "进入函数" });
            //_dgvFunctionEnter.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "进入函数说明" });
            _dgvFunctionEnter.dataGridView.Columns[0].DefaultCellStyle.BackColor = Color.AntiqueWhite;
            _dgvFunctionEnter.dataGridView.GridColor = Color.White;
            for (var i = 0; i < _dgvFunctionEnter.dataGridView.Columns.Count; i++)
                _dgvFunctionEnter.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            _dgvFunctionDuring.Dock = DockStyle.Top;
            _dgvFunctionDuring.label.Text = @"执行期间函数";
            _dgvFunctionDuring.Height = 200;
            _dgvFunctionDuring.dataGridView.AllowUserToAddRows = false;
            _dgvFunctionDuring.dataGridView.Columns.Clear();
            _dgvFunctionDuring.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "执行期间函数" });
            //_dgvFunctionDuring.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "执行期间函数说明" });
            _dgvFunctionDuring.dataGridView.Columns[0].DefaultCellStyle.BackColor = Color.AntiqueWhite;
            for (var i = 0; i < _dgvFunctionDuring.dataGridView.Columns.Count; i++)
                _dgvFunctionDuring.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            _ltStatusUnitName.label.Text = @"状态单元名称";
            _ltStatusUnitName.Dock = DockStyle.Top;

            _lcbFunctionSelect.label.Text = @"函数选择";
            _lcbFunctionSelect.Dock = DockStyle.Top;
            _lcbFunctionSelect.comboBox.Items.Add("进入函数");
            _lcbFunctionSelect.comboBox.Items.Add("执行期间函数");
            _lcbFunctionSelect.comboBox.SelectedIndex = 0;

            _lcbControllerFieldL.label.Text = @"控制器变量L";
            _lcbControllerFieldL.Dock = DockStyle.Top;
            //数据

            _lcbControllerField2.label.Text = @"控制器变量R";
            _lcbControllerField2.Dock = DockStyle.Top;

            foreach (var controller in ClassComm.DeviceConfig.Controllers)
            {
                var typeName = ClassComm.DllAsmb.GetType("Controller." + controller.Type);

                if (typeName != null)
                {
                    var methods =
                        typeName.GetMethods(BindingFlags.Public | BindingFlags.Instance |
                                            BindingFlags.DeclaredOnly);
                    var fields =
                        typeName.GetFields(BindingFlags.Public | BindingFlags.Instance |
                                           BindingFlags.DeclaredOnly);

                    foreach (var field in fields)
                    {
                        //_lcbControllerFieldL.comboBox.Items.Add(controller.Name + ".Field." +
                        //                                        field.ToString()
                        //                                            .Substring(
                        //                                                field.ToString()
                        //                                                    .IndexOf(" ",
                        //                                                        StringComparison.Ordinal) +
                        //                                                1));
                        _lcbControllerFieldL.ListText.Add(controller.Name + ".Field." +
                                                          field.ToString()
                                                              .Substring(
                                                                  field.ToString()
                                                                      .IndexOf(" ", StringComparison.Ordinal) +
                                                                  1));
                        //_lcbControllerField2.comboBox.Items.Add(controller.Name + ".Field." +
                        //                                        field.ToString()
                        //                                            .Substring(
                        //                                                field.ToString()
                        //                                                    .IndexOf(" ",
                        //                                                        StringComparison.Ordinal) +
                        //                                                1));
                        _lcbControllerField2.ListText.Add(controller.Name + ".Field." +
                                                          field.ToString()
                                                              .Substring(
                                                                  field.ToString()
                                                                      .IndexOf(" ", StringComparison.Ordinal) +
                                                                  1));
                    }

                    foreach (var method in methods)
                    {
                        var para = method.GetParameters().ToList();
                        var strPara = string.Join(",", para);

                        //_lcbControllerFieldL.comboBox.Items.Add(controller.Name + ".Method." + method.Name + "(" +
                        //                                        strPara + ")");
                        _lcbControllerFieldL.ListText.Add(controller.Name + ".Method." + method.Name + "(" +
                                                          strPara +
                                                          ")");
                        //_lcbControllerField2.comboBox.Items.Add(controller.Name + ".Method." + method.Name + "(" +
                        //                                        strPara + ")");
                        _lcbControllerField2.ListText.Add(controller.Name + ".Method." + method.Name + "(" +
                                                          strPara +
                                                          ")");
                    }
                }
                else
                    MessageBox.Show(@"Controller.dll中未发现" + controller.Type);
            }

            _ltValue.label.Text = @"值";
            _ltValue.Dock = DockStyle.Top;

            _lcbParts.label.Text = @"传感器执行器";
            _lcbParts.Dock = DockStyle.Top;

            foreach (var item in ClassComm.DeviceConfig.Parts)
            {
                _lcbParts.comboBox.Items.Add(item.ProcessNo + ".Part." + item.Name);
                _lcbParts.ListText.Add(item.ProcessNo + ".Part." + item.Name);
            }

            _lcbEqualSymbol.label.Text = @"函数符号";
            _lcbEqualSymbol.Dock = DockStyle.Top;
            _lcbEqualSymbol.comboBox.Items.Add("=");
            _lcbEqualSymbol.comboBox.Items.Add("+=");
            _lcbEqualSymbol.comboBox.Items.Add("-=");
            _lcbEqualSymbol.comboBox.Items.Add("*=");
            _lcbEqualSymbol.comboBox.Items.Add("/=");
            _lcbEqualSymbol.comboBox.Items.Add("=!");
            _lcbEqualSymbol.comboBox.SelectedIndex = 0;

            _btnAdd.Height = 80;
            _btnAdd.Text = @"添加到函数";
            _btnAdd.Dock = DockStyle.Top;

            splitContainer1.Panel1.Controls.Add(_btnAdd);
            splitContainer1.Panel1.Controls.Add(_ltValue);
            splitContainer1.Panel1.Controls.Add(_lcbControllerField2);
            splitContainer1.Panel1.Controls.Add(_lcbEqualSymbol);
            splitContainer1.Panel1.Controls.Add(_lcbControllerFieldL);
            splitContainer1.Panel1.Controls.Add(_lcbParts);
            splitContainer1.Panel1.Controls.Add(_lcbFunctionSelect);
            splitContainer1.Panel1.Controls.Add(_ltStatusUnitName);

            splitContainer1.Panel2.Controls.Add(_dgvFunctionDuring);
            splitContainer1.Panel2.Controls.Add(_dgvFunctionEnter);

            _lcbFunctionSelect.comboBox.SelectedIndexChanged += FunctionSelectComboBox_SelectedIndexChanged;

            _lcbParts.comboBox.SelectedIndexChanged += SensorOrActuatorComboBox_SelectedIndexChanged;
            _lcbControllerFieldL.comboBox.SelectedIndexChanged += ControllerFieldComboBox_SelectedIndexChanged;

            _lcbControllerField2.comboBox.SelectedIndexChanged += ControllerField2ComboBox_SelectedIndexChanged;
            _ltValue.textBox.TextChanged += ValueTextBox_TextChanged;

            _btnAdd.Click += _btnAdd_Click;
            UpdateControlsVisible();

            if (_statusUnit.Name == null)
                return;
            _ltStatusUnitName.Text = _statusUnit.Name;

            var codeEnter = _statusUnit.EnterFunction ?? string.Empty;
            codeEnter = codeEnter.Replace("\n", string.Empty);
            codeEnter = codeEnter.Replace(" ", string.Empty);
            var lstCodeEnter = codeEnter.Split(';');
            var codeEnterNote = _statusUnit.EnterNote ?? string.Empty;
            codeEnterNote = codeEnterNote.Replace("\n", string.Empty);
            codeEnterNote = codeEnterNote.Replace(" ", string.Empty);
            var lstCodeEnterNote = codeEnterNote.Split(';');
            for (var i = 0; i < lstCodeEnter.Length; i++)
            {
                if (lstCodeEnter[i] == string.Empty) continue;
                var numrow = _dgvFunctionEnter.dataGridView.Rows.Add();
                _dgvFunctionEnter.dataGridView.Rows[numrow].Cells[0].Value = lstCodeEnter[i] + ";";
                //var temp = i < lstCodeEnterNote.Length ? lstCodeEnterNote[i] : string.Empty;
                //_dgvFunctionEnter.dataGridView.Rows[numrow].Cells[1].Value = temp + ";";
            }

            var codeDuring = _statusUnit.DuringFunction ?? string.Empty;
            codeDuring = codeDuring.Replace("\n", string.Empty);
            codeDuring = codeDuring.Replace(" ", string.Empty);
            var lstCodeDuring = codeDuring.Split(';');
            var codeDuringNote = _statusUnit.DuringNote ?? string.Empty;
            codeDuringNote = codeDuringNote.Replace("\n", string.Empty);
            codeDuringNote = codeDuringNote.Replace(" ", string.Empty);
            var lstCodeDuringNote = codeDuringNote.Split(';');

            for (var i = 0; i < lstCodeDuring.Length; i++)
            {
                if (lstCodeDuring[i] == string.Empty) continue;
                var numrow = _dgvFunctionDuring.dataGridView.Rows.Add();
                _dgvFunctionDuring.dataGridView.Rows[numrow].Cells[0].Value = lstCodeDuring[i] + ";";
                //var temp = i < lstCodeDuringNote.Length ? lstCodeDuringNote[i] : string.Empty;
                //_dgvFunctionDuring.dataGridView.Rows[numrow].Cells[1].Value = temp + ";";
            }
        }

        private void UpdateControlsVisible()
        {
            _lcbParts.Visible = true;
            _lcbControllerFieldL.Visible = true;
            _lcbEqualSymbol.Visible = true;
            _lcbControllerField2.Visible = true;
            _ltValue.Visible = true;
        }

        private void _btnAdd_Click(object sender, EventArgs e)
        {
            string code;
            string codeNote;

            if (_lcbParts.Text == string.Empty && _lcbControllerFieldL.Text != string.Empty)
            {
                var s = _lcbControllerFieldL.Text;
                if (s.Contains(".Method."))
                {
                    code = s.Substring(0, s.IndexOf("(", StringComparison.Ordinal));
                    codeNote = code;
                    if (_lcbControllerField2.Text != "" && _ltValue.Text == "")
                    {
                        code += "(" + _lcbControllerField2.Text + ")";
                        codeNote = code;
                    }
                    else if (_lcbControllerField2.Text == "" && _ltValue.Text != "")
                    {
                        code += "(" + _ltValue.Text + ")";
                        codeNote = code;
                    }
                    else
                    {
                        code += "()";
                        codeNote = code;
                    }
                }
                else
                {
                    code = s;
                    codeNote = code;
                    if (_lcbEqualSymbol.Text != "")
                    {
                        code += _lcbEqualSymbol.Text;
                        codeNote += _lcbEqualSymbol.Text;
                    }
                    else
                    {
                        MessageBox.Show(@"符号不能为空");
                        return;
                    }
                    if (_lcbControllerField2.Text != "" && _ltValue.Text == "")
                    {
                        code += _lcbControllerField2.Text;
                        codeNote += _lcbControllerField2.Text;
                    }
                    else if (_lcbControllerField2.Text == "" && _ltValue.Text != "")
                    {
                        code += _ltValue.Text;
                        codeNote += _ltValue.Text;
                    }
                    else
                    {
                        MessageBox.Show(@"参数或者值请选择一个");
                        return;
                    }

                }
                codeNote = code;

            }
            else if (_lcbParts.Text != "")
            {
                code = _lcbControllerFieldL.Text;
                codeNote = _lcbParts.Text;

                if (_lcbEqualSymbol.Text != "")
                {
                    code += _lcbEqualSymbol.Text;
                    codeNote += _lcbEqualSymbol.Text;
                }
                else
                {
                    MessageBox.Show(@"符号不能为空");
                    return;
                }
                if (_lcbControllerField2.Text != "" && _ltValue.Text == "")
                {
                    code += _lcbControllerField2.Text;
                    codeNote += _lcbControllerField2.Text;
                }
                else if (_lcbControllerField2.Text == "" && _ltValue.Text != "")
                {
                    code += _ltValue.Text;
                    codeNote += _ltValue.Text;
                }
                else
                {
                    MessageBox.Show(@"参数或者值请选择一个");
                    return;
                }
            }
            else
            {
                MessageBox.Show(@"部件或控制器字段有误");
                return;
            }

            if (_lcbFunctionSelect.comboBox.SelectedIndex == 0) //enter
            {
                int rowNum;
                if (_dgvFunctionEnter.dataGridView.SelectedRows.Count > 0)
                {
                    rowNum = _dgvFunctionEnter.dataGridView.SelectedRows[0].Index;
                    _dgvFunctionEnter.dataGridView.Rows.Insert(rowNum);
                }
                else
                {
                    rowNum = _dgvFunctionEnter.dataGridView.Rows.Add();
                }
                //
                //_dgvFunctionEnter.dataGridView.Rows[rowNum].Cells[0].Value = code + ";";
                _dgvFunctionEnter.dataGridView.Rows[rowNum].Cells[0].Value = codeNote + ";";
                //_dgvFunctionEnter.dataGridView.Rows[rowNum].Cells[1].Value = codeNote + ";";
            }
            else if (_lcbFunctionSelect.comboBox.SelectedIndex == 1) //during
            {
                int rowNum;
                if (_dgvFunctionDuring.dataGridView.SelectedRows.Count > 0)
                {
                    rowNum = _dgvFunctionDuring.dataGridView.SelectedRows[0].Index;
                    _dgvFunctionDuring.dataGridView.Rows.Insert(rowNum);
                }
                else
                {
                    rowNum = _dgvFunctionDuring.dataGridView.Rows.Add();
                }
                //
                //_dgvFunctionDuring.dataGridView.Rows[rowNum].Cells[0].Value = code + ";";
                _dgvFunctionDuring.dataGridView.Rows[rowNum].Cells[0].Value = codeNote + ";";
                //_dgvFunctionDuring.dataGridView.Rows[rowNum].Cells[1].Value = codeNote + ";";
            }
        }

        private void ValueTextBox_TextChanged(object sender, EventArgs e)
        {
            _lcbControllerField2.Text = string.Empty;
        }

        private void ControllerField2ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ltValue.Text = string.Empty;
        }

        private void ControllerFieldComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var part = ClassComm.DeviceConfig.Parts.ToList().Find(t => t.ControllerField == _lcbControllerFieldL.Text);
            if (part != null)
            {
                _lcbParts.Text = part.ProcessNo + @".Part." + part.Name;
            }
            else
            {
                _lcbParts.Text = string.Empty;
            }
        }

        private void SensorOrActuatorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var part = ClassComm.DeviceConfig.Parts.ToList().Find(t => t.ProcessNo + @".Part." + t.Name == _lcbParts.Text);
            if (part != null)
            {
                _lcbControllerFieldL.Text = part.ControllerField;
            }
            else
            {
                _lcbControllerFieldL.Text = string.Empty;
            }
        }

        private void FunctionSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControlsVisible();
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            var newStatusUnitName = _ltStatusUnitName.Text?.Trim();
            if (string.IsNullOrEmpty(newStatusUnitName))
            {
                MessageBox.Show(@"状态单元名称不能为空，请填写名称");
                return;
            }

            // 重名检测 - 检查是否存在同名状态单元
            var duplicateStatusUnit = ClassComm.DeviceConfig.StatusUnits
                .ToList()
                .FirstOrDefault(t => t.WorkStationName == _workstationName
                                     && t.Name == newStatusUnitName
                                     && !object.ReferenceEquals(t, _statusUnit));

            if (duplicateStatusUnit != null)
            {
                MessageBox.Show(@"状态单元名称存在重名，不能保存");
                return;
            }

            var entercode = string.Empty;
            var entercodenote = string.Empty;
            for (var i = 0; i < _dgvFunctionEnter.dataGridView.RowCount; i++)
            {
                var temp = _dgvFunctionEnter.dataGridView.Rows[i].Cells[0].Value.ToString();
                if (temp.Substring(temp.Length - 1, 1) != ";")
                {
                    temp += ";";
                }
                entercode += temp + "\n";
            }

            var duringcode = string.Empty;
            var duringcodenote = string.Empty;
            for (var i = 0; i < _dgvFunctionDuring.dataGridView.RowCount; i++)
            {
                var temp = _dgvFunctionDuring.dataGridView.Rows[i].Cells[0].Value.ToString();
                if (temp.Substring(temp.Length - 1, 1) != ";")
                {
                    temp += ";";
                }
                duringcode += temp + "\n";
            }

            if (_statusUnitName == "")//new
            {
                _statusUnit.Name = newStatusUnitName;
                _statusUnit.EnterFunction = entercode;
                _statusUnit.EnterNote = entercodenote;
                _statusUnit.DuringFunction = duringcode;
                _statusUnit.DuringNote = duringcodenote;
                _statusUnit.PositionSize = PointStatusUnitInit.X + "," + PointStatusUnitInit.Y + ",180,80";
                _statusUnit.WorkStationName = _workstationName;

                var lstStatusUnit = ClassComm.DeviceConfig.StatusUnits.ToList();
                lstStatusUnit.Add(_statusUnit);
                ClassComm.DeviceConfig.StatusUnits = lstStatusUnit.ToArray();
            }
            else if (_statusUnitName == newStatusUnitName)//update name same, just update function
            {
                _statusUnit.EnterFunction = entercode;
                _statusUnit.EnterNote = entercodenote;
                _statusUnit.DuringFunction = duringcode;
                _statusUnit.DuringNote = duringcodenote;
            }
            else// update include name change
            {
                var existingStatusUnit = ClassComm.DeviceConfig.StatusUnits
                    .ToList()
                    .FirstOrDefault(t => t.Name == _statusUnitName && t.WorkStationName == _workstationName);

                if (existingStatusUnit != null)
                {
                    // 更新状态单元基本信息
                    _statusUnit.Name = newStatusUnitName;
                    _statusUnit.EnterFunction = entercode;
                    _statusUnit.EnterNote = entercodenote;
                    _statusUnit.DuringFunction = duringcode;
                    _statusUnit.DuringNote = duringcodenote;

                    // 同步更新所有相关的条件连线（起点和终点）
                    if (ClassComm.DeviceConfig.Conditions != null)
                    {
                        ClassComm.DeviceConfig.Conditions
                            .ToList()
                            .FindAll(t => t.WorkStationName == _workstationName
                                          && (t.SourceSuName == _statusUnitName || t.TargetSuName == _statusUnitName))
                            .ForEach(condition =>
                            {
                                if (condition.SourceSuName == _statusUnitName)
                                    condition.SourceSuName = newStatusUnitName;

                                if (condition.TargetSuName == _statusUnitName)
                                    condition.TargetSuName = newStatusUnitName;

                                // 更新条件名称为 newName_newName 格式
                                condition.Name = condition.SourceSuName + "_" + condition.TargetSuName;
                            });
                    }
                }
                else
                {
                    MessageBox.Show(@"未找到需要更新的状态单元，请确认");
                    return;
                }
            }

            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

            Close();
        }

        private void toolStripButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public class GridMode
        {
            public string Id { get; set; }
            public string Value { get; set; }
        }

        private void toolStripButtonAddDelay_Click(object sender, EventArgs e)
        {
            var findCheckApp = ClassComm.DeviceConfig.Controllers.ToList().Find(f => f.Type == "CheckApp");
            if (findCheckApp != null)
            {
                if (toolStripTxtAddDelay.Text != "")
                {
                    int ms;
                    if (int.TryParse(toolStripTxtAddDelay.Text, out ms))
                    {
                        var str = string.Format("{0}.Method.Sleep({1})", findCheckApp.Name, ms);

                        if (_lcbFunctionSelect.comboBox.SelectedIndex == 0) //enter
                        {
                            int rowNum;
                            if (_dgvFunctionEnter.dataGridView.SelectedRows.Count > 0)
                            {
                                rowNum = _dgvFunctionEnter.dataGridView.SelectedRows[0].Index;
                                _dgvFunctionEnter.dataGridView.Rows.Insert(rowNum);
                            }
                            else
                            {
                                rowNum = _dgvFunctionEnter.dataGridView.Rows.Add();
                            }
                            //
                            //_dgvFunctionEnter.dataGridView.Rows[rowNum].Cells[0].Value = code + ";";
                            _dgvFunctionEnter.dataGridView.Rows[rowNum].Cells[0].Value = str + ";";
                            //_dgvFunctionEnter.dataGridView.Rows[rowNum].Cells[1].Value = codeNote + ";";
                        }
                        else if (_lcbFunctionSelect.comboBox.SelectedIndex == 1) //during
                        {
                            int rowNum;
                            if (_dgvFunctionDuring.dataGridView.SelectedRows.Count > 0)
                            {
                                rowNum = _dgvFunctionDuring.dataGridView.SelectedRows[0].Index;
                                _dgvFunctionDuring.dataGridView.Rows.Insert(rowNum);
                            }
                            else
                            {
                                rowNum = _dgvFunctionDuring.dataGridView.Rows.Add();
                            }
                            //
                            //_dgvFunctionDuring.dataGridView.Rows[rowNum].Cells[0].Value = code + ";";
                            _dgvFunctionDuring.dataGridView.Rows[rowNum].Cells[0].Value = str + ";";
                            //_dgvFunctionDuring.dataGridView.Rows[rowNum].Cells[1].Value = codeNote + ";";
                        }

                        return;
                    }
                }
            }

            MessageBox.Show(@"比较值请选择一个");
        }
    }
}
