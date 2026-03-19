using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsProductInfo
	{
		public wmsProductInfo()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _productname;
		private string _productmodel;
		private string _productno2;
		private int? _productminpackagenum;
		private string _projectno;
		private string _productstatus;
		private string _isvalid;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:False,@inPanel:True,@index:
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
		/// @controlType:LTextBox,@labelString:产品名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品型号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productModel
		{
			set{ _productmodel=value;}
			get{return _productmodel;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品客户编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo2
		{
			set{ _productno2=value;}
			get{return _productno2;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:最小包装数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? productMinPackageNum
		{
			set{ _productminpackagenum=value;}
			get{return _productminpackagenum;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:项目编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string projectNo
		{
			set{ _projectno=value;}
			get{return _projectno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productStatus
		{
			set{ _productstatus=value;}
			get{return _productstatus;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:是否有效,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string isValid
		{
			set{ _isvalid=value;}
			get{return _isvalid;}
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

