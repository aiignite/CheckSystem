using CheckSystem.SmtForms.DataUploader;
using Emgu.CV.Ocl;
using MiniExcelLibs;
using NationalInstruments.Restricted;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CheckSystem.SmtForms.DataUploader.MyQueueEvent;

namespace CheckSystem.SmtForms
{
    public partial class FrmKohYoungReUpload : Form
    {
        private List<string> _ipList = new List<string>
        {
            "192.168.10.115#IN1415000005",
            //"192.168.10.65#IN1415000006",
            //"192.168.10.56#IN1415000007",
            //"192.168.10.125#IN1415000008",
        };

        internal class AoiResult
        {
            public string PCBGUID;
            public string PCBResultRepair;
            public string PCBModel;
            public string BarCode;
            public string ReviewStartDateTime;
            public string ALLBarCode;
        }

        internal class AoiDetail
        {
            public string DetailGUID;
            public string Defect;
            public string ReDefect;
            public string Failure;
            public string Repair;
            public string PCBGUID;
            public string ComponentGUID;
            public string uname;
            public string ArrayIndex;
            public string InspType;
        }

        internal class AoiLineInfo
        {
            public string LineID;
            public string LineName;
            public string Server;
            public string DataBase;
            public List<AoiResult> AoiResult = new List<AoiResult>();
            public List<AoiDetail> AoiDetail = new List<AoiDetail>();
            public UploadConfigModel Config;
        }

        private List<AoiResult> _aoiResultList5 = new List<AoiResult>();
        private List<AoiResult> _aoiResultList7 = new List<AoiResult>();
        private List<AoiResult> _aoiResultList8 = new List<AoiResult>();

        private List<AoiDetail> _aoiDetailsList5 = new List<AoiDetail>();
        private List<AoiDetail> _aoiDetailsList7 = new List<AoiDetail>();
        private List<AoiDetail> _aoiDetailsList8 = new List<AoiDetail>();

        private List<AoiLineInfo> AoiLines = new List<AoiLineInfo>();

