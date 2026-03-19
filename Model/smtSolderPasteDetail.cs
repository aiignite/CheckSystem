using System;

namespace Model
{
	/// <summary>
	/// smtSolderPasteDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtSolderPasteDetail
	{
		public smtSolderPasteDetail()
		{}
		#region Model
		private int _id;
		private string _no;
		private string _operatetype;
		private string _operater;
		private DateTime? _operatertime;
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
		/// 
		/// </summary>
		public string operateType
		{
			set{ _operatetype=value;}
			get{return _operatetype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string operater
		{
			set{ _operater=value;}
			get{return _operater;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? operaterTime
		{
			set{ _operatertime=value;}
			get{return _operatertime;}
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

