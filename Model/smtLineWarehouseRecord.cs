using System;

namespace Model
{
	/// <summary>
	/// smtLineWarehouseRecord:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtLineWarehouseRecord
	{
		public smtLineWarehouseRecord()
		{}
		#region Model
		private int _id;
		private string _materialno;
		private string _bin;
		private int? _diffcount;
		private string _shelfno;
		private string _barcode;
		private int? _backnum;
		private string _backoperator;
		private DateTime? _backdatetime;
		private string _creater;
		private DateTime? _createtime;
		private int? _outcount;
		private string _supplybin;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:档位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string bin
		{
			set{ _bin=value;}
			get{return _bin;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:差异数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? diffCount
		{
			set{ _diffcount=value;}
			get{return _diffcount;}
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
		/// @controlType:LTextBox,@labelString:条形码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string barcode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:返料数,@searchString:,@tipString:,@inDataGrid:False,@inPanel:True,@index:
		/// </summary>
		public int? backNum
		{
			set{ _backnum=value;}
			get{return _backnum;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:返料人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string backOperator
		{
			set{ _backoperator=value;}
			get{return _backoperator;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:返料时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? backDateTime
		{
			set{ _backdatetime=value;}
			get{return _backdatetime;}
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
		/// @controlType:LTextBox,@labelString:出库数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? outCount
		{
			set{ _outcount=value;}
			get{return _outcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:供应商档位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplyBin
		{
			set{ _supplybin=value;}
			get{return _supplybin;}
		}
		#endregion Model

	}
}

