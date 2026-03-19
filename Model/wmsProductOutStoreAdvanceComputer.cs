using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsProductOutStoreAdvanceComputer
	{
		public wmsProductOutStoreAdvanceComputer()
		{}
		#region Model
		private int _id;
		private string _customerno;
		private int? _customerneedcount;
		private string _productno;
		private string _productname;
		private int? _storecount;
		private int? _difference;
		private string _brightnessgroup;
		private string _boxtype;
		private string _specific;
		private string _special;
		private string _status;
		private string _outstoreno;
		private string _batchno;
		private string _creater;
		private DateTime? _createtime;
		private string _acceptcustomer;
		private string _priority;
		private string _memo1;
		/// <summary>
		/// @controlType:LTextBox,@labelString:编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:客户编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string customerNo
		{
			set{ _customerno=value;}
			get{return _customerno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:客户需求数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? customerNeedCount
		{
			set{ _customerneedcount=value;}
			get{return _customerneedcount;}
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
		/// @controlType:LTextBox,@labelString:产品名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:库存数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? storeCount
		{
			set{ _storecount=value;}
			get{return _storecount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:差额,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? difference
		{
			set{ _difference=value;}
			get{return _difference;}
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
		/// @controlType:LTextBox,@labelString:状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:出库单编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string outStoreNo
		{
			set{ _outstoreno=value;}
			get{return _outstoreno;}
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
		/// <summary>
		/// @controlType:LTextBox,@labelString:备注一,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string memo1
		{
			set{ _memo1=value;}
			get{return _memo1;}
		}
		#endregion Model

	}
}

