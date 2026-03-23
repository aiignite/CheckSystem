using DeviceDesign.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using StateMachine;

namespace DeviceDesign
{
    public sealed partial class FormDeviceConfigComm : FormBase
    {
        public object ObjSelected;
        private int _rowNumSelected;
        private readonly bool _isReadOnly;

        public FormDeviceConfigComm()
        {
            InitializeComponent();
        }

        public FormDeviceConfigComm(string name)
        {
            InitializeComponent();
            Name = name;
            Text = Name;

            if (name == "DeviceConfigPart")
                _isReadOnly = true;
        }

        private void FormDeviceConfigComm_Load(object sender, EventArgs e)
        {
            var asmb = Assembly.LoadFrom("StateMachine.dll");
            var deviceType = "StateMachine." + Name;
            try
            {
                ObjSelected = Activator.CreateInstance(asmb.GetType(deviceType));
            }
            catch (Exception)
            {
                ObjSelected = null;
            }

            DataGridInit();
            ToolStripInit();

            // 订阅控制器名称变更事件，当为部件映射表或工序参数表时需要刷新
            if (Name == "DeviceConfigPart" || Name == "DeviceConfigPara")
            {
                ClassComm.ControllerNameChanged += ClassComm_ControllerNameChanged;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (Name == "DeviceConfigPart" || Name == "DeviceConfigPara")
            {
                ClassComm.ControllerNameChanged -= ClassComm_ControllerNameChanged;
            }
            base.OnFormClosed(e);
        }

        private void ClassComm_ControllerNameChanged(object sender, ClassComm.ControllerNameChangedEventArgs e)
        {
            // 刷新部件映射表
            this.Invoke(new Action(() =>
            {
                UpdateDataGrid();
            }));
        }

        #region ToolStrip

        private ToolStrip _toolStrip;
        private void ToolStripInit()
        {
            _toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top,
                Size = new Size(500, 40),
                BackColor = Color.LightBlue
            };

            var toolStripButtonAdd = new ToolStripButton();
            var toolStripButtonEdit = new ToolStripButton();
            var toolStripButtonDel = new ToolStripButton();
            var toolStripButtonSave = new ToolStripButton();
            // 
            // toolStrip
            // 
            _toolStrip.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            _toolStrip.ImageScalingSize = new Size(32, 32);
            _toolStrip.Items.AddRange(new ToolStripItem[] {
            toolStripButtonAdd,
            toolStripButtonEdit,
            toolStripButtonDel,
            toolStripButtonSave
            });
            _toolStrip.Location = new Point(0, 0);
            _toolStrip.Name = "toolStrip";

            // 
            // toolStripButtonAdd
            // 
            toolStripButtonAdd.Image = Resources.add;
            toolStripButtonAdd.ImageTransparentColor = Color.Magenta;
            toolStripButtonAdd.Name = "toolStripButtonAdd";
            toolStripButtonAdd.Size = new Size(78, 36);
            toolStripButtonAdd.Text = @"添加";
            toolStripButtonAdd.Click += ToolStripButtonAdd_Click;
            // 
            // toolStripButtonEdit
            // 
            toolStripButtonEdit.Image = Resources.edit;
            toolStripButtonEdit.ImageTransparentColor = Color.Magenta;
            toolStripButtonEdit.Name = "toolStripButtonEdit";
            toolStripButtonEdit.Size = new Size(78, 36);
            toolStripButtonEdit.Text = @"修改";
            toolStripButtonEdit.Click += ToolStripButtonEdit_Click;

            toolStripButtonDel.Image = Resources.del;
            toolStripButtonDel.ImageTransparentColor = Color.Magenta;
            toolStripButtonDel.Name = "toolStripButtonDel";
            toolStripButtonDel.Size = new Size(78, 36);
            toolStripButtonDel.Text = @"删除";
            toolStripButtonDel.Click += ToolStripButtonDel_Click;

            toolStripButtonSave.Image = Resources.Save;
            toolStripButtonSave.ImageTransparentColor = Color.Magenta;
            toolStripButtonSave.Name = "toolStripButtonSave";
            toolStripButtonSave.Size = new Size(78, 36);
            toolStripButtonSave.Text = @"保存";
            toolStripButtonSave.Click += ToolStripButtonSave_Click;

            Controls.Add(_toolStrip);
        }

