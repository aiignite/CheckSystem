using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UserControls;
using System.Reflection;
using StateMachine;

namespace DeviceDesign
{
    public partial class FormConditions : Form
    {
        private readonly DeviceConfigWorkStation _selectWorkstation;
        private readonly DeviceConfigCondition _condition;

        private readonly List<DeviceConfigStatusUnit> _statusUnits;
        private readonly List<DeviceConfigCondition> _conditions;

        private readonly UserDataGrid _dgvFunctionCondition = new UserDataGrid();
        private readonly Label _lbFunctionConditiont = new Label();
        private readonly UserDataGrid _dgvFunctionExit = new UserDataGrid();
        private readonly Label _lbFunctionExit = new Label();

        private readonly LabelText _ltConditionName = new LabelText();
        private readonly LabelCombox _lcbSourceStatusUnit = new LabelCombox();
        private readonly LabelCombox _lcbTargetStatusUnit = new LabelCombox();

        private readonly LabelCombox _lcbFunctionSelect = new LabelCombox();
        private readonly LabelCombox _lcbConditionType = new LabelCombox();  //参数条件或者控制器变量条件

        private readonly LabelCombox _lcbTypePara = new LabelCombox();
        private readonly LabelCombox _lcbConditionSymbol = new LabelCombox();
        private readonly LabelCombox _lcbParaResult = new LabelCombox();

        private readonly LabelCombox _lcbControllerField = new LabelCombox();
        private readonly LabelCombox _lcbControllerField2 = new LabelCombox();
        private readonly LabelText _ltValue = new LabelText();

        private readonly LabelCombox _lcbConnector = new LabelCombox();

        private readonly LabelCombox _lcbSensorOrActuator = new LabelCombox();
        private readonly LabelCombox _lcbEqualSymbol = new LabelCombox();

        private readonly Button _btnAdd = new Button();

        public FormConditions()
        {
            InitializeComponent();
        }

        public FormConditions(string workstationname, string conditionname)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Height = SystemInformation.WorkingArea.Height;
            Width = SystemInformation.WorkingArea.Width;
            _selectWorkstation = ClassComm.DeviceConfig.WorkStations.ToList().Find(t => t.Name == workstationname);
            if (_selectWorkstation != null)
            {
                var condition = ClassComm.DeviceConfig.Conditions.ToList().Find(t => t.Name == conditionname && t.WorkStationName == workstationname);
                if (condition != null)
                {
                    _condition = condition;
                    _lcbSourceStatusUnit.Enabled = false;
                    _lcbTargetStatusUnit.Enabled = false;
                }
                else
                {
                    _condition = new DeviceConfigCondition { Name = "_" };
                }
                _statusUnits = ClassComm.DeviceConfig.StatusUnits.ToList().FindAll(t => t.WorkStationName == _selectWorkstation.Name);
                _conditions = ClassComm.DeviceConfig.Conditions.ToList().FindAll(t => t.WorkStationName == _selectWorkstation.Name);

            }
        }

        private void FormConditions_Load(object sender, EventArgs e)
        {
            Text = _selectWorkstation.Name;
            //WindowState = FormWindowState.Maximized;
            ControlsInit();

            _lcbConditionType.comboBox.SelectedIndex = 0;
        }

