using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using CommonUtility;
using CommonUtility.BusLoader;

namespace CheckSystem.CAN
{
    public partial class CanDataUdsSendForm : Form
    {
        public CanBus Can { get; set; }

        public CanDataUdsSendForm(CanBus can)
        {
            InitializeComponent();
            Can = can;
            InitTreeView();
        }

        private void InitTreeView()
        {
            treeView.Nodes.Clear();
            var treeRootNode = new TreeNode("诊断协议列表", 1, 1);
            treeView.Nodes.Add(treeRootNode);

            foreach (var temp in Enum.GetValues(typeof(Uds14229Helper.ServiceType)).Cast<Uds14229Helper.ServiceType>())
                if (temp.GetCustomAttribute<DescriptionAttribute>() != null &&
                       !string.IsNullOrEmpty(temp.GetCustomAttribute<DescriptionAttribute>().Description))
                    treeRootNode.Nodes.Add(string.Format("({0}){1}", ValueHelper.GetHextStrWithOx((byte)temp),
                            temp.GetCustomAttribute<DescriptionAttribute>().Description));
                else
                    treeRootNode.Nodes.Add(string.Format("({0}){1}", ValueHelper.GetHextStrWithOx((byte)temp),
                            temp));
        }
    }
}
