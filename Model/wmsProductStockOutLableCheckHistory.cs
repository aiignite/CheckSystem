using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsProductStockOutLableCheckHistory
	{
		public wmsProductStockOutLableCheckHistory()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _customerno;
		private int? _productcount;
		private string _status;
		private string _stockoutoperator;
		private DateTime? _createtime;
		private string _boxnumber;
		private string _boxno;
		private string _shipno;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
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
		/// @controlType:LTextBox,@labelString:客户编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string customerNo
		{
			set{ _customerno=value;}
			get{return _customerno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? productCount
		{
			set{ _productcount=value;}
			get{return _productcount;}
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
		/// @controlType:LTextBox,@labelString:人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string stockOutOperator
		{
			set{ _stockoutoperator=value;}
			get{return _stockoutoperator;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:箱号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string boxNumber
		{
			set{ _boxnumber=value;}
			get{return _boxnumber;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:标签条码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string boxNo
		{
			set{ _boxno=value;}
			get{return _boxno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:订单号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string shipNo
		{
			set{ _shipno=value;}
			get{return _shipno;}
		}
		#endregion Model

	}
}

