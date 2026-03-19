using System;
using System.Windows.Forms;
using Sunny.UI;

namespace CheckSystem.HelperForms.Psi5Reader
{
    public partial class FrmPsi5MultipleReader : UIAsideMainFrame
    {
        public FrmPsi5MultipleReader()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            //设置关联
            Aside.TabControl = MainTabControl;

            //增加页面到Main
            AddPage(new FrmSingleChipPage(), 1001);
            AddPage(new FrmSingleChipPage(), 1002);
            AddPage(new FrmSingleChipPage(), 1003);

            //设置Header节点索引
            Aside.CreateNode("Page1", 1001);
            Aside.CreateNode("Page2", 1002);
            Aside.CreateNode("Page3", 1003);

            //显示默认界面
            Aside.SelectFirst();
        }
    }
}
