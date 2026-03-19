using System;

namespace Model
{
	/// <summary>
	/// productQualityInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class productQualityInfo
	{
		public productQualityInfo()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _processno;
		private string _statdate;
		private int? _statquantum;
		private string _senduser;
		private string _reason;
		private string _remark;
		private string _status;
		private int? _smsstatus;
		private string _dealer;
		private string _creater;
		private DateTime? _createtime;
		private int? _basicnum;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string statDate
		{
			set{ _statdate=value;}
			get{return _statdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? statQuantum
		{
			set{ _statquantum=value;}
			get{return _statquantum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sendUser
		{
			set{ _senduser=value;}
			get{return _senduser;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string reason
		{
			set{ _reason=value;}
			get{return _reason;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? smsStatus
		{
			set{ _smsstatus=value;}
			get{return _smsstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dealer
		{
			set{ _dealer=value;}
			get{return _dealer;}
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
		/// <summary>
		/// 
		/// </summary>
		public int? basicNum
		{
			set{ _basicnum=value;}
			get{return _basicnum;}
		}
		#endregion Model

	}
}