        private void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveDataGridToXml();
            UpdateDataGrid();
        }

        private void ToolStripButtonDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"请确认彻底删除该选择项！", @"确认", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            //只处理多行数据
            var prop = ClassComm.DeviceConfig.GetType().GetRuntimeProperties().ToList().Find(t => t.PropertyType.Name.Equals(Name + "[]"));
            if (prop != null)
            {
                var asmb = Assembly.LoadFrom("StateMachine.dll");
                var deviceType = "StateMachine." + Name;
                //删除选择行
                _userDataGrid.dataGridView.Rows.RemoveAt(_rowNumSelected);
                //先产生对应类型的数组，然后给数组赋值，则可以完成数据类型对应
                var arrObjs = Array.CreateInstance(asmb.GetType(deviceType), _userDataGrid.dataGridView.Rows.Count - 1);

                for (var i = 0; i < _userDataGrid.dataGridView.Rows.Count - 1; i++)
                {
                    var configItem = Activator.CreateInstance(asmb.GetType(deviceType));
                    foreach (var item in configItem.GetType().GetProperties())
                    {
                        item.SetValue(configItem,
                            ClassComm.MyChanageType(_userDataGrid.dataGridView.Rows[i].Cells[item.Name].Value,
                                item.PropertyType));
                    }
                    //把对象填充到数组中
                    arrObjs.SetValue(configItem, i);
                }
                //把数组赋值给DeviceConfig
                prop.SetValue(ClassComm.DeviceConfig, arrObjs, null);
            }
            else
            {
                MessageBox.Show(@"只删除数组数据");
            }
            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

