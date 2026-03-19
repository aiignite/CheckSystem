using System;

namespace Model
{
	/// <summary>
	/// commFlowLog:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class commFlowLog
	{
		public commFlowLog()
		{}
		#region Model
		private int _id;
		private string _flowno;
		private int? _noteid;
		private string _notecontent;
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
		/// @controlType:LTextBox,@labelString:流程ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string flowNo
		{
			set{ _flowno=value;}
			get{return _flowno;}
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
		/// @controlType:LTextBox,@labelString:处理内容,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string noteContent
		{
			set{ _notecontent=value;}
			get{return _notecontent;}
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

