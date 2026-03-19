using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace CheckSystem.HelperForms
{
    public partial class GetIpList : Form
    {
        public GetIpList()
        {
            InitializeComponent();

            Text = userDataGrid1.label.Text = @"IP地址列表";
            userDataGrid1.label.Visible = true;
            userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "IP地址" });
            userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "MAC地址" });
            userDataGrid1.dataGridView.ReadOnly = true;
            userDataGrid1.dataGridView.RowHeadersVisible = false;
            for (var i = 0; i < userDataGrid1.dataGridView.Columns.Count; i++)
                userDataGrid1.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// 获取ARP查询字符串
        /// </summary>
        /// <returns></returns>
        private static string GetArpResult()
        {
            Process p = null;
            var outPut = string.Empty;
            try
            {
                p = Process.Start(new ProcessStartInfo("arp", "-a")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                });
                if (p != null) outPut = p.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                //throw new Exception("IPInfo: Error Retrieving 'arp -a' Results", ex);
                return ex.Message;
            }
            finally
            {
                if (p != null)
                {
                    p.Close();
                }
            }
            return outPut;
        }

        /// <summary>
        /// 获取IP地址与Mac地址对应数据表
        /// </summary>
        /// <returns>Mac-IP</returns>
        private static IEnumerable<string[]> GetIpInfo()
        {
            try
            {
                return (from arp in GetArpResult().Split('\n', '\r')
                        where !string.IsNullOrEmpty(arp)
                        select
                            (from piece in arp.Split(' ', '\t') where !string.IsNullOrEmpty(piece) select piece).ToArray()
                            into pieces
                            where pieces.Length == 3
                            select new[] { pieces[1], pieces[0] }).ToList();
            }
            catch (Exception ex)
            {
                //throw new Exception("IPInfo: Error Parsing 'arp -a' results", ex);
                MessageBox.Show(ex.Message);
                return new List<string[]>();
            }
        }

        private void ucBtnExt1_BtnClick(object sender, EventArgs e)
        {
            userDataGrid1.dataGridView.Rows.Clear();

            foreach (var t in GetIpInfo())
            {
                if (t == null || t.Length < 2)
                    continue;
                var rowAdd = userDataGrid1.dataGridView.Rows.Add();

                var newRow = userDataGrid1.dataGridView.Rows[rowAdd];

                newRow.Cells[0].Value = t[1];
                newRow.Cells[1].Value = t[0];
            }
        }
    }
}
