using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using DBUtility;

namespace CheckSystem.YfasLogo
{
    public static class YfasLogoSqlHelper
    {
        private static readonly object SqlLocker = new object();
        private const string Dbpath = @"YfasLogoConfig\YfasLogoDb";
        private static readonly Sqlite Sqlite = new Sqlite(Dbpath);

        public static List<LogoConfigModel> GetConfigModels(
            string sql = "select * from ContourConfig order by id asc")
        {
            if (Sqlite == null)
                return new List<LogoConfigModel>();
            lock (SqlLocker)
            {
                var getData = Sqlite.GetRows(sql);

                return getData.Select(t => new LogoConfigModel
                {
                    Id = int.Parse(t.ItemArray[0].ToString()),
                    CreateTime = DateTime.Parse(t.ItemArray[1].ToString()),
                    Object = t.ItemArray[2].ToString(),
                    Type = t.ItemArray[3].ToString(),
                    Position = t.ItemArray[4].ToString(),
                    ProductionName = t.ItemArray[5].ToString(),
                    MinGray = Math.Round(double.Parse(t.ItemArray[6].ToString()), 0),
                    MaxGray = Math.Round(double.Parse(t.ItemArray[7].ToString()), 0),
                    MinHomo = Math.Round(double.Parse(t.ItemArray[8].ToString()), 0),
                    MaxHomo = Math.Round(double.Parse(t.ItemArray[9].ToString()), 0),
                    MinAverage = Math.Round(double.Parse(t.ItemArray[10].ToString()), 0),
                    MaxAverage = Math.Round(double.Parse(t.ItemArray[11].ToString()), 0),
                    MinAllAverage = Math.Round(double.Parse(t.ItemArray[12].ToString()), 0),
                    MaxAllAverage = Math.Round(double.Parse(t.ItemArray[13].ToString()), 0),
                }).ToList();
            }
        }

        public static void AddConfig(string type, string objectStr, string position, string productName)
        {
            if (Sqlite == null)
                return;

            lock (SqlLocker)
            {
                var insert =
                    string.Format("insert into ContourConfig (createTime,Object,Type,Position,ProductName,MinGray,MaxGray,MinHomogenization,MaxHomogenization) values ('{0}','{1}','{2}','{3}','{4}','0','0','0','0')",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), objectStr, type, position, productName);
                Sqlite.Query(insert);
            }
        }

        public static void AddConfig(LogoConfigModel model)
        {
            if (Sqlite == null)
                return;

            lock (SqlLocker)
            {
                var insert =
                    string.Format("insert into ContourConfig (createTime,Object,Type,Position,ProductName,MinGray,MaxGray,MinHomogenization,MaxHomogenization,MinAverageGray,MaxAverageGray) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), model.Object, model.Type, model.Position, model.ProductionName, model.MinGray, model.MaxGray, model.MinHomo, model.MaxHomo, model.MinAverage, model.MaxAverage);
                Sqlite.Query(insert);
            }
        }

        public static void UpdateConfig(LogoConfigModel model)
        {
            if (Sqlite == null)
                return;

            lock (SqlLocker)
            {
                var update = string.Format(
                    "update ContourConfig set createTime = '{0}', Object = '{1}', Type = '{2}', Position = '{3}', ProductName = '{4}', MinGray = '{5}', MaxGray = '{6}', MinHomogenization = '{7}', MaxHomogenization = '{8}', MinAverageGray = '{9}', MaxAverageGray = '{10}', MinAverageAllGray = '{11}', MaxAverageAllGray = '{12}' where id = {13}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), model.Object, model.Type, model.Position,
                    model.ProductionName, model.MinGray, model.MaxGray, model.MinHomo, model.MaxHomo, model.MinAverage, model.MaxAverage, model.MinAllAverage, model.MaxAllAverage, model.Id);
                Sqlite.Query(update);
            }
        }

        public static void ClearAllConfigData(string where = "")
        {
            if (Sqlite == null)
                return;

            lock (SqlLocker)
            {
                var sql = "DELETE FROM ContourConfig";

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }

                Sqlite.Query(sql);
            }
        }

