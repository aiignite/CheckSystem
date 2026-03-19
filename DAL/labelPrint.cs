using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:labelPrint
	/// </summary>
	public partial class labelPrint
	{
		public labelPrint()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "labelPrint"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from labelPrint");
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
		public int Add(Model.labelPrint model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into labelPrint(");
			strSql.Append("SourceStr,Qty,creater,createTime,PrintCount,SBin,str1,str2,str3,str4)");
			strSql.Append(" values (");
			strSql.Append("@SourceStr,@Qty,@creater,@createTime,@PrintCount,@SBin,@str1,@str2,@str3,@str4)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@SourceStr", SqlDbType.NVarChar,150),
					new SqlParameter("@Qty", SqlDbType.Int,4),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@PrintCount", SqlDbType.Int,4),
					new SqlParameter("@SBin", SqlDbType.NVarChar,150),
					new SqlParameter("@str1", SqlDbType.NVarChar,50),
					new SqlParameter("@str2", SqlDbType.NVarChar,50),
					new SqlParameter("@str3", SqlDbType.NVarChar,50),
					new SqlParameter("@str4", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.SourceStr;
			parameters[1].Value = model.Qty;
			parameters[2].Value = model.creater;
			parameters[3].Value = model.createTime;
			parameters[4].Value = model.PrintCount;
			parameters[5].Value = model.SBin;
			parameters[6].Value = model.str1;
			parameters[7].Value = model.str2;
			parameters[8].Value = model.str3;
			parameters[9].Value = model.str4;

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
		public bool Update(Model.labelPrint model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update labelPrint set ");
			strSql.Append("SourceStr=@SourceStr,");
			strSql.Append("Qty=@Qty,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("PrintCount=@PrintCount,");
			strSql.Append("SBin=@SBin,");
			strSql.Append("str1=@str1,");
			strSql.Append("str2=@str2,");
			strSql.Append("str3=@str3,");
			strSql.Append("str4=@str4");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@SourceStr", SqlDbType.NVarChar,150),
					new SqlParameter("@Qty", SqlDbType.Int,4),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@PrintCount", SqlDbType.Int,4),
					new SqlParameter("@SBin", SqlDbType.NVarChar,150),
					new SqlParameter("@str1", SqlDbType.NVarChar,50),
					new SqlParameter("@str2", SqlDbType.NVarChar,50),
					new SqlParameter("@str3", SqlDbType.NVarChar,50),
					new SqlParameter("@str4", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.SourceStr;
			parameters[1].Value = model.Qty;
			parameters[2].Value = model.creater;
			parameters[3].Value = model.createTime;
			parameters[4].Value = model.PrintCount;
			parameters[5].Value = model.SBin;
			parameters[6].Value = model.str1;
			parameters[7].Value = model.str2;
			parameters[8].Value = model.str3;
			parameters[9].Value = model.str4;
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
			strSql.Append("delete from labelPrint ");
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
			strSql.Append("delete from labelPrint ");
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
		public Model.labelPrint GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,SourceStr,Qty,creater,createTime,PrintCount,SBin,str1,str2,str3,str4 from labelPrint ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.labelPrint model=new Model.labelPrint();
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
		public Model.labelPrint DataRowToModel(DataRow row)
		{
			Model.labelPrint model=new Model.labelPrint();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["SourceStr"]!=null)
				{
					model.SourceStr=row["SourceStr"].ToString();
				}
				if(row["Qty"]!=null && row["Qty"].ToString()!="")
				{
					model.Qty=int.Parse(row["Qty"].ToString());
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["PrintCount"]!=null && row["PrintCount"].ToString()!="")
				{
					model.PrintCount=int.Parse(row["PrintCount"].ToString());
				}
				if(row["SBin"]!=null)
				{
					model.SBin=row["SBin"].ToString();
				}
				if(row["str1"]!=null)
				{
					model.str1=row["str1"].ToString();
				}
				if(row["str2"]!=null)
				{
					model.str2=row["str2"].ToString();
				}
				if(row["str3"]!=null)
				{
					model.str3=row["str3"].ToString();
				}
				if(row["str4"]!=null)
				{
					model.str4=row["str4"].ToString();
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
			strSql.Append("select id,SourceStr,Qty,creater,createTime,PrintCount,SBin,str1,str2,str3,str4 ");
			strSql.Append(" FROM labelPrint ");
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
			strSql.Append(" id,SourceStr,Qty,creater,createTime,PrintCount,SBin,str1,str2,str3,str4 ");
			strSql.Append(" FROM labelPrint ");
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
			strSql.Append("select count(1) FROM labelPrint ");
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
			strSql.Append(")AS Row, T.*  from labelPrint T ");
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
			parameters[0].Value = "labelPrint";
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

