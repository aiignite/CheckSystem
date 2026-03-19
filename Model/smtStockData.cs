using System;

namespace Model
{
	/// <summary>
	/// smtStockData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtStockData
	{
		public smtStockData()
		{}
		#region Model
		private int _id;
		private string _materialno;
		private string _count;
		private string _barcode;
		private string _lotno;
		private string _boxno;
		private string _shelfno;
		private DateTime? _instoredate;
		private string _instoreoperator;
		private DateTime? _outstoredate;
		private string _outstoreoperator;
		private string _status;
		private string _creater;
		private DateTime? _createtime;
		private string _bin;
		private string _ledgroup;
		private string _outreceiptno;
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
		public string count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:条形码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:4
		/// </summary>
		public string barcode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:批号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:5
		/// </summary>
		public string lotNo
		{
			set{ _lotno=value;}
			get{return _lotno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:料箱编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:,@index:6
		/// </summary>
		public string boxNo
		{
			set{ _boxno=value;}
			get{return _boxno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:货架编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:7
		/// </summary>
		public string shelfNo
		{
			set{ _shelfno=value;}
			get{return _shelfno;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:入库日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:8
		/// </summary>
		public DateTime? instoreDate
		{
			set{ _instoredate=value;}
			get{return _instoredate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入库人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:9
		/// </summary>
		public string instoreOperator
		{
			set{ _instoreoperator=value;}
			get{return _instoreoperator;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:出库日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:10
		/// </summary>
		public DateTime? outstoreDate
		{
			set{ _outstoredate=value;}
			get{return _outstoredate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:出库人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:11
		/// </summary>
		public string outstoreOperator
		{
			set{ _outstoreoperator=value;}
			get{return _outstoreoperator;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:12
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:13
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:14
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
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
		/// @controlType:LTextBox,@labelString:LED档位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string ledGroup
		{
			set{ _ledgroup=value;}
			get{return _ledgroup;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:出库单编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string outReceiptNo
		{
			set{ _outreceiptno=value;}
			get{return _outreceiptno;}
		}
		#endregion Model

	}
}

