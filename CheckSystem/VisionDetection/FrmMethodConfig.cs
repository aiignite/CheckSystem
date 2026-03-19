using CheckSystem.VisionDetection.Vision;
using CommonUtility;
using CommonUtility.BusLoader;
using Sunny.UI;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CheckSystem.VisionDetection
{
    public partial class FrmMethodConfig : UIForm
    {
        private readonly DataTable _dtL = new DataTable();
        private readonly DataTable _dtR = new DataTable();

        public string MethodStr { get; set; }

        public FrmMethodConfig(string methodStr, string customControllerName = "")
        {
            InitializeComponent();
            Style = UIStyle.LayuiGreen;
            //WindowState = FormWindowState.Maximized;

            uiDataGridView1.Style = UIStyle.Gray;
            uiDataGridView1.ReadOnly = false;
            uiDataGridView1.RowHeadersVisible = false;
            uiDataGridView1.AllowUserToAddRows = true;
            uiDataGridView1.AllowUserToResizeRows = false;
            uiDataGridView1.AllowUserToDeleteRows = true;
            uiDataGridView1.MultiSelect = false;
            uiDataGridView1.RowHeadersVisible = true;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            uiDataGridView1.AddColumn("Content", "Content", readOnly: false);

            foreach (var t in methodStr.Trim().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                uiDataGridView1.AddRow(t.Trim() + ";");
            }

            if (VisionCommon.VisionConfig.DeviceInfo.CustomContrller == null)
                VisionCommon.VisionConfig.DeviceInfo.CustomContrller = new VisionConfigDeviceInfoCustomContrller[0];

            InitL(customControllerName);
            InitR();
            textBox1.TextChanged += textBox1_TextChanged;
        }

        private void InitL(string customControllerName)
        {
            _dtL.Columns.Add("Name", typeof(string));

            {
                var dirPath = Program.SysDir;
                var controllerPath = dirPath + @"\Controller.dll";
                var asmb = Assembly.LoadFrom(controllerPath);
                var type = asmb.GetType("Controller." + "CheckApp");

                var fields =
                            type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                .ToList();
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                .ToList();

                //var controller = customController;
                foreach (var method in methods)
                {
                    var para = method.GetParameters().ToList();
                    var strPara = string.Join(",", para);

                    _dtL.Rows.Add("CheckApp" + ".Method." + method.Name + "(" + strPara + ")");
                }

                foreach (var fv in fields.Select(f => string.Format("{0}.Field.{1}", "CheckApp", f.Name)))
                    _dtL.Rows.Add(fv);
            }

            foreach (var customController in VisionCommon.VisionConfig.DeviceInfo.CustomContrller)
            {
                if (!string.IsNullOrEmpty(customControllerName))
                {
                    if (customController.Name != customControllerName)
                    {
                        continue;
                    }
                }

                var dirPath = Program.SysDir;
                var controllerPath = dirPath + @"\Controller.dll";
                var asmb = Assembly.LoadFrom(controllerPath);
                var type = asmb.GetType("Controller." + customController.Type);

                var fields =
                            type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                .ToList();
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                .ToList();

                var controller = customController;
                foreach (var method in methods)
                {
                    var para = method.GetParameters().ToList();
                    var strPara = string.Join(",", para);

                    _dtL.Rows.Add(controller.Name + ".Method." + method.Name + "(" + strPara + ")");
                }

                foreach (var fv in fields.Select(f => string.Format("{0}.Field.{1}", controller.Name, f.Name)))
                    _dtL.Rows.Add(fv);
            }

            foreach (var customController in VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController)
            {
                if (!string.IsNullOrEmpty(customControllerName))
                {
                    if (customController.Name != customControllerName)
                    {
                        continue;
                    }
                }

                var dirPath = Program.SysDir;
                var controllerPath = dirPath + @"\Controller.dll";
                var asmb = Assembly.LoadFrom(controllerPath);
                var type = asmb.GetType("Controller." + customController.Type);

                var fields =
                            type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                .ToList();
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                .ToList();

                var controller = customController;
                foreach (var method in methods)
                {
                    var para = method.GetParameters().ToList();
                    var strPara = string.Join(",", para);

                    _dtL.Rows.Add(controller.Name + ".Method." + method.Name + "(" + strPara + ")");
                }

                foreach (var fv in fields.Select(f => string.Format("{0}.Field.{1}", controller.Name, f.Name)))
                    _dtL.Rows.Add(fv);
            }

            cmbL.DataGridView.Init();
            cmbL.ItemSize = new System.Drawing.Size(360, 240);
            cmbL.DataGridView.AddColumn("Name", "Name");
            cmbL.DataGridView.ReadOnly = true;
            cmbL.ValueChanged += cmbL_ValueChanged;
            cmbL.SelectIndexChange += cmbL_SelectIndexChange;
            cmbL.ShowFilter = true;
            cmbL.DataGridView.DataSource = _dtL;
            cmbL.FilterColumnName = "Name"; //不设置则全部列过滤
        }

        private void InitR()
        {
            _dtR.Columns.Add("Name", typeof(string));

            foreach (var c in VisionCommon.IoControllers)
            {
                var fields =
                    c.Value.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .ToList().FindAll(f =>
                            f.FieldType == typeof(CanBus) || f.FieldType == typeof(LinBus) ||
                            f.FieldType == typeof(MySerialPort));
                foreach (var fv in fields.Select(f => string.Format("{0}.Field.{1}", c.Key, f.Name)))
                    _dtR.Rows.Add(fv);
            }

            foreach (var c in VisionCommon.CustomControllers)
            {
                var fields =
                    c.Value.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .ToList().FindAll(f =>
                            f.FieldType == typeof(CanBus) || f.FieldType == typeof(LinBus) ||
                            f.FieldType == typeof(MySerialPort));
                foreach (var fv in fields.Select(f => string.Format("{0}.Field.{1}", c.Key, f.Name)))
                    _dtR.Rows.Add(fv);
            }

            foreach (var c in VisionCommon.CommunicationControllers)
            {
                var fields =
                    c.Value.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .ToList().FindAll(f =>
                            f.FieldType == typeof(CanBus) || f.FieldType == typeof(LinBus) ||
                            f.FieldType == typeof(MySerialPort));
                foreach (var fv in fields.Select(f => string.Format("{0}.Field.{1}", c.Key, f.Name)))
                    _dtR.Rows.Add(fv);
            }

            cmbR.DataGridView.Init();
            cmbR.ItemSize = new System.Drawing.Size(360, 240);
            cmbR.DataGridView.AddColumn("Name", "Name");
            cmbR.DataGridView.ReadOnly = true;
            cmbR.ValueChanged += cmbR_ValueChanged;
            cmbR.SelectIndexChange += cmbR_SelectIndexChange;
            cmbR.ShowFilter = true;
            cmbR.DataGridView.DataSource = _dtR;
            cmbR.FilterColumnName = "Name"; //不设置则全部列过滤
        }

        private void cmbL_ValueChanged(object sender, object value)
        {
            cmbL.Text = string.Empty;
            var viewRow = value as DataGridViewRow;
            if (viewRow == null) return;
            var row = viewRow;
            cmbL.Text = row.Cells["Name"].Value.ToString();
        }

        private void cmbL_SelectIndexChange(object sender, int index)
        {
            cmbL.Text = _dtL.Rows[index]["Name"].ToString();
        }

        private void cmbR_ValueChanged(object sender, object value)
        {
            textBox1.Text = string.Empty;
            cmbR.Text = string.Empty;
            var viewRow = value as DataGridViewRow;
            if (viewRow == null) return;
            var row = viewRow;
            cmbR.Text = row.Cells["Name"].Value.ToString();
        }

        private void cmbR_SelectIndexChange(object sender, int index)
        {
            cmbR.Text = _dtR.Rows[index]["Name"].ToString();
        }


        void textBox1_TextChanged(object sender, EventArgs e)
        {
            cmbR.Text = string.Empty;
        }

        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbL.Text))
            {
                string code;

                var s = cmbL.Text;
                if (s.Contains(".Method."))
                {
                    code = s.Substring(0, s.IndexOf("(", StringComparison.Ordinal));

                    if (!string.IsNullOrEmpty(cmbR.Text))
                    {
                        code += "(" + cmbR.Text + ")";
                    }
                    else if (!string.IsNullOrEmpty(textBox1.Text))
                    {
                        code += "(" + textBox1.Text + ")";
                    }
                    else
                    {
                        code += "()";
                    }
                }
                else
                {
                    code = s;

                    if (!string.IsNullOrEmpty(cmbR.Text))
                    {
                        code += "=" + cmbR.Text;
                    }
                    else if (!string.IsNullOrEmpty(textBox1.Text))
                    {
                        code += "=" + textBox1.Text;
                    }
                }

                int rowNum;
                if (uiDataGridView1.SelectedRows.Count > 0)
                {
                    rowNum = uiDataGridView1.SelectedRows[0].Index;
                    uiDataGridView1.Rows.Insert(rowNum);
                }
                else
                {
                    rowNum = uiDataGridView1.Rows.Add();
                }

                uiDataGridView1.Rows[rowNum].Cells[0].Value = code + ";";
            }
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            uiDataGridView1.EndEdit();

            if (uiDataGridView1.RowCount > 1)
            {
                if (this.ShowAskDialog("确定？"))
                {
                    for (var i = 0; i < uiDataGridView1.RowCount - 1; i++)
                    {
                        var temp = uiDataGridView1.Rows[i].Cells[0].Value.ToString();
                        if (temp.Substring(temp.Length - 1, 1) != ";")
                        {
                            temp += ";";
                        }
                        MethodStr += temp + "\n";
                    }

                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
