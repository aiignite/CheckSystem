using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StateMachine;

namespace DeviceDesign
{
    public partial class FormEditDeviceConfigWorkStation : Form
    {
        private DeviceConfigWorkStation _workStation = new DeviceConfigWorkStation();
        public FormEditDeviceConfigWorkStation()
        {
            InitializeComponent();
            OtherPropertiesInit();
        }

        public FormEditDeviceConfigWorkStation(object obj)
        {
            InitializeComponent();
            OtherPropertiesInit();
            _workStation = (DeviceConfigWorkStation)obj;
        }

        private void FormEditDeviceConfigWorkStation_Load(object sender, EventArgs e)
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
        private ToolStrip toolStrip;
        private void ToolStripInit()
        {
            toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.Top;
            toolStrip.Size = new Size(500, 40);
            toolStrip.BackColor = Color.LightBlue;

            ToolStripButton toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            ToolStripButton toolStripButtonCancel = new ToolStripButton();
            ToolStripButton toolStripButtonSave = new ToolStripButton();
            // 
            // toolStrip
            // 
            this.toolStrip.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripButtonSave,
            toolStripButtonCancel,
            });
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";

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

            this.Controls.Add(this.toolStrip);
        }

        private void ToolStripButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            var lst = ClassComm.DeviceConfig.WorkStations.ToList();
            var item = lst.Find(t => t.Name == _workStation.Name );
            var index = lst.IndexOf(item);
            if (item != null)
            {
                lst.Remove(item);
            }
            else
            {
                Close();
            }
            DeviceConfigWorkStation workstation = new DeviceConfigWorkStation();
            workstation.Name = ltName.Text;
            //workstation.InitStatusUnit = ltInitStatusUnit.Text;
            //workstation.CurrentStatusUnit = ltCurrentStatusUnit.Text;
            lst.Insert(index,workstation);
            ClassComm.DeviceConfig.WorkStations = lst.ToArray();
            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);
            Close();
        }
        #endregion

        #region Controls


        UserControls.LabelText ltName = new UserControls.LabelText() { LabelString = "名称" };
        UserControls.LabelText ltInitStatusUnit = new UserControls.LabelText() { LabelString = "初始化状态" };
        UserControls.LabelText ltCurrentStatusUnit = new UserControls.LabelText() { LabelString = "当前状态" };

        private void ControlsInit()
        {
            ltName.Text=_workStation.Name;
            ltInitStatusUnit.textBox.ReadOnly = true;
            ltInitStatusUnit.textBox.Text = StateMachineHelper.EDefaultStateUnits.Idle.ToString().ToUpper();

            ltCurrentStatusUnit.textBox.ReadOnly = true;
            ltCurrentStatusUnit.textBox.Text = StateMachineHelper.EDefaultStateUnits.Idle.ToString().ToUpper();

            Controls.Add(ltCurrentStatusUnit);
            Controls.Add(ltInitStatusUnit);
            Controls.Add(ltName);
        }


        #endregion
    }
}
