using System.Windows.Forms;
using Sunny.UI;

namespace CheckSystem.SyRenesasMcuController
{
    sealed partial class FrmSingleMcuPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSingleMcuPage));
            this.mainTablePanel = new Sunny.UI.UITableLayoutPanel();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.btnConnect = new Sunny.UI.UISymbolButton();
            this.txtIp = new Sunny.UI.UIIPTextBox();
            this.uiTabControl1 = new Sunny.UI.UITabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.controlPanel = new Sunny.UI.UITableLayoutPanel();
            this.diTablePanel = new Sunny.UI.UITableLayoutPanel();
            this.di1 = new Sunny.UI.UILight();
            this.di2 = new Sunny.UI.UILight();
            this.di3 = new Sunny.UI.UILight();
            this.di4 = new Sunny.UI.UILight();
            this.di5 = new Sunny.UI.UILight();
            this.di6 = new Sunny.UI.UILight();
            this.btnMasterReadDIs = new Sunny.UI.UISymbolButton();
            this.doTablePanel = new Sunny.UI.UITableLayoutPanel();
            this.btnMasterSetDOs = new Sunny.UI.UISymbolButton();
            this.do4 = new Sunny.UI.UISwitch();
            this.do3 = new Sunny.UI.UISwitch();
            this.do2 = new Sunny.UI.UISwitch();
            this.do1 = new Sunny.UI.UISwitch();
            this.rlPanel = new Sunny.UI.UIPanel();
            this.uiFlowLayoutPanel1 = new Sunny.UI.UIFlowLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.adPanel = new Sunny.UI.UIPanel();
            this.uiFlowLayoutPanel2 = new Sunny.UI.UIFlowLayoutPanel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.flashPanel = new Sunny.UI.UITableLayoutPanel();
            this.uiGroupBox25 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox24 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox23 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox22 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox21 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox20 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox19 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox18 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox17 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox16 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox15 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox14 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox13 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox12 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox11 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox10 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox9 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox8 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox7 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox6 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox5 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox4 = new Sunny.UI.UIGroupBox();
            this.gpWriteSlaveID = new Sunny.UI.UIGroupBox();
            this.cmbSlaveType = new Sunny.UI.UIComboBox();
            this.cmbSlaveIDs = new Sunny.UI.UIComboBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.btnWriteSlaveID = new Sunny.UI.UISymbolButton();
            this.uiGroupBox2 = new Sunny.UI.UIGroupBox();
            this.cmbSlaveCanFuncType = new Sunny.UI.UIComboBox();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.btnWriteUartCanConfig = new Sunny.UI.UISymbolButton();
            this.btnWriteSlaveCanFunc = new Sunny.UI.UISymbolButton();
            this.uiGroupBox3 = new Sunny.UI.UIGroupBox();
            this.txtToUpdateLinBauteRate = new Sunny.UI.UITextBox();
            this.cmbToUpdateLinCh = new Sunny.UI.UIComboBox();
            this.uiLabel5 = new Sunny.UI.UILabel();
            this.btnUpdateLinBauteRate = new Sunny.UI.UISymbolButton();
            this.uiLabel4 = new Sunny.UI.UILabel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.mainTablePanel.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            this.uiTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.diTablePanel.SuspendLayout();
            this.doTablePanel.SuspendLayout();
            this.rlPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.adPanel.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.flashPanel.SuspendLayout();
            this.gpWriteSlaveID.SuspendLayout();
            this.uiGroupBox2.SuspendLayout();
            this.uiGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTablePanel
            // 
            this.mainTablePanel.ColumnCount = 1;
            this.mainTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTablePanel.Controls.Add(this.uiPanel1, 0, 0);
            this.mainTablePanel.Controls.Add(this.uiTabControl1, 0, 1);
            this.mainTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTablePanel.Location = new System.Drawing.Point(0, 35);
            this.mainTablePanel.Name = "mainTablePanel";
            this.mainTablePanel.RowCount = 2;
            this.mainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTablePanel.Size = new System.Drawing.Size(1170, 733);
            this.mainTablePanel.TabIndex = 0;
            this.mainTablePanel.TagString = null;
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.btnConnect);
            this.uiPanel1.Controls.Add(this.txtIp);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(4, 5);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(1162, 30);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnConnect
            // 
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnConnect.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Location = new System.Drawing.Point(509, 0);
            this.btnConnect.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(176, 30);
            this.btnConnect.Symbol = 558120;
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "连接";
            this.btnConnect.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtIp
            // 
            this.txtIp.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtIp.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.txtIp.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtIp.Location = new System.Drawing.Point(0, 0);
            this.txtIp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtIp.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtIp.Name = "txtIp";
            this.txtIp.Padding = new System.Windows.Forms.Padding(1);
            this.txtIp.ShowText = false;
            this.txtIp.Size = new System.Drawing.Size(509, 30);
            this.txtIp.TabIndex = 0;
            this.txtIp.Text = "192.168.1.28";
            this.txtIp.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtIp.Value = ((System.Net.IPAddress)(resources.GetObject("txtIp.Value")));
            // 
            // uiTabControl1
            // 
            this.uiTabControl1.Controls.Add(this.tabPage1);
            this.uiTabControl1.Controls.Add(this.tabPage2);
            this.uiTabControl1.Controls.Add(this.tabPage3);
            this.uiTabControl1.Controls.Add(this.tabPage4);
            this.uiTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiTabControl1.Enabled = false;
            this.uiTabControl1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTabControl1.ItemSize = new System.Drawing.Size(150, 40);
            this.uiTabControl1.Location = new System.Drawing.Point(3, 43);
            this.uiTabControl1.MainPage = "";
            this.uiTabControl1.Name = "uiTabControl1";
            this.uiTabControl1.SelectedIndex = 0;
            this.uiTabControl1.Size = new System.Drawing.Size(1164, 687);
            this.uiTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiTabControl1.TabIndex = 1;
            this.uiTabControl1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.controlPanel);
            this.tabPage1.Location = new System.Drawing.Point(0, 40);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1164, 647);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "主从站控制";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // controlPanel
            // 
            this.controlPanel.ColumnCount = 2;
            this.controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.controlPanel.Controls.Add(this.diTablePanel, 0, 0);
            this.controlPanel.Controls.Add(this.doTablePanel, 1, 0);
            this.controlPanel.Controls.Add(this.rlPanel, 0, 1);
            this.controlPanel.Controls.Add(this.adPanel, 1, 1);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.RowCount = 2;
            this.controlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.controlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.controlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.controlPanel.Size = new System.Drawing.Size(1164, 647);
            this.controlPanel.TabIndex = 0;
            this.controlPanel.TagString = null;
            // 
            // diTablePanel
            // 
            this.diTablePanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.diTablePanel.ColumnCount = 6;
            this.diTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.diTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.diTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.diTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.diTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.diTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.diTablePanel.Controls.Add(this.di1, 0, 0);
            this.diTablePanel.Controls.Add(this.di2, 1, 0);
            this.diTablePanel.Controls.Add(this.di3, 2, 0);
            this.diTablePanel.Controls.Add(this.di4, 3, 0);
            this.diTablePanel.Controls.Add(this.di5, 4, 0);
            this.diTablePanel.Controls.Add(this.di6, 5, 0);
            this.diTablePanel.Controls.Add(this.btnMasterReadDIs, 0, 1);
            this.diTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diTablePanel.Location = new System.Drawing.Point(3, 3);
            this.diTablePanel.Name = "diTablePanel";
            this.diTablePanel.RowCount = 2;
            this.diTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.diTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.diTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.diTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.diTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.diTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.diTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.diTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.diTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.diTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.diTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.diTablePanel.Size = new System.Drawing.Size(576, 91);
            this.diTablePanel.TabIndex = 0;
            this.diTablePanel.TagString = null;
            // 
            // di1
            // 
            this.di1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.di1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.di1.Location = new System.Drawing.Point(6, 6);
            this.di1.MinimumSize = new System.Drawing.Size(1, 1);
            this.di1.Name = "di1";
            this.di1.Radius = 35;
            this.di1.ShowText = true;
            this.di1.Size = new System.Drawing.Size(86, 35);
            this.di1.State = Sunny.UI.UILightState.Blink;
            this.di1.TabIndex = 0;
            this.di1.Text = "DI1";
            this.di1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // di2
            // 
            this.di2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.di2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.di2.Location = new System.Drawing.Point(101, 6);
            this.di2.MinimumSize = new System.Drawing.Size(1, 1);
            this.di2.Name = "di2";
            this.di2.Radius = 35;
            this.di2.ShowText = true;
            this.di2.Size = new System.Drawing.Size(86, 35);
            this.di2.State = Sunny.UI.UILightState.Blink;
            this.di2.TabIndex = 1;
            this.di2.Text = "DI2";
            this.di2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // di3
            // 
            this.di3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.di3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.di3.Location = new System.Drawing.Point(196, 6);
            this.di3.MinimumSize = new System.Drawing.Size(1, 1);
            this.di3.Name = "di3";
            this.di3.Radius = 35;
            this.di3.ShowText = true;
            this.di3.Size = new System.Drawing.Size(86, 35);
            this.di3.State = Sunny.UI.UILightState.Blink;
            this.di3.TabIndex = 2;
            this.di3.Text = "DI3";
            this.di3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // di4
            // 
            this.di4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.di4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.di4.Location = new System.Drawing.Point(291, 6);
            this.di4.MinimumSize = new System.Drawing.Size(1, 1);
            this.di4.Name = "di4";
            this.di4.Radius = 35;
            this.di4.ShowText = true;
            this.di4.Size = new System.Drawing.Size(86, 35);
            this.di4.State = Sunny.UI.UILightState.Blink;
            this.di4.TabIndex = 3;
            this.di4.Text = "DI4";
            this.di4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // di5
            // 
            this.di5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.di5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.di5.Location = new System.Drawing.Point(386, 6);
            this.di5.MinimumSize = new System.Drawing.Size(1, 1);
            this.di5.Name = "di5";
            this.di5.Radius = 35;
            this.di5.ShowText = true;
            this.di5.Size = new System.Drawing.Size(86, 35);
            this.di5.State = Sunny.UI.UILightState.Blink;
            this.di5.TabIndex = 4;
            this.di5.Text = "DI5";
            this.di5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // di6
            // 
            this.di6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.di6.Enabled = false;
            this.di6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.di6.Location = new System.Drawing.Point(481, 6);
            this.di6.MinimumSize = new System.Drawing.Size(1, 1);
            this.di6.Name = "di6";
            this.di6.Radius = 35;
            this.di6.ShowText = true;
            this.di6.Size = new System.Drawing.Size(89, 35);
            this.di6.State = Sunny.UI.UILightState.Blink;
            this.di6.TabIndex = 5;
            this.di6.Text = "DI6";
            this.di6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnMasterReadDIs
            // 
            this.diTablePanel.SetColumnSpan(this.btnMasterReadDIs, 6);
            this.btnMasterReadDIs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMasterReadDIs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMasterReadDIs.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMasterReadDIs.Location = new System.Drawing.Point(4, 48);
            this.btnMasterReadDIs.Margin = new System.Windows.Forms.Padding(1);
            this.btnMasterReadDIs.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnMasterReadDIs.Name = "btnMasterReadDIs";
            this.btnMasterReadDIs.Size = new System.Drawing.Size(568, 39);
            this.btnMasterReadDIs.Symbol = 559583;
            this.btnMasterReadDIs.TabIndex = 6;
            this.btnMasterReadDIs.Text = "刷新主站DIs";
            this.btnMasterReadDIs.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMasterReadDIs.Click += new System.EventHandler(this.btnMasterReadDIs_Click);
            // 
            // doTablePanel
            // 
            this.doTablePanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.doTablePanel.ColumnCount = 4;
            this.doTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.doTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.doTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.doTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.doTablePanel.Controls.Add(this.btnMasterSetDOs, 0, 1);
            this.doTablePanel.Controls.Add(this.do4, 3, 0);
            this.doTablePanel.Controls.Add(this.do3, 2, 0);
            this.doTablePanel.Controls.Add(this.do2, 1, 0);
            this.doTablePanel.Controls.Add(this.do1, 0, 0);
            this.doTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doTablePanel.Location = new System.Drawing.Point(585, 3);
            this.doTablePanel.Name = "doTablePanel";
            this.doTablePanel.RowCount = 2;
            this.doTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.doTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.doTablePanel.Size = new System.Drawing.Size(576, 91);
            this.doTablePanel.TabIndex = 1;
            this.doTablePanel.TagString = null;
            // 
            // btnMasterSetDOs
            // 
            this.doTablePanel.SetColumnSpan(this.btnMasterSetDOs, 6);
            this.btnMasterSetDOs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMasterSetDOs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMasterSetDOs.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMasterSetDOs.Location = new System.Drawing.Point(2, 47);
            this.btnMasterSetDOs.Margin = new System.Windows.Forms.Padding(1);
            this.btnMasterSetDOs.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnMasterSetDOs.Name = "btnMasterSetDOs";
            this.btnMasterSetDOs.Size = new System.Drawing.Size(572, 42);
            this.btnMasterSetDOs.Symbol = 560350;
            this.btnMasterSetDOs.TabIndex = 7;
            this.btnMasterSetDOs.Text = "设置主站DOs";
            this.btnMasterSetDOs.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMasterSetDOs.Click += new System.EventHandler(this.btnMasterSetDOs_Click);
            // 
            // do4
            // 
            this.do4.ActiveText = "D04开";
            this.do4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.do4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.do4.InActiveText = "D04关";
            this.do4.Location = new System.Drawing.Point(433, 4);
            this.do4.MinimumSize = new System.Drawing.Size(1, 1);
            this.do4.Name = "do4";
            this.do4.Size = new System.Drawing.Size(139, 38);
            this.do4.TabIndex = 3;
            this.do4.Text = "uiSwitch4";
            // 
            // do3
            // 
            this.do3.ActiveText = "D03开";
            this.do3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.do3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.do3.InActiveText = "D03关";
            this.do3.Location = new System.Drawing.Point(290, 4);
            this.do3.MinimumSize = new System.Drawing.Size(1, 1);
            this.do3.Name = "do3";
            this.do3.Size = new System.Drawing.Size(136, 38);
            this.do3.TabIndex = 2;
            this.do3.Text = "uiSwitch3";
            // 
            // do2
            // 
            this.do2.ActiveText = "D02开";
            this.do2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.do2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.do2.InActiveText = "D02关";
            this.do2.Location = new System.Drawing.Point(147, 4);
            this.do2.MinimumSize = new System.Drawing.Size(1, 1);
            this.do2.Name = "do2";
            this.do2.Size = new System.Drawing.Size(136, 38);
            this.do2.TabIndex = 1;
            this.do2.Text = "uiSwitch2";
            // 
            // do1
            // 
            this.do1.ActiveText = "D01开";
            this.do1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.do1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.do1.InActiveText = "D01关";
            this.do1.Location = new System.Drawing.Point(4, 4);
            this.do1.MinimumSize = new System.Drawing.Size(1, 1);
            this.do1.Name = "do1";
            this.do1.Size = new System.Drawing.Size(136, 38);
            this.do1.TabIndex = 0;
            this.do1.Text = "uiSwitch1";
            // 
            // rlPanel
            // 
            this.rlPanel.Controls.Add(this.uiFlowLayoutPanel1);
            this.rlPanel.Controls.Add(this.toolStrip1);
            this.rlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rlPanel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rlPanel.Location = new System.Drawing.Point(4, 102);
            this.rlPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rlPanel.MinimumSize = new System.Drawing.Size(1, 1);
            this.rlPanel.Name = "rlPanel";
            this.rlPanel.Size = new System.Drawing.Size(574, 540);
            this.rlPanel.TabIndex = 2;
            this.rlPanel.Text = null;
            this.rlPanel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiFlowLayoutPanel1
            // 
            this.uiFlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiFlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.uiFlowLayoutPanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiFlowLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.uiFlowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiFlowLayoutPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiFlowLayoutPanel1.Name = "uiFlowLayoutPanel1";
            this.uiFlowLayoutPanel1.Padding = new System.Windows.Forms.Padding(2);
            this.uiFlowLayoutPanel1.ShowText = false;
            this.uiFlowLayoutPanel1.Size = new System.Drawing.Size(574, 515);
            this.uiFlowLayoutPanel1.TabIndex = 1;
            this.uiFlowLayoutPanel1.Text = "uiFlowLayoutPanel1";
            this.uiFlowLayoutPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiFlowLayoutPanel1.WrapContents = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(574, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(74, 22);
            this.toolStripButton1.Text = "添加RL从站";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(141, 22);
            this.toolStripButton2.Text = "设置以下所有RL从站DO";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // adPanel
            // 
            this.adPanel.Controls.Add(this.uiFlowLayoutPanel2);
            this.adPanel.Controls.Add(this.toolStrip2);
            this.adPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.adPanel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.adPanel.Location = new System.Drawing.Point(586, 102);
            this.adPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.adPanel.MinimumSize = new System.Drawing.Size(1, 1);
            this.adPanel.Name = "adPanel";
            this.adPanel.Size = new System.Drawing.Size(574, 540);
            this.adPanel.TabIndex = 3;
            this.adPanel.Text = null;
            this.adPanel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiFlowLayoutPanel2
            // 
            this.uiFlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiFlowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.uiFlowLayoutPanel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiFlowLayoutPanel2.Location = new System.Drawing.Point(0, 25);
            this.uiFlowLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiFlowLayoutPanel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiFlowLayoutPanel2.Name = "uiFlowLayoutPanel2";
            this.uiFlowLayoutPanel2.Padding = new System.Windows.Forms.Padding(2);
            this.uiFlowLayoutPanel2.ShowText = false;
            this.uiFlowLayoutPanel2.Size = new System.Drawing.Size(574, 515);
            this.uiFlowLayoutPanel2.TabIndex = 2;
            this.uiFlowLayoutPanel2.Text = "uiFlowLayoutPanel2";
            this.uiFlowLayoutPanel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiFlowLayoutPanel2.WrapContents = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton3,
            this.toolStripSeparator2,
            this.toolStripButton4});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(574, 25);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(77, 22);
            this.toolStripButton3.Text = "添加AD从站";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(125, 22);
            this.toolStripButton4.Text = "读取以下所有从站AD";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.flashPanel);
            this.tabPage2.Location = new System.Drawing.Point(0, 40);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1164, 647);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Flash配置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // flashPanel
            // 
            this.flashPanel.ColumnCount = 5;
            this.flashPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.flashPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.flashPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.flashPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.flashPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.flashPanel.Controls.Add(this.uiGroupBox25, 4, 4);
            this.flashPanel.Controls.Add(this.uiGroupBox24, 3, 4);
            this.flashPanel.Controls.Add(this.uiGroupBox23, 2, 4);
            this.flashPanel.Controls.Add(this.uiGroupBox22, 1, 4);
            this.flashPanel.Controls.Add(this.uiGroupBox21, 0, 4);
            this.flashPanel.Controls.Add(this.uiGroupBox20, 4, 3);
            this.flashPanel.Controls.Add(this.uiGroupBox19, 3, 3);
            this.flashPanel.Controls.Add(this.uiGroupBox18, 2, 3);
            this.flashPanel.Controls.Add(this.uiGroupBox17, 1, 3);
            this.flashPanel.Controls.Add(this.uiGroupBox16, 0, 3);
            this.flashPanel.Controls.Add(this.uiGroupBox15, 4, 2);
            this.flashPanel.Controls.Add(this.uiGroupBox14, 3, 2);
            this.flashPanel.Controls.Add(this.uiGroupBox13, 2, 2);
            this.flashPanel.Controls.Add(this.uiGroupBox12, 1, 2);
            this.flashPanel.Controls.Add(this.uiGroupBox11, 0, 2);
            this.flashPanel.Controls.Add(this.uiGroupBox10, 4, 1);
            this.flashPanel.Controls.Add(this.uiGroupBox9, 3, 1);
            this.flashPanel.Controls.Add(this.uiGroupBox8, 2, 1);
            this.flashPanel.Controls.Add(this.uiGroupBox7, 1, 1);
            this.flashPanel.Controls.Add(this.uiGroupBox6, 0, 1);
            this.flashPanel.Controls.Add(this.uiGroupBox5, 4, 0);
            this.flashPanel.Controls.Add(this.uiGroupBox4, 3, 0);
            this.flashPanel.Controls.Add(this.gpWriteSlaveID, 0, 0);
            this.flashPanel.Controls.Add(this.uiGroupBox2, 1, 0);
            this.flashPanel.Controls.Add(this.uiGroupBox3, 2, 0);
            this.flashPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flashPanel.Location = new System.Drawing.Point(0, 0);
            this.flashPanel.Name = "flashPanel";
            this.flashPanel.RowCount = 5;
            this.flashPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.flashPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.flashPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.flashPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.flashPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.flashPanel.Size = new System.Drawing.Size(1164, 647);
            this.flashPanel.TabIndex = 0;
            this.flashPanel.TagString = null;
            // 
            // uiGroupBox25
            // 
            this.uiGroupBox25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox25.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox25.Location = new System.Drawing.Point(929, 517);
            this.uiGroupBox25.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox25.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox25.Name = "uiGroupBox25";
            this.uiGroupBox25.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox25.Size = new System.Drawing.Size(234, 129);
            this.uiGroupBox25.TabIndex = 23;
            this.uiGroupBox25.Text = "uiGroupBox25";
            this.uiGroupBox25.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox24
            // 
            this.uiGroupBox24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox24.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox24.Location = new System.Drawing.Point(697, 517);
            this.uiGroupBox24.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox24.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox24.Name = "uiGroupBox24";
            this.uiGroupBox24.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox24.Size = new System.Drawing.Size(230, 129);
            this.uiGroupBox24.TabIndex = 22;
            this.uiGroupBox24.Text = "uiGroupBox24";
            this.uiGroupBox24.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox23
            // 
            this.uiGroupBox23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox23.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox23.Location = new System.Drawing.Point(465, 517);
            this.uiGroupBox23.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox23.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox23.Name = "uiGroupBox23";
            this.uiGroupBox23.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox23.Size = new System.Drawing.Size(230, 129);
            this.uiGroupBox23.TabIndex = 21;
            this.uiGroupBox23.Text = "uiGroupBox23";
            this.uiGroupBox23.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox22
            // 
            this.uiGroupBox22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox22.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox22.Location = new System.Drawing.Point(233, 517);
            this.uiGroupBox22.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox22.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox22.Name = "uiGroupBox22";
            this.uiGroupBox22.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox22.Size = new System.Drawing.Size(230, 129);
            this.uiGroupBox22.TabIndex = 20;
            this.uiGroupBox22.Text = "uiGroupBox22";
            this.uiGroupBox22.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox21
            // 
            this.uiGroupBox21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox21.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox21.Location = new System.Drawing.Point(1, 517);
            this.uiGroupBox21.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox21.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox21.Name = "uiGroupBox21";
            this.uiGroupBox21.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox21.Size = new System.Drawing.Size(230, 129);
            this.uiGroupBox21.TabIndex = 19;
            this.uiGroupBox21.Text = "uiGroupBox21";
            this.uiGroupBox21.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox20
            // 
            this.uiGroupBox20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox20.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox20.Location = new System.Drawing.Point(929, 388);
            this.uiGroupBox20.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox20.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox20.Name = "uiGroupBox20";
            this.uiGroupBox20.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox20.Size = new System.Drawing.Size(234, 127);
            this.uiGroupBox20.TabIndex = 18;
            this.uiGroupBox20.Text = "uiGroupBox20";
            this.uiGroupBox20.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox19
            // 
            this.uiGroupBox19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox19.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox19.Location = new System.Drawing.Point(697, 388);
            this.uiGroupBox19.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox19.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox19.Name = "uiGroupBox19";
            this.uiGroupBox19.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox19.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox19.TabIndex = 17;
            this.uiGroupBox19.Text = "uiGroupBox19";
            this.uiGroupBox19.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox18
            // 
            this.uiGroupBox18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox18.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox18.Location = new System.Drawing.Point(465, 388);
            this.uiGroupBox18.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox18.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox18.Name = "uiGroupBox18";
            this.uiGroupBox18.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox18.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox18.TabIndex = 16;
            this.uiGroupBox18.Text = "uiGroupBox18";
            this.uiGroupBox18.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox17
            // 
            this.uiGroupBox17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox17.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox17.Location = new System.Drawing.Point(233, 388);
            this.uiGroupBox17.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox17.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox17.Name = "uiGroupBox17";
            this.uiGroupBox17.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox17.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox17.TabIndex = 15;
            this.uiGroupBox17.Text = "uiGroupBox17";
            this.uiGroupBox17.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox16
            // 
            this.uiGroupBox16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox16.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox16.Location = new System.Drawing.Point(1, 388);
            this.uiGroupBox16.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox16.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox16.Name = "uiGroupBox16";
            this.uiGroupBox16.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox16.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox16.TabIndex = 14;
            this.uiGroupBox16.Text = "uiGroupBox16";
            this.uiGroupBox16.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox15
            // 
            this.uiGroupBox15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox15.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox15.Location = new System.Drawing.Point(929, 259);
            this.uiGroupBox15.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox15.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox15.Name = "uiGroupBox15";
            this.uiGroupBox15.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox15.Size = new System.Drawing.Size(234, 127);
            this.uiGroupBox15.TabIndex = 13;
            this.uiGroupBox15.Text = "uiGroupBox15";
            this.uiGroupBox15.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox14
            // 
            this.uiGroupBox14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox14.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox14.Location = new System.Drawing.Point(697, 259);
            this.uiGroupBox14.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox14.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox14.Name = "uiGroupBox14";
            this.uiGroupBox14.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox14.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox14.TabIndex = 12;
            this.uiGroupBox14.Text = "uiGroupBox14";
            this.uiGroupBox14.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox13
            // 
            this.uiGroupBox13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox13.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox13.Location = new System.Drawing.Point(465, 259);
            this.uiGroupBox13.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox13.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox13.Name = "uiGroupBox13";
            this.uiGroupBox13.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox13.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox13.TabIndex = 11;
            this.uiGroupBox13.Text = "uiGroupBox13";
            this.uiGroupBox13.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox12
            // 
            this.uiGroupBox12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox12.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox12.Location = new System.Drawing.Point(233, 259);
            this.uiGroupBox12.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox12.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox12.Name = "uiGroupBox12";
            this.uiGroupBox12.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox12.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox12.TabIndex = 10;
            this.uiGroupBox12.Text = "uiGroupBox12";
            this.uiGroupBox12.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox11
            // 
            this.uiGroupBox11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox11.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox11.Location = new System.Drawing.Point(1, 259);
            this.uiGroupBox11.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox11.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox11.Name = "uiGroupBox11";
            this.uiGroupBox11.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox11.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox11.TabIndex = 9;
            this.uiGroupBox11.Text = "uiGroupBox11";
            this.uiGroupBox11.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox10
            // 
            this.uiGroupBox10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox10.Location = new System.Drawing.Point(929, 130);
            this.uiGroupBox10.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox10.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox10.Name = "uiGroupBox10";
            this.uiGroupBox10.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox10.Size = new System.Drawing.Size(234, 127);
            this.uiGroupBox10.TabIndex = 8;
            this.uiGroupBox10.Text = "uiGroupBox10";
            this.uiGroupBox10.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox9
            // 
            this.uiGroupBox9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox9.Location = new System.Drawing.Point(697, 130);
            this.uiGroupBox9.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox9.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox9.Name = "uiGroupBox9";
            this.uiGroupBox9.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox9.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox9.TabIndex = 7;
            this.uiGroupBox9.Text = "uiGroupBox9";
            this.uiGroupBox9.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox8
            // 
            this.uiGroupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox8.Location = new System.Drawing.Point(465, 130);
            this.uiGroupBox8.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox8.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox8.Name = "uiGroupBox8";
            this.uiGroupBox8.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox8.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox8.TabIndex = 6;
            this.uiGroupBox8.Text = "uiGroupBox8";
            this.uiGroupBox8.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox7
            // 
            this.uiGroupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox7.Location = new System.Drawing.Point(233, 130);
            this.uiGroupBox7.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox7.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox7.Name = "uiGroupBox7";
            this.uiGroupBox7.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox7.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox7.TabIndex = 5;
            this.uiGroupBox7.Text = "uiGroupBox7";
            this.uiGroupBox7.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox6
            // 
            this.uiGroupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox6.Location = new System.Drawing.Point(1, 130);
            this.uiGroupBox6.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox6.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox6.Name = "uiGroupBox6";
            this.uiGroupBox6.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox6.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox6.TabIndex = 4;
            this.uiGroupBox6.Text = "uiGroupBox6";
            this.uiGroupBox6.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox5
            // 
            this.uiGroupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox5.Location = new System.Drawing.Point(929, 1);
            this.uiGroupBox5.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox5.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox5.Name = "uiGroupBox5";
            this.uiGroupBox5.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox5.Size = new System.Drawing.Size(234, 127);
            this.uiGroupBox5.TabIndex = 3;
            this.uiGroupBox5.Text = "uiGroupBox5";
            this.uiGroupBox5.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox4
            // 
            this.uiGroupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox4.Location = new System.Drawing.Point(697, 1);
            this.uiGroupBox4.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox4.Name = "uiGroupBox4";
            this.uiGroupBox4.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox4.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox4.TabIndex = 1;
            this.uiGroupBox4.Text = "uiGroupBox4";
            this.uiGroupBox4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gpWriteSlaveID
            // 
            this.gpWriteSlaveID.Controls.Add(this.cmbSlaveType);
            this.gpWriteSlaveID.Controls.Add(this.cmbSlaveIDs);
            this.gpWriteSlaveID.Controls.Add(this.uiLabel1);
            this.gpWriteSlaveID.Controls.Add(this.uiLabel2);
            this.gpWriteSlaveID.Controls.Add(this.btnWriteSlaveID);
            this.gpWriteSlaveID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpWriteSlaveID.Font = new System.Drawing.Font("宋体", 8.5F);
            this.gpWriteSlaveID.Location = new System.Drawing.Point(1, 1);
            this.gpWriteSlaveID.Margin = new System.Windows.Forms.Padding(1);
            this.gpWriteSlaveID.MinimumSize = new System.Drawing.Size(1, 1);
            this.gpWriteSlaveID.Name = "gpWriteSlaveID";
            this.gpWriteSlaveID.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.gpWriteSlaveID.Size = new System.Drawing.Size(230, 127);
            this.gpWriteSlaveID.TabIndex = 0;
            this.gpWriteSlaveID.Text = "设置从站ID";
            this.gpWriteSlaveID.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSlaveType
            // 
            this.cmbSlaveType.DataSource = null;
            this.cmbSlaveType.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbSlaveType.FillColor = System.Drawing.Color.White;
            this.cmbSlaveType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbSlaveType.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbSlaveType.Items.AddRange(new object[] {
            "RL从站",
            "AD从站"});
            this.cmbSlaveType.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbSlaveType.Location = new System.Drawing.Point(67, 26);
            this.cmbSlaveType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSlaveType.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbSlaveType.Name = "cmbSlaveType";
            this.cmbSlaveType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbSlaveType.Size = new System.Drawing.Size(150, 29);
            this.cmbSlaveType.SymbolSize = 24;
            this.cmbSlaveType.TabIndex = 6;
            this.cmbSlaveType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbSlaveType.Watermark = "";
            // 
            // cmbSlaveIDs
            // 
            this.cmbSlaveIDs.DataSource = null;
            this.cmbSlaveIDs.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbSlaveIDs.FillColor = System.Drawing.Color.White;
            this.cmbSlaveIDs.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbSlaveIDs.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbSlaveIDs.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbSlaveIDs.Location = new System.Drawing.Point(67, 60);
            this.cmbSlaveIDs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSlaveIDs.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbSlaveIDs.Name = "cmbSlaveIDs";
            this.cmbSlaveIDs.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbSlaveIDs.Size = new System.Drawing.Size(150, 29);
            this.cmbSlaveIDs.SymbolSize = 24;
            this.cmbSlaveIDs.TabIndex = 5;
            this.cmbSlaveIDs.Text = "uiComboBox1";
            this.cmbSlaveIDs.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbSlaveIDs.Watermark = "";
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 8F);
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(6, 61);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(57, 23);
            this.uiLabel1.TabIndex = 4;
            this.uiLabel1.Text = "ID";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 8.5F);
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(6, 26);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(57, 23);
            this.uiLabel2.TabIndex = 2;
            this.uiLabel2.Text = "Type";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnWriteSlaveID
            // 
            this.btnWriteSlaveID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWriteSlaveID.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteSlaveID.Location = new System.Drawing.Point(9, 94);
            this.btnWriteSlaveID.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWriteSlaveID.Name = "btnWriteSlaveID";
            this.btnWriteSlaveID.Size = new System.Drawing.Size(208, 26);
            this.btnWriteSlaveID.TabIndex = 1;
            this.btnWriteSlaveID.Text = "写入从站ID";
            this.btnWriteSlaveID.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteSlaveID.Click += new System.EventHandler(this.btnWriteSlaveID_Click);
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.cmbSlaveCanFuncType);
            this.uiGroupBox2.Controls.Add(this.uiLabel3);
            this.uiGroupBox2.Controls.Add(this.btnWriteUartCanConfig);
            this.uiGroupBox2.Controls.Add(this.btnWriteSlaveCanFunc);
            this.uiGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox2.Location = new System.Drawing.Point(233, 1);
            this.uiGroupBox2.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox2.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox2.TabIndex = 1;
            this.uiGroupBox2.Text = "Uart_CAN";
            this.uiGroupBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSlaveCanFuncType
            // 
            this.cmbSlaveCanFuncType.DataSource = null;
            this.cmbSlaveCanFuncType.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbSlaveCanFuncType.FillColor = System.Drawing.Color.White;
            this.cmbSlaveCanFuncType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbSlaveCanFuncType.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbSlaveCanFuncType.Items.AddRange(new object[] {
            "从站通讯口",
            "UART_CAN口-500K",
            "UART_CAN口-1M"});
            this.cmbSlaveCanFuncType.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbSlaveCanFuncType.Location = new System.Drawing.Point(73, 26);
            this.cmbSlaveCanFuncType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSlaveCanFuncType.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbSlaveCanFuncType.Name = "cmbSlaveCanFuncType";
            this.cmbSlaveCanFuncType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbSlaveCanFuncType.Size = new System.Drawing.Size(150, 29);
            this.cmbSlaveCanFuncType.SymbolSize = 24;
            this.cmbSlaveCanFuncType.TabIndex = 6;
            this.cmbSlaveCanFuncType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbSlaveCanFuncType.Watermark = "";
            // 
            // uiLabel3
            // 
            this.uiLabel3.Font = new System.Drawing.Font("宋体", 8.5F);
            this.uiLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel3.Location = new System.Drawing.Point(9, 26);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(57, 23);
            this.uiLabel3.TabIndex = 2;
            this.uiLabel3.Text = "Type";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnWriteUartCanConfig
            // 
            this.btnWriteUartCanConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWriteUartCanConfig.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteUartCanConfig.Location = new System.Drawing.Point(12, 93);
            this.btnWriteUartCanConfig.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWriteUartCanConfig.Name = "btnWriteUartCanConfig";
            this.btnWriteUartCanConfig.Size = new System.Drawing.Size(208, 26);
            this.btnWriteUartCanConfig.Symbol = 61473;
            this.btnWriteUartCanConfig.TabIndex = 1;
            this.btnWriteUartCanConfig.Text = "配置UartCan";
            this.btnWriteUartCanConfig.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteUartCanConfig.Click += new System.EventHandler(this.btnWriteUartCanConfig_Click);
            // 
            // btnWriteSlaveCanFunc
            // 
            this.btnWriteSlaveCanFunc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWriteSlaveCanFunc.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteSlaveCanFunc.Location = new System.Drawing.Point(11, 61);
            this.btnWriteSlaveCanFunc.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWriteSlaveCanFunc.Name = "btnWriteSlaveCanFunc";
            this.btnWriteSlaveCanFunc.Size = new System.Drawing.Size(208, 26);
            this.btnWriteSlaveCanFunc.TabIndex = 1;
            this.btnWriteSlaveCanFunc.Text = "更新";
            this.btnWriteSlaveCanFunc.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteSlaveCanFunc.Click += new System.EventHandler(this.btnWriteSlaveCanFunc_Click);
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.Controls.Add(this.txtToUpdateLinBauteRate);
            this.uiGroupBox3.Controls.Add(this.cmbToUpdateLinCh);
            this.uiGroupBox3.Controls.Add(this.uiLabel5);
            this.uiGroupBox3.Controls.Add(this.btnUpdateLinBauteRate);
            this.uiGroupBox3.Controls.Add(this.uiLabel4);
            this.uiGroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox3.Location = new System.Drawing.Point(465, 1);
            this.uiGroupBox3.Margin = new System.Windows.Forms.Padding(1);
            this.uiGroupBox3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox3.Size = new System.Drawing.Size(230, 127);
            this.uiGroupBox3.TabIndex = 2;
            this.uiGroupBox3.Text = "LIN波特率";
            this.uiGroupBox3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtToUpdateLinBauteRate
            // 
            this.txtToUpdateLinBauteRate.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtToUpdateLinBauteRate.DoubleValue = 19200D;
            this.txtToUpdateLinBauteRate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtToUpdateLinBauteRate.IntValue = 19200;
            this.txtToUpdateLinBauteRate.Location = new System.Drawing.Point(72, 61);
            this.txtToUpdateLinBauteRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtToUpdateLinBauteRate.Maximum = 500000D;
            this.txtToUpdateLinBauteRate.Minimum = 110D;
            this.txtToUpdateLinBauteRate.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtToUpdateLinBauteRate.Name = "txtToUpdateLinBauteRate";
            this.txtToUpdateLinBauteRate.Padding = new System.Windows.Forms.Padding(5);
            this.txtToUpdateLinBauteRate.ShowText = false;
            this.txtToUpdateLinBauteRate.Size = new System.Drawing.Size(150, 29);
            this.txtToUpdateLinBauteRate.TabIndex = 7;
            this.txtToUpdateLinBauteRate.Text = "19200";
            this.txtToUpdateLinBauteRate.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtToUpdateLinBauteRate.Type = Sunny.UI.UITextBox.UIEditType.Integer;
            this.txtToUpdateLinBauteRate.Watermark = "";
            // 
            // cmbToUpdateLinCh
            // 
            this.cmbToUpdateLinCh.DataSource = null;
            this.cmbToUpdateLinCh.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbToUpdateLinCh.FillColor = System.Drawing.Color.White;
            this.cmbToUpdateLinCh.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbToUpdateLinCh.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbToUpdateLinCh.Items.AddRange(new object[] {
            "LIN1设置为Master",
            "LIN1设置为Slave",
            "LIN2设置为Master",
            "LIN2设置为Slave"});
            this.cmbToUpdateLinCh.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbToUpdateLinCh.Location = new System.Drawing.Point(72, 28);
            this.cmbToUpdateLinCh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbToUpdateLinCh.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbToUpdateLinCh.Name = "cmbToUpdateLinCh";
            this.cmbToUpdateLinCh.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbToUpdateLinCh.Size = new System.Drawing.Size(150, 29);
            this.cmbToUpdateLinCh.SymbolSize = 24;
            this.cmbToUpdateLinCh.TabIndex = 6;
            this.cmbToUpdateLinCh.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbToUpdateLinCh.Watermark = "";
            // 
            // uiLabel5
            // 
            this.uiLabel5.Font = new System.Drawing.Font("宋体", 8.5F);
            this.uiLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel5.Location = new System.Drawing.Point(8, 60);
            this.uiLabel5.Name = "uiLabel5";
            this.uiLabel5.Size = new System.Drawing.Size(57, 23);
            this.uiLabel5.TabIndex = 2;
            this.uiLabel5.Text = "波特率:";
            this.uiLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnUpdateLinBauteRate
            // 
            this.btnUpdateLinBauteRate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdateLinBauteRate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdateLinBauteRate.Location = new System.Drawing.Point(13, 93);
            this.btnUpdateLinBauteRate.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnUpdateLinBauteRate.Name = "btnUpdateLinBauteRate";
            this.btnUpdateLinBauteRate.Size = new System.Drawing.Size(208, 26);
            this.btnUpdateLinBauteRate.TabIndex = 1;
            this.btnUpdateLinBauteRate.Text = "更新波特率";
            this.btnUpdateLinBauteRate.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdateLinBauteRate.Click += new System.EventHandler(this.btnUpdateLinBauteRate_Click);
            // 
            // uiLabel4
            // 
            this.uiLabel4.Font = new System.Drawing.Font("宋体", 8.5F);
            this.uiLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel4.Location = new System.Drawing.Point(8, 28);
            this.uiLabel4.Name = "uiLabel4";
            this.uiLabel4.Size = new System.Drawing.Size(57, 23);
            this.uiLabel4.TabIndex = 2;
            this.uiLabel4.Text = "CH:";
            this.uiLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(0, 40);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(200, 60);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "LIN";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(0, 40);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(200, 60);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "CAN";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // FrmSingleMcuPage
            // 
            this.AllowShowTitle = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1170, 768);
            this.Controls.Add(this.mainTablePanel);
            this.Name = "FrmSingleMcuPage";
            this.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.ShowTitle = true;
            this.Symbol = 361720;
            this.Text = "FrmSingleMcuPage";
            this.mainTablePanel.ResumeLayout(false);
            this.uiPanel1.ResumeLayout(false);
            this.uiTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.controlPanel.ResumeLayout(false);
            this.diTablePanel.ResumeLayout(false);
            this.doTablePanel.ResumeLayout(false);
            this.rlPanel.ResumeLayout(false);
            this.rlPanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.adPanel.ResumeLayout(false);
            this.adPanel.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.flashPanel.ResumeLayout(false);
            this.gpWriteSlaveID.ResumeLayout(false);
            this.uiGroupBox2.ResumeLayout(false);
            this.uiGroupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UITableLayoutPanel mainTablePanel;
        private UIPanel uiPanel1;
        private UIIPTextBox txtIp;
        private UISymbolButton btnConnect;
        private UITabControl uiTabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private UITableLayoutPanel controlPanel;
        private UITableLayoutPanel diTablePanel;
        private UILight di1;
        private UILight di2;
        private UILight di3;
        private UILight di4;
        private UILight di5;
        private UILight di6;
        private UISymbolButton btnMasterReadDIs;
        private UITableLayoutPanel doTablePanel;
        private UISwitch do1;
        private UISymbolButton btnMasterSetDOs;
        private UISwitch do4;
        private UISwitch do3;
        private UISwitch do2;
        private UIPanel rlPanel;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private UIFlowLayoutPanel uiFlowLayoutPanel1;
        private UIPanel adPanel;
        private ToolStrip toolStrip2;
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton4;
        private UIFlowLayoutPanel uiFlowLayoutPanel2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private UITableLayoutPanel flashPanel;
        private UIGroupBox uiGroupBox25;
        private UIGroupBox uiGroupBox24;
        private UIGroupBox uiGroupBox23;
        private UIGroupBox uiGroupBox22;
        private UIGroupBox uiGroupBox21;
        private UIGroupBox uiGroupBox20;
        private UIGroupBox uiGroupBox19;
        private UIGroupBox uiGroupBox18;
        private UIGroupBox uiGroupBox17;
        private UIGroupBox uiGroupBox16;
        private UIGroupBox uiGroupBox15;
        private UIGroupBox uiGroupBox14;
        private UIGroupBox uiGroupBox13;
        private UIGroupBox uiGroupBox12;
        private UIGroupBox uiGroupBox11;
        private UIGroupBox uiGroupBox10;
        private UIGroupBox uiGroupBox9;
        private UIGroupBox uiGroupBox8;
        private UIGroupBox uiGroupBox7;
        private UIGroupBox uiGroupBox6;
        private UIGroupBox uiGroupBox5;
        private UIGroupBox uiGroupBox4;
        private UIGroupBox uiGroupBox2;
        private UIGroupBox uiGroupBox3;
        private UIGroupBox gpWriteSlaveID;
        private UILabel uiLabel1;
        private UILabel uiLabel2;
        private UISymbolButton btnWriteSlaveID;
        private UIComboBox cmbSlaveType;
        private UIComboBox cmbSlaveIDs;
        private UIComboBox cmbSlaveCanFuncType;
        private UILabel uiLabel3;
        private UISymbolButton btnWriteSlaveCanFunc;
        private UISymbolButton btnWriteUartCanConfig;
        private UIComboBox cmbToUpdateLinCh;
        private UILabel uiLabel5;
        private UISymbolButton btnUpdateLinBauteRate;
        private UILabel uiLabel4;
        private UITextBox txtToUpdateLinBauteRate;
    }
}