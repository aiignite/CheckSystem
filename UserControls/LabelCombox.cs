using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace UserControls
{
    public partial class LabelCombox : UserControl
    {
        public List<string> ListText = new List<string>();

        public string LabelString
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        public override string Text
        {
            get
            {
                return comboBox.Text;
            }

            set
            {
                comboBox.Text = string.Empty;
                comboBox.Text = value;
            }
        }

        public LabelCombox()
        {
            InitializeComponent();
            //获取控件的Type,设置双缓存
            Type dgvType = comboBox.GetType();
            PropertyInfo properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //为控件的属性设置值
            if (properInfo != null) properInfo.SetValue(comboBox, true, null);
            foreach (var item in comboBox.Items)
                ListText.Add(item.ToString());
        }

        private void comboBox_TextUpdate(object sender, EventArgs e)
        {
            if (comboBox.Text==@" ")
            {
                comboBox.Text = "";
            }
            //var listNew = ListText.Where(item => item.Contains(comboBox.Text)).ToList();

            var listNew = ListText.FindAll(item => item.Contains(comboBox.Text));//.ToList();

            if (listNew.Count == 0)
                return;
            comboBox.Items.Clear();
            //combobox添加已经查到的关键词
            comboBox.Items.AddRange(listNew.ToArray());

            //设置光标位置，否则光标位置始终保持在第一列，造成输入关键词的倒序排列

            comboBox.SelectionStart = comboBox.Text.Length;

            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。

            Cursor = Cursors.Default;

            //自动弹出下拉框

            comboBox.DroppedDown = true;
        }

        public void SetText(string strText)
        {
            comboBox.Text = strText;
        }

        public string GetText()
        {
            return comboBox.Text;
        }
    }
}
