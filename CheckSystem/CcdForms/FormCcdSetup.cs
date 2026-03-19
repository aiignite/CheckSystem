using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Controller;
using StateMachine;

namespace CheckSystem.CcdForms
{
    public partial class FormCcdSetup : Form
    {
        protected readonly string CheckAppName = "检测程序";
        protected readonly string VisionCheckAppName = "图像处理";
        protected readonly string ControllerMaster1Name = "控制器主站IP28";
        protected readonly string ControllerMaster2Name = "控制器主站IP29";
        protected readonly string ControllerSlave1With10RName = "继电器从站0x201";
        protected readonly string ControllerSlave2With10RName = "继电器从站0x202";
        protected readonly string ControllerSlave1With14AdName = "AD从站0x101";
        protected readonly string PowerIt6302Name = "精密电源";
        protected readonly string BarcodeScanerName = "扫码枪";

        private readonly Dictionary<string, string> _stateUnitPosition = new Dictionary<string, string>();

        private readonly List<string> _checkItemsColName = new List<string>();
        private readonly List<FormCcdAddBarcode.BarcodeStruct> _barcodeStructs =
            new List<FormCcdAddBarcode.BarcodeStruct>();

        public FormCcdSetup()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            lblCmbIsLAndR.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            lblCmbIsLAndR.comboBox.Items.Add("L");
            lblCmbIsLAndR.comboBox.Items.Add("R");
            lblCmbIsLAndR.comboBox.Items.Add("否");
            lblCmbIsLAndR.comboBox.SelectedIndex = 0;

            var portsList = SerialPort.GetPortNames();

            lblCmbBarcodeScanerPort.comboBox.Items.Add("192.168.1.2:502");
            foreach (var p in portsList)
                lblCmbBarcodeScanerPort.comboBox.Items.Add(p);
            lblCmbBarcodeScanerPort.comboBox.SelectedIndex = 0;

            lblCmbPowerPort.comboBox.Items.Add("192.168.1.1:502");
            foreach (var p in portsList)
                lblCmbPowerPort.comboBox.Items.Add(p);
            lblCmbPowerPort.comboBox.SelectedIndex = 0;

            lblCmbIsStamp.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            for (var i = 1; i <= 20; i++)
                lblCmbIsStamp.comboBox.Items.Add(string.Format("继电器{0}", i));
            lblCmbIsStamp.comboBox.Items.Add("否");
            lblCmbIsStamp.comboBox.SelectedIndex = 15;

            lblCmbIsReadButton.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            lblCmbIsReadButton.comboBox.Items.Add("否");
            lblCmbIsReadButton.comboBox.Items.Add("是");
            lblCmbIsReadButton.comboBox.SelectedIndex = 0;

            _checkItemsColName.Add("电源是否需要串并联");//0
            _checkItemsColName.Add("电压1上限");//1
            _checkItemsColName.Add("电压2上限");//2
            _checkItemsColName.Add("电压3上限");//3
            _checkItemsColName.Add("电流1上限");//4
            _checkItemsColName.Add("电流2上限");//5
            _checkItemsColName.Add("电流3上限");//6
            _checkItemsColName.Add("上电顺序");//7
            _checkItemsColName.Add("是否检测电压或电阻");//8
            _checkItemsColName.Add("电压或电阻档位名称及范围");//9
            _checkItemsColName.Add("输入电压值");//10
            _checkItemsColName.Add("外接电阻阻值");//11
            _checkItemsColName.Add("电压通道");//12
            _checkItemsColName.Add("电压补偿");//13
            _checkItemsColName.Add("是否检测电流");//14
            _checkItemsColName.Add("电流档位名称及范围");//15
            _checkItemsColName.Add("电流通道");//16
            _checkItemsColName.Add("电流补偿");//17
            _checkItemsColName.Add("是否检测光型");//18
            _checkItemsColName.Add("相机序列号");//19
            _checkItemsColName.Add("光型名称");//20

            InitCheckItemDataGridView();
            InitBarcodeDataGridView();

            _stateUnitPosition.Add(StateMachineHelper.EDefaultStateUnits.Idle.ToString().ToUpper(), "-197,-287,107,63");
            _stateUnitPosition.Add(StateMachineHelper.EDefaultStateUnits.CheckFail.ToString().ToUpper(), "-199,527,165,62");
            _stateUnitPosition.Add(StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(), "303,54,152,72");
            _stateUnitPosition.Add(StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString().ToUpper(), "307,520,152,63");
            _stateUnitPosition.Add("检测1", "2166,-1303,180,67");
            _stateUnitPosition.Add("检测2", "3032,-1160,180,80");
            _stateUnitPosition.Add("检测3", "2968,-858,180,80");
            _stateUnitPosition.Add("检测4", "3031,-488,180,80");
            _stateUnitPosition.Add("检测5", "2704,-205,180,80");
            _stateUnitPosition.Add("检测6", "2817,109,180,80");
            _stateUnitPosition.Add("检测7", "2851,462,180,80");
            _stateUnitPosition.Add("检测8", "2872,772,180,80");
            _stateUnitPosition.Add("检测9", "2327,313,180,80");
            _stateUnitPosition.Add("检测10", "1814,515,180,80");
            _stateUnitPosition.Add("检测11", "2147,857,180,80");
            _stateUnitPosition.Add("检测12", "1794,1164,180,80");
            _stateUnitPosition.Add("检测13", "1345,516,180,80");
            _stateUnitPosition.Add("检测14", "782,974,180,80");
            _stateUnitPosition.Add("检测15", "704,594,180,80");

            _stateUnitPosition.Add("扫码1", "-49,-542,180,80");
            _stateUnitPosition.Add("扫码2", "8,-935,180,80");
            _stateUnitPosition.Add("扫码3", "268,-789,180,80");
            _stateUnitPosition.Add("扫码4", "361,-428,180,80");
            _stateUnitPosition.Add("扫码5", "836,-913,180,80");
            _stateUnitPosition.Add("扫码6", "836,-913,180,80");
            _stateUnitPosition.Add("扫码7", "947,-466,180,80");
            _stateUnitPosition.Add("扫码8", "1492,-649,180,80");
            _stateUnitPosition.Add("扫码9", "2060,-675,180,80");
            _stateUnitPosition.Add("扫码10", "975,-239,180,80");
            _stateUnitPosition.Add("扫码11", "983,105,180,80");
            _stateUnitPosition.Add("扫码12", "1448,-157,180,80");
            _stateUnitPosition.Add("扫码13", "1796,-316,180,80");
            _stateUnitPosition.Add("扫码14", "2268,-296,180,80");
            _stateUnitPosition.Add("扫码15", "2336,-863,180,80");
        }

