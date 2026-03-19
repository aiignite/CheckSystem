using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:pdModifyRecord
	/// </summary>
	public partial class pdModifyRecord
	{
		public pdModifyRecord()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "pdModifyRecord"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from pdModifyRecord");
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
		public int Add(Model.pdModifyRecord model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into pdModifyRecord(");
			strSql.Append("operType,buzType,buzID,buzNo,title,field,sourceContent,newContent,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@operType,@buzType,@buzID,@buzNo,@title,@field,@sourceContent,@newContent,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@operType", SqlDbType.NVarChar,50),
					new SqlParameter("@buzType", SqlDbType.NVarChar,50),
					new SqlParameter("@buzID", SqlDbType.Int,4),
					new SqlParameter("@buzNo", SqlDbType.NVarChar,50),
					new SqlParameter("@title", SqlDbType.NVarChar,100),
					new SqlParameter("@field", SqlDbType.NVarChar,50),
					new SqlParameter("@sourceContent", SqlDbType.NVarChar,500),
					new SqlParameter("@newContent", SqlDbType.NVarChar,500),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.operType;
			parameters[1].Value = model.buzType;
			parameters[2].Value = model.buzID;
			parameters[3].Value = model.buzNo;
			parameters[4].Value = model.title;
			parameters[5].Value = model.field;
			parameters[6].Value = model.sourceContent;
			parameters[7].Value = model.newContent;
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
		public bool Update(Model.pdModifyRecord model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update pdModifyRecord set ");
			strSql.Append("operType=@operType,");
			strSql.Append("buzType=@buzType,");
			strSql.Append("buzID=@buzID,");
			strSql.Append("buzNo=@buzNo,");
			strSql.Append("title=@title,");
			strSql.Append("field=@field,");
			strSql.Append("sourceContent=@sourceContent,");
			strSql.Append("newContent=@newContent,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@operType", SqlDbType.NVarChar,50),
					new SqlParameter("@buzType", SqlDbType.NVarChar,50),
					new SqlParameter("@buzID", SqlDbType.Int,4),
					new SqlParameter("@buzNo", SqlDbType.NVarChar,50),
					new SqlParameter("@title", SqlDbType.NVarChar,100),
					new SqlParameter("@field", SqlDbType.NVarChar,50),
					new SqlParameter("@sourceContent", SqlDbType.NVarChar,500),
					new SqlParameter("@newContent", SqlDbType.NVarChar,500),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.operType;
			parameters[1].Value = model.buzType;
			parameters[2].Value = model.buzID;
			parameters[3].Value = model.buzNo;
			parameters[4].Value = model.title;
			parameters[5].Value = model.field;
			parameters[6].Value = model.sourceContent;
			parameters[7].Value = model.newContent;
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
			strSql.Append("delete from pdModifyRecord ");
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
			strSql.Append("delete from pdModifyRecord ");
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
		public Model.pdModifyRecord GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,operType,buzType,buzID,buzNo,title,field,sourceContent,newContent,creater,createTime from pdModifyRecord ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.pdModifyRecord model=new Model.pdModifyRecord();
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
		public Model.pdModifyRecord DataRowToModel(DataRow row)
		{
			Model.pdModifyRecord model=new Model.pdModifyRecord();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["operType"]!=null)
				{
					model.operType=row["operType"].ToString();
				}
				if(row["buzType"]!=null)
				{
					model.buzType=row["buzType"].ToString();
				}
				if(row["buzID"]!=null && row["buzID"].ToString()!="")
				{
					model.buzID=int.Parse(row["buzID"].ToString());
				}
				if(row["buzNo"]!=null)
				{
					model.buzNo=row["buzNo"].ToString();
				}
				if(row["title"]!=null)
				{
					model.title=row["title"].ToString();
				}
				if(row["field"]!=null)
				{
					model.field=row["field"].ToString();
				}
				if(row["sourceContent"]!=null)
				{
					model.sourceContent=row["sourceContent"].ToString();
				}
				if(row["newContent"]!=null)
				{
					model.newContent=row["newContent"].ToString();
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
			strSql.Append("select id,operType,buzType,buzID,buzNo,title,field,sourceContent,newContent,creater,createTime ");
			strSql.Append(" FROM pdModifyRecord ");
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
			strSql.Append(" id,operType,buzType,buzID,buzNo,title,field,sourceContent,newContent,creater,createTime ");
			strSql.Append(" FROM pdModifyRecord ");
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
			strSql.Append("select count(1) FROM pdModifyRecord ");
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
			strSql.Append(")AS Row, T.*  from pdModifyRecord T ");
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
			parameters[0].Value = "pdModifyRecord";
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

