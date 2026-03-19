using System;
using CheckSystem.HelperForms.Psi5Reader;
using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;
using HZH_Controls.IconFont;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.SyRenesasMcuController
{
    public partial class FrmRenesasMcnMultipleControllers : UIAsideMainFrame
    {
        public FrmRenesasMcnMultipleControllers()
        {
            WindowState = FormWindowState.Maximized;
            InitializeComponent();

            Icon = FontImages.GetIcon(
                FontIcons.A_fa_cogs, 32,
                Color.DodgerBlue);

            Load += FrmRenesasMcnMultipleControllers_Load;
        }

        private void FrmRenesasMcnMultipleControllers_Load(object sender, EventArgs e)
        {
            //设置关联
            Aside.TabControl = MainTabControl;

            //增加页面到Main
            for (var i = 0; i < 20; i++)
            {
                var index = 1000 + i + 1;

                var name = "ControllerMaster:" + (index - 1000);
                AddPage(new FrmSingleMcuPage(name), index);
                Aside.CreateNode(name, index);
            }

            //AddPage(new FrmSingleMcuPage(), 1001);
            //AddPage(new FrmSingleMcuPage(), 1002);
            //AddPage(new FrmSingleMcuPage(), 1003);

            ////设置Header节点索引
            //Aside.CreateNode("Page1", 1001);
            //Aside.CreateNode("Page2", 1002);
            //Aside.CreateNode("Page3", 1003);

            //显示默认界面
            Aside.SelectFirst();
        }
    }
}
