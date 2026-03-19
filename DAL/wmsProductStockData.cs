using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:wmsProductStockData
	/// </summary>
	public partial class wmsProductStockData
	{
		public wmsProductStockData()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "wmsProductStockData"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string shelfNo,int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from wmsProductStockData");
			strSql.Append(" where shelfNo=@shelfNo and id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)			};
			parameters[0].Value = shelfNo;
			parameters[1].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(Model.wmsProductStockData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into wmsProductStockData(");
			strSql.Append("storeDataNo,productNo,productCount,outStoreCount,boxNo,trayNo,shelfNo,productInstoreNo,instoreDate,productOutstoreNo,outstoreTime,status,creatTime,skNo,brightnessGroup,boxType,specific,special,productName,batchNo,stockInOperator,stockOutOperator,onShelfOperator,offShelfOperator,outtrayNo,backCount,onShelfTime,offShelfTime,offShelfNo)");
			strSql.Append(" values (");
			strSql.Append("@storeDataNo,@productNo,@productCount,@outStoreCount,@boxNo,@trayNo,@shelfNo,@productInstoreNo,@instoreDate,@productOutstoreNo,@outstoreTime,@status,@creatTime,@skNo,@brightnessGroup,@boxType,@specific,@special,@productName,@batchNo,@stockInOperator,@stockOutOperator,@onShelfOperator,@offShelfOperator,@outtrayNo,@backCount,@onShelfTime,@offShelfTime,@offShelfNo)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@storeDataNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.Int,4),
					new SqlParameter("@outStoreCount", SqlDbType.Int,4),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,200),
					new SqlParameter("@trayNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productInstoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@instoreDate", SqlDbType.DateTime),
					new SqlParameter("@productOutstoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@outstoreTime", SqlDbType.DateTime),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@creatTime", SqlDbType.DateTime),
					new SqlParameter("@skNo", SqlDbType.NVarChar,50),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@boxType", SqlDbType.NVarChar,20),
					new SqlParameter("@specific", SqlDbType.NVarChar,30),
					new SqlParameter("@special", SqlDbType.NVarChar,30),
					new SqlParameter("@productName", SqlDbType.NVarChar,50),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,20),
					new SqlParameter("@stockInOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@stockOutOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@onShelfOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@offShelfOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@outtrayNo", SqlDbType.NVarChar,50),
					new SqlParameter("@backCount", SqlDbType.Int,4),
					new SqlParameter("@onShelfTime", SqlDbType.NVarChar,50),
					new SqlParameter("@offShelfTime", SqlDbType.NVarChar,50),
					new SqlParameter("@offShelfNo", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.storeDataNo;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.productCount;
			parameters[3].Value = model.outStoreCount;
			parameters[4].Value = model.boxNo;
			parameters[5].Value = model.trayNo;
			parameters[6].Value = model.shelfNo;
			parameters[7].Value = model.productInstoreNo;
			parameters[8].Value = model.instoreDate;
			parameters[9].Value = model.productOutstoreNo;
			parameters[10].Value = model.outstoreTime;
			parameters[11].Value = model.status;
			parameters[12].Value = model.creatTime;
			parameters[13].Value = model.skNo;
			parameters[14].Value = model.brightnessGroup;
			parameters[15].Value = model.boxType;
			parameters[16].Value = model.specific;
			parameters[17].Value = model.special;
			parameters[18].Value = model.productName;
			parameters[19].Value = model.batchNo;
			parameters[20].Value = model.stockInOperator;
			parameters[21].Value = model.stockOutOperator;
			parameters[22].Value = model.onShelfOperator;
			parameters[23].Value = model.offShelfOperator;
			parameters[24].Value = model.outtrayNo;
			parameters[25].Value = model.backCount;
			parameters[26].Value = model.onShelfTime;
			parameters[27].Value = model.offShelfTime;
			parameters[28].Value = model.offShelfNo;

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
		public bool Update(Model.wmsProductStockData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update wmsProductStockData set ");
			strSql.Append("storeDataNo=@storeDataNo,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("productCount=@productCount,");
			strSql.Append("outStoreCount=@outStoreCount,");
			strSql.Append("boxNo=@boxNo,");
			strSql.Append("trayNo=@trayNo,");
			strSql.Append("productInstoreNo=@productInstoreNo,");
			strSql.Append("instoreDate=@instoreDate,");
			strSql.Append("productOutstoreNo=@productOutstoreNo,");
			strSql.Append("outstoreTime=@outstoreTime,");
			strSql.Append("status=@status,");
			strSql.Append("creatTime=@creatTime,");
			strSql.Append("skNo=@skNo,");
			strSql.Append("brightnessGroup=@brightnessGroup,");
			strSql.Append("boxType=@boxType,");
			strSql.Append("specific=@specific,");
			strSql.Append("special=@special,");
			strSql.Append("productName=@productName,");
			strSql.Append("batchNo=@batchNo,");
			strSql.Append("stockInOperator=@stockInOperator,");
			strSql.Append("stockOutOperator=@stockOutOperator,");
			strSql.Append("onShelfOperator=@onShelfOperator,");
			strSql.Append("offShelfOperator=@offShelfOperator,");
			strSql.Append("outtrayNo=@outtrayNo,");
			strSql.Append("backCount=@backCount,");
			strSql.Append("onShelfTime=@onShelfTime,");
			strSql.Append("offShelfTime=@offShelfTime,");
			strSql.Append("offShelfNo=@offShelfNo");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@storeDataNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.Int,4),
					new SqlParameter("@outStoreCount", SqlDbType.Int,4),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,200),
					new SqlParameter("@trayNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productInstoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@instoreDate", SqlDbType.DateTime),
					new SqlParameter("@productOutstoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@outstoreTime", SqlDbType.DateTime),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@creatTime", SqlDbType.DateTime),
					new SqlParameter("@skNo", SqlDbType.NVarChar,50),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@boxType", SqlDbType.NVarChar,20),
					new SqlParameter("@specific", SqlDbType.NVarChar,30),
					new SqlParameter("@special", SqlDbType.NVarChar,30),
					new SqlParameter("@productName", SqlDbType.NVarChar,50),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,20),
					new SqlParameter("@stockInOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@stockOutOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@onShelfOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@offShelfOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@outtrayNo", SqlDbType.NVarChar,50),
					new SqlParameter("@backCount", SqlDbType.Int,4),
					new SqlParameter("@onShelfTime", SqlDbType.NVarChar,50),
					new SqlParameter("@offShelfTime", SqlDbType.NVarChar,50),
					new SqlParameter("@offShelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.storeDataNo;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.productCount;
			parameters[3].Value = model.outStoreCount;
			parameters[4].Value = model.boxNo;
			parameters[5].Value = model.trayNo;
			parameters[6].Value = model.productInstoreNo;
			parameters[7].Value = model.instoreDate;
			parameters[8].Value = model.productOutstoreNo;
			parameters[9].Value = model.outstoreTime;
			parameters[10].Value = model.status;
			parameters[11].Value = model.creatTime;
			parameters[12].Value = model.skNo;
			parameters[13].Value = model.brightnessGroup;
			parameters[14].Value = model.boxType;
			parameters[15].Value = model.specific;
			parameters[16].Value = model.special;
			parameters[17].Value = model.productName;
			parameters[18].Value = model.batchNo;
			parameters[19].Value = model.stockInOperator;
			parameters[20].Value = model.stockOutOperator;
			parameters[21].Value = model.onShelfOperator;
			parameters[22].Value = model.offShelfOperator;
			parameters[23].Value = model.outtrayNo;
			parameters[24].Value = model.backCount;
			parameters[25].Value = model.onShelfTime;
			parameters[26].Value = model.offShelfTime;
			parameters[27].Value = model.offShelfNo;
			parameters[28].Value = model.id;
			parameters[29].Value = model.shelfNo;

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
			strSql.Append("delete from wmsProductStockData ");
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
		/// 删除一条数据
		/// </summary>
		public bool Delete(string shelfNo,int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from wmsProductStockData ");
			strSql.Append(" where shelfNo=@shelfNo and id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)			};
			parameters[0].Value = shelfNo;
			parameters[1].Value = id;

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
			strSql.Append("delete from wmsProductStockData ");
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
		public Model.wmsProductStockData GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,storeDataNo,productNo,productCount,outStoreCount,boxNo,trayNo,shelfNo,productInstoreNo,instoreDate,productOutstoreNo,outstoreTime,status,creatTime,skNo,brightnessGroup,boxType,specific,special,productName,batchNo,stockInOperator,stockOutOperator,onShelfOperator,offShelfOperator,outtrayNo,backCount,onShelfTime,offShelfTime,offShelfNo from wmsProductStockData ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.wmsProductStockData model=new Model.wmsProductStockData();
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
		public Model.wmsProductStockData DataRowToModel(DataRow row)
		{
			Model.wmsProductStockData model=new Model.wmsProductStockData();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["storeDataNo"]!=null)
				{
					model.storeDataNo=row["storeDataNo"].ToString();
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["productCount"]!=null && row["productCount"].ToString()!="")
				{
					model.productCount=int.Parse(row["productCount"].ToString());
				}
				if(row["outStoreCount"]!=null && row["outStoreCount"].ToString()!="")
				{
					model.outStoreCount=int.Parse(row["outStoreCount"].ToString());
				}
				if(row["boxNo"]!=null)
				{
					model.boxNo=row["boxNo"].ToString();
				}
				if(row["trayNo"]!=null)
				{
					model.trayNo=row["trayNo"].ToString();
				}
				if(row["shelfNo"]!=null)
				{
					model.shelfNo=row["shelfNo"].ToString();
				}
				if(row["productInstoreNo"]!=null)
				{
					model.productInstoreNo=row["productInstoreNo"].ToString();
				}
				if(row["instoreDate"]!=null && row["instoreDate"].ToString()!="")
				{
					model.instoreDate=DateTime.Parse(row["instoreDate"].ToString());
				}
				if(row["productOutstoreNo"]!=null)
				{
					model.productOutstoreNo=row["productOutstoreNo"].ToString();
				}
				if(row["outstoreTime"]!=null && row["outstoreTime"].ToString()!="")
				{
					model.outstoreTime=DateTime.Parse(row["outstoreTime"].ToString());
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["creatTime"]!=null && row["creatTime"].ToString()!="")
				{
					model.creatTime=DateTime.Parse(row["creatTime"].ToString());
				}
				if(row["skNo"]!=null)
				{
					model.skNo=row["skNo"].ToString();
				}
				if(row["brightnessGroup"]!=null)
				{
					model.brightnessGroup=row["brightnessGroup"].ToString();
				}
				if(row["boxType"]!=null)
				{
					model.boxType=row["boxType"].ToString();
				}
				if(row["specific"]!=null)
				{
					model.specific=row["specific"].ToString();
				}
				if(row["special"]!=null)
				{
					model.special=row["special"].ToString();
				}
				if(row["productName"]!=null)
				{
					model.productName=row["productName"].ToString();
				}
				if(row["batchNo"]!=null)
				{
					model.batchNo=row["batchNo"].ToString();
				}
				if(row["stockInOperator"]!=null)
				{
					model.stockInOperator=row["stockInOperator"].ToString();
				}
				if(row["stockOutOperator"]!=null)
				{
					model.stockOutOperator=row["stockOutOperator"].ToString();
				}
				if(row["onShelfOperator"]!=null)
				{
					model.onShelfOperator=row["onShelfOperator"].ToString();
				}
				if(row["offShelfOperator"]!=null)
				{
					model.offShelfOperator=row["offShelfOperator"].ToString();
				}
				if(row["outtrayNo"]!=null)
				{
					model.outtrayNo=row["outtrayNo"].ToString();
				}
				if(row["backCount"]!=null && row["backCount"].ToString()!="")
				{
					model.backCount=int.Parse(row["backCount"].ToString());
				}
				if(row["onShelfTime"]!=null)
				{
					model.onShelfTime=row["onShelfTime"].ToString();
				}
				if(row["offShelfTime"]!=null)
				{
					model.offShelfTime=row["offShelfTime"].ToString();
				}
				if(row["offShelfNo"]!=null)
				{
					model.offShelfNo=row["offShelfNo"].ToString();
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
			strSql.Append("select id,storeDataNo,productNo,productCount,outStoreCount,boxNo,trayNo,shelfNo,productInstoreNo,instoreDate,productOutstoreNo,outstoreTime,status,creatTime,skNo,brightnessGroup,boxType,specific,special,productName,batchNo,stockInOperator,stockOutOperator,onShelfOperator,offShelfOperator,outtrayNo,backCount,onShelfTime,offShelfTime,offShelfNo ");
			strSql.Append(" FROM wmsProductStockData ");
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
			strSql.Append(" id,storeDataNo,productNo,productCount,outStoreCount,boxNo,trayNo,shelfNo,productInstoreNo,instoreDate,productOutstoreNo,outstoreTime,status,creatTime,skNo,brightnessGroup,boxType,specific,special,productName,batchNo,stockInOperator,stockOutOperator,onShelfOperator,offShelfOperator,outtrayNo,backCount,onShelfTime,offShelfTime,offShelfNo ");
			strSql.Append(" FROM wmsProductStockData ");
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
			strSql.Append("select count(1) FROM wmsProductStockData ");
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
			strSql.Append(")AS Row, T.*  from wmsProductStockData T ");
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
			parameters[0].Value = "wmsProductStockData";
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

