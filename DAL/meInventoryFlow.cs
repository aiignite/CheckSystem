using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:meInventoryFlow
	/// </summary>
	public partial class meInventoryFlow
	{
		public meInventoryFlow()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from meInventoryFlow");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.NVarChar,50)			};
			parameters[0].Value = ID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.meInventoryFlow model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into meInventoryFlow(");
			strSql.Append("ID,InOut,MaterialType,Barcode,Num,StorageNo,Createby,Createdt,NoteNo)");
			strSql.Append(" values (");
			strSql.Append("@ID,@InOut,@MaterialType,@Barcode,@Num,@StorageNo,@Createby,@Createdt,@NoteNo)");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.NVarChar,50),
					new SqlParameter("@InOut", SqlDbType.Int,4),
					new SqlParameter("@MaterialType", SqlDbType.Int,4),
					new SqlParameter("@Barcode", SqlDbType.NVarChar,100),
					new SqlParameter("@Num", SqlDbType.Decimal,9),
					new SqlParameter("@StorageNo", SqlDbType.NVarChar,50),
					new SqlParameter("@Createby", SqlDbType.NVarChar,50),
					new SqlParameter("@Createdt", SqlDbType.DateTime),
					new SqlParameter("@NoteNo", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.InOut;
			parameters[2].Value = model.MaterialType;
			parameters[3].Value = model.Barcode;
			parameters[4].Value = model.Num;
			parameters[5].Value = model.StorageNo;
			parameters[6].Value = model.Createby;
			parameters[7].Value = model.Createdt;
			parameters[8].Value = model.NoteNo;

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
		public bool Update(Model.meInventoryFlow model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update meInventoryFlow set ");
			strSql.Append("InOut=@InOut,");
			strSql.Append("MaterialType=@MaterialType,");
			strSql.Append("Barcode=@Barcode,");
			strSql.Append("Num=@Num,");
			strSql.Append("StorageNo=@StorageNo,");
			strSql.Append("Createby=@Createby,");
			strSql.Append("Createdt=@Createdt,");
			strSql.Append("NoteNo=@NoteNo");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@InOut", SqlDbType.Int,4),
					new SqlParameter("@MaterialType", SqlDbType.Int,4),
					new SqlParameter("@Barcode", SqlDbType.NVarChar,100),
					new SqlParameter("@Num", SqlDbType.Decimal,9),
					new SqlParameter("@StorageNo", SqlDbType.NVarChar,50),
					new SqlParameter("@Createby", SqlDbType.NVarChar,50),
					new SqlParameter("@Createdt", SqlDbType.DateTime),
					new SqlParameter("@NoteNo", SqlDbType.NVarChar,50),
					new SqlParameter("@ID", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.InOut;
			parameters[1].Value = model.MaterialType;
			parameters[2].Value = model.Barcode;
			parameters[3].Value = model.Num;
			parameters[4].Value = model.StorageNo;
			parameters[5].Value = model.Createby;
			parameters[6].Value = model.Createdt;
			parameters[7].Value = model.NoteNo;
			parameters[8].Value = model.ID;

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
		public bool Delete(string ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from meInventoryFlow ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.NVarChar,50)			};
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
			strSql.Append("delete from meInventoryFlow ");
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
		public Model.meInventoryFlow GetModel(string ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,InOut,MaterialType,Barcode,Num,StorageNo,Createby,Createdt,NoteNo from meInventoryFlow ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.NVarChar,50)			};
			parameters[0].Value = ID;

			Model.meInventoryFlow model=new Model.meInventoryFlow();
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
		public Model.meInventoryFlow DataRowToModel(DataRow row)
		{
			Model.meInventoryFlow model=new Model.meInventoryFlow();
			if (row != null)
			{
				if(row["ID"]!=null)
				{
					model.ID=row["ID"].ToString();
				}
				if(row["InOut"]!=null && row["InOut"].ToString()!="")
				{
					model.InOut=int.Parse(row["InOut"].ToString());
				}
				if(row["MaterialType"]!=null && row["MaterialType"].ToString()!="")
				{
					model.MaterialType=int.Parse(row["MaterialType"].ToString());
				}
				if(row["Barcode"]!=null)
				{
					model.Barcode=row["Barcode"].ToString();
				}
				if(row["Num"]!=null && row["Num"].ToString()!="")
				{
					model.Num=decimal.Parse(row["Num"].ToString());
				}
				if(row["StorageNo"]!=null)
				{
					model.StorageNo=row["StorageNo"].ToString();
				}
				if(row["Createby"]!=null)
				{
					model.Createby=row["Createby"].ToString();
				}
				if(row["Createdt"]!=null && row["Createdt"].ToString()!="")
				{
					model.Createdt=DateTime.Parse(row["Createdt"].ToString());
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
			strSql.Append("select ID,InOut,MaterialType,Barcode,Num,StorageNo,Createby,Createdt,NoteNo ");
			strSql.Append(" FROM meInventoryFlow ");
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
			strSql.Append(" ID,InOut,MaterialType,Barcode,Num,StorageNo,Createby,Createdt,NoteNo ");
			strSql.Append(" FROM meInventoryFlow ");
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
			strSql.Append("select count(1) FROM meInventoryFlow ");
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
			strSql.Append(")AS Row, T.*  from meInventoryFlow T ");
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
			parameters[0].Value = "meInventoryFlow";
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

