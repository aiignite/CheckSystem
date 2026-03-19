using CheckSystem.VisionDetection.Vision;
using Controller;
using Sunny.UI;
using System;
using System.Text;
using CommonUtility.FileOperator;

namespace CheckSystem.VisionDetection
{
    public partial class FrmDeviceConfig : UIForm
    {
        public FrmDeviceConfig()
        {
            InitializeComponent();

            cmbPowerType.DropDownStyle = UIDropDownStyle.DropDownList;
            cmbPowerType.Items.Add(typeof(PowerIt6302).Name);
            cmbPowerType.Items.Add(typeof(PowerNgi3412E).Name);
            cmbPowerType.SelectedIndex = 0;

            for (var i = 1; i < 255; i++)
            {
                cmbPowerAddress.Items.Add(string.Format("192.168.1.{0}:?", i));
                cmbBarcodeScanAddress.Items.Add(string.Format("192.168.1.{0}:?", i));
            }

            for (var i = 1; i < 30; i++)
            {
                cmbPowerAddress.Items.Add(string.Format("COM{0}:?", i));
                cmbPowerAddress.Items.Add(string.Format("COM{0}:?", i));
            }

            cmbPowerAddress.SelectedIndex = 0;
            cmbBarcodeScanAddress.SelectedIndex = 0;

            cmbControllerType.DropDownStyle = UIDropDownStyle.DropDownList;
            cmbControllerType.Items.Add("56PIN版本");
            cmbControllerType.Items.Add("绿板子版本");
            cmbControllerType.Items.Add("蓝板子版本");
            cmbControllerType.Items.Add("瑞萨芯片版本");
            cmbControllerType.SelectedIndex = 0;

            cmbPowerType.Text = VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.Power[0].Type;
            cmbPowerAddress.Text = VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.Power[0].Address;

            cmbBarcodeScanAddress.Text = VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.BarcodeScaner[0].Address;

            txtDeviceInNo.Text = VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ShowAskDialog("是否确认保存？"))
            {
                VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.Power[0].Type = cmbPowerType.Text;
                VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.Power[0].Address = cmbPowerAddress.Text;

                VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.BarcodeScaner[0].Address =
                    cmbBarcodeScanAddress.Text;

                if (string.IsNullOrEmpty(txtDeviceInNo.Text) || !txtDeviceInNo.Text.StartsWith("IN"))
                {
                    ShowErrorTip("请填写正确的IN号");
                    return;
                }

                VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo = txtDeviceInNo.Text;

                if (cmbControllerType.SelectedIndex == 0)
                {
                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController =
                        new VisionDeviceConfigDeviceInfoControllersIoController[3];

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[0] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "控制器1IP28",
                        Type = typeof(SyControllerWith56Pin).Name,
                        Address = "192.168.1.28:8088"
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[1] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "控制器2IP29",
                        Type = typeof(SyControllerWith56Pin).Name,
                        Address = "192.168.1.29:8088"
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[2] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "控制器3IP30",
                        Type = typeof(SyControllerWith56Pin).Name,
                        Address = "192.168.1.30:8088"
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings = new VisionDeviceConfigDeviceInfoBidings
                    {
                        Volt = new VisionDeviceConfigDeviceInfoBidingsVolt[3 * 6],
                        Curr = new VisionDeviceConfigDeviceInfoBidingsCurr[3 * 4],
                        Relay = new VisionDeviceConfigDeviceInfoBidingsRelay[3 * 12],
                        StartDi = new VisionDeviceConfigDeviceInfoBidingsStartDi
                        {
                            Controller = "控制器1IP28",
                            Field = "Di1"
                        }
                    };

                    for (var j = 0; j < 6; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Volt[j] =
                            new VisionDeviceConfigDeviceInfoBidingsVolt
                            {
                                Controller = "控制器1IP28",
                                Field = string.Format("Voltage{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 6; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Volt[j + 6] =
                            new VisionDeviceConfigDeviceInfoBidingsVolt
                            {
                                Controller = "控制器2IP29",
                                Field = string.Format("Voltage{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 6; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Volt[j + 12] =
                            new VisionDeviceConfigDeviceInfoBidingsVolt
                            {
                                Controller = "控制器3IP30",
                                Field = string.Format("Voltage{0}", j + 1)
                            };
                    }

                    for (var j = 0; j < 4; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Curr[j] =
                            new VisionDeviceConfigDeviceInfoBidingsCurr
                            {
                                Controller = "控制器1IP28",
                                Field = string.Format("Current{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 4; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Curr[j + 4] =
                            new VisionDeviceConfigDeviceInfoBidingsCurr
                            {
                                Controller = "控制器2IP29",
                                Field = string.Format("Current{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 4; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Curr[j + 8] =
                            new VisionDeviceConfigDeviceInfoBidingsCurr
                            {
                                Controller = "控制器3IP30",
                                Field = string.Format("Current{0}", j + 1)
                            };
                    }

                    for (var j = 0; j < 12; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Relay[j] =
                            new VisionDeviceConfigDeviceInfoBidingsRelay
                            {
                                Controller = "控制器1IP28",
                                Field = string.Format("Relay{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 12; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Relay[j + 12] =
                            new VisionDeviceConfigDeviceInfoBidingsRelay
                            {
                                Controller = "控制器2IP29",
                                Field = string.Format("Relay{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 12; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Relay[j + 24] =
                            new VisionDeviceConfigDeviceInfoBidingsRelay
                            {
                                Controller = "控制器3IP30",
                                Field = string.Format("Relay{0}", j + 1)
                            };
                    }

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Actions =
                        new VisionDeviceConfigDeviceInfoActions
                        {
                            Start =
                                new VisionDeviceConfigDeviceInfoActionsStart
                                {
                                    Controller = "控制器1IP28",
                                    Field = "Di1"
                                },
                            Bang = new VisionDeviceConfigDeviceInfoActionsBang
                            {
                                Controller = "控制器1IP28",
                                Field = "Relay11"
                            }
                        };
                }
                else if (cmbControllerType.SelectedIndex == 1)
                {
                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController =
                        new VisionDeviceConfigDeviceInfoControllersIoController[4];

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[0] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "控制器1IP28",
                        Address = "192.168.1.28:8088",
                        Type = typeof(SyControllerMaster).Name,
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[1] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "继电器从站_0x201",
                        Address = "控制器1IP28:0x201",
                        Type = typeof(SyControllerSlaveWith10R).Name,
                    };
                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[2] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "继电器从站_0x202",
                        Address = "控制器1IP28:0x202",
                        Type = typeof(SyControllerSlaveWith10R).Name,
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[3] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "AD从站_0x101",
                        Address = "控制器1IP28:0x101",
                        Type = typeof(SyControllerSlaveWith14Ad).Name,
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings = new VisionDeviceConfigDeviceInfoBidings
                    {
                        Volt = new VisionDeviceConfigDeviceInfoBidingsVolt[1 * 8],
                        Curr = new VisionDeviceConfigDeviceInfoBidingsCurr[1 * 6],
                        Relay = new VisionDeviceConfigDeviceInfoBidingsRelay[2 * 10],
                        StartDi = new VisionDeviceConfigDeviceInfoBidingsStartDi
                        {
                            Controller = "控制器1IP28",
                            Field = "Di1"
                        }
                    };

                    for (var j = 0; j < 8; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Volt[j] =
                            new VisionDeviceConfigDeviceInfoBidingsVolt
                            {
                                Controller = "AD从站_0x101",
                                Field = string.Format("AdVoltage{0}", j + 1)
                            };
                    }

                    for (var j = 0; j < 6; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Curr[j] =
                            new VisionDeviceConfigDeviceInfoBidingsCurr
                            {
                                Controller = "AD从站_0x101",
                                Field = string.Format("AdCurrent{0}", j + 1)
                            };
                    }

                    for (var j = 0; j < 10; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Relay[j] =
                            new VisionDeviceConfigDeviceInfoBidingsRelay
                            {
                                Controller = "继电器从站_0x201",
                                Field = string.Format("Relay{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 10; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Relay[j + 10] =
                            new VisionDeviceConfigDeviceInfoBidingsRelay
                            {
                                Controller = "继电器从站_0x202",
                                Field = string.Format("Relay{0}", j + 1)
                            };
                    }

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Actions =
                        new VisionDeviceConfigDeviceInfoActions
                        {
                            Start =
                                new VisionDeviceConfigDeviceInfoActionsStart
                                {
                                    Controller = "控制器1IP28",
                                    Field = "Input1"
                                },
                            Bang = new VisionDeviceConfigDeviceInfoActionsBang
                            {
                                Controller = "继电器从站_0x202",
                                Field = "Relay8"
                            }
                        };
                }
                else if (cmbControllerType.SelectedIndex == 2)
                {
                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController =
                        new VisionDeviceConfigDeviceInfoControllersIoController[3];

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[0] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "控制器1IP28",
                        Type = typeof(ControllerWithGateway).Name,
                        Address = "192.168.1.28:8088"
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[1] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "控制器2IP29",
                        Type = typeof(ControllerWithGateway).Name,
                        Address = "192.168.1.29:8088"
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[2] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "控制器3IP30",
                        Type = typeof(ControllerWithGateway).Name,
                        Address = "192.168.1.30:8088"
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings = new VisionDeviceConfigDeviceInfoBidings
                    {
                        Volt = new VisionDeviceConfigDeviceInfoBidingsVolt[3 * 5],
                        Curr = new VisionDeviceConfigDeviceInfoBidingsCurr[3 * 4],
                        Relay = new VisionDeviceConfigDeviceInfoBidingsRelay[3 * 9],
                        StartDi = new VisionDeviceConfigDeviceInfoBidingsStartDi
                        {
                            Controller = "控制器1IP28",
                            Field = "Input1"
                        }
                    };

                    for (var j = 0; j < 5; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Volt[j] =
                            new VisionDeviceConfigDeviceInfoBidingsVolt
                            {
                                Controller = "控制器1IP28",
                                Field = string.Format("Voltage{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 5; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Volt[j + 5] =
                            new VisionDeviceConfigDeviceInfoBidingsVolt
                            {
                                Controller = "控制器2IP29",
                                Field = string.Format("Voltage{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 5; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Volt[j + 10] =
                            new VisionDeviceConfigDeviceInfoBidingsVolt
                            {
                                Controller = "控制器3IP30",
                                Field = string.Format("Voltage{0}", j + 1)
                            };
                    }

                    for (var j = 0; j < 4; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Curr[j] =
                            new VisionDeviceConfigDeviceInfoBidingsCurr
                            {
                                Controller = "控制器1IP28",
                                Field = string.Format("Current{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 4; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Curr[j + 4] =
                            new VisionDeviceConfigDeviceInfoBidingsCurr
                            {
                                Controller = "控制器2IP29",
                                Field = string.Format("Current{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 4; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Curr[j + 8] =
                            new VisionDeviceConfigDeviceInfoBidingsCurr
                            {
                                Controller = "控制器3IP30",
                                Field = string.Format("Current{0}", j + 1)
                            };
                    }

                    for (var j = 0; j < 9; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Relay[j] =
                            new VisionDeviceConfigDeviceInfoBidingsRelay
                            {
                                Controller = "控制器1IP28",
                                Field = string.Format("Relay{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 9; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Relay[j + 9] =
                            new VisionDeviceConfigDeviceInfoBidingsRelay
                            {
                                Controller = "控制器2IP29",
                                Field = string.Format("Relay{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 9; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Relay[j + 18] =
                            new VisionDeviceConfigDeviceInfoBidingsRelay
                            {
                                Controller = "控制器3IP30",
                                Field = string.Format("Relay{0}", j + 1)
                            };
                    }

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Actions =
                        new VisionDeviceConfigDeviceInfoActions
                        {
                            Start =
                                new VisionDeviceConfigDeviceInfoActionsStart
                                {
                                    Controller = "控制器1IP28",
                                    Field = "Input1"
                                },
                            Bang = new VisionDeviceConfigDeviceInfoActionsBang
                            {
                                Controller = "控制器2IP29",
                                Field = "Relay7"
                            }
                        };
                }
                else if (cmbControllerType.SelectedIndex == 3)
                {
                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController =
                        new VisionDeviceConfigDeviceInfoControllersIoController[9];

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[0] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "控制器1IP28",
                        Address = "192.168.1.28:8088",
                        Type = typeof(SyRenesasMcuControllerMaster).Name,
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[1] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "RL从站-201",
                        Address = "192.168.1.28:0x201",
                        Type = typeof(SyRenesasMcuControllerSlaveWith8RLs).Name,
                    };
                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[2] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "RL从站-202",
                        Address = "192.168.1.28:0x202",
                        Type = typeof(SyRenesasMcuControllerSlaveWith8RLs).Name,
                    };
                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[3] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "RL从站-203",
                        Address = "192.168.1.28:0x203",
                        Type = typeof(SyRenesasMcuControllerSlaveWith8RLs).Name,
                    };
                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[4] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "RL从站-204",
                        Address = "192.168.1.28:0x204",
                        Type = typeof(SyRenesasMcuControllerSlaveWith8RLs).Name,
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[5] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "AD从站-101",
                        Address = "192.168.1.28:0x101",
                        Type = typeof(SyRenesasMcuControllerSlaveWith12ADs).Name,
                    };
                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[6] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "AD从站-102",
                        Address = "192.168.1.28:0x102",
                        Type = typeof(SyRenesasMcuControllerSlaveWith12ADs).Name,
                    };
                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[7] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "AD从站-103",
                        Address = "192.168.1.28:0x103",
                        Type = typeof(SyRenesasMcuControllerSlaveWith12ADs).Name,
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Controllers.IoController[8] = new VisionDeviceConfigDeviceInfoControllersIoController
                    {
                        Name = "控制器2IP29",
                        Address = "192.168.1.29:8088",
                        Type = typeof(SyRenesasMcuControllerMaster).Name,
                    };

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings = new VisionDeviceConfigDeviceInfoBidings
                    {
                        Volt = new VisionDeviceConfigDeviceInfoBidingsVolt[3 * 6],
                        Curr = new VisionDeviceConfigDeviceInfoBidingsCurr[3 * 6],
                        Relay = new VisionDeviceConfigDeviceInfoBidingsRelay[4 * 8],
                        StartDi = new VisionDeviceConfigDeviceInfoBidingsStartDi
                        {
                            Controller = "控制器1IP28",
                            Field = "Di1"
                        }
                    };

                    for (var j = 0; j < 6; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Volt[j] =
                            new VisionDeviceConfigDeviceInfoBidingsVolt
                            {
                                Controller = "AD从站-101",
                                Field = string.Format("Voltage{0}", j + 2)
                            };
                    }
                    for (var j = 0; j < 6; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Volt[j + 6] =
                            new VisionDeviceConfigDeviceInfoBidingsVolt
                            {
                                Controller = "AD从站-102",
                                Field = string.Format("Voltage{0}", j + 2)
                            };
                    }
                    for (var j = 0; j < 6; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Volt[j + 12] =
                            new VisionDeviceConfigDeviceInfoBidingsVolt
                            {
                                Controller = "AD从站-103",
                                Field = string.Format("Voltage{0}", j + 2)
                            };
                    }

                    for (var j = 0; j < 6; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Curr[j] =
                            new VisionDeviceConfigDeviceInfoBidingsCurr
                            {
                                Controller = "AD从站-101",
                                Field = string.Format("Current{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 6; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Curr[j + 6] =
                            new VisionDeviceConfigDeviceInfoBidingsCurr
                            {
                                Controller = "AD从站-102",
                                Field = string.Format("Current{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 6; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Curr[j + 12] =
                            new VisionDeviceConfigDeviceInfoBidingsCurr
                            {
                                Controller = "AD从站-103",
                                Field = string.Format("Current{0}", j + 1)
                            };
                    }

                    for (var j = 0; j < 8; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Relay[j] =
                            new VisionDeviceConfigDeviceInfoBidingsRelay
                            {
                                Controller = "RL从站-201",
                                Field = string.Format("Relay{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 8; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Relay[j + 8] =
                            new VisionDeviceConfigDeviceInfoBidingsRelay
                            {
                                Controller = "RL从站-202",
                                Field = string.Format("Relay{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 8; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Relay[j + 16] =
                            new VisionDeviceConfigDeviceInfoBidingsRelay
                            {
                                Controller = "RL从站-203",
                                Field = string.Format("Relay{0}", j + 1)
                            };
                    }
                    for (var j = 0; j < 8; j++)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.Bidings.Relay[j + 24] =
                            new VisionDeviceConfigDeviceInfoBidingsRelay
                            {
                                Controller = "RL从站-204",
                                Field = string.Format("Relay{0}", j + 1)
                            };
                    }

                    VisionCommon.VisionDeviceConfig.DeviceInfo.Actions =
                        new VisionDeviceConfigDeviceInfoActions
                        {
                            Start =
                                new VisionDeviceConfigDeviceInfoActionsStart
                                {
                                    Controller = "控制器1IP28",
                                    Field = "Di1"
                                },
                            Bang = new VisionDeviceConfigDeviceInfoActionsBang
                            {
                                Controller = "RL从站-202",
                                Field = "Relay3"
                            }
                        };
                }

                VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                XmlHelper.SerializeToFile(VisionCommon.VisionDeviceConfig, Program.SysDir + @"\图像检测配置文件\CCD_DeviceConfig.CcdDeviceConfig", Encoding.UTF8);
                ShowSuccessTip("保存成功！");
            }
        }
    }
}
