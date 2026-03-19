using System;

namespace Model
{
	/// <summary>
	/// commDocumentsInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class commDocumentsInfo
	{
		public commDocumentsInfo()
		{}
		#region Model
		private int _id;
		private string _docno;
		private string _docname;
		private string _docextensionname;
		private string _doclink;
		private string _docpath;
		private string _doctype;
		private string _doctypeno;
		private string _uploader;
		private string _uploaddate;
		private string _isvalid;
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
		/// 文档后缀名
		/// </summary>
		public string docExtensionName
		{
			set{ _docextensionname=value;}
			get{return _docextensionname;}
		}
		/// <summary>
		/// 链接
		/// </summary>
		public string docLink
		{
			set{ _doclink=value;}
			get{return _doclink;}
		}
		/// <summary>
		/// 文档路径
		/// </summary>
		public string docPath
		{
			set{ _docpath=value;}
			get{return _docpath;}
		}
		/// <summary>
		/// 文档类型
		/// </summary>
		public string docType
		{
			set{ _doctype=value;}
			get{return _doctype;}
		}
		/// <summary>
		/// 文档类型编号
		/// </summary>
		public string docTypeNo
		{
			set{ _doctypeno=value;}
			get{return _doctypeno;}
		}
		/// <summary>
		/// 上传人员
		/// </summary>
		public string uploader
		{
			set{ _uploader=value;}
			get{return _uploader;}
		}
		/// <summary>
		/// 上传日期
		/// </summary>
		public string uploadDate
		{
			set{ _uploaddate=value;}
			get{return _uploaddate;}
		}
		/// <summary>
		/// @ComboBox:是否有效,typeValue,commTypeCode,typeName,是否有效
		/// </summary>
		public string isValid
		{
			set{ _isvalid=value;}
			get{return _isvalid;}
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

