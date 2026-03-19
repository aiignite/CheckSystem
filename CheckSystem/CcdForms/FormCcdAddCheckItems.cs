using System.Linq;
using System.Windows.Forms;

namespace CheckSystem.CcdForms
{
    public partial class FormCcdAddCheckItems : Form
    {
        private DataGridView Dgv { get; set; }

        public FormCcdAddCheckItems(DataGridView dgv)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Dgv = dgv;

            lblCmbCoupSet.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            lblCmbCoupSet.comboBox.Items.Add("OFF");
            lblCmbCoupSet.comboBox.Items.Add("串联");
            lblCmbCoupSet.comboBox.Items.Add("并联");
            lblCmbCoupSet.comboBox.SelectedIndex = 0;

            lblCmbVolt1Max.comboBox.Items.Add(13.5);
            lblCmbVolt1Max.comboBox.SelectedIndex = 0;

            lblCmbVolt2Max.comboBox.Items.Add(13.5);
            lblCmbVolt2Max.comboBox.SelectedIndex = 0;

            lblCmbVolt3Max.comboBox.Items.Add(5);
            lblCmbVolt3Max.comboBox.SelectedIndex = 0;

            lblCmbCur1Max.comboBox.Items.Add(5);
            lblCmbCur1Max.comboBox.SelectedIndex = 0;

            lblCmbCur2Max.comboBox.Items.Add(5);
            lblCmbCur2Max.comboBox.SelectedIndex = 0;

            lblCmbCur3Max.comboBox.Items.Add(0.1);
            lblCmbCur3Max.comboBox.SelectedIndex = 0;

            lblCmbAdcSelect.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            lblCmbAdcSelect.comboBox.Items.Add("精密电源电流1");
            lblCmbAdcSelect.comboBox.Items.Add("精密电源电流2");
            lblCmbAdcSelect.comboBox.Items.Add("精密电源电流3");
            lblCmbAdcSelect.comboBox.Items.Add("AD板电流1");
            lblCmbAdcSelect.comboBox.Items.Add("AD板电流2");
            lblCmbAdcSelect.comboBox.Items.Add("AD板电流3");
            lblCmbAdcSelect.comboBox.Items.Add("AD板电流4");
            lblCmbAdcSelect.comboBox.Items.Add("AD板电流5");
            lblCmbAdcSelect.comboBox.Items.Add("AD板电流6");
            lblCmbAdcSelect.comboBox.SelectedIndex = 0;

            lblCmbAdcK.comboBox.Items.Add(1);
            lblCmbAdcK.comboBox.SelectedIndex = 0;

            lblCmbAdcB.comboBox.Items.Add(0);
            lblCmbAdcB.comboBox.SelectedIndex = 0;

            lblCmbResVin.comboBox.Items.Add("5");
            lblCmbResVin.comboBox.SelectedIndex = 0;

            lblCmbResValue.comboBox.Items.Add("10000");
            lblCmbResValue.comboBox.SelectedIndex = 0;

            lblCmbAdvSelect.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            lblCmbAdvSelect.comboBox.Items.Add("AD板电压1");
            lblCmbAdvSelect.comboBox.Items.Add("AD板电压2");
            lblCmbAdvSelect.comboBox.Items.Add("AD板电压3");
            lblCmbAdvSelect.comboBox.Items.Add("AD板电压4");
            lblCmbAdvSelect.comboBox.Items.Add("AD板电压5");
            lblCmbAdvSelect.comboBox.Items.Add("AD板电压6");
            lblCmbAdvSelect.comboBox.Items.Add("AD板电压7");
            lblCmbAdvSelect.comboBox.Items.Add("AD板电压8");
            lblCmbAdvSelect.comboBox.Items.Add("精密电源电压1");
            lblCmbAdvSelect.comboBox.Items.Add("精密电源电压2");
            lblCmbAdvSelect.comboBox.Items.Add("精密电源电压3");
            lblCmbAdvSelect.comboBox.SelectedIndex = 0;

            lblCmbAdvK.comboBox.Items.Add(1);
            lblCmbAdvK.comboBox.SelectedIndex = 0;

            lblCmbAdvB.comboBox.Items.Add(0);
            lblCmbAdvB.comboBox.SelectedIndex = 0;

            dgvCurGroup.label.Height = 30;
            dgvCurGroup.label.Text = @"请输入档位及范围/mA";
            dgvCurGroup.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位名称" });
            dgvCurGroup.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Min" });
            dgvCurGroup.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Max" });

            dgvVoltOrResGroup.label.Height = 30;
            dgvVoltOrResGroup.label.Text = @"请输入档位及范围/V";
            dgvVoltOrResGroup.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位名称" });
            dgvVoltOrResGroup.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Min" });
            dgvVoltOrResGroup.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Max" });

