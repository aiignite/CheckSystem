namespace CheckSystem.HelperForms.Asl5115
{
    partial class AslFormLedControl
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
            this.ledTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnMtp = new System.Windows.Forms.Button();
            this.btnNormal = new System.Windows.Forms.Button();
            this.btnLMP = new System.Windows.Forms.Button();
            this.cmbAddr = new System.Windows.Forms.ComboBox();
            this.ledTableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // ledTableLayout
            // 
            this.ledTableLayout.ColumnCount = 6;
            this.ledTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.ledTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.ledTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.ledTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.ledTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.ledTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66945F));
            this.ledTableLayout.Controls.Add(this.btnMtp, 3, 0);
            this.ledTableLayout.Controls.Add(this.btnNormal, 1, 0);
            this.ledTableLayout.Controls.Add(this.btnLMP, 0, 0);
            this.ledTableLayout.Controls.Add(this.cmbAddr, 2, 0);
            this.ledTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledTableLayout.Location = new System.Drawing.Point(0, 0);
            this.ledTableLayout.Name = "ledTableLayout";
            this.ledTableLayout.RowCount = 3;
            this.ledTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.730769F));
            this.ledTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.63462F));
            this.ledTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.63462F));
            this.ledTableLayout.Size = new System.Drawing.Size(1008, 729);
            this.ledTableLayout.TabIndex = 0;
            // 
            // btnMtp
            // 
            this.btnMtp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMtp.Enabled = false;
            this.btnMtp.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold);
            this.btnMtp.Location = new System.Drawing.Point(501, 0);
            this.btnMtp.Margin = new System.Windows.Forms.Padding(0);
            this.btnMtp.Name = "btnMtp";
            this.btnMtp.Size = new System.Drawing.Size(167, 49);
            this.btnMtp.TabIndex = 3;
            this.btnMtp.Text = "读写MTP";
            this.btnMtp.UseVisualStyleBackColor = true;
            this.btnMtp.Click += new System.EventHandler(this.btnMtp_Click);
            // 
            // btnNormal
            // 
            this.btnNormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNormal.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold);
            this.btnNormal.Location = new System.Drawing.Point(167, 0);
            this.btnNormal.Margin = new System.Windows.Forms.Padding(0);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(167, 49);
            this.btnNormal.TabIndex = 1;
            this.btnNormal.Text = "NORMAL";
            this.btnNormal.UseVisualStyleBackColor = true;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // btnLMP
            // 
            this.btnLMP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLMP.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold);
            this.btnLMP.Location = new System.Drawing.Point(0, 0);
            this.btnLMP.Margin = new System.Windows.Forms.Padding(0);
            this.btnLMP.Name = "btnLMP";
            this.btnLMP.Size = new System.Drawing.Size(167, 49);
            this.btnLMP.TabIndex = 0;
            this.btnLMP.Text = "Limp Home";
            this.btnLMP.UseVisualStyleBackColor = true;
            this.btnLMP.Click += new System.EventHandler(this.btnLMP_Click);
            // 
            // cmbAddr
            // 
            this.cmbAddr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAddr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAddr.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbAddr.FormattingEnabled = true;
            this.cmbAddr.Location = new System.Drawing.Point(337, 3);
            this.cmbAddr.Name = "cmbAddr";
            this.cmbAddr.Size = new System.Drawing.Size(161, 28);
            this.cmbAddr.TabIndex = 2;
            // 
            // AslFormLedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.ledTableLayout);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 768);
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "AslFormLedControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Matrix LED Controller";
            this.ledTableLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ledTableLayout;
        private System.Windows.Forms.Button btnNormal;
        private System.Windows.Forms.Button btnLMP;
        private System.Windows.Forms.ComboBox cmbAddr;
        private System.Windows.Forms.Button btnMtp;
    }
}