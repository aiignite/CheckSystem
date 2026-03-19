using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using HZH_Controls.IconFont;

namespace CheckSystem.HelperForms.Sgm458Plg
{
    public partial class SqlDataRead : Form
    {
        public SqlDataRead()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Icon = FontImages.GetIcon(
               FontIcons.A_fa_search, 32,
               Color.DodgerBlue);

            //endTime.Format= DateTimePickerFormat.Long;

            cmbSqlIpAddr.Items.Add("192.168.0.138");
            //cmbSqlIpAddr.Items.Add("127.0.0.1");
            cmbSqlIpAddr.SelectedIndex = 0;

            var dataGridView = keyData.dataGridView;
            keyData.label.Text = @"SGM458-PLG数据查询";

            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AllowUserToResizeColumns = true;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.EditMode = DataGridViewEditMode.EditOnKeystroke;
            dataGridView.Margin = new Padding(3, 4, 3, 4);
            dataGridView.RowTemplate.Height = 30;

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "行号" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "id" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "EcuId" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "EcuIdFromFile" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "TackInfo" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreateTime" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "LastRecordTime" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "IsUsage" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "RandomSeed" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Sha256Total" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Sha256Sub20" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdCodeAscii" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdCode" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "MASTER_ECU_KEY_KeySlot" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "MASTER_ECU_KEY_M1" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "MASTER_ECU_KEY_M2" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "MASTER_ECU_KEY_M3" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "MASTER_ECU_KEY_M4" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "MASTER_ECU_KEY_M5" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "UNLOCK_ECU_KEY_KeySlot" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "UNLOCK_ECU_KEY_M1" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "UNLOCK_ECU_KEY_M2" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "UNLOCK_ECU_KEY_M3" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "UNLOCK_ECU_KEY_M4" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "UNLOCK_ECU_KEY_M5" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "MacKey" });

            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = true;
            dataGridView.AllowUserToResizeRows = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var dataGridView = keyData.dataGridView;
            dataGridView.Rows.Clear();

            try
            {
                var start = DateTime.Parse(string.Format(@"{0} 00:00:00", startTime.Text)).ToString("yyyy/MM/dd HH:mm:ss");
                var end = DateTime.Parse(string.Format(@"{0} 23:59:59", endTime.Text)).ToString("yyyy/MM/dd HH:mm:ss");

                var selectSql =
                    "SELECT " +
                    "[id],[EcuId],[EcuIdFromFile],[TackInfo],[CreateTime],[LastRecordTime],[IsUsage],[IsUploadToSgm],[RandomSeed],[Sha256Total],[Sha256Sub20],[IdCodeAscii],[IdCode],[MASTER_ECU_KEY_KeySlot],[MASTER_ECU_KEY_M1],[MASTER_ECU_KEY_M2],[MASTER_ECU_KEY_M3],[MASTER_ECU_KEY_M4],[MASTER_ECU_KEY_M5],[UNLOCK_ECU_KEY_KeySlot],[UNLOCK_ECU_KEY_M1],[UNLOCK_ECU_KEY_M2],[UNLOCK_ECU_KEY_M3],[UNLOCK_ECU_KEY_M4],[UNLOCK_ECU_KEY_M5],[MacKey] FROM [IPMS].[dbo].[manufactureSgm458PlgDatas] " +
                    "where [IsUsage] = '1' and " + "[LastRecordTime] >= '" + start + "' and [LastRecordTime] <= '" + end + "' order by [id] asc";

                if (string.IsNullOrEmpty(cmbSqlIpAddr.Text))
                    return;

                var ds = Query(selectSql, cmbSqlIpAddr.Text);

                for (var i = 0; i < ds.Tables[0].DefaultView.Count; i++)
                {
                    var newRow = dataGridView.Rows[dataGridView.Rows.Add()];
                    newRow.Cells[0].Value = string.Format("第{0}行", i + 1);

                    for (var j = 1; j < dataGridView.Columns.Count; j++)
                    {
                        var header = dataGridView.Columns[j].HeaderText;

                        newRow.Cells[j].Value = ds.Tables[0].DefaultView[i][header] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[i][header].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"读取失败：" + ex.Message);
            }
        }

        private static DataSet Query(string sqlString, string serverIp)
        {
            var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", serverIp, "IPMS",
                "ipms", "Scae2020#");

            using (var connection = new SqlConnection(sqlConnectiong))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(sqlString, connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbSqlIpAddr.Text))
                return;

            // 选择路径
            string path;
            using (var folder = new FolderBrowserDialog())
            {
                var result = folder.ShowDialog();
                if (result != DialogResult.OK)
                    return;
                path = folder.SelectedPath;
            }

