using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DeviceDesign.Properties;
using StateMachine;
using UserControls;

namespace DeviceDesign
{
    public sealed partial class FormDesignController : FormBase
    {
        private ToolStrip ToolStrip { get; set; }
        private UserDataGrid UserDataGrid { get; set; }
        public object ObjSelected { get; set; }

        public FormDesignController()
        {
            InitializeComponent();
        }

        public FormDesignController(string name, string controllerName)
        {
            InitializeComponent();
            Name = name;
            Text = controllerName;
        }

        #region 重载 Onload OnSizeChanged
        protected override void OnLoad(EventArgs e)
        {
            DataGridInit();
            ToolStripInit();

            //ToolStripInit();
            //DataGridInit();
            //DataGridLoadData();

            //this.userToolStrip.OnToolStripChanged += UserToolStrip_OnToolStripChanged;
            //this.dataGridView.SelectionChanged += DataGridView_SelectionChanged;

            base.OnLoad(e);
        }
        #endregion

        #region ToolStrip

        private void ToolStripInit()
        {
            ToolStrip = new ToolStrip
            {
                Dock = DockStyle.Top,
                Size = new Size(500, 40),
                BackColor = Color.LightBlue
            };

            var toolStripButtonSave = new ToolStripButton
            {
                Image = Resources.Save,
                ImageTransparentColor = Color.Magenta,
                Name = "toolStripButtonSave",
                Size = new Size(78, 36),
                Text = @"保存"
            };
            toolStripButtonSave.Click += ToolStripButtonSave_Click;

            // 
            // toolStrip
            // 
            ToolStrip.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            ToolStrip.ImageScalingSize = new Size(32, 32);
            ToolStrip.Items.AddRange(new ToolStripItem[] { toolStripButtonSave });
            ToolStrip.Location = new Point(0, 0);
            ToolStrip.Name = "toolStrip";

            Controls.Add(ToolStrip);
        }

        private void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveDataGridToXml();
        }

        private void SaveDataGridToXml()
        {
            if (MessageBox.Show(@"请确认没有筛选,鼠标已离开编辑区等，操作会更新XML文件", @"确认", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            var current = new List<DeviceConfigPart>();
            if (ClassComm.DeviceConfig.Parts != null)
                current.AddRange(ClassComm.DeviceConfig.Parts);

            var dgv = UserDataGrid.dataGridView;
            for (var i = 0; i < dgv.RowCount; i++)
            {
                var row = dgv.Rows[i];
                var controllerField = row.Cells[0].Value.ToString();
                if (dgv.Rows[i].Cells[3].Value == null ||
                    string.IsNullOrEmpty(dgv.Rows[i].Cells[3].Value.ToString()))
                    continue;

                var partName = dgv.Rows[i].Cells[3].Value.ToString();
                var dataType = dgv.Rows[i].Cells[2].Value.ToString();

                var findByControllerField =
                    current.FindIndex(f => f.ControllerField == controllerField);

                var findByPartName = current.FindIndex(f => f.Name == partName);

                if (findByControllerField != -1 && findByPartName == -1)
                    current[findByControllerField].Name = partName;

                else if (findByControllerField == -1 && findByPartName != -1)
                    current[findByPartName].ControllerField = controllerField;

                else if (findByControllerField == -1 && findByPartName == -1)
                {
                    current.Add(new DeviceConfigPart
                    {
                        ControllerField = controllerField,
                        ControllerName = Text,
                        DataType = dataType,
                        Name = partName,
                        ProcessNo = ClassComm.DeviceConfig.DeviceInfo.DeviceNo
                    });
                }
            }

            ClassComm.DeviceConfig.Parts = current.ToArray();
            ClassComm.SaveDeviceConfigToFile(
                ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);
        }

        #endregion

        private void DataGridInit()
        {
            Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);

            var asmb = Assembly.LoadFrom("Controller.dll");
            var controllerType = Name.Substring("Controller.".Length);

            UserDataGrid = new UserDataGrid
            {
                label = { Text = Name },
                Dock = DockStyle.Fill
            };

            var dgv = UserDataGrid;

            dgv.dataGridView.Columns.Clear();
            dgv.dataGridView.AllowUserToAddRows = false;

            dgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段全称" });
            dgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段注释" });
            dgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "类型" });
            dgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "名称" });
            //dgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "参数" });
            //dgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "说明" });
            dgv.dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;

            for (var i = 0; i < dgv.dataGridView.ColumnCount; i++)
                dgv.dataGridView.Columns[i].ReadOnly = true;
            dgv.dataGridView.Columns[3].ReadOnly = false;

            Controls.Add(UserDataGrid);

            if (asmb.GetType(Name) != null)
            {
                var typeName = asmb.GetType(Name);

                if (typeName != null)
                {
                    var fields = typeName.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var field in fields)
                    {
                        var numrow = dgv.dataGridView.Rows.Add();
                        dgv.dataGridView.Rows[numrow].Cells[0].Value = Text + ".Field." +
                            field.ToString().Substring(field.ToString().IndexOf(" ", StringComparison.Ordinal) + 1);

                        if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) != null)
                        {
                            var des =
                                    ((DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)))
                                        .Description;
                            dgv.dataGridView.Rows[numrow].Cells[1].Value = des;
                        }

                        dgv.dataGridView.Rows[numrow].Cells[2].Value = field.FieldType.Name;
                        //dgv.dataGridView.Rows[numrow].Cells[2].Value = field.ToString().Substring(field.ToString().IndexOf(" ", StringComparison.Ordinal) + 1);

                        if (ClassComm.DeviceConfig.Paras == null)
                            continue;
                        foreach (var p in ClassComm.DeviceConfig.Parts)
                        {
                            try
                            {
                                var strings = p.ControllerField.Split(new[] { ".Field." }, StringSplitOptions.RemoveEmptyEntries);
                                if (strings[0] == Text && strings[1] == field.ToString().Substring(field.ToString().IndexOf(" ", StringComparison.Ordinal) + 1))
                                {
                                    dgv.dataGridView.Rows[numrow].Cells[3].Value = p.Name;
                                }
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                        }
                    }

                    //var methods = typeName.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    //foreach (var method in methods)
                    //{
                    //    if (method.IsSpecialName) continue;
                    //    var para = method.GetParameters().ToList();
                    //    var strPara = string.Join(",", para);

                    //    var numrow = dgv.dataGridView.Rows.Add();
                    //    dgv.dataGridView.Rows[numrow].Cells[0].Value = controllerType + ".Method." + method.Name + "(" + strPara + ")";
                    //    if (method.ReturnParameter != null)
                    //        dgv.dataGridView.Rows[numrow].Cells[1].Value = method.ReturnParameter.ToString();
                    //    dgv.dataGridView.Rows[numrow].Cells[2].Value = method.Name;
                    //    dgv.dataGridView.Rows[numrow].Cells[3].Value = strPara;

                    //}
                }
            }
            else
            {
                MessageBox.Show(@"Controller.dll中未发现" + controllerType);
            }
        }
    }
}
