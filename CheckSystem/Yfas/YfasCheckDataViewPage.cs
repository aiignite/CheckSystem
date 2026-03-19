using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CheckSystem.Yfas.Utility;
using Newtonsoft.Json;
using Sunny.UI;

namespace CheckSystem.Yfas
{
    public partial class YfasCheckDataViewPage : UIPage
    {
        public YfasCheckDataViewPage()
        {
            InitializeComponent();
            Load += YfasCheckDateViewPage_Load;
            //uiTreeView1.AfterCollapse += uiTreeView1_AfterCollapse;
            uiTreeView1.BeforeCheck += uiTreeView1_BeforeCheck;
            uiTreeView1.BeforeCollapse += uiTreeView1_BeforeCollapse;
            uiTreeView1.AfterCheck += uiTreeView1_AfterCheck;
            uiTreeView1.BeforeSelect += uiTreeView1_BeforeSelect;
            YfasCheckStateMachine.PushCheckDataView += YfasCheckStateMachine_PushDisplay;
            YfasCheckStateMachine.PushEnd += YfasCheckStateMachine_PushEnd;
            YfasCheckStateMachine.PushWaitInfo += YfasCheckStateMachine_PushWaitInfo;
            maintTable.CellFormatting += maintTable_CellFormatting;

            //TreeNode tr1=new TreeNode(){Text = "sasdas"};
            //var tr2=(TreeNode)tr1.Clone();

            adminPreTreeView.Nodes.Add(new TreeNode("OnStart"));
        }

        private void YfasCheckDateViewPage_Load(object sender, EventArgs e)
        {
            SetCheckUiColor(0);
            uiSplitContainer1.Collapse();
            uiTreeView1.ExpandAll();

            if (YfasDeviceBase.Limit == YfasDeviceBase.LimitType.User)
            {
                uiTreeView1.CheckBoxes = false;
                uiSplitContainer3.Panel2Collapsed = true;
                adminPreTreeView.Visible = false;
            }
        }

        private readonly List<YfasCheckStateMachine.CheckDataView> _checkDataViewList = new List<YfasCheckStateMachine.CheckDataView>();
        private int _thisProductId = -1;
        public bool IsChecking;
        private int _retryCount = -1;
        private bool _isRetry;
        //private TreeNode _retryNode = new TreeNode();

        public void ReLoadDetection(int productId)
        {
            if (IsChecking)
                return;

            ResetTimer();

            maintTable.ReadOnly = true;
            maintTable.RowHeadersVisible = false;
            maintTable.AllowUserToAddRows = false;
            maintTable.AllowUserToResizeRows = false;
            maintTable.AllowUserToOrderColumns = false;
            maintTable.MultiSelect = true;
            maintTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            SetCheckUiColor(0);
            _thisProductId = productId;
            _checkDataViewList.Clear();
            maintTable.ClearRows();

            var productBll = new Utility._3TierBll.YfasProductInfo();
            var productModel = productBll.GetModel(productId);
            var prodcutCheckDataBll = new Utility._3TierBll.YfasCheckData();

            var okCount = prodcutCheckDataBll.GetList(
                      string.Format(
                          "productNo = '{0}' and checkResult = '{1}' and DATEDIFF(DD,createTime,GETDATE()) = 0",
                          _thisProductId, "0001")).Tables[0].DefaultView.Count;

            var totalCount = prodcutCheckDataBll.GetList(
                string.Format(
                    "productNo = '{0}' and DATEDIFF(DD,createTime,GETDATE()) = 0",
                    _thisProductId)).Tables[0].DefaultView.Count;

            uiLabel2.Text = string.Format("{0}/{1}", okCount, totalCount);

            if (productModel != null)
            {
                YfasHelper.CreateTreeView(uiTreeView1, productModel.TreeView);

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
                maintTable.ClearRows();
                maintTable.DataSource = _checkDataViewList;
                maintTable.AutoResizeColumns();
            }
        }

        private void StartTimer()
        {
            IsChecking = true;

            //获取控件的Type,设置双缓存
            var dgvType = uiLabel1.GetType();
            var properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //为控件的属性设置值
            if (properInfo != null) properInfo.SetValue(uiLabel1, true, null);

            uiMillisecondTimer1.Start();
            //uiLedStopwatch1.Active = true;
        }

        private void StopTimer()
        {
            IsChecking = false;
            uiMillisecondTimer1.Stop();
            //uiLedStopwatch1.Active = false;
        }

        public void ResetTimer()
        {
            //uiLedStopwatch1.Text = @"00:00";
            uiLabel1.Text = 0.ToString();
        }

