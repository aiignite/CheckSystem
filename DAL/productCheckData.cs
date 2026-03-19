using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:productCheckData
	/// </summary>
	public partial class productCheckData
	{
		public productCheckData()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "productCheckData"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from productCheckData");
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
		public int Add(Model.productCheckData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into productCheckData(");
			strSql.Append("productNo,processNo,checkData,checker,checkResult,pcbaNo,pcbaBarcode,productUID,taskNo,deviceNo,note,createTime,creater)");
			strSql.Append(" values (");
			strSql.Append("@productNo,@processNo,@checkData,@checker,@checkResult,@pcbaNo,@pcbaBarcode,@productUID,@taskNo,@deviceNo,@note,@createTime,@creater)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@checkData", SqlDbType.NVarChar,1000),
					new SqlParameter("@checker", SqlDbType.NVarChar,50),
					new SqlParameter("@checkResult", SqlDbType.NVarChar,50),
					new SqlParameter("@pcbaNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pcbaBarcode", SqlDbType.NVarChar,50),
					new SqlParameter("@productUID", SqlDbType.NVarChar,50),
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@deviceNo", SqlDbType.NVarChar,50),
					new SqlParameter("@note", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@creater", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.productNo;
			parameters[1].Value = model.processNo;
			parameters[2].Value = model.checkData;
			parameters[3].Value = model.checker;
			parameters[4].Value = model.checkResult;
			parameters[5].Value = model.pcbaNo;
			parameters[6].Value = model.pcbaBarcode;
			parameters[7].Value = model.productUID;
			parameters[8].Value = model.taskNo;
			parameters[9].Value = model.deviceNo;
			parameters[10].Value = model.note;
			parameters[11].Value = model.createTime;
			parameters[12].Value = model.creater;

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
		public bool Update(Model.productCheckData model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update productCheckData set ");
			strSql.Append("productNo=@productNo,");
			strSql.Append("processNo=@processNo,");
			strSql.Append("checkData=@checkData,");
			strSql.Append("checker=@checker,");
			strSql.Append("checkResult=@checkResult,");
			strSql.Append("pcbaNo=@pcbaNo,");
			strSql.Append("pcbaBarcode=@pcbaBarcode,");
			strSql.Append("productUID=@productUID,");
			strSql.Append("taskNo=@taskNo,");
			strSql.Append("deviceNo=@deviceNo,");
			strSql.Append("note=@note,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("creater=@creater");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@productNo", SqlDbType.NVarChar,50),
					new SqlParameter("@processNo", SqlDbType.NVarChar,50),
					new SqlParameter("@checkData", SqlDbType.NVarChar,1000),
					new SqlParameter("@checker", SqlDbType.NVarChar,50),
					new SqlParameter("@checkResult", SqlDbType.NVarChar,50),
					new SqlParameter("@pcbaNo", SqlDbType.NVarChar,50),
					new SqlParameter("@pcbaBarcode", SqlDbType.NVarChar,50),
					new SqlParameter("@productUID", SqlDbType.NVarChar,50),
					new SqlParameter("@taskNo", SqlDbType.NVarChar,50),
					new SqlParameter("@deviceNo", SqlDbType.NVarChar,50),
					new SqlParameter("@note", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.productNo;
			parameters[1].Value = model.processNo;
			parameters[2].Value = model.checkData;
			parameters[3].Value = model.checker;
			parameters[4].Value = model.checkResult;
			parameters[5].Value = model.pcbaNo;
			parameters[6].Value = model.pcbaBarcode;
			parameters[7].Value = model.productUID;
			parameters[8].Value = model.taskNo;
			parameters[9].Value = model.deviceNo;
			parameters[10].Value = model.note;
			parameters[11].Value = model.createTime;
			parameters[12].Value = model.creater;
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
			strSql.Append("delete from productCheckData ");
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
			strSql.Append("delete from productCheckData ");
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
		public Model.productCheckData GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,productNo,processNo,checkData,checker,checkResult,pcbaNo,pcbaBarcode,productUID,taskNo,deviceNo,note,createTime,creater from productCheckData ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.productCheckData model=new Model.productCheckData();
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
		public Model.productCheckData DataRowToModel(DataRow row)
		{
			Model.productCheckData model=new Model.productCheckData();
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
				if(row["processNo"]!=null)
				{
					model.processNo=row["processNo"].ToString();
				}
				if(row["checkData"]!=null)
				{
					model.checkData=row["checkData"].ToString();
				}
				if(row["checker"]!=null)
				{
					model.checker=row["checker"].ToString();
				}
				if(row["checkResult"]!=null)
				{
					model.checkResult=row["checkResult"].ToString();
				}
				if(row["pcbaNo"]!=null)
				{
					model.pcbaNo=row["pcbaNo"].ToString();
				}
				if(row["pcbaBarcode"]!=null)
				{
					model.pcbaBarcode=row["pcbaBarcode"].ToString();
				}
				if(row["productUID"]!=null)
				{
					model.productUID=row["productUID"].ToString();
				}
				if(row["taskNo"]!=null)
				{
					model.taskNo=row["taskNo"].ToString();
				}
				if(row["deviceNo"]!=null)
				{
					model.deviceNo=row["deviceNo"].ToString();
				}
				if(row["note"]!=null)
				{
					model.note=row["note"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
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
			strSql.Append("select id,productNo,processNo,checkData,checker,checkResult,pcbaNo,pcbaBarcode,productUID,taskNo,deviceNo,note,createTime,creater ");
			strSql.Append(" FROM productCheckData ");
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
			strSql.Append(" id,productNo,processNo,checkData,checker,checkResult,pcbaNo,pcbaBarcode,productUID,taskNo,deviceNo,note,createTime,creater ");
			strSql.Append(" FROM productCheckData ");
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
			strSql.Append("select count(1) FROM productCheckData ");
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
			strSql.Append(")AS Row, T.*  from productCheckData T ");
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
			parameters[0].Value = "productCheckData";
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

