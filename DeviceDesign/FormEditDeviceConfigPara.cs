using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StateMachine;

namespace DeviceDesign
{
    public partial class FormEditDeviceConfigPara : Form
    {
        private DeviceConfigPara _para = new DeviceConfigPara();
        public FormEditDeviceConfigPara()
        {
            InitializeComponent();
            OtherPropertiesInit();
        }

        public FormEditDeviceConfigPara(object obj)
        {
            InitializeComponent();
            OtherPropertiesInit();
            _para = (DeviceConfigPara)obj;
        }

        private void FormEditDeviceConfigPara_Load(object sender, EventArgs e)
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
            var lst = ClassComm.DeviceConfig.Paras.ToList();
            var item = lst.Find(t => t.Name == _para.Name && t.ProcessNo == lcmbProcessNo.Text);
            var index = lst.IndexOf(item);
            if (item != null)
            {
                lst.Remove(item);
            }
            else
            {
                Close();
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
            lst.Insert(index,para);
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


        UserControls.LabelText ltName = new UserControls.LabelText() { LabelString = "名称" };
        UserControls.LabelText ltValue = new UserControls.LabelText() { LabelString = "值" };

        UserControls.LabelText ltMin = new UserControls.LabelText() { LabelString = "最小值" };
        UserControls.LabelText ltMax = new UserControls.LabelText() { LabelString = "最大值" };
        UserControls.LabelText ltUnit = new UserControls.LabelText() { LabelString = "单位" };

        UserControls.LabelText ltOffset = new UserControls.LabelText() { LabelString = "偏移计算" };



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
            foreach (var controller in controllers)
            {
                var fields = ClassComm.LstControllers.Find(t => t.Name == controller.Type).LstFields;
                lcmbControllerField.comboBox.Items.AddRange(fields.Select(t => controller.Name + ".Field." + t).ToArray());
                lcmbControllerField.ListText.AddRange(fields.Select(t => controller.Name + ".Field." + t));
            }

            ltMax.Text = _para.Max;
            ltMin.Text = _para.Min;
            ltName.Text = _para.Name;
            ltOffset.Text = _para.ControllerFieldOffset;
            ltUnit.Text = _para.Unit;
            ltValue.Text = _para.Value;

            lcmbControllerField.Text = _para.ControllerField;
            lcmbControlName.Text = _para.ControlName;
            lcmbDataType.Text = _para.DataType;
            lcmbOkformat.Text = _para.OkFormat;
            lcmbProcessNo.Text = _para.ProcessNo;

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
            if (lcmbDataType.Text.Equals("string"))
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
