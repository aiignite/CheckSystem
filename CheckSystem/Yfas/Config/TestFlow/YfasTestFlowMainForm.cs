using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CheckSystem.Yfas.Utility;
using Newtonsoft.Json;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.TestFlow
{
    public partial class YfasTestFlowMainForm : UIForm
    {
        private Utility._3TierModel.YfasProductInfo _productModel = new Utility._3TierModel.YfasProductInfo();
        private readonly List<Utility._3TierModel.YfasProductParas> _yfasProductParaModels = new List<Utility._3TierModel.YfasProductParas>();
        private readonly List<Utility._3TierModel.YfasDetectionBase> _yfasDetectionBaseModels = new List<Utility._3TierModel.YfasDetectionBase>();

        private readonly Utility._3TierBll.YfasProductParas _yfasProductParasBll = new Utility._3TierBll.YfasProductParas();
        private readonly Utility._3TierBll.YfasDetectionBase _yfasDetectionBaseBll = new Utility._3TierBll.YfasDetectionBase();

        //private readonly TreeNode _baseNode = new TreeNode { Text = @"OnStart" };
        private int _productId;

        public YfasTestFlowMainForm(int productId)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            InitImg();
            InitParas(productId);
            YfasHelper.CreateTreeView(uiTreeView1, _productModel.TreeView);
            OnPreView();
        }

        private void InitImg()
        {
            uiTreeView1.ImageList = YfasDeviceBase.Images;
        }

        private void InitParas(int productId)
        {
            _productId = productId;
            _productModel = new Utility._3TierBll.YfasProductInfo().GetModel(productId);

            _yfasProductParaModels.Clear();
            _yfasProductParaModels.AddRange(_yfasProductParasBll.GetModelList("ProductId = '" + productId + "'"));

            _yfasDetectionBaseModels.Clear();
            _yfasDetectionBaseModels.AddRange(_yfasDetectionBaseBll.GetModelList(string.Format("IsDelete = '0' and Row = '{0}'", _productModel.RowIndex)));
        }

        #region toolStripBtnClick

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnAddNode_Click(object sender, EventArgs e)
        {
            var configFrm = new YfasTestFlowConfigEditForm(_productId);
            if (configFrm.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(configFrm.EditValue))
            {
                var node = new TreeNode(JsonToShowString(configFrm.EditType, configFrm.EditValue), configFrm.EditType, configFrm.EditType) { Tag = configFrm.EditValue };
                uiTreeView1.SelectedNode.Nodes.Add(node);
                uiTreeView1.SelectedNode.Expand();
                OnPreView();
                //var node = new TreeNode(configFrm.EditValue, configFrm.EditType, configFrm.EditType);
                //uiTreeView1.SelectedNode.Nodes.Add(node);
                //uiTreeView1.ExpandAll();
            }
        }

        /// <summary>
        /// 修改节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnEdid_Click(object sender, EventArgs e)
        {
            if (uiTreeView1.SelectedNode != null &&
                uiTreeView1.SelectedNode.Text != @"OnStart")
            {
                var configFrm =
                    new YfasTestFlowConfigEditForm(
                        _productId, true, uiTreeView1.SelectedNode.ImageIndex, uiTreeView1.SelectedNode.Tag.ToString());
                if (configFrm.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(configFrm.EditValue))
                {
                    uiTreeView1.SelectedNode.Text = JsonToShowString(configFrm.EditType, configFrm.EditValue);
                    uiTreeView1.SelectedNode.Tag = configFrm.EditValue;
                    OnPreView();
                }
            }
            else
            {
                this.ShowErrorTip("这不是一个可以修改的节点！");
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnDeleteNode_Click(object sender, EventArgs e)
        {
            if (uiTreeView1.SelectedNode != null &&
                uiTreeView1.SelectedNode.Text != @"OnStart")
            {
                if (this.ShowAskDialog(string.Format("是否删除此行及所有子节点：\r\n{0}", uiTreeView1.SelectedNode.Text)))
                {
                    uiTreeView1.Nodes.Remove(uiTreeView1.SelectedNode);
                    //OnPreView();
                }
            }
            else
            {
                this.ShowErrorTip("这不是一个可以删除的节点！");
            }
        }

        /// <summary>
        /// 展开所有节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnExpandAll_Click(object sender, EventArgs e)
        {
            uiTreeView1.ExpandAll();
            OnPreView();
        }

        /// <summary>
        /// 折叠所有节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnCollapseAll_Click(object sender, EventArgs e)
        {
            uiTreeView1.CollapseAll();
            OnPreView();
        }

        /// <summary>
        /// 节点上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnMoveUp_Click(object sender, EventArgs e)
        {
            if (uiTreeView1.SelectedNode != null &&
                uiTreeView1.SelectedNode.Text != @"OnStart")
            {
                var node = uiTreeView1.SelectedNode;
                var prevNode = node.PrevNode;

                if (prevNode != null)
                {
                    var newNode = (TreeNode)node.Clone();
                    node.Parent.Nodes.Insert(prevNode.Index, newNode);

                    node.Remove();
                    uiTreeView1.SelectedNode = newNode;
                }

                OnPreView();
            }

        }

        /// <summary>
        /// 节点下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnMoveDown_Click(object sender, EventArgs e)
        {
            if (uiTreeView1.SelectedNode != null &&
                uiTreeView1.SelectedNode.Text != @"OnStart")
            {
                var node = uiTreeView1.SelectedNode;
                var nextNode = node.NextNode;

                if (nextNode != null)
                {
                    var newNode = (TreeNode)node.Clone();
                    node.Parent.Nodes.Insert(nextNode.Index + 1, newNode);

                    node.Remove();
                    uiTreeView1.SelectedNode = newNode;
                }

                OnPreView();
            }
        }

        /// <summary>
        /// 节点左移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnMoveLeft_Click(object sender, EventArgs e)
        {
            if (uiTreeView1.SelectedNode != null &&
                uiTreeView1.SelectedNode.Text != @"OnStart")
            {
                var node = uiTreeView1.SelectedNode;

                if (node.Parent != null && node.Parent.Text != @"OnStart")
                {
                    var newNode = (TreeNode)node.Clone();

                    if (node.Level == 1)
                    {
                        uiTreeView1.Nodes.Insert(node.Parent.Index + 1, newNode);
                    }
                    else
                    {
                        node.Parent.Parent.Nodes.Insert(node.Parent.Index + 1, newNode);
                    }

                    node.Remove();
                    uiTreeView1.SelectedNode = newNode;

                    OnPreView();
                }
            }
        }

        /// <summary>
        /// 节点右移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnMoveRight_Click(object sender, EventArgs e)
        {
            if (uiTreeView1.SelectedNode != null &&
                uiTreeView1.SelectedNode.Text != @"OnStart")
            {
                var node = uiTreeView1.SelectedNode;

                if (node.PrevNode != null)
                {
                    var newNode = (TreeNode)node.Clone();
                    node.PrevNode.Nodes.Insert(node.PrevNode.Nodes.Count, newNode);
                    uiTreeView1.SelectedNode = newNode;
                    node.Remove();
                    OnPreView();
                }
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            if (this.ShowAskDialog("确认保存？"))
            {
                uiTreeView1.ExpandAll();
                var treeState = new XmlTreeViewState();
                treeState.SaveTreeViewState(uiTreeView1, YfasDeviceBase.XmlFileNameSaved);

                var temp = string.Empty;

                using (var sr = new StreamReader(YfasDeviceBase.XmlFileNameSaved))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        try
                        {
                            temp += line;
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }

                File.Delete(YfasDeviceBase.XmlFileNameSaved);

                _productModel.TreeView = temp;
                var bll = new Utility._3TierBll.YfasProductInfo();
                bll.Update(_productModel);

                //ShowInfoDialog("保存结束，请重新打开");
                YfasHelper.CreateTreeView(uiTreeView1, _productModel.TreeView);
                OnPreView();
            }
        }


        private void TsbtnCopyFrom_Click(object sender, EventArgs e)
        {
            var bll = new Utility._3TierBll.YfasProductInfo();
            var models = bll.GetModelList("id != '" + _productModel.id + "'");

            if (models.Any())
            {
                var option = new UIEditOption
                {
                    AutoLabelWidth = true,
                    Text = "添加"
                };
                option.AddCombobox("Product", "Product", models, "Name", "id", models[0].id);

                var frm = new UIEditForm(option);
                frm.ShowDialog();

                if (frm.IsOK)
                {
                    var pid = (int)frm["Product"];
                    var copyFromModel = new Utility._3TierBll.YfasProductInfo().GetModel(pid);
                    _productModel.TreeView = copyFromModel.TreeView;
                    new Utility._3TierBll.YfasProductInfo().Update(_productModel);

                    uiTreeView1.ExpandAll();
                    var treeState = new XmlTreeViewState();
                    treeState.SaveTreeViewState(uiTreeView1, YfasDeviceBase.XmlFileNameSaved);

                    var temp = string.Empty;

                    using (var sr = new StreamReader(YfasDeviceBase.XmlFileNameSaved))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            try
                            {
                                temp += line;
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                        }
                    }

                    File.Delete(YfasDeviceBase.XmlFileNameSaved);

                    //ShowInfoDialog("保存结束，请重新打开");
                    YfasHelper.CreateTreeView(uiTreeView1, _productModel.TreeView);
                    OnPreView();
                }
            }
        }

        #endregion

        private void OnPreView()
        {
            uiTreeView1.BeginUpdate();

            for (var i = 0; i < uiTreeView1.Nodes[0].Nodes.Count; i++)
            {
                var tn = uiTreeView1.Nodes[0].Nodes[i];
                ShowIndex((i + 1).ToString(), tn);
            }

            uiTreeView1.EndUpdate();
        }

        private void OnPreView(TreeNode node)
        {
            //ShowWaitForm();
            //uiTreeView1.BeginUpdate();

            for (var i = 0; i < node.Nodes.Count; i++)
            {
                var tn = uiTreeView1.Nodes[0].Nodes[i];
                ShowIndex((i + 1).ToString(), tn);
            }

            //uiTreeView1.EndUpdate();
            //HideWaitForm();
        }

        private void ShowIndex(string i, TreeNode tn)
        {
            var txt = string.Format("{0}  {1}", i, JsonToShowString(tn.ImageIndex, tn.Tag.ToString()));
            if (tn.Text != txt)
            {
                tn.Text = txt;
            }

            for (var j = 0; j < tn.Nodes.Count; j++)
            {
                var tnSub = tn.Nodes[j];
                ShowIndex(i + "." + (j + 1), tnSub);
            }
        }

        public string JsonToShowString(int type, string jsonValue)
        {
            var str = string.Empty;

            if (type == (int)YfasCheckStateMachine.YfasTreeNodeType.Action)
                str = string.Format("{0}",
                    ActionToString(JsonConvert.DeserializeObject<YfasCheckStateMachine.ExcActions>(jsonValue)));
            else if (type == (int)YfasCheckStateMachine.YfasTreeNodeType.Func)
                str = string.Format("{0}",
                    FuncToString(JsonConvert.DeserializeObject<YfasCheckStateMachine.ExcFunc>(jsonValue)));
            else if (type == (int)YfasCheckStateMachine.YfasTreeNodeType.Para)
                str = string.Format("{0}",
                    ParaToString(JsonConvert.DeserializeObject<YfasCheckStateMachine.ExcDetect>(jsonValue)));

            return str;
        }

        public static string ActionToString(YfasCheckStateMachine.ExcActions excActions)
        {
            var str = string.Format("进入执行：[{0}]；", excActions.Enter.Aggregate(string.Empty, (current, t) => current + t.ActionName + "，").TrimEnd('，'));
            if (excActions.Exit == null || !excActions.Exit.Any())
                return str;
            str += " ";
            str += string.Format("退出执行：[{0}]；", excActions.Exit.Aggregate(string.Empty, (current, t) => current + t.ActionName + "，").TrimEnd('，'));
            return str;
        }

        public static string FuncToString(YfasCheckStateMachine.ExcFunc excFunc)
        {
            return string.Format("等待：[{0}]；超时：[{1}ms]", excFunc.Func.FuncName, excFunc.Func.TimeOut);
        }

        public static string ParaToString(YfasCheckStateMachine.ExcDetect excDetect)
        {
            return string.Format("检测项：[{0}]；", excDetect.ProductDetections.Aggregate(string.Empty, (current, t) => current + t.DetectionName + "，").TrimEnd('，'));
        }
    }
}
