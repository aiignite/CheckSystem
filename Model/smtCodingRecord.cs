using System;

namespace Model
{
	/// <summary>
	/// smtCodingRecord:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtCodingRecord
	{
		public smtCodingRecord()
		{}
		#region Model
		private int _id;
		private string _tlno;
		private string _tlposition;
		private string _tlproductno;
		private string _uid;
		private string _tracenumber;
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
		public string TLNo
		{
			set{ _tlno=value;}
			get{return _tlno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TLPosition
		{
			set{ _tlposition=value;}
			get{return _tlposition;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TLProductNo
		{
			set{ _tlproductno=value;}
			get{return _tlproductno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Uid
		{
			set{ _uid=value;}
			get{return _uid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string traceNumber
		{
			set{ _tracenumber=value;}
			get{return _tracenumber;}
		}
		#endregion Model

	}
}

