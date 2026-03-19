using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:pdCP
	/// </summary>
	public partial class pdCP
	{
		public pdCP()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "pdCP"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from pdCP");
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
		public int Add(Model.pdCP model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into pdCP(");
			strSql.Append("cPNO,productNO,productName,partNO,partName,status,contactor,tel,coreMember,custEngConfirm,custQuaConfirm,supplierNO,supplierName,supplierConfirm,otherConfirm,fileName,fileContent,fileStatus,creater,createTime,updateTime,pdProductID)");
			strSql.Append(" values (");
			strSql.Append("@cPNO,@productNO,@productName,@partNO,@partName,@status,@contactor,@tel,@coreMember,@custEngConfirm,@custQuaConfirm,@supplierNO,@supplierName,@supplierConfirm,@otherConfirm,@fileName,@fileContent,@fileStatus,@creater,@createTime,@updateTime,@pdProductID)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@cPNO", SqlDbType.NVarChar,50),
					new SqlParameter("@productNO", SqlDbType.NVarChar,100),
					new SqlParameter("@productName", SqlDbType.NVarChar,100),
					new SqlParameter("@partNO", SqlDbType.NVarChar,100),
					new SqlParameter("@partName", SqlDbType.NVarChar,100),
					new SqlParameter("@status", SqlDbType.NVarChar,10),
					new SqlParameter("@contactor", SqlDbType.NVarChar,20),
					new SqlParameter("@tel", SqlDbType.NVarChar,20),
					new SqlParameter("@coreMember", SqlDbType.NVarChar,100),
					new SqlParameter("@custEngConfirm", SqlDbType.NVarChar,50),
					new SqlParameter("@custQuaConfirm", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierNO", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierName", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierConfirm", SqlDbType.NVarChar,20),
					new SqlParameter("@otherConfirm", SqlDbType.NVarChar,50),
					new SqlParameter("@fileName", SqlDbType.NVarChar,100),
					new SqlParameter("@fileContent", SqlDbType.Image),
					new SqlParameter("@fileStatus", SqlDbType.NVarChar,10),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@pdProductID", SqlDbType.Int,4)};
			parameters[0].Value = model.cPNO;
			parameters[1].Value = model.productNO;
			parameters[2].Value = model.productName;
			parameters[3].Value = model.partNO;
			parameters[4].Value = model.partName;
			parameters[5].Value = model.status;
			parameters[6].Value = model.contactor;
			parameters[7].Value = model.tel;
			parameters[8].Value = model.coreMember;
			parameters[9].Value = model.custEngConfirm;
			parameters[10].Value = model.custQuaConfirm;
			parameters[11].Value = model.supplierNO;
			parameters[12].Value = model.supplierName;
			parameters[13].Value = model.supplierConfirm;
			parameters[14].Value = model.otherConfirm;
			parameters[15].Value = model.fileName;
			parameters[16].Value = model.fileContent;
			parameters[17].Value = model.fileStatus;
			parameters[18].Value = model.creater;
			parameters[19].Value = model.createTime;
			parameters[20].Value = model.updateTime;
			parameters[21].Value = model.pdProductID;

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
		public bool Update(Model.pdCP model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update pdCP set ");
			strSql.Append("cPNO=@cPNO,");
			strSql.Append("productNO=@productNO,");
			strSql.Append("productName=@productName,");
			strSql.Append("partNO=@partNO,");
			strSql.Append("partName=@partName,");
			strSql.Append("status=@status,");
			strSql.Append("contactor=@contactor,");
			strSql.Append("tel=@tel,");
			strSql.Append("coreMember=@coreMember,");
			strSql.Append("custEngConfirm=@custEngConfirm,");
			strSql.Append("custQuaConfirm=@custQuaConfirm,");
			strSql.Append("supplierNO=@supplierNO,");
			strSql.Append("supplierName=@supplierName,");
			strSql.Append("supplierConfirm=@supplierConfirm,");
			strSql.Append("otherConfirm=@otherConfirm,");
			strSql.Append("fileName=@fileName,");
			strSql.Append("fileContent=@fileContent,");
			strSql.Append("fileStatus=@fileStatus,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("updateTime=@updateTime,");
			strSql.Append("pdProductID=@pdProductID");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@cPNO", SqlDbType.NVarChar,50),
					new SqlParameter("@productNO", SqlDbType.NVarChar,100),
					new SqlParameter("@productName", SqlDbType.NVarChar,100),
					new SqlParameter("@partNO", SqlDbType.NVarChar,100),
					new SqlParameter("@partName", SqlDbType.NVarChar,100),
					new SqlParameter("@status", SqlDbType.NVarChar,10),
					new SqlParameter("@contactor", SqlDbType.NVarChar,20),
					new SqlParameter("@tel", SqlDbType.NVarChar,20),
					new SqlParameter("@coreMember", SqlDbType.NVarChar,100),
					new SqlParameter("@custEngConfirm", SqlDbType.NVarChar,50),
					new SqlParameter("@custQuaConfirm", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierNO", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierName", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierConfirm", SqlDbType.NVarChar,20),
					new SqlParameter("@otherConfirm", SqlDbType.NVarChar,50),
					new SqlParameter("@fileName", SqlDbType.NVarChar,100),
					new SqlParameter("@fileContent", SqlDbType.Image),
					new SqlParameter("@fileStatus", SqlDbType.NVarChar,10),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@pdProductID", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.cPNO;
			parameters[1].Value = model.productNO;
			parameters[2].Value = model.productName;
			parameters[3].Value = model.partNO;
			parameters[4].Value = model.partName;
			parameters[5].Value = model.status;
			parameters[6].Value = model.contactor;
			parameters[7].Value = model.tel;
			parameters[8].Value = model.coreMember;
			parameters[9].Value = model.custEngConfirm;
			parameters[10].Value = model.custQuaConfirm;
			parameters[11].Value = model.supplierNO;
			parameters[12].Value = model.supplierName;
			parameters[13].Value = model.supplierConfirm;
			parameters[14].Value = model.otherConfirm;
			parameters[15].Value = model.fileName;
			parameters[16].Value = model.fileContent;
			parameters[17].Value = model.fileStatus;
			parameters[18].Value = model.creater;
			parameters[19].Value = model.createTime;
			parameters[20].Value = model.updateTime;
			parameters[21].Value = model.pdProductID;
			parameters[22].Value = model.id;

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
			strSql.Append("delete from pdCP ");
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
			strSql.Append("delete from pdCP ");
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
		public Model.pdCP GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,cPNO,productNO,productName,partNO,partName,status,contactor,tel,coreMember,custEngConfirm,custQuaConfirm,supplierNO,supplierName,supplierConfirm,otherConfirm,fileName,fileContent,fileStatus,creater,createTime,updateTime,pdProductID from pdCP ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.pdCP model=new Model.pdCP();
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
		public Model.pdCP DataRowToModel(DataRow row)
		{
			Model.pdCP model=new Model.pdCP();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["cPNO"]!=null)
				{
					model.cPNO=row["cPNO"].ToString();
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
				if(row["status"]!=null)
				{
					model.status=row["status"].ToString();
				}
				if(row["contactor"]!=null)
				{
					model.contactor=row["contactor"].ToString();
				}
				if(row["tel"]!=null)
				{
					model.tel=row["tel"].ToString();
				}
				if(row["coreMember"]!=null)
				{
					model.coreMember=row["coreMember"].ToString();
				}
				if(row["custEngConfirm"]!=null)
				{
					model.custEngConfirm=row["custEngConfirm"].ToString();
				}
				if(row["custQuaConfirm"]!=null)
				{
					model.custQuaConfirm=row["custQuaConfirm"].ToString();
				}
				if(row["supplierNO"]!=null)
				{
					model.supplierNO=row["supplierNO"].ToString();
				}
				if(row["supplierName"]!=null)
				{
					model.supplierName=row["supplierName"].ToString();
				}
				if(row["supplierConfirm"]!=null)
				{
					model.supplierConfirm=row["supplierConfirm"].ToString();
				}
				if(row["otherConfirm"]!=null)
				{
					model.otherConfirm=row["otherConfirm"].ToString();
				}
				if(row["fileName"]!=null)
				{
					model.fileName=row["fileName"].ToString();
				}
				if(row["fileContent"]!=null && row["fileContent"].ToString()!="")
				{
					model.fileContent=(byte[])row["fileContent"];
				}
				if(row["fileStatus"]!=null)
				{
					model.fileStatus=row["fileStatus"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["updateTime"]!=null && row["updateTime"].ToString()!="")
				{
					model.updateTime=DateTime.Parse(row["updateTime"].ToString());
				}
				if(row["pdProductID"]!=null && row["pdProductID"].ToString()!="")
				{
					model.pdProductID=int.Parse(row["pdProductID"].ToString());
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
			strSql.Append("select id,cPNO,productNO,productName,partNO,partName,status,contactor,tel,coreMember,custEngConfirm,custQuaConfirm,supplierNO,supplierName,supplierConfirm,otherConfirm,fileName,fileContent,fileStatus,creater,createTime,updateTime,pdProductID ");
			strSql.Append(" FROM pdCP ");
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
			strSql.Append(" id,cPNO,productNO,productName,partNO,partName,status,contactor,tel,coreMember,custEngConfirm,custQuaConfirm,supplierNO,supplierName,supplierConfirm,otherConfirm,fileName,fileContent,fileStatus,creater,createTime,updateTime,pdProductID ");
			strSql.Append(" FROM pdCP ");
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
			strSql.Append("select count(1) FROM pdCP ");
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
			strSql.Append(")AS Row, T.*  from pdCP T ");
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
			parameters[0].Value = "pdCP";
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