        private Thread _th;
        public FrmKohYoungReUpload()
        {
            InitializeComponent();
            var newFilePath = @"Y:\Projects-2022\SMT-贴片机\高银AOI_7线20241218数据备份.xlsx";
            //var row5Result = MiniExcel.Query(newFilePath, excelType: ExcelType.XLSX, sheetName: "5线Result").ToList();
            //var row5Details = MiniExcel.Query(newFilePath, excelType: ExcelType.XLSX, sheetName: "5线Details").ToList();
            var row7Result = MiniExcel.Query(newFilePath, excelType: ExcelType.XLSX, sheetName: "7线Result").ToList();
            var row7Details = MiniExcel.Query(newFilePath, excelType: ExcelType.XLSX, sheetName: "7线Details").ToList();
            //var row8Result = MiniExcel.Query(newFilePath, excelType: ExcelType.XLSX, sheetName: "8线Result").ToList();
            //var row8Details = MiniExcel.Query(newFilePath, excelType: ExcelType.XLSX, sheetName: "8线Details").ToList();

            //for (var i = 1; i < row5Result.Count; i++)
            //{
            //    var row = row5Result[i];

            //    _aoiResultList5.Add(new AoiResult
            //    {
            //        PCBGUID = row.A,
            //        PCBResultRepair = row.B,
            //        PCBModel = row.C,
            //        BarCode = row.D,
            //        ReviewStartDateTime = row.E,
            //        ALLBarCode = row.F,
            //    });
            //}
            for (var i = 1; i < row7Result.Count; i++)
            {
                var row = row7Result[i];

                _aoiResultList7.Add(new AoiResult
                {
                    PCBGUID = row.A,
                    PCBResultRepair = row.B,
                    PCBModel = row.C,
                    BarCode = row.D,
                    ReviewStartDateTime = row.E,
                    ALLBarCode = row.F,
                });
            }
            //for (var i = 1; i < row8Result.Count; i++)
            //{
            //    var row = row8Result[i];

            //    _aoiResultList8.Add(new AoiResult
            //    {
            //        PCBGUID = row.A,
            //        PCBResultRepair = row.B,
            //        PCBModel = row.C,
            //        BarCode = row.D,
            //        ReviewStartDateTime = row.E,
            //        ALLBarCode = row.F,
            //    });
            //}

            //for (var i = 1; i < row5Details.Count; i++)
            //{
            //    var row = row5Details[i];

            //    _aoiDetailsList5.Add(new AoiDetail
            //    {
            //        DetailGUID = row.A,
            //        Defect = row.B,
            //        ReDefect = row.C,
            //        Failure = row.D,
            //        Repair = row.E,
            //        PCBGUID = row.F,
            //        ComponentGUID = row.G,
            //        uname = row.H,
            //        ArrayIndex = row.I,
            //        InspType = row.J,
            //    });
            //}
            for (var i = 1; i < row7Details.Count; i++)
            {
                var row = row7Details[i];

                _aoiDetailsList7.Add(new AoiDetail
                {
                    DetailGUID = row.A,
                    Defect = row.B,
                    ReDefect = row.C,
                    Failure = row.D,
                    Repair = row.E,
                    PCBGUID = row.F,
                    ComponentGUID = row.G,
                    uname = row.H,
                    ArrayIndex = row.I,
                    InspType = row.J,
                });
            }
            //for (var i = 1; i < row8Details.Count; i++)
            //{
            //    var row = row8Details[i];

            //    _aoiDetailsList8.Add(new AoiDetail
            //    {
            //        DetailGUID = row.A,
            //        Defect = row.B,
            //        ReDefect = row.C,
            //        Failure = row.D,
            //        Repair = row.E,
            //        PCBGUID = row.F,
            //        ComponentGUID = row.G,
            //        uname = row.H,
            //        ArrayIndex = row.I,
            //        InspType = row.J,
            //    });
            //}

            AoiLines.Add(new AoiLineInfo
            {
                LineID = "IN1415000005",
                Server = "192.168.10.115",
                DataBase = "KY_Result_202500",
                Config = new UploadConfigModel
                {
                    CreateTime = DateTime.Now,
                    DeviceNo = "IN1415000005",
                    Folder = "",
                    Id = 0,
                    LineName = "SMT线_5线_SEEYAO_AOI_Zenith Alpha_001_KOH YOUNG",
                    Type = "KouYoungAOI"
                }
            });
            AoiLines.Add(new AoiLineInfo
            {
                LineID = "IN1415000007",
                Server = "192.168.10.56",
                DataBase = "KY_Result_202500",
                Config = new UploadConfigModel
                {
                    CreateTime = DateTime.Now,
                    DeviceNo = "IN1415000007",
                    Folder = "",
                    Id = 0,
                    LineName = "SMT线_7线_SEEYAO_AOI_Zenith Alpha_003_KOH YOUNG",
                    Type = "KouYoungAOI"
                }
            });
            AoiLines.Add(new AoiLineInfo
            {
                LineID = "IN1415000008",
                Server = "192.168.10.125",
                DataBase = "KY_Result_202500",
                Config = new UploadConfigModel
                {
                    CreateTime = DateTime.Now,
                    DeviceNo = "IN1415000008",
                    Folder = "",
                    Id = 0,
                    LineName = "SMT线_8线_SEEYAO_AOI_Zenith Alpha_004_KOH YOUNG",
                    Type = "KouYoungAOI"
                }
            });

            AoiLines[0].AoiResult.AddRange(_aoiResultList5);
            AoiLines[0].AoiDetail.AddRange(_aoiDetailsList5);

            AoiLines[1].AoiResult.AddRange(_aoiResultList7);
            AoiLines[1].AoiDetail.AddRange(_aoiDetailsList7);

            AoiLines[2].AoiResult.AddRange(_aoiResultList8);
            AoiLines[2].AoiDetail.AddRange(_aoiDetailsList8);

            Load += FrmKohYoungReUpload_Load;
            Closed += FrmKohYoungReUpload_Closed;
        }

        private void FrmKohYoungReUpload_Closed(object sender, EventArgs e)
        {
            if (_th == null)
                return;
            _th.Abort();
            _th.Join();
        }

        private void FrmKohYoungReUpload_Load(object sender, EventArgs e)
        {
            _th = new Thread(MainWork) { IsBackground = true };
            _th.Start();

            foreach (var t in AoiLines)
            {
                Task.Factory.StartNew(() =>
                {
                    foreach (var item in t.AoiResult)
                    {
                        EqueueTask(DataConvert(item, t.AoiDetail, t.Config, t.Server, t.DataBase).Invoke());
                    }
                });
            }

            //foreach (var t in _ipList)
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        var txt = string.Format(@"{0}\SMT配置文件\{1}.txt", Program.SysDir, t.Split('#')[0]);
            //        var deviceNo = t.Split('#')[1];

            //        var fs = new FileStream(txt, FileMode.Open); // 初始化文件流

            //        var sr = new StreamReader(fs); // 初始化StreamReader            
            //        var lines = sr.ReadToEnd(); // 读取全文        
            //        sr.Close(); // 关闭流
            //        fs.Close(); // 关闭流

            //        var readLinse = lines.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            //        foreach (var l in readLinse)
            //        {
            //            EqueueTask(KouYoungAoiConvert(l, "KY_Result_202416", t.Split('#')[0], "SA", "koh1234", deviceNo));
            //        }
            //    });
            //}
        }

