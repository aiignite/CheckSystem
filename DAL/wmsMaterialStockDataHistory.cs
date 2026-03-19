using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:wmsMaterialStockDataHistory
	/// </summary>
	public partial class wmsMaterialStockDataHistory
	{
		public wmsMaterialStockDataHistory()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "wmsMaterialStockDataHistory"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from wmsMaterialStockDataHistory");
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
		public int Add(Model.wmsMaterialStockDataHistory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into wmsMaterialStockDataHistory(");
			strSql.Append("materialNo,supplierNo,boxNo,shelfNo,lotNo,count,note,barcode,stockInNo,stockInDate,stockInOperator,stockOutNo,stockOutDate,stockOutOperator,status,createTime,brightnessGroup,supplyLEDGroup)");
			strSql.Append(" values (");
			strSql.Append("@materialNo,@supplierNo,@boxNo,@shelfNo,@lotNo,@count,@note,@barcode,@stockInNo,@stockInDate,@stockInOperator,@stockOutNo,@stockOutDate,@stockOutOperator,@status,@createTime,@brightnessGroup,@supplyLEDGroup)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierNo", SqlDbType.NVarChar,50),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@lotNo", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.Int,4),
					new SqlParameter("@note", SqlDbType.NVarChar,50),
					new SqlParameter("@barcode", SqlDbType.NVarChar,50),
					new SqlParameter("@stockInNo", SqlDbType.NVarChar,50),
					new SqlParameter("@stockInDate", SqlDbType.DateTime),
					new SqlParameter("@stockInOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@stockOutNo", SqlDbType.NVarChar,50),
					new SqlParameter("@stockOutDate", SqlDbType.DateTime),
					new SqlParameter("@stockOutOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@supplyLEDGroup", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.materialNo;
			parameters[1].Value = model.supplierNo;
			parameters[2].Value = model.boxNo;
			parameters[3].Value = model.shelfNo;
			parameters[4].Value = model.lotNo;
			parameters[5].Value = model.count;
			parameters[6].Value = model.note;
			parameters[7].Value = model.barcode;
			parameters[8].Value = model.stockInNo;
			parameters[9].Value = model.stockInDate;
			parameters[10].Value = model.stockInOperator;
			parameters[11].Value = model.stockOutNo;
			parameters[12].Value = model.stockOutDate;
			parameters[13].Value = model.stockOutOperator;
			parameters[14].Value = model.status;
			parameters[15].Value = model.createTime;
			parameters[16].Value = model.brightnessGroup;
			parameters[17].Value = model.supplyLEDGroup;

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
		public bool Update(Model.wmsMaterialStockDataHistory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update wmsMaterialStockDataHistory set ");
			strSql.Append("materialNo=@materialNo,");
			strSql.Append("supplierNo=@supplierNo,");
			strSql.Append("boxNo=@boxNo,");
			strSql.Append("shelfNo=@shelfNo,");
			strSql.Append("lotNo=@lotNo,");
			strSql.Append("count=@count,");
			strSql.Append("note=@note,");
			strSql.Append("barcode=@barcode,");
			strSql.Append("stockInNo=@stockInNo,");
			strSql.Append("stockInDate=@stockInDate,");
			strSql.Append("stockInOperator=@stockInOperator,");
			strSql.Append("stockOutNo=@stockOutNo,");
			strSql.Append("stockOutDate=@stockOutDate,");
			strSql.Append("stockOutOperator=@stockOutOperator,");
			strSql.Append("status=@status,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("brightnessGroup=@brightnessGroup,");
			strSql.Append("supplyLEDGroup=@supplyLEDGroup");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierNo", SqlDbType.NVarChar,50),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@lotNo", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.Int,4),
					new SqlParameter("@note", SqlDbType.NVarChar,50),
					new SqlParameter("@barcode", SqlDbType.NVarChar,50),
					new SqlParameter("@stockInNo", SqlDbType.NVarChar,50),
					new SqlParameter("@stockInDate", SqlDbType.DateTime),
					new SqlParameter("@stockInOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@stockOutNo", SqlDbType.NVarChar,50),
					new SqlParameter("@stockOutDate", SqlDbType.DateTime),
					new SqlParameter("@stockOutOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@supplyLEDGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.materialNo;
			parameters[1].Value = model.supplierNo;
			parameters[2].Value = model.boxNo;
			parameters[3].Value = model.shelfNo;
			parameters[4].Value = model.lotNo;
			parameters[5].Value = model.count;
			parameters[6].Value = model.note;
			parameters[7].Value = model.barcode;
			parameters[8].Value = model.stockInNo;
			parameters[9].Value = model.stockInDate;
			parameters[10].Value = model.stockInOperator;
			parameters[11].Value = model.stockOutNo;
			parameters[12].Value = model.stockOutDate;
			parameters[13].Value = model.stockOutOperator;
			parameters[14].Value = model.status;
			parameters[15].Value = model.createTime;
			parameters[16].Value = model.brightnessGroup;
			parameters[17].Value = model.supplyLEDGroup;
			parameters[18].Value = model.id;

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
			strSql.Append("delete from wmsMaterialStockDataHistory ");
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
			strSql.Append("delete from wmsMaterialStockDataHistory ");
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
		public Model.wmsMaterialStockDataHistory GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,materialNo,supplierNo,boxNo,shelfNo,lotNo,count,note,barcode,stockInNo,stockInDate,stockInOperator,stockOutNo,stockOutDate,stockOutOperator,status,createTime,brightnessGroup,supplyLEDGroup from wmsMaterialStockDataHistory ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.wmsMaterialStockDataHistory model=new Model.wmsMaterialStockDataHistory();
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
		public Model.wmsMaterialStockDataHistory DataRowToModel(DataRow row)
		{
			Model.wmsMaterialStockDataHistory model=new Model.wmsMaterialStockDataHistory();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["materialNo"]!=null)
				{
					model.materialNo=row["materialNo"].ToString();
				}
				if(row["supplierNo"]!=null)
				{
					model.supplierNo=row["supplierNo"].ToString();
				}
				if(row["boxNo"]!=null)
				{
					model.boxNo=row["boxNo"].ToString();
				}
				if(row["shelfNo"]!=null)
				{
					model.shelfNo=row["shelfNo"].ToString();
				}
				if(row["lotNo"]!=null)
				{
					model.lotNo=row["lotNo"].ToString();
				}
				if(row["count"]!=null && row["count"].ToString()!="")
				{
					model.count=int.Parse(row["count"].ToString());
				}
				if(row["note"]!=null)
				{
					model.note=row["note"].ToString();
				}
				if(row["barcode"]!=null)
				{
					model.barcode=row["barcode"].ToString();
				}
				if(row["stockInNo"]!=null)
				{
					model.stockInNo=row["stockInNo"].ToString();
				}
				if(row["stockInDate"]!=null && row["stockInDate"].ToString()!="")
				{
					model.stockInDate=DateTime.Parse(row["stockInDate"].ToString());
				}
				if(row["stockInOperator"]!=null)
				{
					model.stockInOperator=row["stockInOperator"].ToString();
				}
				if(row["stockOutNo"]!=null)
				{
					model.stockOutNo=row["stockOutNo"].ToString();
				}
				if(row["stockOutDate"]!=null && row["stockOutDate"].ToString()!="")
				{
					model.stockOutDate=DateTime.Parse(row["stockOutDate"].ToString());
				}
				if(row["stockOutOperator"]!=null)
				{
					model.stockOutOperator=row["stockOutOperator"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["brightnessGroup"]!=null)
				{
					model.brightnessGroup=row["brightnessGroup"].ToString();
				}
				if(row["supplyLEDGroup"]!=null)
				{
					model.supplyLEDGroup=row["supplyLEDGroup"].ToString();
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
			strSql.Append("select id,materialNo,supplierNo,boxNo,shelfNo,lotNo,count,note,barcode,stockInNo,stockInDate,stockInOperator,stockOutNo,stockOutDate,stockOutOperator,status,createTime,brightnessGroup,supplyLEDGroup ");
			strSql.Append(" FROM wmsMaterialStockDataHistory ");
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
			strSql.Append(" id,materialNo,supplierNo,boxNo,shelfNo,lotNo,count,note,barcode,stockInNo,stockInDate,stockInOperator,stockOutNo,stockOutDate,stockOutOperator,status,createTime,brightnessGroup,supplyLEDGroup ");
			strSql.Append(" FROM wmsMaterialStockDataHistory ");
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
			strSql.Append("select count(1) FROM wmsMaterialStockDataHistory ");
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
			strSql.Append(")AS Row, T.*  from wmsMaterialStockDataHistory T ");
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
			parameters[0].Value = "wmsMaterialStockDataHistory";
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

