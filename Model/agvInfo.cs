using System;

namespace Model
{
	/// <summary>
	/// agvInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class agvInfo
	{
		public agvInfo()
		{}
		#region Model
		private int _id;
		private string _agvno;
		private string _agvname;
		private string _moduleno;
		private string _moduleipport;
		private int? _agvsoc;
		private string _agvposition;
		private string _agvstatus;
		private DateTime? _agvworkindate;
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
		/// AGV编号
		/// </summary>
		public string agvNo
		{
			set{ _agvno=value;}
			get{return _agvno;}
		}
		/// <summary>
		/// AGV名称
		/// </summary>
		public string agvName
		{
			set{ _agvname=value;}
			get{return _agvname;}
		}
		/// <summary>
		/// 通讯模块编号
		/// </summary>
		public string moduleNo
		{
			set{ _moduleno=value;}
			get{return _moduleno;}
		}
		/// <summary>
		/// 通讯端口号
		/// </summary>
		public string moduleIpport
		{
			set{ _moduleipport=value;}
			get{return _moduleipport;}
		}
		/// <summary>
		/// 剩余电量%
		/// </summary>
		public int? agvSoc
		{
			set{ _agvsoc=value;}
			get{return _agvsoc;}
		}
		/// <summary>
		/// 所在地标
		/// </summary>
		public string agvPosition
		{
			set{ _agvposition=value;}
			get{return _agvposition;}
		}
		/// <summary>
		/// AGV状态
		/// </summary>
		public string agvStatus
		{
			set{ _agvstatus=value;}
			get{return _agvstatus;}
		}
		/// <summary>
		/// @DateTimePicker:投运时间
		/// </summary>
		public DateTime? AGVWorkinDate
		{
			set{ _agvworkindate=value;}
			get{return _agvworkindate;}
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

