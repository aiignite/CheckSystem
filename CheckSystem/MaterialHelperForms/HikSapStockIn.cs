using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckSystem.MaterialHelperForms.MaterialModels;
using CommonUtility.FileOperator;
using HZH_Controls.IconFont;

namespace CheckSystem.MaterialHelperForms
{
    public partial class HikSapStockIn : Form
    {
        private readonly IniFileHelper _setup =
            new IniFileHelper(string.Format(@"{0}\仓库电子料标签生成\{1}", Program.SysDir, "StockSysSetup.ini"));

        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        public HikSapStockIn()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_icon_check, 32,
               Color.DodgerBlue);
            notifyIcon1.Icon = FontImages.GetIcon(FontIcons.E_icon_folder_upload, 32,
               Color.DodgerBlue);
            //toolTip1.SetToolTip(notifyIcon1,"入库操作提示");
            cmbStockInNo.comboBox.SelectedIndexChanged += cmbStockInNocomboBox_SelectedIndexChanged;
            cmbMaterialNo.comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            BindData();
        }

        private void BindData()
        {
            cmbStockInNo.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSupplyNo.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMaterialNo.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            {
                // 先获取数量，SELECT COUNT(1) FROM [newMaterialStockInInfo] where restNum > 0 and Status = '待入库'  and IsDelete = '0'
                // 再根据数量长度，分批获取数据并放入List中
                // 再将List排序

                cmbStockInNo.comboBox.Items.Clear();
                cmbSupplyNo.comboBox.Items.Clear();

                const string sql = "select distinct stockInNo from newMaterialStockInDetail where (Other != '入SAP' or Other = '' or Other is null) and (Other2 is not null and LEN(Other2) > 0) and SupplyNo is not null and SupplyNo != '' and Status = '待下发' and IsDelete = \'0\' order by stockInNo desc";
                var result = HikDbHelperSql.GetData(sql).ToArray();
                foreach (var t in result)
                    cmbStockInNo.comboBox.Items.Add(t);
                //cmbStockInNo.comboBox.Items.AddRange(GetData(sql).ToArray());

                if (cmbStockInNo.comboBox.Items.Count > 0)
                    cmbStockInNo.comboBox.SelectedIndex = 0;
            }
        }

        private void cmbStockInNocomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var stockInNo = cmbStockInNo.comboBox.Text;
            if (!string.IsNullOrEmpty(stockInNo))
            {
                {
                    cmbSupplyNo.comboBox.Items.Clear();
                    var sql2 = string.Format("select distinct supplyNo from newMaterialStockInDetail where (Other != '入SAP' or Other = '' or Other is null) and (Other2 is not null and LEN(Other2) > 0) and StockInNo = '{0}' and SupplyNo is not null and SupplyNo != '' and Status = '待下发' and IsDelete = \'0\' order by supplyNo desc", stockInNo);
                    var result = HikDbHelperSql.GetData(sql2).ToArray();
                    foreach (var t in result)
                        cmbSupplyNo.comboBox.Items.Add(t);
                    if (cmbSupplyNo.comboBox.Items.Count > 0)
                        cmbSupplyNo.comboBox.SelectedIndex = 0;
                }

                {
                    cmbMaterialNo.comboBox.Items.Clear();
                    var sql2 =
                        string.Format(
                            "select distinct materialNo from newMaterialStockInDetail where (Other != '入SAP' or Other = '' or Other is null) and (Other2 is not null and LEN(Other2) > 0) and StockInNo = '{0}' and supplyNo = {1} and Status = '待下发' and IsDelete = \'0\' order by materialNo desc",
                            stockInNo, cmbSupplyNo.comboBox.Text);
                    var result = HikDbHelperSql.GetData(sql2).ToArray();
                    foreach (var t in result)
                        cmbMaterialNo.comboBox.Items.Add(t);
                    if (cmbMaterialNo.comboBox.Items.Count > 0)
                        cmbMaterialNo.comboBox.SelectedIndex = 0;

                    ToStockIn.Text = string.Empty;
                    var sql =
                           string.Format(
                               "select Other2 from newMaterialStockInDetail where IsDelete = '0' and (Other != '入SAP' or Other = '' or Other is null) and (Other2 is not null and LEN(Other2) > 0) and Status = '待下发' and StockInNo = '{0}' and materialNo = '{1}' and supplyNo = '{2}'",
                               cmbStockInNo.comboBox.Text, cmbMaterialNo.comboBox.Text, cmbSupplyNo.comboBox.Text);
                    var datas = HikDbHelperSql.GetData(sql);
                    for (var j = 0; j < datas.Count; j++)
                    {
                        var str = datas[j];
                        ToStockIn.Text += string.Format("待入库数据{0}：{1}\r\n", j + 1, str);
                    }
                }
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToStockIn.Text = string.Empty;
            AllreadySotckIn.Text = string.Empty;

            var sql =
                   string.Format(
                       "select Other2 from newMaterialStockInDetail where IsDelete = '0' and (Other != '入SAP' or Other = '' or Other is null) and (Other2 is not null and LEN(Other2) > 0) and Status = '待下发' and StockInNo = '{0}' and materialNo = '{1}' and supplyNo = '{2}'",
                       cmbStockInNo.comboBox.Text, cmbMaterialNo.comboBox.Text, cmbSupplyNo.comboBox.Text);
            var datas = HikDbHelperSql.GetData(sql);
            for (var j = 0; j < datas.Count; j++)
            {
                var str = datas[j];
                ToStockIn.Text += string.Format("待入库数据{0}：{1}\r\n", j + 1, str);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private async void btnStartSapStockIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbStockInNo.comboBox.Text) ||
                string.IsNullOrEmpty(cmbMaterialNo.comboBox.Text) ||
                string.IsNullOrEmpty(cmbSupplyNo.comboBox.Text) ||
                string.IsNullOrEmpty(ToStockIn.Text))
                return;

            const string showValue = "是否确认开始入库？\r\n请先将鼠标点击SAP界面中的入库输入框，再点击确定。\r\n入库过程中请勿操作任何鼠标、键盘等按钮！";
            if (
              MessageBox.Show(showValue, @"确认",
                  MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            mainPanel.Enabled = false;
            WindowState = FormWindowState.Minimized;
            var formName = _setup.IniReadValue("SAP", "FormName");
            var delayMsStr = _setup.IniReadValue("SAP", "SendKeyDelayMs");
            int delayMs;
            if (!int.TryParse(delayMsStr, out delayMs))
                delayMs = 2000;
            var backSpaceCountStr = _setup.IniReadValue("SAP", "BackSpaceCount");
            int backSpacecount;
            if (!int.TryParse(backSpaceCountStr, out backSpacecount))
                delayMs = 100;
            var isSapTest = _setup.IniReadValue("SAP", "IsSapTest");

            await Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        AllreadySotckIn.Text = string.Empty;
                        //var sql =
                        //    string.Format(
                        //        "select MaterialPrintNo from newMaterialStockInDetail where IsDelete = '0' and Status = '待下发' and StockInNo = '{0}' and SupplyNo = '{1}'",
                        //        cmbStockInNo.comboBox.Text, cmbSupplyNo.comboBox.Text);
                        //var sql =
                        //    string.Format(
                        //        "select Other2,MaterialPrintNo from newMaterialStockInDetail where IsDelete = '0' and (Other != '入SAP' or Other = '' or Other is null) and (Other2 is not null and LEN(Other2) > 0) and Status = '待下发' and StockInNo = '{0}' and materialNo = '{1}' and supplyNo = '{2}'",
                        //        cmbStockInNo.comboBox.Text, cmbMaterialNo.comboBox.Text, cmbSupplyNo.comboBox.Text);
                        //var dt = HikDbHelperSql.GetDataTable(sql);

                        var datas = HikDbHelperSql.GetNewMaterialStockInDetail(string.Format(
                            "IsDelete = '0' and (Other != '入SAP' or Other = '' or Other is null) and (Other2 is not null and LEN(Other2) > 0) and Status = '待下发' and StockInNo = '{0}' and materialNo = '{1}' and supplyNo = '{2}'",
                            cmbStockInNo.comboBox.Text, cmbMaterialNo.comboBox.Text, cmbSupplyNo.comboBox.Text));

                        //var datas = HikDbHelperSql.GetDataTable(sql);
                        var count = 0;
                        for (var j = 0; j < datas.Count; j++)
                        {
                            if (string.IsNullOrEmpty(datas[j].MaterialPrintNo) || string.IsNullOrEmpty(datas[j].Other2))
                                continue;

                            var other2Str = datas[j].Other2;
                            var materialPrintNo = datas[j].MaterialPrintNo;
                            var winform = FindWindow(null, formName);
                            //IntPtr winform = FindWindow(null, "USR-TCP232-Test 串口转网络调试助手");

                            if (winform != IntPtr.Zero)
                            {
                                notifyIcon1.ShowBalloonTip(0, "正在执行入库操作", string.Format("入库数据{0}：{1}\r\n", j + 1, other2Str),
                                    ToolTipIcon.Info);

                                ShowWindow(winform, 1);
                                SetForegroundWindow(winform);

                                for (var i = 0; i < backSpacecount; i++)
                                    SendKeys.SendWait("{BACKSPACE}");
                                for (var i = 0; i < backSpacecount; i++)
                                    SendKeys.SendWait("{DEL}");

                                var sendKeys = string.Empty;
                                foreach (var t in other2Str)
                                {
                                    var asciiHex = Encoding.ASCII.GetBytes(t.ToString())[0];
                                    if (asciiHex >= 0x41 && asciiHex <= 0x5A)
                                        sendKeys += string.Format("+{0}", t);
                                    else
                                        sendKeys += string.Format("{0}", t);
                                }

                                SendKeys.SendWait(sendKeys.Substring(0, sendKeys.Length - 11));
                                SendKeys.SendWait("{ENTER}");
                                Thread.Sleep(delayMs);

                                // SAP会调用打印窗体，然后输入“~”完成打印
                                //Thread.Sleep(1500);
                                //IntPtr printWindow = FindWindow("#32770", "打印");

                                //if (printWindow != IntPtr.Zero)
                                //{
                                //    SendKeys.SendWait("~");
                                //}

                                var updateSql =
                                    string.Format(
                                        "update newMaterialStockInDetail SET Other = '入SAP' where IsDelete = '0' and (Other != '入SAP' or Other = '' or Other is null) and (Other2 is not null and LEN(Other2) > 0) and Status = '待下发' and StockInNo = '{0}' and materialNo = '{1}' and supplyNo = '{2}' and MaterialPrintNo = '{3}'",
                                        cmbStockInNo.comboBox.Text, cmbMaterialNo.comboBox.Text,
                                        cmbSupplyNo.comboBox.Text, materialPrintNo);
                                count++;
                                if (isSapTest == 1.ToString())
                                {
                                    var msg = string.Format("已入库数据{0}：{1}", count, other2Str);
                                    AppendTextColor(AllreadySotckIn, msg, Color.Green, true);
                                }
                                else
                                {
                                    var updateCount = HikDbHelperSql.Update(updateSql);
                                    Console.WriteLine(updateCount);

                                    if (updateCount == 1)
                                    {
                                        var msg = string.Format("已入库数据{0}：{1}", count, other2Str);
                                        AppendTextColor(AllreadySotckIn, msg, Color.Green, true);
                                    }
                                    else
                                    {
                                        var msg = string.Format("入库失败{0}：{1}", count, other2Str);
                                        AppendTextColor(AllreadySotckIn, msg, Color.Red, true);
                                    }
                                }
                            }
                            else
                            {
                                WindowState = FormWindowState.Normal;
                                MessageBox.Show(@"未找到SAP窗体，入库失败");
                                return;
                            }
                        }

                        WindowState = FormWindowState.Normal;
                        MessageBox.Show(@"SAP入库完成，请确认SAP中数据是否异常");
                    }));
                }
            });

            mainPanel.Enabled = true;
            //btnRefresh_Click(null, null);
        }

        private static void AppendTextColor(
            RichTextBox txtBox, string text, Color color, bool addNewLine)
        {
            if (addNewLine)
                text += Environment.NewLine;

            txtBox.SelectionStart = txtBox.TextLength;
            txtBox.SelectionLength = 0;
            txtBox.SelectionColor = color;
            txtBox.AppendText(text);
            txtBox.SelectionColor = txtBox.ForeColor;
        }
    }
}
