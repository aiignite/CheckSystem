namespace CheckSystem.HelperForms.Hvsm
{
    partial class FrmOffsetAll
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ucSingleOffset1 = new CheckSystem.HelperForms.Hvsm.UcSingleOffset();
            this.btnUpdate = new Sunny.UI.UIButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ucSingleOffset1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnUpdate, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 35);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(360, 395);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ucSingleOffset1
            // 
            this.ucSingleOffset1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSingleOffset1.Hsd1B = 0D;
            this.ucSingleOffset1.Hsd1K = 1D;
            this.ucSingleOffset1.Hsd1OverShowThreshold = 1D;
            this.ucSingleOffset1.Hsd1OverShowValue = 1D;
            this.ucSingleOffset1.Hsd2B = 0D;
            this.ucSingleOffset1.Hsd2K = 1D;
            this.ucSingleOffset1.Hsd2OverShowThreshold = 1D;
            this.ucSingleOffset1.Hsd2OverShowValue = 1D;
            this.ucSingleOffset1.Hsd3B = 0D;
            this.ucSingleOffset1.Hsd3K = 1D;
            this.ucSingleOffset1.Hsd3OverShowThreshold = 1D;
            this.ucSingleOffset1.Hsd3OverShowValue = 1D;
            this.ucSingleOffset1.Hsd4B = 0D;
            this.ucSingleOffset1.Hsd4K = 1D;
            this.ucSingleOffset1.Hsd4OverShowThreshold = 1D;
            this.ucSingleOffset1.Hsd4OverShowValue = 1D;
            this.ucSingleOffset1.IsHsd1OverShow = false;
            this.ucSingleOffset1.IsHsd2OverShow = false;
            this.ucSingleOffset1.IsHsd3OverShow = false;
            this.ucSingleOffset1.IsHsd4OverShow = false;
            this.ucSingleOffset1.Location = new System.Drawing.Point(4, 4);
            this.ucSingleOffset1.Margin = new System.Windows.Forms.Padding(4);
            this.ucSingleOffset1.Name = "ucSingleOffset1";
            this.ucSingleOffset1.Size = new System.Drawing.Size(352, 287);
            this.ucSingleOffset1.TabIndex = 0;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUpdate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdate.Location = new System.Drawing.Point(3, 298);
            this.btnUpdate.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(354, 94);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // FrmOffsetAll
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(360, 430);
            this.ControlBoxFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(360, 430);
            this.MinimumSize = new System.Drawing.Size(360, 430);
            this.Name = "FrmOffsetAll";
            this.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.Style = Sunny.UI.UIStyle.Custom;
            this.Text = "统一设定";
            this.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private UcSingleOffset ucSingleOffset1;
        private Sunny.UI.UIButton btnUpdate;
    }
}