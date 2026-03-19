using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:agvTask
	/// </summary>
	public partial class agvTask
	{
		public agvTask()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "agvTask"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from agvTask");
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
		public int Add(Model.agvTask model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into agvTask(");
			strSql.Append("taskNo,taskSourceNo,taskStartNo,taskEndNo,taskName,taskPriority,taskRemainTime,agvNo,status,createTime)");
			strSql.Append(" values (");
			strSql.Append("@taskNo,@taskSourceNo,@taskStartNo,@taskEndNo,@taskName,@taskPriority,@taskRemainTime,@agvNo,@status,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@taskSourceNo", SqlDbType.NVarChar,50),
					new SqlParameter("@taskStartNo", SqlDbType.NVarChar,50),
					new SqlParameter("@taskEndNo", SqlDbType.NVarChar,50),
					new SqlParameter("@taskName", SqlDbType.NVarChar,50),
					new SqlParameter("@taskPriority", SqlDbType.NVarChar,50),
					new SqlParameter("@taskRemainTime", SqlDbType.Int,4),
					new SqlParameter("@agvNo", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.taskNo;
			parameters[1].Value = model.taskSourceNo;
			parameters[2].Value = model.taskStartNo;
			parameters[3].Value = model.taskEndNo;
			parameters[4].Value = model.taskName;
			parameters[5].Value = model.taskPriority;
			parameters[6].Value = model.taskRemainTime;
			parameters[7].Value = model.agvNo;
			parameters[8].Value = model.status;
			parameters[9].Value = model.createTime;

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
		public bool Update(Model.agvTask model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update agvTask set ");
			strSql.Append("taskNo=@taskNo,");
			strSql.Append("taskSourceNo=@taskSourceNo,");
			strSql.Append("taskStartNo=@taskStartNo,");
			strSql.Append("taskEndNo=@taskEndNo,");
			strSql.Append("taskName=@taskName,");
			strSql.Append("taskPriority=@taskPriority,");
			strSql.Append("taskRemainTime=@taskRemainTime,");
			strSql.Append("agvNo=@agvNo,");
			strSql.Append("status=@status,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@taskSourceNo", SqlDbType.NVarChar,50),
					new SqlParameter("@taskStartNo", SqlDbType.NVarChar,50),
					new SqlParameter("@taskEndNo", SqlDbType.NVarChar,50),
					new SqlParameter("@taskName", SqlDbType.NVarChar,50),
					new SqlParameter("@taskPriority", SqlDbType.NVarChar,50),
					new SqlParameter("@taskRemainTime", SqlDbType.Int,4),
					new SqlParameter("@agvNo", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.taskNo;
			parameters[1].Value = model.taskSourceNo;
			parameters[2].Value = model.taskStartNo;
			parameters[3].Value = model.taskEndNo;
			parameters[4].Value = model.taskName;
			parameters[5].Value = model.taskPriority;
			parameters[6].Value = model.taskRemainTime;
			parameters[7].Value = model.agvNo;
			parameters[8].Value = model.status;
			parameters[9].Value = model.createTime;
			parameters[10].Value = model.id;

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
			strSql.Append("delete from agvTask ");
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
			strSql.Append("delete from agvTask ");
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
		public Model.agvTask GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,taskNo,taskSourceNo,taskStartNo,taskEndNo,taskName,taskPriority,taskRemainTime,agvNo,status,createTime from agvTask ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.agvTask model=new Model.agvTask();
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
		public Model.agvTask DataRowToModel(DataRow row)
		{
			Model.agvTask model=new Model.agvTask();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["taskNo"]!=null)
				{
					model.taskNo=row["taskNo"].ToString();
				}
				if(row["taskSourceNo"]!=null)
				{
					model.taskSourceNo=row["taskSourceNo"].ToString();
				}
				if(row["taskStartNo"]!=null)
				{
					model.taskStartNo=row["taskStartNo"].ToString();
				}
				if(row["taskEndNo"]!=null)
				{
					model.taskEndNo=row["taskEndNo"].ToString();
				}
				if(row["taskName"]!=null)
				{
					model.taskName=row["taskName"].ToString();
				}
				if(row["taskPriority"]!=null)
				{
					model.taskPriority=row["taskPriority"].ToString();
				}
				if(row["taskRemainTime"]!=null && row["taskRemainTime"].ToString()!="")
				{
					model.taskRemainTime=int.Parse(row["taskRemainTime"].ToString());
				}
				if(row["agvNo"]!=null)
				{
					model.agvNo=row["agvNo"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
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
			strSql.Append("select id,taskNo,taskSourceNo,taskStartNo,taskEndNo,taskName,taskPriority,taskRemainTime,agvNo,status,createTime ");
			strSql.Append(" FROM agvTask ");
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
			strSql.Append(" id,taskNo,taskSourceNo,taskStartNo,taskEndNo,taskName,taskPriority,taskRemainTime,agvNo,status,createTime ");
			strSql.Append(" FROM agvTask ");
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
			strSql.Append("select count(1) FROM agvTask ");
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
			strSql.Append(")AS Row, T.*  from agvTask T ");
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
			parameters[0].Value = "agvTask";
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

