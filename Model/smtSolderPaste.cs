using System;

namespace Model
{
	/// <summary>
	/// smtSolderPaste:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtSolderPaste
	{
		public smtSolderPaste()
		{}
		#region Model
		private int _id;
		private string _no;
		private int? _usecount;
		private DateTime? _instoretime;
		private DateTime? _backtime;
		private DateTime? _mixtime;
		private DateTime? _usertime;
		private DateTime? _expiredtime;
		private string _status;
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
		public string no
		{
			set{ _no=value;}
			get{return _no;}
		}
		/// <summary>
		/// 回收次数
		/// </summary>
		public int? useCount
		{
			set{ _usecount=value;}
			get{return _usecount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? instoreTime
		{
			set{ _instoretime=value;}
			get{return _instoretime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? backTime
		{
			set{ _backtime=value;}
			get{return _backtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? mixTime
		{
			set{ _mixtime=value;}
			get{return _mixtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? userTime
		{
			set{ _usertime=value;}
			get{return _usertime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? expiredTime
		{
			set{ _expiredtime=value;}
			get{return _expiredtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
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

