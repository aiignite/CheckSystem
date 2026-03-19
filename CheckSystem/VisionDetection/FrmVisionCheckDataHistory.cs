using MiniExcelLibs;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CommonUtility;
using HZH_Controls.IconFont;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.VisionDetection
{
    public partial class FrmVisionCheckDataHistory : UIForm
    {
        public readonly List<LocalDbHelper.manufactureCheckData> Datas = new List<LocalDbHelper.manufactureCheckData>();

        public FrmVisionCheckDataHistory()
        {
            InitializeComponent();

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

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Datas.Clear();

            var sql =
                string.Format(
                    "select * from manufactureCheckData where CreateTime >= '{0} 00:00:00' and CreateTime <= '{1} 23:59:59'",
                    dpStart.Value.ToString("yyyy-MM-dd"), dpEnd.Value.ToString("yyyy-MM-dd"));

            if (!string.IsNullOrEmpty(txtBarcode.Text))
                sql += string.Format(" and productUid like '%{0}%'", txtBarcode.Text);

            this.ShowInfoTip(string.Format("正在查询'{0}'~'{1}'之间的数据", dpStart.Value.ToString("yyyy-MM-dd"),
                dpEnd.Value.ToString("yyyy-MM-dd")));

            try
            {
                Datas.AddRange(LocalDbHelper.QueryBySqlString(sql));

                //if (LocalDbHelper.LocalSqlite != null)
                //{
                //    var getData = LocalDbHelper.LocalSqlite.GetRows(sql);

                //    foreach (var row in getData)
                //    {
                //        if (row != null)
                //        {
                //            var model = new Model.manufactureCheckData();

                //            if (row["id"] != null && row["id"].ToString() != "")
                //            {
                //                model.id = int.Parse(row["id"].ToString());
                //            }
                //            if (row["taskNo"] != null)
                //            {
                //                model.taskNo = row["taskNo"].ToString();
                //            }
                //            if (row["productNo"] != null)
                //            {
                //                model.productNo = row["productNo"].ToString();
                //            }
                //            if (row["productUid"] != null)
                //            {
                //                model.productUid = row["productUid"].ToString();
                //            }
                //            if (row["pcbaNo"] != null)
                //            {
                //                model.pcbaNo = row["pcbaNo"].ToString();
                //            }
                //            if (row["pcbaBarcode"] != null)
                //            {
                //                model.pcbaBarcode = row["pcbaBarcode"].ToString();
                //            }
                //            if (row["productBarcode"] != null)
                //            {
                //                model.productBarcode = row["productBarcode"].ToString();
                //            }
                //            if (row["packageBarcode"] != null)
                //            {
                //                model.packageBarcode = row["packageBarcode"].ToString();
                //            }
                //            if (row["processNo"] != null)
                //            {
                //                model.processNo = row["processNo"].ToString();
                //            }
                //            if (row["checkData"] != null)
                //            {
                //                model.checkData = row["checkData"].ToString();
                //            }
                //            if (row["checkDate"] != null)
                //            {
                //                model.checkDate = DateTime.Parse(row["checkDate"].ToString());
                //            }
                //            if (row["checkStaffNo"] != null)
                //            {
                //                model.checkStaffNo = row["checkStaffNo"].ToString();
                //            }
                //            if (row["checkResult"] != null)
                //            {
                //                model.checkResult = row["checkResult"].ToString();
                //            }
                //            if (row["creater"] != null)
                //            {
                //                model.creater = row["creater"].ToString();
                //            }
                //            if (row["createTime"] != null)
                //            {
                //                model.createTime = DateTime.Parse(row["createTime"].ToString());
                //            }

                //            Datas.Add(model);
                //        }
                //    }
                //}
            }
            catch (Exception exception)
            {
                // ignored
            }

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

        private void uiPagination1_PageChanged(object sender, object pagingSource, int pageIndex, int count)
        {
            if (Datas.Count <= 0)
                return;
            var data = new List<LocalDbHelper.manufactureCheckData>();
            for (var i = (pageIndex - 1) * count; i < (pageIndex - 1) * count + count; i++)
            {
                if (i >= Datas.Count)
                    continue;
                data.Add(Datas[i]);
            }

            uiDataGridView1.DataSource = data;
        }
    }
}
