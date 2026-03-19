using System;

namespace Model
{
	/// <summary>
	/// agvTask:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class agvTask
	{
		public agvTask()
		{}
		#region Model
		private int _id;
		private string _taskno;
		private string _tasksourceno;
		private string _taskstartno;
		private string _taskendno;
		private string _taskname;
		private string _taskpriority;
		private int? _taskremaintime;
		private string _agvno;
		private string _status;
		private DateTime? _createtime;
		/// <summary>
		/// ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 任务编号
		/// </summary>
		public string taskNo
		{
			set{ _taskno=value;}
			get{return _taskno;}
		}
		/// <summary>
		/// 发布端编号
		/// </summary>
		public string taskSourceNo
		{
			set{ _tasksourceno=value;}
			get{return _tasksourceno;}
		}
		/// <summary>
		/// 地标开始编号
		/// </summary>
		public string taskStartNo
		{
			set{ _taskstartno=value;}
			get{return _taskstartno;}
		}
		/// <summary>
		/// 地标结束编号
		/// </summary>
		public string taskEndNo
		{
			set{ _taskendno=value;}
			get{return _taskendno;}
		}
		/// <summary>
		/// 任务名称
		/// </summary>
		public string taskName
		{
			set{ _taskname=value;}
			get{return _taskname;}
		}
		/// <summary>
		/// 任务优先级
		/// </summary>
		public string taskPriority
		{
			set{ _taskpriority=value;}
			get{return _taskpriority;}
		}
		/// <summary>
		/// 大约剩余时间
		/// </summary>
		public int? taskRemainTime
		{
			set{ _taskremaintime=value;}
			get{return _taskremaintime;}
		}
		/// <summary>
		/// 分配AGV编号
		/// </summary>
		public string agvNo
		{
			set{ _agvno=value;}
			get{return _agvno;}
		}
		/// <summary>
		/// 状态
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @DateTimePicker:时间
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