        private void ControlsInit()
        {
            //_lbFunctionConditiont.Text = @"条件函数";
            //_lbFunctionConditiont.Dock = DockStyle.Top;
            //_lbFunctionConditiont.BackColor = Color.AntiqueWhite;

            //_lbFunctionExit.Text = @"退出函数";
            //_lbFunctionExit.Dock = DockStyle.Top;
            //_lbFunctionExit.BackColor = Color.AntiqueWhite;

            _dgvFunctionCondition.Dock = DockStyle.Top;
            _dgvFunctionCondition.label.Text = @"条件函数";
            _dgvFunctionCondition.Height = 200;
            _dgvFunctionCondition.dataGridView.AllowUserToAddRows = false;
            _dgvFunctionCondition.dataGridView.Columns.Clear();
            _dgvFunctionCondition.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "条件函数" });
            //_dgvFunctionCondition.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "条件函数说明" });
            _dgvFunctionCondition.dataGridView.Columns[0].DefaultCellStyle.BackColor = Color.AntiqueWhite;
            _dgvFunctionCondition.dataGridView.GridColor = Color.White;
            for (var i = 0; i < _dgvFunctionCondition.dataGridView.Columns.Count; i++)
                _dgvFunctionCondition.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            _dgvFunctionExit.Dock = DockStyle.Top;
            _dgvFunctionExit.label.Text = @"退出函数";
            _dgvFunctionExit.Height = 200;
            _dgvFunctionExit.dataGridView.AllowUserToAddRows = false;
            _dgvFunctionExit.dataGridView.Columns.Clear();
            _dgvFunctionExit.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "退出函数" });
            //_dgvFunctionExit.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "退出函数说明" });
            _dgvFunctionExit.dataGridView.Columns[0].DefaultCellStyle.BackColor = Color.AntiqueWhite;
            for (var i = 0; i < _dgvFunctionExit.dataGridView.Columns.Count; i++)
                _dgvFunctionExit.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            _ltConditionName.label.Text = @"条件名称";
            _ltConditionName.Dock = DockStyle.Top;
            _ltConditionName.Enabled = false;

            _lcbSourceStatusUnit.label.Text = @"源状态单元";
            _lcbSourceStatusUnit.Dock = DockStyle.Top;
            _lcbTargetStatusUnit.label.Text = @"目标状态单元";
            _lcbTargetStatusUnit.Dock = DockStyle.Top;

            foreach (var su in _statusUnits)
            {
                _lcbSourceStatusUnit.comboBox.Items.Add(su.Name);
                _lcbTargetStatusUnit.comboBox.Items.Add(su.Name);
            }

            _lcbFunctionSelect.label.Text = @"函数选择";
            _lcbFunctionSelect.Dock = DockStyle.Top;
            _lcbFunctionSelect.comboBox.Items.Add("条件函数");
            _lcbFunctionSelect.comboBox.Items.Add("退出函数");
            _lcbFunctionSelect.comboBox.SelectedIndex = 0;

            _lcbConditionType.label.Text = @"条件类型";
            _lcbConditionType.Dock = DockStyle.Top;
            _lcbConditionType.comboBox.Items.Add("变量条件");
            _lcbConditionType.comboBox.Items.Add("根据参数");
            _lcbFunctionSelect.comboBox.SelectedIndex = 0;

            _lcbTypePara.label.Text = @"条件类型";
            _lcbTypePara.Dock = DockStyle.Top;
            //添加数据
            foreach (var para in ClassComm.DeviceConfig.Paras.ToList())
            {
                _lcbTypePara.comboBox.Items.Add(para.ProcessNo + ".Para." + para.Name);
            }

            _lcbConditionSymbol.label.Text = @"条件符号";
            _lcbConditionSymbol.Dock = DockStyle.Top;
            _lcbConditionSymbol.comboBox.Items.Add("==");
            _lcbConditionSymbol.comboBox.Items.Add("!=");
            _lcbConditionSymbol.comboBox.Items.Add(">");
            _lcbConditionSymbol.comboBox.Items.Add("<");
            _lcbConditionSymbol.comboBox.Items.Add(">=");
            _lcbConditionSymbol.comboBox.Items.Add("<=");
            _lcbConditionSymbol.comboBox.SelectedIndex = 0;

            _lcbParaResult.label.Text = @"参数结果";
            _lcbParaResult.Dock = DockStyle.Top;
            _lcbParaResult.comboBox.Items.Add("TRUE");
            _lcbParaResult.comboBox.Items.Add("FALSE");


            _lcbControllerField.label.Text = @"控制器变量";
            _lcbControllerField.Dock = DockStyle.Top;
            _lcbControllerField.label.Text = @"控制器变量";
            _lcbControllerField.Dock = DockStyle.Top;
            //数据


            _lcbControllerField2.label.Text = @"控制器变量";
            _lcbControllerField2.Dock = DockStyle.Top;
            _lcbControllerField2.label.Text = @"控制器变量";
            _lcbControllerField2.Dock = DockStyle.Top;

            foreach (var controller in ClassComm.DeviceConfig.Controllers)
            {
                var typeName = ClassComm.DllAsmb.GetType("Controller." + controller.Type);

                if (typeName != null)
                {
                    var methods = typeName.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    var fields = typeName.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                    foreach (var field in fields)
                    {
                        //_lcbControllerField.comboBox.Items.Add(controller.Name + ".Field." +
                        //                                       field.ToString()
                        //                                           .Substring(
                        //                                               field.ToString()
                        //                                                   .IndexOf(" ", StringComparison.Ordinal) + 1));
                        _lcbControllerField.ListText.Add(controller.Name + ".Field." +
                                                      field.ToString().Substring(field.ToString().IndexOf(" ", StringComparison.Ordinal) + 1));
                        //_lcbControllerField2.comboBox.Items.Add(controller.Name + ".Field." +
                        //                                        field.ToString()
                        //                                            .Substring(
                        //                                                field.ToString()
                        //                                                    .IndexOf(" ", StringComparison.Ordinal) + 1));
                        _lcbControllerField2.ListText.Add(controller.Name + ".Field." +
                                                          field.ToString()
                                                              .Substring(
                                                                  field.ToString()
                                                                      .IndexOf(" ", StringComparison.Ordinal) + 1));
                    }
                    foreach (var method in methods)
                    {
                        var para = method.GetParameters().ToList();
                        var strPara = string.Join(",", para);
                        //_lcbControllerField.comboBox.Items.Add(controller.Name + ".Method." + method.Name + "(" + strPara + ")");//
                        _lcbControllerField.ListText.Add(controller.Name + ".Method." + method.Name + "(" + strPara + ")");//
                        //_lcbControllerField2.comboBox.Items.Add(controller.Name + ".Method." + method.Name + "(" + strPara + ")");//
                        _lcbControllerField2.ListText.Add(controller.Name + ".Method." + method.Name + "(" + strPara + ")");//
                    }
                }
            }


            _lcbConnector.label.Text = @"条件连接符";
            _lcbConnector.Dock = DockStyle.Top;
            _lcbConnector.comboBox.Items.Add("&&");
            _lcbConnector.comboBox.Items.Add("||");
            _lcbConnector.comboBox.SelectedIndex = 0;

            _ltValue.label.Text = @"值";
            _ltValue.Dock = DockStyle.Top;

            _lcbSensorOrActuator.label.Text = @"传感器执行器";
            _lcbSensorOrActuator.Dock = DockStyle.Top;

            foreach (var item in ClassComm.DeviceConfig.Parts)
            {
                _lcbSensorOrActuator.comboBox.Items.Add(item.ProcessNo + ".Part." + item.Name);
                _lcbSensorOrActuator.ListText.Add(item.ProcessNo + ".Part." + item.Name);
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
            splitContainer1.Panel1.Controls.Add(_lcbConnector);
            splitContainer1.Panel1.Controls.Add(_ltValue);
            splitContainer1.Panel1.Controls.Add(_lcbControllerField2);
            splitContainer1.Panel1.Controls.Add(_lcbParaResult);  //para result
            splitContainer1.Panel1.Controls.Add(_lcbConditionSymbol);
            splitContainer1.Panel1.Controls.Add(_lcbEqualSymbol);
            splitContainer1.Panel1.Controls.Add(_lcbControllerField);
            splitContainer1.Panel1.Controls.Add(_lcbSensorOrActuator);
            splitContainer1.Panel1.Controls.Add(_lcbTypePara);  //para table
            splitContainer1.Panel1.Controls.Add(_lcbConditionType);
            splitContainer1.Panel1.Controls.Add(_lcbFunctionSelect);
            splitContainer1.Panel1.Controls.Add(_lcbTargetStatusUnit);
            splitContainer1.Panel1.Controls.Add(_lcbSourceStatusUnit);
            splitContainer1.Panel1.Controls.Add(_ltConditionName);

            splitContainer1.Panel2.Controls.Add(_dgvFunctionExit);
            //      this.splitContainer1.Panel2.Controls.Add(_lbFunctionExit);
            splitContainer1.Panel2.Controls.Add(_dgvFunctionCondition);
            //         this.splitContainer1.Panel2.Controls.Add(_lbFunctionConditiont);

            _lcbSourceStatusUnit.comboBox.SelectedIndexChanged += SourceStatusUnitComboBox_SelectedIndexChanged;
            _lcbTargetStatusUnit.comboBox.SelectedIndexChanged += TargetStatusUnitComboBox_SelectedIndexChanged;
            _lcbFunctionSelect.comboBox.SelectedIndexChanged += FunctionSelectComboBox_SelectedIndexChanged;
            _lcbConditionType.comboBox.SelectedIndexChanged += ConditionTypeComboBox_SelectedIndexChanged;

            _lcbSensorOrActuator.comboBox.SelectedIndexChanged += SensorOrActuatorComboBox_SelectedIndexChanged;
            _lcbControllerField.comboBox.SelectedIndexChanged += ControllerFieldComboBox_SelectedIndexChanged;

            _lcbControllerField2.comboBox.SelectedIndexChanged += ControllerField2ComboBox_SelectedIndexChanged;
            _ltValue.textBox.TextChanged += ValueTextBox_TextChanged;

            _btnAdd.Click += _btnAdd_Click;
            UpdateControlsVisible();

            if (_condition.Name != null)
            {
                var sourceName = _condition.Name.Split('_')[0];
                var targetName = _condition.Name.Split('_')[1];
                _lcbSourceStatusUnit.Text = sourceName;
                _lcbTargetStatusUnit.Text = targetName;
                var cf = _condition.ConditionFunction ?? "";
                cf = cf.Replace("\n", "");
                cf = cf.Replace(" ", "");
                var lstCode = cf.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
                //var cn = _condition.ConditionNote ?? "";
                //cn = cn.Replace("\n", "");
                //cn = cn.Replace(" ", "");
                //var lstCodeNote = cn.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
                var i = 0;
                foreach (var item in lstCode)
                {
                    i++;
                    if (item == "") continue;
                    var lstOr = item.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    //var lstNoteOr = lstCodeNote[i - 1].Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    if (lstOr.Length == 1)
                    {
                        var numrow = _dgvFunctionCondition.dataGridView.Rows.Add();
                        _dgvFunctionCondition.dataGridView.Rows[numrow].Cells[0].Value = item + "&&";
                        //if (lstCodeNote.Length > numrow)
                        //    _dgvFunctionCondition.dataGridView.Rows[numrow].Cells[1].Value = lstCodeNote[numrow] + "&&";
                    }
                    else if (lstOr.Length > 1)
                    {
                        var j = 0;
                        foreach (var itemOr in lstOr)
                        {
                            j++;
                            var numrow = _dgvFunctionCondition.dataGridView.Rows.Add();
                            _dgvFunctionCondition.dataGridView.Rows[numrow].Cells[0].Value = itemOr + "||";
                            //if (lstCodeNote.Length > numrow)
                            //_dgvFunctionCondition.dataGridView.Rows[numrow].Cells[1].Value = lstNoteOr[j - 1] + "||";
                        }
                    }

                }

                var exit = _condition.ExitFunction ?? "";
                exit = exit.Replace("\n", "");
                exit = exit.Replace(" ", "");
                var lstExit = exit.Split(';');

                var exitnote = _condition.ExitNote ?? "";
                exitnote = exitnote.Replace("\n", "");
                exitnote = exitnote.Replace(" ", "");
                var lstExitNote = exitnote.Split(';');
                foreach (var item in lstExit)
                {
                    if (item == "") continue;
                    var numrow = _dgvFunctionExit.dataGridView.Rows.Add();
                    _dgvFunctionExit.dataGridView.Rows[numrow].Cells[0].Value = item + ";";
                    //if (lstExitNote.Length > numrow)
                    //    _dgvFunctionExit.dataGridView.Rows[numrow].Cells[1].Value = lstExitNote[numrow] + ";";
                }
            }
        }

        private void UpdateControlsVisible()
        {
            if (_lcbFunctionSelect.comboBox.SelectedIndex == 0) // 条件函数
            {
                _lcbConditionType.Visible = true;

                if (_dgvFunctionCondition.dataGridView.RowCount == 0)
                {
                    if (_lcbConditionType.comboBox.SelectedIndex == 0) // 变量条件
                    {
                        _lcbTypePara.Visible = false;
                        _lcbSensorOrActuator.Visible = true;
                        _lcbControllerField.Visible = true;
                        _lcbEqualSymbol.Visible = false;
                        _lcbConditionSymbol.Visible = true;
                        _lcbConditionSymbol.Enabled = true;
                        _lcbConditionSymbol.comboBox.SelectedIndex = 0;
                        _lcbParaResult.Visible = false;
                        _lcbControllerField2.Visible = true;
                        _ltValue.Visible = true;
                        _lcbConnector.Visible = true;
                        _lcbConnector.Enabled = true;
                        _lcbConnector.comboBox.SelectedIndex = 0;
                    }
                    else // 根据参数
                    {
                        _lcbTypePara.Visible = true;
                        _lcbSensorOrActuator.Visible = false;
                        _lcbControllerField.Visible = false;
                        _lcbEqualSymbol.Visible = false;
                        _lcbConditionSymbol.Visible = true;
                        _lcbConditionSymbol.Enabled = false;
                        _lcbConditionSymbol.comboBox.SelectedIndex = 0;
                        _lcbParaResult.Visible = true;
                        _lcbParaResult.Enabled = false;
                        _lcbParaResult.comboBox.SelectedIndex = 0;
                        _lcbControllerField2.Visible = false;
                        _ltValue.Visible = false;
                        _lcbConnector.Visible = true;
                        _lcbConnector.Enabled = false;
                        _lcbConnector.comboBox.SelectedIndex = 0;
                    }
                }
                else
                {
                    var isHavePara = false;
                    for (var i = 0; i < _dgvFunctionCondition.dataGridView.RowCount; i++)
                    {
                        var row = _dgvFunctionCondition.dataGridView.Rows[i];
                        var cell = row.Cells[0];

                        if (cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                        {
                            if (!cell.Value.ToString().Contains(".Para."))
                                continue;

                            isHavePara = true;
                            break;
                        }
                    }

                    if (_lcbConditionType.comboBox.SelectedIndex == 0) // 变量条件
                    {
                        if (isHavePara)
                        {
                            _lcbConditionType.comboBox.SelectedIndex = 1;
                            return;
                        }

                        _lcbTypePara.Visible = false;
                        _lcbSensorOrActuator.Visible = true;
                        _lcbControllerField.Visible = true;
                        _lcbEqualSymbol.Visible = false;
                        _lcbConditionSymbol.Visible = true;
                        _lcbConditionSymbol.Enabled = true;
                        _lcbConditionSymbol.comboBox.SelectedIndex = 0;
                        _lcbParaResult.Visible = false;
                        _lcbControllerField2.Visible = true;
                        _ltValue.Visible = true;
                        _lcbConnector.Visible = true;
                        _lcbConnector.Enabled = true;
                        _lcbConnector.comboBox.SelectedIndex = 0;
                    }
                    else // 根据参数
                    {
                        if (!isHavePara)
                        {
                            _lcbConditionType.comboBox.SelectedIndex = 0;
                            return;
                        }

                        _lcbTypePara.Visible = true;
                        _lcbSensorOrActuator.Visible = false;
                        _lcbControllerField.Visible = false;
                        _lcbEqualSymbol.Visible = false;
                        _lcbConditionSymbol.Visible = true;
                        _lcbConditionSymbol.Enabled = false;
                        _lcbConditionSymbol.comboBox.SelectedIndex = 0;
                        _lcbParaResult.Visible = true;
                        _lcbParaResult.Enabled = false;
                        _lcbParaResult.comboBox.SelectedIndex = 0;
                        _lcbControllerField2.Visible = false;
                        _ltValue.Visible = false;
                        _lcbConnector.Visible = true;
                        _lcbConnector.Enabled = false;
                        _lcbConnector.comboBox.SelectedIndex = 0;
                    }
                }
            }
            else //退出函数
            {
                _lcbConditionType.Visible = false;
                _lcbTypePara.Visible = false;
                _lcbSensorOrActuator.Visible = true;
                _lcbControllerField.Visible = true;
                _lcbEqualSymbol.Visible = true;
                _lcbConditionSymbol.Visible = false;
                _lcbParaResult.Visible = false;
                _lcbControllerField2.Visible = true;
                _ltValue.Visible = true;
                _lcbConnector.Visible = false;
            }
        }

        private void _btnAdd_Click(object sender, EventArgs e)
        {
            var code = "";
            var codeNote = "";

            if (_ltConditionName.Text != "")
            {
                var name = _ltConditionName.Text;
                var source = name.Substring(0, name.IndexOf("_", StringComparison.Ordinal));
                var target = name.Substring(name.IndexOf("_", StringComparison.Ordinal) + 1);
                if (source == "" || target == "" || (source.Equals(target)))
                {
                    MessageBox.Show(@"条件名称应该包含不同源状态和目标状态");
                    return;
                }
            }
            else
            {
                MessageBox.Show(@"条件名称不能为空");
                return;
            }

            if (_lcbFunctionSelect.comboBox.SelectedIndex == 0)//条件函数
            {
                if (_lcbConditionType.comboBox.SelectedIndex == 0)//变量条件
                {
                    if (_lcbSensorOrActuator.Text == "" && _lcbControllerField.Text != "")
                    {
                        code = _lcbControllerField.Text;
                        codeNote = _lcbControllerField.Text;
                    }
                    else if (_lcbSensorOrActuator.Text != "")
                    {
                        code = _lcbControllerField.Text;
                        codeNote = _lcbSensorOrActuator.Text;
                    }

                    if (_lcbConditionSymbol.Text != "")
                    {
                        code += _lcbConditionSymbol.Text;
                        codeNote += _lcbConditionSymbol.Text;
                    }
                    else
                    {
                        MessageBox.Show(@"条件比较符号不能为空");
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
                        MessageBox.Show(@"比较值请选择一个");
                        return;
                    }

                    if (_lcbConnector.Text != "")
                    {
                        code += _lcbConnector.Text;
                        codeNote += _lcbConnector.Text;
                    }
                    else
                    {
                        MessageBox.Show(@"条件连接符不能为空");
                        return;
                    }

                    int rowNum;
                    if (_dgvFunctionCondition.dataGridView.SelectedRows.Count > 0)
                    {
                        rowNum = _dgvFunctionCondition.dataGridView.SelectedRows[0].Index;
                        _dgvFunctionCondition.dataGridView.Rows.Insert(rowNum);
                    }
                    else
                    {
                        rowNum = _dgvFunctionCondition.dataGridView.Rows.Add();
                    }
                    //
                    //_dgvFunctionCondition.dataGridView.Rows[rowNum].Cells[0].Value = code;
                    _dgvFunctionCondition.dataGridView.Rows[rowNum].Cells[0].Value = codeNote;
                    //_dgvFunctionCondition.dataGridView.Rows[rowNum].Cells[1].Value = codeNote;

                }
                else//根据参数
                {
                    if (_lcbTypePara.Text != "")
                    {
                        code = _lcbTypePara.Text;
                    }
                    else
                    {
                        MessageBox.Show(@"参数不能为空");
                        return;
                    }
                    if (_lcbConditionSymbol.Text != "")
                    {
                        code += _lcbConditionSymbol.Text;
                    }
                    else
                    {
                        MessageBox.Show(@"条件比较符号不能为空");
                        return;
                    }
                    if (_lcbParaResult.Text != "")
                    {
                        code += _lcbParaResult.Text;
                    }
                    else
                    {
                        MessageBox.Show(@"条件参数结果不能为空");
                        return;
                    }
                    if (_lcbConnector.Text != "")
                    {
                        code += _lcbConnector.Text;
                    }
                    else
                    {
                        MessageBox.Show(@"条件连接符不能为空");
                        return;
                    }


                    int rowNum;
                    if (_dgvFunctionCondition.dataGridView.SelectedRows.Count > 0)
                    {
                        rowNum = _dgvFunctionCondition.dataGridView.SelectedRows[0].Index;
                        _dgvFunctionCondition.dataGridView.Rows.Insert(rowNum);
                    }
                    else
                    {
                        rowNum = _dgvFunctionCondition.dataGridView.Rows.Add();
                    }
                    //
                    _dgvFunctionCondition.dataGridView.Rows[rowNum].Cells[0].Value = code;
                    //_dgvFunctionCondition.dataGridView.Rows[rowNum].Cells[1].Value = code;
                }
            }
            else //退出函数
            {
                if (_lcbSensorOrActuator.Text == "" && _lcbControllerField.Text != "")
                {
                    code = _lcbControllerField.Text;
                    codeNote = _lcbControllerField.Text;
                }
                else if (_lcbSensorOrActuator.Text != "")
                {
                    code = _lcbControllerField.Text;
                    codeNote = _lcbSensorOrActuator.Text;
                }

                if (_lcbEqualSymbol.Text != "")
                {
                    if (code.Contains(".Method."))
                    {
                        code = code.Substring(0, code.IndexOf("(", StringComparison.Ordinal));
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
                        code += _lcbEqualSymbol.Text;
                        codeNote += _lcbEqualSymbol.Text;
                    }
                }
                else
                {
                    MessageBox.Show(@"条件比较符号不能为空");
                    return;
                }

                if (_lcbControllerField2.Text != "" && _ltValue.Text == "")
                {
                    code += _lcbControllerField2.Text;
                    codeNote += _lcbControllerField2.Text;
                }
                else if (_lcbControllerField2.Text == "" && _ltValue.Text != "")
                {
                    if (!code.Contains(".Method."))
                    {
                        code += _ltValue.Text;
                        codeNote += _ltValue.Text;
                    }
                }
                else
                {
                    if (!code.Contains(".Method."))
                    {
                        MessageBox.Show(@"比较值请选择一个");
                        return;
                    }
                }

                int rowNum;
                if (_dgvFunctionExit.dataGridView.SelectedRows.Count > 0)
                {
                    rowNum = _dgvFunctionExit.dataGridView.SelectedRows[0].Index;
                    _dgvFunctionExit.dataGridView.Rows.Insert(rowNum);
                }
                else
                {
                    rowNum = _dgvFunctionExit.dataGridView.Rows.Add();
                }
                //
                //_dgvFunctionExit.dataGridView.Rows[rowNum].Cells[0].Value = code + ";";
                _dgvFunctionExit.dataGridView.Rows[rowNum].Cells[0].Value = codeNote + ";";
                //_dgvFunctionExit.dataGridView.Rows[rowNum].Cells[1].Value = codeNote + ";";

            }
        }

        private void ValueTextBox_TextChanged(object sender, EventArgs e)
        {
            _lcbControllerField2.Text = "";
        }

        private void ControllerField2ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ltValue.Text = string.Empty;
        }

        private void ControllerFieldComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var part = ClassComm.DeviceConfig.Parts.ToList().Find(t => t.ControllerField == _lcbControllerField.Text);
            //var sa = ClassComm.DeviceConfig.Parts.ToList().Find(t => t.ControllerField == _lcbControllerField.Text);
            if (part != null)
            {
                _lcbSensorOrActuator.Text = part.ProcessNo + @".Part." + part.Name;
            }
            else
            {
                _lcbSensorOrActuator.Text = "";
            }
        }

        private void SensorOrActuatorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var part = ClassComm.DeviceConfig.Parts.ToList().Find(t => t.ProcessNo + @".Part." + t.Name == _lcbSensorOrActuator.Text);
            _lcbControllerField.Text = part != null ? part.ControllerField : string.Empty;
        }

        private void ConditionTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControlsVisible();
        }

        private void FunctionSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControlsVisible();
        }

        private void TargetStatusUnitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ltConditionName.Text = _lcbSourceStatusUnit.Text + @"_" + _lcbTargetStatusUnit.Text;
        }

        private void SourceStatusUnitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ltConditionName.Text = _lcbSourceStatusUnit.Text + @"_" + _lcbTargetStatusUnit.Text;
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            var cf = string.Empty;
            var cn = string.Empty;
            for (var i = 0; i < _dgvFunctionCondition.dataGridView.RowCount; i++)
            {
                cf += _dgvFunctionCondition.dataGridView.Rows[i].Cells[0].Value + "\n";
                //cn += _dgvFunctionCondition.dataGridView.Rows[i].Cells[1].Value + "\n";
            }

            var exit = string.Empty;
            var exitnote = string.Empty;
            for (var i = 0; i < _dgvFunctionExit.dataGridView.RowCount; i++)
            {
                exit += _dgvFunctionExit.dataGridView.Rows[i].Cells[0].Value + "\n";
                //exitnote += _dgvFunctionExit.dataGridView.Rows[i].Cells[1].Value + "\n";
            }

            var condition = ClassComm.DeviceConfig.Conditions.ToList().Find(t => t.Name == _condition.Name && t.WorkStationName == _selectWorkstation.Name);
            if (condition != null)  //更新记录
            {
                if (MessageBox.Show(@"已经存在该条件名称，是否更新？", @"更新", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    _condition.ConditionFunction = cf;
                    _condition.ConditionNote = cf;
                    _condition.ExitFunction = exit;
                    _condition.ExitNote = exit;
                }
            }
            else//新建记录
            {
                _condition.Name = _ltConditionName.Text;
                _condition.ConditionFunction = cf;
                _condition.ConditionNote = cf;
                _condition.ExitFunction = exit;
                _condition.ExitNote = exit;

                var lstCondition = ClassComm.DeviceConfig.Conditions.ToList();
                lstCondition.Add(_condition);
                ClassComm.DeviceConfig.Conditions = lstCondition.ToArray();
            }

            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

            Close();
        }

        private void toolStripButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButtonAddDelay_Click(object sender, EventArgs e)
        {
            if (_lcbFunctionSelect.comboBox.SelectedIndex == 1) //退出函数
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

                            int rowNum;
                            if (_dgvFunctionExit.dataGridView.SelectedRows.Count > 0)
                            {
                                rowNum = _dgvFunctionExit.dataGridView.SelectedRows[0].Index;
                                _dgvFunctionExit.dataGridView.Rows.Insert(rowNum);
                            }
                            else
                            {
                                rowNum = _dgvFunctionExit.dataGridView.Rows.Add();
                            }
                            //
                            //_dgvFunctionExit.dataGridView.Rows[rowNum].Cells[0].Value = code + ";";
                            _dgvFunctionExit.dataGridView.Rows[rowNum].Cells[0].Value = str + ";";
                            //_dgvFunctionExit.dataGridView.Rows[rowNum].Cells[1].Value = codeNote + ";";

                            return;
                        }
                    }
                }

                MessageBox.Show(@"比较值请选择一个");
            }
        }

        private void toolStripButtonAddEqualTrue_Click(object sender, EventArgs e)
        {
            if (_lcbFunctionSelect.comboBox.SelectedIndex == 0) //条件函数
            {
                var findCheckApp = ClassComm.DeviceConfig.Controllers.ToList().Find(f => f.Type == "CheckApp");

                if (findCheckApp != null && (_lcbConnector.Text != ""))
                {
                    var codeNote = string.Format("{0}.Field.EqualTrue==1{1}", findCheckApp.Name, _lcbConnector.Text);

                    int rowNum;
                    if (_dgvFunctionCondition.dataGridView.SelectedRows.Count > 0)
                    {
                        rowNum = _dgvFunctionCondition.dataGridView.SelectedRows[0].Index;
                        _dgvFunctionCondition.dataGridView.Rows.Insert(rowNum);
                    }
                    else
                    {
                        rowNum = _dgvFunctionCondition.dataGridView.Rows.Add();
                    }

                    _dgvFunctionCondition.dataGridView.Rows[rowNum].Cells[0].Value = codeNote;
                }
            }
        }
    }
}