        public void OnStart()
        {
            //if (_thisProductId == -1 || IsChecking)
            //    return;
            //ReLoadDetection(_thisProductId);
            //IsChecking = true;
            //adminToolStrip.Enabled = false;
            //var newStateMachine = new YfasCheckStateMachine(uiTreeView1.Nodes[0]) { IsByPass = isByPass };
            //newStateMachine.OnBegin(_thisProductId);
            //SetCheckUiColor(4);
            OnStart(false, uiTreeView1.Nodes[0]);
        }

        private void OnStart(bool isManual, TreeNode treeNode)
        {
            if (_thisProductId == -1 || IsChecking)
                return;
            _retryCount = isManual ? 0 : 1;
            ReLoadDetection(_thisProductId);
            StartTimer();
            adminToolStrip.Enabled = false;
            var newStateMachine = new YfasCheckStateMachine(treeNode) { IsByPass = true };
            newStateMachine.OnBegin(_thisProductId);
            SetCheckUiColor(4);
        }

        public void OnEnd()
        {
            // save data
            // show ok/ng
            // show ok/total count

            BeginInvoke(new Action(() =>
            {
                if (_retryCount == 0)
                {
                    _isRetry = false;
                    _retryCount = -1;
                    var isNg = true;

                    if (_checkDataViewList.FindAll(f => f.IsNotNa == "Y" && (f.Result == "NG" || string.IsNullOrEmpty(f.Result))).Any())
                    {
                        //_checkDataViewList = _checkDataViewList.OrderByDescending(f => f.Result).ToList();
                        SetCheckUiColor(2);
                    }
                    else
                    {
                        isNg = false;
                        SetCheckUiColor(1);
                    }

                    var showData = _checkDataViewList.FindAll(f => f.IsNotNa == "Y" && !string.IsNullOrEmpty(f.Result));
                    showData.AddRange(_checkDataViewList.FindAll(f => f.IsNotNa != "Y"));

                    if (showData.Count > 0)
                    {
                        maintTable.ClearRows();
                        maintTable.DataSource = showData;
                        maintTable.FirstDisplayedScrollingRowIndex = maintTable.RowCount - 1;
                    }

                    StopTimer();
                    adminToolStrip.Enabled = true;

                    // SaveData
                    var uids = _checkDataViewList.FindAll(f => f.IsNotNa == "Y" && f.IsUid);
                    var saveModels = new List<Utility._3TierModel.YfasCheckData>();
                    var productModel = new Utility._3TierBll.YfasProductInfo().GetModel(_thisProductId);
                    if (uids.Count == 0)
                    {
                        saveModels.Add(new Utility._3TierModel.YfasCheckData
                        {
                            checkData = JsonConvert.SerializeObject(_checkDataViewList.FindAll(f => f.IsNotNa == "Y")),
                            checkStaffNo = "admin",
                            productNo = _thisProductId.ToString(),
                            creater = productModel.Name + "_" + productModel.YfasPn,
                            checkResult = isNg ? "0002" : "0001",
                            checkDate = DateTime.Now,
                            createTime = DateTime.Now
                        });
                    }
                    else
                    {
                        foreach (var t in uids)
                        {
                            saveModels.Add(new Utility._3TierModel.YfasCheckData
                            {
                                checkData = JsonConvert.SerializeObject(_checkDataViewList.FindAll(f => f.IsNotNa == "Y")),
                                checkStaffNo = "admin",
                                productNo = _thisProductId.ToString(),
                                creater = productModel.Name + "_" + productModel.YfasPn + "_" + productModel.GmPn + "_" + productModel.RowIndex + "_" + productModel.Pos,
                                checkResult = isNg ? "0002" : "0001",
                                productUid = t.Data,
                                checkDate = DateTime.Now,
                                createTime = DateTime.Now
                            });
                        }
                    }

                    var bll = new Utility._3TierBll.YfasCheckData();
                    foreach (var m in saveModels)
                    {
                        bll.Add(m);
                    }

                    var okCount = bll.GetList(
                        string.Format(
                            "productNo = '{0}' and checkResult = '{1}' and DATEDIFF(DD,createTime,GETDATE()) = 0",
                            _thisProductId, "0001")).Tables[0].DefaultView.Count;

                    var totalCount = bll.GetList(
                        string.Format(
                            "productNo = '{0}' and DATEDIFF(DD,createTime,GETDATE()) = 0",
                            _thisProductId)).Tables[0].DefaultView.Count;

                    uiLabel2.Text = string.Format("{0}/{1}", okCount, totalCount);
                }
                else
                {
                    _retryCount--;
                    _isRetry = true;

                    var ngDatas =
                        _checkDataViewList.FindAll(f => f.IsNotNa == "Y" && f.Result == "NG")
                            .ToList();

                    if (ngDatas.Count > 0)
                    {
                        var showNgStr = string.Empty;

                        foreach (var ng in ngDatas)
                        {
                            showNgStr += ng.Name + "\r\n";

                            for (var i = 0; i < uiTreeView1.Nodes[0].Nodes.Count; i++)
                                CreateRetryNode(uiTreeView1.Nodes[0].Nodes[i], ng.Name);
                        }

                        if (this.ShowAskDialog("是否需要复测NG项目？\r\n" + showNgStr))
                        {
                            var newStateMachine = new YfasCheckStateMachine(adminPreTreeView.Nodes[0]) { IsByPass = true };
                            newStateMachine.OnBegin(_thisProductId);
                        }
                        else
                        {
                            YfasCheckStateMachine_PushEnd();
                        }
                    }
                    else
                    {
                        YfasCheckStateMachine_PushEnd();
                    }
                }
            }));
        }

