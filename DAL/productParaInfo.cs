using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:productParaInfo
	/// </summary>
	public partial class productParaInfo
	{
		public productParaInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "productParaInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from productParaInfo");
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
		public int Add(Model.productParaInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into productParaInfo(");
			strSql.Append("paraNo,paraName,paraNameCN,paraType,paraUnit,productType,note,creater,createTIme)");
			strSql.Append(" values (");
			strSql.Append("@paraNo,@paraName,@paraNameCN,@paraType,@paraUnit,@productType,@note,@creater,@createTIme)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@paraNo", SqlDbType.NVarChar,50),
					new SqlParameter("@paraName", SqlDbType.NVarChar,50),
					new SqlParameter("@paraNameCN", SqlDbType.NVarChar,50),
					new SqlParameter("@paraType", SqlDbType.NVarChar,50),
					new SqlParameter("@paraUnit", SqlDbType.NVarChar,50),
					new SqlParameter("@productType", SqlDbType.NVarChar,50),
					new SqlParameter("@note", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTIme", SqlDbType.DateTime)};
			parameters[0].Value = model.paraNo;
			parameters[1].Value = model.paraName;
			parameters[2].Value = model.paraNameCN;
			parameters[3].Value = model.paraType;
			parameters[4].Value = model.paraUnit;
			parameters[5].Value = model.productType;
			parameters[6].Value = model.note;
			parameters[7].Value = model.creater;
			parameters[8].Value = model.createTIme;

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
		public bool Update(Model.productParaInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update productParaInfo set ");
			strSql.Append("paraNo=@paraNo,");
			strSql.Append("paraName=@paraName,");
			strSql.Append("paraNameCN=@paraNameCN,");
			strSql.Append("paraType=@paraType,");
			strSql.Append("paraUnit=@paraUnit,");
			strSql.Append("productType=@productType,");
			strSql.Append("note=@note,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTIme=@createTIme");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@paraNo", SqlDbType.NVarChar,50),
					new SqlParameter("@paraName", SqlDbType.NVarChar,50),
					new SqlParameter("@paraNameCN", SqlDbType.NVarChar,50),
					new SqlParameter("@paraType", SqlDbType.NVarChar,50),
					new SqlParameter("@paraUnit", SqlDbType.NVarChar,50),
					new SqlParameter("@productType", SqlDbType.NVarChar,50),
					new SqlParameter("@note", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTIme", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.paraNo;
			parameters[1].Value = model.paraName;
			parameters[2].Value = model.paraNameCN;
			parameters[3].Value = model.paraType;
			parameters[4].Value = model.paraUnit;
			parameters[5].Value = model.productType;
			parameters[6].Value = model.note;
			parameters[7].Value = model.creater;
			parameters[8].Value = model.createTIme;
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
			strSql.Append("delete from productParaInfo ");
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
			strSql.Append("delete from productParaInfo ");
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
		public Model.productParaInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,paraNo,paraName,paraNameCN,paraType,paraUnit,productType,note,creater,createTIme from productParaInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.productParaInfo model=new Model.productParaInfo();
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
		public Model.productParaInfo DataRowToModel(DataRow row)
		{
			Model.productParaInfo model=new Model.productParaInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["paraNo"]!=null)
				{
					model.paraNo=row["paraNo"].ToString();
				}
				if(row["paraName"]!=null)
				{
					model.paraName=row["paraName"].ToString();
				}
				if(row["paraNameCN"]!=null)
				{
					model.paraNameCN=row["paraNameCN"].ToString();
				}
				if(row["paraType"]!=null)
				{
					model.paraType=row["paraType"].ToString();
				}
				if(row["paraUnit"]!=null)
				{
					model.paraUnit=row["paraUnit"].ToString();
				}
				if(row["productType"]!=null)
				{
					model.productType=row["productType"].ToString();
				}
				if(row["note"]!=null)
				{
					model.note=row["note"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTIme"]!=null && row["createTIme"].ToString()!="")
				{
					model.createTIme=DateTime.Parse(row["createTIme"].ToString());
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
			strSql.Append("select id,paraNo,paraName,paraNameCN,paraType,paraUnit,productType,note,creater,createTIme ");
			strSql.Append(" FROM productParaInfo ");
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
			strSql.Append(" id,paraNo,paraName,paraNameCN,paraType,paraUnit,productType,note,creater,createTIme ");
			strSql.Append(" FROM productParaInfo ");
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
			strSql.Append("select count(1) FROM productParaInfo ");
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
			strSql.Append(")AS Row, T.*  from productParaInfo T ");
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
			parameters[0].Value = "productParaInfo";
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

