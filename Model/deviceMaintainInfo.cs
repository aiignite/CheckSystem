using System;

namespace Model
{
	/// <summary>
	/// deviceMaintainInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class deviceMaintainInfo
	{
		public deviceMaintainInfo()
		{}
		#region Model
		private int _id;
		private string _deviceno;
		private string _maintainno;
		private string _maintainname;
		private string _maintaindetail;
		private string _maintaindocno;
		private string _maintaindocname;
		private string _maintaindoclink;
		private string _designer;
		private string _creater;
		private DateTime _createtime;
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
		/// 保养编号
		/// </summary>
		public string maintainNo
		{
			set{ _maintainno=value;}
			get{return _maintainno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string maintainName
		{
			set{ _maintainname=value;}
			get{return _maintainname;}
		}
		/// <summary>
		/// 保养内容
		/// </summary>
		public string maintainDetail
		{
			set{ _maintaindetail=value;}
			get{return _maintaindetail;}
		}
		/// <summary>
		/// 保养文件编号
		/// </summary>
		public string maintainDocNo
		{
			set{ _maintaindocno=value;}
			get{return _maintaindocno;}
		}
		/// <summary>
		/// 保养文件名称
		/// </summary>
		public string maintainDocName
		{
			set{ _maintaindocname=value;}
			get{return _maintaindocname;}
		}
		/// <summary>
		/// 保养文件链接
		/// </summary>
		public string maintainDocLink
		{
			set{ _maintaindoclink=value;}
			get{return _maintaindoclink;}
		}
		/// <summary>
		/// 编制人
		/// </summary>
		public string designer
		{
			set{ _designer=value;}
			get{return _designer;}
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
		public DateTime createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

