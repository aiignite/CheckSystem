using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:docInfo
	/// </summary>
	public partial class docInfo
	{
		public docInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "docInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from docInfo");
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
		public int Add(Model.docInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into docInfo(");
			strSql.Append("docNo,docName,docType,docChildType,productNo,productName,docVersion,docStatus,docContent,docNote,docWriter,writeFinishDate,docReviewer,reviewerDate,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@docNo,@docName,@docType,@docChildType,@productNo,@productName,@docVersion,@docStatus,@docContent,@docNote,@docWriter,@writeFinishDate,@docReviewer,@reviewerDate,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@docNo", SqlDbType.NVarChar,50),
					new SqlParameter("@docName", SqlDbType.NVarChar,50),
					new SqlParameter("@docType", SqlDbType.NVarChar,50),
					new SqlParameter("@docChildType", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,50),
					new SqlParameter("@docVersion", SqlDbType.NVarChar,50),
					new SqlParameter("@docStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@docContent", SqlDbType.Image),
					new SqlParameter("@docNote", SqlDbType.NVarChar,200),
					new SqlParameter("@docWriter", SqlDbType.NVarChar,50),
					new SqlParameter("@writeFinishDate", SqlDbType.NVarChar,50),
					new SqlParameter("@docReviewer", SqlDbType.NVarChar,50),
					new SqlParameter("@reviewerDate", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.docNo;
			parameters[1].Value = model.docName;
			parameters[2].Value = model.docType;
			parameters[3].Value = model.docChildType;
			parameters[4].Value = model.productNo;
			parameters[5].Value = model.productName;
			parameters[6].Value = model.docVersion;
			parameters[7].Value = model.docStatus;
			parameters[8].Value = model.docContent;
			parameters[9].Value = model.docNote;
			parameters[10].Value = model.docWriter;
			parameters[11].Value = model.writeFinishDate;
			parameters[12].Value = model.docReviewer;
			parameters[13].Value = model.reviewerDate;
			parameters[14].Value = model.creater;
			parameters[15].Value = model.createTime;

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
		public bool Update(Model.docInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update docInfo set ");
			strSql.Append("docNo=@docNo,");
			strSql.Append("docName=@docName,");
			strSql.Append("docType=@docType,");
			strSql.Append("docChildType=@docChildType,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("productName=@productName,");
			strSql.Append("docVersion=@docVersion,");
			strSql.Append("docStatus=@docStatus,");
			strSql.Append("docContent=@docContent,");
			strSql.Append("docNote=@docNote,");
			strSql.Append("docWriter=@docWriter,");
			strSql.Append("writeFinishDate=@writeFinishDate,");
			strSql.Append("docReviewer=@docReviewer,");
			strSql.Append("reviewerDate=@reviewerDate,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@docNo", SqlDbType.NVarChar,50),
					new SqlParameter("@docName", SqlDbType.NVarChar,50),
					new SqlParameter("@docType", SqlDbType.NVarChar,50),
					new SqlParameter("@docChildType", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,50),
					new SqlParameter("@docVersion", SqlDbType.NVarChar,50),
					new SqlParameter("@docStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@docContent", SqlDbType.Image),
					new SqlParameter("@docNote", SqlDbType.NVarChar,200),
					new SqlParameter("@docWriter", SqlDbType.NVarChar,50),
					new SqlParameter("@writeFinishDate", SqlDbType.NVarChar,50),
					new SqlParameter("@docReviewer", SqlDbType.NVarChar,50),
					new SqlParameter("@reviewerDate", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.docNo;
			parameters[1].Value = model.docName;
			parameters[2].Value = model.docType;
			parameters[3].Value = model.docChildType;
			parameters[4].Value = model.productNo;
			parameters[5].Value = model.productName;
			parameters[6].Value = model.docVersion;
			parameters[7].Value = model.docStatus;
			parameters[8].Value = model.docContent;
			parameters[9].Value = model.docNote;
			parameters[10].Value = model.docWriter;
			parameters[11].Value = model.writeFinishDate;
			parameters[12].Value = model.docReviewer;
			parameters[13].Value = model.reviewerDate;
			parameters[14].Value = model.creater;
			parameters[15].Value = model.createTime;
			parameters[16].Value = model.id;

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
			strSql.Append("delete from docInfo ");
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
			strSql.Append("delete from docInfo ");
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
		public Model.docInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,docNo,docName,docType,docChildType,productNo,productName,docVersion,docStatus,docContent,docNote,docWriter,writeFinishDate,docReviewer,reviewerDate,creater,createTime from docInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.docInfo model=new Model.docInfo();
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
		public Model.docInfo DataRowToModel(DataRow row)
		{
			Model.docInfo model=new Model.docInfo();
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
				if(row["docType"]!=null)
				{
					model.docType=row["docType"].ToString();
				}
				if(row["docChildType"]!=null)
				{
					model.docChildType=row["docChildType"].ToString();
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["productName"]!=null)
				{
					model.productName=row["productName"].ToString();
				}
				if(row["docVersion"]!=null)
				{
					model.docVersion=row["docVersion"].ToString();
				}
				if(row["docStatus"]!=null)
				{
					model.docStatus=row["docStatus"].ToString();
				}
				if(row["docContent"]!=null && row["docContent"].ToString()!="")
				{
					model.docContent=(byte[])row["docContent"];
				}
				if(row["docNote"]!=null)
				{
					model.docNote=row["docNote"].ToString();
				}
				if(row["docWriter"]!=null)
				{
					model.docWriter=row["docWriter"].ToString();
				}
				if(row["writeFinishDate"]!=null)
				{
					model.writeFinishDate=row["writeFinishDate"].ToString();
				}
				if(row["docReviewer"]!=null)
				{
					model.docReviewer=row["docReviewer"].ToString();
				}
				if(row["reviewerDate"]!=null)
				{
					model.reviewerDate=row["reviewerDate"].ToString();
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
			strSql.Append("select id,docNo,docName,docType,docChildType,productNo,productName,docVersion,docStatus,docContent,docNote,docWriter,writeFinishDate,docReviewer,reviewerDate,creater,createTime ");
			strSql.Append(" FROM docInfo ");
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
			strSql.Append(" id,docNo,docName,docType,docChildType,productNo,productName,docVersion,docStatus,docContent,docNote,docWriter,writeFinishDate,docReviewer,reviewerDate,creater,createTime ");
			strSql.Append(" FROM docInfo ");
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
			strSql.Append("select count(1) FROM docInfo ");
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
			strSql.Append(")AS Row, T.*  from docInfo T ");
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
			parameters[0].Value = "docInfo";
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

