using System;

namespace Model
{
	/// <summary>
	/// manufactureAssLineInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class manufactureAssLineInfo
	{
		public manufactureAssLineInfo()
		{}
		#region Model
		private int _id;
		private string _asslineno;
		private string _asslinename;
		private string _shopname;
		private string _location;
		private int? _area;
		private string _manager;
		private string _ondate;
		private string _ofdate;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:01
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产线编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:02
		/// </summary>
		public string assLineNo
		{
			set{ _asslineno=value;}
			get{return _asslineno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产线名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:03
		/// </summary>
		public string assLineName
		{
			set{ _asslinename=value;}
			get{return _asslinename;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:车间名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:04
		/// </summary>
		public string shopName
		{
			set{ _shopname=value;}
			get{return _shopname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:位置,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:05
		/// </summary>
		public string location
		{
			set{ _location=value;}
			get{return _location;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:面积,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:06
		/// </summary>
		public int? area
		{
			set{ _area=value;}
			get{return _area;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:主管,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:07
		/// </summary>
		public string manager
		{
			set{ _manager=value;}
			get{return _manager;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:投产时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:08
		/// </summary>
		public string onDate
		{
			set{ _ondate=value;}
			get{return _ondate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:停产时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:09
		/// </summary>
		public string ofDate
		{
			set{ _ofdate=value;}
			get{return _ofdate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建者,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:10
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:11
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

