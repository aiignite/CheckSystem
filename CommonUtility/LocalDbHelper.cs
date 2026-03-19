using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommonUtility
{
    public static class LocalDbHelper
    {
        public static string SysDir = Directory.GetCurrentDirectory();
        private static string _filePath = string.Empty;
        private static string _dbName = @"IPMS_Local";
        private static int _maxCount = 10 * 1000;
        private static SqlSugarClient db = null;

        public static void InitSqLiteLocal()
        {
            _filePath = GetSql();
            if (!string.IsNullOrEmpty(_filePath))
            {
                if (CreateBase())
                {
                    CreateTable();
                    db.Open();
                }
            }
        }

        private static string GetSql()
        {
            var f = string.Format(@"{0}/LocalDB/{1}", Path.GetPathRoot(SysDir), _dbName);
            db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = $"Data Source={f}", // SQLite 连接字符串
                DbType = DbType.Sqlite, // 指定数据库类型为 SQLite
                IsAutoCloseConnection = true, // 自动关闭连接
                InitKeyType = InitKeyType.Attribute // 从实体特性中读取主键自增列信息
            });

            return f;
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        private static bool CreateBase()
        {
            try
            {
                return db.DbMaintenance.CreateDatabase();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void CreateTable() => db.CodeFirst.InitTables<manufactureCheckData>();

        public static bool IsRepeat(string barcode) => !(db is null) && db.Queryable<manufactureCheckData>().Any(f => f.productBarcode == barcode && f.checkResult == "0001");

        public static void InsertData(manufactureCheckData[] insertData)
        {
            try
            {
                var currentCount = db.Queryable<manufactureCheckData>().Count();

                // 执行备份并删除一周前数据的逻辑
                if (currentCount >= _maxCount)
                    BackupAndDeleteOldData(db);

                if (insertData is null || !insertData.Any())
                    return;

                db.Insertable(insertData).ExecuteCommand();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error when insert: {0}", ex.Message);
            }
        }

        /// <summary>
        /// 备份并删除一周前的数据
        /// </summary>
        /// <param name="db">SqlSugar客户端</param>
        private static void BackupAndDeleteOldData(SqlSugarClient db)
        {
            // 计算一周前的日期
            var oneWeekAgo = DateTime.Now.AddDays(-1).Date.ToString("yyyy-MM-dd HH:mm:ss");
            var driveLetter = Path.GetPathRoot(SysDir);
            var localFile = string.Format(@"{0}/LocalDB/Before_{1}_{2}_Backup_{3}.db", driveLetter, _dbName, DateTime.Now.ToString("yyyyMMdd"), HighPrecisionTimer.GetTimestamp());

            var isCopyOk = false;

            try
            {
                File.Copy(_filePath, localFile, true);
                isCopyOk = true;
            }
            catch (Exception)
            {
                isCopyOk = false;
            }

            if (isCopyOk)
            {
                try
                {
                    // 开启事务
                    db.Ado.BeginTran();

                    // 删除原表中的旧数据
                    var deletedCount = db.Deleteable<manufactureCheckData>().Where(it => !string.IsNullOrEmpty(it.createTime) && DateTime.Parse(it.createTime) < DateTime.Parse(oneWeekAgo)).ExecuteCommand();
                    Console.WriteLine($"已从原表删除 {deletedCount} 条旧数据。");

                    // 提交事务
                    db.Ado.CommitTran();
                    Console.WriteLine("备份并删除操作成功完成！");
                }
                catch (Exception ex)
                {
                    // 如果任何一步出错，回滚事务
                    db.Ado.RollbackTran();
                    Console.WriteLine($"操作失败，已回滚: {ex.Message}");
                }
            }
        }

        public static List<manufactureCheckData> QueryBySqlString(string sql)
        {
            try
            {
                var result = db.Ado.SqlQuery<manufactureCheckData>(sql);
                return result;
            }
            catch (Exception)
            {
                return new List<manufactureCheckData>();
            }
        }

        public class manufactureCheckData
        {
            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int Id { get; set; }

            public string taskNo { get; set; } = string.Empty;
            public string productNo { get; set; } = string.Empty;
            public string productUid { get; set; } = string.Empty;
            public string pcbaNo { get; set; } = string.Empty;
            public string pcbaBarcode { get; set; } = string.Empty;
            public string productBarcode { get; set; } = string.Empty;
            public string packageBarcode { get; set; } = string.Empty;
            public string processNo { get; set; } = string.Empty;
            public string checkData { get; set; } = string.Empty;
            public string checkDate { get; set; } = string.Empty;
            public string checkStaffNo { get; set; } = string.Empty;
            public string checkResult { get; set; } = string.Empty;
            public string creater { get; set; } = string.Empty;
            public string createTime { get; set; } = string.Empty;
            public string _checkDate { get; set; } = string.Empty;
            public string checkDataText { get; set; } = string.Empty;
        }
    }
}
