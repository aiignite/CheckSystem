using CheckSystem.VisionDetection;
using CheckSystem.VisionDetection.Vision;
using CommonUtility;
using CommonUtility.FileOperator;
using RestSharp;
using StateMachine;
using Sunny.UI;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CheckSystem
{
    public partial class FormXmlPage : UIPage
    {
        private readonly DataTable _dt = new DataTable();
        private int _selectDtRowIndex = -1;

        private static readonly string SystemXmlFolder = string.Format(@"{0}\{1}", Program.SysDir, "流程配置文件");
        private static readonly string SystemCcdFolder = string.Format(@"{0}\{1}", Program.SysDir, "图像检测配置文件");

        public FormXmlPage()
        {
            InitializeComponent();
            uiLine5.Text = Program.DeviceNo;
            if (!Directory.Exists(SystemXmlFolder))
                Directory.CreateDirectory(SystemXmlFolder);
            if (!Directory.Exists(SystemCcdFolder))
                Directory.CreateDirectory(SystemCcdFolder);
            InitCmbDataGridView();
            Load += FormXmlPage_Load;
        }

        private void FormXmlPage_Load(object sender, EventArgs e)
        {
            if (Program.IsOfflineMode)
                ShowOffLineFile(false);
            else
                ShowOnLineFile();
        }

        private void InitCmbDataGridView()
        {
            _dt.Columns.Add("名称", typeof(string));
            _dt.Columns.Add("文件名", typeof(string));
            _dt.Columns.Add("路径", typeof(string));
            _dt.Columns.Add("类型", typeof(string));

            uiComboDataGridView1.DataGridView.Init();
            uiComboDataGridView1.ItemSize = new Size(360, 240);
            uiComboDataGridView1.DataGridView.AddColumn("名称", "名称");
            uiComboDataGridView1.DataGridView.AddColumn("类型", "类型");
            //uiComboDataGridView1.DataGridView.AddColumn("文件名", "文件名");
            uiComboDataGridView1.DataGridView.ReadOnly = true;
            uiComboDataGridView1.SelectIndexChange += UiComboDataGridView1_SelectIndexChange;
            uiComboDataGridView1.ValueChanged += uiComboDataGridView1_ValueChanged;
            uiComboDataGridView1.ShowFilter = true;
            uiComboDataGridView1.DataGridView.DataSource = _dt;
            //uiComboDataGridView1.FilterColumnName = "Column1"; //不设置则全部列过滤
            //btnEdit_Click(null, null);
        }

        private void UiComboDataGridView1_SelectIndexChange(object sender, int index)
        {
            uiComboDataGridView1.Text = _dt.Rows[index]["名称"].ToString();
        }

        private void uiComboDataGridView1_ValueChanged(object sender, object value)
        {
            uiComboDataGridView1.Text = string.Empty;
            var viewRow = value as DataGridViewRow;
            if (viewRow == null)
                return;
            var row = viewRow;
            uiComboDataGridView1.Text = row.Cells["名称"].Value.ToString();

            _selectDtRowIndex = -1;
            for (var i = 0; i < _dt.Rows.Count; i++)
            {
                var name = _dt.Rows[i]["名称"].ToString();
                if (name != uiComboDataGridView1.Text)
                    continue;
                _selectDtRowIndex = i;
                break;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(uiComboDataGridView1.Text) && _selectDtRowIndex != -1 && _dt.Rows.Count > _selectDtRowIndex)
            {
                if (uiComboDataGridView1.Text == _dt.Rows[_selectDtRowIndex]["名称"].ToString())
                {
                    var filePath = _dt.Rows[_selectDtRowIndex]["路径"].ToString();

                    if (filePath.Contains(uiComboDataGridView1.Text))
                    {
                        if (filePath.EndsWith(".xml"))
                        {
                            bool isDeserializeOk;

                            try
                            {
                                XmlHelper.Deserialize<DeviceConfig>(filePath);
                                isDeserializeOk = true;
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception);
                                isDeserializeOk = false;
                            }

                            if (isDeserializeOk)
                            {
                                Hide();
                                var frmSelection = new FormCheck(filePath);
                                frmSelection.ShowDialog();
                                //Environment.Exit(0);
                                Close();
                                Environment.Exit(0);
                            }
                            else
                            {
                                ShowErrorTip("系统错误，配置文件有错，加载失败，请检查配置文件格式");
                            }
                        }
                        else if (filePath.EndsWith(".ccd"))
                        {
                            bool isDeserializeOk;

                            try
                            {
                                XmlHelper.Deserialize<VisionConfig>(filePath);
                                isDeserializeOk = true;
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception);
                                isDeserializeOk = false;
                            }

                            if (isDeserializeOk)
                            {
                                Hide();
                                var frmSelection = new FrmVisionCheckMain(filePath);
                                frmSelection.ShowDialog();
                                //Environment.Exit(0);
                                Close();
                            }
                            else
                            {
                                ShowErrorTip("系统错误，配置文件有错，加载失败，请检查配置文件格式");
                            }
                        }
                        else
                        {
                            ShowErrorTip("系统错误，当前选择的配置文件后缀名不正确");
                        }
                    }
                    else
                    {
                        ShowErrorTip("系统异常");
                    }
                }
                else
                {
                    ShowErrorTip("系统错误，请选择一个配置文件");
                }
            }
            else
            {
                ShowErrorTip("请选择一个配置文件");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var pwd = string.Empty;
            if (!this.InputPasswordDialog(ref pwd))
                return;
            if (pwd == "098765")
                ShowSuccessTip("密码正确");
            else
            {
                ShowErrorTip("密码错误");
                return;
            }

            if (!string.IsNullOrEmpty(uiComboDataGridView1.Text) && _selectDtRowIndex != -1 && _dt.Rows.Count > _selectDtRowIndex)
            {
                if (uiComboDataGridView1.Text == _dt.Rows[_selectDtRowIndex]["名称"].ToString())
                {
                    var filePath = _dt.Rows[_selectDtRowIndex]["路径"].ToString();

                    if (filePath.Contains(uiComboDataGridView1.Text))
                    {
                        if (filePath.EndsWith(".xml"))
                        {
                            try
                            {
                                bool isDeserializeOk;

                                try
                                {
                                    XmlHelper.Deserialize<DeviceConfig>(filePath);
                                    isDeserializeOk = true;
                                }
                                catch (Exception exception)
                                {
                                    Console.WriteLine(exception);
                                    isDeserializeOk = false;
                                }

                                if (isDeserializeOk)
                                {
                                    var dirPath = Program.SysDir;
                                    var xmlPath = filePath;
                                    var controllerPath = dirPath + @"\Controller.dll";
                                    var userControlPath = dirPath + @"\UserControls.dll";
                                    using (var form = new DeviceDesign.FormMain(xmlPath, controllerPath, userControlPath))
                                        form.ShowDialog();
                                }
                                else
                                {
                                    ShowErrorTip("系统错误，配置文件有错，加载失败，请检查配置文件格式");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(@"打开失败：" + ex.Message);
                            }
                        }
                        else if (filePath.EndsWith(".ccd"))
                        {
                            try
                            {
                                VisionCommon.FilePath = filePath;
                                VisionCommon.VisionConfig = XmlHelper.Deserialize<VisionConfig>(VisionCommon.FilePath);

                                using (var form = new FrmDetectionMainConfig())
                                    form.ShowDialog();
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception);
                                ShowErrorTip("系统错误，配置文件有错，加载失败，请检查配置文件格式");
                            }
                        }
                        else
                        {
                            ShowErrorTip(string.Format("系统错误，当前选择的配置文件后缀名不正确"));
                        }
                    }
                    else
                    {
                        ShowErrorTip("系统异常");
                    }
                }
                else
                {
                    ShowErrorTip("系统错误，请选择一个配置文件");
                }
            }
            else
            {
                ShowErrorTip("请选择一个配置文件");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var pwd = string.Empty;
            if (!this.InputPasswordDialog(ref pwd))
                return;
            if (pwd == "098765")
                ShowSuccessTip("密码正确");
            else
            {
                ShowErrorTip("密码错误");
                return;
            }

            if (!string.IsNullOrEmpty(uiComboDataGridView1.Text) && _selectDtRowIndex != -1 && _dt.Rows.Count > _selectDtRowIndex)
            {
                if (uiComboDataGridView1.Text == _dt.Rows[_selectDtRowIndex]["名称"].ToString())
                {
                    var filePath = _dt.Rows[_selectDtRowIndex]["路径"].ToString();

                    if (filePath.Contains(uiComboDataGridView1.Text))
                    {
                        if (ShowAskDialog(string.Format("密码输入正确，确认要删除文件：\r\n{0}", filePath)))
                        {
                            if (File.Exists(filePath))
                                File.Delete(filePath);

                            MessageBox.Show(string.Format(@"文件【{0}】已删除！", filePath));
                        }
                        else
                        {
                            ShowInfoTip("删除操作已取消");
                        }
                    }
                    else
                    {
                        ShowErrorTip("系统异常");
                    }
                }
                else
                {
                    ShowErrorTip("系统错误，请选择一个配置文件");
                }
            }
            else
            {
                ShowErrorTip("请选择一个配置文件");
            }
        }

        private void btnShowOnLineFile_Click(object sender, EventArgs e)
        {
            ShowOnLineFile();
        }

        private void FileBinding(string folder, bool isClear)
        {
            if (isClear)
                _dt.Rows.Clear();

            uiComboDataGridView1.Text = string.Empty;

            if (string.IsNullOrEmpty(folder))
                return;

            if (!Directory.Exists(folder))
                return;

            foreach (
                var f in Directory.GetFiles(folder).Where(f => f.EndsWith(@".xml") || f.EndsWith(@".ccd")))
            {
                try
                {
                    var type = "流程图";
                    if (f.EndsWith(".ccd"))
                    {
                        type = "CCD";
                    }

                    var fInfo = new FileInfo(f);
                    _dt.Rows.Add(fInfo.NameWithoutExt(), new FileInfo(f).Name, fInfo.FullName, type);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            if (_dt.Rows.Count > 0)
            {
                uiComboDataGridView1.Text = _dt.Rows[0]["名称"].ToString();
                _selectDtRowIndex = 0;
            }
        }

        private void ShowOnLineFile()
        {
            string workOrderNo;
            string pnNo;
            string filePath;
            string fileName;
            var errorCode = string.Empty;
            if (SynchronizationParameters.GetWorkOrderNoByApi(
                    Program.DeviceNo, out workOrderNo, out pnNo, out filePath, out fileName, ref errorCode))
            {
                try
                {
                    var driveLetter = Path.GetPathRoot(Program.SysDir);
                    var wordOrderTemp = string.Format(@"{0}\{1}", driveLetter, "WorkOrderFileTemp");
                    if (!Directory.Exists(wordOrderTemp))
                        Directory.CreateDirectory(wordOrderTemp);

                    try
                    {
                        // 使用DirectoryInfo类获取目录信息
                        var directoryInfo = new DirectoryInfo(wordOrderTemp);

                        // 获取所有文件信息并排序
                        var files = directoryInfo.GetDirectories()
                            .OrderBy(f => f.CreationTime) // 按创建日期升序排序
                            .ToArray(); // 或者使用OrderByDescending(f => f.CreationTime)进行降序排序

                        if (files.Any() && files.Length > 500)
                        {
                            var firstFile = files[0];
                            firstFile.Delete(true);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    var tempFolder = string.Format(@"{0}\{1}_{2}_{3}", wordOrderTemp, pnNo, workOrderNo,
                    HighPrecisionTimer.GetTimestamp());

                    var client = new RestClient(filePath) { ReadWriteTimeout = 500, Timeout = 500 };
                    var request = new RestRequest(Method.GET);

                    // 执行请求以获取文件内容
                    var fileBytes = client.DownloadData(request);

                    var extensionIndex = filePath.LastIndexOf('.');
                    if (extensionIndex != -1)
                    {
                        var extensionName = filePath.Substring(extensionIndex, filePath.Length - extensionIndex);

                        var tempFilePath = string.Format(@"{0}\{1}", tempFolder, fileName + extensionName);

                        Directory.CreateDirectory(tempFolder);

                        // 将文件内容写入本地文件
                        File.WriteAllBytes(tempFilePath, fileBytes);

                        FileBinding(tempFolder, true);

                        lblCurrentMode.Text = @"当前为在线模式：" + @pnNo + "_" + workOrderNo;
                        ShowSuccessTip(string.Format("获取工单成功：\r\n工单={0}；\r\n工序={1}；\r\n文件名={2}", workOrderNo, pnNo, fileName + extensionName));
                    }
                    else
                    {
                        ShowErrorTip(string.Format("获取工单文件失败：{0}", "取到的文件无扩展名"));
                    }
                }
                catch (Exception e)
                {
                    lblCurrentMode.Text = @"当前未获取到工单，请重新获取或选择离线模式";
                    ShowErrorTip(string.Format("获取工单失败：{0}", e.Message));
                }
            }
            else
            {
                FileBinding(string.Empty, true);
                lblCurrentMode.Text = @"当前未获取到工单，请重新获取或选择离线模式";
                ShowErrorTip(string.IsNullOrEmpty(errorCode) ? "获取工单失败！" : string.Format("获取工单失败：【{0}】", errorCode));
            }
        }

        private void ShowOffLineFile(bool isNeedAccess = true)
        {
            var showFileAction = new Action<DateTime, DateTime>((start, end) =>
            {
                Invoke(new Action(() =>
                {
                    FileBinding(SystemXmlFolder, true);
                    FileBinding(SystemCcdFolder, false);
                    if (!isNeedAccess)
                        lblCurrentMode.Text = @"当前为离线模式";
                    else
                        lblCurrentMode.Text = @"当前为离线模式,有效期：" + start + @"~" + end;
                }));
            });

            DateTime startTime;
            DateTime endTime;

            var showAuthorityAction = new Action(() =>
            {
                Invoke(new Action(() =>
                {
                    ShowInfoTip("该设备当前时段不允许使用离线文件，请输入管理密码进行确认");
                    if (ShowOffLineAuthority(out startTime, out endTime))
                    {
                        var str1 = startTime.ToString(CultureInfo.InvariantCulture);
                        str1 = CommonUtility.DEncrypt.DEncrypt.Encrypt(str1);

                        var str2 = endTime.ToString(CultureInfo.InvariantCulture);
                        str2 = CommonUtility.DEncrypt.DEncrypt.Encrypt(str2);

                        Program.OffLineStartTime = str1;
                        Program.OffLineEndTime = str2;

                        Program.SysSetup.IniWriteValue("System", "OffLineStartTime", str1);
                        Program.SysSetup.IniWriteValue("System", "OffLineEndTime", str2);

                        showFileAction(startTime, endTime);
                        ShowSuccessTip(string.Format("管理员权限获取成功，离线文件可用时间段{0}~{1}", startTime, endTime));
                    }
                    else
                    {
                        ShowErrorTip("管理员权限获取失败，无法加载本地文件");
                    }
                }));
            });

            if (!isNeedAccess)
            {
                showFileAction(DateTime.Now, DateTime.Now);
            }
            else
            {
                if (DateTime.TryParse(CommonUtility.DEncrypt.DEncrypt.Decrypt(Program.OffLineStartTime), out startTime) &&
                    DateTime.TryParse(CommonUtility.DEncrypt.DEncrypt.Decrypt(Program.OffLineEndTime), out endTime))
                {
                    var currentTime = DateTime.Now;
                    if (currentTime >= startTime && currentTime <= endTime)
                    {
                        showFileAction(startTime, endTime);
                        ShowSuccessTip(string.Format("已显示离线文件，离线文件可用时间段{0}~{1}", startTime, endTime));
                    }
                    else
                    {
                        showAuthorityAction();
                    }
                }
                else
                {
                    startTime = DateTime.Parse("1970/01/01 23:00:00");
                    endTime = DateTime.Parse("1971/01/01 23:00:00");
                    showAuthorityAction();
                }
            }
        }

        private bool ShowOffLineAuthority(out DateTime startTime, out DateTime endTime)
        {
            startTime = DateTime.Parse(string.Format("{0} 00:00:00", DateTime.Now.ToString("yyyy/MM/dd")));
            endTime = DateTime.Parse(string.Format("{0} 23:59:59", DateTime.Now.ToString("yyyy/MM/dd")));

            var pwd = string.Empty;
            if (this.InputPasswordDialog(ref pwd))
            {
                if (pwd.ToLower() != "098765")
                {
                    ShowInfoTip("密码输入错误");
                    return false;
                }
            }
            else
            {
                ShowInfoTip("取消密码输入");
                return false;
            }

            return true;
        }

        private void btnShowOffLineFile_Click(object sender, EventArgs e)
        {
            ShowOffLineFile(isNeedAccess: !Program.IsOfflineMode);
        }

        private void btnEditDeviceNo_Click(object sender, EventArgs e)
        {
            var pwd = string.Empty;
            if (this.InputPasswordDialog(ref pwd))
            {
                if (pwd.ToLower() != "098765")
                {
                    ShowInfoTip("密码输入错误");
                    return;
                }
            }
            else
            {
                ShowInfoTip("取消密码输入");
                return;
            }

            var opt = new UIEditOption { Text = "修改设备号" };
            opt.AddText("DeviceNo", "设备号", Program.DeviceNo, true);

            var frm = new UIEditForm(opt);
            frm.CheckedData += (frmSender, frmE) =>
            {
                var toCheckValue = frmE.Form["DeviceNo"].ToString();
                if (toCheckValue.StartsWith("IN") && toCheckValue.Length == 12)
                {
                    return true;
                }

                ShowErrorTip("请以IN开头且总长度为12位");
                return false;
            };
            frm.Render();
            frm.ShowDialog();
            if (frm.IsOK)
            {
                var txt = frm["DeviceNo"].ToString();
                Program.DeviceNo = txt;
                Program.SysSetup.IniWriteValue("System", "DeviceNo", txt);

                try
                {
                    if (VisionCommon.VisionDeviceConfig != null)
                    {
                        VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo = txt;
                        VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        XmlHelper.SerializeToFile(VisionCommon.VisionDeviceConfig, Program.SysDir + @"\图像检测配置文件\CCD_DeviceConfig.CcdDeviceConfig", Encoding.UTF8);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

                uiLine5.Text = txt;
            }
            else
            {
                ShowInfoTip("操作已取消！");
            }
        }
    }
}
