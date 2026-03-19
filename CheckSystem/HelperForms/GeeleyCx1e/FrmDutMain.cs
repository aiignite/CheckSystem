using System;
using System.Drawing;
using System.IO;
using Sunny.UI;
using System.Windows.Forms;
using StateMachine;
using Controller;
using System.Threading;
using HZH_Controls;
using HZH_Controls.IconFont;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.HelperForms.GeeleyCx1e
{
    public partial class FrmDutMain : UIAsideMainFrame
    {
        private Thread Th { get; set; }

        public FrmDutMain()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(
                FontIcons.A_fa_dot_circle_o, 32,
                Color.DodgerBlue);
            WindowState = FormWindowState.Maximized;
            Closed += FrmDutMain_Closed;

            //设置关联
            Aside.TabControl = MainTabControl;

            var file1 = Program.SysDir + @"\流程配置文件\CX1E门模块ENV试验负载箱-DUT1-202402191410.xml";
            var file2 = Program.SysDir + @"\流程配置文件\CX1E门模块ENV试验负载箱-DUT2-202402191410.xml";
            var file3 = Program.SysDir + @"\流程配置文件\CX1E门模块ENV试验负载箱-DUT3-202402191410.xml";

            State state1 = null;
            State state2 = null;
            State state3 = null;

            if (File.Exists(file1))
            {
                state1 = new State();
                state1.Init<ControllerBase>(
                    Program.SysDir + @"\流程配置文件\CX1E门模块ENV试验负载箱-DUT1-202402191410.xml",
                    "Controller.dll");
            }

            if (File.Exists(file2))
            {
                state2 = new State();
                state2.Init<ControllerBase>(
                    Program.SysDir + @"\流程配置文件\CX1E门模块ENV试验负载箱-DUT2-202402191410.xml",
                    "Controller.dll");
            }

            if (File.Exists(file3))
            {
                state3 = new State();
                state3.Init<ControllerBase>(
                    Program.SysDir + @"\流程配置文件\CX1E门模块ENV试验负载箱-DUT3-202402191410.xml",
                    "Controller.dll");
            }

            if (Th != null)
            {
                Th.Abort();
                Th.Join();
            }

            Th = new Thread(() =>
                {
                    if (state1 != null) state1.TaskCheckStatus();
                    if (state2 != null) state2.TaskCheckStatus();
                    if (state3 != null) state3.TaskCheckStatus();
                })
            { IsBackground = true };
            Th.Start();

            //增加页面到Main
            if (state1 != null) AddPage(new FrmSingleDutPage(state1), 1001);
            if (state2 != null) AddPage(new FrmSingleDutPage(state2), 1002);
            if (state3 != null) AddPage(new FrmSingleDutPage(state3), 1003);

            //设置Header节点索引
            if (state1 != null) Aside.CreateNode(state1.DeviceConfig.DeviceInfo.DeviceName, 1001);
            if (state2 != null) Aside.CreateNode(state2.DeviceConfig.DeviceInfo.DeviceName, 1002);
            if (state3 != null) Aside.CreateNode(state3.DeviceConfig.DeviceInfo.DeviceName, 1003);

            //显示默认界面
            Aside.SelectFirst();
        }

        private void FrmDutMain_Closed(object sender, EventArgs e)
        {
            if (Th == null)
                return;
            Th.Abort();
            Th.Join();
        }
    }
}
