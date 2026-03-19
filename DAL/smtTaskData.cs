using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:smtTaskData
	/// </summary>
	public partial class smtTaskData
	{
		public smtTaskData()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "smtTaskData"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from smtTaskData");
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
		public int Add(Model.smtTaskData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into smtTaskData(");
			strSql.Append("taskNo,machineTypeNo,productNo,materialNo,count,bomGroupNo,smtLineNo,status,rate,beginDate,endDate,creater,createTime,processNo,supplyBin)");
			strSql.Append(" values (");
			strSql.Append("@taskNo,@machineTypeNo,@productNo,@materialNo,@count,@bomGroupNo,@smtLineNo,@status,@rate,@beginDate,@endDate,@creater,@createTime,@processNo,@supplyBin)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@machineTypeNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.NVarChar,50),
					new SqlParameter("@bomGroupNo", SqlDbType.NVarChar,50),
					new SqlParameter("@smtLineNo", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@rate", SqlDbType.NVarChar,50),
					new SqlParameter("@beginDate", SqlDbType.DateTime),
					new SqlParameter("@endDate", SqlDbType.DateTime),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@supplyBin", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.taskNo;
			parameters[1].Value = model.machineTypeNo;
			parameters[2].Value = model.productNo;
			parameters[3].Value = model.materialNo;
			parameters[4].Value = model.count;
			parameters[5].Value = model.bomGroupNo;
			parameters[6].Value = model.smtLineNo;
			parameters[7].Value = model.status;
			parameters[8].Value = model.rate;
			parameters[9].Value = model.beginDate;
			parameters[10].Value = model.endDate;
			parameters[11].Value = model.creater;
			parameters[12].Value = model.createTime;
			parameters[13].Value = model.processNo;
			parameters[14].Value = model.supplyBin;

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
		public bool Update(Model.smtTaskData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update smtTaskData set ");
			strSql.Append("taskNo=@taskNo,");
			strSql.Append("machineTypeNo=@machineTypeNo,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("materialNo=@materialNo,");
			strSql.Append("count=@count,");
			strSql.Append("bomGroupNo=@bomGroupNo,");
			strSql.Append("smtLineNo=@smtLineNo,");
			strSql.Append("status=@status,");
			strSql.Append("rate=@rate,");
			strSql.Append("beginDate=@beginDate,");
			strSql.Append("endDate=@endDate,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("processNo=@processNo,");
			strSql.Append("supplyBin=@supplyBin");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@machineTypeNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.NVarChar,50),
					new SqlParameter("@bomGroupNo", SqlDbType.NVarChar,50),
					new SqlParameter("@smtLineNo", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@rate", SqlDbType.NVarChar,50),
					new SqlParameter("@beginDate", SqlDbType.DateTime),
					new SqlParameter("@endDate", SqlDbType.DateTime),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@supplyBin", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.taskNo;
			parameters[1].Value = model.machineTypeNo;
			parameters[2].Value = model.productNo;
			parameters[3].Value = model.materialNo;
			parameters[4].Value = model.count;
			parameters[5].Value = model.bomGroupNo;
			parameters[6].Value = model.smtLineNo;
			parameters[7].Value = model.status;
			parameters[8].Value = model.rate;
			parameters[9].Value = model.beginDate;
			parameters[10].Value = model.endDate;
			parameters[11].Value = model.creater;
			parameters[12].Value = model.createTime;
			parameters[13].Value = model.processNo;
			parameters[14].Value = model.supplyBin;
			parameters[15].Value = model.id;

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
			strSql.Append("delete from smtTaskData ");
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
			strSql.Append("delete from smtTaskData ");
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
		public Model.smtTaskData GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,taskNo,machineTypeNo,productNo,materialNo,count,bomGroupNo,smtLineNo,status,rate,beginDate,endDate,creater,createTime,processNo,supplyBin from smtTaskData ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.smtTaskData model=new Model.smtTaskData();
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
		public Model.smtTaskData DataRowToModel(DataRow row)
		{
			Model.smtTaskData model=new Model.smtTaskData();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["taskNo"]!=null)
				{
					model.taskNo=row["taskNo"].ToString();
				}
				if(row["machineTypeNo"]!=null)
				{
					model.machineTypeNo=row["machineTypeNo"].ToString();
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["materialNo"]!=null)
				{
					model.materialNo=row["materialNo"].ToString();
				}
				if(row["count"]!=null)
				{
					model.count=row["count"].ToString();
				}
				if(row["bomGroupNo"]!=null)
				{
					model.bomGroupNo=row["bomGroupNo"].ToString();
				}
				if(row["smtLineNo"]!=null)
				{
					model.smtLineNo=row["smtLineNo"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["rate"]!=null)
				{
					model.rate=row["rate"].ToString();
				}
				if(row["beginDate"]!=null && row["beginDate"].ToString()!="")
				{
					model.beginDate=DateTime.Parse(row["beginDate"].ToString());
				}
				if(row["endDate"]!=null && row["endDate"].ToString()!="")
				{
					model.endDate=DateTime.Parse(row["endDate"].ToString());
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["processNo"]!=null)
				{
					model.processNo=row["processNo"].ToString();
				}
				if(row["supplyBin"]!=null)
				{
					model.supplyBin=row["supplyBin"].ToString();
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
			strSql.Append("select id,taskNo,machineTypeNo,productNo,materialNo,count,bomGroupNo,smtLineNo,status,rate,beginDate,endDate,creater,createTime,processNo,supplyBin ");
			strSql.Append(" FROM smtTaskData ");
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
			strSql.Append(" id,taskNo,machineTypeNo,productNo,materialNo,count,bomGroupNo,smtLineNo,status,rate,beginDate,endDate,creater,createTime,processNo,supplyBin ");
			strSql.Append(" FROM smtTaskData ");
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
			strSql.Append("select count(1) FROM smtTaskData ");
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
			strSql.Append(")AS Row, T.*  from smtTaskData T ");
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
			parameters[0].Value = "smtTaskData";
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

