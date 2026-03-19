using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility;
using HZH_Controls.IconFont;

namespace CheckSystem.HelperForms.Asl5115
{
    public partial class AslFormMtp : Form
    {
        public AslFormMtp()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.A_fa_plug, 32,
               Color.DodgerBlue);

            cmbHz.SelectedIndex = 0;
            cmbProtocol.SelectedIndex = 0;

            cmbSlewRateGroup1.SelectedIndex =
                cmbSlewRateGroup2.SelectedIndex = cmbSlewRateGroup3.SelectedIndex = cmbSlewRateGroup4.SelectedIndex = 7;

            cmbOpenCircuitGroup1.SelectedIndex =
                cmbOpenCircuitGroup2.SelectedIndex =
                    cmbOpenCircuitGroup3.SelectedIndex = cmbOpenCircuitGroup4.SelectedIndex = 1;
        }

        private async void btnRead_Click(object sender, EventArgs e)
        {
            if (AslFormLedControl.MatrixAsl5115 == null)
                return;

            listBox1.Items.Clear();
            Enabled = false;

            await Task.Run(() =>
            {
                AslFormLedControl.MatrixAsl5115.EnterMormalMode();

                for (var i = 0x34; i <= 0x60; i++)
                //for (var i = 0x38; i <= 0x60; i++)
                {
                    var methodName = string.Format("ReadMtpReg0X{0}", ValueHelper.GetHextStr((byte)i));
                    var findMethod = AslFormLedControl.MatrixAsl5115.GetType().GetMethod(methodName);
                    if (findMethod != null)
                        findMethod.Invoke(
                                AslFormLedControl.MatrixAsl5115, null);
                }

                Invoke(new Action(() =>
                {
                    var isNgCount = 0;

                    for (var i = 0x34; i <= 0x60; i++)
                    //for (var i = 0x38; i <= 0x60; i++)
                    {
                        var str = string.Format("Reg0X{0}", ValueHelper.GetHextStr((byte)i));
                        var findField = AslFormLedControl.MatrixAsl5115.GetType().GetField(str);
                        if (findField != null)
                        {
                            if (findField.GetValue(AslFormLedControl.MatrixAsl5115) == null ||
                                string.IsNullOrEmpty(findField.GetValue(AslFormLedControl.MatrixAsl5115).ToString()))
                            {
                                isNgCount++;
                            }
                            else
                            {
                                listBox1.Items.Add(string.Format("[{0}]: {1}", str,
                                findField.GetValue(AslFormLedControl.MatrixAsl5115)));
                            }
                        }
                    }

                    if (isNgCount == 0)
                        btnWrite.Enabled = true;

                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X58))
                    {
                        var reg0X58Bits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X58);
                        for (var i = 0; i < cmbSlewRateGroup1.Items.Count; i++)
                        {
                            var txt = cmbSlewRateGroup1.Items[i].ToString();
                            if (
                                !txt.StartsWith(GetSlewRate(reg0X58Bits[3], reg0X58Bits[2], reg0X58Bits[1],
                                    reg0X58Bits[0]))) continue;
                            cmbSlewRateGroup1.SelectedIndex = i;
                            break;
                        }

                        cmbOpenCircuitGroup1.SelectedIndex = reg0X58Bits[4] == 0.ToString() ? 0 : 1;

                        cbCh1.Checked = reg0X58Bits[5] == "1";
                        cbCh2.Checked = reg0X58Bits[6] == "1";
                        cbCh3.Checked = reg0X58Bits[7] == "1";
                    }
                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X59))
                    {
                        var reg0X59Bits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X59);
                        for (var i = 0; i < cmbSlewRateGroup2.Items.Count; i++)
                        {
                            var txt = cmbSlewRateGroup2.Items[i].ToString();
                            if (
                                !txt.StartsWith(GetSlewRate(reg0X59Bits[3], reg0X59Bits[2], reg0X59Bits[1],
                                    reg0X59Bits[0]))) continue;
                            cmbSlewRateGroup2.SelectedIndex = i;
                            break;
                        }

                        cmbOpenCircuitGroup2.SelectedIndex = reg0X59Bits[4] == 0.ToString() ? 0 : 1;

                        cbCh4.Checked = reg0X59Bits[5] == "1";
                        cbCh5.Checked = reg0X59Bits[6] == "1";
                        cbCh6.Checked = reg0X59Bits[7] == "1";
                    }
                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X5A))
                    {
                        var reg0X5ABits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X5A);
                        for (var i = 0; i < cmbSlewRateGroup3.Items.Count; i++)
                        {
                            var txt = cmbSlewRateGroup3.Items[i].ToString();
                            if (
                                !txt.StartsWith(GetSlewRate(reg0X5ABits[3], reg0X5ABits[2], reg0X5ABits[1],
                                    reg0X5ABits[0]))) continue;
                            cmbSlewRateGroup3.SelectedIndex = i;
                            break;
                        }

                        cmbOpenCircuitGroup3.SelectedIndex = reg0X5ABits[4] == 0.ToString() ? 0 : 1;

                        cbCh7.Checked = reg0X5ABits[5] == "1";
                        cbCh8.Checked = reg0X5ABits[6] == "1";
                        cbCh9.Checked = reg0X5ABits[7] == "1";
                    }
                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X5B))
                    {
                        var reg0X5BBits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X5B);
                        for (var i = 0; i < cmbSlewRateGroup4.Items.Count; i++)
                        {
                            var txt = cmbSlewRateGroup4.Items[i].ToString();
                            if (
                                !txt.StartsWith(GetSlewRate(reg0X5BBits[3], reg0X5BBits[2], reg0X5BBits[1],
                                    reg0X5BBits[0]))) continue;
                            cmbSlewRateGroup4.SelectedIndex = i;
                            break;
                        }

                        cmbOpenCircuitGroup4.SelectedIndex = reg0X5BBits[4] == 0.ToString() ? 0 : 1;

                        cbCh10.Checked = reg0X5BBits[5] == "1";
                        cbCh11.Checked = reg0X5BBits[6] == "1";
                        cbCh12.Checked = reg0X5BBits[7] == "1";
                    }

                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X5C))
                    {
                        var reg0X5CBits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X5C);
                        cmbChargePumpGroup1.Checked = reg0X5CBits[15] == "1";
                    }
                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X5D))
                    {
                        var reg0X5DBits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X5D);
                        cmbChargePumpGroup2.Checked = reg0X5DBits[15] == "1";
                    }
                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X5E))
                    {
                        var reg0X5EBits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X5E);
                        cmbChargePumpGroup3.Checked = reg0X5EBits[15] == "1";
                    }
                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X5F))
                    {
                        var reg0X5FBits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X5F);
                        cmbChargePumpGroup4.Checked = reg0X5FBits[15] == "1";
                    }

                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X60))
                    {
                        var reg0X60Bits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X60);
                        cmbHz.SelectedIndex = reg0X60Bits[0] == "0" ? 0 : 1;
                        cmbProtocol.SelectedIndex = reg0X60Bits[4] == "0" ? 0 : 1;
                    }
                }));
            });

            Enabled = true;
        }

        private async void btnWrite_Click(object sender, EventArgs e)
        {
            if (AslFormLedControl.MatrixAsl5115 == null)
                return;

            listBox1.Items.Clear();
            Enabled = false;

            await Task.Run(() =>
            {
                AslFormLedControl.MatrixAsl5115.EnterMormalMode();

                Invoke(new Action(() =>
                {
                    #region 先写
                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X58))
                    {
                        var reg0X58Bits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X58);
                        reg0X58Bits[0] = GetSlewRateBit(cmbSlewRateGroup1.Text)[3].ToString();
                        reg0X58Bits[1] = GetSlewRateBit(cmbSlewRateGroup1.Text)[2].ToString();
                        reg0X58Bits[2] = GetSlewRateBit(cmbSlewRateGroup1.Text)[1].ToString();
                        reg0X58Bits[3] = GetSlewRateBit(cmbSlewRateGroup1.Text)[0].ToString();

                        reg0X58Bits[4] = cmbOpenCircuitGroup1.SelectedIndex == 0 ? "0" : "1";

                        reg0X58Bits[5] = cbCh1.Checked ? "1" : "0";
                        reg0X58Bits[6] = cbCh2.Checked ? "1" : "0";
                        reg0X58Bits[7] = cbCh3.Checked ? "1" : "0";

                        var lo =
                            Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                            reg0X58Bits[0], reg0X58Bits[1], reg0X58Bits[2],
                            reg0X58Bits[3], reg0X58Bits[4], reg0X58Bits[5],
                            reg0X58Bits[6], reg0X58Bits[7]), 2);

                        var hi =
                           Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                           reg0X58Bits[8], reg0X58Bits[9], reg0X58Bits[10],
                           reg0X58Bits[11], reg0X58Bits[12], reg0X58Bits[13],
                           reg0X58Bits[14], reg0X58Bits[15]), 2);

                        var newUshort = ((ushort)(hi * 256 + lo)).ToString("X").PadLeft(4, '0');
                        AslFormLedControl.MatrixAsl5115.WriteMtpReg0X58(newUshort);
                    }

                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X59))
                    {
                        var reg0X59Bits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X59);
                        reg0X59Bits[0] = GetSlewRateBit(cmbSlewRateGroup2.Text)[3].ToString();
                        reg0X59Bits[1] = GetSlewRateBit(cmbSlewRateGroup2.Text)[2].ToString();
                        reg0X59Bits[2] = GetSlewRateBit(cmbSlewRateGroup2.Text)[1].ToString();
                        reg0X59Bits[3] = GetSlewRateBit(cmbSlewRateGroup2.Text)[0].ToString();

                        reg0X59Bits[4] = cmbOpenCircuitGroup2.SelectedIndex == 0 ? "0" : "1";

                        reg0X59Bits[5] = cbCh4.Checked ? "1" : "0";
                        reg0X59Bits[6] = cbCh5.Checked ? "1" : "0";
                        reg0X59Bits[7] = cbCh6.Checked ? "1" : "0";

                        var lo =
                            Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                            reg0X59Bits[0], reg0X59Bits[1], reg0X59Bits[2],
                            reg0X59Bits[3], reg0X59Bits[4], reg0X59Bits[5],
                            reg0X59Bits[6], reg0X59Bits[7]), 2);

                        var hi =
                           Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                           reg0X59Bits[8], reg0X59Bits[9], reg0X59Bits[10],
                           reg0X59Bits[11], reg0X59Bits[12], reg0X59Bits[13],
                           reg0X59Bits[14], reg0X59Bits[15]), 2);

                        var newUshort = ((ushort)(hi * 256 + lo)).ToString("X").PadLeft(4, '0');
                        AslFormLedControl.MatrixAsl5115.WriteMtpReg0X59(newUshort);
                    }

                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X5A))
                    {
                        var reg0X5ABits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X5A);
                        reg0X5ABits[0] = GetSlewRateBit(cmbSlewRateGroup3.Text)[3].ToString();
                        reg0X5ABits[1] = GetSlewRateBit(cmbSlewRateGroup3.Text)[2].ToString();
                        reg0X5ABits[2] = GetSlewRateBit(cmbSlewRateGroup3.Text)[1].ToString();
                        reg0X5ABits[3] = GetSlewRateBit(cmbSlewRateGroup3.Text)[0].ToString();

                        reg0X5ABits[4] = cmbOpenCircuitGroup3.SelectedIndex == 0 ? "0" : "1";

                        reg0X5ABits[5] = cbCh7.Checked ? "1" : "0";
                        reg0X5ABits[6] = cbCh8.Checked ? "1" : "0";
                        reg0X5ABits[7] = cbCh9.Checked ? "1" : "0";

                        var lo =
                            Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                            reg0X5ABits[0], reg0X5ABits[1], reg0X5ABits[2],
                            reg0X5ABits[3], reg0X5ABits[4], reg0X5ABits[5],
                            reg0X5ABits[6], reg0X5ABits[7]), 2);

                        var hi =
                           Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                           reg0X5ABits[8], reg0X5ABits[9], reg0X5ABits[10],
                           reg0X5ABits[11], reg0X5ABits[12], reg0X5ABits[13],
                           reg0X5ABits[14], reg0X5ABits[15]), 2);

                        var newUshort = ((ushort)(hi * 256 + lo)).ToString("X").PadLeft(4, '0');
                        AslFormLedControl.MatrixAsl5115.WriteMtpReg0X5A(newUshort);
                    }

                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X5B))
                    {
                        var reg0X5BBits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X5B);
                        reg0X5BBits[0] = GetSlewRateBit(cmbSlewRateGroup4.Text)[3].ToString();
                        reg0X5BBits[1] = GetSlewRateBit(cmbSlewRateGroup4.Text)[2].ToString();
                        reg0X5BBits[2] = GetSlewRateBit(cmbSlewRateGroup4.Text)[1].ToString();
                        reg0X5BBits[3] = GetSlewRateBit(cmbSlewRateGroup4.Text)[0].ToString();

                        reg0X5BBits[4] = cmbOpenCircuitGroup4.SelectedIndex == 0 ? "0" : "1";

                        reg0X5BBits[5] = cbCh10.Checked ? "1" : "0";
                        reg0X5BBits[6] = cbCh11.Checked ? "1" : "0";
                        reg0X5BBits[7] = cbCh12.Checked ? "1" : "0";

                        var lo =
                            Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                            reg0X5BBits[0], reg0X5BBits[1], reg0X5BBits[2],
                            reg0X5BBits[3], reg0X5BBits[4], reg0X5BBits[5],
                            reg0X5BBits[6], reg0X5BBits[7]), 2);

                        var hi =
                           Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                           reg0X5BBits[8], reg0X5BBits[9], reg0X5BBits[10],
                           reg0X5BBits[11], reg0X5BBits[12], reg0X5BBits[13],
                           reg0X5BBits[14], reg0X5BBits[15]), 2);

                        var newUshort = ((ushort)(hi * 256 + lo)).ToString("X").PadLeft(4, '0');
                        AslFormLedControl.MatrixAsl5115.WriteMtpReg0X5B(newUshort);
                    }

                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X5C))
                    {
                        var reg0X5CBits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X5C);
                        reg0X5CBits[15] = cmbChargePumpGroup1.Checked ? "1" : "0";

                        var lo =
                            Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                            reg0X5CBits[0], reg0X5CBits[1], reg0X5CBits[2],
                            reg0X5CBits[3], reg0X5CBits[4], reg0X5CBits[5],
                            reg0X5CBits[6], reg0X5CBits[7]), 2);

                        var hi =
                           Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                           reg0X5CBits[8], reg0X5CBits[9], reg0X5CBits[10],
                           reg0X5CBits[11], reg0X5CBits[12], reg0X5CBits[13],
                           reg0X5CBits[14], reg0X5CBits[15]), 2);

                        var newUshort = ((ushort)(hi * 256 + lo)).ToString("X").PadLeft(4, '0');
                        AslFormLedControl.MatrixAsl5115.WriteMtpReg0X5C(newUshort);
                    }
                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X5D))
                    {
                        var reg0X5DBits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X5D);
                        reg0X5DBits[15] = cmbChargePumpGroup2.Checked ? "1" : "0";

                        var lo =
                            Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                            reg0X5DBits[0], reg0X5DBits[1], reg0X5DBits[2],
                            reg0X5DBits[3], reg0X5DBits[4], reg0X5DBits[5],
                            reg0X5DBits[6], reg0X5DBits[7]), 2);

                        var hi =
                           Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                           reg0X5DBits[8], reg0X5DBits[9], reg0X5DBits[10],
                           reg0X5DBits[11], reg0X5DBits[12], reg0X5DBits[13],
                           reg0X5DBits[14], reg0X5DBits[15]), 2);

                        var newUshort = ((ushort)(hi * 256 + lo)).ToString("X").PadLeft(4, '0');
                        AslFormLedControl.MatrixAsl5115.WriteMtpReg0X5D(newUshort);
                    }
                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X5E))
                    {
                        var reg0X5EBits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X5E);
                        reg0X5EBits[15] = cmbChargePumpGroup3.Checked ? "1" : "0";

                        var lo =
                            Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                            reg0X5EBits[0], reg0X5EBits[1], reg0X5EBits[2],
                            reg0X5EBits[3], reg0X5EBits[4], reg0X5EBits[5],
                            reg0X5EBits[6], reg0X5EBits[7]), 2);

                        var hi =
                           Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                           reg0X5EBits[8], reg0X5EBits[9], reg0X5EBits[10],
                           reg0X5EBits[11], reg0X5EBits[12], reg0X5EBits[13],
                           reg0X5EBits[14], reg0X5EBits[15]), 2);

                        var newUshort = ((ushort)(hi * 256 + lo)).ToString("X").PadLeft(4, '0');
                        AslFormLedControl.MatrixAsl5115.WriteMtpReg0X5E(newUshort);
                    }
                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X5F))
                    {
                        var reg0X5FBits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X5F);
                        reg0X5FBits[15] = cmbChargePumpGroup4.Checked ? "1" : "0";

                        var lo =
                            Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                            reg0X5FBits[0], reg0X5FBits[1], reg0X5FBits[2],
                            reg0X5FBits[3], reg0X5FBits[4], reg0X5FBits[5],
                            reg0X5FBits[6], reg0X5FBits[7]), 2);

                        var hi =
                           Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                           reg0X5FBits[8], reg0X5FBits[9], reg0X5FBits[10],
                           reg0X5FBits[11], reg0X5FBits[12], reg0X5FBits[13],
                           reg0X5FBits[14], reg0X5FBits[15]), 2);

                        var newUshort = ((ushort)(hi * 256 + lo)).ToString("X").PadLeft(4, '0');
                        AslFormLedControl.MatrixAsl5115.WriteMtpReg0X5F(newUshort);
                    }

                    if (!string.IsNullOrEmpty(AslFormLedControl.MatrixAsl5115.Reg0X60))
                    {
                        var reg0X60Bits = GetBits(AslFormLedControl.MatrixAsl5115.Reg0X60);
                        reg0X60Bits[0] = cmbHz.SelectedIndex == 0 ? "0" : "1";
                        reg0X60Bits[4] = cmbProtocol.SelectedIndex == 0 ? "0" : "1";

                        var lo =
                            Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                            reg0X60Bits[0], reg0X60Bits[1], reg0X60Bits[2],
                            reg0X60Bits[3], reg0X60Bits[4], reg0X60Bits[5],
                            reg0X60Bits[6], reg0X60Bits[7]), 2);

                        var hi =
                           Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                           reg0X60Bits[8], reg0X60Bits[9], reg0X60Bits[10],
                           reg0X60Bits[11], reg0X60Bits[12], reg0X60Bits[13],
                           reg0X60Bits[14], reg0X60Bits[15]), 2);

                        var newUshort = ((ushort)(hi * 256 + lo)).ToString("X").PadLeft(4, '0');
                        AslFormLedControl.MatrixAsl5115.WriteMtpReg0X60(newUshort);
                    }
                    #endregion

                    #region 再读
                    for (var i = 0x38; i <= 0x60; i++)
                    {
                        var methodName = string.Format("ReadMtpReg0X{0}", ValueHelper.GetHextStr((byte)i));
                        var findMethod = AslFormLedControl.MatrixAsl5115.GetType().GetMethod(methodName);
                        if (findMethod != null)
                            findMethod.Invoke(
                                    AslFormLedControl.MatrixAsl5115, null);
                    }

                    var isNgCount = 0;

                    for (var i = 0x38; i <= 0x60; i++)
                    {
                        var str = string.Format("Reg0X{0}", ValueHelper.GetHextStr((byte)i));
                        var findField = AslFormLedControl.MatrixAsl5115.GetType().GetField(str);
                        if (findField != null)
                        {
                            if (findField.GetValue(AslFormLedControl.MatrixAsl5115) == null ||
                                string.IsNullOrEmpty(findField.GetValue(AslFormLedControl.MatrixAsl5115).ToString()))
                            {
                                isNgCount++;
                            }
                            else
                            {
                                listBox1.Items.Add(string.Format("[{0}]: {1}", str,
                                findField.GetValue(AslFormLedControl.MatrixAsl5115)));
                            }
                        }
                    }
                    #endregion
                }));
            });

            btnWrite.Enabled = false;
            Enabled = true;
        }

        private static List<string> GetBits(string mtpValue)
        {
            var regValue = mtpValue;

            var hi = Convert.ToByte(regValue.Substring(0, 2), 16);
            var lo = Convert.ToByte(regValue.Substring(2, 2), 16);
            var boolList =
                Convert.ToString(lo, 2).PadLeft(8, '0').ToCharArray().Reverse().Select(v => v.ToString()).ToList();
            boolList.AddRange(Convert.ToString(hi, 2).PadLeft(8, '0').ToCharArray().Reverse().Select(v => v.ToString()));

            return boolList;
        }

        public string GetSlewRate(
            string bitD3, string bitD2, string bitD1, string bitD0)
        {
            var str = string.Format("{0}{1}{2}{3}", bitD3, bitD2, bitD1, bitD0);
            switch (str)
            {
                case "0000":
                    return "0,2";

                case "0001":
                    return "0,4";

                case "0010":
                    return "0,6";

                case "0011":
                    return "0,9";

                case "0100":
                    return "1,1";

                case "0101":
                    return "1,3";

                case "0110":
                    return "1,5";

                case "0111":
                    return "1,7";

                case "1000":
                    return "1,9";

                case "1001":
                    return "2,3";

                case "1010":
                    return "2,8";

                case "1011":
                    return "3,2";

                case "1100":
                    return "3,8";

                case "1101":
                    return "4,5";

                case "1110":
                    return "5,5";

                case "1111":
                    return "6,8";
            }

            return str;
        }

        public string GetSlewRateBit(string value)
        {
            if (value.StartsWith("0,2"))
                return "0000";
            if (value.StartsWith("0,4"))
                return "0001";
            if (value.StartsWith("0,6"))
                return "0010";
            if (value.StartsWith("0,9"))
                return "0011";
            if (value.StartsWith("1,1"))
                return "0100";
            if (value.StartsWith("1,3"))
                return "0101";
            if (value.StartsWith("1,5"))
                return "0110";
            if (value.StartsWith("1,7"))
                return "0111";
            if (value.StartsWith("1,9"))
                return "1000";
            if (value.StartsWith("2,3"))
                return "1001";
            if (value.StartsWith("2,8"))
                return "1010";
            if (value.StartsWith("3,2"))
                return "1011";
            if (value.StartsWith("3,8"))
                return "1100";
            if (value.StartsWith("4,5"))
                return "1101";
            if (value.StartsWith("5,5"))
                return "1110";
            if (value.StartsWith("6,8"))
                return "1111";

            return "0111";
        }
    }
}
