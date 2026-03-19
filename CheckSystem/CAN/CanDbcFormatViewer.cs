using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using CommonUtility.BusLoader;

namespace CheckSystem.CAN
{
    public partial class CanDbcFormatViewer : Form
    {
        private readonly DbcHelper _dbcHelper = new DbcHelper();

        private string _path = string.Empty;

        private readonly string[] _byteOrder = { "Motorola", "Intel" };
        private readonly string[] _valueType = { "Unsigned", "Signed", "Float", "Double" };

        public CanDbcFormatViewer()
        {
            InitializeComponent();
        }

        private void MainWid_Load(object sender, EventArgs e)
        {
            //init dbc treeview
            treeView1.Nodes.Add("Nodes");
            treeView1.Nodes.Add("Message");

            //init dbc listview
            listView1.View = View.Details;
            listView1.Columns.Add("Name");

            listView1.Columns.Add("MessageName");
            listView1.Columns.Add("MessageSize");
            listView1.Columns.Add("MessageId");

            listView1.Columns.Add("StartBit");
            listView1.Columns.Add("Length");
            listView1.Columns.Add("Byte Order");
            listView1.Columns.Add("Value Type");
            listView1.Columns.Add("Factor");
            listView1.Columns.Add("Offset");
            listView1.Columns.Add("Minimum");
            listView1.Columns.Add("Maximum");
            listView1.Columns.Add("Uint");
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = @"请选择DBC文件",
                Filter = @"DBC文件|*.dbc"
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            _path = dialog.FileName;
            textBox_path.Text = _path;

            try
            {
                _dbcHelper.Parse(_path);
                UpdateDbcTreeview();
            }
            catch (Exception en)
            {
                DbcHelper.ExceptionHandler.Handle(en);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((e.Node.Parent == null) || (e.Node.Parent.Text != @"Message"))
                return;
            try
            {
                UpdateDbcListview(e.Node.Index);
            }
            catch (Exception en)
            {
                MessageBox.Show(en.Message);
            }
        }

        private void UpdateDbcTreeview()
        {
            var dbName = new FileInfo(_path).Name.Remove(new FileInfo(_path).Name.Length - 4, 4);

            for (var i = 0; i < treeView1.Nodes.Count; i++)
                treeView1.Nodes[i].Nodes.Clear();
            foreach (var t in _dbcHelper.MyDbcFile[dbName].Nodes)
                treeView1.Nodes[0].Nodes.Add(t);
            foreach (var t in _dbcHelper.MyDbcFile[dbName].Messages)
                treeView1.Nodes[1].Nodes.Add(t.MessageName);

            treeView1.ExpandAll();
        }

        private void UpdateDbcListview(int index)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();

            var dbName = new FileInfo(_path).Name.Remove(new FileInfo(_path).Name.Length - 4, 4);

            foreach (var t in _dbcHelper.MyDbcFile[dbName].Messages[index].Signals)
            {
                var item = new ListViewItem { Text = t.SignalName };

                item.SubItems.Add(_dbcHelper.MyDbcFile[dbName].Messages[index].MessageName);
                item.SubItems.Add(_dbcHelper.MyDbcFile[dbName].Messages[index].MessageSize.ToString());
                item.SubItems.Add(_dbcHelper.MyDbcFile[dbName].Messages[index].MessgeId.ToString("x8"));

                item.SubItems.Add(t.StartBit.ToString());
                item.SubItems.Add(t.SignalSize.ToString());
                item.SubItems.Add(_byteOrder[t.ByteOrder]);
                item.SubItems.Add(_valueType[t.ValueType]);
                item.SubItems.Add(t.Factor.ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(t.Offset.ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(t.Minimum.ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(t.Maximum.ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(t.UintStr);
                listView1.Items.Add(item);
            }
            listView1.EndUpdate();
        }
    }
}
