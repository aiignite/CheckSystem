using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:pdCPDetail
	/// </summary>
	public partial class pdCPDetail
	{
		public pdCPDetail()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "pdCPDetail"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from pdCPDetail");
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
		public int Add(Model.pdCPDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into pdCPDetail(");
			strSql.Append("cPNO,pBNO,pBName,pBDetailNo,custPBNO,custPDNO,pBDevice,productKey,processKey,keyCategory,fdTolSpec,fdUpTol,fdDownTol,inTolSpec,inUpTol,inDownTol,eMT,sampleCap,sampleFre,conMethod,respPerson,actPlan,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@cPNO,@pBNO,@pBName,@pBDetailNo,@custPBNO,@custPDNO,@pBDevice,@productKey,@processKey,@keyCategory,@fdTolSpec,@fdUpTol,@fdDownTol,@inTolSpec,@inUpTol,@inDownTol,@eMT,@sampleCap,@sampleFre,@conMethod,@respPerson,@actPlan,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@cPNO", SqlDbType.NVarChar,50),
					new SqlParameter("@pBNO", SqlDbType.NVarChar,50),
					new SqlParameter("@pBName", SqlDbType.NVarChar,100),
					new SqlParameter("@pBDetailNo", SqlDbType.NVarChar,50),
					new SqlParameter("@custPBNO", SqlDbType.NVarChar,50),
					new SqlParameter("@custPDNO", SqlDbType.NVarChar,50),
					new SqlParameter("@pBDevice", SqlDbType.NVarChar,100),
					new SqlParameter("@productKey", SqlDbType.NVarChar,50),
					new SqlParameter("@processKey", SqlDbType.NVarChar,50),
					new SqlParameter("@keyCategory", SqlDbType.NVarChar,2),
					new SqlParameter("@fdTolSpec", SqlDbType.NVarChar,500),
					new SqlParameter("@fdUpTol", SqlDbType.NVarChar,50),
					new SqlParameter("@fdDownTol", SqlDbType.NVarChar,50),
					new SqlParameter("@inTolSpec", SqlDbType.NVarChar,500),
					new SqlParameter("@inUpTol", SqlDbType.NVarChar,50),
					new SqlParameter("@inDownTol", SqlDbType.NVarChar,50),
					new SqlParameter("@eMT", SqlDbType.NVarChar,500),
					new SqlParameter("@sampleCap", SqlDbType.NVarChar,20),
					new SqlParameter("@sampleFre", SqlDbType.NVarChar,10),
					new SqlParameter("@conMethod", SqlDbType.NVarChar,500),
					new SqlParameter("@respPerson", SqlDbType.NVarChar,50),
					new SqlParameter("@actPlan", SqlDbType.NVarChar,500),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.cPNO;
			parameters[1].Value = model.pBNO;
			parameters[2].Value = model.pBName;
			parameters[3].Value = model.pBDetailNo;
			parameters[4].Value = model.custPBNO;
			parameters[5].Value = model.custPDNO;
			parameters[6].Value = model.pBDevice;
			parameters[7].Value = model.productKey;
			parameters[8].Value = model.processKey;
			parameters[9].Value = model.keyCategory;
			parameters[10].Value = model.fdTolSpec;
			parameters[11].Value = model.fdUpTol;
			parameters[12].Value = model.fdDownTol;
			parameters[13].Value = model.inTolSpec;
			parameters[14].Value = model.inUpTol;
			parameters[15].Value = model.inDownTol;
			parameters[16].Value = model.eMT;
			parameters[17].Value = model.sampleCap;
			parameters[18].Value = model.sampleFre;
			parameters[19].Value = model.conMethod;
			parameters[20].Value = model.respPerson;
			parameters[21].Value = model.actPlan;
			parameters[22].Value = model.creater;
			parameters[23].Value = model.createTime;

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
		public bool Update(Model.pdCPDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update pdCPDetail set ");
			strSql.Append("cPNO=@cPNO,");
			strSql.Append("pBNO=@pBNO,");
			strSql.Append("pBName=@pBName,");
			strSql.Append("pBDetailNo=@pBDetailNo,");
			strSql.Append("custPBNO=@custPBNO,");
			strSql.Append("custPDNO=@custPDNO,");
			strSql.Append("pBDevice=@pBDevice,");
			strSql.Append("productKey=@productKey,");
			strSql.Append("processKey=@processKey,");
			strSql.Append("keyCategory=@keyCategory,");
			strSql.Append("fdTolSpec=@fdTolSpec,");
			strSql.Append("fdUpTol=@fdUpTol,");
			strSql.Append("fdDownTol=@fdDownTol,");
			strSql.Append("inTolSpec=@inTolSpec,");
			strSql.Append("inUpTol=@inUpTol,");
			strSql.Append("inDownTol=@inDownTol,");
			strSql.Append("eMT=@eMT,");
			strSql.Append("sampleCap=@sampleCap,");
			strSql.Append("sampleFre=@sampleFre,");
			strSql.Append("conMethod=@conMethod,");
			strSql.Append("respPerson=@respPerson,");
			strSql.Append("actPlan=@actPlan,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@cPNO", SqlDbType.NVarChar,50),
					new SqlParameter("@pBNO", SqlDbType.NVarChar,50),
					new SqlParameter("@pBName", SqlDbType.NVarChar,100),
					new SqlParameter("@pBDetailNo", SqlDbType.NVarChar,50),
					new SqlParameter("@custPBNO", SqlDbType.NVarChar,50),
					new SqlParameter("@custPDNO", SqlDbType.NVarChar,50),
					new SqlParameter("@pBDevice", SqlDbType.NVarChar,100),
					new SqlParameter("@productKey", SqlDbType.NVarChar,50),
					new SqlParameter("@processKey", SqlDbType.NVarChar,50),
					new SqlParameter("@keyCategory", SqlDbType.NVarChar,2),
					new SqlParameter("@fdTolSpec", SqlDbType.NVarChar,500),
					new SqlParameter("@fdUpTol", SqlDbType.NVarChar,50),
					new SqlParameter("@fdDownTol", SqlDbType.NVarChar,50),
					new SqlParameter("@inTolSpec", SqlDbType.NVarChar,500),
					new SqlParameter("@inUpTol", SqlDbType.NVarChar,50),
					new SqlParameter("@inDownTol", SqlDbType.NVarChar,50),
					new SqlParameter("@eMT", SqlDbType.NVarChar,500),
					new SqlParameter("@sampleCap", SqlDbType.NVarChar,20),
					new SqlParameter("@sampleFre", SqlDbType.NVarChar,10),
					new SqlParameter("@conMethod", SqlDbType.NVarChar,500),
					new SqlParameter("@respPerson", SqlDbType.NVarChar,50),
					new SqlParameter("@actPlan", SqlDbType.NVarChar,500),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.cPNO;
			parameters[1].Value = model.pBNO;
			parameters[2].Value = model.pBName;
			parameters[3].Value = model.pBDetailNo;
			parameters[4].Value = model.custPBNO;
			parameters[5].Value = model.custPDNO;
			parameters[6].Value = model.pBDevice;
			parameters[7].Value = model.productKey;
			parameters[8].Value = model.processKey;
			parameters[9].Value = model.keyCategory;
			parameters[10].Value = model.fdTolSpec;
			parameters[11].Value = model.fdUpTol;
			parameters[12].Value = model.fdDownTol;
			parameters[13].Value = model.inTolSpec;
			parameters[14].Value = model.inUpTol;
			parameters[15].Value = model.inDownTol;
			parameters[16].Value = model.eMT;
			parameters[17].Value = model.sampleCap;
			parameters[18].Value = model.sampleFre;
			parameters[19].Value = model.conMethod;
			parameters[20].Value = model.respPerson;
			parameters[21].Value = model.actPlan;
			parameters[22].Value = model.creater;
			parameters[23].Value = model.createTime;
			parameters[24].Value = model.id;

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
			strSql.Append("delete from pdCPDetail ");
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
			strSql.Append("delete from pdCPDetail ");
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
		public Model.pdCPDetail GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,cPNO,pBNO,pBName,pBDetailNo,custPBNO,custPDNO,pBDevice,productKey,processKey,keyCategory,fdTolSpec,fdUpTol,fdDownTol,inTolSpec,inUpTol,inDownTol,eMT,sampleCap,sampleFre,conMethod,respPerson,actPlan,creater,createTime from pdCPDetail ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.pdCPDetail model=new Model.pdCPDetail();
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
		public Model.pdCPDetail DataRowToModel(DataRow row)
		{
			Model.pdCPDetail model=new Model.pdCPDetail();
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
				if(row["pBNO"]!=null)
				{
					model.pBNO=row["pBNO"].ToString();
				}
				if(row["pBName"]!=null)
				{
					model.pBName=row["pBName"].ToString();
				}
				if(row["pBDetailNo"]!=null)
				{
					model.pBDetailNo=row["pBDetailNo"].ToString();
				}
				if(row["custPBNO"]!=null)
				{
					model.custPBNO=row["custPBNO"].ToString();
				}
				if(row["custPDNO"]!=null)
				{
					model.custPDNO=row["custPDNO"].ToString();
				}
				if(row["pBDevice"]!=null)
				{
					model.pBDevice=row["pBDevice"].ToString();
				}
				if(row["productKey"]!=null)
				{
					model.productKey=row["productKey"].ToString();
				}
				if(row["processKey"]!=null)
				{
					model.processKey=row["processKey"].ToString();
				}
				if(row["keyCategory"]!=null)
				{
					model.keyCategory=row["keyCategory"].ToString();
				}
				if(row["fdTolSpec"]!=null)
				{
					model.fdTolSpec=row["fdTolSpec"].ToString();
				}
				if(row["fdUpTol"]!=null)
				{
					model.fdUpTol=row["fdUpTol"].ToString();
				}
				if(row["fdDownTol"]!=null)
				{
					model.fdDownTol=row["fdDownTol"].ToString();
				}
				if(row["inTolSpec"]!=null)
				{
					model.inTolSpec=row["inTolSpec"].ToString();
				}
				if(row["inUpTol"]!=null)
				{
					model.inUpTol=row["inUpTol"].ToString();
				}
				if(row["inDownTol"]!=null)
				{
					model.inDownTol=row["inDownTol"].ToString();
				}
				if(row["eMT"]!=null)
				{
					model.eMT=row["eMT"].ToString();
				}
				if(row["sampleCap"]!=null)
				{
					model.sampleCap=row["sampleCap"].ToString();
				}
				if(row["sampleFre"]!=null)
				{
					model.sampleFre=row["sampleFre"].ToString();
				}
				if(row["conMethod"]!=null)
				{
					model.conMethod=row["conMethod"].ToString();
				}
				if(row["respPerson"]!=null)
				{
					model.respPerson=row["respPerson"].ToString();
				}
				if(row["actPlan"]!=null)
				{
					model.actPlan=row["actPlan"].ToString();
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
			strSql.Append("select id,cPNO,pBNO,pBName,pBDetailNo,custPBNO,custPDNO,pBDevice,productKey,processKey,keyCategory,fdTolSpec,fdUpTol,fdDownTol,inTolSpec,inUpTol,inDownTol,eMT,sampleCap,sampleFre,conMethod,respPerson,actPlan,creater,createTime ");
			strSql.Append(" FROM pdCPDetail ");
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
			strSql.Append(" id,cPNO,pBNO,pBName,pBDetailNo,custPBNO,custPDNO,pBDevice,productKey,processKey,keyCategory,fdTolSpec,fdUpTol,fdDownTol,inTolSpec,inUpTol,inDownTol,eMT,sampleCap,sampleFre,conMethod,respPerson,actPlan,creater,createTime ");
			strSql.Append(" FROM pdCPDetail ");
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
			strSql.Append("select count(1) FROM pdCPDetail ");
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
			strSql.Append(")AS Row, T.*  from pdCPDetail T ");
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
			parameters[0].Value = "pdCPDetail";
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

