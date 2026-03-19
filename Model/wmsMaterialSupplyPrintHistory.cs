using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsMaterialSupplyPrintHistory
	{
		public wmsMaterialSupplyPrintHistory()
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
		public string stockInNo
		{
			set{ _stockinno=value;}
			get{return _stockinno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string materialBarcode
		{
			set{ _materialbarcode=value;}
			get{return _materialbarcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string printNo
		{
			set{ _printno=value;}
			get{return _printno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string lotNo
		{
			set{ _lotno=value;}
			get{return _lotno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string boxNo
		{
			set{ _boxno=value;}
			get{return _boxno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string barcode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string supplyNo
		{
			set{ _supplyno=value;}
			get{return _supplyno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string supplyLedGroup
		{
			set{ _supplyledgroup=value;}
			get{return _supplyledgroup;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string companyGroup
		{
			set{ _companygroup=value;}
			get{return _companygroup;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string stockInOperator
		{
			set{ _stockinoperator=value;}
			get{return _stockinoperator;}
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
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 
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

