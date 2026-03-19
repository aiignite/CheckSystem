using System;

namespace Model
{
	/// <summary>
	/// deviceDocData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class deviceDocData
	{
		public deviceDocData()
		{}
		#region Model
		private int _id;
		private string _deviceno;
		private string _docno;
		private string _docname;
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
		/// 文档编号
		/// </summary>
		public string docNo
		{
			set{ _docno=value;}
			get{return _docno;}
		}
		/// <summary>
		/// 设备名称
		/// </summary>
		public string docName
		{
			set{ _docname=value;}
			get{return _docname;}
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

