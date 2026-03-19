using System;

namespace Model
{
	/// <summary>
	/// deviceResumeData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class deviceResumeData
	{
		public deviceResumeData()
		{}
		#region Model
		private int _id;
		private string _deviceno;
		private string _deptname;
		private string _asslineno;
		private string _note;
		private string _staffno;
		private string _ontime;
		private string _offtime;
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
		/// 部门名称
		/// </summary>
		public string deptName
		{
			set{ _deptname=value;}
			get{return _deptname;}
		}
		/// <summary>
		/// 产线编号
		/// </summary>
		public string assLineNo
		{
			set{ _asslineno=value;}
			get{return _asslineno;}
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
		/// 负责人
		/// </summary>
		public string staffNo
		{
			set{ _staffno=value;}
			get{return _staffno;}
		}
		/// <summary>
		/// 开始日期
		/// </summary>
		public string onTime
		{
			set{ _ontime=value;}
			get{return _ontime;}
		}
		/// <summary>
		/// 结束日期
		/// </summary>
		public string offTime
		{
			set{ _offtime=value;}
			get{return _offtime;}
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

