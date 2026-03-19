using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:ledPbom
	/// </summary>
	public partial class ledPbom
	{
		public ledPbom()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "ledPbom"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from ledPbom");
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
		public int Add(Model.ledPbom model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ledPbom(");
			strSql.Append("commNo,usage,groupIndex,spec,materialIndex,materialNo,num,materialName,positionNo,sType,remark,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@commNo,@usage,@groupIndex,@spec,@materialIndex,@materialNo,@num,@materialName,@positionNo,@sType,@remark,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@commNo", SqlDbType.NVarChar,50),
					new SqlParameter("@usage", SqlDbType.NVarChar,50),
					new SqlParameter("@groupIndex", SqlDbType.NVarChar,50),
					new SqlParameter("@spec", SqlDbType.NVarChar,150),
					new SqlParameter("@materialIndex", SqlDbType.Int,4),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@num", SqlDbType.Int,4),
					new SqlParameter("@materialName", SqlDbType.NVarChar,100),
					new SqlParameter("@positionNo", SqlDbType.NVarChar,250),
					new SqlParameter("@sType", SqlDbType.NVarChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.commNo;
			parameters[1].Value = model.usage;
			parameters[2].Value = model.groupIndex;
			parameters[3].Value = model.spec;
			parameters[4].Value = model.materialIndex;
			parameters[5].Value = model.materialNo;
			parameters[6].Value = model.num;
			parameters[7].Value = model.materialName;
			parameters[8].Value = model.positionNo;
			parameters[9].Value = model.sType;
			parameters[10].Value = model.remark;
			parameters[11].Value = model.creater;
			parameters[12].Value = model.createTime;

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
		public bool Update(Model.ledPbom model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ledPbom set ");
			strSql.Append("commNo=@commNo,");
			strSql.Append("usage=@usage,");
			strSql.Append("groupIndex=@groupIndex,");
			strSql.Append("spec=@spec,");
			strSql.Append("materialIndex=@materialIndex,");
			strSql.Append("materialNo=@materialNo,");
			strSql.Append("num=@num,");
			strSql.Append("materialName=@materialName,");
			strSql.Append("positionNo=@positionNo,");
			strSql.Append("sType=@sType,");
			strSql.Append("remark=@remark,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@commNo", SqlDbType.NVarChar,50),
					new SqlParameter("@usage", SqlDbType.NVarChar,50),
					new SqlParameter("@groupIndex", SqlDbType.NVarChar,50),
					new SqlParameter("@spec", SqlDbType.NVarChar,150),
					new SqlParameter("@materialIndex", SqlDbType.Int,4),
					new SqlParameter("@materialNo", SqlDbType.NVarChar,50),
					new SqlParameter("@num", SqlDbType.Int,4),
					new SqlParameter("@materialName", SqlDbType.NVarChar,100),
					new SqlParameter("@positionNo", SqlDbType.NVarChar,250),
					new SqlParameter("@sType", SqlDbType.NVarChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.commNo;
			parameters[1].Value = model.usage;
			parameters[2].Value = model.groupIndex;
			parameters[3].Value = model.spec;
			parameters[4].Value = model.materialIndex;
			parameters[5].Value = model.materialNo;
			parameters[6].Value = model.num;
			parameters[7].Value = model.materialName;
			parameters[8].Value = model.positionNo;
			parameters[9].Value = model.sType;
			parameters[10].Value = model.remark;
			parameters[11].Value = model.creater;
			parameters[12].Value = model.createTime;
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
			strSql.Append("delete from ledPbom ");
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
			strSql.Append("delete from ledPbom ");
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
		public Model.ledPbom GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,commNo,usage,groupIndex,spec,materialIndex,materialNo,num,materialName,positionNo,sType,remark,creater,createTime from ledPbom ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.ledPbom model=new Model.ledPbom();
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
		public Model.ledPbom DataRowToModel(DataRow row)
		{
			Model.ledPbom model=new Model.ledPbom();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["commNo"]!=null)
				{
					model.commNo=row["commNo"].ToString();
				}
				if(row["usage"]!=null)
				{
					model.usage=row["usage"].ToString();
				}
				if(row["groupIndex"]!=null)
				{
					model.groupIndex=row["groupIndex"].ToString();
				}
				if(row["spec"]!=null)
				{
					model.spec=row["spec"].ToString();
				}
				if(row["materialIndex"]!=null && row["materialIndex"].ToString()!="")
				{
					model.materialIndex=int.Parse(row["materialIndex"].ToString());
				}
				if(row["materialNo"]!=null)
				{
					model.materialNo=row["materialNo"].ToString();
				}
				if(row["num"]!=null && row["num"].ToString()!="")
				{
					model.num=int.Parse(row["num"].ToString());
				}
				if(row["materialName"]!=null)
				{
					model.materialName=row["materialName"].ToString();
				}
				if(row["positionNo"]!=null)
				{
					model.positionNo=row["positionNo"].ToString();
				}
				if(row["sType"]!=null)
				{
					model.sType=row["sType"].ToString();
				}
				if(row["remark"]!=null)
				{
					model.remark=row["remark"].ToString();
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
			strSql.Append("select id,commNo,usage,groupIndex,spec,materialIndex,materialNo,num,materialName,positionNo,sType,remark,creater,createTime ");
			strSql.Append(" FROM ledPbom ");
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
			strSql.Append(" id,commNo,usage,groupIndex,spec,materialIndex,materialNo,num,materialName,positionNo,sType,remark,creater,createTime ");
			strSql.Append(" FROM ledPbom ");
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
			strSql.Append("select count(1) FROM ledPbom ");
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
			strSql.Append(")AS Row, T.*  from ledPbom T ");
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
			parameters[0].Value = "ledPbom";
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

