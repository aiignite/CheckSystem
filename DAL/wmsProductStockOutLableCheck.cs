using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:wmsProductStockOutLableCheck
	/// </summary>
	public partial class wmsProductStockOutLableCheck
	{
		public wmsProductStockOutLableCheck()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "wmsProductStockOutLableCheck"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from wmsProductStockOutLableCheck");
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
		public int Add(Model.wmsProductStockOutLableCheck model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into wmsProductStockOutLableCheck(");
			strSql.Append("productNo,customerNo,productCount,status,stockOutOperator,createTime,boxNumber,boxNo,shipNo)");
			strSql.Append(" values (");
			strSql.Append("@productNo,@customerNo,@productCount,@status,@stockOutOperator,@createTime,@boxNumber,@boxNo,@shipNo)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@customerNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.Int,4),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@stockOutOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@boxNumber", SqlDbType.NVarChar,50),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,200),
					new SqlParameter("@shipNo", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.productNo;
			parameters[1].Value = model.customerNo;
			parameters[2].Value = model.productCount;
			parameters[3].Value = model.status;
			parameters[4].Value = model.stockOutOperator;
			parameters[5].Value = model.createTime;
			parameters[6].Value = model.boxNumber;
			parameters[7].Value = model.boxNo;
			parameters[8].Value = model.shipNo;

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
		public bool Update(Model.wmsProductStockOutLableCheck model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update wmsProductStockOutLableCheck set ");
			strSql.Append("productNo=@productNo,");
			strSql.Append("customerNo=@customerNo,");
			strSql.Append("productCount=@productCount,");
			strSql.Append("status=@status,");
			strSql.Append("stockOutOperator=@stockOutOperator,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("boxNumber=@boxNumber,");
			strSql.Append("boxNo=@boxNo,");
			strSql.Append("shipNo=@shipNo");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@customerNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.Int,4),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@stockOutOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@boxNumber", SqlDbType.NVarChar,50),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,200),
					new SqlParameter("@shipNo", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.productNo;
			parameters[1].Value = model.customerNo;
			parameters[2].Value = model.productCount;
			parameters[3].Value = model.status;
			parameters[4].Value = model.stockOutOperator;
			parameters[5].Value = model.createTime;
			parameters[6].Value = model.boxNumber;
			parameters[7].Value = model.boxNo;
			parameters[8].Value = model.shipNo;
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
			strSql.Append("delete from wmsProductStockOutLableCheck ");
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
			strSql.Append("delete from wmsProductStockOutLableCheck ");
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
		public Model.wmsProductStockOutLableCheck GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,productNo,customerNo,productCount,status,stockOutOperator,createTime,boxNumber,boxNo,shipNo from wmsProductStockOutLableCheck ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.wmsProductStockOutLableCheck model=new Model.wmsProductStockOutLableCheck();
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
		public Model.wmsProductStockOutLableCheck DataRowToModel(DataRow row)
		{
			Model.wmsProductStockOutLableCheck model=new Model.wmsProductStockOutLableCheck();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["customerNo"]!=null)
				{
					model.customerNo=row["customerNo"].ToString();
				}
				if(row["productCount"]!=null && row["productCount"].ToString()!="")
				{
					model.productCount=int.Parse(row["productCount"].ToString());
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["stockOutOperator"]!=null)
				{
					model.stockOutOperator=row["stockOutOperator"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["boxNumber"]!=null)
				{
					model.boxNumber=row["boxNumber"].ToString();
				}
				if(row["boxNo"]!=null)
				{
					model.boxNo=row["boxNo"].ToString();
				}
				if(row["shipNo"]!=null)
				{
					model.shipNo=row["shipNo"].ToString();
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
			strSql.Append("select id,productNo,customerNo,productCount,status,stockOutOperator,createTime,boxNumber,boxNo,shipNo ");
			strSql.Append(" FROM wmsProductStockOutLableCheck ");
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
			strSql.Append(" id,productNo,customerNo,productCount,status,stockOutOperator,createTime,boxNumber,boxNo,shipNo ");
			strSql.Append(" FROM wmsProductStockOutLableCheck ");
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
			strSql.Append("select count(1) FROM wmsProductStockOutLableCheck ");
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
			strSql.Append(")AS Row, T.*  from wmsProductStockOutLableCheck T ");
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
			parameters[0].Value = "wmsProductStockOutLableCheck";
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

