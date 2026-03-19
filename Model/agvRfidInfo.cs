using System;

namespace Model
{
	/// <summary>
	/// agvRfidInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class agvRfidInfo
	{
		public agvRfidInfo()
		{}
		#region Model
		private int _id;
		private string _rfidno;
		private string _rfidtype;
		private string _stationno;
		private string _positionnote;
		private string _coordsfactory;
		private string _coordsimage;
		private string _status;
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
		/// 地标编号
		/// </summary>
		public string rfidNo
		{
			set{ _rfidno=value;}
			get{return _rfidno;}
		}
		/// <summary>
		/// @ComboBox:地标类型,typeValue,commTypeCode,typeName,地标类型
		/// </summary>
		public string rfidType
		{
			set{ _rfidtype=value;}
			get{return _rfidtype;}
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
		/// 位置信息
		/// </summary>
		public string positionNote
		{
			set{ _positionnote=value;}
			get{return _positionnote;}
		}
		/// <summary>
		/// 地理坐标
		/// </summary>
		public string coordsFactory
		{
			set{ _coordsfactory=value;}
			get{return _coordsfactory;}
		}
		/// <summary>
		/// 图像坐标
		/// </summary>
		public string coordsImage
		{
			set{ _coordsimage=value;}
			get{return _coordsimage;}
		}
		/// <summary>
		/// 状态
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
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

