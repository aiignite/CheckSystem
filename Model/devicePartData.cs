using System;

namespace Model
{
	/// <summary>
	/// devicePartData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class devicePartData
	{
		public devicePartData()
		{}
		#region Model
		private int _id;
		private string _deviceno;
		private string _partno;
		private int? _lifetime;
		private int? _usedtime;
		private string _partstatus;
		private string _ontime;
		private string _offtime;
		private string _isvalid;
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
		/// @ComboBox:设备编号,deviceNo,deviceInfo,deviceNo,
		/// </summary>
		public string deviceNo
		{
			set{ _deviceno=value;}
			get{return _deviceno;}
		}
		/// <summary>
		/// 部件编号
		/// </summary>
		public string partNo
		{
			set{ _partno=value;}
			get{return _partno;}
		}
		/// <summary>
		/// 部件寿命
		/// </summary>
		public int? lifeTime
		{
			set{ _lifetime=value;}
			get{return _lifetime;}
		}
		/// <summary>
		/// 已用时间
		/// </summary>
		public int? usedTime
		{
			set{ _usedtime=value;}
			get{return _usedtime;}
		}
		/// <summary>
		/// @ComboBox:部件状态,typeValue,commTypeCode,typeName,部件状态
		/// </summary>
		public string partStatus
		{
			set{ _partstatus=value;}
			get{return _partstatus;}
		}
		/// <summary>
		/// 开始时间
		/// </summary>
		public string onTime
		{
			set{ _ontime=value;}
			get{return _ontime;}
		}
		/// <summary>
		/// 停止时间
		/// </summary>
		public string offTime
		{
			set{ _offtime=value;}
			get{return _offtime;}
		}
		/// <summary>
		/// 是否有效
		/// </summary>
		public string isValid
		{
			set{ _isvalid=value;}
			get{return _isvalid;}
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
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

