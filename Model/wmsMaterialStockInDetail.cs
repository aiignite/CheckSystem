using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsMaterialStockInDetail
	{
		public wmsMaterialStockInDetail()
		{}
		#region Model
		private int _id;
		private string _stockinno;
		private string _materialbarcode;
		private string _materialno;
		private string _printno;
		private string _lotno;
		private int? _count;
		private string _boxno;
		private string _barcode;
		private string _supplyno;
		private string _supplyledgroup;
		private string _companygroup;
		private string _note;
		private string _stockinoperator;
		private string _status;
		private DateTime? _createtime;
		private string _orderno;
		private string _supplydate;
		/// <summary>
		/// ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @ComboBox:入库单编号,stockInNo,wmsMaterialStockInInfo,stockInNo,
		/// </summary>
		public string stockInNo
		{
			set{ _stockinno=value;}
			get{return _stockinno;}
		}
		/// <summary>
		/// 物料供应商编号
		/// </summary>
		public string materialBarcode
		{
			set{ _materialbarcode=value;}
			get{return _materialbarcode;}
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
		/// 打印编号
		/// </summary>
		public string printNo
		{
			set{ _printno=value;}
			get{return _printno;}
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
		/// 料箱编号
		/// </summary>
		public string boxNo
		{
			set{ _boxno=value;}
			get{return _boxno;}
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
		/// @ComboBox:供应商编号,supplyNo,wmsMaterialSupplyInfo,supplyNo,
		/// </summary>
		public string supplyNo
		{
			set{ _supplyno=value;}
			get{return _supplyno;}
		}
		/// <summary>
		/// 供应商档位
		/// </summary>
		public string supplyLedGroup
		{
			set{ _supplyledgroup=value;}
			get{return _supplyledgroup;}
		}
		/// <summary>
		/// 公司档位
		/// </summary>
		public string companyGroup
		{
			set{ _companygroup=value;}
			get{return _companygroup;}
		}
		/// <summary>
		/// 说明
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
		}
		/// <summary>
		/// 操作员
		/// </summary>
		public string stockInOperator
		{
			set{ _stockinoperator=value;}
			get{return _stockinoperator;}
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
		/// 订单编号
		/// </summary>
		public string orderNo
		{
			set{ _orderno=value;}
			get{return _orderno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string supplyDate
		{
			set{ _supplydate=value;}
			get{return _supplydate;}
		}
		#endregion Model

	}
}

