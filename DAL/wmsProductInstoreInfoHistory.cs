using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:wmsProductInstoreInfoHistory
	/// </summary>
	public partial class wmsProductInstoreInfoHistory
	{
		public wmsProductInstoreInfoHistory()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "wmsProductInstoreInfoHistory"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from wmsProductInstoreInfoHistory");
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
		public int Add(Model.wmsProductInstoreInfoHistory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into wmsProductInstoreInfoHistory(");
			strSql.Append("productInStoreNo,productNo,productName,productCount,instoreCount,boxCount,operatorNo,customerNo,brightnessGroup,boxType,specific,special,type,status,creater,createtime,receiver,receiveTime,outStockType,batchNo,receiptNo)");
			strSql.Append(" values (");
			strSql.Append("@productInStoreNo,@productNo,@productName,@productCount,@instoreCount,@boxCount,@operatorNo,@customerNo,@brightnessGroup,@boxType,@specific,@special,@type,@status,@creater,@createtime,@receiver,@receiveTime,@outStockType,@batchNo,@receiptNo)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@productInStoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.Int,4),
					new SqlParameter("@instoreCount", SqlDbType.Int,4),
					new SqlParameter("@boxCount", SqlDbType.Int,4),
					new SqlParameter("@operatorNo", SqlDbType.NVarChar,50),
					new SqlParameter("@customerNo", SqlDbType.NVarChar,50),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@boxType", SqlDbType.NVarChar,50),
					new SqlParameter("@specific", SqlDbType.NVarChar,50),
					new SqlParameter("@special", SqlDbType.NVarChar,50),
					new SqlParameter("@type", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createtime", SqlDbType.DateTime),
					new SqlParameter("@receiver", SqlDbType.NVarChar,50),
					new SqlParameter("@receiveTime", SqlDbType.DateTime),
					new SqlParameter("@outStockType", SqlDbType.NVarChar,50),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,50),
					new SqlParameter("@receiptNo", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.productInStoreNo;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.productName;
			parameters[3].Value = model.productCount;
			parameters[4].Value = model.instoreCount;
			parameters[5].Value = model.boxCount;
			parameters[6].Value = model.operatorNo;
			parameters[7].Value = model.customerNo;
			parameters[8].Value = model.brightnessGroup;
			parameters[9].Value = model.boxType;
			parameters[10].Value = model.specific;
			parameters[11].Value = model.special;
			parameters[12].Value = model.type;
			parameters[13].Value = model.status;
			parameters[14].Value = model.creater;
			parameters[15].Value = model.createtime;
			parameters[16].Value = model.receiver;
			parameters[17].Value = model.receiveTime;
			parameters[18].Value = model.outStockType;
			parameters[19].Value = model.batchNo;
			parameters[20].Value = model.receiptNo;

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
		public bool Update(Model.wmsProductInstoreInfoHistory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update wmsProductInstoreInfoHistory set ");
			strSql.Append("productInStoreNo=@productInStoreNo,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("productName=@productName,");
			strSql.Append("productCount=@productCount,");
			strSql.Append("instoreCount=@instoreCount,");
			strSql.Append("boxCount=@boxCount,");
			strSql.Append("operatorNo=@operatorNo,");
			strSql.Append("customerNo=@customerNo,");
			strSql.Append("brightnessGroup=@brightnessGroup,");
			strSql.Append("boxType=@boxType,");
			strSql.Append("specific=@specific,");
			strSql.Append("special=@special,");
			strSql.Append("type=@type,");
			strSql.Append("status=@status,");
			strSql.Append("creater=@creater,");
			strSql.Append("createtime=@createtime,");
			strSql.Append("receiver=@receiver,");
			strSql.Append("receiveTime=@receiveTime,");
			strSql.Append("outStockType=@outStockType,");
			strSql.Append("batchNo=@batchNo,");
			strSql.Append("receiptNo=@receiptNo");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@productInStoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.Int,4),
					new SqlParameter("@instoreCount", SqlDbType.Int,4),
					new SqlParameter("@boxCount", SqlDbType.Int,4),
					new SqlParameter("@operatorNo", SqlDbType.NVarChar,50),
					new SqlParameter("@customerNo", SqlDbType.NVarChar,50),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@boxType", SqlDbType.NVarChar,50),
					new SqlParameter("@specific", SqlDbType.NVarChar,50),
					new SqlParameter("@special", SqlDbType.NVarChar,50),
					new SqlParameter("@type", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createtime", SqlDbType.DateTime),
					new SqlParameter("@receiver", SqlDbType.NVarChar,50),
					new SqlParameter("@receiveTime", SqlDbType.DateTime),
					new SqlParameter("@outStockType", SqlDbType.NVarChar,50),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,50),
					new SqlParameter("@receiptNo", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.productInStoreNo;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.productName;
			parameters[3].Value = model.productCount;
			parameters[4].Value = model.instoreCount;
			parameters[5].Value = model.boxCount;
			parameters[6].Value = model.operatorNo;
			parameters[7].Value = model.customerNo;
			parameters[8].Value = model.brightnessGroup;
			parameters[9].Value = model.boxType;
			parameters[10].Value = model.specific;
			parameters[11].Value = model.special;
			parameters[12].Value = model.type;
			parameters[13].Value = model.status;
			parameters[14].Value = model.creater;
			parameters[15].Value = model.createtime;
			parameters[16].Value = model.receiver;
			parameters[17].Value = model.receiveTime;
			parameters[18].Value = model.outStockType;
			parameters[19].Value = model.batchNo;
			parameters[20].Value = model.receiptNo;
			parameters[21].Value = model.id;

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
			strSql.Append("delete from wmsProductInstoreInfoHistory ");
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
			strSql.Append("delete from wmsProductInstoreInfoHistory ");
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
		public Model.wmsProductInstoreInfoHistory GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,productInStoreNo,productNo,productName,productCount,instoreCount,boxCount,operatorNo,customerNo,brightnessGroup,boxType,specific,special,type,status,creater,createtime,receiver,receiveTime,outStockType,batchNo,receiptNo from wmsProductInstoreInfoHistory ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.wmsProductInstoreInfoHistory model=new Model.wmsProductInstoreInfoHistory();
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
		public Model.wmsProductInstoreInfoHistory DataRowToModel(DataRow row)
		{
			Model.wmsProductInstoreInfoHistory model=new Model.wmsProductInstoreInfoHistory();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["productInStoreNo"]!=null)
				{
					model.productInStoreNo=row["productInStoreNo"].ToString();
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["productName"]!=null)
				{
					model.productName=row["productName"].ToString();
				}
				if(row["productCount"]!=null && row["productCount"].ToString()!="")
				{
					model.productCount=int.Parse(row["productCount"].ToString());
				}
				if(row["instoreCount"]!=null && row["instoreCount"].ToString()!="")
				{
					model.instoreCount=int.Parse(row["instoreCount"].ToString());
				}
				if(row["boxCount"]!=null && row["boxCount"].ToString()!="")
				{
					model.boxCount=int.Parse(row["boxCount"].ToString());
				}
				if(row["operatorNo"]!=null)
				{
					model.operatorNo=row["operatorNo"].ToString();
				}
				if(row["customerNo"]!=null)
				{
					model.customerNo=row["customerNo"].ToString();
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
				if(row["type"]!=null)
				{
					model.type=row["type"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createtime"]!=null && row["createtime"].ToString()!="")
				{
					model.createtime=DateTime.Parse(row["createtime"].ToString());
				}
				if(row["receiver"]!=null)
				{
					model.receiver=row["receiver"].ToString();
				}
				if(row["receiveTime"]!=null && row["receiveTime"].ToString()!="")
				{
					model.receiveTime=DateTime.Parse(row["receiveTime"].ToString());
				}
				if(row["outStockType"]!=null)
				{
					model.outStockType=row["outStockType"].ToString();
				}
				if(row["batchNo"]!=null)
				{
					model.batchNo=row["batchNo"].ToString();
				}
				if(row["receiptNo"]!=null)
				{
					model.receiptNo=row["receiptNo"].ToString();
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
			strSql.Append("select id,productInStoreNo,productNo,productName,productCount,instoreCount,boxCount,operatorNo,customerNo,brightnessGroup,boxType,specific,special,type,status,creater,createtime,receiver,receiveTime,outStockType,batchNo,receiptNo ");
			strSql.Append(" FROM wmsProductInstoreInfoHistory ");
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
			strSql.Append(" id,productInStoreNo,productNo,productName,productCount,instoreCount,boxCount,operatorNo,customerNo,brightnessGroup,boxType,specific,special,type,status,creater,createtime,receiver,receiveTime,outStockType,batchNo,receiptNo ");
			strSql.Append(" FROM wmsProductInstoreInfoHistory ");
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
			strSql.Append("select count(1) FROM wmsProductInstoreInfoHistory ");
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
			strSql.Append(")AS Row, T.*  from wmsProductInstoreInfoHistory T ");
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
			parameters[0].Value = "wmsProductInstoreInfoHistory";
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

