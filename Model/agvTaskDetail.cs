using System;

namespace Model
{
	/// <summary>
	/// agvTaskDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class agvTaskDetail
	{
		public agvTaskDetail()
		{}
		#region Model
		private int _id;
		private string _taskno;
		private string _rfidno;
		private string _nextrfidno;
		private string _rfidtype;
		private string _tasktype;
		private string _agvderection;
		private int? _agvspeed;
		private string _taskdata;
		private string _rfidstatus;
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
		/// 地标编号
		/// </summary>
		public string rfidNo
		{
			set{ _rfidno=value;}
			get{return _rfidno;}
		}
		/// <summary>
		/// 下一地标编号
		/// </summary>
		public string nextRfidNo
		{
			set{ _nextrfidno=value;}
			get{return _nextrfidno;}
		}
		/// <summary>
		/// @TypeCode:任务类型
		/// </summary>
		public string rfidType
		{
			set{ _rfidtype=value;}
			get{return _rfidtype;}
		}
		/// <summary>
		/// @TypeCode:任务类型
		/// </summary>
		public string taskType
		{
			set{ _tasktype=value;}
			get{return _tasktype;}
		}
		/// <summary>
		/// AGV方向
		/// </summary>
		public string agvDerection
		{
			set{ _agvderection=value;}
			get{return _agvderection;}
		}
		/// <summary>
		/// AGV速度
		/// </summary>
		public int? agvSpeed
		{
			set{ _agvspeed=value;}
			get{return _agvspeed;}
		}
		/// <summary>
		/// 通讯数据
		/// </summary>
		public string taskData
		{
			set{ _taskdata=value;}
			get{return _taskdata;}
		}
		/// <summary>
		/// 地标状态
		/// </summary>
		public string rfidStatus
		{
			set{ _rfidstatus=value;}
			get{return _rfidstatus;}
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

