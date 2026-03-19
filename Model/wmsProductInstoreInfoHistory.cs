using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsProductInstoreInfoHistory
	{
		public wmsProductInstoreInfoHistory()
		{}
		#region Model
		private int _id;
		private string _productinstoreno;
		private string _productno;
		private string _productname;
		private int? _productcount;
		private int? _instorecount;
		private int? _boxcount;
		private string _operatorno;
		private string _customerno;
		private string _brightnessgroup;
		private string _boxtype;
		private string _specific;
		private string _special;
		private string _type;
		private string _status;
		private string _creater;
		private DateTime? _createtime;
		private string _receiver;
		private DateTime? _receivetime;
		private string _outstocktype;
		private string _batchno;
		private string _receiptno;
		/// <summary>
		/// @controlType:LTextBox,@labelString:编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入库单编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productInStoreNo
		{
			set{ _productinstoreno=value;}
			get{return _productinstoreno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品编号,@searchString:select productNo from productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品名称,@searchString:select productName from productInfo where productNo like '%%',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:应入库数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? productCount
		{
			set{ _productcount=value;}
			get{return _productcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:已入库数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? instoreCount
		{
			set{ _instorecount=value;}
			get{return _instorecount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:料箱数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? boxCount
		{
			set{ _boxcount=value;}
			get{return _boxcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入库人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string operatorNo
		{
			set{ _operatorno=value;}
			get{return _operatorno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:客户编号,@searchString:select productNo2 from productInfo where productNo like '%%',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string customerNo
		{
			set{ _customerno=value;}
			get{return _customerno;}
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
		/// @controlType:LComboBox,@labelString:类别,@searchString:select value from commDic where name='成品入库类别',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string type
		{
			set{ _type=value;}
			get{return _type;}
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
		public DateTime? createtime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:接收人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public string receiver
		{
			set{ _receiver=value;}
			get{return _receiver;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:接收时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public DateTime? receiveTime
		{
			set{ _receivetime=value;}
			get{return _receivetime;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:出货类型,@searchString:select value from commDic where name='成品出货类型',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string outStockType
		{
			set{ _outstocktype=value;}
			get{return _outstocktype;}
		}
		/// <summary>
		/// 批次号
		/// </summary>
		public string batchNo
		{
			set{ _batchno=value;}
			get{return _batchno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:单号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public string receiptNo
		{
			set{ _receiptno=value;}
			get{return _receiptno;}
		}
		#endregion Model

	}
}