        private void InitCheckItemDataGridView()
        {
            dgvCheckItems.label.Text = @"检测项列表";
            var dataGridView = dgvCheckItems.dataGridView;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AllowUserToResizeColumns = true;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //userDataGridGrayList.dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView.Margin = new Padding(3, 4, 3, 4);
            dataGridView.RowTemplate.Height = 30;
            dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 10, FontStyle.Regular);
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSkyBlue;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

            //获取控件的Type,设置双缓存
            var dgvType = dataGridView.GetType();
            var properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //为控件的属性设置值
            if (properInfo != null) properInfo.SetValue(dataGridView, true, null);

            foreach (var t in _checkItemsColName)
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = t });
            for (var i = 0; i < dataGridView.Columns.Count; i++)
                dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void InitBarcodeDataGridView()
        {
            treeView.Nodes.Clear();

            var treeRootNode = new TreeNode("二维码列表", 1, 1);
            treeView.Nodes.Add(treeRootNode);

            treeView.Nodes[0].BackColor = Color.DarkOrange;

            treeView.ExpandAll();
            treeView.NodeMouseClick += treeView_NodeMouseClick;

            dgvBarocodeList.label.Text = @"二维码列表";
            dgvBarocodeList.label.Height = 30;
            dgvBarocodeList.dataGridView.ReadOnly = true;
            dgvBarocodeList.dataGridView.AllowUserToDeleteRows = false;
            dgvBarocodeList.dataGridView.AllowUserToAddRows = false;

            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "名称" }); //0
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "长度" }); //1
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "零件号" }); //2
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "零件号位置" }); //3
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "硬件版本号" }); //4
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "硬件版本号位置" }); //5
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "软件版本号" }); //6
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "软件版本号位置" }); //7
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "匹配码" }); //8
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "匹配码位置" }); //9
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位1" }); //10
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位1位置" }); //11
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位2" }); //12
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位2位置" }); //13
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位3" }); //14
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位3位置" }); //15
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位4" }); //16
            dgvBarocodeList.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "档位4位置" }); //17
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (e.Node.Nodes.Count == 0 && e.Node.Text != @"二维码列表")
                    {
                        dgvBarocodeList.dataGridView.Rows.Clear();

                        var findIndex = _barcodeStructs.FindIndex(f => f.Name == e.Node.Text);
                        if (findIndex != -1)
                        {
                            var rowNum = dgvBarocodeList.dataGridView.Rows.Add();
                            var row = dgvBarocodeList.dataGridView.Rows[rowNum];

                            row.Cells[0].Value = _barcodeStructs[findIndex].Name;
                            row.Cells[1].Value = _barcodeStructs[findIndex].Length;
                            row.Cells[2].Value = _barcodeStructs[findIndex].PartNo;
                            row.Cells[3].Value = _barcodeStructs[findIndex].PartNoIndex;
                            row.Cells[4].Value = _barcodeStructs[findIndex].Hardware;
                            row.Cells[5].Value = _barcodeStructs[findIndex].HardwareIndex;
                            row.Cells[6].Value = _barcodeStructs[findIndex].Software;
                            row.Cells[7].Value = _barcodeStructs[findIndex].SofrwareIndex;

                            foreach (var t in _barcodeStructs[findIndex].GearList)
                            {
                                row.Cells[8].Value = t.MatchCode;
                                row.Cells[9].Value = t.MatchCodeIndex;
                                row.Cells[10].Value = t.Gear1;
                                row.Cells[11].Value = t.Gear1Index;
                                row.Cells[12].Value = t.Gear2;
                                row.Cells[13].Value = t.Gear2Index;
                                row.Cells[14].Value = t.Gear3;
                                row.Cells[15].Value = t.Gear3Index;
                                row.Cells[16].Value = t.Gear4;
                                row.Cells[17].Value = t.Gear4Index;
                            }
                        }
                    }
                    break;

                case MouseButtons.None:
                    break;

                case MouseButtons.Right:
                    var pos = new Point(e.Node.Bounds.X + e.Node.Bounds.Width, e.Node.Bounds.Y + e.Node.Bounds.Height / 2);
                    contextMenuStripTree.Show(treeView, pos);
                    break;

                case MouseButtons.Middle:
                    break;

                case MouseButtons.XButton1:
                    break;

                case MouseButtons.XButton2:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void 添加检测项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormCcdAddCheckItems(dgvCheckItems.dataGridView))
                form.ShowDialog();
        }

        private void 添加二维码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newBarocde = new FormCcdAddBarcode.BarcodeStruct();
            var action = new Action<FormCcdAddBarcode.BarcodeStruct>(para =>
            {
                newBarocde = para;
            });

            using (var form = new FormCcdAddBarcode(action))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    treeView.Nodes[0].Nodes.Clear();
                    _barcodeStructs.Add(newBarocde);

                    foreach (var t in _barcodeStructs)
                    {
                        treeView.Nodes[0].Nodes.Add(new TreeNode
                        {
                            Text = t.Name,
                            Tag = t.Name
                        });
                    }
                }
            }

            treeView.ExpandAll();
            dgvBarocodeList.dataGridView.Rows.Clear();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = treeView.SelectedNode;
            if (node.Nodes.Count == 0 && node.Text != @"二维码列表")
            {
                var barcodeName = node.Text;
                var findIndex = _barcodeStructs.FindIndex(f => f.Name == barcodeName);
                if (findIndex != -1)
                    _barcodeStructs.RemoveAt(findIndex);

                treeView.Nodes[0].Nodes.Clear();

                foreach (var t in _barcodeStructs)
                {
                    treeView.Nodes[0].Nodes.Add(new TreeNode
                    {
                        Text = t.Name,
                        Tag = t.Name
                    });
                }
            }

            treeView.ExpandAll();
            dgvBarocodeList.dataGridView.Rows.Clear();
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var productName = lblTxtProductName.textBox.Text;
            var deviceNo = lblTxtDeviceNo.textBox.Text;
            var processNo = lblTxtProductNo.textBox.Text;
            if (lblCmbIsLAndR.comboBox.Text != @"否")
            {
                processNo += lblCmbIsLAndR.comboBox.Text;
                productName += lblCmbIsLAndR.comboBox.Text;
            }
            processNo += "001";

            var deviceConfig = new DeviceConfig
            {
                DeviceInfo = new DeviceConfigDeviceInfo
                {
                    DeviceName = productName,
                    DeviceNo = deviceNo,
                    DeviceVersion = "V01",
                    DeviceUpdateTime = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss")
                },
                ControllerProperties = new DeviceConfigProperty[0],
                Parts = new DeviceConfigPart[0],
                Processes = new DeviceConfigProcess[0],
                WorkStations = new DeviceConfigWorkStation[1],
                Controllers = new DeviceConfigController[1 + 1 + 1 + 1 + 1 + 1 + 2 + 1]
            };

            #region 工作站
            deviceConfig.WorkStations[0] = new DeviceConfigWorkStation
            {
                Name = "CCD检测主程序"
            };
            #endregion

            #region 界面

            deviceConfig.FormLayout = new DeviceConfigFormLayout
            {
                ColumnCount = 1.ToString(),
                RowCount = 1.ToString(),
                ColumnPercent = "25,25,25,25,25",
                RowPixCount = 30.ToString()
            };

            deviceConfig.Controls = new DeviceConfigControl[1];
            deviceConfig.Controls[0] = new DeviceConfigControl
            {
                Name = "检测数据",
                Type = "NiImageViewer",
                RowPosition = 0.ToString(),
                ColumnPosition = 0.ToString(),
                RowSpan = 1.ToString(),
                ColumnSpan = 1.ToString()
            };
            #endregion

            #region Controlers
            deviceConfig.Controllers[0] = new DeviceConfigController
            {
                Name = CheckAppName,
                Note = CheckAppName,
                Type = typeof(CheckApp).Name
            };

            deviceConfig.Controllers[1] = new DeviceConfigController
            {
                Name = VisionCheckAppName,
                Note = VisionCheckAppName,
                Type = typeof(LedVisionAnalysisByDaHengCamera).Name
            };

            deviceConfig.Controllers[2] = new DeviceConfigController
            {
                Name = ControllerMaster1Name,
                Note = ControllerMaster1Name,
                Type = typeof(SyControllerMaster).Name
            };

            deviceConfig.Controllers[3] = new DeviceConfigController
            {
                Name = ControllerMaster2Name,
                Note = ControllerMaster2Name,
                Type = typeof(SyControllerMaster).Name
            };

            deviceConfig.Controllers[4] = new DeviceConfigController
            {
                Name = ControllerSlave1With10RName,
                Note = ControllerSlave1With10RName,
                Type = typeof(SyControllerSlaveWith10R).Name
            };

            deviceConfig.Controllers[5] = new DeviceConfigController
            {
                Name = ControllerSlave2With10RName,
                Note = ControllerSlave2With10RName,
                Type = typeof(SyControllerSlaveWith10R).Name
            };

            deviceConfig.Controllers[6] = new DeviceConfigController
            {
                Name = ControllerSlave1With14AdName,
                Note = ControllerSlave1With14AdName,
                Type = typeof(SyControllerSlaveWith14Ad).Name
            };

            deviceConfig.Controllers[7] = new DeviceConfigController
            {
                Name = PowerIt6302Name,
                Note = PowerIt6302Name,
                Type = typeof(PowerIt6302).Name
            };

            deviceConfig.Controllers[8] = new DeviceConfigController
            {
                Name = BarcodeScanerName,
                Note = BarcodeScanerName,
                Type = typeof(BarcodeScanReader).Name
            };
            #endregion

            var paras = new List<DeviceConfigPara>();
            var barcodeGroup = new List<DeviceConfigGear>();

            #region 扫码

            foreach (var t in _barcodeStructs)
            {
                var strList = new List<string>();
                for (var i = 0; i < t.Length; i++)
                    strList.Add("?");

                for (var i = 0; i < t.PartNo.Length; i++)
                    strList[t.PartNoIndex - 1 + i] = t.PartNo[i].ToString();

                for (var i = 0; i < t.Software.Length; i++)
                    strList[t.SofrwareIndex - 1 + i] = t.Software[i].ToString();

                for (var i = 0; i < t.Hardware.Length; i++)
                    strList[t.HardwareIndex - 1 + i] = t.Hardware[i].ToString();

                var str = strList.Aggregate(string.Empty, (current, s) => current + s);

                paras.Add(new DeviceConfigPara
                {
                    ProcessNo = processNo,
                    Name = t.Name,
                    ControllerField = string.Format("{0}.Field.{1}", BarcodeScanerName, "GetBarcodeStr"),
                    DataType = t.GearList.Any() ? "BarcodeGroup" : "Barcode",
                    Value = str
                });

                if (t.GearList.Any())
                {
                    barcodeGroup.AddRange(t.GearList.Select(g => new DeviceConfigGear
                    {
                        Name = t.Name,
                        Gear1Content = g.Gear1,
                        Gear1Index = g.Gear1Index.ToString(),
                        Gear2Content = g.Gear2,
                        Gear2Index = g.Gear2Index.ToString(),
                        Gear3Content = g.Gear3,
                        Gear3Index = g.Gear3Index.ToString(),
                        Gear4Content = g.Gear4,
                        Gear4Index = g.Gear4Index.ToString(),
                        MatchingCodeContent = g.MatchCode,
                        MatchingCodeIndex = g.MatchCodeIndex.ToString()
                    }));
                }
            }

            deviceConfig.Gears = barcodeGroup.ToArray();

            #endregion

            #region 检测参数

            var paraList = new Dictionary<string, List<string>>();

            try
            {
                for (var i = 0; i < dgvCheckItems.dataGridView.RowCount; i++)
                {
                    var row = dgvCheckItems.dataGridView.Rows[i];
                    paraList.Add(string.Format("检测{0}", i + 1), new List<string>());

                    #region 检测电压或电阻
                    if (row.Cells[8].Value.ToString() != @"否")
                    {
                        var checkItem = row.Cells[9].Value.ToString().StrTrim().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        var groupValue = string.Empty;
                        groupValue += "[";
                        groupValue = checkItem.Aggregate(groupValue, (current, c) => current + c.Split(',')[1] + ",");
                        groupValue = groupValue.TrimEnd(',');
                        groupValue += "]";

                        var minValue = string.Empty;
                        minValue += "[";
                        minValue = checkItem.Aggregate(minValue,
                            (current, c) => current + c.Split(',')[2].Split('~')[0] + ",");
                        minValue = minValue.TrimEnd(',');
                        minValue += "]";

                        var maxValue = string.Empty;
                        maxValue += "[";
                        maxValue = checkItem.Aggregate(maxValue,
                            (current, c) => current + c.Split(',')[2].Split('~')[1] + ",");
                        maxValue = maxValue.TrimEnd(',');
                        maxValue += "]";

                        var controllerField = row.Cells[12].Value.ToString();
                        if (controllerField.StartsWith("AD板电压"))
                            controllerField = string.Format("{0}.Filed.AdVoltage{1}", ControllerSlave1With14AdName,
                                controllerField.Replace("AD板电压", string.Empty));
                        else if (controllerField.StartsWith("精密电源电压"))
                            controllerField = string.Format("{0}.Filed.VoltageRead{1}", ControllerSlave1With14AdName,
                                controllerField.Replace("精密电源电压", string.Empty));

                        if (row.Cells[8].Value.ToString() == "电压")
                        {
                            paras.Add(new DeviceConfigPara
                            {
                                ProcessNo = processNo,
                                Name = checkItem[0].Split(',')[0],
                                DataType = "LedGroup",
                                Value = groupValue,
                                Min = minValue,
                                Max = maxValue,
                                Unit = "V",
                                ControlName = "检测数据",
                                ControllerField = controllerField,
                                ControllerFieldOffset = row.Cells[13].Value.ToString()
                            });

                            paraList[string.Format("检测{0}", i + 1)].Add(checkItem[0].Split(',')[0]);
                        }
                        else if (row.Cells[8].Value.ToString() == "电阻")
                        {
                            var voltName = string.Format("{0}两端电压", checkItem[0].Split(',')[0]);

                            paras.Add(new DeviceConfigPara
                            {
                                ProcessNo = processNo,
                                Name = voltName,
                                DataType = "double",
                                Min = 0.ToString(),
                                Max = 9999.ToString(),
                                Unit = "V",
                                ControlName = "检测数据",
                                ControllerField = controllerField,
                                ControllerFieldOffset = row.Cells[13].Value.ToString(),
                            });

                            paras.Add(new DeviceConfigPara
                            {
                                ProcessNo = processNo,
                                Name = checkItem[0].Split(',')[0],
                                DataType = "LedGroup",
                                Value = groupValue,
                                Min = minValue,
                                Max = maxValue,
                                Unit = "Ω",
                                ControlName = "检测数据",
                                ControllerField = string.Empty,
                                ControllerFieldOffset =
                                    string.Format("({0}*{1})/({2}-{3})", row.Cells[11].Value, voltName,
                                        row.Cells[10].Value, voltName) //(20*LB的NTC电压)/(设备输入电压-LB的NTC电压)
                            });

                            paraList[string.Format("检测{0}", i + 1)].Add(checkItem[0].Split(',')[0]);
                        }
                    }
                    #endregion

                    #region 检测电流

                    if (row.Cells[14].Value.ToString() != @"否")
                    {
                        var checkItem = row.Cells[15].Value.ToString().StrTrim().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        var groupValue = string.Empty;
                        groupValue += "[";
                        groupValue = checkItem.Aggregate(groupValue, (current, c) => current + c.Split(',')[1] + ",");
                        groupValue = groupValue.TrimEnd(',');
                        groupValue += "]";

                        var minValue = string.Empty;
                        minValue += "[";
                        minValue = checkItem.Aggregate(minValue,
                            (current, c) => current + c.Split(',')[2].Split('~')[0] + ",");
                        minValue = minValue.TrimEnd(',');
                        minValue += "]";

                        var maxValue = string.Empty;
                        maxValue += "[";
                        maxValue = checkItem.Aggregate(maxValue,
                            (current, c) => current + c.Split(',')[2].Split('~')[1] + ",");
                        maxValue = maxValue.TrimEnd(',');
                        maxValue += "]";

                        var controllerField = row.Cells[16].Value.ToString();
                        if (controllerField.StartsWith("AD板电流"))
                            controllerField = string.Format("{0}.Filed.AdCurrent{1}", ControllerSlave1With14AdName,
                                controllerField.Replace("AD板电流", string.Empty));
                        else if (controllerField.StartsWith("精密电源电流"))
                            controllerField = string.Format("{0}.Filed.CurreatRead{1}", ControllerSlave1With14AdName,
                                    controllerField.Replace("精密电源电流", string.Empty));

                        paras.Add(new DeviceConfigPara
                        {
                            ProcessNo = processNo,
                            Name = checkItem[0].Split(',')[0],
                            DataType = "LedGroup",
                            Value = groupValue,
                            Min = minValue,
                            Max = maxValue,
                            Unit = "mA",
                            ControlName = "检测数据",
                            ControllerField = controllerField,
                            ControllerFieldOffset = row.Cells[17].Value.ToString()
                        });

                        paraList[string.Format("检测{0}", i + 1)].Add(checkItem[0].Split(',')[0]);
                    }

                    #endregion

                    #region 检测光型

                    if (row.Cells[18].Value.ToString() != @"否")
                    {
                        paras.Add(new DeviceConfigPara
                        {
                            ProcessNo = processNo,
                            Name = row.Cells[20].Value.ToString(),
                            DataType = "Vision",
                            ControlName = "检测数据",
                            ControllerField = string.Format("{0}.Field.{1}", VisionCheckAppName, "LedCheckResult")
                        });

                        paraList[string.Format("检测{0}", i + 1)].Add(row.Cells[20].Value.ToString());
                    }

                    #endregion
                }

                deviceConfig.Paras = paras.ToArray();
            }
            catch (Exception)
            {
                // ignored
            }

            //SaveDeviceConfigToFile(deviceConfig, @"C:\Users\B765\Desktop\test20210510.xml", Encoding.UTF8);

            #endregion

            #region 状态单元

            var readDiUnit = new DeviceConfigStatusUnit
            {
                WorkStationName = deviceConfig.WorkStations[0].Name,
                Name = "读启动按钮",
                PositionSize = _stateUnitPosition[string.Format("扫码{0}", _barcodeStructs.Count + 1)],
                DuringFunction = string.Format("{0}.Method.GetMasterDi();\r\n", ControllerMaster1Name),
                DuringNote = string.Format("{0}.Method.GetMasterDi();\r\n", ControllerMaster1Name)
            };

            var tempStateUnit = (from object t in Enum.GetValues(typeof(StateMachineHelper.EDefaultStateUnits))
                                 select new DeviceConfigStatusUnit
                                 {
                                     WorkStationName = deviceConfig.WorkStations[0].Name,
                                     Name = t.ToString().ToUpper(),
                                     PositionSize = _stateUnitPosition[t.ToString().ToUpper()]
                                 }).ToList();

            if (lblCmbIsReadButton.comboBox.Text != @"否")
                tempStateUnit.Add(readDiUnit);

            var idle = tempStateUnit.Find(f => f.Name == StateMachineHelper.EDefaultStateUnits.Idle.ToString().ToUpper());
            if (idle != null)
            {
                var enterFunction = string.Empty;
                enterFunction += string.Format("{0}.Method.InitRemoteIpAddr(192.168.1.28:8088);\r\n", ControllerMaster1Name);
                enterFunction += string.Format("{0}.Field.Udp={1}.Field.MyControllerUdp;\r\n", ControllerSlave1With10RName, ControllerMaster1Name);
                enterFunction += string.Format("{0}.Field.Udp={1}.Field.MyControllerUdp;\r\n", ControllerSlave2With10RName, ControllerMaster1Name);
                enterFunction += string.Format("{0}.Field.Udp={1}.Field.MyControllerUdp;\r\n", ControllerSlave1With14AdName, ControllerMaster1Name);
                enterFunction += string.Format("{0}.Method.ChangeCanId(0x201);\r\n", ControllerSlave1With10RName);
                enterFunction += string.Format("{0}.Method.ChangeCanId(0x202);\r\n", ControllerSlave2With10RName);
                enterFunction += string.Format("{0}.Method.ChangeCanId(0x101);\r\n", ControllerSlave1With14AdName);
                enterFunction += string.Format("{0}.Method.ConnectPower({1});\r\n", PowerIt6302Name,
                    lblCmbPowerPort.comboBox.Text);
                enterFunction += string.Format("{0}.Method.ConnectBarcodeScanner({1});\r\n", BarcodeScanerName,
                    lblCmbBarcodeScanerPort.comboBox.Text);
                enterFunction += string.Format("{0}.Method.InitConfigFile({1});\r\n", VisionCheckAppName,
                    lblTxtProductName.textBox.Text);

                idle.EnterFunction = idle.EnterNote = enterFunction.TrimEnd();
            }

            var preStart = tempStateUnit.Find(f => f.Name == StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper());
            if (preStart != null)
            {
                var enterFunction = string.Empty;

                enterFunction += string.Format("{0}.Method.SetCombOff();\r\n", PowerIt6302Name);
                enterFunction += string.Format("{0}.Method.CloseAllChannels();\r\n", PowerIt6302Name);
                enterFunction += string.Format("{0}.Method.SetVoltageAll(0);\r\n", PowerIt6302Name);
                enterFunction += string.Format("{0}.Method.SetCurrentAll(0);\r\n", PowerIt6302Name);
                for (var i = 1; i < 11; i++)
                {
                    enterFunction += string.Format("{0}.Field.Relay{1}=0;\r\n", ControllerSlave1With10RName, i);
                    enterFunction += string.Format("{0}.Field.Relay{1}=0;\r\n", ControllerSlave2With10RName, i);
                }
                enterFunction += string.Format("{0}.Method.Sleep(1000);\r\n", CheckAppName);

                preStart.EnterFunction = preStart.EnterNote = enterFunction;
                preStart.DuringFunction =
                                preStart.DuringNote = string.Format("{0}.Method.GetMasterDi();\r\n", ControllerMaster1Name);
            }

            var checkEnd =
                tempStateUnit.Find(f => f.Name == StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString().ToUpper());
            if (checkEnd != null)
            {
                var enterFunction = string.Empty;

                var stampRelay = lblCmbIsStamp.comboBox.Text;

                if (stampRelay != "否")
                {
                    stampRelay = stampRelay.Replace("继电器", string.Empty);

                    if (int.Parse(stampRelay) >= 0 && int.Parse(stampRelay) <= 10)
                        enterFunction += string.Format("{0}.Field.Relay{1}=1;\r\n", ControllerSlave1With10RName,
                                int.Parse(stampRelay));
                    else
                        enterFunction += string.Format("{0}.Field.Relay{1}=1;\r\n", ControllerSlave2With10RName,
                                int.Parse(stampRelay) - 10);

                    enterFunction += string.Format("{0}.Method.Sleep(1500);\r\n", CheckAppName);

                    if (int.Parse(stampRelay) >= 0 && int.Parse(stampRelay) <= 10)
                        enterFunction += string.Format("{0}.Field.Relay{1}=0;\r\n", ControllerSlave1With10RName,
                                int.Parse(stampRelay));
                    else
                        enterFunction += string.Format("{0}.Field.Relay{1}=0;\r\n", ControllerSlave2With10RName,
                                int.Parse(stampRelay) - 10);

                    enterFunction += string.Format("{0}.Method.Sleep(500);\r\n", CheckAppName);

                    checkEnd.EnterFunction = checkEnd.EnterNote = enterFunction.Trim();
                }
            }

            if (true)
            {
                if (_barcodeStructs.Any())
                {
                    tempStateUnit.AddRange(_barcodeStructs.Select((t, i) => new DeviceConfigStatusUnit
                    {
                        WorkStationName = deviceConfig.WorkStations[0].Name,
                        Name = string.Format("{0}-扫码", t.Name),
                        PositionSize =
                            _stateUnitPosition.ContainsKey(string.Format("扫码{0}", i + 1))
                                ? _stateUnitPosition[string.Format("扫码{0}", i + 1)]
                                : _stateUnitPosition["扫码15"],
                        EnterFunction = string.Format("{0}.Method.ReadBarcode(1);\r\n", BarcodeScanerName),
                        EnterNote = string.Format("{0}.Method.ReadBarcode(1);\r\n", BarcodeScanerName),
                    }));
                }

                for (var i = 0; i < dgvCheckItems.dataGridView.RowCount; i++)
                {
                    var enterFunction = string.Empty;

                    var row = dgvCheckItems.dataGridView.Rows[i];

                    if (row.Cells[0].Value.ToString() == @"否")
                    {
                        enterFunction += string.Format("{0}.Method.SetCombOff();\r\n", PowerIt6302Name);
                        enterFunction += string.Format("{0}.Method.SetVoltage1({1});\r\n", PowerIt6302Name, row.Cells[1].Value);
                        enterFunction += string.Format("{0}.Method.SetVoltage2({1});\r\n", PowerIt6302Name, row.Cells[2].Value);
                        enterFunction += string.Format("{0}.Method.SetVoltage3({1});\r\n", PowerIt6302Name, row.Cells[3].Value);
                        enterFunction += string.Format("{0}.Method.SetCurrent1({1});\r\n", PowerIt6302Name, row.Cells[4].Value);
                        enterFunction += string.Format("{0}.Method.SetCurrent1({1});\r\n", PowerIt6302Name, row.Cells[5].Value);
                        enterFunction += string.Format("{0}.Method.SetCurrent1({1});\r\n", PowerIt6302Name, row.Cells[6].Value);
                        enterFunction += string.Format("{0}.Method.OpenAllChannels();\r\n", PowerIt6302Name);
                    }
                    else if (row.Cells[0].Value.ToString() == @"串联")
                    {
                        enterFunction += string.Format("{0}.Method.SetCombSerOn();\r\n", PowerIt6302Name);
                        enterFunction += string.Format("{0}.Method.SetVoltage1({1});\r\n", PowerIt6302Name, row.Cells[1].Value);
                        enterFunction += string.Format("{0}.Method.SetVoltage2({1});\r\n", PowerIt6302Name, row.Cells[2].Value);
                        enterFunction += string.Format("{0}.Method.SetVoltage3({1});\r\n", PowerIt6302Name, row.Cells[3].Value);
                        enterFunction += string.Format("{0}.Method.SetCurrent1({1});\r\n", PowerIt6302Name, row.Cells[4].Value);
                        enterFunction += string.Format("{0}.Method.SetCurrent1({1});\r\n", PowerIt6302Name, row.Cells[5].Value);
                        enterFunction += string.Format("{0}.Method.SetCurrent1({1});\r\n", PowerIt6302Name, row.Cells[6].Value);
                        enterFunction += string.Format("{0}.Method.OpenAllChannels();\r\n", PowerIt6302Name);
                    }
                    else if (row.Cells[0].Value.ToString() == @"并联")
                    {
                        enterFunction += string.Format("{0}.Method.SetCombParaOn();\r\n", PowerIt6302Name);
                        enterFunction += string.Format("{0}.Method.SetVoltage1({1});\r\n", PowerIt6302Name, row.Cells[1].Value);
                        enterFunction += string.Format("{0}.Method.SetVoltage2({1});\r\n", PowerIt6302Name, row.Cells[2].Value);
                        enterFunction += string.Format("{0}.Method.SetVoltage3({1});\r\n", PowerIt6302Name, row.Cells[3].Value);
                        enterFunction += string.Format("{0}.Method.SetCurrent1({1});\r\n", PowerIt6302Name, row.Cells[4].Value);
                        enterFunction += string.Format("{0}.Method.SetCurrent1({1});\r\n", PowerIt6302Name, row.Cells[5].Value);
                        enterFunction += string.Format("{0}.Method.SetCurrent1({1});\r\n", PowerIt6302Name, row.Cells[6].Value);
                        enterFunction += string.Format("{0}.Method.OpenAllChannels();\r\n", PowerIt6302Name);
                    }

                    enterFunction += row.Cells[7].Value.ToString();

                    if (row.Cells[18].Value.ToString() != @"否")
                        enterFunction += string.Format("{0}.Method.VisionAnalysis({1});\r\n", VisionCheckAppName,
                               row.Cells[20].Value);

                    enterFunction += string.Format("{0}.Method.UpdateCurrentsAndVoltages();\r\n", PowerIt6302Name);
                    enterFunction += string.Format("{0}.Method.GetSlaveAds();\r\n", ControllerSlave1With14AdName);

                    enterFunction =
                        row.Cells[7].Value.ToString()
                            .StrTrim()
                            .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                            .Where(t => !t.Contains("Sleep"))
                            .Aggregate(enterFunction, (current, t) => current + (t.Replace("=1", "=0") + ";\r\n"));

                    enterFunction += string.Format("{0}.Method.Sleep(500);\r\n", CheckAppName);

                    tempStateUnit.Add(new DeviceConfigStatusUnit
                    {
                        WorkStationName = deviceConfig.WorkStations[0].Name,
                        Name = string.Format("检测{0}", i + 1),
                        PositionSize =
                            _stateUnitPosition.ContainsKey(string.Format("检测{0}", i + 1))
                                ? _stateUnitPosition[string.Format("检测{0}", i + 1)]
                                : _stateUnitPosition["检测15"],
                        EnterFunction = enterFunction,
                        EnterNote = enterFunction
                    });
                }
            }
            #endregion

            #region 条件

            var tempConditions = new List<DeviceConfigCondition>();

            tempConditions.Add(new DeviceConfigCondition
            {
                WorkStationName = deviceConfig.WorkStations[0].Name,
                Name =
                    string.Format("{0}_{1}", StateMachineHelper.EDefaultStateUnits.Idle.ToString().ToUpper(),
                        StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper()),
                SourceSuName = StateMachineHelper.EDefaultStateUnits.Idle.ToString().ToUpper(),
                TargetSuName = StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                ConditionFunction = string.Format("{0}.Field.{1}==1&&\r\n", CheckAppName, "EqualTrue"),
                ConditionNote = string.Format("{0}.Field.{1}==1&&\r\n", CheckAppName, "EqualTrue"),
                MiddlePisiton = GetMiddlePosition(
                    GetSuRectSrt(StateMachineHelper.EDefaultStateUnits.Idle.ToString().ToUpper(), tempStateUnit),
                    GetSuRectSrt(StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(), tempStateUnit))
            });

            tempConditions.Add(new DeviceConfigCondition
            {
                WorkStationName = deviceConfig.WorkStations[0].Name,
                Name =
                    string.Format("{0}_{1}", StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString().ToUpper(),
                        StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper()),
                SourceSuName = StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString().ToUpper(),
                TargetSuName = StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                ConditionFunction = string.Format("{0}.Field.{1}==1&&\r\n", CheckAppName, "EqualTrue"),
                ConditionNote = string.Format("{0}.Field.{1}==1&&\r\n", CheckAppName, "EqualTrue"),
                MiddlePisiton = GetMiddlePosition(
                    GetSuRectSrt(StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString().ToUpper(), tempStateUnit),
                    GetSuRectSrt(StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(), tempStateUnit))
            });

            tempConditions.Add(new DeviceConfigCondition
            {
                WorkStationName = deviceConfig.WorkStations[0].Name,
                Name =
                    string.Format("{0}_{1}", StateMachineHelper.EDefaultStateUnits.CheckFail.ToString().ToUpper(),
                        StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper()),
                SourceSuName = StateMachineHelper.EDefaultStateUnits.CheckFail.ToString().ToUpper(),
                TargetSuName = StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                ConditionFunction = string.Format("{0}.Field.{1}==1&&\r\n", CheckAppName, "EqualTrue"),
                ConditionNote = string.Format("{0}.Field.{1}==1&&\r\n", CheckAppName, "EqualTrue"),
                MiddlePisiton = GetMiddlePosition(
                    GetSuRectSrt(StateMachineHelper.EDefaultStateUnits.CheckFail.ToString().ToUpper(), tempStateUnit),
                    GetSuRectSrt(StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(), tempStateUnit))
            });

            for (var i = 0; i < _barcodeStructs.Count; i++)
            {
                var barcodeSuNameSrc = string.Format("{0}-扫码", _barcodeStructs[i].Name);
                var barcodeParaName = _barcodeStructs[i].Name;

                if (i == 0) // 第一个
                {
                    tempConditions.Add(new DeviceConfigCondition
                    {
                        WorkStationName = deviceConfig.WorkStations[0].Name,
                        Name =
                            string.Format("{0}_{1}", StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                                barcodeSuNameSrc),
                        SourceSuName = StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                        TargetSuName = barcodeSuNameSrc,
                        ConditionFunction = string.Format("{0}.Field.{1}==0&&\r\n{2}.Field.{3}==0&&\r\n", ControllerMaster1Name, "Di1", CheckAppName, "IsByPass"),
                        ConditionNote = string.Format("{0}.Field.{1}==0&&\r\n{2}.Field.{3}==0&&\r\n", ControllerMaster1Name, "Di1", CheckAppName, "IsByPass"),
                        MiddlePisiton = GetMiddlePosition(
                            GetSuRectSrt(StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                                tempStateUnit), GetSuRectSrt(barcodeSuNameSrc, tempStateUnit))
                    });
                }

                if (i == _barcodeStructs.Count - 1) // 最后一个
                {
                    if (lblCmbIsReadButton.comboBox.Text != @"否") // 需要按键
                    {
                        tempConditions.Add(new DeviceConfigCondition
                        {
                            WorkStationName = deviceConfig.WorkStations[0].Name,
                            Name =
                                string.Format("{0}_{1}", barcodeSuNameSrc, readDiUnit.Name),
                            SourceSuName = barcodeSuNameSrc,
                            TargetSuName = readDiUnit.Name,
                            ConditionFunction = string.Format("{0}.Para.{1}==True&&\r\n", processNo, barcodeParaName),
                            ConditionNote = string.Format("{0}.Para.{1}==True&&\r\n", processNo, barcodeParaName),
                            MiddlePisiton = GetMiddlePosition(
                                GetSuRectSrt(barcodeSuNameSrc, tempStateUnit),
                                GetSuRectSrt(readDiUnit.Name, tempStateUnit))
                        });
                    }
                    else // 不需要按键
                    {
                        if (paraList.Count > 0)
                        {
                            tempConditions.Add(new DeviceConfigCondition
                            {
                                WorkStationName = deviceConfig.WorkStations[0].Name,
                                Name =
                                    string.Format("{0}_{1}", barcodeSuNameSrc, paraList.Keys.ToList()[0]),
                                SourceSuName = barcodeSuNameSrc,
                                TargetSuName = paraList.Keys.ToList()[0],
                                ConditionFunction = string.Format("{0}.Para.{1}==True&&\r\n", processNo, barcodeParaName),
                                ConditionNote = string.Format("{0}.Para.{1}==True&&\r\n", processNo, barcodeParaName),
                                MiddlePisiton = GetMiddlePosition(
                                    GetSuRectSrt(barcodeSuNameSrc, tempStateUnit),
                                    GetSuRectSrt(paraList.Keys.ToList()[0], tempStateUnit))
                            });
                        }
                    }
                }

                if (i >= 0 && i < _barcodeStructs.Count - 1) // 中间几个
                {
                    var barcodeSuNameTarget = string.Format("{0}-扫码", _barcodeStructs[i + 1].Name);

                    tempConditions.Add(new DeviceConfigCondition
                    {
                        WorkStationName = deviceConfig.WorkStations[0].Name,
                        Name =
                            string.Format("{0}_{1}", barcodeSuNameSrc, barcodeSuNameTarget),
                        SourceSuName = barcodeSuNameSrc,
                        TargetSuName = barcodeSuNameTarget,
                        ConditionFunction = string.Format("{0}.Para.{1}==True&&\r\n", processNo, barcodeParaName),
                        ConditionNote = string.Format("{0}.Para.{1}==True&&\r\n", processNo, barcodeParaName),
                        MiddlePisiton = GetMiddlePosition(
                            GetSuRectSrt(barcodeSuNameSrc, tempStateUnit),
                            GetSuRectSrt(barcodeSuNameTarget, tempStateUnit))
                    });
                }
            }

            for (var i = 0; i < paraList.Count; i++)
            {
                if (i == 0)
                {
                    if (!_barcodeStructs.Any()) // 不用扫码
                    {
                        tempConditions.Add(new DeviceConfigCondition
                        {
                            WorkStationName = deviceConfig.WorkStations[0].Name,
                            Name =
                                string.Format("{0}_{1}",
                                    StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                                    paraList.Keys.ToList()[0]),
                            SourceSuName = StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                            TargetSuName = paraList.Keys.ToList()[0],
                            ConditionFunction = string.Format("{0}.Field.{1}==0&&\r\n", ControllerMaster1Name, "Di1"),
                            ConditionNote = string.Format("{0}.Field.{1}==0&&\r\n", ControllerMaster1Name, "Di1"),
                            MiddlePisiton = GetMiddlePosition(
                                GetSuRectSrt(StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                                    tempStateUnit), GetSuRectSrt(paraList.Keys.ToList()[0], tempStateUnit))
                        });
                    }
                    else // 需要扫码
                    {
                        tempConditions.Add(new DeviceConfigCondition
                        {
                            WorkStationName = deviceConfig.WorkStations[0].Name,
                            Name =
                                string.Format("{0}_{1}",
                                    readDiUnit.Name,
                                    paraList.Keys.ToList()[0]),
                            SourceSuName = readDiUnit.Name,
                            TargetSuName = paraList.Keys.ToList()[0],
                            ConditionFunction = string.Format("{0}.Field.{1}==0&&\r\n", ControllerMaster1Name, "Di1"),
                            ConditionNote = string.Format("{0}.Field.{1}==0&&\r\n", ControllerMaster1Name, "Di1"),
                            MiddlePisiton = GetMiddlePosition(
                                GetSuRectSrt(StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                                    tempStateUnit), GetSuRectSrt(paraList.Keys.ToList()[0], tempStateUnit))
                        });

                        tempConditions.Add(new DeviceConfigCondition
                        {
                            WorkStationName = deviceConfig.WorkStations[0].Name,
                            Name =
                                string.Format("{0}_{1}",
                                    StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                                    paraList.Keys.ToList()[0]),
                            SourceSuName = StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                            TargetSuName = paraList.Keys.ToList()[0],
                            ConditionFunction = string.Format("{0}.Field.{1}==0&&\r\n{2}.Field.{3}==1&&\r\n", ControllerMaster1Name, "Di1", CheckAppName, "IsByPass"),
                            ConditionNote = string.Format("{0}.Field.{1}==0&&\r\n{2}.Field.{3}==1&&\r\n", ControllerMaster1Name, "Di1", CheckAppName, "IsByPass"),
                            MiddlePisiton = GetMiddlePosition(
                                GetSuRectSrt(StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper(),
                                    tempStateUnit), GetSuRectSrt(paraList.Keys.ToList()[0], tempStateUnit))
                        });
                    }
                }

                if (i == paraList.Count - 1)
                {
                    var conditionFuc =
                        paraList[string.Format("检测{0}", i + 1)].Aggregate(string.Empty,
                            (current, t) => current + string.Format("{0}.Para.{1}==True&&\r\n", processNo, t));

                    tempConditions.Add(new DeviceConfigCondition
                    {
                        WorkStationName = deviceConfig.WorkStations[0].Name,
                        Name =
                            string.Format("{0}_{1}",
                                paraList.Keys.ToList()[i],
                                StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString().ToUpper()),
                        SourceSuName = paraList.Keys.ToList()[i],
                        TargetSuName = StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString().ToUpper(),
                        ConditionFunction = conditionFuc,
                        ConditionNote = conditionFuc,
                        MiddlePisiton = GetMiddlePosition(
                            GetSuRectSrt(paraList.Keys.ToList()[i],
                                tempStateUnit),
                            GetSuRectSrt(StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString().ToUpper(),
                                tempStateUnit))
                    });
                }

                if (i >= 0 && i < paraList.Count - 1)
                {
                    var sourceSu = string.Format("检测{0}", i + 1);
                    var targetSu = string.Format("检测{0}", i + 2);

                    var conditionFuc =
                        paraList[string.Format("检测{0}", i + 1)].Aggregate(string.Empty,
                            (current, t) => current + string.Format("{0}.Para.{1}==True&&\r\n", processNo, t));

                    tempConditions.Add(new DeviceConfigCondition
                    {
                        WorkStationName = deviceConfig.WorkStations[0].Name,
                        Name =
                            string.Format("{0}_{1}", sourceSu, targetSu),
                        SourceSuName = sourceSu,
                        TargetSuName = targetSu,
                        ConditionFunction = conditionFuc,
                        ConditionNote = conditionFuc,
                        MiddlePisiton = GetMiddlePosition(
                            GetSuRectSrt(sourceSu, tempStateUnit),
                            GetSuRectSrt(targetSu, tempStateUnit))
                    });
                }
            }

            #endregion

            deviceConfig.StatusUnits = tempStateUnit.ToArray();
            deviceConfig.Conditions = tempConditions.ToArray();

            var folder = string.Format(@"{0}\流程配置文件", Program.SysDir);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var filePath =
                string.Format(@"{0}\流程配置文件\{1}_{2}.xml", Program.SysDir, productName, DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace(":", string.Empty).Replace("/", string.Empty).Replace(" ", string.Empty));

            SaveDeviceConfigToFile(deviceConfig, filePath, Encoding.UTF8);
        }

        #region 方法
        public static void SaveDeviceConfigToFile(object config, string filepath, Encoding encoding)
        {
            SerializeToFile(config, filepath, Encoding.UTF8);
        }

        private static void SerializeToFile(object o, string path, Encoding encoding)
        {
            //if (string.IsNullOrEmpty(path))
            //    throw new ArgumentNullException(nameof(path));

            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
                XmlSerializeInternal(file, o, encoding);
        }

        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            //if (o == null)
            //    throw new ArgumentNullException(nameof(o));

            //if (encoding == null)
            //    throw new ArgumentNullException(nameof(encoding));

            var serializer = new XmlSerializer(o.GetType());

            var settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineChars = "\r\n",
                Encoding = encoding,
                IndentChars = "    "
            };

            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }

        private static string GetMiddlePosition(string sourceRect, string targetRect)
        {
            var suSourceRect = GetRectOfSu(sourceRect);
            var pStart = GetRectCenter(suSourceRect);
            var suTargetRect = GetRectOfSu(targetRect);
            var suTargetRectCenter = GetRectCenter(suTargetRect);
            var pEnd = GetCrossPoint(suTargetRect, pStart, suTargetRectCenter);

            return (pEnd.X - pStart.X) / 2 + pStart.X + "," +
                   ((pEnd.Y - pStart.Y) / 2 + pStart.Y);
        }

        private static Rectangle GetRectOfSu(
           string rectStr)
        {
            var rect = new Rectangle(50, 50, 180, 90);

            var position = rectStr;

            if (string.IsNullOrEmpty(position) || !position.Contains(",") || position.Split(',').Length != 4)
            {
                position = "50,50,180,90";
            }
            var lstNum = position.Split(',');
            rect.X = int.Parse(lstNum[0]);
            rect.Y = int.Parse(lstNum[1]);
            rect.Width = int.Parse(lstNum[2]);
            rect.Height = int.Parse(lstNum[3]);

            return rect;
        }

        private static Point GetRectCenter(
           Rectangle rect)
        {
            var p = new Point
            {
                X = rect.Left + rect.Width / 2,
                Y = rect.Top + rect.Height / 2
            };
            return p;
        }

        private static Point GetCrossPoint(
           Rectangle rect, Point pStart, Point pEnd)
        {
            var pCross = new Point();

            if (pEnd.X == pStart.X)
            {
                if (pStart.Y > rect.Bottom)
                {
                    pCross.X = pEnd.X;
                    pCross.Y = rect.Bottom;
                }
                else if (pStart.Y < rect.Top)
                {
                    pCross.X = pEnd.X;
                    pCross.Y = rect.Top;
                }
                return pCross;
            }

            if (pEnd.Y == pStart.Y)
            {
                if (pStart.X > rect.Right)
                {
                    pCross.X = rect.Right;
                    pCross.Y = pStart.Y;
                }
                else if (pStart.X < rect.Left)
                {
                    pCross.X = rect.Left;
                    pCross.Y = pStart.Y;
                }
                return pCross;
            }

            var k = (double)(pEnd.Y - pStart.Y) / (pEnd.X - pStart.X);

            var lstPoints = new List<Point>();

            var pRight = new Point { X = rect.Right - 1 };
            pRight.Y = (int)((pRight.X - pStart.X) * k) + pStart.Y;

            var pLeft = new Point { X = rect.Left + 1 };
            pLeft.Y = (int)((pLeft.X - pStart.X) * k) + pStart.Y;

            var pTop = new Point { Y = rect.Top + 1 };
            pTop.X = (int)((pTop.Y - pStart.Y) / k) + pStart.X;

            var pBottom = new Point { Y = rect.Bottom - 1 };
            pBottom.X = (int)((pBottom.Y - pStart.Y) / k) + pStart.X;

            if (rect.Contains(pRight)) lstPoints.Add(pRight);
            if (rect.Contains(pLeft)) lstPoints.Add(pLeft);
            if (rect.Contains(pTop)) lstPoints.Add(pTop);
            if (rect.Contains(pBottom)) lstPoints.Add(pBottom);

            var distance = int.MaxValue;

            foreach (var p in lstPoints)
            {
                var dis = (p.Y - pStart.Y) * (p.Y - pStart.Y) + (p.X - pStart.X) * (p.X - pStart.X);
                if (dis >= distance)
                    continue;
                pCross = p;
                distance = dis;
            }

            return pCross;
        }

        private static string GetSuRectSrt(
            string suName, List<DeviceConfigStatusUnit> suList)
        {
            var find = suList.Find(f => f.Name == suName);
            return find != null ? find.PositionSize : string.Empty;
        }

        #endregion
    }
}

