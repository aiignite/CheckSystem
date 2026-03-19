using System;

namespace Model
{
	/// <summary>
	/// agvStation:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class agvStation
	{
		public agvStation()
		{}
		#region Model
		private int _id;
		private string _stationno;
		private string _stationname;
		private string _rfidno;
		private string _stationstatus;
		private string _note;
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
		/// 车站编号
		/// </summary>
		public string stationNo
		{
			set{ _stationno=value;}
			get{return _stationno;}
		}
		/// <summary>
		/// 车站名称
		/// </summary>
		public string stationName
		{
			set{ _stationname=value;}
			get{return _stationname;}
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
		/// 站点状态
		/// </summary>
		public string stationStatus
		{
			set{ _stationstatus=value;}
			get{return _stationstatus;}
		}
		/// <summary>
		/// 说明
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
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

