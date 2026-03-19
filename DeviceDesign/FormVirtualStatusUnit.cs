using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DeviceDesign
{
    public partial class FormVirtualStatusUnit : Form
    {
        public List<string> LstStatusUnits = new List<string>();
        public string StatusUnitName;
        public FormVirtualStatusUnit()
        {
            InitializeComponent();
        }

        private void FormVirtualStatusUnit_Load(object sender, EventArgs e)
        {
            labelComboxStatusUnit.label.Text = "状态单元";
            labelComboxStatusUnit.comboBox.DataSource = LstStatusUnits;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if(labelComboxStatusUnit.comboBox.Text == null)
            {
                MessageBox.Show("未选择状态单元！");
                return;
            }
            StatusUnitName = labelComboxStatusUnit.comboBox.Text;
            this.Close();
        }
    }
}
