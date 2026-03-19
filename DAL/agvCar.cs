using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:agvCar
	/// </summary>
	public partial class agvCar
	{
		public agvCar()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "agvCar"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from agvCar");
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
		public int Add(Model.agvCar model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into agvCar(");
			strSql.Append("agvLabel,agvBattery,agvPortNum,agvRunDist,agvCargoLabel,agvSpeed,isUsed,hasCargo,agvTask,agvCurRunStatus,agvAlarmInfo,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@agvLabel,@agvBattery,@agvPortNum,@agvRunDist,@agvCargoLabel,@agvSpeed,@isUsed,@hasCargo,@agvTask,@agvCurRunStatus,@agvAlarmInfo,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@agvLabel", SqlDbType.Int,4),
					new SqlParameter("@agvBattery", SqlDbType.Int,4),
					new SqlParameter("@agvPortNum", SqlDbType.Int,4),
					new SqlParameter("@agvRunDist", SqlDbType.Int,4),
					new SqlParameter("@agvCargoLabel", SqlDbType.Int,4),
					new SqlParameter("@agvSpeed", SqlDbType.Float,8),
					new SqlParameter("@isUsed", SqlDbType.Int,4),
					new SqlParameter("@hasCargo", SqlDbType.Int,4),
					new SqlParameter("@agvTask", SqlDbType.NVarChar,50),
					new SqlParameter("@agvCurRunStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@agvAlarmInfo", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.agvLabel;
			parameters[1].Value = model.agvBattery;
			parameters[2].Value = model.agvPortNum;
			parameters[3].Value = model.agvRunDist;
			parameters[4].Value = model.agvCargoLabel;
			parameters[5].Value = model.agvSpeed;
			parameters[6].Value = model.isUsed;
			parameters[7].Value = model.hasCargo;
			parameters[8].Value = model.agvTask;
			parameters[9].Value = model.agvCurRunStatus;
			parameters[10].Value = model.agvAlarmInfo;
			parameters[11].Value = model.creater;
			parameters[12].Value = model.createTime;

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
		public bool Update(Model.agvCar model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update agvCar set ");
			strSql.Append("agvLabel=@agvLabel,");
			strSql.Append("agvBattery=@agvBattery,");
			strSql.Append("agvPortNum=@agvPortNum,");
			strSql.Append("agvRunDist=@agvRunDist,");
			strSql.Append("agvCargoLabel=@agvCargoLabel,");
			strSql.Append("agvSpeed=@agvSpeed,");
			strSql.Append("isUsed=@isUsed,");
			strSql.Append("hasCargo=@hasCargo,");
			strSql.Append("agvTask=@agvTask,");
			strSql.Append("agvCurRunStatus=@agvCurRunStatus,");
			strSql.Append("agvAlarmInfo=@agvAlarmInfo,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@agvLabel", SqlDbType.Int,4),
					new SqlParameter("@agvBattery", SqlDbType.Int,4),
					new SqlParameter("@agvPortNum", SqlDbType.Int,4),
					new SqlParameter("@agvRunDist", SqlDbType.Int,4),
					new SqlParameter("@agvCargoLabel", SqlDbType.Int,4),
					new SqlParameter("@agvSpeed", SqlDbType.Float,8),
					new SqlParameter("@isUsed", SqlDbType.Int,4),
					new SqlParameter("@hasCargo", SqlDbType.Int,4),
					new SqlParameter("@agvTask", SqlDbType.NVarChar,50),
					new SqlParameter("@agvCurRunStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@agvAlarmInfo", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.agvLabel;
			parameters[1].Value = model.agvBattery;
			parameters[2].Value = model.agvPortNum;
			parameters[3].Value = model.agvRunDist;
			parameters[4].Value = model.agvCargoLabel;
			parameters[5].Value = model.agvSpeed;
			parameters[6].Value = model.isUsed;
			parameters[7].Value = model.hasCargo;
			parameters[8].Value = model.agvTask;
			parameters[9].Value = model.agvCurRunStatus;
			parameters[10].Value = model.agvAlarmInfo;
			parameters[11].Value = model.creater;
			parameters[12].Value = model.createTime;
			parameters[13].Value = model.id;

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
			strSql.Append("delete from agvCar ");
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
			strSql.Append("delete from agvCar ");
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
		public Model.agvCar GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,agvLabel,agvBattery,agvPortNum,agvRunDist,agvCargoLabel,agvSpeed,isUsed,hasCargo,agvTask,agvCurRunStatus,agvAlarmInfo,creater,createTime from agvCar ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.agvCar model=new Model.agvCar();
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
		public Model.agvCar DataRowToModel(DataRow row)
		{
			Model.agvCar model=new Model.agvCar();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["agvLabel"]!=null && row["agvLabel"].ToString()!="")
				{
					model.agvLabel=int.Parse(row["agvLabel"].ToString());
				}
				if(row["agvBattery"]!=null && row["agvBattery"].ToString()!="")
				{
					model.agvBattery=int.Parse(row["agvBattery"].ToString());
				}
				if(row["agvPortNum"]!=null && row["agvPortNum"].ToString()!="")
				{
					model.agvPortNum=int.Parse(row["agvPortNum"].ToString());
				}
				if(row["agvRunDist"]!=null && row["agvRunDist"].ToString()!="")
				{
					model.agvRunDist=int.Parse(row["agvRunDist"].ToString());
				}
				if(row["agvCargoLabel"]!=null && row["agvCargoLabel"].ToString()!="")
				{
					model.agvCargoLabel=int.Parse(row["agvCargoLabel"].ToString());
				}
				if(row["agvSpeed"]!=null && row["agvSpeed"].ToString()!="")
				{
					model.agvSpeed=decimal.Parse(row["agvSpeed"].ToString());
				}
				if(row["isUsed"]!=null && row["isUsed"].ToString()!="")
				{
					model.isUsed=int.Parse(row["isUsed"].ToString());
				}
				if(row["hasCargo"]!=null && row["hasCargo"].ToString()!="")
				{
					model.hasCargo=int.Parse(row["hasCargo"].ToString());
				}
				if(row["agvTask"]!=null)
				{
					model.agvTask=row["agvTask"].ToString();
				}
				if(row["agvCurRunStatus"]!=null)
				{
					model.agvCurRunStatus=row["agvCurRunStatus"].ToString();
				}
				if(row["agvAlarmInfo"]!=null)
				{
					model.agvAlarmInfo=row["agvAlarmInfo"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
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
			strSql.Append("select id,agvLabel,agvBattery,agvPortNum,agvRunDist,agvCargoLabel,agvSpeed,isUsed,hasCargo,agvTask,agvCurRunStatus,agvAlarmInfo,creater,createTime ");
			strSql.Append(" FROM agvCar ");
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
			strSql.Append(" id,agvLabel,agvBattery,agvPortNum,agvRunDist,agvCargoLabel,agvSpeed,isUsed,hasCargo,agvTask,agvCurRunStatus,agvAlarmInfo,creater,createTime ");
			strSql.Append(" FROM agvCar ");
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
			strSql.Append("select count(1) FROM agvCar ");
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
			strSql.Append(")AS Row, T.*  from agvCar T ");
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
			parameters[0].Value = "agvCar";
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

