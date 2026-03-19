using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using CommonUtility;
using Controller;
using Sunny.UI;

namespace CheckSystem.HelperForms.Psi5Reader
{
    public partial class FrmHalReWrite : UIForm
    {
        private HalHac3980Programmer _halHac3980Programmer = new HalHac3980Programmer("HalHac3980Programmer");
        private Color _txtDefaultColor = Color.Gray;
        private Color _txtOkColor = Color.LightGreen;
        private Color _txtNgColor = Color.OrangeRed;

        private Dictionary<byte, RegStruct> _before = new Dictionary<byte, RegStruct>();
        private Dictionary<byte, RegStruct> _after = new Dictionary<byte, RegStruct>();
        private Dictionary<byte, RegStruct> _standardExt = new Dictionary<byte, RegStruct>();

        internal struct RegStruct
        {
            public string Name;
            public string Value;
        }

        public FrmHalReWrite()
        {
            InitializeComponent();
            var s = System.IO.Ports.SerialPort.GetPortNames().OrderBy(f => f);
            if (s.Any())
            {
                foreach (var t in s)
                    uiComboBox1.Items.Add(t);
                uiComboBox1.SelectedIndex = 0;
            }

            Closed += FrmHalReWrite_Closed;
        }

        private void FrmHalReWrite_Closed(object sender, EventArgs e)
        {
            if (_halHac3980Programmer != null)
                _halHac3980Programmer.Dispose();
        }

