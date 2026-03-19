using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonUtility.FileOperator
{
    public class CsvFileHelper<T> : IDisposable
    {
        private BlockingCollection<T> _dataQueue;
        private CsvConfiguration _config;
        private StreamWriter _currentWriter;
        private CsvWriter _currentCsv;
        private string _basePath;
        private Task _writingTask;
        private CancellationTokenSource _cts;
        private Timer _fileRotateTimer;
        private int _currentFileRowCount;
        private string _filePath;

        public CsvFileHelper(string basePath, int queueCapacity = 10000)
        {
            _basePath = basePath;
            _cts = new CancellationTokenSource();
            _dataQueue = new BlockingCollection<T>(queueCapacity);
            InitializeNewFile(queueCapacity);
            _fileRotateTimer = new Timer(_ => RotateFile(), null, TimeSpan.FromHours(1), TimeSpan.FromHours(1));
        }

        private bool _isDisposing;

        ~CsvFileHelper()
        {
            if (!_isDisposing)
            {
                _isDisposing = true;
                Dispose();
            }
        }

        /// <summary>
        /// 停止服务并释放资源
        /// </summary>
        public void Dispose()
        {
            _fileRotateTimer.Dispose();
            _cts.Cancel();
            _dataQueue.CompleteAdding();
            _writingTask?.Wait();
            _dataQueue.Dispose();
            _cts.Dispose();
        }

        private void InitializeNewFile(int queueCapacity = 10000)
        {
            try
            {
                _filePath = string.Format(@"{0}\{1}.csv", _basePath, DateTime.Now.ToString("yyyyMMdd_HH"));
                _currentWriter?.Dispose();
                _config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HasHeaderRecord = !File.Exists(_filePath), // 仅首次写入时添加表头
                    BufferSize = 65536,
                    Mode = CsvMode.RFC4180 // 严格兼容RFC4180标准
                };
                _currentWriter = new StreamWriter(_filePath, true, Encoding.UTF8, 65536);
                _currentCsv = new CsvWriter(_currentWriter, _config);
                _currentFileRowCount = 0;
            }
            catch (Exception)
            {

            }
        }

        private void RotateFile()
        {
            InitializeNewFile();
        }

        /// <summary>
        /// 启动异步写入服务
        /// </summary>
        public void Start()
        {
            _writingTask = Task.Run(async () =>
            {
                try
                {
                    //using (var writer = new StreamWriter(_filePath, true, Encoding.UTF8, 65536))
                    {
                        //using (var csv = new CsvWriter(writer, _config))
                        {
                            // 首次运行时写入表头
                            if (_config.HasHeaderRecord && _currentFileRowCount == 0)
                            {
                                _currentCsv.WriteHeader<T>();
                                await _currentCsv.NextRecordAsync();
                                await _currentWriter.FlushAsync();
                            }

                            // 实时消费队列数据
                            foreach (var data in _dataQueue.GetConsumingEnumerable(_cts.Token))
                            {
                                _currentCsv.WriteRecord(data);
                                await _currentCsv.NextRecordAsync();

                                _currentFileRowCount++;
                                await _currentWriter.FlushAsync();

                                //// 每1000条刷新一次缓冲区
                                //if (_currentFileRowCount % 1000 == 0)
                                //    await _currentWriter.FlushAsync();                               
                            }
                        }
                    }
                }
                catch (OperationCanceledException) { /* 正常退出 */ }
                catch (Exception ex)
                {
                    Console.WriteLine($"CSV写入错误: {ex.Message}");
                    // 可添加错误恢复逻辑
                }
            });
        }

        /// <summary>
        /// 添加数据到队列（线程安全）
        /// </summary>
        /// <param name="data"></param>
        public void EnqueueData(T data)
        {
            try
            {
                if (!_dataQueue.TryAdd(data, 50)) // 50ms超时
                {
                    Console.WriteLine("警告：队列已满，数据可能丢失");
                    // 可替换为其他处理策略
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("警告：" + ex.Message + "，数据可能丢失");
            }
        }
    }
}
