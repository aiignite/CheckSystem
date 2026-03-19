using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsMaterialStockOutDetail
	{
		public wmsMaterialStockOutDetail()
		{}
		#region Model
		private int _id;
		private string _stockoutno;
		private string _shelfno;
		private string _boxno;
		private string _materialno;
		private string _lotno;
		private int? _count;
		private int? _stockoutcount;
		private string _barcode;
		private string _stockoutoperator;
		private string _status;
		private DateTime? _createtime;
		private string _supplyno;
		private string _bin;
		/// <summary>
		/// ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 出库单编号
		/// </summary>
		public string stockOutNo
		{
			set{ _stockoutno=value;}
			get{return _stockoutno;}
		}
		/// <summary>
		/// 货架编号
		/// </summary>
		public string shelfNo
		{
			set{ _shelfno=value;}
			get{return _shelfno;}
		}
		/// <summary>
		/// 料箱编号
		/// </summary>
		public string boxNo
		{
			set{ _boxno=value;}
			get{return _boxno;}
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
		/// 批次号
		/// </summary>
		public string lotNo
		{
			set{ _lotno=value;}
			get{return _lotno;}
		}
		/// <summary>
		/// 数量
		/// </summary>
		public int? count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// 已出库数量
		/// </summary>
		public int? stockOutCount
		{
			set{ _stockoutcount=value;}
			get{return _stockoutcount;}
		}
		/// <summary>
		/// 条形码
		/// </summary>
		public string barcode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		/// <summary>
		/// 操作员
		/// </summary>
		public string stockOutOperator
		{
			set{ _stockoutoperator=value;}
			get{return _stockoutoperator;}
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
		/// @DateTimePicker:日期
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 供应商编号
		/// </summary>
		public string supplyNo
		{
			set{ _supplyno=value;}
			get{return _supplyno;}
		}
		/// <summary>
		/// 档位
		/// </summary>
		public string bin
		{
			set{ _bin=value;}
			get{return _bin;}
		}
		#endregion Model

	}
}

