using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CheckSystem.CcdForms
{
    public partial class FormCcdAddBarcode : Form
    {
        private readonly Action<BarcodeStruct> _setBarcodeFunc;

        public FormCcdAddBarcode(Action<BarcodeStruct> setBarcodeFunc)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            _setBarcodeFunc = setBarcodeFunc;

            dgvBarcodeGroupList.label.Text = @"档位及匹配码";
            dgvBarcodeGroupList.label.Height = 30;
            dgvBarcodeGroupList.dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 10, FontStyle.Regular);

            dgvBarcodeGroupList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "匹配码内容" });
            dgvBarcodeGroupList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "匹配码位置" });
            dgvBarcodeGroupList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位1内容" });
            dgvBarcodeGroupList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位1位置" });
            dgvBarcodeGroupList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位2内容" });
            dgvBarcodeGroupList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位2位置" });
            dgvBarcodeGroupList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位3内容" });
            dgvBarcodeGroupList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位3位置" });
            dgvBarcodeGroupList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位4内容" });
            dgvBarcodeGroupList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位4位置" });

            lblTxtBarcodeLength.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            lblTxtPartNoIndex.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            lblTxtHardwareIndex.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            lblTxtSoftwareIndex.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            for (var i = 1; i < 101; i++)
            {
                lblTxtPartNoIndex.comboBox.Items.Add(i);
                lblTxtBarcodeLength.comboBox.Items.Add(i);
                lblTxtHardwareIndex.comboBox.Items.Add(i);
                lblTxtSoftwareIndex.comboBox.Items.Add(i);
            }

            lblTxtPartNoIndex.comboBox.SelectedIndex = 1 - 1;
            lblTxtBarcodeLength.comboBox.SelectedIndex = 35 - 1;
            lblTxtHardwareIndex.comboBox.SelectedIndex = 10 - 1;
            lblTxtSoftwareIndex.comboBox.SelectedIndex = 15 - 1;
        }

        public class BarcodeStruct
        {
            public string Name;
            public int Length;
            public string PartNo;
            public int PartNoIndex;
            public string Hardware;
            public int HardwareIndex;
            public string Software;
            public int SofrwareIndex;
            public List<GearStruct> GearList;
        }

        public class GearStruct
        {
            public string MatchCode;
            public int MatchCodeIndex;
            public string Gear1;
            public int Gear1Index;
            public string Gear2;
            public int Gear2Index;
            public string Gear3;
            public int Gear3Index;
            public string Gear4;
            public int Gear4Index;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var newBarcodeStruct = new BarcodeStruct
            {
                Name = lblTxtBarcodeName.Text,
                Length = int.Parse(lblTxtBarcodeLength.comboBox.Text),
                PartNo = lblTxtPartNo.Text,
                PartNoIndex = int.Parse(lblTxtPartNoIndex.comboBox.Text),
                Hardware = lblTxtHardware.Text,
                HardwareIndex = int.Parse(lblTxtHardwareIndex.comboBox.Text),
                Software = lblTxtSoftware.Text,
                SofrwareIndex = int.Parse(lblTxtSoftwareIndex.comboBox.Text),
                GearList = new List<GearStruct>()
            };

            for (var i = 0; i < dgvBarcodeGroupList.dataGridView.RowCount - 1; i++)
            {
                var row = dgvBarcodeGroupList.dataGridView.Rows[i];

                var newGear = new GearStruct
                {
                    MatchCode = row.Cells[0].Value == null
                        ? string.Empty
                        : row.Cells[0].Value.ToString().ToUpper(),
                    MatchCodeIndex = row.Cells[1].Value == null
                        ? -1
                        : int.Parse(row.Cells[1].Value.ToString()),
                    Gear1 = row.Cells[2].Value == null
                        ? string.Empty
                        : row.Cells[2].Value.ToString().ToUpper(),
                    Gear1Index = row.Cells[3].Value == null
                        ? -1
                        : int.Parse(row.Cells[3].Value.ToString()),
                    Gear2 = row.Cells[4].Value == null
                        ? string.Empty
                        : row.Cells[4].Value.ToString().ToUpper(),
                    Gear2Index = row.Cells[5].Value == null
                        ? -1
                        : int.Parse(row.Cells[5].Value.ToString()),
                    Gear3 = row.Cells[6].Value == null
                        ? string.Empty
                        : row.Cells[6].Value.ToString().ToUpper(),
                    Gear3Index = row.Cells[7].Value == null
                        ? -1
                        : int.Parse(row.Cells[7].Value.ToString()),
                    Gear4 = row.Cells[8].Value == null
                        ? string.Empty
                        : row.Cells[8].Value.ToString().ToUpper(),
                    Gear4Index = row.Cells[9].Value == null
                        ? -1
                        : int.Parse(row.Cells[9].Value.ToString())
                };

                newBarcodeStruct.GearList.Add(newGear);
            }

            _setBarcodeFunc.Invoke(newBarcodeStruct);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
