using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsNormalMaterialStockData
	{
		public wmsNormalMaterialStockData()
		{}
		#region Model
		private int _id;
		private string _instoreno;
		private string _productno;
		private int? _productcount;
		private string _customerno;
		private string _lotno;
		private string _brightnessgroup;
		private string _supplyledgroup;
		private string _barcode;
		private string _trayno;
		private string _shelfno;
		private string _status;
		private string _productoutstoreno;
		private string _instoreoperator;
		private DateTime? _instoredate;
		private string _onshelfoperator;
		private DateTime? _onshelfdate;
		private string _outtrayno;
		private string _outshelfno;
		private string _outstoreoperator;
		private DateTime? _outstoredate;
		private string _backtype;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:订单号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string instoreNo
		{
			set{ _instoreno=value;}
			get{return _instoreno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? productCount
		{
			set{ _productcount=value;}
			get{return _productcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:供应商编码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string customerNo
		{
			set{ _customerno=value;}
			get{return _customerno;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:生产日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string lotNo
		{
			set{ _lotno=value;}
			get{return _lotno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:档位编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string brightnessGroup
		{
			set{ _brightnessgroup=value;}
			get{return _brightnessgroup;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:供应商档位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplyLEDGroup
		{
			set{ _supplyledgroup=value;}
			get{return _supplyledgroup;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:扫码内容,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string barcode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:托盘号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string trayNo
		{
			set{ _trayno=value;}
			get{return _trayno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:货架号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string shelfNo
		{
			set{ _shelfno=value;}
			get{return _shelfno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:出库单号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productOutstoreNo
		{
			set{ _productoutstoreno=value;}
			get{return _productoutstoreno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入库人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string instoreOperator
		{
			set{ _instoreoperator=value;}
			get{return _instoreoperator;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:入库时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? instoreDate
		{
			set{ _instoredate=value;}
			get{return _instoredate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:上架人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string onShelfOperator
		{
			set{ _onshelfoperator=value;}
			get{return _onshelfoperator;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:上架时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? onShelfDate
		{
			set{ _onshelfdate=value;}
			get{return _onshelfdate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:原托盘,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string outtrayNo
		{
			set{ _outtrayno=value;}
			get{return _outtrayno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:原货架,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string outshelfNo
		{
			set{ _outshelfno=value;}
			get{return _outshelfno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:出库人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string outstoreOperator
		{
			set{ _outstoreoperator=value;}
			get{return _outstoreoperator;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:出库时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? outstoreDate
		{
			set{ _outstoredate=value;}
			get{return _outstoredate;}
		}
		/// <summary>
		/// 返料标识
		/// </summary>
		public string backType
		{
			set{ _backtype=value;}
			get{return _backtype;}
		}
		#endregion Model

	}
}

