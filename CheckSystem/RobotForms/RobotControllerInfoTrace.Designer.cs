namespace CheckSystem.RobotForms
{
    partial class RobotControllerInfoTrace
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
            this.readDataGrid = new UserControls.UserDataGrid();
            this.SuspendLayout();
            // 
            // readDataGrid
            // 
            this.readDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.readDataGrid.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold);
            this.readDataGrid.ForeColor = System.Drawing.Color.Black;
            this.readDataGrid.Location = new System.Drawing.Point(0, 0);
            this.readDataGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.readDataGrid.Name = "readDataGrid";
            this.readDataGrid.Size = new System.Drawing.Size(784, 561);
            this.readDataGrid.TabIndex = 4;
            // 
            // RobotControllerInfoTrace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.readDataGrid);
            this.Name = "RobotControllerInfoTrace";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RobotControllerInfoTrace";
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.UserDataGrid readDataGrid;
    }
}