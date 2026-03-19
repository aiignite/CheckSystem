using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsProductInstoreDetail
	{
		public wmsProductInstoreDetail()
		{}
		#region Model
		private int _id;
		private string _instoredetailno;
		private string _productinstoreno;
		private string _productno;
		private int? _productcount;
		private string _boxno;
		private string _trayno;
		private string _shelfno;
		private string _barcode;
		private string _status;
		private string _operatorno;
		private DateTime? _createtime;
		private DateTime? _instoretime;
		private int? _outstorecount;
		private string _skno;
		private string _brightnessgroup;
		private string _specific;
		private string _productname;
		private string _batchno;
		private string _boxtype;
		private string _special;
		private string _returnman;
		private DateTime? _returntime;
		private DateTime? _deletetime;
		private string _deleteman;
		private string _creater;
		/// <summary>
		/// @controlType:LTextBox,@labelString:编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入库详单编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string instoreDetailNo
		{
			set{ _instoredetailno=value;}
			get{return _instoredetailno;}
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
		/// @controlType:LTextBox,@labelString:条码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string barcode
		{
			set{ _barcode=value;}
			get{return _barcode;}
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
		/// @controlType:LTextBox,@labelString:操作员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string operatorNo
		{
			set{ _operatorno=value;}
			get{return _operatorno;}
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
		/// @controlType:LDateTimePicker,@labelString:入库时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? instoreTime
		{
			set{ _instoretime=value;}
			get{return _instoretime;}
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
		/// @controlType:LTextBox,@labelString:特性,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string specific
		{
			set{ _specific=value;}
			get{return _specific;}
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
		/// @controlType:LTextBox,@labelString:料箱类型,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string boxType
		{
			set{ _boxtype=value;}
			get{return _boxtype;}
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
		/// 更新操作人
		/// </summary>
		public string returnman
		{
			set{ _returnman=value;}
			get{return _returnman;}
		}
		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? returnTime
		{
			set{ _returntime=value;}
			get{return _returntime;}
		}
		/// <summary>
		/// 解绑时间
		/// </summary>
		public DateTime? deleteTime
		{
			set{ _deletetime=value;}
			get{return _deletetime;}
		}
		/// <summary>
		/// 解绑人
		/// </summary>
		public string deleteman
		{
			set{ _deleteman=value;}
			get{return _deleteman;}
		}
		/// <summary>
		/// 创建人
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		#endregion Model

	}
}

