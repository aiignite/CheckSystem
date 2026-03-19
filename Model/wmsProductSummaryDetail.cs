using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsProductSummaryDetail
	{
		public wmsProductSummaryDetail()
		{}
		#region Model
		private int _id;
		private string _serialno;
		private string _productno;
		private string _shipno;
		private int? _stockupcount;
		private int? _stockoutcount;
		private string _plateno;
		private DateTime? _refreshtime;
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
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:订单号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string shipNo
		{
			set{ _shipno=value;}
			get{return _shipno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:备货数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? stockupCount
		{
			set{ _stockupcount=value;}
			get{return _stockupcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:装车数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? stockoutCount
		{
			set{ _stockoutcount=value;}
			get{return _stockoutcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:车牌号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string plateNo
		{
			set{ _plateno=value;}
			get{return _plateno;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:刷新时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? refreshTime
		{
			set{ _refreshtime=value;}
			get{return _refreshtime;}
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

