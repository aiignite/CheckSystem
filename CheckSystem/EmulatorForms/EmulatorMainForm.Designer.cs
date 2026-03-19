namespace CheckSystem.EmulatorForms
{
    partial class EmulatorMainForm
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
            this.mainTable = new System.Windows.Forms.TableLayoutPanel();
            this.btnTitle = new System.Windows.Forms.Button();
            this.funcTable = new System.Windows.Forms.SplitContainer();
            this.btnsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.dgvInputFields = new Sunny.UI.UIDataGridView();
            this.mainTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.funcTable)).BeginInit();
            this.funcTable.Panel1.SuspendLayout();
            this.funcTable.Panel2.SuspendLayout();
            this.funcTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputFields)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTable
            // 
            this.mainTable.ColumnCount = 1;
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainTable.Controls.Add(this.btnTitle, 0, 0);
            this.mainTable.Controls.Add(this.funcTable, 0, 1);
            this.mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTable.Location = new System.Drawing.Point(0, 35);
            this.mainTable.Name = "mainTable";
            this.mainTable.RowCount = 2;
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTable.Size = new System.Drawing.Size(795, 446);
            this.mainTable.TabIndex = 0;
            // 
            // btnTitle
            // 
            this.btnTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTitle.Enabled = false;
            this.btnTitle.Location = new System.Drawing.Point(3, 3);
            this.btnTitle.Name = "btnTitle";
            this.btnTitle.Size = new System.Drawing.Size(789, 94);
            this.btnTitle.TabIndex = 0;
            this.btnTitle.Text = "button1";
            this.btnTitle.UseVisualStyleBackColor = true;
            // 
            // funcTable
            // 
            this.funcTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.funcTable.Location = new System.Drawing.Point(3, 103);
            this.funcTable.Name = "funcTable";
            // 
            // funcTable.Panel1
            // 
            this.funcTable.Panel1.Controls.Add(this.dgvInputFields);
            // 
            // funcTable.Panel2
            // 
            this.funcTable.Panel2.Controls.Add(this.btnsPanel);
            this.funcTable.Size = new System.Drawing.Size(789, 340);
            this.funcTable.SplitterDistance = 400;
            this.funcTable.TabIndex = 1;
            // 
            // btnsPanel
            // 
            this.btnsPanel.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.btnsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnsPanel.Location = new System.Drawing.Point(0, 0);
            this.btnsPanel.Name = "btnsPanel";
            this.btnsPanel.Size = new System.Drawing.Size(385, 340);
            this.btnsPanel.TabIndex = 3;
            // 
            // dgvInputFields
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvInputFields.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvInputFields.BackgroundColor = System.Drawing.Color.White;
            this.dgvInputFields.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInputFields.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvInputFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInputFields.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvInputFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInputFields.EnableHeadersVisualStyles = false;
            this.dgvInputFields.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvInputFields.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvInputFields.Location = new System.Drawing.Point(0, 0);
            this.dgvInputFields.Name = "dgvInputFields";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInputFields.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvInputFields.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvInputFields.RowTemplate.Height = 23;
            this.dgvInputFields.SelectedIndex = -1;
            this.dgvInputFields.Size = new System.Drawing.Size(400, 340);
            this.dgvInputFields.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvInputFields.TabIndex = 0;
            // 
            // EmulatorMainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(795, 481);
            this.Controls.Add(this.mainTable);
            this.Name = "EmulatorMainForm";
            this.Text = "EmulatorMainForm";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 795, 481);
            this.mainTable.ResumeLayout(false);
            this.funcTable.Panel1.ResumeLayout(false);
            this.funcTable.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.funcTable)).EndInit();
            this.funcTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputFields)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTable;
        private System.Windows.Forms.Button btnTitle;
        private System.Windows.Forms.SplitContainer funcTable;
        private System.Windows.Forms.FlowLayoutPanel btnsPanel;
        private Sunny.UI.UIDataGridView dgvInputFields;
    }
}