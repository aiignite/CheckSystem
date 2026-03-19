using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:manufactureExceptionData
	/// </summary>
	public partial class manufactureExceptionData
	{
		public manufactureExceptionData()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "manufactureExceptionData"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from manufactureExceptionData");
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
		public int Add(Model.manufactureExceptionData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into manufactureExceptionData(");
			strSql.Append("exceptionNo,exceptionName,exceptionType,taskNo,productNo,deviceNo,processNo,exceptionTime,operatorStaffNo,auditorStaffNo,exceptionStatus,notes,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@exceptionNo,@exceptionName,@exceptionType,@taskNo,@productNo,@deviceNo,@processNo,@exceptionTime,@operatorStaffNo,@auditorStaffNo,@exceptionStatus,@notes,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@exceptionNo", SqlDbType.NVarChar,50),
					new SqlParameter("@exceptionName", SqlDbType.NVarChar,50),
					new SqlParameter("@exceptionType", SqlDbType.NVarChar,50),
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@deviceNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@exceptionTime", SqlDbType.Int,4),
					new SqlParameter("@operatorStaffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@auditorStaffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@exceptionStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@notes", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.exceptionNo;
			parameters[1].Value = model.exceptionName;
			parameters[2].Value = model.exceptionType;
			parameters[3].Value = model.taskNo;
			parameters[4].Value = model.productNo;
			parameters[5].Value = model.deviceNo;
			parameters[6].Value = model.processNo;
			parameters[7].Value = model.exceptionTime;
			parameters[8].Value = model.operatorStaffNo;
			parameters[9].Value = model.auditorStaffNo;
			parameters[10].Value = model.exceptionStatus;
			parameters[11].Value = model.notes;
			parameters[12].Value = model.creater;
			parameters[13].Value = model.createTime;

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
		public bool Update(Model.manufactureExceptionData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update manufactureExceptionData set ");
			strSql.Append("exceptionNo=@exceptionNo,");
			strSql.Append("exceptionName=@exceptionName,");
			strSql.Append("exceptionType=@exceptionType,");
			strSql.Append("taskNo=@taskNo,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("deviceNo=@deviceNo,");
			strSql.Append("processNo=@processNo,");
			strSql.Append("exceptionTime=@exceptionTime,");
			strSql.Append("operatorStaffNo=@operatorStaffNo,");
			strSql.Append("auditorStaffNo=@auditorStaffNo,");
			strSql.Append("exceptionStatus=@exceptionStatus,");
			strSql.Append("notes=@notes,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@exceptionNo", SqlDbType.NVarChar,50),
					new SqlParameter("@exceptionName", SqlDbType.NVarChar,50),
					new SqlParameter("@exceptionType", SqlDbType.NVarChar,50),
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@deviceNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@exceptionTime", SqlDbType.Int,4),
					new SqlParameter("@operatorStaffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@auditorStaffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@exceptionStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@notes", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.exceptionNo;
			parameters[1].Value = model.exceptionName;
			parameters[2].Value = model.exceptionType;
			parameters[3].Value = model.taskNo;
			parameters[4].Value = model.productNo;
			parameters[5].Value = model.deviceNo;
			parameters[6].Value = model.processNo;
			parameters[7].Value = model.exceptionTime;
			parameters[8].Value = model.operatorStaffNo;
			parameters[9].Value = model.auditorStaffNo;
			parameters[10].Value = model.exceptionStatus;
			parameters[11].Value = model.notes;
			parameters[12].Value = model.creater;
			parameters[13].Value = model.createTime;
			parameters[14].Value = model.id;

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
			strSql.Append("delete from manufactureExceptionData ");
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
			strSql.Append("delete from manufactureExceptionData ");
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
		public Model.manufactureExceptionData GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,exceptionNo,exceptionName,exceptionType,taskNo,productNo,deviceNo,processNo,exceptionTime,operatorStaffNo,auditorStaffNo,exceptionStatus,notes,creater,createTime from manufactureExceptionData ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.manufactureExceptionData model=new Model.manufactureExceptionData();
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
		public Model.manufactureExceptionData DataRowToModel(DataRow row)
		{
			Model.manufactureExceptionData model=new Model.manufactureExceptionData();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["exceptionNo"]!=null)
				{
					model.exceptionNo=row["exceptionNo"].ToString();
				}
				if(row["exceptionName"]!=null)
				{
					model.exceptionName=row["exceptionName"].ToString();
				}
				if(row["exceptionType"]!=null)
				{
					model.exceptionType=row["exceptionType"].ToString();
				}
				if(row["taskNo"]!=null)
				{
					model.taskNo=row["taskNo"].ToString();
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["deviceNo"]!=null)
				{
					model.deviceNo=row["deviceNo"].ToString();
				}
				if(row["processNo"]!=null)
				{
					model.processNo=row["processNo"].ToString();
				}
				if(row["exceptionTime"]!=null && row["exceptionTime"].ToString()!="")
				{
					model.exceptionTime=int.Parse(row["exceptionTime"].ToString());
				}
				if(row["operatorStaffNo"]!=null)
				{
					model.operatorStaffNo=row["operatorStaffNo"].ToString();
				}
				if(row["auditorStaffNo"]!=null)
				{
					model.auditorStaffNo=row["auditorStaffNo"].ToString();
				}
				if(row["exceptionStatus"]!=null)
				{
					model.exceptionStatus=row["exceptionStatus"].ToString();
				}
				if(row["notes"]!=null)
				{
					model.notes=row["notes"].ToString();
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
			strSql.Append("select id,exceptionNo,exceptionName,exceptionType,taskNo,productNo,deviceNo,processNo,exceptionTime,operatorStaffNo,auditorStaffNo,exceptionStatus,notes,creater,createTime ");
			strSql.Append(" FROM manufactureExceptionData ");
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
			strSql.Append(" id,exceptionNo,exceptionName,exceptionType,taskNo,productNo,deviceNo,processNo,exceptionTime,operatorStaffNo,auditorStaffNo,exceptionStatus,notes,creater,createTime ");
			strSql.Append(" FROM manufactureExceptionData ");
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
			strSql.Append("select count(1) FROM manufactureExceptionData ");
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
			strSql.Append(")AS Row, T.*  from manufactureExceptionData T ");
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
			parameters[0].Value = "manufactureExceptionData";
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

