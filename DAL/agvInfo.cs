using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:agvInfo
	/// </summary>
	public partial class agvInfo
	{
		public agvInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "agvInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from agvInfo");
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
		public int Add(Model.agvInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into agvInfo(");
			strSql.Append("agvNo,agvName,moduleNo,moduleIpport,agvSoc,agvPosition,agvStatus,AGVWorkinDate,createTime)");
			strSql.Append(" values (");
			strSql.Append("@agvNo,@agvName,@moduleNo,@moduleIpport,@agvSoc,@agvPosition,@agvStatus,@AGVWorkinDate,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@agvNo", SqlDbType.NVarChar,50),
					new SqlParameter("@agvName", SqlDbType.NVarChar,50),
					new SqlParameter("@moduleNo", SqlDbType.NVarChar,50),
					new SqlParameter("@moduleIpport", SqlDbType.NVarChar,50),
					new SqlParameter("@agvSoc", SqlDbType.Int,4),
					new SqlParameter("@agvPosition", SqlDbType.NVarChar,50),
					new SqlParameter("@agvStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@AGVWorkinDate", SqlDbType.DateTime),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.agvNo;
			parameters[1].Value = model.agvName;
			parameters[2].Value = model.moduleNo;
			parameters[3].Value = model.moduleIpport;
			parameters[4].Value = model.agvSoc;
			parameters[5].Value = model.agvPosition;
			parameters[6].Value = model.agvStatus;
			parameters[7].Value = model.AGVWorkinDate;
			parameters[8].Value = model.createTime;

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
		public bool Update(Model.agvInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update agvInfo set ");
			strSql.Append("agvNo=@agvNo,");
			strSql.Append("agvName=@agvName,");
			strSql.Append("moduleNo=@moduleNo,");
			strSql.Append("moduleIpport=@moduleIpport,");
			strSql.Append("agvSoc=@agvSoc,");
			strSql.Append("agvPosition=@agvPosition,");
			strSql.Append("agvStatus=@agvStatus,");
			strSql.Append("AGVWorkinDate=@AGVWorkinDate,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@agvNo", SqlDbType.NVarChar,50),
					new SqlParameter("@agvName", SqlDbType.NVarChar,50),
					new SqlParameter("@moduleNo", SqlDbType.NVarChar,50),
					new SqlParameter("@moduleIpport", SqlDbType.NVarChar,50),
					new SqlParameter("@agvSoc", SqlDbType.Int,4),
					new SqlParameter("@agvPosition", SqlDbType.NVarChar,50),
					new SqlParameter("@agvStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@AGVWorkinDate", SqlDbType.DateTime),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.agvNo;
			parameters[1].Value = model.agvName;
			parameters[2].Value = model.moduleNo;
			parameters[3].Value = model.moduleIpport;
			parameters[4].Value = model.agvSoc;
			parameters[5].Value = model.agvPosition;
			parameters[6].Value = model.agvStatus;
			parameters[7].Value = model.AGVWorkinDate;
			parameters[8].Value = model.createTime;
			parameters[9].Value = model.id;

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
			strSql.Append("delete from agvInfo ");
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
			strSql.Append("delete from agvInfo ");
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
		public Model.agvInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,agvNo,agvName,moduleNo,moduleIpport,agvSoc,agvPosition,agvStatus,AGVWorkinDate,createTime from agvInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.agvInfo model=new Model.agvInfo();
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
		public Model.agvInfo DataRowToModel(DataRow row)
		{
			Model.agvInfo model=new Model.agvInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["agvNo"]!=null)
				{
					model.agvNo=row["agvNo"].ToString();
				}
				if(row["agvName"]!=null)
				{
					model.agvName=row["agvName"].ToString();
				}
				if(row["moduleNo"]!=null)
				{
					model.moduleNo=row["moduleNo"].ToString();
				}
				if(row["moduleIpport"]!=null)
				{
					model.moduleIpport=row["moduleIpport"].ToString();
				}
				if(row["agvSoc"]!=null && row["agvSoc"].ToString()!="")
				{
					model.agvSoc=int.Parse(row["agvSoc"].ToString());
				}
				if(row["agvPosition"]!=null)
				{
					model.agvPosition=row["agvPosition"].ToString();
				}
				if(row["agvStatus"]!=null)
				{
					model.agvStatus=row["agvStatus"].ToString();
				}
				if(row["AGVWorkinDate"]!=null && row["AGVWorkinDate"].ToString()!="")
				{
					model.AGVWorkinDate=DateTime.Parse(row["AGVWorkinDate"].ToString());
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
			strSql.Append("select id,agvNo,agvName,moduleNo,moduleIpport,agvSoc,agvPosition,agvStatus,AGVWorkinDate,createTime ");
			strSql.Append(" FROM agvInfo ");
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
			strSql.Append(" id,agvNo,agvName,moduleNo,moduleIpport,agvSoc,agvPosition,agvStatus,AGVWorkinDate,createTime ");
			strSql.Append(" FROM agvInfo ");
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
			strSql.Append("select count(1) FROM agvInfo ");
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
			strSql.Append(")AS Row, T.*  from agvInfo T ");
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
			parameters[0].Value = "agvInfo";
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

