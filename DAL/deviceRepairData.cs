using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:deviceRepairData
	/// </summary>
	public partial class deviceRepairData
	{
		public deviceRepairData()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "deviceRepairData"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from deviceRepairData");
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
		public int Add(Model.deviceRepairData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into deviceRepairData(");
			strSql.Append("repairNo,deviceNo,failureDetail,repairDetail,onTime,offTime,repairer,lessonNote,creater,createTIme)");
			strSql.Append(" values (");
			strSql.Append("@repairNo,@deviceNo,@failureDetail,@repairDetail,@onTime,@offTime,@repairer,@lessonNote,@creater,@createTIme)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@repairNo", SqlDbType.NVarChar,50),
					new SqlParameter("@deviceNo", SqlDbType.NVarChar,50),
					new SqlParameter("@failureDetail", SqlDbType.NVarChar,50),
					new SqlParameter("@repairDetail", SqlDbType.NVarChar,50),
					new SqlParameter("@onTime", SqlDbType.DateTime),
					new SqlParameter("@offTime", SqlDbType.DateTime),
					new SqlParameter("@repairer", SqlDbType.NVarChar,50),
					new SqlParameter("@lessonNote", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTIme", SqlDbType.DateTime)};
			parameters[0].Value = model.repairNo;
			parameters[1].Value = model.deviceNo;
			parameters[2].Value = model.failureDetail;
			parameters[3].Value = model.repairDetail;
			parameters[4].Value = model.onTime;
			parameters[5].Value = model.offTime;
			parameters[6].Value = model.repairer;
			parameters[7].Value = model.lessonNote;
			parameters[8].Value = model.creater;
			parameters[9].Value = model.createTIme;

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
		public bool Update(Model.deviceRepairData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update deviceRepairData set ");
			strSql.Append("repairNo=@repairNo,");
			strSql.Append("deviceNo=@deviceNo,");
			strSql.Append("failureDetail=@failureDetail,");
			strSql.Append("repairDetail=@repairDetail,");
			strSql.Append("onTime=@onTime,");
			strSql.Append("offTime=@offTime,");
			strSql.Append("repairer=@repairer,");
			strSql.Append("lessonNote=@lessonNote,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTIme=@createTIme");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@repairNo", SqlDbType.NVarChar,50),
					new SqlParameter("@deviceNo", SqlDbType.NVarChar,50),
					new SqlParameter("@failureDetail", SqlDbType.NVarChar,50),
					new SqlParameter("@repairDetail", SqlDbType.NVarChar,50),
					new SqlParameter("@onTime", SqlDbType.DateTime),
					new SqlParameter("@offTime", SqlDbType.DateTime),
					new SqlParameter("@repairer", SqlDbType.NVarChar,50),
					new SqlParameter("@lessonNote", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTIme", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.repairNo;
			parameters[1].Value = model.deviceNo;
			parameters[2].Value = model.failureDetail;
			parameters[3].Value = model.repairDetail;
			parameters[4].Value = model.onTime;
			parameters[5].Value = model.offTime;
			parameters[6].Value = model.repairer;
			parameters[7].Value = model.lessonNote;
			parameters[8].Value = model.creater;
			parameters[9].Value = model.createTIme;
			parameters[10].Value = model.id;

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
			strSql.Append("delete from deviceRepairData ");
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
			strSql.Append("delete from deviceRepairData ");
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
		public Model.deviceRepairData GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,repairNo,deviceNo,failureDetail,repairDetail,onTime,offTime,repairer,lessonNote,creater,createTIme from deviceRepairData ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.deviceRepairData model=new Model.deviceRepairData();
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
		public Model.deviceRepairData DataRowToModel(DataRow row)
		{
			Model.deviceRepairData model=new Model.deviceRepairData();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["repairNo"]!=null)
				{
					model.repairNo=row["repairNo"].ToString();
				}
				if(row["deviceNo"]!=null)
				{
					model.deviceNo=row["deviceNo"].ToString();
				}
				if(row["failureDetail"]!=null)
				{
					model.failureDetail=row["failureDetail"].ToString();
				}
				if(row["repairDetail"]!=null)
				{
					model.repairDetail=row["repairDetail"].ToString();
				}
				if(row["onTime"]!=null && row["onTime"].ToString()!="")
				{
					model.onTime=DateTime.Parse(row["onTime"].ToString());
				}
				if(row["offTime"]!=null && row["offTime"].ToString()!="")
				{
					model.offTime=DateTime.Parse(row["offTime"].ToString());
				}
				if(row["repairer"]!=null)
				{
					model.repairer=row["repairer"].ToString();
				}
				if(row["lessonNote"]!=null)
				{
					model.lessonNote=row["lessonNote"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTIme"]!=null && row["createTIme"].ToString()!="")
				{
					model.createTIme=DateTime.Parse(row["createTIme"].ToString());
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
			strSql.Append("select id,repairNo,deviceNo,failureDetail,repairDetail,onTime,offTime,repairer,lessonNote,creater,createTIme ");
			strSql.Append(" FROM deviceRepairData ");
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
			strSql.Append(" id,repairNo,deviceNo,failureDetail,repairDetail,onTime,offTime,repairer,lessonNote,creater,createTIme ");
			strSql.Append(" FROM deviceRepairData ");
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
			strSql.Append("select count(1) FROM deviceRepairData ");
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
			strSql.Append(")AS Row, T.*  from deviceRepairData T ");
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
			parameters[0].Value = "deviceRepairData";
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

