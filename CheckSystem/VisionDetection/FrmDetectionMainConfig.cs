using CheckSystem.VisionDetection.Vision;
using CommonUtility.FileOperator;
using HZH_Controls.Helpers;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ImageProcessing = CheckSystem.VisionDetection.Vision.ImageProcessing;

namespace CheckSystem.VisionDetection
{
    public sealed partial class FrmDetectionMainConfig : UIForm
    {
        public FrmDetectionMainConfig()
        {
            InitializeComponent();
            UIStyles.InitColorful(Color.FromArgb(80, 126, 164), Color.White);
            WindowState = FormWindowState.Maximized;
            Text += @" (产品名称：" + VisionCommon.VisionConfig.DeviceInfo.DeviceName + @")";

            cmbStartType.SelectedIndex = 0;
            cmbIsBang.SelectedIndex = 0;

            InitGgv();
            LoadGrv();
            InitBarcodeDgv(true);
            InitHardwarePara();
            IninCustomControllers(true);

            if (!InitTreeView())
                VisionCommon.VisionConfig.TestFlowInfo = new VisionConfigTestFlow[0];
        }

        private void InitGgv()
        {
            detectionDataGridView.Style = UIStyle.Gray;
            detectionDataGridView.ReadOnly = true;
            detectionDataGridView.RowHeadersVisible = false;
            detectionDataGridView.AllowUserToAddRows = true;
            detectionDataGridView.AllowUserToResizeRows = false;
            detectionDataGridView.AllowUserToDeleteRows = false;
            detectionDataGridView.MultiSelect = false;
            detectionDataGridView.RowHeadersVisible = true;
            detectionDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            detectionDataGridView.CellDoubleClick += detectionDataGridView_CellDoubleClick;
        }

        private void LoadGrv()
        {
            detectionDataGridView.ClearAll();

            detectionDataGridView.AddColumn("名称", "名称");
            detectionDataGridView.AddColumn("类型", "类型");
            detectionDataGridView.AddColumn("档位", "档位");
            detectionDataGridView.AddColumn("L/R", "L/R");
            detectionDataGridView.AddColumn("单位", "单位");

            detectionDataGridView.AddColumn("输入电源", "输入电源");

            detectionDataGridView.AddColumn("继电器组", "继电器组");
            detectionDataGridView.AddColumn("测试前执行函数", "测试前执行函数");
            detectionDataGridView.AddColumn("延时", "延时");
            detectionDataGridView.AddColumn("测试后执行函数", "测试后执行函数");
            detectionDataGridView.AddColumn("映射字段", "映射字段");

            foreach (var t in VisionCommon.VisionConfig.ParaInfo)
            {
                var relays = string.Empty;
                if (t.ParaReleysList != null)
                {
                    if (t.ParaReleysList.ParaReleysOnList != null)
                    {
                        relays =
                            t.ParaReleysList.ParaReleysOnList.Trim().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                .Where(rt => !string.IsNullOrEmpty(rt))
                                .Aggregate(relays, (current, rt) => current + rt.Trim() + "：ON，");
                    }

                    if (t.ParaReleysList.ParaReleysOffList != null)
                    {
                        relays =
                            t.ParaReleysList.ParaReleysOffList.Trim().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                .Where(rt => !string.IsNullOrEmpty(rt))
                                .Aggregate(relays, (current, rt) => current + rt.Trim() + "：ON，");
                        //relays =
                        //      t.ParaReleysList.ParaReleysOffList.Split(',')
                        //          .Where(s => !string.IsNullOrEmpty(s))
                        //          .Aggregate(relays, (current, s) => current + "继电器" + s + "：OFF，");
                    }
                }

                relays = relays.TrimEnd('，');

                switch (t.ParaType)
                {
                    case "信息读取（==）":
                        AddRow(t.ParaName, t.ParaType, t.ParaGroups.Length.ToString(), t.ParaLeftOrRight, t.ParaUnit,
                           relays, "双击查看详细",
                           t.ParaDelayMs, "双击查看详细",
                           t.ParaBinding);
                        break;

                    case "信息读取（like）":
                        AddRow(t.ParaName, t.ParaType, t.ParaGroups.Length.ToString(), t.ParaLeftOrRight, t.ParaUnit,
                           relays, "双击查看详细",
                           t.ParaDelayMs, "双击查看详细",
                           t.ParaBinding);
                        break;

                    case "电性能":
                        AddRow(t.ParaName, t.ParaType, t.ParaGroups.Length.ToString(), t.ParaLeftOrRight, t.ParaUnit,
                           relays, "双击查看详细",
                           t.ParaDelayMs, "双击查看详细",
                           t.ParaBinding);
                        break;

                    case "电阻":
                        AddRow(t.ParaName, t.ParaType, t.ParaGroups.Length.ToString(), t.ParaLeftOrRight, t.ParaUnit,
                           relays, "双击查看详细",
                           t.ParaDelayMs, "双击查看详细",
                           t.ParaBinding);
                        break;

                    case "电性能，静态图像":
                        AddRow(t.ParaName, t.ParaType, t.ParaGroups.Length.ToString(), t.ParaLeftOrRight, t.ParaUnit,
                            relays, "双击查看详细",
                            t.ParaDelayMs, "双击查看详细",
                            t.ParaBinding);
                        break;

                    case "动态图像":
                        t.ParaBinding = "/";

                        t.ParaGroups = new VisionConfigParaParaGroup[1];
                        t.ParaGroups[0] = new VisionConfigParaParaGroup
                        {
                            ParaGroupName = "/",
                            ParaGroupMin = "/",
                            ParaGroupMax = "/",
                            ParaGroupValue = "/",
                            ParaGroupK = "/",
                            ParaGroupB = "/"
                        };

                        t.ParaUnit = "/";

                        AddRow(t.ParaName, t.ParaType, t.ParaGroups.Length.ToString(), t.ParaLeftOrRight, t.ParaUnit,
                            relays, "双击查看详细",
                            t.ParaDelayMs, "双击查看详细",
                            t.ParaBinding);
                        break;

                    case "静态图像":
                        t.ParaBinding = "/";

                        t.ParaGroups = new VisionConfigParaParaGroup[1];
                        t.ParaGroups[0] = new VisionConfigParaParaGroup
                        {
                            ParaGroupName = "/",
                            ParaGroupMin = "/",
                            ParaGroupMax = "/",
                            ParaGroupValue = "/",
                            ParaGroupK = "/",
                            ParaGroupB = "/"
                        };

                        t.ParaUnit = "/";

                        AddRow(t.ParaName, t.ParaType, t.ParaGroups.Length.ToString(), t.ParaLeftOrRight, t.ParaUnit,
                            relays, "双击查看详细",
                            t.ParaDelayMs, "双击查看详细",
                            t.ParaBinding);
                        break;
                }
            }

            detectionDataGridView.AutoResizeRows();
        }

        private void InitHardwarePara()
        {
            if (string.IsNullOrEmpty(VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight))
                VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight = "不区分";

            if (VisionCommon.VisionConfig.DeviceInfo.Actions != null)
            {
                if (VisionCommon.VisionConfig.DeviceInfo.Actions.Start == null)
                {
                    VisionCommon.VisionConfig.DeviceInfo.Actions.Start = new VisionConfigDeviceInfoActionsStart
                    {
                        Type = "按键启动"
                    };
                }

                if (VisionCommon.VisionConfig.DeviceInfo.Actions.Bang == null)
                {
                    VisionCommon.VisionConfig.DeviceInfo.Actions.Bang = new VisionConfigDeviceInfoActionsBang
                    {
                        Delay = "800",
                        IsBang = "True"
                    };
                }

                if (VisionCommon.VisionConfig.DeviceInfo.Actions.PropulsionCylinder == null)
                {
                    VisionCommon.VisionConfig.DeviceInfo.Actions.PropulsionCylinder =
                        new VisionConfigDeviceInfoActionsPropulsionCylinder
                        {
                            Delay = "1000",
                            Binding = "继电器12;",
                            IsPropulsionCylinder = "False"
                        };
                }
            }
            else
            {
                VisionCommon.VisionConfig.DeviceInfo.Actions = new VisionConfigDeviceInfoActions
                {
                    Start = new VisionConfigDeviceInfoActionsStart
                    {
                        Type = "按键启动"
                    },
                    Bang = new VisionConfigDeviceInfoActionsBang
                    {
                        Delay = "800",
                        IsBang = "True",
                    },
                    PropulsionCylinder = new VisionConfigDeviceInfoActionsPropulsionCylinder
                    {
                        Delay = "1000",
                        Binding = "继电器12;",
                        IsPropulsionCylinder = "False"
                    }
                };
            }

            for (var i = 0; i < cmbStartType.Items.Count; i++)
            {
                if (cmbStartType.Items[i].ToString() !=
                    VisionCommon.VisionConfig.DeviceInfo.Actions.Start.Type)
                    continue;
                cmbStartType.SelectedIndex = i;
                break;
            }

            cmbLeftOrRight.SelectedIndex = string.Equals(VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight, "区分",
                StringComparison.CurrentCultureIgnoreCase) ? 0 : 1;

            cmbIsBang.SelectedIndex = string.Equals(VisionCommon.VisionConfig.DeviceInfo.Actions.Bang.IsBang, "True",
                StringComparison.CurrentCultureIgnoreCase) ? 0 : 1;

            txtBangDelay.Value = int.Parse(VisionCommon.VisionConfig.DeviceInfo.Actions.Bang.Delay);

            _txtPr1.Text = VisionCommon.VisionConfig.DeviceInfo.DeviceNo;
            _txtPr2.Text = VisionCommon.VisionConfig.DeviceInfo.DeviceNo2;
        }

