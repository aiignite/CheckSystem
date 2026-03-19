namespace CheckSystem
{
    partial class FormBatchStaticVisionCali
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
            this.uiSplitContainer1 = new Sunny.UI.UISplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.parentRoiTreeview = new Sunny.UI.UITreeView();
            this.btnExecute = new Sunny.UI.UIButton();
            this.treeViewCamerasList = new Sunny.UI.UITreeView();
            this.imgPreview = new Sunny.UI.UITitlePanel();
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainer1)).BeginInit();
            this.uiSplitContainer1.Panel1.SuspendLayout();
            this.uiSplitContainer1.Panel2.SuspendLayout();
            this.uiSplitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiSplitContainer1
            // 
            this.uiSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.uiSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSplitContainer1.Location = new System.Drawing.Point(0, 35);
            this.uiSplitContainer1.MinimumSize = new System.Drawing.Size(20, 20);
            this.uiSplitContainer1.Name = "uiSplitContainer1";
            // 
            // uiSplitContainer1.Panel1
            // 
            this.uiSplitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // uiSplitContainer1.Panel2
            // 
            this.uiSplitContainer1.Panel2.Controls.Add(this.imgPreview);
            this.uiSplitContainer1.Size = new System.Drawing.Size(1009, 569);
            this.uiSplitContainer1.SplitterDistance = 336;
            this.uiSplitContainer1.SplitterWidth = 11;
            this.uiSplitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.parentRoiTreeview, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnExecute, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.treeViewCamerasList, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(336, 569);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // parentRoiTreeview
            // 
            this.parentRoiTreeview.CheckBoxes = true;
            this.parentRoiTreeview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parentRoiTreeview.FillColor = System.Drawing.Color.White;
            this.parentRoiTreeview.Font = new System.Drawing.Font("宋体", 10F);
            this.parentRoiTreeview.Location = new System.Drawing.Point(4, 264);
            this.parentRoiTreeview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.parentRoiTreeview.MinimumSize = new System.Drawing.Size(1, 1);
            this.parentRoiTreeview.Name = "parentRoiTreeview";
            this.parentRoiTreeview.ScrollBarStyleInherited = false;
            this.parentRoiTreeview.ShowText = false;
            this.parentRoiTreeview.Size = new System.Drawing.Size(328, 249);
            this.parentRoiTreeview.TabIndex = 1;
            this.parentRoiTreeview.Text = "uiTreeView2";
            this.parentRoiTreeview.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExecute
            // 
            this.btnExecute.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExecute.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExecute.Location = new System.Drawing.Point(3, 521);
            this.btnExecute.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(330, 45);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "执行";
            this.btnExecute.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // treeViewCamerasList
            // 
            this.treeViewCamerasList.CheckBoxes = true;
            this.treeViewCamerasList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCamerasList.FillColor = System.Drawing.Color.White;
            this.treeViewCamerasList.Font = new System.Drawing.Font("宋体", 10F);
            this.treeViewCamerasList.Location = new System.Drawing.Point(4, 5);
            this.treeViewCamerasList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.treeViewCamerasList.MinimumSize = new System.Drawing.Size(1, 1);
            this.treeViewCamerasList.Name = "treeViewCamerasList";
            this.treeViewCamerasList.ScrollBarStyleInherited = false;
            this.treeViewCamerasList.ShowText = false;
            this.treeViewCamerasList.Size = new System.Drawing.Size(328, 249);
            this.treeViewCamerasList.TabIndex = 3;
            this.treeViewCamerasList.Text = "uiTreeView1";
            this.treeViewCamerasList.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imgPreview
            // 
            this.imgPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgPreview.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.imgPreview.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.imgPreview.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imgPreview.Location = new System.Drawing.Point(0, 0);
            this.imgPreview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.imgPreview.MinimumSize = new System.Drawing.Size(1, 1);
            this.imgPreview.Name = "imgPreview";
            this.imgPreview.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.imgPreview.ShowText = false;
            this.imgPreview.Size = new System.Drawing.Size(662, 569);
            this.imgPreview.Style = Sunny.UI.UIStyle.Custom;
            this.imgPreview.TabIndex = 0;
            this.imgPreview.Text = "预览";
            this.imgPreview.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.imgPreview.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            // 
            // FormBatchStaticVisionCali
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1009, 604);
            this.Controls.Add(this.uiSplitContainer1);
            this.Name = "FormBatchStaticVisionCali";
            this.Text = "批量标定光型";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 961, 568);
            this.uiSplitContainer1.Panel1.ResumeLayout(false);
            this.uiSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainer1)).EndInit();
            this.uiSplitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UISplitContainer uiSplitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Sunny.UI.UITreeView parentRoiTreeview;
        private Sunny.UI.UIButton btnExecute;
        private Sunny.UI.UITitlePanel imgPreview;
        private Sunny.UI.UITreeView treeViewCamerasList;
    }
}