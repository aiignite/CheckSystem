using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:ExamPaper
	/// </summary>
	public partial class ExamPaper
	{
		public ExamPaper()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from ExamPaper");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.NVarChar,50)			};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.ExamPaper model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ExamPaper(");
			strSql.Append("id,PaperName,ProblemCount,Score,Createby,Createdt,examid)");
			strSql.Append(" values (");
			strSql.Append("@id,@PaperName,@ProblemCount,@Score,@Createby,@Createdt,@examid)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.NVarChar,50),
					new SqlParameter("@PaperName", SqlDbType.NVarChar,50),
					new SqlParameter("@ProblemCount", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Decimal,9),
					new SqlParameter("@Createby", SqlDbType.NVarChar,50),
					new SqlParameter("@Createdt", SqlDbType.DateTime),
					new SqlParameter("@examid", SqlDbType.Int,4)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.PaperName;
			parameters[2].Value = model.ProblemCount;
			parameters[3].Value = model.Score;
			parameters[4].Value = model.Createby;
			parameters[5].Value = model.Createdt;
			parameters[6].Value = model.examid;

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
		/// 更新一条数据
		/// </summary>
		public bool Update(Model.ExamPaper model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ExamPaper set ");
			strSql.Append("PaperName=@PaperName,");
			strSql.Append("ProblemCount=@ProblemCount,");
			strSql.Append("Score=@Score,");
			strSql.Append("Createby=@Createby,");
			strSql.Append("Createdt=@Createdt,");
			strSql.Append("examid=@examid");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@PaperName", SqlDbType.NVarChar,50),
					new SqlParameter("@ProblemCount", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Decimal,9),
					new SqlParameter("@Createby", SqlDbType.NVarChar,50),
					new SqlParameter("@Createdt", SqlDbType.DateTime),
					new SqlParameter("@examid", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.PaperName;
			parameters[1].Value = model.ProblemCount;
			parameters[2].Value = model.Score;
			parameters[3].Value = model.Createby;
			parameters[4].Value = model.Createdt;
			parameters[5].Value = model.examid;
			parameters[6].Value = model.id;

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
		public bool Delete(string id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ExamPaper ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.NVarChar,50)			};
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
			strSql.Append("delete from ExamPaper ");
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
		public Model.ExamPaper GetModel(string id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,PaperName,ProblemCount,Score,Createby,Createdt,examid from ExamPaper ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.NVarChar,50)			};
			parameters[0].Value = id;

			Model.ExamPaper model=new Model.ExamPaper();
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
		public Model.ExamPaper DataRowToModel(DataRow row)
		{
			Model.ExamPaper model=new Model.ExamPaper();
			if (row != null)
			{
				if(row["id"]!=null)
				{
					model.id=row["id"].ToString();
				}
				if(row["PaperName"]!=null)
				{
					model.PaperName=row["PaperName"].ToString();
				}
				if(row["ProblemCount"]!=null && row["ProblemCount"].ToString()!="")
				{
					model.ProblemCount=int.Parse(row["ProblemCount"].ToString());
				}
				if(row["Score"]!=null && row["Score"].ToString()!="")
				{
					model.Score=decimal.Parse(row["Score"].ToString());
				}
				if(row["Createby"]!=null)
				{
					model.Createby=row["Createby"].ToString();
				}
				if(row["Createdt"]!=null && row["Createdt"].ToString()!="")
				{
					model.Createdt=DateTime.Parse(row["Createdt"].ToString());
				}
				if(row["examid"]!=null && row["examid"].ToString()!="")
				{
					model.examid=int.Parse(row["examid"].ToString());
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
			strSql.Append("select id,PaperName,ProblemCount,Score,Createby,Createdt,examid ");
			strSql.Append(" FROM ExamPaper ");
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
			strSql.Append(" id,PaperName,ProblemCount,Score,Createby,Createdt,examid ");
			strSql.Append(" FROM ExamPaper ");
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
			strSql.Append("select count(1) FROM ExamPaper ");
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
			strSql.Append(")AS Row, T.*  from ExamPaper T ");
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
			parameters[0].Value = "ExamPaper";
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

