namespace UserControls
{
    partial class UserImageDataViewer
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.imageViewer = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.userDataGridCheckData = new UserControls.UserDataGrid();
            this.userGrayData = new UserControls.UserDataGrid();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.userDataGridCheckData, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.userGrayData, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.imageViewer, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(849, 537);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // imageViewer
            // 
            this.imageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer.Location = new System.Drawing.Point(300, 3);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Size = new System.Drawing.Size(333, 531);
            this.imageViewer.TabIndex = 10;
            // 
            // userDataGridCheckData
            // 
            this.userDataGridCheckData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userDataGridCheckData.Location = new System.Drawing.Point(3, 3);
            this.userDataGridCheckData.Name = "userDataGridCheckData";
            this.userDataGridCheckData.Size = new System.Drawing.Size(291, 531);
            this.userDataGridCheckData.TabIndex = 12;
            // 
            // userGrayData
            // 
            this.userGrayData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userGrayData.Location = new System.Drawing.Point(639, 3);
            this.userGrayData.Name = "userGrayData";
            this.userGrayData.Size = new System.Drawing.Size(207, 531);
            this.userGrayData.TabIndex = 11;
            // 
            // UserImageDataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UserImageDataViewer";
            this.Size = new System.Drawing.Size(849, 537);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public UserDataGrid userGrayData;
        public UserDataGrid userDataGridCheckData;
        public NationalInstruments.Vision.WindowsForms.ImageViewer imageViewer;
    }
}
