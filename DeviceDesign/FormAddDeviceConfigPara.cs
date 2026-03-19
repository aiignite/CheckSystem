using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StateMachine;

namespace DeviceDesign
{
    public partial class FormAddDeviceConfigPara : Form
    {
        public FormAddDeviceConfigPara()
        {
            InitializeComponent();
            OtherPropertiesInit();
        }

        private void FormAddDeviceConfigPara_Load(object sender, EventArgs e)
        {
            ControlsInit();
            ToolStripInit();
        }

        private void OtherPropertiesInit()
        {
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
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

            ToolStripButton toolStripButtonEdit = new ToolStripButton();
            ToolStripButton toolStripButtonCancel = new ToolStripButton();
            ToolStripButton toolStripButtonSave = new ToolStripButton();
            // 
            // toolStrip
            // 
            toolStrip.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            toolStrip.ImageScalingSize = new Size(32, 32);
            toolStrip.Items.AddRange(new ToolStripItem[] {
            toolStripButtonSave,
            toolStripButtonCancel,
            });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";

            toolStripButtonSave.Image = Properties.Resources.Save;
            toolStripButtonSave.ImageTransparentColor = Color.Magenta;
            toolStripButtonSave.Name = "toolStripButtonSave";
            toolStripButtonSave.Size = new Size(78, 36);
            toolStripButtonSave.Text = "保存";
            toolStripButtonSave.Click += ToolStripButtonSave_Click;


            toolStripButtonCancel.Image = Properties.Resources.delete;
            toolStripButtonCancel.ImageTransparentColor = Color.Magenta;
            toolStripButtonCancel.Name = "toolStripButtonCancel";
            toolStripButtonCancel.Size = new Size(78, 36);
            toolStripButtonCancel.Text = "取消";
            toolStripButtonCancel.Click += ToolStripButtonCancel_Click;

            Controls.Add(toolStrip);
        }

        private void ToolStripButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            var lst = ClassComm.DeviceConfig.Paras.ToList();
            var item = lst.Find(t => t.Name == ltName.Text && t.ProcessNo == lcmbProcessNo.Text);
            if (item != null)
            {
                MessageBox.Show("已经包含该控制器的当前属性");
                return;
            }
            DeviceConfigPara para = new DeviceConfigPara();
            para.Name = ltName.Text;
            para.ControllerField = lcmbControllerField.Text;
            para.ControllerFieldOffset = ltOffset.Text;
            para.ControlName = lcmbControlName.Text;
            para.DataType = lcmbDataType.Text;
            para.Max = ltMax.Text;
            para.Min = ltMin.Text;
            para.OkFormat = lcmbOkformat.Text;
            para.ProcessNo = lcmbProcessNo.Text;
            para.Unit = ltUnit.Text;
            para.Value = ltValue.Text;
            lst.Add(para);
            ClassComm.DeviceConfig.Paras = lst.ToArray();
            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

            Close();
        }
        #endregion

        #region Controls

        UserControls.LabelCombox lcmbProcessNo = new UserControls.LabelCombox();
        UserControls.LabelCombox lcmbDataType = new UserControls.LabelCombox();
        UserControls.LabelCombox lcmbOkformat = new UserControls.LabelCombox();
        UserControls.LabelCombox lcmbControlName = new UserControls.LabelCombox();
        UserControls.LabelCombox lcmbControllerField = new UserControls.LabelCombox();


        UserControls.LabelText ltName = new UserControls.LabelText() { LabelString = "名称"};
        UserControls.LabelText ltValue = new UserControls.LabelText() { LabelString = "值"};

        UserControls.LabelText ltMin = new UserControls.LabelText() { LabelString = "最小值"};
        UserControls.LabelText ltMax = new UserControls.LabelText() { LabelString = "最大值"};
        UserControls.LabelText ltUnit = new UserControls.LabelText() { LabelString = "单位"};

        UserControls.LabelText ltOffset = new UserControls.LabelText() {LabelString = "偏移计算" };



        private void ControlsInit()
        {
            lcmbProcessNo.Name = "工序编号";
            lcmbProcessNo.label.Text = lcmbProcessNo.Name;
            lcmbProcessNo.Dock = DockStyle.Top;
            var processes = ClassComm.DeviceConfig.Processes;
            lcmbProcessNo.comboBox.Items.AddRange(processes.Select(t => t.No).ToArray());
            lcmbProcessNo.ListText = processes.Select(t => t.No).ToList();
            

            lcmbDataType.Name = "数据类型";
            lcmbDataType.label.Text = lcmbDataType.Name;
            var lstDataTypes = ClassComm.LstDataTypes;
            lcmbDataType.comboBox.Items.AddRange(lstDataTypes.ToArray());
            lcmbDataType.ListText = lstDataTypes;
            lcmbDataType.Dock = DockStyle.Top;
            lcmbDataType.comboBox.SelectedIndexChanged += DataTypeComboBox_SelectedIndexChanged;

            lcmbOkformat.Name = "字符串适配类型";
            lcmbOkformat.label.Text = lcmbOkformat.Name;
            List<string> lstOk = new List<string>() { "=", "like" };
            lcmbOkformat.comboBox.Items.AddRange(lstOk.ToArray());
            lcmbOkformat.ListText = lstOk;
            lcmbOkformat.Dock = DockStyle.Top;

            lcmbControlName.Name = "显示控件名";
            lcmbControlName.label.Text = lcmbControlName.Name;
            lcmbControlName.Dock = DockStyle.Top;
            var controls = ClassComm.DeviceConfig.Controls;
            lcmbControlName.comboBox.Items.AddRange(controls.Select(t => t.Name).ToArray());
            lcmbControlName.ListText = controls.Select(t => t.Name).ToList();

            lcmbControllerField.Name = "对应控制器变量";
            lcmbControllerField.label.Text = lcmbControllerField.Name;
            lcmbControllerField.Dock = DockStyle.Top;
            var controllers = ClassComm.DeviceConfig.Controllers;
            foreach(var controller in controllers)
            {
                var fields = ClassComm.LstControllers.Find(t => t.Name == controller.Type).LstFields;
                lcmbControllerField.comboBox.Items.AddRange(fields.Select(t=>controller.Name+".Field."+t).ToArray());
                lcmbControllerField.ListText.AddRange(fields.Select(t => controller.Name + ".Field." + t));
            }

            Controls.Add(ltOffset);
            Controls.Add(lcmbControllerField);
            Controls.Add(lcmbControlName);
            Controls.Add(ltUnit);
            Controls.Add(ltMax);
            Controls.Add(ltMin);
            Controls.Add(ltValue);
            Controls.Add(lcmbOkformat);
            Controls.Add(lcmbDataType);
            Controls.Add(lcmbProcessNo);
            Controls.Add(ltName);

        }

        private void DataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lcmbDataType.Text.Equals("string"))
            {
                lcmbOkformat.Enabled = true;
                ltValue.Enabled = true;
                ltMin.Enabled = false;
                ltMax.Enabled = false;
                ltUnit.Enabled = false;
            }
            else
            {
                lcmbOkformat.Enabled = false;
                ltValue.Enabled = true;
                ltMin.Enabled = true;
                ltMax.Enabled = true;
                ltUnit.Enabled = true;
            }
        }

 


        #endregion
    }
}
