using MiniExcelLibs;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HZH_Controls.IconFont;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.YfasLogo
{
    public partial class FrmYfasLogoDataLog : UIForm
    {
        public readonly List<YfasLogoSqlHelper.LogoDataLogMode> Datas = new List<YfasLogoSqlHelper.LogoDataLogMode>();

        public FrmYfasLogoDataLog()
        {
            InitializeComponent();

            cmbDut.SelectedIndex = 0;
            cmbResult.SelectedIndex = 0;
            WindowState = FormWindowState.Maximized;
            Icon = FontImages.GetIcon(
               FontIcons.E_social_googledrive, 32, Color.DodgerBlue);
            dpStart.Value = DateTime.Now;
            dpEnd.Value = DateTime.Now;

            uiDataGridView1.AllowUserToDeleteRows = false;
            uiDataGridView1.AllowUserToAddRows = false;
            uiDataGridView1.AllowUserToResizeColumns = true;
            uiDataGridView1.AllowUserToResizeRows = true;
            uiDataGridView1.RowHeadersVisible = false;
            uiDataGridView1.AllowUserToOrderColumns = false;
            uiDataGridView1.ReadOnly = true;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            uiDataGridView1.DataError += uiDataGridView1_DataError;
        }

        private void uiDataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void uiPagination1_PageChanged(object sender, object pagingSource, int pageIndex, int count)
        {
            if (Datas.Count <= 0)
                return;
            var data = new List<YfasLogoSqlHelper.LogoDataLogMode>();
            for (var i = (pageIndex - 1) * count; i < (pageIndex - 1) * count + count; i++)
            {
                if (i >= Datas.Count) continue;
                data.Add(Datas[i]);
            }

            uiDataGridView1.DataSource = data;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Datas.Clear();

            var sql =
                string.Format(
                    "select * from DataHistory where CreateTime >= '{0} 00:00:00' and CreateTime <= '{1} 23:59:59'",
                    dpStart.Value.ToString("yyyy-MM-dd"), dpEnd.Value.ToString("yyyy-MM-dd"));
            if (cmbDut.SelectedIndex > 0)
                sql += string.Format(" and ProductName = '{0}'", cmbDut.SelectedIndex == 1 ? "前灯" : "后灯");

            if (cmbResult.SelectedIndex > 0)
                sql += string.Format(" and Result = '{0}'", cmbResult.Items[cmbResult.SelectedIndex]);

            this.ShowInfoTip(string.Format("正在查询'{0}'~'{1}'之间的数据", dpStart.Value.ToString("yyyy-MM-dd"),
                dpEnd.Value.ToString("yyyy-MM-dd")));

            Datas.AddRange(YfasLogoSqlHelper.GetDataModels(sql));

            //uiDataGridView1.ClearRows();
            //uiDataGridView1.DataSource = Datas;
            //uiDataGridView1.Refresh();

            uiDataGridView1.ClearRows();
            uiPagination1.TotalCount = 0;
            if (Datas != null && Datas.Count > 0)
            {
                uiDataGridView1.DataSource = Datas.ToArray();
                uiDataGridView1.Refresh();
                //设置分页控件总数
                uiPagination1.TotalCount = Datas.Count;
                //设置分页控件每页数量
                uiPagination1.PageSize = 50;
                //uiDataGridView1.SelectIndexChange += uiDataGridView1_SelectIndexChange;
                //设置统计绑定的表格
                //uiDataGridViewFooter1.DataGridView = uiDataGridView1;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (Datas.Count == 0)
            {
                this.ShowErrorTip("当前未查询到数据，请先查询一段时间的数据");
                return;
            }

            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var path = Path.Combine(fbd.SelectedPath, string.Format("export_on_{0}_{1}.xlsx",
                        DateTime.Now.ToString("yyyyMMdd-hhmmss"), Guid.NewGuid().ToString().Substring(24, 12)));
                    MiniExcel.SaveAs(path, Datas);
                    this.ShowSuccessTip("导出成功：" + path);
                }
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            var value = string.Empty;
            if (this.InputPasswordDialog(ref value))
            {
                if (!string.IsNullOrEmpty(value) && value.ToLower() == "admin")
                {
                    YfasLogoSqlHelper.ClearAllDataHistory();
                    this.ShowSuccessTip("删除成功，请重新查询数据");
                }
                else
                {
                    this.ShowErrorTip("密码错误");
                }
            }
            else
            {
                this.ShowErrorTip("密码为空");
            }
        }
    }
}
