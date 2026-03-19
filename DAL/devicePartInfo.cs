using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:devicePartInfo
	/// </summary>
	public partial class devicePartInfo
	{
		public devicePartInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "devicePartInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from devicePartInfo");
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
		public int Add(Model.devicePartInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into devicePartInfo(");
			strSql.Append("partNo,partName,partModel,manufacturer,contact,contactTel,stockMin,stockMax,purchaseCycle,isValid,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@partNo,@partName,@partModel,@manufacturer,@contact,@contactTel,@stockMin,@stockMax,@purchaseCycle,@isValid,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@partNo", SqlDbType.NVarChar,50),
					new SqlParameter("@partName", SqlDbType.NVarChar,50),
					new SqlParameter("@partModel", SqlDbType.NVarChar,50),
					new SqlParameter("@manufacturer", SqlDbType.NVarChar,50),
					new SqlParameter("@contact", SqlDbType.NVarChar,50),
					new SqlParameter("@contactTel", SqlDbType.NVarChar,50),
					new SqlParameter("@stockMin", SqlDbType.Int,4),
					new SqlParameter("@stockMax", SqlDbType.Int,4),
					new SqlParameter("@purchaseCycle", SqlDbType.Int,4),
					new SqlParameter("@isValid", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.partNo;
			parameters[1].Value = model.partName;
			parameters[2].Value = model.partModel;
			parameters[3].Value = model.manufacturer;
			parameters[4].Value = model.contact;
			parameters[5].Value = model.contactTel;
			parameters[6].Value = model.stockMin;
			parameters[7].Value = model.stockMax;
			parameters[8].Value = model.purchaseCycle;
			parameters[9].Value = model.isValid;
			parameters[10].Value = model.creater;
			parameters[11].Value = model.createTime;

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
		public bool Update(Model.devicePartInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update devicePartInfo set ");
			strSql.Append("partNo=@partNo,");
			strSql.Append("partName=@partName,");
			strSql.Append("partModel=@partModel,");
			strSql.Append("manufacturer=@manufacturer,");
			strSql.Append("contact=@contact,");
			strSql.Append("contactTel=@contactTel,");
			strSql.Append("stockMin=@stockMin,");
			strSql.Append("stockMax=@stockMax,");
			strSql.Append("purchaseCycle=@purchaseCycle,");
			strSql.Append("isValid=@isValid,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@partNo", SqlDbType.NVarChar,50),
					new SqlParameter("@partName", SqlDbType.NVarChar,50),
					new SqlParameter("@partModel", SqlDbType.NVarChar,50),
					new SqlParameter("@manufacturer", SqlDbType.NVarChar,50),
					new SqlParameter("@contact", SqlDbType.NVarChar,50),
					new SqlParameter("@contactTel", SqlDbType.NVarChar,50),
					new SqlParameter("@stockMin", SqlDbType.Int,4),
					new SqlParameter("@stockMax", SqlDbType.Int,4),
					new SqlParameter("@purchaseCycle", SqlDbType.Int,4),
					new SqlParameter("@isValid", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.partNo;
			parameters[1].Value = model.partName;
			parameters[2].Value = model.partModel;
			parameters[3].Value = model.manufacturer;
			parameters[4].Value = model.contact;
			parameters[5].Value = model.contactTel;
			parameters[6].Value = model.stockMin;
			parameters[7].Value = model.stockMax;
			parameters[8].Value = model.purchaseCycle;
			parameters[9].Value = model.isValid;
			parameters[10].Value = model.creater;
			parameters[11].Value = model.createTime;
			parameters[12].Value = model.id;

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
			strSql.Append("delete from devicePartInfo ");
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
			strSql.Append("delete from devicePartInfo ");
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
		public Model.devicePartInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,partNo,partName,partModel,manufacturer,contact,contactTel,stockMin,stockMax,purchaseCycle,isValid,creater,createTime from devicePartInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.devicePartInfo model=new Model.devicePartInfo();
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
		public Model.devicePartInfo DataRowToModel(DataRow row)
		{
			Model.devicePartInfo model=new Model.devicePartInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["partNo"]!=null)
				{
					model.partNo=row["partNo"].ToString();
				}
				if(row["partName"]!=null)
				{
					model.partName=row["partName"].ToString();
				}
				if(row["partModel"]!=null)
				{
					model.partModel=row["partModel"].ToString();
				}
				if(row["manufacturer"]!=null)
				{
					model.manufacturer=row["manufacturer"].ToString();
				}
				if(row["contact"]!=null)
				{
					model.contact=row["contact"].ToString();
				}
				if(row["contactTel"]!=null)
				{
					model.contactTel=row["contactTel"].ToString();
				}
				if(row["stockMin"]!=null && row["stockMin"].ToString()!="")
				{
					model.stockMin=int.Parse(row["stockMin"].ToString());
				}
				if(row["stockMax"]!=null && row["stockMax"].ToString()!="")
				{
					model.stockMax=int.Parse(row["stockMax"].ToString());
				}
				if(row["purchaseCycle"]!=null && row["purchaseCycle"].ToString()!="")
				{
					model.purchaseCycle=int.Parse(row["purchaseCycle"].ToString());
				}
				if(row["isValid"]!=null)
				{
					model.isValid=row["isValid"].ToString();
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
			strSql.Append("select id,partNo,partName,partModel,manufacturer,contact,contactTel,stockMin,stockMax,purchaseCycle,isValid,creater,createTime ");
			strSql.Append(" FROM devicePartInfo ");
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
			strSql.Append(" id,partNo,partName,partModel,manufacturer,contact,contactTel,stockMin,stockMax,purchaseCycle,isValid,creater,createTime ");
			strSql.Append(" FROM devicePartInfo ");
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
			strSql.Append("select count(1) FROM devicePartInfo ");
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
			strSql.Append(")AS Row, T.*  from devicePartInfo T ");
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
			parameters[0].Value = "devicePartInfo";
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

