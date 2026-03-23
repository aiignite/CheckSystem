using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HZH_Controls.IconFont;
using WeifenLuo.WinFormsUI.Docking;

namespace DeviceDesign
{
    public partial class FormTree : DockContent
    {
        public FormTree()
        {
            InitializeComponent();

            var nameList = Enum.GetNames(typeof(FontIcons));
            var lst = nameList.ToList();
            lst.Sort();
            for (var i = 0; i < 45; i++)
            {
                var item = lst[i];
                var icon = (FontIcons)Enum.Parse(typeof(FontIcons), item);
                imageList.Images.Add(FontImages.GetImage(icon, 32, Color.DodgerBlue));
            }
        }

        /// <summary>
        /// 根据登录用户的权限和角色，显示树的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTree_Load(object sender, EventArgs e)
        {
            InitTreeView();
            treeView.NodeMouseClick += TreeView_NodeMouseClick;
            refreshToolStripMenuItem.Click += RefreshToolStripMenuItem_Click;

            // 订阅控制器名称变更事件
            ClassComm.ControllerNameChanged += ClassComm_ControllerNameChanged;
        }

        private void ClassComm_ControllerNameChanged(object sender, ClassComm.ControllerNameChangedEventArgs e)
        {
            // 在UI线程上刷新控制器树节点
            if (treeView.InvokeRequired)
            {
                treeView.Invoke(new Action(() => RefreshControllerNode(e.OldControllerName, e.NewControllerName)));
            }
            else
            {
                RefreshControllerNode(e.OldControllerName, e.NewControllerName);
            }
        }

        private void RefreshControllerNode(string oldControllerName, string newControllerName)
        {
            // 找到"控制器"根节点
            foreach (TreeNode rootNode in treeView.Nodes)
            {
                if (rootNode.Text == "控制器")
                {
                    // 如果是新增控制器（oldControllerName为空）
                    if (string.IsNullOrEmpty(oldControllerName))
                    {
                        // 查找是否已存在同名节点
                        bool exists = false;
                        foreach (TreeNode controllerNode in rootNode.Nodes)
                        {
                            if (controllerNode.Text == newControllerName)
                            {
                                exists = true;
                                break;
                            }
                        }
                        if (!exists)
                        {
                            // 查找新控制器在ClassComm中的类型信息
                            var controller = ClassComm.DeviceConfig.Controllers.ToList().Find(c => c.Name == newControllerName);
                            if (controller != null)
                            {
                                rootNode.Nodes.Add(new TreeNode
                                {
                                    Text = controller.Name,
                                    ImageIndex = 15,
                                    Tag = "Controller." + controller.Type
                                });
                            }
                        }
                        return;
                    }

                    // 如果是重命名控制器
                    // 查找并更新旧名称的节点
                    foreach (TreeNode controllerNode in rootNode.Nodes)
                    {
                        if (controllerNode.Text == oldControllerName)
                        {
                            controllerNode.Text = newControllerName;
                            // 更新Tag中的控制器类型（如果需要）
                            return;
                        }
                    }
                }
            }
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitTreeView();
        }

        private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    //var pos = new Point(e.Node.Bounds.X + e.Node.Bounds.Width, e.Node.Bounds.Y + e.Node.Bounds.Height / 2);
                    //contextMenuStripTree.Show(treeView, pos);
                    break;

                case MouseButtons.Left:
                    if (e.Node.Nodes.Count == 0)//说明当前选中节点没有子节点
                    {
                        //ShowForm(e.Node.Text);
                        ShowForm(e.Node.Text, e.Node.Tag.ToString());
                    }
                    else
                    {
                        if (e.Node.Text == @"控制器")
                        //InitTreeView();
                        {
                            e.Node.Nodes.Clear();

                            foreach (var con in ClassComm.DeviceConfig.Controllers)
                            {
                                e.Node.Nodes.Add(new TreeNode
                                {
                                    Text = con.Name,
                                    ImageIndex = 16,
                                    Tag = "Controller." + con.Type
                                });
                            }
                        }
                    }
                    break;

                case MouseButtons.None:
                    break;

                case MouseButtons.Middle:
                    break;

                case MouseButtons.XButton1:
                    break;

