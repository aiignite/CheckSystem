using System;

namespace Model
{
	/// <summary>
	/// smtTaskData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtTaskData
	{
		public smtTaskData()
		{}
		#region Model
		private int _id;
		private string _taskno;
		private string _machinetypeno;
		private string _productno;
		private string _materialno;
		private string _count;
		private string _bomgroupno;
		private string _smtlineno;
		private string _status;
		private string _rate;
		private DateTime? _begindate;
		private DateTime? _enddate;
		private string _creater;
		private DateTime? _createtime;
		private string _processno;
		private string _supplybin;
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
		/// @controlType:LComboBox,@labelString:机种编号,@searchString:select distinct machineTypeNo from smtProcessInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string machineTypeNo
		{
			set{ _machinetypeno=value;}
			get{return _machinetypeno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品编号,@searchString:select productNo from productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:3
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:4
		/// </summary>
		public string count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:BOM组别,@searchString:select distinct bomGroupNo from smtBomInfo where productNo like '%%',@tipString:,@inDataGrid:True,@inPanel:True,@index:5
		/// </summary>
		public string bomGroupNo
		{
			set{ _bomgroupno=value;}
			get{return _bomgroupno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产线编号,@searchString:select assLineNo from manufactureAssLineInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:6
		/// </summary>
		public string smtLineNo
		{
			set{ _smtlineno=value;}
			get{return _smtlineno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:7
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:合格率,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:8
		/// </summary>
		public string rate
		{
			set{ _rate=value;}
			get{return _rate;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:开始时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:9
		/// </summary>
		public DateTime? beginDate
		{
			set{ _begindate=value;}
			get{return _begindate;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:结束时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:10
		/// </summary>
		public DateTime? endDate
		{
			set{ _enddate=value;}
			get{return _enddate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:11
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:12
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:工艺编号,@searchString:select processNo from smtProcessInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:13
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:供应商档位,@searchString:select processNo from smtProcessInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplyBin
		{
			set{ _supplybin=value;}
			get{return _supplybin;}
		}
		#endregion Model

	}
}

