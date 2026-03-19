using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:agvRfidInfo
	/// </summary>
	public partial class agvRfidInfo
	{
		public agvRfidInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "agvRfidInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from agvRfidInfo");
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
		public int Add(Model.agvRfidInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into agvRfidInfo(");
			strSql.Append("rfidNo,rfidType,stationNo,positionNote,coordsFactory,coordsImage,status,createTime)");
			strSql.Append(" values (");
			strSql.Append("@rfidNo,@rfidType,@stationNo,@positionNote,@coordsFactory,@coordsImage,@status,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@rfidNo", SqlDbType.NVarChar,50),
					new SqlParameter("@rfidType", SqlDbType.NVarChar,50),
					new SqlParameter("@stationNo", SqlDbType.NVarChar,50),
					new SqlParameter("@positionNote", SqlDbType.NVarChar,50),
					new SqlParameter("@coordsFactory", SqlDbType.NVarChar,50),
					new SqlParameter("@coordsImage", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.rfidNo;
			parameters[1].Value = model.rfidType;
			parameters[2].Value = model.stationNo;
			parameters[3].Value = model.positionNote;
			parameters[4].Value = model.coordsFactory;
			parameters[5].Value = model.coordsImage;
			parameters[6].Value = model.status;
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
		public bool Update(Model.agvRfidInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update agvRfidInfo set ");
			strSql.Append("rfidNo=@rfidNo,");
			strSql.Append("rfidType=@rfidType,");
			strSql.Append("stationNo=@stationNo,");
			strSql.Append("positionNote=@positionNote,");
			strSql.Append("coordsFactory=@coordsFactory,");
			strSql.Append("coordsImage=@coordsImage,");
			strSql.Append("status=@status,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@rfidNo", SqlDbType.NVarChar,50),
					new SqlParameter("@rfidType", SqlDbType.NVarChar,50),
					new SqlParameter("@stationNo", SqlDbType.NVarChar,50),
					new SqlParameter("@positionNote", SqlDbType.NVarChar,50),
					new SqlParameter("@coordsFactory", SqlDbType.NVarChar,50),
					new SqlParameter("@coordsImage", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.rfidNo;
			parameters[1].Value = model.rfidType;
			parameters[2].Value = model.stationNo;
			parameters[3].Value = model.positionNote;
			parameters[4].Value = model.coordsFactory;
			parameters[5].Value = model.coordsImage;
			parameters[6].Value = model.status;
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
			strSql.Append("delete from agvRfidInfo ");
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
			strSql.Append("delete from agvRfidInfo ");
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
		public Model.agvRfidInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,rfidNo,rfidType,stationNo,positionNote,coordsFactory,coordsImage,status,createTime from agvRfidInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.agvRfidInfo model=new Model.agvRfidInfo();
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
		public Model.agvRfidInfo DataRowToModel(DataRow row)
		{
			Model.agvRfidInfo model=new Model.agvRfidInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["rfidNo"]!=null)
				{
					model.rfidNo=row["rfidNo"].ToString();
				}
				if(row["rfidType"]!=null)
				{
					model.rfidType=row["rfidType"].ToString();
				}
				if(row["stationNo"]!=null)
				{
					model.stationNo=row["stationNo"].ToString();
				}
				if(row["positionNote"]!=null)
				{
					model.positionNote=row["positionNote"].ToString();
				}
				if(row["coordsFactory"]!=null)
				{
					model.coordsFactory=row["coordsFactory"].ToString();
				}
				if(row["coordsImage"]!=null)
				{
					model.coordsImage=row["coordsImage"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
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
			strSql.Append("select id,rfidNo,rfidType,stationNo,positionNote,coordsFactory,coordsImage,status,createTime ");
			strSql.Append(" FROM agvRfidInfo ");
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
			strSql.Append(" id,rfidNo,rfidType,stationNo,positionNote,coordsFactory,coordsImage,status,createTime ");
			strSql.Append(" FROM agvRfidInfo ");
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
			strSql.Append("select count(1) FROM agvRfidInfo ");
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
			strSql.Append(")AS Row, T.*  from agvRfidInfo T ");
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
			parameters[0].Value = "agvRfidInfo";
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

