using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:examQuestions
	/// </summary>
	public partial class examQuestions
	{
		public examQuestions()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "examQuestions"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from examQuestions");
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
		public int Add(Model.examQuestions model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into examQuestions(");
			strSql.Append("knowledgeno,problem,type,Answer1,Answer2,Answer3,Answer4,Answer5,Answer6,TrueAnswer,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@knowledgeno,@problem,@type,@Answer1,@Answer2,@Answer3,@Answer4,@Answer5,@Answer6,@TrueAnswer,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@knowledgeno", SqlDbType.NVarChar,50),
					new SqlParameter("@problem", SqlDbType.NVarChar,150),
					new SqlParameter("@type", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer1", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer2", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer3", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer4", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer5", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer6", SqlDbType.NVarChar,50),
					new SqlParameter("@TrueAnswer", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.knowledgeno;
			parameters[1].Value = model.problem;
			parameters[2].Value = model.type;
			parameters[3].Value = model.Answer1;
			parameters[4].Value = model.Answer2;
			parameters[5].Value = model.Answer3;
			parameters[6].Value = model.Answer4;
			parameters[7].Value = model.Answer5;
			parameters[8].Value = model.Answer6;
			parameters[9].Value = model.TrueAnswer;
			parameters[10].Value = model.creater;
			parameters[11].Value = model.createTime;

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
		public bool Update(Model.examQuestions model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update examQuestions set ");
			strSql.Append("knowledgeno=@knowledgeno,");
			strSql.Append("problem=@problem,");
			strSql.Append("type=@type,");
			strSql.Append("Answer1=@Answer1,");
			strSql.Append("Answer2=@Answer2,");
			strSql.Append("Answer3=@Answer3,");
			strSql.Append("Answer4=@Answer4,");
			strSql.Append("Answer5=@Answer5,");
			strSql.Append("Answer6=@Answer6,");
			strSql.Append("TrueAnswer=@TrueAnswer,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@knowledgeno", SqlDbType.NVarChar,50),
					new SqlParameter("@problem", SqlDbType.NVarChar,150),
					new SqlParameter("@type", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer1", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer2", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer3", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer4", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer5", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer6", SqlDbType.NVarChar,50),
					new SqlParameter("@TrueAnswer", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.knowledgeno;
			parameters[1].Value = model.problem;
			parameters[2].Value = model.type;
			parameters[3].Value = model.Answer1;
			parameters[4].Value = model.Answer2;
			parameters[5].Value = model.Answer3;
			parameters[6].Value = model.Answer4;
			parameters[7].Value = model.Answer5;
			parameters[8].Value = model.Answer6;
			parameters[9].Value = model.TrueAnswer;
			parameters[10].Value = model.creater;
			parameters[11].Value = model.createTime;
			parameters[12].Value = model.id;

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
			strSql.Append("delete from examQuestions ");
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
			strSql.Append("delete from examQuestions ");
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
		public Model.examQuestions GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,knowledgeno,problem,type,Answer1,Answer2,Answer3,Answer4,Answer5,Answer6,TrueAnswer,creater,createTime from examQuestions ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.examQuestions model=new Model.examQuestions();
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
		public Model.examQuestions DataRowToModel(DataRow row)
		{
			Model.examQuestions model=new Model.examQuestions();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["knowledgeno"]!=null)
				{
					model.knowledgeno=row["knowledgeno"].ToString();
				}
				if(row["problem"]!=null)
				{
					model.problem=row["problem"].ToString();
				}
				if(row["type"]!=null)
				{
					model.type=row["type"].ToString();
				}
				if(row["Answer1"]!=null)
				{
					model.Answer1=row["Answer1"].ToString();
				}
				if(row["Answer2"]!=null)
				{
					model.Answer2=row["Answer2"].ToString();
				}
				if(row["Answer3"]!=null)
				{
					model.Answer3=row["Answer3"].ToString();
				}
				if(row["Answer4"]!=null)
				{
					model.Answer4=row["Answer4"].ToString();
				}
				if(row["Answer5"]!=null)
				{
					model.Answer5=row["Answer5"].ToString();
				}
				if(row["Answer6"]!=null)
				{
					model.Answer6=row["Answer6"].ToString();
				}
				if(row["TrueAnswer"]!=null)
				{
					model.TrueAnswer=row["TrueAnswer"].ToString();
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
			strSql.Append("select id,knowledgeno,problem,type,Answer1,Answer2,Answer3,Answer4,Answer5,Answer6,TrueAnswer,creater,createTime ");
			strSql.Append(" FROM examQuestions ");
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
			strSql.Append(" id,knowledgeno,problem,type,Answer1,Answer2,Answer3,Answer4,Answer5,Answer6,TrueAnswer,creater,createTime ");
			strSql.Append(" FROM examQuestions ");
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
			strSql.Append("select count(1) FROM examQuestions ");
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
			strSql.Append(")AS Row, T.*  from examQuestions T ");
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
			parameters[0].Value = "examQuestions";
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