        private Func<PostDetail[]> DataConvert(AoiResult aoiResult, List<AoiDetail> aoiDetailsList, UploadConfigModel config, string server, string dataBase)
        {
            return () =>
            {
                var rowPcb = aoiResult;
                var pd = new PostDetail[1];

                var pcbGuid = rowPcb.PCBGUID;
                var pcbResultRepair = rowPcb.PCBResultRepair == null ? string.Empty : rowPcb.PCBResultRepair;
                var pcbModel = rowPcb.PCBModel == null ? string.Empty : rowPcb.PCBModel;
                var barCode = rowPcb.BarCode == null ? string.Empty : rowPcb.BarCode;
                var reviewStartDateTime = rowPcb.ReviewStartDateTime == null ? string.Empty : rowPcb.ReviewStartDateTime;
                var allBarcode = rowPcb.ALLBarCode == null ? string.Empty : rowPcb.ALLBarCode;

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
                    Config = config,
                    NewCreatedFilePath =
                        string.Format("{0};{1};{2}", server, dataBase, pcbGuid), //kouYoungAoiTbAoiResult.PCBGUID,
                    PostData = new UploadPackage
                    {
                        MaterialName = pcbModel,
                        MaterialUid = pcbGuid,
                        MaterialBarcode = string.IsNullOrEmpty(uid) ? string.Empty : uid,
                        MaterialCustomerBarcode = string.IsNullOrEmpty(uid) ? string.Empty : uid,
                        SubMaterialsInfo = string.IsNullOrEmpty(subMaterialsInfo) ? string.Empty : subMaterialsInfo,
                        DeviceNo = config.DeviceNo,
                        CheckDateTiem = reviewStartDateTime,
                        Result = pcbResultRepair == "13000000" ? "0002" : "0001",
                        Note = string.Format("{0};{1}", server, dataBase)
                    }
                };

                var dtTbAoiDefectDetail = aoiDetailsList.FindAll(f => f.PCBGUID.ToLower() == pcbGuid.ToLower());

                var tempUploadDetail = new List<UploadDetail>();

                for (var a = 0; a < dtTbAoiDefectDetail.Count; a++)
                {
                    var rowDetail = dtTbAoiDefectDetail[a];

                    if (rowDetail.Defect != null && !string.IsNullOrEmpty(rowDetail.Defect) &&
                        rowDetail.DetailGUID != null && !string.IsNullOrEmpty(rowDetail.DetailGUID) &&
                        rowDetail.ReDefect != null && !string.IsNullOrEmpty(rowDetail.ReDefect) &&
                        rowDetail.Failure != null && !string.IsNullOrEmpty(rowDetail.Failure) &&
                        rowDetail.Repair != null && !string.IsNullOrEmpty(rowDetail.Repair) &&
                        rowDetail.ComponentGUID != null && !string.IsNullOrEmpty(rowDetail.ComponentGUID) &&
                        rowDetail.uname != null && !string.IsNullOrEmpty(rowDetail.uname) &&
                        rowDetail.ArrayIndex != null && !string.IsNullOrEmpty(rowDetail.ArrayIndex) &&
                        rowDetail.InspType != null && !string.IsNullOrEmpty(rowDetail.InspType))
                    {
                        var defect = rowDetail.Defect;
                        var reDefect = rowDetail.ReDefect;
                        var failure = rowDetail.Failure;
                        var repair = rowDetail.Repair;

                        var uname = rowDetail.uname;
                        var arrayIndex = rowDetail.ArrayIndex;

                        var inspType = rowDetail.InspType;

                        var tempUploadPackage = new UploadDetail
                        {
                            Type = "AOI",
                            ParaName = defect != "12000000" ? (string.IsNullOrEmpty(defect) ? string.Format("{0},{1}", arrayIndex, uname) : string.Format("{0},{1}_{2}", arrayIndex, uname, defect)) : string.Format("{0},{1}", arrayIndex, uname),//uname,
                            Range = rowDetail.DetailGUID,
                            Value = string.Format("{0},{1},{2}", repair == "0" ? "R" : "Y", inspType, defect),//defect,
                            Result = defect == "12000000" ? true.ToString() : false.ToString()
                        };
                        tempUploadDetail.Add(tempUploadPackage);
                    }
                }

                tempUploadDetail = new List<UploadDetail>(tempUploadDetail.OrderBy(oo => oo.Result));
                pd[0].PostData.CheckData = JsonConvert.SerializeObject(tempUploadDetail);

                return pd[0] != null ? pd : null;
            };
        }

