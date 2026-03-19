using System;

namespace Model
{
	/// <summary>
	/// salaryChangeInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class salaryChangeInfo
	{
		public salaryChangeInfo()
		{}
		#region Model
		private int _id;
		private string _staffno;
		private string _meritpay;
		private string _overtimepay;
		private string _sickleavepay;
		private string _transfee;
		private string _housefee;
		private string _foodfee;
		private string _communfee;
		private string _others;
		private string _annuity;
		private string _medicalinsur;
		private string _unemployinsur;
		private string _accumulfund;
		private string _personaltax;
		private string _otherfee;
		private string _dues;
		private string _onechildbonus;
		private string _taxback;
		private string _date;
		private string _creater;
		private DateTime? _createtime;
		private string _yearoncebonus;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 工号
		/// </summary>
		public string staffNo
		{
			set{ _staffno=value;}
			get{return _staffno;}
		}
		/// <summary>
		/// 绩效奖金
		/// </summary>
		public string meritPay
		{
			set{ _meritpay=value;}
			get{return _meritpay;}
		}
		/// <summary>
		/// 加班费
		/// </summary>
		public string overtimePay
		{
			set{ _overtimepay=value;}
			get{return _overtimepay;}
		}
		/// <summary>
		/// 病事假
		/// </summary>
		public string sickLeavePay
		{
			set{ _sickleavepay=value;}
			get{return _sickleavepay;}
		}
		/// <summary>
		/// 交通补贴
		/// </summary>
		public string transFee
		{
			set{ _transfee=value;}
			get{return _transfee;}
		}
		/// <summary>
		/// 住房补贴
		/// </summary>
		public string houseFee
		{
			set{ _housefee=value;}
			get{return _housefee;}
		}
		/// <summary>
		/// 伙食补贴
		/// </summary>
		public string foodFee
		{
			set{ _foodfee=value;}
			get{return _foodfee;}
		}
		/// <summary>
		/// 通讯补贴
		/// </summary>
		public string communFee
		{
			set{ _communfee=value;}
			get{return _communfee;}
		}
		/// <summary>
		/// 其他
		/// </summary>
		public string others
		{
			set{ _others=value;}
			get{return _others;}
		}
		/// <summary>
		/// 养老金
		/// </summary>
		public string annuity
		{
			set{ _annuity=value;}
			get{return _annuity;}
		}
		/// <summary>
		/// 医保
		/// </summary>
		public string medicalInsur
		{
			set{ _medicalinsur=value;}
			get{return _medicalinsur;}
		}
		/// <summary>
		/// 失业保
		/// </summary>
		public string unemployInsur
		{
			set{ _unemployinsur=value;}
			get{return _unemployinsur;}
		}
		/// <summary>
		/// 公积金
		/// </summary>
		public string accumulFund
		{
			set{ _accumulfund=value;}
			get{return _accumulfund;}
		}
		/// <summary>
		/// 个调税
		/// </summary>
		public string personalTax
		{
			set{ _personaltax=value;}
			get{return _personaltax;}
		}
		/// <summary>
		/// 其他扣
		/// </summary>
		public string otherFee
		{
			set{ _otherfee=value;}
			get{return _otherfee;}
		}
		/// <summary>
		/// 会费
		/// </summary>
		public string dues
		{
			set{ _dues=value;}
			get{return _dues;}
		}
		/// <summary>
		/// 独生子女费
		/// </summary>
		public string onechildBonus
		{
			set{ _onechildbonus=value;}
			get{return _onechildbonus;}
		}
		/// <summary>
		/// 退税金
		/// </summary>
		public string taxBack
		{
			set{ _taxback=value;}
			get{return _taxback;}
		}
		/// <summary>
		/// 日期
		/// </summary>
		public string date
		{
			set{ _date=value;}
			get{return _date;}
		}
		/// <summary>
		/// 创建人
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 全年一次奖金
		/// </summary>
		public string yearOnceBonus
		{
			set{ _yearoncebonus=value;}
			get{return _yearoncebonus;}
		}
		#endregion Model

	}
}

