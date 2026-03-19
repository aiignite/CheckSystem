using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:wmsProductSummaryDetail
	/// </summary>
	public partial class wmsProductSummaryDetail
	{
		public wmsProductSummaryDetail()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "wmsProductSummaryDetail"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from wmsProductSummaryDetail");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(Model.wmsProductSummaryDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into wmsProductSummaryDetail(");
			strSql.Append("serialNo,productNo,shipNo,stockupCount,stockoutCount,plateNo,refreshTime,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@serialNo,@productNo,@shipNo,@stockupCount,@stockoutCount,@plateNo,@refreshTime,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@serialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shipNo", SqlDbType.NVarChar,50),
					new SqlParameter("@stockupCount", SqlDbType.Int,4),
					new SqlParameter("@stockoutCount", SqlDbType.Int,4),
					new SqlParameter("@plateNo", SqlDbType.NVarChar,50),
					new SqlParameter("@refreshTime", SqlDbType.DateTime),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.serialNo;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.shipNo;
			parameters[3].Value = model.stockupCount;
			parameters[4].Value = model.stockoutCount;
			parameters[5].Value = model.plateNo;
			parameters[6].Value = model.refreshTime;
			parameters[7].Value = model.creater;
			parameters[8].Value = model.createTime;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(Model.wmsProductSummaryDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update wmsProductSummaryDetail set ");
			strSql.Append("serialNo=@serialNo,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("shipNo=@shipNo,");
			strSql.Append("stockupCount=@stockupCount,");
			strSql.Append("stockoutCount=@stockoutCount,");
			strSql.Append("plateNo=@plateNo,");
			strSql.Append("refreshTime=@refreshTime,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@serialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shipNo", SqlDbType.NVarChar,50),
					new SqlParameter("@stockupCount", SqlDbType.Int,4),
					new SqlParameter("@stockoutCount", SqlDbType.Int,4),
					new SqlParameter("@plateNo", SqlDbType.NVarChar,50),
					new SqlParameter("@refreshTime", SqlDbType.DateTime),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.serialNo;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.shipNo;
			parameters[3].Value = model.stockupCount;
			parameters[4].Value = model.stockoutCount;
			parameters[5].Value = model.plateNo;
			parameters[6].Value = model.refreshTime;
			parameters[7].Value = model.creater;
			parameters[8].Value = model.createTime;
			parameters[9].Value = model.id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from wmsProductSummaryDetail ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from wmsProductSummaryDetail ");
			strSql.Append(" where id in ("+idlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
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
		public Model.wmsProductSummaryDetail GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,serialNo,productNo,shipNo,stockupCount,stockoutCount,plateNo,refreshTime,creater,createTime from wmsProductSummaryDetail ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.wmsProductSummaryDetail model=new Model.wmsProductSummaryDetail();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
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
		public Model.wmsProductSummaryDetail DataRowToModel(DataRow row)
		{
			Model.wmsProductSummaryDetail model=new Model.wmsProductSummaryDetail();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["serialNo"]!=null)
				{
					model.serialNo=row["serialNo"].ToString();
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["shipNo"]!=null)
				{
					model.shipNo=row["shipNo"].ToString();
				}
				if(row["stockupCount"]!=null && row["stockupCount"].ToString()!="")
				{
					model.stockupCount=int.Parse(row["stockupCount"].ToString());
				}
				if(row["stockoutCount"]!=null && row["stockoutCount"].ToString()!="")
				{
					model.stockoutCount=int.Parse(row["stockoutCount"].ToString());
				}
				if(row["plateNo"]!=null)
				{
					model.plateNo=row["plateNo"].ToString();
				}
				if(row["refreshTime"]!=null && row["refreshTime"].ToString()!="")
				{
					model.refreshTime=DateTime.Parse(row["refreshTime"].ToString());
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,serialNo,productNo,shipNo,stockupCount,stockoutCount,plateNo,refreshTime,creater,createTime ");
			strSql.Append(" FROM wmsProductSummaryDetail ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" id,serialNo,productNo,shipNo,stockupCount,stockoutCount,plateNo,refreshTime,creater,createTime ");
			strSql.Append(" FROM wmsProductSummaryDetail ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM wmsProductSummaryDetail ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.id desc");
			}
			strSql.Append(")AS Row, T.*  from wmsProductSummaryDetail T ");
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
			parameters[0].Value = "wmsProductSummaryDetail";
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

		#endregion  ExtensionMethod
	}
}

