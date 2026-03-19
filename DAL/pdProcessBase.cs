using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:pdProcessBase
	/// </summary>
	public partial class pdProcessBase
	{
		public pdProcessBase()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "pdProcessBase"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from pdProcessBase");
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
		public int Add(Model.pdProcessBase model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into pdProcessBase(");
			strSql.Append("pBNO,pBName,pBDevice,pBType,actType,pBNote,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@pBNO,@pBName,@pBDevice,@pBType,@actType,@pBNote,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@pBNO", SqlDbType.NVarChar,50),
					new SqlParameter("@pBName", SqlDbType.NVarChar,100),
					new SqlParameter("@pBDevice", SqlDbType.NVarChar,100),
					new SqlParameter("@pBType", SqlDbType.NVarChar,50),
					new SqlParameter("@actType", SqlDbType.NVarChar,10),
					new SqlParameter("@pBNote", SqlDbType.NVarChar,200),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.pBNO;
			parameters[1].Value = model.pBName;
			parameters[2].Value = model.pBDevice;
			parameters[3].Value = model.pBType;
			parameters[4].Value = model.actType;
			parameters[5].Value = model.pBNote;
			parameters[6].Value = model.creater;
			parameters[7].Value = model.createTime;

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
		public bool Update(Model.pdProcessBase model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update pdProcessBase set ");
			strSql.Append("pBNO=@pBNO,");
			strSql.Append("pBName=@pBName,");
			strSql.Append("pBDevice=@pBDevice,");
			strSql.Append("pBType=@pBType,");
			strSql.Append("actType=@actType,");
			strSql.Append("pBNote=@pBNote,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@pBNO", SqlDbType.NVarChar,50),
					new SqlParameter("@pBName", SqlDbType.NVarChar,100),
					new SqlParameter("@pBDevice", SqlDbType.NVarChar,100),
					new SqlParameter("@pBType", SqlDbType.NVarChar,50),
					new SqlParameter("@actType", SqlDbType.NVarChar,10),
					new SqlParameter("@pBNote", SqlDbType.NVarChar,200),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.pBNO;
			parameters[1].Value = model.pBName;
			parameters[2].Value = model.pBDevice;
			parameters[3].Value = model.pBType;
			parameters[4].Value = model.actType;
			parameters[5].Value = model.pBNote;
			parameters[6].Value = model.creater;
			parameters[7].Value = model.createTime;
			parameters[8].Value = model.id;

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
			strSql.Append("delete from pdProcessBase ");
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
			strSql.Append("delete from pdProcessBase ");
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
		public Model.pdProcessBase GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,pBNO,pBName,pBDevice,pBType,actType,pBNote,creater,createTime from pdProcessBase ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.pdProcessBase model=new Model.pdProcessBase();
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
		public Model.pdProcessBase DataRowToModel(DataRow row)
		{
			Model.pdProcessBase model=new Model.pdProcessBase();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["pBNO"]!=null)
				{
					model.pBNO=row["pBNO"].ToString();
				}
				if(row["pBName"]!=null)
				{
					model.pBName=row["pBName"].ToString();
				}
				if(row["pBDevice"]!=null)
				{
					model.pBDevice=row["pBDevice"].ToString();
				}
				if(row["pBType"]!=null)
				{
					model.pBType=row["pBType"].ToString();
				}
				if(row["actType"]!=null)
				{
					model.actType=row["actType"].ToString();
				}
				if(row["pBNote"]!=null)
				{
					model.pBNote=row["pBNote"].ToString();
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
			strSql.Append("select id,pBNO,pBName,pBDevice,pBType,actType,pBNote,creater,createTime ");
			strSql.Append(" FROM pdProcessBase ");
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
			strSql.Append(" id,pBNO,pBName,pBDevice,pBType,actType,pBNote,creater,createTime ");
			strSql.Append(" FROM pdProcessBase ");
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
			strSql.Append("select count(1) FROM pdProcessBase ");
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
			strSql.Append(")AS Row, T.*  from pdProcessBase T ");
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
			parameters[0].Value = "pdProcessBase";
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

