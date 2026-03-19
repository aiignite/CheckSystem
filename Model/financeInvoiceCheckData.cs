using System;

namespace Model
{
	/// <summary>
	/// financeInvoiceCheckData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class financeInvoiceCheckData
	{
		public financeInvoiceCheckData()
		{}
		#region Model
		private int _id;
		private string _invoicecode;
		private string _invoiceno;
		private string _invoicingdate;
		private string _invoicescanvalue;
		private string _invoicescan;
		private string _creater;
		private DateTime? _createtime;
		private string _invoicevalue;
		private string _checkcode;
		private string _department;
		private string _person;
		private string _invoicetype;
		private string _invoiceunit;
		/// <summary>
		/// @controlType:LTextBox,@labelString:编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:发票代码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string invoiceCode
		{
			set{ _invoicecode=value;}
			get{return _invoicecode;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:发票号码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string invoiceNo
		{
			set{ _invoiceno=value;}
			get{return _invoiceno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:开票日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string invoicingDate
		{
			set{ _invoicingdate=value;}
			get{return _invoicingdate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:发票金额,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string invoiceScanValue
		{
			set{ _invoicescanvalue=value;}
			get{return _invoicescanvalue;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:二维码内容,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@index:
		/// </summary>
		public string invoiceScan
		{
			set{ _invoicescan=value;}
			get{return _invoicescan;}
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
		/// @controlType:LTextBox,@labelString:税后金额,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string invoiceValue
		{
			set{ _invoicevalue=value;}
			get{return _invoicevalue;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:校验码后6位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkcode
		{
			set{ _checkcode=value;}
			get{return _checkcode;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:所属部门,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string department
		{
			set{ _department=value;}
			get{return _department;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:报销人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string person
		{
			set{ _person=value;}
			get{return _person;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:发票内容,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string invoicetype
		{
			set{ _invoicetype=value;}
			get{return _invoicetype;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:销售方,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string invoiceunit
		{
			set{ _invoiceunit=value;}
			get{return _invoiceunit;}
		}
		#endregion Model

	}
}

