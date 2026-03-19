using System;
using System.Windows.Forms;
using CheckSystem.Yfas.Utility;
using Newtonsoft.Json;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.TestFlow
{
    public sealed partial class YfasTestFlowConfigEditForm : UIAsideMainFrame
    {
        private readonly FormActionConfigPage _actionConfigPage;
        private readonly FormParasConfigPage _parasConfigPage;
        private readonly FormFuncConfigPage _waitConfigPage;

        public int EditType = -1;
        public string EditValue = string.Empty;

        public YfasTestFlowConfigEditForm(int productId, bool isEdit = false, int editType = 0, string obj = null)
        {
            InitializeComponent();
            
            if (!isEdit)
            {
                Style = UIStyle.Orange;
                Text = @"新增";

                _parasConfigPage = new FormParasConfigPage(productId);
                _waitConfigPage = new FormFuncConfigPage(productId);
                _actionConfigPage = new FormActionConfigPage(productId);

                _actionConfigPage.Closed += _actionConfigPage_Closed;
                _parasConfigPage.Closed += _parasConfigPage_Closed;
                _waitConfigPage.Closed += _waitConfigPage_Closed;
                AddPage(_actionConfigPage, 1001);
                AddPage(_waitConfigPage, 1002);
                AddPage(_parasConfigPage, 1003);

                Aside.CreateNode("动作", 1001);
                Aside.CreateNode("等待信号", 1002);
                Aside.CreateNode("检测项", 1003);
            }
            else
            {
                Style = UIStyle.Green;
                Text = @"修改";

                if (editType == (int)YfasCheckStateMachine.YfasTreeNodeType.Action)
                {
                    _actionConfigPage = new FormActionConfigPage(productId, JsonConvert.DeserializeObject<YfasCheckStateMachine.ExcActions>(obj));
                    _actionConfigPage.Closed += _actionConfigPage_Closed;
                    AddPage(_actionConfigPage, 1001);
                    Aside.CreateNode("动作", 1001);
                }
                else if (editType == (int)YfasCheckStateMachine.YfasTreeNodeType.Func)
                {
                    _waitConfigPage = new FormFuncConfigPage(productId, JsonConvert.DeserializeObject<YfasCheckStateMachine.ExcFunc>(obj));
                    _waitConfigPage.Closed += _waitConfigPage_Closed;
                    AddPage(_waitConfigPage, 1002);
                    Aside.CreateNode("等待信号", 1002);
                }
                else if (editType == (int)YfasCheckStateMachine.YfasTreeNodeType.Para)
                {
                    _parasConfigPage = new FormParasConfigPage(productId, JsonConvert.DeserializeObject<YfasCheckStateMachine.ExcDetect>(obj));
                    _parasConfigPage.Closed += _parasConfigPage_Closed;
                    AddPage(_parasConfigPage, 1003);
                    Aside.CreateNode("检测项", 1003);
                }
            }

            Aside.SelectFirst();
        }

        private void _waitConfigPage_Closed(object sender, EventArgs e)
        {
            if (_actionConfigPage != null)
            {
                _actionConfigPage.Closed -= _actionConfigPage_Closed;
            }

            if (_parasConfigPage != null)
            {
                _parasConfigPage.Closed -= _parasConfigPage_Closed;
            }

            page_Closed(sender);
        }

        private void _parasConfigPage_Closed(object sender, EventArgs e)
        {
            if (_actionConfigPage != null)
            {
                _actionConfigPage.Closed -= _actionConfigPage_Closed;
            }

            if (_waitConfigPage != null)
            {
                _waitConfigPage.Closed -= _waitConfigPage_Closed;
            }

            page_Closed(sender);
        }

        private void _actionConfigPage_Closed(object sender, EventArgs e)
        {
            if (_parasConfigPage != null)
            {
                _parasConfigPage.Closed -= _parasConfigPage_Closed;
            }

            if (_waitConfigPage != null)
            {
                _waitConfigPage.Closed -= _waitConfigPage_Closed;
            }

            page_Closed(sender);
        }

        private void page_Closed(object sender)
        {
            if (_actionConfigPage != null && sender.GetType() == _actionConfigPage.GetType())
            {
                EditType = (int)YfasCheckStateMachine.YfasTreeNodeType.Action;
                EditValue = _actionConfigPage.EditValue;
            }
            else if (_parasConfigPage != null && sender.GetType() == _parasConfigPage.GetType())
            {
                EditType = (int)YfasCheckStateMachine.YfasTreeNodeType.Para;
                EditValue = _parasConfigPage.EditValue;
            }
            else if (_waitConfigPage != null && sender.GetType() == _waitConfigPage.GetType())
            {
                EditType = (int)YfasCheckStateMachine.YfasTreeNodeType.Func;
                EditValue = _waitConfigPage.EditValue;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }

            var uiPage = sender as UIPage;
            DialogResult = uiPage != null ? uiPage.DialogResult : DialogResult.Cancel;
            Close();
        }
    }
}
