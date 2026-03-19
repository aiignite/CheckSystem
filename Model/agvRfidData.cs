using System;

namespace Model
{
	/// <summary>
	/// agvRfidData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class agvRfidData
	{
		public agvRfidData()
		{}
		#region Model
		private int _id;
		private string _rfidno;
		private string _nextrfidno;
		private int? _distance;
		private string _pathdetail;
		private string _status;
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
		/// 地标编号
		/// </summary>
		public string rfidNo
		{
			set{ _rfidno=value;}
			get{return _rfidno;}
		}
		/// <summary>
		/// 相邻地标编号
		/// </summary>
		public string nextRfidNo
		{
			set{ _nextrfidno=value;}
			get{return _nextrfidno;}
		}
		/// <summary>
		/// 地标距离
		/// </summary>
		public int? distance
		{
			set{ _distance=value;}
			get{return _distance;}
		}
		/// <summary>
		/// 路径明细
		/// </summary>
		public string pathDetail
		{
			set{ _pathdetail=value;}
			get{return _pathdetail;}
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

