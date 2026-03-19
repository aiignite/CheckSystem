using System;

namespace Model
{
	/// <summary>
	/// agvData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class agvData
	{
		public agvData()
		{}
		#region Model
		private int _id;
		private string _agvno;
		private string _agvstatus;
		private int? _agvsoc;
		private string _agvposition;
		private string _agvalarm= "getdate";
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
		/// @ComboBox:AGV编号,agvNo,agvInfo,agvNo,
		/// </summary>
		public string agvNo
		{
			set{ _agvno=value;}
			get{return _agvno;}
		}
		/// <summary>
		/// @ComboBox:AGV状态,typeValue,commTypeCode,typeName,AGV状态
		/// </summary>
		public string agvStatus
		{
			set{ _agvstatus=value;}
			get{return _agvstatus;}
		}
		/// <summary>
		/// 剩余电量
		/// </summary>
		public int? agvSoc
		{
			set{ _agvsoc=value;}
			get{return _agvsoc;}
		}
		/// <summary>
		/// 位置
		/// </summary>
		public string agvPosition
		{
			set{ _agvposition=value;}
			get{return _agvposition;}
		}
		/// <summary>
		/// 报警信息
		/// </summary>
		public string agvAlarm
		{
			set{ _agvalarm=value;}
			get{return _agvalarm;}
		}
		/// <summary>
		/// @DateTimePicker:创建时间
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

