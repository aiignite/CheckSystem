using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:productQualityData
	/// </summary>
	public partial class productQualityData
	{
		public productQualityData()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("DID", "productQualityData"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int DID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from productQualityData");
			strSql.Append(" where DID=@DID");
			SqlParameter[] parameters = {
					new SqlParameter("@DID", SqlDbType.Int,4)
			};
			parameters[0].Value = DID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(Model.productQualityData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into productQualityData(");
			strSql.Append("ID,productBarcode,status,UpdateDate,UpdateMan)");
			strSql.Append(" values (");
			strSql.Append("@ID,@productBarcode,@status,@UpdateDate,@UpdateMan)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@productBarcode", SqlDbType.NVarChar,150),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@UpdateDate", SqlDbType.DateTime),
					new SqlParameter("@UpdateMan", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.productBarcode;
			parameters[2].Value = model.status;
			parameters[3].Value = model.UpdateDate;
			parameters[4].Value = model.UpdateMan;

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
		public bool Update(Model.productQualityData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update productQualityData set ");
			strSql.Append("ID=@ID,");
			strSql.Append("productBarcode=@productBarcode,");
			strSql.Append("status=@status,");
			strSql.Append("UpdateDate=@UpdateDate,");
			strSql.Append("UpdateMan=@UpdateMan");
			strSql.Append(" where DID=@DID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@productBarcode", SqlDbType.NVarChar,150),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@UpdateDate", SqlDbType.DateTime),
					new SqlParameter("@UpdateMan", SqlDbType.NVarChar,50),
					new SqlParameter("@DID", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.productBarcode;
			parameters[2].Value = model.status;
			parameters[3].Value = model.UpdateDate;
			parameters[4].Value = model.UpdateMan;
			parameters[5].Value = model.DID;

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
		public bool Delete(int DID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from productQualityData ");
			strSql.Append(" where DID=@DID");
			SqlParameter[] parameters = {
					new SqlParameter("@DID", SqlDbType.Int,4)
			};
			parameters[0].Value = DID;

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
		public bool DeleteList(string DIDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from productQualityData ");
			strSql.Append(" where DID in ("+DIDlist + ")  ");
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
		public Model.productQualityData GetModel(int DID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 DID,ID,productBarcode,status,UpdateDate,UpdateMan from productQualityData ");
			strSql.Append(" where DID=@DID");
			SqlParameter[] parameters = {
					new SqlParameter("@DID", SqlDbType.Int,4)
			};
			parameters[0].Value = DID;

			Model.productQualityData model=new Model.productQualityData();
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
		public Model.productQualityData DataRowToModel(DataRow row)
		{
			Model.productQualityData model=new Model.productQualityData();
			if (row != null)
			{
				if(row["DID"]!=null && row["DID"].ToString()!="")
				{
					model.DID=int.Parse(row["DID"].ToString());
				}
				if(row["ID"]!=null && row["ID"].ToString()!="")
				{
					model.ID=int.Parse(row["ID"].ToString());
				}
				if(row["productBarcode"]!=null)
				{
					model.productBarcode=row["productBarcode"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["UpdateDate"]!=null && row["UpdateDate"].ToString()!="")
				{
					model.UpdateDate=DateTime.Parse(row["UpdateDate"].ToString());
				}
				if(row["UpdateMan"]!=null)
				{
					model.UpdateMan=row["UpdateMan"].ToString();
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
			strSql.Append("select DID,ID,productBarcode,status,UpdateDate,UpdateMan ");
			strSql.Append(" FROM productQualityData ");
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
			strSql.Append(" DID,ID,productBarcode,status,UpdateDate,UpdateMan ");
			strSql.Append(" FROM productQualityData ");
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
			strSql.Append("select count(1) FROM productQualityData ");
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
				strSql.Append("order by T.DID desc");
			}
			strSql.Append(")AS Row, T.*  from productQualityData T ");
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
			parameters[0].Value = "productQualityData";
			parameters[1].Value = "DID";
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

