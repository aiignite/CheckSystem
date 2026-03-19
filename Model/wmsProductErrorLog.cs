using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsProductErrorLog
	{
		public wmsProductErrorLog()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _trayno;
		private string _shelfno;
		private string _status;
		private DateTime? _createtime;
		private string _creater;
		private string _remark;
		/// <summary>
		/// 主键
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 产品编号
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// 托盘号
		/// </summary>
		public string trayNo
		{
			set{ _trayno=value;}
			get{return _trayno;}
		}
		/// <summary>
		/// 货架号
		/// </summary>
		public string shelfNo
		{
			set{ _shelfno=value;}
			get{return _shelfno;}
		}
		/// <summary>
		/// 日志类型状态
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 创建人
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// 备注
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

