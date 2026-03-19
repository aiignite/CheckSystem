using System;

namespace Model
{
	/// <summary>
	/// agvCarTask:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class agvCarTask
	{
		public agvCarTask()
		{}
		#region Model
		private int _id;
		private int? _tasklevel;
		private string _taskclassifier;
		private int? _isgiven;
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
		/// @controlType:LTextBox,@labelString:tasklevel,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public int? tasklevel
		{
			set{ _tasklevel=value;}
			get{return _tasklevel;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:taskClassifier,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string taskClassifier
		{
			set{ _taskclassifier=value;}
			get{return _taskclassifier;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:isGiven,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public int? isGiven
		{
			set{ _isgiven=value;}
			get{return _isgiven;}
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

