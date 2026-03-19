using System;

namespace Model
{
	/// <summary>
	/// deviceRepairData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class deviceRepairData
	{
		public deviceRepairData()
		{}
		#region Model
		private int _id;
		private string _repairno;
		private string _deviceno;
		private string _failuredetail;
		private string _repairdetail;
		private DateTime? _ontime;
		private DateTime? _offtime;
		private string _repairer;
		private string _lessonnote;
		private string _creater;
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
		/// 维修编号
		/// </summary>
		public string repairNo
		{
			set{ _repairno=value;}
			get{return _repairno;}
		}
		/// <summary>
		/// @ComboBox:设备编号,deviceNo,deviceInfo,deviceNo,
		/// </summary>
		public string deviceNo
		{
			set{ _deviceno=value;}
			get{return _deviceno;}
		}
		/// <summary>
		/// 故障内容
		/// </summary>
		public string failureDetail
		{
			set{ _failuredetail=value;}
			get{return _failuredetail;}
		}
		/// <summary>
		/// 维修内容
		/// </summary>
		public string repairDetail
		{
			set{ _repairdetail=value;}
			get{return _repairdetail;}
		}
		/// <summary>
		/// @DateTimePicker:开始时间
		/// </summary>
		public DateTime? onTime
		{
			set{ _ontime=value;}
			get{return _ontime;}
		}
		/// <summary>
		/// @DateTimePicker:结束时间
		/// </summary>
		public DateTime? offTime
		{
			set{ _offtime=value;}
			get{return _offtime;}
		}
		/// <summary>
		/// 维修人员
		/// </summary>
		public string repairer
		{
			set{ _repairer=value;}
			get{return _repairer;}
		}
		/// <summary>
		/// 经验教训
		/// </summary>
		public string lessonNote
		{
			set{ _lessonnote=value;}
			get{return _lessonnote;}
		}
		/// <summary>
		/// 创建者
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @DateTimePicker:记录时间
		/// </summary>
		public DateTime? createTIme
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