                case MouseButtons.XButton2:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InitTreeView()
        {
            treeView.Nodes.Clear();

            var treeRootNode = new TreeNode("设备", 17, 18);
            treeView.Nodes.Add(treeRootNode);
            treeRootNode.Nodes.Add(new TreeNode { Text = @"状态机设计", ImageIndex = 0, Tag = "StatusMachineDesigner" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"设备设计", ImageIndex = 1, Tag = "DeviceControllerDesign" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"配置文件", ImageIndex = 2, Tag = "DeviceConfigFile" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"设备信息表", ImageIndex = 3, Tag = "DeviceConfigDeviceInfo" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"控制器信息表", ImageIndex = 4, Tag = "DeviceConfigController" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"控制器属性表", ImageIndex = 5, Tag = "DeviceConfigProperty" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"界面控件设计", ImageIndex = 6, Tag = "DeviceConfigControl" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"界面布局设计", ImageIndex = 7, Tag = "DeviceConfigFormLayout" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"工序信息表", ImageIndex = 8, Tag = "DeviceConfigProcess" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"工序参数表", ImageIndex = 9, Tag = "DeviceConfigPara" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"档位匹配表", ImageIndex = 10, Tag = "DeviceConfigGear" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"视觉标定参数表", ImageIndex = 10, Tag = "DeviceConfigRoi" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"部件映射表", ImageIndex = 11, Tag = "DeviceConfigPart" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"工作站表", ImageIndex = 12, Tag = "DeviceConfigWorkStation" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"状态单元表", ImageIndex = 13, Tag = "DeviceConfigStatusUnit" });
            treeRootNode.Nodes.Add(new TreeNode { Text = @"转换条件表", ImageIndex = 14, Tag = "DeviceConfigCondition" });

            var treeRootNodeController = new TreeNode("控制器", 17, 18);
            treeView.Nodes.Add(treeRootNodeController);
            foreach (var con in ClassComm.DeviceConfig.Controllers)
            {
                treeRootNodeController.Nodes.Add(new TreeNode
                {
                    Text = con.Name,
                    ImageIndex = 15,
                    Tag = "Controller." + con.Type
                });
            }

            //foreach (var con in ClassComm.LstControllers)
            //    treeRootNodeController.Nodes.Add(new TreeNode
            //    {
            //        Text = con.Name,
            //        ImageIndex = 6,
            //        Tag = "Controller." + con.Name
            //    });

            treeView.Nodes[0].BackColor = Color.DarkOrange;
            //treeView.ExpandAll();

            //TreeExpand();
        }

        /// <summary>
        /// 用于外部调用展开树
        /// </summary>
        public void TreeExpand()
        {
            treeView.ExpandAll();
        }

        /// <summary>
        /// 根据类名参数生成类对象
        /// </summary>
        /// <param name="strTreeNodeName"></param>
        /// <param name="strText"></param>
        private void ShowForm(string strTreeNodeName, string strText)
        {
            try
            {
                var formMain = (FormMain)Parent.Parent.Parent.Parent;

                switch (strText)
                {
                    case "StatusMachineDesigner":
                        {
                            var form = new FormStatusMachineDesigner();
                            var frm =
                                formMain.MdiChildren.ToList()
                                    .Find(
                                        t =>
                                            form.AccessibilityObject != null && t.AccessibilityObject != null &&
                                            t.AccessibilityObject.Name == form.AccessibilityObject.Name);
                            if (frm != null)
                            {
                                frm.Activate();
                                form.Dispose();
                                return;
                            }
                            form.Show(formMain.dockPanelMain);
                        }
                        break;

                    case "DeviceConfigFile":
                        {
                            var form = new FormSystemConfigXml();

                            var frm =
                                formMain.MdiChildren.ToList()
                                    .Find(
                                        t =>
                                            form.AccessibilityObject != null && t.AccessibilityObject != null &&
                                            t.AccessibilityObject.Name == form.AccessibilityObject.Name);
                            if (frm != null)
                            {
                                frm.Activate();
                                form.Dispose();
                                return;
                            }
                            form.Show(formMain.dockPanelMain);
                        }
                        break;

                    case "DeviceControllerDesign":
                        {
                            var form = new FormDeviceControllerDesign();
                            var frm =
                                formMain.MdiChildren.ToList()
                                    .Find(
                                        t =>
                                            form.AccessibilityObject != null && t.AccessibilityObject != null &&
                                            t.AccessibilityObject.Name == form.AccessibilityObject.Name);
                            if (frm != null)
                            {
                                frm.Activate();
                                form.Dispose();
                                return;
                            }
                            form.Show(formMain.dockPanelMain);
                        }
                        break;

                    default:
                        if (strText.StartsWith("Controller."))
                        {
                            var controllerType = strText; //.Substring("Controller.".Length);
                            var type = Type.GetType("DeviceDesign.FormDesignController");
                            if (type != null)
                            {
                                var form = (FormDesignController)Activator.CreateInstance(type, controllerType, strTreeNodeName);
                                // form.Text = controllerType;
                                var frm =
                                    formMain.MdiChildren.ToList()
                                        .Find(
                                            t =>
                                                form.AccessibilityObject != null && t.AccessibilityObject != null &&
                                                t.AccessibilityObject.Name == form.AccessibilityObject.Name);
                                if (frm != null)
                                {
                                    frm.Activate();
                                    form.Dispose();
                                    return;
                                }
                                form.Show(formMain.dockPanelMain);
                            }
                        }
                        else if (strText.StartsWith("DeviceConfig"))
                        {
                            var form = (FormBase)Activator.CreateInstance(Type.GetType("DeviceDesign.FormDeviceConfigComm"), strText);
                            var frm = formMain.MdiChildren.ToList().Find(t => t.Name == form.Name);
                            if (frm != null)
                            {
                                frm.Activate();
                                form.Dispose();
                                return;
                            }
                            form.Show(formMain.dockPanelMain);
                        }
                        else
                        {
                            var form = (FormDesignDevice)Activator.CreateInstance(Type.GetType("DeviceDesign.FormDesignDevice"), strText);
                            form.Text = strText;
                            var frm =
                                formMain.MdiChildren.ToList()
                                    .Find(
                                        t =>
                                            form.AccessibilityObject != null && t.AccessibilityObject != null &&
                                            t.AccessibilityObject.Name == form.AccessibilityObject.Name);
                            if (frm != null)
                            {
                                frm.Activate();
                                form.Dispose();
                                return;
                            }
                            form.Show(formMain.dockPanelMain);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
