using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CommonUtility;
using HslCommunication.ModBus;

namespace Controller
{
    public sealed class EcuAutoAssemblyController : ControllerBase
    {
        public EcuAutoAssemblyController(string name)
            : base(name)
        {
        }

        #region 产品选择

        private readonly List<MyProduct> _productList = new List<MyProduct>();
        public string CurrentSelectProductNo = string.Empty;
        public string CurrentSelectProductPartNo = string.Empty;
        public bool IsProductSelected;

        public void AddProduct(string productNoProductName)
        {
            _productList.Add(new MyProduct
            {
                ProductNo = productNoProductName.Split('/')[0],
                ProductName = productNoProductName.Split('/')[1],
                ProductPartNo = productNoProductName.Split('/')[2]
            });
        }

        public void SelectProduct()
        {
            var listStr = _productList.Select(
                p => string.Format("{0}（{1}）:{2}", p.ProductNo, p.ProductPartNo, p.ProductName)).ToList();

            if (new AllLedAutoAssemblyProcutSelect(listStr).ShowDialog() != DialogResult.OK)
                MessageBox.Show(@"请选择产品！");
            else
            {
                CurrentSelectProductNo = AllLedAutoAssemblyProcutSelect.SelectProduct.Split(':')[0].Substring(0, 12);
                CurrentSelectProductPartNo = AllLedAutoAssemblyProcutSelect.SelectProduct.Split(':')[0].Substring(13, 9);
                IsProductSelected = true;
            }
        }

        internal class MyProduct
        {
            public string ProductName;
            public string ProductNo;
            public string ProductPartNo;
        }

        #endregion

        #region 追溯载盘相关

        public bool IsTrackOk;
        public string RemoteSqlIp = "127.0.0.1";

        //private string SqlConnectiong = @"server="+RemoteSqlIp+"";database=IPMS;uid=sa;pwd=123456";

        private string CurrentProcessName { get; set; }
        private string LastProcessName { get; set; }

        public void SetCurrentProcessName(string processName)
        {
            CurrentProcessName = processName;
        }

        public void SetLastProcessName(string processName)
        {
            LastProcessName = processName;
        }

        public void SaveLoadPlateState(string plateState)
        {
            lock (_lockBarcode)
            {
                plateState = plateState.ToUpper();

                var strSql = new StringBuilder();
                strSql.Append("select count(1) from EcuAutoAssemblyRunInfo");
                strSql.Append(" where LoadPlateBarcode=@loadPlateBarcode ");
                SqlParameter[] parameters =
                {
                    new SqlParameter("@loadPlateBarcode", SqlDbType.NVarChar, 200)
                };
                parameters[0].Value = LoadPlateBarcode;

                if (Exists(strSql.ToString(), parameters))
                {
                    var updateSql =
                        string.Format(
                            "update EcuAutoAssemblyRunInfo set " +
                            "ProductType = '{0}', " +
                            "ProcessNo = '{1}', " +
                            "CheckWorkstation = '{2}', " +
                            "ProcessState = '{3}'," +
                            "ProductNo = '{4}',  " +
                            "ProductPartNo = '{5}', " +
                            "IsProduct1Ok = '{6}', " +
                            "IsProduct2Ok = '{7}', " +
                            "IsProduct3Ok = '{8}', " +
                            "IsProduct4Ok = '{9}', " +
                            "Product1Barcode = '{10}', " +
                            "Product2Barcode = '{11}', " +
                            "Product3Barcode = '{12}', " +
                            "Product4Barcode = '{13}' where LoadPlateBarcode = '{14}'",
                            TrackProductType,
                            CurrentProcessName,
                            "",
                            plateState,
                            !string.IsNullOrEmpty(CurrentSelectProductNo)
                                ? CurrentSelectProductNo
                                : TrackProductNo,
                            !string.IsNullOrEmpty(CurrentSelectProductPartNo)
                                ? CurrentSelectProductPartNo
                                : TrackProductPartNo,
                            LoadPlateProduct1CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct2CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct3CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct4CompleteState == 1 ? "1" : "0",
                            TrackProduct1Barcode,
                            TrackProduct2Barcode,
                            TrackProduct3Barcode,
                            TrackProduct4Barcode,
                            LoadPlateBarcode);

                    Query(updateSql);
                }
                else
                {
                    var appendSql =
                        string.Format(
                            "insert into EcuAutoAssemblyRunInfo " +
                            "(LoadPlateBarcode,ProcessNo,CheckWorkstation,ProcessState,ProductNo,ProductPartNo,IsProduct1Ok,IsProduct2Ok,IsProduct3Ok,IsProduct4Ok,Product1Barcode,Product2Barcode,Product3Barcode,Product4Barcode,ProductType) " +
                            "values " +
                            "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')",
                            LoadPlateBarcode,
                            CurrentProcessName,
                            "",
                            plateState,
                            !string.IsNullOrEmpty(CurrentSelectProductNo)
                                ? CurrentSelectProductNo
                                : TrackProductNo,
                            !string.IsNullOrEmpty(CurrentSelectProductPartNo)
                                ? CurrentSelectProductPartNo
                                : TrackProductPartNo,
                            LoadPlateProduct1CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct2CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct3CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct4CompleteState == 1 ? "1" : "0",
                            TrackProduct1Barcode,
                            TrackProduct2Barcode,
                            TrackProduct3Barcode,
                            TrackProduct4Barcode,
                            TrackProductType);

                    Query(appendSql);
                }
            }
        }