            rbVolt.Checked = true;
            rbVolt.CheckedChanged += rbVolt_CheckedChanged;
            rbRes.CheckedChanged += rbRes_CheckedChanged;
            cbCurr.CheckedChanged += cbCurr_CheckedChanged;
            cbVoltOrRes.CheckedChanged += cbVoltOrRes_CheckedChanged;
            cbVision.CheckedChanged += cbVision_CheckedChanged;
        }

        private void cbVision_CheckedChanged(object sender, System.EventArgs e)
        {
            lblCmbCameraSn.Enabled = cbVision.Checked;
            lblCmbVisionName.Enabled = cbVision.Checked;
        }

        private void cbVoltOrRes_CheckedChanged(object sender, System.EventArgs e)
        {
            rbVolt.Enabled = cbVoltOrRes.Checked;
            rbRes.Enabled = cbVoltOrRes.Checked;
            lblCmbResVin.Enabled = cbVoltOrRes.Checked;
            lblCmbResValue.Enabled = cbVoltOrRes.Checked;
            lblCmbAdvSelect.Enabled = cbVoltOrRes.Checked;
            lblCmbAdvK.Enabled = cbVoltOrRes.Checked;
            lblCmbAdvB.Enabled = cbVoltOrRes.Checked;
            lblCmbVoltOrResName.Enabled = cbVoltOrRes.Checked;
            dgvVoltOrResGroup.Enabled = cbVoltOrRes.Checked;

            if (!cbVoltOrRes.Checked)
                return;

            lblCmbResVin.Enabled = !rbVolt.Checked;
            lblCmbResValue.Enabled = !rbVolt.Checked;
        }

        private void cbCurr_CheckedChanged(object sender, System.EventArgs e)
        {
            lblCmbAdcSelect.Enabled = cbCurr.Checked;
            lblCmbAdcK.Enabled = cbCurr.Checked;
            lblCmbAdcB.Enabled = cbCurr.Checked;
            lblCmbCurName.Enabled = cbCurr.Checked;
            dgvCurGroup.Enabled = cbCurr.Checked;
        }

        private void rbRes_CheckedChanged(object sender, System.EventArgs e)
        {
            lblCmbResVin.Enabled = rbRes.Checked;
            lblCmbResValue.Enabled = rbRes.Checked;
        }

        private void rbVolt_CheckedChanged(object sender, System.EventArgs e)
        {
            lblCmbResVin.Enabled = !rbVolt.Checked;
            lblCmbResValue.Enabled = !rbVolt.Checked;
        }

