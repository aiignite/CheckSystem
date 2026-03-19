using System;

namespace Model
{
	/// <summary>
	/// ExamPaperDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ExamPaperDetail
	{
		public ExamPaperDetail()
		{}
		#region Model
		private string _id;
		private string _mid;
		private string _knowledgeno;
		private string _testman;
		private DateTime? _createdate;
		private int? _problemid;
		private string _answer;
		private int? _sortno;
		/// <summary>
		/// 
		/// </summary>
		public string id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Mid
		{
			set{ _mid=value;}
			get{return _mid;}
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
		public string testman
		{
			set{ _testman=value;}
			get{return _testman;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? createdate
		{
			set{ _createdate=value;}
			get{return _createdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? problemid
		{
			set{ _problemid=value;}
			get{return _problemid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string answer
		{
			set{ _answer=value;}
			get{return _answer;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SortNo
		{
			set{ _sortno=value;}
			get{return _sortno;}
		}
		#endregion Model

	}
}

