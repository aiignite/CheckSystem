using System;

namespace Model
{
	/// <summary>
	/// examQuestions:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class examQuestions
	{
		public examQuestions()
		{}
		#region Model
		private int _id;
		private string _knowledgeno;
		private string _problem;
		private string _type;
		private string _answer1;
		private string _answer2;
		private string _answer3;
		private string _answer4;
		private string _answer5;
		private string _answer6;
		private string _trueanswer;
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
		public string problem
		{
			set{ _problem=value;}
			get{return _problem;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Answer1
		{
			set{ _answer1=value;}
			get{return _answer1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Answer2
		{
			set{ _answer2=value;}
			get{return _answer2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Answer3
		{
			set{ _answer3=value;}
			get{return _answer3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Answer4
		{
			set{ _answer4=value;}
			get{return _answer4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Answer5
		{
			set{ _answer5=value;}
			get{return _answer5;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Answer6
		{
			set{ _answer6=value;}
			get{return _answer6;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TrueAnswer
		{
			set{ _trueanswer=value;}
			get{return _trueanswer;}
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

