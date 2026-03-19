using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:smtTaskComputer
	/// </summary>
	public partial class smtTaskComputer
	{
		public smtTaskComputer()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "smtTaskComputer"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from smtTaskComputer");
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
		public int Add(Model.smtTaskComputer model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into smtTaskComputer(");
			strSql.Append("taskNo,machineTypeNo,productNo,count,bomGroupNo,materialNo,materialCount,lineWareCount,instoreCount,diffCount,creater,createTime,bin,status,shelfNo,isLineWare,lineWareOutCount,stockOutCount,stockExtraOutCout,isHaveReplaceMaterial,replaceMaterialTempGroup,priority,supplyBin)");
			strSql.Append(" values (");
			strSql.Append("@taskNo,@machineTypeNo,@productNo,@count,@bomGroupNo,@materialNo,@materialCount,@lineWareCount,@instoreCount,@diffCount,@creater,@createTime,@bin,@status,@shelfNo,@isLineWare,@lineWareOutCount,@stockOutCount,@stockExtraOutCout,@isHaveReplaceMaterial,@replaceMaterialTempGroup,@priority,@supplyBin)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@machineTypeNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.NVarChar,50),
					new SqlParameter("@bomGroupNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialCount", SqlDbType.Int,4),
					new SqlParameter("@lineWareCount", SqlDbType.Int,4),
					new SqlParameter("@instoreCount", SqlDbType.Int,4),
					new SqlParameter("@diffCount", SqlDbType.Int,4),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@bin", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@isLineWare", SqlDbType.NVarChar,10),
					new SqlParameter("@lineWareOutCount", SqlDbType.Int,4),
					new SqlParameter("@stockOutCount", SqlDbType.Int,4),
					new SqlParameter("@stockExtraOutCout", SqlDbType.Int,4),
					new SqlParameter("@isHaveReplaceMaterial", SqlDbType.NVarChar,50),
					new SqlParameter("@replaceMaterialTempGroup", SqlDbType.Int,4),
					new SqlParameter("@priority", SqlDbType.Int,4),
					new SqlParameter("@supplyBin", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.taskNo;
			parameters[1].Value = model.machineTypeNo;
			parameters[2].Value = model.productNo;
			parameters[3].Value = model.count;
			parameters[4].Value = model.bomGroupNo;
			parameters[5].Value = model.materialNo;
			parameters[6].Value = model.materialCount;
			parameters[7].Value = model.lineWareCount;
			parameters[8].Value = model.instoreCount;
			parameters[9].Value = model.diffCount;
			parameters[10].Value = model.creater;
			parameters[11].Value = model.createTime;
			parameters[12].Value = model.bin;
			parameters[13].Value = model.status;
			parameters[14].Value = model.shelfNo;
			parameters[15].Value = model.isLineWare;
			parameters[16].Value = model.lineWareOutCount;
			parameters[17].Value = model.stockOutCount;
			parameters[18].Value = model.stockExtraOutCout;
			parameters[19].Value = model.isHaveReplaceMaterial;
			parameters[20].Value = model.replaceMaterialTempGroup;
			parameters[21].Value = model.priority;
			parameters[22].Value = model.supplyBin;

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
		public bool Update(Model.smtTaskComputer model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update smtTaskComputer set ");
			strSql.Append("taskNo=@taskNo,");
			strSql.Append("machineTypeNo=@machineTypeNo,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("count=@count,");
			strSql.Append("bomGroupNo=@bomGroupNo,");
			strSql.Append("materialNo=@materialNo,");
			strSql.Append("materialCount=@materialCount,");
			strSql.Append("lineWareCount=@lineWareCount,");
			strSql.Append("instoreCount=@instoreCount,");
			strSql.Append("diffCount=@diffCount,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("bin=@bin,");
			strSql.Append("status=@status,");
			strSql.Append("shelfNo=@shelfNo,");
			strSql.Append("isLineWare=@isLineWare,");
			strSql.Append("lineWareOutCount=@lineWareOutCount,");
			strSql.Append("stockOutCount=@stockOutCount,");
			strSql.Append("stockExtraOutCout=@stockExtraOutCout,");
			strSql.Append("isHaveReplaceMaterial=@isHaveReplaceMaterial,");
			strSql.Append("replaceMaterialTempGroup=@replaceMaterialTempGroup,");
			strSql.Append("priority=@priority,");
			strSql.Append("supplyBin=@supplyBin");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@machineTypeNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.NVarChar,50),
					new SqlParameter("@bomGroupNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialCount", SqlDbType.Int,4),
					new SqlParameter("@lineWareCount", SqlDbType.Int,4),
					new SqlParameter("@instoreCount", SqlDbType.Int,4),
					new SqlParameter("@diffCount", SqlDbType.Int,4),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@bin", SqlDbType.NVarChar,50),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@shelfNo", SqlDbType.NVarChar,50),
					new SqlParameter("@isLineWare", SqlDbType.NVarChar,10),
					new SqlParameter("@lineWareOutCount", SqlDbType.Int,4),
					new SqlParameter("@stockOutCount", SqlDbType.Int,4),
					new SqlParameter("@stockExtraOutCout", SqlDbType.Int,4),
					new SqlParameter("@isHaveReplaceMaterial", SqlDbType.NVarChar,50),
					new SqlParameter("@replaceMaterialTempGroup", SqlDbType.Int,4),
					new SqlParameter("@priority", SqlDbType.Int,4),
					new SqlParameter("@supplyBin", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.taskNo;
			parameters[1].Value = model.machineTypeNo;
			parameters[2].Value = model.productNo;
			parameters[3].Value = model.count;
			parameters[4].Value = model.bomGroupNo;
			parameters[5].Value = model.materialNo;
			parameters[6].Value = model.materialCount;
			parameters[7].Value = model.lineWareCount;
			parameters[8].Value = model.instoreCount;
			parameters[9].Value = model.diffCount;
			parameters[10].Value = model.creater;
			parameters[11].Value = model.createTime;
			parameters[12].Value = model.bin;
			parameters[13].Value = model.status;
			parameters[14].Value = model.shelfNo;
			parameters[15].Value = model.isLineWare;
			parameters[16].Value = model.lineWareOutCount;
			parameters[17].Value = model.stockOutCount;
			parameters[18].Value = model.stockExtraOutCout;
			parameters[19].Value = model.isHaveReplaceMaterial;
			parameters[20].Value = model.replaceMaterialTempGroup;
			parameters[21].Value = model.priority;
			parameters[22].Value = model.supplyBin;
			parameters[23].Value = model.id;

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
			strSql.Append("delete from smtTaskComputer ");
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
			strSql.Append("delete from smtTaskComputer ");
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
		public Model.smtTaskComputer GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,taskNo,machineTypeNo,productNo,count,bomGroupNo,materialNo,materialCount,lineWareCount,instoreCount,diffCount,creater,createTime,bin,status,shelfNo,isLineWare,lineWareOutCount,stockOutCount,stockExtraOutCout,isHaveReplaceMaterial,replaceMaterialTempGroup,priority,supplyBin from smtTaskComputer ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.smtTaskComputer model=new Model.smtTaskComputer();
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
		public Model.smtTaskComputer DataRowToModel(DataRow row)
		{
			Model.smtTaskComputer model=new Model.smtTaskComputer();
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
				if(row["count"]!=null)
				{
					model.count=row["count"].ToString();
				}
				if(row["bomGroupNo"]!=null)
				{
					model.bomGroupNo=row["bomGroupNo"].ToString();
				}
				if(row["materialNo"]!=null)
				{
					model.materialNo=row["materialNo"].ToString();
				}
				if(row["materialCount"]!=null && row["materialCount"].ToString()!="")
				{
					model.materialCount=int.Parse(row["materialCount"].ToString());
				}
				if(row["lineWareCount"]!=null && row["lineWareCount"].ToString()!="")
				{
					model.lineWareCount=int.Parse(row["lineWareCount"].ToString());
				}
				if(row["instoreCount"]!=null && row["instoreCount"].ToString()!="")
				{
					model.instoreCount=int.Parse(row["instoreCount"].ToString());
				}
				if(row["diffCount"]!=null && row["diffCount"].ToString()!="")
				{
					model.diffCount=int.Parse(row["diffCount"].ToString());
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
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["shelfNo"]!=null)
				{
					model.shelfNo=row["shelfNo"].ToString();
				}
				if(row["isLineWare"]!=null)
				{
					model.isLineWare=row["isLineWare"].ToString();
				}
				if(row["lineWareOutCount"]!=null && row["lineWareOutCount"].ToString()!="")
				{
					model.lineWareOutCount=int.Parse(row["lineWareOutCount"].ToString());
				}
				if(row["stockOutCount"]!=null && row["stockOutCount"].ToString()!="")
				{
					model.stockOutCount=int.Parse(row["stockOutCount"].ToString());
				}
				if(row["stockExtraOutCout"]!=null && row["stockExtraOutCout"].ToString()!="")
				{
					model.stockExtraOutCout=int.Parse(row["stockExtraOutCout"].ToString());
				}
				if(row["isHaveReplaceMaterial"]!=null)
				{
					model.isHaveReplaceMaterial=row["isHaveReplaceMaterial"].ToString();
				}
				if(row["replaceMaterialTempGroup"]!=null && row["replaceMaterialTempGroup"].ToString()!="")
				{
					model.replaceMaterialTempGroup=int.Parse(row["replaceMaterialTempGroup"].ToString());
				}
				if(row["priority"]!=null && row["priority"].ToString()!="")
				{
					model.priority=int.Parse(row["priority"].ToString());
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
			strSql.Append("select id,taskNo,machineTypeNo,productNo,count,bomGroupNo,materialNo,materialCount,lineWareCount,instoreCount,diffCount,creater,createTime,bin,status,shelfNo,isLineWare,lineWareOutCount,stockOutCount,stockExtraOutCout,isHaveReplaceMaterial,replaceMaterialTempGroup,priority,supplyBin ");
			strSql.Append(" FROM smtTaskComputer ");
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
			strSql.Append(" id,taskNo,machineTypeNo,productNo,count,bomGroupNo,materialNo,materialCount,lineWareCount,instoreCount,diffCount,creater,createTime,bin,status,shelfNo,isLineWare,lineWareOutCount,stockOutCount,stockExtraOutCout,isHaveReplaceMaterial,replaceMaterialTempGroup,priority,supplyBin ");
			strSql.Append(" FROM smtTaskComputer ");
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
			strSql.Append("select count(1) FROM smtTaskComputer ");
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
			strSql.Append(")AS Row, T.*  from smtTaskComputer T ");
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
			parameters[0].Value = "smtTaskComputer";
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

