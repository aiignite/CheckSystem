using System;

namespace Model
{
	/// <summary>
	/// salaryResult:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class salaryResult
	{
		public salaryResult()
		{}
		#region Model
		private int _id;
		private string _dept;
		private string _serial;
		private string _staffno;
		private string _name;
		private string _identityno;
		private string _basesalary;
		private string _postsalary;
		private string _meritpay;
		private string _overtimepay;
		private string _sickleavepay;
		private string _transfee;
		private string _housefee;
		private string _foodfee;
		private string _communfee;
		private string _others;
		private string _totalpay;
		private string _annuity;
		private string _medicalinsur;
		private string _unemployinsur;
		private string _accumulfund;
		private string _personaltax;
		private string _otherfee;
		private string _dues;
		private string _chargeback;
		private string _onechildbonus;
		private string _taxback;
		private string _actualamount;
		private string _date;
		private string _taxbefore;
		private string _allyearsalary;
		private string _allyearbonus;
		private string _deduction;
		private string _childedu;
		private string _pension;
		private string _houseloan;
		private string _houserent;
		private string _furtheredu;
		private string _deductionii;
		private string _payableincome;
		private string _taxrate;
		private string _quickdeduction;
		private string _individualtax;
		private string _curindividualtax;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dept
		{
			set{ _dept=value;}
			get{return _dept;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string serial
		{
			set{ _serial=value;}
			get{return _serial;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string staffNo
		{
			set{ _staffno=value;}
			get{return _staffno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string identityNo
		{
			set{ _identityno=value;}
			get{return _identityno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string baseSalary
		{
			set{ _basesalary=value;}
			get{return _basesalary;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string postSalary
		{
			set{ _postsalary=value;}
			get{return _postsalary;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string meritPay
		{
			set{ _meritpay=value;}
			get{return _meritpay;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string overtimePay
		{
			set{ _overtimepay=value;}
			get{return _overtimepay;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sickLeavePay
		{
			set{ _sickleavepay=value;}
			get{return _sickleavepay;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string transFee
		{
			set{ _transfee=value;}
			get{return _transfee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string houseFee
		{
			set{ _housefee=value;}
			get{return _housefee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string foodFee
		{
			set{ _foodfee=value;}
			get{return _foodfee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string communFee
		{
			set{ _communfee=value;}
			get{return _communfee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string others
		{
			set{ _others=value;}
			get{return _others;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string totalPay
		{
			set{ _totalpay=value;}
			get{return _totalpay;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string annuity
		{
			set{ _annuity=value;}
			get{return _annuity;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string medicalInsur
		{
			set{ _medicalinsur=value;}
			get{return _medicalinsur;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string unemployInsur
		{
			set{ _unemployinsur=value;}
			get{return _unemployinsur;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string accumulFund
		{
			set{ _accumulfund=value;}
			get{return _accumulfund;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string personalTax
		{
			set{ _personaltax=value;}
			get{return _personaltax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string otherFee
		{
			set{ _otherfee=value;}
			get{return _otherfee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dues
		{
			set{ _dues=value;}
			get{return _dues;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string chargeBack
		{
			set{ _chargeback=value;}
			get{return _chargeback;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string onechildBonus
		{
			set{ _onechildbonus=value;}
			get{return _onechildbonus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string taxBack
		{
			set{ _taxback=value;}
			get{return _taxback;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string actualAmount
		{
			set{ _actualamount=value;}
			get{return _actualamount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string date
		{
			set{ _date=value;}
			get{return _date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string taxBefore
		{
			set{ _taxbefore=value;}
			get{return _taxbefore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string allYearSalary
		{
			set{ _allyearsalary=value;}
			get{return _allyearsalary;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string allYearBonus
		{
			set{ _allyearbonus=value;}
			get{return _allyearbonus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string deduction
		{
			set{ _deduction=value;}
			get{return _deduction;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string childEdu
		{
			set{ _childedu=value;}
			get{return _childedu;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string pension
		{
			set{ _pension=value;}
			get{return _pension;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string houseLoan
		{
			set{ _houseloan=value;}
			get{return _houseloan;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string houseRent
		{
			set{ _houserent=value;}
			get{return _houserent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string furtherEdu
		{
			set{ _furtheredu=value;}
			get{return _furtheredu;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string deductionII
		{
			set{ _deductionii=value;}
			get{return _deductionii;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string payableIncome
		{
			set{ _payableincome=value;}
			get{return _payableincome;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string taxRate
		{
			set{ _taxrate=value;}
			get{return _taxrate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string quickDeduction
		{
			set{ _quickdeduction=value;}
			get{return _quickdeduction;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string individualTax
		{
			set{ _individualtax=value;}
			get{return _individualtax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string curIndividualTax
		{
			set{ _curindividualtax=value;}
			get{return _curindividualtax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

