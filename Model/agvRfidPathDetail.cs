using System;

namespace Model
{
	/// <summary>
	/// agvRfidPathDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class agvRfidPathDetail
	{
		public agvRfidPathDetail()
		{}
		#region Model
		private int _id;
		private string _startrfidno;
		private string _endrfidno;
		private string _rfiddetail;
		private int? _distance;
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
		/// 开始地标编号
		/// </summary>
		public string startRfidNo
		{
			set{ _startrfidno=value;}
			get{return _startrfidno;}
		}
		/// <summary>
		/// 结束地标编号
		/// </summary>
		public string endRfidNo
		{
			set{ _endrfidno=value;}
			get{return _endrfidno;}
		}
		/// <summary>
		/// 地标明细
		/// </summary>
		public string rfidDetail
		{
			set{ _rfiddetail=value;}
			get{return _rfiddetail;}
		}
		/// <summary>
		/// 距离
		/// </summary>
		public int? distance
		{
			set{ _distance=value;}
			get{return _distance;}
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

