using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:pdPFD
	/// </summary>
	public partial class pdPFD
	{
		public pdPFD()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "pdPFD"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from pdPFD");
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
		public int Add(Model.pdPFD model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into pdPFD(");
			strSql.Append("pFDNO,productNO,productName,partNO,partName,status,contactor,tel,fax,supplierNO,supplierName,supplierAdd,supplierConfirm,fileName,fileContent,fileStatus,creater,createTime,updateTime,pdProductID)");
			strSql.Append(" values (");
			strSql.Append("@pFDNO,@productNO,@productName,@partNO,@partName,@status,@contactor,@tel,@fax,@supplierNO,@supplierName,@supplierAdd,@supplierConfirm,@fileName,@fileContent,@fileStatus,@creater,@createTime,@updateTime,@pdProductID)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@pFDNO", SqlDbType.NVarChar,50),
					new SqlParameter("@productNO", SqlDbType.NVarChar,100),
					new SqlParameter("@productName", SqlDbType.NVarChar,100),
					new SqlParameter("@partNO", SqlDbType.NVarChar,100),
					new SqlParameter("@partName", SqlDbType.NVarChar,100),
					new SqlParameter("@status", SqlDbType.NVarChar,2),
					new SqlParameter("@contactor", SqlDbType.NVarChar,20),
					new SqlParameter("@tel", SqlDbType.NVarChar,20),
					new SqlParameter("@fax", SqlDbType.NVarChar,20),
					new SqlParameter("@supplierNO", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierName", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierAdd", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierConfirm", SqlDbType.NVarChar,50),
					new SqlParameter("@fileName", SqlDbType.NVarChar,100),
					new SqlParameter("@fileContent", SqlDbType.Image),
					new SqlParameter("@fileStatus", SqlDbType.NVarChar,10),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@pdProductID", SqlDbType.Int,4)};
			parameters[0].Value = model.pFDNO;
			parameters[1].Value = model.productNO;
			parameters[2].Value = model.productName;
			parameters[3].Value = model.partNO;
			parameters[4].Value = model.partName;
			parameters[5].Value = model.status;
			parameters[6].Value = model.contactor;
			parameters[7].Value = model.tel;
			parameters[8].Value = model.fax;
			parameters[9].Value = model.supplierNO;
			parameters[10].Value = model.supplierName;
			parameters[11].Value = model.supplierAdd;
			parameters[12].Value = model.supplierConfirm;
			parameters[13].Value = model.fileName;
			parameters[14].Value = model.fileContent;
			parameters[15].Value = model.fileStatus;
			parameters[16].Value = model.creater;
			parameters[17].Value = model.createTime;
			parameters[18].Value = model.updateTime;
			parameters[19].Value = model.pdProductID;

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
		public bool Update(Model.pdPFD model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update pdPFD set ");
			strSql.Append("pFDNO=@pFDNO,");
			strSql.Append("productNO=@productNO,");
			strSql.Append("productName=@productName,");
			strSql.Append("partNO=@partNO,");
			strSql.Append("partName=@partName,");
			strSql.Append("status=@status,");
			strSql.Append("contactor=@contactor,");
			strSql.Append("tel=@tel,");
			strSql.Append("fax=@fax,");
			strSql.Append("supplierNO=@supplierNO,");
			strSql.Append("supplierName=@supplierName,");
			strSql.Append("supplierAdd=@supplierAdd,");
			strSql.Append("supplierConfirm=@supplierConfirm,");
			strSql.Append("fileName=@fileName,");
			strSql.Append("fileContent=@fileContent,");
			strSql.Append("fileStatus=@fileStatus,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("updateTime=@updateTime,");
			strSql.Append("pdProductID=@pdProductID");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@pFDNO", SqlDbType.NVarChar,50),
					new SqlParameter("@productNO", SqlDbType.NVarChar,100),
					new SqlParameter("@productName", SqlDbType.NVarChar,100),
					new SqlParameter("@partNO", SqlDbType.NVarChar,100),
					new SqlParameter("@partName", SqlDbType.NVarChar,100),
					new SqlParameter("@status", SqlDbType.NVarChar,2),
					new SqlParameter("@contactor", SqlDbType.NVarChar,20),
					new SqlParameter("@tel", SqlDbType.NVarChar,20),
					new SqlParameter("@fax", SqlDbType.NVarChar,20),
					new SqlParameter("@supplierNO", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierName", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierAdd", SqlDbType.NVarChar,50),
					new SqlParameter("@supplierConfirm", SqlDbType.NVarChar,50),
					new SqlParameter("@fileName", SqlDbType.NVarChar,100),
					new SqlParameter("@fileContent", SqlDbType.Image),
					new SqlParameter("@fileStatus", SqlDbType.NVarChar,10),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@pdProductID", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.pFDNO;
			parameters[1].Value = model.productNO;
			parameters[2].Value = model.productName;
			parameters[3].Value = model.partNO;
			parameters[4].Value = model.partName;
			parameters[5].Value = model.status;
			parameters[6].Value = model.contactor;
			parameters[7].Value = model.tel;
			parameters[8].Value = model.fax;
			parameters[9].Value = model.supplierNO;
			parameters[10].Value = model.supplierName;
			parameters[11].Value = model.supplierAdd;
			parameters[12].Value = model.supplierConfirm;
			parameters[13].Value = model.fileName;
			parameters[14].Value = model.fileContent;
			parameters[15].Value = model.fileStatus;
			parameters[16].Value = model.creater;
			parameters[17].Value = model.createTime;
			parameters[18].Value = model.updateTime;
			parameters[19].Value = model.pdProductID;
			parameters[20].Value = model.id;

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
			strSql.Append("delete from pdPFD ");
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
			strSql.Append("delete from pdPFD ");
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
		public Model.pdPFD GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,pFDNO,productNO,productName,partNO,partName,status,contactor,tel,fax,supplierNO,supplierName,supplierAdd,supplierConfirm,fileName,fileContent,fileStatus,creater,createTime,updateTime,pdProductID from pdPFD ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.pdPFD model=new Model.pdPFD();
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
		public Model.pdPFD DataRowToModel(DataRow row)
		{
			Model.pdPFD model=new Model.pdPFD();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["pFDNO"]!=null)
				{
					model.pFDNO=row["pFDNO"].ToString();
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
				if(row["fax"]!=null)
				{
					model.fax=row["fax"].ToString();
				}
				if(row["supplierNO"]!=null)
				{
					model.supplierNO=row["supplierNO"].ToString();
				}
				if(row["supplierName"]!=null)
				{
					model.supplierName=row["supplierName"].ToString();
				}
				if(row["supplierAdd"]!=null)
				{
					model.supplierAdd=row["supplierAdd"].ToString();
				}
				if(row["supplierConfirm"]!=null)
				{
					model.supplierConfirm=row["supplierConfirm"].ToString();
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
			strSql.Append("select id,pFDNO,productNO,productName,partNO,partName,status,contactor,tel,fax,supplierNO,supplierName,supplierAdd,supplierConfirm,fileName,fileContent,fileStatus,creater,createTime,updateTime,pdProductID ");
			strSql.Append(" FROM pdPFD ");
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
			strSql.Append(" id,pFDNO,productNO,productName,partNO,partName,status,contactor,tel,fax,supplierNO,supplierName,supplierAdd,supplierConfirm,fileName,fileContent,fileStatus,creater,createTime,updateTime,pdProductID ");
			strSql.Append(" FROM pdPFD ");
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
			strSql.Append("select count(1) FROM pdPFD ");
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
			strSql.Append(")AS Row, T.*  from pdPFD T ");
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
			parameters[0].Value = "pdPFD";
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

