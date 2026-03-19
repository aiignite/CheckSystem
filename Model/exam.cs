using System;

namespace Model
{
	/// <summary>
	/// exam:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class exam
	{
		public exam()
		{}
		#region Model
		private int _id;
		private string _examname;
		private int? _examtime;
		private string _exammember;
		private string _knowledgeno;
		private int? _singlenum;
		private int? _multinum;
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
		public string examName
		{
			set{ _examname=value;}
			get{return _examname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? examTime
		{
			set{ _examtime=value;}
			get{return _examtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string examMember
		{
			set{ _exammember=value;}
			get{return _exammember;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string knowledgeno
		{
			set{ _knowledgeno=value;}
			get{return _knowledgeno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? singlenum
		{
			set{ _singlenum=value;}
			get{return _singlenum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? multinum
		{
			set{ _multinum=value;}
			get{return _multinum;}
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

