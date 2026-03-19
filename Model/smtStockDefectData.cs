using System;

namespace Model
{
	/// <summary>
	/// smtStockDefectData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtStockDefectData
	{
		public smtStockDefectData()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _barcode;
		private string _creater;
		private DateTime? _createtime;
		private string _status;
		private string _checkresult;
		private string _checkman;
		private DateTime? _checktime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@Writeable:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:不良品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品条码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string barcode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:检测结果,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string checkResult
		{
			set{ _checkresult=value;}
			get{return _checkresult;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:检测人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string checkman
		{
			set{ _checkman=value;}
			get{return _checkman;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:检测时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public DateTime? checkTime
		{
			set{ _checktime=value;}
			get{return _checktime;}
		}
		#endregion Model

	}
}

