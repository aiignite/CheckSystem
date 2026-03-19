using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:wmsProductOutStoreAdvanceComputer
	/// </summary>
	public partial class wmsProductOutStoreAdvanceComputer
	{
		public wmsProductOutStoreAdvanceComputer()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "wmsProductOutStoreAdvanceComputer"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from wmsProductOutStoreAdvanceComputer");
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
		public int Add(Model.wmsProductOutStoreAdvanceComputer model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into wmsProductOutStoreAdvanceComputer(");
			strSql.Append("customerNo,customerNeedCount,productNo,productName,storeCount,difference,brightnessGroup,boxType,specific,special,status,outStoreNo,batchNo,creater,createTime,acceptCustomer,priority,memo1)");
			strSql.Append(" values (");
			strSql.Append("@customerNo,@customerNeedCount,@productNo,@productName,@storeCount,@difference,@brightnessGroup,@boxType,@specific,@special,@status,@outStoreNo,@batchNo,@creater,@createTime,@acceptCustomer,@priority,@memo1)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@customerNo", SqlDbType.NVarChar,50),
					new SqlParameter("@customerNeedCount", SqlDbType.Int,4),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,100),
					new SqlParameter("@storeCount", SqlDbType.Int,4),
					new SqlParameter("@difference", SqlDbType.Int,4),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,100),
					new SqlParameter("@boxType", SqlDbType.NVarChar,50),
					new SqlParameter("@specific", SqlDbType.NVarChar,50),
					new SqlParameter("@special", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,20),
					new SqlParameter("@outStoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@acceptCustomer", SqlDbType.NVarChar,50),
					new SqlParameter("@priority", SqlDbType.NVarChar,10),
					new SqlParameter("@memo1", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.customerNo;
			parameters[1].Value = model.customerNeedCount;
			parameters[2].Value = model.productNo;
			parameters[3].Value = model.productName;
			parameters[4].Value = model.storeCount;
			parameters[5].Value = model.difference;
			parameters[6].Value = model.brightnessGroup;
			parameters[7].Value = model.boxType;
			parameters[8].Value = model.specific;
			parameters[9].Value = model.special;
			parameters[10].Value = model.status;
			parameters[11].Value = model.outStoreNo;
			parameters[12].Value = model.batchNo;
			parameters[13].Value = model.creater;
			parameters[14].Value = model.createTime;
			parameters[15].Value = model.acceptCustomer;
			parameters[16].Value = model.priority;
			parameters[17].Value = model.memo1;

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
		public bool Update(Model.wmsProductOutStoreAdvanceComputer model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update wmsProductOutStoreAdvanceComputer set ");
			strSql.Append("customerNo=@customerNo,");
			strSql.Append("customerNeedCount=@customerNeedCount,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("productName=@productName,");
			strSql.Append("storeCount=@storeCount,");
			strSql.Append("difference=@difference,");
			strSql.Append("brightnessGroup=@brightnessGroup,");
			strSql.Append("boxType=@boxType,");
			strSql.Append("specific=@specific,");
			strSql.Append("special=@special,");
			strSql.Append("status=@status,");
			strSql.Append("outStoreNo=@outStoreNo,");
			strSql.Append("batchNo=@batchNo,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("acceptCustomer=@acceptCustomer,");
			strSql.Append("priority=@priority,");
			strSql.Append("memo1=@memo1");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@customerNo", SqlDbType.NVarChar,50),
					new SqlParameter("@customerNeedCount", SqlDbType.Int,4),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,100),
					new SqlParameter("@storeCount", SqlDbType.Int,4),
					new SqlParameter("@difference", SqlDbType.Int,4),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,100),
					new SqlParameter("@boxType", SqlDbType.NVarChar,50),
					new SqlParameter("@specific", SqlDbType.NVarChar,50),
					new SqlParameter("@special", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,20),
					new SqlParameter("@outStoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@acceptCustomer", SqlDbType.NVarChar,50),
					new SqlParameter("@priority", SqlDbType.NVarChar,10),
					new SqlParameter("@memo1", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.customerNo;
			parameters[1].Value = model.customerNeedCount;
			parameters[2].Value = model.productNo;
			parameters[3].Value = model.productName;
			parameters[4].Value = model.storeCount;
			parameters[5].Value = model.difference;
			parameters[6].Value = model.brightnessGroup;
			parameters[7].Value = model.boxType;
			parameters[8].Value = model.specific;
			parameters[9].Value = model.special;
			parameters[10].Value = model.status;
			parameters[11].Value = model.outStoreNo;
			parameters[12].Value = model.batchNo;
			parameters[13].Value = model.creater;
			parameters[14].Value = model.createTime;
			parameters[15].Value = model.acceptCustomer;
			parameters[16].Value = model.priority;
			parameters[17].Value = model.memo1;
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
			strSql.Append("delete from wmsProductOutStoreAdvanceComputer ");
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
			strSql.Append("delete from wmsProductOutStoreAdvanceComputer ");
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
		public Model.wmsProductOutStoreAdvanceComputer GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,customerNo,customerNeedCount,productNo,productName,storeCount,difference,brightnessGroup,boxType,specific,special,status,outStoreNo,batchNo,creater,createTime,acceptCustomer,priority,memo1 from wmsProductOutStoreAdvanceComputer ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.wmsProductOutStoreAdvanceComputer model=new Model.wmsProductOutStoreAdvanceComputer();
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
		public Model.wmsProductOutStoreAdvanceComputer DataRowToModel(DataRow row)
		{
			Model.wmsProductOutStoreAdvanceComputer model=new Model.wmsProductOutStoreAdvanceComputer();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["customerNo"]!=null)
				{
					model.customerNo=row["customerNo"].ToString();
				}
				if(row["customerNeedCount"]!=null && row["customerNeedCount"].ToString()!="")
				{
					model.customerNeedCount=int.Parse(row["customerNeedCount"].ToString());
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["productName"]!=null)
				{
					model.productName=row["productName"].ToString();
				}
				if(row["storeCount"]!=null && row["storeCount"].ToString()!="")
				{
					model.storeCount=int.Parse(row["storeCount"].ToString());
				}
				if(row["difference"]!=null && row["difference"].ToString()!="")
				{
					model.difference=int.Parse(row["difference"].ToString());
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
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["outStoreNo"]!=null)
				{
					model.outStoreNo=row["outStoreNo"].ToString();
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
				if(row["acceptCustomer"]!=null)
				{
					model.acceptCustomer=row["acceptCustomer"].ToString();
				}
				if(row["priority"]!=null)
				{
					model.priority=row["priority"].ToString();
				}
				if(row["memo1"]!=null)
				{
					model.memo1=row["memo1"].ToString();
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
			strSql.Append("select id,customerNo,customerNeedCount,productNo,productName,storeCount,difference,brightnessGroup,boxType,specific,special,status,outStoreNo,batchNo,creater,createTime,acceptCustomer,priority,memo1 ");
			strSql.Append(" FROM wmsProductOutStoreAdvanceComputer ");
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
			strSql.Append(" id,customerNo,customerNeedCount,productNo,productName,storeCount,difference,brightnessGroup,boxType,specific,special,status,outStoreNo,batchNo,creater,createTime,acceptCustomer,priority,memo1 ");
			strSql.Append(" FROM wmsProductOutStoreAdvanceComputer ");
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
			strSql.Append("select count(1) FROM wmsProductOutStoreAdvanceComputer ");
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
			strSql.Append(")AS Row, T.*  from wmsProductOutStoreAdvanceComputer T ");
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
			parameters[0].Value = "wmsProductOutStoreAdvanceComputer";
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

