using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CheckSystem.Yfas.Utility;
using Newtonsoft.Json;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.TestFlow
{
    public partial class FormFuncConfigPage : UIPage
    {
        private Utility._3TierModel.YfasProductInfo _productModel = new Utility._3TierModel.YfasProductInfo();
        public string EditValue = string.Empty;
        private int _checkedFuncId = -1;

        private readonly List<Utility._3TierModel.YfasPreProgramFunc> _yfasPreProgramFuncModels = new List<Utility._3TierModel.YfasPreProgramFunc>();
        private readonly Utility._3TierBll.YfasPreProgramFunc _yfasPreProgramFuncBll = new Utility._3TierBll.YfasPreProgramFunc();

        public FormFuncConfigPage(int productId)
        {
            InitializeComponent();
            flowLayoutPanel1.AutoScroll = true;
            LoadFuncs(productId);
        }

        public FormFuncConfigPage(int productId, YfasCheckStateMachine.ExcFunc obj)
        {
            InitializeComponent();
            LoadFuncs(productId, obj);
        }

        private void LoadFuncs(int productId, YfasCheckStateMachine.ExcFunc obj = null)
        {
            _productModel = new Utility._3TierBll.YfasProductInfo().GetModel(productId);

            _yfasPreProgramFuncModels.Clear();
            //_yfasPreProgramFuncModels.AddRange(_yfasPreProgramFuncBll.GetModelList(string.Format("Row = '{0}' and CarModel = '{1}'", _productModel.RowIndex, _productModel.CarMode)));
            _yfasPreProgramFuncModels.AddRange(_yfasPreProgramFuncBll.GetModelList(""));

            int[] maxLen = { 0 };
            foreach (var t in _yfasPreProgramFuncModels.Where(t => maxLen[0] <= t.Name.Length))
                maxLen[0] = t.Name.Length;

            foreach (var t in _yfasPreProgramFuncModels)
            {
                var uiRadio = new UIRadioButton { Name = t.id.ToString(), Text = t.Name };
                uiRadio.CheckedChanged += uiRadio_CheckedChanged;
                uiRadio.Width = (int)(maxLen[0] * (uiRadio.Font.Size * 2));
                flowLayoutPanel1.Controls.Add(uiRadio);

                if (obj != null)
                {
                    if (t.Name == obj.Func.FuncName)
                    {
                        uiRadio.Checked = true;
                        uiTextBox2.Text = obj.Func.TimeOut.ToString();
                    }
                }
            }
        }

        private void uiRadio_CheckedChanged(object sender, EventArgs e)
        {
            var uiRadioButton = sender as UIRadioButton;
            if (uiRadioButton != null) _checkedFuncId = int.Parse(uiRadioButton.Name);
        }

        private void tsbtnConfirm_Click(object sender, EventArgs e)
        {
            if (_checkedFuncId == -1 || string.IsNullOrEmpty(uiTextBox2.Text))
            {
                this.ShowErrorTip("请填写超时时间或选择输入信号");
            }
            else
            {
                var funcModel = _yfasPreProgramFuncModels.Find(f => f.id == _checkedFuncId);
                Debug.Assert(funcModel.TimeOutMs != null, "funcModel.TimeOutMs != null");
                var func = new YfasCheckStateMachine.ExcFunc(new YfasCheckStateMachine.PreProgramFunc { FuncName = funcModel.Name, Id = funcModel.id, TimeOut = uiTextBox2.IntValue });

                EditValue = JsonConvert.SerializeObject(func);
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
