using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:salaryChangeInfo
	/// </summary>
	public partial class salaryChangeInfo
	{
		public salaryChangeInfo()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "salaryChangeInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from salaryChangeInfo");
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
		public int Add(Model.salaryChangeInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into salaryChangeInfo(");
			strSql.Append("staffNo,meritPay,overtimePay,sickLeavePay,transFee,houseFee,foodFee,communFee,others,annuity,medicalInsur,unemployInsur,accumulFund,personalTax,otherFee,dues,onechildBonus,taxBack,date,creater,createTime,yearOnceBonus)");
			strSql.Append(" values (");
			strSql.Append("@staffNo,@meritPay,@overtimePay,@sickLeavePay,@transFee,@houseFee,@foodFee,@communFee,@others,@annuity,@medicalInsur,@unemployInsur,@accumulFund,@personalTax,@otherFee,@dues,@onechildBonus,@taxBack,@date,@creater,@createTime,@yearOnceBonus)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@staffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@meritPay", SqlDbType.NVarChar,50),
					new SqlParameter("@overtimePay", SqlDbType.NVarChar,50),
					new SqlParameter("@sickLeavePay", SqlDbType.NVarChar,50),
					new SqlParameter("@transFee", SqlDbType.NVarChar,50),
					new SqlParameter("@houseFee", SqlDbType.NVarChar,50),
					new SqlParameter("@foodFee", SqlDbType.NVarChar,50),
					new SqlParameter("@communFee", SqlDbType.NVarChar,50),
					new SqlParameter("@others", SqlDbType.NVarChar,50),
					new SqlParameter("@annuity", SqlDbType.NVarChar,50),
					new SqlParameter("@medicalInsur", SqlDbType.NVarChar,50),
					new SqlParameter("@unemployInsur", SqlDbType.NVarChar,50),
					new SqlParameter("@accumulFund", SqlDbType.NVarChar,50),
					new SqlParameter("@personalTax", SqlDbType.NVarChar,50),
					new SqlParameter("@otherFee", SqlDbType.NVarChar,50),
					new SqlParameter("@dues", SqlDbType.NVarChar,50),
					new SqlParameter("@onechildBonus", SqlDbType.NVarChar,50),
					new SqlParameter("@taxBack", SqlDbType.NVarChar,50),
					new SqlParameter("@date", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@yearOnceBonus", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.staffNo;
			parameters[1].Value = model.meritPay;
			parameters[2].Value = model.overtimePay;
			parameters[3].Value = model.sickLeavePay;
			parameters[4].Value = model.transFee;
			parameters[5].Value = model.houseFee;
			parameters[6].Value = model.foodFee;
			parameters[7].Value = model.communFee;
			parameters[8].Value = model.others;
			parameters[9].Value = model.annuity;
			parameters[10].Value = model.medicalInsur;
			parameters[11].Value = model.unemployInsur;
			parameters[12].Value = model.accumulFund;
			parameters[13].Value = model.personalTax;
			parameters[14].Value = model.otherFee;
			parameters[15].Value = model.dues;
			parameters[16].Value = model.onechildBonus;
			parameters[17].Value = model.taxBack;
			parameters[18].Value = model.date;
			parameters[19].Value = model.creater;
			parameters[20].Value = model.createTime;
			parameters[21].Value = model.yearOnceBonus;

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
		public bool Update(Model.salaryChangeInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update salaryChangeInfo set ");
			strSql.Append("staffNo=@staffNo,");
			strSql.Append("meritPay=@meritPay,");
			strSql.Append("overtimePay=@overtimePay,");
			strSql.Append("sickLeavePay=@sickLeavePay,");
			strSql.Append("transFee=@transFee,");
			strSql.Append("houseFee=@houseFee,");
			strSql.Append("foodFee=@foodFee,");
			strSql.Append("communFee=@communFee,");
			strSql.Append("others=@others,");
			strSql.Append("annuity=@annuity,");
			strSql.Append("medicalInsur=@medicalInsur,");
			strSql.Append("unemployInsur=@unemployInsur,");
			strSql.Append("accumulFund=@accumulFund,");
			strSql.Append("personalTax=@personalTax,");
			strSql.Append("otherFee=@otherFee,");
			strSql.Append("dues=@dues,");
			strSql.Append("onechildBonus=@onechildBonus,");
			strSql.Append("taxBack=@taxBack,");
			strSql.Append("date=@date,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime,");
			strSql.Append("yearOnceBonus=@yearOnceBonus");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@staffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@meritPay", SqlDbType.NVarChar,50),
					new SqlParameter("@overtimePay", SqlDbType.NVarChar,50),
					new SqlParameter("@sickLeavePay", SqlDbType.NVarChar,50),
					new SqlParameter("@transFee", SqlDbType.NVarChar,50),
					new SqlParameter("@houseFee", SqlDbType.NVarChar,50),
					new SqlParameter("@foodFee", SqlDbType.NVarChar,50),
					new SqlParameter("@communFee", SqlDbType.NVarChar,50),
					new SqlParameter("@others", SqlDbType.NVarChar,50),
					new SqlParameter("@annuity", SqlDbType.NVarChar,50),
					new SqlParameter("@medicalInsur", SqlDbType.NVarChar,50),
					new SqlParameter("@unemployInsur", SqlDbType.NVarChar,50),
					new SqlParameter("@accumulFund", SqlDbType.NVarChar,50),
					new SqlParameter("@personalTax", SqlDbType.NVarChar,50),
					new SqlParameter("@otherFee", SqlDbType.NVarChar,50),
					new SqlParameter("@dues", SqlDbType.NVarChar,50),
					new SqlParameter("@onechildBonus", SqlDbType.NVarChar,50),
					new SqlParameter("@taxBack", SqlDbType.NVarChar,50),
					new SqlParameter("@date", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@yearOnceBonus", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.staffNo;
			parameters[1].Value = model.meritPay;
			parameters[2].Value = model.overtimePay;
			parameters[3].Value = model.sickLeavePay;
			parameters[4].Value = model.transFee;
			parameters[5].Value = model.houseFee;
			parameters[6].Value = model.foodFee;
			parameters[7].Value = model.communFee;
			parameters[8].Value = model.others;
			parameters[9].Value = model.annuity;
			parameters[10].Value = model.medicalInsur;
			parameters[11].Value = model.unemployInsur;
			parameters[12].Value = model.accumulFund;
			parameters[13].Value = model.personalTax;
			parameters[14].Value = model.otherFee;
			parameters[15].Value = model.dues;
			parameters[16].Value = model.onechildBonus;
			parameters[17].Value = model.taxBack;
			parameters[18].Value = model.date;
			parameters[19].Value = model.creater;
			parameters[20].Value = model.createTime;
			parameters[21].Value = model.yearOnceBonus;
			parameters[22].Value = model.id;

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
			strSql.Append("delete from salaryChangeInfo ");
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
			strSql.Append("delete from salaryChangeInfo ");
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
		public Model.salaryChangeInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,staffNo,meritPay,overtimePay,sickLeavePay,transFee,houseFee,foodFee,communFee,others,annuity,medicalInsur,unemployInsur,accumulFund,personalTax,otherFee,dues,onechildBonus,taxBack,date,creater,createTime,yearOnceBonus from salaryChangeInfo ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.salaryChangeInfo model=new Model.salaryChangeInfo();
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
		public Model.salaryChangeInfo DataRowToModel(DataRow row)
		{
			Model.salaryChangeInfo model=new Model.salaryChangeInfo();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["staffNo"]!=null)
				{
					model.staffNo=row["staffNo"].ToString();
				}
				if(row["meritPay"]!=null)
				{
					model.meritPay=row["meritPay"].ToString();
				}
				if(row["overtimePay"]!=null)
				{
					model.overtimePay=row["overtimePay"].ToString();
				}
				if(row["sickLeavePay"]!=null)
				{
					model.sickLeavePay=row["sickLeavePay"].ToString();
				}
				if(row["transFee"]!=null)
				{
					model.transFee=row["transFee"].ToString();
				}
				if(row["houseFee"]!=null)
				{
					model.houseFee=row["houseFee"].ToString();
				}
				if(row["foodFee"]!=null)
				{
					model.foodFee=row["foodFee"].ToString();
				}
				if(row["communFee"]!=null)
				{
					model.communFee=row["communFee"].ToString();
				}
				if(row["others"]!=null)
				{
					model.others=row["others"].ToString();
				}
				if(row["annuity"]!=null)
				{
					model.annuity=row["annuity"].ToString();
				}
				if(row["medicalInsur"]!=null)
				{
					model.medicalInsur=row["medicalInsur"].ToString();
				}
				if(row["unemployInsur"]!=null)
				{
					model.unemployInsur=row["unemployInsur"].ToString();
				}
				if(row["accumulFund"]!=null)
				{
					model.accumulFund=row["accumulFund"].ToString();
				}
				if(row["personalTax"]!=null)
				{
					model.personalTax=row["personalTax"].ToString();
				}
				if(row["otherFee"]!=null)
				{
					model.otherFee=row["otherFee"].ToString();
				}
				if(row["dues"]!=null)
				{
					model.dues=row["dues"].ToString();
				}
				if(row["onechildBonus"]!=null)
				{
					model.onechildBonus=row["onechildBonus"].ToString();
				}
				if(row["taxBack"]!=null)
				{
					model.taxBack=row["taxBack"].ToString();
				}
				if(row["date"]!=null)
				{
					model.date=row["date"].ToString();
				}
				if(row["creater"]!=null)
				{
					model.creater=row["creater"].ToString();
				}
				if(row["createTime"]!=null && row["createTime"].ToString()!="")
				{
					model.createTime=DateTime.Parse(row["createTime"].ToString());
				}
				if(row["yearOnceBonus"]!=null)
				{
					model.yearOnceBonus=row["yearOnceBonus"].ToString();
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
			strSql.Append("select id,staffNo,meritPay,overtimePay,sickLeavePay,transFee,houseFee,foodFee,communFee,others,annuity,medicalInsur,unemployInsur,accumulFund,personalTax,otherFee,dues,onechildBonus,taxBack,date,creater,createTime,yearOnceBonus ");
			strSql.Append(" FROM salaryChangeInfo ");
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
			strSql.Append(" id,staffNo,meritPay,overtimePay,sickLeavePay,transFee,houseFee,foodFee,communFee,others,annuity,medicalInsur,unemployInsur,accumulFund,personalTax,otherFee,dues,onechildBonus,taxBack,date,creater,createTime,yearOnceBonus ");
			strSql.Append(" FROM salaryChangeInfo ");
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
			strSql.Append("select count(1) FROM salaryChangeInfo ");
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
			strSql.Append(")AS Row, T.*  from salaryChangeInfo T ");
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
			parameters[0].Value = "salaryChangeInfo";
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

