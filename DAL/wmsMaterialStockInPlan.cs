using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:wmsMaterialStockInPlan
	/// </summary>
	public partial class wmsMaterialStockInPlan
	{
		public wmsMaterialStockInPlan()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "wmsMaterialStockInPlan"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from wmsMaterialStockInPlan");
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
		public int Add(Model.wmsMaterialStockInPlan model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into wmsMaterialStockInPlan(");
			strSql.Append("receiptNo,materialNo,materialName,needNum,supplyNo,supplyName,scanNum,restNum,status)");
			strSql.Append(" values (");
			strSql.Append("@receiptNo,@materialNo,@materialName,@needNum,@supplyNo,@supplyName,@scanNum,@restNum,@status)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@receiptNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialName", SqlDbType.NVarChar,50),
					new SqlParameter("@needNum", SqlDbType.Float,8),
					new SqlParameter("@supplyNo", SqlDbType.NVarChar,50),
					new SqlParameter("@supplyName", SqlDbType.NVarChar,200),
					new SqlParameter("@scanNum", SqlDbType.Float,8),
					new SqlParameter("@restNum", SqlDbType.Float,8),
					new SqlParameter("@status", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.receiptNo;
			parameters[1].Value = model.materialNo;
			parameters[2].Value = model.materialName;
			parameters[3].Value = model.needNum;
			parameters[4].Value = model.supplyNo;
			parameters[5].Value = model.supplyName;
			parameters[6].Value = model.scanNum;
			parameters[7].Value = model.restNum;
			parameters[8].Value = model.status;

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
		public bool Update(Model.wmsMaterialStockInPlan model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update wmsMaterialStockInPlan set ");
			strSql.Append("receiptNo=@receiptNo,");
			strSql.Append("materialNo=@materialNo,");
			strSql.Append("materialName=@materialName,");
			strSql.Append("needNum=@needNum,");
			strSql.Append("supplyNo=@supplyNo,");
			strSql.Append("supplyName=@supplyName,");
			strSql.Append("scanNum=@scanNum,");
			strSql.Append("restNum=@restNum,");
			strSql.Append("status=@status");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@receiptNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialName", SqlDbType.NVarChar,50),
					new SqlParameter("@needNum", SqlDbType.Float,8),
					new SqlParameter("@supplyNo", SqlDbType.NVarChar,50),
					new SqlParameter("@supplyName", SqlDbType.NVarChar,200),
					new SqlParameter("@scanNum", SqlDbType.Float,8),
					new SqlParameter("@restNum", SqlDbType.Float,8),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.receiptNo;
			parameters[1].Value = model.materialNo;
			parameters[2].Value = model.materialName;
			parameters[3].Value = model.needNum;
			parameters[4].Value = model.supplyNo;
			parameters[5].Value = model.supplyName;
			parameters[6].Value = model.scanNum;
			parameters[7].Value = model.restNum;
			parameters[8].Value = model.status;
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
			strSql.Append("delete from wmsMaterialStockInPlan ");
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
			strSql.Append("delete from wmsMaterialStockInPlan ");
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
		public Model.wmsMaterialStockInPlan GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,receiptNo,materialNo,materialName,needNum,supplyNo,supplyName,scanNum,restNum,status from wmsMaterialStockInPlan ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.wmsMaterialStockInPlan model=new Model.wmsMaterialStockInPlan();
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
		public Model.wmsMaterialStockInPlan DataRowToModel(DataRow row)
		{
			Model.wmsMaterialStockInPlan model=new Model.wmsMaterialStockInPlan();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["receiptNo"]!=null)
				{
					model.receiptNo=row["receiptNo"].ToString();
				}
				if(row["materialNo"]!=null)
				{
					model.materialNo=row["materialNo"].ToString();
				}
				if(row["materialName"]!=null)
				{
					model.materialName=row["materialName"].ToString();
				}
				if(row["needNum"]!=null && row["needNum"].ToString()!="")
				{
					model.needNum=decimal.Parse(row["needNum"].ToString());
				}
				if(row["supplyNo"]!=null)
				{
					model.supplyNo=row["supplyNo"].ToString();
				}
				if(row["supplyName"]!=null)
				{
					model.supplyName=row["supplyName"].ToString();
				}
				if(row["scanNum"]!=null && row["scanNum"].ToString()!="")
				{
					model.scanNum=decimal.Parse(row["scanNum"].ToString());
				}
				if(row["restNum"]!=null && row["restNum"].ToString()!="")
				{
					model.restNum=decimal.Parse(row["restNum"].ToString());
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
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
			strSql.Append("select id,receiptNo,materialNo,materialName,needNum,supplyNo,supplyName,scanNum,restNum,status ");
			strSql.Append(" FROM wmsMaterialStockInPlan ");
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
			strSql.Append(" id,receiptNo,materialNo,materialName,needNum,supplyNo,supplyName,scanNum,restNum,status ");
			strSql.Append(" FROM wmsMaterialStockInPlan ");
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
			strSql.Append("select count(1) FROM wmsMaterialStockInPlan ");
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
			strSql.Append(")AS Row, T.*  from wmsMaterialStockInPlan T ");
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
			parameters[0].Value = "wmsMaterialStockInPlan";
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

