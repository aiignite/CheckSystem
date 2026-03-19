using System;

namespace Model
{
	/// <summary>
	/// manufactureExceptionData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class manufactureExceptionData
	{
		public manufactureExceptionData()
		{}
		#region Model
		private int _id;
		private string _exceptionno;
		private string _exceptionname;
		private string _exceptiontype;
		private string _taskno;
		private string _productno;
		private string _deviceno;
		private string _processno;
		private int? _exceptiontime;
		private string _operatorstaffno;
		private string _auditorstaffno;
		private string _exceptionstatus;
		private string _notes;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:异常编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string exceptionNo
		{
			set{ _exceptionno=value;}
			get{return _exceptionno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:异常名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string exceptionName
		{
			set{ _exceptionname=value;}
			get{return _exceptionname;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:异常类型,@searchString:select value from dbo.commDic (nolock) where name = '异常类型',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string exceptionType
		{
			set{ _exceptiontype=value;}
			get{return _exceptiontype;}
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
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
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
		/// @controlType:LTextBox,@labelString:工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:异常持续时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? exceptionTime
		{
			set{ _exceptiontime=value;}
			get{return _exceptiontime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:操作人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string operatorStaffNo
		{
			set{ _operatorstaffno=value;}
			get{return _operatorstaffno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:审核人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string auditorStaffNo
		{
			set{ _auditorstaffno=value;}
			get{return _auditorstaffno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:异常状态,@searchString:select value from dbo.commDic (nolock) where name = '异常状态',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string exceptionStatus
		{
			set{ _exceptionstatus=value;}
			get{return _exceptionstatus;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建者,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
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

