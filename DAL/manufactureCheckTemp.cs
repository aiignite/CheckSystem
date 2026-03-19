using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:manufactureCheckTemp
	/// </summary>
	public partial class manufactureCheckTemp
	{
		public manufactureCheckTemp()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "manufactureCheckTemp"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from manufactureCheckTemp");
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
		public int Add(Model.manufactureCheckTemp model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into manufactureCheckTemp(");
			strSql.Append("lineName,productNo,productName,processNo,processName,dayCount,qualifyCount,timeCount1,timeCount2,timeCount3,timeCount4,timeCount5,timeCount6,timeCount7,timeCount8,timeCount9,timeCount10,timeCount11,timeCount12)");
			strSql.Append(" values (");
			strSql.Append("@lineName,@productNo,@productName,@processNo,@processName,@dayCount,@qualifyCount,@timeCount1,@timeCount2,@timeCount3,@timeCount4,@timeCount5,@timeCount6,@timeCount7,@timeCount8,@timeCount9,@timeCount10,@timeCount11,@timeCount12)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@lineName", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,100),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processName", SqlDbType.NVarChar,50),
					new SqlParameter("@dayCount", SqlDbType.Int,4),
					new SqlParameter("@qualifyCount", SqlDbType.Int,4),
					new SqlParameter("@timeCount1", SqlDbType.Int,4),
					new SqlParameter("@timeCount2", SqlDbType.Int,4),
					new SqlParameter("@timeCount3", SqlDbType.Int,4),
					new SqlParameter("@timeCount4", SqlDbType.Int,4),
					new SqlParameter("@timeCount5", SqlDbType.Int,4),
					new SqlParameter("@timeCount6", SqlDbType.Int,4),
					new SqlParameter("@timeCount7", SqlDbType.Int,4),
					new SqlParameter("@timeCount8", SqlDbType.Int,4),
					new SqlParameter("@timeCount9", SqlDbType.Int,4),
					new SqlParameter("@timeCount10", SqlDbType.Int,4),
					new SqlParameter("@timeCount11", SqlDbType.Int,4),
					new SqlParameter("@timeCount12", SqlDbType.Int,4)};
			parameters[0].Value = model.lineName;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.productName;
			parameters[3].Value = model.processNo;
			parameters[4].Value = model.processName;
			parameters[5].Value = model.dayCount;
			parameters[6].Value = model.qualifyCount;
			parameters[7].Value = model.timeCount1;
			parameters[8].Value = model.timeCount2;
			parameters[9].Value = model.timeCount3;
			parameters[10].Value = model.timeCount4;
			parameters[11].Value = model.timeCount5;
			parameters[12].Value = model.timeCount6;
			parameters[13].Value = model.timeCount7;
			parameters[14].Value = model.timeCount8;
			parameters[15].Value = model.timeCount9;
			parameters[16].Value = model.timeCount10;
			parameters[17].Value = model.timeCount11;
			parameters[18].Value = model.timeCount12;

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
		public bool Update(Model.manufactureCheckTemp model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update manufactureCheckTemp set ");
			strSql.Append("lineName=@lineName,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("productName=@productName,");
			strSql.Append("processNo=@processNo,");
			strSql.Append("processName=@processName,");
			strSql.Append("dayCount=@dayCount,");
			strSql.Append("qualifyCount=@qualifyCount,");
			strSql.Append("timeCount1=@timeCount1,");
			strSql.Append("timeCount2=@timeCount2,");
			strSql.Append("timeCount3=@timeCount3,");
			strSql.Append("timeCount4=@timeCount4,");
			strSql.Append("timeCount5=@timeCount5,");
			strSql.Append("timeCount6=@timeCount6,");
			strSql.Append("timeCount7=@timeCount7,");
			strSql.Append("timeCount8=@timeCount8,");
			strSql.Append("timeCount9=@timeCount9,");
			strSql.Append("timeCount10=@timeCount10,");
			strSql.Append("timeCount11=@timeCount11,");
			strSql.Append("timeCount12=@timeCount12");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@lineName", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,100),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processName", SqlDbType.NVarChar,50),
					new SqlParameter("@dayCount", SqlDbType.Int,4),
					new SqlParameter("@qualifyCount", SqlDbType.Int,4),
					new SqlParameter("@timeCount1", SqlDbType.Int,4),
					new SqlParameter("@timeCount2", SqlDbType.Int,4),
					new SqlParameter("@timeCount3", SqlDbType.Int,4),
					new SqlParameter("@timeCount4", SqlDbType.Int,4),
					new SqlParameter("@timeCount5", SqlDbType.Int,4),
					new SqlParameter("@timeCount6", SqlDbType.Int,4),
					new SqlParameter("@timeCount7", SqlDbType.Int,4),
					new SqlParameter("@timeCount8", SqlDbType.Int,4),
					new SqlParameter("@timeCount9", SqlDbType.Int,4),
					new SqlParameter("@timeCount10", SqlDbType.Int,4),
					new SqlParameter("@timeCount11", SqlDbType.Int,4),
					new SqlParameter("@timeCount12", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.lineName;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.productName;
			parameters[3].Value = model.processNo;
			parameters[4].Value = model.processName;
			parameters[5].Value = model.dayCount;
			parameters[6].Value = model.qualifyCount;
			parameters[7].Value = model.timeCount1;
			parameters[8].Value = model.timeCount2;
			parameters[9].Value = model.timeCount3;
			parameters[10].Value = model.timeCount4;
			parameters[11].Value = model.timeCount5;
			parameters[12].Value = model.timeCount6;
			parameters[13].Value = model.timeCount7;
			parameters[14].Value = model.timeCount8;
			parameters[15].Value = model.timeCount9;
			parameters[16].Value = model.timeCount10;
			parameters[17].Value = model.timeCount11;
			parameters[18].Value = model.timeCount12;
			parameters[19].Value = model.id;

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
			strSql.Append("delete from manufactureCheckTemp ");
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
			strSql.Append("delete from manufactureCheckTemp ");
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
		public Model.manufactureCheckTemp GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,lineName,productNo,productName,processNo,processName,dayCount,qualifyCount,timeCount1,timeCount2,timeCount3,timeCount4,timeCount5,timeCount6,timeCount7,timeCount8,timeCount9,timeCount10,timeCount11,timeCount12 from manufactureCheckTemp ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.manufactureCheckTemp model=new Model.manufactureCheckTemp();
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
		public Model.manufactureCheckTemp DataRowToModel(DataRow row)
		{
			Model.manufactureCheckTemp model=new Model.manufactureCheckTemp();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["lineName"]!=null)
				{
					model.lineName=row["lineName"].ToString();
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["productName"]!=null)
				{
					model.productName=row["productName"].ToString();
				}
				if(row["processNo"]!=null)
				{
					model.processNo=row["processNo"].ToString();
				}
				if(row["processName"]!=null)
				{
					model.processName=row["processName"].ToString();
				}
				if(row["dayCount"]!=null && row["dayCount"].ToString()!="")
				{
					model.dayCount=int.Parse(row["dayCount"].ToString());
				}
				if(row["qualifyCount"]!=null && row["qualifyCount"].ToString()!="")
				{
					model.qualifyCount=int.Parse(row["qualifyCount"].ToString());
				}
				if(row["timeCount1"]!=null && row["timeCount1"].ToString()!="")
				{
					model.timeCount1=int.Parse(row["timeCount1"].ToString());
				}
				if(row["timeCount2"]!=null && row["timeCount2"].ToString()!="")
				{
					model.timeCount2=int.Parse(row["timeCount2"].ToString());
				}
				if(row["timeCount3"]!=null && row["timeCount3"].ToString()!="")
				{
					model.timeCount3=int.Parse(row["timeCount3"].ToString());
				}
				if(row["timeCount4"]!=null && row["timeCount4"].ToString()!="")
				{
					model.timeCount4=int.Parse(row["timeCount4"].ToString());
				}
				if(row["timeCount5"]!=null && row["timeCount5"].ToString()!="")
				{
					model.timeCount5=int.Parse(row["timeCount5"].ToString());
				}
				if(row["timeCount6"]!=null && row["timeCount6"].ToString()!="")
				{
					model.timeCount6=int.Parse(row["timeCount6"].ToString());
				}
				if(row["timeCount7"]!=null && row["timeCount7"].ToString()!="")
				{
					model.timeCount7=int.Parse(row["timeCount7"].ToString());
				}
				if(row["timeCount8"]!=null && row["timeCount8"].ToString()!="")
				{
					model.timeCount8=int.Parse(row["timeCount8"].ToString());
				}
				if(row["timeCount9"]!=null && row["timeCount9"].ToString()!="")
				{
					model.timeCount9=int.Parse(row["timeCount9"].ToString());
				}
				if(row["timeCount10"]!=null && row["timeCount10"].ToString()!="")
				{
					model.timeCount10=int.Parse(row["timeCount10"].ToString());
				}
				if(row["timeCount11"]!=null && row["timeCount11"].ToString()!="")
				{
					model.timeCount11=int.Parse(row["timeCount11"].ToString());
				}
				if(row["timeCount12"]!=null && row["timeCount12"].ToString()!="")
				{
					model.timeCount12=int.Parse(row["timeCount12"].ToString());
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
			strSql.Append("select id,lineName,productNo,productName,processNo,processName,dayCount,qualifyCount,timeCount1,timeCount2,timeCount3,timeCount4,timeCount5,timeCount6,timeCount7,timeCount8,timeCount9,timeCount10,timeCount11,timeCount12 ");
			strSql.Append(" FROM manufactureCheckTemp ");
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
			strSql.Append(" id,lineName,productNo,productName,processNo,processName,dayCount,qualifyCount,timeCount1,timeCount2,timeCount3,timeCount4,timeCount5,timeCount6,timeCount7,timeCount8,timeCount9,timeCount10,timeCount11,timeCount12 ");
			strSql.Append(" FROM manufactureCheckTemp ");
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
			strSql.Append("select count(1) FROM manufactureCheckTemp ");
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
			strSql.Append(")AS Row, T.*  from manufactureCheckTemp T ");
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
			parameters[0].Value = "manufactureCheckTemp";
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

