using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CheckSystem.Yfas.Utility;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.Prodcut
{
    public partial class YfasHistoryCheckDetailReview : UIForm
    {
        private readonly List<YfasCheckStateMachine.CheckDataView> _checkDataViewList = new List<YfasCheckStateMachine.CheckDataView>();

        public YfasHistoryCheckDetailReview(int productId, IEnumerable<YfasCheckStateMachine.CheckDataView> history)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            uiDataGridView1.ReadOnly = true;
            uiDataGridView1.RowHeadersVisible = false;
            uiDataGridView1.AllowUserToAddRows = false;
            uiDataGridView1.AllowUserToResizeRows = false;
            uiDataGridView1.AllowUserToOrderColumns = false;
            uiDataGridView1.MultiSelect = true;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            uiDataGridView1.CellFormatting += maintTable_CellFormatting;
            LoadData(productId, history);
        }

        private void LoadData(int productId,IEnumerable<YfasCheckStateMachine.CheckDataView> history)
        {
            var productBll = new Utility._3TierBll.YfasProductInfo();
            var productModel = productBll.GetModel(productId);

            if (productModel != null)
            {
                var rowIndex = productModel.RowIndex;
                var detectionsBll = new Utility._3TierBll.YfasDetectionBase();
                var detectionModels = detectionsBll.GetModelList("Row = '" + rowIndex + "'");

                var paraBll = new Utility._3TierBll.YfasProductParas();
                var thisProdcutDetectionParas = paraBll.GetModelList(string.Format("ProductId = '{0}' and IsDelete = '0'", productId));

                foreach (var detectionModel in detectionModels)
                {
                    var newData = new YfasCheckStateMachine.CheckDataView(detectionModel.DetectionName);

                    var paraModel = thisProdcutDetectionParas.FindAll(f => f.DetectionId == detectionModel.id);
                    if (paraModel.Any())
                    {
                        var model = paraModel[0];
                        newData.IsNotNa = "Y";
                        newData.Data = string.Empty;
                        newData.CostTime = string.Empty;
                        newData.Result = string.Empty;
                        newData.Unit = model.Uint;

                        newData.Range = string.Empty;
                        if (model.DateType.ToLower() == "double")
                        {
                            newData.Range = string.Format("{0}~{1}", model.Min, model.Max);
                        }
                        else if (model.DateType.ToLower() == "string" || model.DateType.ToLower() == "uid")
                        {
                            newData.Range = string.Format("{0}", model.Value);
                        }
                    }

                    //var paraModel=new Utility._3TierModel.YfasProductParas();

                    _checkDataViewList.Add(newData);
                }

                foreach (var value in history)
                {
                    var findIndex = _checkDataViewList.FindIndex(f => f.Name == value.Name);
                    if (findIndex != -1)
                    {
                        _checkDataViewList[findIndex].Data = value.Data;
                        _checkDataViewList[findIndex].Result = value.Result;
                        _checkDataViewList[findIndex].CostTime = value.CostTime;
                    }
                }

                if (_checkDataViewList.Count > 0)
                {
                    uiDataGridView1.ClearAll();
                    uiDataGridView1.DataSource = _checkDataViewList;
                    uiDataGridView1.AutoResizeRows();
                }
            }
        }

        private void maintTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (uiDataGridView1.Columns[e.ColumnIndex].HeaderText != @"Result")
                return;
            if (uiDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "NG")
                return;
            uiDataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkRed;
            uiDataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.DarkRed;
            uiDataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
            uiDataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.White;
        }
    }
}
