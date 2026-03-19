using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:Test
	/// </summary>
	public partial class Test
	{
		public Test()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(Model.Test model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Test(");
			strSql.Append("T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12)");
			strSql.Append(" values (");
			strSql.Append("@T1,@T2,@T3,@T4,@T5,@T6,@T7,@T8,@T9,@T10,@T11,@T12)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@T1", SqlDbType.Decimal,9),
					new SqlParameter("@T2", SqlDbType.Decimal,9),
					new SqlParameter("@T3", SqlDbType.Decimal,9),
					new SqlParameter("@T4", SqlDbType.Decimal,9),
					new SqlParameter("@T5", SqlDbType.Decimal,9),
					new SqlParameter("@T6", SqlDbType.NVarChar,50),
					new SqlParameter("@T7", SqlDbType.NVarChar,50),
					new SqlParameter("@T8", SqlDbType.NVarChar,50),
					new SqlParameter("@T9", SqlDbType.NVarChar,50),
					new SqlParameter("@T10", SqlDbType.NVarChar,50),
					new SqlParameter("@T11", SqlDbType.NVarChar,50),
					new SqlParameter("@T12", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.T1;
			parameters[1].Value = model.T2;
			parameters[2].Value = model.T3;
			parameters[3].Value = model.T4;
			parameters[4].Value = model.T5;
			parameters[5].Value = model.T6;
			parameters[6].Value = model.T7;
			parameters[7].Value = model.T8;
			parameters[8].Value = model.T9;
			parameters[9].Value = model.T10;
			parameters[10].Value = model.T11;
			parameters[11].Value = model.T12;

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
		public bool Update(Model.Test model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Test set ");
			strSql.Append("T1=@T1,");
			strSql.Append("T2=@T2,");
			strSql.Append("T3=@T3,");
			strSql.Append("T4=@T4,");
			strSql.Append("T5=@T5,");
			strSql.Append("T6=@T6,");
			strSql.Append("T7=@T7,");
			strSql.Append("T8=@T8,");
			strSql.Append("T9=@T9,");
			strSql.Append("T10=@T10,");
			strSql.Append("T11=@T11,");
			strSql.Append("T12=@T12");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@T1", SqlDbType.Decimal,9),
					new SqlParameter("@T2", SqlDbType.Decimal,9),
					new SqlParameter("@T3", SqlDbType.Decimal,9),
					new SqlParameter("@T4", SqlDbType.Decimal,9),
					new SqlParameter("@T5", SqlDbType.Decimal,9),
					new SqlParameter("@T6", SqlDbType.NVarChar,50),
					new SqlParameter("@T7", SqlDbType.NVarChar,50),
					new SqlParameter("@T8", SqlDbType.NVarChar,50),
					new SqlParameter("@T9", SqlDbType.NVarChar,50),
					new SqlParameter("@T10", SqlDbType.NVarChar,50),
					new SqlParameter("@T11", SqlDbType.NVarChar,50),
					new SqlParameter("@T12", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.T1;
			parameters[1].Value = model.T2;
			parameters[2].Value = model.T3;
			parameters[3].Value = model.T4;
			parameters[4].Value = model.T5;
			parameters[5].Value = model.T6;
			parameters[6].Value = model.T7;
			parameters[7].Value = model.T8;
			parameters[8].Value = model.T9;
			parameters[9].Value = model.T10;
			parameters[10].Value = model.T11;
			parameters[11].Value = model.T12;
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
			strSql.Append("delete from Test ");
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
			strSql.Append("delete from Test ");
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
		public Model.Test GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12 from Test ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.Test model=new Model.Test();
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
		public Model.Test DataRowToModel(DataRow row)
		{
			Model.Test model=new Model.Test();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["T1"]!=null && row["T1"].ToString()!="")
				{
					model.T1=decimal.Parse(row["T1"].ToString());
				}
				if(row["T2"]!=null && row["T2"].ToString()!="")
				{
					model.T2=decimal.Parse(row["T2"].ToString());
				}
				if(row["T3"]!=null && row["T3"].ToString()!="")
				{
					model.T3=decimal.Parse(row["T3"].ToString());
				}
				if(row["T4"]!=null && row["T4"].ToString()!="")
				{
					model.T4=decimal.Parse(row["T4"].ToString());
				}
				if(row["T5"]!=null && row["T5"].ToString()!="")
				{
					model.T5=decimal.Parse(row["T5"].ToString());
				}
				if(row["T6"]!=null)
				{
					model.T6=row["T6"].ToString();
				}
				if(row["T7"]!=null)
				{
					model.T7=row["T7"].ToString();
				}
				if(row["T8"]!=null)
				{
					model.T8=row["T8"].ToString();
				}
				if(row["T9"]!=null)
				{
					model.T9=row["T9"].ToString();
				}
				if(row["T10"]!=null)
				{
					model.T10=row["T10"].ToString();
				}
				if(row["T11"]!=null)
				{
					model.T11=row["T11"].ToString();
				}
				if(row["T12"]!=null)
				{
					model.T12=row["T12"].ToString();
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
			strSql.Append("select id,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12 ");
			strSql.Append(" FROM Test ");
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
			strSql.Append(" id,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12 ");
			strSql.Append(" FROM Test ");
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
			strSql.Append("select count(1) FROM Test ");
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
			strSql.Append(")AS Row, T.*  from Test T ");
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
			parameters[0].Value = "Test";
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

