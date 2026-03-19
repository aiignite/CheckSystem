using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:salaryBaseInfo
	/// </summary>
	public partial class salaryBaseInfo
	{
		public salaryBaseInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "salaryBaseInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from salaryBaseInfo");
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
		public int Add(Model.salaryBaseInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into salaryBaseInfo(");
			strSql.Append("dept,deptNo,staffNo,name,identityNo,baseSalary,postSalary,deduction,childEdu,pension,houseLoan,houseRent,furtherEdu,initPeriod,postState,reviser,reviseDate,creater,createTime,belongDept,leaveDate)");
			strSql.Append(" values (");
			strSql.Append("@dept,@deptNo,@staffNo,@name,@identityNo,@baseSalary,@postSalary,@deduction,@childEdu,@pension,@houseLoan,@houseRent,@furtherEdu,@initPeriod,@postState,@reviser,@reviseDate,@creater,@createTime,@belongDept,@leaveDate)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@dept", SqlDbType.NVarChar,50),
					new SqlParameter("@deptNo", SqlDbType.NVarChar,50),
					new SqlParameter("@staffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@name", SqlDbType.NVarChar,50),
					new SqlParameter("@identityNo", SqlDbType.NVarChar,50),
					new SqlParameter("@baseSalary", SqlDbType.NVarChar,50),
					new SqlParameter("@postSalary", SqlDbType.NVarChar,50),
					new SqlParameter("@deduction", SqlDbType.NVarChar,50),
					new SqlParameter("@childEdu", SqlDbType.NVarChar,50),
					new SqlParameter("@pension", SqlDbType.NVarChar,50),
					new SqlParameter("@houseLoan", SqlDbType.NVarChar,50),
					new SqlParameter("@houseRent", SqlDbType.NVarChar,50),
					new SqlParameter("@furtherEdu", SqlDbType.NVarChar,50),
					new SqlParameter("@initPeriod", SqlDbType.NVarChar,50),
					new SqlParameter("@postState", SqlDbType.NVarChar,50),
					new SqlParameter("@reviser", SqlDbType.NVarChar,50),
					new SqlParameter("@reviseDate", SqlDbType.DateTime),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@belongDept", SqlDbType.NVarChar,50),
					new SqlParameter("@leaveDate", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.dept;
			parameters[1].Value = model.deptNo;
			parameters[2].Value = model.staffNo;
			parameters[3].Value = model.name;
			parameters[4].Value = model.identityNo;
			parameters[5].Value = model.baseSalary;
			parameters[6].Value = model.postSalary;
			parameters[7].Value = model.deduction;
			parameters[8].Value = model.childEdu;
			parameters[9].Value = model.pension;
			parameters[10].Value = model.houseLoan;
			parameters[11].Value = model.houseRent;
			parameters[12].Value = model.furtherEdu;
			parameters[13].Value = model.initPeriod;
			parameters[14].Value = model.postState;
			parameters[15].Value = model.reviser;
			parameters[16].Value = model.reviseDate;
			parameters[17].Value = model.creater;
			parameters[18].Value = model.createTime;
			parameters[19].Value = model.belongDept;
			parameters[20].Value = model.leaveDate;

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
		public bool Update(Model.salaryBaseInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update salaryBaseInfo set ");
			strSql.Append("dept=@dept,");
			strSql.Append("deptNo=@deptNo,");
			strSql.Append("staffNo=@staffNo,");
			strSql.Append("name=@name,");
			strSql.Append("identityNo=@identityNo,");
			strSql.Append("baseSalary=@baseSalary,");
			strSql.Append("postSalary=@postSalary,");
			strSql.Append("deduction=@deduction,");
			strSql.Append("childEdu=@childEdu,");
			strSql.Append("pension=@pension,");
			strSql.Append("houseLoan=@houseLoan,");
			strSql.Append("houseRent=@houseRent,");
			strSql.Append("furtherEdu=@furtherEdu,");
			strSql.Append("initPeriod=@initPeriod,");
			strSql.Append("postState=@postState,");
			strSql.Append("reviser=@reviser,");
			strSql.Append("reviseDate=@reviseDate,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("belongDept=@belongDept,");
			strSql.Append("leaveDate=@leaveDate");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@dept", SqlDbType.NVarChar,50),
					new SqlParameter("@deptNo", SqlDbType.NVarChar,50),
					new SqlParameter("@staffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@name", SqlDbType.NVarChar,50),
					new SqlParameter("@identityNo", SqlDbType.NVarChar,50),
					new SqlParameter("@baseSalary", SqlDbType.NVarChar,50),
					new SqlParameter("@postSalary", SqlDbType.NVarChar,50),
					new SqlParameter("@deduction", SqlDbType.NVarChar,50),
					new SqlParameter("@childEdu", SqlDbType.NVarChar,50),
					new SqlParameter("@pension", SqlDbType.NVarChar,50),
					new SqlParameter("@houseLoan", SqlDbType.NVarChar,50),
					new SqlParameter("@houseRent", SqlDbType.NVarChar,50),
					new SqlParameter("@furtherEdu", SqlDbType.NVarChar,50),
					new SqlParameter("@initPeriod", SqlDbType.NVarChar,50),
					new SqlParameter("@postState", SqlDbType.NVarChar,50),
					new SqlParameter("@reviser", SqlDbType.NVarChar,50),
					new SqlParameter("@reviseDate", SqlDbType.DateTime),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@belongDept", SqlDbType.NVarChar,50),
					new SqlParameter("@leaveDate", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.dept;
			parameters[1].Value = model.deptNo;
			parameters[2].Value = model.staffNo;
			parameters[3].Value = model.name;
			parameters[4].Value = model.identityNo;
			parameters[5].Value = model.baseSalary;
			parameters[6].Value = model.postSalary;
			parameters[7].Value = model.deduction;
			parameters[8].Value = model.childEdu;
			parameters[9].Value = model.pension;
			parameters[10].Value = model.houseLoan;
			parameters[11].Value = model.houseRent;
			parameters[12].Value = model.furtherEdu;
			parameters[13].Value = model.initPeriod;
			parameters[14].Value = model.postState;
			parameters[15].Value = model.reviser;
			parameters[16].Value = model.reviseDate;
			parameters[17].Value = model.creater;
			parameters[18].Value = model.createTime;
			parameters[19].Value = model.belongDept;
			parameters[20].Value = model.leaveDate;
			parameters[21].Value = model.id;

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
			strSql.Append("delete from salaryBaseInfo ");
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
			strSql.Append("delete from salaryBaseInfo ");
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
		public Model.salaryBaseInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,dept,deptNo,staffNo,name,identityNo,baseSalary,postSalary,deduction,childEdu,pension,houseLoan,houseRent,furtherEdu,initPeriod,postState,reviser,reviseDate,creater,createTime,belongDept,leaveDate from salaryBaseInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.salaryBaseInfo model=new Model.salaryBaseInfo();
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
		public Model.salaryBaseInfo DataRowToModel(DataRow row)
		{
			Model.salaryBaseInfo model=new Model.salaryBaseInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["dept"]!=null)
				{
					model.dept=row["dept"].ToString();
				}
				if(row["deptNo"]!=null)
				{
					model.deptNo=row["deptNo"].ToString();
				}
				if(row["staffNo"]!=null)
				{
					model.staffNo=row["staffNo"].ToString();
				}
				if(row["name"]!=null)
				{
					model.name=row["name"].ToString();
				}
				if(row["identityNo"]!=null)
				{
					model.identityNo=row["identityNo"].ToString();
				}
				if(row["baseSalary"]!=null)
				{
					model.baseSalary=row["baseSalary"].ToString();
				}
				if(row["postSalary"]!=null)
				{
					model.postSalary=row["postSalary"].ToString();
				}
				if(row["deduction"]!=null)
				{
					model.deduction=row["deduction"].ToString();
				}
				if(row["childEdu"]!=null)
				{
					model.childEdu=row["childEdu"].ToString();
				}
				if(row["pension"]!=null)
				{
					model.pension=row["pension"].ToString();
				}
				if(row["houseLoan"]!=null)
				{
					model.houseLoan=row["houseLoan"].ToString();
				}
				if(row["houseRent"]!=null)
				{
					model.houseRent=row["houseRent"].ToString();
				}
				if(row["furtherEdu"]!=null)
				{
					model.furtherEdu=row["furtherEdu"].ToString();
				}
				if(row["initPeriod"]!=null)
				{
					model.initPeriod=row["initPeriod"].ToString();
				}
				if(row["postState"]!=null)
				{
					model.postState=row["postState"].ToString();
				}
				if(row["reviser"]!=null)
				{
					model.reviser=row["reviser"].ToString();
				}
				if(row["reviseDate"]!=null && row["reviseDate"].ToString()!="")
				{
					model.reviseDate=DateTime.Parse(row["reviseDate"].ToString());
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["belongDept"]!=null)
				{
					model.belongDept=row["belongDept"].ToString();
				}
				if(row["leaveDate"]!=null)
				{
					model.leaveDate=row["leaveDate"].ToString();
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
			strSql.Append("select id,dept,deptNo,staffNo,name,identityNo,baseSalary,postSalary,deduction,childEdu,pension,houseLoan,houseRent,furtherEdu,initPeriod,postState,reviser,reviseDate,creater,createTime,belongDept,leaveDate ");
			strSql.Append(" FROM salaryBaseInfo ");
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
			strSql.Append(" id,dept,deptNo,staffNo,name,identityNo,baseSalary,postSalary,deduction,childEdu,pension,houseLoan,houseRent,furtherEdu,initPeriod,postState,reviser,reviseDate,creater,createTime,belongDept,leaveDate ");
			strSql.Append(" FROM salaryBaseInfo ");
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
			strSql.Append("select count(1) FROM salaryBaseInfo ");
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
			strSql.Append(")AS Row, T.*  from salaryBaseInfo T ");
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
			parameters[0].Value = "salaryBaseInfo";
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