        // private void AddRow(string name, string type, string group, string leftOrRight, string unit, string delay, string relays, string methodBefore, string methodAfter, string binding, string offsetK, string offsetB)
        private void AddRow(
            string name, string type, string group, string leftOrRight, string unit, string relays, string methodBefore, string delay, string methodAfter, string binding)
        {
            //detectionDataGridView.AddRow(name, type, @group, leftOrRight, unit, delay, relays, methodBefore, methodAfter, binding, offsetK, offsetB);
            detectionDataGridView.AddRow(name, type, @group, leftOrRight, unit, "双击查看详细", relays, methodBefore, delay, methodAfter, binding);
            detectionDataGridView.AutoResizeColumns();
        }

        private void detectionDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 && e.RowIndex == detectionDataGridView.RowCount - 1)
            {
                // add
                var frm = new FrmDetectionParaConfig(null);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var temp = VisionCommon.VisionConfig.ParaInfo.ToList();
                    temp.Add(frm.Para);
                    VisionCommon.VisionConfig.ParaInfo = temp.ToArray();

                    var relays = string.Empty;
                    if (frm.Para.ParaReleysList != null)
                    {
                        if (frm.Para.ParaReleysList.ParaReleysOnList != null)
                            relays =
                                frm.Para.ParaReleysList.ParaReleysOnList.Trim()
                                    .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                    .Where(rt => !string.IsNullOrEmpty(rt))
                                    .Aggregate(relays, (current, rt) => current + rt.Trim() + "：ON，");
                        //relays =
                        //    frm.Para.ParaReleysList.ParaReleysOnList.Trim().Split(',')
                        //        .Where(s => !string.IsNullOrEmpty(s))
                        //        .Aggregate(relays, (current, s) => current + "继电器" + s + "：ON，");

                        if (frm.Para.ParaReleysList.ParaReleysOffList != null)
                            relays =
                                frm.Para.ParaReleysList.ParaReleysOffList.Trim()
                                    .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                    .Where(rt => !string.IsNullOrEmpty(rt))
                                    .Aggregate(relays, (current, rt) => current + rt.Trim() + "：ON，");
                        //relays =
                        //       frm.Para.ParaReleysList.ParaReleysOffList.Split(',')
                        //           .Where(s => !string.IsNullOrEmpty(s))
                        //           .Aggregate(relays, (current, s) => current + "继电器" + s + "：OFF，");
                    }
                    relays = relays.TrimEnd('，');

                    AddRow(frm.Para.ParaName, frm.Para.ParaType, frm.Para.ParaGroups.Length.ToString(), frm.Para.ParaLeftOrRight, frm.Para.ParaUnit,
                           relays, "双击查看详细", frm.Para.ParaDelayMs, "双击查看详细", frm.Para.ParaBinding);

                    var tempVision = VisionCommon.VisionConfig.VisionInfo.ToList();
                    var findVision = tempVision.Find(f => f.VisionFuncName == frm.Para.ParaName);
                    if (findVision == null)
                    {
                        findVision = new VisionConfigVisionFunc
                        {
                            VisionFuncName = frm.Para.ParaName,
                            VisionFuncDetailL = new VisionConfigVisionFuncCamera[1]
                        };
                        findVision.VisionFuncDetailL[0] = new VisionConfigVisionFuncCamera
                        {
                            Analysis = new VisionConfigVisionFuncCameraAnalysis
                            {
                                CaliRegion = new string[0],
                                LookupTable = ImageProcessing.LookupTableType.ImageSource.ToString(),
                                ShapesGroups = new VisionConfigVisionFuncCameraAnalysisShapesShape[1][]
                            }
                        };
                        findVision.VisionFuncDetailL[0].Analysis.ShapesGroups[0] = new VisionConfigVisionFuncCameraAnalysisShapesShape[0];
                        tempVision.Add(findVision);
                    }
                    else
                    {
                        findVision = new VisionConfigVisionFunc
                        {
                            VisionFuncName = frm.Para.ParaName,
                            VisionFuncDetailL = new VisionConfigVisionFuncCamera[1]
                        };
                        findVision.VisionFuncDetailL[0] = new VisionConfigVisionFuncCamera
                        {
                            Analysis = new VisionConfigVisionFuncCameraAnalysis
                            {
                                CaliRegion = new string[0],
                                LookupTable = ImageProcessing.LookupTableType.ImageSource.ToString(),
                                ShapesGroups = new VisionConfigVisionFuncCameraAnalysisShapesShape[1][]
                            }
                        };
                        findVision.VisionFuncDetailL[0].Analysis.ShapesGroups[0] = new VisionConfigVisionFuncCameraAnalysisShapesShape[0];
                    }

                    VisionCommon.VisionConfig.VisionInfo = tempVision.ToArray();
                }
            }
            else
            {
                if (detectionDataGridView.RowCount > 1 && e.ColumnIndex == -1)
                {
                    // edit
                    var name = detectionDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();

                    var para = VisionCommon.VisionConfig.ParaInfo.ToList().Find(f => f.ParaName == name);
                    if (para != null)
                    {
                        var frm = new FrmDetectionParaConfig(para);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            VisionCommon.VisionConfig.ParaInfo[e.RowIndex] = frm.Para;

                            var relays = string.Empty;
                            if (frm.Para.ParaReleysList != null)
                            {
                                if (frm.Para.ParaReleysList.ParaReleysOnList != null)
                                    relays =
                                        frm.Para.ParaReleysList.ParaReleysOnList.Trim()
                                            .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                            .Where(rt => !string.IsNullOrEmpty(rt))
                                            .Aggregate(relays, (current, rt) => current + rt.Trim() + "：ON，");
                                //relays =
                                //    frm.Para.ParaReleysList.ParaReleysOnList.Split(',')
                                //        .Where(s => !string.IsNullOrEmpty(s))
                                //        .Aggregate(relays, (current, s) => current + "继电器" + s + "：ON，");

                                if (frm.Para.ParaReleysList.ParaReleysOffList != null)
                                    relays =
                                        frm.Para.ParaReleysList.ParaReleysOffList.Trim()
                                            .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                            .Where(rt => !string.IsNullOrEmpty(rt))
                                            .Aggregate(relays, (current, rt) => current + rt.Trim() + "：ON，");
                                //relays =
                                //       frm.Para.ParaReleysList.ParaReleysOffList.Split(',')
                                //           .Where(s => !string.IsNullOrEmpty(s))
                                //           .Aggregate(relays, (current, s) => current + "继电器" + s + "：OFF，");
                            }
                            relays = relays.TrimEnd('，');

                            detectionDataGridView.Rows[e.RowIndex].Cells[0].Value = frm.Para.ParaName;
                            detectionDataGridView.Rows[e.RowIndex].Cells[1].Value = frm.Para.ParaType;
                            detectionDataGridView.Rows[e.RowIndex].Cells[2].Value = frm.Para.ParaGroups.Length;
                            detectionDataGridView.Rows[e.RowIndex].Cells[3].Value = frm.Para.ParaLeftOrRight;
                            detectionDataGridView.Rows[e.RowIndex].Cells[4].Value = frm.Para.ParaUnit;
                            detectionDataGridView.Rows[e.RowIndex].Cells[5].Value = "双击查看详细";
                            detectionDataGridView.Rows[e.RowIndex].Cells[6].Value = relays;
                            detectionDataGridView.Rows[e.RowIndex].Cells[7].Value = "双击查看详细";
                            detectionDataGridView.Rows[e.RowIndex].Cells[8].Value = frm.Para.ParaDelayMs;
                            detectionDataGridView.Rows[e.RowIndex].Cells[9].Value = "双击查看详细";
                            detectionDataGridView.Rows[e.RowIndex].Cells[10].Value = frm.Para.ParaBinding;

                            var vision =
                                VisionCommon.VisionConfig.VisionInfo.ToList().Find(f => f.VisionFuncName == name);
                            if (vision != null)
                            {
                                vision.VisionFuncName = frm.Para.ParaName;
                            }
                        }
                    }
                }
                else if (detectionDataGridView.RowCount > 1 && e.RowIndex != detectionDataGridView.RowCount - 1 && (e.ColumnIndex == 7 || e.ColumnIndex == 9))
                {
                    var name = detectionDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                    var para = VisionCommon.VisionConfig.ParaInfo.ToList().Find(f => f.ParaName == name);
                    if (para != null)
                    {
                        //var option = new UIEditOption { AutoLabelWidth = true, Text = @"函数配置" };

                        var methodStr = string.Empty;

                        if (para.ParaMethods == null)
                        {
                            para.ParaMethods = new VisionConfigParaParaMethods
                            {
                                ParaMethodsBefore = string.Empty,
                                ParaMethodsAfter = string.Empty
                            };
                        }
                        if (string.IsNullOrEmpty(para.ParaMethods.ParaMethodsBefore))
                            para.ParaMethods.ParaMethodsBefore = string.Empty;
                        if (string.IsNullOrEmpty(para.ParaMethods.ParaMethodsAfter))
                            para.ParaMethods.ParaMethodsAfter = string.Empty;

                        if (e.ColumnIndex == 7)
                        {
                            methodStr = para.ParaMethods.ParaMethodsBefore.Trim();
                            //option.AddText("Method", "函数内容：", para.ParaMethods.ParaMethodsBefore.Trim(), false);
                        }
                        else if (e.ColumnIndex == 9)
                        {
                            methodStr = para.ParaMethods.ParaMethodsAfter.Trim();
                            //option.AddText("Method", "函数内容：", para.ParaMethods.ParaMethodsAfter.Trim(), false);
                        }

                        var frm = new FrmMethodConfig(methodStr);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            var method = frm.MethodStr.Trim();
                            if (e.ColumnIndex == 7)
                                para.ParaMethods.ParaMethodsBefore = method;
                            else if (e.ColumnIndex == 9)
                                para.ParaMethods.ParaMethodsAfter = method;

                            detectionDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "双击查看详细";
                        }

                        //var frm = new UIEditForm(option);
                        //frm.Render();
                        //frm.ShowDialog();

                        //if (!frm.IsOK)
                        //    return;

                        //var method = frm["Method"];
                        //if (method == null)
                        //    return;


                    }
                }
                else if (detectionDataGridView.RowCount > 1 && e.RowIndex != detectionDataGridView.RowCount - 1 && (e.ColumnIndex == 5))
                {
                    var pName = detectionDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                    var para = VisionCommon.VisionConfig.ParaInfo.ToList().Find(f => f.ParaName == pName);
                    if (para != null)
                    {
                        if (string.IsNullOrEmpty(para.PowerPara))
                            para.PowerPara = "电压1=13.5V，电流1=10A，电压2=0V，电流2=0A，电压3=5V，电流3=1A，串并联模式=无";

                        var option = new UIEditOption { AutoLabelWidth = true, Text = @"电源配置" };
                        var sp = para.PowerPara.Split("，");

                        var powerMode = new[] { "无", "并联", "串联" };
                        foreach (var t in sp)
                        {
                            var name = t.Split('=')[0];
                            var value = t.Split('=')[1];

                            if (name.StartsWith("电压"))
                                option.AddDouble("SetV" + name.Replace("电压", ""), name + "/V：",
                                    double.Parse(value.TrimEnd('V')));
                            else if (name.StartsWith("电流"))
                                option.AddDouble("SetC" + name.Replace("电流", ""), name + "/A：",
                                    double.Parse(value.TrimEnd('A')));
                            else if (name.StartsWith("串并联模式"))
                                option.AddCombobox("PowerMode", "串并联模式:", powerMode,
                                    powerMode.ToList().FindIndex(f => f == value), true, true);
                        }

                        var frmPowerConfig = new UIEditForm(option);
                        frmPowerConfig.Render();
                        frmPowerConfig.ShowDialog();

                        if (!frmPowerConfig.IsOK)
                        {
                            this.ShowInfoTip("用户取消了操作");
                            return;
                        }
                        var v1 = frmPowerConfig["SetV1"].ToString();
                        var v2 = frmPowerConfig["SetV2"].ToString();
                        var v3 = frmPowerConfig["SetV3"].ToString();

                        var c1 = frmPowerConfig["SetC1"].ToString();
                        var c2 = frmPowerConfig["SetC2"].ToString();
                        var c3 = frmPowerConfig["SetC3"].ToString();

                        var powerConfig = powerMode[(int)frmPowerConfig["PowerMode"]];

                        para.PowerPara = string.Format("电压1={0}V，电流1={1}A，电压2={2}V，电流2={3}A，电压3={4}V，电流3={5}A，串并联模式={6}", v1,
                            c1, v2, c2, v3, c3, powerConfig);
                        this.ShowSuccessTip("数据已更新，请保存");
                    }
                }
            }
        }

        private bool InitTreeView()
        {
            TestFlowTreeView.Nodes.Clear();
            TestFlowTreeView.ShowLines = true;

            if (VisionCommon.VisionConfig != null && VisionCommon.VisionConfig.TestFlowInfo != null)
            {
                var listLvl1TreeNodes = new List<TreeNode>();

                for (var i = 0; i < VisionCommon.VisionConfig.TestFlowInfo.Length; i++)
                {
                    var level1 = VisionCommon.VisionConfig.TestFlowInfo[i];

                    if (string.IsNullOrEmpty(level1.TestFlowValue))
                        return false;
                    var lvl1TreeNode = new TreeNode(level1.TestFlowValue);

                    if (level1.TestFlow != null) // level 2
                    {
                        foreach (var level2 in level1.TestFlow)
                        {
                            var lvl2TreeNode = new TreeNode();

                            if (string.IsNullOrEmpty(level2.TestFlowValue))
                                return false;

                            if (level2.TestFlowValue.StartsWith("检测："))
                            {
                                lvl2TreeNode.Text = @"检测：";
                                var checkList = level2.TestFlowValue.Split("：")[1].Split("，");

                                if (!checkList.Any())
                                    return false;

                                foreach (var c in checkList.Where(c => !string.IsNullOrEmpty(c)))
                                {
                                    lvl2TreeNode.Nodes.Add(new TreeNode(c));
                                }
                            }
                            else
                            {
                                lvl2TreeNode.Text = level2.TestFlowValue;
                            }

                            lvl1TreeNode.Nodes.Add(lvl2TreeNode);
                        }
                    }

                    listLvl1TreeNodes.Add(lvl1TreeNode);
                }

                foreach (var t in listLvl1TreeNodes)
                    TestFlowTreeView.Nodes.Add(t);
                TestFlowTreeView.ExpandAll();
            }
            else
            {
                return false;
            }

            return true;
        }

        private void btnAddPowerNode_Click(object sender, EventArgs e)
        {
            var count = TestFlowTreeView.Nodes.Count;

            var option = new UIEditOption { AutoLabelWidth = true, Text = @"电源配置" };
            for (var i = 1; i < 4; i++)
            {
                option.AddDouble("SetV" + i, "电压" + i + "/V：", 0);
                option.AddDouble("SetC" + i, "电流" + i + "/A：", 0);
            }

            var powerMode = new[] { "无", "并联", "串联" };
            option.AddCombobox("PowerMode", "串并联模式:", powerMode, 0, true, true);

            var frmPowerConfig = new UIEditForm(option);
            frmPowerConfig.Render();
            frmPowerConfig.ShowDialog();

            if (!frmPowerConfig.IsOK)
            {
                this.ShowInfoTip("用户取消了操作");
                return;
            }
            var v1 = frmPowerConfig["SetV1"].ToString();
            var v2 = frmPowerConfig["SetV2"].ToString();
            var v3 = frmPowerConfig["SetV3"].ToString();

            var c1 = frmPowerConfig["SetC1"].ToString();
            var c2 = frmPowerConfig["SetC2"].ToString();
            var c3 = frmPowerConfig["SetC3"].ToString();

            var powerConfig = powerMode[(int)frmPowerConfig["PowerMode"]];

            var str = string.Format("电源模式{0}：电压1={1}V，电流1={2}A，电压2={3}V，电流2={4}A，电压3={5}V，电流3={6}A，串并联模式={7}", count + 1,
                v1, c1, v2, c2, v3, c3, powerConfig);
            var node = new TreeNode(str);
            TestFlowTreeView.Nodes.Add(node);
            TestFlowTreeView.ExpandAll();
            TestFlowTreeView.SelectedNode = node;
            this.ShowSuccessTip("数据已更新，请保存");
        }

        private void btnUpdateSelectPowerNode_Click(object sender, EventArgs e)
        {
            if (TestFlowTreeView.Nodes.Count <= 0 || TestFlowTreeView.SelectedNode == null)
            {
                this.ShowErrorTip("请先选中一个电源模式节点");
                return;
            }

            if (!TestFlowTreeView.SelectedNode.Text.StartsWith("电源模式"))
            {
                this.ShowErrorTip("请先选中一个电源模式节点");
                return;
            }

            var sp = TestFlowTreeView.SelectedNode.Text.Split('：')[1].Split('，');

            var option = new UIEditOption { AutoLabelWidth = true, Text = @"电源配置" };

            var powerMode = new[] { "无", "并联", "串联" };
            foreach (var t in sp)
            {
                var name = t.Split('=')[0];
                var value = t.Split('=')[1];

                if (name.StartsWith("电压"))
                    option.AddDouble("SetV" + name.Replace("电压", ""), name + "/V：",
                        double.Parse(value.TrimEnd('V')));
                else if (name.StartsWith("电流"))
                    option.AddDouble("SetC" + name.Replace("电流", ""), name + "/A：",
                        double.Parse(value.TrimEnd('A')));
                else if (name.StartsWith("串并联模式"))
                    option.AddCombobox("PowerMode", "串并联模式:", powerMode,
                        powerMode.ToList().FindIndex(f => f == value), true, true);
            }

            var frmPowerConfig = new UIEditForm(option);
            frmPowerConfig.Render();
            frmPowerConfig.ShowDialog();

            if (!frmPowerConfig.IsOK)
            {
                this.ShowInfoTip("用户取消了操作");
                return;
            }
            var v1 = frmPowerConfig["SetV1"].ToString();
            var v2 = frmPowerConfig["SetV2"].ToString();
            var v3 = frmPowerConfig["SetV3"].ToString();

            var c1 = frmPowerConfig["SetC1"].ToString();
            var c2 = frmPowerConfig["SetC2"].ToString();
            var c3 = frmPowerConfig["SetC3"].ToString();

            var powerConfig = powerMode[(int)frmPowerConfig["PowerMode"]];

            var str = string.Format("{0}：电压1={1}V，电流1={2}A，电压2={3}V，电流2={4}A，电压3={5}V，电流3={6}A，串并联模式={7}",
                TestFlowTreeView.SelectedNode.Text.Split('：')[0], v1, c1, v2, c2, v3, c3, powerConfig);
            TestFlowTreeView.SelectedNode.Text = str;
            this.ShowSuccessTip("数据已更新，请保存");
        }

        private void btnDetectionNodeConfig_Click(object sender, EventArgs e)
        {
            if (detectionDataGridView.RowCount <= 1)
            {
                this.ShowErrorTip("请先配置检测项");
                return;
            }

            if (TestFlowTreeView.Nodes.Count <= 0 || TestFlowTreeView.SelectedNode == null)
            {
                this.ShowErrorTip("请先选中一个节点");
                return;
            }

            if (TestFlowTreeView.SelectedNode.Text.StartsWith("检测：") &&
                (TestFlowTreeView.SelectedNode.Level == 1 || TestFlowTreeView.SelectedNode.Level == 2))
            {
                UpdateCheck(TestFlowTreeView.SelectedNode);
            }
            else if (TestFlowTreeView.SelectedNode.Text.StartsWith("电源模式") &&
                     TestFlowTreeView.SelectedNode.Level == 0)
            {
                if (TestFlowTreeView.SelectedNode.Nodes.Count == 0)
                {
                    AddCheck(TestFlowTreeView.SelectedNode);
                }
                else
                {
                    //var isHaveCheck = false;
                    for (var i = 0; i < TestFlowTreeView.SelectedNode.Nodes.Count; i++)
                    {
                        //if (TestFlowTreeView.SelectedNode.Nodes[i].Nodes.Count > 0 )
                        //{
                        //    this.ShowErrorDialog("请先选中单步");
                        //    return;
                        //}

                        if (TestFlowTreeView.SelectedNode.Nodes[i].Nodes.Count > 0 ||
                            TestFlowTreeView.SelectedNode.Nodes[i].Text.StartsWith("继电器") ||
                            TestFlowTreeView.SelectedNode.Nodes[i].Text.StartsWith("延时：") ||
                            TestFlowTreeView.SelectedNode.Nodes[i].Text.StartsWith("函数："))
                        {
                            this.ShowErrorTip("请先选中包含检测项目的单步动作 ");
                            return;
                        }

                        if (TestFlowTreeView.SelectedNode.Nodes[i].Text.StartsWith("检测"))
                        {
                            UpdateCheck(TestFlowTreeView.SelectedNode);
                            return;
                        }
                    }

                    AddCheck(TestFlowTreeView.SelectedNode);
                }
            }
        }

        private void AddCheck(TreeNode node)
        {
            //value = string.Empty;
            var option = new UIEditOption { AutoLabelWidth = true, Text = @"检测配置" };

            var items = new ComboCheckedListBoxItem[detectionDataGridView.RowCount - 1];
            for (var i = 0; i < items.Length; i++)
                items[i] = new ComboCheckedListBoxItem(detectionDataGridView.Rows[i].Cells[0].Value.ToString(),
                    false);

            option.AddComboCheckedListBox("Detection", "检测项：", items, "");

            var frmDetection = new UIEditForm(option);
            frmDetection.Render();
            frmDetection.ShowDialog();

            if (frmDetection.IsOK)
            {
                var outCheckedItems = (ComboCheckedListBoxItem[])frmDetection["Detection"];

                var addCount = 0;

                var checkNode = new TreeNode("检测：");

                foreach (var item in outCheckedItems)
                {
                    if (item.Checked)
                    {
                        addCount++;
                        var str = item.Text;
                        checkNode.Nodes.Add(new TreeNode(str));
                    }
                }

                if (addCount > 0)
                {
                    node.Nodes.Add(checkNode);
                    TestFlowTreeView.ExpandAll();
                    TestFlowTreeView.SelectedNode = node;
                    this.ShowSuccessTip("数据已更新，请保存");
                }
                else
                {
                    this.ShowErrorTip("取消了所有检测项，请确认");
                }
            }
            else
            {
                this.ShowInfoTip("用户取消了操作");
            }
        }

        private void UpdateCheck(TreeNode parentNode)
        {
            var checkList = new List<string>();
            var showStr = string.Empty;
            var parent = parentNode;//TestFlowTreeView.SelectedNode.Parent;
            for (var i = 0; i < parent.Nodes.Count; i++)
            {
                checkList.Add(parent.Nodes[i].Text);
                showStr += checkList[i] + "；";
            }
            showStr = showStr.TrimEnd('；');

            var option = new UIEditOption { AutoLabelWidth = true, Text = @"检测配置" };

            var items = new ComboCheckedListBoxItem[detectionDataGridView.RowCount - 1];
            for (var i = 0; i < items.Length; i++)
            {
                var checkName = detectionDataGridView.Rows[i].Cells[0].Value.ToString();
                items[i] = new ComboCheckedListBoxItem(checkName, checkList.Contains(checkName));
            }

            option.AddComboCheckedListBox("Detection", "检测项：", items, showStr);

            var frmDetection = new UIEditForm(option);
            frmDetection.Render();
            frmDetection.ShowDialog();

            if (frmDetection.IsOK)
            {
                var outCheckedItems = (ComboCheckedListBoxItem[])frmDetection["Detection"];

                parent.Nodes.Clear();
                var addCount = 0;
                foreach (var item in outCheckedItems)
                {
                    if (item.Checked)
                    {
                        addCount++;
                        var str = item.Text;
                        parent.Nodes.Add(new TreeNode(str));
                    }
                }

                if (addCount > 0)
                {
                    this.ShowSuccessTip("数据已更新，请保存");
                }
                else
                {
                    this.ShowErrorTip("取消了所有检测项，请确认");
                }
            }
            else
            {
                this.ShowInfoTip("用户取消了操作");
            }
        }

        private void btnAddActionNode_Click(object sender, EventArgs e)
        {
            if (TestFlowTreeView.Nodes.Count <= 0 || TestFlowTreeView.SelectedNode == null)
            {
                this.ShowErrorTip("请先选中一个节点");
                return;
            }

            if (TestFlowTreeView.SelectedNode.Text.StartsWith("电源模式") && TestFlowTreeView.SelectedNode.Level == 0)
            {
                if (TestFlowTreeView.SelectedNode.Nodes.Count == 0)
                {
                    var frm = new FrmActionNodeConfig(detectionDataGridView);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        for (var i = 0; i < frm.uiDataGridView1.RowCount; i++)
                        {
                            // cell 2
                            if (frm.uiDataGridView1.Rows[i].Cells[2].Value.ToString().StartsWith("检测："))
                            {
                                var newNode = new TreeNode("检测：");
                                var checkValue = frm.uiDataGridView1.Rows[i].Cells[2].Value;
                                foreach (var t in checkValue.ToString().Split("：")[1].Split("，"))
                                {
                                    if (!string.IsNullOrEmpty(t))
                                    {
                                        newNode.Nodes.Add(t);
                                    }
                                }
                                TestFlowTreeView.SelectedNode.Nodes.Add(newNode);
                            }
                            else
                            {
                                var newNode = new TreeNode(frm.uiDataGridView1.Rows[i].Cells[2].Value.ToString());
                                TestFlowTreeView.SelectedNode.Nodes.Add(newNode);
                            }
                        }

                        TestFlowTreeView.ExpandAll();
                        this.ShowSuccessTip("数据已更新，请保存");
                    }
                    else
                    {
                        this.ShowInfoTip("用户取消了操作");
                    }
                }
                else
                {
                    var frm = new FrmActionNodeConfig(detectionDataGridView, TestFlowTreeView.SelectedNode);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        TestFlowTreeView.SelectedNode.Nodes.Clear();

                        for (var i = 0; i < frm.uiDataGridView1.RowCount; i++)
                        {
                            // cell 2
                            if (frm.uiDataGridView1.Rows[i].Cells[2].Value.ToString().StartsWith("检测："))
                            {
                                var newNode = new TreeNode("检测：");
                                var checkValue = frm.uiDataGridView1.Rows[i].Cells[2].Value;
                                foreach (var t in checkValue.ToString().Split("：")[1].Split("，"))
                                {
                                    if (!string.IsNullOrEmpty(t))
                                    {
                                        newNode.Nodes.Add(t);
                                    }
                                }
                                TestFlowTreeView.SelectedNode.Nodes.Add(newNode);
                            }
                            else
                            {
                                var newNode = new TreeNode(frm.uiDataGridView1.Rows[i].Cells[2].Value.ToString());
                                TestFlowTreeView.SelectedNode.Nodes.Add(newNode);
                            }
                        }

                        TestFlowTreeView.ExpandAll();
                        this.ShowSuccessTip("数据已更新，请保存");
                    }
                    else
                    {
                        this.ShowInfoTip("用户取消了操作");
                    }
                }
            }
        }

        private void btnDeleteSelectNode_Click(object sender, EventArgs e)
        {
            if (TestFlowTreeView.Nodes.Count <= 0 || TestFlowTreeView.SelectedNode == null)
            {
                this.ShowErrorTip("请选择一个节点");
                return;
            }
            if (!this.ShowAskDialog("是否删除?"))
            {
                this.ShowInfoTip("用户取消了操作");
                return;
            }

            if (TestFlowTreeView.SelectedNode.Parent != null && TestFlowTreeView.SelectedNode.Parent.Text.StartsWith("检测：") && TestFlowTreeView.SelectedNode.Parent.Nodes.Count == 1)
            {
                TestFlowTreeView.Nodes.Remove(TestFlowTreeView.SelectedNode.Parent);
            }
            else
            {
                TestFlowTreeView.Nodes.Remove(TestFlowTreeView.SelectedNode);
            }

            for (var i = 0; i < TestFlowTreeView.Nodes.Count; i++)
            {
                var str = TestFlowTreeView.Nodes[i].Text.Split('：')[0];
                TestFlowTreeView.Nodes[i].Text = TestFlowTreeView.Nodes[i].Text.Replace(str, "电源模式" + (i + 1));
            }

            this.ShowSuccessTip("节点已删除，请保存");
        }

        private void btnSaveParas_Click(object sender, EventArgs e)
        {
            if (this.ShowAskDialog("确定保存？"))
            {
                if (TestFlowTreeView.Nodes.Count > 0)
                {
                    // lvl1

                    VisionCommon.VisionConfig.TestFlowInfo = new VisionConfigTestFlow[TestFlowTreeView.Nodes.Count];

                    for (var i = 0; i < TestFlowTreeView.Nodes.Count; i++)
                    {
                        var lvl1 = TestFlowTreeView.Nodes[i];
                        VisionCommon.VisionConfig.TestFlowInfo[i] = new VisionConfigTestFlow();
                        VisionCommon.VisionConfig.TestFlowInfo[i].TestFlowValue = lvl1.Text;

                        VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow =
                            new VisionConfigTestFlowTestFlow[lvl1.Nodes.Count];

                        for (var j = 0; j < lvl1.Nodes.Count; j++)
                        {
                            VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow[j] = new VisionConfigTestFlowTestFlow();
                            //VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow[j].TestFlowValue

                            var lvl2 = lvl1.Nodes[j];

                            if (lvl2.Text.StartsWith("检测："))
                            {
                                var str = "检测：";
                                for (var k = 0; k < lvl2.Nodes.Count; k++)
                                    str += lvl2.Nodes[k].Text + "，";

                                str = str.TrimEnd('，');
                                VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow[j].TestFlowValue = str;
                            }
                            else
                            {
                                VisionCommon.VisionConfig.TestFlowInfo[i].TestFlow[j].TestFlowValue = lvl2.Text;
                            }
                        }
                    }

                }

                VisionCommon.VisionConfig.DeviceInfo.DeviceGuid = Guid.NewGuid().ToString();
                VisionCommon.VisionConfig.DeviceInfo.DeviceUpdateTime =
                        DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                XmlHelper.SerializeToFile(VisionCommon.VisionConfig, VisionCommon.FilePath, Encoding.UTF8);

                this.ShowSuccessDialog("保存成功！");
            }
        }

        private void btnDeleteParas_Click(object sender, EventArgs e)
        {
            if (detectionDataGridView.SelectedRows.Count == 1 && detectionDataGridView.SelectedRows[0].Index != detectionDataGridView.RowCount - 1)
            {
                var index = detectionDataGridView.SelectedRows[0].Index;
                var paraName = detectionDataGridView.SelectedRows[0].Cells[0].Value.ToString();
                if (this.ShowAskDialog(string.Format("确定删除：{0}？", paraName)))
                {
                    var tempParas = VisionCommon.VisionConfig.ParaInfo.ToList();
                    tempParas.RemoveAt(index);
                    VisionCommon.VisionConfig.ParaInfo = tempParas.ToArray();

                    var tempVisions = VisionCommon.VisionConfig.VisionInfo.ToList();
                    var toDeleteVisionIndex = tempVisions.FindIndex(f => f.VisionFuncName == paraName);
                    if (toDeleteVisionIndex != -1)
                        tempVisions.RemoveAt(toDeleteVisionIndex);
                    VisionCommon.VisionConfig.VisionInfo = tempVisions.ToArray();

                    VisionCommon.VisionConfig.DeviceInfo.DeviceGuid = Guid.NewGuid().ToString();
                    VisionCommon.VisionConfig.DeviceInfo.DeviceUpdateTime =
                            DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    XmlHelper.SerializeToFile(VisionCommon.VisionConfig, VisionCommon.FilePath, Encoding.UTF8);

                    LoadGrv();
                    this.ShowSuccessDialog("删除成功！");
                }
            }
        }

        private void btnSaveHardwareConfig_Click(object sender, EventArgs e)
        {
            if (this.ShowAskDialog("确定保存？"))
            {
                VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight = cmbLeftOrRight.Text;

                VisionCommon.VisionConfig.DeviceInfo.Actions = new VisionConfigDeviceInfoActions
                {
                    Start = new VisionConfigDeviceInfoActionsStart
                    {
                        Type = cmbStartType.Text,
                    },
                    Bang = new VisionConfigDeviceInfoActionsBang
                    {
                        Delay = txtBangDelay.Value.ToString(),
                        IsBang = cmbIsBang.SelectedIndex == 0 ? "True" : "False",
                    }
                };
                VisionCommon.VisionConfig.DeviceInfo.DeviceGuid = Guid.NewGuid().ToString();
                VisionCommon.VisionConfig.DeviceInfo.DeviceUpdateTime =
                       DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                XmlHelper.SerializeToFile(VisionCommon.VisionConfig, VisionCommon.FilePath, Encoding.UTF8);

                this.ShowSuccessDialog("保存成功！");
            }
        }

        #region 二维码相关

        private void InitBarcodeDgv(bool isFirst)
        {
            dgvBarcodeGroups.ClearAll();

            dgvBarcodeGroups.Style = UIStyle.Gray;
            dgvBarcodeGroups.ReadOnly = false;
            dgvBarcodeGroups.RowHeadersVisible = false;
            dgvBarcodeGroups.AllowUserToAddRows = true;
            dgvBarcodeGroups.AllowUserToResizeRows = false;
            dgvBarcodeGroups.AllowUserToResizeColumns = true;
            dgvBarcodeGroups.MultiSelect = true;
            dgvBarcodeGroups.RowHeadersVisible = true;
            dgvBarcodeGroups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvBarcodeGroups.AddColumn("档位1位置", "档位1位置", readOnly: false);
            dgvBarcodeGroups.AddColumn("档位1内容", "档位1内容", readOnly: false);

            dgvBarcodeGroups.AddColumn("档位2位置", "档位2位置", readOnly: false);
            dgvBarcodeGroups.AddColumn("档位2内容", "档位2内容", readOnly: false);

            dgvBarcodeGroups.AddColumn("档位3位置", "档位3位置", readOnly: false);
            dgvBarcodeGroups.AddColumn("档位3内容", "档位3内容", readOnly: false);

            dgvBarcodeGroups.AddColumn("档位4位置", "档位4位置", readOnly: false);
            dgvBarcodeGroups.AddColumn("档位4内容", "档位4内容", readOnly: false);

            dgvBarcodeGroups.AddColumn("档位5位置", "档位5位置", readOnly: false);
            dgvBarcodeGroups.AddColumn("档位5内容", "档位5内容", readOnly: false);

            dgvBarcodeGroups.AddColumn("档位6位置", "档位6位置", readOnly: false);
            dgvBarcodeGroups.AddColumn("档位6内容", "档位6内容", readOnly: false);

            dgvBarcodeGroups.AddColumn("档位7位置", "档位7位置", readOnly: false);
            dgvBarcodeGroups.AddColumn("档位7内容", "档位7内容", readOnly: false);

            dgvBarcodeGroups.AddColumn("档位8位置", "档位8位置", readOnly: false);
            dgvBarcodeGroups.AddColumn("档位8内容", "档位8内容", readOnly: false);

            if (isFirst)
                cmbBarcodeList.SelectedIndexChanged += cmbBarcodeList_SelectedIndexChanged;

            if (VisionCommon.VisionConfig.BarcodeInfo == null)
                VisionCommon.VisionConfig.BarcodeInfo = new VisionConfigBarcode[0];

            cmbBarcodeList.Items.Clear();
            foreach (var t in VisionCommon.VisionConfig.BarcodeInfo)
                cmbBarcodeList.Items.Add(t.Name);

            if (cmbBarcodeList.Items.Count > 0)
                cmbBarcodeList.SelectedIndex = 0;
            else
            {
                cmbBarcodeList.Text = string.Empty;
                txtKeyL.Text = string.Empty;
                txtKeyR.Text = string.Empty;
            }
        }

        private void cmbBarcodeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvBarcodeGroups.ClearRows();

            if (VisionCommon.VisionConfig.BarcodeInfo != null)
            {
                var b = VisionCommon.VisionConfig.BarcodeInfo.ToList().Find(f => f.Name == cmbBarcodeList.Text);
                if (b != null)
                {
                    if (b.KeyWord != null && b.KeyWord.Value != null)
                    {
                        txtKeyL.Text = b.KeyWord.Value.L;
                        txtKeyR.Text = b.KeyWord.Value.R;
                        txtKeyIndex.Value = int.Parse(b.KeyWord.Index) + 1;
                        txtBarocdeLen.Value = int.Parse(b.Length);
                    }

                    var row = 0;

                    foreach (var t in b.Groups)
                    {
                        dgvBarcodeGroups.AddRow();
                        var cell = 0;

                        for (var i = 0; i < t.Length; i++)
                        {
                            var sp = t[i].Split('，');

                            dgvBarcodeGroups.Rows[row].Cells[cell].Value = int.Parse(sp[0]) + 1;
                            dgvBarcodeGroups.Rows[row].Cells[cell + 1].Value = sp[1];
                            cell = cell + 2;
                        }

                        row++;
                    }
                }
            }
        }

        private void btnUpdateBarcode_Click(object sender, EventArgs e)
        {
            dgvBarcodeGroups.EndEdit();

            var barcodeName = cmbBarcodeList.Text.Trim();
            var barcodeLength = txtBarocdeLen.Value;
            var barcodeKeyL = txtKeyL.Text.Trim();
            var barcodeKeyR = txtKeyR.Text.Trim();
            var barcodeKeyIndex = txtKeyIndex.Value;

            if (string.IsNullOrEmpty(barcodeKeyL) || string.IsNullOrEmpty(barcodeKeyR))
            {
                this.ShowErrorTip("L、R关键字不能为空");
                return;
            }

            if (barcodeKeyL.Length != barcodeKeyR.Length)
            {
                this.ShowErrorTip("L、R关键字长度不一致");
                return;
            }

            if (barcodeLength <= 0 || barcodeKeyIndex <= 0)
            {
                this.ShowErrorTip("长度或索引不能<1");
                return;
            }

            if (barcodeKeyIndex - 1 + barcodeKeyL.Length > barcodeLength)
            {
                this.ShowErrorTip("关键字长度超过二维码长度");
                return;
            }

            if (dgvBarcodeGroups.RowCount > 1)
            {
                var activeRowCount = dgvBarcodeGroups.RowCount - 1;

                var row = dgvBarcodeGroups.Rows[0];

                for (var j = 0; j < 15; j = j + 2)
                {
                    if (row.Cells[j].Value == null || string.IsNullOrEmpty(row.Cells[j].Value.ToString()))
                    {
                        for (var k = 0; k < activeRowCount; k++)
                        {
                            var toCheckRow = dgvBarcodeGroups.Rows[k];
                            var value = toCheckRow.Cells[j].Value;
                            var str = string.Empty;
                            if (value != null)
                                str = value.ToString();

                            var value2 = toCheckRow.Cells[j + 1].Value;
                            var str2 = string.Empty;
                            if (value2 != null)
                                str2 = value2.ToString();

                            if (value != null || !string.IsNullOrEmpty(str) || value2 != null || !string.IsNullOrEmpty(str2))
                            {
                                this.ShowErrorTip("档位设置应该以第一行为基准，第一行未填的，剩余行都不应填写");
                                return;
                            }
                        }
                    }
                    else
                    {
                        var cellStr = row.Cells[j].Value.ToString();
                        int groupIndex;
                        if (!int.TryParse(cellStr, out groupIndex))
                        {
                            this.ShowErrorTip("档位位置必须为整数");
                            return;
                        }

                        if (groupIndex <= 0)
                        {
                            this.ShowErrorTip("档位位置不能<1");
                            return;
                        }

                        if (row.Cells[j + 1].Value == null || string.IsNullOrEmpty(row.Cells[j + 1].Value.ToString()))
                        {
                            this.ShowErrorTip("档位内容缺失");
                            return;
                        }

                        var groupValue = row.Cells[j + 1].Value.ToString();

                        if (groupIndex - 1 + groupValue.Length > barcodeLength)
                        {
                            this.ShowErrorTip("档位内容长度超过二维码长度");
                            return;
                        }

                        if (groupValue.Contains("，"))
                        {
                            this.ShowErrorTip("档位内容不能包含'，'");
                            return;
                        }

                        for (var k = 0; k < activeRowCount; k++)
                        {
                            var toCheckRow = dgvBarcodeGroups.Rows[k];

                            if (toCheckRow.Cells[j].Value == null || toCheckRow.Cells[j].Value.ToString() != cellStr)
                            {
                                this.ShowErrorTip("同一列的档位位置必须相同");
                                return;
                            }

                            if (toCheckRow.Cells[j + 1].Value == null || string.IsNullOrEmpty(toCheckRow.Cells[j + 1].Value.ToString()))
                            {
                                this.ShowErrorTip("档位内容缺失");
                                return;
                            }

                            var toCheckGroupValue = toCheckRow.Cells[j + 1].Value.ToString();

                            if (toCheckGroupValue.Length != groupValue.Length)
                            {
                                this.ShowErrorTip("同一列的档位内容长度必须相同");
                                return;
                            }
                        }
                    }
                }
            }

            if (!this.ShowAskDialog(string.Format("确定更新二维码信息:{0}？", cmbBarcodeList.Text)))
            {
                this.ShowInfoTip("用户取消了操作");
                return;
            }

            var temp = VisionCommon.VisionConfig.BarcodeInfo.ToList();
            var findIndex = temp.FindIndex(f => f.Name == cmbBarcodeList.Text);
            if (findIndex != -1)
            {
                temp[findIndex] = new VisionConfigBarcode
                {
                    Name = barcodeName,
                    Length = barcodeLength.ToString(),
                    KeyWord = new VisionConfigBarcodeKeyWord
                    {
                        Index = (barcodeKeyIndex - 1).ToString(),
                        Value = new VisionConfigBarcodeKeyWordValue
                        {
                            L = barcodeKeyL,
                            R = barcodeKeyR
                        }
                    }
                };

                if (dgvBarcodeGroups.RowCount > 1)
                {
                    var activeRowCount = dgvBarcodeGroups.RowCount - 1;
                    temp[findIndex].Groups = new string[activeRowCount][];

                    for (var i = 0; i < activeRowCount; i++)
                    {
                        var groups = new List<string>();

                        for (var j = 0; j < 16; j = j + 2)
                        {
                            if (dgvBarcodeGroups.Rows[i].Cells[j].Value != null)
                            {
                                groups.Add(string.Format("{0}，{1}", int.Parse(dgvBarcodeGroups.Rows[i].Cells[j].Value.ToString()) - 1, dgvBarcodeGroups.Rows[i].Cells[j + 1].Value));
                            }
                        }

                        temp[findIndex].Groups[i] = groups.ToArray();
                    }
                }
                else
                {
                    temp[findIndex].Groups = new string[0][];
                }

                VisionCommon.VisionConfig.BarcodeInfo = temp.ToArray();
                VisionCommon.VisionConfig.DeviceInfo.DeviceGuid = Guid.NewGuid().ToString();
                VisionCommon.VisionConfig.DeviceInfo.DeviceUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                XmlHelper.SerializeToFile(VisionCommon.VisionConfig, VisionCommon.FilePath, Encoding.UTF8);
                this.ShowSuccessDialog("二维码信息更新成功！");
            }
        }

        private void btnAddNewBarcode_Click(object sender, EventArgs e)
        {
            var option = new UIEditOption { AutoLabelWidth = true };
            option.AddText("BarcodeName", "名称：", null, true);
            option.AddInteger("BarcodeLength", "二维码长度：", 35);
            option.AddText("BarcodeKeyL", "关键字(L)：", null, true);
            option.AddText("BarcodeKeyR", "关键字(R)：", null, true);
            option.AddInteger("BarcodeKeyIndex", "关键字位置：", 1);

            var frmAddNewBarcode = new UIEditForm(option);
            frmAddNewBarcode.CheckedData += frmAddNewBarcode_CheckedData;
            frmAddNewBarcode.Render();
            frmAddNewBarcode.ShowDialog();

            if (!frmAddNewBarcode.IsOK)
            {
                this.ShowInfoTip("用户取消了操作");
                return;
            }
            var barcodeName = frmAddNewBarcode["BarcodeName"].ToString().Trim();
            var barcodeLength = (int)frmAddNewBarcode["BarcodeLength"];
            var barcodeKeyL = frmAddNewBarcode["BarcodeKeyL"].ToString().Trim();
            var barcodeKeyR = frmAddNewBarcode["BarcodeKeyR"].ToString().Trim();
            var barcodeKeyIndex = (int)frmAddNewBarcode["BarcodeKeyIndex"];

            if (VisionCommon.VisionConfig.BarcodeInfo == null)
                VisionCommon.VisionConfig.BarcodeInfo = new VisionConfigBarcode[0];

            var temp = VisionCommon.VisionConfig.BarcodeInfo.ToList();

            var newBarcode = new VisionConfigBarcode
            {
                Name = barcodeName,
                Length = barcodeLength.ToString(),
                KeyWord = new VisionConfigBarcodeKeyWord
                {
                    Index = (barcodeKeyIndex - 1).ToString(),
                    Value = new VisionConfigBarcodeKeyWordValue
                    {
                        L = barcodeKeyL,
                        R = barcodeKeyR
                    }
                },
                Groups = new string[0][]
            };

            temp.Add(newBarcode);

            VisionCommon.VisionConfig.BarcodeInfo = temp.ToArray();
            VisionCommon.VisionConfig.DeviceInfo.DeviceGuid = Guid.NewGuid().ToString();
            VisionCommon.VisionConfig.DeviceInfo.DeviceUpdateTime =
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            XmlHelper.SerializeToFile(VisionCommon.VisionConfig, VisionCommon.FilePath, Encoding.UTF8);
            this.ShowSuccessDialog("二维码信息添加成功！");

            cmbBarcodeList.Items.Add(newBarcode.Name);
            cmbBarcodeList.SelectedIndex = cmbBarcodeList.Items.Count - 1;
        }

        private bool frmAddNewBarcode_CheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            var barcodeName = e.Form["BarcodeName"].ToString().Trim();
            var barcodeLength = (int)e.Form["BarcodeLength"];
            var barcodeKeyL = e.Form["BarcodeKeyL"].ToString().Trim();
            var barcodeKeyR = e.Form["BarcodeKeyR"].ToString().Trim();
            var barcodeKeyIndex = (int)e.Form["BarcodeKeyIndex"];

            if (VisionCommon.VisionConfig.BarcodeInfo != null &&
                VisionCommon.VisionConfig.BarcodeInfo.ToList().FindIndex(f => f.Name == barcodeName) != -1)
            {
                this.ShowErrorTip("名称重复");
                return false;
            }

            if (barcodeKeyL.Length != barcodeKeyR.Length)
            {
                this.ShowErrorTip("L、R关键字长度不一致");
                return false;
            }

            if (barcodeLength <= 0 || barcodeKeyIndex <= 0)
            {
                this.ShowErrorTip("长度或索引不能<1");
                return false;
            }

            if (barcodeKeyIndex - 1 + barcodeKeyL.Length > barcodeLength)
            {
                this.ShowErrorTip("关键字长度超过二维码长度");
                return false;
            }

            return true;
        }

        private void btnDeleteBarocode_Click(object sender, EventArgs e)
        {
            var temp = VisionCommon.VisionConfig.BarcodeInfo.ToList();
            var findIndex = temp.FindIndex(f => f.Name == cmbBarcodeList.Text);
            if (findIndex != -1)
            {
                if (!this.ShowAskDialog(string.Format("确定删除二维码信息:{0}？", cmbBarcodeList.Text)))
                {
                    this.ShowInfoTip("用户取消了操作");
                    return;
                }
                temp.RemoveAt(findIndex);
                InitBarcodeDgv(true);

                VisionCommon.VisionConfig.BarcodeInfo = temp.ToArray();
                VisionCommon.VisionConfig.DeviceInfo.DeviceGuid = Guid.NewGuid().ToString();
                VisionCommon.VisionConfig.DeviceInfo.DeviceUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                XmlHelper.SerializeToFile(VisionCommon.VisionConfig, VisionCommon.FilePath, Encoding.UTF8);
                this.ShowSuccessDialog("二维码信息删除成功！");
            }
        }

        #endregion

        #region Custom Controllers

        private void IninCustomControllers(bool isFirst)
        {
            txtCustomControllerCodeRevicw.Clear();
            cmbCustomControllers.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCustomControllers.Items.Clear();

            if (VisionCommon.VisionConfig.DeviceInfo.CustomContrller == null)
                VisionCommon.VisionConfig.DeviceInfo.CustomContrller = new VisionConfigDeviceInfoCustomContrller[0];

            foreach (var t in VisionCommon.VisionConfig.DeviceInfo.CustomContrller)
                cmbCustomControllers.Items.Add(t.Name);

            if (isFirst)
                cmbCustomControllers.SelectedIndexChanged += cmbCustomControllers_SelectedIndexChanged;

            if (cmbCustomControllers.Items.Count > 0)
                cmbCustomControllers.SelectedIndex = 0;
        }

        private void cmbCustomControllers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VisionCommon.VisionConfig.DeviceInfo.CustomContrller == null) 
                return;

            var controllerName = cmbCustomControllers.Text;

            var controller =
                VisionCommon.VisionConfig.DeviceInfo.CustomContrller.ToList().Find(f => f.Name == controllerName);
            txtCustomControllerCodeRevicw.Clear();
            if (controller != null && !string.IsNullOrEmpty(controller.InitPara))
                txtCustomControllerCodeRevicw.AppendText(controller.InitPara);
        }

        private void btnAddCustomController_Click(object sender, EventArgs e)
        {
            var dirPath = Program.SysDir;
            var controllerPath = dirPath + @"\Controller.dll";
            var asmb = Assembly.LoadFrom(controllerPath);
            var types = asmb.GetTypes().ToList().FindAll(t => t.BaseType != null && t.BaseType.Name == "ControllerBase");

            var optionNewCustomController = new UIEditOption { Text = "添加自定义控制器" };
            optionNewCustomController.AddText("ControllerName", "自定义控制器名称：", null, true);

            var controllerType = types.Select(type => new ControllerStruct { Name = type.Name }).Select(con => con.Name).ToArray();
            optionNewCustomController.AddCombobox("ControllerType", "控制器类型：", controllerType, 0);

            var frmAddCustomControllerFrm = new UIEditForm(optionNewCustomController);
            frmAddCustomControllerFrm.CheckedData += frmAddCustomControllerFrm_CheckedData;
            frmAddCustomControllerFrm.Render();
            frmAddCustomControllerFrm.ShowDialog();

            if (frmAddCustomControllerFrm.IsOK)
            {
                var cName = frmAddCustomControllerFrm["ControllerName"].ToStringExt();
                var cType = controllerType[(int)frmAddCustomControllerFrm["ControllerType"]];

                if (VisionCommon.VisionConfig.DeviceInfo.CustomContrller == null)
                    VisionCommon.VisionConfig.DeviceInfo.CustomContrller = new VisionConfigDeviceInfoCustomContrller[0];
                var temp = VisionCommon.VisionConfig.DeviceInfo.CustomContrller.ToList();
                temp.Add(new VisionConfigDeviceInfoCustomContrller { InitPara = string.Empty, Name = cName, Type = cType });
                VisionCommon.VisionConfig.DeviceInfo.CustomContrller = temp.ToArray();
                VisionCommon.VisionConfig.DeviceInfo.DeviceGuid = Guid.NewGuid().ToString();
                VisionCommon.VisionConfig.DeviceInfo.DeviceUpdateTime =
                        DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                XmlHelper.SerializeToFile(VisionCommon.VisionConfig, VisionCommon.FilePath, Encoding.UTF8);

                this.ShowSuccessDialog("自定义控制器添加成功！");
                IninCustomControllers(false);
            }
            else
            {
                this.ShowInfoTip("用户取消了操作");
            }
        }

        private bool frmAddCustomControllerFrm_CheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            var cName = e.Form["ControllerName"].ToStringExt();
            if (VisionCommon.VisionConfig.DeviceInfo.CustomContrller != null &&
                VisionCommon.VisionConfig.DeviceInfo.CustomContrller.ToList().FindIndex(f => f.Name == cName) != -1)
            {
                this.ShowErrorTip("名称重复");
                return false;
            }

            return true;
        }

        private void btnDeleteCustomController_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbCustomControllers.Text))
            {
                this.ShowErrorTip("未添加自定义控制器，无需删除");
                return;
            }
            if (!this.ShowAskDialog(string.Format("确定删除自定义控制器：{0}", cmbCustomControllers.Text)))
            {
                this.ShowInfoTip("用户取消了操作");
                return;
            }
            if (VisionCommon.VisionConfig.DeviceInfo.CustomContrller == null)
                VisionCommon.VisionConfig.DeviceInfo.CustomContrller = new VisionConfigDeviceInfoCustomContrller[0];
            var temp = VisionCommon.VisionConfig.DeviceInfo.CustomContrller.ToList();
            var findIndex = temp.FindIndex(f => f.Name == cmbCustomControllers.Text);
            if (findIndex != -1)
                temp.RemoveAt(findIndex);
            VisionCommon.VisionConfig.DeviceInfo.CustomContrller = temp.ToArray();
            VisionCommon.VisionConfig.DeviceInfo.DeviceGuid = Guid.NewGuid().ToString();
            VisionCommon.VisionConfig.DeviceInfo.DeviceUpdateTime =
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            XmlHelper.SerializeToFile(VisionCommon.VisionConfig, VisionCommon.FilePath, Encoding.UTF8);

            this.ShowSuccessDialog("自定义控制器删除成功！");
            IninCustomControllers(false);
        }

        private void btnConfigCustomController_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbCustomControllers.Text))
            {
                this.ShowErrorTip("未添加自定义控制器，无需配置");
                return;
            }

            if (VisionCommon.VisionConfig.DeviceInfo.CustomContrller == null)
                VisionCommon.VisionConfig.DeviceInfo.CustomContrller = new VisionConfigDeviceInfoCustomContrller[0];
            var temp = VisionCommon.VisionConfig.DeviceInfo.CustomContrller.ToList();
            var findIndex = temp.FindIndex(f => f.Name == cmbCustomControllers.Text);
            if (findIndex != -1)
            {
                var frm = new FrmMethodConfig(temp[findIndex].InitPara, temp[findIndex].Name);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    temp[findIndex].InitPara = frm.MethodStr;
                    VisionCommon.VisionConfig.DeviceInfo.CustomContrller = temp.ToArray();
                    VisionCommon.VisionConfig.DeviceInfo.DeviceGuid = Guid.NewGuid().ToString();
                    VisionCommon.VisionConfig.DeviceInfo.DeviceUpdateTime =
                        DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    XmlHelper.SerializeToFile(VisionCommon.VisionConfig, VisionCommon.FilePath, Encoding.UTF8);
                    this.ShowSuccessDialog("配置控制器添加成功！");
                    txtCustomControllerCodeRevicw.Clear();
                    txtCustomControllerCodeRevicw.AppendText(temp[findIndex].InitPara);
                }
            }
        }

        public class ControllerStruct
        {
            public string Name;
            public readonly List<string> LstMethods = new List<string>();
            public readonly List<string> LstFields = new List<string>();
            public readonly List<string> LstProperties = new List<string>();
        }

        #endregion

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var isNeed = false;
                var delay = 1000;
                var relay = "继电器10;";

                try
                {
                    if (string.Equals(
                            VisionCommon.VisionConfig.DeviceInfo.Actions.PropulsionCylinder.IsPropulsionCylinder,
                            "true", StringComparison.CurrentCultureIgnoreCase))
                    {
                        isNeed = true;
                    }

                    delay = int.Parse(VisionCommon.VisionConfig.DeviceInfo.Actions.PropulsionCylinder.Delay);
                    relay = VisionCommon.VisionConfig.DeviceInfo.Actions.PropulsionCylinder.Binding;
                }
                catch (Exception exception)
                {
                    // ignored
                }

                var option = new UIEditOption
                {
                    AutoLabelWidth = true,
                    Text = "确认"
                };

                var relays = new[]
                {
                    "继电器1;", "继电器2;", "继电器3;",
                    "继电器6;", "继电器5;", "继电器4;",
                    "继电器7;", "继电器8;", "继电器9;",
                    "继电器10;", "继电器11;", "继电器12;",
                    "继电器13;", "继电器14;", "继电器15;",
                    "继电器16;", "继电器17;", "继电器18;"
                };

                var findIndex = relays.ToList().FindIndex(f => f == relay);

                option.AddSwitch("IsNeed", "启动前是否需要气缸预压", isNeed);
                option.AddCombobox("Relay", "绑定继电器", relays, selectedIndex: findIndex == -1 ? 0 : findIndex);
                option.AddInteger("Delay", "延时", delay);

                var frm = new UIEditForm(option);
                frm.Render();
                frm.ShowDialog();

                if (!frm.IsOK)
                {
                    this.ShowInfoTip("用户取消了操作");
                    return;
                }

                if (delay < 0)
                {
                    this.ShowInfoTip("延时不能为负");
                    return;
                }

                var a = (bool)frm["IsNeed"];
                var b = frm["Delay"].ToString();
                var c = relays[(int)frm["Relay"]];

                VisionCommon.VisionConfig.DeviceInfo.Actions.PropulsionCylinder =
                    new VisionConfigDeviceInfoActionsPropulsionCylinder
                    {
                        IsPropulsionCylinder = a.ToString(),
                        Delay = b.ToString(),
                        Binding = c.ToString()
                    };

                VisionCommon.VisionConfig.DeviceInfo.DeviceGuid = Guid.NewGuid().ToString();
                VisionCommon.VisionConfig.DeviceInfo.DeviceUpdateTime =
                    DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                XmlHelper.SerializeToFile(VisionCommon.VisionConfig, VisionCommon.FilePath, Encoding.UTF8);

                this.ShowSuccessDialog("保存成功！");
            }
            catch (Exception exception)
            {
                this.ShowErrorTip("操作异常：" + exception.Message);
            }
        }
    }
}
