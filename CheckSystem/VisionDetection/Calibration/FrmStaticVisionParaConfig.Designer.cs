namespace CheckSystem.VisionDetection.Calibration
{
    sealed partial class FrmStaticVisionParaConfig
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.stepBar = new Sunny.UI.UIBreadcrumb();
            this.uiSplitContainerMain = new Sunny.UI.UISplitContainer();
            this.uiTableLayoutPanel1 = new Sunny.UI.UITableLayoutPanel();
            this.treeViewCamerasList = new Sunny.UI.UITreeView();
            this.cameraGroupBox = new Sunny.UI.UIGroupBox();
            this.tablePanelCameraControl = new Sunny.UI.UITableLayoutPanel();
            this.txtDelay = new Sunny.UI.UIIntegerUpDown();
            this.uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            this.btnLoadImg = new Sunny.UI.UIButton();
            this.txtShutter = new Sunny.UI.UIIntegerUpDown();
            this.btnStopGrab = new Sunny.UI.UIButton();
            this.btnSaveImg = new Sunny.UI.UIButton();
            this.btnSnap = new Sunny.UI.UIButton();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.btnStartGrab = new Sunny.UI.UIButton();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.lblCameraName = new Sunny.UI.UILabel();
            this.visionGroupBox = new Sunny.UI.UIGroupBox();
            this.btnAddFromCopy = new Sunny.UI.UIButton();
            this.btnClearRoi = new Sunny.UI.UIButton();
            this.btnRefreshGray = new Sunny.UI.UIButton();
            this.btnFindCircle = new Sunny.UI.UIButton();
            this.btnAddShape = new Sunny.UI.UIButton();
            this.btnAddRect = new Sunny.UI.UIButton();
            this.btnAddRoi = new Sunny.UI.UIButton();
            this.cmbLookupTable = new Sunny.UI.UIComboBox();
            this.uiSplitContainerCalibration = new Sunny.UI.UISplitContainer();
            this.panelGrays = new Sunny.UI.UITitlePanel();
            this.dgvGrays = new Sunny.UI.UIDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainerMain)).BeginInit();
            this.uiSplitContainerMain.Panel1.SuspendLayout();
            this.uiSplitContainerMain.Panel2.SuspendLayout();
            this.uiSplitContainerMain.SuspendLayout();
            this.uiTableLayoutPanel1.SuspendLayout();
            this.cameraGroupBox.SuspendLayout();
            this.tablePanelCameraControl.SuspendLayout();
            this.visionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainerCalibration)).BeginInit();
            this.uiSplitContainerCalibration.Panel2.SuspendLayout();
            this.uiSplitContainerCalibration.SuspendLayout();
            this.panelGrays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrays)).BeginInit();
            this.SuspendLayout();
            // 
            // stepBar
            // 
            this.stepBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.stepBar.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.stepBar.ItemIndex = 0;
            this.stepBar.ItemWidth = 181;
            this.stepBar.Location = new System.Drawing.Point(0, 35);
            this.stepBar.Margin = new System.Windows.Forms.Padding(10);
            this.stepBar.MinimumSize = new System.Drawing.Size(1, 1);
            this.stepBar.Name = "stepBar";
            this.stepBar.Size = new System.Drawing.Size(1536, 35);
            this.stepBar.TabIndex = 95;
            this.stepBar.Text = "uiBreadcrumb1";
            this.stepBar.UnSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            // 
            // uiSplitContainerMain
            // 
            this.uiSplitContainerMain.Cursor = System.Windows.Forms.Cursors.Default;
            this.uiSplitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSplitContainerMain.Location = new System.Drawing.Point(0, 70);
            this.uiSplitContainerMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.uiSplitContainerMain.Name = "uiSplitContainerMain";
            // 
            // uiSplitContainerMain.Panel1
            // 
            this.uiSplitContainerMain.Panel1.Controls.Add(this.uiTableLayoutPanel1);
            // 
            // uiSplitContainerMain.Panel2
            // 
            this.uiSplitContainerMain.Panel2.Controls.Add(this.uiSplitContainerCalibration);
            this.uiSplitContainerMain.Size = new System.Drawing.Size(1536, 698);
            this.uiSplitContainerMain.SplitterDistance = 411;
            this.uiSplitContainerMain.SplitterWidth = 11;
            this.uiSplitContainerMain.TabIndex = 96;
            // 
            // uiTableLayoutPanel1
            // 
            this.uiTableLayoutPanel1.ColumnCount = 1;
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uiTableLayoutPanel1.Controls.Add(this.treeViewCamerasList, 0, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.cameraGroupBox, 0, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.visionGroupBox, 0, 2);
            this.uiTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiTableLayoutPanel1.Name = "uiTableLayoutPanel1";
            this.uiTableLayoutPanel1.RowCount = 3;
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTableLayoutPanel1.Size = new System.Drawing.Size(411, 698);
            this.uiTableLayoutPanel1.TabIndex = 0;
            this.uiTableLayoutPanel1.TagString = null;
            // 
            // treeViewCamerasList
            // 
            this.treeViewCamerasList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCamerasList.FillColor = System.Drawing.Color.White;
            this.treeViewCamerasList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewCamerasList.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.treeViewCamerasList.Location = new System.Drawing.Point(4, 5);
            this.treeViewCamerasList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.treeViewCamerasList.MinimumSize = new System.Drawing.Size(1, 1);
            this.treeViewCamerasList.Name = "treeViewCamerasList";
            this.treeViewCamerasList.ScrollBarStyleInherited = false;
            this.treeViewCamerasList.ShowText = false;
            this.treeViewCamerasList.Size = new System.Drawing.Size(403, 190);
            this.treeViewCamerasList.TabIndex = 1;
            this.treeViewCamerasList.Text = "uiTreeView1";
            this.treeViewCamerasList.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cameraGroupBox
            // 
            this.cameraGroupBox.Controls.Add(this.tablePanelCameraControl);
            this.cameraGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraGroupBox.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cameraGroupBox.Location = new System.Drawing.Point(4, 205);
            this.cameraGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cameraGroupBox.MinimumSize = new System.Drawing.Size(1, 1);
            this.cameraGroupBox.Name = "cameraGroupBox";
            this.cameraGroupBox.Padding = new System.Windows.Forms.Padding(10, 32, 10, 10);
            this.cameraGroupBox.Size = new System.Drawing.Size(403, 239);
            this.cameraGroupBox.TabIndex = 2;
            this.cameraGroupBox.Text = "相机控制：";
            this.cameraGroupBox.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tablePanelCameraControl
            // 
            this.tablePanelCameraControl.ColumnCount = 3;
            this.tablePanelCameraControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tablePanelCameraControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tablePanelCameraControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tablePanelCameraControl.Controls.Add(this.txtDelay, 1, 2);
            this.tablePanelCameraControl.Controls.Add(this.uiMarkLabel3, 0, 2);
            this.tablePanelCameraControl.Controls.Add(this.btnLoadImg, 2, 3);
            this.tablePanelCameraControl.Controls.Add(this.txtShutter, 1, 1);
            this.tablePanelCameraControl.Controls.Add(this.btnStopGrab, 0, 4);
            this.tablePanelCameraControl.Controls.Add(this.btnSaveImg, 1, 4);
            this.tablePanelCameraControl.Controls.Add(this.btnSnap, 1, 3);
            this.tablePanelCameraControl.Controls.Add(this.uiMarkLabel2, 0, 1);
            this.tablePanelCameraControl.Controls.Add(this.btnStartGrab, 0, 3);
            this.tablePanelCameraControl.Controls.Add(this.uiMarkLabel1, 0, 0);
            this.tablePanelCameraControl.Controls.Add(this.lblCameraName, 1, 0);
            this.tablePanelCameraControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanelCameraControl.Location = new System.Drawing.Point(10, 32);
            this.tablePanelCameraControl.Margin = new System.Windows.Forms.Padding(10);
            this.tablePanelCameraControl.Name = "tablePanelCameraControl";
            this.tablePanelCameraControl.RowCount = 5;
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelCameraControl.Size = new System.Drawing.Size(383, 197);
            this.tablePanelCameraControl.TabIndex = 0;
            this.tablePanelCameraControl.TagString = null;
            // 
            // txtDelay
            // 
            this.tablePanelCameraControl.SetColumnSpan(this.txtDelay, 2);
            this.txtDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDelay.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtDelay.Location = new System.Drawing.Point(131, 83);
            this.txtDelay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDelay.Maximum = 1000000;
            this.txtDelay.Minimum = 0;
            this.txtDelay.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtDelay.Name = "txtDelay";
            this.txtDelay.ShowText = false;
            this.txtDelay.Size = new System.Drawing.Size(248, 29);
            this.txtDelay.Step = 100;
            this.txtDelay.TabIndex = 126;
            this.txtDelay.Text = null;
            this.txtDelay.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtDelay.Value = 10000;
            // 
            // uiMarkLabel3
            // 
            this.uiMarkLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel3.Location = new System.Drawing.Point(10, 88);
            this.uiMarkLabel3.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel3.Name = "uiMarkLabel3";
            this.uiMarkLabel3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel3.Size = new System.Drawing.Size(107, 19);
            this.uiMarkLabel3.TabIndex = 125;
            this.uiMarkLabel3.Text = "点亮时间：";
            this.uiMarkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLoadImg
            // 
            this.btnLoadImg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoadImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadImg.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadImg.Location = new System.Drawing.Point(257, 120);
            this.btnLoadImg.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLoadImg.Name = "btnLoadImg";
            this.btnLoadImg.Size = new System.Drawing.Size(123, 33);
            this.btnLoadImg.TabIndex = 123;
            this.btnLoadImg.Text = "选取本地图片";
            this.btnLoadImg.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadImg.Click += new System.EventHandler(this.btnLoadImg_Click);
            // 
            // txtShutter
            // 
            this.tablePanelCameraControl.SetColumnSpan(this.txtShutter, 2);
            this.txtShutter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtShutter.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtShutter.Location = new System.Drawing.Point(131, 44);
            this.txtShutter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtShutter.Maximum = 1000000;
            this.txtShutter.Minimum = 0;
            this.txtShutter.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtShutter.Name = "txtShutter";
            this.txtShutter.ShowText = false;
            this.txtShutter.Size = new System.Drawing.Size(248, 29);
            this.txtShutter.Step = 100;
            this.txtShutter.TabIndex = 122;
            this.txtShutter.Text = null;
            this.txtShutter.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtShutter.Value = 10000;
            // 
            // btnStopGrab
            // 
            this.btnStopGrab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStopGrab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStopGrab.Enabled = false;
            this.btnStopGrab.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStopGrab.Location = new System.Drawing.Point(3, 159);
            this.btnStopGrab.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnStopGrab.Name = "btnStopGrab";
            this.btnStopGrab.Size = new System.Drawing.Size(121, 35);
            this.btnStopGrab.TabIndex = 5;
            this.btnStopGrab.Text = "停止采集";
            this.btnStopGrab.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStopGrab.Click += new System.EventHandler(this.btnStopGrab_Click);
            // 
            // btnSaveImg
            // 
            this.btnSaveImg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveImg.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveImg.Location = new System.Drawing.Point(130, 159);
            this.btnSaveImg.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSaveImg.Name = "btnSaveImg";
            this.btnSaveImg.Size = new System.Drawing.Size(121, 35);
            this.btnSaveImg.TabIndex = 4;
            this.btnSaveImg.Text = "保存";
            this.btnSaveImg.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveImg.Click += new System.EventHandler(this.btnSaveImg_Click);
            // 
            // btnSnap
            // 
            this.btnSnap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSnap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSnap.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSnap.Location = new System.Drawing.Point(130, 120);
            this.btnSnap.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Size = new System.Drawing.Size(121, 33);
            this.btnSnap.TabIndex = 3;
            this.btnSnap.Text = "点亮并抓拍";
            this.btnSnap.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel2.Location = new System.Drawing.Point(10, 49);
            this.uiMarkLabel2.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel2.Size = new System.Drawing.Size(107, 19);
            this.uiMarkLabel2.TabIndex = 1;
            this.uiMarkLabel2.Text = "曝光值：";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStartGrab
            // 
            this.btnStartGrab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartGrab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStartGrab.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStartGrab.Location = new System.Drawing.Point(3, 120);
            this.btnStartGrab.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnStartGrab.Name = "btnStartGrab";
            this.btnStartGrab.Size = new System.Drawing.Size(121, 33);
            this.btnStartGrab.TabIndex = 2;
            this.btnStartGrab.Text = "开始采集";
            this.btnStartGrab.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStartGrab.Click += new System.EventHandler(this.btnStartGrab_Click);
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel1.Location = new System.Drawing.Point(10, 10);
            this.uiMarkLabel1.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(107, 19);
            this.uiMarkLabel1.TabIndex = 0;
            this.uiMarkLabel1.Text = "当前选择相机：";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCameraName
            // 
            this.lblCameraName.BackColor = System.Drawing.Color.PapayaWhip;
            this.tablePanelCameraControl.SetColumnSpan(this.lblCameraName, 2);
            this.lblCameraName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCameraName.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCameraName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblCameraName.Location = new System.Drawing.Point(128, 1);
            this.lblCameraName.Margin = new System.Windows.Forms.Padding(1);
            this.lblCameraName.Name = "lblCameraName";
            this.lblCameraName.Size = new System.Drawing.Size(254, 37);
            this.lblCameraName.TabIndex = 7;
            this.lblCameraName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // visionGroupBox
            // 
            this.visionGroupBox.Controls.Add(this.btnAddFromCopy);
            this.visionGroupBox.Controls.Add(this.btnClearRoi);
            this.visionGroupBox.Controls.Add(this.btnRefreshGray);
            this.visionGroupBox.Controls.Add(this.btnFindCircle);
            this.visionGroupBox.Controls.Add(this.btnAddShape);
            this.visionGroupBox.Controls.Add(this.btnAddRect);
            this.visionGroupBox.Controls.Add(this.btnAddRoi);
            this.visionGroupBox.Controls.Add(this.cmbLookupTable);
            this.visionGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionGroupBox.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.visionGroupBox.Location = new System.Drawing.Point(4, 454);
            this.visionGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.visionGroupBox.MinimumSize = new System.Drawing.Size(1, 1);
            this.visionGroupBox.Name = "visionGroupBox";
            this.visionGroupBox.Padding = new System.Windows.Forms.Padding(10, 32, 10, 10);
            this.visionGroupBox.Size = new System.Drawing.Size(403, 239);
            this.visionGroupBox.TabIndex = 3;
            this.visionGroupBox.Text = "图形设置：";
            this.visionGroupBox.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAddFromCopy
            // 
            this.btnAddFromCopy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddFromCopy.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddFromCopy.Location = new System.Drawing.Point(13, 149);
            this.btnAddFromCopy.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAddFromCopy.Name = "btnAddFromCopy";
            this.btnAddFromCopy.Size = new System.Drawing.Size(147, 35);
            this.btnAddFromCopy.TabIndex = 1;
            this.btnAddFromCopy.Text = "从其它光型中复制";
            this.btnAddFromCopy.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddFromCopy.Click += new System.EventHandler(this.btnAddFromCopy_Click);
            // 
            // btnClearRoi
            // 
            this.btnClearRoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearRoi.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClearRoi.Location = new System.Drawing.Point(166, 190);
            this.btnClearRoi.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnClearRoi.Name = "btnClearRoi";
            this.btnClearRoi.Size = new System.Drawing.Size(100, 35);
            this.btnClearRoi.TabIndex = 1;
            this.btnClearRoi.Text = "清除所有";
            this.btnClearRoi.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClearRoi.Click += new System.EventHandler(this.btnClearRoi_Click);
            // 
            // btnRefreshGray
            // 
            this.btnRefreshGray.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshGray.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRefreshGray.Location = new System.Drawing.Point(166, 149);
            this.btnRefreshGray.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRefreshGray.Name = "btnRefreshGray";
            this.btnRefreshGray.Size = new System.Drawing.Size(100, 35);
            this.btnRefreshGray.TabIndex = 1;
            this.btnRefreshGray.Text = "刷新灰度值";
            this.btnRefreshGray.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRefreshGray.Click += new System.EventHandler(this.btnRefreshGray_Click);
            // 
            // btnFindCircle
            // 
            this.btnFindCircle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFindCircle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFindCircle.Location = new System.Drawing.Point(166, 108);
            this.btnFindCircle.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnFindCircle.Name = "btnFindCircle";
            this.btnFindCircle.Size = new System.Drawing.Size(100, 35);
            this.btnFindCircle.TabIndex = 1;
            this.btnFindCircle.Text = "识别LED";
            this.btnFindCircle.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFindCircle.Click += new System.EventHandler(this.tsBtnFindCircle_Click);
            // 
            // btnAddShape
            // 
            this.btnAddShape.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddShape.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddShape.Location = new System.Drawing.Point(13, 108);
            this.btnAddShape.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAddShape.Name = "btnAddShape";
            this.btnAddShape.Size = new System.Drawing.Size(100, 35);
            this.btnAddShape.TabIndex = 1;
            this.btnAddShape.Text = "添加多边形";
            this.btnAddShape.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddShape.Click += new System.EventHandler(this.btnAddShape_Click);
            // 
            // btnAddRect
            // 
            this.btnAddRect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRect.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddRect.Location = new System.Drawing.Point(166, 67);
            this.btnAddRect.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAddRect.Name = "btnAddRect";
            this.btnAddRect.Size = new System.Drawing.Size(100, 35);
            this.btnAddRect.TabIndex = 1;
            this.btnAddRect.Text = "添加矩形";
            this.btnAddRect.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddRect.Click += new System.EventHandler(this.btnAddRect_Click);
            // 
            // btnAddRoi
            // 
            this.btnAddRoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRoi.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddRoi.Location = new System.Drawing.Point(13, 67);
            this.btnAddRoi.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAddRoi.Name = "btnAddRoi";
            this.btnAddRoi.Size = new System.Drawing.Size(120, 35);
            this.btnAddRoi.TabIndex = 1;
            this.btnAddRoi.Text = "添加识别区域";
            this.btnAddRoi.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddRoi.Click += new System.EventHandler(this.tsBtnAddRoi_Click);
            // 
            // cmbLookupTable
            // 
            this.cmbLookupTable.DataSource = null;
            this.cmbLookupTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbLookupTable.FillColor = System.Drawing.Color.White;
            this.cmbLookupTable.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbLookupTable.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbLookupTable.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbLookupTable.Location = new System.Drawing.Point(10, 32);
            this.cmbLookupTable.Margin = new System.Windows.Forms.Padding(4, 15, 4, 15);
            this.cmbLookupTable.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbLookupTable.Name = "cmbLookupTable";
            this.cmbLookupTable.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbLookupTable.Size = new System.Drawing.Size(383, 29);
            this.cmbLookupTable.SymbolSize = 24;
            this.cmbLookupTable.TabIndex = 0;
            this.cmbLookupTable.Text = "uiComboBox1";
            this.cmbLookupTable.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbLookupTable.Watermark = "";
            // 
            // uiSplitContainerCalibration
            // 
            this.uiSplitContainerCalibration.CollapsePanel = Sunny.UI.UISplitContainer.UICollapsePanel.Panel2;
            this.uiSplitContainerCalibration.Cursor = System.Windows.Forms.Cursors.Default;
            this.uiSplitContainerCalibration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSplitContainerCalibration.Location = new System.Drawing.Point(0, 0);
            this.uiSplitContainerCalibration.MinimumSize = new System.Drawing.Size(20, 20);
            this.uiSplitContainerCalibration.Name = "uiSplitContainerCalibration";
            // 
            // uiSplitContainerCalibration.Panel1
            // 
            this.uiSplitContainerCalibration.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // uiSplitContainerCalibration.Panel2
            // 
            this.uiSplitContainerCalibration.Panel2.Controls.Add(this.panelGrays);
            this.uiSplitContainerCalibration.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.uiSplitContainerCalibration.Size = new System.Drawing.Size(1114, 698);
            this.uiSplitContainerCalibration.SplitterDistance = 795;
            this.uiSplitContainerCalibration.SplitterWidth = 11;
            this.uiSplitContainerCalibration.TabIndex = 11;
            // 
            // panelGrays
            // 
            this.panelGrays.Controls.Add(this.dgvGrays);
            this.panelGrays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrays.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panelGrays.Location = new System.Drawing.Point(0, 0);
            this.panelGrays.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelGrays.MinimumSize = new System.Drawing.Size(1, 1);
            this.panelGrays.Name = "panelGrays";
            this.panelGrays.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.panelGrays.ShowText = false;
            this.panelGrays.Size = new System.Drawing.Size(308, 698);
            this.panelGrays.TabIndex = 0;
            this.panelGrays.Text = "灰度值列表";
            this.panelGrays.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvGrays
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.dgvGrays.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGrays.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.dgvGrays.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGrays.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGrays.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGrays.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvGrays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGrays.EnableHeadersVisualStyles = false;
            this.dgvGrays.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvGrays.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(173)))), ((int)(((byte)(255)))));
            this.dgvGrays.Location = new System.Drawing.Point(0, 35);
            this.dgvGrays.Name = "dgvGrays";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGrays.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.dgvGrays.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvGrays.RowTemplate.Height = 23;
            this.dgvGrays.SelectedIndex = -1;
            this.dgvGrays.Size = new System.Drawing.Size(308, 663);
            this.dgvGrays.TabIndex = 0;
            // 
            // FrmStaticVisionParaConfig
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1536, 768);
            this.Controls.Add(this.uiSplitContainerMain);
            this.Controls.Add(this.stepBar);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1600, 768);
            this.MinimumSize = new System.Drawing.Size(1534, 768);
            this.Name = "FrmStaticVisionParaConfig";
            this.Text = "静态图像检测-参数设置";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 480);
            this.uiSplitContainerMain.Panel1.ResumeLayout(false);
            this.uiSplitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainerMain)).EndInit();
            this.uiSplitContainerMain.ResumeLayout(false);
            this.uiTableLayoutPanel1.ResumeLayout(false);
            this.cameraGroupBox.ResumeLayout(false);
            this.tablePanelCameraControl.ResumeLayout(false);
            this.visionGroupBox.ResumeLayout(false);
            this.uiSplitContainerCalibration.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainerCalibration)).EndInit();
            this.uiSplitContainerCalibration.ResumeLayout(false);
            this.panelGrays.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrays)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIBreadcrumb stepBar;
        private Sunny.UI.UISplitContainer uiSplitContainerMain;
        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel1;
        private Sunny.UI.UITreeView treeViewCamerasList;
        private Sunny.UI.UIGroupBox cameraGroupBox;
        private Sunny.UI.UITableLayoutPanel tablePanelCameraControl;
        private Sunny.UI.UIButton btnStopGrab;
        private Sunny.UI.UIButton btnSaveImg;
        private Sunny.UI.UIButton btnSnap;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UIButton btnStartGrab;
        private Sunny.UI.UIGroupBox visionGroupBox;
        private Sunny.UI.UILabel lblCameraName;
        private Sunny.UI.UIIntegerUpDown txtShutter;
        private Sunny.UI.UIButton btnLoadImg;
        private Sunny.UI.UISplitContainer uiSplitContainerCalibration;
        private Sunny.UI.UIComboBox cmbLookupTable;
        private Sunny.UI.UIButton btnClearRoi;
        private Sunny.UI.UIButton btnRefreshGray;
        private Sunny.UI.UIButton btnFindCircle;
        private Sunny.UI.UIButton btnAddShape;
        private Sunny.UI.UIButton btnAddRect;
        private Sunny.UI.UIButton btnAddRoi;
        private Sunny.UI.UITitlePanel panelGrays;
        private Sunny.UI.UIDataGridView dgvGrays;
        private Sunny.UI.UIButton btnAddFromCopy;
        private Sunny.UI.UIIntegerUpDown txtDelay;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
    }
}