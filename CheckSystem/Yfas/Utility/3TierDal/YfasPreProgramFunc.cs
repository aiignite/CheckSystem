using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CheckSystem.Yfas.Utility._3TierDal
{
    /// <summary>
    /// 数据访问类:YfasPreProgramFunc
    /// </summary>
    public partial class YfasPreProgramFunc
    {
        public YfasPreProgramFunc()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from YfasPreProgramFunc");
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
        public int Add(_3TierModel.YfasPreProgramFunc model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into YfasPreProgramFunc(");
            strSql.Append("Name,IsDelete,TimeOutMs,Row,CarModel,FuncType)");
            strSql.Append(" values (");
            strSql.Append("@Name,@IsDelete,@TimeOutMs,@Row,@CarModel,@FuncType)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@IsDelete", SqlDbType.Int,4),
					new SqlParameter("@TimeOutMs", SqlDbType.Int,4),
					new SqlParameter("@Row", SqlDbType.NVarChar,200),
					new SqlParameter("@CarModel", SqlDbType.NVarChar,200),
					new SqlParameter("@FuncType", SqlDbType.Int,4)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.IsDelete;
            parameters[2].Value = model.TimeOutMs;
            parameters[3].Value = model.Row;
            parameters[4].Value = model.CarModel;
            parameters[5].Value = model.FuncType;

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
        public bool Update(_3TierModel.YfasPreProgramFunc model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update YfasPreProgramFunc set ");
            strSql.Append("Name=@Name,");
            strSql.Append("IsDelete=@IsDelete,");
            strSql.Append("TimeOutMs=@TimeOutMs,");
            strSql.Append("Row=@Row,");
            strSql.Append("CarModel=@CarModel,");
            strSql.Append("FuncType=@FuncType");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@IsDelete", SqlDbType.Int,4),
					new SqlParameter("@TimeOutMs", SqlDbType.Int,4),
					new SqlParameter("@Row", SqlDbType.NVarChar,200),
					new SqlParameter("@CarModel", SqlDbType.NVarChar,200),
					new SqlParameter("@FuncType", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.IsDelete;
            parameters[2].Value = model.TimeOutMs;
            parameters[3].Value = model.Row;
            parameters[4].Value = model.CarModel;
            parameters[5].Value = model.FuncType;
            parameters[6].Value = model.id;

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
            strSql.Append("delete from YfasPreProgramFunc ");
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
            strSql.Append("delete from YfasPreProgramFunc ");
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
        public _3TierModel.YfasPreProgramFunc GetModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,Name,IsDelete,TimeOutMs,Row,CarModel,FuncType from YfasPreProgramFunc ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;

            _3TierModel.YfasPreProgramFunc model = new _3TierModel.YfasPreProgramFunc();
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
        public _3TierModel.YfasPreProgramFunc DataRowToModel(DataRow row)
        {
            _3TierModel.YfasPreProgramFunc model = new _3TierModel.YfasPreProgramFunc();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["IsDelete"] != null && row["IsDelete"].ToString() != "")
                {
                    model.IsDelete = int.Parse(row["IsDelete"].ToString());
                }
                if (row["TimeOutMs"] != null && row["TimeOutMs"].ToString() != "")
                {
                    model.TimeOutMs = int.Parse(row["TimeOutMs"].ToString());
                }
                if (row["Row"] != null)
                {
                    model.Row = row["Row"].ToString();
                }
                if (row["CarModel"] != null)
                {
                    model.CarModel = row["CarModel"].ToString();
                }
                if (row["FuncType"] != null && row["FuncType"].ToString() != "")
                {
                    model.FuncType = int.Parse(row["FuncType"].ToString());
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
            strSql.Append("select id,Name,IsDelete,TimeOutMs,Row,CarModel,FuncType ");
            strSql.Append(" FROM YfasPreProgramFunc ");
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
            strSql.Append(" id,Name,IsDelete,TimeOutMs,Row,CarModel,FuncType ");
            strSql.Append(" FROM YfasPreProgramFunc ");
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
            strSql.Append("select count(1) FROM YfasPreProgramFunc ");
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
            strSql.Append(")AS Row, T.*  from YfasPreProgramFunc T ");
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
            parameters[0].Value = "YfasPreProgramFunc";
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

