using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:sampleLabelPrint
	/// </summary>
	public partial class sampleLabelPrint
	{
		public sampleLabelPrint()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "sampleLabelPrint"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from sampleLabelPrint");
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
		public int Add(Model.sampleLabelPrint model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into sampleLabelPrint(");
			strSql.Append("materialName,customerNo,num,producer,produceDate,partNo,orderNo,mark,printNum,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@materialName,@customerNo,@num,@producer,@produceDate,@partNo,@orderNo,@mark,@printNum,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@materialName", SqlDbType.NVarChar,50),
					new SqlParameter("@customerNo", SqlDbType.NVarChar,50),
					new SqlParameter("@num", SqlDbType.Int,4),
					new SqlParameter("@producer", SqlDbType.NVarChar,50),
					new SqlParameter("@produceDate", SqlDbType.DateTime),
					new SqlParameter("@partNo", SqlDbType.NVarChar,50),
					new SqlParameter("@orderNo", SqlDbType.NVarChar,50),
					new SqlParameter("@mark", SqlDbType.NVarChar,50),
					new SqlParameter("@printNum", SqlDbType.Int,4),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.materialName;
			parameters[1].Value = model.customerNo;
			parameters[2].Value = model.num;
			parameters[3].Value = model.producer;
			parameters[4].Value = model.produceDate;
			parameters[5].Value = model.partNo;
			parameters[6].Value = model.orderNo;
			parameters[7].Value = model.mark;
			parameters[8].Value = model.printNum;
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
		public bool Update(Model.sampleLabelPrint model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update sampleLabelPrint set ");
			strSql.Append("materialName=@materialName,");
			strSql.Append("customerNo=@customerNo,");
			strSql.Append("num=@num,");
			strSql.Append("producer=@producer,");
			strSql.Append("produceDate=@produceDate,");
			strSql.Append("partNo=@partNo,");
			strSql.Append("orderNo=@orderNo,");
			strSql.Append("mark=@mark,");
			strSql.Append("printNum=@printNum,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@materialName", SqlDbType.NVarChar,50),
					new SqlParameter("@customerNo", SqlDbType.NVarChar,50),
					new SqlParameter("@num", SqlDbType.Int,4),
					new SqlParameter("@producer", SqlDbType.NVarChar,50),
					new SqlParameter("@produceDate", SqlDbType.DateTime),
					new SqlParameter("@partNo", SqlDbType.NVarChar,50),
					new SqlParameter("@orderNo", SqlDbType.NVarChar,50),
					new SqlParameter("@mark", SqlDbType.NVarChar,50),
					new SqlParameter("@printNum", SqlDbType.Int,4),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.materialName;
			parameters[1].Value = model.customerNo;
			parameters[2].Value = model.num;
			parameters[3].Value = model.producer;
			parameters[4].Value = model.produceDate;
			parameters[5].Value = model.partNo;
			parameters[6].Value = model.orderNo;
			parameters[7].Value = model.mark;
			parameters[8].Value = model.printNum;
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
			strSql.Append("delete from sampleLabelPrint ");
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
			strSql.Append("delete from sampleLabelPrint ");
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
		public Model.sampleLabelPrint GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,materialName,customerNo,num,producer,produceDate,partNo,orderNo,mark,printNum,creater,createTime from sampleLabelPrint ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.sampleLabelPrint model=new Model.sampleLabelPrint();
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
		public Model.sampleLabelPrint DataRowToModel(DataRow row)
		{
			Model.sampleLabelPrint model=new Model.sampleLabelPrint();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["materialName"]!=null)
				{
					model.materialName=row["materialName"].ToString();
				}
				if(row["customerNo"]!=null)
				{
					model.customerNo=row["customerNo"].ToString();
				}
				if(row["num"]!=null && row["num"].ToString()!="")
				{
					model.num=int.Parse(row["num"].ToString());
				}
				if(row["producer"]!=null)
				{
					model.producer=row["producer"].ToString();
				}
				if(row["produceDate"]!=null && row["produceDate"].ToString()!="")
				{
					model.produceDate=DateTime.Parse(row["produceDate"].ToString());
				}
				if(row["partNo"]!=null)
				{
					model.partNo=row["partNo"].ToString();
				}
				if(row["orderNo"]!=null)
				{
					model.orderNo=row["orderNo"].ToString();
				}
				if(row["mark"]!=null)
				{
					model.mark=row["mark"].ToString();
				}
				if(row["printNum"]!=null && row["printNum"].ToString()!="")
				{
					model.printNum=int.Parse(row["printNum"].ToString());
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
			strSql.Append("select id,materialName,customerNo,num,producer,produceDate,partNo,orderNo,mark,printNum,creater,createTime ");
			strSql.Append(" FROM sampleLabelPrint ");
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
			strSql.Append(" id,materialName,customerNo,num,producer,produceDate,partNo,orderNo,mark,printNum,creater,createTime ");
			strSql.Append(" FROM sampleLabelPrint ");
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
			strSql.Append("select count(1) FROM sampleLabelPrint ");
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
			strSql.Append(")AS Row, T.*  from sampleLabelPrint T ");
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
			parameters[0].Value = "sampleLabelPrint";
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

