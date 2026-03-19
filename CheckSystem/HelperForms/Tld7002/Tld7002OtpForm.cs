using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility.FileOperator;
using Controller;
using HZH_Controls.Controls;
using HZH_Controls.Controls.RadioButton;
using HZH_Controls.IconFont;
using UserControls;

namespace CheckSystem.HelperForms.Tld7002
{
    public partial class Tld7002OtpForm : Form
    {
        private readonly InfineonTld7002 _infineonTld7002 = new InfineonTld7002("TLD7002刷写工具");
        private readonly SyControllerWith56Pin _controllerWith56Pin;
        private readonly Tld7002Config _tld7002Config;
        private string _configFilePath;
        //private bool _isOtpExed = false;//= 0; // 0=不启动，1=仿真，2=OTP WRITE，3=OTP READ
        private bool _isOtpIng;
        private Thread _mainThread;
        private readonly string _title;

        public Tld7002OtpForm(
            SyControllerWith56Pin controllerWith56Pin, Tld7002Config tld7002Config = null, string configFilePath = null)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            _controllerWith56Pin = controllerWith56Pin;
            _infineonTld7002.MySerialPort = _controllerWith56Pin.GatewaySci2;
            _configFilePath = configFilePath;
            _title = Text;

            if (tld7002Config != null)
            {
                _tld7002Config = tld7002Config;
            }
            else
            {
                _tld7002Config = new Tld7002Config
                {
                    DeviceInfo = new Tld7002ConfigDeviceInfo
                    {
                        DeviceName = "新建文件",
                        Relays = new Tld7002ConfigDeviceInfoRelays
                        {
                            Vs = "RELAY1",
                            Gpin0 = "RELAY2",
                            PowerDelay = "250",
                            InputButton = "DI1"
                        },
                        Files = new string[0]
                    }
                };
            }

            Text = string.Format("{0} ({1})", _title, _tld7002Config.DeviceInfo.DeviceName);

            Icon = FontImages.GetIcon(
               FontIcons.A_fa_flash, 32,
               Color.DodgerBlue);
            txtOtpStatus.BackColor = Color.DarkGoldenrod;
            txtOtpResult.BackColor = Color.DarkGoldenrod;
            txtVs.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            txtGpin0.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            txtPowerDelay.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            txtInputBtn.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            Load += Tld7002OtpForm_Load;
            Closed += FormDataViewer_Closed;
            //Closing += Tld7002OtpForm_Closing;
        }

        //void Tld7002OtpForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    if (_isOtp)
        //    {
        //        MessageBox.Show(@"正在编程， 请等待编程结束再关闭");
        //        return;
        //    }
        //}

        private async void Tld7002OtpForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        txtIp.textBox.Text = _controllerWith56Pin.RemoteIpPort;
                        txtControllerType.textBox.Text = _controllerWith56Pin.GetType().Name;

                        for (var i = 0; i < 12; i++)
                        {
                            var str = string.Format("RELAY{0}", i + 1);
                            txtVs.comboBox.Items.Add(str);
                            txtGpin0.comboBox.Items.Add(str);

                            if (str == _tld7002Config.DeviceInfo.Relays.Vs)
                                txtVs.comboBox.SelectedIndex = i;
                            if (str == _tld7002Config.DeviceInfo.Relays.Gpin0)
                                txtGpin0.comboBox.SelectedIndex = i;
                        }

                        txtPowerDelay.comboBox.Items.Add(50);
                        txtPowerDelay.comboBox.Items.Add(100);
                        txtPowerDelay.comboBox.Items.Add(150);
                        txtPowerDelay.comboBox.Items.Add(200);
                        txtPowerDelay.comboBox.Items.Add(250);
                        txtPowerDelay.comboBox.Items.Add(300);
                        txtPowerDelay.comboBox.Items.Add(350);
                        txtPowerDelay.comboBox.Items.Add(400);
                        txtPowerDelay.comboBox.Items.Add(450);
                        txtPowerDelay.comboBox.Items.Add(500);
                        txtPowerDelay.comboBox.Items.Add(1000);

                        for (var i = 0; i < txtPowerDelay.comboBox.Items.Count; i++)
                        {
                            if (txtPowerDelay.comboBox.Items[i].ToString() !=
                                _tld7002Config.DeviceInfo.Relays.PowerDelay)
                                continue;
                            txtPowerDelay.comboBox.SelectedIndex = i;
                            break;
                        }

                        for (var i = 0; i < 2; i++)
                        {
                            var str = string.Format("DI{0}", i + 1);
                            txtInputBtn.comboBox.Items.Add(str);

                            if (str == _tld7002Config.DeviceInfo.Relays.InputButton)
                                txtInputBtn.comboBox.SelectedIndex = i;
                        }