        private static void CreateRetryNode(TreeNode node, string paraName)
        {
            if (node.Text.Contains("检测项") && node.Text.Contains(paraName))
            {
                node.Checked = true;
            }
            else if (node.Nodes.Count > 0)
            {
                for (var i = 0; i < node.Nodes.Count; i++)
                {
                    CreateRetryNode(node.Nodes[i], paraName);
                }
            }
        }

        private void YfasCheckStateMachine_PushDisplay(YfasCheckStateMachine.CheckDataView value)
        {
            if (InvokeRequired && IsChecking)
            {
                BeginInvoke(new Action(() =>
                {
                    if (_isRetry)
                    {
                        var findIndex = _checkDataViewList.FindIndex(f => f.Name == value.Name && !string.IsNullOrEmpty(f.Result) && f.Result == "NG");
                        if (findIndex != -1)
                        {
                            _checkDataViewList[findIndex].Data = value.Data;
                            _checkDataViewList[findIndex].Result = value.Result;
                            _checkDataViewList[findIndex].CostTime = value.CostTime;
                        }
                    }
                    else
                    {
                        var findIndex = _checkDataViewList.FindIndex(f => f.Name == value.Name);
                        if (findIndex != -1)
                        {
                            _checkDataViewList[findIndex].Data = value.Data;
                            _checkDataViewList[findIndex].Result = value.Result;
                            _checkDataViewList[findIndex].CostTime = value.CostTime;
                        }
                    }


                    var totalCount =
                        _checkDataViewList.FindAll(
                            f => string.Equals(f.IsNotNa, "Y", StringComparison.CurrentCultureIgnoreCase)).Count;
                    var checkedCount = _checkDataViewList.FindAll(
                            f => string.Equals(f.IsNotNa, "Y", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(f.Result)).Count;

                    uiProcesAsBar1.Value = (int)((double)checkedCount / totalCount * 100);

                    maintTable.ClearRows();
                    var showData = _checkDataViewList.FindAll(f => f.IsNotNa == "Y" && !string.IsNullOrEmpty(f.Result));
                    if (showData.Count > 0)
                    {
                        maintTable.DataSource = showData;
                        maintTable.AutoResizeColumns();
                        maintTable.FirstDisplayedScrollingRowIndex = maintTable.RowCount - 1;
                    }
                }));
            }
        }

        private void YfasCheckStateMachine_PushEnd()
        {
            if (IsChecking)
                OnEnd();
        }

        private void YfasCheckStateMachine_PushWaitInfo(bool isShow, string value = "")
        {
            if (InvokeRequired && IsChecking)
            {
                BeginInvoke(new Action(() =>
                {
                    //if (isShow)
                    //    ShowWaitForm(value);
                    //else
                    //    HideWaitForm();
                }));
            }
        }

        private void maintTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (maintTable.Columns[e.ColumnIndex].HeaderText != @"Result")
                return;
            if (maintTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "NG")
                return;
            maintTable.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkRed;
            maintTable.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.DarkRed;
            maintTable.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
            maintTable.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.White;
        }

