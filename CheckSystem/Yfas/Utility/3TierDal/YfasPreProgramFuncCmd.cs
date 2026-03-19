using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CheckSystem.Yfas.Utility._3TierDal
{
    /// <summary>
    /// 数据访问类:YfasPreProgramFuncCmd
    /// </summary>
    public partial class YfasPreProgramFuncCmd
    {
        public YfasPreProgramFuncCmd()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from YfasPreProgramFuncCmd");
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
        public int Add(_3TierModel.YfasPreProgramFuncCmd model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into YfasPreProgramFuncCmd(");
            strSql.Append("FuncId,Type,ControllerId,ToCompareName,ToCompareValue,IsDelete,CmdType)");
            strSql.Append(" values (");
            strSql.Append("@FuncId,@Type,@ControllerId,@ToCompareName,@ToCompareValue,@IsDelete,@CmdType)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@FuncId", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@ControllerId", SqlDbType.Int,4),
					new SqlParameter("@ToCompareName", SqlDbType.NVarChar,200),
					new SqlParameter("@ToCompareValue", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsDelete", SqlDbType.Int,4),
					new SqlParameter("@CmdType", SqlDbType.Int,4)};
            parameters[0].Value = model.FuncId;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.ControllerId;
            parameters[3].Value = model.ToCompareName;
            parameters[4].Value = model.ToCompareValue;
            parameters[5].Value = model.IsDelete;
            parameters[6].Value = model.CmdType;

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
        public bool Update(_3TierModel.YfasPreProgramFuncCmd model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update YfasPreProgramFuncCmd set ");
            strSql.Append("FuncId=@FuncId,");
            strSql.Append("Type=@Type,");
            strSql.Append("ControllerId=@ControllerId,");
            strSql.Append("ToCompareName=@ToCompareName,");
            strSql.Append("ToCompareValue=@ToCompareValue,");
            strSql.Append("IsDelete=@IsDelete,");
            strSql.Append("CmdType=@CmdType");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@FuncId", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@ControllerId", SqlDbType.Int,4),
					new SqlParameter("@ToCompareName", SqlDbType.NVarChar,200),
					new SqlParameter("@ToCompareValue", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsDelete", SqlDbType.Int,4),
					new SqlParameter("@CmdType", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = model.FuncId;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.ControllerId;
            parameters[3].Value = model.ToCompareName;
            parameters[4].Value = model.ToCompareValue;
            parameters[5].Value = model.IsDelete;
            parameters[6].Value = model.CmdType;
            parameters[7].Value = model.id;

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
            strSql.Append("delete from YfasPreProgramFuncCmd ");
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
            strSql.Append("delete from YfasPreProgramFuncCmd ");
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
        public _3TierModel.YfasPreProgramFuncCmd GetModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,FuncId,Type,ControllerId,ToCompareName,ToCompareValue,IsDelete,CmdType from YfasPreProgramFuncCmd ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;

            _3TierModel.YfasPreProgramFuncCmd model = new _3TierModel.YfasPreProgramFuncCmd();
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
        public _3TierModel.YfasPreProgramFuncCmd DataRowToModel(DataRow row)
        {
            _3TierModel.YfasPreProgramFuncCmd model = new _3TierModel.YfasPreProgramFuncCmd();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["FuncId"] != null && row["FuncId"].ToString() != "")
                {
                    model.FuncId = int.Parse(row["FuncId"].ToString());
                }
                if (row["Type"] != null && row["Type"].ToString() != "")
                {
                    model.Type = int.Parse(row["Type"].ToString());
                }
                if (row["ControllerId"] != null && row["ControllerId"].ToString() != "")
                {
                    model.ControllerId = int.Parse(row["ControllerId"].ToString());
                }
                if (row["ToCompareName"] != null)
                {
                    model.ToCompareName = row["ToCompareName"].ToString();
                }
                if (row["ToCompareValue"] != null)
                {
                    model.ToCompareValue = row["ToCompareValue"].ToString();
                }
                if (row["IsDelete"] != null && row["IsDelete"].ToString() != "")
                {
                    model.IsDelete = int.Parse(row["IsDelete"].ToString());
                }
                if (row["CmdType"] != null && row["CmdType"].ToString() != "")
                {
                    model.CmdType = int.Parse(row["CmdType"].ToString());
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
            strSql.Append("select id,FuncId,Type,ControllerId,ToCompareName,ToCompareValue,IsDelete,CmdType ");
            strSql.Append(" FROM YfasPreProgramFuncCmd ");
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
            strSql.Append(" id,FuncId,Type,ControllerId,ToCompareName,ToCompareValue,IsDelete,CmdType ");
            strSql.Append(" FROM YfasPreProgramFuncCmd ");
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
            strSql.Append("select count(1) FROM YfasPreProgramFuncCmd ");
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
            strSql.Append(")AS Row, T.*  from YfasPreProgramFuncCmd T ");
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
            parameters[0].Value = "YfasPreProgramFuncCmd";
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

