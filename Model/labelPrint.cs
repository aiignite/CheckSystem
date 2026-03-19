using System;

namespace Model
{
	/// <summary>
	/// labelPrint:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class labelPrint
	{
		public labelPrint()
		{}
		#region Model
		private int _id;
		private string _sourcestr;
		private int? _qty;
		private string _creater;
		private DateTime? _createtime;
		private int? _printcount;
		private string _sbin;
		private string _str1;
		private string _str2;
		private string _str3;
		private string _str4;
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
		public string SourceStr
		{
			set{ _sourcestr=value;}
			get{return _sourcestr;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Qty
		{
			set{ _qty=value;}
			get{return _qty;}
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
		public int? PrintCount
		{
			set{ _printcount=value;}
			get{return _printcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SBin
		{
			set{ _sbin=value;}
			get{return _sbin;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string str1
		{
			set{ _str1=value;}
			get{return _str1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string str2
		{
			set{ _str2=value;}
			get{return _str2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string str3
		{
			set{ _str3=value;}
			get{return _str3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string str4
		{
			set{ _str4=value;}
			get{return _str4;}
		}
		#endregion Model

	}
}

