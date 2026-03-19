using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonUtility;
using HZH_Controls.IconFont;
using Newtonsoft.Json;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.HelperForms.Sgm458Pdm
{
    public partial class FrmLoadBoxLogData : UIForm
    {
        public readonly List<LoadBoxSqlHelper.LoadBoxErrorInfoModel> Datas = new List<LoadBoxSqlHelper.LoadBoxErrorInfoModel>();

        public FrmLoadBoxLogData()
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
            uiDataGridView1.CellClick += uiDataGridView1_CellClick;
        }

        private void uiDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;
                if (uiDataGridView1.Rows[e.RowIndex].Cells[9].Value != null &&
                    !string.IsNullOrEmpty(uiDataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString()))
                {
                    var uiform = new UIForm
                    {
                        Text = uiDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(),
                        Icon = FontImages.GetIcon(
                            FontIcons.E_social_googledrive, 32, Color.DodgerBlue),
                        Style = UIStyle.Gray
                    };
                    var mainDgv = new UIDataGridView { Dock = DockStyle.Fill };
                    uiform.Controls.Add(mainDgv);

                    mainDgv.Style = UIStyle.Blue;
                    mainDgv.ReadOnly = true;
                    mainDgv.RowHeadersVisible = false;
                    mainDgv.AllowUserToAddRows = false;
                    mainDgv.AllowUserToResizeRows = false;
                    mainDgv.MultiSelect = true;
                    mainDgv.RowHeadersVisible = false;
                    mainDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    var dut = "Dut" + uiDataGridView1.Rows[e.RowIndex].Cells[2].Value;
                    //var canCommunicationState = uiDataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    var details = uiDataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();

                    mainDgv.ClearRows();
                    mainDgv.ClearColumns();
                    mainDgv.AddColumn("测试项目", "测试项目");
                    mainDgv.AddColumn("范围", "范围");
                    mainDgv.AddColumn("单位", "单位");
                    mainDgv.AddColumn(dut, dut);

                    var checkItems = JsonConvert.DeserializeObject(details,
                           typeof(List<SyProductionSaveCheckData.CheckDataDetail>)) as List<SyProductionSaveCheckData.CheckDataDetail>;

                    if (checkItems != null)
                    {
                        foreach (var t in checkItems)
                        {
                            mainDgv.AddRow(t.ParaName, t.Range, t.Type, t.Value);

                            mainDgv.Rows[mainDgv.RowCount - 1].DefaultCellStyle.BackColor = t.Result.ToLower() == "false" ? Color.Red : Color.Green;
                            mainDgv.Rows[mainDgv.RowCount - 1].Cells[3].Style.ForeColor = Color.WhiteSmoke;
                        }
                    }

                    //mainDgv.AutoResizeColumns();
                    uiform.ShowDialog();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void uiDataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Datas.Clear();

            var sql =
                string.Format(
                    "select * from LoadBoxErrorInfo where CreateTime >= '{0} 00:00:00' and CreateTime <= '{1} 23:59:59'",
                    dpStart.Value.ToString("yyyy-MM-dd"), dpEnd.Value.ToString("yyyy-MM-dd"));
            if (cmbDut.SelectedIndex > 0)
                sql += string.Format(" and DutIndex = '{0}'", cmbDut.SelectedIndex);

            if (cmbResult.SelectedIndex > 0)
                sql += string.Format(" and CheckResult = '{0}'", cmbResult.Items[cmbResult.SelectedIndex]);

            this.ShowInfoTip(string.Format("正在查询'{0}'~'{1}'之间的数据", dpStart.Value.ToString("yyyy-MM-dd"),
                dpEnd.Value.ToString("yyyy-MM-dd")));

            Datas.AddRange(LoadBoxSqlHelper.GetModels(sql));

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

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            var value = string.Empty;
            if (this.InputPasswordDialog(ref value))
            {
                if (!string.IsNullOrEmpty(value) && value == "098765")
                {
                    LoadBoxSqlHelper.ClearAllData();
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

        private void uiPagination1_PageChanged(object sender, object pagingSource, int pageIndex, int count)
        {
            if (Datas.Count <= 0)
                return;
            var data = new List<LoadBoxSqlHelper.LoadBoxErrorInfoModel>();
            for (var i = (pageIndex - 1) * count; i < (pageIndex - 1) * count + count; i++)
            {
                if (i >= Datas.Count) continue;
                data.Add(Datas[i]);
            }

            uiDataGridView1.DataSource = data;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (Datas.Count == 0)
            {
                this.ShowErrorTip("当前未查询到数据，请先查询一段时间的数据");
                return;
            }

            if (this.ShowAskDialog("确定导出?"))
            {
                // 选择路径
                string path;
                using (var folder = new FolderBrowserDialog())
                {
                    var result = folder.ShowDialog();
                    if (result != DialogResult.OK)
                        return;

                    for (var i = 1; i <= 6; i++)
                    {
                        path = folder.SelectedPath + string.Format(@"\PDM-LoadBoxData-Dut{0}_{1}.txt",
                            i, DateTime.Now.ToString(CultureInfo.InvariantCulture)
                                .Replace(":", string.Empty)
                                .Replace("/", string.Empty)
                                .Replace(" ", string.Empty));

                        var dut = Datas.FindAll(f => f.DutIndex.ToString() == i.ToString());

                        if (dut.Any())
                        {
                            var lines = new List<string>();

                            foreach (var t in dut)
                            {
                                var checkDetails = string.Empty;

                                if (!string.IsNullOrEmpty(t.Detail))
                                {
                                    var checkItems = JsonConvert.DeserializeObject(t.Detail,
                                        typeof(List<SyProductionSaveCheckData.CheckDataDetail>)) as
                                        List<SyProductionSaveCheckData.CheckDataDetail>;

                                    if (checkItems != null)
                                    {
                                        checkDetails =
                                            checkItems.Aggregate(checkDetails,
                                                (current, c) => current + string.Format("{0}={1}$", c.ParaName, c.Value))
                                                .TrimEnd('$');
                                    }
                                }

                                var str =
                                    string.Format(
                                        "{0}$[结果={1}]$[Mode={2}]$[ReleaseMotor={3}]$[CinchMotor={4}]$[ActuatorMoter={5}]$[通信={6}]${7}",
                                        t.CreateTime, t.CheckResult, t.Mode, t.ActuatorMotorState, t.CinchMotorState,
                                        t.ActuatorMotorState, t.CanCommunicationState,
                                        string.IsNullOrEmpty(checkDetails) ? "null" : checkDetails);
                                lines.Add(str);

                                File.WriteAllLines(path, lines);
                            }
                        }
                    }
                }

                this.ShowSuccessTip("导出完成");
            }
            else
            {
                this.ShowWarningTip("操作已取消");
            }
        }
    }
}
