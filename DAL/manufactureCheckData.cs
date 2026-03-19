using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
    /// <summary>
    /// 数据访问类:manufactureCheckData
    /// </summary>
    public partial class manufactureCheckData
    {
        public manufactureCheckData()
        { }

        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("id", "manufactureCheckData");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string productNo, string processNo, DateTime checkDate, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from manufactureCheckData");
            strSql.Append(" where productNo=@productNo and processNo=@processNo and checkDate=@checkDate and id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@checkDate", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)			};
            parameters[0].Value = productNo;
            parameters[1].Value = processNo;
            parameters[2].Value = checkDate;
            parameters[3].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.manufactureCheckData model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into manufactureCheckData(");
            strSql.Append("taskNo,productNo,productUid,pcbaNo,pcbaBarcode,productBarcode,packageBarcode,processNo,checkData,checkDate,checkStaffNo,checkResult,creater,createTime)");
            strSql.Append(" values (");
            strSql.Append("@taskNo,@productNo,@productUid,@pcbaNo,@pcbaBarcode,@productBarcode,@packageBarcode,@processNo,@checkData,@checkDate,@checkStaffNo,@checkResult,@creater,@createTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters =
            {
                new SqlParameter("@taskNo", SqlDbType.NVarChar, 50),
                new SqlParameter("@productNo", SqlDbType.NVarChar, 50),
                new SqlParameter("@productUid", SqlDbType.NVarChar),
                new SqlParameter("@pcbaNo", SqlDbType.NVarChar, 100),
                new SqlParameter("@pcbaBarcode", SqlDbType.NVarChar),
                new SqlParameter("@productBarcode", SqlDbType.NVarChar),
                new SqlParameter("@packageBarcode", SqlDbType.NVarChar),
                new SqlParameter("@processNo", SqlDbType.NVarChar, 50),
                new SqlParameter("@checkData", SqlDbType.NVarChar),
                new SqlParameter("@checkDate", SqlDbType.DateTime),
                new SqlParameter("@checkStaffNo", SqlDbType.NVarChar, 50),
                new SqlParameter("@checkResult", SqlDbType.NVarChar, 50),
                new SqlParameter("@creater", SqlDbType.NVarChar, 50),
                new SqlParameter("@createTime", SqlDbType.DateTime)
            };
            parameters[0].Value = model.taskNo;
            parameters[1].Value = model.productNo;
            parameters[2].Value = model.productUid;
            parameters[3].Value = model.pcbaNo;
            parameters[4].Value = model.pcbaBarcode;
            parameters[5].Value = model.productBarcode;
            parameters[6].Value = model.packageBarcode;
            parameters[7].Value = model.processNo;
            parameters[8].Value = model.checkData;
            parameters[9].Value = model.checkDate;
            parameters[10].Value = model.checkStaffNo;
            parameters[11].Value = model.checkResult;
            parameters[12].Value = model.creater;
            parameters[13].Value = model.createTime;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.manufactureCheckData model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update manufactureCheckData set ");
            strSql.Append("taskNo=@taskNo,");
            strSql.Append("productUid=@productUid,");
            strSql.Append("pcbaNo=@pcbaNo,");
            strSql.Append("pcbaBarcode=@pcbaBarcode,");
            strSql.Append("productBarcode=@productBarcode,");
            strSql.Append("packageBarcode=@packageBarcode,");
            strSql.Append("checkData=@checkData,");
            strSql.Append("checkStaffNo=@checkStaffNo,");
            strSql.Append("checkResult=@checkResult,");
            strSql.Append("creater=@creater,");
            strSql.Append("createTime=@createTime");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productUid", SqlDbType.NVarChar,50),
					new SqlParameter("@pcbaNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pcbaBarcode", SqlDbType.NVarChar,50),
					new SqlParameter("@productBarcode", SqlDbType.NVarChar,50),
					new SqlParameter("@packageBarcode", SqlDbType.NVarChar,50),
					new SqlParameter("@checkData", SqlDbType.NVarChar,500),
					new SqlParameter("@checkStaffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@checkResult", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@checkDate", SqlDbType.DateTime)};
            parameters[0].Value = model.taskNo;
            parameters[1].Value = model.productUid;
            parameters[2].Value = model.pcbaNo;
            parameters[3].Value = model.pcbaBarcode;
            parameters[4].Value = model.productBarcode;
            parameters[5].Value = model.packageBarcode;
            parameters[6].Value = model.checkData;
            parameters[7].Value = model.checkStaffNo;
            parameters[8].Value = model.checkResult;
            parameters[9].Value = model.creater;
            parameters[10].Value = model.createTime;
            parameters[11].Value = model.id;
            parameters[12].Value = model.productNo;
            parameters[13].Value = model.processNo;
            parameters[14].Value = model.checkDate;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from manufactureCheckData ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string productNo, string processNo, DateTime checkDate, int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from manufactureCheckData ");
            strSql.Append(" where productNo=@productNo and processNo=@processNo and checkDate=@checkDate and id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@checkDate", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)			};
            parameters[0].Value = productNo;
            parameters[1].Value = processNo;
            parameters[2].Value = checkDate;
            parameters[3].Value = id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from manufactureCheckData ");
            strSql.Append(" where id in (" + idlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.manufactureCheckData GetModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,taskNo,productNo,productUid,pcbaNo,pcbaBarcode,productBarcode,packageBarcode,processNo,checkData,checkDate,checkStaffNo,checkResult,creater,createTime from manufactureCheckData ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;

            Model.manufactureCheckData model = new Model.manufactureCheckData();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.manufactureCheckData DataRowToModel(DataRow row)
        {
            Model.manufactureCheckData model = new Model.manufactureCheckData();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["taskNo"] != null)
                {
                    model.taskNo = row["taskNo"].ToString();
                }
                if (row["productNo"] != null)
                {
                    model.productNo = row["productNo"].ToString();
                }
                if (row["productUid"] != null)
                {
                    model.productUid = row["productUid"].ToString();
                }
                if (row["pcbaNo"] != null)
                {
                    model.pcbaNo = row["pcbaNo"].ToString();
                }
                if (row["pcbaBarcode"] != null)
                {
                    model.pcbaBarcode = row["pcbaBarcode"].ToString();
                }
                if (row["productBarcode"] != null)
                {
                    model.productBarcode = row["productBarcode"].ToString();
                }
                if (row["packageBarcode"] != null)
                {
                    model.packageBarcode = row["packageBarcode"].ToString();
                }
                if (row["processNo"] != null)
                {
                    model.processNo = row["processNo"].ToString();
                }
                if (row["checkData"] != null)
                {
                    model.checkData = row["checkData"].ToString();
                }
                if (row["checkDate"] != null && row["checkDate"].ToString() != "")
                {
                    model.checkDate = DateTime.Parse(row["checkDate"].ToString());
                }
                if (row["checkStaffNo"] != null)
                {
                    model.checkStaffNo = row["checkStaffNo"].ToString();
                }
                if (row["checkResult"] != null)
                {
                    model.checkResult = row["checkResult"].ToString();
                }
                if (row["creater"] != null)
                {
                    model.creater = row["creater"].ToString();
                }
                if (row["createTime"] != null && row["createTime"].ToString() != "")
                {
                    model.createTime = DateTime.Parse(row["createTime"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,taskNo,productNo,productUid,pcbaNo,pcbaBarcode,productBarcode,packageBarcode,processNo,checkData,checkDate,checkStaffNo,checkResult,creater,createTime ");
            strSql.Append(" FROM manufactureCheckData ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" id,taskNo,productNo,productUid,pcbaNo,pcbaBarcode,productBarcode,packageBarcode,processNo,checkData,checkDate,checkStaffNo,checkResult,creater,createTime ");
            strSql.Append(" FROM manufactureCheckData ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM manufactureCheckData ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.id desc");
            }
            strSql.Append(")AS Row, T.*  from manufactureCheckData T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.VarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.VarChar, 255),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@IsReCount", SqlDbType.Bit),
                    new SqlParameter("@OrderType", SqlDbType.Bit),
                    new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "manufactureCheckData";
            parameters[1].Value = "id";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod

        #region  ExtensionMethod

        public int GetCountByTaskNo(string where)
        {
            var sqlAll =
                    string.Format(
                        "select taskNo from manufactureCheckData where {0} group by taskNo", where);
            var ds=DbHelperSQL.Query(sqlAll);
            var dt = ds.Tables[0];
            var count = dt.Rows.Count;
            return count;

            //using (var connection = new SqlConnection("server=.;database=IPMS;uid=sa;pwd=123456"))
            //{
            //    var ds = new DataSet();
            //    try
            //    {
            //        connection.Open();
            //        var command = new SqlDataAdapter(sqlAll, connection);
            //        command.Fill(ds, "ds");

            //        var dt = ds.Tables[0];
            //        var rowsCount = dt.Rows.Count;
            //        VisionCommon.TotalCount = rowsCount;
            //    }
            //    catch (SqlException)
            //    {
            //        VisionCommon.TotalCount = 0;
            //    }
            //}
        }

        #endregion  ExtensionMethod
    }
}

