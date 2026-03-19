using System;

namespace Model
{
	/// <summary>
	/// smtMaterialOutstoreData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtMaterialOutstoreData
	{
		public smtMaterialOutstoreData()
		{}
		#region Model
		private int _id;
		private string _materialno;
		private string _materialcount;
		private string _materialbarcode;
		private string _shelfno;
		private string _materialstatus;
		private string _creater;
		private DateTime? _createtime;
		private string _lotno;
		private string _outstoreoperator;
		private DateTime? _outstoredate;
		private string _outshelfno;
		private string _receiptno;
		private string _bin;
		private string _supplybin;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:2
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:3
		/// </summary>
		public string materialCount
		{
			set{ _materialcount=value;}
			get{return _materialcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:条形码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:4
		/// </summary>
		public string materialBarcode
		{
			set{ _materialbarcode=value;}
			get{return _materialbarcode;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:库位编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:5
		/// </summary>
		public string shelfNo
		{
			set{ _shelfno=value;}
			get{return _shelfno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:物料出库状态,@searchString:select value from commDic where name = '物料出库状态',@tipString:,@inDataGrid:True,@inPanel:True,@index:6
		/// </summary>
		public string materialStatus
		{
			set{ _materialstatus=value;}
			get{return _materialstatus;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:7
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:8
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:批次号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string lotNo
		{
			set{ _lotno=value;}
			get{return _lotno;}
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
		/// @controlType:LTextBox,@labelString:出库货架,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string outshelfNo
		{
			set{ _outshelfno=value;}
			get{return _outshelfno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:出库单编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string ReceiptNo
		{
			set{ _receiptno=value;}
			get{return _receiptno;}
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

