using CheckSystem.MaterialHelperForms.MaterialModels;
using CommonUtility;
using Go;
using MiniExcelLibs;
using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Delegates;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using ErrorEventHandler = TableDependency.SqlClient.Base.Delegates.ErrorEventHandler;

namespace CheckSystem.SmtForms.DataUploader
{
    public class MyQueueEvent
    {
        public delegate void PushMsgDelegate(PostDetail[] postDetail);
        public event PushMsgDelegate PushMsgEvent;

        public delegate void PushMsgKyAoiDelegate(string msg);
        public event PushMsgKyAoiDelegate PushKyAoiMsgEvent;

        public delegate void PushMsgFileCreatedAoiDelegate(string msg);
        public event PushMsgFileCreatedAoiDelegate PushFileCreatedAoiMsgEvent;

        public delegate void PushMsgDataTransferDelegate(string msg);
        public event PushMsgDataTransferDelegate PushMsgDataTransferEvent;

        public UploadConfigModel Config;
        private readonly Queue<Func<PostDetail[]>> _task = new Queue<Func<PostDetail[]>>();
        private readonly object _locker = new object();
        private readonly EventWaitHandle _wh = new AutoResetEvent(false);

        private FileSystemWatcher _fileSystemWatcher;
        private bool _isFileWatching;
        private readonly bool _isIncludeSubdirectories;

        private readonly Dictionary<int, SqlTableDependency<KouYoungAoi_TB_AOIResult>> _sqlTableDependency =
            new Dictionary<int, SqlTableDependency<KouYoungAoi_TB_AOIResult>>();
        private readonly Dictionary<int, ChangedEventHandler<KouYoungAoi_TB_AOIResult>> _sqHandlers =
            new Dictionary<int, ChangedEventHandler<KouYoungAoi_TB_AOIResult>>();
        private readonly Dictionary<int, ErrorEventHandler>
            _sqlErrorHandlers = new Dictionary<int, ErrorEventHandler>();
        private readonly object _lockSqlTableDependency = new object();
        private readonly string _server = string.Empty;
        private readonly string _uid = string.Empty;
        private readonly string _pwd = string.Empty;
        private readonly Dictionary<int, bool> _weekMonitorState = new Dictionary<int, bool>();

