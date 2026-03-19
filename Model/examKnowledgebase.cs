using System;

namespace Model
{
	/// <summary>
	/// examKnowledgebase:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class examKnowledgebase
	{
		public examKnowledgebase()
		{}
		#region Model
		private int _id;
		private string _knowledgeno;
		private string _knowledgename;
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
		public string knowledgeno
		{
			set{ _knowledgeno=value;}
			get{return _knowledgeno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string knowledgename
		{
			set{ _knowledgename=value;}
			get{return _knowledgename;}
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
		public DateTime? createtime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

