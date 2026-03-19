using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsProductSummaryData
	{
		public wmsProductSummaryData()
		{}
		#region Model
		private int _id;
		private string _serialno;
		private string _productno;
		private string _skno;
		private string _productname;
		private int? _needcount;
		private string _needtime;
		private string _address;
		private string _remark;
		private int? _stockcount;
		private string _status;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:序列号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string serialNo
		{
			set{ _serialno=value;}
			get{return _serialno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品编号,@searchString:select distinct productNo from dbo.productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:客户编号,@searchString:select distinct productNo2 from dbo.productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string skNo
		{
			set{ _skno=value;}
			get{return _skno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品名称,@searchString:select distinct productName from dbo.productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:需求总数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? needCount
		{
			set{ _needcount=value;}
			get{return _needcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:需求时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string needTime
		{
			set{ _needtime=value;}
			get{return _needtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:需求地点,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:备货说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:实时库存数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? stockCount
		{
			set{ _stockcount=value;}
			get{return _stockcount;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:状态,@searchString:select distinct value from dbo.commDic where name = '成品看板状态',@tipString:,@inDataGrid:True,@inPanel:True,@index:
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
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

