using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:wmsProductOutStoreDetail
	/// </summary>
	public partial class wmsProductOutStoreDetail
	{
		public wmsProductOutStoreDetail()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "wmsProductOutStoreDetail"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from wmsProductOutStoreDetail");
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
		public int Add(Model.wmsProductOutStoreDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into wmsProductOutStoreDetail(");
			strSql.Append("outStoreDetailNo,productOutStoreNo,productNo,productCount,outstoreCount,boxNo,trayNo,shelfNo,status,skNo,brightnessGroup,boxType,specific,special,productName,creater,createTime,offShelfOperator,offShelfTime,outStoreOperator,outStoreTime,batchNo,acceptCustomer,priority)");
			strSql.Append(" values (");
			strSql.Append("@outStoreDetailNo,@productOutStoreNo,@productNo,@productCount,@outstoreCount,@boxNo,@trayNo,@shelfNo,@status,@skNo,@brightnessGroup,@boxType,@specific,@special,@productName,@creater,@createTime,@offShelfOperator,@offShelfTime,@outStoreOperator,@outStoreTime,@batchNo,@acceptCustomer,@priority)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@outStoreDetailNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productOutStoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.Int,4),
					new SqlParameter("@outstoreCount", SqlDbType.Int,4),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,200),
					new SqlParameter("@trayNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@skNo", SqlDbType.NVarChar,50),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@boxType", SqlDbType.NVarChar,50),
					new SqlParameter("@specific", SqlDbType.NVarChar,30),
					new SqlParameter("@special", SqlDbType.NVarChar,30),
					new SqlParameter("@productName", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@offShelfOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@offShelfTime", SqlDbType.DateTime),
					new SqlParameter("@outStoreOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@outStoreTime", SqlDbType.DateTime),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,50),
					new SqlParameter("@acceptCustomer", SqlDbType.NVarChar,50),
					new SqlParameter("@priority", SqlDbType.NVarChar,10)};
			parameters[0].Value = model.outStoreDetailNo;
			parameters[1].Value = model.productOutStoreNo;
			parameters[2].Value = model.productNo;
			parameters[3].Value = model.productCount;
			parameters[4].Value = model.outstoreCount;
			parameters[5].Value = model.boxNo;
			parameters[6].Value = model.trayNo;
			parameters[7].Value = model.shelfNo;
			parameters[8].Value = model.status;
			parameters[9].Value = model.skNo;
			parameters[10].Value = model.brightnessGroup;
			parameters[11].Value = model.boxType;
			parameters[12].Value = model.specific;
			parameters[13].Value = model.special;
			parameters[14].Value = model.productName;
			parameters[15].Value = model.creater;
			parameters[16].Value = model.createTime;
			parameters[17].Value = model.offShelfOperator;
			parameters[18].Value = model.offShelfTime;
			parameters[19].Value = model.outStoreOperator;
			parameters[20].Value = model.outStoreTime;
			parameters[21].Value = model.batchNo;
			parameters[22].Value = model.acceptCustomer;
			parameters[23].Value = model.priority;

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
		public bool Update(Model.wmsProductOutStoreDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update wmsProductOutStoreDetail set ");
			strSql.Append("outStoreDetailNo=@outStoreDetailNo,");
			strSql.Append("productOutStoreNo=@productOutStoreNo,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("productCount=@productCount,");
			strSql.Append("outstoreCount=@outstoreCount,");
			strSql.Append("boxNo=@boxNo,");
			strSql.Append("trayNo=@trayNo,");
			strSql.Append("shelfNo=@shelfNo,");
			strSql.Append("status=@status,");
			strSql.Append("skNo=@skNo,");
			strSql.Append("brightnessGroup=@brightnessGroup,");
			strSql.Append("boxType=@boxType,");
			strSql.Append("specific=@specific,");
			strSql.Append("special=@special,");
			strSql.Append("productName=@productName,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("offShelfOperator=@offShelfOperator,");
			strSql.Append("offShelfTime=@offShelfTime,");
			strSql.Append("outStoreOperator=@outStoreOperator,");
			strSql.Append("outStoreTime=@outStoreTime,");
			strSql.Append("batchNo=@batchNo,");
			strSql.Append("acceptCustomer=@acceptCustomer,");
			strSql.Append("priority=@priority");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@outStoreDetailNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productOutStoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.Int,4),
					new SqlParameter("@outstoreCount", SqlDbType.Int,4),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,200),
					new SqlParameter("@trayNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@skNo", SqlDbType.NVarChar,50),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@boxType", SqlDbType.NVarChar,50),
					new SqlParameter("@specific", SqlDbType.NVarChar,30),
					new SqlParameter("@special", SqlDbType.NVarChar,30),
					new SqlParameter("@productName", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@offShelfOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@offShelfTime", SqlDbType.DateTime),
					new SqlParameter("@outStoreOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@outStoreTime", SqlDbType.DateTime),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,50),
					new SqlParameter("@acceptCustomer", SqlDbType.NVarChar,50),
					new SqlParameter("@priority", SqlDbType.NVarChar,10),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.outStoreDetailNo;
			parameters[1].Value = model.productOutStoreNo;
			parameters[2].Value = model.productNo;
			parameters[3].Value = model.productCount;
			parameters[4].Value = model.outstoreCount;
			parameters[5].Value = model.boxNo;
			parameters[6].Value = model.trayNo;
			parameters[7].Value = model.shelfNo;
			parameters[8].Value = model.status;
			parameters[9].Value = model.skNo;
			parameters[10].Value = model.brightnessGroup;
			parameters[11].Value = model.boxType;
			parameters[12].Value = model.specific;
			parameters[13].Value = model.special;
			parameters[14].Value = model.productName;
			parameters[15].Value = model.creater;
			parameters[16].Value = model.createTime;
			parameters[17].Value = model.offShelfOperator;
			parameters[18].Value = model.offShelfTime;
			parameters[19].Value = model.outStoreOperator;
			parameters[20].Value = model.outStoreTime;
			parameters[21].Value = model.batchNo;
			parameters[22].Value = model.acceptCustomer;
			parameters[23].Value = model.priority;
			parameters[24].Value = model.id;

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
			strSql.Append("delete from wmsProductOutStoreDetail ");
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
			strSql.Append("delete from wmsProductOutStoreDetail ");
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
		public Model.wmsProductOutStoreDetail GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,outStoreDetailNo,productOutStoreNo,productNo,productCount,outstoreCount,boxNo,trayNo,shelfNo,status,skNo,brightnessGroup,boxType,specific,special,productName,creater,createTime,offShelfOperator,offShelfTime,outStoreOperator,outStoreTime,batchNo,acceptCustomer,priority from wmsProductOutStoreDetail ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.wmsProductOutStoreDetail model=new Model.wmsProductOutStoreDetail();
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
		public Model.wmsProductOutStoreDetail DataRowToModel(DataRow row)
		{
			Model.wmsProductOutStoreDetail model=new Model.wmsProductOutStoreDetail();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["outStoreDetailNo"]!=null)
				{
					model.outStoreDetailNo=row["outStoreDetailNo"].ToString();
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
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
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
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["offShelfOperator"]!=null)
				{
					model.offShelfOperator=row["offShelfOperator"].ToString();
				}
				if(row["offShelfTime"]!=null && row["offShelfTime"].ToString()!="")
				{
					model.offShelfTime=DateTime.Parse(row["offShelfTime"].ToString());
				}
				if(row["outStoreOperator"]!=null)
				{
					model.outStoreOperator=row["outStoreOperator"].ToString();
				}
				if(row["outStoreTime"]!=null && row["outStoreTime"].ToString()!="")
				{
					model.outStoreTime=DateTime.Parse(row["outStoreTime"].ToString());
				}
				if(row["batchNo"]!=null)
				{
					model.batchNo=row["batchNo"].ToString();
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
			strSql.Append("select id,outStoreDetailNo,productOutStoreNo,productNo,productCount,outstoreCount,boxNo,trayNo,shelfNo,status,skNo,brightnessGroup,boxType,specific,special,productName,creater,createTime,offShelfOperator,offShelfTime,outStoreOperator,outStoreTime,batchNo,acceptCustomer,priority ");
			strSql.Append(" FROM wmsProductOutStoreDetail ");
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
			strSql.Append(" id,outStoreDetailNo,productOutStoreNo,productNo,productCount,outstoreCount,boxNo,trayNo,shelfNo,status,skNo,brightnessGroup,boxType,specific,special,productName,creater,createTime,offShelfOperator,offShelfTime,outStoreOperator,outStoreTime,batchNo,acceptCustomer,priority ");
			strSql.Append(" FROM wmsProductOutStoreDetail ");
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
			strSql.Append("select count(1) FROM wmsProductOutStoreDetail ");
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
			strSql.Append(")AS Row, T.*  from wmsProductOutStoreDetail T ");
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
			parameters[0].Value = "wmsProductOutStoreDetail";
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

