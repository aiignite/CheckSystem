using System;

namespace Model
{
	/// <summary>
	/// productCheckData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class productCheckData
	{
		public productCheckData()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _processno;
		private string _checkdata;
		private string _checker;
		private string _checkresult;
		private string _pcbano;
		private string _pcbabarcode;
		private string _productuid;
		private string _taskno;
		private string _deviceno;
		private string _note;
		private DateTime? _createtime;
		private string _creater;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
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
		/// @controlType:LTextBox,@labelString:工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:检测数据,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkData
		{
			set{ _checkdata=value;}
			get{return _checkdata;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:检测人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checker
		{
			set{ _checker=value;}
			get{return _checker;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:检测结果,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkResult
		{
			set{ _checkresult=value;}
			get{return _checkresult;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:PCBA编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pcbaNo
		{
			set{ _pcbano=value;}
			get{return _pcbano;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:PCBA条形码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pcbaBarcode
		{
			set{ _pcbabarcode=value;}
			get{return _pcbabarcode;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品唯一号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productUID
		{
			set{ _productuid=value;}
			get{return _productuid;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:工单编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string taskNo
		{
			set{ _taskno=value;}
			get{return _taskno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:设备编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string deviceNo
		{
			set{ _deviceno=value;}
			get{return _deviceno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:11
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:9
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		#endregion Model

	}
}