                        foreach (var radio in _tld7002Config.DeviceInfo.Files.Select(f => new UCRadioButton
                        {
                            TextValue = f,
                            Margin = new Padding(25),
                            Height = 100,
                            Dock = DockStyle.Top
                        }))
                        {
                            ckFilesList.Controls.Add(radio);
                            radio.CheckedChangeEvent += radio_CheckedChangeEvent;
                        }

                        if (ckFilesList.Controls.Count > 0)
                        {
                            var radion = ckFilesList.Controls[ckFilesList.Controls.Count - 1] as UCRadioButton;
                            if (radion != null)
                                radion.Checked = true;
                        }

                        if (_mainThread != null)
                        {
                            _mainThread.Abort();
                            _mainThread.Join();
                        }

                        _mainThread = new Thread(MainWork) { IsBackground = true };
                        _mainThread.Start();
                    }));
                }
            });
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void FormDataViewer_Closed(object sender, EventArgs e)
        {
            if (_mainThread != null)
            {
                _mainThread.Abort();
                _mainThread.Join();
            }
            if (_infineonTld7002 != null)
                _infineonTld7002.Dispose();
            if (_controllerWith56Pin != null)
                _controllerWith56Pin.Dispose();
        }

        private void MainWork()
        {
            PowerReset();

            while (_mainThread.IsAlive)
            {
                if (!_mainThread.IsAlive)
                    break;
                Thread.Sleep(50);

                if (_isOtpIng)
                    continue;

                var di = 0.ToString();
                _controllerWith56Pin.GetCurrentVoltageDi();
                if (_tld7002Config.DeviceInfo.Relays.InputButton == "DI1")
                    di = _controllerWith56Pin.Di1;
                else if (_tld7002Config.DeviceInfo.Relays.InputButton == "DI2")
                    di = _controllerWith56Pin.Di2;

                if (_isOtpIng)
                    continue;

                if (di == 1.ToString())
                {
                    btnOtpWrite_Click(null, null);
                }
            }
        }

        public void PowerReset()
        {
            _controllerWith56Pin.Relay1 = false;
            _controllerWith56Pin.Relay2 = false;
            _controllerWith56Pin.Relay3 = false;
            _controllerWith56Pin.Relay4 = false;
            _controllerWith56Pin.Relay5 = false;
            _controllerWith56Pin.Relay6 = false;
            _controllerWith56Pin.Relay7 = false;
            _controllerWith56Pin.Relay8 = false;
            _controllerWith56Pin.Relay9 = false;
            _controllerWith56Pin.Relay10 = false;
            _controllerWith56Pin.Relay11 = false;
            _controllerWith56Pin.Relay12 = false;
            _controllerWith56Pin.SetOutputs();
            Thread.Sleep(50);
        }

        public void PowerSet(bool isOn)
        {
            // vs power on
            var relayVs = _tld7002Config.DeviceInfo.Relays.Gpin0.Replace("RELAY", "Relay");
            var findRelayVs = _controllerWith56Pin.GetType().GetField(relayVs);
            if (findRelayVs != null)
                findRelayVs.SetValue(_controllerWith56Pin, isOn);
            // delay
            Thread.Sleep(int.Parse(_tld7002Config.DeviceInfo.Relays.PowerDelay));
            // gpin0 power on
            var relayGpin0 = _tld7002Config.DeviceInfo.Relays.Gpin0.Replace("RELAY", "Relay");
            var findRelayGpin0 = _controllerWith56Pin.GetType().GetField(relayGpin0);
            if (findRelayGpin0 != null)
                findRelayGpin0.SetValue(_controllerWith56Pin, isOn);
            _controllerWith56Pin.SetOutputs();
            // delay
            Thread.Sleep(int.Parse(_tld7002Config.DeviceInfo.Relays.PowerDelay));
            // otp
        }

        private void radio_CheckedChangeEvent(object sender, EventArgs e)
        {
            var ucRadioButton = sender as UCRadioButton;
            if (ucRadioButton != null) txtSelectedFilePath.Text = ucRadioButton.TextValue;
        }

        private void btnClearCfgList_Click(object sender, EventArgs e)
        {
            ckFilesList.Controls.Clear();
            txtSelectedFilePath.Text = string.Empty;
        }

        private void btnAddCfgToList_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = @"文本文件|*.ocfg",
                Multiselect = true,
            };
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            var files = fileDialog.FileNames;
            foreach (var f in files)
            {
                var radio = new UCRadioButton
                {
                    TextValue = f,
                    Margin = new Padding(25),
                    Height = 100,
                    Dock = DockStyle.Top
                };
                ckFilesList.Controls.Add(radio);
                radio.CheckedChangeEvent += radio_CheckedChangeEvent;
                //Console.WriteLine(f);
            }

            if (ckFilesList.Controls.Count <= 0)
                return;
            var radion = ckFilesList.Controls[ckFilesList.Controls.Count - 1] as UCRadioButton;
            if (radion != null)
                radion.Checked = true;
        }

        private void btnSaveCfgList_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_configFilePath))
            {
                MessageBox.Show(@"请先保存硬件参数");
                return;
            }

            var filePath = _configFilePath;

            var list =
                ckFilesList.Controls.OfType<UCRadioButton>().Select(ucRadioButton => ucRadioButton.TextValue).ToList();

            _tld7002Config.DeviceInfo.Files = new string[list.Count];
            for (var i = 0; i < list.Count; i++)
                _tld7002Config.DeviceInfo.Files[i] = list[i];

            try
            {
                XmlHelper.SerializeToFile(_tld7002Config, filePath, Encoding.UTF8);
                MessageBox.Show(@"保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"保存失败，" + ex.Message);
            }
        }

        private void btnEmulate_Click(object sender, EventArgs e)
        {
            if (_isOtpIng)
            {
                MessageBox.Show(@"正在编程，请稍后在试");
                return;
            }

            txtOtpCostTime.textBox.Text = string.Empty;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _isOtpIng = true;
            ResetOtpStatusResult();
            PowerReset();
            PowerSet(true);
            _infineonTld7002.OtpConfigFilePath = txtSelectedFilePath.Text;
            _infineonTld7002.EmulateConfiguration();
            txtOtpStatus.textBox.Text = _infineonTld7002.OtpStatus;
            txtOtpResult.textBox.Text = _infineonTld7002.CompareReadedOtp;
            txtOtpStatus.BackColor = _infineonTld7002.OtpStatus == "0x01" ? Color.DarkGreen : Color.DarkRed;
            txtOtpResult.BackColor = _infineonTld7002.CompareReadedOtp == "OK" ? Color.DarkGreen : Color.DarkRed;
            PowerSet(false);
            _isOtpIng = false;
            stopWatch.Stop();
            txtOtpCostTime.textBox.Text = stopWatch.ElapsedMilliseconds + @"ms";
        }

        private void btnOtpWrite_Click(object sender, EventArgs e)
        {
            if (_isOtpIng)
            {
                MessageBox.Show(@"正在编程，请稍后在试");
                return;
            }
            txtOtpCostTime.textBox.Text = string.Empty;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _isOtpIng = true;
            ResetOtpStatusResult();
            PowerReset();
            PowerSet(true);
            _infineonTld7002.OtpConfigFilePath = txtSelectedFilePath.Text;
            _infineonTld7002.WriteConfiguration();
            txtOtpStatus.textBox.Text = _infineonTld7002.OtpStatus;
            txtOtpResult.textBox.Text = _infineonTld7002.CompareReadedOtp;
            txtOtpStatus.BackColor = _infineonTld7002.OtpStatus == "0x03" ? Color.DarkGreen : Color.DarkRed;
            txtOtpResult.BackColor = _infineonTld7002.CompareReadedOtp == "OK" ? Color.DarkGreen : Color.DarkRed;
            PowerSet(false);
            _isOtpIng = false;
            stopWatch.Stop();
            txtOtpCostTime.textBox.Text = stopWatch.ElapsedMilliseconds + @"ms";
        }

        private void btnOtpRead_Click(object sender, EventArgs e)
        {
            if (_isOtpIng)
            {
                MessageBox.Show(@"正在编程，请稍后在试");
                return;
            }

            txtOtpCostTime.textBox.Text = string.Empty;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _isOtpIng = true;
            ResetOtpStatusResult();
            PowerReset();
            PowerSet(true);
            _infineonTld7002.OtpConfigFilePath = txtSelectedFilePath.Text;
            _infineonTld7002.OtpRead();
            txtOtpStatus.textBox.Text = _infineonTld7002.OtpStatus;
            txtOtpResult.textBox.Text = _infineonTld7002.CompareReadedOtp;
            //txtOtpStatus.BackColor = _infineonTld7002.OtpStatus != "0x03" ? Color.DarkGreen : Color.DarkRed;
            txtOtpResult.BackColor = _infineonTld7002.CompareReadedOtp == "OK" ? Color.DarkGreen : Color.DarkRed;
            PowerSet(false);
            _isOtpIng = false;
            stopWatch.Stop();
            txtOtpCostTime.textBox.Text = stopWatch.ElapsedMilliseconds + @"ms";
        }

        private void ResetOtpStatusResult()
        {
            txtOtpStatus.BackColor = Color.DarkGoldenrod;
            txtOtpResult.BackColor = Color.DarkGoldenrod;
            txtOtpStatus.textBox.Text = string.Empty;
            txtOtpResult.textBox.Text = string.Empty;
            //Thread.Sleep(1000);
        }

        private void buttonParaEdit_Click(object sender, EventArgs e)
        {
            if (btnEditParas.Text == @"编辑")
            {
                txtVs.Enabled = true;
                txtGpin0.Enabled = true;
                txtPowerDelay.Enabled = true;
                txtInputBtn.Enabled = true;
                btnEditParas.Text = @"取消编辑";
                btnSaveParas.Enabled = true;
            }
            else
            {
                txtVs.Enabled = false;
                txtGpin0.Enabled = false;
                txtPowerDelay.Enabled = false;
                txtInputBtn.Enabled = false;
                btnEditParas.Text = @"编辑";
                btnSaveParas.Enabled = false;
            }
        }

        private void buttonParaSave_Click(object sender, EventArgs e)
        {
            if (txtGpin0.comboBox.Text == txtVs.comboBox.Text)
            {
                MessageBox.Show(@"无法保存，VS引脚和GPIN0引脚无法使用同一继电器");
                return;
            }

            var newForm = new Form
            {
                Text = @"配置文件名称",
                Width = 500,
                Height = 150
            };
            newForm.MaximumSize = new Size(newForm.Width, newForm.Height);
            newForm.MaximizeBox = false;
            newForm.StartPosition = FormStartPosition.CenterParent;
            newForm.Icon = FontImages.GetIcon(
               FontIcons.E_icon_document, 32,
               Color.DodgerBlue);

            var txtDeviceName = new LabelText
            {
                LabelString = "标题",
                Name = "配置文件名称",
                Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134),
                Dock = DockStyle.Top,
                textBox =
                {
                    Text = string.IsNullOrEmpty(_configFilePath)
                        ? string.Empty
                        : _tld7002Config.DeviceInfo.DeviceName
                }
            };

            var btnDeviceName = new Button
            {
                Text = @"确认",
                Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134),
                Dock = DockStyle.Top,
                Height = 50
            };
            btnDeviceName.Click += btnDeviceName_Click;

            newForm.Controls.Add(btnDeviceName);
            newForm.Controls.Add(txtDeviceName);

            if (newForm.ShowDialog() != DialogResult.OK)
                return;
            string filePath;
            if (string.IsNullOrEmpty(_configFilePath))
            {
                var fileDialog = new SaveFileDialog
                {
                    Filter = @"文本文件|*.tld7002",
                    //FileName = txtDeviceName.textBox.Text,
                };
                if (fileDialog.ShowDialog() != DialogResult.OK)
                    return;

                if (string.IsNullOrEmpty(fileDialog.FileName))
                    return;

                filePath = fileDialog.FileName;
            }
            else
            {
                filePath = _configFilePath;
            }

            _tld7002Config.DeviceInfo.DeviceName = txtDeviceName.Text;
            _tld7002Config.DeviceInfo.Relays.Vs = txtVs.comboBox.Text;
            _tld7002Config.DeviceInfo.Relays.Gpin0 = txtGpin0.comboBox.Text;
            _tld7002Config.DeviceInfo.Relays.InputButton = txtInputBtn.comboBox.Text;
            _tld7002Config.DeviceInfo.Relays.PowerDelay = txtPowerDelay.comboBox.Text;

            try
            {
                if (string.IsNullOrEmpty(_configFilePath))
                    _configFilePath = filePath;
                Text = string.Format("{0} ({1})", _title, _tld7002Config.DeviceInfo.DeviceName);
                XmlHelper.SerializeToFile(_tld7002Config, filePath, Encoding.UTF8);
                MessageBox.Show(@"保存成功");
                buttonParaEdit_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"保存失败，" + ex.Message);
            }
        }

        private static void btnDeviceName_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            var parent1 = btn.Parent;
            var form = parent1 as Form;
            if (form == null) return;
            var findTxt = form.Controls.Find("配置文件名称", false);

            if (!findTxt.Any()) return;
            var txt = findTxt[0] as LabelText;
            if (txt != null && !string.IsNullOrEmpty(txt.textBox.Text))
                form.DialogResult = DialogResult.OK;
        }
    }
}
