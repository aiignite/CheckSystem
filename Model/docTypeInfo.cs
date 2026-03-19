using System;

namespace Model
{
	/// <summary>
	/// docTypeInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class docTypeInfo
	{
		public docTypeInfo()
		{}
		#region Model
		private int _id;
		private string _doctype;
		private string _doctypeparent;
		private string _note;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文档类型,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string docType
		{
			set{ _doctype=value;}
			get{return _doctype;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:文档父类型,@searchString:select distinct docType from docTypeInfo where docTypeParent ='',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string docTypeParent
		{
			set{ _doctypeparent=value;}
			get{return _doctypeparent;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
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

