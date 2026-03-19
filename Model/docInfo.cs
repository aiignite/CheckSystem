using System;

namespace Model
{
	/// <summary>
	/// docInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class docInfo
	{
		public docInfo()
		{}
		#region Model
		private int _id;
		private string _docno;
		private string _docname;
		private string _doctype;
		private string _docchildtype;
		private string _productno;
		private string _productname;
		private string _docversion;
		private string _docstatus;
		private byte[] _doccontent;
		private string _docnote;
		private string _docwriter;
		private string _writefinishdate;
		private string _docreviewer;
		private string _reviewerdate;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文档编号,@searchString:,@tipString:自动生成,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string docNo
		{
			set{ _docno=value;}
			get{return _docno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文档名称,@searchString:,@tipString:上传文档自动加载,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string docName
		{
			set{ _docname=value;}
			get{return _docname;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:文档类型,@searchString:select distinct docType from dbo.docTypeInfo where docTypeParent ='',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string docType
		{
			set{ _doctype=value;}
			get{return _doctype;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:文档子类型,@searchString:select distinct docType from dbo.docTypeInfo where docTypeParent ='#docType#',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string docChildType
		{
			set{ _docchildtype=value;}
			get{return _docchildtype;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品编号,@searchString:select distinct productNo from dbo.productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品名称,@searchString:select distinct productName from dbo.productInfo where productNo ='#productNo#',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文档版本,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string docVersion
		{
			set{ _docversion=value;}
			get{return _docversion;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文档状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string docStatus
		{
			set{ _docstatus=value;}
			get{return _docstatus;}
		}
		/// <summary>
		/// @controlType:LFileUpload,@labelString:文档内容,@searchString:,@tipString:,@inDataGrid:False,@inPanel:True,@index:
		/// </summary>
		public byte[] docContent
		{
			set{ _doccontent=value;}
			get{return _doccontent;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string docNote
		{
			set{ _docnote=value;}
			get{return _docnote;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文档编制人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string docWriter
		{
			set{ _docwriter=value;}
			get{return _docwriter;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:编制完成日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string writeFinishDate
		{
			set{ _writefinishdate=value;}
			get{return _writefinishdate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文档审核人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string docReviewer
		{
			set{ _docreviewer=value;}
			get{return _docreviewer;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:审核日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string reviewerDate
		{
			set{ _reviewerdate=value;}
			get{return _reviewerdate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

