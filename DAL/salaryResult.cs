using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

//Please add references
namespace DAL
{
	/// <summary>
	/// 数据访问类:salaryResult
	/// </summary>
	public partial class salaryResult
	{
		public salaryResult()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "salaryResult"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from salaryResult");
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
		public int Add(Model.salaryResult model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into salaryResult(");
			strSql.Append("dept,serial,staffNo,name,identityNo,baseSalary,postSalary,meritPay,overtimePay,sickLeavePay,transFee,houseFee,foodFee,communFee,others,totalPay,annuity,medicalInsur,unemployInsur,accumulFund,personalTax,otherFee,dues,chargeBack,onechildBonus,taxBack,actualAmount,date,taxBefore,allYearSalary,allYearBonus,deduction,childEdu,pension,houseLoan,houseRent,furtherEdu,deductionII,payableIncome,taxRate,quickDeduction,individualTax,curIndividualTax,creater,createTime)");
			strSql.Append(" values (");
			strSql.Append("@dept,@serial,@staffNo,@name,@identityNo,@baseSalary,@postSalary,@meritPay,@overtimePay,@sickLeavePay,@transFee,@houseFee,@foodFee,@communFee,@others,@totalPay,@annuity,@medicalInsur,@unemployInsur,@accumulFund,@personalTax,@otherFee,@dues,@chargeBack,@onechildBonus,@taxBack,@actualAmount,@date,@taxBefore,@allYearSalary,@allYearBonus,@deduction,@childEdu,@pension,@houseLoan,@houseRent,@furtherEdu,@deductionII,@payableIncome,@taxRate,@quickDeduction,@individualTax,@curIndividualTax,@creater,@createTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@dept", SqlDbType.NVarChar,50),
					new SqlParameter("@serial", SqlDbType.NVarChar,50),
					new SqlParameter("@staffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@name", SqlDbType.NVarChar,50),
					new SqlParameter("@identityNo", SqlDbType.NVarChar,50),
					new SqlParameter("@baseSalary", SqlDbType.NVarChar,50),
					new SqlParameter("@postSalary", SqlDbType.NVarChar,50),
					new SqlParameter("@meritPay", SqlDbType.NVarChar,50),
					new SqlParameter("@overtimePay", SqlDbType.NVarChar,50),
					new SqlParameter("@sickLeavePay", SqlDbType.NVarChar,50),
					new SqlParameter("@transFee", SqlDbType.NVarChar,50),
					new SqlParameter("@houseFee", SqlDbType.NVarChar,50),
					new SqlParameter("@foodFee", SqlDbType.NVarChar,50),
					new SqlParameter("@communFee", SqlDbType.NVarChar,50),
					new SqlParameter("@others", SqlDbType.NVarChar,50),
					new SqlParameter("@totalPay", SqlDbType.NVarChar,50),
					new SqlParameter("@annuity", SqlDbType.NVarChar,50),
					new SqlParameter("@medicalInsur", SqlDbType.NVarChar,50),
					new SqlParameter("@unemployInsur", SqlDbType.NVarChar,50),
					new SqlParameter("@accumulFund", SqlDbType.NVarChar,50),
					new SqlParameter("@personalTax", SqlDbType.NVarChar,50),
					new SqlParameter("@otherFee", SqlDbType.NVarChar,50),
					new SqlParameter("@dues", SqlDbType.NVarChar,50),
					new SqlParameter("@chargeBack", SqlDbType.NVarChar,50),
					new SqlParameter("@onechildBonus", SqlDbType.NVarChar,50),
					new SqlParameter("@taxBack", SqlDbType.NVarChar,50),
					new SqlParameter("@actualAmount", SqlDbType.NVarChar,50),
					new SqlParameter("@date", SqlDbType.NVarChar,50),
					new SqlParameter("@taxBefore", SqlDbType.NVarChar,50),
					new SqlParameter("@allYearSalary", SqlDbType.NVarChar,50),
					new SqlParameter("@allYearBonus", SqlDbType.NVarChar,50),
					new SqlParameter("@deduction", SqlDbType.NVarChar,50),
					new SqlParameter("@childEdu", SqlDbType.NVarChar,50),
					new SqlParameter("@pension", SqlDbType.NVarChar,50),
					new SqlParameter("@houseLoan", SqlDbType.NVarChar,50),
					new SqlParameter("@houseRent", SqlDbType.NVarChar,50),
					new SqlParameter("@furtherEdu", SqlDbType.NVarChar,50),
					new SqlParameter("@deductionII", SqlDbType.NVarChar,50),
					new SqlParameter("@payableIncome", SqlDbType.NVarChar,50),
					new SqlParameter("@taxRate", SqlDbType.NVarChar,50),
					new SqlParameter("@quickDeduction", SqlDbType.NVarChar,50),
					new SqlParameter("@individualTax", SqlDbType.NVarChar,50),
					new SqlParameter("@curIndividualTax", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime)};
			parameters[0].Value = model.dept;
			parameters[1].Value = model.serial;
			parameters[2].Value = model.staffNo;
			parameters[3].Value = model.name;
			parameters[4].Value = model.identityNo;
			parameters[5].Value = model.baseSalary;
			parameters[6].Value = model.postSalary;
			parameters[7].Value = model.meritPay;
			parameters[8].Value = model.overtimePay;
			parameters[9].Value = model.sickLeavePay;
			parameters[10].Value = model.transFee;
			parameters[11].Value = model.houseFee;
			parameters[12].Value = model.foodFee;
			parameters[13].Value = model.communFee;
			parameters[14].Value = model.others;
			parameters[15].Value = model.totalPay;
			parameters[16].Value = model.annuity;
			parameters[17].Value = model.medicalInsur;
			parameters[18].Value = model.unemployInsur;
			parameters[19].Value = model.accumulFund;
			parameters[20].Value = model.personalTax;
			parameters[21].Value = model.otherFee;
			parameters[22].Value = model.dues;
			parameters[23].Value = model.chargeBack;
			parameters[24].Value = model.onechildBonus;
			parameters[25].Value = model.taxBack;
			parameters[26].Value = model.actualAmount;
			parameters[27].Value = model.date;
			parameters[28].Value = model.taxBefore;
			parameters[29].Value = model.allYearSalary;
			parameters[30].Value = model.allYearBonus;
			parameters[31].Value = model.deduction;
			parameters[32].Value = model.childEdu;
			parameters[33].Value = model.pension;
			parameters[34].Value = model.houseLoan;
			parameters[35].Value = model.houseRent;
			parameters[36].Value = model.furtherEdu;
			parameters[37].Value = model.deductionII;
			parameters[38].Value = model.payableIncome;
			parameters[39].Value = model.taxRate;
			parameters[40].Value = model.quickDeduction;
			parameters[41].Value = model.individualTax;
			parameters[42].Value = model.curIndividualTax;
			parameters[43].Value = model.creater;
			parameters[44].Value = model.createTime;

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
		public bool Update(Model.salaryResult model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update salaryResult set ");
			strSql.Append("dept=@dept,");
			strSql.Append("serial=@serial,");
			strSql.Append("staffNo=@staffNo,");
			strSql.Append("name=@name,");
			strSql.Append("identityNo=@identityNo,");
			strSql.Append("baseSalary=@baseSalary,");
			strSql.Append("postSalary=@postSalary,");
			strSql.Append("meritPay=@meritPay,");
			strSql.Append("overtimePay=@overtimePay,");
			strSql.Append("sickLeavePay=@sickLeavePay,");
			strSql.Append("transFee=@transFee,");
			strSql.Append("houseFee=@houseFee,");
			strSql.Append("foodFee=@foodFee,");
			strSql.Append("communFee=@communFee,");
			strSql.Append("others=@others,");
			strSql.Append("totalPay=@totalPay,");
			strSql.Append("annuity=@annuity,");
			strSql.Append("medicalInsur=@medicalInsur,");
			strSql.Append("unemployInsur=@unemployInsur,");
			strSql.Append("accumulFund=@accumulFund,");
			strSql.Append("personalTax=@personalTax,");
			strSql.Append("otherFee=@otherFee,");
			strSql.Append("dues=@dues,");
			strSql.Append("chargeBack=@chargeBack,");
			strSql.Append("onechildBonus=@onechildBonus,");
			strSql.Append("taxBack=@taxBack,");
			strSql.Append("actualAmount=@actualAmount,");
			strSql.Append("date=@date,");
			strSql.Append("taxBefore=@taxBefore,");
			strSql.Append("allYearSalary=@allYearSalary,");
			strSql.Append("allYearBonus=@allYearBonus,");
			strSql.Append("deduction=@deduction,");
			strSql.Append("childEdu=@childEdu,");
			strSql.Append("pension=@pension,");
			strSql.Append("houseLoan=@houseLoan,");
			strSql.Append("houseRent=@houseRent,");
			strSql.Append("furtherEdu=@furtherEdu,");
			strSql.Append("deductionII=@deductionII,");
			strSql.Append("payableIncome=@payableIncome,");
			strSql.Append("taxRate=@taxRate,");
			strSql.Append("quickDeduction=@quickDeduction,");
			strSql.Append("individualTax=@individualTax,");
			strSql.Append("curIndividualTax=@curIndividualTax,");
			strSql.Append("creater=@creater,");
			strSql.Append("createTime=@createTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@dept", SqlDbType.NVarChar,50),
					new SqlParameter("@serial", SqlDbType.NVarChar,50),
					new SqlParameter("@staffNo", SqlDbType.NVarChar,50),
					new SqlParameter("@name", SqlDbType.NVarChar,50),
					new SqlParameter("@identityNo", SqlDbType.NVarChar,50),
					new SqlParameter("@baseSalary", SqlDbType.NVarChar,50),
					new SqlParameter("@postSalary", SqlDbType.NVarChar,50),
					new SqlParameter("@meritPay", SqlDbType.NVarChar,50),
					new SqlParameter("@overtimePay", SqlDbType.NVarChar,50),
					new SqlParameter("@sickLeavePay", SqlDbType.NVarChar,50),
					new SqlParameter("@transFee", SqlDbType.NVarChar,50),
					new SqlParameter("@houseFee", SqlDbType.NVarChar,50),
					new SqlParameter("@foodFee", SqlDbType.NVarChar,50),
					new SqlParameter("@communFee", SqlDbType.NVarChar,50),
					new SqlParameter("@others", SqlDbType.NVarChar,50),
					new SqlParameter("@totalPay", SqlDbType.NVarChar,50),
					new SqlParameter("@annuity", SqlDbType.NVarChar,50),
					new SqlParameter("@medicalInsur", SqlDbType.NVarChar,50),
					new SqlParameter("@unemployInsur", SqlDbType.NVarChar,50),
					new SqlParameter("@accumulFund", SqlDbType.NVarChar,50),
					new SqlParameter("@personalTax", SqlDbType.NVarChar,50),
					new SqlParameter("@otherFee", SqlDbType.NVarChar,50),
					new SqlParameter("@dues", SqlDbType.NVarChar,50),
					new SqlParameter("@chargeBack", SqlDbType.NVarChar,50),
					new SqlParameter("@onechildBonus", SqlDbType.NVarChar,50),
					new SqlParameter("@taxBack", SqlDbType.NVarChar,50),
					new SqlParameter("@actualAmount", SqlDbType.NVarChar,50),
					new SqlParameter("@date", SqlDbType.NVarChar,50),
					new SqlParameter("@taxBefore", SqlDbType.NVarChar,50),
					new SqlParameter("@allYearSalary", SqlDbType.NVarChar,50),
					new SqlParameter("@allYearBonus", SqlDbType.NVarChar,50),
					new SqlParameter("@deduction", SqlDbType.NVarChar,50),
					new SqlParameter("@childEdu", SqlDbType.NVarChar,50),
					new SqlParameter("@pension", SqlDbType.NVarChar,50),
					new SqlParameter("@houseLoan", SqlDbType.NVarChar,50),
					new SqlParameter("@houseRent", SqlDbType.NVarChar,50),
					new SqlParameter("@furtherEdu", SqlDbType.NVarChar,50),
					new SqlParameter("@deductionII", SqlDbType.NVarChar,50),
					new SqlParameter("@payableIncome", SqlDbType.NVarChar,50),
					new SqlParameter("@taxRate", SqlDbType.NVarChar,50),
					new SqlParameter("@quickDeduction", SqlDbType.NVarChar,50),
					new SqlParameter("@individualTax", SqlDbType.NVarChar,50),
					new SqlParameter("@curIndividualTax", SqlDbType.NVarChar,50),
					new SqlParameter("@creater", SqlDbType.NVarChar,50),
					new SqlParameter("@createTime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.dept;
			parameters[1].Value = model.serial;
			parameters[2].Value = model.staffNo;
			parameters[3].Value = model.name;
			parameters[4].Value = model.identityNo;
			parameters[5].Value = model.baseSalary;
			parameters[6].Value = model.postSalary;
			parameters[7].Value = model.meritPay;
			parameters[8].Value = model.overtimePay;
			parameters[9].Value = model.sickLeavePay;
			parameters[10].Value = model.transFee;
			parameters[11].Value = model.houseFee;
			parameters[12].Value = model.foodFee;
			parameters[13].Value = model.communFee;
			parameters[14].Value = model.others;
			parameters[15].Value = model.totalPay;
			parameters[16].Value = model.annuity;
			parameters[17].Value = model.medicalInsur;
			parameters[18].Value = model.unemployInsur;
			parameters[19].Value = model.accumulFund;
			parameters[20].Value = model.personalTax;
			parameters[21].Value = model.otherFee;
			parameters[22].Value = model.dues;
			parameters[23].Value = model.chargeBack;
			parameters[24].Value = model.onechildBonus;
			parameters[25].Value = model.taxBack;
			parameters[26].Value = model.actualAmount;
			parameters[27].Value = model.date;
			parameters[28].Value = model.taxBefore;
			parameters[29].Value = model.allYearSalary;
			parameters[30].Value = model.allYearBonus;
			parameters[31].Value = model.deduction;
			parameters[32].Value = model.childEdu;
			parameters[33].Value = model.pension;
			parameters[34].Value = model.houseLoan;
			parameters[35].Value = model.houseRent;
			parameters[36].Value = model.furtherEdu;
			parameters[37].Value = model.deductionII;
			parameters[38].Value = model.payableIncome;
			parameters[39].Value = model.taxRate;
			parameters[40].Value = model.quickDeduction;
			parameters[41].Value = model.individualTax;
			parameters[42].Value = model.curIndividualTax;
			parameters[43].Value = model.creater;
			parameters[44].Value = model.createTime;
			parameters[45].Value = model.id;

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
			strSql.Append("delete from salaryResult ");
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
			strSql.Append("delete from salaryResult ");
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
		public Model.salaryResult GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,dept,serial,staffNo,name,identityNo,baseSalary,postSalary,meritPay,overtimePay,sickLeavePay,transFee,houseFee,foodFee,communFee,others,totalPay,annuity,medicalInsur,unemployInsur,accumulFund,personalTax,otherFee,dues,chargeBack,onechildBonus,taxBack,actualAmount,date,taxBefore,allYearSalary,allYearBonus,deduction,childEdu,pension,houseLoan,houseRent,furtherEdu,deductionII,payableIncome,taxRate,quickDeduction,individualTax,curIndividualTax,creater,createTime from salaryResult ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			Model.salaryResult model=new Model.salaryResult();
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
		public Model.salaryResult DataRowToModel(DataRow row)
		{
			Model.salaryResult model=new Model.salaryResult();
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
				if(row["serial"]!=null)
				{
					model.serial=row["serial"].ToString();
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
				if(row["totalPay"]!=null)
				{
					model.totalPay=row["totalPay"].ToString();
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
				if(row["chargeBack"]!=null)
				{
					model.chargeBack=row["chargeBack"].ToString();
				}
				if(row["onechildBonus"]!=null)
				{
					model.onechildBonus=row["onechildBonus"].ToString();
				}
				if(row["taxBack"]!=null)
				{
					model.taxBack=row["taxBack"].ToString();
				}
				if(row["actualAmount"]!=null)
				{
					model.actualAmount=row["actualAmount"].ToString();
				}
				if(row["date"]!=null)
				{
					model.date=row["date"].ToString();
				}
				if(row["taxBefore"]!=null)
				{
					model.taxBefore=row["taxBefore"].ToString();
				}
				if(row["allYearSalary"]!=null)
				{
					model.allYearSalary=row["allYearSalary"].ToString();
				}
				if(row["allYearBonus"]!=null)
				{
					model.allYearBonus=row["allYearBonus"].ToString();
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
				if(row["deductionII"]!=null)
				{
					model.deductionII=row["deductionII"].ToString();
				}
				if(row["payableIncome"]!=null)
				{
					model.payableIncome=row["payableIncome"].ToString();
				}
				if(row["taxRate"]!=null)
				{
					model.taxRate=row["taxRate"].ToString();
				}
				if(row["quickDeduction"]!=null)
				{
					model.quickDeduction=row["quickDeduction"].ToString();
				}
				if(row["individualTax"]!=null)
				{
					model.individualTax=row["individualTax"].ToString();
				}
				if(row["curIndividualTax"]!=null)
				{
					model.curIndividualTax=row["curIndividualTax"].ToString();
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
			strSql.Append("select id,dept,serial,staffNo,name,identityNo,baseSalary,postSalary,meritPay,overtimePay,sickLeavePay,transFee,houseFee,foodFee,communFee,others,totalPay,annuity,medicalInsur,unemployInsur,accumulFund,personalTax,otherFee,dues,chargeBack,onechildBonus,taxBack,actualAmount,date,taxBefore,allYearSalary,allYearBonus,deduction,childEdu,pension,houseLoan,houseRent,furtherEdu,deductionII,payableIncome,taxRate,quickDeduction,individualTax,curIndividualTax,creater,createTime ");
			strSql.Append(" FROM salaryResult ");
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
			strSql.Append(" id,dept,serial,staffNo,name,identityNo,baseSalary,postSalary,meritPay,overtimePay,sickLeavePay,transFee,houseFee,foodFee,communFee,others,totalPay,annuity,medicalInsur,unemployInsur,accumulFund,personalTax,otherFee,dues,chargeBack,onechildBonus,taxBack,actualAmount,date,taxBefore,allYearSalary,allYearBonus,deduction,childEdu,pension,houseLoan,houseRent,furtherEdu,deductionII,payableIncome,taxRate,quickDeduction,individualTax,curIndividualTax,creater,createTime ");
			strSql.Append(" FROM salaryResult ");
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
			strSql.Append("select count(1) FROM salaryResult ");
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
			strSql.Append(")AS Row, T.*  from salaryResult T ");
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
			parameters[0].Value = "salaryResult";
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

