using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:LogisticsAndon
	/// </summary>
	public partial class LogisticsAndon
	{
		public LogisticsAndon()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("ID", "LogisticsAndon"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from LogisticsAndon");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(Model.LogisticsAndon model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into LogisticsAndon(");
			strSql.Append("creater,createTime,smsStatus,Product,dealer,dealTime,MessageContent,ProblemType,SendLevel)");
			strSql.Append(" values (");
			strSql.Append("@creater,@createTime,@smsStatus,@Product,@dealer,@dealTime,@MessageContent,@ProblemType,@SendLevel)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@smsStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@Product", SqlDbType.NVarChar,50),
					new SqlParameter("@dealer", SqlDbType.NVarChar,50),
					new SqlParameter("@dealTime", SqlDbType.DateTime),
					new SqlParameter("@MessageContent", SqlDbType.NVarChar,150),
					new SqlParameter("@ProblemType", SqlDbType.NVarChar,50),
					new SqlParameter("@SendLevel", SqlDbType.Int,4)};
			parameters[0].Value = model.creater;
			parameters[1].Value = model.createTime;
			parameters[2].Value = model.smsStatus;
			parameters[3].Value = model.Product;
			parameters[4].Value = model.dealer;
			parameters[5].Value = model.dealTime;
			parameters[6].Value = model.MessageContent;
			parameters[7].Value = model.ProblemType;
			parameters[8].Value = model.SendLevel;

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
		public bool Update(Model.LogisticsAndon model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update LogisticsAndon set ");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("smsStatus=@smsStatus,");
			strSql.Append("Product=@Product,");
			strSql.Append("dealer=@dealer,");
			strSql.Append("dealTime=@dealTime,");
			strSql.Append("MessageContent=@MessageContent,");
			strSql.Append("ProblemType=@ProblemType,");
			strSql.Append("SendLevel=@SendLevel");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@smsStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@Product", SqlDbType.NVarChar,50),
					new SqlParameter("@dealer", SqlDbType.NVarChar,50),
					new SqlParameter("@dealTime", SqlDbType.DateTime),
					new SqlParameter("@MessageContent", SqlDbType.NVarChar,150),
					new SqlParameter("@ProblemType", SqlDbType.NVarChar,50),
					new SqlParameter("@SendLevel", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = model.creater;
			parameters[1].Value = model.createTime;
			parameters[2].Value = model.smsStatus;
			parameters[3].Value = model.Product;
			parameters[4].Value = model.dealer;
			parameters[5].Value = model.dealTime;
			parameters[6].Value = model.MessageContent;
			parameters[7].Value = model.ProblemType;
			parameters[8].Value = model.SendLevel;
			parameters[9].Value = model.ID;

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
		public bool Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from LogisticsAndon ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

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
		public bool DeleteList(string IDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from LogisticsAndon ");
			strSql.Append(" where ID in ("+IDlist + ")  ");
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
		public Model.LogisticsAndon GetModel(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,creater,createTime,smsStatus,Product,dealer,dealTime,MessageContent,ProblemType,SendLevel from LogisticsAndon ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			Model.LogisticsAndon model=new Model.LogisticsAndon();
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
		public Model.LogisticsAndon DataRowToModel(DataRow row)
		{
			Model.LogisticsAndon model=new Model.LogisticsAndon();
			if (row != null)
			{
				if(row["ID"]!=null && row["ID"].ToString()!="")
				{
					model.ID=int.Parse(row["ID"].ToString());
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["smsStatus"]!=null)
				{
					model.smsStatus=row["smsStatus"].ToString();
				}
				if(row["Product"]!=null)
				{
					model.Product=row["Product"].ToString();
				}
				if(row["dealer"]!=null)
				{
					model.dealer=row["dealer"].ToString();
				}
				if(row["dealTime"]!=null && row["dealTime"].ToString()!="")
				{
					model.dealTime=DateTime.Parse(row["dealTime"].ToString());
				}
				if(row["MessageContent"]!=null)
				{
					model.MessageContent=row["MessageContent"].ToString();
				}
				if(row["ProblemType"]!=null)
				{
					model.ProblemType=row["ProblemType"].ToString();
				}
				if(row["SendLevel"]!=null && row["SendLevel"].ToString()!="")
				{
					model.SendLevel=int.Parse(row["SendLevel"].ToString());
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
			strSql.Append("select ID,creater,createTime,smsStatus,Product,dealer,dealTime,MessageContent,ProblemType,SendLevel ");
			strSql.Append(" FROM LogisticsAndon ");
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
			strSql.Append(" ID,creater,createTime,smsStatus,Product,dealer,dealTime,MessageContent,ProblemType,SendLevel ");
			strSql.Append(" FROM LogisticsAndon ");
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
			strSql.Append("select count(1) FROM LogisticsAndon ");
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
				strSql.Append("order by T.ID desc");
			}
			strSql.Append(")AS Row, T.*  from LogisticsAndon T ");
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
			parameters[0].Value = "LogisticsAndon";
			parameters[1].Value = "ID";
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

