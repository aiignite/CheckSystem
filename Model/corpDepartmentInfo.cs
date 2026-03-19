using System;

namespace Model
{
	/// <summary>
	/// corpDepartmentInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class corpDepartmentInfo
	{
		public corpDepartmentInfo()
		{}
		#region Model
		private int _id;
		private string _deptno;
		private string _deptname;
		private string _note;
		private string _managerno;
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
		public string deptNo
		{
			set{ _deptno=value;}
			get{return _deptno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string deptName
		{
			set{ _deptname=value;}
			get{return _deptname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string managerNo
		{
			set{ _managerno=value;}
			get{return _managerno;}
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

