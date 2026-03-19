using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:qaProductSmsInfo
	/// </summary>
	public partial class qaProductSmsInfo
	{
		public qaProductSmsInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "qaProductSmsInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from qaProductSmsInfo");
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
		public int Add(Model.qaProductSmsInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into qaProductSmsInfo(");
			strSql.Append("productNo,processNo,statDate,statQuantum,sendUser,reason,remark,status,smsStatus,dealer,creater,createTime,basicNum,EquipQTY,QualityQTY,WorkshopQTY,OtherQTY,ResetTime,FinishTime)");
			strSql.Append(" values (");
			strSql.Append("@productNo,@processNo,@statDate,@statQuantum,@sendUser,@reason,@remark,@status,@smsStatus,@dealer,@creater,@createTime,@basicNum,@EquipQTY,@QualityQTY,@WorkshopQTY,@OtherQTY,@ResetTime,@FinishTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@statDate", SqlDbType.NVarChar,50),
					new SqlParameter("@statQuantum", SqlDbType.Int,4),
					new SqlParameter("@sendUser", SqlDbType.NVarChar,50),
					new SqlParameter("@reason", SqlDbType.NVarChar,500),
					new SqlParameter("@remark", SqlDbType.NVarChar,500),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@smsStatus", SqlDbType.Int,4),
					new SqlParameter("@dealer", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@basicNum", SqlDbType.Int,4),
					new SqlParameter("@EquipQTY", SqlDbType.Int,4),
					new SqlParameter("@QualityQTY", SqlDbType.Int,4),
					new SqlParameter("@WorkshopQTY", SqlDbType.Int,4),
					new SqlParameter("@OtherQTY", SqlDbType.Int,4),
					new SqlParameter("@ResetTime", SqlDbType.DateTime),
					new SqlParameter("@FinishTime", SqlDbType.DateTime)};
			parameters[0].Value = model.productNo;
			parameters[1].Value = model.processNo;
			parameters[2].Value = model.statDate;
			parameters[3].Value = model.statQuantum;
			parameters[4].Value = model.sendUser;
			parameters[5].Value = model.reason;
			parameters[6].Value = model.remark;
			parameters[7].Value = model.status;
			parameters[8].Value = model.smsStatus;
			parameters[9].Value = model.dealer;
			parameters[10].Value = model.creater;
			parameters[11].Value = model.createTime;
			parameters[12].Value = model.basicNum;
			parameters[13].Value = model.EquipQTY;
			parameters[14].Value = model.QualityQTY;
			parameters[15].Value = model.WorkshopQTY;
			parameters[16].Value = model.OtherQTY;
			parameters[17].Value = model.ResetTime;
			parameters[18].Value = model.FinishTime;

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
		public bool Update(Model.qaProductSmsInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update qaProductSmsInfo set ");
			strSql.Append("productNo=@productNo,");
			strSql.Append("processNo=@processNo,");
			strSql.Append("statDate=@statDate,");
			strSql.Append("statQuantum=@statQuantum,");
			strSql.Append("sendUser=@sendUser,");
			strSql.Append("reason=@reason,");
			strSql.Append("remark=@remark,");
			strSql.Append("status=@status,");
			strSql.Append("smsStatus=@smsStatus,");
			strSql.Append("dealer=@dealer,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("basicNum=@basicNum,");
			strSql.Append("EquipQTY=@EquipQTY,");
			strSql.Append("QualityQTY=@QualityQTY,");
			strSql.Append("WorkshopQTY=@WorkshopQTY,");
			strSql.Append("OtherQTY=@OtherQTY,");
			strSql.Append("ResetTime=@ResetTime,");
			strSql.Append("FinishTime=@FinishTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@statDate", SqlDbType.NVarChar,50),
					new SqlParameter("@statQuantum", SqlDbType.Int,4),
					new SqlParameter("@sendUser", SqlDbType.NVarChar,50),
					new SqlParameter("@reason", SqlDbType.NVarChar,500),
					new SqlParameter("@remark", SqlDbType.NVarChar,500),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@smsStatus", SqlDbType.Int,4),
					new SqlParameter("@dealer", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@basicNum", SqlDbType.Int,4),
					new SqlParameter("@EquipQTY", SqlDbType.Int,4),
					new SqlParameter("@QualityQTY", SqlDbType.Int,4),
					new SqlParameter("@WorkshopQTY", SqlDbType.Int,4),
					new SqlParameter("@OtherQTY", SqlDbType.Int,4),
					new SqlParameter("@ResetTime", SqlDbType.DateTime),
					new SqlParameter("@FinishTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.productNo;
			parameters[1].Value = model.processNo;
			parameters[2].Value = model.statDate;
			parameters[3].Value = model.statQuantum;
			parameters[4].Value = model.sendUser;
			parameters[5].Value = model.reason;
			parameters[6].Value = model.remark;
			parameters[7].Value = model.status;
			parameters[8].Value = model.smsStatus;
			parameters[9].Value = model.dealer;
			parameters[10].Value = model.creater;
			parameters[11].Value = model.createTime;
			parameters[12].Value = model.basicNum;
			parameters[13].Value = model.EquipQTY;
			parameters[14].Value = model.QualityQTY;
			parameters[15].Value = model.WorkshopQTY;
			parameters[16].Value = model.OtherQTY;
			parameters[17].Value = model.ResetTime;
			parameters[18].Value = model.FinishTime;
			parameters[19].Value = model.id;

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
			strSql.Append("delete from qaProductSmsInfo ");
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
			strSql.Append("delete from qaProductSmsInfo ");
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
		public Model.qaProductSmsInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,productNo,processNo,statDate,statQuantum,sendUser,reason,remark,status,smsStatus,dealer,creater,createTime,basicNum,EquipQTY,QualityQTY,WorkshopQTY,OtherQTY,ResetTime,FinishTime from qaProductSmsInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.qaProductSmsInfo model=new Model.qaProductSmsInfo();
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
		public Model.qaProductSmsInfo DataRowToModel(DataRow row)
		{
			Model.qaProductSmsInfo model=new Model.qaProductSmsInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["processNo"]!=null)
				{
					model.processNo=row["processNo"].ToString();
				}
				if(row["statDate"]!=null)
				{
					model.statDate=row["statDate"].ToString();
				}
				if(row["statQuantum"]!=null && row["statQuantum"].ToString()!="")
				{
					model.statQuantum=int.Parse(row["statQuantum"].ToString());
				}
				if(row["sendUser"]!=null)
				{
					model.sendUser=row["sendUser"].ToString();
				}
				if(row["reason"]!=null)
				{
					model.reason=row["reason"].ToString();
				}
				if(row["remark"]!=null)
				{
					model.remark=row["remark"].ToString();
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["smsStatus"]!=null && row["smsStatus"].ToString()!="")
				{
					model.smsStatus=int.Parse(row["smsStatus"].ToString());
				}
				if(row["dealer"]!=null)
				{
					model.dealer=row["dealer"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["basicNum"]!=null && row["basicNum"].ToString()!="")
				{
					model.basicNum=int.Parse(row["basicNum"].ToString());
				}
				if(row["EquipQTY"]!=null && row["EquipQTY"].ToString()!="")
				{
					model.EquipQTY=int.Parse(row["EquipQTY"].ToString());
				}
				if(row["QualityQTY"]!=null && row["QualityQTY"].ToString()!="")
				{
					model.QualityQTY=int.Parse(row["QualityQTY"].ToString());
				}
				if(row["WorkshopQTY"]!=null && row["WorkshopQTY"].ToString()!="")
				{
					model.WorkshopQTY=int.Parse(row["WorkshopQTY"].ToString());
				}
				if(row["OtherQTY"]!=null && row["OtherQTY"].ToString()!="")
				{
					model.OtherQTY=int.Parse(row["OtherQTY"].ToString());
				}
				if(row["ResetTime"]!=null && row["ResetTime"].ToString()!="")
				{
					model.ResetTime=DateTime.Parse(row["ResetTime"].ToString());
				}
				if(row["FinishTime"]!=null && row["FinishTime"].ToString()!="")
				{
					model.FinishTime=DateTime.Parse(row["FinishTime"].ToString());
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
			strSql.Append("select id,productNo,processNo,statDate,statQuantum,sendUser,reason,remark,status,smsStatus,dealer,creater,createTime,basicNum,EquipQTY,QualityQTY,WorkshopQTY,OtherQTY,ResetTime,FinishTime ");
			strSql.Append(" FROM qaProductSmsInfo ");
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
			strSql.Append(" id,productNo,processNo,statDate,statQuantum,sendUser,reason,remark,status,smsStatus,dealer,creater,createTime,basicNum,EquipQTY,QualityQTY,WorkshopQTY,OtherQTY,ResetTime,FinishTime ");
			strSql.Append(" FROM qaProductSmsInfo ");
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
			strSql.Append("select count(1) FROM qaProductSmsInfo ");
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
			strSql.Append(")AS Row, T.*  from qaProductSmsInfo T ");
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
			parameters[0].Value = "qaProductSmsInfo";
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

