using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:smtReplaceMaterial
	/// </summary>
	public partial class smtReplaceMaterial
	{
		public smtReplaceMaterial()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "smtReplaceMaterial"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from smtReplaceMaterial");
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
		public int Add(Model.smtReplaceMaterial model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into smtReplaceMaterial(");
			strSql.Append("productNo,productName,componentsNo,oldComponentsNo,specification,skPictureNo,brand,count,unit,supplier,positionNo,memo,priority,mainMaterial,isCM)");
			strSql.Append(" values (");
			strSql.Append("@productNo,@productName,@componentsNo,@oldComponentsNo,@specification,@skPictureNo,@brand,@count,@unit,@supplier,@positionNo,@memo,@priority,@mainMaterial,@isCM)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,50),
					new SqlParameter("@componentsNo", SqlDbType.NVarChar,50),
					new SqlParameter("@oldComponentsNo", SqlDbType.NVarChar,50),
					new SqlParameter("@specification", SqlDbType.NVarChar,50),
					new SqlParameter("@skPictureNo", SqlDbType.NVarChar,50),
					new SqlParameter("@brand", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.NVarChar,50),
					new SqlParameter("@unit", SqlDbType.NVarChar,50),
					new SqlParameter("@supplier", SqlDbType.NVarChar,50),
					new SqlParameter("@positionNo", SqlDbType.NVarChar,50),
					new SqlParameter("@memo", SqlDbType.NVarChar,50),
					new SqlParameter("@priority", SqlDbType.NVarChar,50),
					new SqlParameter("@mainMaterial", SqlDbType.NVarChar,50),
					new SqlParameter("@isCM", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.productNo;
			parameters[1].Value = model.productName;
			parameters[2].Value = model.componentsNo;
			parameters[3].Value = model.oldComponentsNo;
			parameters[4].Value = model.specification;
			parameters[5].Value = model.skPictureNo;
			parameters[6].Value = model.brand;
			parameters[7].Value = model.count;
			parameters[8].Value = model.unit;
			parameters[9].Value = model.supplier;
			parameters[10].Value = model.positionNo;
			parameters[11].Value = model.memo;
			parameters[12].Value = model.priority;
			parameters[13].Value = model.mainMaterial;
			parameters[14].Value = model.isCM;

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
		public bool Update(Model.smtReplaceMaterial model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update smtReplaceMaterial set ");
			strSql.Append("productNo=@productNo,");
			strSql.Append("productName=@productName,");
			strSql.Append("componentsNo=@componentsNo,");
			strSql.Append("oldComponentsNo=@oldComponentsNo,");
			strSql.Append("specification=@specification,");
			strSql.Append("skPictureNo=@skPictureNo,");
			strSql.Append("brand=@brand,");
			strSql.Append("count=@count,");
			strSql.Append("unit=@unit,");
			strSql.Append("supplier=@supplier,");
			strSql.Append("positionNo=@positionNo,");
			strSql.Append("memo=@memo,");
			strSql.Append("priority=@priority,");
			strSql.Append("mainMaterial=@mainMaterial,");
			strSql.Append("isCM=@isCM");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,50),
					new SqlParameter("@componentsNo", SqlDbType.NVarChar,50),
					new SqlParameter("@oldComponentsNo", SqlDbType.NVarChar,50),
					new SqlParameter("@specification", SqlDbType.NVarChar,50),
					new SqlParameter("@skPictureNo", SqlDbType.NVarChar,50),
					new SqlParameter("@brand", SqlDbType.NVarChar,50),
					new SqlParameter("@count", SqlDbType.NVarChar,50),
					new SqlParameter("@unit", SqlDbType.NVarChar,50),
					new SqlParameter("@supplier", SqlDbType.NVarChar,50),
					new SqlParameter("@positionNo", SqlDbType.NVarChar,50),
					new SqlParameter("@memo", SqlDbType.NVarChar,50),
					new SqlParameter("@priority", SqlDbType.NVarChar,50),
					new SqlParameter("@mainMaterial", SqlDbType.NVarChar,50),
					new SqlParameter("@isCM", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.productNo;
			parameters[1].Value = model.productName;
			parameters[2].Value = model.componentsNo;
			parameters[3].Value = model.oldComponentsNo;
			parameters[4].Value = model.specification;
			parameters[5].Value = model.skPictureNo;
			parameters[6].Value = model.brand;
			parameters[7].Value = model.count;
			parameters[8].Value = model.unit;
			parameters[9].Value = model.supplier;
			parameters[10].Value = model.positionNo;
			parameters[11].Value = model.memo;
			parameters[12].Value = model.priority;
			parameters[13].Value = model.mainMaterial;
			parameters[14].Value = model.isCM;
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
			strSql.Append("delete from smtReplaceMaterial ");
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
			strSql.Append("delete from smtReplaceMaterial ");
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
		public Model.smtReplaceMaterial GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,productNo,productName,componentsNo,oldComponentsNo,specification,skPictureNo,brand,count,unit,supplier,positionNo,memo,priority,mainMaterial,isCM from smtReplaceMaterial ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.smtReplaceMaterial model=new Model.smtReplaceMaterial();
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
		public Model.smtReplaceMaterial DataRowToModel(DataRow row)
		{
			Model.smtReplaceMaterial model=new Model.smtReplaceMaterial();
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
				if(row["productName"]!=null)
				{
					model.productName=row["productName"].ToString();
				}
				if(row["componentsNo"]!=null)
				{
					model.componentsNo=row["componentsNo"].ToString();
				}
				if(row["oldComponentsNo"]!=null)
				{
					model.oldComponentsNo=row["oldComponentsNo"].ToString();
				}
				if(row["specification"]!=null)
				{
					model.specification=row["specification"].ToString();
				}
				if(row["skPictureNo"]!=null)
				{
					model.skPictureNo=row["skPictureNo"].ToString();
				}
				if(row["brand"]!=null)
				{
					model.brand=row["brand"].ToString();
				}
				if(row["count"]!=null)
				{
					model.count=row["count"].ToString();
				}
				if(row["unit"]!=null)
				{
					model.unit=row["unit"].ToString();
				}
				if(row["supplier"]!=null)
				{
					model.supplier=row["supplier"].ToString();
				}
				if(row["positionNo"]!=null)
				{
					model.positionNo=row["positionNo"].ToString();
				}
				if(row["memo"]!=null)
				{
					model.memo=row["memo"].ToString();
				}
				if(row["priority"]!=null)
				{
					model.priority=row["priority"].ToString();
				}
				if(row["mainMaterial"]!=null)
				{
					model.mainMaterial=row["mainMaterial"].ToString();
				}
				if(row["isCM"]!=null)
				{
					model.isCM=row["isCM"].ToString();
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
			strSql.Append("select id,productNo,productName,componentsNo,oldComponentsNo,specification,skPictureNo,brand,count,unit,supplier,positionNo,memo,priority,mainMaterial,isCM ");
			strSql.Append(" FROM smtReplaceMaterial ");
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
			strSql.Append(" id,productNo,productName,componentsNo,oldComponentsNo,specification,skPictureNo,brand,count,unit,supplier,positionNo,memo,priority,mainMaterial,isCM ");
			strSql.Append(" FROM smtReplaceMaterial ");
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
			strSql.Append("select count(1) FROM smtReplaceMaterial ");
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
			strSql.Append(")AS Row, T.*  from smtReplaceMaterial T ");
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
			parameters[0].Value = "smtReplaceMaterial";
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

