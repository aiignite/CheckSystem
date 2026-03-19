using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:smtPBomInfo
	/// </summary>
	public partial class smtPBomInfo
	{
		public smtPBomInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "smtPBomInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from smtPBomInfo");
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
		public int Add(Model.smtPBomInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into smtPBomInfo(");
			strSql.Append("pBomNo,pUse,pGroupNo,pFeature,pMaterialOrder,pMaterialNo,pMaterialNum,pMaterialName,pPositionNo,pBin,pCompanyBin,pMemo,pSpecialMemo1)");
			strSql.Append(" values (");
			strSql.Append("@pBomNo,@pUse,@pGroupNo,@pFeature,@pMaterialOrder,@pMaterialNo,@pMaterialNum,@pMaterialName,@pPositionNo,@pBin,@pCompanyBin,@pMemo,@pSpecialMemo1)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@pBomNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pUse", SqlDbType.NVarChar,50),
					new SqlParameter("@pGroupNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pFeature", SqlDbType.NVarChar,50),
					new SqlParameter("@pMaterialOrder", SqlDbType.NVarChar,50),
					new SqlParameter("@pMaterialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pMaterialNum", SqlDbType.NVarChar,50),
					new SqlParameter("@pMaterialName", SqlDbType.NVarChar,50),
					new SqlParameter("@pPositionNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pBin", SqlDbType.NVarChar,50),
					new SqlParameter("@pCompanyBin", SqlDbType.NVarChar,50),
					new SqlParameter("@pMemo", SqlDbType.NVarChar,50),
					new SqlParameter("@pSpecialMemo1", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.pBomNo;
			parameters[1].Value = model.pUse;
			parameters[2].Value = model.pGroupNo;
			parameters[3].Value = model.pFeature;
			parameters[4].Value = model.pMaterialOrder;
			parameters[5].Value = model.pMaterialNo;
			parameters[6].Value = model.pMaterialNum;
			parameters[7].Value = model.pMaterialName;
			parameters[8].Value = model.pPositionNo;
			parameters[9].Value = model.pBin;
			parameters[10].Value = model.pCompanyBin;
			parameters[11].Value = model.pMemo;
			parameters[12].Value = model.pSpecialMemo1;

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
		public bool Update(Model.smtPBomInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update smtPBomInfo set ");
			strSql.Append("pBomNo=@pBomNo,");
			strSql.Append("pUse=@pUse,");
			strSql.Append("pGroupNo=@pGroupNo,");
			strSql.Append("pFeature=@pFeature,");
			strSql.Append("pMaterialOrder=@pMaterialOrder,");
			strSql.Append("pMaterialNo=@pMaterialNo,");
			strSql.Append("pMaterialNum=@pMaterialNum,");
			strSql.Append("pMaterialName=@pMaterialName,");
			strSql.Append("pPositionNo=@pPositionNo,");
			strSql.Append("pBin=@pBin,");
			strSql.Append("pCompanyBin=@pCompanyBin,");
			strSql.Append("pMemo=@pMemo,");
			strSql.Append("pSpecialMemo1=@pSpecialMemo1");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@pBomNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pUse", SqlDbType.NVarChar,50),
					new SqlParameter("@pGroupNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pFeature", SqlDbType.NVarChar,50),
					new SqlParameter("@pMaterialOrder", SqlDbType.NVarChar,50),
					new SqlParameter("@pMaterialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pMaterialNum", SqlDbType.NVarChar,50),
					new SqlParameter("@pMaterialName", SqlDbType.NVarChar,50),
					new SqlParameter("@pPositionNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pBin", SqlDbType.NVarChar,50),
					new SqlParameter("@pCompanyBin", SqlDbType.NVarChar,50),
					new SqlParameter("@pMemo", SqlDbType.NVarChar,50),
					new SqlParameter("@pSpecialMemo1", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.pBomNo;
			parameters[1].Value = model.pUse;
			parameters[2].Value = model.pGroupNo;
			parameters[3].Value = model.pFeature;
			parameters[4].Value = model.pMaterialOrder;
			parameters[5].Value = model.pMaterialNo;
			parameters[6].Value = model.pMaterialNum;
			parameters[7].Value = model.pMaterialName;
			parameters[8].Value = model.pPositionNo;
			parameters[9].Value = model.pBin;
			parameters[10].Value = model.pCompanyBin;
			parameters[11].Value = model.pMemo;
			parameters[12].Value = model.pSpecialMemo1;
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
			strSql.Append("delete from smtPBomInfo ");
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
			strSql.Append("delete from smtPBomInfo ");
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
		public Model.smtPBomInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,pBomNo,pUse,pGroupNo,pFeature,pMaterialOrder,pMaterialNo,pMaterialNum,pMaterialName,pPositionNo,pBin,pCompanyBin,pMemo,pSpecialMemo1 from smtPBomInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.smtPBomInfo model=new Model.smtPBomInfo();
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
		public Model.smtPBomInfo DataRowToModel(DataRow row)
		{
			Model.smtPBomInfo model=new Model.smtPBomInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["pBomNo"]!=null)
				{
					model.pBomNo=row["pBomNo"].ToString();
				}
				if(row["pUse"]!=null)
				{
					model.pUse=row["pUse"].ToString();
				}
				if(row["pGroupNo"]!=null)
				{
					model.pGroupNo=row["pGroupNo"].ToString();
				}
				if(row["pFeature"]!=null)
				{
					model.pFeature=row["pFeature"].ToString();
				}
				if(row["pMaterialOrder"]!=null)
				{
					model.pMaterialOrder=row["pMaterialOrder"].ToString();
				}
				if(row["pMaterialNo"]!=null)
				{
					model.pMaterialNo=row["pMaterialNo"].ToString();
				}
				if(row["pMaterialNum"]!=null)
				{
					model.pMaterialNum=row["pMaterialNum"].ToString();
				}
				if(row["pMaterialName"]!=null)
				{
					model.pMaterialName=row["pMaterialName"].ToString();
				}
				if(row["pPositionNo"]!=null)
				{
					model.pPositionNo=row["pPositionNo"].ToString();
				}
				if(row["pBin"]!=null)
				{
					model.pBin=row["pBin"].ToString();
				}
				if(row["pCompanyBin"]!=null)
				{
					model.pCompanyBin=row["pCompanyBin"].ToString();
				}
				if(row["pMemo"]!=null)
				{
					model.pMemo=row["pMemo"].ToString();
				}
				if(row["pSpecialMemo1"]!=null)
				{
					model.pSpecialMemo1=row["pSpecialMemo1"].ToString();
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
			strSql.Append("select id,pBomNo,pUse,pGroupNo,pFeature,pMaterialOrder,pMaterialNo,pMaterialNum,pMaterialName,pPositionNo,pBin,pCompanyBin,pMemo,pSpecialMemo1 ");
			strSql.Append(" FROM smtPBomInfo ");
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
			strSql.Append(" id,pBomNo,pUse,pGroupNo,pFeature,pMaterialOrder,pMaterialNo,pMaterialNum,pMaterialName,pPositionNo,pBin,pCompanyBin,pMemo,pSpecialMemo1 ");
			strSql.Append(" FROM smtPBomInfo ");
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
			strSql.Append("select count(1) FROM smtPBomInfo ");
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
			strSql.Append(")AS Row, T.*  from smtPBomInfo T ");
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
			parameters[0].Value = "smtPBomInfo";
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