        private async void uiButton1_Click(object sender, EventArgs e)
        {
            txtResult.Text = string.Format(@"[{0}] 正在执行。。。。。", DateTime.Now.ToString("yyyyMMdd hh:mm:ss"));
            txtResult.FillReadOnlyColor = _txtDefaultColor;
            uiPanel1.Enabled = false;

            _standardExt.Clear();

            #region 返工到最新状态

            if (uiRadioButton1.Checked)
            {
                _standardExt.Add(0x0B, new RegStruct { Value = "3EC7" });
                _standardExt.Add(0x0C, new RegStruct { Value = "00F5" });
                _standardExt.Add(0x00, new RegStruct { Value = "1111" });
                _standardExt.Add(0x01, new RegStruct { Value = "1111" });
                _standardExt.Add(0x02, new RegStruct { Value = "1111" });
                _standardExt.Add(0x03, new RegStruct { Value = "1111" });
                _standardExt.Add(0x04, new RegStruct { Value = "1111" });
                _standardExt.Add(0x05, new RegStruct { Value = "1111" });
                _standardExt.Add(0x06, new RegStruct { Value = "1111" });
                _standardExt.Add(0x07, new RegStruct { Value = "6161" });
            }


            #endregion

            #region 最新状态再返工成老状态

            else if (uiRadioButton2.Checked)
            {
                _standardExt.Add(0x0B, new RegStruct { Value = "3EC5" });
                _standardExt.Add(0x0C, new RegStruct { Value = "00F7" });
                _standardExt.Add(0x00, new RegStruct { Value = "0000" });
                _standardExt.Add(0x01, new RegStruct { Value = "0000" });
                _standardExt.Add(0x02, new RegStruct { Value = "0000" });
                _standardExt.Add(0x03, new RegStruct { Value = "0000" });
                _standardExt.Add(0x04, new RegStruct { Value = "0000" });
                _standardExt.Add(0x05, new RegStruct { Value = "0000" });
                _standardExt.Add(0x06, new RegStruct { Value = "0000" });
                _standardExt.Add(0x07, new RegStruct { Value = "6160" });
            }

            #endregion

            listAfter.Items.Clear();
            listBefore.Items.Clear();

            var dicBefore = new Dictionary<byte, RegStruct>();
            var dicBeforeExt = new Dictionary<byte, RegStruct>();
            var dicAfter = new Dictionary<byte, RegStruct>();
            var dicAfterExt = new Dictionary<byte, RegStruct>();

            await Task.Run(() =>
            {
                if (e == null)
                    throw new ArgumentNullException(nameof(e));
                _halHac3980Programmer.GetFirmwareVersion();
                Console.WriteLine(_halHac3980Programmer.FirmwareVersion);

                _halHac3980Programmer.SwitchModeB();
                _halHac3980Programmer.SetBitTime();
                _halHac3980Programmer.VsupOn();
                _halHac3980Programmer.StartListeningMode();
                _halHac3980Programmer.StartProgrammingMode();
                _halHac3980Programmer.ReadAddr0X75();
                _halHac3980Programmer.SelectIoChannel(1);

                // 写之前先读
                var isCanWrite = false;
                {
                    var errorCount = 0;
                    for (var i = 0; i <= 0x4F; i++)
                    {
                        if (errorCount == 0)
                        {
                            string result;
                            var ok = _halHac3980Programmer.ReadOneRegisterFromEEPROM((byte)i, out result);
                            if (!ok)
                            {
                                errorCount++;
                                dicBefore.Add((byte)i, new RegStruct { Value = "read error before write" });
                            }
                            else
                            {
                                dicBefore.Add((byte)i, new RegStruct { Value = result });
                            }
                        }
                        else
                        {
                            dicBefore.Add((byte)i, new RegStruct { Value = "read error before write" });
                        }
                    }
                    for (var i = 0; i < 16; i++)
                    {
                        if (errorCount == 0)
                        {
                            string result;
                            var ok = _halHac3980Programmer.ReadOneRegisterFromEEPROMExt((byte)i, out result);
                            if (!ok)
                            {
                                errorCount++;
                                dicBeforeExt.Add((byte)i, new RegStruct { Value = "read ext error before write" });
                            }
                            else
                            {
                                dicBeforeExt.Add((byte)i, new RegStruct { Value = result });
                            }
                        }
                        else
                        {
                            dicBeforeExt.Add((byte)i, new RegStruct { Value = "read ext error before write" });
                        }
                    }

                    if (errorCount == 0)
                        isCanWrite = true;
                }

                // 写
                {
                    if (isCanWrite)
                    {
                        foreach (var item in _standardExt)
                        {
                            string error;
                            _halHac3980Programmer.WriteAndStoreRegisterToEepromExt(item.Key, item.Value.Value, out error);
                        }
                    }
                }

                // 写之后再读
                {
                    var errorCount = 0;
                    for (var i = 0; i <= 0x4F; i++)
                    {
                        if (errorCount == 0)
                        {
                            string result;
                            var ok = _halHac3980Programmer.ReadOneRegisterFromEEPROM((byte)i, out result);
                            if (!ok)
                            {
                                errorCount++;
                                dicAfter.Add((byte)i, new RegStruct { Value = "read error after write" });
                            }
                            else
                            {
                                dicAfter.Add((byte)i, new RegStruct { Value = result });
                            }
                        }
                        else
                        {
                            dicAfter.Add((byte)i, new RegStruct { Value = "read error after write" });
                        }
                    }
                    for (var i = 0; i < 16; i++)
                    {
                        if (errorCount == 0)
                        {
                            string result;
                            var ok = _halHac3980Programmer.ReadOneRegisterFromEEPROMExt((byte)i, out result);
                            if (!ok)
                            {
                                errorCount++;
                                dicAfterExt.Add((byte)i, new RegStruct { Value = "read ext error after write" });
                            }
                            else
                            {
                                dicAfterExt.Add((byte)i, new RegStruct { Value = result });
                            }
                        }
                        else
                        {
                            dicAfterExt.Add((byte)i, new RegStruct { Value = "read ext error after write" });
                        }
                    }
                }

                _halHac3980Programmer.VsupOff();
            });

            var beforeReadStr = string.Empty;
            foreach (var value in dicBefore.Select(item => string.Format("{0} = {1}", ValueHelper.GetHextStrWithOx(item.Key), item.Value.Value)))
            {
                listBefore.Items.Add(value);
                beforeReadStr += value + Environment.NewLine;
            }
            foreach (var value in dicBeforeExt.Select(item => string.Format("{0}(EXT) = {1}", ValueHelper.GetHextStrWithOx(item.Key), item.Value.Value)))
            {
                listBefore.Items.Add(value);
                beforeReadStr += value + Environment.NewLine;
            }

            var afterStrBefore = string.Empty;
            foreach (var value in dicAfter.Select(item => string.Format("{0} = {1}", ValueHelper.GetHextStrWithOx(item.Key), item.Value.Value)))
            {
                listAfter.Items.Add(value);
                afterStrBefore += value + Environment.NewLine;
            }
            foreach (var value in dicAfterExt.Select(item => string.Format("{0}(EXT) = {1}", ValueHelper.GetHextStrWithOx(item.Key), item.Value.Value)))
            {
                listAfter.Items.Add(value);
                afterStrBefore += value + Environment.NewLine;
            }

            var ngCount = 0;

            for (var i = 0; i <= 0x4F; i++)
            {
                if (dicBefore[(byte)i].Value == dicAfter[(byte)i].Value)
                    continue;

                ngCount++;
            }
            for (var i = 0; i < 16; i++)
            {
                if (_standardExt.ContainsKey((byte)i))
                {
                    if (_standardExt[(byte)i].Value == dicAfterExt[(byte)i].Value)
                        continue;
                    ngCount++;
                }
                else
                {
                    if (dicBeforeExt[(byte)i].Value == dicAfterExt[(byte)i].Value)
                        continue;
                    ngCount++;
                }
            }

            if (ngCount > 0)
            {
                txtResult.Text = string.Format(@"[{0}] NG", DateTime.Now.ToString("yyyyMMdd hh:mm:ss"));
                txtResult.FillReadOnlyColor = _txtNgColor;
            }
            else
            {
                txtResult.Text = string.Format(@"[{0}] OK", DateTime.Now.ToString("yyyyMMdd hh:mm:ss"));
                txtResult.FillReadOnlyColor = _txtOkColor;
            }

            uiPanel1.Enabled = true;
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            //_halHac3980Programmer.ConnectCom("COM8");

            if (uiComboBox1.Items.Count > 0 && !string.IsNullOrEmpty(uiComboBox1.Text) && uiComboBox1.Text.ToUpper().StartsWith("COM"))
            {
                _halHac3980Programmer.ConnectCom(uiComboBox1.Text);
                uiButton1.Enabled = true;
                uiButton2.Enabled = false;
                uiComboBox1.Enabled = false;
            }
            else
            {
                ShowErrorTip("请选择正确的串口号");
            }
        }
    }
}
