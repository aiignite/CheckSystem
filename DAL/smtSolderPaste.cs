using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:smtSolderPaste
	/// </summary>
	public partial class smtSolderPaste
	{
		public smtSolderPaste()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "smtSolderPaste"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from smtSolderPaste");
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
		public int Add(Model.smtSolderPaste model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into smtSolderPaste(");
			strSql.Append("no,useCount,instoreTime,backTime,mixTime,userTime,expiredTime,status,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@no,@useCount,@instoreTime,@backTime,@mixTime,@userTime,@expiredTime,@status,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@no", SqlDbType.NVarChar,50),
					new SqlParameter("@useCount", SqlDbType.Int,4),
					new SqlParameter("@instoreTime", SqlDbType.DateTime),
					new SqlParameter("@backTime", SqlDbType.DateTime),
					new SqlParameter("@mixTime", SqlDbType.DateTime),
					new SqlParameter("@userTime", SqlDbType.DateTime),
					new SqlParameter("@expiredTime", SqlDbType.DateTime),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.no;
			parameters[1].Value = model.useCount;
			parameters[2].Value = model.instoreTime;
			parameters[3].Value = model.backTime;
			parameters[4].Value = model.mixTime;
			parameters[5].Value = model.userTime;
			parameters[6].Value = model.expiredTime;
			parameters[7].Value = model.status;
			parameters[8].Value = model.creater;
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
		public bool Update(Model.smtSolderPaste model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update smtSolderPaste set ");
			strSql.Append("no=@no,");
			strSql.Append("useCount=@useCount,");
			strSql.Append("instoreTime=@instoreTime,");
			strSql.Append("backTime=@backTime,");
			strSql.Append("mixTime=@mixTime,");
			strSql.Append("userTime=@userTime,");
			strSql.Append("expiredTime=@expiredTime,");
			strSql.Append("status=@status,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@no", SqlDbType.NVarChar,50),
					new SqlParameter("@useCount", SqlDbType.Int,4),
					new SqlParameter("@instoreTime", SqlDbType.DateTime),
					new SqlParameter("@backTime", SqlDbType.DateTime),
					new SqlParameter("@mixTime", SqlDbType.DateTime),
					new SqlParameter("@userTime", SqlDbType.DateTime),
					new SqlParameter("@expiredTime", SqlDbType.DateTime),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.no;
			parameters[1].Value = model.useCount;
			parameters[2].Value = model.instoreTime;
			parameters[3].Value = model.backTime;
			parameters[4].Value = model.mixTime;
			parameters[5].Value = model.userTime;
			parameters[6].Value = model.expiredTime;
			parameters[7].Value = model.status;
			parameters[8].Value = model.creater;
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
			strSql.Append("delete from smtSolderPaste ");
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
			strSql.Append("delete from smtSolderPaste ");
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
		public Model.smtSolderPaste GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,no,useCount,instoreTime,backTime,mixTime,userTime,expiredTime,status,creater,createTime from smtSolderPaste ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.smtSolderPaste model=new Model.smtSolderPaste();
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
		public Model.smtSolderPaste DataRowToModel(DataRow row)
		{
			Model.smtSolderPaste model=new Model.smtSolderPaste();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["no"]!=null)
				{
					model.no=row["no"].ToString();
				}
				if(row["useCount"]!=null && row["useCount"].ToString()!="")
				{
					model.useCount=int.Parse(row["useCount"].ToString());
				}
				if(row["instoreTime"]!=null && row["instoreTime"].ToString()!="")
				{
					model.instoreTime=DateTime.Parse(row["instoreTime"].ToString());
				}
				if(row["backTime"]!=null && row["backTime"].ToString()!="")
				{
					model.backTime=DateTime.Parse(row["backTime"].ToString());
				}
				if(row["mixTime"]!=null && row["mixTime"].ToString()!="")
				{
					model.mixTime=DateTime.Parse(row["mixTime"].ToString());
				}
				if(row["userTime"]!=null && row["userTime"].ToString()!="")
				{
					model.userTime=DateTime.Parse(row["userTime"].ToString());
				}
				if(row["expiredTime"]!=null && row["expiredTime"].ToString()!="")
				{
					model.expiredTime=DateTime.Parse(row["expiredTime"].ToString());
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
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
			strSql.Append("select id,no,useCount,instoreTime,backTime,mixTime,userTime,expiredTime,status,creater,createTime ");
			strSql.Append(" FROM smtSolderPaste ");
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
			strSql.Append(" id,no,useCount,instoreTime,backTime,mixTime,userTime,expiredTime,status,creater,createTime ");
			strSql.Append(" FROM smtSolderPaste ");
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
			strSql.Append("select count(1) FROM smtSolderPaste ");
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
			strSql.Append(")AS Row, T.*  from smtSolderPaste T ");
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
			parameters[0].Value = "smtSolderPaste";
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

