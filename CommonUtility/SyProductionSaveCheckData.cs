using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonUtility
{
    // 添加FailedPostData类定义
    public class FailedPostData
    {
        public string Id { get; set; }
        public string PostData { get; set; }
        public string PostUrl { get; set; }
        public DateTime FailedTime { get; set; }
        public int RetryCount { get; set; }
        public DateTime LastRetryTime { get; set; }
    }

    public static class SyProductionSaveCheckData
    {
        private static readonly object LockLotNo = new object();

        /// <summary>
        /// 添加CSV文件路径
        /// </summary>
        private static readonly string CsvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FailedPosts.csv");

        /// <summary>
        /// 添加用于存储失败数据的列表（内存中缓存）
        /// </summary>
        private static readonly List<FailedPostData> FailedDataList = new List<FailedPostData>();

        /// <summary>
        /// 添加线程同步锁
        /// </summary>
        private static readonly object FailedDataLock = new object();

        /// <summary>
        /// 添加定时重试任务
        /// </summary>
        private static Timer _retryTimer;

        /// <summary>
        /// 添加一个标志，确保只初始化一次
        /// </summary>
        private static bool _isInitialized = false;

        /// <summary>
        /// 启动SyProductionSaveCheckData服务
        /// </summary>
        public static void StartSyProductionSaveCheckData()
        {
            // 通过调用EnsureInitialized确保服务已初始化
            EnsureInitialized();
        }

        /// <summary>
        /// 初始化服务，包括加载之前保存的失败数据和启动定时重试任务
        /// </summary>
        private static async void InitializeService()
        {
            // 确保只初始化一次
            if (_isInitialized)
                return;

            _isInitialized = true;

            try
            {
                // 确保CSV文件目录存在
                var directory = Path.GetDirectoryName(CsvFilePath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                if (!File.Exists(CsvFilePath))
                {
                    var _config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ",",
                        HasHeaderRecord = !File.Exists(CsvFilePath), // 仅首次写入时添加表头
                        BufferSize = 65536,
                        Mode = CsvMode.RFC4180 // 严格兼容RFC4180标准
                    };
                    using (var _currentWriter = new StreamWriter(CsvFilePath, true, Encoding.UTF8, 65536))
                    using (var _currentCsv = new CsvWriter(_currentWriter, _config))
                    {
                        _currentCsv.WriteHeader<UploadPackage>();
                        await _currentCsv.NextRecordAsync();
                        await _currentWriter.FlushAsync();
                    }
                }

                // 加载之前保存的失败数据
                LoadFailedPostsFromCsv();

                // 初始化定时重试任务（每5分钟重试一次）
                _retryTimer = new Timer(RetryFailedPosts, null, TimeSpan.FromSeconds(35), TimeSpan.FromSeconds(35));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"初始化失败POST数据处理服务时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 确保服务已初始化
        /// </summary>
        private static void EnsureInitialized()
        {
            if (!_isInitialized)
            {
                lock (FailedDataLock)
                {
                    if (!_isInitialized)
                    {
                        InitializeService();
                    }
                }
            }
        }

        /// <summary>
        /// 从CSV文件加载之前保存的失败数据
        /// </summary>
        private static void LoadFailedPostsFromCsv()
        {
            try
            {
                // 如果CSV文件不存在，直接返回
                if (!File.Exists(CsvFilePath))
                    return;

                // 读取CSV文件中的失败数据
                using (var reader = new StreamReader(CsvFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = new List<FailedPostData>();
                    while (csv.Read())
                    {
                        try
                        {
                            var record = csv.GetRecord<FailedPostData>();
                            if (record != null)
                            {
                                records.Add(record);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"读取CSV记录时出错: {ex.Message}");
                        }
                    }

                    lock (FailedDataLock)
                    {
                        FailedDataList.Clear();
                        FailedDataList.AddRange(records);
                    }
                }

                Console.WriteLine($"成功加载 {FailedDataList.Count} 条失败的POST数据");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载失败的POST数据时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 将失败数据保存到CSV文件
        /// </summary>
        private static void SaveFailedPostsToCsv()
        {
            var backupPath = string.Empty;

            try
            {
                // 备份现有文件（如果存在）
                if (File.Exists(CsvFilePath))
                {
                    backupPath = CsvFilePath + ".bak";
                    try
                    {
                        File.Copy(CsvFilePath, backupPath, true);
                    }
                    catch (Exception backupEx)
                    {
                        Console.WriteLine($"备份CSV文件时出错: {backupEx.Message}");
                    }
                }

                // 写入最新的失败数据到CSV文件
                using (var writer = new StreamWriter(CsvFilePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    lock (FailedDataLock)
                    {
                        csv.WriteRecords(FailedDataList);
                    }
                }

                try
                {
                    if (!string.IsNullOrEmpty(backupPath) && File.Exists(backupPath))
                        File.Delete(backupPath);
                }
                catch (Exception)
                {

                }

                Console.WriteLine($"成功保存 {FailedDataList.Count} 条失败的POST数据到CSV文件");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存失败的POST数据到CSV文件时出错: {ex.Message}");
            }
        }

        public static async void SaveData(
            bool isOk, bool isByPass, string deviceNo, string materialName, string productNo, List<string> barcodes, List<CheckDataDetail> checkData, string creater, int totalCount, string materialGroupBarcode = "")
        {
            // 确保服务已初始化
            EnsureInitialized();

            await Task.Run(() =>
            {
                try
                {
                    lock (LockLotNo)
                    {
                        var itemId = Guid.NewGuid().ToString();
                        var materialNo = productNo;
                        var materialLotNo = string.Format("{0}{1}", DateTime.Now.ToString("yyyMMdd"), totalCount.ToString().PadLeft(4, '0'));

                        string materialUid;
                        string materialBarcode;
                        string materialCustomerBarcode;
                        var subMaterialsInfo = string.Empty;

                        var device = deviceNo;
                        var checkDataJsonStr = JsonConvert.SerializeObject(checkData);
                        var createTime = DateTime.Now;

                        string result;
                        if (!isByPass)
                            result = isOk ? "0001" : "0002";
                        else
                            result = isOk ? "9001" : "9002";

                        if (barcodes.Any())
                        {
                            materialUid = !string.IsNullOrEmpty(barcodes[0]) ? barcodes[0] : materialLotNo;
                            materialBarcode = !string.IsNullOrEmpty(barcodes[0]) ? barcodes[0] : materialLotNo;
                            materialCustomerBarcode = !string.IsNullOrEmpty(barcodes[0])
                                ? barcodes[0]
                                : materialLotNo;

                            subMaterialsInfo =
                                barcodes.Where(b => !string.IsNullOrEmpty(b))
                                    .Aggregate(subMaterialsInfo, (current, b) => current + b + "|");

                            subMaterialsInfo = subMaterialsInfo.TrimEnd('|');
                        }
                        else
                        {
                            materialUid = materialLotNo;
                            materialBarcode = materialLotNo;
                            materialCustomerBarcode = materialLotNo;
                        }

                        var uploadPackage = new UploadPackage
                        {
                            MaterialNo = materialNo,
                            MaterialLotNo = materialLotNo,
                            MaterialUid = materialUid,
                            MaterialBarcode = materialBarcode,
                            MaterialCustomerBarcode = materialCustomerBarcode,
                            SubMaterialsInfo = subMaterialsInfo,
                            DeviceNo = deviceNo,
                            DeviceName = deviceNo,
                            Result = result,
                            CheckData = checkDataJsonStr,
                            MaterialName = materialName,
                            CreateTime = createTime.ToString(CultureInfo.InvariantCulture),
                            Note = creater,
                            MaterialGroupBarcode = materialGroupBarcode
                        };

                        var postJsonStr = JsonConvert.SerializeObject(new[] { uploadPackage });

                        const string postUrl = "http://192.168.0.136:8021/api/Other/UploadProductionCheckData";

                        var client = new RestClient(postUrl);
                        var request = new RestRequest(Method.POST);
                        request.Timeout = 2500;
                        request.AddHeader("cache-control", "no-cache");
                        request.AddParameter("application/json", postJsonStr, ParameterType.RequestBody);
                        var response = client.Execute(request);

                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            // 当POST请求失败时，将数据保存到CSV文件
                            SaveFailedPostData(postJsonStr, postUrl);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        /// <summary>
        /// 保存失败的POST请求数据到CSV文件
        /// </summary>
        /// <param name="postData">POST数据</param>
        /// <param name="postUrl">POST URL</param>
        private static void SaveFailedPostData(string postData, string postUrl)
        {
            try
            {
                var failedData = new FailedPostData
                {
                    Id = Guid.NewGuid().ToString(),
                    PostData = postData,
                    PostUrl = postUrl,
                    FailedTime = DateTime.Now,
                    RetryCount = 0,
                    LastRetryTime = DateTime.Now
                };

                // 添加到内存缓存
                lock (FailedDataLock)
                    FailedDataList.Add(failedData);

                // 保存到CSV文件
                SaveFailedPostsToCsv();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存失败的POST数据时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 定时重试失败的POST请求
        /// </summary>
        /// <param name="state">状态对象</param>
        private static void RetryFailedPosts(object state)
        {
            try
            {
                List<FailedPostData> failedDataCopy;
                lock (FailedDataLock)
                {
                    failedDataCopy = new List<FailedPostData>(FailedDataList);
                }

                var successfullySent = new List<FailedPostData>();
                var hasRetried = false;

                // 批量处理，每次处理25条记录
                const int batchSize = 25;
                for (int i = 0; i < failedDataCopy.Count; i += batchSize)
                {
                    // 获取当前批次的数据
                    var batch = failedDataCopy.Skip(i).Take(batchSize).ToList();

                    // 将当前批次的所有PostData打包成数组
                    var batchPostDataArray = batch.Select(failedData => failedData.PostData).ToArray();

                    var listPackage = new List<UploadPackage>();
                    foreach (var b in batchPostDataArray)
                        listPackage.AddRange(JsonConvert.DeserializeObject<UploadPackage[]>(b));

                    var batchPostDataJson = JsonConvert.SerializeObject(listPackage.ToArray());

                    // 使用批次中的第一条记录的URL作为请求URL（假设同一批次的URL相同）
                    if (batch.Count > 0)
                    {
                        var firstFailedData = batch[0];
                        try
                        {
                            // 增加所有批次数据的重试次数
                            foreach (var failedData in batch)
                            {
                                failedData.RetryCount++;
                                failedData.LastRetryTime = DateTime.Now;
                                hasRetried = true;
                            }

                            lock (LockLotNo)
                            {
                                var client = new RestClient(firstFailedData.PostUrl);
                                var request = new RestRequest(Method.POST);
                                request.Timeout = 2500;
                                request.AddHeader("cache-control", "no-cache");
                                request.AddParameter("application/json", batchPostDataJson, ParameterType.RequestBody);
                                var response = client.Execute(request);

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    // 如果重试成功，添加到成功列表中以便后续删除
                                    successfullySent.AddRange(batch);
                                    Console.WriteLine($"成功重试发送失败的POST请求，批次包含 {batch.Count} 条记录");
                                }

                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            //// 如果批次请求失败，则逐个处理批次中的每条记录
                            //foreach (var failedData in batch)
                            //{
                            //    try
                            //    {
                            //        // 增加重试次数
                            //        failedData.RetryCount++;
                            //        failedData.LastRetryTime = DateTime.Now;
                            //        hasRetried = true;

                            //        var client = new RestClient(failedData.PostUrl);
                            //        var request = new RestRequest(Method.POST);
                            //        request.AddHeader("cache-control", "no-cache");
                            //        request.AddParameter("application/json", failedData.PostData, ParameterType.RequestBody);
                            //        var response = client.Execute(request);

                            //        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            //        {
                            //            // 如果重试成功，添加到成功列表中以便后续删除
                            //            successfullySent.Add(failedData);
                            //            Console.WriteLine($"成功重试发送失败的POST请求，ID: {failedData.Id}");
                            //        }
                            //    }
                            //    catch (Exception singleEx)
                            //    {
                            //        Console.WriteLine($"重试失败的POST请求时出错 (ID: {failedData.Id}): {singleEx.Message}");
                            //    }
                            //}
                        }
                    }
                }

                // 从内存缓存中移除成功发送的数据
                if (successfullySent.Count > 0)
                {
                    lock (FailedDataLock)
                    {
                        foreach (var successData in successfullySent)
                        {
                            FailedDataList.Remove(successData);
                        }
                    }

                    // 更新CSV文件
                    SaveFailedPostsToCsv();
                    Console.WriteLine($"已从失败列表中移除 {successfullySent.Count} 条成功重试的POST请求");
                }
                else if (hasRetried)
                {
                    Console.WriteLine("本次重试没有成功发送任何POST请求");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"重试失败的POST请求时出错: {ex.Message}");
            }
        }

        public class CheckDataDetail
        {
            public string Type;
            public string ParaName;
            public string Range;
            public string Value;
            public string Result;
        }

        internal class UploadPackage
        {
            public string MaterialName { get; set; } = string.Empty;
            public string MaterialUid { get; set; } = string.Empty;
            public string MaterialBarcode { get; set; } = string.Empty;
            public string MaterialCustomerBarcode { get; set; } = string.Empty;
            public string SubMaterialsInfo { get; set; } = string.Empty;
            public string DeviceNo { get; set; } = string.Empty;
            public string CheckData { get; set; } = string.Empty;
            public string CreateTime { get; set; } = string.Empty;
            public string Result { get; set; } = string.Empty;
            public string Note { get; set; } = string.Empty;
            public string MaterialNo { get; set; } = string.Empty;
            public string MaterialLotNo { get; set; } = string.Empty;
            public string DeviceName { get; set; } = string.Empty;
            public string MaterialGroupBarcode { get; set; } = string.Empty;
        }

        private readonly static object _lockget = new object();

        public static bool TryGetSySequenceDailyData(string materialNo, out string date, out int sequence)
        {
            lock (_lockget)
            {
                var postUrl = "http://192.168.0.136:8021/api/Other/GetSySequenceDailyData?MaterialNo=" + materialNo;

                var client = new RestClient(postUrl);
                var request = new RestRequest(Method.POST);
                request.Timeout = 2500;
                request.AddHeader("cache-control", "no-cache");
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        dynamic data = JObject.Parse(response.Content);
                        if (data != null && data.code == 200 && data.info == "响应成功")
                        {
                            sequence = data.data.seqNo;
                            date = data.data.date;
                            return true;
                        }
                        else
                        {
                            date = string.Empty;
                            sequence = int.MinValue;
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        date = string.Empty;
                        sequence = int.MinValue;
                        return false;
                    }
                }
                else
                {
                    date = string.Empty;
                    sequence = int.MinValue;
                    return false;
                }
            }
        }

        public static bool TryGetSySequenceGlobalData(string materialNo, out string date, out int sequence)
        {
            lock (_lockget)
            {
                var postUrl = "http://192.168.0.136:8021/api/Other/GetSySequenceGlobalData?MaterialNo=" + materialNo;

                var client = new RestClient(postUrl);
                var request = new RestRequest(Method.POST);
                request.Timeout = 2500;
                request.AddHeader("cache-control", "no-cache");
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        dynamic data = JObject.Parse(response.Content);
                        if (data != null && data.code == 200 && data.info == "响应成功")
                        {
                            sequence = data.data.seqNo;
                            date = data.data.date;
                            return true;
                        }
                        else
                        {
                            date = string.Empty;
                            sequence = int.MinValue;
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        date = string.Empty;
                        sequence = int.MinValue;
                        return false;
                    }
                }
                else
                {
                    date = string.Empty;
                    sequence = int.MinValue;
                    return false;
                }
            }
        }
    }
}