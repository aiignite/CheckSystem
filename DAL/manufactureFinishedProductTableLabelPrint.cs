using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:manufactureFinishedProductTableLabelPrint
	/// </summary>
	public partial class manufactureFinishedProductTableLabelPrint
	{
		public manufactureFinishedProductTableLabelPrint()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "manufactureFinishedProductTableLabelPrint"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from manufactureFinishedProductTableLabelPrint");
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
		public int Add(Model.manufactureFinishedProductTableLabelPrint model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into manufactureFinishedProductTableLabelPrint(");
			strSql.Append("materialNo,materialName,batchNo,minPackageNum,customerNo,boxSerialNoStart,boxSerialNoEnd,brightnessGroup,special,specialStatus,createTime,packageStyle,status,instoreStatus,creater,productCount,boxCount,instoreReceiptCount,pcNo,instoreReceiptStart)");
			strSql.Append(" values (");
			strSql.Append("@materialNo,@materialName,@batchNo,@minPackageNum,@customerNo,@boxSerialNoStart,@boxSerialNoEnd,@brightnessGroup,@special,@specialStatus,@createTime,@packageStyle,@status,@instoreStatus,@creater,@productCount,@boxCount,@instoreReceiptCount,@pcNo,@instoreReceiptStart)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@materialNo", SqlDbType.NVarChar,40),
					new SqlParameter("@materialName", SqlDbType.NVarChar,50),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,20),
					new SqlParameter("@minPackageNum", SqlDbType.NVarChar,10),
					new SqlParameter("@customerNo", SqlDbType.NVarChar,30),
					new SqlParameter("@boxSerialNoStart", SqlDbType.NVarChar,50),
					new SqlParameter("@boxSerialNoEnd", SqlDbType.NVarChar,50),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@special", SqlDbType.NVarChar,30),
					new SqlParameter("@specialStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@packageStyle", SqlDbType.NVarChar,20),
					new SqlParameter("@status", SqlDbType.NVarChar,10),
					new SqlParameter("@instoreStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.NVarChar,50),
					new SqlParameter("@boxCount", SqlDbType.Int,4),
					new SqlParameter("@instoreReceiptCount", SqlDbType.NVarChar,50),
					new SqlParameter("@pcNo", SqlDbType.NVarChar,50),
					new SqlParameter("@instoreReceiptStart", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.materialNo;
			parameters[1].Value = model.materialName;
			parameters[2].Value = model.batchNo;
			parameters[3].Value = model.minPackageNum;
			parameters[4].Value = model.customerNo;
			parameters[5].Value = model.boxSerialNoStart;
			parameters[6].Value = model.boxSerialNoEnd;
			parameters[7].Value = model.brightnessGroup;
			parameters[8].Value = model.special;
			parameters[9].Value = model.specialStatus;
			parameters[10].Value = model.createTime;
			parameters[11].Value = model.packageStyle;
			parameters[12].Value = model.status;
			parameters[13].Value = model.instoreStatus;
			parameters[14].Value = model.creater;
			parameters[15].Value = model.productCount;
			parameters[16].Value = model.boxCount;
			parameters[17].Value = model.instoreReceiptCount;
			parameters[18].Value = model.pcNo;
			parameters[19].Value = model.instoreReceiptStart;

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
		public bool Update(Model.manufactureFinishedProductTableLabelPrint model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update manufactureFinishedProductTableLabelPrint set ");
			strSql.Append("materialNo=@materialNo,");
			strSql.Append("materialName=@materialName,");
			strSql.Append("batchNo=@batchNo,");
			strSql.Append("minPackageNum=@minPackageNum,");
			strSql.Append("customerNo=@customerNo,");
			strSql.Append("boxSerialNoStart=@boxSerialNoStart,");
			strSql.Append("boxSerialNoEnd=@boxSerialNoEnd,");
			strSql.Append("brightnessGroup=@brightnessGroup,");
			strSql.Append("special=@special,");
			strSql.Append("specialStatus=@specialStatus,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("packageStyle=@packageStyle,");
			strSql.Append("status=@status,");
			strSql.Append("instoreStatus=@instoreStatus,");
			strSql.Append("creater=@creater,");
			strSql.Append("productCount=@productCount,");
			strSql.Append("boxCount=@boxCount,");
			strSql.Append("instoreReceiptCount=@instoreReceiptCount,");
			strSql.Append("pcNo=@pcNo,");
			strSql.Append("instoreReceiptStart=@instoreReceiptStart");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@materialNo", SqlDbType.NVarChar,40),
					new SqlParameter("@materialName", SqlDbType.NVarChar,50),
					new SqlParameter("@batchNo", SqlDbType.NVarChar,20),
					new SqlParameter("@minPackageNum", SqlDbType.NVarChar,10),
					new SqlParameter("@customerNo", SqlDbType.NVarChar,30),
					new SqlParameter("@boxSerialNoStart", SqlDbType.NVarChar,50),
					new SqlParameter("@boxSerialNoEnd", SqlDbType.NVarChar,50),
					new SqlParameter("@brightnessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@special", SqlDbType.NVarChar,30),
					new SqlParameter("@specialStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@packageStyle", SqlDbType.NVarChar,20),
					new SqlParameter("@status", SqlDbType.NVarChar,10),
					new SqlParameter("@instoreStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@productCount", SqlDbType.NVarChar,50),
					new SqlParameter("@boxCount", SqlDbType.Int,4),
					new SqlParameter("@instoreReceiptCount", SqlDbType.NVarChar,50),
					new SqlParameter("@pcNo", SqlDbType.NVarChar,50),
					new SqlParameter("@instoreReceiptStart", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.materialNo;
			parameters[1].Value = model.materialName;
			parameters[2].Value = model.batchNo;
			parameters[3].Value = model.minPackageNum;
			parameters[4].Value = model.customerNo;
			parameters[5].Value = model.boxSerialNoStart;
			parameters[6].Value = model.boxSerialNoEnd;
			parameters[7].Value = model.brightnessGroup;
			parameters[8].Value = model.special;
			parameters[9].Value = model.specialStatus;
			parameters[10].Value = model.createTime;
			parameters[11].Value = model.packageStyle;
			parameters[12].Value = model.status;
			parameters[13].Value = model.instoreStatus;
			parameters[14].Value = model.creater;
			parameters[15].Value = model.productCount;
			parameters[16].Value = model.boxCount;
			parameters[17].Value = model.instoreReceiptCount;
			parameters[18].Value = model.pcNo;
			parameters[19].Value = model.instoreReceiptStart;
			parameters[20].Value = model.id;

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
			strSql.Append("delete from manufactureFinishedProductTableLabelPrint ");
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
			strSql.Append("delete from manufactureFinishedProductTableLabelPrint ");
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
		public Model.manufactureFinishedProductTableLabelPrint GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,materialNo,materialName,batchNo,minPackageNum,customerNo,boxSerialNoStart,boxSerialNoEnd,brightnessGroup,special,specialStatus,createTime,packageStyle,status,instoreStatus,creater,productCount,boxCount,instoreReceiptCount,pcNo,instoreReceiptStart from manufactureFinishedProductTableLabelPrint ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.manufactureFinishedProductTableLabelPrint model=new Model.manufactureFinishedProductTableLabelPrint();
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
		public Model.manufactureFinishedProductTableLabelPrint DataRowToModel(DataRow row)
		{
			Model.manufactureFinishedProductTableLabelPrint model=new Model.manufactureFinishedProductTableLabelPrint();
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
				if(row["materialName"]!=null)
				{
					model.materialName=row["materialName"].ToString();
				}
				if(row["batchNo"]!=null)
				{
					model.batchNo=row["batchNo"].ToString();
				}
				if(row["minPackageNum"]!=null)
				{
					model.minPackageNum=row["minPackageNum"].ToString();
				}
				if(row["customerNo"]!=null)
				{
					model.customerNo=row["customerNo"].ToString();
				}
				if(row["boxSerialNoStart"]!=null)
				{
					model.boxSerialNoStart=row["boxSerialNoStart"].ToString();
				}
				if(row["boxSerialNoEnd"]!=null)
				{
					model.boxSerialNoEnd=row["boxSerialNoEnd"].ToString();
				}
				if(row["brightnessGroup"]!=null)
				{
					model.brightnessGroup=row["brightnessGroup"].ToString();
				}
				if(row["special"]!=null)
				{
					model.special=row["special"].ToString();
				}
				if(row["specialStatus"]!=null)
				{
					model.specialStatus=row["specialStatus"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["packageStyle"]!=null)
				{
					model.packageStyle=row["packageStyle"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["instoreStatus"]!=null)
				{
					model.instoreStatus=row["instoreStatus"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["productCount"]!=null)
				{
					model.productCount=row["productCount"].ToString();
				}
				if(row["boxCount"]!=null && row["boxCount"].ToString()!="")
				{
					model.boxCount=int.Parse(row["boxCount"].ToString());
				}
				if(row["instoreReceiptCount"]!=null)
				{
					model.instoreReceiptCount=row["instoreReceiptCount"].ToString();
				}
				if(row["pcNo"]!=null)
				{
					model.pcNo=row["pcNo"].ToString();
				}
				if(row["instoreReceiptStart"]!=null)
				{
					model.instoreReceiptStart=row["instoreReceiptStart"].ToString();
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
			strSql.Append("select id,materialNo,materialName,batchNo,minPackageNum,customerNo,boxSerialNoStart,boxSerialNoEnd,brightnessGroup,special,specialStatus,createTime,packageStyle,status,instoreStatus,creater,productCount,boxCount,instoreReceiptCount,pcNo,instoreReceiptStart ");
			strSql.Append(" FROM manufactureFinishedProductTableLabelPrint ");
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
			strSql.Append(" id,materialNo,materialName,batchNo,minPackageNum,customerNo,boxSerialNoStart,boxSerialNoEnd,brightnessGroup,special,specialStatus,createTime,packageStyle,status,instoreStatus,creater,productCount,boxCount,instoreReceiptCount,pcNo,instoreReceiptStart ");
			strSql.Append(" FROM manufactureFinishedProductTableLabelPrint ");
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
			strSql.Append("select count(1) FROM manufactureFinishedProductTableLabelPrint ");
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
			strSql.Append(")AS Row, T.*  from manufactureFinishedProductTableLabelPrint T ");
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
			parameters[0].Value = "manufactureFinishedProductTableLabelPrint";
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

