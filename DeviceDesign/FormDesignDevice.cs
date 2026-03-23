using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace DeviceDesign
{
    public partial class FormDesignDevice : FormBase
    {
        private object _deviceConfigSon;
        private int _sortColumnIndex = -1;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

        public FormDesignDevice()
        {
            InitializeComponent();
        }

        public FormDesignDevice(string name)
        {
            InitializeComponent();
            Name = name;
        }


        #region 重载 Onload OnSizeChanged
        protected override void OnLoad(EventArgs e)
        {
            Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte) (134)));


            var asmb = Assembly.LoadFrom("StateMachine.dll");
            var deviceType = "StateMachine." + Name;
            //生成一个对象
            _deviceConfigSon = Activator.CreateInstance(asmb.GetType(deviceType));

            ToolStripInit();
            DataGridInit();
            DataGridLoadData();

            userToolStrip.OnToolStripChanged += UserToolStrip_OnToolStripChanged;
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            dataGridView.ColumnHeaderMouseClick += DataGridView_ColumnHeaderMouseClick;

            base.OnLoad(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }
        #endregion

        #region ToolStrip
        private void ToolStripInit()
        {
            var lstText = _deviceConfigSon.GetType().GetProperties().Select(item => item.Name).ToList();
            userToolStrip.SetComboBoxColValue(lstText);
        }

        private void UserToolStrip_OnToolStripChanged(object sender, EventArgs e)
        {
            var toolstrip = sender as UserControls.UserToolStrip;

            if (toolstrip != null && toolstrip.strButtonName == "添加")
            {
                ShowDialog(_deviceConfigSon, "ADD");
                DataGridLoadData();
            }
            else if (toolstrip != null && toolstrip.strButtonName == "删除")
            {
                if (MessageBox.Show(@"是否删除该记录？", @"更新", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;

                ShowDialog(_deviceConfigSon, "DEL");
                DataGridLoadData();
            }
            else if (toolstrip != null && toolstrip.strButtonName == "修改")
            {
                ShowDialog(_deviceConfigSon, "EDIT");
                DataGridLoadData();
            }
            else if (toolstrip != null && toolstrip.strButtonName == "保存")
                SaveDataGridToXml();
            else if (toolstrip != null && toolstrip.strButtonName == "取消")
                DataGridLoadData();
            else if (toolstrip != null && toolstrip.strButtonName == "刷新")
                DataGridLoadData();
            else if (toolstrip != null && toolstrip.strButtonName == "查询")
                DataGridLoadData(userToolStrip.toolStripComboBoxCol.Text,
                        userToolStrip.toolStripTextBoxSearchContent.Text);
            else if (toolstrip != null && toolstrip.strButtonName == "排序")
                ApplySorting();
            else if (toolstrip != null && toolstrip.strButtonName == "导出")
                MessageBox.Show(@"请选择数据，进行复制（Ctrl+C）然后黏贴（Ctrl+V）在文本或表格中！");
            else if (toolstrip != null && toolstrip.strButtonName == "导入")
                MessageBox.Show(@"导入");

            // DataGridLoadData();
        }

        private static void ShowDialog(object devicepara, string operatemode)
        {
            var frmDialog = new FormDesignNew(devicepara, operatemode);
            frmDialog.ShowDialog();
        }

        private void SaveDataGridToXml()
        {
            if (MessageBox.Show(@"请确认没有筛选,鼠标已离开编辑区等，操作会更新XML文件", @"确认", MessageBoxButtons.OKCancel) ==
                DialogResult.Cancel)
                return;

            var asmb = Assembly.LoadFrom("StateMachine.dll");
            var deviceType = "StateMachine." + Name;
            //生成一个对象

            var typeName = ClassComm.DeviceConfig.GetType();
            var prop = typeName.GetProperties().ToList().Find(t => t.PropertyType.Name.Contains(Name));
            //只有一条记录,而且没有空行
            if (dataGridView.Rows.Count == 1)
            {
                var configItem = Activator.CreateInstance(asmb.GetType(deviceType));
                foreach (var item in configItem.GetType().GetProperties())
                {
                    item.SetValue(configItem, ClassComm.MyChanageType(dataGridView.Rows[0].Cells[item.Name].Value, item.PropertyType));
                }
                prop.SetValue(ClassComm.DeviceConfig, configItem, null);
            }
            else
            {
                //先产生对应类型的数组，然后给数组赋值，则可以完成数据类型对应
                var arr = Array.CreateInstance(asmb.GetType(deviceType), dataGridView.Rows.Count - 1);

                for (var i = 0; i < dataGridView.Rows.Count - 1; i++)
                {
                    var configItem = Activator.CreateInstance(asmb.GetType(deviceType));
                    foreach (var item in configItem.GetType().GetProperties())
                        item.SetValue(configItem,
                            ClassComm.MyChanageType(dataGridView.Rows[i].Cells[item.Name].Value, item.PropertyType));
                    arr.SetValue(configItem, i);
                }
                prop.SetValue(ClassComm.DeviceConfig, arr, null);
            }

            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);
        }

        #endregion

        #region DataGrid排序功能

        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0) return;

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
            if (_sortColumnIndex < 0)
                return;

            var dataSource = dataGridView.DataSource as DataTable;
            if (dataSource != null)
            {
                var columnName = dataSource.Columns[_sortColumnIndex].ColumnName;
                dataSource.DefaultView.Sort = columnName + (_sortDirection == ListSortDirection.Ascending ? " ASC" : " DESC");
            }
            else
            {
                // 对 DataGridView.Rows 进行排序
                var list = dataGridView.Rows.Cast<DataGridViewRow>()
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
                var firstDisplayedRow = dataGridView.FirstDisplayedScrollingRowIndex;

                dataGridView.Rows.Clear();
                foreach (var row in sorted)
                {
                    var index = dataGridView.Rows.Add();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        dataGridView.Rows[index].Cells[i].Value = row.Cells[i].Value;
                    }
                }

                // 恢复滚动位置
                if (firstDisplayedRow >= 0 && firstDisplayedRow < dataGridView.Rows.Count)
                {
                    dataGridView.FirstDisplayedScrollingRowIndex = firstDisplayedRow;
                }
            }
        }

        private void UpdateColumnHeaderSortIndicator(int columnIndex)
        {
            foreach (DataGridViewColumn col in dataGridView.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            if (columnIndex >= 0)
            {
                dataGridView.Columns[columnIndex].SortMode = DataGridViewColumnSortMode.Programmatic;
                dataGridView.Columns[columnIndex].HeaderCell.SortGlyphDirection = _sortDirection == ListSortDirection.Ascending
                    ? SortOrder.Ascending
                    : SortOrder.Descending;
            }
        }

        #endregion

        #region DataGrid功能

        private void DataGridInit()
        {
            dataGridView.AllowUserToAddRows = true;
            dataGridView.AllowUserToDeleteRows = true;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AllowUserToResizeColumns = true;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView.Margin = new Padding(3, 4, 3, 4);
            dataGridView.RowTemplate.Height = 24;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;

            //获取控件的Type,设置双缓存
            var dgvType = dataGridView.GetType();
            var properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //为控件的属性设置值
            if (properInfo != null) properInfo.SetValue(dataGridView, true, null);

            //初始化列
            foreach (var item in _deviceConfigSon.GetType().GetProperties())
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = item.Name });

            //获取当前页面所对应的对象属性名称
            var typeName = ClassComm.DeviceConfig.GetType();
            var prop = typeName.GetProperties().ToList().Find(t => t.PropertyType.Name.Contains(Name));
            //获取配置文件对象中对应对象的数据对象
            var configValue = prop.GetValue(ClassComm.DeviceConfig, null);
            //如果对象时数组
            if (configValue is Array)
                dataGridView.AllowUserToAddRows = true;
            else
            {
                dataGridView.AllowUserToAddRows = false;
                userToolStrip.toolStripButtonAdd.Visible = false;
            }
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //选中header时currentcell为null
            if (dataGridView.CurrentCell == null)
                return;
            //Name值不为空
            if (dataGridView.CurrentRow == null || dataGridView.CurrentRow.Cells[0].Value == null)
                return;

            foreach (
                var item in
                    _deviceConfigSon.GetType()
                        .GetProperties()
                        .Where(item => dataGridView.CurrentRow.Cells[item.Name].Value != null))
            {
                item.SetValue(_deviceConfigSon, dataGridView.CurrentRow.Cells[item.Name].Value.ToString());
            }
        }

        private void DataGridLoadData()
        {
            dataGridView.Rows.Clear();
            //刷新数据
            ClassComm.GetDeviceConfigFromFile(ClassComm.FilePathDeviceConfig);

            //获取当前页面所对应的对象属性名称
            var typeName = ClassComm.DeviceConfig.GetType();
            var prop = typeName.GetProperties().ToList().Find(t => t.PropertyType.Name.Contains(Name));
            //获取配置文件对象中对应对象的数据对象
            var configValue = prop.GetValue(ClassComm.DeviceConfig, null);
            //如果对象时数组
            if (configValue is Array)
            {
                foreach (var item in configValue as Array)
                {
                    var rownum = dataGridView.Rows.Add();
                    foreach (var p in item.GetType().GetProperties())
                        dataGridView.Rows[rownum].Cells[p.Name].Value = p.GetValue(item, null);
                }
            }
            //如果对象不是数组
            else
            {
                var rownum = dataGridView.Rows.Add();
                foreach (var p in configValue.GetType().GetProperties())
                    dataGridView.Rows[rownum].Cells[p.Name].Value = p.GetValue(configValue, null);
            }
        }

        private void DataGridLoadData(string colname, string searchtext)
        {
            dataGridView.Rows.Clear();
            //刷新数据
            ClassComm.GetDeviceConfigFromFile(ClassComm.FilePathDeviceConfig);

            //获取当前页面所对应的对象属性名称
            var typeName = ClassComm.DeviceConfig.GetType();
            var prop = typeName.GetProperties().ToList().Find(t => t.PropertyType.Name.Contains(Name));
            //获取配置文件对象中对应对象的数据对象
            var configValue = prop.GetValue(ClassComm.DeviceConfig, null);

            // 判断是否全字段搜索
            bool searchAllColumns = string.IsNullOrEmpty(colname) ||
                                    colname == "全部" ||
                                    colname == "all";

            //如果对象时数组
            if (configValue is Array)
            {
                foreach (var item in configValue as Array)
                {
                    bool isMatch = false;

                    if (searchAllColumns && !string.IsNullOrEmpty(searchtext))
                    {
                        // 全字段搜索：检查所有列
                        foreach (var p in item.GetType().GetProperties())
                        {
                            var content = p.GetValue(item, null) == null ? string.Empty : p.GetValue(item, null).ToString();
                            if (content.ToLower().Contains(searchtext.ToLower()))
                            {
                                isMatch = true;
                                break;
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(searchtext))
                    {
                        // 单列搜索
                        var pr = item.GetType().GetProperties().ToList().Find(t => t.Name == colname);
                        if (pr != null)
                        {
                            var content = pr.GetValue(item, null) == null ? string.Empty : pr.GetValue(item, null).ToString();
                            isMatch = content.ToLower().Contains(searchtext.ToLower());
                        }
                    }
                    else
                    {
                        isMatch = true; // 无搜索条件时显示所有
                    }

                    if (!isMatch) continue;
                    var rownum = dataGridView.Rows.Add();
                    foreach (var p in item.GetType().GetProperties())
                        dataGridView.Rows[rownum].Cells[p.Name].Value = p.GetValue(item, null);
                }
            }
            //如果对象不是数组
            else
            {
                var rownum = dataGridView.Rows.Add();
                foreach (var p in configValue.GetType().GetProperties())
                    dataGridView.Rows[rownum].Cells[p.Name].Value = p.GetValue(configValue, null);
            }

            // 应用排序（如果有）
            if (_sortColumnIndex >= 0)
            {
                ApplySorting();
            }
        }

        #endregion
    }
}
