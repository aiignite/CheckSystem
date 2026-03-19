using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CheckSystem.Yfas.Utility;
using Newtonsoft.Json;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.TestFlow
{
    public partial class FormParasConfigPage : UIPage
    {
        private Utility._3TierModel.YfasProductInfo _productModel = new Utility._3TierModel.YfasProductInfo();
        private readonly List<Utility._3TierModel.YfasProductParas> _yfasProductParas = new List<Utility._3TierModel.YfasProductParas>();
        private readonly List<Utility._3TierModel.YfasDetectionBase> _yfasDetectionBaseModels = new List<Utility._3TierModel.YfasDetectionBase>();
        private readonly Utility._3TierBll.YfasProductParas _yfasProductParasBll = new Utility._3TierBll.YfasProductParas();
        private readonly Utility._3TierBll.YfasDetectionBase _yfasDetectionBaseBll = new Utility._3TierBll.YfasDetectionBase();

        public string EditValue = string.Empty;

        public FormParasConfigPage(int productId)
        {
            InitializeComponent();
            InitParas(productId);
        }

        public FormParasConfigPage(int productId, YfasCheckStateMachine.ExcDetect obj)
        {
            InitializeComponent();
            InitParas(productId, obj);
        }

        private void InitParas(int productId, YfasCheckStateMachine.ExcDetect obj = null)
        {
            _productModel = new Utility._3TierBll.YfasProductInfo().GetModel(productId);

            _yfasProductParas.Clear();
            _yfasProductParas.AddRange(_yfasProductParasBll.GetModelList("ProductId = '" + productId + "'"));

            _yfasDetectionBaseModels.Clear();

            var temp =
                _yfasDetectionBaseBll.GetModelList(string.Format("IsDelete = '0' and Row = '{0}'",
                    _productModel.RowIndex));

            foreach (var t in _yfasProductParas)
                _yfasDetectionBaseModels.Add(temp.Find(f => f.id == t.DetectionId));

            //_yfasDetectionBaseModels.AddRange(_yfasDetectionBaseBll.GetModelList(string.Format("IsDelete = '0' and Row = '{0}'", _productModel.RowIndex)));

            uiTransferParas.ItemsLeft.Clear();
            uiTransferParas.ItemsRight.Clear();

            foreach (var t in _yfasDetectionBaseModels)
            {
                if (obj == null)
                {
                    uiTransferParas.ItemsLeft.Add(t);
                }
                else
                {
                    if (obj.ProductDetections.ToList().Select(f => f.DetectionName).Contains(t.DetectionName))
                    {
                        uiTransferParas.ItemsRight.Add(t);
                    }
                    else
                    {
                        uiTransferParas.ItemsLeft.Add(t);
                    }
                }
            }

            uiTransferParas.ListBoxLeft.DisplayMember = "DetectionName";
            uiTransferParas.ListBoxRight.DisplayMember = "DetectionName";
        }

        private void tsbtnConfirm_Click(object sender, EventArgs e)
        {
            if (uiTransferParas.ItemsRight.Count == 0)
            {
                this.ShowErrorTip("必须选择至少一个参数");
            }
            else
            {
                var paras = uiTransferParas.ItemsRight.Cast<Utility._3TierModel.YfasDetectionBase>().ToList();
                EditValue = JsonConvert.SerializeObject(new YfasCheckStateMachine.ExcDetect { ProductId = _productModel.id, ProductDetections = paras.ToArray() });
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void tsbtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
