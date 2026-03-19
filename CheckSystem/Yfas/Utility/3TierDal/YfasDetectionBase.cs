using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CheckSystem.Yfas.Utility._3TierDal
{
    /// <summary>
    /// 数据访问类:YfasDetectionBase
    /// </summary>
    public partial class YfasDetectionBase
    {
        public YfasDetectionBase()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from YfasDetectionBase");
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
        public int Add(_3TierModel.YfasDetectionBase model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into YfasDetectionBase(");
            strSql.Append("DetectionName,Row,CarModel,IsDelete)");
            strSql.Append(" values (");
            strSql.Append("@DetectionName,@Row,@CarModel,@IsDelete)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@DetectionName", SqlDbType.NVarChar,500),
					new SqlParameter("@Row", SqlDbType.NVarChar,500),
					new SqlParameter("@CarModel", SqlDbType.NVarChar,500),
					new SqlParameter("@IsDelete", SqlDbType.Int,4)};
            parameters[0].Value = model.DetectionName;
            parameters[1].Value = model.Row;
            parameters[2].Value = model.CarModel;
            parameters[3].Value = model.IsDelete;

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
        public bool Update(_3TierModel.YfasDetectionBase model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update YfasDetectionBase set ");
            strSql.Append("DetectionName=@DetectionName,");
            strSql.Append("Row=@Row,");
            strSql.Append("CarModel=@CarModel,");
            strSql.Append("IsDelete=@IsDelete");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@DetectionName", SqlDbType.NVarChar,500),
					new SqlParameter("@Row", SqlDbType.NVarChar,500),
					new SqlParameter("@CarModel", SqlDbType.NVarChar,500),
					new SqlParameter("@IsDelete", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = model.DetectionName;
            parameters[1].Value = model.Row;
            parameters[2].Value = model.CarModel;
            parameters[3].Value = model.IsDelete;
            parameters[4].Value = model.id;

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
            strSql.Append("delete from YfasDetectionBase ");
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
            strSql.Append("delete from YfasDetectionBase ");
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
        public _3TierModel.YfasDetectionBase GetModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,DetectionName,Row,CarModel,IsDelete from YfasDetectionBase ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;

            _3TierModel.YfasDetectionBase model = new _3TierModel.YfasDetectionBase();
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
        public _3TierModel.YfasDetectionBase DataRowToModel(DataRow row)
        {
            _3TierModel.YfasDetectionBase model = new _3TierModel.YfasDetectionBase();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["DetectionName"] != null)
                {
                    model.DetectionName = row["DetectionName"].ToString();
                }
                if (row["Row"] != null)
                {
                    model.Row = row["Row"].ToString();
                }
                if (row["CarModel"] != null)
                {
                    model.CarModel = row["CarModel"].ToString();
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,DetectionName,Row,CarModel,IsDelete ");
            strSql.Append(" FROM YfasDetectionBase ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
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
            strSql.Append(" id,DetectionName,Row,CarModel,IsDelete ");
            strSql.Append(" FROM YfasDetectionBase ");
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
            strSql.Append("select count(1) FROM YfasDetectionBase ");
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
            strSql.Append(")AS Row, T.*  from YfasDetectionBase T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return YfasDbHelperSql.Query(strSql.ToString());
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
            parameters[0].Value = "YfasDetectionBase";
            parameters[1].Value = "id";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return YfasDbHelperSql.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

