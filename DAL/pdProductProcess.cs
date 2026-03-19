using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:pdProductProcess
	/// </summary>
	public partial class pdProductProcess
	{
		public pdProductProcess()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "pdProductProcess"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from pdProductProcess");
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
		public int Add(Model.pdProductProcess model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into pdProductProcess(");
			strSql.Append("productNO,productName,partNO,partName,pBNOs,pBInfos,fileType,IsComputeSDP,IsCheckSev,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@productNO,@productName,@partNO,@partName,@pBNOs,@pBInfos,@fileType,@IsComputeSDP,@IsCheckSev,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@productNO", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,100),
					new SqlParameter("@partNO", SqlDbType.NVarChar,100),
					new SqlParameter("@partName", SqlDbType.NVarChar,100),
					new SqlParameter("@pBNOs", SqlDbType.NVarChar,200),
					new SqlParameter("@pBInfos", SqlDbType.NVarChar,500),
					new SqlParameter("@fileType", SqlDbType.Int,4),
					new SqlParameter("@IsComputeSDP", SqlDbType.NVarChar,1),
					new SqlParameter("@IsCheckSev", SqlDbType.NVarChar,1),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.productNO;
			parameters[1].Value = model.productName;
			parameters[2].Value = model.partNO;
			parameters[3].Value = model.partName;
			parameters[4].Value = model.pBNOs;
			parameters[5].Value = model.pBInfos;
			parameters[6].Value = model.fileType;
			parameters[7].Value = model.IsComputeSDP;
			parameters[8].Value = model.IsCheckSev;
			parameters[9].Value = model.creater;
			parameters[10].Value = model.createTime;

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
		public bool Update(Model.pdProductProcess model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update pdProductProcess set ");
			strSql.Append("productNO=@productNO,");
			strSql.Append("productName=@productName,");
			strSql.Append("partNO=@partNO,");
			strSql.Append("partName=@partName,");
			strSql.Append("pBNOs=@pBNOs,");
			strSql.Append("pBInfos=@pBInfos,");
			strSql.Append("fileType=@fileType,");
			strSql.Append("IsComputeSDP=@IsComputeSDP,");
			strSql.Append("IsCheckSev=@IsCheckSev,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@productNO", SqlDbType.NVarChar,50),
					new SqlParameter("@productName", SqlDbType.NVarChar,100),
					new SqlParameter("@partNO", SqlDbType.NVarChar,100),
					new SqlParameter("@partName", SqlDbType.NVarChar,100),
					new SqlParameter("@pBNOs", SqlDbType.NVarChar,200),
					new SqlParameter("@pBInfos", SqlDbType.NVarChar,500),
					new SqlParameter("@fileType", SqlDbType.Int,4),
					new SqlParameter("@IsComputeSDP", SqlDbType.NVarChar,1),
					new SqlParameter("@IsCheckSev", SqlDbType.NVarChar,1),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.productNO;
			parameters[1].Value = model.productName;
			parameters[2].Value = model.partNO;
			parameters[3].Value = model.partName;
			parameters[4].Value = model.pBNOs;
			parameters[5].Value = model.pBInfos;
			parameters[6].Value = model.fileType;
			parameters[7].Value = model.IsComputeSDP;
			parameters[8].Value = model.IsCheckSev;
			parameters[9].Value = model.creater;
			parameters[10].Value = model.createTime;
			parameters[11].Value = model.id;

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
			strSql.Append("delete from pdProductProcess ");
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
			strSql.Append("delete from pdProductProcess ");
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
		public Model.pdProductProcess GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,productNO,productName,partNO,partName,pBNOs,pBInfos,fileType,IsComputeSDP,IsCheckSev,creater,createTime from pdProductProcess ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.pdProductProcess model=new Model.pdProductProcess();
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
		public Model.pdProductProcess DataRowToModel(DataRow row)
		{
			Model.pdProductProcess model=new Model.pdProductProcess();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["productNO"]!=null)
				{
					model.productNO=row["productNO"].ToString();
				}
				if(row["productName"]!=null)
				{
					model.productName=row["productName"].ToString();
				}
				if(row["partNO"]!=null)
				{
					model.partNO=row["partNO"].ToString();
				}
				if(row["partName"]!=null)
				{
					model.partName=row["partName"].ToString();
				}
				if(row["pBNOs"]!=null)
				{
					model.pBNOs=row["pBNOs"].ToString();
				}
				if(row["pBInfos"]!=null)
				{
					model.pBInfos=row["pBInfos"].ToString();
				}
				if(row["fileType"]!=null && row["fileType"].ToString()!="")
				{
					model.fileType=int.Parse(row["fileType"].ToString());
				}
				if(row["IsComputeSDP"]!=null)
				{
					model.IsComputeSDP=row["IsComputeSDP"].ToString();
				}
				if(row["IsCheckSev"]!=null)
				{
					model.IsCheckSev=row["IsCheckSev"].ToString();
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
			strSql.Append("select id,productNO,productName,partNO,partName,pBNOs,pBInfos,fileType,IsComputeSDP,IsCheckSev,creater,createTime ");
			strSql.Append(" FROM pdProductProcess ");
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
			strSql.Append(" id,productNO,productName,partNO,partName,pBNOs,pBInfos,fileType,IsComputeSDP,IsCheckSev,creater,createTime ");
			strSql.Append(" FROM pdProductProcess ");
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
			strSql.Append("select count(1) FROM pdProductProcess ");
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
			strSql.Append(")AS Row, T.*  from pdProductProcess T ");
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
			parameters[0].Value = "pdProductProcess";
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

