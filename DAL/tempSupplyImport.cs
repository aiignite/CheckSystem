using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:tempSupplyImport
	/// </summary>
	public partial class tempSupplyImport
	{
		public tempSupplyImport()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.tempSupplyImport model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tempSupplyImport(");
			strSql.Append("ZZ,typeText,cgGroup,supplyNo,material,materialGroup)");
			strSql.Append(" values (");
			strSql.Append("@ZZ,@typeText,@cgGroup,@supplyNo,@material,@materialGroup)");
			SqlParameter[] parameters = {
					new SqlParameter("@ZZ", SqlDbType.NVarChar,255),
					new SqlParameter("@typeText", SqlDbType.NVarChar,255),
					new SqlParameter("@cgGroup", SqlDbType.NVarChar,255),
					new SqlParameter("@supplyNo", SqlDbType.NVarChar,255),
					new SqlParameter("@material", SqlDbType.NVarChar,255),
					new SqlParameter("@materialGroup", SqlDbType.NVarChar,255)};
			parameters[0].Value = model.ZZ;
			parameters[1].Value = model.typeText;
			parameters[2].Value = model.cgGroup;
			parameters[3].Value = model.supplyNo;
			parameters[4].Value = model.material;
			parameters[5].Value = model.materialGroup;

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
		/// 更新一条数据
		/// </summary>
		public bool Update(Model.tempSupplyImport model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tempSupplyImport set ");
			strSql.Append("ZZ=@ZZ,");
			strSql.Append("typeText=@typeText,");
			strSql.Append("cgGroup=@cgGroup,");
			strSql.Append("supplyNo=@supplyNo,");
			strSql.Append("material=@material,");
			strSql.Append("materialGroup=@materialGroup");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@ZZ", SqlDbType.NVarChar,255),
					new SqlParameter("@typeText", SqlDbType.NVarChar,255),
					new SqlParameter("@cgGroup", SqlDbType.NVarChar,255),
					new SqlParameter("@supplyNo", SqlDbType.NVarChar,255),
					new SqlParameter("@material", SqlDbType.NVarChar,255),
					new SqlParameter("@materialGroup", SqlDbType.NVarChar,255)};
			parameters[0].Value = model.ZZ;
			parameters[1].Value = model.typeText;
			parameters[2].Value = model.cgGroup;
			parameters[3].Value = model.supplyNo;
			parameters[4].Value = model.material;
			parameters[5].Value = model.materialGroup;

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
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tempSupplyImport ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
			};

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
		/// 得到一个对象实体
		/// </summary>
		public Model.tempSupplyImport GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ZZ,typeText,cgGroup,supplyNo,material,materialGroup from tempSupplyImport ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
			};

			Model.tempSupplyImport model=new Model.tempSupplyImport();
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
		public Model.tempSupplyImport DataRowToModel(DataRow row)
		{
			Model.tempSupplyImport model=new Model.tempSupplyImport();
			if (row != null)
			{
				if(row["ZZ"]!=null)
				{
					model.ZZ=row["ZZ"].ToString();
				}
				if(row["typeText"]!=null)
				{
					model.typeText=row["typeText"].ToString();
				}
				if(row["cgGroup"]!=null)
				{
					model.cgGroup=row["cgGroup"].ToString();
				}
				if(row["supplyNo"]!=null)
				{
					model.supplyNo=row["supplyNo"].ToString();
				}
				if(row["material"]!=null)
				{
					model.material=row["material"].ToString();
				}
				if(row["materialGroup"]!=null)
				{
					model.materialGroup=row["materialGroup"].ToString();
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
			strSql.Append("select ZZ,typeText,cgGroup,supplyNo,material,materialGroup ");
			strSql.Append(" FROM tempSupplyImport ");
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
			strSql.Append(" ZZ,typeText,cgGroup,supplyNo,material,materialGroup ");
			strSql.Append(" FROM tempSupplyImport ");
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
			strSql.Append("select count(1) FROM tempSupplyImport ");
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
			strSql.Append(")AS Row, T.*  from tempSupplyImport T ");
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
			parameters[0].Value = "tempSupplyImport";
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

