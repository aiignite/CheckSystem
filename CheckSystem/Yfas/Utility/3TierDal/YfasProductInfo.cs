using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CheckSystem.Yfas.Utility._3TierDal
{
    /// <summary>
    /// 数据访问类:YfasProductInfo
    /// </summary>
    public partial class YfasProductInfo
    {
        public YfasProductInfo() { }

        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from YfasProductInfo");
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
        public int Add(_3TierModel.YfasProductInfo model)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into YfasProductInfo(");
            strSql.Append("YfasPn,GmPn,Name,RowIndex,Pos,CarMode,Barcode,TreeView,IsDelete)");
            strSql.Append(" values (");
            strSql.Append("@YfasPn,@GmPn,@Name,@RowIndex,@Pos,@CarMode,@Barcode,@TreeView,@IsDelete)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@YfasPn", SqlDbType.NVarChar,500),
					new SqlParameter("@GmPn", SqlDbType.NVarChar,500),
					new SqlParameter("@Name", SqlDbType.NVarChar,500),
					new SqlParameter("@RowIndex", SqlDbType.NVarChar,500),
					new SqlParameter("@Pos", SqlDbType.NVarChar,500),
					new SqlParameter("@CarMode", SqlDbType.NVarChar,500),
					new SqlParameter("@Barcode", SqlDbType.NVarChar,500),
					new SqlParameter("@TreeView", SqlDbType.Text),
					new SqlParameter("@IsDelete", SqlDbType.Int,4)};
            parameters[0].Value = model.YfasPn;
            parameters[1].Value = model.GmPn;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.RowIndex;
            parameters[4].Value = model.Pos;
            parameters[5].Value = model.CarMode;
            parameters[6].Value = model.Barcode;
            parameters[7].Value = model.TreeView;
            parameters[8].Value = model.IsDelete;

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
        public bool Update(_3TierModel.YfasProductInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update YfasProductInfo set ");
            strSql.Append("YfasPn=@YfasPn,");
            strSql.Append("GmPn=@GmPn,");
            strSql.Append("Name=@Name,");
            strSql.Append("RowIndex=@RowIndex,");
            strSql.Append("Pos=@Pos,");
            strSql.Append("CarMode=@CarMode,");
            strSql.Append("Barcode=@Barcode,");
            strSql.Append("TreeView=@TreeView,");
            strSql.Append("IsDelete=@IsDelete");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@YfasPn", SqlDbType.NVarChar,500),
					new SqlParameter("@GmPn", SqlDbType.NVarChar,500),
					new SqlParameter("@Name", SqlDbType.NVarChar,500),
					new SqlParameter("@RowIndex", SqlDbType.NVarChar,500),
					new SqlParameter("@Pos", SqlDbType.NVarChar,500),
					new SqlParameter("@CarMode", SqlDbType.NVarChar,500),
					new SqlParameter("@Barcode", SqlDbType.NVarChar,500),
					new SqlParameter("@TreeView", SqlDbType.Text),
					new SqlParameter("@IsDelete", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = model.YfasPn;
            parameters[1].Value = model.GmPn;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.RowIndex;
            parameters[4].Value = model.Pos;
            parameters[5].Value = model.CarMode;
            parameters[6].Value = model.Barcode;
            parameters[7].Value = model.TreeView;
            parameters[8].Value = model.IsDelete;
            parameters[9].Value = model.id;

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
            strSql.Append("delete from YfasProductInfo ");
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
            strSql.Append("delete from YfasProductInfo ");
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
        public _3TierModel.YfasProductInfo GetModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,YfasPn,GmPn,Name,RowIndex,Pos,CarMode,Barcode,TreeView,IsDelete from YfasProductInfo ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;

            _3TierModel.YfasProductInfo model = new _3TierModel.YfasProductInfo();
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
        public _3TierModel.YfasProductInfo DataRowToModel(DataRow row)
        {
            _3TierModel.YfasProductInfo model = new _3TierModel.YfasProductInfo();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["YfasPn"] != null)
                {
                    model.YfasPn = row["YfasPn"].ToString();
                }
                if (row["GmPn"] != null)
                {
                    model.GmPn = row["GmPn"].ToString();
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["RowIndex"] != null)
                {
                    model.RowIndex = row["RowIndex"].ToString();
                }
                if (row["Pos"] != null)
                {
                    model.Pos = row["Pos"].ToString();
                }
                if (row["CarMode"] != null)
                {
                    model.CarMode = row["CarMode"].ToString();
                }
                if (row["Barcode"] != null)
                {
                    model.Barcode = row["Barcode"].ToString();
                }
                if (row["TreeView"] != null)
                {
                    model.TreeView = row["TreeView"].ToString();
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
            strSql.Append("select id,YfasPn,GmPn,Name,RowIndex,Pos,CarMode,Barcode,TreeView,IsDelete ");
            strSql.Append(" FROM YfasProductInfo ");
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
            strSql.Append(" id,YfasPn,GmPn,Name,RowIndex,Pos,CarMode,Barcode,TreeView,IsDelete ");
            strSql.Append(" FROM YfasProductInfo ");
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
            strSql.Append("select count(1) FROM YfasProductInfo ");
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
            strSql.Append(")AS Row, T.*  from YfasProductInfo T ");
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
            parameters[0].Value = "YfasProductInfo";
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

