using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:productSampleInfo
	/// </summary>
	public partial class productSampleInfo
	{
		public productSampleInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "productSampleInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from productSampleInfo");
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
		public int Add(Model.productSampleInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into productSampleInfo(");
			strSql.Append("sampleNo,sampleName,sampleType,productNo,processNo,isValid,versionNo,sampleStatus,note,creater,createTime,productNo2,productMinPackageNum)");
			strSql.Append(" values (");
			strSql.Append("@sampleNo,@sampleName,@sampleType,@productNo,@processNo,@isValid,@versionNo,@sampleStatus,@note,@creater,@createTime,@productNo2,@productMinPackageNum)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@sampleNo", SqlDbType.NVarChar,50),
					new SqlParameter("@sampleName", SqlDbType.NVarChar,50),
					new SqlParameter("@sampleType", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@isValid", SqlDbType.NVarChar,10),
					new SqlParameter("@versionNo", SqlDbType.NVarChar,50),
					new SqlParameter("@sampleStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@note", SqlDbType.NVarChar,200),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@productNo2", SqlDbType.NVarChar,50),
					new SqlParameter("@productMinPackageNum", SqlDbType.Int,4)};
			parameters[0].Value = model.sampleNo;
			parameters[1].Value = model.sampleName;
			parameters[2].Value = model.sampleType;
			parameters[3].Value = model.productNo;
			parameters[4].Value = model.processNo;
			parameters[5].Value = model.isValid;
			parameters[6].Value = model.versionNo;
			parameters[7].Value = model.sampleStatus;
			parameters[8].Value = model.note;
			parameters[9].Value = model.creater;
			parameters[10].Value = model.createTime;
			parameters[11].Value = model.productNo2;
			parameters[12].Value = model.productMinPackageNum;

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
		public bool Update(Model.productSampleInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update productSampleInfo set ");
			strSql.Append("sampleNo=@sampleNo,");
			strSql.Append("sampleName=@sampleName,");
			strSql.Append("sampleType=@sampleType,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("processNo=@processNo,");
			strSql.Append("isValid=@isValid,");
			strSql.Append("versionNo=@versionNo,");
			strSql.Append("sampleStatus=@sampleStatus,");
			strSql.Append("note=@note,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("productNo2=@productNo2,");
			strSql.Append("productMinPackageNum=@productMinPackageNum");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@sampleNo", SqlDbType.NVarChar,50),
					new SqlParameter("@sampleName", SqlDbType.NVarChar,50),
					new SqlParameter("@sampleType", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@isValid", SqlDbType.NVarChar,10),
					new SqlParameter("@versionNo", SqlDbType.NVarChar,50),
					new SqlParameter("@sampleStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@note", SqlDbType.NVarChar,200),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@productNo2", SqlDbType.NVarChar,50),
					new SqlParameter("@productMinPackageNum", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.sampleNo;
			parameters[1].Value = model.sampleName;
			parameters[2].Value = model.sampleType;
			parameters[3].Value = model.productNo;
			parameters[4].Value = model.processNo;
			parameters[5].Value = model.isValid;
			parameters[6].Value = model.versionNo;
			parameters[7].Value = model.sampleStatus;
			parameters[8].Value = model.note;
			parameters[9].Value = model.creater;
			parameters[10].Value = model.createTime;
			parameters[11].Value = model.productNo2;
			parameters[12].Value = model.productMinPackageNum;
			parameters[13].Value = model.id;

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
			strSql.Append("delete from productSampleInfo ");
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
			strSql.Append("delete from productSampleInfo ");
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
		public Model.productSampleInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,sampleNo,sampleName,sampleType,productNo,processNo,isValid,versionNo,sampleStatus,note,creater,createTime,productNo2,productMinPackageNum from productSampleInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.productSampleInfo model=new Model.productSampleInfo();
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
		public Model.productSampleInfo DataRowToModel(DataRow row)
		{
			Model.productSampleInfo model=new Model.productSampleInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["sampleNo"]!=null)
				{
					model.sampleNo=row["sampleNo"].ToString();
				}
				if(row["sampleName"]!=null)
				{
					model.sampleName=row["sampleName"].ToString();
				}
				if(row["sampleType"]!=null)
				{
					model.sampleType=row["sampleType"].ToString();
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["processNo"]!=null)
				{
					model.processNo=row["processNo"].ToString();
				}
				if(row["isValid"]!=null)
				{
					model.isValid=row["isValid"].ToString();
				}
				if(row["versionNo"]!=null)
				{
					model.versionNo=row["versionNo"].ToString();
				}
				if(row["sampleStatus"]!=null)
				{
					model.sampleStatus=row["sampleStatus"].ToString();
				}
				if(row["note"]!=null)
				{
					model.note=row["note"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["productNo2"]!=null)
				{
					model.productNo2=row["productNo2"].ToString();
				}
				if(row["productMinPackageNum"]!=null && row["productMinPackageNum"].ToString()!="")
				{
					model.productMinPackageNum=int.Parse(row["productMinPackageNum"].ToString());
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
			strSql.Append("select id,sampleNo,sampleName,sampleType,productNo,processNo,isValid,versionNo,sampleStatus,note,creater,createTime,productNo2,productMinPackageNum ");
			strSql.Append(" FROM productSampleInfo ");
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
			strSql.Append(" id,sampleNo,sampleName,sampleType,productNo,processNo,isValid,versionNo,sampleStatus,note,creater,createTime,productNo2,productMinPackageNum ");
			strSql.Append(" FROM productSampleInfo ");
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
			strSql.Append("select count(1) FROM productSampleInfo ");
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
			strSql.Append(")AS Row, T.*  from productSampleInfo T ");
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
			parameters[0].Value = "productSampleInfo";
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

