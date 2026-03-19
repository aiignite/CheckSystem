using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility.BusLoader;
using Controller;
using HZH_Controls.Controls.Btn;
using HZH_Controls.Forms;
using HZH_Controls.IconFont;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.LIN
{
    public partial class LinDataProductForm : UIForm
    {
        public LinBus Lin { get; set; }
        private Thread RefreshFieldValueThread { get; set; }
        private object ThisController { get; set; }
        private readonly List<string> _productList = new List<string>();
        private readonly object _dataGridviewLocker = new object();

        public LinDataProductForm(LinBus lin)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);
            CheckForIllegalCrossThreadCalls = false;
            WindowState = FormWindowState.Maximized;
            Text = @"请选择产品";
            InitDataGridView();
            InitProduct();
            Lin = lin;
            Closed += LinDataProductFrom_Closed;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void LinDataProductFrom_Closed(object sender, EventArgs e)
        {
            if (RefreshFieldValueThread != null)
            {
                RefreshFieldValueThread.Abort();
                RefreshFieldValueThread.Join();
            }

            if (ThisController != null)
                ((ControllerBase)ThisController).Dispose();

            GC.Collect();
        }

        private void InitDataGridView()
        {
            dgvInputFields.label.Height = 30;
            dgvInputFields.label.Text = @"字段列表-R";
            dgvInputFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段名称" });
            dgvInputFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段备注" });
            dgvInputFields.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段类型" });
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
            dgvOutpuFields.dataGridView.Columns[4].ReadOnly = false;

            //dgvMethods.label.Height = 30;
            //dgvMethods.label.Text = @"方法列表";
            //dgvMethods.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "方法名称" });
            //dgvMethods.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "方法备注" });
            //dgvMethods.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "是否需要填入参数" });
            //dgvMethods.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "参数" });
            //dgvMethods.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "执行按钮", DefaultCellStyle = { NullValue = "执行" } });
            //dgvMethods.dataGridView.RowHeadersVisible = false;
            //dgvMethods.dataGridView.AllowUserToAddRows = false;
            //dgvMethods.dataGridView.AllowUserToDeleteRows = false;
            //dgvMethods.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dgvMethods.dataGridView.AllowUserToResizeColumns = true;
            //dgvMethods.dataGridView.AllowUserToResizeRows = false;
            //for (var i = 0; i < dgvMethods.dataGridView.Columns.Count; i++)
            //{
            //    dgvMethods.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //    dgvMethods.dataGridView.Columns[i].ReadOnly = true;
            //}
            //dgvMethods.dataGridView.Columns[3].ReadOnly = false;
        }

        private void InitProduct()
        {
            cmbProductList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProductList.comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            cmbLinList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            var assmb = Assembly.LoadFrom("Controller.dll");

            foreach (var item in assmb.GetTypes())
            {
                if (item.FullName == null || !item.FullName.StartsWith("Controller."))
                    continue;

                if (Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) == null)
                    continue;

                var des =
                    ((DescriptionAttribute)Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)))
                        .Description;

                if (!des.StartsWith("LIN-Product,"))
                    continue;

                var sp = des.Split(',');
                _productList.Add(item.Name);
                cmbProductList.comboBox.Items.Add(sp[1]);
            }

            if (cmbProductList.comboBox.Items.Count > 0)
                cmbProductList.comboBox.SelectedIndex = 0;
            titlePanel.BackColor = Color.DarkGoldenrod;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbLinList.comboBox.Items.Clear();

            if (string.IsNullOrEmpty(cmbProductList.comboBox.Text))
                return;

            var index = cmbProductList.comboBox.SelectedIndex;
            var controllerName = _productList[index];

            foreach (var f in Assembly.LoadFrom("Controller.dll")
                .GetType("Controller." + controllerName)
                .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(f => f.FieldType == typeof(LinBus)))
                cmbLinList.comboBox.Items.Add(f.Name);

            if (cmbLinList.comboBox.Items.Count > 0)
                cmbLinList.comboBox.SelectedIndex = 0;
        }

        private void ucBtnExt1_BtnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbProductList.comboBox.Text))
                return;

            if (string.IsNullOrEmpty(cmbLinList.comboBox.Text))
                return;

            var index = cmbProductList.comboBox.SelectedIndex;
            var controllerName = _productList[index];

            var asmb = Assembly.LoadFrom("Controller.dll");
            var controllerType = asmb.GetType("Controller." + controllerName);

            var controller = Activator.CreateInstance(controllerType, controllerName);
            var fieldCan = controller.GetType().GetField(cmbLinList.comboBox.Text);
            if (fieldCan == null || Lin == null)
                return;

            ThisController = controller;
            Text = cmbProductList.comboBox.Text;

            fieldCan.SetValue(controller, Lin);
            titlePanel.BackColor = Color.Green;
            titlePanel.Enabled = false;

            dgvInputFields.dataGridView.Rows.Clear();
            dgvOutpuFields.dataGridView.Rows.Clear();
            //dgvMethods.dataGridView.Rows.Clear();

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
                    var row = dgvInputFields.dataGridView.Rows[dgvInputFields.dataGridView.Rows.Add()];

                    row.Cells[0].Value = name;
                    row.Cells[1].Value = fieldNote;
                    row.Cells[2].Value = type;
                    row.Cells[3].Value = string.Empty;
                }
                else if (inputOutputType == "R/W")
                {
                    var row = dgvOutpuFields.dataGridView.Rows[dgvOutpuFields.dataGridView.Rows.Add()];

                    row.Cells[0].Value = name;
                    row.Cells[1].Value = fieldNote;
                    row.Cells[2].Value = type;
                    row.Cells[3].Value = "null";
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

            dgvOutpuFields.dataGridView.CellContentClick += dgvOutpuFields_CellContentClick;
            //dgvMethods.dataGridView.CellContentClick += dgvMethods_CellContentClick; ;


            if (RefreshFieldValueThread != null)
            {
                RefreshFieldValueThread.Abort();
                RefreshFieldValueThread.Join();
            }

            RefreshFieldValueThread =
                new Thread(RefreshFieldValue) { IsBackground = true };
            RefreshFieldValueThread.Start();
        }

        private void RefreshFieldValue()
        {
            while (RefreshFieldValueThread.IsAlive)
            {
                Thread.Sleep(50);

                if (!RefreshFieldValueThread.IsAlive)
                    break;

                try
                {
                    lock (_dataGridviewLocker)
                    {
                        var controller = ThisController;
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
                            var type = fieldInfo.FieldType.ToString();
                            var inputOutputType = sp[0];
                            var fieldNote = sp[1];

                            if (inputOutputType == "R")
                            {
                                for (var i = 0; i < dgvInputFields.dataGridView.RowCount; i++)
                                {
                                    var row = dgvInputFields.dataGridView.Rows[i];

                                    if (row.Cells[0].Value.ToString() != fieldInfo.Name)
                                        continue;

                                    var fieldValue = fieldInfo.GetValue(controller) == null
                                        ? string.Empty
                                        : fieldInfo.FieldType == typeof(bool)
                                            ? fieldInfo.GetValue(controller).ToString() == bool.TrueString ? "1" : "0"
                                            : fieldInfo.GetValue(controller).ToString();

                                    if (row.Cells[3].Value.ToString() != fieldValue)
                                    {
                                        row.Cells[3].Value = fieldValue;
                                    }

                                    //row.Cells[3].Value = fieldInfo.GetValue(controller) == null
                                    //    ? string.Empty
                                    //    : fieldInfo.FieldType == typeof(bool)
                                    //        ? fieldInfo.GetValue(controller).ToString() == bool.TrueString ? "1" : "0"
                                    //        : fieldInfo.GetValue(controller).ToString();

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

                                    var fieldValue = fieldInfo.GetValue(controller) == null
                                        ? string.Empty
                                        : fieldInfo.FieldType == typeof(bool)
                                            ? fieldInfo.GetValue(controller).ToString() == bool.TrueString ? "1" : "0"
                                            : fieldInfo.GetValue(controller).ToString();

                                    if (row.Cells[3].Value.ToString() != fieldValue)
                                    {
                                        row.Cells[3].Value = fieldValue;
                                    }

                                    //row.Cells[3].Value = fieldInfo.GetValue(controller) == null
                                    //    ? string.Empty
                                    //    : fieldInfo.FieldType == typeof(bool)
                                    //        ? fieldInfo.GetValue(controller).ToString() == bool.TrueString ? "1" : "0"
                                    //        : fieldInfo.GetValue(controller).ToString();

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
                    if (dgv.Rows[e.RowIndex].Cells[4].Value == null)
                        return;

                    var row = dgv.Rows[e.RowIndex];
                    var toSetValue = row.Cells[4].Value.ToString();
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

                    var controller = ThisController;

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
                    else if (fieldType == typeof(byte))
                        value = Convert.ToByte(toSetValue);
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
                    dgv.Rows[e.RowIndex].Cells[4].Value = string.Empty;
                }
            }
        }

        private async void newBtn_Click(object sender, EventArgs e)
        {
            dgvInputFields.Enabled = false;
            dgvOutpuFields.Enabled = false;
            listMethods.Enabled = false;

            await Task.Run(() =>
            {
                var btn = sender as UCBtnExt;
                if (btn == null)
                    return;

                var methodName = btn.Name;

                var method = ThisController.GetType().GetMethod(methodName);
                if (method == null)
                    return;

                var isNeedPara = method.GetParameters().Any();
                if (!isNeedPara)
                {
                    method.Invoke(ThisController, null);
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

                            method.Invoke(ThisController, lstParaObjs.ToArray());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            });

            dgvInputFields.Enabled = true;
            dgvOutpuFields.Enabled = true;
            listMethods.Enabled = true;
        }

        private async void dgvMethods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //dgvMethods.Enabled = false;

            //await Task.Run(() =>
            //{
            //    if (InvokeRequired)
            //    {
            //        Invoke(new Action(() =>
            //        {
            //            lock (_dataGridviewLocker)
            //            {
            //                var dgv = sender as DataGridView;
            //                if (dgv == null)
            //                    return;

            //                if (dgv.Columns[e.ColumnIndex].Name != "执行按钮")
            //                    return;

            //                try
            //                {
            //                    if (dgv.Rows[e.RowIndex].Cells[2].Value.ToString() != "否" &&
            //                        dgv.Rows[e.RowIndex].Cells[3].Value == null)
            //                        return;

            //                    var controller = ThisController;

            //                    if (controller == null)
            //                        return;

            //                    var row = dgv.Rows[e.RowIndex];

            //                    var methodName = row.Cells[0].Value.ToString();
            //                    var strParas = row.Cells[2].Value.ToString() == "否"
            //                        ? string.Empty
            //                        : row.Cells[3].Value.ToString();

            //                    var lstParaObjs = new List<object>();

            //                    var getAllMethods = controller.GetType().GetMethods();
            //                    var lstParas = strParas.Split(',').ToList();
            //                    var paraCount = lstParas.Count;

            //                    if (paraCount == 1 && lstParas[0].Equals(""))
            //                        paraCount = 0;
            //                    else
            //                    {
            //                        foreach (
            //                            var methodPara in
            //                                from m in getAllMethods
            //                                where m.Name == methodName && m.GetParameters().Length == paraCount
            //                                select m.GetParameters())
            //                            lstParaObjs.AddRange(
            //                                methodPara.Select((t, i) => Convert.ChangeType(lstParas[i], t.ParameterType)));
            //                    }

            //                    var action =
            //                        getAllMethods.Where(
            //                            m => m.Name == methodName && m.GetParameters().Length == paraCount)
            //                            .Select(m1 => (Action)(() => m1.Invoke(controller, lstParaObjs.ToArray())))
            //                            .FirstOrDefault();

            //                    if (action != null)
            //                        action.Invoke();
            //                }
            //                catch (Exception)
            //                {
            //                    // ignored
            //                }
            //                finally
            //                {
            //                    dgv.Rows[e.RowIndex].Cells[3].Value =
            //                        dgv.Rows[e.RowIndex].Cells[2].Value.ToString() != "否"
            //                            ? string.Empty
            //                            : "不需要填";
            //                }
            //            }
            //        }));
            //    }
            //});

            //dgvMethods.Enabled = true;
        }
    }
}
