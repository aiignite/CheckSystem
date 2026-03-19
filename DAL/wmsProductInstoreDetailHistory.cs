using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:wmsProductInstoreDetailHistory
	/// </summary>
	public partial class wmsProductInstoreDetailHistory
	{
		public wmsProductInstoreDetailHistory()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "wmsProductInstoreDetailHistory"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from wmsProductInstoreDetailHistory");
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
		public int Add(Model.wmsProductInstoreDetailHistory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into wmsProductInstoreDetailHistory(");
			strSql.Append("instoreDetailNo,productInstoreNo,productNo,productCount,boxNo,trayNo,shelfNo,barcode,status,operatorNo,createTime,instoreTime,outStoreCount,skNo,brightnessGroup,specific,productName,batchNo,boxType,special,returnman,returnTime,deleteTime,deleteman,creater)");
			strSql.Append(" values (");
			strSql.Append("@instoreDetailNo,@productInstoreNo,@productNo,@productCount,@boxNo,@trayNo,@shelfNo,@barcode,@status,@operatorNo,@createTime,@instoreTime,@outStoreCount,@skNo,@brightnessGroup,@specific,@productName,@batchNo,@boxType,@special,@returnman,@returnTime,@deleteTime,@deleteman,@creater)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@instoreDetailNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productInstoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.Int,4),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,200),
					new SqlParameter("@trayNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@barcode", SqlDbType.NVarChar,150),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@operatorNo", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@instoreTime", SqlDbType.DateTime),
					new SqlParameter("@outStoreCount", SqlDbType.Int,4),
					new SqlParameter("@skNo", SqlDbType.NVarChar,50),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@specific", SqlDbType.NVarChar,30),
					new SqlParameter("@productName", SqlDbType.NVarChar,50),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,20),
					new SqlParameter("@boxType", SqlDbType.NVarChar,20),
					new SqlParameter("@special", SqlDbType.NVarChar,20),
					new SqlParameter("@returnman", SqlDbType.NVarChar,50),
					new SqlParameter("@returnTime", SqlDbType.DateTime),
					new SqlParameter("@deleteTime", SqlDbType.DateTime),
					new SqlParameter("@deleteman", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.instoreDetailNo;
			parameters[1].Value = model.productInstoreNo;
			parameters[2].Value = model.productNo;
			parameters[3].Value = model.productCount;
			parameters[4].Value = model.boxNo;
			parameters[5].Value = model.trayNo;
			parameters[6].Value = model.shelfNo;
			parameters[7].Value = model.barcode;
			parameters[8].Value = model.status;
			parameters[9].Value = model.operatorNo;
			parameters[10].Value = model.createTime;
			parameters[11].Value = model.instoreTime;
			parameters[12].Value = model.outStoreCount;
			parameters[13].Value = model.skNo;
			parameters[14].Value = model.brightnessGroup;
			parameters[15].Value = model.specific;
			parameters[16].Value = model.productName;
			parameters[17].Value = model.batchNo;
			parameters[18].Value = model.boxType;
			parameters[19].Value = model.special;
			parameters[20].Value = model.returnman;
			parameters[21].Value = model.returnTime;
			parameters[22].Value = model.deleteTime;
			parameters[23].Value = model.deleteman;
			parameters[24].Value = model.creater;

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
		public bool Update(Model.wmsProductInstoreDetailHistory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update wmsProductInstoreDetailHistory set ");
			strSql.Append("instoreDetailNo=@instoreDetailNo,");
			strSql.Append("productInstoreNo=@productInstoreNo,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("productCount=@productCount,");
			strSql.Append("boxNo=@boxNo,");
			strSql.Append("trayNo=@trayNo,");
			strSql.Append("shelfNo=@shelfNo,");
			strSql.Append("barcode=@barcode,");
			strSql.Append("status=@status,");
			strSql.Append("operatorNo=@operatorNo,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("instoreTime=@instoreTime,");
			strSql.Append("outStoreCount=@outStoreCount,");
			strSql.Append("skNo=@skNo,");
			strSql.Append("brightnessGroup=@brightnessGroup,");
			strSql.Append("specific=@specific,");
			strSql.Append("productName=@productName,");
			strSql.Append("batchNo=@batchNo,");
			strSql.Append("boxType=@boxType,");
			strSql.Append("special=@special,");
			strSql.Append("returnman=@returnman,");
			strSql.Append("returnTime=@returnTime,");
			strSql.Append("deleteTime=@deleteTime,");
			strSql.Append("deleteman=@deleteman,");
			strSql.Append("creater=@creater");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@instoreDetailNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productInstoreNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.Int,4),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,200),
					new SqlParameter("@trayNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@barcode", SqlDbType.NVarChar,150),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@operatorNo", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@instoreTime", SqlDbType.DateTime),
					new SqlParameter("@outStoreCount", SqlDbType.Int,4),
					new SqlParameter("@skNo", SqlDbType.NVarChar,50),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@specific", SqlDbType.NVarChar,30),
					new SqlParameter("@productName", SqlDbType.NVarChar,50),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,20),
					new SqlParameter("@boxType", SqlDbType.NVarChar,20),
					new SqlParameter("@special", SqlDbType.NVarChar,20),
					new SqlParameter("@returnman", SqlDbType.NVarChar,50),
					new SqlParameter("@returnTime", SqlDbType.DateTime),
					new SqlParameter("@deleteTime", SqlDbType.DateTime),
					new SqlParameter("@deleteman", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.instoreDetailNo;
			parameters[1].Value = model.productInstoreNo;
			parameters[2].Value = model.productNo;
			parameters[3].Value = model.productCount;
			parameters[4].Value = model.boxNo;
			parameters[5].Value = model.trayNo;
			parameters[6].Value = model.shelfNo;
			parameters[7].Value = model.barcode;
			parameters[8].Value = model.status;
			parameters[9].Value = model.operatorNo;
			parameters[10].Value = model.createTime;
			parameters[11].Value = model.instoreTime;
			parameters[12].Value = model.outStoreCount;
			parameters[13].Value = model.skNo;
			parameters[14].Value = model.brightnessGroup;
			parameters[15].Value = model.specific;
			parameters[16].Value = model.productName;
			parameters[17].Value = model.batchNo;
			parameters[18].Value = model.boxType;
			parameters[19].Value = model.special;
			parameters[20].Value = model.returnman;
			parameters[21].Value = model.returnTime;
			parameters[22].Value = model.deleteTime;
			parameters[23].Value = model.deleteman;
			parameters[24].Value = model.creater;
			parameters[25].Value = model.id;

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
			strSql.Append("delete from wmsProductInstoreDetailHistory ");
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
			strSql.Append("delete from wmsProductInstoreDetailHistory ");
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
		public Model.wmsProductInstoreDetailHistory GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,instoreDetailNo,productInstoreNo,productNo,productCount,boxNo,trayNo,shelfNo,barcode,status,operatorNo,createTime,instoreTime,outStoreCount,skNo,brightnessGroup,specific,productName,batchNo,boxType,special,returnman,returnTime,deleteTime,deleteman,creater from wmsProductInstoreDetailHistory ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.wmsProductInstoreDetailHistory model=new Model.wmsProductInstoreDetailHistory();
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
		public Model.wmsProductInstoreDetailHistory DataRowToModel(DataRow row)
		{
			Model.wmsProductInstoreDetailHistory model=new Model.wmsProductInstoreDetailHistory();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["instoreDetailNo"]!=null)
				{
					model.instoreDetailNo=row["instoreDetailNo"].ToString();
				}
				if(row["productInstoreNo"]!=null)
				{
					model.productInstoreNo=row["productInstoreNo"].ToString();
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["productCount"]!=null && row["productCount"].ToString()!="")
				{
					model.productCount=int.Parse(row["productCount"].ToString());
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
				if(row["barcode"]!=null)
				{
					model.barcode=row["barcode"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["operatorNo"]!=null)
				{
					model.operatorNo=row["operatorNo"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["instoreTime"]!=null && row["instoreTime"].ToString()!="")
				{
					model.instoreTime=DateTime.Parse(row["instoreTime"].ToString());
				}
				if(row["outStoreCount"]!=null && row["outStoreCount"].ToString()!="")
				{
					model.outStoreCount=int.Parse(row["outStoreCount"].ToString());
				}
				if(row["skNo"]!=null)
				{
					model.skNo=row["skNo"].ToString();
				}
				if(row["brightnessGroup"]!=null)
				{
					model.brightnessGroup=row["brightnessGroup"].ToString();
				}
				if(row["specific"]!=null)
				{
					model.specific=row["specific"].ToString();
				}
				if(row["productName"]!=null)
				{
					model.productName=row["productName"].ToString();
				}
				if(row["batchNo"]!=null)
				{
					model.batchNo=row["batchNo"].ToString();
				}
				if(row["boxType"]!=null)
				{
					model.boxType=row["boxType"].ToString();
				}
				if(row["special"]!=null)
				{
					model.special=row["special"].ToString();
				}
				if(row["returnman"]!=null)
				{
					model.returnman=row["returnman"].ToString();
				}
				if(row["returnTime"]!=null && row["returnTime"].ToString()!="")
				{
					model.returnTime=DateTime.Parse(row["returnTime"].ToString());
				}
				if(row["deleteTime"]!=null && row["deleteTime"].ToString()!="")
				{
					model.deleteTime=DateTime.Parse(row["deleteTime"].ToString());
				}
				if(row["deleteman"]!=null)
				{
					model.deleteman=row["deleteman"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
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
			strSql.Append("select id,instoreDetailNo,productInstoreNo,productNo,productCount,boxNo,trayNo,shelfNo,barcode,status,operatorNo,createTime,instoreTime,outStoreCount,skNo,brightnessGroup,specific,productName,batchNo,boxType,special,returnman,returnTime,deleteTime,deleteman,creater ");
			strSql.Append(" FROM wmsProductInstoreDetailHistory ");
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
			strSql.Append(" id,instoreDetailNo,productInstoreNo,productNo,productCount,boxNo,trayNo,shelfNo,barcode,status,operatorNo,createTime,instoreTime,outStoreCount,skNo,brightnessGroup,specific,productName,batchNo,boxType,special,returnman,returnTime,deleteTime,deleteman,creater ");
			strSql.Append(" FROM wmsProductInstoreDetailHistory ");
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
			strSql.Append("select count(1) FROM wmsProductInstoreDetailHistory ");
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
			strSql.Append(")AS Row, T.*  from wmsProductInstoreDetailHistory T ");
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
			parameters[0].Value = "wmsProductInstoreDetailHistory";
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

