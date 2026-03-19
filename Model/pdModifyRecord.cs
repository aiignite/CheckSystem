using System;

namespace Model
{
	/// <summary>
	/// pdModifyRecord:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class pdModifyRecord
	{
		public pdModifyRecord()
		{}
		#region Model
		private int _id;
		private string _opertype;
		private string _buztype;
		private int? _buzid;
		private string _buzno;
		private string _title;
		private string _field;
		private string _sourcecontent;
		private string _newcontent;
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
		/// @controlType:LTextBox,@labelString:操作类型,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string operType
		{
			set{ _opertype=value;}
			get{return _opertype;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:业务类型,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string buzType
		{
			set{ _buztype=value;}
			get{return _buztype;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:业务记录ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? buzID
		{
			set{ _buzid=value;}
			get{return _buzid;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:业务编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string buzNo
		{
			set{ _buzno=value;}
			get{return _buzno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:主题,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数据字段,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string field
		{
			set{ _field=value;}
			get{return _field;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:原内容,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string sourceContent
		{
			set{ _sourcecontent=value;}
			get{return _sourcecontent;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:新内容,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string newContent
		{
			set{ _newcontent=value;}
			get{return _newcontent;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建者,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

