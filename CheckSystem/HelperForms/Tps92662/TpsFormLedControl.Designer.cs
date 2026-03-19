namespace CheckSystem.HelperForms.Tps92662
{
    partial class TpsFormLedControl
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
            this.btnChangeHz = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReadB0B1 = new System.Windows.Forms.Button();
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
            this.ledTableLayout.Controls.Add(this.btnReadB0B1, 4, 0);
            this.ledTableLayout.Controls.Add(this.btnChangeHz, 3, 0);
            this.ledTableLayout.Controls.Add(this.label3, 5, 0);
            this.ledTableLayout.Controls.Add(this.label1, 0, 0);
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
            // btnChangeHz
            // 
            this.btnChangeHz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChangeHz.Location = new System.Drawing.Point(504, 3);
            this.btnChangeHz.Name = "btnChangeHz";
            this.btnChangeHz.Size = new System.Drawing.Size(161, 43);
            this.btnChangeHz.TabIndex = 12;
            this.btnChangeHz.Text = "点击更换频率";
            this.btnChangeHz.UseVisualStyleBackColor = true;
            this.btnChangeHz.Click += new System.EventHandler(this.btnChangeHz_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(838, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 49);
            this.label3.TabIndex = 7;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 49);
            this.label1.TabIndex = 0;
            this.label1.Text = "设置频率/HZ：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReadB0B1
            // 
            this.btnReadB0B1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReadB0B1.Location = new System.Drawing.Point(671, 3);
            this.btnReadB0B1.Name = "btnReadB0B1";
            this.btnReadB0B1.Size = new System.Drawing.Size(161, 43);
            this.btnReadB0B1.TabIndex = 13;
            this.btnReadB0B1.Text = "读B0B1";
            this.btnReadB0B1.UseVisualStyleBackColor = true;
            this.btnReadB0B1.Click += new System.EventHandler(this.btnReadB0B1_Click);
            // 
            // TpsFormLedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.ledTableLayout);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 768);
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "TpsFormLedControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Matrix LED Controller";
            this.ledTableLayout.ResumeLayout(false);
            this.ledTableLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ledTableLayout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnChangeHz;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnReadB0B1;
    }
}