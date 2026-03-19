using System.Drawing;
using HZH_Controls.Controls;
using HZH_Controls.Controls.Btn;

namespace CheckSystem
{
    partial class FormLoadXml
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开配置文件夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置当前设备号ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uiSplitContainer1 = new Sunny.UI.UISplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddConfig = new Sunny.UI.UISymbolButton();
            this.btbAddCcd = new Sunny.UI.UISymbolButton();
            this.btnEdit = new Sunny.UI.UISymbolButton();
            this.btnLoad = new Sunny.UI.UISymbolButton();
            this.btnDelete = new Sunny.UI.UISymbolButton();
            this.mainPainel = new System.Windows.Forms.Panel();
            this.tableOnOffLine = new Sunny.UI.UITableLayoutPanel();
            this.btnShowOnLineFile = new Sunny.UI.UISymbolButton();
            this.btnShowOffLineFile = new Sunny.UI.UISymbolButton();
            this.toolsTitlePanel = new Sunny.UI.UITitlePanel();
            this.toolFlowPanel = new Sunny.UI.UIFlowLayoutPanel();
            this.searchPanel = new Sunny.UI.UIPanel();
            this.lstXml = new Sunny.UI.UIListBox();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.cmbSearchText = new Sunny.UI.UIComboDataGridView();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainer1)).BeginInit();
            this.uiSplitContainer1.Panel1.SuspendLayout();
            this.uiSplitContainer1.Panel2.SuspendLayout();
            this.uiSplitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.mainPainel.SuspendLayout();
            this.tableOnOffLine.SuspendLayout();
            this.toolsTitlePanel.SuspendLayout();
            this.searchPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新ToolStripMenuItem,
            this.打开配置文件夹ToolStripMenuItem,
            this.设置当前设备号ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(175, 70);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // 打开配置文件夹ToolStripMenuItem
            // 
            this.打开配置文件夹ToolStripMenuItem.Name = "打开配置文件夹ToolStripMenuItem";
            this.打开配置文件夹ToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.打开配置文件夹ToolStripMenuItem.Text = "打开配置文件夹";
            this.打开配置文件夹ToolStripMenuItem.Click += new System.EventHandler(this.打开配置文件夹ToolStripMenuItem_Click);
            // 
            // 设置当前设备号ToolStripMenuItem
            // 
            this.设置当前设备号ToolStripMenuItem.Name = "设置当前设备号ToolStripMenuItem";
            this.设置当前设备号ToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.设置当前设备号ToolStripMenuItem.Text = "设置当前设备IN号";
            // 
            // uiSplitContainer1
            // 
            this.uiSplitContainer1.CollapsePanel = Sunny.UI.UISplitContainer.UICollapsePanel.Panel2;
            this.uiSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.uiSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSplitContainer1.Location = new System.Drawing.Point(0, 35);
            this.uiSplitContainer1.MinimumSize = new System.Drawing.Size(20, 20);
            this.uiSplitContainer1.Name = "uiSplitContainer1";
            // 
            // uiSplitContainer1.Panel1
            // 
            this.uiSplitContainer1.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // uiSplitContainer1.Panel2
            // 
            this.uiSplitContainer1.Panel2.Controls.Add(this.toolsTitlePanel);
            this.uiSplitContainer1.Size = new System.Drawing.Size(1022, 726);
            this.uiSplitContainer1.SplitterDistance = 707;
            this.uiSplitContainer1.SplitterWidth = 20;
            this.uiSplitContainer1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.Controls.Add(this.btnAddConfig, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.btbAddCcd, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnEdit, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnLoad, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnDelete, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.mainPainel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableOnOffLine, 5, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(707, 726);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // btnAddConfig
            // 
            this.btnAddConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddConfig.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnAddConfig.Location = new System.Drawing.Point(471, 629);
            this.btnAddConfig.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAddConfig.Name = "btnAddConfig";
            this.btnAddConfig.Size = new System.Drawing.Size(111, 94);
            this.btnAddConfig.Style = Sunny.UI.UIStyle.Custom;
            this.btnAddConfig.StyleCustomMode = true;
            this.btnAddConfig.Symbol = 559362;
            this.btnAddConfig.TabIndex = 86;
            this.btnAddConfig.Text = "添加流程配置";
            this.btnAddConfig.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // btbAddCcd
            // 
            this.btbAddCcd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btbAddCcd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btbAddCcd.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btbAddCcd.Location = new System.Drawing.Point(354, 629);
            this.btbAddCcd.MinimumSize = new System.Drawing.Size(1, 1);
            this.btbAddCcd.Name = "btbAddCcd";
            this.btbAddCcd.Size = new System.Drawing.Size(111, 94);
            this.btbAddCcd.Style = Sunny.UI.UIStyle.Custom;
            this.btbAddCcd.StyleCustomMode = true;
            this.btbAddCcd.Symbol = 558425;
            this.btbAddCcd.TabIndex = 85;
            this.btbAddCcd.Text = "添加CCD配置";
            this.btbAddCcd.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btbAddCcd.Click += new System.EventHandler(this.btnAddCdd_BtnClick);
            // 
            // btnEdit
            // 
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEdit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btnEdit.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btnEdit.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.btnEdit.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnEdit.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnEdit.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnEdit.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.btnEdit.Location = new System.Drawing.Point(237, 629);
            this.btnEdit.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btnEdit.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.btnEdit.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnEdit.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnEdit.Size = new System.Drawing.Size(111, 94);
            this.btnEdit.Style = Sunny.UI.UIStyle.Custom;
            this.btnEdit.StyleCustomMode = true;
            this.btnEdit.Symbol = 559202;
            this.btnEdit.TabIndex = 84;
            this.btnEdit.Text = "编辑";
            this.btnEdit.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_BtnClick);
            // 
            // btnLoad
            // 
            this.btnLoad.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoad.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.btnLoad.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.btnLoad.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.btnLoad.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.btnLoad.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.btnLoad.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnLoad.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.btnLoad.Location = new System.Drawing.Point(3, 629);
            this.btnLoad.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.btnLoad.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.btnLoad.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.btnLoad.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.btnLoad.Size = new System.Drawing.Size(111, 94);
            this.btnLoad.Style = Sunny.UI.UIStyle.Custom;
            this.btnLoad.StyleCustomMode = true;
            this.btnLoad.Symbol = 57389;
            this.btnLoad.TabIndex = 83;
            this.btnLoad.Text = "加载";
            this.btnLoad.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_BtnClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnDelete.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnDelete.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            this.btnDelete.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDelete.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDelete.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnDelete.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.btnDelete.Location = new System.Drawing.Point(120, 629);
            this.btnDelete.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnDelete.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            this.btnDelete.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDelete.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDelete.Size = new System.Drawing.Size(111, 94);
            this.btnDelete.Style = Sunny.UI.UIStyle.Custom;
            this.btnDelete.StyleCustomMode = true;
            this.btnDelete.Symbol = 61453;
            this.btnDelete.TabIndex = 81;
            this.btnDelete.Text = "删除";
            this.btnDelete.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_BtnClick);
            // 
            // mainPainel
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.mainPainel, 6);
            this.mainPainel.Controls.Add(this.lstXml);
            this.mainPainel.Controls.Add(this.searchPanel);
            this.mainPainel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPainel.Location = new System.Drawing.Point(0, 0);
            this.mainPainel.Margin = new System.Windows.Forms.Padding(0);
            this.mainPainel.Name = "mainPainel";
            this.mainPainel.Size = new System.Drawing.Size(707, 626);
            this.mainPainel.TabIndex = 7;
            // 
            // tableOnOffLine
            // 
            this.tableOnOffLine.ColumnCount = 1;
            this.tableOnOffLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableOnOffLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableOnOffLine.Controls.Add(this.btnShowOnLineFile, 0, 0);
            this.tableOnOffLine.Controls.Add(this.btnShowOffLineFile, 0, 1);
            this.tableOnOffLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableOnOffLine.Location = new System.Drawing.Point(585, 626);
            this.tableOnOffLine.Margin = new System.Windows.Forms.Padding(0);
            this.tableOnOffLine.Name = "tableOnOffLine";
            this.tableOnOffLine.RowCount = 2;
            this.tableOnOffLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableOnOffLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableOnOffLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableOnOffLine.Size = new System.Drawing.Size(122, 100);
            this.tableOnOffLine.TabIndex = 87;
            this.tableOnOffLine.TagString = null;
            // 
            // btnShowOnLineFile
            // 
            this.btnShowOnLineFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShowOnLineFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowOnLineFile.FillColor = System.Drawing.Color.Tan;
            this.btnShowOnLineFile.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnShowOnLineFile.Location = new System.Drawing.Point(3, 3);
            this.btnShowOnLineFile.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnShowOnLineFile.Name = "btnShowOnLineFile";
            this.btnShowOnLineFile.Size = new System.Drawing.Size(116, 44);
            this.btnShowOnLineFile.Style = Sunny.UI.UIStyle.Custom;
            this.btnShowOnLineFile.StyleCustomMode = true;
            this.btnShowOnLineFile.Symbol = 561675;
            this.btnShowOnLineFile.TabIndex = 89;
            this.btnShowOnLineFile.Text = "显示工单文件";
            this.btnShowOnLineFile.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShowOnLineFile.Click += new System.EventHandler(this.btnShowOnLineFile_Click);
            // 
            // btnShowOffLineFile
            // 
            this.btnShowOffLineFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShowOffLineFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowOffLineFile.FillColor = System.Drawing.Color.DarkKhaki;
            this.btnShowOffLineFile.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnShowOffLineFile.Location = new System.Drawing.Point(3, 53);
            this.btnShowOffLineFile.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnShowOffLineFile.Name = "btnShowOffLineFile";
            this.btnShowOffLineFile.Size = new System.Drawing.Size(116, 44);
            this.btnShowOffLineFile.Style = Sunny.UI.UIStyle.Custom;
            this.btnShowOffLineFile.StyleCustomMode = true;
            this.btnShowOffLineFile.Symbol = 561440;
            this.btnShowOffLineFile.TabIndex = 88;
            this.btnShowOffLineFile.Text = "显示离线文件";
            this.btnShowOffLineFile.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShowOffLineFile.Click += new System.EventHandler(this.btnShowOffLineFile_Click);
            // 
            // toolsTitlePanel
            // 
            this.toolsTitlePanel.Controls.Add(this.toolFlowPanel);
            this.toolsTitlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolsTitlePanel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.toolsTitlePanel.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.toolsTitlePanel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolsTitlePanel.Location = new System.Drawing.Point(0, 0);
            this.toolsTitlePanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.toolsTitlePanel.MinimumSize = new System.Drawing.Size(1, 1);
            this.toolsTitlePanel.Name = "toolsTitlePanel";
            this.toolsTitlePanel.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.toolsTitlePanel.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.toolsTitlePanel.ShowText = false;
            this.toolsTitlePanel.Size = new System.Drawing.Size(295, 726);
            this.toolsTitlePanel.Style = Sunny.UI.UIStyle.Custom;
            this.toolsTitlePanel.TabIndex = 0;
            this.toolsTitlePanel.Text = "调试工具集合：";
            this.toolsTitlePanel.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolsTitlePanel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolsTitlePanel.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            // 
            // toolFlowPanel
            // 
            this.toolFlowPanel.AutoSize = true;
            this.toolFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolFlowPanel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.toolFlowPanel.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.toolFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.toolFlowPanel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolFlowPanel.Location = new System.Drawing.Point(0, 35);
            this.toolFlowPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.toolFlowPanel.MinimumSize = new System.Drawing.Size(1, 1);
            this.toolFlowPanel.Name = "toolFlowPanel";
            this.toolFlowPanel.Padding = new System.Windows.Forms.Padding(2);
            this.toolFlowPanel.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.toolFlowPanel.ScrollBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.toolFlowPanel.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.toolFlowPanel.ScrollBarStyleInherited = false;
            this.toolFlowPanel.ShowText = false;
            this.toolFlowPanel.Size = new System.Drawing.Size(295, 691);
            this.toolFlowPanel.Style = Sunny.UI.UIStyle.Custom;
            this.toolFlowPanel.TabIndex = 0;
            this.toolFlowPanel.Text = "uiFlowLayoutPanel1";
            this.toolFlowPanel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolFlowPanel.WrapContents = false;
            // 
            // searchPanel
            // 
            this.searchPanel.Controls.Add(this.cmbSearchText);
            this.searchPanel.Controls.Add(this.uiMarkLabel1);
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchPanel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.searchPanel.Location = new System.Drawing.Point(0, 0);
            this.searchPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.searchPanel.MinimumSize = new System.Drawing.Size(1, 1);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(707, 30);
            this.searchPanel.Style = Sunny.UI.UIStyle.Custom;
            this.searchPanel.TabIndex = 3;
            this.searchPanel.Text = "uiPanel1";
            this.searchPanel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstXml
            // 
            this.lstXml.ContextMenuStrip = this.contextMenuStrip1;
            this.lstXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstXml.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lstXml.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.lstXml.ItemSelectForeColor = System.Drawing.Color.White;
            this.lstXml.Location = new System.Drawing.Point(0, 30);
            this.lstXml.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstXml.MinimumSize = new System.Drawing.Size(1, 1);
            this.lstXml.Name = "lstXml";
            this.lstXml.Padding = new System.Windows.Forms.Padding(2);
            this.lstXml.ShowText = false;
            this.lstXml.Size = new System.Drawing.Size(707, 596);
            this.lstXml.Style = Sunny.UI.UIStyle.Custom;
            this.lstXml.TabIndex = 4;
            this.lstXml.Text = "uiListBox1";
            this.lstXml.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstXml_MouseDoubleClick);
            this.lstXml.SizeChanged += new System.EventHandler(this.lstXml_SizeChanged);
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiMarkLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel1.Location = new System.Drawing.Point(0, 0);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(121, 30);
            this.uiMarkLabel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel1.TabIndex = 0;
            this.uiMarkLabel1.Text = "按名称搜索：";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSearchText
            // 
            this.cmbSearchText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSearchText.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbSearchText.FillColor = System.Drawing.Color.White;
            this.cmbSearchText.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbSearchText.Location = new System.Drawing.Point(121, 0);
            this.cmbSearchText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSearchText.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbSearchText.Name = "cmbSearchText";
            this.cmbSearchText.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbSearchText.Size = new System.Drawing.Size(586, 30);
            this.cmbSearchText.Style = Sunny.UI.UIStyle.Custom;
            this.cmbSearchText.SymbolSize = 24;
            this.cmbSearchText.TabIndex = 75;
            this.cmbSearchText.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbSearchText.Watermark = "";
            // 
            // FormLoadXml
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1022, 761);
            this.Controls.Add(this.uiSplitContainer1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 800);
            this.MinimumSize = new System.Drawing.Size(1022, 685);
            this.Name = "FormLoadXml";
            this.Text = "产品加载";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 1022, 761);
            this.contextMenuStrip1.ResumeLayout(false);
            this.uiSplitContainer1.Panel1.ResumeLayout(false);
            this.uiSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainer1)).EndInit();
            this.uiSplitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.mainPainel.ResumeLayout(false);
            this.tableOnOffLine.ResumeLayout(false);
            this.toolsTitlePanel.ResumeLayout(false);
            this.toolsTitlePanel.PerformLayout();
            this.searchPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 打开配置文件夹ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private Sunny.UI.UISplitContainer uiSplitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Sunny.UI.UISymbolButton btnLoad;
        private Sunny.UI.UISymbolButton btnDelete;
        private Sunny.UI.UISymbolButton btnEdit;
        private Sunny.UI.UISymbolButton btnAddConfig;
        private Sunny.UI.UISymbolButton btbAddCcd;
        private System.Windows.Forms.Panel mainPainel;
        private Sunny.UI.UITitlePanel toolsTitlePanel;
        private Sunny.UI.UIFlowLayoutPanel toolFlowPanel;
        private Sunny.UI.UITableLayoutPanel tableOnOffLine;
        private Sunny.UI.UISymbolButton btnShowOnLineFile;
        private Sunny.UI.UISymbolButton btnShowOffLineFile;
        private System.Windows.Forms.ToolStripMenuItem 设置当前设备号ToolStripMenuItem;
        private Sunny.UI.UIPanel searchPanel;
        private Sunny.UI.UIListBox lstXml;
        private Sunny.UI.UIComboDataGridView cmbSearchText;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
    }
}