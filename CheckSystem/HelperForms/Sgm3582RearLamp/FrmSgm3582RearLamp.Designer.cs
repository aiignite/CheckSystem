using HZH_Controls.Controls.RadioButton;

namespace CheckSystem.HelperForms.Sgm3582RearLamp
{
    partial class FrmSgm3582RearLamp
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
            this.uiSwitch1 = new Sunny.UI.UISwitch();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.gpTail = new System.Windows.Forms.GroupBox();
            this.rbTailHighLightOn = new UCRadioButton();
            this.rbTailNormal = new UCRadioButton();
            this.rbTailOff = new UCRadioButton();
            this.gpTurn = new System.Windows.Forms.GroupBox();
            this.rbTurnHoldOnLight = new UCRadioButton();
            this.rbTurnHighLightOn = new UCRadioButton();
            this.rbTurnSwipeTurn = new UCRadioButton();
            this.rbTurnOff = new UCRadioButton();
            this.uiMarkLabel12 = new Sunny.UI.UIMarkLabel();
            this.swIsTurnOnOff = new Sunny.UI.UISwitch();
            this.gpAnimation = new System.Windows.Forms.GroupBox();
            this.rbComplexComingHome = new UCRadioButton();
            this.rbComplexLeavingHome = new UCRadioButton();
            this.rbSimpleComingHome = new UCRadioButton();
            this.rbSimpleLeavingHome = new UCRadioButton();
            this.rbLampShowAbort = new UCRadioButton();
            this.gpError = new System.Windows.Forms.GroupBox();
            this.lblLeftBTail = new Sunny.UI.UILabel();
            this.lblLeftATail = new Sunny.UI.UILabel();
            this.lblRightBTail = new Sunny.UI.UILabel();
            this.lblRightATail = new Sunny.UI.UILabel();
            this.lblRightAStop = new Sunny.UI.UILabel();
            this.lblLeftAStop = new Sunny.UI.UILabel();
            this.lblRightBTurn = new Sunny.UI.UILabel();
            this.lblRightATurn = new Sunny.UI.UILabel();
            this.lblLeftBTurn = new Sunny.UI.UILabel();
            this.lblLeftATurn = new Sunny.UI.UILabel();
            this.uiMarkLabel9 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel8 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel5 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel11 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel7 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel10 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel6 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.gpTail.SuspendLayout();
            this.gpTurn.SuspendLayout();
            this.gpAnimation.SuspendLayout();
            this.gpError.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiSwitch1
            // 
            this.uiSwitch1.Active = true;
            this.uiSwitch1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiSwitch1.Location = new System.Drawing.Point(163, 46);
            this.uiSwitch1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSwitch1.Name = "uiSwitch1";
            this.uiSwitch1.Size = new System.Drawing.Size(75, 29);
            this.uiSwitch1.TabIndex = 0;
            this.uiSwitch1.Text = "uiSwitch1";
            this.uiSwitch1.ActiveChanged += new System.EventHandler(this.uiSwitch1_ActiveChanged);
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel1.Location = new System.Drawing.Point(16, 46);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel1.TabIndex = 1;
            this.uiMarkLabel1.Text = "Start LIN: ";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gpTail
            // 
            this.gpTail.Controls.Add(this.rbTailHighLightOn);
            this.gpTail.Controls.Add(this.rbTailNormal);
            this.gpTail.Controls.Add(this.rbTailOff);
            this.gpTail.Location = new System.Drawing.Point(19, 80);
            this.gpTail.Name = "gpTail";
            this.gpTail.Size = new System.Drawing.Size(272, 99);
            this.gpTail.TabIndex = 2;
            this.gpTail.TabStop = false;
            this.gpTail.Text = "Tail";
            // 
            // rbTailHighLightOn
            // 
            this.rbTailHighLightOn.Checked = false;
            this.rbTailHighLightOn.GroupName = "tail";
            this.rbTailHighLightOn.Location = new System.Drawing.Point(6, 61);
            this.rbTailHighLightOn.Name = "rbTailHighLightOn";
            this.rbTailHighLightOn.Size = new System.Drawing.Size(247, 30);
            this.rbTailHighLightOn.TabIndex = 0;
            this.rbTailHighLightOn.TextValue = "位置灯高亮,复用位置灯低亮";
            // 
            // rbTailNormal
            // 
            this.rbTailNormal.Checked = false;
            this.rbTailNormal.GroupName = "tail";
            this.rbTailNormal.Location = new System.Drawing.Point(134, 25);
            this.rbTailNormal.Name = "rbTailNormal";
            this.rbTailNormal.Size = new System.Drawing.Size(119, 30);
            this.rbTailNormal.TabIndex = 0;
            this.rbTailNormal.TextValue = "位置灯高亮";
            // 
            // rbTailOff
            // 
            this.rbTailOff.Checked = true;
            this.rbTailOff.GroupName = "tail";
            this.rbTailOff.Location = new System.Drawing.Point(6, 25);
            this.rbTailOff.Name = "rbTailOff";
            this.rbTailOff.Size = new System.Drawing.Size(110, 30);
            this.rbTailOff.TabIndex = 0;
            this.rbTailOff.TextValue = "位置灯关";
            // 
            // gpTurn
            // 
            this.gpTurn.Controls.Add(this.rbTurnHoldOnLight);
            this.gpTurn.Controls.Add(this.rbTurnHighLightOn);
            this.gpTurn.Controls.Add(this.rbTurnSwipeTurn);
            this.gpTurn.Controls.Add(this.rbTurnOff);
            this.gpTurn.Controls.Add(this.uiMarkLabel12);
            this.gpTurn.Controls.Add(this.swIsTurnOnOff);
            this.gpTurn.Location = new System.Drawing.Point(19, 185);
            this.gpTurn.Name = "gpTurn";
            this.gpTurn.Size = new System.Drawing.Size(272, 207);
            this.gpTurn.TabIndex = 2;
            this.gpTurn.TabStop = false;
            this.gpTurn.Text = "Turn";
            // 
            // rbTurnHoldOnLight
            // 
            this.rbTurnHoldOnLight.Checked = false;
            this.rbTurnHoldOnLight.GroupName = "turn";
            this.rbTurnHoldOnLight.Location = new System.Drawing.Point(6, 125);
            this.rbTurnHoldOnLight.Name = "rbTurnHoldOnLight";
            this.rbTurnHoldOnLight.Size = new System.Drawing.Size(247, 30);
            this.rbTurnHoldOnLight.TabIndex = 0;
            this.rbTurnHoldOnLight.TextValue = "转向灯A灯低亮B灯高亮";
            // 
            // rbTurnHighLightOn
            // 
            this.rbTurnHighLightOn.Checked = false;
            this.rbTurnHighLightOn.GroupName = "turn";
            this.rbTurnHighLightOn.Location = new System.Drawing.Point(6, 89);
            this.rbTurnHighLightOn.Name = "rbTurnHighLightOn";
            this.rbTurnHighLightOn.Size = new System.Drawing.Size(247, 30);
            this.rbTurnHighLightOn.TabIndex = 0;
            this.rbTurnHighLightOn.TextValue = "转向灯A灯高亮B灯流水";
            // 
            // rbTurnSwipeTurn
            // 
            this.rbTurnSwipeTurn.Checked = false;
            this.rbTurnSwipeTurn.GroupName = "turn";
            this.rbTurnSwipeTurn.Location = new System.Drawing.Point(6, 53);
            this.rbTurnSwipeTurn.Name = "rbTurnSwipeTurn";
            this.rbTurnSwipeTurn.Size = new System.Drawing.Size(247, 30);
            this.rbTurnSwipeTurn.TabIndex = 0;
            this.rbTurnSwipeTurn.TextValue = "转向灯A灯低亮B灯流水";
            // 
            // rbTurnOff
            // 
            this.rbTurnOff.Checked = true;
            this.rbTurnOff.GroupName = "turn";
            this.rbTurnOff.Location = new System.Drawing.Point(6, 17);
            this.rbTurnOff.Name = "rbTurnOff";
            this.rbTurnOff.Size = new System.Drawing.Size(110, 30);
            this.rbTurnOff.TabIndex = 0;
            this.rbTurnOff.TextValue = "转向灯灭";
            // 
            // uiMarkLabel12
            // 
            this.uiMarkLabel12.Font = new System.Drawing.Font("宋体", 10F);
            this.uiMarkLabel12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel12.Location = new System.Drawing.Point(6, 175);
            this.uiMarkLabel12.Name = "uiMarkLabel12";
            this.uiMarkLabel12.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel12.Size = new System.Drawing.Size(120, 18);
            this.uiMarkLabel12.TabIndex = 1;
            this.uiMarkLabel12.Text = "是否400ms切换: ";
            this.uiMarkLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // swIsTurnOnOff
            // 
            this.swIsTurnOnOff.ActiveText = "是";
            this.swIsTurnOnOff.Font = new System.Drawing.Font("宋体", 10F);
            this.swIsTurnOnOff.InActiveText = "否";
            this.swIsTurnOnOff.Location = new System.Drawing.Point(132, 169);
            this.swIsTurnOnOff.MinimumSize = new System.Drawing.Size(1, 1);
            this.swIsTurnOnOff.Name = "swIsTurnOnOff";
            this.swIsTurnOnOff.Size = new System.Drawing.Size(57, 24);
            this.swIsTurnOnOff.TabIndex = 0;
            this.swIsTurnOnOff.Text = "uiSwitch1";
            this.swIsTurnOnOff.ActiveChanged += new System.EventHandler(this.swIsTurnOnOff_ActiveChanged);
            // 
            // gpAnimation
            // 
            this.gpAnimation.Controls.Add(this.rbComplexComingHome);
            this.gpAnimation.Controls.Add(this.rbComplexLeavingHome);
            this.gpAnimation.Controls.Add(this.rbSimpleComingHome);
            this.gpAnimation.Controls.Add(this.rbSimpleLeavingHome);
            this.gpAnimation.Controls.Add(this.rbLampShowAbort);
            this.gpAnimation.Location = new System.Drawing.Point(19, 398);
            this.gpAnimation.Name = "gpAnimation";
            this.gpAnimation.Size = new System.Drawing.Size(272, 199);
            this.gpAnimation.TabIndex = 2;
            this.gpAnimation.TabStop = false;
            this.gpAnimation.Text = "动画";
            // 
            // rbComplexComingHome
            // 
            this.rbComplexComingHome.Checked = false;
            this.rbComplexComingHome.GroupName = "Animation";
            this.rbComplexComingHome.Location = new System.Drawing.Point(6, 164);
            this.rbComplexComingHome.Name = "rbComplexComingHome";
            this.rbComplexComingHome.Size = new System.Drawing.Size(247, 30);
            this.rbComplexComingHome.TabIndex = 0;
            this.rbComplexComingHome.TextValue = "复杂闭锁";
            // 
            // rbComplexLeavingHome
            // 
            this.rbComplexLeavingHome.Checked = false;
            this.rbComplexLeavingHome.GroupName = "Animation";
            this.rbComplexLeavingHome.Location = new System.Drawing.Point(6, 128);
            this.rbComplexLeavingHome.Name = "rbComplexLeavingHome";
            this.rbComplexLeavingHome.Size = new System.Drawing.Size(247, 30);
            this.rbComplexLeavingHome.TabIndex = 0;
            this.rbComplexLeavingHome.TextValue = "复杂解锁";
            // 
            // rbSimpleComingHome
            // 
            this.rbSimpleComingHome.Checked = false;
            this.rbSimpleComingHome.GroupName = "Animation";
            this.rbSimpleComingHome.Location = new System.Drawing.Point(6, 92);
            this.rbSimpleComingHome.Name = "rbSimpleComingHome";
            this.rbSimpleComingHome.Size = new System.Drawing.Size(247, 30);
            this.rbSimpleComingHome.TabIndex = 0;
            this.rbSimpleComingHome.TextValue = "简单闭锁";
            // 
            // rbSimpleLeavingHome
            // 
            this.rbSimpleLeavingHome.Checked = false;
            this.rbSimpleLeavingHome.GroupName = "Animation";
            this.rbSimpleLeavingHome.Location = new System.Drawing.Point(6, 56);
            this.rbSimpleLeavingHome.Name = "rbSimpleLeavingHome";
            this.rbSimpleLeavingHome.Size = new System.Drawing.Size(247, 30);
            this.rbSimpleLeavingHome.TabIndex = 0;
            this.rbSimpleLeavingHome.TextValue = "简单解锁";
            // 
            // rbLampShowAbort
            // 
            this.rbLampShowAbort.Checked = true;
            this.rbLampShowAbort.GroupName = "Animation";
            this.rbLampShowAbort.Location = new System.Drawing.Point(6, 20);
            this.rbLampShowAbort.Name = "rbLampShowAbort";
            this.rbLampShowAbort.Size = new System.Drawing.Size(110, 30);
            this.rbLampShowAbort.TabIndex = 0;
            this.rbLampShowAbort.TextValue = "打断动画";
            // 
            // gpError
            // 
            this.gpError.Controls.Add(this.lblLeftBTail);
            this.gpError.Controls.Add(this.lblLeftATail);
            this.gpError.Controls.Add(this.lblRightBTail);
            this.gpError.Controls.Add(this.lblRightATail);
            this.gpError.Controls.Add(this.lblRightAStop);
            this.gpError.Controls.Add(this.lblLeftAStop);
            this.gpError.Controls.Add(this.lblRightBTurn);
            this.gpError.Controls.Add(this.lblRightATurn);
            this.gpError.Controls.Add(this.lblLeftBTurn);
            this.gpError.Controls.Add(this.lblLeftATurn);
            this.gpError.Controls.Add(this.uiMarkLabel9);
            this.gpError.Controls.Add(this.uiMarkLabel8);
            this.gpError.Controls.Add(this.uiMarkLabel5);
            this.gpError.Controls.Add(this.uiMarkLabel11);
            this.gpError.Controls.Add(this.uiMarkLabel7);
            this.gpError.Controls.Add(this.uiMarkLabel4);
            this.gpError.Controls.Add(this.uiMarkLabel10);
            this.gpError.Controls.Add(this.uiMarkLabel6);
            this.gpError.Controls.Add(this.uiMarkLabel3);
            this.gpError.Controls.Add(this.uiMarkLabel2);
            this.gpError.Location = new System.Drawing.Point(306, 46);
            this.gpError.Name = "gpError";
            this.gpError.Size = new System.Drawing.Size(491, 542);
            this.gpError.TabIndex = 2;
            this.gpError.TabStop = false;
            this.gpError.Text = "故障信号";
            // 
            // lblLeftBTail
            // 
            this.lblLeftBTail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblLeftBTail.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblLeftBTail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblLeftBTail.Location = new System.Drawing.Point(198, 181);
            this.lblLeftBTail.Name = "lblLeftBTail";
            this.lblLeftBTail.Size = new System.Drawing.Size(282, 23);
            this.lblLeftBTail.TabIndex = 2;
            this.lblLeftBTail.Text = "uiLabel1";
            this.lblLeftBTail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLeftATail
            // 
            this.lblLeftATail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblLeftATail.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblLeftATail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblLeftATail.Location = new System.Drawing.Point(198, 85);
            this.lblLeftATail.Name = "lblLeftATail";
            this.lblLeftATail.Size = new System.Drawing.Size(282, 23);
            this.lblLeftATail.TabIndex = 2;
            this.lblLeftATail.Text = "uiLabel1";
            this.lblLeftATail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRightBTail
            // 
            this.lblRightBTail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRightBTail.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblRightBTail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblRightBTail.Location = new System.Drawing.Point(198, 485);
            this.lblRightBTail.Name = "lblRightBTail";
            this.lblRightBTail.Size = new System.Drawing.Size(282, 23);
            this.lblRightBTail.TabIndex = 2;
            this.lblRightBTail.Text = "uiLabel1";
            this.lblRightBTail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRightATail
            // 
            this.lblRightATail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRightATail.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblRightATail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblRightATail.Location = new System.Drawing.Point(198, 395);
            this.lblRightATail.Name = "lblRightATail";
            this.lblRightATail.Size = new System.Drawing.Size(282, 23);
            this.lblRightATail.TabIndex = 2;
            this.lblRightATail.Text = "uiLabel1";
            this.lblRightATail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRightAStop
            // 
            this.lblRightAStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRightAStop.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblRightAStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblRightAStop.Location = new System.Drawing.Point(198, 368);
            this.lblRightAStop.Name = "lblRightAStop";
            this.lblRightAStop.Size = new System.Drawing.Size(282, 23);
            this.lblRightAStop.TabIndex = 2;
            this.lblRightAStop.Text = "uiLabel1";
            this.lblRightAStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLeftAStop
            // 
            this.lblLeftAStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblLeftAStop.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblLeftAStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblLeftAStop.Location = new System.Drawing.Point(198, 59);
            this.lblLeftAStop.Name = "lblLeftAStop";
            this.lblLeftAStop.Size = new System.Drawing.Size(282, 23);
            this.lblLeftAStop.TabIndex = 2;
            this.lblLeftAStop.Text = "uiLabel1";
            this.lblLeftAStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRightBTurn
            // 
            this.lblRightBTurn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRightBTurn.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblRightBTurn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblRightBTurn.Location = new System.Drawing.Point(198, 457);
            this.lblRightBTurn.Name = "lblRightBTurn";
            this.lblRightBTurn.Size = new System.Drawing.Size(282, 23);
            this.lblRightBTurn.TabIndex = 2;
            this.lblRightBTurn.Text = "uiLabel1";
            this.lblRightBTurn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRightATurn
            // 
            this.lblRightATurn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRightATurn.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblRightATurn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblRightATurn.Location = new System.Drawing.Point(198, 342);
            this.lblRightATurn.Name = "lblRightATurn";
            this.lblRightATurn.Size = new System.Drawing.Size(282, 23);
            this.lblRightATurn.TabIndex = 2;
            this.lblRightATurn.Text = "uiLabel1";
            this.lblRightATurn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLeftBTurn
            // 
            this.lblLeftBTurn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblLeftBTurn.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblLeftBTurn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblLeftBTurn.Location = new System.Drawing.Point(198, 153);
            this.lblLeftBTurn.Name = "lblLeftBTurn";
            this.lblLeftBTurn.Size = new System.Drawing.Size(282, 23);
            this.lblLeftBTurn.TabIndex = 2;
            this.lblLeftBTurn.Text = "uiLabel1";
            this.lblLeftBTurn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLeftATurn
            // 
            this.lblLeftATurn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblLeftATurn.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblLeftATurn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblLeftATurn.Location = new System.Drawing.Point(198, 34);
            this.lblLeftATurn.Name = "lblLeftATurn";
            this.lblLeftATurn.Size = new System.Drawing.Size(282, 23);
            this.lblLeftATurn.TabIndex = 2;
            this.lblLeftATurn.Text = "uiLabel1";
            this.lblLeftATurn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel9
            // 
            this.uiMarkLabel9.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel9.Location = new System.Drawing.Point(17, 485);
            this.uiMarkLabel9.Name = "uiMarkLabel9";
            this.uiMarkLabel9.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel9.Size = new System.Drawing.Size(164, 23);
            this.uiMarkLabel9.TabIndex = 1;
            this.uiMarkLabel9.Text = "右侧B灯位置灯: ";
            this.uiMarkLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel8
            // 
            this.uiMarkLabel8.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel8.Location = new System.Drawing.Point(17, 395);
            this.uiMarkLabel8.Name = "uiMarkLabel8";
            this.uiMarkLabel8.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel8.Size = new System.Drawing.Size(164, 23);
            this.uiMarkLabel8.TabIndex = 1;
            this.uiMarkLabel8.Text = "右侧A灯位置灯: ";
            this.uiMarkLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel5
            // 
            this.uiMarkLabel5.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel5.Location = new System.Drawing.Point(17, 457);
            this.uiMarkLabel5.Name = "uiMarkLabel5";
            this.uiMarkLabel5.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel5.Size = new System.Drawing.Size(164, 23);
            this.uiMarkLabel5.TabIndex = 1;
            this.uiMarkLabel5.Text = "右侧B灯转向灯: ";
            this.uiMarkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel11
            // 
            this.uiMarkLabel11.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel11.Location = new System.Drawing.Point(17, 181);
            this.uiMarkLabel11.Name = "uiMarkLabel11";
            this.uiMarkLabel11.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel11.Size = new System.Drawing.Size(164, 23);
            this.uiMarkLabel11.TabIndex = 1;
            this.uiMarkLabel11.Text = "左侧B灯位置灯: ";
            this.uiMarkLabel11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel7
            // 
            this.uiMarkLabel7.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel7.Location = new System.Drawing.Point(17, 368);
            this.uiMarkLabel7.Name = "uiMarkLabel7";
            this.uiMarkLabel7.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel7.Size = new System.Drawing.Size(164, 23);
            this.uiMarkLabel7.TabIndex = 1;
            this.uiMarkLabel7.Text = "右侧A灯制动灯: ";
            this.uiMarkLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel4
            // 
            this.uiMarkLabel4.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel4.Location = new System.Drawing.Point(17, 342);
            this.uiMarkLabel4.Name = "uiMarkLabel4";
            this.uiMarkLabel4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel4.Size = new System.Drawing.Size(164, 23);
            this.uiMarkLabel4.TabIndex = 1;
            this.uiMarkLabel4.Text = "右侧A灯转向灯: ";
            this.uiMarkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel10
            // 
            this.uiMarkLabel10.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel10.Location = new System.Drawing.Point(17, 85);
            this.uiMarkLabel10.Name = "uiMarkLabel10";
            this.uiMarkLabel10.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel10.Size = new System.Drawing.Size(164, 23);
            this.uiMarkLabel10.TabIndex = 1;
            this.uiMarkLabel10.Text = "左侧A灯位置灯: ";
            this.uiMarkLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel6
            // 
            this.uiMarkLabel6.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel6.Location = new System.Drawing.Point(17, 59);
            this.uiMarkLabel6.Name = "uiMarkLabel6";
            this.uiMarkLabel6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel6.Size = new System.Drawing.Size(164, 23);
            this.uiMarkLabel6.TabIndex = 1;
            this.uiMarkLabel6.Text = "左侧A灯制动灯: ";
            this.uiMarkLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel3
            // 
            this.uiMarkLabel3.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel3.Location = new System.Drawing.Point(17, 153);
            this.uiMarkLabel3.Name = "uiMarkLabel3";
            this.uiMarkLabel3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel3.Size = new System.Drawing.Size(164, 23);
            this.uiMarkLabel3.TabIndex = 1;
            this.uiMarkLabel3.Text = "左侧B灯转向灯: ";
            this.uiMarkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel2.Location = new System.Drawing.Point(17, 34);
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel2.Size = new System.Drawing.Size(164, 23);
            this.uiMarkLabel2.TabIndex = 1;
            this.uiMarkLabel2.Text = "左侧A灯转向灯: ";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmSgm3582RearLamp
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.gpAnimation);
            this.Controls.Add(this.gpTurn);
            this.Controls.Add(this.gpError);
            this.Controls.Add(this.gpTail);
            this.Controls.Add(this.uiMarkLabel1);
            this.Controls.Add(this.uiSwitch1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmSgm3582RearLamp";
            this.Text = "SGM358-2";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 832, 528);
            this.gpTail.ResumeLayout(false);
            this.gpTurn.ResumeLayout(false);
            this.gpAnimation.ResumeLayout(false);
            this.gpError.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UISwitch uiSwitch1;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private System.Windows.Forms.GroupBox gpTail;
        private UCRadioButton rbTailHighLightOn;
        private UCRadioButton rbTailOff;
        private System.Windows.Forms.GroupBox gpTurn;
        private UCRadioButton rbTurnSwipeTurn;
        private UCRadioButton rbTurnOff;
        private UCRadioButton rbTurnHoldOnLight;
        private UCRadioButton rbTurnHighLightOn;
        private System.Windows.Forms.GroupBox gpAnimation;
        private UCRadioButton rbComplexLeavingHome;
        private UCRadioButton rbSimpleComingHome;
        private UCRadioButton rbSimpleLeavingHome;
        private UCRadioButton rbLampShowAbort;
        private UCRadioButton rbComplexComingHome;
        private System.Windows.Forms.GroupBox gpError;
        private Sunny.UI.UILabel lblLeftBTail;
        private Sunny.UI.UILabel lblLeftATail;
        private Sunny.UI.UILabel lblRightBTail;
        private Sunny.UI.UILabel lblRightATail;
        private Sunny.UI.UILabel lblRightAStop;
        private Sunny.UI.UILabel lblLeftAStop;
        private Sunny.UI.UILabel lblRightBTurn;
        private Sunny.UI.UILabel lblRightATurn;
        private Sunny.UI.UILabel lblLeftBTurn;
        private Sunny.UI.UILabel lblLeftATurn;
        private Sunny.UI.UIMarkLabel uiMarkLabel9;
        private Sunny.UI.UIMarkLabel uiMarkLabel8;
        private Sunny.UI.UIMarkLabel uiMarkLabel5;
        private Sunny.UI.UIMarkLabel uiMarkLabel11;
        private Sunny.UI.UIMarkLabel uiMarkLabel7;
        private Sunny.UI.UIMarkLabel uiMarkLabel4;
        private Sunny.UI.UIMarkLabel uiMarkLabel10;
        private Sunny.UI.UIMarkLabel uiMarkLabel6;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private UCRadioButton rbTailNormal;
        private Sunny.UI.UIMarkLabel uiMarkLabel12;
        private Sunny.UI.UISwitch swIsTurnOnOff;
    }
}