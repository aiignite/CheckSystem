using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Sunny.UI;

namespace UserControls
{
    public partial class UserDataGrid : UserControl
    {
        public UserDataGrid()
        {
            InitializeComponent();
            InitDataGridStyle();
        }
        private void InitDataGridStyle()
        {
            Dock = DockStyle.Top;

            dataGridView.Style = UIStyle.Gray;
            dataGridView.AllowUserToAddRows = true;
            dataGridView.AllowUserToDeleteRows = true;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AllowUserToResizeColumns = true;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.EditMode = DataGridViewEditMode.EditOnKeystroke;
            //dataGridView.Margin = new Padding(3, 4, 3, 4);
            dataGridView.RowTemplate.Height = 30;
            dataGridView.ColumnHeadersHeight = 30;
            //dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 12, FontStyle.Regular);
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.RowHeadersVisible = true;
            //dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            //dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            //dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            //dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;

            //获取控件的Type,设置双缓存
            Type dgvType = dataGridView.GetType();
            PropertyInfo properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //为控件的属性设置值
            if (properInfo != null) properInfo.SetValue(dataGridView, true, null);

            //var col0 = new DataGridViewTextBoxColumn {Name = "参数名"};
            //var col1 = new DataGridViewTextBoxColumn {Name = "范围"};
            //var col2 = new DataGridViewTextBoxColumn {Name = "数值"};
            //var col3 = new DataGridViewTextBoxColumn {Name = "结果"};

            //dataGridView.Columns.Add(col0);
            //dataGridView.Columns.Add(col1);
            //dataGridView.Columns.Add(col2);
            //dataGridView.Columns.Add(col3);
        }
    }
}
