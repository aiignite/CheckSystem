using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility;
using CommonUtility.FileOperator;
using Controller;
using Sunny.UI;

namespace CheckSystem.SyController
{
    public partial class ControllerWith56PinFlashConfigForm : UIForm
    {
        private SyControllerWith56Pin ThisController { get; set; }
        private FlashConfig FlashConfig { get; set; }

        private readonly string _flashConfigFilePath = Program.SysDir + @"\ControllerConfig\56PinFlashAddrConfig.xml";
        private readonly string _flashConfigBackupFile = Program.SysDir + @"\ControllerConfig\56PinFlashAddrConfig-sysbackup.xml";

        public ControllerWith56PinFlashConfigForm(SyControllerWith56Pin controller)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            txtLastReadFlashTime.textBox.ReadOnly = true;
            ThisController = controller;

            if (!File.Exists(_flashConfigFilePath) && !File.Exists(_flashConfigBackupFile))
            {
                MessageBox.Show(@"配置文件丢失！");
                Enabled = false;
            }
            else
            {
                FlashConfig =
                    XmlHelper.Deserialize<FlashConfig>(_flashConfigFilePath);
            }

            EditFlashConfig(false);
        }

        private void InitDataGrid(bool isEditConfigFile, bool isCanChangeFlash)
        {
            flashConfigData.dataGridView.Rows.Clear();
            flashConfigData.dataGridView.Columns.Clear();

            flashConfigData.label.Height = 30;
            flashConfigData.label.Text = @"FLASH参数-R/W";
            flashConfigData.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Content" });
            flashConfigData.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ParaName" });
            flashConfigData.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "StartByte" });
            flashConfigData.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ByteLength" });
            flashConfigData.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "IsReserve" });
            flashConfigData.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "DataType" });
            flashConfigData.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReadBytes" });
            flashConfigData.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            flashConfigData.dataGridView.AllowUserToResizeColumns = true;
            flashConfigData.dataGridView.AllowUserToResizeRows = false;

            flashConfigData.dataGridView.AllowUserToAddRows = isEditConfigFile;
            flashConfigData.dataGridView.AllowUserToDeleteRows = isEditConfigFile;
            flashConfigData.dataGridView.RowHeadersVisible = isEditConfigFile;

            if (isEditConfigFile)
            {
                flashConfigData.dataGridView.Columns[0].ReadOnly = false;
                flashConfigData.dataGridView.Columns[1].ReadOnly = false;
                flashConfigData.dataGridView.Columns[2].ReadOnly = false;
                flashConfigData.dataGridView.Columns[3].ReadOnly = false;
                flashConfigData.dataGridView.Columns[4].ReadOnly = false;
                flashConfigData.dataGridView.Columns[5].ReadOnly = false;
                flashConfigData.dataGridView.Columns[6].ReadOnly = false;
            }
            else
            {
                flashConfigData.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Value" });

                flashConfigData.dataGridView.Columns[0].ReadOnly = true;
                flashConfigData.dataGridView.Columns[1].ReadOnly = true;
                flashConfigData.dataGridView.Columns[2].ReadOnly = true;
                flashConfigData.dataGridView.Columns[3].ReadOnly = true;
                flashConfigData.dataGridView.Columns[4].ReadOnly = true;
                flashConfigData.dataGridView.Columns[5].ReadOnly = true;
                flashConfigData.dataGridView.Columns[6].ReadOnly = true;
                flashConfigData.dataGridView.Columns[7].ReadOnly = !isCanChangeFlash;
            }

            if (FlashConfig != null)
            {
                foreach (var f in FlashConfig.Flash)
                {
                    var rowAddedIndex = flashConfigData.dataGridView.Rows.Add();

                    var row = flashConfigData.dataGridView.Rows[rowAddedIndex];

                    row.Cells[0].Value = f.Content;
                    row.Cells[1].Value = f.ParameterName;
                    row.Cells[2].Value = f.StartByte;
                    row.Cells[3].Value = f.ByteLength;
                    row.Cells[4].Value = f.IsReserve;
                    row.Cells[5].Value = f.DataType;

                    if (!isEditConfigFile && ThisController.FlashReadBytes != null)
                    {
                        var debugStr = ThisController.FlashReadBytes.Aggregate(string.Empty,
                                (current, t) => current + ValueHelper.GetHextStrWithOx(t) + " ").TrimEnd();
                        Debug.WriteLine(debugStr);

                        var startByte = int.Parse(f.StartByte);
                        var byteLen = int.Parse(f.ByteLength);
                        var tempBytes = new byte[byteLen];
                        Array.Copy(ThisController.FlashReadBytes, startByte, tempBytes, 0, tempBytes.Length);

                        var tempStr =
                            tempBytes.Aggregate(string.Empty,
                                (current, t) => current + ValueHelper.GetHextStrWithOx(t) + " ").TrimEnd();
                        row.Cells[6].Value = tempStr;

                        if (f.IsReserve == "1")
                            Array.Reverse(tempBytes);

                        var value = string.Empty;
                        var memo = string.Empty;

                        switch (f.DataType.ToLower())
                        {
                            case "hex":
                                value = tempBytes.Aggregate(value,
                                    (current, t) => current + ValueHelper.GetHextStr(t));
                                break;

                            case "ascii":
                                value = tempBytes.GetStringByAsciiBytes(true);
                                break;

                            case "decimal":
                                value = ValueHelper.GetDecimal(tempBytes).ToString();
                                break;

                            case "byte":
                                value = tempBytes.Aggregate(value,
                                    (current, t) =>
                                        current + ValueHelper.GetDecimal(new[] { t }).ToString().PadLeft(3, '0'));
                                break;

                            case "float":
                                value = BitConverter.ToSingle(tempBytes, 0).ToString(CultureInfo.InvariantCulture);
                                break;
                        }

                        row.Cells[7].Value = value;
                    }
                }
            }
        }

        private async void btnReadFlashConfig_Click(object sender, EventArgs e)
        {
            Enabled = false;

            await Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        if (!ThisController.ReadFlash())
                        {
                            EditFlashConfig(false);
                            return;
                        }

                        txtLastReadFlashTime.textBox.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                        EditFlashConfig(true);
                        btnReadFlashConfig.Enabled = true;
                        btnWriteFlashConfig.Enabled = true;
                        btnReadFlashConfig.BackColor = Color.CornflowerBlue;
                        btnWriteFlashConfig.BackColor = Color.CornflowerBlue;
                    }));

                }
            });

            Enabled = true;
        }

        private void EditConfigFile()
        {
            btnWriteConfigFile.Enabled = true;
            btnWriteConfigFile.BackColor = Color.CornflowerBlue;

            btnReadFlashConfig.Enabled = false;
            btnWriteFlashConfig.Enabled = false;
            btnReadFlashConfig.BackColor = Color.Gray;
            btnWriteFlashConfig.BackColor = Color.Gray;

            InitDataGrid(true, false);
        }

        private void EditFlashConfig(bool isCanChangeFlash)
        {
            btnWriteConfigFile.Enabled = false;
            btnWriteConfigFile.BackColor = Color.Gray;

            btnReadFlashConfig.Enabled = true;
            btnWriteFlashConfig.Enabled = false;
            btnReadFlashConfig.BackColor = Color.CornflowerBlue;
            btnWriteFlashConfig.BackColor = Color.Gray;

            InitDataGrid(false, isCanChangeFlash);
        }

        private void btnEditConfigFile_Click(object sender, EventArgs e)
        {
            EditConfigFile();
        }

        private void btnEditFlashConfig_Click(object sender, EventArgs e)
        {
            EditFlashConfig(false);
        }

        private void btnWriteFlashConfig_Click(object sender, EventArgs e)
        {
            var listBs = new byte[4096];
            var errorMsg = string.Empty;

            for (var i = 0; i < flashConfigData.dataGridView.RowCount; i++)
            {
                var row = flashConfigData.dataGridView.Rows[i];

                var content = row.Cells[0].Value.ToString();
                var startByte = int.Parse(row.Cells[2].Value.ToString());
                var byteLen = int.Parse(row.Cells[3].Value.ToString());
                var dataTyepe = row.Cells[5].Value.ToString();
                var isReserve = row.Cells[4].Value.ToString() == "1";

                if (row.Cells[7].Value == null || string.IsNullOrEmpty(row.Cells[7].Value.ToString()))
                {
                    errorMsg += string.Format("第{0}行，{1}，写入的数据是空，请检查。\r\n", i + 1, content);
                }
                else
                {
                    var value = row.Cells[7].Value.ToString();
                    var tempBytes = new List<byte>();

                    switch (dataTyepe.ToLower())
                    {
                        case "hex":
                            if (value.Length != byteLen * 2)
                            {
                                errorMsg += string.Format("第{0}行，{1}，应该填入{2}个hex字符，每个字符长度为2，不足的补0，请检查。\r\n", i + 1,
                                    content, byteLen);
                            }
                            else
                            {
                                for (var j = 0; j < value.Length; j = j + 2)
                                {
                                    try
                                    {
                                        tempBytes.Add(Convert.ToByte(string.Format("{0}{1}", value[j], value[j + 1]), 16));
                                    }
                                    catch (Exception ex)
                                    {
                                        errorMsg += string.Format("第{0}行，{1}，应该填入{2}个hex字符，{3}\r\n", i + 1, byteLen,
                                            content,
                                            ex.Message);
                                        break;
                                    }
                                }

                                if (tempBytes.Count == byteLen)
                                {
                                    var tt = tempBytes.ToArray();
                                    if (isReserve)
                                        Array.Reverse(tt);
                                    Array.Copy(tt, 0, listBs, startByte, byteLen);
                                }
                            }
                            break;

                        case "ascii":
                            if (value.Length != byteLen)
                            {
                                errorMsg += string.Format("第{0}行，{1}，应该填入{2}个ascii字符，请检查。\r\n", i + 1,
                                    content, byteLen);
                            }
                            else
                            {
                                try
                                {
                                    tempBytes.AddRange(Encoding.ASCII.GetBytes(value));
                                    if (tempBytes.Count == byteLen)
                                    {
                                        var tt = tempBytes.ToArray();
                                        if (isReserve)
                                            Array.Reverse(tt);
                                        Array.Copy(tt, 0, listBs, startByte, byteLen);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errorMsg += string.Format("第{0}行，{1}，应该填入{2}个ascii字符，{3}\r\n", i + 1, byteLen,
                                           content,
                                           ex.Message);
                                }
                            }
                            break;

                        case "decimal":
                            try
                            {
                                var decimalVaue = BitConverter.GetBytes(int.Parse(value));
                                Array.Reverse(decimalVaue);

                                if (isReserve)
                                {
                                    var j = decimalVaue.Length - 1;
                                    for (var k = 0; k < byteLen; k++)
                                    {
                                        tempBytes.Add(decimalVaue[j]);
                                        j--;
                                    }
                                }
                                else
                                {
                                    var j = 4 - byteLen;
                                    for (var k = 0; k < byteLen; k++)
                                    {
                                        tempBytes.Add(decimalVaue[j]);
                                        j++;
                                    }
                                }

                                if (tempBytes.Count == byteLen)
                                    Array.Copy(tempBytes.ToArray(), 0, listBs, startByte, byteLen);
                            }
                            catch (Exception ex)
                            {
                                errorMsg += string.Format("第{0}行，{1}，应该填入整数字符，{2}\r\n", i + 1, byteLen, ex.Message);
                            }
                            break;

                        case "byte":
                            if (value.Length != byteLen * 3)
                            {
                                errorMsg += string.Format("第{0}行，{1}，应该填入{2}个0~255的整数，每个字符长度为3，不足的补0，请检查。\r\n", i + 1,
                                    content, byteLen);
                            }
                            else
                            {
                                for (var j = 0; j < value.Length; j = j + 3)
                                {
                                    try
                                    {
                                        tempBytes.Add(
                                            byte.Parse(string.Format("{0}{1}{2}", value[j], value[j + 1], value[j + 2])));
                                    }
                                    catch (Exception ex)
                                    {
                                        errorMsg += string.Format("第{0}行，{1}，应该填入{2}个0~255的整数，{3}\r\n", i + 1, content,
                                            byteLen, ex.Message);
                                        break;
                                    }
                                }

                                if (tempBytes.Count == byteLen)
                                {
                                    var tt = tempBytes.ToArray();
                                    if (isReserve)
                                        Array.Reverse(tt);
                                    Array.Copy(tt, 0, listBs, startByte, byteLen);
                                }
                            }
                            break;

                        case "float":
                            try
                            {
                                var fv = BitConverter.GetBytes(Convert.ToSingle(value));
                                //if (isReserve)
                                //    Array.Reverse(fv);
                                tempBytes.AddRange(fv);
                                if (tempBytes.Count == byteLen)
                                    Array.Copy(tempBytes.ToArray(), 0, listBs, startByte, byteLen);
                            }
                            catch (Exception ex)
                            {
                                errorMsg += string.Format("第{0}行，{1}，应该填入float字符，{2}\r\n", i + 1, content, ex.Message);
                            }
                            break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(errorMsg))
            {
                MessageBox.Show(errorMsg);
            }
            else
            {
                var sendBytes = new byte[252];
                Array.Copy(listBs, 0, sendBytes, 0, 252);

                var debugStr = sendBytes.Aggregate(string.Empty,
                               (current, t) => current + ValueHelper.GetHextStrWithOx(t) + " ").TrimEnd();
                Debug.WriteLine(debugStr);

                MessageBox.Show(ThisController.WriteFlah(sendBytes) ? @"写Flash成功，请重新上电后读取确认" : @"写Flash失败");
            }
        }
    }
}
