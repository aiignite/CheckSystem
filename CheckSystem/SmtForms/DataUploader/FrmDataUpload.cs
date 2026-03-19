using CommonUtility.DEncrypt;
using Go;
using HZH_Controls.IconFont;
using Newtonsoft.Json;
using RestSharp;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.SmtForms.DataUploader
{
    public partial class FrmDataUpload : UIForm
    {
        private static shared_strand _strand;
        private generator _timeAction;

        private readonly List<UploadConfigModel> _config = new List<UploadConfigModel>();
        private readonly List<MyQueueEvent> _myQueueEvents = new List<MyQueueEvent>();
        private int delayMs = 0;

        public FrmDataUpload()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(
               FontIcons.E_icon_folder_upload, 32,
               Color.DodgerBlue);
            Load += FrmDataUpload_Load;
            Closed += FrmDataUpload_Closed;
            toolStripButton5.Items.Add("0.01");
            toolStripButton5.Items.Add("0.1");
            toolStripButton5.Items.Add("0.5");
            toolStripButton5.Items.Add("2.5");
            toolStripButton5.Items.Add("5");
            toolStripButton5.Items.Add("10");
            toolStripButton5.Items.Add("20");
            toolStripButton5.Items.Add("30");
            toolStripButton5.Items.Add("40");
            toolStripButton5.Items.Add("50");
            toolStripButton5.Items.Add("60");
            toolStripButton5.SelectedIndex = 0;
            toolStripButton5.SelectedIndexChanged += toolStripButton5_SelectedIndexChanged;
            //Console.WriteLine((int)(double.Parse(toolStripButton5.Text) * 1000));
        }

        private void toolStripButton5_SelectedIndexChanged(object sender, EventArgs e)
        {
            delayMs = (int)(double.Parse(toolStripButton5.Text) * 1000);
        }

        private void FrmDataUpload_Closed(object sender, EventArgs e)
        {
            if (_timeAction != null)
                _timeAction.stop();

            if (_strand != null)
                _strand.release_work();
        }

        private async void FrmDataUpload_Load(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                var work = new work_service();
                _strand = new work_strand(work);
                _timeAction = generator.tgo(_strand, MainWork);
                work.run();
            });
        }

        private Task InitConfig()
        {
            return functional.init(() =>
            {
                var task = new Task(() =>
                {
                    Invoke(new Action(() =>
                    {
                        var chLineName = new ColumnHeader();
                        var chFolder = new ColumnHeader();
                        var chType = new ColumnHeader();
                        var chState = new ColumnHeader();

                        chLineName.TextAlign = HorizontalAlignment.Center;
                        chLineName.Width = 150;
                        chLineName.Text = @"名称";
                        configList.Columns.Add(chLineName);

                        chFolder.TextAlign = HorizontalAlignment.Center;
                        chFolder.Width = 500;
                        chFolder.Text = @"路径";
                        configList.Columns.Add(chFolder);

                        chType.Width = 100;
                        chType.Text = @"类型";
                        configList.Columns.Add(chType);

                        chState.Width = 100;
                        chState.Text = @"状态";
                        configList.Columns.Add(chState);

                        var chEnd = new ColumnHeader
                        {
                            Width = 17,
                            Text = string.Empty
                        };
                        configList.Columns.Add(chEnd);

                        configList.View = View.Details;
                        configList.GridLines = true;
                    }));

                    var children = new generator.children();

                    _config.AddRange(DataUploaderSqlHelper.GetConfigModels());
                    //Console.WriteLine(_config);

                    foreach (var t in _config)
                    {
                        var t1 = t;
                        var findIndex = _myQueueEvents.FindIndex(f => f.Config.LineName == t1.LineName);
                        if (findIndex == -1)
                        {
                            var lvi = new ListViewItem { Text = t.LineName };
                            var t2 = t;

                            lvi.SubItems.Add(t2.Type == "KouYoungAOI" ? DEncrypt.Encrypt(t.Folder) : t.Folder);
                            lvi.SubItems.Add(t.Type);

                            MyQueueEvent newEvent = null;
                            if (t2.Type != "KouYoungAOI")
                            {
                                if (t2.Type == "PanasonicNpmDat")
                                {
                                    //if (Directory.Exists(t2.Folder))
                                    newEvent = new MyQueueEvent(t2.Folder, t2, true);
                                }
                                else if (t2.Type == "CheckData")
                                {
                                    newEvent = new MyQueueEvent("CheckData", t2, 1);
                                }
                                else
                                {
                                    //if (Directory.Exists(t2.Folder))
                                    newEvent = new MyQueueEvent(t2.Folder, t2);
                                }
                            }
                            else if (t2.Type == "KouYoungAOI")
                            {
                                var path = t2.Folder.Split(';');
                                var server = path[0].Split('=')[1];
                                var uid = path[1].Split('=')[1];
                                var pwd = path[2].Split('=')[1];

                                newEvent = new MyQueueEvent(server, uid, pwd, t2);
                            }

                            if (newEvent != null)
                            {
                                newEvent.PushMsgEvent += newEvent_PushMsgEvent;
                                newEvent.PushPanasonicMsgEvent += newEvent_PushPanasonicMsgEvent;
                                newEvent.PushKyAoiMsgEvent += newEvent_PushKyAoiMsgEvent;
                                newEvent.PushFileCreatedAoiMsgEvent += newEvent_PushFileCreatedAoiMsgEvent;
                                newEvent.PushMsgDataTransferEvent += NewEvent_PushMsgDataTransferEvent;
                                lvi.SubItems.Add("正在监控");
                                _myQueueEvents.Add(newEvent);

                                if (t2.Type == "PanasonicNpm")
                                    children.go(() => newEvent.UploadPanasonicTask());
                                else
                                    children.go(() => newEvent.UploadTask());
                            }
                            else
                            {
                                lvi.SubItems.Add("路径不存在");
                            }

                            Invoke(new Action(() =>
                            {
                                configList.Items.Add(lvi);
                            }));
                        }
                    }
                });
                task.RunSynchronously();
                return task;
            });
        }

        private void NewEvent_PushMsgDataTransferEvent(string msg)
        {
            Updatetxt(msg);
        }

        private void newEvent_PushFileCreatedAoiMsgEvent(string msg)
        {
            if (toolStripButton4.Text != @"显示文件监控事件")
            {
                Updatetxt(msg);
            }
        }

        private void newEvent_PushKyAoiMsgEvent(string msg)
        {
            if (toolStripButton3.Text != @"显示SQL监控事件")
            {
                Updatetxt(msg);
            }
        }

        private void newEvent_PushMsgEvent(MyQueueEvent.PostDetail[] postDetail)
        {
            EqueueTask(postDetail);
        }

        private void newEvent_PushPanasonicMsgEvent(MyQueueEvent.PanasonicPostDetail[] postDetail)
        {
            EqueuePanasonicNpmTask(postDetail);
        }

        private delegate void UpdatetxtDelegate(string text);
        private void Updatetxt(string text)
        {
            var updateRichTextBoxDelegate = new UpdatetxtDelegate(Updatetxt);

            if (uiTextBox4.InvokeRequired)
            {
                Invoke(updateRichTextBoxDelegate, text);
            }
            else
            {
                if (uiTextBox4.Text.Length > 10 * 2500)
                    uiTextBox4.Clear();

                uiTextBox4.AppendText(text + Environment.NewLine);
                //uiTextBox4.Text = text;
            }
        }

        private async Task MainWork()
        {
            await InitConfig();
            await RunTask();
        }

        private async Task RunTask()
        {
            var children = new generator.children();

            children.go(PostApiTask);
            children.go(PostPanasonicNpmTask);
            //foreach (var t in _myQueueEvents)
            //{
            //    var t1 = t;
            //    children.go(() => t1.UploadTask());
            //}

            await children.wait_all();
        }

        private readonly Queue<MyQueueEvent.PostDetail[]> _postApiTask = new Queue<MyQueueEvent.PostDetail[]>();
        private readonly object _locker = new object();
        private readonly EventWaitHandle _wh = new AutoResetEvent(false);

        public Task PostApiTask()
        {
            return functional.init(() =>
            {
                var task = new Task(() =>
                {
                    while (true)
                    {
                        MyQueueEvent.PostDetail[] actions = null;

                        lock (_locker)
                        {
                            if (_postApiTask.Count > 0)
                            {
                                actions = _postApiTask.Dequeue();

                                if (actions == null)
                                    return;
                            }
                        }

                        if (actions != null && actions.Any() && actions[0] != null)
                        {
                            var postApiAction = new Action<List<UploadPackage>>(toPostDatas =>
                            {
                                //var toPostDatas = (from t in actions where t != null && t.PostData != null select t.PostData).ToList();
                                var postJsonStr = JsonConvert.SerializeObject(toPostDatas);

                                var _postUrl = "http://192.168.0.136:8021/api/Other/UploadProductionCheckData";

                                var client = new RestClient(_postUrl);
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("cache-control", "no-cache");
                                request.AddParameter("application/json", postJsonStr, ParameterType.RequestBody);
                                var response = client.Execute(request);

                                var isPostOk = false;

                                if (response.StatusCode != HttpStatusCode.OK)
                                {
                                    var showMsg =
                                        string.Format(
                                            "[{0}]: Failed LineName={1}, LineDeviceNo={2}, Type={3}, FileName={4}, UploadResult={5}, DataCount={6}",
                                            DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff"), actions[0].Config.LineName,
                                            actions[0].Config.DeviceNo, actions[0].Config.Type,
                                            actions[0].NewCreatedFilePath,
                                            string.Format("{0}/{1}", response.StatusCode, response.Content),
                                            actions.Length);
                                    Updatetxt(showMsg);
                                    Thread.Sleep(2500);
                                    response = client.Execute(request);

                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        isPostOk = true;
                                    }
                                }
                                else
                                {
                                    isPostOk = true;
                                }

                                if (isPostOk && actions[0].IsNeedDeleteOld)
                                {
                                    var oldFilePath = actions[0].NewCreatedFilePath;

                                    try
                                    {
                                        var fs = new FileInfo(oldFilePath);

                                        var bkDirectPath = string.Format(@"{0}\uploadbk", fs.DirectoryName);
                                        if (!Directory.Exists(bkDirectPath))
                                            Directory.CreateDirectory(bkDirectPath);

                                        //File.Delete(oldFilePath); 
                                        // 如果目标文件存在，删除它
                                        var destinationFilePath = string.Format(@"{0}\{1}", bkDirectPath, fs.Name);
                                        if (File.Exists(destinationFilePath))
                                            File.Delete(destinationFilePath);

                                        File.Move(oldFilePath, destinationFilePath);
                                        var showMsg =
                                            string.Format(
                                                "[{0}]: DELETE OK={1}",
                                                DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff"), oldFilePath);
                                        Updatetxt(showMsg);
                                    }
                                    catch (Exception e)
                                    {
                                        // ignored
                                        var showMsg =
                                            string.Format(
                                                "[{0}]: DELETE FAILED={1}, ERROR={2}",
                                                DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff"), oldFilePath, e.Message);
                                        Updatetxt(showMsg);
                                    }
                                }

                                {
                                    var showMsg =
                                    string.Format(
                                        "[{0}]: LineName={1}, LineDeviceNo={2}, Type={3}, FileName={4}, UploadResult={5}, DataCount={6}",
                                        DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff"), actions[0].Config.LineName,
                                        actions[0].Config.DeviceNo, actions[0].Config.Type, actions[0].NewCreatedFilePath, string.Format("{0}/{1}", response.StatusCode, response.Content), actions.Length);
                                    Updatetxt(showMsg);
                                    //Thread.Sleep(2500);

                                    Thread.Sleep(delayMs);
                                }
                            });

                            if (actions[0].Config.Type == "CheckData")
                            {
                                var totalCount = actions.Length;
                                if (totalCount > 0)
                                {
                                    const int maxCount = 50;

                                    if (totalCount <= maxCount)
                                    {
                                        var toPostDatas = (from t in actions where t != null && t.PostData != null select t.PostData).ToList();
                                        postApiAction.Invoke(toPostDatas);
                                    }
                                    else
                                    {
                                        var count = totalCount / maxCount;
                                        var rest = totalCount % maxCount;

                                        var temp = (from t in actions where t != null && t.PostData != null select t.PostData).ToList();
                                        for (var i = 0; i < count; i++)
                                        {
                                            var sendTemp = new UploadPackage[maxCount];
                                            Array.Copy(temp.ToArray(), sendTemp, maxCount);
                                            temp.RemoveRange(0, maxCount);
                                            postApiAction.Invoke(sendTemp.ToList());
                                        }

                                        if (temp.Any())
                                            postApiAction.Invoke(temp);
                                    }
                                }

                                //try
                                //{
                                //    var insertHeader =
                                //        "insert into SyProductionCheckData (ItemId,MaterialNo,MaterialLotNo,MaterialUid,MaterialBarcode,MaterialCustomerBarcode,SubMaterialsInfo,DeviceNo,DeviceName,Result,CheckData,MaterialName,CreateTime,Note) values ";

                                //    var sbInsertValues = new StringBuilder();
                                //    sbInsertValues.Append(insertHeader);

                                //    for (var i = 0; i < actions.Length; i++)
                                //    {
                                //        var t = actions[i];

                                //        var guid = Guid.NewGuid().ToString();
                                //        var materialNo = t.PostData.MaterialNo;
                                //        var materialLotNo = t.PostData.MaterialLotNo;
                                //        var materialUid = t.PostData.MaterialUid;
                                //        var materialBarcode = t.PostData.MaterialBarcode;
                                //        var materialCustomerBarcode = t.PostData.MaterialCustomerBarcode;
                                //        var subMaterialsInfo = t.PostData.SubMaterialsInfo;
                                //        var deviceNo = t.PostData.DeviceNo;
                                //        var deviceName = t.PostData.DeviceName;
                                //        var result = t.PostData.Result;
                                //        var checkData = t.PostData.CheckData;
                                //        var materialName = t.PostData.MaterialName;
                                //        var createTime = t.PostData.CreateTime;
                                //        var note = t.PostData.Note;

                                //        var str = string.Format(
                                //            "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                                //            guid, materialNo, materialLotNo, materialUid, materialBarcode,
                                //            materialCustomerBarcode, subMaterialsInfo, deviceNo, deviceName, result,
                                //            checkData, materialName, createTime, note);

                                //        if (i != actions.Length - 1)
                                //            str += ",\r\n";
                                //        sbInsertValues.Append(str);
                                //    }

                                //    var returnCount = New138DbHelperSql.Update(sbInsertValues.ToString());

                                //    var isPostOk = false;
                                //    if (returnCount == actions.Length)
                                //    {
                                //        isPostOk = true;
                                //    }
                                //    else
                                //    {
                                //        returnCount = HikDbHelperSql.Update(sbInsertValues.ToString());

                                //        if (returnCount == actions.Length)
                                //        {
                                //            isPostOk = true;
                                //        }
                                //    }

                                //    var showMsg =
                                //        string.Format(
                                //            "[{0}]: LineName={1}, LineDeviceNo={2}, Type={3}, FileName={4}, UploadResult={5}",
                                //            DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff"), actions[0].Config.LineName,
                                //            actions[0].Config.DeviceNo, actions[0].Config.Type,
                                //            actions[0].NewCreatedFilePath,
                                //            string.Format("{0}/{1}", "checkDataUpload", isPostOk ? "OK" : "NG"));
                                //    Updatetxt(showMsg);
                                //    //Thread.Sleep(2500);
                                //}
                                //catch (Exception e)
                                //{
                                //    var showMsg =
                                //        string.Format(
                                //            "[{0}]: LineName={1}, LineDeviceNo={2}, Type={3}, FileName={4}, UploadResult={5}",
                                //            DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff"), actions[0].Config.LineName,
                                //            actions[0].Config.DeviceNo, actions[0].Config.Type,
                                //            actions[0].NewCreatedFilePath,
                                //            string.Format("{0}/{1}", "checkDataUpload Fail: ", e.Message));
                                //    Updatetxt(showMsg);
                                //}

                                Thread.Sleep(delayMs);
                            }
                            else
                            {
                                var toPostDatas = (from t in actions where t != null && t.PostData != null select t.PostData).ToList();
                                postApiAction.Invoke(toPostDatas);

                                //var toPostDatas = (from t in actions where t != null && t.PostData != null select t.PostData).ToList();
                                //var postJsonStr = JsonConvert.SerializeObject(toPostDatas);

                                //var _postUrl = "http://192.168.0.136:8021/api/Other/UploadProductionCheckData";

                                //var client = new RestClient(_postUrl);
                                //var request = new RestRequest(Method.POST);
                                //request.AddHeader("cache-control", "no-cache");
                                //request.AddParameter("application/json", postJsonStr, ParameterType.RequestBody);
                                //var response = client.Execute(request);

                                //var isPostOk = false;

                                //if (response.StatusCode != HttpStatusCode.OK)
                                //{
                                //    var showMsg =
                                //        string.Format(
                                //            "[{0}]: Failed LineName={1}, LineDeviceNo={2}, Type={3}, FileName={4}, UploadResult={5}",
                                //            DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff"), actions[0].Config.LineName,
                                //            actions[0].Config.DeviceNo, actions[0].Config.Type,
                                //            actions[0].NewCreatedFilePath,
                                //            string.Format("{0}/{1}", response.StatusCode, response.Content));
                                //    Updatetxt(showMsg);
                                //    Thread.Sleep(2500);
                                //    response = client.Execute(request);

                                //    if (response.StatusCode == HttpStatusCode.OK)
                                //    {
                                //        isPostOk = true;
                                //    }
                                //}
                                //else
                                //{
                                //    isPostOk = true;
                                //}

                                //if (isPostOk && actions[0].IsNeedDeleteOld)
                                //{
                                //    var oldFilePath = actions[0].NewCreatedFilePath;

                                //    try
                                //    {
                                //        var fs = new FileInfo(oldFilePath);

                                //        var bkDirectPath = string.Format(@"{0}\uploadbk", fs.DirectoryName);
                                //        if (!Directory.Exists(bkDirectPath))
                                //            Directory.CreateDirectory(bkDirectPath);

                                //        //File.Delete(oldFilePath);
                                //        // 如果目标文件存在，删除它
                                //        var destinationFilePath = string.Format(@"{0}\{1}", bkDirectPath, fs.Name);
                                //        if (File.Exists(destinationFilePath))
                                //            File.Delete(destinationFilePath);

                                //        File.Move(oldFilePath, destinationFilePath);
                                //        var showMsg =
                                //            string.Format(
                                //                "[{0}]: DELETE OK={1}",
                                //                DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff"), oldFilePath);
                                //        Updatetxt(showMsg);
                                //    }
                                //    catch (Exception e)
                                //    {
                                //        // ignored
                                //        var showMsg =
                                //            string.Format(
                                //                "[{0}]: DELETE FAILED={1}, ERROR={2}",
                                //                DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff"), oldFilePath, e.Message);
                                //        Updatetxt(showMsg);
                                //    }
                                //}

                                //{
                                //    var showMsg =
                                //    string.Format(
                                //        "[{0}]: LineName={1}, LineDeviceNo={2}, Type={3}, FileName={4}, UploadResult={5}",
                                //        DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff"), actions[0].Config.LineName,
                                //        actions[0].Config.DeviceNo, actions[0].Config.Type, actions[0].NewCreatedFilePath, string.Format("{0}/{1}", response.StatusCode, response.Content));
                                //    Updatetxt(showMsg);
                                //    //Thread.Sleep(2500);

                                //    Thread.Sleep(delayMs);
                                //}
                            }
                        }
                        else
                            _wh.WaitOne();
                    }
                });
                task.Start();
                return task;
            });
        }

        public void EqueueTask(MyQueueEvent.PostDetail[] postDetail)
        {
            lock (_locker)
                _postApiTask.Enqueue(postDetail);

            _wh.Set();
        }

        private readonly Queue<MyQueueEvent.PanasonicPostDetail[]> _postPanasonicNpmTask = new Queue<MyQueueEvent.PanasonicPostDetail[]>();
        private readonly object _panasoniclocker = new object();
        private readonly EventWaitHandle _panasonicWh = new AutoResetEvent(false);

        public Task PostPanasonicNpmTask()
        {
            return functional.init(() =>
            {
                var task = new Task(() =>
                {
                    while (true)
                    {
                        MyQueueEvent.PanasonicPostDetail[] actions = null;

                        lock (_panasoniclocker)
                        {
                            if (_postPanasonicNpmTask.Count > 0)
                            {
                                actions = _postPanasonicNpmTask.Dequeue();

                                if (actions == null)
                                    return;
                            }
                        }

                        if (actions != null && actions.Any() && actions[0] != null)
                        {
                            var toPostDatas = actions.Where(t => t != null).ToList();//actions.ToList();//(from t in actions where t != null && t.PostData != null select t.PostData).ToList();
                            var postJsonStr = JsonConvert.SerializeObject(toPostDatas);

                            const string postUrl = "http://192.168.0.136:8021/api/Other/UpdateMaterialGroupBarcode";

                            var client = new RestClient(postUrl);
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("cache-control", "no-cache");
                            request.AddParameter("application/json", postJsonStr, ParameterType.RequestBody);
                            var response = client.Execute(request);

                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                var showMsg =
                                    string.Format(
                                        "[{0}]: Failed LineName={1}, LineDeviceNo={2}, Type={3}, FileName={4}, UploadResult={5}",
                                        DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff"), actions[0].LotNameLane1,
                                        actions[0].DEVICE_SN, "PanasonicNpm",
                                        actions[0].LotNameLane1,
                                        string.Format("{0}/{1}", response.StatusCode, response.Content));
                                Updatetxt(showMsg);
                                Thread.Sleep(2500);
                                response = client.Execute(request);
                            }

                            {
                                var showMsg =
                                    string.Format(
                                        "[{0}]: LineName={1}, LineDeviceNo={2}, Type={3}, FileName={4}, UploadResult={5}",
                                        DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff"), actions[0].LotNameLane1,
                                        actions[0].DEVICE_SN, "PanasonicNpm", actions[0].LotNameLane1,
                                        string.Format("{0}/{1}", response.StatusCode, response.Content));
                                Updatetxt(showMsg);
                                //Thread.Sleep(2500);

                                Thread.Sleep(delayMs);
                            }
                        }
                        else
                            _panasonicWh.WaitOne();
                    }
                });
                task.Start();
                return task;
            });
        }

        public void EqueuePanasonicNpmTask(MyQueueEvent.PanasonicPostDetail[] postDetail)
        {
            lock (_panasoniclocker)
                _postPanasonicNpmTask.Enqueue(postDetail);

            _panasonicWh.Set();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            uiTextBox4.Clear();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (toolStripButton3.Text == @"显示SQL监控事件")
            {
                toolStripButton3.Text = @"隐藏SQL监控事件";
            }
            else
            {
                toolStripButton3.Text = @"显示SQL监控事件";
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            // 显示文件监控事件
            if (toolStripButton4.Text == @"显示文件监控事件")
            {
                toolStripButton4.Text = @"隐藏文件监控事件";
            }
            else
            {
                toolStripButton4.Text = @"显示文件监控事件";
            }
        }
    }
}
