using System;

namespace Model
{
	/// <summary>
	/// smtManufactureData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtManufactureData
	{
		public smtManufactureData()
		{}
		#region Model
		private int _id;
		private string _taskno;
		private string _productuid;
		private string _productbarcode;
		private string _productno;
		private string _processno;
		private string _checkdata;
		private string _checkstatus;
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
		/// @controlType:LTextBox,@labelString:工单编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:2
		/// </summary>
		public string taskNo
		{
			set{ _taskno=value;}
			get{return _taskno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品唯一号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:3
		/// </summary>
		public string productUID
		{
			set{ _productuid=value;}
			get{return _productuid;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品条码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:4
		/// </summary>
		public string productBarcode
		{
			set{ _productbarcode=value;}
			get{return _productbarcode;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:4
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:5
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:检测数据,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:6
		/// </summary>
		public string checkData
		{
			set{ _checkdata=value;}
			get{return _checkdata;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:检测状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:7
		/// </summary>
		public string checkStatus
		{
			set{ _checkstatus=value;}
			get{return _checkstatus;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:8
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:9
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

