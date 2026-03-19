using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CommonUtility;

namespace TestBatchUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("测试SyProductionSaveCheckData批量上传功能");
            
            // 确保服务已初始化
            SyProductionSaveCheckData.StartSyProductionSaveCheckData();
            
            // 创建测试数据
            for (int i = 0; i < 30; i++) // 创建30条测试数据
            {
                var barcodes = new List<string> { $"TEST{i:D6}" };
                var checkData = new List<SyProductionSaveCheckData.CheckDataDetail>
                {
                    new SyProductionSaveCheckData.CheckDataDetail
                    {
                        Type = "测试类型",
                        ParaName = "测试参数",
                        Range = "测试范围",
                        Value = "测试值",
                        Result = "测试结果"
                    }
                };
                
                // 调用SaveData方法，这应该会因为URL不可达而失败，从而触发保存到CSV的逻辑
                SyProductionSaveCheckData.SaveData(
                    isOk: true,
                    isByPass: false,
                    deviceNo: $"TEST{i:D3}",
                    materialName: "测试材料",
                    productNo: $"TEST-PROD-{i:D3}",
                    barcodes: barcodes,
                    checkData: checkData,
                    creater: "测试用户",
                    totalCount: i + 1
                );
            }
            
            Console.WriteLine("已提交30条测试数据，等待5秒查看结果...");
            Thread.Sleep(5000);
            
            // 检查CSV文件是否存在
            var csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FailedPosts.csv");
            if (File.Exists(csvFilePath))
            {
                Console.WriteLine($"CSV文件已创建: {csvFilePath}");
                Console.WriteLine("文件内容:");
                Console.WriteLine(File.ReadAllText(csvFilePath));
            }
            else
            {
                Console.WriteLine("CSV文件未创建");
            }
            
            Console.WriteLine("测试完成。按任意键退出...");
            Console.ReadKey();
        }
    }
}