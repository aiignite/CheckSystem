using HZH_Controls.IconFont;
using Sunny.UI;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.SgmWirelessCharging
{
    public partial class FrmDataRead : UIForm
    {
        public FrmDataRead()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Load += FrmDataRead_Load;
        }

        private void FrmDataRead_Load(object sender, EventArgs e)
        {
            Icon = HZH_Controls.IconFont.FontImages.GetIcon(FontIcons.A_fa_search, 32, Color.DodgerBlue);

            cmbSqlIpAddr.Items.Add("192.168.0.138");
            //cmbSqlIpAddr.Items.Add("127.0.0.1");
            cmbSqlIpAddr.SelectedIndex = 0;

            var dataGridView = keyData.dataGridView;
            keyData.label.Text = @"SGM无线充电模块数据查询";

            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AllowUserToResizeColumns = true;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.EditMode = DataGridViewEditMode.EditOnKeystroke;
            dataGridView.Margin = new Padding(3, 4, 3, 4);
            dataGridView.RowTemplate.Height = 30;

            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = true;
            dataGridView.AllowUserToResizeRows = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var start = DateTime.Parse(string.Format(@"{0} 00:00:00", startTime.Text));
            var end = DateTime.Parse(string.Format(@"{0} 23:59:59", endTime.Text));

            var dataGridView = keyData.dataGridView;

            using (var wirelessChargingModule = new Controller.WirelessChargingModule("WirelessChargingModule"))
            {
                dataGridView.DataSource = null;
                var src = wirelessChargingModule.ReadEcuIdsFromServer(start, end, 1);
                UIMessageBox.Show("共有" + src.Count + "个ECU ID");
                dataGridView.DataSource = src;

                //MiniExcel.SaveAs(@"export.xlsx", src);
            }
        }

        private void btnSearchNoUsedEcuIds_Click(object sender, EventArgs e)
        {
            var dataGridView = keyData.dataGridView;

            using (var wirelessChargingModule = new Controller.WirelessChargingModule("WirelessChargingModule"))
            {
                dataGridView.DataSource = null;
                var src = wirelessChargingModule.ReadEcuIdsFromServer(DateTime.Now.AddYears(-100), DateTime.Now.AddYears(100), 0);
                UIMessageBox.Show("共有" + src.Count + "个未使用ECU ID");
                dataGridView.DataSource = src;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            TestNewUpload();
            return;

            if (string.IsNullOrEmpty(cmbSqlIpAddr.Text))
                return;

            // 选择路径
            string path;
            using (var folder = new FolderBrowserDialog())
            {
                var result = folder.ShowDialog();
                if (result != DialogResult.OK)
                    return;
                path = folder.SelectedPath;
            }

            using (var wirelessChargingModule = new Controller.WirelessChargingModule("WirelessChargingModule"))
            {
                wirelessChargingModule.ReadEcuIdsFromSRecordAndUploadToServer(path);
                MessageBox.Show(@"导入结束");

                //sgm458.ServerIp = cmbSqlIpAddr.Text;
                //sgm458.EcuidsConfigDirectory = path;
                //sgm458.ReadEcuIdsFromSRecodrAndUploadToServer();
                //MessageBox.Show(@"导入结束");
            }
        }

        private void TestNewUpload()
        {
            string path;
            using (var openFi = new OpenFileDialog
            {
                Site = null,
                Tag = null,
                AddExtension = false,
                CheckPathExists = false,
                DefaultExt = null,
                DereferenceLinks = false,
                FileName = null,
                Filter = null,
                FilterIndex = 0,
                InitialDirectory = null,
                RestoreDirectory = false,
                ShowHelp = false,
                SupportMultiDottedExtensions = false,
                Title = null,
                ValidateNames = false,
                AutoUpgradeEnabled = false,
                CheckFileExists = false,
                Multiselect = false,
                ReadOnlyChecked = false,
                ShowReadOnly = false
            })
            {
                openFi.Filter = "asc加密文件(.ASC)|*.asc;|所有文件(*.*)|*.*";
                if (openFi.ShowDialog() == DialogResult.OK)
                {
                    var fileInfo = new FileInfo(openFi.FileName);

                    if (!fileInfo.Name.ToLower().StartsWith("SGM-ECUIDs-".ToLower()))
                    {
                        UIMessageBox.ShowError("请选择以'\"SGM-ECUIDs-'开头的文件");
                        return;
                    }

                    var option = new UIEditOption();
                    option.AddInteger("count", "待解密ecu id数量", 1);
                    var uiOption = new UIEditForm(option);
                    uiOption.Render();
                    uiOption.CheckedData += (sender, args) =>
                    {
                        var c = (int)args.Form["count"];
                        if (c > 0)
                        {
                            return true;
                        }
                        else
                        {
                            UIMessageTip.ShowWarning("请输入＞0的整数");
                            return false;
                        }
                    };
                    uiOption.ShowDialog();
                    var uiResult = uiOption.IsOK;
                    if (!uiResult)
                    {
                        ShowInfoTip("操作取消");
                        return;
                    }

                    var toVerifuCount = (int)uiOption["count"];

                    using (var wirelessChargingModule = new Controller.WirelessChargingModule("WirelessChargingModule"))
                    {
                        var errorMsg = string.Empty;
                        var isUploadOk = wirelessChargingModule.ReadEcuIdsFromSRecordAndUploadToServerNew(openFi.FileName, toVerifuCount, out errorMsg);

                        if (isUploadOk)
                        {
                            UIMessageBox.ShowSuccess("解密并转换完成，共增加" + toVerifuCount + "个ECU ID");
                        }
                        else
                        {
                            UIMessageBox.ShowError(string.Format("解密并转换完成失败：{0}", errorMsg));
                        }
                    }


                    //UIMessageBox.ShowSuccess("解密并转换完成，共" + ecuDatas.Count + "个ECU ID");

                    //var cmd = new Process();
                    //var startInfo = new ProcessStartInfo
                    //{
                    //    FileName = Program.SysDir + @"\DllImport\OpenPgpDecrypt\OpenPgpDecrypt\OpenPgpDecrypt\bin\Debug\OpenPgpDecrypt.exe",
                    //    Arguments = string.Empty,
                    //    UseShellExecute = false,
                    //    RedirectStandardInput = true,
                    //    RedirectStandardOutput = true,
                    //    CreateNoWindow = true
                    //};
                    //cmd.StartInfo = startInfo;

                    //var keyStr = string.Empty;
                    //if (cmd.Start())
                    //{
                    //    //var srcFilePath = Console.ReadLine();
                    //    //var privateKeyPath = Console.ReadLine();
                    //    //var password = Console.ReadLine();

                    //    var input = fileInfo.FullName;
                    //    var privateKeyPath = Program.SysDir + @"\DllImport\OpenPgpDecrypt\Seeyao_Private_Key.asc";
                    //    var password = "Scae-1234";

                    //    cmd.StandardInput.WriteLine(input);
                    //    Thread.Sleep(10);
                    //    cmd.StandardInput.WriteLine(privateKeyPath);
                    //    Thread.Sleep(10);
                    //    cmd.StandardInput.WriteLine(password);
                    //    Thread.Sleep(10);

                    //    var enterTs = HighPrecisionTimer.GetTimestamp();

                    //    while (true)
                    //    {
                    //        var log = cmd.StandardOutput.ReadLine();
                    //        if (log == null)
                    //            break;
                    //        if (!log.StartsWith(@"{""") || !log.EndsWith(@"""}"))
                    //            continue;

                    //        var nowTs = HighPrecisionTimer.GetTimestamp();
                    //        if (HighPrecisionTimer.GetTimestampIntervalMs(enterTs, nowTs) > 2000)
                    //            break;

                    //        keyStr = log;
                    //        break;
                    //    }
                    //    cmd.StandardInput.WriteLine("\n");
                    //}

                    //if (string.IsNullOrEmpty(keyStr))
                    //{
                    //    UIMessageBox.ShowError("解密失败");
                    //}
                    //else
                    //{
                    //    DecryptResult decryptResult;
                    //    try
                    //    {
                    //        decryptResult = JsonConvert.DeserializeObject<DecryptResult>(keyStr);
                    //    }
                    //    catch (Exception)
                    //    {
                    //        decryptResult = null;
                    //    }

                    //    if (decryptResult is null)
                    //    {
                    //        UIMessageBox.ShowError("解密失败");
                    //    }
                    //    else
                    //    {
                    //        if (!decryptResult.IsDecryptOk)
                    //        {
                    //            UIMessageBox.ShowError("解密失败: " + decryptResult.ErrorMsg);
                    //        }
                    //        else
                    //        {
                    //            var filePath = decryptResult.DecryptFilePath;
                    //            var resultFileInfo = new FileInfo(filePath);
                    //            if (!resultFileInfo.Exists || resultFileInfo.Extension.ToLower() != ".bin")
                    //            {
                    //                UIMessageBox.ShowError("解密失败：解密后的文件非.bin文件");
                    //            }
                    //            else
                    //            {
                    //                var record = BinToS19Converter.ConvertBinToS19Lines(filePath).Where(f => f.Type == CommonUtility.FileOperator.SRecordFileHelper.SRecordType.S3).ToList();
                    //                //if (record.Any(f => (f.Data is null || f.Data.Length != 32 || f.DataLen != 32)))
                    //                //{
                    //                //    UIMessageBox.ShowError("解密后转换ECU ID失败：出现不规则的block数据");
                    //                //}
                    //                //else
                    //                {
                    //                    var blocks = SRecordFileHelper.GetBlocks(record);
                    //                    var listDatas = new List<byte>();

                    //                    foreach (var t in blocks.SelectMany(b => b))
                    //                        listDatas.AddRange(t.Data);

                    //                    if (listDatas.Count % 144 != 0)
                    //                    {
                    //                        UIMessageBox.ShowError("解密后转换ECU ID失败：数据不完整");
                    //                    }
                    //                    else
                    //                    {
                    //                        var ServerIp = "192.168.1.150";
                    //                        var ServerDataBase = "IPMS";
                    //                        var ServerUid = "sa";
                    //                        var ServerPwd = "123456";

                    //                        var serverIp = ServerIp;

                    //                        using (var db = new SqlSugarClient(
                    //                            new ConnectionConfig
                    //                            {
                    //                                ConnectionString = string.Format(@"server={0};database={1};uid={2};pwd={3}", serverIp, ServerDataBase, ServerUid, ServerPwd),
                    //                                DbType = DbType.SqlServer,
                    //                                IsAutoCloseConnection = true,
                    //                                InitKeyType = InitKeyType.SystemTable
                    //                            }))
                    //                        {

                    //                            var ecuDatas = new List<ECUData>();

                    //                            for (var i = 0; i < listDatas.Count; i = i + 144)
                    //                            {
                    //                                var ecuIdStruct = new ECUData
                    //                                {
                    //                                    EcuIdFromFile = fileInfo.Name,
                    //                                    CreateTime = DateTime.Now,
                    //                                    LastRecordTime = DateTime.Now,
                    //                                    MASTER_ECU_KEY_KeySlot = "FF",
                    //                                    UNLOCK_ECU_KEY_KeySlot = "01",
                    //                                    Barcode = string.Empty,
                    //                                    TrackInfo = string.Empty,
                    //                                    IsUploadToSgm = 0,
                    //                                    IsUsage = 0,
                    //                                    IsConstMasterKeyOk = 0,
                    //                                    IsConstUnlockKeyOk = 0
                    //                                };

                    //                                for (var j = 0; j < 144; j++)
                    //                                {
                    //                                    if (j >= 0 && j < 16)
                    //                                        ecuIdStruct.EcuId += ValueHelper.GetHextStr(listDatas[i + j]);
                    //                                    else if (j >= 16 && j < 32)
                    //                                        ecuIdStruct.MASTER_ECU_KEY_M1 += ValueHelper.GetHextStr(listDatas[i + j]);
                    //                                    else if (j >= 32 && j < 64)
                    //                                        ecuIdStruct.MASTER_ECU_KEY_M2 += ValueHelper.GetHextStr(listDatas[i + j]);
                    //                                    else if (j >= 64 && j < 80)
                    //                                        ecuIdStruct.MASTER_ECU_KEY_M3 += ValueHelper.GetHextStr(listDatas[i + j]);
                    //                                    else if (j >= 80 && j < 96)
                    //                                        ecuIdStruct.UNLOCK_ECU_KEY_M1 += ValueHelper.GetHextStr(listDatas[i + j]);
                    //                                    else if (j >= 96 && j < 128)
                    //                                        ecuIdStruct.UNLOCK_ECU_KEY_M2 += ValueHelper.GetHextStr(listDatas[i + j]);
                    //                                    else if (j >= 128 && j < 144)
                    //                                        ecuIdStruct.UNLOCK_ECU_KEY_M3 += ValueHelper.GetHextStr(listDatas[i + j]);
                    //                                }

                    //                                if (!ecuIdStruct.EcuId.StartsWith("000"))
                    //                                {
                    //                                    UIMessageBox.ShowError("解密后转换ECU ID失败：出现异常的ECU ID：" + ecuIdStruct.EcuId);
                    //                                    return;
                    //                                }

                    //                                if (!ecuIdStruct.MASTER_ECU_KEY_M1.StartsWith("00000000000000000000000000000011"))
                    //                                {
                    //                                    UIMessageBox.ShowError(string.Format("解密后转换MASTER_ECU_KEY_M1失败：ECU_ID={0}, 解密后转换MASTER_ECU_KEY_M1失败={1}", ecuIdStruct.EcuId, ecuIdStruct.MASTER_ECU_KEY_M1));
                    //                                    return;
                    //                                }
                    //                                if (!ecuIdStruct.MASTER_ECU_KEY_M2.StartsWith("A4BA2FEECF1"))
                    //                                {
                    //                                    UIMessageBox.ShowError(string.Format("解密后转换MASTER_ECU_KEY_M2失败：ECU_ID={0}, 解密后转换MASTER_ECU_KEY_M2失败={1}", ecuIdStruct.EcuId, ecuIdStruct.MASTER_ECU_KEY_M2));
                    //                                    return;
                    //                                }
                    //                                if (!ecuIdStruct.UNLOCK_ECU_KEY_M1.StartsWith("00000000000000000000000000000044"))
                    //                                {
                    //                                    UIMessageBox.ShowError(string.Format("解密后转换UNLOCK_ECU_KEY_M1失败：ECU_ID={0}, 解密后转换UNLOCK_ECU_KEY_M1失败={1}", ecuIdStruct.EcuId, ecuIdStruct.UNLOCK_ECU_KEY_M1));
                    //                                    return;
                    //                                }
                    //                                if (!ecuIdStruct.UNLOCK_ECU_KEY_M2.StartsWith("1BAFA3B95A64DF9B40B8FD17A47433E1"))
                    //                                {
                    //                                    UIMessageBox.ShowError(string.Format("解密后转换UNLOCK_ECU_KEY_M2失败：ECU_ID={0}, 解密后转换UNLOCK_ECU_KEY_M2失败={1}", ecuIdStruct.EcuId, ecuIdStruct.UNLOCK_ECU_KEY_M2));
                    //                                    return;
                    //                                }

                    //                                ecuDatas.Add(ecuIdStruct);
                    //                            }

                    //                            if (toVerifuCount != ecuDatas.Count)
                    //                            {
                    //                                UIMessageBox.ShowError(string.Format("待解密数量={0}, 本次解密数量={1}，数量不符，无法上传至服务器", toVerifuCount, ecuDatas.Count));
                    //                                return;
                    //                            }
                    //                            else
                    //                            {
                    //                                var getall = db.Queryable<ECUData>().Select(it => new { CsName = it.EcuId }).ToList();
                    //                                foreach (var item in getall)
                    //                                {
                    //                                    var isRepeat = db.Queryable<ECUData>().Any(f => f.EcuId.ToLower() == item.CsName.ToLower());
                    //                                    if (isRepeat)
                    //                                    {
                    //                                        UIMessageBox.ShowError(string.Format("解密后转换的ECU ID: {0}, 在数据库中出现重复的，当前文件标记异常，所有ECU ID都无法导入", item.CsName));
                    //                                        return;
                    //                                    }
                    //                                }


                    //                                var insertable = db.Insertable(ecuDatas);
                    //                                insertable.IgnoreColumns("id"); // 忽略自增长列
                    //                                var result = insertable.ExecuteReturnIdentity(); // 执行插入并返回插入数据的ID

                    //                                UIMessageBox.ShowSuccess("解密并转换完成，共" + ecuDatas.Count + "个ECU ID");
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
        }

        private class DecryptResult
        {
            public bool IsDecryptOk { get; set; } = false;
            public string DecryptFilePath { get; set; } = string.Empty;
            public string ErrorMsg { get; set; } = string.Empty;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var start = DateTime.Parse(string.Format(@"{0} 00:00:00", startTime.Text));
            var end = DateTime.Parse(string.Format(@"{0} 23:59:59", endTime.Text));

            var dataGridView = keyData.dataGridView;

            using (var wirelessChargingModule = new Controller.WirelessChargingModule("WirelessChargingModule"))
            {
                dataGridView.DataSource = null;
                var historyData = wirelessChargingModule.ReadEcuIdsFromServer(start, end, 1);
                dataGridView.DataSource = historyData;

                if (historyData.Any())
                {
                    // 选择路径
                    string path;
                    using (var folder = new FolderBrowserDialog())
                    {
                        var result = folder.ShowDialog();
                        if (result != DialogResult.OK)
                            return;
                        path = folder.SelectedPath + string.Format(@"\WirelessCharging-{0}_From{1}To{2}.xml",
                            DateTime.Now.ToString(CultureInfo.InvariantCulture)
                            .Replace(":", string.Empty)
                            .Replace("/", string.Empty)
                            .Replace(" ", string.Empty),
                            start.ToString("yyyyMMdd"),
                            end.ToString("yyyyMMdd"));

                        int count;
                        var isSuccess = wirelessChargingModule.PrintToXml(path, historyData, out count);
                        if (isSuccess)
                        {
                            var fileDir = Path.GetDirectoryName(path);
                            var fileName = Path.GetFileNameWithoutExtension(path);
                            var subffix = Path.GetExtension(path);
                            var newFileName = string.Format("{0}\\{1}_{2}pcs{3}", fileDir, fileName, count, subffix);
                            File.Move(path, newFileName);

                            MessageBox.Show(string.Format("导出完成，导出内容为当前显示数据。\r\n共导出：{0}个\r\n路径：{1}", count, newFileName));
                        }
                        else
                        {
                            MessageBox.Show(@"导出失败");
                        }
                    }
                }
            }
        }
    }
}
