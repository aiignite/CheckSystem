using System.Windows.Forms;
using CheckSystem.Yfas.Config.Cmds;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.Prodcut
{
    public partial class YfasProductConfigMainForm : UIAsideMainFrame
    {
        public YfasProductConfigMainForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            AddPage(new YfasProductListPage(), 1001);
            AddPage(new YfasDetectionBaseList(), 1002);
            AddPage(new YfasPreProgramPage(), 1003);

            Aside.CreateNode("产品清单", 1001);
            Aside.CreateNode("基础检测项清单", 1002);
            Aside.CreateNode("动作单元集合", 1003);

            Aside.SelectFirst();
        }
    }
}
