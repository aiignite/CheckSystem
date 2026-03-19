using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Controller
{
    public partial class AllLedAutoAssemblyProcutSelect : Form
    {
        public static string SelectProduct { get; set; }

        public AllLedAutoAssemblyProcutSelect(IEnumerable<string> values)
        {
            InitializeComponent();

            foreach (var t in values)
                comboBox1.Items.Add(t);

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count > 0)
            {
                SelectProduct = comboBox1.Text;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
