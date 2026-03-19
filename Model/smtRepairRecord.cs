using System;

namespace Model
{
	/// <summary>
	/// smtRepairRecord:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtRepairRecord
	{
		public smtRepairRecord()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _uid;
		private string _status;
		private string _memo;
		private string _inspector;
		private string _repairperson;
		private string _repaircontent;
		private DateTime? _time;
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
		public string uid
		{
			set{ _uid=value;}
			get{return _uid;}
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
		public string memo
		{
			set{ _memo=value;}
			get{return _memo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string inspector
		{
			set{ _inspector=value;}
			get{return _inspector;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string repairPerson
		{
			set{ _repairperson=value;}
			get{return _repairperson;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string repairContent
		{
			set{ _repaircontent=value;}
			get{return _repaircontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? time
		{
			set{ _time=value;}
			get{return _time;}
		}
		#endregion Model

	}
}

