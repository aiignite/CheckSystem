using System;

namespace Model
{
	/// <summary>
	/// deviceSampleData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class deviceSampleData
	{
		public deviceSampleData()
		{}
		#region Model
		private int _id;
		private string _deviceno;
		private string _sampleno;
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
		/// @ComboBox:设备编号,deviceNo,deviceInfo,deviceNo,
		/// </summary>
		public string deviceNo
		{
			set{ _deviceno=value;}
			get{return _deviceno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sampleNo
		{
			set{ _sampleno=value;}
			get{return _sampleno;}
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

