using DeviceDesign.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using StateMachine;

namespace DeviceDesign
{
    public partial class FormDesignNew : Form
    {
        #region 变量定义

        public string StrClassName = "";
        public string StrOperateMode = "";

        private List<string> _lstControllerTypes = new List<string>();

        /// <summary>
        /// 用于存储控件
        /// </summary>
        private readonly List<Control> _lstControl = new List<Control>();

        /// <summary>
        /// 用于存储ComboBox内容
        /// </summary>
        private readonly Dictionary<string, List<string>> _lstComboItems = new Dictionary<string, List<string>>();

        private void LstComboItemsInit()
        {
            var lstConditions = ClassComm.DeviceConfig.Conditions.Select(item => item.Name).ToList();

            _lstComboItems.Add("Conditions", lstConditions);

            var lstControls = ClassComm.DeviceConfig.Controls.Select(item => item.Name).ToList();
            _lstComboItems.Add("Controls", lstControls);

            var lstControllers = ClassComm.DeviceConfig.Controllers.Select(item => item.Type).ToList();
            _lstComboItems.Add("Controllers", lstControllers);

            var lstParas = ClassComm.DeviceConfig.Paras.Select(item => item.Name).ToList();
            _lstComboItems.Add("Paras", lstParas);

            var lstParts = ClassComm.DeviceConfig.Parts.Select(item => item.Name).ToList();
            _lstComboItems.Add("Parts", lstParts);

            var lstProcesses = ClassComm.DeviceConfig.Processes.Select(item => item.Name).ToList();
            _lstComboItems.Add("Processes", lstProcesses);

            var lstStatusUnits = ClassComm.DeviceConfig.StatusUnits.Select(item => item.Name).ToList();
            _lstComboItems.Add("StatusUnits", lstStatusUnits);

            var lstWorkStations = ClassComm.DeviceConfig.WorkStations.Select(item => item.Name).ToList();
            _lstComboItems.Add("WorkStations", lstWorkStations);

            var lstControlTypes = new List<string>
            {
                "Label",
                "TextBox",
                "DataGrid",
                "LTextBox",
                "NiImageViewer",
                "UserTitle"
            };

            _lstComboItems.Add("ControlTypes", lstControlTypes);

            var lstDataTypes = new List<string>
            {
                "int",
                "double",
                "string",
                "bool"
            };

            _lstComboItems.Add("DataTypes", lstDataTypes);

            var lstOkFormats = new List<string>
            {
                "=",
                "like",              
            };

            _lstComboItems.Add("OkFormats", lstOkFormats);

            var lstControllerTypes =
                (from type in ClassComm.DllAsmb.GetExportedTypes()
                 where type.BaseType != null && type.BaseType.Name.Contains("ControllerBase")
                 select type.Name).ToList();

            _lstComboItems.Add("ControllerTypes", lstControllerTypes);

            var lst = new List<string>();

            foreach (var controllerName in lstControllers)
            {
                var typeName = ClassComm.DllAsmb.GetType("Controller." + controllerName);

                if (typeName == null)
                    continue;

                var methods = typeName.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                var fields = typeName.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                var properties =
                    typeName.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                lst.AddRange(
                    fields.Select(
                        field =>
                            controllerName + ".Field." +
                            field.ToString().Substring(field.ToString().IndexOf(" ", StringComparison.Ordinal) + 1)));
                lst.AddRange(from method in methods
                             let para = method.GetParameters().ToList()
                             let strPara = string.Join(",", para)
                             select controllerName + ".Method." + method.Name + "(" + strPara + ")");
            }

            _lstComboItems.Add("ControllerField", lst);
        }

        private ToolStrip _toolStrip;

        /// <summary>
        /// 接收模型传入和按钮类型传入
        /// </summary>
        private readonly object _devicePara;

        private readonly string _operateMode;

        #endregion

        public FormDesignNew()
        {
            InitializeComponent();
        }

        public FormDesignNew(object devicepara, string operatemode)
        {
            InitializeComponent();
            _devicePara = devicepara;
            _operateMode = operatemode;
        }

