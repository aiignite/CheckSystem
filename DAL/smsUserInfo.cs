using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:smsUserInfo
	/// </summary>
	public partial class smsUserInfo
	{
		public smsUserInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "smsUserInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from smsUserInfo");
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
		public int Add(Model.smsUserInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into smsUserInfo(");
			strSql.Append("userNo,userName,mobile,userRank,leaderUserNo,creater,createTime,smsType,defaultSend,smsTypeNew)");
			strSql.Append(" values (");
			strSql.Append("@userNo,@userName,@mobile,@userRank,@leaderUserNo,@creater,@createTime,@smsType,@defaultSend,@smsTypeNew)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@userNo", SqlDbType.NVarChar,50),
					new SqlParameter("@userName", SqlDbType.NVarChar,50),
					new SqlParameter("@mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@userRank", SqlDbType.NVarChar,50),
					new SqlParameter("@leaderUserNo", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@smsType", SqlDbType.NVarChar,50),
					new SqlParameter("@defaultSend", SqlDbType.NVarChar,50),
					new SqlParameter("@smsTypeNew", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.userNo;
			parameters[1].Value = model.userName;
			parameters[2].Value = model.mobile;
			parameters[3].Value = model.userRank;
			parameters[4].Value = model.leaderUserNo;
			parameters[5].Value = model.creater;
			parameters[6].Value = model.createTime;
			parameters[7].Value = model.smsType;
			parameters[8].Value = model.defaultSend;
			parameters[9].Value = model.smsTypeNew;

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
		public bool Update(Model.smsUserInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update smsUserInfo set ");
			strSql.Append("userNo=@userNo,");
			strSql.Append("userName=@userName,");
			strSql.Append("mobile=@mobile,");
			strSql.Append("userRank=@userRank,");
			strSql.Append("leaderUserNo=@leaderUserNo,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("smsType=@smsType,");
			strSql.Append("defaultSend=@defaultSend,");
			strSql.Append("smsTypeNew=@smsTypeNew");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@userNo", SqlDbType.NVarChar,50),
					new SqlParameter("@userName", SqlDbType.NVarChar,50),
					new SqlParameter("@mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@userRank", SqlDbType.NVarChar,50),
					new SqlParameter("@leaderUserNo", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@smsType", SqlDbType.NVarChar,50),
					new SqlParameter("@defaultSend", SqlDbType.NVarChar,50),
					new SqlParameter("@smsTypeNew", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.userNo;
			parameters[1].Value = model.userName;
			parameters[2].Value = model.mobile;
			parameters[3].Value = model.userRank;
			parameters[4].Value = model.leaderUserNo;
			parameters[5].Value = model.creater;
			parameters[6].Value = model.createTime;
			parameters[7].Value = model.smsType;
			parameters[8].Value = model.defaultSend;
			parameters[9].Value = model.smsTypeNew;
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
			strSql.Append("delete from smsUserInfo ");
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
			strSql.Append("delete from smsUserInfo ");
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
		public Model.smsUserInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,userNo,userName,mobile,userRank,leaderUserNo,creater,createTime,smsType,defaultSend,smsTypeNew from smsUserInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.smsUserInfo model=new Model.smsUserInfo();
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
		public Model.smsUserInfo DataRowToModel(DataRow row)
		{
			Model.smsUserInfo model=new Model.smsUserInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["userNo"]!=null)
				{
					model.userNo=row["userNo"].ToString();
				}
				if(row["userName"]!=null)
				{
					model.userName=row["userName"].ToString();
				}
				if(row["mobile"]!=null)
				{
					model.mobile=row["mobile"].ToString();
				}
				if(row["userRank"]!=null)
				{
					model.userRank=row["userRank"].ToString();
				}
				if(row["leaderUserNo"]!=null)
				{
					model.leaderUserNo=row["leaderUserNo"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["smsType"]!=null)
				{
					model.smsType=row["smsType"].ToString();
				}
				if(row["defaultSend"]!=null)
				{
					model.defaultSend=row["defaultSend"].ToString();
				}
				if(row["smsTypeNew"]!=null)
				{
					model.smsTypeNew=row["smsTypeNew"].ToString();
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
			strSql.Append("select id,userNo,userName,mobile,userRank,leaderUserNo,creater,createTime,smsType,defaultSend,smsTypeNew ");
			strSql.Append(" FROM smsUserInfo ");
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
			strSql.Append(" id,userNo,userName,mobile,userRank,leaderUserNo,creater,createTime,smsType,defaultSend,smsTypeNew ");
			strSql.Append(" FROM smsUserInfo ");
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
			strSql.Append("select count(1) FROM smsUserInfo ");
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
			strSql.Append(")AS Row, T.*  from smsUserInfo T ");
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
			parameters[0].Value = "smsUserInfo";
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

