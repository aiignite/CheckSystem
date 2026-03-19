using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:smtStockData
	/// </summary>
	public partial class smtStockData
	{
		public smtStockData()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "smtStockData"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from smtStockData");
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
		public int Add(Model.smtStockData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into smtStockData(");
			strSql.Append("materialNo,count,barcode,lotNo,boxNo,shelfNo,instoreDate,instoreOperator,outstoreDate,outstoreOperator,status,creater,createTime,bin,ledGroup,outReceiptNo)");
			strSql.Append(" values (");
			strSql.Append("@materialNo,@count,@barcode,@lotNo,@boxNo,@shelfNo,@instoreDate,@instoreOperator,@outstoreDate,@outstoreOperator,@status,@creater,@createTime,@bin,@ledGroup,@outReceiptNo)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.NVarChar,50),
					new SqlParameter("@barcode", SqlDbType.NVarChar,50),
					new SqlParameter("@lotNo", SqlDbType.NVarChar,50),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@instoreDate", SqlDbType.DateTime),
					new SqlParameter("@instoreOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@outstoreDate", SqlDbType.DateTime),
					new SqlParameter("@outstoreOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@bin", SqlDbType.NVarChar,50),
					new SqlParameter("@ledGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@outReceiptNo", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.materialNo;
			parameters[1].Value = model.count;
			parameters[2].Value = model.barcode;
			parameters[3].Value = model.lotNo;
			parameters[4].Value = model.boxNo;
			parameters[5].Value = model.shelfNo;
			parameters[6].Value = model.instoreDate;
			parameters[7].Value = model.instoreOperator;
			parameters[8].Value = model.outstoreDate;
			parameters[9].Value = model.outstoreOperator;
			parameters[10].Value = model.status;
			parameters[11].Value = model.creater;
			parameters[12].Value = model.createTime;
			parameters[13].Value = model.bin;
			parameters[14].Value = model.ledGroup;
			parameters[15].Value = model.outReceiptNo;

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
		public bool Update(Model.smtStockData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update smtStockData set ");
			strSql.Append("materialNo=@materialNo,");
			strSql.Append("count=@count,");
			strSql.Append("barcode=@barcode,");
			strSql.Append("lotNo=@lotNo,");
			strSql.Append("boxNo=@boxNo,");
			strSql.Append("shelfNo=@shelfNo,");
			strSql.Append("instoreDate=@instoreDate,");
			strSql.Append("instoreOperator=@instoreOperator,");
			strSql.Append("outstoreDate=@outstoreDate,");
			strSql.Append("outstoreOperator=@outstoreOperator,");
			strSql.Append("status=@status,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("bin=@bin,");
			strSql.Append("ledGroup=@ledGroup,");
			strSql.Append("outReceiptNo=@outReceiptNo");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.NVarChar,50),
					new SqlParameter("@barcode", SqlDbType.NVarChar,50),
					new SqlParameter("@lotNo", SqlDbType.NVarChar,50),
					new SqlParameter("@boxNo", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@instoreDate", SqlDbType.DateTime),
					new SqlParameter("@instoreOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@outstoreDate", SqlDbType.DateTime),
					new SqlParameter("@outstoreOperator", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@bin", SqlDbType.NVarChar,50),
					new SqlParameter("@ledGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@outReceiptNo", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.materialNo;
			parameters[1].Value = model.count;
			parameters[2].Value = model.barcode;
			parameters[3].Value = model.lotNo;
			parameters[4].Value = model.boxNo;
			parameters[5].Value = model.shelfNo;
			parameters[6].Value = model.instoreDate;
			parameters[7].Value = model.instoreOperator;
			parameters[8].Value = model.outstoreDate;
			parameters[9].Value = model.outstoreOperator;
			parameters[10].Value = model.status;
			parameters[11].Value = model.creater;
			parameters[12].Value = model.createTime;
			parameters[13].Value = model.bin;
			parameters[14].Value = model.ledGroup;
			parameters[15].Value = model.outReceiptNo;
			parameters[16].Value = model.id;

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
			strSql.Append("delete from smtStockData ");
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
			strSql.Append("delete from smtStockData ");
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
		public Model.smtStockData GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,materialNo,count,barcode,lotNo,boxNo,shelfNo,instoreDate,instoreOperator,outstoreDate,outstoreOperator,status,creater,createTime,bin,ledGroup,outReceiptNo from smtStockData ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.smtStockData model=new Model.smtStockData();
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
		public Model.smtStockData DataRowToModel(DataRow row)
		{
			Model.smtStockData model=new Model.smtStockData();
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
				if(row["count"]!=null)
				{
					model.count=row["count"].ToString();
				}
				if(row["barcode"]!=null)
				{
					model.barcode=row["barcode"].ToString();
				}
				if(row["lotNo"]!=null)
				{
					model.lotNo=row["lotNo"].ToString();
				}
				if(row["boxNo"]!=null)
				{
					model.boxNo=row["boxNo"].ToString();
				}
				if(row["shelfNo"]!=null)
				{
					model.shelfNo=row["shelfNo"].ToString();
				}
				if(row["instoreDate"]!=null && row["instoreDate"].ToString()!="")
				{
					model.instoreDate=DateTime.Parse(row["instoreDate"].ToString());
				}
				if(row["instoreOperator"]!=null)
				{
					model.instoreOperator=row["instoreOperator"].ToString();
				}
				if(row["outstoreDate"]!=null && row["outstoreDate"].ToString()!="")
				{
					model.outstoreDate=DateTime.Parse(row["outstoreDate"].ToString());
				}
				if(row["outstoreOperator"]!=null)
				{
					model.outstoreOperator=row["outstoreOperator"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["bin"]!=null)
				{
					model.bin=row["bin"].ToString();
				}
				if(row["ledGroup"]!=null)
				{
					model.ledGroup=row["ledGroup"].ToString();
				}
				if(row["outReceiptNo"]!=null)
				{
					model.outReceiptNo=row["outReceiptNo"].ToString();
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
			strSql.Append("select id,materialNo,count,barcode,lotNo,boxNo,shelfNo,instoreDate,instoreOperator,outstoreDate,outstoreOperator,status,creater,createTime,bin,ledGroup,outReceiptNo ");
			strSql.Append(" FROM smtStockData ");
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
			strSql.Append(" id,materialNo,count,barcode,lotNo,boxNo,shelfNo,instoreDate,instoreOperator,outstoreDate,outstoreOperator,status,creater,createTime,bin,ledGroup,outReceiptNo ");
			strSql.Append(" FROM smtStockData ");
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
			strSql.Append("select count(1) FROM smtStockData ");
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
			strSql.Append(")AS Row, T.*  from smtStockData T ");
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
			parameters[0].Value = "smtStockData";
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

