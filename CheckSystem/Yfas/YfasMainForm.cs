using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CheckSystem.Yfas.Config.Prodcut;
using CheckSystem.Yfas.Utility;
using Sunny.UI;

namespace CheckSystem.Yfas
{
    public partial class YfasMainForm : UIHeaderMainFooterFrame
    {
        private readonly YfasCheckDataViewPage _yfasCheckDateViewPage = new YfasCheckDataViewPage();

        public YfasMainForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            uiLabel2.Text += YfasDeviceBase.User;

            //增加页面到Main
            AddPage(_yfasCheckDateViewPage, 1001);
            //AddPage(new YfasRow2CheckDateViewFrom(), 1002);
            //AddPage(new YfasRow3CheckDateViewFrom(), 1003);

            Load += YfasMainForm_Load;
            Closed += YfasMainForm_Closed;
        }

        private static void YfasMainForm_Closed(object sender, EventArgs e)
        {
            YfasDeviceBase.DisposeController();
        }

        private void YfasMainForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
            YfasDeviceBase.LoadImages();
            YfasDeviceBase.InitController();

            var bll = new Utility._3TierBll.YfasProductInfo();
            var models = bll.GetModelList("");

            foreach (var productMenuItem in models.Select(m => new ToolStripMenuItem
            {
                Text = m.Name,
                Name = m.id.ToString()
            }))
            {
                productMenuItem.Click += productMenuItem_Click;
                btnSelectProduct.DropDownItems.Add(productMenuItem);
            }

            if (YfasDeviceBase.Limit== YfasDeviceBase.LimitType.User)
            {
                btnDeviceInfo.Visible = false;
            }
        }

        #region 定时器

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!_yfasCheckDateViewPage.IsChecking)
            {
                if (YfasDeviceBase.YfasIoControllers[0].SyController.Di1 == 1.ToString())
                {
                    _yfasCheckDateViewPage.OnStart();
                }
            }

            Footer.Text =
                string.Format("当前时间：{0} {1}", DateTime.Now.ToString(CultureInfo.InvariantCulture), GetWeekZhCn());
        }

        private static string GetWeekZhCn()
        {
            switch (DateTime.Now.DayOfWeek.ToString())
            {
                case "Monday":
                    return "星期一";

                case "Tuesday":
                    return "星期二";

                case "Wednesday":
                    return "星期三";

                case "Thursday":
                    return "星期四";

                case "Friday":
                    return "星期五";

                case "Saturday":
                    return "星期六";

                default:
                    return "星期日";
            }
        }

        #endregion

        #region ToolStripMenuItem

        private void 产品清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new YfasProductConfigMainForm();
            frm.ShowDialog();
        }

        private void 检测数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new YfasHistoryCheckDataReview();
            frm.ShowDialog();
        }

        public void productMenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem == null) return;
            var productId = int.Parse(menuItem.Name);

            uiLblTitle.Text = menuItem.Text;

            _yfasCheckDateViewPage.ReLoadDetection(productId);
        }

        private void tsbtnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(uiLblTitle.Text))
            {
                this.ShowErrorDialog("请选择一个产品");
                return;
            }

            if (this.ShowAskDialog("是否开始测试当前产品：" + uiLblTitle.Text))
                _yfasCheckDateViewPage.OnStart();
        }

        #endregion
    }
}
