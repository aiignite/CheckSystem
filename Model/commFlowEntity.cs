using System;

namespace Model
{
	/// <summary>
	/// commFlowEntity:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class commFlowEntity
	{
		public commFlowEntity()
		{}
		#region Model
		private int _id;
		private int? _noteid;
		private string _flowno;
		private string _flowname;
		private string _currentuserno;
		private int? _currentstatus;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:单据ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public int? noteId
		{
			set{ _noteid=value;}
			get{return _noteid;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:流程编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string flowNo
		{
			set{ _flowno=value;}
			get{return _flowno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:流程名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string flowName
		{
			set{ _flowname=value;}
			get{return _flowname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:当前处理人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string currentUserNo
		{
			set{ _currentuserno=value;}
			get{return _currentuserno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:当前状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public int? currentStatus
		{
			set{ _currentstatus=value;}
			get{return _currentstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

