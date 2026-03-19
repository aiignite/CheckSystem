namespace CheckSystem.RobotForms
{
    partial class RobotControllerProgramAdd
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
            this.labelText1 = new UserControls.LabelText();
            this.labelText2 = new UserControls.LabelText();
            this.btnSave = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.userDataGrid1 = new UserControls.UserDataGrid();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelText1
            // 
            this.labelText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelText1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelText1.LabelString = "程序名";
            this.labelText1.Location = new System.Drawing.Point(0, 2);
            this.labelText1.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelText1.Name = "labelText1";
            this.labelText1.Size = new System.Drawing.Size(1134, 98);
            this.labelText1.TabIndex = 0;
            // 
            // labelText2
            // 
            this.labelText2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelText2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelText2.LabelString = "注释";
            this.labelText2.Location = new System.Drawing.Point(0, 102);
            this.labelText2.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelText2.Name = "labelText2";
            this.labelText2.Size = new System.Drawing.Size(1134, 98);
            this.labelText2.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("黑体", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.btnSave.Location = new System.Drawing.Point(3, 583);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(1128, 94);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelText1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelText2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.userDataGrid1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1134, 680);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // userDataGrid1
            // 
            this.userDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userDataGrid1.Font = new System.Drawing.Font("黑体", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.userDataGrid1.Location = new System.Drawing.Point(4, 204);
            this.userDataGrid1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.userDataGrid1.Name = "userDataGrid1";
            this.userDataGrid1.Size = new System.Drawing.Size(1126, 372);
            this.userDataGrid1.TabIndex = 1;
            // 
            // FormProgramAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 680);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormProgramAdd";
            this.Text = "添加程序名";
            this.Load += new System.EventHandler(this.FormProgramAdd_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.LabelText labelText1;
        private UserControls.LabelText labelText2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private UserControls.UserDataGrid userDataGrid1;
    }
}