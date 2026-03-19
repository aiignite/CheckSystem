using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CheckSystem.MaterialHelperForms.MaterialModels;
using Sunny.UI;

namespace CheckSystem.MaterialHelperForms
{
    public partial class HikManualGeneration : UIForm
    {
        private readonly string _stockNo;
        private readonly string _supplyNo;
        private readonly string _trayNo;
        private List<NewMaterialStockInInfo> _materialStockInInfo = new List<NewMaterialStockInInfo>();
        private List<NewMaterialPrintCorrespond> _toComparePrintModels = new List<NewMaterialPrintCorrespond>();

        public HikManualGeneration(string stockInNo, string supplyNo, string trayNo)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            _stockNo = stockInNo;
            _supplyNo = supplyNo;
            _trayNo = trayNo;

            if (ReadData())
                Load += HikManualGeneration_Load;
            else
                uiSymbolButton1.Enabled = false;
        }

        private bool ReadData()
        {
            _materialStockInInfo =
                HikDbHelperSql.GetNewMaterialStockInInfoModels(
                    "NeedNum > 0 and Status = '待入库' and IsDelete = \'0\' and StockInNo = '" +
                    _stockNo + "' and SupplyNo = '" + _supplyNo + "' order by stockInNo desc");
            if (!_materialStockInInfo.Any())
            {
                MessageBox.Show(string.Format("{0}: 根据当前入库单号{1}和供应商代码{2}，未在系统中查询当相关的待入库物料。",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), _stockNo, _supplyNo));
                return false;
            }