        /// <summary>
        /// 0=init,1=ok,2=ng,3=running
        /// </summary>
        /// <param name="type">0=init,1=ok,2=ng,3=running</param>
        private void SetCheckUiColor(int type)
        {
            switch (type)
            {
                case 0:
                    //uiLight1.OnCenterColor = Color.SandyBrown;
                    //uiLight1.OffCenterColor = Color.Gold;

                    //uiLight1.OnColor = Color.SaddleBrown;
                    //uiLight1.OffColor = Color.DarkGoldenrod;
                    //uiLight1.ShowLightLine = true;
                    //uiLight1.State = UILightState.Blink;
                    //titlePanel.BackColor = Color.FromArgb(243, 249, 255);
                    uiProcesAsBar1.RectColor = Color.FromArgb(80, 160, 255);
                    uiProcesAsBar1.Value = 0;
                    maintTable.Style = UIStyle.Blue;

                    break;

                case 1:
                    //uiLight1.OnCenterColor = Color.Green;
                    //uiLight1.OffCenterColor = Color.Red;

                    //uiLight1.OnColor = Color.DarkGreen;
                    //uiLight1.OffColor = Color.DarkRed;
                    //uiLight1.ShowLightLine = true;
                    //uiLight1.State = UILightState.On;
                    //titlePanel.BackColor = Color.DarkGreen;
                    uiProcesAsBar1.RectColor = Color.Green;
                    maintTable.Style = UIStyle.Green;

                    break;

                case 2:
                    //uiLight1.OnCenterColor = Color.Green;
                    //uiLight1.OffCenterColor = Color.Red;

                    //uiLight1.OnColor = Color.DarkGreen;
                    //uiLight1.OffColor = Color.DarkRed;
                    //uiLight1.ShowLightLine = true;
                    //uiLight1.State = UILightState.Off;
                    //titlePanel.BackColor = Color.DarkRed;
                    uiProcesAsBar1.RectColor = Color.Red;
                    maintTable.Style = UIStyle.Red;

                    YfasDeviceBase.CheckApp.Alarm();
                    break;

                case 4:
                    //uiLight1.OnCenterColor = Color.Blue;
                    //uiLight1.OffCenterColor = Color.Gray;

                    //uiLight1.OnColor = Color.DarkBlue;
                    //uiLight1.OffColor = Color.DarkGray;
                    //uiLight1.ShowLightLine = true;
                    //uiLight1.State = UILightState.Blink;
                    //titlePanel.BackColor = Color.DarkGoldenrod;
                    maintTable.Style = UIStyle.Blue;

                    break;
            }
        }

        #region TreeView

        private static void uiTreeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
                e.Cancel = true;
        }

        private static void uiTreeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
                e.Cancel = true;
        }

        private void uiTreeView1_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (IsChecking)
            {
                if (_retryCount == -1)
                {
                    if (e.Action != TreeViewAction.Unknown)
                        e.Cancel = true;
                }
            }
        }

        private void uiTreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (uiTreeView1.Nodes.Count <= 0)
                return;

            //ShowWaitForm();

            adminPreTreeView.Nodes.Clear();
            adminPreTreeView.Nodes.Add(new TreeNode("OnStart"));
            for (var i = 0; i < uiTreeView1.Nodes[0].Nodes.Count; i++)
            {
                var toClone = uiTreeView1.Nodes[0].Nodes[i];
                var newNode = CloneCheckedToPreTreeView(toClone);

                if (newNode != null && newNode.GetNodeCount(false) > 0)
                {
                    adminPreTreeView.Nodes[0].Nodes.Add(newNode);
                }
            }
            //adminPreTreeView.ExpandAll();

            //HideWaitForm();
        }

        private static TreeNode CloneCheckedToPreTreeView(TreeNode node)
        {
            if (node.GetNodeCount(false) > 0)
            {
                var newNode = new TreeNode(node.Text, node.ImageIndex, node.SelectedImageIndex) { Tag = node.Tag };
                for (var i = 0; i < node.Nodes.Count; i++)
                {
                    var tn = CloneCheckedToPreTreeView(node.Nodes[i]);
                    if (tn != null)
                        newNode.Nodes.Add(tn);
                }

                if (newNode.GetNodeCount(false) > 0)
                {
                    return newNode;
                }
                return null;
            }

            if (node.Checked)
            {
                return new TreeNode(node.Text, node.ImageIndex, node.SelectedImageIndex) { Tag = node.Tag };
            }

            return null;
        }
        #endregion

        //private void tsbtnRunToSelectNode_Click(object sender, EventArgs e)
        //{

        //}

        //private static TreeNode FindFatherNode(TreeNode treeNode)
        //{
        //    while (true)
        //    {
        //        var fathernode = treeNode.Parent;
        //        if (string.Equals(fathernode.Text, "OnStart", StringComparison.CurrentCultureIgnoreCase))
        //            return treeNode;
        //        treeNode = fathernode;
        //    }
        //}

        private void tsbRunSelected_Click(object sender, EventArgs e)
        {
            if (adminPreTreeView.Nodes.Count > 0)
            {
                if (adminPreTreeView.Nodes[0].Nodes.Count > 0)
                {
                    if (this.ShowAskDialog("确认要运行选中的步骤吗？"))
                    {
                        OnStart(true, adminPreTreeView.Nodes[0]);
                        return;
                    }
                }
            }

            this.ShowErrorDialog("请勾选需要执行的步骤");
        }

        private void uiMillisecondTimer1_Tick(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    uiLabel1.Text =
                        Math.Round(
                            (double.Parse(uiLabel1.Text) * 1000 + uiMillisecondTimer1.Interval) / 1000f, 4,
                            MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
                }));
            }
        }
    }
}
