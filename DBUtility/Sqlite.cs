using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace DBUtility
{
    /// <summary>
    /// 使用方法：
    /// using System.Data;
    /// using sqlite;
    /// Sqlite ms = new Sqlite("test.sqlite");
    /// string sql = "select * from `posts` where `id`=@id";
    /// Dictionary<string, object> param = new Dictionary<string, object>();
    /// param.Add("@id", 1);
    /// DataRow[] rows = ms.GetRows(sql, param);
    /// </summary>
    public class Sqlite
    {
        private readonly string _dbpath;

        private SQLiteConnection _conn;

        /// <summary>
        /// SQLite连接
        /// </summary>
        private SQLiteConnection Conn
        {
            get
            {
                if (_conn == null)
                {
                    _conn = new SQLiteConnection(
                        string.Format("Data Source={0};Version=3;",
                            _dbpath
                        ));
                    _conn.Open();
                }
                return _conn;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbpath">sqlite数据库文件路径，相对/绝对路径</param>
        public Sqlite(string dbpath)
        {
            _dbpath = Path.IsPathRooted(dbpath) ? dbpath : string.Format("{0}{1}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase, dbpath);
        }

        /// <summary>
        /// 获取多行
        /// </summary>
        /// <param name="sql">执行sql</param>
        /// <param name="param">sql参数</param>
        /// <returns>多行结果</returns>
        public DataRow[] GetRows(string sql, Dictionary<string, object> param = null)
        {
            var sqliteParam = new List<SQLiteParameter>();
            if (param != null)
                sqliteParam.AddRange(param.Select(row => new SQLiteParameter(row.Key, row.Value.ToString())));
            var dt = ExecuteDataTable(sql, sqliteParam.ToArray());
            return dt.Select();
        }

        /// <summary>
        /// 获取单行
        /// </summary>
        /// <param name="sql">执行sql</param>
        /// <param name="param">sql参数</param>
        /// <returns>单行数据</returns>
        public DataRow GetRow(string sql, Dictionary<string, object> param = null)
        {
            var rows = GetRows(sql, param);
            return rows[0];
        }

        /// <summary>
        /// 获取字段
        /// </summary>
        /// <param name="sql">执行sql</param>
        /// <param name="param">sql参数</param>
        /// <returns>字段数据</returns>
        public object GetOne(string sql, Dictionary<string, object> param = null)
        {
            var row = GetRow(sql, param);
            return row[0];
        }

        /// <summary>
        /// SQLite增删改
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="param">所需参数</param>
        /// <returns>所受影响的行数</returns>
        public int Query(string sql, Dictionary<string, object> param = null)
        {
            var sqliteParam = new List<SQLiteParameter>();

            if (param != null)
                sqliteParam.AddRange(param.Select(row => new SQLiteParameter(row.Key, row.Value.ToString())));

            return ExecuteNonQuery(sql, sqliteParam.ToArray());
        }

        /// <summary>
        /// SQLite增删改
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">所需参数</param>
        /// <returns>所受影响的行数</returns>
        private int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
        {
            System.Data.Common.DbTransaction transaction = Conn.BeginTransaction();
            var command = new SQLiteCommand(Conn) { CommandText = sql };
            if (parameters != null)
                command.Parameters.AddRange(parameters);
            var affectedRows = command.ExecuteNonQuery();
            transaction.Commit();

            return affectedRows;
        }

        /// <summary>
        /// SQLite查询
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">所需参数</param>
        /// <returns>结果DataTable</returns>
        private DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
        {
            var data = new DataTable();

            var command = new SQLiteCommand(sql, Conn);

            if (parameters != null)
                command.Parameters.AddRange(parameters);
            var adapter = new SQLiteDataAdapter(command);
            adapter.Fill(data);

            return data;
        }

        /// <summary>
        /// 查询数据库表信息
        /// </summary>
        /// <returns>数据库表信息DataTable</returns>
        public DataTable GetSchema()
        {
            var data = Conn.GetSchema("TABLES");
            return data;
        }
    }
}