            _toComparePrintModels = new List<NewMaterialPrintCorrespond>();
            foreach (var t in _materialStockInInfo)
                _toComparePrintModels.AddRange(
                    HikDbHelperSql.GetNewMaterialPrintCorrespondModels(
                        "IsDelete = '0' and Status = '有效' and MaterialNo = '" + t.MaterialNo + "'"));
            if (!_toComparePrintModels.Any())
            {
                MessageBox.Show(string.Format("{0}: 根据当前入库单号{1}和供应商代码{2}，未在系统中查询到相关物料号的二维码匹配信息。",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), _stockNo, _supplyNo));
                return false;
            }

            return true;
        }

        private void HikManualGeneration_Load(object sender, EventArgs e)
        {
            foreach (var m in _materialStockInInfo)
                cmbMaterialNo.Items.Add(m.MaterialNo);
            cmbMaterialNo.SelectedIndexChanged += cmbMaterialNo_SelectedIndexChanged;
            cmbMaterialBarcode.SelectedValueChanged += cmbMaterialBarcode_SelectedIndexChanged;
            //cmbModelName.SelectedValueChanged += cmbModelName_SelectedValueChanged;
            cmbMaterialNo.SelectedIndex = 0;
        }

        private void cmbMaterialBarcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var materialNo = cmbMaterialNo.Items[cmbMaterialNo.SelectedIndex].ToString();
            var str = cmbMaterialBarcode.Items[cmbMaterialBarcode.SelectedIndex].ToString();

            try
            {
                var materialBarcodeIndex = str.IndexOf("扫描标识：");
                var ledGroupIndex = str.IndexOf("，特征值/档位：");

                var materialBarcode = str.Substring(0 + "扫描标识：".Length, ledGroupIndex - (materialBarcodeIndex + "扫描标识：".Length));
                var ledGroup = str.Substring(ledGroupIndex + "，特征值/档位：".Length, str.Length - ("扫描标识：" + materialBarcode).Length - "，特征值/档位：".Length);

                var find =
                    _toComparePrintModels.Find(f => f.MaterialNo == materialNo && (f.MaterialBarcode == materialBarcode || f.MaterialBarcode == materialBarcode + "\r\n") && f.SupplyLedGroup == ledGroup);
                if (find != null)
                {
                    txtMaterialName.Text = find.MaterialName;
                    txtModelName.Text = find.ModelName;
                    txtSupplyLedGroup.Text = find.SupplyLedGroup;
                    txtSupplyLedNo.Text = find.SupplyLedNo;

                    uiSymbolButton1.Enabled = true;
                }
                else
                {
                    this.ShowErrorDialog("查询失败");
                    uiSymbolButton1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorDialog(ex.Message);
                uiSymbolButton1.Enabled = false;
                Close();
            }
        }

        public void cmbMaterialNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var materialNo = cmbMaterialNo.Items[cmbMaterialNo.SelectedIndex];

            ResetChildValue();

            foreach (var t in _toComparePrintModels.Where(t => t.MaterialNo == (string)materialNo))
            {
                var str = string.Format("扫描标识：{0}，特征值/档位：{1}", t.MaterialBarcode.TrimEnd(), t.SupplyLedGroup);
                cmbMaterialBarcode.Items.Add(str);
                //cmbMaterialBarcode.Items.Add(t.MaterialBarcode);
            }

            if (cmbMaterialBarcode.Items.Count > 0)
                cmbMaterialBarcode.SelectedIndex = 0;
        }

        private void ResetChildValue()
        {
            cmbMaterialBarcode.Items.Clear();
            txtMaterialName.Text = string.Empty;
            txtModelName.Text = string.Empty;
            txtSupplyLedGroup.Text = string.Empty;
            txtSupplyLedNo.Text = string.Empty;
            txtDCode.Text = string.Empty;
            txtCount.Text = string.Empty;
        }

        internal class Info
        {
            public string Value { get; set; }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            var materialNo = cmbMaterialNo.Items[cmbMaterialNo.SelectedIndex].ToString();
            var supplyNo = _supplyNo;
            var stockInNo = _stockNo;

            var supplyLedGroup = txtSupplyLedGroup.Text;
            var supplyLedNo = txtSupplyLedNo.Text;
            var logPartNo = txtModelName.Text;

            if (string.IsNullOrEmpty(logPartNo))
            {
                this.ShowErrorTip("产品型号为空");
                return;
            }
            if (string.IsNullOrEmpty(logPartNo))
            {
                this.ShowErrorTip("产品型号为空");
                return;
            }

            if (string.IsNullOrEmpty(txtCount.Text))
            {
                this.ShowErrorTip("规格未输入");
                return;
            }

            var stockInCount = int.Parse(txtCount.Text);
            var totalNum = 0;
            var findStockIn = _materialStockInInfo.Find(f => f.MaterialNo == materialNo);
            if (findStockIn == null)
                return;

            #region 计算当前入库数和总数，并作对比

            int needNum;
            if (!int.TryParse(findStockIn.NeedNum.ToString(), out needNum))
            {
                this.ShowErrorTip(string.Format(
                    "{0}: 根据入库单号：{1}，供应商编码：{2}，物料号：{3}，未在入库单表中查找到NeedNum数据",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), stockInNo, supplyNo, materialNo));
                return;
            }

            var getHistoryStockInCount =
                HikDbHelperSql.GetDataTable("select count from newMaterialStockInDetail where StockInNo = '" + stockInNo +
                                            "' and MaterialNo = '" + materialNo + "' and SupplyNo = '" +
                                            supplyNo + "' and IsDelete = '0'");
            for (var i = 0; i < getHistoryStockInCount.DefaultView.Count; i++)
            {
                if (getHistoryStockInCount.DefaultView[i]["count"] == null)
                    continue;
                int c;
                if (int.TryParse(getHistoryStockInCount.DefaultView[i]["count"].ToString(), out c))
                    totalNum += c;
            }

            if (totalNum + stockInCount > needNum)
            {
                this.ShowErrorTip(string.Format("{0}: 根据入库单号：{1}，供应商编码：{2}，物料号：{3}，查询到订单数为：{4}，当前已入库订单数为{5}，当前扫描数为{6}，无法入库",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), stockInNo, supplyNo, materialNo, needNum, totalNum, stockInCount));
                return;
            }

            #endregion

            #region 判断当前箱体同一物料号的批次，批次不在同一月不能入库

            string dcNo;
            var errorMsg = string.Empty;
            if (!HikMaterialAnalysisHelper.FormatDcode(txtDCode.Text, out dcNo, ref errorMsg))
            {
                this.ShowErrorTip("DCode输入错误");
                return;
            }

            var toInMaterialNo = materialNo;
            var toInDcLotNo = dcNo;

            var getMaterialInThisTrayNo =
                "select distinct MaterialNo from newMaterialStockInDetail  WHERE IsDelete = '0' AND TrayNo = '" + _trayNo + "'";
            var materials = HikDbHelperSql.GetData(getMaterialInThisTrayNo);
            if (materials.Contains(toInMaterialNo))
            {
                var getModels =
                    HikDbHelperSql.GetNewMaterialStockInDetail(
                        string.Format("IsDelete = '0' AND TrayNo = '" + _trayNo + "' and MaterialNo = '" + toInMaterialNo + "'"));

                if (getModels.Any())
                {
                    var dcLotNo = getModels[0].DclotNo;
                    if (dcLotNo.Substring(0, 6) != toInDcLotNo.Substring(0, 6))
                    {
                        this.ShowErrorTip(string.Format("{0}: {1}",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            string.Format("物料号={0}，箱体={1}，已入箱批次:{2}，当前入箱批次{3}，批次不同无法入同一箱体。", toInMaterialNo, _trayNo,
                                dcLotNo, toInDcLotNo)));
                        return;
                    }
                }
            }

            #endregion

            string stockInDate;
            string orderNo;
            string materialPrintNo;
            if (HikDbHelperSql.TryGetSerialNo(
                materialNo, supplyNo, stockInCount, stockInNo,
                supplyLedGroup, supplyLedNo, logPartNo, dcNo,
                string.Empty, findStockIn.Qualevel, findStockIn.Issporadic, findStockIn.Earmarks,
                string.Empty, logPartNo,
                (totalNum + stockInCount).ToString(), out stockInDate, out orderNo, out materialPrintNo))
            {
                if (!string.IsNullOrEmpty(materialPrintNo))
                {
                    if (HikRobotForm.PringLogs.Count > 100)
                        HikRobotForm.PringLogs.Clear();
                    HikRobotForm.PringLogs.Add(materialPrintNo);

                    var printOrderNo = orderNo.PadLeft(4, '0');

                    HikSetup.PrintLabel(string.Format("P/N:{0}", logPartNo),
                                                             string.Format("L/N:{0}", logPartNo),
                                                             string.Format("BIN:{0}", supplyLedGroup),
                                                             string.Format("PNO:{0}", materialNo),
                                                             string.Format("SUP:{0}", supplyNo),
                                                             string.Format("QTY:{0}", stockInCount),
                                                             string.Format("D/C:{0}", dcNo.Substring(0, 6)),
                                                             string.Format("NO.:{0}", printOrderNo.Substring(printOrderNo.Length - 4, 4)), findStockIn.Qualevel,
                                                             materialPrintNo);

                    var toSapError = string.Empty;
                    if (HikRobotForm.TryToSap(materialPrintNo, _trayNo, ref toSapError))
                    {
                        txtGeneratedBarcode.Text = string.Format("{0}\r\n", materialPrintNo);
                        txtGeneratedBarcode.BackColor = Color.Green;
                    }
                    else
                    {
                        txtGeneratedBarcode.Text = string.Format("{0}: 二维码={1}，{2}。",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), materialPrintNo, toSapError);
                        txtGeneratedBarcode.BackColor = Color.Red;
                    }
                }
                else
                {
                    txtGeneratedBarcode.Text = string.Format("{0}: 生成失败，二维码为空。",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    txtGeneratedBarcode.BackColor = Color.Red;
                }
            }
            else
            {
                txtGeneratedBarcode.Text = string.Format("{0}: 生成失败。",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                txtGeneratedBarcode.BackColor = Color.Red;
            }
        }
    }
}
