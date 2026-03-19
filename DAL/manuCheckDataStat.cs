using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:manuCheckDataStat
	/// </summary>
	public partial class manuCheckDataStat
	{
		public manuCheckDataStat()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "manuCheckDataStat"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from manuCheckDataStat");
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
		public int Add(Model.manuCheckDataStat model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into manuCheckDataStat(");
			strSql.Append("productNo,processNo,checkResult,checkYear,checkMonth,checkDay,checkHour,productNumber,updateTime)");
			strSql.Append(" values (");
			strSql.Append("@productNo,@processNo,@checkResult,@checkYear,@checkMonth,@checkDay,@checkHour,@productNumber,@updateTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@checkResult", SqlDbType.NVarChar,50),
					new SqlParameter("@checkYear", SqlDbType.NVarChar,50),
					new SqlParameter("@checkMonth", SqlDbType.NVarChar,50),
					new SqlParameter("@checkDay", SqlDbType.NVarChar,50),
					new SqlParameter("@checkHour", SqlDbType.NVarChar,50),
					new SqlParameter("@productNumber", SqlDbType.Int,4),
					new SqlParameter("@updateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.productNo;
			parameters[1].Value = model.processNo;
			parameters[2].Value = model.checkResult;
			parameters[3].Value = model.checkYear;
			parameters[4].Value = model.checkMonth;
			parameters[5].Value = model.checkDay;
			parameters[6].Value = model.checkHour;
			parameters[7].Value = model.productNumber;
			parameters[8].Value = model.updateTime;

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
		public bool Update(Model.manuCheckDataStat model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update manuCheckDataStat set ");
			strSql.Append("productNo=@productNo,");
			strSql.Append("processNo=@processNo,");
			strSql.Append("checkResult=@checkResult,");
			strSql.Append("checkYear=@checkYear,");
			strSql.Append("checkMonth=@checkMonth,");
			strSql.Append("checkDay=@checkDay,");
			strSql.Append("checkHour=@checkHour,");
			strSql.Append("productNumber=@productNumber,");
			strSql.Append("updateTime=@updateTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@checkResult", SqlDbType.NVarChar,50),
					new SqlParameter("@checkYear", SqlDbType.NVarChar,50),
					new SqlParameter("@checkMonth", SqlDbType.NVarChar,50),
					new SqlParameter("@checkDay", SqlDbType.NVarChar,50),
					new SqlParameter("@checkHour", SqlDbType.NVarChar,50),
					new SqlParameter("@productNumber", SqlDbType.Int,4),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.productNo;
			parameters[1].Value = model.processNo;
			parameters[2].Value = model.checkResult;
			parameters[3].Value = model.checkYear;
			parameters[4].Value = model.checkMonth;
			parameters[5].Value = model.checkDay;
			parameters[6].Value = model.checkHour;
			parameters[7].Value = model.productNumber;
			parameters[8].Value = model.updateTime;
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
			strSql.Append("delete from manuCheckDataStat ");
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
			strSql.Append("delete from manuCheckDataStat ");
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
		public Model.manuCheckDataStat GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,productNo,processNo,checkResult,checkYear,checkMonth,checkDay,checkHour,productNumber,updateTime from manuCheckDataStat ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.manuCheckDataStat model=new Model.manuCheckDataStat();
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
		public Model.manuCheckDataStat DataRowToModel(DataRow row)
		{
			Model.manuCheckDataStat model=new Model.manuCheckDataStat();
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
				if(row["processNo"]!=null)
				{
					model.processNo=row["processNo"].ToString();
				}
				if(row["checkResult"]!=null)
				{
					model.checkResult=row["checkResult"].ToString();
				}
				if(row["checkYear"]!=null)
				{
					model.checkYear=row["checkYear"].ToString();
				}
				if(row["checkMonth"]!=null)
				{
					model.checkMonth=row["checkMonth"].ToString();
				}
				if(row["checkDay"]!=null)
				{
					model.checkDay=row["checkDay"].ToString();
				}
				if(row["checkHour"]!=null)
				{
					model.checkHour=row["checkHour"].ToString();
				}
				if(row["productNumber"]!=null && row["productNumber"].ToString()!="")
				{
					model.productNumber=int.Parse(row["productNumber"].ToString());
				}
				if(row["updateTime"]!=null && row["updateTime"].ToString()!="")
				{
					model.updateTime=DateTime.Parse(row["updateTime"].ToString());
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
			strSql.Append("select id,productNo,processNo,checkResult,checkYear,checkMonth,checkDay,checkHour,productNumber,updateTime ");
			strSql.Append(" FROM manuCheckDataStat ");
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
			strSql.Append(" id,productNo,processNo,checkResult,checkYear,checkMonth,checkDay,checkHour,productNumber,updateTime ");
			strSql.Append(" FROM manuCheckDataStat ");
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
			strSql.Append("select count(1) FROM manuCheckDataStat ");
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
			strSql.Append(")AS Row, T.*  from manuCheckDataStat T ");
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
			parameters[0].Value = "manuCheckDataStat";
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

