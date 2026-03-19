using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:smtBomInfo
	/// </summary>
	public partial class smtBomInfo
	{
		public smtBomInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "smtBomInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from smtBomInfo");
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
		public int Add(Model.smtBomInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into smtBomInfo(");
			strSql.Append("machineTypeNo,productNO,bomGroupNo,materialNo,materialName,materialSubNo,count,replaceGroup,priority,advancePriority,creater,createTime,materialSubName,isCM,solderpasteType)");
			strSql.Append(" values (");
			strSql.Append("@machineTypeNo,@productNO,@bomGroupNo,@materialNo,@materialName,@materialSubNo,@count,@replaceGroup,@priority,@advancePriority,@creater,@createTime,@materialSubName,@isCM,@solderpasteType)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@machineTypeNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNO", SqlDbType.NVarChar,50),
					new SqlParameter("@bomGroupNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialName", SqlDbType.NVarChar,50),
					new SqlParameter("@materialSubNo", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.Int,4),
					new SqlParameter("@replaceGroup", SqlDbType.Int,4),
					new SqlParameter("@priority", SqlDbType.Int,4),
					new SqlParameter("@advancePriority", SqlDbType.Int,4),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@materialSubName", SqlDbType.NVarChar,50),
					new SqlParameter("@isCM", SqlDbType.NVarChar,10),
					new SqlParameter("@solderpasteType", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.machineTypeNo;
			parameters[1].Value = model.productNO;
			parameters[2].Value = model.bomGroupNo;
			parameters[3].Value = model.materialNo;
			parameters[4].Value = model.materialName;
			parameters[5].Value = model.materialSubNo;
			parameters[6].Value = model.count;
			parameters[7].Value = model.replaceGroup;
			parameters[8].Value = model.priority;
			parameters[9].Value = model.advancePriority;
			parameters[10].Value = model.creater;
			parameters[11].Value = model.createTime;
			parameters[12].Value = model.materialSubName;
			parameters[13].Value = model.isCM;
			parameters[14].Value = model.solderpasteType;

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
		public bool Update(Model.smtBomInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update smtBomInfo set ");
			strSql.Append("machineTypeNo=@machineTypeNo,");
			strSql.Append("productNO=@productNO,");
			strSql.Append("bomGroupNo=@bomGroupNo,");
			strSql.Append("materialNo=@materialNo,");
			strSql.Append("materialName=@materialName,");
			strSql.Append("materialSubNo=@materialSubNo,");
			strSql.Append("count=@count,");
			strSql.Append("replaceGroup=@replaceGroup,");
			strSql.Append("priority=@priority,");
			strSql.Append("advancePriority=@advancePriority,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("materialSubName=@materialSubName,");
			strSql.Append("isCM=@isCM,");
			strSql.Append("solderpasteType=@solderpasteType");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@machineTypeNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNO", SqlDbType.NVarChar,50),
					new SqlParameter("@bomGroupNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialName", SqlDbType.NVarChar,50),
					new SqlParameter("@materialSubNo", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.Int,4),
					new SqlParameter("@replaceGroup", SqlDbType.Int,4),
					new SqlParameter("@priority", SqlDbType.Int,4),
					new SqlParameter("@advancePriority", SqlDbType.Int,4),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@materialSubName", SqlDbType.NVarChar,50),
					new SqlParameter("@isCM", SqlDbType.NVarChar,10),
					new SqlParameter("@solderpasteType", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.machineTypeNo;
			parameters[1].Value = model.productNO;
			parameters[2].Value = model.bomGroupNo;
			parameters[3].Value = model.materialNo;
			parameters[4].Value = model.materialName;
			parameters[5].Value = model.materialSubNo;
			parameters[6].Value = model.count;
			parameters[7].Value = model.replaceGroup;
			parameters[8].Value = model.priority;
			parameters[9].Value = model.advancePriority;
			parameters[10].Value = model.creater;
			parameters[11].Value = model.createTime;
			parameters[12].Value = model.materialSubName;
			parameters[13].Value = model.isCM;
			parameters[14].Value = model.solderpasteType;
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
			strSql.Append("delete from smtBomInfo ");
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
			strSql.Append("delete from smtBomInfo ");
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
		public Model.smtBomInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,machineTypeNo,productNO,bomGroupNo,materialNo,materialName,materialSubNo,count,replaceGroup,priority,advancePriority,creater,createTime,materialSubName,isCM,solderpasteType from smtBomInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.smtBomInfo model=new Model.smtBomInfo();
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
		public Model.smtBomInfo DataRowToModel(DataRow row)
		{
			Model.smtBomInfo model=new Model.smtBomInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["machineTypeNo"]!=null)
				{
					model.machineTypeNo=row["machineTypeNo"].ToString();
				}
				if(row["productNO"]!=null)
				{
					model.productNO=row["productNO"].ToString();
				}
				if(row["bomGroupNo"]!=null)
				{
					model.bomGroupNo=row["bomGroupNo"].ToString();
				}
				if(row["materialNo"]!=null)
				{
					model.materialNo=row["materialNo"].ToString();
				}
				if(row["materialName"]!=null)
				{
					model.materialName=row["materialName"].ToString();
				}
				if(row["materialSubNo"]!=null)
				{
					model.materialSubNo=row["materialSubNo"].ToString();
				}
				if(row["count"]!=null && row["count"].ToString()!="")
				{
					model.count=int.Parse(row["count"].ToString());
				}
				if(row["replaceGroup"]!=null && row["replaceGroup"].ToString()!="")
				{
					model.replaceGroup=int.Parse(row["replaceGroup"].ToString());
				}
				if(row["priority"]!=null && row["priority"].ToString()!="")
				{
					model.priority=int.Parse(row["priority"].ToString());
				}
				if(row["advancePriority"]!=null && row["advancePriority"].ToString()!="")
				{
					model.advancePriority=int.Parse(row["advancePriority"].ToString());
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["materialSubName"]!=null)
				{
					model.materialSubName=row["materialSubName"].ToString();
				}
				if(row["isCM"]!=null)
				{
					model.isCM=row["isCM"].ToString();
				}
				if(row["solderpasteType"]!=null)
				{
					model.solderpasteType=row["solderpasteType"].ToString();
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
			strSql.Append("select id,machineTypeNo,productNO,bomGroupNo,materialNo,materialName,materialSubNo,count,replaceGroup,priority,advancePriority,creater,createTime,materialSubName,isCM,solderpasteType ");
			strSql.Append(" FROM smtBomInfo ");
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
			strSql.Append(" id,machineTypeNo,productNO,bomGroupNo,materialNo,materialName,materialSubNo,count,replaceGroup,priority,advancePriority,creater,createTime,materialSubName,isCM,solderpasteType ");
			strSql.Append(" FROM smtBomInfo ");
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
			strSql.Append("select count(1) FROM smtBomInfo ");
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
			strSql.Append(")AS Row, T.*  from smtBomInfo T ");
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
			parameters[0].Value = "smtBomInfo";
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

