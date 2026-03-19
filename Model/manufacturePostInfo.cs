using System;

namespace Model
{
	/// <summary>
	/// manufacturePostInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class manufacturePostInfo
	{
		public manufacturePostInfo()
		{}
		#region Model
		private int _id;
		private string _postno;
		private string _postname;
		private string _productno;
		private int? _postcycle;
		private string _note;
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
		/// @controlType:LTextBox,@labelString:岗位编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string postNo
		{
			set{ _postno=value;}
			get{return _postno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:岗位名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string postName
		{
			set{ _postname=value;}
			get{return _postname;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品编号,@searchString:select productNo from productInfo (nolock),@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:岗位节拍,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? postCycle
		{
			set{ _postcycle=value;}
			get{return _postcycle;}
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
		/// @controlType:LTextBox,@labelString:创建者,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
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

