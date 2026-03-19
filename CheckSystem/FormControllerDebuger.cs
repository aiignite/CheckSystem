using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;
using HZH_Controls.Controls.Btn;
using HZH_Controls.Forms;
using HZH_Controls.IconFont;
using StateMachine;

namespace CheckSystem
{
    public partial class FormControllerDebuger : Form
    {
        private int _selectedControllerIndex = -1;
        private readonly State _status;
        private Thread _refreshFieldValueThread;
        private readonly object _dataGridviewLocker = new object();

        public FormControllerDebuger(State status)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            WindowState = FormWindowState.Maximized;
            Load += FormControllerDebuger_Load;
            Closed += FormControllerDebuger_Closed;

            Icon = FontImages.GetIcon(FontIcons.A_fa_simplybuilt, 32,
                Color.DodgerBlue);

            _status = status;

            dgvInputFields.label.Height = 30;
            dgvInputFields.label.Text = @"字段列表-R";
            dgvInputFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段名称" });
            dgvInputFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段备注" });
            dgvInputFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段类型" });
            dgvInputFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "映射名称" });
            dgvInputFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "当前值" });
            dgvInputFields.dataGridView.ReadOnly = true;
            dgvInputFields.dataGridView.RowHeadersVisible = false;
            dgvInputFields.dataGridView.AllowUserToAddRows = false;
            dgvInputFields.dataGridView.AllowUserToDeleteRows = false;
            dgvInputFields.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInputFields.dataGridView.AllowUserToResizeColumns = true;
            dgvInputFields.dataGridView.AllowUserToResizeRows = false;
            for (var i = 0; i < dgvInputFields.dataGridView.Columns.Count; i++)
                dgvInputFields.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            dgvOutpuFields.label.Height = 30;
            dgvOutpuFields.label.Text = @"字段列表-R/W";
            dgvOutpuFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段名称" });
            dgvOutpuFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段备注" });
            dgvOutpuFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段类型" });
            dgvOutpuFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "映射名称" });
            dgvOutpuFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "当前值" });
            dgvOutpuFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "修改值" });
            dgvOutpuFields.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "设置按钮", DefaultCellStyle = { NullValue = "设置" } });
            dgvOutpuFields.dataGridView.RowHeadersVisible = false;
            dgvOutpuFields.dataGridView.AllowUserToAddRows = false;
            dgvOutpuFields.dataGridView.AllowUserToDeleteRows = false;
            dgvOutpuFields.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOutpuFields.dataGridView.AllowUserToResizeColumns = true;
            dgvOutpuFields.dataGridView.AllowUserToResizeRows = false;
            for (var i = 0; i < dgvOutpuFields.dataGridView.Columns.Count; i++)
            {
                dgvOutpuFields.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvOutpuFields.dataGridView.Columns[i].ReadOnly = true;
            }
            dgvOutpuFields.dataGridView.Columns[5].ReadOnly = false;
            dgvOutpuFields.dataGridView.CellContentClick += dgvOutpuFields_CellContentClick;

            InitTreeView();
        }

        private void FormControllerDebuger_Load(object sender, EventArgs e)
        {
            _status.IsPaused = true;

            if (_refreshFieldValueThread != null)
            {
                _refreshFieldValueThread.Abort();
                _refreshFieldValueThread.Join();
            }

            _refreshFieldValueThread =
                new Thread(RefreshFieldValue) { IsBackground = true };
            _refreshFieldValueThread.Start();
        }

        private void FormControllerDebuger_Closed(object sender, EventArgs e)
        {
            _status.IsPaused = false;

            if (_refreshFieldValueThread == null)
                return;

            _refreshFieldValueThread.Abort();
            _refreshFieldValueThread.Join();
        }

        private void InitTreeView()
        {
            treeView.Nodes.Clear();

            var treeRootNode = new TreeNode("控制器列表", 1, 1);
            treeView.Nodes.Add(treeRootNode);

            foreach (var controllerBase in _status.LstControllers.OfType<ControllerBase>())
                treeRootNode.Nodes.Add(new TreeNode { Text = controllerBase.Name, ImageIndex = 2, Tag = controllerBase.Name });

            treeView.Nodes[0].BackColor = Color.DarkOrange;

            treeView.ExpandAll();
            treeView.NodeMouseClick += treeView_NodeMouseClick;
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    var pos = new Point(e.Node.Bounds.X + e.Node.Bounds.Width, e.Node.Bounds.Y + e.Node.Bounds.Height / 2);
                    contextMenuStripTree.Show(treeView, pos);
                    break;

                case MouseButtons.Left:
                    if (e.Node.Nodes.Count == 0)//说明当前选中节点没有子节点
                    {
                        lock (_dataGridviewLocker)
                        {
                            for (var i = 0; i < _status.LstControllers.Count; i++)
                            {
                                var c = _status.LstControllers[i] as ControllerBase;
                                if (c == null || c.Name != e.Node.Text)
                                    continue;
                                _selectedControllerIndex = i;
                                break;
                            }

                            if (_selectedControllerIndex < 0)
                                return;

                            dgvInputFields.dataGridView.Rows.Clear();
                            dgvOutpuFields.dataGridView.Rows.Clear();
                            //dgvMethods.dataGridView.Rows.Clear();
                            listMethods.Controls.Clear();

                            var controller =
                                _status.LstControllers[_selectedControllerIndex];
                            if (controller == null)
                                return;

                            #region 字段/Fields

                            var fi = controller.GetType().GetFields();

                            foreach (var fieldInfo in fi)
                            {
                                if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) == null)
                                    continue;

                                var des =
                                    ((DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)))
                                        .Description;

                                var sp = des.Split(',');

                                var name = fieldInfo.Name;
                                var type = fieldInfo.FieldType.Name;
                                var inputOutputType = sp[0];
                                var fieldNote = sp[1];

                                var controllerBase = controller as ControllerBase;
                                var partName = string.Empty;
                                if (controllerBase != null && _status.DeviceConfig != null && _status.DeviceConfig.Parts != null)
                                {
                                    var controllerField = string.Format(@"{0}.Field.{1}", controllerBase.Name, name);
                                    var findPart =
                                        _status.DeviceConfig.Parts.ToList().Find(f => f.ControllerField.Equals(controllerField));
                                    if (findPart != null && !string.IsNullOrEmpty(findPart.ControllerField))
                                        partName = findPart.Name;
                                }


                                if (inputOutputType == "R")
                                {
                                    var row = dgvInputFields.dataGridView.Rows[dgvInputFields.dataGridView.Rows.Add()];

                                    row.Cells[0].Value = name;
                                    row.Cells[1].Value = fieldNote;
                                    row.Cells[2].Value = type;
                                    row.Cells[3].Value = partName;
                                    row.Cells[4].Value = string.Empty;
                                }
                                else if (inputOutputType == "R/W")
                                {
                                    var row = dgvOutpuFields.dataGridView.Rows[dgvOutpuFields.dataGridView.Rows.Add()];

                                    row.Cells[0].Value = name;
                                    row.Cells[1].Value = fieldNote;
                                    row.Cells[2].Value = type;
                                    row.Cells[3].Value = partName;
                                    row.Cells[4].Value = "null";
                                }
                            }

                            #endregion

                            #region 方法/Methods

                            var me = controller.GetType().GetMethods();

                            for (var i = 0; i < me.Length; i++)
                            {
                                var method = me[i];

                                if (Attribute.GetCustomAttribute(method, typeof(DescriptionAttribute)) == null)
                                    continue;

                                var methodNote =
                                   ((DescriptionAttribute)Attribute.GetCustomAttribute(method, typeof(DescriptionAttribute)))
                                       .Description;

                                var name = method.Name;
                                var isNeedPara = method.GetParameters().Any();

                                var newBtn = new UCBtnExt
                                {
                                    BtnText = methodNote,
                                    Name = name,
                                    ConerRadius = 34,
                                    Cursor = Cursors.Hand,
                                    EnabledMouseEffect = true,
                                    BtnFont = new Font("微软雅黑", 8f, FontStyle.Regular, GraphicsUnit.Point, 134),
                                    IsRadius = true,
                                    FillColor = i % 2 == 0 ? Color.CadetBlue : Color.DarkCyan,
                                    RectColor = Color.Gray,
                                    TipsText = name,
                                    Margin = new Padding(3)
                                };
                                newBtn.BtnClick += newBtn_Click;
                                listMethods.Controls.Add(newBtn);
                                listMethods.AutoScroll = true;
                            }

                            #endregion
                        }
                    }
                    break;

                case MouseButtons.None:
                    break;

                case MouseButtons.Middle:
                    break;

                case MouseButtons.XButton1:
                    break;

                case MouseButtons.XButton2:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RefreshFieldValue()
        {
            while (_refreshFieldValueThread.IsAlive)
            {
                Thread.Sleep(50);

                if (!_refreshFieldValueThread.IsAlive)
                    break;

                if (_selectedControllerIndex < 0)
                    continue;

                try
                {
                    lock (_dataGridviewLocker)
                    {
                        var controller =
                            _status.LstControllers[_selectedControllerIndex];
                        if (controller == null)
                            continue;

                        #region 字段/Fields

                        var fi = controller.GetType().GetFields();

                        foreach (var fieldInfo in fi)
                        {
                            if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) == null)
                                continue;

                            var des =
                                ((DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)))
                                    .Description;

                            var sp = des.Split(',');

                            var name = fieldInfo.Name;
                            var type = fieldInfo.FieldType.Name;
                            var inputOutputType = sp[0];
                            var fieldNote = sp[1];

                            if (inputOutputType == "R")
                            {
                                for (var i = 0; i < dgvInputFields.dataGridView.RowCount; i++)
                                {
                                    var row = dgvInputFields.dataGridView.Rows[i];

                                    if (row.Cells[0].Value.ToString() != fieldInfo.Name)
                                        continue;

                                    row.Cells[4].Value = fieldInfo.GetValue(controller) == null
                                        ? string.Empty
                                        : fieldInfo.FieldType == typeof(bool)
                                            ? fieldInfo.GetValue(controller).ToString() == bool.TrueString ? "1" : "0"
                                            : fieldInfo.GetValue(controller).ToString();

                                    break;
                                }
                            }
                            else if (inputOutputType == "R/W")
                            {
                                for (var i = 0; i < dgvOutpuFields.dataGridView.RowCount; i++)
                                {
                                    var row = dgvOutpuFields.dataGridView.Rows[i];

                                    if (row.Cells[0].Value.ToString() != fieldInfo.Name)
                                        continue;

                                    row.Cells[4].Value = fieldInfo.GetValue(controller) == null
                                        ? string.Empty
                                        : fieldInfo.FieldType == typeof(bool)
                                            ? fieldInfo.GetValue(controller).ToString() == bool.TrueString ? "1" : "0"
                                            : fieldInfo.GetValue(controller).ToString();

                                    break;
                                }
                            }
                        }

                        #endregion
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void dgvOutpuFields_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lock (_dataGridviewLocker)
            {
                var dgv = sender as DataGridView;
                if (dgv == null)
                    return;

                if (dgv.Columns[e.ColumnIndex].Name != "设置按钮")
                    return;

                try
                {
                    if (dgv.Rows[e.RowIndex].Cells[5].Value == null)
                        return;

                    var row = dgv.Rows[e.RowIndex];
                    var toSetValue = row.Cells[5].Value.ToString();
                    if (string.IsNullOrEmpty(toSetValue))
                        return;

                    var type = row.Cells[2].Value.ToString();

                    if (type == typeof(bool).Name)
                    {
                        if (toSetValue == 1.ToString())
                            toSetValue = bool.TrueString;
                        else if (toSetValue == 0.ToString())
                            toSetValue = bool.FalseString;
                    }

                    if (_selectedControllerIndex < 0)
                        return;

                    var controller =
                        _status.LstControllers[_selectedControllerIndex];

                    if (controller == null)
                        return;
                    object value = null;

                    var fieldName = row.Cells[0].Value.ToString();
                    var fieldType = controller.GetType().GetField(fieldName).FieldType;

                    if (fieldType == typeof(bool))
                        value = string.Equals(toSetValue, bool.TrueString, StringComparison.CurrentCultureIgnoreCase);
                    else if (fieldType == typeof(ushort))
                        value = Convert.ToUInt16(toSetValue);
                    else if (fieldType == typeof(float))
                        value = Convert.ToSingle(toSetValue);
                    else if (fieldType == typeof(double))
                        value = Convert.ToDouble(toSetValue);
                    else if (fieldType == typeof(int))
                        value = Convert.ToInt32(toSetValue);
                    else if (fieldType == typeof(string))
                        value = toSetValue;

                    controller.GetType()
                        .GetField(fieldName)
                        .SetValue(controller, Convert.ChangeType(value, fieldType));
                }
                catch (Exception)
                {
                    // ignored
                }
                finally
                {
                    dgv.Rows[e.RowIndex].Cells[5].Value = string.Empty;
                }
            }
        }

        private async void newBtn_Click(object sender, EventArgs e)
        {
            Enabled = false;

            await Task.Run(() =>
            {
                var btn = sender as UCBtnExt;
                if (btn == null)
                    return;

                if (_selectedControllerIndex < 0)
                    return;

                var controller =
                    _status.LstControllers[_selectedControllerIndex];

                if (controller == null)
                    return;

                var methodName = btn.Name;

                var method = controller.GetType().GetMethod(methodName);
                if (method == null)
                    return;

                var isNeedPara = method.GetParameters().Any();
                if (!isNeedPara)
                {
                    method.Invoke(controller, null);
                }
                else
                {
                    var paras = method.GetParameters().Select(t => t.Name).ToList();
                    using (var frm = new FrmInputs(@"请输入参数", paras.ToArray()))
                    {
                        if (frm.ShowDialog(this) != DialogResult.OK) return;
                        var inputParas = frm.Values;

                        if (inputParas == null || inputParas.Length != paras.Count)
                            return;

                        try
                        {
                            var lstParaObjs = new List<object>();
                            var lstParas = inputParas.ToList();

                            for (var i = 0; i < lstParas.Count; i++)
                            {
                                var m = method.GetParameters()[i];
                                lstParaObjs.Add(Convert.ChangeType(lstParas[i], m.ParameterType));
                            }

                            method.Invoke(controller, lstParaObjs.ToArray());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            });

            Enabled = true;
        }

        private void dgvMethods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lock (_dataGridviewLocker)
            {
                var dgv = sender as DataGridView;
                if (dgv == null)
                    return;

                if (dgv.Columns[e.ColumnIndex].Name != "执行按钮")
                    return;

                try
                {
                    if (dgv.Rows[e.RowIndex].Cells[2].Value.ToString() != "否" &&
                        dgv.Rows[e.RowIndex].Cells[3].Value == null)
                        return;

                    if (_selectedControllerIndex < 0)
                        return;

                    var controller =
                        _status.LstControllers[_selectedControllerIndex];

                    if (controller == null)
                        return;

                    var row = dgv.Rows[e.RowIndex];

                    var methodName = row.Cells[0].Value.ToString();
                    var strParas = row.Cells[2].Value.ToString() == "否" ? string.Empty : row.Cells[3].Value.ToString();

                    var lstParaObjs = new List<object>();

                    var getAllMethods = controller.GetType().GetMethods();
                    var lstParas = strParas.Split(',').ToList();
                    var paraCount = lstParas.Count;

                    if (paraCount == 1 && lstParas[0].Equals(""))
                        paraCount = 0;
                    else
                    {
                        foreach (
                            var methodPara in
                                from m in getAllMethods
                                where m.Name == methodName && m.GetParameters().Length == paraCount
                                select m.GetParameters())
                            lstParaObjs.AddRange(
                                methodPara.Select((t, i) => Convert.ChangeType(lstParas[i], t.ParameterType)));
                    }

                    var action = getAllMethods.Where(m => m.Name == methodName && m.GetParameters().Length == paraCount)
                        .Select(m1 => (Action)(() => m1.Invoke(controller, lstParaObjs.ToArray())))
                        .FirstOrDefault();

                    if (action != null)
                        action.Invoke();
                }
                catch (Exception)
                {
                    // ignored
                }
                finally
                {
                    dgv.Rows[e.RowIndex].Cells[3].Value =
                        dgv.Rows[e.RowIndex].Cells[2].Value.ToString() != "否"
                        ? string.Empty
                        : "不需要填";
                }
            }
        }
    }
}
