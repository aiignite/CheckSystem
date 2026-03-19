using System;

namespace Model
{
	/// <summary>
	/// staffPostData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class staffPostData
	{
		public staffPostData()
		{}
		#region Model
		private int _id;
		private string _staffno;
		private string _postno;
		private DateTime? _ontime;
		private DateTime? _offtime;
		private int? _validtime;
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
		/// @controlType:LTextBox,@labelString:人员编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string staffNo
		{
			set{ _staffno=value;}
			get{return _staffno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:岗位编号,@searchString:select postNo from manufacturePostInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string postNo
		{
			set{ _postno=value;}
			get{return _postno;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:上岗时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? onTime
		{
			set{ _ontime=value;}
			get{return _ontime;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:离岗时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? offTime
		{
			set{ _offtime=value;}
			get{return _offtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:在岗时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? validTime
		{
			set{ _validtime=value;}
			get{return _validtime;}
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

