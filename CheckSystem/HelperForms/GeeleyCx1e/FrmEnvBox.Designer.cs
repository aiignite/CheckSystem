using System.Windows.Forms;
using Sunny.UI;

namespace CheckSystem.HelperForms.GeeleyCx1e
{
    partial class FrmEnvBox
    {

        #region Windows Form Designer generated code


        #endregion

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.uiTabControl1 = new Sunny.UI.UITabControl();
            this.MainPage = new System.Windows.Forms.TabPage();
            this.mainPanel = new Sunny.UI.UIPanel();
            this.lblSysState = new Sunny.UI.UIMarkLabel();
            this.uiGroupBox2 = new Sunny.UI.UIGroupBox();
            this.swActvnOfPudLi = new Sunny.UI.UISwitch();
            this.swActvnOfIndcrOut = new Sunny.UI.UISwitch();
            this.cmbWindowLight = new Sunny.UI.UIComboBox();
            this.cmbFronHandleLight = new Sunny.UI.UIComboBox();
            this.cmbHndlDoorLiDrvr = new Sunny.UI.UIComboBox();
            this.swStepLight = new Sunny.UI.UISwitch();
            this.swPocketLight = new Sunny.UI.UISwitch();
            this.swBsd = new Sunny.UI.UISwitch();
            this.txtMemoryVolt = new Sunny.UI.UITextBox();
            this.txtLockLightVolt = new Sunny.UI.UITextBox();
            this.txtWindowLightVolt = new Sunny.UI.UITextBox();
            this.txtFronHandleLightVolt = new Sunny.UI.UITextBox();
            this.txtHndlDoorLiDrvrVolt = new Sunny.UI.UITextBox();
            this.txtActvnOfPudLiVolt = new Sunny.UI.UITextBox();
            this.txtActvnOfIndcrOutVolt = new Sunny.UI.UITextBox();
            this.txtStepLightVolt = new Sunny.UI.UITextBox();
            this.txtPocketLightVolt = new Sunny.UI.UITextBox();
            this.txtBsdVolt = new Sunny.UI.UITextBox();
            this.uiMarkLabel29 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel5 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel25 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel12 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel11 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel10 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel9 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel7 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel6 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel8 = new Sunny.UI.UIMarkLabel();
            this.uiGroupBox9 = new Sunny.UI.UIGroupBox();
            this.rbChdLockReLeCtrlHmiReqUnlock = new Sunny.UI.UIRadioButton();
            this.rbChdLockReLeCtrlHmiReqIdle = new Sunny.UI.UIRadioButton();
            this.txtDcChildrenLock = new Sunny.UI.UIIntegerUpDown();
            this.rbChdLockReLeCtrlHmiReqLock = new Sunny.UI.UIRadioButton();
            this.uiMarkLabel27 = new Sunny.UI.UIMarkLabel();
            this.groupRelayBtns = new Sunny.UI.UIGroupBox();
            this.uiButton21 = new Sunny.UI.UIButton();
            this.uiButton11 = new Sunny.UI.UIButton();
            this.uiButton20 = new Sunny.UI.UIButton();
            this.uiButton9 = new Sunny.UI.UIButton();
            this.uiButton19 = new Sunny.UI.UIButton();
            this.uiButton7 = new Sunny.UI.UIButton();
            this.uiButton18 = new Sunny.UI.UIButton();
            this.uiButton5 = new Sunny.UI.UIButton();
            this.uiButton17 = new Sunny.UI.UIButton();
            this.uiButton3 = new Sunny.UI.UIButton();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.uiButton24 = new Sunny.UI.UIButton();
            this.uiButton23 = new Sunny.UI.UIButton();
            this.uiButton22 = new Sunny.UI.UIButton();
            this.uiButton16 = new Sunny.UI.UIButton();
            this.uiButton15 = new Sunny.UI.UIButton();
            this.uiButton10 = new Sunny.UI.UIButton();
            this.uiButton14 = new Sunny.UI.UIButton();
            this.uiButton8 = new Sunny.UI.UIButton();
            this.uiButton13 = new Sunny.UI.UIButton();
            this.uiButton6 = new Sunny.UI.UIButton();
            this.uiButton12 = new Sunny.UI.UIButton();
            this.uiButton4 = new Sunny.UI.UIButton();
            this.uiButton2 = new Sunny.UI.UIButton();
            this.btnAddCustomController = new Sunny.UI.UIButton();
            this.uiGroupBox12 = new Sunny.UI.UIGroupBox();
            this.dgvCurrent = new Sunny.UI.UIDataGridView();
            this.uiGroupBox11 = new Sunny.UI.UIGroupBox();
            this.dgvState = new Sunny.UI.UIDataGridView();
            this.uiGroupBox3 = new Sunny.UI.UIGroupBox();
            this.uiLedBulb2 = new Sunny.UI.UILedBulb();
            this.swStartCan = new Sunny.UI.UISwitch();
            this.uiMarkLabel18 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel14 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel22 = new Sunny.UI.UIMarkLabel();
            this.txtVbat2 = new Sunny.UI.UITextBox();
            this.uiMarkLabel21 = new Sunny.UI.UIMarkLabel();
            this.txtMotorDoorSpeed = new Sunny.UI.UITextBox();
            this.txtMotorPedalSpeed = new Sunny.UI.UITextBox();
            this.txtTemperature = new Sunny.UI.UITextBox();
            this.txtVbat1 = new Sunny.UI.UITextBox();
            this.uiMarkLabel24 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel23 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel13 = new Sunny.UI.UIMarkLabel();
            this.uiGroupBox10 = new Sunny.UI.UIGroupBox();
            this.swPowSupplyConDDSswitch = new Sunny.UI.UISwitch();
            this.swPowSupplyConDoorHall = new Sunny.UI.UISwitch();
            this.swPowSupplyConHandleHall = new Sunny.UI.UISwitch();
            this.swPowSupplyCmdPos = new Sunny.UI.UISwitch();
            this.swPowSupplyConPendalHall = new Sunny.UI.UISwitch();
            this.uiMarkLabel37 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel36 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel35 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel33 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel34 = new Sunny.UI.UIMarkLabel();
            this.txtPowSupplyConDDSswitchVolt = new Sunny.UI.UITextBox();
            this.txtPowSupplyConDoorHallVolt = new Sunny.UI.UITextBox();
            this.txtPowSupplyConHandleHallVolt = new Sunny.UI.UITextBox();
            this.txtPowSupplyCmdPosVolt = new Sunny.UI.UITextBox();
            this.txtowSupplyConPendalHallVolt = new Sunny.UI.UITextBox();
            this.uiGroupBox8 = new Sunny.UI.UIGroupBox();
            this.rbPedalClose = new Sunny.UI.UIRadioButton();
            this.rbPedalIdle = new Sunny.UI.UIRadioButton();
            this.uiMarkLabel26 = new Sunny.UI.UIMarkLabel();
            this.rbPedalOpen = new Sunny.UI.UIRadioButton();
            this.txtDcPedal = new Sunny.UI.UIIntegerUpDown();
            this.uiGroupBox7 = new Sunny.UI.UIGroupBox();
            this.rbDoorDrvrHndlCmdClose = new Sunny.UI.UIRadioButton();
            this.rbDoorDrvrHndlCmdIdle = new Sunny.UI.UIRadioButton();
            this.uiMarkLabel17 = new Sunny.UI.UIMarkLabel();
            this.txtDcHandle = new Sunny.UI.UIIntegerUpDown();
            this.rbDoorDrvrHndlCmdOpen = new Sunny.UI.UIRadioButton();
            this.uiGroupBox6 = new Sunny.UI.UIGroupBox();
            this.rbShortDropWinOpen = new Sunny.UI.UIRadioButton();
            this.uiMarkLabel15 = new Sunny.UI.UIMarkLabel();
            this.rbShortDropWinClose = new Sunny.UI.UIRadioButton();
            this.txtDcWindow = new Sunny.UI.UIIntegerUpDown();
            this.rbShortDropWinIdle = new Sunny.UI.UIRadioButton();
            this.uiGroupBox5 = new Sunny.UI.UIGroupBox();
            this.rbDoorClose = new Sunny.UI.UIRadioButton();
            this.rbDoorOpen = new Sunny.UI.UIRadioButton();
            this.rbDoorIdle = new Sunny.UI.UIRadioButton();
            this.txtDcDoor = new Sunny.UI.UIIntegerUpDown();
            this.uiMarkLabel16 = new Sunny.UI.UIMarkLabel();
            this.uiGroupBox4 = new Sunny.UI.UIGroupBox();
            this.txtDcElectricRelese = new Sunny.UI.UIIntegerUpDown();
            this.txtDcLock2 = new Sunny.UI.UIIntegerUpDown();
            this.uiMarkLabel19 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel20 = new Sunny.UI.UIMarkLabel();
            this.cmbMotorCmd = new Sunny.UI.UIComboBox();
            this.uiGroupBox1 = new Sunny.UI.UIGroupBox();
            this.txtMirrorYPos = new Sunny.UI.UITextBox();
            this.txtMirrorXPos = new Sunny.UI.UITextBox();
            this.uiMarkLabel30 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel28 = new Sunny.UI.UIMarkLabel();
            this.txtMirrTintgCmd = new Sunny.UI.UIIntegerUpDown();
            this.txtDcMirrorY = new Sunny.UI.UIIntegerUpDown();
            this.txtDcMirrorX = new Sunny.UI.UIIntegerUpDown();
            this.swMirrDefrstDrvrCmd = new Sunny.UI.UISwitch();
            this.cmbDrvrAdjCmd = new Sunny.UI.UIComboBox();
            this.cmbDrvrMirrCmd5 = new Sunny.UI.UIComboBox();
            this.txtMirrorTintgVolt = new Sunny.UI.UITextBox();
            this.txtMirrorDefrstVolt = new Sunny.UI.UITextBox();
            this.uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.uiTabControl1.SuspendLayout();
            this.MainPage.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.uiGroupBox2.SuspendLayout();
            this.uiGroupBox9.SuspendLayout();
            this.groupRelayBtns.SuspendLayout();
            this.uiGroupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrent)).BeginInit();
            this.uiGroupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvState)).BeginInit();
            this.uiGroupBox3.SuspendLayout();
            this.uiGroupBox10.SuspendLayout();
            this.uiGroupBox8.SuspendLayout();
            this.uiGroupBox7.SuspendLayout();
            this.uiGroupBox6.SuspendLayout();
            this.uiGroupBox5.SuspendLayout();
            this.uiGroupBox4.SuspendLayout();
            this.uiGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTabControl1
            // 
            this.uiTabControl1.Controls.Add(this.MainPage);
            this.uiTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiTabControl1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTabControl1.ItemSize = new System.Drawing.Size(150, 40);
            this.uiTabControl1.Location = new System.Drawing.Point(0, 35);
            this.uiTabControl1.MainPage = "主页";
            this.uiTabControl1.MenuStyle = Sunny.UI.UIMenuStyle.Custom;
            this.uiTabControl1.Name = "uiTabControl1";
            this.uiTabControl1.SelectedIndex = 0;
            this.uiTabControl1.Size = new System.Drawing.Size(1024, 733);
            this.uiTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiTabControl1.TabIndex = 24;
            this.uiTabControl1.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // MainPage
            // 
            this.MainPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MainPage.Controls.Add(this.mainPanel);
            this.MainPage.Location = new System.Drawing.Point(0, 40);
            this.MainPage.Name = "MainPage";
            this.MainPage.Size = new System.Drawing.Size(1024, 693);
            this.MainPage.TabIndex = 1;
            this.MainPage.Text = "页面";
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.lblSysState);
            this.mainPanel.Controls.Add(this.uiGroupBox2);
            this.mainPanel.Controls.Add(this.uiGroupBox9);
            this.mainPanel.Controls.Add(this.groupRelayBtns);
            this.mainPanel.Controls.Add(this.uiGroupBox12);
            this.mainPanel.Controls.Add(this.uiGroupBox11);
            this.mainPanel.Controls.Add(this.uiGroupBox3);
            this.mainPanel.Controls.Add(this.uiGroupBox10);
            this.mainPanel.Controls.Add(this.uiGroupBox8);
            this.mainPanel.Controls.Add(this.uiGroupBox7);
            this.mainPanel.Controls.Add(this.uiGroupBox6);
            this.mainPanel.Controls.Add(this.uiGroupBox5);
            this.mainPanel.Controls.Add(this.uiGroupBox4);
            this.mainPanel.Controls.Add(this.uiGroupBox1);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.mainPanel.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.mainPanel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mainPanel.MinimumSize = new System.Drawing.Size(1, 1);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.mainPanel.Size = new System.Drawing.Size(1024, 693);
            this.mainPanel.Style = Sunny.UI.UIStyle.Custom;
            this.mainPanel.TabIndex = 0;
            this.mainPanel.Text = null;
            this.mainPanel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSysState
            // 
            this.lblSysState.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSysState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblSysState.Location = new System.Drawing.Point(15, 15);
            this.lblSysState.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.lblSysState.Name = "lblSysState";
            this.lblSysState.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblSysState.Size = new System.Drawing.Size(254, 23);
            this.lblSysState.Style = Sunny.UI.UIStyle.Custom;
            this.lblSysState.TabIndex = 3;
            this.lblSysState.Text = "系统状态：";
            this.lblSysState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.swActvnOfPudLi);
            this.uiGroupBox2.Controls.Add(this.swActvnOfIndcrOut);
            this.uiGroupBox2.Controls.Add(this.cmbWindowLight);
            this.uiGroupBox2.Controls.Add(this.cmbFronHandleLight);
            this.uiGroupBox2.Controls.Add(this.cmbHndlDoorLiDrvr);
            this.uiGroupBox2.Controls.Add(this.swStepLight);
            this.uiGroupBox2.Controls.Add(this.swPocketLight);
            this.uiGroupBox2.Controls.Add(this.swBsd);
            this.uiGroupBox2.Controls.Add(this.txtMemoryVolt);
            this.uiGroupBox2.Controls.Add(this.txtLockLightVolt);
            this.uiGroupBox2.Controls.Add(this.txtWindowLightVolt);
            this.uiGroupBox2.Controls.Add(this.txtFronHandleLightVolt);
            this.uiGroupBox2.Controls.Add(this.txtHndlDoorLiDrvrVolt);
            this.uiGroupBox2.Controls.Add(this.txtActvnOfPudLiVolt);
            this.uiGroupBox2.Controls.Add(this.txtActvnOfIndcrOutVolt);
            this.uiGroupBox2.Controls.Add(this.txtStepLightVolt);
            this.uiGroupBox2.Controls.Add(this.txtPocketLightVolt);
            this.uiGroupBox2.Controls.Add(this.txtBsdVolt);
            this.uiGroupBox2.Controls.Add(this.uiMarkLabel29);
            this.uiGroupBox2.Controls.Add(this.uiMarkLabel5);
            this.uiGroupBox2.Controls.Add(this.uiMarkLabel25);
            this.uiGroupBox2.Controls.Add(this.uiMarkLabel12);
            this.uiGroupBox2.Controls.Add(this.uiMarkLabel11);
            this.uiGroupBox2.Controls.Add(this.uiMarkLabel10);
            this.uiGroupBox2.Controls.Add(this.uiMarkLabel9);
            this.uiGroupBox2.Controls.Add(this.uiMarkLabel7);
            this.uiGroupBox2.Controls.Add(this.uiMarkLabel6);
            this.uiGroupBox2.Controls.Add(this.uiMarkLabel8);
            this.uiGroupBox2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox2.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox2.Location = new System.Drawing.Point(4, 398);
            this.uiGroupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox2.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiGroupBox2.Size = new System.Drawing.Size(282, 290);
            this.uiGroupBox2.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox2.TabIndex = 0;
            this.uiGroupBox2.Text = "灯";
            this.uiGroupBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // swActvnOfPudLi
            // 
            this.swActvnOfPudLi.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.swActvnOfPudLi.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.swActvnOfPudLi.Location = new System.Drawing.Point(93, 143);
            this.swActvnOfPudLi.MinimumSize = new System.Drawing.Size(1, 1);
            this.swActvnOfPudLi.Name = "swActvnOfPudLi";
            this.swActvnOfPudLi.Size = new System.Drawing.Size(75, 23);
            this.swActvnOfPudLi.Style = Sunny.UI.UIStyle.Custom;
            this.swActvnOfPudLi.TabIndex = 5;
            this.swActvnOfPudLi.Text = "uiSwitch1";
            // 
            // swActvnOfIndcrOut
            // 
            this.swActvnOfIndcrOut.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.swActvnOfIndcrOut.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.swActvnOfIndcrOut.Location = new System.Drawing.Point(93, 113);
            this.swActvnOfIndcrOut.MinimumSize = new System.Drawing.Size(1, 1);
            this.swActvnOfIndcrOut.Name = "swActvnOfIndcrOut";
            this.swActvnOfIndcrOut.Size = new System.Drawing.Size(75, 23);
            this.swActvnOfIndcrOut.Style = Sunny.UI.UIStyle.Custom;
            this.swActvnOfIndcrOut.TabIndex = 5;
            this.swActvnOfIndcrOut.Text = "uiSwitch1";
            // 
            // cmbWindowLight
            // 
            this.cmbWindowLight.DataSource = null;
            this.cmbWindowLight.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbWindowLight.FillColor = System.Drawing.Color.White;
            this.cmbWindowLight.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbWindowLight.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.cmbWindowLight.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(242)))), ((int)(((byte)(212)))));
            this.cmbWindowLight.ItemRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbWindowLight.Items.AddRange(new object[] {
            "OFF",
            "ON",
            "Blink"});
            this.cmbWindowLight.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbWindowLight.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbWindowLight.Location = new System.Drawing.Point(117, 230);
            this.cmbWindowLight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbWindowLight.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbWindowLight.Name = "cmbWindowLight";
            this.cmbWindowLight.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbWindowLight.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbWindowLight.ScrollBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbWindowLight.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbWindowLight.Size = new System.Drawing.Size(75, 23);
            this.cmbWindowLight.Style = Sunny.UI.UIStyle.Custom;
            this.cmbWindowLight.SymbolSize = 24;
            this.cmbWindowLight.TabIndex = 4;
            this.cmbWindowLight.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbWindowLight.Watermark = "";
            // 
            // cmbFronHandleLight
            // 
            this.cmbFronHandleLight.DataSource = null;
            this.cmbFronHandleLight.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbFronHandleLight.FillColor = System.Drawing.Color.White;
            this.cmbFronHandleLight.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbFronHandleLight.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.cmbFronHandleLight.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(242)))), ((int)(((byte)(212)))));
            this.cmbFronHandleLight.ItemRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbFronHandleLight.Items.AddRange(new object[] {
            "OFF",
            "ON",
            "Blink"});
            this.cmbFronHandleLight.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbFronHandleLight.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbFronHandleLight.Location = new System.Drawing.Point(93, 201);
            this.cmbFronHandleLight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbFronHandleLight.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbFronHandleLight.Name = "cmbFronHandleLight";
            this.cmbFronHandleLight.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbFronHandleLight.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbFronHandleLight.ScrollBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbFronHandleLight.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbFronHandleLight.Size = new System.Drawing.Size(99, 23);
            this.cmbFronHandleLight.Style = Sunny.UI.UIStyle.Custom;
            this.cmbFronHandleLight.SymbolSize = 24;
            this.cmbFronHandleLight.TabIndex = 4;
            this.cmbFronHandleLight.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbFronHandleLight.Watermark = "";
            // 
            // cmbHndlDoorLiDrvr
            // 
            this.cmbHndlDoorLiDrvr.DataSource = null;
            this.cmbHndlDoorLiDrvr.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbHndlDoorLiDrvr.FillColor = System.Drawing.Color.White;
            this.cmbHndlDoorLiDrvr.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbHndlDoorLiDrvr.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.cmbHndlDoorLiDrvr.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(242)))), ((int)(((byte)(212)))));
            this.cmbHndlDoorLiDrvr.ItemRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbHndlDoorLiDrvr.Items.AddRange(new object[] {
            "OFF",
            "ONStacic",
            "ONDynamic"});
            this.cmbHndlDoorLiDrvr.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbHndlDoorLiDrvr.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbHndlDoorLiDrvr.Location = new System.Drawing.Point(93, 172);
            this.cmbHndlDoorLiDrvr.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbHndlDoorLiDrvr.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbHndlDoorLiDrvr.Name = "cmbHndlDoorLiDrvr";
            this.cmbHndlDoorLiDrvr.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbHndlDoorLiDrvr.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbHndlDoorLiDrvr.ScrollBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbHndlDoorLiDrvr.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbHndlDoorLiDrvr.Size = new System.Drawing.Size(99, 23);
            this.cmbHndlDoorLiDrvr.Style = Sunny.UI.UIStyle.Custom;
            this.cmbHndlDoorLiDrvr.SymbolSize = 24;
            this.cmbHndlDoorLiDrvr.TabIndex = 4;
            this.cmbHndlDoorLiDrvr.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbHndlDoorLiDrvr.Watermark = "";
            // 
            // swStepLight
            // 
            this.swStepLight.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.swStepLight.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.swStepLight.Location = new System.Drawing.Point(93, 83);
            this.swStepLight.MinimumSize = new System.Drawing.Size(1, 1);
            this.swStepLight.Name = "swStepLight";
            this.swStepLight.Size = new System.Drawing.Size(75, 23);
            this.swStepLight.Style = Sunny.UI.UIStyle.Custom;
            this.swStepLight.TabIndex = 5;
            this.swStepLight.Text = "uiSwitch1";
            // 
            // swPocketLight
            // 
            this.swPocketLight.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.swPocketLight.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.swPocketLight.Location = new System.Drawing.Point(93, 53);
            this.swPocketLight.MinimumSize = new System.Drawing.Size(1, 1);
            this.swPocketLight.Name = "swPocketLight";
            this.swPocketLight.Size = new System.Drawing.Size(75, 23);
            this.swPocketLight.Style = Sunny.UI.UIStyle.Custom;
            this.swPocketLight.TabIndex = 5;
            this.swPocketLight.Text = "uiSwitch1";
            // 
            // swBsd
            // 
            this.swBsd.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.swBsd.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.swBsd.Location = new System.Drawing.Point(92, 25);
            this.swBsd.MinimumSize = new System.Drawing.Size(1, 1);
            this.swBsd.Name = "swBsd";
            this.swBsd.Size = new System.Drawing.Size(75, 23);
            this.swBsd.Style = Sunny.UI.UIStyle.Custom;
            this.swBsd.TabIndex = 5;
            this.swBsd.Text = "uiSwitch1";
            // 
            // txtMemoryVolt
            // 
            this.txtMemoryVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMemoryVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMemoryVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMemoryVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMemoryVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMemoryVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMemoryVolt.ButtonStyleInherited = false;
            this.txtMemoryVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMemoryVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtMemoryVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtMemoryVolt.Location = new System.Drawing.Point(236, 260);
            this.txtMemoryVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMemoryVolt.Maximum = 99999999D;
            this.txtMemoryVolt.Minimum = -99999999D;
            this.txtMemoryVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtMemoryVolt.Name = "txtMemoryVolt";
            this.txtMemoryVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtMemoryVolt.ReadOnly = true;
            this.txtMemoryVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMemoryVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMemoryVolt.ScrollBarStyleInherited = false;
            this.txtMemoryVolt.ShowText = false;
            this.txtMemoryVolt.Size = new System.Drawing.Size(39, 23);
            this.txtMemoryVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtMemoryVolt.TabIndex = 3;
            this.txtMemoryVolt.Text = "0.00";
            this.txtMemoryVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtMemoryVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtMemoryVolt.Watermark = "";
            // 
            // txtLockLightVolt
            // 
            this.txtLockLightVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtLockLightVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtLockLightVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtLockLightVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtLockLightVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtLockLightVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtLockLightVolt.ButtonStyleInherited = false;
            this.txtLockLightVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLockLightVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtLockLightVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtLockLightVolt.Location = new System.Drawing.Point(88, 260);
            this.txtLockLightVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtLockLightVolt.Maximum = 99999999D;
            this.txtLockLightVolt.Minimum = -99999999D;
            this.txtLockLightVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtLockLightVolt.Name = "txtLockLightVolt";
            this.txtLockLightVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtLockLightVolt.ReadOnly = true;
            this.txtLockLightVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtLockLightVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtLockLightVolt.ScrollBarStyleInherited = false;
            this.txtLockLightVolt.ShowText = false;
            this.txtLockLightVolt.Size = new System.Drawing.Size(42, 23);
            this.txtLockLightVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtLockLightVolt.TabIndex = 3;
            this.txtLockLightVolt.Text = "0.00";
            this.txtLockLightVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtLockLightVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtLockLightVolt.Watermark = "";
            // 
            // txtWindowLightVolt
            // 
            this.txtWindowLightVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtWindowLightVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtWindowLightVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtWindowLightVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtWindowLightVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtWindowLightVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtWindowLightVolt.ButtonStyleInherited = false;
            this.txtWindowLightVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtWindowLightVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtWindowLightVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtWindowLightVolt.Location = new System.Drawing.Point(201, 230);
            this.txtWindowLightVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtWindowLightVolt.Maximum = 99999999D;
            this.txtWindowLightVolt.Minimum = -99999999D;
            this.txtWindowLightVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtWindowLightVolt.Name = "txtWindowLightVolt";
            this.txtWindowLightVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtWindowLightVolt.ReadOnly = true;
            this.txtWindowLightVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtWindowLightVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtWindowLightVolt.ScrollBarStyleInherited = false;
            this.txtWindowLightVolt.ShowText = false;
            this.txtWindowLightVolt.Size = new System.Drawing.Size(74, 23);
            this.txtWindowLightVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtWindowLightVolt.TabIndex = 3;
            this.txtWindowLightVolt.Text = "0.00";
            this.txtWindowLightVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtWindowLightVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtWindowLightVolt.Watermark = "";
            // 
            // txtFronHandleLightVolt
            // 
            this.txtFronHandleLightVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtFronHandleLightVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtFronHandleLightVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtFronHandleLightVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtFronHandleLightVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtFronHandleLightVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtFronHandleLightVolt.ButtonStyleInherited = false;
            this.txtFronHandleLightVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFronHandleLightVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtFronHandleLightVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtFronHandleLightVolt.Location = new System.Drawing.Point(201, 201);
            this.txtFronHandleLightVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFronHandleLightVolt.Maximum = 99999999D;
            this.txtFronHandleLightVolt.Minimum = -99999999D;
            this.txtFronHandleLightVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtFronHandleLightVolt.Name = "txtFronHandleLightVolt";
            this.txtFronHandleLightVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtFronHandleLightVolt.ReadOnly = true;
            this.txtFronHandleLightVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtFronHandleLightVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtFronHandleLightVolt.ScrollBarStyleInherited = false;
            this.txtFronHandleLightVolt.ShowText = false;
            this.txtFronHandleLightVolt.Size = new System.Drawing.Size(74, 23);
            this.txtFronHandleLightVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtFronHandleLightVolt.TabIndex = 3;
            this.txtFronHandleLightVolt.Text = "0.00";
            this.txtFronHandleLightVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFronHandleLightVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtFronHandleLightVolt.Watermark = "";
            // 
            // txtHndlDoorLiDrvrVolt
            // 
            this.txtHndlDoorLiDrvrVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtHndlDoorLiDrvrVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtHndlDoorLiDrvrVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtHndlDoorLiDrvrVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtHndlDoorLiDrvrVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtHndlDoorLiDrvrVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtHndlDoorLiDrvrVolt.ButtonStyleInherited = false;
            this.txtHndlDoorLiDrvrVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtHndlDoorLiDrvrVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtHndlDoorLiDrvrVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtHndlDoorLiDrvrVolt.Location = new System.Drawing.Point(201, 172);
            this.txtHndlDoorLiDrvrVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtHndlDoorLiDrvrVolt.Maximum = 99999999D;
            this.txtHndlDoorLiDrvrVolt.Minimum = -99999999D;
            this.txtHndlDoorLiDrvrVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtHndlDoorLiDrvrVolt.Name = "txtHndlDoorLiDrvrVolt";
            this.txtHndlDoorLiDrvrVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtHndlDoorLiDrvrVolt.ReadOnly = true;
            this.txtHndlDoorLiDrvrVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtHndlDoorLiDrvrVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtHndlDoorLiDrvrVolt.ScrollBarStyleInherited = false;
            this.txtHndlDoorLiDrvrVolt.ShowText = false;
            this.txtHndlDoorLiDrvrVolt.Size = new System.Drawing.Size(74, 23);
            this.txtHndlDoorLiDrvrVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtHndlDoorLiDrvrVolt.TabIndex = 3;
            this.txtHndlDoorLiDrvrVolt.Text = "0.00";
            this.txtHndlDoorLiDrvrVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtHndlDoorLiDrvrVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtHndlDoorLiDrvrVolt.Watermark = "";
            // 
            // txtActvnOfPudLiVolt
            // 
            this.txtActvnOfPudLiVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtActvnOfPudLiVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtActvnOfPudLiVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtActvnOfPudLiVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtActvnOfPudLiVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtActvnOfPudLiVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtActvnOfPudLiVolt.ButtonStyleInherited = false;
            this.txtActvnOfPudLiVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtActvnOfPudLiVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtActvnOfPudLiVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtActvnOfPudLiVolt.Location = new System.Drawing.Point(175, 143);
            this.txtActvnOfPudLiVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtActvnOfPudLiVolt.Maximum = 99999999D;
            this.txtActvnOfPudLiVolt.Minimum = -99999999D;
            this.txtActvnOfPudLiVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtActvnOfPudLiVolt.Name = "txtActvnOfPudLiVolt";
            this.txtActvnOfPudLiVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtActvnOfPudLiVolt.ReadOnly = true;
            this.txtActvnOfPudLiVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtActvnOfPudLiVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtActvnOfPudLiVolt.ScrollBarStyleInherited = false;
            this.txtActvnOfPudLiVolt.ShowText = false;
            this.txtActvnOfPudLiVolt.Size = new System.Drawing.Size(100, 23);
            this.txtActvnOfPudLiVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtActvnOfPudLiVolt.TabIndex = 3;
            this.txtActvnOfPudLiVolt.Text = "0.00";
            this.txtActvnOfPudLiVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtActvnOfPudLiVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtActvnOfPudLiVolt.Watermark = "";
            // 
            // txtActvnOfIndcrOutVolt
            // 
            this.txtActvnOfIndcrOutVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtActvnOfIndcrOutVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtActvnOfIndcrOutVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtActvnOfIndcrOutVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtActvnOfIndcrOutVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtActvnOfIndcrOutVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtActvnOfIndcrOutVolt.ButtonStyleInherited = false;
            this.txtActvnOfIndcrOutVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtActvnOfIndcrOutVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtActvnOfIndcrOutVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtActvnOfIndcrOutVolt.Location = new System.Drawing.Point(175, 113);
            this.txtActvnOfIndcrOutVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtActvnOfIndcrOutVolt.Maximum = 99999999D;
            this.txtActvnOfIndcrOutVolt.Minimum = -99999999D;
            this.txtActvnOfIndcrOutVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtActvnOfIndcrOutVolt.Name = "txtActvnOfIndcrOutVolt";
            this.txtActvnOfIndcrOutVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtActvnOfIndcrOutVolt.ReadOnly = true;
            this.txtActvnOfIndcrOutVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtActvnOfIndcrOutVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtActvnOfIndcrOutVolt.ScrollBarStyleInherited = false;
            this.txtActvnOfIndcrOutVolt.ShowText = false;
            this.txtActvnOfIndcrOutVolt.Size = new System.Drawing.Size(100, 23);
            this.txtActvnOfIndcrOutVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtActvnOfIndcrOutVolt.TabIndex = 3;
            this.txtActvnOfIndcrOutVolt.Text = "0.00";
            this.txtActvnOfIndcrOutVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtActvnOfIndcrOutVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtActvnOfIndcrOutVolt.Watermark = "";
            // 
            // txtStepLightVolt
            // 
            this.txtStepLightVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtStepLightVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtStepLightVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtStepLightVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtStepLightVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtStepLightVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtStepLightVolt.ButtonStyleInherited = false;
            this.txtStepLightVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtStepLightVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtStepLightVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtStepLightVolt.Location = new System.Drawing.Point(175, 83);
            this.txtStepLightVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtStepLightVolt.Maximum = 99999999D;
            this.txtStepLightVolt.Minimum = -99999999D;
            this.txtStepLightVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtStepLightVolt.Name = "txtStepLightVolt";
            this.txtStepLightVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtStepLightVolt.ReadOnly = true;
            this.txtStepLightVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtStepLightVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtStepLightVolt.ScrollBarStyleInherited = false;
            this.txtStepLightVolt.ShowText = false;
            this.txtStepLightVolt.Size = new System.Drawing.Size(100, 23);
            this.txtStepLightVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtStepLightVolt.TabIndex = 3;
            this.txtStepLightVolt.Text = "0.00";
            this.txtStepLightVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtStepLightVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtStepLightVolt.Watermark = "";
            // 
            // txtPocketLightVolt
            // 
            this.txtPocketLightVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPocketLightVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtPocketLightVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtPocketLightVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPocketLightVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtPocketLightVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtPocketLightVolt.ButtonStyleInherited = false;
            this.txtPocketLightVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPocketLightVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtPocketLightVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtPocketLightVolt.Location = new System.Drawing.Point(175, 53);
            this.txtPocketLightVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPocketLightVolt.Maximum = 99999999D;
            this.txtPocketLightVolt.Minimum = -99999999D;
            this.txtPocketLightVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtPocketLightVolt.Name = "txtPocketLightVolt";
            this.txtPocketLightVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtPocketLightVolt.ReadOnly = true;
            this.txtPocketLightVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPocketLightVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPocketLightVolt.ScrollBarStyleInherited = false;
            this.txtPocketLightVolt.ShowText = false;
            this.txtPocketLightVolt.Size = new System.Drawing.Size(100, 23);
            this.txtPocketLightVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtPocketLightVolt.TabIndex = 3;
            this.txtPocketLightVolt.Text = "0.00";
            this.txtPocketLightVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPocketLightVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtPocketLightVolt.Watermark = "";
            // 
            // txtBsdVolt
            // 
            this.txtBsdVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtBsdVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtBsdVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtBsdVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtBsdVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtBsdVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtBsdVolt.ButtonStyleInherited = false;
            this.txtBsdVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBsdVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtBsdVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtBsdVolt.Location = new System.Drawing.Point(175, 25);
            this.txtBsdVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBsdVolt.Maximum = 99999999D;
            this.txtBsdVolt.Minimum = -99999999D;
            this.txtBsdVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtBsdVolt.Name = "txtBsdVolt";
            this.txtBsdVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtBsdVolt.ReadOnly = true;
            this.txtBsdVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtBsdVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtBsdVolt.ScrollBarStyleInherited = false;
            this.txtBsdVolt.ShowText = false;
            this.txtBsdVolt.Size = new System.Drawing.Size(100, 23);
            this.txtBsdVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtBsdVolt.TabIndex = 3;
            this.txtBsdVolt.Text = "0.00";
            this.txtBsdVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBsdVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtBsdVolt.Watermark = "";
            // 
            // uiMarkLabel29
            // 
            this.uiMarkLabel29.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel29.Location = new System.Drawing.Point(136, 260);
            this.uiMarkLabel29.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel29.Name = "uiMarkLabel29";
            this.uiMarkLabel29.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel29.Size = new System.Drawing.Size(97, 23);
            this.uiMarkLabel29.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel29.TabIndex = 2;
            this.uiMarkLabel29.Text = "记忆开关：";
            this.uiMarkLabel29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel5
            // 
            this.uiMarkLabel5.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel5.Location = new System.Drawing.Point(3, 260);
            this.uiMarkLabel5.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel5.Name = "uiMarkLabel5";
            this.uiMarkLabel5.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel5.Size = new System.Drawing.Size(80, 23);
            this.uiMarkLabel5.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel5.TabIndex = 2;
            this.uiMarkLabel5.Text = "锁开关：";
            this.uiMarkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel25
            // 
            this.uiMarkLabel25.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel25.Location = new System.Drawing.Point(3, 230);
            this.uiMarkLabel25.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel25.Name = "uiMarkLabel25";
            this.uiMarkLabel25.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel25.Size = new System.Drawing.Size(107, 23);
            this.uiMarkLabel25.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel25.TabIndex = 2;
            this.uiMarkLabel25.Text = "车窗开关背光：";
            this.uiMarkLabel25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel12
            // 
            this.uiMarkLabel12.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel12.Location = new System.Drawing.Point(3, 201);
            this.uiMarkLabel12.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel12.Name = "uiMarkLabel12";
            this.uiMarkLabel12.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel12.Size = new System.Drawing.Size(82, 23);
            this.uiMarkLabel12.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel12.TabIndex = 2;
            this.uiMarkLabel12.Text = "内门把手灯：";
            this.uiMarkLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel11
            // 
            this.uiMarkLabel11.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel11.Location = new System.Drawing.Point(3, 172);
            this.uiMarkLabel11.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel11.Name = "uiMarkLabel11";
            this.uiMarkLabel11.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel11.Size = new System.Drawing.Size(82, 23);
            this.uiMarkLabel11.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel11.TabIndex = 2;
            this.uiMarkLabel11.Text = "外门把手灯：";
            this.uiMarkLabel11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel10
            // 
            this.uiMarkLabel10.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel10.Location = new System.Drawing.Point(3, 143);
            this.uiMarkLabel10.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel10.Name = "uiMarkLabel10";
            this.uiMarkLabel10.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel10.Size = new System.Drawing.Size(82, 23);
            this.uiMarkLabel10.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel10.TabIndex = 2;
            this.uiMarkLabel10.Text = "照地灯：";
            this.uiMarkLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel9
            // 
            this.uiMarkLabel9.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel9.Location = new System.Drawing.Point(3, 113);
            this.uiMarkLabel9.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel9.Name = "uiMarkLabel9";
            this.uiMarkLabel9.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel9.Size = new System.Drawing.Size(82, 23);
            this.uiMarkLabel9.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel9.TabIndex = 2;
            this.uiMarkLabel9.Text = "转向灯：";
            this.uiMarkLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel7
            // 
            this.uiMarkLabel7.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel7.Location = new System.Drawing.Point(3, 83);
            this.uiMarkLabel7.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel7.Name = "uiMarkLabel7";
            this.uiMarkLabel7.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel7.Size = new System.Drawing.Size(82, 23);
            this.uiMarkLabel7.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel7.TabIndex = 2;
            this.uiMarkLabel7.Text = "踏步灯：";
            this.uiMarkLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel6
            // 
            this.uiMarkLabel6.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel6.Location = new System.Drawing.Point(3, 53);
            this.uiMarkLabel6.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel6.Name = "uiMarkLabel6";
            this.uiMarkLabel6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel6.Size = new System.Drawing.Size(82, 23);
            this.uiMarkLabel6.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel6.TabIndex = 2;
            this.uiMarkLabel6.Text = "口袋灯：";
            this.uiMarkLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel8
            // 
            this.uiMarkLabel8.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel8.Location = new System.Drawing.Point(3, 25);
            this.uiMarkLabel8.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel8.Name = "uiMarkLabel8";
            this.uiMarkLabel8.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel8.Size = new System.Drawing.Size(82, 23);
            this.uiMarkLabel8.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel8.TabIndex = 2;
            this.uiMarkLabel8.Text = "BSD：";
            this.uiMarkLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiGroupBox9
            // 
            this.uiGroupBox9.Controls.Add(this.rbChdLockReLeCtrlHmiReqUnlock);
            this.uiGroupBox9.Controls.Add(this.rbChdLockReLeCtrlHmiReqIdle);
            this.uiGroupBox9.Controls.Add(this.txtDcChildrenLock);
            this.uiGroupBox9.Controls.Add(this.rbChdLockReLeCtrlHmiReqLock);
            this.uiGroupBox9.Controls.Add(this.uiMarkLabel27);
            this.uiGroupBox9.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox9.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox9.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox9.Location = new System.Drawing.Point(294, 11);
            this.uiGroupBox9.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox9.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox9.Name = "uiGroupBox9";
            this.uiGroupBox9.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox9.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiGroupBox9.Size = new System.Drawing.Size(191, 78);
            this.uiGroupBox9.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox9.TabIndex = 0;
            this.uiGroupBox9.Text = "儿童锁";
            this.uiGroupBox9.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbChdLockReLeCtrlHmiReqUnlock
            // 
            this.rbChdLockReLeCtrlHmiReqUnlock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbChdLockReLeCtrlHmiReqUnlock.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbChdLockReLeCtrlHmiReqUnlock.GroupIndex = 4;
            this.rbChdLockReLeCtrlHmiReqUnlock.Location = new System.Drawing.Point(131, 52);
            this.rbChdLockReLeCtrlHmiReqUnlock.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbChdLockReLeCtrlHmiReqUnlock.Name = "rbChdLockReLeCtrlHmiReqUnlock";
            this.rbChdLockReLeCtrlHmiReqUnlock.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbChdLockReLeCtrlHmiReqUnlock.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbChdLockReLeCtrlHmiReqUnlock.Size = new System.Drawing.Size(49, 23);
            this.rbChdLockReLeCtrlHmiReqUnlock.Style = Sunny.UI.UIStyle.Custom;
            this.rbChdLockReLeCtrlHmiReqUnlock.TabIndex = 68;
            this.rbChdLockReLeCtrlHmiReqUnlock.Text = "解锁";
            // 
            // rbChdLockReLeCtrlHmiReqIdle
            // 
            this.rbChdLockReLeCtrlHmiReqIdle.Checked = true;
            this.rbChdLockReLeCtrlHmiReqIdle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbChdLockReLeCtrlHmiReqIdle.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbChdLockReLeCtrlHmiReqIdle.GroupIndex = 4;
            this.rbChdLockReLeCtrlHmiReqIdle.Location = new System.Drawing.Point(9, 52);
            this.rbChdLockReLeCtrlHmiReqIdle.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbChdLockReLeCtrlHmiReqIdle.Name = "rbChdLockReLeCtrlHmiReqIdle";
            this.rbChdLockReLeCtrlHmiReqIdle.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbChdLockReLeCtrlHmiReqIdle.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbChdLockReLeCtrlHmiReqIdle.Size = new System.Drawing.Size(49, 23);
            this.rbChdLockReLeCtrlHmiReqIdle.Style = Sunny.UI.UIStyle.Custom;
            this.rbChdLockReLeCtrlHmiReqIdle.TabIndex = 68;
            this.rbChdLockReLeCtrlHmiReqIdle.Text = "IDLE";
            // 
            // txtDcChildrenLock
            // 
            this.txtDcChildrenLock.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcChildrenLock.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtDcChildrenLock.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtDcChildrenLock.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcChildrenLock.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcChildrenLock.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcChildrenLock.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtDcChildrenLock.Location = new System.Drawing.Point(80, 26);
            this.txtDcChildrenLock.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDcChildrenLock.Maximum = 100;
            this.txtDcChildrenLock.Minimum = 0;
            this.txtDcChildrenLock.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtDcChildrenLock.Name = "txtDcChildrenLock";
            this.txtDcChildrenLock.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcChildrenLock.ShowText = false;
            this.txtDcChildrenLock.Size = new System.Drawing.Size(100, 23);
            this.txtDcChildrenLock.Step = 10;
            this.txtDcChildrenLock.Style = Sunny.UI.UIStyle.Custom;
            this.txtDcChildrenLock.TabIndex = 8;
            this.txtDcChildrenLock.Text = "_uiIntegerUpDown1";
            this.txtDcChildrenLock.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbChdLockReLeCtrlHmiReqLock
            // 
            this.rbChdLockReLeCtrlHmiReqLock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbChdLockReLeCtrlHmiReqLock.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbChdLockReLeCtrlHmiReqLock.GroupIndex = 4;
            this.rbChdLockReLeCtrlHmiReqLock.Location = new System.Drawing.Point(71, 52);
            this.rbChdLockReLeCtrlHmiReqLock.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbChdLockReLeCtrlHmiReqLock.Name = "rbChdLockReLeCtrlHmiReqLock";
            this.rbChdLockReLeCtrlHmiReqLock.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbChdLockReLeCtrlHmiReqLock.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbChdLockReLeCtrlHmiReqLock.Size = new System.Drawing.Size(49, 23);
            this.rbChdLockReLeCtrlHmiReqLock.Style = Sunny.UI.UIStyle.Custom;
            this.rbChdLockReLeCtrlHmiReqLock.TabIndex = 68;
            this.rbChdLockReLeCtrlHmiReqLock.Text = "上锁";
            // 
            // uiMarkLabel27
            // 
            this.uiMarkLabel27.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel27.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel27.Location = new System.Drawing.Point(6, 26);
            this.uiMarkLabel27.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel27.Name = "uiMarkLabel27";
            this.uiMarkLabel27.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel27.Size = new System.Drawing.Size(67, 23);
            this.uiMarkLabel27.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel27.TabIndex = 2;
            this.uiMarkLabel27.Text = "占空比：";
            this.uiMarkLabel27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupRelayBtns
            // 
            this.groupRelayBtns.Controls.Add(this.uiButton21);
            this.groupRelayBtns.Controls.Add(this.uiButton11);
            this.groupRelayBtns.Controls.Add(this.uiButton20);
            this.groupRelayBtns.Controls.Add(this.uiButton9);
            this.groupRelayBtns.Controls.Add(this.uiButton19);
            this.groupRelayBtns.Controls.Add(this.uiButton7);
            this.groupRelayBtns.Controls.Add(this.uiButton18);
            this.groupRelayBtns.Controls.Add(this.uiButton5);
            this.groupRelayBtns.Controls.Add(this.uiButton17);
            this.groupRelayBtns.Controls.Add(this.uiButton3);
            this.groupRelayBtns.Controls.Add(this.uiButton1);
            this.groupRelayBtns.Controls.Add(this.uiButton24);
            this.groupRelayBtns.Controls.Add(this.uiButton23);
            this.groupRelayBtns.Controls.Add(this.uiButton22);
            this.groupRelayBtns.Controls.Add(this.uiButton16);
            this.groupRelayBtns.Controls.Add(this.uiButton15);
            this.groupRelayBtns.Controls.Add(this.uiButton10);
            this.groupRelayBtns.Controls.Add(this.uiButton14);
            this.groupRelayBtns.Controls.Add(this.uiButton8);
            this.groupRelayBtns.Controls.Add(this.uiButton13);
            this.groupRelayBtns.Controls.Add(this.uiButton6);
            this.groupRelayBtns.Controls.Add(this.uiButton12);
            this.groupRelayBtns.Controls.Add(this.uiButton4);
            this.groupRelayBtns.Controls.Add(this.uiButton2);
            this.groupRelayBtns.Controls.Add(this.btnAddCustomController);
            this.groupRelayBtns.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.groupRelayBtns.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.groupRelayBtns.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupRelayBtns.Location = new System.Drawing.Point(757, 5);
            this.groupRelayBtns.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupRelayBtns.MinimumSize = new System.Drawing.Size(1, 1);
            this.groupRelayBtns.Name = "groupRelayBtns";
            this.groupRelayBtns.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.groupRelayBtns.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.groupRelayBtns.Size = new System.Drawing.Size(263, 389);
            this.groupRelayBtns.Style = Sunny.UI.UIStyle.Custom;
            this.groupRelayBtns.TabIndex = 0;
            this.groupRelayBtns.Text = "继电器";
            this.groupRelayBtns.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiButton21
            // 
            this.uiButton21.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton21.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton21.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton21.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton21.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton21.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton21.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton21.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton21.Location = new System.Drawing.Point(133, 303);
            this.uiButton21.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton21.Name = "uiButton21";
            this.uiButton21.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton21.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton21.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton21.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton21.Size = new System.Drawing.Size(124, 26);
            this.uiButton21.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton21.TabIndex = 2;
            this.uiButton21.Text = "隐藏门把手位置反馈2";
            this.uiButton21.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton11
            // 
            this.uiButton11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton11.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton11.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton11.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton11.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton11.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton11.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton11.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton11.Location = new System.Drawing.Point(133, 164);
            this.uiButton11.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton11.Name = "uiButton11";
            this.uiButton11.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton11.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton11.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton11.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton11.Size = new System.Drawing.Size(124, 26);
            this.uiButton11.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton11.TabIndex = 2;
            this.uiButton11.Text = "记忆开关1/2 1.2kΩ";
            this.uiButton11.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton20
            // 
            this.uiButton20.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton20.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton20.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton20.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton20.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton20.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton20.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton20.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton20.Location = new System.Drawing.Point(133, 276);
            this.uiButton20.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton20.Name = "uiButton20";
            this.uiButton20.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton20.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton20.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton20.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton20.Size = new System.Drawing.Size(124, 26);
            this.uiButton20.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton20.TabIndex = 2;
            this.uiButton20.Text = "门Ajar开关";
            this.uiButton20.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton9
            // 
            this.uiButton9.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton9.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton9.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton9.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton9.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton9.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton9.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton9.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton9.Location = new System.Drawing.Point(133, 136);
            this.uiButton9.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton9.Name = "uiButton9";
            this.uiButton9.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton9.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton9.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton9.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton9.Size = new System.Drawing.Size(124, 26);
            this.uiButton9.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton9.TabIndex = 2;
            this.uiButton9.Text = "车窗本地开关 402Ω";
            this.uiButton9.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton19
            // 
            this.uiButton19.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton19.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton19.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton19.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton19.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton19.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton19.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton19.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton19.Location = new System.Drawing.Point(133, 248);
            this.uiButton19.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton19.Name = "uiButton19";
            this.uiButton19.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton19.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton19.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton19.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton19.Size = new System.Drawing.Size(124, 26);
            this.uiButton19.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton19.TabIndex = 2;
            this.uiButton19.Text = "棘爪（半开）开关";
            this.uiButton19.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton7
            // 
            this.uiButton7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton7.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton7.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton7.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton7.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton7.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton7.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton7.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton7.Location = new System.Drawing.Point(133, 108);
            this.uiButton7.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton7.Name = "uiButton7";
            this.uiButton7.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton7.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton7.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton7.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton7.Size = new System.Drawing.Size(124, 26);
            this.uiButton7.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton7.TabIndex = 2;
            this.uiButton7.Text = "中控锁开关820Ω";
            this.uiButton7.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton18
            // 
            this.uiButton18.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton18.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton18.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton18.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton18.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton18.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton18.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton18.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton18.Location = new System.Drawing.Point(133, 220);
            this.uiButton18.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton18.Name = "uiButton18";
            this.uiButton18.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton18.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton18.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton18.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton18.Size = new System.Drawing.Size(124, 26);
            this.uiButton18.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton18.TabIndex = 2;
            this.uiButton18.Text = "中控锁反馈开关";
            this.uiButton18.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton5
            // 
            this.uiButton5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton5.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton5.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton5.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton5.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton5.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton5.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton5.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton5.Location = new System.Drawing.Point(133, 81);
            this.uiButton5.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton5.Name = "uiButton5";
            this.uiButton5.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton5.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton5.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton5.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton5.Size = new System.Drawing.Size(124, 26);
            this.uiButton5.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton5.TabIndex = 2;
            this.uiButton5.Text = "内把手开关2";
            this.uiButton5.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton17
            // 
            this.uiButton17.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton17.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton17.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton17.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton17.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton17.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton17.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton17.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton17.Location = new System.Drawing.Point(133, 192);
            this.uiButton17.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton17.Name = "uiButton17";
            this.uiButton17.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton17.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton17.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton17.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton17.Size = new System.Drawing.Size(124, 26);
            this.uiButton17.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton17.TabIndex = 2;
            this.uiButton17.Text = "记忆开关M/3 1.2KΩ";
            this.uiButton17.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton3
            // 
            this.uiButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton3.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton3.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton3.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton3.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton3.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton3.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton3.Location = new System.Drawing.Point(133, 53);
            this.uiButton3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton3.Name = "uiButton3";
            this.uiButton3.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton3.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton3.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton3.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton3.Size = new System.Drawing.Size(124, 26);
            this.uiButton3.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton3.TabIndex = 2;
            this.uiButton3.Text = "电动门电机霍尔信号2";
            this.uiButton3.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton1.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton1.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton1.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton1.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton1.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton1.Location = new System.Drawing.Point(133, 26);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton1.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton1.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton1.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton1.Size = new System.Drawing.Size(124, 26);
            this.uiButton1.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton1.TabIndex = 2;
            this.uiButton1.Text = "后遮阳帘电机霍尔信号2";
            this.uiButton1.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton24
            // 
            this.uiButton24.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton24.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton24.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton24.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton24.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton24.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton24.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton24.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton24.Location = new System.Drawing.Point(133, 331);
            this.uiButton24.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton24.Name = "uiButton24";
            this.uiButton24.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton24.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton24.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton24.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton24.Size = new System.Drawing.Size(124, 26);
            this.uiButton24.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton24.TabIndex = 2;
            this.uiButton24.Text = "后视镜上下(Y轴)位置检测";
            this.uiButton24.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton23
            // 
            this.uiButton23.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton23.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton23.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton23.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton23.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton23.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton23.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton23.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton23.Location = new System.Drawing.Point(3, 358);
            this.uiButton23.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton23.Name = "uiButton23";
            this.uiButton23.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton23.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton23.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton23.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton23.Size = new System.Drawing.Size(124, 26);
            this.uiButton23.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton23.TabIndex = 2;
            this.uiButton23.Text = "外温传感器";
            this.uiButton23.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton22
            // 
            this.uiButton22.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton22.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton22.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton22.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton22.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton22.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton22.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton22.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton22.Location = new System.Drawing.Point(3, 331);
            this.uiButton22.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton22.Name = "uiButton22";
            this.uiButton22.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton22.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton22.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton22.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton22.Size = new System.Drawing.Size(124, 26);
            this.uiButton22.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton22.TabIndex = 2;
            this.uiButton22.Text = "后视镜左右(X轴)位置检测";
            this.uiButton22.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton16
            // 
            this.uiButton16.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton16.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton16.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton16.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton16.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton16.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton16.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton16.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton16.Location = new System.Drawing.Point(3, 303);
            this.uiButton16.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton16.Name = "uiButton16";
            this.uiButton16.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton16.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton16.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton16.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton16.Size = new System.Drawing.Size(124, 26);
            this.uiButton16.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton16.TabIndex = 2;
            this.uiButton16.Text = "隐藏门把手位置反馈1";
            this.uiButton16.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton15
            // 
            this.uiButton15.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton15.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton15.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton15.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton15.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton15.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton15.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton15.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton15.Location = new System.Drawing.Point(3, 276);
            this.uiButton15.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton15.Name = "uiButton15";
            this.uiButton15.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton15.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton15.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton15.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton15.Size = new System.Drawing.Size(124, 26);
            this.uiButton15.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton15.TabIndex = 2;
            this.uiButton15.Text = "门全开开关";
            this.uiButton15.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton10
            // 
            this.uiButton10.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton10.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton10.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton10.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton10.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton10.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton10.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton10.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton10.Location = new System.Drawing.Point(3, 164);
            this.uiButton10.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton10.Name = "uiButton10";
            this.uiButton10.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton10.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton10.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton10.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton10.Size = new System.Drawing.Size(124, 26);
            this.uiButton10.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton10.TabIndex = 2;
            this.uiButton10.Text = "车窗本地开关 3.6kΩ";
            this.uiButton10.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton14
            // 
            this.uiButton14.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton14.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton14.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton14.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton14.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton14.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton14.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton14.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton14.Location = new System.Drawing.Point(3, 248);
            this.uiButton14.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton14.Name = "uiButton14";
            this.uiButton14.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton14.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton14.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton14.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton14.Size = new System.Drawing.Size(124, 26);
            this.uiButton14.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton14.TabIndex = 2;
            this.uiButton14.Text = "吸合锁复位开关";
            this.uiButton14.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton8
            // 
            this.uiButton8.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton8.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton8.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton8.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton8.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton8.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton8.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton8.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton8.Location = new System.Drawing.Point(3, 136);
            this.uiButton8.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton8.Name = "uiButton8";
            this.uiButton8.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton8.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton8.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton8.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton8.Size = new System.Drawing.Size(124, 26);
            this.uiButton8.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton8.TabIndex = 2;
            this.uiButton8.Text = "中控锁开关2.2KΩ";
            this.uiButton8.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton13
            // 
            this.uiButton13.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton13.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton13.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton13.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton13.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton13.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton13.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton13.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton13.Location = new System.Drawing.Point(3, 220);
            this.uiButton13.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton13.Name = "uiButton13";
            this.uiButton13.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton13.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton13.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton13.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton13.Size = new System.Drawing.Size(124, 26);
            this.uiButton13.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton13.TabIndex = 2;
            this.uiButton13.Text = "记忆开关M/3 634Ω";
            this.uiButton13.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton6
            // 
            this.uiButton6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton6.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton6.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton6.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton6.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton6.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton6.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton6.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton6.Location = new System.Drawing.Point(3, 108);
            this.uiButton6.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton6.Name = "uiButton6";
            this.uiButton6.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton6.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton6.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton6.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton6.Size = new System.Drawing.Size(124, 26);
            this.uiButton6.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton6.TabIndex = 2;
            this.uiButton6.Text = "内把手开关1";
            this.uiButton6.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton12
            // 
            this.uiButton12.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton12.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton12.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton12.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton12.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton12.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton12.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton12.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton12.Location = new System.Drawing.Point(3, 192);
            this.uiButton12.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton12.Name = "uiButton12";
            this.uiButton12.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton12.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton12.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton12.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton12.Size = new System.Drawing.Size(124, 26);
            this.uiButton12.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton12.TabIndex = 2;
            this.uiButton12.Text = "记忆开关1/2 634Ω";
            this.uiButton12.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton4
            // 
            this.uiButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton4.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton4.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton4.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton4.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton4.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton4.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton4.Location = new System.Drawing.Point(3, 81);
            this.uiButton4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton4.Name = "uiButton4";
            this.uiButton4.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton4.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton4.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton4.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton4.Size = new System.Drawing.Size(124, 26);
            this.uiButton4.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton4.TabIndex = 2;
            this.uiButton4.Text = "外把手开关";
            this.uiButton4.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiButton2
            // 
            this.uiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton2.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton2.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton2.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton2.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton2.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.uiButton2.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiButton2.Location = new System.Drawing.Point(3, 53);
            this.uiButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiButton2.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.uiButton2.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton2.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.uiButton2.Size = new System.Drawing.Size(124, 26);
            this.uiButton2.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton2.TabIndex = 2;
            this.uiButton2.Text = "电动门电机霍尔信号1";
            this.uiButton2.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // btnAddCustomController
            // 
            this.btnAddCustomController.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddCustomController.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.btnAddCustomController.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.btnAddCustomController.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.btnAddCustomController.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.btnAddCustomController.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.btnAddCustomController.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.btnAddCustomController.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.btnAddCustomController.Location = new System.Drawing.Point(3, 26);
            this.btnAddCustomController.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAddCustomController.Name = "btnAddCustomController";
            this.btnAddCustomController.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.btnAddCustomController.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.btnAddCustomController.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.btnAddCustomController.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.btnAddCustomController.Size = new System.Drawing.Size(124, 26);
            this.btnAddCustomController.Style = Sunny.UI.UIStyle.Custom;
            this.btnAddCustomController.TabIndex = 2;
            this.btnAddCustomController.Text = "后遮阳帘电机霍尔信号1";
            this.btnAddCustomController.TipsFont = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // uiGroupBox12
            // 
            this.uiGroupBox12.Controls.Add(this.dgvCurrent);
            this.uiGroupBox12.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox12.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox12.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiGroupBox12.Location = new System.Drawing.Point(590, 392);
            this.uiGroupBox12.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox12.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox12.Name = "uiGroupBox12";
            this.uiGroupBox12.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox12.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiGroupBox12.Size = new System.Drawing.Size(421, 296);
            this.uiGroupBox12.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox12.TabIndex = 0;
            this.uiGroupBox12.Text = "电机运行电流/mA";
            this.uiGroupBox12.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvCurrent
            // 
            this.dgvCurrent.AllowUserToAddRows = false;
            this.dgvCurrent.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.dgvCurrent.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCurrent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCurrent.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.dgvCurrent.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCurrent.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCurrent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(242)))), ((int)(((byte)(212)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCurrent.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCurrent.EnableHeadersVisualStyles = false;
            this.dgvCurrent.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvCurrent.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(199)))), ((int)(((byte)(69)))));
            this.dgvCurrent.Location = new System.Drawing.Point(0, 32);
            this.dgvCurrent.Name = "dgvCurrent";
            this.dgvCurrent.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 8F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCurrent.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvCurrent.RowHeadersVisible = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 8F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(242)))), ((int)(((byte)(212)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.dgvCurrent.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvCurrent.RowTemplate.Height = 23;
            this.dgvCurrent.ScrollBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.dgvCurrent.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.dgvCurrent.ScrollBarHandleWidth = 40;
            this.dgvCurrent.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.dgvCurrent.ScrollBarStyleInherited = false;
            this.dgvCurrent.SelectedIndex = -1;
            this.dgvCurrent.Size = new System.Drawing.Size(421, 264);
            this.dgvCurrent.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.dgvCurrent.Style = Sunny.UI.UIStyle.Custom;
            this.dgvCurrent.TabIndex = 7;
            // 
            // uiGroupBox11
            // 
            this.uiGroupBox11.Controls.Add(this.dgvState);
            this.uiGroupBox11.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox11.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox11.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox11.Location = new System.Drawing.Point(492, 5);
            this.uiGroupBox11.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox11.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox11.Name = "uiGroupBox11";
            this.uiGroupBox11.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox11.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiGroupBox11.Size = new System.Drawing.Size(258, 368);
            this.uiGroupBox11.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox11.TabIndex = 0;
            this.uiGroupBox11.Text = "状态";
            this.uiGroupBox11.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvState
            // 
            this.dgvState.AllowUserToAddRows = false;
            this.dgvState.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.dgvState.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvState.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvState.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.dgvState.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvState.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvState.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(242)))), ((int)(((byte)(212)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvState.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvState.EnableHeadersVisualStyles = false;
            this.dgvState.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvState.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(199)))), ((int)(((byte)(69)))));
            this.dgvState.Location = new System.Drawing.Point(0, 32);
            this.dgvState.Name = "dgvState";
            this.dgvState.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 8F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvState.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvState.RowHeadersVisible = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 8F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(242)))), ((int)(((byte)(212)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.dgvState.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvState.RowTemplate.Height = 23;
            this.dgvState.ScrollBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.dgvState.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.dgvState.ScrollBarHandleWidth = 40;
            this.dgvState.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.dgvState.ScrollBarStyleInherited = false;
            this.dgvState.SelectedIndex = -1;
            this.dgvState.Size = new System.Drawing.Size(258, 336);
            this.dgvState.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.dgvState.Style = Sunny.UI.UIStyle.Custom;
            this.dgvState.TabIndex = 7;
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.Controls.Add(this.uiLedBulb2);
            this.uiGroupBox3.Controls.Add(this.swStartCan);
            this.uiGroupBox3.Controls.Add(this.uiMarkLabel18);
            this.uiGroupBox3.Controls.Add(this.uiMarkLabel14);
            this.uiGroupBox3.Controls.Add(this.uiMarkLabel22);
            this.uiGroupBox3.Controls.Add(this.txtVbat2);
            this.uiGroupBox3.Controls.Add(this.uiMarkLabel21);
            this.uiGroupBox3.Controls.Add(this.txtMotorDoorSpeed);
            this.uiGroupBox3.Controls.Add(this.txtMotorPedalSpeed);
            this.uiGroupBox3.Controls.Add(this.txtTemperature);
            this.uiGroupBox3.Controls.Add(this.txtVbat1);
            this.uiGroupBox3.Controls.Add(this.uiMarkLabel24);
            this.uiGroupBox3.Controls.Add(this.uiMarkLabel23);
            this.uiGroupBox3.Controls.Add(this.uiMarkLabel13);
            this.uiGroupBox3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox3.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox3.Location = new System.Drawing.Point(4, 43);
            this.uiGroupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox3.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiGroupBox3.Size = new System.Drawing.Size(278, 174);
            this.uiGroupBox3.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox3.TabIndex = 0;
            this.uiGroupBox3.Text = "通信、显示";
            this.uiGroupBox3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLedBulb2
            // 
            this.uiLedBulb2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.uiLedBulb2.Location = new System.Drawing.Point(242, 32);
            this.uiLedBulb2.Name = "uiLedBulb2";
            this.uiLedBulb2.Size = new System.Drawing.Size(23, 23);
            this.uiLedBulb2.TabIndex = 71;
            this.uiLedBulb2.Text = "uiLedBulb2";
            // 
            // swStartCan
            // 
            this.swStartCan.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.swStartCan.ActiveText = "唤醒";
            this.swStartCan.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.swStartCan.InActiveText = "休眠";
            this.swStartCan.Location = new System.Drawing.Point(88, 32);
            this.swStartCan.MinimumSize = new System.Drawing.Size(1, 1);
            this.swStartCan.Name = "swStartCan";
            this.swStartCan.Size = new System.Drawing.Size(50, 23);
            this.swStartCan.Style = Sunny.UI.UIStyle.Custom;
            this.swStartCan.TabIndex = 5;
            this.swStartCan.Text = "uiSwitch1";
            // 
            // uiMarkLabel18
            // 
            this.uiMarkLabel18.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel18.Location = new System.Drawing.Point(143, 32);
            this.uiMarkLabel18.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel18.Name = "uiMarkLabel18";
            this.uiMarkLabel18.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel18.Size = new System.Drawing.Size(92, 23);
            this.uiMarkLabel18.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel18.TabIndex = 2;
            this.uiMarkLabel18.Text = "CAN通信检测：";
            this.uiMarkLabel18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel14
            // 
            this.uiMarkLabel14.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel14.Location = new System.Drawing.Point(3, 32);
            this.uiMarkLabel14.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel14.Name = "uiMarkLabel14";
            this.uiMarkLabel14.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel14.Size = new System.Drawing.Size(80, 23);
            this.uiMarkLabel14.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel14.TabIndex = 2;
            this.uiMarkLabel14.Text = "休眠/唤醒：";
            this.uiMarkLabel14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel22
            // 
            this.uiMarkLabel22.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel22.Location = new System.Drawing.Point(143, 61);
            this.uiMarkLabel22.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel22.Name = "uiMarkLabel22";
            this.uiMarkLabel22.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel22.Size = new System.Drawing.Size(61, 23);
            this.uiMarkLabel22.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel22.TabIndex = 2;
            this.uiMarkLabel22.Text = "VBAT2：";
            this.uiMarkLabel22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtVbat2
            // 
            this.txtVbat2.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtVbat2.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtVbat2.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtVbat2.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtVbat2.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtVbat2.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtVbat2.ButtonStyleInherited = false;
            this.txtVbat2.CanEmpty = true;
            this.txtVbat2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtVbat2.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtVbat2.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtVbat2.Location = new System.Drawing.Point(207, 60);
            this.txtVbat2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtVbat2.Maximum = 65535D;
            this.txtVbat2.Minimum = -65535D;
            this.txtVbat2.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtVbat2.Name = "txtVbat2";
            this.txtVbat2.Padding = new System.Windows.Forms.Padding(5);
            this.txtVbat2.ReadOnly = true;
            this.txtVbat2.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtVbat2.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtVbat2.ScrollBarStyleInherited = false;
            this.txtVbat2.ShowText = false;
            this.txtVbat2.Size = new System.Drawing.Size(67, 23);
            this.txtVbat2.Style = Sunny.UI.UIStyle.Custom;
            this.txtVbat2.TabIndex = 3;
            this.txtVbat2.Text = "0.00";
            this.txtVbat2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtVbat2.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtVbat2.Watermark = "";
            // 
            // uiMarkLabel21
            // 
            this.uiMarkLabel21.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel21.Location = new System.Drawing.Point(3, 60);
            this.uiMarkLabel21.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel21.Name = "uiMarkLabel21";
            this.uiMarkLabel21.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel21.Size = new System.Drawing.Size(61, 23);
            this.uiMarkLabel21.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel21.TabIndex = 2;
            this.uiMarkLabel21.Text = "VBAT1：";
            this.uiMarkLabel21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMotorDoorSpeed
            // 
            this.txtMotorDoorSpeed.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMotorDoorSpeed.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMotorDoorSpeed.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMotorDoorSpeed.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMotorDoorSpeed.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMotorDoorSpeed.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMotorDoorSpeed.ButtonStyleInherited = false;
            this.txtMotorDoorSpeed.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMotorDoorSpeed.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtMotorDoorSpeed.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtMotorDoorSpeed.Location = new System.Drawing.Point(137, 145);
            this.txtMotorDoorSpeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMotorDoorSpeed.Maximum = 100D;
            this.txtMotorDoorSpeed.Minimum = 0D;
            this.txtMotorDoorSpeed.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtMotorDoorSpeed.Name = "txtMotorDoorSpeed";
            this.txtMotorDoorSpeed.Padding = new System.Windows.Forms.Padding(5);
            this.txtMotorDoorSpeed.ReadOnly = true;
            this.txtMotorDoorSpeed.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMotorDoorSpeed.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMotorDoorSpeed.ScrollBarStyleInherited = false;
            this.txtMotorDoorSpeed.ShowText = false;
            this.txtMotorDoorSpeed.Size = new System.Drawing.Size(128, 23);
            this.txtMotorDoorSpeed.Style = Sunny.UI.UIStyle.Custom;
            this.txtMotorDoorSpeed.TabIndex = 3;
            this.txtMotorDoorSpeed.Text = "0.00";
            this.txtMotorDoorSpeed.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtMotorDoorSpeed.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtMotorDoorSpeed.Watermark = "";
            // 
            // txtMotorPedalSpeed
            // 
            this.txtMotorPedalSpeed.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMotorPedalSpeed.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMotorPedalSpeed.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMotorPedalSpeed.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMotorPedalSpeed.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMotorPedalSpeed.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMotorPedalSpeed.ButtonStyleInherited = false;
            this.txtMotorPedalSpeed.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMotorPedalSpeed.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtMotorPedalSpeed.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtMotorPedalSpeed.Location = new System.Drawing.Point(137, 116);
            this.txtMotorPedalSpeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMotorPedalSpeed.Maximum = 100D;
            this.txtMotorPedalSpeed.Minimum = 0D;
            this.txtMotorPedalSpeed.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtMotorPedalSpeed.Name = "txtMotorPedalSpeed";
            this.txtMotorPedalSpeed.Padding = new System.Windows.Forms.Padding(5);
            this.txtMotorPedalSpeed.ReadOnly = true;
            this.txtMotorPedalSpeed.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMotorPedalSpeed.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMotorPedalSpeed.ScrollBarStyleInherited = false;
            this.txtMotorPedalSpeed.ShowText = false;
            this.txtMotorPedalSpeed.Size = new System.Drawing.Size(128, 23);
            this.txtMotorPedalSpeed.Style = Sunny.UI.UIStyle.Custom;
            this.txtMotorPedalSpeed.TabIndex = 3;
            this.txtMotorPedalSpeed.Text = "0.00";
            this.txtMotorPedalSpeed.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtMotorPedalSpeed.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtMotorPedalSpeed.Watermark = "";
            // 
            // txtTemperature
            // 
            this.txtTemperature.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtTemperature.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtTemperature.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtTemperature.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtTemperature.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtTemperature.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtTemperature.ButtonStyleInherited = false;
            this.txtTemperature.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTemperature.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtTemperature.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtTemperature.Location = new System.Drawing.Point(137, 89);
            this.txtTemperature.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTemperature.Maximum = 65535D;
            this.txtTemperature.Minimum = -65535D;
            this.txtTemperature.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtTemperature.Name = "txtTemperature";
            this.txtTemperature.Padding = new System.Windows.Forms.Padding(5);
            this.txtTemperature.ReadOnly = true;
            this.txtTemperature.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtTemperature.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtTemperature.ScrollBarStyleInherited = false;
            this.txtTemperature.ShowText = false;
            this.txtTemperature.Size = new System.Drawing.Size(128, 23);
            this.txtTemperature.Style = Sunny.UI.UIStyle.Custom;
            this.txtTemperature.TabIndex = 3;
            this.txtTemperature.Text = "0.00";
            this.txtTemperature.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtTemperature.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtTemperature.Watermark = "";
            // 
            // txtVbat1
            // 
            this.txtVbat1.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtVbat1.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtVbat1.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtVbat1.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtVbat1.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtVbat1.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtVbat1.ButtonStyleInherited = false;
            this.txtVbat1.CanEmpty = true;
            this.txtVbat1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtVbat1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtVbat1.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtVbat1.Location = new System.Drawing.Point(69, 60);
            this.txtVbat1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtVbat1.Maximum = 65535D;
            this.txtVbat1.Minimum = -65535D;
            this.txtVbat1.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtVbat1.Name = "txtVbat1";
            this.txtVbat1.Padding = new System.Windows.Forms.Padding(5);
            this.txtVbat1.ReadOnly = true;
            this.txtVbat1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtVbat1.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtVbat1.ScrollBarStyleInherited = false;
            this.txtVbat1.ShowText = false;
            this.txtVbat1.Size = new System.Drawing.Size(67, 23);
            this.txtVbat1.Style = Sunny.UI.UIStyle.Custom;
            this.txtVbat1.TabIndex = 3;
            this.txtVbat1.Text = "0.00";
            this.txtVbat1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtVbat1.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtVbat1.Watermark = "";
            // 
            // uiMarkLabel24
            // 
            this.uiMarkLabel24.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel24.Location = new System.Drawing.Point(3, 145);
            this.uiMarkLabel24.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel24.Name = "uiMarkLabel24";
            this.uiMarkLabel24.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel24.Size = new System.Drawing.Size(127, 23);
            this.uiMarkLabel24.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel24.TabIndex = 2;
            this.uiMarkLabel24.Text = "电动门电机速度：";
            this.uiMarkLabel24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel23
            // 
            this.uiMarkLabel23.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel23.Location = new System.Drawing.Point(3, 116);
            this.uiMarkLabel23.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel23.Name = "uiMarkLabel23";
            this.uiMarkLabel23.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel23.Size = new System.Drawing.Size(127, 23);
            this.uiMarkLabel23.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel23.TabIndex = 2;
            this.uiMarkLabel23.Text = "电动脚踏电机速度：";
            this.uiMarkLabel23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel13
            // 
            this.uiMarkLabel13.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel13.Location = new System.Drawing.Point(3, 89);
            this.uiMarkLabel13.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel13.Name = "uiMarkLabel13";
            this.uiMarkLabel13.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel13.Size = new System.Drawing.Size(127, 23);
            this.uiMarkLabel13.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel13.TabIndex = 2;
            this.uiMarkLabel13.Text = "温度：";
            this.uiMarkLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiGroupBox10
            // 
            this.uiGroupBox10.Controls.Add(this.swPowSupplyConDDSswitch);
            this.uiGroupBox10.Controls.Add(this.swPowSupplyConDoorHall);
            this.uiGroupBox10.Controls.Add(this.swPowSupplyConHandleHall);
            this.uiGroupBox10.Controls.Add(this.swPowSupplyCmdPos);
            this.uiGroupBox10.Controls.Add(this.swPowSupplyConPendalHall);
            this.uiGroupBox10.Controls.Add(this.uiMarkLabel37);
            this.uiGroupBox10.Controls.Add(this.uiMarkLabel36);
            this.uiGroupBox10.Controls.Add(this.uiMarkLabel35);
            this.uiGroupBox10.Controls.Add(this.uiMarkLabel33);
            this.uiGroupBox10.Controls.Add(this.uiMarkLabel34);
            this.uiGroupBox10.Controls.Add(this.txtPowSupplyConDDSswitchVolt);
            this.uiGroupBox10.Controls.Add(this.txtPowSupplyConDoorHallVolt);
            this.uiGroupBox10.Controls.Add(this.txtPowSupplyConHandleHallVolt);
            this.uiGroupBox10.Controls.Add(this.txtPowSupplyCmdPosVolt);
            this.uiGroupBox10.Controls.Add(this.txtowSupplyConPendalHallVolt);
            this.uiGroupBox10.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox10.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox10.Location = new System.Drawing.Point(294, 505);
            this.uiGroupBox10.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox10.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox10.Name = "uiGroupBox10";
            this.uiGroupBox10.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox10.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiGroupBox10.Size = new System.Drawing.Size(288, 183);
            this.uiGroupBox10.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox10.TabIndex = 0;
            this.uiGroupBox10.Text = "霍尔电机控制";
            this.uiGroupBox10.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // swPowSupplyConDDSswitch
            // 
            this.swPowSupplyConDDSswitch.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.swPowSupplyConDDSswitch.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.swPowSupplyConDDSswitch.Location = new System.Drawing.Point(150, 152);
            this.swPowSupplyConDDSswitch.MinimumSize = new System.Drawing.Size(1, 1);
            this.swPowSupplyConDDSswitch.Name = "swPowSupplyConDDSswitch";
            this.swPowSupplyConDDSswitch.Size = new System.Drawing.Size(72, 28);
            this.swPowSupplyConDDSswitch.Style = Sunny.UI.UIStyle.Custom;
            this.swPowSupplyConDDSswitch.TabIndex = 5;
            this.swPowSupplyConDDSswitch.Text = "uiSwitch1";
            // 
            // swPowSupplyConDoorHall
            // 
            this.swPowSupplyConDoorHall.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.swPowSupplyConDoorHall.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.swPowSupplyConDoorHall.Location = new System.Drawing.Point(150, 122);
            this.swPowSupplyConDoorHall.MinimumSize = new System.Drawing.Size(1, 1);
            this.swPowSupplyConDoorHall.Name = "swPowSupplyConDoorHall";
            this.swPowSupplyConDoorHall.Size = new System.Drawing.Size(72, 28);
            this.swPowSupplyConDoorHall.Style = Sunny.UI.UIStyle.Custom;
            this.swPowSupplyConDoorHall.TabIndex = 5;
            this.swPowSupplyConDoorHall.Text = "uiSwitch1";
            // 
            // swPowSupplyConHandleHall
            // 
            this.swPowSupplyConHandleHall.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.swPowSupplyConHandleHall.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.swPowSupplyConHandleHall.Location = new System.Drawing.Point(150, 89);
            this.swPowSupplyConHandleHall.MinimumSize = new System.Drawing.Size(1, 1);
            this.swPowSupplyConHandleHall.Name = "swPowSupplyConHandleHall";
            this.swPowSupplyConHandleHall.Size = new System.Drawing.Size(72, 28);
            this.swPowSupplyConHandleHall.Style = Sunny.UI.UIStyle.Custom;
            this.swPowSupplyConHandleHall.TabIndex = 5;
            this.swPowSupplyConHandleHall.Text = "uiSwitch1";
            // 
            // swPowSupplyCmdPos
            // 
            this.swPowSupplyCmdPos.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.swPowSupplyCmdPos.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.swPowSupplyCmdPos.Location = new System.Drawing.Point(150, 56);
            this.swPowSupplyCmdPos.MinimumSize = new System.Drawing.Size(1, 1);
            this.swPowSupplyCmdPos.Name = "swPowSupplyCmdPos";
            this.swPowSupplyCmdPos.Size = new System.Drawing.Size(72, 28);
            this.swPowSupplyCmdPos.Style = Sunny.UI.UIStyle.Custom;
            this.swPowSupplyCmdPos.TabIndex = 5;
            this.swPowSupplyCmdPos.Text = "uiSwitch1";
            // 
            // swPowSupplyConPendalHall
            // 
            this.swPowSupplyConPendalHall.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.swPowSupplyConPendalHall.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.swPowSupplyConPendalHall.Location = new System.Drawing.Point(150, 25);
            this.swPowSupplyConPendalHall.MinimumSize = new System.Drawing.Size(1, 1);
            this.swPowSupplyConPendalHall.Name = "swPowSupplyConPendalHall";
            this.swPowSupplyConPendalHall.Size = new System.Drawing.Size(72, 28);
            this.swPowSupplyConPendalHall.Style = Sunny.UI.UIStyle.Custom;
            this.swPowSupplyConPendalHall.TabIndex = 5;
            this.swPowSupplyConPendalHall.Text = "uiSwitch1";
            // 
            // uiMarkLabel37
            // 
            this.uiMarkLabel37.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel37.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel37.Location = new System.Drawing.Point(3, 152);
            this.uiMarkLabel37.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel37.Name = "uiMarkLabel37";
            this.uiMarkLabel37.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel37.Size = new System.Drawing.Size(140, 28);
            this.uiMarkLabel37.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel37.TabIndex = 2;
            this.uiMarkLabel37.Text = "DDS开关供电：";
            this.uiMarkLabel37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel36
            // 
            this.uiMarkLabel36.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel36.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel36.Location = new System.Drawing.Point(3, 122);
            this.uiMarkLabel36.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel36.Name = "uiMarkLabel36";
            this.uiMarkLabel36.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel36.Size = new System.Drawing.Size(140, 28);
            this.uiMarkLabel36.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel36.TabIndex = 2;
            this.uiMarkLabel36.Text = "电动门电机霍尔供电：";
            this.uiMarkLabel36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel35
            // 
            this.uiMarkLabel35.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel35.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel35.Location = new System.Drawing.Point(3, 89);
            this.uiMarkLabel35.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel35.Name = "uiMarkLabel35";
            this.uiMarkLabel35.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel35.Size = new System.Drawing.Size(140, 28);
            this.uiMarkLabel35.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel35.TabIndex = 2;
            this.uiMarkLabel35.Text = "隐藏门把手霍尔供电：";
            this.uiMarkLabel35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel33
            // 
            this.uiMarkLabel33.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel33.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel33.Location = new System.Drawing.Point(3, 56);
            this.uiMarkLabel33.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel33.Name = "uiMarkLabel33";
            this.uiMarkLabel33.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel33.Size = new System.Drawing.Size(140, 28);
            this.uiMarkLabel33.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel33.TabIndex = 2;
            this.uiMarkLabel33.Text = "位置传感器供电：";
            this.uiMarkLabel33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel34
            // 
            this.uiMarkLabel34.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel34.Location = new System.Drawing.Point(3, 25);
            this.uiMarkLabel34.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel34.Name = "uiMarkLabel34";
            this.uiMarkLabel34.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel34.Size = new System.Drawing.Size(141, 28);
            this.uiMarkLabel34.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel34.TabIndex = 2;
            this.uiMarkLabel34.Text = "电动踏板霍尔供电：";
            this.uiMarkLabel34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPowSupplyConDDSswitchVolt
            // 
            this.txtPowSupplyConDDSswitchVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyConDDSswitchVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtPowSupplyConDDSswitchVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtPowSupplyConDDSswitchVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyConDDSswitchVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtPowSupplyConDDSswitchVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtPowSupplyConDDSswitchVolt.ButtonStyleInherited = false;
            this.txtPowSupplyConDDSswitchVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPowSupplyConDDSswitchVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtPowSupplyConDDSswitchVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtPowSupplyConDDSswitchVolt.Location = new System.Drawing.Point(228, 152);
            this.txtPowSupplyConDDSswitchVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPowSupplyConDDSswitchVolt.Maximum = 99999999D;
            this.txtPowSupplyConDDSswitchVolt.Minimum = -99999999D;
            this.txtPowSupplyConDDSswitchVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtPowSupplyConDDSswitchVolt.Name = "txtPowSupplyConDDSswitchVolt";
            this.txtPowSupplyConDDSswitchVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtPowSupplyConDDSswitchVolt.ReadOnly = true;
            this.txtPowSupplyConDDSswitchVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyConDDSswitchVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyConDDSswitchVolt.ScrollBarStyleInherited = false;
            this.txtPowSupplyConDDSswitchVolt.ShowText = false;
            this.txtPowSupplyConDDSswitchVolt.Size = new System.Drawing.Size(55, 23);
            this.txtPowSupplyConDDSswitchVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtPowSupplyConDDSswitchVolt.TabIndex = 3;
            this.txtPowSupplyConDDSswitchVolt.Text = "0.00";
            this.txtPowSupplyConDDSswitchVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPowSupplyConDDSswitchVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtPowSupplyConDDSswitchVolt.Watermark = "";
            // 
            // txtPowSupplyConDoorHallVolt
            // 
            this.txtPowSupplyConDoorHallVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyConDoorHallVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtPowSupplyConDoorHallVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtPowSupplyConDoorHallVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyConDoorHallVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtPowSupplyConDoorHallVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtPowSupplyConDoorHallVolt.ButtonStyleInherited = false;
            this.txtPowSupplyConDoorHallVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPowSupplyConDoorHallVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtPowSupplyConDoorHallVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtPowSupplyConDoorHallVolt.Location = new System.Drawing.Point(229, 122);
            this.txtPowSupplyConDoorHallVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPowSupplyConDoorHallVolt.Maximum = 99999999D;
            this.txtPowSupplyConDoorHallVolt.Minimum = -99999999D;
            this.txtPowSupplyConDoorHallVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtPowSupplyConDoorHallVolt.Name = "txtPowSupplyConDoorHallVolt";
            this.txtPowSupplyConDoorHallVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtPowSupplyConDoorHallVolt.ReadOnly = true;
            this.txtPowSupplyConDoorHallVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyConDoorHallVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyConDoorHallVolt.ScrollBarStyleInherited = false;
            this.txtPowSupplyConDoorHallVolt.ShowText = false;
            this.txtPowSupplyConDoorHallVolt.Size = new System.Drawing.Size(55, 23);
            this.txtPowSupplyConDoorHallVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtPowSupplyConDoorHallVolt.TabIndex = 3;
            this.txtPowSupplyConDoorHallVolt.Text = "0.00";
            this.txtPowSupplyConDoorHallVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPowSupplyConDoorHallVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtPowSupplyConDoorHallVolt.Watermark = "";
            // 
            // txtPowSupplyConHandleHallVolt
            // 
            this.txtPowSupplyConHandleHallVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyConHandleHallVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtPowSupplyConHandleHallVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtPowSupplyConHandleHallVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyConHandleHallVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtPowSupplyConHandleHallVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtPowSupplyConHandleHallVolt.ButtonStyleInherited = false;
            this.txtPowSupplyConHandleHallVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPowSupplyConHandleHallVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtPowSupplyConHandleHallVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtPowSupplyConHandleHallVolt.Location = new System.Drawing.Point(229, 89);
            this.txtPowSupplyConHandleHallVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPowSupplyConHandleHallVolt.Maximum = 99999999D;
            this.txtPowSupplyConHandleHallVolt.Minimum = -99999999D;
            this.txtPowSupplyConHandleHallVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtPowSupplyConHandleHallVolt.Name = "txtPowSupplyConHandleHallVolt";
            this.txtPowSupplyConHandleHallVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtPowSupplyConHandleHallVolt.ReadOnly = true;
            this.txtPowSupplyConHandleHallVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyConHandleHallVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyConHandleHallVolt.ScrollBarStyleInherited = false;
            this.txtPowSupplyConHandleHallVolt.ShowText = false;
            this.txtPowSupplyConHandleHallVolt.Size = new System.Drawing.Size(55, 23);
            this.txtPowSupplyConHandleHallVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtPowSupplyConHandleHallVolt.TabIndex = 3;
            this.txtPowSupplyConHandleHallVolt.Text = "0.00";
            this.txtPowSupplyConHandleHallVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPowSupplyConHandleHallVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtPowSupplyConHandleHallVolt.Watermark = "";
            // 
            // txtPowSupplyCmdPosVolt
            // 
            this.txtPowSupplyCmdPosVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyCmdPosVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtPowSupplyCmdPosVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtPowSupplyCmdPosVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyCmdPosVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtPowSupplyCmdPosVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtPowSupplyCmdPosVolt.ButtonStyleInherited = false;
            this.txtPowSupplyCmdPosVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPowSupplyCmdPosVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtPowSupplyCmdPosVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtPowSupplyCmdPosVolt.Location = new System.Drawing.Point(229, 56);
            this.txtPowSupplyCmdPosVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPowSupplyCmdPosVolt.Maximum = 99999999D;
            this.txtPowSupplyCmdPosVolt.Minimum = -99999999D;
            this.txtPowSupplyCmdPosVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtPowSupplyCmdPosVolt.Name = "txtPowSupplyCmdPosVolt";
            this.txtPowSupplyCmdPosVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtPowSupplyCmdPosVolt.ReadOnly = true;
            this.txtPowSupplyCmdPosVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyCmdPosVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtPowSupplyCmdPosVolt.ScrollBarStyleInherited = false;
            this.txtPowSupplyCmdPosVolt.ShowText = false;
            this.txtPowSupplyCmdPosVolt.Size = new System.Drawing.Size(55, 23);
            this.txtPowSupplyCmdPosVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtPowSupplyCmdPosVolt.TabIndex = 3;
            this.txtPowSupplyCmdPosVolt.Text = "0.00";
            this.txtPowSupplyCmdPosVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPowSupplyCmdPosVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtPowSupplyCmdPosVolt.Watermark = "";
            // 
            // txtowSupplyConPendalHallVolt
            // 
            this.txtowSupplyConPendalHallVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtowSupplyConPendalHallVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtowSupplyConPendalHallVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtowSupplyConPendalHallVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtowSupplyConPendalHallVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtowSupplyConPendalHallVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtowSupplyConPendalHallVolt.ButtonStyleInherited = false;
            this.txtowSupplyConPendalHallVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtowSupplyConPendalHallVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtowSupplyConPendalHallVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtowSupplyConPendalHallVolt.Location = new System.Drawing.Point(229, 25);
            this.txtowSupplyConPendalHallVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtowSupplyConPendalHallVolt.Maximum = 99999999D;
            this.txtowSupplyConPendalHallVolt.Minimum = -99999999D;
            this.txtowSupplyConPendalHallVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtowSupplyConPendalHallVolt.Name = "txtowSupplyConPendalHallVolt";
            this.txtowSupplyConPendalHallVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtowSupplyConPendalHallVolt.ReadOnly = true;
            this.txtowSupplyConPendalHallVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtowSupplyConPendalHallVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtowSupplyConPendalHallVolt.ScrollBarStyleInherited = false;
            this.txtowSupplyConPendalHallVolt.ShowText = false;
            this.txtowSupplyConPendalHallVolt.Size = new System.Drawing.Size(55, 23);
            this.txtowSupplyConPendalHallVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtowSupplyConPendalHallVolt.TabIndex = 3;
            this.txtowSupplyConPendalHallVolt.Text = "0.00";
            this.txtowSupplyConPendalHallVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtowSupplyConPendalHallVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtowSupplyConPendalHallVolt.Watermark = "";
            // 
            // uiGroupBox8
            // 
            this.uiGroupBox8.Controls.Add(this.rbPedalClose);
            this.uiGroupBox8.Controls.Add(this.rbPedalIdle);
            this.uiGroupBox8.Controls.Add(this.uiMarkLabel26);
            this.uiGroupBox8.Controls.Add(this.rbPedalOpen);
            this.uiGroupBox8.Controls.Add(this.txtDcPedal);
            this.uiGroupBox8.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox8.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox8.Location = new System.Drawing.Point(294, 341);
            this.uiGroupBox8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox8.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox8.Name = "uiGroupBox8";
            this.uiGroupBox8.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox8.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiGroupBox8.Size = new System.Drawing.Size(195, 78);
            this.uiGroupBox8.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox8.TabIndex = 0;
            this.uiGroupBox8.Text = "电动踏板";
            this.uiGroupBox8.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbPedalClose
            // 
            this.rbPedalClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbPedalClose.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbPedalClose.GroupIndex = 3;
            this.rbPedalClose.Location = new System.Drawing.Point(131, 50);
            this.rbPedalClose.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbPedalClose.Name = "rbPedalClose";
            this.rbPedalClose.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbPedalClose.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbPedalClose.Size = new System.Drawing.Size(49, 23);
            this.rbPedalClose.Style = Sunny.UI.UIStyle.Custom;
            this.rbPedalClose.TabIndex = 68;
            this.rbPedalClose.Text = "缩回";
            // 
            // rbPedalIdle
            // 
            this.rbPedalIdle.Checked = true;
            this.rbPedalIdle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbPedalIdle.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbPedalIdle.GroupIndex = 3;
            this.rbPedalIdle.Location = new System.Drawing.Point(9, 51);
            this.rbPedalIdle.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbPedalIdle.Name = "rbPedalIdle";
            this.rbPedalIdle.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbPedalIdle.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbPedalIdle.Size = new System.Drawing.Size(49, 23);
            this.rbPedalIdle.Style = Sunny.UI.UIStyle.Custom;
            this.rbPedalIdle.TabIndex = 68;
            this.rbPedalIdle.Text = "IDLE";
            // 
            // uiMarkLabel26
            // 
            this.uiMarkLabel26.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel26.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel26.Location = new System.Drawing.Point(6, 25);
            this.uiMarkLabel26.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel26.Name = "uiMarkLabel26";
            this.uiMarkLabel26.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel26.Size = new System.Drawing.Size(67, 23);
            this.uiMarkLabel26.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel26.TabIndex = 2;
            this.uiMarkLabel26.Text = "占空比：";
            this.uiMarkLabel26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbPedalOpen
            // 
            this.rbPedalOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbPedalOpen.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbPedalOpen.GroupIndex = 3;
            this.rbPedalOpen.Location = new System.Drawing.Point(71, 51);
            this.rbPedalOpen.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbPedalOpen.Name = "rbPedalOpen";
            this.rbPedalOpen.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbPedalOpen.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbPedalOpen.Size = new System.Drawing.Size(49, 23);
            this.rbPedalOpen.Style = Sunny.UI.UIStyle.Custom;
            this.rbPedalOpen.TabIndex = 68;
            this.rbPedalOpen.Text = "伸出";
            // 
            // txtDcPedal
            // 
            this.txtDcPedal.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcPedal.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtDcPedal.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtDcPedal.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcPedal.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcPedal.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcPedal.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtDcPedal.Location = new System.Drawing.Point(80, 25);
            this.txtDcPedal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDcPedal.Maximum = 100;
            this.txtDcPedal.Minimum = 0;
            this.txtDcPedal.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtDcPedal.Name = "txtDcPedal";
            this.txtDcPedal.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcPedal.ShowText = false;
            this.txtDcPedal.Size = new System.Drawing.Size(100, 23);
            this.txtDcPedal.Step = 10;
            this.txtDcPedal.Style = Sunny.UI.UIStyle.Custom;
            this.txtDcPedal.TabIndex = 8;
            this.txtDcPedal.Text = "_uiIntegerUpDown1";
            this.txtDcPedal.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiGroupBox7
            // 
            this.uiGroupBox7.Controls.Add(this.rbDoorDrvrHndlCmdClose);
            this.uiGroupBox7.Controls.Add(this.rbDoorDrvrHndlCmdIdle);
            this.uiGroupBox7.Controls.Add(this.uiMarkLabel17);
            this.uiGroupBox7.Controls.Add(this.txtDcHandle);
            this.uiGroupBox7.Controls.Add(this.rbDoorDrvrHndlCmdOpen);
            this.uiGroupBox7.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox7.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox7.Location = new System.Drawing.Point(294, 257);
            this.uiGroupBox7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox7.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox7.Name = "uiGroupBox7";
            this.uiGroupBox7.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox7.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiGroupBox7.Size = new System.Drawing.Size(195, 82);
            this.uiGroupBox7.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox7.TabIndex = 0;
            this.uiGroupBox7.Text = "隐藏门把手";
            this.uiGroupBox7.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbDoorDrvrHndlCmdClose
            // 
            this.rbDoorDrvrHndlCmdClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbDoorDrvrHndlCmdClose.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbDoorDrvrHndlCmdClose.GroupIndex = 2;
            this.rbDoorDrvrHndlCmdClose.Location = new System.Drawing.Point(131, 53);
            this.rbDoorDrvrHndlCmdClose.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbDoorDrvrHndlCmdClose.Name = "rbDoorDrvrHndlCmdClose";
            this.rbDoorDrvrHndlCmdClose.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbDoorDrvrHndlCmdClose.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbDoorDrvrHndlCmdClose.Size = new System.Drawing.Size(49, 23);
            this.rbDoorDrvrHndlCmdClose.Style = Sunny.UI.UIStyle.Custom;
            this.rbDoorDrvrHndlCmdClose.TabIndex = 68;
            this.rbDoorDrvrHndlCmdClose.Text = "折叠";
            // 
            // rbDoorDrvrHndlCmdIdle
            // 
            this.rbDoorDrvrHndlCmdIdle.Checked = true;
            this.rbDoorDrvrHndlCmdIdle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbDoorDrvrHndlCmdIdle.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbDoorDrvrHndlCmdIdle.GroupIndex = 2;
            this.rbDoorDrvrHndlCmdIdle.Location = new System.Drawing.Point(9, 53);
            this.rbDoorDrvrHndlCmdIdle.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbDoorDrvrHndlCmdIdle.Name = "rbDoorDrvrHndlCmdIdle";
            this.rbDoorDrvrHndlCmdIdle.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbDoorDrvrHndlCmdIdle.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbDoorDrvrHndlCmdIdle.Size = new System.Drawing.Size(49, 23);
            this.rbDoorDrvrHndlCmdIdle.Style = Sunny.UI.UIStyle.Custom;
            this.rbDoorDrvrHndlCmdIdle.TabIndex = 68;
            this.rbDoorDrvrHndlCmdIdle.Text = "IDLE";
            // 
            // uiMarkLabel17
            // 
            this.uiMarkLabel17.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel17.Location = new System.Drawing.Point(6, 27);
            this.uiMarkLabel17.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel17.Name = "uiMarkLabel17";
            this.uiMarkLabel17.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel17.Size = new System.Drawing.Size(67, 23);
            this.uiMarkLabel17.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel17.TabIndex = 2;
            this.uiMarkLabel17.Text = "占空比：";
            this.uiMarkLabel17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDcHandle
            // 
            this.txtDcHandle.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcHandle.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtDcHandle.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtDcHandle.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcHandle.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcHandle.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcHandle.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtDcHandle.Location = new System.Drawing.Point(80, 27);
            this.txtDcHandle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDcHandle.Maximum = 100;
            this.txtDcHandle.Minimum = 0;
            this.txtDcHandle.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtDcHandle.Name = "txtDcHandle";
            this.txtDcHandle.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcHandle.ShowText = false;
            this.txtDcHandle.Size = new System.Drawing.Size(100, 23);
            this.txtDcHandle.Step = 10;
            this.txtDcHandle.Style = Sunny.UI.UIStyle.Custom;
            this.txtDcHandle.TabIndex = 8;
            this.txtDcHandle.Text = "_uiIntegerUpDown1";
            this.txtDcHandle.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbDoorDrvrHndlCmdOpen
            // 
            this.rbDoorDrvrHndlCmdOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbDoorDrvrHndlCmdOpen.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbDoorDrvrHndlCmdOpen.GroupIndex = 2;
            this.rbDoorDrvrHndlCmdOpen.Location = new System.Drawing.Point(71, 53);
            this.rbDoorDrvrHndlCmdOpen.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbDoorDrvrHndlCmdOpen.Name = "rbDoorDrvrHndlCmdOpen";
            this.rbDoorDrvrHndlCmdOpen.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbDoorDrvrHndlCmdOpen.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbDoorDrvrHndlCmdOpen.Size = new System.Drawing.Size(49, 23);
            this.rbDoorDrvrHndlCmdOpen.Style = Sunny.UI.UIStyle.Custom;
            this.rbDoorDrvrHndlCmdOpen.TabIndex = 68;
            this.rbDoorDrvrHndlCmdOpen.Text = "展开";
            // 
            // uiGroupBox6
            // 
            this.uiGroupBox6.Controls.Add(this.rbShortDropWinOpen);
            this.uiGroupBox6.Controls.Add(this.uiMarkLabel15);
            this.uiGroupBox6.Controls.Add(this.rbShortDropWinClose);
            this.uiGroupBox6.Controls.Add(this.txtDcWindow);
            this.uiGroupBox6.Controls.Add(this.rbShortDropWinIdle);
            this.uiGroupBox6.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox6.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox6.Location = new System.Drawing.Point(294, 174);
            this.uiGroupBox6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox6.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox6.Name = "uiGroupBox6";
            this.uiGroupBox6.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox6.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiGroupBox6.Size = new System.Drawing.Size(191, 80);
            this.uiGroupBox6.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox6.TabIndex = 0;
            this.uiGroupBox6.Text = "车窗";
            this.uiGroupBox6.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbShortDropWinOpen
            // 
            this.rbShortDropWinOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbShortDropWinOpen.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbShortDropWinOpen.GroupIndex = 1;
            this.rbShortDropWinOpen.Location = new System.Drawing.Point(131, 51);
            this.rbShortDropWinOpen.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbShortDropWinOpen.Name = "rbShortDropWinOpen";
            this.rbShortDropWinOpen.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbShortDropWinOpen.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbShortDropWinOpen.Size = new System.Drawing.Size(49, 23);
            this.rbShortDropWinOpen.Style = Sunny.UI.UIStyle.Custom;
            this.rbShortDropWinOpen.TabIndex = 68;
            this.rbShortDropWinOpen.Text = "短降";
            // 
            // uiMarkLabel15
            // 
            this.uiMarkLabel15.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel15.Location = new System.Drawing.Point(6, 24);
            this.uiMarkLabel15.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel15.Name = "uiMarkLabel15";
            this.uiMarkLabel15.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel15.Size = new System.Drawing.Size(67, 23);
            this.uiMarkLabel15.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel15.TabIndex = 2;
            this.uiMarkLabel15.Text = "占空比：";
            this.uiMarkLabel15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbShortDropWinClose
            // 
            this.rbShortDropWinClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbShortDropWinClose.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbShortDropWinClose.GroupIndex = 1;
            this.rbShortDropWinClose.Location = new System.Drawing.Point(71, 51);
            this.rbShortDropWinClose.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbShortDropWinClose.Name = "rbShortDropWinClose";
            this.rbShortDropWinClose.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbShortDropWinClose.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbShortDropWinClose.Size = new System.Drawing.Size(49, 23);
            this.rbShortDropWinClose.Style = Sunny.UI.UIStyle.Custom;
            this.rbShortDropWinClose.TabIndex = 68;
            this.rbShortDropWinClose.Text = "短升";
            // 
            // txtDcWindow
            // 
            this.txtDcWindow.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcWindow.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtDcWindow.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtDcWindow.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcWindow.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcWindow.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcWindow.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtDcWindow.Location = new System.Drawing.Point(80, 24);
            this.txtDcWindow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDcWindow.Maximum = 100;
            this.txtDcWindow.Minimum = 0;
            this.txtDcWindow.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtDcWindow.Name = "txtDcWindow";
            this.txtDcWindow.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcWindow.ShowText = false;
            this.txtDcWindow.Size = new System.Drawing.Size(100, 23);
            this.txtDcWindow.Step = 10;
            this.txtDcWindow.Style = Sunny.UI.UIStyle.Custom;
            this.txtDcWindow.TabIndex = 8;
            this.txtDcWindow.Text = "_uiIntegerUpDown1";
            this.txtDcWindow.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbShortDropWinIdle
            // 
            this.rbShortDropWinIdle.Checked = true;
            this.rbShortDropWinIdle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbShortDropWinIdle.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbShortDropWinIdle.GroupIndex = 1;
            this.rbShortDropWinIdle.Location = new System.Drawing.Point(9, 51);
            this.rbShortDropWinIdle.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbShortDropWinIdle.Name = "rbShortDropWinIdle";
            this.rbShortDropWinIdle.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbShortDropWinIdle.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbShortDropWinIdle.Size = new System.Drawing.Size(49, 23);
            this.rbShortDropWinIdle.Style = Sunny.UI.UIStyle.Custom;
            this.rbShortDropWinIdle.TabIndex = 68;
            this.rbShortDropWinIdle.Text = "IDLE";
            // 
            // uiGroupBox5
            // 
            this.uiGroupBox5.Controls.Add(this.rbDoorClose);
            this.uiGroupBox5.Controls.Add(this.rbDoorOpen);
            this.uiGroupBox5.Controls.Add(this.rbDoorIdle);
            this.uiGroupBox5.Controls.Add(this.txtDcDoor);
            this.uiGroupBox5.Controls.Add(this.uiMarkLabel16);
            this.uiGroupBox5.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox5.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox5.Location = new System.Drawing.Point(294, 88);
            this.uiGroupBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox5.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox5.Name = "uiGroupBox5";
            this.uiGroupBox5.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox5.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiGroupBox5.Size = new System.Drawing.Size(191, 85);
            this.uiGroupBox5.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox5.TabIndex = 0;
            this.uiGroupBox5.Text = "电动门";
            this.uiGroupBox5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbDoorClose
            // 
            this.rbDoorClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbDoorClose.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbDoorClose.Location = new System.Drawing.Point(131, 54);
            this.rbDoorClose.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbDoorClose.Name = "rbDoorClose";
            this.rbDoorClose.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbDoorClose.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbDoorClose.Size = new System.Drawing.Size(49, 23);
            this.rbDoorClose.Style = Sunny.UI.UIStyle.Custom;
            this.rbDoorClose.TabIndex = 68;
            this.rbDoorClose.Text = "关门";
            // 
            // rbDoorOpen
            // 
            this.rbDoorOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbDoorOpen.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbDoorOpen.Location = new System.Drawing.Point(71, 54);
            this.rbDoorOpen.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbDoorOpen.Name = "rbDoorOpen";
            this.rbDoorOpen.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbDoorOpen.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbDoorOpen.Size = new System.Drawing.Size(49, 23);
            this.rbDoorOpen.Style = Sunny.UI.UIStyle.Custom;
            this.rbDoorOpen.TabIndex = 68;
            this.rbDoorOpen.Text = "开门";
            // 
            // rbDoorIdle
            // 
            this.rbDoorIdle.Checked = true;
            this.rbDoorIdle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbDoorIdle.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.rbDoorIdle.Location = new System.Drawing.Point(9, 54);
            this.rbDoorIdle.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbDoorIdle.Name = "rbDoorIdle";
            this.rbDoorIdle.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbDoorIdle.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.rbDoorIdle.Size = new System.Drawing.Size(49, 23);
            this.rbDoorIdle.Style = Sunny.UI.UIStyle.Custom;
            this.rbDoorIdle.TabIndex = 68;
            this.rbDoorIdle.Text = "IDLE";
            // 
            // txtDcDoor
            // 
            this.txtDcDoor.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcDoor.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtDcDoor.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtDcDoor.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcDoor.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcDoor.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcDoor.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtDcDoor.Location = new System.Drawing.Point(80, 27);
            this.txtDcDoor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDcDoor.Maximum = 100;
            this.txtDcDoor.Minimum = 0;
            this.txtDcDoor.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtDcDoor.Name = "txtDcDoor";
            this.txtDcDoor.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcDoor.ShowText = false;
            this.txtDcDoor.Size = new System.Drawing.Size(100, 23);
            this.txtDcDoor.Step = 10;
            this.txtDcDoor.Style = Sunny.UI.UIStyle.Custom;
            this.txtDcDoor.TabIndex = 8;
            this.txtDcDoor.Text = "_uiIntegerUpDown1";
            this.txtDcDoor.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel16
            // 
            this.uiMarkLabel16.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel16.Location = new System.Drawing.Point(6, 27);
            this.uiMarkLabel16.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel16.Name = "uiMarkLabel16";
            this.uiMarkLabel16.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel16.Size = new System.Drawing.Size(67, 23);
            this.uiMarkLabel16.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel16.TabIndex = 2;
            this.uiMarkLabel16.Text = "占空比：";
            this.uiMarkLabel16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiGroupBox4
            // 
            this.uiGroupBox4.Controls.Add(this.txtDcElectricRelese);
            this.uiGroupBox4.Controls.Add(this.txtDcLock2);
            this.uiGroupBox4.Controls.Add(this.uiMarkLabel19);
            this.uiGroupBox4.Controls.Add(this.uiMarkLabel20);
            this.uiGroupBox4.Controls.Add(this.cmbMotorCmd);
            this.uiGroupBox4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox4.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox4.Location = new System.Drawing.Point(294, 423);
            this.uiGroupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox4.Name = "uiGroupBox4";
            this.uiGroupBox4.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox4.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiGroupBox4.Size = new System.Drawing.Size(235, 79);
            this.uiGroupBox4.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox4.TabIndex = 0;
            this.uiGroupBox4.Text = "吸合/电释放电机控制";
            this.uiGroupBox4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDcElectricRelese
            // 
            this.txtDcElectricRelese.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcElectricRelese.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtDcElectricRelese.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtDcElectricRelese.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcElectricRelese.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcElectricRelese.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcElectricRelese.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtDcElectricRelese.Location = new System.Drawing.Point(52, 52);
            this.txtDcElectricRelese.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDcElectricRelese.Maximum = 100;
            this.txtDcElectricRelese.Minimum = 0;
            this.txtDcElectricRelese.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtDcElectricRelese.Name = "txtDcElectricRelese";
            this.txtDcElectricRelese.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcElectricRelese.ShowText = false;
            this.txtDcElectricRelese.Size = new System.Drawing.Size(100, 23);
            this.txtDcElectricRelese.Step = 10;
            this.txtDcElectricRelese.Style = Sunny.UI.UIStyle.Custom;
            this.txtDcElectricRelese.TabIndex = 8;
            this.txtDcElectricRelese.Text = "_uiIntegerUpDown1";
            this.txtDcElectricRelese.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDcLock2
            // 
            this.txtDcLock2.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcLock2.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtDcLock2.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtDcLock2.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcLock2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcLock2.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcLock2.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtDcLock2.Location = new System.Drawing.Point(52, 26);
            this.txtDcLock2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDcLock2.Maximum = 100;
            this.txtDcLock2.Minimum = 0;
            this.txtDcLock2.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtDcLock2.Name = "txtDcLock2";
            this.txtDcLock2.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcLock2.ShowText = false;
            this.txtDcLock2.Size = new System.Drawing.Size(100, 23);
            this.txtDcLock2.Step = 10;
            this.txtDcLock2.Style = Sunny.UI.UIStyle.Custom;
            this.txtDcLock2.TabIndex = 8;
            this.txtDcLock2.Text = "_uiIntegerUpDown1";
            this.txtDcLock2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel19
            // 
            this.uiMarkLabel19.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel19.Location = new System.Drawing.Point(3, 52);
            this.uiMarkLabel19.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel19.Name = "uiMarkLabel19";
            this.uiMarkLabel19.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel19.Size = new System.Drawing.Size(47, 23);
            this.uiMarkLabel19.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel19.TabIndex = 2;
            this.uiMarkLabel19.Text = "释放：";
            this.uiMarkLabel19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel20
            // 
            this.uiMarkLabel20.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel20.Location = new System.Drawing.Point(3, 26);
            this.uiMarkLabel20.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel20.Name = "uiMarkLabel20";
            this.uiMarkLabel20.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel20.Size = new System.Drawing.Size(47, 23);
            this.uiMarkLabel20.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel20.TabIndex = 2;
            this.uiMarkLabel20.Text = "吸合：";
            this.uiMarkLabel20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbMotorCmd
            // 
            this.cmbMotorCmd.DataSource = null;
            this.cmbMotorCmd.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbMotorCmd.FillColor = System.Drawing.Color.White;
            this.cmbMotorCmd.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbMotorCmd.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.cmbMotorCmd.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(242)))), ((int)(((byte)(212)))));
            this.cmbMotorCmd.ItemRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbMotorCmd.Items.AddRange(new object[] {
            "Idle",
            "释放",
            "吸合"});
            this.cmbMotorCmd.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbMotorCmd.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbMotorCmd.Location = new System.Drawing.Point(160, 36);
            this.cmbMotorCmd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbMotorCmd.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbMotorCmd.Name = "cmbMotorCmd";
            this.cmbMotorCmd.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbMotorCmd.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbMotorCmd.ScrollBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbMotorCmd.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbMotorCmd.Size = new System.Drawing.Size(63, 23);
            this.cmbMotorCmd.Style = Sunny.UI.UIStyle.Custom;
            this.cmbMotorCmd.SymbolSize = 24;
            this.cmbMotorCmd.TabIndex = 4;
            this.cmbMotorCmd.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbMotorCmd.Watermark = "";
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.txtMirrorYPos);
            this.uiGroupBox1.Controls.Add(this.txtMirrorXPos);
            this.uiGroupBox1.Controls.Add(this.uiMarkLabel30);
            this.uiGroupBox1.Controls.Add(this.uiMarkLabel28);
            this.uiGroupBox1.Controls.Add(this.txtMirrTintgCmd);
            this.uiGroupBox1.Controls.Add(this.txtDcMirrorY);
            this.uiGroupBox1.Controls.Add(this.txtDcMirrorX);
            this.uiGroupBox1.Controls.Add(this.swMirrDefrstDrvrCmd);
            this.uiGroupBox1.Controls.Add(this.cmbDrvrAdjCmd);
            this.uiGroupBox1.Controls.Add(this.cmbDrvrMirrCmd5);
            this.uiGroupBox1.Controls.Add(this.txtMirrorTintgVolt);
            this.uiGroupBox1.Controls.Add(this.txtMirrorDefrstVolt);
            this.uiGroupBox1.Controls.Add(this.uiMarkLabel4);
            this.uiGroupBox1.Controls.Add(this.uiMarkLabel3);
            this.uiGroupBox1.Controls.Add(this.uiMarkLabel2);
            this.uiGroupBox1.Controls.Add(this.uiMarkLabel1);
            this.uiGroupBox1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.uiGroupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox1.Location = new System.Drawing.Point(4, 218);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiGroupBox1.Size = new System.Drawing.Size(282, 176);
            this.uiGroupBox1.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = "后视镜";
            this.uiGroupBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMirrorYPos
            // 
            this.txtMirrorYPos.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorYPos.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMirrorYPos.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMirrorYPos.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorYPos.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMirrorYPos.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMirrorYPos.ButtonStyleInherited = false;
            this.txtMirrorYPos.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMirrorYPos.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtMirrorYPos.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtMirrorYPos.Location = new System.Drawing.Point(211, 148);
            this.txtMirrorYPos.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMirrorYPos.Maximum = 99999999D;
            this.txtMirrorYPos.Minimum = -99999999D;
            this.txtMirrorYPos.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtMirrorYPos.Name = "txtMirrorYPos";
            this.txtMirrorYPos.Padding = new System.Windows.Forms.Padding(5);
            this.txtMirrorYPos.ReadOnly = true;
            this.txtMirrorYPos.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorYPos.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorYPos.ScrollBarStyleInherited = false;
            this.txtMirrorYPos.ShowText = false;
            this.txtMirrorYPos.Size = new System.Drawing.Size(63, 23);
            this.txtMirrorYPos.Style = Sunny.UI.UIStyle.Custom;
            this.txtMirrorYPos.TabIndex = 10;
            this.txtMirrorYPos.Text = "0.00";
            this.txtMirrorYPos.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtMirrorYPos.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtMirrorYPos.Watermark = "";
            // 
            // txtMirrorXPos
            // 
            this.txtMirrorXPos.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorXPos.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMirrorXPos.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMirrorXPos.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorXPos.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMirrorXPos.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMirrorXPos.ButtonStyleInherited = false;
            this.txtMirrorXPos.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMirrorXPos.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtMirrorXPos.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtMirrorXPos.Location = new System.Drawing.Point(71, 148);
            this.txtMirrorXPos.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMirrorXPos.Maximum = 99999999D;
            this.txtMirrorXPos.Minimum = -99999999D;
            this.txtMirrorXPos.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtMirrorXPos.Name = "txtMirrorXPos";
            this.txtMirrorXPos.Padding = new System.Windows.Forms.Padding(5);
            this.txtMirrorXPos.ReadOnly = true;
            this.txtMirrorXPos.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorXPos.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorXPos.ScrollBarStyleInherited = false;
            this.txtMirrorXPos.ShowText = false;
            this.txtMirrorXPos.Size = new System.Drawing.Size(65, 23);
            this.txtMirrorXPos.Style = Sunny.UI.UIStyle.Custom;
            this.txtMirrorXPos.TabIndex = 10;
            this.txtMirrorXPos.Text = "0.00";
            this.txtMirrorXPos.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtMirrorXPos.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtMirrorXPos.Watermark = "";
            // 
            // uiMarkLabel30
            // 
            this.uiMarkLabel30.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel30.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel30.Location = new System.Drawing.Point(143, 148);
            this.uiMarkLabel30.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel30.Name = "uiMarkLabel30";
            this.uiMarkLabel30.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel30.Size = new System.Drawing.Size(61, 23);
            this.uiMarkLabel30.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel30.TabIndex = 9;
            this.uiMarkLabel30.Text = "Y轴：";
            this.uiMarkLabel30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel28
            // 
            this.uiMarkLabel28.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel28.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel28.Location = new System.Drawing.Point(3, 148);
            this.uiMarkLabel28.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel28.Name = "uiMarkLabel28";
            this.uiMarkLabel28.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel28.Size = new System.Drawing.Size(61, 23);
            this.uiMarkLabel28.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel28.TabIndex = 9;
            this.uiMarkLabel28.Text = "X轴：";
            this.uiMarkLabel28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMirrTintgCmd
            // 
            this.txtMirrTintgCmd.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrTintgCmd.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMirrTintgCmd.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMirrTintgCmd.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrTintgCmd.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtMirrTintgCmd.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtMirrTintgCmd.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtMirrTintgCmd.Location = new System.Drawing.Point(89, 117);
            this.txtMirrTintgCmd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMirrTintgCmd.Maximum = 100;
            this.txtMirrTintgCmd.Minimum = 0;
            this.txtMirrTintgCmd.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtMirrTintgCmd.Name = "txtMirrTintgCmd";
            this.txtMirrTintgCmd.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrTintgCmd.ShowText = false;
            this.txtMirrTintgCmd.Size = new System.Drawing.Size(100, 23);
            this.txtMirrTintgCmd.Step = 10;
            this.txtMirrTintgCmd.Style = Sunny.UI.UIStyle.Custom;
            this.txtMirrTintgCmd.TabIndex = 8;
            this.txtMirrTintgCmd.Text = "_uiIntegerUpDown1";
            this.txtMirrTintgCmd.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDcMirrorY
            // 
            this.txtDcMirrorY.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcMirrorY.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtDcMirrorY.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtDcMirrorY.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcMirrorY.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcMirrorY.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcMirrorY.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtDcMirrorY.Location = new System.Drawing.Point(89, 60);
            this.txtDcMirrorY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDcMirrorY.Maximum = 100;
            this.txtDcMirrorY.Minimum = 0;
            this.txtDcMirrorY.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtDcMirrorY.Name = "txtDcMirrorY";
            this.txtDcMirrorY.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcMirrorY.ShowText = false;
            this.txtDcMirrorY.Size = new System.Drawing.Size(100, 23);
            this.txtDcMirrorY.Step = 10;
            this.txtDcMirrorY.Style = Sunny.UI.UIStyle.Custom;
            this.txtDcMirrorY.TabIndex = 8;
            this.txtDcMirrorY.Text = "_uiIntegerUpDown1";
            this.txtDcMirrorY.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDcMirrorX
            // 
            this.txtDcMirrorX.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcMirrorX.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtDcMirrorX.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtDcMirrorX.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcMirrorX.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcMirrorX.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtDcMirrorX.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtDcMirrorX.Location = new System.Drawing.Point(89, 32);
            this.txtDcMirrorX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDcMirrorX.Maximum = 100;
            this.txtDcMirrorX.Minimum = 0;
            this.txtDcMirrorX.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtDcMirrorX.Name = "txtDcMirrorX";
            this.txtDcMirrorX.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtDcMirrorX.ShowText = false;
            this.txtDcMirrorX.Size = new System.Drawing.Size(100, 23);
            this.txtDcMirrorX.Step = 10;
            this.txtDcMirrorX.Style = Sunny.UI.UIStyle.Custom;
            this.txtDcMirrorX.TabIndex = 8;
            this.txtDcMirrorX.Text = "_uiIntegerUpDown1";
            this.txtDcMirrorX.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // swMirrDefrstDrvrCmd
            // 
            this.swMirrDefrstDrvrCmd.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.swMirrDefrstDrvrCmd.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.swMirrDefrstDrvrCmd.Location = new System.Drawing.Point(92, 88);
            this.swMirrDefrstDrvrCmd.MinimumSize = new System.Drawing.Size(1, 1);
            this.swMirrDefrstDrvrCmd.Name = "swMirrDefrstDrvrCmd";
            this.swMirrDefrstDrvrCmd.Size = new System.Drawing.Size(75, 23);
            this.swMirrDefrstDrvrCmd.Style = Sunny.UI.UIStyle.Custom;
            this.swMirrDefrstDrvrCmd.TabIndex = 5;
            this.swMirrDefrstDrvrCmd.Text = "uiSwitch1";
            // 
            // cmbDrvrAdjCmd
            // 
            this.cmbDrvrAdjCmd.DataSource = null;
            this.cmbDrvrAdjCmd.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbDrvrAdjCmd.FillColor = System.Drawing.Color.White;
            this.cmbDrvrAdjCmd.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbDrvrAdjCmd.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.cmbDrvrAdjCmd.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(242)))), ((int)(((byte)(212)))));
            this.cmbDrvrAdjCmd.ItemRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbDrvrAdjCmd.Items.AddRange(new object[] {
            "停止",
            "向上",
            "向下"});
            this.cmbDrvrAdjCmd.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbDrvrAdjCmd.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbDrvrAdjCmd.Location = new System.Drawing.Point(197, 60);
            this.cmbDrvrAdjCmd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbDrvrAdjCmd.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbDrvrAdjCmd.Name = "cmbDrvrAdjCmd";
            this.cmbDrvrAdjCmd.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbDrvrAdjCmd.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbDrvrAdjCmd.ScrollBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbDrvrAdjCmd.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbDrvrAdjCmd.Size = new System.Drawing.Size(78, 23);
            this.cmbDrvrAdjCmd.Style = Sunny.UI.UIStyle.Custom;
            this.cmbDrvrAdjCmd.SymbolSize = 24;
            this.cmbDrvrAdjCmd.TabIndex = 4;
            this.cmbDrvrAdjCmd.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbDrvrAdjCmd.Watermark = "";
            // 
            // cmbDrvrMirrCmd5
            // 
            this.cmbDrvrMirrCmd5.DataSource = null;
            this.cmbDrvrMirrCmd5.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbDrvrMirrCmd5.FillColor = System.Drawing.Color.White;
            this.cmbDrvrMirrCmd5.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbDrvrMirrCmd5.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.cmbDrvrMirrCmd5.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(242)))), ((int)(((byte)(212)))));
            this.cmbDrvrMirrCmd5.ItemRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbDrvrMirrCmd5.Items.AddRange(new object[] {
            "Idle",
            "折叠",
            "展开"});
            this.cmbDrvrMirrCmd5.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbDrvrMirrCmd5.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbDrvrMirrCmd5.Location = new System.Drawing.Point(197, 32);
            this.cmbDrvrMirrCmd5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbDrvrMirrCmd5.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbDrvrMirrCmd5.Name = "cmbDrvrMirrCmd5";
            this.cmbDrvrMirrCmd5.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbDrvrMirrCmd5.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbDrvrMirrCmd5.ScrollBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.cmbDrvrMirrCmd5.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.cmbDrvrMirrCmd5.Size = new System.Drawing.Size(78, 23);
            this.cmbDrvrMirrCmd5.Style = Sunny.UI.UIStyle.Custom;
            this.cmbDrvrMirrCmd5.SymbolSize = 24;
            this.cmbDrvrMirrCmd5.TabIndex = 4;
            this.cmbDrvrMirrCmd5.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbDrvrMirrCmd5.Watermark = "";
            // 
            // txtMirrorTintgVolt
            // 
            this.txtMirrorTintgVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorTintgVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMirrorTintgVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMirrorTintgVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorTintgVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMirrorTintgVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMirrorTintgVolt.ButtonStyleInherited = false;
            this.txtMirrorTintgVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMirrorTintgVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtMirrorTintgVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtMirrorTintgVolt.Location = new System.Drawing.Point(197, 116);
            this.txtMirrorTintgVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMirrorTintgVolt.Maximum = 99999999D;
            this.txtMirrorTintgVolt.Minimum = -99999999D;
            this.txtMirrorTintgVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtMirrorTintgVolt.Name = "txtMirrorTintgVolt";
            this.txtMirrorTintgVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtMirrorTintgVolt.ReadOnly = true;
            this.txtMirrorTintgVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorTintgVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorTintgVolt.ScrollBarStyleInherited = false;
            this.txtMirrorTintgVolt.ShowText = false;
            this.txtMirrorTintgVolt.Size = new System.Drawing.Size(78, 23);
            this.txtMirrorTintgVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtMirrorTintgVolt.TabIndex = 3;
            this.txtMirrorTintgVolt.Text = "0.00";
            this.txtMirrorTintgVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtMirrorTintgVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtMirrorTintgVolt.Watermark = "";
            // 
            // txtMirrorDefrstVolt
            // 
            this.txtMirrorDefrstVolt.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorDefrstVolt.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMirrorDefrstVolt.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMirrorDefrstVolt.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorDefrstVolt.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.txtMirrorDefrstVolt.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.txtMirrorDefrstVolt.ButtonStyleInherited = false;
            this.txtMirrorDefrstVolt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMirrorDefrstVolt.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.txtMirrorDefrstVolt.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtMirrorDefrstVolt.Location = new System.Drawing.Point(175, 88);
            this.txtMirrorDefrstVolt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMirrorDefrstVolt.Maximum = 99999999D;
            this.txtMirrorDefrstVolt.Minimum = -99999999D;
            this.txtMirrorDefrstVolt.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtMirrorDefrstVolt.Name = "txtMirrorDefrstVolt";
            this.txtMirrorDefrstVolt.Padding = new System.Windows.Forms.Padding(5);
            this.txtMirrorDefrstVolt.ReadOnly = true;
            this.txtMirrorDefrstVolt.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorDefrstVolt.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.txtMirrorDefrstVolt.ScrollBarStyleInherited = false;
            this.txtMirrorDefrstVolt.ShowText = false;
            this.txtMirrorDefrstVolt.Size = new System.Drawing.Size(100, 23);
            this.txtMirrorDefrstVolt.Style = Sunny.UI.UIStyle.Custom;
            this.txtMirrorDefrstVolt.TabIndex = 3;
            this.txtMirrorDefrstVolt.Text = "0.00";
            this.txtMirrorDefrstVolt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtMirrorDefrstVolt.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.txtMirrorDefrstVolt.Watermark = "";
            // 
            // uiMarkLabel4
            // 
            this.uiMarkLabel4.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel4.Location = new System.Drawing.Point(3, 116);
            this.uiMarkLabel4.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel4.Name = "uiMarkLabel4";
            this.uiMarkLabel4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel4.Size = new System.Drawing.Size(82, 23);
            this.uiMarkLabel4.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel4.TabIndex = 2;
            this.uiMarkLabel4.Text = "防眩目调光：";
            this.uiMarkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel3
            // 
            this.uiMarkLabel3.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel3.Location = new System.Drawing.Point(3, 88);
            this.uiMarkLabel3.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel3.Name = "uiMarkLabel3";
            this.uiMarkLabel3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel3.Size = new System.Drawing.Size(82, 23);
            this.uiMarkLabel3.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel3.TabIndex = 2;
            this.uiMarkLabel3.Text = "加热：";
            this.uiMarkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel2.Location = new System.Drawing.Point(3, 60);
            this.uiMarkLabel2.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel2.Size = new System.Drawing.Size(82, 23);
            this.uiMarkLabel2.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel2.TabIndex = 2;
            this.uiMarkLabel2.Text = "上/下：";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel1.Location = new System.Drawing.Point(3, 32);
            this.uiMarkLabel1.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(82, 23);
            this.uiMarkLabel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel1.TabIndex = 2;
            this.uiMarkLabel1.Text = "展开/折叠：";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmEnvBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.uiTabControl1);
            this.Name = "FrmEnvBox";
            this.Text = "CX1E";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 480);
            this.uiTabControl1.ResumeLayout(false);
            this.MainPage.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.uiGroupBox2.ResumeLayout(false);
            this.uiGroupBox9.ResumeLayout(false);
            this.groupRelayBtns.ResumeLayout(false);
            this.uiGroupBox12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrent)).EndInit();
            this.uiGroupBox11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvState)).EndInit();
            this.uiGroupBox3.ResumeLayout(false);
            this.uiGroupBox10.ResumeLayout(false);
            this.uiGroupBox8.ResumeLayout(false);
            this.uiGroupBox7.ResumeLayout(false);
            this.uiGroupBox6.ResumeLayout(false);
            this.uiGroupBox5.ResumeLayout(false);
            this.uiGroupBox4.ResumeLayout(false);
            this.uiGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private UITabControl uiTabControl1;
        private TabPage MainPage;
        private UIPanel mainPanel;
        private UIMarkLabel lblSysState;
        private UIGroupBox uiGroupBox2;
        private UISwitch swActvnOfPudLi;
        private UISwitch swActvnOfIndcrOut;
        private UIComboBox cmbWindowLight;
        private UIComboBox cmbFronHandleLight;
        private UIComboBox cmbHndlDoorLiDrvr;
        private UISwitch swStepLight;
        private UISwitch swPocketLight;
        private UISwitch swBsd;
        private UITextBox txtMemoryVolt;
        private UITextBox txtLockLightVolt;
        private UITextBox txtWindowLightVolt;
        private UITextBox txtFronHandleLightVolt;
        private UITextBox txtHndlDoorLiDrvrVolt;
        private UITextBox txtActvnOfPudLiVolt;
        private UITextBox txtActvnOfIndcrOutVolt;
        private UITextBox txtStepLightVolt;
        private UITextBox txtPocketLightVolt;
        private UITextBox txtBsdVolt;
        private UIMarkLabel uiMarkLabel29;
        private UIMarkLabel uiMarkLabel5;
        private UIMarkLabel uiMarkLabel25;
        private UIMarkLabel uiMarkLabel12;
        private UIMarkLabel uiMarkLabel11;
        private UIMarkLabel uiMarkLabel10;
        private UIMarkLabel uiMarkLabel9;
        private UIMarkLabel uiMarkLabel7;
        private UIMarkLabel uiMarkLabel6;
        private UIMarkLabel uiMarkLabel8;
        private UIGroupBox uiGroupBox9;
        private UIRadioButton rbChdLockReLeCtrlHmiReqUnlock;
        private UIRadioButton rbChdLockReLeCtrlHmiReqIdle;
        private UIIntegerUpDown txtDcChildrenLock;
        private UIRadioButton rbChdLockReLeCtrlHmiReqLock;
        private UIMarkLabel uiMarkLabel27;
        private UIGroupBox groupRelayBtns;
        private UIButton uiButton21;
        private UIButton uiButton11;
        private UIButton uiButton20;
        private UIButton uiButton9;
        private UIButton uiButton19;
        private UIButton uiButton7;
        private UIButton uiButton18;
        private UIButton uiButton5;
        private UIButton uiButton17;
        private UIButton uiButton3;
        private UIButton uiButton1;
        private UIButton uiButton24;
        private UIButton uiButton23;
        private UIButton uiButton22;
        private UIButton uiButton16;
        private UIButton uiButton15;
        private UIButton uiButton10;
        private UIButton uiButton14;
        private UIButton uiButton8;
        private UIButton uiButton13;
        private UIButton uiButton6;
        private UIButton uiButton12;
        private UIButton uiButton4;
        private UIButton uiButton2;
        private UIButton btnAddCustomController;
        private UIGroupBox uiGroupBox12;
        private UIDataGridView dgvCurrent;
        private UIGroupBox uiGroupBox11;
        private UIDataGridView dgvState;
        private UIGroupBox uiGroupBox3;
        private UILedBulb uiLedBulb2;
        private UISwitch swStartCan;
        private UIMarkLabel uiMarkLabel18;
        private UIMarkLabel uiMarkLabel14;
        private UIMarkLabel uiMarkLabel22;
        private UITextBox txtVbat2;
        private UIMarkLabel uiMarkLabel21;
        private UITextBox txtMotorDoorSpeed;
        private UITextBox txtMotorPedalSpeed;
        private UITextBox txtTemperature;
        private UITextBox txtVbat1;
        private UIMarkLabel uiMarkLabel24;
        private UIMarkLabel uiMarkLabel23;
        private UIMarkLabel uiMarkLabel13;
        private UIGroupBox uiGroupBox10;
        private UISwitch swPowSupplyConDDSswitch;
        private UISwitch swPowSupplyConDoorHall;
        private UISwitch swPowSupplyConHandleHall;
        private UISwitch swPowSupplyCmdPos;
        private UISwitch swPowSupplyConPendalHall;
        private UIMarkLabel uiMarkLabel37;
        private UIMarkLabel uiMarkLabel36;
        private UIMarkLabel uiMarkLabel35;
        private UIMarkLabel uiMarkLabel33;
        private UIMarkLabel uiMarkLabel34;
        private UITextBox txtPowSupplyConDDSswitchVolt;
        private UITextBox txtPowSupplyConDoorHallVolt;
        private UITextBox txtPowSupplyConHandleHallVolt;
        private UITextBox txtPowSupplyCmdPosVolt;
        private UITextBox txtowSupplyConPendalHallVolt;
        private UIGroupBox uiGroupBox8;
        private UIRadioButton rbPedalClose;
        private UIRadioButton rbPedalIdle;
        private UIMarkLabel uiMarkLabel26;
        private UIRadioButton rbPedalOpen;
        private UIIntegerUpDown txtDcPedal;
        private UIGroupBox uiGroupBox7;
        private UIRadioButton rbDoorDrvrHndlCmdClose;
        private UIRadioButton rbDoorDrvrHndlCmdIdle;
        private UIMarkLabel uiMarkLabel17;
        private UIIntegerUpDown txtDcHandle;
        private UIRadioButton rbDoorDrvrHndlCmdOpen;
        private UIGroupBox uiGroupBox6;
        private UIRadioButton rbShortDropWinOpen;
        private UIMarkLabel uiMarkLabel15;
        private UIRadioButton rbShortDropWinClose;
        private UIIntegerUpDown txtDcWindow;
        private UIRadioButton rbShortDropWinIdle;
        private UIGroupBox uiGroupBox5;
        private UIRadioButton rbDoorClose;
        private UIRadioButton rbDoorOpen;
        private UIRadioButton rbDoorIdle;
        private UIIntegerUpDown txtDcDoor;
        private UIMarkLabel uiMarkLabel16;
        private UIGroupBox uiGroupBox4;
        private UIIntegerUpDown txtDcElectricRelese;
        private UIIntegerUpDown txtDcLock2;
        private UIMarkLabel uiMarkLabel19;
        private UIMarkLabel uiMarkLabel20;
        private UIComboBox cmbMotorCmd;
        private UIGroupBox uiGroupBox1;
        private UITextBox txtMirrorYPos;
        private UITextBox txtMirrorXPos;
        private UIMarkLabel uiMarkLabel30;
        private UIMarkLabel uiMarkLabel28;
        private UIIntegerUpDown txtMirrTintgCmd;
        private UIIntegerUpDown txtDcMirrorY;
        private UIIntegerUpDown txtDcMirrorX;
        private UISwitch swMirrDefrstDrvrCmd;
        private UIComboBox cmbDrvrAdjCmd;
        private UIComboBox cmbDrvrMirrCmd5;
        private UITextBox txtMirrorTintgVolt;
        private UITextBox txtMirrorDefrstVolt;
        private UIMarkLabel uiMarkLabel4;
        private UIMarkLabel uiMarkLabel3;
        private UIMarkLabel uiMarkLabel2;
        private UIMarkLabel uiMarkLabel1;
    }
}