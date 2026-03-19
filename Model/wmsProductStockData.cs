using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsProductStockData
	{
		public wmsProductStockData()
		{}
		#region Model
		private int _id;
		private string _storedatano;
		private string _productno;
		private int? _productcount;
		private int? _outstorecount;
		private string _boxno;
		private string _trayno;
		private string _shelfno;
		private string _productinstoreno;
		private DateTime? _instoredate;
		private string _productoutstoreno;
		private DateTime? _outstoretime;
		private string _status;
		private DateTime? _creattime;
		private string _skno;
		private string _brightnessgroup;
		private string _boxtype;
		private string _specific;
		private string _special;
		private string _productname;
		private string _batchno;
		private string _stockinoperator;
		private string _stockoutoperator;
		private string _onshelfoperator;
		private string _offshelfoperator;
		private string _outtrayno;
		private int? _backcount;
		private string _onshelftime;
		private string _offshelftime;
		private string _offshelfno;
		/// <summary>
		/// @controlType:LTextBox,@labelString:编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数据编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string storeDataNo
		{
			set{ _storedatano=value;}
			get{return _storedatano;}
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
		/// @controlType:LTextBox,@labelString:产品数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? productCount
		{
			set{ _productcount=value;}
			get{return _productcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:出库数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? outStoreCount
		{
			set{ _outstorecount=value;}
			get{return _outstorecount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:料箱编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string boxNo
		{
			set{ _boxno=value;}
			get{return _boxno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:托盘编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string trayNo
		{
			set{ _trayno=value;}
			get{return _trayno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:货架编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string shelfNo
		{
			set{ _shelfno=value;}
			get{return _shelfno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入库单编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productInstoreNo
		{
			set{ _productinstoreno=value;}
			get{return _productinstoreno;}
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
		/// @controlType:LTextBox,@labelString:出库单编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productOutstoreNo
		{
			set{ _productoutstoreno=value;}
			get{return _productoutstoreno;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:出库时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? outstoreTime
		{
			set{ _outstoretime=value;}
			get{return _outstoretime;}
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
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? creatTime
		{
			set{ _creattime=value;}
			get{return _creattime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:客户编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string skNo
		{
			set{ _skno=value;}
			get{return _skno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:档位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string brightnessGroup
		{
			set{ _brightnessgroup=value;}
			get{return _brightnessgroup;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:包装方式,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string boxType
		{
			set{ _boxtype=value;}
			get{return _boxtype;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:特性,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string specific
		{
			set{ _specific=value;}
			get{return _specific;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:专用,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string special
		{
			set{ _special=value;}
			get{return _special;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:批次号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string batchNo
		{
			set{ _batchno=value;}
			get{return _batchno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入库人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string stockInOperator
		{
			set{ _stockinoperator=value;}
			get{return _stockinoperator;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:出库人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string stockOutOperator
		{
			set{ _stockoutoperator=value;}
			get{return _stockoutoperator;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:上架人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string onShelfOperator
		{
			set{ _onshelfoperator=value;}
			get{return _onshelfoperator;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:下架人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string offShelfOperator
		{
			set{ _offshelfoperator=value;}
			get{return _offshelfoperator;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:出库托盘号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string outtrayNo
		{
			set{ _outtrayno=value;}
			get{return _outtrayno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:反入库数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? backCount
		{
			set{ _backcount=value;}
			get{return _backcount;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:上架时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string onShelfTime
		{
			set{ _onshelftime=value;}
			get{return _onshelftime;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:下架时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string offShelfTime
		{
			set{ _offshelftime=value;}
			get{return _offshelftime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:下架货架号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string offShelfNo
		{
			set{ _offshelfno=value;}
			get{return _offshelfno;}
		}
		#endregion Model

	}
}

