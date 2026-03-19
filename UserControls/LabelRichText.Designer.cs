namespace UserControls
{
    partial class LabelRichText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelRichText));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label = new System.Windows.Forms.Label();
            this.userControlRichText1 = new UserControls.UserControlRichText();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.userControlRichText1);
            this.splitContainer1.Size = new System.Drawing.Size(422, 91);
            this.splitContainer1.SplitterDistance = 141;
            this.splitContainer1.SplitterWidth = 7;
            this.splitContainer1.TabIndex = 2;
            // 
            // label
            // 
            this.label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label.Location = new System.Drawing.Point(5, 32);
            this.label.Margin = new System.Windows.Forms.Padding(5);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(56, 25);
            this.label.TabIndex = 1;
            this.label.Text = "label";
            // 
            // userControlRichText1
            // 
            this.userControlRichText1.BackColor = System.Drawing.SystemColors.Info;
            this.userControlRichText1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userControlRichText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichText1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlRichText1.Location = new System.Drawing.Point(0, 0);
            this.userControlRichText1.LstKeyWord = ((System.Collections.Generic.List<string>)(resources.GetObject("userControlRichText1.LstKeyWord")));
            this.userControlRichText1.LstVariant = ((System.Collections.Generic.List<string>)(resources.GetObject("userControlRichText1.LstVariant")));
            this.userControlRichText1.Margin = new System.Windows.Forms.Padding(5);
            this.userControlRichText1.Name = "userControlRichText1";
            this.userControlRichText1.Padding = new System.Windows.Forms.Padding(2);
            this.userControlRichText1.Size = new System.Drawing.Size(274, 91);
            this.userControlRichText1.TabIndex = 0;
            // 
            // LabelRichText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "LabelRichText";
            this.Size = new System.Drawing.Size(422, 91);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.Label label;
        public UserControlRichText userControlRichText1;
    }
}
