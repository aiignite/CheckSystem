using System;

namespace Model
{
	/// <summary>
	/// LogisticsAndon:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class LogisticsAndon
	{
		public LogisticsAndon()
		{}
		#region Model
		private int _id;
		private string _creater;
		private DateTime? _createtime;
		private string _smsstatus;
		private string _product;
		private string _dealer;
		private DateTime? _dealtime;
		private string _messagecontent;
		private string _problemtype;
		private int? _sendlevel;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
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
		/// <summary>
		/// 
		/// </summary>
		public string smsStatus
		{
			set{ _smsstatus=value;}
			get{return _smsstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Product
		{
			set{ _product=value;}
			get{return _product;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dealer
		{
			set{ _dealer=value;}
			get{return _dealer;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? dealTime
		{
			set{ _dealtime=value;}
			get{return _dealtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MessageContent
		{
			set{ _messagecontent=value;}
			get{return _messagecontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProblemType
		{
			set{ _problemtype=value;}
			get{return _problemtype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SendLevel
		{
			set{ _sendlevel=value;}
			get{return _sendlevel;}
		}
		#endregion Model

	}
}

