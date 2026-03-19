namespace DeviceDesign
{
    partial class FormStatusMachineDesign
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStatusMachineDesign));
            this.toolStripSmDesign = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxWorkstation = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButtonAddStatusUnit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAddCondition = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonStatusNote = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonConditionNote = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSaveImage = new System.Windows.Forms.ToolStripButton();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStripSmDesign.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSmDesign
            // 
            this.toolStripSmDesign.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripSmDesign.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripComboBoxWorkstation,
            this.toolStripButtonAddStatusUnit,
            this.toolStripButtonAddCondition,
            this.toolStripSeparator3,
            this.toolStripButtonDelete,
            this.toolStripSeparator1,
            this.toolStripButtonZoomIn,
            this.toolStripButtonZoomOut,
            this.toolStripSeparator2,
            this.toolStripButtonSave,
            this.toolStripSeparator4,
            this.toolStripButtonStatusNote,
            this.toolStripButtonConditionNote,
            this.toolStripSeparator5,
            this.toolStripButtonSaveImage});
            this.toolStripSmDesign.Location = new System.Drawing.Point(0, 0);
            this.toolStripSmDesign.Name = "toolStripSmDesign";
            this.toolStripSmDesign.Size = new System.Drawing.Size(1217, 29);
            this.toolStripSmDesign.TabIndex = 0;
            this.toolStripSmDesign.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(90, 26);
            this.toolStripLabel1.Text = "选择工作站";
            // 
            // toolStripComboBoxWorkstation
            // 
            this.toolStripComboBoxWorkstation.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripComboBoxWorkstation.Name = "toolStripComboBoxWorkstation";
            this.toolStripComboBoxWorkstation.Size = new System.Drawing.Size(200, 29);
            // 
            // toolStripButtonAddStatusUnit
            // 
            this.toolStripButtonAddStatusUnit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonAddStatusUnit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddStatusUnit.Image")));
            this.toolStripButtonAddStatusUnit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddStatusUnit.Name = "toolStripButtonAddStatusUnit";
            this.toolStripButtonAddStatusUnit.Size = new System.Drawing.Size(110, 26);
            this.toolStripButtonAddStatusUnit.Text = "添加状态单元";
            this.toolStripButtonAddStatusUnit.Click += new System.EventHandler(this.toolStripButtonAddStatusUnit_Click);
            // 
            // toolStripButtonAddCondition
            // 
            this.toolStripButtonAddCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonAddCondition.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddCondition.Image")));
            this.toolStripButtonAddCondition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddCondition.Name = "toolStripButtonAddCondition";
            this.toolStripButtonAddCondition.Size = new System.Drawing.Size(110, 26);
            this.toolStripButtonAddCondition.Text = "添加状态条件";
            this.toolStripButtonAddCondition.Click += new System.EventHandler(this.toolStripButtonAddCondition_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelete.Image")));
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(78, 26);
            this.toolStripButtonDelete.Text = "删除选中";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripButtonZoomIn
            // 
            this.toolStripButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomIn.Image")));
            this.toolStripButtonZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomIn.Name = "toolStripButtonZoomIn";
            this.toolStripButtonZoomIn.Size = new System.Drawing.Size(46, 26);
            this.toolStripButtonZoomIn.Text = "放大";
            this.toolStripButtonZoomIn.Click += new System.EventHandler(this.toolStripButtonZoomIn_Click);
            // 
            // toolStripButtonZoomOut
            // 
            this.toolStripButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomOut.Image")));
            this.toolStripButtonZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomOut.Name = "toolStripButtonZoomOut";
            this.toolStripButtonZoomOut.Size = new System.Drawing.Size(46, 26);
            this.toolStripButtonZoomOut.Text = "缩小";
            this.toolStripButtonZoomOut.Click += new System.EventHandler(this.toolStripButtonZoomOut_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(46, 26);
            this.toolStripButtonSave.Text = "保存";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripButtonStatusNote
            // 
            this.toolStripButtonStatusNote.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonStatusNote.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStatusNote.Image")));
            this.toolStripButtonStatusNote.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStatusNote.Name = "toolStripButtonStatusNote";
            this.toolStripButtonStatusNote.Size = new System.Drawing.Size(78, 26);
            this.toolStripButtonStatusNote.Text = "显示状态";
            this.toolStripButtonStatusNote.Click += new System.EventHandler(this.toolStripButtonStatusNote_Click);
            // 
            // toolStripButtonConditionNote
            // 
            this.toolStripButtonConditionNote.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonConditionNote.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonConditionNote.Image")));
            this.toolStripButtonConditionNote.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonConditionNote.Name = "toolStripButtonConditionNote";
            this.toolStripButtonConditionNote.Size = new System.Drawing.Size(78, 26);
            this.toolStripButtonConditionNote.Text = "显示条件";
            this.toolStripButtonConditionNote.Click += new System.EventHandler(this.toolStripButtonConditionNote_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripButtonSaveImage
            // 
            this.toolStripButtonSaveImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSaveImage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveImage.Image")));
            this.toolStripButtonSaveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveImage.Name = "toolStripButtonSaveImage";
            this.toolStripButtonSaveImage.Size = new System.Drawing.Size(78, 26);
            this.toolStripButtonSaveImage.Text = "保存图片";
            this.toolStripButtonSaveImage.Click += new System.EventHandler(this.toolStripButtonSaveImage_Click);
            // 
            // panelMain
            // 
            this.panelMain.AutoScroll = true;
            this.panelMain.BackColor = System.Drawing.SystemColors.Info;
            this.panelMain.Location = new System.Drawing.Point(3, 3);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1042, 701);
            this.panelMain.TabIndex = 1;
            this.panelMain.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMain_Paint);
            this.panelMain.DoubleClick += new System.EventHandler(this.panelMain_DoubleClick);
            this.panelMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelMain_MouseDown);
            this.panelMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelMain_MouseMove);
            this.panelMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelMain_MouseUp);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.panelMain);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1217, 728);
            this.panel1.TabIndex = 2;
            // 
            // FormStatusMachineDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1217, 757);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStripSmDesign);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormStatusMachineDesign";
            this.Text = "FormStatusMachineDesign";
            this.Load += new System.EventHandler(this.FormStatusMachineDesign_Load);
            this.SizeChanged += new System.EventHandler(this.FormStatusMachineDesign_SizeChanged);
            this.toolStripSmDesign.ResumeLayout(false);
            this.toolStripSmDesign.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripSmDesign;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxWorkstation;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddStatusUnit;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddCondition;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomOut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButtonStatusNote;
        private System.Windows.Forms.ToolStripButton toolStripButtonConditionNote;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveImage;
    }
}