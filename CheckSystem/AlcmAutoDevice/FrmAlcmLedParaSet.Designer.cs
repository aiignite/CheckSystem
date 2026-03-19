namespace CheckSystem.AlcmAutoDevice
{
    partial class FrmAlcmLedParaSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAlcmLedParaSet));
            this.userDataGrid = new UserControls.UserDataGrid();
            this.cameraParaGroup = new System.Windows.Forms.GroupBox();
            this.cameraPanel = new System.Windows.Forms.TableLayoutPanel();
            this.cameraDelayTxt = new UserControls.LabelCombox();
            this.funcNameTxt = new UserControls.LabelText();
            this.cameraFrameCountTxt = new UserControls.LabelCombox();
            this.cameraFrameRateTxt = new UserControls.LabelCombox();
            this.cameraShutterTxt = new UserControls.LabelCombox();
            this.cameraSnTxt = new UserControls.LabelCombox();
            this.mainTable = new System.Windows.Forms.TableLayoutPanel();
            this.leftMainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.stepPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnNextStep = new System.Windows.Forms.Button();
            this.btnLastStep = new System.Windows.Forms.Button();
            this.lblWithCmbStepsList = new UserControls.LabelCombox();
            this.btnSaveParas = new System.Windows.Forms.Button();
            this.imgViewerPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ImageViewer = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripCmbImgList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripBtnOpenPics = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxColorPlaneExtraction = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxLookupTable = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripBtnFindCircle = new System.Windows.Forms.ToolStripButton();
            this.toolStripCmbRadiusRange = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripBtnStopAutoGetRrays = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnOpenControllerDebuger = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnGetRrays = new System.Windows.Forms.ToolStripButton();
            this.toolStripCmbRrayPercent = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButtonDeleteRect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAddRect = new System.Windows.Forms.ToolStripButton();
            this.cameraParaGroup.SuspendLayout();
            this.cameraPanel.SuspendLayout();
            this.mainTable.SuspendLayout();
            this.leftMainPanel.SuspendLayout();
            this.stepPanel.SuspendLayout();
            this.imgViewerPanel.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // userDataGrid
            // 
            this.userDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userDataGrid.Location = new System.Drawing.Point(893, 135);
            this.userDataGrid.Name = "userDataGrid";
            this.userDataGrid.Size = new System.Drawing.Size(588, 523);
            this.userDataGrid.TabIndex = 5;
            // 
            // cameraParaGroup
            // 
            this.cameraParaGroup.Controls.Add(this.cameraPanel);
            this.cameraParaGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraParaGroup.Location = new System.Drawing.Point(893, 3);
            this.cameraParaGroup.Name = "cameraParaGroup";
            this.cameraParaGroup.Size = new System.Drawing.Size(588, 126);
            this.cameraParaGroup.TabIndex = 1;
            this.cameraParaGroup.TabStop = false;
            this.cameraParaGroup.Text = "相机参数";
            // 
            // cameraPanel
            // 
            this.cameraPanel.ColumnCount = 2;
            this.cameraPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.cameraPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.cameraPanel.Controls.Add(this.cameraDelayTxt, 0, 2);
            this.cameraPanel.Controls.Add(this.funcNameTxt, 0, 2);
            this.cameraPanel.Controls.Add(this.cameraFrameCountTxt, 1, 1);
            this.cameraPanel.Controls.Add(this.cameraFrameRateTxt, 0, 1);
            this.cameraPanel.Controls.Add(this.cameraShutterTxt, 1, 0);
            this.cameraPanel.Controls.Add(this.cameraSnTxt, 0, 0);
            this.cameraPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraPanel.Location = new System.Drawing.Point(3, 17);
            this.cameraPanel.Name = "cameraPanel";
            this.cameraPanel.RowCount = 3;
            this.cameraPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.cameraPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.cameraPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.cameraPanel.Size = new System.Drawing.Size(582, 106);
            this.cameraPanel.TabIndex = 0;
            // 
            // cameraDelayTxt
            // 
            this.cameraDelayTxt.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.cameraDelayTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraDelayTxt.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cameraDelayTxt.LabelString = "拍照延时";
            this.cameraDelayTxt.Location = new System.Drawing.Point(2, 72);
            this.cameraDelayTxt.Margin = new System.Windows.Forms.Padding(2);
            this.cameraDelayTxt.Name = "cameraDelayTxt";
            this.cameraDelayTxt.Size = new System.Drawing.Size(287, 32);
            this.cameraDelayTxt.TabIndex = 5;
            // 
            // funcNameTxt
            // 
            this.funcNameTxt.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.funcNameTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.funcNameTxt.Enabled = false;
            this.funcNameTxt.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.funcNameTxt.LabelString = "检测项";
            this.funcNameTxt.Location = new System.Drawing.Point(293, 72);
            this.funcNameTxt.Margin = new System.Windows.Forms.Padding(2);
            this.funcNameTxt.Name = "funcNameTxt";
            this.funcNameTxt.Size = new System.Drawing.Size(287, 32);
            this.funcNameTxt.TabIndex = 4;
            // 
            // cameraFrameCountTxt
            // 
            this.cameraFrameCountTxt.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.cameraFrameCountTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraFrameCountTxt.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cameraFrameCountTxt.LabelString = "帧数";
            this.cameraFrameCountTxt.Location = new System.Drawing.Point(293, 37);
            this.cameraFrameCountTxt.Margin = new System.Windows.Forms.Padding(2);
            this.cameraFrameCountTxt.Name = "cameraFrameCountTxt";
            this.cameraFrameCountTxt.Size = new System.Drawing.Size(287, 31);
            this.cameraFrameCountTxt.TabIndex = 3;
            // 
            // cameraFrameRateTxt
            // 
            this.cameraFrameRateTxt.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.cameraFrameRateTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraFrameRateTxt.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cameraFrameRateTxt.LabelString = "帧率";
            this.cameraFrameRateTxt.Location = new System.Drawing.Point(2, 37);
            this.cameraFrameRateTxt.Margin = new System.Windows.Forms.Padding(2);
            this.cameraFrameRateTxt.Name = "cameraFrameRateTxt";
            this.cameraFrameRateTxt.Size = new System.Drawing.Size(287, 31);
            this.cameraFrameRateTxt.TabIndex = 2;
            // 
            // cameraShutterTxt
            // 
            this.cameraShutterTxt.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.cameraShutterTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraShutterTxt.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cameraShutterTxt.LabelString = "快门";
            this.cameraShutterTxt.Location = new System.Drawing.Point(293, 2);
            this.cameraShutterTxt.Margin = new System.Windows.Forms.Padding(2);
            this.cameraShutterTxt.Name = "cameraShutterTxt";
            this.cameraShutterTxt.Size = new System.Drawing.Size(287, 31);
            this.cameraShutterTxt.TabIndex = 1;
            // 
            // cameraSnTxt
            // 
            this.cameraSnTxt.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.cameraSnTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraSnTxt.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cameraSnTxt.LabelString = "相机SN";
            this.cameraSnTxt.Location = new System.Drawing.Point(0, 0);
            this.cameraSnTxt.Margin = new System.Windows.Forms.Padding(0);
            this.cameraSnTxt.Name = "cameraSnTxt";
            this.cameraSnTxt.Size = new System.Drawing.Size(291, 35);
            this.cameraSnTxt.TabIndex = 6;
            // 
            // mainTable
            // 
            this.mainTable.ColumnCount = 2;
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.mainTable.Controls.Add(this.cameraParaGroup, 1, 0);
            this.mainTable.Controls.Add(this.userDataGrid, 1, 1);
            this.mainTable.Controls.Add(this.leftMainPanel, 0, 0);
            this.mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTable.Location = new System.Drawing.Point(0, 0);
            this.mainTable.Name = "mainTable";
            this.mainTable.RowCount = 2;
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.mainTable.Size = new System.Drawing.Size(1484, 661);
            this.mainTable.TabIndex = 1;
            // 
            // leftMainPanel
            // 
            this.leftMainPanel.ColumnCount = 1;
            this.leftMainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.leftMainPanel.Controls.Add(this.stepPanel, 0, 0);
            this.leftMainPanel.Controls.Add(this.imgViewerPanel, 0, 1);
            this.leftMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftMainPanel.Location = new System.Drawing.Point(3, 3);
            this.leftMainPanel.Name = "leftMainPanel";
            this.leftMainPanel.RowCount = 2;
            this.mainTable.SetRowSpan(this.leftMainPanel, 2);
            this.leftMainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.leftMainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.leftMainPanel.Size = new System.Drawing.Size(884, 655);
            this.leftMainPanel.TabIndex = 6;
            // 
            // stepPanel
            // 
            this.stepPanel.ColumnCount = 4;
            this.stepPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.stepPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.stepPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.stepPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.stepPanel.Controls.Add(this.btnNextStep, 0, 0);
            this.stepPanel.Controls.Add(this.btnLastStep, 0, 0);
            this.stepPanel.Controls.Add(this.lblWithCmbStepsList, 0, 0);
            this.stepPanel.Controls.Add(this.btnSaveParas, 1, 0);
            this.stepPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stepPanel.Location = new System.Drawing.Point(3, 3);
            this.stepPanel.Name = "stepPanel";
            this.stepPanel.RowCount = 1;
            this.stepPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.stepPanel.Size = new System.Drawing.Size(878, 44);
            this.stepPanel.TabIndex = 0;
            // 
            // btnNextStep
            // 
            this.btnNextStep.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnNextStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNextStep.Location = new System.Drawing.Point(614, 1);
            this.btnNextStep.Margin = new System.Windows.Forms.Padding(1);
            this.btnNextStep.Name = "btnNextStep";
            this.btnNextStep.Size = new System.Drawing.Size(129, 42);
            this.btnNextStep.TabIndex = 3;
            this.btnNextStep.Text = "下一步";
            this.btnNextStep.UseVisualStyleBackColor = false;
            this.btnNextStep.Click += new System.EventHandler(this.btnNextStep_Click);
            // 
            // btnLastStep
            // 
            this.btnLastStep.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnLastStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLastStep.Location = new System.Drawing.Point(483, 1);
            this.btnLastStep.Margin = new System.Windows.Forms.Padding(1);
            this.btnLastStep.Name = "btnLastStep";
            this.btnLastStep.Size = new System.Drawing.Size(129, 42);
            this.btnLastStep.TabIndex = 2;
            this.btnLastStep.Text = "上一步";
            this.btnLastStep.UseVisualStyleBackColor = false;
            this.btnLastStep.Click += new System.EventHandler(this.btnLastStep_Click);
            // 
            // lblWithCmbStepsList
            // 
            this.lblWithCmbStepsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWithCmbStepsList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWithCmbStepsList.LabelString = "当前步骤：";
            this.lblWithCmbStepsList.Location = new System.Drawing.Point(1, 1);
            this.lblWithCmbStepsList.Margin = new System.Windows.Forms.Padding(1);
            this.lblWithCmbStepsList.Name = "lblWithCmbStepsList";
            this.lblWithCmbStepsList.Size = new System.Drawing.Size(480, 42);
            this.lblWithCmbStepsList.TabIndex = 0;
            // 
            // btnSaveParas
            // 
            this.btnSaveParas.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnSaveParas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveParas.Enabled = false;
            this.btnSaveParas.Location = new System.Drawing.Point(745, 1);
            this.btnSaveParas.Margin = new System.Windows.Forms.Padding(1);
            this.btnSaveParas.Name = "btnSaveParas";
            this.btnSaveParas.Size = new System.Drawing.Size(132, 42);
            this.btnSaveParas.TabIndex = 1;
            this.btnSaveParas.Text = "保存参数";
            this.btnSaveParas.UseVisualStyleBackColor = false;
            this.btnSaveParas.Click += new System.EventHandler(this.btnSaveParas_Click);
            // 
            // imgViewerPanel
            // 
            this.imgViewerPanel.ColumnCount = 1;
            this.imgViewerPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.imgViewerPanel.Controls.Add(this.ImageViewer, 0, 1);
            this.imgViewerPanel.Controls.Add(this.toolStrip2, 0, 0);
            this.imgViewerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgViewerPanel.Location = new System.Drawing.Point(3, 53);
            this.imgViewerPanel.Name = "imgViewerPanel";
            this.imgViewerPanel.RowCount = 2;
            this.imgViewerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.imgViewerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.imgViewerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.imgViewerPanel.Size = new System.Drawing.Size(878, 599);
            this.imgViewerPanel.TabIndex = 1;
            // 
            // ImageViewer
            // 
            this.ImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageViewer.Location = new System.Drawing.Point(3, 38);
            this.ImageViewer.Name = "ImageViewer";
            this.ImageViewer.Size = new System.Drawing.Size(872, 558);
            this.ImageViewer.TabIndex = 7;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCmbImgList,
            this.toolStripBtnOpenPics,
            this.toolStripButtonAddRect,
            this.toolStripLabel2,
            this.toolStripComboBoxColorPlaneExtraction,
            this.toolStripLabel3,
            this.toolStripComboBoxLookupTable,
            this.toolStripBtnFindCircle,
            this.toolStripCmbRadiusRange,
            this.toolStripBtnStopAutoGetRrays,
            this.toolStripBtnOpenControllerDebuger,
            this.toolStripBtnGetRrays,
            this.toolStripCmbRrayPercent,
            this.toolStripButtonDeleteRect});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip2.Size = new System.Drawing.Size(878, 35);
            this.toolStrip2.TabIndex = 6;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripCmbImgList
            // 
            this.toolStripCmbImgList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripCmbImgList.Name = "toolStripCmbImgList";
            this.toolStripCmbImgList.Size = new System.Drawing.Size(125, 35);
            // 
            // toolStripBtnOpenPics
            // 
            this.toolStripBtnOpenPics.Image = global::CheckSystem.Properties.Resources.TSB_Save_Image;
            this.toolStripBtnOpenPics.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnOpenPics.Name = "toolStripBtnOpenPics";
            this.toolStripBtnOpenPics.Size = new System.Drawing.Size(76, 32);
            this.toolStripBtnOpenPics.Text = "选取图片";
            this.toolStripBtnOpenPics.Click += new System.EventHandler(this.toolStripBtnOpenPics_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(56, 32);
            this.toolStripLabel2.Text = "色素提取";
            // 
            // toolStripComboBoxColorPlaneExtraction
            // 
            this.toolStripComboBoxColorPlaneExtraction.Name = "toolStripComboBoxColorPlaneExtraction";
            this.toolStripComboBoxColorPlaneExtraction.Size = new System.Drawing.Size(75, 35);
            this.toolStripComboBoxColorPlaneExtraction.ToolTipText = "色素提取";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(56, 32);
            this.toolStripLabel3.Text = "增强对比";
            // 
            // toolStripComboBoxLookupTable
            // 
            this.toolStripComboBoxLookupTable.Name = "toolStripComboBoxLookupTable";
            this.toolStripComboBoxLookupTable.Size = new System.Drawing.Size(95, 35);
            // 
            // toolStripBtnFindCircle
            // 
            this.toolStripBtnFindCircle.Image = global::CheckSystem.Properties.Resources.TSB_ImageFit_Image;
            this.toolStripBtnFindCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnFindCircle.Name = "toolStripBtnFindCircle";
            this.toolStripBtnFindCircle.Size = new System.Drawing.Size(76, 32);
            this.toolStripBtnFindCircle.Text = "寻找亮斑";
            this.toolStripBtnFindCircle.Click += new System.EventHandler(this.toolStripBtnFindCircle_Click);
            // 
            // toolStripCmbRadiusRange
            // 
            this.toolStripCmbRadiusRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripCmbRadiusRange.Name = "toolStripCmbRadiusRange";
            this.toolStripCmbRadiusRange.Size = new System.Drawing.Size(75, 35);
            // 
            // toolStripBtnStopAutoGetRrays
            // 
            this.toolStripBtnStopAutoGetRrays.Image = global::CheckSystem.Properties.Resources.Btn_GraySet_BackgroundImage;
            this.toolStripBtnStopAutoGetRrays.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnStopAutoGetRrays.Name = "toolStripBtnStopAutoGetRrays";
            this.toolStripBtnStopAutoGetRrays.Size = new System.Drawing.Size(165, 21);
            this.toolStripBtnStopAutoGetRrays.Text = "打开/关闭自动刷新灰度值";
            this.toolStripBtnStopAutoGetRrays.Click += new System.EventHandler(this.toolStripBtnStopAutoGetRrays_Click);
            // 
            // toolStripBtnOpenControllerDebuger
            // 
            this.toolStripBtnOpenControllerDebuger.Image = global::CheckSystem.Properties.Resources.TSB_AddRectangle_Image;
            this.toolStripBtnOpenControllerDebuger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnOpenControllerDebuger.Name = "toolStripBtnOpenControllerDebuger";
            this.toolStripBtnOpenControllerDebuger.Size = new System.Drawing.Size(136, 21);
            this.toolStripBtnOpenControllerDebuger.Text = "打开控制器调试界面";
            this.toolStripBtnOpenControllerDebuger.Click += new System.EventHandler(this.toolStripBtnOpenControllerDebuger_Click);
            // 
            // toolStripBtnGetRrays
            // 
            this.toolStripBtnGetRrays.Image = global::CheckSystem.Properties.Resources.Btn_GraySet_BackgroundImage;
            this.toolStripBtnGetRrays.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnGetRrays.Name = "toolStripBtnGetRrays";
            this.toolStripBtnGetRrays.Size = new System.Drawing.Size(88, 21);
            this.toolStripBtnGetRrays.Text = "刷新灰度值";
            this.toolStripBtnGetRrays.Click += new System.EventHandler(this.toolStripBtnShowGray_Click);
            // 
            // toolStripCmbRrayPercent
            // 
            this.toolStripCmbRrayPercent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripCmbRrayPercent.Name = "toolStripCmbRrayPercent";
            this.toolStripCmbRrayPercent.Size = new System.Drawing.Size(75, 25);
            // 
            // toolStripButtonDeleteRect
            // 
            this.toolStripButtonDeleteRect.Image = global::CheckSystem.Properties.Resources.TSB_ClearShape_Image;
            this.toolStripButtonDeleteRect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteRect.Name = "toolStripButtonDeleteRect";
            this.toolStripButtonDeleteRect.Size = new System.Drawing.Size(52, 21);
            this.toolStripButtonDeleteRect.Text = "删除";
            this.toolStripButtonDeleteRect.Click += new System.EventHandler(this.toolStripButtonDeleteRect_Click);
            // 
            // toolStripButtonAddRect
            // 
            this.toolStripButtonAddRect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonAddRect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddRect.Image")));
            this.toolStripButtonAddRect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddRect.Name = "toolStripButtonAddRect";
            this.toolStripButtonAddRect.Size = new System.Drawing.Size(60, 32);
            this.toolStripButtonAddRect.Text = "添加矩形";
            this.toolStripButtonAddRect.Click += new System.EventHandler(this.toolStripButtonAddRect_Click);
            // 
            // FrmAlcmLedParaSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 661);
            this.Controls.Add(this.mainTable);
            this.MaximizeBox = false;
            this.Name = "FrmAlcmLedParaSet";
            this.Text = "FormLedParaSetMultiplePic";
            this.cameraParaGroup.ResumeLayout(false);
            this.cameraPanel.ResumeLayout(false);
            this.mainTable.ResumeLayout(false);
            this.leftMainPanel.ResumeLayout(false);
            this.stepPanel.ResumeLayout(false);
            this.imgViewerPanel.ResumeLayout(false);
            this.imgViewerPanel.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.UserDataGrid userDataGrid;
        private System.Windows.Forms.GroupBox cameraParaGroup;
        private System.Windows.Forms.TableLayoutPanel cameraPanel;
        private UserControls.LabelText funcNameTxt;
        private UserControls.LabelCombox cameraFrameCountTxt;
        private UserControls.LabelCombox cameraFrameRateTxt;
        private UserControls.LabelCombox cameraShutterTxt;
        private System.Windows.Forms.TableLayoutPanel mainTable;
        private System.Windows.Forms.TableLayoutPanel leftMainPanel;
        private System.Windows.Forms.TableLayoutPanel stepPanel;
        private UserControls.LabelCombox lblWithCmbStepsList;
        private System.Windows.Forms.Button btnSaveParas;
        private System.Windows.Forms.TableLayoutPanel imgViewerPanel;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripComboBox toolStripCmbImgList;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxColorPlaneExtraction;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxLookupTable;
        private System.Windows.Forms.Button btnNextStep;
        private System.Windows.Forms.Button btnLastStep;
        private NationalInstruments.Vision.WindowsForms.ImageViewer ImageViewer;
        private System.Windows.Forms.ToolStripButton toolStripBtnFindCircle;
        private System.Windows.Forms.ToolStripButton toolStripBtnOpenPics;
        private UserControls.LabelCombox cameraDelayTxt;
        private UserControls.LabelCombox cameraSnTxt;
        private System.Windows.Forms.ToolStripButton toolStripBtnOpenControllerDebuger;
        private System.Windows.Forms.ToolStripButton toolStripBtnGetRrays;
        private System.Windows.Forms.ToolStripComboBox toolStripCmbRadiusRange;
        private System.Windows.Forms.ToolStripComboBox toolStripCmbRrayPercent;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteRect;
        private System.Windows.Forms.ToolStripButton toolStripBtnStopAutoGetRrays;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddRect;
    }
}