        public MyQueueEvent(string filePath, UploadConfigModel config, bool isIncludeSubdirectories = false)
        {
            Config = config;
            _isIncludeSubdirectories = isIncludeSubdirectories;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        if (Directory.Exists(filePath))
                        {
                            _fileSystemWatcher = new FileSystemWatcher(filePath)
                            {
                                EnableRaisingEvents = true,
                                IncludeSubdirectories = isIncludeSubdirectories
                            };
                            _fileSystemWatcher.Created += FileSystemWatcher_Created;
                            _fileSystemWatcher.Changed += fileSystemWatcher_Changed;
                            _fileSystemWatcher.Error += FileSystemWatcher_Error;
                            _isFileWatching = true;
                            break;
                        }

                        Thread.Sleep(50);
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(50);
                    }
                }
            });

            if (string.Equals(config.Type, "ICT", StringComparison.CurrentCultureIgnoreCase))
            {
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        try
                        {
                            var dir = new DirectoryInfo(filePath);
                            var files = dir.GetFiles();
                            foreach (var f in files)
                            {
                                if (f.FullName.ToLower().EndsWith(".csv"))
                                {
                                    var fileInfo = new FileInfo(f.FullName);
                                    var fileCreateTime = fileInfo.LastWriteTime;

                                    if (fileCreateTime < DateTime.Now.AddMinutes(-60))
                                    {
                                        EqueueTask(IctConvert(f.FullName));
                                    }
                                }
                            }

                            Thread.Sleep(1000 * 60 * 60);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                });
            }
        }

        public MyQueueEvent(string server, string uid, string pwd, UploadConfigModel config)
        {
            //server = "127.0.0.1";
            //uid = "sa";
            //pwd = "123456";

            Config = config;
            _server = server;
            _uid = uid;
            _pwd = pwd;

            for (var i = 0; i <= 65; i++)
            {
                var dt = DateTime.Now;
                var weeks = dt.DayOfYear / 7 + 1;

                _sqlTableDependency.Add(i, null);
                var i1 = i;
                ChangedEventHandler<KouYoungAoi_TB_AOIResult> handle = (sender, e) => _sqlTableDependency_OnChanged(sender, e, string.Format("KY_Result_{0}{1}", dt.Year, i1.ToString().PadLeft(2, '0')));
                _sqHandlers.Add(i, handle);

                var i2 = i;
                ErrorEventHandler errorHandle = (sender, e) => _sqlTableDependency_OnError(sender, e, i2);
                _sqlErrorHandlers.Add(i, errorHandle);

                _weekMonitorState.Add(i, i < weeks);
            }

            var task = new Task(action: () =>
            {
                while (true)
                {
                    Thread.Sleep(1000);

                    var notMonitoringWeeks = _weekMonitorState.Where(kvp => kvp.Value == false).ToList();

                    foreach (var t in notMonitoringWeeks)
                    {
                        var dt = DateTime.Now;
                        var weeks = dt.DayOfYear / 7 + 1;

                        if (t.Key > weeks)
                            continue;

                        var sqlDb = string.Format("KY_Result_{0}{1}", dt.Year, t.Key.ToString().PadLeft(2, '0'));

                        try
                        {
                            var con = string.Format("server={0};database={1};uid={2};pwd={3}", server, sqlDb, uid, pwd);
                            const string tableName = "TB_AOIResult";

                            var connection =
                                new SqlConnection(string.Format("server={0};uid={1};pwd={2}", _server, _uid, _pwd));
                            var isDbHave = KouYoungAoi_TB_AOIResult.CheckDatabaseExists(connection, sqlDb);
                            if (isDbHave)
                            {
                                lock (_lockSqlTableDependency)
                                {
                                    if (_sqlTableDependency[t.Key] != null && _sqHandlers[t.Key] != null)
                                    {
                                        try
                                        {

                                            _sqlTableDependency[t.Key].OnChanged -= _sqHandlers[t.Key];
                                            _sqlTableDependency[t.Key].OnError -= _sqlErrorHandlers[t.Key];
                                            _sqlTableDependency[t.Key].Stop();
                                            _sqlTableDependency[t.Key].Dispose();
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e);
                                        }
                                    }

                                    _sqlTableDependency[t.Key] = new SqlTableDependency<KouYoungAoi_TB_AOIResult>(con, tableName);
                                    _sqlTableDependency[t.Key].OnChanged += _sqHandlers[t.Key];
                                    _sqlTableDependency[t.Key].OnError += _sqlErrorHandlers[t.Key];
                                    _sqlTableDependency[t.Key].Start();

                                    OnPushKyAoiMsgEvent(sqlDb + " start monitor");
                                    _weekMonitorState[t.Key] = true;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }
            });

            task.Start();
        }

        public MyQueueEvent(string type, UploadConfigModel config, int isCheckData)
        {
            Config = config;

            if (type == "CheckData")
            {
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        try
                        {
                            const int minutes = 5;
                            var endTime = DateTime.Now;
                            var startTime = endTime.AddMinutes(-minutes);
                            startTime = startTime.AddSeconds(1);
                            EqueueTask(CheckDataTransfer(startTime, endTime));

                            Thread.Sleep(minutes * 60 * 1000);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                });
            }
        }

        private void _sqlTableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e, int week)
        {
            lock (_lockSqlTableDependency)
            {
                _weekMonitorState[week] = false;
            }
        }

        private void _sqlTableDependency_OnChanged(object sender, RecordChangedEventArgs<KouYoungAoi_TB_AOIResult> e, string dataBase)
        {
            //if (e.ChangeType == ChangeType.Insert)
            //{
            //    EqueueTask(KouYoungAoiConvert(e.Entity));
            //}

            lock (_lockSqlTableDependency)
            {
                OnPushKyAoiMsgEvent(_server + " " + dataBase + " monitor " + e.ChangeType);

                if (e.ChangeType == ChangeType.Update &&
                    !string.IsNullOrEmpty(e.Entity.ReviewEndDateTime))
                    EqueueTask(KouYoungAoiConvert(e.Entity, dataBase));
            }
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            var fileSystemWatcher = sender as FileSystemWatcher;

            if (fileSystemWatcher != null)
            {
                if (fileSystemWatcher.Path == Config.Folder)
                {
                    var value = string.Format(@"{0}: created file = {1}, config type = {2}", Config.LineName, e.Name, Config.Type);
                    OnPushFileCreatedMsgEvent(value);

                    if (Config.Type == "ParmiSpi" && e.FullPath.ToLower().EndsWith(".csv"))
                    {
                        EqueueTask(ParmiSpiConvert(e.FullPath));
                    }
                    else if (Config.Type == "TriAOI" && e.FullPath.ToLower().EndsWith(".txt") && e.Name.ToLower().Contains("_FovComp_".ToLower()))
                    {
                        EqueueTask(TriAoiConvert(e.FullPath));
                        OnPushFileCreatedMsgEvent(string.Format(@"{0}: EqueueTask = {1}, config type = {2}", Config.LineName, e.Name, Config.Type));
                    }
                    else if (Config.Type == "HollyAOI" && e.FullPath.ToLower().EndsWith(".txt"))
                    {
                        EqueueTask(HollyAoiConvert(e.FullPath));
                    }
                    else if (Config.Type == "PanasonicNpm" && e.FullPath.ToLower().EndsWith(".tmp") && e.Name.StartsWith("10"))
                    {
                        EqueuePanasonicTask(PanasonicNpmConvert(e.FullPath));
                    }
                    else if (Config.Type == "ICT" && e.FullPath.ToLower().EndsWith(".csv"))
                    {
                        EqueueTask(IctConvert(e.FullPath));
                    }
                    else if (Config.Type == "PanasonicNpmDat" && e.FullPath.ToLower().EndsWith(".dat"))
                    {
                        var fileInfo = new FileInfo(e.FullPath);
                        if (fileInfo.Directory != null && fileInfo.Directory.Name.ToLower() == "plot".ToLower())
                            EqueueTask(PanasonicNpmDatConvert(e.FullPath));
                    }
                }
            }
        }

        private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            var value = string.Format(@"{0}: changed file = {1}, config type = {2}", Config.LineName, e.Name, Config.Type);
            OnPushFileCreatedMsgEvent(value);

            if (Config.Type == "Aurote")
            {
                if (e.FullPath.ToLower().EndsWith(".csv"))
                {
                    EqueueTask(AuroteConvert(e.FullPath));
                }
                else if (e.FullPath.ToLower().EndsWith(".tmp"))
                {
                    try
                    {
                        var fileInfo = new FileInfo(e.FullPath);
                        var dir = fileInfo.Directory;

                        if (dir != null)
                        {
                            var files = dir.GetFiles().ToList();

                            var file = files.Find(f =>
                                e.FullPath.Contains(f.Name.TrimEnd(new char[] { '.', 'c', 's', 'v' })));

                            if (file != null)
                                EqueueTask(AuroteConvert(file.FullName));
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }
            }
        }

        private void FileSystemWatcher_Error(object sender, System.IO.ErrorEventArgs e)
        {
            var fileSystemWatcher = sender as FileSystemWatcher;

            if (fileSystemWatcher != null)
            {
                if (_isFileWatching)
                {
                    _isFileWatching = false;

                    if (_fileSystemWatcher != null)
                    {
                        _fileSystemWatcher.Created -= FileSystemWatcher_Created;
                        _fileSystemWatcher.Changed -= fileSystemWatcher_Changed;
                        _fileSystemWatcher.Error -= FileSystemWatcher_Error;
                    }

                    Task.Factory.StartNew(() =>
                    {
                        while (true)
                        {
                            try
                            {
                                if (Directory.Exists(fileSystemWatcher.Path))
                                {
                                    _fileSystemWatcher = new FileSystemWatcher(fileSystemWatcher.Path)
                                    {
                                        EnableRaisingEvents = true,
                                        IncludeSubdirectories = _isIncludeSubdirectories
                                    };
                                    _fileSystemWatcher.Created += FileSystemWatcher_Created;
                                    _fileSystemWatcher.Changed += fileSystemWatcher_Changed;
                                    _fileSystemWatcher.Error += FileSystemWatcher_Error;
                                    _isFileWatching = true;
                                    break;
                                }

                                Thread.Sleep(50);
                            }
                            catch (Exception ex)
                            {
                                Thread.Sleep(50);
                            }
                        }
                    });
                }
            }
        }

        public Task UploadTask()
        {
            return functional.init(() =>
            {
                var task = new Task(() =>
                {
                    while (true)
                    {
                        Func<PostDetail[]> action = null;

                        lock (_locker)
                        {
                            if (_task.Count > 0)
                            {
                                action = _task.Dequeue();

                                if (action == null)
                                    return;
                            }
                        }

                        if (action != null)
                        {
                            var postDetail = action.Invoke();
                            if (postDetail != null)
                                OnPushMsgEvent(postDetail);
                        }
                        else
                            _wh.WaitOne();
                    }
                });
                task.Start();
                return task;
            });
        }

        public void EqueueTask(Func<PostDetail[]> action)
        {
            lock (_locker)
                _task.Enqueue(action);

            _wh.Set();
        }

        private void OnPushMsgEvent(PostDetail[] postDetail)
        {
            try
            {
                var handler = PushMsgEvent;
                if (handler != null) handler(postDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnPushKyAoiMsgEvent(string msg)
        {
            try
            {
                var handler = PushKyAoiMsgEvent;
                if (handler != null) handler(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnPushFileCreatedMsgEvent(string msg)
        {
            try
            {
                var handler = PushFileCreatedAoiMsgEvent;
                if (handler != null) handler(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnPushMsgDataTransferEvent(string msg)
        {
            try
            {
                var handler = PushMsgDataTransferEvent;
                if (handler != null) handler(msg);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// PARMI SPI
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private Func<PostDetail[]> ParmiSpiConvert(string filePath)
        {
            return () =>
            {
                Thread.Sleep(5000);

                var dataParmiSpi = new ParmiSpi();

                try
                {
                    //var excelHelper = new ExcelFileHelper();
                    //var allRows = excelHelper.ReadExcelRows(path);
                    //var str = allRows[0].A;
                    //Console.WriteLine(str);
                    //str = allRows[0].B;
                    //Console.WriteLine(str);

                    var fileInfo = new FileInfo(filePath);

                    var newFilePath = string.Format(@"{0}\SMT配置文件\{1}", Program.SysDir, fileInfo.Name);
                    if (File.Exists(newFilePath))
                        File.Delete(newFilePath);
                    File.Copy(filePath, newFilePath);

                    var rows = MiniExcel.Query(newFilePath, excelType: ExcelType.CSV).ToList();

                    File.Delete(newFilePath);

                    var infoStartIndex = rows.FindIndex(f => f.A.ToString() == "[INFO]");
                    var infoEndIndex = rows.FindIndex(f => f.A.ToString() == "[INFO_END]");

                    if (infoEndIndex - infoStartIndex == 3)
                    {
                        var info = new Info
                        {
                            LineName = rows[infoEndIndex - 1].A,
                            MachineSn = rows[infoEndIndex - 1].B,
                            MachineName = rows[infoEndIndex - 1].C,
                            OperatorId = rows[infoEndIndex - 1].D,
                        };

                        info.LineName = info.LineName.Trim();
                        info.MachineSn = info.MachineSn.Trim();
                        info.MachineName = info.MachineName.Trim();
                        info.OperatorId = info.OperatorId.Trim();

                        dataParmiSpi.Info = info;
                    }

                    var panelInspResultStartIndex = rows.FindIndex(f => f.A.ToString() == "[PANEL_INSP_RESULT]");
                    var panelInspResultEndIndex = rows.FindIndex(f => f.A.ToString() == "[PANEL_INSP_RESULT_END]");

                    if (panelInspResultEndIndex - panelInspResultStartIndex == 3)
                    {
                        var panelInspResult = new PanelInspResult
                        {
                            ModelName = rows[panelInspResultEndIndex - 1].A,
                            ModelCode = rows[panelInspResultEndIndex - 1].B,
                            PanelSide = rows[panelInspResultEndIndex - 1].C,
                            Index = rows[panelInspResultEndIndex - 1].D,
                            Barcode = rows[panelInspResultEndIndex - 1].E,
                            Date = rows[panelInspResultEndIndex - 1].F,
                            StartTime = rows[panelInspResultEndIndex - 1].G,
                            EndTime = rows[panelInspResultEndIndex - 1].H,
                            DefectName = rows[panelInspResultEndIndex - 1].I,
                            DefectCode = rows[panelInspResultEndIndex - 1].J,
                            Result = rows[panelInspResultEndIndex - 1].K,
                        };

                        panelInspResult.ModelName = panelInspResult.ModelName.Trim();
                        panelInspResult.ModelCode = panelInspResult.ModelCode.Trim();
                        panelInspResult.PanelSide = panelInspResult.PanelSide.Trim();
                        panelInspResult.Index = panelInspResult.Index.Trim();
                        panelInspResult.Barcode = panelInspResult.Barcode.Trim();
                        panelInspResult.Date = panelInspResult.Date.Trim();
                        panelInspResult.StartTime = panelInspResult.StartTime.Trim();
                        panelInspResult.EndTime = panelInspResult.EndTime.Trim();
                        panelInspResult.DefectName = panelInspResult.DefectName.Trim();
                        panelInspResult.DefectCode = panelInspResult.DefectCode.Trim();
                        panelInspResult.Result = panelInspResult.Result.Trim();

                        dataParmiSpi.PanelInspResult = panelInspResult;
                    }

                    var boardInspResultStartIndex = rows.FindIndex(f => f.A.ToString() == "[BOARD_INSP_RESULT]");
                    var boardInspResultEndIndex = rows.FindIndex(f => f.A.ToString() == "[BOARD_INSP_RESULT_END]");
                    var tempdataParmiSpi = new List<BoardInspResult>();

                    if (boardInspResultEndIndex - boardInspResultStartIndex >= 3)
                    {
                        for (var i = boardInspResultStartIndex + 1 + 1; i < boardInspResultEndIndex; i++)
                        {
                            var boardInspResult = new BoardInspResult
                            {
                                BoardNo = rows[i].A,
                                Barcode = rows[i].B,
                                DefectCode = rows[i].C,
                                DefectName = rows[i].D,
                                Badmark = rows[i].E,
                                Result = rows[i].F,
                            };

                            boardInspResult.BoardNo = boardInspResult.BoardNo.Trim();
                            boardInspResult.Barcode = boardInspResult.Barcode.Trim();
                            boardInspResult.DefectCode = boardInspResult.DefectCode.Trim();
                            boardInspResult.DefectName = boardInspResult.DefectName.Trim();
                            boardInspResult.Badmark = boardInspResult.Badmark.Trim();
                            boardInspResult.Result = boardInspResult.Result.Trim();

                            tempdataParmiSpi.Add(boardInspResult);
                        }
                    }
                    dataParmiSpi.BoardInspResult = tempdataParmiSpi.Any() ? tempdataParmiSpi.ToArray() : new BoardInspResult[0];

                    var componentInspResultStartIndex = rows.FindIndex(f => f.A.ToString() == "[COMPONENT_INSP_RESULT]");
                    var componentInspResultEndIndex = rows.FindIndex(f => f.A.ToString() == "[COMPONENT_INSP_RESULT_END]");
                    var tempDataParmiSpi = new List<ComponentInspResult>();
                    if (componentInspResultEndIndex - componentInspResultStartIndex >= 3)
                    {
                        for (var i = componentInspResultStartIndex + 1 + 1; i < componentInspResultEndIndex; i++)
                        {
                            var componentInspResult = new ComponentInspResult
                            {
                                BoardNo = rows[i].A,
                                LocationName = rows[i].B,
                                PinNumber = rows[i].C,
                                PosX = rows[i].D,
                                PosY = rows[i].E,
                                DefectName = rows[i].F,
                                DefectCode = rows[i].G,
                                Result = rows[i].H,
                            };

                            componentInspResult.BoardNo = componentInspResult.BoardNo.Trim();
                            componentInspResult.LocationName = componentInspResult.LocationName.Trim();
                            componentInspResult.PinNumber = componentInspResult.PinNumber.Trim();
                            componentInspResult.PosX = componentInspResult.PosX.Trim();
                            componentInspResult.PosY = componentInspResult.PosY.Trim();
                            componentInspResult.DefectName = componentInspResult.DefectName.Trim();
                            componentInspResult.DefectCode = componentInspResult.DefectCode.Trim();
                            componentInspResult.Result = componentInspResult.Result.Trim();

                            tempDataParmiSpi.Add(componentInspResult);
                        }
                    }

                    dataParmiSpi.ComponentInspResult = tempDataParmiSpi.Any() ? tempDataParmiSpi.ToArray() : new ComponentInspResult[0];

                    var toPostDetails = new List<PostDetail>();

                    foreach (var t in dataParmiSpi.BoardInspResult)
                    {
                        if (!string.IsNullOrEmpty(t.BoardNo))
                        {
                            // 20170613	 15:17:39 
                            var year = dataParmiSpi.PanelInspResult.Date.Substring(0, 4);
                            var month = dataParmiSpi.PanelInspResult.Date.Substring(4, 2);
                            var day = dataParmiSpi.PanelInspResult.Date.Substring(6, 2);
                            var time = dataParmiSpi.PanelInspResult.StartTime;
                            var dt = DateTime.Parse(string.Format("{0}/{1}/{2} {3}", year, month, day, time));

                            var detail = new PostDetail
                            {
                                Config = Config,
                                NewCreatedFilePath = filePath,
                            };

                            var postPackage = new UploadPackage
                            {
                                MaterialName = dataParmiSpi.PanelInspResult.ModelName,
                                MaterialUid = string.IsNullOrEmpty(t.Barcode) ? string.Empty : t.Barcode,
                                MaterialBarcode = string.IsNullOrEmpty(t.Barcode) ? string.Empty : t.Barcode,
                                MaterialCustomerBarcode = string.IsNullOrEmpty(t.Barcode) ? string.Empty : t.Barcode,
                                SubMaterialsInfo = string.IsNullOrEmpty(t.Barcode) ? string.Empty : t.Barcode,
                                DeviceNo = Config.DeviceNo,
                                CheckDateTiem = dt.ToString(CultureInfo.InvariantCulture),
                                Result = t.Result == "NG" ? "0002" : "0001",
                                Note = string.Format("{0};{1}", dataParmiSpi.PanelInspResult.DefectCode, filePath),
                            };

                            var t1 = t;
                            var findThisBoard =
                                dataParmiSpi.ComponentInspResult.ToList().FindAll(f => f.BoardNo == t1.BoardNo);

                            var listTempData = new List<ParmiSpiDataStruct>();
                            foreach (var f in findThisBoard)
                            {
                                if (string.IsNullOrEmpty(f.PinNumber) && string.IsNullOrEmpty(f.LocationName))
                                    continue;

                                var pName = string.Empty;
                                if (!string.IsNullOrEmpty(f.LocationName))
                                {
                                    pName = f.LocationName;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(f.PinNumber))
                                    {
                                        pName = string.Format("PIN_NUMBER_{0}", f.PinNumber);
                                    }
                                }

                                var tpData = new ParmiSpiDataStruct
                                {
                                    ParaName = pName,
                                    ErrorType = string.IsNullOrEmpty(f.DefectCode) ? string.Empty : f.DefectCode,
                                    Count = 1,
                                    Result = f.Result != "NG"
                                };

                                var findExist = listTempData.FindIndex(
                                    ff =>
                                        ff.ParaName.ToLower() == tpData.ParaName.ToLower() &&
                                        ff.ErrorType.ToLower() == tpData.ErrorType.ToLower() &&
                                        ff.Result == tpData.Result);
                                if (findExist == -1)
                                {
                                    listTempData.Add(tpData);
                                }
                                else
                                {
                                    listTempData[findExist].Count++;
                                }
                            }

                            var tempDetails = listTempData.Select(lt => new UploadDetail
                            {
                                ParaName = !lt.Result ? string.Format("{0}_{1}", lt.ParaName, lt.ErrorType) : lt.ParaName,
                                Range = string.Format("Boadr_No={0},Appear_Count={1}", t.BoardNo, lt.Count),
                                Result = lt.Result.ToString(),
                                Type = "SPI",
                                Value = lt.ErrorType
                            })
                                .ToList();

                            tempDetails = new List<UploadDetail>(tempDetails.OrderBy(oo => oo.Result));

                            //var tempDetails = (from f in findThisBoard
                            //                   where !string.IsNullOrEmpty(f.PinNumber)
                            //                   select new UploadDetail
                            //                   {
                            //                       Type = "SPI",
                            //                       ParaName = f.LocationName,
                            //                       Range = string.Format("{0},{1},{2}",
                            //                           string.IsNullOrEmpty(f.PinNumber) ? string.Empty : f.PinNumber,
                            //                           string.IsNullOrEmpty(f.PosX) ? string.Empty : f.PosX,
                            //                           string.IsNullOrEmpty(f.PosY) ? string.Empty : f.PosY),
                            //                       Value = string.IsNullOrEmpty(f.DefectCode) ? string.Empty : f.DefectCode,
                            //                       Result = f.Result == "NG" ? false.ToString() : true.ToString()
                            //                   }).ToList();

                            //var tempDetails = findThisBoard.Select(f => new UploadDetail
                            //{
                            //    Type = "SPI",
                            //    ParaName = f.LocationName,
                            //    Range =
                            //        string.Format("{0},{1},{2}",
                            //            string.IsNullOrEmpty(f.PinNumber) ? string.Empty : f.PinNumber,
                            //            string.IsNullOrEmpty(f.PosX) ? string.Empty : f.PosX,
                            //            string.IsNullOrEmpty(f.PosY) ? string.Empty : f.PosY),
                            //    Value = string.IsNullOrEmpty(f.DefectCode) ? string.Empty : f.DefectCode,
                            //    Result = f.Result == "NG" ? false.ToString() : true.ToString()
                            //}).ToList();

                            postPackage.CheckData = JsonConvert.SerializeObject(tempDetails);
                            detail.PostData = postPackage;

                            toPostDetails.Add(detail);
                        }
                    }

                    return toPostDetails.ToArray();
                }
                catch (Exception)
                {
                    // ignored
                }

                return new PostDetail[0];
            };
        }

        internal class ParmiSpiDataStruct
        {
            public string ParaName;
            public int Count;
            public string ErrorType;
            public bool Result;
        }

        /// <summary>
        /// 德律AOI
        /// </summary>
        /// <param name="fovCompFilePath"></param>
        /// <returns></returns>
        private Func<PostDetail[]> TriAoiConvert(string fovCompFilePath)
        {
            return () =>
            {
                try
                {
                    Thread.Sleep(2500);

                    //var str = "20-8281ECP3H2BB20231101HWV0500431_Board_20231220221333.txt";
                    var sp = fovCompFilePath.Split(new[] { "_FovComp_" }, StringSplitOptions.RemoveEmptyEntries);
                    var boardFilePath = string.Format(@"{0}_Board_{1}", sp[0], sp[1]);
                    //Console.WriteLine(boardFilePath);

                    Thread.Sleep(1000);

                    var func = new Func<string, string>(path =>
                    {
                        var fs = new FileStream(path, FileMode.Open); // 初始化文件流

                        var sr = new StreamReader(fs); // 初始化StreamReader            
                        var lines = sr.ReadToEnd(); // 读取全文        
                        sr.Close(); // 关闭流
                        fs.Close(); // 关闭流

                        return lines;
                    });

                    if (File.Exists(boardFilePath))
                    {
                        var boardFile = new FileInfo(boardFilePath);
                        var fovFile = new FileInfo(fovCompFilePath);

                        var uid = boardFile.Name.Split(new[] { "_Board_" }, StringSplitOptions.RemoveEmptyEntries)[0] +
                                  "_" +
                                  boardFile.Name.Split(new[] { "_Board_" }, StringSplitOptions.RemoveEmptyEntries)[1]
                                      .TrimEnd('.', 't', 'x', 't');

                        var newBoardFilePath = string.Format(@"{0}\SMT配置文件\{1}", Program.SysDir, boardFile.Name);
                        var newovCompFilePath = string.Format(@"{0}\SMT配置文件\{1}", Program.SysDir, fovFile.Name);

                        if (File.Exists(newBoardFilePath))
                            File.Delete(newBoardFilePath);
                        if (File.Exists(newovCompFilePath))
                            File.Delete(newovCompFilePath);

                        File.Copy(boardFilePath, newBoardFilePath);
                        File.Copy(fovCompFilePath, newovCompFilePath);

                        var boardLines = func(newBoardFilePath);
                        var fovCompLines = func(newovCompFilePath);

                        File.Delete(newBoardFilePath);
                        File.Delete(newovCompFilePath);

                        OnPushFileCreatedMsgEvent(string.Format(@"{0}: ReadBoard = {1}", Config.LineName, boardFilePath));
                        OnPushFileCreatedMsgEvent(string.Format(@"{0}: ReadFov = {1}", Config.LineName, fovCompLines));

                        //Console.WriteLine(boardLines);
                        //Console.WriteLine(fovCompLines);

                        var triAoi = new TriAoi();

                        var tempTriFovComp = new Dictionary<string, List<TriFovComp>>();

                        var spBoardLines = boardLines.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                        //foreach (var t in spBoardLines)
                        //{
                        //    var spT = t.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        //    tempTriAoiBoard.Add(new TriAoiBoard
                        //    {
                        //        Pname = string.IsNullOrEmpty(spT[0]) ? string.Empty : spT[0],
                        //        Barcode = string.IsNullOrEmpty(spT[1]) ? string.Empty : spT[1],
                        //        Type = string.IsNullOrEmpty(spT[2]) ? string.Empty : spT[2],
                        //        Position = string.IsNullOrEmpty(spT[3]) ? string.Empty : spT[3],
                        //        PbenTime = string.IsNullOrEmpty(spT[4]) ? string.Empty : spT[4],
                        //        EquipNo = string.IsNullOrEmpty(spT[5]) ? string.Empty : spT[5],
                        //        Status = string.IsNullOrEmpty(spT[6]) ? string.Empty : spT[6],
                        //        Pnum1 = string.IsNullOrEmpty(spT[7]) ? string.Empty : spT[7],
                        //        Pnum2 = string.IsNullOrEmpty(spT[8]) ? string.Empty : spT[8],
                        //        EndTime = string.IsNullOrEmpty(spT[9]) ? string.Empty : spT[9],
                        //        Pnum3 = string.IsNullOrEmpty(spT[10]) ? string.Empty : spT[10],
                        //    });
                        //}

                        var tempTriAoiBoard =
                            spBoardLines.Select(t => t.Split(new[] { ',' }, StringSplitOptions.None))
                                .Select(spT => new TriAoiBoard
                                {
                                    Pname = string.IsNullOrEmpty(spT[0]) ? string.Empty : spT[0],
                                    Barcode = string.IsNullOrEmpty(spT[1]) ? string.Empty : spT[1],
                                    Type = string.IsNullOrEmpty(spT[2]) ? string.Empty : spT[2],
                                    Position = string.IsNullOrEmpty(spT[3]) ? string.Empty : spT[3],
                                    PbenTime = string.IsNullOrEmpty(spT[4]) ? string.Empty : spT[4],
                                    EquipNo = string.IsNullOrEmpty(spT[5]) ? string.Empty : spT[5],
                                    Status = string.IsNullOrEmpty(spT[6]) ? string.Empty : spT[6],
                                    Pnum1 = string.IsNullOrEmpty(spT[7]) ? string.Empty : spT[7],
                                    Pnum2 = string.IsNullOrEmpty(spT[8]) ? string.Empty : spT[8],
                                    EndTime = string.IsNullOrEmpty(spT[9]) ? string.Empty : spT[9],
                                    Pnum3 = string.IsNullOrEmpty(spT[10]) ? string.Empty : spT[10],
                                }).ToList();

                        triAoi.TriAoiBoard = tempTriAoiBoard.Any() ? tempTriAoiBoard.ToArray() : new TriAoiBoard[0];

                        foreach (
                            var t in
                                tempTriAoiBoard.Where(
                                    t => !string.IsNullOrEmpty(t.Position) && !tempTriFovComp.ContainsKey(string.Format("{0}", t.Position))))
                            tempTriFovComp.Add(string.Format("{0}", t.Position), new List<TriFovComp>());

                        var spfovCompLines = fovCompLines.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var t in spfovCompLines)
                        {
                            var spT = t.Split(new[] { ',' }, StringSplitOptions.None);

                            var barcode = string.IsNullOrEmpty(spT[1]) ? string.Empty : spT[1];
                            var materialName = string.IsNullOrEmpty(spT[0]) ? string.Empty : spT[0];
                            var position = string.IsNullOrEmpty(spT[3]) ? string.Empty : spT[3];
                            var positionName = string.IsNullOrEmpty(spT[4]) ? string.Empty : spT[4];
                            var pnum0 = string.IsNullOrEmpty(spT[6]) ? string.Empty : spT[6];
                            var materialNo = string.IsNullOrEmpty(spT[7]) ? string.Empty : spT[7];
                            var status = string.IsNullOrEmpty(spT[8]) ? string.Empty : spT[8];
                            var pnum1 = string.IsNullOrEmpty(spT[9]) ? string.Empty : spT[9];
                            var pnum2 = string.IsNullOrEmpty(spT[10]) ? string.Empty : spT[10];
                            var pnum3 = string.IsNullOrEmpty(spT[11]) ? string.Empty : spT[11];

                            if (!string.IsNullOrEmpty(position) &&
                                !string.IsNullOrEmpty(materialName))
                            {
                                var find = tempTriAoiBoard.Find(f => f.Pname == materialName && f.Position == position);

                                if (find != null && tempTriFovComp.ContainsKey(string.Format("{0}", find.Position)))
                                {
                                    tempTriFovComp[string.Format("{0}", find.Position)].Add(new TriFovComp
                                    {
                                        Position = position,
                                        PositionName = positionName,
                                        Pnum0 = pnum0,
                                        Pnum1 = pnum1,
                                        Pnum2 = pnum2,
                                        Pnum3 = pnum3,
                                        MaterialNo = materialNo,
                                        Status = status
                                    });
                                }
                            }
                        }

                        foreach (var key in tempTriFovComp.Keys)
                        {
                            var key1 = key;
                            var findIndex = triAoi.TriAoiBoard.ToList().FindIndex(f => f.Position == key1);

                            if (findIndex != -1)
                            {
                                triAoi.TriAoiBoard[findIndex].TriFovComp = tempTriFovComp[key].ToArray();
                            }
                        }

                        var details = new List<PostDetail>();

                        foreach (var t in triAoi.TriAoiBoard)
                        {
                            // 20231220221333
                            var year = t.PbenTime.Substring(0, 4);
                            var month = t.PbenTime.Substring(4, 2);
                            var day = t.PbenTime.Substring(6, 2);
                            var hour = t.PbenTime.Substring(8, 2);
                            var min = t.PbenTime.Substring(10, 2);
                            var second = t.PbenTime.Substring(12, 2);
                            var dt = DateTime.Parse(string.Format("{0}/{1}/{2} {3}:{4}:{5}", year, month, day, hour, min, second));

                            var postPackage = new UploadPackage
                            {
                                MaterialName = t.Pname,
                                MaterialUid = uid,
                                MaterialBarcode = string.IsNullOrEmpty(t.Barcode) ? string.Empty : t.Barcode,
                                MaterialCustomerBarcode = string.IsNullOrEmpty(t.Barcode) ? string.Empty : t.Barcode,
                                SubMaterialsInfo = string.IsNullOrEmpty(t.Barcode) ? string.Empty : t.Barcode,
                                Result = t.Status == "R" ? "0002" : "0001",
                                DeviceNo = Config.DeviceNo,
                                CheckDateTiem = dt.ToString(CultureInfo.InvariantCulture),
                                Note = string.Format("{0};{1}", fovCompFilePath, boardFilePath),
                            };

                            var tempDetails = t.TriFovComp.Select(fov => new UploadDetail
                            {
                                Type = "AOI",
                                ParaName = fov.Status ==
                                           "R" ?
                                    (string.IsNullOrEmpty(fov.Pnum2) ? string.Format("{0},{1}", fov.Position, fov.PositionName) : string.Format("{0},{1}_{2}", fov.Position, fov.PositionName, fov.Pnum2)) :
                                    string.Format("{0},{1}", fov.Position, fov.PositionName),//fov.PositionName,
                                Range = "/",
                                Value = string.Format("{0},{1},{2}", fov.Status, string.IsNullOrEmpty(fov.Pnum1) ? string.Empty : fov.Pnum1, string.IsNullOrEmpty(fov.Pnum2) ? string.Empty : fov.Pnum2),//string.IsNullOrEmpty(fov.Pnum2) ? string.Empty : fov.Pnum2
                                Result = fov.Status == "R" ? false.ToString() : true.ToString()
                            }).ToList();

                            tempDetails = new List<UploadDetail>(tempDetails.OrderBy(oo => oo.Result));
                            postPackage.CheckData = JsonConvert.SerializeObject(tempDetails);

                            var detail = new PostDetail
                            {
                                Config = Config,
                                NewCreatedFilePath = string.Format("{0};{1}", fovCompFilePath, boardFilePath),
                                PostData = postPackage
                            };
                            details.Add(detail);
                        }

                        OnPushFileCreatedMsgEvent(string.Format(@"{0}: PushTriPost = {1}", Config.LineName, boardFilePath));
                        return details.ToArray();
                    }
                    else
                    {

                    }

                        OnPushFileCreatedMsgEvent(string.Format(@"{0}: ReadBoardFailed", Config.LineName));
                }
                catch (Exception ex)
                {
                    OnPushFileCreatedMsgEvent(string.Format(@"{0}: PushTriPostFailed, error = {1}", Config.LineName,
                        ex.Message));
                    return null;
                }

                OnPushFileCreatedMsgEvent(string.Format(@"{0}: PushTriPostFailed", Config.LineName));
                return null;
            };
        }

        private Func<PostDetail[]> KouYoungAoiConvert(KouYoungAoi_TB_AOIResult kouYoungAoiTbAoiResult, string dateBase)
        {
            return () =>
            {
                try
                {
                    lock (_lockSqlTableDependency)
                    {
                        Thread.Sleep(5500);

                        var getTableFunc = new Func<string, SqlConnection, DataTable>((sql, con) =>
                        {
                            try
                            {
                                using (var cmd = new SqlCommand(sql, con))
                                {
                                    using (var dr = cmd.ExecuteReader())
                                    {
                                        var dt = new DataTable();
                                        dt.Load(dr);
                                        cmd.Prepare();
                                        return dt;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                return new DataTable();
                            }
                        });

                        var sqlConnectiong =
                            string.Format("server={0};database={1};uid={2};pwd={3}", _server, dateBase, _uid, _pwd);
                        var pd = new PostDetail[1];

                        using (var connection = new SqlConnection(sqlConnectiong))
                        {
                            connection.Open();

                            var dtTbAoiResult = getTableFunc(string.Format("select PCBGUID,PCBResultRepair,PCBModel,BarCode,ReviewStartDateTime,ALLBarCode from TB_AOIResult where PCBGUID = '{0}'", kouYoungAoiTbAoiResult.PCBGUID), connection);

                            if (dtTbAoiResult.DefaultView.Count > 0)
                            {
                                var rowPcb = dtTbAoiResult.DefaultView[0];
                                if (rowPcb["PCBGUID"] != null && !string.IsNullOrEmpty(rowPcb["PCBGUID"].ToString()))
                                {
                                    var pcbGuid = rowPcb["PCBGUID"].ToString();
                                    var pcbResultRepair = rowPcb["PCBResultRepair"] == null ? string.Empty : rowPcb["PCBResultRepair"].ToString();
                                    var pcbModel = rowPcb["PCBModel"] == null ? string.Empty : rowPcb["PCBModel"].ToString();
                                    var barCode = rowPcb["BarCode"] == null ? string.Empty : rowPcb["BarCode"].ToString();
                                    var reviewStartDateTime = rowPcb["ReviewStartDateTime"] == null ? string.Empty : rowPcb["ReviewStartDateTime"].ToString();
                                    var allBarcode = rowPcb["ALLBarCode"] == null ? string.Empty : rowPcb["ALLBarCode"].ToString();

                                    var uid = barCode;
                                    var subMaterialsInfo = string.Empty;

                                    var allBarcodeSp = allBarcode.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                    if (allBarcodeSp.Any())
                                    {
                                        if (string.IsNullOrEmpty(uid))
                                            uid = allBarcodeSp[0];

                                        subMaterialsInfo = allBarcodeSp.Aggregate(subMaterialsInfo, (current, t) => current + (t + "|"));
                                        subMaterialsInfo = subMaterialsInfo.TrimEnd('|');
                                    }

                                    pd[0] = new PostDetail
                                    {
                                        Config = Config,
                                        NewCreatedFilePath = string.Format("{0};{1};{2}", _server, dateBase, kouYoungAoiTbAoiResult.PCBGUID),//kouYoungAoiTbAoiResult.PCBGUID,
                                        PostData = new UploadPackage
                                        {
                                            MaterialName = pcbModel,
                                            MaterialUid = pcbGuid,
                                            MaterialBarcode = string.IsNullOrEmpty(uid) ? string.Empty : uid,
                                            MaterialCustomerBarcode = string.IsNullOrEmpty(uid) ? string.Empty : uid,
                                            SubMaterialsInfo = string.IsNullOrEmpty(subMaterialsInfo) ? string.Empty : subMaterialsInfo,
                                            DeviceNo = Config.DeviceNo,
                                            CheckDateTiem = reviewStartDateTime,
                                            Result = pcbResultRepair == "13000000" ? "0002" : "0001",
                                            Note = string.Format("{0};{1}", _server, dateBase)
                                        }
                                    };

                                    var dtTbAoiDefectDetail = getTableFunc(string.Format("select DetailGUID,Defect,ReDefect,Failure,Repair,TB_AOIDefectDetail.PCBGUID,TB_AOIDefectDetail.ComponentGUID,TB_AOIDefect.uname as uname,TB_AOIDefect.ArrayIndex as ArrayIndex,InspType from TB_AOIDefectDetail inner join TB_AOIDefect on TB_AOIDefectDetail.ComponentGUID=TB_AOIDefect.ComponentGUID where  TB_AOIDefectDetail.PCBGUID = '{0}'", kouYoungAoiTbAoiResult.PCBGUID), connection);

                                    var tempUploadDetail = new List<UploadDetail>();

                                    for (var j = 0; j < dtTbAoiDefectDetail.DefaultView.Count; j++)
                                    {
                                        var rowDetail = dtTbAoiDefectDetail.DefaultView[j];

                                        if (rowDetail["Defect"] != null && !string.IsNullOrEmpty(rowDetail["Defect"].ToString()) &&
                                            rowDetail["DetailGUID"] != null &&
                                            rowDetail["ReDefect"] != null && !string.IsNullOrEmpty(rowDetail["ReDefect"].ToString()) &&
                                            rowDetail["Failure"] != null && !string.IsNullOrEmpty(rowDetail["Failure"].ToString()) &&
                                            rowDetail["Repair"] != null && !string.IsNullOrEmpty(rowDetail["Repair"].ToString()) &&
                                            rowDetail["ComponentGUID"] != null && !string.IsNullOrEmpty(rowDetail["ComponentGUID"].ToString()) &&
                                            rowDetail["uname"] != null && !string.IsNullOrEmpty(rowDetail["uname"].ToString()) &&
                                            rowDetail["ArrayIndex"] != null && !string.IsNullOrEmpty(rowDetail["ArrayIndex"].ToString()) &&
                                            rowDetail["InspType"] != null && !string.IsNullOrEmpty(rowDetail["InspType"].ToString()))
                                        {
                                            var defect = rowDetail["Defect"].ToString();
                                            var reDefect = rowDetail["ReDefect"].ToString();
                                            var failure = rowDetail["Failure"].ToString();
                                            var repair = rowDetail["Repair"].ToString();

                                            var uname = rowDetail["uname"].ToString();
                                            var arrayIndex = rowDetail["ArrayIndex"].ToString();

                                            var inspType = rowDetail["InspType"].ToString();

                                            var tempUploadPackage = new UploadDetail
                                            {
                                                Type = "AOI",
                                                ParaName = defect != "12000000" ? (string.IsNullOrEmpty(defect) ? string.Format("{0},{1}", arrayIndex, uname) : string.Format("{0},{1}_{2}", arrayIndex, uname, defect)) : string.Format("{0},{1}", arrayIndex, uname),//uname,
                                                Range = rowDetail["DetailGUID"].ToString(),
                                                Value = string.Format("{0},{1},{2}", repair == "0" ? "R" : "Y", inspType, defect),//defect,
                                                Result = defect == "12000000" ? true.ToString() : false.ToString()
                                            };
                                            tempUploadDetail.Add(tempUploadPackage);
                                        }
                                    }

                                    tempUploadDetail = new List<UploadDetail>(tempUploadDetail.OrderBy(oo => oo.Result));
                                    pd[0].PostData.CheckData = JsonConvert.SerializeObject(tempUploadDetail);
                                }
                            }

                            connection.Close();
                            return pd[0] != null ? pd : null;
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            };
        }

        private Func<PostDetail[]> HollyAoiConvert(string filePath)
        {
            return () =>
            {
                Thread.Sleep(5000);

                try
                {
                    var fileInfo = new FileInfo(filePath);

                    var newFilePath = string.Format(@"{0}\SMT配置文件\{1}", Program.SysDir, fileInfo.Name);
                    if (File.Exists(newFilePath))
                        File.Delete(newFilePath);
                    File.Copy(filePath, newFilePath);

                    var func = new Func<string, string>(path =>
                    {
                        var fs = new FileStream(path, FileMode.Open); // 初始化文件流

                        var sr = new StreamReader(fs); // 初始化StreamReader            
                        var lines = sr.ReadToEnd(); // 读取全文        
                        sr.Close(); // 关闭流
                        fs.Close(); // 关闭流

                        return lines;
                    });

                    var readLinse = func(newFilePath).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    File.Delete(newFilePath);

                    var snIndex = readLinse.ToList().FindIndex(f => f.ToString().ToLower().StartsWith("SN=".ToLower()));
                    var modelIndex = readLinse.ToList().FindIndex(f => f.ToString().ToLower().StartsWith("Model=".ToLower()));
                    var repDateTimeIndex = readLinse.ToList().FindIndex(f => f.ToString().ToLower().StartsWith("Rep_Date_Time=".ToLower()));

                    if (snIndex != -1 && modelIndex != -1 && repDateTimeIndex != -1)
                    {
                        var sn = readLinse.ToList()[snIndex].Split(new[] { "SN=" }, StringSplitOptions.None)[1];
                        var model = readLinse.ToList()[modelIndex].Split(new[] { "Model=" }, StringSplitOptions.None)[1];
                        var repDateTime = readLinse.ToList()[repDateTimeIndex].Split(new[] { "Rep_Date_Time=" }, StringSplitOptions.None)[1];

                        var firstLocationIndex = readLinse.ToList().FindIndex(f => f.ToString().ToLower().StartsWith("Location=".ToLower()));
                        var lastRepairStatusIndex = readLinse.ToList().FindLastIndex(f => f.ToString().ToLower().StartsWith("RepairStatus=".ToLower()));

                        var uploadDetails = new List<UploadDetail>();

                        if (firstLocationIndex > 0 && lastRepairStatusIndex > 0 && (lastRepairStatusIndex - firstLocationIndex + 1) % 4 == 0)
                        {
                            for (var i = firstLocationIndex; i < firstLocationIndex + 4; i = i + 4)
                            {
                                var location = readLinse.ToList()[i].Split(new[] { "Location=" }, StringSplitOptions.None)[1];
                                //var pinNum = readLinse.ToList()[i + 1].Split(new[] { "PinNum=" }, StringSplitOptions.None)[1];
                                var errCode = readLinse.ToList()[i + 2].Split(new[] { "ErrCode=" }, StringSplitOptions.None)[1];
                                var repairStatus = readLinse.ToList()[i + 3].Split(new[] { "RepairStatus=" }, StringSplitOptions.None)[1];

                                if (string.Equals(repairStatus, "Repaired", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    if (location.Split('@').Length == 2)
                                    {
                                        uploadDetails.Add(new UploadDetail
                                        {
                                            Type = "AOI",
                                            ParaName = string.Format("{0},{1}_{2}", location.Split('@')[0], location.Split('@')[1], errCode),//location,
                                            Range = "/",
                                            Result = false.ToString(),
                                            Value = string.Format("R,{0},{1}", errCode, errCode)//errCode
                                        });
                                    }
                                }
                            }
                        }

                        // 20231220221333
                        var year = repDateTime.Substring(0, 4);
                        var month = repDateTime.Substring(4, 2);
                        var day = repDateTime.Substring(6, 2);
                        var hour = repDateTime.Substring(8, 2);
                        var min = repDateTime.Substring(10, 2);
                        var second = repDateTime.Substring(12, 2);
                        var dt = DateTime.Parse(string.Format("{0}/{1}/{2} {3}:{4}:{5}", year, month, day, hour, min, second));

                        uploadDetails = new List<UploadDetail>(uploadDetails.OrderBy(oo => oo.Result));

                        var uploadPackage = new UploadPackage
                        {
                            MaterialName = model,
                            MaterialUid = sn,
                            MaterialBarcode = string.Empty,
                            MaterialCustomerBarcode = string.Empty,
                            SubMaterialsInfo = string.Empty,
                            Note = filePath,
                            CheckDateTiem = dt.ToString(CultureInfo.InvariantCulture),
                            Result = uploadDetails.Any() ? "0002" : "0001",
                            CheckData = JsonConvert.SerializeObject(uploadDetails),
                            DeviceNo = Config.DeviceNo
                        };

                        var listPostDetail = new List<PostDetail>
                        {
                            new PostDetail
                            {
                                Config = Config,
                                NewCreatedFilePath = filePath,
                                PostData = uploadPackage
                            }
                        };

                        return listPostDetail.ToArray();
                    }
                }
                catch (Exception)
                {
                    return null;
                }

                return null;
            };
        }

        private Func<PostDetail[]> IctConvert(string filePath)
        {
            return () =>
            {
                try
                {
                    Thread.Sleep(250);

                    var fileInfo = new FileInfo(filePath);

                    var newFilePath = string.Format(@"{0}\SMT配置文件\{1}", Program.SysDir, fileInfo.Name);
                    if (File.Exists(newFilePath))
                        File.Delete(newFilePath);
                    File.Copy(filePath, newFilePath);

                    var func = new Func<string, string>(path =>
                    {
                        var fs = new FileStream(path, FileMode.Open); // 初始化文件流

                        var sr = new StreamReader(fs); // 初始化StreamReader            
                        var lines = sr.ReadToEnd(); // 读取全文        
                        sr.Close(); // 关闭流
                        fs.Close(); // 关闭流

                        return lines;
                    });

                    var readLinse = func(newFilePath)
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    File.Delete(newFilePath);

                    var fileName = fileInfo.Name;
                    var fileNameSp = fileName.Split(new[] { '#' });
                    if (fileNameSp.Length == 4)
                    {
                        var repDateTime = fileNameSp[0];
                        var name = fileNameSp[1];
                        var uid = fileNameSp[2];
                        var isNg = fileNameSp[3].ToLower().Contains("ng");

                        var subBarcodes = string.Empty;

                        // 20240327083010
                        var year = repDateTime.Substring(0, 4);
                        var month = repDateTime.Substring(4, 2);
                        var day = repDateTime.Substring(6, 2);
                        var hour = repDateTime.Substring(8, 2);
                        var min = repDateTime.Substring(10, 2);
                        var second = repDateTime.Substring(12, 2);
                        var dt = DateTime.Parse(string.Format("{0}/{1}/{2} {3}:{4}:{5}", year, month, day, hour, min,
                            second));

                        var uploadDetails = new List<UploadDetail>();
                        var lineCount = -1;
                        foreach (var t in readLinse)
                        {
                            lineCount++;
                            var tSp = t.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            if (!string.IsNullOrEmpty(uid) && t.Contains(uid))
                            {
                                var findAllBarcodes = tSp.ToList().FindAll(f =>
                                    f.Length == uid.Length && !string.Equals(f, uid, StringComparison.CurrentCultureIgnoreCase));

                                subBarcodes += uid + "|";
                                subBarcodes = findAllBarcodes.Aggregate(subBarcodes, (current, subCode) => current + (subCode + "|"));
                                subBarcodes = subBarcodes.TrimEnd('|');
                            }
                            else
                            {
                                const string type = "ICT";

                                if (tSp.Length > 1 && lineCount != 0)
                                {
                                    var firstCellValue = tSp[0];

                                    int intValue;
                                    if (int.TryParse(firstCellValue, out intValue))
                                    {
                                        // 11列格式
                                        if (tSp.Length >= 3)
                                        {
                                            var paraName = tSp[1].Trim();

                                            var range = string.Empty; //tSp[2].Trim();
                                            for (var i = 2; i < tSp.Length - 1 - 1; i++)
                                                range += tSp[i].Trim() + ",";
                                            range = range.TrimEnd(',');

                                            var value = tSp[tSp.Length - 1 - 1].Trim();

                                            var isDetailOk = 0;
                                            if (!int.TryParse(tSp[tSp.Length - 1].Trim(), out isDetailOk))
                                                isDetailOk = 0;

                                            uploadDetails.Add(new UploadDetail
                                            {
                                                ParaName = paraName,
                                                Type = type,
                                                Range = range,
                                                Value = value,
                                                Result = isDetailOk == 1 ? false.ToString() : true.ToString(),
                                            });
                                        }
                                    }
                                    else
                                    {
                                        // 9列格式
                                        if (tSp.Length >= 4)
                                        {
                                            var paraName = tSp[0].Trim();

                                            var range = string.Empty; //tSp[2].Trim();
                                            for (var i = 1; i < tSp.Length - 1 - 1; i++)
                                                range += tSp[i].Trim() + ",";
                                            range = range.TrimEnd(',');

                                            var value = tSp[tSp.Length - 1 - 1].Trim();

                                            var isDetailOk = 0;
                                            if (!int.TryParse(tSp[tSp.Length - 1].Trim(), out isDetailOk))
                                                isDetailOk = 0;

                                            uploadDetails.Add(new UploadDetail
                                            {
                                                ParaName = paraName,
                                                Type = type,
                                                Range = range,
                                                Value = value,
                                                Result = false.ToString(),
                                            });
                                        }
                                    }
                                }

                                //if (isNg)
                                //{
                                //    const string type = "ICT";

                                //    if (tSp.Length > 1)
                                //    {
                                //        var firstCellValue = tSp[0];

                                //        int intValue;
                                //        if (int.TryParse(firstCellValue, out intValue))
                                //        {
                                //            // 9列格式
                                //            if (tSp.Length >= 9)
                                //            {
                                //                var paraName = tSp[1].Trim();
                                //                var range = tSp[2].Trim();
                                //                var value = tSp[tSp.Length - 1 - 1].Trim();

                                //                uploadDetails.Add(new UploadDetail
                                //                {
                                //                    ParaName = paraName,
                                //                    Type = type,
                                //                    Range = range,
                                //                    Value = value,
                                //                    Result = false.ToString(),
                                //                });
                                //            }
                                //        }
                                //        else
                                //        {
                                //            // 11列格式
                                //            if (tSp.Length >= 11)
                                //            {
                                //                var paraName = tSp[0].Trim();
                                //                var range = tSp[1].Trim();
                                //                var value = tSp[tSp.Length - 1 - 1].Trim();

                                //                uploadDetails.Add(new UploadDetail
                                //                {
                                //                    ParaName = paraName,
                                //                    Type = type,
                                //                    Range = range,
                                //                    Value = value,
                                //                    Result = false.ToString(),
                                //                });
                                //            }
                                //        }
                                //    }
                                //}
                            }
                        }

                        var uploadPackage = new UploadPackage
                        {
                            ProcessName = "ICT检查",
                            MaterialName = name,
                            MaterialUid = uid,
                            MaterialBarcode = uid,
                            MaterialCustomerBarcode = string.Empty,
                            SubMaterialsInfo = subBarcodes,
                            Note = filePath,
                            CheckDateTiem = dt.ToString(CultureInfo.InvariantCulture),

                            Result = isNg ? "0002" : "0001",
                            CheckData = JsonConvert.SerializeObject(uploadDetails),
                            DeviceNo = Config.DeviceNo
                        };

                        var listPostDetail = new List<PostDetail>
                        {
                            new PostDetail
                            {
                                Config = Config,
                                NewCreatedFilePath = filePath,
                                PostData = uploadPackage,
                                IsNeedDeleteOld = true
                            }
                        };

                        return listPostDetail.ToArray();
                    }
                }
                catch (Exception)
                {
                    return null;
                }

                return null;
            };
        }

        private Func<PostDetail[]> PanasonicNpmDatConvert(string filePath)
        {
            return () =>
            {
                Thread.Sleep(2000);

                try
                {
                    var fileInfo = new FileInfo(filePath);

                    var newFilePath = string.Format(@"{0}\SMT配置文件\{1}", Program.SysDir, fileInfo.Name);
                    if (File.Exists(newFilePath))
                        File.Delete(newFilePath);
                    File.Copy(filePath, newFilePath);

                    var func = new Func<string, string>(path =>
                    {
                        var fs = new FileStream(path, FileMode.Open); // 初始化文件流

                        var sr = new StreamReader(fs); // 初始化StreamReader            
                        var lines = sr.ReadToEnd(); // 读取全文        
                        sr.Close(); // 关闭流
                        fs.Close(); // 关闭流

                        return lines;
                    });

                    var readLinse = func(newFilePath)
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    File.Delete(newFilePath);

                    //var fileName = fileInfo.Name;

                    var listPanaconicNpmDat = new List<PanaconicNpmDat>();

                    if (readLinse.Length > 2)
                    {
                        for (var i = 2; i < readLinse.Length - 1; i++)
                        {
                            var line = readLinse[i];

                            if (!string.IsNullOrEmpty(line))
                            {
                                var spLine = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                if (spLine.Length == 14)
                                {
                                    int pickup;
                                    int pMiss;
                                    int rMiss;
                                    int dMiss;
                                    int mMiss;
                                    int hMiss;
                                    int trsMiss;

                                    if (!string.IsNullOrEmpty(spLine[1]) &&
                                        !string.IsNullOrEmpty(spLine[2]) &&
                                        !string.IsNullOrEmpty(spLine[5]) &&
                                        !string.IsNullOrEmpty(spLine[6]) &&
                                        !string.IsNullOrEmpty(spLine[7]) &&
                                        !string.IsNullOrEmpty(spLine[8]) &&
                                        !string.IsNullOrEmpty(spLine[9]) &&
                                        !string.IsNullOrEmpty(spLine[10]) &&
                                        !string.IsNullOrEmpty(spLine[11]) &&
                                        !string.IsNullOrEmpty(spLine[12]) &&
                                        int.TryParse(spLine[6], out pickup) &&
                                        int.TryParse(spLine[7], out pMiss) &&
                                        int.TryParse(spLine[8], out rMiss) &&
                                        int.TryParse(spLine[9], out dMiss) &&
                                        int.TryParse(spLine[10], out mMiss) &&
                                        int.TryParse(spLine[11], out hMiss) &&
                                        int.TryParse(spLine[12], out trsMiss))
                                    {
                                        if (pickup > 0)
                                        {
                                            var panaconicNpmDat = new PanaconicNpmDat
                                            {
                                                Lane = spLine[1],
                                                FAdd = spLine[2],
                                                FeederSerial = spLine[5],
                                                Pickup = pickup,
                                                PMiss = pMiss,
                                                RMiss = rMiss,
                                                DMiss = dMiss,
                                                MMiss = mMiss,
                                                HMiss = hMiss,
                                                TRSMiss = trsMiss
                                            };

                                            listPanaconicNpmDat.Add(panaconicNpmDat);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (listPanaconicNpmDat.Any())
                    {
                        var uploadDetails = listPanaconicNpmDat.Select(t => new UploadDetail
                        {
                            Type = "贴片机抛料",
                            ParaName = string.Format("{0},{1},{2}", t.Lane, t.FAdd, t.FeederSerial),
                            Range = "Pickup,PMiss,RMiss,DMiss,MMiss,HMiss,TRSMiss",
                            Value = string.Format("{0},{1},{2},{3},{4},{5},{6}", t.Pickup, t.PMiss, t.RMiss, t.DMiss, t.MMiss, t.HMiss, t.TRSMiss),
                            Result = bool.TrueString
                        })
                            .ToList();

                        var lineNo = string.Empty;
                        if (filePath.ToLower().Contains("lineB".ToLower()))
                            lineNo = "2";
                        else if (filePath.ToLower().Contains("lineC".ToLower()))
                            lineNo = "3";
                        else if (filePath.ToLower().Contains("lineD".ToLower()))
                            lineNo = "4";

                        var name = lineNo + "_贴片机抛料";
                        var uid = lineNo + "_" + fileInfo.NameWithoutExt();

                        var uploadPackage = new UploadPackage
                        {
                            MaterialName = name,
                            MaterialUid = uid,
                            MaterialBarcode = uid,
                            MaterialCustomerBarcode = string.Empty,
                            SubMaterialsInfo = string.Empty,
                            Note = filePath,
                            CheckDateTiem = DateTime.Now.ToString(CultureInfo.InvariantCulture),

                            Result = "0001",
                            CheckData = JsonConvert.SerializeObject(uploadDetails),
                            DeviceNo = Config.DeviceNo
                        };

                        var listPostDetail = new List<PostDetail>
                        {
                            new PostDetail
                            {
                                Config = Config,
                                NewCreatedFilePath = filePath,
                                PostData = uploadPackage
                            }
                        };

                        return listPostDetail.ToArray();
                    }
                }
                catch (Exception e)
                {
                    return null;
                }

                return null;
            };
        }

        public class PostDetail
        {
            public UploadConfigModel Config { get; set; }
            public string NewCreatedFilePath { get; set; }
            public UploadPackage PostData { get; set; }
            public bool IsNeedDeleteOld { get; set; }
        }

        #region 松下贴片机推送插槽信息

        public class PanasonicPostDetail
        {
            public string DEVICE_SN { get; set; }
            public string MachineName { get; set; }
            public string IP { get; set; }
            public string LotNameLane1 { get; set; }
            public string PU { get; set; }
            public string SIDE { get; set; }
            public string SerialNo { get; set; }
        }

        private readonly Queue<Func<PanasonicPostDetail[]>> _panasonicTask = new Queue<Func<PanasonicPostDetail[]>>();
        private readonly object _panasonicLocker = new object();
        private readonly EventWaitHandle _panasonicWh = new AutoResetEvent(false);

        private Func<PanasonicPostDetail[]> PanasonicNpmConvert(string filePath)
        {
            return () =>
            {
                Thread.Sleep(1500);

                try
                {
                    var fileInfo = new FileInfo(filePath);

                    var newFilePath = string.Format(@"{0}\SMT配置文件\{1}", Program.SysDir, fileInfo.Name);
                    if (File.Exists(newFilePath))
                        File.Delete(newFilePath);
                    File.Copy(filePath, newFilePath);

                    var func = new Func<string, string>(path =>
                    {
                        var fs = new FileStream(path, FileMode.Open); // 初始化文件流

                        var sr = new StreamReader(fs); // 初始化StreamReader            
                        var lines = sr.ReadToEnd(); // 读取全文        
                        sr.Close(); // 关闭流
                        fs.Close(); // 关闭流

                        return lines;
                    });

                    var readLinse = func(newFilePath).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    //File.Delete(filePath);
                    File.Delete(newFilePath);

                    var panasonicNpm = new PanasonicNpm();
                    var panasonicNpmArrangements = new List<Arrangement>();

                    var listPostDetails = new List<PanasonicPostDetail>();

                    for (var i = 0; i < readLinse.Length; i++)
                    {
                        var line = readLinse[i];

                        if (line.ToLower().StartsWith("ip".ToLower()))
                            panasonicNpm.Ip = line.Split('=')[1];
                        else if (line.ToLower().StartsWith("LotNameLane1".ToLower()))
                            panasonicNpm.LotNameLane1 = line.Split('=')[1];
                        else if (line.ToLower().StartsWith("MachineName".ToLower()))
                            panasonicNpm.MachineName = line.Split('=')[1];
                        else if (line.ToLower().Contains("[Arrangement]".ToLower()))
                        {
                            for (var j = i + 2; j < readLinse.Length; j++)
                            {
                                var arrangementLine = readLinse[j].Split(' ');
                                panasonicNpmArrangements.Add(new Arrangement
                                {
                                    Pu = arrangementLine[0].Trim('"'),
                                    SerialNo = arrangementLine[17].Trim('"'),
                                    Side = arrangementLine[1].Trim('"'),
                                });
                            }
                            panasonicNpm.Arrangements = panasonicNpmArrangements.ToArray();
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(panasonicNpm.Ip) &&
                        !string.IsNullOrEmpty(panasonicNpm.LotNameLane1) &&
                        !string.IsNullOrEmpty(panasonicNpm.MachineName) &&
                        panasonicNpm.Arrangements != null &&
                        panasonicNpm.Arrangements.Any())
                    {
                        listPostDetails.AddRange(from t in panasonicNpm.Arrangements
                                                 where !string.IsNullOrEmpty(t.Pu) && !string.IsNullOrEmpty(t.SerialNo) &&
                                                       !string.IsNullOrEmpty(t.Side)
                                                 select new PanasonicPostDetail
                                                 {
                                                     DEVICE_SN = Config.DeviceNo,
                                                     IP = panasonicNpm.Ip,
                                                     LotNameLane1 = panasonicNpm.LotNameLane1,
                                                     PU = t.Pu,
                                                     SerialNo = t.SerialNo,
                                                     SIDE = t.Side,
                                                     MachineName = panasonicNpm.MachineName
                                                 });
                    }

                    return listPostDetails.ToArray();
                }
                catch (Exception)
                {
                    return null;
                }
            };
        }

        public Task UploadPanasonicTask()
        {
            return functional.init(() =>
            {
                var task = new Task(() =>
                {
                    while (true)
                    {
                        Func<PanasonicPostDetail[]> action = null;

                        lock (_panasonicLocker)
                        {
                            if (_panasonicTask.Count > 0)
                            {
                                action = _panasonicTask.Dequeue();

                                if (action == null)
                                    return;
                            }
                        }

                        if (action != null)
                        {
                            var postDetail = action.Invoke();
                            if (postDetail != null)
                                OnPushPanasonicMsgEvent(postDetail);
                        }
                        else
                            _panasonicWh.WaitOne();
                    }
                });
                task.Start();
                return task;
            });
        }

        public void EqueuePanasonicTask(Func<PanasonicPostDetail[]> action)
        {
            lock (_panasonicLocker)
                _panasonicTask.Enqueue(action);

            _panasonicWh.Set();
        }

        public delegate void PushPanasonicMsgDelegate(PanasonicPostDetail[] postDetail);
        public event PushPanasonicMsgDelegate PushPanasonicMsgEvent;

        private void OnPushPanasonicMsgEvent(PanasonicPostDetail[] postDetail)
        {
            try
            {
                var handler = PushPanasonicMsgEvent;
                if (handler != null) handler(postDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 分板机推送数据

        private readonly object _lockAurote = new object();
        private int _lastLCountAurote = -1;
        private int _lastRCountAurote = -1;
        private DateTime _lastLDtAurote = DateTime.Now;
        private DateTime _lastRDtAurote = DateTime.Now;

        /// <summary>
        /// 和椿
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private Func<PostDetail[]> AuroteConvert(string filePath)
        {
            lock (_lockAurote)
            {
                return () =>
                {
                    Thread.Sleep(50);

                    var isL = false;

                    if (filePath.ToLower().EndsWith("L_Table.csv".ToLower()))
                        isL = true;
                    else if (filePath.ToLower().EndsWith("R_Table.csv".ToLower()))
                        isL = false;
                    else
                        return null;

                    try
                    {
                        var fileInfo = new FileInfo(filePath);

                        var newFilePath = string.Format(@"{0}\SMT配置文件\{1}", Program.SysDir, fileInfo.Name);
                        if (File.Exists(newFilePath))
                            File.Delete(newFilePath);
                        File.Copy(filePath, newFilePath);

                        var rows = MiniExcel.Query(newFilePath, excelType: ExcelType.CSV).ToList();

                        File.Delete(newFilePath);

                        var infoStartIndex = rows.FindIndex(f => f.A.ToString().Trim().ToLower() == "Date".ToLower());

                        if (infoStartIndex == 0 && rows.Count > 1)
                        {
                            var lastRow = rows[rows.Count - 1];

                            var date = lastRow.A;
                            var time = lastRow.B;
                            var jigBarcodeNumber = lastRow.C;
                            var pcbBarcodeNumber = lastRow.D;
                            var productName = lastRow.E;
                            var qty = lastRow.F;
                            var cycleTime = lastRow.G;
                            var opeartorIdNumber = lastRow.H;
                            var type = lastRow.J;

                            if (type != null && string.IsNullOrEmpty(type) &&
                                cycleTime != null && !string.IsNullOrEmpty(cycleTime) &&
                                qty != null && !string.IsNullOrEmpty(qty) &&
                                productName != null && !string.IsNullOrEmpty(productName))
                            {
                                double cycleTimeSec;
                                var count = 0;
                                var dt = new DateTime();
                                if (double.TryParse(cycleTime, out cycleTimeSec) &&
                                    int.TryParse(qty, out count) &&
                                    DateTime.TryParse(string.Format("{0} {1}", date, time), out dt) &&
                                    count > 0)
                                {
                                    var ms = -9999;
                                    if (isL)
                                    {
                                        if (count != _lastLCountAurote)
                                        {
                                            ms = ValueHelper.GetTimeSpanMs(_lastLDtAurote, dt);

                                            _lastLCountAurote = count;
                                            _lastLDtAurote = dt;
                                        }
                                    }
                                    else
                                    {
                                        if (count != _lastRCountAurote)
                                        {
                                            ms = ValueHelper.GetTimeSpanMs(_lastRDtAurote, dt);

                                            _lastRCountAurote = count;
                                            _lastRDtAurote = dt;
                                        }
                                    }

                                    if (ms > 0)
                                    {
                                        var timsSpanSec = Math.Round(ms / 1000f, 2, MidpointRounding.AwayFromZero);

                                        var checkData = new UploadDetail
                                        {
                                            Type = "分板",
                                            ParaName = count.ToString(),
                                            Range = dt.ToString(CultureInfo.InvariantCulture),
                                            Value = string.Format("{0}{1}", timsSpanSec.ToString(CultureInfo.InvariantCulture), string.IsNullOrEmpty(cycleTime.ToString()) ? string.Empty : string.Format(",{0}", cycleTime)),//timsSpanSec.ToString(CultureInfo.InvariantCulture) + string.IsNullOrEmpty(cycleTime) ? string.Empty : string.Format(",{0}", cycleTime),
                                            Result = bool.TrueString
                                        };

                                        var subBarcode = string.Empty;
                                        var barcode = pcbBarcodeNumber;
                                        if (barcode != null && !string.IsNullOrEmpty(barcode))
                                            subBarcode += barcode + "|";

                                        if (!string.IsNullOrEmpty(jigBarcodeNumber))
                                            subBarcode += barcode;

                                        subBarcode = subBarcode.TrimEnd('|');

                                        var toPostDetails = new List<PostDetail>
                                        {
                                            new PostDetail()
                                            {
                                                Config = Config,
                                                NewCreatedFilePath = filePath,
                                                PostData = new UploadPackage
                                                {
                                                    CheckData = JsonConvert.SerializeObject(new List<UploadDetail>{checkData}),
                                                    CheckDateTiem = dt.ToString(CultureInfo.InvariantCulture),
                                                    DeviceNo = Config.DeviceNo,
                                                    MaterialBarcode = barcode,
                                                    MaterialCustomerBarcode = subBarcode,
                                                    MaterialName = productName,
                                                    MaterialUid = Guid.NewGuid().ToString(),
                                                    Note = filePath,
                                                    Result = "0001",
                                                    SubMaterialsInfo = barcode,
                                                    ProcessName = "分板"
                                                }
                                            }
                                        };

                                        return toPostDetails.ToArray();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    return new PostDetail[0];
                };
            }
        }

        #endregion

        #region 136检测数据推送

        private readonly List<string> _lastGuid = new List<string>();
        private readonly object _lock136GetData = new object();

        private Func<PostDetail[]> CheckDataTransfer(DateTime startTime, DateTime endTime)
        {
            return () =>
            {
                //var sql = string.Format(
                //    "select ItemId,MaterialNo,MaterialLotNo,MaterialUid,MaterialBarcode,MaterialCustomerBarcode,SubMaterialsInfo,DeviceNo,DeviceName,Result,CheckData,MaterialName,CreateTime,Note from SyProductionCheckData where CreateTime>= '{0}' and CreateTime <= '{1}' and DeviceNo not in ('IN1414000001','IN1414000002','IN1414000003','IN1414000005','IN1414000006','IN1414000007','IN1414000008','IN1415000001','IN1415000002','IN1415000003','IN1415000005','IN1415000006','IN1415000007','IN1415000008','IN1415000004','IN1411000003','IN1411000007','IN1411000010','IN4304000008','IN1411000003','IN1411000007','IN1411000010','IN1423000008','IN4304000193')",
                //    startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));

                const int pastMinutes = 60;
                var sql = string.Format(
                    "select ItemId,MaterialNo,MaterialLotNo,MaterialUid,MaterialBarcode,MaterialCustomerBarcode,SubMaterialsInfo,DeviceNo,DeviceName,Result,CheckData,MaterialName,CreateTime,Note from SyProductionCheckData where CreateTime>= dateadd(minute,-{0},GETDATE()) and DeviceNo not in ('IN1414000001','IN1414000002','IN1414000003','IN1414000005','IN1414000006','IN1414000007','IN1414000008','IN1415000001','IN1415000002','IN1415000003','IN1415000005','IN1415000006','IN1415000007','IN1415000008','IN1415000004','IN1411000003','IN1411000007','IN1411000010','IN4304000008','IN1411000003','IN1411000007','IN1411000010','IN1423000008','IN4304000193')", pastMinutes);
                var error = string.Empty;

                var lastGuidTemp = new List<string>();
                lastGuidTemp.AddRange(_lastGuid);
                if (_lastGuid.Count >= 50000)
                    _lastGuid.Clear();

                lock (_lock136GetData)
                {
                    try
                    {
                        HikDbHelperSql.RemoteSqlIp = "192.168.0.136";
                        HikDbHelperSql.DatabaseName = "PLMS";
                        HikDbHelperSql.SqlPassword = "123456";
                        var dt = HikDbHelperSql.GetDataTable(sql, ref error);
                        var toPostDatas = new List<PostDetail>();

                        if (dt.DefaultView.Count > 0)
                        {
                            var repeatCount = 0;

                            for (var i = 0; i < dt.DefaultView.Count; i++)
                            {
                                var itemId = dt.DefaultView[i]["ItemId"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["ItemId"].ToString();

                                if (!string.IsNullOrEmpty(itemId) && !_lastGuid.Contains(itemId))
                                    _lastGuid.Add(itemId);

                                if (string.IsNullOrEmpty(itemId) || lastGuidTemp.Contains(itemId))
                                {
                                    repeatCount++;
                                    continue;
                                }

                                var materialNo = dt.DefaultView[i]["MaterialNo"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["MaterialNo"].ToString();
                                var materialLotNo = dt.DefaultView[i]["MaterialLotNo"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["MaterialLotNo"].ToString();
                                var materialUid = dt.DefaultView[i]["MaterialUid"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["MaterialUid"].ToString();
                                var materialBarcode = dt.DefaultView[i]["MaterialBarcode"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["MaterialBarcode"].ToString();
                                var materialCustomerBarcode = dt.DefaultView[i]["MaterialCustomerBarcode"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["MaterialCustomerBarcode"].ToString();
                                var subMaterialsInfo = dt.DefaultView[i]["SubMaterialsInfo"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["SubMaterialsInfo"].ToString();
                                var deviceNo = dt.DefaultView[i]["DeviceNo"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["DeviceNo"].ToString();
                                var deviceName = dt.DefaultView[i]["DeviceName"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["DeviceName"].ToString();
                                var result = dt.DefaultView[i]["Result"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["Result"].ToString();
                                var checkData = dt.DefaultView[i]["CheckData"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["CheckData"].ToString();
                                var materialName = dt.DefaultView[i]["MaterialName"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["MaterialName"].ToString();
                                var createTime = dt.DefaultView[i]["CreateTime"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["CreateTime"].ToString();
                                var note = dt.DefaultView[i]["note"] == null
                                    ? string.Empty
                                    : dt.DefaultView[i]["Note"].ToString();

                                var postPackage = new UploadPackage
                                {
                                    ProcessName = string.Empty,
                                    MaterialNo = materialNo,
                                    MaterialLotNo = materialLotNo,
                                    MaterialUid = materialUid,
                                    MaterialBarcode = materialBarcode,
                                    MaterialCustomerBarcode = materialCustomerBarcode,
                                    SubMaterialsInfo = subMaterialsInfo,
                                    DeviceNo = deviceNo,
                                    DeviceName = deviceName,
                                    Result = result,
                                    CheckData = checkData,
                                    MaterialName = materialName,
                                    CheckDateTiem = createTime,
                                    Note = note
                                };

                                var postDetail = new PostDetail
                                {
                                    Config = Config,
                                    NewCreatedFilePath = "192.168.0.136",
                                    PostData = postPackage,
                                    IsNeedDeleteOld = false
                                };

                                toPostDatas.Add(postDetail);
                            }

                            OnPushMsgDataTransferEvent(
                                string.Format("查询136服务器共有数据'{0}'条，其中重复的有'{1}'条，查询的时间段从'{2}'至'{3}'", dt.DefaultView.Count, repeatCount, endTime, endTime.AddMinutes(-pastMinutes)));

                            if (toPostDatas.Any())
                            {
                                OnPushMsgDataTransferEvent(
                                    string.Format("本次待上传的136服务器数据共'{0}'条，查询的时间段从'{1}'至'{2}'", toPostDatas.Count, endTime, endTime.AddMinutes(-pastMinutes)));
                                return toPostDatas.ToArray();
                            }

                            OnPushMsgDataTransferEvent(string.Format("未查询到136服务器的数据，查询的时间段从'{0}'至'{1}'，查询语句为：{2}", startTime, endTime, sql));
                            return null;
                        }
                    }
                    catch (Exception e)
                    {
                        OnPushMsgDataTransferEvent("查询136服务器数据失败：" + e.Message);
                        return null;
                    }

                    OnPushMsgDataTransferEvent(string.IsNullOrEmpty(error)
                        ? string.Format("未查询到136服务器的数据，查询的时间段从'{0}'至'{1}'，查询语句为：{2}", startTime, endTime, sql)
                        : string.Format("未查询到136服务器的数据，查询出现异常：'{0}',查询的时间段从'{1}'至'{2}'，查询语句为：{3}", error, startTime,
                            endTime, sql));
                    return null;
                }
            };
        }

        internal class CheckDataUploadPackage
        {
            public string ItemId { get; set; }
            public string MaterialNo { get; set; }
            public string MaterialLotNo { get; set; }
            public string MaterialUid { get; set; }
            public string MaterialBarcode { get; set; }
            public string MaterialCustomerBarcode { get; set; }
            public string SubMaterialsInfo { get; set; }
            public string DeviceNo { get; set; }
            public string DeviceName { get; set; }
            public string Result { get; set; }
            public string CheckData { get; set; }
            public string MaterialName { get; set; }
            public string CreateTime { get; set; }
            public string Note { get; set; }
        }

        #endregion
    }
}
