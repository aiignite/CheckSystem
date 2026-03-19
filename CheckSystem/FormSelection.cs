using Go;
using Sunny.UI;
using System;

namespace CheckSystem
{
    public partial class FormSelection : UIAsideHeaderMainFrame
    {
        public static control_strand MainStrand;
        public FormSelection()
        {
            InitializeComponent();
            Load += FormSelection_Load;
        }

        private void FormSelection_Load(object sender, EventArgs e)
        {
            MainStrand = new control_strand(this);
            FormToolPage.InitTools();

            var iniForm = Program.SysSetup.IniReadValue("System", "InitializeForm");

            var isContain = -1;
            if (!string.IsNullOrEmpty(iniForm) &&
                !string.Equals(iniForm, "default", StringComparison.CurrentCultureIgnoreCase))
            {
                for (var i = 0; i < FormToolPage.ListTools.Count; i++)
                {
                    if (!string.Equals(FormToolPage.ListTools[i], iniForm, StringComparison.CurrentCultureIgnoreCase))
                        continue;
                    isContain = i;
                    break;
                }
            }

            if (isContain != -1)
            {
                FormToolPage.OpenToolForm(iniForm);
            }
            else
            {
                //设置关联
                Aside.TabControl = MainTabControl;

                //增加页面到Main
                AddPage(new FormXmlPage(), 1);
                AddPage(new FormToolPage(), 2);
                //AddPage(new FPage3(), 1003);

                //设置Header节点索引
                Aside.CreateNode("配置文件", 1);
                Aside.CreateNode("常用工具", 2);
                //Aside.CreateNode("Page3", 1003);

                //显示默认界面
                Aside.SelectFirst();
            }
        }
    }
}