            UpdateDataGrid();
        }

        private void ToolStripButtonEdit_Click(object sender, EventArgs e)
        {
            if (_isReadOnly)
                return;

            var objectType = Type.GetType("DeviceDesign.FormEdit" + Name);
            if (objectType == null) return;

            var form = (Form)Activator.CreateInstance(objectType, ObjSelected);
            form.ShowDialog();
            UpdateDataGrid();
        }

        private void ToolStripButtonAdd_Click(object sender, EventArgs e)
        {
            if (_isReadOnly)
                return;

            var objectType = Type.GetType("DeviceDesign.FormAdd" + Name);
            if (objectType == null) return;

            var form = (Form)Activator.CreateInstance(objectType);
            form.ShowDialog();
            UpdateDataGrid();
        }

        private void SaveDataGridToXml()
        {
            if (MessageBox.Show(@"请确认没有筛选,鼠标已离开编辑区等，操作会更新XML文件", @"确认", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            var asmb = Assembly.LoadFrom("StateMachine.dll");
            var deviceType = "StateMachine." + Name;

            //分析DeviceConfig中对应项时数组还是单个对象
            var prop1 = ClassComm.DeviceConfig.GetType().GetRuntimeProperties().ToList().Find(t => t.PropertyType.Name.Equals(Name));
            if (prop1 != null)
            {
                var configItem = Activator.CreateInstance(asmb.GetType(deviceType));
                foreach (var item in configItem.GetType().GetProperties())
                {
                    item.SetValue(configItem, ClassComm.MyChanageType(_userDataGrid.dataGridView.Rows[0].Cells[item.Name].Value, item.PropertyType));
                }
                prop1.SetValue(ClassComm.DeviceConfig, configItem, null);
            }
            else//数组
            {
                var prop2 = ClassComm.DeviceConfig.GetType().GetRuntimeProperties().ToList().Find(t => t.PropertyType.Name.Equals(Name + "[]"));
                if (prop2 != null)
                {
                    //先产生对应类型的数组，然后给数组赋值，则可以完成数据类型对应
                    var arrObjs = Array.CreateInstance(asmb.GetType(deviceType), _userDataGrid.dataGridView.Rows.Count - 1);

                    for (var i = 0; i < _userDataGrid.dataGridView.Rows.Count - 1; i++)
                    {
                        var configItem = Activator.CreateInstance(asmb.GetType(deviceType));
                        foreach (var item in configItem.GetType().GetProperties())
                        {
                            item.SetValue(configItem, ClassComm.MyChanageType(_userDataGrid.dataGridView.Rows[i].Cells[item.Name].Value, item.PropertyType));
                        }
                        //把对象填充到数组中
                        arrObjs.SetValue(configItem, i);
                    }
                    //把数组赋值给DeviceConfig
                    prop2.SetValue(ClassComm.DeviceConfig, arrObjs, null);
                }
            }

            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);
        }

        #endregion

        #region DataGridView

        private readonly UserControls.UserDataGrid _userDataGrid = new UserControls.UserDataGrid();

        private void DataGridInit()
        {
            _userDataGrid.dataGridView.Columns.Clear();

            if (ObjSelected == null)
                return;

            //初始化列
            foreach (var item in ObjSelected.GetType().GetProperties())
            {
                _userDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = item.Name });
            }
            //禁止排序
            for (var i = 0; i < _userDataGrid.dataGridView.Columns.Count; i++)
            {
                _userDataGrid.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            Controls.Add(_userDataGrid);
            UpdateDataGrid();

            _userDataGrid.dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
            _userDataGrid.dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            _userDataGrid.label.Text = Name;
            _userDataGrid.Dock = DockStyle.Fill;

            if (Name == "DeviceConfigPart")
            {
                _userDataGrid.dataGridView.ReadOnly = true;
            }
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //选中header时currentcell为null
            if (_userDataGrid.dataGridView.CurrentCell == null)
                return;

            if (_userDataGrid.dataGridView.CurrentRow == null)
                return;

            _rowNumSelected = _userDataGrid.dataGridView.CurrentRow.Index;
            //Name值不为空
            //      if (userDataGrid.dataGridView.CurrentRow.Cells[0].Value != null)
            {
                foreach (
                    var item in
                        ObjSelected.GetType()
                            .GetProperties()
                            .Where(item => _userDataGrid.dataGridView.CurrentRow.Cells[item.Name].Value != null))
                {
                    item.SetValue(ObjSelected, _userDataGrid.dataGridView.CurrentRow.Cells[item.Name].Value.ToString());
                }
            }
        }

        private void UpdateDataGrid()
        {
            _userDataGrid.dataGridView.Rows.Clear();
            int rowNum;
            var colNum = 0;

            switch (Name)
            {
                case "DeviceConfigDeviceInfo":
                    var info = ClassComm.DeviceConfig.DeviceInfo.GetType().GetProperties();
                    rowNum = _userDataGrid.dataGridView.Rows.Add();
                    foreach (var item in info)
                    {
                        _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value =
                            item.GetValue(ClassComm.DeviceConfig.DeviceInfo, null);
                    }
                    break;

                case "DeviceConfigFormLayout":
                    var layout = ClassComm.DeviceConfig.FormLayout.GetType().GetProperties();
                    rowNum = _userDataGrid.dataGridView.Rows.Add();
                    foreach (var item in layout)
                        _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value =
                            item.GetValue(ClassComm.DeviceConfig.FormLayout, null);
                    break;

                case "DeviceConfigControl":
                    foreach (var prop in ClassComm.DeviceConfig.Controls)
                    {
                        rowNum = _userDataGrid.dataGridView.Rows.Add();
                        colNum = 0;
                        foreach (var item in prop.GetType().GetProperties())
                            _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value = item.GetValue(prop, null);
                    }
                    break;

                case "DeviceConfigController":

                    foreach (var prop in ClassComm.DeviceConfig.Controllers)
                    {
                        rowNum = _userDataGrid.dataGridView.Rows.Add();
                        colNum = 0;
                        foreach (var item in prop.GetType().GetProperties())
                            _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value = item.GetValue(prop, null);
                    }
                    break;

                case "DeviceConfigProperty":

                    foreach (var prop in ClassComm.DeviceConfig.ControllerProperties)
                    {
                        rowNum = _userDataGrid.dataGridView.Rows.Add();
                        colNum = 0;
                        foreach (var item in prop.GetType().GetProperties())
                            _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value = item.GetValue(prop, null);
                    }
                    break;

                case "DeviceConfigPart":
                    foreach (var prop in ClassComm.DeviceConfig.Parts)
                    {
                        rowNum = _userDataGrid.dataGridView.Rows.Add();
                        colNum = 0;
                        foreach (var item in prop.GetType().GetProperties())
                            _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value = item.GetValue(prop, null);
                    }
                    break;

                case "DeviceConfigPara":

                    if (ClassComm.DeviceConfig.Paras == null)
                        ClassComm.DeviceConfig.Paras = new DeviceConfigPara[0];
                    ClassComm.SaveDeviceConfigToFile(
                        ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

                    foreach (var prop in ClassComm.DeviceConfig.Paras)
                    {
                        rowNum = _userDataGrid.dataGridView.Rows.Add();
                        colNum = 0;
                        foreach (var item in prop.GetType().GetProperties())
                            _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value = item.GetValue(prop, null);
                    }
                    break;

                case "DeviceConfigGear":

                    if (ClassComm.DeviceConfig.Gears == null)
                        ClassComm.DeviceConfig.Gears = new DeviceConfigGear[0];
                    ClassComm.SaveDeviceConfigToFile(
                        ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

                    foreach (var prop in ClassComm.DeviceConfig.Gears)
                    {
                        rowNum = _userDataGrid.dataGridView.Rows.Add();
                        colNum = 0;
                        foreach (var item in prop.GetType().GetProperties())
                            _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value = item.GetValue(prop, null);
                    }
                    break;

                case "DeviceConfigRoi":

                    if (ClassComm.DeviceConfig.Rois == null)
                        ClassComm.DeviceConfig.Rois = new DeviceConfigRoi[0];
                    ClassComm.SaveDeviceConfigToFile(
                        ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

                    foreach (var prop in ClassComm.DeviceConfig.Rois)
                    {
                        rowNum = _userDataGrid.dataGridView.Rows.Add();
                        colNum = 0;
                        foreach (var item in prop.GetType().GetProperties())
                            _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value = item.GetValue(prop, null);
                    }
                    break;

                case "DeviceConfigProcess":

                    if (ClassComm.DeviceConfig.Processes != null)
                    {
                        foreach (var process in ClassComm.DeviceConfig.Processes)
                        {
                            rowNum = _userDataGrid.dataGridView.Rows.Add();
                            colNum = 0;
                            foreach (var item in process.GetType().GetProperties())
                                _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value = item.GetValue(process, null);
                        }
                    }

                    break;

                case "DeviceConfigWorkStation":

                    foreach (var prop in ClassComm.DeviceConfig.WorkStations)
                    {
                        rowNum = _userDataGrid.dataGridView.Rows.Add();
                        colNum = 0;
                        foreach (var item in prop.GetType().GetProperties())
                            _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value = item.GetValue(prop, null);
                    }
                    break;

                case "DeviceConfigStatusUnit":

                    foreach (var prop in ClassComm.DeviceConfig.StatusUnits)
                    {
                        rowNum = _userDataGrid.dataGridView.Rows.Add();
                        colNum = 0;

                        prop.EnterNote = prop.EnterFunction;
                        prop.DuringNote = prop.DuringFunction;

                        foreach (var item in prop.GetType().GetProperties())
                            _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value = item.GetValue(prop, null);
                    }
                    break;

                case "DeviceConfigCondition":

                    foreach (var prop in ClassComm.DeviceConfig.Conditions)
                    {
                        rowNum = _userDataGrid.dataGridView.Rows.Add();
                        colNum = 0;

                        prop.ConditionNote = prop.ConditionFunction;
                        prop.ExitNote = prop.ExitFunction;

                        foreach (var item in prop.GetType().GetProperties())
                            _userDataGrid.dataGridView.Rows[rowNum].Cells[colNum++].Value = item.GetValue(prop, null);
                    }
                    break;
            }
        }
        #endregion
    }
}
