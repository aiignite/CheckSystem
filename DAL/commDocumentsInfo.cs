using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:commDocumentsInfo
	/// </summary>
	public partial class commDocumentsInfo
	{
		public commDocumentsInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "commDocumentsInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from commDocumentsInfo");
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
		public int Add(Model.commDocumentsInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into commDocumentsInfo(");
			strSql.Append("docNo,docName,docExtensionName,docLink,docPath,docType,docTypeNo,uploader,uploadDate,isValid,createTime)");
			strSql.Append(" values (");
			strSql.Append("@docNo,@docName,@docExtensionName,@docLink,@docPath,@docType,@docTypeNo,@uploader,@uploadDate,@isValid,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@docNo", SqlDbType.NVarChar,50),
					new SqlParameter("@docName", SqlDbType.NVarChar,50),
					new SqlParameter("@docExtensionName", SqlDbType.NVarChar,50),
					new SqlParameter("@docLink", SqlDbType.NVarChar,50),
					new SqlParameter("@docPath", SqlDbType.NVarChar,50),
					new SqlParameter("@docType", SqlDbType.NVarChar,50),
					new SqlParameter("@docTypeNo", SqlDbType.NVarChar,50),
					new SqlParameter("@uploader", SqlDbType.NVarChar,50),
					new SqlParameter("@uploadDate", SqlDbType.NVarChar,50),
					new SqlParameter("@isValid", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.docNo;
			parameters[1].Value = model.docName;
			parameters[2].Value = model.docExtensionName;
			parameters[3].Value = model.docLink;
			parameters[4].Value = model.docPath;
			parameters[5].Value = model.docType;
			parameters[6].Value = model.docTypeNo;
			parameters[7].Value = model.uploader;
			parameters[8].Value = model.uploadDate;
			parameters[9].Value = model.isValid;
			parameters[10].Value = model.createTime;

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
		public bool Update(Model.commDocumentsInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update commDocumentsInfo set ");
			strSql.Append("docNo=@docNo,");
			strSql.Append("docName=@docName,");
			strSql.Append("docExtensionName=@docExtensionName,");
			strSql.Append("docLink=@docLink,");
			strSql.Append("docPath=@docPath,");
			strSql.Append("docType=@docType,");
			strSql.Append("docTypeNo=@docTypeNo,");
			strSql.Append("uploader=@uploader,");
			strSql.Append("uploadDate=@uploadDate,");
			strSql.Append("isValid=@isValid,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@docNo", SqlDbType.NVarChar,50),
					new SqlParameter("@docName", SqlDbType.NVarChar,50),
					new SqlParameter("@docExtensionName", SqlDbType.NVarChar,50),
					new SqlParameter("@docLink", SqlDbType.NVarChar,50),
					new SqlParameter("@docPath", SqlDbType.NVarChar,50),
					new SqlParameter("@docType", SqlDbType.NVarChar,50),
					new SqlParameter("@docTypeNo", SqlDbType.NVarChar,50),
					new SqlParameter("@uploader", SqlDbType.NVarChar,50),
					new SqlParameter("@uploadDate", SqlDbType.NVarChar,50),
					new SqlParameter("@isValid", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.docNo;
			parameters[1].Value = model.docName;
			parameters[2].Value = model.docExtensionName;
			parameters[3].Value = model.docLink;
			parameters[4].Value = model.docPath;
			parameters[5].Value = model.docType;
			parameters[6].Value = model.docTypeNo;
			parameters[7].Value = model.uploader;
			parameters[8].Value = model.uploadDate;
			parameters[9].Value = model.isValid;
			parameters[10].Value = model.createTime;
			parameters[11].Value = model.id;

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
			strSql.Append("delete from commDocumentsInfo ");
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
			strSql.Append("delete from commDocumentsInfo ");
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
		public Model.commDocumentsInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,docNo,docName,docExtensionName,docLink,docPath,docType,docTypeNo,uploader,uploadDate,isValid,createTime from commDocumentsInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.commDocumentsInfo model=new Model.commDocumentsInfo();
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
		public Model.commDocumentsInfo DataRowToModel(DataRow row)
		{
			Model.commDocumentsInfo model=new Model.commDocumentsInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["docNo"]!=null)
				{
					model.docNo=row["docNo"].ToString();
				}
				if(row["docName"]!=null)
				{
					model.docName=row["docName"].ToString();
				}
				if(row["docExtensionName"]!=null)
				{
					model.docExtensionName=row["docExtensionName"].ToString();
				}
				if(row["docLink"]!=null)
				{
					model.docLink=row["docLink"].ToString();
				}
				if(row["docPath"]!=null)
				{
					model.docPath=row["docPath"].ToString();
				}
				if(row["docType"]!=null)
				{
					model.docType=row["docType"].ToString();
				}
				if(row["docTypeNo"]!=null)
				{
					model.docTypeNo=row["docTypeNo"].ToString();
				}
				if(row["uploader"]!=null)
				{
					model.uploader=row["uploader"].ToString();
				}
				if(row["uploadDate"]!=null)
				{
					model.uploadDate=row["uploadDate"].ToString();
				}
				if(row["isValid"]!=null)
				{
					model.isValid=row["isValid"].ToString();
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
			strSql.Append("select id,docNo,docName,docExtensionName,docLink,docPath,docType,docTypeNo,uploader,uploadDate,isValid,createTime ");
			strSql.Append(" FROM commDocumentsInfo ");
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
			strSql.Append(" id,docNo,docName,docExtensionName,docLink,docPath,docType,docTypeNo,uploader,uploadDate,isValid,createTime ");
			strSql.Append(" FROM commDocumentsInfo ");
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
			strSql.Append("select count(1) FROM commDocumentsInfo ");
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
			strSql.Append(")AS Row, T.*  from commDocumentsInfo T ");
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
			parameters[0].Value = "commDocumentsInfo";
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

