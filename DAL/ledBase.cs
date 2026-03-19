using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:ledBase
	/// </summary>
	public partial class ledBase
	{
		public ledBase()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "ledBase"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from ledBase");
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
		public int Add(Model.ledBase model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ledBase(");
			strSql.Append("materialNo,materialName,type,sType,light1,color1,voltage1,light2,color2,voltage2,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@materialNo,@materialName,@type,@sType,@light1,@color1,@voltage1,@light2,@color2,@voltage2,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialName", SqlDbType.NVarChar,50),
					new SqlParameter("@type", SqlDbType.NVarChar,50),
					new SqlParameter("@sType", SqlDbType.NVarChar,50),
					new SqlParameter("@light1", SqlDbType.NVarChar,50),
					new SqlParameter("@color1", SqlDbType.NVarChar,50),
					new SqlParameter("@voltage1", SqlDbType.NVarChar,50),
					new SqlParameter("@light2", SqlDbType.NVarChar,50),
					new SqlParameter("@color2", SqlDbType.NVarChar,50),
					new SqlParameter("@voltage2", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.materialNo;
			parameters[1].Value = model.materialName;
			parameters[2].Value = model.type;
			parameters[3].Value = model.sType;
			parameters[4].Value = model.light1;
			parameters[5].Value = model.color1;
			parameters[6].Value = model.voltage1;
			parameters[7].Value = model.light2;
			parameters[8].Value = model.color2;
			parameters[9].Value = model.voltage2;
			parameters[10].Value = model.creater;
			parameters[11].Value = model.createTime;

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
		public bool Update(Model.ledBase model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ledBase set ");
			strSql.Append("materialNo=@materialNo,");
			strSql.Append("materialName=@materialName,");
			strSql.Append("type=@type,");
			strSql.Append("sType=@sType,");
			strSql.Append("light1=@light1,");
			strSql.Append("color1=@color1,");
			strSql.Append("voltage1=@voltage1,");
			strSql.Append("light2=@light2,");
			strSql.Append("color2=@color2,");
			strSql.Append("voltage2=@voltage2,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@materialName", SqlDbType.NVarChar,50),
					new SqlParameter("@type", SqlDbType.NVarChar,50),
					new SqlParameter("@sType", SqlDbType.NVarChar,50),
					new SqlParameter("@light1", SqlDbType.NVarChar,50),
					new SqlParameter("@color1", SqlDbType.NVarChar,50),
					new SqlParameter("@voltage1", SqlDbType.NVarChar,50),
					new SqlParameter("@light2", SqlDbType.NVarChar,50),
					new SqlParameter("@color2", SqlDbType.NVarChar,50),
					new SqlParameter("@voltage2", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.materialNo;
			parameters[1].Value = model.materialName;
			parameters[2].Value = model.type;
			parameters[3].Value = model.sType;
			parameters[4].Value = model.light1;
			parameters[5].Value = model.color1;
			parameters[6].Value = model.voltage1;
			parameters[7].Value = model.light2;
			parameters[8].Value = model.color2;
			parameters[9].Value = model.voltage2;
			parameters[10].Value = model.creater;
			parameters[11].Value = model.createTime;
			parameters[12].Value = model.id;

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
			strSql.Append("delete from ledBase ");
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
			strSql.Append("delete from ledBase ");
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
		public Model.ledBase GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,materialNo,materialName,type,sType,light1,color1,voltage1,light2,color2,voltage2,creater,createTime from ledBase ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.ledBase model=new Model.ledBase();
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
		public Model.ledBase DataRowToModel(DataRow row)
		{
			Model.ledBase model=new Model.ledBase();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["materialNo"]!=null)
				{
					model.materialNo=row["materialNo"].ToString();
				}
				if(row["materialName"]!=null)
				{
					model.materialName=row["materialName"].ToString();
				}
				if(row["type"]!=null)
				{
					model.type=row["type"].ToString();
				}
				if(row["sType"]!=null)
				{
					model.sType=row["sType"].ToString();
				}
				if(row["light1"]!=null)
				{
					model.light1=row["light1"].ToString();
				}
				if(row["color1"]!=null)
				{
					model.color1=row["color1"].ToString();
				}
				if(row["voltage1"]!=null)
				{
					model.voltage1=row["voltage1"].ToString();
				}
				if(row["light2"]!=null)
				{
					model.light2=row["light2"].ToString();
				}
				if(row["color2"]!=null)
				{
					model.color2=row["color2"].ToString();
				}
				if(row["voltage2"]!=null)
				{
					model.voltage2=row["voltage2"].ToString();
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
			strSql.Append("select id,materialNo,materialName,type,sType,light1,color1,voltage1,light2,color2,voltage2,creater,createTime ");
			strSql.Append(" FROM ledBase ");
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
			strSql.Append(" id,materialNo,materialName,type,sType,light1,color1,voltage1,light2,color2,voltage2,creater,createTime ");
			strSql.Append(" FROM ledBase ");
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
			strSql.Append("select count(1) FROM ledBase ");
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
			strSql.Append(")AS Row, T.*  from ledBase T ");
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
			parameters[0].Value = "ledBase";
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

