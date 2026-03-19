using System;

namespace Model
{
	/// <summary>
	/// ExamPaper:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ExamPaper
	{
		public ExamPaper()
		{}
		#region Model
		private string _id;
		private string _papername;
		private int? _problemcount;
		private decimal? _score;
		private string _createby;
		private DateTime? _createdt;
		private int? _examid;
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
		public string PaperName
		{
			set{ _papername=value;}
			get{return _papername;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ProblemCount
		{
			set{ _problemcount=value;}
			get{return _problemcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Score
		{
			set{ _score=value;}
			get{return _score;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Createby
		{
			set{ _createby=value;}
			get{return _createby;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Createdt
		{
			set{ _createdt=value;}
			get{return _createdt;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? examid
		{
			set{ _examid=value;}
			get{return _examid;}
		}
		#endregion Model

	}
}

