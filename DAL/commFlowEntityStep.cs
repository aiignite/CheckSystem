using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:commFlowEntityStep
	/// </summary>
	public partial class commFlowEntityStep
	{
		public commFlowEntityStep()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "commFlowEntityStep"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from commFlowEntityStep");
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
		public int Add(Model.commFlowEntityStep model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into commFlowEntityStep(");
			strSql.Append("noteId,flowNo,stepNo,stepMan,dealStatus,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@noteId,@flowNo,@stepNo,@stepMan,@dealStatus,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@noteId", SqlDbType.Int,4),
					new SqlParameter("@flowNo", SqlDbType.NVarChar,50),
					new SqlParameter("@stepNo", SqlDbType.Int,4),
					new SqlParameter("@stepMan", SqlDbType.NVarChar,50),
					new SqlParameter("@dealStatus", SqlDbType.Int,4),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.noteId;
			parameters[1].Value = model.flowNo;
			parameters[2].Value = model.stepNo;
			parameters[3].Value = model.stepMan;
			parameters[4].Value = model.dealStatus;
			parameters[5].Value = model.creater;
			parameters[6].Value = model.createTime;

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
		public bool Update(Model.commFlowEntityStep model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update commFlowEntityStep set ");
			strSql.Append("noteId=@noteId,");
			strSql.Append("flowNo=@flowNo,");
			strSql.Append("stepNo=@stepNo,");
			strSql.Append("stepMan=@stepMan,");
			strSql.Append("dealStatus=@dealStatus,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@noteId", SqlDbType.Int,4),
					new SqlParameter("@flowNo", SqlDbType.NVarChar,50),
					new SqlParameter("@stepNo", SqlDbType.Int,4),
					new SqlParameter("@stepMan", SqlDbType.NVarChar,50),
					new SqlParameter("@dealStatus", SqlDbType.Int,4),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.noteId;
			parameters[1].Value = model.flowNo;
			parameters[2].Value = model.stepNo;
			parameters[3].Value = model.stepMan;
			parameters[4].Value = model.dealStatus;
			parameters[5].Value = model.creater;
			parameters[6].Value = model.createTime;
			parameters[7].Value = model.id;

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
			strSql.Append("delete from commFlowEntityStep ");
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
			strSql.Append("delete from commFlowEntityStep ");
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
		public Model.commFlowEntityStep GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,noteId,flowNo,stepNo,stepMan,dealStatus,creater,createTime from commFlowEntityStep ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.commFlowEntityStep model=new Model.commFlowEntityStep();
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
		public Model.commFlowEntityStep DataRowToModel(DataRow row)
		{
			Model.commFlowEntityStep model=new Model.commFlowEntityStep();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["noteId"]!=null && row["noteId"].ToString()!="")
				{
					model.noteId=int.Parse(row["noteId"].ToString());
				}
				if(row["flowNo"]!=null)
				{
					model.flowNo=row["flowNo"].ToString();
				}
				if(row["stepNo"]!=null && row["stepNo"].ToString()!="")
				{
					model.stepNo=int.Parse(row["stepNo"].ToString());
				}
				if(row["stepMan"]!=null)
				{
					model.stepMan=row["stepMan"].ToString();
				}
				if(row["dealStatus"]!=null && row["dealStatus"].ToString()!="")
				{
					model.dealStatus=int.Parse(row["dealStatus"].ToString());
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
			strSql.Append("select id,noteId,flowNo,stepNo,stepMan,dealStatus,creater,createTime ");
			strSql.Append(" FROM commFlowEntityStep ");
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
			strSql.Append(" id,noteId,flowNo,stepNo,stepMan,dealStatus,creater,createTime ");
			strSql.Append(" FROM commFlowEntityStep ");
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
			strSql.Append("select count(1) FROM commFlowEntityStep ");
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
			strSql.Append(")AS Row, T.*  from commFlowEntityStep T ");
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
			parameters[0].Value = "commFlowEntityStep";
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

