namespace DeviceDesign
{
    partial class FormConditions
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
            this.toolStripStatusUnits = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAddDelay = new System.Windows.Forms.ToolStripButton();
            this.toolStripTxtAddDelay = new System.Windows.Forms.ToolStripTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStripButtonAddEqualTrue = new System.Windows.Forms.ToolStripButton();
            this.toolStripStatusUnits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripStatusUnits
            // 
            this.toolStripStatusUnits.BackColor = System.Drawing.Color.LightSkyBlue;
            this.toolStripStatusUnits.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripStatusUnits.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripButtonCancel,
            this.toolStripButtonAddDelay,
            this.toolStripTxtAddDelay,
            this.toolStripButtonAddEqualTrue});
            this.toolStripStatusUnits.Location = new System.Drawing.Point(0, 0);
            this.toolStripStatusUnits.Name = "toolStripStatusUnits";
            this.toolStripStatusUnits.Size = new System.Drawing.Size(1013, 39);
            this.toolStripStatusUnits.TabIndex = 1;
            this.toolStripStatusUnits.Text = "toolStrip1";
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = global::DeviceDesign.Properties.Resources.Save;
            this.toolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(78, 36);
            this.toolStripButtonSave.Text = "保存";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonCancel
            // 
            this.toolStripButtonCancel.Image = global::DeviceDesign.Properties.Resources.del;
            this.toolStripButtonCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCancel.Name = "toolStripButtonCancel";
            this.toolStripButtonCancel.Size = new System.Drawing.Size(78, 36);
            this.toolStripButtonCancel.Text = "取消";
            this.toolStripButtonCancel.Click += new System.EventHandler(this.toolStripButtonCancel_Click);
            // 
            // toolStripButtonAddDelay
            // 
            this.toolStripButtonAddDelay.Image = global::DeviceDesign.Properties.Resources.add;
            this.toolStripButtonAddDelay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddDelay.Name = "toolStripButtonAddDelay";
            this.toolStripButtonAddDelay.Size = new System.Drawing.Size(94, 36);
            this.toolStripButtonAddDelay.Text = "添加延时";
            this.toolStripButtonAddDelay.Click += new System.EventHandler(this.toolStripButtonAddDelay_Click);
            // 
            // toolStripTxtAddDelay
            // 
            this.toolStripTxtAddDelay.Name = "toolStripTxtAddDelay";
            this.toolStripTxtAddDelay.Size = new System.Drawing.Size(100, 39);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 39);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(1013, 701);
            this.splitContainer1.SplitterDistance = 505;
            this.splitContainer1.TabIndex = 2;
            // 
            // toolStripButtonAddEqualTrue
            // 
            this.toolStripButtonAddEqualTrue.Image = global::DeviceDesign.Properties.Resources.update;
            this.toolStripButtonAddEqualTrue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddEqualTrue.Name = "toolStripButtonAddEqualTrue";
            this.toolStripButtonAddEqualTrue.Size = new System.Drawing.Size(126, 36);
            this.toolStripButtonAddEqualTrue.Text = "添加默认通过";
            this.toolStripButtonAddEqualTrue.Click += new System.EventHandler(this.toolStripButtonAddEqualTrue_Click);
            // 
            // FormConditions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 740);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStripStatusUnits);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormConditions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormConditions";
            this.Load += new System.EventHandler(this.FormConditions_Load);
            this.toolStripStatusUnits.ResumeLayout(false);
            this.toolStripStatusUnits.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripStatusUnits;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonCancel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddDelay;
        private System.Windows.Forms.ToolStripTextBox toolStripTxtAddDelay;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddEqualTrue;
    }
}