using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:manufactureCheckDataHistory
	/// </summary>
	public partial class manufactureCheckDataHistory
	{
		public manufactureCheckDataHistory()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "manufactureCheckDataHistory"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from manufactureCheckDataHistory");
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
		public int Add(Model.manufactureCheckDataHistory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into manufactureCheckDataHistory(");
			strSql.Append("taskNo,productNo,productUid,pcbaNo,pcbaBarcode,productBarcode,packageBarcode,processNo,checkData,checkDate,checkStaffNo,checkResult,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@taskNo,@productNo,@productUid,@pcbaNo,@pcbaBarcode,@productBarcode,@packageBarcode,@processNo,@checkData,@checkDate,@checkStaffNo,@checkResult,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productUid", SqlDbType.NVarChar,50),
					new SqlParameter("@pcbaNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pcbaBarcode", SqlDbType.NVarChar,50),
					new SqlParameter("@productBarcode", SqlDbType.NVarChar,50),
					new SqlParameter("@packageBarcode", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@checkData", SqlDbType.NVarChar,500),
					new SqlParameter("@checkDate", SqlDbType.DateTime),
					new SqlParameter("@checkStaffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@checkResult", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.taskNo;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.productUid;
			parameters[3].Value = model.pcbaNo;
			parameters[4].Value = model.pcbaBarcode;
			parameters[5].Value = model.productBarcode;
			parameters[6].Value = model.packageBarcode;
			parameters[7].Value = model.processNo;
			parameters[8].Value = model.checkData;
			parameters[9].Value = model.checkDate;
			parameters[10].Value = model.checkStaffNo;
			parameters[11].Value = model.checkResult;
			parameters[12].Value = model.creater;
			parameters[13].Value = model.createTime;

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
		public bool Update(Model.manufactureCheckDataHistory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update manufactureCheckDataHistory set ");
			strSql.Append("taskNo=@taskNo,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("productUid=@productUid,");
			strSql.Append("pcbaNo=@pcbaNo,");
			strSql.Append("pcbaBarcode=@pcbaBarcode,");
			strSql.Append("productBarcode=@productBarcode,");
			strSql.Append("packageBarcode=@packageBarcode,");
			strSql.Append("processNo=@processNo,");
			strSql.Append("checkData=@checkData,");
			strSql.Append("checkDate=@checkDate,");
			strSql.Append("checkStaffNo=@checkStaffNo,");
			strSql.Append("checkResult=@checkResult,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productUid", SqlDbType.NVarChar,50),
					new SqlParameter("@pcbaNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pcbaBarcode", SqlDbType.NVarChar,50),
					new SqlParameter("@productBarcode", SqlDbType.NVarChar,50),
					new SqlParameter("@packageBarcode", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@checkData", SqlDbType.NVarChar,500),
					new SqlParameter("@checkDate", SqlDbType.DateTime),
					new SqlParameter("@checkStaffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@checkResult", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.taskNo;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.productUid;
			parameters[3].Value = model.pcbaNo;
			parameters[4].Value = model.pcbaBarcode;
			parameters[5].Value = model.productBarcode;
			parameters[6].Value = model.packageBarcode;
			parameters[7].Value = model.processNo;
			parameters[8].Value = model.checkData;
			parameters[9].Value = model.checkDate;
			parameters[10].Value = model.checkStaffNo;
			parameters[11].Value = model.checkResult;
			parameters[12].Value = model.creater;
			parameters[13].Value = model.createTime;
			parameters[14].Value = model.id;

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
			strSql.Append("delete from manufactureCheckDataHistory ");
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
			strSql.Append("delete from manufactureCheckDataHistory ");
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
		public Model.manufactureCheckDataHistory GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,taskNo,productNo,productUid,pcbaNo,pcbaBarcode,productBarcode,packageBarcode,processNo,checkData,checkDate,checkStaffNo,checkResult,creater,createTime from manufactureCheckDataHistory ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.manufactureCheckDataHistory model=new Model.manufactureCheckDataHistory();
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
		public Model.manufactureCheckDataHistory DataRowToModel(DataRow row)
		{
			Model.manufactureCheckDataHistory model=new Model.manufactureCheckDataHistory();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["taskNo"]!=null)
				{
					model.taskNo=row["taskNo"].ToString();
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["productUid"]!=null)
				{
					model.productUid=row["productUid"].ToString();
				}
				if(row["pcbaNo"]!=null)
				{
					model.pcbaNo=row["pcbaNo"].ToString();
				}
				if(row["pcbaBarcode"]!=null)
				{
					model.pcbaBarcode=row["pcbaBarcode"].ToString();
				}
				if(row["productBarcode"]!=null)
				{
					model.productBarcode=row["productBarcode"].ToString();
				}
				if(row["packageBarcode"]!=null)
				{
					model.packageBarcode=row["packageBarcode"].ToString();
				}
				if(row["processNo"]!=null)
				{
					model.processNo=row["processNo"].ToString();
				}
				if(row["checkData"]!=null)
				{
					model.checkData=row["checkData"].ToString();
				}
				if(row["checkDate"]!=null && row["checkDate"].ToString()!="")
				{
					model.checkDate=DateTime.Parse(row["checkDate"].ToString());
				}
				if(row["checkStaffNo"]!=null)
				{
					model.checkStaffNo=row["checkStaffNo"].ToString();
				}
				if(row["checkResult"]!=null)
				{
					model.checkResult=row["checkResult"].ToString();
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
			strSql.Append("select id,taskNo,productNo,productUid,pcbaNo,pcbaBarcode,productBarcode,packageBarcode,processNo,checkData,checkDate,checkStaffNo,checkResult,creater,createTime ");
			strSql.Append(" FROM manufactureCheckDataHistory ");
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
			strSql.Append(" id,taskNo,productNo,productUid,pcbaNo,pcbaBarcode,productBarcode,packageBarcode,processNo,checkData,checkDate,checkStaffNo,checkResult,creater,createTime ");
			strSql.Append(" FROM manufactureCheckDataHistory ");
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
			strSql.Append("select count(1) FROM manufactureCheckDataHistory ");
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
			strSql.Append(")AS Row, T.*  from manufactureCheckDataHistory T ");
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
			parameters[0].Value = "manufactureCheckDataHistory";
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

