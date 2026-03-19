using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace CommonUtility
{
    // 测试用的FailedPostData类
    public class FailedPostData
    {
        public string Id { get; set; }
        public string PostData { get; set; }
        public string PostUrl { get; set; }
        public DateTime FailedTime { get; set; }
        public int RetryCount { get; set; }
        public DateTime LastRetryTime { get; set; }
    }

    class TestCsvFunctionality
    {
        private static readonly string CsvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFailedPosts.csv");

        static void Main(string[] args)
        {
            Console.WriteLine("开始测试CSV功能...");

            // 创建测试数据
            var testData = new List<FailedPostData>
            {
                new FailedPostData
                {
                    Id = Guid.NewGuid().ToString(),
                    PostData = "{\"test\": \"data1\"}",
                    PostUrl = "http://example.com/api/test1",
                    FailedTime = DateTime.Now,
                    RetryCount = 0,
                    LastRetryTime = DateTime.Now
                },
                new FailedPostData
                {
                    Id = Guid.NewGuid().ToString(),
                    PostData = "{\"test\": \"data2\"}",
                    PostUrl = "http://example.com/api/test2",
                    FailedTime = DateTime.Now.AddMinutes(-10),
                    RetryCount = 1,
                    LastRetryTime = DateTime.Now.AddMinutes(-5)
                }
            };

            // 测试写入CSV
            Console.WriteLine("测试写入CSV文件...");
            TestWriteToCsv(testData);

            // 测试读取CSV
            Console.WriteLine("测试读取CSV文件...");
            TestReadFromCsv();

            Console.WriteLine("测试完成!");
            Console.ReadKey();
        }

        /// <summary>
        /// 测试写入CSV文件
        /// </summary>
        private static void TestWriteToCsv(List<FailedPostData> data)
        {
            try
            {
                using (var writer = new StreamWriter(CsvFilePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(data);
                }
                Console.WriteLine($"成功写入 {data.Count} 条测试数据到CSV文件");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"写入CSV文件时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 测试从CSV文件读取
        /// </summary>
        private static void TestReadFromCsv()
        {
            try
            {
                if (!File.Exists(CsvFilePath))
                {
                    Console.WriteLine("CSV文件不存在");
                    return;
                }

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

                    Console.WriteLine($"成功读取 {records.Count} 条测试数据从CSV文件");
                    foreach (var record in records)
                    {
                        Console.WriteLine($"  ID: {record.Id}, URL: {record.PostUrl}, 重试次数: {record.RetryCount}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取CSV文件时出错: {ex.Message}");
            }
        }
    }
}