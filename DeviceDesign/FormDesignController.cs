using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
        private ToolStripTextBox ToolStripTextBoxSearch { get; set; }
        private ToolStripLabel ToolStripLabelSearch { get; set; }
        private int _sortColumnIndex = -1;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

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

            // 订阅控制器名称变更事件
            ClassComm.ControllerNameChanged += ClassComm_ControllerNameChanged;

            base.OnLoad(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // 取消订阅事件
            ClassComm.ControllerNameChanged -= ClassComm_ControllerNameChanged;
            base.OnFormClosed(e);
        }

        private void ClassComm_ControllerNameChanged(object sender, ClassComm.ControllerNameChangedEventArgs e)
        {
            // 只处理当前控制器的名称变更
            if (e.OldControllerName == Text)
            {
                // 更新窗体标题和内部数据
                this.Invoke(new Action(() =>
                {
                    Text = e.NewControllerName;

                    // 更新字段全称列（列0）中的控制器名称前缀
                    var dgv = UserDataGrid.dataGridView;
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.Cells[0].Value != null)
                        {
                            var oldFieldFullName = row.Cells[0].Value.ToString();
                            if (oldFieldFullName.StartsWith(e.OldControllerName + ".Field."))
                            {
                                row.Cells[0].Value = e.NewControllerName + ".Field." +
                                    oldFieldFullName.Substring((e.OldControllerName + ".Field.").Length);
                            }
                        }
                    }
                }));
            }
        }

        #endregion

        #region DataGrid排序功能

        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0) return;

            var dgv = UserDataGrid.dataGridView;

            // 如果点击同一列，切换排序方向
            if (_sortColumnIndex == e.ColumnIndex)
            {
                _sortDirection = _sortDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }
            else
            {
                _sortColumnIndex = e.ColumnIndex;
                _sortDirection = ListSortDirection.Ascending;
            }

            ApplySorting();
            UpdateColumnHeaderSortIndicator(e.ColumnIndex);
        }

        private void ApplySorting()
        {
            if (_sortColumnIndex < 0) return;

            var dgv = UserDataGrid.dataGridView;
            var list = dgv.Rows.Cast<DataGridViewRow>()
                .Where(row => !row.IsNewRow)
                .ToList();

            IEnumerable<DataGridViewRow> sorted = list.OrderBy(row =>
            {
                var value = row.Cells[_sortColumnIndex].Value;
                return value?.ToString() ?? string.Empty;
            });

            if (_sortDirection == ListSortDirection.Descending)
                sorted = sorted.Reverse();

            // 保存当前滚动位置
            var firstDisplayedRow = dgv.FirstDisplayedScrollingRowIndex;

            dgv.Rows.Clear();
            foreach (var row in sorted)
            {
                var index = dgv.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dgv.Rows[index].Cells[i].Value = row.Cells[i].Value;
                }
            }

            // 恢复滚动位置
            if (firstDisplayedRow >= 0 && firstDisplayedRow < dgv.Rows.Count)
            {
                dgv.FirstDisplayedScrollingRowIndex = firstDisplayedRow;
            }
        }

        private void UpdateColumnHeaderSortIndicator(int columnIndex)
        {
            var dgv = UserDataGrid.dataGridView;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            if (columnIndex >= 0)
            {
                dgv.Columns[columnIndex].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns[columnIndex].HeaderCell.SortGlyphDirection = _sortDirection == ListSortDirection.Ascending
                    ? SortOrder.Ascending
                    : SortOrder.Descending;
            }
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

            ToolStripLabelSearch = new ToolStripLabel
            {
                Text = "搜索:",
                Size = new Size(50, 36),
                Margin = new Padding(10, 0, 0, 0)
            };

            ToolStripTextBoxSearch = new ToolStripTextBox
            {
                Name = "toolStripTextBoxSearch",
                Size = new Size(150, 36),
                Margin = new Padding(0, 0, 10, 0)
            };
            ToolStripTextBoxSearch.KeyDown += ToolStripTextBoxSearch_KeyDown;

            var toolStripButtonSearch = new ToolStripButton
            {
                Name = "toolStripButtonSearch",
                Size = new Size(60, 36),
                Text = @"搜索"
            };
            toolStripButtonSearch.Click += ToolStripButtonSearch_Click;

            //
            // toolStrip
            //
            ToolStrip.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            ToolStrip.ImageScalingSize = new Size(32, 32);
            ToolStrip.Items.AddRange(new ToolStripItem[] {
                toolStripButtonSave,
                ToolStripLabelSearch,
                ToolStripTextBoxSearch,
                toolStripButtonSearch
            });
            ToolStrip.Location = new Point(0, 0);
            ToolStrip.Name = "toolStrip";

            Controls.Add(ToolStrip);
        }

        private void ToolStripTextBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
                e.SuppressKeyPress = true;
            }
        }

        private void ToolStripButtonSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            var searchText = ToolStripTextBoxSearch.Text;
            var dgv = UserDataGrid.dataGridView;

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Selected = false;
            }

            if (string.IsNullOrEmpty(searchText))
            {
                return;
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {
                bool match = false;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText.ToLower()))
                    {
                        match = true;
                        break;
                    }
                }
                if (match)
                {
                    row.Selected = true;
                    if (dgv.FirstDisplayedScrollingRowIndex < 0 ||
                        row.Index < dgv.FirstDisplayedScrollingRowIndex ||
                        row.Index > dgv.FirstDisplayedScrollingRowIndex + dgv.DisplayedRowCount(true))
                    {
                        dgv.FirstDisplayedScrollingRowIndex = row.Index;
                    }
                }
            }
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

            // 添加排序事件
            dgv.dataGridView.ColumnHeaderMouseClick += DataGridView_ColumnHeaderMouseClick;
            foreach (DataGridViewColumn column in dgv.dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Programmatic;
            }

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
