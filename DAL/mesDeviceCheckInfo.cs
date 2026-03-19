using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:mesDeviceCheckInfo
	/// </summary>
	public partial class mesDeviceCheckInfo
	{
		public mesDeviceCheckInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "mesDeviceCheckInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from mesDeviceCheckInfo");
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
		public int Add(Model.mesDeviceCheckInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into mesDeviceCheckInfo(");
			strSql.Append("department,deviceNo,assetNo,deviceName,address,note,checkoperator,checkdate,status,remark,flag,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@department,@deviceNo,@assetNo,@deviceName,@address,@note,@checkoperator,@checkdate,@status,@remark,@flag,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@department", SqlDbType.NVarChar,50),
					new SqlParameter("@deviceNo", SqlDbType.NVarChar,50),
					new SqlParameter("@assetNo", SqlDbType.NVarChar,50),
					new SqlParameter("@deviceName", SqlDbType.NVarChar,100),
					new SqlParameter("@address", SqlDbType.NVarChar,100),
					new SqlParameter("@note", SqlDbType.NVarChar,50),
					new SqlParameter("@checkoperator", SqlDbType.NVarChar,50),
					new SqlParameter("@checkdate", SqlDbType.DateTime),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,50),
					new SqlParameter("@flag", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.department;
			parameters[1].Value = model.deviceNo;
			parameters[2].Value = model.assetNo;
			parameters[3].Value = model.deviceName;
			parameters[4].Value = model.address;
			parameters[5].Value = model.note;
			parameters[6].Value = model.checkoperator;
			parameters[7].Value = model.checkdate;
			parameters[8].Value = model.status;
			parameters[9].Value = model.remark;
			parameters[10].Value = model.flag;
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
		public bool Update(Model.mesDeviceCheckInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update mesDeviceCheckInfo set ");
			strSql.Append("department=@department,");
			strSql.Append("deviceNo=@deviceNo,");
			strSql.Append("assetNo=@assetNo,");
			strSql.Append("deviceName=@deviceName,");
			strSql.Append("address=@address,");
			strSql.Append("note=@note,");
			strSql.Append("checkoperator=@checkoperator,");
			strSql.Append("checkdate=@checkdate,");
			strSql.Append("status=@status,");
			strSql.Append("remark=@remark,");
			strSql.Append("flag=@flag,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@department", SqlDbType.NVarChar,50),
					new SqlParameter("@deviceNo", SqlDbType.NVarChar,50),
					new SqlParameter("@assetNo", SqlDbType.NVarChar,50),
					new SqlParameter("@deviceName", SqlDbType.NVarChar,100),
					new SqlParameter("@address", SqlDbType.NVarChar,100),
					new SqlParameter("@note", SqlDbType.NVarChar,50),
					new SqlParameter("@checkoperator", SqlDbType.NVarChar,50),
					new SqlParameter("@checkdate", SqlDbType.DateTime),
					new SqlParameter("@status", SqlDbType.NVarChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,50),
					new SqlParameter("@flag", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.department;
			parameters[1].Value = model.deviceNo;
			parameters[2].Value = model.assetNo;
			parameters[3].Value = model.deviceName;
			parameters[4].Value = model.address;
			parameters[5].Value = model.note;
			parameters[6].Value = model.checkoperator;
			parameters[7].Value = model.checkdate;
			parameters[8].Value = model.status;
			parameters[9].Value = model.remark;
			parameters[10].Value = model.flag;
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
			strSql.Append("delete from mesDeviceCheckInfo ");
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
			strSql.Append("delete from mesDeviceCheckInfo ");
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
		public Model.mesDeviceCheckInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,department,deviceNo,assetNo,deviceName,address,note,checkoperator,checkdate,status,remark,flag,creater,createTime from mesDeviceCheckInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.mesDeviceCheckInfo model=new Model.mesDeviceCheckInfo();
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
		public Model.mesDeviceCheckInfo DataRowToModel(DataRow row)
		{
			Model.mesDeviceCheckInfo model=new Model.mesDeviceCheckInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["department"]!=null)
				{
					model.department=row["department"].ToString();
				}
				if(row["deviceNo"]!=null)
				{
					model.deviceNo=row["deviceNo"].ToString();
				}
				if(row["assetNo"]!=null)
				{
					model.assetNo=row["assetNo"].ToString();
				}
				if(row["deviceName"]!=null)
				{
					model.deviceName=row["deviceName"].ToString();
				}
				if(row["address"]!=null)
				{
					model.address=row["address"].ToString();
				}
				if(row["note"]!=null)
				{
					model.note=row["note"].ToString();
				}
				if(row["checkoperator"]!=null)
				{
					model.checkoperator=row["checkoperator"].ToString();
				}
				if(row["checkdate"]!=null && row["checkdate"].ToString()!="")
				{
					model.checkdate=DateTime.Parse(row["checkdate"].ToString());
				}
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["remark"]!=null)
				{
					model.remark=row["remark"].ToString();
				}
				if(row["flag"]!=null)
				{
					model.flag=row["flag"].ToString();
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
			strSql.Append("select id,department,deviceNo,assetNo,deviceName,address,note,checkoperator,checkdate,status,remark,flag,creater,createTime ");
			strSql.Append(" FROM mesDeviceCheckInfo ");
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
			strSql.Append(" id,department,deviceNo,assetNo,deviceName,address,note,checkoperator,checkdate,status,remark,flag,creater,createTime ");
			strSql.Append(" FROM mesDeviceCheckInfo ");
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
			strSql.Append("select count(1) FROM mesDeviceCheckInfo ");
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
			strSql.Append(")AS Row, T.*  from mesDeviceCheckInfo T ");
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
			parameters[0].Value = "mesDeviceCheckInfo";
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

