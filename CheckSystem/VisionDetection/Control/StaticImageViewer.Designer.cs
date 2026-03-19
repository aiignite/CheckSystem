namespace CheckSystem.VisionDetection.Control
{
    partial class StaticImageViewer
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.visionContainer = new Sunny.UI.UISplitContainer();
            this.MainImageViewer = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.grayDgv = new Sunny.UI.UIDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.visionContainer)).BeginInit();
            this.visionContainer.Panel1.SuspendLayout();
            this.visionContainer.Panel2.SuspendLayout();
            this.visionContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grayDgv)).BeginInit();
            this.SuspendLayout();
            // 
            // visionContainer
            // 
            this.visionContainer.CollapsePanel = Sunny.UI.UISplitContainer.UICollapsePanel.Panel2;
            this.visionContainer.Cursor = System.Windows.Forms.Cursors.Default;
            this.visionContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionContainer.Location = new System.Drawing.Point(0, 0);
            this.visionContainer.MinimumSize = new System.Drawing.Size(20, 20);
            this.visionContainer.Name = "visionContainer";
            // 
            // visionContainer.Panel1
            // 
            this.visionContainer.Panel1.Controls.Add(this.MainImageViewer);
            // 
            // visionContainer.Panel2
            // 
            this.visionContainer.Panel2.Controls.Add(this.grayDgv);
            this.visionContainer.Size = new System.Drawing.Size(934, 595);
            this.visionContainer.SplitterDistance = 680;
            this.visionContainer.SplitterWidth = 11;
            this.visionContainer.TabIndex = 1;
            // 
            // MainImageViewer
            // 
            this.MainImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainImageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainImageViewer.Location = new System.Drawing.Point(0, 0);
            this.MainImageViewer.Name = "MainImageViewer";
            this.MainImageViewer.Size = new System.Drawing.Size(680, 595);
            this.MainImageViewer.TabIndex = 11;
            // 
            // grayDgv
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.grayDgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grayDgv.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.grayDgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grayDgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grayDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grayDgv.DefaultCellStyle = dataGridViewCellStyle3;
            this.grayDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grayDgv.EnableHeadersVisualStyles = false;
            this.grayDgv.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grayDgv.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(173)))), ((int)(((byte)(255)))));
            this.grayDgv.Location = new System.Drawing.Point(0, 0);
            this.grayDgv.Name = "grayDgv";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grayDgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.grayDgv.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.grayDgv.RowTemplate.Height = 23;
            this.grayDgv.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.grayDgv.SelectedIndex = -1;
            this.grayDgv.Size = new System.Drawing.Size(243, 595);
            this.grayDgv.TabIndex = 4;
            // 
            // StaticImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.visionContainer);
            this.Name = "StaticImageViewer";
            this.Size = new System.Drawing.Size(934, 595);
            this.visionContainer.Panel1.ResumeLayout(false);
            this.visionContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.visionContainer)).EndInit();
            this.visionContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grayDgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UISplitContainer visionContainer;
        private NationalInstruments.Vision.WindowsForms.ImageViewer MainImageViewer;
        private Sunny.UI.UIDataGridView grayDgv;
    }
}
