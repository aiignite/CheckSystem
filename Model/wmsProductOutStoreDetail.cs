using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsProductOutStoreDetail
	{
		public wmsProductOutStoreDetail()
		{}
		#region Model
		private int _id;
		private string _outstoredetailno;
		private string _productoutstoreno;
		private string _productno;
		private int? _productcount;
		private int? _outstorecount;
		private string _boxno;
		private string _trayno;
		private string _shelfno;
		private string _status;
		private string _skno;
		private string _brightnessgroup;
		private string _boxtype;
		private string _specific;
		private string _special;
		private string _productname;
		private string _creater;
		private DateTime? _createtime;
		private string _offshelfoperator;
		private DateTime? _offshelftime;
		private string _outstoreoperator;
		private DateTime? _outstoretime;
		private string _batchno;
		private string _acceptcustomer;
		private string _priority;
		/// <summary>
		/// @controlType:LTextBox,@labelString:编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:出库详单编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string outStoreDetailNo
		{
			set{ _outstoredetailno=value;}
			get{return _outstoredetailno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:出库单编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productOutStoreNo
		{
			set{ _productoutstoreno=value;}
			get{return _productoutstoreno;}
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
		public int? outstoreCount
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
		/// @controlType:LTextBox,@labelString:状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
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
		/// @controlType:LTextBox,@labelString:料箱类型,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
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
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
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
		/// @controlType:LDateTimePicker,@labelString:下架时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? offShelfTime
		{
			set{ _offshelftime=value;}
			get{return _offshelftime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:出库人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string outStoreOperator
		{
			set{ _outstoreoperator=value;}
			get{return _outstoreoperator;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:出库时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? outStoreTime
		{
			set{ _outstoretime=value;}
			get{return _outstoretime;}
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
		/// @controlType:LTextBox,@labelString:接收客户,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string acceptCustomer
		{
			set{ _acceptcustomer=value;}
			get{return _acceptcustomer;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:优先级,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string priority
		{
			set{ _priority=value;}
			get{return _priority;}
		}
		#endregion Model

	}
}

