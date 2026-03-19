using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StateMachine;

namespace DeviceDesign
{
    public partial class FormAddDeviceConfigWorkStation : Form
    {
        public FormAddDeviceConfigWorkStation()
        {
            InitializeComponent();
            OtherPropertiesInit();
        }

        private void FormAddDeviceConfigWorkStation_Load(object sender, EventArgs e)
        {
            ControlsInit();
            ToolStripInit();
        }

        private void OtherPropertiesInit()
        {
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            Size = new Size(800, 900);
        }

        #region toolstrip
        private ToolStrip _toolStrip;
        private void ToolStripInit()
        {
            _toolStrip = new ToolStrip();
            _toolStrip.Dock = DockStyle.Top;
            _toolStrip.Size = new Size(500, 40);
            _toolStrip.BackColor = Color.LightBlue;

            ToolStripButton toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            ToolStripButton toolStripButtonCancel = new ToolStripButton();
            ToolStripButton toolStripButtonSave = new ToolStripButton();
            // 
            // toolStrip
            // 
            this._toolStrip.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripButtonSave,
            toolStripButtonCancel,
            });
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "toolStrip";

            toolStripButtonSave.Image = Properties.Resources.Save;
            toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSave.Name = "toolStripButtonSave";
            toolStripButtonSave.Size = new System.Drawing.Size(78, 36);
            toolStripButtonSave.Text = "保存";
            toolStripButtonSave.Click += ToolStripButtonSave_Click;


            toolStripButtonCancel.Image = Properties.Resources.delete;
            toolStripButtonCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonCancel.Name = "toolStripButtonCancel";
            toolStripButtonCancel.Size = new System.Drawing.Size(78, 36);
            toolStripButtonCancel.Text = "取消";
            toolStripButtonCancel.Click += ToolStripButtonCancel_Click;

            this.Controls.Add(this._toolStrip);
        }

        private void ToolStripButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            var lst = ClassComm.DeviceConfig.WorkStations.ToList();
            var item = lst.Find(t => t.Name == _ltName.Text);
            if (item != null)
            {
                MessageBox.Show(@"已经包含该工站");
                return;
            }
            var workstation = new DeviceConfigWorkStation();
            workstation.Name = _ltName.Text;
            //workstation.InitStatusUnit = _ltInitStatusUnit.Text;
            //workstation.CurrentStatusUnit = _ltCurrentStatusUnit.Text;
            lst.Add(workstation);
            ClassComm.DeviceConfig.WorkStations = lst.ToArray();
            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

            Close();
        }
        #endregion

        #region Controls

        private readonly UserControls.LabelText _ltName = new UserControls.LabelText { LabelString = "名称" };
        private readonly UserControls.LabelText _ltInitStatusUnit = new UserControls.LabelText { LabelString = "初始化状态" };
        private readonly UserControls.LabelText _ltCurrentStatusUnit = new UserControls.LabelText { LabelString = "当前状态" };

        private void ControlsInit()
        {
            _ltInitStatusUnit.textBox.ReadOnly = true;
            _ltInitStatusUnit.textBox.Text = StateMachineHelper.EDefaultStateUnits.Idle.ToString().ToUpper();

            _ltCurrentStatusUnit.textBox.ReadOnly = true;
            _ltCurrentStatusUnit.textBox.Text = StateMachineHelper.EDefaultStateUnits.Idle.ToString().ToUpper();

            Controls.Add(_ltCurrentStatusUnit);
            Controls.Add(_ltInitStatusUnit);
            Controls.Add(_ltName);
        }

        #endregion
    }
}
