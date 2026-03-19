namespace CheckSystem.CcdForms
{
    partial class FormCcdSetup
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
            this.lblCmbPowerPort = new UserControls.LabelCombox();
            this.lblCmbBarcodeScanerPort = new UserControls.LabelCombox();
            this.lblCmbIsLAndR = new UserControls.LabelCombox();
            this.lblTxtProductNo = new UserControls.LabelText();
            this.lblTxtProductName = new UserControls.LabelText();
            this.lblTxtDeviceNo = new UserControls.LabelText();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加检测项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加二维码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblCmbIsStamp = new UserControls.LabelCombox();
            this.lblCmbIsReadButton = new UserControls.LabelCombox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvCheckItems = new UserControls.UserDataGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.dgvBarocodeList = new UserControls.UserDataGrid();
            this.contextMenuStripTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStripTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCmbPowerPort
            // 
            this.lblCmbPowerPort.BackColor = System.Drawing.Color.Wheat;
            this.lblCmbPowerPort.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCmbPowerPort.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCmbPowerPort.LabelString = "精密电源端口";
            this.lblCmbPowerPort.Location = new System.Drawing.Point(0, 215);
            this.lblCmbPowerPort.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbPowerPort.Name = "lblCmbPowerPort";
            this.lblCmbPowerPort.Size = new System.Drawing.Size(1215, 37);
            this.lblCmbPowerPort.TabIndex = 5;
            // 
            // lblCmbBarcodeScanerPort
            // 
            this.lblCmbBarcodeScanerPort.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblCmbBarcodeScanerPort.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCmbBarcodeScanerPort.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCmbBarcodeScanerPort.LabelString = "扫码枪端口";
            this.lblCmbBarcodeScanerPort.Location = new System.Drawing.Point(0, 178);
            this.lblCmbBarcodeScanerPort.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbBarcodeScanerPort.Name = "lblCmbBarcodeScanerPort";
            this.lblCmbBarcodeScanerPort.Size = new System.Drawing.Size(1215, 37);
            this.lblCmbBarcodeScanerPort.TabIndex = 4;
            // 
            // lblCmbIsLAndR
            // 
            this.lblCmbIsLAndR.BackColor = System.Drawing.Color.Wheat;
            this.lblCmbIsLAndR.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCmbIsLAndR.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCmbIsLAndR.LabelString = "是否区分左右";
            this.lblCmbIsLAndR.Location = new System.Drawing.Point(0, 141);
            this.lblCmbIsLAndR.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbIsLAndR.Name = "lblCmbIsLAndR";
            this.lblCmbIsLAndR.Size = new System.Drawing.Size(1215, 37);
            this.lblCmbIsLAndR.TabIndex = 3;
            // 
            // lblTxtProductNo
            // 
            this.lblTxtProductNo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTxtProductNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTxtProductNo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTxtProductNo.LabelString = "项目编号";
            this.lblTxtProductNo.Location = new System.Drawing.Point(0, 104);
            this.lblTxtProductNo.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblTxtProductNo.Name = "lblTxtProductNo";
            this.lblTxtProductNo.Size = new System.Drawing.Size(1215, 37);
            this.lblTxtProductNo.TabIndex = 2;
            // 
            // lblTxtProductName
            // 
            this.lblTxtProductName.BackColor = System.Drawing.Color.Wheat;
            this.lblTxtProductName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTxtProductName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTxtProductName.LabelString = "产品名称";
            this.lblTxtProductName.Location = new System.Drawing.Point(0, 67);
            this.lblTxtProductName.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblTxtProductName.Name = "lblTxtProductName";
            this.lblTxtProductName.Size = new System.Drawing.Size(1215, 37);
            this.lblTxtProductName.TabIndex = 1;
            // 
            // lblTxtDeviceNo
            // 
            this.lblTxtDeviceNo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTxtDeviceNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTxtDeviceNo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTxtDeviceNo.LabelString = "设备编号";
            this.lblTxtDeviceNo.Location = new System.Drawing.Point(0, 30);
            this.lblTxtDeviceNo.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblTxtDeviceNo.Name = "lblTxtDeviceNo";
            this.lblTxtDeviceNo.Size = new System.Drawing.Size(1215, 37);
            this.lblTxtDeviceNo.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Azure;
            this.menuStrip1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.操作ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1215, 30);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 操作ToolStripMenuItem
            // 
            this.操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存ToolStripMenuItem,
            this.添加检测项ToolStripMenuItem,
            this.添加二维码ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.操作ToolStripMenuItem.Name = "操作ToolStripMenuItem";
            this.操作ToolStripMenuItem.Size = new System.Drawing.Size(54, 26);
            this.操作ToolStripMenuItem.Text = "操作";
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.保存ToolStripMenuItem.Text = "保存";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.保存ToolStripMenuItem_Click);
            // 
            // 添加检测项ToolStripMenuItem
            // 
            this.添加检测项ToolStripMenuItem.Name = "添加检测项ToolStripMenuItem";
            this.添加检测项ToolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.添加检测项ToolStripMenuItem.Text = "添加检测项";
            this.添加检测项ToolStripMenuItem.Click += new System.EventHandler(this.添加检测项ToolStripMenuItem_Click);
            // 
            // 添加二维码ToolStripMenuItem
            // 
            this.添加二维码ToolStripMenuItem.Name = "添加二维码ToolStripMenuItem";
            this.添加二维码ToolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.添加二维码ToolStripMenuItem.Text = "添加二维码";
            this.添加二维码ToolStripMenuItem.Click += new System.EventHandler(this.添加二维码ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.退出ToolStripMenuItem.Text = "退出";
            // 
            // lblCmbIsStamp
            // 
            this.lblCmbIsStamp.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblCmbIsStamp.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCmbIsStamp.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCmbIsStamp.LabelString = "打章";
            this.lblCmbIsStamp.Location = new System.Drawing.Point(0, 252);
            this.lblCmbIsStamp.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbIsStamp.Name = "lblCmbIsStamp";
            this.lblCmbIsStamp.Size = new System.Drawing.Size(1215, 37);
            this.lblCmbIsStamp.TabIndex = 8;
            // 
            // lblCmbIsReadButton
            // 
            this.lblCmbIsReadButton.BackColor = System.Drawing.Color.Wheat;
            this.lblCmbIsReadButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCmbIsReadButton.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCmbIsReadButton.LabelString = "扫码后是否需要按键启动";
            this.lblCmbIsReadButton.Location = new System.Drawing.Point(0, 289);
            this.lblCmbIsReadButton.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbIsReadButton.Name = "lblCmbIsReadButton";
            this.lblCmbIsReadButton.Size = new System.Drawing.Size(1215, 37);
            this.lblCmbIsReadButton.TabIndex = 9;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.tabControl1.Location = new System.Drawing.Point(0, 326);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1215, 413);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvCheckItems);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1207, 381);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "检测项目";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvCheckItems
            // 
            this.dgvCheckItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCheckItems.Location = new System.Drawing.Point(3, 3);
            this.dgvCheckItems.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.dgvCheckItems.Name = "dgvCheckItems";
            this.dgvCheckItems.Size = new System.Drawing.Size(1201, 375);
            this.dgvCheckItems.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1207, 381);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "二维码";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvBarocodeList);
            this.splitContainer1.Size = new System.Drawing.Size(1201, 375);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(200, 375);
            this.treeView.TabIndex = 3;
            // 
            // dgvBarocodeList
            // 
            this.dgvBarocodeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBarocodeList.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.dgvBarocodeList.Location = new System.Drawing.Point(0, 0);
            this.dgvBarocodeList.Margin = new System.Windows.Forms.Padding(12, 16, 12, 16);
            this.dgvBarocodeList.Name = "dgvBarocodeList";
            this.dgvBarocodeList.Size = new System.Drawing.Size(997, 375);
            this.dgvBarocodeList.TabIndex = 2;
            // 
            // contextMenuStripTree
            // 
            this.contextMenuStripTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStripTree.Name = "contextMenuStripTree";
            this.contextMenuStripTree.Size = new System.Drawing.Size(101, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.deleteToolStripMenuItem.Text = "删除";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // FormCcdSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 739);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblCmbIsReadButton);
            this.Controls.Add(this.lblCmbIsStamp);
            this.Controls.Add(this.lblCmbPowerPort);
            this.Controls.Add(this.lblCmbBarcodeScanerPort);
            this.Controls.Add(this.lblCmbIsLAndR);
            this.Controls.Add(this.lblTxtProductNo);
            this.Controls.Add(this.lblTxtProductName);
            this.Controls.Add(this.lblTxtDeviceNo);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormCcdSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加CCD检测模板";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStripTree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UserControls.LabelText lblTxtDeviceNo;
        private UserControls.LabelText lblTxtProductName;
        private UserControls.LabelText lblTxtProductNo;
        private UserControls.LabelCombox lblCmbIsLAndR;
        private UserControls.LabelCombox lblCmbBarcodeScanerPort;
        private UserControls.LabelCombox lblCmbPowerPort;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加检测项ToolStripMenuItem;
        private UserControls.LabelCombox lblCmbIsStamp;
        private UserControls.LabelCombox lblCmbIsReadButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private UserControls.UserDataGrid dgvCheckItems;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem 添加二维码ToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private UserControls.UserDataGrid dgvBarocodeList;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTree;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}