            using (var sgm458 = new Controller.Sgm458Plg(""))
            {
                sgm458.ServerIp = cmbSqlIpAddr.Text;
                sgm458.EcuidsConfigDirectory = path;
                sgm458.ReadEcuIdsFromSRecodrAndUploadToServer();
                MessageBox.Show(@"导入结束");
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var start = DateTime.Parse(string.Format(@"{0} 00:00:00", startTime.Text)).ToString("yyyy/MM/dd HH:mm:ss");
            var end = DateTime.Parse(string.Format(@"{0} 23:59:59", endTime.Text)).ToString("yyyy/MM/dd HH:mm:ss");

            var selectSql =
                "SELECT " +
                "[id],[EcuId],[EcuIdFromFile],[TackInfo],[CreateTime],[LastRecordTime],[IsUsage],[IsUploadToSgm],[RandomSeed],[Sha256Total],[Sha256Sub20],[IdCodeAscii],[IdCode],[MASTER_ECU_KEY_KeySlot],[MASTER_ECU_KEY_M1],[MASTER_ECU_KEY_M2],[MASTER_ECU_KEY_M3],[MASTER_ECU_KEY_M4],[MASTER_ECU_KEY_M5],[UNLOCK_ECU_KEY_KeySlot],[UNLOCK_ECU_KEY_M1],[UNLOCK_ECU_KEY_M2],[UNLOCK_ECU_KEY_M3],[UNLOCK_ECU_KEY_M4],[UNLOCK_ECU_KEY_M5],[MacKey] FROM [IPMS].[dbo].[manufactureSgm458PlgDatas] " +
                "where [IsUsage] = '1' and " + "[LastRecordTime] >= '" + start + "' and [LastRecordTime] <= '" + end + "' order by [id] asc";

            // 选择路径
            string path;
            using (var folder = new FolderBrowserDialog())
            {
                var result = folder.ShowDialog();
                if (result != DialogResult.OK)
                    return;
                path = folder.SelectedPath + string.Format(@"\PLGKey-{0}_From{1}To{2}.xml",
                    DateTime.Now.ToString(CultureInfo.InvariantCulture)
                    .Replace(":", string.Empty)
                    .Replace("/", string.Empty)
                    .Replace(" ", string.Empty),
                    DateTime.Parse(start).ToString("yyyyMMdd"),
                    DateTime.Parse(end).ToString("yyyyMMdd"));
            }

            btnSearch_Click(null, null);
            Thread.Sleep(500);

            using (var sgm458 = new Controller.Sgm458Plg(""))
            {
                sgm458.ServerIp = cmbSqlIpAddr.Text;
                sgm458.EcuidsConfigDirectory = path;

                int count;
                var isSuccess = sgm458.PrintToXml(path, selectSql, out count);
                if (isSuccess)
                {
                    var fileDir = Path.GetDirectoryName(path);
                    var fileName = Path.GetFileNameWithoutExtension(path);
                    var subffix = Path.GetExtension(path);
                    var newFileName = string.Format("{0}\\{1}_{2}pcs{3}", fileDir, fileName, count, subffix);
                    File.Move(path, newFileName);

                    MessageBox.Show(string.Format("导出完成，导出内容为当前显示数据。\r\n共导出：{0}个\r\n路径：{1}", count, newFileName));
                }
                else
                {
                    MessageBox.Show(@"导出失败");
                }
            }
        }

        private void btnSearchNoUsedEcuIds_Click(object sender, EventArgs e)
        {
            var dataGridView = keyData.dataGridView;
            dataGridView.Rows.Clear();

            try
            {
                const string selectSql = "SELECT " +
                                         "[id],[EcuId],[EcuIdFromFile],[TackInfo],[CreateTime],[LastRecordTime],[IsUsage],[IsUploadToSgm],[RandomSeed],[Sha256Total],[Sha256Sub20],[IdCodeAscii],[IdCode],[MASTER_ECU_KEY_KeySlot],[MASTER_ECU_KEY_M1],[MASTER_ECU_KEY_M2],[MASTER_ECU_KEY_M3],[MASTER_ECU_KEY_M4],[MASTER_ECU_KEY_M5],[UNLOCK_ECU_KEY_KeySlot],[UNLOCK_ECU_KEY_M1],[UNLOCK_ECU_KEY_M2],[UNLOCK_ECU_KEY_M3],[UNLOCK_ECU_KEY_M4],[UNLOCK_ECU_KEY_M5],[MacKey] FROM [IPMS].[dbo].[manufactureSgm458PlgDatas] " +
                                         "where [IsUsage] = '0'";

                if (string.IsNullOrEmpty(cmbSqlIpAddr.Text))
                    return;

                var ds = Query(selectSql, cmbSqlIpAddr.Text);

                for (var i = 0; i < ds.Tables[0].DefaultView.Count; i++)
                {
                    var newRow = dataGridView.Rows[dataGridView.Rows.Add()];
                    newRow.Cells[0].Value = string.Format("第{0}行", i + 1);

                    for (var j = 1; j < dataGridView.Columns.Count; j++)
                    {
                        var header = dataGridView.Columns[j].HeaderText;

                        newRow.Cells[j].Value = ds.Tables[0].DefaultView[i][header] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[i][header].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"读取失败：" + ex.Message);
            }
        }
    }
}
