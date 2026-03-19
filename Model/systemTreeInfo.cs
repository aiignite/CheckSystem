using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class systemTreeInfo
	{
		public systemTreeInfo()
		{}
		#region Model
		private int _id;
		private string _nodeno;
		private string _formname;
		private string _nodename;
		private string _nodeparentno;
		private int? _nodeimageid;
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
		/// @controlType:LTextBox,@labelString:节点编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string nodeNo
		{
			set{ _nodeno=value;}
			get{return _nodeno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:界面名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string formName
		{
			set{ _formname=value;}
			get{return _formname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:节点名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string nodeName
		{
			set{ _nodename=value;}
			get{return _nodename;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:父节点编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string nodeParentNo
		{
			set{ _nodeparentno=value;}
			get{return _nodeparentno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:ICO编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? nodeImageID
		{
			set{ _nodeimageid=value;}
			get{return _nodeimageid;}
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