        public void ReadLoadPlateState()
        {
            lock (_lockBarcode)
            {
                IsTrackOk = false;
                LoadPlateProduct1State = 0;
                LoadPlateProduct2State = 0;
                LoadPlateProduct3State = 0;
                LoadPlateProduct4State = 0;
                TrackProductNo = string.Empty;
                TrackProductPartNo = string.Empty;
                TrackProduct1Barcode = string.Empty;
                TrackProduct2Barcode = string.Empty;
                TrackProduct3Barcode = string.Empty;
                TrackProduct4Barcode = string.Empty;

                var strSql = new StringBuilder();
                strSql.Append("select count(1) from EcuAutoAssemblyRunInfo");
                strSql.Append(" where LoadPlateBarcode=@loadPlateBarcode ");
                SqlParameter[] parameters =
                {
                    new SqlParameter("@loadPlateBarcode", SqlDbType.NVarChar, 200)
                };
                parameters[0].Value = LoadPlateBarcode;

                if (Exists(strSql.ToString(), parameters))
                {
                    var getSql = string.Empty;

                    if (!string.IsNullOrEmpty(LastProcessName))
                    {
                        getSql = string.Format(
                               "select id,LoadPlateBarcode,ProductType,ProcessNo,CheckWorkstation,ProcessState,ProductNo,ProductPartNo,IsProduct1Ok,IsProduct2Ok,IsProduct3Ok,IsProduct4Ok,Product1Barcode,Product2Barcode,Product3Barcode,Product4Barcode FROM EcuAutoAssemblyRunInfo where LoadPlateBarcode = '{0}' and ProcessNo = '{1}'",
                               LoadPlateBarcode, LastProcessName);
                    }
                    else
                    {
                        getSql = string.Format(
                               "select id,LoadPlateBarcode,ProductType,ProcessNo,CheckWorkstation,ProcessState,ProductNo,ProductPartNo,IsProduct1Ok,IsProduct2Ok,IsProduct3Ok,IsProduct4Ok,Product1Barcode,Product2Barcode,Product3Barcode,Product4Barcode FROM EcuAutoAssemblyRunInfo where LoadPlateBarcode = '{0}'",
                               LoadPlateBarcode);
                    }

                    var ds = Query(getSql);
                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        if (ds.Tables[0].DefaultView[0]["ProcessState"].ToString().ToUpper() == "DONE")
                            IsTrackOk = true;

                        TrackProductType = ds.Tables[0].DefaultView[0]["ProductType"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["ProductType"].ToString();

                        LoadPlateProduct1State = ds.Tables[0].DefaultView[0]["IsProduct1Ok"] == null
                            ? (ushort)0
                            : ushort.Parse(ds.Tables[0].DefaultView[0]["IsProduct1Ok"].ToString());

                        LoadPlateProduct2State = ds.Tables[0].DefaultView[0]["IsProduct2Ok"] == null
                            ? (ushort)0
                            : ushort.Parse(ds.Tables[0].DefaultView[0]["IsProduct2Ok"].ToString());

                        LoadPlateProduct3State = ds.Tables[0].DefaultView[0]["IsProduct3Ok"] == null
                            ? (ushort)0
                            : ushort.Parse(ds.Tables[0].DefaultView[0]["IsProduct3Ok"].ToString());

                        LoadPlateProduct4State = ds.Tables[0].DefaultView[0]["IsProduct4Ok"] == null
                            ? (ushort)0
                            : ushort.Parse(ds.Tables[0].DefaultView[0]["IsProduct4Ok"].ToString());

                        TrackProductNo = ds.Tables[0].DefaultView[0]["ProductNo"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["ProductNo"].ToString();

                        TrackProductPartNo = ds.Tables[0].DefaultView[0]["ProductPartNo"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["ProductPartNo"].ToString();

                        TrackProduct1Barcode = ds.Tables[0].DefaultView[0]["Product1Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Product1Barcode"].ToString();

                        TrackProduct2Barcode = ds.Tables[0].DefaultView[0]["Product2Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Product2Barcode"].ToString();

                        TrackProduct3Barcode = ds.Tables[0].DefaultView[0]["Product3Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Product3Barcode"].ToString();

                        TrackProduct4Barcode = ds.Tables[0].DefaultView[0]["Product4Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Product4Barcode"].ToString();
                    }
                }
            }
        }

        private bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            var obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if (Equals(obj, null) || Equals(obj, DBNull.Value))
                cmdresult = 0;
            else
                cmdresult = int.Parse(obj.ToString());
            return cmdresult != 0;
        }

        public object GetSingle(string sqlString, params SqlParameter[] cmdParms)
        {
            var sqlConnectiong = @"server=" + RemoteSqlIp + ";database=IPMS;uid=sa;pwd=123456";

            using (var connection = new SqlConnection(sqlConnectiong))
            {
                using (var cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        var obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if (Equals(obj, null) || Equals(obj, DBNull.Value))
                            return null;
                        return obj;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        private static void PrepareCommand(
           SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms == null)
                return;
            foreach (var parameter in cmdParms)
            {
                if ((parameter.Direction == ParameterDirection.InputOutput ||
                     parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                    parameter.Value = DBNull.Value;
                cmd.Parameters.Add(parameter);
            }
        }

        private DataSet Query(string sqlString)
        {
            var sqlConnectiong = @"server=" + RemoteSqlIp + ";database=IPMS;uid=sa;pwd=123456";

            using (var connection = new SqlConnection(sqlConnectiong))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(sqlString, connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        //internal class MyLoadPlate
        //{
        //    #region 与载盘状态相关
        //    public string LoadPlateBarcode;
        //    public string ProcessNo;
        //    public string Workstation;
        //    public string ProcessState;
        //    #endregion

        //    #region 与载盘产品相关
        //    public string ProductNo;
        //    public string ProductPartNo;
        //    public bool IsProduct1Ok;
        //    public bool IsProduct2Ok;
        //    public bool IsProduct3Ok;
        //    public bool IsProduct4Ok;
        //    public string Product1Barcode;
        //    public string Product2Barcode;
        //    public string Product3Barcode;
        //    public string Product4Barcode;
        //    #endregion
        //}

        #endregion

        #region 扫码相关

        public string BarcodeTriggerCmd = @"T";
        public bool IsReadingBarcode;
        private MyAsyncSocketClient BarcodeScaner { get; set; }
        private readonly EventWaitHandle _barcodeReadWait = new AutoResetEvent(false);
        private string KeyAndKeyindexAndLen { get; set; }

        public void ConnectBarcode(string ipPort)
        {
            var split = ipPort.Split(':');
            var ipAddressStr = split[0];
            var port = Convert.ToInt32(split[1]);
            BarcodeScaner = new MyAsyncSocketClient();
            BarcodeScaner.InitSocket(ipAddressStr, port);
            BarcodeScaner.OnPushSocketsToTcpClient += _barcodeScaner_OnPushSocketsToTcpClient;
        }

        private void _barcodeScaner_OnPushSocketsToTcpClient(BaseSocket sockets)
        {
            if (sockets == null || sockets.RecBuffer == null)
                return;

            if (sockets.RecBuffer.Length == 0)
                return;

            if (!IsReadingBarcode)
                return;

            if (string.IsNullOrEmpty(KeyAndKeyindexAndLen))
                return;

            try
            {
                var buffer = new byte[sockets.Offset];
                Array.Copy(sockets.RecBuffer, 0, buffer, 0, sockets.Offset);

                var sp = KeyAndKeyindexAndLen.Split(':');
                var key = sp[0];
                var keyIndex = Convert.ToInt32(sp[1]);
                var len = Convert.ToInt32(sp[2]);

                var temp = Encoding.ASCII.GetString(buffer, 0, buffer.Length);

                if (string.IsNullOrEmpty(temp))
                    return;

                var findKeyIndex = temp.LastIndexOf(key, StringComparison.Ordinal);
                if (findKeyIndex == -1)
                    return;

                LoadPlateBarcode = temp.Substring(findKeyIndex - keyIndex, len).Replace('A', 'B');

                var loadPlateBarcodeBytes = new byte[50];
                Array.Copy(Encoding.ASCII.GetBytes(LoadPlateBarcode), loadPlateBarcodeBytes,
                    Encoding.ASCII.GetBytes(LoadPlateBarcode).Length);
                _hr40006 = (ushort)(loadPlateBarcodeBytes[0] * 256 + loadPlateBarcodeBytes[1]);
                _hr40007 = (ushort)(loadPlateBarcodeBytes[2] * 256 + loadPlateBarcodeBytes[3]);
                _hr40008 = (ushort)(loadPlateBarcodeBytes[4] * 256 + loadPlateBarcodeBytes[5]);
                _hr40009 = (ushort)(loadPlateBarcodeBytes[6] * 256 + loadPlateBarcodeBytes[7]);
                _hr40010 = (ushort)(loadPlateBarcodeBytes[8] * 256 + loadPlateBarcodeBytes[9]);

                IsReadingBarcode = false;
                _barcodeReadWait.Set();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private Thread TriggerTh { get; set; }

        public void ReadBarcode(string format)
        {
            _hr40006 = 0;
            _hr40007 = 0;
            _hr40008 = 0;
            _hr40009 = 0;
            _hr40010 = 0;

            LoadPlateBarcode = string.Empty;
            KeyAndKeyindexAndLen = format;
            IsReadingBarcode = true;

            if (TriggerTh != null)
            {
                TriggerTh.Abort();
                TriggerTh.Join();
            }

            if (BarcodeScaner == null)
                return;

            TriggerTh = new Thread(() =>
            {
                while (true)
                {
                    if (!IsReadingBarcode)
                        break;
                    BarcodeScaner.SendData(Encoding.ASCII.GetBytes(BarcodeTriggerCmd));
                    _barcodeReadWait.WaitOne(1000);
                }
            });
            TriggerTh.Start();
        }

        #endregion

        #region 激光打标相关

        public ushort WtireBarcode1Ok;
        public ushort WtireBarcode2Ok;
        public ushort WtireBarcode3Ok;
        public ushort WtireBarcode4Ok;

        public void WritePlgBarcodeData(string filePath, string index)
        {
            try
            {
                // 第一行：零件号前4
                // 第二行：零件号后4
                // 第三行：VPPS
                // 第四行: DUNS
                // 第五行: 追溯
                // 第六行: 矩阵码

                var barcode = string.Empty;
                //var barcode = "[)>06Y8210700000000XP2646074112V547656863T1A22238AAA000005";
                if (index == 1.ToString())
                    barcode = TrackProduct1Barcode;
                else if (index == 2.ToString())
                    barcode = TrackProduct2Barcode;
                else if (index == 3.ToString())
                    barcode = TrackProduct3Barcode;
                else if (index == 4.ToString())
                    barcode = TrackProduct4Barcode;
                var value = string.Empty;//barcode.Substring(barcode.Length - 16 - 2, 16);

                var partNo = barcode.Substring(24, 8);
                var vpps = barcode.Substring(8, 14);
                var duns = barcode.Substring(36, 9);
                var trackInfo = barcode.Substring(barcode.Length - 16 - 2, 16);

                value += partNo.Substring(0, 4) + "\r\n"; // 第一行：零件号前4
                value += partNo.Substring(4, 4) + "\r\n"; // 第二行：零件号后4
                value += vpps + "\r\n"; // 第三行：VPPS
                value += duns + "\r\n"; // 第四行: DUNS
                value += trackInfo + "\r\n"; // 第五行: 追溯
                value += barcode; // 第六行: 矩阵码
                WriteTxt(filePath, value);

                if (index == 1.ToString())
                    WtireBarcode1Ok = 1;
                else if (index == 2.ToString())
                    WtireBarcode2Ok = 1;
                else if (index == 3.ToString())
                    WtireBarcode3Ok = 1;
                else if (index == 4.ToString())
                    WtireBarcode4Ok = 1;
            }
            catch (Exception)
            {
                var value = string.Empty;
                value += ".\r\n";
                value += ".\r\n";
                value += ".\r\n";
                value += ".\r\n";
                value += ".\r\n";
                value += ".\r\n";
                WriteTxt(filePath, value);

                if (index == 1.ToString())
                    WtireBarcode1Ok = 0;
                else if (index == 2.ToString())
                    WtireBarcode2Ok = 0;
                else if (index == 3.ToString())
                    WtireBarcode3Ok = 0;
                else if (index == 4.ToString())
                    WtireBarcode4Ok = 0;
            }
        }

        private static void WriteTxt(string filePath, string value)
        {
            try
            {
                using (var fs = new FileStream(
                filePath, FileMode.Create, FileAccess.Write))
                {
                    var bs = Encoding.ASCII.GetBytes(value);
                    fs.Write(bs, 0, bs.Length);
                }

                Thread.Sleep(25);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region mobbus client相关
        private readonly object _lockBarcode = new object();
        public bool IsPaused;

        public ushort ModbusClientReadMaxLenth = 120;
        private ModbusTcpNet ModbusTcpClient { get; set; }
        public Thread ModbusTcpClintThread { get; set; }
        public Thread PlcThread { get; set; }

        public void InitClient(string ipPort)
        {
            var sp = ipPort.Split(':');
            ModbusTcpClient = new ModbusTcpNet(sp[0], int.Parse(sp[1]));

            if (ModbusTcpClintThread != null)
            {
                ModbusTcpClintThread.Abort();
                ModbusTcpClintThread.Join();
            }

            ModbusTcpClintThread = new Thread(ModbusTcpClientMainWork) { IsBackground = true };
            ModbusTcpClintThread.Start();
        }

        public void ConnectPlc(string ipPort)
        {
            var sp = ipPort.Split(':');
            ModbusTcpClient = new ModbusTcpNet(sp[0], int.Parse(sp[1]));

            if (PlcThread != null)
            {
                PlcThread.Abort();
                PlcThread.Join();
            }

            PlcThread = new Thread(PlcMainWork) { IsBackground = true };
            PlcThread.Start();
        }

        private void ModbusTcpClientMainWork()
        {
            ModbusTcpClient.ConnectServer();

            while (ModbusTcpClintThread.IsAlive)
            {
                if (!ModbusTcpClintThread.IsAlive)
                    break;

                Thread.Sleep(25);

                if (IsPaused)
                    continue;

                try
                {
                    lock (_lockBarcode)
                    {
                        var readHoldingRegs = ModbusTcpClient.Read(0.ToString(), ModbusClientReadMaxLenth > 120 ? (ushort)120 : ModbusClientReadMaxLenth);
                        if (!readHoldingRegs.IsSuccess)
                            continue;
                        var ushortList = new List<ushort>();
                        for (var i = 0; i < readHoldingRegs.Content.Length; i = i + 2)
                        {
                            var bs = new[] { readHoldingRegs.Content[i], readHoldingRegs.Content[i + 1] };
                            Array.Reverse(bs);
                            var ushortR = BitConverter.ToUInt16(bs, 0);
                            ushortList.Add(ushortR);
                        }

                        for (var i = 0; i < ushortList.Count; i++)
                        {
                            var addr = string.Format("_hr{0}", 40000 + i + 1);
                            GetType().GetField(addr).SetValue(this, ushortList[i]);
                        }

                        #region 赋值
                        //LoadPlateProduct1State = _hr40001;
                        //LoadPlateProduct2State = _hr40002;
                        //LoadPlateProduct3State = _hr40003;
                        //LoadPlateProduct4State = _hr40004;
                        StartSignal = _hr40005;

                        var loadPlateBarcodeBytes = new List<byte>();
                        loadPlateBarcodeBytes.AddRange(BitConverter.GetBytes(_hr40006).Reverse());
                        loadPlateBarcodeBytes.AddRange(BitConverter.GetBytes(_hr40007).Reverse());
                        loadPlateBarcodeBytes.AddRange(BitConverter.GetBytes(_hr40008).Reverse());
                        loadPlateBarcodeBytes.AddRange(BitConverter.GetBytes(_hr40009).Reverse());
                        loadPlateBarcodeBytes.AddRange(BitConverter.GetBytes(_hr40010).Reverse());
                        var temp6 = new List<byte>();
                        for (int i = 0; i < loadPlateBarcodeBytes.Count; i++)
                            if (loadPlateBarcodeBytes[i] != 0x00)
                                temp6.Add(loadPlateBarcodeBytes[i]);
                        LoadPlateBarcode = Encoding.ASCII.GetString(temp6.ToArray());

                        #endregion

                        if (ModbusClientReadMaxLenth > 120)
                            continue;
                        {
                            #region 赋值
                            _hr40121 = ReadySignal;
                            _hr40122 = (ushort)LoadPlateProduct1CompleteState;
                            _hr40123 = (ushort)LoadPlateProduct2CompleteState;
                            _hr40124 = (ushort)LoadPlateProduct3CompleteState;
                            _hr40125 = (ushort)LoadPlateProduct4CompleteState;
                            _hr40126 = CompleteSignal;
                            #endregion

                            var startAddr = ModbusClientReadMaxLenth;

                            var list = new List<ushort>();

                            for (var i = ModbusClientReadMaxLenth; i < ModbusClientReadMaxLenth + 120; i++)
                            {
                                var str = string.Format("_hr{0}", 40000 + i + 1);
                                var f = GetType().GetField(str).GetValue(this);
                                list.Add((ushort)f);
                            }

                            ModbusTcpClient.Write(startAddr.ToString(), list.ToArray());
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void PlcMainWork()
        {
            ModbusTcpClient.ConnectServer();

            while (PlcThread.IsAlive)
            {
                if (!PlcThread.IsAlive)
                    break;

                Thread.Sleep(25);

                try
                {
                    var readHoldingRegs = ModbusTcpClient.Read(0.ToString(), ModbusClientReadMaxLenth > 120 ? (ushort)120 : ModbusClientReadMaxLenth);
                    if (!readHoldingRegs.IsSuccess)
                        continue;
                    var ushortList = new List<ushort>();
                    for (var i = 0; i < readHoldingRegs.Content.Length; i = i + 2)
                    {
                        var bs = new[] { readHoldingRegs.Content[i], readHoldingRegs.Content[i + 1] };
                        Array.Reverse(bs);
                        var ushortR = BitConverter.ToUInt16(bs, 0);
                        ushortList.Add(ushortR);
                    }

                    for (var i = 0; i < ushortList.Count; i++)
                    {
                        var addr = string.Format("_hr{0}", 40000 + i + 1);
                        GetType().GetField(addr).SetValue(this, ushortList[i]);
                    }

                    if (ModbusClientReadMaxLenth > 120)
                        continue;
                    {
                        var startAddr = ModbusClientReadMaxLenth;

                        var list = new List<ushort>();

                        for (var i = ModbusClientReadMaxLenth; i < ModbusClientReadMaxLenth + 120; i++)
                        {
                            var str = string.Format("_hr{0}", 40000 + i + 1);
                            var f = GetType().GetField(str).GetValue(this);
                            list.Add((ushort)f);
                        }

                        ModbusTcpClient.Write(startAddr.ToString(), list.ToArray());
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
        #endregion

        #region modbus server相关
        public ushort ModbusServerReadMaxLenth = 120;

        #region Holding Regs
        public ushort _hr40001;
        public ushort _hr40002;
        public ushort _hr40003;
        public ushort _hr40004;
        public ushort _hr40005;
        public ushort _hr40006;
        public ushort _hr40007;
        public ushort _hr40008;
        public ushort _hr40009;
        public ushort _hr40010;
        public ushort _hr40011;
        public ushort _hr40012;
        public ushort _hr40013;
        public ushort _hr40014;
        public ushort _hr40015;
        public ushort _hr40016;
        public ushort _hr40017;
        public ushort _hr40018;
        public ushort _hr40019;
        public ushort _hr40020;
        public ushort _hr40021;
        public ushort _hr40022;
        public ushort _hr40023;
        public ushort _hr40024;
        public ushort _hr40025;
        public ushort _hr40026;
        public ushort _hr40027;
        public ushort _hr40028;
        public ushort _hr40029;
        public ushort _hr40030;
        public ushort _hr40031;
        public ushort _hr40032;
        public ushort _hr40033;
        public ushort _hr40034;
        public ushort _hr40035;
        public ushort _hr40036;
        public ushort _hr40037;
        public ushort _hr40038;
        public ushort _hr40039;
        public ushort _hr40040;
        public ushort _hr40041;
        public ushort _hr40042;
        public ushort _hr40043;
        public ushort _hr40044;
        public ushort _hr40045;
        public ushort _hr40046;
        public ushort _hr40047;
        public ushort _hr40048;
        public ushort _hr40049;
        public ushort _hr40050;
        public ushort _hr40051;
        public ushort _hr40052;
        public ushort _hr40053;
        public ushort _hr40054;
        public ushort _hr40055;
        public ushort _hr40056;
        public ushort _hr40057;
        public ushort _hr40058;
        public ushort _hr40059;
        public ushort _hr40060;
        public ushort _hr40061;
        public ushort _hr40062;
        public ushort _hr40063;
        public ushort _hr40064;
        public ushort _hr40065;
        public ushort _hr40066;
        public ushort _hr40067;
        public ushort _hr40068;
        public ushort _hr40069;
        public ushort _hr40070;
        public ushort _hr40071;
        public ushort _hr40072;
        public ushort _hr40073;
        public ushort _hr40074;
        public ushort _hr40075;
        public ushort _hr40076;
        public ushort _hr40077;
        public ushort _hr40078;
        public ushort _hr40079;
        public ushort _hr40080;
        public ushort _hr40081;
        public ushort _hr40082;
        public ushort _hr40083;
        public ushort _hr40084;
        public ushort _hr40085;
        public ushort _hr40086;
        public ushort _hr40087;
        public ushort _hr40088;
        public ushort _hr40089;
        public ushort _hr40090;
        public ushort _hr40091;
        public ushort _hr40092;
        public ushort _hr40093;
        public ushort _hr40094;
        public ushort _hr40095;
        public ushort _hr40096;
        public ushort _hr40097;
        public ushort _hr40098;
        public ushort _hr40099;
        public ushort _hr40100;
        public ushort _hr40101;
        public ushort _hr40102;
        public ushort _hr40103;
        public ushort _hr40104;
        public ushort _hr40105;
        public ushort _hr40106;
        public ushort _hr40107;
        public ushort _hr40108;
        public ushort _hr40109;
        public ushort _hr40110;
        public ushort _hr40111;
        public ushort _hr40112;
        public ushort _hr40113;
        public ushort _hr40114;
        public ushort _hr40115;
        public ushort _hr40116;
        public ushort _hr40117;
        public ushort _hr40118;
        public ushort _hr40119;
        public ushort _hr40120;
        public ushort _hr40121;
        public ushort _hr40122;
        public ushort _hr40123;
        public ushort _hr40124;
        public ushort _hr40125;
        public ushort _hr40126;
        public ushort _hr40127;
        public ushort _hr40128;
        public ushort _hr40129;
        public ushort _hr40130;
        public ushort _hr40131;
        public ushort _hr40132;
        public ushort _hr40133;
        public ushort _hr40134;
        public ushort _hr40135;
        public ushort _hr40136;
        public ushort _hr40137;
        public ushort _hr40138;
        public ushort _hr40139;
        public ushort _hr40140;
        public ushort _hr40141;
        public ushort _hr40142;
        public ushort _hr40143;
        public ushort _hr40144;
        public ushort _hr40145;
        public ushort _hr40146;
        public ushort _hr40147;
        public ushort _hr40148;
        public ushort _hr40149;
        public ushort _hr40150;
        public ushort _hr40151;
        public ushort _hr40152;
        public ushort _hr40153;
        public ushort _hr40154;
        public ushort _hr40155;
        public ushort _hr40156;
        public ushort _hr40157;
        public ushort _hr40158;
        public ushort _hr40159;
        public ushort _hr40160;
        public ushort _hr40161;
        public ushort _hr40162;
        public ushort _hr40163;
        public ushort _hr40164;
        public ushort _hr40165;
        public ushort _hr40166;
        public ushort _hr40167;
        public ushort _hr40168;
        public ushort _hr40169;
        public ushort _hr40170;
        public ushort _hr40171;
        public ushort _hr40172;
        public ushort _hr40173;
        public ushort _hr40174;
        public ushort _hr40175;
        public ushort _hr40176;
        public ushort _hr40177;
        public ushort _hr40178;
        public ushort _hr40179;
        public ushort _hr40180;
        public ushort _hr40181;
        public ushort _hr40182;
        public ushort _hr40183;
        public ushort _hr40184;
        public ushort _hr40185;
        public ushort _hr40186;
        public ushort _hr40187;
        public ushort _hr40188;
        public ushort _hr40189;
        public ushort _hr40190;
        public ushort _hr40191;
        public ushort _hr40192;
        public ushort _hr40193;
        public ushort _hr40194;
        public ushort _hr40195;
        public ushort _hr40196;
        public ushort _hr40197;
        public ushort _hr40198;
        public ushort _hr40199;
        public ushort _hr40200;
        public ushort _hr40201;
        public ushort _hr40202;
        public ushort _hr40203;
        public ushort _hr40204;
        public ushort _hr40205;
        public ushort _hr40206;
        public ushort _hr40207;
        public ushort _hr40208;
        public ushort _hr40209;
        public ushort _hr40210;
        public ushort _hr40211;
        public ushort _hr40212;
        public ushort _hr40213;
        public ushort _hr40214;
        public ushort _hr40215;
        public ushort _hr40216;
        public ushort _hr40217;
        public ushort _hr40218;
        public ushort _hr40219;
        public ushort _hr40220;
        public ushort _hr40221;
        public ushort _hr40222;
        public ushort _hr40223;
        public ushort _hr40224;
        public ushort _hr40225;
        public ushort _hr40226;
        public ushort _hr40227;
        public ushort _hr40228;
        public ushort _hr40229;
        public ushort _hr40230;
        public ushort _hr40231;
        public ushort _hr40232;
        public ushort _hr40233;
        public ushort _hr40234;
        public ushort _hr40235;
        public ushort _hr40236;
        public ushort _hr40237;
        public ushort _hr40238;
        public ushort _hr40239;
        public ushort _hr40240;
        #endregion

        private ModbusTcpServer ModbusTcpServer { get; set; }
        private Thread ModbusTcpServerThread { get; set; }

        public void InitServer(string port)
        {
            ModbusTcpServer = new ModbusTcpServer();
            ModbusTcpServer.ServerStart(int.Parse(port));
            ModbusTcpServer.OnDataReceived +=
                _modbusTcpServer_OnDataReceived;

            if (ModbusTcpServerThread != null)
            {
                ModbusTcpServerThread.Abort();
                ModbusTcpServerThread.Join();
            }

            ModbusTcpServerThread = new Thread(ModbusTcpServerMainWork);
            ModbusTcpServerThread.Start();
        }

        private void ModbusTcpServerMainWork()
        {
            while (ModbusTcpServerThread.IsAlive)
            {
                if (!ModbusTcpServerThread.IsAlive)
                    break;

                Thread.Sleep(5);

                _hr40005 = StartSignal;

                for (var i = 0; i < ModbusServerReadMaxLenth; i++)
                {
                    var addr = string.Format("_hr{0}", 40000 + i + 1);
                    var field = GetType().GetField(addr).GetValue(this);
                    ModbusTcpServer.Write(i.ToString(), (ushort)field);
                }
            }
        }

        public void ModbusServerSetBarcode()
        {
            try
            {
                var loadPlateBarcodeBytes = new byte[50];
                Array.Copy(Encoding.ASCII.GetBytes(LoadPlateBarcode), loadPlateBarcodeBytes,
                    Encoding.ASCII.GetBytes(LoadPlateBarcode).Length);
                _hr40006 = (ushort)(loadPlateBarcodeBytes[0] * 256 + loadPlateBarcodeBytes[1]);
                _hr40007 = (ushort)(loadPlateBarcodeBytes[2] * 256 + loadPlateBarcodeBytes[3]);
                _hr40008 = (ushort)(loadPlateBarcodeBytes[4] * 256 + loadPlateBarcodeBytes[5]);
                _hr40009 = (ushort)(loadPlateBarcodeBytes[6] * 256 + loadPlateBarcodeBytes[7]);
                _hr40010 = (ushort)(loadPlateBarcodeBytes[8] * 256 + loadPlateBarcodeBytes[9]);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void _modbusTcpServer_OnDataReceived(
            ModbusTcpServer arg1, byte[] arg2)
        {
            try
            {
                var codeId = arg2[7];
                Console.WriteLine(codeId);
                if (codeId == 0x06)
                {
                    var startAddr = arg2[8] * 256 + arg2[9];
                    var value = (ushort)(arg2[10] * 256 + arg2[11]);
                    var field = GetType().GetField(string.Format("_hr{0}", 40000 + startAddr + 1));
                    field.SetValue(this, value);
                }
                else if (codeId == 0x10)
                {
                    var startAddr = arg2[8] * 256 + arg2[9];
                    var regCount = arg2[10] * 256 + arg2[11];
                    var bs = new byte[regCount * 2];
                    Array.Copy(arg2, 13, bs, 0, regCount * 2);

                    for (var i = 0; i < bs.Length; i = i + 2)
                    {
                        var name = string.Format("_hr{0}", 40000 + startAddr + 1);
                        var field = GetType().GetField(name);
                        field.SetValue(this, (ushort)(bs[i] * 256 + bs[i + 1]));
                        startAddr++;
                    }

                    #region 赋值
                    ReadySignal = _hr40121;
                    LoadPlateProduct1CompleteState = _hr40122;
                    LoadPlateProduct2CompleteState = _hr40123;
                    LoadPlateProduct3CompleteState = _hr40124;
                    LoadPlateProduct4CompleteState = _hr40125;
                    CompleteSignal = _hr40126;
                    #endregion
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        #endregion

        #region 通信相关变量
        public ushort ReadySignal; // 就绪信号,server读,client写 // 400121
        public int LoadPlateProduct1CompleteState; // 完成信号,server读,client写 // 400122
        public int LoadPlateProduct2CompleteState; // 完成信号,server读,client写 // 400123
        public int LoadPlateProduct3CompleteState; // 完成信号,server读,client写 // 400124
        public int LoadPlateProduct4CompleteState; // 完成信号,server读,client写 // 400125
        public ushort CompleteSignal; // 完成信号,server读,client写 // 400126

        public ushort LoadPlateProduct1State; // 载盘产品1状态,server写,client读 // 40001
        public ushort LoadPlateProduct2State; // 载盘产品2状态,server写,client读 // 40002
        public ushort LoadPlateProduct3State; // 载盘产品3状态,server写,client读 // 40003
        public ushort LoadPlateProduct4State; // 载盘产品4状态,server写,client读 // 40004
        public ushort StartSignal; // 启动信号,server写,client读  // 40005
        public string LoadPlateBarcode = string.Empty; // 载盘二维码,server写,client读 // 40006~40010
        #endregion

        #region 追溯信息相关变量

        public string TrackProductType = string.Empty; // 产品类型 PLG/Firewall
        public string TrackProductNo = string.Empty; // 总成号
        public string TrackProductPartNo = string.Empty; // PR号
        public string TrackProduct1Barcode = string.Empty; // 产品1标签
        public string TrackProduct2Barcode = string.Empty; // 产品2标签
        public string TrackProduct3Barcode = string.Empty; // 产品3标签
        public string TrackProduct4Barcode = string.Empty; // 产品4标签
        #endregion
    }
}
