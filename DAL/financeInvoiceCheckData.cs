using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:financeInvoiceCheckData
	/// </summary>
	public partial class financeInvoiceCheckData
	{
		public financeInvoiceCheckData()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "financeInvoiceCheckData"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from financeInvoiceCheckData");
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
		public int Add(Model.financeInvoiceCheckData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into financeInvoiceCheckData(");
			strSql.Append("invoiceCode,invoiceNo,invoicingDate,invoiceScanValue,invoiceScan,creater,createTime,invoiceValue,checkcode,department,person,invoicetype,invoiceunit)");
			strSql.Append(" values (");
			strSql.Append("@invoiceCode,@invoiceNo,@invoicingDate,@invoiceScanValue,@invoiceScan,@creater,@createTime,@invoiceValue,@checkcode,@department,@person,@invoicetype,@invoiceunit)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@invoiceCode", SqlDbType.NVarChar,50),
					new SqlParameter("@invoiceNo", SqlDbType.NVarChar,50),
					new SqlParameter("@invoicingDate", SqlDbType.NVarChar,50),
					new SqlParameter("@invoiceScanValue", SqlDbType.NVarChar,50),
					new SqlParameter("@invoiceScan", SqlDbType.NVarChar,200),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@invoiceValue", SqlDbType.NVarChar,50),
					new SqlParameter("@checkcode", SqlDbType.NVarChar,50),
					new SqlParameter("@department", SqlDbType.NVarChar,50),
					new SqlParameter("@person", SqlDbType.NVarChar,50),
					new SqlParameter("@invoicetype", SqlDbType.NVarChar,50),
					new SqlParameter("@invoiceunit", SqlDbType.NVarChar,100)};
			parameters[0].Value = model.invoiceCode;
			parameters[1].Value = model.invoiceNo;
			parameters[2].Value = model.invoicingDate;
			parameters[3].Value = model.invoiceScanValue;
			parameters[4].Value = model.invoiceScan;
			parameters[5].Value = model.creater;
			parameters[6].Value = model.createTime;
			parameters[7].Value = model.invoiceValue;
			parameters[8].Value = model.checkcode;
			parameters[9].Value = model.department;
			parameters[10].Value = model.person;
			parameters[11].Value = model.invoicetype;
			parameters[12].Value = model.invoiceunit;

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
		public bool Update(Model.financeInvoiceCheckData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update financeInvoiceCheckData set ");
			strSql.Append("invoiceCode=@invoiceCode,");
			strSql.Append("invoiceNo=@invoiceNo,");
			strSql.Append("invoicingDate=@invoicingDate,");
			strSql.Append("invoiceScanValue=@invoiceScanValue,");
			strSql.Append("invoiceScan=@invoiceScan,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("invoiceValue=@invoiceValue,");
			strSql.Append("checkcode=@checkcode,");
			strSql.Append("department=@department,");
			strSql.Append("person=@person,");
			strSql.Append("invoicetype=@invoicetype,");
			strSql.Append("invoiceunit=@invoiceunit");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@invoiceCode", SqlDbType.NVarChar,50),
					new SqlParameter("@invoiceNo", SqlDbType.NVarChar,50),
					new SqlParameter("@invoicingDate", SqlDbType.NVarChar,50),
					new SqlParameter("@invoiceScanValue", SqlDbType.NVarChar,50),
					new SqlParameter("@invoiceScan", SqlDbType.NVarChar,200),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@invoiceValue", SqlDbType.NVarChar,50),
					new SqlParameter("@checkcode", SqlDbType.NVarChar,50),
					new SqlParameter("@department", SqlDbType.NVarChar,50),
					new SqlParameter("@person", SqlDbType.NVarChar,50),
					new SqlParameter("@invoicetype", SqlDbType.NVarChar,50),
					new SqlParameter("@invoiceunit", SqlDbType.NVarChar,100),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.invoiceCode;
			parameters[1].Value = model.invoiceNo;
			parameters[2].Value = model.invoicingDate;
			parameters[3].Value = model.invoiceScanValue;
			parameters[4].Value = model.invoiceScan;
			parameters[5].Value = model.creater;
			parameters[6].Value = model.createTime;
			parameters[7].Value = model.invoiceValue;
			parameters[8].Value = model.checkcode;
			parameters[9].Value = model.department;
			parameters[10].Value = model.person;
			parameters[11].Value = model.invoicetype;
			parameters[12].Value = model.invoiceunit;
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
			strSql.Append("delete from financeInvoiceCheckData ");
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
			strSql.Append("delete from financeInvoiceCheckData ");
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
		public Model.financeInvoiceCheckData GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,invoiceCode,invoiceNo,invoicingDate,invoiceScanValue,invoiceScan,creater,createTime,invoiceValue,checkcode,department,person,invoicetype,invoiceunit from financeInvoiceCheckData ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.financeInvoiceCheckData model=new Model.financeInvoiceCheckData();
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
		public Model.financeInvoiceCheckData DataRowToModel(DataRow row)
		{
			Model.financeInvoiceCheckData model=new Model.financeInvoiceCheckData();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["invoiceCode"]!=null)
				{
					model.invoiceCode=row["invoiceCode"].ToString();
				}
				if(row["invoiceNo"]!=null)
				{
					model.invoiceNo=row["invoiceNo"].ToString();
				}
				if(row["invoicingDate"]!=null)
				{
					model.invoicingDate=row["invoicingDate"].ToString();
				}
				if(row["invoiceScanValue"]!=null)
				{
					model.invoiceScanValue=row["invoiceScanValue"].ToString();
				}
				if(row["invoiceScan"]!=null)
				{
					model.invoiceScan=row["invoiceScan"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["invoiceValue"]!=null)
				{
					model.invoiceValue=row["invoiceValue"].ToString();
				}
				if(row["checkcode"]!=null)
				{
					model.checkcode=row["checkcode"].ToString();
				}
				if(row["department"]!=null)
				{
					model.department=row["department"].ToString();
				}
				if(row["person"]!=null)
				{
					model.person=row["person"].ToString();
				}
				if(row["invoicetype"]!=null)
				{
					model.invoicetype=row["invoicetype"].ToString();
				}
				if(row["invoiceunit"]!=null)
				{
					model.invoiceunit=row["invoiceunit"].ToString();
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
			strSql.Append("select id,invoiceCode,invoiceNo,invoicingDate,invoiceScanValue,invoiceScan,creater,createTime,invoiceValue,checkcode,department,person,invoicetype,invoiceunit ");
			strSql.Append(" FROM financeInvoiceCheckData ");
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
			strSql.Append(" id,invoiceCode,invoiceNo,invoicingDate,invoiceScanValue,invoiceScan,creater,createTime,invoiceValue,checkcode,department,person,invoicetype,invoiceunit ");
			strSql.Append(" FROM financeInvoiceCheckData ");
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
			strSql.Append("select count(1) FROM financeInvoiceCheckData ");
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
			strSql.Append(")AS Row, T.*  from financeInvoiceCheckData T ");
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
			parameters[0].Value = "financeInvoiceCheckData";
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

