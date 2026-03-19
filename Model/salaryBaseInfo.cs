using System;

namespace Model
{
	/// <summary>
	/// salaryBaseInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class salaryBaseInfo
	{
		public salaryBaseInfo()
		{}
		#region Model
		private int _id;
		private string _dept;
		private string _deptno;
		private string _staffno;
		private string _name;
		private string _identityno;
		private string _basesalary;
		private string _postsalary;
		private string _deduction;
		private string _childedu;
		private string _pension;
		private string _houseloan;
		private string _houserent;
		private string _furtheredu;
		private string _initperiod;
		private string _poststate;
		private string _reviser;
		private DateTime? _revisedate;
		private string _creater;
		private DateTime? _createtime;
		private string _belongdept;
		private string _leavedate;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 部门
		/// </summary>
		public string dept
		{
			set{ _dept=value;}
			get{return _dept;}
		}
		/// <summary>
		/// 部门序号
		/// </summary>
		public string deptNo
		{
			set{ _deptno=value;}
			get{return _deptno;}
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
		/// 姓名
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 身份证号码
		/// </summary>
		public string identityNo
		{
			set{ _identityno=value;}
			get{return _identityno;}
		}
		/// <summary>
		/// 基本工资
		/// </summary>
		public string baseSalary
		{
			set{ _basesalary=value;}
			get{return _basesalary;}
		}
		/// <summary>
		/// 岗位工资
		/// </summary>
		public string postSalary
		{
			set{ _postsalary=value;}
			get{return _postsalary;}
		}
		/// <summary>
		/// 减费用额
		/// </summary>
		public string deduction
		{
			set{ _deduction=value;}
			get{return _deduction;}
		}
		/// <summary>
		/// 子女教育
		/// </summary>
		public string childEdu
		{
			set{ _childedu=value;}
			get{return _childedu;}
		}
		/// <summary>
		/// 赡养老人
		/// </summary>
		public string pension
		{
			set{ _pension=value;}
			get{return _pension;}
		}
		/// <summary>
		/// 住房贷款
		/// </summary>
		public string houseLoan
		{
			set{ _houseloan=value;}
			get{return _houseloan;}
		}
		/// <summary>
		/// 住房租金
		/// </summary>
		public string houseRent
		{
			set{ _houserent=value;}
			get{return _houserent;}
		}
		/// <summary>
		/// 继续教育
		/// </summary>
		public string furtherEdu
		{
			set{ _furtheredu=value;}
			get{return _furtheredu;}
		}
		/// <summary>
		/// 初始周期
		/// </summary>
		public string initPeriod
		{
			set{ _initperiod=value;}
			get{return _initperiod;}
		}
		/// <summary>
		/// 岗位状态
		/// </summary>
		public string postState
		{
			set{ _poststate=value;}
			get{return _poststate;}
		}
		/// <summary>
		/// 变更人
		/// </summary>
		public string reviser
		{
			set{ _reviser=value;}
			get{return _reviser;}
		}
		/// <summary>
		/// 变更时间
		/// </summary>
		public DateTime? reviseDate
		{
			set{ _revisedate=value;}
			get{return _revisedate;}
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
		/// 所属部门
		/// </summary>
		public string belongDept
		{
			set{ _belongdept=value;}
			get{return _belongdept;}
		}
		/// <summary>
		/// 离职月份
		/// </summary>
		public string leaveDate
		{
			set{ _leavedate=value;}
			get{return _leavedate;}
		}
		#endregion Model

	}
}