        private void btnRelay1_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x201.Field.Relay1=1;");
        }

        private void btnRelay2_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x201.Field.Relay2=1;");
        }

        private void btnRelay3_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x201.Field.Relay3=1;");
        }

        private void btnRelay4_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x201.Field.Relay4=1;");
        }

        private void btnRelay5_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x201.Field.Relay5=1;");
        }

        private void btnRelay6_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x201.Field.Relay6=1;");
        }

        private void btnRelay7_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x201.Field.Relay7=1;");
        }

        private void btnRelay8_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x201.Field.Relay8=1;");
        }

        private void btnRelay9_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x201.Field.Relay9=1;");
        }

        private void btnRelay10_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x201.Field.Relay10=1;");
        }

        private void btnRelay11_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x202.Field.Relay1=1;");
        }

        private void btnRelay12_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x202.Field.Relay2=1;");
        }

        private void btnRelay13_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x202.Field.Relay3=1;");
        }

        private void btnRelay14_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x202.Field.Relay4=1;");
        }

        private void btnRelay15_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x202.Field.Relay5=1;");
        }

        private void btnRelay16_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x202.Field.Relay6=1;");
        }

        private void btnRelay17_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x202.Field.Relay7=1;");
        }

        private void btnRelay18_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x202.Field.Relay8=1;");
        }

        private void btnRelay19_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x202.Field.Relay9=1;");
        }

        private void btnRelay20_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("继电器从站0x202.Field.Relay10=1;");
        }

        private void btnDelay50_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("检测程序.Method.Sleep(50);");
        }

        private void btnDelay100_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("检测程序.Method.Sleep(100);");
        }

        private void btnDelay500_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("检测程序.Method.Sleep(500);");
        }

        private void btnDelay1000_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Add("检测程序.Method.Sleep(1000);");
        }

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void btnConfirm_Click(object sender, System.EventArgs e)
        {
            var rowsAdd = Dgv.Rows.Add();

            Dgv.Rows[rowsAdd].Cells[0].Value =
                lblCmbCoupSet.comboBox.Text == @"OFF"
                ? @"否"
                : lblCmbCoupSet.comboBox.Text;

            Dgv.Rows[rowsAdd].Cells[1].Value = lblCmbVolt1Max.comboBox.Text;
            Dgv.Rows[rowsAdd].Cells[2].Value = lblCmbVolt2Max.comboBox.Text;
            Dgv.Rows[rowsAdd].Cells[3].Value = lblCmbVolt3Max.comboBox.Text;

            Dgv.Rows[rowsAdd].Cells[4].Value = lblCmbCur1Max.comboBox.Text;
            Dgv.Rows[rowsAdd].Cells[5].Value = lblCmbCur2Max.comboBox.Text;
            Dgv.Rows[rowsAdd].Cells[6].Value = lblCmbCur3Max.comboBox.Text;

            var strItems = listBox1.Items.Cast<object>()
                .Aggregate(string.Empty, (current, t) => current + string.Format("{0}\r\n", t));
            Dgv.Rows[rowsAdd].Cells[7].Value = strItems;

            if (!cbVoltOrRes.Checked)
            {
                Dgv.Rows[rowsAdd].Cells[8].Value = @"否";
            }
            else
            {
                var voltOrResTemp = string.Empty;

                for (var i = 0; i < dgvVoltOrResGroup.dataGridView.RowCount - 1; i++)
                {
                    var row = dgvVoltOrResGroup.dataGridView.Rows[i];
                    var currName = lblCmbVoltOrResName.comboBox.Text;
                    var groupName = row.Cells[0].Value == null ? string.Empty : row.Cells[0].Value.ToString();
                    var min = row.Cells[1].Value == null ? "-9999" : row.Cells[1].Value.ToString();
                    var max = row.Cells[2].Value == null ? "9999" : row.Cells[2].Value.ToString();

                    voltOrResTemp += string.Format("{0},{1},{2}~{3};\r\n", currName, groupName, min, max);
                }

                if (rbVolt.Checked)
                {
                    Dgv.Rows[rowsAdd].Cells[8].Value = @"电压";
                    Dgv.Rows[rowsAdd].Cells[9].Value = voltOrResTemp;
                    Dgv.Rows[rowsAdd].Cells[12].Value = lblCmbAdvSelect.comboBox.Text;
                    Dgv.Rows[rowsAdd].Cells[13].Value = string.Format("{0}*X+{1}", lblCmbAdvK.comboBox.Text,
                        lblCmbAdvB.comboBox.Text);
                }
                else if (rbRes.Checked)
                {
                    Dgv.Rows[rowsAdd].Cells[8].Value = @"电阻";
                    Dgv.Rows[rowsAdd].Cells[9].Value = voltOrResTemp;
                    Dgv.Rows[rowsAdd].Cells[10].Value = lblCmbResVin.comboBox.Text;
                    Dgv.Rows[rowsAdd].Cells[11].Value = lblCmbResValue.comboBox.Text;
                    Dgv.Rows[rowsAdd].Cells[12].Value = lblCmbAdvSelect.comboBox.Text;
                    Dgv.Rows[rowsAdd].Cells[13].Value = string.Format("{0}*X+{1}", lblCmbAdvK.comboBox.Text,
                        lblCmbAdvB.comboBox.Text);
                }
            }

            if (!cbCurr.Checked)
            {
                Dgv.Rows[rowsAdd].Cells[14].Value = @"否";
            }
            else
            {
                var currTemp = string.Empty;

                for (var i = 0; i < dgvCurGroup.dataGridView.RowCount - 1; i++)
                {
                    var row = dgvCurGroup.dataGridView.Rows[i];
                    var currName = lblCmbCurName.comboBox.Text;
                    var groupName = row.Cells[0].Value == null ? string.Empty : row.Cells[0].Value.ToString();
                    var min = row.Cells[1].Value == null ? "-9999" : row.Cells[1].Value.ToString();
                    var max = row.Cells[2].Value == null ? "9999" : row.Cells[2].Value.ToString();

                    currTemp += string.Format("{0},{1},{2}~{3};\r\n", currName, groupName, min, max);
                }

                Dgv.Rows[rowsAdd].Cells[14].Value = @"是";
                Dgv.Rows[rowsAdd].Cells[15].Value = currTemp;
                Dgv.Rows[rowsAdd].Cells[16].Value = lblCmbAdcSelect.comboBox.Text;
                Dgv.Rows[rowsAdd].Cells[17].Value = string.Format("{0}*X+{1}", lblCmbAdcK.comboBox.Text,
                        lblCmbAdcB.comboBox.Text);
            }

            if (!cbVision.Checked)
            {
                Dgv.Rows[rowsAdd].Cells[18].Value = @"否";
            }
            else
            {
                Dgv.Rows[rowsAdd].Cells[18].Value = @"是";
                Dgv.Rows[rowsAdd].Cells[19].Value = lblCmbCameraSn.comboBox.Text;
                Dgv.Rows[rowsAdd].Cells[20].Value = lblCmbVisionName.comboBox.Text;
            }

            Close();
        }
    }
}
