namespace CheckSystem.RobotForms
{
    partial class RobotControllerServoControl
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
            this.dgvServo = new UserControls.UserDataGrid();
            this.SuspendLayout();
            // 
            // dgvServo
            // 
            this.dgvServo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServo.Location = new System.Drawing.Point(0, 0);
            this.dgvServo.Name = "dgvServo";
            this.dgvServo.Size = new System.Drawing.Size(634, 611);
            this.dgvServo.TabIndex = 0;
            // 
            // RobotControllerServoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 611);
            this.Controls.Add(this.dgvServo);
            this.MaximizeBox = false;
            this.Name = "RobotControllerServoControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Servo Control";
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.UserDataGrid dgvServo;
    }
}