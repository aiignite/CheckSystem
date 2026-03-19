using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CheckSystem.MaterialHelperForms.MaterialModels
{
    public static class HikDbHelperSql
    {
        public static string RemoteSqlIp = "192.168.0.138";
        public static string DatabaseName = "PLMS";
        public static string SqlUserName = "ipms";
        public static string SqlPassword = "Scae2020#";

        public static string UserName;
        public static string UserCount;

        private static SqlConnection _connection;

        public static int Update(string strSql)
        {
            var sql = strSql;

            try
            {
                using (var cmd = new SqlCommand(sql, GetConnection()))
                {
                    cmd.Prepare();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return -1;
            }

            //var dataReader = GetDataReader(sql);
            //var list = new List<string>();
            //var strText = string.Empty;
            //if (dataReader == null)
            //    return new List<string>();
            //try
            //{
            //    while (dataReader.Read())
            //    {
            //        if (!dataReader.IsDBNull(0))
            //            strText = dataReader.GetString(0);
            //        list.Add(strText);
            //    }
            //    dataReader.Close();
            //    return list;
            //}
            //catch (Exception)
            //{
            //    return new List<string>();
            //}
        }

        public static List<string> GetData(string strSql)
        {
            var sql = strSql;
            var dataReader = GetDataReader(sql);
            var list = new List<string>();
            var strText = string.Empty;
            if (dataReader == null)
                return new List<string>();
            try
            {
                while (dataReader.Read())
                {
                    if (!dataReader.IsDBNull(0))
                        strText = dataReader.GetString(0);
                    list.Add(strText);
                }
                dataReader.Close();
                return list;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// 得到一个DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql)
        {
            try
            {
                using (var cmd = new SqlCommand(sql, GetConnection()))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        var dt = new DataTable();
                        dt.Load(dr);
                        cmd.Prepare();
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        /// <summary>
        /// 得到一个DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, ref string error)
        {
            try
            {
                using (var cmd = new SqlCommand(sql, GetConnection()))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        var dt = new DataTable();
                        dt.Load(dr);
                        cmd.Prepare();
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                error += ex.Message;
                return new DataTable();
            }
        }


        /// <summary>
        /// 得到一个SqlDataReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public static SqlDataReader GetDataReader(string sql)
        {
            try
            {
                using (var cmd = new SqlCommand(sql, GetConnection()))
                {
                    cmd.Prepare();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static SqlConnection GetConnection()
        {
            try
            {
                if (_connection == null)
                {
                    var sqlConnectiong = @"server=" + RemoteSqlIp + ";database=" + DatabaseName + ";uid="+ SqlUserName + ";pwd=" + SqlPassword + "";
                    _connection = new SqlConnection(sqlConnectiong);
                    _connection.Open();
                }
                else if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 得到NewMaterialStockInInfo的对象列表
        /// <param name="whereValue"></param>
        /// </summary>
        public static List<NewMaterialStockInInfo> GetNewMaterialStockInInfoModels(string whereValue)
        {
            var sqlStr = string.Format("select [id],[StockInNo],[MaterialId],[MaterialNo],[MaterialName],[ModelName],[SupplyNo],[Qualevel],[Issporadic],[Earmarks],[NeedNum],[ScanNum],[RestNum],[Status],[CompletionTime],[CancelTime],[CancelResult],[Note],[CreateTime],[Creator],[Editor],[EditTime],[IsValid],[IsDelete],[ParentId],[ItemOrder],[ItemVersionId],[ItemVersionNo],[ItemWorkflowCode],[ItemWorkflowStatus],[ItemWorkflowId] from newMaterialStockInInfo where  {0}", whereValue);
            var dataTable = GetDataTable(sqlStr);

            var returnList = new List<NewMaterialStockInInfo>();

            for (var i = 0; i < dataTable.DefaultView.Count; i++)
            {
                var model = new NewMaterialStockInInfo();
                var row = dataTable.DefaultView[i];

                if (row != null)
                {
                    if (row["id"] != null && row["id"].ToString() != "")
                    {
                        model.id = int.Parse(row["id"].ToString());
                    }
                    if (row["StockInNo"] != null)
                    {
                        model.StockInNo = row["StockInNo"].ToString();
                    }
                    if (row["MaterialId"] != null)
                    {
                        model.MaterialId = row["MaterialId"].ToString();
                    }
                    if (row["MaterialNo"] != null)
                    {
                        model.MaterialNo = row["MaterialNo"].ToString();
                    }
                    if (row["MaterialName"] != null)
                    {
                        model.MaterialName = row["MaterialName"].ToString();
                    }
                    if (row["ModelName"] != null)
                    {
                        model.ModelName = row["ModelName"].ToString();
                    }
                    if (row["SupplyNo"] != null)
                    {
                        model.SupplyNo = row["SupplyNo"].ToString();
                    }
                    if (row["Qualevel"] != null)
                    {
                        model.Qualevel = row["Qualevel"].ToString();
                    }
                    if (row["Issporadic"] != null)
                    {
                        model.Issporadic = row["Issporadic"].ToString();
                    }
                    if (row["Earmarks"] != null)
                    {
                        model.Earmarks = row["Earmarks"].ToString();
                    }
                    if (row["NeedNum"] != null && row["NeedNum"].ToString() != "")
                    {
                        model.NeedNum = int.Parse(row["NeedNum"].ToString());
                    }
                    if (row["ScanNum"] != null && row["ScanNum"].ToString() != "")
                    {
                        model.ScanNum = int.Parse(row["ScanNum"].ToString());
                    }
                    if (row["RestNum"] != null && row["RestNum"].ToString() != "")
                    {
                        model.RestNum = int.Parse(row["RestNum"].ToString());
                    }
                    if (row["Status"] != null)
                    {
                        model.Status = row["Status"].ToString();
                    }
                    if (row["CompletionTime"] != null && row["CompletionTime"].ToString() != "")
                    {
                        model.CompletionTime = DateTime.Parse(row["CompletionTime"].ToString());
                    }
                    if (row["CancelTime"] != null && row["CancelTime"].ToString() != "")
                    {
                        model.CancelTime = DateTime.Parse(row["CancelTime"].ToString());
                    }
                    if (row["CancelResult"] != null)
                    {
                        model.CancelResult = row["CancelResult"].ToString();
                    }
                    if (row["Note"] != null)
                    {
                        model.Note = row["Note"].ToString();
                    }
                }

                returnList.Add(model);
            }

            return returnList;
        }

        /// <summary>
        /// 得到NewMaterialPrintCorrespond的对象列表
        /// </summary>
        /// <param name="whereValue"></param>
        /// <returns></returns>
        public static List<NewMaterialPrintCorrespond> GetNewMaterialPrintCorrespondModels(string whereValue)
        {
            //var sql =
            //    string.Format(
            //        "select [id],[MaterialBarcode],[MaterialId],[MaterialNo],[MaterialName],[ModelName],[SupplyLedGroup],[SupplyLedNo],[Count],[BrandName],[Barcodetype],[BarcodeCount],[PartNokey],[LotNokey],[DcNokey],[Qtykey],[LEDcatkey],[Status],[Remarks],[Creator],[CreateTime],[Editor],[EditTime],[IsValid],[IsDelete],[ParentId],[ItemOrder],[ItemVersionId],[ItemVersionNo],[ItemWorkflowCode],[ItemWorkflowStatus],[ItemWorkflowId] from dbo.newMaterialPrintCorrespond where {0}", whereValue);

            var sql =
                string.Format(
                    "select [id],[MaterialBarcode],[MaterialId],[MaterialNo],[MaterialName],[ModelName],[SupplyLedGroup],[SupplyLedNo],[Count],[BrandName],[Barcodetype],[PartNokey],[LotNokey],[DcNokey],[Qtykey],[LEDcatkey] from dbo.newMaterialPrintCorrespond where {0}",
                    whereValue);

            var dataTable = GetDataTable(sql);
            var returnList = new List<NewMaterialPrintCorrespond>();

            for (var i = 0; i < dataTable.DefaultView.Count; i++)
            {
                var model = new NewMaterialPrintCorrespond();
                var row = dataTable.DefaultView[i];

                if (row != null)
                {
                    if (row["id"] != null && row["id"].ToString() != "")
                    {
                        model.id = int.Parse(row["id"].ToString());
                    }
                    if (row["MaterialBarcode"] != null)
                    {
                        model.MaterialBarcode = row["MaterialBarcode"].ToString();
                    }
                    if (row["MaterialId"] != null)
                    {
                        model.MaterialId = row["MaterialId"].ToString();
                    }
                    if (row["MaterialNo"] != null)
                    {
                        model.MaterialNo = row["MaterialNo"].ToString();
                    }
                    if (row["MaterialName"] != null)
                    {
                        model.MaterialName = row["MaterialName"].ToString();
                    }
                    if (row["ModelName"] != null)
                    {
                        model.ModelName = row["ModelName"].ToString();
                    }
                    if (row["SupplyLedGroup"] != null)
                    {
                        model.SupplyLedGroup = row["SupplyLedGroup"].ToString();
                    }
                    if (row["SupplyLedNo"] != null)
                    {
                        model.SupplyLedNo = row["SupplyLedNo"].ToString();
                    }
                    if (row["Count"] != null && row["Count"].ToString() != "")
                    {
                        model.Count = int.Parse(row["Count"].ToString());
                    }
                    if (row["BrandName"] != null)
                    {
                        model.BrandName = row["BrandName"].ToString();
                    }
                    if (row["Barcodetype"] != null)
                    {
                        model.Barcodetype = row["Barcodetype"].ToString();
                    }
                    //if (row["BarcodeCount"] != null && row["BarcodeCount"].ToString() != "")
                    //{
                    //    model.BarcodeCount = int.Parse(row["BarcodeCount"].ToString());
                    //}
                    if (row["PartNokey"] != null)
                    {
                        model.PartNokey = row["PartNokey"].ToString();
                    }
                    if (row["LotNokey"] != null)
                    {
                        model.LotNokey = row["LotNokey"].ToString();
                    }
                    if (row["DcNokey"] != null)
                    {
                        model.DcNokey = row["DcNokey"].ToString();
                    }
                    if (row["Qtykey"] != null)
                    {
                        model.Qtykey = row["Qtykey"].ToString();
                    }
                    if (row["LEDcatkey"] != null)
                    {
                        model.LedCatkey = row["LEDcatkey"].ToString();
                    }
                    //if (row["Status"] != null)
                    //{
                    //    model.Status = row["Status"].ToString();
                    //}
                    //if (row["Remarks"] != null)
                    //{
                    //    model.Remarks = row["Remarks"].ToString();
                    //}
                    //if (row["Creator"] != null)
                    //{
                    //    model.Creator = row["Creator"].ToString();
                    //}
                    //if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                    //{
                    //    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                    //}
                    //if (row["Editor"] != null)
                    //{
                    //    model.Editor = row["Editor"].ToString();
                    //}
                    //if (row["EditTime"] != null && row["EditTime"].ToString() != "")
                    //{
                    //    model.EditTime = DateTime.Parse(row["EditTime"].ToString());
                    //}
                    //if (row["IsValid"] != null && row["IsValid"].ToString() != "")
                    //{
                    //    model.IsValid = int.Parse(row["IsValid"].ToString());
                    //}
                    //if (row["IsDelete"] != null && row["IsDelete"].ToString() != "")
                    //{
                    //    model.IsDelete = int.Parse(row["IsDelete"].ToString());
                    //}
                    //if (row["ParentId"] != null)
                    //{
                    //    model.ParentId = row["ParentId"].ToString();
                    //}
                    //if (row["ItemOrder"] != null && row["ItemOrder"].ToString() != "")
                    //{
                    //    model.ItemOrder = int.Parse(row["ItemOrder"].ToString());
                    //}
                    //if (row["ItemVersionId"] != null)
                    //{
                    //    model.ItemVersionId = row["ItemVersionId"].ToString();
                    //}
                    //if (row["ItemVersionNo"] != null)
                    //{
                    //    model.ItemVersionNo = row["ItemVersionNo"].ToString();
                    //}
                    //if (row["ItemWorkflowCode"] != null)
                    //{
                    //    model.ItemWorkflowCode = row["ItemWorkflowCode"].ToString();
                    //}
                    //if (row["ItemWorkflowStatus"] != null)
                    //{
                    //    model.ItemWorkflowStatus = row["ItemWorkflowStatus"].ToString();
                    //}
                    //if (row["ItemWorkflowId"] != null)
                    //{
                    //    model.ItemWorkflowId = row["ItemWorkflowId"].ToString();
                    //}
                }

                returnList.Add(model);
            }

            return returnList;
        }

        public static List<NewMaterialPrintCorrespond> GetNewMaterialPrintCorrespondModelsUnionAll(string[] whereValue)
        {
            //var sql =
            //    string.Format(
            //        "select [id],[MaterialBarcode],[MaterialId],[MaterialNo],[MaterialName],[ModelName],[SupplyLedGroup],[SupplyLedNo],[Count],[BrandName],[Barcodetype],[BarcodeCount],[PartNokey],[LotNokey],[DcNokey],[Qtykey],[LEDcatkey],[Status],[Remarks],[Creator],[CreateTime],[Editor],[EditTime],[IsValid],[IsDelete],[ParentId],[ItemOrder],[ItemVersionId],[ItemVersionNo],[ItemWorkflowCode],[ItemWorkflowStatus],[ItemWorkflowId] from dbo.newMaterialPrintCorrespond where {0}", whereValue);

            var sql = string.Empty;
            //string.Format(
            //    "select [id],[MaterialBarcode],[MaterialId],[MaterialNo],[MaterialName],[ModelName],[SupplyLedGroup],[SupplyLedNo],[Count],[BrandName],[Barcodetype],[PartNokey],[LotNokey],[DcNokey],[Qtykey],[LEDcatkey] from dbo.newMaterialPrintCorrespond where {0}", whereValue);

            for (int i = 0; i < whereValue.Length; i++)
            {
                sql += string.Format("select [id],[MaterialBarcode],[MaterialId],[MaterialNo],[MaterialName],[ModelName],[SupplyLedGroup],[SupplyLedNo],[Count],[BrandName],[Barcodetype],[PartNokey],[LotNokey],[DcNokey],[Qtykey],[LEDcatkey] from dbo.newMaterialPrintCorrespond where {0}", whereValue[i]);

                if (i != whereValue.Length - 1)
                {
                    sql += " \r\n UNION ALL \r\n";
                }
            }

            var dataTable = GetDataTable(sql);
            var returnList = new List<NewMaterialPrintCorrespond>();

            for (var i = 0; i < dataTable.DefaultView.Count; i++)
            {
                var model = new NewMaterialPrintCorrespond();
                var row = dataTable.DefaultView[i];

                if (row != null)
                {
                    if (row["id"] != null && row["id"].ToString() != "")
                    {
                        model.id = int.Parse(row["id"].ToString());
                    }
                    if (row["MaterialBarcode"] != null)
                    {
                        model.MaterialBarcode = row["MaterialBarcode"].ToString();
                    }
                    if (row["MaterialId"] != null)
                    {
                        model.MaterialId = row["MaterialId"].ToString();
                    }
                    if (row["MaterialNo"] != null)
                    {
                        model.MaterialNo = row["MaterialNo"].ToString();
                    }
                    if (row["MaterialName"] != null)
                    {
                        model.MaterialName = row["MaterialName"].ToString();
                    }
                    if (row["ModelName"] != null)
                    {
                        model.ModelName = row["ModelName"].ToString();
                    }
                    if (row["SupplyLedGroup"] != null)
                    {
                        model.SupplyLedGroup = row["SupplyLedGroup"].ToString();
                    }
                    if (row["SupplyLedNo"] != null)
                    {
                        model.SupplyLedNo = row["SupplyLedNo"].ToString();
                    }
                    if (row["Count"] != null && row["Count"].ToString() != "")
                    {
                        model.Count = int.Parse(row["Count"].ToString());
                    }
                    if (row["BrandName"] != null)
                    {
                        model.BrandName = row["BrandName"].ToString();
                    }
                    if (row["Barcodetype"] != null)
                    {
                        model.Barcodetype = row["Barcodetype"].ToString();
                    }
                    //if (row["BarcodeCount"] != null && row["BarcodeCount"].ToString() != "")
                    //{
                    //    model.BarcodeCount = int.Parse(row["BarcodeCount"].ToString());
                    //}
                    if (row["PartNokey"] != null)
                    {
                        model.PartNokey = row["PartNokey"].ToString();
                    }
                    if (row["LotNokey"] != null)
                    {
                        model.LotNokey = row["LotNokey"].ToString();
                    }
                    if (row["DcNokey"] != null)
                    {
                        model.DcNokey = row["DcNokey"].ToString();
                    }
                    if (row["Qtykey"] != null)
                    {
                        model.Qtykey = row["Qtykey"].ToString();
                    }
                    if (row["LEDcatkey"] != null)
                    {
                        model.LedCatkey = row["LEDcatkey"].ToString();
                    }
                    //if (row["Status"] != null)
                    //{
                    //    model.Status = row["Status"].ToString();
                    //}
                    //if (row["Remarks"] != null)
                    //{
                    //    model.Remarks = row["Remarks"].ToString();
                    //}
                    //if (row["Creator"] != null)
                    //{
                    //    model.Creator = row["Creator"].ToString();
                    //}
                    //if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                    //{
                    //    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                    //}
                    //if (row["Editor"] != null)
                    //{
                    //    model.Editor = row["Editor"].ToString();
                    //}
                    //if (row["EditTime"] != null && row["EditTime"].ToString() != "")
                    //{
                    //    model.EditTime = DateTime.Parse(row["EditTime"].ToString());
                    //}
                    //if (row["IsValid"] != null && row["IsValid"].ToString() != "")
                    //{
                    //    model.IsValid = int.Parse(row["IsValid"].ToString());
                    //}
                    //if (row["IsDelete"] != null && row["IsDelete"].ToString() != "")
                    //{
                    //    model.IsDelete = int.Parse(row["IsDelete"].ToString());
                    //}
                    //if (row["ParentId"] != null)
                    //{
                    //    model.ParentId = row["ParentId"].ToString();
                    //}
                    //if (row["ItemOrder"] != null && row["ItemOrder"].ToString() != "")
                    //{
                    //    model.ItemOrder = int.Parse(row["ItemOrder"].ToString());
                    //}
                    //if (row["ItemVersionId"] != null)
                    //{
                    //    model.ItemVersionId = row["ItemVersionId"].ToString();
                    //}
                    //if (row["ItemVersionNo"] != null)
                    //{
                    //    model.ItemVersionNo = row["ItemVersionNo"].ToString();
                    //}
                    //if (row["ItemWorkflowCode"] != null)
                    //{
                    //    model.ItemWorkflowCode = row["ItemWorkflowCode"].ToString();
                    //}
                    //if (row["ItemWorkflowStatus"] != null)
                    //{
                    //    model.ItemWorkflowStatus = row["ItemWorkflowStatus"].ToString();
                    //}
                    //if (row["ItemWorkflowId"] != null)
                    //{
                    //    model.ItemWorkflowId = row["ItemWorkflowId"].ToString();
                    //}
                }

                returnList.Add(model);
            }

            return returnList;
        }

        public static List<NewMaterialStockInDetail> GetNewMaterialStockInDetail(string whereValue)
        {
            try
            {
                //var sql =
                //string.Format(
                //    "SELECT [id],[StockInNo],[MaterialId],[MaterialNo],[MaterialName],[MaterialBarcode],[MaterialPrintNo],[ModelName],[SupplyNo],[TrayNo],[Count],[LotNo],[DclotNo],[StockInDate],[SupplyLedGroup],[SupplyLedNo],[Expdate],[Qualevel],[Issporadic],[Earmarks],[EquipNo],[OrderNo],[Other],[Other2],[Other3],[Status],[Remarks],[SendTime],[BackTime],[Creator],[CreateTime],[Editor],[EditTime],[IsValid],[IsDelete],[ParentId],[ItemOrder],[ItemVersionId],[ItemVersionNo],[ItemWorkflowCode],[ItemWorkflowStatus],[ItemWorkflowId] FROM [PLMS].[dbo].[newMaterialStockInDetail] where {0}", whereValue);

                var sql =
                string.Format(
                    "SELECT [id],[StockInNo],[MaterialNo],[MaterialBarcode],[MaterialPrintNo],[ModelName],[SupplyNo],[TrayNo],[Count],[LotNo],[DclotNo],[StockInDate],[SupplyLedGroup],[SupplyLedNo],[Expdate],[Qualevel],[Issporadic],[Earmarks],[EquipNo],[OrderNo],[Other],[Other2],[Other3],[Status] FROM [PLMS].[dbo].[newMaterialStockInDetail] where {0}", whereValue);

                var dataTable = GetDataTable(sql);
                var returnList = new List<NewMaterialStockInDetail>();

                for (var i = 0; i < dataTable.DefaultView.Count; i++)
                {
                    var model = new NewMaterialStockInDetail();
                    var row = dataTable.DefaultView[i];
                    if (row != null)
                    {
                        if (row["id"] != null && row["id"].ToString() != "")
                        {
                            model.id = int.Parse(row["id"].ToString());
                        }
                        if (row["StockInNo"] != null)
                        {
                            model.StockInNo = row["StockInNo"].ToString();
                        }
                        //if (row["MaterialId"] != null)
                        //{
                        //    model.MaterialId = row["MaterialId"].ToString();
                        //}
                        if (row["MaterialNo"] != null)
                        {
                            model.MaterialNo = row["MaterialNo"].ToString();
                        }
                        //if (row["MaterialName"] != null)
                        //{
                        //    model.MaterialName = row["MaterialName"].ToString();
                        //}
                        if (row["MaterialBarcode"] != null)
                        {
                            model.MaterialBarcode = row["MaterialBarcode"].ToString();
                        }
                        if (row["MaterialPrintNo"] != null)
                        {
                            model.MaterialPrintNo = row["MaterialPrintNo"].ToString();
                        }
                        if (row["ModelName"] != null)
                        {
                            model.ModelName = row["ModelName"].ToString();
                        }
                        if (row["SupplyNo"] != null)
                        {
                            model.SupplyNo = row["SupplyNo"].ToString();
                        }
                        if (row["TrayNo"] != null)
                        {
                            model.TrayNo = row["TrayNo"].ToString();
                        }
                        if (row["Count"] != null && row["Count"].ToString() != "")
                        {
                            model.Count = int.Parse(row["Count"].ToString());
                        }
                        if (row["LotNo"] != null)
                        {
                            model.LotNo = row["LotNo"].ToString();
                        }
                        if (row["DclotNo"] != null)
                        {
                            model.DclotNo = row["DclotNo"].ToString();
                        }
                        if (row["StockInDate"] != null)
                        {
                            model.StockInDate = row["StockInDate"].ToString();
                        }
                        if (row["SupplyLedGroup"] != null)
                        {
                            model.SupplyLedGroup = row["SupplyLedGroup"].ToString();
                        }
                        if (row["SupplyLedNo"] != null)
                        {
                            model.SupplyLedNo = row["SupplyLedNo"].ToString();
                        }
                        if (row["Expdate"] != null)
                        {
                            model.Expdate = row["Expdate"].ToString();
                        }
                        if (row["Qualevel"] != null)
                        {
                            model.Qualevel = row["Qualevel"].ToString();
                        }
                        if (row["Issporadic"] != null)
                        {
                            model.Issporadic = row["Issporadic"].ToString();
                        }
                        if (row["Earmarks"] != null)
                        {
                            model.Earmarks = row["Earmarks"].ToString();
                        }
                        if (row["EquipNo"] != null)
                        {
                            model.EquipNo = row["EquipNo"].ToString();
                        }
                        if (row["OrderNo"] != null)
                        {
                            model.OrderNo = row["OrderNo"].ToString();
                        }
                        if (row["Other"] != null)
                        {
                            model.Other = row["Other"].ToString();
                        }
                        if (row["Other2"] != null)
                        {
                            model.Other2 = row["Other2"].ToString();
                        }
                        if (row["Other3"] != null)
                        {
                            model.Other3 = row["Other3"].ToString();
                        }
                        if (row["Status"] != null)
                        {
                            model.Status = row["Status"].ToString();
                        }
                        //if (row["Remarks"] != null)
                        //{
                        //    model.Remarks = row["Remarks"].ToString();
                        //}
                        //if (row["SendTime"] != null && row["SendTime"].ToString() != "")
                        //{
                        //    model.SendTime = DateTime.Parse(row["SendTime"].ToString());
                        //}
                        //if (row["BackTime"] != null && row["BackTime"].ToString() != "")
                        //{
                        //    model.BackTime = DateTime.Parse(row["BackTime"].ToString());
                        //}
                        //if (row["Creator"] != null)
                        //{
                        //    model.Creator = row["Creator"].ToString();
                        //}
                        //if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                        //{
                        //    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                        //}
                        //if (row["Editor"] != null)
                        //{
                        //    model.Editor = row["Editor"].ToString();
                        //}
                        //if (row["EditTime"] != null && row["EditTime"].ToString() != "")
                        //{
                        //    model.EditTime = DateTime.Parse(row["EditTime"].ToString());
                        //}
                        //if (row["IsValid"] != null && row["IsValid"].ToString() != "")
                        //{
                        //    model.IsValid = int.Parse(row["IsValid"].ToString());
                        //}
                        //if (row["IsDelete"] != null && row["IsDelete"].ToString() != "")
                        //{
                        //    model.IsDelete = int.Parse(row["IsDelete"].ToString());
                        //}
                        //if (row["ParentId"] != null)
                        //{
                        //    model.ParentId = row["ParentId"].ToString();
                        //}
                        //if (row["ItemOrder"] != null && row["ItemOrder"].ToString() != "")
                        //{
                        //    model.ItemOrder = int.Parse(row["ItemOrder"].ToString());
                        //}
                        //if (row["ItemVersionId"] != null)
                        //{
                        //    model.ItemVersionId = row["ItemVersionId"].ToString();
                        //}
                        //if (row["ItemVersionNo"] != null)
                        //{
                        //    model.ItemVersionNo = row["ItemVersionNo"].ToString();
                        //}
                        //if (row["ItemWorkflowCode"] != null)
                        //{
                        //    model.ItemWorkflowCode = row["ItemWorkflowCode"].ToString();
                        //}
                        //if (row["ItemWorkflowStatus"] != null)
                        //{
                        //    model.ItemWorkflowStatus = row["ItemWorkflowStatus"].ToString();
                        //}
                        //if (row["ItemWorkflowId"] != null)
                        //{
                        //    model.ItemWorkflowId = row["ItemWorkflowId"].ToString();
                        //}
                    }

                    returnList.Add(model);
                }

                return returnList;
            }
            catch (Exception)
            {
                return new List<NewMaterialStockInDetail>();
            }
        }

        public static bool TryGetSerialNo(
           string materialNo, string supplyNo, int count,
           string stockInNo, string supplyLedGroup, string supplyLedNo,
           string modelName, string dclotNo, string expdate,
           string qualevel, string issporadic, string earmarks,
           string materialBarcode,
           string lotNo, string totalCount, out string date, out string serialNo, out string materialPrintNo)
        {
            date = string.Empty;
            serialNo = string.Empty;
            materialPrintNo = string.Empty;

            using (var cmd = new SqlCommand("UploadStockInDetailAndStockInfo", GetConnection()) { CommandType = CommandType.StoredProcedure })
            {
                try
                {
                    // using (var cmd = new SqlCommand(sql, GetConnection()))

                    cmd.Parameters.AddWithValue("@MaterialNo", materialNo); //给输入参数赋值
                    cmd.Parameters.AddWithValue("@SupplyNo", supplyNo); //给输入参数赋值
                    cmd.Parameters.AddWithValue("@Count", count); //给输入参数赋值

                    cmd.Parameters.AddWithValue("@StockInNo", stockInNo); //给输入参数赋值
                    cmd.Parameters.AddWithValue("@SupplyLedGroup", supplyLedGroup); //给输入参数赋值
                    cmd.Parameters.AddWithValue("@SupplyLedNo", supplyLedNo); //给输入参数赋值

                    cmd.Parameters.AddWithValue("@ModelName", modelName); //给输入参数赋值
                    cmd.Parameters.AddWithValue("@DclotNo", dclotNo); //给输入参数赋值
                    cmd.Parameters.AddWithValue("@Expdate", expdate); //给输入参数赋值

                    cmd.Parameters.AddWithValue("@Qualevel", qualevel); //给输入参数赋值
                    cmd.Parameters.AddWithValue("@Issporadic", issporadic); //给输入参数赋值
                    cmd.Parameters.AddWithValue("@Earmarks", earmarks); //给输入参数赋值
                    cmd.Parameters.AddWithValue("@Creater", HikRobotForm.UserCount); //给输入参数赋值

                    cmd.Parameters.AddWithValue("@MaterialBarcode", materialBarcode); //给输入参数赋值
                    cmd.Parameters.AddWithValue("@LotNo", lotNo); //给输入参数赋值
                    cmd.Parameters.AddWithValue("@TotalCount", totalCount); //给输入参数赋值

                    var parOutputDate = cmd.Parameters.Add("@StockInDate", SqlDbType.NVarChar, 50); //定义输出参数 
                    parOutputDate.Direction = ParameterDirection.Output; //参数类型为Output 

                    var parOutputPrintNo = cmd.Parameters.Add("@MaterialPrintNo", SqlDbType.NVarChar, 500); //定义输出参数 
                    parOutputPrintNo.Direction = ParameterDirection.Output;

                    var returnSerialNo = cmd.Parameters.Add("@return_value", SqlDbType.NVarChar, 50);
                    returnSerialNo.Direction = ParameterDirection.ReturnValue;
                    //conn.Open();
                    //cmd.ExecuteNonQuery();

                    cmd.Prepare();
                    //cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.ExecuteNonQuery();

                    serialNo = returnSerialNo.Value.ToString();
                    date = parOutputDate.Value.ToString();
                    materialPrintNo = parOutputPrintNo.Value.ToString();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            //using (var conn = new SqlConnection(sqlConnectiong))
        }
    }
}
