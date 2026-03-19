using System;

namespace Model
{
	/// <summary>
	/// deviceMaintainData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class deviceMaintainData
	{
		public deviceMaintainData()
		{}
		#region Model
		private int _id;
		private string _deviceno;
		private string _maintainno;
		private string _plandate;
		private string _actualdate;
		private string _maintainnote;
		private string _maintainstatus;
		private string _maintainer;
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
		/// @ComboBox:保养编号,maintainNo,deviceMaintainInfo,maintainNo,
		/// </summary>
		public string maintainNo
		{
			set{ _maintainno=value;}
			get{return _maintainno;}
		}
		/// <summary>
		/// 计划时间
		/// </summary>
		public string planDate
		{
			set{ _plandate=value;}
			get{return _plandate;}
		}
		/// <summary>
		/// 实际时间
		/// </summary>
		public string actualDate
		{
			set{ _actualdate=value;}
			get{return _actualdate;}
		}
		/// <summary>
		/// 保养说明
		/// </summary>
		public string maintainNote
		{
			set{ _maintainnote=value;}
			get{return _maintainnote;}
		}
		/// <summary>
		/// @ComboBox:保养状态,typeValue,commTypeCode,typeName,保养状态
		/// </summary>
		public string maintainStatus
		{
			set{ _maintainstatus=value;}
			get{return _maintainstatus;}
		}
		/// <summary>
		/// 保养人员
		/// </summary>
		public string maintainer
		{
			set{ _maintainer=value;}
			get{return _maintainer;}
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

