using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:manufactureShowData
	/// </summary>
	public partial class manufactureShowData
	{
		public manufactureShowData()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "manufactureShowData"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from manufactureShowData");
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
		public int Add(Model.manufactureShowData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into manufactureShowData(");
			strSql.Append("productNo,productName,status,shopName,responselevel,responseman,checkTime,listurl,listvalue,createTime)");
			strSql.Append(" values (");
			strSql.Append("@productNo,@productName,@status,@shopName,@responselevel,@responseman,@checkTime,@listurl,@listvalue,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,100),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@shopName", SqlDbType.NVarChar,50),
					new SqlParameter("@responselevel", SqlDbType.NVarChar,50),
					new SqlParameter("@responseman", SqlDbType.NVarChar,50),
					new SqlParameter("@checkTime", SqlDbType.DateTime),
					new SqlParameter("@listurl", SqlDbType.NVarChar,50),
					new SqlParameter("@listvalue", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.productNo;
			parameters[1].Value = model.productName;
			parameters[2].Value = model.status;
			parameters[3].Value = model.shopName;
			parameters[4].Value = model.responselevel;
			parameters[5].Value = model.responseman;
			parameters[6].Value = model.checkTime;
			parameters[7].Value = model.listurl;
			parameters[8].Value = model.listvalue;
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
		public bool Update(Model.manufactureShowData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update manufactureShowData set ");
			strSql.Append("productNo=@productNo,");
			strSql.Append("productName=@productName,");
			strSql.Append("status=@status,");
			strSql.Append("shopName=@shopName,");
			strSql.Append("responselevel=@responselevel,");
			strSql.Append("responseman=@responseman,");
			strSql.Append("checkTime=@checkTime,");
			strSql.Append("listurl=@listurl,");
			strSql.Append("listvalue=@listvalue,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,100),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@shopName", SqlDbType.NVarChar,50),
					new SqlParameter("@responselevel", SqlDbType.NVarChar,50),
					new SqlParameter("@responseman", SqlDbType.NVarChar,50),
					new SqlParameter("@checkTime", SqlDbType.DateTime),
					new SqlParameter("@listurl", SqlDbType.NVarChar,50),
					new SqlParameter("@listvalue", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.productNo;
			parameters[1].Value = model.productName;
			parameters[2].Value = model.status;
			parameters[3].Value = model.shopName;
			parameters[4].Value = model.responselevel;
			parameters[5].Value = model.responseman;
			parameters[6].Value = model.checkTime;
			parameters[7].Value = model.listurl;
			parameters[8].Value = model.listvalue;
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
			strSql.Append("delete from manufactureShowData ");
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
			strSql.Append("delete from manufactureShowData ");
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
		public Model.manufactureShowData GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,productNo,productName,status,shopName,responselevel,responseman,checkTime,listurl,listvalue,createTime from manufactureShowData ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.manufactureShowData model=new Model.manufactureShowData();
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
		public Model.manufactureShowData DataRowToModel(DataRow row)
		{
			Model.manufactureShowData model=new Model.manufactureShowData();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["productName"]!=null)
				{
					model.productName=row["productName"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["shopName"]!=null)
				{
					model.shopName=row["shopName"].ToString();
				}
				if(row["responselevel"]!=null)
				{
					model.responselevel=row["responselevel"].ToString();
				}
				if(row["responseman"]!=null)
				{
					model.responseman=row["responseman"].ToString();
				}
				if(row["checkTime"]!=null && row["checkTime"].ToString()!="")
				{
					model.checkTime=DateTime.Parse(row["checkTime"].ToString());
				}
				if(row["listurl"]!=null)
				{
					model.listurl=row["listurl"].ToString();
				}
				if(row["listvalue"]!=null)
				{
					model.listvalue=row["listvalue"].ToString();
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
			strSql.Append("select id,productNo,productName,status,shopName,responselevel,responseman,checkTime,listurl,listvalue,createTime ");
			strSql.Append(" FROM manufactureShowData ");
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
			strSql.Append(" id,productNo,productName,status,shopName,responselevel,responseman,checkTime,listurl,listvalue,createTime ");
			strSql.Append(" FROM manufactureShowData ");
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
			strSql.Append("select count(1) FROM manufactureShowData ");
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
			strSql.Append(")AS Row, T.*  from manufactureShowData T ");
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
			parameters[0].Value = "manufactureShowData";
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

