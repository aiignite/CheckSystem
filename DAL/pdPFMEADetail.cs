using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:pdPFMEADetail
	/// </summary>
	public partial class pdPFMEADetail
	{
		public pdPFMEADetail()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "pdPFMEADetail"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from pdPFMEADetail");
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
		public int Add(Model.pdPFMEADetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into pdPFMEADetail(");
			strSql.Append("pFMEANO,pBNO,pBName,pBDetailNo,custPBNO,custPDNO,requiment,pFM,pEF,severity,keyCategory,pCMF,prevention,occr,detect,det,rPN,recMeasure,sZ,dZ,pL,respAndTargetDate,mRMeasure,mRSev,mROccr,mRDec,mRRPN,checkOS,checkDS,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@pFMEANO,@pBNO,@pBName,@pBDetailNo,@custPBNO,@custPDNO,@requiment,@pFM,@pEF,@severity,@keyCategory,@pCMF,@prevention,@occr,@detect,@det,@rPN,@recMeasure,@sZ,@dZ,@pL,@respAndTargetDate,@mRMeasure,@mRSev,@mROccr,@mRDec,@mRRPN,@checkOS,@checkDS,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@pFMEANO", SqlDbType.NVarChar,50),
					new SqlParameter("@pBNO", SqlDbType.NVarChar,50),
					new SqlParameter("@pBName", SqlDbType.NVarChar,100),
					new SqlParameter("@pBDetailNo", SqlDbType.NVarChar,50),
					new SqlParameter("@custPBNO", SqlDbType.NVarChar,50),
					new SqlParameter("@custPDNO", SqlDbType.NVarChar,50),
					new SqlParameter("@requiment", SqlDbType.NVarChar,500),
					new SqlParameter("@pFM", SqlDbType.NVarChar,500),
					new SqlParameter("@pEF", SqlDbType.NVarChar,500),
					new SqlParameter("@severity", SqlDbType.Int,4),
					new SqlParameter("@keyCategory", SqlDbType.NVarChar,2),
					new SqlParameter("@pCMF", SqlDbType.NVarChar,500),
					new SqlParameter("@prevention", SqlDbType.NVarChar,500),
					new SqlParameter("@occr", SqlDbType.Int,4),
					new SqlParameter("@detect", SqlDbType.NVarChar,200),
					new SqlParameter("@det", SqlDbType.Int,4),
					new SqlParameter("@rPN", SqlDbType.NVarChar,10),
					new SqlParameter("@recMeasure", SqlDbType.NVarChar,500),
					new SqlParameter("@sZ", SqlDbType.Int,4),
					new SqlParameter("@dZ", SqlDbType.Int,4),
					new SqlParameter("@pL", SqlDbType.Int,4),
					new SqlParameter("@respAndTargetDate", SqlDbType.NVarChar,50),
					new SqlParameter("@mRMeasure", SqlDbType.NVarChar,500),
					new SqlParameter("@mRSev", SqlDbType.Int,4),
					new SqlParameter("@mROccr", SqlDbType.Int,4),
					new SqlParameter("@mRDec", SqlDbType.Int,4),
					new SqlParameter("@mRRPN", SqlDbType.NVarChar,10),
					new SqlParameter("@checkOS", SqlDbType.NVarChar,1),
					new SqlParameter("@checkDS", SqlDbType.NVarChar,1),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.pFMEANO;
			parameters[1].Value = model.pBNO;
			parameters[2].Value = model.pBName;
			parameters[3].Value = model.pBDetailNo;
			parameters[4].Value = model.custPBNO;
			parameters[5].Value = model.custPDNO;
			parameters[6].Value = model.requiment;
			parameters[7].Value = model.pFM;
			parameters[8].Value = model.pEF;
			parameters[9].Value = model.severity;
			parameters[10].Value = model.keyCategory;
			parameters[11].Value = model.pCMF;
			parameters[12].Value = model.prevention;
			parameters[13].Value = model.occr;
			parameters[14].Value = model.detect;
			parameters[15].Value = model.det;
			parameters[16].Value = model.rPN;
			parameters[17].Value = model.recMeasure;
			parameters[18].Value = model.sZ;
			parameters[19].Value = model.dZ;
			parameters[20].Value = model.pL;
			parameters[21].Value = model.respAndTargetDate;
			parameters[22].Value = model.mRMeasure;
			parameters[23].Value = model.mRSev;
			parameters[24].Value = model.mROccr;
			parameters[25].Value = model.mRDec;
			parameters[26].Value = model.mRRPN;
			parameters[27].Value = model.checkOS;
			parameters[28].Value = model.checkDS;
			parameters[29].Value = model.creater;
			parameters[30].Value = model.createTime;

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
		public bool Update(Model.pdPFMEADetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update pdPFMEADetail set ");
			strSql.Append("pFMEANO=@pFMEANO,");
			strSql.Append("pBNO=@pBNO,");
			strSql.Append("pBName=@pBName,");
			strSql.Append("pBDetailNo=@pBDetailNo,");
			strSql.Append("custPBNO=@custPBNO,");
			strSql.Append("custPDNO=@custPDNO,");
			strSql.Append("requiment=@requiment,");
			strSql.Append("pFM=@pFM,");
			strSql.Append("pEF=@pEF,");
			strSql.Append("severity=@severity,");
			strSql.Append("keyCategory=@keyCategory,");
			strSql.Append("pCMF=@pCMF,");
			strSql.Append("prevention=@prevention,");
			strSql.Append("occr=@occr,");
			strSql.Append("detect=@detect,");
			strSql.Append("det=@det,");
			strSql.Append("rPN=@rPN,");
			strSql.Append("recMeasure=@recMeasure,");
			strSql.Append("sZ=@sZ,");
			strSql.Append("dZ=@dZ,");
			strSql.Append("pL=@pL,");
			strSql.Append("respAndTargetDate=@respAndTargetDate,");
			strSql.Append("mRMeasure=@mRMeasure,");
			strSql.Append("mRSev=@mRSev,");
			strSql.Append("mROccr=@mROccr,");
			strSql.Append("mRDec=@mRDec,");
			strSql.Append("mRRPN=@mRRPN,");
			strSql.Append("checkOS=@checkOS,");
			strSql.Append("checkDS=@checkDS,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@pFMEANO", SqlDbType.NVarChar,50),
					new SqlParameter("@pBNO", SqlDbType.NVarChar,50),
					new SqlParameter("@pBName", SqlDbType.NVarChar,100),
					new SqlParameter("@pBDetailNo", SqlDbType.NVarChar,50),
					new SqlParameter("@custPBNO", SqlDbType.NVarChar,50),
					new SqlParameter("@custPDNO", SqlDbType.NVarChar,50),
					new SqlParameter("@requiment", SqlDbType.NVarChar,500),
					new SqlParameter("@pFM", SqlDbType.NVarChar,500),
					new SqlParameter("@pEF", SqlDbType.NVarChar,500),
					new SqlParameter("@severity", SqlDbType.Int,4),
					new SqlParameter("@keyCategory", SqlDbType.NVarChar,2),
					new SqlParameter("@pCMF", SqlDbType.NVarChar,500),
					new SqlParameter("@prevention", SqlDbType.NVarChar,500),
					new SqlParameter("@occr", SqlDbType.Int,4),
					new SqlParameter("@detect", SqlDbType.NVarChar,200),
					new SqlParameter("@det", SqlDbType.Int,4),
					new SqlParameter("@rPN", SqlDbType.NVarChar,10),
					new SqlParameter("@recMeasure", SqlDbType.NVarChar,500),
					new SqlParameter("@sZ", SqlDbType.Int,4),
					new SqlParameter("@dZ", SqlDbType.Int,4),
					new SqlParameter("@pL", SqlDbType.Int,4),
					new SqlParameter("@respAndTargetDate", SqlDbType.NVarChar,50),
					new SqlParameter("@mRMeasure", SqlDbType.NVarChar,500),
					new SqlParameter("@mRSev", SqlDbType.Int,4),
					new SqlParameter("@mROccr", SqlDbType.Int,4),
					new SqlParameter("@mRDec", SqlDbType.Int,4),
					new SqlParameter("@mRRPN", SqlDbType.NVarChar,10),
					new SqlParameter("@checkOS", SqlDbType.NVarChar,1),
					new SqlParameter("@checkDS", SqlDbType.NVarChar,1),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.pFMEANO;
			parameters[1].Value = model.pBNO;
			parameters[2].Value = model.pBName;
			parameters[3].Value = model.pBDetailNo;
			parameters[4].Value = model.custPBNO;
			parameters[5].Value = model.custPDNO;
			parameters[6].Value = model.requiment;
			parameters[7].Value = model.pFM;
			parameters[8].Value = model.pEF;
			parameters[9].Value = model.severity;
			parameters[10].Value = model.keyCategory;
			parameters[11].Value = model.pCMF;
			parameters[12].Value = model.prevention;
			parameters[13].Value = model.occr;
			parameters[14].Value = model.detect;
			parameters[15].Value = model.det;
			parameters[16].Value = model.rPN;
			parameters[17].Value = model.recMeasure;
			parameters[18].Value = model.sZ;
			parameters[19].Value = model.dZ;
			parameters[20].Value = model.pL;
			parameters[21].Value = model.respAndTargetDate;
			parameters[22].Value = model.mRMeasure;
			parameters[23].Value = model.mRSev;
			parameters[24].Value = model.mROccr;
			parameters[25].Value = model.mRDec;
			parameters[26].Value = model.mRRPN;
			parameters[27].Value = model.checkOS;
			parameters[28].Value = model.checkDS;
			parameters[29].Value = model.creater;
			parameters[30].Value = model.createTime;
			parameters[31].Value = model.id;

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
			strSql.Append("delete from pdPFMEADetail ");
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
			strSql.Append("delete from pdPFMEADetail ");
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
		public Model.pdPFMEADetail GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,pFMEANO,pBNO,pBName,pBDetailNo,custPBNO,custPDNO,requiment,pFM,pEF,severity,keyCategory,pCMF,prevention,occr,detect,det,rPN,recMeasure,sZ,dZ,pL,respAndTargetDate,mRMeasure,mRSev,mROccr,mRDec,mRRPN,checkOS,checkDS,creater,createTime from pdPFMEADetail ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.pdPFMEADetail model=new Model.pdPFMEADetail();
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
		public Model.pdPFMEADetail DataRowToModel(DataRow row)
		{
			Model.pdPFMEADetail model=new Model.pdPFMEADetail();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["pFMEANO"]!=null)
				{
					model.pFMEANO=row["pFMEANO"].ToString();
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
				if(row["requiment"]!=null)
				{
					model.requiment=row["requiment"].ToString();
				}
				if(row["pFM"]!=null)
				{
					model.pFM=row["pFM"].ToString();
				}
				if(row["pEF"]!=null)
				{
					model.pEF=row["pEF"].ToString();
				}
				if(row["severity"]!=null && row["severity"].ToString()!="")
				{
					model.severity=int.Parse(row["severity"].ToString());
				}
				if(row["keyCategory"]!=null)
				{
					model.keyCategory=row["keyCategory"].ToString();
				}
				if(row["pCMF"]!=null)
				{
					model.pCMF=row["pCMF"].ToString();
				}
				if(row["prevention"]!=null)
				{
					model.prevention=row["prevention"].ToString();
				}
				if(row["occr"]!=null && row["occr"].ToString()!="")
				{
					model.occr=int.Parse(row["occr"].ToString());
				}
				if(row["detect"]!=null)
				{
					model.detect=row["detect"].ToString();
				}
				if(row["det"]!=null && row["det"].ToString()!="")
				{
					model.det=int.Parse(row["det"].ToString());
				}
				if(row["rPN"]!=null)
				{
					model.rPN=row["rPN"].ToString();
				}
				if(row["recMeasure"]!=null)
				{
					model.recMeasure=row["recMeasure"].ToString();
				}
				if(row["sZ"]!=null && row["sZ"].ToString()!="")
				{
					model.sZ=int.Parse(row["sZ"].ToString());
				}
				if(row["dZ"]!=null && row["dZ"].ToString()!="")
				{
					model.dZ=int.Parse(row["dZ"].ToString());
				}
				if(row["pL"]!=null && row["pL"].ToString()!="")
				{
					model.pL=int.Parse(row["pL"].ToString());
				}
				if(row["respAndTargetDate"]!=null)
				{
					model.respAndTargetDate=row["respAndTargetDate"].ToString();
				}
				if(row["mRMeasure"]!=null)
				{
					model.mRMeasure=row["mRMeasure"].ToString();
				}
				if(row["mRSev"]!=null && row["mRSev"].ToString()!="")
				{
					model.mRSev=int.Parse(row["mRSev"].ToString());
				}
				if(row["mROccr"]!=null && row["mROccr"].ToString()!="")
				{
					model.mROccr=int.Parse(row["mROccr"].ToString());
				}
				if(row["mRDec"]!=null && row["mRDec"].ToString()!="")
				{
					model.mRDec=int.Parse(row["mRDec"].ToString());
				}
				if(row["mRRPN"]!=null)
				{
					model.mRRPN=row["mRRPN"].ToString();
				}
				if(row["checkOS"]!=null)
				{
					model.checkOS=row["checkOS"].ToString();
				}
				if(row["checkDS"]!=null)
				{
					model.checkDS=row["checkDS"].ToString();
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
			strSql.Append("select id,pFMEANO,pBNO,pBName,pBDetailNo,custPBNO,custPDNO,requiment,pFM,pEF,severity,keyCategory,pCMF,prevention,occr,detect,det,rPN,recMeasure,sZ,dZ,pL,respAndTargetDate,mRMeasure,mRSev,mROccr,mRDec,mRRPN,checkOS,checkDS,creater,createTime ");
			strSql.Append(" FROM pdPFMEADetail ");
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
			strSql.Append(" id,pFMEANO,pBNO,pBName,pBDetailNo,custPBNO,custPDNO,requiment,pFM,pEF,severity,keyCategory,pCMF,prevention,occr,detect,det,rPN,recMeasure,sZ,dZ,pL,respAndTargetDate,mRMeasure,mRSev,mROccr,mRDec,mRRPN,checkOS,checkDS,creater,createTime ");
			strSql.Append(" FROM pdPFMEADetail ");
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
			strSql.Append("select count(1) FROM pdPFMEADetail ");
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
			strSql.Append(")AS Row, T.*  from pdPFMEADetail T ");
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
			parameters[0].Value = "pdPFMEADetail";
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

