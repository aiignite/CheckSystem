using System;

namespace Model
{
	/// <summary>
	/// deviceVersionData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class deviceVersionData
	{
		public deviceVersionData()
		{}
		#region Model
		private int _id;
		private string _deviceno;
		private string _softversion;
		private string _hardversion;
		private string _note;
		private string _ondate;
		private string _offdate;
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
		/// 软件版本
		/// </summary>
		public string softVersion
		{
			set{ _softversion=value;}
			get{return _softversion;}
		}
		/// <summary>
		/// 硬件版本
		/// </summary>
		public string hardVersion
		{
			set{ _hardversion=value;}
			get{return _hardversion;}
		}
		/// <summary>
		/// 版本说明
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
		}
		/// <summary>
		/// 开始时间
		/// </summary>
		public string onDate
		{
			set{ _ondate=value;}
			get{return _ondate;}
		}
		/// <summary>
		/// 结束时间
		/// </summary>
		public string offDate
		{
			set{ _offdate=value;}
			get{return _offdate;}
		}
		/// <summary>
		/// @ComboBox:是否有效,typeValue,commTypeCode,typeName,是否有效
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

