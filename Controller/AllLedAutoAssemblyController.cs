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
    public sealed class AllLedAutoAssemblyController : ControllerBase
    {
        public AllLedAutoAssemblyController(string name) :
            base(name)
        {
            //LoadPlateBarcode = "B09";
            //LastProcessName = "终检";
            //CurrentProcessName = "终检1";
            //LoadPlateCompleteProductNo = "PR4101000605";
            //LoadPlateProduct1CompleteState = 1;
            //LoadPlateProduct2CompleteState = 0;
            //LoadPlateProduct3CompleteState = 1;
            //LoadPlateProduct4CompleteState = 0;
            //LoadPlateProduct1CompleteBarcode = "P123456789123456789123456789123456789";
            //LoadPlateProduct2CompleteBarcode = "P123456789123456789123456789123456789";
            //LoadPlateProduct3CompleteBarcode = "P123456789123456789123456789123456789";
            //LoadPlateProduct4CompleteBarcode = "P123456789123456789123456789123456789";
            //SaveLoadPlateState("ING");
            //ReadLoadPlateState();
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

        //private const string SqlConnectiong = @"server=.;database=IPMS;uid=sa;pwd=123456";

        //private readonly MyLoadPlate _myLoadPlate = new MyLoadPlate();
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
                strSql.Append("select count(1) from AllLedAutoAssemblyRunInfo");
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
                            "update AllLedAutoAssemblyRunInfo set " +
                            "ProcessNo = '{0}', " +
                            "CheckWorkstation = '{1}', " +
                            "ProcessState = '{2}'," +
                            "ProductNo = '{3}',  " +
                            "ProductPartNo = '{4}', " +
                            "IsProduct1Ok = '{5}', " +
                            "IsProduct2Ok = '{6}', " +
                            "IsProduct3Ok = '{7}', " +
                            "IsProduct4Ok = '{8}', " +
                            "Product1Barcode = '{9}', " +
                            "Product2Barcode = '{10}', " +
                            "Product3Barcode = '{11}', " +
                            "Product4Barcode = '{12}' where LoadPlateBarcode = '{13}'",
                            CurrentProcessName,
                            "",
                            plateState,
                            !string.IsNullOrEmpty(CurrentSelectProductNo)
                                ? CurrentSelectProductNo
                                : LoadPlateCompleteProductNo,
                            !string.IsNullOrEmpty(CurrentSelectProductPartNo)
                                ? CurrentSelectProductPartNo
                                : LoadPlateCompleteProductPartNo,
                            LoadPlateProduct1CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct2CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct3CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct4CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct1CompleteBarcode,
                            LoadPlateProduct2CompleteBarcode,
                            LoadPlateProduct3CompleteBarcode,
                            LoadPlateProduct4CompleteBarcode,
                            LoadPlateBarcode);

                    Query(updateSql);
                }
                else
                {
                    var appendSql =
                        string.Format(
                            "insert into AllLedAutoAssemblyRunInfo " +
                            "(LoadPlateBarcode,ProcessNo,CheckWorkstation,ProcessState,ProductNo,ProductPartNo,IsProduct1Ok,IsProduct2Ok,IsProduct3Ok,IsProduct4Ok,Product1Barcode,Product2Barcode,Product3Barcode,Product4Barcode) " +
                            "values " +
                            "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                            LoadPlateBarcode,
                            CurrentProcessName,
                            "",
                            plateState,
                            !string.IsNullOrEmpty(CurrentSelectProductNo)
                                ? CurrentSelectProductNo
                                : LoadPlateCompleteProductNo,
                            !string.IsNullOrEmpty(CurrentSelectProductPartNo)
                                ? CurrentSelectProductPartNo
                                : LoadPlateCompleteProductPartNo,
                            LoadPlateProduct1CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct2CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct3CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct4CompleteState == 1 ? "1" : "0",
                            LoadPlateProduct1CompleteBarcode,
                            LoadPlateProduct2CompleteBarcode,
                            LoadPlateProduct3CompleteBarcode,
                            LoadPlateProduct4CompleteBarcode);

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
                LoadPlateProductNo = string.Empty;
                LoadPlateProductPartNo = string.Empty;
                LoadPlateProduct1Barcode = string.Empty;
                LoadPlateProduct2Barcode = string.Empty;
                LoadPlateProduct3Barcode = string.Empty;
                LoadPlateProduct4Barcode = string.Empty;

                var strSql = new StringBuilder();
                strSql.Append("select count(1) from AllLedAutoAssemblyRunInfo");
                strSql.Append(" where LoadPlateBarcode=@loadPlateBarcode ");
                SqlParameter[] parameters =
                {
                    new SqlParameter("@loadPlateBarcode", SqlDbType.NVarChar, 200)
                };
                parameters[0].Value = LoadPlateBarcode;

                if (Exists(strSql.ToString(), parameters))
                {
                    var getSql =
                        string.Format(
                            "select id,LoadPlateBarcode,ProcessNo,CheckWorkstation,ProcessState,ProductNo,ProductPartNo,IsProduct1Ok,IsProduct2Ok,IsProduct3Ok,IsProduct4Ok,Product1Barcode,Product2Barcode,Product3Barcode,Product4Barcode FROM AllLedAutoAssemblyRunInfo where LoadPlateBarcode = '{0}' and ProcessNo = '{1}'",
                            LoadPlateBarcode, LastProcessName);
                    var ds = Query(getSql);
                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        if (ds.Tables[0].DefaultView[0]["ProcessState"].ToString().ToUpper() == "DONE")
                            IsTrackOk = true;

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

                        LoadPlateProductNo = ds.Tables[0].DefaultView[0]["ProductNo"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["ProductNo"].ToString();

                        LoadPlateProductPartNo = ds.Tables[0].DefaultView[0]["ProductPartNo"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["ProductPartNo"].ToString();

                        LoadPlateProduct1Barcode = ds.Tables[0].DefaultView[0]["Product1Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Product1Barcode"].ToString();

                        LoadPlateProduct2Barcode = ds.Tables[0].DefaultView[0]["Product2Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Product2Barcode"].ToString();

                        LoadPlateProduct3Barcode = ds.Tables[0].DefaultView[0]["Product3Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Product3Barcode"].ToString();

                        LoadPlateProduct4Barcode = ds.Tables[0].DefaultView[0]["Product4Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Product4Barcode"].ToString();
                    }
                }

                #region 赋值
                _hr40001 = LoadPlateProduct1State;
                _hr40002 = LoadPlateProduct2State;
                _hr40003 = LoadPlateProduct3State;
                _hr40004 = LoadPlateProduct4State;

                var productNoBytes = new byte[10 * 2];
                Array.Copy(Encoding.ASCII.GetBytes(LoadPlateProductNo), productNoBytes,
                    Encoding.ASCII.GetBytes(LoadPlateProductNo).Length);
                _hr40006 = (ushort)(productNoBytes[0] * 256 + productNoBytes[1]);
                _hr40007 = (ushort)(productNoBytes[2] * 256 + productNoBytes[3]);
                _hr40008 = (ushort)(productNoBytes[4] * 256 + productNoBytes[5]);
                _hr40009 = (ushort)(productNoBytes[6] * 256 + productNoBytes[7]);
                _hr40010 = (ushort)(productNoBytes[8] * 256 + productNoBytes[9]);
                _hr40011 = (ushort)(productNoBytes[10] * 256 + productNoBytes[11]);
                _hr40012 = (ushort)(productNoBytes[12] * 256 + productNoBytes[13]);
                _hr40013 = (ushort)(productNoBytes[14] * 256 + productNoBytes[15]);
                _hr40014 = (ushort)(productNoBytes[16] * 256 + productNoBytes[17]);
                _hr40015 = (ushort)(productNoBytes[18] * 256 + productNoBytes[19]);

                var loadPlateProduct1BarcodeBytes = new byte[50];
                Array.Copy(Encoding.ASCII.GetBytes(LoadPlateProduct1Barcode), loadPlateProduct1BarcodeBytes,
                    Encoding.ASCII.GetBytes(LoadPlateProduct1Barcode).Length);
                _hr40016 = (ushort)(loadPlateProduct1BarcodeBytes[0] * 256 + loadPlateProduct1BarcodeBytes[1]);
                _hr40017 = (ushort)(loadPlateProduct1BarcodeBytes[2] * 256 + loadPlateProduct1BarcodeBytes[3]);
                _hr40018 = (ushort)(loadPlateProduct1BarcodeBytes[4] * 256 + loadPlateProduct1BarcodeBytes[5]);
                _hr40019 = (ushort)(loadPlateProduct1BarcodeBytes[6] * 256 + loadPlateProduct1BarcodeBytes[7]);
                _hr40020 = (ushort)(loadPlateProduct1BarcodeBytes[8] * 256 + loadPlateProduct1BarcodeBytes[9]);
                _hr40021 = (ushort)(loadPlateProduct1BarcodeBytes[10] * 256 + loadPlateProduct1BarcodeBytes[11]);
                _hr40022 = (ushort)(loadPlateProduct1BarcodeBytes[12] * 256 + loadPlateProduct1BarcodeBytes[13]);
                _hr40023 = (ushort)(loadPlateProduct1BarcodeBytes[14] * 256 + loadPlateProduct1BarcodeBytes[15]);
                _hr40024 = (ushort)(loadPlateProduct1BarcodeBytes[16] * 256 + loadPlateProduct1BarcodeBytes[17]);
                _hr40025 = (ushort)(loadPlateProduct1BarcodeBytes[18] * 256 + loadPlateProduct1BarcodeBytes[19]);
                _hr40026 = (ushort)(loadPlateProduct1BarcodeBytes[20] * 256 + loadPlateProduct1BarcodeBytes[21]);
                _hr40027 = (ushort)(loadPlateProduct1BarcodeBytes[22] * 256 + loadPlateProduct1BarcodeBytes[23]);
                _hr40028 = (ushort)(loadPlateProduct1BarcodeBytes[24] * 256 + loadPlateProduct1BarcodeBytes[25]);
                _hr40029 = (ushort)(loadPlateProduct1BarcodeBytes[26] * 256 + loadPlateProduct1BarcodeBytes[27]);
                _hr40030 = (ushort)(loadPlateProduct1BarcodeBytes[28] * 256 + loadPlateProduct1BarcodeBytes[29]);
                _hr40031 = (ushort)(loadPlateProduct1BarcodeBytes[30] * 256 + loadPlateProduct1BarcodeBytes[31]);
                _hr40032 = (ushort)(loadPlateProduct1BarcodeBytes[32] * 256 + loadPlateProduct1BarcodeBytes[33]);
                _hr40033 = (ushort)(loadPlateProduct1BarcodeBytes[34] * 256 + loadPlateProduct1BarcodeBytes[35]);
                _hr40034 = (ushort)(loadPlateProduct1BarcodeBytes[36] * 256 + loadPlateProduct1BarcodeBytes[37]);
                _hr40035 = (ushort)(loadPlateProduct1BarcodeBytes[38] * 256 + loadPlateProduct1BarcodeBytes[39]);
                _hr40036 = (ushort)(loadPlateProduct1BarcodeBytes[40] * 256 + loadPlateProduct1BarcodeBytes[41]);
                _hr40037 = (ushort)(loadPlateProduct1BarcodeBytes[42] * 256 + loadPlateProduct1BarcodeBytes[43]);
                _hr40038 = (ushort)(loadPlateProduct1BarcodeBytes[44] * 256 + loadPlateProduct1BarcodeBytes[45]);
                _hr40039 = (ushort)(loadPlateProduct1BarcodeBytes[46] * 256 + loadPlateProduct1BarcodeBytes[47]);
                _hr40040 = (ushort)(loadPlateProduct1BarcodeBytes[48] * 256 + loadPlateProduct1BarcodeBytes[49]);

                var loadPlateProduct2BarcodeBytes = new byte[50];
                Array.Copy(Encoding.ASCII.GetBytes(LoadPlateProduct2Barcode), loadPlateProduct2BarcodeBytes,
                    Encoding.ASCII.GetBytes(LoadPlateProduct2Barcode).Length);
                _hr40041 = (ushort)(loadPlateProduct2BarcodeBytes[0] * 256 + loadPlateProduct2BarcodeBytes[1]);
                _hr40042 = (ushort)(loadPlateProduct2BarcodeBytes[2] * 256 + loadPlateProduct2BarcodeBytes[3]);
                _hr40043 = (ushort)(loadPlateProduct2BarcodeBytes[4] * 256 + loadPlateProduct2BarcodeBytes[5]);
                _hr40044 = (ushort)(loadPlateProduct2BarcodeBytes[6] * 256 + loadPlateProduct2BarcodeBytes[7]);
                _hr40045 = (ushort)(loadPlateProduct2BarcodeBytes[8] * 256 + loadPlateProduct2BarcodeBytes[9]);
                _hr40046 = (ushort)(loadPlateProduct2BarcodeBytes[10] * 256 + loadPlateProduct2BarcodeBytes[11]);
                _hr40047 = (ushort)(loadPlateProduct2BarcodeBytes[12] * 256 + loadPlateProduct2BarcodeBytes[13]);
                _hr40048 = (ushort)(loadPlateProduct2BarcodeBytes[14] * 256 + loadPlateProduct2BarcodeBytes[15]);
                _hr40049 = (ushort)(loadPlateProduct2BarcodeBytes[16] * 256 + loadPlateProduct2BarcodeBytes[17]);
                _hr40050 = (ushort)(loadPlateProduct2BarcodeBytes[18] * 256 + loadPlateProduct2BarcodeBytes[19]);
                _hr40051 = (ushort)(loadPlateProduct2BarcodeBytes[20] * 256 + loadPlateProduct2BarcodeBytes[21]);
                _hr40052 = (ushort)(loadPlateProduct2BarcodeBytes[22] * 256 + loadPlateProduct2BarcodeBytes[23]);
                _hr40053 = (ushort)(loadPlateProduct2BarcodeBytes[24] * 256 + loadPlateProduct2BarcodeBytes[25]);
                _hr40054 = (ushort)(loadPlateProduct2BarcodeBytes[26] * 256 + loadPlateProduct2BarcodeBytes[27]);
                _hr40055 = (ushort)(loadPlateProduct2BarcodeBytes[28] * 256 + loadPlateProduct2BarcodeBytes[29]);
                _hr40056 = (ushort)(loadPlateProduct2BarcodeBytes[30] * 256 + loadPlateProduct2BarcodeBytes[31]);
                _hr40057 = (ushort)(loadPlateProduct2BarcodeBytes[32] * 256 + loadPlateProduct2BarcodeBytes[33]);
                _hr40058 = (ushort)(loadPlateProduct2BarcodeBytes[34] * 256 + loadPlateProduct2BarcodeBytes[35]);
                _hr40059 = (ushort)(loadPlateProduct2BarcodeBytes[36] * 256 + loadPlateProduct2BarcodeBytes[37]);
                _hr40060 = (ushort)(loadPlateProduct2BarcodeBytes[38] * 256 + loadPlateProduct2BarcodeBytes[39]);
                _hr40061 = (ushort)(loadPlateProduct2BarcodeBytes[40] * 256 + loadPlateProduct2BarcodeBytes[41]);
                _hr40062 = (ushort)(loadPlateProduct2BarcodeBytes[42] * 256 + loadPlateProduct2BarcodeBytes[43]);
                _hr40063 = (ushort)(loadPlateProduct2BarcodeBytes[44] * 256 + loadPlateProduct2BarcodeBytes[45]);
                _hr40064 = (ushort)(loadPlateProduct2BarcodeBytes[46] * 256 + loadPlateProduct2BarcodeBytes[47]);
                _hr40065 = (ushort)(loadPlateProduct2BarcodeBytes[48] * 256 + loadPlateProduct2BarcodeBytes[49]);

                var loadPlateProduct3BarcodeBytes = new byte[50];
                Array.Copy(Encoding.ASCII.GetBytes(LoadPlateProduct3Barcode), loadPlateProduct3BarcodeBytes,
                    Encoding.ASCII.GetBytes(LoadPlateProduct3Barcode).Length);
                _hr40066 = (ushort)(loadPlateProduct3BarcodeBytes[0] * 256 + loadPlateProduct3BarcodeBytes[1]);
                _hr40067 = (ushort)(loadPlateProduct3BarcodeBytes[2] * 256 + loadPlateProduct3BarcodeBytes[3]);
                _hr40068 = (ushort)(loadPlateProduct3BarcodeBytes[4] * 256 + loadPlateProduct3BarcodeBytes[5]);
                _hr40069 = (ushort)(loadPlateProduct3BarcodeBytes[6] * 256 + loadPlateProduct3BarcodeBytes[7]);
                _hr40070 = (ushort)(loadPlateProduct3BarcodeBytes[8] * 256 + loadPlateProduct3BarcodeBytes[9]);
                _hr40071 = (ushort)(loadPlateProduct3BarcodeBytes[10] * 256 + loadPlateProduct3BarcodeBytes[11]);
                _hr40072 = (ushort)(loadPlateProduct3BarcodeBytes[12] * 256 + loadPlateProduct3BarcodeBytes[13]);
                _hr40073 = (ushort)(loadPlateProduct3BarcodeBytes[14] * 256 + loadPlateProduct3BarcodeBytes[15]);
                _hr40074 = (ushort)(loadPlateProduct3BarcodeBytes[16] * 256 + loadPlateProduct3BarcodeBytes[17]);
                _hr40075 = (ushort)(loadPlateProduct3BarcodeBytes[18] * 256 + loadPlateProduct3BarcodeBytes[19]);
                _hr40076 = (ushort)(loadPlateProduct3BarcodeBytes[20] * 256 + loadPlateProduct3BarcodeBytes[21]);
                _hr40077 = (ushort)(loadPlateProduct3BarcodeBytes[22] * 256 + loadPlateProduct3BarcodeBytes[23]);
                _hr40078 = (ushort)(loadPlateProduct3BarcodeBytes[24] * 256 + loadPlateProduct3BarcodeBytes[25]);
                _hr40079 = (ushort)(loadPlateProduct3BarcodeBytes[26] * 256 + loadPlateProduct3BarcodeBytes[27]);
                _hr40080 = (ushort)(loadPlateProduct3BarcodeBytes[28] * 256 + loadPlateProduct3BarcodeBytes[29]);
                _hr40081 = (ushort)(loadPlateProduct3BarcodeBytes[30] * 256 + loadPlateProduct3BarcodeBytes[31]);
                _hr40082 = (ushort)(loadPlateProduct3BarcodeBytes[32] * 256 + loadPlateProduct3BarcodeBytes[33]);
                _hr40083 = (ushort)(loadPlateProduct3BarcodeBytes[34] * 256 + loadPlateProduct3BarcodeBytes[35]);
                _hr40084 = (ushort)(loadPlateProduct3BarcodeBytes[36] * 256 + loadPlateProduct3BarcodeBytes[37]);
                _hr40085 = (ushort)(loadPlateProduct3BarcodeBytes[38] * 256 + loadPlateProduct3BarcodeBytes[39]);
                _hr40086 = (ushort)(loadPlateProduct3BarcodeBytes[40] * 256 + loadPlateProduct3BarcodeBytes[41]);
                _hr40087 = (ushort)(loadPlateProduct3BarcodeBytes[42] * 256 + loadPlateProduct3BarcodeBytes[43]);
                _hr40088 = (ushort)(loadPlateProduct3BarcodeBytes[44] * 256 + loadPlateProduct3BarcodeBytes[45]);
                _hr40089 = (ushort)(loadPlateProduct3BarcodeBytes[46] * 256 + loadPlateProduct3BarcodeBytes[47]);
                _hr40090 = (ushort)(loadPlateProduct3BarcodeBytes[48] * 256 + loadPlateProduct3BarcodeBytes[49]);

                var loadPlateProduct4BarcodeBytes = new byte[50];
                Array.Copy(Encoding.ASCII.GetBytes(LoadPlateProduct4Barcode), loadPlateProduct4BarcodeBytes,
                    Encoding.ASCII.GetBytes(LoadPlateProduct4Barcode).Length);
                _hr40091 = (ushort)(loadPlateProduct4BarcodeBytes[0] * 256 + loadPlateProduct4BarcodeBytes[1]);
                _hr40092 = (ushort)(loadPlateProduct4BarcodeBytes[2] * 256 + loadPlateProduct4BarcodeBytes[3]);
                _hr40093 = (ushort)(loadPlateProduct4BarcodeBytes[4] * 256 + loadPlateProduct4BarcodeBytes[5]);
                _hr40094 = (ushort)(loadPlateProduct4BarcodeBytes[6] * 256 + loadPlateProduct4BarcodeBytes[7]);
                _hr40095 = (ushort)(loadPlateProduct4BarcodeBytes[8] * 256 + loadPlateProduct4BarcodeBytes[9]);
                _hr40096 = (ushort)(loadPlateProduct4BarcodeBytes[10] * 256 + loadPlateProduct4BarcodeBytes[11]);
                _hr40097 = (ushort)(loadPlateProduct4BarcodeBytes[12] * 256 + loadPlateProduct4BarcodeBytes[13]);
                _hr40098 = (ushort)(loadPlateProduct4BarcodeBytes[14] * 256 + loadPlateProduct4BarcodeBytes[15]);
                _hr40099 = (ushort)(loadPlateProduct4BarcodeBytes[16] * 256 + loadPlateProduct4BarcodeBytes[17]);
                _hr40100 = (ushort)(loadPlateProduct4BarcodeBytes[18] * 256 + loadPlateProduct4BarcodeBytes[19]);
                _hr40101 = (ushort)(loadPlateProduct4BarcodeBytes[20] * 256 + loadPlateProduct4BarcodeBytes[21]);
                _hr40102 = (ushort)(loadPlateProduct4BarcodeBytes[22] * 256 + loadPlateProduct4BarcodeBytes[23]);
                _hr40103 = (ushort)(loadPlateProduct4BarcodeBytes[24] * 256 + loadPlateProduct4BarcodeBytes[25]);
                _hr40104 = (ushort)(loadPlateProduct4BarcodeBytes[26] * 256 + loadPlateProduct4BarcodeBytes[27]);
                _hr40105 = (ushort)(loadPlateProduct4BarcodeBytes[28] * 256 + loadPlateProduct4BarcodeBytes[29]);
                _hr40106 = (ushort)(loadPlateProduct4BarcodeBytes[30] * 256 + loadPlateProduct4BarcodeBytes[31]);
                _hr40107 = (ushort)(loadPlateProduct4BarcodeBytes[32] * 256 + loadPlateProduct4BarcodeBytes[33]);
                _hr40108 = (ushort)(loadPlateProduct4BarcodeBytes[34] * 256 + loadPlateProduct4BarcodeBytes[35]);
                _hr40109 = (ushort)(loadPlateProduct4BarcodeBytes[36] * 256 + loadPlateProduct4BarcodeBytes[37]);
                _hr40110 = (ushort)(loadPlateProduct4BarcodeBytes[38] * 256 + loadPlateProduct4BarcodeBytes[39]);
                _hr40111 = (ushort)(loadPlateProduct4BarcodeBytes[40] * 256 + loadPlateProduct4BarcodeBytes[41]);
                _hr40112 = (ushort)(loadPlateProduct4BarcodeBytes[42] * 256 + loadPlateProduct4BarcodeBytes[43]);
                _hr40113 = (ushort)(loadPlateProduct4BarcodeBytes[44] * 256 + loadPlateProduct4BarcodeBytes[45]);
                _hr40114 = (ushort)(loadPlateProduct4BarcodeBytes[46] * 256 + loadPlateProduct4BarcodeBytes[47]);
                _hr40115 = (ushort)(loadPlateProduct4BarcodeBytes[48] * 256 + loadPlateProduct4BarcodeBytes[49]);

                var loadPlateProductPartBytes = new byte[10];
                var productPartNo = string.Empty;
                if (!string.IsNullOrEmpty(LoadPlateProductPartNo) && LoadPlateProductPartNo.Length > 2)
                    productPartNo = LoadPlateProductPartNo.Substring(1, LoadPlateProductPartNo.Length - 1);
                Array.Copy(Encoding.ASCII.GetBytes(productPartNo), loadPlateProductPartBytes,
                   Encoding.ASCII.GetBytes(productPartNo).Length);
                _hr40116 = (ushort)(loadPlateProductPartBytes[0] * 256 + loadPlateProductPartBytes[1]);
                _hr40117 = (ushort)(loadPlateProductPartBytes[2] * 256 + loadPlateProductPartBytes[3]);
                _hr40118 = (ushort)(loadPlateProductPartBytes[4] * 256 + loadPlateProductPartBytes[5]);
                _hr40119 = (ushort)(loadPlateProductPartBytes[6] * 256 + loadPlateProductPartBytes[7]);
                _hr40120 = (ushort)(loadPlateProductPartBytes[8] * 256 + loadPlateProductPartBytes[9]);
                Thread.Sleep(200);
                #endregion
            }
        }

        public void ReadLoadPlateStateCopyToComplete()
        {
            ReadLoadPlateState();
            CopyInfoToComplete();
        }

        private void CopyInfoToComplete()
        {
            LoadPlateProduct1CompleteState = LoadPlateProduct1State;
            LoadPlateProduct2CompleteState = LoadPlateProduct2State;
            LoadPlateProduct3CompleteState = LoadPlateProduct3State;
            LoadPlateProduct4CompleteState = LoadPlateProduct4State;

            LoadPlateCompleteProductNo = LoadPlateProductNo;

            LoadPlateProduct1CompleteBarcode = LoadPlateProduct1Barcode;
            LoadPlateProduct2CompleteBarcode = LoadPlateProduct2Barcode;
            LoadPlateProduct3CompleteBarcode = LoadPlateProduct3Barcode;
            LoadPlateProduct4CompleteBarcode = LoadPlateProduct4Barcode;

            LoadPlateCompleteProductPartNo = LoadPlateProductPartNo;
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

        #region 新增载盘记录PCBA二维码相关

        public string Pcba1Barcode;
        public string Pcba2Barcode;
        public string Pcba3Barcode;
        public string Pcba4Barcode;

        public void SavePcbaBarcode()
        {
            lock (_lockBarcode)
            {
                var strSql = new StringBuilder();
                strSql.Append("select count(1) from AllLedAutoAssemblyRunInfo");
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
                            "update AllLedAutoAssemblyRunInfo set " +
                            "Pcba1Barcode = '{0}', " +
                            "Pcba2Barcode = '{1}', " +
                            "Pcba3Barcode = '{2}'," +
                            "Pcba4Barcode = '{3}' where LoadPlateBarcode = '{4}'",
                            Pcba1Barcode,
                            Pcba2Barcode,
                            Pcba3Barcode,
                            Pcba4Barcode,
                            LoadPlateBarcode);

                    Query(updateSql);
                }
            }
        }

        public void ReadPcbaBarcode()
        {
            lock (_lockBarcode)
            {
                Pcba1Barcode = string.Empty;
                Pcba2Barcode = string.Empty;
                Pcba3Barcode = string.Empty;
                Pcba4Barcode = string.Empty;

                var strSql = new StringBuilder();
                strSql.Append("select count(1) from AllLedAutoAssemblyRunInfo");
                strSql.Append(" where LoadPlateBarcode=@loadPlateBarcode ");
                SqlParameter[] parameters =
                {
                    new SqlParameter("@loadPlateBarcode", SqlDbType.NVarChar, 200)
                };
                parameters[0].Value = LoadPlateProductNo; // 无线充电，借用这个当载盘二维码

                if (Exists(strSql.ToString(), parameters))
                {
                    var getSql =
                        string.Format(
                            "select Pcba1Barcode,Pcba2Barcode,Pcba3Barcode,Pcba4Barcode FROM AllLedAutoAssemblyRunInfo where LoadPlateBarcode = '{0}'",
                            LoadPlateProductNo); // 无线充电，借用这个当载盘二维码
                    var ds = Query(getSql);
                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        Pcba1Barcode = ds.Tables[0].DefaultView[0]["Pcba1Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Pcba1Barcode"].ToString();

                        Pcba2Barcode = ds.Tables[0].DefaultView[0]["Pcba2Barcode"] == null
                              ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Pcba2Barcode"].ToString();

                        Pcba3Barcode = ds.Tables[0].DefaultView[0]["Pcba3Barcode"] == null
                             ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Pcba3Barcode"].ToString();

                        Pcba4Barcode = ds.Tables[0].DefaultView[0]["Pcba4Barcode"] == null
                             ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Pcba4Barcode"].ToString();
                    }
                }
            }
        }

        public void ReadAlcmPcbaBarcode()
        {
            lock (_lockBarcode)
            {
                Pcba1Barcode = string.Empty;
                Pcba2Barcode = string.Empty;
                Pcba3Barcode = string.Empty;
                Pcba4Barcode = string.Empty;

                var strSql = new StringBuilder();
                strSql.Append("select count(1) from AllLedAutoAssemblyRunInfoB");
                strSql.Append(" where LoadPlateBarcode=@loadPlateBarcode ");
                SqlParameter[] parameters =
                {
                    new SqlParameter("@loadPlateBarcode", SqlDbType.NVarChar, 200)
                };
                parameters[0].Value = LoadPlateProductNo; // 无线充电，借用这个当载盘二维码
                //parameters[0].Value = "B09"; // 无线充电，借用这个当载盘二维码

                if (Exists(strSql.ToString(), parameters))
                {
                    var getSql =
                        string.Format(
                            "select Pcba1Barcode,Pcba2Barcode,Pcba3Barcode,Pcba4Barcode FROM AllLedAutoAssemblyRunInfoB where LoadPlateBarcode = '{0}'",
                            LoadPlateProductNo); // 无线充电，借用这个当载盘二维码
                    //var getSql =
                    //   string.Format(
                    //       "select Pcba1Barcode,Pcba2Barcode,Pcba3Barcode,Pcba4Barcode FROM AllLedAutoAssemblyRunInfoB where LoadPlateBarcode = '{0}'",
                    //       "B09"); // 无线充电，借用这个当载盘二维码
                    var ds = Query(getSql);
                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        Pcba1Barcode = ds.Tables[0].DefaultView[0]["Pcba1Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Pcba1Barcode"].ToString();

                        Pcba2Barcode = ds.Tables[0].DefaultView[0]["Pcba2Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Pcba2Barcode"].ToString();

                        Pcba3Barcode = ds.Tables[0].DefaultView[0]["Pcba3Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Pcba3Barcode"].ToString();

                        Pcba4Barcode = ds.Tables[0].DefaultView[0]["Pcba4Barcode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Pcba4Barcode"].ToString();
                    }
                }
            }
        }

        #endregion

        #region 扫码相关

        public string LoadPlateBarcode = string.Empty;
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

        public void PrintOkNg1(string filePath)
        {
            WriteTxt(filePath, LoadPlateProduct1State == 1 ? "Y" : ".");
        }

        public void PrintOkNg2(string filePath)
        {
            WriteTxt(filePath, LoadPlateProduct2State == 1 ? "Y" : ".");
        }

        public void PrintOkNg3(string filePath)
        {
            WriteTxt(filePath, LoadPlateProduct3State == 1 ? "Y" : ".");
        }

        public void PrintOkNg4(string filePath)
        {
            WriteTxt(filePath, LoadPlateProduct4State == 1 ? "Y" : ".");
        }

        public void PrintPartNo1(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProductPartNo;
                    if (string.IsNullOrEmpty(b))
                        return;
                    WriteTxt(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintHwNo1(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct1Barcode;
                    PrintHw(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintSwNo1(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct1Barcode;
                    PrintSw(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintSerialNo1(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct1Barcode;
                    PrintSerialNo(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintBarcode1(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct1Barcode;
                    if (string.IsNullOrEmpty(b))
                        return;
                    WriteTxt(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintPartNo2(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProductPartNo;
                    if (string.IsNullOrEmpty(b))
                        return;
                    WriteTxt(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintHwNo2(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct2Barcode;
                    PrintHw(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintSwNo2(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct2Barcode;
                    PrintSw(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintSerialNo2(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct2Barcode;
                    PrintSerialNo(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintBarcode2(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct2Barcode;
                    if (string.IsNullOrEmpty(b))
                        return;
                    WriteTxt(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintPartNo3(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProductPartNo;
                    if (string.IsNullOrEmpty(b))
                        return;
                    WriteTxt(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintHwNo3(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct3Barcode;
                    PrintHw(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintSwNo3(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct3Barcode;
                    PrintSw(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintSerialNo3(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct3Barcode;
                    PrintSerialNo(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintBarcode3(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct3Barcode;
                    if (string.IsNullOrEmpty(b))
                        return;
                    WriteTxt(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintPartNo4(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProductPartNo;
                    if (string.IsNullOrEmpty(b))
                        return;
                    WriteTxt(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintHwNo4(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct4Barcode;
                    PrintHw(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintSwNo4(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct4Barcode;
                    PrintSw(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintSerialNo4(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct4Barcode;
                    PrintSerialNo(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintBarcode4(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    var b = LoadPlateProduct4Barcode;
                    if (string.IsNullOrEmpty(b))
                        return;
                    WriteTxt(filePath, b);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void PrintHw(string filePath, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return;

                var hwNo = value.Substring(20, 4);
                WriteTxt(filePath, hwNo);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void PrintSw(string filePath, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return;

                var swNo = value.Substring(24, 4);
                WriteTxt(filePath, swNo);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void PrintSerialNo(string filePath, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return;

                var shift = value.Substring(30, 1);
                var serialNo = value.Substring(35, 4);
                var year = value.Substring(31, 1);
                var day = value.Substring(32, 3);
                var partNo = value.Substring(12, 4);

                var code = string.Format("LD{0}QN{1}{2}{3}{4}", partNo, shift, year, day, serialNo);
                WriteTxt(filePath, code);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void WriteTxt(string filePath, string value)
        {
            try
            {
                var code = value;

                var lines = File.ReadAllLines(filePath);
                var list = new List<string>();
                list.AddRange(lines);
                list.Clear();
                list.Add(code);
                lines = list.ToArray();

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void GenerateWirelessChargingBarcode(string partNo, string changeRecord, string swVer)
        {
            var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", RemoteSqlIp, "IPMS",
               "sa", "123456");

            var barcodeList = new List<string>();
            for (var i = 0; i < 4; i++)
            {
                using (var conn = new SqlConnection(sqlConnectiong))
                {
                    try
                    {
                        var cmd = new SqlCommand("GetCheckNo", conn) { CommandType = CommandType.StoredProcedure };
                        cmd.Parameters.AddWithValue("@Productno", partNo);  //给输入参数赋值
                        var parOutputDate = cmd.Parameters.Add("@date", SqlDbType.DateTime);  //定义输出参数 
                        parOutputDate.Direction = ParameterDirection.Output;  //参数类型为Output 
                        var returnSerialNo = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                        returnSerialNo.Direction = ParameterDirection.ReturnValue;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        var serialNo = returnSerialNo.Value.ToString();
                        var date = parOutputDate.Value.ToString();

                        var bs = new List<byte> { 0x5B, 0x29, 0x3E, 0x1E, 0x30, 0x36, 0x1D, 0x50 }; // [)><RS>06<GS>
                        bs.AddRange(Encoding.ASCII.GetBytes(partNo));
                        bs.AddRange(new byte[] { 0x1D, 0x31, 0x32, 0x56 }); // <GS>12V
                        bs.AddRange(new byte[] { 0x35, 0x34, 0x37, 0x36, 0x35, 0x36, 0x38, 0x36, 0x33 }); // 547656863
                        bs.AddRange(new byte[] { 0x1D, 0x54 }); // <GS>T
                        bs.AddRange(new byte[] { 0x53 }); // S
                        bs.AddRange(new byte[] { 0x4D }); // M
                        bs.AddRange(Encoding.ASCII.GetBytes(DateTime.Parse(date).Year.ToString().Substring(2, 2))); // year
                        bs.AddRange(Encoding.ASCII.GetBytes(DateTime.Parse(date).DayOfYear.ToString().PadLeft(3, '0'))); // day of year
                        bs.AddRange(new byte[] { 0x41 }); // A
                        bs.AddRange(Encoding.ASCII.GetBytes(changeRecord)); // change record
                        bs.AddRange(Encoding.ASCII.GetBytes(swVer)); // software ver
                        bs.AddRange(Encoding.ASCII.GetBytes(serialNo.PadLeft(4, '0'))); // serial number
                        bs.AddRange(new byte[] { 0x1E, 0x04 });
                        barcodeList.Add(Encoding.ASCII.GetString(bs.ToArray()));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }

            if (barcodeList.Count != 4)
            {
                LoadPlateProduct1Barcode = "FFFF";
                LoadPlateProduct2Barcode = "FFFF";
                LoadPlateProduct3Barcode = "FFFF";
                LoadPlateProduct4Barcode = "FFFF";
            }
            else
            {
                LoadPlateProduct1Barcode = barcodeList[0];
                LoadPlateProduct2Barcode = barcodeList[0];
                LoadPlateProduct3Barcode = barcodeList[0];
                LoadPlateProduct4Barcode = barcodeList[0];
            }
        }

        public ushort WtireBarcode1Ok;
        public ushort WtireBarcode2Ok;
        public ushort WtireBarcode3Ok;
        public ushort WtireBarcode4Ok;

        public void WriteWirelessChargingSerialNum(string index, string path)
        {
            try
            {
                var barcode = string.Empty;
                //var barcode = "[)>06Y8210700000000XP2646074112V547656863T1A22238AAA000005";
                if (index == 1.ToString())
                    barcode = LoadPlateProduct1Barcode;
                else if (index == 2.ToString())
                    barcode = LoadPlateProduct2Barcode;
                else if (index == 3.ToString())
                    barcode = LoadPlateProduct3Barcode;
                else if (index == 4.ToString())
                    barcode = LoadPlateProduct4Barcode;
                var trackInfo = barcode.Substring(barcode.Length - 16 - 2, 16);
                WriteTxt(path, trackInfo);

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
                WriteTxt(path, ".");

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

        #endregion

        #region mobbus client相关
        private readonly object _lockBarcode = new object();

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
                        LoadPlateProduct1State = _hr40001;
                        LoadPlateProduct2State = _hr40002;
                        LoadPlateProduct3State = _hr40003;
                        LoadPlateProduct4State = _hr40004;
                        StartSignal = _hr40005;

                        var productNoBytes = new List<byte>();
                        productNoBytes.AddRange(BitConverter.GetBytes(_hr40006).Reverse());
                        productNoBytes.AddRange(BitConverter.GetBytes(_hr40007).Reverse());
                        productNoBytes.AddRange(BitConverter.GetBytes(_hr40008).Reverse());
                        productNoBytes.AddRange(BitConverter.GetBytes(_hr40009).Reverse());
                        productNoBytes.AddRange(BitConverter.GetBytes(_hr40010).Reverse());
                        productNoBytes.AddRange(BitConverter.GetBytes(_hr40011).Reverse());
                        productNoBytes.AddRange(BitConverter.GetBytes(_hr40012).Reverse());
                        productNoBytes.AddRange(BitConverter.GetBytes(_hr40013).Reverse());
                        productNoBytes.AddRange(BitConverter.GetBytes(_hr40014).Reverse());
                        productNoBytes.AddRange(BitConverter.GetBytes(_hr40015).Reverse());
                        var temp1 = new List<byte>();
                        for (int i = 0; i < productNoBytes.Count; i++)
                            if (productNoBytes[i] != 0x00)
                                temp1.Add(productNoBytes[i]);
                        LoadPlateProductNo = Encoding.ASCII.GetString(temp1.ToArray());

                        var loadPlateProduct1BarcodeBytes = new List<byte>();
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40016).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40017).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40018).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40019).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40020).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40021).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40022).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40023).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40024).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40025).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40026).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40027).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40028).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40029).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40030).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40031).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40032).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40033).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40034).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40035).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40036).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40037).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40038).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40039).Reverse());
                        loadPlateProduct1BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40040).Reverse());
                        var temp2 = new List<byte>();
                        for (int i = 0; i < loadPlateProduct1BarcodeBytes.Count; i++)
                            if (loadPlateProduct1BarcodeBytes[i] != 0x00)
                                temp2.Add(loadPlateProduct1BarcodeBytes[i]);
                        LoadPlateProduct1Barcode = Encoding.ASCII.GetString(temp2.ToArray());

                        var loadPlateProduct2BarcodeBytes = new List<byte>();
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40041).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40042).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40043).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40044).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40045).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40046).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40047).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40048).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40049).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40050).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40051).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40052).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40053).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40054).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40055).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40056).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40057).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40058).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40059).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40060).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40061).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40062).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40063).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40064).Reverse());
                        loadPlateProduct2BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40065).Reverse());
                        var temp3 = new List<byte>();
                        for (int i = 0; i < loadPlateProduct2BarcodeBytes.Count; i++)
                            if (loadPlateProduct2BarcodeBytes[i] != 0x00)
                                temp3.Add(loadPlateProduct2BarcodeBytes[i]);
                        LoadPlateProduct2Barcode = Encoding.ASCII.GetString(temp3.ToArray());

                        var loadPlateProduct3BarcodeBytes = new List<byte>();
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40066).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40067).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40068).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40069).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40070).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40071).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40072).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40073).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40074).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40075).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40076).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40077).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40078).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40079).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40080).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40081).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40082).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40083).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40084).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40085).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40086).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40087).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40088).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40089).Reverse());
                        loadPlateProduct3BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40090).Reverse());
                        var temp4 = new List<byte>();
                        for (int i = 0; i < loadPlateProduct3BarcodeBytes.Count; i++)
                            if (loadPlateProduct3BarcodeBytes[i] != 0x00)
                                temp4.Add(loadPlateProduct3BarcodeBytes[i]);
                        LoadPlateProduct3Barcode = Encoding.ASCII.GetString(temp4.ToArray());

                        var loadPlateProduct4BarcodeBytes = new List<byte>();
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40091).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40092).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40093).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40094).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40095).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40096).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40097).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40098).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40099).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40100).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40101).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40102).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40103).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40104).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40105).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40106).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40107).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40108).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40109).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40110).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40111).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40112).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40113).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40114).Reverse());
                        loadPlateProduct4BarcodeBytes.AddRange(BitConverter.GetBytes(_hr40115).Reverse());
                        var temp5 = new List<byte>();
                        for (int i = 0; i < loadPlateProduct4BarcodeBytes.Count; i++)
                            if (loadPlateProduct4BarcodeBytes[i] != 0x00)
                                temp5.Add(loadPlateProduct4BarcodeBytes[i]);
                        LoadPlateProduct4Barcode = Encoding.ASCII.GetString(temp5.ToArray());

                        var loadPlateProductPartNoBytes = new List<byte>();
                        loadPlateProductPartNoBytes.AddRange(BitConverter.GetBytes(_hr40116).Reverse());
                        loadPlateProductPartNoBytes.AddRange(BitConverter.GetBytes(_hr40117).Reverse());
                        loadPlateProductPartNoBytes.AddRange(BitConverter.GetBytes(_hr40118).Reverse());
                        loadPlateProductPartNoBytes.AddRange(BitConverter.GetBytes(_hr40119).Reverse());
                        loadPlateProductPartNoBytes.AddRange(BitConverter.GetBytes(_hr40120).Reverse());
                        var temp6 = new List<byte>();
                        for (int i = 0; i < loadPlateProductPartNoBytes.Count; i++)
                            if (loadPlateProductPartNoBytes[i] != 0x00)
                                temp6.Add(loadPlateProductPartNoBytes[i]);
                        LoadPlateProductPartNo = "P" + Encoding.ASCII.GetString(temp6.ToArray());
                        #endregion

                        if (ModbusClientReadMaxLenth > 120)
                            continue;
                        {
                            #region 赋值
                            _hr40121 = ReadySignal;
                            _hr40122 = LoadPlateProduct1CompleteState;
                            _hr40123 = LoadPlateProduct2CompleteState;
                            _hr40124 = LoadPlateProduct3CompleteState;
                            _hr40125 = LoadPlateProduct4CompleteState;
                            _hr40126 = CompleteSignal;

                            var productNoCompleteBytes = new byte[10 * 2];
                            Array.Copy(Encoding.ASCII.GetBytes(LoadPlateCompleteProductNo), productNoCompleteBytes,
                                Encoding.ASCII.GetBytes(LoadPlateCompleteProductNo).Length);
                            _hr40127 = (ushort)(productNoCompleteBytes[0] * 256 + productNoCompleteBytes[1]);
                            _hr40128 = (ushort)(productNoCompleteBytes[2] * 256 + productNoCompleteBytes[3]);
                            _hr40129 = (ushort)(productNoCompleteBytes[4] * 256 + productNoCompleteBytes[5]);
                            _hr40130 = (ushort)(productNoCompleteBytes[6] * 256 + productNoCompleteBytes[7]);
                            _hr40131 = (ushort)(productNoCompleteBytes[8] * 256 + productNoCompleteBytes[9]);
                            _hr40132 = (ushort)(productNoCompleteBytes[10] * 256 + productNoCompleteBytes[11]);
                            _hr40133 = (ushort)(productNoCompleteBytes[12] * 256 + productNoCompleteBytes[13]);
                            _hr40134 = (ushort)(productNoCompleteBytes[14] * 256 + productNoCompleteBytes[15]);
                            _hr40135 = (ushort)(productNoCompleteBytes[16] * 256 + productNoCompleteBytes[17]);
                            _hr40136 = (ushort)(productNoCompleteBytes[18] * 256 + productNoCompleteBytes[19]);

                            var loadPlateProduct1BarcodeCompleteBytes = new byte[50];
                            Array.Copy(Encoding.ASCII.GetBytes(LoadPlateProduct1CompleteBarcode), loadPlateProduct1BarcodeCompleteBytes,
                                Encoding.ASCII.GetBytes(LoadPlateProduct1CompleteBarcode).Length);
                            _hr40137 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[0] * 256 + loadPlateProduct1BarcodeCompleteBytes[1]);
                            _hr40138 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[2] * 256 + loadPlateProduct1BarcodeCompleteBytes[3]);
                            _hr40139 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[4] * 256 + loadPlateProduct1BarcodeCompleteBytes[5]);
                            _hr40140 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[6] * 256 + loadPlateProduct1BarcodeCompleteBytes[7]);
                            _hr40141 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[8] * 256 + loadPlateProduct1BarcodeCompleteBytes[9]);
                            _hr40142 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[10] * 256 + loadPlateProduct1BarcodeCompleteBytes[11]);
                            _hr40143 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[12] * 256 + loadPlateProduct1BarcodeCompleteBytes[13]);
                            _hr40144 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[14] * 256 + loadPlateProduct1BarcodeCompleteBytes[15]);
                            _hr40145 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[16] * 256 + loadPlateProduct1BarcodeCompleteBytes[17]);
                            _hr40146 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[18] * 256 + loadPlateProduct1BarcodeCompleteBytes[19]);
                            _hr40147 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[20] * 256 + loadPlateProduct1BarcodeCompleteBytes[21]);
                            _hr40148 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[22] * 256 + loadPlateProduct1BarcodeCompleteBytes[23]);
                            _hr40149 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[24] * 256 + loadPlateProduct1BarcodeCompleteBytes[25]);
                            _hr40150 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[26] * 256 + loadPlateProduct1BarcodeCompleteBytes[27]);
                            _hr40151 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[28] * 256 + loadPlateProduct1BarcodeCompleteBytes[29]);
                            _hr40152 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[30] * 256 + loadPlateProduct1BarcodeCompleteBytes[31]);
                            _hr40153 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[32] * 256 + loadPlateProduct1BarcodeCompleteBytes[33]);
                            _hr40154 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[34] * 256 + loadPlateProduct1BarcodeCompleteBytes[35]);
                            _hr40155 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[36] * 256 + loadPlateProduct1BarcodeCompleteBytes[37]);
                            _hr40156 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[38] * 256 + loadPlateProduct1BarcodeCompleteBytes[39]);
                            _hr40157 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[40] * 256 + loadPlateProduct1BarcodeCompleteBytes[41]);
                            _hr40158 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[42] * 256 + loadPlateProduct1BarcodeCompleteBytes[43]);
                            _hr40159 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[44] * 256 + loadPlateProduct1BarcodeCompleteBytes[45]);
                            _hr40160 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[46] * 256 + loadPlateProduct1BarcodeCompleteBytes[47]);
                            _hr40161 = (ushort)(loadPlateProduct1BarcodeCompleteBytes[48] * 256 + loadPlateProduct1BarcodeCompleteBytes[49]);

                            var loadPlateProduct2BarcodeCompleteBytes = new byte[50];
                            Array.Copy(Encoding.ASCII.GetBytes(LoadPlateProduct2CompleteBarcode), loadPlateProduct2BarcodeCompleteBytes,
                                Encoding.ASCII.GetBytes(LoadPlateProduct2CompleteBarcode).Length);
                            _hr40162 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[0] * 256 + loadPlateProduct2BarcodeCompleteBytes[1]);
                            _hr40163 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[2] * 256 + loadPlateProduct2BarcodeCompleteBytes[3]);
                            _hr40164 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[4] * 256 + loadPlateProduct2BarcodeCompleteBytes[5]);
                            _hr40165 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[6] * 256 + loadPlateProduct2BarcodeCompleteBytes[7]);
                            _hr40166 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[8] * 256 + loadPlateProduct2BarcodeCompleteBytes[9]);
                            _hr40167 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[10] * 256 + loadPlateProduct2BarcodeCompleteBytes[11]);
                            _hr40168 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[12] * 256 + loadPlateProduct2BarcodeCompleteBytes[13]);
                            _hr40169 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[14] * 256 + loadPlateProduct2BarcodeCompleteBytes[15]);
                            _hr40170 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[16] * 256 + loadPlateProduct2BarcodeCompleteBytes[17]);
                            _hr40171 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[18] * 256 + loadPlateProduct2BarcodeCompleteBytes[19]);
                            _hr40172 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[20] * 256 + loadPlateProduct2BarcodeCompleteBytes[21]);
                            _hr40173 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[22] * 256 + loadPlateProduct2BarcodeCompleteBytes[23]);
                            _hr40174 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[24] * 256 + loadPlateProduct2BarcodeCompleteBytes[25]);
                            _hr40175 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[26] * 256 + loadPlateProduct2BarcodeCompleteBytes[27]);
                            _hr40176 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[28] * 256 + loadPlateProduct2BarcodeCompleteBytes[29]);
                            _hr40177 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[30] * 256 + loadPlateProduct2BarcodeCompleteBytes[31]);
                            _hr40178 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[32] * 256 + loadPlateProduct2BarcodeCompleteBytes[33]);
                            _hr40179 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[34] * 256 + loadPlateProduct2BarcodeCompleteBytes[35]);
                            _hr40180 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[36] * 256 + loadPlateProduct2BarcodeCompleteBytes[37]);
                            _hr40181 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[38] * 256 + loadPlateProduct2BarcodeCompleteBytes[39]);
                            _hr40182 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[40] * 256 + loadPlateProduct2BarcodeCompleteBytes[41]);
                            _hr40183 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[42] * 256 + loadPlateProduct2BarcodeCompleteBytes[43]);
                            _hr40184 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[44] * 256 + loadPlateProduct2BarcodeCompleteBytes[45]);
                            _hr40185 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[46] * 256 + loadPlateProduct2BarcodeCompleteBytes[47]);
                            _hr40186 = (ushort)(loadPlateProduct2BarcodeCompleteBytes[48] * 256 + loadPlateProduct2BarcodeCompleteBytes[49]);

                            var loadPlateProduct3BarcodeCompleteBytes = new byte[50];
                            Array.Copy(Encoding.ASCII.GetBytes(LoadPlateProduct3CompleteBarcode), loadPlateProduct3BarcodeCompleteBytes,
                                Encoding.ASCII.GetBytes(LoadPlateProduct3CompleteBarcode).Length);
                            _hr40187 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[0] * 256 + loadPlateProduct3BarcodeCompleteBytes[1]);
                            _hr40188 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[2] * 256 + loadPlateProduct3BarcodeCompleteBytes[3]);
                            _hr40189 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[4] * 256 + loadPlateProduct3BarcodeCompleteBytes[5]);
                            _hr40190 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[6] * 256 + loadPlateProduct3BarcodeCompleteBytes[7]);
                            _hr40191 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[8] * 256 + loadPlateProduct3BarcodeCompleteBytes[9]);
                            _hr40192 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[10] * 256 + loadPlateProduct3BarcodeCompleteBytes[11]);
                            _hr40193 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[12] * 256 + loadPlateProduct3BarcodeCompleteBytes[13]);
                            _hr40194 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[14] * 256 + loadPlateProduct3BarcodeCompleteBytes[15]);
                            _hr40195 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[16] * 256 + loadPlateProduct3BarcodeCompleteBytes[17]);
                            _hr40196 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[18] * 256 + loadPlateProduct3BarcodeCompleteBytes[19]);
                            _hr40197 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[20] * 256 + loadPlateProduct3BarcodeCompleteBytes[21]);
                            _hr40198 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[22] * 256 + loadPlateProduct3BarcodeCompleteBytes[23]);
                            _hr40199 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[24] * 256 + loadPlateProduct3BarcodeCompleteBytes[25]);
                            _hr40200 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[26] * 256 + loadPlateProduct3BarcodeCompleteBytes[27]);
                            _hr40201 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[28] * 256 + loadPlateProduct3BarcodeCompleteBytes[29]);
                            _hr40202 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[30] * 256 + loadPlateProduct3BarcodeCompleteBytes[31]);
                            _hr40203 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[32] * 256 + loadPlateProduct3BarcodeCompleteBytes[33]);
                            _hr40204 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[34] * 256 + loadPlateProduct3BarcodeCompleteBytes[35]);
                            _hr40205 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[36] * 256 + loadPlateProduct3BarcodeCompleteBytes[37]);
                            _hr40206 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[38] * 256 + loadPlateProduct3BarcodeCompleteBytes[39]);
                            _hr40207 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[40] * 256 + loadPlateProduct3BarcodeCompleteBytes[41]);
                            _hr40208 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[42] * 256 + loadPlateProduct3BarcodeCompleteBytes[43]);
                            _hr40209 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[44] * 256 + loadPlateProduct3BarcodeCompleteBytes[45]);
                            _hr40210 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[46] * 256 + loadPlateProduct3BarcodeCompleteBytes[47]);
                            _hr40211 = (ushort)(loadPlateProduct3BarcodeCompleteBytes[48] * 256 + loadPlateProduct3BarcodeCompleteBytes[49]);

                            var loadPlateProduct4BarcodeCompleteBytes = new byte[50];
                            Array.Copy(Encoding.ASCII.GetBytes(LoadPlateProduct4CompleteBarcode), loadPlateProduct4BarcodeCompleteBytes,
                                Encoding.ASCII.GetBytes(LoadPlateProduct4CompleteBarcode).Length);
                            _hr40212 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[0] * 256 + loadPlateProduct4BarcodeCompleteBytes[1]);
                            _hr40213 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[2] * 256 + loadPlateProduct4BarcodeCompleteBytes[3]);
                            _hr40214 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[4] * 256 + loadPlateProduct4BarcodeCompleteBytes[5]);
                            _hr40215 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[6] * 256 + loadPlateProduct4BarcodeCompleteBytes[7]);
                            _hr40216 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[8] * 256 + loadPlateProduct4BarcodeCompleteBytes[9]);
                            _hr40217 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[10] * 256 + loadPlateProduct4BarcodeCompleteBytes[11]);
                            _hr40218 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[12] * 256 + loadPlateProduct4BarcodeCompleteBytes[13]);
                            _hr40219 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[14] * 256 + loadPlateProduct4BarcodeCompleteBytes[15]);
                            _hr40220 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[16] * 256 + loadPlateProduct4BarcodeCompleteBytes[17]);
                            _hr40221 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[18] * 256 + loadPlateProduct4BarcodeCompleteBytes[19]);
                            _hr40222 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[20] * 256 + loadPlateProduct4BarcodeCompleteBytes[21]);
                            _hr40223 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[22] * 256 + loadPlateProduct4BarcodeCompleteBytes[23]);
                            _hr40224 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[24] * 256 + loadPlateProduct4BarcodeCompleteBytes[25]);
                            _hr40225 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[26] * 256 + loadPlateProduct4BarcodeCompleteBytes[27]);
                            _hr40226 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[28] * 256 + loadPlateProduct4BarcodeCompleteBytes[29]);
                            _hr40227 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[30] * 256 + loadPlateProduct4BarcodeCompleteBytes[31]);
                            _hr40228 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[32] * 256 + loadPlateProduct4BarcodeCompleteBytes[33]);
                            _hr40229 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[34] * 256 + loadPlateProduct4BarcodeCompleteBytes[35]);
                            _hr40230 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[36] * 256 + loadPlateProduct4BarcodeCompleteBytes[37]);
                            _hr40231 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[38] * 256 + loadPlateProduct4BarcodeCompleteBytes[39]);
                            _hr40232 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[40] * 256 + loadPlateProduct4BarcodeCompleteBytes[41]);
                            _hr40233 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[42] * 256 + loadPlateProduct4BarcodeCompleteBytes[43]);
                            _hr40234 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[44] * 256 + loadPlateProduct4BarcodeCompleteBytes[45]);
                            _hr40235 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[46] * 256 + loadPlateProduct4BarcodeCompleteBytes[47]);
                            _hr40236 = (ushort)(loadPlateProduct4BarcodeCompleteBytes[48] * 256 + loadPlateProduct4BarcodeCompleteBytes[49]);

                            var loadPlateProductPartCompleteBytes = new byte[10];
                            var productPartNo = string.Empty;
                            if (!string.IsNullOrEmpty(LoadPlateCompleteProductPartNo) && LoadPlateCompleteProductPartNo.Length > 2)
                                productPartNo = LoadPlateCompleteProductPartNo.Substring(1, LoadPlateCompleteProductPartNo.Length - 1);
                            Array.Copy(Encoding.ASCII.GetBytes(productPartNo), loadPlateProductPartCompleteBytes,
                               Encoding.ASCII.GetBytes(productPartNo).Length);
                            _hr40237 = (ushort)(loadPlateProductPartCompleteBytes[0] * 256 + loadPlateProductPartCompleteBytes[1]);
                            _hr40238 = (ushort)(loadPlateProductPartCompleteBytes[2] * 256 + loadPlateProductPartCompleteBytes[3]);
                            _hr40239 = (ushort)(loadPlateProductPartCompleteBytes[4] * 256 + loadPlateProductPartCompleteBytes[5]);
                            _hr40240 = (ushort)(loadPlateProductPartCompleteBytes[6] * 256 + loadPlateProductPartCompleteBytes[7]);
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

                    var productNoCompleteBytes = new List<byte>();
                    productNoCompleteBytes.AddRange(BitConverter.GetBytes(_hr40127).Reverse());
                    productNoCompleteBytes.AddRange(BitConverter.GetBytes(_hr40128).Reverse());
                    productNoCompleteBytes.AddRange(BitConverter.GetBytes(_hr40129).Reverse());
                    productNoCompleteBytes.AddRange(BitConverter.GetBytes(_hr40130).Reverse());
                    productNoCompleteBytes.AddRange(BitConverter.GetBytes(_hr40131).Reverse());
                    productNoCompleteBytes.AddRange(BitConverter.GetBytes(_hr40132).Reverse());
                    productNoCompleteBytes.AddRange(BitConverter.GetBytes(_hr40133).Reverse());
                    productNoCompleteBytes.AddRange(BitConverter.GetBytes(_hr40134).Reverse());
                    productNoCompleteBytes.AddRange(BitConverter.GetBytes(_hr40135).Reverse());
                    productNoCompleteBytes.AddRange(BitConverter.GetBytes(_hr40136).Reverse());
                    LoadPlateCompleteProductNo = Encoding.ASCII.GetString(productNoCompleteBytes.Where(t => t != 0x00).ToArray());

                    var loadPlateProduct1BarcodeCompleteBytes = new List<byte>();
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40137).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40138).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40139).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40140).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40141).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40142).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40143).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40144).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40145).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40146).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40147).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40148).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40149).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40150).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40151).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40152).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40153).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40154).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40155).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40156).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40157).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40158).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40159).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40160).Reverse());
                    loadPlateProduct1BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40161).Reverse());
                    LoadPlateProduct1CompleteBarcode = Encoding.ASCII.GetString(loadPlateProduct1BarcodeCompleteBytes.Where(t => t != 0x00).ToArray());

                    var loadPlateProduct2BarcodeCompleteBytes = new List<byte>();
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40162).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40163).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40164).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40165).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40166).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40167).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40168).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40169).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40170).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40171).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40172).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40173).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40174).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40175).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40176).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40177).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40178).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40179).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40180).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40181).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40182).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40183).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40184).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40185).Reverse());
                    loadPlateProduct2BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40186).Reverse());
                    var temp3 = new List<byte>();
                    for (int i = 0; i < loadPlateProduct2BarcodeCompleteBytes.Count; i++)
                        if (loadPlateProduct2BarcodeCompleteBytes[i] != 0x00)
                            temp3.Add(loadPlateProduct2BarcodeCompleteBytes[i]);
                    LoadPlateProduct2CompleteBarcode = Encoding.ASCII.GetString(temp3.ToArray());

                    var loadPlateProduct3BarcodeCompleteBytes = new List<byte>();
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40187).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40188).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40189).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40190).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40191).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40192).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40193).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40194).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40195).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40196).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40197).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40198).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40199).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40200).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40201).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40202).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40203).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40204).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40205).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40206).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40207).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40208).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40209).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40210).Reverse());
                    loadPlateProduct3BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40211).Reverse());
                    var temp4 = new List<byte>();
                    for (int i = 0; i < loadPlateProduct3BarcodeCompleteBytes.Count; i++)
                        if (loadPlateProduct3BarcodeCompleteBytes[i] != 0x00)
                            temp4.Add(loadPlateProduct3BarcodeCompleteBytes[i]);
                    LoadPlateProduct3CompleteBarcode = Encoding.ASCII.GetString(temp4.ToArray());

                    var loadPlateProduct4BarcodeCompleteBytes = new List<byte>();
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40212).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40213).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40214).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40215).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40216).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40217).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40218).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40219).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40220).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40221).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40222).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40223).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40224).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40225).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40226).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40227).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40228).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40229).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40230).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40231).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40232).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40233).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40234).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40235).Reverse());
                    loadPlateProduct4BarcodeCompleteBytes.AddRange(BitConverter.GetBytes(_hr40236).Reverse());
                    var temp5 = new List<byte>();
                    for (int i = 0; i < loadPlateProduct4BarcodeCompleteBytes.Count; i++)
                        if (loadPlateProduct4BarcodeCompleteBytes[i] != 0x00)
                            temp5.Add(loadPlateProduct4BarcodeCompleteBytes[i]);
                    LoadPlateProduct4CompleteBarcode = Encoding.ASCII.GetString(temp5.ToArray());

                    var loadPlateProductPartCompleteBytes = new List<byte>();
                    loadPlateProductPartCompleteBytes.AddRange(BitConverter.GetBytes(_hr40237).Reverse());
                    loadPlateProductPartCompleteBytes.AddRange(BitConverter.GetBytes(_hr40238).Reverse());
                    loadPlateProductPartCompleteBytes.AddRange(BitConverter.GetBytes(_hr40239).Reverse());
                    loadPlateProductPartCompleteBytes.AddRange(BitConverter.GetBytes(_hr40240).Reverse());
                    var temp6 = new List<byte>();
                    for (int i = 0; i < loadPlateProductPartCompleteBytes.Count; i++)
                        if (loadPlateProductPartCompleteBytes[i] != 0x00)
                            temp6.Add(loadPlateProductPartCompleteBytes[i]);
                    LoadPlateCompleteProductPartNo = "P" + Encoding.ASCII.GetString(temp6.ToArray());
                    #endregion
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        #endregion

        public ushort ReadySignal; // 就绪信号,server读,client写 // 400121
        public ushort LoadPlateProduct1CompleteState; // 完成信号,server读,client写 // 400122
        public ushort LoadPlateProduct2CompleteState; // 完成信号,server读,client写 // 400123
        public ushort LoadPlateProduct3CompleteState; // 完成信号,server读,client写 // 400124
        public ushort LoadPlateProduct4CompleteState; // 完成信号,server读,client写 // 400125
        public ushort CompleteSignal; // 完成信号,server读,client写 // 400126
        public string LoadPlateCompleteProductNo = string.Empty; // 完成信号,server读,client写 // 400127~40136 // 无线充电，借用这个当载盘二维码
        public string LoadPlateProduct1CompleteBarcode = string.Empty; // 完成信号,server读,client写 // 400137~40161
        public string LoadPlateProduct2CompleteBarcode = string.Empty; // 完成信号,server读,client写 // 400162~40186
        public string LoadPlateProduct3CompleteBarcode = string.Empty; // 完成信号,server读,client写 // 400187~40211
        public string LoadPlateProduct4CompleteBarcode = string.Empty; // 完成信号,server读,client写 // 400212~40236
        public string LoadPlateCompleteProductPartNo = string.Empty; // 完成信号,server读,client写 // 400237~40240 

        public ushort LoadPlateProduct1State; // 载盘产品1状态,server写,client读 // 40001
        public ushort LoadPlateProduct2State; // 载盘产品2状态,server写,client读 // 40002
        public ushort LoadPlateProduct3State; // 载盘产品3状态,server写,client读 // 40003
        public ushort LoadPlateProduct4State; // 载盘产品4状态,server写,client读 // 40004
        public ushort StartSignal; // 启动信号,server写,client读  // 40005
        public string LoadPlateProductNo = string.Empty; // 载盘产品号,server写,client读  // 40006~40015  // 无线充电，借用这个当载盘二维码
        public string LoadPlateProduct1Barcode = string.Empty; // 载盘产品1二维码,server写,client读 // 40016~40040
        public string LoadPlateProduct2Barcode = string.Empty; // 载盘产品2二维码,server写,client读 // 40041~40065
        public string LoadPlateProduct3Barcode = string.Empty; // 载盘产品3二维码,server写,client读 // 40066~40090
        public string LoadPlateProduct4Barcode = string.Empty; // 载盘产品4二维码,server写,client读 // 40091~40115
        public string LoadPlateProductPartNo = string.Empty;  // 载盘产品总成号,server写,client读 // 40116~40120
    }
}
