using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CheckSystem.Yfas.Utility._3TierDal
{
    /// <summary>
    /// 数据访问类:YfasProductParas
    /// </summary>
    public partial class YfasProductParas
    {
        public YfasProductParas()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from YfasProductParas");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;

            return YfasDbHelperSql.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(_3TierModel.YfasProductParas model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into YfasProductParas(");
            strSql.Append("ProductId,DetectionId,DateType,OkForamt,Value,Min,Max,Uint,BindControllerId,BindControllerFieldName,Offset,Filter,IsDelete)");
            strSql.Append(" values (");
            strSql.Append("@ProductId,@DetectionId,@DateType,@OkForamt,@Value,@Min,@Max,@Uint,@BindControllerId,@BindControllerFieldName,@Offset,@Filter,@IsDelete)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.Int,4),
					new SqlParameter("@DetectionId", SqlDbType.Int,4),
					new SqlParameter("@DateType", SqlDbType.NVarChar,50),
					new SqlParameter("@OkForamt", SqlDbType.NVarChar,50),
					new SqlParameter("@Value", SqlDbType.NVarChar,500),
					new SqlParameter("@Min", SqlDbType.NVarChar,50),
					new SqlParameter("@Max", SqlDbType.NVarChar,50),
					new SqlParameter("@Uint", SqlDbType.NVarChar,50),
					new SqlParameter("@BindControllerId", SqlDbType.Int,4),
					new SqlParameter("@BindControllerFieldName", SqlDbType.NVarChar,50),
					new SqlParameter("@Offset", SqlDbType.NVarChar,50),
					new SqlParameter("@Filter", SqlDbType.Int,4),
					new SqlParameter("@IsDelete", SqlDbType.Int,4)};
            parameters[0].Value = model.ProductId;
            parameters[1].Value = model.DetectionId;
            parameters[2].Value = model.DateType;
            parameters[3].Value = model.OkForamt;
            parameters[4].Value = model.Value;
            parameters[5].Value = model.Min;
            parameters[6].Value = model.Max;
            parameters[7].Value = model.Uint;
            parameters[8].Value = model.BindControllerId;
            parameters[9].Value = model.BindControllerFieldName;
            parameters[10].Value = model.Offset;
            parameters[11].Value = model.Filter;
            parameters[12].Value = model.IsDelete;

            object obj = YfasDbHelperSql.GetSingle(strSql.ToString(), parameters);
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
        public bool Update(_3TierModel.YfasProductParas model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update YfasProductParas set ");
            strSql.Append("ProductId=@ProductId,");
            strSql.Append("DetectionId=@DetectionId,");
            strSql.Append("DateType=@DateType,");
            strSql.Append("OkForamt=@OkForamt,");
            strSql.Append("Value=@Value,");
            strSql.Append("Min=@Min,");
            strSql.Append("Max=@Max,");
            strSql.Append("Uint=@Uint,");
            strSql.Append("BindControllerId=@BindControllerId,");
            strSql.Append("BindControllerFieldName=@BindControllerFieldName,");
            strSql.Append("Offset=@Offset,");
            strSql.Append("Filter=@Filter,");
            strSql.Append("IsDelete=@IsDelete");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.Int,4),
					new SqlParameter("@DetectionId", SqlDbType.Int,4),
					new SqlParameter("@DateType", SqlDbType.NVarChar,50),
					new SqlParameter("@OkForamt", SqlDbType.NVarChar,50),
					new SqlParameter("@Value", SqlDbType.NVarChar,500),
					new SqlParameter("@Min", SqlDbType.NVarChar,50),
					new SqlParameter("@Max", SqlDbType.NVarChar,50),
					new SqlParameter("@Uint", SqlDbType.NVarChar,50),
					new SqlParameter("@BindControllerId", SqlDbType.Int,4),
					new SqlParameter("@BindControllerFieldName", SqlDbType.NVarChar,50),
					new SqlParameter("@Offset", SqlDbType.NVarChar,50),
					new SqlParameter("@Filter", SqlDbType.Int,4),
					new SqlParameter("@IsDelete", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = model.ProductId;
            parameters[1].Value = model.DetectionId;
            parameters[2].Value = model.DateType;
            parameters[3].Value = model.OkForamt;
            parameters[4].Value = model.Value;
            parameters[5].Value = model.Min;
            parameters[6].Value = model.Max;
            parameters[7].Value = model.Uint;
            parameters[8].Value = model.BindControllerId;
            parameters[9].Value = model.BindControllerFieldName;
            parameters[10].Value = model.Offset;
            parameters[11].Value = model.Filter;
            parameters[12].Value = model.IsDelete;
            parameters[13].Value = model.id;

            int rows = YfasDbHelperSql.ExecuteSql(strSql.ToString(), parameters);
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
            strSql.Append("delete from YfasProductParas ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;

            int rows = YfasDbHelperSql.ExecuteSql(strSql.ToString(), parameters);
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
            strSql.Append("delete from YfasProductParas ");
            strSql.Append(" where id in (" + idlist + ")  ");
            int rows = YfasDbHelperSql.ExecuteSql(strSql.ToString());
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
        public _3TierModel.YfasProductParas GetModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,ProductId,DetectionId,DateType,OkForamt,Value,Min,Max,Uint,BindControllerId,BindControllerFieldName,Offset,Filter,IsDelete from YfasProductParas ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;

            _3TierModel.YfasProductParas model = new _3TierModel.YfasProductParas();
            DataSet ds = YfasDbHelperSql.Query(strSql.ToString(), parameters);
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
        public _3TierModel.YfasProductParas DataRowToModel(DataRow row)
        {
            _3TierModel.YfasProductParas model = new _3TierModel.YfasProductParas();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = int.Parse(row["ProductId"].ToString());
                }
                if (row["DetectionId"] != null && row["DetectionId"].ToString() != "")
                {
                    model.DetectionId = int.Parse(row["DetectionId"].ToString());
                }
                if (row["DateType"] != null)
                {
                    model.DateType = row["DateType"].ToString();
                }
                if (row["OkForamt"] != null)
                {
                    model.OkForamt = row["OkForamt"].ToString();
                }
                if (row["Value"] != null)
                {
                    model.Value = row["Value"].ToString();
                }
                if (row["Min"] != null)
                {
                    model.Min = row["Min"].ToString();
                }
                if (row["Max"] != null)
                {
                    model.Max = row["Max"].ToString();
                }
                if (row["Uint"] != null)
                {
                    model.Uint = row["Uint"].ToString();
                }
                if (row["BindControllerId"] != null && row["BindControllerId"].ToString() != "")
                {
                    model.BindControllerId = int.Parse(row["BindControllerId"].ToString());
                }
                if (row["BindControllerFieldName"] != null)
                {
                    model.BindControllerFieldName = row["BindControllerFieldName"].ToString();
                }
                if (row["Offset"] != null)
                {
                    model.Offset = row["Offset"].ToString();
                }
                if (row["Filter"] != null && row["Filter"].ToString() != "")
                {
                    model.Filter = int.Parse(row["Filter"].ToString());
                }
                if (row["IsDelete"] != null && row["IsDelete"].ToString() != "")
                {
                    model.IsDelete = int.Parse(row["IsDelete"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            var strSql = new StringBuilder();
            strSql.Append("select id,ProductId,DetectionId,DateType,OkForamt,Value,Min,Max,Uint,BindControllerId,BindControllerFieldName,Offset,Filter,IsDelete ");
            strSql.Append(" FROM YfasProductParas ");
            if (strWhere.Trim() != "")
                strSql.Append(" where " + strWhere);
            return YfasDbHelperSql.Query(strSql.ToString());
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
            strSql.Append(" id,ProductId,DetectionId,DateType,OkForamt,Value,Min,Max,Uint,BindControllerId,BindControllerFieldName,Offset,Filter,IsDelete ");
            strSql.Append(" FROM YfasProductParas ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return YfasDbHelperSql.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM YfasProductParas ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = YfasDbHelperSql.GetSingle(strSql.ToString());
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
            strSql.Append(")AS Row, T.*  from YfasProductParas T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return YfasDbHelperSql.Query(strSql.ToString());
        }

        #endregion  BasicMethod
    }
}