        public static void ClearAllDataHistory(string where = "")
        {
            if (Sqlite == null)
                return;

            lock (SqlLocker)
            {
                var sql = "DELETE FROM DataHistory";

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }

                Sqlite.Query(sql);
            }
        }

        public static List<LogoDataLogMode> GetDataModels(string sql)
        {
            if (Sqlite == null)
                return new List<LogoDataLogMode>();

            lock (SqlLocker)
            {
                //var str = "select * from LoadBoxErrorInfo";
                var getData = Sqlite.GetRows(sql);

                var listModels = new List<LogoDataLogMode>();

                foreach (var row in getData)
                {
                    if (row != null)
                    {
                        var model = new LogoDataLogMode();

                        if (row["id"] != null && row["id"].ToString() != "")
                        {
                            model.id = int.Parse(row["id"].ToString());
                        }
                        if (row["CreateTime"] != null)
                        {
                            model.CreateTime = row["CreateTime"].ToString();
                        }
                        if (row["ProductName"] != null)
                        {
                            model.ProductName = row["ProductName"].ToString();
                        }
                        if (row["Result"] != null)
                        {
                            model.Result = row["Result"].ToString();
                        }
                        if (row["Uid"] != null)
                        {
                            model.Uid = row["Uid"].ToString();
                        }
                        if (row["Curr"] != null)
                        {
                            model.Curr = row["Curr"].ToString();
                        }
                        if (row["Volt"] != null)
                        {
                            model.Volt = row["Volt"].ToString();
                        }
                        if (row["OuterRingAverage"] != null)
                        {
                            model.OuterRingAverage = row["OuterRingAverage"].ToString();
                        }
                        if (row["OuterRingUnif"] != null)
                        {
                            model.OuterRingUnif = row["OuterRingUnif"].ToString();
                        }
                        if (row["V1Average"] != null)
                        {
                            model.V1Average = row["V1Average"].ToString();
                        }
                        if (row["V2Average"] != null)
                        {
                            model.V2Average = row["V2Average"].ToString();
                        }
                        if (row["V3Average"] != null)
                        {
                            model.V3Average = row["V3Average"].ToString();
                        }
                        if (row["VUnif"] != null)
                        {
                            model.VUnif = row["VUnif"].ToString();
                        }
                        if (row["VAverage"] != null)
                        {
                            model.VAverage = row["VAverage"].ToString();
                        }
                        if (row["W1Average"] != null)
                        {
                            model.W1Average = row["W1Average"].ToString();
                        }
                        if (row["W2Average"] != null)
                        {
                            model.W2Average = row["W2Average"].ToString();
                        }
                        if (row["W3Average"] != null)
                        {
                            model.W3Average = row["W3Average"].ToString();
                        }
                        if (row["W4Average"] != null)
                        {
                            model.W4Average = row["W4Average"].ToString();
                        }
                        if (row["W5Average"] != null)
                        {
                            model.W5Average = row["W5Average"].ToString();
                        }
                        if (row["WAverage"] != null)
                        {
                            model.WAverage = row["WAverage"].ToString();
                        }
                        if (row["WUnif"] != null)
                        {
                            model.WUnif = row["WUnif"].ToString();
                        }
                        if (row["InnerOval1Average"] != null)
                        {
                            model.InnerOval1Average = row["InnerOval1Average"].ToString();
                        }
                        if (row["InnerOval2Average"] != null)
                        {
                            model.InnerOval2Average = row["InnerOval2Average"].ToString();
                        }
                        if (row["InnerOval3Average"] != null)
                        {
                            model.InnerOval3Average = row["InnerOval3Average"].ToString();
                        }
                        if (row["InnerOval4Average"] != null)
                        {
                            model.InnerOval4Average = row["InnerOval4Average"].ToString();
                        }
                        if (row["InnerOval5Average"] != null)
                        {
                            model.InnerOval5Average = row["InnerOval5Average"].ToString();
                        }
                        if (row["InnerOval6Average"] != null)
                        {
                            model.InnerOval6Average = row["InnerOval6Average"].ToString();
                        }
                        if (row["InnerOval7Average"] != null)
                        {
                            model.InnerOval7Average = row["InnerOval7Average"].ToString();
                        }
                        if (row["InnerOval8Average"] != null)
                        {
                            model.InnerOval8Average = row["InnerOval8Average"].ToString();
                        }
                        if (row["InnerOval9Average"] != null)
                        {
                            model.InnerOval9Average = row["InnerOval9Average"].ToString();
                        }
                        if (row["InnerOval10Average"] != null)
                        {
                            model.InnerOval10Average = row["InnerOval10Average"].ToString();
                        }
                        if (row["InnerOval11Average"] != null)
                        {
                            model.InnerOval11Average = row["InnerOval11Average"].ToString();
                        }
                        if (row["InnerOval12Average"] != null)
                        {
                            model.InnerOval12Average = row["InnerOval12Average"].ToString();
                        }
                        if (row["InnerOval13Average"] != null)
                        {
                            model.InnerOval13Average = row["InnerOval13Average"].ToString();
                        }
                        if (row["InnerOval14Average"] != null)
                        {
                            model.InnerOval14Average = row["InnerOval14Average"].ToString();
                        }
                        if (row["InnerOval15Average"] != null)
                        {
                            model.InnerOval15Average = row["InnerOval15Average"].ToString();
                        }
                        if (row["InnerOval16Average"] != null)
                        {
                            model.InnerOval16Average = row["InnerOval16Average"].ToString();
                        }
                        if (row["InnerOvalAverage"] != null)
                        {
                            model.InnerOvalAverage = row["InnerOvalAverage"].ToString();
                        }
                        if (row["InnerOvalUnif"] != null)
                        {
                            model.InnerOvalUnif = row["InnerOvalUnif"].ToString();
                        }

                        listModels.Add(model);
                    }
                }

                return listModels;
            }
        }

        public static void AddData(LogoDataLogMode model)
        {
            if (Sqlite == null)
                return;

            lock (SqlLocker)
            {
                //model.Uid = Guid.NewGuid().ToString();
                const string sqlGetCount = "select count(id) as GetCount from DataHistory";

                var getCountRows = Sqlite.GetRows(sqlGetCount);
                var getCount = 0;
                if (getCountRows.Any())
                    getCount = int.Parse(getCountRows[0]["GetCount"].ToString());

                const int maxCount = 5 * 10000;

                if (getCount >= maxCount)
                    Sqlite.Query(string.Format("Delete from DataHistory where rowid IN (Select rowid from DataHistory limit {0})", 100));

                var strSql = new StringBuilder();
                strSql.Append("insert into DataHistory(");
                strSql.Append(
                    "CreateTime,ProductName,Result,Uid,Curr,Volt,OuterRingAverage,OuterRingUnif,V1Average,V2Average,V3Average,VUnif,VAverage,W1Average,W2Average,W3Average,W4Average,W5Average,WAverage,WUnif,InnerOval1Average,InnerOval2Average,InnerOval3Average,InnerOval4Average,InnerOval5Average,InnerOval6Average,InnerOval7Average,InnerOval8Average,InnerOval9Average,InnerOval10Average,InnerOval11Average,InnerOval12Average,InnerOval13Average,InnerOval14Average,InnerOval15Average,InnerOval16Average,InnerOvalAverage,InnerOvalUnif)");
                strSql.Append(" values (");
                strSql.Append(
                    "@CreateTime,@ProductName,@Result,@Uid,@Curr,@Volt,@OuterRingAverage,@OuterRingUnif,@V1Average,@V2Average,@V3Average,@VUnif,@VAverage,@W1Average,@W2Average,@W3Average,@W4Average,@W5Average,@WAverage,@WUnif,@InnerOval1Average,@InnerOval2Average,@InnerOval3Average,@InnerOval4Average,@InnerOval5Average,@InnerOval6Average,@InnerOval7Average,@InnerOval8Average,@InnerOval9Average,@InnerOval10Average,@InnerOval11Average,@InnerOval12Average,@InnerOval13Average,@InnerOval14Average,@InnerOval15Average,@InnerOval16Average,@InnerOvalAverage,@InnerOvalUnif)");
                strSql.Append(";select LAST_INSERT_ROWID()");
                SQLiteParameter[] parameters =
                {
                    new SQLiteParameter("@CreateTime", DbType.String),
                    new SQLiteParameter("@ProductName", DbType.String),
                    new SQLiteParameter("@Result", DbType.String),
                    new SQLiteParameter("@Uid", DbType.String),
                    new SQLiteParameter("@Curr", DbType.String),
                    new SQLiteParameter("@Volt", DbType.String),
                    new SQLiteParameter("@OuterRingAverage", DbType.String),
                    new SQLiteParameter("@OuterRingUnif", DbType.String),
                    new SQLiteParameter("@V1Average", DbType.String),
                    new SQLiteParameter("@V2Average", DbType.String),
                    new SQLiteParameter("@V3Average", DbType.String),
                    new SQLiteParameter("@VUnif", DbType.String),
                    new SQLiteParameter("@VAverage", DbType.String),
                    new SQLiteParameter("@W1Average", DbType.String),
                    new SQLiteParameter("@W2Average", DbType.String),
                    new SQLiteParameter("@W3Average", DbType.String),
                    new SQLiteParameter("@W4Average", DbType.String),
                    new SQLiteParameter("@W5Average", DbType.String),
                    new SQLiteParameter("@WAverage", DbType.String),
                    new SQLiteParameter("@WUnif", DbType.String),
                    new SQLiteParameter("@InnerOval1Average", DbType.String),
                    new SQLiteParameter("@InnerOval2Average", DbType.String),
                    new SQLiteParameter("@InnerOval3Average", DbType.String),
                    new SQLiteParameter("@InnerOval4Average", DbType.String),
                    new SQLiteParameter("@InnerOval5Average", DbType.String),
                    new SQLiteParameter("@InnerOval6Average", DbType.String),
                    new SQLiteParameter("@InnerOval7Average", DbType.String),
                    new SQLiteParameter("@InnerOval8Average", DbType.String),
                    new SQLiteParameter("@InnerOval9Average", DbType.String),
                    new SQLiteParameter("@InnerOval10Average", DbType.String),
                    new SQLiteParameter("@InnerOval11Average", DbType.String),
                    new SQLiteParameter("@InnerOval12Average", DbType.String),
                    new SQLiteParameter("@InnerOval13Average", DbType.String),
                    new SQLiteParameter("@InnerOval14Average", DbType.String),
                    new SQLiteParameter("@InnerOval15Average", DbType.String),
                    new SQLiteParameter("@InnerOval16Average", DbType.String),
                    new SQLiteParameter("@InnerOvalAverage", DbType.String),
                    new SQLiteParameter("@InnerOvalUnif", DbType.String)
                };
                parameters[0].Value = model.CreateTime;
                parameters[1].Value = model.ProductName;
                parameters[2].Value = model.Result;
                parameters[3].Value = model.Uid;
                parameters[4].Value = model.Curr;
                parameters[5].Value = model.Volt;
                parameters[6].Value = model.OuterRingAverage;
                parameters[7].Value = model.OuterRingUnif;
                parameters[8].Value = model.V1Average;
                parameters[9].Value = model.V2Average;
                parameters[10].Value = model.V3Average;
                parameters[11].Value = model.VUnif;
                parameters[12].Value = model.VAverage;
                parameters[13].Value = model.W1Average;
                parameters[14].Value = model.W2Average;
                parameters[15].Value = model.W3Average;
                parameters[16].Value = model.W4Average;
                parameters[17].Value = model.W5Average;
                parameters[18].Value = model.WAverage;
                parameters[19].Value = model.WUnif;
                parameters[20].Value = model.InnerOval1Average;
                parameters[21].Value = model.InnerOval2Average;
                parameters[22].Value = model.InnerOval3Average;
                parameters[23].Value = model.InnerOval4Average;
                parameters[24].Value = model.InnerOval5Average;
                parameters[25].Value = model.InnerOval6Average;
                parameters[26].Value = model.InnerOval7Average;
                parameters[27].Value = model.InnerOval8Average;
                parameters[28].Value = model.InnerOval9Average;
                parameters[29].Value = model.InnerOval10Average;
                parameters[30].Value = model.InnerOval11Average;
                parameters[31].Value = model.InnerOval12Average;
                parameters[32].Value = model.InnerOval13Average;
                parameters[33].Value = model.InnerOval14Average;
                parameters[34].Value = model.InnerOval15Average;
                parameters[35].Value = model.InnerOval16Average;
                parameters[36].Value = model.InnerOvalAverage;
                parameters[37].Value = model.InnerOvalUnif;

                // 创建一个Dictionary<string, object>
                var parametersDictionary = parameters.ToDictionary(param => param.ParameterName, param => param.Value);

                // 遍历SQLiteParameterCollection，将其转化为字典
                Sqlite.Query(strSql.ToString(), parametersDictionary);
            }
        }

        public static int GetTodayCount(bool isOk, string productionName)
        {
            if (Sqlite == null)
                return 0;

            lock (SqlLocker)
            {
                var sql =
                    string.Format(
                        "select count(id) as GetCount from DataHistory where CreateTime >= '{0} 00:00:00' and CreateTime <= '{0} 23:59:59' and ProductName = '{1}' and Result = '{2}'",
                        DateTime.Now.ToString("yyyy-MM-dd"), productionName, isOk ? "OK" : "NG");

                var rows = Sqlite.GetRows(sql);
                return rows.Any() ? int.Parse(rows[0]["GetCount"].ToString()) : 0;
            }
        }

        public class LogoConfigModel
        {
            private int _id;
            private string _object;
            private DateTime _createTime;
            private string _type;
            private string _position;
            private string _productionName;
            private double _minGray;
            private double _maxGray;
            private double _minHomo;
            private double _maxHomo;
            private double _minAverage;
            private double _maxAverage;
            private double _minAllAverage;
            private double _maxAllAverage;

            public int Id
            {
                get { return _id; }
                set { _id = value; }
            }

            public DateTime CreateTime
            {
                get { return _createTime; }
                set { _createTime = value; }
            }

            /// <summary>
            /// ROI参数序列化字符串
            /// </summary>
            public string Object
            {
                get { return _object; }
                set { _object = value; }
            }

            /// <summary>
            /// ROI类型，Oval或Line
            /// </summary>
            public string Type
            {
                get { return _type; }
                set { _type = value; }
            }

            /// <summary>
            /// ROI位置索引+1
            /// </summary>
            public string Position
            {
                get { return _position; }
                set { _position = value; }
            }

            /// <summary>
            /// 产品名称，前灯或后灯
            /// </summary>
            public string ProductionName
            {
                get { return _productionName; }
                set { _productionName = value; }
            }

            /// <summary>
            /// ROI各自最小值
            /// </summary>
            public double MinGray
            {
                get { return _minGray; }
                set { _minGray = value; }
            }

            /// <summary>
            /// ROI各自最大值
            /// </summary>
            public double MaxGray
            {
                get { return _maxGray; }
                set { _maxGray = value; }
            }

            /// <summary>
            /// ROI整体均匀度最小值，整体最小/整体最大
            /// </summary>
            public double MinHomo
            {
                get { return _minHomo; }
                set { _minHomo = value; }
            }

            /// <summary>
            /// ROI整体均匀度最大值，整体最小/整体最大
            /// </summary>
            public double MaxHomo
            {
                get { return _maxHomo; }
                set { _maxHomo = value; }
            }

            /// <summary>
            /// ROI各自平均值的最小值
            /// </summary>
            public double MinAverage
            {
                get { return _minAverage; }
                set { _minAverage = value; }
            }

            /// <summary>
            /// ROI各自平均值的最大值
            /// </summary>
            public double MaxAverage
            {
                get { return _maxAverage; }
                set { _maxAverage = value; }
            }

            /// <summary>
            /// ROI整体平均值的最小值
            /// </summary>
            public double MinAllAverage
            {
                get { return _minAllAverage; }
                set { _minAllAverage = value; }
            }

            /// <summary>
            /// ROI整体平均值的最大值
            /// </summary>
            public double MaxAllAverage
            {
                get { return _maxAllAverage; }
                set { _maxAllAverage = value; }
            }
        }

        public class LogoDataLogMode
        {
            public LogoDataLogMode()
            {
                _createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _uid = Guid.NewGuid().ToString().Substring(24, 12);
            }

            #region Model
            private int _id;
            private string _createtime;
            private string _productname;
            private string _result;
            private string _uid;
            private string _curr;
            private string _volt;
            private string _outerringaverage;
            private string _outerringunif;
            private string _v1average;
            private string _v2average;
            private string _v3average;
            private string _vunif;
            private string _vaverage;
            private string _w1average;
            private string _w2average;
            private string _w3average;
            private string _w4average;
            private string _w5average;
            private string _waverage;
            private string _wunif;
            private string _inneroval1average;
            private string _inneroval2average;
            private string _inneroval3average;
            private string _inneroval4average;
            private string _inneroval5average;
            private string _inneroval6average;
            private string _inneroval7average;
            private string _inneroval8average;
            private string _inneroval9average;
            private string _inneroval10average;
            private string _inneroval11average;
            private string _inneroval12average;
            private string _inneroval13average;
            private string _inneroval14average;
            private string _inneroval15average;
            private string _inneroval16average;
            private string _innerovalaverage;
            private string _innerovalunif;

            /// <summary>
            /// 
            /// </summary>
            public int id
            {
                set { _id = value; }
                get { return _id; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string CreateTime
            {
                set { _createtime = value; }
                get { return _createtime; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string ProductName
            {
                set { _productname = value; }
                get { return _productname; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string Result
            {
                set { _result = value; }
                get { return _result; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string Uid
            {
                set { _uid = value; }
                get { return _uid; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string Curr
            {
                set { _curr = value; }
                get { return _curr; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string Volt
            {
                set { _volt = value; }
                get { return _volt; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string OuterRingAverage
            {
                set { _outerringaverage = value; }
                get { return _outerringaverage; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string OuterRingUnif
            {
                set { _outerringunif = value; }
                get { return _outerringunif; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string V1Average
            {
                set { _v1average = value; }
                get { return _v1average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string V2Average
            {
                set { _v2average = value; }
                get { return _v2average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string V3Average
            {
                set { _v3average = value; }
                get { return _v3average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string VUnif
            {
                set { _vunif = value; }
                get { return _vunif; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string VAverage
            {
                set { _vaverage = value; }
                get { return _vaverage; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string W1Average
            {
                set { _w1average = value; }
                get { return _w1average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string W2Average
            {
                set { _w2average = value; }
                get { return _w2average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string W3Average
            {
                set { _w3average = value; }
                get { return _w3average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string W4Average
            {
                set { _w4average = value; }
                get { return _w4average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string W5Average
            {
                set { _w5average = value; }
                get { return _w5average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string WAverage
            {
                set { _waverage = value; }
                get { return _waverage; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string WUnif
            {
                set { _wunif = value; }
                get { return _wunif; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval1Average
            {
                set { _inneroval1average = value; }
                get { return _inneroval1average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval2Average
            {
                set { _inneroval2average = value; }
                get { return _inneroval2average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval3Average
            {
                set { _inneroval3average = value; }
                get { return _inneroval3average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval4Average
            {
                set { _inneroval4average = value; }
                get { return _inneroval4average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval5Average
            {
                set { _inneroval5average = value; }
                get { return _inneroval5average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval6Average
            {
                set { _inneroval6average = value; }
                get { return _inneroval6average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval7Average
            {
                set { _inneroval7average = value; }
                get { return _inneroval7average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval8Average
            {
                set { _inneroval8average = value; }
                get { return _inneroval8average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval9Average
            {
                set { _inneroval9average = value; }
                get { return _inneroval9average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval10Average
            {
                set { _inneroval10average = value; }
                get { return _inneroval10average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval11Average
            {
                set { _inneroval11average = value; }
                get { return _inneroval11average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval12Average
            {
                set { _inneroval12average = value; }
                get { return _inneroval12average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval13Average
            {
                set { _inneroval13average = value; }
                get { return _inneroval13average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval14Average
            {
                set { _inneroval14average = value; }
                get { return _inneroval14average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval15Average
            {
                set { _inneroval15average = value; }
                get { return _inneroval15average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOval16Average
            {
                set { _inneroval16average = value; }
                get { return _inneroval16average; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOvalAverage
            {
                set { _innerovalaverage = value; }
                get { return _innerovalaverage; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string InnerOvalUnif
            {
                set { _innerovalunif = value; }
                get { return _innerovalunif; }
            }
            #endregion Model
        }

        public enum ProductType
        {
            [Description("前灯")]
            FrontLamp,

            [Description("后灯")]
            RearLamp
        }

        public enum CheckType
        {
            [Description("电流")]
            Curr,

            [Description("电压")]
            Volt,

            [Description("外圈平均值")]
            OuterOvalAverage,

            [Description("外圈均匀度")]
            OuterOvalUnif,

            [Description("V-1平均值")]
            V1Average,

            [Description("V-2平均值")]
            V2Average,

            [Description("V-3平均值")]
            V3Average,

            [Description("V均匀度")]
            VUnif,

            [Description("V整体平均值")]
            VAvarage,

            [Description("“16点”-1平均值")]
            Inner1OvalAverage,

            [Description("“16点”-2平均值")]
            Inner2OvalAverage,

            [Description("“16点”-3平均值")]
            Inner3OvalAverage,

            [Description("“16点”-4平均值")]
            Inner4OvalAverage,

            [Description("“16点”-5平均值")]
            Inner5OvalAverage,

            [Description("“16点”-6平均值")]
            Inner6OvalAverage,

            [Description("“16点”-7平均值")]
            Inner7OvalAverage,

            [Description("“16点”-8平均值")]
            Inner8OvalAverage,

            [Description("“16点”-9平均值")]
            Inner9OvalAverage,

            [Description("“16点”-10平均值")]
            Inner10OvalAverage,

            [Description("“16点”-11平均值")]
            Inner11OvalAverage,

            [Description("“16点”-12平均值")]
            Inner12OvalAverage,

            [Description("“16点”-13平均值")]
            Inner13OvalAverage,

            [Description("“16点”-14平均值")]
            Inner14OvalAverage,

            [Description("“16点”-15平均值")]
            Inner15OvalAverage,

            [Description("“16点”-16平均值")]
            Inner16OvalAverage,

            [Description("“16点”均匀度")]
            InnerUnif,

            [Description("“16点”整体平均值")]
            InnerAvarage,

            [Description("W-1平均值")]
            W1Average,

            [Description("W-2平均值")]
            W2Average,

            [Description("W-3平均值")]
            W3Average,

            [Description("W-4平均值")]
            W4Average,

            [Description("W-5平均值")]
            W5Average,

            [Description("W均匀度")]
            WUnif,

            [Description("W整体平均值")]
            WAverage,
        }

        public enum CheckPostion
        {
            [Description("外圈")]
            OuterOval,

            [Description("V-1")]
            V1,

            [Description("V-2")]
            V2,

            [Description("V-3")]
            V3,

            [Description("W-1")]
            W1,

            [Description("W-2")]
            W2,

            [Description("W-3")]
            W3,

            [Description("W-4")]
            W4,

            [Description("W-5")]
            W5,

            [Description("“16点”-1")]
            InnerOval1,

            [Description("“16点”-2")]
            InnerOval2,

            [Description("“16点”-3")]
            InnerOval3,

            [Description("“16点”-4")]
            InnerOval4,

            [Description("“16点”-5")]
            InnerOval5,

            [Description("“16点”-6")]
            InnerOval6,

            [Description("“16点”-7")]
            InnerOval7,

            [Description("“16点”-8")]
            InnerOval8,

            [Description("“16点”-9")]
            InnerOval9,

            [Description("“16点”-10")]
            InnerOval10,

            [Description("“16点”-11")]
            InnerOval11,

            [Description("“16点”-12")]
            InnerOval12,

            [Description("“16点”-13")]
            InnerOval13,

            [Description("“16点”-14")]
            InnerOval14,

            [Description("“16点”-15")]
            InnerOval15,

            [Description("“16点”-16")]
            InnerOval16
        }
    }
}


