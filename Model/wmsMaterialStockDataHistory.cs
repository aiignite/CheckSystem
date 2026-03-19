using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsMaterialStockDataHistory
	{
		public wmsMaterialStockDataHistory()
		{}
		#region Model
		private int _id;
		private string _materialno;
		private string _supplierno;
		private string _boxno;
		private string _shelfno;
		private string _lotno;
		private int? _count;
		private string _note;
		private string _barcode;
		private string _stockinno;
		private DateTime? _stockindate;
		private string _stockinoperator;
		private string _stockoutno;
		private DateTime? _stockoutdate;
		private string _stockoutoperator;
		private string _status;
		private DateTime? _createtime;
		private string _brightnessgroup;
		private string _supplyledgroup;
		/// <summary>
		/// ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
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
		/// 供应商编号
		/// </summary>
		public string supplierNo
		{
			set{ _supplierno=value;}
			get{return _supplierno;}
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
		/// 货架编号
		/// </summary>
		public string shelfNo
		{
			set{ _shelfno=value;}
			get{return _shelfno;}
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
		/// 说明
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
		}
		/// <summary>
		/// 扫描条码
		/// </summary>
		public string barcode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		/// <summary>
		/// 入库单编号
		/// </summary>
		public string stockInNo
		{
			set{ _stockinno=value;}
			get{return _stockinno;}
		}
		/// <summary>
		/// @DateTimePicker:入库日期
		/// </summary>
		public DateTime? stockInDate
		{
			set{ _stockindate=value;}
			get{return _stockindate;}
		}
		/// <summary>
		/// 入库人员
		/// </summary>
		public string stockInOperator
		{
			set{ _stockinoperator=value;}
			get{return _stockinoperator;}
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
		/// @DateTimePicker:出库日期
		/// </summary>
		public DateTime? stockOutDate
		{
			set{ _stockoutdate=value;}
			get{return _stockoutdate;}
		}
		/// <summary>
		/// 出库人员
		/// </summary>
		public string stockOutOperator
		{
			set{ _stockoutoperator=value;}
			get{return _stockoutoperator;}
		}
		/// <summary>
		/// @ComboBox:原材料库存状态,typeName,commTypeCode,typeName,原材料库存状态
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
		/// 档位
		/// </summary>
		public string brightnessGroup
		{
			set{ _brightnessgroup=value;}
			get{return _brightnessgroup;}
		}
		/// <summary>
		/// 供应商档位
		/// </summary>
		public string supplyLEDGroup
		{
			set{ _supplyledgroup=value;}
			get{return _supplyledgroup;}
		}
		#endregion Model

	}
}

