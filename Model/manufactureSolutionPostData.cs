using System;

namespace Model
{
	/// <summary>
	/// manufactureSolutionPostData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class manufactureSolutionPostData
	{
		public manufactureSolutionPostData()
		{}
		#region Model
		private int _id;
		private string _solutionno;
		private string _postno;
		private int? _postindex;
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
		/// @controlType:LTextBox,@labelString:方案编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string solutionNo
		{
			set{ _solutionno=value;}
			get{return _solutionno;}
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
		/// @controlType:LTextBox,@labelString:岗位序号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? postIndex
		{
			set{ _postindex=value;}
			get{return _postindex;}
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

