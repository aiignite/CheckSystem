using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:wmsMaterialStockInDetail
	/// </summary>
	public partial class wmsMaterialStockInDetail
	{
		public wmsMaterialStockInDetail()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "wmsMaterialStockInDetail"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from wmsMaterialStockInDetail");
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
		public int Add(Model.wmsMaterialStockInDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into wmsMaterialStockInDetail(");
			strSql.Append("stockInNo,materialBarcode,materialNo,printNo,lotNo,count,boxNo,barcode,supplyNo,supplyLedGroup,companyGroup,note,stockInOperator,status,createTime,orderNo,supplyDate)");
			strSql.Append(" values (");
			strSql.Append("@stockInNo,@materialBarcode,@materialNo,@printNo,@lotNo,@count,@boxNo,@barcode,@supplyNo,@supplyLedGroup,@companyGroup,@note,@stockInOperator,@status,@createTime,@orderNo,@supplyDate)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@stockInNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialBarcode", SqlDbType.NVarChar,255),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@printNo", SqlDbType.NVarChar,100),
					new SqlParameter("@lotNo", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.Int,4),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,50),
					new SqlParameter("@barcode", SqlDbType.NVarChar,100),
					new SqlParameter("@supplyNo", SqlDbType.NVarChar,30),
					new SqlParameter("@supplyLedGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@companyGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@note", SqlDbType.NVarChar,50),
					new SqlParameter("@stockInOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@orderNo", SqlDbType.NVarChar,50),
					new SqlParameter("@supplyDate", SqlDbType.NVarChar,10)};
			parameters[0].Value = model.stockInNo;
			parameters[1].Value = model.materialBarcode;
			parameters[2].Value = model.materialNo;
			parameters[3].Value = model.printNo;
			parameters[4].Value = model.lotNo;
			parameters[5].Value = model.count;
			parameters[6].Value = model.boxNo;
			parameters[7].Value = model.barcode;
			parameters[8].Value = model.supplyNo;
			parameters[9].Value = model.supplyLedGroup;
			parameters[10].Value = model.companyGroup;
			parameters[11].Value = model.note;
			parameters[12].Value = model.stockInOperator;
			parameters[13].Value = model.status;
			parameters[14].Value = model.createTime;
			parameters[15].Value = model.orderNo;
			parameters[16].Value = model.supplyDate;

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
		public bool Update(Model.wmsMaterialStockInDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update wmsMaterialStockInDetail set ");
			strSql.Append("stockInNo=@stockInNo,");
			strSql.Append("materialBarcode=@materialBarcode,");
			strSql.Append("materialNo=@materialNo,");
			strSql.Append("printNo=@printNo,");
			strSql.Append("lotNo=@lotNo,");
			strSql.Append("count=@count,");
			strSql.Append("boxNo=@boxNo,");
			strSql.Append("barcode=@barcode,");
			strSql.Append("supplyNo=@supplyNo,");
			strSql.Append("supplyLedGroup=@supplyLedGroup,");
			strSql.Append("companyGroup=@companyGroup,");
			strSql.Append("note=@note,");
			strSql.Append("stockInOperator=@stockInOperator,");
			strSql.Append("status=@status,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("orderNo=@orderNo,");
			strSql.Append("supplyDate=@supplyDate");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@stockInNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialBarcode", SqlDbType.NVarChar,255),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@printNo", SqlDbType.NVarChar,100),
					new SqlParameter("@lotNo", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.Int,4),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,50),
					new SqlParameter("@barcode", SqlDbType.NVarChar,100),
					new SqlParameter("@supplyNo", SqlDbType.NVarChar,30),
					new SqlParameter("@supplyLedGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@companyGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@note", SqlDbType.NVarChar,50),
					new SqlParameter("@stockInOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@orderNo", SqlDbType.NVarChar,50),
					new SqlParameter("@supplyDate", SqlDbType.NVarChar,10),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.stockInNo;
			parameters[1].Value = model.materialBarcode;
			parameters[2].Value = model.materialNo;
			parameters[3].Value = model.printNo;
			parameters[4].Value = model.lotNo;
			parameters[5].Value = model.count;
			parameters[6].Value = model.boxNo;
			parameters[7].Value = model.barcode;
			parameters[8].Value = model.supplyNo;
			parameters[9].Value = model.supplyLedGroup;
			parameters[10].Value = model.companyGroup;
			parameters[11].Value = model.note;
			parameters[12].Value = model.stockInOperator;
			parameters[13].Value = model.status;
			parameters[14].Value = model.createTime;
			parameters[15].Value = model.orderNo;
			parameters[16].Value = model.supplyDate;
			parameters[17].Value = model.id;

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
			strSql.Append("delete from wmsMaterialStockInDetail ");
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
			strSql.Append("delete from wmsMaterialStockInDetail ");
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
		public Model.wmsMaterialStockInDetail GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,stockInNo,materialBarcode,materialNo,printNo,lotNo,count,boxNo,barcode,supplyNo,supplyLedGroup,companyGroup,note,stockInOperator,status,createTime,orderNo,supplyDate from wmsMaterialStockInDetail ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.wmsMaterialStockInDetail model=new Model.wmsMaterialStockInDetail();
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
		public Model.wmsMaterialStockInDetail DataRowToModel(DataRow row)
		{
			Model.wmsMaterialStockInDetail model=new Model.wmsMaterialStockInDetail();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["stockInNo"]!=null)
				{
					model.stockInNo=row["stockInNo"].ToString();
				}
				if(row["materialBarcode"]!=null)
				{
					model.materialBarcode=row["materialBarcode"].ToString();
				}
				if(row["materialNo"]!=null)
				{
					model.materialNo=row["materialNo"].ToString();
				}
				if(row["printNo"]!=null)
				{
					model.printNo=row["printNo"].ToString();
				}
				if(row["lotNo"]!=null)
				{
					model.lotNo=row["lotNo"].ToString();
				}
				if(row["count"]!=null && row["count"].ToString()!="")
				{
					model.count=int.Parse(row["count"].ToString());
				}
				if(row["boxNo"]!=null)
				{
					model.boxNo=row["boxNo"].ToString();
				}
				if(row["barcode"]!=null)
				{
					model.barcode=row["barcode"].ToString();
				}
				if(row["supplyNo"]!=null)
				{
					model.supplyNo=row["supplyNo"].ToString();
				}
				if(row["supplyLedGroup"]!=null)
				{
					model.supplyLedGroup=row["supplyLedGroup"].ToString();
				}
				if(row["companyGroup"]!=null)
				{
					model.companyGroup=row["companyGroup"].ToString();
				}
				if(row["note"]!=null)
				{
					model.note=row["note"].ToString();
				}
				if(row["stockInOperator"]!=null)
				{
					model.stockInOperator=row["stockInOperator"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["orderNo"]!=null)
				{
					model.orderNo=row["orderNo"].ToString();
				}
				if(row["supplyDate"]!=null)
				{
					model.supplyDate=row["supplyDate"].ToString();
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
			strSql.Append("select id,stockInNo,materialBarcode,materialNo,printNo,lotNo,count,boxNo,barcode,supplyNo,supplyLedGroup,companyGroup,note,stockInOperator,status,createTime,orderNo,supplyDate ");
			strSql.Append(" FROM wmsMaterialStockInDetail ");
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
			strSql.Append(" id,stockInNo,materialBarcode,materialNo,printNo,lotNo,count,boxNo,barcode,supplyNo,supplyLedGroup,companyGroup,note,stockInOperator,status,createTime,orderNo,supplyDate ");
			strSql.Append(" FROM wmsMaterialStockInDetail ");
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
			strSql.Append("select count(1) FROM wmsMaterialStockInDetail ");
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
			strSql.Append(")AS Row, T.*  from wmsMaterialStockInDetail T ");
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
			parameters[0].Value = "wmsMaterialStockInDetail";
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

