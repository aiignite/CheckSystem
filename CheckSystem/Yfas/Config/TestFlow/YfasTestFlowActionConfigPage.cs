using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CheckSystem.Yfas.Utility;
using Newtonsoft.Json;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.TestFlow
{
    public partial class FormActionConfigPage : UIPage
    {
        private Utility._3TierModel.YfasProductInfo _productModel = new Utility._3TierModel.YfasProductInfo();
        private readonly List<Utility._3TierModel.YfasPreProgramAction> _yfasPreProgramActionModels = new List<Utility._3TierModel.YfasPreProgramAction>();
        private readonly Utility._3TierBll.YfasPreProgramAction _yfasPreProgramActionBll = new Utility._3TierBll.YfasPreProgramAction();

        public string EditValue = string.Empty;

        public FormActionConfigPage(int productId)
        {
            InitializeComponent();
            InitActions(productId, null);
        }

        public FormActionConfigPage(int productId, YfasCheckStateMachine.ExcActions obj)
        {
            InitializeComponent();
            InitActions(productId, obj);
        }

        private void InitActions(int productId, YfasCheckStateMachine.ExcActions obj)
        {
            _productModel = new Utility._3TierBll.YfasProductInfo().GetModel(productId);

            _yfasPreProgramActionModels.Clear();
            //_yfasPreProgramActionModels.AddRange(_yfasPreProgramActionBll.GetModelList(string.Format("Row = '{0}' and CarModel = '{1}'", _productModel.RowIndex, _productModel.CarMode)));
            _yfasPreProgramActionModels.AddRange(_yfasPreProgramActionBll.GetModelList(""));

            uiTransferEnter.ItemsLeft.Clear();
            uiTransferEnter.ItemsRight.Clear();
            uiTransferExit.ItemsLeft.Clear();
            uiTransferExit.ItemsRight.Clear();

            foreach (var t in _yfasPreProgramActionModels)
            {
                if (obj == null)
                {
                    uiTransferEnter.ItemsLeft.Add(t);
                    uiTransferExit.ItemsLeft.Add(t);
                }
                else
                {
                    if (obj.Enter.ToList().Select(f => f.ActionName).Contains(t.Name))
                    {
                        uiTransferEnter.ItemsRight.Add(t);
                    }
                    else
                    {
                        uiTransferEnter.ItemsLeft.Add(t);
                    }

                    if (obj.Exit == null)
                    {
                        uiTransferExit.ItemsLeft.Add(t);
                    }
                    else
                    {
                        if (obj.Exit.ToList().Select(f => f.ActionName).Contains(t.Name))
                        {
                            uiTransferExit.ItemsRight.Add(t);
                        }
                        else
                        {
                            uiTransferExit.ItemsLeft.Add(t);
                        }
                    }
                }
            }

            uiTransferEnter.ListBoxLeft.DisplayMember = "Name";
            uiTransferEnter.ListBoxRight.DisplayMember = "Name";

            uiTransferExit.ListBoxLeft.DisplayMember = "Name";
            uiTransferExit.ListBoxRight.DisplayMember = "Name";
        }

        private void tsbtnConfirm_Click(object sender, EventArgs e)
        {
            if (uiTransferEnter.ItemsRight.Count == 0)
            {
                this.ShowErrorTip("进入动作不能不选");
            }
            else
            {
                var enterActsModels = uiTransferEnter.ItemsRight.Cast<Utility._3TierModel.YfasPreProgramAction>().ToList();
                var exitActsModels = uiTransferExit.ItemsRight.Cast<Utility._3TierModel.YfasPreProgramAction>().ToList();

                var actions =
                    new YfasCheckStateMachine.ExcActions(
                        enterActsModels.Select(
                            t =>
                                new YfasCheckStateMachine.PreProgramAction
                                {
                                    ActionName = t.Name,
                                    Id = t.id,
                                    //IsDelete = 0
                                }).ToArray(),
                        exitActsModels.Select(
                            t =>
                                new YfasCheckStateMachine.PreProgramAction
                                {
                                    ActionName = t.Name,
                                    Id = t.id,
                                    //IsDelete = 0
                                }).ToArray());
                EditValue = JsonConvert.SerializeObject(actions);
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void tsbtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void tsbtnShowAction_Click(object sender, EventArgs e)
        {
            //var newUiForm = new UIForm();
            //newUiForm.Controls.Add(new YfasPreProgramPage());
            ////newUiForm.AddPage(new YfasPreProgramPage(), 1001);
            //newUiForm.ShowDialog();
        }
    }
}