        #region Overide Onload

        protected override void OnLoad(EventArgs e)
        {
            Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            LstComboItemsInit();
            ControlsInit();
            ToolStripInit();

            if (_operateMode == "DEL")
                ToolStripButtonSave_Click(null, null);

            base.OnLoad(e);
        }

        #endregion

        #region 界面控件

        private void ControlsInit()
        {
            try
            {
                foreach (var item in _devicePara.GetType().GetProperties().Reverse())
                {
                    //不包含displayName属性的使用text
                    if (item.GetCustomAttribute<DisplayNameAttribute>() == null)
                    {
                        var lt = new UserControls.LabelText
                        {
                            Name = item.Name,
                            LabelString = item.Name,
                            Text =
                                item.GetValue(_devicePara, null) == null
                                    ? ""
                                    : item.GetValue(_devicePara, null).ToString(),
                            Dock = DockStyle.Top
                        };
                        Controls.Add(lt);
                        _lstControl.Add(lt);
                    }
                    else
                    {
                        var strDisplayName = item.GetCustomAttribute<DisplayNameAttribute>().DisplayName;

                        var displayName = ClassComm.AnalysisDisplayName(strDisplayName);

                        switch (displayName.ControlType)
                        {
                            case "LComboBox":
                                var lcmb = new UserControls.LabelCombox
                                {
                                    Name = item.Name,
                                    LabelString = displayName.LabelString,
                                    Text =
                                        item.GetValue(_devicePara, null) == null
                                            ? ""
                                            : item.GetValue(_devicePara, null).ToString()
                                };
                                try
                                {

                                    var lst = _lstComboItems[displayName.SearchString];
                                    foreach (var type in lst)
                                        lcmb.comboBox.Items.Add(type);
                                    lcmb.ListText = lst;
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }

                                lcmb.comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

                                lcmb.Dock = DockStyle.Top;
                                Controls.Add(lcmb);
                                _lstControl.Add(lcmb);
                                break;

                            case "LDateTimePicker":
                                var ldtp = new UserControls.LabelDateTimePicker
                                {
                                    label = { Text = displayName.LabelString },
                                    Name = item.Name,
                                    Text = item.GetValue(_devicePara, null).ToString(),
                                    Dock = DockStyle.Fill
                                };
                                Controls.Add(ldtp);
                                _lstControl.Add(ldtp);
                                break;

                            default:
                                var lt = new UserControls.LabelText
                                {
                                    Name = item.Name,
                                    LabelString = displayName.LabelString,
                                    Text =
                                        item.GetValue(_devicePara, null) == null
                                            ? string.Empty
                                            : item.GetValue(_devicePara, null).ToString(),
                                    Dock = DockStyle.Top
                                };
                                Controls.Add(lt);
                                _lstControl.Add(lt);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_devicePara.GetType().Name != "DeviceConfigCondition")
                return;
            var ltName = (UserControls.LabelText) _lstControl.Find(t => t.Name == "Name");
            var lcmbSourceSu = (UserControls.LabelCombox) _lstControl.Find(t => t.Name == "SourceSuName");
            var lcmbTargetSu = (UserControls.LabelCombox) _lstControl.Find(t => t.Name == "TargetSuName");

            ltName.Text = lcmbSourceSu.Text + @"_" + lcmbTargetSu.Text;
        }

        #endregion

        #region 工具栏

        /// <summary>
        /// 工具栏
        /// </summary>
        private void ToolStripInit()
        {
            #region toolstrip init default

            _toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top,
                Size = new Size(500, 40),
                BackColor = Color.LightBlue
            };

            var toolStripButtonSave = new ToolStripButton();
            var toolStripButtonCancel = new ToolStripButton();
            // 
            // _toolStrip
            // 
            _toolStrip.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            _toolStrip.ImageScalingSize = new Size(32, 32);
            _toolStrip.Items.AddRange(new ToolStripItem[] {
            toolStripButtonSave,
            toolStripButtonCancel});
            _toolStrip.Location = new Point(0, 0);
            _toolStrip.Name = "_toolStrip";

            // 
            // toolStripButtonSave
            // 
            toolStripButtonSave.Image = Resources.Save;
            toolStripButtonSave.ImageTransparentColor = Color.Magenta;
            toolStripButtonSave.Name = "toolStripButtonAdd";
            toolStripButtonSave.Size = new Size(78, 36);
            toolStripButtonSave.Text = @"保存";
            toolStripButtonSave.Click += ToolStripButtonSave_Click;

            // 
            // toolStripButtonCancel
            // 
            toolStripButtonCancel.Image = Resources.del;
            toolStripButtonCancel.ImageTransparentColor = Color.Magenta;
            toolStripButtonCancel.Name = "toolStripButtonEdit";
            toolStripButtonCancel.Size = new Size(78, 36);
            toolStripButtonCancel.Text = @"取消";
            toolStripButtonCancel.Click += ToolStripButtonCancel_Click;

            Controls.Add(_toolStrip);

            #endregion
        }

        private void ToolStripButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolStripButtonSave_Click(object sender, EventArgs e)
        {

            foreach (var item in _devicePara.GetType().GetProperties())
            {
                var control = _lstControl.Find(t => t.Name == item.Name);
                if (control == null)
                    continue;
                item.SetValue(_devicePara, ClassComm.MyChanageType(control.Text, item.PropertyType));
            }

            if (_devicePara.GetType().ToString().Contains("DeviceConfigDeviceInfo"))
                ClassComm.DeviceConfig.DeviceInfo = (DeviceConfigDeviceInfo) _devicePara;
            else if (_devicePara.GetType().ToString().Contains("DeviceConfigFormLayout"))
                ClassComm.DeviceConfig.FormLayout = (DeviceConfigFormLayout) _devicePara;
            else if (_devicePara.GetType().ToString().Contains("DeviceConfigController"))
            {
                var lst = ClassComm.DeviceConfig.Controllers.ToList();
                switch (_operateMode)
                {
                    case "EDIT":
                        lst.Remove(lst.Find(t => t.Name == ((DeviceConfigController) _devicePara).Name));
                        lst.Add((DeviceConfigController) _devicePara);
                        break;
                    case "DEL":
                        lst.Remove(lst.Find(t => t.Name == ((DeviceConfigController) _devicePara).Name));
                        break;
                    case "ADD":
                        lst.Add((DeviceConfigController) _devicePara);
                        break;
                }
                ClassComm.DeviceConfig.Controllers = lst.ToArray();
            }
            else if (_devicePara.GetType().ToString().Contains("DeviceConfigWorkStation"))
            {
                var lst = ClassComm.DeviceConfig.WorkStations.ToList();
                if (_operateMode == "EDIT")
                {
                    lst.Remove(lst.Find(t => t.Name == ((DeviceConfigWorkStation) _devicePara).Name));
                    lst.Add((DeviceConfigWorkStation) _devicePara);
                }
                else if (_operateMode == "DEL")
                {
                    lst.Remove(lst.Find(t => t.Name == ((DeviceConfigWorkStation) _devicePara).Name));
                }
                else if (_operateMode == "ADD")
                {
                    lst.Add((DeviceConfigWorkStation) _devicePara);
                }

                ClassComm.DeviceConfig.WorkStations = lst.ToArray();
            }
            else if (_devicePara.GetType().ToString().Contains("DeviceConfigCondition"))
            {
                var lst = ClassComm.DeviceConfig.Conditions.ToList();
                if (_operateMode == "EDIT")
                {
                    lst.Remove(lst.Find(t => t.Name == ((DeviceConfigCondition) _devicePara).Name));
                    lst.Add((DeviceConfigCondition) _devicePara);
                }
                else if (_operateMode == "DEL")
                {
                    lst.Remove(lst.Find(t => t.Name == ((DeviceConfigCondition) _devicePara).Name));
                }
                else if (_operateMode == "ADD")
                {
                    lst.Add((DeviceConfigCondition) _devicePara);
                }

                ClassComm.DeviceConfig.Conditions = lst.ToArray();
            }
            else if (_devicePara.GetType().ToString().Contains("DeviceConfigControl"))
            {
                var lst = ClassComm.DeviceConfig.Controls.ToList();
                if (_operateMode == "EDIT")
                {
                    lst.Remove(lst.Find(t => t.Name == ((DeviceConfigControl) _devicePara).Name));
                    lst.Add((DeviceConfigControl) _devicePara);
                }
                else if (_operateMode == "DEL")
                {
                    lst.Remove(lst.Find(t => t.Name == ((DeviceConfigControl) _devicePara).Name));
                }
                else if (_operateMode == "ADD")
                {
                    lst.Add((DeviceConfigControl) _devicePara);
                }

                ClassComm.DeviceConfig.Controls = lst.ToArray();
            }
            else if (_devicePara.GetType().ToString().Contains("DeviceConfigPara"))
            {
                var lst = ClassComm.DeviceConfig.Paras.ToList();
                if (_operateMode == "EDIT")
                {
                    lst.Remove(lst.Find(t => t.Name == ((DeviceConfigPara) _devicePara).Name));
                    lst.Add((DeviceConfigPara) _devicePara);
                }
                else if (_operateMode == "DEL")
                    lst.Remove(lst.Find(t => t.Name == ((DeviceConfigPara) _devicePara).Name));
                else if (_operateMode == "ADD")
                    lst.Add((DeviceConfigPara) _devicePara);
                ClassComm.DeviceConfig.Paras = lst.ToArray();
            }
            else if (_devicePara.GetType().ToString().Contains("DeviceConfigPart"))
            {
                var lst = ClassComm.DeviceConfig.Parts.ToList();
                switch (_operateMode)
                {
                    case "EDIT":
                        lst.Remove(lst.Find(t => t.Name == ((DeviceConfigPart) _devicePara).Name));
                        lst.Add((DeviceConfigPart) _devicePara);
                        break;

                    case "DEL":
                        lst.Remove(lst.Find(t => t.Name == ((DeviceConfigPart) _devicePara).Name));
                        break;

                    case "ADD":
                        lst.Add((DeviceConfigPart) _devicePara);
                        break;
                }
                ClassComm.DeviceConfig.Parts = lst.ToArray();
            }
            else if (_devicePara.GetType().ToString().Contains("DeviceConfigProcess"))
            {
                var lst = ClassComm.DeviceConfig.Processes.ToList();
                switch (_operateMode)
                {
                    case "EDIT":
                        lst.Remove(lst.Find(t => t.Name == ((DeviceConfigProcess) _devicePara).Name));
                        lst.Add((DeviceConfigProcess) _devicePara);
                        break;

                    case "DEL":
                        lst.Remove(lst.Find(t => t.Name == ((DeviceConfigProcess) _devicePara).Name));
                        break;

                    case "ADD":
                        lst.Add((DeviceConfigProcess) _devicePara);
                        break;
                }
                ClassComm.DeviceConfig.Processes = lst.ToArray();
            }
            else if (_devicePara.GetType().ToString().Contains("DeviceConfigStatusUnit"))
            {
                var lst = ClassComm.DeviceConfig.StatusUnits.ToList();
                if (_operateMode == "EDIT")
                {
                    lst.Remove(lst.Find(t => t.Name == ((DeviceConfigStatusUnit) _devicePara).Name));
                    lst.Add((DeviceConfigStatusUnit) _devicePara);
                }
                else if (_operateMode == "DEL")
                    lst.Remove(lst.Find(t => t.Name == ((DeviceConfigStatusUnit) _devicePara).Name));
                else if (_operateMode == "ADD")
                    lst.Add((DeviceConfigStatusUnit) _devicePara);
                ClassComm.DeviceConfig.StatusUnits = lst.ToArray();
            }

            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

            Close();
        }
        #endregion
    }
}
