using System;
using Controller;
using Sunny.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZH_Controls.Controls;

namespace CheckSystem
{
    public partial class FrmDataGridView : UIForm
    {
        private Cx1EEmc Cx1EEmcDut = new Cx1EEmc("");
        private readonly Dictionary<string, string> _stateBinding = new Dictionary<string, string>();
        BindingList<DictionaryItem<string, string>> bindingList = new BindingList<DictionaryItem<string, string>>();

        public class DictionaryItem<TKey, TValue>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }

            public DictionaryItem(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        public FrmDataGridView()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer |
                          ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint,
                true);
            this.UpdateStyles();

            InitDataSrc();
            InitStateDgv();
        }

        private void InitStateDgv()
        {
            //dgvState.Style = UIStyle.Green;
            dgvState.ReadOnly = true;
            dgvState.RowHeadersVisible = false;
            dgvState.AllowUserToAddRows = false;
            dgvState.AllowUserToResizeRows = false;
            dgvState.MultiSelect = true;
            dgvState.RowHeadersVisible = false;
            dgvState.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvState.ClearRows();
            dgvState.ClearColumns();
            
            // 设置 DataGridView 属性启用虚拟模式
            dgvState.VirtualMode = true;

            // 添加两列，一列显示键，一列显示值
            dgvState.Columns.Add("KeyColumn", "Key");
            dgvState.Columns.Add("ValueColumn", "Value");
            dgvState.RowCount = _stateBinding.Count;

            //// 将 BindingList 设置为 DataGridView 的数据源
            //dgvState.DataSource = bindingList;

            //注册事件处理器
            dgvState.CellValueNeeded += DgvState_CellValueNeeded;

            Task.Run(() =>
            {
                var i = 0;

                while (true)
                {
                    foreach (var key in _stateBinding.Keys.ToList())
                    {
                        _stateBinding[key] = (++i).ToString();
                    }

                    //var bindingList1 = new BindingList<KeyValuePair<string, string>>(_stateBinding.ToList());
                    if (dgvState.InvokeRequired)
                    {
                        dgvState.Invoke(new Action(UpdateDataGridView));
                    }
                    else
                    {
                        UpdateDataGridView();
                    }
                }
            });
        }

        private void DgvState_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // 获取键的列表
            var keys = new List<string>(_stateBinding.Keys);

            // 如果我们在第 0 列（显示键），则设置单元格的值为键
            if (e.ColumnIndex == 0)
            {
                e.Value = keys[e.RowIndex];
            }
            // 如果我们在第 1 列（显示值），则设置单元格的值为字典中键对应的值
            else if (e.ColumnIndex == 1)
            {
                string key = keys[e.RowIndex];
                e.Value = _stateBinding[key]; // 在这里使用正确的语法获取字典中的值
            }
        }

        private void UpdateDataGridView()
        {
            // Update the RowCount and refresh the display to reflect changes in the dictionary
            dgvState.RowCount = _stateBinding.Count;
            dgvState.Refresh();
        }

        private void InitDataSrc()
        {
            foreach (var f in Cx1EEmcDut.GetType().GetFields())
            {
                var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                if (!attrs.Any())
                    continue;
                var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                if (obj != null && obj.EmcType == Cx1EEmc.EmcType.State)
                    _stateBinding.Add(obj.Name, (string)f.GetValue(Cx1EEmcDut));
            }
            bindingList = new BindingList<DictionaryItem<string, string>>();
            foreach (var kvp in _stateBinding)
                bindingList.Add(new DictionaryItem<string, string>(kvp.Key, kvp.Value));
        }
    }
}
