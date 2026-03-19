using System.Windows.Forms;
using Sunny.UI;

namespace CheckSystem
{
    partial class FormCheck
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.operationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工位1样件测试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开流程图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开图像标定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.单帧ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.批量ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开控制器调试界面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看本地检测数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开机器人监控ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开流程监控界面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.s12L数据查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.消息监控ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cANMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.信号ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonTitle = new System.Windows.Forms.Button();
            this.controllerMsgPage = new System.Windows.Forms.TabPage();
            this.contrrollerMsgGrid = new UserControls.UserDataGrid();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.statesGridView = new UserControls.UserDataGrid();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panelMain = new System.Windows.Forms.Panel();
            this.stateTab = new Sunny.UI.UITabControl();
            this.menuStripMain.SuspendLayout();
            this.controllerMsgPage.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.stateTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStripMain
            // 
            this.statusStripMain.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.statusStripMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.statusStripMain.Location = new System.Drawing.Point(0, 746);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(1024, 22);
            this.statusStripMain.TabIndex = 30;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // menuStripMain
            // 
            this.menuStripMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationToolStripMenuItem,
            this.消息监控ToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 35);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1024, 28);
            this.menuStripMain.TabIndex = 31;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // operationToolStripMenuItem
            // 
            this.operationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.工位1样件测试ToolStripMenuItem,
            this.打开流程图ToolStripMenuItem,
            this.打开图像标定ToolStripMenuItem,
            this.打开控制器调试界面ToolStripMenuItem,
            this.查看本地检测数据ToolStripMenuItem,
            this.打开机器人监控ToolStripMenuItem,
            this.打开流程监控界面ToolStripMenuItem,
            this.s12L数据查询ToolStripMenuItem});
            this.operationToolStripMenuItem.Name = "operationToolStripMenuItem";
            this.operationToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.operationToolStripMenuItem.Text = "操作";
            // 
            // 工位1样件测试ToolStripMenuItem
            // 
            this.工位1样件测试ToolStripMenuItem.CheckOnClick = true;
            this.工位1样件测试ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_DeleteShape_Image;
            this.工位1样件测试ToolStripMenuItem.Name = "工位1样件测试ToolStripMenuItem";
            this.工位1样件测试ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.工位1样件测试ToolStripMenuItem.Text = "样件测试";
            this.工位1样件测试ToolStripMenuItem.Click += new System.EventHandler(this.工位1样件测试ToolStripMenuItem_Click);
            // 
            // 打开流程图ToolStripMenuItem
            // 
            this.打开流程图ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_Detect_Image;
            this.打开流程图ToolStripMenuItem.Name = "打开流程图ToolStripMenuItem";
            this.打开流程图ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.打开流程图ToolStripMenuItem.Text = "打开流程配置";
            this.打开流程图ToolStripMenuItem.Click += new System.EventHandler(this.打开流程图ToolStripMenuItem_Click);
            // 
            // 打开图像标定ToolStripMenuItem
            // 
            this.打开图像标定ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.单帧ToolStripMenuItem,
            this.批量ToolStripMenuItem});
            this.打开图像标定ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_Capture_Image;
            this.打开图像标定ToolStripMenuItem.Name = "打开图像标定ToolStripMenuItem";
            this.打开图像标定ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.打开图像标定ToolStripMenuItem.Text = "打开图像标定";
            // 
            // 单帧ToolStripMenuItem
            // 
            this.单帧ToolStripMenuItem.Name = "单帧ToolStripMenuItem";
            this.单帧ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.单帧ToolStripMenuItem.Text = "单帧";
            this.单帧ToolStripMenuItem.Click += new System.EventHandler(this.单帧ToolStripMenuItem_Click);
            // 
            // 批量ToolStripMenuItem
            // 
            this.批量ToolStripMenuItem.Name = "批量ToolStripMenuItem";
            this.批量ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.批量ToolStripMenuItem.Text = "批量";
            this.批量ToolStripMenuItem.Click += new System.EventHandler(this.批量ToolStripMenuItem_Click);
            // 
            // 打开控制器调试界面ToolStripMenuItem
            // 
            this.打开控制器调试界面ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_AddRectangle_Image;
            this.打开控制器调试界面ToolStripMenuItem.Name = "打开控制器调试界面ToolStripMenuItem";
            this.打开控制器调试界面ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.打开控制器调试界面ToolStripMenuItem.Text = "打开控制器调试界面";
            this.打开控制器调试界面ToolStripMenuItem.Click += new System.EventHandler(this.打开控制器调试界面ToolStripMenuItem_Click);
            // 
            // 查看本地检测数据ToolStripMenuItem
            // 
            this.查看本地检测数据ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_Save_Image;
            this.查看本地检测数据ToolStripMenuItem.Name = "查看本地检测数据ToolStripMenuItem";
            this.查看本地检测数据ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.查看本地检测数据ToolStripMenuItem.Text = "查看本地检测数据";
            this.查看本地检测数据ToolStripMenuItem.Click += new System.EventHandler(this.查看本地检测数据ToolStripMenuItem_Click);
            // 
            // 打开机器人监控ToolStripMenuItem
            // 
            this.打开机器人监控ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_AddPolygon_Image;
            this.打开机器人监控ToolStripMenuItem.Name = "打开机器人监控ToolStripMenuItem";
            this.打开机器人监控ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.打开机器人监控ToolStripMenuItem.Text = "打开机器人监控";
            this.打开机器人监控ToolStripMenuItem.Visible = false;
            this.打开机器人监控ToolStripMenuItem.Click += new System.EventHandler(this.打开机器人监控ToolStripMenuItem_Click);
            // 
            // 打开流程监控界面ToolStripMenuItem
            // 
            this.打开流程监控界面ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_ImageFit_Image;
            this.打开流程监控界面ToolStripMenuItem.Name = "打开流程监控界面ToolStripMenuItem";
            this.打开流程监控界面ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.打开流程监控界面ToolStripMenuItem.Text = "打开流程监控界面";
            this.打开流程监控界面ToolStripMenuItem.Click += new System.EventHandler(this.打开流程监控界面ToolStripMenuItem_Click);
            // 
            // s12L数据查询ToolStripMenuItem
            // 
            this.s12L数据查询ToolStripMenuItem.Name = "s12L数据查询ToolStripMenuItem";
            this.s12L数据查询ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.s12L数据查询ToolStripMenuItem.Text = "S12L数据查询";
            this.s12L数据查询ToolStripMenuItem.Visible = false;
            this.s12L数据查询ToolStripMenuItem.Click += new System.EventHandler(this.s12L数据查询ToolStripMenuItem_Click);
            // 
            // 消息监控ToolStripMenuItem
            // 
            this.消息监控ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cANMessageToolStripMenuItem,
            this.linMessageToolStripMenuItem,
            this.信号ToolStripMenuItem});
            this.消息监控ToolStripMenuItem.Name = "消息监控ToolStripMenuItem";
            this.消息监控ToolStripMenuItem.Size = new System.Drawing.Size(85, 24);
            this.消息监控ToolStripMenuItem.Text = "消息监控";
            // 
            // cANMessageToolStripMenuItem
            // 
            this.cANMessageToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_Detect_Image;
            this.cANMessageToolStripMenuItem.Name = "cANMessageToolStripMenuItem";
            this.cANMessageToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.cANMessageToolStripMenuItem.Text = "CAN Message";
            this.cANMessageToolStripMenuItem.Click += new System.EventHandler(this.cANMessageToolStripMenuItem_Click);
            // 
            // linMessageToolStripMenuItem
            // 
            this.linMessageToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_AddPolygon_Image;
            this.linMessageToolStripMenuItem.Name = "linMessageToolStripMenuItem";
            this.linMessageToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.linMessageToolStripMenuItem.Text = "Lin Message";
            this.linMessageToolStripMenuItem.Click += new System.EventHandler(this.linMessageToolStripMenuItem_Click);
            // 
            // 信号ToolStripMenuItem
            // 
            this.信号ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_AddRectangle_Image;
            this.信号ToolStripMenuItem.Name = "信号ToolStripMenuItem";
            this.信号ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.信号ToolStripMenuItem.Text = "信号";
            this.信号ToolStripMenuItem.Click += new System.EventHandler(this.信号ToolStripMenuItem_Click);
            // 
            // buttonTitle
            // 
            this.buttonTitle.BackColor = System.Drawing.Color.LightSeaGreen;
            this.buttonTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonTitle.Enabled = false;
            this.buttonTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTitle.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonTitle.ForeColor = System.Drawing.Color.RoyalBlue;
            this.buttonTitle.Location = new System.Drawing.Point(0, 63);
            this.buttonTitle.Name = "buttonTitle";
            this.buttonTitle.Size = new System.Drawing.Size(1024, 60);
            this.buttonTitle.TabIndex = 32;
            this.buttonTitle.Text = "buttonTitle";
            this.buttonTitle.UseVisualStyleBackColor = false;
            // 
            // controllerMsgPage
            // 
            this.controllerMsgPage.Controls.Add(this.contrrollerMsgGrid);
            this.controllerMsgPage.Location = new System.Drawing.Point(0, 40);
            this.controllerMsgPage.Name = "controllerMsgPage";
            this.controllerMsgPage.Size = new System.Drawing.Size(200, 60);
            this.controllerMsgPage.TabIndex = 6;
            this.controllerMsgPage.Text = "控制器消息列表";
            this.controllerMsgPage.UseVisualStyleBackColor = true;
            // 
            // contrrollerMsgGrid
            // 
            this.contrrollerMsgGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contrrollerMsgGrid.Location = new System.Drawing.Point(0, 0);
            this.contrrollerMsgGrid.Margin = new System.Windows.Forms.Padding(5);
            this.contrrollerMsgGrid.Name = "contrrollerMsgGrid";
            this.contrrollerMsgGrid.Size = new System.Drawing.Size(200, 60);
            this.contrrollerMsgGrid.TabIndex = 2;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.statesGridView);
            this.tabPage4.Location = new System.Drawing.Point(0, 40);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(200, 60);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "状态列表";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // statesGridView
            // 
            this.statesGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statesGridView.Location = new System.Drawing.Point(0, 0);
            this.statesGridView.Margin = new System.Windows.Forms.Padding(5);
            this.statesGridView.Name = "statesGridView";
            this.statesGridView.Size = new System.Drawing.Size(200, 60);
            this.statesGridView.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panelMain);
            this.tabPage1.Location = new System.Drawing.Point(0, 40);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1024, 583);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "检测信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.SystemColors.MenuBar;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1024, 583);
            this.panelMain.TabIndex = 14;
            // 
            // stateTab
            // 
            this.stateTab.Controls.Add(this.tabPage1);
            this.stateTab.Controls.Add(this.tabPage4);
            this.stateTab.Controls.Add(this.controllerMsgPage);
            this.stateTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stateTab.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.stateTab.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stateTab.ItemSize = new System.Drawing.Size(150, 40);
            this.stateTab.Location = new System.Drawing.Point(0, 123);
            this.stateTab.MainPage = "";
            this.stateTab.MenuStyle = Sunny.UI.UIMenuStyle.Custom;
            this.stateTab.Name = "stateTab";
            this.stateTab.SelectedIndex = 0;
            this.stateTab.Size = new System.Drawing.Size(1024, 623);
            this.stateTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.stateTab.TabBackColor = System.Drawing.Color.LightGray;
            this.stateTab.TabIndex = 33;
            this.stateTab.TabSelectedColor = System.Drawing.Color.Gray;
            this.stateTab.TabSelectedForeColor = System.Drawing.Color.Black;
            this.stateTab.TabUnSelectedForeColor = System.Drawing.Color.Black;
            this.stateTab.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // FormCheck
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.stateTab);
            this.Controls.Add(this.buttonTitle);
            this.Controls.Add(this.menuStripMain);
            this.Controls.Add(this.statusStripMain);
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "FormCheck";
            this.Text = "FormCheck";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 1024, 768);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.controllerMsgPage.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.stateTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStripMain;
        private MenuStrip menuStripMain;
        private ToolStripMenuItem operationToolStripMenuItem;
        private ToolStripMenuItem 工位1样件测试ToolStripMenuItem;
        private ToolStripMenuItem 打开流程图ToolStripMenuItem;
        private ToolStripMenuItem 打开图像标定ToolStripMenuItem;
        private ToolStripMenuItem 打开控制器调试界面ToolStripMenuItem;
        private Button buttonTitle;
        private ToolStripMenuItem 打开流程监控界面ToolStripMenuItem;
        private ToolStripMenuItem 打开机器人监控ToolStripMenuItem;
        private ToolStripMenuItem 消息监控ToolStripMenuItem;
        private ToolStripMenuItem cANMessageToolStripMenuItem;
        private ToolStripMenuItem linMessageToolStripMenuItem;
        private ToolStripMenuItem 信号ToolStripMenuItem;
        private ToolStripMenuItem s12L数据查询ToolStripMenuItem;
        private TabPage controllerMsgPage;
        private UserControls.UserDataGrid contrrollerMsgGrid;
        private TabPage tabPage4;
        private UserControls.UserDataGrid statesGridView;
        private TabPage tabPage1;
        private Panel panelMain;
        private UITabControl stateTab;
        private ToolStripMenuItem 查看本地检测数据ToolStripMenuItem;
        private ToolStripMenuItem 单帧ToolStripMenuItem;
        private ToolStripMenuItem 批量ToolStripMenuItem;
    }
}