        private void MainWork()
        {
            while (_th.IsAlive)
            {
                if (!_th.IsAlive)
                    break;

                PostDetail[] actions = null;

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

                            Thread.Sleep(1);
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

                        Thread.Sleep(1);
                    }
                    else
                    {
                        var toPostDatas = (from t in actions where t != null && t.PostData != null select t.PostData).ToList();
                        postApiAction.Invoke(toPostDatas);
                    }
                }
                else
                    _wh.WaitOne();
            }
        }

        private Func<PostDetail[]> KouYoungAoiConvert(string PCBGUID, string dateBase, string _server, string _uid, string _pwd, string deviceNo)
        {
            return () =>
            {
                try
                {
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

                        var dtTbAoiResult = getTableFunc(string.Format("select PCBGUID,PCBResultRepair,PCBModel,BarCode,ReviewStartDateTime,ALLBarCode from TB_AOIResult where PCBGUID = '{0}'", PCBGUID), connection);

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
                                    Config = new UploadConfigModel { CreateTime = DateTime.Now, DeviceNo = deviceNo, Folder = deviceNo, Id = 0, LineName = deviceNo, Type = "KouYoungAOI" },
                                    NewCreatedFilePath = string.Format("{0};{1};{2}", _server, dateBase, PCBGUID),//kouYoungAoiTbAoiResult.PCBGUID,
                                    PostData = new UploadPackage
                                    {
                                        MaterialName = pcbModel,
                                        MaterialUid = pcbGuid,
                                        MaterialBarcode = string.IsNullOrEmpty(uid) ? string.Empty : uid,
                                        MaterialCustomerBarcode = string.IsNullOrEmpty(uid) ? string.Empty : uid,
                                        SubMaterialsInfo = string.IsNullOrEmpty(subMaterialsInfo) ? string.Empty : subMaterialsInfo,
                                        DeviceNo = deviceNo,
                                        CheckDateTiem = reviewStartDateTime,
                                        Result = pcbResultRepair == "13000000" ? "0002" : "0001",
                                        Note = string.Format("{0};{1}", _server, dateBase)
                                    }
                                };

                                var dtTbAoiDefectDetail = getTableFunc(string.Format("select DetailGUID,Defect,ReDefect,Failure,Repair,TB_AOIDefectDetail.PCBGUID,TB_AOIDefectDetail.ComponentGUID,TB_AOIDefect.uname as uname,TB_AOIDefect.ArrayIndex as ArrayIndex from TB_AOIDefectDetail inner join TB_AOIDefect on TB_AOIDefectDetail.ComponentGUID=TB_AOIDefect.ComponentGUID where  TB_AOIDefectDetail.PCBGUID = '{0}'", PCBGUID), connection);

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
                                        rowDetail["ArrayIndex"] != null && !string.IsNullOrEmpty(rowDetail["ArrayIndex"].ToString()))
                                    {
                                        var defect = rowDetail["Defect"].ToString();
                                        var reDefect = rowDetail["ReDefect"].ToString();
                                        var failure = rowDetail["Failure"].ToString();
                                        var repair = rowDetail["Repair"].ToString();

                                        var uname = rowDetail["uname"].ToString();
                                        var arrayIndex = rowDetail["ArrayIndex"].ToString();

                                        var tempUploadPackage = new UploadDetail
                                        {
                                            Type = "AOI",
                                            ParaName = string.Format("{0},{1}", arrayIndex, uname),//uname,
                                            Range = rowDetail["DetailGUID"].ToString(),
                                            Value = string.Format("{0},{1},{2}", repair == "0" ? "R" : "Y", reDefect, defect),//defect,
                                            Result = defect == "12000000" ? true.ToString() : false.ToString()
                                        };
                                        tempUploadDetail.Add(tempUploadPackage);
                                    }
                                }

                                pd[0].PostData.CheckData = JsonConvert.SerializeObject(tempUploadDetail);
                            }
                        }

                        connection.Close();

                        return pd[0] != null ? pd : null;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            };
        }

        public void EqueueTask(PostDetail[] postDetail)
        {
            lock (_locker)
                _postApiTask.Enqueue(postDetail);

            _wh.Set();
        }

        private readonly Queue<PostDetail[]> _postApiTask = new Queue<PostDetail[]>();
        private readonly object _locker = new object();
        private readonly EventWaitHandle _wh = new AutoResetEvent(false);

        private delegate void UpdatetxtDelegate(string text);
        private void Updatetxt(string text)
        {
            var updateRichTextBoxDelegate = new UpdatetxtDelegate(Updatetxt);

            if (richTextBox1.InvokeRequired)
            {
                Invoke(updateRichTextBoxDelegate, text);
            }
            else
            {
                if (richTextBox1.Text.Length > 10 * 2500)
                    richTextBox1.Clear();

                richTextBox1.AppendText(text + Environment.NewLine);
                //uiTextBox4.Text = text;
            }
        }
    }
}
