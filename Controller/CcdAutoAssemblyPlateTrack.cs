using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Controller
{
    public class CcdAutoAssemblyPlateTrack : ControllerBase
    {
        public static string SysDir = Directory.GetCurrentDirectory();
        private static string _filePath = string.Empty;
        private static string _dbName = @"PlateTrack";
        private static SqlSugarClient db = null;

        public CcdAutoAssemblyPlateTrack(string name) : base(name)
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

        ~CcdAutoAssemblyPlateTrack() => Dispose();

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

        public static void CreateTable() => db.CodeFirst.InitTables<TrackInfo>();

        public void SavePlateBarcode()
        {
            if (string.IsNullOrEmpty(PlateBarcode) || string.IsNullOrEmpty(ProductBarcode) || ProductPosition < 1)
            {
                return;
            }

            var isExist = db.Queryable<TrackInfo>().Any(it => it.PlateBarcode == PlateBarcode && it.ProductPosition == ProductPosition);
            if (!isExist)
                db.Insertable<TrackInfo>(new TrackInfo { PlateBarcode = PlateBarcode, ProductBarcode = ProductBarcode, IsOk = IsOk, ProductPosition = ProductPosition }).ExecuteCommand();
            else
                db.Updateable<TrackInfo>().SetColumns(it => new TrackInfo { ProductBarcode = ProductBarcode, IsOk = IsOk }).Where(it => it.PlateBarcode == PlateBarcode && it.ProductPosition == ProductPosition).ExecuteCommand();
        }

        public void ReadPlateBarcode()
        {
            ProductBarcode = string.Empty;
            IsOk = false;

            if (string.IsNullOrEmpty(PlateBarcode) || ProductPosition < 1)
                return;

            var getData = db.Queryable<TrackInfo>().First(f => f.ProductPosition == ProductPosition && f.PlateBarcode == PlateBarcode);
            if (getData != null)
            {
                ProductBarcode = getData.ProductBarcode;
                IsOk = getData.IsOk;
            }
        }

        public string PlateBarcode;

        public string ProductBarcode;

        public bool IsOk;

        public int ProductPosition;

        private class TrackInfo
        {
            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int Id { get; set; }

            public string PlateBarcode { get; set; } = string.Empty;

            public string ProductBarcode { get; set; } = string.Empty;

            public bool IsOk { get; set; }

            public int ProductPosition { get; set; }
        }

        #region Track Server

        private readonly CommonUtility.MyAsyncSocketServer _server = new CommonUtility.MyAsyncSocketServer();
        private int _productCount = 0;

        public void InitServer(int port, int productCount)
        {
            _productCount = productCount;
            _server.InitSocket(port);
            _server.Start();
            _server.OnPushSocketsToTcpServer += _server_OnPushSocketsToTcpServer;
        }

        private void _server_OnPushSocketsToTcpServer(CommonUtility.BaseSocket sockets)
        {
            if (sockets.Offset == 0)
                return;

            var header = "Query:";
            var buff = new byte[sockets.Offset];
            Array.Copy(sockets.RecBuffer, buff, buff.Length);
            var msg = Encoding.ASCII.GetString(buff, 0, buff.Length);
            if (msg.StartsWith(header) && msg.EndsWith("\r\n"))
            {
                var plateBarcode = msg.TrimEnd().Substring(header.Length, msg.Length - header.Length - 2);
                var ackData = new List<AckData>();

                if (db != null)
                {
                    for (int i = 0; i < _productCount; i++)
                    {
                        var getData = db.Queryable<TrackInfo>().First(f => f.PlateBarcode == plateBarcode && f.ProductPosition == (i + 1));
                        if (getData is null)
                        {
                            ackData.Add(new AckData { Position = (i + 1), IsOk = false, PlateBarcode = plateBarcode, ProductBarcode = string.Empty });
                        }
                        else
                        {
                            ackData.Add(new AckData { Position = (i + 1), IsOk = getData.IsOk, PlateBarcode = plateBarcode, ProductBarcode = getData.ProductBarcode });
                        }
                    }

                    var askMsg = JsonConvert.SerializeObject(ackData);
                    _server.SendToAll(string.Format("Ack:{0}\r\n", askMsg));
                }
            }
        }

        public class AckData
        {
            public int Position { get; set; }
            public string PlateBarcode { get; set; } = string.Empty;
            public string ProductBarcode { get; set; } = string.Empty;
            public bool IsOk { get; set; }
        }

        #endregion

        #region Track Client

        private CommonUtility.MyAsyncSocketClient _client = new CommonUtility.MyAsyncSocketClient();

        public void ConnectServer(string ip, int port)
        {
            _client.InitSocket(ip, port);
            _client.OnPushSocketsToTcpClient += _client_OnPushSocketsToTcpClient;
        }

        private bool _bReadData;
        private List<AckData> _ackDataListBuffer = new List<AckData>();
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);

        private void _client_OnPushSocketsToTcpClient(CommonUtility.BaseSocket sockets)
        {
            if (sockets.Offset >= 0)
            {
                if (_bReadData)
                {
                    var buff = new byte[sockets.Offset];
                    Array.Copy(sockets.RecBuffer, buff, buff.Length);
                    var msg = Encoding.ASCII.GetString(buff, 0, buff.Length);
                    var header = "Ack:";
                    if (msg.StartsWith(header) && msg.EndsWith("\r\n"))
                    {
                        var jsonStr = msg.TrimEnd().Substring(header.Length, msg.Length - header.Length - 2);
                        var list = JsonConvert.DeserializeObject<List<AckData>>(jsonStr);
                        _ackDataListBuffer.AddRange(list);
                        _waitHandle.Set();
                    }
                }
            }
        }

        public bool SendQuery(string plateBarcode, out List<AckData> ackDataList)
        {
            _ackDataListBuffer.Clear();
            _bReadData = true;
            var sendMsg = $"Query:{plateBarcode}\r\n";
            _client?.SendData(Encoding.ASCII.GetBytes(sendMsg));
            var isOk = _waitHandle.WaitOne(1000);

            ackDataList = new List<AckData>();
            ackDataList.AddRange(_ackDataListBuffer);

            _bReadData = false;

            return isOk;
        }

        #endregion
    }
}
