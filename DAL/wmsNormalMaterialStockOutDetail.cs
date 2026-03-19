using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:wmsNormalMaterialStockOutDetail
	/// </summary>
	public partial class wmsNormalMaterialStockOutDetail
	{
		public wmsNormalMaterialStockOutDetail()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "wmsNormalMaterialStockOutDetail"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from wmsNormalMaterialStockOutDetail");
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
		public int Add(Model.wmsNormalMaterialStockOutDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into wmsNormalMaterialStockOutDetail(");
			strSql.Append("productOutStoreNo,productNo,productCount,outstoreCount,barcode,trayNo,shelfNo,status,customerNo,brightnessGroup,batchNo,creater,createTime,outStoreOperator,outStoreDate,acceptCustomer,priority)");
			strSql.Append(" values (");
			strSql.Append("@productOutStoreNo,@productNo,@productCount,@outstoreCount,@barcode,@trayNo,@shelfNo,@status,@customerNo,@brightnessGroup,@batchNo,@creater,@createTime,@outStoreOperator,@outStoreDate,@acceptCustomer,@priority)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@productOutStoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.Int,4),
					new SqlParameter("@outstoreCount", SqlDbType.Int,4),
					new SqlParameter("@barcode", SqlDbType.NVarChar,100),
					new SqlParameter("@trayNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@customerNo", SqlDbType.NVarChar,50),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@outStoreOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@outStoreDate", SqlDbType.DateTime),
					new SqlParameter("@acceptCustomer", SqlDbType.NVarChar,50),
					new SqlParameter("@priority", SqlDbType.NVarChar,10)};
			parameters[0].Value = model.productOutStoreNo;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.productCount;
			parameters[3].Value = model.outstoreCount;
			parameters[4].Value = model.barcode;
			parameters[5].Value = model.trayNo;
			parameters[6].Value = model.shelfNo;
			parameters[7].Value = model.status;
			parameters[8].Value = model.customerNo;
			parameters[9].Value = model.brightnessGroup;
			parameters[10].Value = model.batchNo;
			parameters[11].Value = model.creater;
			parameters[12].Value = model.createTime;
			parameters[13].Value = model.outStoreOperator;
			parameters[14].Value = model.outStoreDate;
			parameters[15].Value = model.acceptCustomer;
			parameters[16].Value = model.priority;

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
		public bool Update(Model.wmsNormalMaterialStockOutDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update wmsNormalMaterialStockOutDetail set ");
			strSql.Append("productOutStoreNo=@productOutStoreNo,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("productCount=@productCount,");
			strSql.Append("outstoreCount=@outstoreCount,");
			strSql.Append("barcode=@barcode,");
			strSql.Append("trayNo=@trayNo,");
			strSql.Append("shelfNo=@shelfNo,");
			strSql.Append("status=@status,");
			strSql.Append("customerNo=@customerNo,");
			strSql.Append("brightnessGroup=@brightnessGroup,");
			strSql.Append("batchNo=@batchNo,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("outStoreOperator=@outStoreOperator,");
			strSql.Append("outStoreDate=@outStoreDate,");
			strSql.Append("acceptCustomer=@acceptCustomer,");
			strSql.Append("priority=@priority");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@productOutStoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.Int,4),
					new SqlParameter("@outstoreCount", SqlDbType.Int,4),
					new SqlParameter("@barcode", SqlDbType.NVarChar,100),
					new SqlParameter("@trayNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@customerNo", SqlDbType.NVarChar,50),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@outStoreOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@outStoreDate", SqlDbType.DateTime),
					new SqlParameter("@acceptCustomer", SqlDbType.NVarChar,50),
					new SqlParameter("@priority", SqlDbType.NVarChar,10),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.productOutStoreNo;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.productCount;
			parameters[3].Value = model.outstoreCount;
			parameters[4].Value = model.barcode;
			parameters[5].Value = model.trayNo;
			parameters[6].Value = model.shelfNo;
			parameters[7].Value = model.status;
			parameters[8].Value = model.customerNo;
			parameters[9].Value = model.brightnessGroup;
			parameters[10].Value = model.batchNo;
			parameters[11].Value = model.creater;
			parameters[12].Value = model.createTime;
			parameters[13].Value = model.outStoreOperator;
			parameters[14].Value = model.outStoreDate;
			parameters[15].Value = model.acceptCustomer;
			parameters[16].Value = model.priority;
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
			strSql.Append("delete from wmsNormalMaterialStockOutDetail ");
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
			strSql.Append("delete from wmsNormalMaterialStockOutDetail ");
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
		public Model.wmsNormalMaterialStockOutDetail GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,productOutStoreNo,productNo,productCount,outstoreCount,barcode,trayNo,shelfNo,status,customerNo,brightnessGroup,batchNo,creater,createTime,outStoreOperator,outStoreDate,acceptCustomer,priority from wmsNormalMaterialStockOutDetail ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.wmsNormalMaterialStockOutDetail model=new Model.wmsNormalMaterialStockOutDetail();
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
		public Model.wmsNormalMaterialStockOutDetail DataRowToModel(DataRow row)
		{
			Model.wmsNormalMaterialStockOutDetail model=new Model.wmsNormalMaterialStockOutDetail();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["productOutStoreNo"]!=null)
				{
					model.productOutStoreNo=row["productOutStoreNo"].ToString();
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["productCount"]!=null && row["productCount"].ToString()!="")
				{
					model.productCount=int.Parse(row["productCount"].ToString());
				}
				if(row["outstoreCount"]!=null && row["outstoreCount"].ToString()!="")
				{
					model.outstoreCount=int.Parse(row["outstoreCount"].ToString());
				}
				if(row["barcode"]!=null)
				{
					model.barcode=row["barcode"].ToString();
				}
				if(row["trayNo"]!=null)
				{
					model.trayNo=row["trayNo"].ToString();
				}
				if(row["shelfNo"]!=null)
				{
					model.shelfNo=row["shelfNo"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["customerNo"]!=null)
				{
					model.customerNo=row["customerNo"].ToString();
				}
				if(row["brightnessGroup"]!=null)
				{
					model.brightnessGroup=row["brightnessGroup"].ToString();
				}
				if(row["batchNo"]!=null)
				{
					model.batchNo=row["batchNo"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["outStoreOperator"]!=null)
				{
					model.outStoreOperator=row["outStoreOperator"].ToString();
				}
				if(row["outStoreDate"]!=null && row["outStoreDate"].ToString()!="")
				{
					model.outStoreDate=DateTime.Parse(row["outStoreDate"].ToString());
				}
				if(row["acceptCustomer"]!=null)
				{
					model.acceptCustomer=row["acceptCustomer"].ToString();
				}
				if(row["priority"]!=null)
				{
					model.priority=row["priority"].ToString();
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
			strSql.Append("select id,productOutStoreNo,productNo,productCount,outstoreCount,barcode,trayNo,shelfNo,status,customerNo,brightnessGroup,batchNo,creater,createTime,outStoreOperator,outStoreDate,acceptCustomer,priority ");
			strSql.Append(" FROM wmsNormalMaterialStockOutDetail ");
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
			strSql.Append(" id,productOutStoreNo,productNo,productCount,outstoreCount,barcode,trayNo,shelfNo,status,customerNo,brightnessGroup,batchNo,creater,createTime,outStoreOperator,outStoreDate,acceptCustomer,priority ");
			strSql.Append(" FROM wmsNormalMaterialStockOutDetail ");
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
			strSql.Append("select count(1) FROM wmsNormalMaterialStockOutDetail ");
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
			strSql.Append(")AS Row, T.*  from wmsNormalMaterialStockOutDetail T ");
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
			parameters[0].Value = "wmsNormalMaterialStockOutDetail";
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

