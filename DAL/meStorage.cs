using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:meStorage
	/// </summary>
	public partial class meStorage
	{
		public meStorage()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string StorageID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from meStorage");
			strSql.Append(" where StorageID=@StorageID ");
			SqlParameter[] parameters = {
					new SqlParameter("@StorageID", SqlDbType.NVarChar,50)			};
			parameters[0].Value = StorageID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.meStorage model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into meStorage(");
			strSql.Append("StorageID,MaterialType,StorageNo,BarCode,Num,NoteNo)");
			strSql.Append(" values (");
			strSql.Append("@StorageID,@MaterialType,@StorageNo,@BarCode,@Num,@NoteNo)");
			SqlParameter[] parameters = {
					new SqlParameter("@StorageID", SqlDbType.NVarChar,50),
					new SqlParameter("@MaterialType", SqlDbType.Int,4),
					new SqlParameter("@StorageNo", SqlDbType.NVarChar,50),
					new SqlParameter("@BarCode", SqlDbType.NVarChar,100),
					new SqlParameter("@Num", SqlDbType.Decimal,9),
					new SqlParameter("@NoteNo", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.StorageID;
			parameters[1].Value = model.MaterialType;
			parameters[2].Value = model.StorageNo;
			parameters[3].Value = model.BarCode;
			parameters[4].Value = model.Num;
			parameters[5].Value = model.NoteNo;

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
		public bool Update(Model.meStorage model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update meStorage set ");
			strSql.Append("MaterialType=@MaterialType,");
			strSql.Append("StorageNo=@StorageNo,");
			strSql.Append("BarCode=@BarCode,");
			strSql.Append("Num=@Num,");
			strSql.Append("NoteNo=@NoteNo");
			strSql.Append(" where StorageID=@StorageID ");
			SqlParameter[] parameters = {
					new SqlParameter("@MaterialType", SqlDbType.Int,4),
					new SqlParameter("@StorageNo", SqlDbType.NVarChar,50),
					new SqlParameter("@BarCode", SqlDbType.NVarChar,100),
					new SqlParameter("@Num", SqlDbType.Decimal,9),
					new SqlParameter("@NoteNo", SqlDbType.NVarChar,50),
					new SqlParameter("@StorageID", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.MaterialType;
			parameters[1].Value = model.StorageNo;
			parameters[2].Value = model.BarCode;
			parameters[3].Value = model.Num;
			parameters[4].Value = model.NoteNo;
			parameters[5].Value = model.StorageID;

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
		public bool Delete(string StorageID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from meStorage ");
			strSql.Append(" where StorageID=@StorageID ");
			SqlParameter[] parameters = {
					new SqlParameter("@StorageID", SqlDbType.NVarChar,50)			};
			parameters[0].Value = StorageID;

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
		public bool DeleteList(string StorageIDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from meStorage ");
			strSql.Append(" where StorageID in ("+StorageIDlist + ")  ");
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
		public Model.meStorage GetModel(string StorageID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 StorageID,MaterialType,StorageNo,BarCode,Num,NoteNo from meStorage ");
			strSql.Append(" where StorageID=@StorageID ");
			SqlParameter[] parameters = {
					new SqlParameter("@StorageID", SqlDbType.NVarChar,50)			};
			parameters[0].Value = StorageID;

			Model.meStorage model=new Model.meStorage();
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
		public Model.meStorage DataRowToModel(DataRow row)
		{
			Model.meStorage model=new Model.meStorage();
			if (row != null)
			{
				if(row["StorageID"]!=null)
				{
					model.StorageID=row["StorageID"].ToString();
				}
				if(row["MaterialType"]!=null && row["MaterialType"].ToString()!="")
				{
					model.MaterialType=int.Parse(row["MaterialType"].ToString());
				}
				if(row["StorageNo"]!=null)
				{
					model.StorageNo=row["StorageNo"].ToString();
				}
				if(row["BarCode"]!=null)
				{
					model.BarCode=row["BarCode"].ToString();
				}
				if(row["Num"]!=null && row["Num"].ToString()!="")
				{
					model.Num=decimal.Parse(row["Num"].ToString());
				}
				if(row["NoteNo"]!=null)
				{
					model.NoteNo=row["NoteNo"].ToString();
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
			strSql.Append("select StorageID,MaterialType,StorageNo,BarCode,Num,NoteNo ");
			strSql.Append(" FROM meStorage ");
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
			strSql.Append(" StorageID,MaterialType,StorageNo,BarCode,Num,NoteNo ");
			strSql.Append(" FROM meStorage ");
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
			strSql.Append("select count(1) FROM meStorage ");
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
				strSql.Append("order by T.StorageID desc");
			}
			strSql.Append(")AS Row, T.*  from meStorage T ");
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
			parameters[0].Value = "meStorage";
			parameters[1].Value = "StorageID";
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

