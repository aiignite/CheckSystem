using System;

namespace Model
{
	/// <summary>
	/// processDocData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class processDocData
	{
		public processDocData()
		{}
		#region Model
		private int _id;
		private string _processno;
		private string _docno;
		private string _docname;
		private string _doclink;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 工序编号
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// 文档编号
		/// </summary>
		public string docNo
		{
			set{ _docno=value;}
			get{return _docno;}
		}
		/// <summary>
		/// 文档名称
		/// </summary>
		public string docName
		{
			set{ _docname=value;}
			get{return _docname;}
		}
		/// <summary>
		/// 文档链接
		/// </summary>
		public string docLink
		{
			set{ _doclink=value;}
			get{return _doclink;}
		}
		/// <summary>
		/// 创建者
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @DateTimePicker:记录时间
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

