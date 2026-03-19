using HZH_Controls.IconFont;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DeviceDesign
{
    public partial class FormMain : Form
    {
        private readonly string _xmlFilePath;
        private readonly string _controllerPath;
        private readonly string _userControlPath;

        public FormMain(string xmlFilePath, string controllerPath, string userControlPath)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.A_fa_edit, 32,
                Color.DodgerBlue);
            _xmlFilePath = xmlFilePath;
            _controllerPath = controllerPath;
            _userControlPath = userControlPath;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            ClassComm.GetDeviceConfigFromFile(_xmlFilePath);
            ClassComm.GetLstControllers(_controllerPath);
            ClassComm.GetLstUserControls(_userControlPath);

            var frmTree = new FormTree();
            frmTree.Show(dockPanelMain);
            frmTree.DockTo(dockPanelMain, DockStyle.Left);
            frmTree.CloseButtonVisible = false;
            frmTree.TreeExpand();
            //设置dock默认宽度百分比
            dockPanelMain.DockLeftPortion = 0.15;
        }
    }
}
