using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsMaterialStockInPlan
	{
		public wmsMaterialStockInPlan()
		{}
		#region Model
		private int _id;
		private string _receiptno;
		private string _materialno;
		private string _materialname;
		private decimal? _neednum;
		private string _supplyno;
		private string _supplyname;
		private decimal? _scannum;
		private decimal? _restnum;
		private string _status;
		/// <summary>
		/// ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 单号
		/// </summary>
		public string receiptNo
		{
			set{ _receiptno=value;}
			get{return _receiptno;}
		}
		/// <summary>
		/// 物料编号
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// 物料名称
		/// </summary>
		public string materialName
		{
			set{ _materialname=value;}
			get{return _materialname;}
		}
		/// <summary>
		/// 需求数量
		/// </summary>
		public decimal? needNum
		{
			set{ _neednum=value;}
			get{return _neednum;}
		}
		/// <summary>
		/// 供应商
		/// </summary>
		public string supplyNo
		{
			set{ _supplyno=value;}
			get{return _supplyno;}
		}
		/// <summary>
		/// 供应商名称
		/// </summary>
		public string supplyName
		{
			set{ _supplyname=value;}
			get{return _supplyname;}
		}
		/// <summary>
		/// 扫描数量
		/// </summary>
		public decimal? scanNum
		{
			set{ _scannum=value;}
			get{return _scannum;}
		}
		/// <summary>
		/// 剩余数量
		/// </summary>
		public decimal? restNum
		{
			set{ _restnum=value;}
			get{return _restnum;}
		}
		/// <summary>
		/// 状态
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		#endregion Model

	}
}

