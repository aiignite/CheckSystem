using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:smtProcessInfo
	/// </summary>
	public partial class smtProcessInfo
	{
		public smtProcessInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "smtProcessInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from smtProcessInfo");
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
		public int Add(Model.smtProcessInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into smtProcessInfo(");
			strSql.Append("machineTypeNo,productNo,productNum,materialNo,processNo,pBomNo,bomGroupNo,assLineNo,mounterNo,pMaterialNo,mounterPosNo,feederNo,creater,createTime,pCompanyBin)");
			strSql.Append(" values (");
			strSql.Append("@machineTypeNo,@productNo,@productNum,@materialNo,@processNo,@pBomNo,@bomGroupNo,@assLineNo,@mounterNo,@pMaterialNo,@mounterPosNo,@feederNo,@creater,@createTime,@pCompanyBin)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@machineTypeNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNum", SqlDbType.NVarChar,50),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pBomNo", SqlDbType.NVarChar,50),
					new SqlParameter("@bomGroupNo", SqlDbType.NVarChar,50),
					new SqlParameter("@assLineNo", SqlDbType.NVarChar,50),
					new SqlParameter("@mounterNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pMaterialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@mounterPosNo", SqlDbType.NVarChar,50),
					new SqlParameter("@feederNo", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@pCompanyBin", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.machineTypeNo;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.productNum;
			parameters[3].Value = model.materialNo;
			parameters[4].Value = model.processNo;
			parameters[5].Value = model.pBomNo;
			parameters[6].Value = model.bomGroupNo;
			parameters[7].Value = model.assLineNo;
			parameters[8].Value = model.mounterNo;
			parameters[9].Value = model.pMaterialNo;
			parameters[10].Value = model.mounterPosNo;
			parameters[11].Value = model.feederNo;
			parameters[12].Value = model.creater;
			parameters[13].Value = model.createTime;
			parameters[14].Value = model.pCompanyBin;

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
		public bool Update(Model.smtProcessInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update smtProcessInfo set ");
			strSql.Append("machineTypeNo=@machineTypeNo,");
			strSql.Append("productNo=@productNo,");
			strSql.Append("productNum=@productNum,");
			strSql.Append("materialNo=@materialNo,");
			strSql.Append("processNo=@processNo,");
			strSql.Append("pBomNo=@pBomNo,");
			strSql.Append("bomGroupNo=@bomGroupNo,");
			strSql.Append("assLineNo=@assLineNo,");
			strSql.Append("mounterNo=@mounterNo,");
			strSql.Append("pMaterialNo=@pMaterialNo,");
			strSql.Append("mounterPosNo=@mounterPosNo,");
			strSql.Append("feederNo=@feederNo,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("pCompanyBin=@pCompanyBin");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@machineTypeNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@productNum", SqlDbType.NVarChar,50),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pBomNo", SqlDbType.NVarChar,50),
					new SqlParameter("@bomGroupNo", SqlDbType.NVarChar,50),
					new SqlParameter("@assLineNo", SqlDbType.NVarChar,50),
					new SqlParameter("@mounterNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pMaterialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@mounterPosNo", SqlDbType.NVarChar,50),
					new SqlParameter("@feederNo", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@pCompanyBin", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.machineTypeNo;
			parameters[1].Value = model.productNo;
			parameters[2].Value = model.productNum;
			parameters[3].Value = model.materialNo;
			parameters[4].Value = model.processNo;
			parameters[5].Value = model.pBomNo;
			parameters[6].Value = model.bomGroupNo;
			parameters[7].Value = model.assLineNo;
			parameters[8].Value = model.mounterNo;
			parameters[9].Value = model.pMaterialNo;
			parameters[10].Value = model.mounterPosNo;
			parameters[11].Value = model.feederNo;
			parameters[12].Value = model.creater;
			parameters[13].Value = model.createTime;
			parameters[14].Value = model.pCompanyBin;
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
			strSql.Append("delete from smtProcessInfo ");
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
			strSql.Append("delete from smtProcessInfo ");
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
		public Model.smtProcessInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,machineTypeNo,productNo,productNum,materialNo,processNo,pBomNo,bomGroupNo,assLineNo,mounterNo,pMaterialNo,mounterPosNo,feederNo,creater,createTime,pCompanyBin from smtProcessInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.smtProcessInfo model=new Model.smtProcessInfo();
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
		public Model.smtProcessInfo DataRowToModel(DataRow row)
		{
			Model.smtProcessInfo model=new Model.smtProcessInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["machineTypeNo"]!=null)
				{
					model.machineTypeNo=row["machineTypeNo"].ToString();
				}
				if(row["productNo"]!=null)
				{
					model.productNo=row["productNo"].ToString();
				}
				if(row["productNum"]!=null)
				{
					model.productNum=row["productNum"].ToString();
				}
				if(row["materialNo"]!=null)
				{
					model.materialNo=row["materialNo"].ToString();
				}
				if(row["processNo"]!=null)
				{
					model.processNo=row["processNo"].ToString();
				}
				if(row["pBomNo"]!=null)
				{
					model.pBomNo=row["pBomNo"].ToString();
				}
				if(row["bomGroupNo"]!=null)
				{
					model.bomGroupNo=row["bomGroupNo"].ToString();
				}
				if(row["assLineNo"]!=null)
				{
					model.assLineNo=row["assLineNo"].ToString();
				}
				if(row["mounterNo"]!=null)
				{
					model.mounterNo=row["mounterNo"].ToString();
				}
				if(row["pMaterialNo"]!=null)
				{
					model.pMaterialNo=row["pMaterialNo"].ToString();
				}
				if(row["mounterPosNo"]!=null)
				{
					model.mounterPosNo=row["mounterPosNo"].ToString();
				}
				if(row["feederNo"]!=null)
				{
					model.feederNo=row["feederNo"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["pCompanyBin"]!=null)
				{
					model.pCompanyBin=row["pCompanyBin"].ToString();
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
			strSql.Append("select id,machineTypeNo,productNo,productNum,materialNo,processNo,pBomNo,bomGroupNo,assLineNo,mounterNo,pMaterialNo,mounterPosNo,feederNo,creater,createTime,pCompanyBin ");
			strSql.Append(" FROM smtProcessInfo ");
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
			strSql.Append(" id,machineTypeNo,productNo,productNum,materialNo,processNo,pBomNo,bomGroupNo,assLineNo,mounterNo,pMaterialNo,mounterPosNo,feederNo,creater,createTime,pCompanyBin ");
			strSql.Append(" FROM smtProcessInfo ");
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
			strSql.Append("select count(1) FROM smtProcessInfo ");
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
			strSql.Append(")AS Row, T.*  from smtProcessInfo T ");
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
			parameters[0].Value = "smtProcessInfo";
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

