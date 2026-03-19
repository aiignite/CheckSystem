using System;

namespace Model
{
	/// <summary>
	/// commFlowEntityStep:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class commFlowEntityStep
	{
		public commFlowEntityStep()
		{}
		#region Model
		private int _id;
		private int? _noteid;
		private string _flowno;
		private int? _stepno;
		private string _stepman;
		private int? _dealstatus;
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
		/// @controlType:LTextBox,@labelString:处理序号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public int? stepNo
		{
			set{ _stepno=value;}
			get{return _stepno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:处理人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string stepMan
		{
			set{ _stepman=value;}
			get{return _stepman;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:处理状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public int? dealStatus
		{
			set{ _dealstatus=value;}
			get{return _dealstatus;}
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

